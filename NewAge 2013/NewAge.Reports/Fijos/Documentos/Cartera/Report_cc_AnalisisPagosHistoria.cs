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
using NewAge.DTO.Resultados;
using System.Diagnostics;
using System.Windows.Forms;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_cc_AnalisisPagosHistoria : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_cc_AnalisisPagosHistoria()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        public Report_cc_AnalisisPagosHistoria(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_AnalisisPagosHist"); 
          }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string cliente)
        {
            try
            {
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(cliente)? cliente : null;

                base.ConfigureConnection(this.QueriesDataSource);
                this.CreateReport();
                return base.ReportName;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: "+e);
                throw;
            }
        }
    }
}
