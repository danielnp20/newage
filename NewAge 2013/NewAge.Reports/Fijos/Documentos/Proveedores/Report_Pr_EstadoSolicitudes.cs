using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Pr_EstadoSolicitudes : ReportBaseLandScape
    {
        public Report_Pr_EstadoSolicitudes()
        {

        }

        public Report_Pr_EstadoSolicitudes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35088_EstadoSolicitudes");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(Dictionary<int, string> filtros)
        {
            this.lblReportName.Visible = false;

            ModuloProveedores _moduloProveedores = new ModuloProveedores(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ProveedoresTotal> data = _moduloProveedores.ReportesProveedores_Solicitudes(filtros);

            this.DataSource = data;
            this.ShowPreview();
            //this.CreateReport();
            return this.ReportName;
        }

    }
}
