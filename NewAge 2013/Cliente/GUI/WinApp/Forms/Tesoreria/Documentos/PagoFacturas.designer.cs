using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PagoFacturas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagoFacturas));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvSubDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvDetallePagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNroEgreso = new System.Windows.Forms.TextBox();
            this.lblNroEgreso = new System.Windows.Forms.Label();
            this.txtChequeFin = new System.Windows.Forms.TextBox();
            this.txtChequeIni = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gcDetalleFacturas = new DevExpress.XtraGrid.GridControl();
            this.gvDetalleFacturas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gctrlGrid = new DevExpress.XtraEditors.GroupControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetallePagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).BeginInit();
            this.gctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalleFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).BeginInit();
            this.gctrlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            this.SuspendLayout();
            // 
            // gvSubDetalle
            // 
            this.gvSubDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSubDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvSubDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvSubDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvSubDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSubDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvSubDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSubDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvSubDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvSubDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvSubDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvSubDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvSubDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvSubDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvSubDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvSubDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvSubDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvSubDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvSubDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSubDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvSubDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSubDetalle.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvSubDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvSubDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvSubDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvSubDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvSubDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvSubDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvSubDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvSubDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvSubDetalle.Appearance.VertLine.Options.UseBackColor = true;
            this.gvSubDetalle.GridControl = this.gcPagos;
            this.gvSubDetalle.HorzScrollStep = 50;
            this.gvSubDetalle.Name = "gvSubDetalle";
            this.gvSubDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvSubDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvSubDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvSubDetalle.OptionsCustomization.AllowFilter = false;
            this.gvSubDetalle.OptionsCustomization.AllowSort = false;
            this.gvSubDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvSubDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvSubDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvSubDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvSubDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvSubDetalle.OptionsView.ShowGroupPanel = false;
            this.gvSubDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvSubDetalle.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvSubDetalle_CustomRowCellEdit);
            this.gvSubDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSubDetalle_CustomUnboundColumnData);
            // 
            // gcPagos
            // 
            this.gcPagos.AllowDrop = true;
            this.gcPagos.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcPagos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPagos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcPagos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPagos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcPagos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcPagos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcPagos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPagos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvSubDetalle;
            gridLevelNode1.RelationName = "DetallesFacturas";
            this.gcPagos.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcPagos.Location = new System.Drawing.Point(2, 2);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvDetallePagos;
            this.gcPagos.Margin = new System.Windows.Forms.Padding(4);
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(949, 229);
            this.gcPagos.TabIndex = 51;
            this.gcPagos.UseEmbeddedNavigator = true;
            this.gcPagos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetallePagos,
            this.gvSubDetalle});
            // 
            // gvDetallePagos
            // 
            this.gvDetallePagos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetallePagos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetallePagos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetallePagos.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetallePagos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetallePagos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetallePagos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetallePagos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetallePagos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetallePagos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetallePagos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetallePagos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetallePagos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetallePagos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetallePagos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetallePagos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetallePagos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetallePagos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetallePagos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetallePagos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetallePagos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetallePagos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetallePagos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetallePagos.Appearance.Row.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.Row.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetallePagos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetallePagos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetallePagos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetallePagos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetallePagos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetallePagos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetallePagos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetallePagos.GridControl = this.gcPagos;
            this.gvDetallePagos.HorzScrollStep = 50;
            this.gvDetallePagos.Name = "gvDetallePagos";
            this.gvDetallePagos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetallePagos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetallePagos.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetallePagos.OptionsCustomization.AllowFilter = false;
            this.gvDetallePagos.OptionsCustomization.AllowSort = false;
            this.gvDetallePagos.OptionsMenu.EnableColumnMenu = false;
            this.gvDetallePagos.OptionsMenu.EnableFooterMenu = false;
            this.gvDetallePagos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetallePagos.OptionsView.ColumnAutoWidth = false;
            this.gvDetallePagos.OptionsView.ShowAutoFilterRow = true;
            this.gvDetallePagos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetallePagos.OptionsView.ShowGroupPanel = false;
            this.gvDetallePagos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetallePagos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvPagos_CustomRowCellEdit);
            this.gvDetallePagos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPagos_FocusedRowChanged);
            this.gvDetallePagos.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvPagos_CellValueChanging);
            this.gvDetallePagos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvPagos_CustomUnboundColumnData);
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1022, 603);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.453958F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.54604F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1018, 599);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(13, 581);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(994, 15);
            this.pnlDetail.TabIndex = 112;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gctrlHeader);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(13, 11);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(994, 564);
            this.pnlGrids.TabIndex = 113;
            // 
            // gctrlHeader
            // 
            this.gctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlHeader.Appearance.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.gctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.gctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gctrlHeader.Controls.Add(this.dtPeriod);
            this.gctrlHeader.Controls.Add(this.lblPeriod);
            this.gctrlHeader.Controls.Add(this.dtFecha);
            this.gctrlHeader.Controls.Add(this.label5);
            this.gctrlHeader.Controls.Add(this.txtNroEgreso);
            this.gctrlHeader.Controls.Add(this.lblNroEgreso);
            this.gctrlHeader.Controls.Add(this.txtChequeFin);
            this.gctrlHeader.Controls.Add(this.txtChequeIni);
            this.gctrlHeader.Controls.Add(this.label2);
            this.gctrlHeader.Controls.Add(this.label1);
            this.gctrlHeader.Controls.Add(this.chkSelectAll);
            this.gctrlHeader.Controls.Add(this.groupControl1);
            this.gctrlHeader.Controls.Add(this.masterCuenta);
            this.gctrlHeader.Controls.Add(this.gctrlGrid);
            this.gctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlHeader.Location = new System.Drawing.Point(0, 0);
            this.gctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlHeader.Name = "gctrlHeader";
            this.gctrlHeader.Size = new System.Drawing.Size(994, 564);
            this.gctrlHeader.TabIndex = 0;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.Enabled = false;
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(645, 32);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 123;
            this.dtPeriod.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.dtPeriod_EditValueChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(583, 35);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(80, 14);
            this.lblPeriod.TabIndex = 124;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(854, 32);
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
            this.dtFecha.TabIndex = 122;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(784, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 121;
            this.label5.Text = "31_lblFecha";
            // 
            // txtNroEgreso
            // 
            this.txtNroEgreso.Enabled = false;
            this.txtNroEgreso.Location = new System.Drawing.Point(428, 27);
            this.txtNroEgreso.Name = "txtNroEgreso";
            this.txtNroEgreso.Size = new System.Drawing.Size(32, 21);
            this.txtNroEgreso.TabIndex = 119;
            this.txtNroEgreso.Text = "0";
            // 
            // lblNroEgreso
            // 
            this.lblNroEgreso.AutoSize = true;
            this.lblNroEgreso.BackColor = System.Drawing.Color.Transparent;
            this.lblNroEgreso.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroEgreso.Location = new System.Drawing.Point(339, 30);
            this.lblNroEgreso.Name = "lblNroEgreso";
            this.lblNroEgreso.Size = new System.Drawing.Size(95, 14);
            this.lblNroEgreso.TabIndex = 117;
            this.lblNroEgreso.Text = "31_lblNroEgreso";
            // 
            // txtChequeFin
            // 
            this.txtChequeFin.Enabled = false;
            this.txtChequeFin.Location = new System.Drawing.Point(130, 77);
            this.txtChequeFin.Name = "txtChequeFin";
            this.txtChequeFin.Size = new System.Drawing.Size(68, 21);
            this.txtChequeFin.TabIndex = 116;
            this.txtChequeFin.Text = "0";
            // 
            // txtChequeIni
            // 
            this.txtChequeIni.Enabled = false;
            this.txtChequeIni.Location = new System.Drawing.Point(130, 53);
            this.txtChequeIni.Name = "txtChequeIni";
            this.txtChequeIni.Size = new System.Drawing.Size(68, 21);
            this.txtChequeIni.TabIndex = 115;
            this.txtChequeIni.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 14);
            this.label2.TabIndex = 114;
            this.label2.Text = "31_NumChequeFin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 14);
            this.label1.TabIndex = 113;
            this.label1.Text = "31_NumChequeIni";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectAll.Enabled = false;
            this.chkSelectAll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.ForeColor = System.Drawing.Color.Black;
            this.chkSelectAll.Location = new System.Drawing.Point(859, 81);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(112, 18);
            this.chkSelectAll.TabIndex = 109;
            this.chkSelectAll.Text = "31_chkSelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkSelectAll_MouseClick);
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.groupControl1.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Appearance.Options.UseBorderColor = true;
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.groupControl1.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.groupControl1.Controls.Add(this.gcDetalleFacturas);
            this.groupControl1.Location = new System.Drawing.Point(18, 346);
            this.groupControl1.LookAndFeel.SkinName = "Seven Classic";
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(956, 213);
            this.groupControl1.TabIndex = 112;
            // 
            // gcDetalleFacturas
            // 
            this.gcDetalleFacturas.AllowDrop = true;
            this.gcDetalleFacturas.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcDetalleFacturas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetalleFacturas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetalleFacturas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetalleFacturas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetalleFacturas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetalleFacturas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetalleFacturas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetalleFacturas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetalleFacturas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetalleFacturas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcDetalleFacturas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetalleFacturas.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDetalleFacturas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetalleFacturas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDetalleFacturas.Location = new System.Drawing.Point(2, 2);
            this.gcDetalleFacturas.LookAndFeel.SkinName = "Dark Side";
            this.gcDetalleFacturas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetalleFacturas.MainView = this.gvDetalleFacturas;
            this.gcDetalleFacturas.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetalleFacturas.Name = "gcDetalleFacturas";
            this.gcDetalleFacturas.Size = new System.Drawing.Size(952, 205);
            this.gcDetalleFacturas.TabIndex = 51;
            this.gcDetalleFacturas.UseEmbeddedNavigator = true;
            this.gcDetalleFacturas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalleFacturas});
            // 
            // gvDetalleFacturas
            // 
            this.gvDetalleFacturas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleFacturas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalleFacturas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalleFacturas.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalleFacturas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalleFacturas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleFacturas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalleFacturas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleFacturas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleFacturas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalleFacturas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalleFacturas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalleFacturas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalleFacturas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalleFacturas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalleFacturas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalleFacturas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalleFacturas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalleFacturas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleFacturas.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalleFacturas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalleFacturas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalleFacturas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalleFacturas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalleFacturas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalleFacturas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalleFacturas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetalleFacturas.GridControl = this.gcDetalleFacturas;
            this.gvDetalleFacturas.HorzScrollStep = 50;
            this.gvDetalleFacturas.Name = "gvDetalleFacturas";
            this.gvDetalleFacturas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalleFacturas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalleFacturas.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalleFacturas.OptionsCustomization.AllowFilter = false;
            this.gvDetalleFacturas.OptionsCustomization.AllowSort = false;
            this.gvDetalleFacturas.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalleFacturas.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalleFacturas.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalleFacturas.OptionsView.ColumnAutoWidth = false;
            this.gvDetalleFacturas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalleFacturas.OptionsView.ShowGroupPanel = false;
            this.gvDetalleFacturas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalleFacturas.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetalleFacturas_CustomRowCellEdit);
            this.gvDetalleFacturas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalleFacturas_CustomUnboundColumnData);
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(28, 24);
            this.masterCuenta.Margin = new System.Windows.Forms.Padding(4);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(317, 25);
            this.masterCuenta.TabIndex = 110;
            this.masterCuenta.Value = "";
            this.masterCuenta.Leave += new System.EventHandler(this.masterCuenta_Leave);
            // 
            // gctrlGrid
            // 
            this.gctrlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gctrlGrid.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gctrlGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gctrlGrid.Appearance.Options.UseBackColor = true;
            this.gctrlGrid.Appearance.Options.UseBorderColor = true;
            this.gctrlGrid.Appearance.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gctrlGrid.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.gctrlGrid.AppearanceCaption.Options.UseBorderColor = true;
            this.gctrlGrid.AppearanceCaption.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.Options.UseForeColor = true;
            this.gctrlGrid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gctrlGrid.Controls.Add(this.gcPagos);
            this.gctrlGrid.Location = new System.Drawing.Point(20, 103);
            this.gctrlGrid.LookAndFeel.SkinName = "Seven Classic";
            this.gctrlGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlGrid.Name = "gctrlGrid";
            this.gctrlGrid.ShowCaption = false;
            this.gctrlGrid.Size = new System.Drawing.Size(953, 237);
            this.gctrlGrid.TabIndex = 0;
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
            this.editDate,
            this.editCheck,
            this.editValue,
            this.editValue4});
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
            // editCheck
            // 
            this.editCheck.Name = "editCheck";
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c2";
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
            // PagoFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 603);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "PagoFacturas";
            this.Text = "31";
            ((System.ComponentModel.ISupportInitialize)(this.gvSubDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetallePagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).EndInit();
            this.gctrlHeader.ResumeLayout(false);
            this.gctrlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalleFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalleFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).EndInit();
            this.gctrlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected DevExpress.XtraEditors.GroupControl gctrlHeader;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private System.Windows.Forms.Panel pnlDetail;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        protected System.Windows.Forms.Panel pnlGrids;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraEditors.GroupControl gctrlGrid;
        protected DevExpress.XtraGrid.GridControl gcPagos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetallePagos;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private uc_MasterFind masterCuenta;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        protected DevExpress.XtraGrid.GridControl gcDetalleFacturas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalleFacturas;
        private System.Windows.Forms.TextBox txtChequeIni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChequeFin;
        private System.Windows.Forms.TextBox txtNroEgreso;
        private System.Windows.Forms.Label lblNroEgreso;
        private System.Windows.Forms.Label label5;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        protected uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSubDetalle;       
    }
}