namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ProcesarDeclaracion
    {
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
            this.components = new System.ComponentModel.Container();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtPeriodoFiscal = new System.Windows.Forms.TextBox();
            this.txtAñoFiscal = new System.Windows.Forms.TextBox();
            this.lblAño = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriodo = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbReport = new System.Windows.Forms.GroupBox();
            this.lblReportType = new System.Windows.Forms.Label();
            this.btnReportDec = new System.Windows.Forms.Button();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.gcRenglones = new DevExpress.XtraGrid.GridControl();
            this.gvRenglones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pbProcess = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcRenglones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRenglones)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin});
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtPeriodoFiscal);
            this.panel2.Controls.Add(this.txtAñoFiscal);
            this.panel2.Controls.Add(this.lblAño);
            this.panel2.Controls.Add(this.lblPeriodo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(13, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(641, 31);
            this.panel2.TabIndex = 0;
            // 
            // txtPeriodoFiscal
            // 
            this.txtPeriodoFiscal.Enabled = false;
            this.txtPeriodoFiscal.Location = new System.Drawing.Point(305, 6);
            this.txtPeriodoFiscal.Name = "txtPeriodoFiscal";
            this.txtPeriodoFiscal.ReadOnly = true;
            this.txtPeriodoFiscal.Size = new System.Drawing.Size(154, 20);
            this.txtPeriodoFiscal.TabIndex = 4;
            // 
            // txtAñoFiscal
            // 
            this.txtAñoFiscal.Enabled = false;
            this.txtAñoFiscal.Location = new System.Drawing.Point(125, 6);
            this.txtAñoFiscal.Name = "txtAñoFiscal";
            this.txtAñoFiscal.ReadOnly = true;
            this.txtAñoFiscal.Size = new System.Drawing.Size(66, 20);
            this.txtAñoFiscal.TabIndex = 3;
            // 
            // lblAño
            // 
            this.lblAño.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblAño.Location = new System.Drawing.Point(15, 8);
            this.lblAño.Name = "lblAño";
            this.lblAño.Size = new System.Drawing.Size(102, 14);
            this.lblAño.TabIndex = 2;
            this.lblAño.Text = "20505_lblAñoFiscal";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPeriodo.Location = new System.Drawing.Point(205, 9);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(94, 14);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "20505_lblPeriodo";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbReport);
            this.panel1.Controls.Add(this.btnProcesar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(13, 562);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 76);
            this.panel1.TabIndex = 7;
            // 
            // gbReport
            // 
            this.gbReport.Controls.Add(this.lblReportType);
            this.gbReport.Controls.Add(this.btnReportDec);
            this.gbReport.Controls.Add(this.cmbReportType);
            this.gbReport.Location = new System.Drawing.Point(3, 27);
            this.gbReport.Name = "gbReport";
            this.gbReport.Size = new System.Drawing.Size(635, 40);
            this.gbReport.TabIndex = 5;
            this.gbReport.TabStop = false;
            this.gbReport.Text = "20505_gbReport";
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Location = new System.Drawing.Point(210, 15);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(109, 13);
            this.lblReportType.TabIndex = 4;
            this.lblReportType.Text = "20505_lblReportType";
            // 
            // btnReportDec
            // 
            this.btnReportDec.Location = new System.Drawing.Point(461, 11);
            this.btnReportDec.Name = "btnReportDec";
            this.btnReportDec.Size = new System.Drawing.Size(111, 23);
            this.btnReportDec.TabIndex = 1;
            this.btnReportDec.Text = "20505_btnReportDec";
            this.btnReportDec.UseVisualStyleBackColor = true;
            this.btnReportDec.Click += new System.EventHandler(this.btnReportDec_Click);
            // 
            // cmbReportType
            // 
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.Location = new System.Drawing.Point(327, 12);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(112, 21);
            this.cmbReportType.TabIndex = 3;
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(464, 5);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(111, 23);
            this.btnProcesar.TabIndex = 0;
            this.btnProcesar.Text = "20505_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // gcRenglones
            // 
            this.gcRenglones.AllowDrop = true;
            this.gcRenglones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRenglones.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRenglones.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcRenglones.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcRenglones.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcRenglones.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcRenglones.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcRenglones.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcRenglones.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcRenglones.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcRenglones.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcRenglones.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcRenglones.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcRenglones.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRenglones.Location = new System.Drawing.Point(13, 48);
            this.gcRenglones.LookAndFeel.SkinName = "Dark Side";
            this.gcRenglones.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRenglones.MainView = this.gvRenglones;
            this.gcRenglones.Name = "gcRenglones";
            this.gcRenglones.Size = new System.Drawing.Size(641, 508);
            this.gcRenglones.TabIndex = 3;
            this.gcRenglones.UseEmbeddedNavigator = true;
            this.gcRenglones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRenglones});
            // 
            // gvRenglones
            // 
            this.gvRenglones.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRenglones.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRenglones.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRenglones.Appearance.Empty.Options.UseBackColor = true;
            this.gvRenglones.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRenglones.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRenglones.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRenglones.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRenglones.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRenglones.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRenglones.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRenglones.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRenglones.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRenglones.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRenglones.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvRenglones.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRenglones.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRenglones.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRenglones.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRenglones.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRenglones.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRenglones.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRenglones.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRenglones.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRenglones.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRenglones.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRenglones.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRenglones.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRenglones.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRenglones.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRenglones.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRenglones.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRenglones.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRenglones.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRenglones.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRenglones.Appearance.Row.Options.UseBackColor = true;
            this.gvRenglones.Appearance.Row.Options.UseForeColor = true;
            this.gvRenglones.Appearance.Row.Options.UseTextOptions = true;
            this.gvRenglones.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRenglones.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRenglones.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRenglones.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRenglones.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRenglones.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRenglones.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRenglones.GridControl = this.gcRenglones;
            this.gvRenglones.Name = "gvRenglones";
            this.gvRenglones.OptionsCustomization.AllowFilter = false;
            this.gvRenglones.OptionsCustomization.AllowSort = false;
            this.gvRenglones.OptionsCustomization.AllowColumnMoving = false;
            this.gvRenglones.OptionsView.ShowGroupPanel = false;
            this.gvRenglones.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRenglones_CustomRowCellEdit);
            this.gvRenglones.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvRenglones_CustomUnboundColumnData);
            this.gvRenglones.DoubleClick += new System.EventHandler(this.gvRenglones_DoubleClick);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.636661F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.36334F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.TbLyPanel.Controls.Add(this.gcRenglones, 1, 1);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 2);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 4;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.10478F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.89522F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TbLyPanel.Size = new System.Drawing.Size(672, 650);
            this.TbLyPanel.TabIndex = 0;
            // 
            // pbProcess
            // 
            this.pbProcess.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbProcess.Location = new System.Drawing.Point(0, 632);
            this.pbProcess.Name = "pbProcess";
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(672, 18);
            this.pbProcess.TabIndex = 2;
            // 
            // ProcesarDeclaracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 650);
            this.Controls.Add(this.pbProcess);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "ProcesarDeclaracion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProcesarDeclaracion_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbReport.ResumeLayout(false);
            this.gbReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcRenglones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRenglones)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        public System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtPeriodoFiscal;
        private System.Windows.Forms.TextBox txtAñoFiscal;
        protected DevExpress.XtraEditors.LabelControl lblAño;
        protected DevExpress.XtraEditors.LabelControl lblPeriodo;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnProcesar;
        protected DevExpress.XtraGrid.GridControl gcRenglones;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvRenglones;
        protected System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private System.Windows.Forms.Button btnReportDec;
        protected DevExpress.XtraEditors.ProgressBarControl pbProcess;
        private System.Windows.Forms.GroupBox gbReport;
        private System.Windows.Forms.Label lblReportType;
        private System.Windows.Forms.ComboBox cmbReportType;
    }
}