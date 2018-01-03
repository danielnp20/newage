namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CausacionFacturas
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
            this.masterCuentaHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyectoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCtoCostoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeoHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
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
            this.btnResumenImp = new System.Windows.Forms.Button();
            this.chkProvisiones = new System.Windows.Forms.CheckBox();
            this.lblValorTercero = new DevExpress.XtraEditors.LabelControl();
            this.txtValorTercero = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorML.Properties)).BeginInit();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorBruto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnticipos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTercero.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalLocal
            // 
            this.txtTotalLocal.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTotalLocal.Properties.Appearance.Options.UseFont = true;
            this.txtTotalLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.Mask.EditMask = "c";
            this.txtTotalLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalLocal.Size = new System.Drawing.Size(120, 22);
            // 
            // txtTotalForeign
            // 
            this.txtTotalForeign.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtTotalForeign.Properties.Appearance.Options.UseFont = true;
            this.txtTotalForeign.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalForeign.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalForeign.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.Mask.EditMask = "c";
            this.txtTotalForeign.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalForeign.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalForeign.Size = new System.Drawing.Size(120, 22);
            // 
            // txtBaseML
            // 
            this.txtBaseML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtBaseML.Properties.Appearance.Options.UseFont = true;
            this.txtBaseML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBaseML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBaseML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBaseML.Properties.Mask.EditMask = "c";
            this.txtBaseML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtBaseML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtBaseML.Size = new System.Drawing.Size(123, 22);
            // 
            // txtValorME
            // 
            this.txtValorME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorME.Properties.Appearance.Options.UseFont = true;
            this.txtValorME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorME.Properties.Mask.EditMask = "c";
            this.txtValorME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorME.Size = new System.Drawing.Size(123, 22);
            // 
            // txtBaseME
            // 
            this.txtBaseME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtBaseME.Properties.Appearance.Options.UseFont = true;
            this.txtBaseME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBaseME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBaseME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBaseME.Properties.Mask.EditMask = "c";
            this.txtBaseME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtBaseME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtBaseME.Size = new System.Drawing.Size(123, 22);
            // 
            // txtValorML
            // 
            this.txtValorML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorML.Properties.Appearance.Options.UseFont = true;
            this.txtValorML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorML.Properties.Mask.EditMask = "c2";
            this.txtValorML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorML.Size = new System.Drawing.Size(123, 22);
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
            this.editSpin7,
            this.editDate,
            this.editValue,
            this.editValue4,
            this.editLink,
            this.editSpinPorcen,
            this.editSpin0});
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
            // editSpin7
            // 
            this.editSpin7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.Mask.EditMask = "c7";
            this.editSpin7.Mask.UseMaskAsDisplayFormat = true;
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
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 272);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Size = new System.Drawing.Size(296, 272);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblValorTercero);
            this.grpboxHeader.Controls.Add(this.chkProvisiones);
            this.grpboxHeader.Controls.Add(this.txtValorTercero);
            this.grpboxHeader.Controls.Add(this.btnResumenImp);
            this.grpboxHeader.Controls.Add(this.lblAnticipos);
            this.grpboxHeader.Controls.Add(this.txtAnticipos);
            this.grpboxHeader.Controls.Add(this.btnAnticipo);
            this.grpboxHeader.Controls.Add(this.btnLiquida);
            this.grpboxHeader.Controls.Add(this.masterLugarGeoHeader);
            this.grpboxHeader.Controls.Add(this.masterProyectoHeader);
            this.grpboxHeader.Controls.Add(this.masterCtoCostoHeader);
            this.grpboxHeader.Controls.Add(this.lblIVA);
            this.grpboxHeader.Controls.Add(this.txtIVA);
            this.grpboxHeader.Controls.Add(this.masterConceptoCXP);
            this.grpboxHeader.Controls.Add(this.masterCuentaHeader);
            this.grpboxHeader.Controls.Add(this.lblMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.cmbMonedaOrigen);
            this.grpboxHeader.Controls.Add(this.masterTerceroHeader);
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
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterConceptoCXP
            // 
            this.masterConceptoCXP.BackColor = System.Drawing.Color.Transparent;
            this.masterConceptoCXP.Filtros = null;
            this.masterConceptoCXP.Location = new System.Drawing.Point(6, 83);
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
            this.lblFactNumber.Location = new System.Drawing.Point(313, 15);
            this.lblFactNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblFactNumber.Name = "lblFactNumber";
            this.lblFactNumber.Size = new System.Drawing.Size(98, 14);
            this.lblFactNumber.TabIndex = 19;
            this.lblFactNumber.Text = "21_lblFactNumber";
            // 
            // txtFact
            // 
            this.txtFact.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFact.Location = new System.Drawing.Point(413, 12);
            this.txtFact.Multiline = true;
            this.txtFact.Name = "txtFact";
            this.txtFact.Size = new System.Drawing.Size(89, 19);
            this.txtFact.TabIndex = 9;
            this.txtFact.Enter += new System.EventHandler(this.txtFact_Enter);
            this.txtFact.Leave += new System.EventHandler(this.txtFact_Leave);
            // 
            // lblMonedaOrigen
            // 
            this.lblMonedaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblMonedaOrigen.Location = new System.Drawing.Point(6, 38);
            this.lblMonedaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblMonedaOrigen.Name = "lblMonedaOrigen";
            this.lblMonedaOrigen.Size = new System.Drawing.Size(111, 14);
            this.lblMonedaOrigen.TabIndex = 17;
            this.lblMonedaOrigen.Text = "21_lblMonedaOrigen";
            // 
            // dtFechaFactura
            // 
            this.dtFechaFactura.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFactura.Location = new System.Drawing.Point(738, 13);
            this.dtFechaFactura.Name = "dtFechaFactura";
            this.dtFechaFactura.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFactura.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFactura.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFactura.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFactura.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFactura.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFactura.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFactura.Size = new System.Drawing.Size(120, 20);
            this.dtFechaFactura.TabIndex = 4;
            // 
            // dtFechaVencimiento
            // 
            this.dtFechaVencimiento.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaVencimiento.Enabled = false;
            this.dtFechaVencimiento.Location = new System.Drawing.Point(968, 15);
            this.dtFechaVencimiento.Name = "dtFechaVencimiento";
            this.dtFechaVencimiento.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaVencimiento.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaVencimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaVencimiento.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaVencimiento.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVencimiento.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVencimiento.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaVencimiento.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaVencimiento.Size = new System.Drawing.Size(83, 20);
            this.dtFechaVencimiento.TabIndex = 26;
            // 
            // cmbMonedaOrigen
            // 
            this.cmbMonedaOrigen.BackColor = System.Drawing.Color.White;
            this.cmbMonedaOrigen.Enabled = false;
            this.cmbMonedaOrigen.FormattingEnabled = true;
            this.cmbMonedaOrigen.Location = new System.Drawing.Point(107, 36);
            this.cmbMonedaOrigen.Name = "cmbMonedaOrigen";
            this.cmbMonedaOrigen.Size = new System.Drawing.Size(192, 21);
            this.cmbMonedaOrigen.TabIndex = 18;
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(6, 59);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(200, 22);
            this.masterMoneda.TabIndex = 20;
            this.masterMoneda.Value = "";
            // 
            // masterCuentaHeader
            // 
            this.masterCuentaHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterCuentaHeader.Filtros = null;
            this.masterCuentaHeader.Location = new System.Drawing.Point(6, 106);
            this.masterCuentaHeader.Name = "masterCuentaHeader";
            this.masterCuentaHeader.Size = new System.Drawing.Size(200, 22);
            this.masterCuentaHeader.TabIndex = 16;
            this.masterCuentaHeader.Value = "";
            // 
            // masterTerceroHeader
            // 
            this.masterTerceroHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroHeader.Filtros = null;
            this.masterTerceroHeader.Location = new System.Drawing.Point(6, 10);
            this.masterTerceroHeader.Name = "masterTerceroHeader";
            this.masterTerceroHeader.Size = new System.Drawing.Size(200, 22);
            this.masterTerceroHeader.TabIndex = 8;
            this.masterTerceroHeader.Value = "";
            this.masterTerceroHeader.Enter += new System.EventHandler(this.masterTercero_Enter);
            this.masterTerceroHeader.Leave += new System.EventHandler(this.masterTercero_Leave);
            // 
            // masterProyectoHeader
            // 
            this.masterProyectoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoHeader.Filtros = null;
            this.masterProyectoHeader.Location = new System.Drawing.Point(313, 34);
            this.masterProyectoHeader.Name = "masterProyectoHeader";
            this.masterProyectoHeader.Size = new System.Drawing.Size(202, 22);
            this.masterProyectoHeader.TabIndex = 10;
            this.masterProyectoHeader.Value = "";
            // 
            // masterCtoCostoHeader
            // 
            this.masterCtoCostoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterCtoCostoHeader.Filtros = null;
            this.masterCtoCostoHeader.Location = new System.Drawing.Point(313, 59);
            this.masterCtoCostoHeader.Name = "masterCtoCostoHeader";
            this.masterCtoCostoHeader.Size = new System.Drawing.Size(202, 22);
            this.masterCtoCostoHeader.TabIndex = 11;
            this.masterCtoCostoHeader.Value = "";
            // 
            // masterLugarGeoHeader
            // 
            this.masterLugarGeoHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeoHeader.Filtros = null;
            this.masterLugarGeoHeader.Location = new System.Drawing.Point(313, 83);
            this.masterLugarGeoHeader.Name = "masterLugarGeoHeader";
            this.masterLugarGeoHeader.Size = new System.Drawing.Size(202, 22);
            this.masterLugarGeoHeader.TabIndex = 12;
            this.masterLugarGeoHeader.Value = "";
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(631, 37);
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
            this.txtTasaCambio.Location = new System.Drawing.Point(738, 36);
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
            this.lblDescrDoc.Location = new System.Drawing.Point(865, 40);
            this.lblDescrDoc.Name = "lblDescrDoc";
            this.lblDescrDoc.Size = new System.Drawing.Size(83, 14);
            this.lblDescrDoc.TabIndex = 21;
            this.lblDescrDoc.Text = "21_lblDescrDoc";
            // 
            // lblValorBruto
            // 
            this.lblValorBruto.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorBruto.Location = new System.Drawing.Point(631, 88);
            this.lblValorBruto.Name = "lblValorBruto";
            this.lblValorBruto.Size = new System.Drawing.Size(89, 14);
            this.lblValorBruto.TabIndex = 27;
            this.lblValorBruto.Text = "21_lblValorBruto";
            // 
            // lblFechaRad
            // 
            this.lblFechaRad.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaRad.Location = new System.Drawing.Point(631, 14);
            this.lblFechaRad.Name = "lblFechaRad";
            this.lblFechaRad.Size = new System.Drawing.Size(87, 14);
            this.lblFechaRad.TabIndex = 22;
            this.lblFechaRad.Text = "21_lblFechaFact";
            // 
            // lblFechaVto
            // 
            this.lblFechaVto.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVto.Location = new System.Drawing.Point(864, 16);
            this.lblFechaVto.Name = "lblFechaVto";
            this.lblFechaVto.Size = new System.Drawing.Size(84, 14);
            this.lblFechaVto.TabIndex = 23;
            this.lblFechaVto.Text = "21_lblFechaVto";
            // 
            // txtDescrDoc
            // 
            this.txtDescrDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrDoc.Location = new System.Drawing.Point(864, 58);
            this.txtDescrDoc.Multiline = true;
            this.txtDescrDoc.Name = "txtDescrDoc";
            this.txtDescrDoc.Size = new System.Drawing.Size(241, 47);
            this.txtDescrDoc.TabIndex = 13;
            this.txtDescrDoc.TextChanged += new System.EventHandler(this.txtDescrDoc_TextChanged);
            // 
            // lblIVA
            // 
            this.lblIVA.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIVA.Location = new System.Drawing.Point(631, 113);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(52, 14);
            this.lblIVA.TabIndex = 13;
            this.lblIVA.Text = "21_lblIVA";
            // 
            // txtIVA
            // 
            this.txtIVA.EditValue = "0";
            this.txtIVA.Location = new System.Drawing.Point(738, 110);
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
            this.txtValorBruto.Location = new System.Drawing.Point(738, 85);
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
            this.txtValorBruto.Leave += new System.EventHandler(this.txtValorBruto_Leave);

            // 
            // btnLiquida
            // 
            this.btnLiquida.Enabled = false;
            this.btnLiquida.Location = new System.Drawing.Point(938, 108);
            this.btnLiquida.Name = "btnLiquida";
            this.btnLiquida.Size = new System.Drawing.Size(78, 23);
            this.btnLiquida.TabIndex = 29;
            this.btnLiquida.Text = "21_btnLiquida";
            this.btnLiquida.UseVisualStyleBackColor = true;
            this.btnLiquida.Click += new System.EventHandler(this.btnLiquida_Click);
            // 
            // btnAnticipo
            // 
            this.btnAnticipo.Enabled = false;
            this.btnAnticipo.Location = new System.Drawing.Point(864, 108);
            this.btnAnticipo.Name = "btnAnticipo";
            this.btnAnticipo.Size = new System.Drawing.Size(73, 23);
            this.btnAnticipo.TabIndex = 30;
            this.btnAnticipo.Text = "21_btnAnticipo";
            this.btnAnticipo.UseVisualStyleBackColor = true;
            this.btnAnticipo.Click += new System.EventHandler(this.btnAnticipo_Click);
            // 
            // lblAnticipos
            // 
            this.lblAnticipos.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnticipos.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblAnticipos.Location = new System.Drawing.Point(314, 111);
            this.lblAnticipos.Margin = new System.Windows.Forms.Padding(4);
            this.lblAnticipos.Name = "lblAnticipos";
            this.lblAnticipos.Size = new System.Drawing.Size(81, 14);
            this.lblAnticipos.TabIndex = 31;
            this.lblAnticipos.Text = "21_lblAnticipos";
            // 
            // txtAnticipos
            // 
            this.txtAnticipos.Enabled = false;
            this.txtAnticipos.Location = new System.Drawing.Point(413, 110);
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
            // btnResumenImp
            // 
            this.btnResumenImp.Enabled = false;
            this.btnResumenImp.Location = new System.Drawing.Point(1016, 108);
            this.btnResumenImp.Name = "btnResumenImp";
            this.btnResumenImp.Size = new System.Drawing.Size(91, 23);
            this.btnResumenImp.TabIndex = 33;
            this.btnResumenImp.Text = "21_btnResumenImp";
            this.btnResumenImp.UseVisualStyleBackColor = true;
            this.btnResumenImp.Click += new System.EventHandler(this.btnResumenImp_Click);
            // 
            // chkProvisiones
            // 
            this.chkProvisiones.AutoSize = true;
            this.chkProvisiones.Location = new System.Drawing.Point(507, 14);
            this.chkProvisiones.Name = "chkProvisiones";
            this.chkProvisiones.Size = new System.Drawing.Size(114, 17);
            this.chkProvisiones.TabIndex = 34;
            this.chkProvisiones.Text = "21_chkProvisiones";
            this.chkProvisiones.UseVisualStyleBackColor = true;
            this.chkProvisiones.CheckedChanged += new System.EventHandler(this.chkProvisiones_CheckedChanged);
            // 
            // labelControl4
            // 
            this.lblValorTercero.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTercero.Location = new System.Drawing.Point(631, 63);
            this.lblValorTercero.Name = "lblValorTercero";
            this.lblValorTercero.Size = new System.Drawing.Size(89, 14);
            this.lblValorTercero.TabIndex = 51;
            this.lblValorTercero.Text = "Valor Tercero";
            // 
            // textEdit1
            // 
            this.txtValorTercero.EditValue = "0";
            this.txtValorTercero.Location = new System.Drawing.Point(738, 60);
            this.txtValorTercero.Name = "textEdit1";
            this.txtValorTercero.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorTercero.Properties.Appearance.Options.UseFont = true;
            this.txtValorTercero.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTercero.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTercero.Properties.Mask.EditMask = "c";
            this.txtValorTercero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTercero.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTercero.Size = new System.Drawing.Size(120, 20);
            this.txtValorTercero.TabIndex = 52;
            // 
            // CausacionFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "CausacionFacturas";
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBaseME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorML.Properties)).EndInit();
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorBruto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnticipos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTercero.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion
        
        private ControlsUC.uc_MasterFind masterTerceroHeader;
        private DevExpress.XtraEditors.LabelControl lblFactNumber;
        private System.Windows.Forms.TextBox txtFact;
        private ControlsUC.uc_MasterFind masterMoneda;
        private ControlsUC.uc_MasterFind masterConceptoCXP;
        private ControlsUC.uc_MasterFind masterCuentaHeader;      
        private DevExpress.XtraEditors.LabelControl lblIVA;
        private DevExpress.XtraEditors.TextEdit txtIVA;
        private ControlsUC.uc_MasterFind masterProyectoHeader;
        private ControlsUC.uc_MasterFind masterCtoCostoHeader;
        private ControlsUC.uc_MasterFind masterLugarGeoHeader;
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
        private System.Windows.Forms.Button btnResumenImp;
        private System.Windows.Forms.CheckBox chkProvisiones;
        private DevExpress.XtraEditors.LabelControl lblValorTercero;
        private DevExpress.XtraEditors.TextEdit txtValorTercero;
    }
}