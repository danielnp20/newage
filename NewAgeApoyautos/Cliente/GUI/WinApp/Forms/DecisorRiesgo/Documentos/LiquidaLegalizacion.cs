using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionLegalizacion : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        protected int documentReportID = 0;
        private bool _allowEditValuesInd = true;
        private string _userAutoriza = string.Empty;

        //Datos por defecto
        private string _lugGeoXDef = String.Empty;
        private string _AseguradoraXDef = String.Empty;
        private string _ZonaXDef = String.Empty;
        private string _VitrinaXDef = String.Empty;
        private string centroCostoSol = string.Empty;

        //DTOs        
        private DTO_ccCliente _cliente = new DTO_ccCliente();
        private List<DTO_ccSolicitudComponentes> _componentesVisibles = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudComponentes> _componentesContab = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_Cuota> _planPagos = new List<DTO_Cuota>();
        private DTO_PlanDePagos _liquidador = new DTO_PlanDePagos();
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_glActividadFlujo _actFlujoLegalizacion = new DTO_glActividadFlujo();        
        private DTO_ccSolicitudDocu _headerSolicitud = new DTO_ccSolicitudDocu();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_drActividadChequeoLista> actChequeoBase = new List<DTO_drActividadChequeoLista>();
        private List<DTO_drSolicitudDatosChequeados> actChequeo = new List<DTO_drSolicitudDatosChequeados>();

        private List<DTO_ccSolicitudCompraCartera> _compCartera = new List<DTO_ccSolicitudCompraCartera>();
        private DTO_Cuota _cuota = new DTO_Cuota();
        private List<DTO_ccSolicitudDetallePago> _detallesPago = new List<DTO_ccSolicitudDetallePago>();

        //Identificador de la proxima actividad
        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        //Variables formulario (campos)
        private string _tipoCreditoID = string.Empty;
        private string _clienteID = string.Empty;
        private string _lineaCreditoID = string.Empty;
        private short _plazo = 0;
        private short _plazopoliza = 0;
        private int _libranzaID;
        //private int _libranzaID = 0;
        private string _polizaID = string.Empty;
        private TipoCredito _tipoCredito = TipoCredito.Nuevo;

        //Variables auxiliares del formulario
        private DateTime periodo;
        private bool isValid;
        private bool validateData;
        private bool liquidaInd = false;
        private bool liquidaAll = true;
        private bool _mensajeGuardar = true;
        Dictionary<int, string> servicio = new Dictionary<int, string>();
        Dictionary<int, string> garante = new Dictionary<int, string>();
        Dictionary<int, string> tipoReporte = new Dictionary<int, string>();
        Dictionary<string, string> tipoInmueble = new Dictionary<string, string>();

        Dictionary<string, decimal> compsNuevoValor = new Dictionary<string, decimal>();
        private string _cuentaAbonoCap;
        private bool readOnly = false;
        private bool _verifica = false;

        //Valores temporales
        private decimal vlrLibranza = 0;
        private decimal vlrGiro = 0;
        private int vlrSolicitadoPrestamo = 0;
        
        private decimal porInteres = 0;
        private decimal porInteresPoliza = 0;
        private int edad = 0;

        //Variables de mensajes
        private string msgFinDoc;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public LiquidacionLegalizacion()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public LiquidacionLegalizacion(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public LiquidacionLegalizacion(int libranza, string mod)
        {
            this.Constructor(libranza, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public LiquidacionLegalizacion(int libranza, bool readOnly,bool verifica)
        {
            InitializeComponent();
            try
            {
                this._verifica = verifica;

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cf;
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de las actividades

                List<string> actividadesSolicitud = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                if (actividadesSolicitud.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    string actividadFlujoSolicitud = actividadesSolicitud[0];
                    string ActFlujoLegalizacion = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Legalizacion);

                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
                    this._actFlujoLegalizacion = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, ActFlujoLegalizacion, true);
                }

                #endregion
                #region Carga Actividades a devolver
                //Carga la actividades a revertir
                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                this.actFlujoForDevolucion.Add("Ninguna", "Devolver Flujo");
                foreach (DTO_glActividadFlujo act in actPadres)
                {
                    this.actividadesFlujo.Add(act.ID.Value);
                    this.actFlujoForDevolucion.Add(act.ID.Value, act.Descriptivo.Value);
                }
                //Carga el rescurso de Anulado
                //string msgAnulado = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
                //msgAnulado = msgAnulado.ToUpper();

                ////Agrega el texto de anulado
                //this.actividadesFlujo.Add(msgAnulado);
                //this.actFlujoForReversion.Add("Anular", msgAnulado);

                this.cmbActividades.Properties.DataSource = this.actFlujoForDevolucion;
                this.cmbActividades.EditValue = "Ninguna";
                #endregion
                #region Trae la solicitud
                this.readOnly = readOnly;
                this.LoadData(libranza);
                if (this._verifica)
                {
                    this.tabControl.SelectedTabPageIndex = 5;
                    this.EnableHeader();
                }
                //Acitividades de Chequeo
                this.AddChequeoCols();
                this.LoadActividades();
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacionFinanciera.cs", "LiquidacionLegalizacionFinanciera"));
            }
        }
        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader()
        {
            this.groupBox1.Enabled = false;
            this.groupBox2.Enabled = false;
            this.groupBox3.Enabled = false;
            this.groupBox4.Enabled = false;
            this.groupBox5.Enabled = false;
            this.groupBox6.Enabled = false;
            this.groupBox7.Enabled = false;
            this.groupBox8.Enabled = false;
            this.groupBox9.Enabled = false;
            this.groupBox10.Enabled = false;
            this.groupBox11.Enabled = false;
            this.groupBox12.Enabled = false;
            this.groupBox13.Enabled = false;
            this.groupBox14.Enabled = false;
            this.groupBox15.Enabled = false;
            this.groupBox16.Enabled = false;
            this.groupBox17.Enabled = false;
            this.dtFechaFirmaDocumento.Enabled = false;
            this.btn_Liquidar.Enabled = false;
            

        }
        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(int? libranza, string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (this._frmModule == ModulesPrefix.dr && actividades.Count > 0)
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                else
                {
                    #region Carga la info de la proxima actividad
                    List<string> NextActs = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.SolicitudLibranza);
                    if (NextActs.Count != 1)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                        MessageBox.Show(string.Format(msg, AppDocuments.SolicitudLibranza.ToString()));
                        this.EnableHeader(false);
                    }
                    #endregion
                }
                #endregion
                #region Carga Actividades a devolver
                //Carga la actividades a revertir
                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                this.actFlujoForDevolucion.Add("Ninguna", "Devolver Flujo");
                foreach (DTO_glActividadFlujo act in actPadres)
                {
                    this.actividadesFlujo.Add(act.ID.Value);
                    this.actFlujoForDevolucion.Add(act.ID.Value, act.Descriptivo.Value);
                }
                //Carga el rescurso de Anulado
                //string msgAnulado = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
                //msgAnulado = msgAnulado.ToUpper();

                ////Agrega el texto de anulado
                //this.actividadesFlujo.Add(msgAnulado);
                //this.actFlujoForReversion.Add("Anular", msgAnulado);

                this.cmbActividades.Properties.DataSource = this.actFlujoForDevolucion;
                this.cmbActividades.EditValue = "Ninguna";
                #endregion
                this.LoadData(libranza);


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "LiquidacionLegalizacion"));
            }
        }
     
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga datos
        /// </summary>
        /// <param name="libranzaNro"></param>
        private void LoadData(int? libranzaNro)
        {
            this._data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(libranzaNro.Value);
            DTO_DigitacionCredito sol = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(libranzaNro.Value, this._actFlujo.ID.Value);
            this.SetValuesFromSolicitud();
            if (this._data != null)
            {
                #region Solicitud existente
                this._ctrl = this._data.DocCtrl;

                if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)
                {
                    this.AssignValues(true);
                    this.EnableHeader(true);
                    this._libranzaID = _data.SolicituDocu.Libranza.Value.Value;
                }

                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAprobada));
                    CleanData();

                    this.txtCedulaDeudor.Focus();
                }
                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                    CleanData();

                    this.txtCedulaDeudor.Focus();
                }

                #endregion
            }
            else
            {
                this.CleanData();
            }
        }

        /// <summary>
        /// Asigna datos
        /// </summary>
        /// <param name="isGet"></param>
        private void AssignValues(bool isGet)
        {
            try
            {
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
                DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_drSolicitudDatosPersonales codeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
                DTO_drSolicitudDatosPersonales codeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);
                DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo;
                DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;
                
                #region Garantia1
 
                this.lblDeudorPrenda1.Visible=true;
                this.chkDeudorPrenda1.Visible=true;
                this.lblDeudorHipoteca1.Visible=true;
                this.chkDeudorHipoteca1.Visible=true;

                this.lblConyugeHipoteca1.Visible=false;
                this.chkConyugePrenda1.Visible=false;
                this.lblConyugeHipoteca1.Visible=false;
                this.chkConyugeHipoteca1.Visible=false;

                this.lblCodeudor1Prenda1.Visible=false;
                this.chkCodeudor1Prenda1.Visible=false;
                this.lblCodeudor1Hipoteca1.Visible=false;
                this.chkCodeudor1Hipoteca1.Visible=false;

                this.lblCodeudor2Prenda1.Visible=false;
                this.chkCodeudor2Prenda1.Visible=false;
                this.lblCodeudor2Hipoteca1.Visible=false;
                this.chkCodeudor2Hipoteca1.Visible=false;
                
                #endregion

                #region Garantia2

                this.lblDeudorPrenda2.Visible = true;
                this.chkDeudorPrenda2.Visible = true;
                this.lblDeudorHipoteca2.Visible = true;
                this.chkDeudorHipoteca2.Visible = true;

                this.lblConyugeHipoteca2.Visible = false;
                this.chkConyugePrenda2.Visible = false;
                this.lblConyugeHipoteca2.Visible = false;
                this.chkConyugeHipoteca2.Visible = false;

                this.lblCodeudor1Prenda2.Visible = false;
                this.chkCodeudor1Prenda2.Visible = false;
                this.lblCodeudor1Hipoteca2.Visible = false;
                this.chkCodeudor1Hipoteca2.Visible = false;

                this.lblCodeudor2Prenda2.Visible = false;
                this.chkCodeudor2Prenda2.Visible = false;
                this.lblCodeudor2Hipoteca2.Visible = false;
                this.chkCodeudor2Hipoteca2.Visible = false;
                #endregion 
                #region muestra indicadores

                if (conyuge != null)
                {
                    this.lblConyugeHipoteca1.Visible=true;
                    this.chkConyugePrenda1.Visible=true;
                    this.lblConyugeHipoteca1.Visible=true;
                    this.chkConyugeHipoteca1.Visible=true;

                    this.lblConyugeHipoteca2.Visible = true;
                    this.chkConyugePrenda2.Visible = true;
                    this.lblConyugeHipoteca2.Visible = true;
                    this.chkConyugeHipoteca2.Visible = true;

                }
                if (codeudor1 != null)
                {
                    this.lblCodeudor1Prenda1.Visible=true;
                    this.chkCodeudor1Prenda1.Visible=true;
                    this.lblCodeudor1Hipoteca1.Visible=true;
                    this.chkCodeudor1Hipoteca1.Visible=true;

                    this.lblCodeudor1Prenda2.Visible = true;
                    this.chkCodeudor1Prenda2.Visible = true;
                    this.lblCodeudor1Hipoteca2.Visible = true;
                    this.chkCodeudor1Hipoteca2.Visible = true;

                }
                if (codeudor2 != null)
                {
                    this.lblCodeudor2Prenda1.Visible=true;
                    this.chkCodeudor2Prenda1.Visible=true;
                    this.lblCodeudor2Hipoteca1.Visible=true;
                    this.chkCodeudor2Hipoteca1.Visible=true;

                    this.lblCodeudor2Prenda2.Visible = true;
                    this.chkCodeudor2Prenda2.Visible = true;
                    this.lblCodeudor2Hipoteca2.Visible = true;
                    this.chkCodeudor2Hipoteca1.Visible = true;

                }
                #endregion

                #region muestra hojas
                this.tpDatosPrenda.PageVisible = false;
                this.tpDatosPrenda2.PageVisible = false;
                this.tpDatosHipoteca.PageVisible = false;
                this.tpDatosHipoteca2.PageVisible = false;

                if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                {
                    this.tpDatosPrenda.PageVisible = true;
                    this.tabControl.SelectedTabPageIndex = 0;
                    if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                    {
                        this.tpDatosPrenda2.PageVisible = true;
                    }
                    if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                    {
                        this.tpDatosHipoteca.PageVisible = true;
                    }
                    if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                    {
                        this.tpDatosHipoteca2.PageVisible = true;
                    }

                }
                else
                {
                    this.tpDatosPrenda.PageVisible = false;
                    if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                    {
                        this.tpDatosPrenda.PageVisible = true;
                        this.tabControl.SelectedTabPageIndex = 1;
                        if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                        {
                            this.tpDatosHipoteca.PageVisible = true;
                        }
                        if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                        {
                            this.tpDatosHipoteca2.PageVisible = true;
                        }
                    }
                    else
                    {
                        this.tpDatosPrenda2.PageVisible = false;
                        if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                        {
                            this.tpDatosHipoteca.PageVisible = true;
                            this.tabControl.SelectedTabPageIndex = 2;
                            if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                            {
                                this.tpDatosHipoteca2.PageVisible = true;
                            }

                        }
                        else
                        {
                            this.tpDatosHipoteca.PageVisible = false;
                            if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                            {
                                this.tpDatosHipoteca.PageVisible = true;
                                this.tabControl.SelectedTabPageIndex = 3;
                            }
                            else
                            {
                                this.tpDatosHipoteca.PageVisible = false;
                                this.tabControl.SelectedTabPageIndex = 4;
                            }
                        }
                    }
                }
                #endregion

                this._data.SolicituDocu.Nombre.Value = this._data.SolicituDocu.NombrePri.Value + " "+ this._data.SolicituDocu.NombreSdo.Value + " " + this._data.SolicituDocu.ApellidoPri.Value + " " + this._data.SolicituDocu.ApellidoSdo.Value;
                if (isGet)
                {
                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        #region datos Garantia
                        #region Datos Prenda1
                            this.txtPrenda.Text = vehiculo.NumeroPrenda.Value.ToString();
                            if (!string.IsNullOrWhiteSpace(this.txtPrenda.Text))
                                if (this.txtPrenda.Text != "0")
                                    this.btnGenerarPrenda.Enabled = false;
                            this.chkDeudorPrenda1.Checked = deudor.GarantePrenda1Ind.Value.HasValue ? deudor.GarantePrenda1Ind.Value.Value : false;
                            if (conyuge != null)
                            {
                                this.chkConyugePrenda1.Checked = conyuge.GarantePrenda1Ind.Value.HasValue ? conyuge.GarantePrenda1Ind.Value.Value : false;
                            }
                            if (codeudor1 != null)
                            {
                                this.chkCodeudor1Prenda1.Checked = codeudor1.GarantePrenda1Ind.Value.HasValue ? codeudor1.GarantePrenda1Ind.Value.Value : false;
                            }

                            if (codeudor2 != null)
                            {
                                this.chkCodeudor2Prenda1.Checked = codeudor2.GarantePrenda1Ind.Value.HasValue ? codeudor2.GarantePrenda1Ind.Value.Value : false;
                            }
                            this.masterAseguradoraPrenda1.Value = vehiculo.Aseguradora1VEH.ToString();
                            this.txtVlrPolizaPrenda1.EditValue = vehiculo.VlrPolizaVEH1.Value;
                            this.chkPolizaCancelaContadoGarantiaPrenda1.Checked = vehiculo.CancelaContadoPolizaIndVEH1.Value.HasValue ? vehiculo.CancelaContadoPolizaIndVEH1.Value.Value : false;
                            this.chkPolizaDiferenteCubriendoPrenda1.Checked = vehiculo.IntermediarioExternoIndVEH1.Value.HasValue ? vehiculo.IntermediarioExternoIndVEH1.Value.Value : false;

                            this.txtVlrVehiculoPrenda.EditValue = vehiculo.PrecioVenta.Value;
                            this.chkCeroKMPrenda.Checked = vehiculo.CeroKmInd.Value.Value;
                            this.txtMarcaPrenda.Text = vehiculo.Marca.Value.ToString();
                            this.txtModeloPrenda.Text = vehiculo.Modelo.Value.ToString();
                            this.txtMotorPrenda.Text = vehiculo.Motor.Value.ToString();
                            this.txtSeriePrenda.Text = vehiculo.Serie.Value.ToString();
                            this.txtClasePrenda.Text = vehiculo.Clase.Value.ToString();
                            this.txtChasisPrenda.Text = vehiculo.Chasis.Value.ToString();
                            this.txtPlacaPrenda.Text = vehiculo.Placa.Value.ToString();
                            this.txtColorPrenda.Text = vehiculo.Color.Value.ToString();
                            this.cmbServicioPrenda.EditValue = vehiculo.Servicio.Value;
                            this.txtTipoVehiculoPrenda.Text = vehiculo.Tipo.Value.ToString();
                            this.txtLineaPrenda.Text = vehiculo.Linea.ToString();
                            this.rbDocumentoBase.SelectedIndex = vehiculo.DocPrenda.Value.HasValue ? Convert.ToByte(vehiculo.DocPrenda.Value) : Convert.ToByte(1);
                            this.txtDocumentoBaseDescr1.Text = vehiculo.NumeroFactura.Value;
                        #endregion
                            if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                            {
                                #region Datos Prenda2
                                this.txtPrenda2.Text = vehiculo.NumeroPrenda_2.Value.ToString();
                                if (!string.IsNullOrWhiteSpace(this.txtPrenda2.Text))
                                    if (this.txtPrenda2.Text != "0")
                                        this.btnGenerarPrenda.Enabled = false;
                                this.chkDeudorPrenda2.Checked = deudor.GarantePrenda2Ind.Value.HasValue ? deudor.GarantePrenda2Ind.Value.Value : false;
                                if (conyuge != null)
                                {
                                    this.chkConyugePrenda2.Checked = conyuge.GarantePrenda2Ind.Value.HasValue ? conyuge.GarantePrenda2Ind.Value.Value : false;
                                }
                                if (codeudor1 != null)
                                {
                                    this.chkCodeudor1Prenda2.Checked = codeudor1.GarantePrenda2Ind.Value.HasValue ? codeudor1.GarantePrenda2Ind.Value.Value : false;
                                }

                                if (codeudor2 != null)
                                {
                                    this.chkCodeudor2Prenda2.Checked = codeudor2.GarantePrenda2Ind.Value.HasValue ? codeudor2.GarantePrenda2Ind.Value.Value : false;
                                }
                                this.masterAseguradoraPrenda2.Value = vehiculo.Aseguradora2VEH.ToString();
                                this.txtVlrPolizaPrenda2.EditValue = vehiculo.VlrPolizaVEH2.Value;
                                this.chkPolizaCancelaContadoGarantiaPrenda2.Checked = vehiculo.CancelaContadoPolizaIndVEH2.Value.HasValue ? vehiculo.CancelaContadoPolizaIndVEH2.Value.Value : false;
                                this.chkPolizaDiferenteCubriendoPrenda2.Checked = vehiculo.IntermediarioExternoIndVEH2.Value.HasValue ? vehiculo.IntermediarioExternoIndVEH2.Value.Value : false;

                                this.txtVlrVehiculoPrenda2.EditValue = vehiculo.PrecioVenta_2.Value;
                                this.chkCeroKMPrenda2.Checked = vehiculo.CeroKmInd_2.Value.Value;
                                this.txtMarcaPrenda2.Text = vehiculo.Marca_2.Value.ToString();
                                this.txtModeloPrenda2.Text = vehiculo.Modelo_2.Value.ToString();
                                this.txtMotorPrenda2.Text = vehiculo.Motor_2.Value.ToString();
                                this.txtSeriePrenda2.Text = vehiculo.Serie_2.Value.ToString();
                                this.txtClasePrenda2.Text = vehiculo.Clase_2.Value.ToString();
                                this.txtChasisPrenda2.Text = vehiculo.Chasis_2.Value.ToString();
                                this.txtPlacaPrenda2.Text = vehiculo.Placa_2.Value.ToString();
                                this.txtColorPrenda2.Text = vehiculo.Color_2.Value.ToString();
                                this.cmbServicioPrenda2.EditValue = vehiculo.Servicio_2.Value;
                                this.txtTipoVehiculoPrenda2.Text = vehiculo.Tipo_2.Value.ToString();
                                this.txtLineaPrenda2.Text = vehiculo.Linea_2.ToString();
                                
                                this.rbDocumentoBase2.SelectedIndex = vehiculo.DocPrenda_2 .Value.HasValue ? Convert.ToByte(vehiculo.DocPrenda_2 .Value) : Convert.ToByte(1);
                                this.txtDocumentoBaseDescr1_2.Text = vehiculo.NumeroFactura_2.Value;
                                #endregion
                            }

                        #region datos Hipoteca
                            this.txtHipoteca.Text = vehiculo.NumeroHipoteca.Value.ToString();
                            if (!string.IsNullOrWhiteSpace(this.txtHipoteca.Text))
                                if (this.txtHipoteca.Text != "0")
                                    this.btnHipoteca.Enabled = false;
                            this.chkDeudorHipoteca1.Checked = deudor.GaranteHipoteca1Ind.Value.HasValue ? deudor.GaranteHipoteca1Ind.Value.Value : false;
                            if (conyuge != null)
                            {
                                this.chkConyugeHipoteca1.Checked = conyuge.GaranteHipoteca1Ind.Value.HasValue ? conyuge.GaranteHipoteca1Ind.Value.Value : false;
                            }
                            if (codeudor1 != null)
                            {
                                this.chkCodeudor1Hipoteca1.Checked = codeudor1.GaranteHipoteca1Ind.Value.HasValue ? codeudor1.GaranteHipoteca1Ind.Value.Value : false;
                            }

                            if (codeudor2 != null)
                            {
                                this.chkCodeudor2Hipoteca1.Checked = codeudor2.GaranteHipoteca1Ind.Value.HasValue ? codeudor2.GaranteHipoteca1Ind.Value.Value : false;
                            }
                            this.masterAseguradoraHipoteca1.Value= vehiculo.Aseguradora1HIP.ToString();
                            this.txtVlrPolizaHipoteca1.EditValue = vehiculo.VlrPolizaHIP1.Value;
                            this.cmbTipoInmueble.EditValue = vehiculo.InmuebleTipo.Value;
                            this.txtDireccion.EditValue = vehiculo.Direccion.Value;
                            this.masterCiudadHipoteca.Value = vehiculo.Ciudad.ToString();
                            this.txtFMI.Text = vehiculo.Matricula.Value;
                            this.chkViviendaNueva.Checked = vehiculo.ViviendaNuevaInd.Value.Value;                          
                            this.dtFechaComercial.DateTime = vehiculo.FechaAvaluo.Value.HasValue ? vehiculo.FechaAvaluo.Value.Value : DateTime.Now;
                            this.txtValorComercial.EditValue = vehiculo.ValorAvaluo.Value;
                            this.dtFechaPredial.DateTime = vehiculo.FechaPredial.Value.HasValue ? vehiculo.FechaPredial.Value.Value : DateTime.Now;
                            this.txtValorPredial.EditValue = vehiculo.ValorPredial.Value;
                            this.dtFechaCompraventa.DateTime = vehiculo.FechaPromesa.Value.HasValue ? vehiculo.FechaPromesa.Value.Value : DateTime.Now;
                            this.txtValorCompraventa.EditValue = vehiculo.ValorCompraventa.Value;

                        #endregion
                            if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                            {
                                #region datos Hipoteca2
                                this.txtHipoteca2.Text = vehiculo.NumeroHipoteca_2.Value.ToString();
                                if (!string.IsNullOrWhiteSpace(this.txtHipoteca.Text))
                                    if (this.txtHipoteca.Text != "0")
                                        this.btnHipoteca.Enabled = false;
                                this.chkDeudorHipoteca2.Checked = deudor.GaranteHipoteca2Ind.Value.HasValue ? deudor.GaranteHipoteca2Ind.Value.Value : false;
                                if (conyuge != null)
                                {
                                    this.chkConyugeHipoteca2.Checked = conyuge.GaranteHipoteca2Ind.Value.HasValue ? conyuge.GaranteHipoteca2Ind.Value.Value : false;
                                }
                                if (codeudor1 != null)
                                {
                                    this.chkCodeudor1Hipoteca2.Checked = codeudor1.GaranteHipoteca2Ind.Value.HasValue ? codeudor1.GaranteHipoteca2Ind.Value.Value : false;
                                }

                                if (codeudor2 != null)
                                {
                                    this.chkCodeudor2Hipoteca2.Checked = codeudor2.GaranteHipoteca2Ind.Value.HasValue ? codeudor2.GaranteHipoteca2Ind.Value.Value : false;
                                }
                                this.masterAseguradoraHipoteca2.Value = vehiculo.Aseguradora2HIP.ToString();
                                this.txtVlrPolizaHipoteca2.EditValue = vehiculo.VlrPolizaHIP2.Value;

                                this.cmbTipoInmueble2.EditValue = vehiculo.InmuebleTipo_2.Value;
                                this.txtDireccion2.EditValue = vehiculo.Direccion_2.Value;
                                this.masterCiudadHipoteca2.Value = vehiculo.Ciudad_2.ToString();
                                this.txtFMI2.Text = vehiculo.Matricula_2.Value;
                                this.chkViviendaNueva2.Checked = vehiculo.ViviendaNuevaInd_2.Value.Value;
                                this.dtFechaComercial2.DateTime = vehiculo.FechaAvaluo_2.Value.HasValue ? vehiculo.FechaAvaluo_2.Value.Value : DateTime.Now;
                                this.txtValorComercial2.EditValue = vehiculo.ValorAvaluo_2.Value;
                                this.dtFechaPredial2.DateTime = vehiculo.FechaPredial_2.Value.HasValue ? vehiculo.FechaPredial_2.Value.Value : DateTime.Now;
                                this.txtValorPredial2.EditValue = vehiculo.ValorPredial_2.Value;
                                this.dtFechaCompraventa2.DateTime = vehiculo.FechaPromesa_2.Value.HasValue ? vehiculo.FechaPromesa_2.Value.Value : DateTime.Now;
                                this.txtValorCompraventa2.EditValue = vehiculo.ValorCompraventa_2.Value;
                                #endregion
                            }
                        #endregion                    
                    }
                    #region Datos Generales
                        this.txtDeudor.Text = this._data.SolicituDocu.Nombre.Value.ToString();
                        this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteID.Value.ToString();
                        this.masterVitrina.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                        this.masterVitrinaDesembolso.Value = this._data.SolicituDocu.Concesionario2.Value;
                        this.masterCiudadGeneral.Value = this._data.SolicituDocu.Ciudad.Value;
                        this.masterZona.Value = this._data.SolicituDocu.ZonaID.Value;
                    #endregion

                    #region Credito
                        this.txtPorCredito.EditValue = this._data.SolicituDocu.PorInteres.Value;
                        this.txtMontoSol.EditValue = this._data.SolicituDocu.VlrSolicitado.Value;
                        this.txtPlazoSol.EditValue = this._data.SolicituDocu.Plazo.Value;
                        this.txtVlrGarantia.EditValue = this._data.OtrosDatos.PF_VlrGarantiaFirma3.Value;
                    #endregion

                    #region Poliza
                        this.masterAseguradora.Value = this._data.SolicituDocu.AseguradoraID.Value;
                        this.txtVlrPoliza.EditValue = this._data.SolicituDocu.VlrPoliza.Value;
                        this.chkPolizaCancelaContadoGarantia.Checked = this._data.SolicituDocu.CancelaContadoPolizaInd.Value.HasValue ? this._data.SolicituDocu.CancelaContadoPolizaInd.Value.Value : false;
                        this.chkPolizaDiferenteCubriendo.Checked = this._data.SolicituDocu.IntermediarioExternoInd.Value.HasValue ? this._data.SolicituDocu.IntermediarioExternoInd.Value.Value : false;
                    #endregion
                    this.dtFechaFirmaDocumento.DateTime = otros.FechaFirmaDocumento.Value.HasValue ? otros.FechaFirmaDocumento.Value.Value : DateTime.Now;


                }
                else
                {
                    #region Llena datos de los controles para salvar

                    DTO_drSolicitudDatosPersonales deudorNew = deudor != null ? deudor : new DTO_drSolicitudDatosPersonales();
                    DTO_drSolicitudDatosPersonales conyugeNew = conyuge != null ? conyuge : new DTO_drSolicitudDatosPersonales();
                    DTO_drSolicitudDatosPersonales codeudor1New = codeudor1 != null ? codeudor1 : new DTO_drSolicitudDatosPersonales();
                    DTO_drSolicitudDatosPersonales codeudor2New = codeudor2 != null ? codeudor2 : new DTO_drSolicitudDatosPersonales();

                    this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value : 1;
                    DTO_drSolicitudDatosVehiculo vehiculoNew = vehiculo != null ? vehiculo : new DTO_drSolicitudDatosVehiculo();
                    DTO_drSolicitudDatosOtros otrosNew = otros != null ? otros : new DTO_drSolicitudDatosOtros();

                    #region Datos Generales
                        this._data.SolicituDocu.ConcesionarioID.Value = this.masterVitrina.Value;
                        this._data.SolicituDocu.Concesionario2.Value = this.masterVitrinaDesembolso.Value;
                        this._data.SolicituDocu.Ciudad.Value = this.masterCiudadGeneral.Value;
                        this._data.SolicituDocu.ZonaID.Value = this.masterZona.Value;
                    #endregion
                    #region Credito
                        this._data.SolicituDocu.PorInteres.Value = Convert.ToDecimal(this.txtPorCredito.EditValue);
                        this._data.SolicituDocu.VlrSolicitado.Value = Convert.ToDecimal(this.txtMontoSol.EditValue);
                        this._data.SolicituDocu.Plazo.Value = Convert.ToInt16(this.txtPlazoSol.EditValue);
                        this._data.OtrosDatos.PF_VlrGarantiaFirma3.Value = Convert.ToDecimal(this.txtVlrGarantia.EditValue);
                    #endregion
                    #region Poliza
                        this._data.SolicituDocu.AseguradoraID.Value = this.masterAseguradora.Value;
                        this._data.SolicituDocu.VlrPoliza.Value = Convert.ToInt32(this.txtVlrPoliza.EditValue);
                        this._data.SolicituDocu.VlrFinanciaSeguro.Value = Convert.ToInt32(this.txtVlrPoliza.EditValue);
                        this._data.SolicituDocu.CancelaContadoPolizaInd.Value = this.chkPolizaCancelaContadoGarantia.Checked;
                        this._data.SolicituDocu.IntermediarioExternoInd.Value = this.chkPolizaDiferenteCubriendo.Checked;
                    #endregion
                        this.dtFechaFirmaDocumento.DateTime = otros.FechaFirmaDocumento.Value.HasValue ? otros.FechaFirmaDocumento.Value.Value : DateTime.Now;

                    // Datos Vehiculo
                    #region datos Vehiculo
                        #region Datos Prenda1
                        if (!string.IsNullOrEmpty(this.txtPrenda.Text.ToString()))
                            vehiculoNew.NumeroPrenda.Value = Convert.ToInt32(this.txtPrenda.Text);
                        deudorNew.GarantePrenda1Ind.Value = this.chkDeudorPrenda1.Checked;
                        if (conyuge != null)
                        {
                            conyugeNew.GarantePrenda1Ind.Value = this.chkConyugePrenda1.Checked;
                        }
                        if (codeudor1 != null)
                        {
                            codeudor1New.GarantePrenda1Ind.Value = this.chkCodeudor1Prenda1.Checked;
                        }
                        if (codeudor2 != null)
                        {
                            codeudor2New.GarantePrenda1Ind.Value = this.chkCodeudor2Prenda1.Checked;
                        }
                        vehiculoNew.Aseguradora1VEH.Value = this.masterAseguradoraPrenda1.Value;
                        if (!string.IsNullOrEmpty(this.txtVlrPolizaPrenda1.EditValue.ToString()))
                            vehiculoNew.VlrPolizaVEH1.Value = Convert.ToDecimal(this.txtVlrPolizaPrenda1.EditValue);
                        else
                            vehiculoNew.VlrPolizaVEH1.Value = 0;                        
                        vehiculoNew.CancelaContadoPolizaIndVEH1.Value = this.chkPolizaCancelaContadoGarantiaPrenda1.Checked;
                        vehiculoNew.IntermediarioExternoIndVEH1.Value = this.chkPolizaCancelaContadoGarantiaPrenda1.Checked;

                        vehiculoNew.PrecioVenta.Value = Convert.ToDecimal(this.txtVlrVehiculoPrenda.EditValue);
                        vehiculoNew.CeroKmInd.Value = this.chkCeroKMPrenda.Checked;
                        vehiculoNew.Marca.Value = this.txtMarcaPrenda.Text;
                        vehiculoNew.Modelo.Value = Convert.ToInt32(this.txtModeloPrenda.Text);
                        vehiculoNew.Motor.Value = this.txtMotorPrenda.Text;
                        vehiculoNew.Serie.Value = this.txtSeriePrenda.Text;
                        vehiculoNew.Clase.Value = this.txtClasePrenda.Text;
                        vehiculoNew.Chasis.Value = this.txtChasisPrenda.Text;
                        vehiculoNew.Placa.Value = this.txtPlacaPrenda.Text;
                        vehiculoNew.Color.Value = this.txtColorPrenda.Text;
                        vehiculoNew.Servicio.Value = Convert.ToByte(this.cmbServicioPrenda.EditValue);
                        vehiculoNew.Tipo.Value = this.txtTipoVehiculoPrenda.Text;
                        vehiculoNew.Linea.Value = this.txtLineaPrenda.Text;
                        vehiculoNew.DocPrenda.Value= Convert.ToByte(this.rbDocumentoBase.SelectedIndex);
                        vehiculoNew.NumeroFactura.Value = this.txtDocumentoBaseDescr1.Text;
                        #endregion
                        if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                        {

                            #region Datos Prenda2
                            if (!string.IsNullOrEmpty(this.txtPrenda2.Text.ToString()))
                                vehiculoNew.NumeroPrenda_2.Value = Convert.ToInt32(this.txtPrenda2.Text);

                            deudorNew.GarantePrenda2Ind.Value = this.chkDeudorPrenda2.Checked;
                            if (conyuge != null)
                            {
                                conyugeNew.GarantePrenda2Ind.Value = this.chkConyugePrenda2.Checked;
                            }
                            if (codeudor1 != null)
                            {
                                codeudor1New.GarantePrenda2Ind.Value = this.chkCodeudor1Prenda2.Checked;
                            }
                            if (codeudor2 != null)
                            {
                                codeudor2New.GarantePrenda2Ind.Value = this.chkCodeudor2Prenda2.Checked;
                            }
                            vehiculoNew.Aseguradora2VEH.Value = this.masterAseguradoraPrenda2.Value;
                            if (!string.IsNullOrEmpty(this.txtVlrPolizaPrenda2.EditValue.ToString()))
                                vehiculoNew.VlrPolizaVEH2.Value = Convert.ToDecimal(this.txtVlrPolizaPrenda2.EditValue);
                            else
                                vehiculoNew.VlrPolizaVEH2.Value = 0;                        
                            vehiculoNew.CancelaContadoPolizaIndVEH2.Value = this.chkPolizaCancelaContadoGarantiaPrenda2.Checked;
                            vehiculoNew.IntermediarioExternoIndVEH2.Value = this.chkPolizaCancelaContadoGarantiaPrenda2.Checked;

                            if (!string.IsNullOrEmpty(this.txtVlrVehiculoPrenda2.EditValue.ToString()))
                                vehiculoNew.PrecioVenta_2.Value = Convert.ToDecimal(this.txtVlrVehiculoPrenda2.EditValue);
                            else
                                vehiculoNew.PrecioVenta_2.Value = 0;                        

                            vehiculoNew.CeroKmInd_2.Value = this.chkCeroKMPrenda2.Checked;
                            vehiculoNew.Marca_2.Value = this.txtMarcaPrenda2.Text;
                            vehiculoNew.Modelo_2.Value = Convert.ToInt32(this.txtModeloPrenda2.Text);
                            vehiculoNew.Motor_2.Value = this.txtMotorPrenda2.Text;
                            vehiculoNew.Serie_2.Value = this.txtSeriePrenda2.Text;
                            vehiculoNew.Clase_2.Value = this.txtClasePrenda2.Text;
                            vehiculoNew.Chasis_2.Value = this.txtChasisPrenda2.Text;
                            vehiculoNew.Placa_2.Value = this.txtPlacaPrenda2.Text;
                            vehiculoNew.Color_2.Value = this.txtColorPrenda2.Text;
                            vehiculoNew.Servicio_2.Value = Convert.ToByte(this.cmbServicioPrenda2.EditValue);
                            vehiculoNew.Tipo_2.Value = this.txtTipoVehiculoPrenda2.Text;
                            vehiculoNew.Linea_2.Value = this.txtLineaPrenda2.Text;
                            vehiculoNew.DocPrenda_2.Value = Convert.ToByte(this.rbDocumentoBase2.SelectedIndex);
                            vehiculoNew.NumeroFactura_2.Value = this.txtDocumentoBaseDescr1_2.Text;
                            #endregion
                        }
                        #region Datos Hipoteca1
                        if (!string.IsNullOrEmpty(this.txtHipoteca.Text.ToString()))
                            vehiculoNew.NumeroHipoteca.Value = Convert.ToInt32(this.txtHipoteca.Text);

                        deudorNew.GaranteHipoteca1Ind.Value = this.chkDeudorHipoteca1.Checked;
                        if (conyuge != null)
                        {
                            conyugeNew.GaranteHipoteca1Ind.Value = this.chkConyugeHipoteca1.Checked;
                        }
                        if (codeudor1 != null)
                        {
                            codeudor1New.GaranteHipoteca1Ind.Value = this.chkCodeudor1Hipoteca1.Checked;
                        }
                        if (codeudor2 != null)
                        {
                            codeudor2New.GaranteHipoteca1Ind.Value = this.chkCodeudor2Hipoteca1.Checked;
                        }
                    
                        vehiculoNew.Aseguradora1HIP.Value = this.masterAseguradoraHipoteca1.Value;
                        if (!string.IsNullOrEmpty(this.txtVlrPolizaHipoteca1.EditValue.ToString()))
                            vehiculoNew.VlrPolizaHIP1.Value = Convert.ToDecimal(this.txtVlrPolizaHipoteca1.EditValue);
                        else
                            vehiculoNew.VlrPolizaHIP1.Value = 0;

                        vehiculoNew.InmuebleTipo.Value = Convert.ToByte(this.cmbTipoInmueble.EditValue);
                        vehiculoNew.Direccion.Value = this.txtDireccion.Text;
                        vehiculoNew.Matricula.Value = this.txtFMI.Text;
                        vehiculoNew.ViviendaNuevaInd.Value = this.chkViviendaNueva.Checked;
                        vehiculoNew.FechaAvaluo.Value = this.dtFechaComercial.DateTime;
                        vehiculoNew.ValorAvaluo.Value = Convert.ToDecimal(this.txtValorComercial.EditValue);
                        vehiculoNew.FechaAvaluo.Value = this.dtFechaPredial.DateTime;
                        vehiculoNew.ValorPredial.Value = Convert.ToDecimal(this.txtValorPredial.EditValue);
                        vehiculoNew.FechaAvaluo.Value = this.dtFechaCompraventa.DateTime;
                        vehiculoNew.ValorCompraventa.Value = Convert.ToDecimal(this.txtValorCompraventa.EditValue);
                        #endregion 
                        if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                        {
                            #region Datos Hipoteca2
                            if (!string.IsNullOrEmpty(this.txtHipoteca2.Text.ToString()))
                                vehiculoNew.NumeroHipoteca_2.Value = Convert.ToInt32(this.txtHipoteca2.Text);

                            deudorNew.GaranteHipoteca2Ind.Value = this.chkDeudorHipoteca2.Checked;
                            if (conyuge != null)
                            {
                                conyugeNew.GaranteHipoteca2Ind.Value = this.chkConyugeHipoteca2.Checked;
                            }
                            if (codeudor1 != null)
                            {
                                codeudor1New.GaranteHipoteca2Ind.Value = this.chkCodeudor1Hipoteca2.Checked;
                            }
                            if (codeudor2 != null)
                            {
                                codeudor2New.GaranteHipoteca2Ind.Value = this.chkCodeudor2Hipoteca2.Checked;
                            }
                            vehiculoNew.Aseguradora2HIP.Value = this.masterAseguradoraHipoteca2.Value;
                            vehiculoNew.VlrPolizaHIP2.Value = Convert.ToDecimal(this.txtVlrPolizaHipoteca2.EditValue);

                            vehiculoNew.InmuebleTipo_2.Value = Convert.ToByte(this.cmbTipoInmueble2.EditValue);
                            vehiculoNew.Direccion_2.Value = this.txtDireccion2.Text;
                            vehiculoNew.Matricula_2.Value = this.txtFMI2.Text;
                            vehiculoNew.ViviendaNuevaInd_2.Value = this.chkViviendaNueva2.Checked;
                            vehiculoNew.FechaAvaluo_2.Value = this.dtFechaComercial2.DateTime;
                            vehiculoNew.ValorAvaluo_2.Value = Convert.ToDecimal(this.txtValorComercial2.EditValue);
                            vehiculoNew.FechaAvaluo_2.Value = this.dtFechaPredial2.DateTime;
                            vehiculoNew.ValorAvaluo_2.Value = Convert.ToDecimal(this.txtValorPredial2.EditValue);
                            vehiculoNew.FechaAvaluo_2.Value = this.dtFechaCompraventa2.DateTime;
                            vehiculoNew.ValorAvaluo_2.Value = Convert.ToDecimal(this.txtValorCompraventa2.EditValue);
                            #endregion

                        }
                        deudorNew.Consecutivo.Value = deudor != null && deudor.Consecutivo.Value.HasValue ? deudor.Consecutivo.Value : null;
                        deudorNew.Version.Value = deudor != null ? deudor.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                        this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 1);
                        this._data.DatosPersonales.Add(deudorNew);                   
                   
                        if (conyuge != null)
                        {
                            conyugeNew.Consecutivo.Value = conyuge != null && conyuge.Consecutivo.Value.HasValue ? conyuge.Consecutivo.Value : null;
                            conyugeNew.Version.Value = conyuge != null ? conyuge.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                            this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 2);
                            this._data.DatosPersonales.Add(conyugeNew);
                        }

                        if (codeudor1 != null)
                        {
                            codeudor1New.Consecutivo.Value = codeudor1 != null && codeudor1.Consecutivo.Value.HasValue ? codeudor1.Consecutivo.Value : null;
                            codeudor1New.Version.Value = codeudor1 != null ? codeudor1.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                            this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 3);
                            this._data.DatosPersonales.Add(codeudor1New);

                        }

                        if (codeudor2 != null)
                        {
                            codeudor2New.Consecutivo.Value = codeudor2 != null && codeudor2.Consecutivo.Value.HasValue ? codeudor2.Consecutivo.Value : null;
                            codeudor2New.Version.Value = codeudor2 != null ? codeudor2.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                            this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 4);
                            this._data.DatosPersonales.Add(codeudor2New);

                        }
                        otros.FechaFirmaDocumento.Value=this.dtFechaFirmaDocumento.DateTime;

                        this._data.DatosVehiculo = vehiculoNew;
                        this._data.OtrosDatos = otrosNew;

                    #endregion 
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "AssignValues"));
            }
        }

        private void CleanData()
        {

            this.txtDeudor.Text = String.Empty;
            this.txtCedulaDeudor.Text = String.Empty;
            this.masterCiudadGeneral.Value = this._lugGeoXDef;
            this.masterVitrina.Value = this._VitrinaXDef;
            this.masterZona.Value = this._ZonaXDef;


            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._data = new DTO_DigitaSolicitudDecisor();

            //Variables
            this._clienteID = String.Empty;
            this._libranzaID = 0;

            FormProvider.Master.itemSave.Enabled = true;
        }

        private void EnableHeader(bool enabled)
        {
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {

            if (!this._verifica)
            {
                this._documentID = AppDocuments.LegalizacionSolicitud;
            }
            else
                this._documentID = AppDocuments.RevisionLegalizacionSolicitud;

            this.txtVlrVehiculoPrenda.Enabled = false;
            this.txtMarcaPrenda.Enabled = false;
            this.txtModeloPrenda.Enabled = false;
            this.cmbServicioPrenda.Enabled = false;
            this.txtLineaPrenda.Enabled = false;

            this.cmbTipoInmueble.Enabled = false;
            this.txtDireccion.Enabled = false;
            this.masterCiudadHipoteca.Enabled = false;
            this.masterCiudadHipoteca2.Enabled = false;
            this.txtFMI.Enabled = false;
            this.chkViviendaNueva.Enabled = false;

            this.dtFechaPredial.Enabled = false;
            this.dtFechaComercial.Enabled = false;
            this.dtFechaCompraventa.Enabled = false;
            this.txtValorPredial.Enabled = false;
            this.txtValorComercial.Enabled = false;
            this.txtValorCompraventa.Enabled = false;

//            this._documentID = AppDocuments.LegalizacionSolicitud;
            this._frmModule = ModulesPrefix.dr;

            this._allowEditValuesInd =  true;
            this._userAutoriza = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_UsuarioCambiosConPassword);
            if (string.IsNullOrEmpty(this._userAutoriza))
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No existe el usuario de autorización"));

            if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                this._allowEditValuesInd = true;


            //Crea las opciones del combo servicio
            servicio.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Particular"));
            servicio.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Publico"));

            //Crea las opciones del combo garante
            garante.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Deudor(a)"));
            garante.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Conyugue"));
            garante.Add(3, this._bc.GetResource(LanguageTypes.Tables, "Ambos"));

            //Crea las opciones del tipo de Reporte
            tipoReporte.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Solicitud de Credito"));// Formato
            tipoReporte.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Info de Asegurabilidad"));
            tipoReporte.Add(3, this._bc.GetResource(LanguageTypes.Tables, "Condiciones Generales"));
            tipoReporte.Add(4, this._bc.GetResource(LanguageTypes.Tables, "Pagare Credito"));
            tipoReporte.Add(5, this._bc.GetResource(LanguageTypes.Tables, "Carta Inst Pagare Credito"));
            tipoReporte.Add(6, this._bc.GetResource(LanguageTypes.Tables, "Pagare Seguros"));
            tipoReporte.Add(7, this._bc.GetResource(LanguageTypes.Tables, "Carta Inst Pagare Seguros"));

            tipoReporte.Add(9, this._bc.GetResource(LanguageTypes.Tables, "Condiciones Especificas"));// formato Impreso
            tipoReporte.Add(10, this._bc.GetResource(LanguageTypes.Tables, "Cert Grupo Deudores"));
            tipoReporte.Add(11, this._bc.GetResource(LanguageTypes.Tables, "Carta Envio Prendas"));

//            this.tipoInmueble = new Dictionary<string, string>();
            tipoInmueble.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v0"));
            tipoInmueble.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v1"));
            tipoInmueble.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v2"));
            tipoInmueble.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v3"));
            tipoInmueble.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v4"));
            tipoInmueble.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v5"));
            tipoInmueble.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v6"));


            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadGeneral, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraPrenda1, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraPrenda2, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraHipoteca1, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraHipoteca2, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradora, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadHipoteca, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadHipoteca2, AppMasters.glLugarGeografico, false, true, true, false);

            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, false, true, true, false);

            this.cmbServicioPrenda.Properties.ValueMember = "Key";
            this.cmbServicioPrenda.Properties.DisplayMember = "Value";
            this.cmbServicioPrenda.Properties.DataSource = servicio;
            this.cmbServicioPrenda.EditValue = "0";

            //this.cmbGarante1.Properties.ValueMember = "Key";
            //this.cmbGarante1.Properties.DisplayMember = "Value";
            //this.cmbGarante1.Properties.DataSource = garante;
            //this.cmbGarante1.EditValue = "1";

            this.cmbReportes.Properties.ValueMember = "Key";
            this.cmbReportes.Properties.DisplayMember = "Value";
            this.cmbReportes.Properties.DataSource = tipoReporte;
            this.cmbReportes.EditValue = "0";


            //// Oculta Garante2
            //this.txtNombreGarante2.Visible = false;
            //this.txtDireccionGarante2.Visible = false;
            //this.masterCiudadGarante2.Visible = false;
            //this.masterCedulaGarante2.Visible = false;

            /// Oculta label Prenda

