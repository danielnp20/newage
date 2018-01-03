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
    public class ParametersSaldosDocumentos : ReportParametersForm
    {
        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Saldos Documentos Report)
        /// </summary>
        public ParametersSaldosDocumentos()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coSaldosDocumentos;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            
            TipoMoneda reportMM = TipoMoneda.Local;

            switch (reportParameters["3"][0])
            {
                case "Local":
                    reportMM = TipoMoneda.Local;
                    break;

                case "Foreign":
                    reportMM = TipoMoneda.Foreign;
                    break;

                case "Both":
                    reportMM = TipoMoneda.Both;
                    break;
            };

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();
            List<ConsultasFields> consultaFields = new List<ConsultasFields>();

            DateTime Date = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);

            switch (reportParameters["1"][0])
            {
                case "11":
                case "12":
                    consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));  
                    consultaFields.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
                    consultaFields.Add(new ConsultasFields("DocumentoPrefijo", "Documetno Prefijo", typeof(string)));
                    consultaFields.Add(new ConsultasFields("DocumentoNumero", "Documento Numero", typeof(string)));
                    if (this.Consulta != null && this.Consulta.Filtros != null)
                    {
                        userFilterList.AddRange(this.Consulta.Filtros);
                        foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
                    }
                    userFilterList.Add(Filtro("Year(temp.Fecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(temp.Fecha)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("temp.DocumentoID", "=", "and", reportParameters["1"][0]));
                    SaldosDocumentoReportBuilder sdrb = new SaldosDocumentoReportBuilder(userFilterList, consultaFields, Date, reportMM, reportParameters["4"][0], reportParameters["5"][0], reportParameters["1"][0]);
                    break;

                case "21":
                    consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                    if (this.Consulta != null && this.Consulta.Filtros != null)
                    {
                        userFilterList.AddRange(this.Consulta.Filtros);
                        foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
                    }
                    userFilterList.Add(Filtro("Year(FacturaFecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(FacturaFecha)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("@OrigenMon", "=", "and", ((int)(TipoMoneda)Enum.Parse(typeof(TipoMoneda),reportParameters["6"][0])).ToString()));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "or", reportParameters["1"][0]));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "or", "26"));
                    FacturasPorPagarReportBuilder fprb = new FacturasPorPagarReportBuilder(userFilterList, consultaFields, Date, reportMM, reportParameters["5"][0], reportParameters["1"][0]);
                    break;

                case "22":       
                    consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                    consultaFields.Add(new ConsultasFields("TipoAnticipoID", "TipoAnticipo", typeof(string)));
                    if (this.Consulta != null && this.Consulta.Filtros != null)
                    {
                        userFilterList.AddRange(this.Consulta.Filtros);
                        foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
                    }
                    userFilterList.Add(Filtro("Year(a.RadicaFecha)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(a.RadicaFecha)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("docCtrl.DocumentoID", "=", "and", reportParameters["1"][0]));
                    AnticiposPendReportBuilder aprb = new AnticiposPendReportBuilder(userFilterList, consultaFields, Date, reportMM, reportParameters["5"][0], reportParameters["1"][0]);
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
            if (option.Equals("4"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["5"];
                switch (reportParameters["4"][0])
                {
                    case "CuentaID":
                        listReportType.DefaultKey = listReportType.SelectedListItem.Key;
                        listReportType.RefreshList();
                        break;
                    case "TerceroID":
                        if (listReportType.SelectedListItem.Key =="Consolidated")
                            listReportType.RefreshList();
                        listReportType.RemoveItem("Consolidated");
                        break;
                };
            };

            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();

                ReportParameterList listReportType = (ReportParameterList)this.RPControls["4"]; ;
                ReportParameterList monedaOr = (ReportParameterList)this.RPControls["6"]; ;
                switch (reportParameters["1"][0])
                {
                    case "11":
                    case "12":
                        listReportType.DefaultKey = listReportType.SelectedListItem.Key;
                        listReportType.RefreshList();

                        monedaOr.Visible = false;
                        monedaOr.Enabled = false;
                        break;
                    case "21":
                        listReportType.DefaultKey = "TerceroID";
                        listReportType.RefreshList();
                        listReportType.RemoveItem("CuentaID");

                        monedaOr.Visible = true;
                        monedaOr.Enabled = true;
                        break;
                    case "22":
                        listReportType.DefaultKey = "TerceroID";
                        listReportType.RefreshList();
                        listReportType.RemoveItem("CuentaID");

                        monedaOr.Visible = false;
                        monedaOr.Enabled = false;
                        break;
                };
            };
        }

        protected override void btnFilter_Click(object sender, EventArgs e)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            List<ConsultasFields> fields = new List<ConsultasFields>();
            MasterQuery mq;
            fields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
            mq = new MasterQuery(this, AppReports.coSaldosDocumentos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
            mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
            if (reportParameters["1"][0] == AppDocuments.Anticipos.ToString())
            {
                fields.Add(new ConsultasFields("AnticipoTipoID", "AnticipoTipo", typeof(string)));
                mq = new MasterQuery(this, AppReports.coSaldosDocumentos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
                mq.SetFK("TipoAnticipoID", AppMasters.cpAnticipoTipo, _bc.CreateFKConfig(AppMasters.cpAnticipoTipo));
            }
            if (reportParameters["1"][0] == AppDocuments.ComprobanteManual.ToString() || reportParameters["1"][0] == AppDocuments.DocumentoContable.ToString())
            {
                fields.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
                fields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                fields.Add(new ConsultasFields("DocumentoPrefijo", "Documetno Prefijo", typeof(string)));
                fields.Add(new ConsultasFields("DocumentoNumero", "Documento Numero", typeof(string)));
                mq = new MasterQuery(this, AppReports.coSaldosDocumentos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
                mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                mq.SetFK("DocumentoPrefijo", AppMasters.glPrefijo, _bc.CreateFKConfig(AppMasters.glPrefijo));
            }
            mq.ShowDialog();
        }
        #endregion

    }
}
