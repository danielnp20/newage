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
using NewAge.Librerias.Project;

namespace NewAge.Reports
{
    public partial class Report_No_CesantiasDetalle : ReportBase
    {
        //Valor
        public Report_No_CesantiasDetalle()
        {
            
        } 

        public Report_No_CesantiasDetalle(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _empleadoID, int docID, string fondoID) 
        {
            try
            {
                this.lblReportName.Visible = false;

                this.DataSourceDocVacaciones2.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(_empleadoID.ToString()) ? _empleadoID.ToString() : null;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(docID.ToString()) ? docID.ToString() : null;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[3].Value = fondoID;

                this.ConfigureConnection(this.DataSourceDocVacaciones2);

                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                throw;
            }
        }
    }
}
