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
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_AnalisisPagos : ParametersCarteraForm
    {
        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_AnalisisPagos() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.documentReportID = AppReports.ccRepAnalisisPagos;
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(LanguageTypes.Forms, this.documentReportID.ToString());

            List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = this.Text}, 
                    new ReportParameterListItem() { Key = "2", Desc = "Pagos Cliente"}, 
                    new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Creditos)}, 
                    new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_TotalRecaudos)}, 
                    new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RecaudosMasivos)}, 
                    new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RecaudosManuales)}, 
                    new ReportParameterListItem() { Key = "7", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PagosTotal)}, 
                    new ReportParameterListItem() { Key = "8", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_AjustesCartera)}, 
                    new ReportParameterListItem() { Key = "9", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Refinanciacion)}, 
                };
            this.AddList("TipoReporte", "TipoReporte", reportType, true, "1");
            this.AddMaster("ClienteID", AppMasters.ccCliente, true, null);
            this.AddTextBox("Obligacion", false, "Obligación");
            this.AddCheck("Cuota","Por Cuota");

            #region Configuracion Filtros
            this.btnExportToXLS.Visible = true;
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedItem = this.periodo.Date.Month;
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedItem = this.periodo.Date.Month;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
            uc_MasterFind txtCliente = (uc_MasterFind)this.RPControls["ClienteID"];
            if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
            else
                this._libranza = null; 
            this._cliente = txtCliente.Value;
            if (!string.IsNullOrEmpty(this._cliente) || !string.IsNullOrEmpty(this._libranza.ToString()))
            {
                #region Valida fechas
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.monthCB1.SelectedItem;
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                if (!string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) && !string.IsNullOrEmpty(this.periodoFilter1.monthCB.SelectedItem.ToString()))
                {
                    string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.SelectedItem;
                    this._fechaIni = Convert.ToDateTime(fechaIniString);
                }
                if (this._fechaIni != null)
                {
                    if (this._fechaIni > this._fechaFin)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DateInvalid));
                        return;
                    }
                }
                #endregion                
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_DigitarCliente));
            }
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                if (!string.IsNullOrEmpty(this._cliente) || !string.IsNullOrEmpty(this._libranza.ToString()))
                {
                    this.periodoFilter1.Enabled = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = false;

                    #region Valida fechas
                    string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.monthCB1.SelectedItem;
                    this._fechaFin = Convert.ToDateTime(fechaFinString);

                    if (!string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) && !string.IsNullOrEmpty(this.periodoFilter1.monthCB.SelectedItem.ToString()))
                    {
                        string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.SelectedItem;
                        this._fechaIni = Convert.ToDateTime(fechaIniString);
                    }
                    if (this._fechaIni != null)
                    {
                        if (this._fechaIni > this._fechaFin)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DateInvalid));
                            return;
                        }
                    }
                    #endregion
                    #region Carga filtros
                    ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
                    uc_MasterFind txtCliente = (uc_MasterFind)this.RPControls["ClienteID"];
                    if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                        this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
                    else
                        this._libranza = null;
                    this._cliente = txtCliente.Value;
                    #endregion

                    this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                        this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, null, null);

                    if (this._Query.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(this._Query);
                        frm.Show();
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_DigitarCliente));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_AnalisisPagos", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {


                string reportName = string.Empty;
                CheckEdit byCuota = (CheckEdit)this.RPControls["Cuota"];

                if (!byCuota.Checked)
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                  this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, null, null, null);
                }
                else
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                              this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, null, null, true);

                }
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_AnalisisPagos.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

    }
}
