
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
using DevExpress.XtraGrid.Views.Base;
using System.Drawing;
using System.Globalization;
using System.Linq;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DigitacionSolicitudNuevos : FormWithToolbar
    {
        #region Variables
        bool validaVehiculo = false;

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_glActividadFlujo _actFlujoRevision = new DTO_glActividadFlujo();

        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeoDocumento = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_glDocumentoChequeoLista> _actividad = new List<DTO_glDocumentoChequeoLista>();

        private List<DTO_drActividadChequeoLista> actChequeoBase = new List<DTO_drActividadChequeoLista>();
        private List<DTO_drSolicitudDatosChequeados> actChequeo = new List<DTO_drSolicitudDatosChequeados>();

        //Identificador de la proxima actividad
        private string nextActID;

        //Variables formulario
        private string _clienteID = String.Empty;
        private string _pagaduriaID = String.Empty;
        private string _libranzaID = String.Empty;
        private string _centroPagoID = String.Empty;
        private string _tipoCreditoID = string.Empty;

        //Datos por defecto
        private string _asesorXDef = String.Empty;
        private string _comercioXDef = String.Empty;
        private string _lugGeoXDef = String.Empty;
        private string _centroPagoXDef = String.Empty;
        private string _pagaduriaXDef = String.Empty;
        private Dictionary<string, string> estadoCivil    = null;
        private Dictionary<string, string> fuenteIngresos = null;
        private Dictionary<string, string> tipoCaja  = null;
        private Dictionary<string, string> servicio = null;
        private Dictionary<string, string> tipoInmueble = null;
        //Variables ToolBar
        private bool _isLoaded;
        private bool _readOnly = false;
        private bool _verifica = false;
        private bool _aprueba = false;

        private DateTime periodo = DateTime.Now;

        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DigitacionSolicitudNuevos()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DigitacionSolicitudNuevos(string cliente, int libranzaNro, bool readOnly, bool verifica)
        {
            this.Constructor(cliente, libranzaNro, readOnly,verifica);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string cliente = null, int libranzaNro = 0, bool readOnly = false, bool verifica = false)
        {
            InitializeComponent();
            try
            {
                this._readOnly = readOnly;
                this._verifica = verifica;
                this.groupControl5.Visible = false;
                this.groupControl6.Visible = false;
                if(this._verifica)
                    this.tabControl.SelectedTabPageIndex = 6;
                
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.dr;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                
                if (this._frmModule == ModulesPrefix.dr && actividades.Count > 0)
                {
                    this.nextActID = string.Empty;
                    string actividadFlujoID = actividades[0];

                    string ActFlujoRevision = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Revision);

                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                    this._actFlujoRevision = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, ActFlujoRevision, true);
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
                    else
                    {
                        this.nextActID = NextActs[0];
                        //string actividadFlujoID = actividades[0];
                        //this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
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
                
                this.LoadData(cliente, libranzaNro);

                #region Trae la solicitud
                if(this._verifica)
                {
                    this.AddChequeoCols();
                    this.LoadActividades();
                    this.AddChequeoDocumentoCols();
                    this.LoadActividadesDocumento();
                }

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "Constructor"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                if (!this._verifica)
                {
                    this._documentID = AppDocuments.RegistroSolicitud;
                    this.tpvalidacion.PageVisible = false;
                    this.tpVerifica.PageVisible = false;
                }
                else
                    this._documentID = AppDocuments.VerificacionSolicitud;

                this._frmModule = ModulesPrefix.dr;

                //Carga la informacion de la maestras
                this._bc.InitMasterUC(this.masterTerceroDocTipoDeudor, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCony, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod1, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod2, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod3, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalDeudor, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCony, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod1, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod2, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod3, AppMasters.coActEconomica, true, true, true, false);

                this._bc.InitMasterUC(this.masterCiudadDeudor, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadCony, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadCod1, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadCod3, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadCod2, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
                this._bc.InitMasterUC(this.masterConcesionario, AppMasters.ccConcesionario, true, true, true, false);
                this._bc.InitMasterUC(this.masterConcesionario2, AppMasters.ccConcesionario, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);

                this._bc.InitMasterUC(this.masterFasecolda, AppMasters.ccFasecolda, false, true, true, false);
                this._bc.InitMasterUC(this.masterFasecolda2, AppMasters.ccFasecolda, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadHipoteca, AppMasters.glLugarGeografico, true, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadHipoteca2, AppMasters.glLugarGeografico, true, true, true, false);
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                //Asesor
                this._asesorXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);
                string asesorInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoDeAsesores);

                //this.masterComercio
                this._comercioXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PuntoComercialPorDefecto);
                string comercioInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoPuntosComerciales);

                //Lugar Geográfico
                this._lugGeoXDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Centro de Pago
                this._centroPagoXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);

                //Pagaduría
                this._pagaduriaXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PagaduriaXDefecto);

                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;

                this.linkConyugeInmueble.Dock = DockStyle.Fill;
                this.linkCodeudor1Inmueble.Dock = DockStyle.Fill;                
                this.linkCodeudor2Inmueble.Dock = DockStyle.Fill;
                this.linkCodeudor3Inmueble.Dock = DockStyle.Fill;

                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;                
                  
                this.estadoCivil = new Dictionary<string, string>();
                this.estadoCivil.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v0"));
                this.estadoCivil.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v1"));
                this.estadoCivil.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v2"));
                this.estadoCivil.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v3"));
                this.estadoCivil.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v4"));
                this.estadoCivil.Add("5  ", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v5"));

                this.fuenteIngresos = new Dictionary<string, string>();
                this.fuenteIngresos.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_FuenteIngresos_v1"));
                this.fuenteIngresos.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_FuenteIngresos_v2"));
                this.fuenteIngresos.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_FuenteIngresos_v3"));
                this.fuenteIngresos.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_FuenteIngresos_v4"));
                this.fuenteIngresos.Add("5  ", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_FuenteIngresos_v5"));


                this.tipoCaja = new Dictionary<string, string>();
                this.tipoCaja.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCaja_v1"));
                this.tipoCaja.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCaja_v2"));
                this.tipoCaja.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCaja_v3"));

                this.servicio = new Dictionary<string, string>();
                this.servicio.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Servicio_v1"));
                this.servicio.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Servicio_v2"));

                this.tipoInmueble= new Dictionary<string, string>();
                this.tipoInmueble.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v0"));
                this.tipoInmueble.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v1"));
                this.tipoInmueble.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v2"));
                this.tipoInmueble.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v3"));
                this.tipoInmueble.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v4"));
                this.tipoInmueble.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v5"));
                this.tipoInmueble.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoInmueble_v6"));
               


                this.cmbEstadoCivilDeudor.Properties.ValueMember = "Key";
                this.cmbEstadoCivilDeudor.Properties.DisplayMember = "Value";
                this.cmbEstadoCivilDeudor.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivilDeudor.EditValue = "0";

                this.cmbEstadoCivilCony.Properties.ValueMember = "Key";
                this.cmbEstadoCivilCony.Properties.DisplayMember = "Value";
                this.cmbEstadoCivilCony.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivilCony.EditValue = "0";

                this.cmbEstadoCivilCod1.Properties.ValueMember = "Key";
                this.cmbEstadoCivilCod1.Properties.DisplayMember = "Value";
                this.cmbEstadoCivilCod1.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivilCod1.EditValue = "0";

                this.cmbEstadoCivilCod2.Properties.ValueMember = "Key";
                this.cmbEstadoCivilCod2.Properties.DisplayMember = "Value";
                this.cmbEstadoCivilCod2.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivilCod2.EditValue = "0";

                this.cmbEstadoCivilCod3.Properties.ValueMember = "Key";
                this.cmbEstadoCivilCod3.Properties.DisplayMember = "Value";
                this.cmbEstadoCivilCod3.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivilCod3.EditValue = "0";

                this.cmbFuente1Deudor.Properties.ValueMember = "Key";
                this.cmbFuente1Deudor.Properties.DisplayMember = "Value";
                this.cmbFuente1Deudor.Properties.DataSource = fuenteIngresos;
                this.cmbFuente1Deudor.EditValue = "0";

                this.cmbFuente1Cony.Properties.ValueMember = "Key";
                this.cmbFuente1Cony.Properties.DisplayMember = "Value";
                this.cmbFuente1Cony.Properties.DataSource = fuenteIngresos;
                this.cmbFuente1Cony.EditValue = "0";

                this.cmbFuente1Cod1.Properties.ValueMember = "Key";
                this.cmbFuente1Cod1.Properties.DisplayMember = "Value";
                this.cmbFuente1Cod1.Properties.DataSource = fuenteIngresos;
                this.cmbFuente1Cod1.EditValue = "0";

                this.cmbFuente1Cod2.Properties.ValueMember = "Key";
                this.cmbFuente1Cod2.Properties.DisplayMember = "Value";
                this.cmbFuente1Cod2.Properties.DataSource = fuenteIngresos;
                this.cmbFuente1Cod2.EditValue = "0";

                this.cmbFuente1Cod3.Properties.ValueMember = "Key";
                this.cmbFuente1Cod3.Properties.DisplayMember = "Value";
                this.cmbFuente1Cod3.Properties.DataSource = fuenteIngresos;
                this.cmbFuente1Cod3.EditValue = "0";

                this.cmbFuente2Deudor.Properties.ValueMember = "Key";
                this.cmbFuente2Deudor.Properties.DisplayMember = "Value";
                this.cmbFuente2Deudor.Properties.DataSource = fuenteIngresos;
                this.cmbFuente2Deudor.EditValue = "0";

                this.cmbFuente2Cony.Properties.ValueMember = "Key";
                this.cmbFuente2Cony.Properties.DisplayMember = "Value";
                this.cmbFuente2Cony.Properties.DataSource = fuenteIngresos;
                this.cmbFuente2Cony.EditValue = "0";

                this.cmbFuente2Cod1.Properties.ValueMember = "Key";
                this.cmbFuente2Cod1.Properties.DisplayMember = "Value";
                this.cmbFuente2Cod1.Properties.DataSource = fuenteIngresos;
                this.cmbFuente2Cod1.EditValue = "0";

                this.cmbFuente2Cod2.Properties.ValueMember = "Key";
                this.cmbFuente2Cod2.Properties.DisplayMember = "Value";
                this.cmbFuente2Cod2.Properties.DataSource = fuenteIngresos;
                this.cmbFuente2Cod2.EditValue = "0";

                this.cmbFuente2Cod3.Properties.ValueMember = "Key";
                this.cmbFuente2Cod3.Properties.DisplayMember = "Value";
                this.cmbFuente2Cod3.Properties.DataSource = fuenteIngresos;
                this.cmbFuente2Cod3.EditValue = "0";

                this.cmbTipoCaja.Properties.ValueMember = "Key";
                this.cmbTipoCaja.Properties.DisplayMember = "Value";
                this.cmbTipoCaja.Properties.DataSource = tipoCaja;
                this.cmbTipoCaja.EditValue = "1";

                this.cmbFasecoldaTipoCaja.Properties.ValueMember = "Key";
                this.cmbFasecoldaTipoCaja.Properties.DisplayMember = "Value";
                this.cmbFasecoldaTipoCaja.Properties.DataSource = tipoCaja;
                this.cmbFasecoldaTipoCaja.EditValue = "1";


                this.cmbServicio.Properties.ValueMember = "Key";
                this.cmbServicio.Properties.DisplayMember = "Value";
                this.cmbServicio.Properties.DataSource = servicio;
                this.cmbServicio.EditValue = "1";

                this.cmbServicioFasecolda.Properties.ValueMember = "Key";
                this.cmbServicioFasecolda.Properties.DisplayMember = "Value";
                this.cmbServicioFasecolda.Properties.DataSource = servicio;
                this.cmbServicioFasecolda.EditValue = "1";

                this.cmbTipoInmueble.Properties.ValueMember = "Key";
                this.cmbTipoInmueble.Properties.DisplayMember = "Value";
                this.cmbTipoInmueble.Properties.DataSource = tipoInmueble;
                this.cmbTipoInmueble.EditValue = "1";

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {

            this.txtPriApellidoDeudor.Text = String.Empty;
            this.txtSdoApellidoDeudor.Text = String.Empty;
            this.txtPriNombreDeudor.Text = String.Empty;
            this.txtSdoNombreDeudor.Text = String.Empty;
            this.txtValorIngDeud.Text = "0";
            this.masterTerceroDocTipoDeudor.Value = this._lugGeoXDef;

            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._data = new DTO_DigitaSolicitudDecisor();

            //Variables
            this._clienteID = String.Empty;
            this._pagaduriaID = String.Empty;
            this._centroPagoID = String.Empty;
            this._tipoCreditoID = string.Empty;

            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {

            //this.cmbActividades.Enabled = this._readOnly;

            this.txtPriApellidoDeudor.ReadOnly = !enabled;
            this.txtSdoApellidoDeudor.ReadOnly = !enabled;
            this.txtPriNombreDeudor.ReadOnly = !enabled;
            this.txtSdoNombreDeudor.ReadOnly = !enabled;

            this.cmbActividades.Visible = this._verifica;
            this.chkDeudorDatacredito.Enabled = !this._readOnly;
            this.chkConyugeDatacredito.Enabled = !this._readOnly;
            this.chkCod1Datacredito.Enabled = !this._readOnly;
            this.chkCod2Datacredito.Enabled = !this._readOnly;
            this.chkCod3Datacredito.Enabled = !this._readOnly;


            #region vehiculo1
            this.txtMarca.Enabled = !this._readOnly;
            this.txtLinea.Enabled = !this._readOnly;
            this.txtReferencia.Enabled = !this._readOnly;
            this.txtCilindraje.Enabled = !this._readOnly;
            this.cmbTipoCaja.Enabled = !this._readOnly;
            this.txtComplemento.Enabled = !this._readOnly;
            this.chkTermoking.Enabled = !this._readOnly;
            this.chkAire.Enabled = !this._readOnly;
            this.txtPuertas.Enabled = !this._readOnly;
            this.chkTermoking.Enabled = !this._readOnly;

            this.masterAsesor.Enabled = !this._readOnly;
            this.cmbServicio.Enabled = !this._readOnly;
            this.chkCeroKM.Enabled = !this._readOnly;
            this.txtModelo.Enabled = !this._readOnly;
            this.txtPrecioVenta.Enabled = !this._readOnly;
            this.txtCIRegistrada.Enabled = !this._readOnly;
            this.chkChasis.Enabled = !this._readOnly;

            this.masterFasecolda.Enabled = !this._readOnly;
            this.txtFasecoldaValor.Enabled = !this._readOnly;
            this.txtFasecoldaValor.ReadOnly = this._readOnly;
            #endregion vehiculo 1

            #region vehiculo2
            this.txtMarca2.Enabled = !this._readOnly;
            this.txtLinea2.Enabled = !this._readOnly;
            this.txtReferencia2.Enabled = !this._readOnly;
            this.txtCilindraje2.Enabled = !this._readOnly;
            this.cmbTipoCaja2.Enabled = !this._readOnly;
            this.txtComplemento2.Enabled = !this._readOnly;
            this.chkTermoking2.Enabled = !this._readOnly;
            this.chkAire2.Enabled = !this._readOnly;
            this.txtPuertas2.Enabled = !this._readOnly;
            this.chkTermoking2.Enabled = !this._readOnly;

            this.cmbServicio2.Enabled = !this._readOnly;
            this.chkCeroKM2.Enabled = !this._readOnly;
            this.txtModelo2.Enabled = !this._readOnly;
            this.txtPrecioVenta2.Enabled = !this._readOnly;
            this.txtCIRegistrada2.Enabled = !this._readOnly;
            this.chkChasis2.Enabled = !this._readOnly;

            this.masterFasecolda2.Enabled = !this._readOnly;
            this.txtFasecoldaValor2.Enabled = !this._readOnly;
            this.txtFasecoldaValor2.ReadOnly = this._readOnly;
            #endregion vehiculo 2

            #region inmueble1
            this.cmbTipoInmueble.Enabled = !this._readOnly;
            this.txtDireccion.Enabled = !this._readOnly;
            this.txtFMI.Enabled = !this._readOnly;
            this.masterCiudadHipoteca.Enabled = !this._readOnly;
            this.chkViviendaNueva.Enabled = !this._readOnly;
            this.dtFechaComercial.Enabled = !this._readOnly;
            this.txtValorComercial.Enabled = !this._readOnly;
            this.dtFechaPredial.Enabled = !this._readOnly;
            this.txtValorPredial.Enabled = !this._readOnly;
            this.dtFechaCompraventa.Enabled = !this._readOnly;
            this.txtValorCompraventa.Enabled = !this._readOnly;
            #endregion

            #region inmueble2
            this.cmbTipoInmueble2.Enabled = !this._readOnly;
            this.txtDireccion2.Enabled = !this._readOnly;
            this.txtFMI2.Enabled = !this._readOnly;
            this.masterCiudadHipoteca2.Enabled = !this._readOnly;
            this.chkViviendaNueva2.Enabled = !this._readOnly;
            this.dtFechaComercial2.Enabled = !this._readOnly;
            this.txtValorComercial2.Enabled = !this._readOnly;
            this.dtFechaPredial2.Enabled = !this._readOnly;
            this.txtValorPredial2.Enabled = !this._readOnly;
            this.dtFechaCompraventa2.Enabled = !this._readOnly;
            this.txtValorCompraventa2.Enabled = !this._readOnly;
            #endregion

            this.chkAddHipoteca.Enabled = !this._readOnly;
            this.chkAddPrenda.Enabled = !this._readOnly;
            this.chkAddHipoteca2.Enabled = !this._readOnly;
            this.chkAddPrenda2.Enabled = !this._readOnly;

            #region validaciones
            #region deudor
            this.checkDeudor1.Enabled = this._verifica;
            this.checkDeudor2.Enabled = this._verifica;
            this.checkDeudor3.Enabled = this._verifica;
            this.checkDeudor4.Enabled = this._verifica;
            this.checkDeudor5.Enabled = this._verifica;
            this.checkDeudor6.Enabled = this._verifica;
            this.checkDeudor7.Enabled = this._verifica;
            this.checkDeudor8.Enabled = this._verifica;
            this.checkDeudor9.Enabled = this._verifica;
            this.checkDeudor10.Enabled = this._verifica;
            this.checkDeudor11.Enabled = this._verifica;
            this.checkDeudor12.Enabled = this._verifica;
            this.checkDeudor13.Enabled = this._verifica;
            this.checkDeudor14.Enabled = this._verifica;
            this.checkDeudor15.Enabled = this._verifica;
            this.checkDeudor16.Enabled = this._verifica;
            this.checkDeudor17.Enabled = this._verifica;
            this.checkDeudor18.Enabled = this._verifica;
            this.checkDeudor19.Enabled = this._verifica;
            this.checkDeudor20.Enabled = this._verifica;
            this.checkDeudor21.Enabled = this._verifica;
            this.checkDeudor22.Enabled = this._verifica;
            this.checkDeudor23.Enabled = this._verifica;
            #endregion

            #region conyuge
            this.checkCony1.Enabled = this._verifica;
            this.checkCony2.Enabled = this._verifica;
            this.checkCony3.Enabled = this._verifica;
            this.checkCony4.Enabled = this._verifica;
            this.checkCony5.Enabled = this._verifica;
            this.checkCony6.Enabled = this._verifica;
            this.checkCony7.Enabled = this._verifica;
            this.checkCony8.Enabled = this._verifica;
            this.checkCony9.Enabled = this._verifica;
            this.checkCony10.Enabled = this._verifica;
            this.checkCony11.Enabled = this._verifica;
            this.checkCony12.Enabled = this._verifica;
            this.checkCony13.Enabled = this._verifica;
            this.checkCony14.Enabled = this._verifica;
            this.checkCony15.Enabled = this._verifica;
            this.checkCony16.Enabled = this._verifica;
            this.checkCony17.Enabled = this._verifica;
            this.checkCony18.Enabled = this._verifica;
            this.checkCony19.Enabled = this._verifica;
            this.checkCony20.Enabled = this._verifica;
            this.checkCony21.Enabled = this._verifica;
            this.checkCony22.Enabled = this._verifica;
            this.checkCony23.Enabled = this._verifica;
            #endregion

            #region Codeudor1
            this.checkCod11.Enabled = this._verifica;
            this.checkCod12.Enabled = this._verifica;
            this.checkCod13.Enabled = this._verifica;
            this.checkCod14.Enabled = this._verifica;
            this.checkCod15.Enabled = this._verifica;
            this.checkCod16.Enabled = this._verifica;
            this.checkCod17.Enabled = this._verifica;
            this.checkCod18.Enabled = this._verifica;
            this.checkCod19.Enabled = this._verifica;
            this.checkCod110.Enabled = this._verifica;
            this.checkCod111.Enabled = this._verifica;
            this.checkCod112.Enabled = this._verifica;
            this.checkCod113.Enabled = this._verifica;
            this.checkCod114.Enabled = this._verifica;
            this.checkCod115.Enabled = this._verifica;
            this.checkCod116.Enabled = this._verifica;
            this.checkCod117.Enabled = this._verifica;
            this.checkCod118.Enabled = this._verifica;
            this.checkCod119.Enabled = this._verifica;
            this.checkCod120.Enabled = this._verifica;
            this.checkCod121.Enabled = this._verifica;
            this.checkCod122.Enabled = this._verifica;
            this.checkCod123.Enabled = this._verifica;
            #endregion

            #region Codeudor2
            this.checkCod21.Enabled = this._verifica;
            this.checkCod22.Enabled = this._verifica;
            this.checkCod23.Enabled = this._verifica;
            this.checkCod24.Enabled = this._verifica;
            this.checkCod25.Enabled = this._verifica;
            this.checkCod26.Enabled = this._verifica;
            this.checkCod27.Enabled = this._verifica;
            this.checkCod28.Enabled = this._verifica;
            this.checkCod29.Enabled = this._verifica;
            this.checkCod210.Enabled = this._verifica;
            this.checkCod211.Enabled = this._verifica;
            this.checkCod212.Enabled = this._verifica;
            this.checkCod213.Enabled = this._verifica;
            this.checkCod214.Enabled = this._verifica;
            this.checkCod215.Enabled = this._verifica;
            this.checkCod216.Enabled = this._verifica;
            this.checkCod217.Enabled = this._verifica;
            this.checkCod218.Enabled = this._verifica;
            this.checkCod219.Enabled = this._verifica;
            this.checkCod220.Enabled = this._verifica;
            this.checkCod221.Enabled = this._verifica;
            this.checkCod222.Enabled = this._verifica;
            this.checkCod223.Enabled = this._verifica;
            #endregion

            #region Codeudor3
            this.checkCod31.Enabled = this._verifica;
            this.checkCod32.Enabled = this._verifica;
            this.checkCod33.Enabled = this._verifica;
            this.checkCod34.Enabled = this._verifica;
            this.checkCod35.Enabled = this._verifica;
            this.checkCod36.Enabled = this._verifica;
            this.checkCod37.Enabled = this._verifica;
            this.checkCod38.Enabled = this._verifica;
            this.checkCod39.Enabled = this._verifica;
            this.checkCod310.Enabled = this._verifica;
            this.checkCod311.Enabled = this._verifica;
            this.checkCod312.Enabled = this._verifica;
            this.checkCod313.Enabled = this._verifica;
            this.checkCod314.Enabled = this._verifica;
            this.checkCod315.Enabled = this._verifica;
            this.checkCod316.Enabled = this._verifica;
            this.checkCod317.Enabled = this._verifica;
            this.checkCod318.Enabled = this._verifica;
            this.checkCod319.Enabled = this._verifica;
            this.checkCod320.Enabled = this._verifica;
            this.checkCod321.Enabled = this._verifica;
            this.checkCod322.Enabled = this._verifica;
            this.checkCod323.Enabled = this._verifica;
            #endregion

            #region vehiculo
            this.checkValidaVehiculo.Enabled = this._verifica;
            this.chkValidaInmueble.Enabled = this._verifica;
            #endregion
            #endregion

        }

        /// <summary>
        /// Verifica que los campos obligatorios esten bn
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            string result = string.Empty;
            string coincide = string.Empty;
            
            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);

            DTO_ccLineaCredito LineaCred = (DTO_ccLineaCredito)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccLineaCredito, false, this._data.SolicituDocu.LineaCreditoID.Value, true);
            DTO_ccClasificacionCredito ClaseCred = (DTO_ccClasificacionCredito)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccClasificacionCredito, false, LineaCred.ClaseCredito.Value, true);

            #region Hace las Validaciones de la tabla drSolicitudDatosPersonales

            #region Datos Deudor
            if (string.IsNullOrEmpty(this.txtPriApellidoDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblPriApellido.Text + " Deudor: ") + "\n";
                this.txtPriApellidoDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtPriNombreDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblPriNombre.Text + " Deudor: ") + "\n";
                this.txtPriNombreDeudor.Focus();
            }
            if (!this.masterTerceroDocTipoDeudor.ValidID)
            {
                result += string.Format(msgVacio, this.lblTipoDoc.Text + " Deudor: ") + "\n";
                this.masterTerceroDocTipoDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.dtFechaExpDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Deudor: ") + "\n";
                this.dtFechaExpDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtCedulaDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblNroDocumento.Text + " Deudor: ") + "\n";
                this.txtCedulaDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.dtFechaNacDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Deudor: ") + "\n";
                this.dtFechaNacDeudor.Focus();
            }
            //if (string.IsNullOrEmpty(this.txtValorIngDeud.Text) || this.txtValorIngDeud.EditValue = 0)
            if (Convert.ToInt32(this.txtValorIngDeud.EditValue) == 0)
            {
                result += string.Format(msgVacio, this.lblIngresosSoportados.Text + " Deudor: ") + "\n";
                this.dtFechaNacDeudor.Focus();
            }

            if (Convert.ToInt32(this.txtValorIngDeud.EditValue)==0)
            {
                result += string.Format(msgVacio, this.lblIngresosSoportados.Text + " Deudor: ") + "\n";
                this.dtFechaNacDeudor.Focus();
            }
            if (!this.masterActEconPrincipalDeudor.ValidID)
            {
                result += string.Format(msgVacio, this.lblActEconomica1.Text + " Deudor: ") + "\n";
                this.masterActEconPrincipalDeudor.Focus();
            }

            if (string.IsNullOrEmpty(this.txtCorreoDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblCorreo.Text + " Deudor: ") + "\n";
                this.txtCorreoDeudor.Focus();
            }
            
            if (string.IsNullOrEmpty(this.masterCiudadDeudor.Value))
            {
                result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Deudor: ") + "\n";
                this.masterCiudadDeudor.Focus();
            }

            #endregion
            if (!string.IsNullOrEmpty(this.txtCedulaCony.Text))
            {
                #region Datos Conyugue

                if (string.IsNullOrEmpty(this.txtPriApellidoCony.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Conyugue: ") + "\n";
                    this.txtPriApellidoCony.Focus();
                }
                if (string.IsNullOrEmpty(this.txtPriNombreCony.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Conyugue: ") + "\n";
                    this.txtPriNombreCony.Focus();
                }
                if (!this.masterTerceroDocTipoCony.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Conyugue: ") + "\n";
                    this.masterTerceroDocTipoCony.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaExpCony.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Conyugue: ") + "\n";
                    this.dtFechaExpCony.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCedulaCony.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Conyugue: ") + "\n";
                    this.txtCedulaCony.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCony.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Conyugue: ") + "\n";
                    this.dtFechaNacCony.Focus();
                }
                
                if (!this.masterActEconPrincipalCony.ValidID)
                {
                    result += string.Format(msgVacio, this.lblActEconomica1.Text + " Conyugue: ") + "\n";
                    this.masterActEconPrincipalCony.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCorreoCony.Text))
                {
                    result += string.Format(msgVacio, this.lblCorreo.Text + " Conyugue: ") + "\n";
                    this.txtCorreoCony.Focus();
                }

                if (string.IsNullOrEmpty(this.masterCiudadCony.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Conyugue: ") + "\n";
                    this.masterCiudadCony.Focus();
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
            {
                #region Datos Codeudor1
                if (string.IsNullOrEmpty(this.txtPriApellidoCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor1: ") + "\n";
                    this.txtPriApellidoCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtPriNombreCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor1: ") + "\n";
                    this.txtPriNombreCod1.Focus();
                }
                if (!this.masterTerceroDocTipoCod1.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor1: ") + "\n";
                    this.masterTerceroDocTipoCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaExpCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor1: ") + "\n";
                    this.dtFechaExpCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCedulaCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor1: ") + "\n";
                    this.txtCedulaCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor1: ") + "\n";
                    this.dtFechaNacCod1.Focus();
                }
                
                if (!this.masterActEconPrincipalCod1.ValidID)
                {
                    result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor1: ") + "\n";
                    this.masterActEconPrincipalCod1.Focus();
                }
                
                if (string.IsNullOrEmpty(this.txtCorreoCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor1: ") + "\n";
                    this.txtCorreoCod1.Focus();
                }

                if (string.IsNullOrEmpty(this.masterCiudadCod1.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor1: ") + "\n";
                    this.masterCiudadCod1.Focus();
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
            {
                #region Datos Codeudor2
                if (string.IsNullOrEmpty(this.txtPriApellidoCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor2: ") + "\n";
                    this.txtPriApellidoCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtPriNombreCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor2: ") + "\n";
                    this.txtPriNombreCod2.Focus();
                }
                if (!this.masterTerceroDocTipoCod2.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor2: ") + "\n";
                    this.masterTerceroDocTipoCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaExpCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor2: ") + "\n";
                    this.dtFechaExpCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCedulaCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor2: ") + "\n";
                    this.txtCedulaCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor2: ") + "\n";
                    this.dtFechaNacCod2.Focus();
                }
                
                if (!this.masterActEconPrincipalCod2.ValidID)
                {
                    result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor2: ") + "\n";
                    this.masterActEconPrincipalCod2.Focus();
                }
               

                if (string.IsNullOrEmpty(this.txtCorreoCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor2: ") + "\n";
                    this.txtCorreoCod2.Focus();
                }

                if (string.IsNullOrEmpty(this.masterCiudadCod2.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor2: ") + "\n";
                    this.masterCiudadCod2.Focus();
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
            {
                #region Datos Codeudor3
                if (string.IsNullOrEmpty(this.txtPriApellidoCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor3: ") + "\n";
                    this.txtPriApellidoCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtPriNombreCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor3: ") + "\n";
                    this.txtPriNombreCod3.Focus();
                }
                if (!this.masterTerceroDocTipoCod3.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor3: ") + "\n";
                    this.masterTerceroDocTipoCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaExpCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor3: ") + "\n";
                    this.dtFechaExpCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCedulaCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor3: ") + "\n";
                    this.txtCedulaCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor3: ") + "\n";
                    this.dtFechaNacCod3.Focus();
                }
                
                if (!this.masterActEconPrincipalCod3.ValidID)
                {
                    result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor3: ") + "\n";
                    this.masterActEconPrincipalCod3.Focus();
                }
                
                if (string.IsNullOrEmpty(this.txtCorreoCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor3: ") + "\n";
                    this.txtCorreoCod3.Focus();
                }

                if (string.IsNullOrEmpty(this.masterCiudadCod3.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor3: ") + "\n";
                    this.masterCiudadCod3.Focus();
                }
                #endregion
            }

            #endregion
            if (this.chkAddPrenda.Checked)
            {
                #region  Hace las Validaciones de la tabla drSolicitudDatosVehiculo
                //if (this.masterFasecolda.ValidID)
                //{
                    if (!this.txtMarca.Text.ToUpper().Equals(this.txtFasecoldaMarca.Text.ToUpper()))
                    {
                        coincide += "No coincide la Marca con el seleccionado de fasecolda" + "\n";
                        //this.txtFasecoldaMarca.Focus();
                    }


                    if (Math.Abs(Convert.ToInt32(this.txtCilindraje.Text) - Convert.ToInt32(this.txtFasecoldaCilindraje.Text)) >= Convert.ToInt32(this.txtCilindraje.Text) * 0.01)
                    {
                        coincide += "No coincide el Cilindraje con el seleccionado de fasecolda" + "\n";
                        //this.txtFasecoldaCilindraje.Focus();
                    }

                    if (!this.txtLinea.Text.ToUpper().Equals(this.txtFasecoldaTipo1.Text.ToUpper()))
                    {
                        coincide += "No coincide la Linea con el seleccionado de fasecolda" + "\n";
                        //this.txtFasecoldaClase.Focus();
                    }

                    if (!this.cmbServicio.Text.ToUpper().Equals(this.cmbServicioFasecolda.Text.ToUpper()))
                    {
                        coincide += " No coincide el Servicio con el seleccionado de fasecolda" + "\n";
                        //this.cmbServicioFasecolda.Focus();
                    }

                    if (!this.chkAire.Checked.Equals(this.chkFasecoldaAire.Checked))
                    {
                        coincide += "No coincide el Aire Acondicionado con el seleccionado de fasecolda" + "\n";
                        //this.chkFasecoldaAire.Focus();
                    }

                    if (!this.cmbFasecoldaTipoCaja.EditValue.ToString().Equals(this.cmbTipoCaja.EditValue.ToString()))
                    {
                        coincide += "No coincide el tipo de Caja con el seleccionado de fasecolda" + "\n";
                        //this.chkFasecoldaAire.Focus();
                    }


                    if (!this.txtFasecoldaPuertas.Text.ToUpper().Equals(this.txtPuertas.Text.ToUpper()))
                    {
                        coincide += " No coincide el numero de puertas con el seleccionado de fasecolda" + "\n";
                        //this.cmbServicioFasecolda.Focus();
                    }

                    if (Convert.ToInt32(this.txtModelo.EditValue) == 0)
                    {
                        result += string.Format(msgVacio, " No se Digito Modelo: ") + "\n";
                        this.txtModelo.Focus();
                    }
                    if (Convert.ToInt32(this.txtPrecioVenta.EditValue) == 0)
                    {
                        result += string.Format(msgVacio, " No se Digito Precio Venta: ") + "\n";
                        this.txtPrecioVenta.Focus();
                    }

                //}
                #endregion

            }
            if (this.chkAddPrenda2.Checked)
            {
                #region  Hace las Validaciones de la tabla drSolicitudDatosVehiculo
                //if (this.masterFasecolda.ValidID)
                //{
                if (!this.txtMarca2.Text.ToUpper().Equals(this.txtFasecoldaMarca2.Text.ToUpper()))
                {
                    coincide += "No coincide la Marca con el seleccionado de fasecolda 2" + "\n";
                    //this.txtFasecoldaMarca.Focus();
                }


                if (Math.Abs(Convert.ToInt32(this.txtCilindraje2.Text) - Convert.ToInt32(this.txtFasecoldaCilindraje2.Text)) >= Convert.ToInt32(this.txtCilindraje2.Text) * 0.01)
                {
                    coincide += "No coincide el Cilindraje con el seleccionado de fasecolda 2" + "\n";
                    //this.txtFasecoldaCilindraje.Focus();
                }

                if (!this.txtLinea.Text.ToUpper().Equals(this.txtFasecoldaTipo1.Text.ToUpper()))
                {
                    coincide += "No coincide la Linea con el seleccionado de fasecolda 2" + "\n";
                    //this.txtFasecoldaClase.Focus();
                }

                if (!this.cmbServicio2.Text.ToUpper().Equals(this.cmbServicioFasecolda2.Text.ToUpper()))
                {
                    coincide += " No coincide el Servicio con el seleccionado de fasecolda 2" + "\n";
                    //this.cmbServicioFasecolda.Focus();
                }

                if (!this.chkAire2.Checked.Equals(this.chkFasecoldaAire2.Checked))
                {
                    coincide += "No coincide el Aire Acondicionado con el seleccionado de fasecolda 2" + "\n";
                    //this.chkFasecoldaAire.Focus();
                }

                if (!this.cmbFasecoldaTipoCaja2.EditValue.ToString().Equals(this.cmbTipoCaja2.EditValue.ToString()))
                {
                    coincide += "No coincide el tipo de Caja con el seleccionado de fasecolda 2" + "\n";
                    //this.chkFasecoldaAire.Focus();
                }


                if (!this.txtFasecoldaPuertas2.Text.ToUpper().Equals(this.txtPuertas2.Text.ToUpper()))
                {
                    coincide += " No coincide el numero de puertas con el seleccionado de fasecolda 2" + "\n";
                    //this.cmbServicioFasecolda.Focus();
                }

                if (Convert.ToInt32(this.txtModelo2.EditValue) == 0)
                {
                    result += string.Format(msgVacio, " No se Digito Modelo 2: ") + "\n";
                    this.txtModelo2.Focus();
                }
                if (Convert.ToInt32(this.txtPrecioVenta2.EditValue) == 0)
                {
                    result += string.Format(msgVacio, " No se Digito Precio Venta 2: ") + "\n";
                    this.txtPrecioVenta2.Focus();
                }

                //}
                #endregion

            }
            //Validaciones Hipoteca
            if (this.chkAddHipoteca.Checked)
            {
                #region  Hace las Validaciones de la tabla drSolicitudDatosVehiculo Hipoteca
                if (string.IsNullOrEmpty(this.masterCiudadHipoteca.Value))
                {
                    result += string.Format(msgVacio, "Ciudad Hipoteca 1 ") + "\n";
                    this.masterCiudadHipoteca.Focus();
                }


                if (string.IsNullOrWhiteSpace(Convert.ToString(this.cmbTipoInmueble.EditValue)))
                {
                    result += string.Format(msgVacio, " No se Digito Tipo Inmueble: ") + "\n";
                    this.cmbTipoInmueble.Focus();
                }
                if (string.IsNullOrWhiteSpace(this.txtDireccion.Text))
                {
                    result += string.Format(msgVacio, " No se Digito Direccion Inmueble: ") + "\n";
                    this.txtDireccion.Focus();
                }
                if (string.IsNullOrWhiteSpace(this.txtFMI.Text))
                {
                    result += string.Format(msgVacio, " No se Digito FMI Inmueble: ") + "\n";
                    this.txtFMI.Focus();
                }

                if (string.IsNullOrWhiteSpace(this.dtFechaComercial.Text))
                {
                    result += string.Format(msgVacio, " No se Digito Fecha Inmueble: ") + "\n";
                    this.dtFechaComercial.Focus();
                }
                if (Convert.ToInt32(this.txtValorComercial.EditValue) == 0)
                {
                    result += string.Format(msgVacio, " No se Digito Valor Inmueble: ") + "\n";
                    this.txtValorComercial.Focus();
                }
                #endregion
            }
            //Validaciones Hipoteca 2
            if (this.chkAddHipoteca2.Checked)
            {
                #region  Hace las Validaciones de la tabla drSolicitudDatosVehiculo Hipoteca2
                if (string.IsNullOrEmpty(this.masterCiudadHipoteca2.Value))
                {
                    result += string.Format(msgVacio, "Ciudad Hipoteca 2") + "\n";
                    this.masterCiudadHipoteca2.Focus();
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(this.cmbTipoInmueble2.EditValue)))
                {
                    result += string.Format(msgVacio, " No se Digito Tipo Inmueble 2: ") + "\n";
                    this.cmbTipoInmueble2.Focus();
                }
                if (string.IsNullOrWhiteSpace(this.txtDireccion2.Text))
                {
                    result += string.Format(msgVacio, " No se Digito Direccion Inmueble2: ") + "\n";
                    this.txtDireccion2.Focus();
                }
                if (string.IsNullOrWhiteSpace(this.txtFMI2.Text))
                {
                    result += string.Format(msgVacio, " No se Digito FMI Inmueble 2: ") + "\n";
                    this.txtFMI2.Focus();
                }

                if (string.IsNullOrWhiteSpace(this.dtFechaComercial2.Text))
                {
                    result += string.Format(msgVacio, " No se Digito Fecha Inmueble 2: ") + "\n";
                    this.dtFechaComercial.Focus();
                }
                if (Convert.ToInt32(this.txtValorComercial2.EditValue) == 0)
                {
                    result += string.Format(msgVacio, " No se Digito Valor Inmueble 2: ") + "\n";
                    this.txtValorComercial2.Focus();
                }
                #endregion
            }

            #region Hace las Validaciones de si exige Hipoteca 
            if (ClaseCred.HipotecarioInd.Value.Value)
            {   
                #region Datos Deudor
                if (Convert.ToInt32(this.txtNroInmueblesDeudor.EditValue)<=0)
                {
                    result += string.Format(msgVacio, this.lblNroInmuebles.Text + " Deudor: ") + "\n";
                    this.txtNroInmueblesDeudor.Focus();
                }
                #endregion
                if (!string.IsNullOrEmpty(this.txtCedulaCony.Text))
                {
                    #region Datos Conyugue
                    if (Convert.ToInt32(this.txtNroInmueblesCony.EditValue) <= 0)
                    {
                        result += string.Format(msgVacio, this.lblNroInmuebles.Text + " Conyuge: ") + "\n";
                        this.txtNroInmueblesCony.Focus();
                    }

                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
                {
                    #region Datos Codeudor1
                    if (Convert.ToInt32(this.txtNroInmueblesCod1.Text) <= 0)
                    {
                        result += string.Format(msgVacio, this.lblNroInmuebles.Text + " Codeudor1: ") + "\n";
                        this.txtNroInmueblesCod1.Focus();
                    }

                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
                {
                    #region Datos Codeudor2
                    if (Convert.ToInt32(this.txtNroInmueblesCod2.Text) <= 0)
                    {
                        result += string.Format(msgVacio, this.lblNroInmuebles.Text + " Codeudor2: ") + "\n";
                        this.txtNroInmueblesCod2.Focus();
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
                {
                    #region Datos Codeudor3
                    if (Convert.ToInt32(this.txtNroInmueblesCod3.Text) <= 0)
                    {
                        result += string.Format(msgVacio, this.lblNroInmuebles.Text + " Codeudor3: ") + "\n";
                        this.txtNroInmueblesCod3.Focus();
                    }
                    #endregion
                }

            }
            #endregion

            #region Validacion en proceso verificacion
            if (this._verifica && this._aprueba)
            {
                #region deudor 

                if (this.checkDeudor1.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor1.Focus();
                }
                if (this.checkDeudor2.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblSdoApellido.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor2.Focus();
                }
                if (this.checkDeudor3.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor3.Focus();
                }
                if (this.checkDeudor4.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblSdoNombre.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor4.Focus();
                }
                if (this.checkDeudor5.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor5.Focus();
                }
                if (this.checkDeudor6.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor6.Focus();
                }
                if (this.checkDeudor7.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor7.Focus();
                }
                if (this.checkDeudor8.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblFechaNacimiento.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor8.Focus();
                }
                if (this.checkDeudor9.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblEstadoCivil.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor9.Focus();
                }

                if (this.checkDeudor10.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblActEconomica1.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor10.Focus();
                }
                if (this.checkDeudor11.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblFuente1.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor11.Focus();
                }
                if (this.checkDeudor12.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblFuente2.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor12.Focus();
                }
                if (this.checkDeudor13.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblIngresosRegistrados.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor13.Focus();
                }
                if (this.checkDeudor14.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor14.Focus();
                }

                if (this.checkDeudor15.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblCorreo.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor15.Focus();
                }
                if (this.checkDeudor16.Checked == false)
                {
                    result += string.Format(msgVacio, this.lblCiudadResidencia.Text + "Validacion Deudor: ") + "\n";
                    this.checkDeudor16.Focus();
                }
                if (Convert.ToInt32(this.txtNroInmueblesDeudor.EditValue) <= 0)
                {

                    if (this.checkDeudor17.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblNroInmuebles.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor17.Focus();
                    }
                    if (this.checkDeudor18.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblAntCompra.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor18.Focus();
                    }
                    if (this.checkDeudor19.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblAntAnotacion.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor19.Focus();
                    }
                    if (this.checkDeudor20.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblHipotecas.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor20.Focus();
                    }
                    if (this.checkDeudor21.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblRestriccion.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor21.Focus();
                    }
                    if (this.checkDeudor22.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFolioMatricula.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor22.Focus();
                    }
                    if (this.checkDeudor23.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaMatricula.Text + "Validacion Deudor: ") + "\n";
                        this.checkDeudor23.Focus();
                    }
                }
                #endregion
                if (!string.IsNullOrEmpty(this.txtCedulaCony.Text))
                {
                    #region Datos Conyugue


                    if (this.checkCony1.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriApellido.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony1.Focus();
                    }
                    if (this.checkCony2.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoApellido.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony2.Focus();
                    }
                    if (this.checkCony3.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriNombre.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony3.Focus();
                    }
                    if (this.checkCony4.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoNombre.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony4.Focus();
                    }
                    if (this.checkCony5.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblTipoDoc.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony5.Focus();
                    }
                    if (this.checkCony6.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony6.Focus();
                    }
                    if (this.checkCony7.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblNroDocumento.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony7.Focus();
                    }
                    if (this.checkCony8.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaNacimiento.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony8.Focus();
                    }
                    if (this.checkCony9.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblEstadoCivil.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony9.Focus();
                    }

                    if (this.checkCony10.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblActEconomica1.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony10.Focus();
                    }
                    if (this.checkCony11.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente1.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony11.Focus();
                    }
                    if (this.checkCony12.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente2.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony12.Focus();
                    }
                    if (this.checkCony13.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosRegistrados.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony13.Focus();
                    }
                    if (this.checkCony14.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony14.Focus();
                    }

                    if (this.checkCony15.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCorreo.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony15.Focus();
                    }
                    if (this.checkCony16.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCiudadResidencia.Text + "Validacion Conyuge: ") + "\n";
                        this.checkCony16.Focus();
                    }
                    if (Convert.ToInt32(this.txtNroInmueblesCony.EditValue) <= 0)
                    {
                        if (this.checkCony17.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblNroInmuebles.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony17.Focus();
                        }
                        if (this.checkCony18.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntCompra.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony18.Focus();
                        }
                        if (this.checkCony19.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntAnotacion.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony19.Focus();
                        }
                        if (this.checkCony20.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblHipotecas.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony20.Focus();
                        }
                        if (this.checkCony21.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblRestriccion.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony21.Focus();
                        }
                        if (this.checkCony22.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFolioMatricula.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony22.Focus();
                        }
                        if (this.checkCony23.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFechaMatricula.Text + "Validacion Conyuge: ") + "\n";
                            this.checkCony23.Focus();
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
                {
                    #region Datos Codeudor1


                    if (this.checkCod11.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriApellido.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod11.Focus();
                    }
                    if (this.checkCod12.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoApellido.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod12.Focus();
                    }
                    if (this.checkCod13.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriNombre.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod13.Focus();
                    }
                    if (this.checkCod14.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoNombre.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod14.Focus();
                    }
                    if (this.checkCod15.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblTipoDoc.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod15.Focus();
                    }
                    if (this.checkCod16.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod16.Focus();
                    }

                    if (this.checkCod17.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblNroDocumento.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod17.Focus();
                    }
                    if (this.checkCod18.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaNacimiento.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod18.Focus();
                    }
                    if (this.checkCod19.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblEstadoCivil.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod19.Focus();
                    }

                    if (this.checkCod110.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblActEconomica1.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod110.Focus();
                    }
                    if (this.checkCod111.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente1.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod111.Focus();
                    }
                    if (this.checkCod112.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente2.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod112.Focus();
                    }
                    if (this.checkCod113.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosRegistrados.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod113.Focus();
                    }
                    if (this.checkCod114.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod114.Focus();
                    }

                    if (this.checkCod115.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCorreo.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod115.Focus();
                    }
                    if (this.checkCod116.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCiudadResidencia.Text + "Validacion Codeudor1: ") + "\n";
                        this.checkCod116.Focus();
                    }
                    if (Convert.ToInt32(this.txtNroInmueblesCod1.EditValue) <= 0)
                    {
                        if (this.checkCod117.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblNroInmuebles.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod117.Focus();
                        }
                        if (this.checkCod118.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntCompra.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod118.Focus();
                        }
                        if (this.checkCod119.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntAnotacion.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod119.Focus();
                        }
                        if (this.checkCod120.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblHipotecas.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod120.Focus();
                        }
                        if (this.checkCod121.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblRestriccion.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod121.Focus();
                        }
                        if (this.checkCod122.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFolioMatricula.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod122.Focus();
                        }
                        if (this.checkCod123.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFechaMatricula.Text + "Validacion Codeudor1: ") + "\n";
                            this.checkCod123.Focus();
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
                {
                    #region Datos Codeudor2


                    if (this.checkCod21.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriApellido.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod21.Focus();
                    }
                    if (this.checkCod22.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoApellido.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod22.Focus();
                    }
                    if (this.checkCod23.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriNombre.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod23.Focus();
                    }
                    if (this.checkCod24.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoNombre.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod24.Focus();
                    }
                    if (this.checkCod25.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblTipoDoc.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod25.Focus();
                    }
                    if (this.checkCod26.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod26.Focus();
                    }
                    if (this.checkCod27.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblNroDocumento.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod27.Focus();
                    }
                    if (this.checkCod28.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaNacimiento.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod28.Focus();
                    }
                    if (this.checkCod29.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblEstadoCivil.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod29.Focus();
                    }

                    if (this.checkCod210.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblActEconomica1.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod210.Focus();
                    }
                    if (this.checkCod211.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente1.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod211.Focus();
                    }
                    if (this.checkCod212.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente2.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod212.Focus();
                    }
                    if (this.checkCod213.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosRegistrados.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod213.Focus();
                    }
                    if (this.checkCod214.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod214.Focus();
                    }

                    if (this.checkCod215.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCorreo.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod215.Focus();
                    }
                    if (this.checkCod216.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCiudadResidencia.Text + "Validacion Codeudor2: ") + "\n";
                        this.checkCod216.Focus();
                    }
                    if (Convert.ToInt32(this.txtNroInmueblesCod2.EditValue) <= 0)
                    {
                        if (this.checkCod217.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblNroInmuebles.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod217.Focus();
                        }
                        if (this.checkCod218.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntCompra.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod218.Focus();
                        }
                        if (this.checkCod219.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntAnotacion.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod219.Focus();
                        }
                        if (this.checkCod220.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblHipotecas.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod220.Focus();
                        }
                        if (this.checkCod221.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblRestriccion.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod221.Focus();
                        }
                        if (this.checkCod222.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFolioMatricula.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod222.Focus();
                        }
                        if (this.checkCod223.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFechaMatricula.Text + "Validacion Codeudor2: ") + "\n";
                            this.checkCod223.Focus();
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
                {
                    #region Datos Codeudor3


                    if (this.checkCod31.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriApellido.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod31.Focus();
                    }
                    if (this.checkCod32.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoApellido.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod32.Focus();
                    }
                    if (this.checkCod33.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblPriNombre.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod33.Focus();
                    }
                    if (this.checkCod34.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblSdoNombre.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod34.Focus();
                    }
                    if (this.checkCod35.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblTipoDoc.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod35.Focus();
                    }
                    if (this.checkCod36.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod36.Focus();
                    }
                    if (this.checkCod37.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblNroDocumento.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod37.Focus();
                    }
                    if (this.checkCod38.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFechaNacimiento.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod38.Focus();
                    }
                    if (this.checkCod39.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblEstadoCivil.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod39.Focus();
                    }

                    if (this.checkCod310.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblActEconomica1.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod310.Focus();
                    }
                    if (this.checkCod311.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente1.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod311.Focus();
                    }
                    if (this.checkCod312.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblFuente2.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod312.Focus();
                    }
                    if (this.checkCod313.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosRegistrados.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod313.Focus();
                    }
                    if (this.checkCod314.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod314.Focus();
                    }

                    if (this.checkCod315.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCorreo.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod315.Focus();
                    }
                    if (this.checkCod316.Checked == false)
                    {
                        result += string.Format(msgVacio, this.lblCiudadResidencia.Text + "Validacion Codeudor3: ") + "\n";
                        this.checkCod316.Focus();
                    }
                    if (Convert.ToInt32(this.txtNroInmueblesCod1.EditValue) <= 0)
                    {
                        if (this.checkCod317.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblNroInmuebles.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod317.Focus();
                        }
                        if (this.checkCod318.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntCompra.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod318.Focus();
                        }
                        if (this.checkCod319.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblAntAnotacion.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod319.Focus();
                        }
                        if (this.checkCod320.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblHipotecas.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod320.Focus();
                        }
                        if (this.checkCod321.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblRestriccion.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod321.Focus();
                        }
                        if (this.checkCod322.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFolioMatricula.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod322.Focus();
                        }
                        if (this.checkCod323.Checked == false)
                        {
                            result += string.Format(msgVacio, this.lblFechaMatricula.Text + "Validacion Codeudor3: ") + "\n";
                            this.checkCod323.Focus();
                        }
                    }
                    #endregion
                }
                #region vehiculo
                if (this.chkAddPrenda.Checked)
                    if (this.checkValidaVehiculo.Checked == false)
                    {
                        result += string.Format(msgVacio, "Validacion Vehiculo: ") + "\n";
                        this.checkValidaVehiculo.Focus();
                    }

                if (this.chkAddHipoteca.Checked)
                    if (this.chkValidaInmueble.Checked == false)
                    {
                        result += string.Format(msgVacio, "Validacion Inmueble: ") + "\n";
                        this.chkValidaInmueble.Focus();
                    }
                if (this.chkAddPrenda2.Checked)
                    if (this.checkValidaVehiculo2.Checked == false)
                    {
                        result += string.Format(msgVacio, "Validacion Vehiculo 2: ") + "\n";
                        this.checkValidaVehiculo2.Focus();
                    }

                if (this.chkAddHipoteca2.Checked)
                    if (this.chkValidaInmueble2.Checked == false)
                    {
                        result += string.Format(msgVacio, "Validacion Inmueble 2: ") + "\n";
                        this.chkValidaInmueble2.Focus();
                    }
                #endregion
            }


            #endregion


            

            if (string.IsNullOrEmpty(result))

                if (string.IsNullOrEmpty(coincide))
                {
                    validaVehiculo = true;
                    this.AssignValues(false);
                    this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                    return true;
                }
                else
                {
                    string msgTitle = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    //string msg = "Existen diferencias en los siguientes campos de los datos del heiculo: \n\n" + coincide;

                    MessageBox.Show("Verifique los siguientes campos: \n\n" + coincide);
                    return false;


                    //if (MessageBox.Show(msg, msgTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    validaVehiculo = true;
                    //    this.AssignValues(false);
                    //    this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                    //    return true;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
                    //    return false;
                    //}

                }
            else
            {

                MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
                return false;
            }

            #region glDocumentoControl
            this._ctrl.PeriodoDoc.Value = this.periodo;
            this._ctrl.PeriodoUltMov.Value = this.periodo;
            this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion
            if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value.Value == 0)
            {
                this._ctrl.DocumentoID.Value = this._documentID;
                this._ctrl.NumeroDoc.Value = 0;
                this._ctrl.FechaDoc.Value = DateTime.Now.Month == this.periodo.Month && DateTime.Now.Year == this.periodo.Year ? DateTime.Now : new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._ctrl.Descripcion.Value = "Solicitud Crédito " + this.txtCedulaDeudor.Text;
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.LugarGeograficoID.Value = this.masterTerceroDocTipoDeudor.Value;
                this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
                this._ctrl.PrefijoID.Value = this._bc.GetPrefijo(this._ctrl.AreaFuncionalID.Value, this._documentID);
                this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._ctrl.TasaCambioDOCU.Value = 0;
                this._ctrl.TasaCambioCONT.Value = 0;
                this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValorIngDeud.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.Iva.Value = 0;
                this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            }
            #endregion           

            return true;
        }

        /// <summary>
        /// Verifica que la lista de chequeo este completa
        /// </summary>
        /// <returns></returns>
        private string ValidateListas()
        {
            string Falta = null;
            foreach (DTO_glDocumentoChequeoLista basic in this._actividad)
            {
                if (!basic.IncluidoInd.Value.Value && basic.IncluidoDeudor.Value.Value)
                {
                    Falta += "\r\n"+"Deudor: "+ basic.Descripcion.Value.TrimEnd() ;
                }
                if (!basic.IncluidoConyugeInd.Value.Value && basic.IncluidoConyuge.Value.Value)
                {
                    Falta += "\r\n" + "Conyuge: " + basic.Descripcion.Value.TrimEnd();
                }
                if (!basic.IncluidoCodeudor1Ind.Value.Value && basic.IncluidoCodeudor1.Value.Value)
                {
                    Falta += "\r\n" + "CoDeudor1: " + basic.Descripcion.Value.TrimEnd();
                }
                if (!basic.IncluidoCodeudor2Ind.Value.Value && basic.IncluidoCodeudor2.Value.Value)
                {
                    Falta += "\r\n" + "CoDeudor2: " + basic.Descripcion.Value.TrimEnd();
                }
                if (!basic.IncluidoCodeudor3Ind.Value.Value && basic.IncluidoCodeudor3.Value.Value)
                {
                    Falta += "\r\n" + "CoDeudor3: " + basic.Descripcion.Value.TrimEnd();
                }

            }
            return Falta;
        }
        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void AssignValues(bool isGet)
        {
            if (!this._data.SolicituDocu.PrendaNuevaInd.Value.Value)
                this.tpDatosVehiculo.PageVisible = false;
            else
                this.tpDatosVehiculo.PageVisible = true;

            if (!this._data.SolicituDocu.HipotecaNuevaInd.Value.Value)
                this.tpHipoteca.PageVisible = false;
            else
                this.tpHipoteca.PageVisible = true;

            if (!this._data.SolicituDocu.PrendaNuevaInd2.Value.Value)
                this.tpDatosVehiculo2.PageVisible = false;
            else
                this.tpDatosVehiculo2.PageVisible = true;

            if (!this._data.SolicituDocu.HipotecaNuevaInd2.Value.Value)
                this.tpHipoteca2.PageVisible = false;
            else
                this.tpHipoteca2.PageVisible = true;

            try
            {
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
                DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_drSolicitudDatosPersonales codeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
                DTO_drSolicitudDatosPersonales codeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);
                DTO_drSolicitudDatosPersonales codeudor3 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 5);
                DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo;
                DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;

                if (isGet)
                {
                    #region Asigna datos a los controles
                    //Deudor (TipoPersona 1)
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteRadica.Value;                    
                    this.txtCedulaCony.Text = this._data.SolicituDocu.Codeudor1.Value;

                    this.chkAddPrenda.Checked = this._data.SolicituDocu.PrendaNuevaInd.Value.Value;
                    this.chkAddHipoteca.Checked = this._data.SolicituDocu.HipotecaNuevaInd.Value.Value;
                    this.txtCedulaDeudor.ReadOnly = true;
                    this.lblVersion.Text = "Vers: " + (this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value.ToString() : "1");
                    this.masterAsesor.Value = this._data.SolicituDocu.AsesorID.Value;
                    DTO_ccAsesor asesor = (DTO_ccAsesor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAsesor,false, this._data.SolicituDocu.AsesorID.Value, true);
                    DTO_ccConcesionario conces = (DTO_ccConcesionario)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario,false, this._data.SolicituDocu.ConcesionarioID.Value, true);

                    this.masterConcesionario.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                    this.masterConcesionario2.Value = this._data.SolicituDocu.Concesionario2.Value;
                    this.masterCiudad.Value = this._data.SolicituDocu.Ciudad.Value;
                    this.masterZona.Value = this._data.SolicituDocu.ZonaID.Value;
                    

                    this.chkDeudorDatacredito.Checked = false;
                    this.chkConyugeDatacredito.Checked = false;
                    this.chkCod1Datacredito.Checked = false;
                    this.chkCod2Datacredito.Checked = false;
                    this.chkCod3Datacredito.Checked = false;
                    if (deudor != null)
                    {
                        #region Datos Personales
                        this.txtPriApellidoDeudor.Text = deudor.ApellidoPri.Value;
                        this.txtSdoApellidoDeudor.Text = deudor.ApellidoSdo.Value;
                        this.txtPriNombreDeudor.Text = deudor.NombrePri.Value;
                        this.txtSdoNombreDeudor.Text = deudor.NombreSdo.Value;
                        this.masterTerceroDocTipoDeudor.Value = deudor.TerceroDocTipoID.Value;
                        this.dtFechaExpDeudor.DateTime = deudor.FechaExpDoc.Value.HasValue ? deudor.FechaExpDoc.Value.Value : DateTime.Now;
                        this.dtFechaNacDeudor.DateTime = deudor.FechaNacimiento.Value.HasValue ? deudor.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.dtFechaNacConfDeudor.DateTime = deudor.FechaNacimiento.Value.HasValue ? deudor.FechaNacimiento.Value.Value : DateTime.Now;                    
                        this.cmbEstadoCivilDeudor.EditValue = deudor.EstadoCivil.Value;
                        this.masterActEconPrincipalDeudor.Value = deudor.ActEconomica1.Value;
                        this.cmbFuente1Deudor.EditValue = deudor.FuenteIngresos1.Value;
                        this.cmbFuente2Deudor.EditValue = deudor.FuenteIngresos2.Value;

                        this.txtValorIngDeud.EditValue = deudor.IngresosREG.Value.HasValue ? deudor.IngresosREG.Value : 0;
                        //this.txtValorIngConfDeud.EditValue = 0;// deudor.IngresosREG.Value.HasValue ? deudor.IngresosREG.Value : 0;
                        this.txtValorIngSoporDeud.EditValue = deudor.IngresosSOP.Value.HasValue ? deudor.IngresosSOP.Value : 0;
                        this.txtCorreoDeudor.Text = deudor.Correo.Value;
                        this.masterCiudadDeudor.Value = deudor.CiudadResidencia.Value;
                        #endregion
                        #region Datos Inmueble
                        this.txtNroInmueblesDeudor.EditValue = deudor.NroInmuebles.Value;
                        this.txtAntiguedadAñosDeudor.Text = deudor.AntCompra.Value.ToString();
                        this.txtUltimaAnotacionDeudor.Text = deudor.AntUltimoMOV.Value.ToString();
                        this.txtNroHipotecasDeudor.EditValue= deudor.HipotecasNro.Value;
                        this.txtNroRestriccionesDeudor.EditValue = deudor.HipotecasNro.Value;
                        this.txtFolioMatriculaDeudor.Text = deudor.FolioMatricula.Value.ToString();
                        this.dtFechaFolioDeudor.DateTime = deudor.FechaMatricula.Value.HasValue ? deudor.FechaMatricula.Value.Value : DateTime.Now;
                        #endregion

                        #region Validaciones

                        this.checkDeudor1.Checked = deudor.IndApellidoPri.Value.Value;
                        this.checkDeudor2.Checked = deudor.IndApellidoSdo.Value.Value;
                        this.checkDeudor3.Checked = deudor.IndNombrePri.Value.Value;
                        this.checkDeudor4.Checked = deudor.IndNombreSdo.Value.Value;
                        this.checkDeudor5.Checked = deudor.IndTerceroDocTipoID.Value.Value;
                        this.checkDeudor6.Checked = deudor.IndFechaExpDoc.Value.Value;
                        this.checkDeudor7.Checked = deudor.IndTerceroID.Value.Value;
                        this.checkDeudor8.Checked = deudor.IndFechaNacimiento.Value.Value;
                        this.checkDeudor9.Checked = deudor.IndEstadoCivil.Value.Value;
                        this.checkDeudor10.Checked = deudor.IndActEconomica1.Value.Value;
                        this.checkDeudor11.Checked = deudor.IndFuenteIngresos1.Value.Value;
                        this.checkDeudor12.Checked = deudor.IndFuenteIngresos2.Value.Value;
                        this.checkDeudor13.Checked = deudor.IndIngresosREG.Value.Value;
                        this.checkDeudor14.Checked = deudor.IndIngresosSOP.Value.Value;
                        this.checkDeudor15.Checked = deudor.IndCorreo.Value.Value;
                        this.checkDeudor16.Checked = deudor.IndCiudadResidencia.Value.Value;

                        #region Datos Inmuebles
                        this.checkDeudor17.Checked = deudor.IndNroInmuebles.Value.Value;
                        this.checkDeudor18.Checked = deudor.IndAntCompra.Value.Value;
                        this.checkDeudor19.Checked = deudor.IndAntUltimoMOV.Value.Value;
                        this.checkDeudor20.Checked = deudor.IndHipotecasNro.Value.Value;
                        this.checkDeudor21.Checked = deudor.IndRestriccionesNro.Value.Value;
                        this.checkDeudor22.Checked = deudor.IndFolioMatricula.Value.Value;
                        this.checkDeudor23.Checked = deudor.IndFechaMatricula.Value.Value;
                        #endregion
                        #endregion
                    }
                    else
                    {
                        this.txtPriApellidoDeudor.Text = this._data.SolicituDocu.ApellidoPri.Value.ToString();
                        this.txtSdoApellidoDeudor.Text = this._data.SolicituDocu.ApellidoSdo.Value.ToString();
                        this.txtPriNombreDeudor.Text = this._data.SolicituDocu.NombrePri.Value.ToString();
                        this.txtSdoNombreDeudor.Text = this._data.SolicituDocu.NombreSdo.Value.ToString();
                        DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._data.SolicituDocu.ClienteRadica.Value, true);
                        if (terc != null)
                        {
                            DTO_ccCliente cli = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this._data.SolicituDocu.ClienteRadica.Value, true);
                            this.masterTerceroDocTipoDeudor.Value = terc.TerceroDocTipoID.Value;
                            this.dtFechaExpDeudor.DateTime = cli != null && cli.FechaExpDoc.Value.HasValue ? cli.FechaExpDoc.Value.Value : DateTime.Now;
                            this.dtFechaNacDeudor.DateTime = cli != null && cli.FechaNacimiento.Value.HasValue ? cli.FechaNacimiento.Value.Value : DateTime.Now;
                            this.cmbEstadoCivilDeudor.EditValue = cli != null && cli.EstadoCivil.Value.HasValue ? cli.EstadoCivil.Value : 0;
                            this.masterActEconPrincipalDeudor.Value = terc.ActEconomicaID.Value;
                            this.txtCorreoDeudor.Text = cli != null? cli.Correo.Value : string.Empty;
                            this.masterCiudadDeudor.Value = cli != null ? cli.NacimientoCiudad.Value : string.Empty;

                        }
                    }
                    //Conyuge (TipoPersona 2)
                    if (conyuge != null)
                    {

                        #region Datos Personales
                        this.txtCedulaCony.Text = conyuge.TerceroID.Value.ToString();
                        this.txtPriApellidoCony.Text = conyuge.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = conyuge.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = conyuge.NombrePri.Value;
                        this.txtSdoNombreCony.Text = conyuge.NombreSdo.Value;
                        this.masterTerceroDocTipoCony.Value = conyuge.TerceroDocTipoID.Value;
                        this.dtFechaExpCony.DateTime = conyuge.FechaExpDoc.Value.HasValue ? conyuge.FechaExpDoc.Value.Value : DateTime.Now;
                        this.dtFechaNacCony.DateTime = conyuge.FechaNacimiento.Value.HasValue ? conyuge.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.dtFechaNacConfCony.DateTime = Cony.FechaNacimiento.Value.HasValue ? Cony.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.cmbEstadoCivilCony.EditValue = conyuge.EstadoCivil.Value;
                        this.cmbEstadoCivilCony.EditValue = this.cmbEstadoCivilDeudor.EditValue.ToString();
                        this.masterActEconPrincipalCony.Value = conyuge.ActEconomica1.Value;
                        this.cmbFuente1Cony.EditValue = conyuge.FuenteIngresos1.Value;
                        this.cmbFuente2Cony.EditValue = conyuge.FuenteIngresos2.Value;

                        this.txtValorIngCony.EditValue = conyuge.IngresosREG.Value.HasValue ? conyuge.IngresosREG.Value : 0;
                        //this.txtValorIngConfCony.EditValue = 0;// Cony.IngresosREG.Value.HasValue ? Cony.IngresosREG.Value : 0;
                        this.txtValorIngSoporCony.EditValue = conyuge.IngresosSOP.Value.HasValue ? conyuge.IngresosSOP.Value : 0;
                        this.txtCorreoCony.Text = conyuge.Correo.Value;
                        this.masterCiudadCony.Value = conyuge.CiudadResidencia.Value;
                        this.linkConyuge.Visible = false;
                        #endregion
                        #region Datos Inmuebles
                        this.txtNroInmueblesCony.EditValue = conyuge.NroInmuebles.Value;
                        this.txtAntiguedadAñosCony.Text = conyuge.AntCompra.Value.ToString();
                        this.txtUltimaAnotacionCony.Text = conyuge.AntUltimoMOV.Value.ToString();
                        this.txtNroHipotecasCony.EditValue = conyuge.HipotecasNro.Value;
                        this.txtNroRestriccionesCony.EditValue = conyuge.HipotecasNro.Value;
                        this.txtFolioMatriculaCony.Text = conyuge.FolioMatricula.Value.ToString();
                        this.dtFechaFolioCony.DateTime = conyuge.FechaMatricula.Value.HasValue ? conyuge.FechaMatricula.Value.Value : DateTime.Now;
                        this.linkConyugeInmueble.Visible = false;
                        #endregion

                        #region Validaciones
                        #region Datos Personales

                        this.checkCony1.Checked = conyuge.IndApellidoPri.Value.Value;
                        this.checkCony2.Checked = conyuge.IndApellidoSdo.Value.Value;
                        this.checkCony3.Checked = conyuge.IndNombrePri.Value.Value;
                        this.checkCony4.Checked = conyuge.IndNombreSdo.Value.Value;
                        this.checkCony5.Checked = conyuge.IndTerceroDocTipoID.Value.Value;
                        this.checkCony6.Checked = conyuge.IndFechaExpDoc.Value.Value;
                        this.checkCony7.Checked = conyuge.IndTerceroID.Value.Value;
                        this.checkCony8.Checked = conyuge.IndFechaNacimiento.Value.Value;
                        this.checkCony9.Checked = conyuge.IndEstadoCivil.Value.Value;
                        this.checkCony10.Checked = conyuge.IndActEconomica1.Value.Value;
                        this.checkCony11.Checked = conyuge.IndFuenteIngresos1.Value.Value;
                        this.checkCony12.Checked = conyuge.IndFuenteIngresos2.Value.Value;
                        this.checkCony13.Checked = conyuge.IndIngresosREG.Value.Value;
                        this.checkCony14.Checked = conyuge.IndIngresosSOP.Value.Value;
                        this.checkCony15.Checked = conyuge.IndCorreo.Value.Value;
                        this.checkCony16.Checked = conyuge.IndCiudadResidencia.Value.Value;
                        #endregion
                        #region Datos Inmueble
                        this.checkCony17.Checked = conyuge.IndNroInmuebles.Value.Value;
                        this.checkCony18.Checked = conyuge.IndAntCompra.Value.Value;
                        this.checkCony19.Checked = conyuge.IndAntUltimoMOV.Value.Value;
                        this.checkCony20.Checked = conyuge.IndHipotecasNro.Value.Value;
                        this.checkCony21.Checked = conyuge.IndRestriccionesNro.Value.Value;
                        this.checkCony22.Checked = conyuge.IndFolioMatricula.Value.Value;
                        this.checkCony23.Checked = conyuge.IndFechaMatricula.Value.Value;
                        #endregion
                        #endregion
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this._data.SolicituDocu.Codeudor1.Value.ToString()))                        
                        {
                            DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._data.SolicituDocu.Codeudor1.Value, true);
                            if (terc != null)
                            {
                                this.linkConyuge.Visible = false;
                                this.linkConyugeInmueble.Visible = false;
                                this.txtPriApellidoCony.Text = terc.ApellidoPri.Value;
                                this.txtSdoApellidoCony.Text = terc.ApellidoSdo.Value;
                                this.txtPriNombreCony.Text = terc.NombrePri.Value;
                                this.txtSdoNombreCony.Text = terc.NombreSdo.Value;
                                this.masterTerceroDocTipoCony.Value = terc.TerceroDocTipoID.Value;
                                this.txtCedulaCony.Text = this._data.SolicituDocu.Codeudor1.Value.ToString();
                                this.cmbEstadoCivilCony.EditValue = this.cmbEstadoCivilDeudor.EditValue.ToString();
                                this.masterActEconPrincipalCony.Value = terc.ActEconomicaID.Value;
                                this.txtCorreoCony.Text = terc.CECorporativo.Value;
                                this.masterCiudadCony.Value = terc.LugarGeograficoID.Value;
                            }
                        }
                    }
                    //Codeudor1 (TipoPersona 3)
                    if (codeudor1 != null)
                    {

                        #region Datos Personales
                        this.txtCedulaCod1.Text = codeudor1.TerceroID.Value.ToString();
                        this.txtPriApellidoCod1.Text = codeudor1.ApellidoPri.Value;
                        this.txtSdoApellidoCod1.Text = codeudor1.ApellidoSdo.Value;
                        this.txtPriNombreCod1.Text = codeudor1.NombrePri.Value;
                        this.txtSdoNombreCod1.Text = codeudor1.NombreSdo.Value;
                        this.masterTerceroDocTipoCod1.Value = codeudor1.TerceroDocTipoID.Value;
                        this.dtFechaExpCod1.DateTime = codeudor1.FechaExpDoc.Value.HasValue ? codeudor1.FechaExpDoc.Value.Value : DateTime.Now;
                        this.dtFechaNacCod1.DateTime = codeudor1.FechaNacimiento.Value.HasValue ? codeudor1.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.dtFechaNacConfCod1.DateTime = Cod1.FechaNacimiento.Value.HasValue ? Cod1.FechaNacimiento.Value.Value : DateTime.Now;
                        this.cmbEstadoCivilCod1.EditValue = codeudor1.EstadoCivil.Value;
                        this.masterActEconPrincipalCod1.Value = codeudor1.ActEconomica1.Value;
                        this.cmbFuente1Cod1.EditValue = codeudor1.FuenteIngresos1.Value;
                        this.cmbFuente2Cod1.EditValue = codeudor1.FuenteIngresos2.Value;

                        this.txtValorIngCod1.EditValue = codeudor1.IngresosREG.Value.HasValue ? codeudor1.IngresosREG.Value : 0;
                        //this.txtValorIngConfCod1.EditValue = 0;// Cod1.IngresosREG.Value.HasValue ? Cod1.IngresosREG.Value : 0;
                        this.txtValorIngSoporCod1.EditValue = codeudor1.IngresosSOP.Value.HasValue ? codeudor1.IngresosSOP.Value : 0;
                        this.txtCorreoCod1.Text = codeudor1.Correo.Value;
                        this.masterCiudadCod1.Value = codeudor1.CiudadResidencia.Value;
                        this.linkCodeudor1.Visible = false;
                        #endregion
                        #region Datos Inmuemble
                        this.txtNroInmueblesCod1.EditValue = codeudor1.NroInmuebles.Value;
                        this.txtAntiguedadAñosCod1.Text = codeudor1.AntCompra.Value.ToString();
                        this.txtUltimaAnotacionCod1.Text = codeudor1.AntUltimoMOV.Value.ToString();
                        this.txtNroHipotecasCod1.EditValue = codeudor1.HipotecasNro.Value;
                        this.txtNroRestriccionesCod1.EditValue = codeudor1.HipotecasNro.Value;
                        this.txtFolioMatriculaCod1.Text = codeudor1.FolioMatricula.Value.ToString();
                        this.dtFechaFolioCod1.DateTime = codeudor1.FechaMatricula.Value.HasValue ? codeudor1.FechaMatricula.Value.Value : DateTime.Now;
                        this.linkCodeudor1Inmueble.Visible = false;
                        #endregion
                        #region Validaciones
                        #region Datos Personales

                        this.checkCod11.Checked = codeudor1.IndApellidoPri.Value.Value;
                        this.checkCod12.Checked = codeudor1.IndApellidoSdo.Value.Value;
                        this.checkCod13.Checked = codeudor1.IndNombrePri.Value.Value;
                        this.checkCod14.Checked = codeudor1.IndNombreSdo.Value.Value;
                        this.checkCod15.Checked = codeudor1.IndTerceroDocTipoID.Value.Value;
                        this.checkCod16.Checked = codeudor1.IndFechaExpDoc.Value.Value;
                        this.checkCod17.Checked = codeudor1.IndTerceroID.Value.Value;
                        this.checkCod18.Checked = codeudor1.IndFechaNacimiento.Value.Value;
                        this.checkCod19.Checked = codeudor1.IndEstadoCivil.Value.Value;
                        this.checkCod110.Checked = codeudor1.IndActEconomica1.Value.Value;
                        this.checkCod111.Checked = codeudor1.IndFuenteIngresos1.Value.Value;
                        this.checkCod112.Checked = codeudor1.IndFuenteIngresos2.Value.Value;
                        this.checkCod113.Checked = codeudor1.IndIngresosREG.Value.Value;
                        this.checkCod114.Checked = codeudor1.IndIngresosSOP.Value.Value;
                        this.checkCod115.Checked = codeudor1.IndCorreo.Value.Value;
                        this.checkCod116.Checked = codeudor1.IndCiudadResidencia.Value.Value;
                        #endregion
                        #region Datos Inmuebles
                        this.checkCod117.Checked = codeudor1.IndNroInmuebles.Value.Value;
                        this.checkCod118.Checked = codeudor1.IndAntCompra.Value.Value;
                        this.checkCod119.Checked = codeudor1.IndAntUltimoMOV.Value.Value;
                        this.checkCod120.Checked = codeudor1.IndHipotecasNro.Value.Value;
                        this.checkCod121.Checked = codeudor1.IndRestriccionesNro.Value.Value;
                        this.checkCod122.Checked = codeudor1.IndFolioMatricula.Value.Value;
                        this.checkCod123.Checked = codeudor1.IndFechaMatricula.Value.Value;
                        #endregion
                        #endregion

                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(this._data.SolicituDocu.Codeudor2.Value.ToString()))
                        {
                            DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._data.SolicituDocu.Codeudor2.Value, true);
                            if (terc != null)
                            {
                                this.linkCodeudor1.Visible = false;
                                this.linkCodeudor1Inmueble.Visible = false;
                                this.txtPriApellidoCod1.Text = terc.ApellidoPri.Value;
                                this.txtSdoApellidoCod1.Text = terc.ApellidoSdo.Value;
                                this.txtPriNombreCod1.Text = terc.NombrePri.Value;
                                this.txtSdoNombreCod1.Text = terc.NombreSdo.Value;
                                this.masterTerceroDocTipoCod1.Value = terc.TerceroDocTipoID.Value;

                                this.txtCedulaCod1.Text = this._data.SolicituDocu.Codeudor2.Value.ToString();

                                this.masterActEconPrincipalCod1.Value = terc.ActEconomicaID.Value;
                                this.txtCorreoCod1.Text = terc.CECorporativo.Value;
                                this.masterCiudadCod1.Value = terc.LugarGeograficoID.Value;
                            }
                        } 
                    }
                    //Codeudor2 (TipoPersona 4)
                    if (codeudor2 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod2.Text = codeudor2.TerceroID.Value.ToString();
                        this.txtPriApellidoCod2.Text = codeudor2.ApellidoPri.Value;
                        this.txtSdoApellidoCod2.Text = codeudor2.ApellidoSdo.Value;
                        this.txtPriNombreCod2.Text = codeudor2.NombrePri.Value;
                        this.txtSdoNombreCod2.Text = codeudor2.NombreSdo.Value;
                        this.masterTerceroDocTipoCod2.Value = codeudor2.TerceroDocTipoID.Value;
                        this.dtFechaExpCod2.DateTime = codeudor2.FechaExpDoc.Value.HasValue ? codeudor2.FechaExpDoc.Value.Value : DateTime.Now;
                        this.dtFechaNacCod2.DateTime = codeudor2.FechaNacimiento.Value.HasValue ? codeudor2.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.dtFechaNacConfCod2.DateTime = Cod2.FechaNacimiento.Value.HasValue ? Cod2.FechaNacimiento.Value.Value : DateTime.Now;
                        this.cmbEstadoCivilCod2.EditValue = codeudor2.EstadoCivil.Value;
                        this.masterActEconPrincipalCod2.Value = codeudor2.ActEconomica1.Value;
                        this.cmbFuente1Cod2.EditValue = codeudor2.FuenteIngresos1.Value;
                        this.cmbFuente2Cod2.EditValue = codeudor2.FuenteIngresos2.Value;
                        this.txtValorIngCod2.EditValue = codeudor2.IngresosREG.Value.HasValue ? codeudor2.IngresosREG.Value : 0;
                        //this.txtValorIngConfCod2.EditValue = 0;// Cod2.IngresosREG.Value.HasValue ? Cod2.IngresosREG.Value : 0;
                        this.txtValorIngSoporCod2.EditValue = codeudor2.IngresosSOP.Value.HasValue ? codeudor2.IngresosSOP.Value : 0;
                        this.txtCorreoCod2.Text = codeudor2.Correo.Value;
                        this.masterCiudadCod2.Value = codeudor2.CiudadResidencia.Value;
                        this.linkCodeudor2.Visible = false;
                        #endregion
                        #region Datos Inmuemble
                        this.txtNroInmueblesCod2.EditValue = codeudor2.NroInmuebles.Value;
                        this.txtAntiguedadAñosCod2.Text = codeudor2.AntCompra.Value.ToString();
                        this.txtUltimaAnotacionCod2.Text = codeudor2.AntUltimoMOV.Value.ToString();
                        this.txtNroHipotecasCod2.EditValue = codeudor2.HipotecasNro.Value;
                        this.txtNroRestriccionesCod2.EditValue = codeudor2.HipotecasNro.Value;
                        this.txtFolioMatriculaCod2.Text = codeudor2.FolioMatricula.Value.ToString();
                        this.dtFechaFolioCod2.DateTime = codeudor2.FechaMatricula.Value.HasValue ? codeudor2.FechaMatricula.Value.Value : DateTime.Now;
                        this.linkCodeudor2Inmueble.Visible = false;
                        #endregion
                        #region Validaciones
                        #region Datos Personales

                        this.checkCod21.Checked = codeudor2.IndApellidoPri.Value.Value;
                        this.checkCod22.Checked = codeudor2.IndApellidoSdo.Value.Value;
                        this.checkCod23.Checked = codeudor2.IndNombrePri.Value.Value;
                        this.checkCod24.Checked = codeudor2.IndNombreSdo.Value.Value;
                        this.checkCod25.Checked = codeudor2.IndTerceroDocTipoID.Value.Value;
                        this.checkCod26.Checked = codeudor2.IndFechaExpDoc.Value.Value;
                        this.checkCod27.Checked = codeudor2.IndTerceroID.Value.Value;
                        this.checkCod28.Checked = codeudor2.IndFechaNacimiento.Value.Value;
                        this.checkCod29.Checked = codeudor2.IndEstadoCivil.Value.Value;
                        this.checkCod210.Checked = codeudor2.IndActEconomica1.Value.Value;
                        this.checkCod211.Checked = codeudor2.IndFuenteIngresos1.Value.Value;
                        this.checkCod212.Checked = codeudor2.IndFuenteIngresos2.Value.Value;
                        this.checkCod213.Checked = codeudor2.IndIngresosREG.Value.Value;
                        this.checkCod214.Checked = codeudor2.IndIngresosSOP.Value.Value;
                        this.checkCod215.Checked = codeudor2.IndCorreo.Value.Value;
                        this.checkCod216.Checked = codeudor2.IndCiudadResidencia.Value.Value;
                        #endregion
                        #region Datos Inmuebles
                        this.checkCod217.Checked = codeudor2.IndNroInmuebles.Value.Value;
                        this.checkCod218.Checked = codeudor2.IndAntCompra.Value.Value;
                        this.checkCod219.Checked = codeudor2.IndAntUltimoMOV.Value.Value;
                        this.checkCod220.Checked = codeudor2.IndHipotecasNro.Value.Value;
                        this.checkCod221.Checked = codeudor2.IndRestriccionesNro.Value.Value;
                        this.checkCod222.Checked = codeudor2.IndFolioMatricula.Value.Value;
                        this.checkCod223.Checked = codeudor2.IndFechaMatricula.Value.Value;
                        #endregion
                        #endregion
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(this._data.SolicituDocu.Codeudor4.Value.ToString()))
                        {
                            DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._data.SolicituDocu.Codeudor4.Value, true);
                            if (terc != null)
                            {
                                this.linkCodeudor2.Visible = false;
                                this.linkCodeudor2Inmueble.Visible = false;

                                this.txtPriApellidoCod2.Text = terc.ApellidoPri.Value;
                                this.txtSdoApellidoCod2.Text = terc.ApellidoSdo.Value;
                                this.txtPriNombreCod2.Text = terc.NombrePri.Value;
                                this.txtSdoNombreCod2.Text = terc.NombreSdo.Value;
                                this.masterTerceroDocTipoCod2.Value = terc.TerceroDocTipoID.Value;

                                this.txtCedulaCod2.Text = this._data.SolicituDocu.Codeudor4.Value.ToString();

                                this.masterActEconPrincipalCod2.Value = terc.ActEconomicaID.Value;
                                this.txtCorreoCod2.Text = terc.CECorporativo.Value;
                                this.masterCiudadCod2.Value = terc.LugarGeograficoID.Value;
                            }
                        }
                    }
                    //Codeudor3 (TipoPersona 5)
                    if (codeudor3 != null)
                    {
                        #region Datos Personales
                        DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, codeudor3.TerceroID.Value, true);
                        if (terc != null)
                        {
                            this.txtPriApellidoCod3.Text = terc.ApellidoPri.Value;
                            this.txtSdoApellidoCod3.Text = terc.ApellidoSdo.Value;
                            this.txtPriNombreCod3.Text = terc.NombrePri.Value;
                            this.txtSdoNombreCod3.Text = terc.NombreSdo.Value;
                            this.masterTerceroDocTipoCod3.Value = terc.TerceroDocTipoID.Value;

                            this.txtCedulaCod3.Text = codeudor3.TerceroID.Value.ToString();

                            this.masterActEconPrincipalCod3.Value = terc.ActEconomicaID.Value;
                            this.txtCorreoCod3.Text = terc.CECorporativo.Value;
                            this.masterCiudadCod3.Value = terc.LugarGeograficoID.Value;
                        }

                        this.txtCedulaCod3.Text = codeudor3.TerceroID.Value.ToString();
                        this.txtPriApellidoCod3.Text = codeudor3.ApellidoPri.Value;
                        this.txtSdoApellidoCod3.Text = codeudor3.ApellidoSdo.Value;
                        this.txtPriNombreCod3.Text = codeudor3.NombrePri.Value;
                        this.txtSdoNombreCod3.Text = codeudor3.NombreSdo.Value;
                        this.masterTerceroDocTipoCod3.Value = codeudor3.TerceroDocTipoID.Value;
                        this.dtFechaExpCod3.DateTime = codeudor3.FechaExpDoc.Value.HasValue ? codeudor3.FechaExpDoc.Value.Value : DateTime.Now;
                        this.dtFechaNacCod3.DateTime = codeudor3.FechaNacimiento.Value.HasValue ? codeudor3.FechaNacimiento.Value.Value : DateTime.Now;
                        //this.dtFechaNacConfCod3.DateTime = Cod3.FechaNacimiento.Value.HasValue ? Cod3.FechaNacimiento.Value.Value : DateTime.Now;
                        this.cmbEstadoCivilCod3.EditValue = codeudor3.EstadoCivil.Value;
                        this.masterActEconPrincipalCod3.Value = codeudor3.ActEconomica1.Value;
                        this.cmbFuente1Cod3.EditValue = codeudor3.FuenteIngresos1.Value;
                        this.cmbFuente2Cod3.EditValue = codeudor3.FuenteIngresos2.Value;
                        this.txtValorIngCod3.EditValue = codeudor3.IngresosREG.Value.HasValue ? codeudor3.IngresosREG.Value : 0;
                        //this.txtValorIngConfCod3.EditValue = 0;// Cod3.IngresosREG.Value.HasValue ? Cod3.IngresosREG.Value : 0;
                        this.txtValorIngSoporCod3.EditValue = codeudor3.IngresosSOP.Value.HasValue ? codeudor3.IngresosSOP.Value : 0;
                        this.txtCorreoCod3.Text = codeudor3.Correo.Value;
                        this.masterCiudadCod3.Value = codeudor3.CiudadResidencia.Value;
                        this.linkCodeudor3.Visible = false;
                        #endregion
                        #region Datos Inmuemble
                        this.txtNroInmueblesCod3.Text = codeudor3.NroInmuebles.Value.ToString();
                        this.txtAntiguedadAñosCod3.Text = codeudor3.AntCompra.Value.ToString();
                        this.txtUltimaAnotacionCod3.Text = codeudor3.AntUltimoMOV.Value.ToString();
                        this.txtNroHipotecasCod3.Text = codeudor3.HipotecasNro.Value.ToString();
                        this.txtNroRestriccionesCod3.Text = codeudor3.HipotecasNro.Value.ToString();
                        this.txtFolioMatriculaCod3.Text = codeudor3.FolioMatricula.Value.ToString();
                        this.dtFechaFolioCod3.DateTime = codeudor3.FechaMatricula.Value.HasValue ? codeudor3.FechaMatricula.Value.Value : DateTime.Now;
                        this.linkCodeudor3Inmueble.Visible = false;
                        #endregion
                        #region Validaciones
                        #region Datos Personales
                        
                        this.checkCod31.Checked = codeudor3.IndApellidoPri.Value.Value;
                        this.checkCod32.Checked = codeudor3.IndApellidoSdo.Value.Value;
                        this.checkCod33.Checked = codeudor3.IndNombrePri.Value.Value;
                        this.checkCod34.Checked = codeudor3.IndNombreSdo.Value.Value;
                        this.checkCod35.Checked = codeudor3.IndTerceroDocTipoID.Value.Value;
                        this.checkCod36.Checked = codeudor3.IndFechaExpDoc.Value.Value;
                        this.checkCod37.Checked = codeudor3.IndTerceroID.Value.Value;
                        this.checkCod38.Checked = codeudor3.IndFechaNacimiento.Value.Value;
                        this.checkCod39.Checked = codeudor3.IndEstadoCivil.Value.Value;
                        this.checkCod310.Checked = codeudor3.IndActEconomica1.Value.Value;
                        this.checkCod311.Checked = codeudor3.IndFuenteIngresos1.Value.Value;
                        this.checkCod312.Checked = codeudor3.IndFuenteIngresos2.Value.Value;
                        this.checkCod313.Checked = codeudor3.IndIngresosREG.Value.Value;
                        this.checkCod314.Checked = codeudor3.IndIngresosSOP.Value.Value;
                        this.checkCod315.Checked = codeudor3.IndCorreo.Value.Value;
                        this.checkCod316.Checked = codeudor3.IndCiudadResidencia.Value.Value;
                        #endregion
                        #region Datos Inmuebles
                        this.checkCod317.Checked = codeudor3.IndNroInmuebles.Value.Value;
                        this.checkCod318.Checked = codeudor3.IndAntCompra.Value.Value;
                        this.checkCod319.Checked = codeudor3.IndAntUltimoMOV.Value.Value;
                        this.checkCod320.Checked = codeudor3.IndHipotecasNro.Value.Value;
                        this.checkCod321.Checked = codeudor3.IndRestriccionesNro.Value.Value;
                        this.checkCod322.Checked = codeudor3.IndFolioMatricula.Value.Value;
                        this.checkCod323.Checked = codeudor3.IndFechaMatricula.Value.Value;

                        #endregion
                        #endregion

                    }
                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        #region datos Vehiculo 1
                        
                        this.checkValidaVehiculo.Checked=vehiculo.IndValidado.Value.Value;

                        this.masterFasecolda.Value = vehiculo.FasecoldaCod.Value;
                        this.txtMarca.Text = vehiculo.Marca.Value.ToString();
                        this.txtLinea.Text = vehiculo.Linea.Value.ToString();
                        this.txtReferencia.Text = vehiculo.Referencia.Value.ToString();
                        this.txtCilindraje.Text = vehiculo.Cilindraje.Value.ToString();
                        this.cmbTipoCaja.Text = vehiculo.Tipocaja.Value.ToString();
                        this.txtComplemento.Text = vehiculo.Complemento.Value.ToString();
                        this.chkAire.Checked = vehiculo.AireAcondicionado.Value.Value;
                        this.txtPuertas.Text = vehiculo.PuertasNro.Value.ToString();
                        this.chkTermoking.Checked = vehiculo.Termoking.Value.Value;
                        this.cmbServicio.EditValue = vehiculo.Servicio.Value;
                        this.chkCeroKM.Checked = vehiculo.CeroKmInd.Value.Value;
                        this.chkChasis.Checked = vehiculo.ChasisYComplementoIND.Value.Value;
                        this.txtModelo.Text = vehiculo.Modelo.Value.ToString();
                        this.txtFasecoldaPeso.Text = vehiculo.Peso.Value.ToString();
                        this.txtPrecioVenta.EditValue = vehiculo.PrecioVenta.Value;
                        this.txtCIRegistrada.EditValue = vehiculo.Registrada.Value;
                        if (this.masterFasecolda.ValidID)
                        {
                            DTO_ccFasecolda fase = (DTO_ccFasecolda)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, false, this.masterFasecolda.Value, true);
                            this.txtFasecoldaMarca.Text = fase.Marca.Value;
                            this.txtFasecoldaClase.Text = fase.Clase.Value;
                            this.txtFasecoldaTipo1.Text = fase.Tipo1.Value;
                            this.txtFasecoldaTipo2.Text = fase.Tipo2.Value;
                            this.txtFasecoldaTipo3.Text = fase.Tipo3.Value;
                            this.txtFasecoldaTipoCaja.Text = fase.TipoCaja.Value.ToString();
                            this.cmbFasecoldaTipoCaja.EditValue = fase.TipoCaja.Value;
                            this.txtFasecoldaCilindraje.Text = fase.Cilindraje.Value.ToString();
                            this.txtFasecoldaPeso.Text = vehiculo.Peso.Value.ToString();
                            this.txtFasecoldaPasajeros.Text = fase.Pasajeros.Value.ToString();
                            this.txtFasecoldaPuertas.Text = fase.Puertas.Value.ToString();
                            this.chkFasecoldaAire.Checked = fase.AireAcondicionadoInd.Value.Value;
                            this.txtFasecoldaCombustible.Text = "";
                            this.txtFasecoldaCarga.Text = fase.Carga.Value.ToString();
                            this.txtFasecoldaTransmision.Text = "";

                            Dictionary<string, string> keys = new Dictionary<string, string>();
                            keys.Add("FaseColdaID", this.masterFasecolda.Value);
                            keys.Add("Modelo", this.txtModelo.Text);
                            DTO_ccFasecoldaModelo valor = (DTO_ccFasecoldaModelo)this._bc.GetMasterComplexDTO(AppMasters.ccFasecoldaModelo, keys, true);
                            if (valor != null)
                                this.txtFasecoldaValor.EditValue = valor.Valor.Value;
                            else
                                this.txtFasecoldaValor.EditValue = 0;
                        }


                        #endregion
                        #region datos Vehiculo 2

                        this.checkValidaVehiculo2.Checked = vehiculo.IndValidado_2.Value.Value;

                        this.masterFasecolda2.Value = vehiculo.FasecoldaCod_2.Value;
                        this.txtMarca2.Text = vehiculo.Marca_2.Value.ToString();
                        this.txtLinea2.Text = vehiculo.Linea_2.Value.ToString();
                        this.txtReferencia2.Text = vehiculo.Referencia_2.Value.ToString();
                        this.txtCilindraje2.Text = vehiculo.Cilindraje_2.Value.ToString();
                        this.cmbTipoCaja2.Text = vehiculo.Tipocaja_2.Value.ToString();
                        this.txtComplemento2.Text = vehiculo.Complemento_2.Value.ToString();
                        this.chkAire2.Checked = vehiculo.AireAcondicionado_2.Value.Value;
                        this.txtPuertas2.Text = vehiculo.PuertasNro_2.Value.ToString();
                        this.chkTermoking2.Checked = vehiculo.Termoking_2.Value.Value;
                        this.cmbServicio2.EditValue = vehiculo.Servicio_2.Value;
                        this.chkCeroKM2.Checked = vehiculo.CeroKmInd_2.Value.Value;
                        this.chkChasis2.Checked = vehiculo.ChasisYComplementoIND_2.Value.Value;
                        this.txtModelo2.Text = vehiculo.Modelo_2.Value.ToString();
                        this.txtFasecoldaPeso2.Text = vehiculo.Peso_2.Value.ToString();
                        this.txtPrecioVenta2.EditValue = vehiculo.PrecioVenta_2.Value;
                        this.txtCIRegistrada2.EditValue = vehiculo.Registrada_2.Value;
                        if (this.masterFasecolda2.ValidID)
                        {
                            DTO_ccFasecolda fase2 = (DTO_ccFasecolda)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, false, this.masterFasecolda2.Value, true);
                            this.txtFasecoldaMarca2.Text = fase2.Marca.Value;
                            this.txtFasecoldaClase2.Text = fase2.Clase.Value;
                            this.txtFasecoldaTipo1_2.Text = fase2.Tipo1.Value;
                            this.txtFasecoldaTipo2_2.Text = fase2.Tipo2.Value;
                            this.txtFasecoldaTipo3_2.Text = fase2.Tipo3.Value;
                            this.txtFasecoldaTipoCaja2.Text = fase2.TipoCaja.Value.ToString();
                            this.cmbFasecoldaTipoCaja2.EditValue = fase2.TipoCaja.Value;
                            this.txtFasecoldaCilindraje2.Text = fase2.Cilindraje.Value.ToString();
                            this.txtFasecoldaPeso2.Text = vehiculo.Peso.Value.ToString();
                            this.txtFasecoldaPasajeros2.Text = fase2.Pasajeros.Value.ToString();
                            this.txtFasecoldaPuertas2.Text = fase2.Puertas.Value.ToString();
                            this.chkFasecoldaAire2.Checked = fase2.AireAcondicionadoInd.Value.Value;
                            this.txtFasecoldaCombustible2.Text = "";
                            this.txtFasecoldaCarga2.Text = fase2.Carga.Value.ToString();
                            this.txtFasecoldaTransmision2.Text = "";

                            Dictionary<string, string> keys = new Dictionary<string, string>();
                            keys.Add("FaseColdaID", this.masterFasecolda2.Value);
                            keys.Add("Modelo", this.txtModelo2.Text);
                            DTO_ccFasecoldaModelo valor2 = (DTO_ccFasecoldaModelo)this._bc.GetMasterComplexDTO(AppMasters.ccFasecoldaModelo, keys, true);
                            if (valor2 != null)
                                this.txtFasecoldaValor2.EditValue = valor2.Valor.Value;
                            else
                                this.txtFasecoldaValor2.EditValue = 0;
                        }


                        #endregion
                        #region datos Hipoteca
                        this.chkValidaInmueble.Checked = vehiculo.IndValidaHipoteca.Value.Value;
                        this.cmbTipoInmueble.EditValue = vehiculo.InmuebleTipo.Value;
                        this.txtDireccion.EditValue = vehiculo.Direccion.Value;

                        this.txtFMI.Text = vehiculo.Matricula.Value;
                        this.masterCiudadHipoteca.Value = vehiculo.Ciudad.Value;
                        this.dtFechaComercial.DateTime = vehiculo.FechaAvaluo.Value.HasValue ? vehiculo.FechaAvaluo.Value.Value : DateTime.Now;
                        this.txtValorComercial.EditValue = vehiculo.ValorAvaluo.Value;
                        this.chkViviendaNueva.Checked = vehiculo.ViviendaNuevaInd.Value.Value;
                        this.dtFechaPredial.DateTime = vehiculo.FechaPredial.Value.HasValue ? vehiculo.FechaPredial.Value.Value : DateTime.Now;
                        this.txtValorPredial.EditValue = vehiculo.ValorPredial.Value;
                        this.dtFechaCompraventa.DateTime = vehiculo.FechaPromesa.Value.HasValue ? vehiculo.FechaPromesa.Value.Value : DateTime.Now;
                        this.txtValorCompraventa.EditValue = vehiculo.ValorCompraventa.Value;

                        #endregion
                        #region datos Hipoteca 2
                        this.chkValidaInmueble2.Checked = vehiculo.IndValidaHipoteca_2.Value.Value;
                        this.cmbTipoInmueble2.EditValue = vehiculo.InmuebleTipo_2.Value;
                        this.txtDireccion2.EditValue = vehiculo.Direccion_2.Value;

                        this.txtFMI2.Text = vehiculo.Matricula_2.Value;
                        this.masterCiudadHipoteca2.Value = vehiculo.Ciudad_2.Value;
                        this.dtFechaComercial2.DateTime = vehiculo.FechaAvaluo_2.Value.HasValue ? vehiculo.FechaAvaluo_2.Value.Value : DateTime.Now;
                        this.txtValorComercial2.EditValue = vehiculo.ValorAvaluo_2.Value;
                        this.chkViviendaNueva2.Checked = vehiculo.ViviendaNuevaInd_2.Value.Value;
                        this.dtFechaPredial2.DateTime = vehiculo.FechaPredial_2.Value.HasValue ? vehiculo.FechaPredial_2.Value.Value : DateTime.Now;
                        this.txtValorPredial2.EditValue = vehiculo.ValorPredial_2.Value;
                        this.dtFechaCompraventa2.DateTime = vehiculo.FechaPromesa_2.Value.HasValue ? vehiculo.FechaPromesa_2.Value.Value : DateTime.Now;
                        this.txtValorCompraventa2.EditValue = vehiculo.ValorCompraventa_2.Value;

                        #endregion
                    }
                    else
                    {
                        this.txtMarca.Text = this._data.SolicituDocu.Marca.Value.ToString();
                        this.txtCilindraje.Text = this._data.SolicituDocu.Cilindraje.Value.ToString();
                        this.txtLinea.Text = this._data.SolicituDocu.Linea.Value.ToString();
                        this.cmbServicio.EditValue = this._data.SolicituDocu.Servicio.Value;
                        this.chkAire.Checked = this._data.SolicituDocu.AireAcondicionado.Value.Value;
                        this.cmbTipoCaja.Text = this._data.SolicituDocu.Tipocaja.Value.ToString();
                        this.txtPuertas.Text = this._data.SolicituDocu.PuertasNro.Value.ToString();
                        this.txtReferencia.Text = this._data.SolicituDocu.Referencia.Value.ToString();
                        this.txtComplemento.Text = this._data.SolicituDocu.Complemento.Value.ToString();
                        this.chkTermoking.Checked = this._data.SolicituDocu.Termoking.Value.Value;
                        this.chkCeroKM.Checked = this._data.SolicituDocu.CeroKmInd.Value.Value;
                        this.txtPrecioVenta.EditValue = this._data.SolicituDocu.PrecioVenta.Value;
                        this.txtModelo.Text = this._data.SolicituDocu.Modelo.Value.ToString();
                    }
                    #endregion
                }
                else
                {
                    this._data.SolicituDocu.HipotecaNuevaInd.Value= Convert.ToBoolean(this.chkAddHipoteca.Checked);
                    this._data.SolicituDocu.PrendaNuevaInd.Value = Convert.ToBoolean(this.chkAddPrenda.Checked);
                    this._data.SolicituDocu.HipotecaNuevaInd2.Value = Convert.ToBoolean(this.chkAddHipoteca2.Checked);
                    this._data.SolicituDocu.PrendaNuevaInd2.Value = Convert.ToBoolean(this.chkAddPrenda2.Checked);

                    this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value : 1;
                    this._data.SolicituDocu.AsesorID.Value = this.masterAsesor.Value;
                    this._data.SolicituDocu.ConcesionarioID.Value = this.masterConcesionario.Value;
                    this._data.SolicituDocu.Concesionario2.Value = this.masterConcesionario2.Value;
                    this._data.SolicituDocu.Ciudad.Value = this.masterCiudad.Value;
                    this._data.SolicituDocu.ZonaID.Value = this.masterZona.Value;

                    this._data.SolicituDocu.SolicitarDatacreditoDeudor = this.chkDeudorDatacredito.Checked;
                    this._data.SolicituDocu.SolicitarDatacreditoCony = this.chkConyugeDatacredito.Checked;
                    this._data.SolicituDocu.SolicitarDatacreditoCod1 = this.chkCod1Datacredito.Checked;
                    this._data.SolicituDocu.SolicitarDatacreditoCod2 = this.chkCod2Datacredito.Checked;
                    this._data.SolicituDocu.SolicitarDatacreditoCod3 = this.chkCod3Datacredito.Checked;
                    #region Llena datos de los controles para salvar
                    #region Deudor (TipoPersona 1)
                    DTO_drSolicitudDatosPersonales deudorNew = deudor != null? deudor : new DTO_drSolicitudDatosPersonales();
                    #region Datos Personales
                    deudorNew.TipoPersona.Value = 1;
                    deudorNew.TerceroID.Value = this.txtCedulaDeudor.Text;
                    deudorNew.ApellidoPri.Value = this.txtPriApellidoDeudor.Text;
                    deudorNew.ApellidoSdo.Value = this.txtSdoApellidoDeudor.Text;
                    deudorNew.NombrePri.Value = this.txtPriNombreDeudor.Text;
                    deudorNew.NombreSdo.Value = this.txtSdoNombreDeudor.Text;
                    deudorNew.TerceroDocTipoID.Value = this.masterTerceroDocTipoDeudor.Value;
                    deudorNew.FechaExpDoc.Value = this.dtFechaExpDeudor.DateTime;
                    deudorNew.FechaNacimiento.Value = this.dtFechaNacDeudor.DateTime;
                    deudorNew.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivilDeudor.EditValue);
                    deudorNew.ActEconomica1.Value = this.masterActEconPrincipalDeudor.Value;
                    deudorNew.FuenteIngresos1.Value = Convert.ToByte(this.cmbFuente1Deudor.EditValue);
                    deudorNew.FuenteIngresos2.Value = Convert.ToByte(this.cmbFuente2Deudor.EditValue);

                    deudorNew.IngresosREG.Value = Convert.ToInt32(this.txtValorIngDeud.EditValue);
                    deudorNew.IngresosSOP.Value = Convert.ToInt32(this.txtValorIngSoporDeud.EditValue);
                    deudorNew.Correo.Value = this.txtCorreoDeudor.Text;
                    deudorNew.CiudadResidencia.Value = this.masterCiudadDeudor.Value;
                    #endregion
                    #region Datos Inmueble
                    deudorNew.NroInmuebles.Value = !string.IsNullOrEmpty(this.txtNroInmueblesDeudor.Text) ? Convert.ToDecimal(this.txtNroInmueblesDeudor.EditValue) : deudorNew.NroInmuebles.Value;
                    deudorNew.AntCompra.Value = !string.IsNullOrEmpty(this.txtAntiguedadAñosDeudor.Text) ? Convert.ToByte(this.txtAntiguedadAñosDeudor.Text) : deudorNew.AntCompra.Value;
                    deudorNew.AntUltimoMOV.Value = !string.IsNullOrEmpty(this.txtUltimaAnotacionDeudor.Text) ? Convert.ToByte(this.txtUltimaAnotacionDeudor.Text) : deudorNew.AntUltimoMOV.Value;
                    deudorNew.HipotecasNro.Value = !string.IsNullOrEmpty(this.txtNroHipotecasDeudor.Text) ? Convert.ToDecimal(this.txtNroHipotecasDeudor.EditValue) : deudorNew.RestriccionesNro.Value;
                    deudorNew.RestriccionesNro.Value = !string.IsNullOrEmpty(this.txtNroRestriccionesDeudor.Text) ? Convert.ToDecimal(this.txtNroRestriccionesDeudor.EditValue) : deudorNew.RestriccionesNro.Value;
                    deudorNew.FolioMatricula.Value = this.txtFolioMatriculaDeudor.Text;
                    deudorNew.FechaMatricula.Value = !string.IsNullOrEmpty(this.dtFechaFolioDeudor.Text) ? this.dtFechaFolioDeudor.DateTime : deudorNew.FechaMatricula.Value;
                    #endregion
                    #region validacion
                    #region Datos Personales
                    deudorNew.IndTerceroID.Value = Convert.ToBoolean(this.checkDeudor1.Checked);
                    deudorNew.IndApellidoPri.Value = Convert.ToBoolean(this.checkDeudor2.Checked);
                    deudorNew.IndApellidoSdo.Value = Convert.ToBoolean(this.checkDeudor3.Checked);
                    deudorNew.IndNombrePri.Value = Convert.ToBoolean(this.checkDeudor4.Checked);
                    deudorNew.IndNombreSdo.Value = Convert.ToBoolean(this.checkDeudor5.Checked);
                    deudorNew.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkDeudor6.Checked);
                    deudorNew.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkDeudor7.Checked);
                    deudorNew.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkDeudor8.Checked);
                    deudorNew.IndEstadoCivil.Value = Convert.ToBoolean(this.checkDeudor9.Checked);
                    deudorNew.IndActEconomica1.Value = Convert.ToBoolean(this.checkDeudor10.Checked);
                    deudorNew.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkDeudor11.Checked);
                    deudorNew.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkDeudor12.Checked);
                    deudorNew.IndIngresosREG.Value = Convert.ToBoolean(this.checkDeudor13.Checked);
                    deudorNew.IndIngresosSOP.Value = Convert.ToBoolean(this.checkDeudor14.Checked);
                    deudorNew.IndCorreo.Value = Convert.ToBoolean(this.checkDeudor15.Checked);
                    deudorNew.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkDeudor16.Checked);
                    #endregion
                    #region Datos Inmuebles
                    deudorNew.IndNroInmuebles.Value = Convert.ToBoolean(this.checkDeudor17.Checked);
                    deudorNew.IndAntCompra.Value = Convert.ToBoolean(this.checkDeudor18.Checked);
                    deudorNew.IndAntUltimoMOV.Value = Convert.ToBoolean(this.checkDeudor19.Checked);
                    deudorNew.IndHipotecasNro.Value = Convert.ToBoolean(this.checkDeudor20.Checked);
                    deudorNew.IndRestriccionesNro.Value = Convert.ToBoolean(this.checkDeudor21.Checked);
                    deudorNew.IndFolioMatricula.Value = Convert.ToBoolean(this.checkDeudor22.Checked);
                    deudorNew.IndFechaMatricula.Value = Convert.ToBoolean(this.checkDeudor23.Checked);
                    #endregion
                    #endregion
                    deudorNew.Consecutivo.Value = deudor != null && deudor.Consecutivo.Value.HasValue ? deudor.Consecutivo.Value : null;
                    deudorNew.Version.Value = deudor != null ? deudor.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 1);
                    this._data.DatosPersonales.Add(deudorNew);                   
                    #endregion
                    #region Conyuge (TipoPersona 2)
                    DTO_drSolicitudDatosPersonales conyugeNew = conyuge!= null? conyuge : new DTO_drSolicitudDatosPersonales();
                    #region Datos Personales
                    conyugeNew.TipoPersona.Value = 2;
                    conyugeNew.TerceroID.Value = this.txtCedulaCony.Text;
                    conyugeNew.ApellidoPri.Value = this.txtPriApellidoCony.Text;
                    conyugeNew.ApellidoSdo.Value = this.txtSdoApellidoCony.Text;
                    conyugeNew.NombrePri.Value = this.txtPriNombreCony.Text;
                    conyugeNew.NombreSdo.Value = this.txtSdoNombreCony.Text;
                    conyugeNew.TerceroDocTipoID.Value = this.masterTerceroDocTipoCony.Value;
                    conyugeNew.FechaExpDoc.Value = this.dtFechaExpCony.DateTime;
                    conyugeNew.FechaNacimiento.Value = this.dtFechaNacCony.DateTime;
                    //conyugeNew..Value = this.dtFechaNacConfCony.DateTime ;
                    conyugeNew.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivilCony.EditValue);
                    conyugeNew.ActEconomica1.Value = this.masterActEconPrincipalCony.Value;
                    conyugeNew.FuenteIngresos1.Value = Convert.ToByte(this.cmbFuente1Cony.EditValue);
                    conyugeNew.FuenteIngresos2.Value = Convert.ToByte(this.cmbFuente2Cony.EditValue);

                    conyugeNew.IngresosREG.Value = Convert.ToInt32(this.txtValorIngCony.EditValue);
                    //conyugeNew.IngresosREG.Value =  this.txtValorIngConfDeud.EditValue 
                    conyugeNew.IngresosSOP.Value = Convert.ToInt32(this.txtValorIngSoporCony.EditValue);
                    conyugeNew.Correo.Value = this.txtCorreoCony.Text;
                    conyugeNew.CiudadResidencia.Value = this.masterCiudadDeudor.Value;
                    #endregion
                    #region Datos Inmueble
                    conyugeNew.NroInmuebles.Value = !string.IsNullOrEmpty(this.txtNroInmueblesCony.Text) ? Convert.ToDecimal(this.txtNroInmueblesCony.EditValue) : conyugeNew.NroInmuebles.Value;
                    conyugeNew.AntCompra.Value = !string.IsNullOrEmpty(this.txtAntiguedadAñosCony.Text) ? Convert.ToByte(this.txtAntiguedadAñosCony.Text) : conyugeNew.AntCompra.Value;
                    conyugeNew.AntUltimoMOV.Value = !string.IsNullOrEmpty(this.txtUltimaAnotacionCony.Text) ? Convert.ToByte(this.txtUltimaAnotacionCony.Text) : conyugeNew.AntUltimoMOV.Value;
                    conyugeNew.HipotecasNro.Value = !string.IsNullOrEmpty(this.txtNroHipotecasCony.Text) ? Convert.ToDecimal(this.txtNroHipotecasCony.EditValue) : conyugeNew.HipotecasNro.Value;
                    conyugeNew.RestriccionesNro.Value = !string.IsNullOrEmpty(this.txtNroRestriccionesCony.Text) ? Convert.ToDecimal(this.txtNroRestriccionesCony.EditValue) : conyugeNew.RestriccionesNro.Value;
                    conyugeNew.FolioMatricula.Value = this.txtFolioMatriculaCony.Text;
                    conyugeNew.FechaMatricula.Value = !string.IsNullOrEmpty(this.dtFechaFolioCony.Text) ? this.dtFechaFolioCony.DateTime : conyugeNew.FechaMatricula.Value;
                    #endregion
                    #region validacion
                    #region Datos Personales
                    conyugeNew.IndTerceroID.Value = Convert.ToBoolean(this.checkCony1.Checked);
                    conyugeNew.IndApellidoPri.Value = Convert.ToBoolean(this.checkCony2.Checked);
                    conyugeNew.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCony3.Checked);
                    conyugeNew.IndNombrePri.Value = Convert.ToBoolean(this.checkCony4.Checked);
                    conyugeNew.IndNombreSdo.Value = Convert.ToBoolean(this.checkCony5.Checked);
                    conyugeNew.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCony6.Checked);
                    conyugeNew.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCony7.Checked);
                    conyugeNew.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCony8.Checked);
                    conyugeNew.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCony9.Checked);
                    conyugeNew.IndActEconomica1.Value = Convert.ToBoolean(this.checkCony10.Checked);
                    conyugeNew.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCony11.Checked);
                    conyugeNew.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCony12.Checked);
                    conyugeNew.IndIngresosREG.Value = Convert.ToBoolean(this.checkCony13.Checked);
                    conyugeNew.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCony14.Checked);
                    conyugeNew.IndCorreo.Value = Convert.ToBoolean(this.checkCony15.Checked);
                    conyugeNew.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCony16.Checked);
                    #endregion
                    #region Datos Inmuebles
                    conyugeNew.IndNroInmuebles.Value = Convert.ToBoolean(this.checkCony17.Checked);
                    conyugeNew.IndAntCompra.Value = Convert.ToBoolean(this.checkCony18.Checked);
                    conyugeNew.IndAntUltimoMOV.Value = Convert.ToBoolean(this.checkCony19.Checked);
                    conyugeNew.IndHipotecasNro.Value = Convert.ToBoolean(this.checkCony20.Checked);
                    conyugeNew.IndRestriccionesNro.Value = Convert.ToBoolean(this.checkCony21.Checked);
                    conyugeNew.IndFolioMatricula.Value = Convert.ToBoolean(this.checkCony22.Checked);
                    conyugeNew.IndFechaMatricula.Value = Convert.ToBoolean(this.checkCony23.Checked);
                    #endregion

                    #endregion
                    conyugeNew.Consecutivo.Value = conyuge != null && conyuge.Consecutivo.Value.HasValue ? conyuge.Consecutivo.Value : null;
                    conyugeNew.Version.Value = conyuge != null ? conyuge.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 2);
                    this._data.DatosPersonales.Add(conyugeNew);

                    #endregion
                    #region Codeudor1 (TipoPersona 3)
                    DTO_drSolicitudDatosPersonales cod1New = codeudor1 != null ? codeudor1 : new DTO_drSolicitudDatosPersonales();
                    #region Datos Personales
                    cod1New.TipoPersona.Value = 3;
                    cod1New.TerceroID.Value = this.txtCedulaCod1.Text;
                    cod1New.ApellidoPri.Value = this.txtPriApellidoCod1.Text;
                    cod1New.ApellidoSdo.Value = this.txtSdoApellidoCod1.Text;
                    cod1New.NombrePri.Value = this.txtPriNombreCod1.Text;
                    cod1New.NombreSdo.Value = this.txtSdoNombreCod1.Text;
                    cod1New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod1.Value;
                    cod1New.FechaExpDoc.Value = this.dtFechaExpCod1.DateTime;
                    cod1New.FechaNacimiento.Value = this.dtFechaNacCod1.DateTime;
                    //Cod1New..Value = this.dtFechaNacConfCod1.DateTime ;
                    cod1New.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivilCod1.EditValue);
                    cod1New.ActEconomica1.Value = this.masterActEconPrincipalCod1.Value;
                    cod1New.FuenteIngresos1.Value = Convert.ToByte(this.cmbFuente1Cod1.EditValue);
                    cod1New.FuenteIngresos2.Value = Convert.ToByte(this.cmbFuente2Cod1.EditValue);

                    cod1New.IngresosREG.Value = Convert.ToInt32(this.txtValorIngCod1.EditValue);
                    //Cod1New.IngresosREG.Value =  this.txtValorIngConfDeud.EditValue 
                    cod1New.IngresosSOP.Value = Convert.ToInt32(this.txtValorIngSoporCod1.EditValue);
                    cod1New.Correo.Value = this.txtCorreoCod1.Text;
                    cod1New.CiudadResidencia.Value = this.masterCiudadDeudor.Value;
                    #endregion
                    #region Datos Inmueble
                    cod1New.NroInmuebles.Value = !string.IsNullOrEmpty(this.txtNroInmueblesCod1.Text) ? Convert.ToDecimal(this.txtNroInmueblesCod1.EditValue) : cod1New.NroInmuebles.Value;
                    cod1New.AntCompra.Value = !string.IsNullOrEmpty(this.txtAntiguedadAñosCod1.Text) ? Convert.ToByte(this.txtAntiguedadAñosCod1.Text) : cod1New.AntCompra.Value;
                    cod1New.AntUltimoMOV.Value = !string.IsNullOrEmpty(this.txtUltimaAnotacionCod1.Text) ? Convert.ToByte(this.txtUltimaAnotacionCod1.Text) : cod1New.AntUltimoMOV.Value;
                    cod1New.HipotecasNro.Value = !string.IsNullOrEmpty(this.txtNroHipotecasCod1.Text) ? Convert.ToDecimal(this.txtNroHipotecasCod1.EditValue) : cod1New.HipotecasNro.Value;
                    cod1New.RestriccionesNro.Value = !string.IsNullOrEmpty(this.txtNroRestriccionesCod1.Text) ? Convert.ToDecimal(this.txtNroRestriccionesCod1.EditValue) : cod1New.RestriccionesNro.Value;
                    cod1New.FolioMatricula.Value = this.txtFolioMatriculaCod1.Text;
                    cod1New.FechaMatricula.Value = !string.IsNullOrEmpty(this.dtFechaFolioCod1.Text) ? this.dtFechaFolioCod1.DateTime : cod1New.FechaMatricula.Value;
                    #endregion
                    #region validacion
                    #region Datos Personales
                    cod1New.IndTerceroID.Value = Convert.ToBoolean(this.checkCod11.Checked);
                    cod1New.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod12.Checked);
                    cod1New.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod13.Checked);
                    cod1New.IndNombrePri.Value = Convert.ToBoolean(this.checkCod14.Checked);
                    cod1New.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod15.Checked);
                    cod1New.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod16.Checked);
                    cod1New.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod17.Checked);
                    cod1New.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod18.Checked);
                    cod1New.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod19.Checked);
                    cod1New.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod110.Checked);
                    cod1New.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod111.Checked);
                    cod1New.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod112.Checked);
                    cod1New.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod113.Checked);
                    cod1New.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod114.Checked);
                    cod1New.IndCorreo.Value = Convert.ToBoolean(this.checkCod115.Checked);
                    cod1New.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod116.Checked);
                    #endregion
                    #region Datos Inmuebles
                    cod1New.IndNroInmuebles.Value = Convert.ToBoolean(this.checkCod117.Checked);
                    cod1New.IndAntCompra.Value = Convert.ToBoolean(this.checkCod118.Checked);
                    cod1New.IndAntUltimoMOV.Value = Convert.ToBoolean(this.checkCod119.Checked);
                    cod1New.IndHipotecasNro.Value = Convert.ToBoolean(this.checkCod120.Checked);
                    cod1New.IndRestriccionesNro.Value = Convert.ToBoolean(this.checkCod121.Checked);
                    cod1New.IndFolioMatricula.Value = Convert.ToBoolean(this.checkCod122.Checked);
                    cod1New.IndFechaMatricula.Value = Convert.ToBoolean(this.checkCod123.Checked);
                    #endregion
                    #endregion
                    cod1New.Consecutivo.Value = codeudor1 != null && codeudor1.Consecutivo.Value.HasValue ? codeudor1.Consecutivo.Value : null;
                    cod1New.Version.Value = codeudor1 != null ? codeudor1.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 3);
                    this._data.DatosPersonales.Add(cod1New);
                    #endregion
                    #region Codeudor2 (TipoPersona 4)
                    DTO_drSolicitudDatosPersonales cod2New = codeudor2 != null ? codeudor2 : new DTO_drSolicitudDatosPersonales();
                    #region Datos Personales
                    cod2New.TipoPersona.Value = 4;
                    cod2New.TerceroID.Value = this.txtCedulaCod2.Text;
                    cod2New.ApellidoPri.Value = this.txtPriApellidoCod2.Text;
                    cod2New.ApellidoSdo.Value = this.txtSdoApellidoCod2.Text;
                    cod2New.NombrePri.Value = this.txtPriNombreCod2.Text;
                    cod2New.NombreSdo.Value = this.txtSdoNombreCod2.Text;
                    cod2New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod2.Value;
                    cod2New.FechaExpDoc.Value = this.dtFechaExpCod2.DateTime;
                    cod2New.FechaNacimiento.Value = this.dtFechaNacCod2.DateTime;
                    //cod2New..Value = this.dtFechaNacConfCod2.DateTime ;
                    cod2New.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivilCod2.EditValue);
                    cod2New.ActEconomica1.Value = this.masterActEconPrincipalCod2.Value;
                    cod2New.FuenteIngresos1.Value = Convert.ToByte(this.cmbFuente1Cod2.EditValue);
                    cod2New.FuenteIngresos2.Value = Convert.ToByte(this.cmbFuente2Cod2.EditValue);

                    cod2New.IngresosREG.Value = Convert.ToInt32(this.txtValorIngCod2.EditValue);
                    //cod2New.IngresosREG.Value =  this.txtValorIngConfDeud.EditValue 
                    cod2New.IngresosSOP.Value = Convert.ToInt32(this.txtValorIngSoporCod2.EditValue);
                    cod2New.Correo.Value = this.txtCorreoCod2.Text;
                    cod2New.CiudadResidencia.Value = this.masterCiudadDeudor.Value;
                    #endregion
                    #region Datos Inmueble
                    cod2New.NroInmuebles.Value = !string.IsNullOrEmpty(this.txtNroInmueblesCod2.Text) ? Convert.ToDecimal(this.txtNroInmueblesCod2.EditValue) : cod2New.NroInmuebles.Value;
                    cod2New.AntCompra.Value = !string.IsNullOrEmpty(this.txtAntiguedadAñosCod2.Text) ? Convert.ToByte(this.txtAntiguedadAñosCod2.Text) : cod2New.AntCompra.Value;
                    cod2New.AntUltimoMOV.Value = !string.IsNullOrEmpty(this.txtUltimaAnotacionCod2.Text) ? Convert.ToByte(this.txtUltimaAnotacionCod2.Text) : cod2New.AntUltimoMOV.Value;
                    cod2New.HipotecasNro.Value = !string.IsNullOrEmpty(this.txtNroHipotecasCod2.Text) ? Convert.ToDecimal(this.txtNroHipotecasCod2.EditValue) : cod2New.HipotecasNro.Value;
                    cod2New.RestriccionesNro.Value = !string.IsNullOrEmpty(this.txtNroRestriccionesCod2.Text) ? Convert.ToDecimal(this.txtNroRestriccionesCod2.EditValue) : cod2New.RestriccionesNro.Value;
                    cod2New.FolioMatricula.Value = this.txtFolioMatriculaCod2.Text;
                    cod2New.FechaMatricula.Value = !string.IsNullOrEmpty(this.dtFechaFolioCod2.Text) ? this.dtFechaFolioCod2.DateTime : cod2New.FechaMatricula.Value;
                    #endregion
                    #region validacion
                    #region Datos Personales
                    cod2New.IndTerceroID.Value = Convert.ToBoolean(this.checkCod21.Checked);
                    cod2New.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod22.Checked);
                    cod2New.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod23.Checked);
                    cod2New.IndNombrePri.Value = Convert.ToBoolean(this.checkCod24.Checked);
                    cod2New.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod25.Checked);
                    cod2New.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod26.Checked);
                    cod2New.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod27.Checked);
                    cod2New.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod28.Checked);
                    cod2New.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod29.Checked);
                    cod2New.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod210.Checked);
                    cod2New.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod211.Checked);
                    cod2New.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod212.Checked);
                    cod2New.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod213.Checked);
                    cod2New.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod214.Checked);
                    cod2New.IndCorreo.Value = Convert.ToBoolean(this.checkCod215.Checked);
                    cod2New.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod216.Checked);
                    #endregion
                    #region Datos Inmuebles
                    cod2New.IndNroInmuebles.Value = Convert.ToBoolean(this.checkCod217.Checked);
                    cod2New.IndAntCompra.Value = Convert.ToBoolean(this.checkCod218.Checked);
                    cod2New.IndAntUltimoMOV.Value = Convert.ToBoolean(this.checkCod219.Checked);
                    cod2New.IndHipotecasNro.Value = Convert.ToBoolean(this.checkCod220.Checked);
                    cod2New.IndRestriccionesNro.Value = Convert.ToBoolean(this.checkCod221.Checked);
                    cod2New.IndFolioMatricula.Value = Convert.ToBoolean(this.checkCod222.Checked);
                    cod2New.IndFechaMatricula.Value = Convert.ToBoolean(this.checkCod223.Checked);
                    #endregion
                    #endregion
                    cod2New.Consecutivo.Value = codeudor2 != null && codeudor2.Consecutivo.Value.HasValue ? codeudor2.Consecutivo.Value : null;
                    cod2New.Version.Value = codeudor2 != null ? codeudor2.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 4);
                    this._data.DatosPersonales.Add(cod2New);
                    #endregion
                    #region Codeudor3 (TipoPersona 5)
                    DTO_drSolicitudDatosPersonales cod3New = codeudor3 != null ? codeudor3 : new DTO_drSolicitudDatosPersonales();
                    #region Datos Personales
                    cod3New.TipoPersona.Value = 5;
                    cod3New.TerceroID.Value = this.txtCedulaCod3.Text;
                    cod3New.ApellidoPri.Value = this.txtPriApellidoCod3.Text;
                    cod3New.ApellidoSdo.Value = this.txtSdoApellidoCod3.Text;
                    cod3New.NombrePri.Value = this.txtPriNombreCod3.Text;
                    cod3New.NombreSdo.Value = this.txtSdoNombreCod3.Text;
                    cod3New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod3.Value;
                    cod3New.FechaExpDoc.Value = this.dtFechaExpCod3.DateTime;
                    cod3New.FechaNacimiento.Value = this.dtFechaNacCod3.DateTime;
                    //cod3New..Value = this.dtFechaNacConfCod3.DateTime ;
                    cod3New.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivilCod3.EditValue);
                    cod3New.ActEconomica1.Value = this.masterActEconPrincipalCod3.Value;
                    cod3New.FuenteIngresos1.Value = Convert.ToByte(this.cmbFuente1Cod3.EditValue);
                    cod3New.FuenteIngresos2.Value = Convert.ToByte(this.cmbFuente2Cod3.EditValue);
                    cod3New.IngresosREG.Value = Convert.ToInt32(this.txtValorIngCod3.EditValue);
                    //cod3New.IngresosREG.Value =  this.txtValorIngConfDeud.EditValue 
                    cod3New.IngresosSOP.Value = Convert.ToInt32(this.txtValorIngSoporCod3.EditValue);
                    cod3New.Correo.Value = this.txtCorreoCod3.Text;
                    cod3New.CiudadResidencia.Value = this.masterCiudadDeudor.Value;
                    #endregion
                    #region Datos Inmueble
                    cod3New.NroInmuebles.Value = !string.IsNullOrEmpty(this.txtNroInmueblesCod3.Text) ? Convert.ToDecimal(this.txtNroInmueblesCod3.Text) : cod3New.NroInmuebles.Value;
                    cod3New.AntCompra.Value = !string.IsNullOrEmpty(this.txtAntiguedadAñosCod3.Text) ? Convert.ToByte(this.txtAntiguedadAñosCod3.Text) : cod3New.AntCompra.Value;
                    cod3New.AntUltimoMOV.Value = !string.IsNullOrEmpty(this.txtUltimaAnotacionCod3.Text) ? Convert.ToByte(this.txtUltimaAnotacionCod3.Text) : cod3New.AntUltimoMOV.Value;
                    cod3New.HipotecasNro.Value = !string.IsNullOrEmpty(this.txtNroHipotecasCod3.Text) ? Convert.ToByte(this.txtNroHipotecasCod3.Text) : cod3New.HipotecasNro.Value;
                    cod3New.RestriccionesNro.Value = !string.IsNullOrEmpty(this.txtNroRestriccionesCod3.Text) ? Convert.ToByte(this.txtNroRestriccionesCod3.Text) : cod3New.RestriccionesNro.Value;
                    cod3New.FolioMatricula.Value = this.txtFolioMatriculaCod3.Text;
                    cod3New.FechaMatricula.Value = !string.IsNullOrEmpty(this.dtFechaFolioCod3.Text) ? this.dtFechaFolioCod3.DateTime : cod3New.FechaMatricula.Value;
                    #endregion
                    #region validacion
                    #region Datos Personales
                    cod3New.IndTerceroID.Value = Convert.ToBoolean(this.checkCod31.Checked);
                    cod3New.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod32.Checked);
                    cod3New.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod33.Checked);
                    cod3New.IndNombrePri.Value = Convert.ToBoolean(this.checkCod34.Checked);
                    cod3New.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod35.Checked);
                    cod3New.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod36.Checked);
                    cod3New.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod37.Checked);
                    cod3New.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod38.Checked);
                    cod3New.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod39.Checked);
                    cod3New.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod310.Checked);
                    cod3New.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod311.Checked);
                    cod3New.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod312.Checked);
                    cod3New.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod313.Checked);
                    cod3New.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod314.Checked);
                    cod3New.IndCorreo.Value = Convert.ToBoolean(this.checkCod315.Checked);
                    cod3New.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod316.Checked);
                    #endregion
                    #region Datos Inmuebles
                    cod3New.IndNroInmuebles.Value = Convert.ToBoolean(this.checkCod317.Checked);
                    cod3New.IndAntCompra.Value = Convert.ToBoolean(this.checkCod318.Checked);
                    cod3New.IndAntUltimoMOV.Value = Convert.ToBoolean(this.checkCod319.Checked);
                    cod3New.IndHipotecasNro.Value = Convert.ToBoolean(this.checkCod320.Checked);
                    cod3New.IndRestriccionesNro.Value = Convert.ToBoolean(this.checkCod321.Checked);
                    cod3New.IndFolioMatricula.Value = Convert.ToBoolean(this.checkCod322.Checked);
                    cod3New.IndFechaMatricula.Value = Convert.ToBoolean(this.checkCod323.Checked);
                    #endregion
                    #endregion
                    cod3New.Consecutivo.Value = codeudor3 != null && codeudor3.Consecutivo.Value.HasValue ? codeudor3.Consecutivo.Value : null;
                    cod3New.Version.Value = codeudor3 != null ? codeudor3.Version.Value : this._data.SolicituDocu.VersionNro.Value;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 5);
                    this._data.DatosPersonales.Add(cod3New);
                    #endregion
                    #region Datos Vehiculo

                    DTO_drSolicitudDatosVehiculo vehiculoNew = vehiculo != null ? vehiculo : new DTO_drSolicitudDatosVehiculo();
                    vehiculoNew.IndValidado.Value = Convert.ToBoolean(this.checkValidaVehiculo.Checked);

                    //if (validaVehiculo == true)
                    //{
                    //    vehiculoNew.Marca.Value = this.txtFasecoldaMarca.Text;
                    //    vehiculoNew.Cilindraje.Value = !string.IsNullOrEmpty(this.txtFasecoldaCilindraje.Text) ? Convert.ToInt32(this.txtFasecoldaCilindraje.Text) : vehiculoNew.Cilindraje.Value;
                    //    vehiculoNew.Linea.Value = this.txtFasecoldaTipo1.Text;
                    //    vehiculoNew.Servicio.Value = !string.IsNullOrEmpty(this.cmbServicioFasecolda.EditValue.ToString()) ? Convert.ToByte(this.cmbServicioFasecolda.EditValue) : vehiculoNew.Servicio.Value;
                    //    vehiculoNew.AireAcondicionado.Value = Convert.ToBoolean(this.chkFasecoldaAire.Checked);
                    //    vehiculoNew.Tipocaja.Value = !string.IsNullOrEmpty(this.cmbFasecoldaTipoCaja.EditValue.ToString()) ? Convert.ToByte(this.cmbFasecoldaTipoCaja.EditValue) : vehiculoNew.Tipocaja.Value;
                    //    vehiculoNew.PuertasNro.Value = !string.IsNullOrEmpty(this.txtFasecoldaPuertas.Text) ? Convert.ToByte(this.txtFasecoldaPuertas.Text) : vehiculoNew.PuertasNro.Value;
                    //    vehiculoNew.Referencia.Value = this.txtFasecoldaTipo2.Text;
                    //    vehiculoNew.Complemento.Value = this.txtFasecoldaTipo3.Text;
                    #region vehiculo1
                    vehiculoNew.Marca.Value = this.txtMarca.Text;
                    vehiculoNew.Cilindraje.Value = !string.IsNullOrEmpty(this.txtCilindraje.Text) ? Convert.ToInt32(this.txtCilindraje.Text) : vehiculoNew.Cilindraje.Value;
                    vehiculoNew.Linea.Value = this.txtLinea.Text;
                    vehiculoNew.Servicio.Value = !string.IsNullOrEmpty(this.cmbServicio.ToString()) ? Convert.ToByte(this.cmbServicio.EditValue) : vehiculoNew.Servicio.Value;
                    vehiculoNew.AireAcondicionado.Value = Convert.ToBoolean(this.chkAire.Checked);
                    vehiculoNew.Tipocaja.Value = !string.IsNullOrEmpty(this.cmbTipoCaja.ToString()) ? Convert.ToByte(this.cmbTipoCaja.EditValue) : vehiculoNew.Tipocaja.Value;
                    vehiculoNew.PuertasNro.Value = !string.IsNullOrEmpty(this.txtPuertas.Text) ? Convert.ToByte(this.txtPuertas.Text) : vehiculoNew.PuertasNro.Value;
                    vehiculoNew.Referencia.Value = this.txtReferencia.Text;
                    vehiculoNew.Complemento.Value = this.txtComplemento.Text;

                    vehiculoNew.Termoking.Value = Convert.ToBoolean(this.chkTermoking.Checked);                  
                    vehiculoNew.CeroKmInd.Value = Convert.ToBoolean(this.chkCeroKM.Checked);
                    vehiculoNew.ChasisYComplementoIND.Value = Convert.ToBoolean(this.chkChasis.Checked);
                    vehiculoNew.Modelo.Value = !string.IsNullOrEmpty(this.txtModelo.Text) ? Convert.ToInt32(this.txtModelo.Text) : vehiculoNew.Modelo.Value;
                    vehiculoNew.PrecioVenta.Value = Convert.ToDecimal(this.txtPrecioVenta.EditValue);
                    vehiculoNew.Registrada.Value = Convert.ToInt32(this.txtCIRegistrada.EditValue);
                    vehiculoNew.FasecoldaCod.Value = this.masterFasecolda.Value;
                    vehiculoNew.VlrFasecolda.Value = Convert.ToDecimal(this.txtFasecoldaValor.EditValue);
                    vehiculoNew.Peso.Value = !string.IsNullOrEmpty(this.txtFasecoldaPeso.Text)? Convert.ToInt32(this.txtFasecoldaPeso.Text) : 0;
                    vehiculoNew.Version.Value = vehiculo != null && vehiculo.Version.Value.HasValue ? vehiculo.Version.Value : Convert.ToByte(this._data.SolicituDocu.VersionNro.Value);
                    vehiculoNew.Consecutivo.Value = vehiculo != null && vehiculo.Consecutivo.Value.HasValue ? vehiculo.Consecutivo.Value : null;
                    vehiculoNew.NumeroDoc.Value = vehiculo != null && vehiculo.NumeroDoc.Value.HasValue ? vehiculo.NumeroDoc.Value : Convert.ToInt32(this._data.SolicituDocu.NumeroDoc.Value);
                    #endregion vehiculo1
                    if (this.chkAddPrenda2.Checked)
                    {
                        #region vehiculo2
                        vehiculoNew.Marca_2.Value = this.txtMarca2.Text;
                        vehiculoNew.Cilindraje_2.Value = !string.IsNullOrEmpty(this.txtCilindraje2.Text) ? Convert.ToInt32(this.txtCilindraje2.Text) : vehiculoNew.Cilindraje_2.Value;
                        vehiculoNew.Linea_2.Value = this.txtLinea2.Text;
                        vehiculoNew.Servicio_2.Value = !string.IsNullOrEmpty(this.cmbServicio2.ToString()) ? Convert.ToByte(this.cmbServicio2.EditValue) : vehiculoNew.Servicio_2.Value;
                        vehiculoNew.AireAcondicionado_2.Value = Convert.ToBoolean(this.chkAire.Checked);
                        vehiculoNew.Tipocaja_2.Value = !string.IsNullOrEmpty(this.cmbTipoCaja2.ToString()) ? Convert.ToByte(this.cmbTipoCaja2.EditValue) : vehiculoNew.Tipocaja_2.Value;
                        vehiculoNew.PuertasNro_2.Value = !string.IsNullOrEmpty(this.txtPuertas2.Text) ? Convert.ToByte(this.txtPuertas2.Text) : vehiculoNew.PuertasNro_2.Value;
                        vehiculoNew.Referencia_2.Value = this.txtReferencia2.Text;
                        vehiculoNew.Complemento_2.Value = this.txtComplemento2.Text;

                        vehiculoNew.Termoking_2.Value = Convert.ToBoolean(this.chkTermoking2.Checked);
                        vehiculoNew.CeroKmInd_2.Value = Convert.ToBoolean(this.chkCeroKM2.Checked);
                        vehiculoNew.ChasisYComplementoIND_2.Value = Convert.ToBoolean(this.chkChasis2.Checked);
                        vehiculoNew.Modelo_2.Value = !string.IsNullOrEmpty(this.txtModelo2.Text) ? Convert.ToInt32(this.txtModelo2.Text) : vehiculoNew.Modelo_2.Value;
                        vehiculoNew.PrecioVenta_2.Value = Convert.ToDecimal(this.txtPrecioVenta2.EditValue);
                        vehiculoNew.Registrada_2.Value = Convert.ToInt32(this.txtCIRegistrada2.EditValue);
                        vehiculoNew.FasecoldaCod_2.Value = this.masterFasecolda2.Value;
                        vehiculoNew.VlrFasecolda_2.Value = Convert.ToDecimal(this.txtFasecoldaValor2.EditValue);
                        vehiculoNew.Peso_2.Value = !string.IsNullOrEmpty(this.txtFasecoldaPeso2.Text) ? Convert.ToInt32(this.txtFasecoldaPeso2.Text) : 0;
                        #endregion vehiculo2
                    }
                    if (this.chkAddHipoteca.Checked)
                    {
                        #region datos Hipoteca
                    vehiculoNew.IndValidaHipoteca.Value = Convert.ToBoolean(this.chkValidaInmueble.Checked);
                    vehiculoNew.InmuebleTipo.Value = !string.IsNullOrEmpty(this.cmbTipoInmueble.ToString()) ? Convert.ToByte(this.cmbTipoInmueble.EditValue) : vehiculoNew.InmuebleTipo.Value;
                    vehiculoNew.Direccion.Value = this.txtDireccion.Text;
                    vehiculoNew.Matricula.Value = this.txtFMI.Text;
                    vehiculoNew.Ciudad.Value = this.masterCiudadHipoteca.Value;
                    vehiculoNew.FechaAvaluo.Value = this.dtFechaComercial.DateTime;
                    vehiculoNew.ValorAvaluo.Value = Convert.ToDecimal(this.txtValorComercial.EditValue);
                    vehiculoNew.ViviendaNuevaInd.Value = Convert.ToBoolean(this.chkViviendaNueva.Checked);
                    if (!this.chkViviendaNueva.Checked)
                    {
                        vehiculoNew.FechaPromesa.Value = this.dtFechaCompraventa.DateTime;
                        vehiculoNew.ValorCompraventa.Value = Convert.ToDecimal(this.txtValorCompraventa.EditValue);

                    }
                    else
                    {
                        vehiculoNew.FechaPredial.Value = this.dtFechaPredial.DateTime;
                        vehiculoNew.ValorPredial.Value = Convert.ToDecimal(this.txtValorPredial.EditValue);

                    }

                    #endregion
                    }
                    if (this.chkAddHipoteca2.Checked)
                    {
                        #region datos Hipoteca 2
                        vehiculoNew.IndValidaHipoteca_2.Value = Convert.ToBoolean(this.chkValidaInmueble2.Checked);
                        vehiculoNew.InmuebleTipo_2.Value = !string.IsNullOrEmpty(this.cmbTipoInmueble2.ToString()) ? Convert.ToByte(this.cmbTipoInmueble2.EditValue) : vehiculoNew.InmuebleTipo_2.Value;
                        vehiculoNew.Direccion_2.Value = this.txtDireccion2.Text;
                        vehiculoNew.Matricula_2.Value = this.txtFMI2.Text;
                        vehiculoNew.Ciudad_2.Value = this.masterCiudadHipoteca2.Value;
                        vehiculoNew.FechaAvaluo_2.Value = this.dtFechaComercial2.DateTime;
                        vehiculoNew.ValorAvaluo_2.Value = Convert.ToDecimal(this.txtValorComercial2.EditValue);
                        vehiculoNew.ViviendaNuevaInd_2.Value = Convert.ToBoolean(this.chkViviendaNueva2.Checked);
                        if (this.chkViviendaNueva2.Checked)
                        {
                            vehiculoNew.FechaPromesa_2.Value = this.dtFechaCompraventa2.DateTime;
                            vehiculoNew.ValorCompraventa_2.Value = Convert.ToDecimal(this.txtValorCompraventa2.EditValue);

                        }
                        else
                        {
                            vehiculoNew.FechaPredial_2.Value = this.dtFechaPredial2.DateTime;
                            vehiculoNew.ValorPredial_2.Value = Convert.ToDecimal(this.txtValorPredial2.EditValue);

                        }

                        #endregion
                    }
                    this._data.DatosVehiculo = vehiculoNew;
                    #endregion
                    #region Otros datos
