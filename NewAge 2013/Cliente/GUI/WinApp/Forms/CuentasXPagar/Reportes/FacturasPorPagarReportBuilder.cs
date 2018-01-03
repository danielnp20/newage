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
    class FacturasPorPagarReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Facturas Por Pagar Report (collects and organises report data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        /// <param name="Date">reprot period</param>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="reportType">indicates report Type (detailed, summarized)</param>
        /// <param name="docID">Documento ID</param>
        public FacturasPorPagarReportBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime Date, TipoMoneda reportMM, string reportType, string docID)
        {
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            #region Report data
            List<DTO_BasicReport> data  = _bc.AdministrationModel.GetReportData(AppReports.cpFacturasPorPagar, Consulta, consultaFields).ToList();
            DTO_BasicReport.FillRompimiento(data, "TerceroID");

            List<DTO_ReportFacturasPorPagar> reportData = new List<DTO_ReportFacturasPorPagar>();
            foreach (DTO_BasicReport item in data)
                reportData.Add((DTO_ReportFacturasPorPagar)item);

            reportData = reportData.OrderBy(x => x.FechaDoc).ToList();
            reportData = reportData.OrderBy(x => x.Documento).ToList();
            reportData = reportData.OrderBy(x => x.TerceroID).ToList();
               
            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (filter.CampoFisico.Contains("TerceroID"))
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
            };

            #region Signos de los totales
            decimal saldoTotalML_sinSigno = reportData.Sum(x => x.SaldoTotalML_sinSigno);
            decimal saldoTotalME_sinSigno = reportData.Sum(x => x.SaldoTotalME_sinSigno);

            int signFin = 1;
            if (saldoTotalML_sinSigno < 0 || saldoTotalME_sinSigno < 0)
                signFin = -1;
            #endregion
            #endregion

            #region Report field list
            bool detailedInd = true;
            ArrayList fieldList = null;
            switch (reportType) {
                case "Detailed":
                    fieldList = new ArrayList() { "Documento", "CuentaID", "CuentaDesc", "FechaDoc", "FechaVto", "ComprobanteID" }; 
                    break;
                case "Summarized":
                    detailedInd = false;
                    fieldList = new ArrayList() { "TerceroID", "TerceroDesc" };
                    break; };

            if (reportMM == TipoMoneda.Local) {
                fieldList.Add("VrBrutoML");
                fieldList.Add("VrNetoML");
                fieldList.Add("AbonosML");
                fieldList.Add("SaldoTotalML"); }
            else if (reportMM == TipoMoneda.Foreign) {
                fieldList.Add("VrNetoME");
                fieldList.Add("AbonosME");
                fieldList.Add("SaldoTotalME"); }
            else {
                fieldList.Add("VrNetoML");
                fieldList.Add("AbonosML");
                fieldList.Add("SaldoTotalML");
                fieldList.Add("VrNetoME");
                fieldList.Add("AbonosME");
                fieldList.Add("SaldoTotalME"); }
            #endregion

            FacturasPorPagarReport report = new FacturasPorPagarReport(reportData, fieldList, reportMM, Date, signFin, selectedFiltersList, detailedInd, docID);
            report.ShowPreview();
        } 
        #endregion
    }
}