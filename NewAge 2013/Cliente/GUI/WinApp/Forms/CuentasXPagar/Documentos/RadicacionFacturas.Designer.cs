namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RadicacionFacturas
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.cmbTipoModena = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblTipoMoneda = new System.Windows.Forms.Label();
            this.txtFechaRadicacion = new DevExpress.XtraEditors.TextEdit();
            this.lblNroRadicado = new System.Windows.Forms.Label();
            this.txtNroRadicado = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaRadicado = new System.Windows.Forms.Label();
            this.txtFactura = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoMovimiento = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblTipoMovimiento = new System.Windows.Forms.Label();
            this.txtValorTotal = new DevExpress.XtraEditors.TextEdit();
            this.txtValorIVA = new DevExpress.XtraEditors.TextEdit();
            this.txtValorFactura = new DevExpress.XtraEditors.TextEdit();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.lblValorIVA = new System.Windows.Forms.Label();
            this.lblValorFactura = new System.Windows.Forms.Label();
            this.lblFechaVencimiento = new System.Windows.Forms.Label();
            this.dtFechaVencimiento = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.dtFechaFactura = new DevExpress.XtraEditors.DateEdit();
            this.lblFactura = new System.Windows.Forms.Label();
            this.ucMasterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtOtraCausa = new DevExpress.XtraEditors.TextEdit();
            this.lblOtroCausa = new System.Windows.Forms.Label();
            this.btnGuardarCausa = new System.Windows.Forms.Button();
            this.cbNotaCredito = new System.Windows.Forms.CheckBox();
            this.chkProvisiones = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaRadicacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroRadicado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtraCausa.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.txtOtraCausa);
            this.grpboxDetail.Controls.Add(this.lblOtroCausa);
            this.grpboxDetail.Controls.Add(this.btnGuardarCausa);
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
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 272);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.chkProvisiones);
            this.grpboxHeader.Controls.Add(this.cbNotaCredito);
            this.grpboxHeader.Controls.Add(this.cmbTipoModena);
            this.grpboxHeader.Controls.Add(this.lblTipoMoneda);
            this.grpboxHeader.Controls.Add(this.lblNroRadicado);
            this.grpboxHeader.Controls.Add(this.cmbTipoMovimiento);
            this.grpboxHeader.Controls.Add(this.lblTipoMovimiento);
            this.grpboxHeader.Controls.Add(this.txtNroRadicado);
            this.grpboxHeader.Controls.Add(this.txtFactura);
            this.grpboxHeader.Controls.Add(this.txtValorTotal);
            this.grpboxHeader.Controls.Add(this.txtValorIVA);
            this.grpboxHeader.Controls.Add(this.txtValorFactura);
            this.grpboxHeader.Controls.Add(this.txtDescripcion);
            this.grpboxHeader.Controls.Add(this.lblDescripcion);
            this.grpboxHeader.Controls.Add(this.lblValorTotal);
            this.grpboxHeader.Controls.Add(this.lblValorIVA);
            this.grpboxHeader.Controls.Add(this.lblValorFactura);
            this.grpboxHeader.Controls.Add(this.lblFechaVencimiento);
            this.grpboxHeader.Controls.Add(this.dtFechaVencimiento);
            this.grpboxHeader.Controls.Add(this.lblFechaFactura);
            this.grpboxHeader.Controls.Add(this.dtFechaFactura);
            this.grpboxHeader.Controls.Add(this.lblFactura);
            this.grpboxHeader.Controls.Add(this.ucMasterTercero);
            this.grpboxHeader.Size = new System.Drawing.Size(1102, 234);
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
            // cmbTipoModena
            // 
            this.cmbTipoModena.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoModena.FormattingEnabled = true;
            this.cmbTipoModena.Location = new System.Drawing.Point(833, 72);
            this.cmbTipoModena.Name = "cmbTipoModena";
            this.cmbTipoModena.Size = new System.Drawing.Size(100, 22);
            this.cmbTipoModena.TabIndex = 7;
            // 
            // lblTipoMoneda
            // 
            this.lblTipoMoneda.AutoSize = true;
            this.lblTipoMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMoneda.Location = new System.Drawing.Point(744, 75);
            this.lblTipoMoneda.Name = "lblTipoMoneda";
            this.lblTipoMoneda.Size = new System.Drawing.Size(127, 14);
            this.lblTipoMoneda.TabIndex = 94;
            this.lblTipoMoneda.Text = "21501_lblTipoMoneda";
            // 
            // txtFechaRadicacion
            // 
            this.txtFechaRadicacion.Location = new System.Drawing.Point(0, 0);
            this.txtFechaRadicacion.Name = "txtFechaRadicacion";
            this.txtFechaRadicacion.Size = new System.Drawing.Size(100, 20);
            this.txtFechaRadicacion.TabIndex = 0;
            // 
            // lblNroRadicado
            // 
            this.lblNroRadicado.AutoSize = true;
            this.lblNroRadicado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroRadicado.Location = new System.Drawing.Point(364, 19);
            this.lblNroRadicado.Name = "lblNroRadicado";
            this.lblNroRadicado.Size = new System.Drawing.Size(127, 14);
            this.lblNroRadicado.TabIndex = 92;
            this.lblNroRadicado.Text = "21501_lblNroRadicado";
            // 
            // txtNroRadicado
            // 
            this.txtNroRadicado.Location = new System.Drawing.Point(450, 17);
            this.txtNroRadicado.Name = "txtNroRadicado";
            this.txtNroRadicado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroRadicado.Properties.Appearance.Options.UseFont = true;
            this.txtNroRadicado.Size = new System.Drawing.Size(64, 20);
            this.txtNroRadicado.TabIndex = 2;
            this.txtNroRadicado.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_ValorFactura_KeyPress);
            this.txtNroRadicado.Leave += new System.EventHandler(this.txtNroRadicado_Leave);
            // 
            // lblFechaRadicado
            // 
            this.lblFechaRadicado.Location = new System.Drawing.Point(0, 0);
            this.lblFechaRadicado.Name = "lblFechaRadicado";
            this.lblFechaRadicado.Size = new System.Drawing.Size(100, 23);
            this.lblFechaRadicado.TabIndex = 0;
            // 
            // txtFactura
            // 
            this.txtFactura.Location = new System.Drawing.Point(449, 46);
            this.txtFactura.Name = "txtFactura";
            this.txtFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFactura.Properties.Appearance.Options.UseFont = true;
            this.txtFactura.Size = new System.Drawing.Size(64, 20);
            this.txtFactura.TabIndex = 6;
            this.txtFactura.Leave += new System.EventHandler(this.txt_Factura_Leave);
            // 
            // cmbTipoMovimiento
            // 
            this.cmbTipoMovimiento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoMovimiento.FormattingEnabled = true;
            this.cmbTipoMovimiento.Location = new System.Drawing.Point(126, 14);
            this.cmbTipoMovimiento.Name = "cmbTipoMovimiento";
            this.cmbTipoMovimiento.Size = new System.Drawing.Size(118, 22);
            this.cmbTipoMovimiento.TabIndex = 1;
            this.cmbTipoMovimiento.SelectedValueChanged += new System.EventHandler(this.cmb_TipoMovimiento_SelectedValueChanged);
            // 
            // lblTipoMovimiento
            // 
            this.lblTipoMovimiento.AutoSize = true;
            this.lblTipoMovimiento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMovimiento.Location = new System.Drawing.Point(5, 19);
            this.lblTipoMovimiento.Name = "lblTipoMovimiento";
            this.lblTipoMovimiento.Size = new System.Drawing.Size(146, 14);
            this.lblTipoMovimiento.TabIndex = 0;
            this.lblTipoMovimiento.Text = "21501_lblTipoMovimiento";
            // 
            // txtValorTotal
            // 
            this.txtValorTotal.EditValue = "0";
            this.txtValorTotal.Enabled = false;
            this.txtValorTotal.Location = new System.Drawing.Point(609, 120);
            this.txtValorTotal.Name = "txtValorTotal";
            this.txtValorTotal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorTotal.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotal.Properties.Mask.EditMask = "c";
            this.txtValorTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotal.Size = new System.Drawing.Size(125, 20);
            this.txtValorTotal.TabIndex = 11;
            // 
            // txtValorIVA
            // 
            this.txtValorIVA.EditValue = "0";
            this.txtValorIVA.Location = new System.Drawing.Point(363, 120);
            this.txtValorIVA.Name = "txtValorIVA";
            this.txtValorIVA.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorIVA.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorIVA.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorIVA.Properties.Appearance.Options.UseFont = true;
            this.txtValorIVA.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorIVA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorIVA.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorIVA.Properties.Mask.EditMask = "c";
            this.txtValorIVA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorIVA.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorIVA.Size = new System.Drawing.Size(114, 20);
            this.txtValorIVA.TabIndex = 10;
            this.txtValorIVA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_ValorFactura_KeyPress);
            this.txtValorIVA.Leave += new System.EventHandler(this.txt_ValorIVA_Leave);
            // 
            // txtValorFactura
            // 
            this.txtValorFactura.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorFactura.EditValue = "0";
            this.txtValorFactura.Location = new System.Drawing.Point(126, 120);
            this.txtValorFactura.Name = "txtValorFactura";
            this.txtValorFactura.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorFactura.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorFactura.Properties.Appearance.Options.UseFont = true;
            this.txtValorFactura.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorFactura.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorFactura.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorFactura.Properties.Mask.EditMask = "c";
            this.txtValorFactura.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorFactura.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorFactura.Size = new System.Drawing.Size(118, 20);
            this.txtValorFactura.TabIndex = 9;
            this.txtValorFactura.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_ValorFactura_KeyPress);
            this.txtValorFactura.Leave += new System.EventHandler(this.txt_ValorFactura_Leave);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcion.Location = new System.Drawing.Point(126, 73);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(608, 41);
            this.txtDescripcion.TabIndex = 8;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(6, 72);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(121, 14);
            this.lblDescripcion.TabIndex = 17;
            this.lblDescripcion.Text = "21501_lblDescripcion";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.AutoSize = true;
            this.lblValorTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotal.Location = new System.Drawing.Point(495, 123);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(115, 14);
            this.lblValorTotal.TabIndex = 15;
            this.lblValorTotal.Text = "21501_lblValorTotal";
            // 
            // lblValorIVA
            // 
            this.lblValorIVA.AutoSize = true;
            this.lblValorIVA.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorIVA.Location = new System.Drawing.Point(258, 123);
            this.lblValorIVA.Name = "lblValorIVA";
            this.lblValorIVA.Size = new System.Drawing.Size(107, 14);
            this.lblValorIVA.TabIndex = 13;
            this.lblValorIVA.Text = "21501_lblValorIVA";
            // 
            // lblValorFactura
            // 
            this.lblValorFactura.AutoSize = true;
            this.lblValorFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorFactura.Location = new System.Drawing.Point(6, 123);
            this.lblValorFactura.Name = "lblValorFactura";
            this.lblValorFactura.Size = new System.Drawing.Size(127, 14);
            this.lblValorFactura.TabIndex = 11;
            this.lblValorFactura.Text = "21501_lblValorFactura";
            // 
            // lblFechaVencimiento
            // 
            this.lblFechaVencimiento.AutoSize = true;
            this.lblFechaVencimiento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVencimiento.Location = new System.Drawing.Point(526, 49);
            this.lblFechaVencimiento.Name = "lblFechaVencimiento";
            this.lblFechaVencimiento.Size = new System.Drawing.Size(160, 14);
            this.lblFechaVencimiento.TabIndex = 9;
            this.lblFechaVencimiento.Text = "21501_lblFechaVencimiento";
            // 
            // dtFechaVencimiento
            // 
            this.dtFechaVencimiento.EditValue = null;
            this.dtFechaVencimiento.Location = new System.Drawing.Point(640, 46);
            this.dtFechaVencimiento.Name = "dtFechaVencimiento";
            this.dtFechaVencimiento.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaVencimiento.Properties.Appearance.Options.UseFont = true;
            this.dtFechaVencimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaVencimiento.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaVencimiento.Size = new System.Drawing.Size(94, 20);
            this.dtFechaVencimiento.TabIndex = 4;
            // 
            // lblFechaFactura
            // 
            this.lblFechaFactura.AutoSize = true;
            this.lblFechaFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFactura.Location = new System.Drawing.Point(526, 19);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(132, 14);
            this.lblFechaFactura.TabIndex = 7;
            this.lblFechaFactura.Text = "21501_lblFechaFactura";
            // 
            // dtFechaFactura
            // 
            this.dtFechaFactura.EditValue = null;
            this.dtFechaFactura.Location = new System.Drawing.Point(640, 16);
            this.dtFechaFactura.Name = "dtFechaFactura";
            this.dtFechaFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaFactura.Properties.Appearance.Options.UseFont = true;
            this.dtFechaFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFactura.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFactura.Size = new System.Drawing.Size(94, 20);
            this.dtFechaFactura.TabIndex = 3;
            // 
            // lblFactura
            // 
            this.lblFactura.AutoSize = true;
            this.lblFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFactura.Location = new System.Drawing.Point(363, 48);
            this.lblFactura.Name = "lblFactura";
            this.lblFactura.Size = new System.Drawing.Size(100, 14);
            this.lblFactura.TabIndex = 4;
            this.lblFactura.Text = "21501_lblFactura";
            // 
            // ucMasterTercero
            // 
            this.ucMasterTercero.BackColor = System.Drawing.Color.Transparent;
            this.ucMasterTercero.Filtros = null;
            this.ucMasterTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucMasterTercero.Location = new System.Drawing.Point(9, 42);
            this.ucMasterTercero.Name = "ucMasterTercero";
            this.ucMasterTercero.Size = new System.Drawing.Size(362, 27);
            this.ucMasterTercero.TabIndex = 5;
            this.ucMasterTercero.Value = "";
            this.ucMasterTercero.Leave += new System.EventHandler(this.ucMasterTercero_Leave);
            // 
            // txtOtraCausa
            // 
            this.txtOtraCausa.Location = new System.Drawing.Point(126, 19);
            this.txtOtraCausa.Name = "txtOtraCausa";
            this.txtOtraCausa.Size = new System.Drawing.Size(694, 20);
            this.txtOtraCausa.TabIndex = 88;
            // 
            // lblOtroCausa
            // 
            this.lblOtroCausa.AutoSize = true;
            this.lblOtroCausa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOtroCausa.Location = new System.Drawing.Point(8, 21);
            this.lblOtroCausa.Name = "lblOtroCausa";
            this.lblOtroCausa.Size = new System.Drawing.Size(116, 14);
            this.lblOtroCausa.TabIndex = 83;
            this.lblOtroCausa.Text = "21501_lblOtroCausa";
            // 
            // btnGuardarCausa
            // 
            this.btnGuardarCausa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarCausa.Location = new System.Drawing.Point(827, 17);
            this.btnGuardarCausa.Name = "btnGuardarCausa";
            this.btnGuardarCausa.Size = new System.Drawing.Size(94, 23);
            this.btnGuardarCausa.TabIndex = 89;
            this.btnGuardarCausa.Text = "21501_btnGuardarCausa";
            this.btnGuardarCausa.UseVisualStyleBackColor = true;
            this.btnGuardarCausa.Click += new System.EventHandler(this.btnGuardarCausa_Click);
            // 
            // cbNotaCredito
            // 
            this.cbNotaCredito.AutoSize = true;
            this.cbNotaCredito.Location = new System.Drawing.Point(750, 18);
            this.cbNotaCredito.Name = "cbNotaCredito";
            this.cbNotaCredito.Size = new System.Drawing.Size(131, 17);
            this.cbNotaCredito.TabIndex = 95;
            this.cbNotaCredito.Text = "21501_cbNotaCredito";
            this.cbNotaCredito.UseVisualStyleBackColor = true;
            this.cbNotaCredito.CheckedChanged += new System.EventHandler(this.cbNotaCredito_CheckedChanged);
            // 
            // chkProvisiones
            // 
            this.chkProvisiones.AutoSize = true;
            this.chkProvisiones.Location = new System.Drawing.Point(750, 46);
            this.chkProvisiones.Name = "chkProvisiones";
            this.chkProvisiones.Size = new System.Drawing.Size(114, 17);
            this.chkProvisiones.TabIndex = 96;
            this.chkProvisiones.Text = "21_chkProvisiones";
            this.chkProvisiones.UseVisualStyleBackColor = true;
            this.chkProvisiones.CheckedChanged += new System.EventHandler(this.chkProvisiones_CheckedChanged);
            // 
            // RadicacionFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "RadicacionFacturas";
            this.Text = "RadicacionForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaRadicacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroRadicado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVencimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtraCausa.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Clases.ComboBoxEx cmbTipoMovimiento;
        private System.Windows.Forms.Label lblTipoMovimiento;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.Label lblValorIVA;
        private System.Windows.Forms.Label lblValorFactura;
        private System.Windows.Forms.Label lblFechaVencimiento;
        private DevExpress.XtraEditors.DateEdit dtFechaVencimiento;
        private System.Windows.Forms.Label lblFechaFactura;
        private DevExpress.XtraEditors.DateEdit dtFechaFactura;
        private System.Windows.Forms.Label lblFactura;
        private ControlsUC.uc_MasterFind ucMasterTercero;
        private DevExpress.XtraEditors.TextEdit txtFactura;
        private DevExpress.XtraEditors.TextEdit txtValorTotal;
        private DevExpress.XtraEditors.TextEdit txtValorIVA;
        private DevExpress.XtraEditors.TextEdit txtValorFactura;
        private System.Windows.Forms.GroupBox gr_GrillaCausas;
        private System.Windows.Forms.Label lblFechaRadicado;
        private System.Windows.Forms.Label lblNroRadicado;
        private DevExpress.XtraEditors.TextEdit txtNroRadicado;
        private DevExpress.XtraEditors.TextEdit txtOtraCausa;
        private System.Windows.Forms.Label lblOtroCausa;
        private DevExpress.XtraEditors.TextEdit txtFechaRadicacion;
        private Clases.ComboBoxEx cmbTipoModena;
        private System.Windows.Forms.Label lblTipoMoneda;
        private System.Windows.Forms.Button btnGuardarCausa;
        private System.Windows.Forms.CheckBox cbNotaCredito;
        private System.Windows.Forms.CheckBox chkProvisiones;
    }
}