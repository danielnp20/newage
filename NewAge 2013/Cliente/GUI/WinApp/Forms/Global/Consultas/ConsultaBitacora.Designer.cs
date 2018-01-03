namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaBitacora
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
            this.gcBitacora = new DevExpress.XtraGrid.GridControl();
            this.masterView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterView)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gcBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBitacora.Location = new System.Drawing.Point(0, 0);
            this.gcBitacora.MainView = this.masterView;
            this.gcBitacora.Name = "gridControl1";
            this.gcBitacora.Size = new System.Drawing.Size(1267, 481);
            this.gcBitacora.TabIndex = 0;
            this.gcBitacora.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.masterView});
            // 
            // masterView
            // 
            this.masterView.GridControl = this.gcBitacora;
            this.masterView.Name = "masterView";
            this.masterView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.masterView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.masterView.OptionsBehavior.Editable = false;
            this.masterView.OptionsCustomization.AllowColumnMoving = false;
            this.masterView.OptionsCustomization.AllowFilter = false;
            this.masterView.OptionsCustomization.AllowGroup = false;
            this.masterView.OptionsCustomization.AllowQuickHideColumns = false;
            this.masterView.OptionsCustomization.AllowSort = false;
            this.masterView.OptionsDetail.ShowDetailTabs = false;
            this.masterView.OptionsDetail.SmartDetailHeight = true;
            this.masterView.OptionsFilter.AllowColumnMRUFilterList = false;
            this.masterView.OptionsFilter.AllowFilterEditor = false;
            this.masterView.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.masterView.OptionsFilter.AllowMRUFilterList = false;
            this.masterView.OptionsFilter.AllowMultiSelectInCheckedFilterPopup = false;
            this.masterView.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.masterView.OptionsView.ShowGroupPanel = false;
            this.masterView.MasterRowExpanded += new DevExpress.XtraGrid.Views.Grid.CustomMasterRowEventHandler(this.masterView_MasterRowExpanded);
            // 
            // uc_Pagging
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pgGrid.Location = new System.Drawing.Point(0, 455);
            this.pgGrid.Name = "uc_Pagging";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(1267, 26);
            this.pgGrid.TabIndex = 1;
            // 
            // ConsultaBitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 481);
            this.Controls.Add(this.pgGrid);
            this.Controls.Add(this.gcBitacora);
            this.Name = "ConsultaBitacora";
            this.Text = "ConsultaBitacora";
            ((System.ComponentModel.ISupportInitialize)(this.gcBitacora)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcBitacora;
        private DevExpress.XtraGrid.Views.Grid.GridView masterView;
        private ControlsUC.uc_Pagging pgGrid;
    }
}