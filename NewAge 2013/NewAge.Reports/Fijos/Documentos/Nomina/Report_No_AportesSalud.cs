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
    public partial class Report_No_AportesSalud : ReportBase
    {
        public Report_No_AportesSalud()
        {

        }
        public Report_No_AportesSalud(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_TotalFondo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            this.lbl_Desde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblDesde");
            this.lbl_Hasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Hasta");
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_AportesaFondosDeSalud");
            this.lbl_GranTotalCalculated.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_GranTotal");
            this.lbl_Fondo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblFondo:");
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

            ModuloNomina _moduloNomina = new ModuloNomina(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_noAportesSaludTotales> data = _moduloNomina.Report_No_AportesSalud(fechaIni, fechaFin, empleadoFil, orderBy,terceroid,nofondosaludid,nocajaid);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
