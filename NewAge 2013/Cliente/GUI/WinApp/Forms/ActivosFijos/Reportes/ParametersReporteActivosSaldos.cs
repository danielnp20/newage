using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersReporteActivosSaldos : ReportParametersForm
    {

        #region Hilos
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                if (this._report == this._libroFunc)
                    this.result.Result = ResultValue.NOK;
                else
                {
                    switch (this._tipoMoneda)
                    {
                        //Genera el reporte en Moneda Local
                        case ("1"):
                            this.result = this._bc.AdministrationModel.ReportesActivos_SaldosML(this._report, this._fechaIni, this._plaqueta,
                                this._serial, this._referencia, this._Clase, this._tipo, this._Grupos, this._propietarios, this._formatType);
                            break;
                        //Genera el reporte en Moneda Estranjera
                        case ("2"):
                            this.result = this._bc.AdministrationModel.ReportesActivos_SaldosME(this._report, this._fechaIni, this._plaqueta,
                                this._serial, this._referencia, this._Clase, this._tipo, this._Grupos, this._propietarios, this._formatType);
                            break;
                        //Genera el reporte en Ambas Monedas 
                        case ("3"):
                            this.result = this._bc.AdministrationModel.ReportesActivos_Saldos(this._report, this._fechaIni, this._plaqueta,
                                this._serial, this._referencia, this._Clase, this._tipo, this._Grupos, this._propietarios, this._formatType);
                            break;
                    }
                }

                #region Generacion del reporte

                if (this.result.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, result.ToString());
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }
                if (!string.IsNullOrEmpty(fileURl))
                    Process.Start(fileURl);
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
                    return;
                } 

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteActivosSaldos.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Variable
        //Variables para el reporte
        private string _report;
        private string _tipoMoneda = "1";
        private string _plaqueta = "";
        private string _serial = "";
        private string _referencia = "";
        private string _Clase = "";
        private string _tipo = "";
        private string _Grupos = "";
        private string _propietarios = "";
        DateTime _fechaIni;
        DTO_TxResult result = new DTO_TxResult();
        string fileURl;

        //Variables para libros
        private string _libroFunc;
        private string _libroIFRS;
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.ac;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.acSaldos).ToString());

            #region Configurar Opciones

            //Carga el libro por defecto
            this._report = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

            //Carga los libros necesarios para mostrar
            this._libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();
            this._libroIFRS = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS).ToString();

            //Llena la lista de tipo libro
            List<ReportParameterListItem> tipoRompimiento = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = this._libroFunc, Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LibroLocal) },
                            new ReportParameterListItem() { Key = this._libroIFRS, Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LibroIFRS) } };

            //Llena la lista de tipo moneda
            List<ReportParameterListItem> tipoMoneda = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) },
                            new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) }};


            this.AddList("libro", (AppForms.ReportForm).ToString() + "_Libro", tipoRompimiento, true, "1");
            this.AddList("moneda", (AppForms.ReportForm).ToString() + "_Moneda", tipoMoneda, true, "1");
            this.AddTextBox("plaque", false, (AppForms.ReportForm).ToString() + "_plaqueta", true);
            this.AddTextBox("serial", false, (AppForms.ReportForm).ToString() + "_serial", true);
            this.AddMaster("refe", AppMasters.inReferencia, true, null, true);
            this.AddMaster("clase", AppMasters.acClase, true, null, true);
            this.AddMaster("tipo", AppMasters.acTipo, true, null, true);
            this.AddMaster("grupo", AppMasters.acGrupo, true, null, true);
            this.AddMaster("propietario", AppMasters.coTercero, false, null, true);

            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
            #endregion
        }

        /// <summary>
        /// Form Constructer (for Libro Diario Report)
        /// </summary>
        public ParametersReporteActivosSaldos()
        {
            this.Module = ModulesPrefix.ac;
            this.ReportForm = AppReportParametersForm.acSaldos;
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
            this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            //string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            //this._fechaFin = Convert.ToDateTime(fechaFinString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
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

        protected override void ListValueChanged(string option, object sender)
        {
            try
            {

                //Instancia los controles para utilizarlos
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listLibro = (ReportParameterList)this.RPControls["libro"];
                ReportParameterList listMoneda = (ReportParameterList)this.RPControls["moneda"];
                ReportParameterTextBox tipoPlaqueta = new ReportParameterTextBox();
                ReportParameterTextBox tipoSerial = new ReportParameterTextBox();

                #region Opcion Libros

                if (option.Equals("libro"))
                    this._report = listLibro.SelectedListItem.Key;

                #endregion

                #region Opcion 2 Combo Tipo Moneda

                if (option.Equals("moneda"))
                    this._tipoMoneda = listMoneda.SelectedListItem.Key;

                #endregion

                #region Filtro Plaqueta

                if (option.Equals("plaque"))
                    this._plaqueta = tipoPlaqueta.txtFrom.Text;
                #endregion

                #region Filtro Serial

                if (option.Equals("serial"))
                    this._serial = tipoSerial.txtFrom.Text;

                #endregion

                #region Filtro Referencia

                if (option.Equals("refe"))
                {
                    uc_MasterFind masterReferencia = (uc_MasterFind)sender;
                    this._referencia = masterReferencia.ValidID ? masterReferencia.Value : string.Empty;
                }

                #endregion

                #region Filtro Clase

                if (option.Equals("clase"))
                {
                    uc_MasterFind masterClase = (uc_MasterFind)sender;
                    this._Clase = masterClase.ValidID ? masterClase.Value : string.Empty;
                }

                #endregion

                #region Filtro Tipo

                if (option.Equals("tipo"))
                {
                    uc_MasterFind masterTipo = (uc_MasterFind)sender;
                    this._tipo = masterTipo.ValidID ? masterTipo.Value : string.Empty;
                }

                #endregion

                #region Filtro Grupo

                if (option.Equals("grupo"))
                {
                    uc_MasterFind masterGruposActi = (uc_MasterFind)sender;
                    this._Grupos = masterGruposActi.ValidID ? masterGruposActi.Value : string.Empty;
                }

                #endregion

                #region Filtro Propietario

                if (option.Equals("grupo"))
                {
                    uc_MasterFind masterPropietario = (uc_MasterFind)sender;
                    this._propietarios = masterPropietario.ValidID ? masterPropietario.Value : string.Empty;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteActivosSaldos.cs", "ListValueChanged"));
                throw;
            }
        }
        #endregion

    }
}
