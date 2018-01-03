using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_SaldoTerceroCuenta : ReportBase
    {
        public Report_Co_SaldoTerceroCuenta()
        {

        }

        public Report_Co_SaldoTerceroCuenta(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Año");
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Saldos");
            this.xrLblInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Desde");
            this.xrLblFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Hasta");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_GranTotal");
            this.xrLblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Moneda");
            this.xrLblMonedaloc.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_MonedaLoc");
            this.xrlblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Libro");
            this.xrLblTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Tercero");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_TotalTercero");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mesInicial, int mesFin, string libro)
        {
            this.lblReportName.Visible = false;

            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportSaldosTotales> data = _moduloContabilidad.ReportesContabilidad_SaldosTercero(año, mesInicial, mesFin, libro);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
