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
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Windows.Forms;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteBalPrueba : ReportParametersForm
    {
        #region Variable

        private string cuentaIni = string.Empty;
        private string cuentaFin = string.Empty;
        private DateTime Periodo;
        private string tipoReport = "DePrueba";
        private string libro = string.Empty; 
        
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Balance de Prueba Report)
        /// </summary>
        public ParametersReporteBalPrueba()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coBalancePrueba;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            List<string> RompIndList = new List<string>();

            if (reportParameters["1"][0] == "Detailed")
            {
                if ((reportParameters["6"][0]) != "-")
                {
                    RompIndList.Add(reportParameters["5"][0]);
                    RompIndList.Add(reportParameters["6"][0]);
                }
                else
                {
                    RompIndList.Add(reportParameters["5"][0]);
                    RompIndList.Add(reportParameters["6"][0].Replace("-", ""));
                };
            }
            else
            {
                RompIndList.Add("");
                RompIndList.Add("");
            };

            TipoMoneda reportMM = TipoMoneda.Local;

            switch (reportParameters["2"][0])
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

            if (this.Consulta != null && this.Consulta.Filtros != null)
            {
                userFilterList.AddRange(this.Consulta.Filtros);
                foreach (DTO_glConsultaFiltro filter in userFilterList) if (filter.OperadorSentencia == "N/A") filter.OperadorSentencia = "AND";
            };

            userFilterList.Add(Filtro("b.BalanceTipoID", "=", "or", "'" + reportParameters["4"][0] + "'"));
            userFilterList.Add(Filtro("b.BalanceTipoID", "=", "or", "'test'"));

            string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
            Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
            DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
            int cuentaLength = table.CodeLength(table.LevelsUsed());

            if (!reportParameters["3"][0].Contains("General") && !reportParameters["8"][0].Contains("Max"))
            {
                cuentaLength = 0;
                int i = 1;
                while (table.CodeLength(i) <= Convert.ToInt16(reportParameters["8"][0]))
                {
                    cuentaLength = table.CodeLength(i);
                    i++;
                }
            };

            switch (reportParameters["3"][0])
            {
                case "DePrueba":
                    userFilterList.Add(Filtro("Year(PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("@MesIni", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("@MesFin", "=", "and", this.periodoFilter1.Months[1].ToString()));
                    userFilterList.Add(Filtro("Month(PeriodoID)", "<=", "and", (this.periodoFilter1.Months[1] > 12) ? "12" : this.periodoFilter1.Months[1].ToString()));
                    userFilterList.Add(Filtro("@CuentaLength", "=", "and", cuentaLength.ToString()));
                    userFilterList.Add(Filtro("@SaldoInicialInd", "=", "and", (reportParameters["9"][0].ToLower().Contains("true")) ? "1" : "0"));
                    userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));
                    if (this.cuentaIni != string.Empty && this.cuentaFin != string.Empty)
                    {
                        userFilterList.Add(Filtro("@cuentaIni", "=", "", "'" + this.cuentaIni + "'"));
                        userFilterList.Add(Filtro("@cuentaFin", "=", "", "'" + this.cuentaFin + "'"));
                    }
                    else if(this.cuentaIni != string.Empty && this.cuentaFin == string.Empty)
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

                    BalanceDePruebaReportBuilder bdprp = new BalanceDePruebaReportBuilder(reportMM, userFilterList, consultaFields, reportParameters["4"][0], minDate, maxDate, RompIndList);
                    break;

                case "PorM":
                    userFilterList.Add(Filtro("Year(PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("@CuentaLength", "=", "and", cuentaLength.ToString()));
                    userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));
                    //userFilterList.Add(Filtro("@SaldoInicialInd", "=", "and", (reportParameters["9"][0].ToLower().Contains("true")) ? "1" : "0"));

                    DateTime Date = new DateTime(this.periodoFilter1.Year[0], 1, 1);
                    BalanceDePruebaPorMesesReportBuilder bdppm = new BalanceDePruebaPorMesesReportBuilder(reportMM, userFilterList, consultaFields, reportParameters["4"][0], Date);
                    break;

                case "PorQ":
                    userFilterList.Add(Filtro("Year(PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("@CuentaLength", "=", "and", cuentaLength.ToString()));
                    userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));
                    //userFilterList.Add(Filtro("@SaldoInicialInd", "=", "and", (reportParameters["9"][0].ToLower().Contains("true")) ? "1" : "0"));

                    Date = new DateTime(this.periodoFilter1.Year[0], 1, 1);
                    BalanceDePruebaPorQReportBuilder bdppq = new BalanceDePruebaPorQReportBuilder(reportMM, userFilterList, consultaFields, reportParameters["4"][0], Date);
                    break;

                case "Comparativo":
                    userFilterList.Add(Filtro("@Year", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(PeriodoID)", "<=", "and", this.periodoFilter1.Months[1].ToString()));
                    userFilterList.Add(Filtro("@CuentaLength", "=", "and", cuentaLength.ToString()));
                    userFilterList.Add(Filtro("@SaldoInicialInd", "=", "and", (reportParameters["9"][0].ToLower().Contains("true")) ? "1" : "0"));
                    userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));

                    minDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
                    maxDate = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[1], 1);

                    BalanceDePruebaComparativoReportBuilder bdpcrp = new BalanceDePruebaComparativoReportBuilder(reportMM, userFilterList, consultaFields, reportParameters["4"][0], minDate, maxDate);
                    break;

                case "General":
                    userFilterList.Add(Filtro("Year(PeriodoID)", "=", "and", this.periodoFilter1.Year[0].ToString()));
                    userFilterList.Add(Filtro("Month(PeriodoID)", "=", "and", this.periodoFilter1.Months[0].ToString()));
                    userFilterList.Add(Filtro("@EmpresaID", "=", "", "'" + this._bc.AdministrationModel.Empresa.ID.Value + "'"));

                    for (int i = 2; i < 7; i++)
                        userFilterList.Add(Filtro("@Level" + i.ToString(), "=", "and", table.CodeLength(i).ToString()));

                    Date = new DateTime(this.periodoFilter1.Year[0], this.periodoFilter1.Months[0], 1);
                    BalanceGeneralReportBuilder bgrb = new BalanceGeneralReportBuilder(reportMM, userFilterList, consultaFields, reportParameters["4"][0], Date);
                    break;
            }
        }

        /// <summary>
        /// Funcion que Genera el Reporte en PDF
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                string fechaIniString;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this.Periodo = Convert.ToDateTime(fechaIniString);

                string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
                Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
                DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                int cuentaLength = table.CodeLength(table.LevelsUsed());

                if (!reportParameters["3"][0].Contains("General") && !reportParameters["8"][0].Contains("Max"))
                {
                    cuentaLength = 0;
                    int i = 1;
                    while (table.CodeLength(i) <= Convert.ToInt16(reportParameters["8"][0]))
                    {
                        cuentaLength = table.CodeLength(i);
                        i++;
                    }
                }
                
                //Verifica el libro a imprimir
                var l = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string libroFiltro = this.libro == string.Empty ? l : this.libro;               

                //Verifica la moneda a imprimir
                string moneda = string.Empty;

                switch (reportParameters["2"][0])
                {
                    case "Local":
                        moneda = "Local";
                        break;

                    case "Foreign":
                        moneda = "Foreign";
                        break;

                    case "Both":
                        moneda ="Both";
                        break;
                };

                this._Query = this._bc.AdministrationModel.ReportesContabilidad_BalancePruebas(periodo, cuentaLength, 1, this.cuentaIni, this.cuentaFin, libroFiltro, tipoReport, moneda);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteBalPrueba", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
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
            #region Filtro Libro
            if (option.Equals("4"))
            {
                ReportParameterList typeBook = (ReportParameterList)this.RPControls["4"];
                this.libro = typeBook.SelectedListItem.Key;
            }

            #endregion
            #region Filtro 3
            if (option.Equals("3"))
            {
                
                ReportParameterList typeRpt = (ReportParameterList)this.RPControls["3"];
                Dictionary<string, string[]> reportParameters = this.GetValues();
                this.tipoReport = typeRpt.SelectedListItem.Key;
                switch (reportParameters["3"][0])
                {
                    case "DePrueba":
                        if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonthSpan || !this.periodoFilter1.monthCB.Name.Contains("13"))
                        {
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                            //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                            this.periodoFilter1.monthCB.Name = "monthCB_13";
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB.Enabled = true;
                            this.periodoFilter1.monthCB.AllowDrop = true;
                            this.periodoFilter1.monthCB1.Name = "monthCB_13";
                            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date);
                            this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        }

                        ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                        if (listReportType.Visible != true)
                        {
                            listReportType.RefreshList();
                            listReportType.Enabled = true;
                            listReportType.Visible = true;
                        }

                        ReportParameterList listMoneda = (ReportParameterList)this.RPControls["2"];
                        listMoneda.DefaultKey = listMoneda.SelectedListItem.Key;
                        listMoneda.RefreshList();

                        ReportParameterList cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        CheckEdit saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        }
                        break;
                    case "PorM":
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false;
                        break;
                    case "PorQ":
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false;
                        break;
                    case "Comparativo":

                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB.Enabled = false;
                        this.periodoFilter1.monthCB.AllowDrop = false;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        }
                        break;

                    case "General":
                        if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonth)
                        {
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                            //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB.Enabled = true;
                            this.periodoFilter1.monthCB.AllowDrop = true;
                        }

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        cuentaLength.RefreshList();
                        cuentaLength.Enabled = false;
                        cuentaLength.Visible = false;

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false;
                        break;
                };
            };
            #endregion
            #region Filtro 1
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                switch (reportParameters["1"][0])
                {
                    case "Detailed":
                        ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                        listRompimiento1.Enabled = true;
                        listRompimiento1.Visible = true;
                        listRompimiento1.RefreshList();
                        listRompimiento1.RemoveItem("-");
                        ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                        listRompimiento2.Enabled = true;
                        listRompimiento2.Visible = true;
                        listRompimiento2.RefreshList();
                        listRompimiento2.RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                        break;
                    case "Consolidated":
                        listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                        listRompimiento1.RefreshList();
                        listRompimiento1.Enabled = false;
                        listRompimiento1.Visible = false;
                        listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                        listRompimiento2.RefreshList();
                        listRompimiento2.Enabled = false;
                        listRompimiento2.Visible = false;
                        break;
                };
            };
            #endregion
            #region Filtro 5
            if (option.Equals("5"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                listRompimiento2.RefreshList();
                listRompimiento2.RemoveItem(((ReportParameterList)sender).GetSelectedValue()[0]);
            };
            #endregion
            #region Filtro 6
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

                    case("False"):

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
