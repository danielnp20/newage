namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MDI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDI));
            this.dmLeftContainer = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.pnlLeftContainer = new DevExpress.XtraBars.Docking.DockPanel();
            this.dpOperations = new DevExpress.XtraBars.Docking.DockPanel();
            this.pnlOperations = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tvOperations = new System.Windows.Forms.TreeView();
            this.IconList = new System.Windows.Forms.ImageList(this.components);
            this.dpModules = new DevExpress.XtraBars.Docking.DockPanel();
            this.pnlModules = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tvModules = new System.Windows.Forms.TreeView();
            this.tabForms = new DevExpress.XtraTab.XtraTabControl();
            this.tbMaster = new System.Windows.Forms.ToolStrip();
            this.itemNew = new System.Windows.Forms.ToolStripButton();
            this.itemEdit = new System.Windows.Forms.ToolStripButton();
            this.itemSave = new System.Windows.Forms.ToolStripButton();
            this.itemDelete = new System.Windows.Forms.ToolStripButton();
            this.tbBreak = new System.Windows.Forms.ToolStripSeparator();
            this.itemSearch = new System.Windows.Forms.ToolStripButton();
            this.itemFilter = new System.Windows.Forms.ToolStripButton();
            this.itemFilterDef = new System.Windows.Forms.ToolStripButton();
            this.itemPrint = new System.Windows.Forms.ToolStripButton();
            this.tbBreak0 = new System.Windows.Forms.ToolStripSeparator();
            this.itemGenerateTemplate = new System.Windows.Forms.ToolStripButton();
            this.itemCopy = new System.Windows.Forms.ToolStripButton();
            this.itemPaste = new System.Windows.Forms.ToolStripButton();
            this.itemImport = new System.Windows.Forms.ToolStripButton();
            this.itemExport = new System.Windows.Forms.ToolStripButton();
            this.tbBreak1 = new System.Windows.Forms.ToolStripSeparator();
            this.itemUpdate = new System.Windows.Forms.ToolStripButton();
            this.itemResetPassword = new System.Windows.Forms.ToolStripButton();
            this.itemRevert = new System.Windows.Forms.ToolStripButton();
            this.itemSendtoAppr = new System.Windows.Forms.ToolStripButton();
            this.tbBreak2 = new System.Windows.Forms.ToolStripSeparator();
            this.itemAlarm = new System.Windows.Forms.ToolStripButton();
            this.itemHelp = new System.Windows.Forms.ToolStripButton();
            this.itemClose = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusLAN = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusUserTit = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCancelStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusCompTit = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusCompany = new System.Windows.Forms.ToolStripStatusLabel();
            this.ddlStatusCompany = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnResetSecurity = new System.Windows.Forms.ToolStripDropDownButton();
            this.aaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dmLeftContainer)).BeginInit();
            this.pnlLeftContainer.SuspendLayout();
            this.dpOperations.SuspendLayout();
            this.pnlOperations.SuspendLayout();
            this.dpModules.SuspendLayout();
            this.pnlModules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabForms)).BeginInit();
            this.tbMaster.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dmLeftContainer
            // 
            this.dmLeftContainer.AutoHideSpeed = 2;
            this.dmLeftContainer.DockingOptions.HideImmediatelyOnAutoHide = true;
            this.dmLeftContainer.DockingOptions.ShowCloseButton = false;
            this.dmLeftContainer.Form = this;
            this.dmLeftContainer.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.pnlLeftContainer});
            this.dmLeftContainer.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // pnlLeftContainer
            // 
            this.pnlLeftContainer.ActiveChild = this.dpOperations;
            this.pnlLeftContainer.Controls.Add(this.dpModules);
            this.pnlLeftContainer.Controls.Add(this.dpOperations);
            this.pnlLeftContainer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.pnlLeftContainer.FloatVertical = true;
            this.pnlLeftContainer.ID = new System.Guid("934f7a06-0ad8-4d4f-a74d-bdd48a658409");
            this.pnlLeftContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftContainer.Name = "pnlLeftContainer";
            this.pnlLeftContainer.OriginalSize = new System.Drawing.Size(200, 200);
            this.pnlLeftContainer.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.pnlLeftContainer.Size = new System.Drawing.Size(200, 392);
            this.pnlLeftContainer.Tabbed = true;
            this.pnlLeftContainer.Text = "pnlLeftContainer";
            this.pnlLeftContainer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            // 
            // dpOperations
            // 
            this.dpOperations.Controls.Add(this.pnlOperations);
            this.dpOperations.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dpOperations.FloatVertical = true;
            this.dpOperations.ID = new System.Guid("5c95c73c-22c1-40c0-a01c-76368f089834");
            this.dpOperations.Location = new System.Drawing.Point(4, 23);
            this.dpOperations.Name = "dpOperations";
            this.dpOperations.OriginalSize = new System.Drawing.Size(200, 200);
            this.dpOperations.Size = new System.Drawing.Size(192, 338);
            this.dpOperations.Text = "1000_dpOperations";
            // 
            // pnlOperations
            // 
            this.pnlOperations.Controls.Add(this.tvOperations);
            this.pnlOperations.Location = new System.Drawing.Point(0, 0);
            this.pnlOperations.Name = "pnlOperations";
            this.pnlOperations.Size = new System.Drawing.Size(192, 338);
            this.pnlOperations.TabIndex = 0;
            // 
            // tvOperations
            // 
            this.tvOperations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOperations.ImageIndex = 0;
            this.tvOperations.ImageList = this.IconList;
            this.tvOperations.Location = new System.Drawing.Point(0, 0);
            this.tvOperations.Name = "tvOperations";
            this.tvOperations.SelectedImageIndex = 0;
            this.tvOperations.Size = new System.Drawing.Size(192, 338);
            this.tvOperations.TabIndex = 1;
            this.tvOperations.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvOperations_NodeMouseClick);
            this.tvOperations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvOperations_KeyDown);
            // 
            // IconList
            // 
            this.IconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconList.ImageStream")));
            this.IconList.TransparentColor = System.Drawing.Color.Transparent;
            this.IconList.Images.SetKeyName(0, "TBIconFolder.ico");
            this.IconList.Images.SetKeyName(1, "TBIconPage.ico");
            this.IconList.Images.SetKeyName(2, "FindFkHierarchy.png");
            this.IconList.Images.SetKeyName(3, "ModalFirstPage.png");
            this.IconList.Images.SetKeyName(4, "TBIconCopy.ico");
            this.IconList.Images.SetKeyName(5, "TBIconDelete.ico");
            this.IconList.Images.SetKeyName(6, "TBIconExit.ico");
            this.IconList.Images.SetKeyName(7, "TBIconFilter.ico");
            this.IconList.Images.SetKeyName(8, "TBiconHelp.ico");
            this.IconList.Images.SetKeyName(9, "ModalLastPage.png");
            this.IconList.Images.SetKeyName(10, "TBIconNew.ico");
            this.IconList.Images.SetKeyName(11, "TBIconPaste.ico");
            this.IconList.Images.SetKeyName(12, "TBIconPrint.ico");
            this.IconList.Images.SetKeyName(13, "TBIconSave.ico");
            this.IconList.Images.SetKeyName(14, "ModalNextPage.png");
            this.IconList.Images.SetKeyName(15, "ModalPreviewPage.png");
            this.IconList.Images.SetKeyName(16, "TBIconCanceltx.ico");
            this.IconList.Images.SetKeyName(17, "TBIconEdit.ico");
            this.IconList.Images.SetKeyName(18, "TBIconExport.ico");
            this.IconList.Images.SetKeyName(19, "TBIconFilterDef.ico");
            this.IconList.Images.SetKeyName(20, "TBIconImport.ico");
            this.IconList.Images.SetKeyName(21, "TBIconPending.ico");
            this.IconList.Images.SetKeyName(22, "TBIconExportExcel.ico");
            // 
            // dpModules
            // 
            this.dpModules.Controls.Add(this.pnlModules);
            this.dpModules.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dpModules.ID = new System.Guid("8807cd44-3063-4c4f-9f86-f831278e4b20");
            this.dpModules.Location = new System.Drawing.Point(4, 23);
            this.dpModules.Name = "dpModules";
            this.dpModules.OriginalSize = new System.Drawing.Size(173, 200);
            this.dpModules.Size = new System.Drawing.Size(192, 338);
            this.dpModules.Text = "1000_dpModules";
            // 
            // pnlModules
            // 
            this.pnlModules.Controls.Add(this.tvModules);
            this.pnlModules.Location = new System.Drawing.Point(0, 0);
            this.pnlModules.Name = "pnlModules";
            this.pnlModules.Size = new System.Drawing.Size(192, 338);
            this.pnlModules.TabIndex = 0;
            // 
            // tvModules
            // 
            this.tvModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvModules.ImageIndex = 0;
            this.tvModules.ImageList = this.IconList;
            this.tvModules.Location = new System.Drawing.Point(0, 0);
            this.tvModules.Name = "tvModules";
            this.tvModules.SelectedImageIndex = 0;
            this.tvModules.Size = new System.Drawing.Size(192, 338);
            this.tvModules.TabIndex = 1;
            this.tvModules.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvModules_NodeMouseClick);
            this.tvModules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvModules_KeyDown);
            // 
            // tabForms
            // 
            this.tabForms.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabForms.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.tabForms.Appearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.tabForms.Appearance.Options.UseBackColor = true;
            this.tabForms.Appearance.Options.UseBorderColor = true;
            this.tabForms.AppearancePage.Header.BackColor = System.Drawing.Color.MidnightBlue;
            this.tabForms.AppearancePage.Header.ForeColor = System.Drawing.Color.White;
            this.tabForms.AppearancePage.Header.Options.UseBackColor = true;
            this.tabForms.AppearancePage.Header.Options.UseForeColor = true;
            this.tabForms.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.Khaki;
            this.tabForms.AppearancePage.HeaderActive.BackColor2 = System.Drawing.Color.Khaki;
            this.tabForms.AppearancePage.HeaderActive.BorderColor = System.Drawing.Color.Khaki;
            this.tabForms.AppearancePage.HeaderActive.ForeColor = System.Drawing.Color.Black;
            this.tabForms.AppearancePage.HeaderActive.Options.UseBackColor = true;
            this.tabForms.AppearancePage.HeaderActive.Options.UseBorderColor = true;
            this.tabForms.AppearancePage.HeaderActive.Options.UseForeColor = true;
            this.tabForms.AppearancePage.HeaderDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tabForms.AppearancePage.HeaderDisabled.Options.UseBackColor = true;
            this.tabForms.AppearancePage.HeaderHotTracked.BackColor = System.Drawing.Color.Gray;
            this.tabForms.AppearancePage.HeaderHotTracked.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabForms.AppearancePage.HeaderHotTracked.Options.UseBackColor = true;
            this.tabForms.AppearancePage.HeaderHotTracked.Options.UseBorderColor = true;
            this.tabForms.AppearancePage.PageClient.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.tabForms.AppearancePage.PageClient.Options.UseBackColor = true;
            this.tabForms.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tabForms.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tabForms.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            this.tabForms.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabForms.Location = new System.Drawing.Point(0, 0);
            this.tabForms.MinimumSize = new System.Drawing.Size(1000, 0);
            this.tabForms.Name = "tabForms";
            this.tabForms.PaintStyleName = "PropertyView";
            this.tabForms.Size = new System.Drawing.Size(1165, 22);
            this.tabForms.TabIndex = 40;
            this.tabForms.Visible = false;
            this.tabForms.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabForms_SelectedPageChanged);
            this.tabForms.CloseButtonClick += new System.EventHandler(this.tabForms_CloseButtonClick);
            // 
            // tbMaster
            // 
            this.tbMaster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemNew,
            this.itemEdit,
            this.itemSave,
            this.itemDelete,
            this.tbBreak,
            this.itemSearch,
            this.itemFilter,
            this.itemFilterDef,
            this.itemPrint,
            this.tbBreak0,
            this.itemGenerateTemplate,
            this.itemCopy,
            this.itemPaste,
            this.itemImport,
            this.itemExport,
            this.tbBreak1,
            this.itemUpdate,
            this.itemResetPassword,
            this.itemRevert,
            this.itemSendtoAppr,
            this.tbBreak2,
            this.itemAlarm,
            this.itemHelp,
            this.itemClose});
            this.tbMaster.Location = new System.Drawing.Point(0, 22);
            this.tbMaster.Name = "tbMaster";
            this.tbMaster.Size = new System.Drawing.Size(1165, 25);
            this.tbMaster.TabIndex = 50;
            // 
            // itemNew
            // 
            this.itemNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemNew.Image = ((System.Drawing.Image)(resources.GetObject("itemNew.Image")));
            this.itemNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemNew.Name = "itemNew";
            this.itemNew.Size = new System.Drawing.Size(23, 22);
            this.itemNew.Text = "acc_new";
            this.itemNew.Click += new System.EventHandler(this.itemNew_Click);
            // 
            // itemEdit
            // 
            this.itemEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemEdit.Image = ((System.Drawing.Image)(resources.GetObject("itemEdit.Image")));
            this.itemEdit.Name = "itemEdit";
            this.itemEdit.Size = new System.Drawing.Size(23, 22);
            this.itemEdit.Text = "acc_edit";
            this.itemEdit.Click += new System.EventHandler(this.itemEdit_Click);
            // 
            // itemSave
            // 
            this.itemSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemSave.Image = ((System.Drawing.Image)(resources.GetObject("itemSave.Image")));
            this.itemSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemSave.Name = "itemSave";
            this.itemSave.Size = new System.Drawing.Size(23, 22);
            this.itemSave.Text = "acc_save";
            this.itemSave.Click += new System.EventHandler(this.itemSave_Click);
            // 
            // itemDelete
            // 
            this.itemDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemDelete.Image = ((System.Drawing.Image)(resources.GetObject("itemDelete.Image")));
            this.itemDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemDelete.Name = "itemDelete";
            this.itemDelete.Size = new System.Drawing.Size(23, 22);
            this.itemDelete.Text = "acc_delete";
            this.itemDelete.Click += new System.EventHandler(this.itemDelete_Click);
            // 
            // tbBreak
            // 
            this.tbBreak.Name = "tbBreak";
            this.tbBreak.Size = new System.Drawing.Size(6, 25);
            // 
            // itemSearch
            // 
            this.itemSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemSearch.Image = ((System.Drawing.Image)(resources.GetObject("itemSearch.Image")));
            this.itemSearch.Name = "itemSearch";
            this.itemSearch.Size = new System.Drawing.Size(23, 22);
            this.itemSearch.Text = "acc_search";
            this.itemSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.itemSearch.Click += new System.EventHandler(this.itemSearch_Click);
            // 
            // itemFilter
            // 
            this.itemFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemFilter.Image = ((System.Drawing.Image)(resources.GetObject("itemFilter.Image")));
            this.itemFilter.Name = "itemFilter";
            this.itemFilter.Size = new System.Drawing.Size(23, 22);
            this.itemFilter.Text = "acc_filter";
            this.itemFilter.Click += new System.EventHandler(this.itemFilter_Click);
            // 
            // itemFilterDef
            // 
            this.itemFilterDef.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemFilterDef.Image = ((System.Drawing.Image)(resources.GetObject("itemFilterDef.Image")));
            this.itemFilterDef.Name = "itemFilterDef";
            this.itemFilterDef.Size = new System.Drawing.Size(23, 22);
            this.itemFilterDef.Text = "acc_filterdef";
            this.itemFilterDef.Click += new System.EventHandler(this.itemFilterDef_Click);
            // 
            // itemPrint
            // 
            this.itemPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemPrint.Image = ((System.Drawing.Image)(resources.GetObject("itemPrint.Image")));
            this.itemPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemPrint.Name = "itemPrint";
            this.itemPrint.Size = new System.Drawing.Size(23, 22);
            this.itemPrint.Text = "acc_print";
            this.itemPrint.Click += new System.EventHandler(this.itemPrint_Click);
            // 
            // tbBreak0
            // 
            this.tbBreak0.Name = "tbBreak0";
            this.tbBreak0.Size = new System.Drawing.Size(6, 25);
            // 
            // itemGenerateTemplate
            // 
            this.itemGenerateTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemGenerateTemplate.Image = ((System.Drawing.Image)(resources.GetObject("itemGenerateTemplate.Image")));
            this.itemGenerateTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemGenerateTemplate.Name = "itemGenerateTemplate";
            this.itemGenerateTemplate.Size = new System.Drawing.Size(23, 22);
            this.itemGenerateTemplate.Text = "acc_generatetemplate";
            this.itemGenerateTemplate.Click += new System.EventHandler(this.itemGenerateTemplate_Click);
            // 
            // itemCopy
            // 
            this.itemCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemCopy.Image = ((System.Drawing.Image)(resources.GetObject("itemCopy.Image")));
            this.itemCopy.Name = "itemCopy";
            this.itemCopy.Size = new System.Drawing.Size(23, 22);
            this.itemCopy.Text = "acc_copy";
            this.itemCopy.Click += new System.EventHandler(this.itemCopy_Click);
            // 
            // itemPaste
            // 
            this.itemPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemPaste.Image = ((System.Drawing.Image)(resources.GetObject("itemPaste.Image")));
            this.itemPaste.Name = "itemPaste";
            this.itemPaste.Size = new System.Drawing.Size(23, 22);
            this.itemPaste.Text = "acc_paste";
            this.itemPaste.Click += new System.EventHandler(this.itemPaste_Click);
            // 
            // itemImport
            // 
            this.itemImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemImport.Image = ((System.Drawing.Image)(resources.GetObject("itemImport.Image")));
            this.itemImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemImport.Name = "itemImport";
            this.itemImport.Size = new System.Drawing.Size(23, 22);
            this.itemImport.Text = "acc_import";
            this.itemImport.Click += new System.EventHandler(this.itemImport_Click);
            // 
            // itemExport
            // 
            this.itemExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemExport.Image = ((System.Drawing.Image)(resources.GetObject("itemExport.Image")));
            this.itemExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemExport.Name = "itemExport";
            this.itemExport.Size = new System.Drawing.Size(23, 22);
            this.itemExport.Text = "acc_export";
            this.itemExport.Click += new System.EventHandler(this.itemExport_Click);
            // 
            // tbBreak1
            // 
            this.tbBreak1.Name = "tbBreak1";
            this.tbBreak1.Size = new System.Drawing.Size(6, 25);
            // 
            // itemUpdate
            // 
            this.itemUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemUpdate.Image = ((System.Drawing.Image)(resources.GetObject("itemUpdate.Image")));
            this.itemUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemUpdate.Name = "itemUpdate";
            this.itemUpdate.Size = new System.Drawing.Size(23, 22);
            this.itemUpdate.Text = "acc_update";
            this.itemUpdate.Click += new System.EventHandler(this.itemUpdate_Click);
            // 
            // itemResetPassword
            // 
            this.itemResetPassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemResetPassword.Image = ((System.Drawing.Image)(resources.GetObject("itemResetPassword.Image")));
            this.itemResetPassword.Name = "itemResetPassword";
            this.itemResetPassword.Size = new System.Drawing.Size(23, 22);
            this.itemResetPassword.Text = "acc_resetPwd";
            this.itemResetPassword.Click += new System.EventHandler(this.itemResetPassword_Click);
            // 
            // itemRevert
            // 
            this.itemRevert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemRevert.Image = ((System.Drawing.Image)(resources.GetObject("itemRevert.Image")));
            this.itemRevert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemRevert.Name = "itemRevert";
            this.itemRevert.Size = new System.Drawing.Size(23, 22);
            this.itemRevert.Text = "acc_revert";
            this.itemRevert.Click += new System.EventHandler(this.itemRevert_Click);
            // 
            // itemSendtoAppr
            // 
            this.itemSendtoAppr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemSendtoAppr.Image = ((System.Drawing.Image)(resources.GetObject("itemSendtoAppr.Image")));
            this.itemSendtoAppr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemSendtoAppr.Name = "itemSendtoAppr";
            this.itemSendtoAppr.Size = new System.Drawing.Size(23, 22);
            this.itemSendtoAppr.Text = "acc_sendtoAppr";
            this.itemSendtoAppr.Click += new System.EventHandler(this.itemSendtoAppr_Click);
            // 
            // tbBreak2
            // 
            this.tbBreak2.Name = "tbBreak2";
            this.tbBreak2.Size = new System.Drawing.Size(6, 25);
            // 
            // itemAlarm
            // 
            this.itemAlarm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemAlarm.Image = ((System.Drawing.Image)(resources.GetObject("itemAlarm.Image")));
            this.itemAlarm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemAlarm.Name = "itemAlarm";
            this.itemAlarm.Size = new System.Drawing.Size(23, 22);
            this.itemAlarm.Text = "acc_alarm";
            this.itemAlarm.Click += new System.EventHandler(this.itemAlarm_Click);
            // 
            // itemHelp
            // 
            this.itemHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemHelp.Image = ((System.Drawing.Image)(resources.GetObject("itemHelp.Image")));
            this.itemHelp.Name = "itemHelp";
            this.itemHelp.Size = new System.Drawing.Size(23, 22);
            this.itemHelp.Text = "acc_help";
            this.itemHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.itemHelp.Click += new System.EventHandler(this.itemHelp_Click);
            // 
            // itemClose
            // 
            this.itemClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemClose.Image = ((System.Drawing.Image)(resources.GetObject("itemClose.Image")));
            this.itemClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemClose.Name = "itemClose";
            this.itemClose.Size = new System.Drawing.Size(23, 22);
            this.itemClose.Text = "acc_close";
            this.itemClose.Click += new System.EventHandler(this.itemClose_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.AllowItemReorder = true;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusLAN,
            this.lblStatusUserTit,
            this.lblStatusUser,
            this.pbStatus,
            this.lblStatusMsg,
            this.lblCancelStatusBar,
            this.lblStatusCompTit,
            this.lblStatusCompany,
            this.ddlStatusCompany,
            this.btnResetSecurity});
            this.statusStrip.Location = new System.Drawing.Point(0, 368);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1165, 24);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 53;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatusLAN
            // 
            this.lblStatusLAN.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatusLAN.Name = "lblStatusLAN";
            this.lblStatusLAN.Size = new System.Drawing.Size(75, 19);
            this.lblStatusLAN.Text = "lblStatusLAN";
            // 
            // lblStatusUserTit
            // 
            this.lblStatusUserTit.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblStatusUserTit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusUserTit.Margin = new System.Windows.Forms.Padding(30, 3, 0, 2);
            this.lblStatusUserTit.Name = "lblStatusUserTit";
            this.lblStatusUserTit.Size = new System.Drawing.Size(67, 19);
            this.lblStatusUserTit.Text = "lblUserTit";
            this.lblStatusUserTit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatusUserTit.Visible = false;
            // 
            // lblStatusUser
            // 
            this.lblStatusUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusUser.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(46, 19);
            this.lblStatusUser.Text = "lblUser";
            this.lblStatusUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusUser.Visible = false;
            // 
            // pbStatus
            // 
            this.pbStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pbStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.pbStatus.Margin = new System.Windows.Forms.Padding(10, 3, 1, 3);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(100, 18);
            this.pbStatus.Visible = false;
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.AutoSize = false;
            this.lblStatusMsg.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(197, 19);
            this.lblStatusMsg.Spring = true;
            // 
            // lblCancelStatusBar
            // 
            this.lblCancelStatusBar.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblCancelStatusBar.Margin = new System.Windows.Forms.Padding(0, 3, 120, 2);
            this.lblCancelStatusBar.Name = "lblCancelStatusBar";
            this.lblCancelStatusBar.Size = new System.Drawing.Size(79, 19);
            this.lblCancelStatusBar.Text = "1000_lblCancel";
            this.lblCancelStatusBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCancelStatusBar.Visible = false;
            this.lblCancelStatusBar.Click += new System.EventHandler(this.lblCancelStatusBar_Click);
            this.lblCancelStatusBar.MouseEnter += new System.EventHandler(this.lblCancelStatusBar_MouseEnter);
            this.lblCancelStatusBar.MouseLeave += new System.EventHandler(this.lblCancelStatusBar_MouseLeave);
            // 
            // lblStatusCompTit
            // 
            this.lblStatusCompTit.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblStatusCompTit.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblStatusCompTit.Name = "lblStatusCompTit";
            this.lblStatusCompTit.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblStatusCompTit.Size = new System.Drawing.Size(94, 19);
            this.lblStatusCompTit.Text = "lblCompanyTit";
            this.lblStatusCompTit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusCompTit.Visible = false;
            // 
            // lblStatusCompany
            // 
            this.lblStatusCompany.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatusCompany.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblStatusCompany.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatusCompany.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lblStatusCompany.Name = "lblStatusCompany";
            this.lblStatusCompany.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblStatusCompany.Size = new System.Drawing.Size(197, 19);
            this.lblStatusCompany.Spring = true;
            this.lblStatusCompany.Text = "lblStatusCompany";
            this.lblStatusCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusCompany.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.lblStatusCompany.Visible = false;
            // 
            // ddlStatusCompany
            // 
            this.ddlStatusCompany.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddlStatusCompany.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.ddlStatusCompany.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ddlStatusCompany.Margin = new System.Windows.Forms.Padding(10, 2, 0, 0);
            this.ddlStatusCompany.Name = "ddlStatusCompany";
            this.ddlStatusCompany.Size = new System.Drawing.Size(73, 22);
            this.ddlStatusCompany.Text = "Company";
            this.ddlStatusCompany.Visible = false;
            this.ddlStatusCompany.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.UpdateCompany_Click);
            // 
            // btnResetSecurity
            // 
            this.btnResetSecurity.AutoToolTip = false;
            this.btnResetSecurity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnResetSecurity.Image = global::NewAge.Properties.Resources.TBResetPwd1;
            this.btnResetSecurity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetSecurity.Name = "btnResetSecurity";
            this.btnResetSecurity.ShowDropDownArrow = false;
            this.btnResetSecurity.Size = new System.Drawing.Size(20, 22);
            this.btnResetSecurity.ToolTipText = "Actualizar Seguridades";
            this.btnResetSecurity.Visible = false;
            this.btnResetSecurity.Click += new System.EventHandler(this.btnResetSecurity_Click);
            // 
            // aaaToolStripMenuItem
            // 
            this.aaaToolStripMenuItem.Name = "aaaToolStripMenuItem";
            this.aaaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aaaToolStripMenuItem.Text = "aaa";
            // 
            // MDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1165, 392);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tbMaster);
            this.Controls.Add(this.tabForms);
            this.Controls.Add(this.pnlLeftContainer);
            this.IsMdiContainer = true;
            this.Name = "MDI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Master";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MDI_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.dmLeftContainer)).EndInit();
            this.pnlLeftContainer.ResumeLayout(false);
            this.dpOperations.ResumeLayout(false);
            this.pnlOperations.ResumeLayout(false);
            this.dpModules.ResumeLayout(false);
            this.pnlModules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabForms)).EndInit();
            this.tbMaster.ResumeLayout(false);
            this.tbMaster.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dmLeftContainer;
        internal DevExpress.XtraBars.Docking.DockPanel dpModules;
        private DevExpress.XtraBars.Docking.ControlContainer pnlModules;
        internal System.Windows.Forms.TreeView tvModules;
        internal DevExpress.XtraTab.XtraTabControl tabForms;
        internal DevExpress.XtraBars.Docking.DockPanel dpOperations;
        private DevExpress.XtraBars.Docking.ControlContainer pnlOperations;
        internal System.Windows.Forms.TreeView tvOperations;
        internal DevExpress.XtraBars.Docking.DockPanel pnlLeftContainer;
        private System.Windows.Forms.ImageList IconList;
        internal System.Windows.Forms.ToolStrip tbMaster;
        public System.Windows.Forms.ToolStripButton itemNew;
        public System.Windows.Forms.ToolStripButton itemEdit;
        public System.Windows.Forms.ToolStripButton itemSave;
        public System.Windows.Forms.ToolStripButton itemDelete;
        public System.Windows.Forms.ToolStripSeparator tbBreak0;
        public System.Windows.Forms.ToolStripButton itemSearch;
        public System.Windows.Forms.ToolStripButton itemFilter;
        public System.Windows.Forms.ToolStripButton itemFilterDef;
        public System.Windows.Forms.ToolStripSeparator tbBreak1;
        public System.Windows.Forms.ToolStripButton itemPrint;
        public System.Windows.Forms.ToolStripButton itemImport;
        public System.Windows.Forms.ToolStripButton itemExport;
        public System.Windows.Forms.ToolStripButton itemCopy;
        public System.Windows.Forms.ToolStripButton itemPaste;
        public System.Windows.Forms.ToolStripSeparator tbBreak2;
        public System.Windows.Forms.ToolStripButton itemResetPassword;
        public System.Windows.Forms.ToolStripSeparator tbBreak;
        public System.Windows.Forms.ToolStripButton itemHelp;
        public System.Windows.Forms.ToolStripButton itemClose;
        public System.Windows.Forms.ToolStripButton itemGenerateTemplate;
        private System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        public System.Windows.Forms.ToolStripDropDownButton ddlStatusCompany;
        public System.Windows.Forms.ToolStripProgressBar pbStatus;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusCompany;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusLAN;
        private System.Windows.Forms.ToolStripMenuItem aaaToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusMsg;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusUserTit;
        public System.Windows.Forms.ToolStripStatusLabel lblStatusCompTit;
        public System.Windows.Forms.ToolStripButton itemUpdate;
        public System.Windows.Forms.ToolStripButton itemRevert;
        public System.Windows.Forms.ToolStripButton itemSendtoAppr;
        internal System.Windows.Forms.ToolStripStatusLabel lblCancelStatusBar;
        private System.Windows.Forms.ToolStripButton itemAlarm;
        private System.Windows.Forms.ToolStripDropDownButton btnResetSecurity;
    }
}