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
using System.Diagnostics;
using System.Drawing;
 
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class PlaneacionRecursos : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        private string empresaID = string.Empty;
        private int _documentID;
        private ModulesPrefix frmModule;
        private string areaFuncionalID;
        private string prefijoID;
        private bool isValid = true;
        private bool deleteOP = false;
        private PasteOpDTO pasteRet;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        //Variables para importar
        private string formatTareas = string.Empty;
        private string formatRecursos = string.Empty;
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        private int _numeroDoc = 0;
        protected bool _copyData = false;
        private string _claseServicio = string.Empty;
        private string _tareaXDef = string.Empty;
        private string _trabajoXDef = string.Empty;
        private decimal _IVAPresupuesto = 0;
        private bool _tareaIsFocused = false;
        private decimal _totalRecxTarea = 0;
        private decimal _totalPresupuesto = 0;
        private decimal _totalCliente = 0;
        private decimal _porcPrestManoObra = 0;
        private string _trabajoCurrent = string.Empty;
        private GridView gvRecursoCurrent = new GridView();
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_pyProyectoDeta _rowRecursoCurrent = new DTO_pyProyectoDeta();
        private DTO_pyProyectoMvto _rowMvtoCurrent = new DTO_pyProyectoMvto();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdicion = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoDeta> _listRecursosAll = new List<DTO_pyProyectoDeta>();
        private List<DTO_pyProyectoDeta> _listRecursosDistinct = new List<DTO_pyProyectoDeta>();
        private List<DTO_pyProyectoMvto> _listMvtosProyecto = new List<DTO_pyProyectoMvto>();

        #endregion
        
        #region Delegados

        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private void RefreshGridMethod()
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
        private void SaveMethod() { this.RefreshForm(); }

        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void SendToApproveMethod() { }

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.gcTarea.DataSource = this._listTareasAll;
            this.gcTarea.RefreshDataSource();
            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
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
        public PlaneacionRecursos()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
              
                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "PlaneacionRecursos"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Recursos
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 50;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 250;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDrec = new GridColumn();
            UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDrec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvIDrec.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvIDrec.UnboundType = UnboundColumnType.String;
            UnidadInvIDrec.VisibleIndex = 2;
            UnidadInvIDrec.Width = 40;
            UnidadInvIDrec.Visible = true;
            UnidadInvIDrec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDrec);

            GridColumn MarcaDesc = new GridColumn();
            MarcaDesc.FieldName = this.unboundPrefix + "MarcaDesc";
            MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_MarcaDesc");
            MarcaDesc.UnboundType = UnboundColumnType.String;
            MarcaDesc.VisibleIndex = 3;
            MarcaDesc.Width = 30;
            MarcaDesc.Visible = true;
            MarcaDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(MarcaDesc);

            GridColumn Modelo = new GridColumn();
            Modelo.FieldName = this.unboundPrefix + "Modelo";
            Modelo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Modelo");
            Modelo.UnboundType = UnboundColumnType.String;
            Modelo.VisibleIndex = 4;
            Modelo.Width = 30;
            Modelo.Visible = true;
            Modelo.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(Modelo);

            GridColumn CantidadStock = new GridColumn();
            CantidadStock.FieldName = this.unboundPrefix + "CantidadStock";
            CantidadStock.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PlaneacionRecursos + "_CantidadStock");
            CantidadStock.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadStock.AppearanceCell.Options.UseTextOptions = true;
            CantidadStock.UnboundType = UnboundColumnType.Decimal;
            CantidadStock.VisibleIndex = 5;
            CantidadStock.Width = 40;
            CantidadStock.Visible = false;
            CantidadStock.ColumnEdit = this.editValue2Cant;
            CantidadStock.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadStock);

            GridColumn ViewStock = new GridColumn();
            ViewStock.FieldName = this.unboundPrefix + "ViewStock";
            ViewStock.UnboundType = UnboundColumnType.Object;
            ViewStock.VisibleIndex = 6;
            ViewStock.Width = 46;
            ViewStock.Visible = true;
            ViewStock.OptionsColumn.ShowCaption = false;
            ViewStock.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(ViewStock);

            GridColumn CantidadTOT = new GridColumn();
            CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadTOT");
            CantidadTOT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadTOT.AppearanceCell.Options.UseTextOptions = true;
            CantidadTOT.UnboundType = UnboundColumnType.Decimal;       
            CantidadTOT.VisibleIndex = 7;
            CantidadTOT.Width = 70;
            CantidadTOT.Visible = true;
            CantidadTOT.ColumnEdit = this.editValue2Cant;
            CantidadTOT.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadTOT);

            GridColumn CantidadPROV = new GridColumn();
            CantidadPROV.FieldName = this.unboundPrefix + "CantidadPROV";
            CantidadPROV.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadPROV");
            CantidadPROV.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPROV.AppearanceCell.Options.UseTextOptions = true;
            CantidadPROV.UnboundType = UnboundColumnType.Decimal;
            CantidadPROV.VisibleIndex = 8;
            CantidadPROV.Width = 70;
            CantidadPROV.Visible = true;
            CantidadPROV.ColumnEdit = this.editValue2Cant;
            CantidadPROV.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadPROV);

            GridColumn CantidadRecProp = new GridColumn();
            CantidadRecProp.FieldName = this.unboundPrefix + "CantidadRecProp";
            CantidadRecProp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadRecProp");
            CantidadRecProp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadRecProp.AppearanceCell.Options.UseTextOptions = true;
            CantidadRecProp.UnboundType = UnboundColumnType.Decimal;
            CantidadRecProp.VisibleIndex = 9;
            CantidadRecProp.Width = 70;
            CantidadRecProp.Visible = true;
            CantidadRecProp.ColumnEdit = this.editValue2Cant;
            CantidadRecProp.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadRecProp);

            GridColumn CantSolicitud = new GridColumn();
            CantSolicitud.FieldName = this.unboundPrefix + "CantSolicitud";
            CantSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantSolicitud");
            CantSolicitud.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantSolicitud.AppearanceCell.Options.UseTextOptions = true;
            CantSolicitud.UnboundType = UnboundColumnType.Decimal;
            CantSolicitud.VisibleIndex = 10;
            CantSolicitud.Width = 70;
            CantSolicitud.Visible = true;
            CantSolicitud.ColumnEdit = this.editValue2Cant;
            CantSolicitud.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantSolicitud);

            GridColumn CantidadSOLRecProp = new GridColumn();
            CantidadSOLRecProp.FieldName = this.unboundPrefix + "CantidadSOLRecProp";
            CantidadSOLRecProp.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadSOLRecProp");
            CantidadSOLRecProp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadSOLRecProp.AppearanceCell.Options.UseTextOptions = true;
            CantidadSOLRecProp.UnboundType = UnboundColumnType.Decimal;
            CantidadSOLRecProp.VisibleIndex = 10;
            CantidadSOLRecProp.Width = 70;
            CantidadSOLRecProp.Visible = false;
            CantidadSOLRecProp.ColumnEdit = this.editValue2Cant;
            CantidadSOLRecProp.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadSOLRecProp);

            GridColumn TipoRecurso = new GridColumn();
            TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecurso.UnboundType = UnboundColumnType.Integer;
            TipoRecurso.Width = 80;
            TipoRecurso.Visible = false;
            TipoRecurso.Group();
            TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecurso.Columns.Add(TipoRecurso);

            this.gvRecurso.OptionsBehavior.Editable = true;
            this.gvRecurso.OptionsView.ColumnAutoWidth = true;
            #endregion            

            #region Grilla Mvtos

            GridColumn tareaCliente = new GridColumn();
            tareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            tareaCliente.UnboundType = UnboundColumnType.String;
            tareaCliente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tareaCliente.AppearanceCell.Options.UseTextOptions = true;
            tareaCliente.AppearanceCell.Options.UseFont = true;
            tareaCliente.VisibleIndex = 1;
            tareaCliente.Width = 30;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = false;
            tareaCliente.AppearanceCell.BackColor = Color.LightSteelBlue;
            tareaCliente.AppearanceCell.Options.UseBackColor = true;
            this.gvTarea.Columns.Add(tareaCliente);

            GridColumn tareaID = new GridColumn();
            tareaID.FieldName = this.unboundPrefix + "TareaID";
            tareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            tareaID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaID.AppearanceCell.Options.UseTextOptions = true;
            tareaID.AppearanceCell.Options.UseFont = true;
            tareaID.UnboundType = UnboundColumnType.String;
            tareaID.VisibleIndex = 2;
            tareaID.Width = 38;
            tareaID.Visible = true;
            tareaID.AppearanceCell.BackColor = Color.LightSteelBlue;
            tareaID.AppearanceCell.Options.UseBackColor = true;
            tareaID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(tareaID);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this.unboundPrefix + "TareaDesc";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 3;
            TareaDesc.Width = 180;
            TareaDesc.Visible = true;
            TareaDesc.AppearanceCell.BackColor = Color.LightSteelBlue;
            TareaDesc.AppearanceCell.Options.UseBackColor = true;
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaDesc);

            GridColumn Version = new GridColumn();
            Version.FieldName = this.unboundPrefix + "Version";
            Version.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Version");
            Version.UnboundType = UnboundColumnType.String;
            Version.VisibleIndex = 4;
            Version.Width = 25;
            Version.Visible = true;
            Version.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(Version);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 5;
            UnidadInvID.Width = 30;
            UnidadInvID.Visible = false;
            UnidadInvID.AppearanceCell.BackColor = Color.LightSteelBlue;
            UnidadInvID.AppearanceCell.Options.UseBackColor = true;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(UnidadInvID);

            GridColumn CantidadPresup = new GridColumn();
            CantidadPresup.FieldName = this.unboundPrefix + "CantidadPresup";
            CantidadPresup.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Proyecto + "_CantidadPresup");
            CantidadPresup.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadPresup.UnboundType = UnboundColumnType.Decimal;
            CantidadPresup.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPresup.AppearanceCell.ForeColor = Color.Gray;
            CantidadPresup.AppearanceCell.Options.UseForeColor = true;
            CantidadPresup.AppearanceCell.Options.UseTextOptions = true;
            CantidadPresup.AppearanceCell.Options.UseFont = true;
            CantidadPresup.VisibleIndex = 6;
            CantidadPresup.Width = 30;
            CantidadPresup.Visible = true;
            CantidadPresup.ColumnEdit = this.editValue2Cant;
            CantidadPresup.OptionsColumn.AllowEdit = false;
            CantidadPresup.ToolTip = "Cantidad solicitada en Pre-Proyecto";
            this.gvTarea.Columns.Add(CantidadPresup);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "CantidadTarea";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadTarea");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 7;
            Cantidad.Width = 45;
            Cantidad.Visible = false;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.AppearanceCell.BackColor = Color.LightSteelBlue;
            Cantidad.AppearanceCell.Options.UseBackColor = true;
            Cantidad.OptionsColumn.AllowEdit = true;
            this.gvTarea.Columns.Add(Cantidad);
          
            GridColumn CantidadTOTTarea = new GridColumn();
            CantidadTOTTarea.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOTTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadTOT");
            CantidadTOTTarea.UnboundType = UnboundColumnType.String;
            CantidadTOTTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadTOTTarea.AppearanceCell.Options.UseTextOptions = true;
            CantidadTOTTarea.VisibleIndex = 8;
            CantidadTOTTarea.Width = 50;
            CantidadTOTTarea.Visible = true;
            CantidadTOTTarea.ColumnEdit = this.editValue2Cant;
            CantidadTOTTarea.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(CantidadTOTTarea);

            GridColumn CantidadPROVDet = new GridColumn();
            CantidadPROVDet.FieldName = this.unboundPrefix + "CantidadPROV";
            CantidadPROVDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadPROV");
            CantidadPROVDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPROVDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadPROVDet.UnboundType = UnboundColumnType.Decimal;
            CantidadPROVDet.VisibleIndex = 9;
            CantidadPROVDet.Width = 52;
            CantidadPROVDet.Visible = true;
            CantidadPROVDet.ColumnEdit = this.editValue2Cant;
            CantidadPROVDet.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(CantidadPROVDet);

            GridColumn CantidadRecPropDet = new GridColumn();
            CantidadRecPropDet.FieldName = this.unboundPrefix + "CantidadRecProp";
            CantidadRecPropDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadRecProp");
            CantidadRecPropDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadRecPropDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadRecPropDet.UnboundType = UnboundColumnType.Decimal;
            CantidadRecPropDet.VisibleIndex = 10;
            CantidadRecPropDet.Width = 54;
            CantidadRecPropDet.Visible = true;
            CantidadRecPropDet.ColumnEdit = this.editValue2Cant;
            CantidadRecPropDet.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(CantidadRecPropDet);

            GridColumn CantidadSol = new GridColumn();
            CantidadSol.FieldName = this.unboundPrefix + "CantidadSOL";
            CantidadSol.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadSOL");
            CantidadSol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadSol.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadSol.AppearanceCell.Options.UseTextOptions = true;
            CantidadSol.AppearanceCell.Options.UseFont = true;
            CantidadSol.AppearanceCell.BackColor = Color.PeachPuff;
            CantidadSol.AppearanceCell.Options.UseBackColor = true;
            CantidadSol.UnboundType = UnboundColumnType.String;
            CantidadSol.VisibleIndex = 11;
            CantidadSol.Width = 50;
            CantidadSol.Visible = true;
            CantidadSol.ColumnEdit = this.editValue2Cant;
            CantidadSol.OptionsColumn.AllowEdit = true;
            this.gvTarea.Columns.Add(CantidadSol);

            GridColumn CantidadSOLRecPropDet = new GridColumn();
            CantidadSOLRecPropDet.FieldName = this.unboundPrefix + "CantidadSOLRecProp";
            CantidadSOLRecPropDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadSOLRecProp");
            CantidadSOLRecPropDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadSOLRecPropDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadSOLRecPropDet.UnboundType = UnboundColumnType.Decimal;
            CantidadSOLRecPropDet.VisibleIndex = 12;
            CantidadSOLRecPropDet.Width = 60;
            CantidadSOLRecPropDet.Visible = false;
            CantidadSOLRecPropDet.ColumnEdit = this.editValue2Cant;
            CantidadSOLRecPropDet.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(CantidadSOLRecPropDet);

            GridColumn FechaInicioTarea = new GridColumn();
            FechaInicioTarea.FieldName = this.unboundPrefix + "FechaInicioTarea";
            FechaInicioTarea.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaInicio");
            FechaInicioTarea.UnboundType = UnboundColumnType.DateTime;
            FechaInicioTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaInicioTarea.AppearanceCell.Options.UseTextOptions = true;
            FechaInicioTarea.VisibleIndex = 13;
            FechaInicioTarea.Width = 50;
            FechaInicioTarea.Visible = true;
            FechaInicioTarea.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(FechaInicioTarea);

            GridColumn FechaOrdCompra = new GridColumn();
            FechaOrdCompra.FieldName = this.unboundPrefix + "FechaOrdCompra";
            FechaOrdCompra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaOrdCompra");
            FechaOrdCompra.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            FechaOrdCompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaOrdCompra.AppearanceCell.Options.UseTextOptions = true;
            FechaOrdCompra.AppearanceCell.Options.UseFont = true;
            FechaOrdCompra.AppearanceCell.BackColor = Color.PeachPuff;
            FechaOrdCompra.AppearanceCell.Options.UseBackColor = true;
            FechaOrdCompra.UnboundType = UnboundColumnType.DateTime;
            FechaOrdCompra.VisibleIndex = 14;
            FechaOrdCompra.Width = 58;
            FechaOrdCompra.Visible = true;         
            FechaOrdCompra.OptionsColumn.AllowEdit = true;
            this.gvTarea.Columns.Add(FechaOrdCompra);

            GridColumn Nivel = new GridColumn();
            Nivel.FieldName = this.unboundPrefix + "Nivel";
            Nivel.UnboundType = UnboundColumnType.Integer;
            Nivel.Visible = false;
            Nivel.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(Nivel);

            GridColumn TareaPadre = new GridColumn();
            TareaPadre.FieldName = this.unboundPrefix + "TareaPadre";
            TareaPadre.UnboundType = UnboundColumnType.String;
            TareaPadre.Visible = false;
            TareaPadre.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaPadre);

            GridColumn DetalleInd = new GridColumn();
            DetalleInd.FieldName = this.unboundPrefix + "DetalleInd";
            DetalleInd.UnboundType = UnboundColumnType.Boolean;
            DetalleInd.Visible = false;
            DetalleInd.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(DetalleInd);

            this.gvTarea.OptionsBehavior.Editable = true;
            this.gvTarea.OptionsView.ColumnAutoWidth = true;
            #endregion
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void CalculateValues(DTO_pyProyectoTarea tarea)
        {
            try
            {
                #region Calcula Costos y Cantidades
                foreach (DTO_pyProyectoDeta d in tarea.Detalle)
                {
                    decimal rend = (d.Cantidad.Value.Value * d.FactorID.Value.Value); //Rendimiento
                    d.CantidadTOT.Value = 1;//Cantidad total del recurso
                    if(d.TipoRecurso.Value == (byte)TipoRecurso.Equipo || d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                        d.CostoLocalTOT.Value =d.CantidadTOT.Value * d.CostoLocal.Value / (rend != 0 ? rend : 1); //Valor Total por recurso
                    else
                        d.CostoLocalTOT.Value = d.CantidadTOT.Value * d.CostoLocal.Value * (rend != 0 ? rend : 1); //Valor Total por recurso
                } 
                #endregion
                #region Asigna valores
                decimal costoTotalUnitTarea = tarea.Detalle.Sum(x => x.CostoLocalTOT.Value.Value);
                
                tarea.CostoTotalUnitML.Value = costoTotalUnitTarea != 0 ? Math.Round(costoTotalUnitTarea, 0) : tarea.CostoTotalUnitML.Value;
                tarea.CostoTotalML.Value = tarea.CostoTotalUnitML.Value != 0 ? Math.Round(tarea.CostoTotalUnitML.Value.Value * tarea.Cantidad.Value.Value, 0) : tarea.CostoTotalML.Value;
                tarea.CostoDiferenciaML.Value = tarea.CostoLocalCLI.Value - tarea.CostoTotalML.Value;
                this._totalRecxTarea = tarea.CostoTotalML.Value.Value;

                if (tarea.Nivel.Value > 1)
                {
                    //Calcula totales por cada Nivel
                    var rowPadre1 = this._listTareasAll.Find(x=>x.TareaCliente.Value == tarea.TareaPadre.Value);
                    if (rowPadre1 != null)
                    {
                        rowPadre1.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                        rowPadre1.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                        if (rowPadre1.Nivel.Value > 1)
                        {
                            var rowPadre2 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre1.TareaPadre.Value);
                            rowPadre2.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre1.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                            rowPadre2.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre1.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                            if (rowPadre2.Nivel.Value > 1)
                            {
                                var rowPadre3 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre2.TareaPadre.Value);
                                rowPadre3.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre2.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                rowPadre3.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre2.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                if (rowPadre3.Nivel.Value > 1)
                                {
                                    var rowPadre4 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre3.TareaPadre.Value);
                                    rowPadre4.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre3.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                    rowPadre4.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre3.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                    if (rowPadre4.Nivel.Value > 1)
                                    {
                                        var rowPadre5 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre4.TareaPadre.Value);
                                        rowPadre5.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre4.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                        rowPadre5.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre4.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                        if (rowPadre5.Nivel.Value > 1)
                                        {
                                            var rowPadre6 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre5.TareaPadre.Value);
                                            rowPadre6.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre5.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                            rowPadre6.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre5.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                        }
                                    }
                                }
                            }
                        } 
                    }
                }                
                #endregion        
                #region Actualiza Datos
                this.gvRecursoCurrent.RefreshData();    
                this.gvTarea.RefreshData();
                this.gvRecurso.RefreshData();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "CalculateValues"));
            }
        }

        /// <summary>
        /// Carga la información del documento
        /// </summary>
        private void CargarInformacion(bool exist)
        {
            try
            {
                if (!exist)
                {
                    this._proyectoDocu.ClienteID.Value = this.masterCliente.Value;
                    this._proyectoDocu.DescripcionSOL.Value = this.txtDescripcion.Text;
                    this._proyectoDocu.EmpresaNombre.Value = this.txtSolicitante.Text;
                    this._proyectoDocu.ResponsableEMP.Value = this.masterResponsableEmp.Value;
                    this._proyectoDocu.RecursosXTrabajoInd.Value = this.chkRecursoXTrabInd.Checked;
                    this._proyectoDocu.TipoSolicitud.Value = Convert.ToByte(this.cmbTipoSolicitud.EditValue);
                    this._proyectoDocu.PropositoProyecto.Value = Convert.ToByte(this.cmbProposito.EditValue);
                    this._proyectoDocu.Licitacion.Value = this.txtLicitacion.Text;
                }
                else
                {
                    this.masterClaseServicio.Value = this._proyectoDocu.ClaseServicioID.Value;
                    this.masterCliente.Value = this._proyectoDocu.ClienteID.Value;
                    this.txtDescripcion.Text = this._proyectoDocu.DescripcionSOL.Value;
                    this.txtSolicitante.Text = this._proyectoDocu.EmpresaNombre.Value;
                    this.dtFechaInicio.DateTime =  this._proyectoDocu.FechaInicio.Value.HasValue?  this._proyectoDocu.FechaInicio.Value.Value : DateTime.Today;
                    this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
                    this.masterCentroCto.Value = this._ctrl.CentroCostoID.Value;
                    this.masterResponsableEmp.Value = this._proyectoDocu.ResponsableEMP.Value;
                    this.cmbTipoSolicitud.EditValue = this._proyectoDocu.TipoSolicitud.Value.Value;
                    this.cmbProposito.EditValue = this._proyectoDocu.PropositoProyecto.Value.Value;
                    this.txtObservaciones.Text = this._ctrl.Observacion.Value;
                    this.chkRecursoXTrabInd.Checked = this._proyectoDocu.RecursosXTrabajoInd.Value.Value;
                    this.txtLicitacion.Text = this._proyectoDocu.Licitacion.Value;
                    this.chkRecursoXTrabInd.Checked = this._proyectoDocu.RecursosXTrabajoInd.Value.Value;
                    this.masterAreaFuncional.Value = this._ctrl.AreaFuncionalID.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "CargarInformacion"));
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
                    this.masterCliente.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterResponsableEmp.Value = string.Empty;
                    this.txtDescripcion.Text = string.Empty;
                    this.txtSolicitante.Text = string.Empty;
                    this.txtObservaciones.Text = string.Empty;
                    this.cmbTipoSolicitud.EditValue = ((int)TipoSolicitud.Cotizacion);
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "CleanHeader"));
            }
        }

        /// <summary>
        /// Habilita o desabuilita el header
        /// </summary>
        /// <param name="p"></param>
        private void EnableHeader(bool p)
        {
            this.masterClaseServicio.EnableControl(p);
            this.masterPrefijo.EnableControl(p);
            this.masterAreaFuncional.EnableControl(p);
            this.masterCliente.EnableControl(p);
            this.masterProyecto.EnableControl(p);
            this.masterResponsableEmp.EnableControl(p);
            this.txtNro.Enabled = p;
            this.btnQueryDoc.Enabled = p;
            this.cmbTipoSolicitud.Enabled = p;
            this.cmbProposito.Enabled = p;
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, false, true, true);
                this._bc.InitMasterUC(this.masterAreaFuncional, AppMasters.glAreaFuncional, true, true, true, false);
                this._bc.InitMasterUC(this.masterClaseServicio, AppMasters.pyClaseProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterResponsableEmp, AppMasters.seUsuario, false, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
                this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, false);

                #region Combos

                Dictionary<string, string> datosTipoSolicitud = new Dictionary<string, string>();
                datosTipoSolicitud.Add("1", "Implementación");
                datosTipoSolicitud.Add("2", "Desarrollo");
                datosTipoSolicitud.Add("3", "Investigación");
                datosTipoSolicitud.Add("4", "Obra");
                datosTipoSolicitud.Add("5", "Interventoria");
                this.cmbTipoSolicitud.Properties.ValueMember = "Key";
                this.cmbTipoSolicitud.Properties.DisplayMember = "Value";
                this.cmbTipoSolicitud.Properties.DataSource = datosTipoSolicitud;
                this.cmbTipoSolicitud.EditValue = "1";

                Dictionary<string, string> datosProposito = new Dictionary<string, string>();
                datosProposito.Add(((int)TipoSolicitud.Cotizacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cotizacion));
                datosProposito.Add(((int)TipoSolicitud.Licitacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Licitacion));
                datosProposito.Add(((int)TipoSolicitud.Interna).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Interna));
                this.cmbProposito.Properties.ValueMember = "Key";
                this.cmbProposito.Properties.DisplayMember = "Value";
                this.cmbProposito.Properties.DataSource = datosProposito;
                this.cmbProposito.EditValue = ((int)TipoSolicitud.Cotizacion).ToString();

                Dictionary<string, string> datosReporte = new Dictionary<string, string>();
                datosReporte.Add("1", "Presupuesto");
                datosReporte.Add("2", "Presupuesto Cliente");
                datosReporte.Add("3", "Presupuesto Comparativo");
                datosReporte.Add("4", "A.P.U Detallado");
                this.cmbReporte.Properties.ValueMember = "Key";
                this.cmbReporte.Properties.DisplayMember = "Value";
                this.cmbReporte.Properties.DataSource = datosReporte;
                this.cmbReporte.EditValue = "1";

                #endregion

                this.formatRecursos = _bc.GetImportExportFormat(typeof(DTO_ExportRecursosDeta), AppForms.MasterReportXls);
                this.formatTareas = _bc.GetImportExportFormat(typeof(DTO_pyProyectoTarea), AppDocuments.PreProyecto);
                this._tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);               
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
                string proyectoID = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string centroCto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string IVAPresup = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcIVAUtilidad);
                string prestacionMO = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcPrestacionManoObra);
                
                if (!string.IsNullOrEmpty(IVAPresup))
                    this._IVAPresupuesto = Convert.ToDecimal(IVAPresup, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(prestacionMO))
                    this._porcPrestManoObra = Convert.ToDecimal(prestacionMO, CultureInfo.InvariantCulture);
                else
                    this._porcPrestManoObra = 100;

                this.masterResponsableEmp.Value = (this._bc.AdministrationModel.User).ID.Value;
                this.masterProyecto.Value = proyectoID;
                this.masterCentroCto.Value = centroCto;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "InitControls"));
            }
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

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this._documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this._documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                        this.txtPrefix.Text = this.prefijoID;

                    this.txtDocumentoID.Text = this._documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.py_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    this._listRecursosDistinct = this._listRecursosDistinct.OrderBy(x => x.RecursoDesc.Value).ToList();
                    this.gcRecurso.DataSource = this._listRecursosDistinct;
                    this.gcRecurso.RefreshDataSource();
                    this.gcTarea.DataSource = null;
                    this.gcTarea.RefreshDataSource();
                }
                if (this.gvRecurso.DataRowCount > 0)
                    this.gvRecurso.FocusedRowHandle = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "LoadGrids"));
            }
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                this._listRecursosDistinct = new List<DTO_pyProyectoDeta>(); 
                //Consolida todos los movimientos
                List<DTO_pyProyectoDeta> detalle = new List<DTO_pyProyectoDeta>();
                foreach (var tarea in this._listTareasAll)                   
                    detalle.AddRange(tarea.Detalle.ToList());
                //Ordena la lista
                detalle = detalle.OrderBy(x => x.RecursoDesc.Value).ToList();

                //Hace un distinct de los recursos
                List<string> recursosDistinct = detalle.Select(x => x.RecursoID.Value.ToString()).Distinct().ToList();
                foreach (string rec in recursosDistinct)
                {
                    DTO_pyProyectoDeta det = detalle.Find(x => x.RecursoID.Value == rec);
                    det.DetalleMvto = this._listMvtosProyecto.FindAll(x => x.RecursoID.Value == rec).ToList();
                    foreach (DTO_pyProyectoMvto mvto in det.DetalleMvto)
                    {
                        DTO_pyProyectoTarea tarea = this._listTareasAll.Find(x => x.TareaCliente.Value == mvto.TareaCliente.Value && x.TareaID.Value == mvto.TareaID.Value);
                        mvto.UnidadInvID.Value = tarea.UnidadInvID.Value;
                        mvto.FechaInicioTarea.Value = tarea.FechaInicio.Value;
                        mvto.CantidadPresup.Value = tarea.CantidadPresup.Value;
                        mvto.FactorID.Value = mvto.FactorID.Value;
                        mvto.CostoTotalML.Value = mvto.CantidadTarea.Value * mvto.CostoLocalTOT.Value;                       
                        mvto.CantidadRecProp.Value = mvto.CantidadNOM.Value + mvto.CantidadACT.Value + mvto.CantidadINV.Value;                        
                        mvto.CantidadSOLRecProp.Value = mvto.CantidadTOT.Value.Value - mvto.CantidadPROV.Value - mvto.CantidadRecProp.Value - mvto.CantidadSOL.Value;
                    }
                    det.CantidadTOT.Value = det.DetalleMvto.Sum(x => x.CantidadTOT.Value);
                    det.CantidadPROV.Value = det.DetalleMvto.Sum(x => x.CantidadPROV.Value);
                    det.CantidadRecProp.Value = det.DetalleMvto.Sum(x => x.CantidadRecProp.Value);
                    det.CantSolicitud.Value = det.DetalleMvto.Sum(x => x.CantidadSOL.Value);
                    det.CantidadSOLRecProp.Value = det.DetalleMvto.Sum(x => x.CantidadSOLRecProp.Value);
                    det.CostoLocalTOT.Value = det.DetalleMvto.Sum(x => x.CostoTotalML.Value);
                 
                    if (det.TipoRecurso.Value != (byte)TipoRecurso.Personal && det.TipoRecurso.Value != (byte)TipoRecurso.Transporte)
                    {
                        #region Carga el stock del recurso
                        DTO_inControlSaldosCostos filterInv = new DTO_inControlSaldosCostos();
                        filterInv.inReferenciaID.Value = rec;
                        //List<DTO_inControlSaldosCostos> mvtosxReferencia = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, filterInv);
                        //Suma el saldo de las bodegas Tipo Stock
                        det.CantidadStock.Value = 0;// mvtosxReferencia.FindAll(x => x.BodegaTipo.Value == (byte)TipoBodega.Stock).Sum(y => y.CantidadDisp.Value);
                        #endregion
                    }                   
                    
                    this._listRecursosDistinct.Add(det);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "LoadGrids"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                if (col == "TareaID")
                {
                    props.DocumentoID = AppMasters.pyTarea;
                    props.DTOTipo = "DTO_pyTarea";
                    props.ModuloID = ModulesPrefix.py;
                    props.NombreTabla = "pyTarea";
                }
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {
            this.masterPrefijo.Value = string.Empty;
            this.masterAreaFuncional.Value = string.Empty;
            this.masterClaseServicio.Value = string.Empty;
            this.txtNro.Text = string.Empty;
            this.CleanHeader(true);
            this.EnableHeader(true);

            this._ctrl = null;
            this._numeroDoc = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu();                  
            this._rowRecursoCurrent = new DTO_pyProyectoDeta();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasAdicion = new List<DTO_pyProyectoTarea>();
            this._listRecursosAll = new List<DTO_pyProyectoDeta>();
            this._rowMvtoCurrent = new DTO_pyProyectoMvto();
            this._listRecursosDistinct = new List<DTO_pyProyectoDeta>();
            this._listMvtosProyecto = new List<DTO_pyProyectoMvto>();

            this._claseServicio = string.Empty;
            this._tareaIsFocused = false;
            this._totalRecxTarea = 0;
            this._totalPresupuesto = 0;
            this._totalCliente = 0;
            this._trabajoCurrent = string.Empty;
            this.gvRecursoCurrent = new GridView();
            this.gcTarea.DataSource = this._listTareasAll;
            this.gcTarea.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr); 
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add); 
            FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Import); 

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private  void SetInitParameters()
        {
            try
            {
                InitializeComponent();
                this.frmModule = ModulesPrefix.py;
                this._documentID = AppDocuments.PlaneacionRecursos;
                this.AddGridCols();
                this.InitControls();
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);

                this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
                this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

                this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
                this.saveDelegate = new Save(this.SaveMethod);
                this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);               

            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0)
                {                    
                    #region UnidadInvID
                    //validField = this._bc.ValidGridCell(this.gvTarea, this.unboundPrefix, fila, "UnidadInvID", !this._rowTareaCurrent.DetalleInd.Value.Value, true, false, AppMasters.inUnidad);
                    //if (!validField)
                    //    validRow = false;

                    #endregion                    
                    #region Descriptivo
                    validField = this._bc.ValidGridCell(this.gvTarea, this.unboundPrefix, fila, "Descriptivo", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Cantidad
                    //validField = this._bc.ValidGridCell(this.gvTarea, this.unboundPrefix, fila, "Cantidad", !this._rowTareaCurrent.DetalleInd.Value.Value, false, false, null);
                    ////validField = _bc.ValidGridCellValue(this.gvDocument, this.unboundPrefix, fila, "Cantidad", !this._rowTareaCurrent.DetalleInd.Value.Value, false, false, false);
                    //if (!validField)
                    //    validRow = false;
                    #endregion

                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasProyecto.cs", "ValidateRow"));
            }
            return validRow;
        }

        /// <summary>
        /// Valida el Header
        /// </summary>
        /// <returns></returns>
        private string ValidateHeader()
        {
            string camposObligatorios = string.Empty;

            if (string.IsNullOrEmpty(this.masterAreaFuncional.Value))
                camposObligatorios = camposObligatorios + this.masterAreaFuncional.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.masterClaseServicio.Value))
                camposObligatorios = camposObligatorios + this.masterClaseServicio.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.masterCentroCto.Value))
                camposObligatorios = camposObligatorios + this.masterCentroCto.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.txtSolicitante.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblSolicitante.Text) + "\n";
            if (string.IsNullOrEmpty(this.masterResponsableEmp.Value))
                camposObligatorios = camposObligatorios + this.masterResponsableEmp.LabelRsx + "\n";
            return camposObligatorios;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dtoInsumo">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_pyProyectoTarea dtoTarea, DTO_ExportRecursosDeta dtoRecurso, DTO_TxResultDetail rd, string msgFkNotFound, string msgEmptyField)
        {
            try
            {
                if (dtoTarea != null)
                {      
                    #region Validacion Tarea
                    //if (string.IsNullOrWhiteSpace(dtoTarea.TareaCliente.Value.ToString()))
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.SolicitudProyecto + "_TareaCliente");
                    //    rdF.Message = msgEmptyField;
                    //    rd.DetailsFields.Add(rdF);
                    //}
                    #endregion
                    #region Validacion Descripcion
                    if (string.IsNullOrWhiteSpace(dtoTarea.Descriptivo.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaDesc");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                    
                }
                else if (dtoRecurso != null)
                {
                    #region Validacion TareaCliente
                    if (string.IsNullOrWhiteSpace(dtoRecurso.TareaCliente.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    //if (this._tareaIsFocused && dtoRecurso.TareaCliente.Value != this._rowTareaCurrent.TareaCliente.Value)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.SolicitudProyecto + "_TareaID"); ;
                    //    rdF.Message = this._bc.GetResource(LanguageTypes.Forms, DictionaryMessages.Py_TareaInvalid);
                    //    rd.DetailsFields.Add(rdF);
                    //}
                    #endregion                   
                    #region Validacion Recurso
                    if (string.IsNullOrWhiteSpace(dtoRecurso.RecursoID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Unidad
                    //if (string.IsNullOrWhiteSpace(dtoRecurso.UnidadInvID.Value.ToString()))
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.SolicitudProyecto + "_UnidadInvID");
                    //    rdF.Message = msgEmptyField;
                    //    rd.DetailsFields.Add(rdF);
                    //}
                    #endregion
                    #region Validacion FactorID
                    if (string.IsNullOrWhiteSpace(dtoRecurso.FactorID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// Actualiza los costos generales
        /// </summary>
        private void UpdateValues()
        {
            try
            {
                this._totalPresupuesto = 0;
                this._totalCliente = 0;
                this._totalRecxTarea = 0;
                foreach (DTO_pyProyectoTarea tarea in this._listTareasAll.FindAll(x=>x.DetalleInd.Value.Value))
                {
                    this._totalPresupuesto += tarea.CostoTotalML.Value.Value;
                    this._totalCliente += tarea.CostoLocalCLI.Value.Value;
                }
        }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this.frmModule);

                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {   
                    //FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);                
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Print);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "dtPeriod_EditValueChanged"));
            }
        }

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

        #endregion

        #region Eventos Header

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.Proyecto);
                ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, true);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtNro.Enabled = true;
                    this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtNro.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos", "btnQueryDoc_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterAreaFuncional_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.masterAreaFuncional.Value))
            {
                string prefijo = this._bc.GetPrefijo(this.masterAreaFuncional.Value, this._documentID);
                this.masterPrefijo.Value = prefijo;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, System.EventArgs e)
        {
            if (this.masterCliente.ValidID)
            {
                DTO_faCliente cliente = (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                if (cliente != null)
                    this._proyectoDocu.ClienteDesc.Value = cliente.Descriptivo.Value;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterClaseServicio_Leave(object sender, System.EventArgs e)
        {
            try
            {
                // Carga los Recursos para la Clase Servicio Seleccionada
                if (this.masterClaseServicio.ValidID && this.masterClaseServicio.Value != this._claseServicio)
                {
                    //this._claseServicio = this.masterClaseServicio.Value;
                    //this._listRecursosXTareaAll = this._bc.AdministrationModel.RecursosTrabajo_GetByTarea(this.documentID, string.Empty, string.Empty, null, false);
                }
                else
                {
                    //this._listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
                    this._claseServicio = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionRecursos.cs-masterClaseServicio_Leave"));
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
                int? docNro = null;
                if (!string.IsNullOrEmpty(this.txtNro.Text)) docNro = Convert.ToInt32(this.txtNro.Text);

                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(this._documentID, this.masterPrefijo.Value, docNro, null, string.Empty,string.Empty, false,true,false,false);
                if (transaccion != null)
                {
                    this._ctrl = transaccion.DocCtrl;
                    if (this._ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }

                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto;
                    this.CargarInformacion(true);
                    //foreach (var tarea in _listTareasAll)
                    //    this.CalculateValues(tarea);

                    this._listTareasAdicion = transaccion.DetalleProyectoTareaAdic;
                    this._listMvtosProyecto = transaccion.Movimientos;
                    foreach (DTO_pyProyectoMvto mvto in this._listMvtosProyecto)
                    {
                        if (mvto.FechaOrdCompra.Value != null && mvto.FechaOrdCompra.Value.Value < DateTime.Now.Date)
                            mvto.FechaOrdCompra.Value = DateTime.Now.Date;
                    }
                    this._numeroDoc = this._ctrl.NumeroDoc.Value != null ? this._ctrl.NumeroDoc.Value.Value : 0;
                    this.LoadData();
                    this.LoadGrids(true);                           
                    this.EnableHeader(this._numeroDoc != 0 ? false : true);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error,"No existe"));
                    this._ctrl = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionRecursos.cs-txtNro_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRecursoXTrabInd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkRecursoXTrabInd.Checked)
                {
                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].VisibleIndex = 0;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoTotalML"].VisibleIndex = 5;

                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoTotalML"].Visible = true;

                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocal"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocalTOT"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TipoRecurso"].UnGroup();
                   
                    this.gcRecurso.DataSource = null;
                }
                else
                {
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 0;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocal"].VisibleIndex = 6;  

                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocal"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocalTOT"].Visible = true;

                    this.gvRecurso.Columns[this.unboundPrefix + "CostoTotalML"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TipoRecurso"].Group();
                    this.gcRecurso.DataSource = null;
                }
            }
            catch (Exception ex)
            {               
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "chkRecursoXTrabInd_CheckedChanged"));
            }
        }

        /// <summary>
        /// Al poner el mouse sobre el control
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void txt_MouseHover(object sender, EventArgs e)
        {
            try
            {
                MemoExEdit memo = (MemoExEdit)sender;
                switch (memo.Name)
                {
                    case "txtLicitacion":
                        this.txtLicitacion.ToolTip = memo.Text;
                        break;
                    case "txtDescripcion":
                        this.txtDescripcion.ToolTip = memo.Text;
                        break;
                    case "txtObservaciones":
                        this.txtObservaciones.ToolTip = memo.Text;
                        break;
                    case "txtSolicitante":
                        this.txtSolicitante.ToolTip = memo.Text;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "txt_MouseHover: " + ex.Message));
            }
        }

        /// <summary>
        /// Al poner el mouse sobre el control
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var rec in this._listRecursosDistinct)
                {
                    foreach (var mvto in rec.DetalleMvto)
                    {
                        if(this.chkSelectAll.Checked)
                        {
                            mvto.CantidadSOL.Value = mvto.CantidadTOT.Value - mvto.CantidadPROV.Value;
                            mvto.CantidadSOLRecProp.Value = mvto.CantidadTOT.Value - mvto.CantidadSOL.Value - mvto.CantidadPROV.Value;// mvto.CantidadNOM.Value - mvto.CantidadACT.Value - mvto.CantidadINV.Value;
                        }
                        else
                        {
                            mvto.CantidadSOL.Value = 0;
                            mvto.CantidadSOLRecProp.Value = mvto.CantidadTOT.Value - mvto.CantidadSOL.Value - -mvto.CantidadPROV.Value;//mvto.CantidadNOM.Value - mvto.CantidadACT.Value - mvto.CantidadINV.Value
                        }
                    }
                    rec.CantSolicitud.Value = rec.DetalleMvto.Sum(x => x.CantidadSOL.Value);
                    rec.CantidadSOLRecProp.Value = rec.CantidadTOT.Value - rec.CantSolicitud.Value;
                }
                this.gvTarea.RefreshData();
                this.gvRecurso.RefreshData();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "chkSelectAll_CheckedChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Boton para importar datos 
        /// </summary>
        private void btnExportRecPropios_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._listRecursosDistinct.Count > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    List<DTO_MigracionMvtos> tmp = new List<DTO_MigracionMvtos>();
                    foreach (DTO_pyProyectoDeta rec in this._listRecursosDistinct)
                    {
                        if (rec.CantidadStock.Value >= rec.DetalleMvto.Sum(x=>x.CantidadSOLRecProp.Value) &&  rec.DetalleMvto.Sum(x=>x.CantidadSOLRecProp.Value) > 0)
                        {
                            DTO_MigracionMvtos ex = new DTO_MigracionMvtos();
                            ex.inReferenciaID.Value = rec.RecursoID.Value;
                            ex.DocSoporte.Value = rec.NumeroDoc.Value;
                            ex.CantidadEMP.Value = rec.DetalleMvto.Sum(x => x.CantidadSOLRecProp.Value);
                            ex.EmpaqueInvID.Value = rec.UnidadInvID.Value;
                            ex.ValorUNI.Value = 0;
                            tmp.Add(ex); 
                        }
                    }
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_MigracionMvtos), tmp);
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "btnExportRecPropios_Click"));
            }
        }


        #endregion

        #region Eventos Grilla

        #region Recurso

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvRecurso.Columns[this.unboundPrefix + fieldName];

                //if (fieldName == "RecursoID")
                //{
                //    DTO_pyRecurso rec = (DTO_pyRecurso)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, false, e.Value.ToString(), true);
                //    if (rec != null)
                //    {
                //        this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "RecursoDesc", rec.Descriptivo.Value);
                //        this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "UnidadInvID", rec.UnidadInvID.Value);
                //    }
                //    this.gvRecurso.RefreshData();
                //}
                //else if (fieldName == "CantSolicitud")
                //{
                //    this.CalculateValues(this._rowTareaCurrent);
                //    this.UpdateValues();
                //}



            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvRecurso_CellValueChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowRecursoCurrent = (DTO_pyProyectoDeta)this.gvRecurso.GetRow(e.FocusedRowHandle);
                    this.gcTarea.DataSource = null;
                    this.gcTarea.DataSource = this._rowRecursoCurrent.DetalleMvto;
                    this.gcTarea.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this.gvTarea.HasColumnErrors)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvTarea_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
                    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "MATERIALES";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "EQUIPO-HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = "MANO DE OBRA";
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = "TRANSPORTES";
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = "HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = "SOFTWARE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Asigna editores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvRecurso.Columns[this.unboundPrefix + fieldName];
            if(fieldName == "ViewStock")
            {
                DTO_pyProyectoDeta row = (DTO_pyProyectoDeta)this.gvRecurso.GetRow(e.RowHandle);
                if(row != null)
                {
                    if (row.TipoRecurso.Value == (byte)TipoRecurso.Insumo || row.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                        e.RepositoryItem = this.editBtnGrid;
                }

            }
        }

        #endregion

        #region Tareas

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void gvTarea_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowMvtoCurrent = (DTO_pyProyectoMvto)this.gvTarea.GetRow(e.FocusedRowHandle);

                if (this._rowMvtoCurrent != null && this._rowMvtoCurrent.CantidadTOT.Value < 0)
                    this.gvTarea.Columns[this.unboundPrefix + "CantidadSOL"].OptionsColumn.AllowEdit = false;
                else
                    this.gvTarea.Columns[this.unboundPrefix + "CantidadSOL"].OptionsColumn.AllowEdit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvTarea_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvTarea.Columns[this.unboundPrefix + fieldName]; 
                this._rowMvtoCurrent = (DTO_pyProyectoMvto)this.gvTarea.GetRow(e.RowHandle);
                if (fieldName == "CantidadSOL")
                {
                    decimal sol =  Convert.ToDecimal(e.Value);
                    if ((this._rowRecursoCurrent.DetalleMvto.Sum(x=>x.CantidadTOT.Value) - 
                        this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadPROV.Value) -
                        this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadSOL.Value)) >= 0)
                    {
                        this._rowMvtoCurrent.CantidadRecProp.Value = this._rowMvtoCurrent.CantidadNOM.Value + this._rowMvtoCurrent.CantidadACT.Value + this._rowMvtoCurrent.CantidadINV.Value;
                        //this._rowMvtoCurrent.CantidadSOL.Value = this._rowMvtoCurrent.CantidadTOT.Value - this._rowMvtoCurrent.CantidadSOL.Value - this._rowMvtoCurrent.CantidadRecProp.Value;
                        this._rowMvtoCurrent.CantidadSOLRecProp.Value = this._rowMvtoCurrent.CantidadTOT.Value - this._rowMvtoCurrent.CantidadPROV.Value - sol; 

                        this._rowRecursoCurrent.CantSolicitud.Value = this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadSOL.Value);
                        this._rowRecursoCurrent.CantidadSOLRecProp.Value = this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadSOLRecProp.Value);
                        this.gvTarea.ClearColumnErrors();
                    }
                    else
                        this.gvTarea.SetColumnError(col, "La cantidad solicitada no puede exceder a la disponible");
                }
                this.gvTarea.RefreshData();
                this.gvRecurso.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvDocument_CellValueChanged"));
            }           
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
  
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvTarea_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e) 
        {
            try
            {
                GridColumn col = this.gvTarea.Columns[this.unboundPrefix + "CantidadSOL"];
                this._rowMvtoCurrent = (DTO_pyProyectoMvto)this.gvTarea.GetRow(e.RowHandle);
                if (col.OptionsColumn.AllowEdit && this._rowMvtoCurrent != null && 
                    (this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadTOT.Value) - 
                    this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadPROV.Value) -
                     this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadSOL.Value)) < 0)
                {
                    this.gvTarea.SetColumnError(col, "La cantidad solicitada no puede exceder a la disponible");
                    e.Allow = false;                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "gvTarea_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvTarea_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double" )
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
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvTarea.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                Dictionary<string, object> filter = new Dictionary<string, object>();
                filter.Add("ReferenciaID", this._rowRecursoCurrent.RecursoID.Value);                
                ModalStandar origin = new ModalStandar (this._documentID,filter);
                origin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void editLink_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "editLink_Click"));
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
            this.gvTarea.PostEditor();
            if (this.gvTarea.HasColumnErrors)
            {
                MessageBox.Show("Valide que las cantidades solicitadas sean correctas antes de guardar");
            }
            else 
            {
                Thread process = new Thread(this.SaveThread);
                process.Start(); 
            }                         
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SendToAprrovOC);

            if (MessageBox.Show(msgDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();                
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.masterClaseServicio.ValidID)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    List<DTO_ExportTareas> tmp = new List<DTO_ExportTareas>();
		            foreach (DTO_pyProyectoTarea tarea in this._listTareasAll)
                    {
                        DTO_ExportTareas ex = new DTO_ExportTareas();
                        ex.TareaCliente.Value = tarea.TareaCliente.Value;        
                        ex.Descripcion.Value = tarea.Descriptivo.Value;
                        ex.UnidadInv.Value = tarea.UnidadInvID.Value;
                        ex.Cantidad.Value = tarea.Cantidad.Value;
                        ex.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value;
                        tmp.Add(ex);
                    } 
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportTareas), tmp);
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterClaseServicio.LabelRsx));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "TBExport"));
            }
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint() 
        {
            try
            {
                //DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
                //this.CargarInformacion(false);
                //solicitud.Detalle = ObjectCopier.Clone(this._listTareasAll);
                //solicitud.DetalleTareasAdic = ObjectCopier.Clone(this._listTareasAdicion);
                //solicitud.DocCtrl = this._ctrl;
                //solicitud.Header = ObjectCopier.Clone(this._preProyectoDocu);
                //string reportName = this._bc.AdministrationModel.Reportes_py_PlaneacionCostos(solicitud, string.Empty, Convert.ToByte(this.cmbReporte.EditValue), null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "TBPrint"));
            }
        }

        /// <summary>
        /// Actualiza la info actual
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (this._ctrl != null)
                {
                    DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(this._documentID, this._ctrl.PrefijoID.Value, this._ctrl.DocumentoNro.Value, null, string.Empty, string.Empty, false,true,false,false);
                    if (transaccion != null)
                    {
                        this._proyectoDocu = transaccion.HeaderProyecto;
                        this._listTareasAll = transaccion.DetalleProyecto;
                        this.CargarInformacion(true);
                        foreach (var tarea in _listTareasAll)
                            this.CalculateValues(tarea);
                        this._listTareasAdicion = transaccion.DetalleProyectoTareaAdic;
                        this._listMvtosProyecto = transaccion.Movimientos;
                        foreach (DTO_pyProyectoMvto mvto in this._listMvtosProyecto)
                        {
                            if (mvto.FechaOrdCompra.Value != null && mvto.FechaOrdCompra.Value.Value < DateTime.Now.Date)
                                mvto.FechaOrdCompra.Value = DateTime.Now.Date;
                        }
                        this.UpdateValues();
                        this._numeroDoc = this._ctrl.NumeroDoc.Value != null ? this._ctrl.NumeroDoc.Value.Value : 0;
                        this.LoadData();
                        this.LoadGrids(true);
                        this.EnableHeader(this._numeroDoc != 0 ? false : true);
                    }
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_GettingData));
                        this._ctrl = new DTO_glDocumentoControl();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "TBUpdate"));
            }
        }
        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { _documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                DTO_SerializedObject result = _bc.AdministrationModel.pyProyectoMvto_Upd(this._documentID,this._listMvtosProyecto,false);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this._documentID, this._actFlujo.seUsuarioID.Value, result, true, false);
                if (isOK)
                {
                    //this.newDoc = true;
                    //this.deleteOP = true;
                    //this.data = new DTO_prSolicitud();
                    //this._ctrl = new DTO_glDocumentoControl();
                    //this._solHeader = new DTO_prSolicitudDocu();
                    //this._solFooter = new List<DTO_prSolicitudFooter>();
                    //this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        /// <summary>
        /// Envia al paso de Analisis de Tiempos
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                this.gvTarea.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                DTO_SerializedObject result = _bc.AdministrationModel.pyProyectoMvto_Upd(this._documentID, this._listMvtosProyecto, true);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this._documentID, this._actFlujo.seUsuarioID.Value, result, true, false);
                if (isOK)
                {
                    //this.newDoc = true;
                    //this.deleteOP = true;
                    //this.data = new DTO_prSolicitud();
                    //this._ctrl = new DTO_glDocumentoControl();
                    //this._solHeader = new DTO_prSolicitudDocu();
                    //this._solFooter = new List<DTO_prSolicitudFooter>();
                    //this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionRecursos.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        #endregion                              

       
    }
}
