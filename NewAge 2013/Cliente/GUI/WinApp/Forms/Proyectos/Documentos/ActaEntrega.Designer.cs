using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ActaEntrega
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActaEntrega));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gcDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcHeader = new DevExpress.XtraGrid.GridControl();
            this.gvHeader = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblTitleGrid = new DevExpress.XtraEditors.LabelControl();
            this.grpCtrlProvider = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gcTareas = new DevExpress.XtraGrid.GridControl();
            this.gvTareas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.lblDocTercero = new DevExpress.XtraEditors.LabelControl();
            this.txtDocTercero = new System.Windows.Forms.TextBox();
            this.pnActa = new DevExpress.XtraEditors.PanelControl();
            this.lblNroActa = new DevExpress.XtraEditors.LabelControl();
            this.txtNroActa = new DevExpress.XtraEditors.TextEdit();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.lblFechaActa = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            this.lblRespCliente = new DevExpress.XtraEditors.LabelControl();
            this.txtRespCliente = new System.Windows.Forms.TextBox();
            this.dtFechaActa = new DevExpress.XtraEditors.DateEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).BeginInit();
            this.grpCtrlProvider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnActa)).BeginInit();
            this.pnActa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroActa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaActa.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaActa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGridCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDetalle
            // 
            this.gcDetalle.AllowDrop = true;
            this.gcDetalle.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetalle.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetalle.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetalle.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetalle.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetalle.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null)});
            this.gcDetalle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcDetalle.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetalle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDetalle.Location = new System.Drawing.Point(0, 18);
            this.gcDetalle.LookAndFeel.SkinName = "Dark Side";
            this.gcDetalle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetalle.MainView = this.gvDetalle;
            this.gcDetalle.Margin = new System.Windows.Forms.Padding(5);
            this.gcDetalle.Name = "gcDetalle";
            this.gcDetalle.Size = new System.Drawing.Size(1060, 106);
            this.gcDetalle.TabIndex = 51;
            this.gcDetalle.UseEmbeddedNavigator = true;
            this.gcDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            this.gcDetalle.Leave += new System.EventHandler(this.gcDetalle_Leave);
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvDetalle.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvDetalle.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.gvDetalle.Appearance.ViewCaption.Options.UseFont = true;
            this.gvDetalle.GridControl = this.gcDetalle;
            this.gvDetalle.GroupFormat = "[#image]{1} {2}";
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsDetail.EnableMasterViewMode = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDetalle_BeforeLeaveRow);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
            this.gvDetalle.DataSourceChanged += new System.EventHandler(this.gvDetalle_DataSourceChanged);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1144, 672);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlSeparatorPanel.Controls.Add(this.splitGrids, 1, 1);
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 551F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1140, 668);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(17, 110);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcHeader);
            this.splitGrids.Panel1.Controls.Add(this.lblTitleGrid);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.gcDetalle);
            this.splitGrids.Panel2.Controls.Add(this.grpCtrlProvider);
            this.splitGrids.Panel2.Controls.Add(this.panelControl1);
            this.splitGrids.Size = new System.Drawing.Size(1091, 547);
            this.splitGrids.SplitterPosition = 292;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
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
            this.gcHeader.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcHeader_EmbeddedNavigator_ButtonClick);
            this.gcHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcHeader.Location = new System.Drawing.Point(0, 14);
            this.gcHeader.LookAndFeel.SkinName = "Dark Side";
            this.gcHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcHeader.MainView = this.gvHeader;
            this.gcHeader.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.Name = "gcHeader";
            this.gcHeader.Size = new System.Drawing.Size(1091, 278);
            this.gcHeader.TabIndex = 114;
            this.gcHeader.UseEmbeddedNavigator = true;
            this.gcHeader.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHeader});
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
            this.gvHeader.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gvHeader.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvHeader.GridControl = this.gcHeader;
            this.gvHeader.GroupFormat = "{1} {2}";
            this.gvHeader.HorzScrollStep = 50;
            this.gvHeader.Name = "gvHeader";
            this.gvHeader.OptionsCustomization.AllowColumnMoving = false;
            this.gvHeader.OptionsCustomization.AllowFilter = false;
            this.gvHeader.OptionsCustomization.AllowSort = false;
            this.gvHeader.OptionsDetail.EnableMasterViewMode = false;
            this.gvHeader.OptionsMenu.EnableColumnMenu = false;
            this.gvHeader.OptionsMenu.EnableFooterMenu = false;
            this.gvHeader.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvHeader.OptionsView.ShowAutoFilterRow = true;
            this.gvHeader.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHeader.OptionsView.ShowFooter = true;
            this.gvHeader.OptionsView.ShowGroupPanel = false;
            this.gvHeader.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHeader.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvHeader_FocusedRowChanged);
            this.gvHeader.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvHeader_CellValueChanged);
            this.gvHeader.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvHeader_CellValueChanging);
            this.gvHeader.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvHeader_BeforeLeaveRow);
            this.gvHeader.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Location = new System.Drawing.Point(0, 0);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Size = new System.Drawing.Size(1091, 14);
            this.lblTitleGrid.TabIndex = 113;
            this.lblTitleGrid.Text = "114_lblTareaCliente";
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
            this.grpCtrlProvider.Size = new System.Drawing.Size(1091, 21);
            this.grpCtrlProvider.TabIndex = 52;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl2.Size = new System.Drawing.Size(139, 14);
            this.labelControl2.TabIndex = 114;
            this.labelControl2.Text = "114_lblDetalleEntrega";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.gcTareas);
            this.panelControl1.Location = new System.Drawing.Point(1, 127);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1083, 107);
            this.panelControl1.TabIndex = 56;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(0, 1);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl1.Size = new System.Drawing.Size(104, 14);
            this.labelControl1.TabIndex = 116;
            this.labelControl1.Text = "33510_lblTareas";
            // 
            // gcTareas
            // 
            this.gcTareas.AllowDrop = true;
            this.gcTareas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcTareas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcTareas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcTareas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcTareas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcTareas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTareas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcTareas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcTareas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcTareas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcTareas.Location = new System.Drawing.Point(5, 15);
            this.gcTareas.LookAndFeel.SkinName = "Dark Side";
            this.gcTareas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTareas.MainView = this.gvTareas;
            this.gcTareas.Margin = new System.Windows.Forms.Padding(5);
            this.gcTareas.Name = "gcTareas";
            this.gcTareas.Size = new System.Drawing.Size(525, 85);
            this.gcTareas.TabIndex = 53;
            this.gcTareas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTareas});
            // 
            // gvTareas
            // 
            this.gvTareas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTareas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTareas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.Empty.Options.UseBackColor = true;
            this.gvTareas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTareas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTareas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvTareas.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvTareas.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvTareas.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvTareas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvTareas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTareas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTareas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTareas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTareas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTareas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTareas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTareas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.Row.Options.UseBackColor = true;
            this.gvTareas.Appearance.Row.Options.UseForeColor = true;
            this.gvTareas.Appearance.Row.Options.UseTextOptions = true;
            this.gvTareas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTareas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTareas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTareas.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gvTareas.Appearance.ViewCaption.Options.UseFont = true;
            this.gvTareas.GridControl = this.gcTareas;
            this.gvTareas.GroupFormat = "[#image]{1} {2}";
            this.gvTareas.HorzScrollStep = 50;
            this.gvTareas.Name = "gvTareas";
            this.gvTareas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvTareas.OptionsCustomization.AllowFilter = false;
            this.gvTareas.OptionsDetail.EnableMasterViewMode = false;
            this.gvTareas.OptionsMenu.EnableColumnMenu = false;
            this.gvTareas.OptionsMenu.EnableFooterMenu = false;
            this.gvTareas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvTareas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvTareas.OptionsView.ShowGroupPanel = false;
            this.gvTareas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvTareas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
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
            this.grpctrlHeader.Controls.Add(this.lblDocTercero);
            this.grpctrlHeader.Controls.Add(this.txtDocTercero);
            this.grpctrlHeader.Controls.Add(this.pnActa);
            this.grpctrlHeader.Controls.Add(this.lblFechaActa);
            this.grpctrlHeader.Controls.Add(this.txtObservacion);
            this.grpctrlHeader.Controls.Add(this.lblObservacion);
            this.grpctrlHeader.Controls.Add(this.lblRespCliente);
            this.grpctrlHeader.Controls.Add(this.txtRespCliente);
            this.grpctrlHeader.Controls.Add(this.dtFechaActa);
            this.grpctrlHeader.Controls.Add(this.ucProyecto);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(18, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(1089, 102);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // lblDocTercero
            // 
            this.lblDocTercero.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTercero.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDocTercero.Location = new System.Drawing.Point(914, 6);
            this.lblDocTercero.Margin = new System.Windows.Forms.Padding(4);
            this.lblDocTercero.Name = "lblDocTercero";
            this.lblDocTercero.Size = new System.Drawing.Size(103, 14);
            this.lblDocTercero.TabIndex = 109;
            this.lblDocTercero.Text = "114_lblDocTercero";
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Location = new System.Drawing.Point(1020, 4);
            this.txtDocTercero.MaxLength = 60;
            this.txtDocTercero.Multiline = true;
            this.txtDocTercero.Name = "txtDocTercero";
            this.txtDocTercero.Size = new System.Drawing.Size(59, 18);
            this.txtDocTercero.TabIndex = 2;
            // 
            // pnActa
            // 
            this.pnActa.Controls.Add(this.lblNroActa);
            this.pnActa.Controls.Add(this.txtNroActa);
            this.pnActa.Controls.Add(this.btnQueryDoc);
            this.pnActa.Location = new System.Drawing.Point(722, 50);
            this.pnActa.Name = "pnActa";
            this.pnActa.Size = new System.Drawing.Size(94, 45);
            this.pnActa.TabIndex = 106;
            // 
            // lblNroActa
            // 
            this.lblNroActa.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNroActa.Location = new System.Drawing.Point(7, 3);
            this.lblNroActa.Name = "lblNroActa";
            this.lblNroActa.Size = new System.Drawing.Size(79, 13);
            this.lblNroActa.TabIndex = 104;
            this.lblNroActa.Text = "Acta Actual Nro.";
            // 
            // txtNroActa
            // 
            this.txtNroActa.Location = new System.Drawing.Point(7, 18);
            this.txtNroActa.Name = "txtNroActa";
            this.txtNroActa.Size = new System.Drawing.Size(30, 20);
            this.txtNroActa.TabIndex = 1;
            this.txtNroActa.Leave += new System.EventHandler(this.txtNroActa_Leave);
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(43, 18);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(19, 20);
            this.btnQueryDoc.TabIndex = 103;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Visible = false;
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // lblFechaActa
            // 
            this.lblFechaActa.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaActa.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaActa.Location = new System.Drawing.Point(722, 6);
            this.lblFechaActa.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaActa.Name = "lblFechaActa";
            this.lblFechaActa.Size = new System.Drawing.Size(96, 14);
            this.lblFechaActa.TabIndex = 96;
            this.lblFechaActa.Text = "114_lblFechaActa";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(824, 64);
            this.txtObservacion.MaxLength = 60;
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(255, 31);
            this.txtObservacion.TabIndex = 4;
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblObservacion.Location = new System.Drawing.Point(824, 48);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(117, 14);
            this.lblObservacion.TabIndex = 99;
            this.lblObservacion.Text = "114_lblObservaciones";
            // 
            // lblRespCliente
            // 
            this.lblRespCliente.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRespCliente.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblRespCliente.Location = new System.Drawing.Point(721, 27);
            this.lblRespCliente.Margin = new System.Windows.Forms.Padding(4);
            this.lblRespCliente.Name = "lblRespCliente";
            this.lblRespCliente.Size = new System.Drawing.Size(102, 14);
            this.lblRespCliente.TabIndex = 98;
            this.lblRespCliente.Text = "114_lblRespCliente";
            // 
            // txtRespCliente
            // 
            this.txtRespCliente.Location = new System.Drawing.Point(888, 26);
            this.txtRespCliente.MaxLength = 60;
            this.txtRespCliente.Multiline = true;
            this.txtRespCliente.Name = "txtRespCliente";
            this.txtRespCliente.Size = new System.Drawing.Size(191, 18);
            this.txtRespCliente.TabIndex = 3;
            // 
            // dtFechaActa
            // 
            this.dtFechaActa.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaActa.Location = new System.Drawing.Point(825, 3);
            this.dtFechaActa.Name = "dtFechaActa";
            this.dtFechaActa.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaActa.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaActa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaActa.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaActa.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaActa.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaActa.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaActa.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaActa.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaActa.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaActa.Size = new System.Drawing.Size(83, 20);
            this.dtFechaActa.TabIndex = 1;
            // 
            // ucProyecto
            // 
            this.ucProyecto.BackColor = System.Drawing.Color.Transparent;
            this.ucProyecto.ClienteID = "";
            this.ucProyecto.DocumentoNro = null;
            this.ucProyecto.Location = new System.Drawing.Point(0, 4);
            this.ucProyecto.Name = "ucProyecto";
            this.ucProyecto.PrefijoID = "";
            this.ucProyecto.ProyectoID = "";
            this.ucProyecto.ProyectoInfo = null;
            this.ucProyecto.Size = new System.Drawing.Size(725, 103);
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
            this.editPorc.Mask.EditMask = "P4";
            this.editPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editPorc.Name = "editPorc";
            this.editPorc.NullText = "0";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
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
            // ActaEntrega
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 672);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ActaEntrega";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).EndInit();
            this.grpCtrlProvider.ResumeLayout(false);
            this.grpCtrlProvider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnActa)).EndInit();
            this.pnActa.ResumeLayout(false);
            this.pnActa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroActa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaActa.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaActa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGridCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private DevExpress.XtraGrid.GridControl gcDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        private DevExpress.XtraEditors.GroupControl grpCtrlProvider;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblTitleGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editPorc;
        protected DevExpress.XtraGrid.GridControl gcHeader;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvHeader;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit editPoPup;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editCmb;
        private UC_Proyecto ucProyecto;
        private System.Windows.Forms.TextBox txtRespCliente;
        private DevExpress.XtraEditors.LabelControl lblFechaActa;
        protected DevExpress.XtraEditors.DateEdit dtFechaActa;
        private DevExpress.XtraEditors.LabelControl lblRespCliente;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit editGridCmb;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private System.Windows.Forms.TextBox txtObservacion;
        private DevExpress.XtraEditors.LabelControl lblNroActa;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.TextEdit txtNroActa;
        private DevExpress.XtraEditors.PanelControl pnActa;
        private DevExpress.XtraEditors.LabelControl lblDocTercero;
        private System.Windows.Forms.TextBox txtDocTercero;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcTareas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTareas;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}