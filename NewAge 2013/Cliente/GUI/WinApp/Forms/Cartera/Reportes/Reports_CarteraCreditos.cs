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
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_CarteraCreditos : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = null;
        public DateTime _fechaIni = DateTime.Now;
        public string _pagaduria = string.Empty;
        public DateTime _fechaFin = DateTime.Now;
        DateTime _fechaUltimoCierre;
        public byte _report = 1;
        public byte _detalle = 1;
        public int? _libranza;
        public byte _congiro = 0;
        
        ReportParameterTextBox libranza;
        
        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Funcion que se encarga de Inhabilitar los controles que no son necesarios para este reporte
        /// </summary>
        private void EnableControls()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            
            this.btnResetFilter.Visible = false;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructor
        /// </summary>
        public Reports_CarteraCreditos()
        {
            this.Module = ModulesPrefix.cf;
            
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {                   
            this.Module = ModulesPrefix.cf;
            this.documentReportID = AppReports.ccReporteCredito;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = true;
            this.btnExportToPDF.Enabled = false;
            this.btnExportToXLS.Enabled = true;
            this.btnFilter.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccReporteCredito).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Relacion Creditos" },     
                new ReportParameterListItem() { Key = "2", Desc = "Extracto" },     
            };
            this.AddList("tipoReporte", "Tipo Reporte", ListReport, true, "1");
            this.AddMaster("TipoCredito", AppMasters.ccTipoCredito, true, null, true);
            
            this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", false);
            this.AddCheck("congiro", "Libranzas con Giro", true);

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
            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["libranza"];
            if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
            {
                this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
            }
            else
            {
                MessageBox.Show("Debe digitar un numero de libranza");
                return;
            }

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
                ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["libranza"];
                if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                    this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);

                    this.periodoFilter1.Enabled = false;
                    this.btnExportToPDF.Enabled = false;
                    this.btnExportToXLS.Enabled = false;

                    this.periodoFilter1.Year[0].ToString();
                    DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
                    this._fechaIni = fechaIniString;
                    this._fechaFin = fechaIniString;
                    ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                    uc_MasterFind masterTipoCredito = (uc_MasterFind)this.RPControls["TipoCredito"];

                    if (string.IsNullOrEmpty(masterTipoCredito.Value))
                    {
                        CheckEdit chkgiro = (CheckEdit)this.RPControls["congiro"];
                        if (!chkgiro.Checked)
                            this._congiro = 0;
                        else
                            this._congiro = 1;
                    }
                    else
                        this._congiro = 0;

                    this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);
                    
                    this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._report, null, this._fechaFin, this._clienteID, this._libranza, string.Empty,
                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._pagaduria, string.Empty, null, this._congiro, (masterTipoCredito.ValidID ? masterTipoCredito.Value : null));

                    //Exporta Reporte a Excel.
                    if (this._Query.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(this._Query);
                        frm.Show();
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    this.btnExportToPDF.Enabled = false;
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_PolizaEstado", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #region Eventos

         //<summary>
         //Changes in the form depending on user's operations
         //</summary>
        protected override void ListValueChanged(string option, object sender)
        {

            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["libranza"];

            ReportParameterList txtDetalle = (ReportParameterList)this.RPControls["tipoReporte"];

            CheckEdit chkgiro = (CheckEdit)this.RPControls["congiro"];

              this._detalle = Convert.ToByte(txtDetalle.SelectedListItem.Key);
              if (this._detalle == 1 )
              {
                  this.periodoFilter1.Visible = true;
                  this.btnExportToPDF.Enabled = false;
                  this.btnExportToXLS.Enabled = true;
                  txtlibranzaF.Visible = false;
                  chkgiro.Visible = true;
              }
              else
              {
                  this.periodoFilter1.Visible = false;
                  this.btnExportToPDF.Enabled = true;
                  this.btnExportToXLS.Enabled = false;
                  txtlibranzaF.Visible = true;
                  chkgiro.Visible = false;
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
//                 libranza = (ReportParameterTextBox)this.RPControls["libranza"];
                ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["libranza"];
                if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                    this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
                

        
                ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                this._report = Convert.ToByte(txtTipo.SelectedListItem.Key);


    
                string reportName = string.Empty;
                reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID,this._report,this._fechaIni,this._fechaFin,this._clienteID,this._libranza,string.Empty,string.Empty,
                                                                                       string.Empty,string.Empty,string.Empty,string.Empty,this._pagaduria,string.Empty,null,null,null);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_CarteraCreditos.cs-LoadReportMethod"));
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
