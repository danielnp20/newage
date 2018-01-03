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
    public class Reports_Credito : ReportParametersForm
    {
        #region Variables

        /// <summary>
        /// Variable para iniciar el reporte
        /// </summary>
        private string _Credito = string.Empty;
        string _formato = string.Empty;
        //Variables del hilo
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        private int DocumentoID = 0;
        private int _mesIni = 0;
        private int _mesFin = 0;
        private int _año = 0;
        ReportParameterTextBox libranza;

        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Auxiliar Report)
        /// </summary>
        public Reports_Credito()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccCreditos;
            this.DocumentoID = AppReports.ccCreditoReport;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccCreditoReport).ToString());

            #region Lista de campos para llenar los combos
            //Carga el combo del estado de los reportes
            List<ReportParameterListItem> estadoReporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() {Key = "0"  , Desc = null},
                //new ReportParameterListItem() {Key = "11" , Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_DescripcionComponentes)}, 
                new ReportParameterListItem() {Key = "22" , Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_TablaAmortizacion)}, 
                new ReportParameterListItem() {Key = "33" , Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Pagare)},
                new ReportParameterListItem() {Key = "44" , Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoISS)},
            };
            #endregion

            #region Configuracion de Opciones
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", true);
            this.AddList("Crédito", (AppForms.ReportForm) + "_Credito", estadoReporte, true,"0",true);
            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
            this.btnExportToXLS.Visible = true;
            this.btnExportToXLS.Enabled = true;
            this.btnExportToPDF.Visible = false;
            this.btnExportToPDF.Enabled = false;
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
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
            this._mesIni = this.periodoFilter1.Months[0];
            this._mesFin = this.periodoFilter1.Months[1];
            this._año = Convert.ToInt32(this.periodoFilter1.txtYear.Text);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Exportar a Excel
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;

                if(this._formato != "0")
                {
                    this._Query = this._bc.AdministrationModel.Reports_cc_CreditoXLS(this._Credito);
                }
                
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Credito", "Report_XLS"));
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
        /// Valida los filtros seleccionados por el Usiario
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList Crédito = (ReportParameterList)this.RPControls["Crédito"];
            
            //Asignar el formato a imprimir (Excel o PDF)
            this._formato = Crédito.SelectedListItem.Key;

            //Detalle de controles a mostrar
            if (option.Equals("Crédito"))
                this._Credito = Crédito.SelectedListItem.Key;
            
            switch (_Credito)
            {
                case "11":
                    this.btnExportToPDF.Visible = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Visible = true;
                    this.btnExportToXLS.Enabled = true;
                    this.periodoFilter1.txtYear1.Enabled = false;
                    this.periodoFilter1.monthCB.Enabled = false;
                    this.periodoFilter1.monthCB1.Enabled = false;
                    break;
                case "22":
                    this.btnExportToPDF.Visible = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Visible = true;
                    this.btnExportToXLS.Enabled = true;
                    this.periodoFilter1.txtYear1.Enabled = false;
                    this.periodoFilter1.monthCB.Enabled = false;
                    this.periodoFilter1.monthCB1.Enabled = false;
                    break;
                case "33":
                    this.btnExportToPDF.Visible = true;
                    this.btnExportToPDF.Enabled = true;
                    this.btnExportToXLS.Visible = false;
                    this.btnExportToXLS.Enabled = false;
                    this.periodoFilter1.txtYear1.Enabled = true;
                    this.periodoFilter1.monthCB.Enabled = true;
                    this.periodoFilter1.monthCB1.Enabled = true;
                    break;
                case "44":
                    this.btnExportToPDF.Visible = true;
                    this.btnExportToPDF.Enabled = true;
                    this.btnExportToXLS.Visible = false;
                    this.btnExportToXLS.Enabled = false;
                    this.periodoFilter1.txtYear1.Enabled = true;
                    this.periodoFilter1.monthCB.Enabled = true;
                    this.periodoFilter1.monthCB1.Enabled = true;
                    break;
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
                libranza = (ReportParameterTextBox)this.RPControls["libranza"];

                reportName = this._bc.AdministrationModel.Reports_cc_Credito(this._mesIni, this._mesFin,this._año,libranza.txtFrom.Text, this._Credito);

                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Credito.cs-LoadReportMethod"));
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