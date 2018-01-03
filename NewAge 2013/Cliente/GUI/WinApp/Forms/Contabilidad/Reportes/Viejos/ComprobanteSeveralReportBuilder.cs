using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Collections;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using NewAge.ReportesComunes;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class ComprobanteSeveralReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        ///  Builder for the Report on all Comprobantes in database (collects and organises report data)
        /// </summary>
        /// <param name="userFilterList">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="Date">el periodo del reporte</param>
        /// <param name="isPre">true - reporte sobre Comprobantes Preliminar</param>
        public ComprobanteSeveralReportBuilder(List<DTO_glConsultaFiltro> userFilterList, List<ConsultasFields> consultaFields,string reportType, DateTime Date,bool isPre)
        {
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(userFilterList);

            #region Detalle de los Comprobantes
            if (reportType.Contains("Detailed"))
            {
                #region Report data
                List<DTO_BasicReport> dataC = _bc.AdministrationModel.GetReportData(AppReports.ReporteComprobantes, Consulta, consultaFields).ToList();

                List<DTO_ReportComprobante2> reportData = new List<DTO_ReportComprobante2>();
                foreach (DTO_BasicReport itemC in dataC)
                {
                    reportData.Add((DTO_ReportComprobante2)itemC);
                };

                reportData = reportData.OrderBy(x => x.Header.ComprobanteNro.Value).ToList();
                reportData = reportData.OrderBy(x => x.Header.ComprobanteID.Value).ToList();


                foreach (DTO_ReportComprobante2 item in reportData)
                {
                    item.footerReport = item.footerReport.OrderBy(x => x.LineaPresupuestoID.Value).ToList();
                    item.footerReport = item.footerReport.OrderBy(x => x.ConceptoCargoID.Value).ToList();
                    item.footerReport = item.footerReport.OrderBy(x => x.ProyectoID.Value).ToList();
                    item.footerReport = item.footerReport.OrderBy(x => x.TerceroID.Value).ToList();
                    item.footerReport = item.footerReport.OrderBy(x => x.CuentaID.Value).ToList();
                };

                bool MultiMonedaInd = _bc.AdministrationModel.MultiMoneda;
                int documentId = AppReports.SeveralComprobantes;

                List<string> selectedFiltersList = new List<string>();
                foreach (DTO_glConsultaFiltro filter in userFilterList)
                {
                    if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("coAuxiliar"))
                    {
                        selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                    }
                };

                #endregion

                #region Report field list
                ArrayList fieldList = new ArrayList();
                fieldList.AddRange(ColumnsInfo.ComprobanteFields);

                if (MultiMonedaInd)
                {
                    fieldList.Add("DebitoME");
                    fieldList.Add("CreditoME");
                };
                #endregion

                ComprobanteReport report = new ComprobanteReport(documentId, reportData, MultiMonedaInd, fieldList, isPre, _bc, selectedFiltersList, Date);
                report.ShowPreview();
            }
            #endregion

            #region Control de los comprobantes
            else
            {
                #region Report data
                List<DTO_BasicReport> dataCC = _bc.AdministrationModel.GetReportData(AppReports.ComprobanteControl, Consulta).ToList();
                List<DTO_ReportComprobanteControl> reportDataCtrl = new List<DTO_ReportComprobanteControl>();
                foreach (DTO_BasicReport item in dataCC)
                {
                    DTO_ReportComprobanteControl itemCC = (DTO_ReportComprobanteControl)item;
                    itemCC.CompControlDetail = itemCC.CompControlDetail.OrderBy(x => x.ComprobanteNro).ToList();
                    DTO_ComprobanteControlResume itemCR = new DTO_ComprobanteControlResume();
                    itemCR.ComprobanteNro_From = itemCC.CompControlDetail.First().ComprobanteNro;
                    itemCR.ComprobanteNro_Till = itemCC.CompControlDetail.First().ComprobanteNro;
                    itemCR.RecordQty = itemCC.CompControlDetail.First().RecordQty;
                    itemCR.ComprobanteQty = 1;

                    for (int i = 1; i < itemCC.CompControlDetail.Count; i++)
                    {
                        if (itemCC.CompControlDetail[i].ComprobanteNro > itemCR.ComprobanteNro_Till + 1)
                        {
                            itemCC.CompControlResume.Add(itemCR);
                            itemCR = new DTO_ComprobanteControlResume();
                            itemCR.ComprobanteNro_From = itemCC.CompControlDetail[i].ComprobanteNro;
                            itemCR.ComprobanteNro_Till = itemCC.CompControlDetail[i].ComprobanteNro;
                            itemCR.RecordQty = itemCC.CompControlDetail[i].RecordQty;
                            itemCR.ComprobanteQty = 1;
                        }
                        else
                        {
                            itemCR.ComprobanteNro_Till = itemCC.CompControlDetail[i].ComprobanteNro;
                            itemCR.RecordQty += itemCC.CompControlDetail[i].RecordQty;
                            itemCR.ComprobanteQty++;
                        }
                    }
                    itemCC.CompControlResume.Add(itemCR);
                    reportDataCtrl.Add(itemCC);
                };
                reportDataCtrl = reportDataCtrl.OrderBy(x => x.ComprobanteID).ToList();
                #endregion

                #region Report field list
                ArrayList fieldListCtrl = new ArrayList() { "ComprobanteNro_From", "ComprobanteNro_Till" };               
                #endregion

                ComprobanteControlReport report = new ComprobanteControlReport(reportDataCtrl, fieldListCtrl, Date);
                report.ShowPreview();
            }
            #endregion 
        } 
        #endregion
    }
}
