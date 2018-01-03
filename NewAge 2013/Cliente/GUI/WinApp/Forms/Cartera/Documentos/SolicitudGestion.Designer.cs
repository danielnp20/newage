namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SolicitudGestion
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
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcPendientes = new DevExpress.XtraGrid.GridControl();
            this.gvPendientes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editCant = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tc_QueryCreditos = new DevExpress.XtraTab.XtraTabControl();
            this.tp_DatosCartera = new DevExpress.XtraTab.XtraTabPage();
            this.pnlHeader = new System.Windows.Forms.GroupBox();
            this.pnlGrid = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).BeginInit();
            this.tc_QueryCreditos.SuspendLayout();
            this.tp_DatosCartera.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlGrid.SuspendLayout();
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
            this.gvDetalle.GridControl = this.gcPendientes;
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
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvLibranzas_CustomUnboundColumnData);
            // 
            // gcPendientes
            // 
            this.gcPendientes.AllowDrop = true;
            this.gcPendientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPendientes.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPendientes.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcPendientes.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPendientes.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPendientes.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPendientes.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPendientes.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPendientes.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPendientes.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcPendientes.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcPendientes.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.gcPendientes.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcPendientes.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPendientes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Cuotas";
            this.gcPendientes.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcPendientes.Location = new System.Drawing.Point(4, 19);
            this.gcPendientes.LookAndFeel.SkinName = "Dark Side";
            this.gcPendientes.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPendientes.MainView = this.gvPendientes;
            this.gcPendientes.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.gcPendientes.Name = "gcPendientes";
            this.gcPendientes.Size = new System.Drawing.Size(1460, 602);
            this.gcPendientes.TabIndex = 0;
            this.gcPendientes.UseEmbeddedNavigator = true;
            this.gcPendientes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPendientes,
            this.gvDetalle});
            // 
            // gvPendientes
            // 
            this.gvPendientes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPendientes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvPendientes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPendientes.Appearance.Empty.Options.UseBackColor = true;
            this.gvPendientes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvPendientes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPendientes.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPendientes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvPendientes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPendientes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPendientes.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPendientes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvPendientes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPendientes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPendientes.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvPendientes.Appearance.FooterPanel.Options.UseFont = true;
            this.gvPendientes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvPendientes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPendientes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvPendientes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvPendientes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPendientes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvPendientes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPendientes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPendientes.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPendientes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPendientes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPendientes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPendientes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPendientes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPendientes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvPendientes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPendientes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPendientes.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPendientes.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPendientes.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvPendientes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPendientes.Appearance.Row.Options.UseBackColor = true;
            this.gvPendientes.Appearance.Row.Options.UseForeColor = true;
            this.gvPendientes.Appearance.Row.Options.UseTextOptions = true;
            this.gvPendientes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvPendientes.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPendientes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPendientes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvPendientes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPendientes.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPendientes.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPendientes.GridControl = this.gcPendientes;
            this.gvPendientes.GroupFormat = "[#image]{1} {2}";
            this.gvPendientes.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_VlrCapital", null, "{0:c2}")});
            this.gvPendientes.HorzScrollStep = 50;
            this.gvPendientes.Name = "gvPendientes";
            this.gvPendientes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvPendientes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvPendientes.OptionsCustomization.AllowColumnMoving = false;
            this.gvPendientes.OptionsCustomization.AllowFilter = false;
            this.gvPendientes.OptionsCustomization.AllowSort = false;
            this.gvPendientes.OptionsMenu.EnableColumnMenu = false;
            this.gvPendientes.OptionsMenu.EnableFooterMenu = false;
            this.gvPendientes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPendientes.OptionsView.ShowAutoFilterRow = true;
            this.gvPendientes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvPendientes.OptionsView.ShowGroupPanel = false;
            this.gvPendientes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvPendientes.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPendientes_FocusedRowChanged);
            this.gvPendientes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvLibranzas_CustomUnboundColumnData);
            this.gvPendientes.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvPendientes_CustomColumnDisplayText);
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
            //this.linkEditViewFile.Caption = "Find_Document";
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
            this.tc_QueryCreditos.Size = new System.Drawing.Size(1479, 734);
            this.tc_QueryCreditos.TabIndex = 2;
            this.tc_QueryCreditos.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tp_DatosCartera});
            // 
            // tp_DatosCartera
            // 
            this.tp_DatosCartera.Controls.Add(this.pnlHeader);
            this.tp_DatosCartera.Margin = new System.Windows.Forms.Padding(1);
            this.tp_DatosCartera.Name = "tp_DatosCartera";
            this.tp_DatosCartera.Size = new System.Drawing.Size(1473, 728);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.pnlGrid);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(1);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(1);
            this.pnlHeader.Size = new System.Drawing.Size(1473, 728);
            this.pnlHeader.TabIndex = 2;
            this.pnlHeader.TabStop = false;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.gcPendientes);
            this.pnlGrid.Location = new System.Drawing.Point(1, 48);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(4, 0, 4, 1);
            this.pnlGrid.Size = new System.Drawing.Size(1468, 622);
            this.pnlGrid.TabIndex = 0;
            this.pnlGrid.TabStop = false;
            // 
            // SolicitudGestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1479, 734);
            this.Controls.Add(this.tc_QueryCreditos);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SolicitudGestion";
            this.Text = "SolicitudGestion";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).EndInit();
            this.tc_QueryCreditos.ResumeLayout(false);
            this.tp_DatosCartera.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
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
        protected DevExpress.XtraGrid.GridControl gcPendientes;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvPendientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editCant;
    }
}