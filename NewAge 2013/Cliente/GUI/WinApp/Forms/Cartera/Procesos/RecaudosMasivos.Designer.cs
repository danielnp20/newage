namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RecaudosMasivos
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecaudosMasivos));
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnPreliminar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.masterPagaduria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label3 = new System.Windows.Forms.Label();
            this.lkp_periodo = new DevExpress.XtraEditors.LookUpEdit();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.lblValor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 394);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(360, 18);
            // 
            // masterCentroPago
           // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(180, 329);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Location = new System.Drawing.Point(28, 279);
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
            this.btnTemplate.Location = new System.Drawing.Point(28, 240);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(152, 33);
            this.btnTemplate.TabIndex = 4;
            this.btnTemplate.Text = "1123_btnTemplate";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnProcesar
            // 
            this.btnPreliminar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreliminar.Location = new System.Drawing.Point(186, 240);
            this.btnPreliminar.Name = "btnPreliminar";
            this.btnPreliminar.Size = new System.Drawing.Size(152, 33);
            this.btnPreliminar.TabIndex = 6;
            this.btnPreliminar.Text = "1123_btnPreliminar";
            this.btnPreliminar.UseVisualStyleBackColor = true;
            this.btnPreliminar.Click += new System.EventHandler(this.btnPreliminar_Click);
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
            this.btnInconsistencias.Location = new System.Drawing.Point(105, 326);
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
            this.label1.Location = new System.Drawing.Point(25, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "167_Fecha";
            // 
            // masterPagaduria
            // 
            this.masterPagaduria.BackColor = System.Drawing.Color.Transparent;
            this.masterPagaduria.Filtros = null;
            this.masterPagaduria.Location = new System.Drawing.Point(28, 126);
            this.masterPagaduria.Name = "masterPagaduria";
            this.masterPagaduria.Size = new System.Drawing.Size(291, 25);
            this.masterPagaduria.TabIndex = 97;
            this.masterPagaduria.Value = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 100;
            this.label3.Text = "1123_lblSeleccion";
            // 
            // lkp_periodo
            // 
            this.lkp_periodo.Location = new System.Drawing.Point(128, 161);
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
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(186, 279);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 7;
            this.btnProcesar.Text = "1123_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
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
            // txtValor
            // 
            this.txtValor.EditValue = "0,00 ";
            this.txtValor.Location = new System.Drawing.Point(128, 194);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Properties.NullText = "0";
            this.txtValor.Size = new System.Drawing.Size(121, 20);
            this.txtValor.TabIndex = 102;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(26, 197);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(76, 14);
            this.lblValor.TabIndex = 101;
            this.lblValor.Text = "32551_Valor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 104;
            this.label2.Text = "167_FechaAplica";
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(128, 71);
            this.dtFechaAplica.Name = "dtFechaAplica";
            this.dtFechaAplica.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaAplica.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaAplica.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaAplica.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaAplica.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaAplica.Size = new System.Drawing.Size(100, 20);
            this.dtFechaAplica.TabIndex = 103;
            // 
            // RecaudosMasivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(360, 412);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtFechaAplica);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.lblValor);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lkp_periodo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.masterPagaduria);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnPreliminar);
            this.Name = "RecaudosMasivos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.btnPreliminar, 0);
            this.Controls.SetChildIndex(this.btnTemplate, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.masterPagaduria, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lkp_periodo, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            this.Controls.SetChildIndex(this.lblValor, 0);
            this.Controls.SetChildIndex(this.txtValor, 0);
            this.Controls.SetChildIndex(this.dtFechaAplica, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnPreliminar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        private ControlsUC.uc_MasterFind masterPagaduria;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit lkp_periodo;
        private System.Windows.Forms.Button btnProcesar;
        protected System.Windows.Forms.Button btnClean;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Label label2;
        protected DevExpress.XtraEditors.DateEdit dtFechaAplica;

    }
}