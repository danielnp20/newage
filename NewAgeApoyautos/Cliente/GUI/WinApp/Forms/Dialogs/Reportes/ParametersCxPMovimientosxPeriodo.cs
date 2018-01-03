using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersCxPMovimientosxPeriodo : ReportParametersForm
    {
        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Libro Diario Report)
        /// </summary>
        public ParametersCxPMovimientosxPeriodo()
        {
            this.Module = ModulesPrefix.cp;
            this.ReportForm = AppReportParametersForm.cpMovimientosPeriodo;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            TipoMoneda reportMM = TipoMoneda.Local;
            bool istercero = true;
            switch (reportParameters["1"][0])
            {
                case "TerceroID":
                    istercero = true;
                    break;
            }
            switch (reportParameters["2"][0])
            {
                case "Local":
                    reportMM = TipoMoneda.Local;
                    break;

                case "Foreign":
                    reportMM = TipoMoneda.Foreign;
                    break;
            }

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();

            string tipoBalanceFuncional = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            userFilterList.Add(Filtro("saldo.BalanceTipoID", "=", "and", tipoBalanceFuncional));
            userFilterList.Add(Filtro("year(saldo.PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
            userFilterList.Add(Filtro("Month(saldo.PeriodoID)", "=", "and", this.periodoFilter1.Months[1].ToString()));
            userFilterList.Add(Filtro("year(aux.PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
            userFilterList.Add(Filtro("Month(aux.PeriodoID)", ">=", "and", this.periodoFilter1.Months[0].ToString()));
            userFilterList.Add(Filtro("Month(aux.PeriodoID)", "<=", "and", this.periodoFilter1.Months[1].ToString()));

            DateTime minDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0],1);
            DateTime maxDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[1], 1);

           // DateTime Date = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);

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
