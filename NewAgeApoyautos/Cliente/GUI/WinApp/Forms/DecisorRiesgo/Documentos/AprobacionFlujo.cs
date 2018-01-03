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
    public partial class AprobacionFlujo : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private string _documentDesc;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

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
        protected Dictionary<string, string> actFlujoForReversion = new Dictionary<string, string>();
        protected List<string> actividadesCombo = new List<string>();
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
        public AprobacionFlujo()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public AprobacionFlujo(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public AprobacionFlujo(int libranza, bool readOnly, int documentoID,string Detalle)
        {
            InitializeComponent();
            try
            {
                this._documentID = documentoID;
                this._documentDesc = Detalle;
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
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
                #region Trae la solicitud
                this.readOnly = readOnly;
                this.LoadData(libranza);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujoFinanciera.cs", "AprobacionFlujoFinanciera"));
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
                this.LoadData(libranza);
                if (this._actFlujo != null)
                {
                    //Carga la actividades a devolver
                    List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                    foreach (DTO_glActividadFlujo act in actPadres)
                    {
                        this.actividadesCombo.Add(act.Descriptivo.Value);
                        this.actFlujoForReversion.Add(act.ID.Value , act.Descriptivo.Value);
                    }

                }

                    string ActFlujoDestimiento = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Desestimiento);
                    string ActFlujoNegGestionar = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_NegGestionar);
                    string ActFlujoEvaluacion = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_Evaluacion);
                    string ActFlujoRVC = this._bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.dr_RVC);

                    string ActFlujoNegGestionarDesc = this._bc.GetControlDescripcionValueByCompany(ModulesPrefix.dr, AppControl.dr_NegGestionar);
                    string ActFlujoEvaluacionDesc = this._bc.GetControlDescripcionValueByCompany(ModulesPrefix.dr, AppControl.dr_Evaluacion);
                    string ActFlujoRVCDesc = this._bc.GetControlDescripcionValueByCompany(ModulesPrefix.dr, AppControl.dr_RVC);

//                this._data.SolicituDocu.ActividadFlujoNegociosGestionarID
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "AprobacionFlujo"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "AddComponentesCols"));
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
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteRadica.Value.ToString();
                    this.txtDeudor.Text = this._data.SolicituDocu.Nombre.Value.ToString();
                    this.masterVitrina.Value = this._data.SolicituDocu.ConcesionarioID.Value;
                    this.masterZona.Value = this._data.SolicituDocu.ZonaID.Value;
                    this.masterCiudadGeneral.Value = this._data.SolicituDocu.Ciudad.Value;
                    //this.txtObservacionHist.Text = this._data.OtrosDatos.Observacion.Value;
                    if (vehiculo != null)
                    {
                        // Datos Vehiculo
                        #region datos Vehiculo
                        this.cmbServicioGeneral.EditValue = vehiculo.Servicio.Value;
                        this.chkCeroKMGeneral.Checked = vehiculo.CeroKmInd.Value.Value;
                        #endregion 
                    }
                    this.txtObservacionHist.Text = this._data.SolicituDocu.Observacion.Value;
                }
                else
                {
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
            
            this._frmModule = ModulesPrefix.dr;
      
            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, false, true, true, false);
            this._bc.InitMasterUC(this.masterCiudadGeneral, AppMasters.glLugarGeografico, false, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, false, true, true, false);
            this.lblEtapa.Text = this._documentDesc;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "AddComponentesCols"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "EnableHeader"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "CleanData"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "ValidateHeader"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "GetValuesSolicitud"));
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
                    //FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);

                    FormProvider.Master.itemSave.Enabled = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = true;
                    FormProvider.Master.itemSendtoAppr.ToolTipText = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "Form_FormClosed"));
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
        /// Abre los datos personales del solicitante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDatosPersonales_Click(object sender, EventArgs e)
        {
            if (this._data != null)
            {
                Type frm = typeof(DigitacionSolicitudNuevos);
                FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true,false });

            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "TBNew"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionFlujo.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar?  ");
                if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                this.AssignValues(false);
                this._data.OtrosDatos.Observacion.Value = string.Empty;

                if (this.ValidateHeader())
                {
                    this._data.OtrosDatos.Observacion.Value = string.Empty;
                    this._data.SolicituDocu.NegociosGestionarInd.Value = false;
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    DTO_TxResult result = _bc.AdministrationModel.GestionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, this._actividadChequeo, true);

                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                    MessageBox.Show(string.Format(msg, this._libranzaID));
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

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            Type frm = typeof(Perfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true });
            
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
 
