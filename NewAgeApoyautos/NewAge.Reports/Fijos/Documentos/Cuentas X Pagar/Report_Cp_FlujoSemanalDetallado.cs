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

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_Cp_FlujoSemanalDetallado : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_FlujoSemanalDetallado()
        {

        }

        public Report_Cp_FlujoSemanalDetallado(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_CuentasxPagarxFlujoSemanalDetallado");
            this.lbl_FechaIncial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_FechaInicial:");
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_SubTotal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_Total:");
            this.xrLblMdaOrigen.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35081_MdaOrig");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(List<DateTime> fechaIni, int Moneda, string Tercero, bool isDetallado)
        {
            #region Variables
            // dia/mes/año
            if (fechaIni.Count == 6)
            {
                this.xrTableCell8.Text = "";
                this.xrTableCell9.ForeColor = Color.Transparent;
                this.xrTableCell19.ForeColor = Color.Transparent;
                this.xrTableCell26.ForeColor = Color.Transparent;
            }
            if (fechaIni.Count == 6 && fechaIni[4].Equals(fechaIni[5]))
            {
                this.Cell5.Text = "";
                this.xrTableCell2.ForeColor= Color.Transparent;
                this.xrTableCell16.ForeColor = Color.Transparent;
                this.xrTableCell25.ForeColor = Color.Transparent;
            }
            
            this.Cell1.Text = this.Cell1.Text + "         " + fechaIni[0].ToString().Substring(0, 2) + " - " + fechaIni[1].ToString().Substring(0, 2);
            this.Cell2.Text = this.Cell2.Text + "         " + fechaIni[1].AddDays(1).ToString().Substring(0, 2) + " - " + fechaIni[2].ToString().Substring(0, 2);
            this.Cell3.Text = this.Cell3.Text + "         " + fechaIni[2].AddDays(1).ToString().Substring(0, 2) + " - " + fechaIni[3].ToString().Substring(0, 2);
            this.Cell4.Text = this.Cell4.Text + "         " + fechaIni[3].AddDays(1).ToString().Substring(0, 2) + " - " + fechaIni[4].ToString().Substring(0, 2);
            if (fechaIni.Count > 5 && !fechaIni[4].Equals(fechaIni[5]))
                this.Cell5.Text = this.Cell5.Text + "         " + fechaIni[4].AddDays(1).ToString().Substring(0, 2) + " - " + fechaIni[5].ToString().Substring(0, 2);
            if(fechaIni.Count==7)
            {
            if (!fechaIni[5].Equals(fechaIni[6]))
                this.xrTableCell8.Text = this.xrTableCell8.Text + "         " + fechaIni[5].AddDays(1).ToString().Substring(0, 2) + " - " + fechaIni[6].ToString().Substring(0, 2);
            else
                this.xrTableCell8.Text = this.xrTableCell8.Text + "         " + fechaIni[5].ToString().Substring(0, 2) + " - " + fechaIni[6].ToString().Substring(0, 2);
            }
            this.xrLabel1.Text = fechaIni[0].ToString().Substring(0,10);
            ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            #endregion

            List<DTO_ReportCxPTotales> data = _moduloCxP.ReportesCuentasXPagar_FlujoSemanalDetallado(fechaIni, Moneda, Tercero, isDetallado);
            
            
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
