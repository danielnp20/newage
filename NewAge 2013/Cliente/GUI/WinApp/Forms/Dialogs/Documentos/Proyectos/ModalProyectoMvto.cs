using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
public partial class ModalProyectoMvto : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private bool _filterActive = false;
        //Variables de data
        private DTO_pyProyectoTarea _tareaCurrent = null;
        private List<DTO_pyProyectoTarea> _listTareas = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public DTO_pyProyectoTarea TareaSelected
        {
            get { return this._tareaCurrent; }
        }

        #endregion

        ///<summary>
        /// Constructor  para consultar la ejecucion y trazabilidad
        /// </summary>
        public ModalProyectoMvto(int? numDocProyecto, int? consecMvto, string recursoID)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this.uc_Proyecto.Init(false, false, false, true);
                this.uc_Proyecto.LoadProyectoInfo_Leave += new UC_Proyecto.EventHandler(this.ucProyecto_LoadProyectoInfo_Click);                
                this.Text = this._bc.GetResource(LanguageTypes.Forms, "110_ModalProyectoMvto");
                this.uc_Proyecto.LoadData(string.Empty, null, numDocProyecto, string.Empty);
                this.TabSelect.PageVisible = false;
                this.LoadGridData(recursoID,string.Empty,false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalProyectosMvto.cs", "ModalProyectosMvto: " + ex.Message));
            }
        }

        ///<summary>
        /// Constructor para seleccionar tarea para adicion al proyecto
        /// </summary>
        public ModalProyectoMvto(string proyectoID)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this.uc_Proyecto.Init(false, false, false, false);
                this.uc_Proyecto.LoadProyectoInfo_Leave += new UC_Proyecto.EventHandler(this.ucProyecto_LoadProyectoInfo_Click);  
                this.Text = this._bc.GetResource(LanguageTypes.Forms, "Seleccionar");
                this.uc_Proyecto.LoadData(string.Empty, null, null, proyectoID);
                this.TapQuery.PageVisible = false;
                this.LoadGridData(string.Empty,proyectoID, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalProyectosMvto.cs", "ModalProyectosMvto: " + ex.Message));
            }
        }
        
        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.PreProyecto;
            this._frmModule = ModulesPrefix.py;
            this.AddGridCols();
            FormProvider.LoadResources(this, AppDocuments.PreProyecto);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Recursos 
            GridColumn RecursoIDConsol = new GridColumn();
            RecursoIDConsol.FieldName = this._unboundPrefix + "RecursoID";
            RecursoIDConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoIDConsol.UnboundType = UnboundColumnType.String;
            RecursoIDConsol.VisibleIndex = 1;
            RecursoIDConsol.Width = 100;
            RecursoIDConsol.Visible = true;
            RecursoIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoIDConsol);

            GridColumn RecursoDescConsol = new GridColumn();
            RecursoDescConsol.FieldName = this._unboundPrefix + "RecursoDesc";
            RecursoDescConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDescConsol.UnboundType = UnboundColumnType.String;
            RecursoDescConsol.VisibleIndex = 2;
            RecursoDescConsol.Width = 200;
            RecursoDescConsol.Visible = true;
            RecursoDescConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDescConsol);

            GridColumn UnidadInvIDConsol = new GridColumn();
            UnidadInvIDConsol.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvIDConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDConsol.UnboundType = UnboundColumnType.String;
            UnidadInvIDConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvIDConsol.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvIDConsol.VisibleIndex = 3;
            UnidadInvIDConsol.Width = 35;
            UnidadInvIDConsol.Visible = true;
            UnidadInvIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDConsol);

            //MarcaInvID
            GridColumn MarcaInvIDConsol = new GridColumn();
            MarcaInvIDConsol.FieldName = this._unboundPrefix + "MarcaInvID";
            MarcaInvIDConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_MarcaInvID");
            MarcaInvIDConsol.UnboundType = UnboundColumnType.String;
            MarcaInvIDConsol.VisibleIndex = 4;
            MarcaInvIDConsol.Width = 60;
            MarcaInvIDConsol.Visible = false;
            MarcaInvIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(MarcaInvIDConsol);

            //RefProveedor
            GridColumn RefProveedorConsol = new GridColumn();
            RefProveedorConsol.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedorConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_RefProveedor");
            RefProveedorConsol.UnboundType = UnboundColumnType.String;
            RefProveedorConsol.VisibleIndex = 5;
            RefProveedorConsol.Width = 60;
            RefProveedorConsol.Visible = false;
            RefProveedorConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RefProveedorConsol);

            GridColumn CantPresupuestadoRecConsol = new GridColumn();
            CantPresupuestadoRecConsol.FieldName = this._unboundPrefix + "CantPresupuestado";
            CantPresupuestadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPresupuestado");
            CantPresupuestadoRecConsol.UnboundType = UnboundColumnType.String;
            CantPresupuestadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPresupuestadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantPresupuestadoRecConsol.AppearanceCell.BackColor = Color.LightCyan;
            CantPresupuestadoRecConsol.AppearanceCell.Options.UseBackColor = true;
            CantPresupuestadoRecConsol.VisibleIndex = 6;
            CantPresupuestadoRecConsol.Width = 80;
            CantPresupuestadoRecConsol.Visible = true;
            CantPresupuestadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantPresupuestadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantPresupuestadoRecConsol);

            GridColumn VlrPresupuestadoREcConsol = new GridColumn();
            VlrPresupuestadoREcConsol.FieldName = this._unboundPrefix + "VlrPresupuestado";
            VlrPresupuestadoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrPresupuestado");
            VlrPresupuestadoREcConsol.UnboundType = UnboundColumnType.String;
            VlrPresupuestadoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestadoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestadoREcConsol.AppearanceCell.BackColor = Color.LightCyan;
            VlrPresupuestadoREcConsol.AppearanceCell.Options.UseBackColor = true;
            VlrPresupuestadoREcConsol.VisibleIndex = 7;
            VlrPresupuestadoREcConsol.Width = 100;
            VlrPresupuestadoREcConsol.Visible = true;
            VlrPresupuestadoREcConsol.ColumnEdit = this.editValue2;
            VlrPresupuestadoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrPresupuestadoREcConsol);

            GridColumn CantSolicitadoRecConsol = new GridColumn();
            CantSolicitadoRecConsol.FieldName = this._unboundPrefix + "CantSolicitado";
            CantSolicitadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantSolicitado");
            CantSolicitadoRecConsol.UnboundType = UnboundColumnType.String;
            CantSolicitadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantSolicitadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantSolicitadoRecConsol.AppearanceCell.BackColor = Color.Aquamarine;
            CantSolicitadoRecConsol.AppearanceCell.Options.UseBackColor = true;
            CantSolicitadoRecConsol.VisibleIndex = 8;
            CantSolicitadoRecConsol.Width = 80;
            CantSolicitadoRecConsol.Visible = false;
            CantSolicitadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantSolicitadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantSolicitadoRecConsol);

            GridColumn VlrSolicitadoRecConsol = new GridColumn();
            VlrSolicitadoRecConsol.FieldName = this._unboundPrefix + "VlrSolicitado";
            VlrSolicitadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrSolicitado");
            VlrSolicitadoRecConsol.UnboundType = UnboundColumnType.String;
            VlrSolicitadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrSolicitadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrSolicitadoRecConsol.VisibleIndex = 9;
            VlrSolicitadoRecConsol.Width = 100;
            VlrSolicitadoRecConsol.Visible = false;
            VlrSolicitadoRecConsol.ColumnEdit = this.editValue2;
            VlrSolicitadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrSolicitadoRecConsol);

            GridColumn CantPreCompradoREcConsol = new GridColumn();
            CantPreCompradoREcConsol.FieldName = this._unboundPrefix + "CantPreComprado";
            CantPreCompradoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPreComprado");
            CantPreCompradoREcConsol.UnboundType = UnboundColumnType.String;
            CantPreCompradoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPreCompradoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            CantPreCompradoREcConsol.AppearanceCell.BackColor = Color.Khaki;
            CantPreCompradoREcConsol.AppearanceCell.Options.UseBackColor = true;
            CantPreCompradoREcConsol.VisibleIndex = 10;
            CantPreCompradoREcConsol.Width = 60;
            CantPreCompradoREcConsol.Visible = true;
            CantPreCompradoREcConsol.ColumnEdit = this.editValue2Cant;
            CantPreCompradoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantPreCompradoREcConsol);

            GridColumn VlrPreComprado = new GridColumn();
            VlrPreComprado.FieldName = this._unboundPrefix + "VlrPreComprado";
            VlrPreComprado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrPreComprado");
            VlrPreComprado.UnboundType = UnboundColumnType.String;
            VlrPreComprado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPreComprado.AppearanceCell.Options.UseTextOptions = true;
            VlrPreComprado.AppearanceCell.BackColor = Color.Khaki;
            VlrPreComprado.AppearanceCell.Options.UseBackColor = true;
            VlrPreComprado.VisibleIndex = 11;
            VlrPreComprado.Width = 100;
            VlrPreComprado.Visible = true;
            VlrPreComprado.ColumnEdit = this.editValue2;
            VlrPreComprado.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrPreComprado);

            GridColumn CantCompradoREcConsol = new GridColumn();
            CantCompradoREcConsol.FieldName = this._unboundPrefix + "CantComprado";
            CantCompradoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantComprado");
            CantCompradoREcConsol.UnboundType = UnboundColumnType.String;
            CantCompradoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantCompradoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            CantCompradoREcConsol.AppearanceCell.BackColor = Color.LightGray;
            CantCompradoREcConsol.AppearanceCell.Options.UseBackColor = true;
            CantCompradoREcConsol.VisibleIndex = 12;
            CantCompradoREcConsol.Width = 80;
            CantCompradoREcConsol.Visible = true;
            CantCompradoREcConsol.ColumnEdit = this.editValue2Cant;
            CantCompradoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantCompradoREcConsol);

            GridColumn VlrCompradoRecConsol = new GridColumn();
            VlrCompradoRecConsol.FieldName = this._unboundPrefix + "VlrComprado";
            VlrCompradoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrComprado");
            VlrCompradoRecConsol.UnboundType = UnboundColumnType.String;
            VlrCompradoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrCompradoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrCompradoRecConsol.AppearanceCell.BackColor = Color.LightGray;
            VlrCompradoRecConsol.AppearanceCell.Options.UseBackColor = true;
            VlrCompradoRecConsol.VisibleIndex = 13;
            VlrCompradoRecConsol.Width = 100;
            VlrCompradoRecConsol.Visible = true;
            VlrCompradoRecConsol.ColumnEdit = this.editValue2;
            VlrCompradoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrCompradoRecConsol);

            GridColumn CantRecibidoREcConsol = new GridColumn();
            CantRecibidoREcConsol.FieldName = this._unboundPrefix + "CantRecibido";
            CantRecibidoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantRecibido");
            CantRecibidoREcConsol.UnboundType = UnboundColumnType.String;
            CantRecibidoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantRecibidoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            CantRecibidoREcConsol.AppearanceCell.BackColor = Color.Tan;
            CantRecibidoREcConsol.AppearanceCell.Options.UseBackColor = true;
            CantRecibidoREcConsol.VisibleIndex = 14;
            CantRecibidoREcConsol.Width = 120;
            CantRecibidoREcConsol.Visible = false;
            CantRecibidoREcConsol.ColumnEdit = this.editValue2Cant;
            CantRecibidoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantRecibidoREcConsol);

            GridColumn VlrRecibidoRecConsol = new GridColumn();
            VlrRecibidoRecConsol.FieldName = this._unboundPrefix + "VlrRecibido";
            VlrRecibidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrRecibido");
            VlrRecibidoRecConsol.UnboundType = UnboundColumnType.String;
            VlrRecibidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrRecibidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibidoRecConsol.AppearanceCell.BackColor = Color.Tan;
            VlrRecibidoRecConsol.AppearanceCell.Options.UseBackColor = true;
            VlrRecibidoRecConsol.VisibleIndex = 15;
            VlrRecibidoRecConsol.Width = 100;
            VlrRecibidoRecConsol.Visible = false;
            VlrRecibidoRecConsol.ColumnEdit = this.editValue2;
            VlrRecibidoRecConsol.OptionsColumn.AllowEdit = false; 
            this.gvRecurso.Columns.Add(VlrRecibidoRecConsol);

            GridColumn CantConsumidoRecConsol = new GridColumn();
            CantConsumidoRecConsol.FieldName = this._unboundPrefix + "CantConsumido";
            CantConsumidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantConsumido");
            CantConsumidoRecConsol.UnboundType = UnboundColumnType.String;
            CantConsumidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantConsumidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibidoRecConsol.AppearanceCell.BackColor = Color.PeachPuff;
            VlrRecibidoRecConsol.AppearanceCell.Options.UseBackColor = true;
            CantConsumidoRecConsol.VisibleIndex = 16;
            CantConsumidoRecConsol.Width = 80;
            CantConsumidoRecConsol.Visible = false;
            CantConsumidoRecConsol.ColumnEdit = this.editValue2Cant;
            CantConsumidoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantConsumidoRecConsol);

            GridColumn VlrConsumidoRecConsol = new GridColumn();
            VlrConsumidoRecConsol.FieldName = this._unboundPrefix + "VlrConsumido";
            VlrConsumidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrConsumido");
            VlrConsumidoRecConsol.UnboundType = UnboundColumnType.String;
            VlrConsumidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrConsumidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibidoRecConsol.AppearanceCell.BackColor = Color.PeachPuff;
            VlrRecibidoRecConsol.AppearanceCell.Options.UseBackColor = true;
            VlrConsumidoRecConsol.VisibleIndex = 17;
            VlrConsumidoRecConsol.Width = 100;
            VlrConsumidoRecConsol.Visible = false;
            VlrConsumidoRecConsol.ColumnEdit = this.editValue2;
            VlrConsumidoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrConsumidoRecConsol);

            GridColumn CantFacturadoRecConsol = new GridColumn();
            CantFacturadoRecConsol.FieldName = this._unboundPrefix + "CantFacturado";
            CantFacturadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantFacturado");
            CantFacturadoRecConsol.UnboundType = UnboundColumnType.Decimal;
            CantFacturadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantFacturadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantFacturadoRecConsol.VisibleIndex = 18;
            CantFacturadoRecConsol.Width = 80;
            CantFacturadoRecConsol.Visible = false;
            CantFacturadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantFacturadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantFacturadoRecConsol);     

            GridColumn VlrFacturadoRecConsol = new GridColumn();
            VlrFacturadoRecConsol.FieldName = this._unboundPrefix + "VlrFacturado";
            VlrFacturadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrFacturado");
            VlrFacturadoRecConsol.UnboundType = UnboundColumnType.Decimal;
            VlrFacturadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrFacturadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrFacturadoRecConsol.VisibleIndex = 19;
            VlrFacturadoRecConsol.Width = 100;
            VlrFacturadoRecConsol.Visible = false;
            VlrFacturadoRecConsol.ColumnEdit = this.editValue2;
            VlrFacturadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrFacturadoRecConsol);

            //Ver
            GridColumn fileConsol = new GridColumn();
            fileConsol.FieldName = this._unboundPrefix + "ViewDoc";
            fileConsol.OptionsColumn.ShowCaption = false;
            fileConsol.UnboundType = UnboundColumnType.String;
            fileConsol.Width = 60;
            fileConsol.ColumnEdit = this.editLink;
            fileConsol.VisibleIndex = 18;
            fileConsol.Visible = false;
            fileConsol.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(fileConsol);

            GridColumn TipoRecursoConsol = new GridColumn();
            TipoRecursoConsol.FieldName = this._unboundPrefix + "TipoRecurso";
            TipoRecursoConsol.UnboundType = UnboundColumnType.Integer;
            TipoRecursoConsol.Width = 80;
            TipoRecursoConsol.Visible = false;
            TipoRecursoConsol.Group();
            TipoRecursoConsol.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecursoConsol.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecurso.Columns.Add(TipoRecursoConsol);

            this.gvRecurso.OptionsView.ColumnAutoWidth = true;
            #endregion
            #region Grilla Tareas
            GridColumn TareaCliente = new GridColumn();
            TareaCliente.FieldName = this._unboundPrefix + "TareaCliente";
            TareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            TareaCliente.UnboundType = UnboundColumnType.String;
            TareaCliente.VisibleIndex = 0;
            TareaCliente.Width = 30;
            TareaCliente.Visible = true;
            TareaCliente.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaCliente);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this._unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 70;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaID);

            GridColumn Descriptivo = new GridColumn();
            Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
            Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            Descriptivo.UnboundType = UnboundColumnType.String;
            Descriptivo.VisibleIndex = 2;
            Descriptivo.Width = 250;
            Descriptivo.Visible = true;
            Descriptivo.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(Descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 3;
            UnidadInvID.Width = 35;
            UnidadInvID.Visible = true;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(UnidadInvID);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this._unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 4;
            Cantidad.Width = 40;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(Cantidad);

            this.gvTarea.OptionsView.ColumnAutoWidth = true;
            #endregion
            #region Grilla Recursos Det
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this._unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 1;
            RecursoID.Width = 100;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvRecursoTarea.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this._unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 2;
            RecursoDesc.Width = 200;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecursoTarea.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDRec = new GridColumn();
            UnidadInvIDRec.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvIDRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDRec.UnboundType = UnboundColumnType.String;
            UnidadInvIDRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvIDRec.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvIDRec.VisibleIndex = 3;
            UnidadInvIDRec.Width = 35;
            UnidadInvIDRec.Visible = true;
            UnidadInvIDRec.OptionsColumn.AllowEdit = false;
            this.gvRecursoTarea.Columns.Add(UnidadInvIDRec);

            GridColumn TipoRecurso = new GridColumn();
            TipoRecurso.FieldName = this._unboundPrefix + "TipoRecurso";
            TipoRecurso.UnboundType = UnboundColumnType.Integer;
            TipoRecurso.Width = 80;
            TipoRecurso.Visible = false;
            TipoRecurso.Group();
            TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecursoTarea.Columns.Add(TipoRecurso);

            this.gvRecursoTarea.OptionsView.ColumnAutoWidth = true;
            #endregion
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData(string recursoIDFilter, string proyectoID, bool selectTarea)
        {
            try
            {
                //Valida si solo es consulta o selecciona una tarea
                if (!selectTarea)
                {
                    List<DTO_QueryTrazabilidad> detaAll = new List<DTO_QueryTrazabilidad>();
                    List<DTO_QueryTrazabilidad> detaDistinct = new List<DTO_QueryTrazabilidad>();
                    foreach (DTO_QueryTrazabilidad traz in this.uc_Proyecto.ProyectoInfo.ResumenTrazabilidad)
                        detaAll.AddRange(traz.Detalle);

                    //Filtra por el recurso
                    detaAll = !string.IsNullOrEmpty(recursoIDFilter) ? detaAll.FindAll(x => x.RecursoID.Value == recursoIDFilter).ToList() : detaAll;
                    List<string> recursosDist = detaAll.Select(x => x.RecursoID.Value).Distinct().ToList();
                    foreach (string rec in recursosDist)
                    {
                        DTO_QueryTrazabilidad d = detaAll.Find(x => x.RecursoID.Value == rec);
                        d.Detalle = detaAll.FindAll(x => x.RecursoID.Value == rec);
                        d.CantPresupuestado.Value = d.Detalle.Sum(x => x.CantPresupuestado.Value);
                        d.CantSolicitado.Value = d.Detalle.Sum(x => x.CantSolicitado.Value);
                        d.CantComprado.Value = d.Detalle.Sum(x => x.CantComprado.Value);
                        d.CantPreComprado.Value = d.Detalle.Sum(x => x.CantPreComprado.Value);
                        d.CantRecibido.Value = d.Detalle.Sum(x => x.CantRecibido.Value);
                        d.CantConsumido.Value = d.Detalle.Sum(x => x.CantConsumido.Value);
                        d.CantFacturado.Value = d.Detalle.Sum(x => x.CantFacturado.Value);
                        d.VlrPresupuestado.Value = d.Detalle.Sum(x => x.VlrPresupuestado.Value);
                        d.VlrSolicitado.Value = d.Detalle.Sum(x => x.VlrSolicitado.Value);
                        d.VlrComprado.Value = d.Detalle.Sum(x => x.VlrComprado.Value);
                        d.VlrPreComprado.Value = d.Detalle.Sum(x => x.VlrPreComprado.Value);
                        d.VlrRecibido.Value = d.Detalle.Sum(x => x.VlrRecibido.Value);
                        d.VlrConsumido.Value = d.Detalle.Sum(x => x.VlrConsumido.Value);
                        d.VlrFacturado.Value = d.Detalle.Sum(x => x.VlrFacturado.Value);
                        detaDistinct.Add(d);
                    }
                    this.gcRecurso.DataSource = detaDistinct;
                    this.gcRecurso.RefreshDataSource();
                }
                else
                {
                    this.gcTarea.DataSource = this.uc_Proyecto.ProyectoInfo.DetalleProyecto;
                    this.gcTarea.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalProyectosMvto.cs", "ModalProyectosMvto(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }
    
        #endregion

        #region Eventos Controles

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {        
            this.Close();
        }

        /// <summary>
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucProyecto_LoadProyectoInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.uc_Proyecto.ProyectoInfo != null)
                {
                    if (this.uc_Proyecto.ProyectoInfo.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }                                
                    this.LoadGridData(string.Empty,string.Empty,false);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }
        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
        private void gvRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this._unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTarea_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.gvTarea.FocusedRowHandle >= 0)
                this._tareaCurrent = (DTO_pyProyectoTarea)this.gvTarea.GetRow(this.gvTarea.FocusedRowHandle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTarea_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvRecursoTarea_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }
        #endregion
    }
}
