using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ActaEntregaPreFacturaVenta
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvTareasProy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcHeader = new DevExpress.XtraGrid.GridControl();
            this.gvHeader = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gvSubDetalleFact = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcFooterFact = new DevExpress.XtraGrid.GridControl();
            this.gvFooterFact = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.lblTitleGrid = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPorRetegarantia = new DevExpress.XtraEditors.TextEdit();
            this.masterBodega = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtPorcFacturaActual = new DevExpress.XtraEditors.TextEdit();
            this.txtVlrAmortizacion = new DevExpress.XtraEditors.TextEdit();
            this.lblAmortiza = new System.Windows.Forms.Label();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.gbResumen = new DevExpress.XtraEditors.GroupControl();
            this.btnResumenEjecucion = new DevExpress.XtraEditors.SimpleButton();
            this.ucProyecto = new NewAge.Cliente.GUI.WinApp.Forms.UC_Proyecto();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editPoPup = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.editGridCmb = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtSaldoAnticipo = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareasProy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubDetalleFact)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFooterFact)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFooterFact)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorRetegarantia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcFacturaActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAmortizacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbResumen)).BeginInit();
            this.gbResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGridCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoAnticipo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gvTareasProy
            // 
            this.gvTareasProy.GridControl = this.gcHeader;
            this.gvTareasProy.Name = "gvTareasProy";
            // 
            // gcHeader
            // 
            this.gcHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcHeader.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcHeader.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcHeader.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcHeader.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcHeader.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcHeader.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcHeader.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcHeader.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcHeader.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcHeader.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcHeader.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcHeader.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcHeader.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvTareasProy;
            gridLevelNode1.RelationName = "DetalleTareas";
            gridLevelNode2.RelationName = "Level1";
            this.gcHeader.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.gcHeader.Location = new System.Drawing.Point(0, 23);
            this.gcHeader.LookAndFeel.SkinName = "Dark Side";
            this.gcHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcHeader.MainView = this.gvHeader;
            this.gcHeader.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.Name = "gcHeader";
            this.gcHeader.Size = new System.Drawing.Size(1138, 201);
            this.gcHeader.TabIndex = 114;
            this.gcHeader.UseEmbeddedNavigator = true;
            this.gcHeader.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHeader,
            this.gvTareasProy});
            // 
            // gvHeader
            // 
            this.gvHeader.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHeader.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvHeader.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvHeader.Appearance.Empty.Options.UseBackColor = true;
            this.gvHeader.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvHeader.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvHeader.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHeader.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvHeader.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvHeader.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHeader.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvHeader.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvHeader.Appearance.GroupRow.BackColor = System.Drawing.Color.White;
            this.gvHeader.Appearance.GroupRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvHeader.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvHeader.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvHeader.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvHeader.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvHeader.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHeader.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvHeader.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvHeader.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvHeader.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvHeader.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvHeader.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvHeader.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvHeader.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvHeader.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHeader.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvHeader.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvHeader.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvHeader.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvHeader.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvHeader.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvHeader.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvHeader.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.Row.Options.UseBackColor = true;
            this.gvHeader.Appearance.Row.Options.UseFont = true;
            this.gvHeader.Appearance.Row.Options.UseForeColor = true;
            this.gvHeader.Appearance.Row.Options.UseTextOptions = true;
            this.gvHeader.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvHeader.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHeader.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvHeader.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvHeader.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvHeader.Appearance.VertLine.Options.UseBackColor = true;
            this.gvHeader.GridControl = this.gcHeader;
            this.gvHeader.GroupFormat = "{1} {2}";
            this.gvHeader.HorzScrollStep = 50;
            this.gvHeader.Name = "gvHeader";
            this.gvHeader.OptionsCustomization.AllowColumnMoving = false;
            this.gvHeader.OptionsCustomization.AllowFilter = false;
            this.gvHeader.OptionsCustomization.AllowSort = false;
            this.gvHeader.OptionsMenu.EnableColumnMenu = false;
            this.gvHeader.OptionsMenu.EnableFooterMenu = false;
            this.gvHeader.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvHeader.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHeader.OptionsView.ShowGroupPanel = false;
            this.gvHeader.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHeader.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvHeader_FocusedRowChanged);
            this.gvHeader.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvHeader_CellValueChanged);
            this.gvHeader.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvHeader_CellValueChanging);
            this.gvHeader.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
            // 
            // gvSubDetalleFact
            // 
            this.gvSubDetalleFact.GridControl = this.gcFooterFact;
            this.gvSubDetalleFact.Name = "gvSubDetalleFact";
            // 
            // gcFooterFact
            // 
            this.gcFooterFact.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcFooterFact.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcFooterFact.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcFooterFact.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcFooterFact.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcFooterFact.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcFooterFact.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcFooterFact.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcFooterFact.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcFooterFact.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcFooterFact.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcFooterFact.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcFooterFact.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcFooterFact.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcFooterFact.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcFooterFact.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode3.LevelTemplate = this.gvSubDetalleFact;
            gridLevelNode3.RelationName = "DetalleTareas";
            this.gcFooterFact.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode3});
            this.gcFooterFact.Location = new System.Drawing.Point(0, 25);
            this.gcFooterFact.LookAndFeel.SkinName = "Dark Side";
            this.gcFooterFact.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcFooterFact.MainView = this.gvFooterFact;
            this.gcFooterFact.Margin = new System.Windows.Forms.Padding(5);
            this.gcFooterFact.Name = "gcFooterFact";
            this.gcFooterFact.Size = new System.Drawing.Size(1138, 201);
            this.gcFooterFact.TabIndex = 115;
            this.gcFooterFact.UseEmbeddedNavigator = true;
            this.gcFooterFact.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFooterFact,
            this.gvSubDetalleFact});
            // 
            // gvFooterFact
            // 
            this.gvFooterFact.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvFooterFact.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvFooterFact.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvFooterFact.Appearance.Empty.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvFooterFact.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvFooterFact.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvFooterFact.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvFooterFact.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvFooterFact.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.GroupRow.BackColor = System.Drawing.Color.White;
            this.gvFooterFact.Appearance.GroupRow.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvFooterFact.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvFooterFact.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvFooterFact.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvFooterFact.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvFooterFact.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvFooterFact.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFooterFact.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvFooterFact.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvFooterFact.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvFooterFact.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvFooterFact.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvFooterFact.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvFooterFact.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvFooterFact.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvFooterFact.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvFooterFact.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvFooterFact.Appearance.Row.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.Row.Options.UseFont = true;
            this.gvFooterFact.Appearance.Row.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.Row.Options.UseTextOptions = true;
            this.gvFooterFact.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvFooterFact.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvFooterFact.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFooterFact.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvFooterFact.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvFooterFact.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvFooterFact.Appearance.VertLine.Options.UseBackColor = true;
            this.gvFooterFact.GridControl = this.gcFooterFact;
            this.gvFooterFact.GroupFormat = "{1} {2}";
            this.gvFooterFact.HorzScrollStep = 50;
            this.gvFooterFact.Name = "gvFooterFact";
            this.gvFooterFact.OptionsCustomization.AllowColumnMoving = false;
            this.gvFooterFact.OptionsCustomization.AllowFilter = false;
            this.gvFooterFact.OptionsCustomization.AllowSort = false;
            this.gvFooterFact.OptionsMenu.EnableColumnMenu = false;
            this.gvFooterFact.OptionsMenu.EnableFooterMenu = false;
            this.gvFooterFact.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvFooterFact.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvFooterFact.OptionsView.ShowGroupPanel = false;
            this.gvFooterFact.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvFooterFact.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvFooterFact_CellValueChanged);
            this.gvFooterFact.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvFooterFact_BeforeLeaveRow);
            this.gvFooterFact.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvFooterFact_CustomUnboundColumnData);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1165, 688);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlSeparatorPanel.Controls.Add(this.splitGrids, 1, 1);
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 4;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 507F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1161, 684);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(10, 112);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcHeader);
            this.splitGrids.Panel1.Controls.Add(this.lblTitleGrid);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.txtSaldoAnticipo);
            this.splitGrids.Panel2.Controls.Add(this.label2);
            this.splitGrids.Panel2.Controls.Add(this.label1);
            this.splitGrids.Panel2.Controls.Add(this.txtPorRetegarantia);
            this.splitGrids.Panel2.Controls.Add(this.masterBodega);
            this.splitGrids.Panel2.Controls.Add(this.txtTotal);
            this.splitGrids.Panel2.Controls.Add(this.lblTotal);
            this.splitGrids.Panel2.Controls.Add(this.txtPorcFacturaActual);
            this.splitGrids.Panel2.Controls.Add(this.txtVlrAmortizacion);
            this.splitGrids.Panel2.Controls.Add(this.gcFooterFact);
            this.splitGrids.Panel2.Controls.Add(this.lblAmortiza);
            this.splitGrids.Panel2.Controls.Add(this.labelControl1);
            this.splitGrids.Size = new System.Drawing.Size(1138, 503);
            this.splitGrids.SplitterPosition = 224;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Location = new System.Drawing.Point(0, 0);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Size = new System.Drawing.Size(1138, 23);
            this.lblTitleGrid.TabIndex = 113;
            this.lblTitleGrid.Text = "114_lblTareaCliente";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(497, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 1007;
            this.label1.Text = "ReteGarantia";
            // 
            // txtPorRetegarantia
            // 
            this.txtPorRetegarantia.EditValue = "0";
            this.txtPorRetegarantia.Location = new System.Drawing.Point(575, 232);
            this.txtPorRetegarantia.Name = "txtPorRetegarantia";
            this.txtPorRetegarantia.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorRetegarantia.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPorRetegarantia.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorRetegarantia.Properties.Appearance.Options.UseFont = true;
            this.txtPorRetegarantia.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorRetegarantia.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorRetegarantia.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorRetegarantia.Properties.Mask.EditMask = "P";
            this.txtPorRetegarantia.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorRetegarantia.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorRetegarantia.Properties.ReadOnly = true;
            this.txtPorRetegarantia.Properties.UseReadOnlyAppearance = false;
            this.txtPorRetegarantia.Size = new System.Drawing.Size(40, 20);
            this.txtPorRetegarantia.TabIndex = 1006;
            // 
            // masterBodega
            // 
            this.masterBodega.BackColor = System.Drawing.Color.Transparent;
            this.masterBodega.Filtros = null;
            this.masterBodega.Location = new System.Drawing.Point(817, -1);
            this.masterBodega.Name = "masterBodega";
            this.masterBodega.Size = new System.Drawing.Size(299, 25);
            this.masterBodega.TabIndex = 1;
            this.masterBodega.Value = "";
            // 
            // txtTotal
            // 
            this.txtTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotal.EditValue = "0,00 ";
            this.txtTotal.Location = new System.Drawing.Point(1021, 231);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtTotal.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotal.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotal.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotal.Properties.Appearance.Options.UseFont = true;
            this.txtTotal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotal.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtTotal.Properties.AutoHeight = false;
            this.txtTotal.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtTotal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.Properties.Mask.EditMask = "c2";
            this.txtTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotal.Properties.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(113, 20);
            this.txtTotal.TabIndex = 117;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(902, 232);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(120, 18);
            this.lblTotal.TabIndex = 116;
            this.lblTotal.Text = "33509_lblTotal";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPorcFacturaActual
            // 
            this.txtPorcFacturaActual.EditValue = "0";
            this.txtPorcFacturaActual.Location = new System.Drawing.Point(844, 231);
            this.txtPorcFacturaActual.Name = "txtPorcFacturaActual";
            this.txtPorcFacturaActual.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtPorcFacturaActual.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPorcFacturaActual.Properties.Appearance.Options.UseBorderColor = true;
            this.txtPorcFacturaActual.Properties.Appearance.Options.UseFont = true;
            this.txtPorcFacturaActual.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPorcFacturaActual.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtPorcFacturaActual.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPorcFacturaActual.Properties.Mask.EditMask = "P";
            this.txtPorcFacturaActual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPorcFacturaActual.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPorcFacturaActual.Properties.ReadOnly = true;
            this.txtPorcFacturaActual.Properties.UseReadOnlyAppearance = false;
            this.txtPorcFacturaActual.Size = new System.Drawing.Size(40, 20);
            this.txtPorcFacturaActual.TabIndex = 1003;
            this.txtPorcFacturaActual.Visible = false;
            // 
            // txtVlrAmortizacion
            // 
            this.txtVlrAmortizacion.EditValue = "0";
            this.txtVlrAmortizacion.Location = new System.Drawing.Point(735, 231);
            this.txtVlrAmortizacion.Name = "txtVlrAmortizacion";
            this.txtVlrAmortizacion.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtVlrAmortizacion.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrAmortizacion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrAmortizacion.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrAmortizacion.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrAmortizacion.Properties.Appearance.Options.UseFont = true;
            this.txtVlrAmortizacion.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrAmortizacion.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrAmortizacion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrAmortizacion.Properties.Mask.EditMask = "c";
            this.txtVlrAmortizacion.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrAmortizacion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrAmortizacion.Properties.ReadOnly = true;
            this.txtVlrAmortizacion.Size = new System.Drawing.Size(107, 20);
            this.txtVlrAmortizacion.TabIndex = 1004;
            // 
            // lblAmortiza
            // 
            this.lblAmortiza.AutoSize = true;
            this.lblAmortiza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmortiza.Location = new System.Drawing.Point(642, 234);
            this.lblAmortiza.Name = "lblAmortiza";
            this.lblAmortiza.Size = new System.Drawing.Size(94, 14);
            this.lblAmortiza.TabIndex = 1005;
            this.lblAmortiza.Text = "Vlr Amortización";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(1138, 25);
            this.labelControl1.TabIndex = 114;
            this.labelControl1.Text = "114_lblDetalleFact";
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
            this.grpctrlHeader.Controls.Add(this.gbResumen);
            this.grpctrlHeader.Controls.Add(this.ucProyecto);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(11, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(1136, 104);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // gbResumen
            // 
            this.gbResumen.Controls.Add(this.btnResumenEjecucion);
            this.gbResumen.Location = new System.Drawing.Point(722, 4);
            this.gbResumen.Name = "gbResumen";
            this.gbResumen.Size = new System.Drawing.Size(245, 97);
            this.gbResumen.TabIndex = 1001;
            // 
            // btnResumenEjecucion
            // 
            this.btnResumenEjecucion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnResumenEjecucion.Appearance.Options.UseFont = true;
            this.btnResumenEjecucion.Location = new System.Drawing.Point(25, 40);
            this.btnResumenEjecucion.Name = "btnResumenEjecucion";
            this.btnResumenEjecucion.Size = new System.Drawing.Size(148, 23);
            this.btnResumenEjecucion.TabIndex = 1008;
            this.btnResumenEjecucion.Text = "Estado Proyecto";
            this.btnResumenEjecucion.Click += new System.EventHandler(this.btnResumenEjecucion_Click);
            // 
            // ucProyecto
            // 
            this.ucProyecto.BackColor = System.Drawing.Color.Transparent;
            this.ucProyecto.ClienteID = "";
            this.ucProyecto.DocumentoNro = null;
            this.ucProyecto.Location = new System.Drawing.Point(1, 5);
            this.ucProyecto.Name = "ucProyecto";
            this.ucProyecto.PrefijoID = "";
            this.ucProyecto.ProyectoID = "";
            this.ucProyecto.ProyectoInfo = null;
            this.ucProyecto.Size = new System.Drawing.Size(725, 96);
            this.ucProyecto.TabIndex = 0;
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue2,
            this.editPorc,
            this.editBtnGrid,
            this.editValue2Cant,
            this.editLink,
            this.editPoPup,
            this.editCmb,
            this.editGridCmb});
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
            // editPorc
            // 
            this.editPorc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editPorc.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editPorc.Mask.EditMask = "P1";
            this.editPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editPorc.Name = "editPorc";
            this.editPorc.NullText = "0";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
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
            // 
            // editPoPup
            // 
            this.editPoPup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editPoPup.Name = "editPoPup";
            this.editPoPup.ShowIcon = false;
            // 
            // editCmb
            // 
            this.editCmb.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editCmb.Name = "editCmb";
            // 
            // editGridCmb
            // 
            this.editGridCmb.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editGridCmb.Name = "editGridCmb";
            this.editGridCmb.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txtSaldoAnticipo
            // 
            this.txtSaldoAnticipo.EditValue = "0";
            this.txtSaldoAnticipo.Location = new System.Drawing.Point(77, 231);
            this.txtSaldoAnticipo.Name = "txtSaldoAnticipo";
            this.txtSaldoAnticipo.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtSaldoAnticipo.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtSaldoAnticipo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldoAnticipo.Properties.Appearance.Options.UseBackColor = true;
            this.txtSaldoAnticipo.Properties.Appearance.Options.UseBorderColor = true;
            this.txtSaldoAnticipo.Properties.Appearance.Options.UseFont = true;
            this.txtSaldoAnticipo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldoAnticipo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldoAnticipo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldoAnticipo.Properties.Mask.EditMask = "c";
            this.txtSaldoAnticipo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldoAnticipo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldoAnticipo.Properties.ReadOnly = true;
            this.txtSaldoAnticipo.Size = new System.Drawing.Size(107, 18);
            this.txtSaldoAnticipo.TabIndex = 1008;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1009;
            this.label2.Text = "Saldo Anticipo";
            // 
            // ActaEntregaPreFacturaVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 688);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ActaEntregaPreFacturaVenta";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gvTareasProy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubDetalleFact)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFooterFact)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFooterFact)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPorRetegarantia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorcFacturaActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAmortizacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbResumen)).EndInit();
            this.gbResumen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGridCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoAnticipo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.LabelControl lblTitleGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editPorc;
        protected DevExpress.XtraGrid.GridControl gcHeader;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvHeader;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit editPoPup;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editCmb;
        private UC_Proyecto ucProyecto;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit editGridCmb;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTareasProy;
        protected DevExpress.XtraGrid.GridControl gcFooterFact;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSubDetalleFact;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvFooterFact;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private System.Windows.Forms.Label lblTotal;
        private uc_MasterFind masterBodega;
        private DevExpress.XtraEditors.GroupControl gbResumen;
        private DevExpress.XtraEditors.TextEdit txtVlrAmortizacion;
        private System.Windows.Forms.Label lblAmortiza;
        private DevExpress.XtraEditors.TextEdit txtPorcFacturaActual;
        private DevExpress.XtraEditors.SimpleButton btnResumenEjecucion;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtPorRetegarantia;
        private DevExpress.XtraEditors.TextEdit txtSaldoAnticipo;
        private System.Windows.Forms.Label label2;
    }
}