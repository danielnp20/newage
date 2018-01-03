namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ExportLibranzas
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
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnExport = new DevExpress.XtraBars.BarButtonItem();
            this.btnSearch = new DevExpress.XtraBars.BarButtonItem();
            this.dtpPeriodoIni = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.dtpPeriodoFin = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.rbpExport = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgExport = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpFilters = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgFilters = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgSearch = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.gcExport = new DevExpress.XtraGrid.GridControl();
            this.gvExport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.sfdGuardarDoc = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.ExpandCollapseItem.Name = "";
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.btnExport,
            this.btnSearch,
            this.dtpPeriodoIni,
            this.dtpPeriodoFin});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 5;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rbpExport,
            this.rbpFilters});
            this.ribbon.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            this.ribbon.Size = new System.Drawing.Size(794, 142);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // btnExport
            // 
            this.btnExport.Caption = "Exportar Excel";
            this.btnExport.Id = 1;
            this.btnExport.LargeGlyph = global::NewAge.Properties.Resources.Excel;
            this.btnExport.Name = "btnExport";
            this.btnExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExport_ItemClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Caption = "Consultar";
            this.btnSearch.Id = 2;
            this.btnSearch.LargeGlyph = global::NewAge.Properties.Resources.find;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSearch_ItemClick);
            // 
            // dtpPeriodoIni
            // 
            this.dtpPeriodoIni.Caption = "Periodo Ini";
            this.dtpPeriodoIni.Edit = this.repositoryItemDateEdit1;
            this.dtpPeriodoIni.Id = 3;
            this.dtpPeriodoIni.Name = "dtpPeriodoIni";
            this.dtpPeriodoIni.Width = 100;
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
            // dtpPeriodoFin
            // 
            this.dtpPeriodoFin.Caption = "Periodo Fin";
            this.dtpPeriodoFin.Edit = this.repositoryItemDateEdit2;
            this.dtpPeriodoFin.Id = 4;
            this.dtpPeriodoFin.Name = "dtpPeriodoFin";
            this.dtpPeriodoFin.Width = 100;
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
            // rbpExport
            // 
            this.rbpExport.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgExport});
            this.rbpExport.Name = "rbpExport";
            this.rbpExport.Text = "Exportar";
            // 
            // rpgExport
            // 
            this.rpgExport.ItemLinks.Add(this.btnExport);
            this.rpgExport.Name = "rpgExport";
            this.rpgExport.Text = "Exportar";
            // 
            // rbpFilters
            // 
            this.rbpFilters.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgFilters,
            this.rpgSearch});
            this.rbpFilters.Name = "rbpFilters";
            this.rbpFilters.Text = "Filtros";
            // 
            // rpgFilters
            // 
            this.rpgFilters.ItemLinks.Add(this.dtpPeriodoIni);
            this.rpgFilters.ItemLinks.Add(this.dtpPeriodoFin);
            this.rpgFilters.Name = "rpgFilters";
            this.rpgFilters.Text = "Filtros";
            // 
            // rpgSearch
            // 
            this.rpgSearch.ItemLinks.Add(this.btnSearch);
            this.rpgSearch.Name = "rpgSearch";
            this.rpgSearch.Text = "Buscar";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 422);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(794, 27);
            // 
            // gcExport
            // 
            this.gcExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcExport.Location = new System.Drawing.Point(0, 142);
            this.gcExport.LookAndFeel.SkinName = "Dark Side";
            this.gcExport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcExport.MainView = this.gvExport;
            this.gcExport.MenuManager = this.ribbon;
            this.gcExport.Name = "gcExport";
            this.gcExport.Size = new System.Drawing.Size(794, 280);
            this.gcExport.TabIndex = 2;
            this.gcExport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExport});
            // 
            // gvExport
            // 
            this.gvExport.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvExport.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvExport.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvExport.Appearance.Empty.Options.UseBackColor = true;
            this.gvExport.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvExport.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvExport.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExport.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvExport.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvExport.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvExport.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExport.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvExport.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvExport.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvExport.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvExport.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvExport.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvExport.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvExport.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvExport.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvExport.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvExport.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvExport.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvExport.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvExport.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvExport.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvExport.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvExport.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExport.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvExport.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvExport.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvExport.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvExport.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvExport.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvExport.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvExport.Appearance.Row.Options.UseBackColor = true;
            this.gvExport.Appearance.Row.Options.UseForeColor = true;
            this.gvExport.Appearance.Row.Options.UseTextOptions = true;
            this.gvExport.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvExport.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExport.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvExport.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvExport.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvExport.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvExport.Appearance.VertLine.Options.UseBackColor = true;
            this.gvExport.GridControl = this.gcExport;
            this.gvExport.Name = "gvExport";
            this.gvExport.OptionsBehavior.Editable = false;
            this.gvExport.OptionsFind.AlwaysVisible = true;
            this.gvExport.OptionsView.ShowAutoFilterRow = true;
            // 
            // sfdGuardarDoc
            // 
            this.sfdGuardarDoc.Filter = "Archivo Excel | *.XLS";
            // 
            // ExportLibranzas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 449);
            this.Controls.Add(this.gcExport);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "ExportLibranzas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExportLibranzas";
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpExport;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgExport;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraGrid.GridControl gcExport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExport;
        private System.Windows.Forms.SaveFileDialog sfdGuardarDoc;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpFilters;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgFilters;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgSearch;
        private DevExpress.XtraBars.BarButtonItem btnExport;
        private DevExpress.XtraBars.BarButtonItem btnSearch;
        private DevExpress.XtraBars.BarEditItem dtpPeriodoIni;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraBars.BarEditItem dtpPeriodoFin;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
    }
}