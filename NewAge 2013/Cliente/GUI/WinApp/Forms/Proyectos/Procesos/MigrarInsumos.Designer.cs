namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MigrarInsumos
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrarInsumos));
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportProveedor = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportarInsumo = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportAnalisis = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportAPU = new DevExpress.XtraEditors.SimpleButton();
            this.btnProcesar = new DevExpress.XtraEditors.SimpleButton();
            this.btnInconsistencias = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate3 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate4 = new DevExpress.XtraEditors.SimpleButton();
            this.groupTemplate = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTemplate)).BeginInit();
            this.groupTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 323);
            this.pbProcess.Margin = new System.Windows.Forms.Padding(4);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(462, 18);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(156, 240);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportProveedor
            // 
            this.btnImportProveedor.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportProveedor.Appearance.Options.UseFont = true;
            this.btnImportProveedor.Location = new System.Drawing.Point(28, 28);
            this.btnImportProveedor.Name = "btnImportProveedor";
            this.btnImportProveedor.Size = new System.Drawing.Size(152, 33);
            this.btnImportProveedor.TabIndex = 18;
            this.btnImportProveedor.Text = "1145_btnImportarProveedor";
            this.btnImportProveedor.Click += new System.EventHandler(this.btnImportarProveedor_Click);
            // 
            // btnImportarInsumo
            // 
            this.btnImportarInsumo.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportarInsumo.Appearance.Options.UseFont = true;
            this.btnImportarInsumo.Location = new System.Drawing.Point(199, 28);
            this.btnImportarInsumo.Name = "btnImportarInsumo";
            this.btnImportarInsumo.Size = new System.Drawing.Size(152, 33);
            this.btnImportarInsumo.TabIndex = 19;
            this.btnImportarInsumo.Text = "1145_btnImportarInsumo";
            this.btnImportarInsumo.Click += new System.EventHandler(this.btnImportarInsumo_Click);
            // 
            // btnImportAnalisis
            // 
            this.btnImportAnalisis.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportAnalisis.Appearance.Options.UseFont = true;
            this.btnImportAnalisis.Location = new System.Drawing.Point(28, 68);
            this.btnImportAnalisis.Name = "btnImportAnalisis";
            this.btnImportAnalisis.Size = new System.Drawing.Size(152, 33);
            this.btnImportAnalisis.TabIndex = 20;
            this.btnImportAnalisis.Text = "1145_btnImportarGrupo";
            this.btnImportAnalisis.Click += new System.EventHandler(this.btnImportarGrupos_Click);
            // 
            // btnImportAPU
            // 
            this.btnImportAPU.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportAPU.Appearance.Options.UseFont = true;
            this.btnImportAPU.Location = new System.Drawing.Point(199, 68);
            this.btnImportAPU.Name = "btnImportAPU";
            this.btnImportAPU.Size = new System.Drawing.Size(152, 33);
            this.btnImportAPU.TabIndex = 21;
            this.btnImportAPU.Text = "1145_btnImportarAPU";
            this.btnImportAPU.Click += new System.EventHandler(this.btnImportarAPU_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnProcesar.Appearance.Options.UseFont = true;
            this.btnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("btnProcesar.Image")));
            this.btnProcesar.Location = new System.Drawing.Point(143, 229);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(178, 33);
            this.btnProcesar.TabIndex = 22;
            this.btnProcesar.Text = "1145_btnProcesar";
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnInconsistencias.Appearance.Options.UseFont = true;
            this.btnInconsistencias.Image = global::NewAge.Properties.Resources.errorIcon;
            this.btnInconsistencias.Location = new System.Drawing.Point(143, 268);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(178, 35);
            this.btnInconsistencias.TabIndex = 23;
            this.btnInconsistencias.Text = "1145_btnInconsisten";
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // btnTemplate1
            // 
            this.btnTemplate1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate1.Appearance.Options.UseFont = true;
            this.btnTemplate1.Appearance.Options.UseTextOptions = true;
            this.btnTemplate1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate1.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate1.Image")));
            this.btnTemplate1.Location = new System.Drawing.Point(44, 24);
            this.btnTemplate1.Name = "btnTemplate1";
            this.btnTemplate1.Size = new System.Drawing.Size(110, 30);
            this.btnTemplate1.TabIndex = 24;
            this.btnTemplate1.Text = "1145_btnTempProveedor";
            this.btnTemplate1.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnTemplate2
            // 
            this.btnTemplate2.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate2.Appearance.Options.UseFont = true;
            this.btnTemplate2.Appearance.Options.UseTextOptions = true;
            this.btnTemplate2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate2.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate2.Image")));
            this.btnTemplate2.Location = new System.Drawing.Point(215, 24);
            this.btnTemplate2.Name = "btnTemplate2";
            this.btnTemplate2.Size = new System.Drawing.Size(110, 30);
            this.btnTemplate2.TabIndex = 25;
            this.btnTemplate2.Text = "1145_btnTempInsumo";
            this.btnTemplate2.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnTemplate3
            // 
            this.btnTemplate3.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate3.Appearance.Options.UseFont = true;
            this.btnTemplate3.Appearance.Options.UseTextOptions = true;
            this.btnTemplate3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate3.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate3.Image")));
            this.btnTemplate3.Location = new System.Drawing.Point(44, 59);
            this.btnTemplate3.Name = "btnTemplate3";
            this.btnTemplate3.Size = new System.Drawing.Size(110, 30);
            this.btnTemplate3.TabIndex = 26;
            this.btnTemplate3.Text = "1145_btnTempGrupo";
            this.btnTemplate3.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnTemplate4
            // 
            this.btnTemplate4.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate4.Appearance.Options.UseFont = true;
            this.btnTemplate4.Appearance.Options.UseTextOptions = true;
            this.btnTemplate4.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate4.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate4.Image")));
            this.btnTemplate4.Location = new System.Drawing.Point(215, 59);
            this.btnTemplate4.Name = "btnTemplate4";
            this.btnTemplate4.Size = new System.Drawing.Size(110, 30);
            this.btnTemplate4.TabIndex = 27;
            this.btnTemplate4.Text = "1145_btnTempAPU";
            this.btnTemplate4.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // groupTemplate
            // 
            this.groupTemplate.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupTemplate.AppearanceCaption.Options.UseFont = true;
            this.groupTemplate.Controls.Add(this.btnTemplate1);
            this.groupTemplate.Controls.Add(this.btnTemplate4);
            this.groupTemplate.Controls.Add(this.btnTemplate2);
            this.groupTemplate.Controls.Add(this.btnTemplate3);
            this.groupTemplate.Location = new System.Drawing.Point(47, 7);
            this.groupTemplate.Name = "groupTemplate";
            this.groupTemplate.Size = new System.Drawing.Size(390, 94);
            this.groupTemplate.TabIndex = 28;
            this.groupTemplate.Text = "Generar Plantilla";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.btnImportProveedor);
            this.groupControl1.Controls.Add(this.btnImportarInsumo);
            this.groupControl1.Controls.Add(this.btnImportAnalisis);
            this.groupControl1.Controls.Add(this.btnImportAPU);
            this.groupControl1.Location = new System.Drawing.Point(47, 107);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(390, 116);
            this.groupControl1.TabIndex = 29;
            this.groupControl1.Text = "Importar Archivos";
            // 
            // MigrarInsumos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(462, 341);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupTemplate);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lblMessages);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MigrarInsumos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.groupTemplate, 0);
            this.Controls.SetChildIndex(this.groupControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTemplate)).EndInit();
            this.groupTemplate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessages;
        private DevExpress.XtraEditors.SimpleButton btnImportProveedor;
        private DevExpress.XtraEditors.SimpleButton btnImportarInsumo;
        private DevExpress.XtraEditors.SimpleButton btnImportAnalisis;
        private DevExpress.XtraEditors.SimpleButton btnImportAPU;
        private DevExpress.XtraEditors.SimpleButton btnProcesar;
        private DevExpress.XtraEditors.SimpleButton btnInconsistencias;
        private DevExpress.XtraEditors.SimpleButton btnTemplate1;
        private DevExpress.XtraEditors.SimpleButton btnTemplate2;
        private DevExpress.XtraEditors.SimpleButton btnTemplate3;
        private DevExpress.XtraEditors.SimpleButton btnTemplate4;
        private DevExpress.XtraEditors.GroupControl groupTemplate;
        private DevExpress.XtraEditors.GroupControl groupControl1;

    }
}