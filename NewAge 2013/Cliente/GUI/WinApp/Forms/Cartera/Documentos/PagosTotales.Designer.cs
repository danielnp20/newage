namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PagosTotales
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lkp_Libranzas = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCaja = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaConsigna = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaConsignacion = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBanco = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlPlanPago = new System.Windows.Forms.Panel();
            this.txtTotalComponentes = new DevExpress.XtraEditors.TextEdit();
            this.lblPlanPago = new System.Windows.Forms.Label();
            this.pnlFormaPago = new System.Windows.Forms.Panel();
            this.txtTotalFormaPago = new DevExpress.XtraEditors.TextEdit();
            this.lbl_FormaPago = new System.Windows.Forms.Label();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblVlrDeuda = new System.Windows.Forms.Label();
            this.txtVlrDeuda = new DevExpress.XtraEditors.TextEdit();
            this.txtVlrSaldo = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrSaldo = new System.Windows.Forms.Label();
            this.txtVlrAbonado = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrAbonado = new System.Windows.Forms.Label();
            this.btnUpdValues = new System.Windows.Forms.Button();
            this.lblDescriptivo = new System.Windows.Forms.Label();
            this.txtDescriptivo = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).BeginInit();
            this.pnlPlanPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).BeginInit();
            this.pnlFormaPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrDeuda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAbonado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.tableLayoutPanel1);
            this.grpboxDetail.Location = new System.Drawing.Point(0, 5);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Size = new System.Drawing.Size(1117, 198);
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
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
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
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(843, 303);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(843, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 303);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblDescriptivo);
            this.grpboxHeader.Controls.Add(this.txtDescriptivo);
            this.grpboxHeader.Controls.Add(this.btnUpdValues);
            this.grpboxHeader.Controls.Add(this.txtVlrAbonado);
            this.grpboxHeader.Controls.Add(this.lblVlrAbonado);
            this.grpboxHeader.Controls.Add(this.txtVlrSaldo);
            this.grpboxHeader.Controls.Add(this.lblVlrSaldo);
            this.grpboxHeader.Controls.Add(this.txtVlrDeuda);
            this.grpboxHeader.Controls.Add(this.lblVlrDeuda);
            this.grpboxHeader.Controls.Add(this.masterBanco);
            this.grpboxHeader.Controls.Add(this.lkp_Libranzas);
            this.grpboxHeader.Controls.Add(this.masterCaja);
            this.grpboxHeader.Controls.Add(this.dtFechaConsigna);
            this.grpboxHeader.Controls.Add(this.lblFechaConsignacion);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Location = new System.Drawing.Point(8, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1111, 459);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "c3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lkp_Libranzas
            // 
            this.lkp_Libranzas.Location = new System.Drawing.Point(113, 75);
            this.lkp_Libranzas.Name = "lkp_Libranzas";
            this.lkp_Libranzas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Libranzas.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NumeroDoc", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Descriptivo")});
            this.lkp_Libranzas.Properties.DisplayMember = "Libranza";
            this.lkp_Libranzas.Properties.NullText = "";
            this.lkp_Libranzas.Properties.ValueMember = "NumeroDoc";
            this.lkp_Libranzas.Size = new System.Drawing.Size(117, 20);
            this.lkp_Libranzas.TabIndex = 8;
            this.lkp_Libranzas.Leave += new System.EventHandler(this.lkp_Libranzas_Leave);
            // 
            // masterCaja
            // 
            this.masterCaja.BackColor = System.Drawing.Color.Transparent;
            this.masterCaja.Filtros = null;
            this.masterCaja.Location = new System.Drawing.Point(14, 17);
            this.masterCaja.Margin = new System.Windows.Forms.Padding(12);
            this.masterCaja.Name = "masterCaja";
            this.masterCaja.Size = new System.Drawing.Size(313, 26);
            this.masterCaja.TabIndex = 0;
            this.masterCaja.Value = "";
            // 
            // dtFechaConsigna
            // 
            this.dtFechaConsigna.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaConsigna.Location = new System.Drawing.Point(114, 47);
            this.dtFechaConsigna.Name = "dtFechaConsigna";
            this.dtFechaConsigna.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaConsigna.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaConsigna.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaConsigna.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaConsigna.Properties.Appearance.Options.UseFont = true;
            this.dtFechaConsigna.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaConsigna.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaConsigna.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaConsigna.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaConsigna.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaConsigna.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaConsigna.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaConsigna.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaConsigna.Size = new System.Drawing.Size(117, 20);
            this.dtFechaConsigna.TabIndex = 5;
            // 
            // lblFechaConsignacion
            // 
            this.lblFechaConsignacion.AutoSize = true;
            this.lblFechaConsignacion.Location = new System.Drawing.Point(11, 51);
            this.lblFechaConsignacion.Name = "lblFechaConsignacion";
            this.lblFechaConsignacion.Size = new System.Drawing.Size(123, 13);
            this.lblFechaConsignacion.TabIndex = 4;
            this.lblFechaConsignacion.Text = "168_FechaConsignacion";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(11, 77);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(71, 13);
            this.lblLibranza.TabIndex = 7;
            this.lblLibranza.Text = "168_Libranza";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(326, 46);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(12);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(297, 24);
            this.masterCliente.TabIndex = 6;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // masterBanco
            // 
            this.masterBanco.BackColor = System.Drawing.Color.Transparent;
            this.masterBanco.Filtros = null;
            this.masterBanco.Location = new System.Drawing.Point(326, 18);
            this.masterBanco.Margin = new System.Windows.Forms.Padding(12);
            this.masterBanco.Name = "masterBanco";
            this.masterBanco.Size = new System.Drawing.Size(298, 25);
            this.masterBanco.TabIndex = 1;
            this.masterBanco.Value = "";
            // 
            // pnlPlanPago
            // 
            this.pnlPlanPago.Controls.Add(this.txtTotalComponentes);
            this.pnlPlanPago.Controls.Add(this.lblPlanPago);
            this.pnlPlanPago.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPlanPago.Location = new System.Drawing.Point(808, 3);
            this.pnlPlanPago.Name = "pnlPlanPago";
            this.pnlPlanPago.Size = new System.Drawing.Size(294, 19);
            this.pnlPlanPago.TabIndex = 23;
            // 
            // txtTotalComponentes
            // 
            this.txtTotalComponentes.EditValue = "0";
            this.txtTotalComponentes.Location = new System.Drawing.Point(167, 0);
            this.txtTotalComponentes.Name = "txtTotalComponentes";
            this.txtTotalComponentes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalComponentes.Properties.Appearance.Options.UseFont = true;
            this.txtTotalComponentes.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalComponentes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalComponentes.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalComponentes.Properties.Mask.EditMask = "c";
            this.txtTotalComponentes.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalComponentes.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalComponentes.Properties.ReadOnly = true;
            this.txtTotalComponentes.Size = new System.Drawing.Size(130, 20);
            this.txtTotalComponentes.TabIndex = 24;
            // 
            // lblPlanPago
            // 
            this.lblPlanPago.AutoSize = true;
            this.lblPlanPago.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblPlanPago.Location = new System.Drawing.Point(68, 2);
            this.lblPlanPago.Name = "lblPlanPago";
            this.lblPlanPago.Size = new System.Drawing.Size(106, 13);
            this.lblPlanPago.TabIndex = 22;
            this.lblPlanPago.Text = "lbl_168_VlrDeuda";
            // 
            // pnlFormaPago
            // 
            this.pnlFormaPago.Controls.Add(this.txtTotalFormaPago);
            this.pnlFormaPago.Controls.Add(this.lbl_FormaPago);
            this.pnlFormaPago.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFormaPago.Location = new System.Drawing.Point(808, 145);
            this.pnlFormaPago.Name = "pnlFormaPago";
            this.pnlFormaPago.Size = new System.Drawing.Size(294, 24);
            this.pnlFormaPago.TabIndex = 24;
            // 
            // txtTotalFormaPago
            // 
            this.txtTotalFormaPago.EditValue = "0";
            this.txtTotalFormaPago.Location = new System.Drawing.Point(165, 1);
            this.txtTotalFormaPago.Name = "txtTotalFormaPago";
            this.txtTotalFormaPago.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalFormaPago.Properties.Appearance.Options.UseFont = true;
            this.txtTotalFormaPago.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalFormaPago.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalFormaPago.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.EditMask = "c";
            this.txtTotalFormaPago.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalFormaPago.Properties.ReadOnly = true;
            this.txtTotalFormaPago.Size = new System.Drawing.Size(130, 20);
            this.txtTotalFormaPago.TabIndex = 24;
            // 
            // lbl_FormaPago
            // 
            this.lbl_FormaPago.AutoSize = true;
            this.lbl_FormaPago.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lbl_FormaPago.Location = new System.Drawing.Point(76, 3);
            this.lbl_FormaPago.Name = "lbl_FormaPago";
            this.lbl_FormaPago.Size = new System.Drawing.Size(119, 13);
            this.lbl_FormaPago.TabIndex = 22;
            this.lbl_FormaPago.Text = "lbl_163_FormaPago";
            // 
            // gcPagos
            // 
            this.gcPagos.AllowDrop = true;
            this.gcPagos.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcPagos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPagos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcPagos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPagos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcPagos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcPagos.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcPagos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPagos.Location = new System.Drawing.Point(602, 28);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvPagos;
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(500, 111);
            this.gcPagos.TabIndex = 5;
            this.gcPagos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPagos,
            this.gridView1});
            // 
            // gvPagos
            // 
            this.gvPagos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPagos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvPagos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.Empty.Options.UseBackColor = true;
            this.gvPagos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvPagos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPagos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvPagos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvPagos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPagos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPagos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPagos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPagos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvPagos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPagos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPagos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.Row.Options.UseBackColor = true;
            this.gvPagos.Appearance.Row.Options.UseForeColor = true;
            this.gvPagos.Appearance.Row.Options.UseTextOptions = true;
            this.gvPagos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvPagos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPagos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPagos.GridControl = this.gcPagos;
            this.gvPagos.Name = "gvPagos";
            this.gvPagos.OptionsCustomization.AllowColumnMoving = false;
            this.gvPagos.OptionsCustomization.AllowFilter = false;
            this.gvPagos.OptionsCustomization.AllowSort = false;
            this.gvPagos.OptionsView.ShowGroupPanel = false;
            this.gvPagos.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvPagos_CellValueChanged);
            this.gvPagos.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvPagos_BeforeLeaveRow);
            this.gvPagos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcPagos;
            this.gridView1.Name = "gridView1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.20815F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.79185F));
            this.tableLayoutPanel1.Controls.Add(this.gcPagos, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlFormaPago, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlPlanPago, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.60563F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.39436F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1105, 172);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblVlrDeuda
            // 
            this.lblVlrDeuda.AutoSize = true;
            this.lblVlrDeuda.Location = new System.Drawing.Point(641, 25);
            this.lblVlrDeuda.Name = "lblVlrDeuda";
            this.lblVlrDeuda.Size = new System.Drawing.Size(90, 13);
            this.lblVlrDeuda.TabIndex = 23;
            this.lblVlrDeuda.Text = "lbl_168_VlrDeuda";
            // 
            // txtVlrDeuda
            // 
            this.txtVlrDeuda.EditValue = "0";
            this.txtVlrDeuda.Enabled = false;
            this.txtVlrDeuda.Location = new System.Drawing.Point(737, 21);
            this.txtVlrDeuda.Name = "txtVlrDeuda";
            this.txtVlrDeuda.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrDeuda.Properties.Appearance.Options.UseFont = true;
            this.txtVlrDeuda.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrDeuda.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrDeuda.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrDeuda.Properties.Mask.EditMask = "c";
            this.txtVlrDeuda.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrDeuda.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrDeuda.Properties.ReadOnly = true;
            this.txtVlrDeuda.Size = new System.Drawing.Size(119, 20);
            this.txtVlrDeuda.TabIndex = 25;
            // 
            // txtVlrSaldo
            // 
            this.txtVlrSaldo.EditValue = "0";
            this.txtVlrSaldo.Enabled = false;
            this.txtVlrSaldo.Location = new System.Drawing.Point(737, 76);
            this.txtVlrSaldo.Name = "txtVlrSaldo";
            this.txtVlrSaldo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrSaldo.Properties.Appearance.Options.UseFont = true;
            this.txtVlrSaldo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrSaldo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrSaldo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrSaldo.Properties.Mask.EditMask = "c";
            this.txtVlrSaldo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrSaldo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrSaldo.Properties.ReadOnly = true;
            this.txtVlrSaldo.Size = new System.Drawing.Size(119, 20);
            this.txtVlrSaldo.TabIndex = 27;
            // 
            // lblVlrSaldo
            // 
            this.lblVlrSaldo.AutoSize = true;
            this.lblVlrSaldo.Location = new System.Drawing.Point(641, 80);
            this.lblVlrSaldo.Name = "lblVlrSaldo";
            this.lblVlrSaldo.Size = new System.Drawing.Size(85, 13);
            this.lblVlrSaldo.TabIndex = 26;
            this.lblVlrSaldo.Text = "lbl_168_VlrSaldo";
            // 
            // txtVlrAbonado
            // 
            this.txtVlrAbonado.EditValue = "0";
            this.txtVlrAbonado.Enabled = false;
            this.txtVlrAbonado.Location = new System.Drawing.Point(737, 48);
            this.txtVlrAbonado.Name = "txtVlrAbonado";
            this.txtVlrAbonado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrAbonado.Properties.Appearance.Options.UseFont = true;
            this.txtVlrAbonado.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrAbonado.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrAbonado.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrAbonado.Properties.Mask.EditMask = "c";
            this.txtVlrAbonado.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrAbonado.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrAbonado.Properties.ReadOnly = true;
            this.txtVlrAbonado.Size = new System.Drawing.Size(119, 20);
            this.txtVlrAbonado.TabIndex = 29;
            // 
            // lblVlrAbonado
            // 
            this.lblVlrAbonado.AutoSize = true;
            this.lblVlrAbonado.Location = new System.Drawing.Point(641, 52);
            this.lblVlrAbonado.Name = "lblVlrAbonado";
            this.lblVlrAbonado.Size = new System.Drawing.Size(102, 13);
            this.lblVlrAbonado.TabIndex = 28;
            this.lblVlrAbonado.Text = "lbl_168_VlrAbonado";
            // 
            // btnUpdValues
            // 
            this.btnUpdValues.Location = new System.Drawing.Point(967, 17);
            this.btnUpdValues.Name = "btnUpdValues";
            this.btnUpdValues.Size = new System.Drawing.Size(119, 23);
            this.btnUpdValues.TabIndex = 30;
            this.btnUpdValues.Text = "btn_168_UpdateValues";
            this.btnUpdValues.UseVisualStyleBackColor = true;
            this.btnUpdValues.Visible = false;
            this.btnUpdValues.Click += new System.EventHandler(this.btnUpdValues_Click);
            // 
            // lblDescriptivo
            // 
            this.lblDescriptivo.AutoSize = true;
            this.lblDescriptivo.Location = new System.Drawing.Point(866, 49);
            this.lblDescriptivo.Name = "lblDescriptivo";
            this.lblDescriptivo.Size = new System.Drawing.Size(61, 13);
            this.lblDescriptivo.TabIndex = 32;
            this.lblDescriptivo.Text = "Descripción";
            // 
            // txtDescriptivo
            // 
            this.txtDescriptivo.Location = new System.Drawing.Point(865, 62);
            this.txtDescriptivo.Multiline = true;
            this.txtDescriptivo.Name = "txtDescriptivo";
            this.txtDescriptivo.Size = new System.Drawing.Size(229, 56);
            this.txtDescriptivo.TabIndex = 31;
            // 
            // PagosTotales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1167, 634);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PagosTotales";
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).EndInit();
            this.pnlPlanPago.ResumeLayout(false);
            this.pnlPlanPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).EndInit();
            this.pnlFormaPago.ResumeLayout(false);
            this.pnlFormaPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrDeuda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAbonado.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkp_Libranzas;
        private ControlsUC.uc_MasterFind masterCaja;
        protected DevExpress.XtraEditors.DateEdit dtFechaConsigna;
        private System.Windows.Forms.Label lblFechaConsignacion;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCliente;
        private ControlsUC.uc_MasterFind masterBanco;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected DevExpress.XtraGrid.GridControl gcPagos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvPagos;
        private System.Windows.Forms.Panel pnlFormaPago;
        private DevExpress.XtraEditors.TextEdit txtTotalFormaPago;
        private System.Windows.Forms.Label lbl_FormaPago;
        private System.Windows.Forms.Panel pnlPlanPago;
        private DevExpress.XtraEditors.TextEdit txtTotalComponentes;
        private System.Windows.Forms.Label lblPlanPago;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.TextEdit txtVlrAbonado;
        private System.Windows.Forms.Label lblVlrAbonado;
        private DevExpress.XtraEditors.TextEdit txtVlrSaldo;
        private System.Windows.Forms.Label lblVlrSaldo;
        private DevExpress.XtraEditors.TextEdit txtVlrDeuda;
        private System.Windows.Forms.Label lblVlrDeuda;
        private System.Windows.Forms.Button btnUpdValues;
        private System.Windows.Forms.Label lblDescriptivo;
        private System.Windows.Forms.TextBox txtDescriptivo;
    }
}