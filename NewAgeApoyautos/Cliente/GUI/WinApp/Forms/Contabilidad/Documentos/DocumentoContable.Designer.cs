namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DocumentoContable
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterDocumentoCont = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.lblMonedaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.cmbMonedaOrigen = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.masterMonedaHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuentaHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyectoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCostoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            this.lblDescrDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblValorDoc = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacionHeader = new System.Windows.Forms.TextBox();
            this.lblDocumentoCOM = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoCOMHeader = new System.Windows.Forms.TextBox();
            this.txtValorDoc = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDoc.Properties)).BeginInit();
            this.grpboxHeader.Controls.Add(this.masterLugarGeoHeader);
            this.grpboxHeader.Controls.Add(this.masterProyectoHeader);
            this.grpboxHeader.Controls.Add(this.masterCentroCostoHeader);
            this.grpboxHeader.Controls.Add(this.lblDocumentoCOM);
            this.grpboxHeader.Controls.Add(this.txtDocumentoCOMHeader);
            this.grpboxHeader.Controls.Add(this.masterTerceroHeader);
            this.grpboxHeader.Controls.Add(this.masterCuentaHeader);
            this.grpboxHeader.Controls.Add(this.lblMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.cmbMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.masterDocumentoCont);
            this.grpboxHeader.Controls.Add(this.lblNumber);
            this.grpboxHeader.Controls.Add(this.txtNumber);
            this.grpboxHeader.Controls.Add(this.masterMonedaHeader);
            this.grpboxHeader.Controls.Add(this.lblDescrDoc);
            this.grpboxHeader.Controls.Add(this.txtObservacionHeader);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            this.grpboxHeader.Controls.Add(this.lblValorDoc);
            this.grpboxHeader.Controls.Add(this.txtValorDoc);
            this.SuspendLayout();
            // 
            // masterDocumentoCont
            // 
            this.masterDocumentoCont.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumentoCont.Location = new System.Drawing.Point(10, 18);
            this.masterDocumentoCont.Name = "masterDocumentoCont";
            this.masterDocumentoCont.Size = new System.Drawing.Size(180, 24);
            this.masterDocumentoCont.TabIndex = 8;
            this.masterDocumentoCont.Enter += new System.EventHandler(this.masterDocumentoCont_Enter);
            this.masterDocumentoCont.Leave += new System.EventHandler(this.masterDocumentoCont_Leave);
            // 
            // lblMonedaOrigen
            // 
            this.lblMonedaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaOrigen.Location = new System.Drawing.Point(10, 46);
            this.lblMonedaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblMonedaOrigen.Name = "lblMonedaOrigen";
            this.lblMonedaOrigen.Size = new System.Drawing.Size(140, 13);
            this.lblMonedaOrigen.Text = "12_lblMonedaOrigen";
            this.lblMonedaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            // 
            // cmbMonedaOrigen
            // 
            this.cmbMonedaOrigen.BackColor = System.Drawing.Color.White;
            this.cmbMonedaOrigen.FormattingEnabled = true;
            this.cmbMonedaOrigen.Location = new System.Drawing.Point(111, 44);
            this.cmbMonedaOrigen.Name = "cmbMonedaOrigen";
            this.cmbMonedaOrigen.Size = new System.Drawing.Size(192, 50);
            this.cmbMonedaOrigen.TabIndex = 10;
            this.cmbMonedaOrigen.Enter += new System.EventHandler(this.cmbMonedaOrigen_Enter);
            this.cmbMonedaOrigen.Leave += new System.EventHandler(this.cmbMonedaOrigen_Leave);
            // 
            // masterCuenta
            // 
            this.masterCuentaHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterCuentaHeader.Location = new System.Drawing.Point(10, 67);
            this.masterCuentaHeader.Name = "masterCuenta";
            this.masterCuentaHeader.Size = new System.Drawing.Size(180, 22);
            this.masterCuentaHeader.TabIndex = 12;
            // 
            // masterTercero
            // 
            this.masterTerceroHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroHeader.Location = new System.Drawing.Point(10, 91);
            this.masterTerceroHeader.Name = "masterTercero";
            this.masterTerceroHeader.Size = new System.Drawing.Size(180, 22);
            this.masterTerceroHeader.TabIndex = 13;
            // 
            // lblDocumentoCOM
            // 
            this.lblDocumentoCOM.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoCOM.Location = new System.Drawing.Point(10, 116);
            this.lblDocumentoCOM.Name = "lblDocumentoCOM";
            this.lblDocumentoCOM.Size = new System.Drawing.Size(60, 13);
            this.lblDocumentoCOM.Text = "12_lblDocumentoCOM";
            // 
            // txtDocumentoCOM
            // 
            this.txtDocumentoCOMHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoCOMHeader.Location = new System.Drawing.Point(111, 115);
            this.txtDocumentoCOMHeader.Multiline = true;
            this.txtDocumentoCOMHeader.Name = "txtDocumentoCOM";
            this.txtDocumentoCOMHeader.Size = new System.Drawing.Size(100, 19);
            this.txtDocumentoCOMHeader.TabIndex = 14;
            // 
            // lblNumber
            // 
            this.lblNumber.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.Location = new System.Drawing.Point(320, 23);
            this.lblNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(42, 13);
            this.lblNumber.Text = "12_lblNumber";
            this.lblNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            // 
            // txtNumber
            // 
            this.txtNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumber.Location = new System.Drawing.Point(420, 20);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(48, 19);
            this.txtNumber.TabIndex = 9;
            this.txtNumber.Multiline = true;
            this.txtNumber.Enter += new System.EventHandler(this.txtNumber_Enter);
            this.txtNumber.Leave += new System.EventHandler(this.txtNumber_Leave);
            this.txtNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            // 
            // masterMoneda
            // 
            this.masterMonedaHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterMonedaHeader.Location = new System.Drawing.Point(320, 42);
            this.masterMonedaHeader.Name = "masterMoneda";
            this.masterMonedaHeader.Size = new System.Drawing.Size(180, 22);
            this.masterMonedaHeader.TabIndex = 11;
            // 
            // masterProyecto
            // 
            this.masterProyectoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoHeader.Location = new System.Drawing.Point(320, 67);
            this.masterProyectoHeader.Name = "masterProyecto";
            this.masterProyectoHeader.Size = new System.Drawing.Size(180, 22);
            this.masterProyectoHeader.TabIndex = 15;
            // 
            // masterCentroCosto
            // 
            this.masterCentroCostoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCostoHeader.Location = new System.Drawing.Point(320, 91);
            this.masterCentroCostoHeader.Name = "masterCentroCosto";
            this.masterCentroCostoHeader.Size = new System.Drawing.Size(180, 22);
            this.masterCentroCostoHeader.TabIndex = 16;
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeoHeader.Location = new System.Drawing.Point(320, 114);
            this.masterLugarGeoHeader.Name = "masterLugarGeo";
            this.masterLugarGeoHeader.Size = new System.Drawing.Size(180, 22);
            this.masterLugarGeoHeader.TabIndex = 17;
            // 
            // lblDescrDoc
            // 
            this.lblDescrDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrDoc.Location = new System.Drawing.Point(648, 23);
            this.lblDescrDoc.Name = "lblDescrDoc";
            this.lblDescrDoc.Size = new System.Drawing.Size(60, 13);
            this.lblDescrDoc.Text = "12_lblDescrDoc";
            // 
            // txtDescrDoc
            // 
            this.txtObservacionHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacionHeader.Location = new System.Drawing.Point(737, 20);
            this.txtObservacionHeader.Multiline = true;
            this.txtObservacionHeader.Name = "txtDescrDoc";
            this.txtObservacionHeader.Size = new System.Drawing.Size(230, 66);
            this.txtObservacionHeader.TabIndex = 18;
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.Location = new System.Drawing.Point(648, 91);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(130, 13);
            this.lblTasaCambio.Text = "12_lblTasaCambio";
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.Location = new System.Drawing.Point(737, 89);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(130, 20);
            this.txtTasaCambio.TabIndex = 19;
            this.txtTasaCambio.Text = "0";
            this.txtTasaCambio.Enabled = false;
            // 
            // lblValueDoc
            // 
            this.lblValorDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorDoc.Location = new System.Drawing.Point(648, 115);
            this.lblValorDoc.Name = "lblValueDoc";
            this.lblValorDoc.Size = new System.Drawing.Size(60, 13);
            this.lblValorDoc.Text = "12_lblValueDoc";
            // 
            // txtValorDoc
            // 
            this.txtValorDoc.EditValue = "0";
            this.txtValorDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorDoc.Location = new System.Drawing.Point(737, 113);
            this.txtValorDoc.Name = "txtValueDoc";
            this.txtValorDoc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorDoc.Properties.Mask.EditMask = "c";
            this.txtValorDoc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorDoc.Size = new System.Drawing.Size(130, 20);
            this.txtValorDoc.TabIndex = 20;
            this.txtValorDoc.Enabled = false;

            ((System.ComponentModel.ISupportInitialize)(this.txtValorDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            this.ResumeLayout(false);
        }
        
        #endregion
        private ControlsUC.uc_MasterFind masterDocumentoCont;
        private DevExpress.XtraEditors.LabelControl lblNumber;
        private System.Windows.Forms.TextBox txtNumber;
        private ControlsUC.uc_MasterFind masterMonedaHeader;
        private ControlsUC.uc_MasterFind masterCuentaHeader;
        private ControlsUC.uc_MasterFind masterTerceroHeader;
        private DevExpress.XtraEditors.LabelControl lblDocumentoCOM;
        private System.Windows.Forms.TextBox txtDocumentoCOMHeader;
        private ControlsUC.uc_MasterFind masterProyectoHeader;
        private ControlsUC.uc_MasterFind masterCentroCostoHeader;
        private ControlsUC.uc_MasterFind masterLugarGeoHeader;
        private DevExpress.XtraEditors.LabelControl lblDescrDoc;
        private System.Windows.Forms.TextBox txtObservacionHeader;
        private DevExpress.XtraEditors.LabelControl lblMonedaOrigen;
        private NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx cmbMonedaOrigen;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl lblValorDoc;
        private DevExpress.XtraEditors.TextEdit txtValorDoc;


    }
}