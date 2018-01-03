namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RecaudosMasivosFin
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecaudosMasivosFin));
            this.lblMessages = new System.Windows.Forms.Label();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnPremilinar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lkp_periodo = new DevExpress.XtraEditors.LookUpEdit();
            this.btnPagar = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.txtValorNeto = new DevExpress.XtraEditors.TextEdit();
            this.lblValorNeto = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFechaCierre = new DevExpress.XtraEditors.DateEdit();
            this.masterBanco = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnRelPagos = new System.Windows.Forms.Button();
            this.btnReporte = new System.Windows.Forms.Button();
            this.txtValorIni = new DevExpress.XtraEditors.TextEdit();
            this.lblValorInicial = new System.Windows.Forms.Label();
            this.txtValorInconsistencia = new DevExpress.XtraEditors.TextEdit();
            this.lblValorInconsistencia = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorNeto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIni.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorInconsistencia.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 293);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(533, 18);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(180, 332);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Location = new System.Drawing.Point(13, 192);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(152, 33);
            this.btnImportar.TabIndex = 6;
            this.btnImportar.Text = "1123_btnImportar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnPremilinar
            // 
            this.btnPremilinar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPremilinar.Location = new System.Drawing.Point(187, 193);
            this.btnPremilinar.Name = "btnPremilinar";
            this.btnPremilinar.Size = new System.Drawing.Size(152, 33);
            this.btnPremilinar.TabIndex = 7;
            this.btnPremilinar.Text = "1123_btnProcesar";
            this.btnPremilinar.UseVisualStyleBackColor = true;
            this.btnPremilinar.Click += new System.EventHandler(this.btnPremilinar_Click);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = false;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(130, 15);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            this.dtPeriod.TabIndex = 0;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(25, 18);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 12;
            this.lblPeriod.Text = "1123_lblPeriod";
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInconsistencias.Location = new System.Drawing.Point(362, 233);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(152, 33);
            this.btnInconsistencias.TabIndex = 10;
            this.btnInconsistencias.Text = "1123_btnInconsisten";
            this.btnInconsistencias.UseVisualStyleBackColor = true;
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(378, 46);
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
            this.dtFecha.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(274, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 14);
            this.label1.TabIndex = 96;
            this.label1.Text = "1123_lblFecha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 14);
            this.label3.TabIndex = 100;
            this.label3.Text = "1123_lblSeleccion";
            // 
            // lkp_periodo
            // 
            this.lkp_periodo.Location = new System.Drawing.Point(128, 109);
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
            this.lkp_periodo.TabIndex = 4;
            // 
            // btnPagar
            // 
            this.btnPagar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagar.Location = new System.Drawing.Point(13, 233);
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(152, 33);
            this.btnPagar.TabIndex = 8;
            this.btnPagar.Text = "1123_btnPagar";
            this.btnPagar.UseVisualStyleBackColor = true;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClean.Image = ((System.Drawing.Image)(resources.GetObject("btnClean.Image")));
            this.btnClean.Location = new System.Drawing.Point(485, 5);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(41, 23);
            this.btnClean.TabIndex = 11;
            this.btnClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // txtValorNeto
            // 
            this.txtValorNeto.EditValue = "0,00 ";
            this.txtValorNeto.Location = new System.Drawing.Point(382, 151);
            this.txtValorNeto.Name = "txtValorNeto";
            this.txtValorNeto.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorNeto.Properties.Appearance.Options.UseFont = true;
            this.txtValorNeto.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorNeto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorNeto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorNeto.Properties.Mask.EditMask = "c";
            this.txtValorNeto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorNeto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorNeto.Properties.NullText = "0";
            this.txtValorNeto.Properties.ReadOnly = true;
            this.txtValorNeto.Size = new System.Drawing.Size(121, 20);
            this.txtValorNeto.TabIndex = 5;
            // 
            // lblValorNeto
            // 
            this.lblValorNeto.AutoSize = true;
            this.lblValorNeto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorNeto.Location = new System.Drawing.Point(268, 154);
            this.lblValorNeto.Name = "lblValorNeto";
            this.lblValorNeto.Size = new System.Drawing.Size(85, 14);
            this.lblValorNeto.TabIndex = 101;
            this.lblValorNeto.Text = "Valor Recaudo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 14);
            this.label2.TabIndex = 104;
            this.label2.Text = "1123_lblUltFechaCierre";
            // 
            // dtFechaCierre
            // 
            this.dtFechaCierre.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaCierre.Enabled = false;
            this.dtFechaCierre.Location = new System.Drawing.Point(131, 46);
            this.dtFechaCierre.Name = "dtFechaCierre";
            this.dtFechaCierre.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCierre.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCierre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCierre.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCierre.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCierre.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCierre.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCierre.Size = new System.Drawing.Size(100, 20);
            this.dtFechaCierre.TabIndex = 1;
            // 
            // masterBanco
            // 
            this.masterBanco.BackColor = System.Drawing.Color.Transparent;
            this.masterBanco.Filtros = null;
            this.masterBanco.Location = new System.Drawing.Point(28, 74);
            this.masterBanco.Name = "masterBanco";
            this.masterBanco.Size = new System.Drawing.Size(291, 25);
            this.masterBanco.TabIndex = 3;
            this.masterBanco.Value = "";
            this.masterBanco.Leave += new System.EventHandler(this.masterBanco_Leave);
            // 
            // btnRelPagos
            // 
            this.btnRelPagos.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelPagos.Location = new System.Drawing.Point(362, 192);
            this.btnRelPagos.Name = "btnRelPagos";
            this.btnRelPagos.Size = new System.Drawing.Size(152, 33);
            this.btnRelPagos.TabIndex = 9;
            this.btnRelPagos.Text = "1123_btnRelPagos";
            this.btnRelPagos.UseVisualStyleBackColor = true;
            this.btnRelPagos.Click += new System.EventHandler(this.btnRelPagos_Click);
            // 
            // btnReporte
            // 
            this.btnReporte.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReporte.Location = new System.Drawing.Point(187, 233);
            this.btnReporte.Name = "btnReporte";
            this.btnReporte.Size = new System.Drawing.Size(152, 33);
            this.btnReporte.TabIndex = 105;
            this.btnReporte.Text = "1123_btnReporte";
            this.btnReporte.UseVisualStyleBackColor = true;
            this.btnReporte.Click += new System.EventHandler(this.btnReporte_Click);
            // 
            // txtValorIni
            // 
            this.txtValorIni.EditValue = "0,00 ";
            this.txtValorIni.Location = new System.Drawing.Point(382, 109);
            this.txtValorIni.Name = "txtValorIni";
            this.txtValorIni.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorIni.Properties.Appearance.Options.UseFont = true;
            this.txtValorIni.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorIni.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorIni.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorIni.Properties.Mask.EditMask = "c";
            this.txtValorIni.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorIni.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorIni.Properties.NullText = "0";
            this.txtValorIni.Properties.ReadOnly = true;
            this.txtValorIni.Size = new System.Drawing.Size(121, 20);
            this.txtValorIni.TabIndex = 106;
            // 
            // lblValorInicial
            // 
            this.lblValorInicial.AutoSize = true;
            this.lblValorInicial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorInicial.Location = new System.Drawing.Point(268, 112);
            this.lblValorInicial.Name = "lblValorInicial";
            this.lblValorInicial.Size = new System.Drawing.Size(66, 14);
            this.lblValorInicial.TabIndex = 107;
            this.lblValorInicial.Text = "Valor Total";
            // 
            // txtValorInconsistencia
            // 
            this.txtValorInconsistencia.EditValue = "0,00 ";
            this.txtValorInconsistencia.Location = new System.Drawing.Point(382, 130);
            this.txtValorInconsistencia.Name = "txtValorInconsistencia";
            this.txtValorInconsistencia.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorInconsistencia.Properties.Appearance.Options.UseFont = true;
            this.txtValorInconsistencia.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorInconsistencia.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorInconsistencia.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorInconsistencia.Properties.Mask.EditMask = "c";
            this.txtValorInconsistencia.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorInconsistencia.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorInconsistencia.Properties.NullText = "0";
            this.txtValorInconsistencia.Properties.ReadOnly = true;
            this.txtValorInconsistencia.Size = new System.Drawing.Size(121, 20);
            this.txtValorInconsistencia.TabIndex = 108;
            // 
            // lblValorInconsistencia
            // 
            this.lblValorInconsistencia.AutoSize = true;
            this.lblValorInconsistencia.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorInconsistencia.Location = new System.Drawing.Point(268, 133);
            this.lblValorInconsistencia.Name = "lblValorInconsistencia";
            this.lblValorInconsistencia.Size = new System.Drawing.Size(114, 14);
            this.lblValorInconsistencia.TabIndex = 109;
            this.lblValorInconsistencia.Text = "Valor Inconsistencia";
            // 
            // RecaudosMasivosFin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(533, 311);
            this.Controls.Add(this.txtValorInconsistencia);
            this.Controls.Add(this.lblValorInconsistencia);
            this.Controls.Add(this.txtValorIni);
            this.Controls.Add(this.lblValorInicial);
            this.Controls.Add(this.dtFechaCierre);
            this.Controls.Add(this.btnReporte);
            this.Controls.Add(this.btnRelPagos);
            this.Controls.Add(this.masterBanco);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtValorNeto);
            this.Controls.Add(this.lblValorNeto);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnPagar);
            this.Controls.Add(this.lkp_periodo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnPremilinar);
            this.Name = "RecaudosMasivosFin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.btnPremilinar, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lkp_periodo, 0);
            this.Controls.SetChildIndex(this.btnPagar, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            this.Controls.SetChildIndex(this.lblValorNeto, 0);
            this.Controls.SetChildIndex(this.txtValorNeto, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.masterBanco, 0);
            this.Controls.SetChildIndex(this.btnRelPagos, 0);
            this.Controls.SetChildIndex(this.btnReporte, 0);
            this.Controls.SetChildIndex(this.dtFechaCierre, 0);
            this.Controls.SetChildIndex(this.lblValorInicial, 0);
            this.Controls.SetChildIndex(this.txtValorIni, 0);
            this.Controls.SetChildIndex(this.lblValorInconsistencia, 0);
            this.Controls.SetChildIndex(this.txtValorInconsistencia, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorNeto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIni.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorInconsistencia.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnPremilinar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit lkp_periodo;
        private System.Windows.Forms.Button btnPagar;
        protected System.Windows.Forms.Button btnClean;
        private DevExpress.XtraEditors.TextEdit txtValorNeto;
        private System.Windows.Forms.Label lblValorNeto;
        private System.Windows.Forms.Label label2;
        protected DevExpress.XtraEditors.DateEdit dtFechaCierre;
        private ControlsUC.uc_MasterFind masterBanco;
        private System.Windows.Forms.Button btnRelPagos;
        private System.Windows.Forms.Button btnReporte;
        private DevExpress.XtraEditors.TextEdit txtValorIni;
        private System.Windows.Forms.Label lblValorInicial;
        private DevExpress.XtraEditors.TextEdit txtValorInconsistencia;
        private System.Windows.Forms.Label lblValorInconsistencia;

    }
}