namespace NewAge.Reports
{
    partial class ReportBaseLandScape
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
        protected void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.lblNombreEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.imgLogoEmpresa = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblReportName = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblPage = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUser = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblFecha = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.Visible = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.imgLogoEmpresa,
            this.lblNombreEmpresa,
            this.lblReportName});
            this.TopMargin.HeightF = 126.9167F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblNombreEmpresa
            // 
            this.lblNombreEmpresa.Font = new System.Drawing.Font("Arial Narrow", 16F, System.Drawing.FontStyle.Bold);
            this.lblNombreEmpresa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblNombreEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(117.9873F, 54.99999F);
            this.lblNombreEmpresa.Name = "lblNombreEmpresa";
            this.lblNombreEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNombreEmpresa.SizeF = new System.Drawing.SizeF(820.735F, 23F);
            this.lblNombreEmpresa.StylePriority.UseFont = false;
            this.lblNombreEmpresa.StylePriority.UseForeColor = false;
            this.lblNombreEmpresa.StylePriority.UseTextAlignment = false;
            this.lblNombreEmpresa.Text = "lblNombreEmpresa";
            this.lblNombreEmpresa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // imgLogoEmpresa
            // 
            this.imgLogoEmpresa.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.imgLogoEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(15.91669F, 20F);
            this.imgLogoEmpresa.Name = "imgLogoEmpresa";
            this.imgLogoEmpresa.SizeF = new System.Drawing.SizeF(149.385F, 106.9167F);
            this.imgLogoEmpresa.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // lblReportName
            // 
            this.lblReportName.Font = new System.Drawing.Font("Arial Narrow", 13F, System.Drawing.FontStyle.Bold);
            this.lblReportName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblReportName.LocationFloat = new DevExpress.Utils.PointFloat(117.9873F, 92.54166F);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportName.SizeF = new System.Drawing.SizeF(820.735F, 19.79168F);
            this.lblReportName.StylePriority.UseFont = false;
            this.lblReportName.StylePriority.UseForeColor = false;
            this.lblReportName.StylePriority.UseTextAlignment = false;
            this.lblReportName.Text = "lblReportName";
            this.lblReportName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 92.79169F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.lblPage,
            this.lblUser,
            this.lblUserName,
            this.lblFecha,
            this.xrPageInfo1});
            this.PageFooter.HeightF = 25.00001F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo2.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.xrPageInfo2.Format = "{0:dd/MM/yyyy H:mm:ss}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(491.2974F, 0.4791578F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(217.7083F, 23F);
            this.xrPageInfo2.StylePriority.UseBorders = false;
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblPage
            // 
            this.lblPage.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblPage.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblPage.LocationFloat = new DevExpress.Utils.PointFloat(927F, 2.000014F);
            this.lblPage.Name = "lblPage";
            this.lblPage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPage.SizeF = new System.Drawing.SizeF(50.00012F, 23F);
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
            this.lblUser.LocationFloat = new DevExpress.Utils.PointFloat(3.002472F, 0.4791727F);
            this.lblUser.Name = "lblUser";
            this.lblUser.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUser.SizeF = new System.Drawing.SizeF(54.16669F, 23F);
            this.lblUser.StylePriority.UseFont = false;
            this.lblUser.StylePriority.UseForeColor = false;
            this.lblUser.StylePriority.UseTextAlignment = false;
            this.lblUser.Text = "msg_lblUser";
            this.lblUser.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblUserName.LocationFloat = new DevExpress.Utils.PointFloat(63.17246F, 0.4791727F);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUserName.SizeF = new System.Drawing.SizeF(160.4167F, 23F);
            this.lblUserName.StylePriority.UseFont = false;
            this.lblUserName.StylePriority.UseForeColor = false;
            this.lblUserName.StylePriority.UseTextAlignment = false;
            this.lblUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblFecha
            // 
            this.lblFecha.Font = new System.Drawing.Font("Times New Roman", 9.25F, System.Drawing.FontStyle.Bold);
            this.lblFecha.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblFecha.LocationFloat = new DevExpress.Utils.PointFloat(436.0891F, 0F);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblFecha.SizeF = new System.Drawing.SizeF(55.20831F, 23F);
            this.lblFecha.StylePriority.UseFont = false;
            this.lblFecha.StylePriority.UseForeColor = false;
            this.lblFecha.StylePriority.UseTextAlignment = false;
            this.lblFecha.Text = "msg_lblDate";
            this.lblFecha.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(984F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(47.91669F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // ReportBaseLandScape
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(31, 30, 127, 93);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        protected DevExpress.XtraReports.UI.XRLabel lblNombreEmpresa;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblPage;
        protected DevExpress.XtraReports.UI.XRLabel lblUser;
        protected DevExpress.XtraReports.UI.XRLabel lblUserName;
        protected DevExpress.XtraReports.UI.XRLabel lblFecha;
        public DevExpress.XtraReports.UI.XRPageInfo xrPageInfo2;
        public DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        protected DevExpress.XtraReports.UI.XRPictureBox imgLogoEmpresa;
        public DevExpress.XtraReports.UI.XRLabel lblReportName;
    }
}
