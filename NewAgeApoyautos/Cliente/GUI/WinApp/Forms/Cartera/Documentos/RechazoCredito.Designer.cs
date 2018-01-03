namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RechazoCredito
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterBanco = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.txtVlrPrestamo = new DevExpress.XtraEditors.TextEdit();
            this.lblValorPrestamo = new System.Windows.Forms.Label();
            this.txtVlrGiro = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrGiro = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPrestamo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 306);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(360, 18);
            // 
            // masterBanco
            // 
            this.masterBanco.BackColor = System.Drawing.Color.Transparent;
            this.masterBanco.Filtros = null;
            this.masterBanco.Location = new System.Drawing.Point(28, 104);
            this.masterBanco.Name = "masterBanco";
            this.masterBanco.Size = new System.Drawing.Size(291, 25);
            this.masterBanco.TabIndex = 3;
            this.masterBanco.Value = "";
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(106, 247);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 5;
            this.btnProcesar.Text = "1106_btnProcesar";
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
            this.dtPeriod.TabIndex = 0;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPeriod.Location = new System.Drawing.Point(25, 18);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 12;
            this.lblPeriod.Text = "1123_lblPeriod";
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
            this.dtFecha.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label1.Location = new System.Drawing.Point(25, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 14);
            this.label1.TabIndex = 96;
            this.label1.Text = "1123_lblFecha";
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(128, 73);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(114, 22);
            this.txtLibranza.TabIndex = 2;
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblLibranza.Location = new System.Drawing.Point(25, 77);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(92, 14);
            this.lblLibranza.TabIndex = 103;
            this.lblLibranza.Text = "32551_Libranza";
            // 
            // txtVlrPrestamo
            // 
            this.txtVlrPrestamo.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtVlrPrestamo.Location = new System.Drawing.Point(128, 168);
            this.txtVlrPrestamo.Name = "txtVlrPrestamo";
            this.txtVlrPrestamo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrPrestamo.Properties.Appearance.Options.UseFont = true;
            this.txtVlrPrestamo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrPrestamo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrPrestamo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrPrestamo.Properties.Mask.EditMask = "c";
            this.txtVlrPrestamo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrPrestamo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrPrestamo.Properties.ReadOnly = true;
            this.txtVlrPrestamo.Size = new System.Drawing.Size(114, 20);
            this.txtVlrPrestamo.TabIndex = 106;
            // 
            // lblValorPrestamo
            // 
            this.lblValorPrestamo.AutoSize = true;
            this.lblValorPrestamo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPrestamo.Location = new System.Drawing.Point(25, 171);
            this.lblValorPrestamo.Name = "lblValorPrestamo";
            this.lblValorPrestamo.Size = new System.Drawing.Size(127, 14);
            this.lblValorPrestamo.TabIndex = 105;
            this.lblValorPrestamo.Text = "32555_ValorPrestamo";
            // 
            // txtVlrGiro
            // 
            this.txtVlrGiro.EditValue = "0";
            this.txtVlrGiro.Location = new System.Drawing.Point(128, 200);
            this.txtVlrGiro.Name = "txtVlrGiro";
            this.txtVlrGiro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrGiro.Properties.Appearance.Options.UseFont = true;
            this.txtVlrGiro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrGiro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrGiro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrGiro.Properties.Mask.EditMask = "c";
            this.txtVlrGiro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrGiro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrGiro.Properties.ReadOnly = true;
            this.txtVlrGiro.Size = new System.Drawing.Size(114, 20);
            this.txtVlrGiro.TabIndex = 108;
            // 
            // lblVlrGiro
            // 
            this.lblVlrGiro.AutoSize = true;
            this.lblVlrGiro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrGiro.Location = new System.Drawing.Point(24, 203);
            this.lblVlrGiro.Name = "lblVlrGiro";
            this.lblVlrGiro.Size = new System.Drawing.Size(97, 14);
            this.lblVlrGiro.TabIndex = 107;
            this.lblVlrGiro.Text = "32555_ValorGiro";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(27, 135);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(291, 25);
            this.masterCliente.TabIndex = 4;
            this.masterCliente.Value = "";
            // 
            // RechazoCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(360, 324);
            this.Controls.Add(this.masterCliente);
            this.Controls.Add(this.txtVlrGiro);
            this.Controls.Add(this.lblVlrGiro);
            this.Controls.Add(this.txtVlrPrestamo);
            this.Controls.Add(this.lblValorPrestamo);
            this.Controls.Add(this.txtLibranza);
            this.Controls.Add(this.lblLibranza);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.masterBanco);
            this.Name = "RechazoCredito";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.masterBanco, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblLibranza, 0);
            this.Controls.SetChildIndex(this.txtLibranza, 0);
            this.Controls.SetChildIndex(this.lblValorPrestamo, 0);
            this.Controls.SetChildIndex(this.txtVlrPrestamo, 0);
            this.Controls.SetChildIndex(this.lblVlrGiro, 0);
            this.Controls.SetChildIndex(this.txtVlrGiro, 0);
            this.Controls.SetChildIndex(this.masterCliente, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPrestamo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_MasterFind masterBanco;
        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private DevExpress.XtraEditors.TextEdit txtVlrPrestamo;
        private System.Windows.Forms.Label lblValorPrestamo;
        private DevExpress.XtraEditors.TextEdit txtVlrGiro;
        private System.Windows.Forms.Label lblVlrGiro;
        private ControlsUC.uc_MasterFind masterCliente;

    }
}