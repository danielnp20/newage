namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QuerySobreEjecucion 
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalleNivel2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            this.repositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tcQuery = new DevExpress.XtraTab.XtraTabControl();
            this.tpSaldos = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAreaAprob = new System.Windows.Forms.Label();
            this.masterAreaAprob = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoProyecto = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel2)).BeginInit();
            this.gbGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcQuery)).BeginInit();
            this.tcQuery.SuspendLayout();
            this.tpSaldos.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
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
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
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
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
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
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
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
            gridLevelNode2.LevelTemplate = this.gvDetalle;
            gridLevelNode2.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcDocument.Location = new System.Drawing.Point(3, 16);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1117, 456);
            this.gcDocument.TabIndex = 51;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalleNivel2,
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
            this.gvDocument.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseFont = true;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDocument.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseFont = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.SelectedRow.Options.UseFont = true;
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
            // gbGrid
            // 
            this.gbGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGrid.Controls.Add(this.gcDocument);
            this.gbGrid.Location = new System.Drawing.Point(9, 86);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(1123, 475);
            this.gbGrid.TabIndex = 98;
            this.gbGrid.TabStop = false;
            // 
            // repositoryEdit
            // 
            this.repositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkEditViewFile,
            this.TextEdit,
            this.TextEdit2});
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
            // TextEdit2
            // 
            this.TextEdit2.Mask.EditMask = "n2";
            this.TextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit2.Mask.UseMaskAsDisplayFormat = true;
            this.TextEdit2.Name = "TextEdit2";
            // 
            // tcQuery
            // 
            this.tcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcQuery.Location = new System.Drawing.Point(0, 0);
            this.tcQuery.Name = "tcQuery";
            this.tcQuery.SelectedTabPage = this.tpSaldos;
            this.tcQuery.Size = new System.Drawing.Size(1145, 623);
            this.tcQuery.TabIndex = 105;
            this.tcQuery.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpSaldos});
            // 
            // tpSaldos
            // 
            this.tpSaldos.Controls.Add(this.panel1);
            this.tpSaldos.Controls.Add(this.gbGrid);
            this.tpSaldos.Name = "tpSaldos";
            this.tpSaldos.Size = new System.Drawing.Size(1139, 595);
            this.tpSaldos.Text = "25312_tcSobreEjecucion";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblAreaAprob);
            this.panel1.Controls.Add(this.masterAreaAprob);
            this.panel1.Controls.Add(this.cmbEstado);
            this.panel1.Controls.Add(this.lblTipoProyecto);
            this.panel1.Location = new System.Drawing.Point(18, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 49);
            this.panel1.TabIndex = 105;
            // 
            // lblAreaAprob
            // 
            this.lblAreaAprob.AutoSize = true;
            this.lblAreaAprob.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAreaAprob.Location = new System.Drawing.Point(257, 16);
            this.lblAreaAprob.Name = "lblAreaAprob";
            this.lblAreaAprob.Size = new System.Drawing.Size(118, 14);
            this.lblAreaAprob.TabIndex = 116;
            this.lblAreaAprob.Text = "25312_lblAreaAprob";
            // 
            // masterAreaAprob
            // 
            this.masterAreaAprob.BackColor = System.Drawing.Color.Transparent;
            this.masterAreaAprob.Filtros = null;
            this.masterAreaAprob.Location = new System.Drawing.Point(272, 11);
            this.masterAreaAprob.Name = "masterAreaAprob";
            this.masterAreaAprob.Size = new System.Drawing.Size(303, 25);
            this.masterAreaAprob.TabIndex = 115;
            this.masterAreaAprob.Value = "";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(101, 13);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstado.Properties.DisplayMember = "Value";
            this.cmbEstado.Properties.NullText = " ";
            this.cmbEstado.Properties.ValueMember = "Key";
            this.cmbEstado.Size = new System.Drawing.Size(121, 20);
            this.cmbEstado.TabIndex = 111;
            this.cmbEstado.EditValueChanged += new System.EventHandler(this.cmbEstado_EditValueChanged);
            // 
            // lblTipoProyecto
            // 
            this.lblTipoProyecto.AutoSize = true;
            this.lblTipoProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoProyecto.Location = new System.Drawing.Point(17, 16);
            this.lblTipoProyecto.Name = "lblTipoProyecto";
            this.lblTipoProyecto.Size = new System.Drawing.Size(97, 14);
            this.lblTipoProyecto.TabIndex = 110;
            this.lblTipoProyecto.Text = "25312_lblEstado";
            // 
            // QuerySobreEjecucion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 623);
            this.Controls.Add(this.tcQuery);
            this.Name = "QuerySobreEjecucion";
            this.Text = "25310";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleNivel2)).EndInit();
            this.gbGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcQuery)).EndInit();
            this.tcQuery.ResumeLayout(false);
            this.tpSaldos.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox gbGrid;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalleNivel2;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.Repository.PersistentRepository repositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit;
        private DevExpress.XtraTab.XtraTabControl tcQuery;
        private DevExpress.XtraTab.XtraTabPage tpSaldos;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTipoProyecto;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit2;
        private System.Windows.Forms.Label lblAreaAprob;
        private ControlsUC.uc_MasterFind masterAreaAprob;






    }
}