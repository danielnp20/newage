using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PlaneacionCostosAPUCliente
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaneacionCostosAPUCliente));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalleRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecurso = new DevExpress.XtraGrid.GridControl();
            this.gvRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.pn2 = new System.Windows.Forms.Panel();
            this.chkMultipActivo = new DevExpress.XtraEditors.CheckEdit();
            this.chkPersonalCant = new DevExpress.XtraEditors.CheckEdit();
            this.chkEquipoCant = new DevExpress.XtraEditors.CheckEdit();
            this.lblRedondeo = new DevExpress.XtraEditors.LabelControl();
            this.cmbRedondeo = new DevExpress.XtraEditors.LookUpEdit();
            this.btnCopyAPU = new DevExpress.XtraEditors.SimpleButton();
            this.lblProposito = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoSolicitud = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoPresup = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkRecursoXTrabInd = new DevExpress.XtraEditors.CheckEdit();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl = new DevExpress.XtraEditors.LabelControl();
            this.lblMultipl = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaInicio = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.txtMultiplicador = new DevExpress.XtraEditors.TextEdit();
            this.chkAPUIncluyeAIU = new DevExpress.XtraEditors.CheckEdit();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoExEdit();
            this.lblReporte = new DevExpress.XtraEditors.LabelControl();
            this.cmbReporte = new DevExpress.XtraEditors.LookUpEdit();
            this.pnlAIU = new DevExpress.XtraEditors.GroupControl();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.txtPorUtilEmp = new DevExpress.XtraEditors.TextEdit();
            this.txtPorImprEmp = new DevExpress.XtraEditors.TextEdit();
            this.txtPorAdmEmp = new DevExpress.XtraEditors.TextEdit();
            this.txtPorUtilClient = new DevExpress.XtraEditors.TextEdit();
            this.txtPorImprClient = new DevExpress.XtraEditors.TextEdit();
            this.lblUtil = new System.Windows.Forms.Label();
            this.lblImpr = new System.Windows.Forms.Label();
            this.lblADM = new System.Windows.Forms.Label();
            this.txtPorAdmClient = new DevExpress.XtraEditors.TextEdit();
            this.lblObservaciones = new DevExpress.XtraEditors.LabelControl();
            this.lblJerarquia = new DevExpress.XtraEditors.LabelControl();
            this.cmbJerarquia = new DevExpress.XtraEditors.LookUpEdit();
            this.lblResponableEmp = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.cmbProposito = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoSolicitud = new DevExpress.XtraEditors.LabelControl();
            this.lblSolicitante = new DevExpress.XtraEditors.LabelControl();
            this.masterResponsableEmp = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoExEdit();
            this.txtObservaciones = new DevExpress.XtraEditors.MemoExEdit();
            this.txtSolicitante = new DevExpress.XtraEditors.MemoExEdit();
            this.pn1 = new DevExpress.XtraEditors.PanelControl();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterClaseServicio = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtAF = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblAF = new DevExpress.XtraEditors.LabelControl();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblPrefix = new DevExpress.XtraEditors.LabelControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.chkUserEdit = new DevExpress.XtraEditors.CheckEdit();
            this.lblUsuario = new DevExpress.XtraEditors.LabelControl();
            this.pnResumenCostos = new DevExpress.XtraEditors.PanelControl();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCostoMult = new DevExpress.XtraEditors.TextEdit();
            this.txtOtros = new DevExpress.XtraEditors.TextEdit();
            this.btnVlrAdicional = new DevExpress.XtraEditors.SimpleButton();
            this.txtPorIVA = new DevExpress.XtraEditors.TextEdit();
            this.lblIVA = new System.Windows.Forms.Label();
            this.txtIVA = new DevExpress.XtraEditors.TextEdit();
            this.lblOtros = new System.Windows.Forms.Label();
            this.lblCostoCliente = new System.Windows.Forms.Label();
            this.txtCostoCliente = new DevExpress.XtraEditors.TextEdit();
            this.lblCostoPresupuesto = new System.Windows.Forms.Label();
            this.txtCostoPresupuesto = new DevExpress.XtraEditors.TextEdit();
            this.masterUsuarioPermiso = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.grpCtrlRecurso = new DevExpress.XtraEditors.GroupControl();
            this.txtCostoTotalAPU = new DevExpress.XtraEditors.TextEdit();
            this.lblAPU = new System.Windows.Forms.Label();
            this.txtCostoAIUxAPU = new DevExpress.XtraEditors.TextEdit();
            this.btnExportRecursos = new DevExpress.XtraEditors.SimpleButton();
            this.lblCostoAIU = new System.Windows.Forms.Label();
            this.btnImportRecursos = new DevExpress.XtraEditors.SimpleButton();
            this.lblCostoAPU = new System.Windows.Forms.Label();
            this.txtCostoAPU = new DevExpress.XtraEditors.TextEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin7 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue8Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.pn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkMultipActivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPersonalCant.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEquipoCant.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRedondeo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPresup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMultiplicador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAPUIncluyeAIU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAIU)).BeginInit();
            this.pnlAIU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJerarquia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pn1)).BeginInit();
            this.pn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkUserEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnResumenCostos)).BeginInit();
            this.pnResumenCostos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoMult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtros.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoCliente.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoPresupuesto.Properties)).BeginInit();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlRecurso)).BeginInit();
            this.grpCtrlRecurso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoTotalAPU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoAIUxAPU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoAPU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue8Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Empty.Options.UseFont = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.GridControl = this.gcDocument;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsBehavior.Editable = false;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDocument_EmbeddedNavigator_ButtonClick);
            this.gcDocument.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(0, 0);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(907, 394);
            this.gcDocument.TabIndex = 50;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocument.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocument.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocument.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocument.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.GroupRow.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.GroupRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocument.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocument.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseFont = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.GroupFormat = "{1} {2}";
            this.gvDocument.HorzScrollStep = 50;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsDetail.EnableMasterViewMode = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowAutoFilterRow = true;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDocument_RowCellStyle);
            this.gvDocument.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvDocument_RowStyle);
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocument_CellValueChanged);
            this.gvDocument.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocument_CellValueChanging);
            this.gvDocument.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocument_BeforeLeaveRow);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocument.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocument_CustomColumnDisplayText);
            // 
            // gvDetalleRecurso
            // 
            this.gvDetalleRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleRecurso.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalleRecurso.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleRecurso.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.Empty.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalleRecurso.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleRecurso.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleRecurso.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleRecurso.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalleRecurso.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleRecurso.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvDetalleRecurso.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvDetalleRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvDetalleRecurso.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvDetalleRecurso.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalleRecurso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleRecurso.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalleRecurso.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalleRecurso.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalleRecurso.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleRecurso.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalleRecurso.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalleRecurso.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDetalleRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleRecurso.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.Row.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalleRecurso.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalleRecurso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalleRecurso.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalleRecurso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalleRecurso.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleRecurso.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalleRecurso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalleRecurso.GridControl = this.gcRecurso;
            this.gvDetalleRecurso.GroupFormat = "[#image]{1} {2}";
            this.gvDetalleRecurso.HorzScrollStep = 50;
            this.gvDetalleRecurso.Name = "gvDetalleRecurso";
            this.gvDetalleRecurso.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalleRecurso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalleRecurso.OptionsBehavior.Editable = false;
            this.gvDetalleRecurso.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalleRecurso.OptionsCustomization.AllowFilter = false;
            this.gvDetalleRecurso.OptionsCustomization.AllowSort = false;
            this.gvDetalleRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalleRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalleRecurso.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalleRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvDetalleRecurso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalleRecurso.OptionsView.ShowGroupPanel = false;
            this.gvDetalleRecurso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            // 
            // gcRecurso
            // 
            this.gcRecurso.AllowDrop = true;
            this.gcRecurso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRecurso.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRecurso.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcRecurso.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcRecurso.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcRecurso.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.gcRecurso.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecurso.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcRecurso_EmbeddedNavigator_ButtonClick);
            this.gcRecurso.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode2.LevelTemplate = this.gvDetalleRecurso;
            gridLevelNode2.RelationName = "Detalle";
            this.gcRecurso.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcRecurso.Location = new System.Drawing.Point(7, 76);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(645, 312);
            this.gcRecurso.TabIndex = 51;
            this.gcRecurso.UseEmbeddedNavigator = true;
            this.gcRecurso.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecurso,
            this.gvDetalleRecurso});
            // 
            // gvRecurso
            // 
            this.gvRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRecurso.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRecurso.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecurso.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.FooterPanel.Font = new System.Drawing.Font("Arial Narrow", 7.8F);
            this.gvRecurso.Appearance.FooterPanel.Options.UseFont = true;
            this.gvRecurso.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvRecurso.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupFooter.Image = ((System.Drawing.Image)(resources.GetObject("gvRecurso.Appearance.GroupFooter.Image")));
            this.gvRecurso.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvRecurso.Appearance.GroupFooter.Options.UseImage = true;
            this.gvRecurso.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvRecurso.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvRecurso.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecurso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRecurso.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.Row.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.Options.UseFont = true;
            this.gvRecurso.Appearance.Row.Options.UseForeColor = true;
            this.gvRecurso.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecurso.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecurso.GridControl = this.gcRecurso;
            this.gvRecurso.GroupFormat = "[#image]{1} {2}";
            this.gvRecurso.GroupRowHeight = 25;
            this.gvRecurso.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_CostoLocalTOT", null, "(SubTotal={0:c2})")});
            this.gvRecurso.HorzScrollStep = 50;
            this.gvRecurso.Name = "gvRecurso";
            this.gvRecurso.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvRecurso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecurso.OptionsCustomization.AllowFilter = false;
            this.gvRecurso.OptionsCustomization.AllowSort = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvRecurso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecurso.OptionsView.ShowGroupPanel = false;
            this.gvRecurso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecurso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
            this.gvRecurso.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvRecurso_CellValueChanged);
            this.gvRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvRecurso.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvRecurso_CustomColumnDisplayText);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(9);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1607, 734);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.94391F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.05609F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1603, 730);
            this.tlSeparatorPanel.TabIndex = 54;
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
            this.grpctrlHeader.Controls.Add(this.pn2);
            this.grpctrlHeader.Controls.Add(this.pn1);
            this.grpctrlHeader.Controls.Add(this.txtAF);
            this.grpctrlHeader.Controls.Add(this.txtPrefix);
            this.grpctrlHeader.Controls.Add(this.dtPeriod);
            this.grpctrlHeader.Controls.Add(this.lblAF);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.txtNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblPrefix);
            this.grpctrlHeader.Controls.Add(this.lblDate);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Controls.Add(this.dtFecha);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(17, 4);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1571, 228);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // pn2
            // 
            this.pn2.Controls.Add(this.chkMultipActivo);
            this.pn2.Controls.Add(this.chkPersonalCant);
            this.pn2.Controls.Add(this.chkEquipoCant);
            this.pn2.Controls.Add(this.lblRedondeo);
            this.pn2.Controls.Add(this.cmbRedondeo);
            this.pn2.Controls.Add(this.btnCopyAPU);
            this.pn2.Controls.Add(this.lblProposito);
            this.pn2.Controls.Add(this.cmbTipoSolicitud);
            this.pn2.Controls.Add(this.labelControl2);
            this.pn2.Controls.Add(this.txtTasaCambio);
            this.pn2.Controls.Add(this.cmbTipoPresup);
            this.pn2.Controls.Add(this.labelControl1);
            this.pn2.Controls.Add(this.chkRecursoXTrabInd);
            this.pn2.Controls.Add(this.masterProyecto);
            this.pn2.Controls.Add(this.masterCentroCto);
            this.pn2.Controls.Add(this.lbl);
            this.pn2.Controls.Add(this.lblMultipl);
            this.pn2.Controls.Add(this.lblFechaInicio);
            this.pn2.Controls.Add(this.dtFechaInicio);
            this.pn2.Controls.Add(this.txtMultiplicador);
            this.pn2.Controls.Add(this.chkAPUIncluyeAIU);
            this.pn2.Controls.Add(this.lblLicitacion);
            this.pn2.Controls.Add(this.txtLicitacion);
            this.pn2.Controls.Add(this.lblReporte);
            this.pn2.Controls.Add(this.cmbReporte);
            this.pn2.Controls.Add(this.pnlAIU);
            this.pn2.Controls.Add(this.lblObservaciones);
            this.pn2.Controls.Add(this.lblJerarquia);
            this.pn2.Controls.Add(this.cmbJerarquia);
            this.pn2.Controls.Add(this.lblResponableEmp);
            this.pn2.Controls.Add(this.masterCliente);
            this.pn2.Controls.Add(this.lblDescripcion);
            this.pn2.Controls.Add(this.cmbProposito);
            this.pn2.Controls.Add(this.lblTipoSolicitud);
            this.pn2.Controls.Add(this.lblSolicitante);
            this.pn2.Controls.Add(this.masterResponsableEmp);
            this.pn2.Controls.Add(this.txtDescripcion);
            this.pn2.Controls.Add(this.txtObservaciones);
            this.pn2.Controls.Add(this.txtSolicitante);
            this.pn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn2.Location = new System.Drawing.Point(2, 59);
            this.pn2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pn2.Name = "pn2";
            this.pn2.Size = new System.Drawing.Size(1567, 167);
            this.pn2.TabIndex = 72;
            // 
            // chkMultipActivo
            // 
            this.chkMultipActivo.EditValue = true;
            this.chkMultipActivo.Location = new System.Drawing.Point(1392, 113);
            this.chkMultipActivo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkMultipActivo.Name = "chkMultipActivo";
            this.chkMultipActivo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7F);
            this.chkMultipActivo.Properties.Appearance.Options.UseFont = true;
            this.chkMultipActivo.Properties.AutoWidth = true;
            this.chkMultipActivo.Properties.Caption = "110_chkMultipActivo";
            this.chkMultipActivo.Size = new System.Drawing.Size(135, 19);
            this.chkMultipActivo.TabIndex = 106;
            this.chkMultipActivo.CheckedChanged += new System.EventHandler(this.chkMultipActivo_CheckedChanged);
            // 
            // chkPersonalCant
            // 
            this.chkPersonalCant.Location = new System.Drawing.Point(1388, 60);
            this.chkPersonalCant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkPersonalCant.Name = "chkPersonalCant";
            this.chkPersonalCant.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.chkPersonalCant.Properties.Appearance.Options.UseFont = true;
            this.chkPersonalCant.Properties.AutoWidth = true;
            this.chkPersonalCant.Properties.Caption = "110_chkPersonalCant";
            this.chkPersonalCant.Size = new System.Drawing.Size(139, 19);
            this.chkPersonalCant.TabIndex = 104;
            this.chkPersonalCant.CheckedChanged += new System.EventHandler(this.chkPersonalCant_CheckedChanged);
            // 
            // chkEquipoCant
            // 
            this.chkEquipoCant.Location = new System.Drawing.Point(1389, 87);
            this.chkEquipoCant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkEquipoCant.Name = "chkEquipoCant";
            this.chkEquipoCant.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.chkEquipoCant.Properties.Appearance.Options.UseFont = true;
            this.chkEquipoCant.Properties.AutoWidth = true;
            this.chkEquipoCant.Properties.Caption = "110_chkEquipoCant";
            this.chkEquipoCant.Size = new System.Drawing.Size(131, 19);
            this.chkEquipoCant.TabIndex = 103;
            this.chkEquipoCant.CheckedChanged += new System.EventHandler(this.chkEquipoCant_CheckedChanged);
            // 
            // lblRedondeo
            // 
            this.lblRedondeo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblRedondeo.Location = new System.Drawing.Point(1391, 38);
            this.lblRedondeo.Margin = new System.Windows.Forms.Padding(4);
            this.lblRedondeo.Name = "lblRedondeo";
            this.lblRedondeo.Size = new System.Drawing.Size(63, 17);
            this.lblRedondeo.TabIndex = 102;
            this.lblRedondeo.Text = "Redondeo";
            // 
            // cmbRedondeo
            // 
            this.cmbRedondeo.Location = new System.Drawing.Point(1459, 33);
            this.cmbRedondeo.Margin = new System.Windows.Forms.Padding(4);
            this.cmbRedondeo.Name = "cmbRedondeo";
            this.cmbRedondeo.Properties.Appearance.Options.UseFont = true;
            this.cmbRedondeo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRedondeo.Size = new System.Drawing.Size(93, 22);
            this.cmbRedondeo.TabIndex = 101;
            this.cmbRedondeo.EditValueChanged += new System.EventHandler(this.cmbRedondeo_EditValueChanged);
            // 
            // btnCopyAPU
            // 
            this.btnCopyAPU.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnCopyAPU.Appearance.Options.UseFont = true;
            this.btnCopyAPU.Location = new System.Drawing.Point(1212, 121);
            this.btnCopyAPU.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopyAPU.Name = "btnCopyAPU";
            this.btnCopyAPU.Size = new System.Drawing.Size(172, 27);
            this.btnCopyAPU.TabIndex = 98;
            this.btnCopyAPU.Text = "33512_btnCopyAPU";
            this.btnCopyAPU.Click += new System.EventHandler(this.btnCopyAPU_Click);
            // 
            // lblProposito
            // 
            this.lblProposito.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblProposito.Location = new System.Drawing.Point(426, 121);
            this.lblProposito.Margin = new System.Windows.Forms.Padding(4);
            this.lblProposito.Name = "lblProposito";
            this.lblProposito.Size = new System.Drawing.Size(102, 17);
            this.lblProposito.TabIndex = 97;
            this.lblProposito.Text = "110_lblProposito";
            // 
            // cmbTipoSolicitud
            // 
            this.cmbTipoSolicitud.Location = new System.Drawing.Point(129, 91);
            this.cmbTipoSolicitud.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTipoSolicitud.Name = "cmbTipoSolicitud";
            this.cmbTipoSolicitud.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoSolicitud.Size = new System.Drawing.Size(149, 22);
            this.cmbTipoSolicitud.TabIndex = 96;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl2.Location = new System.Drawing.Point(689, 122);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(118, 17);
            this.labelControl2.TabIndex = 94;
            this.labelControl2.Text = "110_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0,00 ";
            this.txtTasaCambio.Location = new System.Drawing.Point(803, 117);
            this.txtTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTasaCambio.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTasaCambio.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTasaCambio.Properties.Appearance.Options.UseFont = true;
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.AutoHeight = false;
            this.txtTasaCambio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(131, 20);
            this.txtTasaCambio.TabIndex = 93;
            // 
            // cmbTipoPresup
            // 
            this.cmbTipoPresup.Location = new System.Drawing.Point(1091, 117);
            this.cmbTipoPresup.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTipoPresup.Name = "cmbTipoPresup";
            this.cmbTipoPresup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoPresup.Properties.ReadOnly = true;
            this.cmbTipoPresup.Size = new System.Drawing.Size(111, 22);
            this.cmbTipoPresup.TabIndex = 91;
            this.cmbTipoPresup.EditValueChanged += new System.EventHandler(this.cmbTipoPresup_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl1.Location = new System.Drawing.Point(957, 122);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(161, 17);
            this.labelControl1.TabIndex = 90;
            this.labelControl1.Text = "110_cmbTipoPresupuesto";
            // 
            // chkRecursoXTrabInd
            // 
            this.chkRecursoXTrabInd.Location = new System.Drawing.Point(777, 39);
            this.chkRecursoXTrabInd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkRecursoXTrabInd.Name = "chkRecursoXTrabInd";
            this.chkRecursoXTrabInd.Properties.AutoWidth = true;
            this.chkRecursoXTrabInd.Properties.Caption = "110_chkRecursoXTrab";
            this.chkRecursoXTrabInd.Size = new System.Drawing.Size(150, 20);
            this.chkRecursoXTrabInd.TabIndex = 70;
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(13, 32);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(5);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(400, 26);
            this.masterProyecto.TabIndex = 6;
            this.masterProyecto.Value = "";
            // 
            // masterCentroCto
            // 
            this.masterCentroCto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCto.Filtros = null;
            this.masterCentroCto.Location = new System.Drawing.Point(12, 60);
            this.masterCentroCto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterCentroCto.Name = "masterCentroCto";
            this.masterCentroCto.Size = new System.Drawing.Size(404, 26);
            this.masterCentroCto.TabIndex = 84;
            this.masterCentroCto.Value = "";
            // 
            // lbl
            // 
            this.lbl.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.lbl.Location = new System.Drawing.Point(1116, 94);
            this.lbl.Margin = new System.Windows.Forms.Padding(4);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(11, 16);
            this.lbl.TabIndex = 88;
            this.lbl.Text = "%";
            // 
            // lblMultipl
            // 
            this.lblMultipl.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblMultipl.Location = new System.Drawing.Point(957, 96);
            this.lblMultipl.Margin = new System.Windows.Forms.Padding(4);
            this.lblMultipl.Name = "lblMultipl";
            this.lblMultipl.Size = new System.Drawing.Size(118, 17);
            this.lblMultipl.TabIndex = 87;
            this.lblMultipl.Text = "110_lblMultiplicador";
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblFechaInicio.Location = new System.Drawing.Point(957, 69);
            this.lblFechaInicio.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(111, 17);
            this.lblFechaInicio.TabIndex = 86;
            this.lblFechaInicio.Text = "110_lblFechaInicio";
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(1073, 65);
            this.dtFechaInicio.Margin = new System.Windows.Forms.Padding(4);
            this.dtFechaInicio.Name = "dtFechaInicio";
            this.dtFechaInicio.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicio.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicio.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicio.Size = new System.Drawing.Size(128, 22);
            this.dtFechaInicio.TabIndex = 85;
            // 
            // txtMultiplicador
            // 
            this.txtMultiplicador.EditValue = "100.000";
            this.txtMultiplicador.Location = new System.Drawing.Point(1131, 91);
            this.txtMultiplicador.Margin = new System.Windows.Forms.Padding(4);
            this.txtMultiplicador.Name = "txtMultiplicador";
            this.txtMultiplicador.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtMultiplicador.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtMultiplicador.Properties.Appearance.Options.UseBorderColor = true;
            this.txtMultiplicador.Properties.Appearance.Options.UseFont = true;
            this.txtMultiplicador.Properties.Appearance.Options.UseTextOptions = true;
            this.txtMultiplicador.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtMultiplicador.Properties.AutoHeight = false;
            this.txtMultiplicador.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtMultiplicador.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtMultiplicador.Properties.Mask.EditMask = "n3";
            this.txtMultiplicador.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtMultiplicador.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtMultiplicador.Size = new System.Drawing.Size(71, 19);
            this.txtMultiplicador.TabIndex = 83;
            this.txtMultiplicador.EditValueChanged += new System.EventHandler(this.txtMultiplicador_EditValueChanged);
            // 
            // chkAPUIncluyeAIU
            // 
            this.chkAPUIncluyeAIU.Location = new System.Drawing.Point(777, 65);
            this.chkAPUIncluyeAIU.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAPUIncluyeAIU.Name = "chkAPUIncluyeAIU";
            this.chkAPUIncluyeAIU.Properties.AutoWidth = true;
            this.chkAPUIncluyeAIU.Properties.Caption = "110_chkAPUIncluyeAIU";
            this.chkAPUIncluyeAIU.Size = new System.Drawing.Size(152, 20);
            this.chkAPUIncluyeAIU.TabIndex = 78;
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(423, 41);
            this.lblLicitacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(99, 17);
            this.lblLicitacion.TabIndex = 77;
            this.lblLicitacion.Text = "110_lblLicitacion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(559, 37);
            this.txtLicitacion.Margin = new System.Windows.Forms.Padding(4);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLicitacion.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.txtLicitacion.Properties.ShowIcon = false;
            this.txtLicitacion.Size = new System.Drawing.Size(212, 22);
            this.txtLicitacion.TabIndex = 76;
            this.txtLicitacion.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // lblReporte
            // 
            this.lblReporte.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblReporte.Location = new System.Drawing.Point(957, 42);
            this.lblReporte.Margin = new System.Windows.Forms.Padding(4);
            this.lblReporte.Name = "lblReporte";
            this.lblReporte.Size = new System.Drawing.Size(93, 17);
            this.lblReporte.TabIndex = 75;
            this.lblReporte.Text = "110_lblReporte";
            // 
            // cmbReporte
            // 
            this.cmbReporte.Location = new System.Drawing.Point(1060, 38);
            this.cmbReporte.Margin = new System.Windows.Forms.Padding(4);
            this.cmbReporte.Name = "cmbReporte";
            this.cmbReporte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbReporte.Size = new System.Drawing.Size(141, 22);
            this.cmbReporte.TabIndex = 74;
            // 
            // pnlAIU
            // 
            this.pnlAIU.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.pnlAIU.AppearanceCaption.Options.UseFont = true;
            this.pnlAIU.Controls.Add(this.lblEmpresa);
            this.pnlAIU.Controls.Add(this.lblCliente);
            this.pnlAIU.Controls.Add(this.txtPorUtilEmp);
            this.pnlAIU.Controls.Add(this.txtPorImprEmp);
            this.pnlAIU.Controls.Add(this.txtPorAdmEmp);
            this.pnlAIU.Controls.Add(this.txtPorUtilClient);
            this.pnlAIU.Controls.Add(this.txtPorImprClient);
            this.pnlAIU.Controls.Add(this.lblUtil);
            this.pnlAIU.Controls.Add(this.lblImpr);
            this.pnlAIU.Controls.Add(this.lblADM);
            this.pnlAIU.Controls.Add(this.txtPorAdmClient);
            this.pnlAIU.Location = new System.Drawing.Point(1212, 12);
            this.pnlAIU.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAIU.Name = "pnlAIU";
            this.pnlAIU.Size = new System.Drawing.Size(172, 105);
            this.pnlAIU.TabIndex = 73;
            this.pnlAIU.Text = "A.I.U.";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.BackColor = System.Drawing.Color.Transparent;
            this.lblEmpresa.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresa.Location = new System.Drawing.Point(111, 2);
            this.lblEmpresa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(59, 20);
            this.lblEmpresa.TabIndex = 83;
            this.lblEmpresa.Text = "Empresa";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.BackColor = System.Drawing.Color.Transparent;
            this.lblCliente.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.Location = new System.Drawing.Point(64, 2);
            this.lblCliente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(48, 20);
            this.lblCliente.TabIndex = 82;
            this.lblCliente.Text = "Cliente";
            // 
            // txtPorUtilEmp
            // 
            this.txtPorUtilEmp.EditValue = "0,00 ";
            this.txtPorUtilEmp.Location = new System.Drawing.Point(121, 76);
            this.txtPorUtilEmp.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorUtilEmp.Name = "txtPorUtilEmp";
            this.txtPorUtilEmp.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorUtilEmp.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorUtilEmp.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorUtilEmp.Properties.Appearance.Options.UseFont = true;
            this.txtPorUtilEmp.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorUtilEmp.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorUtilEmp.Properties.AutoHeight = false;
            this.txtPorUtilEmp.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorUtilEmp.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorUtilEmp.Properties.Mask.EditMask = "P0";
            this.txtPorUtilEmp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorUtilEmp.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorUtilEmp.Size = new System.Drawing.Size(37, 20);
            this.txtPorUtilEmp.TabIndex = 81;
            // 
            // txtPorImprEmp
            // 
            this.txtPorImprEmp.EditValue = "0,00 ";
            this.txtPorImprEmp.Location = new System.Drawing.Point(121, 50);
            this.txtPorImprEmp.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorImprEmp.Name = "txtPorImprEmp";
            this.txtPorImprEmp.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorImprEmp.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorImprEmp.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorImprEmp.Properties.Appearance.Options.UseFont = true;
            this.txtPorImprEmp.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorImprEmp.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorImprEmp.Properties.AutoHeight = false;
            this.txtPorImprEmp.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorImprEmp.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorImprEmp.Properties.Mask.EditMask = "P0";
            this.txtPorImprEmp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorImprEmp.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorImprEmp.Size = new System.Drawing.Size(37, 20);
            this.txtPorImprEmp.TabIndex = 80;
            // 
            // txtPorAdmEmp
            // 
            this.txtPorAdmEmp.EditValue = "0,00 ";
            this.txtPorAdmEmp.Location = new System.Drawing.Point(121, 25);
            this.txtPorAdmEmp.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorAdmEmp.Name = "txtPorAdmEmp";
            this.txtPorAdmEmp.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorAdmEmp.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorAdmEmp.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorAdmEmp.Properties.Appearance.Options.UseFont = true;
            this.txtPorAdmEmp.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorAdmEmp.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorAdmEmp.Properties.AutoHeight = false;
            this.txtPorAdmEmp.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorAdmEmp.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorAdmEmp.Properties.Mask.EditMask = "P0";
            this.txtPorAdmEmp.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorAdmEmp.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorAdmEmp.Size = new System.Drawing.Size(37, 20);
            this.txtPorAdmEmp.TabIndex = 79;
            // 
            // txtPorUtilClient
            // 
            this.txtPorUtilClient.EditValue = "0,00 ";
            this.txtPorUtilClient.Location = new System.Drawing.Point(72, 76);
            this.txtPorUtilClient.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorUtilClient.Name = "txtPorUtilClient";
            this.txtPorUtilClient.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorUtilClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorUtilClient.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorUtilClient.Properties.Appearance.Options.UseFont = true;
            this.txtPorUtilClient.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorUtilClient.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorUtilClient.Properties.AutoHeight = false;
            this.txtPorUtilClient.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorUtilClient.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorUtilClient.Properties.Mask.EditMask = "P0";
            this.txtPorUtilClient.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorUtilClient.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorUtilClient.Size = new System.Drawing.Size(37, 20);
            this.txtPorUtilClient.TabIndex = 78;
            // 
            // txtPorImprClient
            // 
            this.txtPorImprClient.EditValue = "0,00 ";
            this.txtPorImprClient.Location = new System.Drawing.Point(71, 50);
            this.txtPorImprClient.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorImprClient.Name = "txtPorImprClient";
            this.txtPorImprClient.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorImprClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorImprClient.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorImprClient.Properties.Appearance.Options.UseFont = true;
            this.txtPorImprClient.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorImprClient.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorImprClient.Properties.AutoHeight = false;
            this.txtPorImprClient.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorImprClient.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorImprClient.Properties.Mask.EditMask = "P0";
            this.txtPorImprClient.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorImprClient.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorImprClient.Size = new System.Drawing.Size(37, 20);
            this.txtPorImprClient.TabIndex = 77;
            // 
            // lblUtil
            // 
            this.lblUtil.AutoSize = true;
            this.lblUtil.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtil.Location = new System.Drawing.Point(15, 80);
            this.lblUtil.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUtil.Name = "lblUtil";
            this.lblUtil.Size = new System.Drawing.Size(27, 18);
            this.lblUtil.TabIndex = 76;
            this.lblUtil.Text = "Util";
            // 
            // lblImpr
            // 
            this.lblImpr.AutoSize = true;
            this.lblImpr.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImpr.Location = new System.Drawing.Point(15, 54);
            this.lblImpr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImpr.Name = "lblImpr";
            this.lblImpr.Size = new System.Drawing.Size(40, 18);
            this.lblImpr.TabIndex = 75;
            this.lblImpr.Text = "Impr";
            // 
            // lblADM
            // 
            this.lblADM.AutoSize = true;
            this.lblADM.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblADM.Location = new System.Drawing.Point(15, 26);
            this.lblADM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblADM.Name = "lblADM";
            this.lblADM.Size = new System.Drawing.Size(38, 18);
            this.lblADM.TabIndex = 73;
            this.lblADM.Text = "Adm";
            // 
            // txtPorAdmClient
            // 
            this.txtPorAdmClient.EditValue = "0,00 ";
            this.txtPorAdmClient.Location = new System.Drawing.Point(71, 25);
            this.txtPorAdmClient.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorAdmClient.Name = "txtPorAdmClient";
            this.txtPorAdmClient.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorAdmClient.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorAdmClient.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorAdmClient.Properties.Appearance.Options.UseFont = true;
            this.txtPorAdmClient.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorAdmClient.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorAdmClient.Properties.AutoHeight = false;
            this.txtPorAdmClient.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorAdmClient.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorAdmClient.Properties.Mask.EditMask = "P0";
            this.txtPorAdmClient.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorAdmClient.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorAdmClient.Size = new System.Drawing.Size(37, 20);
            this.txtPorAdmClient.TabIndex = 74;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblObservaciones.Location = new System.Drawing.Point(423, 95);
            this.lblObservaciones.Margin = new System.Windows.Forms.Padding(4);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(133, 17);
            this.lblObservaciones.TabIndex = 66;
            this.lblObservaciones.Text = "110_lblObservaciones";
            // 
            // lblJerarquia
            // 
            this.lblJerarquia.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblJerarquia.Location = new System.Drawing.Point(13, 122);
            this.lblJerarquia.Margin = new System.Windows.Forms.Padding(4);
            this.lblJerarquia.Name = "lblJerarquia";
            this.lblJerarquia.Size = new System.Drawing.Size(99, 17);
            this.lblJerarquia.TabIndex = 72;
            this.lblJerarquia.Text = "110_lblJerarquia";
            // 
            // cmbJerarquia
            // 
            this.cmbJerarquia.Location = new System.Drawing.Point(129, 119);
            this.cmbJerarquia.Margin = new System.Windows.Forms.Padding(4);
            this.cmbJerarquia.Name = "cmbJerarquia";
            this.cmbJerarquia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbJerarquia.Size = new System.Drawing.Size(149, 22);
            this.cmbJerarquia.TabIndex = 71;
            this.cmbJerarquia.EditValueChanged += new System.EventHandler(this.cmbJerarquia_EditValueChanged);
            // 
            // lblResponableEmp
            // 
            this.lblResponableEmp.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblResponableEmp.Location = new System.Drawing.Point(803, 14);
            this.lblResponableEmp.Margin = new System.Windows.Forms.Padding(4);
            this.lblResponableEmp.Name = "lblResponableEmp";
            this.lblResponableEmp.Size = new System.Drawing.Size(142, 17);
            this.lblResponableEmp.TabIndex = 65;
            this.lblResponableEmp.Text = "101_lblResponableEmp";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(13, 5);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(5);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(400, 26);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(423, 68);
            this.lblDescripcion.Margin = new System.Windows.Forms.Padding(4);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(114, 17);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "110_lblDescripcion";
            // 
            // cmbProposito
            // 
            this.cmbProposito.Location = new System.Drawing.Point(559, 117);
            this.cmbProposito.Margin = new System.Windows.Forms.Padding(4);
            this.cmbProposito.Name = "cmbProposito";
            this.cmbProposito.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProposito.Size = new System.Drawing.Size(122, 22);
            this.cmbProposito.TabIndex = 9;
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipoSolicitud.Location = new System.Drawing.Point(13, 94);
            this.lblTipoSolicitud.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(108, 17);
            this.lblTipoSolicitud.TabIndex = 60;
            this.lblTipoSolicitud.Text = "101_TipoSolicitud";
            // 
            // lblSolicitante
            // 
            this.lblSolicitante.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblSolicitante.Location = new System.Drawing.Point(423, 12);
            this.lblSolicitante.Margin = new System.Windows.Forms.Padding(4);
            this.lblSolicitante.Name = "lblSolicitante";
            this.lblSolicitante.Size = new System.Drawing.Size(139, 17);
            this.lblSolicitante.TabIndex = 61;
            this.lblSolicitante.Text = "101_lblNombrEmpresa";
            // 
            // masterResponsableEmp
            // 
            this.masterResponsableEmp.BackColor = System.Drawing.Color.Transparent;
            this.masterResponsableEmp.Filtros = null;
            this.masterResponsableEmp.Location = new System.Drawing.Point(813, 7);
            this.masterResponsableEmp.Margin = new System.Windows.Forms.Padding(5);
            this.masterResponsableEmp.Name = "masterResponsableEmp";
            this.masterResponsableEmp.Size = new System.Drawing.Size(403, 26);
            this.masterResponsableEmp.TabIndex = 2;
            this.masterResponsableEmp.Value = "";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(559, 64);
            this.txtDescripcion.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDescripcion.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.txtDescripcion.Properties.ShowIcon = false;
            this.txtDescripcion.Size = new System.Drawing.Size(212, 22);
            this.txtDescripcion.TabIndex = 10;
            this.txtDescripcion.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(559, 91);
            this.txtObservaciones.Margin = new System.Windows.Forms.Padding(4);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtObservaciones.Properties.ShowIcon = false;
            this.txtObservaciones.Size = new System.Drawing.Size(213, 22);
            this.txtObservaciones.TabIndex = 8;
            this.txtObservaciones.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // txtSolicitante
            // 
            this.txtSolicitante.Location = new System.Drawing.Point(559, 7);
            this.txtSolicitante.Margin = new System.Windows.Forms.Padding(4);
            this.txtSolicitante.Name = "txtSolicitante";
            this.txtSolicitante.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSolicitante.Properties.ShowIcon = false;
            this.txtSolicitante.Size = new System.Drawing.Size(196, 22);
            this.txtSolicitante.TabIndex = 1;
            this.txtSolicitante.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // pn1
            // 
            this.pn1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pn1.Controls.Add(this.txtNro);
            this.pn1.Controls.Add(this.lblNro);
            this.pn1.Controls.Add(this.btnQueryDoc);
            this.pn1.Controls.Add(this.masterPrefijo);
            this.pn1.Controls.Add(this.masterClaseServicio);
            this.pn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn1.Location = new System.Drawing.Point(2, 26);
            this.pn1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pn1.Name = "pn1";
            this.pn1.Size = new System.Drawing.Size(1567, 33);
            this.pn1.TabIndex = 71;
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(737, 5);
            this.txtNro.Margin = new System.Windows.Forms.Padding(4);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(40, 22);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(693, 10);
            this.lblNro.Margin = new System.Windows.Forms.Padding(4);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(66, 17);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "110_lblNro";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(779, 5);
            this.btnQueryDoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(32, 25);
            this.btnQueryDoc.TabIndex = 42;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(425, 1);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(5);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(264, 32);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // masterClaseServicio
            // 
            this.masterClaseServicio.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseServicio.Filtros = null;
            this.masterClaseServicio.Location = new System.Drawing.Point(13, 1);
            this.masterClaseServicio.Margin = new System.Windows.Forms.Padding(5);
            this.masterClaseServicio.Name = "masterClaseServicio";
            this.masterClaseServicio.Size = new System.Drawing.Size(407, 33);
            this.masterClaseServicio.TabIndex = 0;
            this.masterClaseServicio.Value = "";
            // 
            // txtAF
            // 
            this.txtAF.Enabled = false;
            this.txtAF.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAF.Location = new System.Drawing.Point(1032, 1);
            this.txtAF.Margin = new System.Windows.Forms.Padding(4);
            this.txtAF.Multiline = true;
            this.txtAF.Name = "txtAF";
            this.txtAF.Size = new System.Drawing.Size(120, 22);
            this.txtAF.TabIndex = 5;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Enabled = false;
            this.txtPrefix.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrefix.Location = new System.Drawing.Point(1253, 1);
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.txtPrefix.Multiline = true;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(65, 22);
            this.txtPrefix.TabIndex = 6;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(605, 1);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(173, 22);
            this.dtPeriod.TabIndex = 3;
            this.dtPeriod.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.dtPeriod_EditValueChanged);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAF.Location = new System.Drawing.Point(997, 5);
            this.lblAF.Margin = new System.Windows.Forms.Padding(5);
            this.lblAF.Name = "lblAF";
            this.lblAF.Size = new System.Drawing.Size(88, 18);
            this.lblAF.TabIndex = 96;
            this.lblAF.Text = "1005_lblAF";
            // 
            // lblBreak
            // 
            this.lblBreak.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreak.Location = new System.Drawing.Point(89, 5);
            this.lblBreak.Margin = new System.Windows.Forms.Padding(5);
            this.lblBreak.Name = "lblBreak";
            this.lblBreak.Size = new System.Drawing.Size(6, 17);
            this.lblBreak.TabIndex = 7;
            this.lblBreak.Text = "-";
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Enabled = false;
            this.txtDocDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocDesc.Location = new System.Drawing.Point(100, 1);
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocDesc.Multiline = true;
            this.txtDocDesc.Name = "txtDocDesc";
            this.txtDocDesc.Size = new System.Drawing.Size(288, 22);
            this.txtDocDesc.TabIndex = 1;
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Enabled = false;
            this.txtDocumentoID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoID.Location = new System.Drawing.Point(9, 1);
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocumentoID.Multiline = true;
            this.txtDocumentoID.Name = "txtDocumentoID";
            this.txtDocumentoID.Size = new System.Drawing.Size(76, 22);
            this.txtDocumentoID.TabIndex = 0;
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Enabled = false;
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroDoc.Location = new System.Drawing.Point(428, 1);
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(4);
            this.txtNumeroDoc.Multiline = true;
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.Size = new System.Drawing.Size(72, 22);
            this.txtNumeroDoc.TabIndex = 2;
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroDoc.Location = new System.Drawing.Point(409, 5);
            this.lblNumeroDoc.Margin = new System.Windows.Forms.Padding(5);
            this.lblNumeroDoc.Name = "lblNumeroDoc";
            this.lblNumeroDoc.Size = new System.Drawing.Size(12, 18);
            this.lblNumeroDoc.TabIndex = 92;
            this.lblNumeroDoc.Text = "#";
            // 
            // lblPrefix
            // 
            this.lblPrefix.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefix.Location = new System.Drawing.Point(1175, 5);
            this.lblPrefix.Margin = new System.Windows.Forms.Padding(5);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(115, 18);
            this.lblPrefix.TabIndex = 93;
            this.lblPrefix.Text = "1005_lblPrefix";
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(781, 5);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(104, 18);
            this.lblDate.TabIndex = 94;
            this.lblDate.Text = "1005_lblDate";
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(523, 5);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(5);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(118, 18);
            this.lblPeriod.TabIndex = 82;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(840, 1);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(4);
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
            this.dtFecha.Size = new System.Drawing.Size(133, 22);
            this.dtFecha.TabIndex = 4;
            this.dtFecha.Leave += new System.EventHandler(this.dtFecha_Leave);
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(17, 642);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1571, 84);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Controls.Add(this.chkUserEdit);
            this.grpboxDetail.Controls.Add(this.lblUsuario);
            this.grpboxDetail.Controls.Add(this.pnResumenCostos);
            this.grpboxDetail.Controls.Add(this.masterUsuarioPermiso);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(4);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(4);
            this.grpboxDetail.Size = new System.Drawing.Size(1571, 84);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            this.grpboxDetail.Text = "110_gbResumenCostos";
            // 
            // chkUserEdit
            // 
            this.chkUserEdit.Location = new System.Drawing.Point(1071, 49);
            this.chkUserEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkUserEdit.Name = "chkUserEdit";
            this.chkUserEdit.Properties.AutoWidth = true;
            this.chkUserEdit.Properties.Caption = "110_chkUserEdit";
            this.chkUserEdit.Size = new System.Drawing.Size(116, 20);
            this.chkUserEdit.TabIndex = 79;
            // 
            // lblUsuario
            // 
            this.lblUsuario.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUsuario.Location = new System.Drawing.Point(913, 25);
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(92, 18);
            this.lblUsuario.TabIndex = 54;
            this.lblUsuario.Text = "110_lblUsuario";
            // 
            // pnResumenCostos
            // 
            this.pnResumenCostos.Controls.Add(this.label2);
            this.pnResumenCostos.Controls.Add(this.txtCostoMult);
            this.pnResumenCostos.Controls.Add(this.txtOtros);
            this.pnResumenCostos.Controls.Add(this.btnVlrAdicional);
            this.pnResumenCostos.Controls.Add(this.txtPorIVA);
            this.pnResumenCostos.Controls.Add(this.lblIVA);
            this.pnResumenCostos.Controls.Add(this.txtIVA);
            this.pnResumenCostos.Controls.Add(this.lblOtros);
            this.pnResumenCostos.Controls.Add(this.lblCostoCliente);
            this.pnResumenCostos.Controls.Add(this.txtCostoCliente);
            this.pnResumenCostos.Controls.Add(this.lblCostoPresupuesto);
            this.pnResumenCostos.Controls.Add(this.txtCostoPresupuesto);
            this.pnResumenCostos.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnResumenCostos.Location = new System.Drawing.Point(4, 23);
            this.pnResumenCostos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnResumenCostos.Name = "pnResumenCostos";
            this.pnResumenCostos.Size = new System.Drawing.Size(900, 57);
            this.pnResumenCostos.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(341, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 18);
            this.label2.TabIndex = 85;
            this.label2.Text = "110_lblCostoMultip";
            // 
            // txtCostoMult
            // 
            this.txtCostoMult.EditValue = "0,00 ";
            this.txtCostoMult.Location = new System.Drawing.Point(428, 30);
            this.txtCostoMult.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoMult.Name = "txtCostoMult";
            this.txtCostoMult.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoMult.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoMult.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoMult.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoMult.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoMult.Properties.Appearance.Options.UseFont = true;
            this.txtCostoMult.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoMult.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoMult.Properties.AutoHeight = false;
            this.txtCostoMult.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtCostoMult.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoMult.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoMult.Properties.Mask.EditMask = "c";
            this.txtCostoMult.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoMult.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoMult.Properties.ReadOnly = true;
            this.txtCostoMult.Size = new System.Drawing.Size(180, 18);
            this.txtCostoMult.TabIndex = 86;
            // 
            // txtOtros
            // 
            this.txtOtros.EditValue = "0,00 ";
            this.txtOtros.Location = new System.Drawing.Point(741, 2);
            this.txtOtros.Margin = new System.Windows.Forms.Padding(4);
            this.txtOtros.Name = "txtOtros";
            this.txtOtros.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtOtros.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOtros.Properties.Appearance.Options.UseBorderColor = true;
            this.txtOtros.Properties.Appearance.Options.UseFont = true;
            this.txtOtros.Properties.Appearance.Options.UseTextOptions = true;
            this.txtOtros.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtOtros.Properties.AutoHeight = false;
            this.txtOtros.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtOtros.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtOtros.Properties.Mask.EditMask = "c";
            this.txtOtros.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtOtros.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtOtros.Size = new System.Drawing.Size(140, 20);
            this.txtOtros.TabIndex = 76;
            this.txtOtros.EditValueChanged += new System.EventHandler(this.txtOtros_EditValueChanged);
            // 
            // btnVlrAdicional
            // 
            this.btnVlrAdicional.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.btnVlrAdicional.Appearance.Options.UseFont = true;
            this.btnVlrAdicional.Location = new System.Drawing.Point(741, 28);
            this.btnVlrAdicional.Margin = new System.Windows.Forms.Padding(4);
            this.btnVlrAdicional.Name = "btnVlrAdicional";
            this.btnVlrAdicional.Size = new System.Drawing.Size(139, 27);
            this.btnVlrAdicional.TabIndex = 84;
            this.btnVlrAdicional.Text = "Valores Adicionales";
            // 
            // txtPorIVA
            // 
            this.txtPorIVA.EditValue = "19";
            this.txtPorIVA.Location = new System.Drawing.Point(297, 30);
            this.txtPorIVA.Margin = new System.Windows.Forms.Padding(4);
            this.txtPorIVA.Name = "txtPorIVA";
            this.txtPorIVA.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtPorIVA.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorIVA.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.txtPorIVA.Properties.Appearance.Options.UseBackColor = true;
            this.txtPorIVA.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorIVA.Properties.Appearance.Options.UseFont = true;
            this.txtPorIVA.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorIVA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorIVA.Properties.AutoHeight = false;
            this.txtPorIVA.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtPorIVA.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorIVA.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorIVA.Properties.Mask.EditMask = "P0";
            this.txtPorIVA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorIVA.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorIVA.Size = new System.Drawing.Size(35, 20);
            this.txtPorIVA.TabIndex = 79;
            // 
            // lblIVA
            // 
            this.lblIVA.AutoSize = true;
            this.lblIVA.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIVA.Location = new System.Drawing.Point(7, 32);
            this.lblIVA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(136, 18);
            this.lblIVA.TabIndex = 77;
            this.lblIVA.Text = "110_lblCostoIVA";
            // 
            // txtIVA
            // 
            this.txtIVA.EditValue = "0,00 ";
            this.txtIVA.Location = new System.Drawing.Point(157, 30);
            this.txtIVA.Margin = new System.Windows.Forms.Padding(4);
            this.txtIVA.Name = "txtIVA";
            this.txtIVA.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtIVA.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtIVA.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIVA.Properties.Appearance.Options.UseBackColor = true;
            this.txtIVA.Properties.Appearance.Options.UseBorderColor = true;
            this.txtIVA.Properties.Appearance.Options.UseFont = true;
            this.txtIVA.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIVA.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIVA.Properties.AutoHeight = false;
            this.txtIVA.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtIVA.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVA.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVA.Properties.Mask.EditMask = "c";
            this.txtIVA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIVA.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIVA.Size = new System.Drawing.Size(135, 20);
            this.txtIVA.TabIndex = 78;
            // 
            // lblOtros
            // 
            this.lblOtros.AutoSize = true;
            this.lblOtros.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOtros.Location = new System.Drawing.Point(649, 5);
            this.lblOtros.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOtros.Name = "lblOtros";
            this.lblOtros.Size = new System.Drawing.Size(151, 18);
            this.lblOtros.TabIndex = 75;
            this.lblOtros.Text = "110_lblCostoOtros";
            // 
            // lblCostoCliente
            // 
            this.lblCostoCliente.AutoSize = true;
            this.lblCostoCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoCliente.Location = new System.Drawing.Point(341, 6);
            this.lblCostoCliente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCostoCliente.Name = "lblCostoCliente";
            this.lblCostoCliente.Size = new System.Drawing.Size(162, 18);
            this.lblCostoCliente.TabIndex = 73;
            this.lblCostoCliente.Text = "110_lblCostoCliente";
            // 
            // txtCostoCliente
            // 
            this.txtCostoCliente.EditValue = "0,00 ";
            this.txtCostoCliente.Location = new System.Drawing.Point(428, 5);
            this.txtCostoCliente.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoCliente.Name = "txtCostoCliente";
            this.txtCostoCliente.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoCliente.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoCliente.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoCliente.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoCliente.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoCliente.Properties.Appearance.Options.UseFont = true;
            this.txtCostoCliente.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoCliente.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoCliente.Properties.AutoHeight = false;
            this.txtCostoCliente.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtCostoCliente.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoCliente.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoCliente.Properties.Mask.EditMask = "c";
            this.txtCostoCliente.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoCliente.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoCliente.Properties.ReadOnly = true;
            this.txtCostoCliente.Size = new System.Drawing.Size(180, 18);
            this.txtCostoCliente.TabIndex = 74;
            // 
            // lblCostoPresupuesto
            // 
            this.lblCostoPresupuesto.AutoSize = true;
            this.lblCostoPresupuesto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoPresupuesto.Location = new System.Drawing.Point(7, 6);
            this.lblCostoPresupuesto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCostoPresupuesto.Name = "lblCostoPresupuesto";
            this.lblCostoPresupuesto.Size = new System.Drawing.Size(202, 18);
            this.lblCostoPresupuesto.TabIndex = 71;
            this.lblCostoPresupuesto.Text = "110_lblCostoPresupuesto";
            // 
            // txtCostoPresupuesto
            // 
            this.txtCostoPresupuesto.EditValue = "0,00 ";
            this.txtCostoPresupuesto.Location = new System.Drawing.Point(155, 5);
            this.txtCostoPresupuesto.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoPresupuesto.Name = "txtCostoPresupuesto";
            this.txtCostoPresupuesto.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoPresupuesto.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoPresupuesto.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoPresupuesto.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoPresupuesto.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoPresupuesto.Properties.Appearance.Options.UseFont = true;
            this.txtCostoPresupuesto.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoPresupuesto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoPresupuesto.Properties.AutoHeight = false;
            this.txtCostoPresupuesto.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtCostoPresupuesto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoPresupuesto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoPresupuesto.Properties.Mask.EditMask = "c";
            this.txtCostoPresupuesto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoPresupuesto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoPresupuesto.Properties.ReadOnly = true;
            this.txtCostoPresupuesto.Size = new System.Drawing.Size(180, 18);
            this.txtCostoPresupuesto.TabIndex = 72;
            // 
            // masterUsuarioPermiso
            // 
            this.masterUsuarioPermiso.BackColor = System.Drawing.Color.Transparent;
            this.masterUsuarioPermiso.Filtros = null;
            this.masterUsuarioPermiso.Font = new System.Drawing.Font("Tahoma", 9F);
            this.masterUsuarioPermiso.Location = new System.Drawing.Point(913, 17);
            this.masterUsuarioPermiso.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.masterUsuarioPermiso.Name = "masterUsuarioPermiso";
            this.masterUsuarioPermiso.Size = new System.Drawing.Size(472, 28);
            this.masterUsuarioPermiso.TabIndex = 3;
            this.masterUsuarioPermiso.Value = "";
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(17, 240);
            this.pnlGrids.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1571, 394);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(4);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcDocument);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitGrids.Panel2.Text = "Panel2";
            this.splitGrids.Size = new System.Drawing.Size(1571, 394);
            this.splitGrids.SplitterPosition = 907;
            this.splitGrids.TabIndex = 2;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // grpCtrlRecurso
            // 
            this.grpCtrlRecurso.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F);
            this.grpCtrlRecurso.AppearanceCaption.Options.UseFont = true;
            this.grpCtrlRecurso.Controls.Add(this.txtCostoTotalAPU);
            this.grpCtrlRecurso.Controls.Add(this.lblAPU);
            this.grpCtrlRecurso.Controls.Add(this.txtCostoAIUxAPU);
            this.grpCtrlRecurso.Controls.Add(this.btnExportRecursos);
            this.grpCtrlRecurso.Controls.Add(this.lblCostoAIU);
            this.grpCtrlRecurso.Controls.Add(this.btnImportRecursos);
            this.grpCtrlRecurso.Controls.Add(this.lblCostoAPU);
            this.grpCtrlRecurso.Controls.Add(this.txtCostoAPU);
            this.grpCtrlRecurso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCtrlRecurso.Location = new System.Drawing.Point(3, 2);
            this.grpCtrlRecurso.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpCtrlRecurso.Name = "grpCtrlRecurso";
            this.grpCtrlRecurso.ShowCaption = false;
            this.grpCtrlRecurso.Size = new System.Drawing.Size(653, 66);
            this.grpCtrlRecurso.TabIndex = 52;
            // 
            // txtCostoTotalAPU
            // 
            this.txtCostoTotalAPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCostoTotalAPU.EditValue = "0,00 ";
            this.txtCostoTotalAPU.Location = new System.Drawing.Point(475, 45);
            this.txtCostoTotalAPU.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoTotalAPU.Name = "txtCostoTotalAPU";
            this.txtCostoTotalAPU.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoTotalAPU.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoTotalAPU.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoTotalAPU.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoTotalAPU.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoTotalAPU.Properties.Appearance.Options.UseFont = true;
            this.txtCostoTotalAPU.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoTotalAPU.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoTotalAPU.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtCostoTotalAPU.Properties.AutoHeight = false;
            this.txtCostoTotalAPU.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtCostoTotalAPU.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoTotalAPU.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoTotalAPU.Properties.Mask.EditMask = "c2";
            this.txtCostoTotalAPU.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoTotalAPU.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoTotalAPU.Properties.ReadOnly = true;
            this.txtCostoTotalAPU.Size = new System.Drawing.Size(123, 17);
            this.txtCostoTotalAPU.TabIndex = 89;
            // 
            // lblAPU
            // 
            this.lblAPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAPU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAPU.Location = new System.Drawing.Point(374, 46);
            this.lblAPU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAPU.Name = "lblAPU";
            this.lblAPU.Size = new System.Drawing.Size(103, 22);
            this.lblAPU.TabIndex = 69;
            this.lblAPU.Text = "110_lblAPU";
            this.lblAPU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCostoAIUxAPU
            // 
            this.txtCostoAIUxAPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCostoAIUxAPU.EditValue = "0,00 ";
            this.txtCostoAIUxAPU.Location = new System.Drawing.Point(475, 24);
            this.txtCostoAIUxAPU.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoAIUxAPU.Name = "txtCostoAIUxAPU";
            this.txtCostoAIUxAPU.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoAIUxAPU.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoAIUxAPU.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoAIUxAPU.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoAIUxAPU.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoAIUxAPU.Properties.Appearance.Options.UseFont = true;
            this.txtCostoAIUxAPU.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoAIUxAPU.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoAIUxAPU.Properties.AutoHeight = false;
            this.txtCostoAIUxAPU.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtCostoAIUxAPU.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoAIUxAPU.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoAIUxAPU.Properties.Mask.EditMask = "c2";
            this.txtCostoAIUxAPU.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoAIUxAPU.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoAIUxAPU.Properties.ReadOnly = true;
            this.txtCostoAIUxAPU.Size = new System.Drawing.Size(123, 17);
            this.txtCostoAIUxAPU.TabIndex = 88;
            // 
            // btnExportRecursos
            // 
            this.btnExportRecursos.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.btnExportRecursos.Appearance.Options.UseFont = true;
            this.btnExportRecursos.Image = global::NewAge.Properties.Resources.TBIconExportExcel;
            this.btnExportRecursos.Location = new System.Drawing.Point(120, 9);
            this.btnExportRecursos.LookAndFeel.SkinName = "Black";
            this.btnExportRecursos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnExportRecursos.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportRecursos.Name = "btnExportRecursos";
            this.btnExportRecursos.Size = new System.Drawing.Size(108, 47);
            this.btnExportRecursos.TabIndex = 72;
            this.btnExportRecursos.Text = "Exportar \r\nRecursos";
            this.btnExportRecursos.Click += new System.EventHandler(this.btnExportRecursos_Click);
            // 
            // lblCostoAIU
            // 
            this.lblCostoAIU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCostoAIU.AutoSize = true;
            this.lblCostoAIU.BackColor = System.Drawing.Color.Transparent;
            this.lblCostoAIU.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoAIU.ForeColor = System.Drawing.Color.Black;
            this.lblCostoAIU.Location = new System.Drawing.Point(375, 26);
            this.lblCostoAIU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCostoAIU.Name = "lblCostoAIU";
            this.lblCostoAIU.Size = new System.Drawing.Size(56, 16);
            this.lblCostoAIU.TabIndex = 85;
            this.lblCostoAIU.Text = "110_AIU";
            this.lblCostoAIU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnImportRecursos
            // 
            this.btnImportRecursos.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.btnImportRecursos.Appearance.Options.UseFont = true;
            this.btnImportRecursos.Image = global::NewAge.Properties.Resources.TBIconImport;
            this.btnImportRecursos.Location = new System.Drawing.Point(5, 9);
            this.btnImportRecursos.LookAndFeel.SkinName = "Black";
            this.btnImportRecursos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnImportRecursos.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportRecursos.Name = "btnImportRecursos";
            this.btnImportRecursos.Size = new System.Drawing.Size(111, 47);
            this.btnImportRecursos.TabIndex = 71;
            this.btnImportRecursos.Text = "Importar \r\nRecursos";
            this.btnImportRecursos.Click += new System.EventHandler(this.btnImportRecursos_Click);
            // 
            // lblCostoAPU
            // 
            this.lblCostoAPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCostoAPU.AutoSize = true;
            this.lblCostoAPU.BackColor = System.Drawing.Color.Transparent;
            this.lblCostoAPU.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoAPU.ForeColor = System.Drawing.Color.Black;
            this.lblCostoAPU.Location = new System.Drawing.Point(375, 5);
            this.lblCostoAPU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCostoAPU.Name = "lblCostoAPU";
            this.lblCostoAPU.Size = new System.Drawing.Size(59, 16);
            this.lblCostoAPU.TabIndex = 84;
            this.lblCostoAPU.Text = "110_APU";
            this.lblCostoAPU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCostoAPU
            // 
            this.txtCostoAPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCostoAPU.EditValue = "0,00 ";
            this.txtCostoAPU.Location = new System.Drawing.Point(475, 3);
            this.txtCostoAPU.Margin = new System.Windows.Forms.Padding(4);
            this.txtCostoAPU.Name = "txtCostoAPU";
            this.txtCostoAPU.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCostoAPU.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoAPU.Properties.Appearance.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCostoAPU.Properties.Appearance.Options.UseBackColor = true;
            this.txtCostoAPU.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoAPU.Properties.Appearance.Options.UseFont = true;
            this.txtCostoAPU.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoAPU.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoAPU.Properties.AutoHeight = false;
            this.txtCostoAPU.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtCostoAPU.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoAPU.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoAPU.Properties.Mask.EditMask = "c2";
            this.txtCostoAPU.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoAPU.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoAPU.Properties.ReadOnly = true;
            this.txtCostoAPU.Size = new System.Drawing.Size(123, 17);
            this.txtCostoAPU.TabIndex = 87;
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
            this.editValue2Cant,
            this.editValue8Cant,
            this.editLink,
            this.editSpinPorcen});
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
            this.editSpin.AllowMouseWheel = false;
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
            this.editSpin4.AllowMouseWheel = false;
            this.editSpin4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
            // 
            // editSpin7
            // 
            this.editSpin7.AllowMouseWheel = false;
            this.editSpin7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin7.Mask.EditMask = "c7";
            this.editSpin7.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin7.Name = "editSpin7";
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
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            this.editValue4.Name = "editValue4";
            // 
            // editValue2Cant
            // 
            this.editValue2Cant.AllowMouseWheel = false;
            this.editValue2Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.Mask.EditMask = "n2";
            this.editValue2Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2Cant.Name = "editValue2Cant";
            // 
            // editValue8Cant
            // 
            this.editValue8Cant.AllowMouseWheel = false;
            this.editValue8Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue8Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue8Cant.Mask.EditMask = "n8";
            this.editValue8Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue8Cant.Name = "editValue8Cant";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.AllowMouseWheel = false;
            this.editSpinPorcen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorcen.Name = "editSpinPorcen";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.grpCtrlRecurso, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gcRecurso, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.0203F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.9797F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(659, 394);
            this.tableLayoutPanel1.TabIndex = 53;
            // 
            // PlaneacionCostosAPUCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1607, 734);
            this.Controls.Add(this.pnlMainContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlaneacionCostosAPUCliente";
            this.Text = "1005";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            this.pn2.ResumeLayout(false);
            this.pn2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkMultipActivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPersonalCant.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEquipoCant.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRedondeo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPresup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMultiplicador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAPUIncluyeAIU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReporte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAIU)).EndInit();
            this.pnlAIU.ResumeLayout(false);
            this.pnlAIU.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmEmp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJerarquia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pn1)).EndInit();
            this.pn1.ResumeLayout(false);
            this.pn1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkUserEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnResumenCostos)).EndInit();
            this.pnResumenCostos.ResumeLayout(false);
            this.pnResumenCostos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoMult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtros.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoCliente.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoPresupuesto.Properties)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlRecurso)).EndInit();
            this.grpCtrlRecurso.ResumeLayout(false);
            this.grpCtrlRecurso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoTotalAPU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoAIUxAPU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoAPU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue8Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected System.Windows.Forms.GroupBox grpboxDetail;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin7;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraEditors.LabelControl lblNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblDate;
        private DevExpress.XtraEditors.LabelControl lblPrefix;
        private DevExpress.XtraEditors.LabelControl lblBreak;
        private System.Windows.Forms.Panel pnlDetail;
        protected System.Windows.Forms.TextBox txtDocumentoID;
        protected System.Windows.Forms.TextBox txtDocDesc;
        protected System.Windows.Forms.TextBox txtNumeroDoc;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        protected System.Windows.Forms.TextBox txtPrefix;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        protected uc_PeriodoEdit dtPeriod;
        protected System.Windows.Forms.TextBox txtAF;
        protected DevExpress.XtraEditors.LabelControl lblAF;
        protected System.Windows.Forms.Panel pnlGrids;
        protected DevExpress.XtraGrid.GridControl gcRecurso;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvRecurso;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorcen;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalleRecurso;
        private DevExpress.XtraEditors.CheckEdit chkRecursoXTrabInd;
        private DevExpress.XtraEditors.LabelControl lblObservaciones;
        private DevExpress.XtraEditors.LabelControl lblResponableEmp;
        private DevExpress.XtraEditors.LabelControl lblSolicitante;
        private DevExpress.XtraEditors.LabelControl lblTipoSolicitud;
        private DevExpress.XtraEditors.LookUpEdit cmbProposito;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private uc_MasterFind masterResponsableEmp;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private uc_MasterFind masterPrefijo;
        private uc_MasterFind masterProyecto;
        private uc_MasterFind masterCliente;
        private uc_MasterFind masterClaseServicio;
        private DevExpress.XtraEditors.SimpleButton btnExportRecursos;
        private DevExpress.XtraEditors.SimpleButton btnImportRecursos;
        private DevExpress.XtraEditors.GroupControl grpCtrlRecurso;
        private DevExpress.XtraEditors.PanelControl pnResumenCostos;
        private System.Windows.Forms.Label lblCostoCliente;
        private DevExpress.XtraEditors.TextEdit txtCostoCliente;
        private System.Windows.Forms.Label lblCostoPresupuesto;
        private DevExpress.XtraEditors.TextEdit txtCostoPresupuesto;
        private DevExpress.XtraEditors.PanelControl pn1;
        private System.Windows.Forms.Panel pn2;
        private System.Windows.Forms.Label lblAPU;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue8Cant;
        private DevExpress.XtraEditors.LabelControl lblJerarquia;
        private DevExpress.XtraEditors.LookUpEdit cmbJerarquia;
        private DevExpress.XtraEditors.GroupControl pnlAIU;
        private System.Windows.Forms.Label lblADM;
        private DevExpress.XtraEditors.TextEdit txtPorAdmClient;
        private DevExpress.XtraEditors.TextEdit txtPorUtilEmp;
        private DevExpress.XtraEditors.TextEdit txtPorImprEmp;
        private DevExpress.XtraEditors.TextEdit txtPorAdmEmp;
        private DevExpress.XtraEditors.TextEdit txtPorUtilClient;
        private DevExpress.XtraEditors.TextEdit txtPorImprClient;
        private System.Windows.Forms.Label lblUtil;
        private System.Windows.Forms.Label lblImpr;
        private System.Windows.Forms.Label lblEmpresa;
        private System.Windows.Forms.Label lblCliente;
        private DevExpress.XtraEditors.MemoExEdit txtDescripcion;
        private DevExpress.XtraEditors.MemoExEdit txtObservaciones;
        private DevExpress.XtraEditors.LabelControl lblReporte;
        private DevExpress.XtraEditors.LookUpEdit cmbReporte;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private DevExpress.XtraEditors.MemoExEdit txtLicitacion;
        private DevExpress.XtraEditors.CheckEdit chkAPUIncluyeAIU;
        private System.Windows.Forms.Label lblIVA;
        private DevExpress.XtraEditors.TextEdit txtIVA;
        private System.Windows.Forms.Label lblOtros;
        private DevExpress.XtraEditors.TextEdit txtOtros;
        private DevExpress.XtraEditors.TextEdit txtPorIVA;
        private DevExpress.XtraEditors.MemoExEdit txtSolicitante;
        private DevExpress.XtraEditors.SimpleButton btnVlrAdicional;
        private DevExpress.XtraEditors.TextEdit txtCostoTotalAPU;
        private DevExpress.XtraEditors.TextEdit txtCostoAIUxAPU;
        private DevExpress.XtraEditors.TextEdit txtCostoAPU;
        private System.Windows.Forms.Label lblCostoAIU;
        private System.Windows.Forms.Label lblCostoAPU;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.TextEdit txtMultiplicador;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtCostoMult;
        private uc_MasterFind masterCentroCto;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblMultipl;
        private DevExpress.XtraEditors.LabelControl lbl;
        private DevExpress.XtraEditors.LabelControl lblUsuario;
        private uc_MasterFind masterUsuarioPermiso;
        private DevExpress.XtraEditors.CheckEdit chkUserEdit;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoPresup;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl lblProposito;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoSolicitud;
        private DevExpress.XtraEditors.SimpleButton btnCopyAPU;
        private DevExpress.XtraEditors.LabelControl lblRedondeo;
        private DevExpress.XtraEditors.LookUpEdit cmbRedondeo;
        private DevExpress.XtraEditors.CheckEdit chkPersonalCant;
        private DevExpress.XtraEditors.CheckEdit chkEquipoCant;
        private DevExpress.XtraEditors.CheckEdit chkMultipActivo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}