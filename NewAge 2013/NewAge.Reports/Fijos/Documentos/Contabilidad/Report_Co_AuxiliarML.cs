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
    public partial class Report_Co_AuxiliarML : ReportBaseLandScape
    {

        #region Variables

        int count = 0;
        //int countAux = 0;
        decimal vlrInicial = 0;
        decimal deb = 0;
        decimal cre = 0;
        List<decimal> totales = new List<decimal>();
        decimal valorTotal = 0;

        //Variable para registros
        int registros=0;
        #endregion

        public Report_Co_AuxiliarML()
        {

        }

        public Report_Co_AuxiliarML(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Año");
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Desde:");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Libro");
            this.xrLblCuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Cuenta");
            this.xrLblNroRegistros.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Nroregistros");
            this.xrLblSaldoInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoInicialTercero");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvoCuenta");
            this.xrLblSaldoFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoFinalCuenta");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.lblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Hasta:");
            
        }
        
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin, string tercero, string proyecto,
             string centroCosto, string lineaPresupuestal)
        {
            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_Auxiliar(fechaInicial,fechaFinal, libro, cuentaInicial, cuentaFin, tercero, proyecto,
                centroCosto, lineaPresupuestal);


            
            if (data.Count == 0)
            {
                DTO_ReportLibroDiarioTotales rep = new DTO_ReportLibroDiarioTotales();
                rep.Detalles = new List<DTO_ReportLibroDiario>();

                DTO_ReportLibroDiario detalle = new DTO_ReportLibroDiario();
                detalle.BalanceTipoID.Value = libro;
                detalle.PeriodoID.Value = fechaInicial;
                rep.Detalles.Add(detalle);

                data.Add(rep);
            }
            
            data[0].FechaInicial = fechaInicial;
            data[0].FechaFinal = fechaFinal;

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

        #region Funciones Privadas

        /// <summary>
        /// Funcion que carga el Valor inicial y oculta el campo si el valor es Mayor a cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InicalDebito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(XrInicalDebito.Text))
                {
                    vlrInicial = Convert.ToDecimal(XrInicalDebito.Text, CultureInfo.CurrentCulture);
                    if (!string.IsNullOrEmpty(XrInicalDebito.Text))
                    {
                        if (Convert.ToDecimal(XrInicalDebito.Text, CultureInfo.CurrentCulture) >= 0)
                            this.XrInicalDebito.Visible = true;
                        else
                            this.XrInicalDebito.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {;}
        }

        /// <summary>
        /// Funcion que se encarga de Monstrar el control si el valor es menor q cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrInicalCredito_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(xrInicalCredito.Text))
                {
                    vlrInicial = Convert.ToDecimal(xrInicalCredito.Text, CultureInfo.CurrentCulture);
                    if (Convert.ToDecimal(xrInicalCredito.Text, CultureInfo.CurrentCulture) < 0)
                    {
                        valorTotal = Math.Abs(Convert.ToDecimal(xrInicalCredito.Text, CultureInfo.CurrentCulture));
                        xrInicalCredito.Text = valorTotal.ToString("n2");

                        this.xrInicalCredito.Visible = true;
                    }
                    else
                        this.xrInicalCredito.Visible = false;
                }
            }
            catch (Exception ex)
            {; }
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
                this.deb = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
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
                this.cre = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
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

        /// <summary>
        /// Funcion que se encarga de de verificar si hay tercero para pintar la columna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrTable4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cellTercero.Text))
                e.Cancel = true;
        }

        /// <summary>
        /// Funcion que se encarga de de verificar si hay tercero para pintar la columna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrTable5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cellTercero.Text))
                e.Cancel = true;
        }

        /// <summary>
        /// Cuanta la cantidad de registros para quitar el registro que genera los saldos iniciales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCantidadReg_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblCantidadReg.Text))
            {
                lblCantidadReg.Text = (Convert.ToInt16(lblCantidadReg.Text) - 1).ToString(); ;

            }
        }

        #endregion
    }
}
