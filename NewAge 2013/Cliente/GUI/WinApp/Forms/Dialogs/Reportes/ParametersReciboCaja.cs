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
    public class ParametersReciboCaja : ReportParametersForm
    {
        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Recibo de Caja)
        /// </summary>
        public ParametersReciboCaja()
        {
            this.Module = ModulesPrefix.ts;
            this.ReportForm = AppReportParametersForm.tsReciboCaja;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();

            List<ConsultasFields> consultaFields = new List<ConsultasFields>();
            consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));

            if (this.Consulta != null && this.Consulta.Filtros != null)
            {
                userFilterList.AddRange(this.Consulta.Filtros);
                foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
            };

            userFilterList.Add(Filtro("Year(docCtrl.Fecha)", ">=", "and", this.periodoFilter1.Year[0].ToString()));
            userFilterList.Add(Filtro("Year(docCtrl.Fecha)", "<=", "and", this.periodoFilter1.Year[1].ToString()));
            userFilterList.Add(Filtro("Month(docCtrl.Fecha)", ">=", "and", this.periodoFilter1.Months[0].ToString()));
            userFilterList.Add(Filtro("Month(docCtrl.Fecha)", "<=", "and", this.periodoFilter1.Months[1].ToString()));

            DateTime minDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
            DateTime maxDate = new DateTime(this.periodoFilter1.Year[1], this.periodoFilter1.Months[1], 1);

            if (reportParameters["1"][0].Contains("ReciboCaja"))
            {
                userFilterList.Add(Filtro("recibo.CajaID", "=", "and", "'" + reportParameters["2"][0] + "'"));
                userFilterList.Add(Filtro("docCtrl.DocumentoNro", ">=", "and", reportParameters["3"][0]));
                userFilterList.Add(Filtro("docCtrl.DocumentoNro", "<=", "and", reportParameters["3"][1]));
            }

            ReciboCajaReportBuilder rcrb = new ReciboCajaReportBuilder(userFilterList, consultaFields, minDate, maxDate, reportParameters["1"][0]);
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
                switch (reportParameters["1"][0])
                {
                    case "Consecutivo":
                    case "TerceroID":
                        ReportParameterList listCaja = (ReportParameterList)this.RPControls["2"];
                        listCaja.Enabled = false;
                        listCaja.Visible = false;
                        ReportParameterTextBox tbRecibo = (ReportParameterTextBox)this.RPControls["3"];
                        tbRecibo.Enabled = false;
                        tbRecibo.Visible = false;
                        break;
                    case "ReciboCaja":
                        listCaja = (ReportParameterList)this.RPControls["2"];
                        listCaja.Refresh();
                        listCaja.Enabled = true;
                        listCaja.Visible = true;
                        tbRecibo = (ReportParameterTextBox)this.RPControls["3"];
                        tbRecibo.Refresh();
                        tbRecibo.Enabled = true;
                        tbRecibo.Visible = true;
                        break;
                };
            };

            if (option.Equals("5"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                listRompimiento2.RefreshList();
                listRompimiento2.RemoveItem(((ReportParameterList)sender).GetSelectedValue()[0]);
            };

            if (option.Equals("6"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                if (reportParameters["6"][0] == "")
                {
                    ((ReportParameterList)sender).RefreshList();
                    ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                    ((ReportParameterList)sender).RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                };
            };
        }
        #endregion        
    
    }
}
