using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;
using System.Threading;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteAuxiliar : ReportParametersForm, IFiltrable
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;

                //Filtro para mostrar el reporte de acuerdo a la maneda
                switch (_TipoMoneda)
                {
                    //Pinta los reportes de Cuenta o Tercero con Moneda Local
                    case "Local":
                        {
                            #region Tipo De Reporte a mostrar el de cuenta o tercero
                            switch (_report)
                            {
                                case "CuentaID":
                                    reportName = this._bc.AdministrationModel.ReportesContabilidad_AuxiliarML(this._fechaIni, this._fechaFin, this._tipoLibro, this._cuentaInicial,
                                        this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu, this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;

                                case "TerceroID":
                                    reportName = this._bc.AdministrationModel.ReportesContabilidad_AuxiliarxTerceroML(this._fechaIni, this._fechaFin, 
                                        this._tipoLibro, this._cuentaInicial, this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu,
                                       this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;
                                case "Plantilla":

                                    #region Imprime el reporte de Libro mayor en Excel

                                    reportName = this._bc.AdministrationModel.ReportesContabilidad_PlantillaExcelAuxiliar(this._fechaIni, this._fechaFin, this._tipoLibro, this._cuentaInicial,
                                        this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu);

                                    if (reportName != string.Empty)
                                    {
                                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                        Process.Start(fileURl);
                                    }
                                    else
                                        MessageBox.Show(this._errorConsuta);

                                    #endregion

                                    break;

                            }
                            #endregion
                        }
                        break;
                    //Pinta los reportes de la cuenta o Tercero con Moneda Extranjera
                    case "Extranjera":
                        {
                            #region Tipo de Rerpote a mostrar el de cuenta o tercero
                            switch (_report)
                            {
                                case "CuentaID":
                                    reportName = this._bc.AdministrationModel.ReportesContabilidad_AuxiliarME(this._fechaIni, this._fechaFin, this._tipoLibro, this._cuentaInicial,
                                        this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu, this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;

                                case "TerceroID":
                                    reportName = this._bc.AdministrationModel.ReportesContabilidad_AuxiliarxTerceroME(this._fechaIni, this._fechaFin,
                                        this._tipoLibro, this._cuentaInicial, this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu,
                                       this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;
                            }
                            #endregion
                        }
                        break;
                    //Pinta los reportes de la cuenta o Tercero con ambas monedas
                    case "MultiMoneda":
                        {
                            #region Tipo de Reporte a Mostrar el de cuenta o tercero
                            switch (_report)
                            {
                                case "CuentaID":
                                    reportName = this._bc.AdministrationModel.ReportesContabiliad_AuxiliarMultiMoneda(this._fechaIni, this._fechaFin, this._tipoLibro,
                                        this._cuentaInicial, this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto, this._lineaPresu, this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;

                                case "TerceroID":
                                    reportName = this._bc.AdministrationModel.ReportesContabiliad_AuxiliarxTerceroMultiMoneda(this._fechaIni, this._fechaFin,
                                        this._tipoLibro, this._cuentaInicial, this._cuentaFinal, this._Tercero, this._proyecto, this._centroCosto,
                                       this._lineaPresu, this._formatType);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                                    Process.Start(fileURl);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteAuxiliar.cs", "TBSave"));
            }
            finally
            {
                try
                {
                    this.Invoke(this.EndGenerarDelegate);
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// Variable para iniciar el reporte
        /// </summary>
        private string _report = "CuentaID";
        private string _TipoMoneda = "Local";
        private string _tipoLibro;
        private string _cuentaInicial = "";
        private string _cuentaFinal = "";
        private string _Tercero = "";
        private string _proyecto = "";
        private string _centroCosto = "";
        private string _lineaPresu = "";
        public string _cierre = string.Empty;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        private string _comprobante = "";        

        //Varibale para mensaje
        private string _errorConsuta;

        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Auxiliar Report)
        /// </summary>
        public ParametersReporteAuxiliar()
        {
            this.Module = ModulesPrefix.co;
            //this.ReportForm = AppReportParametersForm.Auxiliar;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coAuxiliar;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coAuxiliar).ToString());

            this._errorConsuta = _bc.GetResourceError(DictionaryMessages.Rpt_gl_NoSeGeneranDatos);

            #region Configurar Opciones

            this._tipoLibro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

            //Itemns para el Combo tipo de Moneda
            List<ReportParameterListItem> tipoMoneda = new List<ReportParameterListItem>(){
                new ReportParameterListItem() { Key = "Local", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)},
                new ReportParameterListItem() { Key = "Extranjera", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)},
                new ReportParameterListItem() { Key = "MultiMoneda", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth)}};

            //Items para el combo de libros
            List<ReportParameterListItem> tipoLibros = new List<ReportParameterListItem>();
            long count = 0;
            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coBalanceTipo, null, null, true);
            IEnumerable<DTO_MasterBasic> TiposBalance = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coBalanceTipo, count, 1, null, null, true);
            foreach (var tipo in TiposBalance)
                tipoLibros.Add(new ReportParameterListItem() { Key = tipo.ID.ToString(), Desc = tipo.ID.ToString() + " - " + tipo.Descriptivo.ToString() });


            //Itemns Para el combo del ordenamiento
            List<ReportParameterListItem> Ordenamiento = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "CuentaID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) },
                new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) },
                new ReportParameterListItem() { Key = "Plantilla", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Plantilla) },
            };

            this.AddList("1", (AppForms.ReportForm).ToString() + "_Moneda", tipoMoneda, true, "Local");
            this.AddList("2", (AppForms.ReportForm).ToString() + "_Ordenamiento", Ordenamiento, true, "CuentaID");
