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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_Ejecutado : ReportParametersForm, IFiltrable
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
                    reportName = this._bc.AdministrationModel.Reporte_TsEjecutado(this._fechaIni,this._report);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Ejecutado.cs-", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        private string _report = "1";
        //Variables del hilo
        DateTime _fechaIni;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Ejecutado() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.ts;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.tsEjecutado).ToString());


            #region Configurar Opciones

            List<ReportParameterListItem> reporte = new List<ReportParameterListItem>()
            {
                 new ReportParameterListItem() { Key= "Detallado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)},
                 new ReportParameterListItem() { Key= "Plantilla", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Plantilla)}
            };

            this.AddList("TipoReporte", (AppForms.ReportForm).ToString() + "_tipoReport", reporte, true, "Detallado");

            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

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

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            
      
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

            #region Carga el El tipo de reporte a mostrar

            if (option.Equals("TipoReporte"))
            {
                ReportParameterList tiporeporte = (ReportParameterList)this.RPControls["TipoReporte"];
                this._report = tiporeporte.SelectedListItem.Key;
            }

            #endregion
        }
        #endregion

    }
}
