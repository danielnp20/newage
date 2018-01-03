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
    public partial class Report_Co_ComprobanteControl : ReportBase
    {
         public Report_Co_ComprobanteControl()
        {

        }

         public Report_Co_ComprobanteControl(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Mes");
            this.xrLblTotalReg.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_TotalReg");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_GranTotal");
            //this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(int año, int mes, string comprobanteID)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportComprobanteControlTotal> data = _moduloContabilidad.ReportesContabilidad_ComprobateControl(año, mes, comprobanteID);

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