//                    DTO_drSolicitudDatosVehiculo vehiculoNew = vehiculo != null ? vehiculo : new DTO_drSolicitudDatosVehiculo();

                    DTO_drSolicitudDatosOtros otrosNew = otros != null ? otros: new DTO_drSolicitudDatosOtros();
                    otrosNew.Version.Value = otros != null && otros.Version.Value.HasValue ? otros.Version.Value : Convert.ToByte(this._data.SolicituDocu.VersionNro.Value);
                    otrosNew.NumeroDoc.Value = otros != null && otros.NumeroDoc.Value.HasValue ? otros.NumeroDoc.Value : Convert.ToInt32(this._data.SolicituDocu.NumeroDoc.Value);
                    otrosNew.Consecutivo.Value = otros != null && otros.Consecutivo.Value.HasValue ? otros.Consecutivo.Value : null;
                    //otrosNew.VlrSolicitado.Value = this._data.SolicituDocu.VlrSolicitado.Value; 
                    
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="libranzaNro"></param>
        private void LoadData(string cliente, int libranzaNro)
        {
            this._isLoaded = false;
            this._data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(libranzaNro);
            this._libranzaID = String.Empty;
            if (this._data != null)
            {
                #region Solicitud existente
                this._ctrl = this._data.DocCtrl;

                if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)                  
                {
                    this._libranzaID = this._data.SolicituDocu.Libranza.Value.ToString();
                    this.AssignValues(true);
                    this.EnableHeader(true);
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

        #endregion Funciones Privadas

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

                //Descripcion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, "Descriptivo");
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
                //Observacion.ColumnEdit = this.richTextTareas1;
                Observacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Observacion.OptionsColumn.AllowEdit = true;
                this.gvActividades.Columns.Add(Observacion);

                //Incluye
                //Descripcion
                GridColumn Incluye = new GridColumn();
                Incluye.FieldName = this._unboundPrefix + "Incluye";
                Incluye.Caption = "Valida";
                Incluye.UnboundType = UnboundColumnType.String;
                Incluye.VisibleIndex = 4;
                Incluye.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Valida");
                Incluye.AppearanceHeader.ForeColor = Color.Lime;
                Incluye.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Incluye.Width = 25;
                Incluye.AppearanceCell.ForeColor = Color.Lime;
                Incluye.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Incluye.Visible = true;
                Incluye.OptionsColumn.AllowEdit = false;
                Incluye.AppearanceHeader.Options.UseTextOptions = true;
                Incluye.AppearanceHeader.Options.UseFont = true;
                Incluye.AppearanceCell.Options.UseTextOptions = true;
                Incluye.AppearanceCell.Options.UseFont = true;
                Incluye.AppearanceCell.Options.UseForeColor = true;

                Incluye.AppearanceHeader.Options.UseForeColor = true;

                this.gvActividades.Columns.Add(Incluye);


                //IncluidoInd
                GridColumn IncluidoInd = new GridColumn();
                IncluidoInd.FieldName = this._unboundPrefix + "IncluidoInd";
                IncluidoInd.Caption = "√ Deudor (D)";
                IncluidoInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoInd.VisibleIndex = 5;
                IncluidoInd.Width = 25;
                IncluidoInd.Visible = true;
                IncluidoInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Deudor (D)");
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
                IncluidoConyInd.Caption = "√ Conyuge (C)";
                IncluidoConyInd.UnboundType = UnboundColumnType.Boolean;
                IncluidoConyInd.VisibleIndex = 6;
                IncluidoConyInd.Width = 25;
                IncluidoConyInd.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 2);//Conyuge;
                IncluidoConyInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Conyuge (C)");
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
                IncluidoCodeudor1Ind.Caption = "√ Cod 1 (C1)";
                IncluidoCodeudor1Ind.UnboundType = UnboundColumnType.Boolean;
                IncluidoCodeudor1Ind.VisibleIndex = 7;
                IncluidoCodeudor1Ind.Width = 25;
                IncluidoCodeudor1Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 3);//Codeudor1;
                IncluidoCodeudor1Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, "Cod 1 (C1)");
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
                IncluidoCodeudor2Ind.Caption = "√  Cod 2 (C2)";
                IncluidoCodeudor2Ind.UnboundType = UnboundColumnType.Boolean;
                IncluidoCodeudor2Ind.VisibleIndex = 8;
                IncluidoCodeudor2Ind.Width = 25;
                IncluidoCodeudor2Ind.Visible = true;
                IncluidoCodeudor2Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 4);//Codeudor2;
                IncluidoCodeudor2Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, this._documentID + " Cod 2 (C2)");
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
                IncluidoCodeudor3Ind.Caption = "√  Cod 3  (C3)";
                IncluidoCodeudor3Ind.UnboundType = UnboundColumnType.Boolean;
                IncluidoCodeudor3Ind.VisibleIndex = 9;
                IncluidoCodeudor3Ind.Width = 25;
                IncluidoCodeudor3Ind.Visible = true;
                IncluidoCodeudor3Ind.Visible = this._data != null && this._data.DatosPersonales.Exists(X => X.TipoPersona.Value == 5);//Codeudor3;
                IncluidoCodeudor3Ind.ToolTip = _bc.GetResource(LanguageTypes.Forms, " Cod 3 (C3)");
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
                if (this._actFlujoRevision != null)
                {
                    this.actChequeo = this._bc.AdministrationModel.drSolicitudDatosChequeados_GetByActividadNumDoc(this._actFlujoRevision.ID.Value, this._ctrl.NumeroDoc.Value.Value, this._data.SolicituDocu.VersionNro.Value.Value);
                    //if (this.actChequeo==null ||this.actChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujoRevision.ID.Value);
                        foreach (DTO_drActividadChequeoLista basic in this.actChequeoBase)
                        {
                            DTO_glDocumentoChequeoLista chequeo = new DTO_glDocumentoChequeoLista();
                            bool addChequeo1 = false;
                            bool addChequeo2 = false;
                            bool addChequeo3 = false;
                            bool addChequeo4 = false;
                            bool addChequeo5 = false;
                            bool addTP1 = false;
                            bool addTP2 = false;
                            bool addTP3 = false;
                            bool addTP4 = false;
                            bool addTP5 = false;

                            string Valida = "";
                            chequeo.NumeroDoc.Value = _ctrl.NumeroDoc.Value.Value;
                            chequeo.ActividadFlujoID.Value = basic.ActividadFlujoID.Value;
                            chequeo.Descripcion.Value = basic.Descripcion.Value;
                            chequeo.EmpleadoInd.Value = basic.EmpleadoInd.Value.Value;
                            chequeo.ExcluyeCodInd.Value = basic.ExcluyeCodInd.Value.Value;
                            chequeo.PrestServiciosInd.Value = basic.PrestServiciosInd.Value.Value;
                            chequeo.TransportadorInd.Value = basic.TransportadorInd.Value.Value;
                            chequeo.IndependienteInd.Value = basic.IndependienteInd.Value.Value;
                            chequeo.PensionadoInd.Value = basic.PensionadoInd.Value.Value;
                            chequeo.Consecutivo.Value = basic.consecutivo.Value.Value;
                            if (this.actChequeo == null || this.actChequeo.Count == 0)
                            {
                                chequeo.IncluidoInd.Value = false;
                                chequeo.IncluidoConyugeInd.Value = false;
                                chequeo.IncluidoCodeudor1Ind.Value = false;
                                chequeo.IncluidoCodeudor2Ind.Value = false;
                                chequeo.IncluidoCodeudor3Ind.Value = false;
                            }
                            else
                            {
                                chequeo.IncluidoInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 1 ? x.VerficadoInd.Value.Value : false));
                                chequeo.IncluidoConyugeInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 2 ? x.VerficadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor1Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 3 ? x.VerficadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor2Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 4 ? x.VerficadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor3Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 5 ? x.VerficadoInd.Value.Value : false));
                            }
                            if (basic.EmpleadoInd.Value.Value)
                            {
                                addChequeo1 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1 = true;

                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 2 ? true : false))
                                {
                                    addTP2 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 3 ? true : false))
                                {
                                    addTP3 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 4 ? true : false))
                                {
                                    addTP4 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 5 ? true : false))
                                {
                                    addTP5 = true;
                                }
                            }
                            if (basic.PrestServiciosInd.Value.Value)
                            {
                                addChequeo2 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2 && x.TipoPersona.Value == 2 ? true : false))
                                {
                                    addTP2 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2 && x.TipoPersona.Value == 3 ? true : false))
                                {
                                    addTP3 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2 && x.TipoPersona.Value == 4 ? true : false))
                                {
                                    addTP4 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 2 && x.TipoPersona.Value == 5 ? true : false))
                                {
                                    addTP5 = true;
                                }
                            }
                            if (basic.TransportadorInd.Value.Value)
                            {
                                addChequeo3 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3 && x.TipoPersona.Value == 2 ? true : false))
                                {
                                    addTP2 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3 && x.TipoPersona.Value == 3 ? true : false))
                                {
                                    addTP3 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3 && x.TipoPersona.Value == 4 ? true : false))
                                {
                                    addTP4 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 3 && x.TipoPersona.Value == 5 ? true : false))
                                {
                                    addTP5 = true;
                                }
                            }
                            if (basic.IndependienteInd.Value.Value)
                            {
                                addChequeo4 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4 && x.TipoPersona.Value == 2 ? true : false))
                                {
                                    addTP2 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4 && x.TipoPersona.Value == 3 ? true : false))
                                {
                                    addTP3 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4 && x.TipoPersona.Value == 4 ? true : false))
                                {
                                    addTP4 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 4 && x.TipoPersona.Value == 5 ? true : false))
                                {
                                    addTP5 = true;
                                }
                            }
                            if (basic.PensionadoInd.Value.Value)
                            {
                                addChequeo5 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5 && x.TipoPersona.Value == 2 ? true : false))
                                {
                                    addTP2 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5 && x.TipoPersona.Value == 3 ? true : false))
                                {
                                    addTP3 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5 && x.TipoPersona.Value == 4 ? true : false))
                                {
                                    addTP4 = true;
                                }
                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 5 && x.TipoPersona.Value == 5 ? true : false))
                                {
                                    addTP5 = true;
                                }
                            }
                            if (addTP1)
                            {
                                Valida += "-D";
                                chequeo.IncluidoDeudor.Value = true;
                            }
                            if (addTP2)
                            {
                                Valida += "-C";
                                chequeo.IncluidoConyuge.Value = true;
                            }
                            if (!basic.ExcluyeCodInd.Value.Value)
                            {
                                if (addTP3)
                                {
                                    Valida += "-C1";
                                    chequeo.IncluidoCodeudor1.Value = true;
                                }

                                if (addTP4)
                                {
                                    Valida += "-C2";
                                    chequeo.IncluidoCodeudor2.Value = true;
                                }
                                if (addTP5)
                                {
                                    Valida += "-C3";
                                    chequeo.IncluidoCodeudor3.Value = true;
                                }

                            }
                            Valida += "-";
                            chequeo.Incluye.Value = Valida;
                            if (addChequeo1 || addChequeo2 || addChequeo3 || addChequeo4 || addChequeo5)
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

        private void gvActividades_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    DTO_glDocumentoChequeoLista row = (DTO_glDocumentoChequeoLista)this.gvActividades.GetRow(e.FocusedRowHandle);
                    if (row != null && this._data != null)
                    {

                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;

                        if (row.EmpleadoInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 1))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 1))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;

                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 1))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 1))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 1))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;

                            }
                        }
                        if (row.PrestServiciosInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 2))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 2))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 2))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 2))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 2))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;

                            }
                        }
                        if (row.TransportadorInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 3))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 3))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;
                            }
                        }
                        if (row.IndependienteInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 4))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 4))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;
                            }
                        }
                        if (row.PensionadoInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 5))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 5))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 5))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 5))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 5))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;
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


        /// <summary>
        /// Agrega las columnas 
        /// </summary>
        protected virtual void AddChequeoDocumentoCols()
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
                this.gvDocumentos.Columns.Add(TerceroID);

                //Observacion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, "Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 2;
                Descripcion.Width = 130;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocumentos.Columns.Add(Descripcion);


                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, "Observación");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 3;
                Observacion.Width = 100;
                Observacion.Visible = true;
                //Observacion.ColumnEdit = this.richTextTareas1;
                Observacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Observacion.OptionsColumn.AllowEdit = true;
                this.gvDocumentos.Columns.Add(Observacion);

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
                this.gvDocumentos.Columns.Add(IncluidoInd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Ratificacion.cs", "AddColumsActividades"));
            }
        }

        /// <summary>
        /// Carga la informacion de la grilla de actividades
        /// </summary>
        private void LoadActividadesDocumento()
        {
            try
            {
                if (this._actFlujo != null)
                {
                    this.actChequeo = this._bc.AdministrationModel.drSolicitudDatosChequeados_GetByActividadNumDoc(this._actFlujo.ID.Value, this._ctrl.NumeroDoc.Value.Value, this._data.SolicituDocu.VersionNro.Value.Value);
                    //this._actividadChequeo = this._bc.AdministrationModel.glDocumentoChequeoLista_GetByNumeroDoc(this._ctrl.NumeroDoc.Value.Value);
                    //if (this._actividadChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujo.ID.Value);


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
                            this._actividadChequeoDocumento.Add(chequeo);
                        }
                    }
                }
                this.gcDocumentos.DataSource = this._actividadChequeoDocumento;
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


        #endregion
        #endregion
        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSave.Enabled = true;
                if (!this._verifica)
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar Revisión";
                else
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar Evaluacion";

                if (FormProvider.Master.LoadFormTB)
                {
                    // SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Delete);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que revisa que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Siguiente
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e"><Objeto que envia el evento/param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            
            if (!this._verifica)
            {
                #region  Digita
                if (this.tabControl.SelectedTabPageIndex == 0)
                    this.tabControl.SelectedTabPageIndex = 1;
                else if (this.tabControl.SelectedTabPageIndex == 1)
                {
                    if (this.chkAddPrenda.Checked)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                    {
                        if (this.chkAddPrenda2.Checked)
                            this.tabControl.SelectedTabPageIndex = 3;
                        else
                        {
                            if (this.chkAddHipoteca.Checked)
                                this.tabControl.SelectedTabPageIndex = 4;
                            else
                            {
                                if (this.chkAddHipoteca2.Checked)
                                    this.tabControl.SelectedTabPageIndex = 5;
                                else
                                    this.tabControl.SelectedTabPageIndex = 0;
                            }
                        }
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 2)
                {
                    if (this.chkAddPrenda2.Checked)
                        this.tabControl.SelectedTabPageIndex = 3;
                    else
                    {
                        if (this.chkAddHipoteca.Checked)
                            this.tabControl.SelectedTabPageIndex = 4;
                        else
                        {
                            if (this.chkAddHipoteca2.Checked)
                                this.tabControl.SelectedTabPageIndex = 5;
                            else
                                this.tabControl.SelectedTabPageIndex = 0;
                        }
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 3)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 4;
                    else
                    {
                        if (this.chkAddHipoteca2.Checked)
                            this.tabControl.SelectedTabPageIndex = 5;
                        else
                            this.tabControl.SelectedTabPageIndex = 0;
                    }
                }

                else if (this.tabControl.SelectedTabPageIndex == 4)
                {
                    if (this.chkAddHipoteca2.Checked)
                        this.tabControl.SelectedTabPageIndex = 5;
                    else
                        this.tabControl.SelectedTabPageIndex = 0;
                }
                else if (this.tabControl.SelectedTabPageIndex == 5)
                    this.tabControl.SelectedTabPageIndex = 0;
                #endregion
            }
            
            else
            {
                #region  Verifica

                if (this.tabControl.SelectedTabPageIndex == 6)
                    this.tabControl.SelectedTabPageIndex = 7;
                else if (this.tabControl.SelectedTabPageIndex == 7)
                    this.tabControl.SelectedTabPageIndex = 0;
                else if (this.tabControl.SelectedTabPageIndex == 0)
                    this.tabControl.SelectedTabPageIndex = 1;
                else if (this.tabControl.SelectedTabPageIndex == 1)
                {
                    if (this.chkAddPrenda.Checked)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                    {
                        if (this.chkAddPrenda2.Checked)
                            this.tabControl.SelectedTabPageIndex = 3;
                        else
                        {
                            if (this.chkAddHipoteca.Checked)
                                this.tabControl.SelectedTabPageIndex = 4;
                            else
                            {
                                if (this.chkAddHipoteca2.Checked)
                                    this.tabControl.SelectedTabPageIndex = 5;
                                else
                                    this.tabControl.SelectedTabPageIndex = 6;
                            }
                        }
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 2)
                {
                    if (this.chkAddPrenda2.Checked)
                        this.tabControl.SelectedTabPageIndex = 3;
                    else
                    {
                        if (this.chkAddHipoteca.Checked)
                            this.tabControl.SelectedTabPageIndex = 4;
                        else
                        {
                            if (this.chkAddHipoteca2.Checked)
                                this.tabControl.SelectedTabPageIndex = 5;
                            else
                                this.tabControl.SelectedTabPageIndex = 6;
                        }
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 3)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 4;
                    else
                    {
                        if (this.chkAddHipoteca2.Checked)
                            this.tabControl.SelectedTabPageIndex = 5;
                        else
                            this.tabControl.SelectedTabPageIndex = 6;
                    }
                }

                else if (this.tabControl.SelectedTabPageIndex == 4)
                {
                    if (this.chkAddHipoteca2.Checked)
                        this.tabControl.SelectedTabPageIndex = 5;
                    else
                        this.tabControl.SelectedTabPageIndex = 6;
                }
                else if (this.tabControl.SelectedTabPageIndex == 5)
                    this.tabControl.SelectedTabPageIndex = 6;
                #endregion
            }
        
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (!this._verifica)
            {
            //    if (this.tabControl.SelectedTabPageIndex == 0)
            //        this.tabControl.SelectedTabPageIndex = 3;
            //    else if (this.tabControl.SelectedTabPageIndex == 3)
            //        this.tabControl.SelectedTabPageIndex = 2;
            //    else if (this.tabControl.SelectedTabPageIndex == 2)
            //        this.tabControl.SelectedTabPageIndex = 1;
            //    else if (this.tabControl.SelectedTabPageIndex == 1)
            //        this.tabControl.SelectedTabPageIndex = 0;
            //
                if (this.tabControl.SelectedTabPageIndex == 0)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 3;
                    else
                    {
                        if (this.chkAddHipoteca.Checked)
                            this.tabControl.SelectedTabPageIndex = 2;
                        else
                            this.tabControl.SelectedTabPageIndex = 1;
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 3)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                        this.tabControl.SelectedTabPageIndex = 1;
                }
                else if (this.tabControl.SelectedTabPageIndex == 2)
                    this.tabControl.SelectedTabPageIndex = 1;
                else if (this.tabControl.SelectedTabPageIndex == 1)
                    this.tabControl.SelectedTabPageIndex = 0;


            }
           else
            {
                //if (this.tabControl.SelectedTabPageIndex == 4)
                //    this.tabControl.SelectedTabPageIndex = 3;
                //else if (this.tabControl.SelectedTabPageIndex == 3)
                //    this.tabControl.SelectedTabPageIndex = 2;
                //else if (this.tabControl.SelectedTabPageIndex == 2)
                //    this.tabControl.SelectedTabPageIndex = 1;
                //else if (this.tabControl.SelectedTabPageIndex == 1)
                //    this.tabControl.SelectedTabPageIndex = 0;
                //else if (this.tabControl.SelectedTabPageIndex == 0)
                //    this.tabControl.SelectedTabPageIndex = 5;
                //else if (this.tabControl.SelectedTabPageIndex == 5)
                //    this.tabControl.SelectedTabPageIndex = 4;
                if (this.tabControl.SelectedTabPageIndex == 4)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 3;
                    else
                    {
                        if (this.chkAddHipoteca.Checked)
                            this.tabControl.SelectedTabPageIndex = 2;
                        else
                            this.tabControl.SelectedTabPageIndex = 1;
                    }
                }
                else if (this.tabControl.SelectedTabPageIndex == 3)
                {
                    if (this.chkAddHipoteca.Checked)
                        this.tabControl.SelectedTabPageIndex = 2;
                    else
                        this.tabControl.SelectedTabPageIndex = 1;
                }
                else if (this.tabControl.SelectedTabPageIndex == 2)
                    this.tabControl.SelectedTabPageIndex = 1;
                else if (this.tabControl.SelectedTabPageIndex == 1)
                    this.tabControl.SelectedTabPageIndex = 0;
                else if (this.tabControl.SelectedTabPageIndex == 0)
                    this.tabControl.SelectedTabPageIndex = 5;
                else if (this.tabControl.SelectedTabPageIndex == 5)
                    this.tabControl.SelectedTabPageIndex = 4;


            }
        }

        private void linkConyuge_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyuge.Dock == DockStyle.Fill)
            {
                this.linkConyuge.Dock = DockStyle.None;
                this.linkConyugeInmueble.Dock = DockStyle.None;
            }
            else
            {
                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkConyugeInmueble.Dock = DockStyle.Fill;
            }
        }

        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor1.Dock == DockStyle.Fill)
            {
                linkCodeudor1.Dock = DockStyle.None;
                linkCodeudor1Inmueble.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor1.Dock = DockStyle.Fill;
                linkCodeudor1Inmueble.Dock = DockStyle.Fill;
            }
        }

        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor2.Dock == DockStyle.Fill)
            {
                linkCodeudor2.Dock = DockStyle.None;
                linkCodeudor2Inmueble.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor2.Dock = DockStyle.Fill;
                linkCodeudor2Inmueble.Dock = DockStyle.Fill;
            }
        }

        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor3.Dock == DockStyle.Fill)
            {
                linkCodeudor3.Dock = DockStyle.None;
                linkCodeudor3Inmueble.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor3.Dock = DockStyle.Fill;
                linkCodeudor3Inmueble.Dock = DockStyle.Fill;
            }
        }

        private void linkConyugeInmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyugeInmueble.Dock == DockStyle.Fill)
                this.linkConyugeInmueble.Dock = DockStyle.None;
            else
                this.linkConyugeInmueble.Dock = DockStyle.Fill;
        }

        private void linkCodeudor1Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor1Inmueble.Dock == DockStyle.Fill)
            {
                this.linkCodeudor1Inmueble.Dock = DockStyle.None;
            }
            else
                this.linkCodeudor1Inmueble.Dock = DockStyle.Fill;
        }

        private void linkCodeudor2Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor2Inmueble.Dock == DockStyle.Fill)
                this.linkCodeudor2Inmueble.Dock = DockStyle.None;
            else
                this.linkCodeudor2Inmueble.Dock = DockStyle.Fill;

        }

        private void linkCodeudor3Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor3Inmueble.Dock == DockStyle.Fill)
                this.linkCodeudor3Inmueble.Dock = DockStyle.None;
            else
                this.linkCodeudor3Inmueble.Dock = DockStyle.Fill;

        }
 
        /// <summary>
        /// Al cambiar de tab en el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
            {
                this.lblTituloDatosPersonales.Visible = true;
                this.lblTitDatosInmuebles.Visible = false;

            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
                this.lblTitDatosInmuebles.Visible = true;
                this.lblTituloDatosPersonales.Visible = false;                
            }
            else if (this.tabControl.SelectedTabPageIndex == 2)
            {
                this.lblTituloDatosPersonales.Visible = false;
                this.lblTitDatosInmuebles.Visible = false;
            }

        }

        /// <summary>
        /// Boton para editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (this._readOnly)
                return;

            SimpleButton ctrl = (SimpleButton)sender;
            ModalDigitacion mod = null;
            switch (ctrl.Name)
            {
                #region Deudor

                case "btnPriApellidoDeudor":
                    mod = new ModalDigitacion(this.txtPriApellidoDeudor.GetType(), this.lblPriApellido.Text + " Deudor", false, null,0,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriApellidoDeudor.Text = mod._valorFinal;
                    break;
                case "btnSdoApellidoDeudor":
                    mod = new ModalDigitacion(this.txtSdoApellidoDeudor.GetType(), this.lblSdoApellido.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoApellidoDeudor.Text = mod._valorFinal;
                    break;
                case "btnPriNombreDeudor":
                    mod = new ModalDigitacion(this.txtPriNombreDeudor.GetType(), this.lblPriNombre.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriNombreDeudor.Text = mod._valorFinal;
                    break;
                case "btnSdoNombreDeudor":
                    mod = new ModalDigitacion(this.txtSdoNombreDeudor.GetType(), this.lblSdoNombre.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoNombreDeudor.Text = mod._valorFinal;
                    break;

                case "btnTerceroDocTipoDeudor":
                    mod = new ModalDigitacion(this.masterTerceroDocTipoDeudor.GetType(), this.lblTipoDoc.Text + " Deudor", false, null, AppMasters.coTerceroDocTipo, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterTerceroDocTipoDeudor.Value = mod._valorFinal;
                    break;

                case "btnFechaExpDeudor":
                    mod = new ModalDigitacion(this.dtFechaExpDeudor.GetType(), this.lblFechaExpedicion.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaExpDeudor.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnCedulaDeudor":
                    mod = new ModalDigitacion(this.txtCedulaDeudor.GetType(), this.lblNroDocumento.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCedulaDeudor.Text = mod._valorFinal;
                    break;
                case "btnFechaNacDeudor":
                    mod = new ModalDigitacion(this.dtFechaNacDeudor.GetType(), this.lblFechaNacimiento.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaNacDeudor.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnEstadoCivilDeudor":
                    mod = new ModalDigitacion(this.cmbEstadoCivilDeudor.GetType(), this.lblEstadoCivil.Text + " Deudor", false, this.estadoCivil, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbEstadoCivilDeudor.EditValue = mod._valorFinal;
                    break;

                case "btnActEconPrincipalDeudor":
                    mod = new ModalDigitacion(this.masterActEconPrincipalDeudor.GetType(), this.lblActEconomica1.Text + " Deudor", false, null, AppMasters.coActEconomica,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterActEconPrincipalDeudor.Value = mod._valorFinal;
                    break;

                case "btnFuente1Deud":
                    mod = new ModalDigitacion(this.cmbFuente1Deudor.GetType(), this.lblFuente1.Text + " Deudor", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente1Deudor.EditValue = mod._valorFinal;
                    break;

                case "btnFuente2Deud":
                    mod = new ModalDigitacion(this.cmbFuente2Deudor.GetType(), this.lblFuente2.Text + " Deudor", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Deudor.EditValue = mod._valorFinal;
                    break;

                case "btnValorIngDeud":
                    mod = new ModalDigitacion(this.txtValorIngDeud.GetType(), this.lblIngresosRegistrados.Text + " Deudor", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngDeud.Text = mod._valorFinal;
                    break;

                case "btnValorIngSoporDeud":
                    mod = new ModalDigitacion(this.txtValorIngSoporDeud.GetType(), this.lblIngresosSoportados.Text + " Deudor", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngSoporDeud.Text = mod._valorFinal;
                    break;

                case "btnCorreoDeudor":
                    mod = new ModalDigitacion(this.txtCorreoDeudor.GetType(), this.lblCorreo.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCorreoDeudor.Text = mod._valorFinal;
                    break;

                case "btnCiudadDeudor":
                    mod = new ModalDigitacion(this.masterCiudadDeudor.GetType(), this.lblCiudadResidencia.Text + " Deudor", false, null, AppMasters.glLugarGeografico,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterCiudadDeudor.Value = mod._valorFinal;
                    break;

                case "btnNroInmueblesDeudor":
                    mod = new ModalDigitacion(this.txtNroInmueblesDeudor.GetType(), this.lblNroInmuebles.Text + " Deudor", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroInmueblesDeudor.EditValue = mod._valorFinal;
                    break;

                case "btnAntiguedadAñosDeudor":
                    mod = new ModalDigitacion(this.txtAntiguedadAñosDeudor.GetType(), this.lblAntCompra.Text + " Deudor", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtAntiguedadAñosDeudor.Text = mod._valorFinal;
                    break;

                case "btnUltimaAnotacionDeudor":
                    mod = new ModalDigitacion(this.txtUltimaAnotacionDeudor.GetType(), this.lblAntAnotacion.Text + " Deudor", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtUltimaAnotacionDeudor.Text = mod._valorFinal;
                    break;

                case "btnNroHipotecasDeudor":
                    mod = new ModalDigitacion(this.txtNroHipotecasDeudor.GetType(), this.lblHipotecas.Text + " Deudor", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroHipotecasDeudor.EditValue = mod._valorFinal;
                    break;

                case "btnNroRestriccionesDeudor":
                    mod = new ModalDigitacion(this.txtNroRestriccionesDeudor.GetType(), this.lblRestriccion.Text + " Deudor", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroRestriccionesDeudor.Text = mod._valorFinal;
                    break;


                case "btnFolioMatriculaDeudor":
                    mod = new ModalDigitacion(this.txtFolioMatriculaDeudor.GetType(), this.lblFolioMatricula.Text + " Deudor", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtFolioMatriculaDeudor.Text = mod._valorFinal;
                    break;

                case "btnFechaFolioDeudor":
                    mod = new ModalDigitacion(this.dtFechaFolioDeudor.GetType(), this.lblFechaMatricula.Text + " Deudor", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaFolioDeudor.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                #endregion

                #region Conyugue

                case "btnPriApellidoCony":
                    mod = new ModalDigitacion(this.txtPriApellidoCony.GetType(), this.lblPriApellido.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriApellidoCony.Text = mod._valorFinal;
                    break;
                case "btnSdoApellidoCony":
                    mod = new ModalDigitacion(this.txtSdoApellidoCony.GetType(), this.lblSdoApellido.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoApellidoCony.Text = mod._valorFinal;
                    break;
                case "btnPriNombreCony":
                    mod = new ModalDigitacion(this.txtPriNombreCony.GetType(), this.lblPriNombre.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriNombreCony.Text = mod._valorFinal;
                    break;
                case "btnSdoNombreCony":
                    mod = new ModalDigitacion(this.txtSdoNombreCony.GetType(), this.lblSdoNombre.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoNombreCony.Text = mod._valorFinal;
                    break;

                case "btnTerceroDocTipoCony":
                    mod = new ModalDigitacion(this.masterTerceroDocTipoCony.GetType(), this.lblTipoDoc.Text + " Conyugue", false, null, AppMasters.coTerceroDocTipo,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterTerceroDocTipoCony.Value = mod._valorFinal;
                    break;

                case "btnFechaExpCony":
                    mod = new ModalDigitacion(this.dtFechaExpCony.GetType(), this.lblFechaExpedicion.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaExpCony.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnCedulaCony":
                    mod = new ModalDigitacion(this.txtCedulaCony.GetType(), this.lblNroDocumento.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCedulaCony.Text = mod._valorFinal;

                    DTO_coTercero tercCony = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.txtCedulaCony.Text, true);
                    if (tercCony != null)
                    {
                        this.txtPriApellidoCony.Text = tercCony.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = tercCony.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = tercCony.NombrePri.Value;
                        this.txtSdoNombreCony.Text = tercCony.NombreSdo.Value;
                        this.masterTerceroDocTipoCony.Value = tercCony.TerceroDocTipoID.Value;                      
                            
                        this.cmbEstadoCivilCony.EditValue = this.cmbEstadoCivilDeudor.EditValue.ToString();
                        this.masterActEconPrincipalCony.Value = tercCony.ActEconomicaID.Value;
                        this.txtCorreoCony.Text = tercCony.CECorporativo.Value;
                        this.masterCiudadCony.Value = tercCony.LugarGeograficoID.Value;
                    }


                    break;
                case "btnFechaNacCony":
                    mod = new ModalDigitacion(this.dtFechaNacCony.GetType(), this.lblFechaNacimiento.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaNacCony.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnEstadoCivilCony":
                    mod = new ModalDigitacion(this.cmbEstadoCivilCony.GetType(), this.lblEstadoCivil.Text + " Conyugue", false, this.estadoCivil, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbEstadoCivilCony.EditValue = mod._valorFinal;
                    break;

                case "btnActEconPrincipalCony":
                    mod = new ModalDigitacion(this.masterActEconPrincipalCony.GetType(), this.lblActEconomica1.Text + " Conyugue", false, null, AppMasters.coActEconomica,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterActEconPrincipalCony.Value = mod._valorFinal;
                    break;

                case "btnFuente1Cony":
                    mod = new ModalDigitacion(this.cmbFuente1Cony.GetType(), this.lblFuente1.Text + " Conyugue", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente1Cony.EditValue = mod._valorFinal;
                    break;

                case "btnFuente2Cony":
                    mod = new ModalDigitacion(this.cmbFuente2Cony.GetType(), this.lblFuente2.Text + " Conyugue", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Cony.EditValue = mod._valorFinal;
                    break;

                case "btnValorIngCony":
                    mod = new ModalDigitacion(this.txtValorIngCony.GetType(), this.lblIngresosRegistrados.Text + " Conyugue", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngCony.Text = mod._valorFinal;
                    break;

                case "btnValorIngSoporCony":
                    mod = new ModalDigitacion(this.txtValorIngSoporCony.GetType(), this.lblIngresosSoportados.Text + " Conyugue", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngSoporCony.Text = mod._valorFinal;
                    break;

                case "btnCorreoCony":
                    mod = new ModalDigitacion(this.txtCorreoCony.GetType(), this.lblCorreo.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCorreoCony.Text = mod._valorFinal;
                    break;

                case "btnCiudadCony":
                    mod = new ModalDigitacion(this.masterCiudadCony.GetType(), this.lblCiudadResidencia.Text + " Conyugue", false, null, AppMasters.glLugarGeografico,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterCiudadCony.Value = mod._valorFinal;
                    break;

                case "btnNroInmueblesCony":
                    mod = new ModalDigitacion(this.txtNroInmueblesCony.GetType(), this.lblNroInmuebles.Text + " Conyugue", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroInmueblesCony.EditValue = mod._valorFinal;
                    break;

                case "btnAntiguedadAñosCony":
                    mod = new ModalDigitacion(this.txtAntiguedadAñosCony.GetType(), this.lblAntCompra.Text + " Conyugue", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtAntiguedadAñosCony.Text = mod._valorFinal;
                    break;

                case "btnUltimaAnotacionCony":
                    mod = new ModalDigitacion(this.txtUltimaAnotacionCony.GetType(), this.lblAntAnotacion.Text + " Conyugue", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtUltimaAnotacionCony.Text = mod._valorFinal;
                    break;

                case "btnNroHipotecasCony":
                    mod = new ModalDigitacion(this.txtNroHipotecasCony.GetType(), this.lblHipotecas.Text + " Conyugue", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroHipotecasCony.EditValue = mod._valorFinal;
                    break;

                case "btnNroRestriccionesCony":
                    mod = new ModalDigitacion(this.txtNroRestriccionesCony.GetType(), this.lblRestriccion.Text + " Conyugue", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroRestriccionesCony.Text = mod._valorFinal;
                    break;

                case "btnFolioMatriculaCony":
                    mod = new ModalDigitacion(this.txtFolioMatriculaCony.GetType(), this.lblFolioMatricula.Text + " Conyugue", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtFolioMatriculaCony.Text = mod._valorFinal;
                    break;

                case "btnFechaFolioCony":
                    mod = new ModalDigitacion(this.dtFechaFolioCony.GetType(), this.lblFechaMatricula.Text + " Conyugue", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaFolioCony.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;


                #endregion

                #region Codeudor1

                case "btnPriApellidoCod1":
                    mod = new ModalDigitacion(this.txtPriApellidoCod1.GetType(), this.lblPriApellido.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriApellidoCod1.Text = mod._valorFinal;
                    break;
                case "btnSdoApellidoCod1":
                    mod = new ModalDigitacion(this.txtSdoApellidoCod1.GetType(), this.lblSdoApellido.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoApellidoCod1.Text = mod._valorFinal;
                    break;
                case "btnPriNombreCod1":
                    mod = new ModalDigitacion(this.txtPriNombreCod1.GetType(), this.lblPriNombre.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriNombreCod1.Text = mod._valorFinal;
                    break;
                case "btnSdoNombreCod1":
                    mod = new ModalDigitacion(this.txtSdoNombreCod1.GetType(), this.lblSdoNombre.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoNombreCod1.Text = mod._valorFinal;
                    break;

                case "btnTerceroDocTipoCod1":
                    mod = new ModalDigitacion(this.masterTerceroDocTipoCod1.GetType(), this.lblTipoDoc.Text + " Codeudor1", false, null, AppMasters.coTerceroDocTipo,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterTerceroDocTipoCod1.Value = mod._valorFinal;
                    break;

                case "btnFechaExpCod1":
                    mod = new ModalDigitacion(this.dtFechaExpCod1.GetType(), this.lblFechaExpedicion.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaExpCod1.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnCedulaCod1":
                    mod = new ModalDigitacion(this.txtCedulaCod1.GetType(), this.lblNroDocumento.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCedulaCod1.Text = mod._valorFinal;

                    DTO_coTercero tercCod1 = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.txtCedulaCod1.Text, true);
                    if (tercCod1 != null)
                    {
                        this.txtPriApellidoCony.Text = tercCod1.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = tercCod1.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = tercCod1.NombrePri.Value;
                        this.txtSdoNombreCony.Text = tercCod1.NombreSdo.Value;
                        this.masterTerceroDocTipoCony.Value = tercCod1.TerceroDocTipoID.Value;                      
                            
                        this.masterActEconPrincipalCony.Value = tercCod1.ActEconomicaID.Value;
                        this.txtCorreoCony.Text = tercCod1.CECorporativo.Value;
                        this.masterCiudadCony.Value = tercCod1.LugarGeograficoID.Value;
                    }

                    break;
                case "btnFechaNacCod1":
                    mod = new ModalDigitacion(this.dtFechaNacCod1.GetType(), this.lblFechaNacimiento.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaNacCod1.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnEstadoCivilCod1":
                    mod = new ModalDigitacion(this.cmbEstadoCivilCod1.GetType(), this.lblEstadoCivil.Text + " Codeudor1", false, this.estadoCivil, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbEstadoCivilCod1.EditValue = mod._valorFinal;
                    break;

                case "btnActEconPrincipalCod1":
                    mod = new ModalDigitacion(this.masterActEconPrincipalCod1.GetType(), this.lblActEconomica1.Text + " Codeudor1", false, null, AppMasters.coActEconomica, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterActEconPrincipalCod1.Value = mod._valorFinal;
                    break;

                case "btnFuente1Cod1":
                    mod = new ModalDigitacion(this.cmbFuente1Cod1.GetType(), this.lblFuente1.Text + " Codeudor1", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente1Cod1.EditValue = mod._valorFinal;
                    break;

                case "btnFuente2Cod1":
                    mod = new ModalDigitacion(this.cmbFuente2Cod1.GetType(), this.lblFuente2.Text + " Codeudor1", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Cod1.EditValue = mod._valorFinal;
                    break;

                case "btnValorIngCod1":
                    mod = new ModalDigitacion(this.txtValorIngCod1.GetType(), this.lblIngresosRegistrados.Text + " Codeudor1", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngCod1.Text = mod._valorFinal;
                    break;

                case "btnValorIngSoporCod1":
                    mod = new ModalDigitacion(this.txtValorIngSoporCod1.GetType(), this.lblIngresosSoportados.Text + " Codeudor1", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngSoporCod1.Text = mod._valorFinal;
                    break;

                case "btnCorreoCod1":
                    mod = new ModalDigitacion(this.txtCorreoCod1.GetType(), this.lblCorreo.Text + " Codeudor1", false, null);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCorreoCod1.Text = mod._valorFinal;
                    break;

                case "btnCiudadCod1":
                    mod = new ModalDigitacion(this.masterCiudadCod1.GetType(), this.lblCiudadResidencia.Text + " Codeudor1", false, null, AppMasters.glLugarGeografico,0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterCiudadCod1.Value = mod._valorFinal;
                    break;

                case "btnNroInmueblesCod1":
                    mod = new ModalDigitacion(this.txtNroInmueblesCod1.GetType(), this.lblNroInmuebles.Text + " Codeudor1", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroInmueblesCod1.Text = mod._valorFinal;
                    break;

                case "btnAntiguedadAñosCod1":
                    mod = new ModalDigitacion(this.txtAntiguedadAñosCod1.GetType(), this.lblAntCompra.Text + " Codeudor1", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtAntiguedadAñosCod1.Text = mod._valorFinal;
                    break;

                case "btnUltimaAnotacionCod1":
                    mod = new ModalDigitacion(this.txtUltimaAnotacionCod1.GetType(), this.lblAntAnotacion.Text + " Codeudor1", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtUltimaAnotacionCod1.Text = mod._valorFinal;
                    break;

                case "btnNroHipotecasCod1":
                    mod = new ModalDigitacion(this.txtNroHipotecasCod1.GetType(), this.lblHipotecas.Text + " Codeudor1", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroHipotecasCod1.Text = mod._valorFinal;
                    break;

                case "btnNroRestriccionesCod1":
                    mod = new ModalDigitacion(this.txtNroRestriccionesCod1.GetType(), this.lblRestriccion.Text + " Codeudor1", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroRestriccionesCod1.Text = mod._valorFinal;
                    break;

                case "btnFolioMatriculaCod1":
                    mod = new ModalDigitacion(this.txtFolioMatriculaCod1.GetType(), this.lblFolioMatricula.Text + " Codeudor1", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtFolioMatriculaCod1.Text = mod._valorFinal;
                    break;

                case "btnFechaFolioCod1":
                    mod = new ModalDigitacion(this.dtFechaFolioCod1.GetType(), this.lblFechaMatricula.Text + " Codeudor1", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaFolioCod1.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;


                #endregion

                #region Codeudor2

                case "btnPriApellidoCod2":
                    mod = new ModalDigitacion(this.txtPriApellidoCod2.GetType(), this.lblPriApellido.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriApellidoCod2.Text = mod._valorFinal;
                    break;
                case "btnSdoApellidoCod2":
                    mod = new ModalDigitacion(this.txtSdoApellidoCod2.GetType(), this.lblSdoApellido.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoApellidoCod2.Text = mod._valorFinal;
                    break;
                case "btnPriNombreCod2":
                    mod = new ModalDigitacion(this.txtPriNombreCod2.GetType(), this.lblPriNombre.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriNombreCod2.Text = mod._valorFinal;
                    break;
                case "btnSdoNombreCod2":
                    mod = new ModalDigitacion(this.txtSdoNombreCod2.GetType(), this.lblSdoNombre.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoNombreCod2.Text = mod._valorFinal;
                    break;

                case "btnTerceroDocTipoCod2":
                    mod = new ModalDigitacion(this.masterTerceroDocTipoCod2.GetType(), this.lblTipoDoc.Text + " Codeudor2", false, null, AppMasters.coTerceroDocTipo,  0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterTerceroDocTipoCod2.Value = mod._valorFinal;
                    break;

                case "btnFechaExpCod2":
                    mod = new ModalDigitacion(this.dtFechaExpCod2.GetType(), this.lblFechaExpedicion.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaExpCod2.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnCedulaCod2":
                    mod = new ModalDigitacion(this.txtCedulaCod2.GetType(), this.lblNroDocumento.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCedulaCod2.Text = mod._valorFinal;

                    DTO_coTercero tercCod2 = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.txtCedulaCod2.Text, true);
                    if (tercCod2 != null)
                    {
                        this.txtPriApellidoCony.Text = tercCod2.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = tercCod2.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = tercCod2.NombrePri.Value;
                        this.txtSdoNombreCony.Text = tercCod2.NombreSdo.Value;
                        this.masterTerceroDocTipoCony.Value = tercCod2.TerceroDocTipoID.Value;                      
                            
                        this.masterActEconPrincipalCony.Value = tercCod2.ActEconomicaID.Value;
                        this.txtCorreoCony.Text = tercCod2.CECorporativo.Value;
                        this.masterCiudadCony.Value = tercCod2.LugarGeograficoID.Value;
                    }

                    break;
                case "btnFechaNacCod2":
                    mod = new ModalDigitacion(this.dtFechaNacCod2.GetType(), this.lblFechaNacimiento.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaNacCod2.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnEstadoCivilCod2":
                    mod = new ModalDigitacion(this.cmbEstadoCivilCod2.GetType(), this.lblEstadoCivil.Text + " Codeudor2", false, this.estadoCivil, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbEstadoCivilCod2.EditValue = mod._valorFinal;
                    break;

                case "btnActEconPrincipalCod2":
                    mod = new ModalDigitacion(this.masterActEconPrincipalCod2.GetType(), this.lblActEconomica1.Text + " Codeudor2", false, null, AppMasters.coActEconomica, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterActEconPrincipalCod2.Value = mod._valorFinal;
                    break;

                case "btnFuente1Cod2":
                    mod = new ModalDigitacion(this.cmbFuente1Cod2.GetType(), this.lblFuente1.Text + " Codeudor2", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Cod2.EditValue = mod._valorFinal;
                    break;

                case "btnFuente2Cod2":
                    mod = new ModalDigitacion(this.cmbFuente2Cod2.GetType(), this.lblFuente2.Text + " Codeudor2", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Cod2.EditValue = mod._valorFinal;
                    break;

                case "btnValorIngCod2":
                    mod = new ModalDigitacion(this.txtValorIngCod2.GetType(), this.lblIngresosRegistrados.Text + " Codeudor2", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngCod2.Text = mod._valorFinal;
                    break;

                case "btnValorIngSoporCod2":
                    mod = new ModalDigitacion(this.txtValorIngSoporCod2.GetType(), this.lblIngresosSoportados.Text + " Codeudor2", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngSoporCod2.Text = mod._valorFinal;
                    break;

                case "btnCorreoCod2":
                    mod = new ModalDigitacion(this.txtCorreoCod2.GetType(), this.lblCorreo.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCorreoCod2.Text = mod._valorFinal;
                    break;

                case "btnCiudadCod2":
                    mod = new ModalDigitacion(this.masterCiudadCod2.GetType(), this.lblCiudadResidencia.Text + " Codeudor2", false, null, AppMasters.glLugarGeografico,  0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterCiudadCod2.Value = mod._valorFinal;
                    break;

                case "btnNroInmueblesCod2":
                    mod = new ModalDigitacion(this.txtNroInmueblesCod2.GetType(), this.lblNroInmuebles.Text + " Codeudor2", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroInmueblesCod2.Text = mod._valorFinal;
                    break;

                case "btnAntiguedadAñosCod2":
                    mod = new ModalDigitacion(this.txtAntiguedadAñosCod2.GetType(), this.lblAntCompra.Text + " Codeudor2", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtAntiguedadAñosCod2.Text = mod._valorFinal;
                    break;

                case "btnUltimaAnotacionCod2":
                    mod = new ModalDigitacion(this.txtUltimaAnotacionCod2.GetType(), this.lblAntAnotacion.Text + " Codeudor2", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtUltimaAnotacionCod2.Text = mod._valorFinal;
                    break;

                case "btnNroHipotecasCod2":
                    mod = new ModalDigitacion(this.txtNroHipotecasCod2.GetType(), this.lblHipotecas.Text + " Codeudor2", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroHipotecasCod2.Text = mod._valorFinal;
                    break;

                case "btnNroRestriccionesCod2":
                    mod = new ModalDigitacion(this.txtNroRestriccionesCod2.GetType(), this.lblRestriccion.Text + " Codeudor2", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroRestriccionesCod2.Text = mod._valorFinal;
                    break;


                case "btnFolioMatriculaCod2":
                    mod = new ModalDigitacion(this.txtFolioMatriculaCod2.GetType(), this.lblFolioMatricula.Text + " Codeudor2", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtFolioMatriculaCod2.Text = mod._valorFinal;
                    break;

                case "btnFechaFolioCod2":
                    mod = new ModalDigitacion(this.dtFechaFolioCod2.GetType(), this.lblFechaMatricula.Text + " Codeudor2", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaFolioCod2.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                #endregion

                #region Codeudor3

                case "btnPriApellidoCod3":
                    mod = new ModalDigitacion(this.txtPriApellidoCod3.GetType(), this.lblPriApellido.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriApellidoCod3.Text = mod._valorFinal;
                    break;
                case "btnSdoApellidoCod3":
                    mod = new ModalDigitacion(this.txtSdoApellidoCod3.GetType(), this.lblSdoApellido.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoApellidoCod3.Text = mod._valorFinal;
                    break;
                case "btnPriNombreCod3":
                    mod = new ModalDigitacion(this.txtPriNombreCod3.GetType(), this.lblPriNombre.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtPriNombreCod3.Text = mod._valorFinal;
                    break;
                case "btnSdoNombreCod3":
                    mod = new ModalDigitacion(this.txtSdoNombreCod3.GetType(), this.lblSdoNombre.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtSdoNombreCod3.Text = mod._valorFinal;
                    break;

                case "btnTerceroDocTipoCod3":
                    mod = new ModalDigitacion(this.masterTerceroDocTipoCod3.GetType(), this.lblTipoDoc.Text + " Codeudor3", false, null, AppMasters.coTerceroDocTipo, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterTerceroDocTipoCod3.Value = mod._valorFinal;
                    break;

                case "btnFechaExpCod3":
                    mod = new ModalDigitacion(this.dtFechaExpCod3.GetType(), this.lblFechaExpedicion.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaExpCod3.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnCedulaCod3":
                    mod = new ModalDigitacion(this.txtCedulaCod3.GetType(), this.lblNroDocumento.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCedulaCod3.Text = mod._valorFinal;
                    break;

                    DTO_coTercero tercCod3 = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.txtCedulaCod3.Text, true);
                    if (tercCod3 != null)
                    {
                        this.txtPriApellidoCony.Text = tercCod3.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = tercCod3.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = tercCod3.NombrePri.Value;
                        this.txtSdoNombreCony.Text = tercCod3.NombreSdo.Value;
                        this.masterTerceroDocTipoCony.Value = tercCod3.TerceroDocTipoID.Value;                      
                            
                        this.masterActEconPrincipalCony.Value = tercCod3.ActEconomicaID.Value;
                        this.txtCorreoCony.Text = tercCod3.CECorporativo.Value;
                        this.masterCiudadCony.Value = tercCod3.LugarGeograficoID.Value;
                    }

                case "btnFechaNacCod3":
                    mod = new ModalDigitacion(this.dtFechaNacCod3.GetType(), this.lblFechaNacimiento.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaNacCod3.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;

                case "btnEstadoCivilCod3":
                    mod = new ModalDigitacion(this.cmbEstadoCivilCod3.GetType(), this.lblEstadoCivil.Text + " Codeudor3", false, this.estadoCivil, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbEstadoCivilCod3.EditValue = mod._valorFinal;
                    break;

                case "btnActEconPrincipalCod3":
                    mod = new ModalDigitacion(this.masterActEconPrincipalCod3.GetType(), this.lblActEconomica1.Text + " Codeudor3", false, null, AppMasters.coActEconomica, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterActEconPrincipalCod3.Value = mod._valorFinal;
                    break;

                case "btnFuente1Cod3":
                    mod = new ModalDigitacion(this.cmbFuente1Cod3.GetType(), this.lblFuente1.Text + " Codeudor3", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente1Cod3.EditValue = mod._valorFinal;
                    break;

                case "btnFuente2Cod3":
                    mod = new ModalDigitacion(this.cmbFuente2Cod3.GetType(), this.lblFuente2.Text + " Codeudor3", false, this.fuenteIngresos, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.cmbFuente2Cod3.EditValue = mod._valorFinal;
                    break;

                case "btnValorIngCod3":
                    mod = new ModalDigitacion(this.txtValorIngCod3.GetType(), this.lblIngresosRegistrados.Text + " Codeudor3", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngCod3.Text = mod._valorFinal;
                    break;

                case "btnValorIngSoporCod3":
                    mod = new ModalDigitacion(this.txtValorIngSoporCod3.GetType(), this.lblIngresosSoportados.Text + " Codeudor3", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtValorIngSoporCod3.Text = mod._valorFinal;
                    break;

                case "btnCorreoCod3":
                    mod = new ModalDigitacion(this.txtCorreoCod3.GetType(), this.lblCorreo.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtCorreoCod3.Text = mod._valorFinal;
                    break;

                case "btnCiudadCod3":
                    mod = new ModalDigitacion(this.masterCiudadCod3.GetType(), this.lblCiudadResidencia.Text + " Codeudor3", false, null, AppMasters.glLugarGeografico, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.masterCiudadCod3.Value = mod._valorFinal;
                    break;

                case "btnNroInmueblesCod3":
                    mod = new ModalDigitacion(this.txtNroInmueblesCod3.GetType(), this.lblNroInmuebles.Text + " Codeudor3", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroInmueblesCod3.Text = mod._valorFinal;
                    break;

                case "btnAntiguedadAñosCod3":
                    mod = new ModalDigitacion(this.txtAntiguedadAñosCod3.GetType(), this.lblAntCompra.Text + " Codeudor3", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtAntiguedadAñosCod3.Text = mod._valorFinal;
                    break;

                case "btnUltimaAnotacionCod3":
                    mod = new ModalDigitacion(this.txtUltimaAnotacionCod3.GetType(), this.lblAntAnotacion.Text + " Codeudor3", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtUltimaAnotacionCod3.Text = mod._valorFinal;
                    break;

                case "btnNroHipotecasCod3":
                    mod = new ModalDigitacion(this.txtNroHipotecasCod3.GetType(), this.lblHipotecas.Text + " Codeudor3", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroHipotecasCod3.Text = mod._valorFinal;
                    break;

                case "btnNroRestriccionesCod3":
                    mod = new ModalDigitacion(this.txtNroRestriccionesCod3.GetType(), this.lblRestriccion.Text + " Codeudor3", true, null, 0, 2);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtNroRestriccionesCod3.Text = mod._valorFinal;
                    break;

                case "btnFolioMatriculaCod3":
                    mod = new ModalDigitacion(this.txtFolioMatriculaCod3.GetType(), this.lblFolioMatricula.Text + " Codeudor3", true, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.txtFolioMatriculaCod3.Text = mod._valorFinal;
                    break;

                case "btnFechaFolioCod3":
                    mod = new ModalDigitacion(this.dtFechaFolioCod3.GetType(), this.lblFechaMatricula.Text + " Codeudor3", false, null, 0, 0);
                    mod.ShowDialog();
                    if (!string.IsNullOrEmpty(mod._valorFinal))
                        this.dtFechaFolioCod3.DateTime = Convert.ToDateTime(mod._valorFinal);
                    break;


                    #endregion

            }
        }

        /// <summary>
        /// Trae valores de acuerdo al valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterFasecolda_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterFasecolda.ValidID)
                {
                    DTO_ccFasecolda fase = (DTO_ccFasecolda)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, false, this.masterFasecolda.Value, true);
                    this.cmbServicioFasecolda.EditValue = fase.Servicio.Value;
                    this.txtFasecoldaMarca.Text = fase.Marca.Value;
                    this.txtFasecoldaClase.Text = fase.Clase.Value;
                    this.txtFasecoldaTipo1.Text = fase.Tipo1.Value;
                    this.txtFasecoldaTipo2.Text = fase.Tipo2.Value;
                    this.txtFasecoldaTipo3.Text = fase.Tipo3.Value;
                    this.txtFasecoldaTipoCaja.Text = fase.TipoCaja.Value.ToString();
                    this.cmbFasecoldaTipoCaja.EditValue = fase.TipoCaja.Value;
                    this.txtFasecoldaCilindraje.Text = fase.Cilindraje.Value.ToString();
                    this.txtFasecoldaPeso.Text = "0";
                    this.txtFasecoldaPasajeros.Text = fase.Pasajeros.Value.ToString();
                    this.txtFasecoldaPuertas.Text = fase.Puertas.Value.ToString();
                    this.chkFasecoldaAire.Checked = fase.AireAcondicionadoInd.Value.Value;
                    this.txtFasecoldaCombustible.Text = "";
                    this.txtFasecoldaCarga.Text = fase.Carga.Value.ToString();
                    this.txtFasecoldaTransmision.Text = "";

                    Dictionary<string, string> keys = new Dictionary<string, string>();
                    keys.Add("FaseColdaID", this.masterFasecolda.Value);
                    keys.Add("Modelo", this.txtModelo.Text);
                    DTO_ccFasecoldaModelo valor = (DTO_ccFasecoldaModelo)this._bc.GetMasterComplexDTO(AppMasters.ccFasecoldaModelo, keys, true);
                    if (valor != null)
                        this.txtFasecoldaValor.EditValue = valor.Valor.Value;
                    else
                        this.txtFasecoldaValor.EditValue = 0;
                }
                else
                {
                    this.cmbServicioFasecolda.EditValue = "1";
                    this.txtFasecoldaMarca.Text = string.Empty;
                    this.txtFasecoldaClase.Text = string.Empty;
                    this.txtFasecoldaTipo1.Text = string.Empty;
                    this.txtFasecoldaTipo2.Text = string.Empty;
                    this.txtFasecoldaTipo3.Text = string.Empty;
                    this.txtFasecoldaTipoCaja.Text = string.Empty;
                    this.cmbFasecoldaTipoCaja.EditValue = "1";
                    this.txtFasecoldaCilindraje.Text = string.Empty;
                    this.txtFasecoldaPeso.Text = "0";
                    this.txtFasecoldaPasajeros.Text = string.Empty; 
                    this.txtFasecoldaPuertas.Text = "0";
                    this.chkFasecoldaAire.Checked = false;
                    this.txtFasecoldaCombustible.Text = "";
                    this.txtFasecoldaCarga.Text = string.Empty; 
                    this.txtFasecoldaTransmision.Text = string.Empty; 
                    this.txtFasecoldaValor.EditValue = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionSolicitudNuevos.cs", "masterFasecolda_Leave"));
            }
        }

        /// <summary>
        /// Al dar clic en el fasecolda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFaseColda_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ModalFasecoldaFilter mod = new ModalFasecoldaFilter(AppDocuments.RegistroSolicitud);
                mod.ShowDialog();
                this.masterFasecolda.Value = mod.IDSelected;
                this.masterFasecolda.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DigitacionSolNuevos.cs", "TBPaste: " + ex.Message));
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
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


        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        /// 

        public override void TBSave()
        {
            try
            {
               // this._data.DatosPersonales.Clear();
                //this.AssignValues(false);
                this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                if (this.ValidateData())
                {
                    this._actividad = new List<DTO_glDocumentoChequeoLista>();
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = null;
                    if (this._verifica)
                    {
                        foreach (DTO_glDocumentoChequeoLista basic in this._actividadChequeo)
                        {
                            this._actividad.Add(basic);
                        }
                        foreach (DTO_glDocumentoChequeoLista basic2 in this._actividadChequeoDocumento)
                        {
                            this._actividad.Add(basic2);
                        }

                       result = _bc.AdministrationModel.DigitacionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividad, false);
                    }
                    else
                    {
                        result = _bc.AdministrationModel.DigitacionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, false);
                    }
                     if (result.Result == ResultValue.OK)
                    {
                        //#region Obtiene el nombre

                        //string nombre = this._data.SolicituDocu.NombrePri.Value;
                        //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.NombreSdo.Value))
                        //    nombre += " " + this._data.SolicituDocu.NombreSdo.Value;
                        //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.ApellidoPri.Value))
                        //    nombre += " " + this._data.SolicituDocu.ApellidoPri.Value;
                        //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.ApellidoSdo.Value))
                        //    nombre += " " + this._data.SolicituDocu.ApellidoSdo.Value;

                        //#endregion
                        //#region Variables para el mail

                        //DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                        //string body = string.Empty;
                        //string subject = string.Empty;
                        //string email = user.CorreoElectronico.Value;

                        //string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                        //string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_SentToAprobCartera_Body);
                        //string formName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                        //#endregion
                        //#region Envia el correo
                        //subject = string.Format(subjectApr, formName);
                        //body = string.Format(bodyApr, formName, this.txtCedulaDeudor.Text.Trim(), nombre, this._data.SolicituDocu.Observacion.Value);
                        //_bc.SendMail(this._documentID, subject, body, email);
                        //#endregion

                        //Actualiza el control para las financieras
                        string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                        if (sectorLib == ((byte)SectorCartera.Financiero).ToString()) //Financieras
                        {
                            string numeroControl = _bc.AdministrationModel.Empresa.NumeroControl.Value;
                            _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numeroControl).ToList();
                        }

                        this.txtCedulaDeudor.Focus();
                        if (!this._aprueba)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Dr_SolicitudGuardada);
                            MessageBox.Show(string.Format(msg, this._libranzaID));
                        }

                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                
                this._aprueba = false;
                string msgDoc ="";
                if(!this._verifica)
                    msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar el registro de la solicitud?  ");
                else
                    msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar la revision?  ");

                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                else
                    if (this._verifica)
                    {
                        this._aprueba = true;
                        //this._actividadChequeo.Add(this._actividadChequeoDocumento);
                        this._actividad= new List<DTO_glDocumentoChequeoLista>();
                        foreach (DTO_glDocumentoChequeoLista basic in this._actividadChequeo)
                        {
                            this._actividad.Add(basic);
                        }
                        foreach (DTO_glDocumentoChequeoLista basic2 in this._actividadChequeoDocumento)
                        {
                            this._actividad.Add(basic2);
                        }


                    }

                this.AssignValues(false);
                this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                if (this.ValidateData())
                {
                    DTO_TxResult result = null;
                    if (this._verifica)
                    {
                        string Observacion = this.ValidateListas();

                        string msgDocAprob = "";
                        msgDocAprob = this._bc.GetResource(LanguageTypes.Messages, "Hay documentos pendientes desea enviar a Negocios para gestionar:" + Observacion);

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
                        TBSave();
                        result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividad, true);
                    }
                    else
                    {
                        result = _bc.AdministrationModel.DigitacionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, !this._aprueba);
                    }
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                    if (result.Result == ResultValue.OK)
                    {
                        this.CleanData();
                        //CIerra el formulario
                        FormProvider.CloseCurrent();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSendtoAppr"));
            }
        }


        #endregion Eventos Barra Herramientas

        private void btnDesestimiento_Click(object sender, EventArgs e)
        {
            ModalDesestimiento mod2 = null;

            mod2 = new ModalDesestimiento("Desistimiento", AppMasters.ccDevolucionCausal);
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


        private void txtDireccion_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            CodificacionDireccion dir = new CodificacionDireccion((DevExpress.XtraEditors.ButtonEdit)sender);
            dir.ShowDialog();
        }

        private void chkAddPrenda_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkAddPrenda.Checked)
                this.tpDatosVehiculo.PageVisible = false;
            else
                this.tpDatosVehiculo.PageVisible = true;
        }

        private void chkAddHipoteca_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkAddHipoteca.Checked)
                this.tpHipoteca.PageVisible = false;
            else
                this.tpHipoteca.PageVisible = true;
        }

        private void chkAddPrenda2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkAddPrenda2.Checked)
                this.tpDatosVehiculo2.PageVisible = false;
            else
                this.tpDatosVehiculo2.PageVisible = true;

        }

        private void chkAddHipoteca2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.chkAddHipoteca2.Checked)
                this.tpHipoteca2.PageVisible = false;
            else
                this.tpHipoteca2.PageVisible = true;
        }

        private void chkViviendaNueva_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkViviendaNueva.Checked)
            {
                this.txtValorCompraventa.Enabled = true;
                this.dtFechaCompraventa.Enabled = true;
                
                this.txtValorPredial.EditValue = 0;
                this.dtFechaPredial.EditValue = null;

                this.txtValorPredial.Enabled = false;
                this.dtFechaPredial.Enabled = false;
            }
            else
            {
                this.txtValorPredial.Enabled = true;
                this.dtFechaPredial.Enabled = true;

                this.txtValorCompraventa.EditValue = 0;
                this.dtFechaCompraventa.EditValue = null;

                this.txtValorCompraventa.Enabled = false;
                this.dtFechaCompraventa.Enabled = false;
            }
        }

        private void chkViviendaNueva2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkViviendaNueva.Checked)
            {
                this.txtValorCompraventa.Enabled = true;
                this.dtFechaCompraventa.Enabled = true;

                this.txtValorPredial.EditValue = 0;
                this.dtFechaPredial.EditValue = null;

                this.txtValorPredial.Enabled = false;
                this.dtFechaPredial.Enabled = false;
            }
            else
            {
                this.txtValorPredial.Enabled = true;
                this.dtFechaPredial.Enabled = true;

                this.txtValorCompraventa.EditValue = 0;
                this.dtFechaCompraventa.EditValue = null;

                this.txtValorCompraventa.Enabled = false;
                this.dtFechaCompraventa.Enabled = false;
            }
        }





     
    }
}