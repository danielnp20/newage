using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors;
using System.Collections;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports.Formularios;
using NewAge.Cliente.GUI.WinApp.Reports.Certificados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersCertificates : ReportParametersForm
    {
        #region Variables
        private DTO_coImpuestoDeclaracion _impuestoInfo;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ParametersCertificates()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coCertificates;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();
            userFilterList.Add(Filtro("impC.ImpuestoDeclID", "=", "and", reportParameters["1"][0]));
            userFilterList.Add(Filtro("Year(aux.Fecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));

            #region Filtro adicional
            List<ConsultasFields> consultaFields = new List<ConsultasFields>();
            consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));

            if (this.Consulta != null && this.Consulta.Filtros != null)
            {
                userFilterList.AddRange(this.Consulta.Filtros);
                foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
            };
            #endregion           

            CertificatesReportBuilder crp = new CertificatesReportBuilder(userFilterList, consultaFields, this.periodoFilter1.Year[0], this._impuestoInfo.ImpuestoTipoID.Value, reportParameters["2"][0]);
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

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                if (!string.IsNullOrEmpty(reportParameters["1"][0].Trim()) && reportParameters["1"][0] != "*")
                {
                    this._impuestoInfo = (DTO_coImpuestoDeclaracion)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, false, reportParameters["1"][0], true);

                    ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];
                    listReportType.RefreshList();
                    listReportType.Enabled = true;

                    this.periodoFilter1.txtYear.Text = this.periodo.Date.Year.ToString();
                    this.periodoFilter1.Enabled = true;
                }
            }
        }
        #endregion
    }
}
