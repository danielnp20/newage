using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReclasificacionSaldos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReclasificacionSaldos));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.txtDocExterno = new System.Windows.Forms.TextBox();
            this.lblTerceroDoc = new System.Windows.Forms.Label();
            this.rbtTercero = new System.Windows.Forms.RadioButton();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtNumDocInterno = new System.Windows.Forms.TextBox();
            this.lblPrefijoNroDoc = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.rbtPrefijo = new System.Windows.Forms.RadioButton();
            this.masterDocument = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gbQuery = new System.Windows.Forms.GroupBox();
            this.gctrlDetalle = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.masterProyectoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCostoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblMonedaExtr = new System.Windows.Forms.Label();
            this.txtSaldosExtr = new DevExpress.XtraEditors.TextEdit();
            this.lblMonedaLocal = new System.Windows.Forms.Label();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.txtSaldos = new DevExpress.XtraEditors.TextEdit();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblSaldos = new System.Windows.Forms.Label();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.periodoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
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
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).BeginInit();
            this.gctrlHeader.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            this.gbQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlDetalle)).BeginInit();
            this.gctrlDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldosExtr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
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
            this.pnlMainContainer.Size = new System.Drawing.Size(1144, 656);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.gctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.10985F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.89014F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1140, 652);
            this.tlSeparatorPanel.TabIndex = 54;
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
            this.gctrlHeader.Controls.Add(this.txtDocExterno);
            this.gctrlHeader.Controls.Add(this.lblTerceroDoc);
            this.gctrlHeader.Controls.Add(this.rbtTercero);
            this.gctrlHeader.Controls.Add(this.masterTercero);
            this.gctrlHeader.Controls.Add(this.txtNumDocInterno);
            this.gctrlHeader.Controls.Add(this.lblPrefijoNroDoc);
            this.gctrlHeader.Controls.Add(this.masterPrefijo);
            this.gctrlHeader.Controls.Add(this.rbtPrefijo);
            this.gctrlHeader.Controls.Add(this.masterDocument);
            this.gctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlHeader.Location = new System.Drawing.Point(13, 3);
            this.gctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlHeader.Name = "gctrlHeader";
            this.gctrlHeader.Size = new System.Drawing.Size(1116, 168);
            this.gctrlHeader.TabIndex = 0;
            this.gctrlHeader.Text = "20503_gctrlSearchBalance";
            // 
            // txtDocExterno
            // 
            this.txtDocExterno.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDocExterno.Location = new System.Drawing.Point(543, 123);
            this.txtDocExterno.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtDocExterno.Name = "txtDocExterno";
            this.txtDocExterno.Size = new System.Drawing.Size(116, 20);
            this.txtDocExterno.TabIndex = 4;
            // 
            // lblTerceroDoc
            // 
            this.lblTerceroDoc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTerceroDoc.AutoSize = true;
            this.lblTerceroDoc.BackColor = System.Drawing.Color.Transparent;
            this.lblTerceroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerceroDoc.Location = new System.Drawing.Point(409, 125);
            this.lblTerceroDoc.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblTerceroDoc.Name = "lblTerceroDoc";
            this.lblTerceroDoc.Size = new System.Drawing.Size(124, 14);
            this.lblTerceroDoc.TabIndex = 30;
            this.lblTerceroDoc.Text = "20503_lblTerceroDoc";
            // 
            // rbtTercero
            // 
            this.rbtTercero.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbtTercero.AutoSize = true;
            this.rbtTercero.BackColor = System.Drawing.Color.Transparent;
            this.rbtTercero.Location = new System.Drawing.Point(51, 125);
            this.rbtTercero.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rbtTercero.Name = "rbtTercero";
            this.rbtTercero.Size = new System.Drawing.Size(14, 13);
            this.rbtTercero.TabIndex = 33;
            this.rbtTercero.UseVisualStyleBackColor = false;
            this.rbtTercero.CheckedChanged += new System.EventHandler(this.rbtTercero_CheckedChanged);
            // 
            // masterTercero
            // 
            this.masterTercero.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(80, 119);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(302, 25);
            this.masterTercero.TabIndex = 3;
            this.masterTercero.Value = "";
            // 
            // txtNumDocInterno
            // 
            this.txtNumDocInterno.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNumDocInterno.Location = new System.Drawing.Point(541, 84);
            this.txtNumDocInterno.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtNumDocInterno.Name = "txtNumDocInterno";
            this.txtNumDocInterno.Size = new System.Drawing.Size(116, 20);
            this.txtNumDocInterno.TabIndex = 2;
            // 
            // lblPrefijoNroDoc
            // 
            this.lblPrefijoNroDoc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrefijoNroDoc.AutoSize = true;
            this.lblPrefijoNroDoc.BackColor = System.Drawing.Color.Transparent;
            this.lblPrefijoNroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijoNroDoc.Location = new System.Drawing.Point(407, 88);
            this.lblPrefijoNroDoc.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblPrefijoNroDoc.Name = "lblPrefijoNroDoc";
            this.lblPrefijoNroDoc.Size = new System.Drawing.Size(134, 14);
            this.lblPrefijoNroDoc.TabIndex = 27;
            this.lblPrefijoNroDoc.Text = "20503_lblPrefijoNroDoc";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(80, 83);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(302, 24);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // rbtPrefijo
            // 
            this.rbtPrefijo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbtPrefijo.AutoSize = true;
            this.rbtPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.rbtPrefijo.Checked = true;
            this.rbtPrefijo.Location = new System.Drawing.Point(51, 88);
            this.rbtPrefijo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rbtPrefijo.Name = "rbtPrefijo";
            this.rbtPrefijo.Size = new System.Drawing.Size(14, 13);
            this.rbtPrefijo.TabIndex = 29;
            this.rbtPrefijo.TabStop = true;
            this.rbtPrefijo.UseVisualStyleBackColor = false;
            this.rbtPrefijo.CheckedChanged += new System.EventHandler(this.rbtPrefijo_CheckedChanged);
            // 
            // masterDocument
            // 
            this.masterDocument.BackColor = System.Drawing.Color.Transparent;
            this.masterDocument.Filtros = null;
            this.masterDocument.Location = new System.Drawing.Point(80, 46);
            this.masterDocument.Name = "masterDocument";
            this.masterDocument.Size = new System.Drawing.Size(291, 25);
            this.masterDocument.TabIndex = 0;
            this.masterDocument.Value = "";
            // 
            // pnlDetail
            // 
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(13, 622);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1116, 27);
            this.pnlDetail.TabIndex = 112;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gbQuery);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(13, 177);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1116, 439);
            this.pnlGrids.TabIndex = 113;
            // 
            // gbQuery
            // 
            this.gbQuery.BackColor = System.Drawing.SystemColors.Control;
            this.gbQuery.Controls.Add(this.gctrlDetalle);
            this.gbQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbQuery.Enabled = false;
            this.gbQuery.Location = new System.Drawing.Point(0, 0);
            this.gbQuery.Name = "gbQuery";
            this.gbQuery.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.gbQuery.Size = new System.Drawing.Size(1116, 439);
            this.gbQuery.TabIndex = 54;
            this.gbQuery.TabStop = false;
            // 
            // gctrlDetalle
            // 
            this.gctrlDetalle.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlDetalle.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gctrlDetalle.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gctrlDetalle.Appearance.Options.UseBackColor = true;
            this.gctrlDetalle.Appearance.Options.UseBorderColor = true;
            this.gctrlDetalle.Appearance.Options.UseFont = true;
            this.gctrlDetalle.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.gctrlDetalle.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gctrlDetalle.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.gctrlDetalle.AppearanceCaption.Options.UseBorderColor = true;
            this.gctrlDetalle.AppearanceCaption.Options.UseFont = true;
            this.gctrlDetalle.AppearanceCaption.Options.UseForeColor = true;
            this.gctrlDetalle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gctrlDetalle.Controls.Add(this.groupControl1);
            this.gctrlDetalle.Controls.Add(this.lblMonedaExtr);
            this.gctrlDetalle.Controls.Add(this.txtSaldosExtr);
            this.gctrlDetalle.Controls.Add(this.lblMonedaLocal);
            this.gctrlDetalle.Controls.Add(this.lblObservacion);
            this.gctrlDetalle.Controls.Add(this.txtObservacion);
            this.gctrlDetalle.Controls.Add(this.lblPeriodo);
            this.gctrlDetalle.Controls.Add(this.txtSaldos);
            this.gctrlDetalle.Controls.Add(this.masterCuenta);
            this.gctrlDetalle.Controls.Add(this.masterProyecto);
            this.gctrlDetalle.Controls.Add(this.lblSaldos);
            this.gctrlDetalle.Controls.Add(this.masterCentroCosto);
            this.gctrlDetalle.Controls.Add(this.periodoEdit);
            this.gctrlDetalle.Controls.Add(this.masterLugarGeo);
            this.gctrlDetalle.Location = new System.Drawing.Point(52, 19);
            this.gctrlDetalle.LookAndFeel.SkinName = "Seven Classic";
            this.gctrlDetalle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlDetalle.Name = "gctrlDetalle";
            this.gctrlDetalle.Size = new System.Drawing.Size(721, 379);
            this.gctrlDetalle.TabIndex = 0;
            this.gctrlDetalle.Text = "20503_gctrlValueCurrent";
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
            this.groupControl1.Controls.Add(this.masterProyectoEdit);
            this.groupControl1.Controls.Add(this.masterLugarGeoEdit);
            this.groupControl1.Controls.Add(this.masterCentroCostoEdit);
            this.groupControl1.Location = new System.Drawing.Point(354, 94);
            this.groupControl1.LookAndFeel.SkinName = "Seven Classic";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(321, 156);
            this.groupControl1.TabIndex = 21;
            this.groupControl1.Text = "20503_gctrlValueNew";
            // 
            // masterProyectoEdit
            // 
            this.masterProyectoEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterProyectoEdit.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoEdit.Filtros = null;
            this.masterProyectoEdit.Location = new System.Drawing.Point(13, 42);
            this.masterProyectoEdit.Name = "masterProyectoEdit";
            this.masterProyectoEdit.Size = new System.Drawing.Size(301, 24);
            this.masterProyectoEdit.TabIndex = 0;
            this.masterProyectoEdit.Value = "";
            // 
            // masterLugarGeoEdit
            // 
            this.masterLugarGeoEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterLugarGeoEdit.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeoEdit.Filtros = null;
            this.masterLugarGeoEdit.Location = new System.Drawing.Point(12, 122);
            this.masterLugarGeoEdit.Name = "masterLugarGeoEdit";
            this.masterLugarGeoEdit.Size = new System.Drawing.Size(301, 24);
            this.masterLugarGeoEdit.TabIndex = 2;
            this.masterLugarGeoEdit.Value = "";
            // 
            // masterCentroCostoEdit
            // 
            this.masterCentroCostoEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterCentroCostoEdit.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCostoEdit.Filtros = null;
            this.masterCentroCostoEdit.Location = new System.Drawing.Point(13, 82);
            this.masterCentroCostoEdit.Name = "masterCentroCostoEdit";
            this.masterCentroCostoEdit.Size = new System.Drawing.Size(301, 24);
            this.masterCentroCostoEdit.TabIndex = 1;
            this.masterCentroCostoEdit.Value = "";
            // 
            // lblMonedaExtr
            // 
            this.lblMonedaExtr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMonedaExtr.AutoSize = true;
            this.lblMonedaExtr.BackColor = System.Drawing.Color.Transparent;
            this.lblMonedaExtr.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaExtr.Location = new System.Drawing.Point(325, 286);
            this.lblMonedaExtr.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblMonedaExtr.Name = "lblMonedaExtr";
            this.lblMonedaExtr.Size = new System.Drawing.Size(111, 13);
            this.lblMonedaExtr.TabIndex = 42;
            this.lblMonedaExtr.Text = "20503_lblMonedaExtr";
            this.lblMonedaExtr.Visible = false;
            // 
            // txtSaldosExtr
            // 
            this.txtSaldosExtr.EditValue = "0";
            this.txtSaldosExtr.Enabled = false;
            this.txtSaldosExtr.Location = new System.Drawing.Point(131, 282);
            this.txtSaldosExtr.Name = "txtSaldosExtr";
            this.txtSaldosExtr.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldosExtr.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldosExtr.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldosExtr.Properties.Mask.EditMask = "c";
            this.txtSaldosExtr.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldosExtr.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldosExtr.Properties.ReadOnly = true;
            this.txtSaldosExtr.Size = new System.Drawing.Size(192, 20);
            this.txtSaldosExtr.TabIndex = 41;
            this.txtSaldosExtr.Visible = false;
            // 
            // lblMonedaLocal
            // 
            this.lblMonedaLocal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMonedaLocal.AutoSize = true;
            this.lblMonedaLocal.BackColor = System.Drawing.Color.Transparent;
            this.lblMonedaLocal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaLocal.Location = new System.Drawing.Point(325, 262);
            this.lblMonedaLocal.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblMonedaLocal.Name = "lblMonedaLocal";
            this.lblMonedaLocal.Size = new System.Drawing.Size(115, 13);
            this.lblMonedaLocal.TabIndex = 40;
            this.lblMonedaLocal.Text = "20503_lblMonedaLocal";
            // 
            // lblObservacion
            // 
            this.lblObservacion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.BackColor = System.Drawing.Color.Transparent;
            this.lblObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(29, 315);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(126, 14);
            this.lblObservacion.TabIndex = 35;
            this.lblObservacion.Text = "20503_lblObservacion";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtObservacion.Location = new System.Drawing.Point(131, 314);
            this.txtObservacion.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(537, 54);
            this.txtObservacion.TabIndex = 0;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.lblPeriodo.Enabled = false;
            this.lblPeriodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.Location = new System.Drawing.Point(29, 55);
            this.lblPeriodo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(101, 14);
            this.lblPeriodo.TabIndex = 30;
            this.lblPeriodo.Text = "20503_lblPeriodo";
            // 
            // txtSaldos
            // 
            this.txtSaldos.EditValue = "0";
            this.txtSaldos.Enabled = false;
            this.txtSaldos.Location = new System.Drawing.Point(131, 258);
            this.txtSaldos.Name = "txtSaldos";
            this.txtSaldos.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldos.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldos.Properties.Mask.EditMask = "c";
            this.txtSaldos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldos.Properties.ReadOnly = true;
            this.txtSaldos.Size = new System.Drawing.Size(192, 20);
            this.txtSaldos.TabIndex = 38;
            // 
            // masterCuenta
            // 
            this.masterCuenta.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Enabled = false;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(32, 96);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(301, 24);
            this.masterCuenta.TabIndex = 31;
            this.masterCuenta.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Enabled = false;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(32, 136);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(301, 24);
            this.masterProyecto.TabIndex = 34;
            this.masterProyecto.Value = "";
            // 
            // lblSaldos
            // 
            this.lblSaldos.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSaldos.AutoSize = true;
            this.lblSaldos.BackColor = System.Drawing.Color.Transparent;
            this.lblSaldos.Enabled = false;
            this.lblSaldos.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldos.Location = new System.Drawing.Point(27, 261);
            this.lblSaldos.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblSaldos.Name = "lblSaldos";
            this.lblSaldos.Size = new System.Drawing.Size(94, 14);
            this.lblSaldos.TabIndex = 37;
            this.lblSaldos.Text = "20503_lblSaldos";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Enabled = false;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(32, 176);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(301, 24);
            this.masterCentroCosto.TabIndex = 33;
            this.masterCentroCosto.Value = "";
            // 
            // periodoEdit
            // 
            this.periodoEdit.BackColor = System.Drawing.Color.Transparent;
            this.periodoEdit.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.periodoEdit.Enabled = false;
            this.periodoEdit.EnabledControl = true;
            this.periodoEdit.ExtraPeriods = 0;
            this.periodoEdit.Location = new System.Drawing.Point(131, 53);
            this.periodoEdit.MaxValue = new System.DateTime(((long)(0)));
            this.periodoEdit.MinValue = new System.DateTime(((long)(0)));
            this.periodoEdit.Name = "periodoEdit";
            this.periodoEdit.Size = new System.Drawing.Size(152, 22);
            this.periodoEdit.TabIndex = 29;
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Enabled = false;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(32, 216);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(301, 24);
            this.masterLugarGeo.TabIndex = 32;
            this.masterLugarGeo.Value = "";
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
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            this.editDate.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
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
            // ReclasificacionSaldos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 656);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ReclasificacionSaldos";
            this.Text = "20503";
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).EndInit();
            this.gctrlHeader.ResumeLayout(false);
            this.gctrlHeader.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            this.gbQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlDetalle)).EndInit();
            this.gctrlDetalle.ResumeLayout(false);
            this.gctrlDetalle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldosExtr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected DevExpress.XtraEditors.GroupControl gctrlHeader;
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
        private System.Windows.Forms.Panel pnlDetail;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        protected System.Windows.Forms.Panel pnlGrids;
        protected System.Windows.Forms.GroupBox gbQuery;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TextBox txtDocExterno;
        private System.Windows.Forms.Label lblTerceroDoc;
        private System.Windows.Forms.RadioButton rbtTercero;
        private uc_MasterFind masterTercero;
        private System.Windows.Forms.TextBox txtNumDocInterno;
        private System.Windows.Forms.Label lblPrefijoNroDoc;
        private uc_MasterFind masterPrefijo;
        private System.Windows.Forms.RadioButton rbtPrefijo;
        private uc_MasterFind masterDocument;
        private DevExpress.XtraEditors.GroupControl gctrlDetalle;
        private System.Windows.Forms.Label lblPeriodo;
        private DevExpress.XtraEditors.TextEdit txtSaldos;
        private uc_MasterFind masterCuenta;
        private uc_MasterFind masterProyecto;
        private System.Windows.Forms.Label lblSaldos;
        private uc_MasterFind masterCentroCosto;
        private uc_PeriodoEdit periodoEdit;
        private uc_MasterFind masterLugarGeo;
        private uc_MasterFind masterProyectoEdit;
        private uc_MasterFind masterLugarGeoEdit;
        private uc_MasterFind masterCentroCostoEdit;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label lblMonedaExtr;
        private DevExpress.XtraEditors.TextEdit txtSaldosExtr;
        private System.Windows.Forms.Label lblMonedaLocal;
        private DevExpress.XtraEditors.GroupControl groupControl1;       
    }
}