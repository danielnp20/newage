using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_Incorporacion : ReportBase
    {
        public Report_Cc_Incorporacion()
        {

        }

        public Report_Cc_Incorporacion(DTO_glEmpresa empresa) 
            : base(empresa)
        {
            this.InitializeComponent();
        }

        public Report_Cc_Incorporacion(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_ReporteMensualVentaDiaria");
            //this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Año");
            //this.xrLblFiltro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Filtrado");
            //this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Desde");
            //this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Hasta");

        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc, bool isLiquidacion)
        {
            this.lblReportName.Visible = false;
            this.lblNombreEmpresa.Visible = false;

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ccCartas> data = _moduloCartera.Report_Cc_Incorporacion(numeroDoc, isLiquidacion);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
