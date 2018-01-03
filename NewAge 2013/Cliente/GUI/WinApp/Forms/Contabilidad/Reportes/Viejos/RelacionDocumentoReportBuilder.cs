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
    class RelacionDocumentoReportBuilder
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
        public RelacionDocumentoReportBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime minDate, DateTime maxDate,string docId, string Rompimiento)
        {
            #region Report data

            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);
            
            List<DTO_BasicReport> dataRD = _bc.AdministrationModel.GetReportData(AppReports.coRelacionDocumentos, Consulta, consultaFields).ToList();
            if (!Rompimiento.Contains("Consecutivo")) DTO_BasicReport.FillRompimiento(dataRD, Rompimiento);

            List<DTO_ReportRelacionDocumentos> reportData = new List<DTO_ReportRelacionDocumentos>();
            foreach (DTO_BasicReport itemRD_basic in dataRD)
            {
                DTO_ReportRelacionDocumentos itemRD = (DTO_ReportRelacionDocumentos)itemRD_basic;
                reportData.Add(itemRD);
            };

            reportData = reportData.OrderBy(x => x.Fecha).ToList();
            reportData = reportData.OrderBy(x => x.DocumentoPrefijo).ToList();
            reportData = reportData.OrderBy(x => x.DocumentoNumero_order).ToList();
            if (Rompimiento.Contains("TerceroID")) reportData = reportData.OrderBy(x => x.TerceroID).ToList();

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (filter.CampoFisico.Contains("TerceroID") || filter.CampoFisico.Contains("PrefijoID") || filter.CampoFisico.Contains("DocumentoNro"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report field list
            ArrayList fieldList = new ArrayList() { "DocumentoNombre_" + docId, "Fecha", "TerceroID", "TerceroDesc", "DocumentoDesc", "MonedaCodigo", "DocumentoEstado" };
            #endregion

            RelacionDocumentoReport report = new RelacionDocumentoReport(reportData, fieldList, minDate, maxDate, Rompimiento, docId, selectedFiltersList);
            report.ShowPreview();

        } 
        #endregion
    }
}
