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
    public class ReportsChequesGirados : ReportParametersForm, IFiltrable
    {
        #region Variables
        private int _report = 0;
        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        private int _tipo;
        //Variables Dto
        private string _bancoCuenta = string.Empty;
        private string _tercero = string.Empty;
        private bool _beneficiarioInd =false;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ReportsChequesGirados() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.ts;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.tsChequesGirados).ToString());

            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tsChequesGiradosOrder = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Banco) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) }, 
            };
            //List<ReportParameterListItem> tsChequesGiradosReportType = new List<ReportParameterListItem>()
            //{
            //    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado) }, 
            //    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido) }, 
            //};
            this.AddList("1", (AppForms.ReportForm) + "_Rompimiento", tsChequesGiradosOrder, true, "1");
            this.AddMaster("3", AppMasters.tsBancosCuenta, true, null);
            this.AddMaster("4", AppMasters.coTercero, true, null);
            this.AddCheck("5", "Nombre Beneficiario");
            #endregion);

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
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
            this.btnExportToPDF.Enabled = false;

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
                if (option.Equals("5"))
                {
                    CheckEdit check = (CheckEdit)sender;
                    this._beneficiarioInd = check.Checked;
                }
                if (option.Equals("3")) //bancoCuenta
                {
                    uc_MasterFind master = (uc_MasterFind)sender;
                    if (master.ValidID)
                        this._bancoCuenta = master.Value;
                }
                if (option.Equals("4")) //Tercero
                {
                    uc_MasterFind master = (uc_MasterFind)sender;
                    if (master.ValidID)
                        this._tercero = master.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_ChequesGirados.cs-ListValueChanged"));
            }
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
                string reportName;
                string fileURl;
                if (this._report == 1 || this._report == 0)
                {
                    reportName = this._bc.AdministrationModel.Reporte_Ts_ChequesGiradosRep(this._bancoCuenta, this._tercero, this._fechaIni, this._fechaFin, "1", this._beneficiarioInd);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                if (this._report == 2)
                {
                    reportName = this._bc.AdministrationModel.Reporte_Ts_ChequesGiradosRep(this._bancoCuenta, this._tercero, this._fechaIni, this._fechaFin, "2", this._beneficiarioInd);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_ChequesGirados.cs-LoadReportMethod"));
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
