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
    /// Orden de Compra
    /// </summary>
    public partial class Contrato : DocumentOrdenCompForm
    {
        public Contrato()
        {
            //InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtContratoNro.Enabled = true;
            this.CleanFooter();
            this.EnableFooter(false);

            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtContratoNro.Enabled = true;
            this.CleanFooter();
            this.EnableFooter(false);

            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        private int _ordenCompraNro = 0;

        private DTO_glDocumentoControl _ctrl = null;
        private DTO_prContratoDocu _contratoHeader = null;
        private List<DTO_prOrdenCompraFooter> _contratoFooter = null;

        private bool _headerLoaded = false;

        private bool _prefijoFocus = false;
        private bool _txtOrdenCompraNroFocus = false;
        
        TipoMoneda _tipoMoneda = TipoMoneda.Local;
        private Dictionary<int, string> _tipoDoc;
        private Dictionary<int, string> _tipoOtrosi;

        private int _daysHighPriority = 0;
        private int _daysMediumPriority = 0;
        private int _daysLowPriority = 0;
        private bool _copyData = false;

        private List<DTO_prContratoPlanPago> _contratoPlanPagos;
        private List<DTO_prConvenio> _convenios;
        private List<DTO_prContratoPolizas> _polizas;
        private List<DTO_prOrdenCompraCotiza> _cotizaciones;
        //DTO_glConsultaFiltro filtroArea;
        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_prOrdenCompra TempData
        {
            get
            {
                return (DTO_prOrdenCompra)this.data;
            }
            set
            {
                this.data = value;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la Informacion del Combo de tipo Doc
        /// </summary>
        private void LookUpDataSource()
        {
            this._tipoDoc = new Dictionary<int, string>();
            this._tipoDoc.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoContrato));
            this._tipoDoc.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Otrosi));

            this._tipoOtrosi = new Dictionary<int, string>();
            this._tipoOtrosi.Add(1, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Adiciona));
            this._tipoOtrosi.Add(2, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Reemplaza));

            this.lookUpTipoDoc.Properties.ValueMember = "Key";
            this.lookUpTipoDoc.Properties.DisplayMember = "Value";
            this.lookUpTipoDoc.Properties.DataSource = this._tipoDoc;
            this.lookUpTipoDoc.EditValue = 1;

            this.lookUpTipoOtrosi.Properties.ValueMember = "Key";
            this.lookUpTipoOtrosi.Properties.DisplayMember = "Value";
            this.lookUpTipoOtrosi.Properties.DataSource = this._tipoOtrosi;
            this.lookUpTipoOtrosi.EditValue = 1;
        } 

        #endregion

        #region Funciones Virtuales
        
        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Contrato;
            this.frmModule = ModulesPrefix.pr;
            InitializeComponent();

            base.SetInitParameters();

            this.data = new DTO_prOrdenCompra();
            this._ctrl = new DTO_glDocumentoControl();
            this._contratoHeader = new DTO_prContratoDocu();
            this._contratoFooter = new List<DTO_prOrdenCompraFooter>();
            this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
            this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
            this._convenios = new List<DTO_prConvenio>();
            this._polizas = new List<DTO_prContratoPolizas>();
            try
            {
                List<DTO_glConsultaFiltro> filtrosLugarRecibido = new List<DTO_glConsultaFiltro>();
                filtrosLugarRecibido.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "RecibidosInd",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "1"
                });

                _bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                _bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false);
                _bc.InitMasterUC(this.masterMonedaPago, AppMasters.glMoneda, false, true, true, false);
                _bc.InitMasterUC(this.masterMonedaOrden, AppMasters.glMoneda, false, true, true, false);
                _bc.InitMasterUC(this.masterLugarEntr, AppMasters.glLocFisica, true, true, true, false, filtrosLugarRecibido);
                _bc.InitMasterUC(this.masterAreaAprob, AppMasters.glAreaFuncional, true, true, true, false);

                //Llena los combos
                TablesResources.GetTableResources(this.cmbPrioridad, typeof(Prioridad));
                this.cmbPrioridad.SelectedIndexChanged += new EventHandler(cmbPrioridad_SelectedIndexChanged);

                //Trae datos de glControl
                string diasPermPriAlta = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadAlta);
                string diasPermPriMed = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadMedia);
                string diasPermPriBaj = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadBaja);
                this._daysHighPriority = !string.IsNullOrEmpty(diasPermPriAlta) ? Convert.ToInt32(diasPermPriAlta) : 0;
                this._daysMediumPriority = !string.IsNullOrEmpty(diasPermPriMed) ? Convert.ToInt32(diasPermPriMed) : 0;
                this._daysLowPriority = !string.IsNullOrEmpty(diasPermPriBaj) ? Convert.ToInt32(diasPermPriBaj) : 0;

                this.tlSeparatorPanel.RowStyles[0].Height = 44;
                this.tlSeparatorPanel.RowStyles[1].Height = 65;
                this.tlSeparatorPanel.RowStyles[2].Height = 119;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-SetInitParameters"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.EnableFooter(false);
            this.EnableHeader(false);

            if (!_headerLoaded)
            {
                this.txtNumeroDoc.Text = "0";
                this.masterPrefijo.EnableControl(true);
                this.txtContratoNro.Enabled = true;
                if (string.IsNullOrEmpty(this.txtContratoNro.Text)) this.txtContratoNro.Text = "0";
                this.txtEstado.Text = ((int)EstadoDocControl.SinAprobar).ToString();
                this.lblCotiza.Text = "0";
                this.cmbPrioridad.SelectedIndex = 0;
                _daysEntr = _daysHighPriority;
                this.masterPrefijo.Value = base.prefijoID;
                this.masterAreaAprob.Value = base.areaFuncionalID;
            }

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.lblTasaOC.Visible = false;
                this.txtTasaOC.Visible = false;
            }
            this.LookUpDataSource();
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
                this.dtPeriod.Text = periodo;
                this.prefijoID = string.Empty;
                this.masterPrefijo.Value = this.txtPrefix.Text;
                this.txtNumeroDoc.Text = "0";
                this._ordenCompraNro = 0;
                this.txtContratoNro.Text = "0";
            }
            #region Controles de maestras
            this.masterProveedor.Value = string.Empty;
            this.masterMonedaPago.Value = string.Empty;
            this.masterMonedaOrden.Value = string.Empty;
            this.masterAreaAprob.Value = this.areaFuncionalID;
            this.masterLugarEntr.Value = string.Empty;
            #endregion
            #region Controles de texto
            this.txtContratoNro.Text = string.Empty;
            this.rtbObservaciones.Text = string.Empty;
            this.pceObservaciones.Text = string.Empty;
            this.rtbInstrucciones.Text = string.Empty;
            this.pceInstrucciones.Text = string.Empty;
            this.rtbFormaPago.Text = string.Empty;
            this.pceFormaPago.Text = string.Empty;
            this.txtEstado.Text = string.Empty;
            this.lblCotiza.Text = "0";
            #endregion            
            #region Controles numericos
            this.txtValorPagoMes.EditValue = 0;
            this.txtTotPlanPagos.EditValue = 0;
            this.txtNroPagos.EditValue = 0;
            this.txtTasaOC.EditValue = "0";
            this.txtPorcPrPago.EditValue = "0";
            this.txtDiasProntoPago.EditValue = 0;
            this.txtAnticipo.EditValue = "0";
            this.txtRetGarantias.EditValue = "0";
            this.txtPorcAdministra.EditValue = "0";
            this.txtPorcHolgura.EditValue = "0";
            this.txtPorcImprevistos.EditValue = "0";
            this.txtPorcUtilidad.EditValue = "0";
            this.txtValorDoc.EditValue = "0";
            this.txtValorIVADoc.EditValue = "0";
            #endregion
            #region Otros controles
            this.cbTerminosCond.Checked = false;
            this.cbAiuIncCosto.Checked = false;
            this.cmbPrioridad.SelectedIndex = 0;
            this.dtFechaPago1.DateTime = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;
            this.lookUpTipoDoc.EditValue = 1;
            this.lookUpTipoOtrosi.EditValue = 1;
            this.chkPagoVariableInd.Checked = false;
            #endregion
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
            this.txtNumeroDoc.Enabled = false;

            this.masterPrefijo.EnableControl(enable);
            //this.masterProveedor.EnableControl(enable);
            this.masterMonedaPago.EnableControl(enable);
           // this.masterMonedaOrden.EnableControl(enable);
            this.masterAreaAprob.EnableControl(enable);
            //this.masterLugarEntr.EnableControl(enable);

            this.txtContratoNro.Enabled = enable;
            this.txtTasaOC.Enabled = enable;
            this.pceObservaciones.Enabled = enable;
            this.pceInstrucciones.Enabled = enable;
            this.pceFormaPago.Enabled = enable;

            this.txtEstado.Enabled = false;
            this.cbTerminosCond.Enabled = enable;
            //this.btnCotizacion.Enabled = enable;
            this.btnConvenio.Enabled = enable;
            this.btnPolizas.Enabled = enable;
            this.btnEjecucion.Enabled = enable;

            this.cmbPrioridad.Enabled = enable;
            this.txtAnticipo.Enabled = enable;
            this.txtRetGarantias.Enabled = enable;
            this.txtDiasProntoPago.Enabled = enable;
            this.txtPorcPrPago.Enabled = enable;

            this.txtPorcAdministra.Enabled = enable;
            this.txtPorcHolgura.Enabled = enable;
            this.txtPorcImprevistos.Enabled = enable;
            this.txtPorcUtilidad.Enabled = enable;
            this.cbAiuIncCosto.Enabled = enable;

            this.chkPagoVariableInd.Enabled = enable;
            this.chkPagoVariableInd.Checked = enable;
            this.dtFechaPago1.Enabled = enable;
            this.dtFechaVencimiento.Enabled = enable;
            this.lookUpTipoDoc.Enabled = enable;
            this.lookUpTipoOtrosi.Enabled = enable;

            this.txtValorPagoMes.Enabled = enable;
            this.txtNroPagos.Enabled = enable;
            //this.btnQueryDoc.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override object LoadTempHeader()
        {
            #region Load DocumentoControl
            DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
            this._ctrl.EmpresaID.Value = this.empresaID;
            this._ctrl.TerceroID.Value = prov != null? prov.TerceroID.Value : this.defTercero;
            this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._ctrl.ComprobanteID.Value = string.Empty; 
            this._ctrl.ComprobanteIDNro.Value = 0; 
            this._ctrl.MonedaID.Value = this.masterMonedaOrden.Value; 
            this._ctrl.CuentaID.Value = string.Empty; 
            this._ctrl.ProyectoID.Value = this.defProyecto; 
            this._ctrl.CentroCostoID.Value = this.defCentroCosto; 
            this._ctrl.LugarGeograficoID.Value = this.defLugarGeo; 
            this._ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
            this._ctrl.Fecha.Value = DateTime.Now;
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._ctrl.PrefijoID.Value = this.masterPrefijo.Value;
            this._ctrl.TasaCambioCONT.Value = 0;
            this._ctrl.TasaCambioDOCU.Value = 0;
            this._ctrl.DocumentoNro.Value = Convert.ToInt32(txtContratoNro.Text);
            this._ctrl.DocumentoID.Value = this.documentID;
            this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.seUsuarioID.Value = this.userID;
            this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this._ctrl.ConsSaldo.Value = 0;
            this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
            this._ctrl.Valor.Value = 0;
            this._ctrl.Iva.Value = 0;

            #endregion
            #region Load OrdenCompraHeader
            this._contratoHeader.EmpresaID.Value = this.empresaID;
            this._contratoHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._contratoHeader.OrdenCompraNro.Value = Convert.ToInt32(this.txtContratoNro.Text);
            this._contratoHeader.TipoDocumento.Value = Convert.ToByte(this.lookUpTipoDoc.EditValue);
            this._contratoHeader.TipoOtroSi.Value = Convert.ToByte(this.lookUpTipoOtrosi.EditValue);
            this._contratoHeader.PagoVariableInd.Value = this.chkPagoVariableInd.Checked;
            this._contratoHeader.VlrPagoMes.Value = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.NroPagos.Value = Convert.ToByte(this.txtNroPagos.EditValue);
            this._contratoHeader.FechaPago1.Value = this.dtFechaPago1.DateTime;
            this._contratoHeader.FechaVencimiento.Value = this.dtFechaVencimiento.DateTime;
            this._contratoHeader.FormaPago.Value = this.rtbFormaPago.Text;
            this._contratoHeader.IncluyeAUICosto.Value = this.cbAiuIncCosto.Checked;
            this._contratoHeader.Instrucciones.Value = this.rtbInstrucciones.Text;
            this._contratoHeader.MonedaOrden.Value = this.masterMonedaOrden.Value;
            this._contratoHeader.MonedaPago.Value = this.masterMonedaPago.Value;
            this._contratoHeader.LugarEntrega.Value = this.masterLugarEntr.Value;
            this._contratoHeader.Observaciones.Value = this.rtbObservaciones.Text;
            this._contratoHeader.PorcentAdministra.Value = Convert.ToDecimal(this.txtPorcAdministra.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.PorcentHolgura.Value = Convert.ToDecimal(this.txtPorcHolgura.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.Porcentimprevisto.Value = Convert.ToDecimal(this.txtPorcImprevistos.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.PorcentUtilidad.Value = Convert.ToDecimal(this.txtPorcUtilidad.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.ProveedorID.Value = this.masterProveedor.Value;
            this._contratoHeader.VlrAnticipo.Value = Convert.ToDecimal(this.txtAnticipo.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.RteGarantiaPor.Value = Convert.ToDecimal(this.txtRetGarantias.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.TasaOrden.Value = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.TerminosInd.Value = this.cbTerminosCond.Checked;
            this._contratoHeader.AreaAprobacion.Value = this.masterAreaAprob.Value;
            this._contratoHeader.DtoProntoPago.Value = Convert.ToDecimal(this.txtDiasProntoPago.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.Prioridad.Value = Convert.ToByte((((ComboBoxItem)(this.cmbPrioridad.SelectedItem)).Value));
            this._contratoHeader.DiasPtoPago.Value = Convert.ToByte(this.txtDiasProntoPago.EditValue);
            this._contratoHeader.Valor.Value = Convert.ToDecimal(this.txtValorDoc.EditValue, CultureInfo.InvariantCulture);
            this._contratoHeader.IVA.Value = Convert.ToDecimal(this.txtValorIVADoc.EditValue, CultureInfo.InvariantCulture);
            #endregion

            if (this._contratoPlanPagos == null) this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
            if (this._cotizaciones == null) this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();

            this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda.Local : TipoMoneda.Foreign;
            this.monedaId = this._ctrl.MonedaID.Value;

            DTO_prOrdenCompra orden = new DTO_prOrdenCompra();
            orden.HeaderContrato = this._contratoHeader;
            orden.DocCtrl = this._ctrl;
            orden.Convenio = this._convenios;
            orden.Polizas = this._polizas;
            orden.Cotizacion = this._cotizaciones;
            orden.ContratoPlanPagos = this._contratoPlanPagos;
            orden.Footer = new List<DTO_prOrdenCompraFooter>();
            this._contratoFooter = orden.Footer;

            return orden;
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = (int)this._tipoMonedaOr;
            this._tipoMoneda = (TipoMoneda)monOr;

            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            this.masterMonedaOrden.Value = this.monedaId;
            //Sio la empresa no permite mmultimoneda
            if (!this.multiMoneda)
                this.txtTasaOC.EditValue = "0";
            else
            {
                this.txtTasaOC.EditValue = this.LoadTasaCambio(monOr, this.dtFecha.DateTime);
                decimal tc = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
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
            if (this.multiMoneda && !this.AsignarTasaCambio(false))
                return false;
            #endregion

            #region Valida datos en la maestra de Prefijo
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Proveedor
            if (!this.masterProveedor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProveedor.CodeRsx);

                MessageBox.Show(msg);
                this.masterProveedor.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de MonedaOrden
            if (!this.masterMonedaOrden.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaOrden.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaOrden.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de MonedaPago
            if (!this.masterMonedaPago.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaPago.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaPago.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de LugarRecibido
            if (!this.masterLugarEntr.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarEntr.CodeRsx);

                MessageBox.Show(msg);
                this.masterLugarEntr.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Area Aprobacion
            if (!this.masterAreaAprob.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAreaAprob.CodeRsx);

                MessageBox.Show(msg);
                this.masterAreaAprob.Focus();

                return false;
            }
            #endregion

            #region Valida datos de FormaPago 
            if (string.IsNullOrEmpty(this.pceFormaPago.Text.Trim()))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFormaPago");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.pceFormaPago.Focus();
                return false;
            }
            #endregion

            #region Valida datos de Observaciones
            if (string.IsNullOrEmpty(this.pceObservaciones.Text.Trim()))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblObservaciones");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.pceObservaciones.Focus();
                return false;
            }
            #endregion

            #region Valida datos de Instrucciones
            if (string.IsNullOrEmpty(this.pceInstrucciones.Text.Trim()))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblInstrucciones");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.pceInstrucciones.Focus();
                return false;
            }
            #endregion

            #region Valida datos de Cotizaciones
            if ((string.IsNullOrEmpty(this.txtNumeroDoc.Text.Trim()) || Convert.ToInt32(this.txtNumeroDoc.Text) == 0)
                && (this._cotizaciones == null || this._cotizaciones.Count == 0))
            {
                string txtRsx = this.btnCotizacion.Text;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
              
                MessageBox.Show(msg);
                this.btnCotizacion.Focus();
                return false;
            }
            #endregion
            #endregion
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(object c)
        {
            DTO_prOrdenCompra contrato = (DTO_prOrdenCompra)c;

            #region Asigna variables de data
            this._ctrl = contrato.DocCtrl;
            this._contratoHeader = contrato.HeaderContrato;
            this._contratoPlanPagos = contrato.ContratoPlanPagos == null ? contrato.ContratoPlanPagos = new List<DTO_prContratoPlanPago>() : contrato.ContratoPlanPagos;
            this._cotizaciones = contrato.Cotizacion == null ? contrato.Cotizacion = new List<DTO_prOrdenCompraCotiza>() : contrato.Cotizacion;
            this._convenios = contrato.Convenio == null ? contrato.Convenio = new List<DTO_prConvenio>() : contrato.Convenio;
            this._polizas = contrato.Polizas == null ? contrato.Polizas = new List<DTO_prContratoPolizas>() : contrato.Polizas;
            #endregion
            #region Controles  Iniciales
            this.dtPeriod.DateTime = this._ctrl.PeriodoDoc.Value.Value;
            this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
            this.dtFecha.DateTime = this._ctrl.FechaDoc.Value.Value;
            #endregion
            #region Controles de maestras
            this.masterPrefijo.Value = this._ctrl.PrefijoID.Value;
            this.masterProveedor.Value = this._contratoHeader.ProveedorID.Value;
            this.masterMonedaOrden.Value = this._contratoHeader.MonedaOrden.Value;
            this.masterMonedaPago.Value = this._contratoHeader.MonedaPago.Value;
            this.masterAreaAprob.Value = this._contratoHeader.AreaAprobacion.Value;
            this.masterLugarEntr.Value = this._contratoHeader.LugarEntrega.Value;
            #endregion
            #region Controles varios
            this.lookUpTipoDoc.EditValue = this._contratoHeader.TipoDocumento.Value;
            this.lookUpTipoOtrosi.EditValue = this._contratoHeader.TipoOtroSi.Value;
            this.txtContratoNro.Text = this._ctrl.DocumentoNro.Value.Value.ToString();
            this.dtFecha.DateTime = this._ctrl.FechaDoc.Value.Value;
            this.txtAnticipo.EditValue = this._contratoHeader.VlrAnticipo.Value.Value;
            this.txtRetGarantias.EditValue = this._contratoHeader.RteGarantiaPor.Value.Value;
            this.txtDiasProntoPago.EditValue = this._contratoHeader.DiasPtoPago.Value.Value;
            this.txtPorcPrPago.EditValue = this._contratoHeader.DtoProntoPago.Value.Value;
            this.rtbObservaciones.Text = this._contratoHeader.Observaciones.Value;
            this.pceObservaciones.Text = this.rtbObservaciones.Text;
            this.rtbInstrucciones.Text = this._contratoHeader.Instrucciones.Value;
            this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
            this.rtbFormaPago.Text = this._contratoHeader.FormaPago.Value;
            this.pceFormaPago.Text = this.rtbFormaPago.Text;          
            this.txtEstado.Text = this._ctrl.Estado.Value.Value.ToString();
            this.cbTerminosCond.Checked = this._contratoHeader.TerminosInd.Value.Value;
            this.chkPagoVariableInd.Checked = this._contratoHeader.PagoVariableInd.Value != null ? this._contratoHeader.PagoVariableInd.Value.Value : false;
            this.txtNroPagos.EditValue = this._contratoHeader.NroPagos.Value != null ? this._contratoHeader.NroPagos.Value.Value : 0;
            this.dtFechaPago1.DateTime = this._contratoHeader.FechaPago1.Value != null ? this._contratoHeader.FechaPago1.Value.Value : this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this._contratoHeader.FechaVencimiento.Value != null ? this._contratoHeader.FechaVencimiento.Value.Value : this.dtFecha.DateTime;
            #endregion
            #region Asigna dias de prioridad
            if (this._contratoHeader.Prioridad.Value != null)
	        {
		        Prioridad prior = (Prioridad)this._contratoHeader.Prioridad.Value.Value;
                string priorString = ((int)prior).ToString();
                this.cmbPrioridad.SelectedItem = this.cmbPrioridad.GetItem(priorString);
                this._daysEntr = (prior == Prioridad.High) ? this._daysHighPriority : (prior == Prioridad.Medium) ? this._daysMediumPriority : this._daysLowPriority;
	        }           
            #endregion
            #region Controles de valores
            this.txtPorcAdministra.EditValue = this._contratoHeader.PorcentAdministra.Value.Value;
            this.txtPorcHolgura.EditValue = this._contratoHeader.PorcentHolgura.Value.Value;
            this.txtPorcImprevistos.EditValue = this._contratoHeader.Porcentimprevisto.Value.Value;
            this.txtPorcUtilidad.EditValue = this._contratoHeader.PorcentUtilidad.Value.Value;
            this.cbAiuIncCosto.Checked = this._contratoHeader.IncluyeAUICosto.Value.Value;
            this.txtTasaOC.EditValue = this._contratoHeader.TasaOrden.Value.Value;
            this.txtValorDoc.EditValue = this._contratoHeader.Valor.Value.Value;
            this.txtValorIVADoc.EditValue = this._contratoHeader.IVA.Value.Value;
            this.txtValorPagoMes.EditValue = this._contratoHeader.VlrPagoMes.Value != null ?this._contratoHeader.VlrPagoMes.Value.Value: 0;
            this.txtTotPlanPagos.EditValue = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtNroPagos.EditValue,CultureInfo.InvariantCulture);
            this.lblCotiza.Text = this._cotizaciones.Count.ToString(); 
            #endregion
            #region Asigna valores de monedas
            this.monedaId = this._ctrl.MonedaID.Value;
            this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda.Local : TipoMoneda.Foreign;          
            #endregion

            //Si se presenta un problema asignando la tasa de cambio lo bloquea
            if (this.ValidateHeader())
            {
                this.EnableHeader(false);
                this.data = contrato;
                this.validHeader = true;
                this._headerLoaded = true;
                if (contrato.Footer != null)
                {
                    this._contratoFooter = contrato.Footer;
                    this.LoadData(true);
                    this.gcDocument.Focus();
                }
                else
                    this._contratoFooter = new List<DTO_prOrdenCompraFooter>();

                #region Carga Solicitudes
                this._solicitudes = new List<DTO_prSolicitudResumen>();
                DTO_prSolicitudResumen solRes;
                foreach (DTO_prOrdenCompraFooter item in this._contratoFooter)
                {
                    if (!this._solicitudes.Exists(x => x.ConsecutivoDetaID.Value == item.DetalleDocu.SolicitudDetaID.Value))
                    {
                        if (this._contratoFooter.Where(x => x.DetalleDocu.SolicitudDetaID.Value == item.DetalleDocu.SolicitudDetaID.Value).Count() == 1 || 
                            this._contratoFooter.Where(x => x.DetalleDocu.SolicitudDetaID.Value == item.DetalleDocu.SolicitudDetaID.Value).Count() > 1 &&
                            (!string.IsNullOrEmpty(item.DetalleDocu.CodigoAdminAIU.Value)||!string.IsNullOrEmpty(item.DetalleDocu.CodigoImprevAIU.Value)||
                            !string.IsNullOrEmpty(item.DetalleDocu.CodigoUtilidadAIU.Value)))
                        {
                            solRes = new DTO_prSolicitudResumen();
                            solRes.NumeroDoc.Value = item.DetalleDocu.SolicitudDocuID.Value;
                            solRes.ConsecutivoDetaID.Value = item.DetalleDocu.SolicitudDetaID.Value;
                            solRes.SolicitudDocuID.Value = item.DetalleDocu.SolicitudDocuID.Value;
                            solRes.CodigoBSID.Value = item.DetalleDocu.CodigoBSID.Value;
                            solRes.inReferenciaID.Value = item.DetalleDocu.inReferenciaID.Value;
                            solRes.Parametro1.Value = item.DetalleDocu.Parametro1.Value;
                            solRes.Parametro2.Value = item.DetalleDocu.Parametro2.Value;
                            solRes.DocumentoID.Value = AppDocuments.Solicitud;
                            solRes.CantidadOrdenComp.Value = item.DetalleDocu.CantidadCont.Value;
                            solRes.Descriptivo.Value = item.DetalleDocu.Descriptivo.Value;
                            solRes.UnidadInvID.Value = item.DetalleDocu.UnidadInvID.Value;
                            solRes.ValorUni.Value = item.DetalleDocu.ValorUni.Value.Value;
                            solRes.Selected.Value = true;
                            solRes.SolicitudCargos = item.SolicitudCargos;

                            this._solicitudes.Add(solRes);
                        }
                    }
                }


                #endregion
            }
            else
                this.CleanHeader(true);

            this.valorTotalDoc = contrato.DocCtrl.Valor.Value.Value;
            this.valorIVATotalDoc = contrato.DocCtrl.Iva.Value.Value;
        }

        /// <summary>
        /// Calcula valores 
        /// </summary>
        protected override void GetValuesDocument (int index,bool isIndividual,bool isDescuento)
        {
            base.valorTotalDoc = 0;
            base.valorIVATotalDoc = 0;
            try
            {
                #region Trae y calcula valor total e IVA por cada registro
                decimal porcIVA = (this.data.Footer[index].DetalleDocu.PorcentajeIVA.Value.Value / 100);
                decimal tasaCambio = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
                decimal valorUnit = this.data.Footer[index].DetalleDocu.ValorUni.Value.Value;
                decimal cantidadContrato = this.data.Footer[index].DetalleDocu.CantidadCont.Value.Value;

                this.data.Footer[index].DetalleDocu.IVAUni.Value =  valorUnit * porcIVA;
                this.data.Footer[index].DetalleDocu.IvaTotML.Value =Math.Round( (valorUnit * cantidadContrato) * porcIVA,0);
                this.data.Footer[index].DetalleDocu.ValorTotML.Value = Math.Round((valorUnit * cantidadContrato) + this.data.Footer[index].DetalleDocu.IVAUni.Value.Value,0); 
                #endregion
                #region Calcula Monedas
                if (this.multiMoneda && tasaCambio != 0)
                {
                    this.data.Footer[index].DetalleDocu.IvaTotME.Value = ((valorUnit * cantidadContrato) * porcIVA) / tasaCambio;
                    this.data.Footer[index].DetalleDocu.ValorTotME.Value = ((valorUnit * cantidadContrato) / tasaCambio) + (this.data.Footer[index].DetalleDocu.IVAUni.Value / tasaCambio);
                } 
                #endregion
                #region Calcula totales documento
                this.txtValorIVAUnit.EditValue = this.data.Footer[index].DetalleDocu.IvaTotML.Value.Value;
                this.txtValorTotal.EditValue = this.data.Footer[index].DetalleDocu.ValorTotML.Value.Value;

                foreach (var footer in base.data.Footer)
                {
                    if (base.data.HeaderContrato.MonedaOrden.Value == this.monedaLocal)
                    {
                        base.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
                        base.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
                    }
                    else
                    {
                        base.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
                        base.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
                    }
                }
                base.data.HeaderContrato.Valor.Value = base.valorTotalDoc;
                base.data.HeaderContrato.IVA.Value = base.valorIVATotalDoc;
                this.txtValorDoc.EditValue = base.valorTotalDoc;
                this.txtValorIVADoc.EditValue = base.valorIVATotalDoc;
             
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-GetValuesDocument"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Enter(object sender, EventArgs e)
        {
            this._prefijoFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_prefijoFocus)
                {
                    _prefijoFocus = false;
                    if (this.masterPrefijo.ValidID)
                    {
                        this.prefijoID = this.masterPrefijo.Value;
                        //this.txtPrefix.Text = this.prefijoID;
                    }
                    else                   
                        CleanHeader(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-masterPrefijo_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContratoNro_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtContratoNro_Enter(object sender, EventArgs e)
        {           
            this._txtOrdenCompraNroFocus = true;
            if (!this.masterPrefijo.ValidID)
            {
                this._txtOrdenCompraNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtContratoNro_Leave(object sender, EventArgs e)
        {
            if (this._txtOrdenCompraNroFocus)
            {
                _txtOrdenCompraNroFocus = false;
                if (this.txtContratoNro.Text == string.Empty)
                    this.txtContratoNro.Text = "0";

                if (this.txtContratoNro.Text == "0")
                {
                    #region Nuevo orden de compra
                    this.gcDocument.DataSource = null;
                    this.data = new DTO_prOrdenCompra();
                    this.newDoc = true;

                    this.EnableHeader(true);
                    this.masterPrefijo.EnableControl(false);
                    this.txtContratoNro.Enabled = false;

                    this.masterProveedor.Value = string.Empty;
                    this.txtContratoNro.Text = string.Empty;
                    this.masterLugarEntr.Value = string.Empty;
                    this.masterAreaAprob.Value = base.areaFuncionalID;
                    this.masterMonedaOrden.Value = this.monedaLocal;
                    this.masterMonedaPago.Value = this.monedaLocal; 
                    this.txtEstado.Text = ((int)EstadoDocControl.SinAprobar).ToString();
                    this.lblCotiza.Text = "0";
                    base.dtFecha.Enabled = true;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_prOrdenCompra Contrato = this._bc.AdministrationModel.OrdenCompra_Load(this.documentID, this.masterPrefijo.Value, Convert.ToInt32(this.txtContratoNro.Text));
                        //Valida si existe
                        if (Contrato == null)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode);
                            MessageBox.Show(string.Format(msg, this.Text));
                            this.txtContratoNro.Focus();
                            this.validHeader = false;
                            return;
                        }
                        if (this._copyData)
                        {
                            Contrato.DocCtrl.NumeroDoc.Value = 0;
                            Contrato.DocCtrl.DocumentoNro.Value = 0;
                            Contrato.HeaderContrato.NumeroDoc.Value = 0;
                            Contrato.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            this._copyData = false;  
                        }
                        this.newDoc = false;

                        //Carga toda la info del documento existente
                        this.LoadTempData(Contrato);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-txtContratoNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// valida la edición de las RichTextEdit
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RichTextBox dt = (RichTextBox)sender;

                if (dt.Name.Contains("Observaciones"))
                    this.pceObservaciones.Text = this.rtbObservaciones.Text;
                else if (dt.Name.Contains("Instrucciones"))
                    this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
                else if (dt.Name.Contains("FormaPago"))
                    this.pceFormaPago.Text = this.rtbFormaPago.Text;
            }
            catch (Exception)
            { ; }

        }

        /// <summary>
        /// valida al digitar teclas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void richTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                RichTextBox dt = (RichTextBox)sender;
                if (dt.Name.Contains("FormaPago"))
                    this.pceFormaPago.Text = this.rtbFormaPago.Text;
                else if (dt.Name.Contains("Instrucciones"))
                    this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
                else if (dt.Name.Contains("Observaciones"))
                    this.pceObservaciones.Text = this.rtbObservaciones.Text;
            }
            catch (Exception)
            { ; }
        }

        /// <summary>
        /// valida al digitar teclas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                RichTextBox dt = (RichTextBox)sender;
                if (dt.Name.Contains("FormaPago"))
                    this.pceFormaPago.Text = this.rtbFormaPago.Text;
                else if (dt.Name.Contains("Instrucciones"))
                    this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
                else if (dt.Name.Contains("Observaciones"))
                    this.pceObservaciones.Text = this.rtbObservaciones.Text;
                if (e.KeyValue == 27 || e.KeyValue == 9)
                {
                    if (dt.Name.Contains("FormaPago"))
                        this.pceInstrucciones.Focus();
                    else if (dt.Name.Contains("Instrucciones"))
                        this.pceObservaciones.Focus();
                    else if (dt.Name.Contains("Observaciones"))
                        this.cmbPrioridad.Focus();
                }
            }
            catch (Exception)
            { ; }
        }

        /// <summary>
        /// valida al digitar teclas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void richTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                RichTextBox dt = (RichTextBox)sender;
                if (dt.Name.Contains("FormaPago"))
                    this.pceFormaPago.Text = this.rtbFormaPago.Text;
                else if (dt.Name.Contains("Instrucciones"))
                    this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
                else if (dt.Name.Contains("Observaciones"))
                    this.pceObservaciones.Text = this.rtbObservaciones.Text;
                if (e.KeyValue == 27 || e.KeyValue == 9)
                {
                    if (dt.Name.Contains("FormaPago"))
                        this.pceInstrucciones.Focus();
                    else if (dt.Name.Contains("Instrucciones"))
                        this.pceObservaciones.Focus();
                    else if (dt.Name.Contains("Observaciones"))
                        this.cmbPrioridad.Focus();
                }
            }
            catch (Exception)
            { ; }
        }

        /// <summary>
        /// Trae el formulario auxiliar de los sotizaciones
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCotizacion_Click(object sender, EventArgs e)
        {
            ModalCotizaOC cot = new ModalCotizaOC(this._cotizaciones, Convert.ToInt32(this.txtNumeroDoc.Text), this.dtFecha.DateTime);
            cot.ShowDialog();
            if (cot.ReturnVals)
            {
                this._cotizaciones = cot.ReturnList;
                this.lblCotiza.Text = this._cotizaciones.Count.ToString();
            }
        }

        /// <summary>
        /// Abre la pantalla de Convenios
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnConvenio_Click(object sender, EventArgs e)
        {
            ModalConvenioProveedor convenios = new ModalConvenioProveedor(this._convenios, Convert.ToInt32(this.txtNumeroDoc.Text));
            convenios.ShowDialog();
            if (convenios.ReturnVals)
                this._convenios = convenios.ReturnList;
        }

        /// <summary>
        /// Se realiza al cambia el valor del combo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpTipoDoc_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.lookUpTipoDoc.EditValue) == 2)
            {
                this.lookUpTipoOtrosi.Visible = true;
                this.lblTipoOtroSi.Visible = true;
            }
            else
            {
                this.lookUpTipoOtrosi.Visible = false;
                this.lblTipoOtroSi.Visible = false;
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtValorPagoMes_Leave(object sender, EventArgs e)
        {
            TextEdit ctrl = (TextEdit)sender;
            try
            {
                if (this.chkPagoVariableInd.Checked)
                {
                    switch (ctrl.Name)
                    {
                        case "txtValorPagoMes":
                            #region txtValorPagoMes
                            this.data.HeaderContrato.VlrPagoMes.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                            this.data.HeaderContrato.NroPagos.Value = Convert.ToByte(this.txtNroPagos.EditValue);
                            #endregion
                            break;
                        case "txtNroPagos":
                            #region txtNroPagos
                            this.data.HeaderContrato.NroPagos.Value = Convert.ToByte(ctrl.EditValue);
                            this.data.HeaderContrato.VlrPagoMes.Value = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture);
                            #endregion
                            break;
                    }
                    DateTime fecha1erPago = this.dtFechaPago1.DateTime;
                    this._contratoPlanPagos.Clear();
                    for (int i = 0; i < this.data.HeaderContrato.NroPagos.Value; i++)
                    {
                        DTO_prContratoPlanPago pagos = new DTO_prContratoPlanPago();
                        pagos.EmpresaID.Value = this.empresaID;
                        pagos.Fecha.Value = fecha1erPago.AddMonths(i);
                        pagos.Observacion.Value = "Pago " + (i == 0 ? 1 : (i + 1)).ToString();
                        pagos.Valor.Value = this.data.HeaderContrato.VlrPagoMes.Value;
                        pagos.ValorAdicional.Value = 0; 
                        this._contratoPlanPagos.Add(pagos);
                    }
                    this.dtFechaVencimiento.DateTime = this._contratoPlanPagos.Count > 0 ? this._contratoPlanPagos.Last().Fecha.Value.Value : fecha1erPago;
                    this.valorTotalPagosMes = this._contratoPlanPagos.Sum(x => x.Valor.Value.Value + x.ValorAdicional.Value.Value);
                    this.txtTotPlanPagos.EditValue = this.valorTotalPagosMes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contrato.cs", "txtValorPagoMes_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha1Pago_Leave(object sender, EventArgs e)
        {
            try
            {
                this.txtValorPagoMes_Leave(this.txtNroPagos, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contrato.cs", "dtFecha1Pago_Leave"));
            }
        }

        /// <summary>
        /// valida la edición de las fechas segun nivel de prioridad
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbPrioridad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Prioridad)Enum.Parse(typeof(Prioridad), (this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.High)
                this._daysEntr = this._daysHighPriority;
            if ((Prioridad)Enum.Parse(typeof(Prioridad), (this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.Medium)
                this._daysEntr = this._daysMediumPriority;
            if ((Prioridad)Enum.Parse(typeof(Prioridad), (this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.Low)
                this._daysEntr = this._daysLowPriority;
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.Contrato);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this.txtContratoNro.Enabled = true;
                this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtContratoNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.txtContratoNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Consulta de Polizas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnPolizas_Click(object sender, EventArgs e)
        {
            ModalPolizaProveedor convenios = new ModalPolizaProveedor(this._polizas, Convert.ToInt32(this.txtNumeroDoc.Text));
            convenios.ShowDialog();
            if (convenios.ReturnVals)
                this._polizas = convenios.ReturnList;
        }

        /// <summary>
        /// Evento que se ejecuta al salir control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_Leave(object sender, EventArgs e)
        {
            try
            {
                this.AsignarTasaCambio(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Contrato.cs", "dtFecha_Leave"));
            }
        }

        /// <summary>
        /// Evento que se valida el plan de pagos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkPagoVariableInd_CheckedChanged(object sender, EventArgs e)
        {
            this.txtValorPagoMes.Enabled = this.chkPagoVariableInd.Checked;
            this.txtNroPagos.Enabled = this.chkPagoVariableInd.Checked;
            this.dtFechaPago1.Enabled = this.chkPagoVariableInd.Checked;
            this.dtFechaVencimiento.Enabled = this.chkPagoVariableInd.Checked;
            this.btnEjecucion.Enabled = this.chkPagoVariableInd.Checked;
        }

        /// <summary>
        /// Abre una modal de plan de pagos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnEjecucion_Click(object sender, EventArgs e)
        {
            ModalPlanPagos planPagos = new ModalPlanPagos(this._contratoPlanPagos, Convert.ToInt32(this.txtNumeroDoc.Text), this.dtFecha.DateTime, this.valorTotalDoc, this.chkPagoVariableInd.Checked);
            planPagos.ShowDialog();
            if (planPagos.ReturnVals)
                this._contratoPlanPagos = planPagos.ReturnList;
            this.valorTotalPagosMes = this._contratoPlanPagos.Sum(x => x.Valor.Value.Value + x.ValorAdicional.Value.Value);
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
                if (this.txtNumeroDoc.Text == "0")
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                this.EnableFooter(true);
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_prOrdenCompra orden = (DTO_prOrdenCompra)this.LoadTempHeader();
                        this._contratoHeader = orden.HeaderContrato;
                        this._contratoFooter = orden.Footer;
                        this.TempData = orden;

                        this.data = this.data == null ? this.data = new DTO_prOrdenCompra() : this.data;                 
                        this.LoadData(true);

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-grlDocument_Enter" + ex.Message));
                }
                #endregion
                #region Si ya tiene datos cargados
                if (!this.dataLoaded)
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                this.chkPagoVariableInd.Enabled = true;
                #endregion
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
                this.masterPrefijo.Focus();

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
                    this.data = new DTO_prOrdenCompra();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._contratoHeader = new DTO_prContratoDocu();
                    this._contratoFooter = new List<DTO_prOrdenCompraFooter>();
                    this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                    this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
                    this._convenios = new List<DTO_prConvenio>();
                    this._polizas = new List<DTO_prContratoPolizas>();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;

                    this._prefijoFocus = false;
                    this._txtOrdenCompraNroFocus = false;
                    this.CleanHeader(true);
                    this.EnableHeader(false);
                    this.masterPrefijo.EnableControl(true);
                    this.masterPrefijo.Focus();
                    this.txtContratoNro.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this._headerLoaded = false;
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
                if (this.CanSave())
                {
                    base.TBSave();
                    this.gvDocument.PostEditor();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    if (this.ValidGrid())
                    {
                        var det = ObjectCopier.Clone(this._contratoFooter);
                        this.data = (DTO_prOrdenCompra)this.LoadTempHeader();
                        this.data.Footer = det;
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                bool update = false;
                if (this._ctrl.NumeroDoc.Value.Value != 0)
                {
                    numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                    update = true;
                };
                DTO_SerializedObject result = this._bc.AdministrationModel.OrdenCompra_Guardar(this.documentID, this.data, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prOrdenCompra();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._contratoHeader = new DTO_prContratoDocu();
                    this._contratoFooter = new List<DTO_prOrdenCompraFooter>();
                    this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                    this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
                    this._convenios = new List<DTO_prConvenio>();
                    this._polizas = new List<DTO_prContratoPolizas>();
                    this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Contrato.cs-SaveThread"));
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
                int numeroDoc = this.data.HeaderContrato.NumeroDoc.Value.Value;
                if (numeroDoc == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                    return;
                }

                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;
                if (this._ctrl == null)
                    result = resultNOK;
                else
                    result = _bc.AdministrationModel.Solicitud_SendToAprob(this.documentID, this._ctrl.NumeroDoc.Value.Value, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prOrdenCompra();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._contratoHeader = new DTO_prContratoDocu();
                    this._contratoFooter = new List<DTO_prOrdenCompraFooter>();
                    this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                    this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
                    this._convenios = new List<DTO_prConvenio>();
                    this._polizas = new List<DTO_prContratoPolizas>();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "OrdeonCompra.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion        
    }
}
