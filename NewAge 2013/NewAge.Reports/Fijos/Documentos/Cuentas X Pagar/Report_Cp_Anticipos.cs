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
    public partial class Report_Cp_Anticipos : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_Anticipos()
        {

        }

        public Report_Cp_Anticipos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35074_lbl_Anticipos");
            this.xrLblPeriodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35074_Periodo");
            this.xrLblMdaOrigen.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35074_MdaOrigen");
            this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35074_SubTotal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35074_Total");
            //this.lbl_NombreReporte.Text.ToUpper();
            //this.lbl_Mes.Text.ToUpper();
            //this.lbl_Empresa2.Text = this.Empresa.Descriptivo.Value;
            //this.lbl_Empresa2.Text.ToUpper();
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime Fecha, int Moneda, string Tercero, bool isDetallado)
        {
            DateTime ultimoDia = Fecha.AddMonths(1);
            ultimoDia = ultimoDia.AddDays(-1);
            string fecha = ultimoDia.ToString().Substring(0, 10);
            this.xrLabel3.Text = fecha;
            Fecha = ultimoDia;
            ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportCxPTotales> data = _moduloCxP.ReportesCuentasXPagar_Anticipos(Fecha,Moneda,Tercero,isDetallado);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
