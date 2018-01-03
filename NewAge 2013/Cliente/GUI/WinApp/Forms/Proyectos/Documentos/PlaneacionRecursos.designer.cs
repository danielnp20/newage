using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PlaneacionRecursos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaneacionRecursos));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcTarea = new DevExpress.XtraGrid.GridControl();
            this.gvTarea = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalleRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecurso = new DevExpress.XtraGrid.GridControl();
            this.gvRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.pn2 = new System.Windows.Forms.Panel();
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.txtObservaciones = new DevExpress.XtraEditors.MemoExEdit();
            this.lblFechaInicio = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.masterCentroCto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoExEdit();
            this.lblReporte = new DevExpress.XtraEditors.LabelControl();
            this.cmbReporte = new DevExpress.XtraEditors.LookUpEdit();
            this.lblObservaciones = new DevExpress.XtraEditors.LabelControl();
            this.lblJerarquia = new DevExpress.XtraEditors.LabelControl();
            this.cmbProposito = new DevExpress.XtraEditors.LookUpEdit();
            this.lblResponableEmp = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkRecursoXTrabInd = new DevExpress.XtraEditors.CheckEdit();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoSolicitud = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoSolicitud = new DevExpress.XtraEditors.LabelControl();
            this.lblSolicitante = new DevExpress.XtraEditors.LabelControl();
            this.masterResponsableEmp = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoExEdit();
            this.txtSolicitante = new DevExpress.XtraEditors.MemoExEdit();
            this.pn1 = new DevExpress.XtraEditors.PanelControl();
            this.masterAreaFuncional = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.pnlGrid2 = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
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
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue6Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.pn2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pn1)).BeginInit();
            this.pn1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGrid2)).BeginInit();
            this.pnlGrid2.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.editValue6Cant)).BeginInit();
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
            this.gvDetalle.GridControl = this.gcTarea;
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
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
            // 
            // gcTarea
            // 
            this.gcTarea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTarea.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcTarea.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcTarea.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcTarea.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcTarea.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcTarea.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcTarea.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcTarea.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcTarea.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcTarea.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcTarea.Location = new System.Drawing.Point(0, 21);
            this.gcTarea.LookAndFeel.SkinName = "Dark Side";
            this.gcTarea.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTarea.MainView = this.gvTarea;
            this.gcTarea.Margin = new System.Windows.Forms.Padding(5);
            this.gcTarea.Name = "gcTarea";
            this.gcTarea.Size = new System.Drawing.Size(1157, 139);
            this.gcTarea.TabIndex = 50;
            this.gcTarea.UseEmbeddedNavigator = true;
            this.gcTarea.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTarea,
            this.gvDetalle});
            // 
            // gvTarea
            // 
            this.gvTarea.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTarea.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Empty.Options.UseBackColor = true;
            this.gvTarea.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTarea.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTarea.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvTarea.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvTarea.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTarea.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTarea.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvTarea.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.Row.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.Options.UseFont = true;
            this.gvTarea.Appearance.Row.Options.UseForeColor = true;
            this.gvTarea.Appearance.Row.Options.UseTextOptions = true;
            this.gvTarea.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTarea.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvTarea.Appearance.ViewCaption.Options.UseFont = true;
            this.gvTarea.GridControl = this.gcTarea;
            this.gvTarea.HorzScrollStep = 50;
            this.gvTarea.Name = "gvTarea";
            this.gvTarea.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvTarea.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvTarea.OptionsCustomization.AllowColumnMoving = false;
            this.gvTarea.OptionsCustomization.AllowFilter = false;
            this.gvTarea.OptionsCustomization.AllowSort = false;
            this.gvTarea.OptionsDetail.EnableMasterViewMode = false;
            this.gvTarea.OptionsMenu.EnableColumnMenu = false;
            this.gvTarea.OptionsMenu.EnableFooterMenu = false;
            this.gvTarea.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvTarea.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvTarea.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvTarea.OptionsView.ColumnAutoWidth = false;
            this.gvTarea.OptionsView.ShowGroupPanel = false;
            this.gvTarea.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvTarea.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTarea_FocusedRowChanged);
            this.gvTarea.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvTarea_CellValueChanged);
            this.gvTarea.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvTarea_CellValueChanging);
            this.gvTarea.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvTarea_BeforeLeaveRow);
            this.gvTarea.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
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
            this.gvDetalleRecurso.Appearance.GroupRow.BackColor = System.Drawing.Color.DarkSlateGray;
            this.gvDetalleRecurso.Appearance.GroupRow.BackColor2 = System.Drawing.Color.DimGray;
            this.gvDetalleRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
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
            this.gvDetalleRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
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
            this.gcRecurso.Location = new System.Drawing.Point(0, 0);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(1157, 249);
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
            this.gvRecurso.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gvRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.Row.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.Options.UseFont = true;
            this.gvRecurso.Appearance.Row.Options.UseForeColor = true;
            this.gvRecurso.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecurso.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvRecurso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.SelectedRow.Options.UseFont = true;
            this.gvRecurso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecurso.GridControl = this.gcRecurso;
            this.gvRecurso.GroupFormat = "[#image]{1} ";
            this.gvRecurso.GroupRowHeight = 21;
            this.gvRecurso.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_CostoLocalTOT", null, "(SubTotal={0:c2})")});
            this.gvRecurso.HorzScrollStep = 50;
            this.gvRecurso.Name = "gvRecurso";
            this.gvRecurso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecurso.OptionsCustomization.AllowFilter = false;
            this.gvRecurso.OptionsCustomization.AllowSort = false;
            this.gvRecurso.OptionsDetail.EnableMasterViewMode = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvRecurso.OptionsView.ShowAutoFilterRow = true;
            this.gvRecurso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecurso.OptionsView.ShowGroupPanel = false;
            this.gvRecurso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecurso.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecurso_CustomRowCellEdit);
            this.gvRecurso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
            this.gvRecurso.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvRecurso_CellValueChanged);
            this.gvRecurso.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvRecurso_BeforeLeaveRow);
            this.gvRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
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
            this.pnlMainContainer.Size = new System.Drawing.Size(1195, 596);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.41652F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.58348F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1191, 592);
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
            this.grpctrlHeader.Location = new System.Drawing.Point(12, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1157, 152);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // pn2
            // 
            this.pn2.Controls.Add(this.chkSelectAll);
            this.pn2.Controls.Add(this.labelControl2);
            this.pn2.Controls.Add(this.txtTasaCambio);
            this.pn2.Controls.Add(this.txtObservaciones);
            this.pn2.Controls.Add(this.lblFechaInicio);
            this.pn2.Controls.Add(this.dtFechaInicio);
            this.pn2.Controls.Add(this.masterCentroCto);
            this.pn2.Controls.Add(this.lblLicitacion);
            this.pn2.Controls.Add(this.txtLicitacion);
            this.pn2.Controls.Add(this.lblReporte);
            this.pn2.Controls.Add(this.cmbReporte);
            this.pn2.Controls.Add(this.lblObservaciones);
            this.pn2.Controls.Add(this.lblJerarquia);
            this.pn2.Controls.Add(this.cmbProposito);
            this.pn2.Controls.Add(this.lblResponableEmp);
            this.pn2.Controls.Add(this.masterCliente);
            this.pn2.Controls.Add(this.masterProyecto);
            this.pn2.Controls.Add(this.chkRecursoXTrabInd);
            this.pn2.Controls.Add(this.lblDescripcion);
            this.pn2.Controls.Add(this.cmbTipoSolicitud);
            this.pn2.Controls.Add(this.lblTipoSolicitud);
            this.pn2.Controls.Add(this.lblSolicitante);
            this.pn2.Controls.Add(this.masterResponsableEmp);
            this.pn2.Controls.Add(this.txtDescripcion);
            this.pn2.Controls.Add(this.txtSolicitante);
            this.pn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn2.Location = new System.Drawing.Point(2, 50);
            this.pn2.Margin = new System.Windows.Forms.Padding(2);
            this.pn2.Name = "pn2";
            this.pn2.Size = new System.Drawing.Size(1153, 100);
            this.pn2.TabIndex = 72;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAll.Location = new System.Drawing.Point(998, 68);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.chkSelectAll.Properties.Appearance.Options.UseFont = true;
            this.chkSelectAll.Properties.AutoWidth = true;
            this.chkSelectAll.Properties.Caption = "33508_chkSelectAll";
            this.chkSelectAll.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.chkSelectAll.Properties.LookAndFeel.SkinName = "Foggy";
            this.chkSelectAll.Size = new System.Drawing.Size(146, 22);
            this.chkSelectAll.TabIndex = 71;
            this.chkSelectAll.ToolTip = "Solicitud la cantidad total a Terceros";
            this.chkSelectAll.ToolTipTitle = "Solicitud";
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl2.Location = new System.Drawing.Point(594, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(92, 13);
            this.labelControl2.TabIndex = 118;
            this.labelControl2.Text = "110_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0,00 ";
            this.txtTasaCambio.Location = new System.Drawing.Point(694, 72);
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
            this.txtTasaCambio.Size = new System.Drawing.Size(92, 20);
            this.txtTasaCambio.TabIndex = 117;
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(421, 50);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtObservaciones.Properties.ShowIcon = false;
            this.txtObservaciones.Size = new System.Drawing.Size(161, 20);
            this.txtObservaciones.TabIndex = 8;
            this.txtObservaciones.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblFechaInicio.Location = new System.Drawing.Point(594, 53);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(88, 13);
            this.lblFechaInicio.TabIndex = 86;
            this.lblFechaInicio.Text = "110_lblFechaInicio";
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(694, 50);
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
            this.dtFechaInicio.Size = new System.Drawing.Size(92, 20);
            this.dtFechaInicio.TabIndex = 85;
            // 
            // masterCentroCto
            // 
            this.masterCentroCto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCto.Filtros = null;
            this.masterCentroCto.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterCentroCto.Location = new System.Drawing.Point(9, 46);
            this.masterCentroCto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCentroCto.Name = "masterCentroCto";
            this.masterCentroCto.Size = new System.Drawing.Size(292, 21);
            this.masterCentroCto.TabIndex = 84;
            this.masterCentroCto.Value = "";
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(317, 29);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(77, 13);
            this.lblLicitacion.TabIndex = 77;
            this.lblLicitacion.Text = "110_lblLicitacion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(421, 26);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLicitacion.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.txtLicitacion.Properties.ShowIcon = false;
            this.txtLicitacion.Size = new System.Drawing.Size(161, 20);
            this.txtLicitacion.TabIndex = 76;
            this.txtLicitacion.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // lblReporte
            // 
            this.lblReporte.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblReporte.Location = new System.Drawing.Point(594, 29);
            this.lblReporte.Name = "lblReporte";
            this.lblReporte.Size = new System.Drawing.Size(73, 13);
            this.lblReporte.TabIndex = 75;
            this.lblReporte.Text = "110_lblReporte";
            // 
            // cmbReporte
            // 
            this.cmbReporte.Location = new System.Drawing.Point(694, 28);
            this.cmbReporte.Name = "cmbReporte";
            this.cmbReporte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbReporte.Size = new System.Drawing.Size(92, 20);
            this.cmbReporte.TabIndex = 74;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblObservaciones.Location = new System.Drawing.Point(317, 52);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(105, 13);
            this.lblObservaciones.TabIndex = 66;
            this.lblObservaciones.Text = "110_lblObservaciones";
            // 
            // lblJerarquia
            // 
            this.lblJerarquia.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblJerarquia.Location = new System.Drawing.Point(181, 76);
            this.lblJerarquia.Name = "lblJerarquia";
            this.lblJerarquia.Size = new System.Drawing.Size(79, 13);
            this.lblJerarquia.TabIndex = 72;
            this.lblJerarquia.Text = "110_lblProposito";
            // 
            // cmbProposito
            // 
            this.cmbProposito.Location = new System.Drawing.Point(242, 72);
            this.cmbProposito.Name = "cmbProposito";
            this.cmbProposito.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProposito.Size = new System.Drawing.Size(61, 20);
            this.cmbProposito.TabIndex = 71;
            // 
            // lblResponableEmp
            // 
            this.lblResponableEmp.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblResponableEmp.Location = new System.Drawing.Point(591, 6);
            this.lblResponableEmp.Name = "lblResponableEmp";
            this.lblResponableEmp.Size = new System.Drawing.Size(110, 13);
            this.lblResponableEmp.TabIndex = 65;
            this.lblResponableEmp.Text = "101_lblResponableEmp";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterCliente.Location = new System.Drawing.Point(10, 0);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(300, 22);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterProyecto.Location = new System.Drawing.Point(10, 23);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(300, 21);
            this.masterProyecto.TabIndex = 6;
            this.masterProyecto.Value = "";
            // 
            // chkRecursoXTrabInd
            // 
            this.chkRecursoXTrabInd.Location = new System.Drawing.Point(791, 29);
            this.chkRecursoXTrabInd.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecursoXTrabInd.Name = "chkRecursoXTrabInd";
            this.chkRecursoXTrabInd.Properties.AutoWidth = true;
            this.chkRecursoXTrabInd.Properties.Caption = "110_chkRecursoXTrab";
            this.chkRecursoXTrabInd.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style1;
            this.chkRecursoXTrabInd.Properties.LookAndFeel.SkinName = "Foggy";
            this.chkRecursoXTrabInd.Size = new System.Drawing.Size(132, 22);
            this.chkRecursoXTrabInd.TabIndex = 70;
            this.chkRecursoXTrabInd.Visible = false;
            this.chkRecursoXTrabInd.CheckedChanged += new System.EventHandler(this.chkRecursoXTrabInd_CheckedChanged);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(317, 75);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(88, 13);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "110_lblDescripcion";
            // 
            // cmbTipoSolicitud
            // 
            this.cmbTipoSolicitud.Location = new System.Drawing.Point(108, 72);
            this.cmbTipoSolicitud.Name = "cmbTipoSolicitud";
            this.cmbTipoSolicitud.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoSolicitud.Size = new System.Drawing.Size(71, 20);
            this.cmbTipoSolicitud.TabIndex = 9;
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipoSolicitud.Location = new System.Drawing.Point(10, 76);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(83, 13);
            this.lblTipoSolicitud.TabIndex = 60;
            this.lblTipoSolicitud.Text = "101_TipoSolicitud";
            // 
            // lblSolicitante
            // 
            this.lblSolicitante.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblSolicitante.Location = new System.Drawing.Point(317, 6);
            this.lblSolicitante.Name = "lblSolicitante";
            this.lblSolicitante.Size = new System.Drawing.Size(106, 13);
            this.lblSolicitante.TabIndex = 61;
            this.lblSolicitante.Text = "101_lblNombrEmpresa";
            // 
            // masterResponsableEmp
            // 
            this.masterResponsableEmp.BackColor = System.Drawing.Color.Transparent;
            this.masterResponsableEmp.Filtros = null;
            this.masterResponsableEmp.Location = new System.Drawing.Point(594, 2);
            this.masterResponsableEmp.Margin = new System.Windows.Forms.Padding(4);
            this.masterResponsableEmp.Name = "masterResponsableEmp";
            this.masterResponsableEmp.Size = new System.Drawing.Size(296, 21);
            this.masterResponsableEmp.TabIndex = 2;
            this.masterResponsableEmp.Value = "";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(421, 72);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDescripcion.Properties.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.FrameResize;
            this.txtDescripcion.Properties.ShowIcon = false;
            this.txtDescripcion.Size = new System.Drawing.Size(161, 20);
            this.txtDescripcion.TabIndex = 10;
            this.txtDescripcion.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // txtSolicitante
            // 
            this.txtSolicitante.Location = new System.Drawing.Point(421, 3);
            this.txtSolicitante.Name = "txtSolicitante";
            this.txtSolicitante.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtSolicitante.Properties.ShowIcon = false;
            this.txtSolicitante.Size = new System.Drawing.Size(161, 20);
            this.txtSolicitante.TabIndex = 1;
            this.txtSolicitante.MouseHover += new System.EventHandler(this.txt_MouseHover);
            // 
            // pn1
            // 
            this.pn1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pn1.Controls.Add(this.masterAreaFuncional);
            this.pn1.Controls.Add(this.txtNro);
            this.pn1.Controls.Add(this.lblNro);
            this.pn1.Controls.Add(this.btnQueryDoc);
            this.pn1.Controls.Add(this.masterPrefijo);
            this.pn1.Controls.Add(this.masterClaseServicio);
            this.pn1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pn1.Location = new System.Drawing.Point(2, 22);
            this.pn1.Margin = new System.Windows.Forms.Padding(2);
            this.pn1.Name = "pn1";
            this.pn1.Size = new System.Drawing.Size(1153, 28);
            this.pn1.TabIndex = 71;
            // 
            // masterAreaFuncional
            // 
            this.masterAreaFuncional.BackColor = System.Drawing.Color.Transparent;
            this.masterAreaFuncional.Filtros = null;
            this.masterAreaFuncional.Location = new System.Drawing.Point(619, 3);
            this.masterAreaFuncional.Margin = new System.Windows.Forms.Padding(4);
            this.masterAreaFuncional.Name = "masterAreaFuncional";
            this.masterAreaFuncional.Size = new System.Drawing.Size(266, 24);
            this.masterAreaFuncional.TabIndex = 3;
            this.masterAreaFuncional.Value = "";
            this.masterAreaFuncional.Leave += new System.EventHandler(this.masterAreaFuncional_Leave);
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(553, 6);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(30, 20);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(520, 9);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(51, 13);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "110_lblNro";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(584, 6);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(24, 20);
            this.btnQueryDoc.TabIndex = 42;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(319, 4);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(198, 26);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // masterClaseServicio
            // 
            this.masterClaseServicio.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseServicio.Filtros = null;
            this.masterClaseServicio.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterClaseServicio.Location = new System.Drawing.Point(10, 4);
            this.masterClaseServicio.Margin = new System.Windows.Forms.Padding(4);
            this.masterClaseServicio.Name = "masterClaseServicio";
            this.masterClaseServicio.Size = new System.Drawing.Size(305, 27);
            this.masterClaseServicio.TabIndex = 0;
            this.masterClaseServicio.Value = "";
            this.masterClaseServicio.Leave += new System.EventHandler(this.masterClaseServicio_Leave);
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
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Enabled = false;
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroDoc.Location = new System.Drawing.Point(321, 1);
            this.txtNumeroDoc.Multiline = true;
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 19);
            this.txtNumeroDoc.TabIndex = 2;
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
            this.dtFecha.Leave += new System.EventHandler(this.dtFecha_Leave);
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.simpleButton1);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(12, 581);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1157, 8);
            this.pnlDetail.TabIndex = 112;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseTextOptions = true;
            this.simpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleButton1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleButton1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.simpleButton1.Location = new System.Drawing.Point(954, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(193, 15);
            this.simpleButton1.TabIndex = 119;
            this.simpleButton1.Text = "Exportar Items  para Inventario";
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.btnExportRecPropios_Click);
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(12, 161);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1157, 414);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcRecurso);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.Controls.Add(this.gcTarea);
            this.splitGrids.Panel2.Controls.Add(this.pnlGrid2);
            this.splitGrids.Panel2.Text = "Panel2";
            this.splitGrids.Size = new System.Drawing.Size(1157, 414);
            this.splitGrids.SplitterPosition = 249;
            this.splitGrids.TabIndex = 2;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // pnlGrid2
            // 
            this.pnlGrid2.Controls.Add(this.lblTitle);
            this.pnlGrid2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrid2.Location = new System.Drawing.Point(0, 0);
            this.pnlGrid2.Name = "pnlGrid2";
            this.pnlGrid2.Size = new System.Drawing.Size(1157, 21);
            this.pnlGrid2.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(11, 3);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(101, 14);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "33508_lblTareas";
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
            this.editValue6Cant,
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Stock", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.HideSelection = false;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
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
            // editValue6Cant
            // 
            this.editValue6Cant.AllowMouseWheel = false;
            this.editValue6Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue6Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue6Cant.Mask.EditMask = "n6";
            this.editValue6Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue6Cant.Name = "editValue6Cant";
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
            // PlaneacionRecursos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 596);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "PlaneacionRecursos";
            this.Text = "1005";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReporte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProposito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecursoXTrabInd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSolicitud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSolicitante.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pn1)).EndInit();
            this.pn1.ResumeLayout(false);
            this.pn1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlGrid2)).EndInit();
            this.pnlGrid2.ResumeLayout(false);
            this.pnlGrid2.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.editValue6Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected DevExpress.XtraGrid.GridControl gcTarea;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvTarea;
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
        private DevExpress.XtraEditors.LookUpEdit cmbTipoSolicitud;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private uc_MasterFind masterResponsableEmp;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private uc_MasterFind masterPrefijo;
        private uc_MasterFind masterProyecto;
        private uc_MasterFind masterCliente;
        private uc_MasterFind masterClaseServicio;
        private uc_MasterFind masterAreaFuncional;
        private DevExpress.XtraEditors.PanelControl pn1;
        private System.Windows.Forms.Panel pn2;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue6Cant;
        private DevExpress.XtraEditors.MemoExEdit txtDescripcion;
        private DevExpress.XtraEditors.MemoExEdit txtObservaciones;
        private DevExpress.XtraEditors.LabelControl lblReporte;
        private DevExpress.XtraEditors.LookUpEdit cmbReporte;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private DevExpress.XtraEditors.MemoExEdit txtLicitacion;
        private DevExpress.XtraEditors.MemoExEdit txtSolicitante;
        private DevExpress.XtraEditors.PanelControl pnlGrid2;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private uc_MasterFind masterCentroCto;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.LabelControl lblJerarquia;
        private DevExpress.XtraEditors.LookUpEdit cmbProposito;
        private DevExpress.XtraEditors.CheckEdit chkSelectAll;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}