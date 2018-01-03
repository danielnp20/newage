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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    //[System.Runtime.InteropServices.GuidAttribute("8DB7B708-88F8-49AB-891C-884E91F75A02")]
    public class Reports_CobroJuridico : ParametersCarteraForm
    {
        private byte claseDeuda = 1;
        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_CobroJuridico() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.documentReportID = AppReports.ccCobroJuridico;
            base.InitReport();
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            #region Carga el filtro por libranza(obligacion)
            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
            if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
            #endregion

            ReportParameterList claseDeuda = (ReportParameterList)this.RPControls["ClaseDeuda"];
            this.claseDeuda = Convert.ToByte(claseDeuda.SelectedListItem.Key);

            this.btnExportToPDF.Enabled = false;
            this.btnExportToXLS.Enabled = false;

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();

        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;
                
                #region Carga el filtro por libranza(obligacion)
                ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
                if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                    this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
                #endregion

                ReportParameterList claseDeuda = (ReportParameterList)this.RPControls["ClaseDeuda"];
                this.claseDeuda = Convert.ToByte(claseDeuda.SelectedListItem.Key);

                this._Query = this._bc.AdministrationModel.Report_Cc_CobroJuridicoToExcel(this.documentReportID, this._tipoReporte.Value, this._cliente, this._libranza.ToString(), this.claseDeuda);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CobroJuridico", "Report_XLS"));
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
                string reportName;
                string fileURl;

                reportName = this._bc.AdministrationModel.Report_Cc_CobroJuridico(AppReports.ccCobroJuridico, this.claseDeuda, 1, this._cliente, this._libranza.ToString());
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CobroJuridico.cs-LoadReportMethod"));
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
