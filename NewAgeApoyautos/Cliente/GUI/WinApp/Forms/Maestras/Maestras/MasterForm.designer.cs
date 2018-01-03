using DevExpress.Utils;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Controls;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MasterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlModule = new System.Windows.Forms.TableLayoutPanel();
            this.pnGrid = new System.Windows.Forms.Panel();
            this.grlcontrolModule = new DevExpress.XtraGrid.GridControl();
            this.gvModule = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnSearch = new DevExpress.XtraEditors.GroupControl();
            this.txtDescrip = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblDescrip = new DevExpress.XtraEditors.LabelControl();
            this.lblCode = new DevExpress.XtraEditors.LabelControl();
            this.pnlRecordEdit = new System.Windows.Forms.Panel();
            this.tabRecordEdit = new DevExpress.XtraTab.XtraTabControl();
            this.tpRecordEdit = new DevExpress.XtraTab.XtraTabPage();
            this.grlControlRecordEdit = new DevExpress.XtraGrid.GridControl();
            this.gvRecordEdit = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.chkRecordEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.txtRecordEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.txtRecorMax = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnRecordEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editFecha = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editPeriodo = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.cmbRank = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.richTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.popupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.imgEdit = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.editPorc = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValor = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.pnlModule.SuspendLayout();
            this.pnGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolModule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvModule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnSearch)).BeginInit();
            this.pnSearch.SuspendLayout();
            this.pnlRecordEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabRecordEdit)).BeginInit();
            this.tabRecordEdit.SuspendLayout();
            this.tpRecordEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlControlRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecorMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFecha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFecha.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPeriodo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPeriodo.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl)).BeginInit();
            this.popupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValor)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlModule
            // 
            this.pnlModule.ColumnCount = 1;
            this.pnlModule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlModule.Controls.Add(this.pnGrid, 0, 0);
            this.pnlModule.Controls.Add(this.pnlRecordEdit, 0, 1);
            this.pnlModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModule.Location = new System.Drawing.Point(0, 0);
            this.pnlModule.Name = "pnlModule";
            this.pnlModule.RowCount = 2;
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.08419F));
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.91581F));
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlModule.Size = new System.Drawing.Size(856, 482);
            this.pnlModule.TabIndex = 66;
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.grlcontrolModule);
            this.pnGrid.Controls.Add(this.pnSearch);
            this.pnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGrid.Location = new System.Drawing.Point(3, 3);
            this.pnGrid.Name = "pnGrid";
            this.pnGrid.Size = new System.Drawing.Size(850, 269);
            this.pnGrid.TabIndex = 68;
            // 
            // grlcontrolModule
            // 
            this.grlcontrolModule.AllowDrop = true;
            this.grlcontrolModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grlcontrolModule.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grlcontrolModule.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.grlcontrolModule.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.grlcontrolModule.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.grlcontrolModule.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grlcontrolModule.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grlcontrolModule.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grlcontrolModule.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grlcontrolModule.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grlcontrolModule.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolModule.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.grlcontrolModule.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.grlcontrolModule.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grlcontrolModule.Location = new System.Drawing.Point(0, 28);
            this.grlcontrolModule.LookAndFeel.SkinName = "Dark Side";
            this.grlcontrolModule.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grlcontrolModule.MainView = this.gvModule;
            this.grlcontrolModule.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolModule.Name = "grlcontrolModule";
            this.grlcontrolModule.Size = new System.Drawing.Size(850, 241);
            this.grlcontrolModule.TabIndex = 0;
            this.grlcontrolModule.UseEmbeddedNavigator = true;
            this.grlcontrolModule.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvModule});
            // 
            // gvModule
            // 
            this.gvModule.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvModule.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvModule.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvModule.Appearance.Empty.Options.UseBackColor = true;
            this.gvModule.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvModule.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvModule.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModule.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvModule.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvModule.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvModule.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModule.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvModule.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvModule.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvModule.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvModule.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvModule.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvModule.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvModule.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvModule.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvModule.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvModule.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvModule.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvModule.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvModule.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvModule.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvModule.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvModule.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModule.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvModule.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvModule.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvModule.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvModule.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvModule.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvModule.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvModule.Appearance.Row.Options.UseBackColor = true;
            this.gvModule.Appearance.Row.Options.UseForeColor = true;
            this.gvModule.Appearance.Row.Options.UseTextOptions = true;
            this.gvModule.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvModule.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvModule.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvModule.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvModule.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvModule.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvModule.Appearance.VertLine.Options.UseBackColor = true;
            this.gvModule.FixedLineWidth = 1;
            this.gvModule.GridControl = this.grlcontrolModule;
            this.gvModule.HorzScrollStep = 50;
            this.gvModule.Name = "gvModule";
            this.gvModule.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvModule.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvModule.OptionsBehavior.Editable = false;
            this.gvModule.OptionsCustomization.AllowColumnMoving = false;
            this.gvModule.OptionsCustomization.AllowFilter = false;
            this.gvModule.OptionsCustomization.AllowSort = false;
            this.gvModule.OptionsMenu.EnableColumnMenu = false;
            this.gvModule.OptionsMenu.EnableFooterMenu = false;
            this.gvModule.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvModule.OptionsView.ColumnAutoWidth = false;
            this.gvModule.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvModule.OptionsView.ShowGroupPanel = false;
            this.gvModule.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvModule.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvModule_RowClick);
            this.gvModule.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvModule_FocusedRowChanged);
            this.gvModule.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvModule_BeforeLeaveRow);
            this.gvModule.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvModule_CustomUnboundColumnData);
            // 
            // pnSearch
            // 
            this.pnSearch.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pnSearch.Appearance.BackColor2 = System.Drawing.Color.White;
            this.pnSearch.Appearance.Options.UseBackColor = true;
            this.pnSearch.Controls.Add(this.txtDescrip);
            this.pnSearch.Controls.Add(this.txtCode);
            this.pnSearch.Controls.Add(this.btnSearch);
            this.pnSearch.Controls.Add(this.lblDescrip);
            this.pnSearch.Controls.Add(this.lblCode);
            this.pnSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSearch.Location = new System.Drawing.Point(0, 0);
            this.pnSearch.LookAndFeel.SkinName = "Dark Side";
            this.pnSearch.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnSearch.Name = "pnSearch";
            this.pnSearch.ShowCaption = false;
            this.pnSearch.Size = new System.Drawing.Size(850, 28);
            this.pnSearch.TabIndex = 67;
            this.pnSearch.Visible = false;
            // 
            // txtDescrip
            // 
            this.txtDescrip.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrip.Location = new System.Drawing.Point(289, 4);
            this.txtDescrip.Name = "txtDescrip";
            this.txtDescrip.Size = new System.Drawing.Size(128, 22);
            this.txtDescrip.TabIndex = 6;
            this.txtDescrip.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_Click);
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(79, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(119, 22);
            this.txtCode.TabIndex = 5;
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(437, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "1004_btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblDescrip
            // 
            this.lblDescrip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrip.Location = new System.Drawing.Point(211, 7);
            this.lblDescrip.Name = "lblDescrip";
            this.lblDescrip.Size = new System.Drawing.Size(107, 14);
            this.lblDescrip.TabIndex = 3;
            this.lblDescrip.Text = "1004_lblDescripcion";
            // 
            // lblCode
            // 
            this.lblCode.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.Location = new System.Drawing.Point(24, 7);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(83, 14);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "1004_lblCodigo";
            // 
            // pnlRecordEdit
            // 
            this.pnlRecordEdit.BackColor = System.Drawing.Color.GhostWhite;
            this.pnlRecordEdit.Controls.Add(this.pgGrid);
            this.pnlRecordEdit.Controls.Add(this.tabRecordEdit);
            this.pnlRecordEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRecordEdit.Location = new System.Drawing.Point(3, 278);
            this.pnlRecordEdit.Name = "pnlRecordEdit";
            this.pnlRecordEdit.Size = new System.Drawing.Size(850, 201);
            this.pnlRecordEdit.TabIndex = 56;
            // 
            // tabRecordEdit
            // 
            this.tabRecordEdit.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.tabRecordEdit.Appearance.Options.UseBackColor = true;
            this.tabRecordEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.tabRecordEdit.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tabRecordEdit.HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.WhenNeeded;
            this.tabRecordEdit.Location = new System.Drawing.Point(9, 13);
            this.tabRecordEdit.Name = "tabRecordEdit";
            this.tabRecordEdit.SelectedTabPage = this.tpRecordEdit;
            this.tabRecordEdit.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.tabRecordEdit.Size = new System.Drawing.Size(386, 154);
            this.tabRecordEdit.TabIndex = 66;
            this.tabRecordEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpRecordEdit});
            // 
            // tpRecordEdit
            // 
            this.tpRecordEdit.AutoScroll = true;
            this.tpRecordEdit.Controls.Add(this.grlControlRecordEdit);
            this.tpRecordEdit.Name = "tpRecordEdit";
            this.tpRecordEdit.Size = new System.Drawing.Size(378, 124);
            this.tpRecordEdit.Text = "1004_tpRecordEdit";
            // 
            // grlControlRecordEdit
            // 
            this.grlControlRecordEdit.Location = new System.Drawing.Point(15, 19);
            this.grlControlRecordEdit.LookAndFeel.SkinName = "Dark Side";
            this.grlControlRecordEdit.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grlControlRecordEdit.MainView = this.gvRecordEdit;
            this.grlControlRecordEdit.Name = "grlControlRecordEdit";
            this.grlControlRecordEdit.Size = new System.Drawing.Size(338, 86);
            this.grlControlRecordEdit.TabIndex = 0;
            this.grlControlRecordEdit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecordEdit});
            this.grlControlRecordEdit.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.grlRecordEdit_ProcessGridKey);
            // 
            // gvRecordEdit
            // 
            this.gvRecordEdit.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecordEdit.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecordEdit.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecordEdit.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecordEdit.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecordEdit.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.FooterPanel.BackColor = System.Drawing.Color.White;
            this.gvRecordEdit.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvRecordEdit.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.White;
            this.gvRecordEdit.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecordEdit.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecordEdit.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecordEdit.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecordEdit.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRecordEdit.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecordEdit.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecordEdit.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecordEdit.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRecordEdit.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecordEdit.Appearance.Row.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.Row.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecordEdit.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecordEdit.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecordEdit.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvRecordEdit.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecordEdit.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecordEdit.GridControl = this.grlControlRecordEdit;
            this.gvRecordEdit.HorzScrollStep = 1;
            this.gvRecordEdit.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gvRecordEdit.Name = "gvRecordEdit";
            this.gvRecordEdit.OptionsCustomization.AllowColumnMoving = false;
            this.gvRecordEdit.OptionsCustomization.AllowColumnResizing = false;
            this.gvRecordEdit.OptionsCustomization.AllowFilter = false;
            this.gvRecordEdit.OptionsDetail.AllowZoomDetail = false;
            this.gvRecordEdit.OptionsMenu.EnableColumnMenu = false;
            this.gvRecordEdit.OptionsMenu.EnableFooterMenu = false;
            this.gvRecordEdit.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRecordEdit.OptionsView.ColumnAutoWidth = false;
            this.gvRecordEdit.OptionsView.ShowGroupPanel = false;
            this.gvRecordEdit.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecordEdit_CustomRowCellEdit);
            this.gvRecordEdit.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecordEdit_CustomRowCellEditForEditing);
            this.gvRecordEdit.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gvRecordEdit_MouseWheel);
            this.gvRecordEdit.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gvRecordEdit_ValidatingEditor);
            // 
            // pgGrid
            // 
            this.pgGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(369, 2);
            this.pgGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(552, 32);
            this.pgGrid.TabIndex = 65;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(200, 100);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "myRichEditControl1";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkRecordEdit,
            this.txtRecordEdit,
            this.txtRecorMax,
            this.btnRecordEdit,
            this.editFecha,
            this.editPeriodo,
            this.cmbRank,
            this.richTextEdit,
            this.editSpin,
            this.riPopup,
            this.imgEdit,
            this.editPorc,
            this.editValor});
            // 
            // chkRecordEdit
            // 
            this.chkRecordEdit.Caption = "";
            this.chkRecordEdit.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.chkRecordEdit.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.chkRecordEdit.Name = "chkRecordEdit";
            this.chkRecordEdit.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // txtRecordEdit
            // 
            this.txtRecordEdit.Name = "txtRecordEdit";
            this.txtRecordEdit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtRecordEdit_EditValueChanging);
            this.txtRecordEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRecordEdit_KeyPress);
            // 
            // txtRecorMax
            // 
            this.txtRecorMax.Name = "txtRecorMax";
            // 
            // btnRecordEdit
            // 
            this.btnRecordEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.BottomCenter, ((System.Drawing.Image)(resources.GetObject("btnRecordEdit.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnRecordEdit.Name = "btnRecordEdit";
            this.btnRecordEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnRecordEdit_ButtonClick);
            this.btnRecordEdit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.btnRecordEdit_EditValueChanging);
            // 
            // editFecha
            // 
            this.editFecha.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.editFecha.AppearanceDropDown.Options.UseBackColor = true;
            this.editFecha.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editFecha.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editFecha.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editFecha.EditFormat.FormatString = "dd/MM/yyyy";
            this.editFecha.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editFecha.Mask.EditMask = "dd/MM/yyyy";
            this.editFecha.Name = "editFecha";
            this.editFecha.Leave += new System.EventHandler(this.editFecha_Leave);
            // 
            // editPeriodo
            // 
            this.editPeriodo.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.editPeriodo.AppearanceDropDown.Options.UseBackColor = true;
            this.editPeriodo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editPeriodo.DisplayFormat.FormatString = "yyyy/MM";
            this.editPeriodo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editPeriodo.EditFormat.FormatString = "yyyy/MM";
            this.editPeriodo.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editPeriodo.Mask.EditMask = "yyyy/MM";
            this.editPeriodo.Name = "editPeriodo";
            this.editPeriodo.Popup += new System.EventHandler(this.editPeriodo_Popup);
            this.editPeriodo.Leave += new System.EventHandler(this.editFecha_Leave);
            // 
            // cmbRank
            // 
            this.cmbRank.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.cmbRank.AppearanceDropDown.Options.UseBackColor = true;
            this.cmbRank.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbRank.Name = "cmbRank";
            this.cmbRank.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.cmbRank_DrawItem);
            // 
            // richTextEdit
            // 
            this.richTextEdit.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.Rtf;
            this.richTextEdit.Name = "richTextEdit";
            this.richTextEdit.ShowCaretInReadOnly = false;
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // riPopup
            // 
            this.riPopup.AutoHeight = false;
            this.riPopup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riPopup.Name = "riPopup";
            this.riPopup.PopupControl = this.popupContainerControl;
            this.riPopup.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.riPopup_QueryResultValue);
            this.riPopup.QueryDisplayText += new DevExpress.XtraEditors.Controls.QueryDisplayTextEventHandler(this.riPopup_QueryDisplayText);
            this.riPopup.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.riPopup_QueryPopUp);
            // 
            // popupContainerControl
            // 
            this.popupContainerControl.Controls.Add(this.richEditControl);
            this.popupContainerControl.Location = new System.Drawing.Point(31, 323);
            this.popupContainerControl.Name = "popupContainerControl";
            this.popupContainerControl.Size = new System.Drawing.Size(200, 100);
            this.popupContainerControl.TabIndex = 1;
            // 
            // imgEdit
            // 
            this.imgEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imgEdit.Name = "imgEdit";
            // 
            // editPorc
            // 
            this.editPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editPorc.Mask.EditMask = "P7";
            this.editPorc.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editPorc.Name = "editPorc";
            // 
            // editValor
            // 
            this.editValor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValor.Mask.EditMask = "c0";
            this.editValor.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValor.Mask.UseMaskAsDisplayFormat = true;
            this.editValor.Name = "editValor";
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 482);
            this.Controls.Add(this.pnlModule);
            this.Controls.Add(this.popupContainerControl);
            this.Name = "MasterForm";
            this.Text = "1004";
            this.pnlModule.ResumeLayout(false);
            this.pnGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolModule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvModule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnSearch)).EndInit();
            this.pnSearch.ResumeLayout(false);
            this.pnSearch.PerformLayout();
            this.pnlRecordEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabRecordEdit)).EndInit();
            this.tabRecordEdit.ResumeLayout(false);
            this.tpRecordEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grlControlRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecorMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFecha.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editFecha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPeriodo.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPeriodo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbRank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl)).EndInit();
            this.popupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel pnlModule;
        protected System.Windows.Forms.Panel pnlRecordEdit;
        protected DevExpress.XtraTab.XtraTabControl tabRecordEdit;
        protected DevExpress.XtraTab.XtraTabPage tpRecordEdit;
        protected DevExpress.XtraGrid.GridControl grlControlRecordEdit;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvRecordEdit;
        protected ControlsUC.uc_Pagging pgGrid;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkRecordEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtRecordEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtRecorMax;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editFecha;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editPeriodo;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnRecordEdit;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbRank;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        private DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraEditors.PopupContainerControl popupContainerControl;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit imgEdit;
        protected DevExpress.XtraGrid.GridControl grlcontrolModule;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvModule;
        private System.Windows.Forms.Panel pnGrid;
        protected DevExpress.XtraEditors.GroupControl pnSearch;
        private DevExpress.XtraEditors.LabelControl lblDescrip;
        private DevExpress.XtraEditors.LabelControl lblCode;
        protected System.Windows.Forms.TextBox txtDescrip;
        protected System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnSearch;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editPorc;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValorInt;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValor;

    }
}