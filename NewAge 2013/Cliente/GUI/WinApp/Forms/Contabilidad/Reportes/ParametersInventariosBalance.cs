using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersInventariosBalance : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            string reportName;
            string fileURl;

            try
            {
                if (this._report == "1")
                {
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_InventariosBalance
                        (this._fechaIni.Month,this._fechaFin.Month, this._Libro, this._cuentaIni,this._cuentaFin,this._año, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }

                if (this._report == "2")
                {
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_InventariosBalanceSinSaldo
                        (this._fechaIni.Month, this._fechaFin.Month, this._Libro, this._cuentaIni, this._cuentaFin, this._año, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ParametersInventariosBalance.cs-LoadReportMethod"));
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
        private string _Libro;
        private int _año = 0;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        public string tipoBalance = null;
        public string _cuentaIni = string.Empty;
        public string _cuentaFin = string.Empty;

        //Varibale para mensaje
        private string _errorConsuta;

        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coInventariosBalance;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coInventariosBalance).ToString());

            #region Configurar Opciones

            this._Libro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            this._errorConsuta = _bc.GetResourceError(DictionaryMessages.Rpt_gl_NoSeGeneranDatos);

            //Carga el combo de Tipo de Reporte
            List<ReportParameterListItem> itemsInventariosBalance = new List<ReportParameterListItem>()
                        {                            
                            new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ConSaldoInicial) }, 
                            new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SinSaldoInicial) }                        };

            this.AddList("1", (AppForms.ReportForm).ToString() + "_tipoReport", itemsInventariosBalance, true, "1");
            this.AddList("2", ReportParameterListSource.TipoBalance, true, _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString());
            this.AddCheck("Cuentas", (AppForms.ReportForm).ToString() + "_RangoCuenta", true);
            this.AddMaster("cuentaIni", AppMasters.coPlanCuenta, true, null, false);
            this.AddMaster("cuentaFin", AppMasters.coPlanCuenta, true, null, false);

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
            #endregion

        }

        /// <summary>
        /// Form Constructer (for Inventarios y Balance Report)
        /// </summary>
        public ParametersInventariosBalance()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coInventariosBalance;
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
                this.btnExportToXLS.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);
                this._año = Convert.ToInt32(this.periodoFilter1.txtYear.Text);
            }
            catch (Exception)
            {

                throw;
            }

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Collects data for the report from the form
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
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);
                this._año = Convert.ToInt32(this.periodoFilter1.txtYear.Text);

                byte tipoReport = Convert.ToByte(this._report);
                this._Query = this._bc.AdministrationModel.Reportes_Co_ContabilidadToExcel(this.documentReportID, tipoReport, this._fechaIni, this._fechaFin, string.Empty, this._cuentaIni,
                                         string.Empty, string.Empty, string.Empty, this._Libro, string.Empty, string.Empty,this._cuentaFin, null, null);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ParametersInventariosBalance.cs-ReportXLS"));
                throw;
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

        protected override void ListValueChanged(string option, object sender)
        {
            #region Opcion 1 Carga que tipo de reporte se quiere pintar

            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                this._report = listReportType.SelectedListItem.Key;

            }

            #endregion
            #region Opcion 2 Carga el Tipo de Libro

            if (option.Equals("2"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];

                this._Libro = listReportType.SelectedListItem.Key;

            }

            #endregion
            #region Carga el rango de cuentas

            if (option.Equals("Cuentas"))
            {

                Dictionary<string, string[]> rangoCuentas = this.GetValues();
                uc_MasterFind masterCuentaIni = (uc_MasterFind)this.RPControls["cuentaIni"];
                uc_MasterFind masterCuentaFin = (uc_MasterFind)this.RPControls["cuentaFin"];

                switch (rangoCuentas["Cuentas"][0])
                {
                    case ("True"):
                        //Habilita los controles para filtrar las cuentas
                        masterCuentaIni.Visible = true;
                        masterCuentaFin.Visible = true;
                        break;

                    case ("False"):
                        //Deshabilita los controles para filtro de cuentas
                        masterCuentaIni.Visible = false;
                        masterCuentaFin.Visible = false;

                        //Limpia los controles
                        masterCuentaIni.Value = string.Empty;
                        masterCuentaFin.Value = string.Empty;
                        this._cuentaIni = string.Empty;
                        this._cuentaFin = string.Empty;
                        break;
                }
            }

            #endregion
            #region Cuenta Inicial
            if (option.Equals("cuentaIni"))
            {
                uc_MasterFind masterIncial = (uc_MasterFind)sender;
                this._cuentaIni = masterIncial.ValidID ? masterIncial.Value : string.Empty;
            }

            #endregion
            #region Cuenta Final

            if (option.Equals("cuentaFin"))
            {
                uc_MasterFind masterCuentaFinal = (uc_MasterFind)sender;
                this._cuentaFin = masterCuentaFinal.ValidID ? masterCuentaFinal.Value : string.Empty;
            }

            #endregion
        }

        #endregion
    }
}
