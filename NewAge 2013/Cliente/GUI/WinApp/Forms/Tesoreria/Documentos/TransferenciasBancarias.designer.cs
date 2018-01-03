using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TransferenciasBancarias
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferenciasBancarias));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.scrollControl = new DevExpress.XtraEditors.XtraScrollableControl();
            this.gctrlGrid = new DevExpress.XtraEditors.GroupControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblComp = new DevExpress.XtraEditors.LabelControl();
            this.lblDetail = new DevExpress.XtraEditors.LabelControl();
            this.lblDetailTit = new DevExpress.XtraEditors.LabelControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.lblCompTit = new DevExpress.XtraEditors.LabelControl();
            this.lblDateTit = new DevExpress.XtraEditors.LabelControl();
            this.lblBeneficiario = new DevExpress.XtraEditors.LabelControl();
            this.uc_Beneficiario = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.txtSumatoriaTercero = new DevExpress.XtraEditors.TextEdit();
            this.lblSumatoriaTercero = new DevExpress.XtraEditors.LabelControl();
            this.txtSumatoriaCuenta = new DevExpress.XtraEditors.TextEdit();
            this.lblSumatoriaCuenta = new DevExpress.XtraEditors.LabelControl();
            this.txtSumatoriaTotal = new DevExpress.XtraEditors.TextEdit();
            this.lblSumatoriaTotal = new DevExpress.XtraEditors.LabelControl();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblMonedaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.cmbMonedaOrigen = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).BeginInit();
            this.gctrlHeader.SuspendLayout();
            this.scrollControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).BeginInit();
            this.gctrlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaTercero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1126, 603);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 1;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 0, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.470385F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.05923F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.470385F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1122, 599);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gctrlHeader);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(3, 23);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1116, 551);
            this.pnlGrids.TabIndex = 113;
            // 
            // gctrlHeader
            // 
            this.gctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlHeader.Appearance.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.gctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.gctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gctrlHeader.Controls.Add(this.scrollControl);
            this.gctrlHeader.Controls.Add(this.chkSelectAll);
            this.gctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlHeader.Location = new System.Drawing.Point(0, 0);
            this.gctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlHeader.Name = "gctrlHeader";
            this.gctrlHeader.Size = new System.Drawing.Size(1116, 551);
            this.gctrlHeader.TabIndex = 0;
            // 
            // scrollControl
            // 
            this.scrollControl.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.scrollControl.Appearance.Options.UseBackColor = true;
            this.scrollControl.Controls.Add(this.gctrlGrid);
            this.scrollControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollControl.Location = new System.Drawing.Point(2, 22);
            this.scrollControl.Margin = new System.Windows.Forms.Padding(2);
            this.scrollControl.Name = "scrollControl";
            this.scrollControl.Size = new System.Drawing.Size(1112, 527);
            this.scrollControl.TabIndex = 35;
            // 
            // gctrlGrid
            // 
            this.gctrlGrid.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gctrlGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gctrlGrid.Appearance.Options.UseBackColor = true;
            this.gctrlGrid.Appearance.Options.UseBorderColor = true;
            this.gctrlGrid.Appearance.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gctrlGrid.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.gctrlGrid.AppearanceCaption.Options.UseBorderColor = true;
            this.gctrlGrid.AppearanceCaption.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.Options.UseForeColor = true;
            this.gctrlGrid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gctrlGrid.Controls.Add(this.dtFecha);
            this.gctrlGrid.Controls.Add(this.label5);
            this.gctrlGrid.Controls.Add(this.groupControl1);
            this.gctrlGrid.Controls.Add(this.lblBeneficiario);
            this.gctrlGrid.Controls.Add(this.uc_Beneficiario);
            this.gctrlGrid.Controls.Add(this.dtPeriod);
            this.gctrlGrid.Controls.Add(this.lblPeriod);
            this.gctrlGrid.Controls.Add(this.txtSumatoriaTercero);
            this.gctrlGrid.Controls.Add(this.lblSumatoriaTercero);
            this.gctrlGrid.Controls.Add(this.txtSumatoriaCuenta);
            this.gctrlGrid.Controls.Add(this.lblSumatoriaCuenta);
            this.gctrlGrid.Controls.Add(this.txtSumatoriaTotal);
            this.gctrlGrid.Controls.Add(this.lblSumatoriaTotal);
            this.gctrlGrid.Controls.Add(this.masterCuenta);
            this.gctrlGrid.Controls.Add(this.lblMonedaOrigen);
            this.gctrlGrid.Controls.Add(this.cmbMonedaOrigen);
            this.gctrlGrid.Controls.Add(this.gcPagos);
            this.gctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlGrid.Location = new System.Drawing.Point(0, 0);
            this.gctrlGrid.LookAndFeel.SkinName = "Seven Classic";
            this.gctrlGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlGrid.Name = "gctrlGrid";
            this.gctrlGrid.ShowCaption = false;
            this.gctrlGrid.Size = new System.Drawing.Size(1112, 527);
            this.gctrlGrid.TabIndex = 35;
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(333, 336);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 131;
            this.dtFecha.EditValueChanged += new System.EventHandler(this.dtFecha_EditValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(263, 340);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 130;
            this.label5.Text = "31_lblFecha";
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.groupControl1.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Appearance.Options.UseBorderColor = true;
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.groupControl1.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.groupControl1.Controls.Add(this.lblComp);
            this.groupControl1.Controls.Add(this.lblDetail);
            this.groupControl1.Controls.Add(this.lblDetailTit);
            this.groupControl1.Controls.Add(this.lblDate);
            this.groupControl1.Controls.Add(this.lblCompTit);
            this.groupControl1.Controls.Add(this.lblDateTit);
            this.groupControl1.Location = new System.Drawing.Point(2, 434);
            this.groupControl1.LookAndFeel.SkinName = "The Asphalt World";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1108, 88);
            this.groupControl1.TabIndex = 129;
            // 
            // lblComp
            // 
            this.lblComp.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComp.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblComp.Location = new System.Drawing.Point(205, 38);
            this.lblComp.Margin = new System.Windows.Forms.Padding(4);
            this.lblComp.Name = "lblComp";
            this.lblComp.Size = new System.Drawing.Size(14, 14);
            this.lblComp.TabIndex = 102;
            this.lblComp.Text = "03";
            // 
            // lblDetail
            // 
            this.lblDetail.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetail.Location = new System.Drawing.Point(371, 38);
            this.lblDetail.Margin = new System.Windows.Forms.Padding(4);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(656, 14);
            this.lblDetail.TabIndex = 99;
            this.lblDetail.Text = "Descripcion de cada registro de la grilla / Descripcion de cada registro de la gr" +
    "illa / Descripcion de cada registro de la grilla";
            // 
            // lblDetailTit
            // 
            this.lblDetailTit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailTit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDetailTit.Location = new System.Drawing.Point(370, 5);
            this.lblDetailTit.Margin = new System.Windows.Forms.Padding(4);
            this.lblDetailTit.Name = "lblDetailTit";
            this.lblDetailTit.Size = new System.Drawing.Size(104, 14);
            this.lblDetailTit.TabIndex = 98;
            this.lblDetailTit.Text = "22501_lblDetalle";
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(24, 38);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(59, 14);
            this.lblDate.TabIndex = 55;
            this.lblDate.Text = "9/21/2011";
            // 
            // lblCompTit
            // 
            this.lblCompTit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompTit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblCompTit.Location = new System.Drawing.Point(205, 5);
            this.lblCompTit.Margin = new System.Windows.Forms.Padding(4);
            this.lblCompTit.Name = "lblCompTit";
            this.lblCompTit.Size = new System.Drawing.Size(146, 14);
            this.lblCompTit.TabIndex = 97;
            this.lblCompTit.Text = "22501_lblComprobante";
            // 
            // lblDateTit
            // 
            this.lblDateTit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDateTit.Location = new System.Drawing.Point(22, 5);
            this.lblDateTit.Margin = new System.Windows.Forms.Padding(4);
            this.lblDateTit.Name = "lblDateTit";
            this.lblDateTit.Size = new System.Drawing.Size(157, 14);
            this.lblDateTit.TabIndex = 96;
            this.lblDateTit.Text = "22501_lblFechaCausacion";
            // 
            // lblBeneficiario
            // 
            this.lblBeneficiario.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeneficiario.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBeneficiario.Location = new System.Drawing.Point(691, 339);
            this.lblBeneficiario.Margin = new System.Windows.Forms.Padding(4);
            this.lblBeneficiario.Name = "lblBeneficiario";
            this.lblBeneficiario.Size = new System.Drawing.Size(114, 14);
            this.lblBeneficiario.TabIndex = 128;
            this.lblBeneficiario.Text = "22501_lblBeneficiario";
            // 
            // uc_Beneficiario
            // 
            this.uc_Beneficiario.BackColor = System.Drawing.Color.Transparent;
            this.uc_Beneficiario.Filtros = null;
            this.uc_Beneficiario.Location = new System.Drawing.Point(712, 334);
            this.uc_Beneficiario.Margin = new System.Windows.Forms.Padding(4);
            this.uc_Beneficiario.Name = "uc_Beneficiario";
            this.uc_Beneficiario.Size = new System.Drawing.Size(291, 25);
            this.uc_Beneficiario.TabIndex = 2;
            this.uc_Beneficiario.Value = "";
            this.uc_Beneficiario.Leave += new System.EventHandler(this.uc_Beneficiario_Leave);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.Enabled = false;
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(127, 337);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 125;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(26, 339);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(80, 14);
            this.lblPeriod.TabIndex = 126;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // txtSumatoriaTercero
            // 
            this.txtSumatoriaTercero.EditValue = "0";
            this.txtSumatoriaTercero.Location = new System.Drawing.Point(127, 371);
            this.txtSumatoriaTercero.Name = "txtSumatoriaTercero";
            this.txtSumatoriaTercero.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSumatoriaTercero.Properties.Appearance.Options.UseFont = true;
            this.txtSumatoriaTercero.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumatoriaTercero.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumatoriaTercero.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumatoriaTercero.Properties.Mask.EditMask = "c";
            this.txtSumatoriaTercero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSumatoriaTercero.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSumatoriaTercero.Properties.ReadOnly = true;
            this.txtSumatoriaTercero.Size = new System.Drawing.Size(98, 20);
            this.txtSumatoriaTercero.TabIndex = 105;
            // 
            // lblSumatoriaTercero
            // 
            this.lblSumatoriaTercero.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumatoriaTercero.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblSumatoriaTercero.Location = new System.Drawing.Point(26, 374);
            this.lblSumatoriaTercero.Margin = new System.Windows.Forms.Padding(4);
            this.lblSumatoriaTercero.Name = "lblSumatoriaTercero";
            this.lblSumatoriaTercero.Size = new System.Drawing.Size(150, 14);
            this.lblSumatoriaTercero.TabIndex = 3;
            this.lblSumatoriaTercero.Text = "22501_lblSumatoriaTercero";
            // 
            // txtSumatoriaCuenta
            // 
            this.txtSumatoriaCuenta.EditValue = "0";
            this.txtSumatoriaCuenta.Location = new System.Drawing.Point(333, 372);
            this.txtSumatoriaCuenta.Name = "txtSumatoriaCuenta";
            this.txtSumatoriaCuenta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSumatoriaCuenta.Properties.Appearance.Options.UseFont = true;
            this.txtSumatoriaCuenta.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumatoriaCuenta.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumatoriaCuenta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumatoriaCuenta.Properties.Mask.EditMask = "c";
            this.txtSumatoriaCuenta.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSumatoriaCuenta.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSumatoriaCuenta.Properties.ReadOnly = true;
            this.txtSumatoriaCuenta.Size = new System.Drawing.Size(120, 20);
            this.txtSumatoriaCuenta.TabIndex = 103;
            // 
            // lblSumatoriaCuenta
            // 
            this.lblSumatoriaCuenta.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumatoriaCuenta.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblSumatoriaCuenta.Location = new System.Drawing.Point(233, 375);
            this.lblSumatoriaCuenta.Margin = new System.Windows.Forms.Padding(4);
            this.lblSumatoriaCuenta.Name = "lblSumatoriaCuenta";
            this.lblSumatoriaCuenta.Size = new System.Drawing.Size(145, 14);
            this.lblSumatoriaCuenta.TabIndex = 4;
            this.lblSumatoriaCuenta.Text = "22501_lblSumatoriaBancos";
            // 
            // txtSumatoriaTotal
            // 
            this.txtSumatoriaTotal.EditValue = "0";
            this.txtSumatoriaTotal.Location = new System.Drawing.Point(559, 372);
            this.txtSumatoriaTotal.Name = "txtSumatoriaTotal";
            this.txtSumatoriaTotal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtSumatoriaTotal.Properties.Appearance.Options.UseFont = true;
            this.txtSumatoriaTotal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSumatoriaTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSumatoriaTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSumatoriaTotal.Properties.Mask.EditMask = "c";
            this.txtSumatoriaTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSumatoriaTotal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSumatoriaTotal.Properties.ReadOnly = true;
            this.txtSumatoriaTotal.Size = new System.Drawing.Size(123, 20);
            this.txtSumatoriaTotal.TabIndex = 101;
            // 
            // lblSumatoriaTotal
            // 
            this.lblSumatoriaTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSumatoriaTotal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblSumatoriaTotal.Location = new System.Drawing.Point(456, 375);
            this.lblSumatoriaTotal.Margin = new System.Windows.Forms.Padding(4);
            this.lblSumatoriaTotal.Name = "lblSumatoriaTotal";
            this.lblSumatoriaTotal.Size = new System.Drawing.Size(135, 14);
            this.lblSumatoriaTotal.TabIndex = 5;
            this.lblSumatoriaTotal.Text = "22501_lblSumatoriaTotal";
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(6, 10);
            this.masterCuenta.Margin = new System.Windows.Forms.Padding(4);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(291, 25);
            this.masterCuenta.TabIndex = 0;
            this.masterCuenta.Value = "";
            this.masterCuenta.Leave += new System.EventHandler(this.masterCuenta_Leave);
            // 
            // lblMonedaOrigen
            // 
            this.lblMonedaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblMonedaOrigen.Location = new System.Drawing.Point(448, 339);
            this.lblMonedaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblMonedaOrigen.Name = "lblMonedaOrigen";
            this.lblMonedaOrigen.Size = new System.Drawing.Size(132, 14);
            this.lblMonedaOrigen.TabIndex = 53;
            this.lblMonedaOrigen.Text = "22501_lblMonedaOrigen";
            // 
            // cmbMonedaOrigen
            // 
            this.cmbMonedaOrigen.BackColor = System.Drawing.Color.White;
            this.cmbMonedaOrigen.FormattingEnabled = true;
            this.cmbMonedaOrigen.Location = new System.Drawing.Point(542, 336);
            this.cmbMonedaOrigen.Name = "cmbMonedaOrigen";
            this.cmbMonedaOrigen.Size = new System.Drawing.Size(142, 21);
            this.cmbMonedaOrigen.TabIndex = 1;
            this.cmbMonedaOrigen.SelectedIndexChanged += new System.EventHandler(this.cmbMonedaOrigen_SelectedIndexChanged);
            // 
            // gcPagos
            // 
            this.gcPagos.AllowDrop = true;
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
            this.gcPagos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcPagos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcPagos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcPagos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPagos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPagos.Location = new System.Drawing.Point(2, 43);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvPagos;
            this.gcPagos.Margin = new System.Windows.Forms.Padding(4);
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(1108, 283);
            this.gcPagos.TabIndex = 51;
            this.gcPagos.UseEmbeddedNavigator = true;
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
            this.gvPagos.HorzScrollStep = 50;
            this.gvPagos.Name = "gvPagos";
            this.gvPagos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvPagos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvPagos.OptionsCustomization.AllowColumnMoving = false;
            this.gvPagos.OptionsCustomization.AllowFilter = false;
            this.gvPagos.OptionsCustomization.AllowSort = false;
            this.gvPagos.OptionsMenu.EnableColumnMenu = false;
            this.gvPagos.OptionsMenu.EnableFooterMenu = false;
            this.gvPagos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPagos.OptionsView.ColumnAutoWidth = false;
            this.gvPagos.OptionsView.ShowAutoFilterRow = true;
            this.gvPagos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvPagos.OptionsView.ShowGroupPanel = false;
            this.gvPagos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvPagos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvPagos_CustomRowCellEdit);
            this.gvPagos.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvPagos_ShowingEditor);
            this.gvPagos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPagos_FocusedRowChanged);
            this.gvPagos.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvPagos_CellValueChanged);
            this.gvPagos.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvPagos_CellValueChanging);
            this.gvPagos.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvPagos_BeforeLeaveRow);
            this.gvPagos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvPagos_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcPagos;
            this.gridView1.Name = "gridView1";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectAll.Enabled = false;
            this.chkSelectAll.ForeColor = System.Drawing.Color.Black;
            this.chkSelectAll.Location = new System.Drawing.Point(17, 3);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(118, 17);
            this.chkSelectAll.TabIndex = 109;
            this.chkSelectAll.Text = "22501_chkSelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkSelectAll_MouseClick);
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
            this.editCheck,
            this.editValue,
            this.editValue4});
            // 
            // editChkBox
            // 
            this.editChkBox.Caption = "";
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // editCmb
            // 
            this.editCmb.Name = "editCmb";
            // 
            // editText
            // 
            this.editText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            this.editText.Name = "editText";
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpin4
            // 
            this.editSpin4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
            // 
            // editDate
            // 
            this.editDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            // 
            // editCheck
            // 
            this.editCheck.Name = "editCheck";
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editValue4
            // 
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            this.editValue4.Name = "editValue4";
            // 
            // TransferenciasBancarias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 603);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "TransferenciasBancarias";
            this.Text = "20501";
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).EndInit();
            this.gctrlHeader.ResumeLayout(false);
            this.gctrlHeader.PerformLayout();
            this.scrollControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).EndInit();
            this.gctrlGrid.ResumeLayout(false);
            this.gctrlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaTercero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumatoriaTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        private System.ComponentModel.IContainer components;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected System.Windows.Forms.Panel pnlGrids;
        protected DevExpress.XtraEditors.GroupControl gctrlHeader;
        private DevExpress.XtraEditors.XtraScrollableControl scrollControl;
        private DevExpress.XtraEditors.GroupControl gctrlGrid;
        private DevExpress.XtraEditors.LabelControl lblBeneficiario;
        private uc_MasterFind uc_Beneficiario;
        protected uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraEditors.TextEdit txtSumatoriaTercero;
        private DevExpress.XtraEditors.LabelControl lblSumatoriaTercero;
        private DevExpress.XtraEditors.TextEdit txtSumatoriaCuenta;
        private DevExpress.XtraEditors.LabelControl lblSumatoriaCuenta;
        private DevExpress.XtraEditors.TextEdit txtSumatoriaTotal;
        private DevExpress.XtraEditors.LabelControl lblSumatoriaTotal;
        private uc_MasterFind masterCuenta;
        private DevExpress.XtraEditors.LabelControl lblMonedaOrigen;
        private Clases.ComboBoxEx cmbMonedaOrigen;
        protected DevExpress.XtraGrid.GridControl gcPagos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvPagos;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblComp;
        private DevExpress.XtraEditors.LabelControl lblDetail;
        private DevExpress.XtraEditors.LabelControl lblDetailTit;
        private DevExpress.XtraEditors.LabelControl lblDate;
        private DevExpress.XtraEditors.LabelControl lblCompTit;
        private DevExpress.XtraEditors.LabelControl lblDateTit;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private System.Windows.Forms.Label label5;       
    }
}