using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_GestionCobranza : ReportParametersForm
    {
        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaUltimoCierre;
        DTO_TxResult result = new DTO_TxResult();
        private string _reporte = "1";
        private string _etapa = string.Empty;
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cf;
            this.documentReportID = AppReports.ccGestionCobranza;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccReporteGEstion).ToString());
           
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tipo = new List<ReportParameterListItem>()
            { 
                new ReportParameterListItem() { Key = "7", Desc = "Compromisos Incumplidos" },
                new ReportParameterListItem() { Key = "8", Desc = "Llamadas para Normalización" }
            };
            this.AddList("tipoReporte", this.documentReportID.ToString(), tipo, true, "7");
            this.AddMaster("etapa", AppMasters.glIncumplimientoEtapa, true, null,true);
            this.AddCheck("ExcluirDemanda", "Excluir en Proceso Demanda",false);
            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            #endregion

            #region Obtiene ultima fecha de corte
            var ultimoCierre = this._bc.AdministrationModel.ccCierreDiaCartera_GetUltimoDiaCierre(string.Empty, null);
            if (ultimoCierre != null)
                this._fechaUltimoCierre = ultimoCierre.Fecha.Value.Value;
            else
                this._fechaUltimoCierre =this.periodo;

            #endregion
            this.btnExportToPDF.Visible = false;
            this.btnExportToXLS.Visible = true;
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public Reports_GestionCobranza()
        {
       
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
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] ;
            if(this.periodoFilter1.monthCB1.SelectedItem != null)
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + this.periodoFilter1.monthCB1.SelectedItem;

            this._fechaIni = Convert.ToDateTime(fechaIniString);

            ReportParameterList txtlibranzaF = (ReportParameterList)this.RPControls["tipoReporte"];
            this._reporte = txtlibranzaF.SelectedListItem.Key;

            uc_MasterFind etapaID = (uc_MasterFind)this.RPControls["etapa"];
            this._etapa = etapaID.Value;

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Exportar a Excel
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
                if (this.periodoFilter1.monthCB1.SelectedItem != null)
                    fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + this.periodoFilter1.monthCB1.SelectedItem;

                this._fechaIni = Convert.ToDateTime(fechaIniString);

                CheckEdit checkExcluir = (CheckEdit)this.RPControls["ExcluirDemanda"];
                ReportParameterList txtTipoReporte = (ReportParameterList)this.RPControls["tipoReporte"];
                this._reporte = txtTipoReporte.SelectedListItem.Key;

                uc_MasterFind etapaID = (uc_MasterFind)this.RPControls["etapa"];
                this._etapa = etapaID.Value;// se usa pagaduria para no modificar la funcion

                //Act Flujo Cobranza
                string actCobranza = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ActFlujoGestCobranza);

                this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, Convert.ToByte(this._reporte), this._fechaIni, this._fechaIni, string.Empty, null, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._etapa,checkExcluir.Checked.ToString(), null, null, Convert.ToByte(this._reporte) != 7?  this._etapa : actCobranza);
          
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Credito", "Report_XLS"));
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
            this.periodoFilter1.Visible = true;
            ReportParameterList txtTipoRep = (ReportParameterList)this.RPControls["tipoReporte"];
            uc_MasterFind etapaID = (uc_MasterFind)this.RPControls["etapa"];
            CheckEdit checkExcluir = (CheckEdit)this.RPControls["ExcluirDemanda"];
            checkExcluir.Visible = false;
            this._reporte = txtTipoRep.SelectedListItem.Key;

            this.btnExportToPDF.Visible = true;
            this.btnExportToPDF.Enabled = true;
            
            #region Asigna Fechas
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.SelectedItem = 1;
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            #endregion
            etapaID.Visible = true;
            if ( this._reporte.Equals("7"))
            {
                this.btnExportToXLS.Visible = true;
                this.btnExportToXLS.Enabled = true;
                this.btnExportToPDF.Visible = false;
                this.btnExportToPDF.Enabled = false;
            }
            else if (this._reporte.Equals("8"))
            {
                this.periodoFilter1.Visible = false;
                this.btnExportToXLS.Visible = true;
                this.btnExportToXLS.Enabled = true;
                this.btnExportToPDF.Visible = false;
                this.btnExportToPDF.Enabled = false;
            }
            else
            {
                this.btnExportToXLS.Visible = false;
                this.btnExportToXLS.Enabled = false;
            }   
            this._etapa = etapaID.Value;            
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
                string fileURl;
                CheckEdit checkExcluir = (CheckEdit)this.RPControls["ExcluirDemanda"];
                string reportname = this._bc.AdministrationModel.Report_Cc_GestionCobranza(this.documentReportID, this._reporte, this._fechaIni,this._etapa, checkExcluir.Checked);

                #region Generacion del reporte

                if (!string.IsNullOrEmpty(reportname))
                {
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportname);
                    Process.Start(fileURl);
                }
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReportSaldos.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion
    }
}
