using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using SentenceTransformer;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class DocumentoContable : DocumentAuxiliarForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer;
            this.gvDocument.RefreshData();
            if (this.resultOK)
            {
                //this.CleanHeader(true);
                //this.EnableHeader(false);

                //this.dtPeriod.Enabled = true;
                //this.dtFecha.Enabled = true;
                //this.cmbMonedaOrigen.Enabled = true;
                //this.masterDocumentoCont.EnableControl(true);
                //this.txtNumber.Enabled = true;
                //this.txtObservacionHeader.Enabled = true;

                //this.masterDocumentoCont.Focus();

                this.txtNumber.Text = this.compNro;
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer;
            FormProvider.Master.itemSendtoAppr.Enabled = false;

            this.CleanHeader(true);
            this.EnableHeader(false);

            this.dtPeriod.Enabled = true;
            this.dtFecha.Enabled = true;
            this.cmbMonedaOrigen.Enabled = true;
            this.masterDocumentoCont.EnableControl(true);
            this.txtNumber.Enabled = true;
            this.txtObservacionHeader.Enabled = true;

            this.masterDocumentoCont.Focus();
        }

        #endregion

        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_coDocumento _coDocumento = null;
        private DTO_coPlanCuenta _cuentaDoc = null;
        private DTO_glConceptoSaldo _concSaldoDoc = null;
        private int _currency = (int)TipoMoneda_LocExt.Local;
        //Variables para no repetir validaciones sobre un control
        private bool _coDocFocus;
        private bool _txtValorFocus;
        private bool _txtNumeroDocFocus;
        private bool _cmbMonedaFocus;
        private bool _isLoadingHeader;

        private string compNro = string.Empty;

        //Variables de Reportes
        private string reportName;

        #endregion

        #region Propiedades

        /// <summary>
        /// Cambia la informacion de un objeto con los datos por uno de tipo temporal
        /// </summary>
        protected override DTO_Comprobante Comprobante
        {
            set 
            {
                this.data = value;
                base.Comprobante = value; 
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion de glDocumentoControl relacionada a un comprobante
        /// </summary>
        /// <param name="comp">Comprobante</param>
        private void LoadTempMainData(DTO_Comprobante comp)
        {
            comp.ValorDoc = this.txtValorDoc.EditValue.ToString();
            comp.coDocumentoID = this.masterDocumentoCont.Value;
            comp.CuentaID = this.masterCuentaHeader.Value;
            comp.TerceroID = this.masterTerceroHeader.Value;
            comp.DocumentoTercero = this.txtDocumentoCOMHeader.Text;
            comp.ProyectoID = this.masterProyectoHeader.Value;
            comp.CentroCostoID = this.masterCentroCostoHeader.Value;
            comp.LugarGeograficoID=this.masterLugarGeoHeader.Value;
            comp.Observacion = this.txtObservacionHeader.Text;
            comp.LineaPresupuestoID = this.defLineaPresupuesto;
            comp.TipoDoc = DocumentoTipo.DocExterno;
        }

        /// <summary>
        /// Carga la informacion del header  a partir de condiciones del formulario
        /// Trae una cuenta y el respectivo concepto de saldo
        /// </summary>
        private bool LoadHeader()
        {
            int cMonedaOrigen = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            UDT_BasicID udt = new UDT_BasicID();

            //Tarea la cuenta de acuerdo al documento y la moneda
            udt.Value = cMonedaOrigen == (int)TipoMoneda_LocExt.Local ? this._coDocumento.CuentaLOC.Value : this._coDocumento.CuentaEXT.Value;
            DTO_MasterBasic basic = _bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, udt, true);
            if (basic != null)
            {
                this._cuentaDoc = (DTO_coPlanCuenta)basic;
                this.masterCuentaHeader.Value = this._cuentaDoc.ID.Value;

                #region Trae el concelpto de saldo
                udt.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                this._concSaldoDoc = (DTO_glConceptoSaldo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, udt, true);
                #endregion
                #region Asigna los valores segun la cuenta
                this.txtDocumentoCOMHeader.Text = string.Empty;
                this.txtDocumentoCOMHeader.Enabled = true;

                //Asigna el tercero
                if (this._cuentaDoc.TerceroInd.Value.Value)
                {
                    this.masterTerceroHeader.EnableControl(true);
                    this.masterTerceroHeader.Value = string.Empty;
                }
                else
                {
                    this.masterTerceroHeader.EnableControl(false);
                    this.masterTerceroHeader.Value = this.defTercero;
                }

                //Asigna el centro de costo
                if (this._cuentaDoc.CentroCostoInd.Value.Value)
                {
                    this.masterCentroCostoHeader.EnableControl(true);
                    this.masterCentroCostoHeader.Value = string.Empty;
                }
                else
                {
                    this.masterCentroCostoHeader.EnableControl(false);
                    this.masterCentroCostoHeader.Value = this.defCentroCosto;
                }

                //Asigna el proyecto
                if (this._cuentaDoc.ProyectoInd.Value.Value)
                {
                    this.masterProyectoHeader.EnableControl(true);
                    this.masterProyectoHeader.Value = string.Empty;
                }
                else
                {
                    this.masterProyectoHeader.EnableControl(false);
                    this.masterProyectoHeader.Value = this.defProyecto;
                }

                //Asigna el Lugar geografico
                if (this._cuentaDoc.LugarGeograficoInd.Value.Value)
                {
                    this.masterLugarGeoHeader.EnableControl(true);
                    this.masterLugarGeoHeader.Value = string.Empty;
                }
                else
                {
                    this.masterLugarGeoHeader.EnableControl(false);
                    this.masterLugarGeoHeader.Value = this.defLugarGeo;
                }

                #endregion
                return true;
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NocoContCta));
                this.CleanHeader(false);

                this._isLoadingHeader = true;

                this.EnableHeader(false);
                this.dtFecha.Enabled = true;
                this.masterDocumentoCont.EnableControl(true);
                this.txtNumber.Enabled = true;
                this.txtObservacionHeader.Enabled = true;

                this._isLoadingHeader = false;
                return false;
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DocumentoContable;
            InitializeComponent();

            this.frmModule = ModulesPrefix.co;
            base.SetInitParameters();

            //Controles del header
            //List<DTO_glConsultaFiltro> listaRel = new List<DTO_glConsultaFiltro>();
            //List<DTO_glConsultaFiltro> listaFil = new List<DTO_glConsultaFiltro>();
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //Dictionary<string, Type> types = new Dictionary<string, Type>();
            //dict.Add("DocumentoID", "DocumentoID");
            //DTO_glConsultaFiltro docId = new DTO_glConsultaFiltro();
            //docId.CampoFisico = "DocumentoID";
            //docId.OperadorFiltro = OperadoresFiltro.Igual;
            //docId.ValorFiltro = this.DocumentID.ToString();
            //docId.OperadorSentencia = "AND";
            //listaFil.Add(docId);
            //DTO_glConsultaFiltroComplejo filtro = new DTO_glConsultaFiltroComplejo(AppMasters.glDocumento, dict, listaFil, types);
            List<DTO_glConsultaFiltro> filtrosCoDocumento = new List<DTO_glConsultaFiltro>();
            //filtrosCoDocumento.Add(filtro);
            filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "DocumentoID",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = this.documentID.ToString()
            });
            _bc.InitMasterUC(this.masterDocumentoCont, AppMasters.coDocumento, true, true, true, false, filtrosCoDocumento);
            _bc.InitMasterUC(this.masterMonedaHeader, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterCuentaHeader, AppMasters.coPlanCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterTerceroHeader, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterProyectoHeader, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroCostoHeader, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeoHeader, AppMasters.glLugarGeografico, true, true, true, false);
            this.masterCuentaHeader.EnableControl(false);
            this.masterMonedaHeader.EnableControl(false);

            //Llena los combos
            TablesResources.GetTableResources(this.cmbMonedaOrigen, typeof(TipoMoneda_LocExt));
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.EnableHeader(false);
            this.dtPeriod.Enabled = true;
            this.dtFecha.Enabled = true;
            this.cmbMonedaOrigen.Enabled = true;
            this.masterDocumentoCont.EnableControl(true);
            this.txtNumber.Enabled = true;
            this.txtObservacionHeader.Enabled = true;

            base.AfterInitialize();

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.cmbMonedaOrigen.Enabled = false;
                this.lblTasaCambio.Visible = false;
                this.txtTasaCambio.Visible = false; 
                this.AsignarTasaCambio(false);
            }
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                this.txtPrefix.Text = string.Empty;
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                this.dtPeriod.Text = periodo;

                this._coDocumento = null;
                this._cuentaDoc = null;
                this._concSaldoDoc = null;
                this.compNro = "0";
            }

            this.txtNumeroDoc.Text = "0";
            this.txtNumber.Text = "0";
            this.txtTasaCambio.Text = "0";
            this.txtValorDoc.Text = "0";

            this.masterDocumentoCont.Value = string.Empty;
            this.masterCuentaHeader.Value = string.Empty;
            this.masterTerceroHeader.Value = string.Empty;
            this.masterProyectoHeader.Value = string.Empty;
            this.masterCentroCostoHeader.Value = string.Empty;
            this.masterLugarGeoHeader.Value = string.Empty;

            this.txtDocumentoCOMHeader.Text = string.Empty;
            this.txtObservacionHeader.Text = string.Empty;

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterDocumentoCont.EnableControl(enable);
            this.dtPeriod.Enabled = enable;
            this.txtNumber.Enabled = enable;

            this.masterTerceroHeader.EnableControl(enable);
            this.masterProyectoHeader.EnableControl(enable);
            this.masterCentroCostoHeader.EnableControl(enable);
            this.masterLugarGeoHeader.EnableControl(enable);

            this.txtDocumentoCOMHeader.Enabled = enable;
            this.txtObservacionHeader.Enabled = enable;

            this.cmbMonedaOrigen.Enabled = this.multiMoneda && enable ? true : false;
            this.dtFecha.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override Object LoadTempHeader()
        {
            DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
            header.ComprobanteID.Value = this.comprobanteID;
            header.ComprobanteNro.Value = Convert.ToInt16(this.txtNumber.Text);
            header.EmpresaID.Value = this.empresaID;
            header.Fecha.Value = this.dtFecha.DateTime;
            header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            header.MdaOrigen.Value = Convert.ToByte((((ComboBoxItem)(this.cmbMonedaOrigen.SelectedItem)).Value));
            header.MdaTransacc.Value = this.monedaId;
            header.PeriodoID.Value = this.dtPeriod.DateTime;
            header.TasaCambioBase.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            header.TasaCambioOtr.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

            return header;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            this.masterMonedaHeader.Value = this.monedaId;
            //Sio la empresa no permite mmultimoneda
            if (!this.multiMoneda)
            {
                this.txtTasaCambio.EditValue = 0;
            }
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(monOr);
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (tc == 0)
                {
                    this.validHeader = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                    return false;
                }
            }

            if (!fromTop)
                this.validHeader = true;
            else
                this.validHeader = false;

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            #region Valida los datos obligatorios
            //Valida que ya este asignada una tasa de cambio
            if (!this.masterMonedaHeader.ValidID)
            {
                this.cmbMonedaOrigen.Focus();
                return false;
            }
            else if (!this.AsignarTasaCambio(false))
            {
                this.cmbMonedaOrigen.Focus();
                return false;
            }


            //Valida datos en la maestra de documento contable
            if (!this.masterDocumentoCont.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterDocumentoCont.CodeRsx);

                MessageBox.Show(msg);
                this.masterDocumentoCont.Focus();

                return false;
            }

            //Valida datos en la maestra de cuentas
            if (!this.masterCuentaHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCuentaHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterCuentaHeader.Focus();

                return false;
            }

            //Valida datos en la maestra de terceros
            if (!this.masterTerceroHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTerceroHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterTerceroHeader.Focus();

                return false;
            }

            //Valida que el documentoCOM tenga información
            if (string.IsNullOrWhiteSpace(this.txtDocumentoCOMHeader.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDocumentoCOM");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtDocumentoCOMHeader.Focus();

                return false;
            }

            //Valida datos en la maestra de proyectos
            if (!this.masterProyectoHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyectoHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyectoHeader.Focus();

                return false;
            }

            //Valida datos en la maestra de centros de costo
            if (!this.masterCentroCostoHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroCostoHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCostoHeader.Focus();

                return false;
            }

            //Valida datos en la maestra de lugares geograficos
            if (!this.masterLugarGeoHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarGeoHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterLugarGeoHeader.Focus();

                return false;
            }

            //Valida que la observacion tenga información
            if (string.IsNullOrWhiteSpace(this.txtObservacionHeader.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtObservacionHeader.Focus();

                return false;
            }
            #endregion
            #region Valida que el documento del concepto de saldo sea el mismo que el documento sobre el cual se esta trabajando
            if (this._coDocumento.DocumentoID.Value != this.documentID.ToString() && this.txtNumber.Text == "0")
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocID);

                MessageBox.Show(msg);
                this.masterDocumentoCont.Focus();

                return false;
            }
            #endregion
            #region Valida que la informacion que se este guardando sea un documento externo
            if (this.txtNumber.Text == "0")
            {
                DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(this.documentID, this.masterTerceroHeader.Value, this.txtDocumentoCOMHeader.Text);
                if (ctrl != null)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocExtExists);

                    MessageBox.Show(msg);
                    this.masterDocumentoCont.Focus();

                    return false;
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave(int monOr)
        {
            if (this._cuentaDoc.Naturaleza == null || !this._cuentaDoc.Naturaleza.Value.HasValue)
                return false;

            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            DTO_Comprobante comp = (DTO_Comprobante)aux;
            DTO_ComprobanteHeader header = comp.Header;
            if (comp.Footer == null)
                comp.Footer = new List<DTO_ComprobanteFooter>();

            bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, header.PeriodoID.Value.Value, header.ComprobanteID.Value, header.ComprobanteNro.Value.Value);
            if (usefulTemp || header.ComprobanteNro.Value == 0)
            {
                #region Trae los datos del formulario dado el documento contable
                this._coDocumento = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, comp.coDocumentoID, true);
                this.comprobanteID = this._coDocumento.ComprobanteID.Value;
                this.prefijoID = this._coDocumento.PrefijoID.Value;
                this.txtPrefix.Text = this.prefijoID;
                #endregion
                #region Trae la cuenta y el concepto de saldo
                //Trae la cuenta
                this._cuentaDoc = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, comp.CuentaID, true);
                
                //Trae el concepto de saldo
                this._concSaldoDoc = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, this._cuentaDoc.ConceptoSaldoID.Value, true);

                #endregion

                this.masterDocumentoCont.Value = this._coDocumento.ID.Value;
                this.txtNumber.Text = header.ComprobanteNro.Value.Value.ToString();
                this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(header.MdaOrigen.Value.Value.ToString());
                this.masterMonedaHeader.Value = header.MdaTransacc.Value;
                this.masterCuentaHeader.Value = comp.CuentaID;
                this.masterTerceroHeader.Value = comp.TerceroID;
                this.masterProyectoHeader.Value = comp.ProyectoID;
                this.masterCentroCostoHeader.Value = comp.CentroCostoID;
                this.masterLugarGeoHeader.Value = comp.LugarGeograficoID;

                this.txtDocumentoCOMHeader.Text = comp.DocumentoTercero;
                this.txtObservacionHeader.Text = comp.Observacion;

                this.dtPeriod.DateTime = header.PeriodoID.Value.Value;
                this.dtFecha.DateTime = header.Fecha.Value.Value;
                this.txtValorDoc.Text = comp.ValorDoc;
                //Si se presenta un problema asignando la tasa de cambio lo bloquea
                if (this.ValidateHeader())
                {
                    this.EnableHeader(false);

                    this.Comprobante = comp;
                    this.LoadData(true);
                    this.validHeader = true;
                    this.gcDocument.Focus();
                }
                else
                {
                    this.CleanHeader(true);
                }
            }
            else
            {
                this.validHeader = false;
                string rsx = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCompTemp);
                string msg = string.Format(rsx, header.ComprobanteID.Value, header.ComprobanteNro.Value.Value, header.PeriodoID.Value);
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected override void CalcularTotal()
        {
            try
            {
                base.CalcularTotal();

                int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                decimal val = 0;
                if (monOr == (int)TipoMoneda.Local)
                    val = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                else
                    val = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

                this.txtValorDoc.EditValue = Math.Abs(val);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar del control de documento contable
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterDocumentoCont_Enter(object sender, EventArgs e)
        {
            this._coDocFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterDocumentoCont_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._coDocFocus && !this._isLoadingHeader)
                {
                    this._coDocFocus = false;
                    ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                    if (master.ValidID)
                    {
                        if (this._coDocumento == null || master.Value != this._coDocumento.ID.Value)
                        {
                            //Trae el documento
                            this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, master.Value, true);
                            this.comprobanteID = this._coDocumento.ComprobanteID.Value;

                            this.prefijoID = this._coDocumento.PrefijoID.Value;
                            this.txtPrefix.Text = this.prefijoID;

                            if (!this.LoadHeader())
                                this.masterDocumentoCont.Focus();
                        }
                    }
                    else
                    {
                        this._coDocumento = null;
                        this._cuentaDoc = null;
                        this._concSaldoDoc = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "masterDocumentoCont_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Enter(object sender, EventArgs e)
        {
            this._txtNumeroDocFocus = true;
            if (this.masterDocumentoCont.ValidID)
            {
                UDT_BasicID basic = new UDT_BasicID() { Value = this.comprobanteID };
                DTO_coComprobante comp = (DTO_coComprobante)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coComprobante, basic, true);

                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";
            }
            else
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterDocumentoCont.CodeRsx);

                MessageBox.Show(msg);
                this.masterDocumentoCont.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Leave(object sender, EventArgs e)
        {
            if (this._txtNumeroDocFocus)
            {
                this._txtNumeroDocFocus = false;
                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";

                if (this.txtNumber.Text == "0")
                {
                    #region Nuevo Documento
                    this.gcDocument.DataSource = null;
                    this.Comprobante = null;
                    #endregion
                }
                else
                {
                    #region Documento existente
                    try
                    {
                        if (_bc.AdministrationModel.ComprobantePre_Exists(this.documentID, this.dtPeriod.DateTime, this.comprobanteID, Convert.ToInt32(this.txtNumber.Text)))
                        {
                            DTO_Comprobante comp = _bc.AdministrationModel.Comprobante_Get(true, true, this.dtPeriod.DateTime, this.comprobanteID, Convert.ToInt32(this.txtNumber.Text), null, null);
                            this.dtFecha.DateTime = comp.Header.Fecha.Value.Value;
                            this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(comp.Header.MdaOrigen.Value.Value.ToString());
                            this.masterMonedaHeader.Value = comp.Header.MdaTransacc.Value;

                            if (this.AsignarTasaCambio(false))
                            {
                                List<DTO_ComprobanteFooter> footer = comp.Footer.Where(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()).ToList();
                                DTO_ComprobanteFooter contra = footer.First();

                                #region Asigna los valores del header
                                this.masterCuentaHeader.Value = contra.CuentaID.Value;
                                this.masterTerceroHeader.Value = contra.TerceroID.Value;
                                this.masterProyectoHeader.Value = contra.ProyectoID.Value;
                                this.masterCentroCostoHeader.Value = contra.CentroCostoID.Value;
                                this.masterLugarGeoHeader.Value = contra.LugarGeograficoID.Value;

                                this.txtDocumentoCOMHeader.Text = contra.DocumentoCOM.Value;
                                this.txtObservacionHeader.Text = contra.Descriptivo.Value;

                                int cMonedaOrigen = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                                this.txtValorDoc.EditValue = cMonedaOrigen == (int)TipoMoneda_LocExt.Local ? contra.vlrMdaLoc.Value.Value : contra.vlrMdaExt.Value.Value;

                                this.EnableHeader(false);
                                #endregion

                                //Remueve el ultimo registro (Se cambio por la forma como trae los datos)
                                comp.Footer.RemoveAll(x =>
                                    x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString() ||
                                    x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString() ||
                                    x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());
                                
                                this.LoadTempMainData(comp);
                                this.Comprobante = comp;
                                // Cambia los valores de signos segun la cuenta
                                if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                                    this.CambiarSignoValor();

                                this.UpdateTemp(this.Comprobante);
                                this.LoadData(true);

                                this.gcDocument.Focus();
                                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                            }
                            else
                            {
                                this.validHeader = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ComprobantesCount));
                            this.txtNumber.Focus();
                            FormProvider.Master.itemSendtoAppr.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "txtNumber_Leave"));
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar del control de moneda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void cmbMonedaOrigen_Enter(object sender, EventArgs e)
        {
            this._cmbMonedaFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la moneda origen
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbMonedaOrigen_Leave(object sender, EventArgs e)
        {
            if (this._cmbMonedaFocus && !this._isLoadingHeader)
            {
                this._cmbMonedaFocus = false;

                int cMonedaOrigen = -1;
                try
                {
                    cMonedaOrigen = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidComboValue));
                    this.cmbMonedaOrigen.SelectedIndex = 0;
                    this.cmbMonedaOrigen.Focus();
                    return;
                }

                if(this._currency != cMonedaOrigen)
                {
                    this._currency = cMonedaOrigen;
                    if (this._coDocumento != null && !this.LoadHeader())
                        this.cmbMonedaOrigen.Focus();
                    else
                        this.AsignarTasaCambio(false);
                }
                else
                    this.AsignarTasaCambio(false);

                this.validHeader = false;
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();
                if (this.txtNumber.Text == "0")
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
                #region Si ya tiene datos cargados
                if (!this.dataLoaded && this.txtNumber.Text != "0")
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                #endregion
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                DTO_Comprobante comp = new DTO_Comprobante();
                try
                {
                    if (this.Comprobante == null || this.Comprobante.Footer.Count == 0)
                    {
                        this.LoadTempMainData(comp);
                        comp.Header = (DTO_ComprobanteHeader)this.LoadTempHeader(); 
                        comp.Footer = new List<DTO_ComprobanteFooter>();
                        this.Comprobante = comp;

                        this.LoadData(true);
                        this.UpdateTemp(this.Comprobante);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "grlDocument_Enter: " + ex.Message));
                }
                #endregion
            }
            else
            {
                this.masterDocumentoCont.Focus();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
            if (!this.validHeader)
            {
                this.masterDocumentoCont.Focus();
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();

                this.EnableHeader(false);
                this.dtPeriod.Enabled = true;
                this.dtFecha.Enabled = true;
                this.cmbMonedaOrigen.Enabled = true;
                this.masterDocumentoCont.EnableControl(true);
                this.txtNumber.Enabled = true;
                this.txtObservacionHeader.Enabled = true;

                if (this.cleanDoc)
                {
                    this.data = new DTO_Comprobante();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;
                    
                    this.masterDocumentoCont.Focus();

                    this.CalcularTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();
                
                int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();

                if (this.ValidGrid() && this.CanSave(monOr))
                {
                    //Si es cta debito cambia los valores
                    decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture) *-1;
                    decimal ME = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture) *-1;
                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                    {
                        this.CambiarSignoValor();
                        ML *= -1;
                        ME *= -1;
                    }
                    #region Crea el ultimo registro
                    DTO_ComprobanteFooter last = new DTO_ComprobanteFooter();
                    last.Index = this.Comprobante.Footer.Count;
                    last.CuentaID.Value = this._cuentaDoc.ID.Value;
                    last.TerceroID.Value = this.masterTerceroHeader.Value;
                    last.ProyectoID.Value = this.masterProyectoHeader.Value;
                    last.CentroCostoID.Value = this.masterCentroCostoHeader.Value;
                    last.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    last.ConceptoCargoID.Value = string.IsNullOrEmpty(this._cuentaDoc.ConceptoCargoID.Value) ? this.defConceptoCargo : this._cuentaDoc.ConceptoCargoID.Value;
                    last.LugarGeograficoID.Value = this.masterLugarGeoHeader.Value;
                    last.PrefijoCOM.Value = this.prefijoID;
                    last.DocumentoCOM.Value = this.txtDocumentoCOMHeader.Text;
                    last.ActivoCOM.Value = string.Empty;
                    last.ConceptoSaldoID.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                    last.IdentificadorTR.Value = 0;
                    last.Descriptivo.Value = this.txtObservacionHeader.Text;
                    last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                    last.vlrBaseML.Value = 0;
                    last.vlrBaseME.Value = 0;
                    last.vlrMdaLoc.Value = ML;//Math.Abs(ML);
                    last.vlrMdaExt.Value = ME;// Math.Abs(ME);

                    last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                    last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    this.Comprobante.Footer.Add(last);

                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            base.TBDelete();
            try
            {
                if (this.ValidGrid())
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgDelDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Document);

                    if (MessageBox.Show(msgDelDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.gvDocument.ActiveFilterString = string.Empty;
                        
                        int compNro = Convert.ToInt32(this.txtNumber.Text);
                        if (compNro == 0)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                            return;
                        }
                        _bc.AdministrationModel.ComprobantePre_Delete(this.documentID, this._actFlujo.ID.Value, this.dtPeriod.DateTime, this.comprobanteID, compNro);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CancelledComp));

                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        this.newDoc = true;
                        this.deleteOP = true;
                        this.Comprobante = new DTO_Comprobante();
                        this.gcDocument.DataSource = this.Comprobante.Footer;

                        this.CleanHeader(true);
                        this.EnableHeader(true);
                        this.masterDocumentoCont.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {

            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                result = _bc.AdministrationModel.ComprobantePre_Add(this.documentID, this.frmModule, this.Comprobante, this.areaFuncionalID, this.prefijoID, null, null);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                //Remueve el ultimo registro
                this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());//At(this.Comprobante.Footer.Count - 1);

                // Cambia los valores de signos segun la cuenta
                if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                    this.CambiarSignoValor();

                if (result.Result.Equals(ResultValue.OK))
                {
                    string[] vars = Regex.Split(result.ResultMessage, "&&");
                    this.compNro = vars.ElementAt(2);

                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.resultOK = true;
                }
                else
                {
                    this.resultOK = false;
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                int compNro = Convert.ToInt32(this.txtNumber.Text);
                if (compNro == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                    return;
                }

                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.ComprobantePre_SendToAprob(this.documentID, this._actFlujo.ID.Value, this.frmModule, this.dtPeriod.DateTime, this.comprobanteID, compNro, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                #region Genera Reporte

                if (obj.GetType() == typeof(DTO_Alarma))
                {
                    string numDoc = ((DTO_Alarma)obj).NumeroDoc;
                    bool finaliza = ((DTO_Alarma)obj).Finaliza;
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_DocumentoContable(Convert.ToInt32(numDoc), finaliza, AppDocuments.DocumentoContable, ExportFormatType.pdf);
                    
                    if (reportName == string.Empty)
                       MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                }

                #endregion

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.Comprobante = new DTO_Comprobante();
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoContable.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
