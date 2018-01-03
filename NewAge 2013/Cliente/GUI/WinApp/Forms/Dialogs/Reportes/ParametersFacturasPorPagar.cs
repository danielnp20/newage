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
    public class ParametersFacturasPorPagar : ReportParametersForm
    {
        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Facturas Por Pagar Report)
        /// </summary>
        public ParametersFacturasPorPagar()
        {
            this.Module = ModulesPrefix.cp;
            this.ReportForm = AppReportParametersForm.cpFacturasPorPagar;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            
            //TipoMoneda reportMM = TipoMoneda.Local;

            //switch (reportParameters["4"][0])
            //{
            //    case "Local":
            //        reportMM = TipoMoneda.Local;
            //        break;

            //    case "Foreign":
            //        reportMM = TipoMoneda.Foreign;
            //        break;

            //    case "Both":
            //        reportMM = TipoMoneda.Both;
            //        break;
            //};

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();

            List<ConsultasFields> consultaFields = new List<ConsultasFields>();
            consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));          

            switch (reportParameters["1"][0])
            {
                case "21":
                    if (this.Consulta != null && this.Consulta.Filtros != null)
                    {
                        userFilterList.AddRange(this.Consulta.Filtros);
                        foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
                    };
                    userFilterList.Add(Filtro("Year(FacturaFecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(FacturaFecha)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("@OrigenMon", "=", "and", ((int)(TipoMoneda)Enum.Parse(typeof(TipoMoneda),reportParameters["3"][0])).ToString()));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "or", reportParameters["1"][0]));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "or", "26"));
                    DateTime Date = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
                    FacturasPorPagarReportBuilder fprb = new FacturasPorPagarReportBuilder(userFilterList, consultaFields, Date, (TipoMoneda)Enum.Parse(typeof(TipoMoneda), reportParameters["4"][0]), reportParameters["2"][0], reportParameters["1"][0]);
                    break;

                case "22":
                    consultaFields.Add(new ConsultasFields("TipoAnticipoID", "TipoAnticipo", typeof(string)));
                    if (this.Consulta != null && this.Consulta.Filtros != null)
                    {
                        userFilterList.AddRange(this.Consulta.Filtros);
                        foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
                    };

                    userFilterList.Add(Filtro("Year(a.RadicaFecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(a.RadicaFecha)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "and", reportParameters["1"][0]));
                    Date = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
                    AnticiposPendReportBuilder aprb = new AnticiposPendReportBuilder(userFilterList, consultaFields, Date, (TipoMoneda)Enum.Parse(typeof(TipoMoneda), reportParameters["4"][0]), reportParameters["2"][0], reportParameters["1"][0]);
                    break;
            };           
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
                ReportParameterList listMonedaOr = (ReportParameterList)this.RPControls["3"];
                if (reportParameters["1"][0] == AppDocuments.CausarFacturas.ToString())
                {
                    listMonedaOr.Enabled = true;
                    listMonedaOr.Visible = true;
                }
                else
                {
                    listMonedaOr.Enabled = false;
                    listMonedaOr.Visible = false;
                }
            }
        }

        protected override void btnFilter_Click(object sender, EventArgs e)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            List<ConsultasFields> fields = new List<ConsultasFields>();
            MasterQuery mq;
            fields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
            mq = new MasterQuery(this, AppReports.cpFacturasPorPagar, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
            mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
            if (reportParameters["1"][0] != AppDocuments.CausarFacturas.ToString())
            {
                fields.Add(new ConsultasFields("AnticipoTipoID", "AnticipoTipo", typeof(string)));
                mq = new MasterQuery(this, AppReports.cpFacturasPorPagar, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
                mq.SetFK("TipoAnticipoID", AppMasters.cpAnticipoTipo, _bc.CreateFKConfig(AppMasters.cpAnticipoTipo));
            }
            mq.ShowDialog();
        }
        #endregion        
    }
}