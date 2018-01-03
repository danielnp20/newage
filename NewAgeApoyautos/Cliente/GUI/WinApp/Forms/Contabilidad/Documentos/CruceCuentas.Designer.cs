namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CruceCuentas
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblDocumentoNroInt = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoNroInt = new System.Windows.Forms.TextBox();
            this.dtFechaDoc = new DevExpress.XtraEditors.DateEdit();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaDoc = new DevExpress.XtraEditors.LabelControl();
            this.masterPrefijo_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocumentoNroExt = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoNroExt = new System.Windows.Forms.TextBox();
            this.masterDocumento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta_ = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.rbtTercero = new System.Windows.Forms.RadioButton();
            this.rbtPrefijo = new System.Windows.Forms.RadioButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblDescripcionHeader = new DevExpress.XtraEditors.LabelControl();
            this.txtDescripcionHeader = new System.Windows.Forms.TextBox();
            this.txtSaldo = new DevExpress.XtraEditors.TextEdit();
            this.lblSaldo = new DevExpress.XtraEditors.LabelControl();
            this.lblSaldoAjustado = new DevExpress.XtraEditors.LabelControl();
            this.txtSaldoAjustado = new DevExpress.XtraEditors.TextEdit();
            this.lblAjuste = new DevExpress.XtraEditors.LabelControl();
            this.txtAjuste = new DevExpress.XtraEditors.TextEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoAjustado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAjuste.Properties)).BeginInit();
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
            this.grpboxHeader.Controls.Add(this.lblAjuste);
            this.grpboxHeader.Controls.Add(this.txtAjuste);
            this.grpboxHeader.Controls.Add(this.lblSaldoAjustado);
            this.grpboxHeader.Controls.Add(this.txtSaldoAjustado);
            this.grpboxHeader.Controls.Add(this.txtDescripcionHeader);
            this.grpboxHeader.Controls.Add(this.lblSaldo);
            this.grpboxHeader.Controls.Add(this.masterLugarGeo);
            this.grpboxHeader.Controls.Add(this.masterCentroCosto);
            this.grpboxHeader.Controls.Add(this.lblDescripcionHeader);
            this.grpboxHeader.Controls.Add(this.panelControl1);
            this.grpboxHeader.Controls.Add(this.masterCuenta_);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.lblFechaDoc);
            this.grpboxHeader.Controls.Add(this.dtFechaDoc);
            this.grpboxHeader.Controls.Add(this.txtSaldo);
            this.grpboxHeader.Size = new System.Drawing.Size(1072, 127);
            // 
            // lblDocumentoNroInt
            // 
            this.lblDocumentoNroInt.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoNroInt.Location = new System.Drawing.Point(738, 27);
            this.lblDocumentoNroInt.Margin = new System.Windows.Forms.Padding(4);
            this.lblDocumentoNroInt.Name = "lblDocumentoNroInt";
            this.lblDocumentoNroInt.Size = new System.Drawing.Size(131, 14);
            this.lblDocumentoNroInt.TabIndex = 19;
            this.lblDocumentoNroInt.Text = "18_lblDocumentoNroInt";
            // 
            // txtDocumentoNroInt
            // 
            this.txtDocumentoNroInt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoNroInt.Location = new System.Drawing.Point(861, 25);
            this.txtDocumentoNroInt.Multiline = true;
            this.txtDocumentoNroInt.Name = "txtDocumentoNroInt";
            this.txtDocumentoNroInt.Size = new System.Drawing.Size(48, 19);
            this.txtDocumentoNroInt.TabIndex = 14;
            this.txtDocumentoNroInt.Leave += new System.EventHandler(this.txtDocumentoNroInt_Leave);
            // 
            // dtFechaDoc
            // 
            this.dtFechaDoc.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaDoc.Enabled = false;
            this.dtFechaDoc.Location = new System.Drawing.Point(113, 65);
            this.dtFechaDoc.Name = "dtFechaDoc";
            this.dtFechaDoc.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDoc.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDoc.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDoc.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDoc.Size = new System.Drawing.Size(120, 20);
            this.dtFechaDoc.TabIndex = 26;
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(13, 86);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(291, 22);
            this.masterMoneda.TabIndex = 20;
            this.masterMoneda.Value = "";
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(315, 36);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(180, 22);
            this.masterCuenta.TabIndex = 16;
            this.masterCuenta.Value = "";
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(10, 39);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(180, 22);
            this.masterTercero.TabIndex = 8;
            this.masterTercero.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(317, 63);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(305, 22);
            this.masterProyecto.TabIndex = 10;
            this.masterProyecto.Value = "";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(317, 86);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(305, 22);
            this.masterCentroCosto.TabIndex = 11;
            this.masterCentroCosto.Value = "";
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(317, 109);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(305, 22);
            this.masterLugarGeo.TabIndex = 12;
            this.masterLugarGeo.Value = "";
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaDoc.Location = new System.Drawing.Point(13, 67);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(85, 14);
            this.lblFechaDoc.TabIndex = 23;
            this.lblFechaDoc.Text = "18_lblFechaDoc";
            // 
            // masterPrefijo_
            // 
            this.masterPrefijo_.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo_.Filtros = null;
            this.masterPrefijo_.Location = new System.Drawing.Point(414, 22);
            this.masterPrefijo_.Name = "masterPrefijo_";
            this.masterPrefijo_.Size = new System.Drawing.Size(291, 25);
            this.masterPrefijo_.TabIndex = 13;
            this.masterPrefijo_.Value = "";
            // 
            // lblDocumentoNroExt
            // 
            this.lblDocumentoNroExt.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoNroExt.Location = new System.Drawing.Point(738, 5);
            this.lblDocumentoNroExt.Margin = new System.Windows.Forms.Padding(4);
            this.lblDocumentoNroExt.Name = "lblDocumentoNroExt";
            this.lblDocumentoNroExt.Size = new System.Drawing.Size(133, 14);
            this.lblDocumentoNroExt.TabIndex = 37;
            this.lblDocumentoNroExt.Text = "18_lblDocumentoNroExt";
            // 
            // txtDocumentoNroExt
            // 
            this.txtDocumentoNroExt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoNroExt.Location = new System.Drawing.Point(861, 2);
            this.txtDocumentoNroExt.Multiline = true;
            this.txtDocumentoNroExt.Name = "txtDocumentoNroExt";
            this.txtDocumentoNroExt.Size = new System.Drawing.Size(48, 19);
            this.txtDocumentoNroExt.TabIndex = 12;
            this.txtDocumentoNroExt.Leave += new System.EventHandler(this.txtDocumentoNroExt_Leave);
            // 
            // masterDocumento
            // 
            this.masterDocumento.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumento.Filtros = null;
            this.masterDocumento.Location = new System.Drawing.Point(15, 0);
            this.masterDocumento.Name = "masterDocumento";
            this.masterDocumento.Size = new System.Drawing.Size(291, 25);
            this.masterDocumento.TabIndex = 8;
            this.masterDocumento.Value = "";
            // 
            // masterTercero_
            // 
            this.masterTercero_.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero_.Filtros = null;
            this.masterTercero_.Location = new System.Drawing.Point(414, 0);
            this.masterTercero_.Name = "masterTercero_";
            this.masterTercero_.Size = new System.Drawing.Size(291, 25);
            this.masterTercero_.TabIndex = 11;
            this.masterTercero_.Value = "";
            // 
            // masterCuenta_
            // 
            this.masterCuenta_.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta_.Filtros = null;
            this.masterCuenta_.Location = new System.Drawing.Point(13, 109);
            this.masterCuenta_.Name = "masterCuenta_";
            this.masterCuenta_.Size = new System.Drawing.Size(291, 22);
            this.masterCuenta_.TabIndex = 105;
            this.masterCuenta_.Value = "";
            // 
            // rbtTercero
            // 
            this.rbtTercero.AutoSize = true;
            this.rbtTercero.Checked = true;
            this.rbtTercero.Location = new System.Drawing.Point(378, 5);
            this.rbtTercero.Name = "rbtTercero";
            this.rbtTercero.Size = new System.Drawing.Size(14, 13);
            this.rbtTercero.TabIndex = 9;
            this.rbtTercero.TabStop = true;
            this.rbtTercero.UseVisualStyleBackColor = true;
            this.rbtTercero.CheckedChanged += new System.EventHandler(this.rbtTercero_CheckedChanged);
            // 
            // rbtPrefijo
            // 
            this.rbtPrefijo.AutoSize = true;
            this.rbtPrefijo.Location = new System.Drawing.Point(378, 28);
            this.rbtPrefijo.Name = "rbtPrefijo";
            this.rbtPrefijo.Size = new System.Drawing.Size(14, 13);
            this.rbtPrefijo.TabIndex = 10;
            this.rbtPrefijo.UseVisualStyleBackColor = true;
            this.rbtPrefijo.CheckedChanged += new System.EventHandler(this.rbtPrefijo_CheckedChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.masterPrefijo_);
            this.panelControl1.Controls.Add(this.rbtPrefijo);
            this.panelControl1.Controls.Add(this.rbtTercero);
            this.panelControl1.Controls.Add(this.masterTercero_);
            this.panelControl1.Controls.Add(this.masterDocumento);
            this.panelControl1.Controls.Add(this.txtDocumentoNroExt);
            this.panelControl1.Controls.Add(this.lblDocumentoNroExt);
            this.panelControl1.Controls.Add(this.lblDocumentoNroInt);
            this.panelControl1.Controls.Add(this.txtDocumentoNroInt);
            this.panelControl1.Location = new System.Drawing.Point(5, 11);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1059, 47);
            this.panelControl1.TabIndex = 108;
            // 
            // lblDescripcionHeader
            // 
            this.lblDescripcionHeader.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionHeader.Location = new System.Drawing.Point(841, 62);
            this.lblDescripcionHeader.Name = "lblDescripcionHeader";
            this.lblDescripcionHeader.Size = new System.Drawing.Size(93, 14);
            this.lblDescripcionHeader.TabIndex = 111;
            this.lblDescripcionHeader.Text = "18_lblDescripcion";
            // 
            // txtDescripcionHeader
            // 
            this.txtDescripcionHeader.Enabled = false;
            this.txtDescripcionHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcionHeader.Location = new System.Drawing.Point(841, 78);
            this.txtDescripcionHeader.Multiline = true;
            this.txtDescripcionHeader.Name = "txtDescripcionHeader";
            this.txtDescripcionHeader.Size = new System.Drawing.Size(215, 53);
            this.txtDescripcionHeader.TabIndex = 38;
            // 
            // txtSaldo
            // 
            this.txtSaldo.EditValue = "0";
            this.txtSaldo.Enabled = false;
            this.txtSaldo.Location = new System.Drawing.Point(705, 64);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldo.Properties.Appearance.Options.UseFont = true;
            this.txtSaldo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldo.Properties.Mask.EditMask = "c";
            this.txtSaldo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldo.Size = new System.Drawing.Size(120, 20);
            this.txtSaldo.TabIndex = 28;
            // 
            // lblSaldo
            // 
            this.lblSaldo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.Location = new System.Drawing.Point(623, 67);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(61, 14);
            this.lblSaldo.TabIndex = 27;
            this.lblSaldo.Text = "18_lblSaldo";
            // 
            // lblSaldoAjustado
            // 
            this.lblSaldoAjustado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldoAjustado.Location = new System.Drawing.Point(624, 117);
            this.lblSaldoAjustado.Name = "lblSaldoAjustado";
            this.lblSaldoAjustado.Size = new System.Drawing.Size(109, 14);
            this.lblSaldoAjustado.TabIndex = 112;
            this.lblSaldoAjustado.Text = "18_lblSaldoAjustado";
            // 
            // txtSaldoAjustado
            // 
            this.txtSaldoAjustado.EditValue = "0";
            this.txtSaldoAjustado.Enabled = false;
            this.txtSaldoAjustado.Location = new System.Drawing.Point(706, 114);
            this.txtSaldoAjustado.Name = "txtSaldoAjustado";
            this.txtSaldoAjustado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldoAjustado.Properties.Appearance.Options.UseFont = true;
            this.txtSaldoAjustado.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldoAjustado.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldoAjustado.Properties.Mask.EditMask = "c";
            this.txtSaldoAjustado.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldoAjustado.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldoAjustado.Size = new System.Drawing.Size(120, 20);
            this.txtSaldoAjustado.TabIndex = 113;
            // 
            // lblAjuste
            // 
            this.lblAjuste.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAjuste.Location = new System.Drawing.Point(623, 92);
            this.lblAjuste.Name = "lblAjuste";
            this.lblAjuste.Size = new System.Drawing.Size(67, 14);
            this.lblAjuste.TabIndex = 114;
            this.lblAjuste.Text = "18_lblAjuste";
            // 
            // txtAjuste
            // 
            this.txtAjuste.EditValue = "0";
            this.txtAjuste.Enabled = false;
            this.txtAjuste.Location = new System.Drawing.Point(705, 89);
            this.txtAjuste.Name = "txtAjuste";
            this.txtAjuste.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAjuste.Properties.Appearance.Options.UseFont = true;
            this.txtAjuste.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAjuste.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtAjuste.Properties.Mask.EditMask = "c";
            this.txtAjuste.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAjuste.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtAjuste.Size = new System.Drawing.Size(120, 20);
            this.txtAjuste.TabIndex = 115;
            // 
            // CruceCuentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "CruceCuentas";
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoAjustado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAjuste.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion
        
        private ControlsUC.uc_MasterFind masterTercero;
        private DevExpress.XtraEditors.LabelControl lblDocumentoNroInt;
        private System.Windows.Forms.TextBox txtDocumentoNroInt;
        private ControlsUC.uc_MasterFind masterMoneda;
        private ControlsUC.uc_MasterFind masterCuenta;      
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        private ControlsUC.uc_MasterFind masterLugarGeo;
        private DevExpress.XtraEditors.LabelControl lblFechaDoc;
        private DevExpress.XtraEditors.DateEdit dtFechaDoc;
        private ControlsUC.uc_MasterFind masterPrefijo_;
        private DevExpress.XtraEditors.LabelControl lblDocumentoNroExt;
        private System.Windows.Forms.TextBox txtDocumentoNroExt;
        private ControlsUC.uc_MasterFind masterCuenta_;
        private ControlsUC.uc_MasterFind masterTercero_;
        private ControlsUC.uc_MasterFind masterDocumento;
        private System.Windows.Forms.RadioButton rbtPrefijo;
        private System.Windows.Forms.RadioButton rbtTercero;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TextBox txtDescripcionHeader;
        private DevExpress.XtraEditors.LabelControl lblDescripcionHeader;
        private DevExpress.XtraEditors.LabelControl lblSaldo;
        private DevExpress.XtraEditors.TextEdit txtSaldo;
        private DevExpress.XtraEditors.LabelControl lblSaldoAjustado;
        private DevExpress.XtraEditors.TextEdit txtSaldoAjustado;
        private DevExpress.XtraEditors.LabelControl lblAjuste;
        private DevExpress.XtraEditors.TextEdit txtAjuste;

    }
}