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
    public class Reports_ReportLineas : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                //string reportName = this._bc.AdministrationModel.ReportesContabilidad_ReporteLineaParametrizable(this.documentReportID,this._reporteID,this._fechaIni, this._fechaFin);
                //string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                ////Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_ReportLineaParam.cs-LoadReportMethod"));
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
        DateTime _fechaIni ;
        DateTime _fechaFin;
        //Filtro
        private string _reporteID = "";
        private byte _tipoReporte = 0;
        private int _documentSituacionFinanciera = AppReports.coReporteSituacionFinanciero;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_ReportLineas() 
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coReporteLineas;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = "Reportes por Lineas Contabilidad";

            #region Configurar Opciones

            this.AddMaster("ReporteID", AppMasters.coReporte, true, null);

            //Filtro para Monedas
            List<ReportParameterListItem> tipoReport = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() {Key = "1", Desc="Activo" },
                            new ReportParameterListItem() {Key = "2", Desc= "Pasivo-Patrimonio" }};

           // this.AddList("tipoReporte", "tipoReporte", tipoReport, true, "1",false);

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

                if (string.IsNullOrEmpty(this._reporteID))
                {
                    MessageBox.Show("Debe digitar un reporte");
                    return;
                }
                else
                {
                    DTO_coReporte reporte = (DTO_coReporte)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.coReporte,false,this._reporteID,true);
                    if (reporte != null)
                        this.documentReportID = Convert.ToInt32(reporte.DocumentoID.Value);
                }
                   

                string reportName = this._bc.AdministrationModel.ReportesContabilidad_ReporteLineaParametrizable(this.documentReportID, this._reporteID,this._tipoReporte, this._fechaIni, this._fechaFin);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);

                this.btnFilter.Enabled = true;
                this.btnResetFilter.Enabled = true;
                this.periodoFilter1.Enabled = true;
                this.btnExportToPDF.Enabled = true;
                this.btnExportToXLS.Enabled = true;
                //Thread process = new Thread(this.LoadReportMethod_PDF);
                //process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "Reports_ReportLineaParam", "Report"));
                throw;
            }
        }

        #endregion

        #region Eventos
        protected override void ListValueChanged(string option, object sender)
        {
            //ReportParameterList listReportType = (ReportParameterList)this.RPControls["tipoReporte"];

            if (option.Equals("ReporteID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._reporteID = master.ValidID ? master.Value : string.Empty;
                //DTO_coReporte reporte = (DTO_coReporte)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coReporte, false, this._reporteID, true);
                //if (reporte != null)
                //    this.documentReportID = Convert.ToInt32(reporte.DocumentoID.Value);

                //if (this.documentReportID == this._documentSituacionFinanciera)                
                //    listReportType.Visible = true;
                //else
                //    listReportType.Visible = false;
            }
        }

        #endregion

    }
}
