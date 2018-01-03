using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.Cliente.GUI.WinApp.Clases;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteLibroMayor : ReportParametersForm
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
                switch (this._tipoReport)
                {
                    case "reporte":
                        #region Imprime el rerpote en .PDF

                        reportName = this._bc.AdministrationModel.ReportesContabilidad_LibroMayor(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance, this._formatType);/*, this._cuentaIni,
                                            this._cuentaFin, this._formatType);*/
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);

                        #endregion
                        break;

                    default:
                        #region Imprime el reporte de Libro mayor en Excel

                        reportName = this._bc.AdministrationModel.ReportesContabilidad_PlantillaExcelLibroMayor(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance);
                            /*this._cuentaIni, this._cuentaFin);*/

                        if (reportName != string.Empty)
                        {
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        else
                            MessageBox.Show(this._errorConsuta);

                        #endregion
                        break;
                }


                //reportName = this._bc.AdministrationModel.ReportesContabilidad_LibroMayor(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance, this._cuentaIni,
                //    this._cuentaFin, this._formatType);
                //fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ParametersReporteLibroMayor.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variable

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        public string tipoBalance;
        public string _cuentaIni = string.Empty;
        public string _cuentaFin = string.Empty;
        public string _tipoReport = "reporte";

        //Varibale para mensaje
        private string _errorConsuta;

        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coLibroMayor).ToString());
            this._errorConsuta = _bc.GetResourceError(DictionaryMessages.Rpt_gl_NoSeGeneranDatos);

            #region Configurar Opciones
            this.tipoBalance = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

            //this.AddCheck("1", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkCtasCasaMatriz"));

            List<ReportParameterListItem> tipoReport = new List<ReportParameterListItem>()
            {
                 new ReportParameterListItem() { Key = "reporte", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) }
            };

            this.AddList("tipoReporte", (AppForms.ReportForm).ToString() + "_tipoReport", tipoReport, true, "reporte");
            this.AddList("2", ReportParameterListSource.TipoBalance, true, _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString());
            //this.AddCheck("Cuentas", (AppForms.ReportForm).ToString() + "_RangoCuenta", true);
            //this.AddMaster("cuentaIni", AppMasters.coPlanCuenta, true, null, false);
            //this.AddMaster("cuentaFin", AppMasters.coPlanCuenta, true, null, false);

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
        /// Form Constructer (for Libro Mayor Report)
        /// </summary>
        public ParametersReporteLibroMayor()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coLibroMayor;
            this.ReportForm = AppReportParametersForm.coLibroMayor;
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
            #region Tipo de reporte a mostrar

            if (option.Equals("tipoReporte"))
            {
                ReportParameterList typeReport = (ReportParameterList)this.RPControls["tipoReporte"];
                this._tipoReport = typeReport.SelectedListItem.Key;
            }

            #endregion

            #region Carga el lirbo al que se desea consultar
            if (option.Equals("2"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];

                this.tipoBalance = listReportType.SelectedListItem.Key;
            }
            #endregion
            #region Carga el rango de cuentas

            //if (option.Equals("Cuentas"))
            //{

            //    Dictionary<string, string[]> rangoCuentas = this.GetValues();
            //    uc_MasterFind masterCuentaIni = (uc_MasterFind)this.RPControls["cuentaIni"];
            //    uc_MasterFind masterCuentaFin = (uc_MasterFind)this.RPControls["cuentaFin"];

            //    switch (rangoCuentas["Cuentas"][0])
            //    {
            //        case ("True"):
            //            //Habilita los controles para filtrar las cuentas
            //            masterCuentaIni.Visible = true;
            //            masterCuentaFin.Visible = true;
            //            break;

            //        case ("False"):
            //            //Deshabilita los controles para filtro de cuentas
            //            masterCuentaIni.Visible = false;
            //            masterCuentaFin.Visible = false;

            //            //Limpia los controles
            //            masterCuentaIni.Value = string.Empty;
            //            masterCuentaFin.Value = string.Empty;
            //            this._cuentaIni = string.Empty;
            //            this._cuentaFin = string.Empty;
            //            break;
            //    }
            //}

            #endregion
            #region Cuenta Inicial
            //if (option.Equals("cuentaIni"))
            //{
            //    uc_MasterFind masterIncial = (uc_MasterFind)sender;
            //    this._cuentaIni = masterIncial.ValidID ? masterIncial.Value : string.Empty;
            //}

            #endregion
            #region Cuenta Final

            //if (option.Equals("cuentaFin"))
            //{
            //    uc_MasterFind masterCuentaFinal = (uc_MasterFind)sender;
            //    this._cuentaFin = masterCuentaFinal.ValidID ? masterCuentaFinal.Value : string.Empty;
            //}

            #endregion
        }

        #endregion
    }
}
