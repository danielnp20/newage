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
    public partial class Report_Cc_DetalladoCarteraVendida : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_DetalladoCarteraVendida()
        {

        }

        public Report_Cc_DetalladoCarteraVendida(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_lblDetalleCarteraVendida");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_FechaCorte");
            this.xrLblOferta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_Oferta");
            this.xrLblComprador.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_Comprador");
            //this.xrLblFactor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_Factor");
            this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_LblSubTotalO");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35097_LblTotalO");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida)
        {
            // le asigna a fechaFin la fecha con el ultimo dia del mes de fechaIni, solamente si fechaFin=fechaIni
            #region Validacion
            if (fechaFin == fechaIni)
            {
                fechaFin = new DateTime(fechaIni.Year, fechaIni.Month, DateTime.DaysInMonth(fechaIni.Year, fechaIni.Month));
            }
            #endregion
            // pinta la fecha de Corte del reporte en estilo DD/MM/YYYY teniendo en cuenta si el fin de mes es 31, 30 o 28
            if (fechaFin.Month.ToString().Length == 1 && fechaFin.Day.ToString().Length == 1)
            {
                this.xrLabel1.Text = "0" + fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
            }
            else
            {
                if (fechaFin.Month.ToString().Length == 1)
                {
                    if (fechaFin.Day == 30)
                    {
                        this.xrLabel1.Text = fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day==31) 
                    {
                        this.xrLabel1.Text = fechaFin.Day-1 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day==28)
                    {
                        this.xrLabel1.Text = fechaFin.Day + 2 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                }
                if (fechaFin.Day.ToString().Length == 1)
                {
                    this.xrLabel1.Text = "0" + fechaFin.Day + "/" + fechaFin.Month + "/" + fechaFin.Year;
                }
            }
            this.xrTableCell8.CanGrow = false;
            this.xrTableCell8.CanShrink = false;
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_VentaCartera(fechaIni, fechaFin, comprador, oferta, libranza, isResumida);
            //this.lblReportName.Visible = false;
            this.xrTableCell21.Visible = true;
            this.xrTableCell34.Visible = true;
            this.xrTableCell49.Visible = true;
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
