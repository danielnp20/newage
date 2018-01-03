using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_RelacionPagos : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                if (this.txtChequeNro == "0")
                    this.txtChequeNro = "";
                string reportName;
                string fileURl;
                if (this._report == 0 || this._report == 1)
                {
                  
                    reportName = this._bc.AdministrationModel.Report_Ts_RelacionPagos(this._fechaIni, this._fechaFin, this._tsBanco, this._coTercero, this.txtChequeNro, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                if (this._report == 2)
                {
                    reportName = this._bc.AdministrationModel.Report_Ts_RelacionPagosXBancos(this._fechaIni, this._fechaFin, this._tsBanco, this._coTercero, this.txtChequeNro, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_RelacionPagos.cs-", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        private int _report = 0;

        //Variables del hilo
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        private string _cheque;
        //Filtro
        private string _tsBanco = string.Empty;
        private string _coTercero = string.Empty;
        private string txtChequeNro;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_RelacionPagos() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.tsRelacionPagos;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.tsRelacionPagos).ToString());
            #region Configurar Opciones
            List<ReportParameterListItem> tsRelacionPagosReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Nit) }, 
                           new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Banco) },
                        };
            //Determina el nombre del combo y el item donde debe quedar
            this.AddList("1", (AppForms.ReportForm) + "_Orden", tsRelacionPagosReportType, true, "1");
            this.AddTextBox("2", false, (AppForms.ReportForm).ToString() + "_ChequeNro");
            ReportParameterTextBox tbChequeNro = (ReportParameterTextBox)this.RPControls["2"];
            //txtChequeNro = tbChequeNro;
            tbChequeNro.Enabled = true;
            tbChequeNro.Visible = true;
            
            this.AddMaster("3", AppMasters.tsBancosCuenta, true, null);
            this.AddMaster("4", AppMasters.coTercero, true, null);

            #endregion
            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            base.btnExportToXLS.Visible = true;
            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            this.txtChequeNro = reportParameters["2"][0].ToString();

            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;

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
                base.btnExportToPDF.Enabled = false;
                base.btnExportToXLS.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                this._Query = this._bc.AdministrationModel.Reportes_Ts_TesoreriaToExcel(this.documentReportID, null, this._fechaIni, this._fechaFin, this._coTercero, this.txtChequeNro,string.Empty, this._tsBanco, null, null);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_RelacionPagos.cs", "Report_XLS"));
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
            if (option.Equals("1"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                if (listReportType.SelectedListItem.Key == 1.ToString())
                {
                    this._report = 1;
                }

                if (listReportType.SelectedListItem.Key == 2.ToString())
                {
                    this._report = 2;
                }
            }
            if (option.Equals("2"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterTextBox listReportType = (ReportParameterTextBox)this.RPControls["2"];
                this._cheque = listReportType.txtFrom.Text;
            }
            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._tsBanco = master.Value;
                else
                    this._tsBanco = string.Empty;
            }
            if (option.Equals("4"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (!string.IsNullOrWhiteSpace(master.Value))
                    this._coTercero = master.Value;
                else
                    this._coTercero = string.Empty;
            }
        }
        #endregion
    }
}
