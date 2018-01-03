using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaProyectoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsultaProyectoForm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gcHeader = new DevExpress.XtraGrid.GridControl();
            this.gvHeader = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.groupProy = new DevExpress.XtraEditors.GroupControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoEdit();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoEdit();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.lblTitleGrid = new DevExpress.XtraEditors.LabelControl();
            this.grpCtrlProvider = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).BeginInit();
            this.groupProy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).BeginInit();
            this.grpCtrlProvider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.SuspendLayout();
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
            this.gcHeader.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(6, 6)});
            this.gcHeader.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcHeader.Location = new System.Drawing.Point(0, 13);
            this.gcHeader.LookAndFeel.SkinName = "Dark Side";
            this.gcHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcHeader.MainView = this.gvHeader;
            this.gcHeader.Margin = new System.Windows.Forms.Padding(5);
            this.gcHeader.Name = "gcHeader";
            this.gcHeader.Size = new System.Drawing.Size(963, 182);
            this.gcHeader.TabIndex = 50;
            this.gcHeader.UseEmbeddedNavigator = true;
            this.gcHeader.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHeader});
            // 
            // gvHeader
            // 
            this.gvHeader.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvHeader.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvHeader.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvHeader.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvHeader.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvHeader.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvHeader.Appearance.Row.Options.UseBackColor = true;
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
            this.gvHeader.HorzScrollStep = 50;
            this.gvHeader.Name = "gvHeader";
            this.gvHeader.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvHeader.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvHeader.OptionsCustomization.AllowColumnMoving = false;
            this.gvHeader.OptionsCustomization.AllowFilter = false;
            this.gvHeader.OptionsCustomization.AllowSort = false;
            this.gvHeader.OptionsDetail.EnableMasterViewMode = false;
            this.gvHeader.OptionsMenu.EnableColumnMenu = false;
            this.gvHeader.OptionsMenu.EnableFooterMenu = false;
            this.gvHeader.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvHeader.OptionsView.ColumnAutoWidth = false;
            this.gvHeader.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHeader.OptionsView.ShowGroupPanel = false;
            this.gvHeader.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHeader.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDocument_RowCellStyle);
            this.gvHeader.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvDocument_RowStyle);
            this.gvHeader.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvHeader.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcDetalle
            // 
            this.gcDetalle.AllowDrop = true;
            this.gcDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.gcDetalle.Location = new System.Drawing.Point(0, 17);
            this.gcDetalle.LookAndFeel.SkinName = "Dark Side";
            this.gcDetalle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetalle.MainView = this.gvDetalle;
            this.gcDetalle.Margin = new System.Windows.Forms.Padding(5);
            this.gcDetalle.Name = "gcDetalle";
            this.gcDetalle.Size = new System.Drawing.Size(963, 230);
            this.gcDetalle.TabIndex = 51;
            this.gcDetalle.UseEmbeddedNavigator = true;
            this.gcDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
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
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.VertLine.Options.UseBackColor = true;
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
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvDocument_RowCellStyle);
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvRecurso_CustomColumnDisplayText);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1022, 602);
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
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.37834F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 78.62166F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1018, 598);
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
            this.grpctrlHeader.Controls.Add(this.groupProy);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(17, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(963, 117);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // groupProy
            // 
            this.groupProy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupProy.AppearanceCaption.Options.UseFont = true;
            this.groupProy.Controls.Add(this.masterCliente);
            this.groupProy.Controls.Add(this.lblLicitacion);
            this.groupProy.Controls.Add(this.masterProyecto);
            this.groupProy.Controls.Add(this.masterPrefijo);
            this.groupProy.Controls.Add(this.btnQueryDoc);
            this.groupProy.Controls.Add(this.lblNro);
            this.groupProy.Controls.Add(this.txtNro);
            this.groupProy.Controls.Add(this.lblDescripcion);
            this.groupProy.Controls.Add(this.txtLicitacion);
            this.groupProy.Controls.Add(this.txtDescripcion);
            this.groupProy.Location = new System.Drawing.Point(4, 6);
            this.groupProy.Margin = new System.Windows.Forms.Padding(2);
            this.groupProy.Name = "groupProy";
            this.groupProy.Size = new System.Drawing.Size(763, 98);
            this.groupProy.TabIndex = 112;
            this.groupProy.Text = "Filtrar Proyecto";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(8, 70);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(307, 21);
            this.masterCliente.TabIndex = 113;
            this.masterCliente.Value = "";
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(8, 50);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(77, 13);
            this.lblLicitacion.TabIndex = 112;
            this.lblLicitacion.Text = "113_lblLicitacion";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(10, 20);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(281, 21);
            this.masterProyecto.TabIndex = 110;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.masterProyecto_Leave);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(331, 21);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(262, 21);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(691, 23);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 3;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(599, 26);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(51, 13);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "113_lblNro";
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(655, 24);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(34, 20);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(330, 50);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(88, 13);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "113_lblDescripcion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(109, 46);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.ReadOnly = true;
            this.txtLicitacion.Size = new System.Drawing.Size(195, 23);
            this.txtLicitacion.TabIndex = 111;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(430, 48);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(313, 37);
            this.txtDescripcion.TabIndex = 16;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(17, 579);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(963, 16);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.AutoSize = true;
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Size = new System.Drawing.Size(963, 16);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(17, 126);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(963, 447);
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
            this.splitGrids.Panel2.Controls.Add(this.gcDetalle);
            this.splitGrids.Panel2.Controls.Add(this.grpCtrlProvider);
            this.splitGrids.Size = new System.Drawing.Size(963, 447);
            this.splitGrids.SplitterPosition = 195;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // lblTitleGrid
            // 
            this.lblTitleGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleGrid.Location = new System.Drawing.Point(0, 0);
            this.lblTitleGrid.Name = "lblTitleGrid";
            this.lblTitleGrid.Size = new System.Drawing.Size(127, 13);
            this.lblTitleGrid.TabIndex = 113;
            this.lblTitleGrid.Text = "33509_lblTareaCliente";
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
            this.grpCtrlProvider.Size = new System.Drawing.Size(963, 17);
            this.grpCtrlProvider.TabIndex = 52;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(89, 13);
            this.labelControl2.TabIndex = 114;
            this.labelControl2.Text = "33509_lblItems";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue2,
            this.editBtnGrid,
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
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
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
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // ConsultaProyectoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 602);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "ConsultaProyectoForm";
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gcHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).EndInit();
            this.groupProy.ResumeLayout(false);
            this.groupProy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlProvider)).EndInit();
            this.grpCtrlProvider.ResumeLayout(false);
            this.grpCtrlProvider.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.GroupBox grpboxDetail;
        private DevExpress.XtraGrid.GridControl gcHeader;
        private DevExpress.XtraGrid.Views.Grid.GridView gvHeader;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
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
        private DevExpress.XtraEditors.GroupControl groupProy;
        private uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private uc_MasterFind masterProyecto;
        private uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.MemoEdit txtLicitacion;
        private DevExpress.XtraEditors.MemoEdit txtDescripcion;
        private DevExpress.XtraEditors.LabelControl lblTitleGrid;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}