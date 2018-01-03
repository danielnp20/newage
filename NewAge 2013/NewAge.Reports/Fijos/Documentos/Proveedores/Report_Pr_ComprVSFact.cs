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
    public partial class Report_Pr_ComprVSFact : ReportBase
    {
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public Report_Pr_ComprVSFact()
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
        public Report_Pr_ComprVSFact(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35088_EstadoSolicitudes");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Año");
            this.lblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Desde");
            this.lblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Hasta");
            this.lblTotalGrupo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_TotalGrupo");
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
        public DTO_TxResult GenerateReport(DateTime FechaInicial, DateTime FechaFinal, string proveedor)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloProveedores _moduloProveedores = new ModuloProveedores(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ProveedoresTotal> data = _moduloProveedores.ReportesProveedores_CompromisosVSFacturas(FechaInicial, FechaFinal, proveedor);
                if (data.Count != 0)
                {
                    this.xrLblAño.Text = FechaInicial.Year.ToString("yyyy");
                    this.xrLblDesde.Text = FechaInicial.Month.ToString("MMMM");
                    this.xrLblHasta.Text = FechaFinal.Month.ToString("MMMM");

                    this.DataSource = data;
                    this.ShowPreview();
                    //this.CreateReport();
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
