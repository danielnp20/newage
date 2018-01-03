using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PlaneacionTiempo
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaneacionTiempo));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
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
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmbProposito = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbTipoPresup = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtSolicitante = new DevExpress.XtraEditors.MemoExEdit();
            this.masterAreaFuncional = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkTiempoAutInd = new DevExpress.XtraEditors.CheckEdit();
            this.chkRecursoXTrabInd = new DevExpress.XtraEditors.CheckEdit();
            this.chkAPUIncluyeAIU = new DevExpress.XtraEditors.CheckEdit();
            this.lblJerarquia = new DevExpress.XtraEditors.LabelControl();
            this.cmbJerarquia = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCentroCto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoExEdit();
            this.lblFechaInicio = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
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
            this.lblSolicitante = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtAF = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblAF = new DevExpress.XtraEditors.LabelControl();
            this.lblObservaciones = new DevExpress.XtraEditors.LabelControl();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.lblResponableEmp = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.lblTelefono = new DevExpress.XtraEditors.LabelControl();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.txtTelefono = new DevExpress.XtraEditors.TextEdit();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.lblCorreo = new DevExpress.XtraEditors.LabelControl();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.txtCorreo = new DevExpress.XtraEditors.TextEdit();
            this.lblPrefix = new DevExpress.XtraEditors.LabelControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoSolicitud = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.cmbTipoSolicitud = new DevExpress.XtraEditors.LookUpEdit();
            this.masterClaseServicio = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            this.lblResponsableCli = new DevExpress.XtraEditors.LabelControl();
            this.txtResposableCli = new DevExpress.XtraEditors.TextEdit();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.masterResponsableEmp = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtObservaciones = new DevExpress.XtraEditors.MemoExEdit();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoExEdit();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtModelo = new DevExpress.XtraEditors.TextEdit();
            this.txtMarca = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtSortByCap = new DevExpress.XtraEditors.SimpleButton();
            this.lblCostoPresupuesto = new System.Windows.Forms.Label();
            this.txtCostoPresupuesto = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCostoCliente = new DevExpress.XtraEditors.TextEdit();
            this.txtCostoMult = new DevExpress.XtraEditors.TextEdit();
            this.lblCostoCliente = new System.Windows.Forms.Label();
            this.txtPorIVA = new DevExpress.XtraEditors.TextEdit();
            this.txtIVA = new DevExpress.XtraEditors.TextEdit();
            this.lblIVA = new System.Windows.Forms.Label();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.grpCtrlProvider = new DevExpress.XtraEditors.GroupControl();
            this.masterUnidadBaseDia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin7 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue6Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPresup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTiempoAutInd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAPUIncluyeAIU.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJerarquia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlAIU)).BeginInit();
            this.pnlAIU.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmEmp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorUtilClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorImprClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorAdmClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefono.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorreo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResposableCli.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtModelo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMarca.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoPresupuesto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoCliente.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoMult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorIVA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).BeginInit();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).BeginInit();
            this.grpCtrlProvider.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue6Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
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
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDocument_EmbeddedNavigator_ButtonClick);
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(0, 0);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(5);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(710, 341);
            this.gcDocument.TabIndex = 50;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
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
            this.gvDetalleRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDetalleRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDetalleRecurso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalleRecurso_FocusedRowChanged);
            this.gvDetalleRecurso.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalleRecurso_CellValueChanged);
            this.gvDetalleRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDetalleRecurso.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalleRecurso_CustomColumnDisplayText);
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
            this.gcRecurso.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcRecurso.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null)});
            this.gcRecurso.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcRecurso.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecurso.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode2.LevelTemplate = this.gvDetalleRecurso;
            gridLevelNode2.RelationName = "Detalle";
            this.gcRecurso.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcRecurso.Location = new System.Drawing.Point(0, 34);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(335, 307);
            this.gcRecurso.TabIndex = 51;
            this.gcRecurso.UseEmbeddedNavigator = true;
            this.gcRecurso.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecurso,
            this.gvDetalleRecurso});
            // 
            // gvRecurso
            // 
            this.gvRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvRecurso.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvRecurso.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvRecurso.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecurso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvRecurso.HorzScrollStep = 50;
            this.gvRecurso.Name = "gvRecurso";
            this.gvRecurso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecurso.OptionsCustomization.AllowFilter = false;
            this.gvRecurso.OptionsCustomization.AllowSort = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsMenu.EnableGroupPanelMenu = false;
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
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1078, 620);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.71429F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.28571F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1074, 616);
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
            this.grpctrlHeader.Controls.Add(this.labelControl3);
            this.grpctrlHeader.Controls.Add(this.cmbProposito);
            this.grpctrlHeader.Controls.Add(this.labelControl2);
            this.grpctrlHeader.Controls.Add(this.txtTasaCambio);
            this.grpctrlHeader.Controls.Add(this.lblNro);
            this.grpctrlHeader.Controls.Add(this.masterPrefijo);
            this.grpctrlHeader.Controls.Add(this.cmbTipoPresup);
            this.grpctrlHeader.Controls.Add(this.labelControl1);
            this.grpctrlHeader.Controls.Add(this.txtSolicitante);
            this.grpctrlHeader.Controls.Add(this.masterAreaFuncional);
            this.grpctrlHeader.Controls.Add(this.chkTiempoAutInd);
            this.grpctrlHeader.Controls.Add(this.chkRecursoXTrabInd);
            this.grpctrlHeader.Controls.Add(this.chkAPUIncluyeAIU);
            this.grpctrlHeader.Controls.Add(this.lblJerarquia);
            this.grpctrlHeader.Controls.Add(this.cmbJerarquia);
            this.grpctrlHeader.Controls.Add(this.masterCentroCto);
            this.grpctrlHeader.Controls.Add(this.lblLicitacion);
            this.grpctrlHeader.Controls.Add(this.txtLicitacion);
            this.grpctrlHeader.Controls.Add(this.lblFechaInicio);
            this.grpctrlHeader.Controls.Add(this.dtFechaInicio);
            this.grpctrlHeader.Controls.Add(this.lblReporte);
            this.grpctrlHeader.Controls.Add(this.cmbReporte);
            this.grpctrlHeader.Controls.Add(this.pnlAIU);
            this.grpctrlHeader.Controls.Add(this.lblSolicitante);
            this.grpctrlHeader.Controls.Add(this.masterCliente);
            this.grpctrlHeader.Controls.Add(this.txtAF);
            this.grpctrlHeader.Controls.Add(this.txtPrefix);
            this.grpctrlHeader.Controls.Add(this.dtPeriod);
            this.grpctrlHeader.Controls.Add(this.lblAF);
            this.grpctrlHeader.Controls.Add(this.lblObservaciones);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.lblResponableEmp);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.lblTelefono);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.txtTelefono);
            this.grpctrlHeader.Controls.Add(this.txtNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblCorreo);
            this.grpctrlHeader.Controls.Add(this.lblNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.txtCorreo);
            this.grpctrlHeader.Controls.Add(this.lblPrefix);
            this.grpctrlHeader.Controls.Add(this.lblDate);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Controls.Add(this.lblTipoSolicitud);
            this.grpctrlHeader.Controls.Add(this.dtFecha);
            this.grpctrlHeader.Controls.Add(this.cmbTipoSolicitud);
            this.grpctrlHeader.Controls.Add(this.masterClaseServicio);
            this.grpctrlHeader.Controls.Add(this.masterProyecto);
            this.grpctrlHeader.Controls.Add(this.lblDescripcion);
            this.grpctrlHeader.Controls.Add(this.txtNro);
            this.grpctrlHeader.Controls.Add(this.lblResponsableCli);
            this.grpctrlHeader.Controls.Add(this.txtResposableCli);
            this.grpctrlHeader.Controls.Add(this.btnQueryDoc);
            this.grpctrlHeader.Controls.Add(this.masterResponsableEmp);
            this.grpctrlHeader.Controls.Add(this.txtObservaciones);
            this.grpctrlHeader.Controls.Add(this.txtDescripcion);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(13, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1050, 187);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl3.Location = new System.Drawing.Point(218, 163);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(79, 13);
            this.labelControl3.TabIndex = 114;
            this.labelControl3.Text = "110_lblProposito";
            // 
            // cmbProposito
            // 
            this.cmbProposito.Location = new System.Drawing.Point(316, 160);
            this.cmbProposito.Name = "cmbProposito";
            this.cmbProposito.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProposito.Size = new System.Drawing.Size(88, 20);
            this.cmbProposito.TabIndex = 113;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl2.Location = new System.Drawing.Point(733, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(92, 13);
            this.labelControl2.TabIndex = 96;
            this.labelControl2.Text = "110_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0,00 ";
            this.txtTasaCambio.Location = new System.Drawing.Point(826, 71);
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
            this.txtTasaCambio.Size = new System.Drawing.Size(96, 20);
            this.txtTasaCambio.TabIndex = 95;
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(522, 29);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(51, 13);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "110_lblNro";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(315, 23);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(203, 21);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // cmbTipoPresup
            // 
            this.cmbTipoPresup.Location = new System.Drawing.Point(813, 140);
            this.cmbTipoPresup.Name = "cmbTipoPresup";
            this.cmbTipoPresup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoPresup.Properties.ReadOnly = true;
            this.cmbTipoPresup.Size = new System.Drawing.Size(111, 20);
            this.cmbTipoPresup.TabIndex = 112;
            this.cmbTipoPresup.EditValueChanged += new System.EventHandler(this.cmbTipoPresup_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl1.Location = new System.Drawing.Point(736, 143);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(123, 13);
            this.labelControl1.TabIndex = 111;
            this.labelControl1.Text = "110_cmbTipoPresupuesto";
            // 
            // txtSolicitante
            // 
            this.txtSolicitante.Location = new System.Drawing.Point(414, 49);
            this.txtSolicitante.Name = "txtSolicitante";
            this.txtSolicitante.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSolicitante.Properties.ShowIcon = false;
            this.txtSolicitante.Size = new System.Drawing.Size(172, 20);
            this.txtSolicitante.TabIndex = 6;
            // 
            // masterAreaFuncional
            // 
            this.masterAreaFuncional.BackColor = System.Drawing.Color.Transparent;
            this.masterAreaFuncional.Filtros = null;
            this.masterAreaFuncional.Location = new System.Drawing.Point(628, 23);
            this.masterAreaFuncional.Margin = new System.Windows.Forms.Padding(4);
            this.masterAreaFuncional.Name = "masterAreaFuncional";
            this.masterAreaFuncional.Size = new System.Drawing.Size(296, 21);
            this.masterAreaFuncional.TabIndex = 4;
            this.masterAreaFuncional.Value = "";
            this.masterAreaFuncional.Leave += new System.EventHandler(this.masterAreaFuncional_Leave);
            // 
            // chkTiempoAutInd
            // 
            this.chkTiempoAutInd.Location = new System.Drawing.Point(594, 96);
            this.chkTiempoAutInd.Margin = new System.Windows.Forms.Padding(2);
            this.chkTiempoAutInd.Name = "chkTiempoAutInd";
            this.chkTiempoAutInd.Properties.AutoWidth = true;
            this.chkTiempoAutInd.Properties.Caption = "110_chkTiempoAutInd";
            this.chkTiempoAutInd.Properties.ReadOnly = true;
            this.chkTiempoAutInd.Size = new System.Drawing.Size(129, 19);
            this.chkTiempoAutInd.TabIndex = 110;
            // 
            // chkRecursoXTrabInd
            // 
            this.chkRecursoXTrabInd.Location = new System.Drawing.Point(594, 117);
            this.chkRecursoXTrabInd.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecursoXTrabInd.Name = "chkRecursoXTrabInd";
            this.chkRecursoXTrabInd.Properties.AutoWidth = true;
            this.chkRecursoXTrabInd.Properties.Caption = "110_chkRecursoXTrab";
            this.chkRecursoXTrabInd.Size = new System.Drawing.Size(129, 19);
            this.chkRecursoXTrabInd.TabIndex = 70;
            this.chkRecursoXTrabInd.Visible = false;
            this.chkRecursoXTrabInd.CheckedChanged += new System.EventHandler(this.chkRecursoXTrabInd_CheckedChanged);
            // 
            // chkAPUIncluyeAIU
            // 
            this.chkAPUIncluyeAIU.Location = new System.Drawing.Point(594, 140);
            this.chkAPUIncluyeAIU.Margin = new System.Windows.Forms.Padding(2);
            this.chkAPUIncluyeAIU.Name = "chkAPUIncluyeAIU";
            this.chkAPUIncluyeAIU.Properties.AutoWidth = true;
            this.chkAPUIncluyeAIU.Properties.Caption = "110_chkAPUIncluyeAIU";
            this.chkAPUIncluyeAIU.Size = new System.Drawing.Size(135, 19);
            this.chkAPUIncluyeAIU.TabIndex = 109;
            // 
            // lblJerarquia
            // 
            this.lblJerarquia.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblJerarquia.Location = new System.Drawing.Point(14, 162);
            this.lblJerarquia.Name = "lblJerarquia";
            this.lblJerarquia.Size = new System.Drawing.Size(79, 13);
            this.lblJerarquia.TabIndex = 108;
            this.lblJerarquia.Text = "110_lblJerarquia";
            // 
            // cmbJerarquia
            // 
            this.cmbJerarquia.Location = new System.Drawing.Point(111, 160);
            this.cmbJerarquia.Name = "cmbJerarquia";
            this.cmbJerarquia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbJerarquia.Size = new System.Drawing.Size(100, 20);
            this.cmbJerarquia.TabIndex = 107;
            // 
            // masterCentroCto
            // 
            this.masterCentroCto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCto.Filtros = null;
            this.masterCentroCto.Location = new System.Drawing.Point(12, 112);
            this.masterCentroCto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCentroCto.Name = "masterCentroCto";
            this.masterCentroCto.Size = new System.Drawing.Size(294, 21);
            this.masterCentroCto.TabIndex = 106;
            this.masterCentroCto.Value = "";
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(317, 98);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(77, 13);
            this.lblLicitacion.TabIndex = 105;
            this.lblLicitacion.Text = "110_lblLicitacion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(414, 94);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLicitacion.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.txtLicitacion.Properties.ShowIcon = false;
            this.txtLicitacion.Size = new System.Drawing.Size(172, 20);
            this.txtLicitacion.TabIndex = 104;
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblFechaInicio.Location = new System.Drawing.Point(734, 122);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(88, 13);
            this.lblFechaInicio.TabIndex = 102;
            this.lblFechaInicio.Text = "110_lblFechaInicio";
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(813, 118);
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
            this.dtFechaInicio.Size = new System.Drawing.Size(111, 20);
            this.dtFechaInicio.TabIndex = 101;
            // 
            // lblReporte
            // 
            this.lblReporte.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblReporte.Location = new System.Drawing.Point(734, 98);
            this.lblReporte.Name = "lblReporte";
            this.lblReporte.Size = new System.Drawing.Size(73, 13);
            this.lblReporte.TabIndex = 99;
            this.lblReporte.Text = "110_lblReporte";
            // 
            // cmbReporte
            // 
            this.cmbReporte.Location = new System.Drawing.Point(813, 94);
            this.cmbReporte.Name = "cmbReporte";
            this.cmbReporte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbReporte.Size = new System.Drawing.Size(110, 20);
            this.cmbReporte.TabIndex = 98;
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
            this.pnlAIU.Location = new System.Drawing.Point(935, 58);
            this.pnlAIU.Margin = new System.Windows.Forms.Padding(2);
            this.pnlAIU.Name = "pnlAIU";
            this.pnlAIU.Size = new System.Drawing.Size(113, 100);
            this.pnlAIU.TabIndex = 97;
            this.pnlAIU.Text = "A.I.U.";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.BackColor = System.Drawing.Color.Transparent;
            this.lblEmpresa.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresa.Location = new System.Drawing.Point(68, 18);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(46, 16);
            this.lblEmpresa.TabIndex = 83;
            this.lblEmpresa.Text = "Empresa";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.BackColor = System.Drawing.Color.Transparent;
            this.lblCliente.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.Location = new System.Drawing.Point(33, 18);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(37, 16);
            this.lblCliente.TabIndex = 82;
            this.lblCliente.Text = "Cliente";
            // 
            // txtPorUtilEmp
            // 
            this.txtPorUtilEmp.EditValue = "0,00 ";
            this.txtPorUtilEmp.Location = new System.Drawing.Point(74, 71);
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
            this.txtPorUtilEmp.Size = new System.Drawing.Size(28, 21);
            this.txtPorUtilEmp.TabIndex = 81;
            // 
            // txtPorImprEmp
            // 
            this.txtPorImprEmp.EditValue = "0,00 ";
            this.txtPorImprEmp.Location = new System.Drawing.Point(74, 53);
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
            this.txtPorImprEmp.Size = new System.Drawing.Size(28, 21);
            this.txtPorImprEmp.TabIndex = 80;
            // 
            // txtPorAdmEmp
            // 
            this.txtPorAdmEmp.EditValue = "0,00 ";
            this.txtPorAdmEmp.Location = new System.Drawing.Point(74, 35);
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
            this.txtPorAdmEmp.Size = new System.Drawing.Size(28, 21);
            this.txtPorAdmEmp.TabIndex = 79;
            // 
            // txtPorUtilClient
            // 
            this.txtPorUtilClient.EditValue = "0,00 ";
            this.txtPorUtilClient.Location = new System.Drawing.Point(39, 71);
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
            this.txtPorUtilClient.Size = new System.Drawing.Size(28, 21);
            this.txtPorUtilClient.TabIndex = 78;
            // 
            // txtPorImprClient
            // 
            this.txtPorImprClient.EditValue = "0,00 ";
            this.txtPorImprClient.Location = new System.Drawing.Point(38, 53);
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
            this.txtPorImprClient.Size = new System.Drawing.Size(28, 21);
            this.txtPorImprClient.TabIndex = 77;
            // 
            // lblUtil
            // 
            this.lblUtil.AutoSize = true;
            this.lblUtil.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtil.Location = new System.Drawing.Point(5, 72);
            this.lblUtil.Name = "lblUtil";
            this.lblUtil.Size = new System.Drawing.Size(24, 14);
            this.lblUtil.TabIndex = 76;
            this.lblUtil.Text = "Util";
            // 
            // lblImpr
            // 
            this.lblImpr.AutoSize = true;
            this.lblImpr.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImpr.Location = new System.Drawing.Point(5, 54);
            this.lblImpr.Name = "lblImpr";
            this.lblImpr.Size = new System.Drawing.Size(32, 14);
            this.lblImpr.TabIndex = 75;
            this.lblImpr.Text = "Impr";
            // 
            // lblADM
            // 
            this.lblADM.AutoSize = true;
            this.lblADM.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblADM.Location = new System.Drawing.Point(5, 36);
            this.lblADM.Name = "lblADM";
            this.lblADM.Size = new System.Drawing.Size(32, 14);
            this.lblADM.TabIndex = 73;
            this.lblADM.Text = "Adm";
            // 
            // txtPorAdmClient
            // 
            this.txtPorAdmClient.EditValue = "0,00 ";
            this.txtPorAdmClient.Location = new System.Drawing.Point(38, 35);
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
            this.txtPorAdmClient.Size = new System.Drawing.Size(28, 21);
            this.txtPorAdmClient.TabIndex = 74;
            // 
            // lblSolicitante
            // 
            this.lblSolicitante.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblSolicitante.Location = new System.Drawing.Point(317, 52);
            this.lblSolicitante.Name = "lblSolicitante";
            this.lblSolicitante.Size = new System.Drawing.Size(106, 13);
            this.lblSolicitante.TabIndex = 61;
            this.lblSolicitante.Text = "101_lblNombrEmpresa";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(13, 46);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(304, 21);
            this.masterCliente.TabIndex = 5;
            this.masterCliente.Value = "";
            // 
            // txtAF
            // 
            this.txtAF.Enabled = false;
            this.txtAF.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAF.Location = new System.Drawing.Point(774, 1);
            this.txtAF.Multiline = true;
            this.txtAF.Name = "txtAF";
            this.txtAF.Size = new System.Drawing.Size(91, 19);
            this.txtAF.TabIndex = 5;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Enabled = false;
            this.txtPrefix.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrefix.Location = new System.Drawing.Point(940, 1);
            this.txtPrefix.Multiline = true;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(50, 19);
            this.txtPrefix.TabIndex = 6;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(454, 1);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 3;
            this.dtPeriod.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.dtPeriod_EditValueChanged);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAF.Location = new System.Drawing.Point(748, 4);
            this.lblAF.Margin = new System.Windows.Forms.Padding(4);
            this.lblAF.Name = "lblAF";
            this.lblAF.Size = new System.Drawing.Size(69, 14);
            this.lblAF.TabIndex = 96;
            this.lblAF.Text = "1005_lblAF";
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblObservaciones.Location = new System.Drawing.Point(317, 142);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(105, 13);
            this.lblObservaciones.TabIndex = 66;
            this.lblObservaciones.Text = "110_lblObservaciones";
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
            // lblResponableEmp
            // 
            this.lblResponableEmp.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblResponableEmp.Location = new System.Drawing.Point(626, 51);
            this.lblResponableEmp.Name = "lblResponableEmp";
            this.lblResponableEmp.Size = new System.Drawing.Size(110, 13);
            this.lblResponableEmp.TabIndex = 65;
            this.lblResponableEmp.Text = "101_lblResponableEmp";
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
            // lblTelefono
            // 
            this.lblTelefono.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTelefono.Location = new System.Drawing.Point(521, 75);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(76, 13);
            this.lblTelefono.TabIndex = 64;
            this.lblTelefono.Text = "101_lblTelefono";
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
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(593, 72);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(72, 20);
            this.txtTelefono.TabIndex = 14;
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Enabled = false;
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroDoc.Location = new System.Drawing.Point(321, 1);
            this.txtNumeroDoc.Multiline = true;
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 19);
            this.txtNumeroDoc.TabIndex = 2;
            this.txtNumeroDoc.Leave += new System.EventHandler(this.txtNumeroDoc_Leave);
            // 
            // lblCorreo
            // 
            this.lblCorreo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblCorreo.Location = new System.Drawing.Point(317, 75);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(67, 13);
            this.lblCorreo.TabIndex = 63;
            this.lblCorreo.Text = "101_lblCorreo";
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroDoc.Location = new System.Drawing.Point(307, 4);
            this.lblNumeroDoc.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumeroDoc.Name = "lblNumeroDoc";
            this.lblNumeroDoc.Size = new System.Drawing.Size(10, 14);
            this.lblNumeroDoc.TabIndex = 92;
            this.lblNumeroDoc.Text = "#";
            // 
            // txtCorreo
            // 
            this.txtCorreo.Location = new System.Drawing.Point(414, 71);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(94, 20);
            this.txtCorreo.TabIndex = 9;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefix.Location = new System.Drawing.Point(881, 4);
            this.lblPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(89, 14);
            this.lblPrefix.TabIndex = 93;
            this.lblPrefix.Text = "1005_lblPrefix";
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(586, 4);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(83, 14);
            this.lblDate.TabIndex = 94;
            this.lblDate.Text = "1005_lblDate";
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(392, 4);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(93, 14);
            this.lblPeriod.TabIndex = 82;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipoSolicitud.Location = new System.Drawing.Point(12, 140);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(83, 13);
            this.lblTipoSolicitud.TabIndex = 60;
            this.lblTipoSolicitud.Text = "101_TipoSolicitud";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(630, 1);
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
            this.dtFecha.TabIndex = 4;
            this.dtFecha.Enter += new System.EventHandler(this.dtFecha_Enter);
            this.dtFecha.Leave += new System.EventHandler(this.dtFecha_Leave);
            // 
            // cmbTipoSolicitud
            // 
            this.cmbTipoSolicitud.Location = new System.Drawing.Point(111, 137);
            this.cmbTipoSolicitud.Name = "cmbTipoSolicitud";
            this.cmbTipoSolicitud.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoSolicitud.Size = new System.Drawing.Size(100, 20);
            this.cmbTipoSolicitud.TabIndex = 15;
            // 
            // masterClaseServicio
            // 
            this.masterClaseServicio.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseServicio.Filtros = null;
            this.masterClaseServicio.Location = new System.Drawing.Point(13, 22);
            this.masterClaseServicio.Margin = new System.Windows.Forms.Padding(4);
            this.masterClaseServicio.Name = "masterClaseServicio";
            this.masterClaseServicio.Size = new System.Drawing.Size(304, 21);
            this.masterClaseServicio.TabIndex = 0;
            this.masterClaseServicio.Value = "";
            this.masterClaseServicio.Leave += new System.EventHandler(this.masterClaseServicio_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(13, 90);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(292, 21);
            this.masterProyecto.TabIndex = 11;
            this.masterProyecto.Value = "";
            this.masterProyecto.Load += new System.EventHandler(this.masterProyecto_Load);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(317, 119);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(88, 13);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "110_lblDescripcion";
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(565, 24);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(30, 20);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblResponsableCli
            // 
            this.lblResponsableCli.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblResponsableCli.Location = new System.Drawing.Point(11, 74);
            this.lblResponsableCli.Name = "lblResponsableCli";
            this.lblResponsableCli.Size = new System.Drawing.Size(106, 13);
            this.lblResponsableCli.TabIndex = 50;
            this.lblResponsableCli.Text = "101_lblResponsableCli";
            // 
            // txtResposableCli
            // 
            this.txtResposableCli.Location = new System.Drawing.Point(113, 70);
            this.txtResposableCli.Name = "txtResposableCli";
            this.txtResposableCli.Size = new System.Drawing.Size(155, 20);
            this.txtResposableCli.TabIndex = 8;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(596, 24);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(24, 20);
            this.btnQueryDoc.TabIndex = 3;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // masterResponsableEmp
            // 
            this.masterResponsableEmp.BackColor = System.Drawing.Color.Transparent;
            this.masterResponsableEmp.Filtros = null;
            this.masterResponsableEmp.Location = new System.Drawing.Point(627, 46);
            this.masterResponsableEmp.Margin = new System.Windows.Forms.Padding(4);
            this.masterResponsableEmp.Name = "masterResponsableEmp";
            this.masterResponsableEmp.Size = new System.Drawing.Size(297, 23);
            this.masterResponsableEmp.TabIndex = 7;
            this.masterResponsableEmp.Value = "";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(414, 141);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtObservaciones.Properties.ShowIcon = false;
            this.txtObservaciones.Size = new System.Drawing.Size(172, 20);
            this.txtObservaciones.TabIndex = 13;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(414, 117);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDescripcion.Properties.ShowIcon = false;
            this.txtDescripcion.Size = new System.Drawing.Size(173, 20);
            this.txtDescripcion.TabIndex = 16;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.groupControl3);
            this.pnlDetail.Controls.Add(this.groupControl1);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(13, 543);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1050, 70);
            this.pnlDetail.TabIndex = 112;
            // 
            // groupControl3
            // 
            this.groupControl3.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.groupControl3.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.groupControl3.AppearanceCaption.Options.UseFont = true;
            this.groupControl3.Controls.Add(this.panelControl1);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl3.Location = new System.Drawing.Point(573, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(163, 70);
            this.groupControl3.TabIndex = 98;
            this.groupControl3.Text = "Detalle Recursos";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtModelo);
            this.panelControl1.Controls.Add(this.txtMarca);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Location = new System.Drawing.Point(7, 23);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(139, 45);
            this.panelControl1.TabIndex = 86;
            // 
            // txtModelo
            // 
            this.txtModelo.EditValue = "";
            this.txtModelo.Location = new System.Drawing.Point(58, 3);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtModelo.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold);
            this.txtModelo.Properties.Appearance.Options.UseBorderColor = true;
            this.txtModelo.Properties.Appearance.Options.UseFont = true;
            this.txtModelo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtModelo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtModelo.Properties.AutoHeight = false;
            this.txtModelo.Properties.ReadOnly = true;
            this.txtModelo.Size = new System.Drawing.Size(77, 18);
            this.txtModelo.TabIndex = 83;
            // 
            // txtMarca
            // 
            this.txtMarca.EditValue = "";
            this.txtMarca.Location = new System.Drawing.Point(58, 23);
            this.txtMarca.Name = "txtMarca";
            this.txtMarca.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtMarca.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold);
            this.txtMarca.Properties.Appearance.Options.UseBorderColor = true;
            this.txtMarca.Properties.Appearance.Options.UseFont = true;
            this.txtMarca.Properties.Appearance.Options.UseTextOptions = true;
            this.txtMarca.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtMarca.Properties.AutoHeight = false;
            this.txtMarca.Properties.ReadOnly = true;
            this.txtMarca.Size = new System.Drawing.Size(77, 18);
            this.txtMarca.TabIndex = 81;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 15);
            this.label3.TabIndex = 82;
            this.label3.Text = "Modelo/Ref";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 80;
            this.label4.Text = "Marca";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.txtSortByCap);
            this.groupControl1.Controls.Add(this.lblCostoPresupuesto);
            this.groupControl1.Controls.Add(this.txtCostoPresupuesto);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.txtCostoCliente);
            this.groupControl1.Controls.Add(this.txtCostoMult);
            this.groupControl1.Controls.Add(this.lblCostoCliente);
            this.groupControl1.Controls.Add(this.txtPorIVA);
            this.groupControl1.Controls.Add(this.txtIVA);
            this.groupControl1.Controls.Add(this.lblIVA);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(573, 70);
            this.groupControl1.TabIndex = 97;
            this.groupControl1.Text = "Resumen Costos  - Tiempo";
            // 
            // txtSortByCap
            // 
            this.txtSortByCap.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Bold);
            this.txtSortByCap.Appearance.Options.UseFont = true;
            this.txtSortByCap.Location = new System.Drawing.Point(498, 0);
            this.txtSortByCap.Name = "txtSortByCap";
            this.txtSortByCap.Size = new System.Drawing.Size(72, 18);
            toolTipTitleItem1.Text = "Genera una nueva versión de la Cotización o Licitación";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.txtSortByCap.SuperTip = superToolTip1;
            this.txtSortByCap.TabIndex = 99;
            this.txtSortByCap.Text = "Ordenar Capitulo";
            this.txtSortByCap.Click += new System.EventHandler(this.txtSortByCap_Click);
            // 
            // lblCostoPresupuesto
            // 
            this.lblCostoPresupuesto.AutoSize = true;
            this.lblCostoPresupuesto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoPresupuesto.Location = new System.Drawing.Point(5, 21);
            this.lblCostoPresupuesto.Name = "lblCostoPresupuesto";
            this.lblCostoPresupuesto.Size = new System.Drawing.Size(166, 14);
            this.lblCostoPresupuesto.TabIndex = 87;
            this.lblCostoPresupuesto.Text = "110_lblCostoPresupuesto";
            // 
            // txtCostoPresupuesto
            // 
            this.txtCostoPresupuesto.EditValue = "0,00 ";
            this.txtCostoPresupuesto.Location = new System.Drawing.Point(116, 21);
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
            this.txtCostoPresupuesto.Size = new System.Drawing.Size(124, 18);
            this.txtCostoPresupuesto.TabIndex = 88;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(256, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 14);
            this.label2.TabIndex = 94;
            this.label2.Text = "110_lblCostoMultip";
            // 
            // txtCostoCliente
            // 
            this.txtCostoCliente.EditValue = "0,00 ";
            this.txtCostoCliente.Location = new System.Drawing.Point(321, 21);
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
            this.txtCostoCliente.Size = new System.Drawing.Size(126, 18);
            this.txtCostoCliente.TabIndex = 90;
            // 
            // txtCostoMult
            // 
            this.txtCostoMult.EditValue = "0,00 ";
            this.txtCostoMult.Location = new System.Drawing.Point(321, 41);
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
            this.txtCostoMult.Size = new System.Drawing.Size(126, 18);
            this.txtCostoMult.TabIndex = 95;
            // 
            // lblCostoCliente
            // 
            this.lblCostoCliente.AutoSize = true;
            this.lblCostoCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostoCliente.Location = new System.Drawing.Point(256, 21);
            this.lblCostoCliente.Name = "lblCostoCliente";
            this.lblCostoCliente.Size = new System.Drawing.Size(131, 14);
            this.lblCostoCliente.TabIndex = 89;
            this.lblCostoCliente.Text = "110_lblCostoCliente";
            // 
            // txtPorIVA
            // 
            this.txtPorIVA.EditValue = "16";
            this.txtPorIVA.Location = new System.Drawing.Point(214, 40);
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
            this.txtPorIVA.Size = new System.Drawing.Size(26, 20);
            this.txtPorIVA.TabIndex = 93;
            // 
            // txtIVA
            // 
            this.txtIVA.EditValue = "0,00 ";
            this.txtIVA.Location = new System.Drawing.Point(117, 40);
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
            this.txtIVA.Size = new System.Drawing.Size(94, 20);
            this.txtIVA.TabIndex = 92;
            // 
            // lblIVA
            // 
            this.lblIVA.AutoSize = true;
            this.lblIVA.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIVA.Location = new System.Drawing.Point(5, 42);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(111, 14);
            this.lblIVA.TabIndex = 91;
            this.lblIVA.Text = "110_lblCostoIVA";
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(13, 196);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1050, 341);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcDocument);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.gcRecurso);
            this.splitGrids.Panel2.Controls.Add(this.grpCtrlProvider);
            this.splitGrids.Size = new System.Drawing.Size(1050, 341);
            this.splitGrids.SplitterPosition = 710;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // grpCtrlProvider
            // 
            this.grpCtrlProvider.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F);
            this.grpCtrlProvider.AppearanceCaption.Options.UseFont = true;
            this.grpCtrlProvider.Controls.Add(this.masterUnidadBaseDia);
            this.grpCtrlProvider.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCtrlProvider.Location = new System.Drawing.Point(0, 0);
            this.grpCtrlProvider.Margin = new System.Windows.Forms.Padding(2);
            this.grpCtrlProvider.Name = "grpCtrlProvider";
            this.grpCtrlProvider.ShowCaption = false;
            this.grpCtrlProvider.Size = new System.Drawing.Size(335, 34);
            this.grpCtrlProvider.TabIndex = 52;
            // 
            // masterUnidadBaseDia
            // 
            this.masterUnidadBaseDia.BackColor = System.Drawing.Color.Transparent;
            this.masterUnidadBaseDia.Filtros = null;
            this.masterUnidadBaseDia.Location = new System.Drawing.Point(11, 5);
            this.masterUnidadBaseDia.Margin = new System.Windows.Forms.Padding(4);
            this.masterUnidadBaseDia.Name = "masterUnidadBaseDia";
            this.masterUnidadBaseDia.Size = new System.Drawing.Size(267, 23);
            this.masterUnidadBaseDia.TabIndex = 73;
            this.masterUnidadBaseDia.Value = "";
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
            this.editValue2Cant,
            this.editValue6Cant,
            this.editValue4,
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
            // editSpin7
            // 
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
            // editValue2Cant
            // 
            this.editValue2Cant.AllowMouseWheel = false;
            this.editValue2Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.Mask.EditMask = "n2";
            this.editValue2Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2Cant.Name = "editValue2Cant";
            // 
            // editValue6Cant
            // 
            this.editValue6Cant.AllowMouseWheel = false;
            this.editValue6Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue6Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue6Cant.Mask.EditMask = "n6";
            this.editValue6Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue6Cant.Name = "editValue6Cant";
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
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorcen.Name = "editSpinPorcen";
            // 
            // PlaneacionTiempo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 620);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "PlaneacionTiempo";
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
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPresup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTiempoAutInd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAPUIncluyeAIU.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJerarquia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefono.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorreo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResposableCli.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtModelo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMarca.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoPresupuesto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoCliente.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoMult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorIVA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVA.Properties)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).EndInit();
            this.grpCtrlProvider.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue6Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
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
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue6Cant;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalleRecurso;
        private DevExpress.XtraEditors.CheckEdit chkRecursoXTrabInd;
        private DevExpress.XtraEditors.LabelControl lblObservaciones;
        private DevExpress.XtraEditors.LabelControl lblResponableEmp;
        private DevExpress.XtraEditors.LabelControl lblTelefono;
        private DevExpress.XtraEditors.TextEdit txtTelefono;
        private DevExpress.XtraEditors.LabelControl lblCorreo;
        private DevExpress.XtraEditors.TextEdit txtCorreo;
        private DevExpress.XtraEditors.LabelControl lblSolicitante;
        private DevExpress.XtraEditors.LabelControl lblTipoSolicitud;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoSolicitud;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.LabelControl lblResponsableCli;
        private DevExpress.XtraEditors.TextEdit txtResposableCli;
        private uc_MasterFind masterResponsableEmp;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private uc_MasterFind masterPrefijo;
        private uc_MasterFind masterProyecto;
        private uc_MasterFind masterCliente;
        private uc_MasterFind masterClaseServicio;
        private uc_MasterFind masterAreaFuncional;
        private DevExpress.XtraEditors.GroupControl grpCtrlProvider;
        private uc_MasterFind masterUnidadBaseDia;
        private DevExpress.XtraEditors.MemoExEdit txtSolicitante;
        private DevExpress.XtraEditors.MemoExEdit txtObservaciones;
        private DevExpress.XtraEditors.MemoExEdit txtDescripcion;
        private DevExpress.XtraEditors.LabelControl lblFechaInicio;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblReporte;
        private DevExpress.XtraEditors.LookUpEdit cmbReporte;
        private DevExpress.XtraEditors.GroupControl pnlAIU;
        private System.Windows.Forms.Label lblEmpresa;
        private System.Windows.Forms.Label lblCliente;
        private DevExpress.XtraEditors.TextEdit txtPorUtilEmp;
        private DevExpress.XtraEditors.TextEdit txtPorImprEmp;
        private DevExpress.XtraEditors.TextEdit txtPorAdmEmp;
        private DevExpress.XtraEditors.TextEdit txtPorUtilClient;
        private DevExpress.XtraEditors.TextEdit txtPorImprClient;
        private System.Windows.Forms.Label lblUtil;
        private System.Windows.Forms.Label lblImpr;
        private System.Windows.Forms.Label lblADM;
        private DevExpress.XtraEditors.TextEdit txtPorAdmClient;
        private uc_MasterFind masterCentroCto;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private DevExpress.XtraEditors.MemoExEdit txtLicitacion;
        private DevExpress.XtraEditors.LabelControl lblJerarquia;
        private DevExpress.XtraEditors.LookUpEdit cmbJerarquia;
        private DevExpress.XtraEditors.CheckEdit chkAPUIncluyeAIU;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtCostoMult;
        private DevExpress.XtraEditors.TextEdit txtPorIVA;
        private System.Windows.Forms.Label lblIVA;
        private DevExpress.XtraEditors.TextEdit txtIVA;
        private System.Windows.Forms.Label lblCostoCliente;
        private DevExpress.XtraEditors.TextEdit txtCostoCliente;
        private System.Windows.Forms.Label lblCostoPresupuesto;
        private DevExpress.XtraEditors.TextEdit txtCostoPresupuesto;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.CheckEdit chkTiempoAutInd;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoPresup;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit cmbProposito;
        private DevExpress.XtraEditors.SimpleButton txtSortByCap;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txtModelo;
        private DevExpress.XtraEditors.TextEdit txtMarca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}