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
    public partial class Report_py_ResumenDocsCotizacion : ReportBase
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Report_py_ResumenDocsCotizacion()
        {

        }

        public Report_py_ResumenDocsCotizacion(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
        public string GenerateReport(DateTime mesIni, DateTime mesFin)
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
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = mesIni;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = mesFin;
                #endregion

                base.ConfigureConnection(this.QueriesDataSource);
                this.ShowPreview();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
