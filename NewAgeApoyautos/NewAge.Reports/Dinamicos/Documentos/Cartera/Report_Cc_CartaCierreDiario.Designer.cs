namespace NewAge.Reports.Dinamicos
{
    partial class Report_Cc_CartaCierreDiario
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_CartaCierreDiario));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.txtRichFilter = new DevExpress.XtraReports.UI.XRRichText();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.QueriesDatasource = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.txtRichFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueriesDatasource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.TopMargin.BorderWidth = 1F;
            this.TopMargin.HeightF = 32F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseBorders = false;
            this.TopMargin.StylePriority.UseBorderWidth = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.BackColor = System.Drawing.Color.Transparent;
            this.BottomMargin.HeightF = 29F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseBackColor = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrControlStyle2
            // 
            this.xrControlStyle2.Name = "xrControlStyle2";
            this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageFooter
            // 
            this.PageFooter.Expanded = false;
            this.PageFooter.HeightF = 0.4166921F;
            this.PageFooter.Name = "PageFooter";
            // 
            // PageHeader
            // 
            this.PageHeader.HeightF = 0F;
            this.PageHeader.Name = "PageHeader";
            // 
            // txtRichFilter
            // 
            this.txtRichFilter.BackColor = System.Drawing.Color.Transparent;
            this.txtRichFilter.CanShrink = true;
            this.txtRichFilter.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Rtf", null, "Detalle.PlantillaCarta.Value")});
            this.txtRichFilter.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.txtRichFilter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.txtRichFilter.Name = "txtRichFilter";
            this.txtRichFilter.SerializableRtfString = resources.GetString("txtRichFilter.SerializableRtfString");
            this.txtRichFilter.SizeF = new System.Drawing.SizeF(683.9999F, 42.00624F);
            this.txtRichFilter.StylePriority.UseBackColor = false;
            this.txtRichFilter.StylePriority.UseFont = false;
            this.txtRichFilter.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.txtRichFilter_BeforePrint);
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.GroupHeader1,
            this.GroupFooter1});
            this.DetailReport.DataMember = "Detalle";
            this.DetailReport.DataSource = this.QueriesDatasource;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.BackColor = System.Drawing.Color.Transparent;
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.txtRichFilter});
            this.Detail1.HeightF = 74.92288F;
            this.Detail1.Name = "Detail1";
            this.Detail1.StylePriority.UseBackColor = false;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("PlantillaCarta.Value", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WholePage;
            this.GroupHeader1.HeightF = 1.5625F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 16.66667F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // QueriesDatasource
            // 
            this.QueriesDatasource.DataSource = typeof(NewAge.DTO.Negocio.DTO_ccHistoricoGestionCobranza);
            this.QueriesDatasource.Name = "QueriesDatasource";
            // 
            // Report_Cc_CartaCierreDiario
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader,
            this.DetailReport});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.QueriesDatasource});
            this.DataSource = this.QueriesDatasource;
            this.Margins = new System.Drawing.Printing.Margins(116, 48, 32, 29);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.txtRichFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueriesDatasource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle2;
        protected DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        protected DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRRichText txtRichFilter;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource QueriesDatasource;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
    }
}
