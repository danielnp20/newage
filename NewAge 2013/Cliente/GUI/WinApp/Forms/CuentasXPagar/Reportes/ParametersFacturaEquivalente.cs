using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Windows.Forms;
using System.Diagnostics;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersFacturaEquivalente : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName = string.Empty;

                if (this._tercero != string.Empty)
                {
                    DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._tercero, true);
                    DTO_coRegimenFiscal regimenFiscal = (DTO_coRegimenFiscal)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, tercero.ReferenciaID.ToString(), true);

                    if (!regimenFiscal.FactEquivalenteInd.Value.Value)
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_Cp_RegimenFiscalNogeneraFactEquivalente);
                        MessageBox.Show(string.Format(msg, this._tercero));
                    }
                    else
                    {
                        reportName = this._bc.AdministrationModel.Reportes_Cp_FacturaEquivalente(this._fechaIni, this._tercero, true, this._formatType);

                        if(reportName != string.Empty)
                            Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
                        else
                        {
                            string msg1 = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_Cp_NogeneraFactEquivalente);
                            MessageBox.Show(string.Format(msg1, this._tercero));
                        }
                    }
                }

                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_Cp_sinTercero));
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variables para el hilo
        DateTime _fechaIni;

        //Varible para reporte
        private string _tercero = "";

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Inicializa la pantalla con los controles
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cp;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturaEquivalente).ToString());

            #region Configurar Opciones

            this.AddMaster("masterCliente", AppMasters.coTercero, true, null);

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

            #endregion
        }

        /// <summary>
        /// Form Constructer (for Factura Equivalente Report)
        /// </summary>
        public ParametersFacturaEquivalente()
        {
            this.Module = ModulesPrefix.cp;
            this.ReportForm = AppReportParametersForm.faFacturaEquivalente;
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
            this._fechaIni = Convert.ToDateTime(fechaIniString);

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
            #region Opcion Tercero Filtro Bodega
            if (option.Equals("masterCliente"))
            {
                uc_MasterFind masterTercero = (uc_MasterFind)sender;
                this._tercero = masterTercero.ValidID ? masterTercero.Value : string.Empty;
            }
            #endregion
        }

        #endregion
    }
}

