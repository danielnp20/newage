namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RecaudosManuales
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
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaConsigna = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaConsignacion = new System.Windows.Forms.Label();
            this.lblFechaAplica = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlFormaPago = new System.Windows.Forms.Panel();
            this.txtTotalFormaPago = new DevExpress.XtraEditors.TextEdit();
            this.lbl_FormaPago = new System.Windows.Forms.Label();
            this.pnlPlanPago = new System.Windows.Forms.Panel();
            this.txtTotalPlanPagos = new DevExpress.XtraEditors.TextEdit();
            this.lblPlanPago = new System.Windows.Forms.Label();
            this.plnTotalComponentes = new System.Windows.Forms.Panel();
            this.txtTotalComponentes = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalComponentes = new System.Windows.Forms.Label();
            this.masterBanco = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).BeginInit();
            this.pnlFormaPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).BeginInit();
            this.pnlPlanPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPlanPagos.Properties)).BeginInit();
            this.plnTotalComponentes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.tableLayoutPanel1);
            this.grpboxDetail.Location = new System.Drawing.Point(3, 4);
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
            this.gbGridDocument.Size = new System.Drawing.Size(820, 357);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 357);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.masterBanco);
            this.grpboxHeader.Controls.Add(this.lkp_Libranzas);
            this.grpboxHeader.Controls.Add(this.masterCaja);
            this.grpboxHeader.Controls.Add(this.dtFechaAplica);
            this.grpboxHeader.Controls.Add(this.dtFechaConsigna);
            this.grpboxHeader.Controls.Add(this.lblFechaConsignacion);
            this.grpboxHeader.Controls.Add(this.lblFechaAplica);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 24);
            this.grpboxHeader.Size = new System.Drawing.Size(1102, 599);
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
            // lkp_Libranzas
            // 
            this.lkp_Libranzas.Location = new System.Drawing.Point(107, 102);
            this.lkp_Libranzas.Name = "lkp_Libranzas";
            this.lkp_Libranzas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Libranzas.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NumeroDoc", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Descriptivo")});
            this.lkp_Libranzas.Properties.DisplayMember = "Libranza";
            this.lkp_Libranzas.Properties.NullText = " ";
            this.lkp_Libranzas.Properties.ValueMember = "NumeroDoc";
            this.lkp_Libranzas.Size = new System.Drawing.Size(117, 20);
            this.lkp_Libranzas.TabIndex = 8;
            this.lkp_Libranzas.Leave += new System.EventHandler(this.lkp_Libranzas_Leave);
            // 
            // masterCaja
            // 
            this.masterCaja.BackColor = System.Drawing.Color.Transparent;
            this.masterCaja.Filtros = null;
            this.masterCaja.Location = new System.Drawing.Point(7, 12);
            this.masterCaja.Name = "masterCaja";
            this.masterCaja.Size = new System.Drawing.Size(304, 25);
            this.masterCaja.TabIndex = 0;
            this.masterCaja.Value = "";
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(445, 44);
            this.dtFechaAplica.Name = "dtFechaAplica";
            this.dtFechaAplica.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaAplica.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaAplica.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaAplica.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaAplica.Properties.Appearance.Options.UseFont = true;
            this.dtFechaAplica.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaAplica.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaAplica.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaAplica.Size = new System.Drawing.Size(117, 20);
            this.dtFechaAplica.TabIndex = 3;
            // 
            // dtFechaConsigna
            // 
            this.dtFechaConsigna.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaConsigna.Location = new System.Drawing.Point(107, 45);
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
            this.lblFechaConsignacion.Location = new System.Drawing.Point(5, 48);
            this.lblFechaConsignacion.Name = "lblFechaConsignacion";
            this.lblFechaConsignacion.Size = new System.Drawing.Size(135, 13);
            this.lblFechaConsignacion.TabIndex = 4;
            this.lblFechaConsignacion.Text = "52563_FechaConsignacion";
            // 
            // lblFechaAplica
            // 
            this.lblFechaAplica.AutoSize = true;
            this.lblFechaAplica.Location = new System.Drawing.Point(342, 47);
            this.lblFechaAplica.Name = "lblFechaAplica";
            this.lblFechaAplica.Size = new System.Drawing.Size(100, 13);
            this.lblFechaAplica.TabIndex = 2;
            this.lblFechaAplica.Text = "32563_FechaAplica";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(4, 105);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(83, 13);
            this.lblLibranza.TabIndex = 7;
            this.lblLibranza.Text = "32563_Libranza";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(7, 72);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(305, 25);
            this.masterCliente.TabIndex = 6;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.67212F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.32788F));
            this.tableLayoutPanel1.Controls.Add(this.gcDetails, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gcPagos, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlFormaPago, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlPlanPago, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.plnTotalComponentes, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.82734F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.17266F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1111, 178);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gcDetails
            // 
            this.gcDetails.AllowDrop = true;
            this.gcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetails.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetails.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetails.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetails.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDetails.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetails.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDetails.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetails.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDetails_EmbeddedNavigator_ButtonClick);
            this.gcDetails.Location = new System.Drawing.Point(3, 26);
            this.gcDetails.LookAndFeel.SkinName = "Dark Side";
            this.gcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetails.MainView = this.gvDetails;
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.Size = new System.Drawing.Size(668, 119);
            this.gcDetails.TabIndex = 4;
            this.gcDetails.UseEmbeddedNavigator = true;
            this.gcDetails.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetails});
            // 
            // gvDetails
            // 
            this.gvDetails.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetails.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetails.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetails.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetails.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetails.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetails.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetails.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetails.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetails.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetails.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetails.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetails.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetails.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetails.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.Row.Options.UseBackColor = true;
            this.gvDetails.Appearance.Row.Options.UseForeColor = true;
            this.gvDetails.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetails.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetails.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetails.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetails.GridControl = this.gcDetails;
            this.gvDetails.Name = "gvDetails";
            this.gvDetails.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetails.OptionsCustomization.AllowFilter = false;
            this.gvDetails.OptionsCustomization.AllowSort = false;
            this.gvDetails.OptionsView.ShowGroupPanel = false;
            this.gvDetails.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetails_FocusedRowChanged);
            this.gvDetails.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetails_CellValueChanged);
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcPagos
            // 
            this.gcPagos.AllowDrop = true;
            this.gcPagos.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.gcPagos.Location = new System.Drawing.Point(677, 26);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvPagos;
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(431, 119);
            this.gcPagos.TabIndex = 5;
            this.gcPagos.UseEmbeddedNavigator = true;
            this.gcPagos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPagos});
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
            // pnlFormaPago
            // 
            this.pnlFormaPago.Controls.Add(this.txtTotalFormaPago);
            this.pnlFormaPago.Controls.Add(this.lbl_FormaPago);
            this.pnlFormaPago.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlFormaPago.Location = new System.Drawing.Point(814, 151);
            this.pnlFormaPago.Name = "pnlFormaPago";
            this.pnlFormaPago.Size = new System.Drawing.Size(294, 24);
            this.pnlFormaPago.TabIndex = 24;
            // 
            // txtTotalFormaPago
            // 
            this.txtTotalFormaPago.EditValue = "0";
            this.txtTotalFormaPago.Location = new System.Drawing.Point(169, 1);
            this.txtTotalFormaPago.Name = "txtTotalFormaPago";
            this.txtTotalFormaPago.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalFormaPago.Properties.Appearance.Options.UseFont = true;
            this.txtTotalFormaPago.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalFormaPago.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalFormaPago.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.EditMask = "c";
            this.txtTotalFormaPago.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalFormaPago.Properties.ReadOnly = true;
            this.txtTotalFormaPago.Size = new System.Drawing.Size(119, 20);
            this.txtTotalFormaPago.TabIndex = 24;
            // 
            // lbl_FormaPago
            // 
            this.lbl_FormaPago.AutoSize = true;
            this.lbl_FormaPago.Location = new System.Drawing.Point(76, 2);
            this.lbl_FormaPago.Name = "lbl_FormaPago";
            this.lbl_FormaPago.Size = new System.Drawing.Size(101, 13);
            this.lbl_FormaPago.TabIndex = 22;
            this.lbl_FormaPago.Text = "lbl_163_FormaPago";
            // 
            // pnlPlanPago
            // 
            this.pnlPlanPago.Controls.Add(this.txtTotalPlanPagos);
            this.pnlPlanPago.Controls.Add(this.lblPlanPago);
            this.pnlPlanPago.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPlanPago.Location = new System.Drawing.Point(814, 3);
            this.pnlPlanPago.Name = "pnlPlanPago";
            this.pnlPlanPago.Size = new System.Drawing.Size(294, 17);
            this.pnlPlanPago.TabIndex = 23;
            // 
            // txtTotalPlanPagos
            // 
            this.txtTotalPlanPagos.EditValue = "0";
            this.txtTotalPlanPagos.Location = new System.Drawing.Point(172, 0);
            this.txtTotalPlanPagos.Name = "txtTotalPlanPagos";
            this.txtTotalPlanPagos.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPlanPagos.Properties.Appearance.Options.UseFont = true;
            this.txtTotalPlanPagos.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalPlanPagos.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalPlanPagos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalPlanPagos.Properties.Mask.EditMask = "c";
            this.txtTotalPlanPagos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPlanPagos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalPlanPagos.Properties.ReadOnly = true;
            this.txtTotalPlanPagos.Size = new System.Drawing.Size(119, 20);
            this.txtTotalPlanPagos.TabIndex = 24;
            // 
            // lblPlanPago
            // 
            this.lblPlanPago.AutoSize = true;
            this.lblPlanPago.Location = new System.Drawing.Point(85, 2);
            this.lblPlanPago.Name = "lblPlanPago";
            this.lblPlanPago.Size = new System.Drawing.Size(91, 13);
            this.lblPlanPago.TabIndex = 22;
            this.lblPlanPago.Text = "lbl_163_PlanPago";
            // 
            // plnTotalComponentes
            // 
            this.plnTotalComponentes.Controls.Add(this.txtTotalComponentes);
            this.plnTotalComponentes.Controls.Add(this.lblTotalComponentes);
            this.plnTotalComponentes.Dock = System.Windows.Forms.DockStyle.Right;
            this.plnTotalComponentes.Location = new System.Drawing.Point(377, 151);
            this.plnTotalComponentes.Name = "plnTotalComponentes";
            this.plnTotalComponentes.Size = new System.Drawing.Size(294, 24);
            this.plnTotalComponentes.TabIndex = 25;
            // 
            // txtTotalComponentes
            // 
            this.txtTotalComponentes.EditValue = "0";
            this.txtTotalComponentes.Location = new System.Drawing.Point(175, 1);
            this.txtTotalComponentes.Name = "txtTotalComponentes";
            this.txtTotalComponentes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalComponentes.Properties.Appearance.Options.UseFont = true;
            this.txtTotalComponentes.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalComponentes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalComponentes.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalComponentes.Properties.Mask.EditMask = "c";
            this.txtTotalComponentes.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalComponentes.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalComponentes.Properties.ReadOnly = true;
            this.txtTotalComponentes.Size = new System.Drawing.Size(119, 20);
            this.txtTotalComponentes.TabIndex = 23;
            // 
            // lblTotalComponentes
            // 
            this.lblTotalComponentes.AutoSize = true;
            this.lblTotalComponentes.Location = new System.Drawing.Point(76, 2);
            this.lblTotalComponentes.Name = "lblTotalComponentes";
            this.lblTotalComponentes.Size = new System.Drawing.Size(137, 13);
            this.lblTotalComponentes.TabIndex = 22;
            this.lblTotalComponentes.Text = "lbl_163_TotalComponentes";
            // 
            // masterBanco
            // 
            this.masterBanco.BackColor = System.Drawing.Color.Transparent;
            this.masterBanco.Filtros = null;
            this.masterBanco.Location = new System.Drawing.Point(345, 12);
            this.masterBanco.Name = "masterBanco";
            this.masterBanco.Size = new System.Drawing.Size(304, 25);
            this.masterBanco.TabIndex = 1;
            this.masterBanco.Value = "";
            // 
            // RecaudosManuales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 724);
            this.Name = "RecaudosManuales";
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).EndInit();
            this.pnlFormaPago.ResumeLayout(false);
            this.pnlFormaPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).EndInit();
            this.pnlPlanPago.ResumeLayout(false);
            this.pnlPlanPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPlanPagos.Properties)).EndInit();
            this.plnTotalComponentes.ResumeLayout(false);
            this.plnTotalComponentes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkp_Libranzas;
        private ControlsUC.uc_MasterFind masterCaja;
        protected DevExpress.XtraEditors.DateEdit dtFechaAplica;
        protected DevExpress.XtraEditors.DateEdit dtFechaConsigna;
        private System.Windows.Forms.Label lblFechaConsignacion;
        private System.Windows.Forms.Label lblFechaAplica;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected DevExpress.XtraGrid.GridControl gcPagos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvPagos;
        protected DevExpress.XtraGrid.GridControl gcDetails;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        private System.Windows.Forms.Panel pnlFormaPago;
        private System.Windows.Forms.Label lbl_FormaPago;
        private System.Windows.Forms.Panel pnlPlanPago;
        private System.Windows.Forms.Label lblPlanPago;
        private System.Windows.Forms.Panel plnTotalComponentes;
        private System.Windows.Forms.Label lblTotalComponentes;
        private DevExpress.XtraEditors.TextEdit txtTotalFormaPago;
        private DevExpress.XtraEditors.TextEdit txtTotalComponentes;
        private DevExpress.XtraEditors.TextEdit txtTotalPlanPagos;
        private ControlsUC.uc_MasterFind masterBanco;


    }
}