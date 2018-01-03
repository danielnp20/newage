namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MigrarLocacionEntregas
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrarLocacionEntregas));
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportLocacionTarea = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportLocacion = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportLocacionDeta = new DevExpress.XtraEditors.SimpleButton();
            this.btnInconsistencias = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplate3 = new DevExpress.XtraEditors.SimpleButton();
            this.groupTemplate = new DevExpress.XtraEditors.GroupControl();
            this.groupImportar = new DevExpress.XtraEditors.GroupControl();
            this.groupProy = new DevExpress.XtraEditors.GroupControl();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTemplate)).BeginInit();
            this.groupTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupImportar)).BeginInit();
            this.groupImportar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).BeginInit();
            this.groupProy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 374);
            this.pbProcess.Margin = new System.Windows.Forms.Padding(4);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(394, 18);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(120, 305);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportLocacionTarea
            // 
            this.btnImportLocacionTarea.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportLocacionTarea.Appearance.Options.UseFont = true;
            this.btnImportLocacionTarea.Appearance.Options.UseTextOptions = true;
            this.btnImportLocacionTarea.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnImportLocacionTarea.Image = ((System.Drawing.Image)(resources.GetObject("btnImportLocacionTarea.Image")));
            this.btnImportLocacionTarea.Location = new System.Drawing.Point(22, 38);
            this.btnImportLocacionTarea.Name = "btnImportLocacionTarea";
            this.btnImportLocacionTarea.Size = new System.Drawing.Size(152, 45);
            this.btnImportLocacionTarea.TabIndex = 18;
            this.btnImportLocacionTarea.Text = "1158_btnImportarLocacionTarea";
            this.btnImportLocacionTarea.Click += new System.EventHandler(this.btnImportarLocacionTarea_Click);
            // 
            // btnImportLocacion
            // 
            this.btnImportLocacion.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportLocacion.Appearance.Options.UseFont = true;
            this.btnImportLocacion.Location = new System.Drawing.Point(389, 326);
            this.btnImportLocacion.Name = "btnImportLocacion";
            this.btnImportLocacion.Size = new System.Drawing.Size(10, 33);
            this.btnImportLocacion.TabIndex = 19;
            this.btnImportLocacion.Text = "1158_btnImportarLocacion";
            this.btnImportLocacion.Visible = false;
            this.btnImportLocacion.Click += new System.EventHandler(this.btnImportarLocacion_Click);
            // 
            // btnImportLocacionDeta
            // 
            this.btnImportLocacionDeta.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnImportLocacionDeta.Appearance.Options.UseFont = true;
            this.btnImportLocacionDeta.Appearance.Options.UseTextOptions = true;
            this.btnImportLocacionDeta.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnImportLocacionDeta.Image = ((System.Drawing.Image)(resources.GetObject("btnImportLocacionDeta.Image")));
            this.btnImportLocacionDeta.Location = new System.Drawing.Point(193, 38);
            this.btnImportLocacionDeta.Name = "btnImportLocacionDeta";
            this.btnImportLocacionDeta.Size = new System.Drawing.Size(152, 45);
            this.btnImportLocacionDeta.TabIndex = 20;
            this.btnImportLocacionDeta.Text = "1158_btnImportarLocacionDeta";
            this.btnImportLocacionDeta.Click += new System.EventHandler(this.btnImportarLocacionDeta_Click);
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnInconsistencias.Appearance.Options.UseFont = true;
            this.btnInconsistencias.Image = global::NewAge.Properties.Resources.errorIcon;
            this.btnInconsistencias.Location = new System.Drawing.Point(101, 335);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(178, 32);
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
            this.btnTemplate1.Location = new System.Drawing.Point(389, 365);
            this.btnTemplate1.Name = "btnTemplate1";
            this.btnTemplate1.Size = new System.Drawing.Size(10, 30);
            this.btnTemplate1.TabIndex = 24;
            this.btnTemplate1.Text = "1158_btnTempLocacion";
            this.btnTemplate1.Visible = false;
            this.btnTemplate1.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnTemplate2
            // 
            this.btnTemplate2.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate2.Appearance.Options.UseFont = true;
            this.btnTemplate2.Appearance.Options.UseTextOptions = true;
            this.btnTemplate2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate2.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate2.Image")));
            this.btnTemplate2.Location = new System.Drawing.Point(16, 37);
            this.btnTemplate2.Name = "btnTemplate2";
            this.btnTemplate2.Size = new System.Drawing.Size(153, 37);
            this.btnTemplate2.TabIndex = 25;
            this.btnTemplate2.Text = "1158_btnTempLocTarea";
            this.btnTemplate2.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnTemplate3
            // 
            this.btnTemplate3.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnTemplate3.Appearance.Options.UseFont = true;
            this.btnTemplate3.Appearance.Options.UseTextOptions = true;
            this.btnTemplate3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnTemplate3.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate3.Image")));
            this.btnTemplate3.Location = new System.Drawing.Point(201, 37);
            this.btnTemplate3.Name = "btnTemplate3";
            this.btnTemplate3.Size = new System.Drawing.Size(141, 37);
            this.btnTemplate3.TabIndex = 26;
            this.btnTemplate3.Text = "1158_btnTempLocDeta";
            this.btnTemplate3.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // groupTemplate
            // 
            this.groupTemplate.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupTemplate.AppearanceCaption.Options.UseFont = true;
            this.groupTemplate.Controls.Add(this.btnTemplate2);
            this.groupTemplate.Controls.Add(this.btnTemplate3);
            this.groupTemplate.Location = new System.Drawing.Point(13, 92);
            this.groupTemplate.Name = "groupTemplate";
            this.groupTemplate.Size = new System.Drawing.Size(367, 94);
            this.groupTemplate.TabIndex = 28;
            this.groupTemplate.Text = "Generar Plantilla";
            // 
            // groupImportar
            // 
            this.groupImportar.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupImportar.AppearanceCaption.Options.UseFont = true;
            this.groupImportar.Controls.Add(this.btnImportLocacionTarea);
            this.groupImportar.Controls.Add(this.btnImportLocacionDeta);
            this.groupImportar.Location = new System.Drawing.Point(13, 199);
            this.groupImportar.Name = "groupImportar";
            this.groupImportar.Size = new System.Drawing.Size(367, 103);
            this.groupImportar.TabIndex = 29;
            this.groupImportar.Text = "Importar Archivos";
            // 
            // groupProy
            // 
            this.groupProy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupProy.AppearanceCaption.Options.UseFont = true;
            this.groupProy.Controls.Add(this.txtNro);
            this.groupProy.Controls.Add(this.masterProyecto);
            this.groupProy.Controls.Add(this.masterPrefijo);
            this.groupProy.Controls.Add(this.btnQueryDoc);
            this.groupProy.Location = new System.Drawing.Point(13, 11);
            this.groupProy.Margin = new System.Windows.Forms.Padding(2);
            this.groupProy.Name = "groupProy";
            this.groupProy.Size = new System.Drawing.Size(367, 76);
            this.groupProy.TabIndex = 113;
            this.groupProy.Text = "Filtrar Proyecto";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(8, 20);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(317, 23);
            this.masterProyecto.TabIndex = 110;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.masterProyecto_Leave);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(8, 44);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(295, 26);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(346, 47);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(16, 20);
            this.btnQueryDoc.TabIndex = 3;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "Buscar Documento";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(305, 47);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(40, 20);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // MigrarLocacionEntregas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(394, 392);
            this.Controls.Add(this.btnTemplate1);
            this.Controls.Add(this.groupProy);
            this.Controls.Add(this.btnImportLocacion);
            this.Controls.Add(this.groupImportar);
            this.Controls.Add(this.groupTemplate);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.lblMessages);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MigrarLocacionEntregas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.groupTemplate, 0);
            this.Controls.SetChildIndex(this.groupImportar, 0);
            this.Controls.SetChildIndex(this.btnImportLocacion, 0);
            this.Controls.SetChildIndex(this.groupProy, 0);
            this.Controls.SetChildIndex(this.btnTemplate1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTemplate)).EndInit();
            this.groupTemplate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupImportar)).EndInit();
            this.groupImportar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).EndInit();
            this.groupProy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessages;
        private DevExpress.XtraEditors.SimpleButton btnImportLocacionTarea;
        private DevExpress.XtraEditors.SimpleButton btnImportLocacion;
        private DevExpress.XtraEditors.SimpleButton btnImportLocacionDeta;
        private DevExpress.XtraEditors.SimpleButton btnInconsistencias;
        private DevExpress.XtraEditors.SimpleButton btnTemplate1;
        private DevExpress.XtraEditors.SimpleButton btnTemplate2;
        private DevExpress.XtraEditors.SimpleButton btnTemplate3;
        private DevExpress.XtraEditors.GroupControl groupTemplate;
        private DevExpress.XtraEditors.GroupControl groupImportar;
        private DevExpress.XtraEditors.GroupControl groupProy;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.TextEdit txtNro;
    }
}