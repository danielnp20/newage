namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReportCumplimiento
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
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgInicio = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.btnExport = new DevExpress.XtraBars.BarButtonItem();
            this.dtPeriodo = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.cmbTipoProyecto = new DevExpress.XtraBars.BarEditItem();
            this.lkp_TipoProyecto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.rbcExport = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnSearch = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.sfdGuardaDoc = new System.Windows.Forms.SaveFileDialog();
            this.gcExport = new DevExpress.XtraGrid.GridControl();
            this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoProyecto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbcExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgInicio});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Exportar";
            // 
            // rpgInicio
            // 
            this.rpgInicio.Glyph = global::NewAge.Properties.Resources.Excel;
            this.rpgInicio.ItemLinks.Add(this.btnExport);
            this.rpgInicio.Name = "rpgInicio";
            this.rpgInicio.Text = "Exportar";
            // 
            // btnExport
            // 
            this.btnExport.Caption = "Exportar Excel";
            this.btnExport.Id = 12;
            this.btnExport.LargeGlyph = global::NewAge.Properties.Resources.Excel;
            this.btnExport.Name = "btnExport";
            this.btnExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Export_ItemClick);
            // 
            // dtPeriodo
            // 
            this.dtPeriodo.Description = "Periodo";
            this.dtPeriodo.Edit = this.repositoryItemDateEdit3;
            this.dtPeriodo.Id = 13;
            this.dtPeriodo.LargeGlyph = global::NewAge.Properties.Resources.date;
            this.dtPeriodo.Name = "dtPeriodo";
            toolTipTitleItem1.Text = "Periodo\r\n";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Ingrese una Fecha";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.dtPeriodo.SuperTip = superToolTip1;
            this.dtPeriodo.Width = 100;
            // 
            // repositoryItemDateEdit3
            // 
            this.repositoryItemDateEdit3.AutoHeight = false;
            this.repositoryItemDateEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit3.Name = "repositoryItemDateEdit3";
            this.repositoryItemDateEdit3.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // cmbTipoProyecto
            // 
            this.cmbTipoProyecto.Description = "Tipo Proyecto";
            this.cmbTipoProyecto.Edit = this.lkp_TipoProyecto;
            this.cmbTipoProyecto.Id = 19;
            this.cmbTipoProyecto.LargeGlyph = global::NewAge.Properties.Resources.date;
            this.cmbTipoProyecto.Name = "cmbTipoProyecto";
            superToolTip2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            toolTipTitleItem2.Text = "Tipo Proyecto";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Seleccione el Tipo de Proyecto q desea ver\r\n";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.cmbTipoProyecto.SuperTip = superToolTip2;
            this.cmbTipoProyecto.Width = 100;
            // 
            // lkp_TipoProyecto
            // 
            this.lkp_TipoProyecto.AutoHeight = false;
            this.lkp_TipoProyecto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoProyecto.DisplayMember = "Value";
            this.lkp_TipoProyecto.Name = "lkp_TipoProyecto";
            this.lkp_TipoProyecto.ValueMember = "Key";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // rbcExport
            // 
            this.rbcExport.ExpandCollapseItem.Id = 0;
            this.rbcExport.ExpandCollapseItem.Name = "";
            this.rbcExport.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rbcExport.ExpandCollapseItem,
            this.btnSearch,
            this.btnExport,
            this.dtPeriodo,
            this.cmbTipoProyecto});
            this.rbcExport.Location = new System.Drawing.Point(0, 0);
            this.rbcExport.MaxItemId = 23;
            this.rbcExport.Name = "rbcExport";
            this.rbcExport.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.rbcExport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemDateEdit2,
            this.repositoryItemDateEdit3,
            this.lkp_TipoProyecto,
            this.repositoryItemTextEdit2});
            this.rbcExport.Size = new System.Drawing.Size(775, 142);
            // 
            // btnSearch
            // 
            this.btnSearch.Id = 22;
            this.btnSearch.Name = "btnSearch";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            this.repositoryItemDateEdit2.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // sfdGuardaDoc
            // 
            this.sfdGuardaDoc.Filter = "Archivos Excel | *.XLS";
            // 
            // gcExport
            // 
            this.gcExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcExport.Location = new System.Drawing.Point(0, 142);
            this.gcExport.LookAndFeel.SkinName = "Sharp";
            this.gcExport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcExport.MainView = this.gvExport;
            this.gcExport.MenuManager = this.rbcExport;
            this.gcExport.Name = "gcExport";
            this.gcExport.Size = new System.Drawing.Size(775, 132);
            this.gcExport.TabIndex = 3;
            this.gcExport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExport});
            // 
            // gvExport
            // 
            this.gvExport.GridControl = this.gcExport;
            this.gvExport.Name = "gvExport";
            this.gvExport.OptionsBehavior.Editable = false;
            this.gvExport.OptionsFind.AlwaysVisible = true;
            this.gvExport.OptionsView.ColumnAutoWidth = false;
            this.gvExport.OptionsView.ShowAutoFilterRow = true;
            // 
            // ReportExcelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 274);
            this.Controls.Add(this.gcExport);
            this.Controls.Add(this.rbcExport);
            this.Name = "ReportExcelBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReporExcelBase";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoProyecto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbcExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgInicio;
        private DevExpress.XtraBars.Ribbon.RibbonControl rbcExport;
        private System.Windows.Forms.SaveFileDialog sfdGuardaDoc;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraBars.BarButtonItem btnExport;
        private DevExpress.XtraBars.BarEditItem dtPeriodo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit3;
        private DevExpress.XtraBars.BarButtonItem btnSearch;
        private DevExpress.XtraBars.BarEditItem cmbTipoProyecto;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkp_TipoProyecto;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
    }
}