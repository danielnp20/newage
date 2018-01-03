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
    class Reports_Medios : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = string.Empty;
        public DateTime _fechaIni = DateTime.Now;
        public DateTime _fechaFin = DateTime.Now;
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
        public Reports_Medios()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coMediosMagneticos;
            this.documentReportID = AppReports.coMediosMagneticos;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccCarteraEspeciales).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Resumen por Cuentas " },
                new ReportParameterListItem() { Key = "2", Desc = "Resumen por Terceros"},
            };

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("1p", (AppReports.coMediosMagneticos).ToString() + "_report", ListReport, true, "1");
            

            #endregion

            #region Configurar Filtros del periodo
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
            this.EnableControls();
            #endregion
        }
        #endregion



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
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / 1";
                string fechaFinString = this.periodoFilter1.txtYear.Text + " / 12";
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                //Tipo de libro
                string libro = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string terceroEmpresa = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                ReportParameterList txttiporeporte = (ReportParameterList)this.RPControls["1p"];

                this._report = Convert.ToByte(txttiporeporte.SelectedListItem.Key);

                this._Query = this._bc.AdministrationModel.Reportes_Co_ContabilidadToExcel(this.documentReportID, this._report, this._fechaIni, this._fechaFin, terceroEmpresa, string.Empty,
                                         string.Empty, string.Empty, string.Empty, libro, string.Empty, string.Empty, string.Empty, null, null);

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

                    this.btnExportToXLS.Visible = true;
                    this.btnExportToPDF.Visible = false;
            }
            #endregion
        }

        #endregion


    }
}
