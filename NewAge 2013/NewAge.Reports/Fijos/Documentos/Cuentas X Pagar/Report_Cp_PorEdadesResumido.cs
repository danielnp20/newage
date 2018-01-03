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

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_Cp_PorEdadesResumido : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_PorEdadesResumido()
        {

        }

        public Report_Cp_PorEdadesResumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_CuentasxPagarxEdadesResumido");
            this.lbl_FechaIncial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_FechaInicial:");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, string terceroID,string cuentaID, bool isDetallada)
        {
            this.xrLblFechaCorte.Text = fechaIni.ToString("MMMM' de 'yyyy");

            ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportCxPTotales> data = _moduloCxP.Report_Cp_PorEdadesResumido(fechaIni, terceroID,cuentaID, isDetallada);

            this.DataSource = data;           
            this.CreateReport();
            return this.ReportName;
        }
    }
}
