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
    public class Reports_LibroVentas : ReportParametersForm, IFiltrable
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
                ReportParameterTextBox prefijo = (ReportParameterTextBox)this.RPControls["Prefijo"];
                ReportParameterTextBox NroFactura = (ReportParameterTextBox)this.RPControls["Factura"];

                reportName = this._bc.AdministrationModel.ReportesFacturacion_LibroVentas(this._fechaIni, this._fechaFin.Month, this._clienteFiltro, prefijo.txtFrom.Text,
                    NroFactura.txtFrom.Text, this._formatType);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_LibroVentas.cs-LoadReportMethod"));
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
        //Filtro
        private string _clienteFiltro = "";
        private string _Prefijo;
        private string _Factura;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_LibroVentas() 
        {
            this.Module = ModulesPrefix.fa;
            this.ReportForm = AppReportParametersForm.faLibroVentas;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.fa;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.faLibroVentas).ToString());

            #region Configurar Opciones

            this.AddMaster("Cliente", AppMasters.faCliente, true, null);
            this.AddTextBox("Prefijo", false, (AppForms.ReportForm).ToString() + "_Prefijo", true);
            this.AddTextBox("Factura", false, (AppForms.ReportForm).ToString() + "_Factura", true);
            #endregion);

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;

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
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaFin = Convert.ToDateTime(fechaFinString);


                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "Reports_LibroVentas", "Report"));
                throw;
            }
        }

        #endregion

        #region Eventos
        protected override void ListValueChanged(string option, object sender)
        {
            //ReportParameterList listReportType = (ReportParameterList)this.RPControls["Cliente"];

            if (option.Equals("Cliente"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._clienteFiltro = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
        }

        #endregion

    }
}
