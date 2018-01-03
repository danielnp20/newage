namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QueryInformeMensual
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalleNivel1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalleNivel2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterPozo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterGrupo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            this.repositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.tcSaldos = new DevExpress.XtraTab.XtraTabControl();
            this.tpSaldos = new DevExpress.XtraTab.XtraTabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.lblGrupo = new System.Windows.Forms.Label();
            this.lblPozo = new System.Windows.Forms.Label();
            this.masterRecurso = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCampo = new System.Windows.Forms.Label();
            this.masterCampo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBloque = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterContrato = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbTipoMda = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbTipoProyecto = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoMda = new System.Windows.Forms.Label();
            this.lblTipoProyecto = new System.Windows.Forms.Label();
            this.cmbOrigenMda = new DevExpress.XtraEditors.LookUpEdit();
            this.lblOrigenMda = new System.Windows.Forms.Label();
            this.cmbTipoInforme = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoInforme = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel2)).BeginInit();
            this.gbGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcSaldos)).BeginInit();
            this.tcSaldos.SuspendLayout();
            this.tpSaldos.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoProyecto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrigenMda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoInforme.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetalleNivel1
            // 
            this.gvDetalleNivel1.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel1.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalleNivel1.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel1.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.Empty.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalleNivel1.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel1.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalleNivel1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel1.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalleNivel1.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalleNivel1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalleNivel1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalleNivel1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalleNivel1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalleNivel1.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDetalleNivel1.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel1.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.Row.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalleNivel1.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalleNivel1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalleNivel1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalleNivel1.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel1.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalleNivel1.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalleNivel1.GridControl = this.gcDocument;
            this.gvDetalleNivel1.HorzScrollStep = 50;
            this.gvDetalleNivel1.Name = "gvDetalleNivel1";
            this.gvDetalleNivel1.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalleNivel1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalleNivel1.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalleNivel1.OptionsCustomization.AllowFilter = false;
            this.gvDetalleNivel1.OptionsCustomization.AllowSort = false;
            this.gvDetalleNivel1.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalleNivel1.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalleNivel1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalleNivel1.OptionsView.ColumnAutoWidth = false;
            this.gvDetalleNivel1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalleNivel1.OptionsView.ShowGroupPanel = false;
            this.gvDetalleNivel1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalleNivel1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
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
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalleNivel1;
            gridLevelNode2.LevelTemplate = this.gvDetalleNivel2;
            gridLevelNode2.RelationName = "DetalleNivel2";
            gridLevelNode1.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            gridLevelNode1.RelationName = "DetalleNivel1";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(3, 16);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1117, 391);
            this.gcDocument.TabIndex = 51;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalleNivel2,
            this.gvDetalleNivel1});
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
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDocument.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseFont = true;
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
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gvDetalleNivel2
            // 
            this.gvDetalleNivel2.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel2.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalleNivel2.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel2.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.Empty.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalleNivel2.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel2.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel2.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel2.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalleNivel2.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel2.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel2.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalleNivel2.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleNivel2.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalleNivel2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalleNivel2.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalleNivel2.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel2.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalleNivel2.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalleNivel2.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDetalleNivel2.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel2.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.Row.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalleNivel2.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalleNivel2.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalleNivel2.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleNivel2.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalleNivel2.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleNivel2.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalleNivel2.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalleNivel2.GridControl = this.gcDocument;
            this.gvDetalleNivel2.HorzScrollStep = 50;
            this.gvDetalleNivel2.Name = "gvDetalleNivel2";
            this.gvDetalleNivel2.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalleNivel2.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalleNivel2.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalleNivel2.OptionsCustomization.AllowFilter = false;
            this.gvDetalleNivel2.OptionsCustomization.AllowSort = false;
            this.gvDetalleNivel2.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalleNivel2.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalleNivel2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalleNivel2.OptionsView.ColumnAutoWidth = false;
            this.gvDetalleNivel2.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalleNivel2.OptionsView.ShowGroupPanel = false;
            this.gvDetalleNivel2.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalleNivel2.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
            // 
            // masterPozo
            // 
            this.masterPozo.BackColor = System.Drawing.Color.Transparent;
            this.masterPozo.Filtros = null;
            this.masterPozo.Location = new System.Drawing.Point(17, 8);
            this.masterPozo.Name = "masterPozo";
            this.masterPozo.Size = new System.Drawing.Size(307, 25);
            this.masterPozo.TabIndex = 88;
            this.masterPozo.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(17, 93);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(302, 25);
            this.masterProyecto.TabIndex = 89;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterGrupo
            // 
            this.masterGrupo.BackColor = System.Drawing.Color.Transparent;
            this.masterGrupo.Filtros = null;
            this.masterGrupo.Location = new System.Drawing.Point(17, 38);
            this.masterGrupo.Name = "masterGrupo";
            this.masterGrupo.Size = new System.Drawing.Size(301, 25);
            this.masterGrupo.TabIndex = 97;
            this.masterGrupo.Value = "";
            // 
            // gbGrid
            // 
            this.gbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGrid.Controls.Add(this.gcDocument);
            this.gbGrid.Location = new System.Drawing.Point(9, 163);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(1123, 410);
            this.gbGrid.TabIndex = 98;
            this.gbGrid.TabStop = false;
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
            this.TextEdit.Mask.EditMask = "c0";
            this.TextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit.Mask.UseMaskAsDisplayFormat = true;
            this.TextEdit.Name = "TextEdit";
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(119, 62);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(125, 24);
            this.dtPeriod.TabIndex = 103;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(20, 64);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 104;
            this.lblPeriod.Text = "25310_lblPeriod";
            // 
            // tcSaldos
            // 
            this.tcSaldos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSaldos.Location = new System.Drawing.Point(0, 0);
            this.tcSaldos.Name = "tcSaldos";
            this.tcSaldos.SelectedTabPage = this.tpSaldos;
            this.tcSaldos.Size = new System.Drawing.Size(1145, 623);
            this.tcSaldos.TabIndex = 105;
            this.tcSaldos.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpSaldos});
            // 
            // tpSaldos
            // 
            this.tpSaldos.Controls.Add(this.panel3);
            this.tpSaldos.Controls.Add(this.panel2);
            this.tpSaldos.Controls.Add(this.panel1);
            this.tpSaldos.Controls.Add(this.gbGrid);
            this.tpSaldos.Name = "tpSaldos";
            this.tpSaldos.Size = new System.Drawing.Size(1139, 595);
            this.tpSaldos.Text = "Informe Mensual";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblProyecto);
            this.panel3.Controls.Add(this.lblGrupo);
            this.panel3.Controls.Add(this.lblPozo);
            this.panel3.Controls.Add(this.masterRecurso);
            this.panel3.Controls.Add(this.masterGrupo);
            this.panel3.Controls.Add(this.masterPozo);
            this.panel3.Controls.Add(this.masterProyecto);
            this.panel3.Location = new System.Drawing.Point(738, 14);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(346, 143);
            this.panel3.TabIndex = 107;
            // 
            // lblProyecto
            // 
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProyecto.Location = new System.Drawing.Point(14, 98);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(81, 14);
            this.lblProyecto.TabIndex = 116;
            this.lblProyecto.Text = "25310_lblAFE";
            // 
            // lblGrupo
            // 
            this.lblGrupo.AutoSize = true;
            this.lblGrupo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupo.Location = new System.Drawing.Point(14, 41);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(105, 14);
            this.lblGrupo.TabIndex = 115;
            this.lblGrupo.Text = "25310_lblGrupoID";
            // 
            // lblPozo
            // 
            this.lblPozo.AutoSize = true;
            this.lblPozo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPozo.Location = new System.Drawing.Point(14, 13);
            this.lblPozo.Name = "lblPozo";
            this.lblPozo.Size = new System.Drawing.Size(98, 14);
            this.lblPozo.TabIndex = 114;
            this.lblPozo.Text = "25310_lblPozoID";
            // 
            // masterRecurso
            // 
            this.masterRecurso.BackColor = System.Drawing.Color.Transparent;
            this.masterRecurso.Filtros = null;
            this.masterRecurso.Location = new System.Drawing.Point(17, 66);
            this.masterRecurso.Name = "masterRecurso";
            this.masterRecurso.Size = new System.Drawing.Size(302, 25);
            this.masterRecurso.TabIndex = 99;
            this.masterRecurso.Value = "";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblCampo);
            this.panel2.Controls.Add(this.masterCampo);
            this.panel2.Controls.Add(this.masterBloque);
            this.panel2.Controls.Add(this.masterContrato);
            this.panel2.Location = new System.Drawing.Point(380, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 143);
            this.panel2.TabIndex = 106;
            // 
            // lblCampo
            // 
            this.lblCampo.AutoSize = true;
            this.lblCampo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCampo.Location = new System.Drawing.Point(12, 71);
            this.lblCampo.Name = "lblCampo";
            this.lblCampo.Size = new System.Drawing.Size(109, 14);
            this.lblCampo.TabIndex = 113;
            this.lblCampo.Text = "25310_lblCampoID";
            // 
            // masterCampo
            // 
            this.masterCampo.BackColor = System.Drawing.Color.Transparent;
            this.masterCampo.Filtros = null;
            this.masterCampo.Location = new System.Drawing.Point(14, 66);
            this.masterCampo.Name = "masterCampo";
            this.masterCampo.Size = new System.Drawing.Size(303, 25);
            this.masterCampo.TabIndex = 100;
            this.masterCampo.Value = "";
            // 
            // masterBloque
            // 
            this.masterBloque.BackColor = System.Drawing.Color.Transparent;
            this.masterBloque.Filtros = null;
            this.masterBloque.Location = new System.Drawing.Point(15, 38);
            this.masterBloque.Name = "masterBloque";
            this.masterBloque.Size = new System.Drawing.Size(303, 25);
            this.masterBloque.TabIndex = 99;
            this.masterBloque.Value = "";
            // 
            // masterContrato
            // 
            this.masterContrato.BackColor = System.Drawing.Color.Transparent;
            this.masterContrato.Filtros = null;
            this.masterContrato.Location = new System.Drawing.Point(14, 9);
            this.masterContrato.Name = "masterContrato";
            this.masterContrato.Size = new System.Drawing.Size(303, 25);
            this.masterContrato.TabIndex = 98;
            this.masterContrato.Value = "";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cmbTipoInforme);
            this.panel1.Controls.Add(this.lblTipoInforme);
            this.panel1.Controls.Add(this.cmbOrigenMda);
            this.panel1.Controls.Add(this.lblOrigenMda);
            this.panel1.Controls.Add(this.cmbTipoMda);
            this.panel1.Controls.Add(this.cmbTipoProyecto);
            this.panel1.Controls.Add(this.lblTipoMda);
            this.panel1.Controls.Add(this.lblTipoProyecto);
            this.panel1.Controls.Add(this.dtPeriod);
            this.panel1.Controls.Add(this.lblPeriod);
            this.panel1.Location = new System.Drawing.Point(18, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 143);
            this.panel1.TabIndex = 105;
            // 
            // cmbTipoMda
            // 
            this.cmbTipoMda.Location = new System.Drawing.Point(120, 89);
            this.cmbTipoMda.Name = "cmbTipoMda";
            this.cmbTipoMda.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoMda.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoMda.Properties.DisplayMember = "Value";
            this.cmbTipoMda.Properties.NullText = " ";
            this.cmbTipoMda.Properties.ValueMember = "Key";
            this.cmbTipoMda.Size = new System.Drawing.Size(99, 20);
            this.cmbTipoMda.TabIndex = 109;
            this.cmbTipoMda.EditValueChanged += new System.EventHandler(this.cmbMdOrigen_EditValueChanged);
            // 
            // cmbTipoProyecto
            // 
            this.cmbTipoProyecto.Location = new System.Drawing.Point(120, 35);
            this.cmbTipoProyecto.Name = "cmbTipoProyecto";
            this.cmbTipoProyecto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoProyecto.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoProyecto.Properties.DisplayMember = "Value";
            this.cmbTipoProyecto.Properties.NullText = " ";
            this.cmbTipoProyecto.Properties.ValueMember = "Key";
            this.cmbTipoProyecto.Size = new System.Drawing.Size(99, 20);
            this.cmbTipoProyecto.TabIndex = 111;
            this.cmbTipoProyecto.EditValueChanged += new System.EventHandler(this.cmbProyectoTipo_EditValueChanged);
            // 
            // lblTipoMda
            // 
            this.lblTipoMda.AutoSize = true;
            this.lblTipoMda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMda.Location = new System.Drawing.Point(17, 91);
            this.lblTipoMda.Name = "lblTipoMda";
            this.lblTipoMda.Size = new System.Drawing.Size(106, 14);
            this.lblTipoMda.TabIndex = 112;
            this.lblTipoMda.Text = "25310_lblTipoMda";
            // 
            // lblTipoProyecto
            // 
            this.lblTipoProyecto.AutoSize = true;
            this.lblTipoProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoProyecto.Location = new System.Drawing.Point(17, 37);
            this.lblTipoProyecto.Name = "lblTipoProyecto";
            this.lblTipoProyecto.Size = new System.Drawing.Size(133, 14);
            this.lblTipoProyecto.TabIndex = 110;
            this.lblTipoProyecto.Text = "25310_lblTipoProyecto";
            // 
            // cmbOrigenMda
            // 
            this.cmbOrigenMda.Location = new System.Drawing.Point(120, 116);
            this.cmbOrigenMda.Name = "cmbOrigenMda";
            this.cmbOrigenMda.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOrigenMda.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbOrigenMda.Properties.DisplayMember = "Value";
            this.cmbOrigenMda.Properties.NullText = " ";
            this.cmbOrigenMda.Properties.ValueMember = "Key";
            this.cmbOrigenMda.Size = new System.Drawing.Size(99, 20);
            this.cmbOrigenMda.TabIndex = 113;
            // 
            // lblOrigenMda
            // 
            this.lblOrigenMda.AutoSize = true;
            this.lblOrigenMda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigenMda.Location = new System.Drawing.Point(17, 119);
            this.lblOrigenMda.Name = "lblOrigenMda";
            this.lblOrigenMda.Size = new System.Drawing.Size(118, 14);
            this.lblOrigenMda.TabIndex = 114;
            this.lblOrigenMda.Text = "25310_lblOrigenMda";
            // 
            // cmbTipoInforme
            // 
            this.cmbTipoInforme.Location = new System.Drawing.Point(120, 9);
            this.cmbTipoInforme.Name = "cmbTipoInforme";
            this.cmbTipoInforme.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoInforme.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoInforme.Properties.DisplayMember = "Value";
            this.cmbTipoInforme.Properties.NullText = " ";
            this.cmbTipoInforme.Properties.ValueMember = "Key";
            this.cmbTipoInforme.Size = new System.Drawing.Size(99, 20);
            this.cmbTipoInforme.TabIndex = 116;
            // 
            // lblTipoInforme
            // 
            this.lblTipoInforme.AutoSize = true;
            this.lblTipoInforme.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoInforme.Location = new System.Drawing.Point(17, 12);
            this.lblTipoInforme.Name = "lblTipoInforme";
            this.lblTipoInforme.Size = new System.Drawing.Size(127, 14);
            this.lblTipoInforme.TabIndex = 115;
            this.lblTipoInforme.Text = "25310_lblTipoInforme";
            // 
            // QueryInformeMensual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 623);
            this.Controls.Add(this.tcSaldos);
            this.Name = "QueryInformeMensual";
            this.Text = "25310";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel2)).EndInit();
            this.gbGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcSaldos)).EndInit();
            this.tcSaldos.ResumeLayout(false);
            this.tpSaldos.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoProyecto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrigenMda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoInforme.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterPozo;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterGrupo;
        protected System.Windows.Forms.GroupBox gbGrid;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalleNivel1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalleNivel2;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.Repository.PersistentRepository repositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraTab.XtraTabControl tcSaldos;
        private DevExpress.XtraTab.XtraTabPage tpSaldos;
        private System.Windows.Forms.Panel panel3;
        private ControlsUC.uc_MasterFind masterContrato;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoMda;
        private System.Windows.Forms.Label lblTipoProyecto;
        private ControlsUC.uc_MasterFind masterRecurso;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoProyecto;
        private System.Windows.Forms.Label lblTipoMda;
        private ControlsUC.uc_MasterFind masterCampo;
        private ControlsUC.uc_MasterFind masterBloque;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.Label lblGrupo;
        private System.Windows.Forms.Label lblPozo;
        private System.Windows.Forms.Label lblCampo;
        private DevExpress.XtraEditors.LookUpEdit cmbOrigenMda;
        private System.Windows.Forms.Label lblOrigenMda;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoInforme;
        private System.Windows.Forms.Label lblTipoInforme;






    }
}