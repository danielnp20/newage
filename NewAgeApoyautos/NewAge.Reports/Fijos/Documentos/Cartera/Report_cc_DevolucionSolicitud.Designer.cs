namespace NewAge.Reports
{
    partial class Report_cc_DevolucionSolicitud
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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_cc_DevolucionSolicitud));
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataSourceDevolucionSolicitud = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_nroDev = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_fechaDev = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_nombre = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_cliente = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_seUsuario = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.tableHeaderVencimiento = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tbl_codCausal = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbl_descMaestra = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbl_descMaestraGrupo = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbl_observaciones = new DevExpress.XtraReports.UI.XRTableCell();
            this.shp_Titulo = new DevExpress.XtraReports.UI.XRShape();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lbl_ReportNameDev = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeaderVencimiento)).BeginInit();
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
            this.TopMargin.HeightF = 117.0832F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 79.58295F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
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
            this.xrControlStyle2.BackColor = System.Drawing.Color.Transparent;
            this.xrControlStyle2.Name = "xrControlStyle2";
            this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // DataSourceDevolucionSolicitud
            // 
            this.DataSourceDevolucionSolicitud.ConnectionName = "MC_JEFFER\\SQLSERVER2012_NewAgeConnection";
            msSqlConnectionParameters1.AuthorizationType = DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.Windows;
            msSqlConnectionParameters1.DatabaseName = "NewAge";
            msSqlConnectionParameters1.ServerName = "MC_JEFFER\\SQLSERVER2012";
            this.DataSourceDevolucionSolicitud.ConnectionParameters = msSqlConnectionParameters1;
            this.DataSourceDevolucionSolicitud.Name = "DataSourceDevolucionSolicitud";
            customSqlQuery1.Name = "CustomSqlQuery";
            queryParameter1.Name = "@EmpresaID";
            queryParameter1.Type = typeof(string);
            queryParameter1.ValueInfo = "0";
            queryParameter2.Name = "@Credito";
            queryParameter2.Type = typeof(string);
            queryParameter2.ValueInfo = "0";
            queryParameter3.Name = "@NumeroDoc";
            queryParameter3.Type = typeof(int);
            queryParameter3.ValueInfo = "0";
            queryParameter4.Name = "@NumeroDev";
            queryParameter4.Type = typeof(int);
            queryParameter4.ValueInfo = "0";
            customSqlQuery1.Parameters.Add(queryParameter1);
            customSqlQuery1.Parameters.Add(queryParameter2);
            customSqlQuery1.Parameters.Add(queryParameter3);
            customSqlQuery1.Parameters.Add(queryParameter4);
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.DataSourceDevolucionSolicitud.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.DataSourceDevolucionSolicitud.ResultSchemaSerializable = resources.GetString("DataSourceDevolucionSolicitud.ResultSchemaSerializable");
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "CustomSqlQuery";
            this.DetailReport.DataSource = this.DataSourceDevolucionSolicitud;
            this.DetailReport.Font = new System.Drawing.Font("Tahoma", 8F);
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            this.DetailReport.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail1.HeightF = 20.83333F;
            this.Detail1.Name = "Detail1";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.OddStyleName = "xrControlStyle1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(809F, 13.95833F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Codigo_Causal")});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.Weight = 0.59213517121832515D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Descripcion_Causal")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.Weight = 1.1568455643058559D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Descripcion_Causal_Grupo")});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.Weight = 1.0862075937693141D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Observaciones")});
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.Weight = 1.164811670706505D;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.lbl_nroDev,
            this.lbl_fechaDev,
            this.lbl_nombre,
            this.lbl_cliente,
            this.lbl_seUsuario,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel4,
            this.xrLabel5,
            this.tableHeaderVencimiento,
            this.shp_Titulo});
            this.PageHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.PageHeader.HeightF = 159.375F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StylePriority.UseFont = false;
            this.PageHeader.StylePriority.UseTextAlignment = false;
            this.PageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Nro_Cliente")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(224.9814F, 6.08334F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(251.0417F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.WordWrap = false;
            // 
            // lbl_nroDev
            // 
            this.lbl_nroDev.LocationFloat = new DevExpress.Utils.PointFloat(36.4397F, 75.0833F);
            this.lbl_nroDev.Name = "lbl_nroDev";
            this.lbl_nroDev.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_nroDev.SizeF = new System.Drawing.SizeF(188.5417F, 23F);
            this.lbl_nroDev.Text = "35113_nroDev";
            // 
            // lbl_fechaDev
            // 
            this.lbl_fechaDev.LocationFloat = new DevExpress.Utils.PointFloat(36.4397F, 52.08331F);
            this.lbl_fechaDev.Name = "lbl_fechaDev";
            this.lbl_fechaDev.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_fechaDev.SizeF = new System.Drawing.SizeF(188.5417F, 23F);
            this.lbl_fechaDev.Text = "35113_fechaDev";
            // 
            // lbl_nombre
            // 
            this.lbl_nombre.LocationFloat = new DevExpress.Utils.PointFloat(36.4397F, 29.08332F);
            this.lbl_nombre.Name = "lbl_nombre";
            this.lbl_nombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_nombre.SizeF = new System.Drawing.SizeF(188.5417F, 23F);
            this.lbl_nombre.Text = "35113_nombre";
            // 
            // lbl_cliente
            // 
            this.lbl_cliente.LocationFloat = new DevExpress.Utils.PointFloat(36.4397F, 6.08334F);
            this.lbl_cliente.Name = "lbl_cliente";
            this.lbl_cliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_cliente.SizeF = new System.Drawing.SizeF(188.5417F, 23F);
            this.lbl_cliente.Text = "35113_cliente";
            // 
            // lbl_seUsuario
            // 
            this.lbl_seUsuario.LocationFloat = new DevExpress.Utils.PointFloat(36.4397F, 98.08325F);
            this.lbl_seUsuario.Name = "lbl_seUsuario";
            this.lbl_seUsuario.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_seUsuario.SizeF = new System.Drawing.SizeF(188.5417F, 23.00001F);
            this.lbl_seUsuario.Text = "35113_seUsuario";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Nombre")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(224.9814F, 29.08332F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(251.0417F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.WordWrap = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.fecha_Devolucion", "{0:dd/MM/yyyy}")});
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(224.9814F, 52.08331F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(251.0416F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "xrLabel3";
            this.xrLabel3.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Consecutivo_Devolucion")});
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(224.9814F, 75.0833F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(251.0416F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Usuario")});
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(224.9814F, 98.08331F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(251.0417F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.WordWrap = false;
            // 
            // tableHeaderVencimiento
            // 
            this.tableHeaderVencimiento.BackColor = System.Drawing.Color.DimGray;
            this.tableHeaderVencimiento.BorderColor = System.Drawing.Color.White;
            this.tableHeaderVencimiento.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableHeaderVencimiento.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.tableHeaderVencimiento.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 126.7917F);
            this.tableHeaderVencimiento.Name = "tableHeaderVencimiento";
            this.tableHeaderVencimiento.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.tableHeaderVencimiento.SizeF = new System.Drawing.SizeF(809F, 25.00001F);
            this.tableHeaderVencimiento.StylePriority.UseBackColor = false;
            this.tableHeaderVencimiento.StylePriority.UseBorderColor = false;
            this.tableHeaderVencimiento.StylePriority.UseBorders = false;
            this.tableHeaderVencimiento.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tbl_codCausal,
            this.tbl_descMaestra,
            this.tbl_descMaestraGrupo,
            this.tbl_observaciones});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // tbl_codCausal
            // 
            this.tbl_codCausal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.tbl_codCausal.ForeColor = System.Drawing.Color.White;
            this.tbl_codCausal.Name = "tbl_codCausal";
            this.tbl_codCausal.StylePriority.UseFont = false;
            this.tbl_codCausal.StylePriority.UseForeColor = false;
            this.tbl_codCausal.StylePriority.UseTextAlignment = false;
            this.tbl_codCausal.Text = "35113_codCausal";
            this.tbl_codCausal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbl_codCausal.Weight = 0.304957751901624D;
            // 
            // tbl_descMaestra
            // 
            this.tbl_descMaestra.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.tbl_descMaestra.ForeColor = System.Drawing.Color.White;
            this.tbl_descMaestra.Name = "tbl_descMaestra";
            this.tbl_descMaestra.StylePriority.UseFont = false;
            this.tbl_descMaestra.StylePriority.UseForeColor = false;
            this.tbl_descMaestra.StylePriority.UseTextAlignment = false;
            this.tbl_descMaestra.Text = "35113_descMaestra";
            this.tbl_descMaestra.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbl_descMaestra.Weight = 0.59579086678367665D;
            // 
            // tbl_descMaestraGrupo
            // 
            this.tbl_descMaestraGrupo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.tbl_descMaestraGrupo.ForeColor = System.Drawing.Color.White;
            this.tbl_descMaestraGrupo.Name = "tbl_descMaestraGrupo";
            this.tbl_descMaestraGrupo.StylePriority.UseFont = false;
            this.tbl_descMaestraGrupo.StylePriority.UseForeColor = false;
            this.tbl_descMaestraGrupo.StylePriority.UseTextAlignment = false;
            this.tbl_descMaestraGrupo.Text = "35113_desCausalGrupo";
            this.tbl_descMaestraGrupo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbl_descMaestraGrupo.Weight = 0.55941157387079943D;
            // 
            // tbl_observaciones
            // 
            this.tbl_observaciones.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.tbl_observaciones.ForeColor = System.Drawing.Color.White;
            this.tbl_observaciones.Name = "tbl_observaciones";
            this.tbl_observaciones.StylePriority.UseFont = false;
            this.tbl_observaciones.StylePriority.UseForeColor = false;
            this.tbl_observaciones.StylePriority.UseTextAlignment = false;
            this.tbl_observaciones.Text = "35113_Observaciones";
            this.tbl_observaciones.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbl_observaciones.Weight = 0.599893784527985D;
            // 
            // shp_Titulo
            // 
            this.shp_Titulo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.shp_Titulo.BorderWidth = 1F;
            this.shp_Titulo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Tag", null, "CustomSqlQuery.Nombre")});
            this.shp_Titulo.LocationFloat = new DevExpress.Utils.PointFloat(10.00012F, 6.083345F);
            this.shp_Titulo.Name = "shp_Titulo";
            shapeRectangle1.Fillet = 45;
            this.shp_Titulo.Shape = shapeRectangle1;
            this.shp_Titulo.SizeF = new System.Drawing.SizeF(808.9999F, 117.0832F);
            this.shp_Titulo.StylePriority.UseBorders = false;
            this.shp_Titulo.StylePriority.UseBorderWidth = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_ReportNameDev});
            this.ReportHeader.HeightF = 45.83333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lbl_ReportNameDev
            // 
            this.lbl_ReportNameDev.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold);
            this.lbl_ReportNameDev.LocationFloat = new DevExpress.Utils.PointFloat(10.00004F, 10.00001F);
            this.lbl_ReportNameDev.Name = "lbl_ReportNameDev";
            this.lbl_ReportNameDev.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_ReportNameDev.SizeF = new System.Drawing.SizeF(808.9999F, 25F);
            this.lbl_ReportNameDev.StylePriority.UseFont = false;
            this.lbl_ReportNameDev.StylePriority.UseTextAlignment = false;
            this.lbl_ReportNameDev.Text = "35113_devsolicitud";
            this.lbl_ReportNameDev.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 127.0832F;
            this.PageFooter.Name = "PageFooter";
            // 
            // Report_cc_DevolucionSolicitud
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport,
            this.PageHeader,
            this.ReportHeader,
            this.PageFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DataSourceDevolucionSolicitud});
            this.DataSource = this.DataSourceDevolucionSolicitud;
            this.Margins = new System.Drawing.Printing.Margins(9, 12, 117, 80);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeaderVencimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle2;
        private DevExpress.DataAccess.Sql.SqlDataSource DataSourceDevolucionSolicitud;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel lbl_cliente;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel lbl_nombre;
        private DevExpress.XtraReports.UI.XRLabel lbl_fechaDev;
        private DevExpress.XtraReports.UI.XRLabel lbl_nroDev;
        private DevExpress.XtraReports.UI.XRLabel lbl_seUsuario;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTable tableHeaderVencimiento;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tbl_codCausal;
        private DevExpress.XtraReports.UI.XRTableCell tbl_descMaestra;
        private DevExpress.XtraReports.UI.XRTableCell tbl_descMaestraGrupo;
        private DevExpress.XtraReports.UI.XRTableCell tbl_observaciones;
        private DevExpress.XtraReports.UI.XRLabel lbl_ReportNameDev;
        private DevExpress.XtraReports.UI.XRShape shp_Titulo;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
    }
}
