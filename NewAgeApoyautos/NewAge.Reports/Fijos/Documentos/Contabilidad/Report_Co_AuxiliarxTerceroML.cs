using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using System.Globalization;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_AuxiliarxTerceroML : ReportBaseLandScape
    {
        #region Varibles

        int count = 0;
        decimal vlrInicial = 0;
        decimal deb = 0;
        decimal cre = 0;
        List<decimal> totales = new List<decimal>();
        decimal valorTotal = 0;

        #endregion

        public Report_Co_AuxiliarxTerceroML()
        {

        }

        public Report_Co_AuxiliarxTerceroML(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Desde:");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Hasta:");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Libro");
            this.xrLblCuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Cuenta");
            this.xrLblNroRegistros.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Nroregistros");
            this.xrLblSaldoInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoInicial");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvoTercero");
            //this.xrLblSaldoFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoFinalCuenta");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.lblTotalMtoTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvo");
            this.lblCuentaTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_CuentaTercero");
            this.xrLblSaldoInicialCuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoInicialCuenta");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin, string tercero, string proyecto, string centroCosto,
            string lineaPresupuestal)
        {

            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_AuxiliarxTercero( fechaInicial,  fechaFinal, libro, cuentaInicial, cuentaFin, tercero
                , proyecto, centroCosto, lineaPresupuestal);

            data[0].FechaInicial = fechaInicial;
            data[0].FechaFinal = fechaFinal;

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

        #region Funciones Privadas

        /// <summary>
        /// Verifica si el valor inicial del tercero es mayor a cero Para colocarlo en la casilla de Debito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblInicialDebito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblInicialDebito.Text))
            {
                vlrInicial = Convert.ToDecimal(lblInicialDebito.Text,CultureInfo.CurrentCulture);
                if (Convert.ToDecimal(lblInicialDebito.Text) >= 0)
                    this.lblInicialDebito.Visible = true;
                else
                    this.lblInicialDebito.Visible = false;
            }
        }

        /// <summary>
        /// Verifica si el Valor Inicial del tercero es menor a cero  Para colocarlo en la casilla de Credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblInicialCredito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblInicialCredito.Text))
            {
                vlrInicial = Convert.ToDecimal(lblInicialCredito.Text, CultureInfo.CurrentCulture);
                if (Convert.ToDecimal(lblInicialCredito.Text) < 0)
                {
                    valorTotal = Math.Abs(Convert.ToDecimal(lblInicialCredito.Text));

                    lblInicialCredito.Text = valorTotal.ToString("n2");
                    this.lblInicialCredito.Visible = true;
                }
                else
                    this.lblInicialCredito.Visible = false;
            }
        }

        /// <summary>
        /// Funcion que se encarga de Sumar el Debito por tercero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTotalDebitoTer_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.cre = 0;
                this.deb = Convert.ToDecimal(e.Value);
            }
        }

        /// <summary>
        /// Funcion q se encarga de sumar el Credito por tercero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalCreditoTer_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.cre = Convert.ToDecimal(e.Value);
                this.totales.Add(this.vlrInicial + this.deb - this.cre);
            }
        }

        /// <summary>
        /// Verifica si el total por tercero es Mayor a Cero para pintarlo en la casilla de Debitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTotalDebitoTercero_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totales[this.count];
                if (total >= 0)
                    this.lblTotalDebitoTercero.Text = Math.Abs(total).ToString("n2");
                else
                    this.lblTotalDebitoTercero.Visible = false;

                this.count++;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Verifica si el total por tercero es Menor a Cero para pintarlo en la casilla de Creditos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTotalCreditoTercero_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totales[this.count - 1];
                if (total < 0)
                    this.lblTotalCreditoTercero.Text = Math.Abs(total).ToString("n2");
                else
                    this.lblTotalCreditoTercero.Visible = false;

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Verifica si el saldo Inicial de la cuenta es mayor a Cero para pintarlo en la casilla de Debitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaldoInicialCuentaDeb_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(SaldoInicialCuentaDeb.Text))
            {
                vlrInicial = Convert.ToDecimal(SaldoInicialCuentaDeb.Text);
                if (Convert.ToDecimal(SaldoInicialCuentaDeb.Text) >= 0)
                    this.SaldoInicialCuentaDeb.Visible = true;
                else
                    this.SaldoInicialCuentaDeb.Visible = false;
            }
        }

        /// <summary>
        /// Verifica si el saldo inicial de la cuenta es menor a Cero para pintarlo en la casilla de Creditos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaldoIncialCuentaCre_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(SaldoIncialCuentaCre.Text))
            {
                vlrInicial = Convert.ToDecimal(SaldoIncialCuentaCre.Text);
                if (Convert.ToDecimal(SaldoIncialCuentaCre.Text) < 0)
                {
                    valorTotal = Math.Abs(Convert.ToDecimal(SaldoIncialCuentaCre.Text));

                    SaldoIncialCuentaCre.Text = valorTotal.ToString("n2");
                    this.SaldoIncialCuentaCre.Visible = true;
                }
                else
                    this.SaldoIncialCuentaCre.Visible = false;
            }
        }

        /// <summary>
        /// Funcion que se encarga de de verificar si hay tercero para pintar la columna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrTable4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cellCenroCosto.Text))
                e.Cancel = true;
        }

        /// <summary>
        /// Funcion que se encarga de de verificar si hay tercero para pintar la columna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrTable5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cellCenroCosto.Text))
                e.Cancel = true;
        }

        /// <summary>
        /// Cuanta la cantidad de registros para quitar el registro que genera los saldos iniciales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCantidadRegistros_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCantidadRegistros.Text))
            {
                lblCantidadRegistros.Text = (Convert.ToInt16(lblCantidadRegistros.Text) - 1).ToString();

            }
        }

        #endregion
    }
}
