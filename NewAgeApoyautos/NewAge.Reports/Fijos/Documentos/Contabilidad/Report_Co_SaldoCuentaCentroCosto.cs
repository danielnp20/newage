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
    public partial class Report_Co_SaldoCuentaCentroCosto : ReportBase
    {
        public Report_Co_SaldoCuentaCentroCosto()
        {

        }

        public Report_Co_SaldoCuentaCentroCosto(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
            this.xrLblCuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Cuenta");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35010_Total");
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
            List<DTO_ReportSaldosTotales> data = _moduloContabilidad.ReportesContabilidad_SaldosCuenta(año, mesInicial, mesFin, libro);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
