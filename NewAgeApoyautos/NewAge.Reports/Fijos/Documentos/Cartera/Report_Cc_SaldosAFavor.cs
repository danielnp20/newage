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
    public partial class Report_Cc_SaldosAFavor : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion

        public Report_Cc_SaldosAFavor()
        {

        }

        public Report_Cc_SaldosAFavor(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_SaldosAFavor");
            this.lbl_NombreReporte.Text.ToUpper();
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor)
        {
            try
            {
                this.lblReportName.Visible = false;
                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_CarteraTotales> totales = _moduloCartera.Report_Cc_Saldos(periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor);
                this.lblPeriodo.Text = periodo.ToString("dd  MMMM 'de' yyyy").ToUpper();

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
