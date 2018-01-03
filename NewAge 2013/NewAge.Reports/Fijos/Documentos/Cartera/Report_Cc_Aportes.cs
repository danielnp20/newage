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
    public partial class Report_Cc_Aportes : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_Aportes()
        {

        }

        public Report_Cc_Aportes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, DateTime mes, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_Afiliaciones");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_Fecha");
            this.xrLblPagaduria.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lblPagaduria");
            this.xrLblTotalPag.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_LblTotalPagaduria");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_LblTotal");
            
            
            
            this.lbl_Mes.Text.ToUpper();

        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime mes, DateTime fechaIni, DateTime fechaFin, string filter)
        {
            this.lblReportName.Visible = false;

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ccAportesTotales> data = _moduloCartera.Report_Cc_Aportes(mes, fechaIni, fechaFin, filter);
            this.lbl_Mes.Text.ToUpper();
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
