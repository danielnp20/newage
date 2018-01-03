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
    class Reports_GestionComercial : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = string.Empty;
        public DateTime _fechaIni = DateTime.Now;
        public DateTime _fechaFin = DateTime.Now;
        DateTime _fechaUltimoCierre;
        public byte _report = 1;
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
        public Reports_GestionComercial()
        {
            this.Module = ModulesPrefix.cf;
            
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {                   
            this.Module = ModulesPrefix.cf;
            this.documentReportID = AppReports.ccActivacionReoperacion;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = true;

            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccCarteraEspeciales).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Reoperaciones" },
                new ReportParameterListItem() { Key = "2", Desc = "Activaciones" },
                new ReportParameterListItem() { Key = "3", Desc = "Reestructuracion con extra" },                
            };
            this.AddList("tipoReporte", "Tipo Reporte", ListReport, true, "1");
            
            #endregion

            #region Obtiene ultima fecha de corte
            var ultimoCierre = this._bc.AdministrationModel.ccCierreDiaCartera_GetUltimoDiaCierre(string.Empty, null);
            if (ultimoCierre != null)
                this._fechaUltimoCierre = ultimoCierre.Fecha.Value.Value;
            else
                this._fechaUltimoCierre = this.periodo;

            #endregion

            #endregion

            #region Configurar Filtros del periodo
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Clear();
            this.periodoFilter1.monthCB1.Items.Clear();
            this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
            this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
            this.periodoFilter1.monthCB1.Items.Add(this._fechaUltimoCierre.Date.Day);
            this.periodoFilter1.monthCB1.SelectedItem = this._fechaUltimoCierre.Date.Day;
            this.EnableControls();
            #endregion


            #region Asigna Fecha de Ultimo Cierre
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
            
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
            this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
            this._fechaIni = fechaIniString;
            this._fechaFin = fechaIniString;

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
                    DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
                    this._fechaIni = fechaIniString;
                    this._fechaFin = fechaIniString;
                    ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                    this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);
                    
                    string reportName;
                    string fileURl;


                    this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(AppReports.ccActivacionReoperacion, this._report, null, this._fechaFin, this._clienteID, null, string.Empty,
                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null,null);


                    //reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(AppReports.ccActivacionReoperacion, this._report, null, this._fechaFin, this._clienteID, null, string.Empty, string.Empty,
                    //                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, this._formatType);


                    //Exporta Reporte a Excel.
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
              ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
              this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);

              if (this._report == 1 || this._report == 2)
              {
                  this.btnExportToPDF.Enabled = true;
                  this.btnExportToXLS.Enabled = true;

              }
              else
              {
                  this.btnExportToPDF.Enabled = true;
                  this.btnExportToXLS.Enabled = false;

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


                ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);
                    
                string reportName = string.Empty;
                reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID,this._report,this._fechaIni,this._fechaFin,this._clienteID,null,string.Empty,string.Empty,
                                                                                       string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,null,null,null);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_GestionComercial.cs-LoadReportMethod"));
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
