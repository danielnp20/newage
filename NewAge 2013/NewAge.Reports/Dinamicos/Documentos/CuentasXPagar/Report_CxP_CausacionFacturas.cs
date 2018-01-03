using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_CxP_CausacionFacturas : ReportBaseLandScape
    {
        #region Variable del formulario
        //Variables para convertir a positivos los valores negativos
        decimal retefuente = 0;
        decimal reteIva = 0;
        decimal reteIca = 0;
        decimal anticipo = 0;

        //Variables para calculto total
        decimal vlrBruto = 0;
        decimal iva = 0;
        decimal retIva = 0;
        decimal retFuente = 0;
        decimal retIca = 0;
        decimal antici = 0;
        decimal total = 0;
        #endregion

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_CxP_CausacionFacturas() { }

        public Report_CxP_CausacionFacturas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numeroDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numeroDoc)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_CausacionFac");
            this.lblAFavorDe.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_AFavorDe");
            this.lblFacturaNro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_FacturaNro");
            this.lblFechaFactura.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_FechaFactura");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Fecha");
            this.lblDescripcion.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Descripcion");
            this.lblFechaPago.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_FechaPago");
            this.lblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Comprobante");
            this.lblTRM.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_TRM");
            this.lblNit.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Nit");
            this.lblSolicita.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Solicita");
            this.lblAprueba.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "21_Aprueba");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc, bool isAprobada,bool isNotaCredito)
        {
            try
            {
                if (isNotaCredito)
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Menu, "mnu_cp_NotaCredito");
                

                #region Verifica si la causacion esta en estado aprobada  o Preaprobar
                //(Si es para aprobadar colocar el preliminar, si es aprobada le quita el Preliminar)
                if (!isAprobada)
                {
                    this.Watermark.Font = new System.Drawing.Font("Arial", 144F);
                    this.Watermark.ForeColor = System.Drawing.Color.Gainsboro;
                    this.Watermark.Text = "PRELIMINAR";
                    this.Watermark.TextTransparency = 119;
                }
                #endregion

                ModuloCuentasXPagar _moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> data = _moduloCxP.Reportes_Cp_CausacionFacturas(numeroDoc, isAprobada);
                this.DataSource = data;

                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Caculcula el valor Bruto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrLabel16_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(xrLabel16.Text))
                vlrBruto = Convert.ToDecimal(xrLabel16.Text);
        }

        /// <summary>
        /// Calcula el valor del iva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VlrIva_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                this.iva = Convert.ToDecimal(e.Value);
        }

        /// <summary>
        /// Calcula el Valor del Rete iva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ivaRete_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                this.retIva = Convert.ToDecimal(e.Value);
        }

        /// <summary>
        /// Calcula el Valor del ReteFuente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fuenteRete_SummaryCalculated_1(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                this.retFuente = Convert.ToDecimal(e.Value);
        }

        /// <summary>
        /// Calcula el Valor del ReteIca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icaRete_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                this.retIca = Convert.ToDecimal(e.Value);
        }

        /// <summary>
        /// Calcula el valor del anticipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void anti_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                this.antici = Convert.ToDecimal(e.Value);
        }

        /// <summary>
        /// Varifica si el valor es menor que cero para pintarlo positivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ivaRete_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(ivaRete.Text))
            {
                if (Convert.ToDecimal(ivaRete.Text) < 0)
                {
                    reteIva = Math.Abs(Convert.ToDecimal(ivaRete.Text));
                    this.ivaRete.Text = reteIva.ToString("n2");
                }
            }
        }

        /// <summary>
        /// Varifica si el valor es menor que cero para pintarlo positivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fuenteRete_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(fuenteRete.Text))
            {
                if (Convert.ToDecimal(fuenteRete.Text) < 0)
                {
                    retefuente = Math.Abs(Convert.ToDecimal(fuenteRete.Text));
                    this.fuenteRete.Text = retefuente.ToString("n2");
                }
            }
        }

        /// <summary>
        /// Varifica si el valor es menor que cero para pintarlo positivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icaRete_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(icaRete.Text))
            {
                if (Convert.ToDecimal(icaRete.Text) < 0)
                {
                    reteIca = Math.Abs(Convert.ToDecimal(icaRete.Text));
                    this.icaRete.Text = reteIca.ToString("n2");
                }
            }
        }

        /// <summary>
        /// Varifica si el valor es menor que cero para pintarlo positivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void anti_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!string.IsNullOrEmpty(anti.Text))
            {
                if (Convert.ToDecimal(anti.Text) < 0)
                {
                    anticipo = Math.Abs(Convert.ToDecimal(anti.Text));
                    this.anti.Text = anticipo.ToString("n2");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Total_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            this.Total.Text = (vlrBruto + iva + retIva + retFuente + retIca + antici).ToString("n2");
        }
        #endregion




























    }
}
