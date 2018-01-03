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
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteSaldos : ReportParametersForm
    {
        #region Variable

        private string cuentaIni = string.Empty;
        private string cuentaFin = string.Empty;

        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ParametersReporteSaldos()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coSaldos;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            List<string> RompIndList = new List<string>();

            if (reportParameters["5"][0] != "-")
            {
                RompIndList.Add(reportParameters["5"][0]);
                RompIndList.Add(reportParameters["4"][0]);
                RompIndList.Add(reportParameters["3"][0]);
            }
            else
            {
                if ((reportParameters["4"][0]) != "-")
                {
                    RompIndList.Add(reportParameters["4"][0]);
                    RompIndList.Add(reportParameters["3"][0]);
                    RompIndList.Add(reportParameters["5"][0].Replace("-", ""));
                }
                else
                {
                    RompIndList.Add(reportParameters["3"][0]);
                    RompIndList.Add(reportParameters["4"][0].Replace("-", ""));
                    RompIndList.Add(reportParameters["5"][0].Replace("-", ""));
                };
            };

            TipoMoneda reportMM = TipoMoneda.Local;

            switch (reportParameters["1"][0])
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
            consultaFields.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
            consultaFields.Add(new ConsultasFields("CentroCostoID", "Centro Costo", typeof(string)));
            consultaFields.Add(new ConsultasFields("ProyectoID", "Proyecto", typeof(string)));
            consultaFields.Add(new ConsultasFields("LineaPresupuestoID", "Linea Presupuesto", typeof(string)));
            consultaFields.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));

            if (this.Consulta != null && this.Consulta.Filtros != null)
            {
                userFilterList.AddRange(this.Consulta.Filtros);
                foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
            };

            userFilterList.Add(Filtro("Year(temp1.PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
            userFilterList.Add(Filtro("@MesIni", "=", "and", this.periodoFilter1.Months[0].ToString()));
            userFilterList.Add(Filtro("Month(temp1.PeriodoID)", "<=", "and", (this.periodoFilter1.Months[1] > 12) ? "12" : this.periodoFilter1.Months[1].ToString()));
            userFilterList.Add(Filtro("temp1.BalanceTipoID", "=", "or", "'" + reportParameters["2"][0] + "'"));
            userFilterList.Add(Filtro("temp1.BalanceTipoID", "=", "or", "'test'"));
            userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));

            if (this.cuentaIni != string.Empty && this.cuentaFin != string.Empty)
            {
                userFilterList.Add(Filtro("@cuentaIni", "=", "", "'" + this.cuentaIni + "'"));
                userFilterList.Add(Filtro("@cuentaFin", "=", "", "'" + this.cuentaFin + "'"));
            }
            else if (this.cuentaIni != string.Empty && this.cuentaFin == string.Empty)
            {
                userFilterList.Add(Filtro("@cuentaIni", "=", "", "'" + this.cuentaIni + "'"));
                userFilterList.Add(Filtro("@cuentaFin", "=", "", "'" + this.cuentaIni + "'"));
            }
            else
            {
                userFilterList.Add(Filtro("@cuentaIni", "=", "", "'" + 0 + "'"));
                userFilterList.Add(Filtro("@cuentaFin", "=", "", "'" + 999999999999 + "'"));
            }


            DateTime minDate = new DateTime(this.periodoFilter1.Year[0], (this.periodoFilter1.Months[0] > 12) ? 12 : this.periodoFilter1.Months[0], (this.periodoFilter1.Months[0] > 12) ? this.periodoFilter1.Months[0] - 11 : 1);
            DateTime maxDate = new DateTime(this.periodoFilter1.Year[0], (this.periodoFilter1.Months[1] > 12) ? 12 : this.periodoFilter1.Months[1], (this.periodoFilter1.Months[1] > 12) ? this.periodoFilter1.Months[1] - 11 : 1);

            SaldosReportBuilder srp = new SaldosReportBuilder(reportMM, RompIndList, userFilterList, consultaFields, minDate, maxDate, reportParameters["2"][0]);
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
            #region Opcion 3
            if (option.Equals("3"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ((ReportParameterList)sender).RemoveItem("-");
                ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["4"];
                listRompimiento2.RefreshList();
                listRompimiento2.RemoveItem(((ReportParameterList)sender).GetSelectedValue()[0]);
            }; 
            #endregion
            #region Opcion 4
            if (option.Equals("4"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listRompimiento3 = (ReportParameterList)this.RPControls["5"];
                if (reportParameters["4"][0] == "")
                {
                    ((ReportParameterList)sender).RefreshList();
                    ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["3"];
                    ((ReportParameterList)sender).RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                    listRompimiento3.Enabled = false;
                    listRompimiento3.RefreshList();
                }
                else
                {
                    if (reportParameters["4"][0] != "-")
                    {
                        listRompimiento3.RefreshList();
                        listRompimiento3.RemoveItem(((ReportParameterList)sender).GetSelectedValue()[0]);
                        ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["3"];
                        listRompimiento3.RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                        listRompimiento3.Enabled = true;
                    }
                    else
                    {
                        listRompimiento3.Enabled = false;
                        listRompimiento3.RefreshList();
                    };
                };
            }; 
            #endregion
            #region Opcion 5
            if (option.Equals("5"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                if (reportParameters["5"][0] == "")
                {
                    ((ReportParameterList)sender).RefreshList();
                    ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["3"];
                    ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["4"];
                    ((ReportParameterList)sender).RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                    ((ReportParameterList)sender).RemoveItem(listRompimiento2.GetSelectedValue()[0]);
                };
            }; 
            #endregion
            #region Filtro Rango Cuentas

            if (option.Equals("filtroCuentas"))
            {
                Dictionary<string, string[]> rangoCuentas = this.GetValues();
                uc_MasterFind masterCtaIni = (uc_MasterFind)this.RPControls["CuentaIncial"];
                uc_MasterFind masterCtaFin = (uc_MasterFind)this.RPControls["CuentaFinal"];

                switch (rangoCuentas["filtroCuentas"][0])
                {
                    case ("True"):

                        masterCtaIni.Visible = true;
                        masterCtaFin.Visible = true;

                        break;

                    case ("False"):

                        masterCtaIni.Visible = false;
                        masterCtaFin.Visible = false;
                        this.cuentaIni = string.Empty;
                        this.cuentaFin = string.Empty;

                        break;
                }

            }

            #endregion
            #region Carga la cuenta Inicial

            if (option.Equals("CuentaIncial"))
            {
                uc_MasterFind masterCuentaIni = (uc_MasterFind)sender;
                this.cuentaIni = masterCuentaIni.ValidID ? masterCuentaIni.Value : string.Empty;
            }
            if (option.Equals("CuentaFinal"))
            {
                uc_MasterFind masterCuentaFinal = (uc_MasterFind)sender;
                this.cuentaFin = masterCuentaFinal.ValidID ? masterCuentaFinal.Value : string.Empty;
            }

            #endregion
        }
        #endregion
    }
}