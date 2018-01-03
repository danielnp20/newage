using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaEjecucionPresupuestal
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
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcProyectos = new DevExpress.XtraGrid.GridControl();
            this.gvProyectos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnTarea = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcProyectos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProyectos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
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
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
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
            this.gvDetalle.GridControl = this.gcProyectos;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsDetail.EnableMasterViewMode = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDetalle_CustomColumnDisplayText);
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
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcProyectos.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcProyectos.Location = new System.Drawing.Point(0, 13);
            this.gcProyectos.LookAndFeel.SkinName = "Dark Side";
            this.gcProyectos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcProyectos.MainView = this.gvProyectos;
            this.gcProyectos.Margin = new System.Windows.Forms.Padding(5);
            this.gcProyectos.Name = "gcProyectos";
            this.gcProyectos.Size = new System.Drawing.Size(1021, 459);
            this.gcProyectos.TabIndex = 50;
            this.gcProyectos.UseEmbeddedNavigator = true;
            this.gcProyectos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvProyectos,
            this.gvDetalle});
            this.gcProyectos.FocusedViewChanged += new DevExpress.XtraGrid.ViewFocusEventHandler(this.gcProyectos_FocusedViewChanged);
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
            this.gvProyectos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvProyectos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvProyectos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
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
            this.splitGrids.Size = new System.Drawing.Size(1021, 522);
            this.splitGrids.SplitterPosition = 472;
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
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue2,
            this.editSpinPorcen,
            this.editBtnTarea,
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
            this.editValue2.Mask.EditMask = "c0";
            this.editValue2.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2.Name = "editValue2";
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.AllowMouseWheel = false;
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorcen.Name = "editSpinPorcen";
            // 
            // editBtnTarea
            // 
            this.editBtnTarea.Name = "editBtnTarea";
            // 
            // editValue2Cant
            // 
            this.editValue2Cant.AllowMouseWheel = false;
            this.editValue2Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.Mask.EditMask = "n0";
            this.editValue2Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2Cant.Name = "editValue2Cant";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // ConsultaEjecucionPresupuestal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 602);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ConsultaEjecucionPresupuestal";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcProyectos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProyectos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnTarea)).EndInit();
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
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnTarea;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorcen;

        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Panel pnlGrids;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;

        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblDate;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
    }
}