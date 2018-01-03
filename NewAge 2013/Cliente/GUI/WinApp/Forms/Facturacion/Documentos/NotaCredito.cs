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
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Facturas de Venta
    /// </summary>
    public partial class NotaCredito : DocumentFacturaForm
    {
        //public NotaCredito()
        //{
        //    InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            base.SaveMethod();

            this.masterPrefijoDoc.EnableControl(true);
            this.masterPrefijoDoc.Focus();
            this.masterPrefijoNot.EnableControl(true);
            this.txtFacturaNro.Enabled = true;
            this.txtNotaNro.Enabled = true;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            base.SendToApproveMethod();

            this.masterPrefijoDoc.EnableControl(true);
            this.masterPrefijoDoc.Focus();
            this.masterPrefijoNot.EnableControl(true);
            this.txtFacturaNro.Enabled = true;
            this.txtNotaNro.Enabled = true;
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        private DTO_faAsesor _asesor = null;
        private DTO_coTercero _tercero = null;
        private string _coDocumentoID;
        private string _facturaTipoID;
        private bool _prefijoDocFocus = false;
        private bool _prefijoNotFocus = false;
        private bool _txtNotaNroFocus = false;
        private bool _txtFacturaNroFocus = false;
        private bool _loadDataFactura = false;
        private decimal _factValorNeto = 0;
        private List<DTO_glConsultaFiltro> filtrosfacturaTipo = null;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion del header  a partir de condiciones del formulario
        /// Trae una cuenta y el respectivo concepto de saldo
        /// </summary>
        private bool GetCuenta(bool getAllData)
        {
            int cMonedaOrigen = (int)this.tipoMonedaOr;
            #region Tarea la cuenta de acuerdo al documento y la moneda
            string cuentaID = cMonedaOrigen == (int)TipoMoneda_LocExt.Local ? this.coDocumento.CuentaLOC.Value : this.coDocumento.CuentaEXT.Value;
            DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, cuentaID, true);
            if (cta != null)
            {
                this.concSaldoDoc = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cta.ConceptoSaldoID.Value, true);
                if (this.concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Interno )//|| this.coDocumento.DocumentoID.Value != this.documentID.ToString())
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocInt));

                    this.CleanHeader(false);

                    this.isLoadingHeader = true;
                    this.EnableHeader(false);
                    this.masterPrefijoNot.EnableControl(true);
                    this.txtNotaNro.Enabled = true;
                    this.masterCliente.EnableControl(true);
                    this.masterCliente.Focus();
                    this.isLoadingHeader = false;

                    return false;
                }
                else if (this.coDocumento.DocumentoID.Value != AppDocuments.FacturaVenta.ToString())
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocID));

                    this.CleanHeader(false);

                    this.isLoadingHeader = true;
                    this.EnableHeader(false);
                    this.masterPrefijoNot.EnableControl(true);
                    this.txtNotaNro.Enabled = true;
                    this.masterCliente.EnableControl(true);
                    this.masterCliente.Focus();
                    this.isLoadingHeader = false;

                    return false;
                }
                else
                {
                    this.cuentaDoc = cta;
                    if (getAllData)
                    {
                        this.masterCentroCosto.Value = this.defCentroCosto;
                        this.masterProyecto.Value = this.defProyecto;

                        #region Habilita o deshabilita los campos segun la cuenta
                        // Abilita o desabilita el lugar geografico
                        if (this.cuentaDoc.LugarGeograficoInd.Value.Value)
                            this.masterLugarGeo.EnableControl(true);
                        else
                            this.masterLugarGeo.EnableControl(false);
                        // Abilita o desabilita el centro de costo
                        if (this.cuentaDoc.CentroCostoInd.Value.Value)
                            this.masterCentroCosto.EnableControl(true);
                        else
                            this.masterCentroCosto.EnableControl(false);

                        // Abilita o desabilita el proyecto
                        if (this.cuentaDoc.ProyectoInd.Value.Value)
                            this.masterProyecto.EnableControl(true);
                        else
                            this.masterProyecto.EnableControl(false);
                        #endregion
                    }
                    return true;
                }
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NocoContCta));

                this.CleanHeader(false);

                this.isLoadingHeader = true;
                this.EnableHeader(false);
                this.masterPrefijoNot.EnableControl(true);
                this.txtNotaNro.Enabled = true;
                this.masterCliente.EnableControl(true);
                this.masterCliente.Focus();
                this.isLoadingHeader = false;

                return false;
            }
            #endregion
        }

        /// <summary>
        /// Funcion para validacion de fechas
        /// </summary>
        private void ValidateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFechaFact.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFechaFact.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaFact.DateTime = new DateTime(currentYear, currentMonth, minDay);
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;

            try
            {
                validRow = base.ValidateRow(fila);

                if (validRow)
                {
                    #region Valida que la suma de los netos no supere al maximo permitido en valorFondo ni inferior a 0
                    if (Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture) != 0)
                    {
                        GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                        string noCuentaIca = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoCuentaIca);

                        if (this.cuentaIcaNoExiste)
                        {
                            this.gvDocument.SetColumnError(col, noCuentaIca);
                            validRow = false;
                        }
                    }
                    #endregion
                }

                if (validRow)
                {
                    this.isValid = true;
                    this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.NotaCredito;
            this.frmModule = ModulesPrefix.fa;
            InitializeComponent();

            base.SetInitParameters();

            this._bc.InitMasterUC(this.masterPrefijoDoc, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoNot, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterFacturaTipo, AppMasters.faFacturaTipo, true, true, true, false);
            this._bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            this._bc.InitMasterUC(this.masterAsesor, AppMasters.faAsesor, true, true, true, false);
            this._bc.InitMasterUC(this.masterMonedaFact, AppMasters.glMoneda, false, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);

            this.filtrosfacturaTipo = new List<DTO_glConsultaFiltro>();
            this.filtrosfacturaTipo.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "NotaCreditoInd",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "1"
            });

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.EnableFooter(false);
            this.EnableHeader(false);

            if (!headerLoaded)
            {
                this.txtNumeroDoc.Text = "0";
                this.ValidateDates();
                this.masterPrefijoDoc.EnableControl(true);
                this.txtFacturaNro.Enabled = true;
                this.masterPrefijoNot.EnableControl(true);
                this.txtNotaNro.Enabled = true;
                this.txtFacturaNro.Text = string.Empty;
                if (string.IsNullOrEmpty(this.txtNotaNro.Text)) this.txtNotaNro.Text = "0";
                this.masterPrefijoDoc.Focus();
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
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                this.dtPeriod.Text = periodo;
                this.prefijoID = string.Empty;
                this.txtPrefix.Text = this.prefijoID;
                this.txtNumeroDoc.Text = "0";
                this.facturaNro = 0;
                this.masterPrefijoDoc.Value = string.Empty;
                this.txtFacturaNro.Text = string.Empty;
                this.masterPrefijoNot.Value = string.Empty;
                this.txtNotaNro.Text = "0";
            }
          
            //this._porcICA = 0;
            //this._porcRemesa = 0;
            this.coDocumento = new DTO_coDocumento();
            this.concSaldoDoc = new DTO_glConceptoSaldo();
            this.cuentaDoc = new DTO_coPlanCuenta();
            this.cliente = new DTO_faCliente();
            this.facturaTipo = new DTO_faFacturaTipo();
            //this.mvtoTipo = new DTO_faMovimientoTipo();
            this._asesor = new DTO_faAsesor();
            this._tercero = new DTO_coTercero();
            this._factValorNeto = 0;
            
            
            this.masterCliente.Value = string.Empty;
            this.masterFacturaTipo.Value = string.Empty;
            this.masterAsesor.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;
            this.masterMonedaFact.Value = string.Empty;
            this.masterZona.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;

            this.ValidateDates();
            this.txtTasaCambio.EditValue = 0;
            this.rtbDescrDoc.Text = string.Empty;
            this.pceDescrDoc.Text = string.Empty;
            this.rtbEncabezado.Text = string.Empty;
            this.pceEncabezado.Text = string.Empty;
            this.txtPorcICA.EditValue = 0;
            this.txtPorcRemesa.EditValue = 0;
            this.rtbPiePagina.Text = string.Empty;
            this.pcePiePagina.Text = string.Empty;
            this.monedaId = this.monedaLocal;

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.dtFecha.Enabled = false;
            this.dtPeriod.Enabled = false;
            this.txtPrefix.Enabled = false;
            this.txtPrefix.Enabled = false; 
            this.txtNumeroDoc.Enabled = false;

            this.masterPrefijoNot.EnableControl(enable);
            this.masterPrefijoDoc.EnableControl(enable);
            this.masterCliente.EnableControl(enable);
            this.masterFacturaTipo.EnableControl(enable);
            this.masterLugarGeo.EnableControl(enable);
            this.masterAsesor.EnableControl(enable);
            this.masterMonedaFact.EnableControl(enable);
            //this.masterMonedaPago.EnableControl(enable);
            this.masterZona.EnableControl(enable);
            this.masterCentroCosto.EnableControl(enable);
            this.masterProyecto.EnableControl(enable);

            this.txtNotaNro.Enabled = enable;
            this.txtFacturaNro.Enabled = enable;
            this.dtFechaFact.Enabled = enable;
            //this.dtFechaVto.Enabled = enable;
            this.txtTasaCambio.Enabled = enable;
            //this.txtFormaPago.Enabled = enable;
            this.pceDescrDoc.Enabled = enable;
            this.pceEncabezado.Enabled = enable;
            this.txtPorcICA.Enabled = (!this.indContabilizaRetencionFactura) ? false : enable;
            this.txtPorcRemesa.Enabled = (!this.indContabilizaRetencionFactura) ? false : enable;
            //this.dtFechaProntoPago.Enabled = false;
            //this.txtPorcProntoPago.Enabled = false;
            //this.txtValorProntoPago.Enabled = false;
            this.pcePiePagina.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_faFacturacion LoadTempHeader()
        {
            if (string.IsNullOrWhiteSpace(this.txtFacturaNro.Text))
                this.txtFacturaNro.Text = "0";

            #region Load DocumentoControl
            this.ctrl.EmpresaID.Value = this.empresaID;
            this.ctrl.TerceroID.Value = this._tercero.ID.Value;
            this.ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this.ctrl.ComprobanteID.Value = this.comprobanteID;
            this.ctrl.ComprobanteIDNro.Value = 0;
            this.ctrl.MonedaID.Value = this.masterMonedaFact.Value;
            this.ctrl.CuentaID.Value = this.cuentaDoc.ID.Value;
            this.ctrl.ProyectoID.Value = this.masterProyecto.Value;
            this.ctrl.CentroCostoID.Value = this.masterCentroCosto.Value;
            this.ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
            this.ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
            this.ctrl.FechaDoc.Value = this.dtFechaFact.DateTime;
            this.ctrl.Fecha.Value = DateTime.Now;
            this.ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this.ctrl.PrefijoID.Value = this.masterPrefijoNot.Value;
            this.ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this.ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this.ctrl.DocumentoNro.Value = Convert.ToInt32(txtNotaNro.Text);
            this.ctrl.DocumentoID.Value = this.documentID;
            this.ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this.ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this.ctrl.seUsuarioID.Value = this.userID;
            this.ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this.ctrl.ConsSaldo.Value = 0;
            this.ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this.ctrl.Observacion.Value = this.rtbDescrDoc.Text;
            this.ctrl.Descripcion.Value = base.txtDocDesc.Text;
            #endregion
            #region Load FacturaHeader
            this.factHeader.EmpresaID.Value = this.empresaID;
            this.factHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this.factHeader.AsesorID.Value = this.masterAsesor.Value;
            this.factHeader.FacturaTipoID.Value = this.masterFacturaTipo.Value;
            //this.factHeader.MvtoTipoCarID.Value = this.facturaTipo.MvtoTipoNCID.Value;
            this.factHeader.DocumentoREL.Value = 0; // Convert.ToInt32(this.txtNotaNro.Text);
            this.factHeader.FacturaREL.Value = Convert.ToInt32(this.txtFacturaNro.Text);
            //this._factHeader.MonedaPago.Value = this.masterMonedaPago.Value;
            this.factHeader.ClienteID.Value = this.masterCliente.Value;
            this.factHeader.ListaPrecioID.Value = this.cliente.ListaPrecioID.Value;
            this.factHeader.ZonaID.Value = this.masterZona.Value;
            this.factHeader.TasaPago.Value = 1; // 0-FechaPago ; 1-Fecha Factura
            this.factHeader.ObservacionENC.Value = this.rtbEncabezado.Text;
            this.factHeader.ObservacionPIE.Value = this.rtbPiePagina.Text;
            this.factHeader.Valor.Value = 0;
            this.factHeader.Iva.Value = 0;
            this.factHeader.Porcentaje1.Value = Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture);
            this.factHeader.Porcentaje2.Value = Convert.ToDecimal(this.txtPorcRemesa.EditValue, CultureInfo.InvariantCulture);
            this.factHeader.FacturaFijaInd.Value = false;
            #endregion

            //this._porcICA = Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture);
            //this._porcRemesa = Convert.ToDecimal(this.txtPorcRemesa.EditValue, CultureInfo.InvariantCulture);
            this.tipoMonedaOr = this.ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            this.monedaId = this.ctrl.MonedaID.Value;

            DTO_faFacturacion fact = new DTO_faFacturacion();
            fact.Header = this.factHeader;
            fact.DocCtrl = this.ctrl;
            fact.Footer = new List<DTO_faFacturacionFooter>();
            this.factFooter = fact.Footer;

            return fact;
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = (int)this.tipoMonedaOr;

            //Asigna monedaId
            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            //Si la empresa no permite mmultimoneda
            if (!this.multiMoneda)
            {
                this.txtTasaCambio.EditValue = 0;
            }
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(monOr, this.dtFechaFact.DateTime);
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

            #region Valida que ya este asignada una tasa de cambio
            if (!this.AsignarTasaCambio(false))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                return false;
            }
            #endregion

            #region Valida datos en la maestra de Prefijo
            if (!this.masterPrefijoNot.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijoNot.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijoNot.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Cliente
            if (!this.masterCliente.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijoNot.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de FacturaTipo
            if (!this.masterFacturaTipo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterFacturaTipo.CodeRsx);

                MessageBox.Show(msg);
                this.masterFacturaTipo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Lugar Geografico
            if (!this.masterLugarGeo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarGeo.CodeRsx);

                MessageBox.Show(msg);
                this.masterLugarGeo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra del proyecto
            if (!this.masterProyecto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra del centro costo
            if (!this.masterCentroCosto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroCosto.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCosto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Asesor
            if (!this.masterAsesor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAsesor.CodeRsx);

                MessageBox.Show(msg);
                this.masterAsesor.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Moneda
            if (!this.masterMonedaFact.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaFact.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaFact.Focus();

                return false;
            }

            //if (!this.masterMonedaPago.ValidID)
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaPago.CodeRsx);

            //    MessageBox.Show(msg);
            //    this.masterMonedaPago.Focus();

            //    return false;
            //}
            #endregion

            #region Valida datos en la maestra de Zona
            if (!this.masterZona.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterZona.CodeRsx);

                MessageBox.Show(msg);
                this.masterZona.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la fecha de la factura
            if (string.IsNullOrEmpty(this.dtFechaFact.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_dtFechaFact");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaFact.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la fecha de vencimiento
            //if (string.IsNullOrEmpty(this.dtFechaVto.Text))
            //{
            //    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_dtFechaVto");
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
            //    MessageBox.Show(msg);

            //    this.dtFechaVto.Focus();
            //    return false;
            //}
            #endregion

            #region Valida datos en la forma de pago
            //if (string.IsNullOrEmpty(this.txtFormaPago.Text))
            //{
            //    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_txtFormaPago");
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
            //    MessageBox.Show(msg);

            //    this.txtFormaPago.Focus();
            //    return false;
            //}
            #endregion

            #region Valida datos en el descriptivo
            if (string.IsNullOrEmpty(this.rtbDescrDoc.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_rtbDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.pceDescrDoc.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la fecha de pronto pago
            //if (string.IsNullOrEmpty(this.dtFechaProntoPago.Text))
            //{
            //    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_dtFechaProntoPagot");
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
            //    MessageBox.Show(msg);

            //    this.dtFechaProntoPago.Focus();
            //    return false;
            //}
            #endregion

            #region Valida datos en el Porcentaje de pronto pago
            //if (string.IsNullOrEmpty(this.txtPorcProntoPago.Text))
            //{
            //    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_txtPorcProntoPagot");
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
            //    MessageBox.Show(msg);

            //    this.txtPorcProntoPago.Focus();
            //    return false;
            //}
            #endregion

            #region Valida datos en el valor de pronto pago
            //if (string.IsNullOrEmpty(this.txtValorProntoPago.Text))
            //{
            //    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_txtValorProntoPagot");
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
            //    MessageBox.Show(msg);

            //    this.txtValorProntoPago.Focus();
            //    return false;
            //}
            #endregion

            #endregion
            return true;
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave(int monOr)
        {
            decimal dif = Convert.ToDecimal(this.txtValorNeto.EditValue, CultureInfo.InvariantCulture);

            if (this._factValorNeto != 0 && dif > this._factValorNeto || dif<0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_InvalidValorNeto));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(DTO_faFacturacion fact)
        {
            DTO_glDocumentoControl ctrl = fact.DocCtrl;
            DTO_faFacturaDocu factHeader = fact.Header;

            if (fact.Footer == null)
                fact.Footer = new List<DTO_faFacturacionFooter>();
            this.factFooter = fact.Footer;

            #region Trae los datos del formulario dado la factura
            //Trae la info de faFacturaTipo
            DTO_faFacturaTipo faTipo = (DTO_faFacturaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.faFacturaTipo,false, factHeader.FacturaTipoID.Value,true);

            this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, faTipo.coDocumentoID.Value, true);
            this.prefijoID = ctrl.PrefijoID.Value;
            this.txtPrefix.Text = this.prefijoID;
            base.comprobanteID = this.coDocumento != null ? this.coDocumento.ComprobanteID.Value : string.Empty;
            #endregion

            this.txtFacturaNro.Text = string.Empty;
            this.masterPrefijoDoc.Value = string.Empty;
            this._factValorNeto = 0;

            if (!string.IsNullOrEmpty(factHeader.FacturaREL.Value.ToString()))
            {            
                DTO_faFacturacion Fact = _bc.AdministrationModel.FacturaVenta_Load(AppDocuments.FacturaVenta, ctrl.PrefijoID.Value, factHeader.FacturaREL.Value.Value);
                if (Fact != null)
                {
                    this.txtFacturaNro.Text = factHeader.FacturaREL.Value.Value.ToString();
                    this.masterPrefijoDoc.Value = ctrl.PrefijoID.Value;
                    this._factValorNeto = Fact.Footer.Sum(x=>x.ValorNeto);
                }             
            }          

            this.masterPrefijoNot.Value = ctrl.PrefijoID.Value;
            this.masterCliente.Value = factHeader.ClienteID.Value;
            this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
            this.masterFacturaTipo.Value = factHeader.FacturaTipoID.Value;
            //this.mvtoTipo = (DTO_faMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faMovimientoTipo, false, factHeader.MvtoTipoCarID.Value, true);

            this.masterFacturaTipo.Value = this.cliente.FacturaTipoID.Value;   //  !!!!!!!!!!!!!!!!!!!!!!!!
            this.masterAsesor.Value = factHeader.AsesorID.Value;
            this.masterLugarGeo.Value = ctrl.LugarGeograficoID.Value;
            this.masterMonedaFact.Value = ctrl.MonedaID.Value;
            //this.masterMonedaPago.Value = factHeader.MonedaPago.Value;
            this.masterZona.Value = factHeader.ZonaID.Value;
            this.masterProyecto.Value = ctrl.ProyectoID.Value;
            this.masterCentroCosto.Value = ctrl.CentroCostoID.Value;

            this.txtNotaNro.Text = ctrl.DocumentoNro.Value.Value.ToString();
            this.dtFechaFact.DateTime = ctrl.FechaDoc.Value.Value;
            //this.dtFechaVto.DateTime = factHeader.FechaVto.Value.Value;
            this.txtTasaCambio.EditValue = ctrl.TasaCambioCONT.Value.Value;
            //this.txtFormaPago.Text = factHeader.FormaPago.Value;
            this.rtbDescrDoc.Text = ctrl.Observacion.Value;
            this.pceDescrDoc.Text = this.rtbDescrDoc.Text;
            this.rtbEncabezado.Text = factHeader.ObservacionENC.Value;
            this.pceEncabezado.Text = this.rtbEncabezado.Text;
            this.txtPorcICA.EditValue = factHeader.Porcentaje1.Value.Value;
            this.txtPorcRemesa.EditValue = factHeader.Porcentaje2.Value.Value;
            //this.dtFechaProntoPago.DateTime = factHeader.FechaPtoPago.Value.Value;
            //this.txtPorcProntoPago.EditValue = factHeader.PorcPtoPago.Value.Value;
            //this.txtValorProntoPago.EditValue = factHeader.ValorPtoPago.Value.Value;
            this.rtbPiePagina.Text = factHeader.ObservacionPIE.Value;
            this.pcePiePagina.Text = this.rtbPiePagina.Text;

            //this._porcICA = factHeader.Porcentaje1.Value.Value;
            //this._porcRemesa = factHeader.Porcentaje2.Value.Value;
            this.monedaId = ctrl.MonedaID.Value;
            this.tipoMonedaOr = this.monedaId == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

            //this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
            this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();
            if (this.dtPeriod.DateTime.Month == DateTime.Now.Month)
                base.dtFecha.DateTime = DateTime.Now;
            else
                base.dtFecha.DateTime = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));

            //Si se presenta un problema asignando la tasa de cambio lo bloquea
            if (this.ValidateHeader())
            {
                this.EnableHeader(false);
                this.data = fact;
                this.ctrl = fact.DocCtrl;
                this.factHeader = fact.Header;
                this.factFooter = fact.Footer;

                this.validHeader = true;
                this.headerLoaded = true;

                this.LoadData(true);
                this.gcDocument.Focus();
            }
            else
                this.CleanHeader(true);
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected override void CalcularTotal()
        {
            try
            {
                base.CalcularTotal();

                if (this.data.Footer.Count != 0)
                {
                    this.data.Header.Valor.Value = this.data.Footer.Sum(x => x.ValorBruto);
                    this.data.Header.Iva.Value = this.data.Footer.Sum(x => x.ValorIVA);
                    //this.data.Header.ValorPtoPago.Value = this.data.Footer.Sum(x => x.ValorNeto) * this.data.Header.PorcPtoPago.Value.Value / 100;
                    this.data.Header.Retencion1.Value = this.data.Footer.Sum(x => x.ValorRIVA);
                    this.data.Header.Retencion2.Value = this.data.Footer.Sum(x => x.ValorRFT);
                    this.data.Header.Retencion3.Value = this.data.Footer.Sum(x => x.ValorRICA);
                    this.data.Header.Retencion4.Value = this.data.Header.ValorRteGarantia;
                    this.data.Header.Retencion5.Value = this.data.Footer.Sum(x => x.ValorOtros);

                    //this.txtValorProntoPago.EditValue = this.data.Header.ValorPtoPago.Value.Value;
                    this.txtPorcICA.EditValue = this.data.Header.Porcentaje1.Value.Value;
                    this.txtPorcRemesa.EditValue = this.data.Header.Porcentaje2.Value.Value;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoDoc_Enter(object sender, EventArgs e)
        {
            if (this.txtNotaNro.Text == "0" && (this.txtFacturaNro.Text == "0"||string.IsNullOrEmpty(this.txtFacturaNro.Text)))
                this._prefijoDocFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoDoc_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_prefijoDocFocus)
                {
                    _prefijoDocFocus = false;
                    if (this.masterPrefijoDoc.ValidID)
                    {
                        this.prefijoID = this.masterPrefijoDoc.Value;
                        this.txtPrefix.Text = this.prefijoID;
                        this.masterPrefijoNot.Value = string.Empty;
                        this.txtNotaNro.Text = "0";
                        this.masterPrefijoNot.EnableControl(false);
                        this.txtNotaNro.Enabled = false;
                    }
                    else
                    {
                        this.masterPrefijoDoc.Value = string.Empty;
                        this.masterPrefijoNot.EnableControl(true);
                        this.txtNotaNro.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterPrefijoDoc_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFacturaNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFacturaNro_Enter(object sender, EventArgs e)
        {
            this._txtFacturaNroFocus = true;
            if (!this.masterPrefijoDoc.ValidID)
            {
                this._txtFacturaNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijoDoc.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijoDoc.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFacturaNro_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._txtFacturaNroFocus)
                {
                    this._bc.InitMasterUC(this.masterFacturaTipo, AppMasters.faFacturaTipo, true, true, true, false);
                    this._txtFacturaNroFocus = false;
                    if (this.txtFacturaNro.Text == "0")
                        this.txtFacturaNro.Text = string.Empty;

                    #region No refiere a una factura
                    if (this.txtFacturaNro.Text == string.Empty)
                    {
                        this.gcDocument.DataSource = null;
                        //this.Comprobante = null;
                        this.data = null;
                        this.newDoc = true;
                    }
                    #endregion
                    #region Refiere a una factura
                    else
                    {
                        try
                        {
                            #region Trae Factura Relacionada y valida si existe
                            DTO_faFacturacion Fact = _bc.AdministrationModel.FacturaVenta_Load(AppDocuments.FacturaVenta, this.masterPrefijoDoc.Value, Convert.ToInt32(this.txtFacturaNro.Text));

                            if (Fact == null || Fact.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoFacturas));
                                this.txtFacturaNro.Focus();
                                this.validHeader = false;
                                return;
                            }
                            #endregion
                            this.masterPrefijoNot.Value = this.prefijoID;
                            this.newDoc = false;

                            #region Asigna variables para Nota Credito
                            //Trae informacion sobre el cliente
                            this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, Fact.Header.ClienteID.Value, true);

                            //Trae informacion sobre el tipo de la factura
                            this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, Fact.Header.FacturaTipoID.Value, true);
                            this._facturaTipoID = this.facturaTipo.ID.Value;

                            //Trae informacion sobre el Movimiento Tipo
                            //this.mvtoTipo = (DTO_faMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faMovimientoTipo, false, this.facturaTipo.MvtoTipoNCID.Value, true);
                            this._coDocumentoID = this.facturaTipo.coDocumentoID.Value;

                            //Trae informacion sobre el coDocumento
                            this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._coDocumentoID, true);
                            DTO_coComprobante compAnul = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, this.coDocumento.ComprobanteID.Value, true);
                            base.comprobanteID = this.coDocumento.PrefijoID.Value == this.masterPrefijoNot.Value ? compAnul.ComprobanteAnulID.Value : string.Empty;

                            //this.masterMonedaPago.Value = this.monedaLocal;
                            this.monedaId = Fact.DocCtrl.MonedaID.Value;
                            this.tipoMonedaOr = this.monedaId == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                            #endregion

                            if (this.GetCuenta(false))
                            {
                                #region Cambia los datos para Nota Credito
                                #region Cambia los datos del Header
                                Fact.Header.NumeroDoc.Value = 0;
                                Fact.Header.FacturaREL.Value = Fact.DocCtrl.DocumentoNro.Value.Value;
                                Fact.Header.ObservacionENC.Value = this.facturaTipo.ObservacionENC.Value;
                                Fact.Header.ObservacionPIE.Value = this.facturaTipo.ObservacionPIE.Value;
                                //Fact.Header.MvtoTipoCarID.Value = this.mvtoTipo.ID.Value;
                                #endregion
                                #region Cambia los datos del DocCtrl
                                Fact.DocCtrl.NumeroDoc.Value = 0;
                                Fact.DocCtrl.DocumentoNro.Value = 0;
                                Fact.DocCtrl.DocumentoID.Value = this.documentID;
                                Fact.DocCtrl.ComprobanteIDNro.Value = 0;
                                Fact.DocCtrl.ComprobanteID.Value = base.comprobanteID;
                                Fact.DocCtrl.CuentaID.Value = this.cuentaDoc.ID.Value;
                                Fact.DocCtrl.Observacion.Value = string.Empty;
                                Fact.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                                Fact.DocCtrl.Descripcion.Value = base.txtDocDesc.Text;
                                #endregion
                                #endregion

                                this.ctrl = Fact.DocCtrl;
                                this.factHeader = Fact.Header;

                                if (this.AsignarTasaCambio(false))
                                {
                                    #region Asigna los valores
                                    this.txtNumeroDoc.Text = this.ctrl.NumeroDoc.Value.Value.ToString();
                                    this.masterPrefijoNot.Value = this.ctrl.PrefijoID.Value;
                                    this.masterCliente.Value = this.factHeader.ClienteID.Value;
                                    this.masterFacturaTipo.Value = this.factHeader.FacturaTipoID.Value; ;
                                    this.masterAsesor.Value = this.factHeader.AsesorID.Value;
                                    this.masterLugarGeo.Value = this.ctrl.LugarGeograficoID.Value;
                                    this.masterMonedaFact.Value = this.ctrl.MonedaID.Value;
                                    //this.masterMonedaPago.Value = this._factHeader.MonedaPago.Value;
                                    this.masterZona.Value = this.factHeader.ZonaID.Value;
                                    this.masterProyecto.Value = this.ctrl.ProyectoID.Value;
                                    this.masterCentroCosto.Value = this.ctrl.CentroCostoID.Value;

                                    this.txtNotaNro.Text = this.ctrl.DocumentoNro.ToString();
                                    this.dtFechaFact.DateTime = this.ctrl.FechaDoc.Value.Value;
                                    //this.dtFechaVto.DateTime = this._factHeader.FechaVto.Value.Value;
                                    this.txtTasaCambio.EditValue = this.ctrl.TasaCambioCONT.Value.Value;
                                    //this.txtFormaPago.Text = this._factHeader.FormaPago.Value;
                                    this.rtbDescrDoc.Text = this.ctrl.Observacion.Value;
                                    this.pceDescrDoc.Text = this.rtbDescrDoc.Text;
                                    this.rtbEncabezado.Text = this.factHeader.ObservacionENC.Value;
                                    this.pceEncabezado.Text = this.rtbEncabezado.Text;
                                    this.txtPorcICA.EditValue = this.factHeader.Porcentaje1.Value.Value;
                                    this.txtPorcRemesa.EditValue = this.factHeader.Porcentaje2.Value.Value;
                                    //this.dtFechaProntoPago.DateTime = this._factHeader.FechaPtoPago.Value.Value;
                                    //this.txtPorcProntoPago.EditValue = this._factHeader.PorcPtoPago.Value.Value;
                                    //this.txtValorProntoPago.EditValue = this._factHeader.ValorPtoPago.Value.Value;
                                    this.rtbPiePagina.Text = this.factHeader.ObservacionPIE.Value;
                                    this.pcePiePagina.Text = this.rtbPiePagina.Text;

                                    //this._porcICA = Convert.ToDecimal(this.txtPorcICA.Text);
                                    //this._porcRemesa = Convert.ToDecimal(this.txtPorcRemesa.Text);
                                    this.tipoMonedaOr = this.ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                                    this.monedaId = this.ctrl.MonedaID.Value;
                                    this.headerLoaded = true;

                                    if (Fact.Footer != null)
                                    {
                                        _loadDataFactura = true;
                                        foreach (DTO_faFacturacionFooter footer in Fact.Footer)
                                            footer.Movimiento.NumeroDoc.Value = 0;
                                        this.factFooter = Fact.Footer;
                                        //this.LoadData(true);
                                        //this.gcDocument.Focus();
                                    }
                                    else
                                    {
                                        this.factFooter = new List<DTO_faFacturacionFooter>();
                                    }
                                    this.data = Fact;
                                    this._factValorNeto = Fact.Footer.Sum(x => x.ValorNeto);
                                    this.LoadData(true);
                                    this.EnableHeader(false);
                                    this.pceDescrDoc.Enabled = true;
                                    this.pceEncabezado.Enabled = true;
                                    this.pcePiePagina.Enabled = true;
                                    #endregion
                                }
                                else
                                    this.validHeader = false;
                            }
                            else
                                this.validHeader = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "txtFacturaNro_Leave"));
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "txtFacturaNro_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoNot_Enter(object sender, EventArgs e)
        {
            if (this.txtNotaNro.Text == "0" && (this.txtFacturaNro.Text == "0" || string.IsNullOrEmpty(this.txtFacturaNro.Text)))
            this._prefijoNotFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoNot_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_prefijoNotFocus)
                {
                    _prefijoNotFocus = false;
                    if (this.masterPrefijoNot.ValidID)
                    {
                        this.prefijoID = this.masterPrefijoNot.Value;
                        this.txtPrefix.Text = this.prefijoID;
                        this.masterPrefijoDoc.Value = string.Empty;
                        this.txtFacturaNro.Text = string.Empty;
                        this.masterPrefijoDoc.EnableControl(false);
                        this.txtFacturaNro.Enabled = false;
                        this.masterCliente.EnableControl(true);
                    }
                    else
                    {
                        this.masterPrefijoNot.Value = string.Empty;
                        this.masterPrefijoDoc.EnableControl(true);
                        this.txtFacturaNro.Enabled = true;
                        this.masterCliente.EnableControl(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterPrefijoNot_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNotaNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNotaNro_Enter(object sender, EventArgs e)
        {
            this._txtNotaNroFocus = true;
            if (!this.masterPrefijoNot.ValidID)
            {
                this._txtNotaNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijoNot.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijoNot.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNotaNro_Leave(object sender, EventArgs e)
        {
            if (this._txtNotaNroFocus)
            {
                this._bc.InitMasterUC(this.masterFacturaTipo, AppMasters.faFacturaTipo, true, true, true, false, this.filtrosfacturaTipo);
                this._txtNotaNroFocus = false;
                if (this.txtNotaNro.Text == string.Empty)
                    this.txtNotaNro.Text = "0";

                if (this.txtNotaNro.Text == "0")
                {
                    #region Nueva Factura
                    this.gcDocument.DataSource = null;
                    //this.Comprobante = null;
                    this.data = null;
                    this.newDoc = true;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_faFacturacion NotaCredito = _bc.AdministrationModel.FacturaVenta_Load(this.documentID, this.masterPrefijoNot.Value, Convert.ToInt32(this.txtNotaNro.Text));
                        //Valida si existe
                        if (NotaCredito == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoFacturas));
                            this.txtNotaNro.Focus();
                            this.validHeader = false;
                            return;
                        }

                        this.newDoc = false;

                        //Carga los datos
                        this.ctrl = NotaCredito.DocCtrl;
                        this.factHeader = NotaCredito.Header;

                        if (this.AsignarTasaCambio(false))
                        {
                            #region Asigna los valores
                            this.txtNumeroDoc.Text = this.ctrl.NumeroDoc.Value.Value.ToString();
                            this.masterPrefijoNot.Value = this.ctrl.PrefijoID.Value;
                            this.masterCliente.Value = this.factHeader.ClienteID.Value;
                            this.masterFacturaTipo.Value = this.factHeader.FacturaTipoID.Value; ;
                            this.masterAsesor.Value = this.factHeader.AsesorID.Value;
                            this.masterLugarGeo.Value = this.ctrl.LugarGeograficoID.Value;
                            this.masterMonedaFact.Value = this.ctrl.MonedaID.Value;
                            //this.masterMonedaPago.Value = this._factHeader.MonedaPago.Value;
                            this.masterZona.Value = this.factHeader.ZonaID.Value;
                            this.masterProyecto.Value = this.ctrl.ProyectoID.Value;
                            this.masterCentroCosto.Value = this.ctrl.CentroCostoID.Value;

                            this.txtNotaNro.Text = this.ctrl.DocumentoNro.Value.Value.ToString();
                            this.dtFechaFact.DateTime = this.ctrl.FechaDoc.Value.Value;
                            //this.dtFechaVto.DateTime = this._factHeader.FechaVto.Value.Value;
                            this.txtTasaCambio.EditValue = this.ctrl.TasaCambioCONT.Value.Value;
                            //this.txtFormaPago.Text = this._factHeader.FormaPago.Value;
                            this.rtbDescrDoc.Text = this.ctrl.Observacion.Value;
                            this.pceDescrDoc.Text = this.rtbDescrDoc.Text;
                            this.rtbEncabezado.Text = this.factHeader.ObservacionENC.Value;
                            this.pceEncabezado.Text = this.rtbEncabezado.Text;
                            this.txtPorcICA.EditValue = this.factHeader.Porcentaje1.Value.Value;
                            this.txtPorcRemesa.EditValue = this.factHeader.Porcentaje2.Value.Value;
                            //this.dtFechaProntoPago.DateTime = this._factHeader.FechaPtoPago.Value.Value;
                            //this.txtPorcProntoPago.EditValue = this._factHeader.PorcPtoPago.Value.Value;
                            //this.txtValorProntoPago.EditValue = this._factHeader.ValorPtoPago.Value.Value;
                            this.rtbPiePagina.Text = this.factHeader.ObservacionPIE.Value;
                            this.pcePiePagina.Text = this.rtbPiePagina.Text;

                            //this._porcICA = Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture);
                            //this._porcRemesa = Convert.ToDecimal(this.txtPorcRemesa.EditValue, CultureInfo.InvariantCulture);
                            this.tipoMonedaOr = this.ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                            this.monedaId = this.ctrl.MonedaID.Value;
                            this.headerLoaded = true;
                            
                            if (NotaCredito.Footer != null)
                            {
                                //if (this._factFooter.Footer != null &&this._factFooter.Footer.Count > 0)
                                //    this._factFooter.Footer.RemoveAt(this._factFooter.Footer.Count - 1);

                                //this.Comprobante.coDocumentoID = this.documentID.ToString();
                                //this.Comprobante.PrefijoID = this._ctrl.PrefijoID.Value;
                                //this.Comprobante.DocumentoNro = this._ctrl.DocumentoNro.Value.Value;
                                //this.Comprobante.CuentaID = this.masterCuenta_.Value;
                                //this.Comprobante.ProyectoID = this.masterProyecto.Value;
                                //this.Comprobante.CentroCostoID = this.masterCentroCosto.Value;
                                //this.Comprobante.LineaPresupuestoID = this.defLineaPresupuesto;
                                //this.Comprobante.LugarGeograficoID = this.masterLugarGeo.Value;
                                //this.Comprobante.Observacion = this.txtDescrDoc.Text;

                                //this._compNro = this.Comprobante.Header.ComprobanteNro.Value.Value;
                                //this._compLoaded = true;
                                this.factFooter = NotaCredito.Footer;
                                this.LoadData(true);
                                //if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                                //    this.CambiarSignoValor();

                                this.gcDocument.Focus();
                            }
                            else
                            {
                                this.factFooter = new List<DTO_faFacturacionFooter>();
                                //this.Comprobante = new DTO_Comprobante();
                                //this._compLoaded = false;
                            }

                            this.data = NotaCredito;
                            this.LoadData(true);

                            //// Cambia los valores de signos segun la cuenta
                            //if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito && CxP.Comp != null)
                            //{
                            //    this.LoadData(true);
                            //}

                            //if (CxP.Comp != null)
                            //    this.gcDocument.Focus();
                            //else
                            //    this.Comprobante = new DTO_Comprobante();
                            //}
                            //else
                            //    this.validHeader = false;
                            #endregion
                        }
                        else
                            this.validHeader = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "txtNotaNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterCliente control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID)
                {
                    this.txtNotaNro.Enabled = false;
                    this.newDoc = true;
                    if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                    {
                        //Trae informacion sobre el cliente
                        this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                        this.masterFacturaTipo.Value = this.cliente.FacturaTipoID.Value;

                        //Trae informacion sobre el tipo de la factura
                        this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);
                        this._facturaTipoID = this.masterFacturaTipo.Value;

                        //Trae informacion sobre el Movimiento Tipo
                        //this.mvtoTipo = (DTO_faMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faMovimientoTipo, false, this.facturaTipo.MvtoTipoNCID.Value, true);
                        this._coDocumentoID = this.facturaTipo.coDocumentoID.Value;

                        //Trae informacion sobre el coDocumento
                        this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._coDocumentoID, true);

                        //Informacion sobre las monedas por defecto
                        this.masterMonedaFact.Value = this.monedaLocal;
                        //this.masterMonedaPago.Value = this.monedaLocal;
                        this.monedaId = this.masterMonedaFact.Value;
                        this.tipoMonedaOr = TipoMoneda_LocExt.Local;

                        //Trae informacion sobre la Cuenta
                        if (this.GetCuenta(true))
                        {
                            base.comprobanteID = this.coDocumento != null ? this.coDocumento.ComprobanteID.Value : string.Empty;

                            this.rtbEncabezado.Text = this.facturaTipo.ObservacionENC.Value;
                            this.pceEncabezado.Text = this.rtbEncabezado.Text;
                            this.rtbPiePagina.Text = this.facturaTipo.ObservacionPIE.Value;
                            this.pcePiePagina.Text = this.rtbPiePagina.Text;

                            this.masterZona.Value = this.cliente.ZonaID.Value;
                            this._asesor = (DTO_faAsesor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faAsesor, false, this.masterAsesor.Value, true);

                            //Trae informacion sobre el Tercero
                            this._tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.cliente.TerceroID.Value, true);
                            this.masterLugarGeo.Value = this._tercero.LugarGeograficoID.Value;

                            this.EnableHeader(true);
                            this.txtFacturaNro.Enabled = false;
                            this.masterPrefijoDoc.EnableControl(false);
                            this.txtNotaNro.Enabled = false;
                            this.masterPrefijoNot.EnableControl(false);
                        }
                        else
                            this.validHeader = false;
                    }
                }
                else if (!string.IsNullOrEmpty(this.masterCliente.Value) || !string.IsNullOrWhiteSpace(this.masterCliente.Value))
                    this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterFacturaTipo control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterFacturaTipo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterFacturaTipo.ValidID)
                {
                    if (this.facturaTipo.ID.Value != this.masterFacturaTipo.Value)
                    {
                        //Trae informacion sobre el tipo de la factura
                        this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);

                        if (this._coDocumentoID == null || this._coDocumentoID != this.facturaTipo.coDocumentoID.Value)
                        {
                            //Trae informacion sobre el coDocumento
                            this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this.facturaTipo.coDocumentoID.Value, true);
                            if (this.GetCuenta(true))
                            {
                                base.comprobanteID = this.coDocumento != null ?  this.coDocumento.ComprobanteID.Value : string.Empty;
                                this._coDocumentoID = this.coDocumento.ID.Value;

                                this.rtbEncabezado.Text = this.facturaTipo.ObservacionENC.Value;
                                this.pceEncabezado.Text = this.rtbEncabezado.Text;
                                this.rtbPiePagina.Text = this.facturaTipo.ObservacionPIE.Value;
                                this.pcePiePagina.Text = this.rtbPiePagina.Text;
                            }
                            else
                            {
                                this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._coDocumentoID, true);
                                this.masterFacturaTipo.Value = this._facturaTipoID;

                                //Trae informacion sobre el tipo de la factura
                                this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);
                            }
                        }
                    }
                }
                else
                    this.masterFacturaTipo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterFacturaTipo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterAsesor control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterAsesor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterAsesor.ValidID)
                {
                    if (this._asesor == null || this._asesor.ID.Value != this.masterAsesor.Value)
                        this._asesor = (DTO_faAsesor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faAsesor, false, this.masterAsesor.Value, true);
                }
                else
                    this.masterAsesor.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterAsesor_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterLugarGeo control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterLugarGeo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!this.masterLugarGeo.ValidID)
                    this.masterLugarGeo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterLugarGeo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterCliente control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterMonedaFact_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterMonedaFact.ValidID)
                {
                    if (this.monedaId != this.masterMonedaFact.Value)
                    {
                        this.tipoMonedaOr = this.masterMonedaFact.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                        //Trae informacion sobre la Cuenta
                        if (this.GetCuenta(true))
                            this.monedaId = this.masterMonedaFact.Value;
                        else
                        {
                            this.masterMonedaFact.Value = this.monedaId;
                            this.tipoMonedaOr = this.masterMonedaFact.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                        }
                    }
                }
                else
                    this.masterMonedaFact.Value = this.monedaId;
                //this.masterMonedaFact.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterMonedaFact_Leave"));
            }
        }

        /// <summary>
        /// valida la edición de las Observaciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rtb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RichTextBox dt = (RichTextBox)sender;

                if (dt.Name.Contains("Encabezado"))
                    this.pceEncabezado.Text = this.rtbEncabezado.Text;
                else if (dt.Name.Contains("PiePagina"))
                    this.pcePiePagina.Text = this.rtbPiePagina.Text;
                else if (dt.Name.Contains("DescrDoc"))
                    this.pceDescrDoc.Text = this.rtbDescrDoc.Text;
            }
            catch (Exception)
            { ; }

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
            try
            {
                if (this.ValidateHeader())
                    this.validHeader = true;
                else
                    this.validHeader = false;

                //Si el diseño esta cargado y el header es valido
                if (this.validHeader)
                {
                    this.ValidHeaderTB();
                    if (this.txtNumeroDoc.Text == "0")
                    {
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                        FormProvider.Master.itemExport.Enabled = false;
                        FormProvider.Master.itemPrint.Enabled = false;
                    }

                    #region Si entra al detalle y no tiene datos
                    this.EnableHeader(false);
                    try
                    {
                        if (!this.headerLoaded)
                        {
                            DTO_faFacturacion fact = this.LoadTempHeader();
                            this.factHeader = fact.Header;
                            this.factFooter = fact.Footer;
                            this.TempData = fact;

                            this.LoadData(true);

                            this.UpdateTemp(this.data);
                            this.headerLoaded = true;
                        }
                        else if (this._loadDataFactura)
                        {
                            this.TempData = this.data;
                            this.data.DocCtrl.Observacion.Value = this.rtbDescrDoc.Text;
                            this.data.Header.ObservacionENC.Value = this.rtbEncabezado.Text;
                            this.data.Header.ObservacionPIE.Value = this.rtbPiePagina.Text;
                            this.LoadData(true);
                            this.UpdateTemp(this.data);

                            this._loadDataFactura = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "gcDocument_Enter" + ex.Message));
                    }
                    #endregion
                    #region Si ya tiene datos cargados
                    if (!this.dataLoaded)
                    {
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "gcDocument_Enter"));
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
                this.masterCliente.Focus();

            if (this.txtNumeroDoc.Text != "0")
            {
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
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
                    this.data = new DTO_faFacturacion();
                    this.ctrl = new DTO_glDocumentoControl();
                    this.factHeader = new DTO_faFacturaDocu();
                    this.factFooter = new List<DTO_faFacturacionFooter>();
                    

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;

                    this.CleanHeader(true);
                    this.EnableHeader(false);
                    this.masterPrefijoDoc.EnableControl(true);
                    this.masterPrefijoDoc.Focus();
                    this.masterPrefijoNot.EnableControl(true);
                    this.txtFacturaNro.Enabled = true;
                    this.txtNotaNro.Enabled = true;
                    
                    this.headerLoaded = false;
                    this._loadDataFactura = false;
                    this._factValorNeto = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

                int monOr = (int)this.tipoMonedaOr;
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();
                if (this.ValidGrid()&& this.CanSave(monOr))
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
