using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;

namespace NewAge.Reports.Dinamicos
{
    public partial class ReportesCuentasXPagar_LegalizaTarjetas : ReportBase
    {
         public ReportesCuentasXPagar_LegalizaTarjetas()
        {

        }

         public ReportesCuentasXPagar_LegalizaTarjetas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblResponsable.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_Responsable");
            this.xrLblNumTarjeta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_NumTarjeta");
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_LegalizaTarjetas");
            this.xrLblFechaIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_FechaIni");
            this.xrLblFechaFin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_FechaFin");
            this.xrLblCargoEspecial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_CargoEspecial");
            this.xrLblTotalLegalizado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35098_TotalLegalizado");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numDoc)
        {
            this.lblReportName.Visible = false;

            ModuloCuentasXPagar _moduloCuentaXPagar = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportBaseCXP> data = _moduloCuentaXPagar.ReportesCuentasXPagar_LegalizaTarjeta(numDoc);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
