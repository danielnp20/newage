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
    public partial class Report_Cc_ReintegroAjusteGiro : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion

        public Report_Cc_ReintegroAjusteGiro()
        {

        }

        public Report_Cc_ReintegroAjusteGiro(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
           
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte tipoReport, DateTime periodo, string cliente, bool pendienteInd)
        {
            try
            {
                if (tipoReport == 2)
                {
                    this.DetailReport.FilterString = "Contains([Tipo], \'Giro\')";
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Reintegro Saldos Giro");
                }
                else if (tipoReport == 3)
                {
                    this.DetailReport.FilterString = "Contains([Tipo], \'Ajuste\')";
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Reintegro Saldos Ajuste");                
                }

                this.lblPeriodo.Text = periodo.ToString("MMMM 'de' yyyy");

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(cliente) ? cliente : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = pendienteInd? 2 : 3;
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
