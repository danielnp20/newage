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
    public class Reports_Solicitudes : ReportParametersForm
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

                reportName = this._bc.AdministrationModel.ReportesCartera_Cc_Solicitudes(this._fechaIni, this._fechaFin, this._Cliente, tipoLibranza.txtFrom.Text, this._Asesor,
                    this._estado, this._formatType);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Solicitudes.cs-LoadReportMethod"));
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
        private string _Cliente = "";
        private string _Asesor = "";
        private string _estado = "Todos";
        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;

        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Form Constructer (for Auxiliar Report)
        /// </summary>
        public Reports_Solicitudes()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccSolicitudes;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccSolicitudes).ToString());

            #region Lista de campos para llenar los combos
            //Carga el combo del estado de los reportes
            List<ReportParameterListItem> estadoReporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() {Key = "Todos", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_All)}, 
                new ReportParameterListItem() {Key = "sinAprobar", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateSinAprobar)},
                new ReportParameterListItem() {Key = "Aprobadas", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado)},
                new ReportParameterListItem() {Key = "Cerrada", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCerrado)},
                new ReportParameterListItem() {Key = "Anuladas", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Annulled)},
            };
            #endregion

            #region Configuracion de Opciones

            this.AddList("estado", (AppForms.ReportForm) + "_Estado", estadoReporte, true, "Todos");
            this.AddMaster("cliente", AppMasters.ccCliente, true, null, true);
            this.AddMaster("Asesor", AppMasters.ccAsesor, true, null, true);
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
            ReportParameterList tipoEstado = (ReportParameterList)this.RPControls["estado"];
            #region Carga la opcion del estado que se quiere verificar en el reporte

            if (option.Equals("estado"))
                this._estado = tipoEstado.SelectedListItem.Key;

            #endregion

            #region Carga el cliente por el que se quiere filtrar
            if (option.Equals("cliente"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._Cliente = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
            #endregion

            #region Carga El asesor por el cual se quiere filtrar
            if (option.Equals("Asesor"))
            {
                uc_MasterFind masterAsesor = (uc_MasterFind)sender;
                this._Asesor = masterAsesor.ValidID ? masterAsesor.Value : string.Empty;
            }
            #endregion
        }
        #endregion

    }
}
