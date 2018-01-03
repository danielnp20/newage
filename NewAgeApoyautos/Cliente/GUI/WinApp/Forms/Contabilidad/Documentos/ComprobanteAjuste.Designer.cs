namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ComprobanteAjuste
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterComprobante = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNumber = new DevExpress.XtraEditors.LabelControl();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrencySource = new DevExpress.XtraEditors.LabelControl();
            this.cmbMonedaOrigen = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.lblValorFondo = new DevExpress.XtraEditors.LabelControl();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.masterDocumento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalLocal
            // 
            this.txtTotalLocal.Properties.Appearance.Options.UseFont = true;
            this.txtTotalLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.Mask.EditMask = "c";
            this.txtTotalLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtTotalForeign
            // 
            this.txtTotalForeign.Properties.Appearance.Options.UseFont = true;
            this.txtTotalForeign.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalForeign.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalForeign.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.Mask.EditMask = "c";
            this.txtTotalForeign.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalForeign.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editValue,
            this.editValue4});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // btnMark
            // 
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.masterDocumento);
            this.grpboxHeader.Controls.Add(this.lblValorFondo);
            this.grpboxHeader.Controls.Add(this.txtValor);
            this.grpboxHeader.Controls.Add(this.masterComprobante);
            this.grpboxHeader.Controls.Add(this.lblNumber);
            this.grpboxHeader.Controls.Add(this.txtNumber);
            this.grpboxHeader.Controls.Add(this.lblCurrencySource);
            this.grpboxHeader.Controls.Add(this.cmbMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            // 
            // masterComprobante
            // 
            this.masterComprobante.BackColor = System.Drawing.Color.Transparent;
            this.masterComprobante.Filtros = null;
            this.masterComprobante.Location = new System.Drawing.Point(10, 20);
            this.masterComprobante.Name = "masterComprobante";
            this.masterComprobante.Size = new System.Drawing.Size(180, 24);
            this.masterComprobante.TabIndex = 8;
            this.masterComprobante.Value = "";
            // 
            // lblNumber
            // 
            this.lblNumber.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblNumber.Location = new System.Drawing.Point(349, 25);
            this.lblNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(96, 14);
            this.lblNumber.TabIndex = 9;
            this.lblNumber.Text = "20504_lblNumber";
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(565, 60);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(131, 14);
            this.lblTasaCambio.TabIndex = 12;
            this.lblTasaCambio.Text = "20504_lblExchangeRate";
            // 
            // lblCurrencySource
            // 
            this.lblCurrencySource.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencySource.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblCurrencySource.Location = new System.Drawing.Point(12, 60);
            this.lblCurrencySource.Margin = new System.Windows.Forms.Padding(4);
            this.lblCurrencySource.Name = "lblCurrencySource";
            this.lblCurrencySource.Size = new System.Drawing.Size(139, 14);
            this.lblCurrencySource.TabIndex = 10;
            this.lblCurrencySource.Text = "20504_lblCurrencySource";
            // 
            // cmbMonedaOrigen
            // 
            this.cmbMonedaOrigen.BackColor = System.Drawing.Color.White;
            this.cmbMonedaOrigen.Enabled = false;
            this.cmbMonedaOrigen.FormattingEnabled = true;
            this.cmbMonedaOrigen.Location = new System.Drawing.Point(110, 57);
            this.cmbMonedaOrigen.Name = "cmbMonedaOrigen";
            this.cmbMonedaOrigen.Size = new System.Drawing.Size(120, 21);
            this.cmbMonedaOrigen.TabIndex = 10;
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(250, 57);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(294, 22);
            this.masterMoneda.TabIndex = 11;
            this.masterMoneda.Value = "";
            // 
            // txtNumber
            // 
            this.txtNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumber.Location = new System.Drawing.Point(376, 22);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(48, 22);
            this.txtNumber.TabIndex = 9;
            this.txtNumber.Enter += new System.EventHandler(this.txtNumber_Enter);
            this.txtNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            this.txtNumber.Leave += new System.EventHandler(this.txtNumber_Leave);
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0";
            this.txtTasaCambio.Enabled = false;
            this.txtTasaCambio.Location = new System.Drawing.Point(663, 57);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(120, 20);
            this.txtTasaCambio.TabIndex = 12;
            // 
            // lblValorFondo
            // 
            this.lblValorFondo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorFondo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblValorFondo.Location = new System.Drawing.Point(566, 97);
            this.lblValorFondo.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorFondo.Name = "lblValorFondo";
            this.lblValorFondo.Size = new System.Drawing.Size(80, 14);
            this.lblValorFondo.TabIndex = 26;
            this.lblValorFondo.Text = "20504_lblValor";
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0";
            this.txtValor.Enabled = false;
            this.txtValor.Location = new System.Drawing.Point(663, 95);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(120, 20);
            this.txtValor.TabIndex = 27;
            // 
            // masterDocumento
            // 
            this.masterDocumento.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumento.Filtros = null;
            this.masterDocumento.Location = new System.Drawing.Point(249, 92);
            this.masterDocumento.Name = "masterDocumento";
            this.masterDocumento.Size = new System.Drawing.Size(309, 22);
            this.masterDocumento.TabIndex = 28;
            this.masterDocumento.Value = "";
            // 
            // ComprobanteAjuste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "ComprobanteAjuste";
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNumber;
        private DevExpress.XtraEditors.LabelControl lblCurrencySource;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private System.Windows.Forms.TextBox txtNumber;
        private NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx cmbMonedaOrigen;
        private ControlsUC.uc_MasterFind masterMoneda;
        private ControlsUC.uc_MasterFind masterComprobante;
        private DevExpress.XtraEditors.LabelControl lblValorFondo;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private ControlsUC.uc_MasterFind masterDocumento;
    }
}