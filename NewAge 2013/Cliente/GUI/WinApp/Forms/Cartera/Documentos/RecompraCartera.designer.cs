namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RecompraCartera
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterCompradorCartera = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.lblFechaCorte = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.txtDocRecompra = new System.Windows.Forms.TextBox();
            this.lblDocRecompra = new System.Windows.Forms.Label();
            this.lkp_Recompra = new DevExpress.XtraEditors.LookUpEdit();
            this.lblLkpRecompra = new System.Windows.Forms.Label();
            this.lblVlrTotalSaldo = new System.Windows.Forms.Label();
            this.txtVlrTotalRecompra = new DevExpress.XtraEditors.TextEdit();
            this.txtVlrTotalSaldo = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrTotalRecompra = new System.Windows.Forms.Label();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.gbSaldoCredito = new DevExpress.XtraEditors.GroupControl();
            this.lblVlrCapital = new System.Windows.Forms.Label();
            this.txtVlrCapital = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrInteres = new System.Windows.Forms.Label();
            this.txtVlrInteres = new DevExpress.XtraEditors.TextEdit();
            this.gbSaldoCesion = new DevExpress.XtraEditors.GroupControl();
            this.lblVlrUtilidad = new System.Windows.Forms.Label();
            this.txtVlrUtilidad = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrCesion = new System.Windows.Forms.Label();
            this.txtVlrCesion = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrDerechos = new System.Windows.Forms.Label();
            this.txtVlrDerechos = new DevExpress.XtraEditors.TextEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Recompra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalRecompra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbSaldoCredito)).BeginInit();
            this.gbSaldoCredito.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrCapital.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrInteres.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbSaldoCesion)).BeginInit();
            this.gbSaldoCesion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrUtilidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrCesion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrDerechos.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.gbSaldoCesion);
            this.grpboxDetail.Controls.Add(this.gbSaldoCredito);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Size = new System.Drawing.Size(1135, 104);
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
            this.editSpin0,
            this.editSpinPorc});
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
            this.editValue.Mask.EditMask = "c";
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(2);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(6);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(6);
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(6);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(839, 273);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(839, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 273);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.lblVlrTotalSaldo);
            this.grpboxHeader.Controls.Add(this.txtVlrTotalRecompra);
            this.grpboxHeader.Controls.Add(this.txtVlrTotalSaldo);
            this.grpboxHeader.Controls.Add(this.lblVlrTotalRecompra);
            this.grpboxHeader.Controls.Add(this.lkp_Recompra);
            this.grpboxHeader.Controls.Add(this.lblLkpRecompra);
            this.grpboxHeader.Controls.Add(this.txtDocRecompra);
            this.grpboxHeader.Controls.Add(this.lblDocRecompra);
            this.grpboxHeader.Controls.Add(this.lblFechaCorte);
            this.grpboxHeader.Controls.Add(this.dtFechaCorte);
            this.grpboxHeader.Controls.Add(this.masterCompradorCartera);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1131, 160);
            this.grpboxHeader.TabIndex = 0;
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
            // masterCompradorCartera
            // 
            this.masterCompradorCartera.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorCartera.Filtros = null;
            this.masterCompradorCartera.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCompradorCartera.Location = new System.Drawing.Point(6, 54);
            this.masterCompradorCartera.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.masterCompradorCartera.Name = "masterCompradorCartera";
            this.masterCompradorCartera.Size = new System.Drawing.Size(318, 29);
            this.masterCompradorCartera.TabIndex = 2;
            this.masterCompradorCartera.Value = "";
            this.masterCompradorCartera.Leave += new System.EventHandler(this.masterCompradorCartera_Leave);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P2";
            this.editSpinPorc.Name = "editSpinPorc";
            // 
            // lblFechaCorte
            // 
            this.lblFechaCorte.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaCorte.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaCorte.Location = new System.Drawing.Point(353, 61);
            this.lblFechaCorte.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaCorte.Name = "lblFechaCorte";
            this.lblFechaCorte.Size = new System.Drawing.Size(101, 14);
            this.lblFechaCorte.TabIndex = 5;
            this.lblFechaCorte.Text = "165_lblFechaCorte";
            // 
            // dtFechaCorte
            // 
            this.dtFechaCorte.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaCorte.Location = new System.Drawing.Point(455, 58);
            this.dtFechaCorte.Name = "dtFechaCorte";
            this.dtFechaCorte.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCorte.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaCorte.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCorte.Properties.Appearance.Options.UseFont = true;
            this.dtFechaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCorte.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCorte.Size = new System.Drawing.Size(100, 20);
            this.dtFechaCorte.TabIndex = 6;
            // 
            // txtDocRecompra
            // 
            this.txtDocRecompra.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocRecompra.Location = new System.Drawing.Point(454, 25);
            this.txtDocRecompra.Margin = new System.Windows.Forms.Padding(2);
            this.txtDocRecompra.Name = "txtDocRecompra";
            this.txtDocRecompra.Size = new System.Drawing.Size(64, 22);
            this.txtDocRecompra.TabIndex = 4;
            // 
            // lblDocRecompra
            // 
            this.lblDocRecompra.AutoSize = true;
            this.lblDocRecompra.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocRecompra.Location = new System.Drawing.Point(351, 28);
            this.lblDocRecompra.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDocRecompra.Name = "lblDocRecompra";
            this.lblDocRecompra.Size = new System.Drawing.Size(115, 16);
            this.lblDocRecompra.TabIndex = 3;
            this.lblDocRecompra.Text = "165_DocRecompra";
            // 
            // lkp_Recompra
            // 
            this.lkp_Recompra.Location = new System.Drawing.Point(123, 26);
            this.lkp_Recompra.Margin = new System.Windows.Forms.Padding(2);
            this.lkp_Recompra.Name = "lkp_Recompra";
            this.lkp_Recompra.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkp_Recompra.Properties.Appearance.Options.UseFont = true;
            this.lkp_Recompra.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Recompra.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_Recompra.Properties.DisplayMember = "Value";
            this.lkp_Recompra.Properties.NullText = " ";
            this.lkp_Recompra.Properties.ValueMember = "Key";
            this.lkp_Recompra.Size = new System.Drawing.Size(117, 20);
            this.lkp_Recompra.TabIndex = 1;
            this.lkp_Recompra.EditValueChanged += new System.EventHandler(this.lkp_Recompra_EditValueChanged);
            // 
            // lblLkpRecompra
            // 
            this.lblLkpRecompra.AutoSize = true;
            this.lblLkpRecompra.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLkpRecompra.Location = new System.Drawing.Point(4, 28);
            this.lblLkpRecompra.Name = "lblLkpRecompra";
            this.lblLkpRecompra.Size = new System.Drawing.Size(119, 16);
            this.lblLkpRecompra.TabIndex = 0;
            this.lblLkpRecompra.Text = "165_TipoRecompra";
            // 
            // lblVlrTotalSaldo
            // 
            this.lblVlrTotalSaldo.AutoSize = true;
            this.lblVlrTotalSaldo.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrTotalSaldo.Location = new System.Drawing.Point(644, 60);
            this.lblVlrTotalSaldo.Name = "lblVlrTotalSaldo";
            this.lblVlrTotalSaldo.Size = new System.Drawing.Size(126, 16);
            this.lblVlrTotalSaldo.TabIndex = 13;
            this.lblVlrTotalSaldo.Text = "165_lblVlrTotalSaldo";
            // 
            // txtVlrTotalRecompra
            // 
            this.txtVlrTotalRecompra.EditValue = "0";
            this.txtVlrTotalRecompra.Location = new System.Drawing.Point(788, 26);
            this.txtVlrTotalRecompra.Name = "txtVlrTotalRecompra";
            this.txtVlrTotalRecompra.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalRecompra.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalRecompra.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalRecompra.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalRecompra.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalRecompra.Properties.Mask.EditMask = "c";
            this.txtVlrTotalRecompra.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalRecompra.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalRecompra.Properties.ReadOnly = true;
            this.txtVlrTotalRecompra.Size = new System.Drawing.Size(121, 20);
            this.txtVlrTotalRecompra.TabIndex = 12;
            // 
            // txtVlrTotalSaldo
            // 
            this.txtVlrTotalSaldo.EditValue = "0";
            this.txtVlrTotalSaldo.Location = new System.Drawing.Point(787, 58);
            this.txtVlrTotalSaldo.Name = "txtVlrTotalSaldo";
            this.txtVlrTotalSaldo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalSaldo.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalSaldo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalSaldo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalSaldo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalSaldo.Properties.Mask.EditMask = "c";
            this.txtVlrTotalSaldo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalSaldo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalSaldo.Size = new System.Drawing.Size(121, 20);
            this.txtVlrTotalSaldo.TabIndex = 14;
            // 
            // lblVlrTotalRecompra
            // 
            this.lblVlrTotalRecompra.AutoSize = true;
            this.lblVlrTotalRecompra.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrTotalRecompra.Location = new System.Drawing.Point(644, 28);
            this.lblVlrTotalRecompra.Name = "lblVlrTotalRecompra";
            this.lblVlrTotalRecompra.Size = new System.Drawing.Size(139, 16);
            this.lblVlrTotalRecompra.TabIndex = 11;
            this.lblVlrTotalRecompra.Text = "165_VlrTotalRecompra";
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(123, 90);
            this.txtLibranza.Margin = new System.Windows.Forms.Padding(2);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(78, 22);
            this.txtLibranza.TabIndex = 16;
            this.txtLibranza.Visible = false;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(8, 93);
            this.lblLibranza.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(84, 16);
            this.lblLibranza.TabIndex = 15;
            this.lblLibranza.Text = "165_Libranza";
            this.lblLibranza.Visible = false;
            // 
            // gbSaldoCredito
            // 
            this.gbSaldoCredito.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gbSaldoCredito.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.gbSaldoCredito.AppearanceCaption.Options.UseFont = true;
            this.gbSaldoCredito.Controls.Add(this.lblVlrInteres);
            this.gbSaldoCredito.Controls.Add(this.txtVlrInteres);
            this.gbSaldoCredito.Controls.Add(this.lblVlrCapital);
            this.gbSaldoCredito.Controls.Add(this.txtVlrCapital);
            this.gbSaldoCredito.Location = new System.Drawing.Point(9, 11);
            this.gbSaldoCredito.Name = "gbSaldoCredito";
            this.gbSaldoCredito.Size = new System.Drawing.Size(233, 87);
            this.gbSaldoCredito.TabIndex = 70;
            this.gbSaldoCredito.Text = "Saldo Crédito";
            this.gbSaldoCredito.Visible = false;
            // 
            // lblVlrCapital
            // 
            this.lblVlrCapital.AutoSize = true;
            this.lblVlrCapital.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrCapital.Location = new System.Drawing.Point(7, 27);
            this.lblVlrCapital.Name = "lblVlrCapital";
            this.lblVlrCapital.Size = new System.Drawing.Size(46, 17);
            this.lblVlrCapital.TabIndex = 71;
            this.lblVlrCapital.Text = "Capital";
            // 
            // txtVlrCapital
            // 
            this.txtVlrCapital.EditValue = "0,00 ";
            this.txtVlrCapital.Location = new System.Drawing.Point(85, 25);
            this.txtVlrCapital.Name = "txtVlrCapital";
            this.txtVlrCapital.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtVlrCapital.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrCapital.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrCapital.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrCapital.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrCapital.Properties.Appearance.Options.UseFont = true;
            this.txtVlrCapital.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrCapital.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrCapital.Properties.AutoHeight = false;
            this.txtVlrCapital.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrCapital.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrCapital.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrCapital.Properties.Mask.EditMask = "c";
            this.txtVlrCapital.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrCapital.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrCapital.Properties.ReadOnly = true;
            this.txtVlrCapital.Size = new System.Drawing.Size(115, 18);
            this.txtVlrCapital.TabIndex = 72;
            // 
            // lblVlrInteres
            // 
            this.lblVlrInteres.AutoSize = true;
            this.lblVlrInteres.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrInteres.Location = new System.Drawing.Point(7, 48);
            this.lblVlrInteres.Name = "lblVlrInteres";
            this.lblVlrInteres.Size = new System.Drawing.Size(44, 17);
            this.lblVlrInteres.TabIndex = 87;
            this.lblVlrInteres.Text = "Interés";
            // 
            // txtVlrInteres
            // 
            this.txtVlrInteres.EditValue = "0,00 ";
            this.txtVlrInteres.Location = new System.Drawing.Point(85, 46);
            this.txtVlrInteres.Name = "txtVlrInteres";
            this.txtVlrInteres.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtVlrInteres.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrInteres.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrInteres.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrInteres.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrInteres.Properties.Appearance.Options.UseFont = true;
            this.txtVlrInteres.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrInteres.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrInteres.Properties.AutoHeight = false;
            this.txtVlrInteres.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrInteres.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrInteres.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrInteres.Properties.Mask.EditMask = "c";
            this.txtVlrInteres.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrInteres.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrInteres.Properties.ReadOnly = true;
            this.txtVlrInteres.Size = new System.Drawing.Size(115, 18);
            this.txtVlrInteres.TabIndex = 88;
            // 
            // gbSaldoCesion
            // 
            this.gbSaldoCesion.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gbSaldoCesion.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.gbSaldoCesion.AppearanceCaption.Options.UseFont = true;
            this.gbSaldoCesion.Controls.Add(this.lblVlrDerechos);
            this.gbSaldoCesion.Controls.Add(this.lblVlrUtilidad);
            this.gbSaldoCesion.Controls.Add(this.txtVlrDerechos);
            this.gbSaldoCesion.Controls.Add(this.txtVlrUtilidad);
            this.gbSaldoCesion.Controls.Add(this.lblVlrCesion);
            this.gbSaldoCesion.Controls.Add(this.txtVlrCesion);
            this.gbSaldoCesion.Location = new System.Drawing.Point(253, 11);
            this.gbSaldoCesion.Name = "gbSaldoCesion";
            this.gbSaldoCesion.Size = new System.Drawing.Size(233, 88);
            this.gbSaldoCesion.TabIndex = 71;
            this.gbSaldoCesion.Text = "Saldo Cesión";
            this.gbSaldoCesion.Visible = false;
            // 
            // lblVlrUtilidad
            // 
            this.lblVlrUtilidad.AutoSize = true;
            this.lblVlrUtilidad.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrUtilidad.Location = new System.Drawing.Point(8, 46);
            this.lblVlrUtilidad.Name = "lblVlrUtilidad";
            this.lblVlrUtilidad.Size = new System.Drawing.Size(49, 17);
            this.lblVlrUtilidad.TabIndex = 87;
            this.lblVlrUtilidad.Text = "Utilidad";
            // 
            // txtVlrUtilidad
            // 
            this.txtVlrUtilidad.EditValue = "0,00 ";
            this.txtVlrUtilidad.Location = new System.Drawing.Point(86, 44);
            this.txtVlrUtilidad.Name = "txtVlrUtilidad";
            this.txtVlrUtilidad.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtVlrUtilidad.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrUtilidad.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrUtilidad.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrUtilidad.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrUtilidad.Properties.Appearance.Options.UseFont = true;
            this.txtVlrUtilidad.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrUtilidad.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrUtilidad.Properties.AutoHeight = false;
            this.txtVlrUtilidad.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrUtilidad.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrUtilidad.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrUtilidad.Properties.Mask.EditMask = "c";
            this.txtVlrUtilidad.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrUtilidad.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrUtilidad.Properties.ReadOnly = true;
            this.txtVlrUtilidad.Size = new System.Drawing.Size(115, 18);
            this.txtVlrUtilidad.TabIndex = 88;
            // 
            // lblVlrCesion
            // 
            this.lblVlrCesion.AutoSize = true;
            this.lblVlrCesion.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrCesion.Location = new System.Drawing.Point(8, 25);
            this.lblVlrCesion.Name = "lblVlrCesion";
            this.lblVlrCesion.Size = new System.Drawing.Size(46, 17);
            this.lblVlrCesion.TabIndex = 71;
            this.lblVlrCesion.Text = "Cesión";
            // 
            // txtVlrCesion
            // 
            this.txtVlrCesion.EditValue = "0,00 ";
            this.txtVlrCesion.Location = new System.Drawing.Point(86, 23);
            this.txtVlrCesion.Name = "txtVlrCesion";
            this.txtVlrCesion.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtVlrCesion.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrCesion.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrCesion.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrCesion.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrCesion.Properties.Appearance.Options.UseFont = true;
            this.txtVlrCesion.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrCesion.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrCesion.Properties.AutoHeight = false;
            this.txtVlrCesion.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrCesion.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrCesion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrCesion.Properties.Mask.EditMask = "c";
            this.txtVlrCesion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrCesion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrCesion.Properties.ReadOnly = true;
            this.txtVlrCesion.Size = new System.Drawing.Size(115, 18);
            this.txtVlrCesion.TabIndex = 72;
            // 
            // lblVlrDerechos
            // 
            this.lblVlrDerechos.AutoSize = true;
            this.lblVlrDerechos.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrDerechos.Location = new System.Drawing.Point(8, 66);
            this.lblVlrDerechos.Name = "lblVlrDerechos";
            this.lblVlrDerechos.Size = new System.Drawing.Size(58, 17);
            this.lblVlrDerechos.TabIndex = 89;
            this.lblVlrDerechos.Text = "Derechos";
            // 
            // txtVlrDerechos
            // 
            this.txtVlrDerechos.EditValue = "0,00 ";
            this.txtVlrDerechos.Location = new System.Drawing.Point(86, 65);
            this.txtVlrDerechos.Name = "txtVlrDerechos";
            this.txtVlrDerechos.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtVlrDerechos.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrDerechos.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrDerechos.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrDerechos.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrDerechos.Properties.Appearance.Options.UseFont = true;
            this.txtVlrDerechos.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrDerechos.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrDerechos.Properties.AutoHeight = false;
            this.txtVlrDerechos.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrDerechos.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrDerechos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrDerechos.Properties.Mask.EditMask = "c";
            this.txtVlrDerechos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrDerechos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrDerechos.Properties.ReadOnly = true;
            this.txtVlrDerechos.Size = new System.Drawing.Size(115, 18);
            this.txtVlrDerechos.TabIndex = 90;
            // 
            // RecompraCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1163, 583);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "RecompraCartera";
            this.grpboxDetail.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Recompra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalRecompra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbSaldoCredito)).EndInit();
            this.gbSaldoCredito.ResumeLayout(false);
            this.gbSaldoCredito.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrCapital.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrInteres.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbSaldoCesion)).EndInit();
            this.gbSaldoCesion.ResumeLayout(false);
            this.gbSaldoCesion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrUtilidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrCesion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrDerechos.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private ControlsUC.uc_MasterFind masterCompradorCartera;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        private DevExpress.XtraEditors.LabelControl lblFechaCorte;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
        private System.Windows.Forms.TextBox txtDocRecompra;
        private System.Windows.Forms.Label lblDocRecompra;
        private DevExpress.XtraEditors.LookUpEdit lkp_Recompra;
        private System.Windows.Forms.Label lblLkpRecompra;
        private System.Windows.Forms.Label lblVlrTotalSaldo;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalRecompra;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalSaldo;
        private System.Windows.Forms.Label lblVlrTotalRecompra;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private DevExpress.XtraEditors.GroupControl gbSaldoCredito;
        private System.Windows.Forms.Label lblVlrInteres;
        private DevExpress.XtraEditors.TextEdit txtVlrInteres;
        private System.Windows.Forms.Label lblVlrCapital;
        private DevExpress.XtraEditors.TextEdit txtVlrCapital;
        private DevExpress.XtraEditors.GroupControl gbSaldoCesion;
        private System.Windows.Forms.Label lblVlrUtilidad;
        private DevExpress.XtraEditors.TextEdit txtVlrUtilidad;
        private System.Windows.Forms.Label lblVlrCesion;
        private DevExpress.XtraEditors.TextEdit txtVlrCesion;
        private System.Windows.Forms.Label lblVlrDerechos;
        private DevExpress.XtraEditors.TextEdit txtVlrDerechos;


    }
}