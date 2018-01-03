namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CobroJuridico
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaEC = new DevExpress.XtraEditors.DateEdit();
            this.cmbClaseDeuda = new DevExpress.XtraEditors.LookUpEdit();
            this.lblClaseDeuda = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaDoc = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaDoc = new DevExpress.XtraEditors.DateEdit();
            this.cmbEstadoActual = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstadoActual = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoMov = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoMov = new DevExpress.XtraEditors.LabelControl();
            this.cmbFiltro = new DevExpress.XtraEditors.LookUpEdit();
            this.lblFiltro = new DevExpress.XtraEditors.LabelControl();
            this.btnAbonos = new System.Windows.Forms.Button();
            this.btnInteres = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.gcHistoria = new DevExpress.XtraGrid.GridControl();
            this.gvHistoria = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEC.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClaseDeuda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstadoActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMov.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFiltro.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcHistoria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHistoria)).BeginInit();
            this.pnlGrids.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
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
            this.gvDetalle.GridControl = this.gcDocument;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
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
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDetalle_BeforeLeaveRow);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, false, ""),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(6, 13);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.ShowOnlyPredefinedDetails = true;
            this.gcDocument.Size = new System.Drawing.Size(1045, 189);
            this.gcDocument.TabIndex = 50;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocument.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocument.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocument.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocument.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocument.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocument.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.HorzScrollStep = 50;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocument.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocument.BeforeLeaveRow += new  DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocument_BeforeLeaveRow);
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
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
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin,
            this.editSpin4,
            this.editValue});
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(0, 0);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.39548F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.60452F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1080, 620);
            this.tlSeparatorPanel.TabIndex = 63;
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
            this.grpctrlHeader.Controls.Add(this.grpboxHeader);
            this.grpctrlHeader.Controls.Add(this.dtPeriod);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.txtNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(12, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1057, 137);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Controls.Add(this.labelControl1);
            this.grpboxHeader.Controls.Add(this.dtFechaEC);
            this.grpboxHeader.Controls.Add(this.cmbClaseDeuda);
            this.grpboxHeader.Controls.Add(this.lblClaseDeuda);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Controls.Add(this.lblFechaDoc);
            this.grpboxHeader.Controls.Add(this.dtFechaDoc);
            this.grpboxHeader.Controls.Add(this.cmbEstadoActual);
            this.grpboxHeader.Controls.Add(this.lblEstadoActual);
            this.grpboxHeader.Controls.Add(this.cmbTipoMov);
            this.grpboxHeader.Controls.Add(this.lblTipoMov);
            this.grpboxHeader.Controls.Add(this.cmbFiltro);
            this.grpboxHeader.Controls.Add(this.lblFiltro);
            this.grpboxHeader.Controls.Add(this.btnAbonos);
            this.grpboxHeader.Controls.Add(this.btnInteres);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(1053, 113);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(674, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(74, 14);
            this.labelControl1.TabIndex = 77;
            this.labelControl1.Text = "177_FechaEC";
            // 
            // dtFechaEC
            // 
            this.dtFechaEC.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaEC.Location = new System.Drawing.Point(790, 17);
            this.dtFechaEC.Margin = new System.Windows.Forms.Padding(2);
            this.dtFechaEC.Name = "dtFechaEC";
            this.dtFechaEC.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaEC.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaEC.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaEC.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaEC.Properties.Appearance.Options.UseFont = true;
            this.dtFechaEC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaEC.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaEC.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaEC.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaEC.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaEC.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaEC.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaEC.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaEC.Size = new System.Drawing.Size(113, 20);
            this.dtFechaEC.TabIndex = 78;
            // 
            // cmbClaseDeuda
            // 
            this.cmbClaseDeuda.Location = new System.Drawing.Point(496, 69);
            this.cmbClaseDeuda.Name = "cmbClaseDeuda";
            this.cmbClaseDeuda.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbClaseDeuda.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbClaseDeuda.Properties.DisplayMember = "Value";
            this.cmbClaseDeuda.Properties.NullText = " ";
            this.cmbClaseDeuda.Properties.ValueMember = "Key";
            this.cmbClaseDeuda.Size = new System.Drawing.Size(117, 20);
            this.cmbClaseDeuda.TabIndex = 75;
            // 
            // lblClaseDeuda
            // 
            this.lblClaseDeuda.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClaseDeuda.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblClaseDeuda.Location = new System.Drawing.Point(406, 72);
            this.lblClaseDeuda.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblClaseDeuda.Name = "lblClaseDeuda";
            this.lblClaseDeuda.Size = new System.Drawing.Size(101, 14);
            this.lblClaseDeuda.TabIndex = 76;
            this.lblClaseDeuda.Text = "177_lblClaseDeuda";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(16, 13);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(363, 29);
            this.masterCliente.TabIndex = 63;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaDoc.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaDoc.Location = new System.Drawing.Point(16, 45);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(81, 14);
            this.lblFechaDoc.TabIndex = 64;
            this.lblFechaDoc.Text = "177_FechaDoc";
            // 
            // dtFechaDoc
            // 
            this.dtFechaDoc.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaDoc.Location = new System.Drawing.Point(132, 42);
            this.dtFechaDoc.Margin = new System.Windows.Forms.Padding(2);
            this.dtFechaDoc.Name = "dtFechaDoc";
            this.dtFechaDoc.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaDoc.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDoc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaDoc.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDoc.Properties.Appearance.Options.UseFont = true;
            this.dtFechaDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDoc.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDoc.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDoc.Size = new System.Drawing.Size(113, 20);
            this.dtFechaDoc.TabIndex = 65;
            // 
            // cmbEstadoActual
            // 
            this.cmbEstadoActual.Location = new System.Drawing.Point(496, 17);
            this.cmbEstadoActual.Name = "cmbEstadoActual";
            this.cmbEstadoActual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstadoActual.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstadoActual.Properties.DisplayMember = "Value";
            this.cmbEstadoActual.Properties.NullText = " ";
            this.cmbEstadoActual.Properties.ValueMember = "Key";
            this.cmbEstadoActual.Size = new System.Drawing.Size(117, 20);
            this.cmbEstadoActual.TabIndex = 67;
            // 
            // lblEstadoActual
            // 
            this.lblEstadoActual.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoActual.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblEstadoActual.Location = new System.Drawing.Point(406, 20);
            this.lblEstadoActual.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblEstadoActual.Name = "lblEstadoActual";
            this.lblEstadoActual.Size = new System.Drawing.Size(110, 14);
            this.lblEstadoActual.TabIndex = 68;
            this.lblEstadoActual.Text = "177_lblEstadoActual";
            // 
            // cmbTipoMov
            // 
            this.cmbTipoMov.Location = new System.Drawing.Point(497, 42);
            this.cmbTipoMov.Name = "cmbTipoMov";
            this.cmbTipoMov.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoMov.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoMov.Properties.DisplayMember = "Value";
            this.cmbTipoMov.Properties.NullText = " ";
            this.cmbTipoMov.Properties.ValueMember = "Key";
            this.cmbTipoMov.Size = new System.Drawing.Size(117, 20);
            this.cmbTipoMov.TabIndex = 69;
            // 
            // lblTipoMov
            // 
            this.lblTipoMov.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMov.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTipoMov.Location = new System.Drawing.Point(406, 45);
            this.lblTipoMov.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblTipoMov.Name = "lblTipoMov";
            this.lblTipoMov.Size = new System.Drawing.Size(85, 14);
            this.lblTipoMov.TabIndex = 70;
            this.lblTipoMov.Text = "177_lblTipoMov";
            // 
            // cmbFiltro
            // 
            this.cmbFiltro.Location = new System.Drawing.Point(132, 70);
            this.cmbFiltro.Name = "cmbFiltro";
            this.cmbFiltro.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFiltro.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbFiltro.Properties.DisplayMember = "Value";
            this.cmbFiltro.Properties.NullText = " ";
            this.cmbFiltro.Properties.ValueMember = "Key";
            this.cmbFiltro.Size = new System.Drawing.Size(117, 20);
            this.cmbFiltro.TabIndex = 71;
            // 
            // lblFiltro
            // 
            this.lblFiltro.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFiltro.Location = new System.Drawing.Point(16, 73);
            this.lblFiltro.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(65, 14);
            this.lblFiltro.TabIndex = 72;
            this.lblFiltro.Text = "177_lblFiltro";
            // 
            // btnAbonos
            // 
            this.btnAbonos.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbonos.Location = new System.Drawing.Point(673, 63);
            this.btnAbonos.Name = "btnAbonos";
            this.btnAbonos.Size = new System.Drawing.Size(90, 23);
            this.btnAbonos.TabIndex = 73;
            this.btnAbonos.Text = "177_btnAbonos";
            this.btnAbonos.UseVisualStyleBackColor = true;
            // 
            // btnInteres
            // 
            this.btnInteres.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInteres.Location = new System.Drawing.Point(790, 63);
            this.btnInteres.Name = "btnInteres";
            this.btnInteres.Size = new System.Drawing.Size(113, 23);
            this.btnInteres.TabIndex = 74;
            this.btnInteres.Text = "Reliquidar";
            this.btnInteres.UseVisualStyleBackColor = true;
            this.btnInteres.Click += new System.EventHandler(this.btnInteres_Click);
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
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(12, 357);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1057, 260);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Controls.Add(this.gcHistoria);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Size = new System.Drawing.Size(1057, 260);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            // 
            // gcHistoria
            // 
            this.gcHistoria.AllowDrop = true;
            this.gcHistoria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcHistoria.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcHistoria.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcHistoria.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcHistoria.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcHistoria.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcHistoria.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcHistoria.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcHistoria.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcHistoria.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcHistoria.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcHistoria.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcHistoria.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcHistoria.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcHistoria.Location = new System.Drawing.Point(3, 16);
            this.gcHistoria.LookAndFeel.SkinName = "Dark Side";
            this.gcHistoria.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcHistoria.MainView = this.gvHistoria;
            this.gcHistoria.Margin = new System.Windows.Forms.Padding(4);
            this.gcHistoria.Name = "gcHistoria";
            this.gcHistoria.Size = new System.Drawing.Size(1051, 241);
            this.gcHistoria.TabIndex = 52;
            this.gcHistoria.UseEmbeddedNavigator = true;
            this.gcHistoria.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHistoria});
            // 
            // gvHistoria
            // 
            this.gvHistoria.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHistoria.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvHistoria.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvHistoria.Appearance.Empty.Options.UseBackColor = true;
            this.gvHistoria.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvHistoria.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvHistoria.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHistoria.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvHistoria.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvHistoria.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvHistoria.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHistoria.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvHistoria.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvHistoria.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvHistoria.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvHistoria.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvHistoria.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvHistoria.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvHistoria.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHistoria.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvHistoria.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvHistoria.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvHistoria.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvHistoria.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvHistoria.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvHistoria.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvHistoria.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvHistoria.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHistoria.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvHistoria.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvHistoria.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvHistoria.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvHistoria.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvHistoria.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvHistoria.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvHistoria.Appearance.Row.Options.UseBackColor = true;
            this.gvHistoria.Appearance.Row.Options.UseForeColor = true;
            this.gvHistoria.Appearance.Row.Options.UseTextOptions = true;
            this.gvHistoria.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvHistoria.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvHistoria.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvHistoria.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvHistoria.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvHistoria.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvHistoria.Appearance.VertLine.Options.UseBackColor = true;
            this.gvHistoria.GridControl = this.gcHistoria;
            this.gvHistoria.HorzScrollStep = 50;
            this.gvHistoria.Name = "gvHistoria";
            this.gvHistoria.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvHistoria.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvHistoria.OptionsCustomization.AllowFilter = false;
            this.gvHistoria.OptionsCustomization.AllowSort = false;
            this.gvHistoria.OptionsMenu.EnableColumnMenu = false;
            this.gvHistoria.OptionsMenu.EnableFooterMenu = false;
            this.gvHistoria.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvHistoria.OptionsView.ColumnAutoWidth = false;
            this.gvHistoria.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHistoria.OptionsView.ShowGroupPanel = false;
            this.gvHistoria.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHistoria.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvHistoria.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvHistoria_CustomColumnDisplayText);
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gbGridDocument);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(12, 146);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1057, 205);
            this.pnlGrids.TabIndex = 113;
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gcDocument);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(0, 0);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbGridDocument.Size = new System.Drawing.Size(1057, 205);
            this.gbGridDocument.TabIndex = 54;
            this.gbGridDocument.TabStop = false;
            // 
            // CobroJuridico
            // 
            this.ClientSize = new System.Drawing.Size(1080, 620);
            this.Controls.Add(this.tlSeparatorPanel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CobroJuridico";
            this.Text = "32564";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEC.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClaseDeuda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstadoActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoMov.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFiltro.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.grpboxDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcHistoria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHistoria)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private System.ComponentModel.IContainer components;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.Panel pnlDetail;
        protected System.Windows.Forms.GroupBox grpboxDetail;
        protected DevExpress.XtraGrid.GridControl gcHistoria;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvHistoria;
        protected System.Windows.Forms.Panel pnlGrids;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.LabelControl lblFechaDoc;
        protected DevExpress.XtraEditors.DateEdit dtFechaDoc;
        private DevExpress.XtraEditors.LookUpEdit cmbEstadoActual;
        private DevExpress.XtraEditors.LabelControl lblEstadoActual;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoMov;
        private DevExpress.XtraEditors.LabelControl lblTipoMov;
        private DevExpress.XtraEditors.LookUpEdit cmbFiltro;
        private DevExpress.XtraEditors.LabelControl lblFiltro;
        private System.Windows.Forms.Button btnAbonos;
        private System.Windows.Forms.Button btnInteres;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblBreak;
        protected System.Windows.Forms.TextBox txtDocDesc;
        protected System.Windows.Forms.TextBox txtDocumentoID;
        protected System.Windows.Forms.TextBox txtNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private DevExpress.XtraEditors.LookUpEdit cmbClaseDeuda;
        private DevExpress.XtraEditors.LabelControl lblClaseDeuda;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        protected DevExpress.XtraEditors.DateEdit dtFechaEC;
    }
}