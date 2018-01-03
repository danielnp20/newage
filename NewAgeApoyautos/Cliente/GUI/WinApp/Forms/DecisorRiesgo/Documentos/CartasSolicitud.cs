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
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CartasSolicitud : FormWithToolbar
    //    public partial class CartasSolicitud : ProcessForm
    {
        #region Variables


        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private int _firma;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        protected int documentReportID = 0;


        //Datos por defecto
        private string _lugGeoXDef = String.Empty;
        private string _AseguradoraXDef = String.Empty;
        private string _ZonaXDef = String.Empty;
        private string _VitrinaXDef = String.Empty;
        private Dictionary<string, string> servicio = null;
        private Dictionary<string, string> accion = null;
        private Dictionary<string, string> decision = null;

        //DTOs        
        private DTO_ccCliente _cliente = new DTO_ccCliente();
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_glActividadFlujo _actFlujoDev = new DTO_glActividadFlujo();
        private DTO_ccSolicitudDocu _headerSolicitud = new DTO_ccSolicitudDocu();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        private Dictionary<string, int> Datos = new Dictionary<string, int>();
        //Variables formulario (campos)
        private int _libranzaID;

        //Variables auxiliares del formulario
        private DateTime periodo;
        Dictionary<int, string> tipoReporte = new Dictionary<int, string>();
        Dictionary<string, decimal> compsNuevoValor = new Dictionary<string, decimal>();
        private bool readOnly = false;

        //Variables de mensajes
        private string msgFinDoc;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CartasSolicitud()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CartasSolicitud(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CartasSolicitud(int libranza, bool readOnly, int firma)
        {
            this._firma = firma;
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cf;
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddComponentesCols();

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
                    string ActFlujoReversion = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Evaluacion);

                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
                    this._actFlujoDev = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, ActFlujoReversion, true);
                }

                #endregion
                #region Carga Actividades a devolver
                //Carga la actividades a revertir
                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujoDev.ID.Value);
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

                string ActEvaluacion = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Evaluacion);


                this.lblCarta.Text = "Carta Segun Perfil";

                if (this._data.OtrosDatos != null)
                {

                    if (this._data.OtrosDatos.CartaAprobDirUsu.Value == null && this._data.OtrosDatos.CartaAprobDirFecha.Value == null && this._data.OtrosDatos.CartaAprobDirInd.Value.HasValue && this._data.OtrosDatos.CartaAprobDirInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drAprobacionDirectaSinDoc;
                        this.lblCarta.Text = "APROBACION DIRECTA SIN DOCUMENTOS' ";
                    }
                    else if (this._data.OtrosDatos.CartaAprobDocUsu.Value == null && this._data.OtrosDatos.CartaAprobDocFecha.Value == null && this._data.OtrosDatos.CartaAprobDocInd.Value.HasValue && this._data.OtrosDatos.CartaAprobDocInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drAprobacionDirectaConDoc;
                        this.lblCarta.Text = "APROBACION DIRECTA SUJETA A DOCUMENTOS";
                    }
                    else if (this._data.OtrosDatos.CartapreAprobUsu.Value == null && this._data.OtrosDatos.CartapreAprobFecha.Value == null && this._data.OtrosDatos.CartapreAprobInd.Value.HasValue && this._data.OtrosDatos.CartapreAprobInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drPreAprobacion;
                        this.lblCarta.Text = "PRE APROBACION ";
                    }
                    else if (this._data.OtrosDatos.CartaNoViableUsu.Value == null && this._data.OtrosDatos.CartaNoViableFecha.Value == null && this._data.OtrosDatos.CartaNoViableInd.Value.HasValue && this._data.OtrosDatos.CartaNoViableInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drNoViable;
                        this.lblCarta.Text = "NO VIABLE ";
                    }
                    else if (this._data.OtrosDatos.CartaRevocaUsu.Value == null && this._data.OtrosDatos.CartaRevocaFecha.Value == null && this._data.OtrosDatos.CartaRevocaInd.Value.HasValue && this._data.OtrosDatos.CartaRevocaInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drRevocacionAprobacion;
                        this.lblCarta.Text = "REVOCACION";
                    }
                    else if (this._data.OtrosDatos.CartaRatificaUsu.Value == null && this._data.OtrosDatos.CartaRatificaFecha.Value == null && this._data.OtrosDatos.CartaRatificaInd.Value.HasValue && this._data.OtrosDatos.CartaRatificaInd.Value.Value)
                    {
                        this.documentReportID = AppReports.drRatificacion;
                        this.lblCarta.Text = "RATIFICACION";
                    }
                }

                //if (this.documentReportID == 0)
                //{
                //    this.btnImprimir.Enabled = false;
                //    this.btnFirmar.Enabled = false;
                //}
                //else
                //{
                //    this.btnImprimir.Enabled = true;
                //    this.btnFirmar.Enabled = true;
                //}

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitudFinanciera.cs", "CartasSolicitudFinanciera"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "CartasSolicitud"));
            }
        }
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion de la solicitud
        /// </summary>
        /// <param name="libranzaNro">Libranza a consultar</param>
        private void LoadData(int? libranzaNro)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "AddComponentesCols"));
            }
        }

        /// <summary>
        /// Asigna o envia el valor de los campos relacionados en la solicitud
        /// </summary>
        /// <param name="isGet">valida si obtiene los datos o los asigna</param>
        private void AssignValues(bool isGet)
        {
            try
            {
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
                DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo;

                this._data.SolicituDocu.Nombre.Value = this._data.SolicituDocu.NombrePri.Value + " " + this._data.SolicituDocu.NombreSdo.Value + " " + this._data.SolicituDocu.ApellidoPri.Value + " " + this._data.SolicituDocu.ApellidoSdo.Value;
                if (isGet)
                {

                    this.txtDeudor.Text = this._data.SolicituDocu.Nombre.Value.ToString();
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteRadica.Value.ToString();
                    this.masterVitrina.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                    //this.masterVitrinaDesembolso.Value = this._data.SolicituDocu.Concesionario2.Value;

                    if (string.IsNullOrWhiteSpace(this._data.SolicituDocu.Concesionario2.ToString()))
                    {
                        this.masterVitrinaDesembolso.Value = this._data.SolicituDocu.Concesionario2.Value;
                        this.masterVitrinaDesembolso.Enabled = true;
                    }
                    else
                    {
                        this.masterVitrinaDesembolso.Value = this._data.SolicituDocu.Concesionario2.Value;
                        this.masterVitrinaDesembolso.Enabled = false;
                    }
                    this.masterCiudadGeneral.Value = this._data.SolicituDocu.Ciudad.Value;
                    this.masterZona.Value = this._data.SolicituDocu.ZonaID.Value;

                    if (string.IsNullOrWhiteSpace(this._data.OtrosDatos.VlrSolicitado.ToString()))
                    {
                        this.txtMontoSol.EditValue = this._data.SolicituDocu.VlrSolicitado.Value;
                    }
                    else
                    {
                        this.txtMontoSol.EditValue = this._data.OtrosDatos.VlrSolicitado.Value;
                    }

                    if (string.IsNullOrWhiteSpace(this._data.OtrosDatos.Plazo.ToString()))
                    {
                        this.txtPlazoSol.EditValue = this._data.SolicituDocu.Plazo.Value;
                    }
                    else
                    {
                        this.txtPlazoSol.EditValue = this._data.OtrosDatos.Plazo.Value;
                    }

                    this.cmbAccionP.EditValue = this._data.OtrosDatos.AccionSolicitud.Value;
                    this.txtPlazoP.EditValue = this._data.OtrosDatos.PF_PlazoFinal.Value;
                    this.txtMontoP.EditValue = this._data.OtrosDatos.PF_VlrMontoSOL.Value;
                    this.txtPorcMaximoP.EditValue = this._data.OtrosDatos.PF_PorMaxEstadoActual.Value;
                    this.txtTasaPerfil.EditValue = this._data.OtrosDatos.PF_TasaPerfilOBL.Value;
                    this.txtVlrGarantiaP.EditValue = this._data.OtrosDatos.PF_VlrGarantiaPerfil.Value;


                    this.cmbAccion.EditValue = this._data.OtrosDatos.AccionSolicitud1.Value;
                    this.txtPlazo.EditValue = this._data.OtrosDatos.PF_PlazoFinal1.Value;
                    this.txtMonto.EditValue = this._data.OtrosDatos.PF_VlrMontoFirma1.Value;
                    this.txtPorcMaximo.EditValue = this._data.OtrosDatos.PF_PorMaxFirma1.Value;
                    this.txtVlrGarantia.EditValue = this._data.OtrosDatos.PF_VlrGarantiaFirma1.Value;

                    this.cmbAccion2.EditValue = this._data.OtrosDatos.AccionSolicitud2.Value;
                    this.txtPlazoFirma2.EditValue = this._data.OtrosDatos.PF_PlazoFinal2.Value;
                    this.txtMontoFirma2.EditValue = this._data.OtrosDatos.PF_VlrMontoFirma2.Value;
                    this.txtPorcMaximoFirma2.EditValue = this._data.OtrosDatos.PF_PorMaxFirma2.Value;
                    this.txtVlrGarantiaFirma2.EditValue = this._data.OtrosDatos.PF_VlrGarantiaFirma2.Value;

                    this.cmbAccion3.EditValue = this._data.OtrosDatos.AccionSolicitud3.Value;
                    this.txtPlazoFirma3.EditValue = this._data.OtrosDatos.PF_PlazoFinal3.Value;
                    this.txtMontoFirma3.EditValue = this._data.OtrosDatos.PF_VlrMontoFirma3.Value;
                    this.txtPorcMaximoFirma3.EditValue = this._data.OtrosDatos.PF_PorMaxFirma3.Value;
                    this.txtTasaFirma3.EditValue = this._data.OtrosDatos.PF_TasaFirma3OBL.Value;
                    
                    this.txtVlrGarantiaFirma3.EditValue = this._data.OtrosDatos.PF_VlrGarantiaFirma3.Value;

                    //this.cmbAccion3.EditValue = this._data.OtrosDatos.AccionSolicitud3.Value;
                    //this.txtPlazoFirma3.EditValue = this._data.OtrosDatos.Plazo.Value;
                    //this.txtMontoFirma3.EditValue = this._data.OtrosDatos.VlrSolicitado.Value;
                    //this.txtPorcMaximoFirma3.EditValue = this._data.OtrosDatos.porMaximo.Value;
                    //this.txtVlrGarantiaFirma3.EditValue = this._data.OtrosDatos.VlrGarantia.Value;
                    if (this._firma == 1)
                    {


                        if (!string.IsNullOrWhiteSpace(this.txtMonto.Text))
                        {
                            this.cmbAccion.EditValue = this._data.OtrosDatos.AccionSolicitud.Value;
                            this.txtPlazo.EditValue = this._data.OtrosDatos.PF_PlazoFinal.Value;
                            this.txtMonto.EditValue = this._data.OtrosDatos.PF_VlrMontoSOL.Value;
                            this.txtPorcMaximo.EditValue = this._data.OtrosDatos.PF_TasaPerfilOBL.Value;
                            this.txtVlrGarantia.EditValue = this._data.OtrosDatos.PF_VlrGarantiaPerfil.Value;
                        }
                        this._data.OtrosDatos.UsuarioFirma1.Value = null;
                        this._data.OtrosDatos.UsuarioFirma2.Value = null;
                        this._data.OtrosDatos.UsuarioFirma3.Value = null;
                        this._data.OtrosDatos.FechaFirma1.Value = null;
                        this._data.OtrosDatos.FechaFirma2.Value = null;
                        this._data.OtrosDatos.FechaFirma3.Value = null;
                        this.txtMonto.Enabled = true;
                        this.txtPlazo.Enabled = true;
                        this.cmbAccion.Enabled = true;
                        this.cmbAccion.ReadOnly = false;
                        //this.txtVlrGarantia.Enabled = true;

                    }
                    if (this._firma == 2)
                    {
                        this.txtMontoFirma2.Enabled = true;
                        this.txtPlazoFirma2.Enabled = true;
                        this.cmbAccion2.Enabled = true;
                        this.cmbAccion2.ReadOnly = false;
                        //this.txtVlrGarantiaFirma2.Enabled = true;
                    }
                    if (this._firma == 3)
                    {

                        this.txtMontoFirma3.Enabled = true;
                        this.txtPorcMaximoFirma3.Enabled = true;
                        this.txtTasaFirma3.Enabled = true;
                        this.txtPlazoFirma3.Enabled = true;
                        this.txtVlrGarantiaFirma3.Enabled = true;
                        this.cmbAccion3.Enabled = true;
                        this.cmbAccion3.ReadOnly = false;
                    }


                    this.txtFirma1.Text = this._data.OtrosDatos.UsuarioFirma1.Value;
                    this.txtFirma2.Text = this._data.OtrosDatos.UsuarioFirma2.Value;
                    this.txtFirma3.Text = this._data.OtrosDatos.UsuarioFirma3.Value;
                    this.txtFechaFirma1.EditValue = this._data.OtrosDatos.FechaFirma1.Value;
                    this.txtFechaFirma2.EditValue = this._data.OtrosDatos.FechaFirma2.Value;
                    this.txtFechaFirma3.EditValue = this._data.OtrosDatos.FechaFirma3.Value;


                    this.Datos.Add("1", Convert.ToInt32(this._data.SolicituDocu.CtasPagadas.Value));
                    this.Datos.Add("2", Convert.ToInt32(this._data.SolicituDocu.AbonosCapital.Value));
                    this.Datos.Add("3", Convert.ToInt32(this._data.SolicituDocu.OblPrepagadas.Value));
                    this.Datos.Add("4", Convert.ToInt32(this._data.SolicituDocu.NroPrejuridicos.Value));


                    #region revisar oscar

                    //if (string.IsNullOrWhiteSpace(this._data.OtrosDatos.porMaximo.ToString()))
                    //{
                    //    this.txtPorcMaximo.EditValue = this._data.OtrosDatos.PF_PorMaxEstadoActual.Value;
                    //    this.txtPorcMaximo.ReadOnly = false;
                    //    this.txtPorcMaximo.Enabled = true;

                    //}
                    //else
                    //{
                    //    this.txtPorcMaximo.EditValue = this._data.OtrosDatos.porMaximo.Value;
                    //    this.txtPorcMaximo.ReadOnly = true;
                    //}



                    //if (string.IsNullOrWhiteSpace(this._data.OtrosDatos.VlrGarantia.ToString()))
                    //{
                    //    this.txtVlrGarantia.EditValue = this._data.OtrosDatos.VlrGarantia.Value;
                    //    this.txtVlrGarantia.Enabled = true;
                    //    this.txtVlrGarantia.Enabled = true;

                    //}
                    //else
                    //{
                    //    this.txtVlrGarantia.EditValue = this._data.OtrosDatos.VlrGarantia.Value;
                    //    this.txtVlrGarantia.Enabled = false;
                    //}

                    #endregion
                    //this.txtVlrGarantia.EditValue = this._data.OtrosDatos.VlrGarantia.Value;

                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        #region datos Vehiculo
                        this.txtMarca.Text = vehiculo.Marca.Value;
                        this.txtLinea.Text = vehiculo.Linea.Value;
                        this.txtModelo.Text = vehiculo.Modelo.Value.ToString();
                        this.cmbServicioGeneral.EditValue = vehiculo.Servicio.Value;
                        this.chkCeroKMGeneral.Checked = vehiculo.CeroKmInd.Value.Value;
                        #endregion
                    }
                }
                else
                {
                    string TopeMinF3 = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_TopeMinF3);
                    string CodDesistimiento = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_CodDesistimiento);                              

                    if (this._firma == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(this.cmbAccion.EditValue)))
                            this._data.OtrosDatos.AccionSolicitud1.Value = Convert.ToByte(this.cmbAccion.EditValue);
                        else
                            this._data.OtrosDatos.AccionSolicitud1.Value = null;
                        
                        this._data.OtrosDatos.PF_PlazoFinal1.Value = Convert.ToByte(this.txtPlazo.EditValue);
                        this._data.OtrosDatos.PF_VlrMontoFirma1.Value = Convert.ToDecimal(this.txtMonto.EditValue);
                        this._data.OtrosDatos.PF_PorMaxFirma1.Value = Convert.ToDecimal(this.txtPorcMaximo.EditValue);
                        this._data.OtrosDatos.PF_VlrGarantiaFirma1.Value = Convert.ToDecimal(this.txtVlrGarantia.EditValue);


                        if (!this.checkEdit3.Checked)
                        {
                            #region Validacion firma

                            //Si firma 1 es igual a perfil en monto , % maximo de financiacion y plazo pasa a firma 3
                            if (
                                this.txtMonto.Text.ToUpper().Equals(this.txtMontoP.Text.ToUpper())
                                && Convert.ToDecimal(this.txtPorcMaximo.EditValue).Equals(Convert.ToDecimal(this.txtPorcMaximoP.EditValue))
                                && Convert.ToDecimal(this.txtPlazo.EditValue).Equals(Convert.ToDecimal(this.txtPlazoP.EditValue))
                                )
                            {
                                this.checkEdit2.Checked = false;
                                this.checkEdit3.Checked = true;
                            }

                            //Si perfil es viable 
                            else if (Convert.ToByte(this.cmbAccionP.EditValue) == 1)
                            {
                                //Si perfil es viable y firma 1 dice Negacion (no viable) o disminuye monto , % maximo de financiacion y plazo pasa a firma 2
                                if (
                                    Convert.ToByte(this.cmbAccion.EditValue) == 3
                                    || Convert.ToInt32(this.txtMontoSol.EditValue) < Convert.ToInt32(this.txtMontoSol.EditValue)
                                    || Convert.ToInt32(this.txtPorcMaximo.EditValue) < Convert.ToInt32(this.txtPorcMaximoP.EditValue)
                                    || Convert.ToInt32(this.txtPlazo.EditValue) < Convert.ToInt32(this.txtPlazoP.EditValue)
                                    )
                                {
                                    this.checkEdit2.Checked = true;
                                    this.checkEdit3.Checked = false;
                                }
                                //Si perfil es viable y firma 1  dice Aprobacion  y  aumenta  monto , % maximo de financiacion o plazo pasa a firma 3
                                if (
                                    Convert.ToByte(this.cmbAccion.EditValue) == 1
                                    && (Convert.ToInt32(this.txtMontoSol.EditValue) > Convert.ToInt32(this.txtMontoSol.EditValue)
                                    || Convert.ToInt32(this.txtPorcMaximo.EditValue) > Convert.ToInt32(this.txtPorcMaximoP.EditValue)
                                    || Convert.ToInt32(this.txtPlazo.EditValue) > Convert.ToInt32(this.txtPlazoP.EditValue))
                                    )
                                {
                                    this.checkEdit2.Checked = false;
                                    this.checkEdit3.Checked = true;
                                }
                            }
                            else if (Convert.ToByte(this.cmbAccionP.EditValue) == 3)
                            {
                                //Si perfil es no viable y firma 1  dice aprobacion (viable) pasa a firma 3 
                                if (Convert.ToByte(this.cmbAccion.EditValue) == 1)
                                {
                                    this.checkEdit2.Checked = false;
                                    this.checkEdit3.Checked = true;
                                }
                                //Si perfil es no viable y firma 1  dice no aprobacion (no viable) se termina
                                if (Convert.ToByte(this.cmbAccion.EditValue) == 3)
                                {
                                    this._data.SolicituDocu.RechazoCausal.Value = CodDesistimiento;
                                    this._data.SolicituDocu.DesestimientoInd.Value = true;
                                    this._data.SolicituDocu.Observacion.Value = "Negocio no Viable";
                                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);
                                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                                    MessageBox.Show(string.Format(msg, this._libranzaID));
                                    if (result.Result == ResultValue.OK)
                                        FormProvider.CloseCurrent();
                                }

                            }

                            #endregion
                        }
                        if (this.checkEdit2.Checked)
                        {
                            this.cmbAccion2.EditValue = this._data.OtrosDatos.AccionSolicitud.Value;
                            this.txtPlazoFirma2.EditValue = Convert.ToDecimal(this.txtPlazo.EditValue);
                            this.txtMontoFirma2.EditValue = Convert.ToDecimal(this.txtMonto.EditValue);
                            this.txtPorcMaximoFirma2.EditValue = Convert.ToDecimal(this.txtPorcMaximo.EditValue);
                            this.txtVlrGarantiaFirma2.EditValue = Convert.ToDecimal(this.txtVlrGarantia.EditValue);

                        }
                        else if (this.checkEdit3.Checked)
                        {
                            this.cmbAccion3.EditValue = this._data.OtrosDatos.AccionSolicitud.Value;
                            this.txtPlazoFirma3.EditValue = Convert.ToDecimal(this.txtPlazo.EditValue);
                            this.txtMontoFirma3.EditValue = Convert.ToDecimal(this.txtMonto.EditValue);
                            this.txtPorcMaximoFirma3.EditValue = Convert.ToDecimal(this.txtPorcMaximo.EditValue);
                            this.txtTasaFirma3.EditValue = Convert.ToDecimal(this.txtTasaPerfil.EditValue);
                            this.txtVlrGarantiaFirma3.EditValue = Convert.ToDecimal(this.txtVlrGarantia.EditValue);

                        }


                    }
                    else if (this._firma == 2)
                    {

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(this.cmbAccion2.EditValue)))
                            this._data.OtrosDatos.AccionSolicitud2.Value = Convert.ToByte(this.cmbAccion2.EditValue);
                        else
                        {
                            MessageBox.Show("Debe Seleccionar una Carta");
                            this.cmbAccion2.Focus();
                            return ;
                        }

                                #region Validacion firma
                            //Si  perfil dice viable y firma 1 dice  Negacion (no viable) 
                            //y firna 2 dice Negacion( no viable) - Termina   -  DESISTIMIENTO (Crear codigo en control)					                            
                            if (
                                Convert.ToByte(this.cmbAccionP.EditValue) == 1 
                                && Convert.ToByte(this.cmbAccion.EditValue) == 3
                                && Convert.ToByte(this.cmbAccion2.EditValue) == 3
                                )
                            {

                                this._data.SolicituDocu.RechazoCausal.Value = CodDesistimiento;
                                this._data.SolicituDocu.DesestimientoInd.Value = true;
                                this._data.SolicituDocu.Observacion.Value = "Negocio no Viable";
                                DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                                DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                                MessageBox.Show(string.Format(msg, this._libranzaID));
                                if (result.Result == ResultValue.OK)
                                    FormProvider.CloseCurrent();

                            }
                            //Si hay acuerdo entre firma 1y firma 2 pasa a siguiente etapa sjeto al monto del credito
                            else if (
                                this.cmbAccion.Text.ToUpper().Equals(this.cmbAccion2.Text.ToUpper())
                                && Convert.ToDecimal(this.txtMonto.EditValue).Equals(Convert.ToDecimal(this.txtMontoFirma2.EditValue))
                                && Convert.ToDecimal(this.txtPorcMaximo.EditValue).Equals(Convert.ToDecimal(this.txtPorcMaximoFirma2.EditValue))
                                && Convert.ToDecimal(this.txtPlazo.EditValue).Equals(Convert.ToDecimal(this.txtPlazoFirma2.EditValue))
                                )
                            {
                                //Se debe incluir un monto minimo sobre el cual es obliatoria la firma 3					
                                if (Convert.ToDecimal(this.txtMontoFirma2.EditValue)<Convert.ToDecimal(TopeMinF3) ) // Cambiar por valor de Control                            
                                    this.checkEdit3.Checked=false;
                                else
                                    this.checkEdit3.Checked = true;
                            }
                            //Si no hay acuerdo entre firma 1y firma 2 pasa a firma 3					
                            else if (
                                !this.cmbAccion.Text.ToUpper().Equals(this.cmbAccion2.Text.ToUpper())                                
                                || !Convert.ToDecimal(this.txtMonto.EditValue).Equals(Convert.ToDecimal(this.txtMontoFirma2.EditValue))
                                || !Convert.ToDecimal(this.txtPorcMaximo.EditValue).Equals(Convert.ToDecimal(this.txtPorcMaximoFirma2.EditValue))
                                || !Convert.ToDecimal(this.txtPlazo.EditValue).Equals(Convert.ToDecimal(this.txtPlazoFirma2.EditValue))
                                )
                                this.checkEdit3.Checked = true;                            
                            #endregion
                

                        this._data.OtrosDatos.PF_PlazoFinal2.Value = Convert.ToByte(this.txtPlazoFirma2.EditValue);
                        this._data.OtrosDatos.PF_VlrMontoFirma2.Value = Convert.ToDecimal(this.txtMontoFirma2.EditValue);
                        this._data.OtrosDatos.PF_PorMaxFirma2.Value = Convert.ToDecimal(this.txtPorcMaximoFirma2.EditValue);
                        this._data.OtrosDatos.PF_VlrGarantiaFirma2.Value = Convert.ToDecimal(this.txtVlrGarantiaFirma2.EditValue);

                        this.cmbAccion3.EditValue = this._data.OtrosDatos.AccionSolicitud2.Value;
                        this.txtPlazoFirma3.EditValue = Convert.ToDecimal(this.txtPlazoFirma2.EditValue);
                        this.txtMontoFirma3.EditValue = Convert.ToDecimal(this.txtMontoFirma2.EditValue);
                        this.txtPorcMaximoFirma3.EditValue = Convert.ToDecimal(this.txtPorcMaximoFirma2.EditValue);
                        this.txtTasaFirma3.EditValue = Convert.ToDecimal(this.txtTasaPerfil.EditValue);
                        this.txtVlrGarantiaFirma3.EditValue = Convert.ToDecimal(this.txtVlrGarantiaFirma2.EditValue);
                        
                        if (!this.checkEdit3.Checked)
                        {
                            this._data.SolicituDocu.Plazo.Value = Convert.ToByte(this.txtPlazoFirma2.EditValue);
                            this._data.SolicituDocu.VlrSolicitado.Value = Convert.ToDecimal(this.txtMontoFirma2.EditValue);
                            this._data.SolicituDocu.PorInteres.Value = Convert.ToDecimal(this.txtTasaPerfil.EditValue);
                        }

                    }
                    else if (this._firma == 3)
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(this.cmbAccion3.EditValue)))
                            this._data.OtrosDatos.AccionSolicitud3.Value = Convert.ToByte(this.cmbAccion3.EditValue);
                        else
                        {
                            MessageBox.Show("Debe Seleccionar una Carta");
                            this.cmbAccion3.Focus();
                            return;
                        }

                        this._data.OtrosDatos.PF_PlazoFinal3.Value = Convert.ToByte(this.txtPlazoFirma3.EditValue);
                        this._data.OtrosDatos.PF_VlrMontoFirma3.Value = Convert.ToDecimal(this.txtMontoFirma3.EditValue);
                        this._data.OtrosDatos.PF_PorMaxFirma3.Value = Convert.ToDecimal(this.txtPorcMaximoFirma3.EditValue);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(this.txtTasaFirma3.EditValue)) && Convert.ToDecimal(this.txtTasaFirma3.EditValue) > 0)
                            this._data.OtrosDatos.PF_TasaFirma3OBL.Value = Convert.ToDecimal(this.txtTasaFirma3.EditValue);
                        else
                        {
                            MessageBox.Show("Debe Digitar una Tasa Valida");
                            this.txtTasaFirma3.Focus();
                            return;
                        }
                        
                        this._data.OtrosDatos.PF_VlrGarantiaFirma3.Value = Convert.ToDecimal(this.txtVlrGarantiaFirma3.EditValue);

                        this._data.SolicituDocu.Plazo.Value = Convert.ToByte(this.txtPlazoFirma3.EditValue);
                        this._data.SolicituDocu.VlrSolicitado.Value = Convert.ToDecimal(this.txtMontoFirma3.EditValue);
                        this._data.SolicituDocu.PorInteres.Value = Convert.ToDecimal(this.txtTasaFirma3.EditValue);

                    }
                    this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value : 1;
                    #region Llena datos de los controles para salvar
                    DTO_drSolicitudDatosVehiculo vehiculoNew = new DTO_drSolicitudDatosVehiculo();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "AssignValues"));
            }
        }

        /// <summary>
        /// Limpia los datos
        /// </summary>
        private void CleanData()
        {

            this.txtDeudor.Text = String.Empty;
            this.txtCedulaDeudor.Text = String.Empty;
            this.masterCiudadGeneral.Value = this._lugGeoXDef;
            this.masterVitrina.Value = this._VitrinaXDef;
            this.masterVitrinaDesembolso.Value = this._VitrinaXDef;
            this.masterZona.Value = this._ZonaXDef;


            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._data = new DTO_DigitaSolicitudDecisor();

            //Variables
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
            if (this._firma == 1)
            {
                this.checkEdit2.Visible = true;
                this.checkEdit2.Enabled = false;
                this.checkEdit3.Visible = true;
                this.label14.Visible = true;
                this.label15.Visible = true;
                this._documentID = AppDocuments.CartasSolicitud;

            }
            else if (this._firma == 2)
            {
                this.checkEdit2.Visible = false;
                this.checkEdit3.Visible = true;
                this.label14.Visible = false;
                this.label15.Visible = true;
                this._documentID = AppDocuments.Firma2;
            }
            else if (this._firma == 3)
            {
                this.checkEdit2.Visible = false;
                this.checkEdit3.Visible = false;
                this.label14.Visible = false;
                this.label15.Visible = false;

                this._documentID = AppDocuments.Firma3;
            }

            this._frmModule = ModulesPrefix.dr;

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, false, true, true, false);
            this._bc.InitMasterUC(this.masterVitrinaDesembolso, AppMasters.ccConcesionario, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadGeneral, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, false, true, true, false);

            this.servicio = new Dictionary<string, string>();
            this.servicio.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Servicio_v1"));
            this.servicio.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Servicio_v2"));

            this.cmbServicioGeneral.Properties.ValueMember = "Key";
            this.cmbServicioGeneral.Properties.DisplayMember = "Value";
            this.cmbServicioGeneral.Properties.DataSource = servicio;
            this.cmbServicioGeneral.EditValue = "1";

            this.accion = new Dictionary<string, string>();
            this.accion.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v1"));
            //this.accion.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v2"));
            this.accion.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v3"));

            this.decision = new Dictionary<string, string>();
            this.decision.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v1"));
            this.decision.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v2"));
            this.decision.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Accion_v3"));

            this.cmbAccionP.Properties.ValueMember = "Key";
            this.cmbAccionP.Properties.DisplayMember = "Value";
            this.cmbAccionP.Properties.DataSource = accion;
            this.cmbAccionP.EditValue = "1";

            this.cmbAccion.Properties.ValueMember = "Key";
            this.cmbAccion.Properties.DisplayMember = "Value";
            this.cmbAccion.Properties.DataSource = accion;
            this.cmbAccion.EditValue = "1";

            this.cmbAccion2.Properties.ValueMember = "Key";
            this.cmbAccion2.Properties.DisplayMember = "Value";
            this.cmbAccion2.Properties.DataSource = accion;
            this.cmbAccion2.EditValue = "1";

            this.cmbAccion3.Properties.ValueMember = "Key";
            this.cmbAccion3.Properties.DisplayMember = "Value";
            this.cmbAccion3.Properties.DataSource = decision;
            this.cmbAccion3.EditValue = "1";



            this.txtDeudor.ReadOnly = true;
            this.txtCedulaDeudor.ReadOnly = true;
            this.masterVitrina.Enabled = false;
            this.masterZona.Enabled = false;
            this.masterCiudadGeneral.Enabled = false;
            this.cmbServicioGeneral.ReadOnly = true;
            this.cmbAccion.ReadOnly = true;
            this.chkCeroKMGeneral.Enabled = false;

            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            //Carga los mensajes de la grilla
            this.msgFinDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFinDoc);
            this.EnableHeaderNuevo(false);

            //Ultimo consecutivo
            string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
            //this.txtUltSolicitud.Text = consecutivoSolStr;
        }

        /// <summary>
        /// Agrega las columnas a la grilla 1
        /// </summary>
        private void AddComponentesCols()
        {
            try
            {
                #region Doc Solicitud Componentes
                ////Campo de codigo
                //GridColumn codigo = new GridColumn();
                //codigo.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                //codigo.Caption = _bc.GetResource(LanguageTypes.Forms, "Componente");
                //codigo.UnboundType = UnboundColumnType.String;
                //codigo.VisibleIndex = 0;
                //codigo.Width = 50;
                //codigo.OptionsColumn.AllowEdit = false;
                //this.gvComponentes.Columns.Add(codigo);

                ////Descriptivo
                //GridColumn descriptivo = new GridColumn();
                //descriptivo.FieldName = this._unboundPrefix + "Descripcion";
                //descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, "Descripción");
                //descriptivo.UnboundType = UnboundColumnType.String;
                //descriptivo.VisibleIndex = 1;
                //descriptivo.Width = 100;
                //descriptivo.OptionsColumn.AllowEdit = false;
                //this.gvComponentes.Columns.Add(descriptivo);

                ////Valor Total
                //GridColumn valorTotal = new GridColumn();
                //valorTotal.FieldName = this._unboundPrefix + "TotalValor";
                //valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, "Valor Total");
                //valorTotal.UnboundType = UnboundColumnType.Decimal;
                //valorTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //valorTotal.AppearanceCell.Options.UseTextOptions = true;
                //valorTotal.VisibleIndex = 2;
                //valorTotal.Width = 150;
                //valorTotal.OptionsColumn.AllowEdit = false;
                //valorTotal.ColumnEdit = this.editSpin;
                //this.gvComponentes.Columns.Add(valorTotal);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "AddComponentesCols"));
            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData(bool cleanObligaciones)
        {
            try
            {
                //Footer
                this._ctrl = new DTO_glDocumentoControl();
                this._headerSolicitud = new DTO_ccSolicitudDocu();
                this.compsNuevoValor = new Dictionary<string, decimal>();

                //Ultimo Consecutivo
                string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
                //      this.txtUltSolicitud.Text = consecutivoSolStr;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "CleanData"));
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
                if (!this.masterVitrina.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterVitrina.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterVitrina.Focus();
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "ValidateHeader"));
                return false;
            }

            return true;
        }


        /// <summary>
        /// Verifica que la lista de chequeo este completa
        /// </summary>
        /// <returns></returns>
        private string ValidateListas()
        {
            string Falta = null;

            if (this._firma == 2 && !this.checkEdit3.Checked)
            {
                Falta += "\r\n" + "Firma 2 Cambia Datos: ";
                if (Convert.ToDecimal(this.txtMontoFirma2.EditValue) < Convert.ToDecimal(this.txtMonto.EditValue))
                    Falta += "\r\n" + "Monto:" + this.txtMontoFirma2.Text.ToString();
                if (Convert.ToInt32(this.txtPlazoFirma2.EditValue) > Convert.ToInt32(this.txtPlazo.EditValue))
                    Falta += "\r\n" + "Plazo:" + this.txtPlazoFirma2.Text.ToString();
                if (Convert.ToDecimal(this.txtVlrGarantiaFirma2.EditValue) > Convert.ToDecimal(this.txtVlrGarantia.EditValue))
                    Falta += "\r\n" + "Garantia:" + this.txtVlrGarantiaFirma2.Text.ToString();
            }
            else if (this._firma == 3 && this._firma == 2)
            {
                if (!string.IsNullOrWhiteSpace(this._data.OtrosDatos.UsuarioFirma2.Value))
                {
                    Falta += "\r\n" + "Firma 3 Cambia Datos: ";
                    if (Convert.ToDecimal(this.txtMontoFirma3.EditValue) < Convert.ToDecimal(this.txtMonto.EditValue))
                        Falta += "\r\n" + "Monto:" + this.txtMontoFirma3.EditValue.ToString();
                    if (Convert.ToInt32(this.txtPlazoFirma3.EditValue) > Convert.ToInt32(this.txtPlazo.EditValue))
                        Falta += "\r\n" + "Plazo:" + this.txtPlazoFirma3.EditValue.ToString();
                    if (Convert.ToDecimal(this.txtVlrGarantiaFirma3.EditValue) > Convert.ToDecimal(this.txtVlrGarantia.EditValue))
                        Falta += "\r\n" + "Garantia:" + this.txtVlrGarantiaFirma3.EditValue.ToString();
                }
                //else
                //{
                //    Falta += "\r\n" + "Firma 3 Cambia Datos: ";
                //    if (Convert.ToDecimal(this.txtMontoFirma3.EditValue) < Convert.ToDecimal(this.txtMontoFirma2.EditValue))
                //        Falta += "\r\n" + "Monto:" + this.txtMontoFirma3.EditValue.ToString();
                //    if (Convert.ToInt32(this.txtPlazoFirma3.EditValue) > Convert.ToInt32(this.txtPlazoFirma2.EditValue))
                //        Falta += "\r\n" + "Plazo:" + this.txtPorcMaximoFirma3.EditValue.ToString();
                //    if (Convert.ToDecimal(this.txtVlrGarantiaFirma3.EditValue) > Convert.ToDecimal(this.txtVlrGarantiaFirma2.EditValue))
                //        Falta += "\r\n" + "Garantia:" + this.txtVlrGarantiaFirma3.EditValue.ToString();
                //}
            }
            return Falta;
        }

        /// <summary>
        /// 
        /// Funcion que carga los valores previamente guardados
        /// </summary>
        private void SetValuesFromSolicitud()
        {
            try
            {
                if (this._data != null && this._data.SolicituDocu != null)
                {
                    //this._clienteID = !string.IsNullOrEmpty(this._data.SolicituDocu.ClienteID.Value) ? this._data.SolicituDocu.ClienteID.Value : this._data.SolicituDocu.ClienteRadica.Value;
                    //this._cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.ccCliente,false,this._clienteID,true);

                    //this.vlrSolicitadoPrestamo = Convert.ToInt32(this._data.SolicituDocu.VlrSolicitado.Value);
                    //int vlrGiro = Convert.ToInt32(this._data.SolicituDocu.VlrGiro.Value);
                    //this._lineaCreditoID = this._data.SolicituDocu.LineaCreditoID.Value;
                    //this._plazo = this._data.SolicituDocu.Plazo.Value.Value;
                    //this.porInteres = 0;               
                    //this._libranzaID = this._data.SolicituDocu.Libranza.Value.Value;
                    //this._polizaID = this._data.SolicituDocu.Poliza.Value;
                    //this._data.SolicituDocu.PorInteres.Value = this._data.SolicituDocu.PorInteres.Value ?? 0;
                }
                ////Se carga al final para autocalcular el valor del giro
                //#endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "GetValuesSolicitud"));
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

        /// <summary>
        /// Verifica que los campos obligatorios esten bn
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            string result = string.Empty;
            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);


            return true;
        }

                    #endregion

        #endregion

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
                    FormProvider.Master.itemPrint.Visible = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    FormProvider.Master.itemUpdate.Visible = false;
                    FormProvider.Master.itemSave.Visible = true;
                    FormProvider.Master.itemSendtoAppr.Visible = true;
                    //FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                    FormProvider.Master.itemSave.Enabled = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = true;
                    //FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    //FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar Legalización";
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario


        ///// <summary>
        ///// Changes in the form depending on user's operations
        ///// </summary>
        //private void cmbAccion_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.lblCarta.Text = "";
        //}

        private void cmbAccion2_EditValueChanged(object sender, EventArgs e)
        {
            this.lblCarta.Text = cmbAccion2.Text;
        }

        private void cmbAccion3_EditValueChanged(object sender, EventArgs e)
        {
            this.lblCarta.Text = cmbAccion3.Text;
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
        /// Abre los datos personales del solicitante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// Firma la carta segun el perfil actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirmar_Click(object sender, EventArgs e)
        {

            DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;
            DTO_drSolicitudDatosOtros otrosNew = otros != null ? otros : new DTO_drSolicitudDatosOtros();

            //            DTO_drSolicitudDatosOtros otrosNew = new DTO_drSolicitudDatosOtros();
            if (this._firma == 1)
            {
                otrosNew.Firma1Ind.Value = true;
                otrosNew.UsuarioFirma1.Value = this._bc.AdministrationModel.User.ID.Value;
                //otrosNew.FechaFirma1.Value = DateTime.Now.Date;
                otrosNew.FechaFirma1.Value = System.DateTime.Now;
                this.txtFirma1.Text = otrosNew.UsuarioFirma1.Value;
                this.txtFechaFirma1.EditValue = otrosNew.FechaFirma1.Value;
            }
            else if (this._firma == 2)
            {
                otrosNew.Firma2Ind.Value = true;
                otrosNew.UsuarioFirma2.Value = this._bc.AdministrationModel.User.ID.Value;
                otrosNew.FechaFirma2.Value = DateTime.Now.Date;
                this.txtFirma2.Text = otrosNew.UsuarioFirma2.Value;
                this.txtFechaFirma2.EditValue = otrosNew.FechaFirma2.Value;

            }
            else if (this._firma == 3)
            {

                //UsuarioFirma2
                otrosNew.Firma3Ind.Value = true;
                otrosNew.UsuarioFirma3.Value = this._bc.AdministrationModel.User.ID.Value;
                otrosNew.FechaFirma3.Value = DateTime.Now.Date;
                this.txtFirma3.Text = otrosNew.UsuarioFirma3.Value;
                this.txtFechaFirma3.EditValue = otrosNew.FechaFirma3.Value;

            }


            switch (this.documentReportID)
            {

                case AppReports.drAprobacionDirectaSinDoc:
                    otrosNew.CartaAprobDirUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartaAprobDirFecha.Value = DateTime.Now.Date;
                    break;
                case AppReports.drAprobacionDirectaConDoc:
                    otrosNew.CartaAprobDocUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartaAprobDocFecha.Value = DateTime.Now.Date;
                    break;
                case AppReports.drPreAprobacion:
                    otrosNew.CartapreAprobUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartapreAprobFecha.Value = DateTime.Now.Date;
                    break;
                case AppReports.drNoViable:
                    otrosNew.CartaNoViableUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartaNoViableFecha.Value = DateTime.Now.Date;
                    break;
                case AppReports.drRevocacionAprobacion:
                    otrosNew.CartaRevocaUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartaRevocaFecha.Value = DateTime.Now.Date;
                    break;
                case AppReports.drRatificacion:
                    otrosNew.CartaRatificaUsu.Value = this._bc.AdministrationModel.User.ID.Value;
                    otrosNew.CartaRatificaFecha.Value = DateTime.Now.Date;
                    break;
            }
            #region otrosdatos

            otrosNew.Version.Value = otros != null && otros.Version.Value.HasValue ? otros.Version.Value : Convert.ToByte(this._data.SolicituDocu.VersionNro.Value);
            otrosNew.NumeroDoc.Value = otros != null && otros.NumeroDoc.Value.HasValue ? otros.NumeroDoc.Value : Convert.ToInt32(this._data.SolicituDocu.NumeroDoc.Value);
            otrosNew.Consecutivo.Value = otros != null && otros.Consecutivo.Value.HasValue ? otros.Consecutivo.Value : null;
            this._data.OtrosDatos = otrosNew;
            MessageBox.Show(string.Format("Firma Incluida"));

            #endregion
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

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.masterVitrina.Value = string.Empty;
                this.CleanData(true);
                this.EnableHeaderNuevo(false);
                this.masterVitrina.Value = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "TBNew"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea Continuar el Proceso?  ");
                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;


                if (this._firma == 1 && (string.IsNullOrWhiteSpace(this.txtFechaFirma1.Text)))
                {
                    MessageBox.Show("Debe firmar antes de aprobar");
                    this.btnFirmar.Focus();
                    return;
                
                 }
                if (this._firma == 2 && (string.IsNullOrWhiteSpace(this.txtFechaFirma2.Text)))
                {
                    MessageBox.Show("Debe firmar antes de aprobar");
                    this.btnFirmar.Focus();
                    return;
                }
                if (this._firma == 3 && (string.IsNullOrWhiteSpace(this.txtFechaFirma3.Text)))
                {
                    MessageBox.Show("Debe firmar antes de aprobar");
                    this.btnFirmar.Focus();
                    return;
                }
                TBSave();
                string Observacion = this.ValidateListas();

                string msgDocAprob = "";
                msgDocAprob = this._bc.GetResource(LanguageTypes.Messages, "Hay Cambios desea enviar a Negocios para gestionar:" + Observacion);

                if (!string.IsNullOrEmpty(Observacion))
                {
                    if (MessageBox.Show(msgDocAprob, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                    else
                    {
                        this._data.SolicituDocu.NegociosGestionarInd.Value = true;
                        this._data.SolicituDocu.Observacion.Value = Observacion;
                    }
                }

                string ActFirma2 = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Firma2);
                string ActFirma3 = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Firma3);

                if (this.checkEdit2.Checked)
                    this._data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = ActFirma2;
                else if (this.checkEdit3.Checked)
                    this._data.SolicituDocu.ActividadFlujoNegociosGestionarID.Value = ActFirma3;

                this.AssignValues(false);
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

                   // string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                  //  MessageBox.Show(string.Format(msg, this._libranzaID));
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

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            Type frm = typeof(Perfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true });
        }

        private void btnDatosPersonales_Click(object sender, EventArgs e)
        {
            if (this._data != null)
            {
                Type frm = typeof(DigitacionSolicitudNuevos);
                FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, false });
            }
        }

        private void btnDatacredito_Click(object sender, EventArgs e)
        {
            Type frm = typeof(Datacredito);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.Libranza.Value, true });


        }

        //private void btnGenerarPerfil_Click(object sender, EventArgs e)
        //{
        //    DTO_TxResult _resPreliminar = _bc.AdministrationModel.Genera_Perfil(this._data.DocCtrl.PeriodoDoc.Value.Value, this._data.SolicituDocu.NumeroDoc.Value.Value);
        //    string msg = _resPreliminar.ResultMessage;
        //    MessageBox.Show(string.Format(msg, this._documentID.ToString()));

        //}

        /// <summary>
        /// Imprime la carta del perfil segun exista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.documentReportID != 0)
                {
                    string terceroEmpresa = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string responsFirma = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ResponsableFirmaCertif);
                    DTO_coTercero dtoFirma = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, responsFirma, true);
                    DTO_ccCliente dtoCliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.txtCedulaDeudor.Text, true);
                    DTO_glLugarGeografico dtoLugarGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, dtoCliente.ResidenciaCiudad.Value, true);

                    DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranzaID));
                    this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, new Dictionary<string, string>());
                }
                else
                    MessageBox.Show("No hay respuesta del perfil");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CartasSolicitud.cs", "btnImprimir_Click"));
            }

        }

        private void btnGarantias_Click(object sender, EventArgs e)
        {
            //DTO_TxResult _resPreliminar = _bc.AdministrationModel.Genera_ObligacionesGarantias(this._data.SolicituDocu.NumeroDoc.Value.Value,null);
            //string msg = _resPreliminar.ResultMessage;

            Type frm = typeof(ConsultaObligacionesGarantias);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value,Convert.ToInt32(this._data.SolicituDocu.NumeroDoc.Value), this.Datos,false });

        }

        private void btnDesestimiento_Click(object sender, EventArgs e)
        {
            ModalDesestimiento mod2 = null;

            mod2 = new ModalDesestimiento("Desestimiento", AppMasters.ccDevolucionCausal);
            mod2.ShowDialog();
            if (!string.IsNullOrEmpty(mod2._valorFinal))
            {
                this._data.SolicituDocu.RechazoCausal.Value = mod2._valorFinal;
                this._data.SolicituDocu.DesestimientoInd.Value = true;
                this._data.SolicituDocu.Observacion.Value = mod2._Observacion;
            }
            else
                return;
            DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
            DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
            MessageBox.Show(string.Format(msg, this._libranzaID));
            if (result.Result == ResultValue.OK)
                FormProvider.CloseCurrent();


        }

    }
}
 
