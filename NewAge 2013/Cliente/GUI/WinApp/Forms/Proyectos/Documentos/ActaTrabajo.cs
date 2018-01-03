using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ActaTrabajo : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        private bool _prefijoFocus = false;
        private int _numeroDocActa = 0;
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_coProyecto _proyecto = null;
        private int _numeroDocProy = 0;
        private DTO_pyProyectoTarea _rowTareaCurrent = new DTO_pyProyectoTarea();
        private DTO_pyActaTrabajoDeta _rowActaCurrent = new DTO_pyActaTrabajoDeta();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyActaTrabajoDeta> _listActaTrabajoExist = new List<DTO_pyActaTrabajoDeta>();
        private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        private string monedaLocal = string.Empty;
        private bool sendToApproveInd = false;
        #endregion

        #region Delegados

        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private  void RefreshGridMethod()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "RefreshGridMethod"));
            }
        }

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private  void SaveMethod() { this.RefreshForm(); }

        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void SendToApproveMethod() { this.RefreshForm(); }

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            
            this.gcTarea.DataSource = this._listTareasAll;
            this.gcTarea.RefreshDataSource();
            this.gcActas.DataSource = null;
            this.gcActas.RefreshDataSource();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        private bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ActaTrabajo()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
              
                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo.cs", "PlanecionTiempo"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Principal

            GridColumn tareaCliente = new GridColumn();
            tareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            tareaCliente.UnboundType = UnboundColumnType.String;
            tareaCliente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            tareaCliente.AppearanceCell.Options.UseTextOptions = true;
            tareaCliente.AppearanceCell.Options.UseFont = true;
            tareaCliente.VisibleIndex = 1;
            tareaCliente.Width = 40;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(tareaCliente);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaID.AppearanceCell.Options.UseTextOptions = true;
            TareaID.AppearanceCell.Options.UseFont = true;
            TareaID.VisibleIndex = 2;
            TareaID.Width = 70;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaID);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 3;
            descriptivo.Width = 590;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(descriptivo);

            GridColumn CantidadREC = new GridColumn();
            CantidadREC.FieldName = this.unboundPrefix + "CantidadREC";
            CantidadREC.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ActaTrabajo + "_CantidadREC");
            CantidadREC.UnboundType = UnboundColumnType.Decimal;
            CantidadREC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadREC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadREC.AppearanceCell.Options.UseTextOptions = true;
            CantidadREC.AppearanceCell.Options.UseFont = true;
            CantidadREC.VisibleIndex = 6;
            CantidadREC.Width = 50;
            CantidadREC.Visible = false;
            CantidadREC.ColumnEdit = this.editValue3Cant;
            CantidadREC.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(CantidadREC);

            #endregion

            #region Grilla Recursos
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 60;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 150;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(RecursoDesc);

            GridColumn PrefDocOC = new GridColumn();
            PrefDocOC.FieldName = this.unboundPrefix + "PrefDocOC";
            PrefDocOC.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_PrefDocOC");
            PrefDocOC.UnboundType = UnboundColumnType.String;
            PrefDocOC.VisibleIndex = 2;
            PrefDocOC.Width = 60;
            PrefDocOC.Visible = true;
            PrefDocOC.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(PrefDocOC);

            GridColumn ProveedorID = new GridColumn();
            ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
            ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_ProveedorID");
            ProveedorID.UnboundType = UnboundColumnType.String;
            ProveedorID.VisibleIndex = 3;
            ProveedorID.Width = 120;
            ProveedorID.Visible = true;
            ProveedorID.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(ProveedorID);

            GridColumn UnidadInvIDRec = new GridColumn();
            UnidadInvIDRec.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDRec.UnboundType = UnboundColumnType.String;           
            UnidadInvIDRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvIDRec.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvIDRec.VisibleIndex = 4;
            UnidadInvIDRec.Width = 50;
            UnidadInvIDRec.Visible = true;
            UnidadInvIDRec.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(UnidadInvIDRec);

            GridColumn CantidadOC = new GridColumn();
            CantidadOC.FieldName = this.unboundPrefix + "CantidadOC";
            CantidadOC.Caption = _bc.GetResource(LanguageTypes.Forms,"Cant OC");
            CantidadOC.UnboundType = UnboundColumnType.Decimal;
            CantidadOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadOC.AppearanceCell.Options.UseTextOptions = true;
            CantidadOC.VisibleIndex = 5;
            CantidadOC.Width = 40;
            CantidadOC.Visible = false;
            CantidadOC.ColumnEdit = this.editValue3Cant;
            CantidadOC.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(CantidadOC);

            GridColumn CantPendiente = new GridColumn();
            CantPendiente.FieldName = this.unboundPrefix + "CantidadPend";
            CantPendiente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ActaTrabajo + "_CantidadPend");
            CantPendiente.UnboundType = UnboundColumnType.Decimal;
            CantPendiente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPendiente.AppearanceCell.Options.UseTextOptions = true;
            CantPendiente.VisibleIndex = 6;
            CantPendiente.Width = 50;
            CantPendiente.Visible = true;
            CantPendiente.ColumnEdit = this.editValue3Cant;
            CantPendiente.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(CantPendiente);

            GridColumn CantidadRECRecu = new GridColumn();
            CantidadRECRecu.FieldName = this.unboundPrefix + "CantidadREC";
            CantidadRECRecu.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ActaTrabajo + "_CantidadREC");
            CantidadRECRecu.UnboundType = UnboundColumnType.Decimal;
            CantidadRECRecu.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadRECRecu.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadRECRecu.AppearanceCell.Options.UseTextOptions = true;
            CantidadRECRecu.AppearanceCell.Options.UseFont = true;
            CantidadRECRecu.AppearanceCell.BackColor = Color.PeachPuff;
            CantidadRECRecu.AppearanceCell.Options.UseBackColor = true;
            CantidadRECRecu.VisibleIndex = 7;
            CantidadRECRecu.Width = 50;
            CantidadRECRecu.Visible = true;
            CantidadRECRecu.ColumnEdit = this.editValue3Cant;
            CantidadRECRecu.OptionsColumn.AllowEdit = true;
            this.gvActas.Columns.Add(CantidadRECRecu);

            GridColumn ValorUniREC = new GridColumn();
            ValorUniREC.FieldName = this.unboundPrefix + "ValorUniREC";
            ValorUniREC.Caption = _bc.GetResource(LanguageTypes.Forms, "Valor Uni OC");
            ValorUniREC.UnboundType = UnboundColumnType.Decimal;
            ValorUniREC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorUniREC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorUniREC.AppearanceCell.Options.UseTextOptions = true;
            ValorUniREC.AppearanceCell.Options.UseFont = true;
            ValorUniREC.AppearanceCell.BackColor = Color.PeachPuff;
            ValorUniREC.AppearanceCell.Options.UseBackColor = true;
            ValorUniREC.VisibleIndex = 8;
            ValorUniREC.Width = 50;
            ValorUniREC.Visible = true;
            ValorUniREC.ColumnEdit = this.editValue3Cant;
            ValorUniREC.OptionsColumn.AllowEdit = false;
            this.gvActas.Columns.Add(ValorUniREC);

            GridColumn TipoRecurso = new GridColumn();
            TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecurso.UnboundType = UnboundColumnType.Integer;
            TipoRecurso.Width = 80;
            TipoRecurso.Visible = false;
            this.gvActas.Columns.Add(TipoRecurso);

            this.gvActas.OptionsBehavior.Editable = true;
            this.gvActas.OptionsView.ColumnAutoWidth = true;
            #endregion
        }

        /// <summary>
        /// Carga la información del documento
        /// </summary>
        private void CargarInformacion(bool exist, DTO_SolicitudTrabajo transaccion)
        {
            try
            {
                if (exist)
                {
                    this.masterProyecto.Value = transaccion.DocCtrl.ProyectoID.Value;
                    this.masterPrefijo.Value = transaccion.DocCtrl.PrefijoID.Value;
                    this.txtNro.Text = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                    this.txtDescripcion.Text = transaccion.HeaderProyecto.DescripcionSOL.Value;
                    this.masterCliente.Value = transaccion.HeaderProyecto.ClienteID.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "CargarInformacion"));
            }
        }

        /// <summary>
        /// Restaura los valores iniciales
        /// </summary>
        /// <param name="p"></param>
        private void CleanHeader(bool p)
        {
            try
            {
                if (p)
                {
                    this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                    this.masterProyecto.Value = string.Empty;
                    this.masterCliente.Value = string.Empty;
                    this.txtObservacionGral.Text = string.Empty;
                    this.txtNro.Text = string.Empty;
                    this.txtNroActa.Text = string.Empty;
                    this.txtDescripcion.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "CleanHeader"));
            }
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, false, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
                this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                this.dtFechaActa.DateTime = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Habilita o desabuilita el header
        /// </summary>
        /// <param name="p"></param>
        private void EnableHeader(bool p)
        {
            //this.masterClaseServicio.EnableControl(p);
            //this.masterPrefijo.EnableControl(p);
            //this.masterAreaFuncional.EnableControl(p);
            //this.masterCliente.EnableControl(p);
            //this.masterContrato.EnableControl(p);
            ////this.masterProyecto.EnableControl(p);
            //this.masterCentroCto.EnableControl(p);
            //this.masterResponsableEmp.EnableControl(p);
            //this.txtNro.Enabled = p;
            //this.cmbJerarquia.Enabled = p;
            //this.chkAPUIncluyeAIU.Enabled = p;
            //this.dtFechaInicio.Enabled = p;
            //this.txtPorAdmClient.Enabled = p;
            //this.txtPorImprClient.Enabled = p;
            //this.txtPorUtilClient.Enabled = p;
            //this.txtPorAdmEmp.Enabled = p;
            //this.txtPorImprEmp.Enabled = p;
            //this.txtPorUtilEmp.Enabled = p;
            ////this.txtDescripcion.Enabled = p;
            ////this.txtSolicitante.Enabled = p;
            //this.txtResposableCli.Enabled = p;
            //this.txtCorreo.Enabled = p;
            //this.txtTelefono.Enabled = p;
            ////this.txtObservaciones.Enabled = p;
            //this.cmbTipoSolicitud.Enabled = p;
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                        this.txtPrefix.Text = this.prefijoID;

                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    if (this.documentID == AppDocuments.ComprobanteManual || this.documentID == AppDocuments.DocumentoContable)
                        this.dtPeriod.Enabled = true;
                    else
                        this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData(string prefijoID, int? docNro, int? numeroDoc, string proyectoID)
        {
            try
            {
                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, false, true, false, false);
                
                if (transaccion != null)
                {
                    if (transaccion.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }
                    this._numeroDocProy = transaccion.DocCtrl.NumeroDoc.Value.Value;
                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto.OrderBy(x=>x.CapituloGrupoID.Value).ToList();
                    this._listMvtos = transaccion.Movimientos;
                    this._ctrlProyecto = transaccion.DocCtrl;
                    this._proyecto = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this._ctrlProyecto.ProyectoID.Value, true);
                    this._proyecto.ConsActaTrabajo.Value = this._proyecto.ConsActaTrabajo.Value ?? 0;
                    this.txtNroActa.Text = this._proyecto.ConsActaTrabajo.Value.ToString();
                    this.LoadActaExistente();

                    this.CargarInformacion(true, transaccion);

                    if (this._listActaTrabajoExist.Count > 0)
                        this.txtObservacionGral.Text = this._ctrl.Observacion.Value;
                    else
                        this._listActaTrabajoExist = new List<DTO_pyActaTrabajoDeta>();

                    //Obtiene las Orden de Compra Pendientes
                    List<DTO_prOrdenCompraResumen> resumenOrdCompraPend = this._bc.AdministrationModel.OrdenCompra_GetResumen(this.documentID, null, ModulesPrefix.pr, null);
                    resumenOrdCompraPend = resumenOrdCompraPend.FindAll(x => x.Documento4ID.Value == transaccion.DocCtrl.NumeroDoc.Value).ToList();

                    foreach (DTO_pyProyectoTarea tarea in _listTareasAll)
                    {
                        //Filtras solo servicios
                        tarea.Detalle = tarea.Detalle.FindAll(x => x.TipoRecurso.Value != (byte)TipoRecurso.Insumo).ToList();
                        foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                        {
                            //Filtra los mvtos que ya tengan una solicitud de OC
                            det.DetalleMvto = det.DetalleMvto.FindAll(x => x.CantidadPROV.Value > 0).ToList();
                            foreach (DTO_pyProyectoMvto mvto in det.DetalleMvto)
                            {
                                //Valida si el mvto tiene cantidades por recibir pendientes
                                if (mvto.CantidadTOT.Value > mvto.CantidadREC.Value)
                                {
                                    //Valida si el mvto ya tiene registrada una Orden de Compra para incluir un registro
                                    if (resumenOrdCompraPend.Exists(x => x.Detalle4ID.Value == mvto.Consecutivo.Value))
                                    {
                                        DTO_pyActaTrabajoDeta acta = new DTO_pyActaTrabajoDeta();
                                        acta.NumeroDoc.Value = 0;
                                        acta.ConsecTarea.Value = mvto.ConsecTarea.Value;
                                        acta.ConsProyMvto.Value = mvto.Consecutivo.Value;
                                        acta.ConsOrdCompraDeta.Value = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).OrdCompraDetaID.Value;
                                        acta.NumDocOrdCompra.Value = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).OrdCompraDocuID.Value;
                                        acta.NumDocProyecto.Value = transaccion.DocCtrl.NumeroDoc.Value;
                                        acta.TareaCliente.Value = tarea.TareaCliente.Value;
                                        acta.RecursoID.Value = det.RecursoID.Value;
                                        acta.RecursoDesc.Value = det.RecursoDesc.Value;
                                        acta.UnidadInvID.Value = det.UnidadInvID.Value;
                                        acta.TipoRecurso.Value = det.TipoRecurso.Value;

                                        //valida si ya existe un acta de trabajo con la referencia actual
                                        if (!this._listActaTrabajoExist.Exists(x => x.ConsecTarea.Value == tarea.Consecutivo.Value))
                                        {
                                            acta.CantidadOC.Value = resumenOrdCompraPend.Where(x => x.CodigoBSID.Value == det.RecursoID.Value).Sum(x => x.CantidadDoc4.Value);// - mvto.CantidadREC.Value;
                                            acta.CantidadREC.Value = 0;
                                            acta.CantidadPend.Value = resumenOrdCompraPend.Where(x => x.CodigoBSID.Value == det.RecursoID.Value).Sum(x => x.CantidadOC.Value);// - mvto.CantidadREC.Value;
                                            acta.PrefDocOC = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).PrefDocOC;
                                            acta.ProveedorID.Value = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).ProveedorID.Value;
                                            DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, acta.ProveedorID.Value, true);
                                            acta.ProveedorID.Value = prov != null ? (acta.ProveedorID.Value + "-"+ prov.Descriptivo.Value): acta.ProveedorID.Value;
                                            acta.MonedaOrdCompra.Value = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).MonedaOrdenOC.Value;
                                            acta.ValorUniREC.Value = resumenOrdCompraPend.Find(x => x.Detalle4ID.Value == mvto.Consecutivo.Value).ValorUni.Value;
                                            this._listActaTrabajoExist.Add(acta);
                                        }                                        
                                    }        
                                }                            
                            }
                        }
                        tarea.CantidadREC.Value = this._listActaTrabajoExist.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value).Sum(y => y.CantidadREC.Value);
                    }

                    #region Valida el tipo de Proyecto
                    DTO_pyClaseProyecto clase = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, transaccion.HeaderProyecto.ClaseServicioID.Value, true);
                    if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                    {
                        this.gvTarea.Columns[this.unboundPrefix + "CapituloDesc"].UnGroup();
                        this.gvTarea.Columns[this.unboundPrefix + "CapituloDesc"].Visible = false;

                    }
                    else if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                    {
                        //this.gvTarea.Columns[this.unboundPrefix + "CapituloDesc"].Group();
                        //this.gvTarea.Columns[this.unboundPrefix + "CapituloDesc"].SortOrder = ColumnSortOrder.None;
                    }
                    #endregion


                    this.UpdateValues();
                    this.LoadGrids();
                    this.EnableHeader(this._numeroDocActa != 0 ? false : true);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this.CargarInformacion(false, null);
                    this._ctrl = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected void LoadDocCtrl()
        {
            #region Load DocumentoControl
            if (this._numeroDocActa == 0)
            {
                this._ctrl = new DTO_glDocumentoControl();
                this._ctrl.MonedaID.Value = this.monedaLocal;
                this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.PrefijoID.Value = this.masterPrefijo.Value;
                this._ctrl.TasaCambioCONT.Value = 0;
                this._ctrl.TasaCambioDOCU.Value = 0;
                this._ctrl.DocumentoNro.Value = 0;
                this._ctrl.DocumentoID.Value = AppDocuments.ActaTrabajo;
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.seUsuarioID.Value = this.userID;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.ConsSaldo.Value = !this._proyecto.ConsActaTrabajo.Value.HasValue ? 1 : this._proyecto.ConsActaTrabajo.Value;
                this._ctrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                this._ctrl.Observacion.Value = this.txtObservacionGral.Text;
                this._ctrl.FechaDoc.Value = this.dtFechaActa.DateTime;
                this._ctrl.Descripcion.Value = "Acta de Trabajo Proyecto";
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0; 
            }
            else
            {
                this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(_numeroDocActa);
                this._ctrl.ConsSaldo.Value = !this._ctrl.ConsSaldo.Value.HasValue || this._ctrl.ConsSaldo.Value == 0 ? this._proyecto.ConsActaEntrega.Value : this._ctrl.ConsSaldo.Value;

            }
            #endregion            

        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcTarea.DataSource = null;
                this.gcActas.DataSource = null;
                this.gcTarea.DataSource = this._listTareasAll;
                this.gcTarea.RefreshDataSource();
                this.gcActas.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle existente
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadActaExistente()
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.txtNroActa.Text) && this._numeroDocProy != 0)
                    {
                        int consActa = Convert.ToInt32(this.txtNroActa.Text);
                        DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                        filter.DocumentoID.Value = AppDocuments.ActaTrabajo;
                        filter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                        filter.ConsSaldo.Value = consActa;
                        filter.Estado.Value = 2;//Para aprobacion
                        List<DTO_glDocumentoControl> ctrlList = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                        //Si existen actas sin aprobar las asigna para el doc actual
                        if (ctrlList != null && ctrlList.Count > 0)
                        {
                            this._ctrl = ctrlList.First();
                            this._numeroDocActa = this._ctrl.NumeroDoc.Value.Value;
                            this.dtFechaActa.DateTime = this._ctrl.FechaDoc.Value.Value;
                            DTO_pyActaTrabajoDeta actaFilter = new DTO_pyActaTrabajoDeta();
                            actaFilter.NumDocProyecto.Value = this._numeroDocProy;
                            actaFilter.NumeroDoc.Value = this._numeroDocActa;
                            this._listActaTrabajoExist = this._bc.AdministrationModel.ActasTrabajo_Load(this._numeroDocActa);
                        }
                        else
                        {
                            this._listActaTrabajoExist = new List<DTO_pyActaTrabajoDeta>();
                            this._numeroDocActa = 0;
                            this._proyecto.ConsActaTrabajo.Value = consActa + 1;
                            this.txtNroActa.Text = this._proyecto.ConsActaTrabajo.Value.ToString();
                        }
                        this.LoadGrids();
                        this.gcActas.RefreshDataSource();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-txtNroActa_Leave", "LoadData"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {
            this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.txtNro.Text = string.Empty;
            this.txtObservaciones.Text = string.Empty;
            this.CleanHeader(true);
            this.EnableHeader(true);

            this._ctrl = null;
            this._numeroDocActa = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu();
            this._rowTareaCurrent = new DTO_pyProyectoTarea();
            this._rowActaCurrent = new DTO_pyActaTrabajoDeta();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();

            this.gcTarea.DataSource = this._listTareasAll;
            this.gcTarea.RefreshDataSource();

            this.gcActas.DataSource = null;
            this.gcActas.RefreshDataSource();
            this.sendToApproveInd = false;
            this._listActaTrabajoExist = new List<DTO_pyActaTrabajoDeta>();

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private  void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.ActaTrabajo;
            this.AddGridCols();
            this.InitControls();
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
            this.txtNro.Focus();
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private  bool ValidateRow(int fila)
        {
            return true;
        }

        /// <summary>
        /// Actualiza los costos generales
        /// </summary>
        private void UpdateValues()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemPrint.Enabled = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtPeriod_EditValueChanged()
        {
            try
            {
                EstadoPeriodo validPeriod = _bc.AdministrationModel.CheckPeriod(this.dtPeriod.DateTime, this.frmModule);
                if (this.dtPeriod.Enabled && validPeriod != EstadoPeriodo.Abierto)
                {
                    if (validPeriod == EstadoPeriodo.Cerrado)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoCerrado));
                    if (validPeriod == EstadoPeriodo.EnCierre)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoEnCierre));

                    this.dtPeriod.Focus();
                }
                else
                {
                    int currentMonth = this.dtPeriod.DateTime.Month;
                    int currentYear = this.dtPeriod.DateTime.Year;
                    int minDay = 1;
                    int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                    this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtFecha_Enter(object sender, EventArgs e) { }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtFecha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }
        }

        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkVerProcesados_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                List<int> consTareas = new List<int>();
                foreach (var tarea in this._listTareasAll)
                {
                    if (!this._listActaTrabajoExist.Exists(x => x.ConsecTarea.Value == tarea.Consecutivo.Value))
                        consTareas.Add(tarea.Consecutivo.Value.Value);
                }
                foreach (var t in consTareas)
                {
                    this._listTareasAll.RemoveAll(x => x.Consecutivo.Value == t);
                }
                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ActaTrabajo.cs-chkVerProcesados_CheckedChanged"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.Proyecto);
            ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtNro.Enabled = true;
                this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDocQueryActa_Click(object sender, EventArgs e)
        {
            //List<int> docs = new List<int>();
            //docs.Add(AppDocuments.ActaTrabajo);
            //ModalQueryDocument getDocControl = new ModalQueryDocument(docs, false, false);
            //getDocControl.ShowDialog();
            //if (getDocControl.DocumentoControl != null)
            //{
            //    this.txtNroActa.Enabled = true;
            //    this.txtNroActa.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
            //    this.txtNroActa.Focus();
            //    this.btnDocQueryActa.Focus();
            //    this.btnDocQueryActa.Enabled = false;
            //}
        }

        /// <summary>
        /// Valida el prefijo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        this.txtPrefix.Text = this.prefijoID;
                    }
                    else
                        CleanHeader(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-SolicitudProy.cs", "uc_Prefijo_Leave"));
            }
        }

        /// <summary>
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNro.Text) && !string.IsNullOrEmpty(this.masterPrefijo.Value))
                {
                    int docNro = Convert.ToInt32(this.txtNro.Text);
                    DTO_glDocumentoControl docCtrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.masterPrefijo.Value, docNro);
                    if (docCtrl != null)
                    {
                        this.LoadData(this.masterPrefijo.Value, docNro, null, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ActaTrabajo.cs-txtNro_Leave"));
            }
        }

        /// <summary>
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroActa_Leave(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.txtNroActa.Text) && !string.IsNullOrEmpty(this.masterPrefijoActa.Value))
            //{
            //    int docNro = Convert.ToInt32(this.txtNroActa.Text);
            //    this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.ActaTrabajo, this.masterPrefijoActa.Value, docNro);
            //    if (this._ctrl != null)
            //    {
            //        if (this._ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
            //        {
            //            this._listActaTrabajoExist = this._bc.AdministrationModel.ActasTrabajo_Load(this._ctrl.NumeroDoc.Value);
            //            if (this._listActaTrabajoExist.Count > 0)
            //                this.LoadData(string.Empty, null, this._listActaTrabajoExist.First().NumDocProyecto.Value, string.Empty);
            //            this._numeroDocActa = this._ctrl.NumeroDoc.Value.Value;

            //        }
            //        else
            //        {
            //            MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_DocumentEstateAprob));

            //        }
            //    }
            //}
        }

        /// <summary>
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProyecto.ValidID)
                {
                    this.LoadData(string.Empty, null, null, this.masterProyecto.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ActaTrabajo.cs-masterProyecto_Leave"));
            }
        }

        #endregion

        #region Eventos Grilla

        #region Tareas

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowTareaCurrent = (DTO_pyProyectoTarea)this.gvTarea.GetRow(e.FocusedRowHandle);
                    if (this._rowTareaCurrent != null)
                    {
                        this.gcActas.DataSource = null;
                        this.gcActas.DataSource = this._listActaTrabajoExist.FindAll(x => x.ConsecTarea.Value == this._rowTareaCurrent.Consecutivo.Value).ToList();
                        this.gcActas.RefreshDataSource();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];

                this.gvTarea = (GridView)sender;
                this._rowTareaCurrent = (DTO_pyProyectoTarea)gvTarea.GetRow(e.RowHandle);
                if (fieldName == "TareaID")
                {
                  
                }                           
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }   

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                    if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                        e.Value = 0;
            }
            if (e.IsSetData)
            {               
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                //if (e.Column.FieldName == this.unboundPrefix + "CapituloDesc" && e.IsForGroupRow)
                //{
                //    DTO_pyProyectoTarea row = (DTO_pyProyectoTarea)this.gvTarea.GetRow(e.ListSourceRowIndex);
                //    e.DisplayText = row.CapituloDesc.Value;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempoProy.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        #endregion

        #region Actas

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvActasFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowActaCurrent = (DTO_pyActaTrabajoDeta)this.gvActas.GetRow(e.FocusedRowHandle);
                    this.txtObservaciones.Text = this._rowActaCurrent.Observaciones.Value;
                }
                else
                    this.txtObservaciones.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvActas_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvActas.Columns[this.unboundPrefix + fieldName];

                if (fieldName == "CantidadREC")
                {
                    decimal rec = Convert.ToDecimal(e.Value);
                    this._rowActaCurrent = (DTO_pyActaTrabajoDeta)this.gvActas.GetRow(e.RowHandle);
                    DTO_pyProyectoMvto mvto = this._listMvtos.Find(x => x.Consecutivo.Value == this._rowActaCurrent.ConsProyMvto.Value);
                    decimal existencias = mvto.CantidadPROV.Value.Value - mvto.CantidadREC.Value.Value - rec;
                    if (existencias >= 0)
                    {
                        this._rowActaCurrent.CantidadPend.Value = existencias;
                        this._rowTareaCurrent.CantidadREC.Value = this._listActaTrabajoExist.FindAll(x => x.TareaCliente.Value == this._rowTareaCurrent.TareaCliente.Value).Sum(y=>y.CantidadREC.Value);
                        this.gvActas.ClearColumnErrors();
                    }
                    else
                        this.gvActas.SetColumnError(col, "La cantidad solicitada no puede exceder a la disponible");
                }
                this.gvTarea.RefreshData();
                this.gvActas.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvRecurso_CellValueChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvActas_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                //if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                //{
                //    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                //    if (Convert.ToByte(e.Value) == 1)
                //        e.DisplayText = "MATERIALES";
                //    else if (Convert.ToByte(e.Value) == 2)
                //        e.DisplayText = "EQUIPO-HERRAMIENTA";
                //    else if (Convert.ToByte(e.Value) == 3)
                //        e.DisplayText = "MANO DE OBRA";
                //    else if (Convert.ToByte(e.Value) == 4)
                //        e.DisplayText = "TRANSPORTES";
                //    else if (Convert.ToByte(e.Value) == 5)
                //        e.DisplayText = "HERRAMIENTA";
                //    else if (Convert.ToByte(e.Value) == 6)
                //        e.DisplayText = "SOFTWARE";
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvActas_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this.gvActas.HasColumnErrors)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "gvTarea_BeforeLeaveRow"));
            }
        }

        #endregion

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {
            this.gvActas.PostEditor();
            if (this.gvActas.HasColumnErrors)
                MessageBox.Show("Valide que las cantidades solicitadas sean correctas antes de guardar");
            else 
            {
                if (this._numeroDocProy != 0)
                {
                    this.LoadDocCtrl();
                    Thread process = new Thread(this.SaveThread);
                    process.Start(); 
                }
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.gvActas.ClearColumnErrors();
            this.RefreshForm();
        }

        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
            string msgAprobar = string.Format(msgDoc, "Aprobar");

            if (MessageBox.Show(msgAprobar, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.sendToApproveInd = true;
                this.LoadDocCtrl();
                Thread process = new Thread(this.SaveThread);
                process.Start();                
            }
        }        

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
                solicitud.DocCtrl = this._ctrlProyecto;
                solicitud.HeaderProyecto = this._proyectoDocu;
                foreach (var tarea in this._listTareasAll)
                {
                    if (this._listActaTrabajoExist.Exists(x=>x.TareaCliente.Value == tarea.TareaCliente.Value && tarea.Detalle.Exists(y=>y.RecursoID.Value == x.RecursoID.Value)))
                        solicitud.DetalleProyecto.Add(tarea);
                }
                solicitud.DetalleProyecto = this._listTareasAll;
                solicitud.Movimientos = this._listMvtos;
                solicitud.ActaTrabajosDeta = this._listActaTrabajoExist;
                string reportName = this._bc.AdministrationModel.Reportes_py_ActaTrabajo(solicitud,1,this.dtFechaActa.DateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaTrabajo.cs", "TBPrint"));
            }
        }
        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                bool update = false;
                if (this._ctrl.NumeroDoc.Value != null && this._ctrl.NumeroDoc.Value != 0)
                    update = true;

                DTO_SerializedObject res = this._bc.AdministrationModel.ActaTrabajo_Add(this.documentID, this._ctrl, this._listActaTrabajoExist,this._proyecto, update);
                if (this._ctrl.NumeroDoc.Value != null)
                    this._numeroDocActa = this._ctrl.NumeroDoc.Value.Value;
                bool isOK = res.GetType() == typeof(DTO_TxResult)? false : true;// _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, res, !this.sendToApproveInd? true: false,true);
                if (isOK)
                {
                    if (this.sendToApproveInd)
                    {
                        DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                        transaccion.DocCtrl = ObjectCopier.Clone(this._ctrlProyecto);
                        transaccion.ActaTrabajosDeta = this._listActaTrabajoExist;
                        res = _bc.AdministrationModel.ActaTrabajo_ApproveRecibidoBS(this.documentID, transaccion, this._ctrl);
                        isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, res,true);
                        if (isOK)
                        {
                            this.sendToApproveInd = false;
                            this.Invoke(this.sendToApproveDelegate);
                        }
                    }
                    else
                        isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, res, true);

                }

                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudProye.cs", "SaveThread"));
            }
            finally
            {
                //if (result.Result != ResultValue.NOK)
                //    this.Invoke(this.saveDelegate);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Envia al paso de Analisis de Tiempos
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvTarea.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;
                if (this._numeroDocActa == 0)
                    result = resultNOK;
                else
                {
                    DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                    transaccion.DocCtrl = ObjectCopier.Clone(this._ctrlProyecto);
                    transaccion.ActaTrabajosDeta = this._listActaTrabajoExist;
                    result = _bc.AdministrationModel.ActaTrabajo_ApproveRecibidoBS(this.documentID, transaccion,this._ctrl);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                if (isOK)
                {
                    //_bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ActaTrabajo.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

        private void txtObservaciones_Leave(object sender, EventArgs e)
        {
            if (this._rowActaCurrent != null)
                this._rowActaCurrent.Observaciones.Value = this.txtObservaciones.Text;
        }
    }
}
