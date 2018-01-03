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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersReporteRelacionActivos : ReportParametersForm
    {

        #region Hilos
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;

                reportName = this._bc.AdministrationModel.ReportesActivos_ComparacionLibros(this._fechaIni.Year, this._fechaIni.Month, this._clase, this._Tipo,
                            this._Grupo, this._CentroCosto, this._LogFisica, this._Proyecto,  this._formatType);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);

                //switch (this._tipoMoneda)
                //{
                //    case ("1"):
                //        reportName = this._bc.AdministrationModel.ReportesActivos_SaldosML(this._report, this._fechaIni.Year, this._fechaIni.Month, this._clase, this._Tipo,
                //            this._Grupo, this._CentroCosto, this._LogFisica, this._Proyecto, this.orderby, this._formatType);
                //        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //        Process.Start(fileURl);
                //        break;
                //    case ("2"):
                //        reportName = this._bc.AdministrationModel.ReportesActivos_SaldosME(this._report, this._fechaIni.Year, this._fechaIni.Month, this._clase, this._Tipo,
                //            this._Grupo, this._CentroCosto, this._LogFisica, this._Proyecto, this.orderby, this._formatType);
                //        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //        Process.Start(fileURl);
                //        break;
                //    case ("3"):

                //        reportName = this._bc.AdministrationModel.ReportesActivos_Saldos(this._report, this._fechaIni.Year, this._fechaIni.Month, this._clase, this._Tipo,
                //            this._Grupo, this._CentroCosto, this._LogFisica, this._Proyecto, this.orderby,  this._formatType);
                //        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //        Process.Start(fileURl);

                //        break;
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteRelacionActivos.cs", "LoadReportMethod"));
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

        //private string _report; -------
        //private string _tipoMoneda = "1";
        private string _clase = "";
        private string _Tipo = "";
        private string _Grupo = "";
        private string _CentroCosto = "";
        private string _LogFisica = "";
        private string _Proyecto = "";

        //Carga las variable para el ordenamiento
        Dictionary<int, bool> orderby = new Dictionary<int, bool>();
        
        private bool _componente = false;
        private bool _cuenta = false;
        private bool _plaqueta = false;
        private bool _IdentificadorTR = false;
        private bool _Saldo = false;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;


        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.ac;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.acRelacionActivos).ToString());

            #region Configurar Opciones
           // _report = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

            //Llena la lista de tipo libro
            //List<ReportParameterListItem> tipoRompimiento = new List<ReportParameterListItem>(){
            //                new ReportParameterListItem() { Key = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString(),
            //                    Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LibroLocal) },
            //                new ReportParameterListItem() { Key = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS).ToString(),
            //                    Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LibroIFRS) } };

            //Llena la lista de tipo moneda
            List<ReportParameterListItem> tipoMoneda = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) },
                            new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) }};

            this.orderby.Add(1, false);
            this.orderby.Add(2, false);
            this.orderby.Add(3, false);
            this.orderby.Add(4, false);

            //this.AddList("1", (AppForms.ReportForm).ToString() + "_Libro", tipoRompimiento, true, "1");
            //this.AddList("2", (AppForms.ReportForm).ToString() + "_Moneda", tipoMoneda, true, "1");
            this.AddCheck("9", "Ordenar por Componente");
            this.AddCheck("10", "Ordenar por Cuenta");
            this.AddCheck("11", "Ordenar por plaqueta");
            this.AddCheck("12", "Ordenar por IdentificadorTR");
            this.AddCheck("13", "Ordenar por Saldo");
            this.AddMaster("3", AppMasters.acClase, true, null, true);
            this.AddMaster("4", AppMasters.acTipo, true, null, true);
            this.AddMaster("5", AppMasters.acGrupo, true, null, true);
            this.AddMaster("6", AppMasters.coCentroCosto, true, null, true);
            this.AddMaster("7", AppMasters.glLocFisica, true, null, true);
            this.AddMaster("8", AppMasters.coProyecto, true, null, true);

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
        public ParametersReporteRelacionActivos()
        {
            this.Module = ModulesPrefix.ac;
            this.ReportForm = AppReportParametersForm.acRelacionActivos;
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
            Dictionary<string, string[]> reportParameters = this.GetValues();

            #region Opcion 1

            //if (option.Equals("1"))
            //{
            //    ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

            //    this._report = listReportType.SelectedListItem.Key;
            //}

            #endregion

            #region Opcion 2 Combo Tipo Moneda

            //if (option.Equals("2"))
            //{
            //    ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];

            //    this._tipoMoneda = listReportType.SelectedListItem.Key;
            //}

            #endregion

            #region Filtro Clase

            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._Grupo = master.ValidID ? master.Value : string.Empty;

            }

            #endregion

            #region Filtro Tipo

            if (option.Equals("4"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._Tipo = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Filtro Grupo

            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._Grupo = master.ValidID ? master.Value : string.Empty;

            }

            #endregion

            #region Filtro CentroCosto

            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._CentroCosto = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Filtro Log Fisica

            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._LogFisica = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Filtro Clase

            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._Proyecto = master.ValidID ? master.Value : string.Empty;

            }

            #endregion

            #region Ordenar por Componente
            if (option.Equals("9"))
            {
                switch (reportParameters["9"][0])
                {
                    case "True":
                        this._componente = true;
                        break;
                    case "False":
                        this._componente = false;
                        break;
                }
            } 
            #endregion

            #region Ordenar por Cuenta
            if (option.Equals("10"))
            {
                switch (reportParameters["10"][0])
                {
                    case "True":
                        this._cuenta = true;
                        break;
                    case "False":
                        this._cuenta = false;
                        break;
                }
            }
            #endregion

            #region Ordenar por Plaqueta
            if (option.Equals("11"))
            {
                switch (reportParameters["11"][0])
                {
                    case "True":
                        this._plaqueta = true;
                        break;
                    case "False":
                        this._plaqueta = false;
                        break;
                }
            }
            #endregion

            #region Ordenar por IdentificadorTR
            if (option.Equals("12"))
            {
                switch (reportParameters["12"][0])
                {
                    case "True":
                        this._IdentificadorTR = true;
                        break;
                    case "False":
                        this._IdentificadorTR = false;
                        break;
                }
            }
            #endregion

            #region Ordenar por Saldo
            if (option.Equals("13"))
            {
                switch (reportParameters["13"][0])
                {
                    case "True":
                        this._Saldo = true;
                        break;
                    case "False":
                        this._Saldo = false;
                        break;
                }
            }
            #endregion

            #region LLena el Diccionario con el ordenamiento
            orderby = new Dictionary<int, bool>();
            orderby.Add(1, this._componente);
            orderby.Add(2, this._cuenta);
            orderby.Add(3, this._plaqueta);
            orderby.Add(4, this._IdentificadorTR);
            orderby.Add(5, this._Saldo); 
            #endregion

        }
        #endregion

    }
}
