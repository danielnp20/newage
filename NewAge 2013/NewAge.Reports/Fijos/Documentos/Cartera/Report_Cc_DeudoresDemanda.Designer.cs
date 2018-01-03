namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    partial class Report_Cc_DeudoresDemanda
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
            DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters msSqlConnectionParameters1 = new DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_DeudoresDemanda));
            this.DetailAll = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.oddStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.eventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.tableHeaderVencimiento = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.tableDeta = new DevExpress.XtraReports.UI.XRTable();
            this.negrita = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tbTipo = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDescripcion = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta01 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta02 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta03 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta04 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta05 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta06 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta07 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta08 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta09 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDeta12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.QueriesDatasource = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.acumulado = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeaderVencimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableDeta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DetailAll
            // 
            this.DetailAll.HeightF = 0F;
            this.DetailAll.Name = "DetailAll";
            this.DetailAll.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.DetailAll.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 28.00001F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 10.83167F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // oddStyle
            // 
            this.oddStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.oddStyle.Name = "oddStyle";
            this.oddStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // eventStyle
            // 
            this.eventStyle.BackColor = System.Drawing.Color.White;
            this.eventStyle.Name = "eventStyle";
            this.eventStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.tableHeaderVencimiento});
            this.PageHeader.HeightF = 101.9145F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel6
            // 
            this.xrLabel6.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(27.66512F, 13.72615F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(1035.335F, 21.18833F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Clientes en Proceso de Demanda";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // tableHeaderVencimiento
            // 
            this.tableHeaderVencimiento.BackColor = System.Drawing.Color.DimGray;
            this.tableHeaderVencimiento.BorderColor = System.Drawing.Color.White;
            this.tableHeaderVencimiento.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableHeaderVencimiento.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold);
            this.tableHeaderVencimiento.LocationFloat = new DevExpress.Utils.PointFloat(0F, 45.6645F);
            this.tableHeaderVencimiento.Name = "tableHeaderVencimiento";
            this.tableHeaderVencimiento.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17});
            this.tableHeaderVencimiento.SizeF = new System.Drawing.SizeF(1173F, 56.25F);
            this.tableHeaderVencimiento.StylePriority.UseBackColor = false;
            this.tableHeaderVencimiento.StylePriority.UseBorderColor = false;
            this.tableHeaderVencimiento.StylePriority.UseBorders = false;
            this.tableHeaderVencimiento.StylePriority.UseFont = false;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28,
            this.xrTableCell50,
            this.xrTableCell33,
            this.xrTableCell47,
            this.xrTableCell45,
            this.xrTableCell52,
            this.xrTableCell46,
            this.xrTableCell53,
            this.xrTableCell1,
            this.xrTableCell34,
            this.xrTableCell51,
            this.xrTableCell42,
            this.xrTableCell48,
            this.xrTableCell43});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell28.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.ForeColor = System.Drawing.Color.White;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBorderColor = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UseForeColor = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "NOMBRE DEUDOR";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.43176082330567089D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell50.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell50.ForeColor = System.Drawing.Color.White;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StylePriority.UseBorderColor = false;
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.StylePriority.UseForeColor = false;
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.Text = "CEDULA";
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell50.Weight = 0.23363190270321954D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell33.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell33.ForeColor = System.Drawing.Color.White;
            this.xrTableCell33.Multiline = true;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBorderColor = false;
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.StylePriority.UseForeColor = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "FECHA ULTIMO ABONO";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell33.Weight = 0.15999465970021917D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell47.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell47.ForeColor = System.Drawing.Color.White;
            this.xrTableCell47.Multiline = true;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseBorderColor = false;
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.StylePriority.UseForeColor = false;
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.Text = "VR. CUOTA";
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell47.Weight = 0.16951814287663658D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell45.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell45.ForeColor = System.Drawing.Color.White;
            this.xrTableCell45.Multiline = true;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseBorderColor = false;
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.StylePriority.UseForeColor = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.Text = "FECHA VCTO CUOTA MÁS ANTIGUA";
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell45.Weight = 0.16775048452336333D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell52.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell52.ForeColor = System.Drawing.Color.White;
            this.xrTableCell52.Multiline = true;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.StylePriority.UseBorderColor = false;
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.StylePriority.UseForeColor = false;
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.Text = "VR. TOTAL VENCIDO";
            this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell52.Weight = 0.17298803083279815D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell46.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell46.ForeColor = System.Drawing.Color.White;
            this.xrTableCell46.Multiline = true;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseBorderColor = false;
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.StylePriority.UseForeColor = false;
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.Text = "VR. TOTAL DEUDA";
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell46.Weight = 0.15315851866214209D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell53.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell53.ForeColor = System.Drawing.Color.White;
            this.xrTableCell53.Multiline = true;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StylePriority.UseBorderColor = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.StylePriority.UseForeColor = false;
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.Text = "VR. GARANTÍA";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell53.Weight = 0.15315851866214197D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell1.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "FECHA DEMANDA INMOVILIZA SISTEMA";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.15999469152026366D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell34.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell34.ForeColor = System.Drawing.Color.White;
            this.xrTableCell34.Multiline = true;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseBorderColor = false;
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.StylePriority.UseForeColor = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "FECHA DEMANDA INMOVILIZA JUZGADO";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell34.Weight = 0.15999469152026366D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell51.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell51.ForeColor = System.Drawing.Color.White;
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StylePriority.UseBorderColor = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.StylePriority.UseForeColor = false;
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.Text = "FECHA  ORDEN INMOVILIZA";
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell51.Weight = 0.15999468295507768D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell42.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.ForeColor = System.Drawing.Color.White;
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseBorderColor = false;
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.StylePriority.UseForeColor = false;
            this.xrTableCell42.StylePriority.UseTextAlignment = false;
            this.xrTableCell42.Text = "FECHA INMOVILIZA VEHICULO";
            this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell42.Weight = 0.15999468884315146D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell48.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell48.ForeColor = System.Drawing.Color.White;
            this.xrTableCell48.Multiline = true;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseBorderColor = false;
            this.xrTableCell48.StylePriority.UseFont = false;
            this.xrTableCell48.StylePriority.UseForeColor = false;
            this.xrTableCell48.StylePriority.UseTextAlignment = false;
            this.xrTableCell48.Text = "FECHA ACUERDO de PAGO";
            this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell48.Weight = 0.159994682355872D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell43.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.ForeColor = System.Drawing.Color.White;
            this.xrTableCell43.Multiline = true;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseBorderColor = false;
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.StylePriority.UseForeColor = false;
            this.xrTableCell43.StylePriority.UseTextAlignment = false;
            this.xrTableCell43.Text = "FECHA RECUPERA x CANCELACION TOTAL de la OBLIGACION";
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell43.Weight = 0.17904177727585344D;
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tableDeta});
            this.Detail1.FormattingRules.Add(this.negrita);
            this.Detail1.HeightF = 32.91261F;
            this.Detail1.Name = "Detail1";
            // 
            // tableDeta
            // 
            this.tableDeta.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.tableDeta.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableDeta.FormattingRules.Add(this.negrita);
            this.tableDeta.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.tableDeta.Name = "tableDeta";
            this.tableDeta.OddStyleName = "oddStyle";
            this.tableDeta.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.tableDeta.SizeF = new System.Drawing.SizeF(1173F, 32.91261F);
            this.tableDeta.StylePriority.UseBorders = false;
            this.tableDeta.StylePriority.UseTextAlignment = false;
            this.tableDeta.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // negrita
            // 
            this.negrita.Condition = "[Tipo] == \'10RENTAB\'";
            this.negrita.DataMember = "Cartera_ReportResumenMesRentabilidad";
            // 
            // 
            // 
            this.negrita.Formatting.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold);
            this.negrita.Formatting.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.negrita.Name = "negrita";
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tbTipo,
            this.tbDescripcion,
            this.tbDeta01,
            this.tbDeta02,
            this.tbDeta03,
            this.tbDeta04,
            this.tbDeta05,
            this.tbDeta06,
            this.tbDeta07,
            this.xrTableCell2,
            this.tbDeta08,
            this.tbDeta09,
            this.tbDeta10,
            this.tbDeta11,
            this.tbDeta12});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tbTipo
            // 
            this.tbTipo.CanGrow = false;
            this.tbTipo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_ReportResumenMesRentabilidad.tipo")});
            this.tbTipo.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.tbTipo.Name = "tbTipo";
            this.tbTipo.StylePriority.UseFont = false;
            this.tbTipo.StylePriority.UseTextAlignment = false;
            this.tbTipo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tbTipo.Visible = false;
            this.tbTipo.Weight = 0.008067471901812423D;
            // 
            // tbDescripcion
            // 
            this.tbDescripcion.CanGrow = false;
            this.tbDescripcion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.Nombre")});
            this.tbDescripcion.Font = new System.Drawing.Font("Arial Narrow", 8.7F);
            this.tbDescripcion.FormattingRules.Add(this.negrita);
            this.tbDescripcion.Name = "tbDescripcion";
            this.tbDescripcion.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
            this.tbDescripcion.StylePriority.UseFont = false;
            this.tbDescripcion.StylePriority.UsePadding = false;
            this.tbDescripcion.StylePriority.UseTextAlignment = false;
            this.tbDescripcion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tbDescripcion.Weight = 0.6254412890188511D;
            this.tbDescripcion.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.tbDescripcion_BeforePrint);
            // 
            // tbDeta01
            // 
            this.tbDeta01.CanGrow = false;
            this.tbDeta01.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.ClienteID", "{0:n2}")});
            this.tbDeta01.Font = new System.Drawing.Font("Arial Narrow", 8.5F);
            this.tbDeta01.FormattingRules.Add(this.negrita);
            this.tbDeta01.Name = "tbDeta01";
            this.tbDeta01.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 2, 0, 0, 100F);
            this.tbDeta01.StylePriority.UseFont = false;
            this.tbDeta01.StylePriority.UsePadding = false;
            this.tbDeta01.StylePriority.UseTextAlignment = false;
            this.tbDeta01.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tbDeta01.Weight = 0.34280046195512487D;
            // 
            // tbDeta02
            // 
            this.tbDeta02.CanGrow = false;
            this.tbDeta02.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.UltAbono", "{0:dd/MM/yyyy}")});
            this.tbDeta02.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta02.FormattingRules.Add(this.negrita);
            this.tbDeta02.Name = "tbDeta02";
            this.tbDeta02.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta02.StylePriority.UseFont = false;
            this.tbDeta02.StylePriority.UsePadding = false;
            this.tbDeta02.StylePriority.UseTextAlignment = false;
            this.tbDeta02.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta02.Weight = 0.23475501190098411D;
            this.tbDeta02.WordWrap = false;
            // 
            // tbDeta03
            // 
            this.tbDeta03.CanGrow = false;
            this.tbDeta03.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.vlrCuota", "{0:n0}")});
            this.tbDeta03.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta03.FormattingRules.Add(this.negrita);
            this.tbDeta03.Name = "tbDeta03";
            this.tbDeta03.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta03.StylePriority.UseFont = false;
            this.tbDeta03.StylePriority.UsePadding = false;
            this.tbDeta03.StylePriority.UseTextAlignment = false;
            this.tbDeta03.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tbDeta03.Weight = 0.24872854338151637D;
            this.tbDeta03.WordWrap = false;
            // 
            // tbDeta04
            // 
            this.tbDeta04.CanGrow = false;
            this.tbDeta04.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaMasAntigua", "{0:dd/MM/yyyy}")});
            this.tbDeta04.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta04.FormattingRules.Add(this.negrita);
            this.tbDeta04.Name = "tbDeta04";
            this.tbDeta04.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta04.StylePriority.UseFont = false;
            this.tbDeta04.StylePriority.UsePadding = false;
            this.tbDeta04.StylePriority.UseTextAlignment = false;
            this.tbDeta04.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta04.Weight = 0.24613489848650966D;
            this.tbDeta04.WordWrap = false;
            // 
            // tbDeta05
            // 
            this.tbDeta05.CanGrow = false;
            this.tbDeta05.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.SaldoVencido", "{0:n0}")});
            this.tbDeta05.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta05.FormattingRules.Add(this.negrita);
            this.tbDeta05.Name = "tbDeta05";
            this.tbDeta05.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta05.StylePriority.UseFont = false;
            this.tbDeta05.StylePriority.UsePadding = false;
            this.tbDeta05.StylePriority.UseTextAlignment = false;
            this.tbDeta05.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tbDeta05.Weight = 0.25381976350394836D;
            this.tbDeta05.WordWrap = false;
            // 
            // tbDeta06
            // 
            this.tbDeta06.CanGrow = false;
            this.tbDeta06.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.SaldoTotal", "{0:n0}")});
            this.tbDeta06.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta06.FormattingRules.Add(this.negrita);
            this.tbDeta06.Name = "tbDeta06";
            this.tbDeta06.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta06.StylePriority.UseFont = false;
            this.tbDeta06.StylePriority.UsePadding = false;
            this.tbDeta06.StylePriority.UseTextAlignment = false;
            this.tbDeta06.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tbDeta06.Weight = 0.22472473972851298D;
            this.tbDeta06.WordWrap = false;
            // 
            // tbDeta07
            // 
            this.tbDeta07.CanGrow = false;
            this.tbDeta07.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.VlrGarantia", "{0:n0}")});
            this.tbDeta07.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta07.FormattingRules.Add(this.negrita);
            this.tbDeta07.Name = "tbDeta07";
            this.tbDeta07.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta07.StylePriority.UseFont = false;
            this.tbDeta07.StylePriority.UsePadding = false;
            this.tbDeta07.StylePriority.UseTextAlignment = false;
            this.tbDeta07.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tbDeta07.Weight = 0.2247243289716411D;
            this.tbDeta07.WordWrap = false;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.CanGrow = false;
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaDemandaSist", "{0:dd/MM/yyyy}")});
            this.xrTableCell2.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.23475452554793308D;
            // 
            // tbDeta08
            // 
            this.tbDeta08.CanGrow = false;
            this.tbDeta08.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaDemanda", "{0:dd/MM/yyyy}")});
            this.tbDeta08.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta08.FormattingRules.Add(this.negrita);
            this.tbDeta08.Name = "tbDeta08";
            this.tbDeta08.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta08.StylePriority.UseFont = false;
            this.tbDeta08.StylePriority.UsePadding = false;
            this.tbDeta08.StylePriority.UseTextAlignment = false;
            this.tbDeta08.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta08.Weight = 0.23475539890599778D;
            this.tbDeta08.WordWrap = false;
            // 
            // tbDeta09
            // 
            this.tbDeta09.CanGrow = false;
            this.tbDeta09.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaOrden", "{0:dd/MM/yyyy}")});
            this.tbDeta09.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta09.FormattingRules.Add(this.negrita);
            this.tbDeta09.Name = "tbDeta09";
            this.tbDeta09.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta09.StylePriority.UseFont = false;
            this.tbDeta09.StylePriority.UsePadding = false;
            this.tbDeta09.StylePriority.UseTextAlignment = false;
            this.tbDeta09.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tbDeta09.Weight = 0.23475508666874345D;
            this.tbDeta09.WordWrap = false;
            // 
            // tbDeta10
            // 
            this.tbDeta10.CanGrow = false;
            this.tbDeta10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaInmoviliza", "{0:dd/MM/yyyy}")});
            this.tbDeta10.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta10.FormattingRules.Add(this.negrita);
            this.tbDeta10.Name = "tbDeta10";
            this.tbDeta10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta10.StylePriority.UseFont = false;
            this.tbDeta10.StylePriority.UsePadding = false;
            this.tbDeta10.StylePriority.UseTextAlignment = false;
            this.tbDeta10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta10.Weight = 0.2347546876113279D;
            this.tbDeta10.WordWrap = false;
            // 
            // tbDeta11
            // 
            this.tbDeta11.CanGrow = false;
            this.tbDeta11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_RelacionDemandas.FechaAcuerdo", "{0:dd/MM/yyyy}")});
            this.tbDeta11.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta11.FormattingRules.Add(this.negrita);
            this.tbDeta11.Name = "tbDeta11";
            this.tbDeta11.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta11.StylePriority.UseFont = false;
            this.tbDeta11.StylePriority.UsePadding = false;
            this.tbDeta11.StylePriority.UseTextAlignment = false;
            this.tbDeta11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta11.Weight = 0.23475549262389864D;
            this.tbDeta11.WordWrap = false;
            // 
            // tbDeta12
            // 
            this.tbDeta12.CanGrow = false;
            this.tbDeta12.Font = new System.Drawing.Font("Arial Narrow", 7F);
            this.tbDeta12.FormattingRules.Add(this.negrita);
            this.tbDeta12.Name = "tbDeta12";
            this.tbDeta12.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tbDeta12.StylePriority.UseFont = false;
            this.tbDeta12.StylePriority.UsePadding = false;
            this.tbDeta12.StylePriority.UseTextAlignment = false;
            this.tbDeta12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDeta12.Weight = 0.26270207681922264D;
            this.tbDeta12.WordWrap = false;
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "Cartera_RelacionDemandas";
            this.DetailReport.DataSource = this.QueriesDatasource;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // QueriesDatasource
            // 
            this.QueriesDatasource.ConnectionName = "DESARROLLOAMP\\SQLSERVER2012_NewAgeApoyosConnection";
            msSqlConnectionParameters1.AuthorizationType = DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.Windows;
            msSqlConnectionParameters1.DatabaseName = "NewAgeApoyos";
            msSqlConnectionParameters1.ServerName = "DESARROLLOAMP\\SQLSERVER2012";
            this.QueriesDatasource.ConnectionParameters = msSqlConnectionParameters1;
            this.QueriesDatasource.Name = "QueriesDatasource";
            storedProcQuery1.Name = "Cartera_RelacionDemandas";
            queryParameter1.Name = "@EmpresaID";
            queryParameter1.Type = typeof(string);
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.StoredProcName = "Cartera_RelacionDemandas";
            this.QueriesDatasource.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.QueriesDatasource.ResultSchemaSerializable = resources.GetString("QueriesDatasource.ResultSchemaSerializable");
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            this.PageFooter.Name = "PageFooter";
            // 
            // acumulado
            // 
            this.acumulado.DataMember = "Cartera_ReportResumenMesRentabilidad";
            this.acumulado.Expression = "[Valor01]+[Valor02]+[Valor03]+[Valor04]+[Valor05]+[Valor06]+[Valor07]+[Valor08]+[" +
    "Valor09]+[Valor10]+[Valor11]+[Valor12]";
            this.acumulado.Name = "acumulado";
            // 
            // Report_Cc_DeudoresDemanda
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetailAll,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.DetailReport,
            this.PageHeader,
            this.PageFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.acumulado});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.QueriesDatasource});
            this.DataSource = this.QueriesDatasource;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.negrita});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(6, 11, 0, 28);
            this.PageHeight = 927;
            this.PageWidth = 1200;
            this.PaperKind = System.Drawing.Printing.PaperKind.LetterExtra;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.oddStyle,
            this.eventStyle});
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.tableHeaderVencimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableDeta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand DetailAll;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRControlStyle oddStyle;
        private DevExpress.XtraReports.UI.XRControlStyle eventStyle;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.DataAccess.Sql.SqlDataSource QueriesDatasource;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRTable tableHeaderVencimiento;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell33;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell34;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell42;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell43;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell50;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell47;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell45;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell52;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell46;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell53;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell51;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell48;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.FormattingRule negrita;
        private DevExpress.XtraReports.UI.CalculatedField acumulado;
        private DevExpress.XtraReports.UI.XRTable tableDeta;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tbTipo;
        private DevExpress.XtraReports.UI.XRTableCell tbDescripcion;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta01;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta02;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta03;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta04;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta05;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta06;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta07;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta08;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta09;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta10;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta11;
        private DevExpress.XtraReports.UI.XRTableCell tbDeta12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
    }
}
