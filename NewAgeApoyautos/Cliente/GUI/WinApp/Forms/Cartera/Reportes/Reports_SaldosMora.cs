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
    public class Reports_SaldosMora : ReportParametersForm, IFiltrable
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

                if (!this._isMora)
                    reportName = this._bc.AdministrationModel.Report_Cc_SaldosMora(this._fechaIni, this._cliente, this._pagaduria, this._lineaCredi, this._compradorCatera, 
                        this._asesor, this._plazo,this._TipoCartera, this._formatType);
                else
                    reportName = this._bc.AdministrationModel.Report_Cc_CarteraMora(new DateTime(), this._fechaIni, this._fechaFin, this._compradorCatera, this._oferta, this._libranza, true, this._orden, this._formatType);

                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_SaldosMora.cs-LoadReportMethod"));
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
        private string _oferta = "";
        private string _libranza = "";
        private int _plazo = 0;
        private bool _isMora = false;
        private string _orden = "Saldo";
        private string _TipoCartera = "Toda";

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_SaldosMora()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccSaldosMora;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.ccReportesCartera).ToString());

            #region Configurar Opciones

            #region Carga las lista que se van a mostrar en los combos

            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> noFondoCajaCompensacionReportType = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosCartera) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosAFavor) },
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosMora) }
            };
            List<ReportParameterListItem> noDetalleNominaReportType2 = new List<ReportParameterListItem>()
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
            this.AddList("1", (AppForms.ReportForm) + "_Tipo", noFondoCajaCompensacionReportType, true, "1");
            this.AddList("TipoCartera", (AppForms.ReportForm) + "_Filtro", noDetalleNominaReportType2, true, "Toda");
            this.AddMaster("cliente", AppMasters.ccCliente, true, null);
            this.AddMaster("pagaduria", AppMasters.ccPagaduria, true, null);
            this.AddMaster("lineaCre", AppMasters.ccLineaCredito, true, null);
            this.AddMaster("compCartera", AppMasters.ccCompradorCartera, true, null);
            this.AddMaster("asesor", AppMasters.ccAsesor, true, null);
            this.AddList("plazo", (AppForms.ReportForm) + "_plazo", plazo, true, "0");
            this.AddList("Orden", (AppForms.ReportForm) + "_Orden", Orden, true, "Saldo");
            this.AddTextBox("oferta", false, (AppForms.ReportForm).ToString() + "_oferta", false);
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", false);

            #endregion);

            #region Configuracion Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Visible = false;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            //List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
            //fieldsNRT.Add(new ConsultasFields("PagaduriaID", "Pagaduria", typeof(string)));
            //mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
            //mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));

            //fieldsNRT.Add(new ConsultasFields("ClienteID", "Cliente", typeof(string)));
            //mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
            //mq.SetFK("ClienteID", AppMasters.ccCliente, _bc.CreateFKConfig(AppMasters.ccCliente));
            ////Inicializacion de Master Find

            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                if (this._isMora)
                {
                    string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
                    this._fechaFin = Convert.ToDateTime(fechaFinString);
                }

                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["1"];
            ReportParameterList tipoCartera = (ReportParameterList)this.RPControls["TipoCartera"];
            ReportParameterList plazo = (ReportParameterList)this.RPControls["plazo"];
            ReportParameterList orden = (ReportParameterList)this.RPControls["Orden"];
            uc_MasterFind masterClienteF = (uc_MasterFind)this.RPControls["cliente"];
            uc_MasterFind masterPagaduriaF = (uc_MasterFind)this.RPControls["pagaduria"];
            uc_MasterFind masterLineaCrediF = (uc_MasterFind)this.RPControls["lineaCre"];
            uc_MasterFind masterCompraCarteF = (uc_MasterFind)this.RPControls["compCartera"];
            uc_MasterFind masterAsesorF = (uc_MasterFind)this.RPControls["asesor"];
            ReportParameterTextBox txtofertaF = (ReportParameterTextBox)this.RPControls["oferta"];
            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["libranza"];

            #region Carga el tipo de reporte a mostrar

            if (option.Equals("1"))
            {
                this._report = tipoReporte.SelectedListItem.Key;
                if (this._report.Equals("3"))
                {
                    tipoCartera.Visible = false;
                    plazo.Visible = false;
                    masterClienteF.Visible = false;
                    masterPagaduriaF.Visible = false;
                    masterLineaCrediF.Visible = false;
                    masterCompraCarteF.Visible = true;
                    masterAsesorF.Visible = false;
                    txtofertaF.Visible = false;
                    txtlibranzaF.Visible = false;
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;
                    this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB1.SelectedIndex = 0;
                    this._isMora = true;
                    orden.Visible = true;
                }
                else
                {
                    tipoCartera.Visible = true;
                    plazo.Visible = true;
                    masterClienteF.Visible = true;
                    masterPagaduriaF.Visible = true;
                    masterLineaCrediF.Visible = true;
                    masterCompraCarteF.Visible = true;
                    masterAsesorF.Visible = true;
                    txtofertaF.Visible = false;
                    txtlibranzaF.Visible = false;
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;
                    this._isMora = false;
                    orden.Visible = false;
                }
            }

            #endregion


            #region Carga el tipo de cartera a mostrar 

            if (option.Equals("TipoCartera"))
                this._TipoCartera = tipoCartera.SelectedListItem.Key;
            #endregion
            #region Carga el filtro por oferta
            if (option.Equals("oferta"))
            {
                ReportParameterTextBox textoferta = (ReportParameterTextBox)sender;
                this._oferta = txtofertaF.txtFrom.Text;
            }
            #endregion
            #region Carga el filtro por Libranza
            if (option.Equals("libranza"))
            {
                ReportParameterTextBox textoferta = (ReportParameterTextBox)sender;
                this._libranza = txtlibranzaF.txtFrom.Text;
            }
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
                this._cliente = masterPagaduria.ValidID ? masterPagaduria.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Linea de Credito

            if (option.Equals("lineaCre"))
            {
                uc_MasterFind masterlineaCre = (uc_MasterFind)sender;
                this._cliente = masterlineaCre.ValidID ? masterlineaCre.Value : string.Empty;
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
                this._cliente = masterasesor.ValidID ? masterasesor.Value : string.Empty;
            }

            #endregion
            #region Carga el estilo de ordenamiento
            if (option.Equals("Orden"))
            {
                this._orden = orden.SelectedListItem.Key;

            }
            #endregion
        }
        #endregion

    }
}