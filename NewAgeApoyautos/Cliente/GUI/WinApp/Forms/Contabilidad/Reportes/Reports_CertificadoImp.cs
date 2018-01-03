using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors;
using System.Collections;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_CertificadoImp : ReportParametersForm
    {
        #region Variables

        private byte _tipoReport = 1;
        private DateTime? _fechaIni;
        private DateTime? _fechaFin;
        private string _terceroID = string.Empty;

        #endregion
   
        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_CertificadoImp()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coCertificates;
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coCertificates;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coCertificates).ToString());

            #region Configurar Opciones

            #region Cargo los combos del reporte

            //Carga el combo con el tipo de formato que desea exportar
            List<ReportParameterListItem> tipoCertificado = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Retefuente) },
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_IVA) }, 
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ReteICA) }, 
                new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ImpConsumo) }, 
                new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, "DIAN") }
            };

            #endregion

            this.AddList("TipoReporte", (AppForms.ReportForm).ToString() + "_TipoCertificado", tipoCertificado, true, "1", true);
            this.AddMaster("TerceroID", AppMasters.coTercero, false, null);
            #endregion

            #region Configurar Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
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
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            base.btnExportToPDF.Enabled = false;
            base.btnExportToXLS.Enabled = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);
            this._fechaFin = new DateTime(this._fechaFin.Value.Year, this._fechaFin.Value.Month, DateTime.DaysInMonth(this._fechaFin.Value.Year, this._fechaFin.Value.Month));

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            #region Carga el tipo de impuesto

            if (option.Equals("TipoReporte"))
            {
                ReportParameterList TipoReporte = (ReportParameterList)this.RPControls["TipoReporte"];
                this._tipoReport = Convert.ToByte(TipoReporte.SelectedListItem.Key);
            }
            else if (option.Equals("TerceroID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._terceroID = master.Value;
                else
                    this._terceroID = string.Empty;
            }     
            #endregion
            
        }

        #endregion

        #region Hilos

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {  
            try
            {               
                string name = this._bc.AdministrationModel.ReportesContabilidad_GetByParameter(this.documentReportID,this._tipoReport,this._fechaIni,this._fechaFin,string.Empty,string.Empty,null,string.Empty,
                              this._terceroID,string.Empty,string.Empty,string.Empty,null,null,null);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, name);
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CertificadoImp.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #endregion
    }
}
