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
    public class Reports_RecaudosNominaDeta : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                if (!string.IsNullOrEmpty(this._centroPago))
                {
                    string reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(this.documentReportID, null, null, this._fechaFin, string.Empty, null, string.Empty, string.Empty, string.Empty, string.Empty
                                , string.Empty, string.Empty, this._estadoCruce, this._centroPago, null, null, ExportFormatType.pdf);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                    
                }
                else
                    MessageBox.Show("Debe digitar una Pagaduria(CP)");

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_RrecaudosNominaDeta.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variable para reporte
        DateTime _fechaFin;

        //Cariables para filtros
        private string _centroPago = "";
        private string _estadoCruce = "";

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_RecaudosNominaDeta() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.documentReportID = AppReports.ccRecaudosNominaDeta;
            this.Text = "Recaudos por Nómina";

            #region Configurar Opciones

            #region Carga las lista que se van a mostrar en los combos

            List<ReportParameterListItem> estadoCruce = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "0", Desc = "-"}, 
                new ReportParameterListItem() { Key = "1", Desc = "Cruce correcto"}, 
                new ReportParameterListItem() { Key = "2", Desc = "No Opero Inc. Previa"}, 
                new ReportParameterListItem() { Key = "3", Desc = "No Opero Inc. Liquidacion"}, 
                new ReportParameterListItem() { Key = "4", Desc = "Opero por Valor Diferente"}, 
                new ReportParameterListItem() { Key = "5", Desc = "Dejo de Operar"}, 
                new ReportParameterListItem() { Key = "6", Desc = "Valor diferente"}, 
                new ReportParameterListItem() { Key = "7", Desc = "Pago Atrasado"}, 
                new ReportParameterListItem() { Key = "8", Desc = "Desc. Sin saldo"}, 
                new ReportParameterListItem() { Key = "9", Desc = "Solicitud"}, 
                new ReportParameterListItem() { Key = "10", Desc = "Opero Adelantado"}
            };
            #endregion

            //Inicializa los Controles del Fomulario    
            this.AddMaster("CentroPagoID", AppMasters.ccCentroPagoPAG, true, null);
            this.AddList("EstadoCruce", (AppReports.ccRecaudosNominaDeta) + "_EstadoCruce", estadoCruce, false, "0", true);
            #endregion);

            #region Configuracion Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            this.btnExportToXLS.Visible = true;
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
            this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + DateTime.Today.Day;
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
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + DateTime.Today.Day;
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                this._Query = this._bc.AdministrationModel.Report_Cc_CarteraToExcel(this.documentReportID, null, null, this._fechaFin, string.Empty, null, string.Empty, string.Empty, string.Empty,
                                                                                 string.Empty, string.Empty, string.Empty, this._estadoCruce, this._centroPago,null, null);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Proyecciones", "Report_XLS"));
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
            #region Carga el filtro por CentroPagoID

            if (option.Equals("CentroPagoID"))
            {
                uc_MasterFind masterCentroPag = (uc_MasterFind)sender;
                this._centroPago = masterCentroPag.ValidID ? masterCentroPag.Value : string.Empty;
            }
            #endregion
            #region Carga el filtro por Estado Cruce

            if (option.Equals("EstadoCruce"))
            {
                ReportParameterList estadoCruce = (ReportParameterList)this.RPControls["EstadoCruce"];
                this._estadoCruce = estadoCruce.SelectedListItem.Key;
            }

            #endregion        
        }

        #endregion

    }
}
