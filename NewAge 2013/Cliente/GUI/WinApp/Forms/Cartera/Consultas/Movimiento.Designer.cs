namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Movimiento
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcGenerales = new DevExpress.XtraGrid.GridControl();
            this.gvGenerales = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.tc_QueryCreditos = new DevExpress.XtraTab.XtraTabControl();
            this.tp_DatosGenerales = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_TipoMovimiento = new System.Windows.Forms.Label();
            this.masterTipoMovimiento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDateHasta = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaHasta = new DevExpress.XtraEditors.DateEdit();
            this.lblDateDe = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaDe = new DevExpress.XtraEditors.DateEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).BeginInit();
            this.tc_QueryCreditos.SuspendLayout();
            this.tp_DatosGenerales.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDe.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDe.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.gvDetalle.GridControl = this.gcGenerales;
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
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGenerales_CustomUnboundColumnData);
            // 
            // gcGenerales
            // 
            this.gcGenerales.AllowDrop = true;
            this.gcGenerales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcGenerales.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcGenerales.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcGenerales.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcGenerales.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcGenerales.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcGenerales.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcGenerales.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcGenerales.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcGenerales.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode2.RelationName = "Level1";
            gridLevelNode1.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            gridLevelNode1.RelationName = "Detalle";
            this.gcGenerales.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcGenerales.Location = new System.Drawing.Point(3, 15);
            this.gcGenerales.LookAndFeel.SkinName = "Dark Side";
            this.gcGenerales.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcGenerales.MainView = this.gvGenerales;
            this.gcGenerales.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcGenerales.Name = "gcGenerales";
            this.gcGenerales.Size = new System.Drawing.Size(867, 292);
            this.gcGenerales.TabIndex = 0;
            this.gcGenerales.UseEmbeddedNavigator = true;
            this.gcGenerales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGenerales,
            this.gvDetalle});
            // 
            // gvGenerales
            // 
            this.gvGenerales.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGenerales.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvGenerales.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.Empty.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvGenerales.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvGenerales.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvGenerales.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvGenerales.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGenerales.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvGenerales.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvGenerales.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvGenerales.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvGenerales.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvGenerales.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvGenerales.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.Row.Options.UseBackColor = true;
            this.gvGenerales.Appearance.Row.Options.UseForeColor = true;
            this.gvGenerales.Appearance.Row.Options.UseTextOptions = true;
            this.gvGenerales.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvGenerales.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvGenerales.Appearance.VertLine.Options.UseBackColor = true;
            this.gvGenerales.GridControl = this.gcGenerales;
            this.gvGenerales.HorzScrollStep = 50;
            this.gvGenerales.Name = "gvGenerales";
            this.gvGenerales.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvGenerales.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvGenerales.OptionsCustomization.AllowColumnMoving = false;
            this.gvGenerales.OptionsCustomization.AllowFilter = false;
            this.gvGenerales.OptionsCustomization.AllowSort = false;
            this.gvGenerales.OptionsMenu.EnableColumnMenu = false;
            this.gvGenerales.OptionsMenu.EnableFooterMenu = false;
            this.gvGenerales.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvGenerales.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvGenerales.OptionsView.ShowGroupPanel = false;
            this.gvGenerales.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvGenerales.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvGenerales_CustomRowCellEdit);
            this.gvGenerales.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvGenerales_FocusedRowChanged);
            this.gvGenerales.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvGenerales_BeforeLeaveRow);
            this.gvGenerales.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGenerales_CustomUnboundColumnData);
            this.gvGenerales.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvGenerales_CustomColumnDisplayText);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editCheck,
            this.linkEditViewFile});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "C2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editCheck
            // 
            this.editCheck.DisplayValueChecked = "True";
            this.editCheck.DisplayValueUnchecked = "False";
            this.editCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editCheck.Name = "editCheck";
            this.editCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // linkEditViewFile
            // 
            this.linkEditViewFile.Caption = "Find_Document";
            this.linkEditViewFile.Name = "linkEditViewFile";
            this.linkEditViewFile.Click += new System.EventHandler(this.linkEditViewFile_Click);
            // 
            // tc_QueryCreditos
            // 
            this.tc_QueryCreditos.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tc_QueryCreditos.AppearancePage.Header.Options.UseFont = true;
            this.tc_QueryCreditos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_QueryCreditos.Location = new System.Drawing.Point(0, 0);
            this.tc_QueryCreditos.Margin = new System.Windows.Forms.Padding(1);
            this.tc_QueryCreditos.Name = "tc_QueryCreditos";
            this.tc_QueryCreditos.SelectedTabPage = this.tp_DatosGenerales;
            this.tc_QueryCreditos.Size = new System.Drawing.Size(881, 482);
            this.tc_QueryCreditos.TabIndex = 2;
            this.tc_QueryCreditos.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tp_DatosGenerales});
            this.tc_QueryCreditos.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tc_QueryCreditos_SelectedPageChanged);
            // 
            // tp_DatosGenerales
            // 
            this.tp_DatosGenerales.Controls.Add(this.groupBox2);
            this.tp_DatosGenerales.Controls.Add(this.groupBox1);
            this.tp_DatosGenerales.Margin = new System.Windows.Forms.Padding(1);
            this.tp_DatosGenerales.Name = "tp_DatosGenerales";
            this.tp_DatosGenerales.Size = new System.Drawing.Size(875, 453);
            this.tp_DatosGenerales.Text = "32311_tp_DatosGenerales";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.gbGridDocument);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 128);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(875, 325);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "32316_InfoDetalle";
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gcGenerales);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(1, 16);
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(1);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.gbGridDocument.Size = new System.Drawing.Size(873, 308);
            this.gbGridDocument.TabIndex = 0;
            this.gbGridDocument.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.lbl_TipoMovimiento);
            this.groupBox1.Controls.Add(this.masterTipoMovimiento);
            this.groupBox1.Controls.Add(this.lblDateHasta);
            this.groupBox1.Controls.Add(this.dtFechaHasta);
            this.groupBox1.Controls.Add(this.lblDateDe);
            this.groupBox1.Controls.Add(this.dtFechaDe);
            this.groupBox1.Controls.Add(this.masterCliente);
            this.groupBox1.Controls.Add(this.txtLibranza);
            this.groupBox1.Controls.Add(this.lblLibranza);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(875, 128);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32316_InfoGeneral";
            // 
            // lbl_TipoMovimiento
            // 
            this.lbl_TipoMovimiento.AutoSize = true;
            this.lbl_TipoMovimiento.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TipoMovimiento.Location = new System.Drawing.Point(50, 89);
            this.lbl_TipoMovimiento.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lbl_TipoMovimiento.Name = "lbl_TipoMovimiento";
            this.lbl_TipoMovimiento.Size = new System.Drawing.Size(98, 16);
            this.lbl_TipoMovimiento.TabIndex = 58;
            this.lbl_TipoMovimiento.Text = "32316_TipoMov";
            // 
            // masterTipoMovimiento
            // 
            this.masterTipoMovimiento.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoMovimiento.Filtros = null;
            this.masterTipoMovimiento.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterTipoMovimiento.Location = new System.Drawing.Point(53, 83);
            this.masterTipoMovimiento.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterTipoMovimiento.Name = "masterTipoMovimiento";
            this.masterTipoMovimiento.Size = new System.Drawing.Size(350, 25);
            this.masterTipoMovimiento.TabIndex = 2;
            this.masterTipoMovimiento.Value = "";
            this.masterTipoMovimiento.Leave += new System.EventHandler(this.masterTipoMovimiento_Leave);
            // 
            // lblDateHasta
            // 
            this.lblDateHasta.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateHasta.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDateHasta.Location = new System.Drawing.Point(446, 81);
            this.lblDateHasta.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblDateHasta.Name = "lblDateHasta";
            this.lblDateHasta.Size = new System.Drawing.Size(109, 14);
            this.lblDateHasta.TabIndex = 27;
            this.lblDateHasta.Text = "32316_lblDateHasta";
            // 
            // dtFechaHasta
            // 
            this.dtFechaHasta.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaHasta.Location = new System.Drawing.Point(563, 78);
            this.dtFechaHasta.Name = "dtFechaHasta";
            this.dtFechaHasta.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaHasta.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaHasta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaHasta.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaHasta.Properties.Appearance.Options.UseFont = true;
            this.dtFechaHasta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaHasta.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaHasta.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaHasta.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaHasta.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaHasta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaHasta.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaHasta.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaHasta.Size = new System.Drawing.Size(117, 20);
            this.dtFechaHasta.TabIndex = 4;
            // 
            // lblDateDe
            // 
            this.lblDateDe.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateDe.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDateDe.Location = new System.Drawing.Point(447, 38);
            this.lblDateDe.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblDateDe.Name = "lblDateDe";
            this.lblDateDe.Size = new System.Drawing.Size(94, 14);
            this.lblDateDe.TabIndex = 25;
            this.lblDateDe.Text = "32316_lblDateDe";
            // 
            // dtFechaDe
            // 
            this.dtFechaDe.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaDe.Location = new System.Drawing.Point(563, 35);
            this.dtFechaDe.Name = "dtFechaDe";
            this.dtFechaDe.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaDe.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDe.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaDe.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDe.Properties.Appearance.Options.UseFont = true;
            this.dtFechaDe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDe.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDe.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDe.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDe.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDe.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDe.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDe.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDe.Size = new System.Drawing.Size(117, 20);
            this.dtFechaDe.TabIndex = 3;
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(54, 26);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(350, 25);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(171, 56);
            this.txtLibranza.Margin = new System.Windows.Forms.Padding(1);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(86, 22);
            this.txtLibranza.TabIndex = 1;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(50, 59);
            this.lblLibranza.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(98, 16);
            this.lblLibranza.TabIndex = 4;
            this.lblLibranza.Text = "32316_Libranza";
            // 
            // Movimiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 482);
            this.Controls.Add(this.tc_QueryCreditos);
            this.Name = "Movimiento";
            this.Text = "ShowDocumentForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).EndInit();
            this.tc_QueryCreditos.ResumeLayout(false);
            this.tp_DatosGenerales.ResumeLayout(false);
            this.tp_DatosGenerales.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDe.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDe.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private DevExpress.XtraTab.XtraTabControl tc_QueryCreditos;
        private DevExpress.XtraTab.XtraTabPage tp_DatosGenerales;
        private System.Windows.Forms.GroupBox groupBox1;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.GroupBox groupBox2;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcGenerales;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvGenerales;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        private DevExpress.XtraEditors.LabelControl lblDateHasta;
        protected DevExpress.XtraEditors.DateEdit dtFechaHasta;
        private DevExpress.XtraEditors.LabelControl lblDateDe;
        protected DevExpress.XtraEditors.DateEdit dtFechaDe;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private ControlsUC.uc_MasterFind masterTipoMovimiento;
        private System.Windows.Forms.Label lbl_TipoMovimiento;
    }
}