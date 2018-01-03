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
    class Reports_LiquidaContrato : ReportParametersForm
    {
        #region Variables
        private byte _report = 1;

        // Filtro para generar Reporte
        private string _documentoID = string.Empty;
        private int _docID = 0;
        private int _numeroDoc = 0;
        private string fechaFiltro = string.Empty;
        private string _empleadoID = string.Empty;
        DateTime? _fechaIni;
        DateTime? _fechaFin;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_LiquidaContrato()
        {          
            this.ReportForm = AppReportParametersForm.noVacacionesParameter;            
            _numeroDoc = (int)ReportForm;
            this.periodoFilter1.Visible = false;
        }

        /// <summary>
        /// Inicializa la info del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.no;
            this.documentReportID = AppReports.noLiquidacionContrato;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.noLiquidacionContrato).ToString());
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> noVacacionesParameterReportType = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "DocumentoLiquida", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_DocumentoLiquidacion)},
            };
            // Controles
            this.AddList("1", (AppForms.ReportForm).ToString() + "_Tipo", noVacacionesParameterReportType, true, "DocumentoLiquida");
            this.AddMaster("empleadoid", AppMasters.coTercero, false, null, true);
            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            //this.btnExportToXLS.Visible = true;         
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
            this.periodoFilter1.Visible = false;

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
                this.periodoFilter1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                if (this._report == 3)
                {
                    if (!string.IsNullOrEmpty(this.fechaFiltro))
                        this._fechaIni = Convert.ToDateTime(fechaFiltro);
                    else
                        this._fechaIni = null;
                }                  
                
                this._Query = this._bc.AdministrationModel.Reportes_No_NominaToExcel(this.documentReportID, this._report, this._fechaIni, this._fechaFin, this._empleadoID, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_LiquidaContrato.cs", "Report_XLS"));
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
            ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];   
            ControlsUC.uc_MasterFind masterterceroid = (ControlsUC.uc_MasterFind)this.RPControls["empleadoid"];
            this._empleadoID = masterterceroid.ValidID? masterterceroid.Value : string.Empty;
         
            if (option.Equals ("1"))
            {
                #region DocumentoLiquida

                if (listReportType.SelectedListItem.Key == "DocumentoLiquida")
                    this._report = 1;

                #endregion
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
                switch(this._report)
                {
                    case 1:
                        this._docID = AppDocuments.LiquidacionContrato;
                        reportName = this._bc.AdministrationModel.Report_No_VacacionesDocumento(this._empleadoID, this._docID, this.fechaFiltro);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp,null,null,reportName.ToString());
                        Process.Start(fileURl);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_LiquidaContrato.cs", "LoadReportMethod"));
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
