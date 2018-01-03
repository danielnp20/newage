namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SaldosLibranza
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
            this.gvCuotas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcLibranzas = new DevExpress.XtraGrid.GridControl();
            this.gvLibranzas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editCant = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tc_QueryCreditos = new DevExpress.XtraTab.XtraTabControl();
            this.tp_DatosCartera = new DevExpress.XtraTab.XtraTabPage();
            this.pnlHeader = new System.Windows.Forms.GroupBox();
            this.lblFechaCorte = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlGrid = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibranzas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLibranzas)).BeginInit();
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
            this.gvCuotas.GridControl = this.gcLibranzas;
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
            // gcLibranzas
            // 
            this.gcLibranzas.AllowDrop = true;
            this.gcLibranzas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLibranzas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcLibranzas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcLibranzas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcLibranzas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcLibranzas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcLibranzas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcLibranzas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcLibranzas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcLibranzas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcLibranzas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcLibranzas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcLibranzas.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcLibranzas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcLibranzas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvCuotas;
            gridLevelNode1.RelationName = "Cuotas";
            this.gcLibranzas.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcLibranzas.Location = new System.Drawing.Point(3, 15);
            this.gcLibranzas.LookAndFeel.SkinName = "Dark Side";
            this.gcLibranzas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcLibranzas.MainView = this.gvLibranzas;
            this.gcLibranzas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcLibranzas.Name = "gcLibranzas";
            this.gcLibranzas.Size = new System.Drawing.Size(1095, 458);
            this.gcLibranzas.TabIndex = 0;
            this.gcLibranzas.UseEmbeddedNavigator = true;
            this.gcLibranzas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLibranzas,
            this.gvCuotas});
            // 
            // gvLibranzas
            // 
            this.gvLibranzas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvLibranzas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvLibranzas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvLibranzas.Appearance.Empty.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvLibranzas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvLibranzas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvLibranzas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvLibranzas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvLibranzas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvLibranzas.Appearance.FooterPanel.Options.UseFont = true;
            this.gvLibranzas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvLibranzas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvLibranzas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvLibranzas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvLibranzas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvLibranzas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvLibranzas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvLibranzas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvLibranzas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvLibranzas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvLibranzas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvLibranzas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvLibranzas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvLibranzas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvLibranzas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvLibranzas.Appearance.Row.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.Row.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.Row.Options.UseTextOptions = true;
            this.gvLibranzas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvLibranzas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvLibranzas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvLibranzas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvLibranzas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvLibranzas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvLibranzas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvLibranzas.GridControl = this.gcLibranzas;
            this.gvLibranzas.GroupFormat = "[#image]{1} {2}";
            this.gvLibranzas.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_VlrCapital", null, "{0:c2}")});
            this.gvLibranzas.HorzScrollStep = 50;
            this.gvLibranzas.Name = "gvLibranzas";
            this.gvLibranzas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvLibranzas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvLibranzas.OptionsCustomization.AllowColumnMoving = false;
            this.gvLibranzas.OptionsCustomization.AllowFilter = false;
            this.gvLibranzas.OptionsCustomization.AllowSort = false;
            this.gvLibranzas.OptionsMenu.EnableColumnMenu = false;
            this.gvLibranzas.OptionsMenu.EnableFooterMenu = false;
            this.gvLibranzas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLibranzas.OptionsView.ShowAutoFilterRow = true;
            this.gvLibranzas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvLibranzas.OptionsView.ShowFooter = true;
            this.gvLibranzas.OptionsView.ShowGroupPanel = false;
            this.gvLibranzas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvLibranzas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvLibranzas_CustomUnboundColumnData);
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
            this.tp_DatosCartera.Text = "32321_tp_Saldos";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblFechaCorte);
            this.pnlHeader.Controls.Add(this.dtFechaCorte);
            this.pnlHeader.Controls.Add(this.masterCliente);
            this.pnlHeader.Controls.Add(this.pnlGrid);
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
            // lblFechaCorte
            // 
            this.lblFechaCorte.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaCorte.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaCorte.Location = new System.Drawing.Point(401, 31);
            this.lblFechaCorte.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaCorte.Name = "lblFechaCorte";
            this.lblFechaCorte.Size = new System.Drawing.Size(115, 14);
            this.lblFechaCorte.TabIndex = 78;
            this.lblFechaCorte.Text = "32321_lblFechaCorte";
            // 
            // dtFechaCorte
            // 
            this.dtFechaCorte.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaCorte.Location = new System.Drawing.Point(522, 28);
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
            this.dtFechaCorte.TabIndex = 79;
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(13, 25);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(350, 25);
            this.masterCliente.TabIndex = 70;
            this.masterCliente.Value = "";
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.gcLibranzas);
            this.pnlGrid.Location = new System.Drawing.Point(1, 71);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.pnlGrid.Size = new System.Drawing.Size(1101, 474);
            this.pnlGrid.TabIndex = 0;
            this.pnlGrid.TabStop = false;
            // 
            // SaldosLibranza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 596);
            this.Controls.Add(this.tc_QueryCreditos);
            this.Name = "SaldosLibranza";
            this.Text = "Saldos Libranza";
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibranzas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLibranzas)).EndInit();
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
        protected DevExpress.XtraGrid.GridControl gcLibranzas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvLibranzas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCuotas;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editCant;
        private DevExpress.XtraEditors.LabelControl lblFechaCorte;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
        private ControlsUC.uc_MasterFind masterCliente;
    }
}