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
    class Reports_LiquidacionContrato : ReportParametersForm, IFiltrable
    {
        #region Variables
        private string _filtro = null;        
        private int _report = 1;
        List<DTO_ReportVacacionesDocumento> _filtroFecha = new List<DTO_ReportVacacionesDocumento>();
        List<ReportParameterListItem> fechas = new List<ReportParameterListItem>();
        ReportParameterList filt = new ReportParameterList();

        // Filtro para generar Reporte
        private string _docuementoID = string.Empty;
        private int _vacaciones = 0;
        private string fechaFiltro = string.Empty;
        private String _empleadoID = null;
        DateTime _fechaIni;
        DateTime _fechaFin;
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
        public Reports_LiquidacionContrato()
        {
            this.ReportForm = AppReportParametersForm.noLiquidacionContrato;
            this.periodoFilter1.Visible = false;
        }

        /// <summary>
        /// Inicializa la info del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.no;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.noLiquidacionContrato).ToString());
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> combReports = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "01", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_relacionLiquidacion) }, 
                new ReportParameterListItem() { Key = "02", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_DocumentoLiquidacion)},
            };
            List<ReportParameterListItem> filtro = new List<ReportParameterListItem>();
            {
                new ReportParameterListItem() { Key = "filtro", Desc = DictionaryTables.Rpt_filtroFecha };
            };
            
            // Controles
            this.AddList    ("1", (AppForms.ReportForm).ToString() + "_Tipo", combReports, true,"01",true);
            this.AddMaster  ("2",  AppMasters.coTercero, false, null, true);
            this.AddList    ("3", (AppForms.ReportForm).ToString() + "_Fecha", filtro, false,null, false);
            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
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
            //Oculta el filtro de la fecha
            this.periodoFilter1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

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
            try
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                ControlsUC.uc_MasterFind masterterceroid = (ControlsUC.uc_MasterFind)this.RPControls["2"];
                this._empleadoID = masterterceroid.Value;
                filt = (ReportParameterList)this.RPControls["3"];

                if (option.Equals("1"))
                {
                    fechas = new List<ReportParameterListItem>();
                    filt.Visible = false;
                    #region Relación de liquidación
                    if (listReportType.SelectedListItem.Key == "01")
                    {
                        this._report = 1;

                        #region Combo Dinamico que muestra las fechas tras una consulta para servir como filtro en el Reporte
                        if (!string.IsNullOrEmpty(_empleadoID))
                        {
                            filt.Visible = true;

                            _filtroFecha = this._bc.AdministrationModel.Report_No_GetLiquidaContratoByEmpleado(this._empleadoID);
                            foreach (var fec in _filtroFecha)
                            {
                                ReportParameterListItem item = new ReportParameterListItem();
                                item.Key = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString(); // -> Fecha ingreso a la compañia
                                item.Desc = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                                fechas.Add(item);
                            }
                            if (_filtroFecha == null)
                            {
                                filt.SetItems("Fecha Ingreso:", null);
                            }
                            else
                            {
                                filt.SetItems("Fechas Ingreso:", fechas);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Documento Liquidación

                    if (listReportType.SelectedListItem.Key == "02")
                    {
                        this._report = 2;
                        filt.Visible = false;
                    }
                    #endregion
                }

                #region Combo Dinamico que muestra las fechas tras una consulta para servir como filtro en el Reporte 
                //--
                if (!string.IsNullOrEmpty(_empleadoID) && listReportType.SelectedListItem.Key == "01" && !option.Equals("3"))
                {
                    fechas = new List<ReportParameterListItem>();
                    filt = (ReportParameterList)this.RPControls["3"];
                    filt.Visible = true;
                    _filtroFecha = this._bc.AdministrationModel.Report_No_GetLiquidaContratoByEmpleado(this._empleadoID);
                    foreach (var fec in _filtroFecha)
                    {
                        ReportParameterListItem item = new ReportParameterListItem();
                        item.Key = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                        item.Desc = fec.PeriodoDescansoInicial.Value.Value.ToShortDateString();
                        fechas.Add(item);
                    }
                    if (_filtroFecha.Count == 0)
                    {
                        filt.SetItems("Fecha Ingreso:", null);
                    }
                    else
                    {
                        filt.SetItems("Fechas Ingreso:", fechas);
                    }
                }
                #endregion

                #region Asigna el valor del item a una varible que sirve como filtro
                if (option.Equals("3") && filt.SelectedListItem != null)
                {
                        this.fechaFiltro = filt.SelectedListItem.Key;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_LiquidacionContrato.cs", "ListValueChanged"));
                throw;
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
                        this._vacaciones = AppDocuments.LiquidacionContrato;  // -> Liquidacion contrato = 84
                        reportName = this._bc.AdministrationModel.Report_No_VacacionesDocumento(this._empleadoID, this._vacaciones, this.fechaFiltro);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp,null,null,reportName.ToString());
                        Process.Start(fileURl);
                        break;
                    case 2:
                        // Proximo reporte
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_LiquidacionContrato.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Consulta
        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<SentenceTransformer.ConsultasFields> fields)
        {
            foreach (var fil in consulta.Filtros)
                this._filtro = fil.ValorFiltro;
        } 
        #endregion
    }
}
