namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QuerySaldosCierre
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterLibro = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLineaPresup = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.repositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.cmbRomp1 = new DevExpress.XtraEditors.LookUpEdit();
            this.lblRomp1 = new System.Windows.Forms.Label();
            this.lblRomp2 = new System.Windows.Forms.Label();
            this.cmbRomp2 = new DevExpress.XtraEditors.LookUpEdit();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.gcSaldos = new DevExpress.XtraGrid.GridControl();
            this.gvSaldos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterConceptoCargo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbTipoMoneda = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoMoneda = new System.Windows.Forms.Label();
            this.gbFilter = new DevExpress.XtraEditors.GroupControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRomp1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRomp2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSaldos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSaldos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMoneda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).BeginInit();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Empty.Options.UseFont = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.GridControl = this.gcDocument;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsDetail.EnableMasterViewMode = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvDetalle_RowClick);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
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
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(6, 26);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(561, 366);
            this.gcDocument.TabIndex = 51;
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
            this.gvDocument.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvDocument_RowClick);
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // masterLibro
            // 
            this.masterLibro.BackColor = System.Drawing.Color.Transparent;
            this.masterLibro.Filtros = null;
            this.masterLibro.Location = new System.Drawing.Point(241, 7);
            this.masterLibro.Name = "masterLibro";
            this.masterLibro.Size = new System.Drawing.Size(304, 25);
            this.masterLibro.TabIndex = 86;
            this.masterLibro.Value = "";
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(8, 4);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(323, 25);
            this.masterCuenta.TabIndex = 88;
            this.masterCuenta.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(342, 4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(323, 25);
            this.masterProyecto.TabIndex = 89;
            this.masterProyecto.Value = "";
            // 
            // masterLineaPresup
            // 
            this.masterLineaPresup.BackColor = System.Drawing.Color.Transparent;
            this.masterLineaPresup.Filtros = null;
            this.masterLineaPresup.Location = new System.Drawing.Point(342, 29);
            this.masterLineaPresup.Name = "masterLineaPresup";
            this.masterLineaPresup.Size = new System.Drawing.Size(301, 25);
            this.masterLineaPresup.TabIndex = 97;
            this.masterLineaPresup.Value = "";
            // 
            // repositoryEdit
            // 
            this.repositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkEditViewFile,
            this.TextEdit});
            // 
            // linkEditViewFile
            // 
            this.linkEditViewFile.Caption = "Find_Document";
            this.linkEditViewFile.Name = "linkEditViewFile";
            // 
            // TextEdit
            // 
            this.TextEdit.Mask.EditMask = "c2";
            this.TextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit.Mask.UseMaskAsDisplayFormat = true;
            this.TextEdit.Name = "TextEdit";
            // 
            // cmbRomp1
            // 
            this.cmbRomp1.Location = new System.Drawing.Point(134, 7);
            this.cmbRomp1.Name = "cmbRomp1";
            this.cmbRomp1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRomp1.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbRomp1.Properties.DisplayMember = "Value";
            this.cmbRomp1.Properties.NullText = " ";
            this.cmbRomp1.Properties.ValueMember = "Key";
            this.cmbRomp1.Size = new System.Drawing.Size(117, 20);
            this.cmbRomp1.TabIndex = 99;
            this.cmbRomp1.EditValueChanged += new System.EventHandler(this.cmbRomp1_EditValueChanged);
            // 
            // lblRomp1
            // 
            this.lblRomp1.AutoSize = true;
            this.lblRomp1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRomp1.Location = new System.Drawing.Point(5, 10);
            this.lblRomp1.Name = "lblRomp1";
            this.lblRomp1.Size = new System.Drawing.Size(127, 14);
            this.lblRomp1.TabIndex = 100;
            this.lblRomp1.Text = "20316_Rompimiento1";
            // 
            // lblRomp2
            // 
            this.lblRomp2.AutoSize = true;
            this.lblRomp2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRomp2.Location = new System.Drawing.Point(266, 10);
            this.lblRomp2.Name = "lblRomp2";
            this.lblRomp2.Size = new System.Drawing.Size(127, 14);
            this.lblRomp2.TabIndex = 102;
            this.lblRomp2.Text = "20316_Rompimiento2";
            // 
            // cmbRomp2
            // 
            this.cmbRomp2.Location = new System.Drawing.Point(369, 7);
            this.cmbRomp2.Name = "cmbRomp2";
            this.cmbRomp2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRomp2.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbRomp2.Properties.DisplayMember = "Value";
            this.cmbRomp2.Properties.NullText = " ";
            this.cmbRomp2.Properties.ValueMember = "Key";
            this.cmbRomp2.Size = new System.Drawing.Size(117, 20);
            this.cmbRomp2.TabIndex = 101;
            this.cmbRomp2.EditValueChanged += new System.EventHandler(this.cmbRomp2_EditValueChanged);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(75, 9);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(125, 18);
            this.dtPeriod.TabIndex = 103;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(8, 12);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 104;
            this.lblPeriod.Text = "20316_lblPeriod";
            // 
            // gcSaldos
            // 
            this.gcSaldos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcSaldos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcSaldos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcSaldos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcSaldos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcSaldos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcSaldos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcSaldos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcSaldos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcSaldos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcSaldos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcSaldos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcSaldos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcSaldos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcSaldos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcSaldos.Location = new System.Drawing.Point(575, 26);
            this.gcSaldos.LookAndFeel.SkinName = "Dark Side";
            this.gcSaldos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcSaldos.MainView = this.gvSaldos;
            this.gcSaldos.Margin = new System.Windows.Forms.Padding(4);
            this.gcSaldos.Name = "gcSaldos";
            this.gcSaldos.Size = new System.Drawing.Size(530, 294);
            this.gcSaldos.TabIndex = 51;
            this.gcSaldos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSaldos});
            // 
            // gvSaldos
            // 
            this.gvSaldos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSaldos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvSaldos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvSaldos.Appearance.Empty.Options.UseBackColor = true;
            this.gvSaldos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvSaldos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvSaldos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvSaldos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvSaldos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvSaldos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvSaldos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvSaldos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvSaldos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvSaldos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSaldos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvSaldos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvSaldos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvSaldos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvSaldos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvSaldos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvSaldos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvSaldos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvSaldos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvSaldos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvSaldos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSaldos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvSaldos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvSaldos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.Row.Options.UseBackColor = true;
            this.gvSaldos.Appearance.Row.Options.UseForeColor = true;
            this.gvSaldos.Appearance.Row.Options.UseTextOptions = true;
            this.gvSaldos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvSaldos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvSaldos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvSaldos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSaldos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvSaldos.GridControl = this.gcSaldos;
            this.gvSaldos.HorzScrollStep = 50;
            this.gvSaldos.Name = "gvSaldos";
            this.gvSaldos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvSaldos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvSaldos.OptionsCustomization.AllowColumnMoving = false;
            this.gvSaldos.OptionsCustomization.AllowFilter = false;
            this.gvSaldos.OptionsCustomization.AllowSort = false;
            this.gvSaldos.OptionsMenu.EnableColumnMenu = false;
            this.gvSaldos.OptionsMenu.EnableFooterMenu = false;
            this.gvSaldos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvSaldos.OptionsView.ColumnAutoWidth = false;
            this.gvSaldos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvSaldos.OptionsView.ShowGroupPanel = false;
            this.gvSaldos.PaintStyleName = "WindowsXP";
            this.gvSaldos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvSaldos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(7, 29);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(323, 25);
            this.masterTercero.TabIndex = 100;
            this.masterTercero.Value = "";
            // 
            // masterConceptoCargo
            // 
            this.masterConceptoCargo.BackColor = System.Drawing.Color.Transparent;
            this.masterConceptoCargo.Filtros = null;
            this.masterConceptoCargo.Location = new System.Drawing.Point(647, 4);
            this.masterConceptoCargo.Name = "masterConceptoCargo";
            this.masterConceptoCargo.Size = new System.Drawing.Size(302, 25);
            this.masterConceptoCargo.TabIndex = 99;
            this.masterConceptoCargo.Value = "";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(646, 29);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(303, 25);
            this.masterCentroCosto.TabIndex = 98;
            this.masterCentroCosto.Value = "";
            // 
            // cmbTipoMoneda
            // 
            this.cmbTipoMoneda.Location = new System.Drawing.Point(653, 9);
            this.cmbTipoMoneda.Name = "cmbTipoMoneda";
            this.cmbTipoMoneda.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoMoneda.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoMoneda.Properties.DisplayMember = "Value";
            this.cmbTipoMoneda.Properties.NullText = " ";
            this.cmbTipoMoneda.Properties.ValueMember = "Key";
            this.cmbTipoMoneda.Size = new System.Drawing.Size(103, 20);
            this.cmbTipoMoneda.TabIndex = 109;
            this.cmbTipoMoneda.EditValueChanged += new System.EventHandler(this.cmbTipoMoneda_EditValueChanged);
            // 
            // lblTipoMoneda
            // 
            this.lblTipoMoneda.AutoSize = true;
            this.lblTipoMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMoneda.Location = new System.Drawing.Point(561, 11);
            this.lblTipoMoneda.Name = "lblTipoMoneda";
            this.lblTipoMoneda.Size = new System.Drawing.Size(116, 14);
            this.lblTipoMoneda.TabIndex = 110;
            this.lblTipoMoneda.Text = "20316_TipoMoneda";
            // 
            // gbFilter
            // 
            this.gbFilter.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilter.AppearanceCaption.Options.UseFont = true;
            this.gbFilter.Controls.Add(this.panelControl3);
            this.gbFilter.Controls.Add(this.panelControl2);
            this.gbFilter.Controls.Add(this.panelControl1);
            this.gbFilter.Location = new System.Drawing.Point(12, 10);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(1118, 186);
            this.gbFilter.TabIndex = 109;
            this.gbFilter.Text = "Filtros";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.masterTercero);
            this.panelControl3.Controls.Add(this.masterCuenta);
            this.panelControl3.Controls.Add(this.masterConceptoCargo);
            this.panelControl3.Controls.Add(this.masterProyecto);
            this.panelControl3.Controls.Add(this.masterCentroCosto);
            this.panelControl3.Controls.Add(this.masterLineaPresup);
            this.panelControl3.Location = new System.Drawing.Point(16, 113);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1089, 59);
            this.panelControl3.TabIndex = 110;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lblRomp1);
            this.panelControl2.Controls.Add(this.cmbRomp1);
            this.panelControl2.Controls.Add(this.lblRomp2);
            this.panelControl2.Controls.Add(this.cmbRomp2);
            this.panelControl2.Location = new System.Drawing.Point(16, 75);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1089, 34);
            this.panelControl2.TabIndex = 109;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbTipoMoneda);
            this.panelControl1.Controls.Add(this.lblPeriod);
            this.panelControl1.Controls.Add(this.masterLibro);
            this.panelControl1.Controls.Add(this.dtPeriod);
            this.panelControl1.Controls.Add(this.lblTipoMoneda);
            this.panelControl1.Location = new System.Drawing.Point(16, 33);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1089, 36);
            this.panelControl1.TabIndex = 108;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.gcSaldos);
            this.groupControl1.Controls.Add(this.gcDocument);
            this.groupControl1.Location = new System.Drawing.Point(12, 206);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1118, 405);
            this.groupControl1.TabIndex = 110;
            this.groupControl1.Text = "Detalle";
            // 
            // QuerySaldosCierre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1190, 623);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.groupControl1);
            this.Name = "QuerySaldosCierre";
            this.Text = "20316";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRomp1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRomp2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSaldos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSaldos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMoneda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).EndInit();
            this.gbFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterLibro;
        private ControlsUC.uc_MasterFind masterCuenta;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterLineaPresup;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.Repository.PersistentRepository repositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit;
        private DevExpress.XtraEditors.LookUpEdit cmbRomp1;
        private System.Windows.Forms.Label lblRomp1;
        private System.Windows.Forms.Label lblRomp2;
        private DevExpress.XtraEditors.LookUpEdit cmbRomp2;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private ControlsUC.uc_MasterFind masterConceptoCargo;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        protected DevExpress.XtraGrid.GridControl gcSaldos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvSaldos;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoMoneda;
        private System.Windows.Forms.Label lblTipoMoneda;
        private ControlsUC.uc_MasterFind masterTercero;
        private DevExpress.XtraEditors.GroupControl gbFilter;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;





    }
}