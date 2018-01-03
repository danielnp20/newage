using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Fijos
{
    public partial class Report_in_MovimientosDetallado : ReportBase
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Report_in_MovimientosDetallado()
        {

        }

        public Report_in_MovimientosDetallado(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) 
        {
            this.lblReportName.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime? fechaIni)
        {
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
//                this.filterProy.Text = proyectoID;
                #endregion
                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = proyectoID;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = movimientoID;
                this.QueriesDataSource.Queries[0].Parameters[3].Value = bodegaID;
                this.QueriesDataSource.Queries[0].Parameters[4].Value = tipoReporte;
                this.QueriesDataSource.Queries[0].Parameters[5].Value = fechaIni;

                #endregion

                base.ConfigureConnection(this.QueriesDataSource);
                //this.ShowPreview();
                this.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void xrTable5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
