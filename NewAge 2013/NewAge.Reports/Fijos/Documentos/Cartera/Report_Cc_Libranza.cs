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
using NewAge.DTO.Resultados;

namespace NewAge.Reports
{
    public partial class Report_Cc_Libranza : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_Libranza()
        {

        }

        public Report_Cc_Libranza(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35029_lblLibranzas");
            this.lbl_FechaIncial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35029_FechaInicial");
            this.lbl_fechafin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35029_FechaFinal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35029_LblTotal");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime Periodo, DateTime PeriodoFin, string Cliente, string Libranza, string Asesor, string Pagaduria)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                this.xrLabel1.Text = Periodo.ToString().Substring(0, 10);
                this.xrLabel3.Text = PeriodoFin.ToString().Substring(0, 10);
                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_CarteraTotales> data = _moduloCartera.Report_Cc_Libranzas(Periodo, PeriodoFin, Cliente, Libranza, Asesor, Pagaduria);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                }
                return result;
            }
            catch (Exception)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                return result;
            }
        }

        private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

    }
}
