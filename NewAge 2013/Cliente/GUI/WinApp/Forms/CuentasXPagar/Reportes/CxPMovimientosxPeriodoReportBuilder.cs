using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using System.Collections;
using SentenceTransformer;
using System.Reflection;
using NewAge.Librerias.Project;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class CxPMovimientosxPeriodoReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Cuentas por pagar Reports (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign</param>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="Date">month of the report </param>
        /// <param name="reportType">type of the report (Detallado,Resumido,Flujo semanal)</param>
        public CxPMovimientosxPeriodoReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, DateTime minDate, DateTime maxDate, bool isTercero, string reportType)
        {
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> dataC = null;

            #region Report data

            dataC = _bc.AdministrationModel.GetReportData(AppReports.cpMovimientosPeriodo, Consulta).ToList();

            DTO_BasicReport.FillRompimiento(dataC, "TerceroID", "DocumentoTercero");

            List<DTO_ReportCxPxMovimientosxPeriodo> reportData = new List<DTO_ReportCxPxMovimientosxPeriodo>();
            foreach (DTO_BasicReport itemC in dataC)
            {
                reportData.Add((DTO_ReportCxPxMovimientosxPeriodo)itemC);
            }

            if (isTercero)
                reportData = reportData.OrderBy(x => x.TerceroID.Value).ToList();
            else
                reportData = reportData.OrderBy(x => x.TerceroDesc.Value).ToList();

            #endregion

            #region Report field list

            ArrayList fieldListDet = new ArrayList() {  "Fecha", "CuentaID", "Descriptivo", "ComprobanteNro" };

            if (reportMM == TipoMoneda.Local)
            {
                fieldListDet.Add("vlrMdaLoc");
                //fieldListDet.Add("SaldoMdaLoc");
            }
            else
            {
                fieldListDet.Add("vlrMdaExt");
                //fieldListDet.Add("SaldoMdaExt");
            }

            #endregion

            CxPMovimientosxPeriodoReport reportDet = new CxPMovimientosxPeriodoReport(reportData, minDate, maxDate, fieldListDet, reportMM);
            reportDet.ShowPreview();
           
        }
        #endregion
    }
}
