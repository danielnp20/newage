using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteOrdenCompras : ReportParametersForm
    {

        #region Hilos
        /// <summary>
        /// Hilo, Genera el PDF
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                int estado = Convert.ToInt16(this._estado);
                
                if (this._report == "resumido")
                    reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompras(this._fechaIni, this._fechaFin, this._provedor, new Dictionary<int, string>(), false, this._moneda, this._formatType);
                else
                    reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenComprasDetallada(this._fechaIni, this._fechaFin, this._provedor, new Dictionary<int, string>(), true, this._moneda, this._formatType);

                #region Genera el  El reporte

                //Genera el PDF
                if (reportName.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }

                //Verifica q se halla ejecutado con exito la generacion del PDF para mostrarlo al usuario
                if (!string.IsNullOrEmpty(fileURl))
                    Process.Start(fileURl);
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
                    return;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteOrdenCompras.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Variable
        private string _estado = "10";
        private string _provedor = string.Empty;
        private string _moneda = string.Empty;
        private DTO_TxResult reportName;
        private string fileURl;
        private string _report = "resumido";

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;


        #endregion

        #region Funciones Privadad

        /// <summary>
        /// Funcion que se encarga de desahbilitar los controles que no se utilizan
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
        /// Inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.pr;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.prOrdenCompras).ToString());

            #region Configurar Opciones

            #region Opciones de los combos

            //Carga la Informacion del combo para saber el estado de las facturas
            List<ReportParameterListItem> estado = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem(){Key ="10", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_All)},
                new ReportParameterListItem(){Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Aprobadas)},
                new ReportParameterListItem(){Key = "0", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Annulled)}
            };

            //Carga el combo tipo Moneda
            List<ReportParameterListItem> moneda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem(){Key="Loc", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal ) },
                new ReportParameterListItem(){Key="Ext", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign ) },
                new ReportParameterListItem(){Key="Ambas", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth ) }
            };

            //Carga el tipo de reporte
            List<ReportParameterListItem> reporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem(){Key="detallado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed ) },
                new ReportParameterListItem(){Key="resumido", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Summarized ) }
            };


            #endregion

            //Opciones para impresion del reporte
            this.AddList("reporte", (AppForms.ReportForm).ToString() + "_tipoReport", reporte, true, "resumido");
            this.AddList("estado", (AppForms.ReportForm).ToString() + "_Estado", estado, true, "Todas");
            this.AddList("moneda", (AppForms.ReportForm).ToString() + "_Moneda", moneda, true, "Ambas");
            this.AddMaster("proveedor", AppMasters.prProveedor, true, null);

            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString(); 
            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year+1).ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(1);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;

            this.EnableControls();
            #endregion
        }

        /// <summary>
        /// Form Constructer (for Libro Diario Report)
        /// </summary>
        public ParametersReporteOrdenCompras()
        {
            this.Module = ModulesPrefix.pr;
            this.ReportForm = AppReportParametersForm.prOrdenCompras;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            if (!string.IsNullOrEmpty(this.periodoFilter1.txtYear.Text) && this.periodoFilter1.Months[0] != null)
            {
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Enabled = false;
                this.periodoFilter1.txtYear.Enabled = false;

                int añoInicial = Convert.ToInt16(this.periodoFilter1.txtYear.Text);
                int añoFinal = Convert.ToInt16(this.periodoFilter1.txtYear1.Text);
                int mesIni = this.periodoFilter1.Months[0];
                int mesFin = this.periodoFilter1.Months[1];

                this._fechaIni = new DateTime(añoInicial, mesIni, 1);
                this._fechaFin = new DateTime(añoFinal, mesFin, DateTime.DaysInMonth(añoFinal, mesFin));

               this.LoadReportMethod_PDF();
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Funcion para el manejo de enventos
        /// </summary>
        /// <param name="option"></param>
        /// <param name="sender"></param>
        protected override void ListValueChanged(string option, object sender)
        {
            #region Opcion Reporte

            if (option.Equals("reporte"))
            {
                ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["reporte"];
                this._report = tipoReporte.SelectedListItem.Key;
            }

            #endregion
            #region Opcion estado

            if (option.Equals("estado"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["estado"];
                this._estado = listReportType.SelectedListItem.Key;
            }

            #endregion
            #region Opcion Moneda

            if (option.Equals("moneda"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["moneda"];
                this._moneda = listReportType.SelectedListItem.Key;
            }

            #endregion
            #region opcion proveedor
            if (option.Equals("proveedor"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._provedor = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

        }

        #endregion
    }
}
