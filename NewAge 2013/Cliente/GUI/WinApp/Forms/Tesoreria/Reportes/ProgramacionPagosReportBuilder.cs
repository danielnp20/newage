using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using System.Collections;
using SentenceTransformer;
using System.Reflection;
using NewAge.ReportesComunes;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class ProgramacionPagosReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Saldos Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="RompIndList">list of bool parameters to enable rompimientos (true - enabled)</param>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        ///  <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="tipoBalance">tipo de balance</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public ProgramacionPagosReportBuilder()
        {
            #region Report data

            //Consulta = new DTO_glConsulta();

            List<DTO_BasicReport> data = _bc.AdministrationModel.GetReportData(AppReports.tsProgramacionPagos, Consulta).ToList();
            DTO_BasicReport.FillRompimiento(data, "BancoCuentaID", "TerceroID");

            List<DTO_ReportProgramacionPagos> reportData = new List<DTO_ReportProgramacionPagos>();
            foreach (DTO_BasicReport item in data)
            {
                reportData.Add((DTO_ReportProgramacionPagos)item);
            };
            reportData = reportData.OrderBy(x => x.BancoCuentaID).ToList();
            reportData = reportData.OrderBy(x => x.TerceroID).ToList();
            reportData = reportData.OrderBy(x => x.Factura).ToList();
         
            #endregion

            #region Report field list
            ArrayList fieldList = ColumnsInfo.ProgramacionPagosFields;
            #endregion

            ProgramacionPagosReport report = new ProgramacionPagosReport(AppReports.tsProgramacionPagos, reportData, fieldList, _bc);
            report.ShowPreview();

        } 
        #endregion
    }
}
