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

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_AportesCliente : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_Cc_AportesCliente()
        {

        }

        public Report_Cc_AportesCliente(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_AportesCliente");
            this.lbl_NombreReporte.Text.ToUpper();
            this.lbl_Periodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_Periodo");//
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");//
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string filter)
        {
            this.lblReportName.Visible = false;
            List<DTO_ccAportesClienteTotales> totales = new List<DTO_ccAportesClienteTotales>();
            DTO_ccAportesClienteTotales total = new DTO_ccAportesClienteTotales();

            total.Detalles = new List<DTO_ccAportesCliente>();
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            total.Detalles = _moduloCartera.Report_Cc_AportesCliente(periodo, filter);
            total.FechaIni = periodo;

            totales.Add(total);
            this.DataSource = totales;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
