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
            this.btnPreliminar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lkp_periodo = new DevExpress.XtraEditors.LookUpEdit();
            this.btnProcesar = new System.Windows.Forms.Button();
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
            this.lblFechaAplica = new System.Windows.Forms.Label();
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            this.masterPagaduria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnTemplate = new DevExpress.XtraEditors.SimpleButton();
            this.txtValorPag = new DevExpress.XtraEditors.TextEdit();
            this.lblValorPag = new System.Windows.Forms.Label();
            this.cmbTipoPersona = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoPersona = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorNeto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIni.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorInconsistencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorPag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPersona.Properties)).BeginInit();
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
            // btnPreliminar
            // 
            this.btnPreliminar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreliminar.Location = new System.Drawing.Point(187, 193);
            this.btnPreliminar.Name = "btnPreliminar";
            this.btnPreliminar.Size = new System.Drawing.Size(152, 33);
            this.btnPreliminar.TabIndex = 7;
            this.btnPreliminar.Text = "1123_btnProcesar";
            this.btnPreliminar.UseVisualStyleBackColor = true;
            this.btnPreliminar.Click += new System.EventHandler(this.btnPreliminar_Click);
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
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(13, 233);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 8;
            this.btnProcesar.Text = "1123_btnPagar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClean.Image = ((System.Drawing.Image)(resources.GetObject("btnClean.Image")));
            this.btnClean.Location = new System.Drawing.Point(483, 13);
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
            this.txtValorNeto.Location = new System.Drawing.Point(382, 154);
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
            this.lblValorNeto.Location = new System.Drawing.Point(268, 157);
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
            this.txtValorInconsistencia.Location = new System.Drawing.Point(382, 131);
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
            this.lblValorInconsistencia.Location = new System.Drawing.Point(268, 134);
            this.lblValorInconsistencia.Name = "lblValorInconsistencia";
            this.lblValorInconsistencia.Size = new System.Drawing.Size(114, 14);
            this.lblValorInconsistencia.TabIndex = 109;
            this.lblValorInconsistencia.Text = "Valor Inconsistencia";
            // 
            // lblFechaAplica
            // 
            this.lblFechaAplica.AutoSize = true;
            this.lblFechaAplica.Location = new System.Drawing.Point(274, 18);
            this.lblFechaAplica.Name = "lblFechaAplica";
            this.lblFechaAplica.Size = new System.Drawing.Size(90, 13);
            this.lblFechaAplica.TabIndex = 111;
            this.lblFechaAplica.Text = "167_FechaAplica";
            this.lblFechaAplica.Visible = false;
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(377, 15);
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
            this.dtFechaAplica.TabIndex = 110;
            this.dtFechaAplica.Visible = false;
            // 
            // masterPagaduria
            // 
            this.masterPagaduria.BackColor = System.Drawing.Color.Transparent;
            this.masterPagaduria.Filtros = null;
            this.masterPagaduria.Location = new System.Drawing.Point(28, 77);
            this.masterPagaduria.Name = "masterPagaduria";
            this.masterPagaduria.Size = new System.Drawing.Size(291, 25);
            this.masterPagaduria.TabIndex = 113;
            this.masterPagaduria.Value = "";
            this.masterPagaduria.Visible = false;
            this.masterPagaduria.Leave += new System.EventHandler(this.masterPag_Leave);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Image = ((System.Drawing.Image)(resources.GetObject("btnTemplate.Image")));
            this.btnTemplate.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnTemplate.Location = new System.Drawing.Point(484, 39);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(40, 36);
            this.btnTemplate.TabIndex = 114;
            this.btnTemplate.ToolTip = "Generar Plantilla";
            this.btnTemplate.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Application;
            this.btnTemplate.ToolTipTitle = "Generar Plantilla";
            this.btnTemplate.Visible = false;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // txtValorPag
            // 
            this.txtValorPag.EditValue = "0,00 ";
            this.txtValorPag.Location = new System.Drawing.Point(128, 154);
            this.txtValorPag.Name = "txtValorPag";
            this.txtValorPag.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorPag.Properties.Appearance.Options.UseFont = true;
            this.txtValorPag.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorPag.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorPag.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorPag.Properties.Mask.EditMask = "c";
            this.txtValorPag.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorPag.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorPag.Properties.NullText = "0";
            this.txtValorPag.Size = new System.Drawing.Size(100, 20);
            this.txtValorPag.TabIndex = 115;
            // 
            // lblValorPag
            // 
            this.lblValorPag.AutoSize = true;
            this.lblValorPag.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPag.Location = new System.Drawing.Point(25, 157);
            this.lblValorPag.Name = "lblValorPag";
            this.lblValorPag.Size = new System.Drawing.Size(66, 14);
            this.lblValorPag.TabIndex = 116;
            this.lblValorPag.Text = "Valor Total";
            // 
            // cmbTipoPersona
            // 
            this.cmbTipoPersona.Location = new System.Drawing.Point(128, 132);
            this.cmbTipoPersona.Name = "cmbTipoPersona";
            this.cmbTipoPersona.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoPersona.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoPersona.Properties.DisplayMember = "Value";
            this.cmbTipoPersona.Properties.NullText = " ";
            this.cmbTipoPersona.Properties.ValueMember = "Key";
            this.cmbTipoPersona.Size = new System.Drawing.Size(100, 20);
            this.cmbTipoPersona.Visible = false;
            this.cmbTipoPersona.TabIndex = 117;
            // 
            // lblTipoCliente
            // 
            this.lblTipoPersona.AutoSize = true;
            this.lblTipoPersona.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoPersona.Location = new System.Drawing.Point(25, 134);
            this.lblTipoPersona.Name = "lblTipoCliente";
            this.lblTipoPersona.Size = new System.Drawing.Size(72, 14);
            this.lblTipoPersona.TabIndex = 118;
            this.lblTipoPersona.Visible = false;
            this.lblTipoPersona.Text = "Tipo Cliente";
            // 
            // RecaudosMasivosFin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(533, 311);
            this.Controls.Add(this.cmbTipoPersona);
            this.Controls.Add(this.lblTipoPersona);
            this.Controls.Add(this.txtValorPag);
            this.Controls.Add(this.lblValorPag);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.masterPagaduria);
            this.Controls.Add(this.lblFechaAplica);
            this.Controls.Add(this.dtFechaAplica);
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
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lkp_periodo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnPreliminar);
            this.Name = "RecaudosMasivosFin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.btnPreliminar, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lkp_periodo, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
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
            this.Controls.SetChildIndex(this.dtFechaAplica, 0);
            this.Controls.SetChildIndex(this.lblFechaAplica, 0);
            this.Controls.SetChildIndex(this.masterPagaduria, 0);
            this.Controls.SetChildIndex(this.btnTemplate, 0);
            this.Controls.SetChildIndex(this.lblValorPag, 0);
            this.Controls.SetChildIndex(this.txtValorPag, 0);
            this.Controls.SetChildIndex(this.lblTipoPersona, 0);
            this.Controls.SetChildIndex(this.cmbTipoPersona, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_periodo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorNeto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIni.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorInconsistencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorPag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPersona.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnPreliminar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit lkp_periodo;
        private System.Windows.Forms.Button btnProcesar;
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
        private System.Windows.Forms.Label lblFechaAplica;
        protected DevExpress.XtraEditors.DateEdit dtFechaAplica;
        private ControlsUC.uc_MasterFind masterPagaduria;
        private DevExpress.XtraEditors.SimpleButton btnTemplate;
        private DevExpress.XtraEditors.TextEdit txtValorPag;
        private System.Windows.Forms.Label lblValorPag;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoPersona;
        private System.Windows.Forms.Label lblTipoPersona;
    }
}