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
using SentenceTransformer;
using DevExpress.XtraEditors;
using NewAge.Forms;

using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DesembolsoSolicitud : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private int _documentIDGarantia;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //Datos por defecto
        private string _lugGeoXDef = String.Empty;
        private string _AseguradoraXDef = String.Empty;
        private string _ZonaXDef = String.Empty;
        private string _VitrinaXDef = String.Empty;

        //DTOs        
        private DTO_ccCliente _cliente = new DTO_ccCliente();
        private List<DTO_ccSolicitudComponentes> _componentesVisibles = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudComponentes> _componentesContab = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_Cuota> _planPagos = new List<DTO_Cuota>();
        private DTO_PlanDePagos _liquidador = new DTO_PlanDePagos();
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_glActividadFlujo _actFlujoLiq = new DTO_glActividadFlujo();
        private DTO_ccSolicitudDocu _headerSolicitud = new DTO_ccSolicitudDocu();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_drActividadChequeoLista> actChequeoBase = new List<DTO_drActividadChequeoLista>();
        private List<DTO_drSolicitudDatosChequeados> actChequeo = new List<DTO_drSolicitudDatosChequeados>();
        private List<DTO_glGarantiaControl> Garantias = new List<DTO_glGarantiaControl>();

        //Identificador de la proxima actividad
        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        //Variables formulario (campos)
        private string _tipoCreditoID = string.Empty;
        private string _clienteID = string.Empty;
        private string _lineaCreditoID = string.Empty;
        private int _libranzaID;
        //private int _libranzaID = 0;
        private string _polizaID = string.Empty;
        private TipoCredito _tipoCredito = TipoCredito.Nuevo;

        //Variables auxiliares del formulario
        private DateTime periodo;
        private bool validateData;
        Dictionary<int, string> servicio = new Dictionary<int, string>();
        Dictionary<int, string> garante = new Dictionary<int, string>();
        Dictionary<int, string> tipoReporte = new Dictionary<int, string>();

        Dictionary<string, decimal> compsNuevoValor = new Dictionary<string, decimal>();
        private string _cuentaAbonoCap;
        private bool readOnly = false;

        //Valores temporales
        private decimal vlrLibranza = 0;
        private decimal vlrGiro = 0;
        private int vlrSolicitadoPrestamo = 0;

        //Variables de mensajes
        private string msgFinDoc;

        #endregion Variables

        #region Constructor
        
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DesembolsoSolicitud()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DesembolsoSolicitud(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DesembolsoSolicitud(int libranza, string mod)
        {
            this.Constructor(libranza, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DesembolsoSolicitud(int libranza, bool readOnly)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.dr;
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
                    string ActFlujoLegalizacion= this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Legalizacion);

                    string actividadFlujoSolicitud = actividadesSolicitud[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
                    this._actFlujoLiq = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, ActFlujoLegalizacion, true);
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

                //Acitividades de Chequeo
                this.AddChequeoCols();
                this.LoadActividades();
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitudFinanciera.cs", "DesembolsoSolicitudFinanciera"));
            }
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
                    List<string> NextActs = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DesembolsoSolicitud);
                    if (NextActs.Count != 1)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                        MessageBox.Show(string.Format(msg, AppDocuments.DesembolsoSolicitud.ToString()));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "DesembolsoSolicitud"));
            }
        }
     
        #endregion

        #region Funciones Privadas

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

        private void AssignValues(bool isGet)
        {
            try
            {
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
                DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo;
                if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                {
                    this.gbDatosGeneralesVEH1.Visible = true;
                    this.gbPrenda1.Visible = true;
                    this.gbPolizaPrenda1.Visible = true;
                }
                else
                {
                    this.gbDatosGeneralesVEH1.Visible = false;
                    this.gbPrenda1.Visible = false;
                    this.gbPolizaPrenda1.Visible = false;
                }

                if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                {
                    this.gbDatosGeneralesVEH2.Visible = true;
                    this.gbPrenda2.Visible = true;
                    this.gbPolizaPrenda2.Visible = true;
                }
                else
                {
                    this.gbDatosGeneralesVEH2.Visible = false;
                    this.gbPrenda2.Visible = false;
                    this.gbPolizaPrenda2.Visible = false;
                }

                if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                {
                    this.gbDatosGeneralesHIP1.Visible = true;
                    this.gbHipoteca1.Visible = true;
                    this.gbPolizaHipoteca1.Visible = true;
                }
                else
                {
                    this.gbDatosGeneralesHIP1.Visible = false;
                    this.gbHipoteca1.Visible = false;
                    this.gbPolizaHipoteca1.Visible = false;
                }
                if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                {
                    this.gbDatosGeneralesHIP2.Visible = true;
                    this.gbHipoteca2.Visible = true;
                    this.gbPolizaHipoteca2.Visible = true;
                }
                else
                {
                    this.gbDatosGeneralesHIP2.Visible = false;
                    this.gbHipoteca2.Visible = false;
                    this.gbPolizaHipoteca2.Visible = false;
                }                

                this._data.SolicituDocu.Nombre.Value = this._data.SolicituDocu.NombrePri.Value + " "+ this._data.SolicituDocu.NombreSdo.Value + " " + this._data.SolicituDocu.ApellidoPri.Value + " " + this._data.SolicituDocu.ApellidoSdo.Value;
                if (isGet)
                {
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteID.Value.ToString();
                    this.txtDeudor.Text = this._data.SolicituDocu.Nombre.Value.ToString();
                    this.masterVitrina.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        this.dtFechaPrimeraCuota.EditValue = System.DateTime.Now.AddMonths(1);
                        #region datos Vehiculo 1
                        
                        this.txtMarca.Text = vehiculo.Marca.Value.ToString();
                        this.txtLinea.Text = vehiculo.Linea.Value.ToString();
                        this.txtModelo.Text = vehiculo.Modelo.Value.ToString();
                        this.cmbServicioGeneral.EditValue = vehiculo.Servicio.Value;
                        this.chkCeroKM.Checked = vehiculo.CeroKmInd.Value.Value;
                        this.txtPrecioVenta.EditValue = vehiculo.PrecioVenta.Value;
                        this.txtCuotaInicial.EditValue = vehiculo.Registrada.Value;
                        this.txtMonto.EditValue = vehiculo.VlrFasecolda.Value;
                                                
                        this.txtRunt.Text = vehiculo.RUNT.Value.ToString();
                        this.txtConfecamaras.Text = vehiculo.Confecamaras.Value.ToString();
                        this.txtPlaca.Text = vehiculo.Placa.Value.ToString();
                        this.dtFechaSOAT.DateTime = vehiculo.FechaVTO.Value.HasValue ? vehiculo.FechaVTO.Value.Value : DateTime.Now;
                        this.masterAseguradoraPrenda1.Value = vehiculo.Aseguradora1VEH.Value;
                        this.txtNumeroPolizaVEH1.Text = vehiculo.PolizaVEH1.Value.ToString();
                        this.dtFechaPolizaIniVEH1.DateTime = vehiculo.FechaIniVEH1.Value.HasValue ? vehiculo.FechaIniVEH1.Value.Value : DateTime.Now;
                        this.dtFechaPolizaFinVEH1.DateTime = vehiculo.FechaFinVEH1.Value.HasValue ? vehiculo.FechaFinVEH1.Value.Value : DateTime.Now.AddMonths(12);
                        this.txtValorRealPolizaVEH1.EditValue = vehiculo.VlrPolizaVEH1.Value;

                        #endregion

                        #region datos Vivienda 1

                        this.txtDireccionHIP1.Text = vehiculo.Direccion.Value.ToString();
                        this.txtVlrAvaluoComercialHIP1.EditValue = vehiculo.ValorAvaluo.Value;
                        this.txtFMIHIP1.Text = vehiculo.Matricula.Value.ToString();
                        this.txtVlrAvaluoPredialHIP1.EditValue = vehiculo.ValorPredial.Value;
                        
                        this.txtEscritura.Text = vehiculo.Escritura.Value.ToString();
                        this.dtFechaEscritura.DateTime = vehiculo.FechaEscritura.Value.HasValue ? vehiculo.FechaEscritura.Value.Value : DateTime.Now;
                        this.txtNotaria.Text = vehiculo.Notaria.Value.ToString();

                        this.masterAseguradoraHipoteca1.Value = vehiculo.Aseguradora1HIP.Value;
                        this.txtNumeroPolizaHIP1.Text = vehiculo.PolizaHIP1.Value.ToString();
                        this.dtFechaPolizaIniHIP1.DateTime = vehiculo.FechaIniHIP1.Value.HasValue ? vehiculo.FechaIniHIP1.Value.Value : DateTime.Now;
                        this.dtFechaPolizaFinHIP1.DateTime = vehiculo.FechaFinHIP1.Value.HasValue ? vehiculo.FechaFinHIP1.Value.Value : DateTime.Now.AddMonths(12);
                        this.txtValorRealPolizaVEH1.EditValue = vehiculo.VlrPolizaHIP1.Value;

                        #endregion

                        if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                        {
                            #region datos Vehiculo 2

                            this.txtMarca2.Text = vehiculo.Marca_2.Value.ToString();
                            this.txtLinea2.Text = vehiculo.Linea_2.Value.ToString();
                            this.txtModelo2.Text = vehiculo.Modelo_2.Value.ToString();
                            this.cmbServicioGeneral2.EditValue = vehiculo.Servicio_2.Value;
                            this.chkCeroKM2.Checked = vehiculo.CeroKmInd_2.Value.Value;
                            this.txtPrecioVenta2.EditValue = vehiculo.PrecioVenta_2.Value;
                            this.txtCuotaInicial2.EditValue = vehiculo.Registrada_2.Value;
                            this.txtMonto2.EditValue = vehiculo.VlrFasecolda_2.Value;

                            this.txtRunt2.Text = vehiculo.RUNT_2.Value.ToString();
                            this.txtConfecamaras2.Text = vehiculo.Confecamaras_2.Value.ToString();
                            this.txtPlaca2.Text = vehiculo.Placa_2.Value.ToString();
                            this.dtFechaSOAT2.DateTime = vehiculo.FechaVto_2.Value.HasValue ? vehiculo.FechaVto_2.Value.Value : DateTime.Now;
                            this.masterAseguradoraPrenda2.Value = vehiculo.Aseguradora2VEH.Value;
                            this.txtNumeroPolizaVEH2.Text = vehiculo.PolizaVEH2.Value.ToString();
                            this.dtFechaPolizaIniVEH2.DateTime = vehiculo.FechaIniVEH2.Value.HasValue ? vehiculo.FechaIniVEH2.Value.Value : DateTime.Now;
                            this.dtFechaPolizaFinVEH2.DateTime = vehiculo.FechaFinVEH2.Value.HasValue ? vehiculo.FechaFinVEH2.Value.Value : DateTime.Now.AddMonths(12);
                            this.txtValorRealPolizaVEH2.EditValue = vehiculo.VlrPolizaVEH2.Value;

                            #endregion
                        }

                        if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                        {
                            #region datos Vivienda 2

                            this.txtDireccionHIP2.Text = vehiculo.Direccion_2.Value.ToString();
                            this.txtVlrAvaluoComercialHIP2.EditValue = vehiculo.ValorAvaluo_2.Value;
                            this.txtFMIHIP2.Text = vehiculo.Matricula_2.Value.ToString();
                            this.txtVlrAvaluoPredialHIP2.EditValue = vehiculo.ValorPredial_2.Value;

                            this.txtEscritura2.Text = vehiculo.Escritura_2.Value.ToString();
                            this.dtFechaEscritura2.DateTime = vehiculo.FechaEscritura_2.Value.HasValue ? vehiculo.FechaEscritura_2.Value.Value : DateTime.Now;
                            this.txtNotaria2.Text = vehiculo.Notaria_2.Value.ToString();

                            this.masterAseguradoraHipoteca2.Value = vehiculo.Aseguradora2HIP.Value;
                            this.txtNumeroPolizaHIP2.Text = vehiculo.PolizaHIP2.Value.ToString();
                            this.dtFechaPolizaIniHIP2.DateTime = vehiculo.FechaIniHIP2.Value.HasValue ? vehiculo.FechaIniHIP2.Value.Value : DateTime.Now;
                            this.dtFechaPolizaFinHIP2.DateTime = vehiculo.FechaFinHIP2.Value.HasValue ? vehiculo.FechaFinHIP2.Value.Value : DateTime.Now;
                            this.txtValorRealPolizaVEH2.EditValue = vehiculo.VlrPolizaHIP2.Value;

                            #endregion
                        }
                    }
                }
                else
                {
                    this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value : 1;
                    #region Llena datos de los controles para salvar
                    DTO_drSolicitudDatosVehiculo vehiculoNew = vehiculo != null ? vehiculo : new DTO_drSolicitudDatosVehiculo();


                    #region datos Vehiculo

                    vehiculoNew.Marca.Value = this.txtMarca.Text;
                    vehiculoNew.Linea.Value = this.txtLinea.Text;
                    vehiculoNew.Modelo.Value = Convert.ToInt16(this.txtModelo.Text);
                    vehiculoNew.Servicio.Value = !string.IsNullOrEmpty(this.cmbServicioGeneral.EditValue.ToString()) ? Convert.ToByte(this.cmbServicioGeneral.EditValue) : vehiculoNew.Servicio.Value;
                    vehiculoNew.CeroKmInd.Value = this.chkCeroKM.Checked;
                    vehiculoNew.PrecioVenta.Value = Convert.ToDecimal(this.txtPrecioVenta.EditValue);
                    vehiculoNew.CuotaInicial.Value = Convert.ToDecimal(this.txtCuotaInicial.EditValue);
//                    this.txtMonto.EditValue = vehiculo.VlrFasecolda.Value;
                    
                    vehiculoNew.RUNT.Value = this.txtRunt.Text;
                    vehiculoNew.Confecamaras.Value = this.txtConfecamaras.Text;
                    vehiculoNew.Placa.Value = this.txtPlaca.Text;
                    vehiculoNew.FechaVTO.Value = Convert.ToDateTime(this.dtFechaSOAT.EditValue);
                    vehiculoNew.Aseguradora1VEH.Value = this.masterAseguradoraPrenda1.Value;
                    vehiculoNew.PolizaVEH1.Value = this.txtNumeroPolizaVEH1.Text;
                    vehiculoNew.FechaIniVEH1.Value = this.dtFechaPolizaIniVEH1.DateTime;
                    vehiculoNew.FechaFinVEH1.Value = this.dtFechaPolizaFinVEH1.DateTime;
                    vehiculoNew.VlrPolizaVEH1.Value = Convert.ToDecimal(this.txtValorRealPolizaVEH1.EditValue);
                    #endregion

                    #region datos Vivienda 1

                    vehiculoNew.Direccion.Value = this.txtDireccionHIP1.Text;
                    vehiculoNew.ValorAvaluo.Value = Convert.ToDecimal(this.txtVlrAvaluoComercialHIP1.EditValue);
                    vehiculoNew.Matricula.Value = this.txtFMIHIP1.Text;
                    vehiculoNew.ValorPredial.Value = Convert.ToDecimal(this.txtVlrAvaluoPredialHIP1.EditValue);

                    vehiculoNew.Escritura.Value = this.txtEscritura.Text;
                    vehiculoNew.FechaEscritura.Value = this.dtFechaEscritura.DateTime;
                    vehiculoNew.Notaria.Value = this.txtNotaria.Text;

                    vehiculoNew.Aseguradora1HIP.Value = this.masterAseguradoraHipoteca1.Value;
                    vehiculoNew.PolizaHIP1.Value = this.txtNumeroPolizaHIP1.Text;
                    vehiculoNew.FechaIniHIP1.Value = this.dtFechaPolizaIniHIP1.DateTime;
                    vehiculoNew.FechaFinHIP1.Value = this.dtFechaPolizaFinHIP1.DateTime;
                    vehiculoNew.VlrPolizaHIP1.Value = Convert.ToDecimal(this.txtValorRealPolizaVEH1.EditValue);

                    #endregion
                    if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                    {
                        #region datos Vehiculo 2

                        vehiculoNew.Marca_2.Value = this.txtMarca2.Text;
                        vehiculoNew.Linea_2.Value = this.txtLinea2.Text;
                        vehiculoNew.Modelo_2.Value = Convert.ToInt16(this.txtModelo2.Text);
                        vehiculoNew.Servicio_2.Value = !string.IsNullOrEmpty(this.cmbServicioGeneral2.EditValue.ToString()) ? Convert.ToByte(this.cmbServicioGeneral2.EditValue) : vehiculoNew.Servicio_2.Value;
                        vehiculoNew.CeroKmInd_2.Value = this.chkCeroKM2.Checked;
                        vehiculoNew.PrecioVenta_2.Value = Convert.ToDecimal(this.txtPrecioVenta2.EditValue);
                        //vehiculoNew.CuotaInicial.Value = Convert.ToDecimal(this.txtCuotaInicial.EditValue); Revisa
                        //                    this.txtMonto.EditValue = vehiculo.VlrFasecolda.Value;

                        vehiculoNew.RUNT_2.Value = this.txtRunt2.Text;
                        vehiculoNew.Confecamaras_2.Value = this.txtConfecamaras2.Text;
                        vehiculoNew.Placa_2.Value = this.txtPlaca2.Text;
                        vehiculoNew.FechaVto_2.Value = Convert.ToDateTime(this.dtFechaSOAT2.EditValue);
                        vehiculoNew.Aseguradora2VEH.Value = this.masterAseguradoraPrenda2.Value;
                        vehiculoNew.PolizaVEH2.Value = this.txtNumeroPolizaVEH2.Text;
                        vehiculoNew.FechaIniVEH2.Value = this.dtFechaPolizaIniVEH2.DateTime;
                        vehiculoNew.FechaFinVEH2.Value = this.dtFechaPolizaFinVEH2.DateTime;
                        vehiculoNew.VlrPolizaVEH2.Value = Convert.ToDecimal(this.txtValorRealPolizaVEH2.EditValue);
                        #endregion
                    }
                    if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                    {
                        #region datos Vivienda 2

                        vehiculoNew.Direccion_2.Value = this.txtDireccionHIP1.Text;
                        vehiculoNew.ValorAvaluo_2.Value = Convert.ToDecimal(this.txtVlrAvaluoComercialHIP1.EditValue);
                        vehiculoNew.Matricula_2.Value = this.txtFMIHIP1.Text;
                        vehiculoNew.ValorPredial_2.Value = Convert.ToDecimal(this.txtVlrAvaluoPredialHIP1.EditValue);

                        vehiculoNew.Escritura_2.Value = this.txtEscritura2.Text;
                        vehiculoNew.FechaEscritura_2.Value = this.dtFechaEscritura2.DateTime;
                        vehiculoNew.Notaria_2.Value = this.txtNotaria2.Text;

                        vehiculoNew.Aseguradora2HIP.Value = this.masterAseguradoraHipoteca2.Value;
                        vehiculoNew.PolizaHIP2.Value = this.txtNumeroPolizaHIP2.Text;
                        vehiculoNew.FechaIniHIP2.Value = this.dtFechaPolizaIniHIP2.DateTime;
                        vehiculoNew.FechaFinHIP2.Value = this.dtFechaPolizaFinHIP2.DateTime;
                        vehiculoNew.VlrPolizaHIP2.Value = Convert.ToDecimal(this.txtValorRealPolizaVEH2.EditValue);

                        #endregion
                    }
                    this._data.DatosVehiculo = vehiculoNew;


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
            this.masterVitrina.Value = this._VitrinaXDef;


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
            this._documentID = AppDocuments.DesembolsoSolicitud;
            this._frmModule = ModulesPrefix.dr;


            //Crea las opciones del combo servicio
            servicio.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Particular"));
            servicio.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Publico"));

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, false, true, true, false);

            this.cmbServicioGeneral.Properties.ValueMember = "Key";
            this.cmbServicioGeneral.Properties.DisplayMember = "Value";
            this.cmbServicioGeneral.Properties.DataSource = servicio;
            this.cmbServicioGeneral.EditValue = "0";

            this.cmbServicioGeneral2.Properties.ValueMember = "Key";
            this.cmbServicioGeneral2.Properties.DisplayMember = "Value";
            this.cmbServicioGeneral2.Properties.DataSource = servicio;
            this.cmbServicioGeneral2.EditValue = "0";

            this._bc.InitMasterUC(this.masterAseguradoraPrenda1, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraPrenda2, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraHipoteca1, AppMasters.ccAseguradora, false, true, true, false);
            this._bc.InitMasterUC(this.masterAseguradoraHipoteca2, AppMasters.ccAseguradora, false, true, true, false);


            // Deja en Modo Lectura datos Generales

            this.txtDeudor.ReadOnly = true;
            this.txtCedulaDeudor.ReadOnly = true;
            this.masterVitrina.Enabled = false;
            this.txtMarca.ReadOnly = true;
            this.txtLinea.ReadOnly = true;
            this.txtModelo.ReadOnly = true;
            this.cmbServicioGeneral.ReadOnly = true;
            this.chkCeroKM.Enabled = false;


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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "EnableHeader"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "CleanData"));
            }
        }

        /// <summary>
        /// Verifica que agregue Garantias
        /// </su-mmary>
        /// <returns></returns>
        private bool AdicionaGarantia()
        {
            List<DTO_glGarantiaControl> Garantias = new List<DTO_glGarantiaControl>();
            DTO_glGarantiaControl garantia1 = new DTO_glGarantiaControl();
            DTO_glGarantiaControl garantia2 = new DTO_glGarantiaControl();
            DTO_glGarantiaControl garantia3 = new DTO_glGarantiaControl();
            DTO_glGarantiaControl garantia4 = new DTO_glGarantiaControl();

            if (this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
            {

//*****
                List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();

                DTO_glConsultaFiltro Tipo = new DTO_glConsultaFiltro();
                Tipo.ValorFiltro = "1";

                #region Filtros Tipo de credito para Clientes nuevo
                Tipo.CampoFisico = "GarantiaTipo";
                Tipo.OperadorFiltro = OperadorFiltro.Igual;
                Tipo.OperadorSentencia = "AND";
                filtrosComplejos.Add(Tipo);
                #endregion


                DTO_glGarantia garan = (DTO_glGarantia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glGarantia, false, string.Empty, true,filtrosComplejos);

/// *****
                //garantia1.GarantiaID.Value= garan.GarantiaTipo.Value.ToString();
                garantia1.TerceroID.Value = this._data.SolicituDocu.ClienteID.Value;
                garantia1.DocumentoID.Value = AppDocuments.ControlGarantias;
                garantia1.PrefijoID.Value = this._data.DatosVehiculo.PrefijoPrenda.Value;
                garantia1.DocumentoNro.Value = this._data.DatosVehiculo.NumeroPrenda.Value;
                garantia1.Placa.Value = this._data.DatosVehiculo.Placa.Value;
                garantia1.Modelo.Value = Convert.ToInt16(this._data.DatosVehiculo.Modelo.Value);
                //garantia1.FuentePRE.Value = this._data.DatosVehiculo.DocPrenda.Value;
                garantia1.FaseColdaID.Value = this._data.DatosVehiculo.FasecoldaCod.Value;
                garantia1.VlrFuente.Value = this._data.DatosVehiculo.VlrFasecolda.Value;
                garantia1.Dato1.Value = this._data.DatosVehiculo.Chasis.Value;
                garantia1.Dato2.Value = this._data.DatosVehiculo.Serie.Value;
                garantia1.Dato3.Value = this._data.DatosVehiculo.Motor.Value;
                garantia1.ActivoInd.Value = true;
                garantia1.Consecutivo.Value = 0;
                garantia1.VehiculoTipo.Value = this._data.DatosVehiculo.Servicio.Value;

            }

            if (this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
            {
            }

            if (this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
            {
            }
            if (this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
            {
            }                

            return true;
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
                DTO_DigitacionCredito digCredito = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(this._libranzaID, this._actFlujo.ID.Value);
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);

                //Cálculo de la edad
                //DateTime fechaDesembolso = Convert.ToDateTime(this._data.OtrosDatos.FechaDesembolso.Value);
                DateTime fechaDesembolso = this._data.OtrosDatos.FechaDesembolso.Value.HasValue ? this._data.OtrosDatos.FechaDesembolso.Value.Value : DateTime.Now;
                TimeSpan difFecha = fechaDesembolso.Date.Subtract(deudor.FechaNacimiento.Value.Value);
                int edad = (int)Math.Floor((double)difFecha.Days / 365);
                Dictionary<string, decimal> compsNuevoValor = new Dictionary<string, decimal>();
                
                //object res2 = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._lineaCreditoID, this.vlrSolicitadoPrestamo, this.vlrSolicitadoPoliza,
                //    vlrGiro, plazo, plazoPol, this.edad, this.dtFechaCredito.DateTime, fechaCuota1, this.porInteres, this.porInteresPoliza,
                //    cta1Pol, vlrCuotaPol, this.liquidaAll, this._cuotasExtras, this.compsNuevoValor, this._ctrl.NumeroDoc.Value.Value, this.masterTipoCredito.Value, this.chkExcluyeCompExtra.Checked);

                object res = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._data.SolicituDocu.LineaCreditoID.Value, Convert.ToInt32(this._data.SolicituDocu.VlrSolicitado.Value), Convert.ToInt32(this._data.SolicituDocu.VlrPoliza.Value),
                             Convert.ToInt32(this._data.SolicituDocu.VlrGiro.Value), Convert.ToInt16(this._data.SolicituDocu.Plazo.Value), Convert.ToInt16(this._data.SolicituDocu.PlazoSeguro.Value), edad, fechaDesembolso, this.dtFechaPrimeraCuota.DateTime, Convert.ToDecimal(this._data.SolicituDocu.PorInteres.Value),Convert.ToDecimal(this._data.SolicituDocu.PorSeguro.Value),
                             1, 0, true, new List<DTO_Cuota>(), this.compsNuevoValor, this._ctrl.NumeroDoc.Value.Value,this._data.SolicituDocu.TipoCreditoID.Value, false);



                this._liquidador = (DTO_PlanDePagos)res; 
                if (this._liquidador.ComponentesAll.Count == 0)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                    MessageBox.Show(msg);
                    return false;
                }

                this._componentesVisibles = this._liquidador.ComponentesUsuario;
                this._componentesContab = this._liquidador.ComponentesAll;

                        
                int VlrCuota= this._liquidador.Cuotas[1].ValorCuota;

                //Valida que la grilla de componentes no este vacia
                if ((this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado) && this._componentesVisibles.Count <= 0)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded);
                    MessageBox.Show(msg);
                    return false;
                }

                if (this._liquidador.Cuotas.Count == 0)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_LiquidacionRequerida);
                    MessageBox.Show(msg);
                    return false;
                }

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "ValidateHeader"));
                return false;
            }

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
                    this._libranzaID = this._data.SolicituDocu.Libranza.Value.Value;

                }
                ////Se carga al final para autocalcular el valor del giro
                //#endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "GetValuesSolicitud"));             
            }
        }



 
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
                if (this._actFlujoLiq != null)
                {
                   // this._actividadChequeo = this._bc.AdministrationModel.glDocumentoChequeoLista_GetByNumeroDoc(this._ctrl.NumeroDoc.Value.Value);
                    this.actChequeo = this._bc.AdministrationModel.drSolicitudDatosChequeados_GetByActividadNumDoc(this._actFlujoLiq.ID.Value, this._ctrl.NumeroDoc.Value.Value, this._data.SolicituDocu.VersionNro.Value.Value);                    

                  //  if (this._actividadChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujoLiq.ID.Value);

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
                                chequeo.IncluidoInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 1 ? x.ChequeadoInd.Value.Value : false));

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
                    FormProvider.Master.itemUpdate.Visible = true;
                    FormProvider.Master.itemSave.Enabled = true;
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar Liquidación";
                }
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSendtoAppr.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "Form_FormClosed"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "txtVlrSolicitado_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.AssignValues(false);
                this.gvActividades.PostEditor();
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, false);

                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                    MessageBox.Show(string.Format(msg, this._libranzaID));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesembolsoSolicitud.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar el desembolso de la solicitud?  ");
                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                this.AssignValues(false);
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                    MessageBox.Show(string.Format(msg, this._libranzaID));
                    if (result.Result == ResultValue.OK)
                        FormProvider.CloseCurrent();

                    this._documentIDGarantia = AppDocuments.ControlGarantias;
                    string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                    DTO_TxResult results = _bc.AdministrationModel.glGarantiaControl_Add(this._documentID, this.Garantias);
                    FormProvider.Master.StopProgressBarThread(this._documentID);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas

        private void pnlGeneral_Enter(object sender, EventArgs e)
        {

        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 1;
            else if (this.tabControl.SelectedTabPageIndex == 1)
                this.tabControl.SelectedTabPageIndex = 0;
            //else if (this.tabControl.SelectedTabPageIndex == 2)
            //    this.tabControl.SelectedTabPageIndex = 1;

        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {

            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 1;
            else if (this.tabControl.SelectedTabPageIndex == 1)
            //    this.tabControl.SelectedTabPageIndex = 2;
            //else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.AdicionaGarantia();
        }

    }
 }

