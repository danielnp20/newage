using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_VacacionesPagadas : ReportBase
    {
        public Report_No_VacacionesPagadas()
        {
            
        } 

        public Report_No_VacacionesPagadas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String empleadoid) 
        {
            this.lblReportName.Visible = false;

            this.CreateReport();

            return this.ReportName;
        }
    }
}
