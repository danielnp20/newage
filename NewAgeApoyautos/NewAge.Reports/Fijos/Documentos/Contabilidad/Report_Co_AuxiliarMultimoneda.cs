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

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_AuxiliarMultimoneda : ReportBaseLandScape
    {
        #region Variables

        int count = 0;
        int countE = 0;
        decimal vlrInicial = 0;
        decimal vlrInicialE = 0;
        decimal deb = 0;
        decimal debE = 0;
        decimal cre = 0;
        decimal creE = 0;
        List<decimal> totales = new List<decimal>();
        List<decimal> totalesE = new List<decimal>();
        decimal valorTotal = 0;
        decimal valorTotalE = 0;

        #endregion
        public Report_Co_AuxiliarMultimoneda()
        {

        }

        public Report_Co_AuxiliarMultimoneda(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Mes");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Libro");
            this.xrLblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Cuenta");
            this.xrLblNroRegistros.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Nroregistros");
            this.xrLblSaldoInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoInicial");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvoCuenta");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_GranTotal");
            //this.xrLblSaldoFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoFinalCuenta");
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
            //this.lblReportName.Visible = false;
            this.xrTableCell27.CanGrow = false;
            this.xrTableCell27.CanShrink = false;
            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_Auxiliar(fechaInicial,fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto,
                centroCosto, lineaPresupuestal);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

        /// <summary>
        /// Funcion que carga el Valor inicial y oculta el campo si el valor es Mayor a cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InicalDebitoE_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(XrInicalDebitoE.Text))
            {
                this.vlrInicialE = Convert.ToDecimal(XrInicalDebitoE.Text);
                if (!string.IsNullOrEmpty(XrInicalDebitoE.Text))
                {
                    if (Convert.ToDecimal(XrInicalDebitoE.Text) >= 0)
                        this.XrInicalDebitoE.Visible = true;
                    else
                        this.XrInicalDebitoE.Visible = false;
                }
            }
        }

        /// <summary>
        /// Funcion que se encarga de Monstrar el control si el valor es menor q cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrInicalCreditoE_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(xrInicalCreditoE.Text))
            {
                this.vlrInicialE = Convert.ToDecimal(xrInicalCreditoE.Text);
                if (Convert.ToDecimal(xrInicalCreditoE.Text) < 0)
                {
                    this.valorTotalE = Math.Abs(Convert.ToDecimal(xrInicalCreditoE.Text));
                    xrInicalCreditoE.Text = this.valorTotalE.ToString("n2");

                    this.xrInicalCreditoE.Visible = true;
                }
                else
                    this.xrInicalCreditoE.Visible = false;
            }
        }

        /// <summary>
        /// Funcion q se encarga de verificar el valor del campo Debito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void totalDebitoE_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.creE = 0;
                this.debE = Convert.ToDecimal(e.Value);
            }
        }

        /// <summary>
        /// Funcion q se encarga de verificar el campo Credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalCreditoE_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.creE = Convert.ToDecimal(e.Value);
                this.totalesE.Add(this.vlrInicialE + this.debE - this.creE);
            }
        }

        /// <summary>
        /// Muestra en el u oculta el control; si el Valor es mayor a cero muestra el control, si es menor q cero oculta el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalPorTerceroE_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totalesE[this.countE];
                if (total >= 0)
                    this.TotalPorTerceroE.Text = Math.Abs(total).ToString("n2");
                else
                    this.TotalPorTerceroE.Visible = false;

                this.countE++;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        ///  Muestra en el u oculta el control; si el Valor es mayor a cero muestra el control, si es menor q cero oculta el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalTerceroCreditoE_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totalesE[this.countE - 1];
                if (total < 0)
                    this.TotalTerceroCreditoE.Text = Math.Abs(total).ToString("n2");
                else
                    this.TotalTerceroCreditoE.Visible = false;

            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Funcion que carga el Valor inicial y oculta el campo si el valor es Mayor a cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InicalDebito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(XrInicalDebito.Text))
            {
                this.vlrInicial = Convert.ToDecimal(XrInicalDebito.Text);
                if (!string.IsNullOrEmpty(XrInicalDebito.Text))
                {
                    if (Convert.ToDecimal(XrInicalDebito.Text) >= 0)
                        this.XrInicalDebito.Visible = true;
                    else
                        this.XrInicalDebito.Visible = false;
                }
            }
        }

        /// <summary>
        /// Funcion que se encarga de Monstrar el control si el valor es menor q cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrInicalCredito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (!string.IsNullOrEmpty(xrInicalCredito.Text))
            {
                vlrInicial = Convert.ToDecimal(xrInicalCredito.Text);
                if (Convert.ToDecimal(xrInicalCredito.Text) < 0)
                {
                    valorTotal = Math.Abs(Convert.ToDecimal(xrInicalCredito.Text));
                    xrInicalCredito.Text = valorTotal.ToString("n2");

                    this.xrInicalCredito.Visible = true;
                }
                else
                    this.xrInicalCredito.Visible = false;
            }
        }

        /// <summary>
        /// Funcion q se encarga de verificar el valor del campo Debito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void totalDebito_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.cre = 0;
                this.deb = Convert.ToDecimal(e.Value);
            }
        }

        /// <summary>
        /// Funcion q se encarga de verificar el campo Credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalCredito_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                this.cre = Convert.ToDecimal(e.Value);
                this.totales.Add(this.vlrInicial + this.deb - this.cre);
            }
        }

        /// <summary>
        /// Muestra en el u oculta el control; si el Valor es mayor a cero muestra el control, si es menor q cero oculta el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalPorTercero_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totales[this.count];
                if (total >= 0)
                    this.TotalPorTercero.Text = Math.Abs(total).ToString("n2");
                else
                    this.TotalPorTercero.Visible = false;

                this.count++;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        ///  Muestra en el u oculta el control; si el Valor es mayor a cero muestra el control, si es menor q cero oculta el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalTerceroCredito_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            try
            {
                decimal total = this.totales[this.count - 1];
                if (total < 0)
                    this.TotalTerceroCredito.Text = Math.Abs(total).ToString("n2");
                else
                    this.TotalTerceroCredito.Visible = false;

            }
            catch (Exception ex)
            {

            }
        }

    }
}
