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
    public partial class Report_Cp_FacturasXPagar : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_FacturasXPagar()
        {

        }

        public Report_Cp_FacturasXPagar(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_Afiliaciones");
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
        public string GenerateReport(DateTime fecha, string Tercero, int Moneda, string Cuenta, bool isMultimoneda)
        {
            this.lblReportName.Visible = false;

            ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportCxpFacturasXPagarTotales> data = _moduloCxP.Report_FacturasXPagar(Tercero, Moneda, Cuenta, fecha, isMultimoneda);
            for (int i = 0; i < data.Count; i++)
                data[i].FechaIni = fecha;

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
