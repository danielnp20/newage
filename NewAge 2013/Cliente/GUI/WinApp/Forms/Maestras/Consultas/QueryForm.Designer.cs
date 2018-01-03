namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QueryForm
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
        protected virtual void InitializeComponent()
        {
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.repositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            this.gbGrid.SuspendLayout();
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
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalle_CustomColumnDisplayText);
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
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(3, 16);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1093, 315);
            this.gcDocument.TabIndex = 51;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            this.gcDocument.FocusedViewChanged += new DevExpress.XtraGrid.ViewFocusEventHandler(this.gcDocument_FocusedViewChanged);
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
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gbHeader
            // 
            this.gbHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHeader.Location = new System.Drawing.Point(0, 0);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(1099, 153);
            this.gbHeader.TabIndex = 0;
            this.gbHeader.TabStop = false;
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
            this.linkEditViewFile.Click += new System.EventHandler(this.linkEditViewFile_Click);
            // 
            // TextEdit
            // 
            this.TextEdit.Mask.EditMask = "c2";
            this.TextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit.Mask.UseMaskAsDisplayFormat = true;
            this.TextEdit.Name = "TextEdit";
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.gcDocument);
            this.gbGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGrid.Location = new System.Drawing.Point(0, 153);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(1099, 334);
            this.gbGrid.TabIndex = 5;
            this.gbGrid.TabStop = false;
            // 
            // QueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 546);
            this.Controls.Add(this.gbGrid);
            this.Controls.Add(this.gbHeader);
            this.Name = "QueryForm";
            this.Text = "QueryForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            this.gbGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox gbHeader;
        protected System.Windows.Forms.GroupBox gbGrid;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.Repository.PersistentRepository repositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit;
    }
}