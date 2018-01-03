namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReciboCaja
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterCodigoCaja = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaRec = new DevExpress.XtraEditors.DateEdit();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.lblDescrDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblValor = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaRec = new DevExpress.XtraEditors.LabelControl();
            this.txtDescrDoc = new System.Windows.Forms.TextBox();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.btnFacturas = new System.Windows.Forms.Button();
            this.masterBancoCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblReciboNro = new DevExpress.XtraEditors.LabelControl();
            this.txtReciboNro = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRec.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRec.Properties)).BeginInit();
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
            this.grpboxHeader.Controls.Add(this.lblReciboNro);
            this.grpboxHeader.Controls.Add(this.txtReciboNro);
            this.grpboxHeader.Controls.Add(this.masterCuenta_);
            this.grpboxHeader.Controls.Add(this.masterTercero_);
            this.grpboxHeader.Controls.Add(this.masterBancoCuenta);
            this.grpboxHeader.Controls.Add(this.btnFacturas);
            this.grpboxHeader.Controls.Add(this.masterLugarGeo);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.masterCentroCosto);
            this.grpboxHeader.Controls.Add(this.masterCodigoCaja);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.lblDescrDoc);
            this.grpboxHeader.Controls.Add(this.lblFechaRec);
            this.grpboxHeader.Controls.Add(this.txtDescrDoc);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            this.grpboxHeader.Controls.Add(this.dtFechaRec);
            this.grpboxHeader.Controls.Add(this.lblValor);
            this.grpboxHeader.Controls.Add(this.txtValor);
            // 
            // masterCodigoCaja
            // 
            this.masterCodigoCaja.BackColor = System.Drawing.Color.Transparent;
            this.masterCodigoCaja.Filtros = null;
            this.masterCodigoCaja.Location = new System.Drawing.Point(5, 15);
            this.masterCodigoCaja.Name = "masterCodigoCaja";
            this.masterCodigoCaja.Size = new System.Drawing.Size(200, 22);
            this.masterCodigoCaja.TabIndex = 1;
            this.masterCodigoCaja.Value = "";
            this.masterCodigoCaja.Enter += new System.EventHandler(this.masterCodigoCaja_Enter);
            this.masterCodigoCaja.Leave += new System.EventHandler(this.masterCodigoCaja_Leave);
            // 
            // dtFechaRec
            // 
            this.dtFechaRec.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaRec.Location = new System.Drawing.Point(736, 42);
            this.dtFechaRec.Name = "dtFechaRec";
            this.dtFechaRec.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaRec.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaRec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaRec.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaRec.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaRec.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaRec.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaRec.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaRec.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaRec.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaRec.Size = new System.Drawing.Size(120, 20);
            this.dtFechaRec.TabIndex = 6;
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(5, 89);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(200, 22);
            this.masterMoneda.TabIndex = 10;
            this.masterMoneda.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(320, 39);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(202, 22);
            this.masterProyecto.TabIndex = 5;
            this.masterProyecto.Value = "";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(320, 64);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(202, 22);
            this.masterCentroCosto.TabIndex = 8;
            this.masterCentroCosto.Value = "";
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(321, 89);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(202, 22);
            this.masterLugarGeo.TabIndex = 11;
            this.masterLugarGeo.Value = "";
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(629, 67);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(96, 14);
            this.lblTasaCambio.TabIndex = 24;
            this.lblTasaCambio.Text = "32_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0";
            this.txtTasaCambio.Enabled = false;
            this.txtTasaCambio.Location = new System.Drawing.Point(735, 66);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(120, 20);
            this.txtTasaCambio.TabIndex = 9;
            // 
            // lblDescrDoc
            // 
            this.lblDescrDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrDoc.Location = new System.Drawing.Point(870, 43);
            this.lblDescrDoc.Name = "lblDescrDoc";
            this.lblDescrDoc.Size = new System.Drawing.Size(83, 14);
            this.lblDescrDoc.TabIndex = 21;
            this.lblDescrDoc.Text = "32_lblDescrDoc";
            // 
            // lblValor
            // 
            this.lblValor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(629, 93);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(59, 14);
            this.lblValor.TabIndex = 20;
            this.lblValor.Text = "32_lblValor";
            // 
            // lblFechaRec
            // 
            this.lblFechaRec.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaRec.Location = new System.Drawing.Point(629, 43);
            this.lblFechaRec.Name = "lblFechaRec";
            this.lblFechaRec.Size = new System.Drawing.Size(84, 14);
            this.lblFechaRec.TabIndex = 22;
            this.lblFechaRec.Text = "32_lblFechaRec";
            // 
            // txtDescrDoc
            // 
            this.txtDescrDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrDoc.Location = new System.Drawing.Point(870, 59);
            this.txtDescrDoc.Multiline = true;
            this.txtDescrDoc.Name = "txtDescrDoc";
            this.txtDescrDoc.Size = new System.Drawing.Size(210, 61);
            this.txtDescrDoc.TabIndex = 13;
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0";
            this.txtValor.Location = new System.Drawing.Point(736, 90);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(120, 20);
            this.txtValor.TabIndex = 12;
            // 
            // btnFacturas
            // 
            this.btnFacturas.Enabled = false;
            this.btnFacturas.Location = new System.Drawing.Point(976, 15);
            this.btnFacturas.Name = "btnFacturas";
            this.btnFacturas.Size = new System.Drawing.Size(101, 23);
            this.btnFacturas.TabIndex = 30;
            this.btnFacturas.Text = "32_btnFacturas";
            this.btnFacturas.UseVisualStyleBackColor = true;
            this.btnFacturas.Click += new System.EventHandler(this.btnFacturas_Click);
            // 
            // masterBancoCuenta
            // 
            this.masterBancoCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterBancoCuenta.Filtros = null;
            this.masterBancoCuenta.Location = new System.Drawing.Point(635, 15);
            this.masterBancoCuenta.Name = "masterBancoCuenta";
            this.masterBancoCuenta.Size = new System.Drawing.Size(291, 22);
            this.masterBancoCuenta.TabIndex = 3;
            this.masterBancoCuenta.Value = "";
            this.masterBancoCuenta.Leave += new System.EventHandler(this.masterBancoCuenta_Leave);
            // 
            // masterTercero_
            // 
            this.masterTercero_.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero_.Filtros = null;
            this.masterTercero_.Location = new System.Drawing.Point(5, 39);
            this.masterTercero_.Name = "masterTercero_";
            this.masterTercero_.Size = new System.Drawing.Size(291, 22);
            this.masterTercero_.TabIndex = 4;
            this.masterTercero_.Value = "";
            this.masterTercero_.Enter += new System.EventHandler(this.masterTercero__Enter);
            this.masterTercero_.Leave += new System.EventHandler(this.masterTercero__Leave);
            // 
            // masterCuenta_
            // 
            this.masterCuenta_.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta_.Filtros = null;
            this.masterCuenta_.Location = new System.Drawing.Point(6, 64);
            this.masterCuenta_.Name = "masterCuenta_";
            this.masterCuenta_.Size = new System.Drawing.Size(291, 22);
            this.masterCuenta_.TabIndex = 7;
            this.masterCuenta_.Value = "";
            // 
            // lblReciboNro
            // 
            this.lblReciboNro.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReciboNro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblReciboNro.Location = new System.Drawing.Point(320, 20);
            this.lblReciboNro.Margin = new System.Windows.Forms.Padding(4);
            this.lblReciboNro.Name = "lblReciboNro";
            this.lblReciboNro.Size = new System.Drawing.Size(87, 14);
            this.lblReciboNro.TabIndex = 37;
            this.lblReciboNro.Text = "32_lblReciboNro";
            // 
            // txtReciboNro
            // 
            this.txtReciboNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReciboNro.Location = new System.Drawing.Point(420, 17);
            this.txtReciboNro.Multiline = true;
            this.txtReciboNro.Name = "txtReciboNro";
            this.txtReciboNro.Size = new System.Drawing.Size(96, 19);
            this.txtReciboNro.TabIndex = 2;
            this.txtReciboNro.Enter += new System.EventHandler(this.txtReciboNro_Enter);
            this.txtReciboNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtReciboNro_KeyPress);
            this.txtReciboNro.Leave += new System.EventHandler(this.txtReciboNro_Leave);
            // 
            // ReciboCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "ReciboCaja";
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRec.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion

        private ControlsUC.uc_MasterFind masterMoneda;
        private ControlsUC.uc_MasterFind masterCodigoCaja;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        private ControlsUC.uc_MasterFind masterLugarGeo;
        private DevExpress.XtraEditors.LabelControl lblDescrDoc;
        private System.Windows.Forms.TextBox txtDescrDoc;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl lblValor;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.LabelControl lblFechaRec;
        private DevExpress.XtraEditors.DateEdit dtFechaRec;
        private System.Windows.Forms.Button btnFacturas;
        private ControlsUC.uc_MasterFind masterBancoCuenta;
        private ControlsUC.uc_MasterFind masterTercero_;
        private ControlsUC.uc_MasterFind masterCuenta_;
        private DevExpress.XtraEditors.LabelControl lblReciboNro;
        private System.Windows.Forms.TextBox txtReciboNro;

    }
}