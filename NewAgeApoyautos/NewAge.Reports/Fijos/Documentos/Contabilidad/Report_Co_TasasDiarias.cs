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
using NewAge.DTO.Reportes;
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_TasasDiarias : ReportBaseLandScape
    {
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public Report_Co_TasasDiarias()
        {

        }

        /// <summary>
        /// Constructor con parametros de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Co_TasasDiarias(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35037_TasasDiarias");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Año");
            this.lblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Total");
        }

        /// <summary>
        /// Inicializa el reporte
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime Periodo, bool isDiaria)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloContabilidad _moduloProveedores = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ContabilidadTotal> data = _moduloProveedores.ReportesContabilidad_Tasas(Periodo, isDiaria);
                if (data[0].DetallesTasas.Count != 0)
                {
                    data[0].PeriodoInicial = Periodo;
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
