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
using System.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_Presupuesto : ReportParametersForm
    {
        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        private string _proyecto = string.Empty;

        //variables para opciones
        private string _tipoReporte = "meses";
        private string _meneda = "local";
        private bool _isConsolidado = false;
        private bool _TipoMoneda = true;

        #endregion

        #region Funciones Privadas

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
        /// Form Constructor
        /// </summary>
        public Reports_Presupuesto()
        {
            this.Module = ModulesPrefix.pl;
            this.ReportForm = AppReportParametersForm.plPresupuesto;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.pl;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false; 
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.plPresupuesto).ToString());

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del tipo de reporte
            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>()
            {
                            new ReportParameterListItem() { Key = "meses", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PorMeses) },
                            new ReportParameterListItem() { Key = "acumulado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Acumulado) }
            };

            //Items para el combo de Moneda
            List<ReportParameterListItem> moneda = new List<ReportParameterListItem>()
            {
                            new ReportParameterListItem() { Key = "local", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Local) },
                            new ReportParameterListItem() { Key = "extr", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Extranjera) }
            };

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("tipoReporte", (AppForms.ReportForm).ToString() + "_tipoReport", tipoReporte, true, "meses");
            this.AddList("tipoMoneda", (AppForms.ReportForm).ToString() + "_Moneda", moneda, true, "local");
            this.AddMaster("proyecto", AppMasters.coProyecto, true, null);
            this.AddCheck("consolidado", (AppForms.ReportForm).ToString() + "_consolidado", true);

            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();

            this.EnableControls();

            #endregion
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                string fechaIniString;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                if (this._tipoReporte == "acumulado")
                {
                    this.periodoFilter1.Year[0].ToString();
                    fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                    this._fechaIni = Convert.ToDateTime(fechaIniString);

                    this._Query = this._bc.AdministrationModel.ReportesPlaneacion_PresupuestoAcumulado(this._fechaIni, this._proyecto, true, false, false);
                }
                else
                {
                    this.periodoFilter1.Year[0].ToString();
                    fechaIniString = this.periodoFilter1.txtYear.Text + " / " + DateTime.Now.Month + " / " + DateTime.Now.Day;
                    this._fechaIni = Convert.ToDateTime(fechaIniString);

                    this._Query = this._bc.AdministrationModel.ReportesPlaneacion_PresupuestoAcumulado(this._fechaIni, this._proyecto, false, _TipoMoneda, _isConsolidado);
                }

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Presupuesto.cs", "Report_XLS"));
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
            #region Carga el tipo de reporte a imprimir

            if (option.Equals("tipoReporte"))
            {
                ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["tipoReporte"];
                this._tipoReporte = tipoReporte.SelectedListItem.Key;
                uc_MasterFind masterProyecto = (uc_MasterFind)this.RPControls["proyecto"];
                ReportParameterList moneda = (ReportParameterList)this.RPControls["tipoMoneda"];
                CheckEdit consoli = (CheckEdit)this.RPControls["consolidado"];

                switch (_tipoReporte)
                {
                    //Carga los filtros del periodo para el reporte de mes
                    case ("meses"):

                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                      
                        masterProyecto.Visible = true;
                        moneda.Visible = true;
                        consoli.Visible = true;

                        this.EnableControls();
                        break;

                    //Carga los filtros del perioso para el reporte Acumulado 
                    default:

                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                       
                        masterProyecto.Visible = false;
                        moneda.Visible = false;
                        consoli.Visible = false;
                        masterProyecto.Value = string.Empty;
                        this._proyecto = string.Empty;

                        this.EnableControls();
                        break;
                }

            }

            #endregion
            #region Carga el tipo de moneda en que se quiere imprimir los reportes

            if (option.Equals("tipoMoneda"))
            {
                ReportParameterList tipoMoneda = (ReportParameterList)this.RPControls["tipoMoneda"];
                this._meneda = tipoMoneda.SelectedListItem.Key;

                if (this._meneda  == "extr")
                    this._TipoMoneda = false;
                else
                     this._TipoMoneda = true;
            }

            #endregion
            #region Carga el proyecto que se desea verificar

            if (option.Equals("proyecto"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)sender;
                this._proyecto = masterProyecto.ValidID ? masterProyecto.Value : string.Empty;
            }

            #endregion
            #region Verifica si quiere consolidar el reporte

            if (option.Equals("consolidado"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();

                switch (reportParameters["consolidado"][0])
                {
                    case "True":
                        this._isConsolidado = true;
                        break;

                    case "False":
                        this._isConsolidado = false;
                        break;
                }
            }

            #endregion
        }

        #endregion

    }
}
