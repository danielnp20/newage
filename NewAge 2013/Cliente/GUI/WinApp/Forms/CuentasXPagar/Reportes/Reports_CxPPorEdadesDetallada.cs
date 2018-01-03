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
    public class Reports_CxPPorEdadesDetallada : ReportParametersForm, IFiltrable
    {
        #region Variables
        private byte _tipoReport = 0;
        private DateTime? _fechaIni;
        private string _tercero = string.Empty;
        private string _cuentaID= string.Empty;
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
                if (this._tipoReport == 1 || this._tipoReport == 0)
                {
                    reportName = this._bc.AdministrationModel.Report_Cp_PorEdadesDetallado(this._fechaIni.Value, this._tercero,this._cuentaID, true, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    Process.Start(fileURl);
                }
                if (this._tipoReport == 2)
                {
                    reportName = this._bc.AdministrationModel.Report_Cp_PorEdadesResumido(this._fechaIni.Value, this._tercero,this._cuentaID, false, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CxPPorEdadesDetallada.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_CxPPorEdadesDetallada() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cp;
            this.documentReportID = AppReports.cpPorEdades;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.cpPorEdades).ToString());

            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> reportxEdades = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido) }, 
            };
            //Determina el nombre del combo y el item donde debe quedar
            this.AddList("1", (AppForms.ReportForm) + "_Tipo", reportxEdades, true, "1");

            //Se establece el filtro del Master Find
            this.AddMaster("3", AppMasters.coTercero, false, null);
            this.AddMaster("4", AppMasters.coPlanCuenta, true, null);
            #endregion);

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            base.btnExportToXLS.Visible = true;
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
            this._fechaIni = Convert.ToDateTime(fechaIniString);

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
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                if (this._tipoReport == 1 || this._tipoReport == 0)
                    this._tipoReport =1;


                this._Query = this._bc.AdministrationModel.Reportes_Cp_CxPToExcel(this.documentReportID, this._tipoReport, this._fechaIni.Value, null, this._tercero, string.Empty, this._cuentaID, string.Empty, string.Empty, null, null, null);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CxPPorEdadesDetallado.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Eventos

        protected override void ListValueChanged(string option, object sender)
        {
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                if (listReportType.SelectedListItem.Key == 1.ToString())
                    this._tipoReport = 1;
                
                if (listReportType.SelectedListItem.Key == 2.ToString())
                    this._tipoReport = 2;
            }
            if (option.Equals("3"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._tercero = master.Value;
                else
                    this._tercero = string.Empty;
            }
            if (option.Equals("4"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._cuentaID = master.Value;
                else
                    this._cuentaID = string.Empty;
            }
        }

        #endregion
    }
}
