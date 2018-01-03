namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MigracionNomina
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigracionNomina));
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.masterPagaduria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocTercero = new System.Windows.Forms.Label();
            this.txtDocTercero = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lkp_periodo = new DevExpress.XtraEditors.LookUpEdit();
            this.btnPagar = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 348);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(360, 18);
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(28, 71);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(291, 25);
            this.masterCentroPago.TabIndex = 1;
            this.masterCentroPago.Value = "";
            this.masterCentroPago.Leave += new System.EventHandler(this.masterCentroPago_Leave);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(180, 299);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Location = new System.Drawing.Point(28, 249);
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
            this.btnTemplate.Location = new System.Drawing.Point(28, 210);
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
            this.btnProcesar.Location = new System.Drawing.Point(186, 210);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 6;
            this.btnProcesar.Text = "1123_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = false;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(128, 15);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            this.dtPeriod.TabIndex = 13;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(25, 17);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(77, 13);
            this.lblPeriod.TabIndex = 12;
            this.lblPeriod.Text = "1123_lblPeriod";
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInconsistencias.Location = new System.Drawing.Point(105, 309);
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
            this.dtFecha.Location = new System.Drawing.Point(128, 44);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "1123_lblFecha";
            // 
            // masterPagaduria
            // 
            this.masterPagaduria.BackColor = System.Drawing.Color.Transparent;
            this.masterPagaduria.Filtros = null;
            this.masterPagaduria.Location = new System.Drawing.Point(28, 101);
            this.masterPagaduria.Name = "masterPagaduria";
            this.masterPagaduria.Size = new System.Drawing.Size(291, 25);
            this.masterPagaduria.TabIndex = 97;
            this.masterPagaduria.Value = "";
            // 
            // lblDocTercero
            // 
            this.lblDocTercero.AutoSize = true;
            this.lblDocTercero.Location = new System.Drawing.Point(25, 136);
            this.lblDocTercero.Name = "lblDocTercero";
            this.lblDocTercero.Size = new System.Drawing.Size(104, 13);
            this.lblDocTercero.TabIndex = 98;
            this.lblDocTercero.Text = "1123_lblDocTercero";
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Location = new System.Drawing.Point(128, 133);
            this.txtDocTercero.Name = "txtDocTercero";
            this.txtDocTercero.Size = new System.Drawing.Size(100, 20);
            this.txtDocTercero.TabIndex = 2;
            this.txtDocTercero.Leave += new System.EventHandler(this.txtDocTercero_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 100;
            this.label3.Text = "1123_lblSeleccion";
            // 
            // lkp_periodo
            // 
            this.lkp_periodo.Location = new System.Drawing.Point(128, 164);
            this.lkp_periodo.Name = "lkp_periodo";
            this.lkp_periodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_periodo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_periodo.Properties.DisplayMember = "Value";
            this.lkp_periodo.Properties.NullText = " ";
            this.lkp_periodo.Properties.ValueMember = "Key";
            this.lkp_periodo.Size = new System.Drawing.Size(100, 20);
            this.lkp_periodo.TabIndex = 3;
            // 
            // btnPagar
            // 
            this.btnPagar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagar.Location = new System.Drawing.Point(186, 249);
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(152, 33);
            this.btnPagar.TabIndex = 7;
            this.btnPagar.Text = "1123_btnPagar";
            this.btnPagar.UseVisualStyleBackColor = true;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClean.Image = ((System.Drawing.Image)(resources.GetObject("btnClean.Image")));
            this.btnClean.Location = new System.Drawing.Point(297, 15);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(41, 23);
            this.btnClean.TabIndex = 10;
            this.btnClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // MigracionNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(360, 366);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnPagar);
            this.Controls.Add(this.lkp_periodo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDocTercero);
            this.Controls.Add(this.lblDocTercero);
            this.Controls.Add(this.masterPagaduria);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.masterCentroPago);
            this.Name = "MigracionNomina";
            this.Controls.SetChildIndex(this.masterCentroPago, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.btnTemplate, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.masterPagaduria, 0);
            this.Controls.SetChildIndex(this.lblDocTercero, 0);
            this.Controls.SetChildIndex(this.txtDocTercero, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lkp_periodo, 0);
            this.Controls.SetChildIndex(this.btnPagar, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_MasterFind masterCentroPago;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        private ControlsUC.uc_MasterFind masterPagaduria;
        private System.Windows.Forms.Label lblDocTercero;
        private System.Windows.Forms.TextBox txtDocTercero;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit lkp_periodo;
        private System.Windows.Forms.Button btnPagar;
        protected System.Windows.Forms.Button btnClean;

    }
}