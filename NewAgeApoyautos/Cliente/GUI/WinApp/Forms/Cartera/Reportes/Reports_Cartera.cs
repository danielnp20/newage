using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_Cartera : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;

                switch (_report)
                {
                    case "1":

                        reportName = this._bc.AdministrationModel.Report_Cc_Saldos(this._fechaIni, this._cliente, this._pagaduria, this._lineaCredi, this._compradorCatera, this._asesor, this._TipoCartera, false, this._formatType);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);

                        break;

                    case "2":
                        reportName = this._bc.AdministrationModel.Report_Cc_SaldosAFavor(this._fechaIni, this._cliente, this._pagaduria, this._lineaCredi, this._compradorCatera, this._asesor, this._TipoCartera, true, this._formatType);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);
                        break;

                    case "3":
                         if (this._isMora)
                            reportName = this._bc.AdministrationModel.Report_Cc_SaldosMora(this._fechaIni, this._cliente, this._pagaduria, this._lineaCredi, this._compradorCatera, 
                                         this._asesor, this._plazo,this._TipoCartera, this._formatType);
                        else
                            reportName = this._bc.AdministrationModel.Report_Cc_CarteraMora(new DateTime(), this._fechaIni, this._fechaFin, this._compradorCatera, this._oferta, this._libranza, true, this._orden, this._formatType);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Cartera.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variable para reporte
        DateTime _fechaIni;
        DateTime _fechaFin;
        private string _report = "1";

        //Cariables para filtros
        private string _cliente = "";
        private string _pagaduria = "";
        private string _lineaCredi = "";
        private string _compradorCatera = "";
        private string _asesor = "";
        private string _TipoCartera = "Toda";

        private string _oferta = "";
        private string _libranza = "";
        private int _plazo = 0;
        private bool _isMora = false;
        private string _orden = "Saldo";
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Cartera() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.documentReportID = AppReports.ccReportesCartera;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.ccReportesCartera).ToString());

            #region Configurar Opciones

            #region Carga las lista que se van a mostrar en los combos

            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tipoReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosCartera) }, 
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosMora) }
            };
            List<ReportParameterListItem> tipoCartera = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "Toda", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CarteraToda) },
                new ReportParameterListItem() { Key = "Propia", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CarteraPropia) }, 
                new ReportParameterListItem() { Key = "Cedida", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CarteraCedida) }       
            };
            List<ReportParameterListItem> plazo = new List<ReportParameterListItem>() {
                new ReportParameterListItem() {Key ="0", Desc = "-"},
                new ReportParameterListItem() {Key ="1", Desc= "12"},
                new ReportParameterListItem() {Key ="2", Desc= "24"},
                new ReportParameterListItem() {Key ="3", Desc= "36"},
                new ReportParameterListItem() {Key ="4", Desc= "48"},
                new ReportParameterListItem() {Key ="5", Desc= "120"},};

            List<ReportParameterListItem> Orden = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "Saldo", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosMora) }, 
                new ReportParameterListItem() { Key = "Libranza", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Libranzas) },
                new ReportParameterListItem() { Key = "Cedula", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Cedula) },
                new ReportParameterListItem() { Key = "Nombre", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Name) }
            };
            #endregion

            //Inicializa los Controles del Fomulario
            this.AddList("TipoReporte", (AppForms.ReportForm) + "_Tipo", tipoReport, true, "1");
            this.AddList("TipoCartera", (AppForms.ReportForm) + "_Filtro", tipoCartera, true, "toda");
            this.AddMaster("cliente", AppMasters.ccCliente, true, null);
            this.AddMaster("pagaduria", AppMasters.ccPagaduria, true, null);
            this.AddMaster("lineaCre", AppMasters.ccLineaCredito, true, null);
            this.AddMaster("compCartera", AppMasters.ccCompradorCartera, true, null);
            this.AddMaster("asesor", AppMasters.ccAsesor, true, null);
            this.AddList("plazo", (AppForms.ReportForm) + "_plazo", plazo, true, "0",false);
            this.AddList("Orden", (AppForms.ReportForm) + "_Orden", Orden, true, "Saldo",false);
            this.AddTextBox("oferta", false, (AppForms.ReportForm).ToString() + "_oferta", false);
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", false);

            #endregion);

            #region Configuracion Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;

            List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
            fieldsNRT.Add(new ConsultasFields("PagaduriaID", "Pagaduria", typeof(string)));
            this.mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
            this.mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));

            fieldsNRT.Add(new ConsultasFields("ClienteID", "Cliente", typeof(string)));
            this.mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
            this.mq.SetFK("ClienteID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
            //Inicializacion de Master Find

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
            this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + DateTime.Today.Day;
            this._fechaIni = Convert.ToDateTime(fechaIniString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["TipoReporte"];
            ReportParameterList tipoCartera = (ReportParameterList)this.RPControls["TipoCartera"];

            #region Carga el tipo de reporte a mostrar

            if (option.Equals("TipoReporte"))
            {
                this._report = tipoReporte.SelectedListItem.Key;
                if (this._report.Equals("3"))
                {
                    tipoCartera.Visible = false;
                    this._isMora = true;
                }
                else
                {
                    tipoCartera.Visible = true;
                    this._isMora = false;
                }
            }
            #endregion
            #region Carga el tipo de cartera a mostrar

            if (option.Equals("TipoCartera"))
                this._TipoCartera = tipoCartera.SelectedListItem.Key;

            #endregion
            #region Carga el filtro por cliente

            if (option.Equals("cliente"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._cliente = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
            #endregion
            #region Carga el filtro por pagaduria

            if (option.Equals("pagaduria"))
            {
                uc_MasterFind masterPagaduria = (uc_MasterFind)sender;
                this._pagaduria = masterPagaduria.ValidID ? masterPagaduria.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Linea de Credito

            if (option.Equals("lineaCre"))
            {
                uc_MasterFind masterlineaCre = (uc_MasterFind)sender;
                this._lineaCredi = masterlineaCre.ValidID ? masterlineaCre.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Comprador de Cartera

            if (option.Equals("compCartera"))
            {
                uc_MasterFind mastercompCartera = (uc_MasterFind)sender;
                this._compradorCatera = mastercompCartera.ValidID ? mastercompCartera.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Asesor

            if (option.Equals("asesor"))
            {
                uc_MasterFind masterasesor = (uc_MasterFind)sender;
                this._asesor = masterasesor.ValidID ? masterasesor.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por oferta
            if (option.Equals("oferta"))
            {
                ReportParameterTextBox textoferta = (ReportParameterTextBox)sender;
                this._oferta = textoferta.txtFrom.Text;
            }
            #endregion
            #region Carga el filtro por Libranza
            if (option.Equals("libranza"))
            {
                ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)sender;
                this._libranza = txtlibranzaF.txtFrom.Text;
            }
            #endregion
            #region Carga el estilo de ordenamiento
            if (option.Equals("Orden"))
            {
                ReportParameterList orden = (ReportParameterList)this.RPControls["Orden"];
                this._orden = orden.SelectedListItem.Key;
            }
            #endregion
        }

        #endregion

    }
}
