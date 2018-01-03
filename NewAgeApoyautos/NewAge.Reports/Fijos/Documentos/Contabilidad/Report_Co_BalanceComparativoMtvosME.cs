using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_BalanceComparativoMtvosME : ReportBase
    {
        public Report_Co_BalanceComparativoMtvosME()
        {

        }

        public Report_Co_BalanceComparativoMtvosME(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_BalanceComparativos");
            this.xrLbl_Año.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Año");
            this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Desde");
            this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Hasta");
            this.xrLblLirboFunc.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_LibroFuncional");
            this.xrLblFUNC.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_FUNC");
            this.xrLblLibroAux.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_LibroAux");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Total");
            this.xrLblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string libroAux, string moneda, int año, DateTime fechaFin, DateTime fechaIni)
        {
            this.lblReportName.Visible = false;
            this.xrLblMonedas.Text = moneda;
            this.xrLblMesIni.Text = Convert.ToString(fechaIni.ToString("MMMM")).ToUpper();
            this.xrLblFechaFin.Text = Convert.ToString(fechaFin.ToString("MMMM")).ToUpper();
            this.xrLblAño.Text = año.ToString();

            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ContabilidadTotal> data = _moduloContabilidad.ReportesContabilidad_BalanceComparativo(libroAux, fechaFin, fechaIni, año);
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
