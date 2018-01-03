using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalTareasComite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalTareasComite));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcModificaciones = new DevExpress.XtraGrid.GridControl();
            this.gvModificaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.groupProy = new DevExpress.XtraEditors.GroupControl();
            this.btnAprobar = new DevExpress.XtraEditors.SimpleButton();
            this.btnGuardar = new DevExpress.XtraEditors.SimpleButton();
            this.dtFechaNueva = new DevExpress.XtraEditors.DateEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.dtEntrFechaActual = new DevExpress.XtraEditors.DateEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtNroDoc = new DevExpress.XtraEditors.TextEdit();
            this.dtSolFechaActual = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dtTrabFechaActual = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.masterJustifica = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTarea = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.txtNroVersion = new DevExpress.XtraEditors.TextEdit();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoEdit();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.splitGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcModificaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvModificaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).BeginInit();
            this.groupProy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaNueva.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaNueva.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEntrFechaActual.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEntrFechaActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSolFechaActual.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSolFechaActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTrabFechaActual.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTrabFechaActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroVersion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
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
            this.gvDetalle.GridControl = this.gcModificaciones;
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
            // gcModificaciones
            // 
            this.gcModificaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcModificaciones.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcModificaciones.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcModificaciones.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcModificaciones.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcModificaciones.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcModificaciones.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcModificaciones.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcModificaciones.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcModificaciones.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcModificaciones.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcModificaciones.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcModificaciones.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcModificaciones.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcModificaciones.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.RelationName = "Detalle";
            this.gcModificaciones.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcModificaciones.Location = new System.Drawing.Point(0, 13);
            this.gcModificaciones.LookAndFeel.SkinName = "Dark Side";
            this.gcModificaciones.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcModificaciones.MainView = this.gvModificaciones;
            this.gcModificaciones.Margin = new System.Windows.Forms.Padding(5);
            this.gcModificaciones.Name = "gcModificaciones";
            this.gcModificaciones.Size = new System.Drawing.Size(857, 172);
            this.gcModificaciones.TabIndex = 50;
            this.gcModificaciones.UseEmbeddedNavigator = true;
            this.gcModificaciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvModificaciones,
            this.gvDetalle});
            // 
            // gvModificaciones
            // 
            this.gvModificaciones.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvModificaciones.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvModificaciones.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvModificaciones.Appearance.Empty.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvModificaciones.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModificaciones.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvModificaciones.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModificaciones.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvModificaciones.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvModificaciones.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvModificaciones.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvModificaciones.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvModificaciones.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvModificaciones.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvModificaciones.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvModificaciones.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvModificaciones.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvModificaciones.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvModificaciones.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModificaciones.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvModificaciones.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvModificaciones.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvModificaciones.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvModificaciones.Appearance.Row.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.Row.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.Row.Options.UseTextOptions = true;
            this.gvModificaciones.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvModificaciones.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModificaciones.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvModificaciones.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvModificaciones.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvModificaciones.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvModificaciones.Appearance.VertLine.Options.UseBackColor = true;
            this.gvModificaciones.GridControl = this.gcModificaciones;
            this.gvModificaciones.HorzScrollStep = 50;
            this.gvModificaciones.Name = "gvModificaciones";
            this.gvModificaciones.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvModificaciones.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvModificaciones.OptionsCustomization.AllowColumnMoving = false;
            this.gvModificaciones.OptionsCustomization.AllowFilter = false;
            this.gvModificaciones.OptionsCustomization.AllowSort = false;
            this.gvModificaciones.OptionsDetail.EnableMasterViewMode = false;
            this.gvModificaciones.OptionsMenu.EnableColumnMenu = false;
            this.gvModificaciones.OptionsMenu.EnableFooterMenu = false;
            this.gvModificaciones.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvModificaciones.OptionsView.ColumnAutoWidth = false;
            this.gvModificaciones.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvModificaciones.OptionsView.ShowGroupPanel = false;
            this.gvModificaciones.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvModificaciones.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvModificaciones.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(891, 448);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.36782F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.63218F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(887, 444);
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
            this.grpctrlHeader.Size = new System.Drawing.Size(857, 187);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // groupProy
            // 
            this.groupProy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupProy.AppearanceCaption.Options.UseFont = true;
            this.groupProy.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.groupProy.Controls.Add(this.masterTarea);
            this.groupProy.Controls.Add(this.btnAprobar);
            this.groupProy.Controls.Add(this.btnGuardar);
            this.groupProy.Controls.Add(this.dtFechaNueva);
            this.groupProy.Controls.Add(this.labelControl10);
            this.groupProy.Controls.Add(this.radioGroup1);
            this.groupProy.Controls.Add(this.labelControl7);
            this.groupProy.Controls.Add(this.labelControl8);
            this.groupProy.Controls.Add(this.labelControl9);
            this.groupProy.Controls.Add(this.dtEntrFechaActual);
            this.groupProy.Controls.Add(this.labelControl6);
            this.groupProy.Controls.Add(this.labelControl5);
            this.groupProy.Controls.Add(this.txtNroDoc);
            this.groupProy.Controls.Add(this.dtSolFechaActual);
            this.groupProy.Controls.Add(this.labelControl3);
            this.groupProy.Controls.Add(this.dtTrabFechaActual);
            this.groupProy.Controls.Add(this.labelControl2);
            this.groupProy.Controls.Add(this.masterJustifica);
            this.groupProy.Controls.Add(this.masterProyecto);
            this.groupProy.Controls.Add(this.lblNro);
            this.groupProy.Controls.Add(this.txtNroVersion);
            this.groupProy.Controls.Add(this.lblDescripcion);
            this.groupProy.Controls.Add(this.txtDescripcion);
            this.groupProy.Location = new System.Drawing.Point(4, 2);
            this.groupProy.Margin = new System.Windows.Forms.Padding(2);
            this.groupProy.Name = "groupProy";
            this.groupProy.Size = new System.Drawing.Size(1007, 186);
            this.groupProy.TabIndex = 112;
            this.groupProy.Tag = "";
            this.groupProy.Text = "Modificación Tarea";
            // 
            // btnAprobar
            // 
            this.btnAprobar.Appearance.Options.UseFont = true;
            this.btnAprobar.Image = ((System.Drawing.Image)(resources.GetObject("btnAprobar.Image")));
            this.btnAprobar.Location = new System.Drawing.Point(754, 158);
            this.btnAprobar.Name = "btnAprobar";
            this.btnAprobar.Size = new System.Drawing.Size(85, 21);
            this.btnAprobar.TabIndex = 134;
            this.btnAprobar.Text = "Aprobar";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Appearance.Options.UseFont = true;
            this.btnGuardar.Image = ((System.Drawing.Image)(resources.GetObject("btnGuardar.Image")));
            this.btnGuardar.Location = new System.Drawing.Point(754, 135);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(85, 21);
            this.btnGuardar.TabIndex = 133;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // dtFechaNueva
            // 
            this.dtFechaNueva.EditValue = null;
            this.dtFechaNueva.Location = new System.Drawing.Point(425, 71);
            this.dtFechaNueva.Name = "dtFechaNueva";
            this.dtFechaNueva.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaNueva.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaNueva.Size = new System.Drawing.Size(100, 20);
            this.dtFechaNueva.TabIndex = 131;
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl10.Location = new System.Drawing.Point(335, 75);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(63, 13);
            this.labelControl10.TabIndex = 130;
            this.labelControl10.Text = "Fecha Nueva";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(301, 45);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.AllowMouseWheel = false;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Columns = 1;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem()});
            this.radioGroup1.Size = new System.Drawing.Size(24, 85);
            this.radioGroup1.TabIndex = 129;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Location = new System.Drawing.Point(16, 101);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 128;
            this.labelControl7.Text = "Entrega";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Location = new System.Drawing.Point(16, 54);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(53, 14);
            this.labelControl8.TabIndex = 127;
            this.labelControl8.Text = "Solicitud";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl9.Location = new System.Drawing.Point(16, 78);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(46, 14);
            this.labelControl9.TabIndex = 126;
            this.labelControl9.Text = "Trabajo";
            // 
            // dtEntrFechaActual
            // 
            this.dtEntrFechaActual.EditValue = null;
            this.dtEntrFechaActual.Location = new System.Drawing.Point(182, 101);
            this.dtEntrFechaActual.Name = "dtEntrFechaActual";
            this.dtEntrFechaActual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEntrFechaActual.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEntrFechaActual.Size = new System.Drawing.Size(113, 20);
            this.dtEntrFechaActual.TabIndex = 125;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl6.Location = new System.Drawing.Point(110, 104);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(62, 13);
            this.labelControl6.TabIndex = 124;
            this.labelControl6.Text = "Fecha Actual";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl5.Location = new System.Drawing.Point(895, 5);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(58, 13);
            this.labelControl5.TabIndex = 123;
            this.labelControl5.Text = "Numero Doc";
            this.labelControl5.Visible = false;
            // 
            // txtNroDoc
            // 
            this.txtNroDoc.Location = new System.Drawing.Point(973, 1);
            this.txtNroDoc.Name = "txtNroDoc";
            this.txtNroDoc.Size = new System.Drawing.Size(26, 20);
            this.txtNroDoc.TabIndex = 122;
            this.txtNroDoc.Visible = false;
            // 
            // dtSolFechaActual
            // 
            this.dtSolFechaActual.EditValue = null;
            this.dtSolFechaActual.Location = new System.Drawing.Point(182, 52);
            this.dtSolFechaActual.Name = "dtSolFechaActual";
            this.dtSolFechaActual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtSolFechaActual.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtSolFechaActual.Size = new System.Drawing.Size(113, 20);
            this.dtSolFechaActual.TabIndex = 119;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl3.Location = new System.Drawing.Point(109, 55);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(62, 13);
            this.labelControl3.TabIndex = 118;
            this.labelControl3.Text = "Fecha Actual";
            // 
            // dtTrabFechaActual
            // 
            this.dtTrabFechaActual.EditValue = null;
            this.dtTrabFechaActual.Location = new System.Drawing.Point(182, 77);
            this.dtTrabFechaActual.Name = "dtTrabFechaActual";
            this.dtTrabFechaActual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTrabFechaActual.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTrabFechaActual.Size = new System.Drawing.Size(113, 20);
            this.dtTrabFechaActual.TabIndex = 117;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl2.Location = new System.Drawing.Point(111, 81);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(62, 13);
            this.labelControl2.TabIndex = 115;
            this.labelControl2.Text = "Fecha Actual";
            // 
            // masterJustifica
            // 
            this.masterJustifica.BackColor = System.Drawing.Color.Transparent;
            this.masterJustifica.Filtros = null;
            this.masterJustifica.Location = new System.Drawing.Point(533, 69);
            this.masterJustifica.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterJustifica.Name = "masterJustifica";
            this.masterJustifica.Size = new System.Drawing.Size(307, 25);
            this.masterJustifica.TabIndex = 113;
            this.masterJustifica.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(10, 21);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(352, 23);
            this.masterProyecto.TabIndex = 110;
            this.masterProyecto.Value = "";
            // 
            // masterTarea
            // 
            this.masterTarea.BackColor = System.Drawing.Color.Transparent;
            this.masterTarea.Filtros = null;
            this.masterTarea.Location = new System.Drawing.Point(339, 21);
            this.masterTarea.Margin = new System.Windows.Forms.Padding(4);
            this.masterTarea.Name = "masterTarea";
            this.masterTarea.Size = new System.Drawing.Size(299, 23);
            this.masterTarea.TabIndex = 1;
            this.masterTarea.Value = "";
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(666, 29);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(59, 13);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "110_Version";
            // 
            // txtNroVersion
            // 
            this.txtNroVersion.Location = new System.Drawing.Point(729, 25);
            this.txtNroVersion.Name = "txtNroVersion";
            this.txtNroVersion.Size = new System.Drawing.Size(34, 20);
            this.txtNroVersion.TabIndex = 2;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(10, 147);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(96, 13);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "33314_Observacion";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(111, 134);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(637, 45);
            this.txtDescripcion.TabIndex = 16;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.splitGrids);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(17, 196);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(857, 236);
            this.pnlGrids.TabIndex = 113;
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Horizontal = false;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Margin = new System.Windows.Forms.Padding(2);
            this.splitGrids.Name = "splitGrids";
            this.splitGrids.Panel1.Controls.Add(this.gcModificaciones);
            this.splitGrids.Panel1.Controls.Add(this.labelControl1);
            this.splitGrids.Panel1.Text = "Panel1";
            this.splitGrids.Panel2.AutoScroll = true;
            this.splitGrids.Panel2.Controls.Add(this.btnAceptar);
            this.splitGrids.Panel2.Controls.Add(this.btnCancelar);
            this.splitGrids.Panel2.Controls.Add(this.labelControl4);
            this.splitGrids.Panel2.Controls.Add(this.memoEdit1);
            this.splitGrids.Size = new System.Drawing.Size(857, 236);
            this.splitGrids.SplitterPosition = 185;
            this.splitGrids.TabIndex = 55;
            this.splitGrids.Text = "splitContainerControl1";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(83, 13);
            this.labelControl1.TabIndex = 113;
            this.labelControl1.Text = "Modificaciones";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(703, 8);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(59, 23);
            this.btnAceptar.TabIndex = 128;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(768, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 127;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.labelControl4.Location = new System.Drawing.Point(14, 8);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(100, 13);
            this.labelControl4.TabIndex = 123;
            this.labelControl4.Text = "1015_lblObservacion";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(115, 6);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.ReadOnly = true;
            this.memoEdit1.Size = new System.Drawing.Size(469, 35);
            this.memoEdit1.TabIndex = 122;
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
            // 
            // ModalTareasComite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 448);
            this.Controls.Add(this.pnlMainContainer);
            this.MaximizeBox = false;
            this.Name = "ModalTareasComite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "33509";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcModificaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvModificaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.tlSeparatorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).EndInit();
            this.groupProy.ResumeLayout(false);
            this.groupProy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaNueva.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaNueva.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEntrFechaActual.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEntrFechaActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSolFechaActual.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtSolFechaActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTrabFechaActual.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTrabFechaActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroVersion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private DevExpress.XtraGrid.GridControl gcModificaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView gvModificaciones;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        private System.Windows.Forms.Panel pnlGrids;
        private System.ComponentModel.IContainer components;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;

        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.SplitContainerControl splitGrids;
        private DevExpress.XtraEditors.GroupControl groupProy;
        private uc_MasterFind masterJustifica;
        private uc_MasterFind masterProyecto;
        private uc_MasterFind masterTarea;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.TextEdit txtNroVersion;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.MemoEdit txtDescripcion;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dtTrabFechaActual;
        private DevExpress.XtraEditors.DateEdit dtSolFechaActual;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.TextEdit txtNroDoc;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit dtEntrFechaActual;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.DateEdit dtFechaNueva;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton btnAprobar;
        private DevExpress.XtraEditors.SimpleButton btnGuardar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}