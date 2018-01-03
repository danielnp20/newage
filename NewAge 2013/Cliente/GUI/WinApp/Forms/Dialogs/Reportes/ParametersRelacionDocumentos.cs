using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersRelacionDocumentos : ReportParametersForm
    {
        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Relacion Documentos Report)
        /// </summary>
        public ParametersRelacionDocumentos()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coRelacionDocumentos;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();

            List<ConsultasFields> consultaFields = new List<ConsultasFields>();
            //consultaFields.Add(new ConsultasFields("DocumentoID", "Documento", typeof(string)));
            consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
            consultaFields.Add(new ConsultasFields("PrefijoID", "Documetno Prefijo", typeof(string)));
            consultaFields.Add(new ConsultasFields("DocumentoNro", "Documento Numero", typeof(string)));

            if (this.Consulta != null && this.Consulta.Filtros != null)
            {
                userFilterList.AddRange(this.Consulta.Filtros);
                foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
            };

            userFilterList.Add(Filtro("Year(temp.Fecha)", ">=", "and", this.periodoFilter1.Year[0].ToString()));
            userFilterList.Add(Filtro("Year(temp.Fecha)", "<=", "and", this.periodoFilter1.Year[1].ToString()));
            userFilterList.Add(Filtro("Month(temp.Fecha)", ">=", "and", this.periodoFilter1.Months[0].ToString()));
            userFilterList.Add(Filtro("Month(temp.Fecha)", "<=", "and", this.periodoFilter1.Months[1].ToString()));
            userFilterList.Add(Filtro("temp.DocumentoID", "=", "and", reportParameters["1"][0]));

            DateTime minDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
            DateTime maxDate = new DateTime(this.periodoFilter1.Year[1], this.periodoFilter1.Months[1], 1);

            RelacionDocumentoReportBuilder rdrb = new RelacionDocumentoReportBuilder(userFilterList, consultaFields, minDate, maxDate,  reportParameters["1"][0], reportParameters["4"][0]);
        } 
        #endregion

        #region Funciones Privadas
        private DTO_glConsultaFiltro Filtro(string campoFisico, string operadorFiltro, string operadorSentencia, string valorFiltro)
        {
            DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro();
            filter.CampoFisico = campoFisico;
            filter.OperadorFiltro = operadorFiltro;
            filter.OperadorSentencia = operadorSentencia;
            filter.ValorFiltro = valorFiltro;

            return filter;
        }
        #endregion
    
    }
}
