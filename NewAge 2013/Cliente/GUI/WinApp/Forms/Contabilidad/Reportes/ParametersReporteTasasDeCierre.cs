using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors;
using System.Collections;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteTasasDeCierre : ReportParametersForm
    {

        #region Hilos

        /// <summary>
        /// Hilo, Genera el PDF
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                if (reporte.Equals("DeCierre"))
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_Tasas(this._Periodo, false, this._formatType);
                else
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_Tasas(this._Periodo, true, this._formatType);

                #region Genera el  El reporte

                //Genera el PDF
                if (reportName.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }

                //Verifica q se halla ejecutado con exito la generacion del PDF para mostrarlo al usuario
                if (!string.IsNullOrEmpty(fileURl))
                    Process.Start(fileURl);
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
                    return;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersRerporteTasasDeCierre.cs", "LoadReportMethod_PDF"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variables Genericas
        DTO_TxResult reportName = new DTO_TxResult();
        private string fileURl;

        //Variables para hilo
        private string reporte = "DeCierre";
        DateTime _Periodo;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de desahbilitar los controles que no se utilizan
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
        /// Form Constructer (for Tasas de Cierre Report)
        /// </summary>
        public ParametersReporteTasasDeCierre()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coTasas;
        }

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Funcion que se encarga de inicicar los controles
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coTasasDeCierre).ToString());

            #region Configurar Opciones

            #region Carga los combos respectivamente

            List<ReportParameterListItem> tipoReporteTasas = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "DeCierre", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_DeCierre) },
                new ReportParameterListItem() { Key = "Diarias", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Diarias) } 
            };

            #endregion

            this.AddList("reporte", (AppForms.ReportForm).ToString() + "_tipoReport", tipoReporteTasas, true, "DeCierre");

            #endregion

            #region Configurar Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.EnableControls();

            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.btnExportToPDF.Enabled = false;
            this.EnableControls();

            int año = Convert.ToInt16(this.periodoFilter1.txtYear.Text);
            int mes = 0;
            if (reporte.Equals("DeCierre"))
                mes = this.periodoFilter1.Months[0];
            else
                mes = DateTime.Now.Month;

            _Periodo = new DateTime(año, mes, DateTime.DaysInMonth(año, mes));
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
            if (option.Equals("reporte"))
            {
                ReportParameterList report = (ReportParameterList)this.RPControls["reporte"];
                Dictionary<string, string[]> reportParameters = this.GetValues();

                this.reporte = report.SelectedListItem.Key;

                switch (reportParameters["reporte"][0])
                {
                    case "DeCierre":
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                        if (this.periodoFilter1.monthCB.Items.Count == 0)
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedItem = this.periodo.Date.Month;


                        this.btnFilter.Enabled = true;
                        this.btnResetFilter.Enabled = true;
                        this.btnFilter.Visible = true;
                        this.btnResetFilter.Visible = true;
                        break;
                    case "Diarias":
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;

                        this.btnFilter.Enabled = false;
                        this.btnResetFilter.Enabled = false;
                        this.btnFilter.Visible = false;
                        this.btnResetFilter.Visible = false;
                        break;
                };
            };
        }

        #endregion
    }
}