namespace NewAge.ReportesComunes
{
    partial class ComprobanteReport
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
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.FooterEvenRowStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.CaptionStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.HeaderStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FooterOddRowStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.SumStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
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
            this.BottomMargin.HeightF = 120F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // FooterEvenRowStyle
            // 
            this.FooterEvenRowStyle.BackColor = System.Drawing.Color.LightGray;
            this.FooterEvenRowStyle.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.FooterEvenRowStyle.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FooterEvenRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.FooterEvenRowStyle.Name = "FooterEvenRowStyle";
            this.FooterEvenRowStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // CaptionStyle
            // 
            this.CaptionStyle.BackColor = System.Drawing.Color.DimGray;
            this.CaptionStyle.BorderColor = System.Drawing.Color.Transparent;
            this.CaptionStyle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CaptionStyle.BorderWidth = 0;
            this.CaptionStyle.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CaptionStyle.ForeColor = System.Drawing.Color.White;
            this.CaptionStyle.Name = "CaptionStyle";
            this.CaptionStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // HeaderStyle
            // 
            this.HeaderStyle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.HeaderStyle.Name = "HeaderStyle";
            this.HeaderStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // FooterOddRowStyle
            // 
            this.FooterOddRowStyle.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.FooterOddRowStyle.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FooterOddRowStyle.Name = "FooterOddRowStyle";
            this.FooterOddRowStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // SumStyle
            // 
            this.SumStyle.BackColor = System.Drawing.Color.DarkGray;
            this.SumStyle.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SumStyle.Name = "SumStyle";
            this.SumStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(NewAge.DTO.Reportes.DTO_ReportComprobante2);
            // 
            // ComprobanteReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataSource = this.bindingSource1;
            this.Margins = new System.Drawing.Printing.Margins(31, 30, 29, 120);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.HeaderStyle,
            this.CaptionStyle,
            this.FooterEvenRowStyle,
            this.FooterOddRowStyle,
            this.SumStyle});
            this.Version = "11.2";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRControlStyle FooterEvenRowStyle;
        private DevExpress.XtraReports.UI.XRControlStyle CaptionStyle;
        private DevExpress.XtraReports.UI.XRControlStyle HeaderStyle;
        private DevExpress.XtraReports.UI.XRControlStyle FooterOddRowStyle;
        private DevExpress.XtraReports.UI.XRControlStyle SumStyle;
        protected DevExpress.XtraReports.UI.DetailBand Detail;
        protected DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        protected DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}
