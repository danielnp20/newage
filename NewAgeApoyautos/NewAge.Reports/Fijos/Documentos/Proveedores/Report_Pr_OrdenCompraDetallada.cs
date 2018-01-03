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
    public partial class Report_Pr_OrdenCompraDetallada : ReportBase
    {
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public Report_Pr_OrdenCompraDetallada()
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
        public Report_Pr_OrdenCompraDetallada(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_OrdenCompraResumida");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Fecha");
            this.lblElaborado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_Elaborado");
            //this.lblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Hasta");
            this.lblProveedor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Proveedor");
            this.lblNroOrden.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35092_NroOrden");
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
        public DTO_TxResult GenerateReport(DateTime FechaIni, DateTime FechaFin, string Proveedor, int Estado, bool isDetallado, string Moneda)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloProveedores _moduloProveedores = new ModuloProveedores(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ProveedoresTotal> data = _moduloProveedores.ReportesProveedores_OrdenCompraArchivo(FechaIni, FechaFin, Proveedor, Estado, isDetallado, Moneda);
                if (data.Count != 0)
                {
                    data[0].PeriodoInicial = FechaIni;
                    data[0].PeriodoFinal = FechaFin;

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
