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
    public class Reports_VentaCartera : ReportParametersForm
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
                ReportParameterTextBox tipoLibranza = (ReportParameterTextBox)this.RPControls["libranza"];
                ReportParameterTextBox Oferta = (ReportParameterTextBox)this.RPControls["oferta"];

                switch (this._reporte)
                {
                    case "Resumido":
                        reportName = this._bc.AdministrationModel.ReportesCartera_VentaCartera(this._fechaIni, this._fechaFin, this._Comprador, Oferta.txtFrom.Text, tipoLibranza.txtFrom.Text, true, this._formatType);
                        Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
                        break;

                    case "Detallado":
                        reportName = this._bc.AdministrationModel.ReportesCartera_VentaCarteraDetallado(this._fechaIni, this._fechaFin, this._Comprador, Oferta.txtFrom.Text, tipoLibranza.txtFrom.Text, false, this._formatType);
                        Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_VentaCartera.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        /// <summary>
        /// Variable para iniciar el reporte
        /// </summary>

        private string _reporte = "Resumido";
        private string _Comprador= "";
        private string _Asesor = "";

        //Varibles Reporte
        DateTime _fechaIni;
        DateTime _fechaFin;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Auxiliar Report)
        /// </summary>
        public Reports_VentaCartera()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccVentaCartera;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.CcVentaCartera).ToString());

            #region Lista q carga el combo del Reporte
            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>() 
                {
                  new ReportParameterListItem() {Key ="Resumido", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido)},
                  new ReportParameterListItem() {Key = "Detallado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)}
                };

            #endregion

            #region Configuracion de Opciones

            this.AddList("reporte", (AppForms.ReportForm).ToString() + "_Tipo", tipoReporte, true, "Resumido");
            this.AddMaster("comprador", AppMasters.ccCompradorCartera, true, null, true);
            this.AddTextBox("oferta", false, (AppForms.ReportForm).ToString() + "_oferta", true);
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", true);

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
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;
            this.periodoFilter1.txtYear1.Visible = false;

            //this.periodoFilter1.Year[0].ToString();
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

            ReportParameterList listReportType = (ReportParameterList)this.RPControls["reporte"];

            #region Carga el tipo de reporte a mostrar
            if (option.Equals("reporte"))
                this._reporte = listReportType.SelectedListItem.Key;
            #endregion

            #region Carga el Comprador por el que se quiere filtrar
            if (option.Equals("comprador"))
            {
                uc_MasterFind masterComprador = (uc_MasterFind)sender;
                this._Comprador = masterComprador.ValidID ? masterComprador.Value : string.Empty;
            }
            #endregion

        }
        #endregion

    }
}
