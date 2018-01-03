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
    public partial class Report_Cc_Pagare : ReportBase
    {

        #region Variable
        // variable para convertir en letras los valores
        private string mdaLocal = string.Empty;
        //Variable de error
        string Libranzavalidation = string.Empty;
        #endregion

        public Report_Cc_Pagare(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int mesIni, int mesFin,int año, string libranza, string Credito)
        {
            // Convierte Valor en Letras
            this.mdaLocal = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            // No muestra la descripciomn de reporte Padre
            this.lblNombreEmpresa.Visible = false;
            this.lblReportName.Visible = false;
            this.imgLogoEmpresa.Visible = false;
            this.lblUser.Visible = false;
            this.lblUserName.Visible = false;
            this.lblFecha.Visible = false;
            this.xrPageInfo1.Visible = false;
            this.xrPageInfo2.Visible = false;
            this.lblPage.Visible = false;

            // Parametros para trabar sobre el Query
            this.DataSourcePagare.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourcePagare.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(mesIni.ToString()) ? mesIni.ToString() : null;
            this.DataSourcePagare.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(mesFin.ToString()) ? mesFin.ToString() : null;
            this.DataSourcePagare.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(año.ToString()) ? año.ToString() : null;
            this.DataSourcePagare.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(libranza.ToString()) ? libranza.ToString() : null;

            base.ConfigureConnection(this.DataSourcePagare);

                this.CreateReport();
                return base.ReportName;
        }

        #region Eventos
        /// <summary>
        /// Impresion valor en letra, se colocan 2 label para que el valor se encapsule y se lea en el siguiente label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbVlrLibranza_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel Libranza = FindControl("lbVlrLibranza", true) as XRLabel;
            this.lbLibranzaLetra.Visible = false;
            XRLabel Libranza2 = FindControl("lbVlrLibranza", true) as XRLabel;

            if (!string.IsNullOrEmpty(Libranza.Text))
            {
                decimal Lib = Convert.ToDecimal(Libranza.Text);
                decimal Lib2 = Convert.ToDecimal(Libranza2.Text);

                this.lbLibranzaLetra.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, Lib);
                this.lbLibranzaLetra2.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, Lib2);
            }
        }
        #endregion
    }
}
