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
    public class Reports_CxPvsPagos : ReportParametersForm, IFiltrable
    {
        #region Variables
        private byte _tipoReport = 1;
        private DateTime? _fechaIni;
        private DateTime? _fechaFin;
        private string _cuentaID = string.Empty;
        private string _bancoCuenta = string.Empty;
        private string _terceroID = string.Empty;
        private byte _orden = 1;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_CxPvsPagos() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cp;
            this.documentReportID = AppReports.cpCxPvsPagos;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.cpCxPvsPagos).ToString());

            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> reportTipo = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "Cuenta x Pagar vs Pagos") }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, "Pagos vs Cuenta x Pagar") }, 
            };
            //Determina el nombre del combo y el item por defecto
            this.AddList("Tipo", (AppForms.ReportForm) + "_Tipo", reportTipo, true, "1");

            //Se establece el filtro del Master Find
            this.AddMaster("CuentaID", AppMasters.coPlanCuenta, true, null);
            this.AddMaster("BancoCuentaID", AppMasters.tsBancosCuenta, false, null);
            this.AddMaster("TerceroID", AppMasters.coTercero, false, null);

            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> reportOrden = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "Comprobante") }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, "Tercero") }, 
            };
            //Determina el nombre del combo y el item por defecto
            this.AddList("Orden", (AppForms.ReportForm) + "_Orden", reportOrden, true, "1");

            #endregion);

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;

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
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);
            this._fechaFin = new DateTime(this._fechaFin.Value.Year, this._fechaFin.Value.Month, DateTime.DaysInMonth(this._fechaFin.Value.Year, this._fechaFin.Value.Month));

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
                this._fechaFin = new DateTime(this._fechaFin.Value.Year, this._fechaFin.Value.Month, DateTime.DaysInMonth(this._fechaFin.Value.Year, this._fechaFin.Value.Month));
                
                this._tipoReport = this._tipoReport == (byte)0 ? (byte)1 : this._tipoReport;

                this._Query = this._bc.AdministrationModel.Reportes_Cp_CxPToExcel(this.documentReportID, this._tipoReport,this._fechaIni,this._fechaFin,this._terceroID,string.Empty,this._cuentaID,this._bancoCuenta,string.Empty,null,null,this._orden);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CxPvsPagos.cs", "Report_XLS"));
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
            if (option.Equals("Tipo"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["Tipo"];
                this._tipoReport = Convert.ToByte(listReportType.SelectedListItem.Key);
            }
            if (option.Equals("Orden"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportOrden = (ReportParameterList)this.RPControls["Orden"];
                this._orden = Convert.ToByte(listReportOrden.SelectedListItem.Key);
            }
            else if (option.Equals("CuentaID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._cuentaID = master.Value;
                else
                    this._cuentaID = string.Empty;
            }
            else if (option.Equals("BancoCuentaID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._bancoCuenta = master.Value;
                else
                    this._bancoCuenta = string.Empty;
            }
            else if (option.Equals("TerceroID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (master.ValidID)
                    this._terceroID = master.Value;
                else
                    this._terceroID = string.Empty;
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
                string reportName = this._bc.AdministrationModel.Reportes_Cp_CxPvsPagos(this._tipoReport, this._fechaIni.Value, this._fechaFin.Value, this._cuentaID, this._bancoCuenta, this._terceroID, this._orden);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CxPvsPagos.cs-LoadReportMethod"));
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
