using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;
using System.Data;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_SolicitudTrabajo : ReportParametersForm
    {
        #region Variables
        private byte _agrupam = 0;
        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        private int _documentoID = 0;
        //Filtro
        private string _filtro;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_SolicitudTrabajo() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            try
            {
                this.Module = ModulesPrefix.py;
                this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.pySolicitudProyecto).ToString());
                this._documentoID = AppReports.pySolicitudProyecto;
                #region Configurar Opciones

                List<ReportParameterListItem> agrupamiento = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = "Gestión Comercial"}, 
                    new ReportParameterListItem() { Key = "2", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)},
                    new ReportParameterListItem() { Key = "3", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)}

                };
                this.AddList("Agrupamiento", "Tipo Reporte", agrupamiento, true, "1");

                #endregion

                #region Configurar Filtros
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB.SelectedIndex = 0;
                this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB1.SelectedIndex = 0;

                this.btnFilter.Enabled = true;
                this.btnResetFilter.Enabled = true;
                this.btnFilter.Visible = true;
                this.btnResetFilter.Visible = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Report_SolicitudTrabajo.cs", "InitReport: " + ex.Message));
            }
            
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

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " /1";
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1] + " /1";
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);
                this._fechaFin = new DateTime(this._fechaFin .Year, this._fechaFin .Month,DateTime.DaysInMonth(this._fechaFin.Year, this._fechaFin .Month));

                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["Agrupamiento"];
                this._agrupam = Convert.ToByte(listReportType.SelectedListItem.Key);

                string fileURl;
                string reportName = this._bc.AdministrationModel.Reportes_py_PlaneacionCostos(null, false, this._agrupam, this._fechaIni, this._fechaFin);

                if (this._agrupam != 1)
                {
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }

                //Thread process = new Thread(this.LoadReportMethod_PDF);
                //process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verifique que la fecha inicial y fecha final existan y tengan el formato correcto");
                this._bc.GetResourceForException(ex, "WinApp-Report_SolicitudTrabajo.cs", "InitReport: " + ex.Message);

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
            Dictionary<string, string[]> reportParameters = this.GetValues();
            ReportParameterList listReportType = (ReportParameterList)this.RPControls["Agrupamiento"];
            this._agrupam = Convert.ToByte(listReportType.SelectedListItem.Key);        
        }

        #endregion
    }
}
