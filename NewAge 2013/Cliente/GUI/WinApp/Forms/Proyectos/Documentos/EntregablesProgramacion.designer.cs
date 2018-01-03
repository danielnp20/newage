using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class EntregablesProgramacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntregablesProgramacion));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gcDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.btnLoadFechasProgr = new DevExpress.XtraEditors.SimpleButton();
            this.ucProyecto = new NewAge.Cliente.GUI.WinApp.Forms.UC_Proyecto();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcHeader = new DevExpress.XtraGrid.GridControl();
            this.gvHeader = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblTitleGrid = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcTareas = new DevExpress.XtraGrid.GridControl();
            this.gvTareas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpCtrlProvider = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editPoPup = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).BeginInit();
            this.grpCtrlProvider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDetalle
            // 
            this.gcDetalle.AllowDrop = true;
            this.gcDetalle.Dock = System.Windows.Forms.DockStyle.Left;
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
            this.gcDetalle.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcDetalle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcDetalle.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetalle.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDetalle_EmbeddedNavigator_ButtonClick);
            this.gcDetalle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDetalle.Location = new System.Drawing.Point(0, 22);
            this.gcDetalle.LookAndFeel.SkinName = "Dark Side";
            this.gcDetalle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetalle.MainView = this.gvDetalle;
            this.gcDetalle.Margin = new System.Windows.Forms.Padding(5);
            this.gcDetalle.Name = "gcDetalle";
            this.gcDetalle.Size = new System.Drawing.Size(713, 181);
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
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
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
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 7.9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDetalle.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.8F);
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
            this.gvDetalle.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7F);
            this.gvDetalle.Appearance.ViewCaption.Options.UseFont = true;
            this.gvDetalle.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.ViewCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
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
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1144, 602);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.9966F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.0034F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1140, 598);
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
            this.grpctrlHeader.Controls.Add(this.btnLoadFechasProgr);
            this.grpctrlHeader.Controls.Add(this.ucProyecto);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(18, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(1085, 97);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // btnLoadFechasProgr
            // 
            this.btnLoadFechasProgr.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLoadFechasProgr.Appearance.Options.UseFont = true;
            this.btnLoadFechasProgr.Location = new System.Drawing.Point(735, 69);
            this.btnLoadFechasProgr.Name = "btnLoadFechasProgr";
            this.btnLoadFechasProgr.Size = new System.Drawing.Size(138, 23);
            this.btnLoadFechasProgr.TabIndex = 115;
            this.btnLoadFechasProgr.Text = "Cargar Fechas Autom";
            this.btnLoadFechasProgr.Click += new System.EventHandler(this.btnLoadFechasProgr_Click);
            // 
            // ucProyecto
            // 
            this.ucProyecto.BackColor = System.Drawing.Color.Transparent;
            this.ucProyecto.ClienteID = "";
            this.ucProyecto.DocumentoNro = null;
            this.ucProyecto.Location = new System.Drawing.Point(5, 3);
            this.ucProyecto.Name = "ucProyecto";
            this.ucProyecto.PrefijoID = "";
            this.ucProyecto.ProyectoID = "";
            this.ucProyecto.ProyectoInfo = null;
            this.ucProyecto.Size = new System.Drawing.Size(724, 98);
            this.ucProyecto.TabIndex = 114;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(18, 578);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1085, 17);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.AutoSize = true;
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Size = new System.Drawing.Size(1085, 17);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(18, 106);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1085, 466);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcHeader);
            this.splitGrids.Panel1.Controls.Add(this.lblTitleGrid);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.panelControl1);
            this.splitGrids.Panel2.Controls.Add(this.gcDetalle);
            this.splitGrids.Panel2.Controls.Add(this.grpCtrlProvider);
            this.splitGrids.Size = new System.Drawing.Size(1085, 466);
            this.splitGrids.SplitterPosition = 258;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // gcHeader
            // 
            this.gcHeader.Dock = System.Windows.Forms.DockStyle.Top;
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
            this.gcHeader.Location = new System.Drawing.Point(0, 19);
            this.gcHeader.LookAndFeel.SkinName = "Dark Side";
            this.gcHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcHeader.MainView = this.gvHeader;
            this.gcHeader.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.Name = "gcHeader";
            this.gcHeader.Size = new System.Drawing.Size(1085, 226);
            this.gcHeader.TabIndex = 114;
            this.gcHeader.UseEmbeddedNavigator = true;
            this.gcHeader.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHeader,
            this.gridView1});
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
            this.gvHeader.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvHeader.Appearance.ViewCaption.Options.UseFont = true;
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
            this.gvHeader.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHeader.OptionsView.ShowGroupPanel = false;
            this.gvHeader.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHeader.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvHeader.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcHeader;
            this.gridView1.Name = "gridView1";
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Location = new System.Drawing.Point(0, 0);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Size = new System.Drawing.Size(1085, 19);
            this.lblTitleGrid.TabIndex = 113;
            this.lblTitleGrid.Text = "33510_lblTareaCliente";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcTareas);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(713, 22);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(365, 181);
            this.panelControl1.TabIndex = 54;
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
            this.gcTareas.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDetalle_EmbeddedNavigator_ButtonClick);
            this.gcTareas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcTareas.Location = new System.Drawing.Point(10, 1);
            this.gcTareas.LookAndFeel.SkinName = "Dark Side";
            this.gcTareas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTareas.MainView = this.gvTareas;
            this.gcTareas.Margin = new System.Windows.Forms.Padding(5);
            this.gcTareas.Name = "gcTareas";
            this.gcTareas.Size = new System.Drawing.Size(347, 178);
            this.gcTareas.TabIndex = 53;
            this.gcTareas.UseEmbeddedNavigator = true;
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
            this.gvTareas.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTareas_FocusedRowChanged);
            this.gvTareas.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvTareas_CellValueChanging);
            this.gvTareas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // grpCtrlProvider
            // 
            this.grpCtrlProvider.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F);
            this.grpCtrlProvider.AppearanceCaption.Options.UseFont = true;
            this.grpCtrlProvider.Controls.Add(this.labelControl1);
            this.grpCtrlProvider.Controls.Add(this.labelControl2);
            this.grpCtrlProvider.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCtrlProvider.Location = new System.Drawing.Point(0, 0);
            this.grpCtrlProvider.Margin = new System.Windows.Forms.Padding(2);
            this.grpCtrlProvider.Name = "grpCtrlProvider";
            this.grpCtrlProvider.ShowCaption = false;
            this.grpCtrlProvider.Size = new System.Drawing.Size(1085, 22);
            this.grpCtrlProvider.TabIndex = 52;
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(731, 3);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl1.Size = new System.Drawing.Size(104, 14);
            this.labelControl1.TabIndex = 115;
            this.labelControl1.Text = "33510_lblTareas";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelControl2.Size = new System.Drawing.Size(153, 14);
            this.labelControl2.TabIndex = 114;
            this.labelControl2.Text = "33510_lblFechasEntrega";
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
            this.editCheck});
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
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
            // editCheck
            // 
            this.editCheck.Name = "editCheck";
            // 
            // EntregablesProgramacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 602);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "EntregablesProgramacion";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).EndInit();
            this.grpCtrlProvider.ResumeLayout(false);
            this.grpCtrlProvider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPoPup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.GroupBox grpboxDetail;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;        
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlGrids;
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
        protected DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit editPoPup;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editCmb;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private UC_Proyecto ucProyecto;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcTareas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTareas;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnLoadFechasProgr;
    }
}