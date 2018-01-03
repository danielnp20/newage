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
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_ComprobanteME : ReportBaseLandScape
    {
        public Report_Co_ComprobanteME()
        {

        }

        public Report_Co_ComprobanteME(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Mes");
            this.xrLblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobante");
            this.xrLblNro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Nro");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Fecha");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Total");
            this.xrLblUsuario.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Usuario");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Libro");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");
            //this.xrLblTotalComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_TotalComprobante"); 
            //this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");     
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal, bool porHoja)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                this.xrTableCell28.CanGrow = false;
                this.xrTableCell28.CanShrink = false;

                //Verfica si el usuario quiere ver el comprobate por hoja
                if (porHoja)
                    this.DetailReport.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;

                this.ReportFooter.Visible = true;
                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportComprobanteTotal> data = _moduloContabilidad.ReportesContabilidad_Comprobantes(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }
                else
                    result.Result = ResultValue.NOK;

                return result;
            }
            catch (Exception)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                return result;
            }
        }

    }
}
