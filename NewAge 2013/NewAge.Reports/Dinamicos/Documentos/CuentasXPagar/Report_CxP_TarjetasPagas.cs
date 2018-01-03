using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_CxP_TarjetasPagas : ReportBase
    {
         public Report_CxP_TarjetasPagas()
        {
            
        }

         public Report_CxP_TarjetasPagas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, int numDoc, ExportFormatType formatType)
             : base(loggerConn, c, tx, empresa, userId, formatType, numDoc)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35062_TarjetasPago");
            this.xrLblResponsable.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35062_Responsable");
            this.xrLblFechaPago.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35062_FechaPago");
            this.xrLblNumTarjeta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35062_NumTarjeta");
            this.xrlLblTotalBodega.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35062_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numDoc)
        {
            ModuloCuentasXPagar _moduloCuentaXPagar = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportBaseCXP> data = _moduloCuentaXPagar.ReportesCuentasXPagar_TarjetasPago(numDoc);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
