using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Tesoreria
{
    public partial class Report_Ts_Ejecutado_Resumido : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Ts_Ejecutado_Resumido()
        {

        }

        public Report_Ts_Ejecutado_Resumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
        public string GenerateReport(DateTime fechaIni)
        {
            this.lblReportName.Visible = false;

            #region Asigna Info Header
            this.lblFechaIni.Text = fechaIni.ToShortDateString();
            
            #endregion

            #region Asigna Filtros
            this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDataSource.Queries[0].Parameters[1].Value = fechaIni;
            #endregion

            base.ConfigureConnection(this.QueriesDataSource);
            base.CreateReport();
            return base.ReportName;
    

           
        }
    }
}