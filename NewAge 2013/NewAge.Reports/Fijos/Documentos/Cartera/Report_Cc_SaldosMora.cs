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

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_SaldosMora : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_SaldosMora()
        {

        }

        public Report_Cc_SaldosMora(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lblSaldosMora").ToUpper();
            this.lbl_Totales.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_Totales");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, int plazo, string tipoCartera)
        {
            try
            {
                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_CarteraTotales> totales = _moduloCartera.Report_Cc_SaldosMora(periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, plazo, tipoCartera);

                this.xrlFechaReporte.Text = periodo.ToString("dd  MMMM 'de' yyyy").ToUpper();

                this.DataSource = totales;
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
