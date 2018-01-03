using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Pl_EjePresupuestalLineaXCentroCtoML : ReportBaseLandScape
    {

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_Pl_EjePresupuestalLineaXCentroCtoML()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor Con la cadena de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Pl_EjePresupuestalLineaXCentroCtoML(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35030_EjecucionPresupuestal");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Año");
            this.lblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Mes");
            this.lblMoneda.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Moneda");
            this.lblTotalGrupo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_TotalGrupo");
            this.lblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Total");
            this.lblTipoProyecto.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35030_TipoProyecto");
        }

        /// <summary>
        /// Inicializa los componentes
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime Periodo, string ProyectoTipo, string TipoReporte, string ProyectoID, string ActividadID, string LineaPresupuestalID,
            string CentroCostoID, string RecursoGrupoID)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloPlaneacion _moduloPlaneacion = new ModuloPlaneacion(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_PlaneacionTotal> data = _moduloPlaneacion.ReportesPlaneacion_EjecucionPresupuestalxMoneda(Periodo, ProyectoTipo, TipoReporte, ProyectoID, ActividadID,
                    LineaPresupuestalID, CentroCostoID, RecursoGrupoID);

                if (data.Count != 0)
                {
                    this.lblYear.Text = Periodo.Year.ToString();
                    this.lblMonth.Text = Periodo.Month.ToString();

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
