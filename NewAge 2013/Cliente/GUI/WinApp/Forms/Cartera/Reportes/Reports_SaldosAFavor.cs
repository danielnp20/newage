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
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_SaldosAFavor : ReportParametersForm, IFiltrable
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

                if (this._tipoReport != 1)
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, this._tipoReport, this._fechaIni, this._fechaFin, this._cliente, null, string.Empty, string.Empty,
                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, this._pendienteInd);

                }
                else
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_SaldosAFavor(this._fechaIni, this._cliente,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty, true, this._formatType);
                               
                }




                
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());

                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_SaldosAFavor.cs-LoadReportMethod"));
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
        private byte _tipoReport = 1;

        //Cariables para filtros
        private string _cliente = "";
        private string _libranza = "";
        private bool _pendienteInd = false;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_SaldosAFavor()
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
            this.documentReportID = AppReports.ccSaldosAFavor;
            #region Configurar Opciones

            #region Carga las lista que se van a mostrar en los combos

            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SaldosCartera) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoReintegros_1) },
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoReintegros_2) }
            };
          
            #endregion

            //Inicializa los Controles del Fomulario
            this.AddList("tipoReport", (AppForms.ReportForm) + "_Tipo", reportType, true, "1");
            this.AddMaster("Cliente", AppMasters.ccCliente, true, null);
            this.AddCheck("Pendientes", "Pendientes", false);

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
            this.btnExportToXLS.Visible = true;

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

                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
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

                if (this._tipoReport != 1)
                {
                    this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._tipoReport, this._fechaIni, this._fechaFin, this._cliente, null, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, this._pendienteInd);

                }
                else
                { 
                
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Saldos", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["tipoReport"];
            uc_MasterFind masterClienteF = (uc_MasterFind)this.RPControls["Cliente"];
            CheckEdit pendientes = (CheckEdit)this.RPControls["Pendientes"];

            this._tipoReport = Convert.ToByte(tipoReporte.SelectedListItem.Key);
            #region Carga el tipo de reporte a mostrar
            
            if (this._tipoReport ==  1)
                pendientes.Visible = false;
            else
                pendientes.Visible = true;

            this._pendienteInd = pendientes.Checked;

            this._cliente = masterClienteF.ValidID ? masterClienteF.Value : string.Empty;

            #endregion
        }
        #endregion

    }
}