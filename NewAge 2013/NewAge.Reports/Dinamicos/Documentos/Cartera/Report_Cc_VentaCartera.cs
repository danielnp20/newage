using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_VentaCartera : ReportBase
    {
        public Report_Cc_VentaCartera()
        {

        }

        public Report_Cc_VentaCartera(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35090_CesionCartera");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc)
        {
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ccCartas> data = _moduloCartera.Report_Cc_Cesion(numeroDoc);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
