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
    class Reports_CesantiasNomina : ReportParametersForm
    {
        #region Variables
     
        private byte _tipoReport = 2;
        // Filtro para generar Reporte
        private int _docID =  AppDocuments.Cesantias;
        private string _empleadoID = string.Empty;
        DateTime? _fechaIni;
        DateTime? _fechaFin;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_CesantiasNomina()
        {          
            this.ReportForm = AppReportParametersForm.noVacacionesParameter;            
        }

        /// <summary>
        /// Inicializa la info del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.no;
            this.documentReportID = AppReports.noCesantias;
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tipoReport = new List<ReportParameterListItem>()
            {
                //new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "Cesantias Acumuladas") }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables,"Cesantias Detalle") },
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, "Cesantias Documento")},
            };
            List<ReportParameterListItem> filtro = new List<ReportParameterListItem>();
            {
                new ReportParameterListItem() { Key = "1", Desc = DictionaryTables.Rpt_filtroFecha };
            };
            
            // Controles
            this.AddList("TipoReporte", (AppForms.ReportForm).ToString() + "_Tipo", tipoReport, true, "2");
            this.AddMaster("empleadoid", AppMasters.coTercero, false, null, true);
            this.AddList("3", (AppForms.ReportForm).ToString() + "_Fecha", filtro, false, "null", false);
            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            this.btnExportToXLS.Visible = true;        
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
            this.btnExportToXLS.Enabled = false;


            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

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
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                //Oculta el filtro de la fecha

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                this._Query = this._bc.AdministrationModel.Reportes_No_NominaToExcel(this.documentReportID, this._tipoReport, this._fechaIni, this._fechaFin,
                             this._empleadoID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._docID, null, null);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query, this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_AportesNomina.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            ReportParameterList listReportType = (ReportParameterList)this.RPControls["TipoReporte"];   
            ControlsUC.uc_MasterFind masterEmpleado = (ControlsUC.uc_MasterFind)this.RPControls["empleadoid"];
            this._empleadoID = masterEmpleado.ValidID? masterEmpleado.Value : string.Empty;

            if (option.Equals("TipoReporte"))
            {
                this._tipoReport = Convert.ToByte(listReportType.SelectedListItem.Key);
                if (this._tipoReport == 3) //Documento
                    this.btnExportToXLS.Enabled = false;
                else
                    this.btnExportToXLS.Enabled = true;
            }
               
       
        }
        #endregion

        #region Hilos
        protected override void LoadReportMethod_PDF()
        {
            string reportName;
            string fileURl;
            try
            {
                reportName = this._bc.AdministrationModel.Report_No_NominaGetByParameter(this.documentReportID,this._tipoReport,this._fechaIni,this._fechaFin,
                             this._empleadoID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,this._docID,null,null);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp,null,null,reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CesantiasNomina.cs", "LoadReportMethod"));
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
