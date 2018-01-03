using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;
using System.Threading;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_CarteraPagaduria : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = string.Empty;
        public DateTime _fechaIni = DateTime.Now;
        public string _pagaduria = string.Empty;
        public DateTime _fechaFin = DateTime.Now;
        DateTime _fechaUltimoCierre;
        public byte _report = 1;
        public byte _detalle = 1;
        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Funcion que se encarga de Inhabilitar los controles que no son necesarios para este reporte
        /// </summary>
        private void EnableControls()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructor
        /// </summary>
        public Reports_CarteraPagaduria()
        {
            this.Module = ModulesPrefix.cf;
            
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {                   
            this.Module = ModulesPrefix.cf;
            this.documentReportID = AppReports.ccReportePagaduria;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = true;
            this.btnExportToPDF.Enabled = true;
            this.btnExportToXLS.Enabled = false;

            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccReportePagaduria).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Mvto Pagadurias" },
                new ReportParameterListItem() { Key = "2", Desc = "Novedades" },    
                new ReportParameterListItem() { Key = "3", Desc = "Detalle Nomina" },   
                new ReportParameterListItem() { Key = "4", Desc = "Reporte para Analisis" },   
                new ReportParameterListItem() { Key = "5", Desc = "Reporte para Auditoria" },   
            
            };
            this.AddList("tipoReporte", "Tipo Reporte", ListReport, true, "1");
            List<ReportParameterListItem> ListDetalle = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Resumido" },
                new ReportParameterListItem() { Key = "2", Desc = "Detallado" },                
            };
            this.AddList("tipoDetalle", "Detalle", ListDetalle, true, "1");

            this.AddMaster("masterPagaduria", AppMasters.ccPagaduria, true, null);
            this.AddMaster("masterCliente", AppMasters.ccCliente, true, null,false);

            #endregion

            #region Obtiene ultima fecha de corte
            var ultimoCierre = this._bc.AdministrationModel.ccCierreDiaCartera_GetUltimoDiaCierre(string.Empty, null);
            if (ultimoCierre != null)
                this._fechaUltimoCierre = ultimoCierre.Fecha.Value.Value;
            else
                this._fechaUltimoCierre = this.periodo;

            #endregion

            #endregion

            #region Configurar Filtros del periodo
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Clear();
            this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
            this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
            this.EnableControls();
            #endregion


            #region Asigna Fecha de Ultimo Cierre
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            
            #endregion
        }
        #endregion

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {            
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;
            this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
            this._fechaIni = fechaIniString;
            this._fechaFin = fechaIniString;

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

                #region Carga filtros
                uc_MasterFind txtpagaduria = (uc_MasterFind)this.RPControls["masterPagaduria"];
                this._pagaduria = txtpagaduria.Value;
                uc_MasterFind txtcliente = (uc_MasterFind)this.RPControls["masterCliente"];
                this._clienteID = txtcliente.Value;

                #endregion

                    this.periodoFilter1.Enabled = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = false;

                    ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                    this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);
                    if (this._report != 4 && this._report != 5)
                    {
                        //this.periodoFilter1.Year[0].ToString();
                        //#region Asigna Fecha de Ultimo Cierre
                        //this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
                        //this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
                        //this.periodoFilter1.monthCB.Items.Clear();
                        //this.periodoFilter1.monthCB1.Items.Clear();
                        //this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
                        //this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
                        //this.periodoFilter1.monthCB1.Items.Add(this._fechaUltimoCierre.Date.Day);
                        //this.periodoFilter1.monthCB1.SelectedItem = this._fechaUltimoCierre.Date.Day;

                        //#endregion
                        DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / 1");
                        DateTime fechaFinString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / 1");
                        //DateTime fechaFinString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / " + this.periodoFilter1.monthCB1.Text);
                        //DateTime fechaFinString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
                        this._fechaIni = fechaIniString;
                        this._fechaFin = fechaIniString;

                    }
                    else
                    {
                        DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / 1");
                        DateTime fechaFinString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / 1");
                        //DateTime fechaFinString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.Text + " / " + this.periodoFilter1.monthCB1.Text);
                        
                        this._fechaIni = fechaIniString;
                        this._fechaFin = fechaFinString;

                    }                    
                string reportName;
                string fileURl;

                this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._report, this._fechaIni, this._fechaFin, this._clienteID, null, string.Empty,
                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._pagaduria, string.Empty, null, null,null);
                
                //Exporta Reporte a Excel.
                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));

                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CarteraPagaduria", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            uc_MasterFind txtcliente = (uc_MasterFind)this.RPControls["masterCliente"];
            txtcliente.Visible = false;
            ReportParameterList txtDetalle = (ReportParameterList)this.RPControls["tipoDetalle"];
            this._detalle = Convert.ToByte(txtDetalle.SelectedListItem.Key);
            ReportParameterList txtReporte = (ReportParameterList)this.RPControls["tipoReporte"];
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this._report = Convert.ToByte(txtReporte.SelectedListItem.Key);
            txtDetalle.Visible = true;

              if (this._detalle == 1 )
              {
                  this.btnExportToPDF.Enabled = true;
                  this.btnExportToXLS.Enabled = false;
              }
              else 
              {
                  this.btnExportToPDF.Enabled = false;
                  this.btnExportToXLS.Enabled = true;
              }
                if (this._report == 3 )             
              {
                  this.btnExportToPDF.Enabled = false;
                  this.btnExportToXLS.Enabled = true;
                  txtcliente.Visible = true;
                  txtDetalle.Visible = false;
              }
                if (this._report == 4)
                {
                    //#region Asigna Fecha de Ultimo Cierre
                    //this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
                    //this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
                    //this.periodoFilter1.monthCB.Items.Clear();
                    //this.periodoFilter1.monthCB1.Items.Clear();
                    //this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
                    //this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
                    //this.periodoFilter1.monthCB1.Items.Add(this._fechaUltimoCierre.Date.Day);
                    //this.periodoFilter1.monthCB1.SelectedItem = this._fechaUltimoCierre.Date.Day;

                    //#endregion

                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = true;
                    txtcliente.Visible = false;
                    txtDetalle.Visible = false;
                }

                if (this._report == 5)
                {
                    //#region Asigna Fecha de Ultimo Cierre
                    //this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
                    //this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
                    //this.periodoFilter1.monthCB.Items.Clear();
                    //this.periodoFilter1.monthCB1.Items.Clear();
                    //this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
                    //this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
                    //this.periodoFilter1.monthCB1.Items.Add(this._fechaUltimoCierre.Date.Day);
                    //this.periodoFilter1.monthCB1.SelectedItem = this._fechaUltimoCierre.Date.Day;

                    //#endregion

                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = true;
                    txtcliente.Visible = false;
                    txtDetalle.Visible = false;
                }




        }

        #endregion

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
             {
                 #region Carga filtros
                 uc_MasterFind txtpagaduria = (uc_MasterFind)this.RPControls["masterPagaduria"];
                 this._pagaduria = txtpagaduria.Value;
                 #endregion

                ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);
    
                string reportName = string.Empty;
                reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID,this._report,this._fechaIni,this._fechaFin,this._clienteID,null,string.Empty,string.Empty,
                                                                                       string.Empty,string.Empty,string.Empty,string.Empty,this._pagaduria,string.Empty,null,null,null);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CarteraPagaduria.cs-LoadReportMethod"));
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
