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
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Orden de Compra
    /// </summary>
    public partial class OrdenCompra : DocumentOrdenCompForm
    {
        public OrdenCompra()
        {
         // InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
          
            FormProvider.Master.itemSave.Enabled = true;
            FormProvider.Master.itemSendtoAppr.Enabled = true;
            string docNro = this._ctrl.DocumentoNro.Value.ToString();
            string prefijo = this._ctrl.PrefijoID.Value;

            this.validHeader = false;
            this.TBNew();
            this.txtOrdenCompraNro.Text = docNro;
            this.masterPrefijo.Value = prefijo;
            this._txtOrdenCompraNroFocus = true;
            this.LoadDocument();
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
            this.txtOrdenCompraNro.Enabled = true;
            this.btnAprRech.Enabled = false;
            this.CleanFooter();
            this.EnableFooter(false);
            this.btnQueryDoc.Enabled = true;
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        private DTO_glDocumentoControl _ctrl = null;
        private DTO_prOrdenCompraDocu _ordenHeader = null;
        private List<DTO_prOrdenCompraFooter> _ordenFooter = null;

        private bool _headerLoaded = false;

        private bool _prefijoFocus = false;
        private bool _txtOrdenCompraNroFocus = false;
        
        TipoMoneda _tipoMoneda = TipoMoneda.Local;
        private int _daysHighPriority = 0;
        private int _daysMediumPriority = 0;
        private int _daysLowPriority = 0;
        private bool _copyData = false;

        public List<DTO_prOrdenCompraCotiza> _cotizaciones;
        private List<DTO_prContratoPlanPago> _contratoPlanPagos;
        private List<DTO_prContratoPolizas> _polizas;
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
        
        #region Funciones Virtuales
        
        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.OrdenCompra;
            this.frmModule = ModulesPrefix.pr;
            InitializeComponent();

            base.SetInitParameters();

            this.data = new DTO_prOrdenCompra();
            this._ctrl = new DTO_glDocumentoControl();
            this._ordenHeader = new DTO_prOrdenCompraDocu();
            this._ordenFooter = new List<DTO_prOrdenCompraFooter>();
            this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
            this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
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

                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                _bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false);
                _bc.InitMasterUC(this.masterLugarEntr, AppMasters.glLocFisica, false, true, true, false, filtrosLugarRecibido);
                _bc.InitMasterUC(this.masterMonedaPago, AppMasters.glMoneda, false, true, true, false);
                _bc.InitMasterUC(this.masterMonedaOrden, AppMasters.glMoneda, false, true, true, false);
                _bc.InitMasterUC(this.masterAreaAprob, AppMasters.glAreaFuncional, true, true, true, false);

                //Llena los combos
                TablesResources.GetTableResources(this.cmbPrioridad, typeof(Prioridad));
                this.cmbPrioridad.SelectedIndexChanged += new EventHandler(cmbPrioridad_SelectedIndexChanged);

                //Combo Reporte
                Dictionary<string, string> datosReporte = new Dictionary<string, string>();
                datosReporte.Add("1", "Orden de Compra");
                datosReporte.Add("2", "Orden de Compra Anexo");
                datosReporte.Add("3", "Orden de Servicio");
                this.cmbReporte.Properties.ValueMember = "Key";
                this.cmbReporte.Properties.DisplayMember = "Value";
                this.cmbReporte.Properties.DataSource = datosReporte;
                this.cmbReporte.EditValue = "1";

                //Trae datos de glControl
                string diasPermPriAlta = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadAlta);
                string diasPermPriMed = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadMedia);
                string diasPermPriBaj = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadBaja);
                this._daysHighPriority = !string.IsNullOrEmpty(diasPermPriAlta) ? Convert.ToInt32(diasPermPriAlta) : 0;
                this._daysMediumPriority = !string.IsNullOrEmpty(diasPermPriMed) ? Convert.ToInt32(diasPermPriMed) : 0;
                this._daysLowPriority = !string.IsNullOrEmpty(diasPermPriBaj) ? Convert.ToInt32(diasPermPriBaj) : 0;

                this.tlSeparatorPanel.RowStyles[0].Height = 50;
                this.tlSeparatorPanel.RowStyles[1].Height = 60;
                this.tlSeparatorPanel.RowStyles[2].Height = 119;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "OrdenCompra.cs-SetInitParameters"));
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
                this.txtOrdenCompraNro.Enabled = true;
                this.btnAprRech.Enabled = false;
                if (string.IsNullOrEmpty(this.txtOrdenCompraNro.Text)) 
                    this.txtOrdenCompraNro.Text = "0";
                this.txtEstado.Text = ((int)EstadoDocControl.SinAprobar).ToString();
                this.cmbPrioridad.SelectedIndex = 0;
                this._daysEntr = _daysHighPriority;
                this.lblCotiza.Text = "0";
                this.dtFechaOC.DateTime = DateTime.Now;
                this.dtFechaEntrega.DateTime = DateTime.Now;
                this.masterPrefijo.Value = base.prefijoID;
                this.masterAreaAprob.Value = base.areaFuncionalID;
            }
            base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
            //base.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;                        
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
                this.txtNumeroDoc.Text = "0";
                this.masterPrefijo.Value = base.prefijoID;
                this.txtOrdenCompraNro.Text = "0";
            }

            #region Controles Maestras
            this.masterProveedor.Value = string.Empty;
            this.masterLugarEntr.Value = string.Empty;
            this.masterMonedaPago.Value = string.Empty;
            this.masterMonedaOrden.Value = string.Empty;
            this.masterAreaAprob.Value = this.areaFuncionalID; 
            #endregion
            #region Controles de texto  
            this.txtContrato.Text = string.Empty;
            this.rtbObservaciones.Text = string.Empty;
            this.pceObservaciones.Text = string.Empty;
            this.rtbInstrucciones.Text = string.Empty;
            this.pceInstrucciones.Text = string.Empty;
            this.rtbFormaPago.Text = string.Empty;
            this.pceFormaPago.Text = string.Empty;
            this.txtEstado.Text = string.Empty;
            this.txtDirEntr.Text = string.Empty;
            this.txtTelefEntr.Text = string.Empty;
            this.txtEncargadoEntr.Text = string.Empty;
            this.lblCotiza.Text = "0";
            this.txtContacto.Text = string.Empty;
            #endregion
            #region Controles Numericos
            this.txtValorPagoMes.EditValue = 0;
            this.txtTotPlanPagos.EditValue = 0;
            this.txtNroPagos.EditValue = 0;
            this.txtDiasProntoPago.EditValue = 0;
            this.txtPorcPrPago.EditValue = "0";
            this.txtAnticipo.EditValue = 0;
            this.txtTasaOC.EditValue = "0";
            this.txtPorcAdministra.EditValue = "0";
            this.txtPorcHolgura.EditValue = "0";
            this.txtPorcImprevistos.EditValue = "0";
            this.txtPorcUtilidad.EditValue = "0";
            this.txtValorDoc.EditValue = "0";
            this.txtValorIVADoc.EditValue = "0";
            #endregion  
            #region Otros controles
            this.dtFechaOC.DateTime = DateTime.Now.Date;
            this.dtFechaEntrega.DateTime = DateTime.Now.Date;
            this.cbAiuIncCosto.Checked = false;
            this.cbTerminosCond.Checked = false;
            this.cmbPrioridad.SelectedIndex = 0;
            this.dtFechaPago1.DateTime = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;
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
            //this.masterLugarEntr.EnableControl(enable);
            this.masterMonedaPago.EnableControl(enable);
            //this.masterMonedaOrden.EnableControl(enable);
            this.masterAreaAprob.EnableControl(enable);

            this.txtOrdenCompraNro.Enabled = enable;
            this.dtFechaOC.Enabled = true;
            this.cmbPrioridad.Enabled = enable;
            this.txtContrato.Enabled = enable;
            this.txtAnticipo.Enabled = enable;
            //this.txtTasaOC.Enabled = enable;
            this.txtDiasProntoPago.Enabled = enable;
            //this.txtPorcPrPago.Enabled = enable;
            //this.pceObservaciones.Enabled = enable;
            //this.pceInstrucciones.Enabled = enable;
            //this.pceFormaPago.Enabled = enable;

            this.txtEstado.Enabled = false;
            this.cbTerminosCond.Enabled = enable;
            this.btnAprRech.Enabled = !enable;            
            //this.btnCotizacion.Enabled = enable;
            this.btnPolizas.Enabled = enable;
            this.btnEjecucion.Enabled = enable;

            this.dtFechaPago1.Enabled = enable;
            this.dtFechaVencimiento.Enabled = enable;
            this.chkPagoVariableInd.Enabled = enable;
            this.chkPagoVariableInd.Checked = enable;
            this.txtValorPagoMes.Enabled = enable;
            this.txtNroPagos.Enabled = enable;
            this.txtPorcAdministra.Enabled = enable;
            this.txtPorcHolgura.Enabled = enable;
            this.txtPorcImprevistos.Enabled = enable;
            this.txtPorcUtilidad.Enabled = enable;
            this.cbAiuIncCosto.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override object LoadTempHeader()
        {
            try
            {
                #region Load DocumentoControl
                if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value == 0)
                {
                    //Asigna valores cuando es nuevo docuemnto
                    this._ctrl.EmpresaID.Value = this.empresaID;
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
                    this._ctrl.DocumentoNro.Value = Convert.ToInt32(this.txtOrdenCompraNro.Text);
                    this._ctrl.DocumentoID.Value = this.documentID;
                    this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                    this._ctrl.seUsuarioID.Value = this.userID;
                    this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._ctrl.ConsSaldo.Value = 0;
                    this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    this._ctrl.FechaDoc.Value = this.dtFechaOC.DateTime;
                    this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
                    this._ctrl.Valor.Value = 0;
                    this._ctrl.Iva.Value = 0;
                    DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                    this._ctrl.TerceroID.Value = prov != null ? prov.TerceroID.Value : this.defTercero;
                }
                else
                {
                    //Asigna valores cuando ya existe la Orden Compra
                    this._ctrl.MonedaID.Value = this.masterMonedaOrden.Value;
                    this._ctrl.Fecha.Value = DateTime.Now;
                    this._ctrl.seUsuarioID.Value = this.userID;
                    this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    this._ctrl.FechaDoc.Value = this.dtFechaOC.DateTime;
                    DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                    this._ctrl.TerceroID.Value = prov != null ? prov.TerceroID.Value : this.defTercero;
                }
                
                #endregion
                #region Load OrdenCompraHeader
                
                this._ordenHeader.EmpresaID.Value = this.empresaID;
                if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value == 0)
                    this._ordenHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ordenHeader.ContratoNro.Value = Convert.ToInt32(this.txtContrato.Text);
                this._ordenHeader.FormaPago.Value = this.rtbFormaPago.Text;
                this._ordenHeader.IncluyeAUICosto.Value = this.cbAiuIncCosto.Checked;
                this._ordenHeader.Instrucciones.Value = this.rtbInstrucciones.Text;
                this._ordenHeader.LugarEntrega.Value = this.masterLugarEntr.Value;
                this._ordenHeader.FechaEntrega.Value = this.dtFechaEntrega.DateTime;
                this._ordenHeader.Encargado.Value = this.txtEncargadoEntr.Text;
                this._ordenHeader.DireccionEntrega.Value = this.txtDirEntr.Text;
                this._ordenHeader.TelefonoEntrega.Value = this.txtTelefEntr.Text;
                this._ordenHeader.MonedaOrden.Value = this.masterMonedaOrden.Value;
                this._ordenHeader.MonedaPago.Value = this.masterMonedaPago.Value;
                this._ordenHeader.Observaciones.Value = this.rtbObservaciones.Text;
                this._ordenHeader.PagoVariablend.Value = this.chkPagoVariableInd.Checked;
                this._ordenHeader.VlrPagoMes.Value = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.NroPagos.Value = Convert.ToByte(this.txtNroPagos.EditValue);
                this._ordenHeader.FechaPago1.Value = this.dtFechaPago1.DateTime;
                this._ordenHeader.FechaVencimiento.Value = this.dtFechaVencimiento.DateTime;
                this._ordenHeader.PorcentAdministra.Value = Convert.ToDecimal(this.txtPorcAdministra.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.PorcentHolgura.Value = Convert.ToDecimal(this.txtPorcHolgura.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.Porcentimprevisto.Value = Convert.ToDecimal(this.txtPorcImprevistos.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.PorcentUtilidad.Value = Convert.ToDecimal(this.txtPorcUtilidad.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.ProveedorID.Value = this.masterProveedor.Value;
                this._ordenHeader.VlrAnticipo.Value = Convert.ToDecimal(this.txtAnticipo.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.TasaOrden.Value = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.TerminosInd.Value = this.cbTerminosCond.Checked;
                this._ordenHeader.AreaAprobacion.Value = this.masterAreaAprob.Value;
                this._ordenHeader.DtoProntoPago.Value = Convert.ToDecimal(this.txtPorcPrPago.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.Prioridad.Value = Convert.ToByte((((ComboBoxItem)(this.cmbPrioridad.SelectedItem)).Value));
                this._ordenHeader.DiasPtoPago.Value = Convert.ToByte(this.txtDiasProntoPago.EditValue);
                this._ordenHeader.Valor.Value = Convert.ToDecimal(this.txtValorDoc.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.IVA.Value = Convert.ToDecimal(this.txtValorIVADoc.EditValue, CultureInfo.InvariantCulture);
                this._ordenHeader.ContactoComercial.Value = this.txtContacto.Text;
                this._ordenHeader.TelefonoEntrega.Value = this.txtTelefEntr.Text;
                this._ordenHeader.DireccionEntrega.Value = this.txtDirEntr.Text;
                this._ordenHeader.Encargado.Value = this.txtEncargadoEntr.Text;

                     if (this.cmbIncoTerm.Properties.DataSource != null)
                    this._ordenHeader.Inconterm.Value = Convert.ToByte(this.cmbIncoTerm.EditValue);
                #endregion

                if (this._cotizaciones == null) this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                if (this._contratoPlanPagos == null) this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();

                this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda.Local : TipoMoneda.Foreign;
                this.monedaId = this._ctrl.MonedaID.Value;

                DTO_prOrdenCompra orden = new DTO_prOrdenCompra();
                orden.HeaderOrdenCompra = this._ordenHeader;
                orden.DocCtrl = this._ctrl;
                orden.Cotizacion = this._cotizaciones;
                orden.Polizas = this._polizas;
                orden.ContratoPlanPagos = this._contratoPlanPagos;
                orden.Footer = new List<DTO_prOrdenCompraFooter>();
                this._ordenFooter = orden.Footer;

                return orden;
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "LoadTempHeader"));
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
                int monOr = (int)this._tipoMonedaOr;
                this._tipoMoneda = (TipoMoneda)monOr;

                if (monOr == (int)TipoMoneda.Local)
                    this.monedaId = this.monedaLocal;
                else
                    this.monedaId = this.monedaExtranjera;

                this.masterMonedaOrden.Value = this.monedaId;
                this._vlrTasaCambio = this.LoadTasaCambio(monOr, this.dtFechaOC.DateTime);
                this.txtTasaOC.EditValue = this._vlrTasaCambio;

                if (!fromTop)
                    this.validHeader = true;
                else
                    this.validHeader = false;
            }
            catch (Exception ex)
            {                
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            try
            {
                #region Valida los datos obligatorios

                #region Valida datos en la maestra de Prefijo
                if (!this.masterPrefijo.ValidID)
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                    MessageBox.Show(msg);
                    this.masterPrefijo.Focus();

                    return false;
                }
                #endregion

                #region Valida datos en la maestra de Preveedor
                if (!this.masterProveedor.ValidID)
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProveedor.CodeRsx);

                    MessageBox.Show(msg);
                    this.masterProveedor.Focus();

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
                else
                    this.monedaOrden = this.masterMonedaOrden.Value;
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

                #region Valida datos en la fecha del orden
                if (string.IsNullOrEmpty(this.dtFechaOC.Text))
                {
                    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaOC");
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                    MessageBox.Show(msg);

                    this.dtFechaOC.Focus();
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
                    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblCotiza");
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                    MessageBox.Show(msg);
                    this.btnCotizacion.Focus();
                    return false;
                }
                #endregion
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "ValidateHeader"));
                 return false;
            }
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(object oc)
        {
            try
            {
                DTO_prOrdenCompra orden = (DTO_prOrdenCompra)oc;

                #region Asigna Variables de data
                this._ctrl = orden.DocCtrl;
                this._ordenHeader = orden.HeaderOrdenCompra;

                if (orden.Cotizacion == null)
                    orden.Cotizacion = new List<DTO_prOrdenCompraCotiza>();
                this._cotizaciones = orden.Cotizacion;
                this._contratoPlanPagos = orden.ContratoPlanPagos == null ? orden.ContratoPlanPagos = new List<DTO_prContratoPlanPago>() : orden.ContratoPlanPagos;
                this._polizas = orden.Polizas == null ? orden.Polizas = new List<DTO_prContratoPolizas>() : orden.Polizas;
                #endregion
                #region Asigna controles de maestras
                this.masterPrefijo.Value = this._ctrl.PrefijoID.Value;
                this.masterProveedor.Value = this._ordenHeader.ProveedorID.Value;                
                this.masterMonedaOrden.Value = this._ordenHeader.MonedaOrden.Value;
                this.masterMonedaPago.Value = this._ordenHeader.MonedaPago.Value;
                this.masterAreaAprob.Value = this._ordenHeader.AreaAprobacion.Value;
                this.masterLugarEntr.Value = this._ordenHeader.LugarEntrega.Value;
                #endregion
                #region Asigna controles varios
                this.txtContacto.EditValue = this._ordenHeader.ContactoComercial.Value;
                this.txtContrato.Text = this._ordenHeader.ContratoNro.Value.ToString();
                this.txtOrdenCompraNro.Text = this._ctrl.DocumentoNro.Value.ToString();
                this.dtFechaOC.DateTime = this._ctrl.FechaDoc.Value.Value;
                this.dtFechaEntrega.DateTime = this._ordenHeader.FechaEntrega.Value != null ? this._ordenHeader.FechaEntrega.Value.Value : this.dtFechaEntrega.DateTime;
                this.txtAnticipo.EditValue = this._ordenHeader.VlrAnticipo.Value.Value;
                this.txtDiasProntoPago.EditValue = this._ordenHeader.DiasPtoPago.Value.Value;
                this.txtEncargadoEntr.Text = this._ordenHeader.Encargado.Value;
                this.txtDirEntr.Text = this._ordenHeader.DireccionEntrega.Value;
                this.txtTelefEntr.Text = this._ordenHeader.TelefonoEntrega.Value;
                this.txtPorcPrPago.EditValue = this._ordenHeader.DtoProntoPago.Value.Value;
                this.rtbObservaciones.Text = this._ordenHeader.Observaciones.Value;
                this.pceObservaciones.Text = this.rtbObservaciones.Text;
                this.rtbInstrucciones.Text = this._ordenHeader.Instrucciones.Value;
                this.pceInstrucciones.Text = this.rtbInstrucciones.Text;
                this.rtbFormaPago.Text = this._ordenHeader.FormaPago.Value;
                this.pceFormaPago.Text = this.rtbFormaPago.Text;
                this.txtEstado.Text = this._ctrl.Estado.Value.ToString();
                this.cbTerminosCond.Checked = this._ordenHeader.TerminosInd.Value.Value;
                this.lblCotiza.Text = this._cotizaciones.Count.ToString();
                this.dtPeriod.DateTime = this._ctrl.PeriodoDoc.Value.Value;
                this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.ToString();
                this.dtFecha.DateTime = this._ctrl.FechaDoc.Value.Value;
                this.dtFechaEntrega.DateTime = this._ordenHeader.FechaEntrega.Value.Value;
                if (this.cmbIncoTerm.Properties.DataSource != null)
                    this.cmbIncoTerm.EditValue = this._ordenHeader.Inconterm.Value;
                this.txtNroPagos.EditValue = this._ordenHeader.NroPagos.Value != null ? this._ordenHeader.NroPagos.Value.Value : 0;
                this.dtFechaPago1.DateTime = this._ordenHeader.FechaPago1.Value != null ? this._ordenHeader.FechaPago1.Value.Value : this.dtFecha.DateTime;
                this.dtFechaVencimiento.DateTime = this._ordenHeader.FechaVencimiento.Value != null ? this._ordenHeader.FechaVencimiento.Value.Value : this.dtFecha.DateTime;
                this.chkPagoVariableInd.Checked = this._ordenHeader.PagoVariablend.Value != null ? this._ordenHeader.PagoVariablend.Value.Value : false;
                #endregion
                #region Asigna dias de prioridad
                Prioridad prior = (Prioridad)this._ordenHeader.Prioridad.Value.Value;
                string priorString = ((int)prior).ToString();
                this.cmbPrioridad.SelectedItem = this.cmbPrioridad.GetItem(priorString);
                this._daysEntr = (prior == Prioridad.High) ? this._daysHighPriority : (prior == Prioridad.Medium) ? this._daysMediumPriority : this._daysLowPriority;
                #endregion
                #region Asigna controles de valores
                this.txtTasaOC.EditValue = this._ordenHeader.TasaOrden.Value.Value;
                this.txtPorcAdministra.EditValue = this._ordenHeader.PorcentAdministra.Value.Value;
                this.txtPorcHolgura.EditValue = this._ordenHeader.PorcentHolgura.Value.Value;
                this.txtPorcImprevistos.EditValue = this._ordenHeader.Porcentimprevisto.Value.Value;
                this.txtPorcUtilidad.EditValue = this._ordenHeader.PorcentUtilidad.Value.Value;
                this.cbAiuIncCosto.Checked = this._ordenHeader.IncluyeAUICosto.Value.Value;
                this.txtValorDoc.EditValue = this._ordenHeader.Valor.Value.Value;
                this.txtValorIVADoc.EditValue = this._ordenHeader.IVA.Value.Value;
                this.txtValorPagoMes.EditValue = this._ordenHeader.VlrPagoMes.Value != null ? this._ordenHeader.VlrPagoMes.Value.Value : 0;
                this.txtTotPlanPagos.EditValue = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtNroPagos.EditValue, CultureInfo.InvariantCulture);

                #endregion
                #region Asigna Monedas
                this.monedaId = this._ctrl.MonedaID.Value;
                this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda.Local : TipoMoneda.Foreign;
                #endregion

                //Si se presenta un problema asignando la tasa de cambio lo bloquea
                if (this.ValidateHeader())
                {
                    this.EnableHeader(false);
                    this.data = orden;
                    this.validHeader = true;
                    this._headerLoaded = true;
                    if (orden.Footer != null)
                    {
                        this._ordenFooter = orden.Footer;
                        this.LoadData(true);
                        //this.gcDocument.Focus();
                    }
                    else
                        this._ordenFooter = new List<DTO_prOrdenCompraFooter>();

                    #region Carga Solicitudes
                    this._solicitudes = new List<DTO_prSolicitudResumen>();
                    DTO_prSolicitudResumen solRes;
                    foreach (DTO_prOrdenCompraFooter detOC in this._ordenFooter)
                    {
                        if (!this._solicitudes.Exists(x => x.ConsecutivoDetaID.Value == detOC.DetalleDocu.SolicitudDetaID.Value))
                        {
                            if (this._ordenFooter.Where(x => x.DetalleDocu.SolicitudDetaID.Value == detOC.DetalleDocu.SolicitudDetaID.Value).Count() == 1 ||
                                this._ordenFooter.Where(x => x.DetalleDocu.SolicitudDetaID.Value == detOC.DetalleDocu.SolicitudDetaID.Value).Count() > 1 &&
                                (!string.IsNullOrEmpty(detOC.DetalleDocu.CodigoAdminAIU.Value) || !string.IsNullOrEmpty(detOC.DetalleDocu.CodigoImprevAIU.Value) ||
                                !string.IsNullOrEmpty(detOC.DetalleDocu.CodigoUtilidadAIU.Value)))
                            {
                                solRes = new DTO_prSolicitudResumen();
                                solRes.NumeroDoc.Value = detOC.DetalleDocu.SolicitudDocuID.Value;
                                solRes.NumeroDocOC.Value = detOC.DetalleDocu.NumeroDoc.Value;
                                solRes.ConsecutivoDetaID.Value = detOC.DetalleDocu.SolicitudDetaID.Value;
                                solRes.SolicitudDocuID.Value = detOC.DetalleDocu.SolicitudDocuID.Value;
                                solRes.CodigoBSID.Value = detOC.DetalleDocu.CodigoBSID.Value;
                                solRes.inReferenciaID.Value = detOC.DetalleDocu.inReferenciaID.Value;
                                solRes.Parametro1.Value = detOC.DetalleDocu.Parametro1.Value;
                                solRes.Parametro2.Value = detOC.DetalleDocu.Parametro2.Value;
                                solRes.DocumentoID.Value = AppDocuments.Solicitud;                              
                                solRes.Descriptivo.Value = detOC.DetalleDocu.Descriptivo.Value;
                                solRes.UnidadInvID.Value = detOC.DetalleDocu.UnidadInvID.Value;
                                //Valida si las unidades de medida son diferentes
                                if (!string.IsNullOrEmpty(detOC.DetalleDocu.inReferenciaID.Value))
                                {
                                    if (string.IsNullOrEmpty(detOC.DetalleDocu.EmpaqueInvID.Value))
                                    {
                                        DTO_inReferencia refer = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, detOC.DetalleDocu.inReferenciaID.Value, true);
                                        DTO_inEmpaque emp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, refer.EmpaqueInvID.Value, true);
                                        detOC.DetalleDocu.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                                        detOC.DetalleDocu.CantidadxEmpaque.Value = emp.Cantidad.Value;
                                        detOC.DetalleDocu.CantEmpaque.Value = emp.Cantidad.Value > 1 ? Math.Ceiling(Math.Abs(detOC.DetalleDocu.CantidadSol.Value.Value) / emp.Cantidad.Value.Value) : 1;
                                    }
                                    else
                                    {
                                        DTO_inEmpaque emp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, detOC.DetalleDocu.EmpaqueInvID.Value, true);
                                        detOC.DetalleDocu.CantidadxEmpaque.Value = emp.Cantidad.Value;
                                        detOC.DetalleDocu.CantEmpaque.Value = emp.Cantidad.Value > 1 ? Math.Ceiling(Math.Abs(detOC.DetalleDocu.CantidadSol.Value.Value) / emp.Cantidad.Value.Value) : 1;
                                    }
                                }                               
                                solRes.CantidadOrdenComp.Value = detOC.DetalleDocu.CantidadOC.Value;
                                solRes.EmpaqueInvID.Value = detOC.DetalleDocu.EmpaqueInvID.Value;
                                solRes.CantidadxEmpaque.Value = detOC.DetalleDocu.CantidadxEmpaque.Value;
                                solRes.ValorUni.Value = detOC.DetalleDocu.ValorUni.Value.Value;
                                solRes.CantidadDoc1.Value = detOC.DetalleDocu.CantidadDoc1.Value;
                                solRes.CantidadDoc2.Value = detOC.DetalleDocu.CantidadDoc2.Value;
                                solRes.CantidadDoc3.Value = detOC.DetalleDocu.CantidadDoc3.Value;
                                solRes.CantidadDoc4.Value = detOC.DetalleDocu.CantidadDoc4.Value;
                                solRes.CantidadDoc5.Value = detOC.DetalleDocu.CantidadDoc5.Value;
                                solRes.Documento1ID.Value = detOC.DetalleDocu.Documento1ID.Value;
                                solRes.Documento2ID.Value = detOC.DetalleDocu.Documento2ID.Value;
                                solRes.Documento3ID.Value = detOC.DetalleDocu.Documento3ID.Value;
                                solRes.Documento4ID.Value = detOC.DetalleDocu.Documento4ID.Value;
                                solRes.Documento5ID.Value = detOC.DetalleDocu.Documento5ID.Value;
                                solRes.Detalle1ID.Value = detOC.DetalleDocu.Detalle1ID.Value;
                                solRes.Detalle2ID.Value = detOC.DetalleDocu.Detalle2ID.Value;
                                solRes.Detalle3ID.Value = detOC.DetalleDocu.Detalle3ID.Value;
                                solRes.Detalle4ID.Value = detOC.DetalleDocu.Detalle4ID.Value;
                                solRes.Detalle5ID.Value = detOC.DetalleDocu.Detalle5ID.Value;
                                solRes.CantidadSol.Value = detOC.DetalleDocu.CantidadSol.Value.HasValue? Math.Abs(detOC.DetalleDocu.CantidadSol.Value.Value) : 0;
                                solRes.Selected.Value = true;
                                solRes.SolicitudCargos = detOC.SolicitudCargos;

                                this._solicitudes.Add(solRes);
                            }
                        }
                    }


                    #endregion
                }
                else
                {
                    this.CleanHeader(true);
                    this.validHeader = false;
                }
                this.valorTotalDoc = orden.DocCtrl.Valor.Value.Value;
                this.valorIVATotalDoc = orden.DocCtrl.Iva.Value.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "LoadTempData"));
            }
        }

        /// <summary>
        /// Calcula valores globales del documento
        /// </summary>
        /// <param name="index">identificador de fila actual de la grilla</param>
        protected override void GetValuesDocument(int index,bool isIndividual,bool isDescuento)
        {
            base.valorTotalDoc = 0;
            base.valorIVATotalDoc = 0;
            this._vlrTasaCambio = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);

            if (isIndividual)
            {
                if (this._vlrTasaCambio != 0)
                {
                    decimal porcIVA = (this._currentRow.DetalleDocu.PorcentajeIVA.Value.Value / 100);
                    decimal? vlrTot = this._currentRow.DetalleDocu.ValorUni.Value * (this._currentRow.DetalleDocu.UnidadInvID.Value.Equals(this._currentRow.DetalleDocu.EmpaqueInvID.Value) ? this._currentRow.DetalleDocu.CantidadOC.Value : this._currentRow.DetalleDocu.CantEmpaque.Value);
                    if (this.monedaOrden.Equals(this.monedaLocal))
                    {
                        this._currentRow.DetalleDocu.ValorTotME.Value = (vlrTot / this._vlrTasaCambio);
                        this._currentRow.DetalleDocu.IvaTotME.Value = (vlrTot * porcIVA) / this._vlrTasaCambio;
                    }
                    else
                    {
                        this._currentRow.DetalleDocu.ValorTotML.Value = Math.Round((vlrTot.Value * this._vlrTasaCambio), 2);
                        this._currentRow.DetalleDocu.IvaTotML.Value = Math.Round((vlrTot.Value * porcIVA) * this._vlrTasaCambio, 2);
                    }
                }

                #region Asigna totales
                foreach (DTO_prOrdenCompraFooter footer in data.Footer)
                {
                    if (this.monedaLocal.Equals(this.monedaOrden))
                    {
                        this.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
                        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
                    }
                    else
                    {
                        this.valorTotalDoc += footer.DetalleDocu.ValorTotME.Value.Value;
                        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotME.Value.Value;
                    }
                }
                #endregion
            }
            else //Cambia todos los registros
            {
                if (this._ordenHeader != null && this.data.Footer.Count > 0)
                {
                    base.valorTotalDoc = 0;
                    base.valorIVATotalDoc = 0;
                    this._ordenHeader.DtoProntoPago.Value = Convert.ToDecimal(this.txtPorcPrPago.EditValue, CultureInfo.InvariantCulture);
                    foreach (DTO_prOrdenCompraFooter det in this.data.Footer)
                    {
                        decimal porcIVA = (det.DetalleDocu.PorcentajeIVA.Value.Value / 100);
                        det.DetalleDocu.MonedaID.Value = this.monedaOrden;
                        if (isDescuento)
                        {
                            det.DetalleDocu.VlrLocal01.Value = !det.DetalleDocu.VlrLocal01.Value.HasValue ? det.DetalleDocu.ValorUni.Value : det.DetalleDocu.VlrLocal01.Value;
                            det.DetalleDocu.ValorUni.Value = det.DetalleDocu.VlrLocal01.Value - (det.DetalleDocu.VlrLocal01.Value * (this._ordenHeader.DtoProntoPago.Value / 100));
                        }
                        else if (!det.DetalleDocu.VlrLocal01.Value.HasValue)
                            det.DetalleDocu.VlrLocal01.Value = det.DetalleDocu.ValorUni.Value;
                        
                            this.GetValuesAIU(det, this.data.Footer.Count);
                        decimal vlrTot = det.DetalleDocu.ValorUni.Value.Value * (det.DetalleDocu.UnidadInvID.Value.Equals(det.DetalleDocu.EmpaqueInvID.Value) ? det.DetalleDocu.CantidadOC.Value.Value : det.DetalleDocu.CantEmpaque.Value.Value);
                        if (this.monedaOrden.Equals(this.monedaLocal))
                        {
                            det.DetalleDocu.OrigenMonetario.Value = 1;
                            det.DetalleDocu.ValorTotME.Value = (vlrTot / (this._vlrTasaCambio != 0? this._vlrTasaCambio : 1));
                            det.DetalleDocu.IvaTotME.Value = (vlrTot * porcIVA) / (this._vlrTasaCambio != 0? this._vlrTasaCambio : 1);
                            this.valorTotalDoc += det.DetalleDocu.ValorTotML.Value.Value;
                            this.valorIVATotalDoc += det.DetalleDocu.IvaTotML.Value.Value;
                        }
                        else
                        {
                            det.DetalleDocu.OrigenMonetario.Value = 2;
                            det.DetalleDocu.ValorTotML.Value = Math.Round((vlrTot * this._vlrTasaCambio), 2);
                            det.DetalleDocu.IvaTotML.Value = Math.Round((vlrTot * porcIVA) * this._vlrTasaCambio, 2);
                            this.valorTotalDoc += det.DetalleDocu.ValorTotME.Value.Value;
                            this.valorIVATotalDoc += det.DetalleDocu.IvaTotME.Value.Value;
                        }                        
                    }  
                }
            }
            if (this.data.Footer.Count > 0)
            {
                if (this.monedaLocal.Equals(this.monedaOrden))
                {
                    this.txtValorIVAUnit.EditValue = this._currentRow.DetalleDocu.IvaTotML.Value.Value;
                    this.txtValorTotal.EditValue = this._currentRow.DetalleDocu.ValorTotML.Value.Value;
                }
                else
                {
                    this.txtValorIVAUnit.EditValue = this._currentRow.DetalleDocu.IvaTotME.Value.Value;
                    this.txtValorTotal.EditValue = this._currentRow.DetalleDocu.ValorTotME.Value.Value;
                }

                this.txtValorAIU.EditValue = this._currentRow.DetalleDocu.ValorAIU.Value.Value;
                this.txtValorIVAAIU.EditValue = this._currentRow.DetalleDocu.VlrIVAAIU.Value.Value;

                this.gvDocument.RefreshData();

                base.data.HeaderOrdenCompra.Valor.Value = base.valorTotalDoc;
                base.data.HeaderOrdenCompra.IVA.Value = base.valorIVATotalDoc;
                this.txtValorDoc.EditValue = base.valorTotalDoc;
                this.txtValorIVADoc.EditValue = base.valorIVATotalDoc;   
            }
            
        }

        /// <summary>
        /// Carga el documento 
        /// </summary>
        private void LoadDocument()
        {
            try
            {
                if (this._txtOrdenCompraNroFocus)
                {
                    this._txtOrdenCompraNroFocus = false;
                    if (this.txtOrdenCompraNro.Text == string.Empty)
                        this.txtOrdenCompraNro.Text = "0";

                    if (this.txtOrdenCompraNro.Text == "0")
                    {
                        #region Nuevo orden de compra
                        this.gcDocument.DataSource = null;
                        this.data = new DTO_prOrdenCompra();
                        this.newDoc = true;

                        this.EnableHeader(true);
                        this.masterPrefijo.EnableControl(false);
                        this.txtOrdenCompraNro.Enabled = false;
                        this.btnAprRech.Enabled = false;

                        this.masterProveedor.Value = string.Empty;
                        this.txtContrato.Text = string.Empty;
                        this.masterLugarEntr.Value = string.Empty;
                        this.masterAreaAprob.Value = base.areaFuncionalID;
                        this.masterMonedaOrden.Value = this.monedaLocal;
                        this.masterMonedaPago.Value = this.monedaLocal;
                        base.monedaOrden = this.masterMonedaOrden.Value;
                        this.cmbPrioridad.SelectedIndex = 0;
                        this.txtEstado.Text = ((int)EstadoDocControl.SinAprobar).ToString();
                        this.lblCotiza.Text = "0";
                        #endregion
                    }
                    else
                    {                    
                       #region Carga la orden de Compra
                            DTO_prOrdenCompra Orden = _bc.AdministrationModel.OrdenCompra_Load(this.documentID, this.masterPrefijo.Value, Convert.ToInt32(this.txtOrdenCompraNro.Text));
                            //Valida si existe
                            if (Orden == null)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NoOrdenCompra)); ////
                                this.txtOrdenCompraNro.Focus();
                                this.validHeader = false;
                                return;
                            }
                            if (this._copyData)
                            {
                                Orden.DocCtrl.NumeroDoc.Value = 0;
                                Orden.DocCtrl.DocumentoNro.Value = 0;
                                Orden.HeaderOrdenCompra.NumeroDoc.Value = 0;
                                if (Orden.DocCtrl.Estado.Value != (byte)EstadoDocControl.Revertido)
                                    Orden.Footer = new List<DTO_prOrdenCompraFooter>();
                                Orden.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                                this._copyData = false;
                            }
                            this.newDoc = false;

                            if (Orden.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                            {
                                FormProvider.Master.itemSave.Enabled = false;
                                FormProvider.Master.itemSendtoAppr.Enabled = false;
                            }
                            this.LoadTempData(Orden);

                            if (!this.monedaLocal.Equals(this.masterMonedaOrden.Value))
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = false;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].Visible = false;
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].VisibleIndex = 12;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].VisibleIndex = 13;
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].Visible = true;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].Visible = true;
                                
                            }
                            else //if (!this.monedaExtranjera.Equals(this.monedaOrden))
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].Visible = false;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].Visible = false;
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].VisibleIndex = 12;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].VisibleIndex = 13;
                                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = true;
                                this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].Visible = true;
                                
                            }
                            #endregion                    
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "LoadDocument"));
            }
        }
        #endregion

        #region Eventos Header

        #region Maestras

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
                throw ex;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProveedor.ValidID)
                {
                    DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                    if (proveedor.TipoProveedor.Value == (byte)TipoProveedor.Local || proveedor.TipoProveedor.Value == null)
                        this.cmbIncoTerm.Properties.DataSource = null;
                    else 
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermCFR));
                        dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermCIF));
                        dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermCIP));
                        dic.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermCPT));
                        dic.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermDAF));
                        dic.Add("6", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermDDP));
                        dic.Add("7", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermDDU));
                        dic.Add("8", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermDEQ));
                        dic.Add("9", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermDES));
                        dic.Add("10", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermEXW));
                        dic.Add("11", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermFAS));
                        dic.Add("12", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermFCA));
                        dic.Add("13", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_IncotermFOB));
                        this.cmbIncoTerm.EditValue = 1;
                        this.cmbIncoTerm.Properties.DataSource = dic;
                    }
                    if (string.IsNullOrEmpty(this.txtContacto.Text))
                        this.txtContacto.Text = string.IsNullOrEmpty(proveedor.ContactoComercial.Value) ? proveedor.Contacto.Value : proveedor.ContactoComercial.Value;                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "masterProveedor_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterLocFisica_Leave(object sender, EventArgs e)
        {
            try
            {
                DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica,false, this.masterLugarEntr.Value, true);
                if (locFisica != null)
                {
                    this.txtDirEntr.Text = this._ordenHeader != null && this._ordenHeader.LugarEntrega.Value != locFisica.ID.Value ? locFisica.Direccion.Value : this.txtDirEntr.Text;
                    this.txtTelefEntr.Text =  this._ordenHeader != null && this._ordenHeader.LugarEntrega.Value != locFisica.ID.Value ?locFisica.Telefono.Value :  this.txtTelefEntr.Text;
                    this.txtEncargadoEntr.Text = this._ordenHeader != null && this._ordenHeader.LugarEntrega.Value != locFisica.ID.Value ? locFisica.Encargado.Value : this.txtEncargadoEntr.Text;
                    if (this._ordenHeader != null)
                    {
                        this._ordenHeader.LugarEntrega.Value = this.masterLugarEntr.Value;
                        this._ordenHeader.DireccionEntrega.Value = this.txtDirEntr.Text;
                        this._ordenHeader.TelefonoEntrega.Value = this.txtTelefEntr.Text;
                        this._ordenHeader.Encargado.Value = this.txtEncargadoEntr.Text;
                    }
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "masterLocFisica_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterMonedaOrden_Leave(object sender, EventArgs e)
        {
            try
            {
                this.monedaOrden = this.masterMonedaOrden.ValidID ? this.masterMonedaOrden.Value : string.Empty;
                this.data.HeaderOrdenCompra.MonedaOrden.Value = this.monedaOrden;
                this._ordenHeader.MonedaOrden.Value = this.monedaOrden;
                base.valorTotalDoc = 0;
                base.valorIVATotalDoc = 0;
                this._vlrTasaCambio = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
                if (!this.monedaLocal.Equals(this.monedaOrden))
                {
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].VisibleIndex = 12;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].VisibleIndex = 13;
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].Visible = false;
                }
                else 
                {
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].VisibleIndex = 10;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].VisibleIndex = 11;
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotML"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "ValorTotME"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "IvaTotME"].Visible = false;
                }

                foreach (DTO_prOrdenCompraFooter det in this.data.Footer)
                {
                    if (!this.monedaLocal.Equals(this.monedaOrden) && this._vlrTasaCambio != 0)
                    {
                        decimal vlr = Math.Round(det.DetalleDocu.ValorTotML.Value.Value / (det.DetalleDocu.UnidadInvID.Value.Equals(det.DetalleDocu.EmpaqueInvID.Value) ? det.DetalleDocu.CantidadOC.Value.Value :det.DetalleDocu.CantEmpaque.Value.Value), 2);
                        det.DetalleDocu.ValorUni.Value = Math.Round(vlr / this._vlrTasaCambio, 4);
                    }
                    else
                    {
                        decimal vlr = Math.Round(det.DetalleDocu.ValorTotME.Value.Value / (det.DetalleDocu.UnidadInvID.Value.Equals(det.DetalleDocu.EmpaqueInvID.Value) ? det.DetalleDocu.CantidadOC.Value.Value : det.DetalleDocu.CantEmpaque.Value.Value), 2);
                        det.DetalleDocu.ValorUni.Value = Math.Round(vlr * (this._vlrTasaCambio != 0 ? this._vlrTasaCambio : 1), 4);
                    }
                    this.GetValuesAIU(det, this.data.Footer.Count);
                    decimal porcIVA = (det.DetalleDocu.PorcentajeIVA.Value.Value / 100);
                    decimal vlrTot = det.DetalleDocu.ValorUni.Value.Value * (det.DetalleDocu.UnidadInvID.Value.Equals(det.DetalleDocu.EmpaqueInvID.Value) ? det.DetalleDocu.CantidadOC.Value.Value : det.DetalleDocu.CantEmpaque.Value.Value);
                    if (this.monedaOrden.Equals(this.monedaLocal))
                        {
                            det.DetalleDocu.OrigenMonetario.Value = 1;
                        det.DetalleDocu.ValorTotME.Value = this._vlrTasaCambio != 0? (vlrTot / this._vlrTasaCambio) : 0;
                        det.DetalleDocu.IvaTotME.Value =this._vlrTasaCambio != 0? (vlrTot * porcIVA) / this._vlrTasaCambio : 0;
                            this.valorTotalDoc += det.DetalleDocu.ValorTotML.Value.Value;
                            this.valorIVATotalDoc += det.DetalleDocu.IvaTotML.Value.Value;
                        }
                        else
                        {
                            det.DetalleDocu.OrigenMonetario.Value = 2;
                            det.DetalleDocu.ValorTotML.Value = Math.Round((vlrTot * this._vlrTasaCambio), 2);
                            det.DetalleDocu.IvaTotML.Value = Math.Round((vlrTot * porcIVA) * this._vlrTasaCambio, 2);
                            this.valorTotalDoc += det.DetalleDocu.ValorTotME.Value.Value;
                            this.valorIVATotalDoc += det.DetalleDocu.IvaTotME.Value.Value;
                        }   
                }
                this.gvDocument.RefreshData();

                base.data.HeaderOrdenCompra.Valor.Value = base.valorTotalDoc;
                base.data.HeaderOrdenCompra.IVA.Value = base.valorIVATotalDoc;
                this.txtValorDoc.EditValue = base.valorTotalDoc;
                this.txtValorIVADoc.EditValue = base.valorIVATotalDoc;   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "masterMonedaOrden_Leave"));
            }
        }

        #endregion

        #region Botones
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
        /// Trae el formulario auxiliar de aprobar/rechazar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAprRech_Click(object sender, EventArgs e)
        {
            if (this._ordenHeader != null && this._ctrl != null)
            {
                ModalAprRechOC mod = new ModalAprRechOC(this._ordenHeader, this._ctrl);
                mod.ShowDialog();
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.OrdenCompra);
            ModalQueryOrdCompra getDocControl = new ModalQueryOrdCompra(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this.txtOrdenCompraNro.Enabled = true;
                this.txtOrdenCompraNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtOrdenCompraNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Abre una modal de plan de pagos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnEjecucion_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPlanPagos planPagos = new ModalPlanPagos(this._contratoPlanPagos, Convert.ToInt32(this.txtNumeroDoc.Text), this.dtFecha.DateTime, this.valorTotalDoc, this.chkPagoVariableInd.Checked);
                planPagos.ShowDialog();
                if (planPagos.ReturnVals)
                    this._contratoPlanPagos = planPagos.ReturnList;
                this.valorTotalPagosMes = this._contratoPlanPagos.Sum(x => x.Valor.Value.Value + x.ValorAdicional.Value.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "btnEjecucion_Click"));
            }
        }

        /// <summary>
        /// Abre una modal de polizas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnPolizas_Click(object sender, EventArgs e)
        {
            ModalPolizaProveedor convenios = new ModalPolizaProveedor(this._polizas, Convert.ToInt32(this.txtNumeroDoc.Text));
            convenios.ShowDialog();
            if (convenios.ReturnVals)
                this._polizas = convenios.ReturnList;
        }

        #endregion

        #region Valores

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void txtOrdenCompraNro_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtOrdenCompraNro_Enter(object sender, EventArgs e)
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
        private void txtOrdenCompraNro_Leave(object sender, EventArgs e)
        {
            this.LoadDocument();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtTasaCambio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._vlrTasaCambio = Convert.ToDecimal(this.txtTasaOC.EditValue, CultureInfo.InvariantCulture);
                if (this._vlrTasaCambio != 0)
                {
                    foreach (DTO_prOrdenCompraFooter det in this.data.Footer)
                    {
                        if (this.monedaOrden.Equals(this.monedaLocal))
                        {
                            det.DetalleDocu.ValorTotME.Value = det.DetalleDocu.ValorTotML.Value / this._vlrTasaCambio;
                            det.DetalleDocu.IvaTotME.Value = det.DetalleDocu.IvaTotML.Value / this._vlrTasaCambio;
                        }

                        else
                        {
                            det.DetalleDocu.ValorTotME.Value = det.DetalleDocu.ValorTotME.Value ?? 0;
                            det.DetalleDocu.IvaTotME.Value = det.DetalleDocu.IvaTotME.Value ?? 0;
                            det.DetalleDocu.ValorTotML.Value = Math.Round(det.DetalleDocu.ValorTotME.Value.Value * this._vlrTasaCambio, 2);
                            det.DetalleDocu.IvaTotML.Value = Math.Round(det.DetalleDocu.IvaTotME.Value.Value * this._vlrTasaCambio, 2);
                        }
                    }
                    this.gvDocument.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "txtTasaCambio_TextChanged"));
            }
        }

        /// <summary>
        /// Hace un descuento sobre los items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPorcPrPago_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.GetValuesDocument(0, false,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "txtPorcPrPago_EditValueChanged"));
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
                if (!this.chkPagoVariableInd.Checked)
                {
                    switch (ctrl.Name)
                    {
                        case "txtValorPagoMes":
                            #region txtValorPagoMes
                            this.data.HeaderOrdenCompra.VlrPagoMes.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                            this.data.HeaderOrdenCompra.NroPagos.Value = Convert.ToByte(this.txtNroPagos.EditValue);
                            #endregion
                            break;
                        case "txtNroPagos":
                            #region txtNroPagos
                            this.data.HeaderOrdenCompra.NroPagos.Value = Convert.ToByte(ctrl.EditValue);
                            this.data.HeaderOrdenCompra.VlrPagoMes.Value = Convert.ToDecimal(this.txtValorPagoMes.EditValue, CultureInfo.InvariantCulture);
                            #endregion
                            break;
                    }
                    DateTime fecha1erPago = this.dtFechaPago1.DateTime;
                    this._contratoPlanPagos.Clear();
                    for (int i = 0; i < this.data.HeaderOrdenCompra.NroPagos.Value; i++)
                    {
                        DTO_prContratoPlanPago pagos = new DTO_prContratoPlanPago();
                        pagos.EmpresaID.Value = this.empresaID;
                        pagos.Fecha.Value = fecha1erPago.AddMonths(i);
                        pagos.Observacion.Value = "Pago " + (i == 0 ? 1 : (i + 1)).ToString();
                        pagos.Valor.Value = this.data.HeaderOrdenCompra.VlrPagoMes.Value;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "txtValorPagoMes_Leave"));
            }
        }

        #endregion

        #region Fechas
        /// <summary>
        /// Evento que se ejecuta al salir control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechaOC_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.dtFechaEntrega.DateTime < this.dtFechaOC.DateTime)
                    this.dtFechaEntrega.DateTime = this.dtFechaOC.DateTime;
                // this.AsignarTasaCambio(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "masterProveedor_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechaEntrega_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.dtFechaEntrega.DateTime < this.dtFechaOC.DateTime)
                    this.dtFechaEntrega.DateTime = this.dtFechaOC.DateTime;
                // this.AsignarTasaCambio(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "masterProveedor_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "dtFecha1Pago_Leave"));
            }
        }
        #endregion

        #region Otros
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
            {; }

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
            {; }
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
            {; }
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
            {; }
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
        /// Evento que se valida el plan de pagos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkPagoVariableInd_CheckedChanged(object sender, EventArgs e)
        {
            this.txtValorPagoMes.Enabled = !this.chkPagoVariableInd.Checked;
            this.txtNroPagos.Enabled = !this.chkPagoVariableInd.Checked;
            this.dtFechaPago1.Enabled = !this.chkPagoVariableInd.Checked;
            this.dtFechaVencimiento.Enabled = !this.chkPagoVariableInd.Checked;
            //this.btnEjecucion.Enabled = this.chkPagoVariableInd.Checked;
        }

        /// <summary>
        /// Cambia valores segun el item seleccionado
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbReporte_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._tipoReporte = Convert.ToByte(this.cmbReporte.EditValue);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        #endregion
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
                if (this.txtOrdenCompraNro.Text == "0")
                {
                    //FormProvider.Master.itemSendtoAppr.Enabled = false;
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
                        this._ordenHeader = orden.HeaderOrdenCompra;
                        this._ordenFooter = orden.Footer;
                        this.TempData = orden;
                                               
                        this.LoadData(true);

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "gcDocument_Enter" + ex.Message));
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
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
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
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
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
                    this._ordenHeader = new DTO_prOrdenCompraDocu();
                    this._ordenFooter = new List<DTO_prOrdenCompraFooter>();
                    this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                    this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
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
                    this.txtOrdenCompraNro.Enabled = true;
                    this.btnAprRech.Enabled = false;
                    this.btnQueryDoc.Enabled = true;
                    this._headerLoaded = false;               
                }
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
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
                        var det = ObjectCopier.Clone(this.data.Footer);
                        this.data = (DTO_prOrdenCompra)this.LoadTempHeader();
                        this.data.Footer = det;
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
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
            if (this.CanSave())
            {
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }
            }          
        }

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            base.GenerateReport(true);
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
                    this.data.DocCtrl.Valor.Value = this.data.HeaderOrdenCompra.Valor.Value;
                    this.data.DocCtrl.Iva.Value = this.data.HeaderOrdenCompra.IVA.Value;
                    numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                    update = true;
                };
                DTO_SerializedObject result = _bc.AdministrationModel.OrdenCompra_Guardar(this.documentID, this.data, update, out numeroDoc);               
                
                bool isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.Invoke(this.saveDelegate);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "SaveThread"));
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
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result = new DTO_SerializedObject();
                if (this._ctrl == null)
                {
                    result = resultNOK;
                    return;
                }
                else if (this.data.DocCtrl.NumeroDoc.Value.Value == 0)
                {
                    int numeroDoc = 0;
                    bool update = false;
                    if (this._ctrl.NumeroDoc.Value.Value != 0)
                    {                       
                        numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                        update = true;
                    }
                    result = this._bc.AdministrationModel.OrdenCompra_Guardar(this.documentID, this.data, update, out numeroDoc);
                    this._ctrl.NumeroDoc.Value = numeroDoc;
                }
                if (result.GetType() != typeof(DTO_TxResult))
                    result = this._bc.AdministrationModel.Solicitud_SendToAprob(this.documentID, this._ctrl.NumeroDoc.Value.Value, true);

                bool isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prOrdenCompra();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._ordenHeader = new DTO_prOrdenCompraDocu();
                    this._ordenFooter = new List<DTO_prOrdenCompraFooter>();
                    this._cotizaciones = new List<DTO_prOrdenCompraCotiza>();
                    this._contratoPlanPagos = new List<DTO_prContratoPlanPago>();
                    this._polizas = new List<DTO_prContratoPolizas>();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion     
    }
}
