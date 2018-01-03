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
    public partial class Report_Cc_CarteraMora : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_CarteraMora()
        {

        }

        public Report_Cc_CarteraMora(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_lblCarteraEnMora");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_LblFecha");
            this.xrLblClase.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_Clase");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_LblSubTotal");
            this.xrLblTotalG.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_LblTotalG");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, DateTime fechaIni, DateTime fechaFin, string comprador, string oferta, string libranza, bool isResumida, string orden, ExportFormatType formatType)
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
                this.xrLabel2.Text = "0" + fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
            }
            else
            {
                if (fechaFin.Month.ToString().Length == 1)
                {
                    if (fechaFin.Day == 30)
                    {
                        this.xrLabel2.Text = fechaFin.Day + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day==31) 
                    {
                        this.xrLabel2.Text = fechaFin.Day - 1 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                    else if (fechaFin.Day==28)
                    {
                        this.xrLabel2.Text = fechaFin.Day + 2 + "/0" + fechaFin.Month + "/" + fechaFin.Year;
                    }
                }
                if (fechaFin.Day.ToString().Length == 1)
                {
                    this.xrLabel2.Text = "0" + fechaFin.Day + "/" + fechaFin.Month + "/" + fechaFin.Year;
                }
            }
            

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> totales = _moduloCartera.Report_Cc_CarteraMora(periodo,fechaIni, fechaFin, comprador, oferta ,libranza,isResumida,orden);
            //this.xrLabel2.Text = Convert.ToString(periodo);

            this.DataSource = totales;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
