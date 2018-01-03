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
    public partial class Report_Cc_EstadoDeCuenta : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_Cc_EstadoDeCuenta()
        {

        }

        public Report_Cc_EstadoDeCuenta(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_EstadoDeCuenta");
            this.lbl_NombreReporte.Text.ToUpper();
            this.lbl_FechaIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_FechaIni");
            this.lbl_FechaFin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_FechaFin");
            //this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            //this.lbl_Subtotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            this.lbl_SaldoIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_SaldoIni");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin,string _tercero, string filter)
        {
            this.lblReportName.Visible = false;

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ccEstadoDeCuentaTotales> data = _moduloCartera.Report_Cc_EstadoDeCuenta(fechaIni, fechaFin,_tercero, filter);
            
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
