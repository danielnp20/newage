namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalQueryDocument
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
        protected virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtDocumentoNro = new System.Windows.Forms.TextBox();
            this.txtDocTercero = new System.Windows.Forms.TextBox();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.lblTercero = new System.Windows.Forms.Label();
            this.cmbEstate = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstate = new DevExpress.XtraEditors.LabelControl();
            this.tbFilter = new System.Windows.Forms.TableLayoutPanel();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterDocumento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCtoCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.gbFilterGeneral = new DevExpress.XtraEditors.GroupControl();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.dtFechaFinal = new DevExpress.XtraEditors.DateEdit();
            this.pnDocument = new DevExpress.XtraEditors.PanelControl();
            this.btnFind = new DevExpress.XtraEditors.SimpleButton();
            this.dtFechaInicial = new DevExpress.XtraEditors.DateEdit();
            this.pnGeneral = new System.Windows.Forms.Panel();
            this.gbFilterByDocumento = new DevExpress.XtraEditors.GroupControl();
            this.pnGrid = new DevExpress.XtraEditors.PanelControl();
            this.pnPageGrid = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnGet = new System.Windows.Forms.Button();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).BeginInit();
            this.gbFilterGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).BeginInit();
            this.pnDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).BeginInit();
            this.pnGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).BeginInit();
            this.pnGrid.SuspendLayout();
            this.pnPageGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
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
            // 
            // gcDocument
            // 
            this.gcDocument.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(12, 21);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4, 4, 100, 4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(854, 179);
            this.gcDocument.TabIndex = 53;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            this.gcDocument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcDocument_KeyDown);
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
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocument_CellValueChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocument.DoubleClick += new System.EventHandler(this.gvDocument_DoubleClick);
            // 
            // txtDocumentoNro
            // 
            this.txtDocumentoNro.Location = new System.Drawing.Point(433, 59);
            this.txtDocumentoNro.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txtDocumentoNro.Name = "txtDocumentoNro";
            this.txtDocumentoNro.Size = new System.Drawing.Size(78, 22);
            this.txtDocumentoNro.TabIndex = 1;
            this.txtDocumentoNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNroInv_KeyPress);
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Location = new System.Drawing.Point(433, 84);
            this.txtDocTercero.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txtDocTercero.Name = "txtDocTercero";
            this.txtDocTercero.Size = new System.Drawing.Size(78, 22);
            this.txtDocTercero.TabIndex = 3;
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijo.Location = new System.Drawing.Point(358, 65);
            this.lblPrefijo.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(106, 14);
            this.lblPrefijo.TabIndex = 0;
            this.lblPrefijo.Text = "1034_lblNroPrefijo";
            // 
            // lblTercero
            // 
            this.lblTercero.AutoSize = true;
            this.lblTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTercero.Location = new System.Drawing.Point(358, 89);
            this.lblTercero.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblTercero.Name = "lblTercero";
            this.lblTercero.Size = new System.Drawing.Size(117, 14);
            this.lblTercero.TabIndex = 0;
            this.lblTercero.Text = "1034_lblDocTercero";
            // 
            // cmbEstate
            // 
            this.cmbEstate.Location = new System.Drawing.Point(422, 154);
            this.cmbEstate.Name = "cmbEstate";
            this.cmbEstate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstate.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstate.Properties.DisplayMember = "Value";
            this.cmbEstate.Properties.ValueMember = "Key";
            this.cmbEstate.Size = new System.Drawing.Size(89, 20);
            this.cmbEstate.TabIndex = 2;
            // 
            // lblEstate
            // 
            this.lblEstate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblEstate.Location = new System.Drawing.Point(360, 157);
            this.lblEstate.Margin = new System.Windows.Forms.Padding(4);
            this.lblEstate.Name = "lblEstate";
            this.lblEstate.Size = new System.Drawing.Size(83, 14);
            this.lblEstate.TabIndex = 13;
            this.lblEstate.Text = "1034_lblEstado";
            // 
            // tbFilter
            // 
            this.tbFilter.ColumnCount = 4;
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.875F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 93.125F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tbFilter.Location = new System.Drawing.Point(24, 81);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.RowCount = 4;
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.Size = new System.Drawing.Size(607, 122);
            this.tbFilter.TabIndex = 0;
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(15, 82);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(346, 24);
            this.masterTercero.TabIndex = 2;
            this.masterTercero.Value = "";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(15, 58);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(346, 24);
            this.masterPrefijo.TabIndex = 0;
            this.masterPrefijo.Value = "";
            // 
            // masterDocumento
            // 
            this.masterDocumento.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumento.Filtros = null;
            this.masterDocumento.Location = new System.Drawing.Point(6, 2);
            this.masterDocumento.Name = "masterDocumento";
            this.masterDocumento.Size = new System.Drawing.Size(351, 27);
            this.masterDocumento.TabIndex = 0;
            this.masterDocumento.Value = "";
            // 
            // masterCtoCosto
            // 
            this.masterCtoCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCtoCosto.Filtros = null;
            this.masterCtoCosto.Location = new System.Drawing.Point(15, 130);
            this.masterCtoCosto.Name = "masterCtoCosto";
            this.masterCtoCosto.Size = new System.Drawing.Size(346, 24);
            this.masterCtoCosto.TabIndex = 11;
            this.masterCtoCosto.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(15, 106);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(346, 24);
            this.masterProyecto.TabIndex = 10;
            this.masterProyecto.Value = "";
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicial.Location = new System.Drawing.Point(358, 113);
            this.lblFechaInicial.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(98, 14);
            this.lblFechaInicial.TabIndex = 31;
            this.lblFechaInicial.Text = "1034_lblFechaIni";
            // 
            // gbFilterGeneral
            // 
            this.gbFilterGeneral.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterGeneral.Appearance.Options.UseFont = true;
            this.gbFilterGeneral.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterGeneral.AppearanceCaption.Options.UseFont = true;
            this.gbFilterGeneral.Controls.Add(this.lblFechaFinal);
            this.gbFilterGeneral.Controls.Add(this.lblEstate);
            this.gbFilterGeneral.Controls.Add(this.cmbEstate);
            this.gbFilterGeneral.Controls.Add(this.dtFechaFinal);
            this.gbFilterGeneral.Controls.Add(this.lblFechaInicial);
            this.gbFilterGeneral.Controls.Add(this.pnDocument);
            this.gbFilterGeneral.Controls.Add(this.lblPrefijo);
            this.gbFilterGeneral.Controls.Add(this.txtDocumentoNro);
            this.gbFilterGeneral.Controls.Add(this.masterCtoCosto);
            this.gbFilterGeneral.Controls.Add(this.dtFechaInicial);
            this.gbFilterGeneral.Controls.Add(this.lblTercero);
            this.gbFilterGeneral.Controls.Add(this.txtDocTercero);
            this.gbFilterGeneral.Controls.Add(this.masterPrefijo);
            this.gbFilterGeneral.Controls.Add(this.masterProyecto);
            this.gbFilterGeneral.Controls.Add(this.masterTercero);
            this.gbFilterGeneral.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbFilterGeneral.Location = new System.Drawing.Point(0, 0);
            this.gbFilterGeneral.Name = "gbFilterGeneral";
            this.gbFilterGeneral.Size = new System.Drawing.Size(518, 179);
            this.gbFilterGeneral.TabIndex = 34;
            this.gbFilterGeneral.Text = "1034_gbGeneral";
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinal.Location = new System.Drawing.Point(358, 135);
            this.lblFechaFinal.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinal.TabIndex = 96;
            this.lblFechaFinal.Text = "1034_lblFechaFin";
            // 
            // dtFechaFinal
            // 
            this.dtFechaFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinal.Location = new System.Drawing.Point(433, 132);
            this.dtFechaFinal.Name = "dtFechaFinal";
            this.dtFechaFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.dtFechaFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinal.Properties.Appearance.Options.UseFont = true;
            this.dtFechaFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinal.Size = new System.Drawing.Size(78, 18);
            this.dtFechaFinal.TabIndex = 97;
            // 
            // pnDocument
            // 
            this.pnDocument.Controls.Add(this.btnFind);
            this.pnDocument.Controls.Add(this.masterDocumento);
            this.pnDocument.Location = new System.Drawing.Point(11, 26);
            this.pnDocument.Name = "pnDocument";
            this.pnDocument.Size = new System.Drawing.Size(501, 30);
            this.pnDocument.TabIndex = 98;
            // 
            // btnFind
            // 
            this.btnFind.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnFind.Appearance.Options.UseFont = true;
            this.btnFind.Location = new System.Drawing.Point(373, 3);
            this.btnFind.LookAndFeel.SkinName = "Black";
            this.btnFind.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(111, 23);
            this.btnFind.TabIndex = 3;
            this.btnFind.Text = "1034_btnFind";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // dtFechaInicial
            // 
            this.dtFechaInicial.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicial.Location = new System.Drawing.Point(433, 109);
            this.dtFechaInicial.Name = "dtFechaInicial";
            this.dtFechaInicial.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicial.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.dtFechaInicial.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicial.Properties.Appearance.Options.UseFont = true;
            this.dtFechaInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicial.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicial.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicial.Size = new System.Drawing.Size(78, 18);
            this.dtFechaInicial.TabIndex = 95;
            // 
            // pnGeneral
            // 
            this.pnGeneral.Controls.Add(this.gbFilterByDocumento);
            this.pnGeneral.Controls.Add(this.gbFilterGeneral);
            this.pnGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnGeneral.Location = new System.Drawing.Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new System.Drawing.Size(878, 179);
            this.pnGeneral.TabIndex = 35;
            // 
            // gbFilterByDocumento
            // 
            this.gbFilterByDocumento.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterByDocumento.Appearance.Options.UseFont = true;
            this.gbFilterByDocumento.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterByDocumento.AppearanceCaption.Options.UseFont = true;
            this.gbFilterByDocumento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilterByDocumento.Location = new System.Drawing.Point(518, 0);
            this.gbFilterByDocumento.Name = "gbFilterByDocumento";
            this.gbFilterByDocumento.Size = new System.Drawing.Size(360, 179);
            this.gbFilterByDocumento.TabIndex = 35;
            this.gbFilterByDocumento.Text = "1034_gbDocumento";
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.pnPageGrid);
            this.pnGrid.Controls.Add(this.gcDocument);
            this.pnGrid.Controls.Add(this.chkSelectAll);
            this.pnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGrid.Location = new System.Drawing.Point(0, 179);
            this.pnGrid.Name = "pnGrid";
            this.pnGrid.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pnGrid.Size = new System.Drawing.Size(878, 238);
            this.pnGrid.TabIndex = 36;
            // 
            // pnPageGrid
            // 
            this.pnPageGrid.BackColor = System.Drawing.Color.Transparent;
            this.pnPageGrid.Controls.Add(this.btnCancel);
            this.pnPageGrid.Controls.Add(this.btnCopy);
            this.pnPageGrid.Controls.Add(this.btnGet);
            this.pnPageGrid.Controls.Add(this.pgGrid);
            this.pnPageGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPageGrid.Location = new System.Drawing.Point(12, 200);
            this.pnPageGrid.Name = "pnPageGrid";
            this.pnPageGrid.Size = new System.Drawing.Size(854, 32);
            this.pnPageGrid.TabIndex = 55;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(743, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 23);
            this.btnCancel.TabIndex = 56;
            this.btnCancel.Text = "1034_btnCancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(639, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(101, 23);
            this.btnCopy.TabIndex = 55;
            this.btnCopy.Text = "1034_btnCopy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnGet
            // 
            this.btnGet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(535, 3);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(101, 23);
            this.btnGet.TabIndex = 3;
            this.btnGet.Text = "1034_btnGet";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // pgGrid
            // 
            this.pgGrid.BackColor = System.Drawing.Color.Transparent;
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(25, 2);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(652, 25);
            this.pgGrid.TabIndex = 54;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 2);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkSelectAll.Properties.Appearance.Options.UseFont = true;
            this.chkSelectAll.Properties.Caption = "1034_chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(854, 19);
            this.chkSelectAll.TabIndex = 52;
            this.chkSelectAll.Visible = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editSpinPorcen});
            // 
            // editChkBox
            // 
            this.editChkBox.Caption = "";
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.editChkBox.CheckedChanged += new System.EventHandler(this.editChek_CheckedChanged);
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
            // editSpinPorcen
            // 
            this.editSpinPorcen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorcen.Name = "editSpinPorcen";
            // 
            // ModalQueryDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(878, 417);
            this.Controls.Add(this.pnGrid);
            this.Controls.Add(this.pnGeneral);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalQueryDocument";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1034";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).EndInit();
            this.gbFilterGeneral.ResumeLayout(false);
            this.gbFilterGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).EndInit();
            this.pnDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).EndInit();
            this.pnGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).EndInit();
            this.pnGrid.ResumeLayout(false);
            this.pnPageGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected ControlsUC.uc_MasterFind masterDocumento;
        protected ControlsUC.uc_MasterFind masterPrefijo;
        protected ControlsUC.uc_MasterFind masterTercero;
        protected ControlsUC.uc_MasterFind masterProyecto;
        protected ControlsUC.uc_MasterFind masterCtoCosto;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.Label lblTercero;
        private System.Windows.Forms.Label lblFechaFinal;
        private System.Windows.Forms.Label lblFechaInicial;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicial;
        protected DevExpress.XtraEditors.DateEdit dtFechaFinal;
        protected System.Windows.Forms.TextBox txtDocumentoNro;
        protected System.Windows.Forms.TextBox txtDocTercero;
        protected System.Windows.Forms.TableLayoutPanel tbFilter;
        protected System.Windows.Forms.Panel pnGeneral;
        protected DevExpress.XtraEditors.PanelControl pnDocument;
        protected DevExpress.XtraEditors.PanelControl pnGrid;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        protected DevExpress.XtraEditors.CheckEdit chkSelectAll;
        protected DevExpress.XtraEditors.GroupControl gbFilterGeneral;
        protected DevExpress.XtraEditors.GroupControl gbFilterByDocumento;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorcen;
        protected DevExpress.XtraEditors.LookUpEdit cmbEstate;
        private System.Windows.Forms.ToolTip toolTipGrid;
        private ControlsUC.uc_Pagging pgGrid;
        protected System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Panel pnPageGrid;
        protected System.Windows.Forms.Button btnCopy;
        private DevExpress.XtraEditors.LabelControl lblEstate;
        protected System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnFind;

    }
}