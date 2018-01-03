namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ChequesGirados
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gb_Header = new System.Windows.Forms.GroupBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbOrden = new DevExpress.XtraEditors.LookUpEdit();
            this.dtFechaIni = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaFin = new DevExpress.XtraEditors.DateEdit();
            this.masterCuentaBancaria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_Orden = new System.Windows.Forms.Label();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_RangoFecha = new System.Windows.Forms.Label();
            this.lbl_A = new System.Windows.Forms.Label();
            this.txt_NumCheque = new DevExpress.XtraEditors.TextEdit();
            this.lbl_NumCheque = new System.Windows.Forms.Label();
            this.Btn_Cheque = new System.Windows.Forms.Button();
            this.repositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.spinEdit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.LinkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.LinkEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.LinkEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gbGrid = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            this.gb_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NumCheque.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbGrid)).BeginInit();
            this.gbGrid.SuspendLayout();
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
            this.gvDetalle.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetalleCustomRowCellEdit);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocument_CustomColumnDisplayText);
            // 
            // gcDocument
            // 
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Top;
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
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(2, 22);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(941, 295);
            this.gcDocument.TabIndex = 0;
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
            this.gvDocument.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocument.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocument_CustomColumnDisplayText);
            // 
            // gb_Header
            // 
            this.gb_Header.Controls.Add(this.panelControl1);
            this.gb_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_Header.Location = new System.Drawing.Point(0, 0);
            this.gb_Header.Name = "gb_Header";
            this.gb_Header.Size = new System.Drawing.Size(1013, 132);
            this.gb_Header.TabIndex = 0;
            this.gb_Header.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbOrden);
            this.panelControl1.Controls.Add(this.dtFechaIni);
            this.panelControl1.Controls.Add(this.dtFechaFin);
            this.panelControl1.Controls.Add(this.masterCuentaBancaria);
            this.panelControl1.Controls.Add(this.lbl_Orden);
            this.panelControl1.Controls.Add(this.masterTercero);
            this.panelControl1.Controls.Add(this.lbl_RangoFecha);
            this.panelControl1.Controls.Add(this.lbl_A);
            this.panelControl1.Controls.Add(this.txt_NumCheque);
            this.panelControl1.Controls.Add(this.lbl_NumCheque);
            this.panelControl1.Location = new System.Drawing.Point(18, 19);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(955, 97);
            this.panelControl1.TabIndex = 0;
            // 
            // cmbOrden
            // 
            this.cmbOrden.Location = new System.Drawing.Point(764, 17);
            this.cmbOrden.Name = "cmbOrden";
            this.cmbOrden.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOrden.Properties.Appearance.Options.UseFont = true;
            this.cmbOrden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOrden.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbOrden.Properties.DisplayMember = "Value";
            this.cmbOrden.Properties.NullText = " ";
            this.cmbOrden.Properties.ValueMember = "Key";
            this.cmbOrden.Size = new System.Drawing.Size(119, 20);
            this.cmbOrden.TabIndex = 3;
            // 
            // dtFechaIni
            // 
            this.dtFechaIni.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaIni.Location = new System.Drawing.Point(120, 58);
            this.dtFechaIni.Name = "dtFechaIni";
            this.dtFechaIni.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIni.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIni.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIni.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIni.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIni.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIni.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIni.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIni.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIni.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIni.Size = new System.Drawing.Size(100, 20);
            this.dtFechaIni.TabIndex = 5;
            // 
            // dtFechaFin
            // 
            this.dtFechaFin.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFin.Location = new System.Drawing.Point(434, 56);
            this.dtFechaFin.Name = "dtFechaFin";
            this.dtFechaFin.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFin.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFin.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFin.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFin.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFin.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFin.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFin.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFin.Size = new System.Drawing.Size(100, 20);
            this.dtFechaFin.TabIndex = 7;
            // 
            // masterCuentaBancaria
            // 
            this.masterCuentaBancaria.BackColor = System.Drawing.Color.Transparent;
            this.masterCuentaBancaria.Filtros = null;
            this.masterCuentaBancaria.Location = new System.Drawing.Point(21, 15);
            this.masterCuentaBancaria.Name = "masterCuentaBancaria";
            this.masterCuentaBancaria.Size = new System.Drawing.Size(297, 24);
            this.masterCuentaBancaria.TabIndex = 0;
            this.masterCuentaBancaria.Value = "";
            // 
            // lbl_Orden
            // 
            this.lbl_Orden.AutoSize = true;
            this.lbl_Orden.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Orden.Location = new System.Drawing.Point(655, 20);
            this.lbl_Orden.Name = "lbl_Orden";
            this.lbl_Orden.Size = new System.Drawing.Size(101, 14);
            this.lbl_Orden.TabIndex = 2;
            this.lbl_Orden.Text = "22310_lbl_Orden";
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(332, 15);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(304, 24);
            this.masterTercero.TabIndex = 1;
            this.masterTercero.Value = "";
            // 
            // lbl_RangoFecha
            // 
            this.lbl_RangoFecha.AutoSize = true;
            this.lbl_RangoFecha.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RangoFecha.Location = new System.Drawing.Point(18, 61);
            this.lbl_RangoFecha.Name = "lbl_RangoFecha";
            this.lbl_RangoFecha.Size = new System.Drawing.Size(133, 14);
            this.lbl_RangoFecha.TabIndex = 4;
            this.lbl_RangoFecha.Text = "22310_lbl_RangoFecha";
            // 
            // lbl_A
            // 
            this.lbl_A.AutoSize = true;
            this.lbl_A.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_A.Location = new System.Drawing.Point(330, 60);
            this.lbl_A.Name = "lbl_A";
            this.lbl_A.Size = new System.Drawing.Size(75, 14);
            this.lbl_A.TabIndex = 6;
            this.lbl_A.Text = "22310_lbl_A";
            // 
            // txt_NumCheque
            // 
            this.txt_NumCheque.Location = new System.Drawing.Point(764, 58);
            this.txt_NumCheque.Name = "txt_NumCheque";
            this.txt_NumCheque.Size = new System.Drawing.Size(119, 20);
            this.txt_NumCheque.TabIndex = 9;
            // 
            // lbl_NumCheque
            // 
            this.lbl_NumCheque.AutoSize = true;
            this.lbl_NumCheque.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NumCheque.Location = new System.Drawing.Point(655, 61);
            this.lbl_NumCheque.Name = "lbl_NumCheque";
            this.lbl_NumCheque.Size = new System.Drawing.Size(118, 14);
            this.lbl_NumCheque.TabIndex = 8;
            this.lbl_NumCheque.Text = "22310_lbl_#Cheque";
            // 
            // Btn_Cheque
            // 
            this.Btn_Cheque.Enabled = false;
            this.Btn_Cheque.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cheque.Location = new System.Drawing.Point(801, 325);
            this.Btn_Cheque.Name = "Btn_Cheque";
            this.Btn_Cheque.Size = new System.Drawing.Size(139, 34);
            this.Btn_Cheque.TabIndex = 8;
            this.Btn_Cheque.Text = "22310_Btn_Cheque";
            this.Btn_Cheque.UseVisualStyleBackColor = true;
            this.Btn_Cheque.Click += new System.EventHandler(this.Btn_Cheque_Click);
            // 
            // repositoryEdit
            // 
            this.repositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.spinEdit,
            this.editValue,
            this.LinkEdit,
            this.LinkEdit2,
            this.LinkEdit3});
            // 
            // spinEdit
            // 
            this.spinEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spinEdit.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.spinEdit.Mask.EditMask = "c4";
            this.spinEdit.Name = "spinEdit";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // LinkEdit
            // 
            this.LinkEdit.Caption = "Find_Docs";
            this.LinkEdit.Name = "LinkEdit";
            this.LinkEdit.Click += new System.EventHandler(this.LinkEdit_Click);
            // 
            // LinkEdit2
            // 
            this.LinkEdit2.Caption = "Find_Docs";
            this.LinkEdit2.Name = "LinkEdit2";
            this.LinkEdit2.Click += new System.EventHandler(this.LinkEdit2_Click);
            // 
            // LinkEdit3
            // 
            this.LinkEdit3.Caption = "Imprimir";
            this.LinkEdit3.Name = "LinkEdit3";
            this.LinkEdit3.Click += new System.EventHandler(this.LinkEdit3_Click);
            // 
            // gbGrid
            // 
            this.gbGrid.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbGrid.AppearanceCaption.Options.UseFont = true;
            this.gbGrid.Controls.Add(this.gcDocument);
            this.gbGrid.Controls.Add(this.Btn_Cheque);
            this.gbGrid.Location = new System.Drawing.Point(18, 149);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.gbGrid.Size = new System.Drawing.Size(955, 370);
            this.gbGrid.TabIndex = 1;
            this.gbGrid.Text = "22310_gbCheques";
            // 
            // ChequesGirados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 562);
            this.Controls.Add(this.gbGrid);
            this.Controls.Add(this.gb_Header);
            this.Name = "ChequesGirados";
            this.Text = "22310_ChequesGirados";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            this.gb_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NumCheque.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbGrid)).EndInit();
            this.gbGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Header;
        private System.Windows.Forms.Label lbl_Orden;
        private System.Windows.Forms.Label lbl_A;
        private System.Windows.Forms.Label lbl_RangoFecha;
        private DevExpress.XtraEditors.TextEdit txt_NumCheque;
        private System.Windows.Forms.Label lbl_NumCheque;
        private System.Windows.Forms.Button Btn_Cheque;
        private ControlsUC.uc_MasterFind masterCuentaBancaria;
        private ControlsUC.uc_MasterFind masterTercero;
        private DevExpress.XtraEditors.Repository.PersistentRepository repositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spinEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        protected DevExpress.XtraEditors.DateEdit dtFechaIni;
        protected DevExpress.XtraEditors.DateEdit dtFechaFin;
        private DevExpress.XtraEditors.GroupControl gbGrid;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        private DevExpress.XtraEditors.LookUpEdit cmbOrden;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit3;
    }
}