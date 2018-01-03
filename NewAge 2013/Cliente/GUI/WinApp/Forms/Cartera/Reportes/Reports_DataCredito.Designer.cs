namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Reports_DataCredito
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
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.gcGenerales = new DevExpress.XtraGrid.GridControl();
            this.gvGenerales = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTipoConsulta = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipo = new DevExpress.XtraEditors.LabelControl();
            this.dtPeriodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoConsulta.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "C2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue});
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.gbGridDocument);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 71);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(881, 411);
            this.groupBox2.TabIndex = 2;
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
            this.gbGridDocument.Size = new System.Drawing.Size(879, 394);
            this.gbGridDocument.TabIndex = 0;
            this.gbGridDocument.TabStop = false;
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
            this.gcGenerales.Location = new System.Drawing.Point(3, 15);
            this.gcGenerales.LookAndFeel.SkinName = "Dark Side";
            this.gcGenerales.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcGenerales.MainView = this.gvGenerales;
            this.gcGenerales.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcGenerales.Name = "gcGenerales";
            this.gcGenerales.Size = new System.Drawing.Size(873, 378);
            this.gcGenerales.TabIndex = 0;
            this.gcGenerales.UseEmbeddedNavigator = true;
            this.gcGenerales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGenerales});
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
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.cmbTipoConsulta);
            this.groupBox1.Controls.Add(this.lblTipo);
            this.groupBox1.Controls.Add(this.dtPeriodo);
            this.groupBox1.Controls.Add(this.lblPeriod);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(881, 71);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32316_InfoGeneral";
            // 
            // cmbTipoConsulta
            // 
            this.cmbTipoConsulta.Location = new System.Drawing.Point(386, 30);
            this.cmbTipoConsulta.Name = "cmbTipoConsulta";
            this.cmbTipoConsulta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoConsulta.Size = new System.Drawing.Size(153, 20);
            this.cmbTipoConsulta.TabIndex = 85;
            // 
            // lblTipo
            // 
            this.lblTipo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipo.Location = new System.Drawing.Point(321, 33);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(66, 13);
            this.lblTipo.TabIndex = 86;
            this.lblTipo.Text = "32311_lblTipo";
            // 
            // dtPeriodo
            // 
            this.dtPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriodo.EnabledControl = true;
            this.dtPeriodo.ExtraPeriods = 0;
            this.dtPeriodo.Location = new System.Drawing.Point(131, 30);
            this.dtPeriodo.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dtPeriodo.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.Name = "dtPeriodo";
            this.dtPeriodo.Size = new System.Drawing.Size(152, 19);
            this.dtPeriodo.TabIndex = 83;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(13, 33);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(80, 14);
            this.lblPeriod.TabIndex = 84;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // Reports_DataCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 482);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Reports_DataCredito";
            this.Text = "ShowDocumentForm";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoConsulta.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private System.Windows.Forms.GroupBox groupBox2;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcGenerales;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvGenerales;
        private System.Windows.Forms.GroupBox groupBox1;
        protected ControlsUC.uc_PeriodoEdit dtPeriodo;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoConsulta;
        private DevExpress.XtraEditors.LabelControl lblTipo;

    }
}