//            this.lblPrendaGenerada.Visible = false;

            // Deja en Modo Lectura datos Generales

            this.txtDeudor.ReadOnly = true;
            this.txtCedulaDeudor.ReadOnly = true;
            this.masterVitrina.Enabled = false;
            this.masterZona.Enabled = false;
            this.masterCiudadGeneral.Enabled = false;
            //this.txtMarca.ReadOnly = true;
            //this.txtLinea.ReadOnly = true;
            //this.txtModelo.ReadOnly = true;
            //this.cmbServicioGeneral.ReadOnly = true;
            //this.chkCeroKMGeneral.Enabled = false;

            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            //Carga los mensajes de la grilla
            this.msgFinDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFinDoc);
            this.EnableHeaderNuevo(false);
            this._cuentaAbonoCap = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaPagosAbonosDeuda);

            //Ultimo consecutivo
            string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
            //this.txtUltSolicitud.Text = consecutivoSolStr;
        }


        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeaderNuevo(bool enabled)
        {
            try
            {
                //Datos Generales


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData(bool cleanObligaciones)
        {
            try
            {
                this.validateData = false;
      
                this.txtPorCredito.EditValue = "0";
                //Footer
                this._ctrl = new DTO_glDocumentoControl();
                this._headerSolicitud = new DTO_ccSolicitudDocu();
                this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                this._liquidador = new DTO_PlanDePagos();
                this._planPagos = new List<DTO_Cuota>();

                //Variables
                this.vlrLibranza = 0;
                this.vlrGiro = 0;
                this._clienteID = String.Empty;
                this._lineaCreditoID = String.Empty;
                this._polizaID = string.Empty;
                this.compsNuevoValor = new Dictionary<string, decimal>();

                //Ultimo Consecutivo
                string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
                //      this.txtUltSolicitud.Text = consecutivoSolStr;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "CleanData"));
            }
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            try
            {
                #region Hace las Validaciones

                //Valida que el tipo de credito exista
        

                ////Valida la info de la poliza
                //if (!String.IsNullOrWhiteSpace(this.txtPrenda.Text))
                //{
                //    //Valida el valor solicitado de la poliza
                //    if ((string.IsNullOrWhiteSpace(this.txtVlrSolicitadoPoliza.Text) || Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue) == 0) &&
                //        this._poliza != null && this._poliza.FinanciadaIND.Value.Value)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                //        MessageBox.Show(msg);
                //        return false;
                //    }

                //    //Valida el valor de la poliza
                //    if ((string.IsNullOrWhiteSpace(this.txtVlrPoliza.Text) || Convert.ToDecimal(this.txtVlrPoliza.EditValue) == 0) &&  
                //        this._poliza != null && this._poliza.FinanciadaIND.Value.Value)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrPoliza.Text);
                //        MessageBox.Show(msg);
                //        return false;
                //    }

                //    //Agregar un indicador para ver si la info de la póliza es válida
                //}

                #endregion         
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "ValidateHeader"));
                return false;
            }

            return true;
        }
        private bool ValidatePrenda1()
        {
            string result = string.Empty;

            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);

            #region Hace las Validaciones

            #region Datos Deudor o Prendario
            if (this.chkDeudorPrenda1.Checked == false && this.chkDeudorHipoteca1.Checked == false && this.chkDeudorPrenda2.Checked == false && this.chkDeudorHipoteca2.Checked == false)
            {
                result += string.Format(msgVacio, "No ha seleccionado Deudor o Garante") + "\n";
                this.chkDeudorPrenda1.Focus();
            }
            #endregion

            #region Poliza
            if (string.IsNullOrEmpty(this.masterAseguradoraPrenda1.Value))
            {
                result += string.Format(msgVacio, "No ha Seleccionado Aseguradora" +"Prenda 1") + "\n";
                this.masterAseguradoraPrenda1.Focus();
            }
            if (string.IsNullOrEmpty(this.txtVlrPolizaPrenda1.Text) && Convert.ToDecimal(this.txtVlrPolizaPrenda1.EditValue)<=0)
            {
                result += string.Format(msgVacio, "No ha digitado Valor Poliza " + " Prenda 1") + "\n";
                this.txtVlrPolizaPrenda1.Focus();
            }
            #endregion


            if (!string.IsNullOrEmpty(result))
            {

                MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
                return false;

            }
            else
                return true;

            #endregion
            #region glDocumentoControl
            this._ctrl.PeriodoDoc.Value = this.periodo;
            this._ctrl.PeriodoUltMov.Value = this.periodo;
            this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion            
            #endregion

            return true;
        }


        /// <summary>
        /// Funcion que carga los valores previamente guardados
        /// </summary>
        private void SetValuesFromSolicitud()
        {
            try
            {
                if (this._data != null && this._data.SolicituDocu != null)
                {
                    this._clienteID = !string.IsNullOrEmpty(this._data.SolicituDocu.ClienteID.Value) ? this._data.SolicituDocu.ClienteID.Value : this._data.SolicituDocu.ClienteRadica.Value;
                    this._cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.ccCliente,false,this._clienteID,true);

                    this.vlrSolicitadoPrestamo = Convert.ToInt32(this._data.SolicituDocu.VlrSolicitado.Value);
                    int vlrGiro = Convert.ToInt32(this._data.SolicituDocu.VlrGiro.Value);
                    this._lineaCreditoID = this._data.SolicituDocu.LineaCreditoID.Value;
                    this._plazo = this._data.SolicituDocu.Plazo.Value.Value;
                    this.porInteres = 0; 
                    this._libranzaID = this._data.SolicituDocu.Libranza.Value.Value;
                    this._polizaID = this._data.SolicituDocu.Poliza.Value;
                    this._data.SolicituDocu.PorInteres.Value = this._data.SolicituDocu.PorInteres.Value ?? 0;
                    this.txtPorCredito.EditValue = this._data.SolicituDocu.PorInteres.Value;
                }
                ////Se carga al final para autocalcular el valor del giro
                //#endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "GetValuesSolicitud"));             
            }
        }

        /// <summary>
        /// Funcion que trae el valor de los interes de los componentes
        /// </summary>
        private void GetIntereses(List<DTO_ccSolicitudComponentes> componentes)
        {
            string compInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
            string compInteresSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            foreach (DTO_ccSolicitudComponentes componente in componentes)
            {
                if (componente.ComponenteCarteraID.Value == compInteres)
                {
                    if (Convert.ToDecimal(this.txtPorCredito.EditValue) != 0)
                        this.porInteres = Convert.ToDecimal(this.txtPorCredito.EditValue);
                    else
                        this.porInteres = componente.Porcentaje.Value.HasValue ? componente.Porcentaje.Value.Value : 0;
                }
                else if (componente.ComponenteCarteraID.Value == compInteresSeguro)
                {
                        this.porInteresPoliza = componente.Porcentaje.Value.HasValue ? componente.Porcentaje.Value.Value : 0;
                }
            }
        }

        ///<summary>
        ///Metodo que calcula el valor de la compra de la cartera
        ///</summary>
        private void CalcularGiro()
        {
            try
            {

                
                decimal vlrTotalCompra = this._data.SolicituDocu.CompraCarteraInd.Value.Value ? (from p in this._compCartera select p.VlrSaldo.Value.Value).Sum() : 0;
                // this.txtVlrGiro.EditValue = Convert.ToDecimal(this.txtVlrGiro.EditValue) - vlrTotalCompra - dto1Cuota;
                this.vlrGiro= _liquidador.VlrPrestamo - vlrTotalCompra - _liquidador.VlrDescuento;
                this._liquidador.VlrGiro = Convert.ToInt32(this.vlrGiro);
                //this.vlrGiro = this._liquidador.VlrGiro;
                //this.txtVlrCompra.EditValue = vlrTotalCompra;

                //this.txtVlrGiro.EditValue = this.vlrGiro;

                //decimal vlrTotalCompra = this.chkCompraCartera.Checked ? (from p in this._compCartera select p.VlrSaldo.Value.Value).Sum() : 0;
                //this.txtVlrGiro.EditValue = Convert.ToDecimal(this.txtVlrGiro.EditValue) - vlrTotalCompra;
                //this.txtVlrCompra.EditValue = vlrTotalCompra;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "CalcularGiro"));
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateCalcularLiquidacion()
        {
            try
            {
                liquidaInd = true;              

                if (this.liquidaAll)
                {                  
                    //Valida que el interes del credito sea valido
                    if (Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture) <= 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrPoliza.Text);
                        MessageBox.Show(msg);
                        this.txtPorCredito.Focus();
                        liquidaInd = false;
                    }                 
                }

                ////Valida que el valor de la poliza sea positivo
                //if (Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture) == 0)
                //{
                //    if (!this.liquidaAll)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                //        MessageBox.Show(msg);
                //        this.txtVlrSolicitado.Focus();
                //        liquidaInd = false;
                //    }
                //    else
                //    {
                //        if (this._tipoCredito == TipoCredito.PolizaRenueva || this._tipoCredito == TipoCredito.PolizaSinCredito || !string.IsNullOrEmpty(this.txtPoliza.Text))
                //        {
                //            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                //            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_CreditoSinPoliza);
                //            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                //            {
                //                this.txtVlrSolicitadoPoliza.Focus();
                //                liquidaInd = false;
                //            } 
                //        }
                //    }
                //}
                //else if (!liquidaAll && Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture) < 0)
                //{
                //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                //    MessageBox.Show(msg);
                //    this.txtVlrSolicitado.Focus();
                //    liquidaInd = false;
                //}
                //else
                //{
                //    //Valida que el interes de la poliza sea valido
                //    if (Convert.ToDecimal(this.txtInteresPoliza.EditValue, CultureInfo.InvariantCulture) < 0)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblInteresPol.Text);
                //        MessageBox.Show(msg);
                //        this.txtInteresPoliza.Focus();
                //        liquidaInd = false;
                //    }

                //    //Valida que el plazo de la poliza sea valido
                //    if (string.IsNullOrWhiteSpace(this.comboPlazoPol.Text))
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazoPol.Text);
                //        MessageBox.Show(msg);
                //        this.txtPlazo.Focus();
                //        liquidaInd = false;
                //    }

                //    //Valida que el plazo de la poliza sea valido
                //    if (string.IsNullOrWhiteSpace(this.txtCta1Pol.Text))
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblCta1Pol.Text);
                //        MessageBox.Show(msg);
                //        this.txtPlazo.Focus();
                //        liquidaInd = false;
                //    }

                //    //Valida que el plazo de la poliza sea valido
                //    if (Convert.ToInt32(this.txtCta1Pol.Text) <= 0)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblCta1Pol.Text);
                //        MessageBox.Show(msg);
                //        this.txtPlazo.Focus();
                //        liquidaInd = false;
                //    }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "ValidateGetLiquidacion"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {

            }
        }

        
        //oscar
        #region Lista de Chequeo
        /// <summary>
        /// Agrega las columnas 
        /// </summary>
        protected virtual void AddChequeoCols()
        {
            try
            {
                //Descripcion
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, "TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 1;
                TerceroID.Width = 70;
                TerceroID.Visible = false;
                TerceroID.OptionsColumn.AllowEdit = false;
                TerceroID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvActividades.Columns.Add(TerceroID);

                //Observacion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, "Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 2;
                Descripcion.Width = 130;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvActividades.Columns.Add(Descripcion);


                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, "Observación");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 3;
                Observacion.Width = 100;
                Observacion.Visible = true;
                Observacion.ColumnEdit = this.richTextTareas1;
                Observacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Observacion.OptionsColumn.AllowEdit = true;
                this.gvActividades.Columns.Add(Observacion);

                //IncluidoInd
                GridColumn IncluidoInd = new GridColumn();
                IncluidoInd.FieldName = this._unboundPrefix + "IncluidoInd";
                IncluidoInd.Caption = "√ OK";
                IncluidoInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoInd.VisibleIndex = 4;
                IncluidoInd.Width = 15;
                IncluidoInd.Visible = true;
                IncluidoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, "OK");
                IncluidoInd.AppearanceHeader.ForeColor = Color.Lime;
                IncluidoInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                IncluidoInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                IncluidoInd.AppearanceHeader.Options.UseTextOptions = true;
                IncluidoInd.AppearanceHeader.Options.UseFont = true;
                IncluidoInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvActividades.Columns.Add(IncluidoInd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Ratificacion.cs", "AddColumsActividades"));
            }
        }

        /// <summary>
        /// Carga la informacion de la grilla de actividades
        /// </summary>
        private void LoadActividades()
        {
            try
            {
                if (this._actFlujoLegalizacion != null)
                {
                    this.actChequeo = this._bc.AdministrationModel.drSolicitudDatosChequeados_GetByActividadNumDoc(this._actFlujoLegalizacion.ID.Value, this._ctrl.NumeroDoc.Value.Value, this._data.SolicituDocu.VersionNro.Value.Value);                    
                    //this._actividadChequeo = this._bc.AdministrationModel.glDocumentoChequeoLista_GetByNumeroDoc(this._ctrl.NumeroDoc.Value.Value);
                    //if (this._actividadChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujoLegalizacion.ID.Value);
                        

                        //this.actChequeoBase = _bc.AdministrationModel.glActividadChequeoLista_GetByActividaddrActividadChequeoLista_GetByActividad(this._actFlujo.ID.Value);
                        foreach (DTO_drActividadChequeoLista basic in this.actChequeoBase)
                        {
                            DTO_glDocumentoChequeoLista chequeo = new DTO_glDocumentoChequeoLista();

                            chequeo.NumeroDoc.Value = _ctrl.NumeroDoc.Value.Value;
                            chequeo.ActividadFlujoID.Value = basic.ActividadFlujoID.Value;
                            chequeo.Descripcion.Value = basic.Descripcion.Value;
                            
                            chequeo.EmpleadoInd.Value = basic.EmpleadoInd.Value.Value;
                            chequeo.PrestServiciosInd.Value = basic.PrestServiciosInd.Value.Value;
                            chequeo.TransportadorInd.Value = basic.TransportadorInd.Value.Value;
                            chequeo.IndependienteInd.Value = basic.IndependienteInd.Value.Value;
                            chequeo.PensionadoInd.Value = basic.PensionadoInd.Value.Value;
                            chequeo.IncluidoDeudor.Value = true;
                            chequeo.Consecutivo.Value = basic.consecutivo.Value.Value;
                            if (this.actChequeo == null || this.actChequeo.Count == 0)                            
                                chequeo.IncluidoInd.Value = false;
                            else
                                if(!this._verifica)
                                    chequeo.IncluidoInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 1 ? x.ChequeadoInd.Value.Value : false));
                                else
                                    chequeo.IncluidoInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 1 ? x.VerficadoInd.Value.Value : false));
                            this._actividadChequeo.Add(chequeo);
                        }
                    }
                }
                this.gcActividades.DataSource = this._actividadChequeo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnalisisRiesgo.cs", "LoadTareas"));
            }
        }

        #region Eventos Grilla Lista Chequeo

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvActividades_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "DateTime")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "DateTime")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double"
                        || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime")
                    {
                        e.Value = e.Value.ToString();//pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                    }
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double"
                            || fi.FieldType.Name == "Decimal" || fi.FieldType.Name == "DateTime")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvActividades_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Observacion")
            {
                e.RepositoryItem = this.riPopupTareas;
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupActividad_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvActividades.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControlTareas.Document.Text = this.gvActividades.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopupActividad_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControlTareas.Document.Text;
        }

        #endregion      
        #endregion
        #endregion Funciones Privadas

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemUpdate.Visible = true;
                    FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Visible = true;
                    FormProvider.Master.itemPrint.Enabled = true;
                    FormProvider.Master.itemUpdate.Visible = false;
                    FormProvider.Master.itemSave.Enabled = true;
                    if (!this._verifica)
                    {
                        FormProvider.Master.itemEdit.Enabled = true;
                        FormProvider.Master.itemEdit.Visible = true;
                    }
                    else
                    {
                        FormProvider.Master.itemEdit.Enabled = false;
                        FormProvider.Master.itemEdit.Visible = false;
                    }
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar Desembolso";

                }
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSendtoAppr.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que calcula nuevamente los valores de la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrSolicitado_Leave(object sender, EventArgs e)
        {
            try
            {
                //decimal vlrSolicitadoTemp = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                //if (validateData && this.vlrSolicitadoPrestamo != vlrSolicitadoTemp)
                //{
                //    this._lineaCreditoID = string.Empty;
                //    this.btn_Liquidar_Click(sender, e);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "txtVlrSolicitado_Leave"));
            }
        }

        /// <summary>
        /// Evento que se encarga de calcular la liquidacion del credito
        /// </summary>
        private void btn_Liquidar_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateCalcularLiquidacion();
                if (this.liquidaInd)
                {
                    this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                    this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                    bool hasLiquidacion = true;

                    if (this._data != null && this._data.SolicituDocu != null)
                    {
                        this._tipoCreditoID = this.masterVitrina.Value;
                        TimeSpan difFecha = DateTime.Now.Date.Subtract(this._cliente.FechaNacimiento.Value.Value);
                        this.edad = (int)Math.Floor((double)difFecha.Days / 365);
                        this.porInteres = Convert.ToDecimal(this.txtPorCredito.Text.Replace("%", string.Empty));
                        DTO_ccTipoCredito tipoDTO = (DTO_ccTipoCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, false, this._data.SolicituDocu.TipoCreditoID.Value, true);
                        this._tipoCredito = (TipoCredito)Enum.Parse(typeof(TipoCredito), tipoDTO.TipoCredito.Value.Value.ToString());
                        this.liquidaAll = true;
                        this._plazopoliza = 12;
                        if (this.chkPolizaCancelaContadoGarantia.Checked )//|| this.chkPolizaDiferenteCubriendo.Checked)
                        {
                            this.txtVlrPoliza.EditValue = 0;
                            this.porInteresPoliza = 0;
                            this._plazopoliza = 0;
                        }

                        object res = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._lineaCreditoID, this.vlrSolicitadoPrestamo,Convert.ToInt32(this.txtVlrPoliza.EditValue),
                                      Convert.ToInt32(this.vlrGiro), this._plazo, this._plazopoliza, this.edad, DateTime.Now.Date,
                                      this._data.SolicituDocu.FechaCuota1.Value.HasValue ? this._data.SolicituDocu.FechaCuota1.Value.Value : DateTime.Now.Date, this.porInteres, this.porInteresPoliza,
                                      1, 0, this.liquidaAll, new List<DTO_Cuota>(), this.compsNuevoValor, this._data.SolicituDocu.NumeroDoc.Value.Value, this._data.SolicituDocu.TipoCreditoID.Value, false);

                        if (res.GetType() == typeof(DTO_TxResult))
                        {
                            hasLiquidacion = false;
                            MessageForm msg = new MessageForm((DTO_TxResult)res);
                            msg.ShowDialog();
                            this.isValid = false;
                        }
                        else
                        {
                            this.isValid = true;
                            this._liquidador = (DTO_PlanDePagos)res;
                            this._data.SolicituDocu.LiquidaAll.Value = true;
                            this._data.SolicituDocu.FechaVto.Value = this._ctrl.FechaDoc.Value.Value.AddMonths(_data.SolicituDocu.Plazo.Value.Value);

                                    res = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._lineaCreditoID, this.vlrSolicitadoPrestamo, Convert.ToInt32(this.txtVlrPoliza.EditValue),
                                          Convert.ToInt32(this.vlrGiro), this._plazo, this._plazopoliza, this.edad, DateTime.Now.Date,
                                          this._data.SolicituDocu.FechaCuota1.Value.HasValue ? this._data.SolicituDocu.FechaCuota1.Value.Value : DateTime.Now.Date, this.porInteres, this.porInteresPoliza,
                                          1, 0, this.liquidaAll, new List<DTO_Cuota>(), this.compsNuevoValor, this._data.SolicituDocu.NumeroDoc.Value.Value, this._data.SolicituDocu.TipoCreditoID.Value, false);


                            this._liquidador = (DTO_PlanDePagos)res;

                            if (this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado)
                            {
                                #region Credito Nuevo

                                #region Carga las variables y calcula el interes
                                this._componentesVisibles = this._liquidador.ComponentesUsuario;
                                this._componentesContab = this._liquidador.ComponentesAll;
                                this._planPagos = this._liquidador.Cuotas;

                                this.GetIntereses(this._componentesVisibles);
                                #endregion
                                #region Asigna los valores calculados

                                //Valores Credito
                                //this.vlrGiro = this._liquidador.VlrGiro;
                                //this.txtVlrAdicional.EditValue = this._liquidador.VlrAdicional;
                                //this.txtVlrCompra.EditValue = this._liquidador.VlrCompra;
                                //this.txtVlrGiro.EditValue = this._liquidador.VlrGiro;
                                //this.txtVlrPrestamo.EditValue = this._liquidador.VlrPrestamo;
                                //this.txtVlrDescuento.EditValue = this._liquidador.VlrDescuento;
                                //this.txtPorCredito.EditValue = this.porInteres;

                                //this.txtVlrPoliza.EditValue = this._liquidador.VlrPoliza);

                                //Variables
                                this.vlrLibranza = this._liquidador.VlrLibranza;

                                this.CalcularGiro();
                                this._data.SolicituDocu.VlrGiro.Value = Convert.ToDecimal(this.vlrGiro);
                                this._data.SolicituDocu.PorSeguro.Value = this.porInteresPoliza;
                                this._data.SolicituDocu.PlazoSeguro.Value = this._plazopoliza;
                                TBSave();
                                #endregion
                                #region Actualiza las grillas

                                if (hasLiquidacion && this._componentesVisibles.Count == 0)
                                {
                                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                                    MessageBox.Show(msg);
                                }


                                #endregion

                                #endregion
                            }

                        }
                        this._ctrl.FechaDoc.Value = System.DateTime.Now;
                        this._ctrl.CentroCostoID.Value = this.centroCostoSol;

                        List<DTO_ccSolicitudCompraCartera> compraTemp = new List<DTO_ccSolicitudCompraCartera>();
                        List<DTO_ccSolicitudDetallePago> detaPagoTemp = new List<DTO_ccSolicitudDetallePago>();

                        DTO_glZona zona = (DTO_glZona)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glZona, false, this.masterZona.Value, true);
                        DTO_glAreaFisica areaFis = (DTO_glAreaFisica)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false,(zona != null? zona.AreaFisica.Value :string.Empty), true);

                        DTO_DigitacionCredito digCredito = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(this._libranzaID, this._actFlujoLegalizacion.ID.Value);
                        this._compCartera = digCredito.CompraCartera;
                        this._detallesPago = digCredito.DetaPagos;
                        if (areaFis != null)
                            this.centroCostoSol = areaFis.CentroCostoID.Value;
                        
                        //if (this.chkCompraCartera.Checked)
                            compraTemp = this._compCartera;

                        //if (!this.chkPagoVentanilla.Checked)
                            detaPagoTemp = this._detallesPago;
                        DTO_TxResult result = new DTO_TxResult();

                        digCredito.AddData(this._ctrl, this._data.SolicituDocu, null, this._liquidador, this._componentesContab, compraTemp, detaPagoTemp);


                        result = _bc.AdministrationModel.DigitacionCredito_Add(this._documentID, this._actFlujoLegalizacion.ID.Value, digCredito, new List<DTO_Cuota>());

                        MessageBox.Show("Se Genero el Plan de Cuotas");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "btn_Liquidar_Click"));
            }
        }

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Deshabilita el scroll del spin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditSpin_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Devuelve el flujo al dar clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDevolver_Click(object sender, EventArgs e)
        {
            if (this.actividadesFlujo.Exists(x => x == this.cmbActividades.EditValue.ToString() && this._ctrl != null))
            {
                DTO_TxResult res = this._bc.AdministrationModel.DevolverFlujoDocumento(this._documentID, this.cmbActividades.EditValue.ToString(), this._ctrl.NumeroDoc.Value, string.Empty);
                if (res.Result == ResultValue.OK)
                    FormProvider.CloseCurrent();
            }
        }

        /// <summary>
        /// Al cmabiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbActividades_EditValueChanged(object sender, EventArgs e)
        {
            if (this.actividadesFlujo.Exists(x => x == this.cmbActividades.EditValue.ToString() && this._ctrl != null))
                this.lblDevolver.Visible = true;
            else
                this.lblDevolver.Visible = false;
        }
        #endregion Eventos Formulario

        #region Eventos Grillas

        //Generales

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "DateTime")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "DateTime")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double"
                        || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime")
                    {
                        e.Value = e.Value.ToString();//pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                    }
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double"
                            || fi.FieldType.Name == "Decimal" || fi.FieldType.Name == "DateTime")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }



        #endregion Enventos Grilla

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                //this.lkpObligaciones.EditValue = 0;
                this._tipoCreditoID = string.Empty;
                this.masterVitrina.Value = string.Empty;
                this.CleanData(true);
                this.EnableHeaderNuevo(false);
                this.masterVitrina.Value = string.Empty;
                this._tipoCreditoID = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvActividades.PostEditor();
                this.AssignValues(false);
                this.gvActividades.PostEditor();
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, false);
                    if (_mensajeGuardar)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                        MessageBox.Show(string.Format(msg, this._libranzaID));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionLegalizacion.cs", "TBSave"));
            }
        }

        public override void TBEdit()
        {

            try
            {
                    string pass = string.Empty;

                    //Pide la contrasena del usuario que autoriza la edicion
                    if (Prompt.InputBox("Autorización de edición ", this._userAutoriza + " por favor ingrese su contraseña: ", ref pass, true) == DialogResult.OK)
                    {
                        if (string.IsNullOrEmpty(pass))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginNoData));
                            return;
                        }

                        #region Valida las credenciales y activa la edicion
                        UserResult userVal = _bc.AdministrationModel.seUsuario_ValidateUserCredentials(this._userAutoriza, pass);
                        if (userVal == UserResult.NotExists)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginFailure));
                            return;
                        }
                        else if (userVal == UserResult.IncorrectPassword)
                        {
                            DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._userAutoriza);
                            string ctrl = _bc.GetControlValue(AppControl.RepeticionesContrasenaBloqueo);
                            int repPermitidas = Convert.ToInt16(ctrl);

                            if ((repPermitidas - user.ContrasenaRep.Value.Value) == 0)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginBlockUser));
                            }
                            else
                            {
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginIncorrectPwd);
                                msg = string.Format(msg, repPermitidas - user.ContrasenaRep.Value.Value);
                                MessageBox.Show(msg);
                            }

                            return;
                        }
                        else if (userVal == UserResult.AlreadyMember)
                        {
                            this._allowEditValuesInd = true;
                            this.txtVlrVehiculoPrenda.Enabled = true;
                            this.chkCeroKMPrenda.Enabled = true;
                            this.txtMarcaPrenda.Enabled = true;
                            this.txtModeloPrenda.Enabled = true;
                            this.cmbServicioPrenda.Enabled = true;
                            this.txtLineaPrenda.Enabled = true;

                            this.cmbTipoInmueble.Enabled = true;
                            this.txtDireccion.Enabled = true;
                            this.masterCiudadHipoteca.Enabled = true;
                            this.masterCiudadHipoteca2.Enabled = true;
                            this.txtFMI.Enabled = true;
                            this.chkViviendaNueva.Enabled = true;

                            this.dtFechaPredial.Enabled = true;
                            this.dtFechaComercial.Enabled = true;
                            this.dtFechaCompraventa.Enabled = true;
                            this.txtValorPredial.Enabled = true;
                            this.txtValorComercial.Enabled = true;
                            this.txtValorCompraventa.Enabled = true;
                        }
                        #endregion
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "itemEdit_Click"));
            }


        }


        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this._mensajeGuardar = false;
                TBSave();
                this._mensajeGuardar = true;

                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar la legalización de la solicitud?  ");
                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                //this.AssignValues(false);
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                    MessageBox.Show(string.Format(msg, this._libranzaID));
                    if (result.Result == ResultValue.OK)
                        FormProvider.CloseCurrent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas


        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 2)
            {
                if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                    this.tabControl.SelectedTabPageIndex = 1;
                else
                {
                    if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                        this.tabControl.SelectedTabPageIndex = 0;
                    else
                        this.tabControl.SelectedTabPageIndex = 3;
                }
            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
                if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                    this.tabControl.SelectedTabPageIndex = 0;
                else
                    this.tabControl.SelectedTabPageIndex = 3;
            }
            else if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 3;
            else if (this.tabControl.SelectedTabPageIndex == 3)
                this.tabControl.SelectedTabPageIndex = 2;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            if (this.tabControl.SelectedTabPageIndex == 5)
            {
                if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                {
                    this.tabControl.SelectedTabPageIndex = 0;
                }
                else
                {
                    if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                        this.tabControl.SelectedTabPageIndex = 1;
                    else
                    {
                        if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                            this.tabControl.SelectedTabPageIndex = 2;
                        else
                        {
                            if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                                this.tabControl.SelectedTabPageIndex = 3;
                            else
                                this.tabControl.SelectedTabPageIndex = 4;
                        }
                    }
                }
            }
            else if (this.tabControl.SelectedTabPageIndex == 0)
            {
                if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                    this.tabControl.SelectedTabPageIndex = 1;
                else
                {
                    if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                    {
                        if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                            this.tabControl.SelectedTabPageIndex = 3;
                        else
                            this.tabControl.SelectedTabPageIndex = 4;
                    }
                }
            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
                    if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                    {
                        if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                            this.tabControl.SelectedTabPageIndex = 3;
                        else
                            this.tabControl.SelectedTabPageIndex = 4;
                    }
            }
            else if (this.tabControl.SelectedTabPageIndex == 2)
            {
                if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                    this.tabControl.SelectedTabPageIndex = 3;
                else
                    this.tabControl.SelectedTabPageIndex = 4;
            }
            else if (this.tabControl.SelectedTabPageIndex == 3)
                this.tabControl.SelectedTabPageIndex = 4;
            else if (this.tabControl.SelectedTabPageIndex == 4)
                this.tabControl.SelectedTabPageIndex = 5;
        }


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (this.cmbReportes.EditValue.ToString() == "1")
                this.documentReportID = AppReports.drSolicitudCredito;
            else if (this.cmbReportes.EditValue.ToString() == "2")
                this.documentReportID = AppReports.drAsegurabilidad;
            else if (this.cmbReportes.EditValue.ToString() == "3")
                this.documentReportID = AppReports.drCondicionesGenerales;
            else if (this.cmbReportes.EditValue.ToString() == "4")
                this.documentReportID = AppReports.drPagareCredito;
            else if (this.cmbReportes.EditValue.ToString() == "5")
                this.documentReportID = AppReports.drCartaPagareCredito;
            else if (this.cmbReportes.EditValue.ToString() == "6")
                this.documentReportID = AppReports.drPagareSeguro;
            else if (this.cmbReportes.EditValue.ToString() == "7")
                this.documentReportID = AppReports.drCartaPagareSeguro;
            else if (this.cmbReportes.EditValue.ToString() == "8")
                this.documentReportID = AppReports.drPrendaDeudor;
            else if (this.cmbReportes.EditValue.ToString() == "9")
                this.documentReportID = AppReports.drCondicionesEspecificas;
            else if (this.cmbReportes.EditValue.ToString() == "10")
                this.documentReportID = AppReports.drCertGrupoDeudores;
            else if (this.cmbReportes.EditValue.ToString() == "11")
                this.documentReportID = AppReports.drCartaEnvioPrenda;

            //tipoReporte.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Solicitud de Credito"));// Formato
            //tipoReporte.Add(8, this._bc.GetResource(LanguageTypes.Tables, "Prendas"));//Validar dos deudores
            //tipoReporte.Add(9, this._bc.GetResource(LanguageTypes.Tables, "Condiciones Especificas"));// formato Impreso


            Dictionary<string, string> list = new Dictionary<string, string>();
            int consecutivo = 0;
            DTO_ccSolicitudDocu solicitudSelect = null;
            string terceroEmpresa = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            string responsFirma = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ResponsableFirmaCertif);
            DTO_coTercero dtoFirma = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, responsFirma, true);
            DTO_ccCliente dtoCliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, _clienteID, true);
            DTO_glLugarGeografico dtoLugarGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, dtoCliente.ResidenciaCiudad.Value, true);
            if (this.cmbReportes.EditValue.ToString() == "1" || this.cmbReportes.EditValue.ToString() == "9")
            {
                string reportName = this._bc.AdministrationModel.Report_Dr_DecisorByNumeroDoc(this.documentReportID, Convert.ToInt32(this._ctrl.NumeroDoc.Value), Convert.ToInt32(this._libranzaID), ExportFormatType.pdf);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            else
            {
                DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranzaID));
                this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
            }
        }

        private void btnGenerarPrenda_Click(object sender, EventArgs e)
        {
            #region Crea glDocumentoControl
            DTO_glAreaFuncional AreaFuncional = (DTO_glAreaFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this._data.DocCtrl.AreaFuncionalID.Value, true);

            DTO_glDocumentoControl ctrlPrenda = new DTO_glDocumentoControl();
            ctrlPrenda.DocumentoNro.Value = 0;
            ctrlPrenda.DocumentoID.Value = AppDocuments.Garantias;
            ctrlPrenda.LugarGeograficoID.Value = this._data.DocCtrl.LugarGeograficoID.Value;
            ctrlPrenda.NumeroDoc.Value = 0;
            ctrlPrenda.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
            //ctrlPrenda.ComprobanteID.Value = coDocReoperacion.ComprobanteID.Value;
            ctrlPrenda.Fecha.Value = DateTime.Now;
            ctrlPrenda.FechaDoc.Value = this._data.SolicituDocu.FechaLiquida.Value;
            ctrlPrenda.PeriodoDoc.Value = periodo;
            ctrlPrenda.PeriodoUltMov.Value = periodo;
            //ctrlPrenda.CuentaID.Value = coDoc.CuentaLOC.Value;
            ctrlPrenda.AreaFuncionalID.Value = this._data.DocCtrl.AreaFuncionalID.Value;
            ctrlPrenda.PrefijoID.Value = AreaFuncional.Prefijo1.Value; 
            ctrlPrenda.ProyectoID.Value = this._data.DocCtrl.ProyectoID.Value;
            ctrlPrenda.CentroCostoID.Value = this._data.DocCtrl.CentroCostoID.Value;
            ctrlPrenda.LineaPresupuestoID.Value = this._data.DocCtrl.LineaPresupuestoID.Value;
            ctrlPrenda.TerceroID.Value = this._data.SolicituDocu.ClienteID.Value;
            ctrlPrenda.DocumentoTercero.Value = this._data.SolicituDocu.Libranza.Value.ToString();
            ctrlPrenda.MonedaID.Value = this._data.DocCtrl.MonedaID.Value;
            ctrlPrenda.TasaCambioCONT.Value = this._data.DocCtrl.TasaCambioCONT.Value;
            ctrlPrenda.TasaCambioDOCU.Value = this._data.DocCtrl.TasaCambioDOCU.Value;
            //ctrlPrenda.Observacion.Value = this._data.SolicituDocu.Observacion.Value;
            ctrlPrenda.Descripcion.Value = "Numero Prenda " + this._data.SolicituDocu.Libranza.Value;
            ctrlPrenda.Estado.Value = (byte)EstadoDocControl.Aprobado;
            ctrlPrenda.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
            ctrlPrenda.Valor.Value = Convert.ToDecimal(this.txtVlrVehiculoPrenda.EditValue);
            ctrlPrenda.Iva.Value = 0;
            ctrlPrenda.DocumentoPadre.Value = this._data.DocCtrl.NumeroDoc.Value;

            DTO_TxResultDetail result = _bc.AdministrationModel.Garantia_Add(this._documentID, ctrlPrenda);
            if (result.Message == "NOK")
            {
            }
            this._data.DatosVehiculo.PrefijoPrenda.Value = ctrlPrenda.PrefijoID.Value;
            this._data.DatosVehiculo.NumeroPrenda.Value = Convert.ToInt32(result.Key);
            //ctrlPrenda.NumeroDoc.Value = Convert.ToInt32(result.Key);
            this.txtPrenda.Text = this._data.DatosVehiculo.NumeroPrenda.ToString();
            this._mensajeGuardar=false;
            TBSave();
            this.btnGenerarPrenda.Enabled = false;
            this._mensajeGuardar = true;
            #endregion
        }

        private void btnHipoteca_Click(object sender, EventArgs e)
        {
            #region Crea glDocumentoControl
            DTO_glAreaFuncional AreaFuncional = (DTO_glAreaFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this._data.DocCtrl.AreaFuncionalID.Value, true);

            DTO_glDocumentoControl ctrlHipoteca = new DTO_glDocumentoControl();
            ctrlHipoteca.DocumentoNro.Value = 0;
            ctrlHipoteca.DocumentoID.Value = AppDocuments.Garantias;
            ctrlHipoteca.LugarGeograficoID.Value = this._data.DocCtrl.LugarGeograficoID.Value;
            ctrlHipoteca.NumeroDoc.Value = 0;
            ctrlHipoteca.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
            //ctrlHipoteca.ComprobanteID.Value = coDocReoperacion.ComprobanteID.Value;
            ctrlHipoteca.Fecha.Value = DateTime.Now;
            ctrlHipoteca.FechaDoc.Value = this._data.SolicituDocu.FechaLiquida.Value;
            ctrlHipoteca.PeriodoDoc.Value = periodo;
            ctrlHipoteca.PeriodoUltMov.Value = periodo;
            //ctrlHipoteca.CuentaID.Value = coDoc.CuentaLOC.Value;
            ctrlHipoteca.AreaFuncionalID.Value = this._data.DocCtrl.AreaFuncionalID.Value;
            ctrlHipoteca.PrefijoID.Value = AreaFuncional.Prefijo2.Value; 
            ctrlHipoteca.ProyectoID.Value = this._data.DocCtrl.ProyectoID.Value;
            ctrlHipoteca.CentroCostoID.Value = this._data.DocCtrl.CentroCostoID.Value;
            ctrlHipoteca.LineaPresupuestoID.Value = this._data.DocCtrl.LineaPresupuestoID.Value;
            ctrlHipoteca.TerceroID.Value = this._data.SolicituDocu.ClienteID.Value;
            ctrlHipoteca.DocumentoTercero.Value = this._data.SolicituDocu.Libranza.Value.ToString();
            ctrlHipoteca.MonedaID.Value = this._data.DocCtrl.MonedaID.Value;
            ctrlHipoteca.TasaCambioCONT.Value = this._data.DocCtrl.TasaCambioCONT.Value;
            ctrlHipoteca.TasaCambioDOCU.Value = this._data.DocCtrl.TasaCambioDOCU.Value;
            //ctrlHipoteca.Observacion.Value = this._data.SolicituDocu.Observacion.Value;
            ctrlHipoteca.Descripcion.Value = "Numero Hipoteca " + this._data.SolicituDocu.Libranza.Value;
            ctrlHipoteca.Estado.Value = (byte)EstadoDocControl.Aprobado;
            ctrlHipoteca.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
            ctrlHipoteca.Valor.Value = Convert.ToDecimal(this.txtValorComercial.EditValue);
            ctrlHipoteca.Iva.Value = 0;
            ctrlHipoteca.DocumentoPadre.Value = this._data.DocCtrl.DocumentoNro.Value;

            DTO_TxResultDetail result = _bc.AdministrationModel.Garantia_Add(this._documentID, ctrlHipoteca);
            if (result.Message == "NOK")
            {
            }
            this._data.DatosVehiculo.PrefijoHipoteca.Value = ctrlHipoteca.PrefijoID.Value;
            this._data.DatosVehiculo.NumeroHipoteca.Value = Convert.ToInt32(result.Key);
            //ctrlHipoteca.NumeroDoc.Value = Convert.ToInt32(result.Key);
            this.txtHipoteca.Text = this._data.DatosVehiculo.NumeroHipoteca.ToString();
            this._mensajeGuardar = false;
            TBSave();
            this.btnHipoteca.Enabled = false;
            this._mensajeGuardar = true;

            #endregion
        }

        private void btnGenerarPrenda2_Click(object sender, EventArgs e)
        {
            #region Crea glDocumentoControl
            DTO_glAreaFuncional AreaFuncional = (DTO_glAreaFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this._data.DocCtrl.AreaFuncionalID.Value, true);

            DTO_glDocumentoControl ctrlPrenda2 = new DTO_glDocumentoControl();
            ctrlPrenda2.DocumentoNro.Value = 0;
            ctrlPrenda2.DocumentoID.Value = AppDocuments.Garantias;
            ctrlPrenda2.LugarGeograficoID.Value = this._data.DocCtrl.LugarGeograficoID.Value;
            ctrlPrenda2.NumeroDoc.Value = 0;
            ctrlPrenda2.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
            //ctrlPrenda2.ComprobanteID.Value = coDocReoperacion.ComprobanteID.Value;
            ctrlPrenda2.Fecha.Value = DateTime.Now;
            ctrlPrenda2.FechaDoc.Value = this._data.SolicituDocu.FechaLiquida.Value;
            ctrlPrenda2.PeriodoDoc.Value = periodo;
            ctrlPrenda2.PeriodoUltMov.Value = periodo;
            //ctrlPrenda2.CuentaID.Value = coDoc.CuentaLOC.Value;
            ctrlPrenda2.AreaFuncionalID.Value = this._data.DocCtrl.AreaFuncionalID.Value;
            ctrlPrenda2.PrefijoID.Value = AreaFuncional.Prefijo1.Value;
            ctrlPrenda2.ProyectoID.Value = this._data.DocCtrl.ProyectoID.Value;
            ctrlPrenda2.CentroCostoID.Value = this._data.DocCtrl.CentroCostoID.Value;
            ctrlPrenda2.LineaPresupuestoID.Value = this._data.DocCtrl.LineaPresupuestoID.Value;
            ctrlPrenda2.TerceroID.Value = this._data.SolicituDocu.ClienteID.Value;
            ctrlPrenda2.DocumentoTercero.Value = this._data.SolicituDocu.Libranza.Value.ToString();
            ctrlPrenda2.MonedaID.Value = this._data.DocCtrl.MonedaID.Value;
            ctrlPrenda2.TasaCambioCONT.Value = this._data.DocCtrl.TasaCambioCONT.Value;
            ctrlPrenda2.TasaCambioDOCU.Value = this._data.DocCtrl.TasaCambioDOCU.Value;
            //ctrlPrenda2.Observacion.Value = this._data.SolicituDocu.Observacion.Value;
            ctrlPrenda2.Descripcion.Value = "Numero Prenda " + this._data.SolicituDocu.Libranza.Value;
            ctrlPrenda2.Estado.Value = (byte)EstadoDocControl.Aprobado;
            ctrlPrenda2.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
            ctrlPrenda2.Valor.Value = Convert.ToDecimal(this.txtVlrVehiculoPrenda2.EditValue);
            ctrlPrenda2.Iva.Value = 0;
            ctrlPrenda2.DocumentoPadre.Value = this._data.DocCtrl.NumeroDoc.Value;

            DTO_TxResultDetail result = _bc.AdministrationModel.Garantia_Add(this._documentID, ctrlPrenda2);
            if (result.Message == "NOK")
            {
            }
            this._data.DatosVehiculo.PrefijoPrenda_2.Value = ctrlPrenda2.PrefijoID.Value;
            this._data.DatosVehiculo.NumeroPrenda_2.Value = Convert.ToInt32(result.Key);
            //ctrlPrenda2.NumeroDoc.Value = Convert.ToInt32(result.Key);
            this.txtPrenda2.Text = this._data.DatosVehiculo.NumeroPrenda_2.ToString();
            this._mensajeGuardar = false;
            TBSave();
            this.btnGenerarPrenda2.Enabled = false;
            this._mensajeGuardar = true;


            #endregion
        }

        private void btnHipoteca2_Click(object sender, EventArgs e)
        {
            #region Crea glDocumentoControl
            DTO_glAreaFuncional AreaFuncional = (DTO_glAreaFuncional)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this._data.DocCtrl.AreaFuncionalID.Value, true);

            DTO_glDocumentoControl ctrlHipoteca2 = new DTO_glDocumentoControl();
            ctrlHipoteca2.DocumentoNro.Value = 0;
            ctrlHipoteca2.DocumentoID.Value = AppDocuments.Garantias;
            ctrlHipoteca2.LugarGeograficoID.Value = this._data.DocCtrl.LugarGeograficoID.Value;
            ctrlHipoteca2.NumeroDoc.Value = 0;
            ctrlHipoteca2.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
            //ctrlHipoteca2.ComprobanteID.Value = coDocReoperacion.ComprobanteID.Value;
            ctrlHipoteca2.Fecha.Value = DateTime.Now;
            ctrlHipoteca2.FechaDoc.Value = this._data.SolicituDocu.FechaLiquida.Value;
            ctrlHipoteca2.PeriodoDoc.Value = periodo;
            ctrlHipoteca2.PeriodoUltMov.Value = periodo;
            //ctrlHipoteca2.CuentaID.Value = coDoc.CuentaLOC.Value;
            ctrlHipoteca2.AreaFuncionalID.Value = this._data.DocCtrl.AreaFuncionalID.Value;
            ctrlHipoteca2.PrefijoID.Value = AreaFuncional.Prefijo1.Value;
            ctrlHipoteca2.ProyectoID.Value = this._data.DocCtrl.ProyectoID.Value;
            ctrlHipoteca2.CentroCostoID.Value = this._data.DocCtrl.CentroCostoID.Value;
            ctrlHipoteca2.LineaPresupuestoID.Value = this._data.DocCtrl.LineaPresupuestoID.Value;
            ctrlHipoteca2.TerceroID.Value = this._data.SolicituDocu.ClienteID.Value;
            ctrlHipoteca2.DocumentoTercero.Value = this._data.SolicituDocu.Libranza.Value.ToString();
            ctrlHipoteca2.MonedaID.Value = this._data.DocCtrl.MonedaID.Value;
            ctrlHipoteca2.TasaCambioCONT.Value = this._data.DocCtrl.TasaCambioCONT.Value;
            ctrlHipoteca2.TasaCambioDOCU.Value = this._data.DocCtrl.TasaCambioDOCU.Value;
            //ctrlHipoteca2.Observacion.Value = this._data.SolicituDocu.Observacion.Value;
            ctrlHipoteca2.Descripcion.Value = "Numero Hipoteca " + this._data.SolicituDocu.Libranza.Value;
            ctrlHipoteca2.Estado.Value = (byte)EstadoDocControl.Aprobado;
            ctrlHipoteca2.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
            ctrlHipoteca2.Valor.Value = Convert.ToDecimal(this.txtValorComercial.EditValue);
            ctrlHipoteca2.Iva.Value = 0;
            ctrlHipoteca2.DocumentoPadre.Value = this._data.DocCtrl.DocumentoNro.Value;

            DTO_TxResultDetail result = _bc.AdministrationModel.Garantia_Add(this._documentID, ctrlHipoteca2);
            if (result.Message == "NOK")
            {
            }
            this._data.DatosVehiculo.PrefijoHipoteca_2.Value = ctrlHipoteca2.PrefijoID.Value;
            this._data.DatosVehiculo.NumeroHipoteca_2.Value = Convert.ToInt32(result.Key);
            //ctrlHipoteca2.NumeroDoc.Value = Convert.ToInt32(result.Key);
            this.txtHipoteca2.Text = this._data.DatosVehiculo.NumeroHipoteca_2.ToString();
            this._mensajeGuardar = false;
            TBSave();
            this.btnHipoteca2.Enabled = false;
            this._mensajeGuardar = true;
            #endregion

        }


        private void txtVlrPolizaPrenda1_Validated(object sender, EventArgs e)
        {
            decimal _totalPoliza = 0;
            if (this.chkPolizaCancelaContadoGarantiaPrenda1.Checked)
                _totalPoliza = _totalPoliza + Convert.ToDecimal(this.txtVlrPolizaPrenda1.EditValue);
            if (this.chkPolizaCancelaContadoGarantiaPrenda2.Checked)
                _totalPoliza = _totalPoliza + Convert.ToDecimal(this.txtVlrPolizaPrenda2.EditValue);
            this.txtVlrPoliza.EditValue = _totalPoliza;

        }


    }
 }

