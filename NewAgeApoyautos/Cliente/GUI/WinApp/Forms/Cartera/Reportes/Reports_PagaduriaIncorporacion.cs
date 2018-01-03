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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_PagaduriaIncorporacion : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;
                switch (this.report)
                {
                    case ("Detallado"):

                        reportName = this._bc.AdministrationModel.ReportesCartera_Cc_PagaduriaIncorporacion(this._fechaIni, this._fechaFin, pagaduria, this._formatType);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);

                        break;

                    case ("Plantilla"):

                        reportName = this._bc.AdministrationModel.ReportesCartera_Cc_PagaduriaIncorporacionPlantilla(this._fechaIni, this._fechaFin, pagaduria);
                        if (reportName != string.Empty)
                        {
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        else
                        {
                            MessageBox.Show(this._errorConsuta);
                        }

                        break;

                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReportSaldos.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;

        private string report = "Detallado";
        private string pagaduria = "";
        private string _errorConsuta;
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccPagaduriaIncorporaciones).ToString());
            this._errorConsuta = _bc.GetResourceError(DictionaryMessages.Rpt_gl_NoSeGeneranDatos);

            #region Configurar Opciones

            //Carga la lista del combo para ver el tipo de reporte a mostrar
            List<ReportParameterListItem> reporte = new List<ReportParameterListItem>()
            {
                 new ReportParameterListItem() { Key= "Detallado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)},
                 new ReportParameterListItem() { Key= "Plantilla", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Plantilla)}
            };

            this.AddList("TipoReporte", (AppForms.ReportForm).ToString() + "_tipoReport", reporte, true, "Detallado");
            this.AddMaster("masterPagaduria", AppMasters.ccPagaduria, true, null);

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

            #endregion
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public Reports_PagaduriaIncorporacion()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccPagaduriaIncorporaciones;
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
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
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
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {

            #region Carga el El tipo de reporte a mostrar

            if (option.Equals("TipoReporte"))
            {
                ReportParameterList tiporeporte = (ReportParameterList)this.RPControls["TipoReporte"];
                this.report = tiporeporte.SelectedListItem.Key;
            }

            #endregion

            #region Carga el filtro del Pagaduria

            if (option.Equals("masterPagaduria"))
            {
                uc_MasterFind masterPagaduria = (uc_MasterFind)sender;
                this.pagaduria = masterPagaduria.ValidID ? masterPagaduria.Value : string.Empty;
            }

            #endregion
        }
        #endregion
    }
}
