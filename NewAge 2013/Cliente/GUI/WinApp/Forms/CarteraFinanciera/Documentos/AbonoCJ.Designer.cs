namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AbonoCJ
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.lblVlrAPagar = new System.Windows.Forms.Label();
            this.txtVlrAPagar = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVlrPendOtros = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVlrPendPol = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVlrPendCap = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrPoliza = new System.Windows.Forms.Label();
            this.txtVlrPend = new DevExpress.XtraEditors.TextEdit();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.masterBanco = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lkp_Libranzas = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCaja = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaConsigna = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaConsignacion = new System.Windows.Forms.Label();
            this.lblFechaAplica = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbClaseDeuda = new DevExpress.XtraEditors.LookUpEdit();
            this.lblClaseDeuda = new DevExpress.XtraEditors.LabelControl();
            this.lblEstadoActual = new DevExpress.XtraEditors.LabelControl();
            this.dtPeriodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTotalFormaPago = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalComponentes = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAPagar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendOtros.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendPol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendCap.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClaseDeuda.Properties)).BeginInit();
            this.pnlGrids.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin,
            this.editSpin4,
            this.editValue});
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Controls.Add(this.panel1, 1, 2);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(0, 0);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.3663F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.6337F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1201, 620);
            this.tlSeparatorPanel.TabIndex = 63;
            // 
            // grpctrlHeader
            // 
            this.grpctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grpctrlHeader.Appearance.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.grpctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.grpctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.grpctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.grpctrlHeader.Controls.Add(this.grpboxHeader);
            this.grpctrlHeader.Controls.Add(this.dtPeriodo);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(12, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1178, 267);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Controls.Add(this.lblVlrAPagar);
            this.grpboxHeader.Controls.Add(this.txtVlrAPagar);
            this.grpboxHeader.Controls.Add(this.label3);
            this.grpboxHeader.Controls.Add(this.txtVlrPendOtros);
            this.grpboxHeader.Controls.Add(this.label2);
            this.grpboxHeader.Controls.Add(this.txtVlrPendPol);
            this.grpboxHeader.Controls.Add(this.label1);
            this.grpboxHeader.Controls.Add(this.txtVlrPendCap);
            this.grpboxHeader.Controls.Add(this.lblVlrPoliza);
            this.grpboxHeader.Controls.Add(this.txtVlrPend);
            this.grpboxHeader.Controls.Add(this.txtEstado);
            this.grpboxHeader.Controls.Add(this.masterBanco);
            this.grpboxHeader.Controls.Add(this.lkp_Libranzas);
            this.grpboxHeader.Controls.Add(this.masterCaja);
            this.grpboxHeader.Controls.Add(this.dtFechaAplica);
            this.grpboxHeader.Controls.Add(this.dtFechaConsigna);
            this.grpboxHeader.Controls.Add(this.lblFechaConsignacion);
            this.grpboxHeader.Controls.Add(this.lblFechaAplica);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Controls.Add(this.cmbClaseDeuda);
            this.grpboxHeader.Controls.Add(this.lblClaseDeuda);
            this.grpboxHeader.Controls.Add(this.lblEstadoActual);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(1174, 243);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // lblVlrAPagar
            // 
            this.lblVlrAPagar.AutoSize = true;
            this.lblVlrAPagar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrAPagar.Location = new System.Drawing.Point(21, 214);
            this.lblVlrAPagar.Name = "lblVlrAPagar";
            this.lblVlrAPagar.Size = new System.Drawing.Size(87, 14);
            this.lblVlrAPagar.TabIndex = 95;
            this.lblVlrAPagar.Text = "167_VlrAPagar";
            // 
            // txtVlrAPagar
            // 
            this.txtVlrAPagar.EditValue = "0";
            this.txtVlrAPagar.Location = new System.Drawing.Point(157, 212);
            this.txtVlrAPagar.Name = "txtVlrAPagar";
            this.txtVlrAPagar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrAPagar.Properties.Appearance.Options.UseFont = true;
            this.txtVlrAPagar.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrAPagar.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrAPagar.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrAPagar.Properties.Mask.EditMask = "c";
            this.txtVlrAPagar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrAPagar.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrAPagar.Size = new System.Drawing.Size(97, 20);
            this.txtVlrAPagar.TabIndex = 12;
            this.txtVlrAPagar.Leave += new System.EventHandler(this.txtVlrAPagar_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(653, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 14);
            this.label3.TabIndex = 93;
            this.label3.Text = "167_VlrPendienteOtros";
            // 
            // txtVlrPendOtros
            // 
            this.txtVlrPendOtros.EditValue = "0";
            this.txtVlrPendOtros.Enabled = false;
            this.txtVlrPendOtros.Location = new System.Drawing.Point(782, 179);
            this.txtVlrPendOtros.Name = "txtVlrPendOtros";
            this.txtVlrPendOtros.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrPendOtros.Properties.Appearance.Options.UseFont = true;
            this.txtVlrPendOtros.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrPendOtros.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrPendOtros.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrPendOtros.Properties.Mask.EditMask = "c";
            this.txtVlrPendOtros.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrPendOtros.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrPendOtros.Size = new System.Drawing.Size(99, 20);
            this.txtVlrPendOtros.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(342, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 14);
            this.label2.TabIndex = 91;
            this.label2.Text = "167_VlrPendientePoliza";
            // 
            // txtVlrPendPol
            // 
            this.txtVlrPendPol.EditValue = "0";
            this.txtVlrPendPol.Enabled = false;
            this.txtVlrPendPol.Location = new System.Drawing.Point(481, 179);
            this.txtVlrPendPol.Name = "txtVlrPendPol";
            this.txtVlrPendPol.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrPendPol.Properties.Appearance.Options.UseFont = true;
            this.txtVlrPendPol.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrPendPol.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrPendPol.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrPendPol.Properties.Mask.EditMask = "c";
            this.txtVlrPendPol.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrPendPol.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrPendPol.Size = new System.Drawing.Size(105, 20);
            this.txtVlrPendPol.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 14);
            this.label1.TabIndex = 89;
            this.label1.Text = "167_VlrPendienteCapital";
            // 
            // txtVlrPendCap
            // 
            this.txtVlrPendCap.EditValue = "0";
            this.txtVlrPendCap.Enabled = false;
            this.txtVlrPendCap.Location = new System.Drawing.Point(157, 176);
            this.txtVlrPendCap.Name = "txtVlrPendCap";
            this.txtVlrPendCap.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrPendCap.Properties.Appearance.Options.UseFont = true;
            this.txtVlrPendCap.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrPendCap.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrPendCap.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrPendCap.Properties.Mask.EditMask = "c";
            this.txtVlrPendCap.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrPendCap.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrPendCap.Size = new System.Drawing.Size(97, 20);
            this.txtVlrPendCap.TabIndex = 9;
            // 
            // lblVlrPoliza
            // 
            this.lblVlrPoliza.AutoSize = true;
            this.lblVlrPoliza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrPoliza.Location = new System.Drawing.Point(341, 148);
            this.lblVlrPoliza.Name = "lblVlrPoliza";
            this.lblVlrPoliza.Size = new System.Drawing.Size(105, 14);
            this.lblVlrPoliza.TabIndex = 87;
            this.lblVlrPoliza.Text = "167_VlrPendiente";
            // 
            // txtVlrPend
            // 
            this.txtVlrPend.EditValue = "0";
            this.txtVlrPend.Enabled = false;
            this.txtVlrPend.Location = new System.Drawing.Point(449, 145);
            this.txtVlrPend.Name = "txtVlrPend";
            this.txtVlrPend.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrPend.Properties.Appearance.Options.UseFont = true;
            this.txtVlrPend.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrPend.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrPend.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrPend.Properties.Mask.EditMask = "c";
            this.txtVlrPend.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrPend.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrPend.Size = new System.Drawing.Size(91, 20);
            this.txtVlrPend.TabIndex = 8;
            // 
            // txtEstado
            // 
            this.txtEstado.Enabled = false;
            this.txtEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstado.Location = new System.Drawing.Point(449, 114);
            this.txtEstado.Multiline = true;
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.Size = new System.Drawing.Size(117, 19);
            this.txtEstado.TabIndex = 6;
            // 
            // masterBanco
            // 
            this.masterBanco.BackColor = System.Drawing.Color.Transparent;
            this.masterBanco.Filtros = null;
            this.masterBanco.Location = new System.Drawing.Point(344, 20);
            this.masterBanco.Name = "masterBanco";
            this.masterBanco.Size = new System.Drawing.Size(304, 25);
            this.masterBanco.TabIndex = 78;
            this.masterBanco.Value = "";
            // 
            // lkp_Libranzas
            // 
            this.lkp_Libranzas.Location = new System.Drawing.Point(123, 115);
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
            this.lkp_Libranzas.TabIndex = 5;
            this.lkp_Libranzas.Leave += new System.EventHandler(this.lkp_Libranzas_Leave);
            // 
            // masterCaja
            // 
            this.masterCaja.BackColor = System.Drawing.Color.Transparent;
            this.masterCaja.Filtros = null;
            this.masterCaja.Location = new System.Drawing.Point(24, 20);
            this.masterCaja.Name = "masterCaja";
            this.masterCaja.Size = new System.Drawing.Size(304, 25);
            this.masterCaja.TabIndex = 1;
            this.masterCaja.Value = "";
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(123, 53);
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
            this.dtFechaAplica.TabIndex = 2;
            this.dtFechaAplica.DateTimeChanged += new System.EventHandler(this.dtFechaAplica_DateTimeChanged);
            // 
            // dtFechaConsigna
            // 
            this.dtFechaConsigna.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaConsigna.Location = new System.Drawing.Point(449, 53);
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
            this.dtFechaConsigna.TabIndex = 3;
            this.dtFechaConsigna.DateTimeChanged += new System.EventHandler(this.dtFechaConsigna_DateTimeChanged);
            // 
            // lblFechaConsignacion
            // 
            this.lblFechaConsignacion.AutoSize = true;
            this.lblFechaConsignacion.Location = new System.Drawing.Point(341, 56);
            this.lblFechaConsignacion.Name = "lblFechaConsignacion";
            this.lblFechaConsignacion.Size = new System.Drawing.Size(135, 13);
            this.lblFechaConsignacion.TabIndex = 81;
            this.lblFechaConsignacion.Text = "52563_FechaConsignacion";
            // 
            // lblFechaAplica
            // 
            this.lblFechaAplica.AutoSize = true;
            this.lblFechaAplica.Location = new System.Drawing.Point(21, 56);
            this.lblFechaAplica.Name = "lblFechaAplica";
            this.lblFechaAplica.Size = new System.Drawing.Size(100, 13);
            this.lblFechaAplica.TabIndex = 79;
            this.lblFechaAplica.Text = "32563_FechaAplica";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(21, 118);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(83, 13);
            this.lblLibranza.TabIndex = 84;
            this.lblLibranza.Text = "32563_Libranza";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(24, 81);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(305, 25);
            this.masterCliente.TabIndex = 4;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // cmbClaseDeuda
            // 
            this.cmbClaseDeuda.Location = new System.Drawing.Point(123, 146);
            this.cmbClaseDeuda.Name = "cmbClaseDeuda";
            this.cmbClaseDeuda.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbClaseDeuda.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbClaseDeuda.Properties.DisplayMember = "Value";
            this.cmbClaseDeuda.Properties.NullText = " ";
            this.cmbClaseDeuda.Properties.ValueMember = "Key";
            this.cmbClaseDeuda.Size = new System.Drawing.Size(117, 20);
            this.cmbClaseDeuda.TabIndex = 7;
            this.cmbClaseDeuda.EditValueChanged += new System.EventHandler(this.cmbClaseDeuda_EditValueChanged);
            // 
            // lblClaseDeuda
            // 
            this.lblClaseDeuda.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClaseDeuda.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblClaseDeuda.Location = new System.Drawing.Point(24, 148);
            this.lblClaseDeuda.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblClaseDeuda.Name = "lblClaseDeuda";
            this.lblClaseDeuda.Size = new System.Drawing.Size(101, 14);
            this.lblClaseDeuda.TabIndex = 76;
            this.lblClaseDeuda.Text = "177_lblClaseDeuda";
            // 
            // lblEstadoActual
            // 
            this.lblEstadoActual.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoActual.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblEstadoActual.Location = new System.Drawing.Point(344, 117);
            this.lblEstadoActual.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblEstadoActual.Name = "lblEstadoActual";
            this.lblEstadoActual.Size = new System.Drawing.Size(110, 14);
            this.lblEstadoActual.TabIndex = 68;
            this.lblEstadoActual.Text = "177_lblEstadoActual";
            // 
            // dtPeriodo
            // 
            this.dtPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriodo.EnabledControl = true;
            this.dtPeriodo.ExtraPeriods = 0;
            this.dtPeriodo.Location = new System.Drawing.Point(384, 1);
            this.dtPeriodo.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriodo.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.Name = "dtPeriodo";
            this.dtPeriodo.Size = new System.Drawing.Size(130, 18);
            this.dtPeriodo.TabIndex = 3;
            // 
            // lblBreak
            // 
            this.lblBreak.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreak.Location = new System.Drawing.Point(67, 4);
            this.lblBreak.Margin = new System.Windows.Forms.Padding(4);
            this.lblBreak.Name = "lblBreak";
            this.lblBreak.Size = new System.Drawing.Size(5, 13);
            this.lblBreak.TabIndex = 7;
            this.lblBreak.Text = "-";
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Enabled = false;
            this.txtDocDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocDesc.Location = new System.Drawing.Point(75, 1);
            this.txtDocDesc.Multiline = true;
            this.txtDocDesc.Name = "txtDocDesc";
            this.txtDocDesc.Size = new System.Drawing.Size(217, 19);
            this.txtDocDesc.TabIndex = 1;
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Enabled = false;
            this.txtDocumentoID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoID.Location = new System.Drawing.Point(7, 1);
            this.txtDocumentoID.Multiline = true;
            this.txtDocumentoID.Name = "txtDocumentoID";
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 19);
            this.txtDocumentoID.TabIndex = 0;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(322, 4);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(93, 14);
            this.lblPeriod.TabIndex = 82;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gbGridDocument);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(12, 276);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1178, 264);
            this.pnlGrids.TabIndex = 113;
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.tableLayoutPanel1);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(0, 0);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbGridDocument.Size = new System.Drawing.Size(1178, 264);
            this.gbGridDocument.TabIndex = 54;
            this.gbGridDocument.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.9043F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0957F));
            this.tableLayoutPanel1.Controls.Add(this.gcPagos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gcDetails, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1166, 248);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.gcPagos.Location = new System.Drawing.Point(701, 3);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvPagos;
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(462, 242);
            this.gcPagos.TabIndex = 6;
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
            this.gcDetails.Location = new System.Drawing.Point(3, 3);
            this.gcDetails.LookAndFeel.SkinName = "Dark Side";
            this.gcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetails.MainView = this.gvDetails;
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.Size = new System.Drawing.Size(692, 242);
            this.gcDetails.TabIndex = 5;
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
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTotalFormaPago);
            this.panel1.Controls.Add(this.txtTotalComponentes);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(12, 546);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1178, 71);
            this.panel1.TabIndex = 114;
            // 
            // txtTotalFormaPago
            // 
            this.txtTotalFormaPago.EditValue = "0";
            this.txtTotalFormaPago.Enabled = false;
            this.txtTotalFormaPago.Location = new System.Drawing.Point(934, 6);
            this.txtTotalFormaPago.Name = "txtTotalFormaPago";
            this.txtTotalFormaPago.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalFormaPago.Properties.Appearance.Options.UseFont = true;
            this.txtTotalFormaPago.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalFormaPago.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalFormaPago.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.EditMask = "c";
            this.txtTotalFormaPago.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalFormaPago.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalFormaPago.Size = new System.Drawing.Size(105, 20);
            this.txtTotalFormaPago.TabIndex = 98;
            // 
            // txtTotalComponentes
            // 
            this.txtTotalComponentes.EditValue = "0";
            this.txtTotalComponentes.Enabled = false;
            this.txtTotalComponentes.Location = new System.Drawing.Point(513, 6);
            this.txtTotalComponentes.Name = "txtTotalComponentes";
            this.txtTotalComponentes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalComponentes.Properties.Appearance.Options.UseFont = true;
            this.txtTotalComponentes.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalComponentes.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalComponentes.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalComponentes.Properties.Mask.EditMask = "c";
            this.txtTotalComponentes.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalComponentes.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalComponentes.Size = new System.Drawing.Size(105, 20);
            this.txtTotalComponentes.TabIndex = 96;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(847, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 14);
            this.label5.TabIndex = 97;
            this.label5.Text = "1005_lblTotal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(426, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 14);
            this.label4.TabIndex = 96;
            this.label4.Text = "1005_lblTotal";
            // 
            // AbonoCJ
            // 
            this.ClientSize = new System.Drawing.Size(1201, 620);
            this.Controls.Add(this.tlSeparatorPanel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AbonoCJ";
            this.Text = "32564";
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAPagar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendOtros.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendPol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPendCap.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrPend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsigna.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClaseDeuda.Properties)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalFormaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalComponentes.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.Panel pnlGrids;
        private System.Windows.Forms.GroupBox gbGridDocument;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        private DevExpress.XtraEditors.LabelControl lblEstadoActual;
        private ControlsUC.uc_PeriodoEdit dtPeriodo;
        private DevExpress.XtraEditors.LabelControl lblBreak;
        private System.Windows.Forms.TextBox txtDocDesc;
        private System.Windows.Forms.TextBox txtDocumentoID;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraEditors.LookUpEdit cmbClaseDeuda;
        private DevExpress.XtraEditors.LabelControl lblClaseDeuda;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ControlsUC.uc_MasterFind masterBanco;
        private DevExpress.XtraEditors.LookUpEdit lkp_Libranzas;
        private ControlsUC.uc_MasterFind masterCaja;
        private DevExpress.XtraEditors.DateEdit dtFechaAplica;
        private DevExpress.XtraEditors.DateEdit dtFechaConsigna;
        private System.Windows.Forms.Label lblFechaConsignacion;
        private System.Windows.Forms.Label lblFechaAplica;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraGrid.GridControl gcDetails;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        private System.Windows.Forms.TextBox txtEstado;
        private DevExpress.XtraGrid.GridControl gcPagos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPagos;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtVlrPendOtros;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtVlrPendPol;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtVlrPendCap;
        private System.Windows.Forms.Label lblVlrPoliza;
        private DevExpress.XtraEditors.TextEdit txtVlrPend;
        private System.Windows.Forms.Label lblVlrAPagar;
        private DevExpress.XtraEditors.TextEdit txtVlrAPagar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtTotalComponentes;
        private DevExpress.XtraEditors.TextEdit txtTotalFormaPago;
    }
}