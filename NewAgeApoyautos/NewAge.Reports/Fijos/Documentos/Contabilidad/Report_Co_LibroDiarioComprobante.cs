using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Negocio;
using System.Windows.Forms;
using NewAge.Librerias.Project;

namespace NewAge.Reports
{
    public partial class Report_Co_LibroDiarioComprobante : ReportBase
    {
        public Report_Co_LibroDiarioComprobante()
        {

        }

        public Report_Co_LibroDiarioComprobante(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_Año");
            this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_Mes");
            this.xrLblTipoBalan.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_TipoBalance");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_LibroDiario");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_GranTotal");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mes, string tipoBalance)
        {
            try
            {
                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_LibroDiarioComprobante(año, mes, tipoBalance);

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
