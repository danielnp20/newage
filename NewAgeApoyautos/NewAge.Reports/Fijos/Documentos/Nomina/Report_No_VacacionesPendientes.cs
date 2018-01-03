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
    public partial class Report_No_VacacionesPendientes : ReportBase
    {
        public Report_No_VacacionesPendientes()
        {
            
        } 

        public Report_No_VacacionesPendientes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _empleadoID, int _vacaciones) 
        {
            this.lblReportName.Visible = false;
                       
            this.DataSourceVacionesPendientes.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceVacionesPendientes.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(_empleadoID.ToString()) ? _empleadoID.ToString() : null;
            this.DataSourceVacionesPendientes.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(_vacaciones.ToString()) ? _vacaciones.ToString() : null;

            this.ConfigureConnection(this.DataSourceVacionesPendientes);
            this.CreateReport();

            return this.ReportName;
        }
    }
}
