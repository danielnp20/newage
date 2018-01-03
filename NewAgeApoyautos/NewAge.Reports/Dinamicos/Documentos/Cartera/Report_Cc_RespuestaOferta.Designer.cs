namespace NewAge.Reports.Dinamicos
{
    partial class Report_Cc_RespuestaOferta
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
System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_RespuestaOferta));
this.Detail = new DevExpress.XtraReports.UI.DetailBand();
this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
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
this.TopMargin.Name = "TopMargin";
this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
// 
// BottomMargin
// 
this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
this.BottomMargin.HeightF = 505.4583F;
this.BottomMargin.Name = "BottomMargin";
this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
// 
// xrPanel1
// 
this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrPageInfo1,
            this.xrLabel3});
this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
this.xrPanel1.Name = "xrPanel1";
this.xrPanel1.SizeF = new System.Drawing.SizeF(716F, 483.3334F);
// 
// xrTable4
// 
this.xrTable4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 74.95833F);
this.xrTable4.Name = "xrTable4";
this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
this.xrTable4.SizeF = new System.Drawing.SizeF(706F, 21.45834F);
this.xrTable4.StylePriority.UseFont = false;
// 
// xrTableRow6
// 
this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16});
this.xrTableRow6.Name = "xrTableRow6";
this.xrTableRow6.Weight = 1D;
// 
// xrTableCell16
// 
this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DetallesCesionCartera.NombreComprador.Value")});
this.xrTableCell16.Name = "xrTableCell16";
this.xrTableCell16.Text = "xrTableCell16";
this.xrTableCell16.Weight = 3D;
// 
// xrLabel6
// 
this.xrLabel6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 137.4583F);
this.xrLabel6.Name = "xrLabel6";
this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
this.xrLabel6.SizeF = new System.Drawing.SizeF(322.9166F, 23F);
this.xrLabel6.StylePriority.UseFont = false;
this.xrLabel6.Text = "ASUNTO:    RESPUESTA OFERTA No.";
// 
// xrLabel5
// 
this.xrLabel5.Font = new System.Drawing.Font("Arial", 12F);
this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 96.41666F);
this.xrLabel5.Multiline = true;
this.xrLabel5.Name = "xrLabel5";
this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 29.16667F);
this.xrLabel5.StylePriority.UseFont = false;
this.xrLabel5.Text = "Ciudad\r\n";
// 
// xrLabel4
// 
this.xrLabel4.Font = new System.Drawing.Font("Arial", 12F);
this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 72.87499F);
this.xrLabel4.Name = "xrLabel4";
this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 2.083332F);
this.xrLabel4.StylePriority.UseFont = false;
this.xrLabel4.Text = "Señores";
// 
// xrPageInfo1
// 
this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 12F);
this.xrPageInfo1.Format = "Bogotá, {0:d\' de \'MMMM\' de \'yyyy}";
this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
this.xrPageInfo1.Name = "xrPageInfo1";
this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
this.xrPageInfo1.SizeF = new System.Drawing.SizeF(266.6666F, 25F);
this.xrPageInfo1.StylePriority.UseFont = false;
// 
// xrLabel3
// 
this.xrLabel3.Font = new System.Drawing.Font("Arial", 12F);
this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 178.4583F);
this.xrLabel3.Multiline = true;
this.xrLabel3.Name = "xrLabel3";
this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
this.xrLabel3.SizeF = new System.Drawing.SizeF(713.3334F, 294.8752F);
this.xrLabel3.StylePriority.UseFont = false;
this.xrLabel3.StylePriority.UseTextAlignment = false;
this.xrLabel3.Text = resources.GetString("xrLabel3.Text");
this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify;
// 
// Report_Cc_RespuestaOferta
// 
this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
this.Margins = new System.Drawing.Printing.Margins(81, 51, 100, 505);
this.Version = "12.1";
((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPanel xrPanel1;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    }
}
