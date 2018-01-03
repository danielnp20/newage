using DevExpress.Utils;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Controls;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class seGrupoDocumento
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
            this.pnlModule = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGroup = new System.Windows.Forms.Panel();
            this.grlControlGroup = new DevExpress.XtraGrid.GridControl();
            this.gvGroup = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tcSecurity = new DevExpress.XtraTab.XtraTabControl();
            this.tpMaster = new DevExpress.XtraTab.XtraTabPage();
            this.gcMaster = new DevExpress.XtraGrid.GridControl();
            this.gvMaster = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpProcess = new DevExpress.XtraTab.XtraTabPage();
            this.gcProcess = new DevExpress.XtraGrid.GridControl();
            this.gvProcess = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpBitacora = new DevExpress.XtraTab.XtraTabPage();
            this.gcBitacora = new DevExpress.XtraGrid.GridControl();
            this.gvBitacora = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpReport = new DevExpress.XtraTab.XtraTabPage();
            this.gcReport = new DevExpress.XtraGrid.GridControl();
            this.gvReport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpQuery = new DevExpress.XtraTab.XtraTabPage();
            this.gcQuery = new DevExpress.XtraGrid.GridControl();
            this.gvQuery = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpDocument = new DevExpress.XtraTab.XtraTabPage();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpDocumentAprob = new DevExpress.XtraTab.XtraTabPage();
            this.gcDocumentAprob = new DevExpress.XtraGrid.GridControl();
            this.gvDocumentAprob = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpControl = new DevExpress.XtraTab.XtraTabPage();
            this.gcControl = new DevExpress.XtraGrid.GridControl();
            this.gvControl = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tpActivities = new DevExpress.XtraTab.XtraTabPage();
            this.gcActivities = new DevExpress.XtraGrid.GridControl();
            this.gvActivities = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.chkRecordEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.txtRecordEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.pnlModule.SuspendLayout();
            this.pnlGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcSecurity)).BeginInit();
            this.tcSecurity.SuspendLayout();
            this.tpMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMaster)).BeginInit();
            this.tpProcess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProcess)).BeginInit();
            this.tpBitacora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBitacora)).BeginInit();
            this.tpReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).BeginInit();
            this.tpQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).BeginInit();
            this.tpDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            this.tpDocumentAprob.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocumentAprob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocumentAprob)).BeginInit();
            this.tpControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvControl)).BeginInit();
            this.tpActivities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcActivities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvActivities)).BeginInit();

            ((System.ComponentModel.ISupportInitialize)(this.chkRecordEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlModule
            // 
            this.pnlModule.ColumnCount = 1;
            this.pnlModule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlModule.Controls.Add(this.pnlGroup, 0, 0);
            this.pnlModule.Controls.Add(this.tcSecurity, 0, 1);
            this.pnlModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModule.Location = new System.Drawing.Point(0, 0);
            this.pnlModule.Name = "pnlModule";
            this.pnlModule.RowCount = 3;
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.59548F));
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.40452F));
            this.pnlModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.pnlModule.Size = new System.Drawing.Size(856, 487);
            this.pnlModule.TabIndex = 66;
            // 
            // pnlGroup
            // 
            this.pnlGroup.BackColor = System.Drawing.Color.GhostWhite;
            this.pnlGroup.Controls.Add(this.grlControlGroup);
            this.pnlGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGroup.Location = new System.Drawing.Point(3, 3);
            this.pnlGroup.Name = "pnlGroup";
            this.pnlGroup.Size = new System.Drawing.Size(850, 140);
            this.pnlGroup.TabIndex = 56;
            // 
            // grlControlGroup
            // 
            this.grlControlGroup.AllowDrop = true;
            this.grlControlGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.grlControlGroup.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grlControlGroup.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grlControlGroup.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.grlControlGroup.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.grlControlGroup.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.grlControlGroup.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.grlControlGroup.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.grlControlGroup.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grlControlGroup.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.grlControlGroup.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.grlControlGroup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grlControlGroup.Location = new System.Drawing.Point(0, 0);
            this.grlControlGroup.MainView = this.gvGroup;
            this.grlControlGroup.Margin = new System.Windows.Forms.Padding(4);
            this.grlControlGroup.Name = "grlControlGroup";
            this.grlControlGroup.Size = new System.Drawing.Size(465, 140);
            this.grlControlGroup.TabIndex = 1;
            this.grlControlGroup.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGroup});
            // 
            // gvGroup
            // 
            this.gvGroup.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGroup.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvGroup.Appearance.FocusedRow.BackColor = System.Drawing.Color.Lavender;
            this.gvGroup.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvGroup.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGroup.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvGroup.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvGroup.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvGroup.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvGroup.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvGroup.Appearance.Row.Options.UseBackColor = true;
            this.gvGroup.Appearance.Row.Options.UseTextOptions = true;
            this.gvGroup.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvGroup.GridControl = this.grlControlGroup;
            this.gvGroup.HorzScrollStep = 50;
            this.gvGroup.Name = "gvGroup";
            this.gvGroup.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvGroup.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvGroup.OptionsBehavior.AutoPopulateColumns = false;
            this.gvGroup.OptionsBehavior.Editable = false;
            this.gvGroup.OptionsCustomization.AllowFilter = false;
            this.gvGroup.OptionsCustomization.AllowSort = false;
            this.gvGroup.OptionsFind.AllowFindPanel = false;
            this.gvGroup.OptionsMenu.EnableColumnMenu = false;
            this.gvGroup.OptionsMenu.EnableFooterMenu = false;
            this.gvGroup.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvGroup.OptionsView.ColumnAutoWidth = false;
            this.gvGroup.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvGroup.OptionsView.ShowGroupPanel = false;
            this.gvGroup.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gvGroup.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvGroup_FocusedRowChanged);
            this.gvGroup.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGroup_CustomUnboundColumnData);
            // 
            // tcSecurity
            // 
            this.tcSecurity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSecurity.Location = new System.Drawing.Point(3, 149);
            this.tcSecurity.Name = "tcSecurity";
            this.tcSecurity.SelectedTabPage = this.tpMaster;
            this.tcSecurity.Size = new System.Drawing.Size(850, 326);
            this.tcSecurity.TabIndex = 57;
            this.tcSecurity.SelectedPageChanging += new DevExpress.XtraTab.TabPageChangingEventHandler(this.gvSecurity_SelectedPageChanging);
            this.tcSecurity.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.gvSecurity_SelectedPageChanged);
            this.tcSecurity.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpMaster,
            this.tpProcess,
            this.tpBitacora,
            this.tpReport,
            this.tpQuery,
            this.tpDocument,
            this.tpDocumentAprob,
            this.tpControl, 
            this.tpActivities});
            // 
            // tpMasters
            // 
            this.tpMaster.Controls.Add(this.gcMaster);
            this.tpMaster.Name = "tpMaster";
            this.tpMaster.Size = new System.Drawing.Size(844, 300);
            this.tpMaster.Text = this.DocumentID + "_tpMaster";
            // 
            // gcMaster
            // 
            this.gcMaster.Location = new System.Drawing.Point(72, 87);
            this.gcMaster.MainView = this.gvMaster;
            this.gcMaster.Name = "gcMaster";
            this.gcMaster.Size = new System.Drawing.Size(400, 200);
            this.gcMaster.TabIndex = 0;
            this.gcMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMaster.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMaster});
            // 
            // gvMaster
            // 
            this.gvMaster.GridControl = this.gcMaster;
            this.gvMaster.Name = "gvMaster";
            this.gvMaster.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMaster.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvMaster.HorzScrollStep = 1;
            this.gvMaster.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvMaster.OptionsBehavior.AutoPopulateColumns = false;
            this.gvMaster.OptionsCustomization.AllowFilter = false;
            this.gvMaster.OptionsCustomization.AllowSort = false;
            this.gvMaster.OptionsCustomization.AllowColumnMoving = false;
            this.gvMaster.OptionsDetail.AllowZoomDetail = false;
            this.gvMaster.OptionsMenu.EnableColumnMenu = false;
            this.gvMaster.OptionsMenu.EnableFooterMenu = false;
            this.gvMaster.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvMaster.OptionsView.ColumnAutoWidth = false;
            this.gvMaster.OptionsView.ShowGroupPanel = false;
            this.gvMaster.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvMaster.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpProcess
            // 
            this.tpProcess.Controls.Add(this.gcProcess);
            this.tpProcess.Name = "tpProcess";
            this.tpProcess.Size = new System.Drawing.Size(844, 300);
            this.tpProcess.Text = this.DocumentID + "_tpProcess";
            // 
            // gcProcess
            // 
            this.gcProcess.Location = new System.Drawing.Point(196, 89);
            this.gcProcess.MainView = this.gvProcess;
            this.gcProcess.Name = "gcProcess";
            this.gcProcess.Size = new System.Drawing.Size(400, 200);
            this.gcProcess.TabIndex = 1;
            this.gcProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcProcess.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvProcess});
            // 
            // gvProcess
            // 
            this.gvProcess.GridControl = this.gcProcess;
            this.gvProcess.Name = "gvProcess";
            this.gvProcess.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProcess.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvProcess.HorzScrollStep = 1;
            this.gvProcess.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvProcess.OptionsCustomization.AllowFilter = false;
            this.gvProcess.OptionsCustomization.AllowSort = false;
            this.gvProcess.OptionsCustomization.AllowColumnMoving = false;
            this.gvProcess.OptionsDetail.AllowZoomDetail = false;
            this.gvProcess.OptionsMenu.EnableColumnMenu = false;
            this.gvProcess.OptionsMenu.EnableFooterMenu = false;
            this.gvProcess.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvProcess.OptionsView.ColumnAutoWidth = false;
            this.gvProcess.OptionsView.ShowGroupPanel = false;
            this.gvProcess.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvProcess.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpBitacora
            // 
            this.tpBitacora.Controls.Add(this.gcBitacora);
            this.tpBitacora.Name = "tpBitacora";
            this.tpBitacora.Size = new System.Drawing.Size(844, 300);
            this.tpBitacora.Text = this.DocumentID + "_tpBitacora";
            // 
            // gcBitacora
            // 
            this.gcBitacora.Location = new System.Drawing.Point(72, 87);
            this.gcBitacora.MainView = this.gvBitacora;
            this.gcBitacora.Name = "gcBitacora";
            this.gcBitacora.Size = new System.Drawing.Size(400, 200);
            this.gcBitacora.TabIndex = 2;
            this.gcBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBitacora.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBitacora});
            // 
            // gvBitacora
            // 
            this.gvBitacora.GridControl = this.gcBitacora;
            this.gvBitacora.Name = "gvBitacora";
            this.gvBitacora.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBitacora.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvBitacora.HorzScrollStep = 1;
            this.gvBitacora.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvBitacora.OptionsBehavior.AutoPopulateColumns = false;
            this.gvBitacora.OptionsCustomization.AllowFilter = false;
            this.gvBitacora.OptionsCustomization.AllowSort = false;
            this.gvBitacora.OptionsCustomization.AllowColumnMoving = false;
            this.gvBitacora.OptionsDetail.AllowZoomDetail = false;
            this.gvBitacora.OptionsMenu.EnableColumnMenu = false;
            this.gvBitacora.OptionsMenu.EnableFooterMenu = false;
            this.gvBitacora.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvBitacora.OptionsView.ColumnAutoWidth = false;
            this.gvBitacora.OptionsView.ShowGroupPanel = false;
            this.gvBitacora.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvBitacora.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpReport
            // 
            this.tpReport.Controls.Add(this.gcReport);
            this.tpReport.Name = "tpReport";
            this.tpReport.Size = new System.Drawing.Size(844, 300);
            this.tpReport.Text = this.DocumentID + "_tpReport";
            // 
            // gcReport
            // 
            this.gcReport.Location = new System.Drawing.Point(72, 87);
            this.gcReport.MainView = this.gvReport;
            this.gcReport.Name = "gcReport";
            this.gcReport.Size = new System.Drawing.Size(400, 200);
            this.gcReport.TabIndex = 3;
            this.gcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReport});
            // 
            // gvReport
            // 
            this.gvReport.GridControl = this.gcReport;
            this.gvReport.Name = "gvReport";
            this.gvReport.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvReport.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvReport.HorzScrollStep = 1;
            this.gvReport.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvReport.OptionsBehavior.AutoPopulateColumns = false;
            this.gvReport.OptionsCustomization.AllowFilter = false;
            this.gvReport.OptionsCustomization.AllowSort = false;
            this.gvReport.OptionsCustomization.AllowColumnMoving = false;
            this.gvReport.OptionsDetail.AllowZoomDetail = false;
            this.gvReport.OptionsMenu.EnableColumnMenu = false;
            this.gvReport.OptionsMenu.EnableFooterMenu = false;
            this.gvReport.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvReport.OptionsView.ColumnAutoWidth = false;
            this.gvReport.OptionsView.ShowGroupPanel = false;
            this.gvReport.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvReport.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpQuery
            // 
            this.tpQuery.Controls.Add(this.gcQuery);
            this.tpQuery.Name = "tpQuery";
            this.tpQuery.Size = new System.Drawing.Size(844, 300);
            this.tpQuery.Text = this.DocumentID + "_tpQuery";
            // 
            // gcQuery
            // 
            this.gcQuery.Location = new System.Drawing.Point(149, 103);
            this.gcQuery.MainView = this.gvQuery;
            this.gcQuery.Name = "gcQuery";
            this.gcQuery.Size = new System.Drawing.Size(400, 200);
            this.gcQuery.TabIndex = 4;
            this.gcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQuery.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQuery});
            // 
            // gvQueries
            // 
            this.gvQuery.GridControl = this.gcQuery;
            this.gvQuery.Name = "gvQuery";
            this.gvQuery.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvQuery.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvQuery.HorzScrollStep = 1;
            this.gvQuery.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvQuery.OptionsCustomization.AllowFilter = false;
            this.gvQuery.OptionsCustomization.AllowSort = false;
            this.gvQuery.OptionsCustomization.AllowColumnMoving = false;
            this.gvQuery.OptionsDetail.AllowZoomDetail = false;
            this.gvQuery.OptionsMenu.EnableColumnMenu = false;
            this.gvQuery.OptionsMenu.EnableFooterMenu = false;
            this.gvQuery.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvQuery.OptionsView.ColumnAutoWidth = false;
            this.gvQuery.OptionsView.ShowGroupPanel = false;
            this.gvQuery.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvQuery.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpDocument
            // 
            this.tpDocument.Controls.Add(this.gcDocument);
            this.tpDocument.Name = "tpDocument";
            this.tpDocument.Size = new System.Drawing.Size(844, 300);
            this.tpDocument.Text = this.DocumentID + "_tpDocument";
            // 
            // gcDocs
            // 
            this.gcDocument.Location = new System.Drawing.Point(136, 105);
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(400, 200);
            this.gcDocument.TabIndex = 5;
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument});
            // 
            // gvDocument
            // 
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.HorzScrollStep = 1;
            this.gvDocument.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsDetail.AllowZoomDetail = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpDocumentAprob
            // 
            this.tpDocumentAprob.Controls.Add(this.gcDocumentAprob);
            this.tpDocumentAprob.Name = "tpDocumentAprob";
            this.tpDocumentAprob.Size = new System.Drawing.Size(844, 300);
            this.tpDocumentAprob.Text = this.DocumentID + "_tpDocumentAprob";
            // 
            // gcDocumentAprob
            // 
            this.gcDocumentAprob.Location = new System.Drawing.Point(136, 105);
            this.gcDocumentAprob.MainView = this.gvDocumentAprob;
            this.gcDocumentAprob.Name = "gcDocumentAprob";
            this.gcDocumentAprob.Size = new System.Drawing.Size(400, 200);
            this.gcDocumentAprob.TabIndex = 6;
            this.gcDocumentAprob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocumentAprob.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocumentAprob});
            // 
            // gvDocumentAprob
            // 
            this.gvDocumentAprob.GridControl = this.gcDocumentAprob;
            this.gvDocumentAprob.Name = "gvDocumentAprob";
            this.gvDocumentAprob.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocumentAprob.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocumentAprob.HorzScrollStep = 1;
            this.gvDocumentAprob.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvDocumentAprob.OptionsCustomization.AllowFilter = false;
            this.gvDocumentAprob.OptionsCustomization.AllowSort = false;
            this.gvDocumentAprob.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocumentAprob.OptionsDetail.AllowZoomDetail = false;
            this.gvDocumentAprob.OptionsMenu.EnableColumnMenu = false;
            this.gvDocumentAprob.OptionsMenu.EnableFooterMenu = false;
            this.gvDocumentAprob.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocumentAprob.OptionsView.ColumnAutoWidth = false;
            this.gvDocumentAprob.OptionsView.ShowGroupPanel = false;
            this.gvDocumentAprob.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvDocumentAprob.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpControl
            // 
            this.tpControl.Controls.Add(this.gcControl);
            this.tpControl.Name = "tpControl";
            this.tpControl.Size = new System.Drawing.Size(844, 300);
            this.tpControl.Text = this.DocumentID + "_tpControl";
            // 
            // gcControl
            // 
            this.gcControl.Location = new System.Drawing.Point(136, 105);
            this.gcControl.MainView = this.gvControl;
            this.gcControl.Name = "gcControl";
            this.gcControl.Size = new System.Drawing.Size(400, 200);
            this.gcControl.TabIndex = 7;
            this.gcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvControl});
            // 
            // gvControl
            // 
            this.gvControl.GridControl = this.gcControl;
            this.gvControl.Name = "gvControl";
            this.gvControl.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvControl.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvControl.HorzScrollStep = 1;
            this.gvControl.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvControl.OptionsCustomization.AllowFilter = false;
            this.gvControl.OptionsCustomization.AllowSort = false;
            this.gvControl.OptionsCustomization.AllowColumnMoving = false;
            this.gvControl.OptionsDetail.AllowZoomDetail = false;
            this.gvControl.OptionsMenu.EnableColumnMenu = false;
            this.gvControl.OptionsMenu.EnableFooterMenu = false;
            this.gvControl.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvControl.OptionsView.ColumnAutoWidth = false;
            this.gvControl.OptionsView.ShowGroupPanel = false;
            this.gvControl.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvControl.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // tpActivities
            // 
            this.tpActivities.Controls.Add(this.gcActivities);
            this.tpActivities.Name = "tpActivities";
            this.tpActivities.Size = new System.Drawing.Size(844, 300);
            this.tpActivities.Text = this.DocumentID + "_tpActivities";
            // 
            // gcActivities
            // 
            this.gcActivities.Location = new System.Drawing.Point(72, 87);
            this.gcActivities.MainView = this.gvActivities;
            this.gcActivities.Name = "gcActivities";
            this.gcActivities.Size = new System.Drawing.Size(400, 200);
            this.gcActivities.TabIndex = 8;
            this.gcActivities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcActivities.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvActivities});
            // 
            // gvActivities
            // 
            this.gvActivities.GridControl = this.gcActivities;
            this.gvActivities.Name = "gvActivities";
            this.gvActivities.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvActivities.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvActivities.HorzScrollStep = 1;
            this.gvActivities.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            this.gvActivities.OptionsBehavior.AutoPopulateColumns = false;
            this.gvActivities.OptionsCustomization.AllowFilter = false;
            this.gvActivities.OptionsCustomization.AllowSort = false;
            this.gvActivities.OptionsCustomization.AllowColumnMoving = false;
            this.gvActivities.OptionsDetail.AllowZoomDetail = false;
            this.gvActivities.OptionsMenu.EnableColumnMenu = false;
            this.gvActivities.OptionsMenu.EnableFooterMenu = false;
            this.gvActivities.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvActivities.OptionsView.ColumnAutoWidth = false;
            this.gvActivities.OptionsView.ShowGroupPanel = false;
            this.gvActivities.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvSecurity_FocusedRowChanged);
            this.gvActivities.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvSecurity_CustomUnboundColumnData);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkRecordEdit,
            this.txtRecordEdit});
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
            // 
            // seGrupoDocumento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 487);
            this.Controls.Add(this.pnlModule);
            this.Name = "seGrupoDocumento";
            this.Text = this.DocumentID.ToString();
            this.pnlModule.ResumeLayout(false);
            this.pnlGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grlControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tcSecurity)).EndInit();
            this.tcSecurity.ResumeLayout(false);
            this.tpMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMaster)).EndInit();
            this.tpProcess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProcess)).EndInit();
            this.tpBitacora.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBitacora)).EndInit();
            this.tpReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).EndInit();
            this.tpQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).EndInit();
            this.tpDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            this.tpDocumentAprob.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocumentAprob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocumentAprob)).EndInit();
            this.tpControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvControl)).EndInit();
            this.tpActivities.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcActivities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvActivities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecordEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRecordEdit)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pnlModule;
        private System.Windows.Forms.Panel pnlGroup;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkRecordEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtRecordEdit;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraGrid.GridControl grlControlGroup;
        private DevExpress.XtraGrid.Views.Grid.GridView gvGroup;
        private DevExpress.XtraTab.XtraTabControl tcSecurity;
        // Maestras
        private DevExpress.XtraTab.XtraTabPage tpMaster;
        private DevExpress.XtraGrid.GridControl gcMaster;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMaster;
        // Procesos
        private DevExpress.XtraTab.XtraTabPage tpProcess;
        private DevExpress.XtraGrid.GridControl gcProcess;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProcess;
        // Bitacora
        private DevExpress.XtraTab.XtraTabPage tpBitacora;
        private DevExpress.XtraGrid.GridControl gcBitacora;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBitacora;
        //Reportes
        private DevExpress.XtraTab.XtraTabPage tpReport;
        private DevExpress.XtraGrid.GridControl gcReport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReport;
        //Consultas
        private DevExpress.XtraTab.XtraTabPage tpQuery;
        private DevExpress.XtraGrid.GridControl gcQuery;
        private DevExpress.XtraGrid.Views.Grid.GridView gvQuery;
        //Documentos
        private DevExpress.XtraTab.XtraTabPage tpDocument;
        private DevExpress.XtraGrid.GridControl gcDocument;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        //Documentos de Aprobacion
        private DevExpress.XtraTab.XtraTabPage tpDocumentAprob;
        private DevExpress.XtraGrid.GridControl gcDocumentAprob;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocumentAprob;
        //Control
        private DevExpress.XtraTab.XtraTabPage tpControl;
        private DevExpress.XtraGrid.GridControl gcControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gvControl;
        //Actividades
        private DevExpress.XtraTab.XtraTabPage tpActivities;
        private DevExpress.XtraGrid.GridControl gcActivities;
        private DevExpress.XtraGrid.Views.Grid.GridView gvActivities;

    }
}