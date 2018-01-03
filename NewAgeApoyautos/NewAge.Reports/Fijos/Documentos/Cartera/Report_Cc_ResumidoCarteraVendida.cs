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
    public partial class Report_Cc_ResumidoCarteraVendida : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_ResumidoCarteraVendida()
        {

        }

        public Report_Cc_ResumidoCarteraVendida(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_lblResumidoCarteraVendida");
            this.xrLblComprador.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_Comprador");
            this.xrLblFechaCorte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_FechaCorte");
            this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_LblSubTotal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_LblTotal");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fechaIni">Mes inicial por el cual se va a filtrar</param>
        /// <param name="fechaFin">Mes final por el cual se va a filtrar</param>
        /// <param name="comprador">Comprador por el cual se desea filtrar</param>
        /// <param name="oferta">Oferta que se desea ver</param>
        /// <param name="libranza">Numero de Libranza por el cual se desea ver</param>
        /// <param name="isResumida">Filtra el reportes (True) para Resumido (False) para Detallado</param>
        /// <returns>URL del reporte</returns>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida)
        {
            //this.lblReportName.Visible = false;
            #region Validacion
            if (fechaFin == fechaIni)
            {
                fechaFin = new DateTime(fechaIni.Year, fechaIni.Month, DateTime.DaysInMonth(fechaIni.Year, fechaIni.Month));
            }
            #endregion
            if (fechaFin.Month.ToString().Length == 1 && fechaFin.Day.ToString().Length == 1)
            {
                this.xrLabel2.Text = "0" + fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
            }
            else 
            {
                if (fechaFin.Month.ToString().Length == 1)
                {
                    if (fechaFin.Day == 30)
                    {
                        this.xrLabel1.Text = fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day == 31)
                    {
                        this.xrLabel1.Text = fechaFin.Day - 1 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day == 28)
                    {
                        this.xrLabel1.Text = fechaFin.Day + 2 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                }
                if (fechaFin.Day.ToString().Length == 1)
                {
                    this.xrLabel2.Text = "0" + fechaFin.Day + "/" + fechaFin.Month + "/" + fechaFin.Year;
                }
            }
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_VentaCartera(fechaIni, fechaFin, comprador, oferta, libranza, isResumida);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
