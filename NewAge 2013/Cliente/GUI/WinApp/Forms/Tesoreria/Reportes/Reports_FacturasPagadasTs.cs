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
    class Reports_FacturasPagadasTs : ReportParametersForm, IFiltrable
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
                if (this._report == 1 || this._report == 0)
                {
                    reportName = this._bc.AdministrationModel.Reporte_Cp_FacturasPagadas(this._fechaIni, this._fechaFin, this._filtro, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_FacturasPagadas.cs-", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        private int _report = 0;
        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        //Filtro
        private string _filtro;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_FacturasPagadasTs() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPagadas).ToString());
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> noFondoCajaCompensacionReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "") }, 
                        };
            //Determina el nombre del combo y el item donde debe quedar
            //this.AddList("1", (AppForms.ReportForm).ToString() + "_Tipo", noFondoCajaCompensacionReportType, true, "1");
            this.AddMaster("3", AppMasters.coTercero, true, null);
            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;

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
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

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
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                if (listReportType.SelectedListItem.Key == 1.ToString())
                    this._report = 1;

                if (listReportType.SelectedListItem.Key == 2.ToString())
                    this._report = 2;

                if (listReportType.SelectedListItem.Key == 3.ToString())
                    this._report = 3;

                if (listReportType.SelectedListItem.Key == 4.ToString())
                    this._report = 4;

                if (listReportType.SelectedListItem.Key == 5.ToString())
                    this._report = 5;
            }
        }
        #endregion

        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<SentenceTransformer.ConsultasFields> fields)
        {
            foreach (var fil in consulta.Filtros)
                this._filtro = fil.ValorFiltro;
        }
    }
}
