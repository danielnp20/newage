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

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_AmortizaDerechoResumen : ReportBaseLandScape
    {
        #region Variables
        #endregion
        public Report_Cc_AmortizaDerechoResumen()
        {
            
        }

        public Report_Cc_AmortizaDerechoResumen(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35105_AmortizaDerechoResumen");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera)
        {
            try
            {
                #region Asigna Info Header
                this.lblFilter.Text = "Periodo: " + " " + (fechaIni.Value).ToString(FormatString.Period) + "\t   "; 
                #endregion

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(compCartera) ? compCartera : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = fechaIni;

                #endregion        

                base.ConfigureConnection(this.QueriesDataSource);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
