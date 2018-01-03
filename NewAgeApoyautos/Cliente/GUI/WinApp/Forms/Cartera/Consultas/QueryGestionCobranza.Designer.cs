namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QueryGestionCobranza
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
            this.gvCuotas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcCobranzas = new DevExpress.XtraGrid.GridControl();
            this.gvCobranzas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editCant = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tc_QueryCreditos = new DevExpress.XtraTab.XtraTabControl();
            this.tp_DatosCartera = new DevExpress.XtraTab.XtraTabPage();
            this.pnlHeader = new System.Windows.Forms.GroupBox();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaInicial = new DevExpress.XtraEditors.DateEdit();
            this.pnlGrid = new System.Windows.Forms.GroupBox();
            this.lblFechaFin = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaInicial = new DevExpress.XtraEditors.LabelControl();
            this.masterEtapa = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCobranzas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCobranzas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).BeginInit();
            this.tc_QueryCreditos.SuspendLayout();
            this.tp_DatosCartera.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvCuotas
            // 
            this.gvCuotas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuotas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCuotas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.Empty.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCuotas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCuotas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCuotas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCuotas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuotas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCuotas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCuotas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCuotas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCuotas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuotas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCuotas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.Row.Options.UseBackColor = true;
            this.gvCuotas.Appearance.Row.Options.UseForeColor = true;
            this.gvCuotas.Appearance.Row.Options.UseTextOptions = true;
            this.gvCuotas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCuotas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuotas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCuotas.GridControl = this.gcCobranzas;
            this.gvCuotas.HorzScrollStep = 50;
            this.gvCuotas.Name = "gvCuotas";
            this.gvCuotas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvCuotas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCuotas.OptionsCustomization.AllowColumnMoving = false;
            this.gvCuotas.OptionsCustomization.AllowFilter = false;
            this.gvCuotas.OptionsCustomization.AllowSort = false;
            this.gvCuotas.OptionsMenu.EnableColumnMenu = false;
            this.gvCuotas.OptionsMenu.EnableFooterMenu = false;
            this.gvCuotas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCuotas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvCuotas.OptionsView.ShowGroupPanel = false;
            this.gvCuotas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvCuotas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvLibranzas_CustomUnboundColumnData);
            // 
            // gcCobranzas
            // 
            this.gcCobranzas.AllowDrop = true;
            this.gcCobranzas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCobranzas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCobranzas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCobranzas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCobranzas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCobranzas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCobranzas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCobranzas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCobranzas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCobranzas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcCobranzas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcCobranzas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCobranzas.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcCobranzas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCobranzas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvCuotas;
            gridLevelNode1.RelationName = "Cuotas";
            this.gcCobranzas.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcCobranzas.Location = new System.Drawing.Point(3, 15);
            this.gcCobranzas.LookAndFeel.SkinName = "Dark Side";
            this.gcCobranzas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCobranzas.MainView = this.gvCobranzas;
            this.gcCobranzas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcCobranzas.Name = "gcCobranzas";
            this.gcCobranzas.Size = new System.Drawing.Size(1095, 458);
            this.gcCobranzas.TabIndex = 0;
            this.gcCobranzas.UseEmbeddedNavigator = true;
            this.gcCobranzas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCobranzas,
            this.gvCuotas});
            // 
            // gvCobranzas
            // 
            this.gvCobranzas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCobranzas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCobranzas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCobranzas.Appearance.Empty.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCobranzas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCobranzas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCobranzas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCobranzas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCobranzas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvCobranzas.Appearance.FooterPanel.Options.UseFont = true;
            this.gvCobranzas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvCobranzas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCobranzas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCobranzas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCobranzas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCobranzas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCobranzas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCobranzas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCobranzas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCobranzas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCobranzas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCobranzas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCobranzas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCobranzas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCobranzas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCobranzas.Appearance.Row.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.Row.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.Row.Options.UseTextOptions = true;
            this.gvCobranzas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCobranzas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCobranzas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCobranzas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCobranzas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCobranzas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCobranzas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCobranzas.GridControl = this.gcCobranzas;
            this.gvCobranzas.GroupFormat = "[#image]{1} {2}";
            this.gvCobranzas.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_VlrCapital", null, "{0:c2}")});
            this.gvCobranzas.HorzScrollStep = 50;
            this.gvCobranzas.Name = "gvCobranzas";
            this.gvCobranzas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvCobranzas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCobranzas.OptionsCustomization.AllowColumnMoving = false;
            this.gvCobranzas.OptionsCustomization.AllowFilter = false;
            this.gvCobranzas.OptionsCustomization.AllowSort = false;
            this.gvCobranzas.OptionsMenu.EnableColumnMenu = false;
            this.gvCobranzas.OptionsMenu.EnableFooterMenu = false;
            this.gvCobranzas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCobranzas.OptionsView.ShowAutoFilterRow = true;
            this.gvCobranzas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvCobranzas.OptionsView.ShowGroupPanel = false;
            this.gvCobranzas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvCobranzas.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCobranzas_FocusedRowChanged);
            this.gvCobranzas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvLibranzas_CustomUnboundColumnData);
            this.gvCobranzas.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvCobranzas_CustomColumnDisplayText);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editCheck,
            this.linkEditViewFile,
            this.editCant});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "C0";
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
            // editCant
            // 
            this.editCant.EditFormat.FormatString = "n0";
            this.editCant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant.Mask.EditMask = "n0";
            this.editCant.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editCant.Mask.UseMaskAsDisplayFormat = true;
            this.editCant.Name = "editCant";
            // 
            // tc_QueryCreditos
            // 
            this.tc_QueryCreditos.AppearancePage.Header.Font = new System.Drawing.Font("Verdana", 9F);
            this.tc_QueryCreditos.AppearancePage.Header.Options.UseFont = true;
            this.tc_QueryCreditos.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.tc_QueryCreditos.AppearancePage.HeaderActive.Options.UseFont = true;
            this.tc_QueryCreditos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_QueryCreditos.Location = new System.Drawing.Point(0, 0);
            this.tc_QueryCreditos.Margin = new System.Windows.Forms.Padding(1);
            this.tc_QueryCreditos.Name = "tc_QueryCreditos";
            this.tc_QueryCreditos.SelectedTabPage = this.tp_DatosCartera;
            this.tc_QueryCreditos.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tc_QueryCreditos.Size = new System.Drawing.Size(1109, 596);
            this.tc_QueryCreditos.TabIndex = 2;
            this.tc_QueryCreditos.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tp_DatosCartera});
            // 
            // tp_DatosCartera
            // 
            this.tp_DatosCartera.Controls.Add(this.pnlHeader);
            this.tp_DatosCartera.Margin = new System.Windows.Forms.Padding(1);
            this.tp_DatosCartera.Name = "tp_DatosCartera";
            this.tp_DatosCartera.Size = new System.Drawing.Size(1103, 590);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.masterEtapa);
            this.pnlHeader.Controls.Add(this.dtFechaCorte);
            this.pnlHeader.Controls.Add(this.dtFechaInicial);
            this.pnlHeader.Controls.Add(this.pnlGrid);
            this.pnlHeader.Controls.Add(this.lblFechaFin);
            this.pnlHeader.Controls.Add(this.lblFechaInicial);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(1);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(1);
            this.pnlHeader.Size = new System.Drawing.Size(1103, 590);
            this.pnlHeader.TabIndex = 2;
            this.pnlHeader.TabStop = false;
            // 
            // dtFechaCorte
            // 
            this.dtFechaCorte.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaCorte.Location = new System.Drawing.Point(110, 25);
            this.dtFechaCorte.Name = "dtFechaCorte";
            this.dtFechaCorte.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaCorte.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCorte.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaCorte.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCorte.Properties.Appearance.Options.UseFont = true;
            this.dtFechaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCorte.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCorte.Size = new System.Drawing.Size(117, 20);
            this.dtFechaCorte.TabIndex = 81;
            // 
            // dtFechaInicial
            // 
            this.dtFechaInicial.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaInicial.Location = new System.Drawing.Point(110, 49);
            this.dtFechaInicial.Name = "dtFechaInicial";
            this.dtFechaInicial.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaInicial.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicial.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaInicial.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicial.Properties.Appearance.Options.UseFont = true;
            this.dtFechaInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicial.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicial.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicial.Size = new System.Drawing.Size(117, 20);
            this.dtFechaInicial.TabIndex = 79;
            this.dtFechaInicial.Visible = false;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.gcCobranzas);
            this.pnlGrid.Location = new System.Drawing.Point(1, 71);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.pnlGrid.Size = new System.Drawing.Size(1101, 474);
            this.pnlGrid.TabIndex = 0;
            this.pnlGrid.TabStop = false;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFin.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaFin.Location = new System.Drawing.Point(20, 28);
            this.lblFechaFin.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFin.TabIndex = 80;
            this.lblFechaFin.Text = "32322_lblFechaFin";
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicial.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaInicial.Location = new System.Drawing.Point(24, 52);
            this.lblFechaInicial.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(98, 14);
            this.lblFechaInicial.TabIndex = 78;
            this.lblFechaInicial.Text = "32322_lblFechaIni";
            this.lblFechaInicial.Visible = false;
            // 
            // masterEtapa
            // 
            this.masterEtapa.BackColor = System.Drawing.Color.Transparent;
            this.masterEtapa.Filtros = null;
            this.masterEtapa.Location = new System.Drawing.Point(305, 23);
            this.masterEtapa.Name = "masterEtapa";
            this.masterEtapa.Size = new System.Drawing.Size(387, 25);
            this.masterEtapa.TabIndex = 82;
            this.masterEtapa.Value = "";
            // 
            // QueryGestionCobranza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 596);
            this.Controls.Add(this.tc_QueryCreditos);
            this.Name = "QueryGestionCobranza";
            this.Text = "Saldos Libranza";
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCobranzas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCobranzas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).EndInit();
            this.tc_QueryCreditos.ResumeLayout(false);
            this.tp_DatosCartera.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private DevExpress.XtraTab.XtraTabControl tc_QueryCreditos;
        private DevExpress.XtraTab.XtraTabPage tp_DatosCartera;
        private System.Windows.Forms.GroupBox pnlHeader;
        protected System.Windows.Forms.GroupBox pnlGrid;
        protected DevExpress.XtraGrid.GridControl gcCobranzas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvCobranzas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCuotas;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editCant;
        private DevExpress.XtraEditors.LabelControl lblFechaInicial;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicial;
        private DevExpress.XtraEditors.LabelControl lblFechaFin;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
        private ControlsUC.uc_MasterFind masterEtapa;
    }
}