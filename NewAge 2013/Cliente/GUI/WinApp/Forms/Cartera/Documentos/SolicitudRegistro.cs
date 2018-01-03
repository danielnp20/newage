
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
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class SolicitudRegistro : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudCredito _data = new DTO_DigitaSolicitudCredito();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

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

        //Variables ToolBar
        private bool _isLoaded;

        private DateTime periodo = DateTime.Now;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SolicitudRegistro()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SolicitudRegistro(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor que permite filtrar la libranza y validar si es solo lectura para Modulo cf
        /// </summary>
        /// <param name="libranza"> Libranza o credito a filtrar</param>
        /// <param name="readOnly"> Si es solo lectura</param>
        public SolicitudRegistro(int libranza, bool readOnly)
        {
            this.Constructor(libranza,null);
            //this..ActiveFilterString = "StartsWith([Unbound_Libranza]," + libranza.ToString() + ")";
            if (readOnly)
            {
                //this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Visible = false;
                //this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].Visible = false;
                FormProvider.Master.itemSave.Enabled = false;
            }
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(int? libranzaNro,string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cf;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (this._frmModule == ModulesPrefix.cf && actividades.Count > 0)
                {
                    this.nextActID = string.Empty;
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
                    else
                    {
                        this.nextActID = NextActs[0];
                        //string actividadFlujoID = actividades[0];
                        //this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                    }
                    #endregion
                }
                #endregion
                this.LoadData(libranzaNro.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "Constructor"));
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
                this._documentID = AppDocuments.RegistroSolicitud;
                this._frmModule = ModulesPrefix.cf;

                //Carga la informacion de la maestras
                this._bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
                this._bc.InitMasterUC(this.masterEntidad, AppMasters.ccPagaduria, false, true, true,true);
                this._bc.InitMasterUC(this.masterBanco, AppMasters.tsBanco, false, true, true, false);

                this._bc.InitMasterUC(this.masterCiudadExpDeudor, AppMasters.glLugarGeografico, false, false, true, false);
                this._bc.InitMasterUC(this.masterCiudadExpCod1, AppMasters.glLugarGeografico, false, false, true, false);
                this._bc.InitMasterUC(this.masterCiudadExpCod2, AppMasters.glLugarGeografico, false, false, true, false);
                this._bc.InitMasterUC(this.masterCiudadExpCod3, AppMasters.glLugarGeografico, false, false, true, false);                

                this._bc.InitMasterUC(this.masterTerceroDocTipoDeudor, AppMasters.coTerceroDocTipo, false, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod1, AppMasters.coTerceroDocTipo, false, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod2, AppMasters.coTerceroDocTipo, false, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod3, AppMasters.coTerceroDocTipo, false, true, true, false);
                
                this._bc.InitMasterUC(this.masterCiudadResDeudor, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadResCod1, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadResCod2, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadResCod3, AppMasters.glLugarGeografico, false, true, true, false);

                this._bc.InitMasterUC(this.masterCiudadTrabajoDeudor, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadTrabajoCod1, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadTrabajoCod2, AppMasters.glLugarGeografico, false, true, true, false);
                this._bc.InitMasterUC(this.masterCiudadTrabajoCod3, AppMasters.glLugarGeografico, false, true, true, false);

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);
                
                //Pagaduría
                this._pagaduriaXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PagaduriaXDefecto);

                
                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                
                
                Dictionary<string, string> tipoVivienda= new Dictionary<string, string>();                               
                tipoVivienda.Add("1",this._bc.GetResource(LanguageTypes.Tables,"tbl_restr_ResidenciaTipo_v1"));
                tipoVivienda.Add("2",this._bc.GetResource(LanguageTypes.Tables,"tbl_restr_ResidenciaTipo_v2"));
                tipoVivienda.Add("3",this._bc.GetResource(LanguageTypes.Tables,"tbl_restr_ResidenciaTipo_v3"));                
                                
                Dictionary<string, string> tipoContrato = new Dictionary<string, string>();
                tipoContrato.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TerminoContrato_v1"));
                tipoContrato.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TerminoContrato_v2"));
                tipoContrato.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TerminoContrato_v3"));                
                
                Dictionary<string, string> referencia = new Dictionary<string, string>();
                referencia.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Referencia_v1"));
                referencia.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Referencia_v2"));
                referencia.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Referencia_v3"));
                referencia.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Referencia_v4"));
                
                Dictionary<string, string> ctaTipo = new Dictionary<string, string>();
                ctaTipo.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_CtaTipo_v1"));
                ctaTipo.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_CtaTipo_v2"));
                ctaTipo.Add("3", this._bc.GetResource(LanguageTypes.Tables, "No Aplica"));

                Dictionary<string, string> sexo= new Dictionary<string, string>();
                sexo.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Sexo_v1"));
                sexo.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_Sexo_v2"));

                Dictionary<string, string> periodicidad = new Dictionary<string, string>();
                periodicidad.Add("1", this._bc.GetResource(LanguageTypes.Tables,"Mensual"));
                periodicidad.Add("2", this._bc.GetResource(LanguageTypes.Tables, "Quincenal"));

                Dictionary<string, string> estadoCivil= new Dictionary<string, string>();
                estadoCivil.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v0"));
                estadoCivil.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v1"));
                estadoCivil.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v2"));
                estadoCivil.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v3"));
                estadoCivil.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v4"));
                estadoCivil.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_EstadoCivil_v5"));

                this.cmbCuentatipo.Properties.ValueMember = "Key";
                this.cmbCuentatipo.Properties.DisplayMember = "Value";
                this.cmbCuentatipo.Properties.DataSource = ctaTipo;
                this.cmbCuentatipo.EditValue = "3";

                this.cmbPeriodicidad.Properties.ValueMember = "Key";
                this.cmbPeriodicidad.Properties.DisplayMember = "Value";
                this.cmbPeriodicidad.Properties.DataSource = periodicidad;
                this.cmbPeriodicidad.EditValue = "1";

                this.cmbTipoContratoDeudor.Properties.ValueMember = "Key";
                this.cmbTipoContratoDeudor.Properties.DisplayMember = "Value";
                this.cmbTipoContratoDeudor.Properties.DataSource = tipoContrato;
                this.cmbTipoContratoDeudor.EditValue = "1";

                this.cmbTipoContratoCod1.Properties.ValueMember = "Key";
                this.cmbTipoContratoCod1.Properties.DisplayMember = "Value";
                this.cmbTipoContratoCod1.Properties.DataSource = tipoContrato;
                this.cmbTipoContratoCod1.EditValue = "1";

                this.cmbTipoContratoCod2.Properties.ValueMember = "Key";
                this.cmbTipoContratoCod2.Properties.DisplayMember = "Value";
                this.cmbTipoContratoCod2.Properties.DataSource = tipoContrato;
                this.cmbTipoContratoCod2.EditValue = "1";

                this.cmbTipoContratoCod3.Properties.ValueMember = "Key";
                this.cmbTipoContratoCod3.Properties.DisplayMember = "Value";
                this.cmbTipoContratoCod3.Properties.DataSource = tipoContrato;
                this.cmbTipoContratoCod3.EditValue = "1";

                this.cmbTipoViviendaDeudor.Properties.ValueMember = "Key";
                this.cmbTipoViviendaDeudor.Properties.DisplayMember = "Value";
                this.cmbTipoViviendaDeudor.Properties.DataSource = tipoVivienda;
                this.cmbTipoViviendaDeudor.EditValue = "1";

                this.cmbTipoViviendaCod1.Properties.ValueMember = "Key";
                this.cmbTipoViviendaCod1.Properties.DisplayMember = "Value";
                this.cmbTipoViviendaCod1.Properties.DataSource = tipoVivienda;
                this.cmbTipoViviendaCod1.EditValue = "1";

                this.cmbTipoViviendaCod2.Properties.ValueMember = "Key";
                this.cmbTipoViviendaCod2.Properties.DisplayMember = "Value";
                this.cmbTipoViviendaCod2.Properties.DataSource = tipoVivienda;
                this.cmbTipoViviendaCod2.EditValue = "1";

                this.cmbTipoViviendaCod3.Properties.ValueMember = "Key";
                this.cmbTipoViviendaCod3.Properties.DisplayMember = "Value";
                this.cmbTipoViviendaCod3.Properties.DataSource = tipoVivienda;
                this.cmbTipoViviendaCod3.EditValue = "1";

                this.cmbTipoReferencia1Deudor.Properties.ValueMember = "Key";
                this.cmbTipoReferencia1Deudor.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia1Deudor.Properties.DataSource = referencia;
                this.cmbTipoReferencia1Deudor.EditValue = "1";

                this.cmbTipoReferencia2Deudor.Properties.ValueMember = "Key";
                this.cmbTipoReferencia2Deudor.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia2Deudor.Properties.DataSource = referencia;
                this.cmbTipoReferencia2Deudor.EditValue = "2";

                this.cmbTipoReferencia1Cod1.Properties.ValueMember = "Key";
                this.cmbTipoReferencia1Cod1.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia1Cod1.Properties.DataSource = referencia;
                this.cmbTipoReferencia1Cod1.EditValue = "1";

                this.cmbTipoReferencia2Cod1.Properties.ValueMember = "Key";
                this.cmbTipoReferencia2Cod1.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia2Cod1.Properties.DataSource = referencia;
                this.cmbTipoReferencia2Cod1.EditValue = "2";

                this.cmbTipoReferencia1Cod2.Properties.ValueMember = "Key";
                this.cmbTipoReferencia1Cod2.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia1Cod2.Properties.DataSource = referencia;
                this.cmbTipoReferencia1Cod2.EditValue = "1";

                this.cmbTipoReferencia2Cod2.Properties.ValueMember = "Key";
                this.cmbTipoReferencia2Cod2.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia2Cod2.Properties.DataSource = referencia;
                this.cmbTipoReferencia2Cod2.EditValue = "2";

                this.cmbTipoReferencia1Cod3.Properties.ValueMember = "Key";
                this.cmbTipoReferencia1Cod3.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia1Cod3.Properties.DataSource = referencia;
                this.cmbTipoReferencia1Cod3.EditValue = "1";

                this.cmbTipoReferencia2Cod3.Properties.ValueMember = "Key";
                this.cmbTipoReferencia2Cod3.Properties.DisplayMember = "Value";
                this.cmbTipoReferencia2Cod3.Properties.DataSource = referencia;
                this.cmbTipoReferencia2Cod3.EditValue = "2";

                this.cmbSexo.Properties.ValueMember = "Key";
                this.cmbSexo.Properties.DisplayMember = "Value";
                this.cmbSexo.Properties.DataSource = sexo;
                this.cmbSexo.EditValue = "1";

                this.cmbEstadoCivil.Properties.ValueMember = "Key";
                this.cmbEstadoCivil.Properties.DisplayMember = "Value";
                this.cmbEstadoCivil.Properties.DataSource = estadoCivil;
                this.cmbEstadoCivil.EditValue = "0";

                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;

                this.linkConyCodeudor1.Dock = DockStyle.Fill;
                this.linkConyCodeudor2.Dock = DockStyle.Fill;
                this.linkConyCodeudor3.Dock = DockStyle.Fill;

                this.linkInmuebleCodeudor1.Dock = DockStyle.Fill;
                this.linkInmuebleCodeudor2.Dock = DockStyle.Fill;
                this.linkInmuebleCodeudor3.Dock = DockStyle.Fill;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this._data = new DTO_DigitaSolicitudCredito();
            this._ctrl = new DTO_glDocumentoControl();
            this.masterEntidad.Value = string.Empty;
            this.masterBanco.Value = string.Empty;
            this.masterAsesor.Value = string.Empty;
            this.txtCredito.Text = string.Empty;
            this.txtPlazo.EditValue = 0;
            this.txtMontoSolicitud.EditValue = 0;
            this.txtMontoAprobado.EditValue = 0;
            this.txtCuentaNumero.Text = string.Empty;
            this.txtSucursal.Text = string.Empty;

            #region Deudor
            //Deudor (TipoPersona 1)
            this.txtCedulaDeudor.Text = string.Empty;

            #region Datos Personales
            this.txtPriApellidoDeudor.Text = string.Empty;
            this.txtSdoApellidoDeudor.Text = string.Empty;
            this.txtPriNombreDeudor.Text = string.Empty;
            this.txtSdoNombreDeudor.Text = string.Empty;
            this.dtFechaExpDeudor.DateTime = DateTime.Today.Date;
            this.masterCiudadExpDeudor.Value = string.Empty;
            this.masterTerceroDocTipoDeudor.Value = string.Empty;
            this.txtCedulaDeudor.Text = string.Empty;
            this.dtFechaNacDeudor.DateTime = DateTime.Today.Date;
            this.txtCelular1Deudor.Text = string.Empty;
            this.txtCelular2Deudor.Text = string.Empty;
            this.cmbEstadoCivil.EditValue = string.Empty;
            this.cmbSexo.EditValue = "1";
            this.txtCorreoDeudor.Text = string.Empty;
            this.txtDireccionDeudor.EditValue = string.Empty;
            this.txtBarrioResidenciaDeudor.Text = string.Empty;
            this.masterCiudadResDeudor.Value = string.Empty;
            this.txtAntResidenciaDeudor.EditValue = string.Empty;
            this.cmbTipoViviendaDeudor.EditValue = "1";
            this.txtTelResidenciaDeudor.Text = string.Empty;
            this.txtLugarTrabajoDeudor.Text = string.Empty;
            this.txtDirTrabajoDeudor.Text = string.Empty;
            this.txtBarrioTrabajoDeudor.Text = string.Empty;
            this.masterCiudadTrabajoDeudor.Value = string.Empty;
            this.txtTelTrabajoDeudor.Text = string.Empty;
            this.txtCargoDeudor.Text = string.Empty;
            this.txtAntTrabajoDeudor.EditValue = string.Empty;
            this.cmbTipoContratoDeudor.EditValue = "1";
            this.txtEpsDeudor.Text = string.Empty;
            this.txtCargoDeudor.Text = string.Empty;
            this.txtPersonasCargoDeudor.EditValue = string.Empty;
            #endregion

            #region Datos Conyugue
            this.txtConyNombreDeudor.Text = string.Empty;
            this.txtConyDocumentoDeudor.Text = string.Empty;
            this.txtConyActividadDeudor.Text = string.Empty;
            this.txtConyAntDeudor.EditValue = string.Empty;
            this.txtConyEmpresaDeudor.Text = string.Empty;
            this.txtConyDirResidenciaDeudor.Text = string.Empty;
            this.txtConyTelefonoDeudor.Text = string.Empty;
            this.txtConyCelularDeudor.Text = string.Empty;
            #endregion

            #region Referencias   
            this.cmbTipoReferencia1Deudor.EditValue = string.Empty;
            this.txtNombreReferencia1Deudor.Text = string.Empty;
            this.txtParentescoReferencia1Deudor.Text = string.Empty;
            this.txtDireccionReferencia1Deudor.Text = string.Empty;
            this.txtBarrioReferencia1Deudor.Text = string.Empty;
            this.txtTelefonoReferencia1Deudor.Text = string.Empty;
            this.txtCelularReferencia1Deudor.Text = string.Empty;

            this.cmbTipoReferencia2Deudor.EditValue = string.Empty;
            this.txtNombreReferencia2Deudor.Text = string.Empty;
            this.txtParentescoReferencia2Deudor.Text = string.Empty;
            this.txtDireccionReferencia2Deudor.Text = string.Empty;
            this.txtBarrioReferencia2Deudor.Text = string.Empty;
            this.txtTelefonoReferencia2Deudor.Text = string.Empty;
            this.txtCelularReferencia2Deudor.Text = string.Empty;
            #endregion

            #region Bienes Raices
            //this.txtBRDireccionDeudor.Text = deudor.BR_Direccion.Value;
            //this.txtBRValorDeudor.EditValue = deudor.BR_Valor.Value.ToString();
            //this.chkBRAfectacionDeudor.Checked = deudor.BR_AfectacionFamInd.Value.Value;
            //this.chkBRHipotecaDeudor.Checked = deudor.BR_HipotecaInd.Value.Value;
            //this.txtBRHipotecaNombreDeudor.Text = deudor.BR_HipotecaNombre.Value;
            #endregion

            #region Vehiculos
            //this.txtVEMarcaDeudor.Text = deudor.VE_Marca.Value;
            //this.txtVEClaseDeudor.Text = deudor.VE_Clase.Value;
            //this.txtVEModeloDeudor.EditValue = deudor.VE_Modelo.Value.ToString();
            //this.txtVEPlacaDeudor.Text = deudor.VE_Placa.Value;
            //this.chkVEPignoradoDeudor.Checked = deudor.VE_PignoradoInd.Value.Value;
            //this.txtVEPignoradoNombreDeudor.Text = deudor.VE_PignoradoNombre.Value;
            //this.txtVEValorDeudor.EditValue = deudor.VE_Valor.Value.ToString();
            #endregion

            #region informacion Financiera
            //this.txtActivos.EditValue = deudor.VlrActivos.Value.ToString();
            //this.txtPasivos.EditValue = deudor.VlrPasivos.Value.ToString();
            //this.txtPatrimonio.EditValue = deudor.VlrPatrimonio.Value.ToString();
            //this.txtEgresos.EditValue = deudor.VlrEgresosMes.Value.ToString();
            //this.txtIngresos.EditValue = deudor.VlrIngresosMes.Value.ToString();
            //this.txtVlrIngresosNoOpe.EditValue = deudor.VlrIngresosNoOpe.Value.ToString();
            //this.txtDescrOtrosIng.Text = deudor.DescrOtrosIng.Value;
            //this.txtDescrOtrosBinenes.Text = deudor.DescrOtrosBinenes.Value;
            #endregion

            #region Creditos
            //this.txtEntCredito1.Text = deudor.EntCredito1.ToString();
            //this.txtPlazoCredito1.EditValue = deudor.Plazo1.Value.ToString();
            //this.txtSaldoCredito1.EditValue = deudor.Saldo1.Value.ToString();
            //this.chkSolicitudInd1.Checked = deudor.SolicitudInd1.Value.Value;
            //this.txtDeclFondos.Text = deudor.DeclFondos.Value;
            #endregion
          
            #endregion
            //Codeudor1 (TipoPersona 2)
            #region codeudor1
                //#region Datos Personales
                //this.txtCedulaCod1.Text = codeudor1.TerceroID.Value;
                //this.txtPriApellidoCod1.Text = codeudor1.ApellidoPri.Value;
                //this.txtSdoApellidoCod1.Text = codeudor1.ApellidoSdo.Value;
                //this.txtPriNombreCod1.Text = codeudor1.NombrePri.Value;
                //this.txtSdoNombreCod1.Text = codeudor1.NombreSdo.Value;
                //this.dtFechaExpCod1.DateTime = codeudor1.FechaExpDoc.Value.HasValue ? codeudor1.FechaExpDoc.Value.Value : DateTime.Today;
                //this.masterTerceroDocTipoCod1.Value = codeudor1.TerceroDocTipoID.Value;
                //this.dtFechaNacCod1.DateTime = codeudor1.FechaNacimiento.Value.HasValue ? codeudor1.FechaNacimiento.Value.Value : DateTime.Today;
                //this.txtCelular1Cod1.Text = codeudor1.Celular1.Value;
                //this.txtCelular2Cod1.Text = codeudor1.Celular2.Value;
                //this.txtCorreoCod1.Text = codeudor1.CorreoElectronico.Value;
                //this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                //this.txtBarrioResidenciaCod1.Text = codeudor1.BarrioResidencia.Value;
                //this.masterCiudadResCod1.Value = codeudor1.CiudadResidencia.Value;
                //this.txtAntResidenciaCod1.EditValue = codeudor1.AntResidencia.Value.ToString();
                //this.cmbTipoViviendaCod1.EditValue = codeudor1.TipoVivienda.Value.ToString();
                //this.txtTelResidenciaCod1.Text = codeudor1.TelResidencia.Value;
                //this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                //this.txtLugarTrabajoCod1.Text = codeudor1.LugarTrabajo.Value;
                //this.txtDirTrabajoCod1.Text = codeudor1.DirTrabajo.Value;
                //this.txtBarrioTrabajoCod1.Text = codeudor1.BarrioTrabajo.Value;
                //this.masterCiudadTrabajoCod1.Value = codeudor1.CiudadTrabajo.Value;
                //this.txtTelTrabajoCod1.Text = codeudor1.TelTrabajo.Value;
                //this.txtCargoCod1.Text = codeudor1.Cargo.Value;
                //this.txtAntTrabajoCod1.EditValue = codeudor1.AntTrabajo.Value.ToString();
                //this.cmbTipoContratoCod1.EditValue = codeudor1.TipoContrato.Value.ToString();
                //this.txtEpsCod1.Text = codeudor1.EPS.Value;
                //this.txtPersonasCargoCod1.EditValue = codeudor1.Personascargo.Value.ToString();

                //if (!string.IsNullOrEmpty(codeudor1.TerceroID.Value))
                //    this.linkCodeudor1.Dock = DockStyle.None;
                //#endregion

                //#region Datos Conyugue
                //this.txtConyNombreCod1.Text = codeudor1.NombreConyugue.Value;
                //this.txtConyDocumentoCod1.Text = codeudor1.Conyugue.Value;
                //this.txtConyActividadCod1.Text = codeudor1.ActConyugue.Value;
                //this.txtConyAntCod1.Text = codeudor1.AntConyugue.Value.ToString();
                //this.txtConyEmpresaCod1.Text = codeudor1.EmpresaConyugue.Value.ToString();
                //this.txtConyDirResidenciaCod1.Text = codeudor1.DirResConyugue.Value;
                //this.txtConyTelefonoCod1.Text = codeudor1.TelefonoConyugue.Value;
                //this.txtConyCelularCod1.Text = codeudor1.CelularConyugue.Value;
                //#endregion

                //#region Referencias
                //this.cmbTipoReferencia1Cod1.EditValue = codeudor1.TipoReferencia1.Value.ToString();
                //this.txtNombreReferencia1Cod1.Text = codeudor1.NombreReferencia1.Value;
                //this.txtParentescoReferencia1Cod1.Text = codeudor1.RelReferencia1.Value;
                //this.txtDireccionReferencia1Cod1.Text = codeudor1.DirReferencia1.Value;
                //this.txtBarrioReferencia1Cod1.Text = codeudor1.BarrioReferencia1.Value;
                //this.txtTelefonoReferencia1Cod1.Text = codeudor1.TelefonoReferencia1.Value;
                //this.txtCelularReferencia1Cod1.Text = codeudor1.CelularReferencia1.Value;

                //this.cmbTipoReferencia2Cod1.EditValue = codeudor1.TipoReferencia2.Value.ToString();
                //this.txtNombreReferencia2Cod1.Text = codeudor1.NombreReferencia2.Value;
                //this.txtParentescoReferencia2Cod1.Text = codeudor1.RelReferencia2.Value;
                //this.txtDireccionReferencia2Cod1.Text = codeudor1.DirReferencia2.Value;
                //this.txtBarrioReferencia2Cod1.Text = codeudor1.BarrioReferencia2.Value;
                //this.txtTelefonoReferencia2Cod1.Text = codeudor1.TelefonoReferencia2.Value;
                //this.txtCelularReferencia2Cod1.Text = codeudor1.CelularReferencia2.Value;
                //#endregion

                //#region Bienes Raices
                //this.txtBRDireccionCod1.Text = codeudor1.BR_Direccion.Value;
                //this.txtBRValorCod1.EditValue = codeudor1.BR_Valor.Value.ToString();
                //this.chkBRAfectacionCod1.Checked = codeudor1.BR_AfectacionFamInd.Value.Value;
                //this.chkBRHipotecaCod1.Checked = codeudor1.BR_HipotecaInd.Value.Value;
                //this.txtBRHipotecaNombreCod1.Text = codeudor1.BR_HipotecaNombre.Value;
                //#endregion

                //#region Vehiculos
                //this.txtVEMarcaCod1.Text = codeudor1.VE_Marca.Value;
                //this.txtVEClaseCod1.Text = codeudor1.VE_Clase.Value;
                //this.txtVEModeloCod1.EditValue = codeudor1.VE_Modelo.Value.ToString();
                //this.txtVEPlacaCod1.Text = codeudor1.VE_Placa.Value;
                //this.chkVEPignoradoCod1.Checked = codeudor1.VE_PignoradoInd.Value.Value;
                //this.txtVEPignoradoNombreCod1.Text = codeudor1.VE_PignoradoNombre.Value;
                //this.txtVEValorCod1.EditValue = codeudor1.VE_Valor.Value.ToString();
                //#endregion
            #endregion
            //Codeudor1 (TipoPersona 3)
            #region codeudor2
                //#region Datos Personales
                //this.txtCedulaCod2.Text = codeudor2.TerceroID.Value;
                //this.txtPriApellidoCod2.Text = codeudor2.ApellidoPri.Value;
                //this.txtSdoApellidoCod2.Text = codeudor2.ApellidoSdo.Value;
                //this.txtPriNombreCod2.Text = codeudor2.NombrePri.Value;
                //this.txtSdoNombreCod2.Text = codeudor2.NombreSdo.Value;
                //this.dtFechaExpCod2.DateTime = codeudor2.FechaExpDoc.Value.HasValue ? codeudor2.FechaExpDoc.Value.Value : DateTime.Now;
                //this.masterTerceroDocTipoCod2.Value = codeudor2.TerceroDocTipoID.Value;
                //this.dtFechaNacCod2.DateTime = codeudor2.FechaNacimiento.Value.HasValue ? codeudor2.FechaNacimiento.Value.Value : DateTime.Now;
                //this.txtCelular1Cod2.Text = codeudor2.Celular1.Value;
                //this.txtCelular2Cod2.Text = codeudor2.Celular2.Value;
                //this.txtCorreoCod2.Text = codeudor2.CorreoElectronico.Value;
                //this.txtBarrioResidenciaCod2.Text = codeudor2.BarrioResidencia.Value;
                //this.masterCiudadResCod2.Value = codeudor2.CiudadResidencia.Value;
                //this.txtAntResidenciaCod2.EditValue = codeudor2.AntResidencia.Value.ToString();
                //this.cmbTipoViviendaCod2.EditValue = codeudor2.TipoVivienda.Value.ToString();
                //this.txtTelResidenciaCod2.Text = codeudor2.TelResidencia.Value;
                //this.txtDireccionCod2.EditValue = codeudor2.DirResidencia.Value;
                //this.txtLugarTrabajoCod2.Text = codeudor2.LugarTrabajo.Value;
                //this.txtDirTrabajoCod2.Text = codeudor2.DirTrabajo.Value;
                //this.txtBarrioTrabajoCod2.Text = codeudor2.BarrioTrabajo.Value;
                //this.masterCiudadTrabajoCod2.Value = codeudor2.CiudadTrabajo.Value;
                //this.txtTelTrabajoCod2.Text = codeudor2.TelTrabajo.Value;
                //this.txtCargoCod2.Text = codeudor2.Cargo.Value;
                //this.txtAntTrabajoCod2.EditValue = codeudor2.AntTrabajo.Value.ToString();
                //this.cmbTipoContratoCod2.EditValue = codeudor2.TipoContrato.Value.ToString();
                //this.txtEpsCod2.Text = codeudor2.EPS.Value;
                //this.txtPersonasCargoCod2.Text = codeudor2.Personascargo.Value.ToString();

                //if (!string.IsNullOrEmpty(codeudor2.TerceroID.Value))
                //    this.linkCodeudor2.Dock = DockStyle.None;
                //#endregion

                //#region Datos Conyugue
                //this.txtConyNombreCod2.Text = codeudor2.NombreConyugue.Value;
                //this.txtConyDocumentoCod2.Text = codeudor2.Conyugue.Value;
                //this.txtConyActividadCod2.Text = codeudor2.ActConyugue.Value;
                //this.txtConyAntCod2.EditValue = codeudor2.AntConyugue.Value.ToString();
                //this.txtConyEmpresaCod2.Text = codeudor2.EmpresaConyugue.Value.ToString();
                //this.txtConyDirResidenciaCod2.Text = codeudor2.DirResConyugue.Value;
                //this.txtConyTelefonoCod2.Text = codeudor2.TelefonoConyugue.Value;
                //this.txtConyCelularCod2.Text = codeudor2.CelularConyugue.Value;
                //#endregion

                //#region Referencias
                //this.cmbTipoReferencia1Cod2.EditValue = codeudor2.TipoReferencia1.Value.ToString();
                //this.txtNombreReferencia1Cod2.Text = codeudor2.NombreReferencia1.Value;
                //this.txtParentescoReferencia1Cod2.Text = codeudor2.RelReferencia1.Value;
                //this.txtDireccionReferencia1Cod2.Text = codeudor2.DirReferencia1.Value;
                //this.txtBarrioReferencia1Cod2.Text = codeudor2.BarrioReferencia1.Value;
                //this.txtTelefonoReferencia1Cod2.Text = codeudor2.TelefonoReferencia1.Value;
                //this.txtCelularReferencia1Cod2.Text = codeudor2.CelularReferencia1.Value;

                //this.cmbTipoReferencia2Cod2.EditValue = codeudor2.TipoReferencia2.Value.ToString();
                //this.txtNombreReferencia2Cod2.Text = codeudor2.NombreReferencia2.Value;
                //this.txtParentescoReferencia2Cod2.Text = codeudor2.RelReferencia2.Value;
                //this.txtDireccionReferencia2Cod2.Text = codeudor2.DirReferencia2.Value;
                //this.txtBarrioReferencia2Cod2.Text = codeudor2.BarrioReferencia2.Value;
                //this.txtTelefonoReferencia2Cod2.Text = codeudor2.TelefonoReferencia2.Value;
                //this.txtCelularReferencia2Cod2.Text = codeudor2.CelularReferencia2.Value;
                //#endregion

                //#region Bienes Raices
                //this.txtBRDireccionCod2.Text = codeudor2.BR_Direccion.Value;
                //this.txtBRValorCod2.EditValue = codeudor2.BR_Valor.Value.ToString();
                //this.chkBRAfectacionCod2.Checked = codeudor2.BR_AfectacionFamInd.Value.Value;
                //this.chkBRHipotecaCod2.Checked = codeudor2.BR_HipotecaInd.Value.Value;
                //this.txtBRHipotecaNombreCod2.Text = codeudor2.BR_HipotecaNombre.Value;
                //#endregion

                //#region Vehiculos
                //this.txtVEMarcaCod2.Text = codeudor2.VE_Marca.Value;
                //this.txtVEClaseCod2.Text = codeudor2.VE_Clase.Value;
                //this.txtVEModeloCod2.EditValue = codeudor2.VE_Modelo.Value.ToString();
                //this.txtVEPlacaCod2.Text = codeudor2.VE_Placa.Value;
                //this.chkVEPignoradoCod2.Checked = codeudor2.VE_PignoradoInd.Value.Value;
                //this.txtVEPignoradoNombreCod2.Text = codeudor2.VE_PignoradoNombre.Value;
                //this.txtVEValorCod2.EditValue = codeudor2.VE_Valor.Value.ToString();
                //#endregion
            #endregion
            //Codeudor1 (TipoPersona 4)
            #region codeudor3

                //#region Datos Personales
                //this.txtCedulaCod3.Text = codeudor3.TerceroID.Value;
                //this.txtPriApellidoCod3.Text = codeudor3.ApellidoPri.Value;
                //this.txtSdoApellidoCod3.Text = codeudor3.ApellidoSdo.Value;
                //this.txtPriNombreCod3.Text = codeudor3.NombrePri.Value;
                //this.txtSdoNombreCod3.Text = codeudor3.NombreSdo.Value;
                //this.dtFechaExpCod3.DateTime = codeudor3.FechaExpDoc.Value.HasValue ? codeudor3.FechaExpDoc.Value.Value : DateTime.Now;
                //this.masterTerceroDocTipoCod3.Value = codeudor3.TerceroDocTipoID.Value;
                //this.dtFechaNacCod3.DateTime = codeudor3.FechaNacimiento.Value.HasValue ? codeudor3.FechaNacimiento.Value.Value : DateTime.Now;
                //this.txtCelular1Cod3.Text = codeudor3.Celular1.Value;
                //this.txtCelular2Cod3.Text = codeudor3.Celular2.Value;
                //this.txtCorreoCod3.Text = codeudor3.CorreoElectronico.Value;
                //this.txtBarrioResidenciaCod3.Text = codeudor3.BarrioResidencia.Value;
                //this.masterCiudadResCod3.Value = codeudor3.CiudadResidencia.Value;
                //this.txtAntResidenciaCod3.EditValue = codeudor3.AntResidencia.Value.ToString();
                //this.cmbTipoViviendaCod3.EditValue = codeudor3.TipoVivienda.Value.ToString();
                //this.txtTelResidenciaCod3.Text = codeudor3.TelResidencia.Value;
                //this.txtDireccionCod3.EditValue = codeudor3.DirResidencia.Value;
                //this.txtLugarTrabajoCod3.Text = codeudor3.LugarTrabajo.Value;
                //this.txtDirTrabajoCod3.Text = codeudor3.DirTrabajo.Value;
                //this.txtBarrioTrabajoCod3.Text = codeudor3.BarrioTrabajo.Value;
                //this.masterCiudadTrabajoCod3.Value = codeudor3.CiudadTrabajo.Value;
                //this.txtTelTrabajoCod3.Text = codeudor3.TelTrabajo.Value;
                //this.txtCargoCod3.Text = codeudor3.Cargo.Value;
                //this.txtAntTrabajoCod3.EditValue = codeudor3.AntTrabajo.Value.ToString();
                //this.cmbTipoContratoCod3.EditValue = codeudor3.TipoContrato.Value.ToString();
                //this.txtEpsCod3.Text = codeudor3.EPS.Value;
                //this.txtPersonasCargoCod3.EditValue = codeudor3.Personascargo.Value.ToString();

                //if (!string.IsNullOrEmpty(codeudor3.TerceroID.Value))
                //    this.linkCodeudor3.Dock = DockStyle.None;
                //#endregion

                //#region Datos Conyugue
                //this.txtConyNombreCod3.Text = codeudor3.NombreConyugue.Value;
                //this.txtConyDocumentoCod3.Text = codeudor3.Conyugue.Value;
                //this.txtConyActividadCod3.Text = codeudor3.ActConyugue.Value;
                //this.txtConyAntCod3.Text = codeudor3.AntConyugue.Value.ToString();
                //this.txtConyEmpresaCod3.Text = codeudor3.EmpresaConyugue.Value.ToString();
                //this.txtConyDirResidenciaCod3.Text = codeudor3.DirResConyugue.Value;
                //this.txtConyTelefonoCod3.Text = codeudor3.TelefonoConyugue.Value;
                //this.txtConyCelularCod3.Text = codeudor3.CelularConyugue.Value;
                //#endregion

                //#region Referencias
                //this.cmbTipoReferencia1Cod3.EditValue = codeudor3.TipoReferencia1.Value.ToString();
                //this.txtNombreReferencia1Cod3.Text = codeudor3.NombreReferencia1.Value;
                //this.txtParentescoReferencia1Cod3.Text = codeudor3.RelReferencia1.Value;
                //this.txtDireccionReferencia1Cod3.Text = codeudor3.DirReferencia1.Value;
                //this.txtBarrioReferencia1Cod3.Text = codeudor3.BarrioReferencia1.Value;
                //this.txtTelefonoReferencia1Cod3.Text = codeudor3.TelefonoReferencia1.Value;
                //this.txtCelularReferencia1Cod3.Text = codeudor3.CelularReferencia1.Value;

                //this.cmbTipoReferencia2Cod3.EditValue = codeudor3.TipoReferencia2.Value.ToString();
                //this.txtNombreReferencia2Cod3.Text = codeudor3.NombreReferencia2.Value;
                //this.txtParentescoReferencia2Cod3.Text = codeudor3.RelReferencia2.Value;
                //this.txtDireccionReferencia2Cod3.Text = codeudor3.DirReferencia2.Value;
                //this.txtBarrioReferencia2Cod3.Text = codeudor3.BarrioReferencia2.Value;
                //this.txtTelefonoReferencia2Cod3.Text = codeudor3.TelefonoReferencia2.Value;
                //this.txtCelularReferencia2Cod3.Text = codeudor3.CelularReferencia2.Value;
                //#endregion

                //#region Bienes Raices
                //this.txtBRDireccionCod3.Text = codeudor3.BR_Direccion.Value;
                //this.txtBRValorCod3.EditValue = codeudor3.BR_Valor.Value.ToString();
                //this.chkBRAfectacionCod3.Checked = codeudor3.BR_AfectacionFamInd.Value.Value;
                //this.chkBRHipotecaCod3.Checked = codeudor3.BR_HipotecaInd.Value.Value;
                //this.txtBRHipotecaNombreCod3.Text = codeudor3.BR_HipotecaNombre.Value;
                //#endregion

                //#region Vehiculos
                //this.txtVEMarcaCod3.Text = codeudor3.VE_Marca.Value;
                //this.txtVEClaseCod3.Text = codeudor3.VE_Clase.Value;
                //this.txtVEModeloCod3.EditValue = codeudor3.VE_Modelo.Value.ToString();
                //this.txtVEPlacaCod3.Text = codeudor3.VE_Placa.Value;
                //this.chkVEPignoradoCod3.Checked = codeudor3.VE_PignoradoInd.Value.Value;
                //this.txtVEPignoradoNombreCod3.Text = codeudor3.VE_PignoradoNombre.Value;
                //this.txtVEValorCod3.EditValue = codeudor3.VE_Valor.Value.ToString();
                //#endregion
            #endregion

            FormProvider.Master.itemSave.Enabled = false;
            FormProvider.Master.itemSendtoAppr.Enabled = false;

            linkConyCodeudor1.Dock = DockStyle.Fill;
            linkConyCodeudor2.Dock = DockStyle.Fill;
            linkConyCodeudor3.Dock = DockStyle.Fill;
            linkCodeudor1.Dock = DockStyle.Fill;
            linkCodeudor2.Dock = DockStyle.Fill;
            linkCodeudor3.Dock = DockStyle.Fill;
            linkInmuebleCodeudor1.Dock = DockStyle.Fill;
            linkInmuebleCodeudor2.Dock = DockStyle.Fill;
            linkInmuebleCodeudor3.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            
            this.txtPriApellidoDeudor.ReadOnly = !enabled;
            this.txtSdoApellidoDeudor.ReadOnly = !enabled;
            this.txtPriNombreDeudor.ReadOnly = !enabled;
            this.txtSdoNombreDeudor.ReadOnly = !enabled;
            this.dtFechaExpDeudor.ReadOnly = !enabled;
            
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            string result = string.Empty;
            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            #region Hace las Validaciones
            #region Datos Deudor
            if (string.IsNullOrEmpty(this.txtCedulaDeudor.Text))
            {
                result += string.Format(msgVacio,this.lblNroDocumento.Text + " Deudor: ") + "\n";
                this.txtCedulaDeudor.Focus();
            }
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
                result += string.Format(msgVacio, this.lblFechaExp.Text + " Deudor: ") + "\n";
                this.dtFechaExpDeudor.Focus();
            }
            if (!this.masterCiudadExpDeudor.ValidID)
            {
                result += string.Format(msgVacio, this.lblFechaExp.Text + " Deudor: ") + "\n";
                this.masterCiudadExpDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.dtFechaNacDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblFechaNac.Text + " Deudor: ") + "\n";
                this.dtFechaNacDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtCelular1Deudor.Text))
            {
                result += string.Format(msgVacio,this.lblCelular1.Text + " Deudor: ") + "\n";
                this.txtCelular1Deudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtCorreoDeudor.Text))
            {
                result += string.Format(msgVacio, this.lblEmail.Text + " Deudor: ") + "\n";
                this.txtCorreoDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtDireccionDeudor.EditValue.ToString()))
            {
                result += string.Format(msgVacio,this.lblDirResidencia.Text + " Deudor: ") + "\n";
                this.txtDireccionDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.masterCiudadResDeudor.Value))
            {
                result += string.Format(msgVacio, this.lblCiudadRes.Text + " Deudor: ") + "\n";
                this.masterCiudadResDeudor.Focus();
            }
            if (string.IsNullOrEmpty(this.txtTelResidenciaDeudor.Text))
            {
                result += string.Format(msgVacio,this.lblTelResid.Text + " Deudor:") + "\n";
                this.txtTelResidenciaDeudor.Focus();
            }
            if (!string.IsNullOrEmpty(this.masterCiudadResDeudor.Value) && !this.masterCiudadResDeudor.ValidID)
            {
                result += string.Format("La ciudad residencia del Deudor no es válida, verifique") + "\n";
                this.masterCiudadResDeudor.Focus();
            }
            if (!string.IsNullOrEmpty(this.masterTerceroDocTipoDeudor.Value) && !this.masterTerceroDocTipoDeudor.ValidID)
            {
                result += string.Format("El Tipo de documento del Deudor no es válido, verifique") + "\n";
                this.masterTerceroDocTipoDeudor.Focus();
            }
            #endregion

            if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
            {
                #region Datos CoDeudor 1        
                if (String.IsNullOrEmpty(this.txtCedulaCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 1: ") + "\n";
                    this.txtCedulaCod1.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriApellidoCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 1: ") + "\n";
                    this.txtPriApellidoCod1.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriNombreCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 1: ") + "\n";
                    this.txtPriNombreCod1.Focus();
                }
                if (!this.masterTerceroDocTipoCod1.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 1: ") + "\n";
                    this.masterTerceroDocTipoCod1.Focus();
                }
                
                if (string.IsNullOrEmpty(this.dtFechaExpCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 1:") + "\n";
                    this.dtFechaExpCod1.Focus();
                }
                if (!this.masterCiudadExpCod1.ValidID)
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 1: ") + "\n";
                    this.masterCiudadExpCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 1: ") + "\n";
                    this.dtFechaNacCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelular1Cod1.Text))
                {
                    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 1: ") + "\n";
                    this.txtCelular1Cod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCorreoCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 1: ") + "\n";
                    this.txtCorreoCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtDireccionCod1.EditValue.ToString()))
                {
                    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 1: ") + "\n";
                    this.txtDireccionCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.masterCiudadResCod1.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 1: ") + "\n";
                    this.masterCiudadResCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtTelResidenciaCod1.Text))
                {
                    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 1:") + "\n";
                    this.txtTelResidenciaCod1.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterCiudadResCod1.Value) && !this.masterCiudadResCod1.ValidID)
                {
                    result += string.Format("La ciudad residencia del Codeudor 1 no es válida, verifique") + "\n";
                    this.masterCiudadResCod1.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod1.Value) && !this.masterTerceroDocTipoCod1.ValidID)
                {
                    result += string.Format("El Tipo de documento del Codeudor 1 no es válido, verifique") + "\n";
                    this.masterTerceroDocTipoCod1.Focus();
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
            {
                #region Datos CoDeudor 2  
                if (String.IsNullOrEmpty(this.txtCedulaCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 2: ") + "\n";
                    this.txtCedulaCod2.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriApellidoCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 2: ") + "\n";
                    this.txtPriApellidoCod2.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriNombreCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 2: ") + "\n";
                    this.txtPriNombreCod2.Focus();
                }
                if (!this.masterTerceroDocTipoCod2.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 2: ") + "\n";
                    this.masterTerceroDocTipoCod2.Focus();
                }             
                if (string.IsNullOrEmpty(this.dtFechaExpCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 2:") + "\n";
                    this.dtFechaExpCod2.Focus();
                }
                if (!this.masterCiudadExpCod2.ValidID)
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 2: ") + "\n";
                    this.masterCiudadExpCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 2: ") + "\n";
                    this.dtFechaNacCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelular1Cod2.Text))
                {
                    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 2: ") + "\n";
                    this.txtCelular1Cod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCorreoCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 2: ") + "\n";
                    this.txtCorreoCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtDireccionCod2.EditValue.ToString()))
                {
                    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 2: ") + "\n";
                    this.txtDireccionCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.masterCiudadResCod2.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 2: ") + "\n";
                    this.masterCiudadResCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtTelResidenciaCod2.Text))
                {
                    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 2:") + "\n";
                    this.txtTelResidenciaCod2.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterCiudadResCod2.Value) && !this.masterCiudadResCod2.ValidID)
                {
                    result += string.Format("La ciudad residencia del Codeudor 2 no es válida, verifique") + "\n";
                    this.masterCiudadResCod2.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod2.Value) && !this.masterTerceroDocTipoCod2.ValidID)
                {
                    result += string.Format("El Tipo de documento del Codeudor 2 no es válido, verifique") + "\n";
                    this.masterTerceroDocTipoCod2.Focus();
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
            {
                #region Datos CoDeudor 3     
                if (String.IsNullOrEmpty(this.txtCedulaCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 3: ") + "\n";
                    this.txtCedulaCod3.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriApellidoCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 3: ") + "\n";
                    this.txtPriApellidoCod3.Focus();
                }
                if (String.IsNullOrEmpty(this.txtPriNombreCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 3: ") + "\n";
                    this.txtPriNombreCod3.Focus();
                }
                if (!this.masterTerceroDocTipoCod3.ValidID)
                {
                    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 3: ") + "\n";
                    this.masterTerceroDocTipoCod3.Focus();
                }  
                if (string.IsNullOrEmpty(this.dtFechaExpCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 3:") + "\n";
                    this.dtFechaExpCod3.Focus();
                }
                if (!this.masterCiudadExpCod3.ValidID)
                {
                    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 3: ") + "\n";
                    this.masterCiudadExpCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.dtFechaNacCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 3: ") + "\n";
                    this.dtFechaNacCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelular1Cod3.Text))
                {
                    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 3: ") + "\n";
                    this.txtCelular1Cod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCorreoCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 3: ") + "\n";
                    this.txtCorreoCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtDireccionCod3.EditValue.ToString()))
                {
                    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 3: ") + "\n";
                    this.txtDireccionCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.masterCiudadResCod3.Value))
                {
                    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 3: ") + "\n";
                    this.masterCiudadResCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtTelResidenciaCod3.Text))
                {
                    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 3: ") + "\n";
                    this.txtTelResidenciaCod3.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterCiudadResCod3.Value) && !this.masterCiudadResCod3.ValidID)
                {
                    result += string.Format("La ciudad residencia del Codeudor 3 no es válida, verifique") + "\n";
                    this.masterCiudadResCod3.Focus();
                }
                if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod3.Value) && !this.masterTerceroDocTipoCod3.ValidID)
                {
                    result += string.Format("El Tipo de documento del Codeudor 3 no es válido, verifique") + "\n";
                    this.masterTerceroDocTipoCod3.Focus();
                }
                #endregion
            }

            if (string.IsNullOrEmpty(result))
                return true;
            else
            {
                MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
                return false;
            }
            #endregion
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
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            }
            #endregion           

            return true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void AssignValues(bool isGet)
        {
            try
            {
                
                DTO_ccSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);                
                DTO_ccSolicitudDatosPersonales codeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_ccSolicitudDatosPersonales codeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
                DTO_ccSolicitudDatosPersonales codeudor3 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);

                if (isGet)
                {
                    #region Asigna datos a los controles
                    #region Asigna datos solicitud
                    this.masterEntidad.Value = this._data.SolicituDocu.PagaduriaID.Value;
                    this.txtCredito.EditValue = this._data.SolicituDocu.Libranza.Value;
                    this.dtFechaSolicitud.DateTime = this._data.DocCtrl.FechaDoc.Value.Value;
                    this.txtMontoSolicitud.EditValue = this._data.SolicituDocu.VlrPreSolicitado.Value.HasValue ? this._data.SolicituDocu.VlrPreSolicitado.Value : this._data.SolicituDocu.VlrSolicitado.Value;
                    this.txtPlazo.EditValue = this._data.SolicituDocu.Plazo.Value;
                    this.cmbPeriodicidad.EditValue = this._data.SolicituDocu.PeriodoPago.Value.HasValue ? this._data.SolicituDocu.PeriodoPago.Value.ToString() : "1";
                    this.txtMontoAprobado.EditValue = this._data.SolicituDocu.VlrSolicitado.Value;
                    this.masterBanco.Value = this._data.SolicituDocu.BancoID_1.Value;
                    this.txtCuentaNumero.Text = this._data.SolicituDocu.BcoCtaNro_1.Value;
                    this.cmbCuentatipo.EditValue = this._data.SolicituDocu.CuentaTipo_1.Value.HasValue ? this._data.SolicituDocu.CuentaTipo_1.Value.ToString() : "3";
                    this.txtSucursal.Text = this._data.SolicituDocu.DatoAdd5.Value;
                    this.masterAsesor.Value = this._data.SolicituDocu.AsesorID.Value;
                    this.lblVersion.Text = "Versión: " + (this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value.ToString() : "1");

                    #endregion
                    #region Deudor
                    //Deudor (TipoPersona 1)
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteRadica.Value.ToString();
                    
                    if (deudor != null)
                    {
                        #region Datos Personales
                        this.txtPriApellidoDeudor.Text = deudor.ApellidoPri.Value;
                        this.txtSdoApellidoDeudor.Text = deudor.ApellidoSdo.Value;
                        this.txtPriNombreDeudor.Text = deudor.NombrePri.Value;
                        this.txtSdoNombreDeudor.Text = deudor.NombreSdo.Value;
                        this.dtFechaExpDeudor.DateTime = deudor.FechaExpDoc.Value.HasValue ? deudor.FechaExpDoc.Value.Value : DateTime.Today.Date;
                        this.masterCiudadExpDeudor.Value = deudor.CiudadExpDoc.Value;
                        this.masterTerceroDocTipoDeudor.Value = deudor.TerceroDocTipoID.Value;
                        this.txtCedulaDeudor.Text = deudor.TerceroID.Value;
                        this.dtFechaNacDeudor.DateTime = deudor.FechaNacimiento.Value.HasValue ? deudor.FechaNacimiento.Value.Value : DateTime.Today.Date;
                        this.txtCelular1Deudor.Text = deudor.Celular1.Value;
                        this.txtCelular2Deudor.Text = deudor.Celular2.Value;
                        this.cmbEstadoCivil.EditValue = deudor.EstadoCivil.Value.HasValue? deudor.EstadoCivil.Value.ToString() : "0";
                        this.cmbSexo.EditValue = deudor.Sexo.Value.HasValue? deudor.Sexo.Value.ToString() : "1";
                        this.txtCorreoDeudor.Text = deudor.CorreoElectronico.Value;
                        this.txtDireccionDeudor.EditValue = deudor.DirResidencia.Value;
                        this.txtBarrioResidenciaDeudor.Text = deudor.BarrioResidencia.Value;
                        this.masterCiudadResDeudor.Value= deudor.CiudadResidencia.Value;
                        this.txtAntResidenciaDeudor.EditValue = deudor.AntResidencia.Value.ToString();
                        this.cmbTipoViviendaDeudor.EditValue= deudor.TipoVivienda.Value.ToString();
                        this.txtTelResidenciaDeudor.Text = deudor.TelResidencia.Value;
                        this.txtLugarTrabajoDeudor.Text = deudor.LugarTrabajo.Value;
                        this.txtDirTrabajoDeudor.Text = deudor.DirTrabajo.Value;
                        this.txtBarrioTrabajoDeudor.Text = deudor.BarrioTrabajo.Value;
                        this.masterCiudadTrabajoDeudor.Value = deudor.CiudadTrabajo.Value;
                        this.txtTelTrabajoDeudor.Text = deudor.TelTrabajo.Value;
                        this.txtCargoDeudor.Text = deudor.Cargo.Value;
                        this.txtAntTrabajoDeudor.EditValue= deudor.AntTrabajo.Value.ToString();
                        this.cmbTipoContratoDeudor.EditValue= deudor.TipoContrato.Value.ToString();
                        this.txtEpsDeudor.Text= deudor.EPS.Value;
                        this.txtCargoDeudor.Text = deudor.Cargo.Value;
                        this.txtPersonasCargoDeudor.EditValue = deudor.Personascargo.Value.ToString();
                        #endregion

                        #region Datos Conyugue
                        this.txtConyNombreDeudor.Text = deudor.NombreConyugue.Value;
                        this.txtConyDocumentoDeudor.Text = deudor.Conyugue.Value;
                        this.txtConyActividadDeudor.Text = deudor.ActConyugue.Value;
                        this.txtConyAntDeudor.EditValue = deudor.AntConyugue.Value.HasValue? deudor.AntConyugue.Value.ToString() : "0";
                        this.txtConyEmpresaDeudor.Text = deudor.EmpresaConyugue.Value.ToString();
                        this.txtConyDirResidenciaDeudor.Text = deudor.DirResConyugue.Value;
                        this.txtConyTelefonoDeudor.Text = deudor.TelefonoConyugue.Value;
                        this.txtConyCelularDeudor.Text = deudor.CelularConyugue.Value;
                        #endregion

                        #region Referencias   
                        this.cmbTipoReferencia1Deudor.EditValue= deudor.TipoReferencia1.Value.HasValue? deudor.TipoReferencia1.Value.ToString() : "1";
                        this.txtNombreReferencia1Deudor.Text = deudor.NombreReferencia1.Value;
                        this.txtParentescoReferencia1Deudor.Text = deudor.RelReferencia1.Value;
                        this.txtDireccionReferencia1Deudor.Text = deudor.DirReferencia1.Value;
                        this.txtBarrioReferencia1Deudor.Text = deudor.BarrioReferencia1.Value;
                        this.txtTelefonoReferencia1Deudor.Text = deudor.TelefonoReferencia1.Value;
                        this.txtCelularReferencia1Deudor.Text = deudor.CelularReferencia1.Value;

                        this.cmbTipoReferencia2Deudor.EditValue = deudor.TipoReferencia2.Value.HasValue? deudor.TipoReferencia2.Value.ToString() : "2";
                        this.txtNombreReferencia2Deudor.Text = deudor.NombreReferencia2.Value;
                        this.txtParentescoReferencia2Deudor.Text = deudor.RelReferencia2.Value;
                        this.txtDireccionReferencia2Deudor.Text = deudor.DirReferencia2.Value;
                        this.txtBarrioReferencia2Deudor.Text = deudor.BarrioReferencia2.Value;
                        this.txtTelefonoReferencia2Deudor.Text = deudor.TelefonoReferencia2.Value;
                        this.txtCelularReferencia2Deudor.Text = deudor.CelularReferencia2.Value;
                        #endregion

                        #region Bienes Raices
                        this.txtBRDireccionDeudor.Text = deudor.BR_Direccion.Value;
                        this.txtBRValorDeudor.EditValue = deudor.BR_Valor.Value.ToString();
                        this.chkBRAfectacionDeudor.Checked = deudor.BR_AfectacionFamInd.Value.HasValue? deudor.BR_AfectacionFamInd.Value.Value : false;
                        this.chkBRHipotecaDeudor.Checked= deudor.BR_HipotecaInd.Value.HasValue ? deudor.BR_HipotecaInd.Value.Value : false;
                        this.txtBRHipotecaNombreDeudor.Text = deudor.BR_HipotecaNombre.Value;                                                
                        #endregion

                        #region Vehiculos
                        this.txtVEMarcaDeudor.Text= deudor.VE_Marca.Value;
                        this.txtVEClaseDeudor.Text = deudor.VE_Clase.Value;
                        this.txtVEModeloDeudor.EditValue = deudor.VE_Modelo.Value.HasValue? deudor.VE_Modelo.Value.ToString() : "0";
                        this.txtVEPlacaDeudor.Text = deudor.VE_Placa.Value;
                        this.chkVEPignoradoDeudor.Checked = deudor.VE_PignoradoInd.Value.HasValue? deudor.VE_PignoradoInd.Value.Value : false;
                        this.txtVEPignoradoNombreDeudor.Text= deudor.VE_PignoradoNombre.Value;
                        this.txtVEValorDeudor.EditValue = deudor.VE_Valor.Value.HasValue? deudor.VE_Valor.Value.ToString() : "0";
                        #endregion

                        #region informacion Financiera
                        this.txtActivos.EditValue = deudor.VlrActivos.Value.ToString();
                        this.txtPasivos.EditValue = deudor.VlrPasivos.Value.ToString();
                        this.txtPatrimonio.EditValue = deudor.VlrPatrimonio.Value.ToString();
                        this.txtEgresos.EditValue = deudor.VlrEgresosMes.Value.ToString();
                        this.txtIngresos.EditValue = deudor.VlrIngresosMes.Value.ToString();
                        this.txtVlrIngresosNoOpe.EditValue = deudor.VlrIngresosNoOpe.Value.ToString();
                        this.txtDescrOtrosIng.Text = deudor.DescrOtrosIng.Value;
                        this.txtDescrOtrosBinenes.Text = deudor.DescrOtrosBinenes.Value;
                        #endregion

                        #region Creditos
                        this.txtEntCredito1.Text = deudor.EntCredito1.ToString();
                        this.txtPlazoCredito1.EditValue = deudor.Plazo1.Value.HasValue? deudor.Plazo1.Value.ToString() : "0";
                        this.txtSaldoCredito1.EditValue = deudor.Saldo1.Value.HasValue? deudor.Saldo1.Value.ToString() : "0";
                        this.chkSolicitudInd1.Checked = deudor.SolicitudInd1.Value.HasValue ? deudor.SolicitudInd1.Value.Value : false;
                        this.txtDeclFondos.Text = deudor.DeclFondos.Value;
                        #endregion

                    }
                    else
                    {
                        this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value ?? 1;
                        DTO_ccSolicitudDatosPersonales deudorExist = this._bc.AdministrationModel.RegistroSolicitud_GetByCedula(this.txtCedulaDeudor.Text, this.txtCedulaDeudor.Text, 1, this._data.SolicituDocu.VersionNro.Value.Value);
                        if (deudorExist != null)
                        {
                            #region Datos Personales
                            this.txtCedulaDeudor.Text = deudorExist.TerceroID.Value;
                            this.txtPriApellidoDeudor.Text = deudorExist.ApellidoPri.Value;
                            this.txtSdoApellidoDeudor.Text = deudorExist.ApellidoSdo.Value;
                            this.txtPriNombreDeudor.Text = deudorExist.NombrePri.Value;
                            this.txtSdoNombreDeudor.Text = deudorExist.NombreSdo.Value;
                            this.dtFechaExpDeudor.DateTime = deudorExist.FechaExpDoc.Value.HasValue ? deudorExist.FechaExpDoc.Value.Value : this.dtFechaExpDeudor.DateTime;
                            this.masterCiudadExpDeudor.Value = deudorExist.CiudadExpDoc.Value;
                            this.masterTerceroDocTipoDeudor.Value = deudorExist.TerceroDocTipoID.Value;
                            this.dtFechaNacDeudor.DateTime = deudorExist.FechaNacimiento.Value.HasValue ? deudorExist.FechaNacimiento.Value.Value : this.dtFechaNacDeudor.DateTime;
                            this.txtCelular1Deudor.Text = deudorExist.Celular1.Value;
                            this.txtCelular2Deudor.Text = deudorExist.Celular2.Value;
                            this.txtCorreoDeudor.Text = deudorExist.CorreoElectronico.Value;
                            this.txtDireccionDeudor.EditValue = deudorExist.DirResidencia.Value;
                            this.txtBarrioResidenciaDeudor.Text = deudorExist.BarrioResidencia.Value;
                            this.masterCiudadResDeudor.Value = deudorExist.CiudadResidencia.Value;
                            this.txtAntResidenciaDeudor.EditValue = deudorExist.AntResidencia.Value.HasValue? deudorExist.AntResidencia.Value.ToString() : "0";
                            this.cmbTipoViviendaDeudor.EditValue = deudorExist.TipoVivienda.Value.HasValue ? deudorExist.TipoVivienda.Value.ToString() : "1";
                            this.txtTelResidenciaDeudor.Text = deudorExist.TelResidencia.Value;
                            this.txtDireccionDeudor.EditValue = deudorExist.DirResidencia.Value;
                            this.txtLugarTrabajoDeudor.Text = deudorExist.LugarTrabajo.Value;
                            this.txtDirTrabajoDeudor.Text = deudorExist.DirTrabajo.Value;
                            this.txtBarrioTrabajoDeudor.Text = deudorExist.BarrioTrabajo.Value;
                            this.masterCiudadTrabajoDeudor.Value = deudorExist.CiudadTrabajo.Value;
                            this.txtTelTrabajoDeudor.Text = deudorExist.TelTrabajo.Value;
                            this.txtCargoDeudor.Text = deudorExist.Cargo.Value;
                            this.txtAntTrabajoDeudor.EditValue = deudorExist.AntTrabajo.Value.HasValue ? deudorExist.AntTrabajo.Value.ToString() : "0";
                            this.cmbTipoContratoDeudor.EditValue = deudorExist.TipoContrato.Value.HasValue ? deudorExist.TipoContrato.Value.ToString() : "1";
                            this.txtEpsDeudor.Text = deudorExist.EPS.Value;
                            this.txtPersonasCargoDeudor.EditValue = deudorExist.Personascargo.Value.HasValue ? deudorExist.Personascargo.Value.ToString() : "0";
                            #endregion

                            #region Datos Conyugue
                            this.txtConyNombreDeudor.Text = deudorExist.NombreConyugue.Value;
                            this.txtConyDocumentoDeudor.Text = deudorExist.Conyugue.Value;
                            this.txtConyActividadDeudor.Text = deudorExist.ActConyugue.Value;
                            this.txtConyAntDeudor.Text = deudorExist.AntConyugue.Value.HasValue?  deudorExist.AntConyugue.Value.ToString() : "0";
                            this.txtConyEmpresaDeudor.Text = deudorExist.EmpresaConyugue.Value;
                            this.txtConyDirResidenciaDeudor.Text = deudorExist.DirResConyugue.Value;
                            this.txtConyTelefonoDeudor.Text = deudorExist.TelefonoConyugue.Value;
                            this.txtConyCelularDeudor.Text = deudorExist.CelularConyugue.Value;
                            #endregion

                            #region Referencias
                            this.cmbTipoReferencia1Deudor.EditValue = deudorExist.TipoReferencia1.Value.HasValue? deudorExist.TipoReferencia1.Value.ToString() : "1";
                            this.txtNombreReferencia1Deudor.Text = deudorExist.NombreReferencia1.Value;
                            this.txtParentescoReferencia1Deudor.Text = deudorExist.RelReferencia1.Value;
                            this.txtDireccionReferencia1Deudor.Text = deudorExist.DirReferencia1.Value;
                            this.txtBarrioReferencia1Deudor.Text = deudorExist.BarrioReferencia1.Value;
                            this.txtTelefonoReferencia1Deudor.Text = deudorExist.TelefonoReferencia1.Value;
                            this.txtCelularReferencia1Deudor.Text = deudorExist.CelularReferencia1.Value;

                            this.cmbTipoReferencia2Deudor.EditValue = deudorExist.TipoReferencia2.Value.HasValue? deudorExist.TipoReferencia2.Value.ToString() : "2";
                            this.txtNombreReferencia2Deudor.Text = deudorExist.NombreReferencia2.Value;
                            this.txtParentescoReferencia2Deudor.Text = deudorExist.RelReferencia2.Value;
                            this.txtDireccionReferencia2Deudor.Text = deudorExist.DirReferencia2.Value;
                            this.txtBarrioReferencia2Deudor.Text = deudorExist.BarrioReferencia2.Value;
                            this.txtTelefonoReferencia2Deudor.Text = deudorExist.TelefonoReferencia2.Value;
                            this.txtCelularReferencia2Deudor.Text = deudorExist.CelularReferencia2.Value;
                            #endregion

                            #region Bienes Raices
                            this.txtBRDireccionDeudor.Text = deudorExist.BR_Direccion.Value;
                            this.txtBRValorDeudor.EditValue = deudorExist.BR_Valor.Value.ToString();
                            this.chkBRAfectacionDeudor.Checked = deudorExist.BR_AfectacionFamInd.Value.HasValue ? deudorExist.BR_AfectacionFamInd.Value.Value : false;
                            this.chkBRHipotecaDeudor.Checked = deudorExist.BR_HipotecaInd.Value.HasValue ? deudorExist.BR_HipotecaInd.Value.Value : false;
                            this.txtBRHipotecaNombreDeudor.Text = deudorExist.BR_HipotecaNombre.Value;
                            #endregion

                            #region Vehiculos
                            this.txtVEMarcaDeudor.Text = deudorExist.VE_Marca.Value;
                            this.txtVEClaseDeudor.Text = deudorExist.VE_Clase.Value;
                            this.txtVEModeloDeudor.EditValue = deudorExist.VE_Modelo.Value.HasValue? deudorExist.VE_Modelo.Value.ToString() : "0";
                            this.txtVEPlacaDeudor.Text = deudorExist.VE_Placa.Value;
                            this.chkVEPignoradoDeudor.Checked = deudorExist.VE_PignoradoInd.Value.HasValue ? deudorExist.VE_PignoradoInd.Value.Value : false;
                            this.txtVEPignoradoNombreDeudor.Text = deudorExist.VE_PignoradoNombre.Value;
                            this.txtVEValorDeudor.EditValue = deudorExist.VE_Valor.Value.HasValue? deudorExist.VE_Valor.Value.ToString(): "0";
                            #endregion
                        }
                        else
                        {
                            this.txtPriApellidoDeudor.Text = this._data.SolicituDocu.ApellidoPri.Value.ToString();
                            this.txtSdoApellidoDeudor.Text = this._data.SolicituDocu.ApellidoSdo.Value.ToString();
                            this.txtPriNombreDeudor.Text = this._data.SolicituDocu.NombrePri.Value.ToString();
                            this.txtSdoNombreDeudor.Text = this._data.SolicituDocu.NombreSdo.Value.ToString();
                        }
                    }
                    #endregion
                    //Codeudor1 (TipoPersona 2)
                    #region codeudor1
                    if (codeudor1 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod1.Text = codeudor1.TerceroID.Value;
                        this.txtPriApellidoCod1.Text = codeudor1.ApellidoPri.Value;
                        this.txtSdoApellidoCod1.Text = codeudor1.ApellidoSdo.Value;
                        this.txtPriNombreCod1.Text = codeudor1.NombrePri.Value;
                        this.txtSdoNombreCod1.Text = codeudor1.NombreSdo.Value;
                        this.dtFechaExpCod1.DateTime = codeudor1.FechaExpDoc.Value.HasValue ? codeudor1.FechaExpDoc.Value.Value : DateTime.Today;
                        this.masterCiudadExpCod1.Value = codeudor1.CiudadExpDoc.Value;
                        this.masterTerceroDocTipoCod1.Value = codeudor1.TerceroDocTipoID.Value;
                        this.dtFechaNacCod1.DateTime = codeudor1.FechaNacimiento.Value.HasValue ? codeudor1.FechaNacimiento.Value.Value : DateTime.Today;
                        this.txtCelular1Cod1.Text = codeudor1.Celular1.Value;
                        this.txtCelular2Cod1.Text = codeudor1.Celular2.Value;
                        this.txtCorreoCod1.Text = codeudor1.CorreoElectronico.Value;
                        this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                        this.txtBarrioResidenciaCod1.Text = codeudor1.BarrioResidencia.Value;
                        this.masterCiudadResCod1.Value = codeudor1.CiudadResidencia.Value;
                        this.txtAntResidenciaCod1.EditValue = codeudor1.AntResidencia.Value.HasValue? codeudor1.AntResidencia.Value.ToString() : "0";
                        this.cmbTipoViviendaCod1.EditValue = codeudor1.TipoVivienda.Value.HasValue ? codeudor1.TipoVivienda.Value.ToString() : "1";
                        this.txtTelResidenciaCod1.Text = codeudor1.TelResidencia.Value;
                        this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                        this.txtLugarTrabajoCod1.Text = codeudor1.LugarTrabajo.Value;
                        this.txtDirTrabajoCod1.Text = codeudor1.DirTrabajo.Value;
                        this.txtBarrioTrabajoCod1.Text = codeudor1.BarrioTrabajo.Value;
                        this.masterCiudadTrabajoCod1.Value = codeudor1.CiudadTrabajo.Value;
                        this.txtTelTrabajoCod1.Text = codeudor1.TelTrabajo.Value;
                        this.txtCargoCod1.Text = codeudor1.Cargo.Value;
                        this.txtAntTrabajoCod1.EditValue = codeudor1.AntTrabajo.Value.HasValue ? codeudor1.AntTrabajo.Value.ToString() : "0";
                        this.cmbTipoContratoCod1.EditValue = codeudor1.TipoContrato.Value.HasValue ? codeudor1.TipoContrato.Value.ToString() : "1";
                        this.txtEpsCod1.Text = codeudor1.EPS.Value;
                        this.txtPersonasCargoCod1.EditValue = codeudor1.Personascargo.Value.HasValue ? codeudor1.Personascargo.Value.ToString() : "0";

                        if (!string.IsNullOrEmpty(codeudor1.TerceroID.Value))
                            this.linkCodeudor1.Dock = DockStyle.None;
                        #endregion

                        #region Datos Conyugue
                        this.txtConyNombreCod1.Text = codeudor1.NombreConyugue.Value;
                        this.txtConyDocumentoCod1.Text = codeudor1.Conyugue.Value;
                        this.txtConyActividadCod1.Text = codeudor1.ActConyugue.Value;
                        this.txtConyAntCod1.Text = codeudor1.AntConyugue.Value.HasValue? codeudor1.AntConyugue.Value.ToString() : "0";
                        this.txtConyEmpresaCod1.Text = codeudor1.EmpresaConyugue.Value.ToString();
                        this.txtConyDirResidenciaCod1.Text = codeudor1.DirResConyugue.Value;
                        this.txtConyTelefonoCod1.Text = codeudor1.TelefonoConyugue.Value;
                        this.txtConyCelularCod1.Text = codeudor1.CelularConyugue.Value;
                        #endregion

                        #region Referencias
                        this.cmbTipoReferencia1Cod1.EditValue = codeudor1.TipoReferencia1.Value.HasValue? codeudor1.TipoReferencia1.Value.ToString() : "1";
                        this.txtNombreReferencia1Cod1.Text = codeudor1.NombreReferencia1.Value;
                        this.txtParentescoReferencia1Cod1.Text = codeudor1.RelReferencia1.Value;
                        this.txtDireccionReferencia1Cod1.Text = codeudor1.DirReferencia1.Value;
                        this.txtBarrioReferencia1Cod1.Text = codeudor1.BarrioReferencia1.Value;
                        this.txtTelefonoReferencia1Cod1.Text = codeudor1.TelefonoReferencia1.Value;
                        this.txtCelularReferencia1Cod1.Text = codeudor1.CelularReferencia1.Value;

                        this.cmbTipoReferencia2Cod1.EditValue = codeudor1.TipoReferencia2.Value.HasValue? codeudor1.TipoReferencia2.Value.ToString() : "2";
                        this.txtNombreReferencia2Cod1.Text = codeudor1.NombreReferencia2.Value;
                        this.txtParentescoReferencia2Cod1.Text = codeudor1.RelReferencia2.Value;
                        this.txtDireccionReferencia2Cod1.Text = codeudor1.DirReferencia2.Value;
                        this.txtBarrioReferencia2Cod1.Text = codeudor1.BarrioReferencia2.Value;
                        this.txtTelefonoReferencia2Cod1.Text = codeudor1.TelefonoReferencia2.Value;
                        this.txtCelularReferencia2Cod1.Text = codeudor1.CelularReferencia2.Value;
                        #endregion

                        #region Bienes Raices
                        this.txtBRDireccionCod1.Text = codeudor1.BR_Direccion.Value;
                        this.txtBRValorCod1.EditValue = codeudor1.BR_Valor.Value.ToString();
                        this.chkBRAfectacionCod1.Checked = codeudor1.BR_AfectacionFamInd.Value.HasValue ? codeudor1.BR_AfectacionFamInd.Value.Value : false;
                        this.chkBRHipotecaCod1.Checked = codeudor1.BR_HipotecaInd.Value.HasValue ? codeudor1.BR_HipotecaInd.Value.Value : false;
                        this.txtBRHipotecaNombreCod1.Text = codeudor1.BR_HipotecaNombre.Value;
                        #endregion

                        #region Vehiculos
                        this.txtVEMarcaCod1.Text = codeudor1.VE_Marca.Value;
                        this.txtVEClaseCod1.Text = codeudor1.VE_Clase.Value;
                        this.txtVEModeloCod1.EditValue = codeudor1.VE_Modelo.Value.HasValue? codeudor1.VE_Modelo.Value.ToString() : "0";
                        this.txtVEPlacaCod1.Text = codeudor1.VE_Placa.Value;
                        this.chkVEPignoradoCod1.Checked = codeudor1.VE_PignoradoInd.Value.HasValue ? codeudor1.VE_PignoradoInd.Value.Value : false;
                        this.txtVEPignoradoNombreCod1.Text = codeudor1.VE_PignoradoNombre.Value;
                        this.txtVEValorCod1.EditValue = codeudor1.VE_Valor.Value.HasValue? codeudor1.VE_Valor.Value.ToString() : "0";
                        #endregion
                    }
                    #endregion
                    //Codeudor1 (TipoPersona 3)
                    #region codeudor2
                    if (codeudor2 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod2.Text = codeudor2.TerceroID.Value;
                        this.txtPriApellidoCod2.Text = codeudor2.ApellidoPri.Value;
                        this.txtSdoApellidoCod2.Text = codeudor2.ApellidoSdo.Value;
                        this.txtPriNombreCod2.Text = codeudor2.NombrePri.Value;
                        this.txtSdoNombreCod2.Text = codeudor2.NombreSdo.Value;
                        this.dtFechaExpCod2.DateTime = codeudor2.FechaExpDoc.Value.HasValue ? codeudor2.FechaExpDoc.Value.Value : DateTime.Now;
                        this.masterCiudadExpCod2.Value = codeudor2.CiudadExpDoc.Value;
                        this.masterTerceroDocTipoCod2.Value = codeudor2.TerceroDocTipoID.Value;
                        this.dtFechaNacCod2.DateTime = codeudor2.FechaNacimiento.Value.HasValue ? codeudor2.FechaNacimiento.Value.Value : DateTime.Now;
                        this.txtCelular1Cod2.Text = codeudor2.Celular1.Value;
                        this.txtCelular2Cod2.Text = codeudor2.Celular2.Value;
                        this.txtCorreoCod2.Text = codeudor2.CorreoElectronico.Value;
                        this.txtBarrioResidenciaCod2.Text = codeudor2.BarrioResidencia.Value;
                        this.masterCiudadResCod2.Value = codeudor2.CiudadResidencia.Value;
                        this.txtAntResidenciaCod2.EditValue = codeudor2.AntResidencia.Value.HasValue? codeudor2.AntResidencia.Value.ToString() : "0";
                        this.cmbTipoViviendaCod2.EditValue = codeudor2.TipoVivienda.Value.HasValue ? codeudor2.TipoVivienda.Value.ToString() : "1";
                        this.txtTelResidenciaCod2.Text = codeudor2.TelResidencia.Value;
                        this.txtDireccionCod2.EditValue = codeudor2.DirResidencia.Value;
                        this.txtLugarTrabajoCod2.Text = codeudor2.LugarTrabajo.Value;
                        this.txtDirTrabajoCod2.Text = codeudor2.DirTrabajo.Value;
                        this.txtBarrioTrabajoCod2.Text = codeudor2.BarrioTrabajo.Value;
                        this.masterCiudadTrabajoCod2.Value = codeudor2.CiudadTrabajo.Value;
                        this.txtTelTrabajoCod2.Text = codeudor2.TelTrabajo.Value;
                        this.txtCargoCod2.Text = codeudor2.Cargo.Value;
                        this.txtAntTrabajoCod2.EditValue = codeudor2.AntTrabajo.Value.HasValue ? codeudor2.AntTrabajo.Value.ToString() : "0";
                        this.cmbTipoContratoCod2.EditValue = codeudor2.TipoContrato.Value.HasValue ? codeudor2.TipoContrato.Value.ToString() : "1";
                        this.txtEpsCod2.Text = codeudor2.EPS.Value;
                        this.txtPersonasCargoCod2.Text = codeudor2.Personascargo.Value.HasValue ? codeudor2.Personascargo.Value.ToString() : "0";

                        if (!string.IsNullOrEmpty(codeudor2.TerceroID.Value))
                            this.linkCodeudor2.Dock = DockStyle.None;
                        #endregion

                        #region Datos Conyugue
                        this.txtConyNombreCod2.Text = codeudor2.NombreConyugue.Value;
                        this.txtConyDocumentoCod2.Text = codeudor2.Conyugue.Value;
                        this.txtConyActividadCod2.Text = codeudor2.ActConyugue.Value;
                        this.txtConyAntCod2.EditValue = codeudor2.AntConyugue.Value.HasValue? codeudor2.AntConyugue.Value.ToString() : "0";
                        this.txtConyEmpresaCod2.Text = codeudor2.EmpresaConyugue.Value.ToString();
                        this.txtConyDirResidenciaCod2.Text = codeudor2.DirResConyugue.Value;
                        this.txtConyTelefonoCod2.Text = codeudor2.TelefonoConyugue.Value;
                        this.txtConyCelularCod2.Text = codeudor2.CelularConyugue.Value;
                        #endregion

                        #region Referencias
                        this.cmbTipoReferencia1Cod2.EditValue = codeudor2.TipoReferencia1.Value.HasValue? codeudor2.TipoReferencia1.Value.ToString() : "1";
                        this.txtNombreReferencia1Cod2.Text = codeudor2.NombreReferencia1.Value;
                        this.txtParentescoReferencia1Cod2.Text = codeudor2.RelReferencia1.Value;
                        this.txtDireccionReferencia1Cod2.Text = codeudor2.DirReferencia1.Value;
                        this.txtBarrioReferencia1Cod2.Text = codeudor2.BarrioReferencia1.Value;
                        this.txtTelefonoReferencia1Cod2.Text = codeudor2.TelefonoReferencia1.Value;
                        this.txtCelularReferencia1Cod2.Text = codeudor2.CelularReferencia1.Value;

                        this.cmbTipoReferencia2Cod2.EditValue = codeudor2.TipoReferencia2.Value.HasValue? codeudor2.TipoReferencia2.Value.ToString() : "2";
                        this.txtNombreReferencia2Cod2.Text = codeudor2.NombreReferencia2.Value;
                        this.txtParentescoReferencia2Cod2.Text = codeudor2.RelReferencia2.Value;
                        this.txtDireccionReferencia2Cod2.Text = codeudor2.DirReferencia2.Value;
                        this.txtBarrioReferencia2Cod2.Text = codeudor2.BarrioReferencia2.Value;
                        this.txtTelefonoReferencia2Cod2.Text = codeudor2.TelefonoReferencia2.Value;
                        this.txtCelularReferencia2Cod2.Text = codeudor2.CelularReferencia2.Value;
                        #endregion

                        #region Bienes Raices
                        this.txtBRDireccionCod2.Text = codeudor2.BR_Direccion.Value;
                        this.txtBRValorCod2.EditValue = codeudor2.BR_Valor.Value.ToString();
                        this.chkBRAfectacionCod2.Checked = codeudor2.BR_AfectacionFamInd.Value.HasValue ? codeudor2.BR_AfectacionFamInd.Value.Value : false;
                        this.chkBRHipotecaCod2.Checked = codeudor2.BR_HipotecaInd.Value.HasValue ? codeudor2.BR_HipotecaInd.Value.Value : false;
                        this.txtBRHipotecaNombreCod2.Text = codeudor2.BR_HipotecaNombre.Value;
                        #endregion

                        #region Vehiculos
                        this.txtVEMarcaCod2.Text = codeudor2.VE_Marca.Value;
                        this.txtVEClaseCod2.Text = codeudor2.VE_Clase.Value;
                        this.txtVEModeloCod2.EditValue = codeudor2.VE_Modelo.Value.HasValue? codeudor2.VE_Modelo.Value.ToString() : "0";
                        this.txtVEPlacaCod2.Text = codeudor2.VE_Placa.Value;
                        this.chkVEPignoradoCod2.Checked = codeudor2.VE_PignoradoInd.Value.HasValue ? codeudor2.VE_PignoradoInd.Value.Value : false;
                        this.txtVEPignoradoNombreCod2.Text = codeudor2.VE_PignoradoNombre.Value;
                        this.txtVEValorCod2.EditValue = codeudor2.VE_Valor.Value.HasValue? codeudor2.VE_Valor.Value.ToString() : "0";
                        #endregion
                    }
                    #endregion
                    //Codeudor1 (TipoPersona 4)
                    #region codeudor3
                    if (codeudor3 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod3.Text = codeudor3.TerceroID.Value;
                        this.txtPriApellidoCod3.Text = codeudor3.ApellidoPri.Value;
                        this.txtSdoApellidoCod3.Text = codeudor3.ApellidoSdo.Value;
                        this.txtPriNombreCod3.Text = codeudor3.NombrePri.Value;
                        this.txtSdoNombreCod3.Text = codeudor3.NombreSdo.Value;
                        this.dtFechaExpCod3.DateTime = codeudor3.FechaExpDoc.Value.HasValue ? codeudor3.FechaExpDoc.Value.Value : DateTime.Now;
                        this.masterCiudadExpCod3.Value = codeudor3.CiudadExpDoc.Value;
                        this.masterTerceroDocTipoCod3.Value = codeudor3.TerceroDocTipoID.Value;
                         this.dtFechaNacCod3.DateTime = codeudor3.FechaNacimiento.Value.HasValue ? codeudor3.FechaNacimiento.Value.Value : DateTime.Now;
                        this.txtCelular1Cod3.Text = codeudor3.Celular1.Value;
                        this.txtCelular2Cod3.Text = codeudor3.Celular2.Value;
                        this.txtCorreoCod3.Text = codeudor3.CorreoElectronico.Value;                      
                        this.txtBarrioResidenciaCod3.Text = codeudor3.BarrioResidencia.Value;
                        this.masterCiudadResCod3.Value = codeudor3.CiudadResidencia.Value;
                        this.txtAntResidenciaCod3.EditValue = codeudor3.AntResidencia.Value.HasValue? codeudor3.AntResidencia.Value.ToString() : "0";
                        this.cmbTipoViviendaCod3.EditValue = codeudor3.TipoVivienda.Value.HasValue ? codeudor3.TipoVivienda.Value.ToString() : "1";
                        this.txtTelResidenciaCod3.Text = codeudor3.TelResidencia.Value;
                        this.txtDireccionCod3.EditValue = codeudor3.DirResidencia.Value;
                        this.txtLugarTrabajoCod3.Text = codeudor3.LugarTrabajo.Value;
                        this.txtDirTrabajoCod3.Text = codeudor3.DirTrabajo.Value;
                        this.txtBarrioTrabajoCod3.Text = codeudor3.BarrioTrabajo.Value;
                        this.masterCiudadTrabajoCod3.Value = codeudor3.CiudadTrabajo.Value;
                        this.txtTelTrabajoCod3.Text = codeudor3.TelTrabajo.Value;
                        this.txtCargoCod3.Text = codeudor3.Cargo.Value;
                        this.txtAntTrabajoCod3.EditValue = codeudor3.AntTrabajo.Value.HasValue ? codeudor3.AntTrabajo.Value.ToString() : "0";
                        this.cmbTipoContratoCod3.EditValue  = codeudor3.TipoContrato.Value.HasValue ? codeudor3.TipoContrato.Value.ToString() : "1";
                        this.txtEpsCod3.Text = codeudor3.EPS.Value;
                        this.txtPersonasCargoCod3.EditValue = codeudor3.Personascargo.Value.HasValue ? codeudor3.Personascargo.Value.ToString() : "0";

                        if (!string.IsNullOrEmpty(codeudor3.TerceroID.Value))
                            this.linkCodeudor3.Dock = DockStyle.None;
                        #endregion

                        #region Datos Conyugue
                        this.txtConyNombreCod3.Text = codeudor3.NombreConyugue.Value;
                        this.txtConyDocumentoCod3.Text = codeudor3.Conyugue.Value;
                        this.txtConyActividadCod3.Text = codeudor3.ActConyugue.Value;
                        this.txtConyAntCod3.Text = codeudor3.AntConyugue.Value.HasValue? codeudor3.AntConyugue.Value.ToString() : "0";
                        this.txtConyEmpresaCod3.Text = codeudor3.EmpresaConyugue.Value.ToString();
                        this.txtConyDirResidenciaCod3.Text = codeudor3.DirResConyugue.Value;
                        this.txtConyTelefonoCod3.Text = codeudor3.TelefonoConyugue.Value;
                        this.txtConyCelularCod3.Text = codeudor3.CelularConyugue.Value;
                        #endregion

                        #region Referencias
                        this.cmbTipoReferencia1Cod3.EditValue = codeudor3.TipoReferencia1.Value.HasValue? codeudor3.TipoReferencia1.Value.ToString() : "1";
                        this.txtNombreReferencia1Cod3.Text = codeudor3.NombreReferencia1.Value;
                        this.txtParentescoReferencia1Cod3.Text = codeudor3.RelReferencia1.Value;
                        this.txtDireccionReferencia1Cod3.Text = codeudor3.DirReferencia1.Value;
                        this.txtBarrioReferencia1Cod3.Text = codeudor3.BarrioReferencia1.Value;
                        this.txtTelefonoReferencia1Cod3.Text = codeudor3.TelefonoReferencia1.Value;
                        this.txtCelularReferencia1Cod3.Text = codeudor3.CelularReferencia1.Value;

                        this.cmbTipoReferencia2Cod3.EditValue = codeudor3.TipoReferencia2.Value.HasValue? codeudor3.TipoReferencia2.Value.ToString() : "2";
                        this.txtNombreReferencia2Cod3.Text = codeudor3.NombreReferencia2.Value;
                        this.txtParentescoReferencia2Cod3.Text = codeudor3.RelReferencia2.Value;
                        this.txtDireccionReferencia2Cod3.Text = codeudor3.DirReferencia2.Value;
                        this.txtBarrioReferencia2Cod3.Text = codeudor3.BarrioReferencia2.Value;
                        this.txtTelefonoReferencia2Cod3.Text = codeudor3.TelefonoReferencia2.Value;
                        this.txtCelularReferencia2Cod3.Text = codeudor3.CelularReferencia2.Value;
                        #endregion

                        #region Bienes Raices
                        this.txtBRDireccionCod3.Text = codeudor3.BR_Direccion.Value;
                        this.txtBRValorCod3.EditValue = codeudor3.BR_Valor.Value.ToString();
                        this.chkBRAfectacionCod3.Checked = codeudor3.BR_AfectacionFamInd.Value.HasValue ? codeudor3.BR_AfectacionFamInd.Value.Value : false;
                        this.chkBRHipotecaCod3.Checked = codeudor3.BR_HipotecaInd.Value.HasValue ? codeudor3.BR_HipotecaInd.Value.Value : false;
                        this.txtBRHipotecaNombreCod3.Text = codeudor3.BR_HipotecaNombre.Value;
                        #endregion

                        #region Vehiculos
                        this.txtVEMarcaCod3.Text = codeudor3.VE_Marca.Value;
                        this.txtVEClaseCod3.Text = codeudor3.VE_Clase.Value;
                        this.txtVEModeloCod3.EditValue = codeudor3.VE_Modelo.Value.HasValue? codeudor3.VE_Modelo.Value.ToString() : "0";
                        this.txtVEPlacaCod3.Text = codeudor3.VE_Placa.Value;
                        this.chkVEPignoradoCod3.Checked = codeudor3.VE_PignoradoInd.Value.HasValue ? codeudor3.VE_PignoradoInd.Value.Value : false;
                        this.txtVEPignoradoNombreCod3.Text = codeudor3.VE_PignoradoNombre.Value;
                        this.txtVEValorCod3.EditValue = codeudor3.VE_Valor.Value.HasValue? codeudor3.VE_Valor.Value.ToString() : "0";
                        #endregion
                    }                    
                    #endregion
                    #endregion

                }
                else
                {
                    #region Actualiza la solicitud
                    //this._data.SolicituDocu.ClienteID.Value = this.txtCedulaDeudor.Text;
                    this._data.SolicituDocu.ClienteRadica.Value = this.txtCedulaDeudor.Text;
                    this._data.SolicituDocu.ApellidoPri.Value = this.txtPriApellidoDeudor.Text;
                    this._data.SolicituDocu.ApellidoSdo.Value = this.txtSdoApellidoDeudor.Text;
                    this._data.SolicituDocu.NombrePri.Value = this.txtPriNombreDeudor.Text;
                    this._data.SolicituDocu.NombreSdo.Value = this.txtSdoNombreDeudor.Text;
                    this._data.SolicituDocu.PagaduriaID.Value = this.masterEntidad.Value;
                    this._data.SolicituDocu.Libranza.Value = Convert.ToInt32(this.txtCredito.EditValue);
                    this._data.SolicituDocu.VlrSolicitado.Value = Convert.ToDecimal(this.txtMontoAprobado.EditValue);
                    this._data.SolicituDocu.VlrPreSolicitado.Value = Convert.ToDecimal(this.txtMontoSolicitud.EditValue);
                    this._data.SolicituDocu.Plazo.Value = Convert.ToInt16(this.txtPlazo.EditValue);
                    this._data.SolicituDocu.PeriodoPago.Value = Convert.ToByte(this.cmbPeriodicidad.EditValue);
                    this._data.SolicituDocu.AsesorID.Value = this.masterAsesor.Value;
                    this._data.SolicituDocu.BancoID_1.Value = this.masterBanco.Value;
                    this._data.SolicituDocu.BcoCtaNro_1.Value = this.txtCuentaNumero.Text;
                    this._data.SolicituDocu.CuentaTipo_1.Value = Convert.ToByte(this.cmbCuentatipo.EditValue) == 3? this._data.SolicituDocu.CuentaTipo_1.Value : Convert.ToByte(this.cmbCuentatipo.EditValue);
                    this._data.SolicituDocu.DatoAdd5.Value = this.txtSucursal.Text;
                    this._data.SolicituDocu.VersionNro.Value =this._data.SolicituDocu.VersionNro.Value.HasValue? this._data.SolicituDocu.VersionNro.Value : 1;

                    #endregion
                    #region Llena datos de los controles para salvar
                    #region Deudor (TipoPersona 1)
                    DTO_ccSolicitudDatosPersonales deudorNew = new DTO_ccSolicitudDatosPersonales();                    

                    #region Datos Personales
                    deudorNew.TipoPersona.Value = 1;
                    deudorNew.ApellidoPri.Value = this.txtPriApellidoDeudor.Text;
                    deudorNew.ApellidoSdo.Value = this.txtSdoApellidoDeudor.Text;
                    deudorNew.NombrePri.Value = this.txtPriNombreDeudor.Text;
                    deudorNew.NombreSdo.Value = this.txtSdoNombreDeudor.Text;
                    deudorNew.FechaExpDoc.Value = Convert.ToDateTime(this.dtFechaExpDeudor.DateTime);
                    deudorNew.CiudadExpDoc.Value = this.masterCiudadExpDeudor.Value;
                    deudorNew.TerceroDocTipoID.Value = this.masterTerceroDocTipoDeudor.Value;
                    deudorNew.Sexo.Value = Convert.ToByte(this.cmbSexo.EditValue);
                    deudorNew.EstadoCivil.Value = Convert.ToByte(this.cmbEstadoCivil.EditValue);
                    deudorNew.TerceroID.Value = this.txtCedulaDeudor.Text;
                    deudorNew.FechaNacimiento.Value = Convert.ToDateTime(this.dtFechaNacDeudor.DateTime);
                    deudorNew.Celular1.Value = this.txtCelular1Deudor.Text;
                    deudorNew.Celular2.Value = this.txtCelular2Deudor.Text;
                    deudorNew.CorreoElectronico.Value = this.txtCorreoDeudor.Text;
                    deudorNew.BarrioResidencia.Value = this.txtBarrioResidenciaDeudor.Text;
                    deudorNew.CiudadResidencia.Value = this.masterCiudadResDeudor.Value;
                    deudorNew.AntResidencia.Value = Convert.ToByte(this.txtAntResidenciaDeudor.EditValue);
                    deudorNew.TipoVivienda.Value = Convert.ToByte(this.cmbTipoViviendaDeudor.EditValue);
                    deudorNew.TelResidencia.Value = this.txtTelResidenciaDeudor.Text;
                    deudorNew.DirResidencia.Value = this.txtDireccionDeudor.EditValue.ToString();
                    deudorNew.LugarTrabajo.Value = this.txtLugarTrabajoDeudor.Text;
                    deudorNew.DirTrabajo.Value = this.txtDirTrabajoDeudor.Text;
                    deudorNew.BarrioTrabajo.Value = this.txtBarrioTrabajoDeudor.Text;
                    deudorNew.CiudadTrabajo.Value = this.masterCiudadTrabajoDeudor.Value;
                    deudorNew.TelTrabajo.Value = this.txtTelTrabajoDeudor.Text;
                    deudorNew.Cargo.Value = this.txtCargoDeudor.Text;
                    deudorNew.AntTrabajo.Value = Convert.ToByte(this.txtAntTrabajoDeudor.EditValue);
                    deudorNew.TipoContrato.Value = Convert.ToByte(this.cmbTipoContratoDeudor.EditValue);
                    deudorNew.EPS.Value = this.txtEpsDeudor.Text;
                    if (!string.IsNullOrEmpty(this.txtPersonasCargoDeudor.EditValue.ToString()))
                        deudorNew.Personascargo.Value =  Convert.ToByte(this.txtPersonasCargoDeudor.EditValue);

                    #endregion

                    #region Datos Conyugue
                    deudorNew.NombreConyugue.Value = this.txtConyNombreDeudor.Text;
                    deudorNew.Conyugue.Value = this.txtConyDocumentoDeudor.Text;
                    deudorNew.ActConyugue.Value = this.txtConyActividadDeudor.Text;
                    deudorNew.AntConyugue.Value = Convert.ToByte(this.txtConyAntDeudor.EditValue);
                    deudorNew.EmpresaConyugue.Value = this.txtConyEmpresaDeudor.Text;
                    deudorNew.DirResConyugue.Value = this.txtConyDirResidenciaDeudor.Text;
                    deudorNew.TelefonoConyugue.Value = this.txtConyTelefonoDeudor.Text;
                    deudorNew.CelularConyugue.Value = this.txtConyCelularDeudor.Text;
                    #endregion

                    #region Referencias
                    deudorNew.TipoReferencia1.Value = Convert.ToByte(this.cmbTipoReferencia1Deudor.EditValue);
                    deudorNew.NombreReferencia1.Value = this.txtNombreReferencia1Deudor.Text;                    
                    deudorNew.RelReferencia1.Value =this.txtParentescoReferencia1Deudor.Text;
                    deudorNew.DirReferencia1.Value = this.txtDireccionReferencia1Deudor.Text;
                    deudorNew.BarrioReferencia1.Value = this.txtBarrioReferencia1Deudor.Text;
                    deudorNew.TelefonoReferencia1.Value = this.txtTelefonoReferencia1Deudor.Text;
                    deudorNew.CelularReferencia1.Value = this.txtCelularReferencia1Deudor.Text;
                    deudorNew.TipoReferencia2.Value = Convert.ToByte(this.cmbTipoReferencia2Deudor.EditValue);
                    deudorNew.NombreReferencia2.Value = this.txtNombreReferencia2Deudor.Text;
                    deudorNew.RelReferencia2.Value = this.txtParentescoReferencia2Deudor.Text;
                    deudorNew.DirReferencia2.Value = this.txtDireccionReferencia2Deudor.Text;
                    deudorNew.BarrioReferencia2.Value = this.txtBarrioReferencia2Deudor.Text;
                    deudorNew.TelefonoReferencia2.Value = this.txtTelefonoReferencia2Deudor.Text;
                    deudorNew.CelularReferencia2.Value = this.txtCelularReferencia2Deudor.Text;
                    #endregion

                    #region Bienes Raices
                    deudorNew.BR_Direccion.Value = this.txtBRDireccionDeudor.Text;
                    deudorNew.BR_Valor.Value = Convert.ToDecimal(this.txtBRValorDeudor.EditValue);
                    deudorNew.BR_AfectacionFamInd.Value = Convert.ToBoolean(this.chkBRAfectacionDeudor.Checked);
                    deudorNew.BR_HipotecaInd.Value = Convert.ToBoolean(this.chkBRHipotecaDeudor.Checked);
                    deudorNew.BR_HipotecaNombre.Value = this.txtBRHipotecaNombreDeudor.Text;
                    #endregion

                    #region Vehiculos
                    deudorNew.VE_Marca.Value = this.txtVEMarcaDeudor.Text;
                    deudorNew.VE_Clase.Value = this.txtVEClaseDeudor.Text;
                     deudorNew.VE_Modelo.Value = Convert.ToInt16(this.txtVEModeloDeudor.EditValue);
                    deudorNew.VE_Placa.Value = this.txtVEPlacaDeudor.Text;
                    deudorNew.VE_PignoradoInd.Value = Convert.ToBoolean(this.chkVEPignoradoDeudor.Checked);
                    deudorNew.VE_PignoradoNombre.Value = this.txtVEPignoradoNombreDeudor.Text;
                    deudorNew.VE_Valor.Value = Convert.ToDecimal(this.txtVEValorDeudor.EditValue);
                    #endregion

                    #region informacion Financiera

                    deudorNew.VlrActivos.Value = Convert.ToDecimal(this.txtActivos.EditValue);
                    deudorNew.VlrPasivos.Value = Convert.ToDecimal(this.txtPasivos.EditValue);
                    deudorNew.VlrPatrimonio.Value = Convert.ToDecimal(this.txtPatrimonio.EditValue);
                    deudorNew.VlrEgresosMes.Value = Convert.ToDecimal(this.txtEgresos.EditValue);
                    deudorNew.VlrIngresosMes.Value = Convert.ToDecimal(this.txtIngresos.EditValue);
                    deudorNew.VlrIngresosNoOpe.Value = Convert.ToDecimal(this.txtVlrIngresosNoOpe.EditValue);
                    deudorNew.DescrOtrosIng.Value = this.txtDescrOtrosIng.Text;
                    deudorNew.DescrOtrosBinenes.Value = this.txtDescrOtrosBinenes.Text;

                    #endregion

                    #region Creditos
                    deudorNew.EntCredito1.Value = this.txtEntCredito1.Text;
                    deudorNew.Plazo1.Value = Convert.ToByte(this.txtPlazoCredito1.EditValue);
                    deudorNew.Saldo1.Value = Convert.ToDecimal(this.txtSaldoCredito1.EditValue);
                    deudorNew.SolicitudInd1.Value= Convert.ToBoolean(this.chkSolicitudInd1.Checked);
                    deudorNew.DeclFondos.Value = this.txtDeclFondos.Text;

                    #endregion


                    deudorNew.Consecutivo.Value = deudor != null && deudor.Consecutivo.Value.HasValue ? deudor.Consecutivo.Value : null;
                    deudorNew.Version.Value = deudor != null ? deudor.Version.Value : 1;
                    this._data.DatosPersonales.RemoveAll(x=>x.TipoPersona.Value == 1);
                    this._data.DatosPersonales.Add(deudorNew);
                    #endregion
                    #region codeudor1 (TipoPersona 2)
                    DTO_ccSolicitudDatosPersonales codeudor1New = new DTO_ccSolicitudDatosPersonales();

                    #region Datos Personales
                    codeudor1New.TipoPersona.Value = 2;
                    codeudor1New.ApellidoPri.Value = this.txtPriApellidoCod1.Text;
                    codeudor1New.ApellidoSdo.Value = this.txtSdoApellidoCod1.Text;
                    codeudor1New.NombrePri.Value = this.txtPriNombreCod1.Text;
                    codeudor1New.NombreSdo.Value = this.txtSdoNombreCod1.Text;
                    codeudor1New.FechaExpDoc.Value = Convert.ToDateTime(this.dtFechaExpCod1.DateTime);
                    codeudor1New.CiudadExpDoc.Value = this.masterCiudadExpCod1.Value;
                    codeudor1New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod1.Value;
                    codeudor1New.TerceroID.Value = this.txtCedulaCod1.Text;
                    codeudor1New.FechaNacimiento.Value = Convert.ToDateTime(this.dtFechaNacCod1.DateTime);
                    codeudor1New.Celular1.Value = this.txtCelular1Cod1.Text;
                    codeudor1New.Celular2.Value = this.txtCelular2Cod1.Text;
                    codeudor1New.CorreoElectronico.Value = this.txtCorreoCod1.Text;
                    codeudor1New.DirResidencia.Value = this.txtDireccionCod1.EditValue.ToString();
                    codeudor1New.BarrioResidencia.Value = this.txtBarrioResidenciaCod1.Text;
                    codeudor1New.CiudadResidencia.Value = this.masterCiudadResCod1.Value;
                    codeudor1New.AntResidencia.Value = Convert.ToByte(this.txtAntResidenciaCod1.EditValue);
                    codeudor1New.TipoVivienda.Value = Convert.ToByte(this.cmbTipoViviendaCod1.EditValue);
                    codeudor1New.TelResidencia.Value = this.txtTelResidenciaCod1.Text;
                    codeudor1New.LugarTrabajo.Value = this.txtLugarTrabajoCod1.Text;
                    codeudor1New.DirTrabajo.Value = this.txtDirTrabajoCod1.Text;
                    codeudor1New.BarrioTrabajo.Value = this.txtBarrioTrabajoCod1.Text;
                    codeudor1New.CiudadTrabajo.Value = this.masterCiudadTrabajoCod1.Value;
                    codeudor1New.TelTrabajo.Value = this.txtTelTrabajoCod1.Text;
                    codeudor1New.Cargo.Value = this.txtCargoCod1.Text;
                    codeudor1New.AntTrabajo.Value = Convert.ToByte(this.txtAntTrabajoCod1.EditValue);
                    codeudor1New.TipoContrato.Value = Convert.ToByte(this.cmbTipoContratoCod1.EditValue);
                    codeudor1New.EPS.Value = this.txtEpsCod1.Text;
                    if(!string.IsNullOrEmpty(this.txtPersonasCargoCod1.EditValue.ToString()))
                        codeudor1New.Personascargo.Value = Convert.ToByte(this.txtPersonasCargoCod1.EditValue);

                    #endregion

                    #region Datos Conyugue
                    codeudor1New.NombreConyugue.Value = this.txtConyNombreCod1.Text;
                    codeudor1New.Conyugue.Value = this.txtConyDocumentoCod1.Text;
                    codeudor1New.ActConyugue.Value = this.txtConyActividadCod1.Text;
                    codeudor1New.AntConyugue.Value = Convert.ToByte(this.txtConyAntCod1.EditValue);
                    codeudor1New.EmpresaConyugue.Value = this.txtConyEmpresaCod1.Text;
                    codeudor1New.DirResConyugue.Value = this.txtConyDirResidenciaCod1.Text;
                    codeudor1New.TelefonoConyugue.Value = this.txtConyTelefonoCod1.Text;
                    codeudor1New.CelularConyugue.Value = this.txtConyCelularCod1.Text;
                    #endregion

                    #region Referencias
                    codeudor1New.TipoReferencia1.Value = Convert.ToByte(this.cmbTipoReferencia1Cod1.EditValue);
                    codeudor1New.NombreReferencia1.Value = this.txtNombreReferencia1Cod1.Text;
                    codeudor1New.RelReferencia1.Value = this.txtParentescoReferencia1Cod1.Text;
                    codeudor1New.DirReferencia1.Value = this.txtDireccionReferencia1Cod1.Text;
                    codeudor1New.BarrioReferencia1.Value = this.txtBarrioReferencia1Cod1.Text;
                    codeudor1New.TelefonoReferencia1.Value = this.txtTelefonoReferencia1Cod1.Text;
                    codeudor1New.CelularReferencia1.Value = this.txtCelularReferencia1Cod1.Text;
                    codeudor1New.TipoReferencia2.Value = Convert.ToByte(this.cmbTipoReferencia2Cod1.EditValue);
                    codeudor1New.NombreReferencia2.Value = this.txtNombreReferencia2Cod1.Text;
                    codeudor1New.RelReferencia2.Value = this.txtParentescoReferencia2Cod1.Text;
                    codeudor1New.DirReferencia2.Value = this.txtDireccionReferencia2Cod1.Text;
                    codeudor1New.BarrioReferencia2.Value = this.txtBarrioReferencia2Cod1.Text;
                    codeudor1New.TelefonoReferencia2.Value = this.txtTelefonoReferencia2Cod1.Text;
                    codeudor1New.CelularReferencia2.Value = this.txtCelularReferencia2Cod1.Text;
                    #endregion

                    #region Bienes Raices
                    codeudor1New.BR_Direccion.Value = this.txtBRDireccionCod1.Text;
                    codeudor1New.BR_Valor.Value = Convert.ToDecimal(this.txtBRValorCod1.EditValue);
                    codeudor1New.BR_AfectacionFamInd.Value = Convert.ToBoolean(this.chkBRAfectacionCod1.Checked);
                    codeudor1New.BR_HipotecaInd.Value = Convert.ToBoolean(this.chkBRHipotecaCod1.Checked);
                    codeudor1New.BR_HipotecaNombre.Value = this.txtBRHipotecaNombreCod1.Text;
                    #endregion

                    #region Vehiculos
                    codeudor1New.VE_Marca.Value = this.txtVEMarcaCod1.Text;
                    codeudor1New.VE_Clase.Value = this.txtVEClaseCod1.Text;
                    codeudor1New.VE_Modelo.Value = Convert.ToInt16(this.txtVEModeloCod1.EditValue);
                    codeudor1New.VE_Placa.Value = this.txtVEPlacaCod1.Text;
                    codeudor1New.VE_PignoradoInd.Value = Convert.ToBoolean(this.chkVEPignoradoCod1.Checked);
                    codeudor1New.VE_PignoradoNombre.Value = this.txtVEPignoradoNombreCod1.Text;
                    codeudor1New.VE_Valor.Value = Convert.ToDecimal(this.txtVEValorCod1.EditValue);
                    #endregion

                    #region informacion Financiera
                    codeudor1New.VlrActivos.Value = 0;
                    codeudor1New.VlrPasivos.Value = 0;
                    codeudor1New.VlrPatrimonio.Value = 0;
                    codeudor1New.VlrEgresosMes.Value = 0;
                    codeudor1New.VlrIngresosMes.Value = 0;
                    codeudor1New.VlrIngresosNoOpe.Value = 0;
                    #endregion

                    codeudor1New.Consecutivo.Value = codeudor1 != null && codeudor1.Consecutivo.Value.HasValue ? codeudor1.Consecutivo.Value : null;
                    codeudor1New.Version.Value = codeudor1 != null ? codeudor1.Version.Value : 1;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 2);
                    this._data.DatosPersonales.Add(codeudor1New);
                    #endregion
                    #region Codeudor2 (TipoPersona 3)
                    DTO_ccSolicitudDatosPersonales codeudor2New = new DTO_ccSolicitudDatosPersonales();

                    #region Datos Personales
                    codeudor2New.TipoPersona.Value = 3;

                    codeudor2New.ApellidoPri.Value = this.txtPriApellidoCod2.Text;
                    codeudor2New.ApellidoSdo.Value = this.txtSdoApellidoCod2.Text;
                    codeudor2New.NombrePri.Value = this.txtPriNombreCod2.Text;
                    codeudor2New.NombreSdo.Value = this.txtSdoNombreCod2.Text;
                    codeudor2New.FechaExpDoc.Value = Convert.ToDateTime(this.dtFechaExpCod2.DateTime);
                    codeudor2New.CiudadExpDoc.Value = this.masterCiudadExpCod2.Value;
                    codeudor2New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod2.Value;

                    codeudor2New.TerceroID.Value = this.txtCedulaCod2.Text;
                    codeudor2New.FechaNacimiento.Value = Convert.ToDateTime(this.dtFechaNacCod2.DateTime);
                    codeudor2New.Celular1.Value = this.txtCelular1Cod2.Text;
                    codeudor2New.Celular2.Value = this.txtCelular2Cod2.Text;
                    codeudor2New.CorreoElectronico.Value = this.txtCorreoCod2.Text;
                    codeudor2New.DirResidencia.Value = this.txtDireccionCod2.EditValue.ToString();
                    codeudor2New.BarrioResidencia.Value = this.txtBarrioResidenciaCod2.Text;
                    codeudor2New.CiudadResidencia.Value = this.masterCiudadResCod2.Value;
                    codeudor2New.AntResidencia.Value = Convert.ToByte(this.txtAntResidenciaCod2.EditValue);
                    codeudor2New.TipoVivienda.Value = Convert.ToByte(this.cmbTipoViviendaCod2.EditValue);
                    codeudor2New.TelResidencia.Value = this.txtTelResidenciaCod2.Text;
                    codeudor2New.LugarTrabajo.Value = this.txtLugarTrabajoCod2.Text;
                    codeudor2New.DirTrabajo.Value = this.txtDirTrabajoCod2.Text;
                    codeudor2New.BarrioTrabajo.Value = this.txtBarrioTrabajoCod2.Text;
                    codeudor2New.CiudadTrabajo.Value = this.masterCiudadTrabajoCod2.Value;
                    codeudor2New.TelTrabajo.Value = this.txtTelTrabajoCod2.Text;
                    codeudor2New.Cargo.Value = this.txtCargoCod2.Text;
                    codeudor2New.AntTrabajo.Value = Convert.ToByte(this.txtAntTrabajoCod2.EditValue);
                    codeudor2New.TipoContrato.Value = Convert.ToByte(this.cmbTipoContratoCod2.EditValue);
                    codeudor2New.EPS.Value = this.txtEpsCod2.Text;
                    if (!string.IsNullOrEmpty(this.txtPersonasCargoCod2.EditValue.ToString()))
                        codeudor2New.Personascargo.Value = Convert.ToByte(this.txtPersonasCargoCod2.EditValue);

                    #endregion

                    #region Datos Conyugue
                    codeudor2New.NombreConyugue.Value = this.txtConyNombreCod2.Text;
                    codeudor2New.Conyugue.Value = this.txtConyDocumentoCod2.Text;
                    codeudor2New.ActConyugue.Value = this.txtConyActividadCod2.Text;
                    codeudor2New.AntConyugue.Value = Convert.ToByte(this.txtConyAntCod2.EditValue);
                    codeudor2New.EmpresaConyugue.Value = this.txtConyEmpresaCod2.Text;
                    codeudor2New.DirResConyugue.Value = this.txtConyDirResidenciaCod2.Text;
                    codeudor2New.TelefonoConyugue.Value = this.txtConyTelefonoCod2.Text;
                    codeudor2New.CelularConyugue.Value = this.txtConyCelularCod2.Text;
                    #endregion

                    #region Referencias
                    codeudor2New.TipoReferencia1.Value = Convert.ToByte(this.cmbTipoReferencia1Cod2.EditValue);
                    codeudor2New.NombreReferencia1.Value = this.txtNombreReferencia1Cod2.Text;
                    codeudor2New.RelReferencia1.Value = this.txtParentescoReferencia1Cod2.Text;
                    codeudor2New.DirReferencia1.Value = this.txtDireccionReferencia1Cod2.Text;
                    codeudor2New.BarrioReferencia1.Value = this.txtBarrioReferencia1Cod2.Text;
                    codeudor2New.TelefonoReferencia1.Value = this.txtTelefonoReferencia1Cod2.Text;
                    codeudor2New.CelularReferencia1.Value = this.txtCelularReferencia1Cod2.Text;
                    codeudor2New.TipoReferencia2.Value = Convert.ToByte(this.cmbTipoReferencia2Cod2.EditValue);
                    codeudor2New.NombreReferencia2.Value = this.txtNombreReferencia2Cod2.Text;
                    codeudor2New.RelReferencia2.Value = this.txtParentescoReferencia2Cod2.Text;
                    codeudor2New.DirReferencia2.Value = this.txtDireccionReferencia2Cod2.Text;
                    codeudor2New.BarrioReferencia2.Value = this.txtBarrioReferencia2Cod2.Text;
                    codeudor2New.TelefonoReferencia2.Value = this.txtTelefonoReferencia2Cod2.Text;
                    codeudor2New.CelularReferencia2.Value = this.txtCelularReferencia2Cod2.Text;
                    #endregion

                    #region Bienes Raices
                    codeudor2New.BR_Direccion.Value = this.txtBRDireccionCod2.Text;
                    codeudor2New.BR_Valor.Value = Convert.ToDecimal(this.txtBRValorCod2.EditValue);
                    codeudor2New.BR_AfectacionFamInd.Value = Convert.ToBoolean(this.chkBRAfectacionCod2.Checked);
                    codeudor2New.BR_HipotecaInd.Value = Convert.ToBoolean(this.chkBRHipotecaCod2.Checked);
                    codeudor2New.BR_HipotecaNombre.Value = this.txtBRHipotecaNombreCod2.Text;
                    #endregion

                    #region Vehiculos
                    codeudor2New.VE_Marca.Value = this.txtVEMarcaCod2.Text;
                    codeudor2New.VE_Clase.Value = this.txtVEClaseCod2.Text;
                    codeudor2New.VE_Modelo.Value = Convert.ToInt16(this.txtVEModeloCod2.EditValue);
                    codeudor2New.VE_Placa.Value = this.txtVEPlacaCod2.Text;
                    codeudor2New.VE_PignoradoInd.Value = Convert.ToBoolean(this.chkVEPignoradoCod2.Checked);
                    codeudor2New.VE_PignoradoNombre.Value = this.txtVEPignoradoNombreCod2.Text;
                    codeudor2New.VE_Valor.Value = Convert.ToDecimal(this.txtVEValorCod2.EditValue);
                    #endregion

                    #region informacion Financiera
                    codeudor2New.VlrActivos.Value = 0;
                    codeudor2New.VlrPasivos.Value = 0;
                    codeudor2New.VlrPatrimonio.Value = 0;
                    codeudor2New.VlrEgresosMes.Value = 0;
                    codeudor2New.VlrIngresosMes.Value = 0;
                    codeudor2New.VlrIngresosNoOpe.Value = 0;
                    #endregion

                    codeudor2New.Consecutivo.Value = codeudor2 != null && codeudor2.Consecutivo.Value.HasValue ? codeudor2.Consecutivo.Value : null;
                    codeudor2New.Version.Value = codeudor2 != null ? codeudor2.Version.Value : 1;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 3);
                    this._data.DatosPersonales.Add(codeudor2New);
                    #endregion
                    #region codeudor3 (TipoPersona 4)
                    DTO_ccSolicitudDatosPersonales codeudor3New = new DTO_ccSolicitudDatosPersonales();

                    #region Datos Personales
                    codeudor3New.TipoPersona.Value = 4;

                    codeudor3New.ApellidoPri.Value = this.txtPriApellidoCod3.Text;
                    codeudor3New.ApellidoSdo.Value = this.txtSdoApellidoCod3.Text;
                    codeudor3New.NombrePri.Value = this.txtPriNombreCod3.Text;
                    codeudor3New.NombreSdo.Value = this.txtSdoNombreCod3.Text;
                    codeudor3New.FechaExpDoc.Value = Convert.ToDateTime(this.dtFechaExpCod3.DateTime);
                    codeudor3New.CiudadExpDoc.Value = this.masterCiudadExpCod3.Value;
                    codeudor3New.TerceroDocTipoID.Value = this.masterTerceroDocTipoCod3.Value;
                    codeudor3New.TerceroID.Value = this.txtCedulaCod3.Text;
                    codeudor3New.FechaNacimiento.Value = Convert.ToDateTime(this.dtFechaNacCod3.DateTime);
                    codeudor3New.Celular1.Value = this.txtCelular1Cod3.Text;
                    codeudor3New.Celular2.Value = this.txtCelular2Cod3.Text;
                    codeudor3New.CorreoElectronico.Value = this.txtCorreoCod3.Text;
                    codeudor3New.DirResidencia.Value = this.txtDireccionCod3.EditValue.ToString();
                    codeudor3New.BarrioResidencia.Value = this.txtBarrioResidenciaCod3.Text;
                    codeudor3New.CiudadResidencia.Value = this.masterCiudadResCod3.Value;
                    codeudor3New.AntResidencia.Value = Convert.ToByte(this.txtAntResidenciaCod3.EditValue);
                    codeudor3New.TipoVivienda.Value = Convert.ToByte(this.cmbTipoViviendaCod3.EditValue);
                    codeudor3New.TelResidencia.Value = this.txtTelResidenciaCod3.Text;
                    codeudor3New.LugarTrabajo.Value = this.txtLugarTrabajoCod3.Text;
                    codeudor3New.DirTrabajo.Value = this.txtDirTrabajoCod3.Text;
                    codeudor3New.BarrioTrabajo.Value = this.txtBarrioTrabajoCod3.Text;
                    codeudor3New.CiudadTrabajo.Value = this.masterCiudadTrabajoCod3.Value;
                    codeudor3New.TelTrabajo.Value = this.txtTelTrabajoCod3.Text;
                    codeudor3New.Cargo.Value = this.txtCargoCod3.Text;
                    codeudor3New.AntTrabajo.Value = Convert.ToByte(this.txtAntTrabajoCod3.EditValue);
                    codeudor3New.TipoContrato.Value = Convert.ToByte(this.cmbTipoContratoCod3.EditValue);
                    codeudor3New.EPS.Value = this.txtEpsCod3.Text;
                    if (!string.IsNullOrEmpty(this.txtPersonasCargoCod3.EditValue.ToString()))
                        codeudor3New.Personascargo .Value = Convert.ToByte(this.txtPersonasCargoCod3.EditValue);

                    #endregion

                    #region Datos Conyugue
                    codeudor3New.NombreConyugue.Value = this.txtConyNombreCod3.Text;
                    codeudor3New.Conyugue.Value = this.txtConyDocumentoCod3.Text;
                    codeudor3New.ActConyugue.Value = this.txtConyActividadCod3.Text;
                    codeudor3New.AntConyugue.Value = Convert.ToByte(this.txtConyAntCod3.EditValue);
                    codeudor3New.EmpresaConyugue.Value = this.txtConyEmpresaCod3.Text;
                    codeudor3New.DirResConyugue.Value = this.txtConyDirResidenciaCod3.Text;
                    codeudor3New.TelefonoConyugue.Value = this.txtConyTelefonoCod3.Text;
                    codeudor3New.CelularConyugue.Value = this.txtConyCelularCod3.Text;
                    #endregion

                    #region Referencias
                    codeudor3New.TipoReferencia1.Value = Convert.ToByte(this.cmbTipoReferencia1Cod3.EditValue);
                    codeudor3New.NombreReferencia1.Value = this.txtNombreReferencia1Cod3.Text;
                    codeudor3New.RelReferencia1.Value = this.txtParentescoReferencia1Cod3.Text;
                    codeudor3New.DirReferencia1.Value = this.txtDireccionReferencia1Cod3.Text;
                    codeudor3New.BarrioReferencia1.Value = this.txtBarrioReferencia1Cod3.Text;
                    codeudor3New.TelefonoReferencia1.Value = this.txtTelefonoReferencia1Cod3.Text;
                    codeudor3New.CelularReferencia1.Value = this.txtCelularReferencia1Cod3.Text;
                    codeudor3New.TipoReferencia2.Value = Convert.ToByte(this.cmbTipoReferencia2Cod3.EditValue);
                    codeudor3New.NombreReferencia2.Value = this.txtNombreReferencia2Cod3.Text;
                    codeudor3New.RelReferencia2.Value = this.txtParentescoReferencia2Cod3.Text;
                    codeudor3New.DirReferencia2.Value = this.txtDireccionReferencia2Cod3.Text;
                    codeudor3New.BarrioReferencia2.Value = this.txtBarrioReferencia2Cod3.Text;
                    codeudor3New.TelefonoReferencia2.Value = this.txtTelefonoReferencia2Cod3.Text;
                    codeudor3New.CelularReferencia2.Value = this.txtCelularReferencia2Cod3.Text;
                    #endregion

                    #region Bienes Raices
                    codeudor3New.BR_Direccion.Value = this.txtBRDireccionCod3.Text;
                    codeudor3New.BR_Valor.Value = Convert.ToDecimal(this.txtBRValorCod3.EditValue);
                    codeudor3New.BR_AfectacionFamInd.Value = Convert.ToBoolean(this.chkBRAfectacionCod3.Checked);
                    codeudor3New.BR_HipotecaInd.Value = Convert.ToBoolean(this.chkBRHipotecaCod3.Checked);
                    codeudor3New.BR_HipotecaNombre.Value = this.txtBRHipotecaNombreCod3.Text;
                    #endregion

                    #region Vehiculos
                    codeudor3New.VE_Marca.Value = this.txtVEMarcaCod3.Text;
                    codeudor3New.VE_Clase.Value = this.txtVEClaseCod3.Text;
                    codeudor3New.VE_Modelo.Value = Convert.ToInt16(this.txtVEModeloCod3.EditValue);
                    codeudor3New.VE_Placa.Value = this.txtVEPlacaCod3.Text;
                    codeudor3New.VE_PignoradoInd.Value = Convert.ToBoolean(this.chkVEPignoradoCod3.Checked);
                    codeudor3New.VE_PignoradoNombre.Value = this.txtVEPignoradoNombreCod3.Text;
                    codeudor3New.VE_Valor.Value = Convert.ToDecimal(this.txtVEValorCod3.EditValue);
                    #endregion

                    #region informacion Financiera
                    codeudor3New.VlrActivos.Value = 0;
                    codeudor3New.VlrPasivos.Value = 0;
                    codeudor3New.VlrPatrimonio.Value = 0;
                    codeudor3New.VlrEgresosMes.Value = 0;
                    codeudor3New.VlrIngresosMes.Value = 0;
                    codeudor3New.VlrIngresosNoOpe.Value = 0;
                    #endregion


                    codeudor3New.Consecutivo.Value = codeudor3 != null && codeudor3.Consecutivo.Value.HasValue ? codeudor3.Consecutivo.Value : null;
                    codeudor3New.Version.Value = codeudor3 != null ? codeudor3.Version.Value : 1;
                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 4);
                    this._data.DatosPersonales.Add(codeudor3New);
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "AssignValues"));
            }
        }

        /// <summary>
        /// Carga la informacion principal de la solicitud
        /// </summary>
        /// <param name="libranzaNro">Libranza</param>
        private void LoadData( int libranzaNro)
        {
            this._isLoaded = false;
            this._data = _bc.AdministrationModel.RegistroSolicitud_GetBySolicitud(libranzaNro);

            if (this._data != null)
            {
                #region Solicitud existente
                this._ctrl = this._data.DocCtrl;

                if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)                  
                {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "Form_FormClosed"));
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
            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 1;
            else if (this.tabControl.SelectedTabPageIndex == 1)
                this.tabControl.SelectedTabPageIndex = 2;
            else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 3;
            else if (this.tabControl.SelectedTabPageIndex == 3)
                this.tabControl.SelectedTabPageIndex = 0;
        }

        /// <summary>
        /// Atras
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e"><Objeto que envia el evento/param>
        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 3;
            else if (this.tabControl.SelectedTabPageIndex == 1)
                this.tabControl.SelectedTabPageIndex = 0;
            else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 1;
            else if (this.tabControl.SelectedTabPageIndex == 3)
                this.tabControl.SelectedTabPageIndex = 2;
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor1.Dock == DockStyle.Fill)
            {
                linkCodeudor1.Dock = DockStyle.None;
                linkConyCodeudor1.Dock = DockStyle.None;
                linkInmuebleCodeudor1.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor1.Dock = DockStyle.Fill;
                linkConyCodeudor1.Dock = DockStyle.Fill;
                linkInmuebleCodeudor1.Dock = DockStyle.Fill;
            }
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor2.Dock == DockStyle.Fill)
            {
                linkCodeudor2.Dock = DockStyle.None;
                linkConyCodeudor2.Dock = DockStyle.None;
                linkInmuebleCodeudor2.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor2.Dock = DockStyle.Fill;
                linkConyCodeudor2.Dock = DockStyle.Fill;
                linkInmuebleCodeudor2.Dock = DockStyle.Fill;
            }

        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor3.Dock == DockStyle.Fill)
            {
                linkCodeudor3.Dock = DockStyle.None;
                linkConyCodeudor3.Dock = DockStyle.None;
                linkInmuebleCodeudor3.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor3.Dock = DockStyle.Fill;
                linkConyCodeudor3.Dock = DockStyle.Fill;
                linkInmuebleCodeudor3.Dock = DockStyle.Fill;
            }
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkConyCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkConyCodeudor1.Dock == DockStyle.Fill)
                linkConyCodeudor1.Dock = DockStyle.None;
            else
                linkConyCodeudor1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkConyCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkConyCodeudor2.Dock == DockStyle.Fill)
                linkConyCodeudor2.Dock = DockStyle.None;
            else
                linkConyCodeudor2.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkConyCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkConyCodeudor3.Dock == DockStyle.Fill)
                linkConyCodeudor3.Dock = DockStyle.None;
            else
                linkConyCodeudor3.Dock = DockStyle.Fill;

        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkInmuebleCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkInmuebleCodeudor1.Dock == DockStyle.Fill)
                linkInmuebleCodeudor1.Dock = DockStyle.None;
            else
                linkInmuebleCodeudor1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkInmuebleCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkInmuebleCodeudor2.Dock == DockStyle.Fill)
                linkInmuebleCodeudor2.Dock = DockStyle.None;
            else
                linkInmuebleCodeudor2.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Al dar clic y digitar la info de cada persona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkInmuebleCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkInmuebleCodeudor3.Dock == DockStyle.Fill)
                linkInmuebleCodeudor3.Dock = DockStyle.None;
            else
                linkInmuebleCodeudor3.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Permite crear los clientes en base a la info suministrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearClientes_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._data.DatosPersonales.Count > 0)
                {
                    DTO_TxResult result = new DTO_TxResult();
                    DTO_TxResultDetail res = this._bc.AdministrationModel.ccCliente_AddFromSource(this._documentID, this._data);
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Details.Add(res);
                    if (res.Message == ResultValue.OK.ToString())
                        result.Result = ResultValue.OK;
                    else
                        result.Result = ResultValue.NOK;

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "btnCrearClientes_Click"));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtActivos_EditValueChanged(object sender, EventArgs e)
        {            
            this.txtPatrimonio.EditValue = Convert.ToDecimal(this.txtActivos.EditValue, CultureInfo.InvariantCulture) - Convert.ToDecimal(this.txtPasivos.EditValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPasivos_EditValueChanged(object sender, EventArgs e)
        {
            this.txtPatrimonio.EditValue = Convert.ToDecimal(this.txtActivos.EditValue, CultureInfo.InvariantCulture) - Convert.ToDecimal(this.txtPasivos.EditValue, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Button para validar la direccion
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void txtDireccionDeudor_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            CodificacionDireccion dir = new CodificacionDireccion((DevExpress.XtraEditors.ButtonEdit)sender);
            dir.ShowDialog();
        }

        /// <summary>
        /// Al salir control asigna datos
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void txtCedulaCod1_Leave(object sender, EventArgs e)
        {
            try
            {
                this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value ?? 1;
                DTO_ccSolicitudDatosPersonales codeudor1 = this._bc.AdministrationModel.RegistroSolicitud_GetByCedula(this.txtCedulaDeudor.Text, this.txtCedulaCod1.Text, 2, this._data.SolicituDocu.VersionNro.Value.Value);
                if (codeudor1 != null)
                {
                    #region Datos Personales
                    this.txtCedulaCod1.Text = codeudor1.TerceroID.Value;
                    this.txtPriApellidoCod1.Text = codeudor1.ApellidoPri.Value;
                    this.txtSdoApellidoCod1.Text = codeudor1.ApellidoSdo.Value;
                    this.txtPriNombreCod1.Text = codeudor1.NombrePri.Value;
                    this.txtSdoNombreCod1.Text = codeudor1.NombreSdo.Value;
                    this.dtFechaExpCod1.DateTime = codeudor1.FechaExpDoc.Value.HasValue ? codeudor1.FechaExpDoc.Value.Value : this.dtFechaExpCod1.DateTime;
                    this.masterCiudadExpCod1.Value = codeudor1.CiudadExpDoc.Value;
                    this.masterTerceroDocTipoCod1.Value = codeudor1.TerceroDocTipoID.Value;
                    this.dtFechaNacCod1.DateTime = codeudor1.FechaNacimiento.Value.HasValue ? codeudor1.FechaNacimiento.Value.Value : this.dtFechaNacCod1.DateTime;
                    this.txtCelular1Cod1.Text = codeudor1.Celular1.Value;
                    this.txtCelular2Cod1.Text = codeudor1.Celular2.Value;
                    this.txtCorreoCod1.Text = codeudor1.CorreoElectronico.Value;
                    this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                    this.txtBarrioResidenciaCod1.Text = codeudor1.BarrioResidencia.Value;
                    this.masterCiudadResCod1.Value = codeudor1.CiudadResidencia.Value;
                    this.txtAntResidenciaCod1.EditValue = codeudor1.AntResidencia.Value.ToString();
                    this.cmbTipoViviendaCod1.EditValue = codeudor1.TipoVivienda.Value.ToString();
                    this.txtTelResidenciaCod1.Text = codeudor1.TelResidencia.Value;
                    this.txtDireccionCod1.EditValue = codeudor1.DirResidencia.Value;
                    this.txtLugarTrabajoCod1.Text = codeudor1.LugarTrabajo.Value;
                    this.txtDirTrabajoCod1.Text = codeudor1.DirTrabajo.Value;
                    this.txtBarrioTrabajoCod1.Text = codeudor1.BarrioTrabajo.Value;
                    this.masterCiudadTrabajoCod1.Value = codeudor1.CiudadTrabajo.Value;
                    this.txtTelTrabajoCod1.Text = codeudor1.TelTrabajo.Value;
                    this.txtCargoCod1.Text = codeudor1.Cargo.Value;
                    this.txtAntTrabajoCod1.EditValue = codeudor1.AntTrabajo.Value.ToString();
                    this.cmbTipoContratoCod1.EditValue = codeudor1.TipoContrato.Value.ToString();
                    this.txtEpsCod1.Text = codeudor1.EPS.Value;
                    this.txtPersonasCargoCod1.EditValue = codeudor1.Personascargo.Value.ToString();
                    #endregion

                    #region Datos Conyugue
                    this.txtConyNombreCod1.Text = codeudor1.NombreConyugue.Value;
                    this.txtConyDocumentoCod1.Text = codeudor1.Conyugue.Value;
                    this.txtConyActividadCod1.Text = codeudor1.ActConyugue.Value;
                    this.txtConyAntCod1.Text = codeudor1.AntConyugue.Value.ToString();
                    this.txtConyEmpresaCod1.Text = codeudor1.EmpresaConyugue.Value.ToString();
                    this.txtConyDirResidenciaCod1.Text = codeudor1.DirResConyugue.Value;
                    this.txtConyTelefonoCod1.Text = codeudor1.TelefonoConyugue.Value;
                    this.txtConyCelularCod1.Text = codeudor1.CelularConyugue.Value;
                    #endregion

                    #region Referencias
                    this.cmbTipoReferencia1Cod1.EditValue = codeudor1.TipoReferencia1.Value.ToString();
                    this.txtNombreReferencia1Cod1.Text = codeudor1.NombreReferencia1.Value;
                    this.txtParentescoReferencia1Cod1.Text = codeudor1.RelReferencia1.Value;
                    this.txtDireccionReferencia1Cod1.Text = codeudor1.DirReferencia1.Value;
                    this.txtBarrioReferencia1Cod1.Text = codeudor1.BarrioReferencia1.Value;
                    this.txtTelefonoReferencia1Cod1.Text = codeudor1.TelefonoReferencia1.Value;
                    this.txtCelularReferencia1Cod1.Text = codeudor1.CelularReferencia1.Value;

                    this.cmbTipoReferencia2Cod1.EditValue = codeudor1.TipoReferencia2.Value.ToString();
                    this.txtNombreReferencia2Cod1.Text = codeudor1.NombreReferencia2.Value;
                    this.txtParentescoReferencia2Cod1.Text = codeudor1.RelReferencia2.Value;
                    this.txtDireccionReferencia2Cod1.Text = codeudor1.DirReferencia2.Value;
                    this.txtBarrioReferencia2Cod1.Text = codeudor1.BarrioReferencia2.Value;
                    this.txtTelefonoReferencia2Cod1.Text = codeudor1.TelefonoReferencia2.Value;
                    this.txtCelularReferencia2Cod1.Text = codeudor1.CelularReferencia2.Value;
                    #endregion

                    #region Bienes Raices
                    this.txtBRDireccionCod1.Text = codeudor1.BR_Direccion.Value;
                    this.txtBRValorCod1.EditValue = codeudor1.BR_Valor.Value.ToString();
                    this.chkBRAfectacionCod1.Checked = codeudor1.BR_AfectacionFamInd.Value.HasValue ? codeudor1.BR_AfectacionFamInd.Value.Value : false;
                    this.chkBRHipotecaCod1.Checked = codeudor1.BR_HipotecaInd.Value.HasValue ? codeudor1.BR_HipotecaInd.Value.Value : false;
                    this.txtBRHipotecaNombreCod1.Text = codeudor1.BR_HipotecaNombre.Value;
                    #endregion

                    #region Vehiculos
                    this.txtVEMarcaCod1.Text = codeudor1.VE_Marca.Value;
                    this.txtVEClaseCod1.Text = codeudor1.VE_Clase.Value;
                    this.txtVEModeloCod1.EditValue = codeudor1.VE_Modelo.Value.ToString();
                    this.txtVEPlacaCod1.Text = codeudor1.VE_Placa.Value;
                    this.chkVEPignoradoCod1.Checked = codeudor1.VE_PignoradoInd.Value.HasValue ? codeudor1.VE_PignoradoInd.Value.Value : false;
                    this.txtVEPignoradoNombreCod1.Text = codeudor1.VE_PignoradoNombre.Value;
                    this.txtVEValorCod1.EditValue = codeudor1.VE_Valor.Value.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "txtCedulaCod1_Leave"));
            }
        }

        /// <summary>
        /// Al salir control asigna datos
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void txtCedulaCod2_Leave(object sender, EventArgs e)
        {
            try
            {
                this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value ?? 1;
                DTO_ccSolicitudDatosPersonales codeudor2 = this._bc.AdministrationModel.RegistroSolicitud_GetByCedula(this.txtCedulaDeudor.Text, this.txtCedulaCod2.Text, 3, this._data.SolicituDocu.VersionNro.Value.Value);
                if (codeudor2 != null)
                {
                    #region Datos Personales
                    this.txtCedulaCod2.Text = codeudor2.TerceroID.Value;
                    this.txtPriApellidoCod2.Text = codeudor2.ApellidoPri.Value;
                    this.txtSdoApellidoCod2.Text = codeudor2.ApellidoSdo.Value;
                    this.txtPriNombreCod2.Text = codeudor2.NombrePri.Value;
                    this.txtSdoNombreCod2.Text = codeudor2.NombreSdo.Value;
                    this.dtFechaExpCod2.DateTime = codeudor2.FechaExpDoc.Value.HasValue ? codeudor2.FechaExpDoc.Value.Value : this.dtFechaExpCod2.DateTime;
                    this.masterTerceroDocTipoCod2.Value = codeudor2.TerceroDocTipoID.Value;
                    this.dtFechaNacCod2.DateTime = codeudor2.FechaNacimiento.Value.HasValue ? codeudor2.FechaNacimiento.Value.Value : this.dtFechaNacCod2.DateTime;
                    this.txtCelular1Cod2.Text = codeudor2.Celular1.Value;
                    this.txtCelular2Cod2.Text = codeudor2.Celular2.Value;
                    this.txtCorreoCod2.Text = codeudor2.CorreoElectronico.Value;
                    this.txtDireccionCod2.EditValue = codeudor2.DirResidencia.Value;
                    this.txtBarrioResidenciaCod2.Text = codeudor2.BarrioResidencia.Value;
                    this.masterCiudadResCod2.Value = codeudor2.CiudadResidencia.Value;
                    this.txtAntResidenciaCod2.EditValue = codeudor2.AntResidencia.Value.ToString();
                    this.cmbTipoViviendaCod2.EditValue = codeudor2.TipoVivienda.Value.ToString();
                    this.txtTelResidenciaCod2.Text = codeudor2.TelResidencia.Value;
                    this.txtDireccionCod2.EditValue = codeudor2.DirResidencia.Value;
                    this.txtLugarTrabajoCod2.Text = codeudor2.LugarTrabajo.Value;
                    this.txtDirTrabajoCod2.Text = codeudor2.DirTrabajo.Value;
                    this.txtBarrioTrabajoCod2.Text = codeudor2.BarrioTrabajo.Value;
                    this.masterCiudadTrabajoCod2.Value = codeudor2.CiudadTrabajo.Value;
                    this.txtTelTrabajoCod2.Text = codeudor2.TelTrabajo.Value;
                    this.txtCargoCod2.Text = codeudor2.Cargo.Value;
                    this.txtAntTrabajoCod2.EditValue = codeudor2.AntTrabajo.Value.ToString();
                    this.cmbTipoContratoCod2.EditValue = codeudor2.TipoContrato.Value.ToString();
                    this.txtEpsCod2.Text = codeudor2.EPS.Value;
                    this.txtPersonasCargoCod2.EditValue = codeudor2.Personascargo.Value.ToString();
                    #endregion

                    #region Datos Conyugue
                    this.txtConyNombreCod2.Text = codeudor2.NombreConyugue.Value;
                    this.txtConyDocumentoCod2.Text = codeudor2.Conyugue.Value;
                    this.txtConyActividadCod2.Text = codeudor2.ActConyugue.Value;
                    this.txtConyAntCod2.Text = codeudor2.AntConyugue.Value.ToString();
                    this.txtConyEmpresaCod2.Text = codeudor2.EmpresaConyugue.Value.ToString();
                    this.txtConyDirResidenciaCod2.Text = codeudor2.DirResConyugue.Value;
                    this.txtConyTelefonoCod2.Text = codeudor2.TelefonoConyugue.Value;
                    this.txtConyCelularCod2.Text = codeudor2.CelularConyugue.Value;
                    #endregion

                    #region Referencias
                    this.cmbTipoReferencia1Cod2.EditValue = codeudor2.TipoReferencia1.Value.ToString();
                    this.txtNombreReferencia1Cod2.Text = codeudor2.NombreReferencia1.Value;
                    this.txtParentescoReferencia1Cod2.Text = codeudor2.RelReferencia1.Value;
                    this.txtDireccionReferencia1Cod2.Text = codeudor2.DirReferencia1.Value;
                    this.txtBarrioReferencia1Cod2.Text = codeudor2.BarrioReferencia1.Value;
                    this.txtTelefonoReferencia1Cod2.Text = codeudor2.TelefonoReferencia1.Value;
                    this.txtCelularReferencia1Cod2.Text = codeudor2.CelularReferencia1.Value;

                    this.cmbTipoReferencia2Cod2.EditValue = codeudor2.TipoReferencia2.Value.ToString();
                    this.txtNombreReferencia2Cod2.Text = codeudor2.NombreReferencia2.Value;
                    this.txtParentescoReferencia2Cod2.Text = codeudor2.RelReferencia2.Value;
                    this.txtDireccionReferencia2Cod2.Text = codeudor2.DirReferencia2.Value;
                    this.txtBarrioReferencia2Cod2.Text = codeudor2.BarrioReferencia2.Value;
                    this.txtTelefonoReferencia2Cod2.Text = codeudor2.TelefonoReferencia2.Value;
                    this.txtCelularReferencia2Cod2.Text = codeudor2.CelularReferencia2.Value;
                    #endregion

                    #region Bienes Raices
                    this.txtBRDireccionCod2.Text = codeudor2.BR_Direccion.Value;
                    this.txtBRValorCod2.EditValue = codeudor2.BR_Valor.Value.ToString();
                    this.chkBRAfectacionCod2.Checked = codeudor2.BR_AfectacionFamInd.Value.HasValue ? codeudor2.BR_AfectacionFamInd.Value.Value : false;
                    this.chkBRHipotecaCod2.Checked = codeudor2.BR_HipotecaInd.Value.HasValue ? codeudor2.BR_HipotecaInd.Value.Value : false;
                    this.txtBRHipotecaNombreCod2.Text = codeudor2.BR_HipotecaNombre.Value;
                    #endregion

                    #region Vehiculos
                    this.txtVEMarcaCod2.Text = codeudor2.VE_Marca.Value;
                    this.txtVEClaseCod2.Text = codeudor2.VE_Clase.Value;
                    this.txtVEModeloCod2.EditValue = codeudor2.VE_Modelo.Value.ToString();
                    this.txtVEPlacaCod2.Text = codeudor2.VE_Placa.Value;
                    this.chkVEPignoradoCod2.Checked = codeudor2.VE_PignoradoInd.Value.HasValue ? codeudor2.VE_PignoradoInd.Value.Value : false;
                    this.txtVEPignoradoNombreCod2.Text = codeudor2.VE_PignoradoNombre.Value;
                    this.txtVEValorCod2.EditValue = codeudor2.VE_Valor.Value.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "txtCedulaCod2_Leave"));
            }
        }

        /// <summary>
        /// Al salir control asigna datos
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void txtCedulaCod3_Leave(object sender, EventArgs e)
        {
            try
            {
                this._data.SolicituDocu.VersionNro.Value = this._data.SolicituDocu.VersionNro.Value ?? 1;
                DTO_ccSolicitudDatosPersonales codeudor3 = this._bc.AdministrationModel.RegistroSolicitud_GetByCedula(this.txtCedulaDeudor.Text, this.txtCedulaCod3.Text, 4, this._data.SolicituDocu.VersionNro.Value.Value);
                if (codeudor3 != null)
                {
                    #region Datos Personales
                    this.txtCedulaCod3.Text = codeudor3.TerceroID.Value;
                    this.txtPriApellidoCod3.Text = codeudor3.ApellidoPri.Value;
                    this.txtSdoApellidoCod3.Text = codeudor3.ApellidoSdo.Value;
                    this.txtPriNombreCod3.Text = codeudor3.NombrePri.Value;
                    this.txtSdoNombreCod3.Text = codeudor3.NombreSdo.Value;
                    this.dtFechaExpCod3.DateTime = codeudor3.FechaExpDoc.Value.HasValue ? codeudor3.FechaExpDoc.Value.Value : this.dtFechaExpCod3.DateTime;
                    this.masterCiudadExpCod3.Value = codeudor3.CiudadExpDoc.Value;
                    this.masterTerceroDocTipoCod3.Value = codeudor3.TerceroDocTipoID.Value;
                    this.dtFechaNacCod3.DateTime = codeudor3.FechaNacimiento.Value.HasValue ? codeudor3.FechaNacimiento.Value.Value : this.dtFechaNacCod3.DateTime;
                    this.txtCelular1Cod3.Text = codeudor3.Celular1.Value;
                    this.txtCelular2Cod3.Text = codeudor3.Celular2.Value;
                    this.txtCorreoCod3.Text = codeudor3.CorreoElectronico.Value;
                    this.txtDireccionCod3.EditValue = codeudor3.DirResidencia.Value;
                    this.txtBarrioResidenciaCod3.Text = codeudor3.BarrioResidencia.Value;
                    this.masterCiudadResCod3.Value = codeudor3.CiudadResidencia.Value;
                    this.txtAntResidenciaCod3.EditValue = codeudor3.AntResidencia.Value.ToString();
                    this.cmbTipoViviendaCod3.EditValue = codeudor3.TipoVivienda.Value.ToString();
                    this.txtTelResidenciaCod3.Text = codeudor3.TelResidencia.Value;
                    this.txtDireccionCod3.EditValue = codeudor3.DirResidencia.Value;
                    this.txtLugarTrabajoCod3.Text = codeudor3.LugarTrabajo.Value;
                    this.txtDirTrabajoCod3.Text = codeudor3.DirTrabajo.Value;
                    this.txtBarrioTrabajoCod3.Text = codeudor3.BarrioTrabajo.Value;
                    this.masterCiudadTrabajoCod3.Value = codeudor3.CiudadTrabajo.Value;
                    this.txtTelTrabajoCod3.Text = codeudor3.TelTrabajo.Value;
                    this.txtCargoCod3.Text = codeudor3.Cargo.Value;
                    this.txtAntTrabajoCod3.EditValue = codeudor3.AntTrabajo.Value.ToString();
                    this.cmbTipoContratoCod3.EditValue = codeudor3.TipoContrato.Value.ToString();
                    this.txtEpsCod3.Text = codeudor3.EPS.Value;
                    this.txtPersonasCargoCod3.EditValue = codeudor3.Personascargo.Value.ToString();
                    #endregion

                    #region Datos Conyugue
                    this.txtConyNombreCod3.Text = codeudor3.NombreConyugue.Value;
                    this.txtConyDocumentoCod3.Text = codeudor3.Conyugue.Value;
                    this.txtConyActividadCod3.Text = codeudor3.ActConyugue.Value;
                    this.txtConyAntCod3.Text = codeudor3.AntConyugue.Value.ToString();
                    this.txtConyEmpresaCod3.Text = codeudor3.EmpresaConyugue.Value.ToString();
                    this.txtConyDirResidenciaCod3.Text = codeudor3.DirResConyugue.Value;
                    this.txtConyTelefonoCod3.Text = codeudor3.TelefonoConyugue.Value;
                    this.txtConyCelularCod3.Text = codeudor3.CelularConyugue.Value;
                    #endregion

                    #region Referencias
                    this.cmbTipoReferencia1Cod3.EditValue = codeudor3.TipoReferencia1.Value.ToString();
                    this.txtNombreReferencia1Cod3.Text = codeudor3.NombreReferencia1.Value;
                    this.txtParentescoReferencia1Cod3.Text = codeudor3.RelReferencia1.Value;
                    this.txtDireccionReferencia1Cod3.Text = codeudor3.DirReferencia1.Value;
                    this.txtBarrioReferencia1Cod3.Text = codeudor3.BarrioReferencia1.Value;
                    this.txtTelefonoReferencia1Cod3.Text = codeudor3.TelefonoReferencia1.Value;
                    this.txtCelularReferencia1Cod3.Text = codeudor3.CelularReferencia1.Value;

                    this.cmbTipoReferencia2Cod3.EditValue = codeudor3.TipoReferencia2.Value.ToString();
                    this.txtNombreReferencia2Cod3.Text = codeudor3.NombreReferencia2.Value;
                    this.txtParentescoReferencia2Cod3.Text = codeudor3.RelReferencia2.Value;
                    this.txtDireccionReferencia2Cod3.Text = codeudor3.DirReferencia2.Value;
                    this.txtBarrioReferencia2Cod3.Text = codeudor3.BarrioReferencia2.Value;
                    this.txtTelefonoReferencia2Cod3.Text = codeudor3.TelefonoReferencia2.Value;
                    this.txtCelularReferencia2Cod3.Text = codeudor3.CelularReferencia2.Value;
                    #endregion

                    #region Bienes Raices
                    this.txtBRDireccionCod3.Text = codeudor3.BR_Direccion.Value;
                    this.txtBRValorCod3.EditValue = codeudor3.BR_Valor.Value.ToString();
                    this.chkBRAfectacionCod3.Checked = codeudor3.BR_AfectacionFamInd.Value.HasValue ? codeudor3.BR_AfectacionFamInd.Value.Value : false;
                    this.chkBRHipotecaCod3.Checked = codeudor3.BR_HipotecaInd.Value.HasValue ? codeudor3.BR_HipotecaInd.Value.Value : false;
                    this.txtBRHipotecaNombreCod3.Text = codeudor3.BR_HipotecaNombre.Value;
                    #endregion

                    #region Vehiculos
                    this.txtVEMarcaCod3.Text = codeudor3.VE_Marca.Value;
                    this.txtVEClaseCod3.Text = codeudor3.VE_Clase.Value;
                    this.txtVEModeloCod3.EditValue = codeudor3.VE_Modelo.Value.ToString();
                    this.txtVEPlacaCod3.Text = codeudor3.VE_Placa.Value;
                    this.chkVEPignoradoCod3.Checked = codeudor3.VE_PignoradoInd.Value.HasValue ? codeudor3.VE_PignoradoInd.Value.Value : false;
                    this.txtVEPignoradoNombreCod3.Text = codeudor3.VE_PignoradoNombre.Value;
                    this.txtVEValorCod3.EditValue = codeudor3.VE_Valor.Value.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "txtCedulaCod3_Leave"));
            }
        }

        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this._data.DatosPersonales.RemoveAll(x=>!x.Consecutivo.Value.HasValue);
                this.AssignValues(false);
                this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                if (this.ValidateHeader())
                {
                    DTO_TxResult result = _bc.AdministrationModel.RegistroSolicitud_Add(this._documentID,this._actFlujo.ID.Value,this._data,false);
                    if (result.Result == ResultValue.OK)
                    {
                        this.txtCedulaDeudor.Focus();
                    }
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
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
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar el registro de la solicitud?  ");
                if (MessageBox.Show(msgDoc,"Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                this.AssignValues(false);
                this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
                if (this.ValidateHeader())
                {
                    DTO_TxResult result = _bc.AdministrationModel.RegistroSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, true);
                  
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
            }
        }


        #endregion Eventos Barra Herramientas  
    }
}