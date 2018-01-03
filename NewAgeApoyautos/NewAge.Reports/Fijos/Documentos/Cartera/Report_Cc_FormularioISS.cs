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
    public partial class Report_Cc_FormularioISS : ReportBase
    {

        #region Variable
        // variable para convertir en letras los valores
        private string mdaLocal = string.Empty;
        #endregion

        public Report_Cc_FormularioISS()
        {

        }

        public Report_Cc_FormularioISS(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
            this.DataSourceFormatoISS.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceFormatoISS.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(mesIni.ToString()) ? mesIni.ToString() : null;
            this.DataSourceFormatoISS.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(mesFin.ToString()) ? mesFin.ToString() : null;
            this.DataSourceFormatoISS.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(año.ToString()) ? año.ToString() : null;
            this.DataSourceFormatoISS.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(libranza.ToString()) ? libranza.ToString() : null;

            base.ConfigureConnection(this.DataSourceFormatoISS);
            this.CreateReport();
            return this.ReportName;
        }

        // Impresion valor en letra, se colocan 2 label para que el valor se encapsule y se lea en el siguiente label.
        private void lbVlrAfiliacion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel Afiliacion  = FindControl("lbVlrAfiliacion", true) as XRLabel;
            this.lbLetraAfiliacion.Visible = false;
            XRLabel Afiliacion2 = FindControl("lbVlrAfiliacion", true) as XRLabel;

            if (!string.IsNullOrEmpty(Afiliacion.Text))
            {
                decimal Afi = Convert.ToDecimal(Afiliacion.Text);
                decimal Afi2 = Convert.ToDecimal(Afiliacion2.Text);

                this.lbLetraAfiliacion.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, Afi);
                this.lbLetraAfiliciacion2.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, Afi2);
            }
        }

        // Impresion valor en letra, se colocan 2 label para que el valor se encapsule y se lea en el siguiente label.
        private void lbCoutaFija_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel cuotafija = FindControl("lbCoutaFija", true) as XRLabel;
            this.lbLetraCuota.Visible = false;
            XRLabel cuotafija2 = FindControl("lbCoutaFija", true) as XRLabel;

            if (!string.IsNullOrEmpty(cuotafija.Text))
            {
                decimal cuo = Convert.ToDecimal(cuotafija.Text);
                decimal cuo2 = Convert.ToDecimal(cuotafija2.Text);

                this.lbLetraCuota.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, cuo);
                this.lbLetraCuota2.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, cuo2);
            }
        }

        // Impresion valor en letra, se colocan 2 label para que el valor se encapsule y se lea en el siguiente label.
        private void lbVlrLibranza_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel Vlrlibranza = FindControl("lbVlrLibranza", true) as XRLabel;
            XRLabel VlrLibranza2 = FindControl("lbVlrLibranza", true) as XRLabel;

            if (!string.IsNullOrEmpty(Vlrlibranza.Text))
            {
                decimal vl = Convert.ToDecimal(Vlrlibranza.Text);
                decimal vl2 = Convert.ToDecimal(VlrLibranza2.Text);

                this.lbVlrLibranzaLetra.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, vl);
                this.lbVlrLibranzaLetra2.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, vl2);

                this.lbVlrLibranzaLetra.Visible = false;
            }
        }
    }
}
