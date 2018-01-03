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
    public partial class PlaneacionCostosAPUCliente : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string userCurrent = string.Empty;
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string areaFuncionalID;
        private string prefijoID;
        private bool isValid = true;
        private bool deleteOP = false;
        private PasteOpDTO pasteRet;
        //Variables para importar
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
        private DTO_glDocumentoControl _ctrl = null;
        #region PreProyecto
        private DTO_pyPreProyectoDocu _preProyectoDocu = new DTO_pyPreProyectoDocu();
        private DTO_pyPreProyectoTarea _rowPreTareaCurrent = new DTO_pyPreProyectoTarea();
        private DTO_pyPreProyectoDeta _rowPreRecursoCurrent = new DTO_pyPreProyectoDeta();
        private List<DTO_pyPreProyectoTarea> _listPreTareasAll = new List<DTO_pyPreProyectoTarea>();
        private List<DTO_pyPreProyectoTarea> _listPreTareasAdicion = new List<DTO_pyPreProyectoTarea>();
        private List<DTO_pyPreProyectoDeta> _listPreRecursosXTareaAll = new List<DTO_pyPreProyectoDeta>(); 
        #endregion
        private DTO_pyClaseProyecto _dtoClaseProyecto = null;
        private string _tareaXDef = string.Empty;
        private string _trabajoXDef = string.Empty;
        private decimal _tasaCambio = 0;
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

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.gcDocument.DataSource = this._listPreTareasAll;
            this.gcDocument.RefreshDataSource();
            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public PlaneacionCostosAPUCliente()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "PlaneacionCostosAPUCliente"));
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
            tareaCliente.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tareaCliente.AppearanceCell.Options.UseTextOptions = true;
            tareaCliente.AppearanceCell.Options.UseFont = true;
            tareaCliente.VisibleIndex = 1;
            tareaCliente.Width = 42;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(tareaCliente);

            GridColumn tareaID = new GridColumn();
            tareaID.FieldName = this.unboundPrefix + "TareaID";
            tareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            tareaID.UnboundType = UnboundColumnType.String;
            tareaID.VisibleIndex = 2;
            tareaID.Width = 40;
            tareaID.Visible = false;
            tareaID.ColumnEdit = this.editBtnGrid;
            tareaID.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(tareaID);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 3;
            descriptivo.Width = 350;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 4;
            UnidadInvID.Width = 30;
            UnidadInvID.Visible = true;
            UnidadInvID.ColumnEdit = this.editBtnGrid;
            UnidadInvID.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(UnidadInvID);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 5;
            Cantidad.Width = 35;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(Cantidad);

            GridColumn CostoTotalUnitML = new GridColumn();
            CostoTotalUnitML.FieldName = this.unboundPrefix + "CostoTotalUnitML";
            CostoTotalUnitML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOTUnit");
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
            CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
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
            CostoLocalUnitCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalUnitCLI");
            CostoLocalUnitCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalUnitCLI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoLocalUnitCLI.AppearanceCell.Options.UseTextOptions = true;
            CostoLocalUnitCLI.VisibleIndex = 8;
            CostoLocalUnitCLI.Width = 50;
            CostoLocalUnitCLI.Visible = true;
            CostoLocalUnitCLI.ColumnEdit = this.editSpin;
            CostoLocalUnitCLI.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(CostoLocalUnitCLI);

            GridColumn CostoLocalCLI = new GridColumn();
            CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
            CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalCLI");
            CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalCLI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoLocalCLI.AppearanceCell.Options.UseTextOptions = true;
            CostoLocalCLI.VisibleIndex = 9;
            CostoLocalCLI.Width = 80;
            CostoLocalCLI.Visible = true;
            CostoLocalCLI.ColumnEdit = this.editSpin;
            CostoLocalCLI.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(CostoLocalCLI);

            GridColumn CostoDiferencia = new GridColumn();
            CostoDiferencia.FieldName = this.unboundPrefix + "CostoDiferenciaML";
            CostoDiferencia.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoDiferencia");
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
            ImprimirTareaInd.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_ImprimirTareaInd");
            ImprimirTareaInd.UnboundType = UnboundColumnType.Boolean;
            ImprimirTareaInd.VisibleIndex = 11;
            ImprimirTareaInd.Width = 20;
            ImprimirTareaInd.Visible = true;
            ImprimirTareaInd.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(ImprimirTareaInd);

            GridColumn TituloPrintInd = new GridColumn();
            TituloPrintInd.FieldName = this.unboundPrefix + "TituloPrintInd";
            TituloPrintInd.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TituloPrintInd");
            TituloPrintInd.UnboundType = UnboundColumnType.Boolean;
            TituloPrintInd.VisibleIndex = 12;
            TituloPrintInd.Width = 20;
            TituloPrintInd.Visible = true;
            TituloPrintInd.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(TituloPrintInd);

            GridColumn UsuarioID = new GridColumn();
            UsuarioID.FieldName = this.unboundPrefix + "UsuarioID";
            UsuarioID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UsuarioID");
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
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 80;
            RecursoID.Visible = true;        
            RecursoID.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 200;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDrec = new GridColumn();
            UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDrec.UnboundType = UnboundColumnType.String;
            UnidadInvIDrec.VisibleIndex = 2;
            UnidadInvIDrec.Width = 70;
            UnidadInvIDrec.Visible = true;
            UnidadInvIDrec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDrec);

            GridColumn TrabajoID = new GridColumn();
            TrabajoID.FieldName = this.unboundPrefix + "TrabajoID";
            TrabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TrabajoID");
            TrabajoID.UnboundType = UnboundColumnType.String;
            TrabajoID.VisibleIndex = 3;
            TrabajoID.Width = 120;
            TrabajoID.Visible = false;
            TrabajoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(TrabajoID);

            //Cant x 
            GridColumn FactorID = new GridColumn();
            FactorID.FieldName = this.unboundPrefix + "FactorID";
            FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
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
            CantidadRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
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
            CanSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
            CanSolicitud.UnboundType = UnboundColumnType.Decimal;
            CanSolicitud.VisibleIndex = 7;
            CanSolicitud.Width = 80;
            CanSolicitud.Visible = false;
            CanSolicitud.ColumnEdit = this.editValue2Cant;
            CanSolicitud.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(CanSolicitud);

            GridColumn CantidadTOT = new GridColumn();
            CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTOT");
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
            CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
            CostoLocal.UnboundType = UnboundColumnType.Decimal;
            CostoLocal.VisibleIndex =9;
            CostoLocal.Width = 80;
            CostoLocal.Visible = true;
            CostoLocal.ColumnEdit = this.editSpin;
            CostoLocal.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(CostoLocal);

            GridColumn CostoLocalTOT = new GridColumn();
            CostoLocalTOT.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
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
            CostoTotalMLRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
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
            RecursoIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoIDDet.UnboundType = UnboundColumnType.String;
            RecursoIDDet.VisibleIndex = 0;
            RecursoIDDet.Width = 80;
            RecursoIDDet.Visible = true;
            RecursoIDDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(RecursoIDDet);

            GridColumn RecursoDescet = new GridColumn();
            RecursoDescet.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDescet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDescet.UnboundType = UnboundColumnType.String;
            RecursoDescet.VisibleIndex = 1;
            RecursoDescet.Width = 200;
            RecursoDescet.Visible = true;
            RecursoDescet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(RecursoDescet);

            GridColumn UnidadInvIDDet = new GridColumn();
            UnidadInvIDDet.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDDet.UnboundType = UnboundColumnType.Integer;
            UnidadInvIDDet.VisibleIndex = 2;
            UnidadInvIDDet.Width = 80;
            UnidadInvIDDet.Visible = true;
            UnidadInvIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(UnidadInvIDDet);

            GridColumn TrabajoIDDet = new GridColumn();
            TrabajoIDDet.FieldName = this.unboundPrefix + "TrabajoID";
            TrabajoIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TrabajoID");
            TrabajoIDDet.UnboundType = UnboundColumnType.String;
            TrabajoIDDet.VisibleIndex = 3;
            TrabajoIDDet.Width = 120;
            TrabajoIDDet.Visible = false;
            TrabajoIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(TrabajoIDDet);

            GridColumn FactorIDDet = new GridColumn();
            FactorIDDet.FieldName = this.unboundPrefix + "FactorID";
            FactorIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
            FactorIDDet.UnboundType = UnboundColumnType.Decimal;
            FactorIDDet.VisibleIndex = 5;
            FactorIDDet.Width = 80;
            FactorIDDet.Visible = true;
            FactorIDDet.OptionsColumn.AllowEdit = true;
            FactorIDDet.ColumnEdit = this.editValue8Cant;
            this.gvDetalleRecurso.Columns.Add(FactorIDDet);

            GridColumn CantidadDet = new GridColumn();
            CantidadDet.FieldName = this.unboundPrefix + "Cantidad";
            CantidadDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
            CantidadDet.UnboundType = UnboundColumnType.Decimal;
            CantidadDet.VisibleIndex = 6;
            CantidadDet.Width = 80;
            CantidadDet.Visible = false;
            CantidadDet.OptionsColumn.AllowEdit = false;
            CantidadDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantidadDet);

            GridColumn CantSolicitudDet = new GridColumn();
            CantSolicitudDet.FieldName = this.unboundPrefix + "CantSolicitud";
            CantSolicitudDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
            CantSolicitudDet.UnboundType = UnboundColumnType.Decimal;
            CantSolicitudDet.VisibleIndex = 7;
            CantSolicitudDet.Width = 80;
            CantSolicitudDet.Visible = false;
            CantSolicitudDet.OptionsColumn.AllowEdit = false;
            CantSolicitudDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantSolicitudDet);

            GridColumn CantidadTOTDet = new GridColumn();
            CantidadTOTDet.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTOT");
            CantidadTOTDet.UnboundType = UnboundColumnType.Decimal;
            CantidadTOTDet.VisibleIndex = 8;
            CantidadTOTDet.Width = 80;
            CantidadTOTDet.Visible = false;
            CantidadTOTDet.OptionsColumn.AllowEdit = false;
            CantidadTOTDet.ColumnEdit = this.editValue2Cant;
            this.gvDetalleRecurso.Columns.Add(CantidadTOTDet);

            GridColumn CostoLocalDet = new GridColumn();
            CostoLocalDet.FieldName = this.unboundPrefix + "CostoLocal";
            CostoLocalDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
            CostoLocalDet.UnboundType = UnboundColumnType.Decimal;
            CostoLocalDet.VisibleIndex = 9;
            CostoLocalDet.Width = 80;
            CostoLocalDet.Visible = true;
            CostoLocalDet.ColumnEdit = this.editSpin;
            CostoLocalDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(CostoLocalDet);

            GridColumn CostoLocalTOTDet = new GridColumn();
            CostoLocalTOTDet.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
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
                byte redondeo = Convert.ToByte(this.cmbRedondeo.EditValue);
                ModalAPUProyectos modal = new ModalAPUProyectos(this._rowPreTareaCurrent, this.chkAPUIncluyeAIU.Checked, porcAdmin, porcImpr, porcUtil,
                                                                this._tasaCambio,redondeo,this.chkEquipoCant.Checked,this.chkPersonalCant.Checked, (Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture) / 100), true);
                modal.ShowDialog();
                this._rowPreTareaCurrent.DetalleAPUCliente = modal.ListSelected;
                if (this._rowPreTareaCurrent.DetalleAPUCliente.Count > 0)
                {
                    this._rowPreTareaCurrent.DetalleInd.Value = true;
                    this._rowPreTareaCurrent.TituloPrintInd.Value = false;
                }                   
                else
                    this._rowPreTareaCurrent.DetalleInd.Value = this.nivelMax == this._rowPreTareaCurrent.Nivel.Value? true : false;
                foreach (var rec in this._rowPreTareaCurrent.DetalleAPUCliente)
                {
                    rec.TareaID.Value = this._rowPreTareaCurrent.TareaID.Value;
                    rec.TrabajoID.Value = string.IsNullOrEmpty(rec.TrabajoID.Value) ? this._trabajoCurrent : rec.TrabajoID.Value;
                    rec.Cantidad.Value = 1;
                    rec.CantSolicitud.Value = 1;
                }
                this.AssignRecursos();

                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowTarea()
        {
            try
            {
                DTO_pyPreProyectoTarea footerDet = new DTO_pyPreProyectoTarea();

                #region Asigna datos a la fila
                try
                {
                    if (this._listPreTareasAll.Count > 0)
                    {
                        decimal indexTareaID = Convert.ToDecimal(this._listPreTareasAll.Last().TareaID.Value,CultureInfo.InvariantCulture)+1;
                        footerDet.Index = this._listPreTareasAll.Last().Index + 1;
                        footerDet.TareaCliente.Value = this._listPreTareasAll.Last().TareaCliente.Value;
                        footerDet.TareaID.Value = indexTareaID.ToString(); 
                        footerDet.Descriptivo.Value = this._listPreTareasAll.Last().Descriptivo.Value;
                        footerDet.UnidadInvID.Value = this._listPreTareasAll.Last().UnidadInvID.Value;
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
                    this._listPreTareasAll.Add(footerDet);
                    this._rowPreTareaCurrent = footerDet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
                this.gcDocument.DataSource = this._listPreTareasAll;
                this.gcDocument.RefreshDataSource();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "AddNewRow: " + ex.Message));
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
                    if (this.chkRecursoXTrabInd.Checked)
                    {
                        #region Recursos X Trabajo
                        List<string> trabajos = this._rowPreTareaCurrent.DetalleAPUCliente.Select(x => x.TrabajoID.Value.ToString()).Distinct().ToList();
                        List<DTO_pyPreProyectoTarea> listTarea = new List<DTO_pyPreProyectoTarea>();
                        foreach (var t in trabajos)
                        {
                            DTO_pyPreProyectoTarea tarea = new DTO_pyPreProyectoTarea();
                            tarea.TareaID.Value = this._rowPreTareaCurrent.TareaID.Value;
                            tarea.Descriptivo.Value = this._rowPreTareaCurrent.Descriptivo.Value;
                            tarea.TrabajoID.Value = t;
                            tarea.UnidadInvID.Value = this._rowPreTareaCurrent.UnidadInvID.Value;
                            tarea.Cantidad.Value = 1;
                            tarea.DetalleAPUCliente = this._rowPreTareaCurrent.DetalleAPUCliente.FindAll(x => x.TrabajoID.Value == t).ToList();
                            listTarea.Add(tarea);
                        }

                        foreach (var t in listTarea)
                            this.CalculateValues(t);

                        if (listTarea.Count > 0)
                        {
                            this._rowPreTareaCurrent.CostoTotalML.Value = listTarea.Sum(x => x.CostoTotalML.Value);
                            //this._rowCapituloCurrent.Detalle[this._rowTareaCurrent.Index].CostoTotalML.Value = this._rowTareaCurrent.CostoTotalML.Value;
                        }

                        #region Reasigna Detalle
                        this._rowPreTareaCurrent.DetalleAPUCliente = new List<DTO_pyPreProyectoDeta>();
                        foreach (var t in listTarea)
                            this._rowPreTareaCurrent.DetalleAPUCliente.AddRange(t.DetalleAPUCliente);
                        this._totalRecxTarea = this._rowPreTareaCurrent.CostoTotalML.Value.Value;
                        #endregion

                        this.gcRecurso.DataSource = listTarea;
                        #endregion
                    }
                    else
                    {
                        #region Recursos por Tarea
                        if (this._rowPreTareaCurrent.DetalleAPUCliente.Count > 0)
                        {
                            this.CalculateValues(this._rowPreTareaCurrent);
                        }

                        if (this.gvRecurso.DataRowCount > 0)
                            this.gvRecurso.FocusedRowHandle = 0;
                        this.gcRecurso.DataSource = this._rowPreTareaCurrent.DetalleAPUCliente;
                        #endregion
                    }

                    this.gcRecurso.RefreshDataSource();
                    this.gvRecurso.MoveFirst();

                    this.gvDocument.RefreshRow(this._rowPreTareaCurrent.Index);
                    this.gvDocument.RefreshData();
                }
                else
                {
                    foreach (var tarea in this._listPreTareasAll)
                        this.CalculateValues(tarea);
                }
                this.UpdateValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        private bool AsignarTasaCambio(bool fromTop)
        {
            //int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            //TipoMoneda tipoMoneda = (TipoMoneda)monOr;

            //if (monOr == (int)TipoMoneda.Local)
            //    this.monedaId = this._mdaLocalXDef;
            //else
            //    this.monedaId = this._mdaExtranjXDef;

            //this.masterMoneda.Value = this.monedaId;
            //Si la empresa no permite mmultimoneda
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
        private void CalculateValues(DTO_pyPreProyectoTarea tarea)
        {
            try
            {
                #region Calcula Costos y Cantidades
                foreach (DTO_pyPreProyectoDeta d in tarea.DetalleAPUCliente)
                {
                    decimal rend = (d.Cantidad.Value.Value * d.FactorID.Value.Value); //Rendimiento
                    d.CantidadTOT.Value = 1;//Cantidad total del recurso
                    if (d.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                    {
                        if (!this.chkEquipoCant.Checked)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * (rend != 0 ? rend : 1), 2);
                    }
                    else if (d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        if (!this.chkPersonalCant.Checked)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * (rend != 0 ? rend : 1), 2);
                    }
                    else
                        d.CostoLocalTOT.Value = Math.Round(d.CantidadTOT.Value.Value * d.CostoLocal.Value.Value * (rend != 0 ? rend : 1),2); //Valor Total por recurso
                } 
                #endregion
                #region Asigna valores
                decimal costoTotalUnitTarea = tarea.DetalleAPUCliente.Sum(x => x.CostoLocalTOT.Value.Value);
                #region Calcula AIU por APU
                if (this.chkAPUIncluyeAIU.Checked)
                {
                    decimal AIUAdminAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorAdmEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    decimal AIUImprAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    decimal AIUUtilAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture))/ 100,2);
                    tarea.DetalleAPUCliente.ForEach(x =>
                    {
                                            x.VlrAIUAdmin.Value = AIUAdminAPU;
                                            x.VlrAIUImpr.Value = AIUImprAPU;
                                            x.VlrAIUUtil.Value = AIUUtilAPU;
                                            });
                    tarea.VlrAIUxAPUAdmin.Value = AIUAdminAPU;
                    tarea.VlrAIUxAPUImpr.Value = AIUImprAPU;
                    tarea.VlrAIUxAPUUtil.Value = AIUUtilAPU;
                    //tarea.VlrAIUxAPUIVA.Value = Math.Round(tarea.Detalle.Where(x => x.TipoRecurso.Value != (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value) * (Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture) / 100), 2);
                    costoTotalUnitTarea += 0;// AIUAdminAPU + AIUImprAPU + AIUUtilAPU;
                } 
                #endregion
                if (this.cmbRedondeo.EditValue.Equals("2"))//Redonde hacia arriba
                    tarea.CostoTotalUnitML.Value = Math.Ceiling(costoTotalUnitTarea);
                else if (this.cmbRedondeo.EditValue.Equals("3")) //Redondea hacia abajo
                    tarea.CostoTotalUnitML.Value = Math.Floor(costoTotalUnitTarea);
                else
                    tarea.CostoTotalUnitML.Value = costoTotalUnitTarea != 0 ? Math.Round(costoTotalUnitTarea, 0) : tarea.CostoTotalUnitML.Value; 
                tarea.CostoTotalML.Value = tarea.CostoTotalUnitML.Value != 0 ? Math.Round(tarea.CostoTotalUnitML.Value.Value * tarea.Cantidad.Value.Value, 0) : tarea.CostoTotalML.Value;
                this._totalRecxTarea = tarea.CostoTotalML.Value.Value;
                if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) //Interno
                {
                    //this._rowPreTareaCurrent.PorDescuento.Value = this._rowPreTareaCurrent.PorDescuento.Value == null ? 0 : this._rowPreTareaCurrent.PorDescuento.Value;
                    //if (this._preProyectoDocu != null && this._preProyectoDocu.MultiplicadorActivoInd.Value.Value)
                    //    tarea.CostoLocalUnitCLI.Value = Math.Round((tarea.CostoTotalUnitML.Value.Value - (tarea.CostoTotalUnitML.Value.Value * (tarea.PorDescuento.Value.Value / 100))) * ((this._preProyectoDocu.PorMultiplicadorPresup.Value.Value / 100)), 0);
                    //tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    //tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                else if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
                {
                    //tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    //tarea.PorDescuento.Value = tarea.PorDescuento.Value == null ? 0 : tarea.PorDescuento.Value;
                    //if (this._preProyectoDocu != null && this._preProyectoDocu.MultiplicadorActivoInd.Value.Value)
                    //    tarea.CostoLocalUnitCLI.Value = Math.Round((tarea.CostoTotalUnitML.Value.Value - (tarea.CostoTotalUnitML.Value.Value * (tarea.PorDescuento.Value.Value / 100))) , 0);

                    //tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value ?? 0;
                    //tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
                }
                tarea.CostoDiferenciaML.Value = tarea.CostoLocalCLI.Value - tarea.CostoTotalML.Value;

                if (this.cmbTipoPresup.EditValue == "1")
                    tarea.LineaPresupuestoID.Value = tarea.TareaCliente.Value;
                if (tarea.Nivel.Value > 1)
                {
                    //Calcula totales(Sumatorias) por cada Nivel
                    var rowPadre1 = this._listPreTareasAll.Find(x=>x.TareaCliente.Value == tarea.TareaPadre.Value);
                    if (rowPadre1 != null)
                    {
                        rowPadre1.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                        rowPadre1.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                        if (rowPadre1.Nivel.Value > 1)
                        {
                            var rowPadre2 = this._listPreTareasAll.Find(x => x.TareaCliente.Value == rowPadre1.TareaPadre.Value);
                            if (rowPadre2 != null)
                            {
                                rowPadre2.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre1.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                rowPadre2.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre1.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                if (rowPadre2.Nivel.Value > 1)
                                {
                                    var rowPadre3 = this._listPreTareasAll.Find(x => x.TareaCliente.Value == rowPadre2.TareaPadre.Value);
                                    rowPadre3.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre2.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                    rowPadre3.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre2.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                    if (rowPadre3.Nivel.Value > 1)
                                    {
                                        var rowPadre4 = this._listPreTareasAll.Find(x => x.TareaCliente.Value == rowPadre3.TareaPadre.Value);
                                        rowPadre4.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre3.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                        rowPadre4.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre3.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                        if (rowPadre4.Nivel.Value > 1)
                                        {
                                            var rowPadre5 = this._listPreTareasAll.Find(x => x.TareaCliente.Value == rowPadre4.TareaPadre.Value);
                                            rowPadre5.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre4.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                            rowPadre5.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre4.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                            if (rowPadre5.Nivel.Value > 1)
                                            {
                                                var rowPadre6 = this._listPreTareasAll.Find(x => x.TareaCliente.Value == rowPadre5.TareaPadre.Value);
                                                rowPadre6.CostoTotalML.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre5.TareaPadre.Value).ToList().Sum(x => x.CostoTotalML.Value);
                                                rowPadre6.CostoLocalCLI.Value = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre5.TareaPadre.Value).ToList().Sum(x => x.CostoLocalCLI.Value);
                                            }
                                        }
                                    }
                                } 
                            }
                        } 
                    }
                }

                if (this.chkRecursoXTrabInd.Checked)
                {
                   //Asigna el costo por trabajo
                    decimal? costoxTrabajo =   this._rowPreTareaCurrent.DetalleAPUCliente.FindAll(x=>x.TrabajoID.Value == this._trabajoCurrent).Sum(y=>y.CostoLocalTOT.Value);
                    this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "CostoTotalML", costoxTrabajo);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "CalculateValues"));
            }
        }

        /// <summary>
        /// Calcula la jerarquia de cada tarea para saber si es detalle o no
        /// </summary>
        /// <param name="tareaCliente">tarea actual a validar</param>
        /// <param name="value">valor ingresado</param>
        private void CalculateHierarchy(DTO_pyPreProyectoTarea tareaCliente, string value)
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
                   var rowPreview = this._listPreTareasAll.FindLast(x => x.Nivel.Value == nivelPreview && !x.TareaCliente.Value.Equals("tit") && !x.TareaCliente.Value.Equals("TIT"));
                    if(rowPreview != null)
                        tareaCliente.TareaPadre.Value = rowPreview.TareaCliente.Value;
                }
            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "CalculateHierarchy"));
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
                    this._preProyectoDocu.ClienteID.Value = this.masterCliente.Value;
                    this._preProyectoDocu.DescripcionSOL.Value = this.txtDescripcion.Text;
                    this._preProyectoDocu.EmpresaNombre.Value = this.txtSolicitante.Text;
                    this._preProyectoDocu.ResponsableEMP.Value = this.masterResponsableEmp.Value;
                    this._preProyectoDocu.RecursosXTrabajoInd.Value = this.chkRecursoXTrabInd.Checked;
                    this._preProyectoDocu.PropositoProyecto.Value = Convert.ToByte(this.cmbProposito.EditValue);
                    this._preProyectoDocu.TipoSolicitud.Value = Convert.ToByte(this.cmbTipoSolicitud.EditValue);
                    this._preProyectoDocu.Licitacion.Value = this.txtLicitacion.Text;
                    this._preProyectoDocu.APUIncluyeAIUInd.Value = this.chkAPUIncluyeAIU.Checked;
                    this._preProyectoDocu.Jerarquia.Value = Convert.ToByte(this.cmbJerarquia.EditValue);
                    this._preProyectoDocu.PorClienteADM.Value = Convert.ToDecimal(this.txtPorAdmClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorClienteIMP.Value = Convert.ToDecimal(this.txtPorImprClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorClienteUTI.Value = Convert.ToDecimal(this.txtPorUtilClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaADM.Value = Convert.ToDecimal(this.txtPorAdmEmp.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaIMP.Value = Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaUTI.Value = Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorMultiplicadorPresup.Value = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.TasaCambio.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    //this._preProyectoDocu.Version.Value = Convert.ToByte(this._versionCotizacion);
                    this._preProyectoDocu.TipoRedondeo.Value = Convert.ToByte(this.cmbRedondeo.EditValue);
                    this._preProyectoDocu.EquipoCantidadInd.Value = this.chkEquipoCant.Checked;
                    this._preProyectoDocu.PersonalCantidadInd.Value = this.chkPersonalCant.Checked;
                    this._preProyectoDocu.MultiplicadorActivoInd.Value = this.chkMultipActivo.Checked;
                    this._preProyectoDocu.PorIVA.Value = Convert.ToInt32(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture);
                    //Adicionales
                    this._preProyectoDocu.Valor.Value = Convert.ToDecimal(this.txtCostoPresupuesto.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.ValorCliente.Value = Convert.ToDecimal(this.txtCostoCliente.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.ValorIVA.Value = Convert.ToDecimal(this.txtIVA.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.ValorOtros.Value = Convert.ToDecimal(this.txtOtros.EditValue, CultureInfo.InvariantCulture); 
                }
                else
                {
                    this.cmbTipoPresup.EditValue = this._dtoClaseProyecto != null? this._dtoClaseProyecto.TipoPresupuesto.Value.ToString() : "1";
                    this.masterClaseServicio.Value = this._preProyectoDocu.ClaseServicioID.Value;
                    this.masterCliente.Value = this._preProyectoDocu.ClienteID.Value;
                    this.masterResponsableEmp.Value = this._preProyectoDocu.ResponsableEMP.Value;
                    this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
                    this.masterCentroCto.Value = this._ctrl.CentroCostoID.Value;
                    this.cmbTipoSolicitud.EditValue = this._preProyectoDocu.TipoSolicitud.Value.ToString();
                    this.cmbProposito.EditValue = this._preProyectoDocu.PropositoProyecto.Value.ToString();
                    this.cmbJerarquia.EditValue = this._preProyectoDocu.Jerarquia.Value.ToString();                 
                    this.chkRecursoXTrabInd.Checked = this._preProyectoDocu.RecursosXTrabajoInd.Value.Value;                  
                    this.chkAPUIncluyeAIU.Checked = this._preProyectoDocu.APUIncluyeAIUInd.Value.Value;
                    this.chkRecursoXTrabInd.Checked = this._preProyectoDocu.RecursosXTrabajoInd.Value.Value;
                    this.txtObservaciones.Text = this._ctrl.Observacion.Value;
                    this.txtDescripcion.Text = this._preProyectoDocu.DescripcionSOL.Value;
                    this.txtSolicitante.Text = this._preProyectoDocu.EmpresaNombre.Value;
                    this.txtLicitacion.Text = this._preProyectoDocu.Licitacion.Value;
                    this.txtPorAdmClient.EditValue = this._preProyectoDocu.PorClienteADM.Value;
                    this.txtPorImprClient.EditValue = this._preProyectoDocu.PorClienteIMP.Value;
                    this.txtPorUtilClient.EditValue = this._preProyectoDocu.PorClienteUTI.Value;
                    this.txtPorAdmEmp.EditValue = this._preProyectoDocu.PorEmpresaADM.Value;
                    this.txtPorImprEmp.EditValue = this._preProyectoDocu.PorEmpresaIMP.Value;
                    this.txtPorUtilEmp.EditValue = this._preProyectoDocu.PorEmpresaUTI.Value;
                    this.txtMultiplicador.EditValue = this._preProyectoDocu.PorMultiplicadorPresup.Value;                  
                    this.txtTasaCambio.EditValue = this._preProyectoDocu.TasaCambio.Value;
                    //this._versionCotizacion = this._preProyectoDocu.Version.Value;
                    this.cmbRedondeo.EditValue = this._preProyectoDocu.TipoRedondeo.Value.ToString();
                    this.chkEquipoCant.Checked = this._preProyectoDocu.EquipoCantidadInd.Value.Value;
                    this.chkPersonalCant.Checked = this._preProyectoDocu.PersonalCantidadInd.Value.Value;
                    this.chkMultipActivo.Checked = this._preProyectoDocu.MultiplicadorActivoInd.Value.Value;
                    this.txtPorIVA.EditValue = this._preProyectoDocu.PorIVA.Value.Value;
                    //Adicionales
                    this.txtIVA.EditValue = this._ctrl.Iva.Value;
                    this.txtOtros.EditValue = this._preProyectoDocu.ValorOtros.Value;                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "CargarInformacion"));
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
                    this.cmbProposito.EditValue = ((int)TipoSolicitud.Cotizacion);
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "CleanHeader"));
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
            this.masterResponsableEmp.EnableControl(p);
            this.txtNro.Enabled = p;
            this.btnQueryDoc.Enabled = p;            
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
                this._bc.InitMasterUC(this.masterClaseServicio, AppMasters.pyClaseProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterResponsableEmp, AppMasters.seUsuario, false, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true,true);
                this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, true);
                this._bc.InitMasterUC(this.masterUsuarioPermiso, AppMasters.seUsuario, true, false, true, true);

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
                this.cmbReporte.EditValue = "1";

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
                this._tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);               
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
                string proyectoxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
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
                this.masterProyecto.Value = string.Empty;
                this.masterCentroCto.Value = centroCto;
                this.AsignarTasaCambio(true);
                this._listPreRecursosXTareaAll = this._bc.AdministrationModel.pyPreProyectoDeta_GetByTarea(this.documentID, string.Empty, string.Empty, null, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "InitControls"));
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

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.py_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrid()
        {
            try
            {                
                this.gcDocument.DataSource = this._listPreTareasAll;
                this.gcDocument.RefreshDataSource();
                this.gcRecurso.DataSource = null;
                this.gcRecurso.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "LoadData"));
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

                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(this.documentID, this.masterPrefijo.Value, docNro, null, string.Empty, this.masterProyecto.Value, true,false,false,true);
                if (transaccion != null)
                {
                    this._ctrl = transaccion.DocCtrl;

                    #region Valida si copia el documento seleccionado
                    if (this._copyData)
                    {
                        this._ctrl.NumeroDoc.Value = null;
                        this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        transaccion.Header.NumeroDoc.Value = null;
                        transaccion.DocCtrl.NumeroDoc.Value = null;
                        transaccion.DocCtrl.NumeroDoc.Value = null;
                        transaccion.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        foreach (var tarea in transaccion.Detalle)
                        {
                            tarea.NumeroDoc.Value = null;
                            tarea.Consecutivo.Value = null;
                            foreach (var rec in tarea.DetalleAPUCliente)
                            {
                                rec.NumeroDoc.Value = null;
                                rec.Consecutivo.Value = null;
                            }
                        }
                        foreach (var tarea in transaccion.DetalleTareasAdic)
                        {
                            tarea.NumeroDoc.Value = null;
                            tarea.Consecutivo.Value = null;
                            foreach (var rec in tarea.DetalleAPUCliente)
                            {
                                rec.NumeroDoc.Value = null;
                                rec.Consecutivo.Value = null;
                            }
                        }
                        this._copyData = false;
                    }
                    #endregion

                    this._preProyectoDocu = transaccion.Header;
                    this._listPreTareasAll = transaccion.Detalle;
                    this._dtoClaseProyecto = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, transaccion.Header.ClaseServicioID.Value, true);
                    this.CargarInformacion(true);
                    foreach (var tarea in _listPreTareasAll)
                        this.CalculateValues(tarea);
                    this._listPreTareasAdicion = transaccion.DetalleTareasAdic;
                    this.UpdateValues();
                    this._numeroDoc = this._ctrl.NumeroDoc.Value != null ? this._ctrl.NumeroDoc.Value.Value : 0;
                    this.LoadGrid();
                    this.EnableHeader(this._numeroDoc != 0 ? false : true);                    
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.InvalidDocument));
                    this._ctrl = new DTO_glDocumentoControl();
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
                string _mdaExtranjXDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                valor = _bc.AdministrationModel.TasaDeCambio_Get(_mdaExtranjXDef, this.dtFecha.DateTime);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "LoadTasaCambio"));
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
            this.masterPrefijo.Value = string.Empty;
            this.masterClaseServicio.Value = string.Empty;
            this.txtNro.Text = string.Empty;
            this.CleanHeader(true);
            this.EnableHeader(true);

            this._ctrl = null;
            this._numeroDoc = 0;
            this._preProyectoDocu = new DTO_pyPreProyectoDocu();  
            this._rowPreTareaCurrent = new DTO_pyPreProyectoTarea();
            this._rowPreRecursoCurrent = new DTO_pyPreProyectoDeta();
            this._listPreTareasAll = new List<DTO_pyPreProyectoTarea>();
            this._listPreTareasAdicion = new List<DTO_pyPreProyectoTarea>();
            this._listPreRecursosXTareaAll = new List<DTO_pyPreProyectoDeta>();
            this._dtoClaseProyecto = null;
            this._tareaIsFocused = false;
            this._totalRecxTarea = 0;
            this._totalPresupuesto = 0;
            this._totalCliente = 0;
            this._trabajoCurrent = string.Empty;
            this.gvRecursoCurrent = new GridView();
            this.gcDocument.DataSource = this._listPreTareasAll;
            this.gcDocument.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
            this.gcRecurso.Enabled = true;
            this.grpCtrlRecurso.Enabled = true;

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
                this.documentID = AppDocuments.PlaneacionCostosAPUCliente;
                this.AddGridCols();
                this.InitControls();
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);

                this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
                this.userCurrent = _bc.AdministrationModel.User.ID.Value;

                this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
                this.saveDelegate = new Save(this.SaveMethod);
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "SetInitParameters"));
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
                        if (string.IsNullOrEmpty(this._rowPreTareaCurrent.TareaCliente.Value) && string.IsNullOrEmpty(this._rowPreTareaCurrent.TareaPadre.Value))
                        {
                            this._rowPreTareaCurrent.TareaCliente.Value = "tit";
                            this._rowPreTareaCurrent.DetalleInd.Value = false;
                            this._rowPreTareaCurrent.TituloPrintInd.Value = true;
                        }
                          
                        if (this.nivelMax < this._listPreTareasAll[fila].Nivel.Value)
                        {
                            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"];
                            string colVal = this.gvDocument.GetRowCellValue(this.gvDocument.FocusedRowHandle, col).ToString();
                            this.gvDocument.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, "El nivel maximo seleccionado no corresponde al digitado"));
                            validRow = false;
                        }
                    }
                    #endregion
                    #region UnidadInvID
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "UnidadInvID", !this._rowPreTareaCurrent.DetalleInd.Value.Value, true, false, AppMasters.inUnidad);
                    if (!validField)
                        validRow = false;

                    #endregion                    
                    #region Descriptivo
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "Descriptivo", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Cantidad
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "Cantidad", !this._rowPreTareaCurrent.DetalleInd.Value.Value, false, false, null);
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

            if (string.IsNullOrEmpty(this.masterClaseServicio.Value))
                camposObligatorios = camposObligatorios + this.masterClaseServicio.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.masterCentroCto.Value))
                camposObligatorios = camposObligatorios + this.masterCentroCto.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.txtSolicitante.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblSolicitante.Text) + "\n";
            if (string.IsNullOrEmpty(this.masterResponsableEmp.Value))
                camposObligatorios = camposObligatorios + this.lblResponableEmp.Text + "\n";
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
        private void ValidateDataImport(DTO_pyPreProyectoTarea dtoTarea, DTO_ExportRecursosDeta dtoRecurso, DTO_TxResultDetail rd, string msgFkNotFound, string msgEmptyField)
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
                    #region Validacion Unidad
                    if (string.IsNullOrWhiteSpace(dtoTarea.UnidadInvID.Value.ToString()))
                    {
                        //DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        //rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.SolicitudProyecto + "_UnidadInvID");
                        //rdF.Message = msgEmptyField;
                        //rd.DetailsFields.Add(rdF);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "ValidateDataImport"));
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
                foreach (DTO_pyPreProyectoTarea tarea in this._listPreTareasAll.FindAll(x=>x.DetalleInd.Value.Value))
                {
                    this._totalPresupuesto += tarea.CostoTotalML.Value.Value;
                    this._totalCliente += tarea.CostoLocalCLI.Value.Value;
                }
                //Actualiza valores APU
                this.txtCostoAPU.EditValue = this._rowPreTareaCurrent.CostoTotalUnitML.Value;
                this.txtCostoAIUxAPU.EditValue = this._rowPreTareaCurrent.VlrAIUxAPUAdmin.Value + this._rowPreTareaCurrent.VlrAIUxAPUImpr.Value + this._rowPreTareaCurrent.VlrAIUxAPUUtil.Value + this._rowPreTareaCurrent.VlrAIUxAPUIVA.Value.Value;
                this.txtCostoTotalAPU.EditValue = this._rowPreTareaCurrent.CostoTotalUnitML.Value+ this._rowPreTareaCurrent.VlrAIUxAPUAdmin.Value + 
                                                    this._rowPreTareaCurrent.VlrAIUxAPUImpr.Value + this._rowPreTareaCurrent.VlrAIUxAPUUtil.Value + this._rowPreTareaCurrent.VlrAIUxAPUIVA.Value.Value;
              
                //Actualiza valores generales
                this.txtCostoPresupuesto.EditValue = this._totalPresupuesto;
                this.txtCostoCliente.EditValue = this._totalCliente;
                if (this._dtoClaseProyecto != null && this._dtoClaseProyecto.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) 
                    this.txtCostoMult.EditValue = this._totalCliente;
                else // 1 Cliente
                {
                    //decimal totalClienteMult = 0;
                    //decimal porcMultiplicador = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture) / 100;
                    //foreach (DTO_pyPreProyectoTarea tarea in this._listPreTareasAll.FindAll(x => x.DetalleInd.Value.Value))
                    //    totalClienteMult += Math.Round(tarea.CostoLocalCLI.Value.Value * porcMultiplicador, 0);
                    //if (this._dtoClaseProyecto != null && !this._dtoClaseProyecto.CalcularValorClienteInd.Value.Value)
                    //    this.txtCostoMult.EditValue = totalClienteMult;
                    //else
                        this.txtCostoMult.EditValue = this._totalCliente;
                }
                decimal vlrUtilidad = Math.Round((this._totalPresupuesto * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
                decimal vlrTotalBaseIVA = vlrUtilidad + Convert.ToDecimal(this.txtOtros.EditValue, CultureInfo.InvariantCulture);
                this.txtIVA.EditValue = Math.Round((vlrTotalBaseIVA * Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture))/100,0);
            
                //Actualiza permiso usuario x tarea
                this.masterUsuarioPermiso.Value = this._rowPreTareaCurrent.UsuarioID.Value;
                if(this.userCurrent.Equals(this.masterResponsableEmp.Value))
                    this.chkUserEdit.Properties.ReadOnly = false;
                else
                    this.chkUserEdit.Properties.ReadOnly = true;

                if(!this.masterUsuarioPermiso.Value.Equals(this.userCurrent))
                {
                    this.grpCtrlRecurso.Enabled = false;
                    this.gcRecurso.Enabled = false;
                }
                else
                {
                    this.grpCtrlRecurso.Enabled = true;
                    this.gcRecurso.Enabled = true;
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvDetalle_FocusedRowChanged"));
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
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {               
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr); 
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemSendtoAppr.ToolTipText = this._bc.GetResource(LanguageTypes.ToolBar, "acc_approve");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "Form_FormClosed"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "dtPeriod_EditValueChanged"));
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
        /// Al seleccionar el proyecto Origen 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdProyectoOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

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
                docs.Add(AppDocuments.PreProyecto);
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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente", "btnQueryDoc_Click"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionCostosAPUCliente.cs-txtNro_Leave"));
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
                this._listPreRecursosXTareaAll = this._bc.AdministrationModel.pyPreProyectoDeta_GetByTarea(this.documentID, string.Empty, this.masterClaseServicio.Value, null, false);
                #region Convierte Dolares en Pesos
                foreach (var det in this._listPreRecursosXTareaAll)
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
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PlaneacionCostosAPUCliente.cs - Creacion de DTO y validacion Formatos");
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
                            this._rowPreTareaCurrent.DetalleAPUCliente = new List<DTO_pyPreProyectoDeta>();
                            #region Agrega los recursos a las tareas en Masivo
                            foreach (DTO_pyPreProyectoTarea tarea in this._listPreTareasAll)
                            {
                                //Validad si la tarea tiene recursos importados
                                if (recursosList.Exists(x => x.TareaCliente.Value == tarea.TareaCliente.Value && x.TareaID.Value == tarea.TareaID.Value))
                                {
                                    i = 0;
                                    foreach (DTO_ExportRecursosDeta r in recursosList.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value && x.TareaID.Value == tarea.TareaID.Value))
                                    {
                                        #region Agrega el recurso
                                        DTO_pyPreProyectoDeta det = new DTO_pyPreProyectoDeta();
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
                                        det.CostoLocal.Value = r.CostoLocal.Value;
                                        if (det.CostoLocal.Value == null || det.CostoLocal.Value == 0)
                                        {
                                            //Trae el costo del recurso
                                            var rec = this._listPreRecursosXTareaAll.Find(x => x.RecursoID.Value == det.RecursoID.Value);
                                            if (rec != null)
                                                det.CostoLocal.Value = rec.CostoLocal.Value;
                                            else
                                                det.CostoLocal.Value = 0;
                                        }                                           
                                        tarea.DetalleAPUCliente.Add(det);
                                        i++;
                                        #endregion
                                    }
                                    if (result.Details.Count > 0)
                                    {
                                        tarea.DetalleAPUCliente = new List<DTO_pyPreProyectoDeta>();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "ImportThread"));
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
                    List<DTO_pyPreProyectoDeta> data = this._rowPreTareaCurrent.DetalleAPUCliente;
                    List<DTO_ExportRecursosDeta> list = new List<DTO_ExportRecursosDeta>();
                    foreach (var d in data)
                    {
                        DTO_ExportRecursosDeta deta = new DTO_ExportRecursosDeta();
                        deta.TareaID.Value = this._rowPreTareaCurrent.TareaID.Value;
                        deta.TareaCliente.Value = this._rowPreTareaCurrent.TareaCliente.Value;
                        deta.Descripcion.Value = this._rowPreTareaCurrent.Descriptivo.Value;
                        deta.RecursoID.Value = d.RecursoID.Value;
                        deta.RecursoDesc.Value = d.RecursoDesc.Value;
                        deta.UnidadInvID.Value = d.UnidadInvID.Value;
                        deta.CostoLocal.Value = d.CostoLocal.Value;
                        deta.FactorID.Value = d.FactorID.Value;                      
                        list.Add(deta);
                    }
                    DataTableOperations tableOp = new DataTableOperations();
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportRecursosDeta), list);
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
                else
                {
                    List<DTO_ExportRecursosDeta> list = new List<DTO_ExportRecursosDeta>();
                    foreach (DTO_pyPreProyectoTarea tarea in this._listPreTareasAll)
                    {
                        List<DTO_pyPreProyectoDeta> data = tarea.DetalleAPUCliente;
                        foreach (DTO_pyPreProyectoDeta d in data)
                        {
                            DTO_ExportRecursosDeta deta = new DTO_ExportRecursosDeta();
                            deta.TareaID.Value = tarea.TareaID.Value;
                            deta.TareaCliente.Value = tarea.TareaCliente.Value;
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
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "btnExportRecursos"));
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
        private void txtMultiplicador_EditValueChanged(object sender, EventArgs e)
        {
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
                 MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "txt_MouseHover: " + ex.Message));
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
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].UnGroup();
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = false;
                
            }
            else if (this._dtoClaseProyecto != null) // Interno
            {
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].Group();
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortOrder = ColumnSortOrder.None;
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].Visible = false;               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyAPU_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var tarea in this._listPreTareasAll)
                {
                    tarea.DetalleAPUCliente = tarea.Detalle;
                }
                this.LoadGrid();
                this._tareaIsFocused = false;
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
            //this.po = Convert.ToDecimal(this.txtMultiplicador.EditValue, CultureInfo.InvariantCulture);

            //if (this._dtoClaseServicio != null && this._dtoClaseServicio.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros && this.chkMultipActivo.Checked) // Interno
            //{
            //    foreach (var tarea in this._listPreTareasAll)
            //    {
            //        tarea.PorDescuento.Value = tarea.PorDescuento.Value ?? 0;
            //        tarea.CostoLocalUnitCLI.Value = tarea.CostoTotalUnitML.Value * (this.porcIncrementoMult / 100) - (tarea.CostoTotalUnitML.Value * (tarea.PorDescuento.Value / 100));
            //        tarea.CostoLocalCLI.Value = tarea.CostoLocalUnitCLI.Value * tarea.Cantidad.Value;
            //    }

            //    this._tareaIsFocused = false;
            //    this.AssignRecursos();
            //}
            //else //Cliente
            //    this.UpdateValues();
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
                    this._rowPreTareaCurrent = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.FocusedRowHandle);
                    this._tareaIsFocused = true;
                    this.AssignRecursos();
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true;
                }
                else
                {
                    this.gcRecurso.DataSource = null;
                    this.gcRecurso.RefreshDataSource();
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvDocument_FocusedRowChanged"));
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

                        if (this._listPreTareasAll.Count > 0)
                        {
                            List<DTO_pyPreProyectoTarea> hijosTarea = this._listPreTareasAll.FindAll(x => x.TareaPadre.Value == this._rowPreTareaCurrent.TareaCliente.Value).ToList();
                            if (hijosTarea.Count > 0)
                            {
                                if (MessageBox.Show("Esta tarea tiene niveles de jerarquía dependientes, desea eliminarla?", msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    this._listPreTareasAll.RemoveAll(x => x.TareaCliente.Value == this._rowPreTareaCurrent.TareaCliente.Value &&
                                                              x.Descriptivo.Value == this._rowPreTareaCurrent.Descriptivo.Value);
                                }
                            }
                            else 
                            {
                                this._listPreTareasAll.RemoveAll(x => x.TareaCliente.Value == this._rowPreTareaCurrent.TareaCliente.Value &&
                                                                x.Descriptivo.Value == this._rowPreTareaCurrent.Descriptivo.Value);
                            }

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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
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
                this._rowPreTareaCurrent = (DTO_pyPreProyectoTarea)gvDocument.GetRow(e.RowHandle);
                if (fieldName == "TareaID")
                {
                    DTO_pyTarea dtoTarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, e.Value.ToString(), true);
                    if (dtoTarea != null)
                    {
                        this._rowPreTareaCurrent.Descriptivo.Value = dtoTarea.Descriptivo.Value;
                        if (this._dtoClaseProyecto.TareaAsociadaInd.Value.Value)
                        {
                            Dictionary<string, string> pks = new Dictionary<string, string>();
                            pks.Add("TareaID", dtoTarea.ID.Value);
                            pks.Add("ClaseServicioID", this._dtoClaseProyecto.ID.Value);
                            DTO_pyTareaClase dtoTareaClase = (DTO_pyTareaClase)this._bc.GetMasterComplexDTO(AppMasters.pyTareaClase, pks, true);
                            if (dtoTareaClase != null)
                            {
                                #region Obtiene APUs
                                //Trae APUs parametrizados
                                var apuNews = this._bc.AdministrationModel.pyPreProyectoDeta_GetByTarea(this.documentID, dtoTarea.ID.Value, this._dtoClaseProyecto.ID.Value, null, false, this._tasaCambio).ToList();
                                apuNews.ForEach(x =>
                                {
                                    //Valida si ya existe para asignar valores
                                    if (this._rowPreTareaCurrent.Detalle.Exists(y => y.RecursoID.Value == x.RecursoID.Value))
                                    {
                                        x.Consecutivo.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).Consecutivo.Value;
                                        x.ConsecTarea.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).ConsecTarea.Value;
                                        x.NumeroDoc.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).NumeroDoc.Value;
                                        x.TrabajoID.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).TrabajoID.Value;
                                    }
                                });

                                //Valida si borra APUs existentes
                                foreach (var apuOld in this._rowPreTareaCurrent.Detalle)
                                {
                                    if (apuOld.Consecutivo.Value != null && !apuNews.Exists(x => x.Consecutivo.Value == apuOld.Consecutivo.Value))
                                        if (!this._preProyectoDocu.RecursosDeleted.Exists(x => x == apuOld.Consecutivo.Value))
                                            this._preProyectoDocu.RecursosDeleted.Add(apuOld.Consecutivo.Value.Value);
                                }
                                #endregion
                                this._rowPreTareaCurrent.Detalle = apuNews;
                                if (this._rowPreTareaCurrent.Detalle.Count > 0) this._rowPreTareaCurrent.DetalleInd.Value = true;
                                this.AssignRecursos();
                            }
                        }
                        else
                        {
                            #region Obtiene APUs
                            //Trae APUs parametrizados
                            var apuNews = this._bc.AdministrationModel.pyPreProyectoDeta_GetByTarea(this.documentID, dtoTarea.ID.Value, string.Empty, null, false, this._tasaCambio).ToList();
                            apuNews.ForEach(x =>
                            {
                                //Valida si ya existe para asignar valores
                                if (this._rowPreTareaCurrent.Detalle.Exists(y => y.RecursoID.Value == x.RecursoID.Value))
                                {
                                    x.Consecutivo.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).Consecutivo.Value;
                                    x.ConsecTarea.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).ConsecTarea.Value;
                                    x.NumeroDoc.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).NumeroDoc.Value;
                                    x.TrabajoID.Value = this._rowPreTareaCurrent.Detalle.Find(y => y.RecursoID.Value == x.RecursoID.Value).TrabajoID.Value;
                                }
                            });

                            //Valida si borra APUs existentes
                            foreach (var apuOld in this._rowPreTareaCurrent.Detalle)
                            {
                                if (apuOld.Consecutivo.Value != null && !apuNews.Exists(x => x.Consecutivo.Value == apuOld.Consecutivo.Value))
                                    if (!this._preProyectoDocu.RecursosDeleted.Exists(x => x == apuOld.Consecutivo.Value))
                                        this._preProyectoDocu.RecursosDeleted.Add(apuOld.Consecutivo.Value.Value);
                            }
                            #endregion
                            this._rowPreTareaCurrent.Detalle = apuNews;
                            if (this._rowPreTareaCurrent.Detalle.Count > 0) this._rowPreTareaCurrent.DetalleInd.Value = true;
                            this.AssignRecursos();
                        }
                    }
                }
                if (fieldName == "Cantidad")
                {
                    this._rowPreTareaCurrent.Cantidad.Value = e.Value != null? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();                     
                }
                else if (fieldName == "CostoTotalUnitML")
                {

                    this._rowPreTareaCurrent.CostoTotalML.Value = e.Value != null ? this._rowPreTareaCurrent.Cantidad.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "CostoTotalML")
                {
                    this._rowPreTareaCurrent.CostoTotalML.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalUnitCLI")
                {
                    this._rowPreTareaCurrent.CostoLocalCLI.Value = e.Value != null ? this._rowPreTareaCurrent.Cantidad.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.gvDocument.RefreshData();
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalCLI")
                {
                    this._rowPreTareaCurrent.CostoLocalCLI.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.gvDocument.RefreshData();
                    this.UpdateValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvDocument_CellValueChanged"));
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
                        this.CalculateHierarchy(this._rowPreTareaCurrent, e.Value.ToString());                   
                }        
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvDocument_CellValueChanging"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvDocument_BeforeLeaveRow"));
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
                DTO_pyPreProyectoTarea currentRow = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.RowHandle);
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
                    string capituloDesc = this._listPreTareasAll.Find(x => x.CapituloTareaID.Value == e.Value.ToString()).CapituloDesc.Value;
                    e.DisplayText = e.Value.ToString() + "  " + capituloDesc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvRecurso_CustomColumnDisplayText"));
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

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "editBtnGrid_ButtonClick"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "editLink_Click"));
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
                if (string.IsNullOrEmpty(this._rowPreTareaCurrent.TareaCliente.Value))
                {
                    MessageBox.Show("No existe un análisis digitado");
                    return;
                }

                if (this._rowPreTareaCurrent != null)
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

                            if (this._rowPreTareaCurrent.DetalleAPUCliente.Count >= 1)
                            {
                                this._rowPreTareaCurrent.DetalleAPUCliente.RemoveAll(x => x.RecursoID.Value == this._rowPreRecursoCurrent.RecursoID.Value &&
                                                                            x.TareaID.Value == this._rowPreRecursoCurrent.TareaID.Value);
                                this.gcRecurso.DataSource = this._rowPreTareaCurrent.DetalleAPUCliente;
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvRecurso.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvRecurso.FocusedRowHandle = rowHandle - 1;

                                this.CalculateValues(this._rowPreTareaCurrent);
                                this.UpdateValues();
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gcDetalle_EmbeddedNavigator_ButtonClick"));
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

                //this._rowTareaCurrent = (DTO_pyPreProyectoTarea)gvDocument.GetRow(gvDocument.FocusedRowHandle);
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
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "FactorID")
                {
                    if (this._rowPreRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte || this._rowPreRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
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
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();                       
                }
                else if (fieldName == "CostoLocal")
                {
                    if (this._rowPreRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte || this._rowPreRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
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
                    this.CalculateValues(this._rowPreTareaCurrent);
                    this.UpdateValues();
                }
                
            }
            catch (Exception ex )
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvRecurso_CellValueChanged"));
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
                    if(this.chkRecursoXTrabInd.Checked)
                        this._rowPreRecursoCurrent = (DTO_pyPreProyectoDeta)this.gvRecursoCurrent.GetRow(e.FocusedRowHandle);
                    else
                        this._rowPreRecursoCurrent = (DTO_pyPreProyectoDeta)this.gvRecurso.GetRow(e.FocusedRowHandle);
                    if (this._rowPreRecursoCurrent != null)
                    {
                        //this.txtDistanciaTurno.EditValue = this._rowRecursoCurrent.Distancia_Turnos.Value != null ? this._rowRecursoCurrent.Distancia_Turnos.Value : 0;
                        //this.txtPesoCant.EditValue = this._rowRecursoCurrent.Peso_Cantidad.Value != null ? this._rowRecursoCurrent.Peso_Cantidad.Value : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvRecurso_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "gvRecurso_CustomColumnDisplayText"));
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
            this.gvDocument.PostEditor();
            string camposObligatorios = this.ValidateHeader();
            if (string.IsNullOrEmpty(camposObligatorios))
            {
                this.CargarInformacion(false);
                Thread process = new Thread(this.SaveThread);
                process.Start();              
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
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint() 
        {
            try
            {
                DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
                this.CargarInformacion(false);
                solicitud.Detalle = ObjectCopier.Clone(this._listPreTareasAll);
                solicitud.DetalleTareasAdic = ObjectCopier.Clone(this._listPreTareasAdicion);
                solicitud.DocCtrl = this._ctrl;
                solicitud.Header = ObjectCopier.Clone(this._preProyectoDocu);
                string reportName = this._bc.AdministrationModel.Reportes_py_PlaneacionCostos(solicitud, false, Convert.ToByte(this.cmbReporte.EditValue), null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "TBPrint"));
            }
        }

        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentActionConfirm);
            string msgAprobar = string.Format(msgDoc, "Aprobar");

            if (MessageBox.Show(msgAprobar, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {   
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();                
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

                if (this._preProyectoDocu.NumeroDoc.Value != null && this._preProyectoDocu.NumeroDoc.Value != 0)
                    this._numeroDoc = this._preProyectoDocu.NumeroDoc.Value.Value;

                DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                transaccion.Header = ObjectCopier.Clone(this._preProyectoDocu);
                transaccion.Detalle = this._listPreTareasAll;
                transaccion.DetalleTareasAdic = this._listPreTareasAdicion;             

                result = this._bc.AdministrationModel.SolicitudProyecto_AddAPUCliente(this.documentID,this._numeroDoc,transaccion,false);

                if (result.Result == ResultValue.OK)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SuccessSol), this.masterPrefijo.Value, result.ExtraField));
                    this._preProyectoDocu = transaccion.Header;
                    this._listPreTareasAll = transaccion.Detalle;
                    this._listPreTareasAdicion = transaccion.DetalleTareasAdic;
                    this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc);
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "SaveThread"));
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
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                if (this._preProyectoDocu.NumeroDoc.Value != null && this._preProyectoDocu.NumeroDoc.Value != 0)
                    this._numeroDoc = this._preProyectoDocu.NumeroDoc.Value.Value;

                DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                transaccion.Header = ObjectCopier.Clone(this._preProyectoDocu);
                transaccion.Detalle = this._listPreTareasAll;
                transaccion.DetalleTareasAdic = this._listPreTareasAdicion;

                result = this._bc.AdministrationModel.SolicitudProyecto_AddAPUCliente(this.documentID, this._numeroDoc, transaccion, true);

                if (result.Result == ResultValue.OK)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SuccessSol), this.masterPrefijo.Value, result.ExtraField));
                    this._preProyectoDocu = transaccion.Header;
                    this._listPreTareasAll = transaccion.Detalle;
                    this._listPreTareasAdicion = transaccion.DetalleTareasAdic;
                    this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._numeroDoc);
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostosAPUCliente.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion                                           

    }
}
