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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Drawing;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Datacredito : FormWithToolbar
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
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private List<DTO_glDocumentoChequeoLista> _actividadChequeo = new List<DTO_glDocumentoChequeoLista>();
        private List<DTO_drActividadChequeoLista> actChequeoBase = new List<DTO_drActividadChequeoLista>();
        private List<DTO_drSolicitudDatosChequeados> actChequeo = new List<DTO_drSolicitudDatosChequeados>();
        //Identificador de la proxima actividad
        private string nextActID;

        //Variables formulario
        private DateTime periodo = DateTime.Now;
        Dictionary<string, string> carta = new Dictionary<string, string>();
        protected int documentReportID = 0;

        private string _clienteID = String.Empty;
        private string _clienteDesc = String.Empty;

        private string _conyugueID = String.Empty;
        private string _conyugueDesc = String.Empty;

        private string _codeudor1ID = String.Empty;
        private string _codeudor1Desc = String.Empty;

        private string _codeudor2ID = String.Empty;
        private string _codeudor2Desc = String.Empty;

        private string _codeudor3ID = String.Empty;
        private string _codeudor3Desc = String.Empty;
        private int _libranza=0;

        private bool _isLoaded;
        private bool _mensajeGuardar=true;
        private bool _readOnly = false;

        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();
        private Dictionary<string, string> tipo = null;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Datacredito()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Datacredito(int libranza, bool readOnly)
        {

            this._libranza = libranza;
            InitializeComponent();
            this.documentReportID = AppReports.drAprobacionDirectaConDoc;
            this.lblCarta.Text = "APROBACION DIRECTA SUJETA A DOCUMENTOS";
            //this.tabControl.SelectedTabPageIndex = 2;
            try
            {
                this.SetInitParameters();
                this.groupControl5.Visible = false;
                this.groupControl6.Visible = false;
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
                this._readOnly = readOnly;                
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
        public void Constructor(int libranzaNro = 0)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.dr;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);
                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (this._frmModule == ModulesPrefix.dr && actividades.Count > 0)
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
                this.LoadData(libranzaNro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "Constructor"));
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
                this._documentID = AppDocuments.Revision1;
                this._frmModule = ModulesPrefix.dr;

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;

                this.linkDatosConyuge.Dock = DockStyle.Fill;
                this.linkDatosCodeudor1.Dock = DockStyle.Fill;
                this.linkDatosCodeudor2.Dock = DockStyle.Fill;
                this.linkDatosCodeudor3.Dock = DockStyle.Fill;

                //Determina el nombre del combo y el item donde debe quedar
                
                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;

                this.tipo = new Dictionary<string, string>();
                this.tipo.Add("RES", this._bc.GetResource(LanguageTypes.Tables, "Residencia"));
                this.tipo.Add("LAB", this._bc.GetResource(LanguageTypes.Tables, "Laboral"));

                
                #region direccion Digita
                this.cmbTipoDireccionDigitaDeudor.Properties.ValueMember = "Key";
                this.cmbTipoDireccionDigitaDeudor.Properties.DisplayMember = "Value";
                this.cmbTipoDireccionDigitaDeudor.Properties.DataSource = tipo;
                this.cmbTipoDireccionDigitaDeudor.EditValue = "0";

                this.cmbTipoDireccionDigitaCony.Properties.ValueMember = "Key";
                this.cmbTipoDireccionDigitaCony.Properties.DisplayMember = "Value";
                this.cmbTipoDireccionDigitaCony.Properties.DataSource = tipo;
                this.cmbTipoDireccionDigitaCony.EditValue = "0";

                this.cmbTipoDireccionDigitaCod1.Properties.ValueMember = "Key";
                this.cmbTipoDireccionDigitaCod1.Properties.DisplayMember = "Value";
                this.cmbTipoDireccionDigitaCod1.Properties.DataSource = tipo;
                this.cmbTipoDireccionDigitaCod1.EditValue = "0";

                this.cmbTipoDireccionDigitaCod2.Properties.ValueMember = "Key";
                this.cmbTipoDireccionDigitaCod2.Properties.DisplayMember = "Value";
                this.cmbTipoDireccionDigitaCod2.Properties.DataSource = tipo;
                this.cmbTipoDireccionDigitaCod2.EditValue = "0";

                this.cmbTipoDireccionDigitaCod3.Properties.ValueMember = "Key";
                this.cmbTipoDireccionDigitaCod3.Properties.DisplayMember = "Value";
                this.cmbTipoDireccionDigitaCod3.Properties.DataSource = tipo;
                this.cmbTipoDireccionDigitaCod3.EditValue = "0";


                #endregion

                #region Telefono Digita
                this.cmbTipoTelefonoDigitaDeudor.Properties.ValueMember = "Key";
                this.cmbTipoTelefonoDigitaDeudor.Properties.DisplayMember = "Value";
                this.cmbTipoTelefonoDigitaDeudor.Properties.DataSource = tipo;
                this.cmbTipoTelefonoDigitaDeudor.EditValue = "0";

                this.cmbTipoTelefonoDigitaCony.Properties.ValueMember = "Key";
                this.cmbTipoTelefonoDigitaCony.Properties.DisplayMember = "Value";
                this.cmbTipoTelefonoDigitaCony.Properties.DataSource = tipo;
                this.cmbTipoTelefonoDigitaCony.EditValue = "0";

                this.cmbTipoTelefonoDigitaCod1.Properties.ValueMember = "Key";
                this.cmbTipoTelefonoDigitaCod1.Properties.DisplayMember = "Value";
                this.cmbTipoTelefonoDigitaCod1.Properties.DataSource = tipo;
                this.cmbTipoTelefonoDigitaCod1.EditValue = "0";

                this.cmbTipoTelefonoDigitaCod2.Properties.ValueMember = "Key";
                this.cmbTipoTelefonoDigitaCod2.Properties.DisplayMember = "Value";
                this.cmbTipoTelefonoDigitaCod2.Properties.DataSource = tipo;
                this.cmbTipoTelefonoDigitaCod2.EditValue = "0";

                this.cmbTipoTelefonoDigitaCod3.Properties.ValueMember = "Key";
                this.cmbTipoTelefonoDigitaCod3.Properties.DisplayMember = "Value";
                this.cmbTipoTelefonoDigitaCod3.Properties.DataSource = tipo;
                this.cmbTipoTelefonoDigitaCod3.EditValue = "0";
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {

            //Variables
            this._clienteID = String.Empty;
            this._clienteDesc = String.Empty;
            this._conyugueID = String.Empty;
            this._conyugueDesc = String.Empty;

            this._codeudor1ID = String.Empty;
            this._codeudor1Desc = String.Empty;

            this._codeudor2ID = String.Empty;
            this._codeudor2Desc = String.Empty;

            this._codeudor3ID = String.Empty;
            this._codeudor3Desc = String.Empty;

            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            //this.txtPriApellidoDeudor.ReadOnly = !enabled;
            //this.txtSdoApellidoDeudor.ReadOnly = !enabled;
            //this.txtPriNombreDeudor.ReadOnly = !enabled;
            //this.txtSdoNombreDeudor.ReadOnly = !enabled;

            
            this.cmbActividades.Enabled = !this._readOnly;
            this.cmbTipoDireccionDigitaCod1.Enabled = !this._readOnly;
            this.cmbTipoDireccionDigitaCod2.Enabled = !this._readOnly;
            this.cmbTipoDireccionDigitaCod3.Enabled = !this._readOnly;
            this.cmbTipoDireccionDigitaCony.Enabled = !this._readOnly;
            this.cmbTipoDireccionDigitaDeudor.Enabled = !this._readOnly;
            this.cmbTipoTelefonoDigitaCod1.Enabled = !this._readOnly;
            this.cmbTipoTelefonoDigitaCod2.Enabled = !this._readOnly;
            this.cmbTipoTelefonoDigitaCod3.Enabled = !this._readOnly;
            this.cmbTipoTelefonoDigitaCony.Enabled = !this._readOnly;
            this.cmbTipoTelefonoDigitaDeudor.Enabled = !this._readOnly;
            this.txtDireccionDigitaCod1.Enabled = !this._readOnly;
            this.txtDireccionDigitaCod2.Enabled = !this._readOnly;
            this.txtDireccionDigitaCod3.Enabled = !this._readOnly;
            this.txtDireccionDigitaCony.Enabled = !this._readOnly;
            this.txtDireccionDigitaDeudor.Enabled = !this._readOnly;
            this.txtTelefonoDigitaCod1.Enabled = !this._readOnly;
            this.txtTelefonoDigitaCod2.Enabled = !this._readOnly;
            this.txtTelefonoDigitaCod3.Enabled = !this._readOnly;
            this.txtTelefonoDigitaCony.Enabled = !this._readOnly;
            this.txtTelefonoDigitaDeudor.Enabled = !this._readOnly;
            this.txtCorreoDigitaCod1.Enabled = !this._readOnly;
            this.txtCorreoDigitaCod2.Enabled = !this._readOnly;
            this.txtCorreoDigitaCod3.Enabled = !this._readOnly;
            this.txtCorreoDigitaCony.Enabled = !this._readOnly;
            this.txtCorreoDigitaDeudor.Enabled = !this._readOnly;
            this.txtCelularDigitaCod1.Enabled = !this._readOnly;
            this.txtCelularDigitaCod2.Enabled = !this._readOnly;
            this.txtCelularDigitaCod3.Enabled = !this._readOnly;
            this.txtCelularDigitaCony.Enabled = !this._readOnly;
            this.txtCelularDigitaDeudor.Enabled = !this._readOnly;
            this.chkDir1Cod1.Enabled = !this._readOnly;
            this.chkDir1Cod2.Enabled = !this._readOnly;
            this.chkDir1Cod3.Enabled = !this._readOnly;
            this.chkDir1Cony.Enabled = !this._readOnly;
            this.chkDir1Deudor.Enabled = !this._readOnly;
            this.chkDir2Cod1.Enabled = !this._readOnly;
            this.chkDir2Cod2.Enabled = !this._readOnly;
            this.chkDir2Cod3.Enabled = !this._readOnly;
            this.chkDir2Cony.Enabled = !this._readOnly;
            this.chkDir2Deudor.Enabled = !this._readOnly;
            this.chkDir3Cod1.Enabled = !this._readOnly;
            this.chkDir3Cod2.Enabled = !this._readOnly;
            this.chkDir3Cod3.Enabled = !this._readOnly;
            this.chkDir3Cony.Enabled = !this._readOnly;
            this.chkDir3Deudor.Enabled = !this._readOnly;

            this.chkTel1Cod1.Enabled = !this._readOnly;
            this.chkTel1Cod2.Enabled = !this._readOnly;
            this.chkTel1Cod3.Enabled = !this._readOnly;
            this.chkTel1Cony.Enabled = !this._readOnly;
            this.chkTel1Deudor.Enabled = !this._readOnly;
            this.chkTel2Cod1.Enabled = !this._readOnly;
            this.chkTel2Cod2.Enabled = !this._readOnly;
            this.chkTel2Cod3.Enabled = !this._readOnly;
            this.chkTel2Cony.Enabled = !this._readOnly;
            this.chkTel2Deudor.Enabled = !this._readOnly;
            this.chkTel3Cod1.Enabled = !this._readOnly;
            this.chkTel3Cod2.Enabled = !this._readOnly;
            this.chkTel3Cod3.Enabled = !this._readOnly;
            this.chkTel3Cony.Enabled = !this._readOnly;
            this.chkTel3Deudor.Enabled = !this._readOnly;

            this.chkCel1Cod1.Enabled = !this._readOnly;
            this.chkCel1Cod2.Enabled = !this._readOnly;
            this.chkCel1Cod3.Enabled = !this._readOnly;
            this.chkCel1Cony.Enabled = !this._readOnly;
            this.chkCel1Deudor.Enabled = !this._readOnly;
            this.chkCel2Cod1.Enabled = !this._readOnly;
            this.chkCel2Cod2.Enabled = !this._readOnly;
            this.chkCel2Cod3.Enabled = !this._readOnly;
            this.chkCel2Cony.Enabled = !this._readOnly;
            this.chkCel2Deudor.Enabled = !this._readOnly;

            this.chkCorreo1Cod1.Enabled = !this._readOnly;
            this.chkCorreo1Cod2.Enabled = !this._readOnly;
            this.chkCorreo1Cod3.Enabled = !this._readOnly;
            this.chkCorreo1Cony.Enabled = !this._readOnly;
            this.chkCorreo1Deudor.Enabled = !this._readOnly;
            this.chkCorreo2Cod1.Enabled = !this._readOnly;
            this.chkCorreo2Cod2.Enabled = !this._readOnly;
            this.chkCorreo2Cod3.Enabled = !this._readOnly;
            this.chkCorreo2Cony.Enabled = !this._readOnly;
            this.chkCorreo2Deudor.Enabled = !this._readOnly;

            this.gcActividades.Enabled = !this._readOnly;
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            string result = string.Empty;

            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);

            #region Hace las Validaciones          
            #region Datos Deudor
            if (!string.IsNullOrEmpty(this.lblDeudor.Text))
            {
                if (string.IsNullOrEmpty(this.txtDireccionDigitaDeudor.Text) && this.chkDir1Deudor.Checked == false && this.chkDir2Deudor.Checked == false && this.chkDir3Deudor.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado direccion  " + " Deudor: ") + "\n";
                    this.txtDireccionDigitaDeudor.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelularDigitaDeudor.Text) && this.chkCel1Deudor.Checked == false && this.chkCel2Deudor.Checked == false )
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Deudor: ") + "\n";
                    this.txtCelularDigitaDeudor.Focus();
                }
                //if (string.IsNullOrEmpty(this.txtCorreoDigitaDeudor.Text) && this.chkCorreo1Deudor.Checked == false && this.chkCorreo2Deudor.Checked == false)
                //{
                //    result += string.Format(msgVacio, "No ha confirmado ni digitado correo " + " Deudor: ") + "\n";
                //    this.txtDireccionDigitaDeudor.Focus();
                //}

            }

            #endregion
            if (!string.IsNullOrEmpty(this.lblConyuge.Text))
            {
                #region Datos Conyugue
                if (string.IsNullOrEmpty(this.txtDireccionDigitaCony.Text) && this.chkDir1Cony.Checked == false && this.chkDir2Cony.Checked == false && this.chkDir3Cony.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado direccion  " + " Conyuge: ") + "\n";
                    this.txtDireccionDigitaCony.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelularDigitaCony.Text) && this.chkCel1Cony.Checked == false && this.chkCel2Cony.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Conyuge: ") + "\n";
                    this.txtCelularDigitaCony.Focus();
                }
                //if (string.IsNullOrEmpty(this.txtCorreoDigitaCony.Text) && this.chkCorreo1Cony.Checked == false && this.chkCorreo2Cony.Checked == false)
                //{
                //    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Conyuge: ") + "\n";
                //    this.txtDireccionDigitaCony.Focus();
                //}

                #endregion
            }
            if (!string.IsNullOrEmpty(this.lblCodeudor1.Text))
            {
                #region Datos Codeudor1
                if (string.IsNullOrEmpty(this.txtDireccionDigitaCod1.Text) && this.chkDir1Cod1.Checked == false && this.chkDir2Cod1.Checked == false && this.chkDir3Cod1.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado direccion  " + " Cod1: ") + "\n";
                    this.txtDireccionDigitaCod1.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelularDigitaCod1.Text) && this.chkCel1Cod1.Checked == false && this.chkCel2Cod1.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod1: ") + "\n";
                    this.txtCelularDigitaCod1.Focus();
                }
                //if (string.IsNullOrEmpty(this.txtCorreoDigitaCod1.Text) && this.chkCorreo1Cod1.Checked == false && this.chkCorreo2Cod1.Checked == false)
                //{
                //    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod1: ") + "\n";
                //    this.txtDireccionDigitaCod1.Focus();
                //}

                #endregion
            }
            if (!string.IsNullOrEmpty(this.lblCodeudor2.Text))
            {
                #region Datos Codeudor2
                if (string.IsNullOrEmpty(this.txtDireccionDigitaCod2.Text) && this.chkDir1Cod2.Checked == false && this.chkDir2Cod2.Checked == false && this.chkDir3Cod2.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado direccion  " + " Cod2: ") + "\n";
                    this.txtDireccionDigitaCod2.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelularDigitaCod2.Text) && this.chkCel1Cod2.Checked == false && this.chkCel2Cod2.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod2: ") + "\n";
                    this.txtCelularDigitaCod2.Focus();
                }
                //if (string.IsNullOrEmpty(this.txtCorreoDigitaCod2.Text) && this.chkCorreo1Cod2.Checked == false && this.chkCorreo2Cod2.Checked == false)
                //{
                //    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod2: ") + "\n";
                //    this.txtDireccionDigitaCod2.Focus();
                //}

                #endregion
            }
            if (!string.IsNullOrEmpty(this.lblCodeudor3.Text))
            {
                #region Datos Codeudor3
                if (string.IsNullOrEmpty(this.txtDireccionDigitaCod3.Text) && this.chkDir1Cod3.Checked == false && this.chkDir2Cod3.Checked == false && this.chkDir3Cod3.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado direccion  " + " Cod3: ") + "\n";
                    this.txtDireccionDigitaCod3.Focus();
                }
                if (string.IsNullOrEmpty(this.txtCelularDigitaCod3.Text) && this.chkCel1Cod3.Checked == false && this.chkCel2Cod3.Checked == false)
                {
                    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod3: ") + "\n";
                    this.txtCelularDigitaCod3.Focus();
                }
                //if (string.IsNullOrEmpty(this.txtCorreoDigitaCod3.Text) && this.chkCorreo1Cod3.Checked == false && this.chkCorreo2Cod3.Checked == false)
                //{
                //    result += string.Format(msgVacio, "No ha confirmado ni digitado celular  " + " Cod3: ") + "\n";
                //    this.txtDireccionDigitaCod3.Focus();
                //}

                #endregion

            }

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
        /// Verifica que la lista de chequeo este completa
        /// </summary>
        /// <returns></returns>
        private string ValidateListas()
        {
            string Falta = null;
            foreach (DTO_glDocumentoChequeoLista basic in this._actividadChequeo)
            {
                if (!basic.IncluidoInd.Value.Value && basic.IncluidoDeudor.Value.Value)
                {
                    Falta += "\r\n" + "Deudor: " + basic.Descripcion.Value.TrimEnd();
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
        /// Verifica que los campos obligatorios esten bn
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            string result = string.Empty;

            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);

            
            
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
                //if (this.checkDeudor6.Checked == false)
                //{
                //    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Deudor: ") + "\n";
                //    this.checkDeudor6.Focus();
                //}
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
                //if (this.checkDeudor14.Checked == false)
                //{
                //    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Deudor: ") + "\n";
                //    this.checkDeudor14.Focus();
                //}

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
                    //if (this.checkCony6.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Conyuge: ") + "\n";
                    //    this.checkCony6.Focus();
                    //}
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
                    //if (this.checkCony14.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Conyuge: ") + "\n";
                    //    this.checkCony14.Focus();
                    //}

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
                    //if (this.checkCod16.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor1: ") + "\n";
                    //    this.checkCod16.Focus();
                    //}
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
                    //if (this.checkCod114.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor1: ") + "\n";
                    //    this.checkCod114.Focus();
                    //}

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
                    //if (this.checkCod26.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor2: ") + "\n";
                    //    this.checkCod26.Focus();
                    //}
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
                    //if (this.checkCod214.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor2: ") + "\n";
                    //    this.checkCod214.Focus();
                    //}

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
                    //if (this.checkCod36.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblFechaExpedicion.Text + "Validacion Codeudor3: ") + "\n";
                    //    this.checkCod36.Focus();
                    //}
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
                    //if (this.checkCod314.Checked == false)
                    //{
                    //    result += string.Format(msgVacio, this.lblIngresosSoportados.Text + "Validacion Codeudor3: ") + "\n";
                    //    this.checkCod314.Focus();
                    //}

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

                    #endregion
                }
            
            if (string.IsNullOrEmpty(result))

                    return true;
            else
            {

                MessageBox.Show("Verifique los siguientes campos paraa continuar: \n\n" + result);
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
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void AssignValues(bool isGet)
        {
            DTO_drSolicitudDatosPersonales Datosdeudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
            DTO_drSolicitudDatosPersonales Datosconyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
            DTO_drSolicitudDatosPersonales Datoscodeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
            DTO_drSolicitudDatosPersonales Datoscodeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);
            DTO_drSolicitudDatosPersonales Datoscodeudor3 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 5);
            DTO_drSolicitudDatosVehiculo Datosvehiculo = this._data.DatosVehiculo;
            

            DTO_ccSolicitudDataCreditoUbica deudor = this._data.DataCreditoUbica.Find(x => x.TipoId.Value == "1");
            DTO_ccSolicitudDataCreditoUbica conyuge = this._data.DataCreditoUbica.Find(x => x.TipoId.Value == "2");
            DTO_ccSolicitudDataCreditoUbica codeudor1 = this._data.DataCreditoUbica.Find(x => x.TipoId.Value == "3");
            DTO_ccSolicitudDataCreditoUbica codeudor2 = this._data.DataCreditoUbica.Find(x => x.TipoId.Value == "4");
            DTO_ccSolicitudDataCreditoUbica codeudor3 = this._data.DataCreditoUbica.Find(x => x.TipoId.Value == "5");
            DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;
            //DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo.Find(x => x.NumeroDoc.Value == this._data.DocCtrl.NumeroDoc.Value);
            _clienteID = this._data.SolicituDocu.ClienteID.Value.ToString();
            //this.dtFechaFirmaDocumento.DateTime = otros.FechaFirmaDocumento.Value.HasValue ? otros.FechaFirmaDocumento.Value.Value : DateTime.Now;
            #region asigna
            if (isGet) //Asigna datos a los controles
            {

                this.lblDeudor.Text="";
                this.lblConyuge.Text = "";
                this.lblCodeudor1.Text = "";
                this.lblCodeudor2.Text = "";
                this.lblCodeudor3.Text = "";
                #region Deudor
                #region Datos personales Deudor
                if (Datosdeudor != null)
                {
                    #region Datos Personales
                    this.txtCedulaDeudor.Text = Datosdeudor.TerceroID.Value.ToString();
                    this.txtPriApellidoDeudor.Text = Datosdeudor.ApellidoPri.Value;
                    this.txtSdoApellidoDeudor.Text = Datosdeudor.ApellidoSdo.Value;
                    this.txtPriNombreDeudor.Text = Datosdeudor.NombrePri.Value;
                    this.txtSdoNombreDeudor.Text = Datosdeudor.NombreSdo.Value;
                    this.masterTerceroDocTipoDeudor.Value = Datosdeudor.TerceroDocTipoID.Value;
                    this.dtFechaExpDeudor.DateTime = Datosdeudor.FechaExpDoc.Value.HasValue ? Datosdeudor.FechaExpDoc.Value.Value : DateTime.Now;
                    this.dtFechaNacDeudor.DateTime = Datosdeudor.FechaNacimiento.Value.HasValue ? Datosdeudor.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.dtFechaNacConfDeudor.DateTime = deudor.FechaNacimiento.Value.HasValue ? deudor.FechaNacimiento.Value.Value : DateTime.Now;                    
                    this.cmbEstadoCivilDeudor.EditValue = Datosdeudor.EstadoCivil.Value;
                    this.masterActEconPrincipalDeudor.Value = Datosdeudor.ActEconomica1.Value;
                    this.cmbFuente1Deudor.EditValue = Datosdeudor.FuenteIngresos1.Value;
                    this.cmbFuente2Deudor.EditValue = Datosdeudor.FuenteIngresos2.Value;

                    this.txtValorIngDeud.EditValue = Datosdeudor.IngresosREG.Value.HasValue ? Datosdeudor.IngresosREG.Value : 0;
                    //this.txtValorIngConfDeud.EditValue = 0;// deudor.IngresosREG.Value.HasValue ? deudor.IngresosREG.Value : 0;
                    this.txtValorIngSoporDeud.EditValue = Datosdeudor.IngresosSOP.Value.HasValue ? Datosdeudor.IngresosSOP.Value : 0;
                    this.txtCorreoDeudor.Text = Datosdeudor.Correo.Value;
                    this.masterCiudadDeudor.Value = Datosdeudor.CiudadResidencia.Value;
                    #endregion

                    #region Validaciones

                    this.checkDeudor1.Checked = Datosdeudor.IndApellidoPri.Value.Value;
                    this.checkDeudor2.Checked = Datosdeudor.IndApellidoSdo.Value.Value;
                    this.checkDeudor3.Checked = Datosdeudor.IndNombrePri.Value.Value;
                    this.checkDeudor4.Checked = Datosdeudor.IndNombreSdo.Value.Value;
                    this.checkDeudor5.Checked = Datosdeudor.IndTerceroDocTipoID.Value.Value;
                    this.checkDeudor6.Checked = Datosdeudor.IndFechaExpDoc.Value.Value;
                    this.checkDeudor7.Checked = Datosdeudor.IndTerceroID.Value.Value;
                    this.checkDeudor8.Checked = Datosdeudor.IndFechaNacimiento.Value.Value;
                    this.checkDeudor9.Checked = Datosdeudor.IndEstadoCivil.Value.Value;
                    this.checkDeudor10.Checked = Datosdeudor.IndActEconomica1.Value.Value;
                    this.checkDeudor11.Checked = Datosdeudor.IndFuenteIngresos1.Value.Value;
                    this.checkDeudor12.Checked = Datosdeudor.IndFuenteIngresos2.Value.Value;
                    this.checkDeudor13.Checked = Datosdeudor.IndIngresosREG.Value.Value;
                    this.checkDeudor14.Checked = Datosdeudor.IndIngresosSOP.Value.Value;
                    this.checkDeudor15.Checked = Datosdeudor.IndCorreo.Value.Value;
                    this.checkDeudor16.Checked = Datosdeudor.IndCiudadResidencia.Value.Value;

                    #endregion
                }
                #endregion

                #region Datacredito deudor
                //Deudor (TipoPersona 1)
                if (deudor != null)
                {
                   // = deudor.NumeroDoc.Value.ToString();
                    this.lblDeudor.Text=deudor.Nombre.Value;
                    this.txtDireccion1Deudor.Text=deudor.DireccionDir1.Value;
                    this.txtDireccion2Deudor.Text=deudor.DireccionDir2.Value;
                    this.txtDireccion3Deudor.Text=deudor.DireccionDir3.Value;
                    this.txtDireccionDigitaDeudor.Text=deudor.DireccionOtra.Value;
                    this.txtTipoDireccion1Deudor.Text=deudor.TipoDir1.Value;
                    this.txtTipoDireccion2Deudor.Text=deudor.TipoDir1.Value;
                    this.txtTipoDireccion3Deudor.Text=deudor.TipoDir1.Value;
                    this.cmbTipoDireccionDigitaDeudor.EditValue = deudor.DireccionOtraIND.Value;
                    this.txtTelefono1Deudor.Text=deudor.NumeroTel1.Value;
                    this.txtTelefono2Deudor.Text=deudor.NumeroTel2.Value;
                    this.txtTelefono3Deudor.Text=deudor.NumeroTel3.Value;
                    this.txtTelefonoDigitaDeudor.Text = deudor.TelefonoOtro.Value;
                    this.txtTipoTelefono1Deudor.Text=deudor.TipoTel1.Value;
                    this.txtTipoTelefono2Deudor.Text=deudor.TipoTel2.Value;
                    this.txtTipoTelefono3Deudor.Text=deudor.TipoTel3.Value;
                    this.cmbTipoTelefonoDigitaDeudor.EditValue="";
                    this.txtCelular1Deudor.Text=deudor.Celular1.Value;
                    this.txtCelular2Deudor.Text=deudor.Celular2.Value;
                    this.txtCelularDigitaDeudor.Text = deudor.CelularOtro.Value;
                    this.txtCorreo1Deudor.Text=deudor.Email1.Value;
                    this.txtCorreo2Deudor.Text=deudor.Email1.Value;
                    this.txtCorreoDigitaDeudor.Text = deudor.EMailOtro.Value;

                    // Validados
                    this.chkDir1Deudor.Checked=deudor.Direccion1IND.Value.Value;
                    this.chkDir2Deudor.Checked = deudor.Direccion2IND.Value.Value;
                    this.chkDir3Deudor.Checked = deudor.Direccion3IND.Value.Value;

                    this.chkTel1Deudor.Checked = deudor.Telefono1IND.Value.Value;
                    this.chkTel2Deudor.Checked = deudor.Telefono2IND.Value.Value;
                    this.chkTel3Deudor.Checked = deudor.Telefono3IND.Value.Value;

                    this.chkCel1Deudor.Checked = deudor.Celular1IND.Value.Value;
                    this.chkCel2Deudor.Checked = deudor.Celular2IND.Value.Value;

                    this.chkCorreo1Deudor.Checked = deudor.EMail1IND.Value.Value;
                    this.chkCorreo2Deudor.Checked = deudor.EMail2IND.Value.Value;


                }
                #endregion
                #endregion
                #region Conyuge
                //Conyuge (TipoPersona 2)

                #region datos personales conyuge
                if (Datosconyuge != null)
                {

                    #region Datos Personales
                    this.txtCedulaCony.Text = Datosconyuge.TerceroID.Value.ToString();
                    this.txtPriApellidoCony.Text = Datosconyuge.ApellidoPri.Value;
                    this.txtSdoApellidoCony.Text = Datosconyuge.ApellidoSdo.Value;
                    this.txtPriNombreCony.Text = Datosconyuge.NombrePri.Value;
                    this.txtSdoNombreCony.Text = Datosconyuge.NombreSdo.Value;
                    this.masterTerceroDocTipoCony.Value = Datosconyuge.TerceroDocTipoID.Value;
                    this.dtFechaExpCony.DateTime = Datosconyuge.FechaExpDoc.Value.HasValue ? Datosconyuge.FechaExpDoc.Value.Value : DateTime.Now;
                    this.dtFechaNacCony.DateTime = Datosconyuge.FechaNacimiento.Value.HasValue ? Datosconyuge.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.dtFechaNacConfCony.DateTime = Cony.FechaNacimiento.Value.HasValue ? Cony.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.cmbEstadoCivilCony.EditValue = conyuge.EstadoCivil.Value;
                    this.cmbEstadoCivilCony.EditValue = this.cmbEstadoCivilDeudor.EditValue.ToString();
                    this.masterActEconPrincipalCony.Value = Datosconyuge.ActEconomica1.Value;
                    this.cmbFuente1Cony.EditValue = Datosconyuge.FuenteIngresos1.Value;
                    this.cmbFuente2Cony.EditValue = Datosconyuge.FuenteIngresos2.Value;

                    this.txtValorIngCony.EditValue = Datosconyuge.IngresosREG.Value.HasValue ? Datosconyuge.IngresosREG.Value : 0;
                    //this.txtValorIngConfCony.EditValue = 0;// Cony.IngresosREG.Value.HasValue ? Cony.IngresosREG.Value : 0;
                    this.txtValorIngSoporCony.EditValue = Datosconyuge.IngresosSOP.Value.HasValue ? Datosconyuge.IngresosSOP.Value : 0;
                    this.txtCorreoCony.Text = Datosconyuge.Correo.Value;
                    this.masterCiudadCony.Value = Datosconyuge.CiudadResidencia.Value;
                    this.linkDatosConyuge.Visible = false;
                    #endregion

                    #region Validaciones                    
                    this.checkCony1.Checked = Datosconyuge.IndApellidoPri.Value.Value;
                    this.checkCony2.Checked = Datosconyuge.IndApellidoSdo.Value.Value;
                    this.checkCony3.Checked = Datosconyuge.IndNombrePri.Value.Value;
                    this.checkCony4.Checked = Datosconyuge.IndNombreSdo.Value.Value;
                    this.checkCony5.Checked = Datosconyuge.IndTerceroDocTipoID.Value.Value;
                    this.checkCony6.Checked = Datosconyuge.IndFechaExpDoc.Value.Value;
                    this.checkCony7.Checked = Datosconyuge.IndTerceroID.Value.Value;
                    this.checkCony8.Checked = Datosconyuge.IndFechaNacimiento.Value.Value;
                    this.checkCony9.Checked = Datosconyuge.IndEstadoCivil.Value.Value;
                    this.checkCony10.Checked = Datosconyuge.IndActEconomica1.Value.Value;
                    this.checkCony11.Checked = Datosconyuge.IndFuenteIngresos1.Value.Value;
                    this.checkCony12.Checked = Datosconyuge.IndFuenteIngresos2.Value.Value;
                    this.checkCony13.Checked = Datosconyuge.IndIngresosREG.Value.Value;
                    this.checkCony14.Checked = Datosconyuge.IndIngresosSOP.Value.Value;
                    this.checkCony15.Checked = Datosconyuge.IndCorreo.Value.Value;
                    this.checkCony16.Checked = Datosconyuge.IndCiudadResidencia.Value.Value;
                    #endregion
                }
                #endregion

                #region Datacredito Deudor
                if (conyuge != null)
                {
                    _conyugueID = conyuge.NumeroDoc.Value.ToString();
                    this.lblConyuge.Text = conyuge.Nombre.Value;
                    this.txtDireccion1Cony.Text=conyuge.DireccionDir1.Value;
                    this.txtDireccion2Cony.Text=conyuge.DireccionDir2.Value;
                    this.txtDireccion3Cony.Text=conyuge.DireccionDir3.Value;
                    this.txtDireccionDigitaCony.Text = conyuge.DireccionOtra.Value;
                    this.txtTipoDireccion1Cony.Text=conyuge.TipoDir1.Value;
                    this.txtTipoDireccion2Cony.Text=conyuge.TipoDir1.Value;
                    this.txtTipoDireccion3Cony.Text=conyuge.TipoDir1.Value;
                    this.cmbTipoDireccionDigitaCony.EditValue="";
                    this.txtTelefono1Cony.Text=conyuge.NumeroTel1.Value;
                    this.txtTelefono2Cony.Text=conyuge.NumeroTel2.Value;
                    this.txtTelefono3Cony.Text=conyuge.NumeroTel3.Value;
                    this.txtTelefonoDigitaCony.Text = conyuge.TelefonoOtro.Value;
                    this.txtTipoTelefono1Cony.Text=conyuge.TipoTel1.Value;
                    this.txtTipoTelefono2Cony.Text=conyuge.TipoTel2.Value;
                    this.txtTipoTelefono3Cony.Text=conyuge.TipoTel3.Value;
                    this.cmbTipoTelefonoDigitaCony.EditValue="";
                    this.txtCelular1Cony.Text=conyuge.Celular1.Value;
                    this.txtCelular2Cony.Text=conyuge.Celular2.Value;
                    this.txtCelularDigitaCony.Text = conyuge.CelularOtro.Value;
                    this.txtCorreo1Cony.Text=conyuge.Email1.Value;
                    this.txtCorreo2Cony.Text=conyuge.Email1.Value;
                    this.txtCorreoDigitaCony.Text = conyuge.EMailOtro.Value;
                    this.linkConyuge.Visible = false;

                    // Validados
                    this.chkDir1Cony.Checked = conyuge.Direccion1IND.Value.Value;
                    this.chkDir2Cony.Checked = conyuge.Direccion2IND.Value.Value;
                    this.chkDir3Cony.Checked = conyuge.Direccion3IND.Value.Value;

                    this.chkTel1Cony.Checked = conyuge.Telefono1IND.Value.Value;
                    this.chkTel2Cony.Checked = conyuge.Telefono2IND.Value.Value;
                    this.chkTel3Cony.Checked = conyuge.Telefono3IND.Value.Value;

                    this.chkCel1Cony.Checked = conyuge.Celular1IND.Value.Value;
                    this.chkCel2Cony.Checked = conyuge.Celular2IND.Value.Value;

                    this.chkCorreo1Cony.Checked = conyuge.EMail1IND.Value.Value;
                    this.chkCorreo2Cony.Checked = conyuge.EMail2IND.Value.Value;

                }
                #endregion
                #endregion
                #region Codeudor1
                
                if (Datoscodeudor1 != null)
                {
                    #region Datos personales coDeudor1
                    this.txtCedulaCod1.Text = Datoscodeudor1.TerceroID.Value.ToString();
                    this.txtPriApellidoCod1.Text = Datoscodeudor1.ApellidoPri.Value;
                    this.txtSdoApellidoCod1.Text = Datoscodeudor1.ApellidoSdo.Value;
                    this.txtPriNombreCod1.Text = Datoscodeudor1.NombrePri.Value;
                    this.txtSdoNombreCod1.Text = Datoscodeudor1.NombreSdo.Value;
                    this.masterTerceroDocTipoCod1.Value = Datoscodeudor1.TerceroDocTipoID.Value;
                    this.dtFechaExpCod1.DateTime = Datoscodeudor1.FechaExpDoc.Value.HasValue ? Datoscodeudor1.FechaExpDoc.Value.Value : DateTime.Now;
                    this.dtFechaNacCod1.DateTime = Datoscodeudor1.FechaNacimiento.Value.HasValue ? Datoscodeudor1.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.dtFechaNacConfCod1.DateTime = Cod1.FechaNacimiento.Value.HasValue ? Cod1.FechaNacimiento.Value.Value : DateTime.Now;
                    this.cmbEstadoCivilCod1.EditValue = Datoscodeudor1.EstadoCivil.Value;
                    this.masterActEconPrincipalCod1.Value = Datoscodeudor1.ActEconomica1.Value;
                    this.cmbFuente1Cod1.EditValue = Datoscodeudor1.FuenteIngresos1.Value;
                    this.cmbFuente2Cod1.EditValue = Datoscodeudor1.FuenteIngresos2.Value;

                    this.txtValorIngCod1.EditValue = Datoscodeudor1.IngresosREG.Value.HasValue ? Datoscodeudor1.IngresosREG.Value : 0;
                    //this.txtValorIngConfCod1.EditValue = 0;// Cod1.IngresosREG.Value.HasValue ? Cod1.IngresosREG.Value : 0;
                    this.txtValorIngSoporCod1.EditValue = Datoscodeudor1.IngresosSOP.Value.HasValue ? Datoscodeudor1.IngresosSOP.Value : 0;
                    this.txtCorreoCod1.Text = Datoscodeudor1.Correo.Value;
                    this.masterCiudadCod1.Value = Datoscodeudor1.CiudadResidencia.Value;
                    this.linkDatosCodeudor1.Visible = false;
                    #endregion

                    #region Validaciones
                    this.checkCod11.Checked = Datoscodeudor1.IndApellidoPri.Value.Value;
                    this.checkCod12.Checked = Datoscodeudor1.IndApellidoSdo.Value.Value;
                    this.checkCod13.Checked = Datoscodeudor1.IndNombrePri.Value.Value;
                    this.checkCod14.Checked = Datoscodeudor1.IndNombreSdo.Value.Value;
                    this.checkCod15.Checked = Datoscodeudor1.IndTerceroDocTipoID.Value.Value;
                    this.checkCod16.Checked = Datoscodeudor1.IndFechaExpDoc.Value.Value;
                    this.checkCod17.Checked = Datoscodeudor1.IndTerceroID.Value.Value;
                    this.checkCod18.Checked = Datoscodeudor1.IndFechaNacimiento.Value.Value;
                    this.checkCod19.Checked = Datoscodeudor1.IndEstadoCivil.Value.Value;
                    this.checkCod110.Checked = Datoscodeudor1.IndActEconomica1.Value.Value;
                    this.checkCod111.Checked = Datoscodeudor1.IndFuenteIngresos1.Value.Value;
                    this.checkCod112.Checked = Datoscodeudor1.IndFuenteIngresos2.Value.Value;
                    this.checkCod113.Checked = Datoscodeudor1.IndIngresosREG.Value.Value;
                    this.checkCod114.Checked = Datoscodeudor1.IndIngresosSOP.Value.Value;
                    this.checkCod115.Checked = Datoscodeudor1.IndCorreo.Value.Value;
                    this.checkCod116.Checked = Datoscodeudor1.IndCiudadResidencia.Value.Value;
                    #endregion
                        
                }
                //Codeudor1 (TipoPersona 3)
                #region Datacredito codeudor1
                if (codeudor1 != null)
                {
                    _codeudor1ID = codeudor1.NumeroDoc.Value.ToString();
                    this.lblCodeudor1.Text = codeudor1.Nombre.Value;
                    this.txtDireccion1Cod1.Text=codeudor1.DireccionDir1.Value;
                    this.txtDireccion2Cod1.Text=codeudor1.DireccionDir2.Value;
                    this.txtDireccion3Cod1.Text=codeudor1.DireccionDir3.Value;
                    this.txtDireccionDigitaCod1.Text = codeudor1.DireccionOtra.Value;
                    this.txtTipoDireccion1Cod1.Text=codeudor1.TipoDir1.Value;
                    this.txtTipoDireccion2Cod1.Text=codeudor1.TipoDir1.Value;
                    this.txtTipoDireccion3Cod1.Text=codeudor1.TipoDir1.Value;
                    this.cmbTipoDireccionDigitaCod1.EditValue="";
                    this.txtTelefono1Cod1.Text=codeudor1.NumeroTel1.Value;
                    this.txtTelefono2Cod1.Text=codeudor1.NumeroTel2.Value;
                    this.txtTelefono3Cod1.Text=codeudor1.NumeroTel3.Value;
                    this.txtTelefonoDigitaCod1.Text = codeudor1.TelefonoOtro.Value;
                    this.txtTipoTelefono1Cod1.Text=codeudor1.TipoTel1.Value;
                    this.txtTipoTelefono2Cod1.Text=codeudor1.TipoTel2.Value;
                    this.txtTipoTelefono3Cod1.Text=codeudor1.TipoTel3.Value;
                    this.cmbTipoTelefonoDigitaCod1.EditValue="";
                    this.txtCelular1Cod1.Text=codeudor1.Celular1.Value;
                    this.txtCelular2Cod1.Text=codeudor1.Celular2.Value;
                    this.txtCelularDigitaCod1.Text = codeudor1.CelularOtro.Value;
                    this.txtCorreo1Cod1.Text=codeudor1.Email1.Value;
                    this.txtCorreo2Cod1.Text=codeudor1.Email1.Value;
                    this.txtCorreoDigitaCod1.Text = codeudor1.EMailOtro.Value;
                    this.linkCodeudor1.Visible = false;


                    // Validados
                    this.chkDir1Cod1.Checked = codeudor1.Direccion1IND.Value.Value;
                    this.chkDir2Cod1.Checked = codeudor1.Direccion2IND.Value.Value;
                    this.chkDir3Cod1.Checked = codeudor1.Direccion3IND.Value.Value;

                    this.chkTel1Cod1.Checked = codeudor1.Telefono1IND.Value.Value;
                    this.chkTel2Cod1.Checked = codeudor1.Telefono2IND.Value.Value;
                    this.chkTel3Cod1.Checked = codeudor1.Telefono3IND.Value.Value;

                    this.chkCel1Cod1.Checked = codeudor1.Celular1IND.Value.Value;
                    this.chkCel2Cod1.Checked = codeudor1.Celular2IND.Value.Value;

                    this.chkCorreo1Cod1.Checked = codeudor1.EMail1IND.Value.Value;
                    this.chkCorreo2Cod1.Checked = codeudor1.EMail2IND.Value.Value;
                }
                #endregion
                #endregion
                #region Codeudor2
                
                if (Datoscodeudor2 != null)
                {
                    #region Datos personales coDeudor2
                    this.txtCedulaCod2.Text = Datoscodeudor2.TerceroID.Value.ToString();
                    this.txtPriApellidoCod2.Text = Datoscodeudor2.ApellidoPri.Value;
                    this.txtSdoApellidoCod2.Text = Datoscodeudor2.ApellidoSdo.Value;
                    this.txtPriNombreCod2.Text = Datoscodeudor2.NombrePri.Value;
                    this.txtSdoNombreCod2.Text = Datoscodeudor2.NombreSdo.Value;
                    this.masterTerceroDocTipoCod2.Value = Datoscodeudor2.TerceroDocTipoID.Value;
                    this.dtFechaExpCod2.DateTime = Datoscodeudor2.FechaExpDoc.Value.HasValue ? Datoscodeudor2.FechaExpDoc.Value.Value : DateTime.Now;
                    this.dtFechaNacCod2.DateTime = Datoscodeudor2.FechaNacimiento.Value.HasValue ? Datoscodeudor2.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.dtFechaNacConfCod2.DateTime = Cod2.FechaNacimiento.Value.HasValue ? Cod2.FechaNacimiento.Value.Value : DateTime.Now;
                    this.cmbEstadoCivilCod2.EditValue = Datoscodeudor2.EstadoCivil.Value;
                    this.masterActEconPrincipalCod2.Value = Datoscodeudor2.ActEconomica1.Value;
                    this.cmbFuente1Cod2.EditValue = Datoscodeudor2.FuenteIngresos1.Value;
                    this.cmbFuente2Cod2.EditValue = Datoscodeudor2.FuenteIngresos2.Value;

                    this.txtValorIngCod2.EditValue = Datoscodeudor2.IngresosREG.Value.HasValue ? Datoscodeudor2.IngresosREG.Value : 0;
                    //this.txtValorIngConfCod2.EditValue = 0;// Cod2.IngresosREG.Value.HasValue ? Cod2.IngresosREG.Value : 0;
                    this.txtValorIngSoporCod2.EditValue = Datoscodeudor2.IngresosSOP.Value.HasValue ? Datoscodeudor2.IngresosSOP.Value : 0;
                    this.txtCorreoCod2.Text = Datoscodeudor2.Correo.Value;
                    this.masterCiudadCod2.Value = Datoscodeudor2.CiudadResidencia.Value;
                    this.linkDatosCodeudor2.Visible = false;
                    #endregion

                    #region Validaciones
                    this.checkCod21.Checked = Datoscodeudor2.IndApellidoPri.Value.Value;
                    this.checkCod22.Checked = Datoscodeudor2.IndApellidoSdo.Value.Value;
                    this.checkCod23.Checked = Datoscodeudor2.IndNombrePri.Value.Value;
                    this.checkCod24.Checked = Datoscodeudor2.IndNombreSdo.Value.Value;
                    this.checkCod25.Checked = Datoscodeudor2.IndTerceroDocTipoID.Value.Value;
                    this.checkCod26.Checked = Datoscodeudor2.IndFechaExpDoc.Value.Value;
                    this.checkCod27.Checked = Datoscodeudor2.IndTerceroID.Value.Value;
                    this.checkCod28.Checked = Datoscodeudor2.IndFechaNacimiento.Value.Value;
                    this.checkCod29.Checked = Datoscodeudor2.IndEstadoCivil.Value.Value;
                    this.checkCod210.Checked = Datoscodeudor2.IndActEconomica1.Value.Value;
                    this.checkCod211.Checked = Datoscodeudor2.IndFuenteIngresos1.Value.Value;
                    this.checkCod212.Checked = Datoscodeudor2.IndFuenteIngresos2.Value.Value;
                    this.checkCod213.Checked = Datoscodeudor2.IndIngresosREG.Value.Value;
                    this.checkCod214.Checked = Datoscodeudor2.IndIngresosSOP.Value.Value;
                    this.checkCod215.Checked = Datoscodeudor2.IndCorreo.Value.Value;
                    this.checkCod216.Checked = Datoscodeudor2.IndCiudadResidencia.Value.Value;
                    #endregion
                        
                }
                //Codeudor2 (TipoPersona 4)
                #region Datacredito codeudor2
                if (codeudor2 != null)
                {
                    _codeudor2ID = codeudor2.NumeroDoc.Value.ToString();

                    this.lblCodeudor2.Text = codeudor2.Nombre.Value;
                    this.txtDireccion1Cod2.Text=codeudor2.DireccionDir1.Value;
                    this.txtDireccion2Cod2.Text=codeudor2.DireccionDir2.Value;
                    this.txtDireccion3Cod2.Text=codeudor2.DireccionDir3.Value;
                    this.txtDireccionDigitaCod2.Text = codeudor2.DireccionOtra.Value;
                    this.txtTipoDireccion1Cod2.Text=codeudor2.TipoDir1.Value;
                    this.txtTipoDireccion2Cod2.Text=codeudor2.TipoDir1.Value;
                    this.txtTipoDireccion3Cod2.Text=codeudor2.TipoDir1.Value;
                    this.cmbTipoDireccionDigitaCod2.EditValue="";
                    this.txtTelefono1Cod2.Text=codeudor2.NumeroTel1.Value;
                    this.txtTelefono2Cod2.Text=codeudor2.NumeroTel2.Value;
                    this.txtTelefono3Cod2.Text=codeudor2.NumeroTel3.Value;
                    this.txtTelefonoDigitaCod2.Text = codeudor2.TelefonoOtro.Value;
                    this.txtTipoTelefono1Cod2.Text=codeudor2.TipoTel1.Value;
                    this.txtTipoTelefono2Cod2.Text=codeudor2.TipoTel2.Value;
                    this.txtTipoTelefono3Cod2.Text=codeudor2.TipoTel3.Value;
                    this.cmbTipoTelefonoDigitaCod2.EditValue = "";
                    this.txtCelular1Cod2.Text=codeudor2.Celular1.Value;
                    this.txtCelular2Cod2.Text=codeudor2.Celular2.Value;
                    this.txtCelularDigitaCod2.Text = codeudor2.CelularOtro.Value;
                    this.txtCorreo1Cod2.Text=codeudor2.Email1.Value;
                    this.txtCorreo2Cod2.Text=codeudor2.Email1.Value;
                    this.txtCorreoDigitaCod2.Text = codeudor2.EMailOtro.Value;
                    this.linkDatosCodeudor2.Visible = false;

                    // Validados
                    this.chkDir1Cod2.Checked = codeudor2.Direccion1IND.Value.Value;
                    this.chkDir2Cod2.Checked = codeudor2.Direccion2IND.Value.Value;
                    this.chkDir3Cod2.Checked = codeudor2.Direccion3IND.Value.Value;

                    this.chkTel1Cod2.Checked = codeudor2.Telefono1IND.Value.Value;
                    this.chkTel2Cod2.Checked = codeudor2.Telefono2IND.Value.Value;
                    this.chkTel3Cod2.Checked = codeudor2.Telefono3IND.Value.Value;

                    this.chkCel1Cod2.Checked = codeudor2.Celular1IND.Value.Value;
                    this.chkCel2Cod2.Checked = codeudor2.Celular2IND.Value.Value;

                    this.chkCorreo1Cod2.Checked = codeudor2.EMail1IND.Value.Value;
                    this.chkCorreo2Cod2.Checked = codeudor2.EMail2IND.Value.Value;
                }
                #endregion
                #endregion
                #region Codeudor3

                if (Datoscodeudor3 != null)
                {
                    #region Datos personales coDeudor3
                    this.txtCedulaCod3.Text = Datoscodeudor3.TerceroID.Value.ToString();
                    this.txtPriApellidoCod3.Text = Datoscodeudor3.ApellidoPri.Value;
                    this.txtSdoApellidoCod3.Text = Datoscodeudor3.ApellidoSdo.Value;
                    this.txtPriNombreCod3.Text = Datoscodeudor3.NombrePri.Value;
                    this.txtSdoNombreCod3.Text = Datoscodeudor3.NombreSdo.Value;
                    this.masterTerceroDocTipoCod3.Value = Datoscodeudor3.TerceroDocTipoID.Value;
                    this.dtFechaExpCod3.DateTime = Datoscodeudor3.FechaExpDoc.Value.HasValue ? Datoscodeudor3.FechaExpDoc.Value.Value : DateTime.Now;
                    this.dtFechaNacCod3.DateTime = Datoscodeudor3.FechaNacimiento.Value.HasValue ? Datoscodeudor3.FechaNacimiento.Value.Value : DateTime.Now;
                    //this.dtFechaNacConfCod3.DateTime = Cod3.FechaNacimiento.Value.HasValue ? Cod3.FechaNacimiento.Value.Value : DateTime.Now;
                    this.cmbEstadoCivilCod3.EditValue = Datoscodeudor3.EstadoCivil.Value;
                    this.masterActEconPrincipalCod3.Value = Datoscodeudor3.ActEconomica1.Value;
                    this.cmbFuente1Cod3.EditValue = Datoscodeudor3.FuenteIngresos1.Value;
                    this.cmbFuente2Cod3.EditValue = Datoscodeudor3.FuenteIngresos2.Value;

                    this.txtValorIngCod3.EditValue = Datoscodeudor3.IngresosREG.Value.HasValue ? Datoscodeudor3.IngresosREG.Value : 0;
                    //this.txtValorIngConfCod3.EditValue = 0;// Cod3.IngresosREG.Value.HasValue ? Cod3.IngresosREG.Value : 0;
                    this.txtValorIngSoporCod3.EditValue = Datoscodeudor3.IngresosSOP.Value.HasValue ? Datoscodeudor3.IngresosSOP.Value : 0;
                    this.txtCorreoCod3.Text = Datoscodeudor3.Correo.Value;
                    this.masterCiudadCod3.Value = Datoscodeudor3.CiudadResidencia.Value;
                    this.linkDatosCodeudor3.Visible = false;
                    #endregion

                    #region Validaciones
                    this.checkCod31.Checked = Datoscodeudor3.IndApellidoPri.Value.Value;
                    this.checkCod32.Checked = Datoscodeudor3.IndApellidoSdo.Value.Value;
                    this.checkCod33.Checked = Datoscodeudor3.IndNombrePri.Value.Value;
                    this.checkCod34.Checked = Datoscodeudor3.IndNombreSdo.Value.Value;
                    this.checkCod35.Checked = Datoscodeudor3.IndTerceroDocTipoID.Value.Value;
                    this.checkCod36.Checked = Datoscodeudor3.IndFechaExpDoc.Value.Value;
                    this.checkCod37.Checked = Datoscodeudor3.IndTerceroID.Value.Value;
                    this.checkCod38.Checked = Datoscodeudor3.IndFechaNacimiento.Value.Value;
                    this.checkCod39.Checked = Datoscodeudor3.IndEstadoCivil.Value.Value;
                    this.checkCod310.Checked = Datoscodeudor3.IndActEconomica1.Value.Value;
                    this.checkCod311.Checked = Datoscodeudor3.IndFuenteIngresos1.Value.Value;
                    this.checkCod312.Checked = Datoscodeudor3.IndFuenteIngresos2.Value.Value;
                    this.checkCod313.Checked = Datoscodeudor3.IndIngresosREG.Value.Value;
                    this.checkCod314.Checked = Datoscodeudor3.IndIngresosSOP.Value.Value;
                    this.checkCod315.Checked = Datoscodeudor3.IndCorreo.Value.Value;
                    this.checkCod316.Checked = Datoscodeudor3.IndCiudadResidencia.Value.Value;
                    #endregion

                }

                //Codeudor3 (TipoPersona 5)
                #region Datacredito codeudor3
                if (codeudor3 != null)
                {
                    _codeudor3ID = codeudor3.NumeroDoc.Value.ToString();
                    this.lblCodeudor3.Text = codeudor3.Nombre.Value;
                    this.txtDireccion1Cod3.Text=codeudor3.DireccionDir1.Value;
                    this.txtDireccion2Cod3.Text=codeudor3.DireccionDir2.Value;
                    this.txtDireccion3Cod3.Text=codeudor3.DireccionDir3.Value;
                    this.txtDireccionDigitaCod3.Text = codeudor3.DireccionOtra.Value;
                    this.txtTipoDireccion1Cod3.Text=codeudor3.TipoDir1.Value;
                    this.txtTipoDireccion2Cod3.Text=codeudor3.TipoDir1.Value;
                    this.txtTipoDireccion3Cod3.Text=codeudor3.TipoDir1.Value;
                    this.cmbTipoDireccionDigitaCod3.EditValue="";
                    this.txtTelefono1Cod3.Text=codeudor3.NumeroTel1.Value;
                    this.txtTelefono2Cod3.Text=codeudor3.NumeroTel2.Value;
                    this.txtTelefono3Cod3.Text=codeudor3.NumeroTel3.Value;
                    this.txtTelefonoDigitaCod3.Text=codeudor3.TelefonoOtro.Value;
                    this.txtTipoTelefono1Cod3.Text=codeudor3.TipoTel1.Value;
                    this.txtTipoTelefono2Cod3.Text=codeudor3.TipoTel2.Value;
                    this.txtTipoTelefono3Cod3.Text=codeudor3.TipoTel3.Value;
                    this.cmbTipoTelefonoDigitaCod3.EditValue = "";
                    this.txtCelular1Cod3.Text=codeudor3.Celular1.Value;
                    this.txtCelular2Cod3.Text=codeudor3.Celular2.Value;
                    this.txtCelularDigitaCod3.Text = codeudor3.CelularOtro.Value;
                    this.txtCorreo1Cod3.Text=codeudor3.Email1.Value;
                    this.txtCorreo2Cod3.Text=codeudor3.Email1.Value;
                    this.txtCorreoDigitaCod3.Text = codeudor3.EMailOtro.Value;
                    this.linkCodeudor3.Visible = false;
                    // Validados
                    this.chkDir1Cod3.Checked = codeudor3.Direccion1IND.Value.Value;
                    this.chkDir2Cod3.Checked = codeudor3.Direccion2IND.Value.Value;
                    this.chkDir3Cod3.Checked = codeudor3.Direccion3IND.Value.Value;

                    this.chkTel1Cod3.Checked = codeudor3.Telefono1IND.Value.Value;
                    this.chkTel2Cod3.Checked = codeudor3.Telefono2IND.Value.Value;
                    this.chkTel3Cod3.Checked = codeudor3.Telefono3IND.Value.Value;

                    this.chkCel1Cod3.Checked = codeudor3.Celular1IND.Value.Value;
                    this.chkCel2Cod3.Checked = codeudor3.Celular2IND.Value.Value;

                    this.chkCorreo1Cod3.Checked = codeudor3.EMail1IND.Value.Value;
                    this.chkCorreo2Cod3.Checked = codeudor3.EMail2IND.Value.Value;
                }
                #endregion
                #endregion
            }
            #endregion
            #region llena
            else //Llena datos de los controles para salvar
            {
                DTO_drSolicitudDatosOtros otrosNew = new DTO_drSolicitudDatosOtros();
                //otros.FechaFirmaDocumento.Value = this.dtFechaFirmaDocumento.DateTime;
                otrosNew.Version.Value = otros != null && otros.Version.Value.HasValue ? otros.Version.Value : Convert.ToByte(this._data.SolicituDocu.VersionNro.Value);
                otrosNew.NumeroDoc.Value = otros != null && otros.NumeroDoc.Value.HasValue ? otros.NumeroDoc.Value : Convert.ToInt32(this._data.SolicituDocu.NumeroDoc.Value);
                otrosNew.Consecutivo.Value = otros != null && otros.Consecutivo.Value.HasValue ? otros.Consecutivo.Value : null;
                otrosNew.VlrSolicitado.Value = this._data.SolicituDocu.VlrSolicitado.Value;
                this._data.OtrosDatos = otrosNew;

                #region Deudor (TipoPersona 1)
                #region Data Credito
                DTO_ccSolicitudDataCreditoUbica deudorNew = deudor != null ? deudor: new DTO_ccSolicitudDataCreditoUbica();
                if (deudor != null)
                {
                    deudor.DireccionDir1.Value = this.txtDireccion1Deudor.Text;
                    deudor.DireccionDir2.Value = this.txtDireccion2Deudor.Text;
                    deudor.DireccionDir3.Value = this.txtDireccion3Deudor.Text;
                    deudor.TipoDir1.Value = this.txtTipoDireccion1Deudor.Text;
                    deudor.TipoDir1.Value = this.txtTipoDireccion2Deudor.Text;
                    deudor.TipoDir1.Value = this.txtTipoDireccion3Deudor.Text;
                    deudor.NumeroTel1.Value = this.txtTelefono1Deudor.Text;
                    deudor.NumeroTel2.Value = this.txtTelefono2Deudor.Text;
                    deudor.NumeroTel3.Value = this.txtTelefono3Deudor.Text;
                    deudor.TipoTel1.Value = this.txtTipoTelefono1Deudor.Text;
                    deudor.TipoTel2.Value = this.txtTipoTelefono2Deudor.Text;
                    deudor.TipoTel3.Value = this.txtTipoTelefono3Deudor.Text;
                    deudor.Celular1.Value = this.txtCelular1Deudor.Text;
                    deudor.Celular2.Value = this.txtCelular2Deudor.Text;
                    deudor.Email1.Value = this.txtCorreo1Deudor.Text;
                    deudor.Email1.Value = this.txtCorreo2Deudor.Text;

                    //Digitados
                    deudor.DireccionOtra.Value = this.txtDireccionDigitaDeudor.Text;
                    deudor.DireccionOtraIND.Value = string.IsNullOrEmpty(deudor.DireccionOtra.Value) ? false : true;
                    deudor.EMailOtro.Value = this.txtCorreoDigitaDeudor.Text;
                    deudor.EMailOtroIND.Value = string.IsNullOrEmpty(deudor.EMailOtro.Value) ? false : true;
                    deudor.TelefonoOtro.Value = this.txtTelefonoDigitaDeudor.Text;
                    deudor.TelefonoOtroIND.Value = string.IsNullOrEmpty(deudor.TelefonoOtro.Value) ? false : true;
                    deudor.CelularOtro.Value = this.txtCelularDigitaDeudor.Text;
                    deudor.CelularOtraIND.Value = string.IsNullOrEmpty(deudor.CelularOtro.Value) ? false : true;
                    
                    // Validados
                    deudor.Direccion1IND.Value = Convert.ToBoolean(this.chkDir1Deudor.Checked);
                    deudor.Direccion2IND.Value = Convert.ToBoolean(this.chkDir2Deudor.Checked);
                    deudor.Direccion3IND.Value = Convert.ToBoolean(this.chkDir3Deudor.Checked);

                    deudor.Telefono1IND.Value = Convert.ToBoolean(this.chkTel1Deudor.Checked);
                    deudor.Telefono2IND.Value = Convert.ToBoolean(this.chkTel2Deudor.Checked);
                    deudor.Telefono3IND.Value = Convert.ToBoolean(this.chkTel2Deudor.Checked);

                    deudor.Celular1IND.Value = Convert.ToBoolean(this.chkCel1Deudor.Checked);
                    deudor.Celular2IND.Value = Convert.ToBoolean(this.chkCel2Deudor.Checked);
                    

                    deudor.EMail1IND.Value = Convert.ToBoolean(this.chkCorreo1Deudor.Checked);
                    deudor.EMail2IND.Value = Convert.ToBoolean(this.chkCorreo2Deudor.Checked);

                    if (deudor == null)
                        this._data.DataCreditoUbica.Add(deudorNew);
                    else
                        deudor = deudorNew; 
                }
                #endregion
                #region validacion Datos Personales
                DTO_drSolicitudDatosPersonales DatosdeudorNew = ObjectCopier.Clone(Datosdeudor) != null ? Datosdeudor : new DTO_drSolicitudDatosPersonales();
                if (DatosdeudorNew != null)
                {

                    Datosdeudor.IndApellidoPri.Value = Convert.ToBoolean(this.checkDeudor1.Checked);
                    Datosdeudor.IndApellidoSdo.Value = Convert.ToBoolean(this.checkDeudor2.Checked);
                    Datosdeudor.IndNombrePri.Value = Convert.ToBoolean(this.checkDeudor3.Checked);
                    Datosdeudor.IndNombreSdo.Value = Convert.ToBoolean(this.checkDeudor4.Checked);
                    Datosdeudor.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkDeudor5.Checked);
                    Datosdeudor.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkDeudor6.Checked);
                    Datosdeudor.IndTerceroID.Value = Convert.ToBoolean(this.checkDeudor7.Checked);
                    Datosdeudor.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkDeudor8.Checked);
                    Datosdeudor.IndEstadoCivil.Value = Convert.ToBoolean(this.checkDeudor9.Checked);
                    Datosdeudor.IndActEconomica1.Value = Convert.ToBoolean(this.checkDeudor10.Checked);
                    Datosdeudor.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkDeudor11.Checked);
                    Datosdeudor.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkDeudor12.Checked);
                    Datosdeudor.IndIngresosREG.Value = Convert.ToBoolean(this.checkDeudor13.Checked);
                    Datosdeudor.IndIngresosSOP.Value = Convert.ToBoolean(this.checkDeudor14.Checked);
                    Datosdeudor.IndCorreo.Value = Convert.ToBoolean(this.checkDeudor15.Checked);
                    Datosdeudor.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkDeudor16.Checked);

                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 1);
                    this._data.DatosPersonales.Add(Datosdeudor);                   

                    if (Datosdeudor == null)
                        this._data.DatosPersonales.Add(DatosdeudorNew);
                    else
                        Datosdeudor = DatosdeudorNew; 
                }
                #endregion

                #endregion
                #region Conyuge (TipoPersona 2)
                #region datacredito
                DTO_ccSolicitudDataCreditoUbica conyugeNew = conyuge != null ? conyuge : new DTO_ccSolicitudDataCreditoUbica();

                if (conyuge != null)
                {
                    conyuge.DireccionDir1.Value = this.txtDireccion1Cony.Text;
                    conyuge.DireccionDir2.Value = this.txtDireccion2Cony.Text;
                    conyuge.DireccionDir3.Value = this.txtDireccion3Cony.Text;
                    conyuge.TipoDir1.Value = this.txtTipoDireccion1Cony.Text;
                    conyuge.TipoDir1.Value = this.txtTipoDireccion2Cony.Text;
                    conyuge.TipoDir1.Value = this.txtTipoDireccion3Cony.Text;
                    conyuge.NumeroTel1.Value = this.txtTelefono1Cony.Text;
                    conyuge.NumeroTel2.Value = this.txtTelefono2Cony.Text;
                    conyuge.NumeroTel3.Value = this.txtTelefono3Cony.Text;
                    conyuge.TipoTel1.Value = this.txtTipoTelefono1Cony.Text;
                    conyuge.TipoTel2.Value = this.txtTipoTelefono2Cony.Text;
                    conyuge.TipoTel3.Value = this.txtTipoTelefono3Cony.Text;
                    conyuge.Celular1.Value = this.txtCelular1Cony.Text;
                    conyuge.Celular2.Value = this.txtCelular2Cony.Text;
                    conyuge.Email1.Value = this.txtCorreo1Cony.Text;
                    conyuge.Email1.Value = this.txtCorreo2Cony.Text;
                    //Digitados
                    conyuge.DireccionOtra.Value = this.txtDireccionDigitaCony.Text;
                    conyuge.DireccionOtraIND.Value = string.IsNullOrEmpty(conyuge.DireccionOtra.Value) ? false : true;
                    conyuge.EMailOtro.Value = this.txtCorreoDigitaCony.Text;
                    conyuge.EMailOtroIND.Value = string.IsNullOrEmpty(conyuge.EMailOtro.Value) ? false : true;
                    conyuge.TelefonoOtro.Value = this.txtTelefonoDigitaCony.Text;
                    conyuge.TelefonoOtroIND.Value = string.IsNullOrEmpty(conyuge.TelefonoOtro.Value) ? false : true;
                    conyuge.CelularOtro.Value = this.txtCelularDigitaCony.Text;
                    conyuge.CelularOtraIND.Value = string.IsNullOrEmpty(conyuge.CelularOtro.Value) ? false : true;
                    // Validados
                    conyuge.Direccion1IND.Value = Convert.ToBoolean(this.chkDir1Cony.Checked);
                    conyuge.Direccion2IND.Value = Convert.ToBoolean(this.chkDir2Cony.Checked);
                    conyuge.Direccion3IND.Value = Convert.ToBoolean(this.chkDir3Cony.Checked);

                    conyuge.Telefono1IND.Value = Convert.ToBoolean(this.chkTel1Cony.Checked);
                    conyuge.Telefono2IND.Value = Convert.ToBoolean(this.chkTel2Cony.Checked);
                    conyuge.Telefono3IND.Value = Convert.ToBoolean(this.chkTel2Cony.Checked);

                    conyuge.Celular1IND.Value = Convert.ToBoolean(this.chkCel1Cony.Checked);
                    conyuge.Celular2IND.Value = Convert.ToBoolean(this.chkCel2Cony.Checked);


                    conyuge.EMail1IND.Value = Convert.ToBoolean(this.chkCorreo1Cony.Checked);
                    conyuge.EMail2IND.Value = Convert.ToBoolean(this.chkCorreo2Cony.Checked);

                    if (conyuge == null)
                        this._data.DataCreditoUbica.Add(conyugeNew);
                    else
                        conyuge = conyugeNew; 
                }
                #endregion
                #region validacion Datos Personales
                DTO_drSolicitudDatosPersonales DatosconyugeNew = Datosconyuge != null ? Datosconyuge : new DTO_drSolicitudDatosPersonales();
                if (DatosconyugeNew != null)
                {
                    Datosconyuge.IndApellidoPri.Value = Convert.ToBoolean(this.checkCony1.Checked);
                    Datosconyuge.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCony2.Checked);
                    Datosconyuge.IndNombrePri.Value = Convert.ToBoolean(this.checkCony3.Checked);
                    Datosconyuge.IndNombreSdo.Value = Convert.ToBoolean(this.checkCony4.Checked);
                    Datosconyuge.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCony5.Checked);
                    Datosconyuge.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCony6.Checked);
                    Datosconyuge.IndTerceroID.Value = Convert.ToBoolean(this.checkCony7.Checked);
                    Datosconyuge.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCony8.Checked);
                    Datosconyuge.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCony9.Checked);
                    Datosconyuge.IndActEconomica1.Value = Convert.ToBoolean(this.checkCony10.Checked);
                    Datosconyuge.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCony11.Checked);
                    Datosconyuge.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCony12.Checked);
                    Datosconyuge.IndIngresosREG.Value = Convert.ToBoolean(this.checkCony13.Checked);
                    Datosconyuge.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCony14.Checked);
                    Datosconyuge.IndCorreo.Value = Convert.ToBoolean(this.checkCony15.Checked);
                    Datosconyuge.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCony16.Checked);

                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 2);
                    this._data.DatosPersonales.Add(Datosconyuge);

                    if (Datosconyuge == null)
                        this._data.DatosPersonales.Add(DatosconyugeNew);
                    else
                        Datosconyuge = DatosconyugeNew;
                }
                #endregion

                #endregion
                #region Codeudor1 (TipoPersona 3)
                # region datacredito
                DTO_ccSolicitudDataCreditoUbica cod1New = codeudor1 != null ? codeudor1: new DTO_ccSolicitudDataCreditoUbica();

                if (codeudor1 != null)
                {
                    codeudor1.DireccionDir1.Value = this.txtDireccion1Cod1.Text;
                    codeudor1.DireccionDir2.Value = this.txtDireccion2Cod1.Text;
                    codeudor1.DireccionDir3.Value = this.txtDireccion3Cod1.Text;
                    codeudor1.TipoDir1.Value = this.txtTipoDireccion1Cod1.Text;
                    codeudor1.TipoDir1.Value = this.txtTipoDireccion2Cod1.Text;
                    codeudor1.TipoDir1.Value = this.txtTipoDireccion3Cod1.Text;
                    codeudor1.NumeroTel1.Value = this.txtTelefono1Cod1.Text;
                    codeudor1.NumeroTel2.Value = this.txtTelefono2Cod1.Text;
                    codeudor1.NumeroTel3.Value = this.txtTelefono3Cod1.Text;
                    codeudor1.TipoTel1.Value = this.txtTipoTelefono1Cod1.Text;
                    codeudor1.TipoTel2.Value = this.txtTipoTelefono2Cod1.Text;
                    codeudor1.TipoTel3.Value = this.txtTipoTelefono3Cod1.Text;
                    codeudor1.Celular1.Value = this.txtCelular1Cod1.Text;
                    codeudor1.Celular2.Value = this.txtCelular2Cod1.Text;
                    codeudor1.Email1.Value = this.txtCorreo1Cod1.Text;
                    codeudor1.Email1.Value = this.txtCorreo2Cod1.Text;
                    //Digitados
                    codeudor1.DireccionOtra.Value = this.txtDireccionDigitaCod1.Text;
                    codeudor1.DireccionOtraIND.Value = string.IsNullOrEmpty(codeudor1.DireccionOtra.Value) ? false : true;
                    codeudor1.EMailOtro.Value = this.txtCorreoDigitaCod1.Text;
                    codeudor1.EMailOtroIND.Value = string.IsNullOrEmpty(codeudor1.EMailOtro.Value) ? false : true;
                    codeudor1.TelefonoOtro.Value = this.txtTelefonoDigitaCod1.Text;
                    codeudor1.TelefonoOtroIND.Value = string.IsNullOrEmpty(codeudor1.TelefonoOtro.Value) ? false : true;
                    codeudor1.CelularOtro.Value = this.txtCelularDigitaCod1.Text;
                    codeudor1.CelularOtraIND.Value = string.IsNullOrEmpty(codeudor1.CelularOtro.Value) ? false : true;

                    // Validados
                    codeudor1.Direccion1IND.Value = Convert.ToBoolean(this.chkDir1Cod1.Checked);
                    codeudor1.Direccion2IND.Value = Convert.ToBoolean(this.chkDir2Cod1.Checked);
                    codeudor1.Direccion3IND.Value = Convert.ToBoolean(this.chkDir3Cod1.Checked);

                    codeudor1.Telefono1IND.Value = Convert.ToBoolean(this.chkTel1Cod1.Checked);
                    codeudor1.Telefono2IND.Value = Convert.ToBoolean(this.chkTel2Cod1.Checked);
                    codeudor1.Telefono3IND.Value = Convert.ToBoolean(this.chkTel2Cod1.Checked);

                    codeudor1.Celular1IND.Value = Convert.ToBoolean(this.chkCel1Cod1.Checked);
                    codeudor1.Celular2IND.Value = Convert.ToBoolean(this.chkCel2Cod1.Checked);


                    codeudor1.EMail1IND.Value = Convert.ToBoolean(this.chkCorreo1Cod1.Checked);
                    codeudor1.EMail2IND.Value = Convert.ToBoolean(this.chkCorreo2Cod1.Checked);

                    if (codeudor1 == null)
                        this._data.DataCreditoUbica.Add(cod1New);
                    else
                        codeudor1 = cod1New;

                }                
                #endregion
                #region validacion Datos Personales
                DTO_drSolicitudDatosPersonales Datoscodeudor1New = Datoscodeudor1 != null ? Datoscodeudor1 : new DTO_drSolicitudDatosPersonales();
                if (Datoscodeudor1 != null)
                {
                    Datoscodeudor1.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod11.Checked);
                    Datoscodeudor1.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod12.Checked);
                    Datoscodeudor1.IndNombrePri.Value = Convert.ToBoolean(this.checkCod13.Checked);
                    Datoscodeudor1.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod14.Checked);
                    Datoscodeudor1.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod15.Checked);
                    Datoscodeudor1.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod16.Checked);
                    Datoscodeudor1.IndTerceroID.Value = Convert.ToBoolean(this.checkCod17.Checked);
                    Datoscodeudor1.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod18.Checked);
                    Datoscodeudor1.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod19.Checked);
                    Datoscodeudor1.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod110.Checked);
                    Datoscodeudor1.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod111.Checked);
                    Datoscodeudor1.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod112.Checked);
                    Datoscodeudor1.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod113.Checked);
                    Datoscodeudor1.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod114.Checked);
                    Datoscodeudor1.IndCorreo.Value = Convert.ToBoolean(this.checkCod115.Checked);
                    Datoscodeudor1.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod116.Checked);

                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 3);
                    this._data.DatosPersonales.Add(Datoscodeudor1);

                    if (Datoscodeudor1 == null)
                        this._data.DatosPersonales.Add(Datoscodeudor1New);
                    else
                        Datoscodeudor1 = Datoscodeudor1New;
                }
                #endregion

                #endregion

                #region Codeudor2 (TipoPersona 4)
                #region dataacredito
                DTO_ccSolicitudDataCreditoUbica cod2New = codeudor2 != null ? codeudor1 : new DTO_ccSolicitudDataCreditoUbica();
                if (codeudor2 != null)
                {
                    codeudor2.DireccionDir1.Value = this.txtDireccion1Cod2.Text;
                    codeudor2.DireccionDir2.Value = this.txtDireccion2Cod2.Text;
                    codeudor2.DireccionDir3.Value = this.txtDireccion3Cod2.Text;
                    codeudor2.TipoDir1.Value = this.txtTipoDireccion1Cod2.Text;
                    codeudor2.TipoDir1.Value = this.txtTipoDireccion2Cod2.Text;
                    codeudor2.TipoDir1.Value = this.txtTipoDireccion3Cod2.Text;
                    codeudor2.NumeroTel1.Value = this.txtTelefono1Cod2.Text;
                    codeudor2.NumeroTel2.Value = this.txtTelefono2Cod2.Text;
                    codeudor2.NumeroTel3.Value = this.txtTelefono3Cod2.Text;
                    codeudor2.TipoTel1.Value = this.txtTipoTelefono1Cod2.Text;
                    codeudor2.TipoTel2.Value = this.txtTipoTelefono2Cod2.Text;
                    codeudor2.TipoTel3.Value = this.txtTipoTelefono3Cod2.Text;
                    codeudor2.Celular1.Value = this.txtCelular1Cod2.Text;
                    codeudor2.Celular2.Value = this.txtCelular2Cod2.Text;
                    codeudor2.Email1.Value = this.txtCorreo1Cod2.Text;
                    codeudor2.Email1.Value = this.txtCorreo2Cod2.Text;
                    //Digitados
                    codeudor2.DireccionOtra.Value = this.txtDireccionDigitaCod2.Text;
                    codeudor2.DireccionOtraIND.Value = string.IsNullOrEmpty(codeudor2.DireccionOtra.Value) ? false : true;
                    codeudor2.EMailOtro.Value = this.txtCorreoDigitaCod2.Text;
                    codeudor2.EMailOtroIND.Value = string.IsNullOrEmpty(codeudor2.EMailOtro.Value) ? false : true;
                    codeudor2.TelefonoOtro.Value = this.txtTelefonoDigitaCod2.Text;
                    codeudor2.TelefonoOtroIND.Value = string.IsNullOrEmpty(codeudor2.TelefonoOtro.Value) ? false : true;
                    codeudor2.CelularOtro.Value = this.txtCelularDigitaCod2.Text;
                    codeudor2.CelularOtraIND.Value = string.IsNullOrEmpty(codeudor2.CelularOtro.Value) ? false : true;

                    // Validados
                    codeudor2.Direccion1IND.Value = Convert.ToBoolean(this.chkDir1Cod2.Checked);
                    codeudor2.Direccion2IND.Value = Convert.ToBoolean(this.chkDir2Cod2.Checked);
                    codeudor2.Direccion3IND.Value = Convert.ToBoolean(this.chkDir3Cod2.Checked);

                    codeudor2.Telefono1IND.Value = Convert.ToBoolean(this.chkTel1Cod2.Checked);
                    codeudor2.Telefono2IND.Value = Convert.ToBoolean(this.chkTel2Cod2.Checked);
                    codeudor2.Telefono3IND.Value = Convert.ToBoolean(this.chkTel2Cod2.Checked);

                    codeudor2.Celular1IND.Value = Convert.ToBoolean(this.chkCel1Cod2.Checked);
                    codeudor2.Celular2IND.Value = Convert.ToBoolean(this.chkCel2Cod2.Checked);


                    codeudor2.EMail1IND.Value = Convert.ToBoolean(this.chkCorreo1Cod2.Checked);
                    codeudor2.EMail2IND.Value = Convert.ToBoolean(this.chkCorreo2Cod2.Checked);
                    if (codeudor2 == null)
                        this._data.DataCreditoUbica.Add(cod2New);
                    else
                        codeudor2 = cod2New; 
                }
                #endregion
                #region validacion Datos Personales
                DTO_drSolicitudDatosPersonales Datoscodeudor2New = Datoscodeudor2 != null ? Datoscodeudor2 : new DTO_drSolicitudDatosPersonales();
                if (Datoscodeudor2 != null)
                {

                    Datoscodeudor2.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod21.Checked);
                    Datoscodeudor2.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod22.Checked);
                    Datoscodeudor2.IndNombrePri.Value = Convert.ToBoolean(this.checkCod23.Checked);
                    Datoscodeudor2.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod24.Checked);
                    Datoscodeudor2.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod25.Checked);
                    Datoscodeudor2.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod26.Checked);
                    Datoscodeudor2.IndTerceroID.Value = Convert.ToBoolean(this.checkCod27.Checked);
                    Datoscodeudor2.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod28.Checked);
                    Datoscodeudor2.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod29.Checked);
                    Datoscodeudor2.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod210.Checked);
                    Datoscodeudor2.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod211.Checked);
                    Datoscodeudor2.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod212.Checked);
                    Datoscodeudor2.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod213.Checked);
                    Datoscodeudor2.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod214.Checked);
                    Datoscodeudor2.IndCorreo.Value = Convert.ToBoolean(this.checkCod215.Checked);
                    Datoscodeudor2.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod216.Checked);

                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 4);
                    this._data.DatosPersonales.Add(Datoscodeudor2);

                    if (Datoscodeudor2 == null)
                        this._data.DatosPersonales.Add(Datoscodeudor2New);
                    else
                        Datoscodeudor2 = Datoscodeudor2New;
                }
                #endregion

                #endregion
                #region Codeudor3 (TipoPersona 5)
                #region datacredito
                DTO_ccSolicitudDataCreditoUbica cod3New = codeudor3 != null ? codeudor3 : new DTO_ccSolicitudDataCreditoUbica();
                if (codeudor3 != null)
                {
                    codeudor3.DireccionDir1.Value = this.txtDireccion1Cod3.Text;
                    codeudor3.DireccionDir2.Value = this.txtDireccion2Cod3.Text;
                    codeudor3.DireccionDir3.Value = this.txtDireccion3Cod3.Text;
                    codeudor3.TipoDir1.Value = this.txtTipoDireccion1Cod3.Text;
                    codeudor3.TipoDir1.Value = this.txtTipoDireccion2Cod3.Text;
                    codeudor3.TipoDir1.Value = this.txtTipoDireccion3Cod3.Text;
                    codeudor3.NumeroTel1.Value = this.txtTelefono1Cod3.Text;
                    codeudor3.NumeroTel2.Value = this.txtTelefono2Cod3.Text;
                    codeudor3.NumeroTel3.Value = this.txtTelefono3Cod3.Text;
                    codeudor3.TipoTel1.Value = this.txtTipoTelefono1Cod3.Text;
                    codeudor3.TipoTel2.Value = this.txtTipoTelefono2Cod3.Text;
                    codeudor3.TipoTel3.Value = this.txtTipoTelefono3Cod3.Text;
                    codeudor3.Celular1.Value = this.txtCelular1Cod3.Text;
                    codeudor3.Celular2.Value = this.txtCelular2Cod3.Text;
                    codeudor3.Email1.Value = this.txtCorreo1Cod3.Text;
                    codeudor3.Email1.Value = this.txtCorreo2Cod3.Text;
                    //Digitados
                    codeudor3.DireccionOtra.Value = this.txtDireccionDigitaCod3.Text;
                    codeudor3.DireccionOtraIND.Value = string.IsNullOrEmpty(codeudor3.DireccionOtra.Value) ? false : true;
                    codeudor3.EMailOtro.Value = this.txtCorreoDigitaCod3.Text;
                    codeudor3.EMailOtroIND.Value = string.IsNullOrEmpty(codeudor3.EMailOtro.Value) ? false : true;
                    codeudor3.TelefonoOtro.Value = this.txtTelefonoDigitaCod3.Text;
                    codeudor3.TelefonoOtroIND.Value = string.IsNullOrEmpty(codeudor3.TelefonoOtro.Value) ? false : true;
                    codeudor3.CelularOtro.Value = this.txtCelularDigitaCod3.Text;
                    codeudor3.CelularOtraIND.Value = string.IsNullOrEmpty(codeudor3.CelularOtro.Value) ? false : true;
                    // Validados
                    codeudor3.Direccion1IND.Value = Convert.ToBoolean(this.chkDir1Cod3.Checked);
                    codeudor3.Direccion2IND.Value = Convert.ToBoolean(this.chkDir2Cod3.Checked);
                    codeudor3.Direccion3IND.Value = Convert.ToBoolean(this.chkDir3Cod3.Checked);

                    codeudor3.Telefono1IND.Value = Convert.ToBoolean(this.chkTel1Cod3.Checked);
                    codeudor3.Telefono2IND.Value = Convert.ToBoolean(this.chkTel2Cod3.Checked);
                    codeudor3.Telefono3IND.Value = Convert.ToBoolean(this.chkTel2Cod3.Checked);

                    codeudor3.Celular1IND.Value = Convert.ToBoolean(this.chkCel1Cod3.Checked);
                    codeudor3.Celular2IND.Value = Convert.ToBoolean(this.chkCel2Cod3.Checked);


                    codeudor3.EMail1IND.Value = Convert.ToBoolean(this.chkCorreo1Cod3.Checked);
                    codeudor3.EMail2IND.Value = Convert.ToBoolean(this.chkCorreo2Cod3.Checked);
                    if (codeudor3 == null)
                        this._data.DataCreditoUbica.Add(cod3New);
                    else
                        codeudor3 = cod3New; 
                }
                #endregion
                #region validacion Datos Personales
                DTO_drSolicitudDatosPersonales Datoscodeudor3New = Datoscodeudor3 != null ? Datoscodeudor3 : new DTO_drSolicitudDatosPersonales();
                if (Datoscodeudor3 != null)
                {

                    Datoscodeudor3.IndApellidoPri.Value = Convert.ToBoolean(this.checkCod31.Checked);
                    Datoscodeudor3.IndApellidoSdo.Value = Convert.ToBoolean(this.checkCod32.Checked);
                    Datoscodeudor3.IndNombrePri.Value = Convert.ToBoolean(this.checkCod33.Checked);
                    Datoscodeudor3.IndNombreSdo.Value = Convert.ToBoolean(this.checkCod34.Checked);
                    Datoscodeudor3.IndTerceroDocTipoID.Value = Convert.ToBoolean(this.checkCod35.Checked);
                    Datoscodeudor3.IndFechaExpDoc.Value = Convert.ToBoolean(this.checkCod36.Checked);
                    Datoscodeudor3.IndTerceroID.Value = Convert.ToBoolean(this.checkCod37.Checked);
                    Datoscodeudor3.IndFechaNacimiento.Value = Convert.ToBoolean(this.checkCod38.Checked);
                    Datoscodeudor3.IndEstadoCivil.Value = Convert.ToBoolean(this.checkCod39.Checked);
                    Datoscodeudor3.IndActEconomica1.Value = Convert.ToBoolean(this.checkCod310.Checked);
                    Datoscodeudor3.IndFuenteIngresos1.Value = Convert.ToBoolean(this.checkCod311.Checked);
                    Datoscodeudor3.IndFuenteIngresos2.Value = Convert.ToBoolean(this.checkCod312.Checked);
                    Datoscodeudor3.IndIngresosREG.Value = Convert.ToBoolean(this.checkCod313.Checked);
                    Datoscodeudor3.IndIngresosSOP.Value = Convert.ToBoolean(this.checkCod314.Checked);
                    Datoscodeudor3.IndCorreo.Value = Convert.ToBoolean(this.checkCod315.Checked);
                    Datoscodeudor3.IndCiudadResidencia.Value = Convert.ToBoolean(this.checkCod316.Checked);

                    this._data.DatosPersonales.RemoveAll(x => x.TipoPersona.Value == 5);
                    this._data.DatosPersonales.Add(Datoscodeudor3);

                    if (Datoscodeudor3 == null)
                        this._data.DatosPersonales.Add(Datoscodeudor3New);
                    else
                        Datoscodeudor3 = Datoscodeudor3New;
                }
                #endregion

                #endregion


            }
            #endregion
        }

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
                //this._clienteID
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
                    }
                    else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                    {
                        //Mostrar mensaje de que esta libranza esta cerrada
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                        CleanData();
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
                if (this._actFlujo != null)
                {
                    this.actChequeo = this._bc.AdministrationModel.drSolicitudDatosChequeados_GetByActividadNumDoc(this._actFlujo.ID.Value, this._ctrl.NumeroDoc.Value.Value, this._data.SolicituDocu.VersionNro.Value.Value);
                    //if (this.actChequeo==null ||this.actChequeo.Count == 0)
                    {
                        this.actChequeoBase = _bc.AdministrationModel.drActividadChequeoLista_GetByActividad(this._actFlujo.ID.Value);
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

                            string Valida="";
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
                                chequeo.IncluidoInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 1 ? x.ChequeadoInd.Value.Value : false));
                                chequeo.IncluidoConyugeInd.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 2 ? x.ChequeadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor1Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 3 ? x.ChequeadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor2Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 4 ? x.ChequeadoInd.Value.Value : false));
                                chequeo.IncluidoCodeudor3Ind.Value = (this.actChequeo.Any(x => x.NroRegistro.Value == basic.consecutivo.Value.Value && x.TipoPersona.Value == 5 ? x.ChequeadoInd.Value.Value : false));
                            }
                            if (basic.EmpleadoInd.Value.Value)
                            {
                                addChequeo1 = this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1) ? true : false;

                                if (this._data.DatosPersonales.Any(x => x.FuenteIngresos1.Value == 1 && x.TipoPersona.Value == 1 ? true : false))
                                {
                                    addTP1=true;
                                    
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
                            chequeo.Incluye.Value=Valida;
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
        private void gvActividades_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
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
        private void riPopupActividad_QueryPopUp(object sender, CancelEventArgs e)
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
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Delete);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSendtoAppr.Visible = true;
                    FormProvider.Master.itemSearch.Visible = false;
                }
                FormProvider.Master.itemSendtoAppr.ToolTipText = "Generar R-V-C";
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "Form_FormClosed"));
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
            if (this.tabControl.SelectedTabPageIndex == 0) // Datos Personales
            {
                //if (this.ValidateData())
                {
                    _mensajeGuardar = false;
                    this.TBSave();
                    _mensajeGuardar = true;
                    this.tabControl.SelectedTabPageIndex = 1; 
                }
                
            }
            else if (this.tabControl.SelectedTabPageIndex == 1) //Ubicabilidad
            {
                if (this.ValidateHeader())
                {
                    _mensajeGuardar = false;
                    this.TBSave();
                    _mensajeGuardar = true;
                    if (!this._readOnly)
                    {
                        DTO_TxResult _resPreliminar = _bc.AdministrationModel.Genera_Perfil(this._data.DocCtrl.PeriodoDoc.Value.Value, this._data.SolicituDocu.NumeroDoc.Value.Value);
                        string msg = _resPreliminar.ResultMessage;
                        MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                    }
                    this.tabControl.SelectedTabPageIndex = 2;
                }


            }
            else if (this.tabControl.SelectedTabPageIndex == 2) //Lista de Chequeo
            {
                this.tabControl.SelectedTabPageIndex = 0;
            }
        }

        /// <summary>
        /// Atras
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e"><Objeto que envia el evento/param>
        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 2;
            else if (this.tabControl.SelectedTabPageIndex == 1)
                this.tabControl.SelectedTabPageIndex = 0;
            else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 1;
        }

        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor1.Dock == DockStyle.Fill)
                linkCodeudor1.Dock = DockStyle.None;
            else
                linkCodeudor1.Dock = DockStyle.Fill;
        }

        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor2.Dock == DockStyle.Fill)
                linkCodeudor2.Dock = DockStyle.None;
            else
                linkCodeudor2.Dock = DockStyle.Fill;
        }

        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor3.Dock == DockStyle.Fill)
                linkCodeudor3.Dock = DockStyle.None;
            else
                linkCodeudor3.Dock = DockStyle.Fill;
        }

        private void linkConyuge_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyuge.Dock == DockStyle.Fill)
                this.linkConyuge.Dock = DockStyle.None;
            else
                this.linkConyuge.Dock = DockStyle.Fill;
        }

        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
            {
             
            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
             
            }
            else if (this.tabControl.SelectedTabPageIndex == 2)
            {
             
            }

        }
        
        private void txtDireccionDeudor_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            CodificacionDireccion dir = new CodificacionDireccion((DevExpress.XtraEditors.ButtonEdit)sender);
            dir.ShowDialog();
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

        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {

            try
            {
                this.AssignValues(false);
                this.gvActividades.PostEditor();
//                if (this.ValidateHeader())
  //              {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, false);
 
                    //DTO_TxResult result = _bc.AdministrationModel.drSolicitudDatosChequeados_Add(this.actChequeoBase);
                    if (_mensajeGuardar)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                        MessageBox.Show(string.Format(msg, this._libranza));
                    }
    //            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar la revisión de la solicitud?  ");
                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                string Observacion = this.ValidateListas();
                if (!string.IsNullOrEmpty(Observacion))
                {
                    this._data.SolicituDocu.NegociosGestionarInd.Value = true;
                    this._data.SolicituDocu.Observacion.Value = Observacion;
                }

                //if (this.checkEdit1.Checked)
                //    this._data.SolicituDocu.NegociosGestionarInd.Value = true;

                this.AssignValues(false);
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                    MessageBox.Show(string.Format(msg, this._libranza));
                    if (result.Result == ResultValue.OK)
                        FormProvider.CloseCurrent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Datacredito.cs", "TBSave"));
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
                        
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = false;
                        this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = false;

                        if (row.EmpleadoInd.Value.Value)
                        {
                            if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 1))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 1))
                               this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
    
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 1))
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
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 2))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 2))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 2))
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
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if (this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 3))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;
                            }
                        }
                        if (row.IndependienteInd.Value.Value)
                        {
                            if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 4))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 4))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 4))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor3Ind"].OptionsColumn.AllowEdit = true;
                            }
                        }
                        if (row.PensionadoInd.Value.Value)
                        {
                            if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 1 && x.FuenteIngresos1.Value == 5))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoInd"].OptionsColumn.AllowEdit = true;
                            if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 2 && x.FuenteIngresos1.Value == 5))
                                this.gvActividades.Columns[this._unboundPrefix + "IncluidoConyugeInd"].OptionsColumn.AllowEdit = true;
                            if (row.ExcluyeCodInd.Value.Value)
                            {
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 3 && x.FuenteIngresos1.Value == 5))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor1Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 4 && x.FuenteIngresos1.Value == 5))
                                    this.gvActividades.Columns[this._unboundPrefix + "IncluidoCodeudor2Ind"].OptionsColumn.AllowEdit = true;
                                if(this._data.DatosPersonales.Any(x => x.TipoPersona.Value == 5 && x.FuenteIngresos1.Value == 5))
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

        private void btnEditar_Click(object sender, EventArgs e)
        {

        }

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

                    DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
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
            MessageBox.Show(string.Format(msg, this._libranza));
            if (result.Result == ResultValue.OK)
                FormProvider.CloseCurrent();
                               
            
        }
    }
}

