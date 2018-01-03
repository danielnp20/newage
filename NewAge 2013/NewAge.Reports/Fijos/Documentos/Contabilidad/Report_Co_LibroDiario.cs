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
    public partial class Report_Co_LibroDiario : ReportBase
    {
        public Report_Co_LibroDiario()
        {

        }

        public Report_Co_LibroDiario(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35002_Mes");
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
            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_LibroDiario(año, mes, tipoBalance);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
