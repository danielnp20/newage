using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Ac_SaldosME : ReportBase
    {
         public Report_Ac_SaldosME()
        {

        }

         public Report_Ac_SaldosME(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Saldos");
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Año");
            this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Mes");
            this.xrLblClase.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Clase");
            this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_SubTotal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Total");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Libro");
            this.xrLblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Moneda");
            this.xrLblExtranjera.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Extranjera");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, bool isMonedaLoc)
        {

            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloActivosFijos _moduloActivosFijos = new ModuloActivosFijos(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ActivosTotales> data = _moduloActivosFijos.ReportesActivos_Saldos(libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos, propietario, isMonedaLoc);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                }
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
