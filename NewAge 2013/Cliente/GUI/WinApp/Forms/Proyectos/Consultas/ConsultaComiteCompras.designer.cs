using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaComiteCompras
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode3 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode4 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvOCPendientes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcProyectos = new DevExpress.XtraGrid.GridControl();
            this.gvOCEnProceso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvRecPendientes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvFactPendientes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvProyectos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvDetalleRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecurso = new DevExpress.XtraGrid.GridControl();
            this.gvRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.chkNoPendientes = new DevExpress.XtraEditors.CheckEdit();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.grpCtrlProvider = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnCompra = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOCPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcProyectos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOCEnProceso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFactPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProyectos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNoPendientes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).BeginInit();
            this.grpCtrlProvider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnCompra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.SuspendLayout();
            // 
            // gvOCPendientes
            // 
            this.gvOCPendientes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCPendientes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvOCPendientes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOCPendientes.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCPendientes.Appearance.Empty.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.Empty.Options.UseFont = true;
            this.gvOCPendientes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvOCPendientes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCPendientes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvOCPendientes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.FocusedCell.Options.UseFont = true;
            this.gvOCPendientes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCPendientes.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvOCPendientes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCPendientes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.FocusedRow.Options.UseFont = true;
            this.gvOCPendientes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCPendientes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvOCPendientes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvOCPendientes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCPendientes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvOCPendientes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOCPendientes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvOCPendientes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvOCPendientes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvOCPendientes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCPendientes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvOCPendientes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvOCPendientes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvOCPendientes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvOCPendientes.Appearance.Row.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.Row.Options.UseFont = true;
            this.gvOCPendientes.Appearance.Row.Options.UseForeColor = true;
            this.gvOCPendientes.Appearance.Row.Options.UseTextOptions = true;
            this.gvOCPendientes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOCPendientes.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCPendientes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOCPendientes.Appearance.SelectedRow.Options.UseFont = true;
            this.gvOCPendientes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCPendientes.Appearance.TopNewRow.Options.UseFont = true;
            this.gvOCPendientes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvOCPendientes.GridControl = this.gcProyectos;
            this.gvOCPendientes.HorzScrollStep = 50;
            this.gvOCPendientes.Name = "gvOCPendientes";
            this.gvOCPendientes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvOCPendientes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvOCPendientes.OptionsCustomization.AllowColumnMoving = false;
            this.gvOCPendientes.OptionsCustomization.AllowFilter = false;
            this.gvOCPendientes.OptionsDetail.EnableMasterViewMode = false;
            this.gvOCPendientes.OptionsMenu.EnableColumnMenu = false;
            this.gvOCPendientes.OptionsMenu.EnableFooterMenu = false;
            this.gvOCPendientes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvOCPendientes.OptionsView.ColumnAutoWidth = false;
            this.gvOCPendientes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvOCPendientes.OptionsView.ShowGroupPanel = false;
            this.gvOCPendientes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvOCPendientes.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvOCPendientes_RowClick);
            this.gvOCPendientes.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalleSol_FocusedRowChanged);
            this.gvOCPendientes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvOCPendientes.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalleSol_CustomColumnDisplayText);
            // 
            // gcProyectos
            // 
            this.gcProyectos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcProyectos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcProyectos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcProyectos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcProyectos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcProyectos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcProyectos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcProyectos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcProyectos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcProyectos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcProyectos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcProyectos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcProyectos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcProyectos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcProyectos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvOCPendientes;
            gridLevelNode1.RelationName = "OrdenCompraPendientes";
            gridLevelNode2.LevelTemplate = this.gvOCEnProceso;
            gridLevelNode2.RelationName = "OrdenCompraEnProceso";
            gridLevelNode3.LevelTemplate = this.gvRecPendientes;
            gridLevelNode3.RelationName = "RecibidoPendientes";
            gridLevelNode4.LevelTemplate = this.gvFactPendientes;
            gridLevelNode4.RelationName = "FacturaPendientes";
            this.gcProyectos.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2,
            gridLevelNode3,
            gridLevelNode4});
            this.gcProyectos.Location = new System.Drawing.Point(0, 13);
            this.gcProyectos.LookAndFeel.SkinName = "Dark Side";
            this.gcProyectos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcProyectos.MainView = this.gvProyectos;
            this.gcProyectos.Margin = new System.Windows.Forms.Padding(5);
            this.gcProyectos.Name = "gcProyectos";
            this.gcProyectos.Size = new System.Drawing.Size(1021, 373);
            this.gcProyectos.TabIndex = 50;
            this.gcProyectos.UseEmbeddedNavigator = true;
            this.gcProyectos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOCEnProceso,
            this.gvRecPendientes,
            this.gvFactPendientes,
            this.gvProyectos,
            this.gvOCPendientes});
            this.gcProyectos.FocusedViewChanged += new DevExpress.XtraGrid.ViewFocusEventHandler(this.gcProyectos_FocusedViewChanged);
            // 
            // gvOCEnProceso
            // 
            this.gvOCEnProceso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCEnProceso.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOCEnProceso.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCEnProceso.Appearance.Empty.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.Empty.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvOCEnProceso.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCEnProceso.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvOCEnProceso.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.FocusedCell.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCEnProceso.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvOCEnProceso.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCEnProceso.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.FocusedRow.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCEnProceso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvOCEnProceso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvOCEnProceso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOCEnProceso.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvOCEnProceso.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOCEnProceso.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvOCEnProceso.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvOCEnProceso.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvOCEnProceso.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCEnProceso.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvOCEnProceso.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvOCEnProceso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvOCEnProceso.Appearance.Row.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.Row.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.Row.Options.UseForeColor = true;
            this.gvOCEnProceso.Appearance.Row.Options.UseTextOptions = true;
            this.gvOCEnProceso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOCEnProceso.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvOCEnProceso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOCEnProceso.Appearance.SelectedRow.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvOCEnProceso.Appearance.TopNewRow.Options.UseFont = true;
            this.gvOCEnProceso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvOCEnProceso.GridControl = this.gcProyectos;
            this.gvOCEnProceso.HorzScrollStep = 50;
            this.gvOCEnProceso.Name = "gvOCEnProceso";
            this.gvOCEnProceso.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvOCEnProceso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvOCEnProceso.OptionsCustomization.AllowColumnMoving = false;
            this.gvOCEnProceso.OptionsCustomization.AllowFilter = false;
            this.gvOCEnProceso.OptionsDetail.EnableMasterViewMode = false;
            this.gvOCEnProceso.OptionsMenu.EnableColumnMenu = false;
            this.gvOCEnProceso.OptionsMenu.EnableFooterMenu = false;
            this.gvOCEnProceso.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvOCEnProceso.OptionsView.ColumnAutoWidth = false;
            this.gvOCEnProceso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvOCEnProceso.OptionsView.ShowGroupPanel = false;
            this.gvOCEnProceso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvOCEnProceso.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvOCPendientes_RowClick);
            this.gvOCEnProceso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalleSol_FocusedRowChanged);
            this.gvOCEnProceso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvOCEnProceso.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalleSol_CustomColumnDisplayText);
            // 
            // gvRecPendientes
            // 
            this.gvRecPendientes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecPendientes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRecPendientes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecPendientes.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecPendientes.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.Empty.Options.UseFont = true;
            this.gvRecPendientes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRecPendientes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvRecPendientes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecPendientes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.FocusedCell.Options.UseFont = true;
            this.gvRecPendientes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvRecPendientes.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvRecPendientes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecPendientes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.FocusedRow.Options.UseFont = true;
            this.gvRecPendientes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecPendientes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecPendientes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecPendientes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecPendientes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecPendientes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecPendientes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRecPendientes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRecPendientes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRecPendientes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvRecPendientes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecPendientes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvRecPendientes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvRecPendientes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecPendientes.Appearance.Row.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.Row.Options.UseFont = true;
            this.gvRecPendientes.Appearance.Row.Options.UseForeColor = true;
            this.gvRecPendientes.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecPendientes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecPendientes.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvRecPendientes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecPendientes.Appearance.SelectedRow.Options.UseFont = true;
            this.gvRecPendientes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecPendientes.Appearance.TopNewRow.Options.UseFont = true;
            this.gvRecPendientes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecPendientes.GridControl = this.gcProyectos;
            this.gvRecPendientes.HorzScrollStep = 50;
            this.gvRecPendientes.Name = "gvRecPendientes";
            this.gvRecPendientes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvRecPendientes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecPendientes.OptionsCustomization.AllowColumnMoving = false;
            this.gvRecPendientes.OptionsCustomization.AllowFilter = false;
            this.gvRecPendientes.OptionsDetail.EnableMasterViewMode = false;
            this.gvRecPendientes.OptionsMenu.EnableColumnMenu = false;
            this.gvRecPendientes.OptionsMenu.EnableFooterMenu = false;
            this.gvRecPendientes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRecPendientes.OptionsView.ColumnAutoWidth = false;
            this.gvRecPendientes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecPendientes.OptionsView.ShowGroupPanel = false;
            this.gvRecPendientes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecPendientes.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvOCPendientes_RowClick);
            this.gvRecPendientes.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalleSol_FocusedRowChanged);
            this.gvRecPendientes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvRecPendientes.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalleSol_CustomColumnDisplayText);
            // 
            // gvFactPendientes
            // 
            this.gvFactPendientes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvFactPendientes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvFactPendientes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvFactPendientes.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvFactPendientes.Appearance.Empty.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.Empty.Options.UseFont = true;
            this.gvFactPendientes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvFactPendientes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvFactPendientes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvFactPendientes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.FocusedCell.Options.UseFont = true;
            this.gvFactPendientes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvFactPendientes.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvFactPendientes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvFactPendientes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.FocusedRow.Options.UseFont = true;
            this.gvFactPendientes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvFactPendientes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvFactPendientes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvFactPendientes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvFactPendientes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvFactPendientes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFactPendientes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvFactPendientes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvFactPendientes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvFactPendientes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvFactPendientes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvFactPendientes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvFactPendientes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvFactPendientes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvFactPendientes.Appearance.Row.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.Row.Options.UseFont = true;
            this.gvFactPendientes.Appearance.Row.Options.UseForeColor = true;
            this.gvFactPendientes.Appearance.Row.Options.UseTextOptions = true;
            this.gvFactPendientes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvFactPendientes.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvFactPendientes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFactPendientes.Appearance.SelectedRow.Options.UseFont = true;
            this.gvFactPendientes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvFactPendientes.Appearance.TopNewRow.Options.UseFont = true;
            this.gvFactPendientes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvFactPendientes.GridControl = this.gcProyectos;
            this.gvFactPendientes.HorzScrollStep = 50;
            this.gvFactPendientes.Name = "gvFactPendientes";
            this.gvFactPendientes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvFactPendientes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvFactPendientes.OptionsCustomization.AllowColumnMoving = false;
            this.gvFactPendientes.OptionsCustomization.AllowFilter = false;
            this.gvFactPendientes.OptionsDetail.EnableMasterViewMode = false;
            this.gvFactPendientes.OptionsMenu.EnableColumnMenu = false;
            this.gvFactPendientes.OptionsMenu.EnableFooterMenu = false;
            this.gvFactPendientes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvFactPendientes.OptionsView.ColumnAutoWidth = false;
            this.gvFactPendientes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvFactPendientes.OptionsView.ShowGroupPanel = false;
            this.gvFactPendientes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvFactPendientes.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvOCPendientes_RowClick);
            this.gvFactPendientes.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalleSol_FocusedRowChanged);
            this.gvFactPendientes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvFactPendientes.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalleSol_CustomColumnDisplayText);
            // 
            // gvProyectos
            // 
            this.gvProyectos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProyectos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvProyectos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvProyectos.Appearance.Empty.Options.UseBackColor = true;
            this.gvProyectos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvProyectos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvProyectos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProyectos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvProyectos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvProyectos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvProyectos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProyectos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvProyectos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvProyectos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvProyectos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvProyectos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvProyectos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvProyectos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvProyectos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProyectos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvProyectos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvProyectos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvProyectos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvProyectos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvProyectos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvProyectos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvProyectos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvProyectos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProyectos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvProyectos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvProyectos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvProyectos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProyectos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvProyectos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvProyectos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvProyectos.Appearance.Row.Options.UseBackColor = true;
            this.gvProyectos.Appearance.Row.Options.UseForeColor = true;
            this.gvProyectos.Appearance.Row.Options.UseTextOptions = true;
            this.gvProyectos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvProyectos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProyectos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvProyectos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvProyectos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvProyectos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProyectos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvProyectos.GridControl = this.gcProyectos;
            this.gvProyectos.HorzScrollStep = 50;
            this.gvProyectos.Name = "gvProyectos";
            this.gvProyectos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvProyectos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvProyectos.OptionsCustomization.AllowColumnMoving = false;
            this.gvProyectos.OptionsCustomization.AllowFilter = false;
            this.gvProyectos.OptionsMenu.EnableColumnMenu = false;
            this.gvProyectos.OptionsMenu.EnableFooterMenu = false;
            this.gvProyectos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvProyectos.OptionsView.ColumnAutoWidth = false;
            this.gvProyectos.OptionsView.ShowAutoFilterRow = true;
            this.gvProyectos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvProyectos.OptionsView.ShowGroupPanel = false;
            this.gvProyectos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvProyectos.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDocument_RowCellStyle);
            this.gvProyectos.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvDocument_RowStyle);
            this.gvProyectos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvProyectos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
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
            this.gcRecurso.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcRecurso.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null)});
            this.gcRecurso.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecurso.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcRecurso.Location = new System.Drawing.Point(0, 17);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(1021, 114);
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
            this.gvRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.Row.Options.UseBackColor = true;
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
            this.gvRecurso.OptionsDetail.EnableMasterViewMode = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvRecurso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecurso.OptionsView.ShowGroupPanel = false;
            this.gvRecurso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecurso.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDocument_RowCellStyle);
            this.gvRecurso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
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
            this.pnlMainContainer.Size = new System.Drawing.Size(1080, 602);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.66666F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1076, 598);
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
            this.grpctrlHeader.AutoSize = true;
            this.grpctrlHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.grpctrlHeader.Controls.Add(this.chkNoPendientes);
            this.grpctrlHeader.Controls.Add(this.lblDate);
            this.grpctrlHeader.Controls.Add(this.dtFechaCorte);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(17, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(1021, 42);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // chkNoPendientes
            // 
            this.chkNoPendientes.Location = new System.Drawing.Point(907, 13);
            this.chkNoPendientes.Margin = new System.Windows.Forms.Padding(2);
            this.chkNoPendientes.Name = "chkNoPendientes";
            this.chkNoPendientes.Properties.AutoWidth = true;
            this.chkNoPendientes.Properties.Caption = "Ver No Pendientes";
            this.chkNoPendientes.Size = new System.Drawing.Size(110, 19);
            this.chkNoPendientes.TabIndex = 115;
            this.chkNoPendientes.CheckedChanged += new System.EventHandler(this.chkNoPendientes_CheckedChanged);
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(14, 14);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(130, 14);
            this.lblDate.TabIndex = 114;
            this.lblDate.Text = "33311_lblFechaCorte";
            // 
            // dtFechaCorte
            // 
            this.dtFechaCorte.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaCorte.Location = new System.Drawing.Point(112, 12);
            this.dtFechaCorte.Name = "dtFechaCorte";
            this.dtFechaCorte.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCorte.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCorte.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCorte.Size = new System.Drawing.Size(100, 20);
            this.dtFechaCorte.TabIndex = 113;
            this.dtFechaCorte.EditValueChanged += new System.EventHandler(this.dtFechaCorte_EditValueChanged);
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(17, 579);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1021, 16);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.AutoSize = true;
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Size = new System.Drawing.Size(1021, 16);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(17, 51);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1021, 522);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcProyectos);
            this.splitGrids.Panel1.Controls.Add(this.labelControl1);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.gcRecurso);
            this.splitGrids.Panel2.Controls.Add(this.grpCtrlProvider);
            this.splitGrids.Size = new System.Drawing.Size(1021, 522);
            this.splitGrids.SplitterPosition = 386;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(89, 13);
            this.labelControl1.TabIndex = 113;
            this.labelControl1.Text = "33311_lblItems";
            // 
            // grpCtrlProvider
            // 
            this.grpCtrlProvider.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F);
            this.grpCtrlProvider.AppearanceCaption.Options.UseFont = true;
            this.grpCtrlProvider.Controls.Add(this.labelControl2);
            this.grpCtrlProvider.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCtrlProvider.Location = new System.Drawing.Point(0, 0);
            this.grpCtrlProvider.Margin = new System.Windows.Forms.Padding(2);
            this.grpCtrlProvider.Name = "grpCtrlProvider";
            this.grpCtrlProvider.ShowCaption = false;
            this.grpCtrlProvider.Size = new System.Drawing.Size(1021, 17);
            this.grpCtrlProvider.TabIndex = 52;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(107, 13);
            this.labelControl2.TabIndex = 114;
            this.labelControl2.Text = "33311_lblRecursos";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue2,
            this.editBtnCompra,
            this.editValue2Cant,
            this.editLink});
            // 
            // editValue2
            // 
            this.editValue2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editValue2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editValue2.Mask.EditMask = "c2";
            this.editValue2.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2.Name = "editValue2";
            // 
            // editBtnCompra
            // 
            this.editBtnCompra.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Editar", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "Ver detalle de la referencia", null, null, true)});
            this.editBtnCompra.Name = "editBtnCompra";
            this.editBtnCompra.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnCompra.ValidateOnEnterKey = true;
            this.editBtnCompra.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnCompra_ButtonClick);
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
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // ConsultaComiteCompras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 602);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ConsultaComiteCompras";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gvOCPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcProyectos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOCEnProceso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFactPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProyectos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNoPendientes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).EndInit();
            this.grpCtrlProvider.ResumeLayout(false);
            this.grpCtrlProvider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnCompra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.GroupBox grpboxDetail;
        private DevExpress.XtraGrid.GridControl gcProyectos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProyectos;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlGrids;
        private DevExpress.XtraGrid.GridControl gcRecurso;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRecurso;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
       
        private DevExpress.XtraGrid.Views.Grid.GridView gvOCPendientes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalleRecurso;
        private DevExpress.XtraEditors.GroupControl grpCtrlProvider;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblDate;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOCEnProceso;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRecPendientes;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnCompra;
        private DevExpress.XtraGrid.Views.Grid.GridView gvFactPendientes;
        private DevExpress.XtraEditors.CheckEdit chkNoPendientes;
    }
}