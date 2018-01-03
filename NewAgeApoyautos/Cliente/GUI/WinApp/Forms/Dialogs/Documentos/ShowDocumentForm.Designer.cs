namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ShowDocumentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowDocumentForm));
            this.tc_GetDocument = new DevExpress.XtraTab.XtraTabControl();
            this.tp_document = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlDocument = new System.Windows.Forms.Panel();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.uc_DocCtrl_MasterDocumentID = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.txtFechaDoc = new System.Windows.Forms.TextBox();
            this.lblPeriodoDoc = new DevExpress.XtraEditors.LabelControl();
            this.txtPeriodoDoc = new System.Windows.Forms.TextBox();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.gb2 = new System.Windows.Forms.GroupBox();
            this.uc_DocCtrl_MasterPrefixId = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_DocCtrl_MasterAreaFuncional = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocumentoNro = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoNro = new System.Windows.Forms.TextBox();
            this.gb3 = new System.Windows.Forms.GroupBox();
            this.lblTasaCambioDOCU = new DevExpress.XtraEditors.LabelControl();
            this.txtIVA = new DevExpress.XtraEditors.TextEdit();
            this.txtTasaCambioCONT = new DevExpress.XtraEditors.TextEdit();
            this.lblIVA = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambioDOCU = new DevExpress.XtraEditors.TextEdit();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.uc_DocCtrl_MasteModena = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblValor = new DevExpress.XtraEditors.LabelControl();
            this.lblTasaCambioCONT = new DevExpress.XtraEditors.LabelControl();
            this.gb4 = new System.Windows.Forms.GroupBox();
            this.uc_DocCtrl_MasterComprobante = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtComprobanteIDNro = new System.Windows.Forms.TextBox();
            this.lblComprobanteIDNro = new DevExpress.XtraEditors.LabelControl();
            this.gb5 = new System.Windows.Forms.GroupBox();
            this.lblNumDocPadre = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoTercero = new System.Windows.Forms.TextBox();
            this.txtNumDocPadre = new System.Windows.Forms.TextBox();
            this.uc_DocCtrl_MasterLugarGeografico = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.uc_DocCtrl_MasterLineaPresupuesto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_DocCtrl_MasterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_DocCtrl_MasterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_DocCtrl_MasterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_DocCtrl_MasterProject = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocumentoTercero = new DevExpress.XtraEditors.LabelControl();
            this.gb6 = new System.Windows.Forms.GroupBox();
            this.lblFechaCreacion = new DevExpress.XtraEditors.LabelControl();
            this.txtFechaCreacion = new System.Windows.Forms.TextBox();
            this.btnRegenPdf = new System.Windows.Forms.Button();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblseUsuarioID = new DevExpress.XtraEditors.LabelControl();
            this.btnPdf = new System.Windows.Forms.Button();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtseUsuarioID = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.tp_comprobante = new DevExpress.XtraTab.XtraTabPage();
            this.gbDescripcion = new System.Windows.Forms.GroupBox();
            this.lblConceptoDesc = new System.Windows.Forms.Label();
            this.lblcuentaDesc = new System.Windows.Forms.Label();
            this.txtConceptoDesc = new DevExpress.XtraEditors.TextEdit();
            this.txtCuentaDesc = new DevExpress.XtraEditors.TextEdit();
            this.lblCentroCtoDesc = new System.Windows.Forms.Label();
            this.txtTerceroDesc = new DevExpress.XtraEditors.TextEdit();
            this.txtCentroDesc = new DevExpress.XtraEditors.TextEdit();
            this.lblTerceroDesc = new System.Windows.Forms.Label();
            this.lblLineaPresDesc = new System.Windows.Forms.Label();
            this.txtProyectoDesc = new DevExpress.XtraEditors.TextEdit();
            this.txtLineaDesc = new DevExpress.XtraEditors.TextEdit();
            this.lblProyectoDesc = new System.Windows.Forms.Label();
            this.pnGridComp = new DevExpress.XtraEditors.PanelControl();
            this.gcComprobante = new DevExpress.XtraGrid.GridControl();
            this.gvComprobante = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gb_Header = new System.Windows.Forms.GroupBox();
            this.lblComprobanteNro = new DevExpress.XtraEditors.LabelControl();
            this.uc_Comp_MasterComprobante = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txt_comp_TCOtr = new DevExpress.XtraEditors.TextEdit();
            this.lblTasaCambioOtra = new DevExpress.XtraEditors.LabelControl();
            this.txt_comp_TCBase = new DevExpress.XtraEditors.TextEdit();
            this.lblTasaCambioBase = new DevExpress.XtraEditors.LabelControl();
            this.txt_comp_MdaOrigen = new System.Windows.Forms.TextBox();
            this.lblMdaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.txt_comp_fecha = new System.Windows.Forms.TextBox();
            this.lblFechaComp = new DevExpress.XtraEditors.LabelControl();
            this.txt_comp_NroComp = new System.Windows.Forms.TextBox();
            this.txt_comp_Periodo = new System.Windows.Forms.TextBox();
            this.lblPeriodoDocComp = new DevExpress.XtraEditors.LabelControl();
            this.uc_Comp_MasterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.tp_detalle = new DevExpress.XtraTab.XtraTabPage();
            this.tp_anexos = new DevExpress.XtraTab.XtraTabPage();
            this.btnUpdateAnexos = new DevExpress.XtraEditors.SimpleButton();
            this.lblInfoAnexo = new System.Windows.Forms.LinkLabel();
            this.btnEditAnexo = new System.Windows.Forms.Button();
            this.pnlAnexos = new System.Windows.Forms.Panel();
            this.gcAnexos = new DevExpress.XtraGrid.GridControl();
            this.gvAnexos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tp_saldos = new DevExpress.XtraTab.XtraTabPage();
            this.pcSaldosDetail = new DevExpress.XtraEditors.PanelControl();
            this.gcSaldosDetail = new DevExpress.XtraGrid.GridControl();
            this.gvSaldosDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grSaldos = new System.Windows.Forms.GroupBox();
            this.pnSaldos = new DevExpress.XtraEditors.PanelControl();
            this.tlpSaldos = new System.Windows.Forms.TableLayoutPanel();
            this.txtSaldoML = new System.Windows.Forms.TextBox();
            this.txtSaldoME = new System.Windows.Forms.TextBox();
            this.lblMdaLoc = new System.Windows.Forms.Label();
            this.lblMdaExt = new System.Windows.Forms.Label();
            this.lblMdaLocName = new System.Windows.Forms.Label();
            this.lblMdaExtName = new System.Windows.Forms.Label();
            this.grParameters = new System.Windows.Forms.GroupBox();
            this.txt_saldos_terceroDesc = new System.Windows.Forms.TextBox();
            this.txt_saldos_cuentaDesc = new System.Windows.Forms.TextBox();
            this.lblPeriodTit = new System.Windows.Forms.Label();
            this.uc_Saldos_PerEditPeriodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.txt_saldos_tercero = new System.Windows.Forms.TextBox();
            this.txt_saldos_cuenta = new System.Windows.Forms.TextBox();
            this.lblTerceroTit = new System.Windows.Forms.Label();
            this.lblDocTit = new System.Windows.Forms.Label();
            this.txt_saldos_document = new System.Windows.Forms.TextBox();
            this.lblCuentaTit = new System.Windows.Forms.Label();
            this.tp_bitacora = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcBitacora = new DevExpress.XtraGrid.GridControl();
            this.gvBitacora = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tp_masInfo = new DevExpress.XtraTab.XtraTabPage();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkVer = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.pbProcess = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.tc_GetDocument)).BeginInit();
            this.tc_GetDocument.SuspendLayout();
            this.tp_document.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlDocument.SuspendLayout();
            this.gb1.SuspendLayout();
            this.gb2.SuspendLayout();
            this.gb3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambioCONT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambioDOCU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            this.gb4.SuspendLayout();
            this.gb5.SuspendLayout();
            this.gb6.SuspendLayout();
            this.tp_comprobante.SuspendLayout();
            this.gbDescripcion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConceptoDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerceroDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentroDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProyectoDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineaDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGridComp)).BeginInit();
            this.pnGridComp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComprobante)).BeginInit();
            this.gb_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comp_TCOtr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comp_TCBase.Properties)).BeginInit();
            this.tp_anexos.SuspendLayout();
            this.pnlAnexos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAnexos)).BeginInit();
            this.tp_saldos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcSaldosDetail)).BeginInit();
            this.pcSaldosDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcSaldosDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSaldosDetail)).BeginInit();
            this.grSaldos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnSaldos)).BeginInit();
            this.pnSaldos.SuspendLayout();
            this.tlpSaldos.SuspendLayout();
            this.grParameters.SuspendLayout();
            this.tp_bitacora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBitacora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tc_GetDocument
            // 
            this.tc_GetDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_GetDocument.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tc_GetDocument.AppearancePage.Header.Options.UseFont = true;
            this.tc_GetDocument.Location = new System.Drawing.Point(13, 6);
            this.tc_GetDocument.Name = "tc_GetDocument";
            this.tc_GetDocument.SelectedTabPage = this.tp_document;
            this.tc_GetDocument.Size = new System.Drawing.Size(841, 546);
            this.tc_GetDocument.TabIndex = 0;
            this.tc_GetDocument.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tp_document,
            this.tp_comprobante,
            this.tp_detalle,
            this.tp_anexos,
            this.tp_saldos,
            this.tp_bitacora,
            this.tp_masInfo});
            this.tc_GetDocument.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tc_GetDocument_SelectedPageChanged);
            // 
            // tp_document
            // 
            this.tp_document.Controls.Add(this.panel1);
            this.tp_document.Name = "tp_document";
            this.tp_document.Size = new System.Drawing.Size(835, 517);
            this.tp_document.Text = "1015_tp_document";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.pnlDocument);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(829, 511);
            this.panel1.TabIndex = 0;
            // 
            // pnlDocument
            // 
            this.pnlDocument.Controls.Add(this.gb1);
            this.pnlDocument.Controls.Add(this.gb2);
            this.pnlDocument.Controls.Add(this.gb3);
            this.pnlDocument.Controls.Add(this.gb4);
            this.pnlDocument.Controls.Add(this.gb5);
            this.pnlDocument.Controls.Add(this.gb6);
            this.pnlDocument.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDocument.Location = new System.Drawing.Point(0, 15);
            this.pnlDocument.Name = "pnlDocument";
            this.pnlDocument.Size = new System.Drawing.Size(829, 496);
            this.pnlDocument.TabIndex = 1;
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.uc_DocCtrl_MasterDocumentID);
            this.gb1.Controls.Add(this.lblFecha);
            this.gb1.Controls.Add(this.txtFechaDoc);
            this.gb1.Controls.Add(this.lblPeriodoDoc);
            this.gb1.Controls.Add(this.txtPeriodoDoc);
            this.gb1.Controls.Add(this.txtEstado);
            this.gb1.Controls.Add(this.lblEstado);
            this.gb1.Location = new System.Drawing.Point(24, 0);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(777, 71);
            this.gb1.TabIndex = 0;
            this.gb1.TabStop = false;
            // 
            // uc_DocCtrl_MasterDocumentID
            // 
            this.uc_DocCtrl_MasterDocumentID.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterDocumentID.Filtros = null;
            this.uc_DocCtrl_MasterDocumentID.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterDocumentID.Location = new System.Drawing.Point(20, 16);
            this.uc_DocCtrl_MasterDocumentID.Name = "uc_DocCtrl_MasterDocumentID";
            this.uc_DocCtrl_MasterDocumentID.Size = new System.Drawing.Size(309, 22);
            this.uc_DocCtrl_MasterDocumentID.TabIndex = 0;
            this.uc_DocCtrl_MasterDocumentID.Value = "";
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F);
            this.lblFecha.Location = new System.Drawing.Point(20, 46);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(99, 14);
            this.lblFecha.TabIndex = 9;
            this.lblFecha.Text = "1015_lblFechaDoc";
            // 
            // txtFechaDoc
            // 
            this.txtFechaDoc.Location = new System.Drawing.Point(118, 44);
            this.txtFechaDoc.Name = "txtFechaDoc";
            this.txtFechaDoc.ReadOnly = true;
            this.txtFechaDoc.Size = new System.Drawing.Size(100, 21);
            this.txtFechaDoc.TabIndex = 2;
            // 
            // lblPeriodoDoc
            // 
            this.lblPeriodoDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodoDoc.Location = new System.Drawing.Point(526, 47);
            this.lblPeriodoDoc.Name = "lblPeriodoDoc";
            this.lblPeriodoDoc.Size = new System.Drawing.Size(108, 14);
            this.lblPeriodoDoc.TabIndex = 11;
            this.lblPeriodoDoc.Text = "1015_lblPeriodoDoc";
            // 
            // txtPeriodoDoc
            // 
            this.txtPeriodoDoc.Location = new System.Drawing.Point(646, 44);
            this.txtPeriodoDoc.Name = "txtPeriodoDoc";
            this.txtPeriodoDoc.ReadOnly = true;
            this.txtPeriodoDoc.Size = new System.Drawing.Size(86, 21);
            this.txtPeriodoDoc.TabIndex = 3;
            // 
            // txtEstado
            // 
            this.txtEstado.Location = new System.Drawing.Point(646, 19);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.ReadOnly = true;
            this.txtEstado.Size = new System.Drawing.Size(86, 21);
            this.txtEstado.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(526, 22);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(83, 14);
            this.lblEstado.TabIndex = 47;
            this.lblEstado.Text = "1015_lblEstado";
            // 
            // gb2
            // 
            this.gb2.Controls.Add(this.uc_DocCtrl_MasterPrefixId);
            this.gb2.Controls.Add(this.uc_DocCtrl_MasterAreaFuncional);
            this.gb2.Controls.Add(this.lblDocumentoNro);
            this.gb2.Controls.Add(this.txtDocumentoNro);
            this.gb2.Location = new System.Drawing.Point(24, 66);
            this.gb2.Name = "gb2";
            this.gb2.Size = new System.Drawing.Size(777, 67);
            this.gb2.TabIndex = 1;
            this.gb2.TabStop = false;
            // 
            // uc_DocCtrl_MasterPrefixId
            // 
            this.uc_DocCtrl_MasterPrefixId.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterPrefixId.Filtros = null;
            this.uc_DocCtrl_MasterPrefixId.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterPrefixId.Location = new System.Drawing.Point(20, 38);
            this.uc_DocCtrl_MasterPrefixId.Name = "uc_DocCtrl_MasterPrefixId";
            this.uc_DocCtrl_MasterPrefixId.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterPrefixId.TabIndex = 1;
            this.uc_DocCtrl_MasterPrefixId.Value = "";
            // 
            // uc_DocCtrl_MasterAreaFuncional
            // 
            this.uc_DocCtrl_MasterAreaFuncional.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterAreaFuncional.Filtros = null;
            this.uc_DocCtrl_MasterAreaFuncional.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterAreaFuncional.Location = new System.Drawing.Point(20, 12);
            this.uc_DocCtrl_MasterAreaFuncional.Name = "uc_DocCtrl_MasterAreaFuncional";
            this.uc_DocCtrl_MasterAreaFuncional.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterAreaFuncional.TabIndex = 0;
            this.uc_DocCtrl_MasterAreaFuncional.Value = "";
            // 
            // lblDocumentoNro
            // 
            this.lblDocumentoNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoNro.Location = new System.Drawing.Point(526, 40);
            this.lblDocumentoNro.Name = "lblDocumentoNro";
            this.lblDocumentoNro.Size = new System.Drawing.Size(129, 14);
            this.lblDocumentoNro.TabIndex = 3;
            this.lblDocumentoNro.Text = "1015_lblDocumentoNro";
            // 
            // txtDocumentoNro
            // 
            this.txtDocumentoNro.Location = new System.Drawing.Point(646, 38);
            this.txtDocumentoNro.Name = "txtDocumentoNro";
            this.txtDocumentoNro.ReadOnly = true;
            this.txtDocumentoNro.Size = new System.Drawing.Size(86, 21);
            this.txtDocumentoNro.TabIndex = 2;
            // 
            // gb3
            // 
            this.gb3.Controls.Add(this.lblTasaCambioDOCU);
            this.gb3.Controls.Add(this.txtIVA);
            this.gb3.Controls.Add(this.txtTasaCambioCONT);
            this.gb3.Controls.Add(this.lblIVA);
            this.gb3.Controls.Add(this.txtTasaCambioDOCU);
            this.gb3.Controls.Add(this.txtValor);
            this.gb3.Controls.Add(this.uc_DocCtrl_MasteModena);
            this.gb3.Controls.Add(this.lblValor);
            this.gb3.Controls.Add(this.lblTasaCambioCONT);
            this.gb3.Location = new System.Drawing.Point(24, 128);
            this.gb3.Name = "gb3";
            this.gb3.Size = new System.Drawing.Size(777, 63);
            this.gb3.TabIndex = 2;
            this.gb3.TabStop = false;
            // 
            // lblTasaCambioDOCU
            // 
            this.lblTasaCambioDOCU.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambioDOCU.Location = new System.Drawing.Point(343, 16);
            this.lblTasaCambioDOCU.Name = "lblTasaCambioDOCU";
            this.lblTasaCambioDOCU.Size = new System.Drawing.Size(142, 14);
            this.lblTasaCambioDOCU.TabIndex = 23;
            this.lblTasaCambioDOCU.Text = "1015_lblTasaCambioDOCU";
            // 
            // txtIVA
            // 
            this.txtIVA.Location = new System.Drawing.Point(646, 38);
            this.txtIVA.Name = "txtIVA";
            this.txtIVA.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIVA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIVA.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVA.Properties.Mask.EditMask = "c";
            this.txtIVA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIVA.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIVA.Properties.ReadOnly = true;
            this.txtIVA.Size = new System.Drawing.Size(86, 20);
            this.txtIVA.TabIndex = 36;
            // 
            // txtTasaCambioCONT
            // 
            this.txtTasaCambioCONT.Location = new System.Drawing.Point(646, 13);
            this.txtTasaCambioCONT.Name = "txtTasaCambioCONT";
            this.txtTasaCambioCONT.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambioCONT.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambioCONT.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambioCONT.Properties.Mask.EditMask = "c";
            this.txtTasaCambioCONT.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambioCONT.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambioCONT.Properties.ReadOnly = true;
            this.txtTasaCambioCONT.Size = new System.Drawing.Size(86, 20);
            this.txtTasaCambioCONT.TabIndex = 2;
            // 
            // lblIVA
            // 
            this.lblIVA.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIVA.Location = new System.Drawing.Point(579, 41);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(66, 14);
            this.lblIVA.TabIndex = 37;
            this.lblIVA.Text = "1015_lblIVA";
            // 
            // txtTasaCambioDOCU
            // 
            this.txtTasaCambioDOCU.Location = new System.Drawing.Point(447, 13);
            this.txtTasaCambioDOCU.Name = "txtTasaCambioDOCU";
            this.txtTasaCambioDOCU.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambioDOCU.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambioDOCU.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambioDOCU.Properties.Mask.EditMask = "c";
            this.txtTasaCambioDOCU.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambioDOCU.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambioDOCU.Properties.ReadOnly = true;
            this.txtTasaCambioDOCU.Size = new System.Drawing.Size(74, 20);
            this.txtTasaCambioDOCU.TabIndex = 1;
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(421, 38);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Properties.ReadOnly = true;
            this.txtValor.Size = new System.Drawing.Size(100, 20);
            this.txtValor.TabIndex = 34;
            // 
            // uc_DocCtrl_MasteModena
            // 
            this.uc_DocCtrl_MasteModena.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasteModena.Filtros = null;
            this.uc_DocCtrl_MasteModena.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasteModena.Location = new System.Drawing.Point(18, 12);
            this.uc_DocCtrl_MasteModena.Name = "uc_DocCtrl_MasteModena";
            this.uc_DocCtrl_MasteModena.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasteModena.TabIndex = 0;
            this.uc_DocCtrl_MasteModena.Value = "";
            // 
            // lblValor
            // 
            this.lblValor.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(342, 41);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(73, 14);
            this.lblValor.TabIndex = 35;
            this.lblValor.Text = "1015_lblValor";
            // 
            // lblTasaCambioCONT
            // 
            this.lblTasaCambioCONT.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambioCONT.Location = new System.Drawing.Point(526, 16);
            this.lblTasaCambioCONT.Name = "lblTasaCambioCONT";
            this.lblTasaCambioCONT.Size = new System.Drawing.Size(142, 14);
            this.lblTasaCambioCONT.TabIndex = 25;
            this.lblTasaCambioCONT.Text = "1015_lblTasaCambioCONT";
            // 
            // gb4
            // 
            this.gb4.Controls.Add(this.uc_DocCtrl_MasterComprobante);
            this.gb4.Controls.Add(this.txtComprobanteIDNro);
            this.gb4.Controls.Add(this.lblComprobanteIDNro);
            this.gb4.Location = new System.Drawing.Point(24, 186);
            this.gb4.Name = "gb4";
            this.gb4.Size = new System.Drawing.Size(779, 45);
            this.gb4.TabIndex = 3;
            this.gb4.TabStop = false;
            // 
            // uc_DocCtrl_MasterComprobante
            // 
            this.uc_DocCtrl_MasterComprobante.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterComprobante.Filtros = null;
            this.uc_DocCtrl_MasterComprobante.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterComprobante.Location = new System.Drawing.Point(20, 13);
            this.uc_DocCtrl_MasterComprobante.Name = "uc_DocCtrl_MasterComprobante";
            this.uc_DocCtrl_MasterComprobante.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterComprobante.TabIndex = 0;
            this.uc_DocCtrl_MasterComprobante.Value = "";
            // 
            // txtComprobanteIDNro
            // 
            this.txtComprobanteIDNro.Location = new System.Drawing.Point(646, 17);
            this.txtComprobanteIDNro.Name = "txtComprobanteIDNro";
            this.txtComprobanteIDNro.ReadOnly = true;
            this.txtComprobanteIDNro.Size = new System.Drawing.Size(86, 21);
            this.txtComprobanteIDNro.TabIndex = 1;
            // 
            // lblComprobanteIDNro
            // 
            this.lblComprobanteIDNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprobanteIDNro.Location = new System.Drawing.Point(519, 19);
            this.lblComprobanteIDNro.Name = "lblComprobanteIDNro";
            this.lblComprobanteIDNro.Size = new System.Drawing.Size(151, 14);
            this.lblComprobanteIDNro.TabIndex = 29;
            this.lblComprobanteIDNro.Text = "1015_lblComprobanteIDNro";
            // 
            // gb5
            // 
            this.gb5.Controls.Add(this.lblNumDocPadre);
            this.gb5.Controls.Add(this.txtDocumentoTercero);
            this.gb5.Controls.Add(this.txtNumDocPadre);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterLugarGeografico);
            this.gb5.Controls.Add(this.lblNumeroDoc);
            this.gb5.Controls.Add(this.txtNumeroDoc);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterLineaPresupuesto);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterCentroCosto);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterTercero);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterCuenta);
            this.gb5.Controls.Add(this.uc_DocCtrl_MasterProject);
            this.gb5.Controls.Add(this.lblDocumentoTercero);
            this.gb5.Location = new System.Drawing.Point(24, 226);
            this.gb5.Name = "gb5";
            this.gb5.Size = new System.Drawing.Size(777, 145);
            this.gb5.TabIndex = 0;
            this.gb5.TabStop = false;
            // 
            // lblNumDocPadre
            // 
            this.lblNumDocPadre.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumDocPadre.Location = new System.Drawing.Point(589, 119);
            this.lblNumDocPadre.Name = "lblNumDocPadre";
            this.lblNumDocPadre.Size = new System.Drawing.Size(71, 13);
            this.lblNumDocPadre.TabIndex = 51;
            this.lblNumDocPadre.Text = "NumDoc Padre";
            // 
            // txtDocumentoTercero
            // 
            this.txtDocumentoTercero.Location = new System.Drawing.Point(541, 52);
            this.txtDocumentoTercero.Name = "txtDocumentoTercero";
            this.txtDocumentoTercero.ReadOnly = true;
            this.txtDocumentoTercero.Size = new System.Drawing.Size(96, 21);
            this.txtDocumentoTercero.TabIndex = 3;
            // 
            // txtNumDocPadre
            // 
            this.txtNumDocPadre.Font = new System.Drawing.Font("Tahoma", 8.3F);
            this.txtNumDocPadre.Location = new System.Drawing.Point(664, 116);
            this.txtNumDocPadre.Name = "txtNumDocPadre";
            this.txtNumDocPadre.ReadOnly = true;
            this.txtNumDocPadre.Size = new System.Drawing.Size(70, 21);
            this.txtNumDocPadre.TabIndex = 50;
            // 
            // uc_DocCtrl_MasterLugarGeografico
            // 
            this.uc_DocCtrl_MasterLugarGeografico.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterLugarGeografico.Filtros = null;
            this.uc_DocCtrl_MasterLugarGeografico.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterLugarGeografico.Location = new System.Drawing.Point(18, 113);
            this.uc_DocCtrl_MasterLugarGeografico.Name = "uc_DocCtrl_MasterLugarGeografico";
            this.uc_DocCtrl_MasterLugarGeografico.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterLugarGeografico.TabIndex = 6;
            this.uc_DocCtrl_MasterLugarGeografico.Value = "";
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroDoc.Location = new System.Drawing.Point(440, 120);
            this.lblNumeroDoc.Name = "lblNumeroDoc";
            this.lblNumeroDoc.Size = new System.Drawing.Size(41, 13);
            this.lblNumeroDoc.TabIndex = 49;
            this.lblNumeroDoc.Text = "NumDoc";
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 8.3F);
            this.txtNumeroDoc.Location = new System.Drawing.Point(499, 116);
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.ReadOnly = true;
            this.txtNumeroDoc.Size = new System.Drawing.Size(75, 21);
            this.txtNumeroDoc.TabIndex = 48;
            // 
            // uc_DocCtrl_MasterLineaPresupuesto
            // 
            this.uc_DocCtrl_MasterLineaPresupuesto.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterLineaPresupuesto.Filtros = null;
            this.uc_DocCtrl_MasterLineaPresupuesto.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterLineaPresupuesto.Location = new System.Drawing.Point(441, 82);
            this.uc_DocCtrl_MasterLineaPresupuesto.Name = "uc_DocCtrl_MasterLineaPresupuesto";
            this.uc_DocCtrl_MasterLineaPresupuesto.Size = new System.Drawing.Size(313, 25);
            this.uc_DocCtrl_MasterLineaPresupuesto.TabIndex = 5;
            this.uc_DocCtrl_MasterLineaPresupuesto.Value = "";
            // 
            // uc_DocCtrl_MasterCentroCosto
            // 
            this.uc_DocCtrl_MasterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterCentroCosto.Filtros = null;
            this.uc_DocCtrl_MasterCentroCosto.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterCentroCosto.Location = new System.Drawing.Point(18, 82);
            this.uc_DocCtrl_MasterCentroCosto.Name = "uc_DocCtrl_MasterCentroCosto";
            this.uc_DocCtrl_MasterCentroCosto.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterCentroCosto.TabIndex = 4;
            this.uc_DocCtrl_MasterCentroCosto.Value = "";
            // 
            // uc_DocCtrl_MasterTercero
            // 
            this.uc_DocCtrl_MasterTercero.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterTercero.Filtros = null;
            this.uc_DocCtrl_MasterTercero.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterTercero.Location = new System.Drawing.Point(18, 51);
            this.uc_DocCtrl_MasterTercero.Name = "uc_DocCtrl_MasterTercero";
            this.uc_DocCtrl_MasterTercero.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterTercero.TabIndex = 2;
            this.uc_DocCtrl_MasterTercero.Value = "";
            // 
            // uc_DocCtrl_MasterCuenta
            // 
            this.uc_DocCtrl_MasterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterCuenta.Filtros = null;
            this.uc_DocCtrl_MasterCuenta.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterCuenta.Location = new System.Drawing.Point(441, 21);
            this.uc_DocCtrl_MasterCuenta.Name = "uc_DocCtrl_MasterCuenta";
            this.uc_DocCtrl_MasterCuenta.Size = new System.Drawing.Size(313, 25);
            this.uc_DocCtrl_MasterCuenta.TabIndex = 1;
            this.uc_DocCtrl_MasterCuenta.Value = "";
            // 
            // uc_DocCtrl_MasterProject
            // 
            this.uc_DocCtrl_MasterProject.BackColor = System.Drawing.Color.Transparent;
            this.uc_DocCtrl_MasterProject.Filtros = null;
            this.uc_DocCtrl_MasterProject.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.uc_DocCtrl_MasterProject.Location = new System.Drawing.Point(18, 19);
            this.uc_DocCtrl_MasterProject.Name = "uc_DocCtrl_MasterProject";
            this.uc_DocCtrl_MasterProject.Size = new System.Drawing.Size(309, 25);
            this.uc_DocCtrl_MasterProject.TabIndex = 0;
            this.uc_DocCtrl_MasterProject.Value = "";
            // 
            // lblDocumentoTercero
            // 
            this.lblDocumentoTercero.Appearance.Font = new System.Drawing.Font("Tahoma", 8.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoTercero.Location = new System.Drawing.Point(441, 55);
            this.lblDocumentoTercero.Name = "lblDocumentoTercero";
            this.lblDocumentoTercero.Size = new System.Drawing.Size(153, 14);
            this.lblDocumentoTercero.TabIndex = 33;
            this.lblDocumentoTercero.Text = "1015_lblDocumentoTercero";
            // 
            // gb6
            // 
            this.gb6.Controls.Add(this.lblFechaCreacion);
            this.gb6.Controls.Add(this.txtFechaCreacion);
            this.gb6.Controls.Add(this.btnRegenPdf);
            this.gb6.Controls.Add(this.lblObservacion);
            this.gb6.Controls.Add(this.txtObservacion);
            this.gb6.Controls.Add(this.lblseUsuarioID);
            this.gb6.Controls.Add(this.btnPdf);
            this.gb6.Controls.Add(this.txtDescripcion);
            this.gb6.Controls.Add(this.txtseUsuarioID);
            this.gb6.Controls.Add(this.lblDescripcion);
            this.gb6.Location = new System.Drawing.Point(24, 368);
            this.gb6.Name = "gb6";
            this.gb6.Size = new System.Drawing.Size(777, 114);
            this.gb6.TabIndex = 5;
            this.gb6.TabStop = false;
            // 
            // lblFechaCreacion
            // 
            this.lblFechaCreacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.8F);
            this.lblFechaCreacion.Location = new System.Drawing.Point(275, 88);
            this.lblFechaCreacion.Name = "lblFechaCreacion";
            this.lblFechaCreacion.Size = new System.Drawing.Size(78, 14);
            this.lblFechaCreacion.TabIndex = 78;
            this.lblFechaCreacion.Text = "1015_lblFecha";
            // 
            // txtFechaCreacion
            // 
            this.txtFechaCreacion.Location = new System.Drawing.Point(370, 84);
            this.txtFechaCreacion.Name = "txtFechaCreacion";
            this.txtFechaCreacion.ReadOnly = true;
            this.txtFechaCreacion.Size = new System.Drawing.Size(127, 21);
            this.txtFechaCreacion.TabIndex = 77;
            // 
            // btnRegenPdf
            // 
            this.btnRegenPdf.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegenPdf.Location = new System.Drawing.Point(601, 83);
            this.btnRegenPdf.Name = "btnRegenPdf";
            this.btnRegenPdf.Size = new System.Drawing.Size(131, 23);
            this.btnRegenPdf.TabIndex = 76;
            this.btnRegenPdf.Text = "1015_btnRegenPdf";
            this.btnRegenPdf.UseVisualStyleBackColor = true;
            this.btnRegenPdf.Visible = false;
            this.btnRegenPdf.Click += new System.EventHandler(this.btnRegenPdf_Click);
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(17, 44);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(112, 14);
            this.lblObservacion.TabIndex = 45;
            this.lblObservacion.Text = "1015_lblObservacion";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(119, 42);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.ReadOnly = true;
            this.txtObservacion.Size = new System.Drawing.Size(614, 35);
            this.txtObservacion.TabIndex = 0;
            // 
            // lblseUsuarioID
            // 
            this.lblseUsuarioID.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblseUsuarioID.Location = new System.Drawing.Point(17, 87);
            this.lblseUsuarioID.Name = "lblseUsuarioID";
            this.lblseUsuarioID.Size = new System.Drawing.Size(109, 14);
            this.lblseUsuarioID.TabIndex = 55;
            this.lblseUsuarioID.Text = "1015_lblseUsuarioID";
            // 
            // btnPdf
            // 
            this.btnPdf.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPdf.Location = new System.Drawing.Point(509, 83);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(88, 23);
            this.btnPdf.TabIndex = 3;
            this.btnPdf.Text = "1015_btnPdf";
            this.btnPdf.UseVisualStyleBackColor = true;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(118, 15);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(616, 21);
            this.txtDescripcion.TabIndex = 2;
            // 
            // txtseUsuarioID
            // 
            this.txtseUsuarioID.Location = new System.Drawing.Point(120, 84);
            this.txtseUsuarioID.Name = "txtseUsuarioID";
            this.txtseUsuarioID.ReadOnly = true;
            this.txtseUsuarioID.Size = new System.Drawing.Size(144, 21);
            this.txtseUsuarioID.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(19, 18);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(107, 14);
            this.lblDescripcion.TabIndex = 75;
            this.lblDescripcion.Text = "1015_lblDescripcion";
            // 
            // tp_comprobante
            // 
            this.tp_comprobante.Controls.Add(this.gbDescripcion);
            this.tp_comprobante.Controls.Add(this.pnGridComp);
            this.tp_comprobante.Controls.Add(this.gb_Header);
            this.tp_comprobante.Name = "tp_comprobante";
            this.tp_comprobante.Size = new System.Drawing.Size(835, 517);
            this.tp_comprobante.Text = "1015_tp_comprobante";
            // 
            // gbDescripcion
            // 
            this.gbDescripcion.Controls.Add(this.lblConceptoDesc);
            this.gbDescripcion.Controls.Add(this.lblcuentaDesc);
            this.gbDescripcion.Controls.Add(this.txtConceptoDesc);
            this.gbDescripcion.Controls.Add(this.txtCuentaDesc);
            this.gbDescripcion.Controls.Add(this.lblCentroCtoDesc);
            this.gbDescripcion.Controls.Add(this.txtTerceroDesc);
            this.gbDescripcion.Controls.Add(this.txtCentroDesc);
            this.gbDescripcion.Controls.Add(this.lblTerceroDesc);
            this.gbDescripcion.Controls.Add(this.lblLineaPresDesc);
            this.gbDescripcion.Controls.Add(this.txtProyectoDesc);
            this.gbDescripcion.Controls.Add(this.txtLineaDesc);
            this.gbDescripcion.Controls.Add(this.lblProyectoDesc);
            this.gbDescripcion.Location = new System.Drawing.Point(29, 423);
            this.gbDescripcion.Name = "gbDescripcion";
            this.gbDescripcion.Size = new System.Drawing.Size(788, 80);
            this.gbDescripcion.TabIndex = 118;
            this.gbDescripcion.TabStop = false;
            this.gbDescripcion.Text = "Descripción";
            // 
            // lblConceptoDesc
            // 
            this.lblConceptoDesc.AutoSize = true;
            this.lblConceptoDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConceptoDesc.Location = new System.Drawing.Point(502, 56);
            this.lblConceptoDesc.Name = "lblConceptoDesc";
            this.lblConceptoDesc.Size = new System.Drawing.Size(137, 14);
            this.lblConceptoDesc.TabIndex = 117;
            this.lblConceptoDesc.Text = "1015_txtConceptoDesc";
            // 
            // lblcuentaDesc
            // 
            this.lblcuentaDesc.AutoSize = true;
            this.lblcuentaDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcuentaDesc.Location = new System.Drawing.Point(5, 28);
            this.lblcuentaDesc.Name = "lblcuentaDesc";
            this.lblcuentaDesc.Size = new System.Drawing.Size(123, 14);
            this.lblcuentaDesc.TabIndex = 110;
            this.lblcuentaDesc.Text = "1015_txtCuentaDesc";
            // 
            // txtConceptoDesc
            // 
            this.txtConceptoDesc.EditValue = "";
            this.txtConceptoDesc.Location = new System.Drawing.Point(604, 53);
            this.txtConceptoDesc.Name = "txtConceptoDesc";
            this.txtConceptoDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtConceptoDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtConceptoDesc.Properties.ReadOnly = true;
            this.txtConceptoDesc.Size = new System.Drawing.Size(152, 20);
            this.txtConceptoDesc.TabIndex = 115;
            // 
            // txtCuentaDesc
            // 
            this.txtCuentaDesc.EditValue = "";
            this.txtCuentaDesc.Location = new System.Drawing.Point(95, 25);
            this.txtCuentaDesc.Name = "txtCuentaDesc";
            this.txtCuentaDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCuentaDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtCuentaDesc.Properties.ReadOnly = true;
            this.txtCuentaDesc.Size = new System.Drawing.Size(154, 20);
            this.txtCuentaDesc.TabIndex = 106;
            // 
            // lblCentroCtoDesc
            // 
            this.lblCentroCtoDesc.AutoSize = true;
            this.lblCentroCtoDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCentroCtoDesc.Location = new System.Drawing.Point(5, 56);
            this.lblCentroCtoDesc.Name = "lblCentroCtoDesc";
            this.lblCentroCtoDesc.Size = new System.Drawing.Size(140, 14);
            this.lblCentroCtoDesc.TabIndex = 116;
            this.lblCentroCtoDesc.Text = "1015_txtCentroCtoDesc";
            // 
            // txtTerceroDesc
            // 
            this.txtTerceroDesc.EditValue = "";
            this.txtTerceroDesc.Location = new System.Drawing.Point(348, 25);
            this.txtTerceroDesc.Name = "txtTerceroDesc";
            this.txtTerceroDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTerceroDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtTerceroDesc.Properties.ReadOnly = true;
            this.txtTerceroDesc.Size = new System.Drawing.Size(146, 20);
            this.txtTerceroDesc.TabIndex = 107;
            // 
            // txtCentroDesc
            // 
            this.txtCentroDesc.EditValue = "";
            this.txtCentroDesc.Location = new System.Drawing.Point(95, 53);
            this.txtCentroDesc.Name = "txtCentroDesc";
            this.txtCentroDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCentroDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtCentroDesc.Properties.ReadOnly = true;
            this.txtCentroDesc.Size = new System.Drawing.Size(152, 20);
            this.txtCentroDesc.TabIndex = 114;
            // 
            // lblTerceroDesc
            // 
            this.lblTerceroDesc.AutoSize = true;
            this.lblTerceroDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerceroDesc.Location = new System.Drawing.Point(255, 28);
            this.lblTerceroDesc.Name = "lblTerceroDesc";
            this.lblTerceroDesc.Size = new System.Drawing.Size(127, 14);
            this.lblTerceroDesc.TabIndex = 111;
            this.lblTerceroDesc.Text = "1015_txtTerceroDesc";
            // 
            // lblLineaPresDesc
            // 
            this.lblLineaPresDesc.AutoSize = true;
            this.lblLineaPresDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineaPresDesc.Location = new System.Drawing.Point(255, 56);
            this.lblLineaPresDesc.Name = "lblLineaPresDesc";
            this.lblLineaPresDesc.Size = new System.Drawing.Size(135, 14);
            this.lblLineaPresDesc.TabIndex = 113;
            this.lblLineaPresDesc.Text = "1015_txtLineaPresDesc";
            // 
            // txtProyectoDesc
            // 
            this.txtProyectoDesc.EditValue = "";
            this.txtProyectoDesc.Location = new System.Drawing.Point(602, 25);
            this.txtProyectoDesc.Name = "txtProyectoDesc";
            this.txtProyectoDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtProyectoDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtProyectoDesc.Properties.ReadOnly = true;
            this.txtProyectoDesc.Size = new System.Drawing.Size(154, 20);
            this.txtProyectoDesc.TabIndex = 108;
            // 
            // txtLineaDesc
            // 
            this.txtLineaDesc.EditValue = "";
            this.txtLineaDesc.Location = new System.Drawing.Point(348, 53);
            this.txtLineaDesc.Name = "txtLineaDesc";
            this.txtLineaDesc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLineaDesc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtLineaDesc.Properties.ReadOnly = true;
            this.txtLineaDesc.Size = new System.Drawing.Size(146, 20);
            this.txtLineaDesc.TabIndex = 109;
            // 
            // lblProyectoDesc
            // 
            this.lblProyectoDesc.AutoSize = true;
            this.lblProyectoDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProyectoDesc.Location = new System.Drawing.Point(502, 28);
            this.lblProyectoDesc.Name = "lblProyectoDesc";
            this.lblProyectoDesc.Size = new System.Drawing.Size(133, 14);
            this.lblProyectoDesc.TabIndex = 112;
            this.lblProyectoDesc.Text = "1015_txtProyectoDesc";
            // 
            // pnGridComp
            // 
            this.pnGridComp.Controls.Add(this.gcComprobante);
            this.pnGridComp.Location = new System.Drawing.Point(27, 147);
            this.pnGridComp.Name = "pnGridComp";
            this.pnGridComp.Size = new System.Drawing.Size(790, 277);
            this.pnGridComp.TabIndex = 1;
            // 
            // gcComprobante
            // 
            this.gcComprobante.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcComprobante.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcComprobante.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcComprobante.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcComprobante.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcComprobante.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcComprobante.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcComprobante.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcComprobante.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcComprobante.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcComprobante.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcComprobante.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcComprobante.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcComprobante.Location = new System.Drawing.Point(2, 2);
            this.gcComprobante.LookAndFeel.SkinName = "Dark Side";
            this.gcComprobante.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcComprobante.MainView = this.gvComprobante;
            this.gcComprobante.Margin = new System.Windows.Forms.Padding(4);
            this.gcComprobante.Name = "gcComprobante";
            this.gcComprobante.Size = new System.Drawing.Size(786, 273);
            this.gcComprobante.TabIndex = 0;
            this.gcComprobante.UseEmbeddedNavigator = true;
            this.gcComprobante.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvComprobante});
            // 
            // gvComprobante
            // 
            this.gvComprobante.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComprobante.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvComprobante.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvComprobante.Appearance.Empty.Options.UseBackColor = true;
            this.gvComprobante.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvComprobante.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvComprobante.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvComprobante.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvComprobante.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvComprobante.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvComprobante.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvComprobante.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvComprobante.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvComprobante.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvComprobante.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvComprobante.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvComprobante.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvComprobante.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvComprobante.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComprobante.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvComprobante.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvComprobante.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvComprobante.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvComprobante.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvComprobante.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvComprobante.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvComprobante.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvComprobante.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvComprobante.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvComprobante.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvComprobante.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvComprobante.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvComprobante.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvComprobante.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvComprobante.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvComprobante.Appearance.Row.Options.UseBackColor = true;
            this.gvComprobante.Appearance.Row.Options.UseForeColor = true;
            this.gvComprobante.Appearance.Row.Options.UseTextOptions = true;
            this.gvComprobante.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvComprobante.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvComprobante.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvComprobante.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvComprobante.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvComprobante.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvComprobante.Appearance.VertLine.Options.UseBackColor = true;
            this.gvComprobante.GridControl = this.gcComprobante;
            this.gvComprobante.Name = "gvComprobante";
            this.gvComprobante.OptionsCustomization.AllowColumnMoving = false;
            this.gvComprobante.OptionsCustomization.AllowFilter = false;
            this.gvComprobante.OptionsMenu.EnableColumnMenu = false;
            this.gvComprobante.OptionsMenu.EnableFooterMenu = false;
            this.gvComprobante.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvComprobante.OptionsView.ColumnAutoWidth = false;
            this.gvComprobante.OptionsView.ShowGroupPanel = false;
            this.gvComprobante.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvComprobante.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvComprobante_CustomRowCellEdit);
            this.gvComprobante.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvComprobante_FocusedRowChanged);
            this.gvComprobante.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvComprobante_CustomUnboundColumnData);
            this.gvComprobante.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvAnexos_CustomColumnDisplayText);
            this.gvComprobante.DoubleClick += new System.EventHandler(this.gvComprobante_DoubleClick);
            // 
            // gb_Header
            // 
            this.gb_Header.Controls.Add(this.lblComprobanteNro);
            this.gb_Header.Controls.Add(this.uc_Comp_MasterComprobante);
            this.gb_Header.Controls.Add(this.txt_comp_TCOtr);
            this.gb_Header.Controls.Add(this.lblTasaCambioOtra);
            this.gb_Header.Controls.Add(this.txt_comp_TCBase);
            this.gb_Header.Controls.Add(this.lblTasaCambioBase);
            this.gb_Header.Controls.Add(this.txt_comp_MdaOrigen);
            this.gb_Header.Controls.Add(this.lblMdaOrigen);
            this.gb_Header.Controls.Add(this.txt_comp_fecha);
            this.gb_Header.Controls.Add(this.lblFechaComp);
            this.gb_Header.Controls.Add(this.txt_comp_NroComp);
            this.gb_Header.Controls.Add(this.txt_comp_Periodo);
            this.gb_Header.Controls.Add(this.lblPeriodoDocComp);
            this.gb_Header.Controls.Add(this.uc_Comp_MasterMoneda);
            this.gb_Header.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Header.Location = new System.Drawing.Point(29, 25);
            this.gb_Header.Name = "gb_Header";
            this.gb_Header.Size = new System.Drawing.Size(788, 116);
            this.gb_Header.TabIndex = 0;
            this.gb_Header.TabStop = false;
            this.gb_Header.Text = "1015_gb_Header";
            // 
            // lblComprobanteNro
            // 
            this.lblComprobanteNro.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblComprobanteNro.Location = new System.Drawing.Point(366, 27);
            this.lblComprobanteNro.Name = "lblComprobanteNro";
            this.lblComprobanteNro.Size = new System.Drawing.Size(151, 14);
            this.lblComprobanteNro.TabIndex = 86;
            this.lblComprobanteNro.Text = "1015_lblComprobanteIDNro";
            // 
            // uc_Comp_MasterComprobante
            // 
            this.uc_Comp_MasterComprobante.BackColor = System.Drawing.Color.Transparent;
            this.uc_Comp_MasterComprobante.Filtros = null;
            this.uc_Comp_MasterComprobante.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_Comp_MasterComprobante.Location = new System.Drawing.Point(15, 22);
            this.uc_Comp_MasterComprobante.Name = "uc_Comp_MasterComprobante";
            this.uc_Comp_MasterComprobante.Size = new System.Drawing.Size(351, 25);
            this.uc_Comp_MasterComprobante.TabIndex = 0;
            this.uc_Comp_MasterComprobante.Value = "";
            // 
            // txt_comp_TCOtr
            // 
            this.txt_comp_TCOtr.Location = new System.Drawing.Point(342, 84);
            this.txt_comp_TCOtr.Name = "txt_comp_TCOtr";
            this.txt_comp_TCOtr.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_comp_TCOtr.Properties.Mask.EditMask = "c";
            this.txt_comp_TCOtr.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_comp_TCOtr.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txt_comp_TCOtr.Properties.ReadOnly = true;
            this.txt_comp_TCOtr.Size = new System.Drawing.Size(95, 20);
            this.txt_comp_TCOtr.TabIndex = 7;
            // 
            // lblTasaCambioOtra
            // 
            this.lblTasaCambioOtra.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTasaCambioOtra.Location = new System.Drawing.Point(240, 88);
            this.lblTasaCambioOtra.Name = "lblTasaCambioOtra";
            this.lblTasaCambioOtra.Size = new System.Drawing.Size(134, 14);
            this.lblTasaCambioOtra.TabIndex = 95;
            this.lblTasaCambioOtra.Text = "1015_lblTasaCambioOtra";
            // 
            // txt_comp_TCBase
            // 
            this.txt_comp_TCBase.Location = new System.Drawing.Point(133, 85);
            this.txt_comp_TCBase.Name = "txt_comp_TCBase";
            this.txt_comp_TCBase.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_comp_TCBase.Properties.Mask.EditMask = "c";
            this.txt_comp_TCBase.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_comp_TCBase.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txt_comp_TCBase.Properties.ReadOnly = true;
            this.txt_comp_TCBase.Size = new System.Drawing.Size(100, 20);
            this.txt_comp_TCBase.TabIndex = 6;
            // 
            // lblTasaCambioBase
            // 
            this.lblTasaCambioBase.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTasaCambioBase.Location = new System.Drawing.Point(15, 88);
            this.lblTasaCambioBase.Name = "lblTasaCambioBase";
            this.lblTasaCambioBase.Size = new System.Drawing.Size(135, 14);
            this.lblTasaCambioBase.TabIndex = 93;
            this.lblTasaCambioBase.Text = "1015_lblTasaCambioBase";
            // 
            // txt_comp_MdaOrigen
            // 
            this.txt_comp_MdaOrigen.Location = new System.Drawing.Point(336, 54);
            this.txt_comp_MdaOrigen.Name = "txt_comp_MdaOrigen";
            this.txt_comp_MdaOrigen.ReadOnly = true;
            this.txt_comp_MdaOrigen.Size = new System.Drawing.Size(100, 22);
            this.txt_comp_MdaOrigen.TabIndex = 4;
            // 
            // lblMdaOrigen
            // 
            this.lblMdaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblMdaOrigen.Location = new System.Drawing.Point(241, 58);
            this.lblMdaOrigen.Name = "lblMdaOrigen";
            this.lblMdaOrigen.Size = new System.Drawing.Size(125, 14);
            this.lblMdaOrigen.TabIndex = 90;
            this.lblMdaOrigen.Text = "1015_lblMonedaOrigen";
            // 
            // txt_comp_fecha
            // 
            this.txt_comp_fecha.Location = new System.Drawing.Point(132, 54);
            this.txt_comp_fecha.Name = "txt_comp_fecha";
            this.txt_comp_fecha.ReadOnly = true;
            this.txt_comp_fecha.Size = new System.Drawing.Size(100, 22);
            this.txt_comp_fecha.TabIndex = 3;
            // 
            // lblFechaComp
            // 
            this.lblFechaComp.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblFechaComp.Location = new System.Drawing.Point(15, 58);
            this.lblFechaComp.Name = "lblFechaComp";
            this.lblFechaComp.Size = new System.Drawing.Size(78, 14);
            this.lblFechaComp.TabIndex = 88;
            this.lblFechaComp.Text = "1015_lblFecha";
            // 
            // txt_comp_NroComp
            // 
            this.txt_comp_NroComp.Location = new System.Drawing.Point(471, 23);
            this.txt_comp_NroComp.Name = "txt_comp_NroComp";
            this.txt_comp_NroComp.ReadOnly = true;
            this.txt_comp_NroComp.Size = new System.Drawing.Size(100, 22);
            this.txt_comp_NroComp.TabIndex = 1;
            // 
            // txt_comp_Periodo
            // 
            this.txt_comp_Periodo.Location = new System.Drawing.Point(676, 22);
            this.txt_comp_Periodo.Name = "txt_comp_Periodo";
            this.txt_comp_Periodo.ReadOnly = true;
            this.txt_comp_Periodo.Size = new System.Drawing.Size(100, 22);
            this.txt_comp_Periodo.TabIndex = 2;
            // 
            // lblPeriodoDocComp
            // 
            this.lblPeriodoDocComp.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPeriodoDocComp.Location = new System.Drawing.Point(589, 26);
            this.lblPeriodoDocComp.Name = "lblPeriodoDocComp";
            this.lblPeriodoDocComp.Size = new System.Drawing.Size(108, 14);
            this.lblPeriodoDocComp.TabIndex = 84;
            this.lblPeriodoDocComp.Text = "1015_lblPeriodoDoc";
            // 
            // uc_Comp_MasterMoneda
            // 
            this.uc_Comp_MasterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.uc_Comp_MasterMoneda.Filtros = null;
            this.uc_Comp_MasterMoneda.Location = new System.Drawing.Point(441, 53);
            this.uc_Comp_MasterMoneda.Name = "uc_Comp_MasterMoneda";
            this.uc_Comp_MasterMoneda.Size = new System.Drawing.Size(333, 25);
            this.uc_Comp_MasterMoneda.TabIndex = 5;
            this.uc_Comp_MasterMoneda.Value = "";
            // 
            // tp_detalle
            // 
            this.tp_detalle.Name = "tp_detalle";
            this.tp_detalle.Size = new System.Drawing.Size(835, 517);
            this.tp_detalle.Text = "1015_tp_detalle";
            // 
            // tp_anexos
            // 
            this.tp_anexos.Controls.Add(this.btnUpdateAnexos);
            this.tp_anexos.Controls.Add(this.lblInfoAnexo);
            this.tp_anexos.Controls.Add(this.btnEditAnexo);
            this.tp_anexos.Controls.Add(this.pnlAnexos);
            this.tp_anexos.Name = "tp_anexos";
            this.tp_anexos.Size = new System.Drawing.Size(835, 517);
            this.tp_anexos.Text = "1015_tp_anexos";
            // 
            // btnUpdateAnexos
            // 
            this.btnUpdateAnexos.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateAnexos.Image")));
            this.btnUpdateAnexos.Location = new System.Drawing.Point(601, 353);
            this.btnUpdateAnexos.Name = "btnUpdateAnexos";
            this.btnUpdateAnexos.Size = new System.Drawing.Size(24, 21);
            this.btnUpdateAnexos.TabIndex = 79;
            this.btnUpdateAnexos.ToolTip = "Actualizar registros";
            this.btnUpdateAnexos.Click += new System.EventHandler(this.btnUpdateAnexos_Click);
            // 
            // lblInfoAnexo
            // 
            this.lblInfoAnexo.AutoSize = true;
            this.lblInfoAnexo.Font = new System.Drawing.Font("Tahoma", 6.5F);
            this.lblInfoAnexo.LinkColor = System.Drawing.Color.Navy;
            this.lblInfoAnexo.Location = new System.Drawing.Point(100, 349);
            this.lblInfoAnexo.Name = "lblInfoAnexo";
            this.lblInfoAnexo.Size = new System.Drawing.Size(467, 11);
            this.lblInfoAnexo.TabIndex = 78;
            this.lblInfoAnexo.TabStop = true;
            this.lblInfoAnexo.Text = "Si desea ver anexos nuevos, asegurese que el  documento exista en maestra Documen" +
    "to Anexo de Mod. Global";
            this.lblInfoAnexo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblInfoAnexo_LinkClicked);
            // 
            // btnEditAnexo
            // 
            this.btnEditAnexo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditAnexo.Location = new System.Drawing.Point(631, 351);
            this.btnEditAnexo.Name = "btnEditAnexo";
            this.btnEditAnexo.Size = new System.Drawing.Size(125, 23);
            this.btnEditAnexo.TabIndex = 77;
            this.btnEditAnexo.Text = "1015_btnEditAnexo";
            this.btnEditAnexo.UseVisualStyleBackColor = true;
            this.btnEditAnexo.Click += new System.EventHandler(this.btnEditAnexo_Click);
            // 
            // pnlAnexos
            // 
            this.pnlAnexos.Controls.Add(this.gcAnexos);
            this.pnlAnexos.Location = new System.Drawing.Point(102, 28);
            this.pnlAnexos.Name = "pnlAnexos";
            this.pnlAnexos.Size = new System.Drawing.Size(654, 318);
            this.pnlAnexos.TabIndex = 2;
            // 
            // gcAnexos
            // 
            this.gcAnexos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcAnexos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcAnexos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcAnexos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcAnexos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcAnexos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcAnexos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcAnexos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcAnexos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcAnexos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcAnexos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcAnexos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcAnexos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcAnexos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcAnexos.Location = new System.Drawing.Point(0, 0);
            this.gcAnexos.LookAndFeel.SkinName = "Dark Side";
            this.gcAnexos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcAnexos.MainView = this.gvAnexos;
            this.gcAnexos.Margin = new System.Windows.Forms.Padding(4);
            this.gcAnexos.Name = "gcAnexos";
            this.gcAnexos.Size = new System.Drawing.Size(654, 318);
            this.gcAnexos.TabIndex = 1;
            this.gcAnexos.UseEmbeddedNavigator = true;
            this.gcAnexos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAnexos});
            // 
            // gvAnexos
            // 
            this.gvAnexos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvAnexos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvAnexos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.Empty.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvAnexos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAnexos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvAnexos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAnexos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvAnexos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvAnexos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvAnexos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvAnexos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvAnexos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvAnexos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvAnexos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvAnexos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAnexos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvAnexos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvAnexos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvAnexos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvAnexos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvAnexos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.Row.Options.UseBackColor = true;
            this.gvAnexos.Appearance.Row.Options.UseForeColor = true;
            this.gvAnexos.Appearance.Row.Options.UseTextOptions = true;
            this.gvAnexos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvAnexos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvAnexos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvAnexos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvAnexos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvAnexos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvAnexos.GridControl = this.gcAnexos;
            this.gvAnexos.Name = "gvAnexos";
            this.gvAnexos.OptionsCustomization.AllowColumnMoving = false;
            this.gvAnexos.OptionsCustomization.AllowFilter = false;
            this.gvAnexos.OptionsCustomization.AllowSort = false;
            this.gvAnexos.OptionsMenu.EnableColumnMenu = false;
            this.gvAnexos.OptionsMenu.EnableFooterMenu = false;
            this.gvAnexos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvAnexos.OptionsView.ColumnAutoWidth = false;
            this.gvAnexos.OptionsView.ShowGroupPanel = false;
            this.gvAnexos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvAnexos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvComprobante_CustomUnboundColumnData);
            this.gvAnexos.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvAnexos_CustomColumnDisplayText);
            // 
            // tp_saldos
            // 
            this.tp_saldos.Controls.Add(this.pcSaldosDetail);
            this.tp_saldos.Controls.Add(this.grSaldos);
            this.tp_saldos.Controls.Add(this.grParameters);
            this.tp_saldos.Name = "tp_saldos";
            this.tp_saldos.Size = new System.Drawing.Size(835, 517);
            this.tp_saldos.Text = "1015_tp_saldos";
            // 
            // pcSaldosDetail
            // 
            this.pcSaldosDetail.Controls.Add(this.gcSaldosDetail);
            this.pcSaldosDetail.Location = new System.Drawing.Point(24, 183);
            this.pcSaldosDetail.Name = "pcSaldosDetail";
            this.pcSaldosDetail.Size = new System.Drawing.Size(783, 300);
            this.pcSaldosDetail.TabIndex = 16;
            // 
            // gcSaldosDetail
            // 
            this.gcSaldosDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSaldosDetail.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcSaldosDetail.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcSaldosDetail.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcSaldosDetail.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcSaldosDetail.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcSaldosDetail.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcSaldosDetail.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcSaldosDetail.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcSaldosDetail.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcSaldosDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcSaldosDetail.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcSaldosDetail.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcSaldosDetail.Location = new System.Drawing.Point(2, 2);
            this.gcSaldosDetail.LookAndFeel.SkinName = "Dark Side";
            this.gcSaldosDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcSaldosDetail.MainView = this.gvSaldosDetail;
            this.gcSaldosDetail.Margin = new System.Windows.Forms.Padding(4);
            this.gcSaldosDetail.Name = "gcSaldosDetail";
            this.gcSaldosDetail.Size = new System.Drawing.Size(779, 296);
            this.gcSaldosDetail.TabIndex = 0;
            this.gcSaldosDetail.UseEmbeddedNavigator = true;
            this.gcSaldosDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSaldosDetail});
            // 
            // gvSaldosDetail
            // 
            this.gvSaldosDetail.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSaldosDetail.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvSaldosDetail.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvSaldosDetail.Appearance.Empty.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvSaldosDetail.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldosDetail.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvSaldosDetail.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldosDetail.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldosDetail.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldosDetail.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvSaldosDetail.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvSaldosDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSaldosDetail.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvSaldosDetail.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvSaldosDetail.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvSaldosDetail.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvSaldosDetail.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldosDetail.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvSaldosDetail.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSaldosDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvSaldosDetail.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvSaldosDetail.Appearance.Row.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.Row.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.Row.Options.UseTextOptions = true;
            this.gvSaldosDetail.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvSaldosDetail.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSaldosDetail.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvSaldosDetail.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvSaldosDetail.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvSaldosDetail.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSaldosDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.gvSaldosDetail.GridControl = this.gcSaldosDetail;
            this.gvSaldosDetail.Name = "gvSaldosDetail";
            this.gvSaldosDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvSaldosDetail.OptionsCustomization.AllowFilter = false;
            this.gvSaldosDetail.OptionsCustomization.AllowSort = false;
            this.gvSaldosDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvSaldosDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvSaldosDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvSaldosDetail.OptionsView.ColumnAutoWidth = false;
            this.gvSaldosDetail.OptionsView.ShowGroupPanel = false;
            this.gvSaldosDetail.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvSaldosDetail.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvSaldosDetail_CustomRowCellEdit);
            this.gvSaldosDetail.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvComprobante_CustomUnboundColumnData);
            // 
            // grSaldos
            // 
            this.grSaldos.Controls.Add(this.pnSaldos);
            this.grSaldos.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grSaldos.Location = new System.Drawing.Point(514, 18);
            this.grSaldos.Name = "grSaldos";
            this.grSaldos.Size = new System.Drawing.Size(293, 150);
            this.grSaldos.TabIndex = 15;
            this.grSaldos.TabStop = false;
            this.grSaldos.Text = "1015_grSaldos";
            // 
            // pnSaldos
            // 
            this.pnSaldos.Controls.Add(this.tlpSaldos);
            this.pnSaldos.Location = new System.Drawing.Point(20, 36);
            this.pnSaldos.Name = "pnSaldos";
            this.pnSaldos.Size = new System.Drawing.Size(254, 94);
            this.pnSaldos.TabIndex = 1;
            // 
            // tlpSaldos
            // 
            this.tlpSaldos.BackColor = System.Drawing.Color.Transparent;
            this.tlpSaldos.ColumnCount = 2;
            this.tlpSaldos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSaldos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSaldos.Controls.Add(this.txtSaldoML, 0, 2);
            this.tlpSaldos.Controls.Add(this.txtSaldoME, 0, 2);
            this.tlpSaldos.Controls.Add(this.lblMdaLoc, 0, 0);
            this.tlpSaldos.Controls.Add(this.lblMdaExt, 1, 0);
            this.tlpSaldos.Controls.Add(this.lblMdaLocName, 0, 1);
            this.tlpSaldos.Controls.Add(this.lblMdaExtName, 1, 1);
            this.tlpSaldos.Location = new System.Drawing.Point(2, 2);
            this.tlpSaldos.Name = "tlpSaldos";
            this.tlpSaldos.RowCount = 3;
            this.tlpSaldos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSaldos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSaldos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpSaldos.Size = new System.Drawing.Size(250, 90);
            this.tlpSaldos.TabIndex = 0;
            // 
            // txtSaldoML
            // 
            this.txtSaldoML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSaldoML.Location = new System.Drawing.Point(0, 50);
            this.txtSaldoML.Margin = new System.Windows.Forms.Padding(0);
            this.txtSaldoML.Multiline = true;
            this.txtSaldoML.Name = "txtSaldoML";
            this.txtSaldoML.ReadOnly = true;
            this.txtSaldoML.Size = new System.Drawing.Size(125, 40);
            this.txtSaldoML.TabIndex = 3;
            // 
            // txtSaldoME
            // 
            this.txtSaldoME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSaldoME.Location = new System.Drawing.Point(125, 50);
            this.txtSaldoME.Margin = new System.Windows.Forms.Padding(0);
            this.txtSaldoME.Multiline = true;
            this.txtSaldoME.Name = "txtSaldoME";
            this.txtSaldoME.ReadOnly = true;
            this.txtSaldoME.Size = new System.Drawing.Size(125, 40);
            this.txtSaldoME.TabIndex = 2;
            // 
            // lblMdaLoc
            // 
            this.lblMdaLoc.BackColor = System.Drawing.Color.Transparent;
            this.lblMdaLoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMdaLoc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMdaLoc.Location = new System.Drawing.Point(0, 0);
            this.lblMdaLoc.Margin = new System.Windows.Forms.Padding(0);
            this.lblMdaLoc.Name = "lblMdaLoc";
            this.lblMdaLoc.Size = new System.Drawing.Size(125, 25);
            this.lblMdaLoc.TabIndex = 0;
            this.lblMdaLoc.Text = "1011_monedaLocal";
            this.lblMdaLoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMdaExt
            // 
            this.lblMdaExt.BackColor = System.Drawing.Color.Transparent;
            this.lblMdaExt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMdaExt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMdaExt.Location = new System.Drawing.Point(125, 0);
            this.lblMdaExt.Margin = new System.Windows.Forms.Padding(0);
            this.lblMdaExt.Name = "lblMdaExt";
            this.lblMdaExt.Size = new System.Drawing.Size(125, 25);
            this.lblMdaExt.TabIndex = 1;
            this.lblMdaExt.Text = "1011_monedaExtranjera";
            this.lblMdaExt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMdaLocName
            // 
            this.lblMdaLocName.AutoSize = true;
            this.lblMdaLocName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMdaLocName.Location = new System.Drawing.Point(0, 25);
            this.lblMdaLocName.Margin = new System.Windows.Forms.Padding(0);
            this.lblMdaLocName.Name = "lblMdaLocName";
            this.lblMdaLocName.Size = new System.Drawing.Size(125, 25);
            this.lblMdaLocName.TabIndex = 4;
            this.lblMdaLocName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMdaExtName
            // 
            this.lblMdaExtName.AutoSize = true;
            this.lblMdaExtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMdaExtName.Location = new System.Drawing.Point(125, 25);
            this.lblMdaExtName.Margin = new System.Windows.Forms.Padding(0);
            this.lblMdaExtName.Name = "lblMdaExtName";
            this.lblMdaExtName.Size = new System.Drawing.Size(125, 25);
            this.lblMdaExtName.TabIndex = 5;
            this.lblMdaExtName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grParameters
            // 
            this.grParameters.Controls.Add(this.txt_saldos_terceroDesc);
            this.grParameters.Controls.Add(this.txt_saldos_cuentaDesc);
            this.grParameters.Controls.Add(this.lblPeriodTit);
            this.grParameters.Controls.Add(this.uc_Saldos_PerEditPeriodo);
            this.grParameters.Controls.Add(this.txt_saldos_tercero);
            this.grParameters.Controls.Add(this.txt_saldos_cuenta);
            this.grParameters.Controls.Add(this.lblTerceroTit);
            this.grParameters.Controls.Add(this.lblDocTit);
            this.grParameters.Controls.Add(this.txt_saldos_document);
            this.grParameters.Controls.Add(this.lblCuentaTit);
            this.grParameters.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grParameters.Location = new System.Drawing.Point(24, 17);
            this.grParameters.Name = "grParameters";
            this.grParameters.Size = new System.Drawing.Size(463, 151);
            this.grParameters.TabIndex = 14;
            this.grParameters.TabStop = false;
            this.grParameters.Text = "1015_grParameters";
            // 
            // txt_saldos_terceroDesc
            // 
            this.txt_saldos_terceroDesc.Location = new System.Drawing.Point(244, 111);
            this.txt_saldos_terceroDesc.Name = "txt_saldos_terceroDesc";
            this.txt_saldos_terceroDesc.ReadOnly = true;
            this.txt_saldos_terceroDesc.Size = new System.Drawing.Size(192, 22);
            this.txt_saldos_terceroDesc.TabIndex = 8;
            // 
            // txt_saldos_cuentaDesc
            // 
            this.txt_saldos_cuentaDesc.Location = new System.Drawing.Point(244, 82);
            this.txt_saldos_cuentaDesc.Name = "txt_saldos_cuentaDesc";
            this.txt_saldos_cuentaDesc.ReadOnly = true;
            this.txt_saldos_cuentaDesc.Size = new System.Drawing.Size(192, 22);
            this.txt_saldos_cuentaDesc.TabIndex = 7;
            // 
            // lblPeriodTit
            // 
            this.lblPeriodTit.AutoSize = true;
            this.lblPeriodTit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodTit.Location = new System.Drawing.Point(19, 27);
            this.lblPeriodTit.Name = "lblPeriodTit";
            this.lblPeriodTit.Size = new System.Drawing.Size(87, 14);
            this.lblPeriodTit.TabIndex = 0;
            this.lblPeriodTit.Text = "1011_lblPeriod";
            // 
            // uc_Saldos_PerEditPeriodo
            // 
            this.uc_Saldos_PerEditPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Saldos_PerEditPeriodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_Saldos_PerEditPeriodo.EnabledControl = true;
            this.uc_Saldos_PerEditPeriodo.ExtraPeriods = 0;
            this.uc_Saldos_PerEditPeriodo.Location = new System.Drawing.Point(113, 25);
            this.uc_Saldos_PerEditPeriodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Saldos_PerEditPeriodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Saldos_PerEditPeriodo.Name = "uc_Saldos_PerEditPeriodo";
            this.uc_Saldos_PerEditPeriodo.Size = new System.Drawing.Size(140, 20);
            this.uc_Saldos_PerEditPeriodo.TabIndex = 0;
            this.uc_Saldos_PerEditPeriodo.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.uc_Saldos_PerEditPeriodo_ValueChanged);
            // 
            // txt_saldos_tercero
            // 
            this.txt_saldos_tercero.Location = new System.Drawing.Point(113, 111);
            this.txt_saldos_tercero.Name = "txt_saldos_tercero";
            this.txt_saldos_tercero.ReadOnly = true;
            this.txt_saldos_tercero.Size = new System.Drawing.Size(117, 22);
            this.txt_saldos_tercero.TabIndex = 3;
            // 
            // txt_saldos_cuenta
            // 
            this.txt_saldos_cuenta.Location = new System.Drawing.Point(113, 82);
            this.txt_saldos_cuenta.Name = "txt_saldos_cuenta";
            this.txt_saldos_cuenta.ReadOnly = true;
            this.txt_saldos_cuenta.Size = new System.Drawing.Size(117, 22);
            this.txt_saldos_cuenta.TabIndex = 2;
            // 
            // lblTerceroTit
            // 
            this.lblTerceroTit.AutoSize = true;
            this.lblTerceroTit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerceroTit.Location = new System.Drawing.Point(19, 114);
            this.lblTerceroTit.Name = "lblTerceroTit";
            this.lblTerceroTit.Size = new System.Drawing.Size(96, 14);
            this.lblTerceroTit.TabIndex = 2;
            this.lblTerceroTit.Text = "1011_lblTercero";
            // 
            // lblDocTit
            // 
            this.lblDocTit.AutoSize = true;
            this.lblDocTit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTit.Location = new System.Drawing.Point(19, 56);
            this.lblDocTit.Name = "lblDocTit";
            this.lblDocTit.Size = new System.Drawing.Size(74, 14);
            this.lblDocTit.TabIndex = 4;
            this.lblDocTit.Text = "1011_lblDoc";
            // 
            // txt_saldos_document
            // 
            this.txt_saldos_document.Location = new System.Drawing.Point(113, 53);
            this.txt_saldos_document.Name = "txt_saldos_document";
            this.txt_saldos_document.ReadOnly = true;
            this.txt_saldos_document.Size = new System.Drawing.Size(117, 22);
            this.txt_saldos_document.TabIndex = 1;
            // 
            // lblCuentaTit
            // 
            this.lblCuentaTit.AutoSize = true;
            this.lblCuentaTit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaTit.Location = new System.Drawing.Point(19, 85);
            this.lblCuentaTit.Name = "lblCuentaTit";
            this.lblCuentaTit.Size = new System.Drawing.Size(92, 14);
            this.lblCuentaTit.TabIndex = 6;
            this.lblCuentaTit.Text = "1011_lblCuenta";
            // 
            // tp_bitacora
            // 
            this.tp_bitacora.Controls.Add(this.panelControl1);
            this.tp_bitacora.Name = "tp_bitacora";
            this.tp_bitacora.Size = new System.Drawing.Size(835, 517);
            this.tp_bitacora.Text = "1015_tp_bitacora";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcBitacora);
            this.panelControl1.Location = new System.Drawing.Point(19, 22);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(790, 298);
            this.panelControl1.TabIndex = 81;
            // 
            // gcBitacora
            // 
            this.gcBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBitacora.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcBitacora.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcBitacora.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcBitacora.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcBitacora.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcBitacora.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcBitacora.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcBitacora.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcBitacora.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcBitacora.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcBitacora.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcBitacora.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcBitacora.Location = new System.Drawing.Point(2, 2);
            this.gcBitacora.LookAndFeel.SkinName = "Dark Side";
            this.gcBitacora.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcBitacora.MainView = this.gvBitacora;
            this.gcBitacora.Margin = new System.Windows.Forms.Padding(4);
            this.gcBitacora.Name = "gcBitacora";
            this.gcBitacora.Size = new System.Drawing.Size(786, 294);
            this.gcBitacora.TabIndex = 0;
            this.gcBitacora.UseEmbeddedNavigator = true;
            this.gcBitacora.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBitacora});
            // 
            // gvBitacora
            // 
            this.gvBitacora.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBitacora.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvBitacora.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvBitacora.Appearance.Empty.Options.UseBackColor = true;
            this.gvBitacora.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvBitacora.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvBitacora.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBitacora.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvBitacora.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvBitacora.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvBitacora.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBitacora.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvBitacora.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvBitacora.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvBitacora.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvBitacora.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvBitacora.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvBitacora.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvBitacora.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBitacora.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvBitacora.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvBitacora.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvBitacora.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvBitacora.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvBitacora.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvBitacora.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvBitacora.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvBitacora.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBitacora.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvBitacora.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvBitacora.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvBitacora.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBitacora.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvBitacora.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvBitacora.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvBitacora.Appearance.Row.Options.UseBackColor = true;
            this.gvBitacora.Appearance.Row.Options.UseForeColor = true;
            this.gvBitacora.Appearance.Row.Options.UseTextOptions = true;
            this.gvBitacora.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvBitacora.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBitacora.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvBitacora.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvBitacora.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvBitacora.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBitacora.Appearance.VertLine.Options.UseBackColor = true;
            this.gvBitacora.GridControl = this.gcBitacora;
            this.gvBitacora.Name = "gvBitacora";
            this.gvBitacora.OptionsCustomization.AllowColumnMoving = false;
            this.gvBitacora.OptionsCustomization.AllowFilter = false;
            this.gvBitacora.OptionsMenu.EnableColumnMenu = false;
            this.gvBitacora.OptionsMenu.EnableFooterMenu = false;
            this.gvBitacora.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvBitacora.OptionsView.ColumnAutoWidth = false;
            this.gvBitacora.OptionsView.ShowGroupPanel = false;
            this.gvBitacora.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvBitacora.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvBitacora_CustomRowCellEdit);
            this.gvBitacora.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvBitacora_CustomUnboundColumnData);
            // 
            // tp_masInfo
            // 
            this.tp_masInfo.Name = "tp_masInfo";
            this.tp_masInfo.Size = new System.Drawing.Size(835, 517);
            this.tp_masInfo.Text = "1015_tp_masInfo";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editValue4,
            this.editCheck,
            this.linkVer});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editValue4
            // 
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c2";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            this.editValue4.Name = "editValue4";
            // 
            // editCheck
            // 
            this.editCheck.Caption = "";
            this.editCheck.DisplayValueChecked = "True";
            this.editCheck.DisplayValueUnchecked = "False";
            this.editCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editCheck.Name = "editCheck";
            this.editCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // linkVer
            // 
            this.linkVer.Name = "linkVer";
            this.linkVer.SingleClick = true;
            this.linkVer.Click += new System.EventHandler(this.linkVer_Click);
            // 
            // pbProcess
            // 
            this.pbProcess.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbProcess.Location = new System.Drawing.Point(0, 553);
            this.pbProcess.Name = "pbProcess";
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(866, 18);
            this.pbProcess.TabIndex = 1;
            // 
            // ShowDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 571);
            this.Controls.Add(this.pbProcess);
            this.Controls.Add(this.tc_GetDocument);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowDocumentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowDocumentForm";
            ((System.ComponentModel.ISupportInitialize)(this.tc_GetDocument)).EndInit();
            this.tc_GetDocument.ResumeLayout(false);
            this.tp_document.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlDocument.ResumeLayout(false);
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.gb2.ResumeLayout(false);
            this.gb2.PerformLayout();
            this.gb3.ResumeLayout(false);
            this.gb3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambioCONT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambioDOCU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            this.gb4.ResumeLayout(false);
            this.gb4.PerformLayout();
            this.gb5.ResumeLayout(false);
            this.gb5.PerformLayout();
            this.gb6.ResumeLayout(false);
            this.gb6.PerformLayout();
            this.tp_comprobante.ResumeLayout(false);
            this.gbDescripcion.ResumeLayout(false);
            this.gbDescripcion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtConceptoDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerceroDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentroDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProyectoDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineaDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGridComp)).EndInit();
            this.pnGridComp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComprobante)).EndInit();
            this.gb_Header.ResumeLayout(false);
            this.gb_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comp_TCOtr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_comp_TCBase.Properties)).EndInit();
            this.tp_anexos.ResumeLayout(false);
            this.tp_anexos.PerformLayout();
            this.pnlAnexos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAnexos)).EndInit();
            this.tp_saldos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcSaldosDetail)).EndInit();
            this.pcSaldosDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcSaldosDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSaldosDetail)).EndInit();
            this.grSaldos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnSaldos)).EndInit();
            this.pnSaldos.ResumeLayout(false);
            this.tlpSaldos.ResumeLayout(false);
            this.tlpSaldos.PerformLayout();
            this.grParameters.ResumeLayout(false);
            this.grParameters.PerformLayout();
            this.tp_bitacora.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBitacora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tc_GetDocument;
        private DevExpress.XtraTab.XtraTabPage tp_document;
        private DevExpress.XtraTab.XtraTabPage tp_comprobante;
        private DevExpress.XtraTab.XtraTabPage tp_detalle;
        private DevExpress.XtraTab.XtraTabPage tp_anexos;
        private DevExpress.XtraTab.XtraTabPage tp_saldos;
        private DevExpress.XtraTab.XtraTabPage tp_bitacora;
        private DevExpress.XtraTab.XtraTabPage tp_masInfo;
        private System.Windows.Forms.Button btnPdf;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlDocument;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterPrefixId;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterAreaFuncional;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterDocumentID;
        private System.Windows.Forms.GroupBox gb3;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasteModena;
        private DevExpress.XtraEditors.LabelControl lblTasaCambioDOCU;
        private DevExpress.XtraEditors.LabelControl lblTasaCambioCONT;
        private System.Windows.Forms.GroupBox gb5;
        private DevExpress.XtraEditors.LabelControl lblDocumentoTercero;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.LabelControl lblseUsuarioID;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private DevExpress.XtraEditors.LabelControl lblComprobanteIDNro;
        private DevExpress.XtraEditors.LabelControl lblDocumentoNro;
        private DevExpress.XtraEditors.LabelControl lblPeriodoDoc;
        private DevExpress.XtraEditors.LabelControl lblFecha;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterComprobante;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterProject;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterCuenta;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterTercero;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterCentroCosto;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterLineaPresupuesto;
        private ControlsUC.uc_MasterFind uc_DocCtrl_MasterLugarGeografico;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.TextBox txtFechaDoc;
        private System.Windows.Forms.TextBox txtPeriodoDoc;
        private System.Windows.Forms.TextBox txtDocumentoNro;
        private System.Windows.Forms.TextBox txtComprobanteIDNro;
        private System.Windows.Forms.TextBox txtDocumentoTercero;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.TextBox txtseUsuarioID;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPeriodTit;
        private System.Windows.Forms.Label lblDocTit;
        private System.Windows.Forms.TextBox txt_saldos_document;
        private ControlsUC.uc_PeriodoEdit uc_Saldos_PerEditPeriodo;
        private ControlsUC.uc_MasterFind uc_Comp_MasterComprobante;
        private System.Windows.Forms.GroupBox gb_Header;
        private DevExpress.XtraEditors.TextEdit txt_comp_TCBase;
        private DevExpress.XtraEditors.LabelControl lblTasaCambioBase;
        private ControlsUC.uc_MasterFind uc_Comp_MasterMoneda;
        private System.Windows.Forms.TextBox txt_comp_MdaOrigen;
        private DevExpress.XtraEditors.LabelControl lblMdaOrigen;
        private System.Windows.Forms.TextBox txt_comp_fecha;
        private DevExpress.XtraEditors.LabelControl lblFechaComp;
        private System.Windows.Forms.TextBox txt_comp_NroComp;
        private DevExpress.XtraEditors.LabelControl lblComprobanteNro;
        private System.Windows.Forms.TextBox txt_comp_Periodo;
        private DevExpress.XtraEditors.LabelControl lblPeriodoDocComp;
        private DevExpress.XtraEditors.TextEdit txt_comp_TCOtr;
        private DevExpress.XtraEditors.LabelControl lblTasaCambioOtra;
        private DevExpress.XtraEditors.PanelControl pnGridComp;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        private DevExpress.XtraEditors.TextEdit txtTasaCambioCONT;
        private DevExpress.XtraEditors.TextEdit txtTasaCambioDOCU;
        private System.Windows.Forms.GroupBox gb2;
        private System.Windows.Forms.GroupBox gb4;
        private System.Windows.Forms.GroupBox grParameters;
        private System.Windows.Forms.GroupBox gb6;
        private System.Windows.Forms.GroupBox gb1;
        private DevExpress.XtraGrid.GridControl gcComprobante;
        private DevExpress.XtraGrid.Views.Grid.GridView gvComprobante;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcBitacora;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBitacora;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        protected DevExpress.XtraEditors.ProgressBarControl pbProcess;
        private System.Windows.Forms.TextBox txt_saldos_terceroDesc;
        private System.Windows.Forms.TextBox txt_saldos_cuentaDesc;
        private System.Windows.Forms.TextBox txt_saldos_tercero;
        private System.Windows.Forms.TextBox txt_saldos_cuenta;
        private System.Windows.Forms.Label lblTerceroTit;
        private System.Windows.Forms.Label lblCuentaTit;
        private System.Windows.Forms.GroupBox grSaldos;
        private System.Windows.Forms.TableLayoutPanel tlpSaldos;
        private System.Windows.Forms.Label lblMdaLoc;
        private System.Windows.Forms.Label lblMdaExt;
        private DevExpress.XtraEditors.PanelControl pnSaldos;
        private System.Windows.Forms.TextBox txtSaldoML;
        private System.Windows.Forms.TextBox txtSaldoME;
        private System.Windows.Forms.Label lblMdaLocName;
        private System.Windows.Forms.Label lblMdaExtName;
        private DevExpress.XtraEditors.PanelControl pcSaldosDetail;
        private DevExpress.XtraGrid.GridControl gcSaldosDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSaldosDetail;
        private System.Windows.Forms.Panel pnlAnexos;
        private DevExpress.XtraGrid.GridControl gcAnexos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAnexos;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkVer;
        private System.Windows.Forms.Button btnRegenPdf;
        private System.Windows.Forms.Label lblLineaPresDesc;
        private DevExpress.XtraEditors.TextEdit txtLineaDesc;
        private System.Windows.Forms.Label lblProyectoDesc;
        private DevExpress.XtraEditors.TextEdit txtProyectoDesc;
        private System.Windows.Forms.Label lblTerceroDesc;
        private DevExpress.XtraEditors.TextEdit txtTerceroDesc;
        private System.Windows.Forms.Label lblcuentaDesc;
        private DevExpress.XtraEditors.TextEdit txtCuentaDesc;
        private System.Windows.Forms.GroupBox gbDescripcion;
        private System.Windows.Forms.Label lblConceptoDesc;
        private DevExpress.XtraEditors.TextEdit txtConceptoDesc;
        private System.Windows.Forms.Label lblCentroCtoDesc;
        private DevExpress.XtraEditors.TextEdit txtCentroDesc;
        private System.Windows.Forms.LinkLabel lblInfoAnexo;
        private System.Windows.Forms.Button btnEditAnexo;
        private DevExpress.XtraEditors.SimpleButton btnUpdateAnexos;
        private DevExpress.XtraEditors.LabelControl lblNumeroDoc;
        private System.Windows.Forms.TextBox txtNumeroDoc;
        private DevExpress.XtraEditors.TextEdit txtIVA;
        private DevExpress.XtraEditors.LabelControl lblIVA;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.LabelControl lblValor;
        private DevExpress.XtraEditors.LabelControl lblNumDocPadre;
        private System.Windows.Forms.TextBox txtNumDocPadre;
        private DevExpress.XtraEditors.LabelControl lblFechaCreacion;
        private System.Windows.Forms.TextBox txtFechaCreacion;
    }
}