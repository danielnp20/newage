using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_Prestamo : ReportBase
    {
        public Report_No_Prestamo()
        {
            
        }

        public Report_No_Prestamo(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, bool orderByName, String empleadoid)
        {
            this.lblReportName.Visible = false;
            this.CreateReport();
            return this.ReportName;
        }
    }
}
