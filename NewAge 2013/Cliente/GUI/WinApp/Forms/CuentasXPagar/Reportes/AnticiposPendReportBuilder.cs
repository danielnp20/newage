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
    class AnticiposPendReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Anticipos Pendientes Report (collects and organises report data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        /// <param name="Date">reprot period</param>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="reportType">indicates report Type (detailed, summarized)</param>
        /// <param name="docID">Documento ID</param>
        public AnticiposPendReportBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime Date, TipoMoneda reportMM, string reportType, string docID)
        {
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            #region Report data
            List<DTO_BasicReport> data = _bc.AdministrationModel.GetReportData(AppReports.cpAnticiposPendientes, Consulta, consultaFields).ToList();
            DTO_BasicReport.FillRompimiento(data,"TerceroID");

            List<DTO_ReportAnticipo> reportData = new List<DTO_ReportAnticipo>();
            foreach (DTO_BasicReport item in data)
            {
                DTO_ReportAnticipo itemA = (DTO_ReportAnticipo)item;
                if (itemA.MonedaID == this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_MonedaLocal))
                {
                    itemA.ValorML = itemA.Valor;
                    itemA.ValorME = (itemA.TasaCambio!=0)? itemA.Valor/itemA.TasaCambio:0;
                }
                else
                {
                    itemA.ValorME = itemA.Valor;
                    itemA.ValorML = itemA.Valor*itemA.TasaCambio;
                }
                reportData.Add((DTO_ReportAnticipo)item);
            }

            reportData = reportData.OrderBy(x => x.Fecha).ToList();
            reportData = reportData.OrderBy(x => x.Documento).ToList();
            reportData = reportData.OrderBy(x => x.TerceroID).ToList();
               
            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (filter.CampoFisico.Contains("TerceroID"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report field list
            bool detailedInd = true;
            ArrayList fieldList = null;
            switch (reportType) {
                case "Detailed":
                    fieldList = new ArrayList() { "AnticipoTipoID", "Documento", "Observacion", "Fecha", "ValorML", "ValorME" };  
                    break;
                case "Summarized":
                    detailedInd = false;
                    fieldList = new ArrayList() { "TerceroID", "TerceroDesc", "ValorML", "ValorME" };
                    break; };

            if (reportMM == TipoMoneda.Local) 
                fieldList.Remove("ValorME");
            else if (reportMM == TipoMoneda.Foreign)
                fieldList.Remove("ValorML");
            #endregion

            AnticiposPendReport report = new AnticiposPendReport(reportData, fieldList, reportMM, Date, selectedFiltersList, detailedInd, docID);
            report.ShowPreview();
        } 
        #endregion
    }
}