namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ContabilizarNomina
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
        private void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode4 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode5 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode6 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvAportes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvProvisiones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtProceso = new DevExpress.XtraEditors.RadioGroup();
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
            ((System.ComponentModel.ISupportInitialize)(this.gvAportes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProvisiones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbtProceso.Properties)).BeginInit();
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
            this.gbGridDocument.Controls.Add(this.rbtProceso);
            this.gbGridDocument.Controls.Add(this.groupBox1);
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
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetalle_CustomRowCellEdit);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
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
            gridLevelNode4.LevelTemplate = this.gvDetalle;
            gridLevelNode4.RelationName = "Detalle";
            gridLevelNode5.LevelTemplate = this.gvAportes;
            gridLevelNode5.RelationName = "Aportes";
            gridLevelNode6.LevelTemplate = this.gvProvisiones;
            gridLevelNode6.RelationName = "Provisiones";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode4,
            gridLevelNode5,
            gridLevelNode6});
            this.gcDocument.Location = new System.Drawing.Point(3, 17);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1104, 466);
            this.gcDocument.TabIndex = 0;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAportes,
            this.gvProvisiones,
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvAportes
            // 
            this.gvAportes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvAportes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvAportes.Appearance.Empty.Options.UseBackColor = true;
            this.gvAportes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvAportes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvAportes.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAportes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvAportes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvAportes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvAportes.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAportes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvAportes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvAportes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvAportes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvAportes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvAportes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvAportes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvAportes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gvAportes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvAportes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvAportes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvAportes.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvAportes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvAportes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvAportes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvAportes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvAportes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAportes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvAportes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvAportes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvAportes.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvAportes.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvAportes.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvAportes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvAportes.Appearance.Row.Options.UseBackColor = true;
            this.gvAportes.Appearance.Row.Options.UseForeColor = true;
            this.gvAportes.Appearance.Row.Options.UseTextOptions = true;
            this.gvAportes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvAportes.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAportes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvAportes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvAportes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvAportes.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvAportes.Appearance.VertLine.Options.UseBackColor = true;
            this.gvAportes.GridControl = this.gcDocument;
            this.gvAportes.HorzScrollStep = 50;
            this.gvAportes.Name = "gvAportes";
            this.gvAportes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvAportes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvAportes.OptionsCustomization.AllowColumnMoving = false;
            this.gvAportes.OptionsCustomization.AllowFilter = false;
            this.gvAportes.OptionsCustomization.AllowSort = false;
            this.gvAportes.OptionsMenu.EnableColumnMenu = false;
            this.gvAportes.OptionsMenu.EnableFooterMenu = false;
            this.gvAportes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvAportes.OptionsView.ColumnAutoWidth = false;
            this.gvAportes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvAportes.OptionsView.ShowGroupPanel = false;
            this.gvAportes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvAportes.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvAportes_CustomRowCellEdit);
            this.gvAportes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvAportes_CustomUnboundColumnData);
            // 
            // gvProvisiones
            // 
            this.gvProvisiones.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProvisiones.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvProvisiones.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvProvisiones.Appearance.Empty.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvProvisiones.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvisiones.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvProvisiones.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvisiones.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvisiones.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvisiones.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvProvisiones.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvProvisiones.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProvisiones.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvProvisiones.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvProvisiones.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvProvisiones.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvProvisiones.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvProvisiones.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvProvisiones.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvisiones.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvProvisiones.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProvisiones.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvProvisiones.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvProvisiones.Appearance.Row.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.Row.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.Row.Options.UseTextOptions = true;
            this.gvProvisiones.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvProvisiones.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvisiones.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvProvisiones.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvisiones.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvProvisiones.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProvisiones.Appearance.VertLine.Options.UseBackColor = true;
            this.gvProvisiones.GridControl = this.gcDocument;
            this.gvProvisiones.HorzScrollStep = 50;
            this.gvProvisiones.Name = "gvProvisiones";
            this.gvProvisiones.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvProvisiones.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvProvisiones.OptionsCustomization.AllowColumnMoving = false;
            this.gvProvisiones.OptionsCustomization.AllowFilter = false;
            this.gvProvisiones.OptionsCustomization.AllowSort = false;
            this.gvProvisiones.OptionsMenu.EnableColumnMenu = false;
            this.gvProvisiones.OptionsMenu.EnableFooterMenu = false;
            this.gvProvisiones.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvProvisiones.OptionsView.ColumnAutoWidth = false;
            this.gvProvisiones.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvProvisiones.OptionsView.ShowGroupPanel = false;
            this.gvProvisiones.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvProvisiones.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvProvisiones_CustomRowCellEdit);
            this.gvProvisiones.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvProvisiones_CustomUnboundColumnData);
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
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcDocument);
            this.groupBox1.Location = new System.Drawing.Point(3, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1110, 486);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // rbtProceso
            // 
            this.rbtProceso.EditValue = true;
            this.rbtProceso.Location = new System.Drawing.Point(16, 17);
            this.rbtProceso.Name = "rbtProceso";
            this.rbtProceso.Properties.AllowMouseWheel = false;
            this.rbtProceso.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rbtProceso.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.rbtProceso.Properties.Appearance.Options.UseBackColor = true;
            this.rbtProceso.Properties.Appearance.Options.UseFont = true;
            this.rbtProceso.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rbtProceso.Properties.Columns = 4;
            this.rbtProceso.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Todos"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Nómina"),
             new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Provisiones"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Planilla") });
            this.rbtProceso.Size = new System.Drawing.Size(505, 23);
            this.rbtProceso.TabIndex = 5;
            this.rbtProceso.SelectedIndexChanged += new System.EventHandler(rbtProceso_SelectIndex);
            // 
            // ContabilizarNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "ContabilizarNomina";
            this.Text = "ContabilizarNomina";
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
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAportes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProvisiones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rbtProceso.Properties)).EndInit();
            this.ResumeLayout(false);

        }        

        #endregion

        private DevExpress.XtraGrid.GridControl gcDocument;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAportes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProvisiones;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.RadioGroup rbtProceso;
    }
}