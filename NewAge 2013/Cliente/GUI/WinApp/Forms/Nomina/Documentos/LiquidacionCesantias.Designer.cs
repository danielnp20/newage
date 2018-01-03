using NewAge.Librerias.Project;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class LiquidacionCesantias
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.conceptoNOIDColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.conceptoNODescColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.diasColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.baseColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valorColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.fechaCorteCesantiasColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.fechaPagoCesantiasColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valorCesantiasColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valorInteresesCesantiasColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.resolucionColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.estadoColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.uc_Empleados = new NewAge.Cliente.GUI.WinApp.Forms.UC_Empleados();
            this.grGrilla = new System.Windows.Forms.GroupBox();
            this.rbtTipoLiquidacion = new DevExpress.XtraEditors.RadioGroup();
            this.txtResolucion = new DevExpress.XtraEditors.TextEdit();
            this.lblResolucion = new System.Windows.Forms.Label();
            this.lbl_fechaPago = new System.Windows.Forms.Label();
            this.dtFechaPago = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.gbGridDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            this.grGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbtTipoLiquidacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResolucion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editValue,
            this.editValue4});
            // 
            // editCmb
            // 
            this.editCmb.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.editCmb.AppearanceDropDown.Options.UseBackColor = true;
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.dtFechaPago);
            this.gbGridDocument.Controls.Add(this.lbl_fechaPago);
            this.gbGridDocument.Controls.Add(this.lblResolucion);
            this.gbGridDocument.Controls.Add(this.txtResolucion);
            this.gbGridDocument.Controls.Add(this.rbtTipoLiquidacion);
            this.gbGridDocument.Controls.Add(this.grGrilla);
            this.gbGridDocument.Controls.Add(this.uc_Empleados);
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.conceptoNOIDColumn,
            this.conceptoNODescColumn,
            this.diasColumn,
            this.baseColumn,
            this.valorColumn});
            this.gvDetalle.GridControl = this.gcDocument;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetalle_CustomRowCellEdit);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
            this.gvDetalle.CellValueChanged += gvDetalle_CellValueChanged;
            // 
            // conceptoNOIDColumn
            // 
            this.conceptoNOIDColumn.Name = "conceptoNOIDColumn";
            this.conceptoNOIDColumn.OptionsColumn.AllowEdit = false;
            this.conceptoNOIDColumn.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.conceptoNOIDColumn.Visible = true;
            this.conceptoNOIDColumn.VisibleIndex = 0;
            this.conceptoNOIDColumn.Width = 100;
            // 
            // conceptoNODescColumn
            // 
            this.conceptoNODescColumn.Name = "conceptoNODescColumn";
            this.conceptoNODescColumn.OptionsColumn.AllowEdit = false;
            this.conceptoNODescColumn.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.conceptoNODescColumn.Visible = true;
            this.conceptoNODescColumn.VisibleIndex = 1;
            this.conceptoNODescColumn.Width = 200;
            // 
            // diasColumn
            // 
            this.diasColumn.Name = "diasColumn";
            this.diasColumn.OptionsColumn.AllowEdit = false;
            this.diasColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.diasColumn.Visible = true;
            this.diasColumn.VisibleIndex = 2;
            this.diasColumn.Width = 100;
            // 
            // baseColumn
            // 
            this.baseColumn.Name = "baseColumn";
            this.baseColumn.OptionsColumn.AllowEdit = false;
            this.baseColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.baseColumn.Visible = true;
            this.baseColumn.VisibleIndex = 3;
            this.baseColumn.Width = 150;
            this.baseColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.baseColumn.AppearanceCell.Options.UseTextOptions = true;
            this.baseColumn.ColumnEdit = this.editValue;
            // 
            // valorColumn
            // 
            this.valorColumn.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.valorColumn.AppearanceCell.Options.UseBackColor = true;
            this.valorColumn.Name = "valorColumn";
            this.valorColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.valorColumn.Visible = true;
            this.valorColumn.VisibleIndex = 4;
            this.valorColumn.Width = 150;
            this.valorColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.valorColumn.AppearanceCell.Options.UseTextOptions = true;
            this.valorColumn.ColumnEdit = this.editValue;
            // 
            // gcDocument
            // 
            this.gcDocument.AllowDrop = true;
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(3, 17);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1096, 284);
            this.gcDocument.TabIndex = 0;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocument.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocument.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocument.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocument.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocument.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocument.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocument.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.fechaCorteCesantiasColumn,
            this.fechaPagoCesantiasColumn,
            this.valorCesantiasColumn,
            this.valorInteresesCesantiasColumn,
            this.resolucionColumn,
            this.estadoColumn});
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.HorzScrollStep = 50;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocument.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // fechaCorteCesantiasColumn
            // 
            this.fechaCorteCesantiasColumn.Name = "fechaCorteCesantiasColumn";
            this.fechaCorteCesantiasColumn.OptionsColumn.AllowEdit = false;
            this.fechaCorteCesantiasColumn.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.fechaCorteCesantiasColumn.Visible = true;
            this.fechaCorteCesantiasColumn.VisibleIndex = 0;
            this.fechaCorteCesantiasColumn.Width = 200;
            // 
            // fechaPagoCesantiasColumn
            // 
            this.fechaPagoCesantiasColumn.Name = "fechaPagoCesantiasColumn";
            this.fechaPagoCesantiasColumn.OptionsColumn.AllowEdit = false;
            this.fechaPagoCesantiasColumn.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.fechaPagoCesantiasColumn.Visible = true;
            this.fechaPagoCesantiasColumn.VisibleIndex = 1;
            this.fechaPagoCesantiasColumn.Width = 200;
            // 
            // valorCesantiasColumn
            // 
            this.valorCesantiasColumn.Name = "valorCesantiasColumn";
            this.valorCesantiasColumn.OptionsColumn.AllowEdit = false;
            this.valorCesantiasColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.valorCesantiasColumn.Visible = true;
            this.valorCesantiasColumn.VisibleIndex = 2;
            this.valorCesantiasColumn.Width = 200;
            // 
            // valorInteresesCesantiasColumn
            // 
            this.valorInteresesCesantiasColumn.Name = "valorInteresesCesantiasColumn";
            this.valorInteresesCesantiasColumn.OptionsColumn.AllowEdit = false;
            this.valorInteresesCesantiasColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.valorInteresesCesantiasColumn.Visible = true;
            this.valorInteresesCesantiasColumn.VisibleIndex = 3;
            this.valorInteresesCesantiasColumn.Width = 200;
            // 
            // resolucionColumn
            // 
            this.resolucionColumn.Name = "resolucionColumn";
            this.resolucionColumn.OptionsColumn.AllowEdit = false;
            this.resolucionColumn.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.resolucionColumn.Visible = true;
            this.resolucionColumn.VisibleIndex = 4;
            this.resolucionColumn.Width = 200;
            // 
            // estadoColumn
            // 
            this.estadoColumn.Name = "estadoColumn";
            this.estadoColumn.OptionsColumn.AllowEdit = false;
            this.estadoColumn.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.estadoColumn.Visible = true;
            this.estadoColumn.VisibleIndex = 5;
            // 
            // uc_Empleados
            // 
            this.uc_Empleados.EmpActivos = 1;
            this.uc_Empleados.IsMultipleSeleccion = true;
            this.uc_Empleados.Location = new System.Drawing.Point(6, 9);
            this.uc_Empleados.Name = "uc_Empleados";
            this.uc_Empleados.Size = new System.Drawing.Size(810, 239);
            this.uc_Empleados.TabIndex = 0;
            this.uc_Empleados.UseFilters = false;
            // 
            // grGrilla
            // 
            this.grGrilla.Controls.Add(this.gcDocument);
            this.grGrilla.Location = new System.Drawing.Point(5, 243);
            this.grGrilla.Name = "grGrilla";
            this.grGrilla.Size = new System.Drawing.Size(1102, 304);
            this.grGrilla.TabIndex = 1;
            this.grGrilla.TabStop = false;
            // 
            // rbtTipoLiquidacion
            // 
            this.rbtTipoLiquidacion.Location = new System.Drawing.Point(822, 37);
            this.rbtTipoLiquidacion.Name = "rbtTipoLiquidacion";
            this.rbtTipoLiquidacion.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rbtTipoLiquidacion.Properties.Appearance.Options.UseBackColor = true;
            this.rbtTipoLiquidacion.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Anual"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Parcial")});
            this.rbtTipoLiquidacion.Size = new System.Drawing.Size(282, 60);
            this.rbtTipoLiquidacion.TabIndex = 2;
            this.rbtTipoLiquidacion.SelectedIndexChanged += new System.EventHandler(this.rbtTipoLiquidacion_SelectedIndexChanged);
            // 
            // txtResolucion
            // 
            this.txtResolucion.Location = new System.Drawing.Point(1007, 71);
            this.txtResolucion.Name = "txtResolucion";
            this.txtResolucion.Size = new System.Drawing.Size(89, 20);
            this.txtResolucion.TabIndex = 3;
            this.txtResolucion.Visible = false;
            // 
            // lblResolucion
            // 
            this.lblResolucion.AutoSize = true;
            this.lblResolucion.Location = new System.Drawing.Point(915, 74);
            this.lblResolucion.Name = "lblResolucion";
            this.lblResolucion.Size = new System.Drawing.Size(86, 13);
            this.lblResolucion.TabIndex = 4;
            this.lblResolucion.Text = "85_Resolucion";
            this.lblResolucion.Visible = false;
            // 
            // lbl_fechaPago
            // 
            this.lbl_fechaPago.AutoSize = true;
            this.lbl_fechaPago.Location = new System.Drawing.Point(914, 50);
            this.lbl_fechaPago.Name = "lbl_fechaPago";
            this.lbl_fechaPago.Size = new System.Drawing.Size(76, 13);
            this.lbl_fechaPago.TabIndex = 5;
            this.lbl_fechaPago.Text = "85_fechaPago";
            this.lbl_fechaPago.Visible = true;
            // 
            // dtFechaPago
            // 
            this.dtFechaPago.EditValue = null;
            this.dtFechaPago.Location = new System.Drawing.Point(1007, 48);
            this.dtFechaPago.Name = "dtFechaPago";
            this.dtFechaPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaPago.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaPago.Size = new System.Drawing.Size(89, 20);
            this.dtFechaPago.TabIndex = 6;
            // 
            // LiquidacionCesantias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "LiquidacionCesantias";
            this.Text = "Liquidacion de Cesantias";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.gbGridDocument.ResumeLayout(false);
            this.gbGridDocument.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            this.grGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rbtTipoLiquidacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResolucion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.GroupBox grGrilla;
        private DevExpress.XtraGrid.GridControl gcDocument;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        private UC_Empleados uc_Empleados;
        private DevExpress.XtraEditors.RadioGroup rbtTipoLiquidacion;
        public DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private System.Windows.Forms.Label lblResolucion;
        private DevExpress.XtraEditors.TextEdit txtResolucion;
        private DevExpress.XtraGrid.Columns.GridColumn conceptoNOIDColumn;
        private DevExpress.XtraGrid.Columns.GridColumn conceptoNODescColumn;
        private DevExpress.XtraGrid.Columns.GridColumn diasColumn;
        private DevExpress.XtraGrid.Columns.GridColumn baseColumn;
        private DevExpress.XtraGrid.Columns.GridColumn valorColumn;
        private DevExpress.XtraGrid.Columns.GridColumn fechaCorteCesantiasColumn;
        private DevExpress.XtraGrid.Columns.GridColumn fechaPagoCesantiasColumn;
        private DevExpress.XtraGrid.Columns.GridColumn valorCesantiasColumn;
        private DevExpress.XtraGrid.Columns.GridColumn valorInteresesCesantiasColumn;
        private DevExpress.XtraGrid.Columns.GridColumn resolucionColumn;
        private DevExpress.XtraGrid.Columns.GridColumn estadoColumn;
        private DevExpress.XtraEditors.DateEdit dtFechaPago;
        private System.Windows.Forms.Label lbl_fechaPago;

    }
}