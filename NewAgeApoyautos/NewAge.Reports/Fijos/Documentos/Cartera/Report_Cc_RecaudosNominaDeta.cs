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
    public partial class Report_Cc_RecaudosNominaDeta : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_RecaudosNominaDeta()
        {

        }

        public Report_Cc_RecaudosNominaDeta(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35118").ToUpper();
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime? periodo, string centroPagoID, string estadoCruce)
        {
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                this.lblFechaReporte.Text = periodo.Value.ToString("dd  MMMM 'de' yyyy").ToUpper();
                this.lblFilter.Text = "Pagaduria(CP): " + centroPagoID + "\t   ";
                #endregion

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(centroPagoID) ? centroPagoID : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(periodo.ToString()) ? periodo : null;
                this.QueriesDataSource.Queries[0].Parameters[3].Value =!string.IsNullOrEmpty(estadoCruce) ? estadoCruce : null;
                #endregion

                base.ConfigureConnection(this.QueriesDataSource);
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
