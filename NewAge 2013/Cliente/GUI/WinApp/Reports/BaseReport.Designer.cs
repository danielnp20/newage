namespace NewAge.Cliente.GUI.WinApp.Reports
{
    partial class BaseReport
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.lblUserName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPage = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblPage = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUser = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.lblParamFecha = new DevExpress.XtraReports.UI.XRLabel();
            this.lblFecha = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.imgLogoEmpresa = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblNombreEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportName = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.Detail.HeightF = 101.0417F;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 29F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.BottomMargin.HeightF = 23F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseFont = false;
            this.BottomMargin.StylePriority.UseTextAlignment = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblUserName.LocationFloat = new DevExpress.Utils.PointFloat(62.25333F, 0.9583156F);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserName.SizeF = new System.Drawing.SizeF(160.4167F, 23F);
            this.lblUserName.StylePriority.UseFont = false;
            this.lblUserName.StylePriority.UseForeColor = false;
            this.lblUserName.StylePriority.UseTextAlignment = false;
            this.lblUserName.Text = "Nombre";
            this.lblUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrPage
            // 
            this.xrPage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPage.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.xrPage.LocationFloat = new DevExpress.Utils.PointFloat(752.7867F, 0.9583156F);
            this.xrPage.Name = "xrPage";
            this.xrPage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPage.SizeF = new System.Drawing.SizeF(32.29167F, 23F);
            this.xrPage.StylePriority.UseFont = false;
            this.xrPage.StylePriority.UseForeColor = false;
            this.xrPage.StylePriority.UseTextAlignment = false;
            this.xrPage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblPage
            // 
            this.lblPage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPage.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblPage.LocationFloat = new DevExpress.Utils.PointFloat(698.62F, 0.9583156F);
            this.lblPage.Name = "lblPage";
            this.lblPage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPage.SizeF = new System.Drawing.SizeF(54.16669F, 23F);
            this.lblPage.StylePriority.UseFont = false;
            this.lblPage.StylePriority.UseForeColor = false;
            this.lblPage.StylePriority.UseTextAlignment = false;
            this.lblPage.Text = "msg_lblPage";
            this.lblPage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblUser
            // 
            this.lblUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblUser.LocationFloat = new DevExpress.Utils.PointFloat(2.083341F, 0.9583156F);
            this.lblUser.Name = "lblUser";
            this.lblUser.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUser.SizeF = new System.Drawing.SizeF(54.16669F, 23F);
            this.lblUser.StylePriority.UseFont = false;
            this.lblUser.StylePriority.UseForeColor = false;
            this.lblUser.StylePriority.UseTextAlignment = false;
            this.lblUser.Text = "msg_lblUser";
            this.lblUser.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblUserName,
            this.lblUser,
            this.lblPage,
            this.xrPage,
            this.lblParamFecha,
            this.lblFecha});
            this.PageFooter.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.PageFooter.HeightF = 25F;
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.StylePriority.UseFont = false;
            // 
            // lblParamFecha
            // 
            this.lblParamFecha.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamFecha.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblParamFecha.LocationFloat = new DevExpress.Utils.PointFloat(347.52F, 1.99997F);
            this.lblParamFecha.Name = "lblParamFecha";
            this.lblParamFecha.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblParamFecha.SizeF = new System.Drawing.SizeF(140.71F, 23F);
            this.lblParamFecha.StylePriority.UseFont = false;
            this.lblParamFecha.StylePriority.UseForeColor = false;
            this.lblParamFecha.StylePriority.UseTextAlignment = false;
            this.lblParamFecha.Text = "lblParamFecha";
            this.lblParamFecha.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblFecha
            // 
            this.lblFecha.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblFecha.LocationFloat = new DevExpress.Utils.PointFloat(300.77F, 1.99997F);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFecha.SizeF = new System.Drawing.SizeF(55.20831F, 23F);
            this.lblFecha.StylePriority.UseFont = false;
            this.lblFecha.StylePriority.UseForeColor = false;
            this.lblFecha.StylePriority.UseTextAlignment = false;
            this.lblFecha.Text = "msg_lblDate";
            this.lblFecha.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.PageHeader.BorderWidth = 1;
            this.PageHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold);
            this.PageHeader.HeightF = 19.79167F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StylePriority.UseBorderDashStyle = false;
            this.PageHeader.StylePriority.UseBorderWidth = false;
            this.PageHeader.StylePriority.UseFont = false;
            this.PageHeader.StylePriority.UseTextAlignment = false;
            this.PageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.imgLogoEmpresa,
            this.lblNombreEmpresa,
            this.lblReportName});
            this.ReportHeader.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportHeader.HeightF = 148.6667F;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.StylePriority.UseFont = false;
            // 
            // imgLogoEmpresa
            // 
            this.imgLogoEmpresa.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.imgLogoEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.imgLogoEmpresa.Name = "imgLogoEmpresa";
            this.imgLogoEmpresa.SizeF = new System.Drawing.SizeF(93.76F, 100.79F);
            this.imgLogoEmpresa.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // lblNombreEmpresa
            // 
            this.lblNombreEmpresa.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(106.1817F, 38.17001F);
            this.lblNombreEmpresa.Name = "lblNombreEmpresa";
            this.lblNombreEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNombreEmpresa.SizeF = new System.Drawing.SizeF(354.1667F, 23F);
            this.lblNombreEmpresa.StylePriority.UseFont = false;
            this.lblNombreEmpresa.Text = "lblNombreEmpresa";
            // 
            // lblReportName
            // 
            this.lblReportName.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportName.LocationFloat = new DevExpress.Utils.PointFloat(185.8334F, 100.54F);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportName.SizeF = new System.Drawing.SizeF(422.9166F, 23.00002F);
            this.lblReportName.StylePriority.UseFont = false;
            this.lblReportName.StylePriority.UseTextAlignment = false;
            this.lblReportName.Text = "lblReportName";
            this.lblReportName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.ReportFooter.HeightF = 0F;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.StylePriority.UseFont = false;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Name = "formattingRule1";
            // 
            // BaseReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader,
            this.ReportHeader,
            this.ReportFooter});
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Margins = new System.Drawing.Printing.Margins(31, 30, 29, 23);
            this.Version = "11.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected DevExpress.XtraReports.UI.DetailBand Detail;
        protected DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        protected DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        protected DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        protected DevExpress.XtraReports.UI.XRLabel lblFecha;
        protected DevExpress.XtraReports.UI.XRLabel lblParamFecha;
        protected DevExpress.XtraReports.UI.XRLabel lblReportName;
        protected DevExpress.XtraReports.UI.XRPageInfo xrPage;
        private DevExpress.XtraReports.UI.FormattingRule formattingRule1;
        protected DevExpress.XtraReports.UI.XRLabel lblPage;
        protected DevExpress.XtraReports.UI.XRLabel lblUser;
        protected DevExpress.XtraReports.UI.XRLabel lblUserName;
        protected DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        protected DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblNombreEmpresa;
        protected DevExpress.XtraReports.UI.XRPictureBox imgLogoEmpresa;
    }
}
