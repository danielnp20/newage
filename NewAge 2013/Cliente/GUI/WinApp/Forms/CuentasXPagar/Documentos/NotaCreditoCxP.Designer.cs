namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class NotaCreditoCxP
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterConceptoCXP = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFactNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtFact = new System.Windows.Forms.TextBox();
            this.lblMonedaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaFactura = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaVencimiento = new DevExpress.XtraEditors.DateEdit();
            this.cmbMonedaOrigen = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.lblDescrDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblValorBruto = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaRad = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaVto = new DevExpress.XtraEditors.LabelControl();
            this.txtDescrDoc = new System.Windows.Forms.TextBox();
            this.lblIVA = new DevExpress.XtraEditors.LabelControl();
            this.txtIVA = new DevExpress.XtraEditors.TextEdit();
            this.txtValorBruto = new DevExpress.XtraEditors.TextEdit();
            this.btnLiquida = new System.Windows.Forms.Button();
            this.btnAnticipo = new System.Windows.Forms.Button();
            this.lblAnticipos = new DevExpress.XtraEditors.LabelControl();
            this.txtAnticipos = new DevExpress.XtraEditors.TextEdit();
            this.chkProvisiones = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).BeginInit();
            this.grpboxDetail.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorBruto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnticipos.Properties)).BeginInit();
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
            this.grpboxHeader.Controls.Add(this.chkProvisiones);
            this.grpboxHeader.Controls.Add(this.lblAnticipos);
            this.grpboxHeader.Controls.Add(this.txtAnticipos);
            this.grpboxHeader.Controls.Add(this.btnAnticipo);
            this.grpboxHeader.Controls.Add(this.btnLiquida);
            this.grpboxHeader.Controls.Add(this.masterLugarGeo);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.masterCentroCosto);
            this.grpboxHeader.Controls.Add(this.lblIVA);
            this.grpboxHeader.Controls.Add(this.txtIVA);
            this.grpboxHeader.Controls.Add(this.masterConceptoCXP);
            this.grpboxHeader.Controls.Add(this.masterCuenta_);
            this.grpboxHeader.Controls.Add(this.lblMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.cmbMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.masterTercero_);
            this.grpboxHeader.Controls.Add(this.lblFactNumber);
            this.grpboxHeader.Controls.Add(this.txtFact);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.lblDescrDoc);
            this.grpboxHeader.Controls.Add(this.lblFechaRad);
            this.grpboxHeader.Controls.Add(this.lblFechaVto);
            this.grpboxHeader.Controls.Add(this.txtDescrDoc);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            this.grpboxHeader.Controls.Add(this.dtFechaFactura);
            this.grpboxHeader.Controls.Add(this.dtFechaVencimiento);
            this.grpboxHeader.Controls.Add(this.lblValorBruto);
            this.grpboxHeader.Controls.Add(this.txtValorBruto);
            // 
            // masterConceptoCXP
            // 
            this.masterConceptoCXP.BackColor = System.Drawing.Color.Transparent;
            this.masterConceptoCXP.Filtros = null;
            this.masterConceptoCXP.Location = new System.Drawing.Point(10, 88);
            this.masterConceptoCXP.Name = "masterConceptoCXP";
            this.masterConceptoCXP.Size = new System.Drawing.Size(200, 24);
            this.masterConceptoCXP.TabIndex = 15;
            this.masterConceptoCXP.Value = "";
            this.masterConceptoCXP.Leave += new System.EventHandler(this.masterConceptoCXP_Leave);
            // 
            // lblFactNumber
            // 
            this.lblFactNumber.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFactNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFactNumber.Location = new System.Drawing.Point(320, 20);
            this.lblFactNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblFactNumber.Name = "lblFactNumber";
            this.lblFactNumber.Size = new System.Drawing.Size(98, 14);
            this.lblFactNumber.TabIndex = 19;
            this.lblFactNumber.Text = "26_lblFactNumber";
            // 
            // txtFact
            // 
            this.txtFact.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFact.Location = new System.Drawing.Point(420, 17);
            this.txtFact.Multiline = true;
            this.txtFact.Name = "txtFact";
            this.txtFact.Size = new System.Drawing.Size(94, 19);
            this.txtFact.TabIndex = 9;
            this.txtFact.Enter += new System.EventHandler(this.txtFact_Enter);
            this.txtFact.Leave += new System.EventHandler(this.txtFact_Leave);
            // 
            // lblMonedaOrigen
            // 
            this.lblMonedaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblMonedaOrigen.Location = new System.Drawing.Point(10, 43);
            this.lblMonedaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblMonedaOrigen.Name = "lblMonedaOrigen";
            this.lblMonedaOrigen.Size = new System.Drawing.Size(111, 14);
            this.lblMonedaOrigen.TabIndex = 17;
            this.lblMonedaOrigen.Text = "21_lblMonedaOrigen";
            // 
            // dtFechaFact
            // 
            this.dtFechaFactura.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFactura.Location = new System.Drawing.Point(743, 18);
            this.dtFechaFactura.Name = "dtFechaFact";
            this.dtFechaFactura.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFactura.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFactura.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFactura.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFactura.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFactura.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFactura.Size = new System.Drawing.Size(120, 20);
            this.dtFechaFactura.TabIndex = 4;
            // 
            // dtFechaVto
            // 
            this.dtFechaVencimiento.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaVencimiento.Enabled = false;
            this.dtFechaVencimiento.Location = new System.Drawing.Point(743, 41);
            this.dtFechaVencimiento.Name = "dtFechaVto";
            this.dtFechaVencimiento.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaVencimiento.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaVencimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaVencimiento.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVencimiento.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVencimiento.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaVencimiento.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaVencimiento.Size = new System.Drawing.Size(120, 20);
            this.dtFechaVencimiento.TabIndex = 26;
            // 
            // cmbMonedaOrigen
            // 
            this.cmbMonedaOrigen.BackColor = System.Drawing.Color.White;
            this.cmbMonedaOrigen.Enabled = false;
            this.cmbMonedaOrigen.FormattingEnabled = true;
            this.cmbMonedaOrigen.Location = new System.Drawing.Point(111, 41);
            this.cmbMonedaOrigen.Name = "cmbMonedaOrigen";
            this.cmbMonedaOrigen.Size = new System.Drawing.Size(192, 21);
            this.cmbMonedaOrigen.TabIndex = 18;
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(10, 64);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(200, 22);
            this.masterMoneda.TabIndex = 20;
            this.masterMoneda.Value = "";
            // 
            // masterCuenta_
            // 
            this.masterCuenta_.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta_.Filtros = null;
            this.masterCuenta_.Location = new System.Drawing.Point(10, 111);
            this.masterCuenta_.Name = "masterCuenta_";
            this.masterCuenta_.Size = new System.Drawing.Size(200, 22);
            this.masterCuenta_.TabIndex = 16;
            this.masterCuenta_.Value = "";
            // 
            // masterTercero_
            // 
            this.masterTercero_.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero_.Filtros = null;
            this.masterTercero_.Location = new System.Drawing.Point(10, 15);
            this.masterTercero_.Name = "masterTercero_";
            this.masterTercero_.Size = new System.Drawing.Size(200, 22);
            this.masterTercero_.TabIndex = 8;
            this.masterTercero_.Value = "";
            this.masterTercero_.Enter += new System.EventHandler(this.masterTercero_Enter);
            this.masterTercero_.Leave += new System.EventHandler(this.masterTercero_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(320, 39);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(202, 22);
            this.masterProyecto.TabIndex = 10;
            this.masterProyecto.Value = "";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(320, 64);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(202, 22);
            this.masterCentroCosto.TabIndex = 11;
            this.masterCentroCosto.Value = "";
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(320, 88);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(202, 22);
            this.masterLugarGeo.TabIndex = 12;
            this.masterLugarGeo.Value = "";
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(636, 67);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(96, 14);
            this.lblTasaCambio.TabIndex = 24;
            this.lblTasaCambio.Text = "21_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0";
            this.txtTasaCambio.Enabled = false;
            this.txtTasaCambio.Location = new System.Drawing.Point(743, 66);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtTasaCambio.Properties.Appearance.Options.UseFont = true;
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(120, 20);
            this.txtTasaCambio.TabIndex = 25;
            // 
            // lblDescrDoc
            // 
            this.lblDescrDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrDoc.Location = new System.Drawing.Point(870, 17);
            this.lblDescrDoc.Name = "lblDescrDoc";
            this.lblDescrDoc.Size = new System.Drawing.Size(83, 14);
            this.lblDescrDoc.TabIndex = 21;
            this.lblDescrDoc.Text = "21_lblDescrDoc";
            // 
            // lblValorBruto
            // 
            this.lblValorBruto.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorBruto.Location = new System.Drawing.Point(636, 92);
            this.lblValorBruto.Name = "lblValorBruto";
            this.lblValorBruto.Size = new System.Drawing.Size(89, 14);
            this.lblValorBruto.TabIndex = 27;
            this.lblValorBruto.Text = "21_lblValorBruto";
            // 
            // lblFechaRad
            // 
            this.lblFechaRad.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaRad.Location = new System.Drawing.Point(636, 19);
            this.lblFechaRad.Name = "lblFechaRad";
            this.lblFechaRad.Size = new System.Drawing.Size(87, 14);
            this.lblFechaRad.TabIndex = 22;
            this.lblFechaRad.Text = "21_lblFechaFact";
            // 
            // lblFechaVto
            // 
            this.lblFechaVto.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVto.Location = new System.Drawing.Point(636, 43);
            this.lblFechaVto.Name = "lblFechaVto";
            this.lblFechaVto.Size = new System.Drawing.Size(84, 14);
            this.lblFechaVto.TabIndex = 23;
            this.lblFechaVto.Text = "21_lblFechaVto";
            // 
            // txtDescrDoc
            // 
            this.txtDescrDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrDoc.Location = new System.Drawing.Point(870, 34);
            this.txtDescrDoc.Multiline = true;
            this.txtDescrDoc.Name = "txtDescrDoc";
            this.txtDescrDoc.Size = new System.Drawing.Size(210, 72);
            this.txtDescrDoc.TabIndex = 13;
            // 
            // lblIVA
            // 
            this.lblIVA.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIVA.Location = new System.Drawing.Point(636, 114);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(52, 14);
            this.lblIVA.TabIndex = 13;
            this.lblIVA.Text = "21_lblIVA";
            // 
            // txtIVA
            // 
            this.txtIVA.EditValue = "0";
            this.txtIVA.Location = new System.Drawing.Point(743, 112);
            this.txtIVA.Name = "txtIVA";
            this.txtIVA.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIVA.Properties.Appearance.Options.UseFont = true;
            this.txtIVA.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIVA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIVA.Properties.Mask.EditMask = "c";
            this.txtIVA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIVA.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIVA.Size = new System.Drawing.Size(120, 20);
            this.txtIVA.TabIndex = 14;
            // 
            // txtValorBruto
            // 
            this.txtValorBruto.EditValue = "0";
            this.txtValorBruto.Location = new System.Drawing.Point(743, 89);
            this.txtValorBruto.Name = "txtValorBruto";
            this.txtValorBruto.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorBruto.Properties.Appearance.Options.UseFont = true;
            this.txtValorBruto.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorBruto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorBruto.Properties.Mask.EditMask = "c";
            this.txtValorBruto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorBruto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorBruto.Size = new System.Drawing.Size(120, 20);
            this.txtValorBruto.TabIndex = 28;
            // 
            // btnLiquida
            // 
            this.btnLiquida.Enabled = false;
            this.btnLiquida.Location = new System.Drawing.Point(979, 110);
            this.btnLiquida.Name = "btnLiquida";
            this.btnLiquida.Size = new System.Drawing.Size(101, 23);
            this.btnLiquida.TabIndex = 29;
            this.btnLiquida.Text = "21_btnLiquida";
            this.btnLiquida.UseVisualStyleBackColor = true;
            this.btnLiquida.Click += new System.EventHandler(this.btnLiquida_Click);
            // 
            // btnAnticipo
            // 
            this.btnAnticipo.Enabled = false;
            this.btnAnticipo.Location = new System.Drawing.Point(872, 110);
            this.btnAnticipo.Name = "btnAnticipo";
            this.btnAnticipo.Size = new System.Drawing.Size(101, 23);
            this.btnAnticipo.TabIndex = 30;
            this.btnAnticipo.Text = "21_btnAnticipo";
            this.btnAnticipo.UseVisualStyleBackColor = true;
            this.btnAnticipo.Click += new System.EventHandler(this.btnAnticipo_Click);
            // 
            // lblAnticipos
            // 
            this.lblAnticipos.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnticipos.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblAnticipos.Location = new System.Drawing.Point(321, 116);
            this.lblAnticipos.Margin = new System.Windows.Forms.Padding(4);
            this.lblAnticipos.Name = "lblAnticipos";
            this.lblAnticipos.Size = new System.Drawing.Size(81, 14);
            this.lblAnticipos.TabIndex = 31;
            this.lblAnticipos.Text = "21_lblAnticipos";
            // 
            // txtAnticipos
            // 
            this.txtAnticipos.Enabled = false;
            this.txtAnticipos.Location = new System.Drawing.Point(420, 115);
            this.txtAnticipos.Name = "txtAnticipos";
            this.txtAnticipos.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAnticipos.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtAnticipos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAnticipos.Properties.Mask.EditMask = "c";
            this.txtAnticipos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAnticipos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtAnticipos.Size = new System.Drawing.Size(120, 20);
            this.txtAnticipos.TabIndex = 32;
            // 
            // chkProvisiones
            // 
            this.chkProvisiones.AutoSize = true;
            this.chkProvisiones.Location = new System.Drawing.Point(517, 19);
            this.chkProvisiones.Name = "chkProvisiones";
            this.chkProvisiones.Size = new System.Drawing.Size(116, 17);
            this.chkProvisiones.TabIndex = 33;
            this.chkProvisiones.Text = "21_chkProvisiones";
            this.chkProvisiones.UseVisualStyleBackColor = true;
            this.chkProvisiones.CheckedChanged += new System.EventHandler(this.chkProvisiones_CheckedChanged);
            // 
            // NotaCreditoCxP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "NotaCreditoCxP";
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).EndInit();
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorBruto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnticipos.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion
        
        private ControlsUC.uc_MasterFind masterTercero_;
        private DevExpress.XtraEditors.LabelControl lblFactNumber;
        private System.Windows.Forms.TextBox txtFact;
        private ControlsUC.uc_MasterFind masterMoneda;
        private ControlsUC.uc_MasterFind masterConceptoCXP;
        private ControlsUC.uc_MasterFind masterCuenta_;      
        private DevExpress.XtraEditors.LabelControl lblIVA;
        private DevExpress.XtraEditors.TextEdit txtIVA;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        private ControlsUC.uc_MasterFind masterLugarGeo;
        private DevExpress.XtraEditors.LabelControl lblDescrDoc;
        private System.Windows.Forms.TextBox txtDescrDoc;
        private DevExpress.XtraEditors.LabelControl lblMonedaOrigen;
        private NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx cmbMonedaOrigen;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl lblValorBruto;
        private DevExpress.XtraEditors.TextEdit txtValorBruto;
        private DevExpress.XtraEditors.LabelControl lblFechaRad;
        private DevExpress.XtraEditors.LabelControl lblFechaVto;
        private DevExpress.XtraEditors.DateEdit dtFechaFactura;
        private DevExpress.XtraEditors.DateEdit dtFechaVencimiento;
        private System.Windows.Forms.Button btnLiquida;
        private System.Windows.Forms.Button btnAnticipo;
        private DevExpress.XtraEditors.LabelControl lblAnticipos;
        private DevExpress.XtraEditors.TextEdit txtAnticipos;
        private System.Windows.Forms.CheckBox chkProvisiones;

    }
}