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

namespace NewAge.Reports.Fijos
{
    public partial class Report_Cp_LibroCompras : ReportBaseLandScape
    {
        public Report_Cp_LibroCompras()
        {

        }

        public Report_Cp_LibroCompras(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35039_LibroCompras");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35039_Año");
            this.lblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35039_Mes");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fecha, string tercero, bool facturaEquivalente)
        {
            try
            {
                ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> data = _moduloCxP.Reportes_Cp_LibroCompras(fecha, tercero, facturaEquivalente);

                if (data.Count > 0)
                    data[0].FechaIni = fecha;

                this.DataSource = data;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
