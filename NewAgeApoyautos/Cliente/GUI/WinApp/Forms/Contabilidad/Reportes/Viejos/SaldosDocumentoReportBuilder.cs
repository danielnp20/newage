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
    class SaldosDocumentoReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null; 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Saldos Documentos Report (collects and organises report data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        /// <param name="Date">reprot period</param>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Rompimiento">indicates report rompimiento (por cuenta, por tercero)</param>
        /// <param name="reportType">indicates report Type (detailed,consolidated, summarized)</param>
        /// <param name="docID">Documento ID</param>
        public SaldosDocumentoReportBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime Date, TipoMoneda reportMM, string Rompimiento, string reportType, string docID)
        {
            #region Report data
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> dataSD = _bc.AdministrationModel.GetReportData(AppReports.coSaldosDocumentos, Consulta, consultaFields).ToList();

            if (Rompimiento.Contains("Cuenta")) DTO_BasicReport.FillRompimiento(dataSD, "CuentaID", "TerceroID");
            else DTO_BasicReport.FillRompimiento(dataSD, "TerceroID");


            List<DTO_ReportSaldosDocumentos> reportData = new List<DTO_ReportSaldosDocumentos>();
            foreach (DTO_BasicReport itemSD in dataSD)
            {
                reportData.Add((DTO_ReportSaldosDocumentos)itemSD);
            };

            reportData = reportData.OrderBy(x => x.DocumentoTercero).ToList();
            reportData = reportData.OrderBy(x => x.DocumentoNumero_order).ToList();
            reportData = reportData.OrderBy(x => x.DocumentoPrefijo).ToList();
            reportData = reportData.OrderBy(x => x.DocSaldoControl).ToList();
            reportData = reportData.OrderBy(x => x.TerceroID).ToList();


            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (filter.CampoFisico.Contains("CuentaID") || filter.CampoFisico.Contains("TerceroID") || filter.CampoFisico.Contains("DocumentoPrefijo") || filter.CampoFisico.Contains("DocumentoNumero"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report field list
            List<bool> RompIndList = null;
            ArrayList fieldList = null;

            if (Rompimiento.Contains("Cuenta"))
            {
                reportData = reportData.OrderBy(x => x.CuentaID).ToList();

                switch (reportType)
                {
                    case "Detailed":
                        RompIndList = new List<bool>() { true, true, false };
                        fieldList = new ArrayList() { "DocumentoNombre", "Fecha", "CuentaID", "DocumentoDesc", "TerceroID" };
                        break;

                    case "Summarized":
                        RompIndList = new List<bool>() { true, false, false };
                        fieldList = new ArrayList() { "TerceroID", "TerceroDesc" };
                        break;

                    case "Consolidated":
                        RompIndList = new List<bool>() { false, false, false };
                        fieldList = new ArrayList() { "CuentaID", "CuentaDesc" };
                        break;
                };
            }
            else
            {
                switch (reportType)
                {
                    case "Detailed":
                        RompIndList = new List<bool>() { true, false, true };
                        fieldList = new ArrayList() { "DocumentoNombre", "Fecha", "CuentaID", "DocumentoDesc", "TerceroID" };
                        break;

                    case "Summarized":
                        RompIndList = new List<bool>() { false, false, true };
                        fieldList = new ArrayList() { "TerceroID", "TerceroDesc" };
                        break;
                };
            };

            switch (reportMM)
            {
                case TipoMoneda.Local:
                    fieldList.Add("FinalML");
                    break;
                case TipoMoneda.Foreign:
                    fieldList.Add("FinalME");
                    break;
                case TipoMoneda.Both:
                    fieldList.Add("FinalML");
                    fieldList.Add("FinalME");
                    break;

            };
            #endregion

            SaldosDocumentoReport report = new SaldosDocumentoReport(reportData, fieldList, reportMM, Date, RompIndList, selectedFiltersList, docID);
            report.ShowPreview();

        } 
        #endregion
    }
}
