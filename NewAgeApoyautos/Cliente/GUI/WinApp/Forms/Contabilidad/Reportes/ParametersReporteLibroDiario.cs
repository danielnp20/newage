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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteLibroDiario : ReportParametersForm
    {

        #region Hilos
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;
                if (this._report == "1")
                {
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_LibroDiario(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                if (this._report == "2")
                {
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_LibroDiarioComprobante(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
           }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteLibroDiario.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Variable
        private string _report = "1";
        private string tipoBalance;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        

        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coLibroDiario).ToString());
            #region Configurar Opciones
            //Carga el valor del libro
            this.tipoBalance = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();
             
            this.AddList("1", (AppForms.ReportForm).ToString() + "_lblRompimiento", new List<ReportParameterListItem> { new ReportParameterListItem() 
            { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) }, new ReportParameterListItem() 
            { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Voucher) } }, true, "CuentaID");

            this.AddCheck("2", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkCtasCasaMatriz"));
            this.AddCheck("3", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkLibroDiarioEspecial"));
            this.AddList("4", ReportParameterListSource.TipoBalance, true, _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString());
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
            this.btnExportToXLS.Visible = true;
            #endregion
        }

        /// <summary>
        /// Form Constructer (for Libro Diario Report)
        /// </summary>
        public ParametersReporteLibroDiario()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coLibroDiario;
            this.ReportForm = AppReportParametersForm.coLibroDiario;
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
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                this._Query = this._bc.AdministrationModel.Reportes_Co_ContabilidadToExcel(this.documentReportID, null, this._fechaIni, null, string.Empty, string.Empty,
                                        string.Empty, string.Empty, string.Empty, this.tipoBalance, string.Empty, string.Empty, null, null, null);

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

        protected override void ListValueChanged(string option, object sender)
        {
            #region Opcion 1

            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                this._report = listReportType.SelectedListItem.Key;
            }

            #endregion

            #region Opcion 2 Combo Tipo Balance

            if (option.Equals("4"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["4"];

                this.tipoBalance = listReportType.SelectedListItem.Key;
            }

            #endregion
        }
        #endregion
    }
}
