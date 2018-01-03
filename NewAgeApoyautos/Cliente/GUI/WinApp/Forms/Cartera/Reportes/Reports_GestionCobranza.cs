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
        DateTime _fechaFin;
        DTO_TxResult result = new DTO_TxResult();
        private string _Reporte = "1";
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cf;
            this.documentReportID = AppReports.ccGestionCobranza;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (this.documentReportID).ToString());
           
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tipo = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Diario" },
                new ReportParameterListItem() { Key = "2", Desc = "Mensual" },
                new ReportParameterListItem() { Key = "3", Desc = "En proceso Demanda" },
            };
            this.AddList("tipoReporte", this.documentReportID.ToString(), tipo, true, "1");
            //this.AddMaster("masterCliente", AppMasters.ccCliente, true, null);
            //this.AddMaster("Asesor", AppMasters.ccAsesor, true, null, true);
            //this.AddMaster("Pagaduria", AppMasters.ccPagaduria, true, null, true);
            //this.AddTextBox("libranza", false, (AppForms.ReportForm).ToString() + "_Libranza", true);
            //this.AddCheck("Excel", (AppForms.ReportForm).ToString() + "_Exportar", true);
            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            #endregion
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

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            this._fechaIni = Convert.ToDateTime(fechaIniString);

            ReportParameterList txtlibranzaF = (ReportParameterList)this.RPControls["tipoReporte"];
            this._Reporte = txtlibranzaF.SelectedListItem.Key;

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
            ReportParameterList txtlibranzaF = (ReportParameterList)this.RPControls["tipoReporte"];
            this._Reporte = txtlibranzaF.SelectedListItem.Key;
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
                string reportname = this._bc.AdministrationModel.Report_Cc_GestionCobranza(this.documentReportID, this._Reporte, this._fechaIni);

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
