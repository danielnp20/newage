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
using NewAge.ReportesComunes;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class ReciboCajaReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Relacion Documentos Report (collects and organises report data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        /// <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="docId">id del documento que esta mostrado en el reporte</param>
        /// <param name="Rompimiento">indocates report rompimiento (por documento, por tercero)</param>
        public ReciboCajaReportBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime minDate, DateTime maxDate, string reportType)
        {
            #region Report data
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> dataRC = _bc.AdministrationModel.GetReportData(AppReports.coReciboCaja, Consulta, consultaFields).ToList();

            if (reportType.Contains("Tercero")) 
                DTO_BasicReport.FillRompimiento(dataRC, reportType);
            else if (reportType.Contains("ReciboCaja"))
                DTO_BasicReport.FillRompimiento(dataRC, "ReciboNro");
            else
                DTO_BasicReport.FillRompimiento(dataRC, "CajaID","Fecha");

            List<DTO_ReportReciboCaja> reportData = new List<DTO_ReportReciboCaja>();
            foreach (DTO_BasicReport itemRC_basic in dataRC)
            {
                DTO_ReportReciboCaja itemRC = (DTO_ReportReciboCaja)itemRC_basic;
                itemRC.Valor_letters = CurrencyFormater.GetCurrencyString("ES1", itemRC.MonedaID, itemRC.Valor);
                reportData.Add(itemRC);
            };

            reportData = reportData.OrderBy(x => x.Fecha).ToList();

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (filter.CampoFisico.Contains("TerceroID"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report Por dia, Report Por Tercero
            if (!reportType.Contains("ReciboCaja"))
            {
                #region Report field list
                ArrayList fieldList = new ArrayList()  { "CajaID_calc", "CajaDesc", "Fecha", "TerceroID", "TerceroDesc", "DocumentoDesc", "MonedaID", "Valor" };
                if (reportType.Contains("Tercero"))
                    fieldList.Remove("TerceroDesc");
                else
                    fieldList.Remove("CajaDesc");
                #endregion

                ReciboCajaSummaryReport report = new ReciboCajaSummaryReport(reportData, fieldList, minDate, maxDate, reportType, selectedFiltersList);
                report.ShowPreview();
                
            }
            #endregion

            #region Report Recibo De caja
            else
            {
                #region Report field list
                ArrayList detailFieldList = new ArrayList();
                detailFieldList.AddRange(ColumnsInfo.ReciboCajaFields);   

                //ArrayList footerFieldList = new ArrayList() 
                //{ 
                //    "FP",
                //    "Documento",
                //    "Valor",
                //};
                #endregion

                ReciboCajaReport report = new ReciboCajaReport(AppReports.coReciboCaja, reportData, detailFieldList, _bc);
                report.ShowPreview();
            }
            #endregion
        }
        #endregion
    }
}
