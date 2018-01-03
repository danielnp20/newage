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
    public partial class Report_py_PlanCostosResumido : ReportBase
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Report_py_PlanCostosResumido()
        {

        }

        public Report_py_PlanCostosResumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) 
        {
            //    this.xrLbl_Año.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Año");
            //    this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Desde");
            //    this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Hasta");
            //    this.xrLblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_Moneda");
            //    this.xrLblLibroAux.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_LibroBalance");
            //    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35091_NombreReporte");                   
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
        public string GenerateReport(DateTime? mesIni, DateTime? mesFin)
        {
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                //filter = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_FechaFin") + " " + fechaFin.ToShortDateString() + "\t   ";
                //if (!string.IsNullOrEmpty(cliente))
                //    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ClienteID") + " " + cliente + "\t   ";
                #endregion
                #region Asigna Filtros
                //this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                //this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(cliente) ? cliente : null;
                //this.QueriesDataSource.Queries[0].Parameters[10].Value = fechaFin;
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
