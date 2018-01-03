namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TaskForm
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode3 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.tcTask = new DevExpress.XtraTab.XtraTabControl();
            this.tpPendientes = new DevExpress.XtraTab.XtraTabPage();
            this.gcDocPendientes = new DevExpress.XtraGrid.GridControl();
            this.gvDocPendientes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gbFilter = new DevExpress.XtraEditors.GroupControl();
            this.lblActividadFluj = new DevExpress.XtraEditors.LabelControl();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.dtFechaFinPend = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.dtFechaIniPend = new DevExpress.XtraEditors.DateEdit();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterActividadFlujoPend = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbOrden = new DevExpress.XtraEditors.LookUpEdit();
            this.lblOrden = new DevExpress.XtraEditors.LabelControl();
            this.tpLlamadas = new DevExpress.XtraTab.XtraTabPage();
            this.gcDocLLamadas = new DevExpress.XtraGrid.GridControl();
            this.gvDocLlamadas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnLlamadas = new DevExpress.XtraEditors.PanelControl();
            this.lblCorreoLlam = new DevExpress.XtraEditors.LabelControl();
            this.txtCorreoLlam = new System.Windows.Forms.TextBox();
            this.lblDireccionLlam = new DevExpress.XtraEditors.LabelControl();
            this.txtDireccionLlam = new System.Windows.Forms.TextBox();
            this.lblTelefonoLlam = new DevExpress.XtraEditors.LabelControl();
            this.txtTelefonoLlam = new System.Windows.Forms.TextBox();
            this.gbFilterLlamada = new DevExpress.XtraEditors.GroupControl();
            this.lblActivFlujoLlam = new DevExpress.XtraEditors.LabelControl();
            this.masterActivFlujoLlam = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaFinLlam = new System.Windows.Forms.Label();
            this.lblDocNroLlam = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaFinLlam = new DevExpress.XtraEditors.DateEdit();
            this.txtDocNroLlam = new System.Windows.Forms.TextBox();
            this.lblFechaIniLlam = new System.Windows.Forms.Label();
            this.dtFechaIniLlam = new DevExpress.XtraEditors.DateEdit();
            this.masterPrefijoLlamadas = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterDocumentoLlamadas = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroLlamadas = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.tpNotas = new DevExpress.XtraTab.XtraTabPage();
            this.gcDocNotas = new DevExpress.XtraGrid.GridControl();
            this.gvDocNotas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpHistoria = new DevExpress.XtraTab.XtraTabPage();
            this.pnHist = new DevExpress.XtraEditors.PanelControl();
            this.lblCorreoHist = new DevExpress.XtraEditors.LabelControl();
            this.txtCorreoHist = new System.Windows.Forms.TextBox();
            this.lblDireccionHist = new DevExpress.XtraEditors.LabelControl();
            this.txtDireccionHist = new System.Windows.Forms.TextBox();
            this.lblTelefonoHist = new DevExpress.XtraEditors.LabelControl();
            this.txtTelefonoHist = new System.Windows.Forms.TextBox();
            this.groupHist = new DevExpress.XtraEditors.GroupControl();
            this.lblActivFlujoHist = new DevExpress.XtraEditors.LabelControl();
            this.masterActividadFlujoHist = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaFinHist = new System.Windows.Forms.Label();
            this.dtFechaFinHist = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaIniHist = new System.Windows.Forms.Label();
            this.dtFechaIniHist = new DevExpress.XtraEditors.DateEdit();
            this.lblDonNroHist = new DevExpress.XtraEditors.LabelControl();
            this.lblDocNroHist = new System.Windows.Forms.TextBox();
            this.masterPrefijoHist = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterDocumentoHist = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroHist = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gcDocHistoria = new DevExpress.XtraGrid.GridControl();
            this.gvDocHistoria = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.linkVer = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.pbProcess = new DevExpress.XtraEditors.ProgressBarControl();
            this.groupNotas = new DevExpress.XtraEditors.GroupControl();
            this.lblFechaFinNotas = new System.Windows.Forms.Label();
            this.dtFechaFinNotas = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaIniNotas = new System.Windows.Forms.Label();
            this.dtFechaIniNotas = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.tcTask)).BeginInit();
            this.tcTask.SuspendLayout();
            this.tpPendientes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).BeginInit();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinPend.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinPend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniPend.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniPend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).BeginInit();
            this.tpLlamadas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocLLamadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocLlamadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnLlamadas)).BeginInit();
            this.pnLlamadas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterLlamada)).BeginInit();
            this.gbFilterLlamada.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinLlam.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinLlam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniLlam.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniLlam.Properties)).BeginInit();
            this.tpNotas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocNotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocNotas)).BeginInit();
            this.tpHistoria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnHist)).BeginInit();
            this.pnHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupHist)).BeginInit();
            this.groupHist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinHist.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinHist.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniHist.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniHist.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocHistoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocHistoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNotas)).BeginInit();
            this.groupNotas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinNotas.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinNotas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniNotas.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniNotas.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tcTask
            // 
            this.tcTask.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcTask.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tcTask.AppearancePage.Header.Options.UseFont = true;
            this.tcTask.Location = new System.Drawing.Point(13, 6);
            this.tcTask.Name = "tcTask";
            this.tcTask.SelectedTabPage = this.tpPendientes;
            this.tcTask.Size = new System.Drawing.Size(895, 552);
            this.tcTask.TabIndex = 0;
            this.tcTask.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpPendientes,
            this.tpLlamadas,
            this.tpNotas,
            this.tpHistoria});
            this.tcTask.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tcTask_SelectedPageChanged);
            // 
            // tpPendientes
            // 
            this.tpPendientes.Controls.Add(this.gcDocPendientes);
            this.tpPendientes.Controls.Add(this.panelControl1);
            this.tpPendientes.Name = "tpPendientes";
            this.tpPendientes.Size = new System.Drawing.Size(889, 523);
            this.tpPendientes.Text = "1041_tpPendientes";
            // 
            // gcDocPendientes
            // 
            this.gcDocPendientes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocPendientes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcDocPendientes.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocPendientes.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocPendientes.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocPendientes.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocPendientes.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocPendientes.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocPendientes.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocPendientes.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocPendientes.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocPendientes.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocPendientes.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocPendientes.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDocPendientes.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocPendientes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode3.RelationName = "Detalle";
            this.gcDocPendientes.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode3});
            this.gcDocPendientes.Location = new System.Drawing.Point(13, 169);
            this.gcDocPendientes.LookAndFeel.SkinName = "Dark Side";
            this.gcDocPendientes.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocPendientes.MainView = this.gvDocPendientes;
            this.gcDocPendientes.Margin = new System.Windows.Forms.Padding(4, 4, 100, 4);
            this.gcDocPendientes.Name = "gcDocPendientes";
            this.gcDocPendientes.Size = new System.Drawing.Size(826, 285);
            this.gcDocPendientes.TabIndex = 54;
            this.gcDocPendientes.UseEmbeddedNavigator = true;
            this.gcDocPendientes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocPendientes,
            this.gvDetalle});
            // 
            // gvDocPendientes
            // 
            this.gvDocPendientes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocPendientes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocPendientes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocPendientes.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocPendientes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocPendientes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocPendientes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocPendientes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocPendientes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocPendientes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocPendientes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocPendientes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocPendientes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocPendientes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocPendientes.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocPendientes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocPendientes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocPendientes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocPendientes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocPendientes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocPendientes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocPendientes.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocPendientes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocPendientes.Appearance.Row.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.Row.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocPendientes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocPendientes.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocPendientes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocPendientes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocPendientes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocPendientes.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocPendientes.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocPendientes.GridControl = this.gcDocPendientes;
            this.gvDocPendientes.HorzScrollStep = 50;
            this.gvDocPendientes.Name = "gvDocPendientes";
            this.gvDocPendientes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocPendientes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocPendientes.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocPendientes.OptionsCustomization.AllowFilter = false;
            this.gvDocPendientes.OptionsCustomization.AllowSort = false;
            this.gvDocPendientes.OptionsMenu.EnableColumnMenu = false;
            this.gvDocPendientes.OptionsMenu.EnableFooterMenu = false;
            this.gvDocPendientes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocPendientes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocPendientes.OptionsView.ShowGroupPanel = false;
            this.gvDocPendientes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocPendientes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            this.gvDocPendientes.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gv_CustomColumnDisplayText);
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
            this.gvDetalle.GridControl = this.gcDocPendientes;
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
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.gbFilter);
            this.panelControl1.Controls.Add(this.cmbOrden);
            this.panelControl1.Controls.Add(this.lblOrden);
            this.panelControl1.Location = new System.Drawing.Point(14, 14);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(825, 144);
            this.panelControl1.TabIndex = 1;
            // 
            // gbFilter
            // 
            this.gbFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilter.Appearance.Options.UseFont = true;
            this.gbFilter.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilter.AppearanceCaption.Options.UseFont = true;
            this.gbFilter.Controls.Add(this.lblActividadFluj);
            this.gbFilter.Controls.Add(this.cmbEstado);
            this.gbFilter.Controls.Add(this.lblEstado);
            this.gbFilter.Controls.Add(this.lblFechaFinal);
            this.gbFilter.Controls.Add(this.dtFechaFinPend);
            this.gbFilter.Controls.Add(this.lblFechaInicial);
            this.gbFilter.Controls.Add(this.dtFechaIniPend);
            this.gbFilter.Controls.Add(this.masterTercero);
            this.gbFilter.Controls.Add(this.masterActividadFlujoPend);
            this.gbFilter.Location = new System.Drawing.Point(13, 34);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(784, 98);
            this.gbFilter.TabIndex = 102;
            this.gbFilter.Text = "1041_gbFilter";
            // 
            // lblActividadFluj
            // 
            this.lblActividadFluj.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblActividadFluj.Location = new System.Drawing.Point(12, 38);
            this.lblActividadFluj.Name = "lblActividadFluj";
            this.lblActividadFluj.Size = new System.Drawing.Size(121, 14);
            this.lblActividadFluj.TabIndex = 105;
            this.lblActividadFluj.Text = "1041_ActividadFlujoID";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(616, 42);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstado.Properties.DisplayMember = "Value";
            this.cmbEstado.Properties.ValueMember = "Key";
            this.cmbEstado.Size = new System.Drawing.Size(108, 20);
            this.cmbEstado.TabIndex = 104;
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblEstado.Location = new System.Drawing.Point(530, 45);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(83, 14);
            this.lblEstado.TabIndex = 103;
            this.lblEstado.Text = "1041_lblEstado";
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinal.Location = new System.Drawing.Point(333, 65);
            this.lblFechaFinal.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinal.TabIndex = 100;
            this.lblFechaFinal.Text = "1041_lblFechaFin";
            // 
            // dtFechaFinPend
            // 
            this.dtFechaFinPend.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinPend.Location = new System.Drawing.Point(408, 62);
            this.dtFechaFinPend.Name = "dtFechaFinPend";
            this.dtFechaFinPend.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinPend.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinPend.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinPend.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinPend.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinPend.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinPend.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinPend.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinPend.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinPend.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinPend.Size = new System.Drawing.Size(78, 20);
            this.dtFechaFinPend.TabIndex = 101;
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicial.Location = new System.Drawing.Point(333, 43);
            this.lblFechaInicial.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(98, 14);
            this.lblFechaInicial.TabIndex = 98;
            this.lblFechaInicial.Text = "1041_lblFechaIni";
            // 
            // dtFechaIniPend
            // 
            this.dtFechaIniPend.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaIniPend.Location = new System.Drawing.Point(408, 39);
            this.dtFechaIniPend.Name = "dtFechaIniPend";
            this.dtFechaIniPend.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIniPend.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIniPend.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIniPend.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniPend.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniPend.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniPend.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniPend.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIniPend.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIniPend.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIniPend.Size = new System.Drawing.Size(78, 20);
            this.dtFechaIniPend.TabIndex = 99;
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(11, 59);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(302, 25);
            this.masterTercero.TabIndex = 102;
            this.masterTercero.Value = "";
            // 
            // masterActividadFlujoPend
            // 
            this.masterActividadFlujoPend.BackColor = System.Drawing.Color.Transparent;
            this.masterActividadFlujoPend.Filtros = null;
            this.masterActividadFlujoPend.Location = new System.Drawing.Point(12, 32);
            this.masterActividadFlujoPend.Name = "masterActividadFlujoPend";
            this.masterActividadFlujoPend.Size = new System.Drawing.Size(301, 25);
            this.masterActividadFlujoPend.TabIndex = 91;
            this.masterActividadFlujoPend.Value = "";
            // 
            // cmbOrden
            // 
            this.cmbOrden.Location = new System.Drawing.Point(108, 9);
            this.cmbOrden.Name = "cmbOrden";
            this.cmbOrden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOrden.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbOrden.Properties.DisplayMember = "Value";
            this.cmbOrden.Properties.ValueMember = "Key";
            this.cmbOrden.Size = new System.Drawing.Size(108, 20);
            this.cmbOrden.TabIndex = 101;
            this.cmbOrden.EditValueChanged += new System.EventHandler(this.cmbOrden_EditValueChanged);
            // 
            // lblOrden
            // 
            this.lblOrden.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblOrden.Location = new System.Drawing.Point(25, 12);
            this.lblOrden.Name = "lblOrden";
            this.lblOrden.Size = new System.Drawing.Size(80, 14);
            this.lblOrden.TabIndex = 99;
            this.lblOrden.Text = "1041_lblOrden";
            // 
            // tpLlamadas
            // 
            this.tpLlamadas.Controls.Add(this.gcDocLLamadas);
            this.tpLlamadas.Controls.Add(this.pnLlamadas);
            this.tpLlamadas.Name = "tpLlamadas";
            this.tpLlamadas.Size = new System.Drawing.Size(889, 523);
            this.tpLlamadas.Text = "1041_tpLlamadas";
            // 
            // gcDocLLamadas
            // 
            this.gcDocLLamadas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocLLamadas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocLLamadas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocLLamadas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocLLamadas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocLLamadas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocLLamadas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocLLamadas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocLLamadas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocLLamadas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocLLamadas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocLLamadas.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocLLamadas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocLLamadas.Location = new System.Drawing.Point(14, 170);
            this.gcDocLLamadas.LookAndFeel.SkinName = "Dark Side";
            this.gcDocLLamadas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocLLamadas.MainView = this.gvDocLlamadas;
            this.gcDocLLamadas.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocLLamadas.Name = "gcDocLLamadas";
            this.gcDocLLamadas.Size = new System.Drawing.Size(826, 350);
            this.gcDocLLamadas.TabIndex = 103;
            this.gcDocLLamadas.UseEmbeddedNavigator = true;
            this.gcDocLLamadas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocLlamadas});
            // 
            // gvDocLlamadas
            // 
            this.gvDocLlamadas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocLlamadas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocLlamadas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocLlamadas.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocLlamadas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocLlamadas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocLlamadas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocLlamadas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocLlamadas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocLlamadas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocLlamadas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocLlamadas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocLlamadas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocLlamadas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocLlamadas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocLlamadas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocLlamadas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocLlamadas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocLlamadas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocLlamadas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocLlamadas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocLlamadas.Appearance.Row.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.Row.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocLlamadas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocLlamadas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocLlamadas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocLlamadas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocLlamadas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocLlamadas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocLlamadas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocLlamadas.GridControl = this.gcDocLLamadas;
            this.gvDocLlamadas.Name = "gvDocLlamadas";
            this.gvDocLlamadas.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocLlamadas.OptionsCustomization.AllowFilter = false;
            this.gvDocLlamadas.OptionsCustomization.AllowSort = false;
            this.gvDocLlamadas.OptionsMenu.EnableColumnMenu = false;
            this.gvDocLlamadas.OptionsMenu.EnableFooterMenu = false;
            this.gvDocLlamadas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocLlamadas.OptionsView.ColumnAutoWidth = false;
            this.gvDocLlamadas.OptionsView.ShowGroupPanel = false;
            this.gvDocLlamadas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocLlamadas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            // 
            // pnLlamadas
            // 
            this.pnLlamadas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnLlamadas.Controls.Add(this.lblCorreoLlam);
            this.pnLlamadas.Controls.Add(this.txtCorreoLlam);
            this.pnLlamadas.Controls.Add(this.lblDireccionLlam);
            this.pnLlamadas.Controls.Add(this.txtDireccionLlam);
            this.pnLlamadas.Controls.Add(this.lblTelefonoLlam);
            this.pnLlamadas.Controls.Add(this.txtTelefonoLlam);
            this.pnLlamadas.Controls.Add(this.gbFilterLlamada);
            this.pnLlamadas.Location = new System.Drawing.Point(12, 15);
            this.pnLlamadas.Name = "pnLlamadas";
            this.pnLlamadas.Size = new System.Drawing.Size(825, 138);
            this.pnLlamadas.TabIndex = 102;
            // 
            // lblCorreoLlam
            // 
            this.lblCorreoLlam.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreoLlam.Location = new System.Drawing.Point(494, 115);
            this.lblCorreoLlam.Name = "lblCorreoLlam";
            this.lblCorreoLlam.Size = new System.Drawing.Size(82, 14);
            this.lblCorreoLlam.TabIndex = 108;
            this.lblCorreoLlam.Text = "1041_lblCorreo";
            // 
            // txtCorreoLlam
            // 
            this.txtCorreoLlam.Location = new System.Drawing.Point(560, 112);
            this.txtCorreoLlam.Name = "txtCorreoLlam";
            this.txtCorreoLlam.ReadOnly = true;
            this.txtCorreoLlam.Size = new System.Drawing.Size(149, 20);
            this.txtCorreoLlam.TabIndex = 107;
            // 
            // lblDireccionLlam
            // 
            this.lblDireccionLlam.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccionLlam.Location = new System.Drawing.Point(231, 115);
            this.lblDireccionLlam.Name = "lblDireccionLlam";
            this.lblDireccionLlam.Size = new System.Drawing.Size(95, 14);
            this.lblDireccionLlam.TabIndex = 106;
            this.lblDireccionLlam.Text = "1041_lblDireccion";
            // 
            // txtDireccionLlam
            // 
            this.txtDireccionLlam.Location = new System.Drawing.Point(307, 112);
            this.txtDireccionLlam.Name = "txtDireccionLlam";
            this.txtDireccionLlam.ReadOnly = true;
            this.txtDireccionLlam.Size = new System.Drawing.Size(132, 20);
            this.txtDireccionLlam.TabIndex = 105;
            // 
            // lblTelefonoLlam
            // 
            this.lblTelefonoLlam.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefonoLlam.Location = new System.Drawing.Point(15, 115);
            this.lblTelefonoLlam.Name = "lblTelefonoLlam";
            this.lblTelefonoLlam.Size = new System.Drawing.Size(95, 14);
            this.lblTelefonoLlam.TabIndex = 104;
            this.lblTelefonoLlam.Text = "1041_lblTelefono";
            // 
            // txtTelefonoLlam
            // 
            this.txtTelefonoLlam.Location = new System.Drawing.Point(82, 112);
            this.txtTelefonoLlam.Name = "txtTelefonoLlam";
            this.txtTelefonoLlam.ReadOnly = true;
            this.txtTelefonoLlam.Size = new System.Drawing.Size(106, 20);
            this.txtTelefonoLlam.TabIndex = 103;
            // 
            // gbFilterLlamada
            // 
            this.gbFilterLlamada.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterLlamada.Appearance.Options.UseFont = true;
            this.gbFilterLlamada.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterLlamada.AppearanceCaption.Options.UseFont = true;
            this.gbFilterLlamada.Controls.Add(this.lblActivFlujoLlam);
            this.gbFilterLlamada.Controls.Add(this.masterActivFlujoLlam);
            this.gbFilterLlamada.Controls.Add(this.lblFechaFinLlam);
            this.gbFilterLlamada.Controls.Add(this.lblDocNroLlam);
            this.gbFilterLlamada.Controls.Add(this.dtFechaFinLlam);
            this.gbFilterLlamada.Controls.Add(this.txtDocNroLlam);
            this.gbFilterLlamada.Controls.Add(this.lblFechaIniLlam);
            this.gbFilterLlamada.Controls.Add(this.dtFechaIniLlam);
            this.gbFilterLlamada.Controls.Add(this.masterPrefijoLlamadas);
            this.gbFilterLlamada.Controls.Add(this.masterDocumentoLlamadas);
            this.gbFilterLlamada.Controls.Add(this.masterTerceroLlamadas);
            this.gbFilterLlamada.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilterLlamada.Location = new System.Drawing.Point(2, 2);
            this.gbFilterLlamada.Name = "gbFilterLlamada";
            this.gbFilterLlamada.Size = new System.Drawing.Size(821, 105);
            this.gbFilterLlamada.TabIndex = 102;
            this.gbFilterLlamada.Text = "1041_gbFilter";
            // 
            // lblActivFlujoLlam
            // 
            this.lblActivFlujoLlam.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblActivFlujoLlam.Location = new System.Drawing.Point(10, 83);
            this.lblActivFlujoLlam.Name = "lblActivFlujoLlam";
            this.lblActivFlujoLlam.Size = new System.Drawing.Size(121, 14);
            this.lblActivFlujoLlam.TabIndex = 115;
            this.lblActivFlujoLlam.Text = "1041_ActividadFlujoID";
            // 
            // masterActivFlujoLlam
            // 
            this.masterActivFlujoLlam.BackColor = System.Drawing.Color.Transparent;
            this.masterActivFlujoLlam.Filtros = null;
            this.masterActivFlujoLlam.Location = new System.Drawing.Point(10, 78);
            this.masterActivFlujoLlam.Name = "masterActivFlujoLlam";
            this.masterActivFlujoLlam.Size = new System.Drawing.Size(301, 25);
            this.masterActivFlujoLlam.TabIndex = 114;
            this.masterActivFlujoLlam.Value = "";
            // 
            // lblFechaFinLlam
            // 
            this.lblFechaFinLlam.AutoSize = true;
            this.lblFechaFinLlam.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinLlam.Location = new System.Drawing.Point(522, 83);
            this.lblFechaFinLlam.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinLlam.Name = "lblFechaFinLlam";
            this.lblFechaFinLlam.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinLlam.TabIndex = 106;
            this.lblFechaFinLlam.Text = "1041_lblFechaFin";
            // 
            // lblDocNroLlam
            // 
            this.lblDocNroLlam.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocNroLlam.Location = new System.Drawing.Point(618, 58);
            this.lblDocNroLlam.Name = "lblDocNroLlam";
            this.lblDocNroLlam.Size = new System.Drawing.Size(86, 14);
            this.lblDocNroLlam.TabIndex = 101;
            this.lblDocNroLlam.Text = "1041_lblDocNro";
            // 
            // dtFechaFinLlam
            // 
            this.dtFechaFinLlam.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinLlam.Location = new System.Drawing.Point(630, 80);
            this.dtFechaFinLlam.Name = "dtFechaFinLlam";
            this.dtFechaFinLlam.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinLlam.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinLlam.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinLlam.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinLlam.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinLlam.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinLlam.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinLlam.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinLlam.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinLlam.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinLlam.Size = new System.Drawing.Size(92, 20);
            this.dtFechaFinLlam.TabIndex = 107;
            // 
            // txtDocNroLlam
            // 
            this.txtDocNroLlam.Location = new System.Drawing.Point(685, 55);
            this.txtDocNroLlam.Name = "txtDocNroLlam";
            this.txtDocNroLlam.Size = new System.Drawing.Size(37, 20);
            this.txtDocNroLlam.TabIndex = 100;
            // 
            // lblFechaIniLlam
            // 
            this.lblFechaIniLlam.AutoSize = true;
            this.lblFechaIniLlam.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaIniLlam.Location = new System.Drawing.Point(318, 83);
            this.lblFechaIniLlam.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaIniLlam.Name = "lblFechaIniLlam";
            this.lblFechaIniLlam.Size = new System.Drawing.Size(98, 14);
            this.lblFechaIniLlam.TabIndex = 104;
            this.lblFechaIniLlam.Text = "1041_lblFechaIni";
            // 
            // dtFechaIniLlam
            // 
            this.dtFechaIniLlam.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaIniLlam.Location = new System.Drawing.Point(420, 80);
            this.dtFechaIniLlam.Name = "dtFechaIniLlam";
            this.dtFechaIniLlam.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIniLlam.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIniLlam.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIniLlam.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniLlam.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniLlam.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniLlam.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniLlam.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIniLlam.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIniLlam.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIniLlam.Size = new System.Drawing.Size(97, 20);
            this.dtFechaIniLlam.TabIndex = 105;
            // 
            // masterPrefijoLlamadas
            // 
            this.masterPrefijoLlamadas.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoLlamadas.Filtros = null;
            this.masterPrefijoLlamadas.Location = new System.Drawing.Point(320, 53);
            this.masterPrefijoLlamadas.Name = "masterPrefijoLlamadas";
            this.masterPrefijoLlamadas.Size = new System.Drawing.Size(302, 25);
            this.masterPrefijoLlamadas.TabIndex = 103;
            this.masterPrefijoLlamadas.Value = "";
            // 
            // masterDocumentoLlamadas
            // 
            this.masterDocumentoLlamadas.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumentoLlamadas.Filtros = null;
            this.masterDocumentoLlamadas.Location = new System.Drawing.Point(11, 53);
            this.masterDocumentoLlamadas.Name = "masterDocumentoLlamadas";
            this.masterDocumentoLlamadas.Size = new System.Drawing.Size(302, 25);
            this.masterDocumentoLlamadas.TabIndex = 102;
            this.masterDocumentoLlamadas.Value = "";
            // 
            // masterTerceroLlamadas
            // 
            this.masterTerceroLlamadas.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroLlamadas.Filtros = null;
            this.masterTerceroLlamadas.Location = new System.Drawing.Point(12, 27);
            this.masterTerceroLlamadas.Name = "masterTerceroLlamadas";
            this.masterTerceroLlamadas.Size = new System.Drawing.Size(301, 25);
            this.masterTerceroLlamadas.TabIndex = 91;
            this.masterTerceroLlamadas.Value = "";
            this.masterTerceroLlamadas.Leave += new System.EventHandler(this.masterTercero_Leave);
            // 
            // tpNotas
            // 
            this.tpNotas.Controls.Add(this.groupNotas);
            this.tpNotas.Controls.Add(this.gcDocNotas);
            this.tpNotas.Name = "tpNotas";
            this.tpNotas.Size = new System.Drawing.Size(889, 523);
            this.tpNotas.Text = "1041_tpNotas";
            // 
            // gcDocNotas
            // 
            this.gcDocNotas.AllowDrop = true;
            this.gcDocNotas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcDocNotas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocNotas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocNotas.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcDocNotas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocNotas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocNotas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocNotas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocNotas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocNotas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocNotas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocNotas.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gcDocNotas.EmbeddedNavigator.Buttons.PrevPage.Enabled = false;
            this.gcDocNotas.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcDocNotas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcDocNotas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocNotas.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcDocNotas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocNotas.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gvDocNotas_EmbeddedNavigator_ButtonClick);
            this.gcDocNotas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDocNotas.Location = new System.Drawing.Point(11, 99);
            this.gcDocNotas.LookAndFeel.SkinName = "Dark Side";
            this.gcDocNotas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocNotas.MainView = this.gvDocNotas;
            this.gcDocNotas.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocNotas.Name = "gcDocNotas";
            this.gcDocNotas.Size = new System.Drawing.Size(832, 341);
            this.gcDocNotas.TabIndex = 2;
            this.gcDocNotas.UseEmbeddedNavigator = true;
            this.gcDocNotas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocNotas});
            // 
            // gvDocNotas
            // 
            this.gvDocNotas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocNotas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocNotas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocNotas.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvDocNotas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocNotas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocNotas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvDocNotas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocNotas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocNotas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvDocNotas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvDocNotas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocNotas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocNotas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocNotas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocNotas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocNotas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocNotas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocNotas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocNotas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvDocNotas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocNotas.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocNotas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocNotas.Appearance.Row.Options.UseBackColor = true;
            this.gvDocNotas.Appearance.Row.Options.UseFont = true;
            this.gvDocNotas.Appearance.Row.Options.UseForeColor = true;
            this.gvDocNotas.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocNotas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocNotas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvDocNotas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocNotas.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvDocNotas.GridControl = this.gcDocNotas;
            this.gvDocNotas.HorzScrollStep = 50;
            this.gvDocNotas.Name = "gvDocNotas";
            this.gvDocNotas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocNotas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocNotas.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocNotas.OptionsCustomization.AllowFilter = false;
            this.gvDocNotas.OptionsCustomization.AllowSort = false;
            this.gvDocNotas.OptionsFind.AllowFindPanel = false;
            this.gvDocNotas.OptionsMenu.EnableColumnMenu = false;
            this.gvDocNotas.OptionsMenu.EnableFooterMenu = false;
            this.gvDocNotas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocNotas.OptionsView.ColumnAutoWidth = false;
            this.gvDocNotas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocNotas.OptionsView.ShowGroupPanel = false;
            this.gvDocNotas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocNotas.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocNotas_BeforeLeaveRow);
            this.gvDocNotas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            // 
            // tpHistoria
            // 
            this.tpHistoria.Controls.Add(this.pnHist);
            this.tpHistoria.Controls.Add(this.gcDocHistoria);
            this.tpHistoria.Name = "tpHistoria";
            this.tpHistoria.Size = new System.Drawing.Size(889, 523);
            this.tpHistoria.Text = "1041_tpHistoria";
            // 
            // pnHist
            // 
            this.pnHist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnHist.Controls.Add(this.lblCorreoHist);
            this.pnHist.Controls.Add(this.txtCorreoHist);
            this.pnHist.Controls.Add(this.lblDireccionHist);
            this.pnHist.Controls.Add(this.txtDireccionHist);
            this.pnHist.Controls.Add(this.lblTelefonoHist);
            this.pnHist.Controls.Add(this.txtTelefonoHist);
            this.pnHist.Controls.Add(this.groupHist);
            this.pnHist.Location = new System.Drawing.Point(13, 15);
            this.pnHist.Name = "pnHist";
            this.pnHist.Size = new System.Drawing.Size(825, 138);
            this.pnHist.TabIndex = 103;
            // 
            // lblCorreoHist
            // 
            this.lblCorreoHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreoHist.Location = new System.Drawing.Point(494, 116);
            this.lblCorreoHist.Name = "lblCorreoHist";
            this.lblCorreoHist.Size = new System.Drawing.Size(82, 14);
            this.lblCorreoHist.TabIndex = 108;
            this.lblCorreoHist.Text = "1041_lblCorreo";
            // 
            // txtCorreoHist
            // 
            this.txtCorreoHist.Location = new System.Drawing.Point(560, 113);
            this.txtCorreoHist.Name = "txtCorreoHist";
            this.txtCorreoHist.ReadOnly = true;
            this.txtCorreoHist.Size = new System.Drawing.Size(149, 20);
            this.txtCorreoHist.TabIndex = 107;
            // 
            // lblDireccionHist
            // 
            this.lblDireccionHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccionHist.Location = new System.Drawing.Point(231, 116);
            this.lblDireccionHist.Name = "lblDireccionHist";
            this.lblDireccionHist.Size = new System.Drawing.Size(95, 14);
            this.lblDireccionHist.TabIndex = 106;
            this.lblDireccionHist.Text = "1041_lblDireccion";
            // 
            // txtDireccionHist
            // 
            this.txtDireccionHist.Location = new System.Drawing.Point(307, 113);
            this.txtDireccionHist.Name = "txtDireccionHist";
            this.txtDireccionHist.ReadOnly = true;
            this.txtDireccionHist.Size = new System.Drawing.Size(132, 20);
            this.txtDireccionHist.TabIndex = 105;
            // 
            // lblTelefonoHist
            // 
            this.lblTelefonoHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefonoHist.Location = new System.Drawing.Point(15, 116);
            this.lblTelefonoHist.Name = "lblTelefonoHist";
            this.lblTelefonoHist.Size = new System.Drawing.Size(95, 14);
            this.lblTelefonoHist.TabIndex = 104;
            this.lblTelefonoHist.Text = "1041_lblTelefono";
            // 
            // txtTelefonoHist
            // 
            this.txtTelefonoHist.Location = new System.Drawing.Point(82, 113);
            this.txtTelefonoHist.Name = "txtTelefonoHist";
            this.txtTelefonoHist.ReadOnly = true;
            this.txtTelefonoHist.Size = new System.Drawing.Size(106, 20);
            this.txtTelefonoHist.TabIndex = 103;
            // 
            // groupHist
            // 
            this.groupHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupHist.Appearance.Options.UseFont = true;
            this.groupHist.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupHist.AppearanceCaption.Options.UseFont = true;
            this.groupHist.Controls.Add(this.lblActivFlujoHist);
            this.groupHist.Controls.Add(this.masterActividadFlujoHist);
            this.groupHist.Controls.Add(this.lblFechaFinHist);
            this.groupHist.Controls.Add(this.dtFechaFinHist);
            this.groupHist.Controls.Add(this.lblFechaIniHist);
            this.groupHist.Controls.Add(this.dtFechaIniHist);
            this.groupHist.Controls.Add(this.lblDonNroHist);
            this.groupHist.Controls.Add(this.lblDocNroHist);
            this.groupHist.Controls.Add(this.masterPrefijoHist);
            this.groupHist.Controls.Add(this.masterDocumentoHist);
            this.groupHist.Controls.Add(this.masterTerceroHist);
            this.groupHist.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupHist.Location = new System.Drawing.Point(2, 2);
            this.groupHist.Name = "groupHist";
            this.groupHist.Size = new System.Drawing.Size(821, 104);
            this.groupHist.TabIndex = 102;
            this.groupHist.Text = "1041_gbFilter";
            // 
            // lblActivFlujoHist
            // 
            this.lblActivFlujoHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblActivFlujoHist.Location = new System.Drawing.Point(11, 82);
            this.lblActivFlujoHist.Name = "lblActivFlujoHist";
            this.lblActivFlujoHist.Size = new System.Drawing.Size(121, 14);
            this.lblActivFlujoHist.TabIndex = 113;
            this.lblActivFlujoHist.Text = "1041_ActividadFlujoID";
            // 
            // masterActividadFlujoHist
            // 
            this.masterActividadFlujoHist.BackColor = System.Drawing.Color.Transparent;
            this.masterActividadFlujoHist.Filtros = null;
            this.masterActividadFlujoHist.Location = new System.Drawing.Point(11, 77);
            this.masterActividadFlujoHist.Name = "masterActividadFlujoHist";
            this.masterActividadFlujoHist.Size = new System.Drawing.Size(301, 25);
            this.masterActividadFlujoHist.TabIndex = 112;
            this.masterActividadFlujoHist.Value = "";
            // 
            // lblFechaFinHist
            // 
            this.lblFechaFinHist.AutoSize = true;
            this.lblFechaFinHist.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinHist.Location = new System.Drawing.Point(521, 82);
            this.lblFechaFinHist.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinHist.Name = "lblFechaFinHist";
            this.lblFechaFinHist.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinHist.TabIndex = 110;
            this.lblFechaFinHist.Text = "1041_lblFechaFin";
            // 
            // dtFechaFinHist
            // 
            this.dtFechaFinHist.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinHist.Location = new System.Drawing.Point(629, 78);
            this.dtFechaFinHist.Name = "dtFechaFinHist";
            this.dtFechaFinHist.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinHist.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinHist.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinHist.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinHist.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinHist.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinHist.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinHist.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinHist.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinHist.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinHist.Size = new System.Drawing.Size(90, 20);
            this.dtFechaFinHist.TabIndex = 111;
            // 
            // lblFechaIniHist
            // 
            this.lblFechaIniHist.AutoSize = true;
            this.lblFechaIniHist.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaIniHist.Location = new System.Drawing.Point(318, 82);
            this.lblFechaIniHist.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaIniHist.Name = "lblFechaIniHist";
            this.lblFechaIniHist.Size = new System.Drawing.Size(98, 14);
            this.lblFechaIniHist.TabIndex = 108;
            this.lblFechaIniHist.Text = "1041_lblFechaIni";
            // 
            // dtFechaIniHist
            // 
            this.dtFechaIniHist.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaIniHist.Location = new System.Drawing.Point(420, 79);
            this.dtFechaIniHist.Name = "dtFechaIniHist";
            this.dtFechaIniHist.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIniHist.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIniHist.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIniHist.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniHist.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniHist.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniHist.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniHist.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIniHist.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIniHist.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIniHist.Size = new System.Drawing.Size(98, 20);
            this.dtFechaIniHist.TabIndex = 109;
            // 
            // lblDonNroHist
            // 
            this.lblDonNroHist.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDonNroHist.Location = new System.Drawing.Point(621, 58);
            this.lblDonNroHist.Name = "lblDonNroHist";
            this.lblDonNroHist.Size = new System.Drawing.Size(86, 14);
            this.lblDonNroHist.TabIndex = 101;
            this.lblDonNroHist.Text = "1041_lblDocNro";
            // 
            // lblDocNroHist
            // 
            this.lblDocNroHist.Location = new System.Drawing.Point(678, 55);
            this.lblDocNroHist.Name = "lblDocNroHist";
            this.lblDocNroHist.Size = new System.Drawing.Size(41, 20);
            this.lblDocNroHist.TabIndex = 100;
            // 
            // masterPrefijoHist
            // 
            this.masterPrefijoHist.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoHist.Filtros = null;
            this.masterPrefijoHist.Location = new System.Drawing.Point(321, 53);
            this.masterPrefijoHist.Name = "masterPrefijoHist";
            this.masterPrefijoHist.Size = new System.Drawing.Size(302, 25);
            this.masterPrefijoHist.TabIndex = 103;
            this.masterPrefijoHist.Value = "";
            // 
            // masterDocumentoHist
            // 
            this.masterDocumentoHist.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumentoHist.Filtros = null;
            this.masterDocumentoHist.Location = new System.Drawing.Point(11, 52);
            this.masterDocumentoHist.Name = "masterDocumentoHist";
            this.masterDocumentoHist.Size = new System.Drawing.Size(302, 25);
            this.masterDocumentoHist.TabIndex = 102;
            this.masterDocumentoHist.Value = "";
            // 
            // masterTerceroHist
            // 
            this.masterTerceroHist.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroHist.Filtros = null;
            this.masterTerceroHist.Location = new System.Drawing.Point(12, 27);
            this.masterTerceroHist.Name = "masterTerceroHist";
            this.masterTerceroHist.Size = new System.Drawing.Size(301, 25);
            this.masterTerceroHist.TabIndex = 91;
            this.masterTerceroHist.Value = "";
            this.masterTerceroHist.Leave += new System.EventHandler(this.masterTercero_Leave);
            // 
            // gcDocHistoria
            // 
            this.gcDocHistoria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocHistoria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcDocHistoria.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocHistoria.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocHistoria.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocHistoria.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocHistoria.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocHistoria.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocHistoria.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocHistoria.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocHistoria.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocHistoria.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocHistoria.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocHistoria.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDocHistoria.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocHistoria.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocHistoria.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocHistoria.Location = new System.Drawing.Point(15, 169);
            this.gcDocHistoria.LookAndFeel.SkinName = "Dark Side";
            this.gcDocHistoria.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocHistoria.MainView = this.gvDocHistoria;
            this.gcDocHistoria.Margin = new System.Windows.Forms.Padding(4, 4, 100, 4);
            this.gcDocHistoria.Name = "gcDocHistoria";
            this.gcDocHistoria.Size = new System.Drawing.Size(826, 285);
            this.gcDocHistoria.TabIndex = 55;
            this.gcDocHistoria.UseEmbeddedNavigator = true;
            this.gcDocHistoria.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocHistoria,
            this.gridView3});
            // 
            // gvDocHistoria
            // 
            this.gvDocHistoria.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocHistoria.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocHistoria.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocHistoria.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocHistoria.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocHistoria.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocHistoria.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocHistoria.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocHistoria.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocHistoria.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocHistoria.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocHistoria.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocHistoria.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocHistoria.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocHistoria.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocHistoria.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocHistoria.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocHistoria.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocHistoria.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocHistoria.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocHistoria.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocHistoria.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocHistoria.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocHistoria.Appearance.Row.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.Row.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocHistoria.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocHistoria.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocHistoria.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocHistoria.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocHistoria.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocHistoria.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocHistoria.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocHistoria.GridControl = this.gcDocHistoria;
            this.gvDocHistoria.HorzScrollStep = 50;
            this.gvDocHistoria.Name = "gvDocHistoria";
            this.gvDocHistoria.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocHistoria.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocHistoria.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocHistoria.OptionsCustomization.AllowFilter = false;
            this.gvDocHistoria.OptionsCustomization.AllowSort = false;
            this.gvDocHistoria.OptionsMenu.EnableColumnMenu = false;
            this.gvDocHistoria.OptionsMenu.EnableFooterMenu = false;
            this.gvDocHistoria.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocHistoria.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocHistoria.OptionsView.ShowGroupPanel = false;
            this.gvDocHistoria.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocHistoria.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            this.gvDocHistoria.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gv_CustomColumnDisplayText);
            // 
            // gridView3
            // 
            this.gridView3.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView3.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridView3.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gridView3.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView3.Appearance.Empty.Options.UseBackColor = true;
            this.gridView3.Appearance.Empty.Options.UseFont = true;
            this.gridView3.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gridView3.Appearance.FixedLine.Options.UseBackColor = true;
            this.gridView3.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView3.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gridView3.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView3.Appearance.FocusedCell.Options.UseFont = true;
            this.gridView3.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridView3.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView3.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gridView3.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gridView3.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView3.Appearance.FocusedRow.Options.UseFont = true;
            this.gridView3.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridView3.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gridView3.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridView3.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gridView3.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gridView3.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView3.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gridView3.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridView3.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridView3.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridView3.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView3.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView3.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridView3.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView3.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gridView3.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridView3.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gridView3.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridView3.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridView3.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gridView3.Appearance.Row.Options.UseBackColor = true;
            this.gridView3.Appearance.Row.Options.UseFont = true;
            this.gridView3.Appearance.Row.Options.UseForeColor = true;
            this.gridView3.Appearance.Row.Options.UseTextOptions = true;
            this.gridView3.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridView3.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView3.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView3.Appearance.SelectedRow.Options.UseFont = true;
            this.gridView3.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gridView3.Appearance.TopNewRow.Options.UseFont = true;
            this.gridView3.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gridView3.GridControl = this.gcDocHistoria;
            this.gridView3.HorzScrollStep = 50;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gridView3.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsCustomization.AllowColumnMoving = false;
            this.gridView3.OptionsCustomization.AllowFilter = false;
            this.gridView3.OptionsCustomization.AllowSort = false;
            this.gridView3.OptionsMenu.EnableColumnMenu = false;
            this.gridView3.OptionsMenu.EnableFooterMenu = false;
            this.gridView3.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gridView3.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editValue4,
            this.editCheck,
            this.linkVer,
            this.editDate});
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
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            this.editDate.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // pbProcess
            // 
            this.pbProcess.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbProcess.Location = new System.Drawing.Point(0, 559);
            this.pbProcess.Name = "pbProcess";
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(920, 18);
            this.pbProcess.TabIndex = 1;
            // 
            // groupNotas
            // 
            this.groupNotas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupNotas.Appearance.Options.UseFont = true;
            this.groupNotas.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupNotas.AppearanceCaption.Options.UseFont = true;
            this.groupNotas.Controls.Add(this.lblFechaFinNotas);
            this.groupNotas.Controls.Add(this.dtFechaFinNotas);
            this.groupNotas.Controls.Add(this.lblFechaIniNotas);
            this.groupNotas.Controls.Add(this.dtFechaIniNotas);
            this.groupNotas.Location = new System.Drawing.Point(12, 17);
            this.groupNotas.Name = "groupNotas";
            this.groupNotas.Size = new System.Drawing.Size(831, 75);
            this.groupNotas.TabIndex = 103;
            this.groupNotas.Text = "1041_gbFilter";
            // 
            // lblFechaFinNotas
            // 
            this.lblFechaFinNotas.AutoSize = true;
            this.lblFechaFinNotas.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinNotas.Location = new System.Drawing.Point(209, 38);
            this.lblFechaFinNotas.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinNotas.Name = "lblFechaFinNotas";
            this.lblFechaFinNotas.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinNotas.TabIndex = 100;
            this.lblFechaFinNotas.Text = "1041_lblFechaFin";
            // 
            // dtFechaFinNotas
            // 
            this.dtFechaFinNotas.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinNotas.Location = new System.Drawing.Point(284, 35);
            this.dtFechaFinNotas.Name = "dtFechaFinNotas";
            this.dtFechaFinNotas.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinNotas.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinNotas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinNotas.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinNotas.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinNotas.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinNotas.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinNotas.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinNotas.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinNotas.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinNotas.Size = new System.Drawing.Size(78, 20);
            this.dtFechaFinNotas.TabIndex = 101;
            // 
            // lblFechaIniNotas
            // 
            this.lblFechaIniNotas.AutoSize = true;
            this.lblFechaIniNotas.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaIniNotas.Location = new System.Drawing.Point(19, 39);
            this.lblFechaIniNotas.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaIniNotas.Name = "lblFechaIniNotas";
            this.lblFechaIniNotas.Size = new System.Drawing.Size(98, 14);
            this.lblFechaIniNotas.TabIndex = 98;
            this.lblFechaIniNotas.Text = "1041_lblFechaIni";
            // 
            // dtFechaIniNotas
            // 
            this.dtFechaIniNotas.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaIniNotas.Location = new System.Drawing.Point(94, 35);
            this.dtFechaIniNotas.Name = "dtFechaIniNotas";
            this.dtFechaIniNotas.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIniNotas.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIniNotas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIniNotas.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniNotas.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniNotas.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIniNotas.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIniNotas.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIniNotas.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIniNotas.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIniNotas.Size = new System.Drawing.Size(78, 20);
            this.dtFechaIniNotas.TabIndex = 99;
            // 
            // TaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 577);
            this.Controls.Add(this.pbProcess);
            this.Controls.Add(this.tcTask);
            this.Name = "TaskForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskForm";
            ((System.ComponentModel.ISupportInitialize)(this.tcTask)).EndInit();
            this.tcTask.ResumeLayout(false);
            this.tpPendientes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).EndInit();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinPend.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinPend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniPend.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniPend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).EndInit();
            this.tpLlamadas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocLLamadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocLlamadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnLlamadas)).EndInit();
            this.pnLlamadas.ResumeLayout(false);
            this.pnLlamadas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterLlamada)).EndInit();
            this.gbFilterLlamada.ResumeLayout(false);
            this.gbFilterLlamada.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinLlam.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinLlam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniLlam.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniLlam.Properties)).EndInit();
            this.tpNotas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocNotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocNotas)).EndInit();
            this.tpHistoria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnHist)).EndInit();
            this.pnHist.ResumeLayout(false);
            this.pnHist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupHist)).EndInit();
            this.groupHist.ResumeLayout(false);
            this.groupHist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinHist.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinHist.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniHist.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniHist.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocHistoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocHistoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupNotas)).EndInit();
            this.groupNotas.ResumeLayout(false);
            this.groupNotas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinNotas.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinNotas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniNotas.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIniNotas.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tcTask;
        private DevExpress.XtraTab.XtraTabPage tpPendientes;
        private DevExpress.XtraTab.XtraTabPage tpLlamadas;
        private DevExpress.XtraTab.XtraTabPage tpNotas;
        private DevExpress.XtraTab.XtraTabPage tpHistoria;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private DevExpress.XtraEditors.ProgressBarControl pbProcess;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkVer;
        private DevExpress.XtraGrid.GridControl gcDocPendientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocPendientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ControlsUC.uc_MasterFind masterActividadFlujoPend;
        private DevExpress.XtraEditors.LabelControl lblOrden;
        private DevExpress.XtraEditors.LookUpEdit cmbOrden;
        private DevExpress.XtraEditors.GroupControl gbFilter;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private ControlsUC.uc_MasterFind masterTercero;
        private System.Windows.Forms.Label lblFechaFinal;
        private DevExpress.XtraEditors.DateEdit dtFechaFinPend;
        private System.Windows.Forms.Label lblFechaInicial;
        private DevExpress.XtraEditors.DateEdit dtFechaIniPend;
        private DevExpress.XtraEditors.LabelControl lblDocNroLlam;
        private System.Windows.Forms.TextBox txtDocNroLlam;
        private DevExpress.XtraGrid.GridControl gcDocHistoria;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocHistoria;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.PanelControl pnLlamadas;
        private DevExpress.XtraEditors.GroupControl gbFilterLlamada;
        private ControlsUC.uc_MasterFind masterPrefijoLlamadas;
        private ControlsUC.uc_MasterFind masterDocumentoLlamadas;
        private ControlsUC.uc_MasterFind masterTerceroLlamadas;
        private DevExpress.XtraEditors.LabelControl lblCorreoLlam;
        private System.Windows.Forms.TextBox txtCorreoLlam;
        private DevExpress.XtraEditors.LabelControl lblDireccionLlam;
        private System.Windows.Forms.TextBox txtDireccionLlam;
        private DevExpress.XtraEditors.LabelControl lblTelefonoLlam;
        private System.Windows.Forms.TextBox txtTelefonoLlam;
        private DevExpress.XtraGrid.GridControl gcDocLLamadas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocLlamadas;
        private DevExpress.XtraEditors.PanelControl pnHist;
        private DevExpress.XtraEditors.LabelControl lblCorreoHist;
        private System.Windows.Forms.TextBox txtCorreoHist;
        private DevExpress.XtraEditors.LabelControl lblDireccionHist;
        private System.Windows.Forms.TextBox txtDireccionHist;
        private DevExpress.XtraEditors.LabelControl lblTelefonoHist;
        private System.Windows.Forms.TextBox txtTelefonoHist;
        private DevExpress.XtraEditors.GroupControl groupHist;
        private DevExpress.XtraEditors.LabelControl lblDonNroHist;
        private System.Windows.Forms.TextBox lblDocNroHist;
        private ControlsUC.uc_MasterFind masterPrefijoHist;
        private ControlsUC.uc_MasterFind masterDocumentoHist;
        private ControlsUC.uc_MasterFind masterTerceroHist;
        private DevExpress.XtraEditors.LabelControl lblActividadFluj;
        private DevExpress.XtraGrid.GridControl gcDocNotas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocNotas;
        private System.Windows.Forms.Label lblFechaFinLlam;
        private DevExpress.XtraEditors.DateEdit dtFechaFinLlam;
        private System.Windows.Forms.Label lblFechaIniLlam;
        private DevExpress.XtraEditors.DateEdit dtFechaIniLlam;
        private System.Windows.Forms.Label lblFechaFinHist;
        private DevExpress.XtraEditors.DateEdit dtFechaFinHist;
        private System.Windows.Forms.Label lblFechaIniHist;
        private DevExpress.XtraEditors.DateEdit dtFechaIniHist;
        private DevExpress.XtraEditors.LabelControl lblActivFlujoHist;
        private ControlsUC.uc_MasterFind masterActividadFlujoHist;
        private DevExpress.XtraEditors.LabelControl lblActivFlujoLlam;
        private ControlsUC.uc_MasterFind masterActivFlujoLlam;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        private DevExpress.XtraEditors.GroupControl groupNotas;
        private System.Windows.Forms.Label lblFechaFinNotas;
        private DevExpress.XtraEditors.DateEdit dtFechaFinNotas;
        private System.Windows.Forms.Label lblFechaIniNotas;
        private DevExpress.XtraEditors.DateEdit dtFechaIniNotas;
    }
}