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
    public partial class Report_Cc_VentaCartera : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_VentaCartera()
        {

        }

        public Report_Cc_VentaCartera(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, DateTime mes, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblVentaCartera");
            this.xrLblOferta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblOferta");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblFecha");
            this.xrLblFactor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblFactor");
            this.xrLblValorVenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblValorVenta");
            this.xrLblValorFlujo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblValorflujo");
            this.xrLblSaldoFlujo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblSaldoFlujo");
            this.xrLblTotalLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblTotalLibranza");
            this.xrLblCreditoPendiente.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblCreditoPendiente");
            this.xrLblCreditoMora.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblCreditoMora");
            this.xrLblCreditoPrePag.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblCreditoPrePag");
            this.xrLblCreditoReco.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_lblCreditoReco");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35094_LblTotal");
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
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
