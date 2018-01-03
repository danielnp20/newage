namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DeclaracionRenglonDetalle
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
            this.txtNroComp = new System.Windows.Forms.TextBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtComp = new System.Windows.Forms.TextBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtRenglon = new System.Windows.Forms.TextBox();
            this.lblRenglon = new DevExpress.XtraEditors.LabelControl();
            this.txtDeclaracion = new System.Windows.Forms.TextBox();
            this.lblDeclaracion = new DevExpress.XtraEditors.LabelControl();
            this.txtPeriodoFiscal = new System.Windows.Forms.TextBox();
            this.txtAñoFiscal = new System.Windows.Forms.TextBox();
            this.lblAño = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriodo = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gcCuentas = new DevExpress.XtraGrid.GridControl();
            this.gvCuentas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCuentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuentas)).BeginInit();
            this.TbLyPanel.SuspendLayout();
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
            this.panel2.Controls.Add(this.txtNroComp);
            this.panel2.Controls.Add(this.labelControl1);
            this.panel2.Controls.Add(this.txtComp);
            this.panel2.Controls.Add(this.labelControl2);
            this.panel2.Controls.Add(this.txtRenglon);
            this.panel2.Controls.Add(this.lblRenglon);
            this.panel2.Controls.Add(this.txtDeclaracion);
            this.panel2.Controls.Add(this.lblDeclaracion);
            this.panel2.Controls.Add(this.txtPeriodoFiscal);
            this.panel2.Controls.Add(this.txtAñoFiscal);
            this.panel2.Controls.Add(this.lblAño);
            this.panel2.Controls.Add(this.lblPeriodo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(18, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(935, 82);
            this.panel2.TabIndex = 0;
            // 
            // txtNroComp
            // 
            this.txtNroComp.Enabled = false;
            this.txtNroComp.Location = new System.Drawing.Point(362, 59);
            this.txtNroComp.Name = "txtNroComp";
            this.txtNroComp.ReadOnly = true;
            this.txtNroComp.Size = new System.Drawing.Size(107, 20);
            this.txtNroComp.TabIndex = 12;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Location = new System.Drawing.Point(253, 59);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(92, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "20505_NroComp";
            // 
            // txtComp
            // 
            this.txtComp.Enabled = false;
            this.txtComp.Location = new System.Drawing.Point(147, 56);
            this.txtComp.Name = "txtComp";
            this.txtComp.ReadOnly = true;
            this.txtComp.Size = new System.Drawing.Size(89, 20);
            this.txtComp.TabIndex = 10;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl2.Location = new System.Drawing.Point(15, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(127, 14);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "20505_lblComprobante";
            // 
            // txtRenglon
            // 
            this.txtRenglon.Enabled = false;
            this.txtRenglon.Location = new System.Drawing.Point(362, 35);
            this.txtRenglon.Name = "txtRenglon";
            this.txtRenglon.ReadOnly = true;
            this.txtRenglon.Size = new System.Drawing.Size(107, 20);
            this.txtRenglon.TabIndex = 8;
            // 
            // lblRenglon
            // 
            this.lblRenglon.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRenglon.Location = new System.Drawing.Point(253, 35);
            this.lblRenglon.Name = "lblRenglon";
            this.lblRenglon.Size = new System.Drawing.Size(97, 14);
            this.lblRenglon.TabIndex = 7;
            this.lblRenglon.Text = "20505_lblRenglon";
            // 
            // txtDeclaracion
            // 
            this.txtDeclaracion.Enabled = false;
            this.txtDeclaracion.Location = new System.Drawing.Point(147, 32);
            this.txtDeclaracion.Name = "txtDeclaracion";
            this.txtDeclaracion.ReadOnly = true;
            this.txtDeclaracion.Size = new System.Drawing.Size(89, 20);
            this.txtDeclaracion.TabIndex = 6;
            // 
            // lblDeclaracion
            // 
            this.lblDeclaracion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblDeclaracion.Location = new System.Drawing.Point(15, 35);
            this.lblDeclaracion.Name = "lblDeclaracion";
            this.lblDeclaracion.Size = new System.Drawing.Size(114, 14);
            this.lblDeclaracion.TabIndex = 5;
            this.lblDeclaracion.Text = "20505_lblDeclaracion";
            // 
            // txtPeriodoFiscal
            // 
            this.txtPeriodoFiscal.Enabled = false;
            this.txtPeriodoFiscal.Location = new System.Drawing.Point(362, 7);
            this.txtPeriodoFiscal.Name = "txtPeriodoFiscal";
            this.txtPeriodoFiscal.ReadOnly = true;
            this.txtPeriodoFiscal.Size = new System.Drawing.Size(107, 20);
            this.txtPeriodoFiscal.TabIndex = 4;
            // 
            // txtAñoFiscal
            // 
            this.txtAñoFiscal.Enabled = false;
            this.txtAñoFiscal.Location = new System.Drawing.Point(147, 7);
            this.txtAñoFiscal.Name = "txtAñoFiscal";
            this.txtAñoFiscal.ReadOnly = true;
            this.txtAñoFiscal.Size = new System.Drawing.Size(89, 20);
            this.txtAñoFiscal.TabIndex = 3;
            // 
            // lblAño
            // 
            this.lblAño.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblAño.Location = new System.Drawing.Point(15, 9);
            this.lblAño.Name = "lblAño";
            this.lblAño.Size = new System.Drawing.Size(102, 14);
            this.lblAño.TabIndex = 2;
            this.lblAño.Text = "20505_lblAñoFiscal";
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPeriodo.Location = new System.Drawing.Point(252, 10);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(94, 14);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "20505_lblPeriodo";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(18, 448);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(935, 50);
            this.panel1.TabIndex = 7;
            // 
            // gcCuentas
            // 
            this.gcCuentas.AllowDrop = true;
            this.gcCuentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCuentas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCuentas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCuentas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcCuentas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCuentas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCuentas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCuentas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCuentas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCuentas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCuentas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcCuentas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcCuentas.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcCuentas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCuentas.Location = new System.Drawing.Point(18, 100);
            this.gcCuentas.LookAndFeel.SkinName = "Dark Side";
            this.gcCuentas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCuentas.MainView = this.gvCuentas;
            this.gcCuentas.Name = "gcCuentas";
            this.gcCuentas.Size = new System.Drawing.Size(935, 342);
            this.gcCuentas.TabIndex = 3;
            this.gcCuentas.UseEmbeddedNavigator = true;
            this.gcCuentas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCuentas});
            // 
            // gvCuentas
            // 
            this.gvCuentas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuentas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCuentas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCuentas.Appearance.Empty.Options.UseBackColor = true;
            this.gvCuentas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCuentas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCuentas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuentas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCuentas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCuentas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCuentas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuentas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuentas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCuentas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCuentas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuentas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCuentas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCuentas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCuentas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuentas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCuentas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCuentas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCuentas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCuentas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCuentas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCuentas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCuentas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCuentas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuentas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCuentas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCuentas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCuentas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuentas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCuentas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCuentas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCuentas.Appearance.Row.Options.UseBackColor = true;
            this.gvCuentas.Appearance.Row.Options.UseForeColor = true;
            this.gvCuentas.Appearance.Row.Options.UseTextOptions = true;
            this.gvCuentas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCuentas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuentas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCuentas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuentas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCuentas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuentas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCuentas.GridControl = this.gcCuentas;
            this.gvCuentas.Name = "gvCuentas";
            this.gvCuentas.OptionsCustomization.AllowFilter = false;
            this.gvCuentas.OptionsCustomization.AllowSort = false;
            this.gvCuentas.OptionsCustomization.AllowColumnMoving = false;
            this.gvCuentas.OptionsView.ShowGroupPanel = false;
            this.gvCuentas.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvCuentas_CustomRowCellEdit);
            this.gvCuentas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvCuentas_CustomUnboundColumnData);
            this.gvCuentas.DoubleClick += new System.EventHandler(this.gvCuentas_DoubleClick);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.636661F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.36334F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.TbLyPanel.Controls.Add(this.gcCuentas, 1, 1);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 2);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 4;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.10478F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.89522F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TbLyPanel.Size = new System.Drawing.Size(970, 510);
            this.TbLyPanel.TabIndex = 0;
            // 
            // DeclaracionRenglonDetalle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 510);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "DeclaracionRenglonDetalle";
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCuentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuentas)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
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
        protected DevExpress.XtraGrid.GridControl gcCuentas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvCuentas;
        protected System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private System.Windows.Forms.TextBox txtRenglon;
        protected DevExpress.XtraEditors.LabelControl lblRenglon;
        private System.Windows.Forms.TextBox txtDeclaracion;
        protected DevExpress.XtraEditors.LabelControl lblDeclaracion;
        private System.Windows.Forms.TextBox txtNroComp;
        protected DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.TextBox txtComp;
        protected DevExpress.XtraEditors.LabelControl labelControl2;
    }
}