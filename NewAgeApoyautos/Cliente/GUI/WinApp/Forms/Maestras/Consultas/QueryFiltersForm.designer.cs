namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QueryFiltersForm
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
            this.gcQuery = new DevExpress.XtraGrid.GridControl();
            this.gvQuery = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.lkpConsulta = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoSolicitud = new DevExpress.XtraEditors.LabelControl();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).BeginInit();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkpConsulta.Properties)).BeginInit();
            this.gbGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcQuery
            // 
            this.gcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQuery.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcQuery.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcQuery.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcQuery.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcQuery.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcQuery.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcQuery.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcQuery.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcQuery.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcQuery.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcQuery.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcQuery.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcQuery.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcQuery.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcQuery.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcQuery.Location = new System.Drawing.Point(3, 16);
            this.gcQuery.LookAndFeel.SkinName = "Dark Side";
            this.gcQuery.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcQuery.MainView = this.gvQuery;
            this.gcQuery.Margin = new System.Windows.Forms.Padding(4);
            this.gcQuery.Name = "gcQuery";
            this.gcQuery.Size = new System.Drawing.Size(1093, 376);
            this.gcQuery.TabIndex = 51;
            this.gcQuery.UseEmbeddedNavigator = true;
            this.gcQuery.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQuery});
            // 
            // gvQuery
            // 
            this.gvQuery.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvQuery.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvQuery.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvQuery.Appearance.Empty.Options.UseBackColor = true;
            this.gvQuery.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvQuery.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvQuery.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvQuery.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvQuery.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvQuery.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvQuery.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvQuery.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvQuery.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvQuery.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvQuery.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvQuery.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvQuery.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvQuery.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvQuery.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvQuery.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvQuery.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvQuery.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvQuery.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvQuery.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvQuery.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvQuery.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvQuery.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvQuery.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvQuery.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvQuery.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvQuery.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvQuery.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvQuery.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvQuery.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvQuery.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvQuery.Appearance.Row.Options.UseBackColor = true;
            this.gvQuery.Appearance.Row.Options.UseForeColor = true;
            this.gvQuery.Appearance.Row.Options.UseTextOptions = true;
            this.gvQuery.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvQuery.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvQuery.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvQuery.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvQuery.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvQuery.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvQuery.Appearance.VertLine.Options.UseBackColor = true;
            this.gvQuery.GridControl = this.gcQuery;
            this.gvQuery.HorzScrollStep = 50;
            this.gvQuery.Name = "gvQuery";
            this.gvQuery.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvQuery.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvQuery.OptionsBehavior.ReadOnly = true;
            this.gvQuery.OptionsCustomization.AllowColumnMoving = false;
            this.gvQuery.OptionsCustomization.AllowFilter = false;
            this.gvQuery.OptionsCustomization.AllowSort = false;
            this.gvQuery.OptionsMenu.EnableColumnMenu = false;
            this.gvQuery.OptionsMenu.EnableFooterMenu = false;
            this.gvQuery.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvQuery.OptionsView.ColumnAutoWidth = false;
            this.gvQuery.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvQuery.OptionsView.ShowGroupPanel = false;
            this.gvQuery.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.lkpConsulta);
            this.gbHeader.Controls.Add(this.lblTipoSolicitud);
            this.gbHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHeader.Location = new System.Drawing.Point(0, 0);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(1099, 75);
            this.gbHeader.TabIndex = 0;
            this.gbHeader.TabStop = false;
            // 
            // lkpConsulta
            // 
            this.lkpConsulta.Location = new System.Drawing.Point(135, 31);
            this.lkpConsulta.Name = "lkpConsulta";
            this.lkpConsulta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpConsulta.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkpConsulta.Properties.DisplayMember = "Value";
            this.lkpConsulta.Properties.NullText = " ";
            this.lkpConsulta.Properties.ValueMember = "Key";
            this.lkpConsulta.Size = new System.Drawing.Size(95, 20);
            this.lkpConsulta.TabIndex = 0;
            this.lkpConsulta.EditValueChanged += new System.EventHandler(this.lkpConsulta_EditValueChanged);
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipoSolicitud.Location = new System.Drawing.Point(19, 35);
            this.lblTipoSolicitud.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(83, 13);
            this.lblTipoSolicitud.TabIndex = 64;
            this.lblTipoSolicitud.Text = "101_TipoSolicitud";
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.gcQuery);
            this.gbGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGrid.Location = new System.Drawing.Point(0, 75);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(1099, 395);
            this.gbGrid.TabIndex = 5;
            this.gbGrid.TabStop = false;
            // 
            // QueryFiltersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 546);
            this.Controls.Add(this.gbGrid);
            this.Controls.Add(this.gbHeader);
            this.Name = "QueryFiltersForm";
            this.Text = "QueryForm";
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).EndInit();
            this.gbHeader.ResumeLayout(false);
            this.gbHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkpConsulta.Properties)).EndInit();
            this.gbGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.GroupBox gbHeader;
        protected System.Windows.Forms.GroupBox gbGrid;
        protected DevExpress.XtraGrid.GridControl gcQuery;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvQuery;
        protected DevExpress.XtraEditors.LookUpEdit lkpConsulta;
        private DevExpress.XtraEditors.LabelControl lblTipoSolicitud;
    }
}