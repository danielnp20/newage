using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_CxC_PorEdadesResumido : ReportBase
    {
        public Report_CxC_PorEdadesResumido()
        {
        }

        public Report_CxC_PorEdadesResumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_CuentasxCobrarxEdadesResumido");
            this.lbl_FechaIncial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_FechaInicial:");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaCorte, string tercero, bool isDetallada)
        {
            this.lblReportName.Visible = false;
            this.xrLblFechaCorte.Text = fechaCorte.ToString("MMMM' de 'yyyy");

            ModuloFacturacion _moduloFacturacion = new ModuloFacturacion(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_FacturacionTotales> data = _moduloFacturacion.ReportesFacturacion_CxCPorEdadesResumida(fechaCorte, tercero, isDetallada);

            this.DataSource = data;           
            this.CreateReport();

            return this.ReportName;
        }
    }
}
