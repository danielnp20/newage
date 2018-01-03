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
using System.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_Saldos : ParametersCarteraForm
    {
        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Saldos() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.documentReportID = AppReports.ccReportesCartera;
            base.InitReport();
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
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
            #region Carga el filtro por libranza(obligacion)
            ReportParameterTextBox txtlibranza = (ReportParameterTextBox)this.RPControls["Obligacion"];
            if (!string.IsNullOrEmpty(txtlibranza.txtFrom.Text))
                this._libranza = Convert.ToInt32(txtlibranza.txtFrom.Text);
            else
                this._libranza = null;
            #endregion
            #region Carga el filtro Rompimiento
            ReportParameterList listRomp = (ReportParameterList)this.RPControls["Rompimiento"];
            if (!string.IsNullOrEmpty(listRomp.SelectedListItem.Key))
                this._rompimiento = Convert.ToByte(listRomp.SelectedListItem.Key);
            #endregion
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;
            if (string.IsNullOrEmpty(this._cliente) && string.IsNullOrEmpty(this._libranza.ToString()) && _tipoReporte == 2)
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_DigitarCliente));
            else
            {
                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                //if (!string.IsNullOrEmpty(this._cliente) || !string.IsNullOrEmpty(this._libranza.ToString()))
                //{
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
                    #region Carga el filtro por libranza(obligacion)
                    ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
                    if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                        this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
                    else
                        this._libranza = null;
                    #endregion
                    #region Carga el filtro Rompimiento
                    ReportParameterList listRomp = (ReportParameterList)this.RPControls["Rompimiento"];
                    if (!string.IsNullOrEmpty(listRomp.SelectedListItem.Key))
                        this._rompimiento = Convert.ToByte(listRomp.SelectedListItem.Key);
                    #endregion

                    this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                        this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, this._agrupamiento, this._rompimiento);
                    if (this._Query.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(this._Query);
                        frm.Show();
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                //}
                //else
                //{
                //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_DigitarCliente));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Saldos", "Report_XLS"));
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
                string fileURl;

                string reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                  this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, this._agrupamiento, this._rompimiento, this._formatType);

                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Saldos.cs-LoadReportMethod_PDF"));
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
