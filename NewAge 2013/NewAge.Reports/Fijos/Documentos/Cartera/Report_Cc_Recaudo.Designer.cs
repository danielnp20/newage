namespace NewAge.Reports
{
    partial class Report_Cc_Recaudo
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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_Recaudo));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataSourceLibranza = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRadicaPeriodo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRadicaDia = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCiudadResidencia = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCedula = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNombre = new DevExpress.XtraReports.UI.XRLabel();
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
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 361.8746F;
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
            // DataSourceLibranza
            // 
            this.DataSourceLibranza.ConnectionName = "DESARROLLOAMP\\SQLSERVER2012_NewAge_AvalConnection";
            this.DataSourceLibranza.Name = "DataSourceLibranza";
            customSqlQuery1.Name = "CustomSqlQuery";
            queryParameter1.Name = "@Empresa";
            queryParameter1.Type = typeof(string);
            queryParameter1.ValueInfo = "AVAL";
            queryParameter2.Name = "@Libranza";
            queryParameter2.Type = typeof(string);
            queryParameter2.ValueInfo = "984";
            customSqlQuery1.Parameters.Add(queryParameter1);
            customSqlQuery1.Parameters.Add(queryParameter2);
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.DataSourceLibranza.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.DataSourceLibranza.ResultSchemaSerializable = resources.GetString("DataSourceLibranza.ResultSchemaSerializable");
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "CustomSqlQuery";
            this.DetailReport.DataSource = this.DataSourceLibranza;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel15,
            this.xrLabel16,
            this.xrLabel17,
            this.xrLabel9,
            this.lblRadicaPeriodo,
            this.lblRadicaDia,
            this.xrLabel1,
            this.lblCiudadResidencia,
            this.lblCedula,
            this.lblNombre});
            this.Detail1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Detail1.HeightF = 883.2084F;
            this.Detail1.Name = "Detail1";
            this.Detail1.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBandExceptLastEntry;
            this.Detail1.StylePriority.UseFont = false;
            this.Detail1.StylePriority.UseTextAlignment = false;
            this.Detail1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ahorros", "{0:#}")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(748.3333F, 246.3751F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(37.08337F, 23F);
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.WordWrap = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.corriente", "{0:#}")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(696.2499F, 246.3751F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(38.54169F, 23F);
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel2.WordWrap = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ResidenciaDir", "{0:dd/MM/yyyy}")});
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(113.2501F, 130.5834F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel19.WordWrap = false;
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Telefono", "{0:dd/MM/yyyy}")});
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(388.9584F, 130.5834F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel18.WordWrap = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.OTRO", "{0:#}")});
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(620.2084F, 97.41656F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(30.20831F, 23F);
            this.xrLabel15.WordWrap = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.cedula", "{0:#}")});
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(509.375F, 97.41656F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(21.875F, 23F);
            this.xrLabel16.WordWrap = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.NIT", "{0:#}")});
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(563.9584F, 97.4166F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(21.875F, 23F);
            this.xrLabel17.WordWrap = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.mesRadica", "{0:#}")});
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(696.2499F, 10.00001F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(39.58331F, 23F);
            this.xrLabel9.WordWrap = false;
            // 
            // lblRadicaPeriodo
            // 
            this.lblRadicaPeriodo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.periodoRadica", "{0:#}")});
            this.lblRadicaPeriodo.LocationFloat = new DevExpress.Utils.PointFloat(646.8749F, 10.00001F);
            this.lblRadicaPeriodo.Name = "lblRadicaPeriodo";
            this.lblRadicaPeriodo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRadicaPeriodo.SizeF = new System.Drawing.SizeF(39.58331F, 23F);
            this.lblRadicaPeriodo.WordWrap = false;
            // 
            // lblRadicaDia
            // 
            this.lblRadicaDia.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.diaRadica", "{0:#}")});
            this.lblRadicaDia.LocationFloat = new DevExpress.Utils.PointFloat(748.3333F, 10.00001F);
            this.lblRadicaDia.Name = "lblRadicaDia";
            this.lblRadicaDia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRadicaDia.SizeF = new System.Drawing.SizeF(47.91669F, 23F);
            this.lblRadicaDia.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.BcoCtaNro_1", "{0:#}")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(163.5416F, 246.3751F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel1.WordWrap = false;
            // 
            // lblCiudadResidencia
            // 
            this.lblCiudadResidencia.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.CiudadRes", "{0:dd/MM/yyyy}")});
            this.lblCiudadResidencia.LocationFloat = new DevExpress.Utils.PointFloat(573.4587F, 130.5834F);
            this.lblCiudadResidencia.Name = "lblCiudadResidencia";
            this.lblCiudadResidencia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCiudadResidencia.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.lblCiudadResidencia.StylePriority.UseTextAlignment = false;
            this.lblCiudadResidencia.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblCiudadResidencia.WordWrap = false;
            // 
            // lblCedula
            // 
            this.lblCedula.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.documento", "{0:#}")});
            this.lblCedula.LocationFloat = new DevExpress.Utils.PointFloat(113.2501F, 97.41656F);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCedula.SizeF = new System.Drawing.SizeF(109.375F, 23.00003F);
            this.lblCedula.StylePriority.UseTextAlignment = false;
            this.lblCedula.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblCedula.WordWrap = false;
            // 
            // lblNombre
            // 
            this.lblNombre.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Nombre")});
            this.lblNombre.LocationFloat = new DevExpress.Utils.PointFloat(113.2501F, 71.16655F);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNombre.SizeF = new System.Drawing.SizeF(602.5001F, 21F);
            this.lblNombre.StylePriority.UseTextAlignment = false;
            this.lblNombre.Text = "lblNombre";
            this.lblNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblNombre.WordWrap = false;
            // 
            // Report_Cc_Recaudo
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DataSourceLibranza});
            this.DataSource = this.DataSourceLibranza;
            this.Margins = new System.Drawing.Printing.Margins(9, 12, 0, 362);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
            this.Version = "14.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle2;
        private DevExpress.DataAccess.Sql.SqlDataSource DataSourceLibranza;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.XRLabel lblCedula;
        private DevExpress.XtraReports.UI.XRLabel lblNombre;
        private DevExpress.XtraReports.UI.XRLabel lblCiudadResidencia;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblRadicaDia;
        private DevExpress.XtraReports.UI.XRLabel lblRadicaPeriodo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
    }
}
