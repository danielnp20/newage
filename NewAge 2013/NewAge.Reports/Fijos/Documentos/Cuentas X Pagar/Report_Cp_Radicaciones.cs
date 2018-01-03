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

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_Cp_Radicaciones : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_Radicaciones()
        {

        }

        public Report_Cp_Radicaciones(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35075_ReporteDeRadicacion");
            this.xrLlbMda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35075_MdaOrg");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35075_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden)
        {
            this.lblReportName.Visible = false;
            this.xrTableCell11.CanGrow = false;
            this.xrTableCell11.CanShrink = false;

            ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportCxPTotales> data = _moduloCxP.Report_Radicaciones(yearIni, yearFin,fechaIni,fechaFin,Tercero,Estado, Orden);
            
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
