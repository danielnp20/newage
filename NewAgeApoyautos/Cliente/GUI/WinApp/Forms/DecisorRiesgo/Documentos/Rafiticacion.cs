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
    public partial class Rafiticacion : FormWithToolbar
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

        //Datos por defecto
        private string _lugGeoXDef = String.Empty;
        private string _AseguradoraXDef = String.Empty;
        private string _ZonaXDef = String.Empty;
        private string _VitrinaXDef = String.Empty;

        //DTOs        
        private DTO_ccCliente _cliente = new DTO_ccCliente();
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_ccSolicitudDocu _headerSolicitud = new DTO_ccSolicitudDocu();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_drActividadChequeoLista> actChequeoBase = new List<DTO_drActividadChequeoLista>();
        private List<DTO_drSolicitudDatosChequeados> actChequeo = new List<DTO_drSolicitudDatosChequeados>();

        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        //Variables formulario (campos)
        private string _tipoCreditoID = string.Empty;
        private string _clienteID = string.Empty;
        private string _lineaCreditoID = string.Empty;
        private short _plazo = 0;
        private int _libranzaID;
        //private int _libranzaID = 0;
        private string _polizaID = string.Empty;
        private TipoCredito _tipoCredito = TipoCredito.Nuevo;

        //Valores temporales
        private decimal vlrLibranza = 0;
        private decimal vlrGiro = 0;
        private int vlrSolicitadoPrestamo = 0;
        private decimal porInteres = 0;
        private int edad = 0;


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
        public Rafiticacion()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Rafiticacion(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Rafiticacion(int libranza, bool readOnly)
        {
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
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RafiticacionFinanciera.cs", "RafiticacionFinanciera"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "Rafiticacion"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "AddComponentesCols"));
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
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteID.Value.ToString();
                    this.txtDeudor.Text = this._data.SolicituDocu.Nombre.Value.ToString();
                    this.masterVitrina.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                    this.masterZona.Value = this._data.SolicituDocu.ZonaID.Value;
                    this.masterCiudadGeneral.Value = this._data.SolicituDocu.Ciudad.Value;
                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        #region datos Vehiculo
                        this.cmbServicioGeneral.EditValue = vehiculo.Servicio.Value;
                        this.chkCeroKMGeneral.Checked = vehiculo.CeroKmInd.Value.Value;
                        //this.masterAseguradora.Value = this._data.SolicituDocu.AseguradoraID.Value;
                        //this.txtVlrPoliza.EditValue = this._data.SolicituDocu.VlrPoliza.Value.ToString();
                        //this.txtVlrOtroSeguro.EditValue = this._data.SolicituDocu.VlrOtrasFinanciaciones.Value.ToString();
                        //this.txtVlrGarantia.EditValue = this._data.SolicituDocu.VlrFinanciaSeguro.Value.ToString();
                        //this.chkPolizaValorOtros.Checked = this._data.SolicituDocu.CancelaContadoPolizaInd.Value.HasValue? this._data.SolicituDocu.CancelaContadoPolizaInd.Value.Value : false;
                        //this.chkPolizaCancelaContadoOtros.Checked = this._data.SolicituDocu.CancelaContadoOtrosSegInd.Value.HasValue? this._data.SolicituDocu.CancelaContadoOtrosSegInd.Value.Value : false;
                        //this.chkPolizaDiferenteCubriendo.Checked = this._data.SolicituDocu.IntermediarioExternoInd.Value.HasValue? this._data.SolicituDocu.IntermediarioExternoInd.Value.Value : false;
                        //this.rbTipoPrenda.EditValue = vehiculo.TipoPrenda.Value;
                        //this.txtPrenda.Text = vehiculo.NumeroPrenda.Value.ToString();
                        //this.cmbGarante1.EditValue = vehiculo.Garante.Value;
                        //this.txtNombreGarante1.Text = deudor.NombrePri.Value;
                        //this.txtDireccionGarante1.Text = string.Empty;
                        //this.masterCedulaGarante1.Value = string.Empty;
                        //this.masterCiudadGarante1.Value = string.Empty;
                        //this.txtNombreGarante2.Text = conyuge.NombrePri.Value;
                        //this.txtDireccionGarante2.Text = string.Empty;
                        //this.masterCedulaGarante2.Value = string.Empty;
                        //this.masterCiudadGarante2.Value = string.Empty;
                        //this.txtVlrVehiculoPrenda.EditValue = vehiculo.PrecioVenta.Value.ToString();
                        //this.txtMarcaPrenda.Text = vehiculo.Marca.Value.ToString();
                        //this.txtModeloPrenda.Text = vehiculo.Modelo.Value.ToString();
                        //this.txtMotorPrenda.Text = vehiculo.Motor.Value.ToString();
                        //this.txtSeriePrenda.Text = vehiculo.Serie.Value.ToString();
                        //this.txtClasePrenda.Text = vehiculo.Clase.Value.ToString();
                        //this.txtChasisPrenda.Text = vehiculo.Chasis.Value.ToString();
                        //this.txtPlacaPrenda.Text = vehiculo.Placa.Value.ToString();
                        //this.txtColorPrenda.Text = vehiculo.Color.Value.ToString();
                        //this.cmbServicioPrenda.EditValue = vehiculo.Servicio.Value;
                        //this.txtTipoVehiculoPrenda.Text = vehiculo.Tipo.Value.ToString();
                        //this.txtLineaPrenda.Text = vehiculo.Linea.ToString();
                        //this.rbDocumentoBase.EditValue = vehiculo.TipoPrenda.Value;
                        //this.txtDocumentoBaseDescr1.Text = vehiculo.TipoFactura.Value;
                        //this.txtDocumentoBaseDescr2.Text = vehiculo.NumeroFactura.Value;
                        //this.chkCeroKMPrenda.Checked = vehiculo.CeroKmInd.Value.Value;
                        #endregion 
                    }
                }
                else
                {
                    this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value : 1;
                    #region Llena datos de los controles para salvar
                    DTO_drSolicitudDatosVehiculo vehiculoNew = new DTO_drSolicitudDatosVehiculo();
                    // Datos Vehiculo
                    #region datos Vehiculo

                    //vehiculoNew.Marca.Value = this.txtMarca.Text;
                    //vehiculoNew.Linea.Value = this.txtLinea.Text;
                    //vehiculoNew.Modelo.Value = Convert.ToInt16(this.txtModelo.Text);
                    //vehiculoNew.Servicio.Value = !string.IsNullOrEmpty(this.cmbServicioGeneral.EditValue.ToString()) ? Convert.ToByte(this.cmbServicioGeneral.EditValue) : vehiculoNew.Servicio.Value;
                    //vehiculoNew.CeroKmInd.Value = this.chkCeroKMGeneral.Checked;
                    //this._data.SolicituDocu.AseguradoraID.Value = this.masterAseguradora.Value;
                    //this._data.SolicituDocu.VlrPoliza.Value = Convert.ToInt32(this.txtVlrPoliza.EditValue);
                    //this._data.SolicituDocu.VlrOtrasFinanciaciones.Value = Convert.ToInt32(this.txtVlrOtroSeguro.EditValue);
                    //this._data.SolicituDocu.VlrFinanciaSeguro.Value = Convert.ToInt32(this.txtVlrGarantia.EditValue);
                    //this._data.SolicituDocu.CancelaContadoPolizaInd.Value = this.chkPolizaValorOtros.Checked;
                    //this._data.SolicituDocu.CancelaContadoOtrosSegInd.Value = this.chkPolizaCancelaContadoOtros.Checked;
                    //this._data.SolicituDocu.IntermediarioExternoInd.Value = this.chkPolizaDiferenteCubriendo.Checked;
                    //vehiculoNew.TipoPrenda.Value = Convert.ToByte(this.rbTipoPrenda.EditValue);
                    //vehiculoNew.NumeroPrenda.Value = this.txtPrenda.Text;
                    //vehiculoNew.Garante.Value = !string.IsNullOrEmpty(this.cmbGarante1.EditValue.ToString()) ? Convert.ToByte(this.cmbGarante1.EditValue) : vehiculoNew.Garante.Value;

                    ////deudor.NombrePri.Value = this.txtNombreGarante1.Text;
                    ////string.Empty = this.txtDireccionGarante1.Text;
                    ////string.Empty = this.masterCedulaGarante1.Value;
                    ////string.Empty = this.masterCiudadGarante1.Value;
                    ////conyuge.NombrePri.Value = this.txtNombreGarante2.Text;
                    ////string.Empty = this.txtDireccionGarante2.Text;
                    ////string.Empty = this.masterCedulaGarante2.Value;
                    ////string.Empty = this.masterCiudadGarante2.Value;
                    //vehiculoNew.PrecioVenta.Value = Convert.ToInt32(this.txtVlrVehiculoPrenda.EditValue);
                    //vehiculoNew.Marca.Value = this.txtMarcaPrenda.Text;
                    //vehiculoNew.Modelo.Value = Convert.ToInt32(this.txtModeloPrenda.Text);
                    //vehiculoNew.Motor.Value = this.txtMotorPrenda.Text;
                    //vehiculoNew.Serie.Value = this.txtSeriePrenda.Text;
                    //vehiculoNew.Clase.Value = this.txtClasePrenda.Text;
                    //vehiculoNew.Chasis.Value = this.txtChasisPrenda.Text;
                    //vehiculoNew.Placa.Value = this.txtPlacaPrenda.Text;
                    //vehiculoNew.Color.Value = this.txtColorPrenda.Text;
                    //vehiculoNew.Servicio.Value = Convert.ToByte(this.cmbServicioPrenda.EditValue);
                    //vehiculoNew.Tipo.Value = this.txtTipoVehiculoPrenda.Text;

                    //vehiculoNew.TipoPrenda.Value = Convert.ToByte(this.rbDocumentoBase.EditValue);
                    //vehiculoNew.TipoFactura.Value = this.txtDocumentoBaseDescr1.Text;
                    //vehiculoNew.NumeroFactura.Value = this.txtDocumentoBaseDescr2.Text;
                    //vehiculoNew.CeroKmInd.Value = this.chkCeroKMPrenda.Checked;


                    this._data.DatosVehiculo = vehiculoNew;

                    #endregion 
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
            this._documentID = AppDocuments.RatificacionSolicitud;
            this._frmModule = ModulesPrefix.dr;

            //Crea las opciones del tipo de Reporte
            tipoReporte.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Solicitud de Credito"));// Formato
            tipoReporte.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Info de Asegurabilidad"));
            tipoReporte.Add(3, this._bc.GetResource(LanguageTypes.Tables, "Condiciones Generales"));
            tipoReporte.Add(4, this._bc.GetResource(LanguageTypes.Tables, "Pagare Credito"));
            tipoReporte.Add(5, this._bc.GetResource(LanguageTypes.Tables, "Carta Inst Pagare Credito"));
            tipoReporte.Add(6, this._bc.GetResource(LanguageTypes.Tables, "Pagare Seguros"));
            tipoReporte.Add(7, this._bc.GetResource(LanguageTypes.Tables, "Carta Inst Pagare Seguros"));
            tipoReporte.Add(8, this._bc.GetResource(LanguageTypes.Tables, "Prendas"));//Validar dos deudores
            tipoReporte.Add(9, this._bc.GetResource(LanguageTypes.Tables, "Condiciones Especificas"));// formato Impreso
            tipoReporte.Add(10, this._bc.GetResource(LanguageTypes.Tables, "Cert Grupo Deudores"));
            tipoReporte.Add(11, this._bc.GetResource(LanguageTypes.Tables, "Carta Envio Prendas"));

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadGeneral, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, false, true, true, false);

            this.cmbReportes.Properties.ValueMember = "Key";
            this.cmbReportes.Properties.DisplayMember = "Value";
            this.cmbReportes.Properties.DataSource = tipoReporte;
            this.cmbReportes.EditValue = "0";

            this.txtDeudor.ReadOnly = true;
            this.txtCedulaDeudor.ReadOnly = true;
            this.masterVitrina.Enabled = false;
            this.masterZona.Enabled = false;
            this.masterCiudadGeneral.Enabled = false;
            this.cmbServicioGeneral.ReadOnly = true;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "AddComponentesCols"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "EnableHeader"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "CleanData"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "ValidateHeader"));
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
                    this._cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this._clienteID, true);

                    this.vlrSolicitadoPrestamo = Convert.ToInt32(this._data.SolicituDocu.VlrSolicitado.Value);
                    int vlrGiro = Convert.ToInt32(this._data.SolicituDocu.VlrGiro.Value);
                    this._lineaCreditoID = this._data.SolicituDocu.LineaCreditoID.Value;
                    this._plazo = this._data.SolicituDocu.Plazo.Value.Value;
                    this.porInteres = 0;
                    this._libranzaID = this._data.SolicituDocu.Libranza.Value.Value;
                    this._polizaID = this._data.SolicituDocu.Poliza.Value;
                    this._data.SolicituDocu.PorInteres.Value = this._data.SolicituDocu.PorInteres.Value ?? 0;
                }
                ////Se carga al final para autocalcular el valor del giro
                //#endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "GetValuesSolicitud"));
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
                IncluidoInd.Caption = "√ Deudor";
                IncluidoInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoInd.VisibleIndex = 4;
                IncluidoInd.Width = 15;
                IncluidoInd.Visible = true;
                IncluidoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Deudor");
                IncluidoInd.AppearanceHeader.ForeColor = Color.Lime;
                IncluidoInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                IncluidoInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                IncluidoInd.AppearanceHeader.Options.UseTextOptions = true;
                IncluidoInd.AppearanceHeader.Options.UseFont = true;
                IncluidoInd.AppearanceHeader.Options.UseForeColor = true;
                this.gvActividades.Columns.Add(IncluidoInd);

                    //Incluido de Conyuge
                    GridColumn IncluidoConyInd = new GridColumn();
                    IncluidoConyInd.FieldName = this._unboundPrefix + "IncluidoConyugeInd";
                    IncluidoConyInd.Caption = "√ Conyuge";
                    IncluidoConyInd.UnboundType = UnboundColumnType.Boolean;
                    IncluidoConyInd.VisibleIndex = 5;
                    IncluidoConyInd.Width = 15;
                    IncluidoConyInd.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 2);//Conyuge;
                    IncluidoConyInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Conyuge");
                    IncluidoConyInd.AppearanceHeader.ForeColor = Color.Lime;
                    IncluidoConyInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    IncluidoConyInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    IncluidoConyInd.AppearanceHeader.Options.UseTextOptions = true;
                    IncluidoConyInd.AppearanceHeader.Options.UseFont = true;
                    IncluidoConyInd.AppearanceHeader.Options.UseForeColor = true;
                    this.gvActividades.Columns.Add(IncluidoConyInd);

                    //Incluido de Codeudor1
                    GridColumn IncluidoCodeudor1Ind = new GridColumn();
                    IncluidoCodeudor1Ind.FieldName = this._unboundPrefix + "IncluidoCodeudor1Ind";
                    IncluidoCodeudor1Ind.Caption = "√ Cod 1";
                    IncluidoCodeudor1Ind.UnboundType = UnboundColumnType.Boolean;
                    IncluidoCodeudor1Ind.VisibleIndex = 6;
                    IncluidoCodeudor1Ind.Width = 15;
                    IncluidoCodeudor1Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 3);//Codeudor1;
                    IncluidoCodeudor1Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Codeudor 1");
                    IncluidoCodeudor1Ind.AppearanceHeader.ForeColor = Color.Lime;
                    IncluidoCodeudor1Ind.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    IncluidoCodeudor1Ind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    IncluidoCodeudor1Ind.AppearanceHeader.Options.UseTextOptions = true;
                    IncluidoCodeudor1Ind.AppearanceHeader.Options.UseFont = true;
                    IncluidoCodeudor1Ind.AppearanceHeader.Options.UseForeColor = true;
                    this.gvActividades.Columns.Add(IncluidoCodeudor1Ind);

                    //Incluido de Codeudor2
                    GridColumn IncluidoCodeudor2Ind = new GridColumn();
                    IncluidoCodeudor2Ind.FieldName = this._unboundPrefix + "IncluidoCodeudor2Ind";
                    IncluidoCodeudor2Ind.Caption = "√  Cod 2";
                    IncluidoCodeudor2Ind.UnboundType = UnboundColumnType.Boolean;
                    IncluidoCodeudor2Ind.VisibleIndex = 7;
                    IncluidoCodeudor2Ind.Width = 15;
                    IncluidoCodeudor2Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 4);//Codeudor2;
                    IncluidoCodeudor2Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, this._documentID + " Cod 2");
                    IncluidoCodeudor2Ind.AppearanceHeader.ForeColor = Color.Lime;
                    IncluidoCodeudor2Ind.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    IncluidoCodeudor2Ind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    IncluidoCodeudor2Ind.AppearanceHeader.Options.UseTextOptions = true;
                    IncluidoCodeudor2Ind.AppearanceHeader.Options.UseFont = true;
                    IncluidoCodeudor2Ind.AppearanceHeader.Options.UseForeColor = true;
                    this.gvActividades.Columns.Add(IncluidoCodeudor2Ind);

                    //Incluido de Codeudor3
                    GridColumn IncluidoCodeudor3Ind = new GridColumn();
                    IncluidoCodeudor3Ind.FieldName = this._unboundPrefix + "IncluidoCodeudor3Ind";
                    IncluidoCodeudor3Ind.Caption = "√  Cod 3";
                    IncluidoCodeudor3Ind.UnboundType = UnboundColumnType.Boolean;
                    IncluidoCodeudor3Ind.VisibleIndex = 8;
                    IncluidoCodeudor3Ind.Width = 15;
                    IncluidoCodeudor3Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 5);//Codeudor3;
                    IncluidoCodeudor3Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, " Cod 3");
                    IncluidoCodeudor3Ind.AppearanceHeader.ForeColor = Color.Lime;
                    IncluidoCodeudor3Ind.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    IncluidoCodeudor3Ind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    IncluidoCodeudor3Ind.AppearanceHeader.Options.UseTextOptions = true;
                    IncluidoCodeudor3Ind.AppearanceHeader.Options.UseFont = true;
                    IncluidoCodeudor3Ind.AppearanceHeader.Options.UseForeColor = true;
                    this.gvActividades.Columns.Add(IncluidoCodeudor3Ind);

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
                if (this._actFlujo != null)
                {
                    this._actividadChequeo = this._bc.AdministrationModel.glDocumentoChequeoLista_GetByNumeroDoc(this._ctrl.NumeroDoc.Value.Value);
                    if (this._actividadChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujo.ID.Value);
                        foreach (DTO_drActividadChequeoLista basic in this.actChequeoBase)
                        {
                            DTO_glDocumentoChequeoLista chequeo = new DTO_glDocumentoChequeoLista();
                            bool addChequeo = false;
                            chequeo.NumeroDoc.Value = _ctrl.NumeroDoc.Value.Value;
                            chequeo.ActividadFlujoID.Value = basic.ActividadFlujoID.Value;
                            chequeo.Descripcion.Value = basic.Descripcion.Value;
                            chequeo.IncluidoInd.Value = false;
                            chequeo.EmpleadoInd.Value = basic.EmpleadoInd.Value.Value;
                            chequeo.PrestServiciosInd.Value = basic.PrestServiciosInd.Value.Value;
                            chequeo.TransportadorInd.Value = basic.TransportadorInd.Value.Value;
                            chequeo.IndependienteInd.Value = basic.IndependienteInd.Value.Value;
                            chequeo.PensionadoInd.Value = basic.PensionadoInd.Value.Value;
                            if (basic.EmpleadoInd.Value.Value)
                                addChequeo = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1) ? true : false;
                            else if (basic.PrestServiciosInd.Value.Value)
                                addChequeo = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2) ? true : false;
                            else if(basic.TransportadorInd.Value.Value)
                                addChequeo = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3) ? true : false;
                            else if(basic.IndependienteInd.Value.Value)
                                addChequeo = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4) ? true : false;
                            else if(basic.PensionadoInd.Value.Value)
                                addChequeo = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5) ? true : false;
                            if (addChequeo)
                                this._actividadChequeo.Add(chequeo);
                        }
                    }
                }
                this.gcActividades.DataSource = this._actividadChequeo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Ratificacion.cs", "LoadActividades"));
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
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "TBNew"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Rafiticacion.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar la ratificación de la solicitud?  ");
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas

        private void gvActividades_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    DTO_glDocumentoChequeoLista row = (DTO_glDocumentoChequeoLista)this.gvActividades.GetRow(e.FocusedRowHandle);
                    if (row != null && this._data != null)
                    {
                        if (row.EmpleadoInd.Value.Value)
                        {
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 1);
                            //if (this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 3))//Codeudor1
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 1);
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 1);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 1);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 1);

                            }
                             else
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;
                            }
                        }
                        else if (row.PrestServiciosInd.Value.Value)
                        {
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 2);
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 2);
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 2);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 2);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 2);

                            }
                            else
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;
                            }
                        }
                        else if (row.TransportadorInd.Value.Value)
                        {
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 3);
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 3);
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 3);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 3);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 3);
                            }
                            else
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;
                            }
                        }
                        else if (row.IndependienteInd.Value.Value)
                        {
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 4);
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 4);
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 4);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 4);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 4);
                            }
                            else
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;
                            }

                        }
                        else if (row.PensionadoInd.Value.Value)
                        {
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 5);
                            this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 5);
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 5);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 5);
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 5);
                            }
                            else
                            {
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionViabilidad.cs", "gvDocuments_FocusedRowChanged"));
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (this.cmbReportes.EditValue.ToString() == "1")
                this.documentReportID = AppReports.drRatificacion;
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
                this.documentReportID = AppReports.drCertGrupoDeudores ;
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

            DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranzaID));
            this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);



        }
    }
}
 
