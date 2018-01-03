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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
 
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class PlaneacionCostosProyecto : FormWithToolbar
    {
      #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string userCurrent = string.Empty;
        private string userEditValues = string.Empty;
        private string empresaID = string.Empty;
        private int documentID;
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
        private decimal _IVAPresupuesto = 0;
        private bool _tareaIsFocused = false;
        private decimal _totalRecxTarea = 0;
        private decimal _totalPresupuesto = 0;
        private decimal _totalCliente = 0;
        private decimal _porcPrestManoObra = 0;
        private string _trabajoCurrent = string.Empty;
        private short nivelMax = 0;
        private GridView gvRecursoCurrent = new GridView();
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_pyProyectoTarea _rowTareaCurrent = new DTO_pyProyectoTarea();
        private DTO_pyProyectoDeta _rowRecursoCurrent = new DTO_pyProyectoDeta();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdicion = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoDeta> _listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
        private DTO_pyClaseProyecto _dtoClaseProyecto = null;
        private string _claseServicio = string.Empty;
        private string _tareaXDef = string.Empty;
        private string _trabajoXDef = string.Empty;
        private string _mdaLocalXDef = string.Empty;
        private string _mdaExtranjXDef = string.Empty;
        private decimal _tasaCambio = 0;
        private List<int> _tareasDeleted = new List<int>();
        private decimal porcIncrementoMult = 100;
        private byte? _versionCotizacion = 1;
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
                this.AssignRecursos();
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
        private void SendToApproveMethod() { this.RefreshForm(); }

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                this._listTareasAll = this._listTareasAll.OrderBy(x => x.CapituloTareaID.Value).ToList();
            this.gcDocument.DataSource = this._listTareasAll;
            this._tareaIsFocused = false;
            this.AssignRecursos();
            this.gcDocument.RefreshDataSource();
            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public PlaneacionCostosProyecto()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Proyecto.ToString());

                this.LoadDocumentInfo(true);
                this.AsignarTasaCambio(true);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "PlaneacionCostosProyecto"));
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
            tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaCliente");
            tareaCliente.UnboundType = UnboundColumnType.String;
            tareaCliente.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 8.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 7.9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tareaCliente.AppearanceCell.Options.UseTextOptions = true;
            tareaCliente.AppearanceHeader.Options.UseFont = true;
            tareaCliente.AppearanceCell.Options.UseFont = true;
            tareaCliente.VisibleIndex = 0;
            tareaCliente.Width = 36;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(tareaCliente);

            GridColumn tareaID = new GridColumn();
            tareaID.FieldName = this.unboundPrefix + "TareaID";
            tareaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaID");
            tareaID.UnboundType = UnboundColumnType.String;
            tareaID.VisibleIndex = 1;
            tareaID.Width = 45;
            tareaID.Visible = false;
            tareaID.ColumnEdit = this.editBtnGrid;
            tareaID.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(tareaID);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 2;
            descriptivo.Width = 332;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 3;
            UnidadInvID.Width = 30;
            UnidadInvID.Visible = true;
            UnidadInvID.ColumnEdit = this.editBtnGrid;
            UnidadInvID.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(UnidadInvID);

            GridColumn CantidadPresup = new GridColumn();
            CantidadPresup.FieldName = this.unboundPrefix + "CantidadPresup";
            CantidadPresup.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadPresup");
            CantidadPresup.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadPresup.UnboundType = UnboundColumnType.Decimal;
            CantidadPresup.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPresup.AppearanceCell.ForeColor = Color.Gray;
            CantidadPresup.AppearanceCell.Options.UseForeColor = true;
            CantidadPresup.AppearanceCell.Options.UseTextOptions = true;
            CantidadPresup.AppearanceCell.Options.UseFont = true;
            CantidadPresup.VisibleIndex = 4;
            CantidadPresup.Width = 33;
            CantidadPresup.Visible = true;
            CantidadPresup.ColumnEdit = this.editValue2Cant;
            CantidadPresup.OptionsColumn.AllowEdit = false;
            CantidadPresup.ToolTip = "Cantidad solicitada en Pre-Proyecto";
            this.gvDocument.Columns.Add(CantidadPresup);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 5;
            Cantidad.Width = 37;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(Cantidad);

            GridColumn CostoTotalUnitML = new GridColumn();
            CostoTotalUnitML.FieldName = this.unboundPrefix + "CostoTotalUnitML";
            CostoTotalUnitML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalTOTUnit");
            CostoTotalUnitML.UnboundType = UnboundColumnType.Decimal;
            CostoTotalUnitML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoTotalUnitML.AppearanceCell.Options.UseTextOptions = true;
            CostoTotalUnitML.VisibleIndex = 6;
            CostoTotalUnitML.Width = 55;
            CostoTotalUnitML.Visible = true;
            CostoTotalUnitML.ColumnEdit = this.editSpin;
            CostoTotalUnitML.OptionsColumn.AllowEdit = true;
            CostoTotalUnitML.AppearanceCell.BackColor = Color.Gainsboro;
            CostoTotalUnitML.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(CostoTotalUnitML);

            GridColumn CostoTotalML = new GridColumn();
            CostoTotalML.FieldName = this.unboundPrefix + "CostoTotalML";
            CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoTotalML");
            CostoTotalML.UnboundType = UnboundColumnType.Decimal;
            CostoTotalML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoTotalML.AppearanceCell.Options.UseTextOptions = true;
            CostoTotalML.VisibleIndex = 7;
            CostoTotalML.Width = 80;
            CostoTotalML.Visible = true;
            CostoTotalML.ColumnEdit = this.editSpin;
            CostoTotalML.OptionsColumn.AllowEdit = true;
            CostoTotalML.AppearanceCell.BackColor = Color.Gainsboro;
            CostoTotalML.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(CostoTotalML);

            GridColumn CostoLocalUnitCLI = new GridColumn();
            CostoLocalUnitCLI.FieldName = this.unboundPrefix + "CostoLocalUnitCLI";
            CostoLocalUnitCLI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalUnitCLI");
            CostoLocalUnitCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalUnitCLI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoLocalUnitCLI.AppearanceCell.Options.UseTextOptions = true;
            CostoLocalUnitCLI.VisibleIndex = 8;
            CostoLocalUnitCLI.Width = 50;
            CostoLocalUnitCLI.Visible = true;
            CostoLocalUnitCLI.ColumnEdit = this.editSpin;
            CostoLocalUnitCLI.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(CostoLocalUnitCLI);

            GridColumn CostoLocalCLI = new GridColumn();
            CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
            CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalCLI");
            CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalCLI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoLocalCLI.AppearanceCell.Options.UseTextOptions = true;
            CostoLocalCLI.VisibleIndex = 9;
            CostoLocalCLI.Width = 80;
            CostoLocalCLI.Visible = true;
            CostoLocalCLI.ColumnEdit = this.editSpin;
            CostoLocalCLI.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(CostoLocalCLI);

            GridColumn CostoDiferencia = new GridColumn();
            CostoDiferencia.FieldName = this.unboundPrefix + "CostoDiferenciaML";
            CostoDiferencia.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoDiferencia");
            CostoDiferencia.UnboundType = UnboundColumnType.Decimal;
            CostoDiferencia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoDiferencia.AppearanceCell.Options.UseTextOptions = true;
            CostoDiferencia.VisibleIndex = 10;
            CostoDiferencia.Width = 80;
            CostoDiferencia.Visible = true;
            CostoDiferencia.ColumnEdit = this.editSpin;
            CostoDiferencia.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(CostoDiferencia);

            GridColumn ImprimirTareaInd = new GridColumn();
            ImprimirTareaInd.FieldName = this.unboundPrefix + "ImprimirTareaInd";
            ImprimirTareaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ImprimirTareaInd");
            ImprimirTareaInd.UnboundType = UnboundColumnType.Boolean;
            ImprimirTareaInd.VisibleIndex = 11;
            ImprimirTareaInd.Width = 20;
            ImprimirTareaInd.Visible = true;
            ImprimirTareaInd.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(ImprimirTareaInd);

            GridColumn TituloPrintInd = new GridColumn();
            TituloPrintInd.FieldName = this.unboundPrefix + "TituloPrintInd";
            TituloPrintInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TituloPrintInd");
            TituloPrintInd.UnboundType = UnboundColumnType.Boolean;
            TituloPrintInd.VisibleIndex = 12;
            TituloPrintInd.Width = 20;
            TituloPrintInd.Visible = true;
            TituloPrintInd.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(TituloPrintInd);

            GridColumn UsuarioID = new GridColumn();
            UsuarioID.FieldName = this.unboundPrefix + "UsuarioID";
            UsuarioID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioID");
            UsuarioID.UnboundType = UnboundColumnType.String;
            UsuarioID.VisibleIndex = 13;
            UsuarioID.Width = 70;
            UsuarioID.Visible = false;
            UsuarioID.ColumnEdit = this.editBtnGrid;
            UsuarioID.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(UsuarioID);

            GridColumn Nivel = new GridColumn();
            Nivel.FieldName = this.unboundPrefix + "Nivel";
            Nivel.UnboundType = UnboundColumnType.Integer;
            Nivel.VisibleIndex = 14;
            Nivel.Width = 50;
            Nivel.Visible = false;
            Nivel.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(Nivel);

            GridColumn TareaPadre = new GridColumn();
            TareaPadre.FieldName = this.unboundPrefix + "TareaPadre";
            TareaPadre.UnboundType = UnboundColumnType.String;
            TareaPadre.VisibleIndex = 15;
            TareaPadre.Width = 50;
            TareaPadre.Visible = false;
            TareaPadre.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(TareaPadre);

            GridColumn DetalleInd = new GridColumn();
            DetalleInd.FieldName = this.unboundPrefix + "DetalleInd";
            DetalleInd.UnboundType = UnboundColumnType.Boolean;
            DetalleInd.VisibleIndex = 16;
            DetalleInd.Width = 50;
            DetalleInd.Visible = false;
            DetalleInd.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(DetalleInd);

            GridColumn CapituloTareaID = new GridColumn();
            CapituloTareaID.FieldName = this.unboundPrefix + "CapituloTareaID";
            CapituloTareaID.UnboundType = UnboundColumnType.String;
            CapituloTareaID.VisibleIndex = 16;
            CapituloTareaID.Visible = false;
            CapituloTareaID.UnGroup();            
            this.gvDocument.Columns.Add(CapituloTareaID);
            this.gvDocument.OptionsBehavior.Editable = true;
            #endregion

            #region Grilla Recursos x Trabajo
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 95;
            RecursoID.Visible = true;        
            RecursoID.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 200;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDrec = new GridColumn();
            UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            UnidadInvIDrec.UnboundType = UnboundColumnType.String;
            UnidadInvIDrec.VisibleIndex = 2;
            UnidadInvIDrec.Width = 70;
            UnidadInvIDrec.Visible = true;
            UnidadInvIDrec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDrec);

            GridColumn TrabajoID = new GridColumn();
            TrabajoID.FieldName = this.unboundPrefix + "TrabajoID";
            TrabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            TrabajoID.UnboundType = UnboundColumnType.String;
            TrabajoID.VisibleIndex = 3;
            TrabajoID.Width = 120;
            TrabajoID.Visible = false;
            TrabajoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(TrabajoID);

            //Cant x 
            GridColumn FactorID = new GridColumn();
            FactorID.FieldName = this.unboundPrefix + "FactorID";
            FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactorID");
            FactorID.UnboundType = UnboundColumnType.Decimal;
            FactorID.VisibleIndex = 5;
            FactorID.Width = 80;
            FactorID.Visible = true;
            FactorID.ColumnEdit = this.editValue8Cant;
            FactorID.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(FactorID);

            // Cant x Rec
            GridColumn CantidadRec = new GridColumn();
            CantidadRec.FieldName = this.unboundPrefix + "Cantidad";
            CantidadRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            CantidadRec.UnboundType = UnboundColumnType.Decimal;
            CantidadRec.VisibleIndex = 6;
            CantidadRec.Width = 80;
            CantidadRec.Visible = false;
            CantidadRec.ColumnEdit = this.editValue2Cant;
            CantidadRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadRec);

            //Cant X Trabajo
            GridColumn CanSolicitud = new GridColumn();
            CanSolicitud.FieldName = this.unboundPrefix + "CantSolicitud";
            CanSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantSolicitud");
            CanSolicitud.UnboundType = UnboundColumnType.Decimal;
            CanSolicitud.VisibleIndex = 7;
            CanSolicitud.Width = 80;
            CanSolicitud.Visible = false;
            CanSolicitud.ColumnEdit = this.editValue2Cant;
            CanSolicitud.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(CanSolicitud);

            GridColumn CantidadTOT = new GridColumn();
            CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadTOT");
            CantidadTOT.UnboundType = UnboundColumnType.Decimal;
            CantidadTOT.VisibleIndex = 8;
            CantidadTOT.Width = 80;
            CantidadTOT.Visible = false;
            CantidadTOT.ColumnEdit = this.editValue2Cant;
            CantidadTOT.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadTOT);

            //Costo Unit
            GridColumn CostoLocal = new GridColumn();
            CostoLocal.FieldName = this.unboundPrefix + "CostoLocal";
            CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocal");
            CostoLocal.UnboundType = UnboundColumnType.Decimal;
            CostoLocal.VisibleIndex =9;
            CostoLocal.Width = 80;
            CostoLocal.Visible = true;
            CostoLocal.ColumnEdit = this.editSpin;
            CostoLocal.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CostoLocal);

            GridColumn CostoLocalTOT = new GridColumn();
            CostoLocalTOT.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOT.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalTOT");
            CostoLocalTOT.UnboundType = UnboundColumnType.Decimal;
            CostoLocalTOT.VisibleIndex = 10;
            CostoLocalTOT.Width = 80;
            CostoLocalTOT.Visible = true;
            CostoLocalTOT.ColumnEdit = this.editSpin;
            CostoLocalTOT.OptionsColumn.AllowEdit = false;
            CostoLocalTOT.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            CostoLocalTOT.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
            this.gvRecurso.Columns.Add(CostoLocalTOT);

            GridColumn CostoTotalMLRec = new GridColumn();
            CostoTotalMLRec.FieldName = this.unboundPrefix + "CostoTotalML";
            CostoTotalMLRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocal");
            CostoTotalMLRec.UnboundType = UnboundColumnType.Decimal;
            CostoTotalMLRec.VisibleIndex = 12;
            CostoTotalMLRec.Width = 80;
            CostoTotalMLRec.Visible = false;
            CostoTotalMLRec.ColumnEdit = this.editSpin;
            CostoTotalMLRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CostoTotalMLRec);

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

            #region Grilla Detalle Recursos 
            GridColumn RecursoIDDet = new GridColumn();
            RecursoIDDet.FieldName = this.unboundPrefix + "RecursoID";
            RecursoIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoID");
            RecursoIDDet.UnboundType = UnboundColumnType.String;
            RecursoIDDet.VisibleIndex = 0;
            RecursoIDDet.Width = 80;
            RecursoIDDet.Visible = true;
            RecursoIDDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(RecursoIDDet);

            GridColumn RecursoDescet = new GridColumn();
            RecursoDescet.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDescet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoIDDesc");
            RecursoDescet.UnboundType = UnboundColumnType.String;
            RecursoDescet.VisibleIndex = 1;
            RecursoDescet.Width = 200;
            RecursoDescet.Visible = true;
            RecursoDescet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(RecursoDescet);

            GridColumn UnidadInvIDDet = new GridColumn();
            UnidadInvIDDet.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            UnidadInvIDDet.UnboundType = UnboundColumnType.Integer;
            UnidadInvIDDet.VisibleIndex = 2;
            UnidadInvIDDet.Width = 80;
            UnidadInvIDDet.Visible = true;
            UnidadInvIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(UnidadInvIDDet);

            GridColumn TrabajoIDDet = new GridColumn();
            TrabajoIDDet.FieldName = this.unboundPrefix + "TrabajoID";
            TrabajoIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            TrabajoIDDet.UnboundType = UnboundColumnType.String;
            TrabajoIDDet.VisibleIndex = 3;
            TrabajoIDDet.Width = 120;
            TrabajoIDDet.Visible = false;
            TrabajoIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(TrabajoIDDet);

            GridColumn FactorIDDet = new GridColumn();
            FactorIDDet.FieldName = this.unboundPrefix + "FactorID";
            FactorIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactorID");
            FactorIDDet.UnboundType = UnboundColumnType.Decimal;
            FactorIDDet.VisibleIndex = 5;
            FactorIDDet.Width = 80;
            FactorIDDet.Visible = true;
            FactorIDDet.OptionsColumn.AllowEdit = true;
            FactorIDDet.ColumnEdit = this.editValue8Cant;
            this.gvDetalleRecurso.Columns.Add(FactorIDDet);

            GridColumn CantidadDet = new GridColumn();
            CantidadDet.FieldName = this.unboundPrefix + "Cantidad";
            CantidadDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            CantidadDet.UnboundType = UnboundColumnType.Decimal;
            CantidadDet.VisibleIndex = 6;
            CantidadDet.Width = 80;
            CantidadDet.Visible = false;
            CantidadDet.OptionsColumn.AllowEdit = false;
            CantidadDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantidadDet);

            GridColumn CantSolicitudDet = new GridColumn();
            CantSolicitudDet.FieldName = this.unboundPrefix + "CantSolicitud";
            CantSolicitudDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantSolicitud");
            CantSolicitudDet.UnboundType = UnboundColumnType.Decimal;
            CantSolicitudDet.VisibleIndex = 7;
            CantSolicitudDet.Width = 80;
            CantSolicitudDet.Visible = false;
            CantSolicitudDet.OptionsColumn.AllowEdit = false;
            CantSolicitudDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantSolicitudDet);

            GridColumn CantidadTOTDet = new GridColumn();
            CantidadTOTDet.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadTOT");
            CantidadTOTDet.UnboundType = UnboundColumnType.Decimal;
            CantidadTOTDet.VisibleIndex = 8;
            CantidadTOTDet.Width = 80;
            CantidadTOTDet.Visible = false;
            CantidadTOTDet.OptionsColumn.AllowEdit = false;
            CantidadTOTDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantidadTOTDet);

            GridColumn CostoLocalDet = new GridColumn();
            CostoLocalDet.FieldName = this.unboundPrefix + "CostoLocal";
            CostoLocalDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocal");
            CostoLocalDet.UnboundType = UnboundColumnType.Decimal;
            CostoLocalDet.VisibleIndex = 9;
            CostoLocalDet.Width = 80;
            CostoLocalDet.Visible = true;
            CostoLocalDet.ColumnEdit = this.editSpin;
            CostoLocalDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(CostoLocalDet);

            GridColumn CostoLocalTOTDet = new GridColumn();
            CostoLocalTOTDet.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalTOT");
            CostoLocalTOTDet.UnboundType = UnboundColumnType.Decimal;
            CostoLocalTOTDet.VisibleIndex = 10;
            CostoLocalTOTDet.Width = 80;
            CostoLocalTOTDet.Visible = false;
            CostoLocalTOTDet.ColumnEdit = this.editSpin;
            CostoLocalTOTDet.OptionsColumn.AllowEdit = false;
            CostoLocalTOTDet.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            CostoLocalTOTDet.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
            this.gvDetalleRecurso.Columns.Add(CostoLocalTOTDet);

            GridColumn TipoRecursoDet = new GridColumn();
            TipoRecursoDet.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecursoDet.UnboundType = UnboundColumnType.Integer;
            TipoRecursoDet.Width = 80;
            TipoRecursoDet.Visible = false;
            TipoRecursoDet.Group();
            TipoRecursoDet.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecursoDet.SortOrder = ColumnSortOrder.Ascending;
            this.gvDetalleRecurso.Columns.Add(TipoRecursoDet);

            this.gvDetalleRecurso.OptionsBehavior.Editable = true;
            this.gvDetalleRecurso.OptionsView.ColumnAutoWidth = true;
            #endregion

            this.gvDocument.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvRecurso.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowRecurso()
        {
            try
            {
                decimal porcAdmin = Convert.ToDecimal(this.txtPorAdmEmp.EditValue,CultureInfo.InvariantCulture);
                decimal porcImpr = Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture);
                decimal porcUtil = Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture);

                DTO_pyPreProyectoTarea tareaTemp = new DTO_pyPreProyectoTarea();
                tareaTemp.NumeroDoc.Value = this._rowTareaCurrent.NumeroDoc.Value;
                tareaTemp.Consecutivo.Value = this._rowTareaCurrent.Consecutivo.Value;
                tareaTemp.VlrAIUxAPUAdmin.Value = this._rowTareaCurrent.VlrAIUxAPUAdmin.Value;
                tareaTemp.VlrAIUxAPUImpr.Value = this._rowTareaCurrent.VlrAIUxAPUImpr.Value;
                tareaTemp.VlrAIUxAPUUtil.Value = this._rowTareaCurrent.VlrAIUxAPUUtil.Value;
                tareaTemp.VlrAIUxAPUIVA.Value = this._rowTareaCurrent.VlrAIUxAPUIVA.Value;
                foreach (DTO_pyProyectoDeta det in this._rowTareaCurrent.Detalle)
                {
                    #region Asigna valores
                    DTO_pyPreProyectoDeta detTmp = new DTO_pyPreProyectoDeta();
                    detTmp.Consecutivo.Value = det.Consecutivo.Value;
                    detTmp.ConsecTarea.Value = det.ConsecTarea.Value;
                    detTmp.NumeroDoc.Value = det.NumeroDoc.Value;
                    detTmp.RecursoDesc.Value = det.RecursoDesc.Value;
                    detTmp.RecursoID.Value = det.RecursoID.Value;
                    detTmp.TipoRecurso.Value = det.TipoRecurso.Value;
                    detTmp.UnidadInvID.Value = det.UnidadInvID.Value;
                    detTmp.FactorID.Value = det.FactorID.Value;
                    detTmp.Cantidad.Value = det.Cantidad.Value;
                    detTmp.CantidadTOT.Value = det.CantidadTOT.Value;
                    detTmp.CostoLocal.Value = det.CostoLocal.Value;
                    detTmp.CostoLocalInicial.Value = det.CostoLocalInicial.Value;
                    detTmp.CostoLocConPrestacion.Value = det.CostoLocConPrestacion.Value;
                    detTmp.CostoLocalTOT.Value = det.CostoLocalTOT.Value;
                    detTmp.CostoLocalEMP.Value = det.CostoLocalEMP.Value;
                    detTmp.CostoLocalPRY.Value = det.CostoLocalPRY.Value;
                    detTmp.CostoExtra.Value = det.CostoExtra.Value;
                    detTmp.CostoExtraEMP.Value = det.CostoExtraEMP.Value;
                    detTmp.CostoExtraPRY.Value = det.CostoExtraPRY.Value;
                    detTmp.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                    detTmp.CantSolicitud.Value = det.CantSolicitud.Value;
                    detTmp.Distancia_Turnos.Value = det.Distancia_Turnos.Value;
                    detTmp.Peso_Cantidad.Value = det.Peso_Cantidad.Value;
                    detTmp.DocCompra.Value = det.DocCompra.Value;
                    detTmp.FechaFijadaInd.Value = det.FechaFijadaInd.Value;
                    detTmp.FechaFin.Value = det.FechaFin.Value;
                    detTmp.FechaInicio.Value = det.FechaInicio.Value;
                    detTmp.FechaInicioAUT.Value = det.FechaInicioAUT.Value;
                    detTmp.MarcaDesc.Value = det.MarcaDesc.Value;
                    detTmp.Modelo.Value = det.Modelo.Value;
                    detTmp.Observaciones.Value = det.Observaciones.Value;
                    detTmp.PorVariacion.Value = det.PorVariacion.Value;
                    detTmp.ProveedorID.Value = det.ProveedorID.Value;
                    detTmp.SelectInd.Value = det.SelectInd.Value;
                    detTmp.TareaID.Value = det.TareaID.Value;
                    detTmp.TiempoTotal.Value = det.TiempoTotal.Value;
                    detTmp.TrabajoID.Value = det.TrabajoID.Value;
                    detTmp.VlrAIUAdmin.Value = det.VlrAIUAdmin.Value;
                    detTmp.VlrAIUImpr.Value = det.VlrAIUImpr.Value;
                    detTmp.VlrAIUUtil.Value = det.VlrAIUUtil.Value;
                    detTmp.AjCambioExtra.Value = det.AjCambioExtra.Value;
                    detTmp.AjCambioLocal.Value = det.AjCambioLocal.Value;
                    tareaTemp.Detalle.Add(detTmp); 
                    #endregion
                }

                byte redondeo = Convert.ToByte(this.cmbRedondeo.EditValue);
                ModalAPUProyectos modal = new ModalAPUProyectos(tareaTemp, this.chkAPUIncluyeAIU.Checked, porcAdmin, porcImpr, porcUtil,
                                                                this._tasaCambio,redondeo,this.chkEquipoCant.Checked,this.chkPersonalCant.Checked, (Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture) / 100));
                modal.ShowDialog();
                this._rowTareaCurrent.VlrAIUxAPUAdmin.Value = tareaTemp.VlrAIUxAPUAdmin.Value;
                this._rowTareaCurrent.VlrAIUxAPUImpr.Value = tareaTemp.VlrAIUxAPUImpr.Value;
                this._rowTareaCurrent.VlrAIUxAPUUtil.Value = tareaTemp.VlrAIUxAPUUtil.Value;
                this._rowTareaCurrent.VlrAIUxAPUIVA.Value = tareaTemp.VlrAIUxAPUIVA.Value;
                this._rowTareaCurrent.Detalle = new List<DTO_pyProyectoDeta>();
                foreach (DTO_pyPreProyectoDeta det in modal.ListSelected)
                {
                    #region Asigna valores
		            DTO_pyProyectoDeta detTmp = new DTO_pyProyectoDeta();
                    detTmp.Consecutivo.Value = det.Consecutivo.Value;
                    detTmp.ConsecTarea.Value = det.ConsecTarea.Value;
                    detTmp.NumeroDoc.Value = det.NumeroDoc.Value;
                    detTmp.RecursoDesc.Value = det.RecursoDesc.Value;
                    detTmp.RecursoID.Value = det.RecursoID.Value;
                    detTmp.TipoRecurso.Value = det.TipoRecurso.Value;
                    detTmp.UnidadInvID.Value = det.UnidadInvID.Value;
                    detTmp.FactorID.Value = det.FactorID.Value;
                    detTmp.Cantidad.Value = det.Cantidad.Value;
                    detTmp.CantidadTOT.Value = det.CantidadTOT.Value;                  
                    detTmp.CostoLocal.Value = det.CostoLocal.Value;
                    detTmp.CostoLocalInicial.Value = det.CostoLocalInicial.Value;
                    detTmp.CostoLocConPrestacion.Value = det.CostoLocConPrestacion.Value;
                    detTmp.CostoLocalTOT.Value = det.CostoLocalTOT.Value;
                    detTmp.CostoLocalEMP.Value = det.CostoLocalEMP.Value;                
                    detTmp.CostoLocalPRY.Value = det.CostoLocalPRY.Value;                   
                    detTmp.CostoExtra.Value = det.CostoExtra.Value;
                    detTmp.CostoExtraEMP.Value = det.CostoExtraEMP.Value;
                    detTmp.CostoExtraPRY.Value = det.CostoExtraPRY.Value;
                    detTmp.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                    detTmp.CantSolicitud.Value = det.CantSolicitud.Value;
                    detTmp.Distancia_Turnos.Value = det.Distancia_Turnos.Value;
                    detTmp.Peso_Cantidad.Value = det.Peso_Cantidad.Value;
                    detTmp.DocCompra.Value = det.DocCompra.Value;                 
                    detTmp.FechaFijadaInd.Value = det.FechaFijadaInd.Value;
                    detTmp.FechaFin.Value = det.FechaFin.Value;
                    detTmp.FechaInicio.Value = det.FechaInicio.Value;
                    detTmp.FechaInicioAUT.Value = det.FechaInicioAUT.Value;
                    detTmp.MarcaDesc.Value = det.MarcaDesc.Value;
                    detTmp.Modelo.Value = det.Modelo.Value;
                    detTmp.Observaciones.Value = det.Observaciones.Value;                   
                    detTmp.PorVariacion.Value = det.PorVariacion.Value;
                    detTmp.ProveedorID.Value = det.ProveedorID.Value;                   
                    detTmp.SelectInd.Value = det.SelectInd.Value;
                    detTmp.TareaID.Value = det.TareaID.Value;
                    detTmp.TiempoTotal.Value = det.TiempoTotal.Value; 
                    detTmp.TrabajoID.Value = det.TrabajoID.Value;                   
                    detTmp.VlrAIUAdmin.Value = det.VlrAIUAdmin.Value;
                    detTmp.VlrAIUImpr.Value = det.VlrAIUImpr.Value;
                    detTmp.VlrAIUUtil.Value = det.VlrAIUUtil.Value;
                    detTmp.AjCambioExtra.Value = det.AjCambioExtra.Value;
                    detTmp.AjCambioLocal.Value = det.AjCambioLocal.Value;
                    this._rowTareaCurrent.Detalle.Add(detTmp); 
	                #endregion                
                }
                if (this._rowTareaCurrent.Detalle.Count > 0)
                {
                    this._rowTareaCurrent.DetalleInd.Value = true;
                    this._rowTareaCurrent.TituloPrintInd.Value = false;
                }                   
                else
                    this._rowTareaCurrent.DetalleInd.Value = this.nivelMax == this._rowTareaCurrent.Nivel.Value? true : false;
                foreach (var rec in this._rowTareaCurrent.Detalle)
                {
                    rec.TareaID.Value = this._rowTareaCurrent.TareaID.Value;
                    rec.TrabajoID.Value = string.IsNullOrEmpty(rec.TrabajoID.Value) ? this._trabajoCurrent : rec.TrabajoID.Value;
                    rec.Cantidad.Value = 1;
                    rec.CantSolicitud.Value = 1;
                }
                foreach (int rec in modal.RecursosDelete)
                {
                    if (!this._proyectoDocu.RecursosDeleted.Exists(x=>x == rec))
                        this._proyectoDocu.RecursosDeleted.Add(rec);
                } 

                this.AssignRecursos();
                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowTarea()
        {
            try
            {
                DTO_pyProyectoTarea footerDet = new DTO_pyProyectoTarea();

                #region Asigna datos a la fila
                try
                {
                    if (this._listTareasAll.Count > 0)
                    {
                        if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                        {
                            decimal indexTareaID = Convert.ToDecimal(this._listTareasAll.Last().TareaID.Value, CultureInfo.InvariantCulture) + 1;
                            footerDet.TareaID.Value = indexTareaID.ToString();
                        }
                        footerDet.Index = this._listTareasAll.Last().Index + 1;
                        footerDet.TareaCliente.Value = this._listTareasAll.Last().TareaCliente.Value;
                        footerDet.Cantidad.Value = 1;
                        footerDet.CantidadTarea.Value = 1;
                        footerDet.CostoLocalCLI.Value = 0;
                        footerDet.CostoTotalUnitML.Value = 0;
                        footerDet.CostoTotalML.Value = 0;
                        footerDet.UsuarioID.Value = this.userCurrent;
                    }
                    else
                    {
                        footerDet.Index = 0;
                        footerDet.TareaCliente.Value = string.Empty;
                        footerDet.TareaID.Value = "10000";
                        footerDet.Descriptivo.Value = string.Empty;
                        footerDet.UnidadInvID.Value = string.Empty;
                        footerDet.Cantidad.Value = 1;
                        footerDet.CantidadTarea.Value = 1;
                        footerDet.CostoLocalCLI.Value = 0;
                        footerDet.CostoTotalUnitML.Value = 0;
                        footerDet.CostoTotalML.Value = 0;
                        footerDet.UsuarioID.Value = this.userCurrent;
                    }                       
                    this._listTareasAll.Add(footerDet);
                    this._rowTareaCurrent = footerDet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
                this.gcDocument.DataSource = this._listTareasAll;
                this.gcDocument.RefreshDataSource();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
                this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].OptionsColumn.AllowEdit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna los recursos  a cada tarea
        /// </summary>
        private void AssignRecursos()
        {
            try
            {
                if (this._tareaIsFocused)
                {
                    #region Recursos por Tarea
                    if (this._rowTareaCurrent.Detalle.Count > 0)
                        this.CalculateValues(this._rowTareaCurrent);

                    if (this.gvRecurso.DataRowCount > 0)
                        this.gvRecurso.FocusedRowHandle = 0;
                    this.gcRecurso.DataSource = this._rowTareaCurrent.Detalle;
                    #endregion

                    this.gcRecurso.RefreshDataSource();
                    this.gvRecurso.MoveFirst();

                    this.gvDocument.RefreshRow(this._rowTareaCurrent.Index.Value);
                    this.gvDocument.RefreshData();
                }
                else
                {
                    foreach (var tarea in this._listTareasAll)
                        this.CalculateValues(tarea);
                }
                this.UpdateValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        private bool AsignarTasaCambio(bool fromTop)
        {
            if (!this._bc.AdministrationModel.MultiMoneda)
                this.txtTasaCambio.EditValue = 0;
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(1);
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (tc == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                    return false;
                }
            }
            return true;
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
                    if (d.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                    {
                        if (!this.chkEquipoCant.Checked)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * rend, 2);
                    }
                    else if (d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        if (!this.chkPersonalCant.Checked)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * rend, 2);
                    }
                    else 
                        d.CostoLocalTOT.Value = Math.Round(d.CantidadTOT.Value.Value * d.CostoLocal.Value.Value * rend, 2); //Valor Total por recurso
                } 
                #endregion
                #region Asigna valores
                decimal costoTotalUnitTarea = tarea.Detalle.Sum(x => x.CostoLocalTOT.Value.Value);
                #region Calcula AIU por APU
                if (this.chkAPUIncluyeAIU.Checked)
                {
                    decimal AIUAdminAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorAdmEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    decimal AIUImprAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    decimal AIUUtilAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    tarea.Detalle.ForEach(x=>{
                                            x.VlrAIUAdmin.Value = AIUAdminAPU;
                                            x.VlrAIUImpr.Value = AIUImprAPU;
                                            x.VlrAIUUtil.Value = AIUUtilAPU;
                                            });
                    tarea.VlrAIUxAPUAdmin.Value = AIUAdminAPU;
                    tarea.VlrAIUxAPUImpr.Value = AIUImprAPU;
                    tarea.VlrAIUxAPUUtil.Value = AIUUtilAPU;
                    //tarea.VlrAIUxAPUIVA.Value = Math.Round(tarea.Detalle.Where(x => x.TipoRecurso.Value != (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value) * (Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture) / 100), 2);
                    costoTotalUnitTarea += 0;// AIUAdminAPU + AIUImprAPU + AIUUtilAPU ;
                } 
                #endregion
                if (this.cmbRedondeo.EditValue.Equals("2"))//Redonde hacia arriba
                    tarea.CostoTotalUnitML.Value = Math.Ceiling(costoTotalUnitTarea);
                else if (this.cmbRedondeo.EditValue.Equals("3")) //Redondea hacia abajo
                    tarea.CostoTotalUnitML.Value = Math.Floor(costoTotalUnitTarea);
                else
                    tarea.CostoTotalUnitML.Value = costoTotalUnitTarea;
                tarea.CostoTotalML.Value = Math.Round(tarea.CostoTotalUnitML.Value.Value * tarea.Cantidad.Value.Value, 0);
                this._totalRecxTarea = tarea.CostoTotalML.Value.Value;
                if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) //Interno
                {
                    tarea.PorDescuento.Value = tarea.PorDescuento.Value == null ? 0 : tarea.PorDescuento.Value;
                    //if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                    //    tarea.CostoLocalUnitCLI.Value = Math.Round(tarea.CostoTotalUnitML.Value.Value * (this.porcIncrementoMult / 100) - (tarea.CostoTotalUnitML.Value.Value * (tarea.PorDescuento.Value.Value / 100)), 0);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                else if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
                {
                    tarea.PorDescuento.Value = tarea.PorDescuento.Value == null ? 0 : tarea.PorDescuento.Value;
                    //if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                    //    tarea.CostoLocalUnitCLI.Value = Math.Round(tarea.CostoTotalUnitML.Value.Value * (this.porcIncrementoMult / 100) - (tarea.CostoTotalUnitML.Value.Value * (tarea.PorDescuento.Value.Value / 100)), 0);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                tarea.CostoDiferenciaML.Value = tarea.CostoLocalCLI.Value - tarea.CostoTotalML.Value;

                if (this.cmbTipoPresup.EditValue == "1")
                    tarea.LineaPresupuestoID.Value = tarea.TareaCliente.Value;
                if (tarea.Nivel.Value > 1)
                {
                    //Calcula totales(Sumatorias) por cada Nivel
                    var rowPadre1 = this._listTareasAll.Find(x=>x.TareaCliente.Value == tarea.TareaPadre.Value);
                    if (rowPadre1 != null)
                    {
                        rowPadre1.CostoTotalML.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                        rowPadre1.CostoLocalCLI.Value = this._listTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                        if (rowPadre1.Nivel.Value > 1)
                        {
                            var rowPadre2 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre1.TareaPadre.Value);
                            if (rowPadre2 != null)
                            {
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
                }
                #endregion        
                #region Actualiza Datos
                this.gvRecursoCurrent.RefreshData();    
                this.gvDocument.RefreshData();
                this.gvRecurso.RefreshData();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "CalculateValues"));
            }
        }

        /// <summary>
        /// Calcula la jerarquia de cada tarea para saber si es detalle o no
        /// </summary>
        /// <param name="tareaCliente">tarea actual a validar</param>
        /// <param name="value">valor ingresado</param>
        private void CalculateHierarchy(DTO_pyProyectoTarea tareaCliente, string value)
        {
            try
            {
                //Obtiene los niveles digitados en TareaCliente
                string[] nivel = value.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                tareaCliente.Nivel.Value = Convert.ToInt16(nivel.Count(x => !x.Equals("0") && !x.Equals("00") && !x.Equals("000")));

                if (this.nivelMax == 1) tareaCliente.Nivel.Value = 1;

                //Valida si es titulo
                if (nivel.Contains("tit") || nivel.Contains("TIT"))  tareaCliente.Nivel.Value = 0;

                //Valida si el registro es el ultimo Nivel de movimiento
                if (this.nivelMax == tareaCliente.Nivel.Value)
                {
                    tareaCliente.DetalleInd.Value = true;
                    tareaCliente.TituloPrintInd.Value = false;
                }
                   
                else
                {
                    //Valida si tiene cantidad para indicar que sea de movimiento
                    if (tareaCliente.Cantidad.Value != 0 && tareaCliente.Cantidad.Value != null)
                    {
                        tareaCliente.DetalleInd.Value = true;
                        tareaCliente.TituloPrintInd.Value = false;
                    }
                        
                    else
                    {
                        tareaCliente.DetalleInd.Value = false;
                        tareaCliente.TituloPrintInd.Value = true;
                    }
                      
                }
                //Asigna la tareaPadre de la tareaCliente digitada
                if (tareaCliente.Index == 0)
                    tareaCliente.TareaPadre.Value = string.Empty;
                else
                {
                   int nivelPreview = tareaCliente.Nivel.Value.Value - 1;
                   var rowPreview = this._listTareasAll.FindLast(x => x.Nivel.Value == nivelPreview && !x.TareaCliente.Value.Equals("tit") && !x.TareaCliente.Value.Equals("TIT"));
                    if(rowPreview != null)
                        tareaCliente.TareaPadre.Value = rowPreview.TareaCliente.Value;
                }
            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "CalculateHierarchy"));
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
                    this._proyectoDocu.ClaseServicioID.Value = this.masterClaseServicio.Value;
                    this._proyectoDocu.ClienteID.Value = this.masterCliente.Value;
                    this._proyectoDocu.DescripcionSOL.Value = this.txtDescripcion.Text;
                    this._proyectoDocu.EmpresaNombre.Value = this.txtSolicitante.Text;
                    this._proyectoDocu.ResponsableCLI.Value = this.txtResposableCli.Text;
                    this._proyectoDocu.ResponsableEMP.Value = this.masterResponsableEmp.Value;
                    this._proyectoDocu.ResponsableCorreo.Value = this.txtCorreo.Text;
                    this._proyectoDocu.ResponsableTelefono.Value = this.txtTelefono.Text;
                    this._proyectoDocu.RecursosXTrabajoInd.Value = false;
                    this._proyectoDocu.PropositoProyecto.Value = Convert.ToByte(this.cmbProposito.EditValue);
                    this._proyectoDocu.TipoSolicitud.Value = Convert.ToByte(this.cmbTipoSolicitud.EditValue);
                    this._proyectoDocu.Licitacion.Value = this.txtLicitacion.Text;
                    this._proyectoDocu.APUIncluyeAIUInd.Value = this.chkAPUIncluyeAIU.Checked;
                    this._proyectoDocu.Jerarquia.Value = Convert.ToByte(this.cmbJerarquia.EditValue);
                    this._proyectoDocu.PorClienteADM.Value = Convert.ToDecimal(this.txtPorAdmClient.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorClienteIMP.Value = Convert.ToDecimal(this.txtPorImprClient.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorClienteUTI.Value = Convert.ToDecimal(this.txtPorUtilClient.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorEmpresaADM.Value = Convert.ToDecimal(this.txtPorAdmEmp.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorEmpresaIMP.Value = Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorEmpresaUTI.Value = Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorMultiplicadorPresup.Value = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.MonedaPresupuesto.Value = this.masterMonedaPresup.Value;
                    this._proyectoDocu.TasaCambio.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.Version.Value = this._versionCotizacion;
                    this._proyectoDocu.TipoRedondeo.Value = Convert.ToByte(this.cmbRedondeo.EditValue);
                    this._proyectoDocu.EquipoCantidadInd.Value = this.chkEquipoCant.Checked;
                    this._proyectoDocu.PersonalCantidadInd.Value = this.chkPersonalCant.Checked;
                    this._proyectoDocu.MultiplicadorActivoInd.Value = false;
                    this._proyectoDocu.PorIVA.Value = Convert.ToInt32(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.PorcRteGarantia.Value = Convert.ToDecimal(this.txtPorRetGarantia.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.RteGarantiaIvaInd.Value = this.chkRteGarIncluyeIVA.Checked;
                    this._proyectoDocu.ValorAnticipoInicial.Value = Convert.ToDecimal(this.txtAnticipoInicial.EditValue, CultureInfo.InvariantCulture);

                    //Adicionales
                    this._proyectoDocu.Valor.Value = Convert.ToDecimal(this.txtCostoPresupuesto.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.ValorCliente.Value = Convert.ToDecimal(this.txtCostoCliente.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.ValorIVA.Value = Convert.ToDecimal(this.txtIVA.EditValue, CultureInfo.InvariantCulture);
                    this._proyectoDocu.ValorOtros.Value = Convert.ToDecimal(this.txtOtros.EditValue, CultureInfo.InvariantCulture);
                }
                else
                {
                    this.cmbTipoPresup.EditValue = this._dtoClaseProyecto != null? this._dtoClaseProyecto.TipoPresupuesto.Value.ToString() : "1";
                    this.masterClaseServicio.Value = this._proyectoDocu.ClaseServicioID.Value;
                    this.masterCliente.Value = this._proyectoDocu.ClienteID.Value;
                    this.masterResponsableEmp.Value = this._proyectoDocu.ResponsableEMP.Value;
                    this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
                    this.masterCentroCto.Value = this._ctrl.CentroCostoID.Value;
                    this.masterAreaFuncional.Value = this._ctrl.AreaFuncionalID.Value;
                    this.masterMonedaPresup.Value = this._proyectoDocu.MonedaPresupuesto.Value;                  
                    this.cmbTipoSolicitud.EditValue = this._proyectoDocu.TipoSolicitud.Value.ToString();
                    this.cmbProposito.EditValue = this._proyectoDocu.PropositoProyecto.Value.ToString();
                    this.cmbJerarquia.EditValue = this._proyectoDocu.Jerarquia.Value.ToString();                 
                    this.chkAPUIncluyeAIU.Checked = this._proyectoDocu.APUIncluyeAIUInd.Value.Value;
                    this.txtObservaciones.Text = this._ctrl.Observacion.Value;
                    this.txtDescripcion.Text = this._proyectoDocu.DescripcionSOL.Value;
                    this.txtSolicitante.Text = this._proyectoDocu.EmpresaNombre.Value;
                    this.txtResposableCli.Text = this._proyectoDocu.ResponsableCLI.Value;
                    this.txtCorreo.Text = this._proyectoDocu.ResponsableCorreo.Value;
                    this.txtTelefono.Text = this._proyectoDocu.ResponsableTelefono.Value;
                    this.txtLicitacion.Text = this._proyectoDocu.Licitacion.Value;
                    this.txtPorAdmClient.EditValue = this._proyectoDocu.PorClienteADM.Value;
                    this.txtPorImprClient.EditValue = this._proyectoDocu.PorClienteIMP.Value;
                    this.txtPorUtilClient.EditValue = this._proyectoDocu.PorClienteUTI.Value;
                    this.txtPorAdmEmp.EditValue = this._proyectoDocu.PorEmpresaADM.Value;
                    this.txtPorImprEmp.EditValue = this._proyectoDocu.PorEmpresaIMP.Value;
                    this.txtPorUtilEmp.EditValue = this._proyectoDocu.PorEmpresaUTI.Value;
                    this.txtMultiplicador.EditValue = this._proyectoDocu.PorMultiplicadorPresup.Value != null? this._proyectoDocu.PorMultiplicadorPresup.Value : 100;                  
                    this.txtTasaCambio.EditValue = this._proyectoDocu.TasaCambio.Value;
                    this.btnVersion.Text = "Versión Actual: " + this._proyectoDocu.Version.Value.ToString();
                    this._versionCotizacion = this._proyectoDocu.Version.Value;
                    this.cmbRedondeo.EditValue = this._proyectoDocu.TipoRedondeo.Value.ToString();
                    this.chkEquipoCant.Checked = this._proyectoDocu.EquipoCantidadInd.Value.Value;
                    this.chkPersonalCant.Checked = this._proyectoDocu.PersonalCantidadInd.Value.Value;
                    this.chkMultipActivo.Checked = false;
                    this.txtPorIVA.EditValue = this._proyectoDocu.PorIVA.Value.Value;
                    this.txtPorRetGarantia.EditValue = this._proyectoDocu.PorcRteGarantia.Value;
                    this.chkRteGarIncluyeIVA.Checked = this._proyectoDocu.RteGarantiaIvaInd.Value.HasValue? this._proyectoDocu.RteGarantiaIvaInd.Value.Value : false;
                    this.txtAnticipoInicial.EditValue = this._proyectoDocu.ValorAnticipoInicial.Value;

                    //Adicionales
                    this.txtIVA.EditValue = this._ctrl.Iva.Value;
                    this.txtOtros.EditValue = this._proyectoDocu.ValorOtros.Value;                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "CargarInformacion"));
            }
        }

        /// <summary>
        /// Restaura los valores iniciales
        /// </summary>
        /// <param name="p"></param>
        private void CleanHeader()
        {
            try
            {
                this.masterCliente.Value = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.masterResponsableEmp.Value = string.Empty;
                this.txtDescripcion.Text = string.Empty;
                this.txtSolicitante.Text = string.Empty;
                this.txtResposableCli.Text = string.Empty;
                this.txtCorreo.Text = string.Empty;
                this.txtTelefono.Text = string.Empty;
                this.txtObservaciones.Text = string.Empty;
                this.txtLicitacion.Text = string.Empty;
                this.txtMultiplicador.EditValue = 100;
                this.txtPorAdmEmp.EditValue = 0;
                this.txtPorImprEmp.EditValue = 0;
                this.txtPorUtilEmp.EditValue = 0;
                this.txtPorAdmClient.EditValue = 0;
                this.txtPorImprClient.EditValue = 0;
                this.txtPorUtilClient.EditValue = 0;
                this.txtCostoPresupuesto.EditValue = 0;
                this.txtCostoCliente.EditValue = 0;
                this.txtCostoMult.EditValue = 0;
                this.txtCostoTotalAPU.EditValue = 0;
                this.txtCostoAPU.EditValue = 0;
                this.txtCostoAIUxAPU.EditValue = 0;
                this.txtIVA.EditValue = 0;
                this.txtTasaCambio.EditValue = 0;
                this.txtAnticipoInicial.EditValue = 0;
                this.txtPorRetGarantia.EditValue = 0;
                this.cmbProposito.EditValue = ((int)TipoSolicitud.Cotizacion);
                this.dtFecha.DateTime = DateTime.Now;
                this.btnVersion.Text = "Versión Actual:";
                this.txtModelo.EditValue = string.Empty;
                this.txtMarca.EditValue = string.Empty;
                this.txtDistanciaTurno.EditValue = 0;
                this.txtPesoCant.EditValue = 0;

                this.chkEquipoCant.Checked = true;
                this.chkPersonalCant.Checked = true;
                this.chkRteGarIncluyeIVA.Checked = false;

            }
            catch (Exception ex)
            {                
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "CleanHeader"));
            }
        }

        /// <summary>
        /// Cacula el valor del cliente si incluye AIU el APU o no
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        private decimal GetValorCliente(DTO_pyProyectoTarea tarea)
        {
            decimal result = 0;
            decimal costoBaseIVA = 0;
            try
            {
                if (this.chkAPUIncluyeAIU.Checked)
                {
                    costoBaseIVA = tarea.CostoTotalUnitML.Value.Value - tarea.Detalle.Where(x => x.TipoRecurso.Value == (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value);
                    decimal valorIVA = costoBaseIVA * (Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture) / 100);

                    decimal base1 = (costoBaseIVA + valorIVA) * (this.porcIncrementoMult / 100);
                    decimal valorManoObra = tarea.Detalle.Where(x => x.TipoRecurso.Value == (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value) * (this.porcIncrementoMult / 100);
                    result = Math.Round(base1 + valorManoObra - (base1 + valorManoObra) * (tarea.PorDescuento.Value.Value / 100));
                }
                else
                    result = Math.Round((tarea.CostoTotalUnitML.Value.Value - (tarea.CostoTotalUnitML.Value.Value * (tarea.PorDescuento.Value.Value / 100))) * (this.porcIncrementoMult / 100), 0);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosPro.cs", "GetValorCliente"));
                return 0;
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
            this.masterResponsableEmp.EnableControl(p);
            this.txtNro.Enabled = p;
            this.btnQueryDoc.Enabled = p;
            this.txtResposableCli.Enabled = p;
            this.txtCorreo.Enabled = p;
            this.txtTelefono.Enabled = p;
            this.cmbJerarquia.Enabled = p;
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, false, true, true);
                this._bc.InitMasterUC(this.masterAreaFuncional, AppMasters.glAreaFuncional, true, true, true, true);
                this._bc.InitMasterUC(this.masterClaseServicio, AppMasters.pyClaseProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterResponsableEmp, AppMasters.seUsuario, false, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true,true);
                this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, true);
                this._bc.InitMasterUC(this.masterUsuarioPermiso, AppMasters.seUsuario, true, false, true,false);
                this._bc.InitMasterUC(this.masterMonedaPresup, AppMasters.glMoneda, true, false, true, false);
                //this.masterMonedaPresup.EnableControl(false);

                #region Combos

                Dictionary<string, string> datosTipoSolic = new Dictionary<string, string>();
                datosTipoSolic.Add("1", "Implementación");
                datosTipoSolic.Add("2", "Desarrollo");
                datosTipoSolic.Add("3", "Investigación");
                datosTipoSolic.Add("4", "Obra");
                datosTipoSolic.Add("5", "Interventoria");
                this.cmbTipoSolicitud.Properties.ValueMember = "Key";
                this.cmbTipoSolicitud.Properties.DisplayMember = "Value";
                this.cmbTipoSolicitud.Properties.DataSource = datosTipoSolic;
                this.cmbTipoSolicitud.EditValue = "1";

                Dictionary<string, string> datosProposito = new Dictionary<string, string>();
                datosProposito.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cotizacion));
                datosProposito.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Licitacion));
                datosProposito.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Interna));
                this.cmbProposito.Properties.ValueMember = "Key";
                this.cmbProposito.Properties.DisplayMember = "Value";
                this.cmbProposito.Properties.DataSource = datosProposito;
                this.cmbProposito.EditValue = "1";

                Dictionary<string, string> datosJerarquia = new Dictionary<string, string>();
                datosJerarquia.Add("1", "Nivel 1 - Ej: 1");
                datosJerarquia.Add("2", "Nivel 2 - Ej: 1.1");
                datosJerarquia.Add("3", "Nivel 3 - Ej: 1.1.1");
                datosJerarquia.Add("4", "Nivel 4 - Ej: 1.1.1.1");
                datosJerarquia.Add("5", "Nivel 5 - Ej: 1.1.1.1.1");
                datosJerarquia.Add("6", "Nivel 6 - Ej: 1.1.1.1.1.1");
                datosJerarquia.Add("7", "Nivel 7 - Ej: 1.1.1.1.1.1.1");
                datosJerarquia.Add("8", "Nivel 8 - Ej: 1.1.1.1.1.1.1.1");
                this.cmbJerarquia.Properties.ValueMember = "Key";
                this.cmbJerarquia.Properties.DisplayMember = "Value";
                this.cmbJerarquia.Properties.DataSource = datosJerarquia;
                this.cmbJerarquia.EditValue = "1";

                Dictionary<string, string> datosReporte = new Dictionary<string, string>(); 
                datosReporte.Add("1", "Presupuesto"); //Revisar como se valida por usuario
                datosReporte.Add("2", "Presupuesto Cliente");
                datosReporte.Add("3", "Presupuesto Comparativo");
                datosReporte.Add("4", "A.P.U Detallado(Presupuesto)");
                datosReporte.Add("5", "A.P.U Detallado(Cliente)");
                datosReporte.Add("6", "A.P.U Resumido-Compras");
                datosReporte.Add("7", "A.P.U Resumido-Clientes");
                datosReporte.Add("8", "Presupuesto Detallado");
                this.cmbReporte.Properties.ValueMember = "Key";
                this.cmbReporte.Properties.DisplayMember = "Value";
                this.cmbReporte.Properties.DataSource = datosReporte;
                this.cmbReporte.EditValue = "2";

                Dictionary<string, string> datosTipoPresupuesto = new Dictionary<string, string>();
                datosTipoPresupuesto.Add("1", "Cliente");
                datosTipoPresupuesto.Add("2", "Interno");
                this.cmbTipoPresup.Properties.ValueMember = "Key";
                this.cmbTipoPresup.Properties.DisplayMember = "Value";
                this.cmbTipoPresup.Properties.DataSource = datosTipoPresupuesto;
                this.cmbTipoPresup.EditValue = "1";

                Dictionary<string, string> datosRedondeo = new Dictionary<string, string>();
                datosRedondeo.Add("1", "Normal");
                datosRedondeo.Add("2", "Al Mayor valor");
                datosRedondeo.Add("3", "Al Menor valor");
                this.cmbRedondeo.Properties.ValueMember = "Key";
                this.cmbRedondeo.Properties.DisplayMember = "Value";
                this.cmbRedondeo.Properties.DataSource = datosRedondeo;
                this.cmbRedondeo.EditValue = "1";
                #endregion

                this.formatRecursos = _bc.GetImportExportFormat(typeof(DTO_ExportRecursosDeta), AppForms.MasterReportXls);
                this.formatTareas = _bc.GetImportExportFormat(typeof(DTO_pyProyectoTarea), AppDocuments.PreProyecto);
                this._tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);               
                string proyectoxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string centroCto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string IVAPresup = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcIVAUtilidad);
                string prestacionMO = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcPrestacionManoObra);
                this._mdaLocalXDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._mdaExtranjXDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string factorIncremento = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_FactorIncrementoComercial);
                this.userEditValues = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_UsuarioEdicion);

                if (!string.IsNullOrEmpty(IVAPresup))
                    this._IVAPresupuesto = Convert.ToDecimal(IVAPresup, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(prestacionMO))
                    this._porcPrestManoObra = Convert.ToDecimal(prestacionMO, CultureInfo.InvariantCulture);
                else
                    this._porcPrestManoObra = 100;

                if (!string.IsNullOrEmpty(factorIncremento) && !factorIncremento.Equals("0"))
                    this.porcIncrementoMult = Convert.ToDecimal(factorIncremento, CultureInfo.InvariantCulture);

                this.txtMultiplicador.EditValue = this.porcIncrementoMult;
                this.masterResponsableEmp.Value = (this._bc.AdministrationModel.User).ID.Value;
                this.masterProyecto.Value = string.Empty;
                this.masterCentroCto.Value = centroCto;
                this.masterMonedaPresup.Value = this._mdaLocalXDef;
                this.btnVersion.Text = "Versión Actual: " + this._versionCotizacion.ToString();
                this._listRecursosXTareaAll = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, string.Empty, string.Empty, null, false,false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "InitControls"));
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

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true,AppDocuments.Proyecto.ToString(), true);

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

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.py_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrid(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                        this._listTareasAll = this._listTareasAll.OrderBy(x => x.CapituloTareaID.Value).ToList();
                    this.gcDocument.DataSource = this._listTareasAll;
                    this.gcDocument.RefreshDataSource();
                    this.gcRecurso.DataSource = null;
                    this.gcRecurso.RefreshDataSource();
                }

                //this.gvDocument.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del documento
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                int? docNro = null;
                if (!string.IsNullOrEmpty(this.txtNro.Text)) docNro = Convert.ToInt32(this.txtNro.Text);

                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(this.documentID, this.masterPrefijo.Value, docNro, null, string.Empty, this.masterProyecto.Value, false, false, false, false);
                if (transaccion != null)
                {
                    this._ctrl = transaccion.DocCtrl;  
                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto;
                    this._claseServicio = transaccion.Header.ClaseServicioID.Value;
                    this._dtoClaseProyecto = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, transaccion.HeaderProyecto.ClaseServicioID.Value, true);
                    this.CargarInformacion(true);
                    foreach (var tarea in _listTareasAll)
                        this.CalculateValues(tarea);
                    this._listTareasAdicion = transaccion.DetalleProyectoTareaAdic;
                    this.UpdateValues();
                    this._numeroDoc = this._ctrl.NumeroDoc.Value != null ? this._ctrl.NumeroDoc.Value.Value : 0;
                    this.LoadGrid(true);
                    this.EnableHeader(this._numeroDoc != 0 ? false : true);
                    if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros && this._proyectoDocu.Version.Value == this._proyectoDocu.VersionPrevia.Value)
                    {
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemImport.Enabled = false;
                        this.gcDocument.Enabled = false;
                        this.gcRecurso.Enabled = false;
                        this.chkMultipActivo.Enabled = false;
                        this.btnVersion.Enabled = true;                      
                    }
                    else
                    {
                        FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                        FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                        this.gcDocument.Enabled = true;
                        this.gcRecurso.Enabled = true;
                        this.chkMultipActivo.Enabled = true;
                        this.btnVersion.Enabled = false;                     
                    }
                    if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) //Deshabilita la edicion de Multiplicador en Servicios
                        this.chkMultipActivo.Enabled = false;

                    this.btnLoad.Enabled = false;
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.InvalidDocument));
                    this._ctrl = new DTO_glDocumentoControl();
                    this.btnLoad.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempoProy", "LoadGrid"));
            }
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        private decimal LoadTasaCambio(int monOr)
        {
            try
            {
                decimal valor = 0;
                //string tasaMon = this.monedaId;
                //if (monOr == (int)TipoMoneda.Local)
                //    tasaMon = this.monedaExtranjera;

                valor = _bc.AdministrationModel.TasaDeCambio_Get(this._mdaExtranjXDef, this.dtFecha.DateTime);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "LoadTasaCambio"));
                return 0;
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
            string factorIncremento = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_FactorIncrementoComercial);

            if (!string.IsNullOrEmpty(factorIncremento) && !factorIncremento.Equals("0"))
                this.porcIncrementoMult = Convert.ToDecimal(factorIncremento, CultureInfo.InvariantCulture);

            this.masterPrefijo.Value = this.prefijoID;
            this.masterAreaFuncional.Value = string.Empty;
            this.masterClaseServicio.Value = string.Empty;
            this.txtNro.Text = string.Empty;
            this.CleanHeader();
            this.EnableHeader(true);

            this._ctrl = null;
            this._numeroDoc = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu(); 
            this._rowTareaCurrent = new DTO_pyProyectoTarea();
            this._rowRecursoCurrent = new DTO_pyProyectoDeta();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasAdicion = new List<DTO_pyProyectoTarea>();
            this._listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
            this._tareasDeleted = new List<int>();
            this._dtoClaseProyecto = null;
            this._claseServicio = string.Empty;
            this._tareaIsFocused = false;
            this._totalRecxTarea = 0;
            this._totalPresupuesto = 0;
            this._totalCliente = 0;
            this._trabajoCurrent = string.Empty;
            this.gvRecursoCurrent = new GridView();
            this.gcDocument.DataSource = this._listTareasAll;
            this.gcDocument.RefreshDataSource();
            this.gvDocument.ActiveFilterString = string.Empty;
            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
            this.gcRecurso.Enabled = true;
            this.grpCtrlRecurso.Enabled = true;
            this.btnLoad.Enabled = true;

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr); 
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add); 
            FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import); 

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
                this.documentID = AppDocuments.PreProyecto;
                this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
                this.userCurrent = _bc.AdministrationModel.User.ID.Value;

                this.AddGridCols();
                this.InitControls();
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
                this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
                this.saveDelegate = new Save(this.SaveMethod);
                this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "SetInitParameters"));
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
                    #region Analisis Cliente(TareaCliente)
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "TareaCliente", true, false, false, null);
                    //validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "TareaCliente", false, true, false, AppMasters.pyTarea);
                    if (!validField)
                        validRow = false;
                    else
                    {
                        //int count = this._listTareasAll.Count(x => x.TareaCliente.Value == this._listTareasAll[fila].TareaCliente.Value);
                        //if (count > 1)
                        //{
                        //    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"];
                        //    string colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();
                        //    this.gvDocument.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_TareaClienteExist));
                        //    validRow = false;
                        //}
                        if (string.IsNullOrEmpty(this._rowTareaCurrent.TareaCliente.Value) && string.IsNullOrEmpty(this._rowTareaCurrent.TareaPadre.Value))
                        {
                            this._rowTareaCurrent.TareaCliente.Value = "tit";
                            this._rowTareaCurrent.DetalleInd.Value = false;
                            this._rowTareaCurrent.TituloPrintInd.Value = true;
                        }
                          
                        if (this.nivelMax < this._listTareasAll[fila].Nivel.Value)
                        {
                            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"];
                            string colVal = this.gvDocument.GetRowCellValue(this.gvDocument.FocusedRowHandle, col).ToString();
                            this.gvDocument.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, "El nivel maximo seleccionado no corresponde al digitado"));
                            validRow = false;
                        }
                    }
                    #endregion
                    #region UnidadInvID
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "UnidadInvID", !this._rowTareaCurrent.DetalleInd.Value.Value, true, false, AppMasters.inUnidad);
                    if (!validField)
                        validRow = false;

                    #endregion                    
                    #region Descriptivo
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "Descriptivo", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Cantidad
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "Cantidad", !this._rowTareaCurrent.DetalleInd.Value.Value, false, false, null);
                    //validField = _bc.ValidGridCellValue(this.gvDocument, this.unboundPrefix, fila, "Cantidad", !this._rowTareaCurrent.DetalleInd.Value.Value, false, false, false);
                    if (!validField)
                        validRow = false;
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
                camposObligatorios = camposObligatorios + this.lblResponableEmp.Text + "\n";
            if (string.IsNullOrEmpty(this.txtResposableCli.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblResponsableCli.Text) + "\n";
            if (string.IsNullOrEmpty(this.txtCorreo.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblCorreo.Text) + "\n";
            if (string.IsNullOrEmpty(this.txtTelefono.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblTelefono.Text) + "\n";
           //if (string.IsNullOrEmpty(this.uc_ListaPrecios.Value))
            //{
            //    camposObligatorios = camposObligatorios + this.uc_ListaPrecios.LabelRsx + "\n";
            //}
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
                    #region Validacion TareaCliente
                    if (string.IsNullOrWhiteSpace(dtoTarea.TareaCliente.Value) && this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
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
                    #region Validacion Unidad
                    if (string.IsNullOrWhiteSpace(dtoTarea.UnidadInvID.Value))
                    {
                        if(!string.IsNullOrWhiteSpace(dtoTarea.TareaCliente.Value))
                        {
                            //DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            //rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                            //rdF.Message = msgEmptyField;
                            //rd.DetailsFields.Add(rdF);
                        }                     
                    }
                    else
                    {
                        DTO_MasterBasic und = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, dtoTarea.UnidadInvID.Value, true);
                        if(und == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                            rdF.Message = string.Format(msgFkNotFound, dtoTarea.UnidadInvID.Value);
                            rd.DetailsFields.Add(rdF);
                        }
                    }
                    #endregion
                    #region Validacion Cantidad
                    if (string.IsNullOrWhiteSpace(dtoTarea.CantidadTarea.Value.ToString()))
                    {
                        //DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        //rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.SolicitudProyecto + "_CantidadTarea");
                        //rdF.Message = msgEmptyField;
                        //rd.DetailsFields.Add(rdF);
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
                    if (string.IsNullOrWhiteSpace(dtoRecurso.UnidadInvID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                        rdF.Message = string.Format(msgFkNotFound, dtoRecurso.UnidadInvID.Value); ;
                        rd.DetailsFields.Add(rdF);
                    }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "ValidateDataImport"));
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
                //Actualiza valores APU
                this.txtCostoAPU.EditValue = this._rowTareaCurrent.Detalle.Sum(x=>x.CostoLocalTOT.Value);
                this.txtCostoAIUxAPU.EditValue = this._rowTareaCurrent.VlrAIUxAPUAdmin.Value + this._rowTareaCurrent.VlrAIUxAPUImpr.Value + this._rowTareaCurrent.VlrAIUxAPUUtil.Value;
                this.txtCostoTotalAPU.EditValue = Math.Round(this._rowTareaCurrent.Detalle.Sum(x => x.CostoLocalTOT.Value.Value) + this._rowTareaCurrent.VlrAIUxAPUAdmin.Value.Value + 
                                                  this._rowTareaCurrent.VlrAIUxAPUImpr.Value.Value + this._rowTareaCurrent.VlrAIUxAPUUtil.Value.Value,0);
              
                //Actualiza valores generales
                this.txtCostoPresupuesto.EditValue = this._totalPresupuesto;
                this.txtCostoCliente.EditValue = this._totalCliente;
                if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) //2  Interno
                    this.txtCostoMult.EditValue = this._totalCliente;
                else // 1 Cliente
                {
                    decimal totalClienteMult = 0;
                    decimal porcMultiplicador = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture) / 100;
                    foreach (DTO_pyProyectoTarea tarea in this._listTareasAll.FindAll(x => x.DetalleInd.Value.Value))
                        totalClienteMult += Math.Round(tarea.CostoLocalCLI.Value.Value * porcMultiplicador, 0);
                    if(this._dtoClaseProyecto != null && !this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
                        this.txtCostoMult.EditValue = totalClienteMult;
                    else
                        this.txtCostoMult.EditValue = this._totalCliente;
                }
                decimal vlrUtilidad = Math.Round((this._totalPresupuesto * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture)) / 100,0);
                decimal vlrTotalBaseIVA = vlrUtilidad + Convert.ToDecimal(this.txtOtros.EditValue, CultureInfo.InvariantCulture);
                if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                    this.txtIVA.EditValue = Math.Round((vlrTotalBaseIVA * Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
                else if (this._dtoClaseProyecto != null)
                {
                    if (this.chkAPUIncluyeAIU.Checked)
                    {
                        this.txtIVA.EditValue = Math.Round(((this._totalCliente * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture) / 100) * Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
                        this.lblIVA.Text = "IVA(Cliente Util)";
                    }
                    else
                    {
                        this.txtIVA.EditValue = Math.Round((this._totalCliente * Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
                        this.lblIVA.Text = "IVA(Cliente)";
                    }
                }            

                //Actualiza info de la tarea
                this.masterUsuarioPermiso.Value = this._rowTareaCurrent.UsuarioID.Value;
                this.txtPorcDesc.EditValue = this._rowTareaCurrent.PorDescuento.Value;
                #region Asigna permiso de edicion
                if (this.userCurrent.Equals(this.masterResponsableEmp.Value))
                    this.chkUserEdit.Properties.ReadOnly = false;
                else
                    this.chkUserEdit.Properties.ReadOnly = true;

                if (!this.masterUsuarioPermiso.Value.Equals(this.userCurrent))
                {
                    //this.grpCtrlRecurso.Enabled = false;
                    //this.gcRecurso.Enabled = false;
                }
                else
                {
                    //this.grpCtrlRecurso.Enabled = true;
                    //this.gcRecurso.Enabled = true;
                } 
                #endregion
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvDetalle_FocusedRowChanged"));
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
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemImport.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;              
                FormProvider.Master.itemGenerateTemplate.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {               
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr); 
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemSendtoAppr.ToolTipText = this._bc.GetResource(LanguageTypes.ToolBar, "acc_approve");
                    FormProvider.Master.itemUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "Form_FormClosed"));
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

                    this.dtFechaInicio.DateTime = new DateTime(currentYear, currentMonth, minDay);
                    this.dtFechaInicio.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //this.AsignarTasaCambio(false);
                //EstadoPeriodo validPeriod = _bc.AdministrationModel.CheckPeriod(this.dtPeriod.DateTime, this.frmModule);
                //if (this.dtPeriod.Enabled && validPeriod != EstadoPeriodo.Abierto)
                //{
                //    if (validPeriod == EstadoPeriodo.Cerrado)
                //        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoCerrado));
                //    if (validPeriod == EstadoPeriodo.EnCierre)
                //        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoEnCierre));

                //    this.dtPeriod.Focus();
                //}
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosProyecto.cs-dtFecha_EditValueChanged"));
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
                ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, false);
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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "btnQueryDoc_Click"));
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
                string prefijo = this._bc.GetPrefijo(this.masterAreaFuncional.Value, this.documentID);
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
                    this._dtoClaseProyecto = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, this.masterClaseServicio.Value, true);
                    this._claseServicio = this.masterClaseServicio.Value;
                    this._listRecursosXTareaAll = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, string.Empty, string.Empty, null, false,false);
                    this.cmbTipoPresup.EditValue = this._dtoClaseProyecto.TipoPresupuesto.Value.ToString();                    
                }
                else
                    this._claseServicio = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosProyecto.cs-masterClaseServicio_Leave"));
            }
        }

        /// <summary>
        /// Carga detalla inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadInitial_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.masterClaseServicio.ValidID)
                {
                    this._rowTareaCurrent = new DTO_pyProyectoTarea();
                    this._tareaIsFocused = false;
                    this._listTareasAll = this._bc.AdministrationModel.pyProyectoTarea_Get(null, string.Empty, this.masterClaseServicio.Value);
                    this._listTareasAll = this._listTareasAll.FindAll(x => !x.Descriptivo.Value.Equals("CONSECUTIVO")).ToList();
                    if (this._listRecursosXTareaAll.Count == 0)
                        this._listRecursosXTareaAll = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, string.Empty, this.masterClaseServicio.Value, null, false,false);
                    #region Convierte Dolares en Pesos
                    foreach (var det in this._listRecursosXTareaAll)
                    {
                        if (det.CostoExtra.Value != 0 && det.CostoLocal.Value == 0 && this._tasaCambio != 0)
                            det.CostoLocal.Value = det.CostoExtra.Value * this._tasaCambio;
                    }
                    #endregion

                    foreach (var tarea in this._listTareasAll)
                    {
                        tarea.UsuarioID.Value = this.userCurrent;
                        this.CalculateHierarchy(tarea, tarea.CapituloTareaID.Value);                        
                    }
                    foreach (var tarea in _listTareasAll)
                        this.CalculateValues(tarea);
                    this.UpdateValues();
                    //this._listTareasAll = this._listTareasAll.OrderBy(x => Convert.ToInt32(x.CapituloTareaID.Value.Substring()).ToList();
                    this.gcDocument.DataSource = this._listTareasAll;
                }
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterClaseServicio.LabelRsx));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosProyecto.cs-btnLoadInitial_Click"));
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
                if (this.masterPrefijo.ValidID)
                    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosProyecto.cs-txtNro_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAPUIncluyeAIU_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var tarea in this._listTareasAll)
                {
                    this.CalculateValues(tarea);
                    this.UpdateValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto", "chkAPUIncluyeAIU_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbJerarquia_EditValueChanged(object sender, EventArgs e)
        {
            this.nivelMax = Convert.ToInt16(this.cmbJerarquia.EditValue);
        }

        /// <summary>
        /// Al hacer clic para importar los recursos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnImportRecursos_Click(object sender, EventArgs e)
        {
            try
            {
                //Carga los recursos
                this._listRecursosXTareaAll = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, string.Empty, this.masterClaseServicio.Value, null, false,false);
                #region Convierte Dolares en Pesos
                foreach (var det in this._listRecursosXTareaAll)
                {
                    if (det.CostoExtra.Value != 0 && det.CostoLocal.Value == 0 && this._tasaCambio != 0)
                        det.CostoLocal.Value = det.CostoExtra.Value * this._tasaCambio;
                }
                #endregion
                //Variables
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();
                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatRecursos);

                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error   
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgInvalidFormat = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    DTO_ExportRecursosDeta recurso = null;
                    List<DTO_ExportRecursosDeta> recursosList = new List<DTO_ExportRecursosDeta>();
                    bool createDTO = true;
                    bool validList = true;

                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatRecursos.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_ExportRecursosDeta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, AppForms.MasterReportXls + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            recurso = new DTO_ExportRecursosDeta();
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = recurso.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(recurso, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
                                                if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                                {
                                                    try
                                                    {
                                                        DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                                {
                                                    try
                                                    {
                                                        decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                        if (colValue.Trim().Contains(','))
                                                        {
                                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                            rdF.Field = colRsx;
                                                            rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                            rd.DetailsFields.Add(rdF);

                                                            createDTO = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PlaneacionCostosProyecto.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                recursosList.Add(recurso);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();
                        DTO_pyRecurso dtoRecurso = new DTO_pyRecurso();
                        int i = 0;
                        percent = 0;
                        for (int index = 0; index < recursosList.Count; ++index)
                        {
                            recurso = recursosList[index];
                            #region Variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Message = "OK";
                            #endregion
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (recursosList.Count);
                            i++;
                            #endregion
                            #region Valida y asigna datos finales
                            this.ValidateDataImport(null, recurso, rd, msgFkNotFound, msgEmptyField);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                            }
                            #endregion
                        }
                        if (result.Details.Count > 0)
                            result.Result = ResultValue.NOK;
                        else
                        {
                            //this._rowTareaCurrent.Detalle = new List<DTO_pyProyectoDeta>();
                            #region Agrega los recursos a las tareas en Masivo
                            foreach (DTO_pyProyectoTarea tarea in this._listTareasAll)
                            {
                                //Validad si la tarea tiene recursos importados
                                if (recursosList.Exists(x => x.TareaCliente.Value == tarea.TareaCliente.Value && x.TareaID.Value == tarea.TareaID.Value))
                                {
                                    i = 0;
                                    foreach (DTO_ExportRecursosDeta r in recursosList.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value && x.TareaID.Value == tarea.TareaID.Value))
                                    {
                                        #region Agrega el recurso
                                        DTO_pyProyectoDeta det = new DTO_pyProyectoDeta();
                                        det.TareaID.Value = !string.IsNullOrEmpty(r.TareaID.Value) ? r.TareaID.Value : this._tareaXDef;
                                        det.TrabajoID.Value = !string.IsNullOrEmpty(r.TareaID.Value) ? r.TareaID.Value : this._trabajoXDef;
                                        det.RecursoID.Value = r.RecursoID.Value;
                                        #region Obtiene info del Recurso
                                        dtoRecurso = (DTO_pyRecurso)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, false, det.RecursoID.Value, true);
                                        if (dtoRecurso != null)
                                        {
                                            det.RecursoDesc.Value = dtoRecurso.Descriptivo.Value;
                                            det.UnidadInvID.Value = dtoRecurso.UnidadInvID.Value;
                                            det.TipoRecurso.Value = dtoRecurso.TipoRecurso.Value;
                                        }
                                        else
                                        {
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                                            rd.line = i + 1;
                                            rd.Message = "El recurso " + det.RecursoID.Value + " no existe";
                                            result.Details.Add(rd);
                                        }
                                        #endregion                                          
                                        det.FactorID.Value = r.FactorID.Value;
                                        det.Cantidad.Value = 1;
                                        det.CantSolicitud.Value = 1;
                                        if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal || det.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                                        {
                                            det.Peso_Cantidad.Value = r.Peso_Cantidad.Value != null? r.Peso_Cantidad.Value : 1;
                                            det.Distancia_Turnos.Value = r.Distancia_Turnos.Value != null? r.Distancia_Turnos.Value : 1;
                                        }                                      
                                         //Trae el costo del recurso
                                        var recExist = this._listRecursosXTareaAll.Find(x => x.RecursoID.Value == det.RecursoID.Value);
                                        if (r.CostoLocal.Value == null || r.CostoLocal.Value == 0)
                                            det.CostoLocal.Value = recExist != null ? recExist.CostoLocal.Value : 0;
                                        else
                                            det.CostoLocal.Value = r.CostoLocal.Value;
                                        det.CostoExtra.Value = recExist != null ? recExist.CostoExtra.Value : 0;

                                        DTO_pyProyectoDeta exist = tarea.Detalle.Find(x => x.RecursoID.Value == r.RecursoID.Value);
                                        if (exist != null)
                                        {
                                            det.Consecutivo.Value = exist.Consecutivo.Value;
                                            det.ConsecTarea.Value = exist.ConsecTarea.Value;
                                            det.NumeroDoc.Value = exist.NumeroDoc.Value;
                                        }

                                        tarea.Detalle.RemoveAll(x => x.RecursoID.Value == r.RecursoID.Value && r.TareaCliente.Value == tarea.TareaCliente.Value);
                                        tarea.Detalle.Add(det);
                                        i++;
                                        #endregion
                                    }
                                    if (result.Details.Count > 0)
                                    {
                                        tarea.Detalle = new List<DTO_pyProyectoDeta>();
                                        result.Result = ResultValue.NOK;
                                    }
                                }
                            }
                            #endregion
                            this._tareaIsFocused = false;
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(this.refreshGridDelegate);

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        recursosList = new List<DTO_ExportRecursosDeta>();
                    }
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "ImportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Al hacer clic para exportar los recursos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnExportRecursos_Click(object sender, EventArgs e)
        {
            try
            {
                this._tareaIsFocused = false;
                if (this._tareaIsFocused)
                {
                    List<DTO_pyProyectoDeta> data = this._rowTareaCurrent.Detalle;
                    List<DTO_ExportRecursosDeta> list = new List<DTO_ExportRecursosDeta>();
                    foreach (DTO_pyProyectoDeta d in data)
                    {
                        DTO_ExportRecursosDeta deta = new DTO_ExportRecursosDeta();
                        deta.TareaID.Value = this._rowTareaCurrent.TareaID.Value;
                        deta.TareaCliente.Value = this._rowTareaCurrent.TareaCliente.Value;
                        deta.Descripcion.Value = this._rowTareaCurrent.Descriptivo.Value;
                        deta.RecursoID.Value = d.RecursoID.Value;
                        deta.RecursoDesc.Value = d.RecursoDesc.Value;
                        deta.UnidadInvID.Value = d.UnidadInvID.Value;
                        deta.CostoLocal.Value = d.CostoLocal.Value;
                        deta.FactorID.Value = d.FactorID.Value;                      
                        list.Add(deta);
                    }
                    DataTableOperations tableOp = new DataTableOperations();
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportRecursosDeta), list);
                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
                else
                {
                    List<DTO_ExportRecursosDeta> list = new List<DTO_ExportRecursosDeta>();
                    foreach (DTO_pyProyectoTarea tarea in this._listTareasAll)
                    {
                        List<DTO_pyProyectoDeta> data = tarea.Detalle;
                        foreach (DTO_pyProyectoDeta d in data)
                        {
                            DTO_ExportRecursosDeta deta = new DTO_ExportRecursosDeta();
                            deta.TareaID.Value = tarea.TareaID.Value;
                           // if (this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                               deta.TareaCliente.Value = tarea.TareaCliente.Value;
                            //else
                            //   deta.TareaCliente.Value = tarea.TareaID.Value;                           
                            deta.Descripcion.Value = tarea.Descriptivo.Value;
                            deta.RecursoID.Value = d.RecursoID.Value;
                            deta.RecursoDesc.Value = d.RecursoDesc.Value;
                            deta.UnidadInvID.Value = d.UnidadInvID.Value;
                            deta.CostoLocal.Value = d.CostoLocal.Value;                      
                            deta.FactorID.Value = d.FactorID.Value;
                            list.Add(deta);
                        }
                    }
                    DataTableOperations tableOp = new DataTableOperations();
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportRecursosDeta), list);
                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "btnExportRecursos"));
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtOtros_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.UpdateValues();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVlrAdicional_Click(object sender, EventArgs e)
        {
            try
            {
                //ModalTareasAdicional modal = new ModalTareasAdicional(this._listTareasAdicion);
                //modal.ShowDialog();
                //this._listTareasAdicion = modal.TareasAdic;
                //if (this._listTareasAdicion.Count > 0)
                //    this.txtOtros.EditValue = this._listTareasAdicion.Sum(x => x.CostoTotalML.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Permite cambiar el % para multiplicar el presupuesto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMultiplicador_EditValueChanged(object sender, EventArgs e)
        {           
            this.porcIncrementoMult = Convert.ToDecimal(this.txtMultiplicador.EditValue,CultureInfo.InvariantCulture);
            if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
            {
                foreach (var tarea in this._listTareasAll)
                {
                    if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                        tarea.CostoLocalUnitCLI.Value = this.GetValorCliente(tarea);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            //Construccion
            else if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
            {
                foreach (var tarea in this._listTareasAll)
                {
                    tarea.PorDescuento.Value = tarea.PorDescuento.Value ?? 0;
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                        tarea.CostoLocalUnitCLI.Value = this.GetValorCliente(tarea);
                    tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            else //Cliente
                this.UpdateValues();
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
                 MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "txt_MouseHover: " + ex.Message));
            }  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUserEdit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkUserEdit.Checked)
                {
                    this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 2;
                    this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].VisibleIndex = 3;

                    this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "Cantidad"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalUnitML"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalML"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalCLI"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoDiferenciaML"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "ImprimirTareaInd"].Visible = false;
                    this.gvDocument.Columns[this.unboundPrefix + "TituloPrintInd"].Visible = false;

                }
                else
                {
                    this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 2;
                    this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvDocument.Columns[this.unboundPrefix + "Cantidad"].VisibleIndex =4;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalUnitML"].VisibleIndex = 5;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalML"].VisibleIndex = 6;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].VisibleIndex = 7;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalCLI"].VisibleIndex = 8;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoDiferenciaML"].VisibleIndex = 9;
                    this.gvDocument.Columns[this.unboundPrefix + "ImprimirTareaInd"].VisibleIndex = 10;
                    this.gvDocument.Columns[this.unboundPrefix + "TituloPrintInd"].VisibleIndex = 11;

                    this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "Cantidad"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalUnitML"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoTotalML"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoLocalCLI"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "CostoDiferenciaML"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "ImprimirTareaInd"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "TituloPrintInd"].Visible = true;
                    this.gvDocument.Columns[this.unboundPrefix + "UsuarioID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "chkUserEdit_CheckedChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoPresup_EditValueChanged(object sender, EventArgs e)
        {
            if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
            {
                this.btnLoad.Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].UnGroup();
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = false;
                this.txtMultiplicador.Visible = true;
                this.lblMultipl.Visible = true;
                this.lbl.Visible = true;
                this.btnVersion.Visible = false;
            }
            else if (this._dtoClaseProyecto != null) // Interno
            {
                this.btnLoad.Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortOrder = ColumnSortOrder.None;
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = true;
                this.txtMultiplicador.Visible = false;
                this.lblMultipl.Visible = false;
                this.lbl.Visible = false;
                this.btnVersion.Visible = true;
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtTasaCambio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._tasaCambio = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                foreach (var t in this._listTareasAll)
                {
                    foreach (var rec in t.Detalle.FindAll(x => x.CostoExtra.Value != 0 && x.CostoExtra.Value != null))
                        rec.CostoLocal.Value = this._tasaCambio * rec.CostoExtra.Value;
                }
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "txtTasaCambio_EditValueChanged"));                
            }
        }

        /// <summary>
        /// Al entrar al control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnUpdateCosto_Click(object sender, EventArgs e)
        {
            try
            {
                List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
                List<DTO_pyProyectoMvto> movs = new List<DTO_pyProyectoMvto>();

                if (this._listTareasAll.Count > 0)
                {
                    List<DTO_pyProyectoDeta> detalle = new List<DTO_pyProyectoDeta>();
                    foreach (var tarea in this._listTareasAll)
                        detalle.AddRange(tarea.Detalle.ToList());

                    List<string> recursosDistinct = detalle.Select(x => x.RecursoID.Value.ToString()).Distinct().ToList();
                    foreach (string rec in recursosDistinct)
                    {
                        DTO_pyProyectoDeta det = detalle.Find(x => x.RecursoID.Value == rec);
                        DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto();
                        decimal cantSUM = 0;
                        decimal costoSUM = 0;
                        string tareasInsumo = string.Empty;
                        mvto.RecursoID.Value = rec;
                        mvto.RecursoDesc.Value = det.RecursoDesc.Value;
                        mvto.UnidadInvID.Value = det.UnidadInvID.Value;
                        mvto.TipoRecurso.Value = det.TipoRecurso.Value;
                        foreach (var r in detalle.FindAll(x => x.RecursoID.Value == rec))
                        {
                            decimal cantTarea = this._listTareasAll.Find(x => x.Consecutivo.Value == r.ConsecTarea.Value).Cantidad.Value.Value;
                            if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                            {
                                if (!this.chkEquipoCant.Checked)
                                    cantSUM += r.FactorID.Value != 0 ? Math.Round((cantTarea / r.FactorID.Value.Value), 2) : 0;
                                else
                                    cantSUM += r.FactorID.Value != 0 ? Math.Round((cantTarea * r.FactorID.Value.Value), 2) : 0;
                                costoSUM += Math.Round((r.CostoLocalTOT.Value.Value), 0) * cantTarea;
                            }
                            else if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            {
                                if (!this.chkEquipoCant.Checked)
                                    cantSUM += r.FactorID.Value != 0 ? Math.Round((cantTarea / r.FactorID.Value.Value), 2) : 0;
                                else
                                    cantSUM += r.FactorID.Value != 0 ? Math.Round((cantTarea * r.FactorID.Value.Value), 2) : 0;
                                costoSUM += Math.Round((r.CostoLocalTOT.Value.Value), 0) * cantTarea;
                            }
                            else
                            {
                                cantSUM += Math.Round((cantTarea * r.FactorID.Value.Value), 2);
                                costoSUM += Math.Round((r.CostoLocalTOT.Value.Value), 0) * cantTarea;
                            }
                            tareasInsumo += this._listTareasAll.Find(x => x.Consecutivo.Value == r.ConsecTarea.Value.Value).TareaCliente.Value + "-";
                        }
                        tareasInsumo = tareasInsumo.Substring(0, tareasInsumo.Length - 1);
                        mvto.RecursoDesc.Value += "(" + tareasInsumo + ")";
                        mvto.CostoLocal.Value = det.CostoLocal.Value;
                        mvto.CostoTotalML.Value = costoSUM;
                        mvto.CantidadSUM.Value = cantSUM;
                        if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                        {
                            decimal total = detalle.FindAll(x => x.RecursoID.Value == rec).Sum(x => x.CostoLocal.Value.Value);
                            decimal cantPers = detalle.FindAll(x => x.RecursoID.Value == rec && x.Peso_Cantidad.Value != null).Sum(x => x.Peso_Cantidad.Value.Value);
                            mvto.CostoLocal.Value = cantPers != 0 && cantPers != null ? total / cantPers : 0;
                            mvto.CantidadSUM.Value = mvto.CostoLocal.Value != 0 ? costoSUM / mvto.CostoLocal.Value : 0;
                        }
                        else if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                            mvto.CantidadSUM.Value = mvto.CostoLocal.Value != 0 ? costoSUM / mvto.CostoLocal.Value : 0;

                        movs.Add(mvto);
                    }
                }
                movs = movs.OrderBy(x => x.RecursoDesc.Value).ToList();
                decimal AIU = this._proyectoDocu.PorEmpresaUTI.Value.Value + this._proyectoDocu.PorEmpresaADM.Value.Value + this._proyectoDocu.PorEmpresaIMP.Value.Value;
                ModalCostosInsumo modal = new ModalCostosInsumo(movs, AIU);
                modal.ShowDialog();
                if (!modal.isCancelSelected)
                {
                    foreach (var costoNew in modal.recursosModif)
                    {
                        foreach (var tarea in this._listTareasAll)
                        {
                            DTO_pyProyectoDeta rec = tarea.Detalle.Find(x => x.RecursoID.Value == costoNew.Key);
                            if (rec != null)
                                rec.CostoLocal.Value = costoNew.Value;
                        }
                    }
                }
                this._tareaIsFocused = false;
                this.AssignRecursos();
                this.isValid = true;
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /// <summary>
        /// Al entrar al control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnVersion_Click(object sender, EventArgs e)
        {
            try
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                if (MessageBox.Show("Desea crear una nueva versión del documento y editarlo?", "Editar Proyecto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this._versionCotizacion++;
                    this.btnVersion.Text = "Versión Actual: " + this._versionCotizacion.ToString();
                    this.btnVersion.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    this.gcDocument.Enabled = true;
                    this.gcRecurso.Enabled = true;
                    //this.chkMultipActivo.Enabled = true;
                }

            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Al entrar al control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtPorcDesc_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._rowTareaCurrent.PorDescuento.Value = Convert.ToDecimal(this.txtPorcDesc.EditValue);
                this._tareaIsFocused = true;
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Al cambiar el valor al control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbRedondeo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "cmbRedondeo_EditValueChanged"));
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPersonalCant_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "chkPersonalCant_CheckedChanged"));
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkEquipoCant_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this._tareaIsFocused = false;
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "chkEquipoCant_CheckedChanged"));
            }
        }

        /// <summary>
        /// Activa el multiplcicador para calcular el valor del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMultipActivo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.porcIncrementoMult = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture);

                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                if (this._proyectoDocu != null)
                    this._proyectoDocu.MultiplicadorActivoInd.Value =  this.chkMultipActivo.Checked;
                if (this.chkMultipActivo.Checked)
                {
                    if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
                    {
                        if (MessageBox.Show("Desea cambiar los valores ya fijados para el cliente?", "Editar Proyecto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach (var tarea in this._listTareasAll)
                            {
                                tarea.PorDescuento.Value = tarea.PorDescuento.Value ?? 0;
                                if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                                    tarea.CostoLocalUnitCLI.Value = this.GetValorCliente(tarea);
                                tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                                tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                            }

                            this._tareaIsFocused = false;
                            this.AssignRecursos();
                        }
                        else
                        {
                            this.chkMultipActivo.Checked = false;
                            this._proyectoDocu.MultiplicadorActivoInd.Value = false;
                        }
                    }
                    //Construccion
                    else if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
                    {
                        if (MessageBox.Show("Desea cambiar los valores ya fijados para el cliente?", "Editar Proyecto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach (var tarea in this._listTareasAll)
                            {
                                tarea.PorDescuento.Value = tarea.PorDescuento.Value ?? 0;
                                if (this._proyectoDocu != null && this._proyectoDocu.MultiplicadorActivoInd.Value.Value)
                                    tarea.CostoLocalUnitCLI.Value = this.GetValorCliente(tarea);
                                tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                                tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                            }

                            this._tareaIsFocused = false;
                            this.AssignRecursos();
                        }
                        else
                        {
                            this.chkMultipActivo.Checked = false;
                            this._proyectoDocu.MultiplicadorActivoInd.Value = false;
                        }
                    }
                    else //Cliente
                        this.UpdateValues();
                }
            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "chkMultipActivo_CheckedChanged"));
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSortByCap_Click(object sender, EventArgs e)
        {
            try
            {
                this._listTareasAll = this._listTareasAll.OrderBy(x => x.CapituloTareaID.Value).ToList();
                int count = 1;
                foreach (var t in this._listTareasAll)
                {
                    t.TareaCliente.Value = count.ToString();
                    count++;
                }
                this.gcDocument.DataSource = this._listTareasAll;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "txtSortByCap_Click"));
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
        private  void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowTareaCurrent = (DTO_pyProyectoTarea)this.gvDocument.GetRow(e.FocusedRowHandle);
                    this._tareaIsFocused = true;
                    this.AssignRecursos();
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true;
                    if (this.userCurrent == this.userEditValues)
                        this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].OptionsColumn.AllowEdit = true;
                    else
                    {
                        if(this._rowTareaCurrent.NumeroDoc.Value.HasValue)
                           this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].OptionsColumn.AllowEdit = false;
                        else
                           this.gvDocument.Columns[this.unboundPrefix + "CostoLocalUnitCLI"].OptionsColumn.AllowEdit = true;

                    }
                }
                else
                {
                    this.gcRecurso.DataSource = null;
                    this.gcRecurso.RefreshDataSource();
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
                }
                if (this.gvDocument.DataRowCount > 0)
                    this.gvDocument.OptionsView.ShowAutoFilterRow = true;
                else
                    this.gvDocument.OptionsView.ShowAutoFilterRow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (!this.masterClaseServicio.ValidID)
                {
                    MessageBox.Show("No existe una Clase de servicio digitada");
                    return;
                }

                #region Agregar

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDocument.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                        {
                            this.AddNewRowTarea();
                        }
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            if (isV)
                            {
                                this.AddNewRowTarea();
                            }
                        }
                    }
                }
                #endregion

                #region Eliminar
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    e.Handled = true;
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDocument.FocusedRowHandle;

                        if (this._listTareasAll.Count > 0)
                        {
                            List<DTO_pyProyectoTarea> hijosTarea = this._listTareasAll.FindAll(x => x.TareaPadre.Value == this._rowTareaCurrent.TareaCliente.Value).ToList();
                            if (hijosTarea.Count > 0)
                            {
                                if (MessageBox.Show("Esta tarea tiene niveles de jerarquía dependientes, desea eliminarla?", msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    this._listTareasAll.RemoveAll(x => x.TareaCliente.Value == this._rowTareaCurrent.TareaCliente.Value &&
                                                              x.Descriptivo.Value == this._rowTareaCurrent.Descriptivo.Value);
                                }
                            }
                            else 
                            {
                                this._listTareasAll.RemoveAll(x => x.TareaCliente.Value == this._rowTareaCurrent.TareaCliente.Value &&
                                                                x.Descriptivo.Value == this._rowTareaCurrent.Descriptivo.Value);
                            }

                            //Asigna las tareas a eliminar de bd
                            if (this._rowTareaCurrent.Consecutivo.Value != null && !this._tareasDeleted.Exists(x => x == this._rowTareaCurrent.Consecutivo.Value.Value))
                                this._tareasDeleted.Add(this._rowTareaCurrent.Consecutivo.Value.Value);

                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            //this.CalculateValues(this._rowTareaCurrent);
                            this.UpdateValues(); 
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];

                this.gvDocument = (GridView)sender;
                this._rowTareaCurrent = (DTO_pyProyectoTarea)gvDocument.GetRow(e.RowHandle);
                if (fieldName == "TareaID")
                {
                    this._tareaIsFocused = true;
                    DTO_pyTarea dtoTarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, e.Value.ToString(), true);
                    if (dtoTarea != null)
                    {
                        this._rowTareaCurrent.Descriptivo.Value = dtoTarea.Descriptivo.Value;
                        this._rowTareaCurrent.UnidadInvID.Value = dtoTarea.UnidadInvID.Value;

                        if (this._dtoClaseProyecto.TareaAsociadaInd.Value.Value)
                        {
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("TareaID", dtoTarea.ID.Value);
                            pks.Add("ClaseServicioID", this._claseServicio);
                            DTO_pyTareaClase dtoTareaClase = (DTO_pyTareaClase)this._bc.GetMasterComplexDTO(AppMasters.pyTareaClase, pks, true);
                            if (dtoTareaClase != null)
                            {
                                #region Obtiene APUs
                                //Trae APUs parametrizados
                                var apuNews = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, dtoTarea.ID.Value, this._claseServicio, null, false, false, this._tasaCambio);
                                apuNews.ForEach(x =>
                                {
                                    //Valida si ya existe para asignar valores
                                    if (this._rowTareaCurrent.Detalle.Exists(y => y.RecursoID.Value == x.RecursoID.Value))
                                    {
                                        x.Consecutivo.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).Consecutivo.Value;
                                        x.ConsecTarea.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).ConsecTarea.Value;
                                        x.NumeroDoc.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).NumeroDoc.Value;
                                        x.TrabajoID.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).TrabajoID.Value;
                                    }
                                });

                                //Valida si borra APUs existentes
                                foreach (var apuOld in this._rowTareaCurrent.Detalle)
                                {
                                    if (apuOld.Consecutivo.Value != null && !apuNews.Exists(x => x.Consecutivo.Value == apuOld.Consecutivo.Value))
                                        if (!this._proyectoDocu.RecursosDeleted.Exists(x => x == apuOld.Consecutivo.Value))
                                            this._proyectoDocu.RecursosDeleted.Add(apuOld.Consecutivo.Value.Value);
                                }
                                #endregion
                                this._rowTareaCurrent.Detalle = apuNews;
                                if (this._rowTareaCurrent.Detalle.Count > 0) this._rowTareaCurrent.DetalleInd.Value = true;
                                this.AssignRecursos();
                            }
                            //this._rowTareaCurrent.TareaID.Value = dtoTareaClase.TareaID.Value;                         
                        }
                        else
                        {
                            #region Obtiene APUs
                            //Trae APUs parametrizados
                            var apuNews = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, dtoTarea.ID.Value, string.Empty, null, false, false, this._tasaCambio);
                            apuNews.ForEach(x =>
                            {
                                //Valida si ya existe para asignar valores
                                if (this._rowTareaCurrent.Detalle.Exists(y => y.RecursoID.Value == x.RecursoID.Value))
                                {
                                    x.Consecutivo.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).Consecutivo.Value;
                                    x.ConsecTarea.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).ConsecTarea.Value;
                                    x.NumeroDoc.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).NumeroDoc.Value;
                                    x.TrabajoID.Value = this._rowTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).TrabajoID.Value;
                                }
                            });

                            //Valida si borra APUs existentes
                            foreach (var apuOld in this._rowTareaCurrent.Detalle)
                            {
                                if (apuOld.Consecutivo.Value != null && !apuNews.Exists(x => x.Consecutivo.Value == apuOld.Consecutivo.Value))
                                    if (!this._proyectoDocu.RecursosDeleted.Exists(x => x == apuOld.Consecutivo.Value))
                                        this._proyectoDocu.RecursosDeleted.Add(apuOld.Consecutivo.Value.Value);
                            }
                            #endregion
                            this._rowTareaCurrent.Detalle = apuNews;
                            if (this._rowTareaCurrent.Detalle.Count > 0) this._rowTareaCurrent.DetalleInd.Value = true;
                            this.AssignRecursos();
                        }
                    }
                    else
                        this._rowTareaCurrent.Descriptivo.Value = string.Empty;
                }
                if (fieldName == "Cantidad")
                {
                    this._rowTareaCurrent.Cantidad.Value = e.Value != null? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();               
                }
                else if (fieldName == "CostoTotalUnitML")
                {

                    this._rowTareaCurrent.CostoTotalML.Value = e.Value != null ? this._rowTareaCurrent.Cantidad.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "CostoTotalML")
                {
                    this._rowTareaCurrent.CostoTotalML.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalUnitCLI")
                {
                    this._rowTareaCurrent.CostoLocalCLI.Value = e.Value != null ? this._rowTareaCurrent.Cantidad.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowTareaCurrent);
                    this.gvDocument.RefreshData();
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalCLI")
                {
                    this._rowTareaCurrent.CostoLocalCLI.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.gvDocument.RefreshData();
                    this.UpdateValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvDocument_CellValueChanged"));
            }           
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            try
            {
                if (fieldName == "TareaCliente")
                {
                    if(!e.Value.ToString().EndsWith("."))
                        this.CalculateHierarchy(this._rowTareaCurrent, e.Value.ToString());                   
                }        
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvDocument_CellValueChanging"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e) 
        {
            try
            {
                if (!this.deleteOP && this.gvDocument.DataRowCount > 0)
                    this.ValidateRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "CostoDiferenciaML" && e.RowHandle >= 0)
            {

                decimal cellvalue = Convert.ToDecimal(e.CellValue, CultureInfo.InvariantCulture);
                if (cellvalue >= 0)
                    e.Appearance.ForeColor = Color.Black;
                else
                    e.Appearance.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvDocument.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                   if(currentRow.DetalleInd.Value.Value)
                        e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))); 
                    else
                        e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); 
                }
            }
            catch (Exception ex)
            {                
                throw ex;
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
                if (e.Column.FieldName == this.unboundPrefix + "CapituloTareaID" && e.IsForGroupRow)
                {
                    string capituloDesc = this._listTareasAll.Find(x => x.CapituloTareaID.Value == e.Value.ToString()).CapituloDesc.Value;
                    e.DisplayText = e.Value.ToString() + "  " + capituloDesc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvRecurso_CustomColumnDisplayText"));
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
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                if (colName.Equals("TareaID"))
                {
                    ModalTareasFilter mod = new ModalTareasFilter(false);
                    mod.ShowDialog();
                    this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, this.unboundPrefix + "TareaID", mod.TareaSelected);
                }
                else
                {
                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "editBtnGrid_ButtonClick"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "editLink_Click"));
            }
        }

        #endregion        

        #region Recurso-Trabajo
        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcRecurso_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this._rowTareaCurrent.TareaCliente.Value))
                {
                    MessageBox.Show("No existe una tarea o ítem digitado");
                    return;
                }

                if (this._rowTareaCurrent != null)
                {
                    #region Agregar

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        if (this.gvRecurso.ActiveFilterString != string.Empty)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                        else
                        {
                            this.deleteOP = false;
                            if (this.isValid)
                                this.AddNewRowRecurso();
                            else
                            {
                                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                if (isV)
                                    this.AddNewRowRecurso();
                            }
                        }
                    }
                    #endregion

                    #region Eliminar
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        e.Handled = true;
                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.deleteOP = true;
                            int rowHandle = this.gvRecurso.FocusedRowHandle;

                            if (this._rowTareaCurrent.Detalle.Count >= 1)
                            {
                                //Acumula los recursos que se borraran
                                if (this._rowRecursoCurrent.Consecutivo.Value != null && !this._proyectoDocu.RecursosDeleted.Exists(x => x == this._rowRecursoCurrent.Consecutivo.Value))
                                    this._proyectoDocu.RecursosDeleted.Add(this._rowRecursoCurrent.Consecutivo.Value.Value);

                                this._rowTareaCurrent.Detalle.RemoveAll(x => x.RecursoID.Value == this._rowRecursoCurrent.RecursoID.Value &&
                                                                            x.TareaID.Value == this._rowRecursoCurrent.TareaID.Value);
                                this.gcRecurso.DataSource = this._rowTareaCurrent.Detalle;
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvRecurso.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvRecurso.FocusedRowHandle = rowHandle - 1;

                                

                                this.CalculateValues(this._rowTareaCurrent);
                                this.UpdateValues();
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gcDetalle_EmbeddedNavigator_ButtonClick"));
            }
        }

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

                //this._rowTareaCurrent = (DTO_pyProyectoTarea)gvDocument.GetRow(gvDocument.FocusedRowHandle);
                if (fieldName == "RecursoID")
                {
                    DTO_pyRecurso rec = (DTO_pyRecurso)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, false, e.Value.ToString(), true);
                    if (rec != null)
                    {
                        this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "RecursoDesc", rec.Descriptivo.Value);
                        this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "UnidadInvID", rec.UnidadInvID.Value);
                    }
                    this.gvRecurso.RefreshData();
                }
                else if (fieldName == "CantSolicitud")
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "FactorID")
                {
                    if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte || this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        //decimal pesoOrCantObrer = Convert.ToDecimal(this.txtVolPeso.EditValue, CultureInfo.InvariantCulture);
                        //decimal vlrDistanOrCantTurno = Convert.ToDecimal(this.txtDistanciaTurno.EditValue, CultureInfo.InvariantCulture);
                        //if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                        //    this._rowRecursoCurrent.FactorID.Value = pesoOrCantObrer * vlrDistanOrCantTurno;

                        //else if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                        //{
                        //    this._rowRecursoCurrent.CostoLocal.Value = this._rowRecursoCurrent.CostoLocal.Value * pesoOrCantObrer * (_porcPrestManoObra / 100);
                        //    this._rowRecursoCurrent.FactorID.Value = this._rowRecursoCurrent.FactorID.Value * vlrDistanOrCantTurno;
                        //}   
                    }
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();                       
                }
                else if (fieldName == "CostoLocal")
                {
                    if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte || this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        //decimal pesoOrCantObrer = Convert.ToDecimal(this.txtVolPeso.EditValue, CultureInfo.InvariantCulture);
                        //decimal vlrDistanOrCantTurno = Convert.ToDecimal(this.txtDistanciaTurno.EditValue, CultureInfo.InvariantCulture);
                        //if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                        //    this._rowRecursoCurrent.FactorID.Value = pesoOrCantObrer * vlrDistanOrCantTurno;

                        //else if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                        //{
                        //    this._rowRecursoCurrent.CostoLocal.Value = this._rowRecursoCurrent.CostoLocal.Value * pesoOrCantObrer * (_porcPrestManoObra / 100);
                        //    this._rowRecursoCurrent.FactorID.Value = this._rowRecursoCurrent.FactorID.Value * vlrDistanOrCantTurno;
                        //}
                    }
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();
                }
                
            }
            catch (Exception ex )
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvRecurso_CellValueChanged"));
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
               
                if (e.FocusedRowHandle >= 0 && this.gvRecurso.DataRowCount > 0)
                {
                    var trabajo = this.gvRecurso.GetRowCellValue(e.FocusedRowHandle, this.unboundPrefix + "TrabajoID");
                    this._trabajoCurrent = trabajo != null ? trabajo.ToString() : string.Empty;
                    this._rowRecursoCurrent = (DTO_pyProyectoDeta)this.gvRecurso.GetRow(e.FocusedRowHandle);
                    if (this._rowRecursoCurrent != null)
                    {
                        this.txtModelo.EditValue = this._rowRecursoCurrent.Modelo.Value;
                        this.txtMarca.EditValue = this._rowRecursoCurrent.MarcaDesc.Value;
                        if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal || this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                        {
                            this.txtDistanciaTurno.EditValue = this._rowRecursoCurrent.Distancia_Turnos.Value != null ? this._rowRecursoCurrent.Distancia_Turnos.Value : 1;
                            this.txtPesoCant.EditValue = this._rowRecursoCurrent.Peso_Cantidad.Value != null ? this._rowRecursoCurrent.Peso_Cantidad.Value : 1;
                        }
                        else
                        {
                            this.txtDistanciaTurno.EditValue = 0;
                            this.txtPesoCant.EditValue = 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvRecurso_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Antes de dejar la fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (this._rowRecursoCurrent != null && this._rowRecursoCurrent.FactorID.Value > 999999)
            { 
                GridColumn col = this.gvRecurso.Columns[this.unboundPrefix + "FactorID"];
                this.gvRecurso.SetColumnError(col, "Cantidad o Rend. Inválido");
                e.Allow = false;
            }
            else
                e.Allow = true;
        }

        #endregion

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            string camposObligatorios = this.ValidateHeader();
            if (string.IsNullOrEmpty(camposObligatorios))
            {
                this.CargarInformacion(false);
                Thread process = new Thread(this.SaveThread);
                process.Start();
                this.gvDocument.ActiveFilterString = string.Empty;   
            }
            else
                MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios));
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
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
            string msgAprobar = string.Format(msgDoc, "Aprobar");

            if (MessageBox.Show(msgAprobar, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string camposObligatorios = string.Empty;
                if (!this.masterCliente.ValidID)
                    camposObligatorios = this.masterCliente.LabelRsx + "\n";
                if (!this.masterCentroCto.ValidID)
                    camposObligatorios = camposObligatorios + this.masterCentroCto.LabelRsx + "\n";
                if (!this.masterProyecto.ValidID)
                    camposObligatorios = camposObligatorios + this.masterProyecto.LabelRsx + "\n";

                if(string.IsNullOrEmpty(camposObligatorios))
                {
                    this._proyectoDocu.Version.Value = this._versionCotizacion;
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }      
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios));
                }
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBImport()
        {
            if (this.masterClaseServicio.ValidID)
            {
                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatTareas);
                Thread process = new Thread(this.ImportThreadTareas);
                process.Start();
            }
            else
            {
                MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterClaseServicio.LabelRsx));
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.formatTareas.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                throw ex;
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
                        ex.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "TBExport"));
            }
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint() 
        {
            try
            {
                this.CargarInformacion(false);
                List<DTO_pyPreProyectoTarea> tareasPre = new List<DTO_pyPreProyectoTarea>();
                List<DTO_pyPreProyectoTarea> tareasPreAdic = new List<DTO_pyPreProyectoTarea>();
                DTO_pyPreProyectoDocu docuPre = new DTO_pyPreProyectoDocu();

                foreach (var tarea in this._listTareasAll)
                {
                    #region Copia Tareas
                    DTO_pyPreProyectoTarea tareaTemp = new DTO_pyPreProyectoTarea();
                    tareaTemp.Index = tarea.Index.Value;
                    tareaTemp.NumeroDoc.Value = tarea.NumeroDoc.Value;
                    tareaTemp.Consecutivo.Value = tarea.Consecutivo.Value;
                    tareaTemp.Cantidad.Value = tarea.Cantidad.Value;
                    tareaTemp.TareaCliente.Value = tarea.TareaCliente.Value;
                    tareaTemp.TareaID.Value = tarea.TareaID.Value;
                    tareaTemp.CantidadTarea.Value = tarea.CantidadTarea.Value;
                    tareaTemp.CapituloDesc.Value = tarea.CapituloDesc.Value;
                    tareaTemp.CapituloGrupoID.Value = tarea.CapituloGrupoID.Value;
                    tareaTemp.CapituloTareaID.Value = tarea.CapituloTareaID.Value;
                    tareaTemp.CentroCostoID.Value = tarea.CentroCostoID.Value;
                    tareaTemp.CostoAdicionalInd.Value = tarea.CostoAdicionalInd.Value;
                    tareaTemp.CostoDiferenciaML.Value = tarea.CostoDiferenciaML.Value;
                    tareaTemp.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value;
                    tareaTemp.CostoExtraCLI.Value = tarea.CostoExtraCLI.Value;
                    tareaTemp.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value;
                    tareaTemp.CostoTotalML.Value = tarea.CostoTotalML.Value;
                    tareaTemp.CostoTotalME.Value = tarea.CostoTotalME.Value;
                    tareaTemp.CostoTotalUnitML.Value = tarea.CostoTotalUnitML.Value;
                    tareaTemp.CostoTotalUnitME.Value = tarea.CostoTotalUnitME.Value;
                    tareaTemp.Descriptivo.Value = tarea.Descriptivo.Value;
                    tareaTemp.DetalleInd.Value = tarea.DetalleInd.Value;
                    tareaTemp.FechaInicio.Value = tarea.FechaInicio.Value;
                    tareaTemp.FechaFin.Value = tarea.FechaFin.Value;
                    tareaTemp.ImprimirTareaInd.Value = tarea.ImprimirTareaInd.Value;
                    tareaTemp.LineaPresupuestoID.Value = tarea.LineaPresupuestoID.Value;
                    tareaTemp.Nivel.Value = tarea.Nivel.Value;
                    tareaTemp.Observacion.Value = tarea.Observacion.Value;
                    tareaTemp.PorDescuento.Value = tarea.PorDescuento.Value;
                    tareaTemp.TareaPadre.Value = tarea.TareaPadre.Value;
                    tareaTemp.TituloPrintInd.Value = tarea.TituloPrintInd.Value;
                    tareaTemp.TrabajoID.Value = tarea.TrabajoID.Value;
                    tareaTemp.UnidadInvID.Value = tarea.UnidadInvID.Value;
                    tareaTemp.UsuarioID.Value = tarea.UsuarioID.Value;
                    tareaTemp.VlrAIUxAPUAdmin.Value = tarea.VlrAIUxAPUAdmin.Value;
                    tareaTemp.VlrAIUxAPUImpr.Value = tarea.VlrAIUxAPUImpr.Value;
                    tareaTemp.VlrAIUxAPUUtil.Value = tarea.VlrAIUxAPUUtil.Value;
                    tareaTemp.VlrAIUxAPUIVA.Value = tarea.VlrAIUxAPUIVA.Value;
                    foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                    {
                        #region Copia Recursos
                        DTO_pyPreProyectoDeta detTmp = new DTO_pyPreProyectoDeta();
                        detTmp.Consecutivo.Value = det.Consecutivo.Value;
                        detTmp.ConsecTarea.Value = det.ConsecTarea.Value;
                        detTmp.NumeroDoc.Value = det.NumeroDoc.Value;
                        detTmp.RecursoDesc.Value = det.RecursoDesc.Value;
                        detTmp.RecursoID.Value = det.RecursoID.Value;
                        detTmp.TipoRecurso.Value = det.TipoRecurso.Value;
                        detTmp.UnidadInvID.Value = det.UnidadInvID.Value;
                        detTmp.FactorID.Value = det.FactorID.Value;
                        detTmp.Cantidad.Value = det.Cantidad.Value;
                        detTmp.CantidadTOT.Value = det.CantidadTOT.Value;
                        detTmp.CostoLocal.Value = det.CostoLocal.Value;
                        detTmp.CostoLocalInicial.Value = det.CostoLocalInicial.Value;
                        detTmp.CostoLocConPrestacion.Value = det.CostoLocConPrestacion.Value;
                        detTmp.CostoLocalTOT.Value = det.CostoLocalTOT.Value;
                        detTmp.CostoLocalEMP.Value = det.CostoLocalEMP.Value;
                        detTmp.CostoLocalPRY.Value = det.CostoLocalPRY.Value;
                        detTmp.CostoExtra.Value = det.CostoExtra.Value;
                        detTmp.CostoExtraEMP.Value = det.CostoExtraEMP.Value;
                        detTmp.CostoExtraPRY.Value = det.CostoExtraPRY.Value;
                        detTmp.CostoExtraTOT.Value = det.CostoExtraTOT.Value;
                        detTmp.CantSolicitud.Value = det.CantSolicitud.Value;
                        detTmp.Distancia_Turnos.Value = det.Distancia_Turnos.Value;
                        detTmp.Peso_Cantidad.Value = det.Peso_Cantidad.Value;
                        detTmp.DocCompra.Value = det.DocCompra.Value;
                        detTmp.FechaFijadaInd.Value = det.FechaFijadaInd.Value;
                        detTmp.FechaFin.Value = det.FechaFin.Value;
                        detTmp.FechaInicio.Value = det.FechaInicio.Value;
                        detTmp.FechaInicioAUT.Value = det.FechaInicioAUT.Value;
                        detTmp.MarcaDesc.Value = det.MarcaDesc.Value;
                        detTmp.Modelo.Value = det.Modelo.Value;
                        detTmp.Observaciones.Value = det.Observaciones.Value;
                        detTmp.PorVariacion.Value = det.PorVariacion.Value;
                        detTmp.ProveedorID.Value = det.ProveedorID.Value;
                        detTmp.SelectInd.Value = det.SelectInd.Value;
                        detTmp.TareaID.Value = det.TareaID.Value;
                        detTmp.TiempoTotal.Value = det.TiempoTotal.Value;
                        detTmp.TrabajoID.Value = det.TrabajoID.Value;
                        detTmp.VlrAIUAdmin.Value = det.VlrAIUAdmin.Value;
                        detTmp.VlrAIUImpr.Value = det.VlrAIUImpr.Value;
                        detTmp.VlrAIUUtil.Value = det.VlrAIUUtil.Value;
                        detTmp.AjCambioExtra.Value = det.AjCambioExtra.Value;
                        detTmp.AjCambioLocal.Value = det.AjCambioLocal.Value;
                        tareaTemp.Detalle.Add(detTmp);
                        #endregion
                    }
                    tareasPre.Add(tareaTemp);      
                    #endregion              
                }

                #region Copia Docu
                docuPre.ClaseServicioID.Value = this._proyectoDocu.ClaseServicioID.Value;
                docuPre.ClienteID.Value = this._proyectoDocu.ClienteID.Value;
                docuPre.ClienteDesc.Value = this._proyectoDocu.ClienteDesc.Value;
                docuPre.EmpresaNombre.Value = this._proyectoDocu.EmpresaNombre.Value;
                docuPre.Jerarquia.Value = this._proyectoDocu.Jerarquia.Value;
                docuPre.APUIncluyeAIUInd.Value = this._proyectoDocu.APUIncluyeAIUInd.Value;
                docuPre.RecursosXTrabajoInd.Value = this._proyectoDocu.RecursosXTrabajoInd.Value;
                docuPre.Licitacion.Value = this._proyectoDocu.Licitacion.Value;
                docuPre.DescripcionSOL.Value = this._proyectoDocu.DescripcionSOL.Value;
                docuPre.PorClienteADM.Value = this._proyectoDocu.PorClienteADM.Value;
                docuPre.PorClienteIMP.Value = this._proyectoDocu.PorClienteIMP.Value;
                docuPre.PorClienteUTI.Value = this._proyectoDocu.PorClienteUTI.Value;
                docuPre.PorEmpresaADM.Value = this._proyectoDocu.PorEmpresaADM.Value;
                docuPre.PorEmpresaIMP.Value = this._proyectoDocu.PorEmpresaIMP.Value;
                docuPre.PorEmpresaUTI.Value = this._proyectoDocu.PorEmpresaUTI.Value;
                docuPre.PorMultiplicadorPresup.Value = this._proyectoDocu.PorMultiplicadorPresup.Value;
                docuPre.PorIVA.Value = this._proyectoDocu.PorIVA.Value;
                docuPre.ResponsableCLI.Value = this._proyectoDocu.ResponsableCLI.Value;
                docuPre.ResponsableCorreo.Value = this._proyectoDocu.ResponsableCorreo.Value;
                docuPre.ResponsableEMP.Value = this._proyectoDocu.ResponsableEMP.Value;
                docuPre.ResponsableTelefono.Value = this._proyectoDocu.ResponsableTelefono.Value;
                docuPre.TasaCambio.Value = this._proyectoDocu.TasaCambio.Value;
                docuPre.Valor.Value = this._proyectoDocu.Valor.Value;
                docuPre.ValorCliente.Value = this._proyectoDocu.ValorCliente.Value;
                docuPre.ValorIVA.Value = this._proyectoDocu.ValorIVA.Value;
                docuPre.ValorOtros.Value = this._proyectoDocu.ValorOtros.Value;
                docuPre.Version.Value = this._proyectoDocu.Version.Value;
                docuPre.EquipoCantidadInd.Value = this._proyectoDocu.EquipoCantidadInd.Value;
                docuPre.PersonalCantidadInd.Value = this._proyectoDocu.PersonalCantidadInd.Value;
                docuPre.MultiplicadorActivoInd.Value = this._proyectoDocu.MultiplicadorActivoInd.Value;
                docuPre.TipoRedondeo.Value = this._proyectoDocu.TipoRedondeo.Value;
                docuPre.MonedaPresupuesto.Value = this._proyectoDocu.MonedaPresupuesto.Value;
                #endregion

                DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
                solicitud.Detalle = tareasPre;
                solicitud.DetalleTareasAdic = tareasPreAdic;
                solicitud.DocCtrl = this._ctrl;
                solicitud.Header = docuPre;
                string reportName = this._bc.AdministrationModel.Reportes_py_PlaneacionCostos(solicitud,false, Convert.ToByte(this.cmbReporte.EditValue), null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "TBPrint"));
            }
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if(this.masterProyecto.ValidID)
                    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "TBUpdate"));
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

                if (this._proyectoDocu.NumeroDoc.Value != null && this._proyectoDocu.NumeroDoc.Value != 0)
                    this._numeroDoc = this._proyectoDocu.NumeroDoc.Value.Value;

                DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                transaccion.HeaderProyecto = ObjectCopier.Clone(this._proyectoDocu);
                transaccion.HeaderProyecto.TareasDeleted = this._tareasDeleted;
                transaccion.DetalleProyecto = this._listTareasAll;
                transaccion.DetalleProyectoTareaAdic = this._listTareasAdicion;             

                result = this._bc.AdministrationModel.SolicitudProyecto_Add(AppDocuments.Proyecto,ref this._numeroDoc,
                         this.masterClaseServicio.Value, this.masterAreaFuncional.Value,this.masterPrefijo.Value,
                         this.masterProyecto.Value, this.masterCentroCto.Value, this.txtObservaciones.Text, transaccion);

                if (result.Result == ResultValue.OK)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SuccessSol), this.masterPrefijo.Value, result.ExtraField));
                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto;
                    this._listTareasAdicion = transaccion.DetalleProyectoTareaAdic;
                    this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc);
                    this._tareasDeleted = new List<int>();
                    this._proyectoDocu.RecursosDeleted = new List<int>();
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
                // this.Invoke(this.sendToApproveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "SaveThread"));
            }
            finally
            {
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

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;
                if (this._numeroDoc == 0)
                    result = resultNOK;
                else
                {
                    DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                    transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                    transaccion.HeaderProyecto = ObjectCopier.Clone(this._proyectoDocu);
                    transaccion.DetalleProyecto = this._listTareasAll;
                    transaccion.DetalleProyectoTareaAdic = this._listTareasAdicion;
                    if (transaccion.DocCtrl != null)
                        transaccion.DocCtrl.ProyectoID.Value = this.masterProyecto.Value;
                    result = _bc.AdministrationModel.SolicitudProyecto_AprobarProy(AppDocuments.Proyecto, transaccion,this.dtFechaInicio.DateTime);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.deleteOP = true;
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosProyecto.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadTareas()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            List<DTO_pyProyectoTarea> listImport = new List<DTO_pyProyectoTarea>();
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    //Lista con los dtos a subir y Fks a validas                    
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);


                    //Popiedades
                    DTO_pyProyectoTarea tarea = new DTO_pyProyectoTarea();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatTareas.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_pyProyectoTarea).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto.ToString() + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    //Fks
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            tarea = new DTO_pyProyectoTarea();
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = tarea.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(tarea, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
                                                if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                                {
                                                    try
                                                    {
                                                        DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                                {
                                                    try
                                                    {
                                                        decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                        if (colValue.Trim().Contains(','))
                                                        {
                                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                            rdF.Field = colRsx;
                                                            rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                            rd.DetailsFields.Add(rdF);

                                                            createDTO = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PlaneacionCostosProyecto.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                listImport.Add(tarea);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la migracion
                    if (validList)
                    {
                        #region Variables generales
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        //percent = 0;

                        #endregion
                        foreach (DTO_pyProyectoTarea dto in listImport)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            //this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            //percent = ((i + 1) * 100) / (listImport.Count);
                            i++;

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;
                            #endregion
                            #region Validaciones particulares del DTO
                            this.ValidateDataImport(dto, null, rd, msgFkNotFound, msgEmptyField);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                            }
                            #endregion
                        }
                        if (result.Details.Count > 0)
                        {
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }

                    }
                    #endregion
                    if (validList)
                    {
                        List<DTO_pyProyectoTarea> tareasTmp = ObjectCopier.Clone(this._listTareasAll);
                        this._listTareasAll = new List<DTO_pyProyectoTarea>();
                        int indexTarea = 0;
                        int indexTareaID = 10000;
                        if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                        {
                            foreach (var tar in listImport)
                            {
                                tar.Index = indexTarea;
                                tar.TrabajoID.Value = string.Empty;
                                tar.TareaID.Value = indexTareaID.ToString();
                                tar.CostoTotalML.Value = 0;
                                tar.CostoTotalUnitML.Value = 0;
                                tar.Cantidad.Value = tar.CantidadTarea.Value != null ? tar.CantidadTarea.Value : 0;
                                tar.CostoLocalCLI.Value = tar.Cantidad.Value * (tar.CostoLocalUnitCLI.Value != null ? tar.CostoLocalUnitCLI.Value : 0);
                                tar.CostoDiferenciaML.Value = tar.CostoLocalCLI.Value - tar.CostoTotalML.Value;
                                tar.UsuarioID.Value = this.userCurrent;
                                tar.Consecutivo.Value = tareasTmp.Exists(x => x.TareaCliente.Value == tar.TareaCliente.Value && x.Descriptivo.Value == tar.Descriptivo.Value) ?
                                                                         tareasTmp.Find(x => x.TareaCliente.Value == tar.TareaCliente.Value && x.Descriptivo.Value == tar.Descriptivo.Value).Consecutivo.Value : null;
                                tar.TareaCliente.Value = string.IsNullOrEmpty(tar.TareaCliente.Value) ? "tit" : tar.TareaCliente.Value;
                                this.CalculateHierarchy(tar, tar.TareaCliente.Value);
                                this._listTareasAll.Add(tar);
                                indexTarea++;
                                indexTareaID++;
                            } 
                        }
                        else
                        {
                            foreach (var tar in listImport)
                            {
                                tar.Index = indexTarea;
                                tar.TrabajoID.Value = this._trabajoXDef;
                                tar.TareaID.Value = tar.TareaCliente.Value;
                                tar.TareaCliente.Value = (indexTarea + 1).ToString();
                                tar.Cantidad.Value = tar.CantidadTarea.Value != null ? tar.CantidadTarea.Value : 0;                              
                                tar.UsuarioID.Value = this.userCurrent;
                                tar.Consecutivo.Value = tareasTmp.Exists(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value) ?
                                                                         tareasTmp.Find(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value).Consecutivo.Value : null;
                                tar.Detalle = this._bc.AdministrationModel.pyProyectoDeta_GetByParameter(this.documentID, tar.TareaID.Value, this._claseServicio, null, false,false);
                                tar.CostoTotalML.Value = 0;
                                tar.CostoTotalUnitML.Value = 0;
                                tar.CostoLocalCLI.Value = tar.Cantidad.Value * (tar.CostoLocalUnitCLI.Value != null ? tar.CostoLocalUnitCLI.Value : 0);
                                tar.CostoDiferenciaML.Value = tar.CostoLocalCLI.Value - tar.CostoTotalML.Value;
                                this.CalculateHierarchy(tar, tar.TareaCliente.Value);
                                this._listTareasAll.Add(tar);
                                indexTarea++;
                                indexTareaID++;
                            } 
                        }
                    }
                }                
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = this.pasteRet.MsgResult;
                }
                #region Actualiza la información de la grilla
                if (result.Result == ResultValue.OK)
                {
                    MessageForm frm = new MessageForm(result);
                    if (result.Result.Equals(ResultValue.OK))
                        this.Invoke(this.endImportarDelegate);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    listImport = new List<DTO_pyProyectoTarea>();
                }
                FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosProyecto.cs", "ImportThreadTareas"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);               
            }            
        }

        #endregion                                                   

        private void txtNro_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
