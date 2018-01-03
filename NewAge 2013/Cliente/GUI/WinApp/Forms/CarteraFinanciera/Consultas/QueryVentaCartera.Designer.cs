namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class QueryVentaCartera
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
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.pnConsulta = new DevExpress.XtraEditors.PanelControl();
            this.dtMesFIN = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblOferta = new System.Windows.Forms.Label();
            this.lblMesFIN = new System.Windows.Forms.Label();
            this.lblMesINI = new System.Windows.Forms.Label();
            this.txtOferta = new System.Windows.Forms.TextBox();
            this.dtMesINI = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.cmbFiltro = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCompradorCartera = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnConsulta)).BeginInit();
            this.pnConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFiltro.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.pnConsulta);
            this.gbHeader.Size = new System.Drawing.Size(1099, 97);
            // 
            // gbGrid
            // 
            this.gbGrid.Location = new System.Drawing.Point(0, 97);
            this.gbGrid.Size = new System.Drawing.Size(1099, 377);
            // 
            // repositoryEdit
            // 
            this.repositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkEditViewFile,
            this.TextEdit});
            // 
            // TextEdit
            // 
            this.TextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit.Mask.UseMaskAsDisplayFormat = true;
            // 
            // pnConsulta
            // 
            this.pnConsulta.Controls.Add(this.dtMesFIN);
            this.pnConsulta.Controls.Add(this.lblOferta);
            this.pnConsulta.Controls.Add(this.lblMesFIN);
            this.pnConsulta.Controls.Add(this.lblMesINI);
            this.pnConsulta.Controls.Add(this.txtOferta);
            this.pnConsulta.Controls.Add(this.dtMesINI);
            this.pnConsulta.Controls.Add(this.cmbFiltro);
            this.pnConsulta.Controls.Add(this.masterCompradorCartera);
            this.pnConsulta.Controls.Add(this.lblFiltro);
            this.pnConsulta.Location = new System.Drawing.Point(20, 13);
            this.pnConsulta.Name = "pnConsulta";
            this.pnConsulta.Size = new System.Drawing.Size(922, 69);
            this.pnConsulta.TabIndex = 18;
            // 
            // dtMesFIN
            // 
            this.dtMesFIN.BackColor = System.Drawing.Color.Transparent;
            this.dtMesFIN.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtMesFIN.EnabledControl = true;
            this.dtMesFIN.ExtraPeriods = 0;
            this.dtMesFIN.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtMesFIN.Location = new System.Drawing.Point(740, 38);
            this.dtMesFIN.MaxValue = new System.DateTime(((long)(0)));
            this.dtMesFIN.MinValue = new System.DateTime(((long)(0)));
            this.dtMesFIN.Name = "dtMesFIN";
            this.dtMesFIN.Size = new System.Drawing.Size(143, 20);
            this.dtMesFIN.TabIndex = 3;
            // 
            // lblOferta
            // 
            this.lblOferta.AutoSize = true;
            this.lblOferta.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOferta.Location = new System.Drawing.Point(394, 14);
            this.lblOferta.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblOferta.Name = "lblOferta";
            this.lblOferta.Size = new System.Drawing.Size(99, 16);
            this.lblOferta.TabIndex = 21;
            this.lblOferta.Text = "32315_lblOferta";
            // 
            // lblMesFIN
            // 
            this.lblMesFIN.AutoSize = true;
            this.lblMesFIN.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMesFIN.Location = new System.Drawing.Point(605, 41);
            this.lblMesFIN.Name = "lblMesFIN";
            this.lblMesFIN.Size = new System.Drawing.Size(99, 14);
            this.lblMesFIN.TabIndex = 23;
            this.lblMesFIN.Text = "32315_lblMesFIN";
            // 
            // lblMesINI
            // 
            this.lblMesINI.AutoSize = true;
            this.lblMesINI.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMesINI.Location = new System.Drawing.Point(605, 15);
            this.lblMesINI.Name = "lblMesINI";
            this.lblMesINI.Size = new System.Drawing.Size(97, 14);
            this.lblMesINI.TabIndex = 1;
            this.lblMesINI.Text = "32315_lblMesINI";
            // 
            // txtOferta
            // 
            this.txtOferta.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOferta.Location = new System.Drawing.Point(475, 11);
            this.txtOferta.Margin = new System.Windows.Forms.Padding(1);
            this.txtOferta.Name = "txtOferta";
            this.txtOferta.Size = new System.Drawing.Size(87, 22);
            this.txtOferta.TabIndex = 1;
            // 
            // dtMesINI
            // 
            this.dtMesINI.BackColor = System.Drawing.Color.Transparent;
            this.dtMesINI.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtMesINI.EnabledControl = true;
            this.dtMesINI.ExtraPeriods = 0;
            this.dtMesINI.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtMesINI.Location = new System.Drawing.Point(740, 12);
            this.dtMesINI.MaxValue = new System.DateTime(((long)(0)));
            this.dtMesINI.MinValue = new System.DateTime(((long)(0)));
            this.dtMesINI.Name = "dtMesINI";
            this.dtMesINI.Size = new System.Drawing.Size(143, 20);
            this.dtMesINI.TabIndex = 2;
            // 
            // cmbFiltro
            // 
            this.cmbFiltro.Location = new System.Drawing.Point(134, 37);
            this.cmbFiltro.Name = "cmbFiltro";
            this.cmbFiltro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFiltro.Properties.Appearance.Options.UseFont = true;
            this.cmbFiltro.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFiltro.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbFiltro.Properties.DisplayMember = "Value";
            this.cmbFiltro.Properties.ValueMember = "Key";
            this.cmbFiltro.Size = new System.Drawing.Size(118, 20);
            this.cmbFiltro.TabIndex = 4;
            this.cmbFiltro.EditValueChanged += new System.EventHandler(this.cmbFilter_EditValueChanged);
            // 
            // masterCompradorCartera
            // 
            this.masterCompradorCartera.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorCartera.Filtros = null;
            this.masterCompradorCartera.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCompradorCartera.Location = new System.Drawing.Point(18, 7);
            this.masterCompradorCartera.Margin = new System.Windows.Forms.Padding(4);
            this.masterCompradorCartera.Name = "masterCompradorCartera";
            this.masterCompradorCartera.Size = new System.Drawing.Size(347, 31);
            this.masterCompradorCartera.TabIndex = 0;
            this.masterCompradorCartera.Leave += new System.EventHandler(this.masterCompradorCar_Leave);
            // 
            // lblFiltro
            // 
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltro.Location = new System.Drawing.Point(17, 40);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(130, 14);
            this.lblFiltro.TabIndex = 6;
            this.lblFiltro.Text = "32315_lblTipoConsulta";
            // 
            // QueryVentaCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 546);
            this.Name = "QueryVentaCartera";
            this.Text = "32315";
            this.gbHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnConsulta)).EndInit();
            this.pnConsulta.ResumeLayout(false);
            this.pnConsulta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFiltro.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMesINI;
        private ControlsUC.uc_PeriodoEdit dtMesINI;
        private System.Windows.Forms.Label lblFiltro;
        private DevExpress.XtraEditors.LookUpEdit cmbFiltro;
        private DevExpress.XtraEditors.PanelControl pnConsulta;
        private ControlsUC.uc_PeriodoEdit dtMesFIN;
        private System.Windows.Forms.Label lblMesFIN;
        private System.Windows.Forms.TextBox txtOferta;
        private System.Windows.Forms.Label lblOferta;
        private ControlsUC.uc_MasterFind masterCompradorCartera;
    }
}