namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MigrarActivoGarantia
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrarActivoGarantia));
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportarActivos = new DevExpress.XtraEditors.SimpleButton();
            this.btnProcesar = new DevExpress.XtraEditors.SimpleButton();
            this.btnInconsistencias = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplateActivos = new DevExpress.XtraEditors.SimpleButton();
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
            this.pbProcess.Location = new System.Drawing.Point(0, 237);
            this.pbProcess.Margin = new System.Windows.Forms.Padding(4);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(423, 18);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(156, 240);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportarActivos
            // 
            this.btnImportarActivos.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportarActivos.Appearance.Options.UseFont = true;
            this.btnImportarActivos.Location = new System.Drawing.Point(85, 28);
            this.btnImportarActivos.Name = "btnImportarActivos";
            this.btnImportarActivos.Size = new System.Drawing.Size(152, 33);
            this.btnImportarActivos.TabIndex = 19;
            this.btnImportarActivos.Text = "1152_btnImportarActivos";
            this.btnImportarActivos.Click += new System.EventHandler(this.btnImportarInsumo_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnProcesar.Appearance.Options.UseFont = true;
            this.btnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("btnProcesar.Image")));
            this.btnProcesar.Location = new System.Drawing.Point(49, 177);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(155, 33);
            this.btnProcesar.TabIndex = 22;
            this.btnProcesar.Text = "1152_btnProcesar";
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnInconsistencias.Appearance.Options.UseFont = true;
            this.btnInconsistencias.Image = global::NewAge.Properties.Resources.errorIcon;
            this.btnInconsistencias.Location = new System.Drawing.Point(210, 177);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(168, 35);
            this.btnInconsistencias.TabIndex = 23;
            this.btnInconsistencias.Text = "1152_btnInconsisten";
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // btnTemplateActivos
            // 
            this.btnTemplateActivos.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplateActivos.Appearance.Options.UseFont = true;
            this.btnTemplateActivos.Appearance.Options.UseTextOptions = true;
            this.btnTemplateActivos.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplateActivos.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplateActivos.Image")));
            this.btnTemplateActivos.Location = new System.Drawing.Point(85, 24);
            this.btnTemplateActivos.Name = "btnTemplateActivos";
            this.btnTemplateActivos.Size = new System.Drawing.Size(154, 30);
            this.btnTemplateActivos.TabIndex = 25;
            this.btnTemplateActivos.Text = "1152_btnGetActivos";
            this.btnTemplateActivos.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // groupTemplate
            // 
            this.groupTemplate.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupTemplate.AppearanceCaption.Options.UseFont = true;
            this.groupTemplate.Controls.Add(this.btnTemplateActivos);
            this.groupTemplate.Location = new System.Drawing.Point(47, 7);
            this.groupTemplate.Name = "groupTemplate";
            this.groupTemplate.Size = new System.Drawing.Size(331, 65);
            this.groupTemplate.TabIndex = 28;
            this.groupTemplate.Text = "Generar Plantilla ";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.btnImportarActivos);
            this.groupControl1.Location = new System.Drawing.Point(47, 91);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(331, 73);
            this.groupControl1.TabIndex = 29;
            this.groupControl1.Text = "Importar Archivos";
            // 
            // MigrarActivoGarantia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(423, 255);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupTemplate);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lblMessages);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MigrarActivoGarantia";
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
        private DevExpress.XtraEditors.SimpleButton btnImportarActivos;
        private DevExpress.XtraEditors.SimpleButton btnProcesar;
        private DevExpress.XtraEditors.SimpleButton btnInconsistencias;
        private DevExpress.XtraEditors.SimpleButton btnTemplateActivos;
        private DevExpress.XtraEditors.GroupControl groupTemplate;
        private DevExpress.XtraEditors.GroupControl groupControl1;

    }
}