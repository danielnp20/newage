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
    public class Reports_EstadoCesion : ParametersCarteraForm
    {
        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_EstadoCesion() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.documentReportID = AppReports.ccEstadoCesionCartera;
            base.InitReport();

            this.periodo = this.periodo.AddMonths(-1);
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedItem = this.periodo.Date.Month;
            this.btnExportToXLS.Visible = false;

            ReportParameterList txtAgrup = (ReportParameterList)this.RPControls["Agrupamiento"];
            txtAgrup.Visible = false;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            #region Valida fechas
            if (string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) || string.IsNullOrEmpty(this.periodoFilter1.txtYear1.Text))
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "La fecha es inválida"));
                return;
            }

            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.monthCB1.SelectedItem;
            this._fechaFin = Convert.ToDateTime(fechaFinString);
            this._fechaFin = new DateTime(this._fechaFin.Year,this._fechaFin.Month,DateTime.DaysInMonth(this._fechaFin.Year,this._fechaFin.Month));

            if (!string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) && !string.IsNullOrEmpty(this.periodoFilter1.monthCB.SelectedItem.ToString()))
            {
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.SelectedItem;
                this._fechaIni = Convert.ToDateTime(fechaIniString);
            }
            if (this._fechaIni != null)
            {
                if (this._fechaIni > this._fechaFin)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DateInvalid));
                    return;
                }
            }
            #endregion
            #region Carga el filtro por libranza(obligacion)
            ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
            if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
            #endregion
            #region Carga el filtro por Agrupamiento
            #endregion
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;

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
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;

                #region Valida fechas
                if (string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) || string.IsNullOrEmpty(this.periodoFilter1.txtYear1.Text))
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "La fecha es inválida"));
                    return;
                }
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.monthCB1.SelectedItem;
                this._fechaFin = Convert.ToDateTime(fechaFinString);
                this._fechaFin = new DateTime(this._fechaFin.Year, this._fechaFin.Month, DateTime.DaysInMonth(this._fechaFin.Year, this._fechaFin.Month));

                if (!string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) && !string.IsNullOrEmpty(this.periodoFilter1.monthCB.SelectedItem.ToString()))
                {
                    string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.monthCB.SelectedItem;
                    this._fechaIni = Convert.ToDateTime(fechaIniString);
                }
                if (this._fechaIni != null)
                {
                    if (this._fechaIni > this._fechaFin)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DateInvalid));
                        return;
                    }
                }
                #endregion

                #region Carga el filtro por libranza(obligacion)
                ReportParameterTextBox txtlibranzaF = (ReportParameterTextBox)this.RPControls["Obligacion"];
                if (!string.IsNullOrEmpty(txtlibranzaF.txtFrom.Text))
                    this._libranza = Convert.ToInt32(txtlibranzaF.txtFrom.Text);
                #endregion

                uc_MasterFind masterCOmp = (uc_MasterFind)this.RPControls["CompradorCarteraID"]; 
                this._compradorCatera = masterCOmp.ValidID ? masterCOmp.Value : string.Empty;

                DTO_ccCompradorCartera comp = (DTO_ccCompradorCartera)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCompradorCartera, false, this._compradorCatera, true);
                this._compradorCatera = comp != null ? comp.TerceroID.Value : string.Empty;

                if (this._tipoReporte == 6)//Valida si es el reporte de resumen de saldos
                    this._compradorCatera = comp != null ? comp.ID.Value : string.Empty;
                ReportParameterList txtAgrup = (ReportParameterList)this.RPControls["Agrupamiento"];
                if (txtAgrup.Visible == true)
                    this._agrupamiento = Convert.ToByte(txtAgrup.SelectedListItem.Key);


                this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                    this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, this._agrupamiento, this._rompimiento);

                if (this._Query.Rows.Count != 0)
                {
                    if (this._tipoReporte == 2) //Amortizacion Derechos
                    {
                        this._Query.Columns.Remove("DevolGiro"); 
                        this._Query.Columns.Remove("DerechosCed");
                        this._Query.Columns.Remove("RecursoDirecto");
                        this._Query.Columns.Remove("RecursoxCap");
                        this._Query.Columns.Remove("RecursoxInt"); 
                        this._Query.Columns.Remove("Recompra");
                        this._Query.Columns.Remove("SaldoInicial28");
                    }
                    else if (this._tipoReporte == 5) //Detalle Saldos
                    {
                        this._Query.Columns.Remove("SaldoTOT");
                    }
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_EstadoCesion", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
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
                uc_MasterFind masterCOmp = (uc_MasterFind)this.RPControls["CompradorCarteraID"];
                this._compradorCatera = masterCOmp.ValidID ? masterCOmp.Value : string.Empty;

                DTO_ccCompradorCartera comp = (DTO_ccCompradorCartera)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.ccCompradorCartera,false,this._compradorCatera,true);
                this._compradorCatera = comp != null ? comp.TerceroID.Value : string.Empty;

                if (this._tipoReporte == 6)//Valida si es el reporte de resumen de saldos
                    this._compradorCatera = comp != null ? comp.ID.Value : string.Empty;
                #region Carga el filtro por Agrupamiento
                ReportParameterList txtAgrup = (ReportParameterList)this.RPControls["Agrupamiento"];
                if (txtAgrup.Visible == true)
                    this._agrupamiento = Convert.ToByte(txtAgrup.SelectedListItem.Key);


                #endregion


                reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, this._tipoReporte, this._fechaIni, this._fechaFin, this._cliente, this._libranza, this._zona, this._ciudad,
                                                                                  this._concesionario, this._asesor, this._lineaCredi, this._compradorCatera, string.Empty, string.Empty, this._agrupamiento, this._rompimiento, this._formatType);

                if (!string.IsNullOrEmpty(reportName))
                {
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_EstadoCesion.cs-LoadReportMethod"));
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
