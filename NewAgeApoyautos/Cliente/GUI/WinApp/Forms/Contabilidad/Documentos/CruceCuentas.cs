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
using DevExpress.XtraGrid.Columns;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class CruceCuentas : DocumentAuxiliarForm
    {
        //public CruceCuentas()
        //{
        //    InitializeComponent();
        //}

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
                FormProvider.Master.itemSendtoAppr.Enabled = false;

                this.CleanHeader(true);
                this.EnableHeader(true);

                this.ChangeStatusControls(1);
            }
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        //protected override void SendToApproveMethod()
        //{
        //    this.gcDocument.DataSource = this.Comprobante.Footer;
        //    FormProvider.Master.itemSendtoAppr.Enabled = false;

        //    this.CleanHeader(true);
        //    this.EnableHeader(true);

        //    this.ChangeStatusControls(1);
        //}

        #endregion

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl;
        private DTO_coDocumentoAjuste _ajuste;
        private TipoMoneda_LocExt _tipoMoneda;
        private decimal _tasaCambio;
        private string compID;
        private int _compNro = 0;
        private DTO_coPlanCuenta _cuentaDoc;
        private bool _compLoaded = false;
        private bool _headerLoaded = false;

        private string _docIntExt = string.Empty;
        private bool validcoDoc = true;

        //Variables de Reportes
        private string reportName;

        #endregion

        #region Propiedades
        
        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_CruceCuentas TempData
        {
            get
            {
                return (DTO_CruceCuentas)this.data;
            }
            set
            {
                this.data = value;
                if (value.Comp != null)
                {
                    this.Comprobante = value.Comp;
                }
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Cambia estado de los controles segun indice (busqueda segun el documento)
        /// </summary>
        /// <param name="index">Indice a cambiar estado (1 - Externo / 2 - interno)</param>
        private void ChangeStatusControls(int index)
        {
            try
            {
                this._docIntExt = string.Empty;
                switch (index)
                {
                    case 1:
                        #region Doc Externo
                        this.masterPrefijo_.EnableControl(false);
                        this.masterTercero_.EnableControl(true);
                        this.txtDocumentoNroInt.Enabled = false;
                        this.txtDocumentoNroExt.Enabled = true;

                        this.masterPrefijo_.Value = string.Empty;
                        this.txtDocumentoNroInt.Text = string.Empty;
                        #endregion
                        break;
                    case 2:
                        #region Doc Interno
                        this.masterPrefijo_.EnableControl(true);
                        this.masterTercero_.EnableControl(false);
                        this.txtDocumentoNroInt.Enabled = true;
                        this.txtDocumentoNroExt.Enabled = false;

                        this.masterTercero_.Value = string.Empty;
                        this.txtDocumentoNroExt.Text = string.Empty;
                        #endregion
                        break;
                    default:
                        #region Error - sin seleccion
                        this.masterPrefijo_.EnableControl(false);
                        this.masterTercero_.EnableControl(false);
                        this.txtDocumentoNroInt.Enabled = false;
                        this.txtDocumentoNroExt.Enabled = false;

                        this.masterPrefijo_.Value = string.Empty;
                        this.txtDocumentoNroInt.Text = string.Empty;
                        this.masterTercero_.Value = string.Empty;
                        this.txtDocumentoNroExt.Text = string.Empty;
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "ChangeStatusControls"));
            }
        }

        /// <summary>
        /// Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentInt()
        {
            try
            {
                string prefijo = this.masterPrefijo_.Value;
                int docId = Convert.ToInt32(this.masterDocumento.Value);
                int docInt = Convert.ToInt32(this.txtDocumentoNroInt.Text);
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(docId, prefijo, docInt);
                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene un documento externo
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = masterTercero_.Value;
                int docId = Convert.ToInt32(this.masterDocumento.Value);
                string docExt = this.txtDocumentoNroExt.Text;
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(docId, tercero, docExt);

                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Asigna los valores a los campos de detalle
        /// </summary>
        private void AssignValues()
        {            
            //Muestra los valores correspondientes en el header
            this.masterCuenta_.Value = this._ctrl.CuentaID.Value;
            this.masterCentroCosto.Value = this._ctrl.CentroCostoID.Value;
            this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
            this.masterLugarGeo.Value = this._ctrl.LugarGeograficoID.Value;
            this.masterMoneda.Value = this._ctrl.MonedaID.Value;

            this.dtFechaDoc.DateTime = this._ctrl.FechaDoc.Value.Value;
            //this.dtFecha.DateTime = this._ctrl.FechaDoc.Value.Value;
            //this.dtFecha.Properties.MinValue = this._ctrl.FechaDoc.Value.Value;

            this.masterDocumento.EnableControl(false);
            this.masterTercero_.EnableControl(false);
            this.masterPrefijo_.EnableControl(false);
            this.txtDocumentoNroExt.Enabled = false;
            this.txtDocumentoNroInt.Enabled = false;

            if (this.txtNumeroDoc.Text == "0")
            {
                this.txtDescripcionHeader.Enabled = true;
                this.txtDescripcionHeader.Focus();
                this.txtDescripcionHeader.Text = "CRUCE DE CUENTAS";
            }
            else 
                this.txtDescripcionHeader.Text = "CRUCE DE CUENTAS";

            this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
            this.txtPrefix.Text = this._ctrl.PrefijoID.Value;
            this._cuentaDoc = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, this._ctrl.CuentaID.Value, true);
        }

        /// <summary>
        /// Funcion que calcula el valor total de los anticipos pendientes
        /// </summary>
        private decimal CalcularSaldos(DTO_glDocumentoControl doc)
        {
            string concSaldo = string.Empty;
            int creditoInd = 1;
            if (!string.IsNullOrEmpty(doc.CuentaID.Value))
            {
                DTO_coPlanCuenta _cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, doc.CuentaID.Value, false);
                concSaldo = _cta.ConceptoSaldoID.Value;
                creditoInd = _cta.Naturaleza.Value == 2 ? -1 : 1;
            }
            else
                concSaldo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);

            DTO_coCuentaSaldo _ctaSaldoDTO = _bc.AdministrationModel.Saldo_GetByDocumento(doc.CuentaID.Value, concSaldo, doc.NumeroDoc.Value.Value, string.Empty);
            if (_ctaSaldoDTO != null)
            {
                decimal sumaML = _ctaSaldoDTO.DbOrigenLocML.Value.Value + _ctaSaldoDTO.DbOrigenExtML.Value.Value + _ctaSaldoDTO.CrOrigenLocML.Value.Value + _ctaSaldoDTO.CrOrigenExtML.Value.Value
                    + _ctaSaldoDTO.DbSaldoIniLocML.Value.Value + _ctaSaldoDTO.DbSaldoIniExtML.Value.Value + _ctaSaldoDTO.CrSaldoIniLocML.Value.Value + _ctaSaldoDTO.CrSaldoIniExtML.Value.Value;
                return Math.Round(sumaML * creditoInd, 2);
            }
            else 
                return 0;
        }

        /// <summary>
        /// Calcula los totales
        /// </summary>
        protected override void CalcularTotal()
        {
            if (!this.newDoc)
            {
                base.CalcularTotal();

                decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);

                if (this._cuentaDoc!=null && this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                    ML *= -1;

                this.txtAjuste.Text = (-1 * ML).ToString();

                if (string.IsNullOrEmpty(this.txtSaldo.Text.Trim())) this.txtSaldo.Text = "0";
                this.txtSaldoAjustado.Text = (Convert.ToDecimal(this.txtSaldo.EditValue, CultureInfo.CurrentCulture) - ML).ToString();
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.CruceCuentas;
            InitializeComponent();

            this.frmModule = ModulesPrefix.co;
            base.SetInitParameters();

            _bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false);
            _bc.InitMasterUC(this.masterTercero_, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterPrefijo_, AppMasters.glPrefijo, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterCuenta_, AppMasters.coPlanCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);

            this._tipoMoneda = TipoMoneda_LocExt.Local;
            this._tasaCambio = 0;
            this._ctrl = new DTO_glDocumentoControl();
            this._ajuste = new DTO_coDocumentoAjuste();

            //Variables de inicio
            string coDocID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocContableCruceCtas);
            #region Validaciones
            //Valida el coDocumento
            if (string.IsNullOrWhiteSpace(coDocID))
            {
                validcoDoc = false;
                return;
            }

            DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, coDocID, true);

            //Valida que tenga comprobante
            if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
            {
                validcoDoc = false;
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCompDoc));
                return;
            }
            this.compID = coDoc.ComprobanteID.Value;

            #endregion
        }
        
        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            ChangeStatusControls(1);

            #region Disable Control Master
            this.masterCuenta_.EnableControl(false);
            this.masterCentroCosto.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterLugarGeo.EnableControl(false);
            this.masterMoneda.EnableControl(false); 
            #endregion

            this.masterDocumento.EnableControl(true);            
            base.AfterInitialize();
            if (this.dtPeriod.DateTime.Month == DateTime.Now.Date.Month)
            {
                this.dtFecha.DateTime = DateTime.Now.Date;
                this.dtFechaDoc.DateTime = DateTime.Now.Date;
            }
            else
            {
                this.dtFecha.DateTime = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));
                this.dtFechaDoc.DateTime = this.dtFecha.DateTime;
            }
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.txtNumeroDoc.Enabled = false;//enable;
            this.dtFecha.Enabled = enable;

            this.masterDocumento.EnableControl(enable);
            this.masterPrefijo_.EnableControl(enable);
            this.masterTercero_.EnableControl(enable);
            this.txtDocumentoNroInt.Enabled = enable;
            this.txtDocumentoNroExt.Enabled = enable;
           // this.txtDescripcionHeader.Enabled = enable;

            this.rbtPrefijo.Enabled = enable;
            this.rbtTercero.Enabled = enable;

            //this.btnAnticipo.Enabled = enable ? false : true;
            //this.btnLiquida.Enabled = enable ? false : true;
        }
        
        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                this.txtNumeroDoc.Text = "0";

                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                this.dtPeriod.Text = periodo;

                this.txtPrefix.Text = string.Empty;
                this.masterDocumento.Value = string.Empty;
            }

            this.masterPrefijo_.Value = string.Empty;
            this.masterTercero_.Value = string.Empty;
            this.masterCuenta_.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;
            this.txtDocumentoNroExt.Text = string.Empty;
            this.txtDocumentoNroInt.Text = string.Empty;
            this.txtSaldo.Text = "0";
            this.txtSaldoAjustado.Text = "0";
            this.txtAjuste.Text = "0";
            this.monedaId = string.Empty;
            //this.dtFechaDoc.Text = string.Empty;
            this.txtDescripcionHeader.Text = string.Empty;
            this._docIntExt = string.Empty;

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override object LoadTempHeader()
        {
            try
            {
                // Ajuste
                this._ajuste.EmpresaID = this._ctrl.EmpresaID;
                this._ajuste.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ajuste.IdentificadorTR.Value = this._ctrl.NumeroDoc.Value;
                this._ajuste.Valor.Value = Convert.ToDecimal(this.txtSaldo.EditValue, CultureInfo.InvariantCulture);

                // Documento
                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ctrl.Observacion.Value = this.txtDescripcionHeader.Text;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.TasaCambioDOCU.Value = this._tasaCambio;
                this._ctrl.TasaCambioCONT.Value = this._tasaCambio;
                this._ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                this._ctrl.DocumentoNro.Value = 0;
                this._ctrl.ComprobanteID.Value = this.compID;
                this._ctrl.ComprobanteIDNro.Value = 0;
                this._ctrl.DocumentoPadre.Value = null;
                this._ctrl.Valor.Value = null;
                this._ctrl.Estado.Value = (Int16)EstadoDocControl.Aprobado;

                #region Comprobante
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = this.compID;
                header.ComprobanteNro.Value = this._compNro;
                header.EmpresaID.Value = this.empresaID;
                header.Fecha.Value = this.dtFecha.DateTime;
                header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                header.MdaOrigen.Value = Convert.ToByte(this._tipoMoneda);
                header.MdaTransacc.Value = this.monedaId;
                header.PeriodoID.Value = this.dtPeriod.DateTime;
                header.TasaCambioBase.Value = this._tasaCambio;
                header.TasaCambioOtr.Value = this._tasaCambio;

                DTO_Comprobante comp = new DTO_Comprobante();
                comp.Header = header;
                comp.Footer = new List<DTO_ComprobanteFooter>();

                comp.coDocumentoID = this.documentID.ToString();
                comp.CuentaID = this._ctrl.CuentaID.Value;
                comp.ProyectoID = this._ctrl.ProyectoID.Value;
                comp.CentroCostoID = this._ctrl.CentroCostoID.Value;
                comp.LineaPresupuestoID = this._ctrl.LineaPresupuestoID.Value;
                comp.LugarGeograficoID = this._ctrl.LugarGeograficoID.Value;
                comp.coDocumentoID = this._ctrl.DocumentoID.Value.ToString();
                comp.Observacion = this._ctrl.Observacion.Value;
                comp.TerceroID = this._ctrl.TerceroID.Value;
                comp.TerceroID = this._ctrl.TerceroID.Value;
                comp.PrefijoID = this._ctrl.PrefijoID.Value;
                comp.DocumentoNro = this._ctrl.DocumentoNro.Value.Value;
                comp.DocumentoTercero = this._ctrl.DocumentoTercero.Value;
                comp.TipoDoc = (DocumentoTipo)_ctrl.DocumentoTipo.Value;

                this.Comprobante = comp;
                this.comprobanteID = this.compID;
                #endregion

                DTO_CruceCuentas Ajuste = new DTO_CruceCuentas();
                Ajuste.AjusteDoc = this._ajuste;
                Ajuste.DocControl = this._ctrl;
                Ajuste.Comp = comp;

                return Ajuste;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "LoadTempHeader"));
                return null;
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            try
            {
                TipoMoneda_LocExt tm = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                int monOr = (int)tm;
                this._tipoMoneda = tm;

                if (monOr == (int)TipoMoneda.Local)
                    this.monedaId = this.monedaLocal;
                else
                    this.monedaId = this.monedaExtranjera;

                //Sio la empresa permite multimoneda
                if (this.multiMoneda)
                {
                    decimal tc = this.LoadTasaCambio(monOr);
                    if (tc == 0)
                    {
                        this.validHeader = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                        return false;
                    }

                    this._tasaCambio = tc;
                }
                else
                    this._tasaCambio = 0;

                if (!fromTop)
                    this.validHeader = true;
                else
                    this.validHeader = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "LoadTempData"));
            }

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            if (!validcoDoc)
                return false;

            //Valida que ya este asignada una tasa de cambio
            if (!this.AsignarTasaCambio(false))
                return false;

            //Valida que se haya seleccionado un documento
            if (this._ctrl == null || this._ctrl == new DTO_glDocumentoControl())
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NoDoc));
                return false;
            }

            //Valida que la observacion tenga información
            if (string.IsNullOrWhiteSpace(this.txtDescripcionHeader.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtDescripcionHeader.Focus();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave(int monOr)
        {
            decimal difLoc = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
            decimal difExt = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

            decimal saldo = Convert.ToDecimal(this.txtSaldo.EditValue, CultureInfo.InvariantCulture);
            if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                saldo *= -1;

            if (monOr == (int)TipoMoneda.Local)
            {
                if ((difLoc < 0 && saldo > 0) || (difLoc > 0 && saldo < 0) || Math.Abs(difLoc) >  Math.Abs(saldo))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidSaldoML));
                    return false;
                }
            }
            else
            {
                if ((difExt < 0 && saldo > 0) || (difExt > 0 && saldo < 0) || Math.Abs(difExt) > Math.Abs(saldo))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidSaldoME));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            try
            {
                DTO_CruceCuentas Ajuste = (DTO_CruceCuentas)aux;
                DTO_glDocumentoControl ctrl = Ajuste.DocControl;
                DTO_coDocumentoAjuste ajuste = Ajuste.AjusteDoc;
                DTO_Comprobante comp = Ajuste.Comp;

                this._ctrl = ctrl;

                DTO_ComprobanteHeader header = comp.Header;
                this.comprobanteID = header.ComprobanteID.Value;
                this._compNro = header.ComprobanteNro.Value.Value;

                if (comp.Footer == null)
                    comp.Footer = new List<DTO_ComprobanteFooter>();

                bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, header.PeriodoID.Value.Value, this.comprobanteID, this._compNro);
                if (usefulTemp || this._compNro == 0)
                {
                    this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
                    this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.ToString();
                    this.dtFecha.DateTime = ctrl.Fecha.Value.Value;

                    this.AssignValues();
                    this.txtDescripcionHeader.Text = "CRUCE DE CUENTAS";
                    this.txtSaldo.Text = ajuste.Valor.Value.ToString();

                    DTO_glDocumentoControl ajustadoDoc = _bc.AdministrationModel.glDocumentoControl_GetByID(Convert.ToInt32(ajuste.IdentificadorTR.Value.Value));
                    this.masterDocumento.Value = ajustadoDoc.DocumentoID.Value.ToString();

                    if (ajustadoDoc.DocumentoTipo.Value == (int)DocumentoTipo.DocInterno)
                    {
                        this.rbtPrefijo.Checked = true;

                        this.masterPrefijo_.Value = ctrl.PrefijoID.Value;
                        this.txtDocumentoNroInt.Text = ctrl.DocumentoNro.Value.Value.ToString();
                        comp.PrefijoID = ctrl.PrefijoID.Value;
                        comp.DocumentoNro = ctrl.DocumentoNro.Value.Value;
                    }
                    else
                    {
                        this.rbtTercero.Checked = true;

                        this.masterTercero_.Value = ctrl.TerceroID.Value;
                        this.txtDocumentoNroExt.Text = ctrl.DocumentoTercero.Value;
                        comp.TerceroID = ctrl.TerceroID.Value;
                        comp.DocumentoTercero = ctrl.DocumentoTercero.Value;
                    }

                    //Si se presenta un problema asignando la tasa de cambio lo bloquea
                    if (this.ValidateHeader())
                    {
                        this.EnableHeader(false);
                        this.txtDescripcionHeader.Enabled = false;

                        this.Comprobante = comp;
                        this.data = Ajuste;
                        this.LoadData(true);
                        this.validHeader = true;
                        this._headerLoaded = true;
                        this._compLoaded = true;
                        this.gcDocument.Focus();
                    }
                    else
                        this.CleanHeader(true);
                }
                else
                {
                    this.validHeader = false;
                    string rsx = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCompTemp);
                    string msg = string.Format(rsx, this.comprobanteID, this._compNro, header.PeriodoID.Value);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "LoadTempData"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtPrefijo_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(2);
        }

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtTercero_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(1);
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control del num de doc externo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocumentoNroExt_Leave(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;

                if (this._docIntExt != this.txtDocumentoNroExt.Text)
                {
                    if (this.masterDocumento.ValidID)
                    {
                        if (this.masterTercero_.ValidID)
                        {
                            if (!string.IsNullOrEmpty(this.txtDocumentoNroExt.Text))
                            {
                                this._docIntExt = this.txtDocumentoNroExt.Text;
                                this._ctrl = this.GetDocumentExt();
                                if (this._ctrl == null || this._ctrl.Estado.Value.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                    MessageBox.Show(msg);
                                    this.masterTercero_.Focus();
                                }
                                else
                                {
                                    this.AssignValues();
                                    this.Comprobante = new DTO_Comprobante();
                                    this._compLoaded = false;
                                    decimal saldo = this.CalcularSaldos(this._ctrl);
                                    this.txtSaldo.Text = saldo.ToString();
                                }
                            }
                            else
                                this.masterTercero_.Focus();
                        }
                        else
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            MessageBox.Show(string.Format(msg, this.masterTercero_.LabelRsx));
                            this.masterTercero_.Focus();
                        }
                    }
                    else
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                        MessageBox.Show(string.Format(msg, this.masterDocumento.LabelRsx));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "txtDocumentNroExt_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control del num de doc interno
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocumentoNroInt_Leave(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (this._docIntExt != this.txtDocumentoNroInt.Text)
                {
                    if (this.masterDocumento.ValidID)
                    {
                        if (this.masterPrefijo_.ValidID)
                        {
                            if (!string.IsNullOrEmpty(this.txtDocumentoNroInt.Text))
                            {
                                this._docIntExt = this.txtDocumentoNroInt.Text;
                                this._ctrl = this.GetDocumentInt();
                                if (this._ctrl == null || this._ctrl.Estado.Value.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                    MessageBox.Show(msg);
                                    this.txtDocumentoNroInt.Focus();
                                }
                                else
                                {
                                    this.AssignValues();
                                    this.Comprobante = new DTO_Comprobante();
                                    this._compLoaded = false;
                                    decimal saldo = this.CalcularSaldos(this._ctrl);
                                    this.txtSaldo.Text = saldo.ToString();
                                }
                            }
                            else
                                this.masterPrefijo_.Focus();
                        }
                        else
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            MessageBox.Show(string.Format(msg, this.masterPrefijo_.LabelRsx));
                            this.masterPrefijo_.Focus();
                        }
                    }
                    else
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                        MessageBox.Show(string.Format(msg, this.masterDocumento.LabelRsx));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "txtDocumentNroInt_Leave"));
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
                if (this.txtNumeroDoc.Text == "0"|| this._ctrl.DocumentoID.Value.Value!=AppDocuments.CruceCuentas)
                {
                    //FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                this.txtDescripcionHeader.Enabled = false;
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_CruceCuentas ajuste = (DTO_CruceCuentas)this.LoadTempHeader();
                        DTO_Comprobante comp = ajuste.Comp;

                        if (this._compLoaded)
                            comp.Footer = this.Comprobante.Footer;

                        ajuste.Comp = comp;
                        this.TempData = ajuste;

                        if (this.Comprobante.Footer.Count == 0)
                        {
                            //FormProvider.Master.itemSendtoAppr.Enabled = false;
                            FormProvider.Master.itemExport.Enabled = false;
                            FormProvider.Master.itemPrint.Enabled = false;
                        }

                        this.LoadData(true);
                        this.UpdateTemp(this.TempData);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "grlDocument_Enter" + ex.Message));
                }
                #endregion
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
                if (this.cleanDoc)
                {
                    this.data = new DTO_CruceCuentas();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;

                    this.ChangeStatusControls(1);
                    this.rbtTercero.Focus();
                    this.masterDocumento.EnableControl(true);

                    //this.btnAnticipo.Enabled = false;
                    //this.btnLiquida.Enabled = false;
                    
                    this._compLoaded = false;
                    this._headerLoaded = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "TBNew"));
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
                
                int monOr = (int)this._tipoMoneda;
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();
                if (this.ValidGrid() && this.CanSave(monOr))
                {
                    //Si es cta debito cambia los valores
                    decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture) *-1;
                    decimal ME = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture) *-1;
                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito)
                    {
                        this.CambiarSignoValor();
                        ML *= -1;
                        ME *= -1;
                    }

                    #region Crea el ultimo registro
                    DTO_ComprobanteFooter last = new DTO_ComprobanteFooter();
                    last.Index = this.Comprobante.Footer.Count;
                    last.CuentaID.Value = this._cuentaDoc.ID.Value;
                    last.TerceroID.Value = this._ctrl.TerceroID.Value;//this.masterTercero_.Value;
                    last.ProyectoID.Value = this.masterProyecto.Value;
                    last.CentroCostoID.Value = this.masterCentroCosto.Value;
                    last.LineaPresupuestoID.Value = this._ctrl.LineaPresupuestoID.Value;//this.defLineaPresupuesto;
                    last.ConceptoCargoID.Value = string.IsNullOrEmpty(this._cuentaDoc.ConceptoCargoID.Value) ? this.defConceptoCargo : this._cuentaDoc.ConceptoCargoID.Value;
                    last.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                    last.PrefijoCOM.Value = this._ctrl.PrefijoID.Value;//this.masterPrefijo_.Value;
                    last.DocumentoCOM.Value = this._ctrl.DocumentoTercero.Value + "*" + this._ctrl.DocumentoNro.Value.Value.ToString();//this.txtDocumentoNroInt.Text : this.txtDocumentoNroExt.Text;
                    last.ActivoCOM.Value = string.Empty;
                    last.ConceptoSaldoID.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                    last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                    last.Descriptivo.Value = this.txtDescripcionHeader.Text ;

                    last.IdentificadorTR.Value = Convert.ToInt32(this.txtNumeroDoc.Text);// this._ctrl.NumeroDoc.Value;

                    decimal vlrDoc = Convert.ToDecimal(this.txtSaldo.EditValue, CultureInfo.InvariantCulture);
                    last.vlrBaseML.Value = 0;
                    last.vlrBaseME.Value = 0;
                    last.vlrMdaLoc.Value = ML;
                    last.vlrMdaExt.Value = ME;
                    last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                    last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    this.Comprobante.Footer.Add(last);
                    this.Comprobante.Observacion = this.masterDocumento.Value;
                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "SaveThread"));
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
                        int compNro = this.Comprobante.Header.ComprobanteNro.Value.Value;

                        _bc.AdministrationModel.ComprobantePre_Delete(this.documentID, this._actFlujo.ID.Value, this.dtPeriod.DateTime, this.comprobanteID, compNro);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CancelledComp));
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        this.newDoc = true;
                        this.deleteOP = true;
                        this.data = new DTO_CruceCuentas();
                        this.Comprobante = new DTO_Comprobante();
                        this.gcDocument.DataSource = this.Comprobante.Footer;

                        this.CleanHeader(true);
                        this.EnableHeader(true);

                        //this.btnAnticipo.Enabled = false;
                        //this.btnLiquida.Enabled = false;

                        this._compLoaded = false;
                        this._headerLoaded = false;

                        this.ChangeStatusControls(1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "TBDelete"));
            }
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

                this.TempData.DocControl.Descripcion.Value = this.TempData.Comp.Footer.First().Descriptivo.Value;
                result = _bc.AdministrationModel.CruceCuentas_Ajustar(this.documentID, this._actFlujo.ID.Value, this.TempData.DocControl, 
                    this.TempData.AjusteDoc, this.TempData.Comp);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                #region Genera el reporte

                if (result.Result == ResultValue.OK)
                {
                    #region Pregunta si desea abrir los reportes

                    bool deseaImp = false;
                  
                    //string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    //var msg = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (msg == DialogResult.Yes)
                    //    deseaImp = true;

                    #endregion
                    #region Genera e imprime los reportes

                    //int numDoc = Convert.ToInt32(result.ExtraField);
                    //this.reportName = this._bc.AdministrationModel.ReportesContabilidad_DocumentoContable(numDoc, true, AppDocuments.CruceCuentas, ExportFormatType.pdf);

                    //if (this.reportName == string.Empty)
                    //    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                    //else
                    //{
                    //    if (deseaImp)
                    //    {
                    //        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, reportName.ToString());
                    //        Process.Start(fileURl);
                    //    }
                    //}

                    #endregion
                } 

                #endregion

                if (result.Result.Equals(ResultValue.OK))
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_CruceCuentas();
                    this.Comprobante = new DTO_Comprobante();
                    this._compLoaded = false;
                    this._headerLoaded = false;
                    this.resultOK = true;
                }
                else
                {
                    //Remueve el ultimo registro
                    this.resultOK = false;
                    this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());//At(this.Comprobante.Footer.Count - 1);

                    // Cambia los valores de signos segun la cuenta
                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                        this.CambiarSignoValor();
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    base.ImportThread();
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CruceCuentas.cs", "ImportThread"));
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
