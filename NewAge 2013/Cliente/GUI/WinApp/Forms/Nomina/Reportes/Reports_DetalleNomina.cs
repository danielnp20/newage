using System;
using System.Collections.Generic;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Librerias.Project;
using System.Windows.Forms;
using System.Threading;
using SentenceTransformer;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_DetalleNomina : ReportParametersForm
    {
        #region Variables
        private int _documentID = 0;
        private string _ordenReport = null;
        private bool _orderByName = false;
        private bool _isAll = false;
        private bool _isPre = false;

        private string _terceroID;
        private string _operacionNoID;
        private string _areaFuncionalID;
        private string _conceptonoID;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_DetalleNomina()
        {
            this.Module = ModulesPrefix.no;
            this.ReportForm = AppReportParametersForm.inSaldos;
            this.periodo = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
            this._documentID = AppDocuments.Nomina;
        }

        /// <summary>
        /// Inicializa la info del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.no;
            this.documentReportID = AppReports.noDetalleNomina;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.noDetalleNomina).ToString()); 
            #region Configurar Opciones

            List<ReportParameterListItem> noDetalleNominaReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Nomina) }, 
                           new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Vacaciones) },
                           new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Liquidacion) },
                           new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Prima) },
                           new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Cesantias) },
                           //new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos) },
                        };
            List<ReportParameterListItem> noDetalleNominaReportType2 = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Det_Empleado_Concepto) }, 
                           new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Det_Concepto_Empleado) },
                           new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido_X_Concepto) },
                           new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido_X_Empleado) },
                        };
            this.AddList("1", (AppForms.ReportForm).ToString() + "_TipoTransaccion", noDetalleNominaReportType, true, "Nomina");
            this.AddList("3", (AppForms.ReportForm).ToString() + "_tipoReport", noDetalleNominaReportType2, true, "Det.Empleado Concepto");
            this.AddCheck("4", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkPreliminar"));
            this.AddCheck("2", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkNombre"));

            this.AddMaster("terceroid", AppMasters.coTercero, false, null, true);
            this.AddMaster("operacionid", AppMasters.noOperacion, false, null, true);
            this.AddMaster("areafuncionalid", AppMasters.glAreaFuncional, false, null, true);
            this.AddMaster("conceptonomid", AppMasters.noConceptoNOM, false, null, true);

            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            this.btnExportToXLS.Visible = true;
            List<ConsultasFields> fieldsNR = new List<ConsultasFields>();
            fieldsNR.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
            mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNR);
            mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
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

            int year = Convert.ToInt16(this.periodoFilter1.Year[1].ToString());
            int Month = this.periodoFilter1.Months[1];
            this._fechaFin = new DateTime(year, Month, DateTime.DaysInMonth(year, Month));


            //string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];

            this._fechaIni = Convert.ToDateTime(fechaIniString);
            //this._fechaFin = Convert.ToDateTime(fechaFinString);

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

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                int year = Convert.ToInt16(this.periodoFilter1.Year[1].ToString());
                int Month = this.periodoFilter1.Months[1];
                this._fechaFin = new DateTime(year, Month, DateTime.DaysInMonth(year, Month));

                byte tipoReport = !string.IsNullOrEmpty(this._ordenReport) ? Convert.ToByte(this._ordenReport) : (byte)1;

                this._Query = this._bc.AdministrationModel.Reportes_No_NominaToExcel(this.documentReportID, tipoReport, this._fechaIni, this._fechaFin, string.Empty, this._operacionNoID,
                                                                                    this._conceptonoID, this._areaFuncionalID,this._orderByName.ToString(), this._isAll.ToString(),this._terceroID,this._isPre, null,Convert.ToByte(this._documentID));

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
            #region Opc 1 Documento
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                if (listReportType.SelectedListItem.Key == 1.ToString())
                    this._documentID = AppDocuments.Nomina;

                if (listReportType.SelectedListItem.Key == 2.ToString())
                    this._documentID = AppDocuments.Vacaciones;

                if (listReportType.SelectedListItem.Key == 3.ToString())
                    this._documentID = AppDocuments.LiquidacionContrato;

                if (listReportType.SelectedListItem.Key == 4.ToString())
                    this._documentID = AppDocuments.Prima;

                if (listReportType.SelectedListItem.Key == 5.ToString())
                    this._documentID = AppDocuments.Cesantias;

                if (listReportType.SelectedListItem.Key == 6.ToString())
                    this._isAll = true;
            }
            #endregion

            #region Opc 2 Ordenar Por Nombre Check
            if (option.Equals("2"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                switch (reportParameters["2"][0])
                {
                    case "True":
                        this._orderByName = true;
                        break;
                    case "False":
                        this._orderByName = false;
                        break;
                }

            }
            #endregion

            #region Opc 3 Orden del Reporte (Rompimientos)
            if (option.Equals("3"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                this._ordenReport = reportParameters["3"][0];
            }

            #endregion

            #region Opc 4 Indicica si la liquidación es Preliminar o definitiva
            if (option.Equals("4"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                switch (reportParameters["4"][0])
                {
                    case "True":
                        this._isPre = true;
                        break;
                    case "False":
                        this._isPre = false;
                        break;
                }
            }
            #endregion
            
            #region opc 5 asigna filtros
            ControlsUC.uc_MasterFind masterterceroid = (ControlsUC.uc_MasterFind)this.RPControls["terceroid"];
            this._terceroID = masterterceroid.Value;

            ControlsUC.uc_MasterFind masteroperacionid = (ControlsUC.uc_MasterFind)this.RPControls["operacionid"];
            this._operacionNoID = masteroperacionid.Value;

            ControlsUC.uc_MasterFind masterareafuncionalid = (ControlsUC.uc_MasterFind)this.RPControls["areafuncionalid"];
            this._areaFuncionalID = masterareafuncionalid.Value;

            ControlsUC.uc_MasterFind masterconceptoid = (ControlsUC.uc_MasterFind)this.RPControls["conceptonomid"];
            this._conceptonoID = masterconceptoid.Value;

            #endregion
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
                string reportName = string.Empty;
                string fileURl;

                if (this._ordenReport == null)
                    this._ordenReport = "1";

                if (this._ordenReport == "1")
                    reportName = this._bc.AdministrationModel.Report_No_DetailLiquidaciones(this._documentID, Convert.ToDateTime(this.periodo), this._ordenReport, this._fechaIni, this._fechaFin, this._isAll, this._orderByName, this._isPre, this._formatType, this._terceroID, this._operacionNoID, this._areaFuncionalID, this._conceptonoID);

                if (this._ordenReport == "2")
                    reportName = this._bc.AdministrationModel.Report_No_XConcepto(this._documentID, Convert.ToDateTime(this.periodo), this._ordenReport, this._fechaIni, this._fechaFin, this._isAll, this._orderByName, this._isPre, this._formatType, this._terceroID, this._operacionNoID, this._areaFuncionalID, this._conceptonoID);

                if (this._ordenReport == "3")
                    reportName = this._bc.AdministrationModel.Report_No_TotalXConcepto(this._documentID, Convert.ToDateTime(this.periodo), this._ordenReport, this._fechaIni, this._fechaFin, this._isAll, this._orderByName, this._isPre, this._formatType, this._terceroID, this._operacionNoID, this._areaFuncionalID, this._conceptonoID);

                if (this._ordenReport == "4")
                    reportName = this._bc.AdministrationModel.Report_No_Detalle(this._documentID, Convert.ToDateTime(this.periodo), this._ordenReport, this._fechaIni, this._fechaFin, this._isAll, this._orderByName, this._isPre, this._formatType, this._terceroID, this._operacionNoID, this._areaFuncionalID, this._conceptonoID);

                if (!string.IsNullOrEmpty(reportName))
                {
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_ReporteNotDataFound));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DetalleNomina.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion
    }
}