//            this.AddList("3", ReportParameterListSource.TipoBalance, true, _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString());
            this.AddList("3", (AppForms.ReportForm).ToString() + "_TipoBalance", tipoLibros, true, this._tipoLibro);
            this.AddCheck("Cuentas", (AppForms.ReportForm).ToString() + "_RangoCuenta", true);
            //this.AddCheck("4.1", (AppForms.ReportForm).ToString() + "_cierre", true);
            this.AddMaster("4", AppMasters.coPlanCuenta, true, null);
            this.AddMaster("CuentaFin", AppMasters.coPlanCuenta, true, null, false);
            this.AddMaster("5", AppMasters.coTercero, true, null);
            this.AddMaster("6", AppMasters.coProyecto, true, null);
            this.AddMaster("7", AppMasters.coCentroCosto, true, null);
            this.AddMaster("8", AppMasters.plLineaPresupuesto, true, null);
            //this.AddMaster("9", AppMasters.coPlanCuenta, true, null);
            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            this.btnExportToXLS.Visible = true;
            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte para plantlla de excel
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                byte tipoReport = 0;
                if (this._report.Equals("CuentaID"))
                    tipoReport = 1;
                else if (this._report.Equals("TerceroID"))
                    tipoReport = 2;
                else if (this._report.Equals("Plantilla"))
                    tipoReport = 3;

                this._Query = this._bc.AdministrationModel.Reportes_Co_ContabilidadToExcel(this.documentReportID, tipoReport, this._fechaIni, this._fechaFin, this._Tercero, this._cuentaInicial,
                                        this._centroCosto, this._proyecto, this._lineaPresu, this._tipoLibro, this._comprobante, string.Empty, this._cuentaFinal, null, null);

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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteAuxiliar.cs", "TBSave"));
            }
            finally
            {
                try
                {
                    this.Invoke(this.EndGenerarDelegate);
                }
                catch (Exception ex)
                {
                }
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
            Dictionary<string, string[]> rangoCuentas = this.GetValues();
            ReportParameterList tipoMoneda = (ReportParameterList)this.RPControls["1"];
            ReportParameterList tipoOrdenamiento = (ReportParameterList)this.RPControls["2"];
            ReportParameterList tipoLibro = (ReportParameterList)this.RPControls["3"];
            uc_MasterFind masterTercero = (uc_MasterFind)this.RPControls["5"];
            uc_MasterFind masterCuentaFin = (uc_MasterFind)this.RPControls["CuentaFin"];

            #region Tipo de Moneda en que se presenta el reporte
            if (option.Equals("1"))
                //Carga el valor de la llave para imprimir el reporte en la un tipo de moneda especifico
                this._TipoMoneda = tipoMoneda.SelectedListItem.Key;
            #endregion

            #region Tipo de ordenamiento x Cuenta o Por Tercero
            if (option.Equals("2"))
            {
                //Carga el valor del filtro segun el reporte a pintar
                this._report = tipoOrdenamiento.SelectedListItem.Key;
                switch (this._report)
                {
                    case ("CuentaID"):
                        masterTercero.Visible = false;
                        break;
                    case ("TerceroID"):
                        masterTercero.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region Libro
            if (option.Equals("3"))
                this._tipoLibro = tipoLibro.SelectedListItem.Key;
            #endregion

            #region Verifica si quiere rango de cuentas

            if (option.Equals("Cuentas"))
            {
                switch (rangoCuentas["Cuentas"][0])
                {
                    case ("True"):
                        masterCuentaFin.Visible = true;
                        break;

                    case ("False"):
                        masterCuentaFin.Visible = false;
                        masterCuentaFin.txtCode.Text = string.Empty;
                        masterCuentaFin.Value = string.Empty;
                        this._cuentaFinal = string.Empty;
                        break;

                }
            }

            #endregion

            #region Cuenta Inicial
            if (option.Equals("4"))
            {
                uc_MasterFind masterIncial = (uc_MasterFind)sender;
                this._cuentaInicial = masterIncial.ValidID ? masterIncial.Value : string.Empty;
            }

            #region Check
            if(option.Equals("4.1"))
            {
                _cierre = "AND DAY(aux.PeriodoID) != 2";
            }
            #endregion

            #endregion

            #region Cuenta Final

            if (option.Equals("CuentaFin"))
            {
                uc_MasterFind masterCuentaFinal = (uc_MasterFind)sender;
                this._cuentaFinal = masterCuentaFinal.ValidID ? masterCuentaFinal.Value : string.Empty;
            }

            #endregion

            #region Tercero
            if (option.Equals("5"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._Tercero = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Proyecto
            if (option.Equals("6"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._proyecto = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

            #region Centro Costo

            if (option.Equals("7"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._centroCosto = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

            #region Linea Presupuestal
            if (option.Equals("8"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._lineaPresu = master.ValidID ? master.Value : string.Empty;
            }
            #endregion
        }
        #endregion

    }
}
