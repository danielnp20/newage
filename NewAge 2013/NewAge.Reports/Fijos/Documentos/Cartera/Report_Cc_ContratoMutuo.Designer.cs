namespace NewAge.Reports.Dinamicos
{
    partial class Report_Cc_ContratoMutuo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_ContratoMutuo));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataSourceLibranza = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrestamoLetra = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPeriodo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiaRadica = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRadicaPeriodo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRadicaDia = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPLazoLetra2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblValorCuotaLetra = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVlrCuota = new DevExpress.XtraReports.UI.XRLabel();
            this.lblValorLetra = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCiudadExp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPLazoLetra = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCiudadResidencia = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPlazo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVlrPrestamo = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel10,
            this.lblPrestamoLetra,
            this.lblPeriodo,
            this.lblDiaRadica,
            this.xrLabel11,
            this.xrLabel12,
            this.lblRadicaPeriodo,
            this.lblRadicaDia,
            this.lblPLazoLetra2,
            this.lblValorCuotaLetra,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.lblVlrCuota,
            this.lblValorLetra,
            this.xrLabel1,
            this.lblCiudadExp,
            this.lblPLazoLetra,
            this.lblCiudadResidencia,
            this.lblPlazo,
            this.lblVlrPrestamo,
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
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ResidenciaDir")});
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(586.4583F, 860.2083F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.periodoRadica", "{0:#}")});
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(313.5417F, 860.2083F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel13.WordWrap = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.diaRadica", "{0:#}")});
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(550.4167F, 834.375F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel10.WordWrap = false;
            // 
            // lblPrestamoLetra
            // 
            this.lblPrestamoLetra.LocationFloat = new DevExpress.Utils.PointFloat(113.2501F, 153.5834F);
            this.lblPrestamoLetra.Name = "lblPrestamoLetra";
            this.lblPrestamoLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPrestamoLetra.SizeF = new System.Drawing.SizeF(682.9999F, 23F);
            this.lblPrestamoLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPrestamoTexto_BeforePrint);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.LocationFloat = new DevExpress.Utils.PointFloat(163.5416F, 860.2084F);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPeriodo.SizeF = new System.Drawing.SizeF(118.75F, 23F);
            this.lblPeriodo.WordWrap = false;
            this.lblPeriodo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPeriodo_BeforePrint);
            // 
            // lblDiaRadica
            // 
            this.lblDiaRadica.LocationFloat = new DevExpress.Utils.PointFloat(292.0834F, 834.375F);
            this.lblDiaRadica.Name = "lblDiaRadica";
            this.lblDiaRadica.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDiaRadica.SizeF = new System.Drawing.SizeF(252.0833F, 23F);
            this.lblDiaRadica.WordWrap = false;
            this.lblDiaRadica.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblDiaRadica_BeforePrint);
            // 
            // xrLabel11
            // 
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.mesRadica", "{0:#}")});
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(670.8333F, 834.375F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel11.WordWrap = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.fechacuota1", "{0:dd/MM/yyyy}")});
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(431.25F, 597.9999F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel12.WordWrap = false;
            // 
            // lblRadicaPeriodo
            // 
            this.lblRadicaPeriodo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.periodoRadica", "{0:#}")});
            this.lblRadicaPeriodo.LocationFloat = new DevExpress.Utils.PointFloat(380.2084F, 10.00001F);
            this.lblRadicaPeriodo.Name = "lblRadicaPeriodo";
            this.lblRadicaPeriodo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRadicaPeriodo.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lblRadicaPeriodo.Visible = false;
            this.lblRadicaPeriodo.WordWrap = false;
            // 
            // lblRadicaDia
            // 
            this.lblRadicaDia.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.diaRadica", "{0:#}")});
            this.lblRadicaDia.LocationFloat = new DevExpress.Utils.PointFloat(261.4583F, 10.00001F);
            this.lblRadicaDia.Name = "lblRadicaDia";
            this.lblRadicaDia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRadicaDia.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lblRadicaDia.Visible = false;
            this.lblRadicaDia.WordWrap = false;
            // 
            // lblPLazoLetra2
            // 
            this.lblPLazoLetra2.LocationFloat = new DevExpress.Utils.PointFloat(544.1667F, 575F);
            this.lblPLazoLetra2.Name = "lblPLazoLetra2";
            this.lblPLazoLetra2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPLazoLetra2.SizeF = new System.Drawing.SizeF(252.0833F, 23F);
            this.lblPLazoLetra2.WordWrap = false;
            this.lblPLazoLetra2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPLazoLetra2_BeforePrint);
            // 
            // lblValorCuotaLetra
            // 
            this.lblValorCuotaLetra.LocationFloat = new DevExpress.Utils.PointFloat(498.9583F, 534.375F);
            this.lblValorCuotaLetra.Name = "lblValorCuotaLetra";
            this.lblValorCuotaLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblValorCuotaLetra.SizeF = new System.Drawing.SizeF(297.2917F, 23F);
            this.lblValorCuotaLetra.WordWrap = false;
            this.lblValorCuotaLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblValorCuotaLetra_BeforePrint);
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrCuota", "{0:n0}")});
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(431.25F, 575F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel8.WordWrap = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Pagaduria")});
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(22.62509F, 491.6667F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(275.2916F, 23.00003F);
            this.xrLabel7.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Descripcion")});
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(696.25F, 469.7917F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ciudadObli")});
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(696.25F, 308.3333F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel5.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.dia", "{0:n0}")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(431.25F, 246.4584F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel4.WordWrap = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.dia", "{0:n0}")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(313.5416F, 246.4584F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.WordWrap = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrCuota", "{0:n0}")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(58.33337F, 246.4584F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.WordWrap = false;
            // 
            // lblVlrCuota
            // 
            this.lblVlrCuota.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrCuota", "{0:#}")});
            this.lblVlrCuota.LocationFloat = new DevExpress.Utils.PointFloat(139.3334F, 10.00001F);
            this.lblVlrCuota.Name = "lblVlrCuota";
            this.lblVlrCuota.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblVlrCuota.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lblVlrCuota.Visible = false;
            this.lblVlrCuota.WordWrap = false;
            // 
            // lblValorLetra
            // 
            this.lblValorLetra.LocationFloat = new DevExpress.Utils.PointFloat(498.9583F, 218.3334F);
            this.lblValorLetra.Name = "lblValorLetra";
            this.lblValorLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblValorLetra.SizeF = new System.Drawing.SizeF(297.2917F, 23F);
            this.lblValorLetra.WordWrap = false;
            this.lblValorLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblValorLetra_BeforePrint);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Plazo", "{0:#}")});
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(313.5417F, 218.3334F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.WordWrap = false;
            // 
            // lblCiudadExp
            // 
            this.lblCiudadExp.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.ciudadExp", "{0:}")});
            this.lblCiudadExp.LocationFloat = new DevExpress.Utils.PointFloat(279.1667F, 118.25F);
            this.lblCiudadExp.Name = "lblCiudadExp";
            this.lblCiudadExp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCiudadExp.SizeF = new System.Drawing.SizeF(109.375F, 23.00003F);
            this.lblCiudadExp.WordWrap = false;
            // 
            // lblPLazoLetra
            // 
            this.lblPLazoLetra.LocationFloat = new DevExpress.Utils.PointFloat(113.2501F, 195.3334F);
            this.lblPLazoLetra.Name = "lblPLazoLetra";
            this.lblPLazoLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPLazoLetra.SizeF = new System.Drawing.SizeF(682.9999F, 23F);
            this.lblPLazoLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPLazoLetra_BeforePrint);
            // 
            // lblCiudadResidencia
            // 
            this.lblCiudadResidencia.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.CiudadRes", "{0:dd/MM/yyyy}")});
            this.lblCiudadResidencia.LocationFloat = new DevExpress.Utils.PointFloat(544.1667F, 97.41656F);
            this.lblCiudadResidencia.Name = "lblCiudadResidencia";
            this.lblCiudadResidencia.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCiudadResidencia.SizeF = new System.Drawing.SizeF(142.2916F, 23F);
            this.lblCiudadResidencia.WordWrap = false;
            // 
            // lblPlazo
            // 
            this.lblPlazo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Plazo", "{0:#}")});
            this.lblPlazo.LocationFloat = new DevExpress.Utils.PointFloat(22.62509F, 10.00001F);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPlazo.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lblPlazo.Visible = false;
            this.lblPlazo.WordWrap = false;
            // 
            // lblVlrPrestamo
            // 
            this.lblVlrPrestamo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrPrestamo", "{0:n0}")});
            this.lblVlrPrestamo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVlrPrestamo.LocationFloat = new DevExpress.Utils.PointFloat(650.4167F, 48.16656F);
            this.lblVlrPrestamo.Name = "lblVlrPrestamo";
            this.lblVlrPrestamo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblVlrPrestamo.SizeF = new System.Drawing.SizeF(145.8333F, 23F);
            this.lblVlrPrestamo.StylePriority.UseFont = false;
            this.lblVlrPrestamo.WordWrap = false;
            // 
            // lblCedula
            // 
            this.lblCedula.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Cedula", "{0:#}")});
            this.lblCedula.LocationFloat = new DevExpress.Utils.PointFloat(129.9584F, 118.25F);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCedula.SizeF = new System.Drawing.SizeF(109.375F, 23.00003F);
            this.lblCedula.Text = "lblCedula";
            this.lblCedula.WordWrap = false;
            // 
            // lblNombre
            // 
            this.lblNombre.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Nombre")});
            this.lblNombre.LocationFloat = new DevExpress.Utils.PointFloat(47.91663F, 76.41656F);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNombre.SizeF = new System.Drawing.SizeF(602.5001F, 21F);
            this.lblNombre.Text = "lblNombre";
            this.lblNombre.WordWrap = false;
            // 
            // Report_Cc_ContratoMutuo
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
        private DevExpress.XtraReports.UI.XRLabel lblVlrPrestamo;
        private DevExpress.XtraReports.UI.XRLabel lblCiudadResidencia;
        private DevExpress.XtraReports.UI.XRLabel lblPlazo;
        private DevExpress.XtraReports.UI.XRLabel lblPLazoLetra;
        private DevExpress.XtraReports.UI.XRLabel lblCiudadExp;
        private DevExpress.XtraReports.UI.XRLabel lblVlrCuota;
        private DevExpress.XtraReports.UI.XRLabel lblValorLetra;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblValorCuotaLetra;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblPLazoLetra2;
        private DevExpress.XtraReports.UI.XRLabel lblRadicaDia;
        private DevExpress.XtraReports.UI.XRLabel lblPeriodo;
        private DevExpress.XtraReports.UI.XRLabel lblDiaRadica;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel lblRadicaPeriodo;
        private DevExpress.XtraReports.UI.XRLabel lblPrestamoLetra;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
    }
}
