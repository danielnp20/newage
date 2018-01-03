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
    class Reports_VacacionesNomina : ReportParametersForm, IFiltrable
    {
        #region Variables
        private string _filtro = null;        
        private byte _report = 1;
        List<DTO_ReportVacacionesDocumento> _filtroFecha = new List<DTO_ReportVacacionesDocumento>();
        List<ReportParameterListItem> fechas = new List<ReportParameterListItem>();
        ReportParameterList filt = new ReportParameterList();

        // Filtro para generar Reporte
        private string _docuementoID = string.Empty;
        private int _vacaciones = 0;
        private int _numeroDoc = 0;
        private string fechaFiltro = string.Empty;
        private String _empleadoID = null;
        DateTime? _fechaIni;
        DateTime? _fechaFin;
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

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_VacacionesNomina()
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
            this.documentReportID = AppReports.noVacacionesParameter;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.noVacacionesParameter).ToString());
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> noVacacionesParameterReportType = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "VacacionesPagadas", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_VacacionesPagadas) }, 
                new ReportParameterListItem() { Key = "VacacionesPendientes", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_VacacionesPendientes) },
                new ReportParameterListItem() { Key = "DocumentoLiquida", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_DocumentoLiquidacion)},
            };
            List<ReportParameterListItem> filtro = new List<ReportParameterListItem>();
            {
                new ReportParameterListItem() { Key = "1", Desc = DictionaryTables.Rpt_filtroFecha };
            };
            
            // Controles
            this.AddList("1", (AppForms.ReportForm).ToString() + "_Tipo", noVacacionesParameterReportType, true, "VacaionesPagadas");
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
            List<ConsultasFields> fieldsVA = new List<ConsultasFields>();
            fieldsVA.Add(new ConsultasFields("EmpleadoID", "Cedula", typeof(string)));
            mq = new MasterQuery(this, AppReports.noVacacionesParameter, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsVA);
         
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
            ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];   
            ControlsUC.uc_MasterFind masterterceroid = (ControlsUC.uc_MasterFind)this.RPControls["empleadoid"];
            this._empleadoID = masterterceroid.Value;
            filt = (ReportParameterList)this.RPControls["3"];
            

            if (option.Equals ("1"))
            {
                filt.Visible = false;
                #region Vacaciones Pagadas

                if (listReportType.SelectedListItem.Key == "VacacionesPagadas")
                {
                    this._report = 1;

                    filt.Visible = false;
                }
                    

                #endregion

                #region Vacaciones Pendientes

                if (listReportType.SelectedListItem.Key == "VacacionesPendientes")
                {
                    this._report = 2;

                    filt.Visible = false;
                }
                    

                #endregion

                #region DocumentoLiquida

                if (listReportType.SelectedListItem.Key == "DocumentoLiquida")
                {
                    this._report = 3;

                    #region Combo Dinamico que muestra las fechas tras una consulta
                    if (!string.IsNullOrEmpty(_empleadoID))
                    {
                        // Recetea los item del combo para refrezcar la lista
                        fechas = new List<ReportParameterListItem>();
                        // Hacer control visible
                        filt.Visible = true;
                        // Realiza la consuta para el traer los filtros de fechas
                        _filtroFecha = this._bc.AdministrationModel.Report_No_GetVacacionesByEmpleado(this._empleadoID);
                        // Guarda los item en la lista para mostrar en el combo de filtros
                        foreach (var fec in _filtroFecha)
                        {
                            ReportParameterListItem item = new ReportParameterListItem();
                            item.Key = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                            item.Desc = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                            fechas.Add(item);
                        }
                        if (_filtroFecha == null)
                            filt.SetItems("Fecha:", null);
                        else
                            filt.SetItems("Fechas:", fechas);                       
                    }                   
                    #endregion
                }
                #endregion
            }

            #region Combo Dinamico que muestra las fechas tras una consulta
            if (!string.IsNullOrEmpty(_empleadoID) && listReportType.SelectedListItem.Key == "DocumentoLiquida" && !option.Equals("3"))
            {
                // Recetea los item del combo para refrezcar la lista
                fechas = new List<ReportParameterListItem>();
                // Hacer control visible
                filt.Visible = true;
                // Realiza la consuta para el traer los filtros de fechas
                _filtroFecha = this._bc.AdministrationModel.Report_No_GetVacacionesByEmpleado(this._empleadoID);
                // Guarda los item en la lista para mostrar en el combo de filtros
                foreach (var fec in _filtroFecha)
                {
                    ReportParameterListItem item = new ReportParameterListItem();
                    item.Key = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                    item.Desc = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                    fechas.Add(item);
                }

                if(_filtroFecha.Count == 0)
                    filt.SetItems("Fecha:", null);
                else
                    filt.SetItems("Fechas:", fechas);
            }    
            #endregion      

            if (option.Equals("3") && filt.SelectedListItem != null)
            {
                try
                {
                    this.fechaFiltro = filt.SelectedListItem.Key;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                    throw;
                }
            }
            if (this._report == 3) //Documento
                this.btnExportToXLS.Enabled = false;
            else
                this.btnExportToXLS.Enabled = true;
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
                        reportName = this._bc.AdministrationModel.Report_No_VacacionesPagadas(this._fechaIni.Value, this._fechaFin.Value, this._filtro, false, this._formatType, this._empleadoID);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);
                        break;
                    case 2:
                        this._vacaciones = AppDocuments.Vacaciones;
                        reportName = this._bc.AdministrationModel.Report_No_VacacionesPendientes(this._empleadoID, this._vacaciones, this._formatType);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        Process.Start(fileURl);
                        break;
                    case 3:
                        this._vacaciones = AppDocuments.Vacaciones;
                        reportName = this._bc.AdministrationModel.Report_No_VacacionesDocumento(this._empleadoID, this._vacaciones, this.fechaFiltro);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp,null,null,reportName.ToString());
                        Process.Start(fileURl);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_VacacionesNomina.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<SentenceTransformer.ConsultasFields> fields)
        {
            foreach (var fil in consulta.Filtros)
                this._filtro = fil.ValorFiltro;
        }
    }
}
