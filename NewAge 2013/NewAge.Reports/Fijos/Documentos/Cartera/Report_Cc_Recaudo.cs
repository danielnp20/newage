using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_Cc_Recaudo : ReportBase
    {

        #region Variable
        // variable para convertir en letras los valores
        private string mdaLocal = string.Empty;
        //Variable de error
        string Libranzavalidation = string.Empty;
        #endregion

        public Report_Cc_Recaudo(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
        public string GenerateReport(string libranza)
        {
            // Convierte Valor en Letras
            this.mdaLocal = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            // No muestra la descripciomn de reporte Padre
            this.lblNombreEmpresa.Visible = false;
            this.lblReportName.Visible = false;
            this.imgLogoEmpresa.Visible = false;
            this.lblUser.Visible = false;
            this.lblUserName.Visible = false;
            this.lblFecha.Visible = false;
            this.xrPageInfo1.Visible = false;
            this.xrPageInfo2.Visible = false;
            this.lblPage.Visible = false;

            // Parametros para trabajar sobre el Query
            this.DataSourceLibranza.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;         
            this.DataSourceLibranza.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(libranza.ToString()) ? libranza.ToString() : null;

            base.ConfigureConnection(this.DataSourceLibranza);

            

                this.CreateReport();
                return base.ReportName;

        }
   
    }
}
