namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SustitucionCreditos
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SustitucionCreditos));
            this.masterCompradorCartera = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClean = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 364);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(360, 18);
            // 
            // masterVendedor
            // 
            this.masterCompradorCartera.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorCartera.Filtros = null;
            this.masterCompradorCartera.Location = new System.Drawing.Point(28, 101);
            this.masterCompradorCartera.Name = "masterVendedor";
            this.masterCompradorCartera.Size = new System.Drawing.Size(299, 25);
            this.masterCompradorCartera.TabIndex = 1;
            this.masterCompradorCartera.Value = "";
            this.masterCompradorCartera.Leave += new System.EventHandler(this.masterCompradorCartera_Leave);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(180, 339);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Location = new System.Drawing.Point(105, 195);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(152, 33);
            this.btnImportar.TabIndex = 5;
            this.btnImportar.Text = "1123_btnImportar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTemplate.Location = new System.Drawing.Point(105, 152);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(152, 33);
            this.btnTemplate.TabIndex = 4;
            this.btnTemplate.Text = "1123_btnTemplate";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(106, 238);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 6;
            this.btnProcesar.Text = "1123_btnPagar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = false;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(128, 25);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            this.dtPeriod.TabIndex = 13;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(25, 27);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(77, 13);
            this.lblPeriod.TabIndex = 12;
            this.lblPeriod.Text = "1123_lblPeriod";
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInconsistencias.Location = new System.Drawing.Point(106, 280);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(152, 33);
            this.btnInconsistencias.TabIndex = 8;
            this.btnInconsistencias.Text = "1123_btnInconsisten";
            this.btnInconsistencias.UseVisualStyleBackColor = true;
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(128, 62);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "1123_lblFecha";
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClean.Image = ((System.Drawing.Image)(resources.GetObject("btnClean.Image")));
            this.btnClean.Location = new System.Drawing.Point(297, 25);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(41, 23);
            this.btnClean.TabIndex = 10;
            this.btnClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // SustitucionCreditos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(360, 382);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.masterCompradorCartera);
            this.Name = "SustitucionCreditos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.masterCompradorCartera, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.btnTemplate, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_MasterFind masterCompradorCartera;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button btnClean;

    }
}