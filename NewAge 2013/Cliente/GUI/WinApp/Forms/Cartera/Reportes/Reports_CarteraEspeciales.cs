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
    class Reports_CarteraEspeciales : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = string.Empty;
        public DateTime _fechaIni = DateTime.Now;
        public DateTime _fechaFin = DateTime.Now;
        DateTime _fechaUltimoCierre;
        public byte _report = 1;
        private string _reporte = "1";

        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Funcion que se encarga de Inhabilitar los controles que no son necesarios para este reporte
        /// </summary>
        private void EnableControls()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructor
        /// </summary>
        public Reports_CarteraEspeciales()
        {
            this.Module = ModulesPrefix.cf;
            this.ReportForm = AppReportParametersForm.ccCarteraVarios;
            this.documentReportID = AppReports.ccCarteraEspeciales;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cf;
            this.btnExportToXLS.Visible = false;
            this.btnExportToPDF.Visible = true;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccCarteraEspeciales).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = " " },
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Prejuridico)},
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, "Análisis de Pagos")},
                new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, "Saldo Seguro Vida")},
                new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, "Créditos Cancelados")},
                new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, "Créditos Nuevos")},
                new ReportParameterListItem() { Key = "7", Desc = _bc.GetResource(LanguageTypes.Tables, "Polizas para renovar")},
            };

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("1p", (AppReports.ccCarteraEspeciales).ToString() + "_report", ListReport, true, "1");
            this.AddMaster("2p", AppMasters.ccCliente, true, null, true);

            #endregion

            #region Configurar Filtros del periodo
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.EnableControls();
            #endregion
        }
        #endregion

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
            this._fechaIni = Convert.ToDateTime(fechaIniString);

            ReportParameterList txtlibranzaF = (ReportParameterList)this.RPControls["1p"];
            
            this._reporte = txtlibranzaF.SelectedListItem.Key;
            if (this._reporte.Equals("7") )
            {
                this._fechaIni = this._fechaUltimoCierre;
                string fechaFinString = this._fechaIni.Year.ToString() + " / " + this._fechaIni.Month.ToString() + " / " + this._fechaIni.Day.ToString();
                this._fechaFin = Convert.ToDateTime(fechaFinString);                
            }
            else if (this._reporte.Equals("5") || this._reporte.Equals("6"))
            {
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];                
                this._fechaFin = Convert.ToDateTime(fechaFinString); 
            }
            else
                this._fechaFin = Convert.ToDateTime(fechaIniString);
           
            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                    this.periodoFilter1.Enabled = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = false;
            
                    this.periodoFilter1.Year[0].ToString();
                    string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];

                    this._fechaIni = Convert.ToDateTime(fechaIniString);
                    ReportParameterList txtlibranzaF = (ReportParameterList)this.RPControls["1p"];
                    this._reporte = txtlibranzaF.SelectedListItem.Key;

                if (this._reporte.Equals("7"))
                {
                    this._fechaIni = this._fechaUltimoCierre;
                    string fechaFinString = this._fechaIni.Year.ToString() + " / " + this._fechaIni.Month.ToString() + " / " + this._fechaIni.Day.ToString();
                    this._fechaFin = Convert.ToDateTime(fechaFinString);
                }
                else if (this._reporte.Equals("5") || this._reporte.Equals("6"))
                {
                    string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                    this._fechaFin = Convert.ToDateTime(fechaFinString);
                }
                else
                    this._fechaFin = Convert.ToDateTime(fechaIniString);


                this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(AppReports.ccCarteraEspeciales, this._report, this._fechaIni, this._fechaFin, this._clienteID,null,string.Empty,string.Empty,
                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null);
                if (this._report.Equals(7))
                {
                    this._Query.Columns.Remove("Dias");
                    this._Query.Columns.Remove("Meses");
                }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_PolizaEstado", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            #region Reporte a Imprimir
            ReportParameterList Reporte = (ReportParameterList)this.RPControls["1p"];
            if (option.Equals("1p"))
            {
                this._report = Convert.ToByte(Reporte.SelectedListItem.Key);
                if (this._report == 4)
                {
                    this.btnExportToXLS.Visible = true;
                    this.btnExportToPDF.Visible = false;
                }
                else if (this._report == 5 || this._report == 6 || this._report == 7)
                {
                    this.btnExportToXLS.Visible = true;
                    this.btnExportToPDF.Visible = true;
                }
                else
                {
                    this.btnExportToXLS.Visible = false;
                    this.btnExportToPDF.Visible = true;
                }                
            }
            #endregion

            #region Seleccionar Cliente
            if (option.Equals("2p"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._clienteID = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
            #endregion

            this._reporte = Reporte.SelectedListItem.Key;
            this.periodoFilter1.Visible = true;
            if (this._reporte.Equals("7"))
            {
                #region Configurar Filtros
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                this.periodoFilter1.Visible = false;                
                this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB.SelectedIndex = 0;
                #endregion

                #region Obtiene ultima fecha de corte
                var ultimoCierre = this._bc.AdministrationModel.ccCierreDiaCartera_GetUltimoDiaCierre(string.Empty, null);
                if (ultimoCierre != null)
                    this._fechaUltimoCierre = ultimoCierre.Fecha.Value.Value;
                else
                    this._fechaUltimoCierre = this.periodo;
                #endregion
            }
            else if (this._reporte.Equals("5") ||this._reporte.Equals("6") )
            {
                #region Asigna Fecha de Ultimo Cierre
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB.Items.Clear();
                this.periodoFilter1.monthCB1.Items.Clear();
                this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB.SelectedItem = this.periodo.Date.Month;
                this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB1.SelectedItem = this.periodo.Date.Month;                
                #endregion
            }            
            else
            {
                #region Asigna Fechas
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB1.SelectedItem = 1;
                this.periodoFilter1.monthCB.SelectedIndex = 0;
                #endregion
            }

        }

        #endregion

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName = string.Empty;
                reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID,this._report,this._fechaIni,this._fechaFin,this._clienteID,null,string.Empty,string.Empty,
                                                                                       string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,null,null,null);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CarteraEspeciales.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

    }
}
