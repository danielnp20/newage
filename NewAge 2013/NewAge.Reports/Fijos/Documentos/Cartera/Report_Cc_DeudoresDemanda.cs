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
using System.Linq;
using DevExpress.DataAccess.Sql.Native;
using DevExpress.DataAccess.Sql;
using System.Data;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_DeudoresDemanda : ReportBase
    {
        #region Variables
        private string cesionario = string.Empty;
        private List<decimal> valorRecInteres = new List<decimal>();
        private List<decimal> valorSdoCarteraAnt = new List<decimal>();
        private List<decimal> valorSaldoPrimaComp = new List<decimal>();
        #endregion 
        public Report_Cc_DeudoresDemanda()
        {
            
        }

        public Report_Cc_DeudoresDemanda(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = "";
            base.lblNombreEmpresa.Text = "";   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport()
        {
            try
            {
                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region Eventos
        /// <summary>
        /// Antesde imprimir el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAcumulado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
              
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Antesde imprimir el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDescripcion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
             
            }
            catch (Exception ex)
            {
                ;
            }
        }    
        #endregion
    }
}
