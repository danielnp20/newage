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
    public class Reports_FacturaVentaMasivo : ReportParametersForm
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVentaMasivo(this._prefijoID, this._docNroIni, this._docNroFin);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_FacturaVentaMasivo.cs-LoadReportMethod"));
                throw;
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
        //Variables Dto
        private string _prefijoID = string.Empty;
        private int _docNroIni = 0;
        private int _docNroFin = 0;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_FacturaVentaMasivo() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.fa;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.faFacturasVentaMasivo).ToString());

            #region Configurar Opciones
            this.AddMaster("PrefijoID", AppMasters.glPrefijo, false, null);
            this.AddTextBox("DocNroIni", false, "DocNroIni");
            this.AddTextBox("DocNroFin", false, "DocNroFin");
            #endregion

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

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

        #region Eventos
        protected override void ListValueChanged(string option, object sender)
        {
             uc_MasterFind master = (uc_MasterFind)this.RPControls["PrefijoID"];
             this._prefijoID = master.ValidID? master.Value : string.Empty;

             ReportParameterTextBox docNroIni = (ReportParameterTextBox)this.RPControls["DocNroIni"];
             ReportParameterTextBox docNroFin = (ReportParameterTextBox)this.RPControls["DocNroFin"];
             this._docNroIni = !string.IsNullOrEmpty(docNroIni.txtFrom.Text) ? Convert.ToInt32(docNroIni.txtFrom.Text) : 0;
             this._docNroFin = !string.IsNullOrEmpty(docNroFin.txtFrom.Text) ? Convert.ToInt32(docNroFin.txtFrom.Text) : 0;
        }

        #endregion    
    }
}
