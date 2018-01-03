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
    public partial class Report_No_ProvisionesDetalle : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion

        public Report_No_ProvisionesDetalle()
        {

        }

        public Report_No_ProvisionesDetalle(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35120_lbl_DetalleProvisiones");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string empleado)
        {
            try
            {
                this.lblPeriodo.Text = periodo.ToString("MMMM / yyyy").ToUpper();

                this.DataSourceQueries.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;             
                this.DataSourceQueries.Queries[0].Parameters[1].Value = periodo;
                this.DataSourceQueries.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(empleado) ? empleado : null;

                this.ConfigureConnection(this.DataSourceQueries);

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
