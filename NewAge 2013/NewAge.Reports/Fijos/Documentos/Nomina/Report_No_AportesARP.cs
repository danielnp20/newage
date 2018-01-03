using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_AportesARP : ReportBase
    {
        public Report_No_AportesARP()
        {

        }

        public Report_No_AportesARP(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            this.lbl_Desde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblDesde");
            this.lbl_Hasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Hasta");
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_AportesaArp");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String terceroid, String nofondosaludid, String nocajaid)
        {
            this.lblReportName.Visible = false;
            this.lbFechaIni.Text = Convert.ToString(fechaIni);
            this.lbFechaFin.Text = Convert.ToString(fechaFin);

            ModuloNomina _moduloNomina = new ModuloNomina(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_noAportesARPTotales> data = _moduloNomina.Report_No_AportesARP(fechaIni, fechaFin, empleadoFil, orderBy, terceroid, nofondosaludid, nocajaid);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
