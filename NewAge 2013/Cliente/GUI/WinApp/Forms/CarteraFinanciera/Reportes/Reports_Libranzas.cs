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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_Libranzas : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string fileURl;

                ReportParameterTextBox numeroLibranza = (ReportParameterTextBox)this.RPControls["libranza"];
                this.result = this._bc.AdministrationModel.ReportesCartera_Cc_Libranzas(this._fechaIni, this._fechaFin, this.cliente, numeroLibranza.txtFrom.Text, this._Asesor, this._Pagaduria, this._formatType);

                #region Generacion del reporte

                if (this.result.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, this.result.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }
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

        DTO_TxResult result = new DTO_TxResult();
        private string cliente = "";
        private string _Reporte = "Referenciacion";
        private string _Asesor = "";
        private string _Pagaduria = "";
        private bool _Excel = false;
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccLibranzas).ToString());

            #region Configurar Opciones
            this.AddMaster("masterCliente", AppMasters.ccCliente, true, null);
            this.AddMaster("Asesor", AppMasters.ccAsesor, true, null, true);
            this.AddMaster("Pagaduria", AppMasters.ccPagaduria, true, null, true);
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", true);
            this.AddCheck("Excel", (AppForms.ReportForm).ToString() + "_Exportar", true);
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
        /// Form Constructer
        /// </summary>
        public Reports_Libranzas()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccCreditos;
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
            #region Carga el filtro del cliente
            Dictionary<string, string[]> reportParameters = this.GetValues();
            uc_MasterFind masterAsesor = (uc_MasterFind)this.RPControls["Asesor"];
            uc_MasterFind masterPagaduria = (uc_MasterFind)this.RPControls["Pagaduria"];
            CheckEdit Excel = (CheckEdit)this.RPControls["Excel"];

            if (option.Equals("masterCliente"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this.cliente = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }

            if (option.Equals("Asesor"))
            {
                uc_MasterFind Asesor = (uc_MasterFind)sender;
                this._Asesor = Asesor.ValidID ? Asesor.Value : string.Empty;
            }

            if (option.Equals("Pagaduria"))
            {
                uc_MasterFind Pagaduria = (uc_MasterFind)sender;
                this._Pagaduria = Pagaduria.ValidID ? Pagaduria.Value : string.Empty;
            }
            if (option.Equals("Excel"))
            {
                switch (reportParameters["Excel"][0])
                {
                    case "True":
                        this._Excel = true;
                        break;
                    case "False":
                        this._Excel = false;
                        break;
                }
            }
            #endregion
        }
        #endregion
    }
}
