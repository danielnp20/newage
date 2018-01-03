namespace NewAge.Reports
{
    partial class Report_Cc_Pagare
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
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report_Cc_Pagare));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.lbVlrLibranza = new DevExpress.XtraReports.UI.XRLabel();
            this.lbLibranzaLetra = new DevExpress.XtraReports.UI.XRLabel();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataSourcePagare = new DevExpress.DataAccess.Sql.SqlDataSource();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.lbLibranzaLetra2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbSolicitud = new DevExpress.XtraReports.UI.XRLabel();
            this.lbFecha = new DevExpress.XtraReports.UI.XRLabel();
            this.lbVlrLibranza1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbCuota = new DevExpress.XtraReports.UI.XRLabel();
            this.lbVlrCuota = new DevExpress.XtraReports.UI.XRLabel();
            this.lbPagador = new DevExpress.XtraReports.UI.XRLabel();
            this.lbCiudad = new DevExpress.XtraReports.UI.XRLabel();
            this.lbSolicitudDesc = new DevExpress.XtraReports.UI.XRLabel();
            this.lbCedula = new DevExpress.XtraReports.UI.XRLabel();
            this.lbNombre = new DevExpress.XtraReports.UI.XRLabel();
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
            // lbVlrLibranza
            // 
            this.lbVlrLibranza.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrLibranza", "{0:n2}")});
            this.lbVlrLibranza.LocationFloat = new DevExpress.Utils.PointFloat(92.70834F, 214.3334F);
            this.lbVlrLibranza.Name = "lbVlrLibranza";
            this.lbVlrLibranza.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbVlrLibranza.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbVlrLibranza.Text = "lbVlrLibranza";
            this.lbVlrLibranza.WordWrap = false;
            this.lbVlrLibranza.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbVlrLibranza_BeforePrint);
            // 
            // lbLibranzaLetra
            // 
            this.lbLibranzaLetra.LocationFloat = new DevExpress.Utils.PointFloat(129.9584F, 280.7084F);
            this.lbLibranzaLetra.Name = "lbLibranzaLetra";
            this.lbLibranzaLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbLibranzaLetra.SizeF = new System.Drawing.SizeF(682.9999F, 18.83334F);
            this.lbLibranzaLetra.WordWrap = false;
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
            // DataSourcePagare
            // 
            this.DataSourcePagare.ConnectionName = "localhost_NewAgeConnection";
            msSqlConnectionParameters1.AuthorizationType = DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.Windows;
            msSqlConnectionParameters1.DatabaseName = "NewAge";
            msSqlConnectionParameters1.ServerName = "localhost";
            this.DataSourcePagare.ConnectionParameters = msSqlConnectionParameters1;
            this.DataSourcePagare.Name = "DataSourcePagare";
            customSqlQuery1.Name = "CustomSqlQuery";
            queryParameter1.Name = "@Empresa";
            queryParameter1.Type = typeof(string);
            queryParameter1.ValueInfo = "0";
            queryParameter2.Name = "@mesIni";
            queryParameter2.Type = typeof(short);
            queryParameter2.ValueInfo = "0";
            queryParameter3.Name = "@mesFin";
            queryParameter3.Type = typeof(short);
            queryParameter3.ValueInfo = "0";
            queryParameter4.Name = "@año";
            queryParameter4.Type = typeof(short);
            queryParameter4.ValueInfo = "0";
            queryParameter5.Name = "@libranza";
            queryParameter5.Type = typeof(string);
            queryParameter5.ValueInfo = "0";
            customSqlQuery1.Parameters.Add(queryParameter1);
            customSqlQuery1.Parameters.Add(queryParameter2);
            customSqlQuery1.Parameters.Add(queryParameter3);
            customSqlQuery1.Parameters.Add(queryParameter4);
            customSqlQuery1.Parameters.Add(queryParameter5);
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.DataSourcePagare.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.DataSourcePagare.ResultSchemaSerializable = resources.GetString("DataSourcePagare.ResultSchemaSerializable");
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "CustomSqlQuery";
            this.DetailReport.DataSource = this.DataSourcePagare;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbLibranzaLetra2,
            this.lbSolicitud,
            this.lbFecha,
            this.lbVlrLibranza1,
            this.lbCuota,
            this.lbVlrCuota,
            this.lbPagador,
            this.lbCiudad,
            this.lbSolicitudDesc,
            this.lbCedula,
            this.lbNombre,
            this.lbVlrLibranza,
            this.lbLibranzaLetra});
            this.Detail1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.Detail1.HeightF = 647.7917F;
            this.Detail1.Name = "Detail1";
            this.Detail1.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBandExceptLastEntry;
            this.Detail1.StylePriority.UseFont = false;
            this.Detail1.StylePriority.UseTextAlignment = false;
            this.Detail1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lbLibranzaLetra2
            // 
            this.lbLibranzaLetra2.LocationFloat = new DevExpress.Utils.PointFloat(129.9584F, 195.3334F);
            this.lbLibranzaLetra2.Name = "lbLibranzaLetra2";
            this.lbLibranzaLetra2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbLibranzaLetra2.SizeF = new System.Drawing.SizeF(682.9999F, 23F);
            // 
            // lbSolicitud
            // 
            this.lbSolicitud.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Solicitud", "{0:#}")});
            this.lbSolicitud.LocationFloat = new DevExpress.Utils.PointFloat(721.4583F, 13.125F);
            this.lbSolicitud.Name = "lbSolicitud";
            this.lbSolicitud.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbSolicitud.SizeF = new System.Drawing.SizeF(62.49994F, 23F);
            this.lbSolicitud.StylePriority.UseTextAlignment = false;
            this.lbSolicitud.Text = "lbSolicitud";
            this.lbSolicitud.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbSolicitud.WordWrap = false;
            // 
            // lbFecha
            // 
            this.lbFecha.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Fecha", "{0:dd/MM/yyyy}")});
            this.lbFecha.LocationFloat = new DevExpress.Utils.PointFloat(453F, 97.12489F);
            this.lbFecha.Name = "lbFecha";
            this.lbFecha.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbFecha.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbFecha.Text = "lbFecha";
            this.lbFecha.WordWrap = false;
            // 
            // lbVlrLibranza1
            // 
            this.lbVlrLibranza1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrLibranza", "{0:n2}")});
            this.lbVlrLibranza1.LocationFloat = new DevExpress.Utils.PointFloat(650.4167F, 97.12489F);
            this.lbVlrLibranza1.Name = "lbVlrLibranza1";
            this.lbVlrLibranza1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbVlrLibranza1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbVlrLibranza1.Text = "lbVlrLibranza1";
            this.lbVlrLibranza1.WordWrap = false;
            // 
            // lbCuota
            // 
            this.lbCuota.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Cuota", "{0:#}")});
            this.lbCuota.LocationFloat = new DevExpress.Utils.PointFloat(368.75F, 214.3334F);
            this.lbCuota.Name = "lbCuota";
            this.lbCuota.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbCuota.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbCuota.Text = "lbCuota";
            this.lbCuota.WordWrap = false;
            // 
            // lbVlrCuota
            // 
            this.lbVlrCuota.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.VlrCuota", "{0:n2}")});
            this.lbVlrCuota.LocationFloat = new DevExpress.Utils.PointFloat(650.4583F, 213.3334F);
            this.lbVlrCuota.Name = "lbVlrCuota";
            this.lbVlrCuota.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbVlrCuota.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbVlrCuota.Text = "lbVlrCuota";
            this.lbVlrCuota.WordWrap = false;
            // 
            // lbPagador
            // 
            this.lbPagador.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Pagador")});
            this.lbPagador.LocationFloat = new DevExpress.Utils.PointFloat(510.4166F, 581.2084F);
            this.lbPagador.Name = "lbPagador";
            this.lbPagador.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbPagador.SizeF = new System.Drawing.SizeF(176.0417F, 23F);
            this.lbPagador.WordWrap = false;
            // 
            // lbCiudad
            // 
            this.lbCiudad.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Ciudad", "{0}")});
            this.lbCiudad.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lbCiudad.LocationFloat = new DevExpress.Utils.PointFloat(92.70834F, 97.12489F);
            this.lbCiudad.Name = "lbCiudad";
            this.lbCiudad.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbCiudad.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbCiudad.StylePriority.UseFont = false;
            this.lbCiudad.Text = "lbCiudad";
            this.lbCiudad.WordWrap = false;
            // 
            // lbSolicitudDesc
            // 
            this.lbSolicitudDesc.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbSolicitudDesc.LocationFloat = new DevExpress.Utils.PointFloat(645.4166F, 13.125F);
            this.lbSolicitudDesc.Name = "lbSolicitudDesc";
            this.lbSolicitudDesc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbSolicitudDesc.SizeF = new System.Drawing.SizeF(76.04169F, 23F);
            this.lbSolicitudDesc.StylePriority.UseFont = false;
            this.lbSolicitudDesc.StylePriority.UseTextAlignment = false;
            this.lbSolicitudDesc.Text = "35112_solicitud";
            this.lbSolicitudDesc.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbCedula
            // 
            this.lbCedula.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Cedula", "{0:#}")});
            this.lbCedula.LocationFloat = new DevExpress.Utils.PointFloat(436.625F, 135.9583F);
            this.lbCedula.Name = "lbCedula";
            this.lbCedula.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbCedula.SizeF = new System.Drawing.SizeF(109.375F, 23.00003F);
            this.lbCedula.Text = "lbCedula";
            this.lbCedula.WordWrap = false;
            // 
            // lbNombre
            // 
            this.lbNombre.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSqlQuery.Nombre")});
            this.lbNombre.LocationFloat = new DevExpress.Utils.PointFloat(123.9583F, 119.1249F);
            this.lbNombre.Name = "lbNombre";
            this.lbNombre.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbNombre.SizeF = new System.Drawing.SizeF(267.71F, 21F);
            this.lbNombre.Text = "lbNombre";
            this.lbNombre.WordWrap = false;
            // 
            // Report_Cc_Pagare
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DataSourcePagare});
            this.DataSource = this.DataSourcePagare;
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
        private DevExpress.DataAccess.Sql.SqlDataSource DataSourcePagare;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.XRLabel lbCedula;
        private DevExpress.XtraReports.UI.XRLabel lbNombre;
        private DevExpress.XtraReports.UI.XRLabel lbSolicitudDesc;
        private DevExpress.XtraReports.UI.XRLabel lbLibranzaLetra;
        private DevExpress.XtraReports.UI.XRLabel lbCiudad;
        private DevExpress.XtraReports.UI.XRLabel lbPagador;
        private DevExpress.XtraReports.UI.XRLabel lbSolicitud;
        private DevExpress.XtraReports.UI.XRLabel lbFecha;
        private DevExpress.XtraReports.UI.XRLabel lbVlrLibranza1;
        private DevExpress.XtraReports.UI.XRLabel lbCuota;
        private DevExpress.XtraReports.UI.XRLabel lbVlrCuota;
        private DevExpress.XtraReports.UI.XRLabel lbVlrLibranza;
        private DevExpress.XtraReports.UI.XRLabel lbLibranzaLetra2;
    }
}
