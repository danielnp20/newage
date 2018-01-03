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
    class Reports_PresupuestoContable : ReportParametersForm

    {
        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        private string _proyecto = string.Empty;
        private string _centroCto = string.Empty;
        private DTO_TxResult reportName;
        private string fileURl;

        //variables para opciones
        private byte _rompimiento = 1;
        private string _moneda = "1";
        private string _libro = "";

        #endregion

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string  reportName = this._bc.AdministrationModel.ReportesContabilidad_EjecucionPresupuestal(this._fechaIni,this._rompimiento, this._proyecto,this._centroCto,this._libro,this._moneda);

                #region Genera el reporte

                //Genera el PDF
                if (!string.IsNullOrEmpty(reportName))
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_PresupuestoContable.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que restablecer los valores iniciales de cada filtro
        /// </summary>
        private void cleanData()
        {
            this._proyecto = string.Empty;
        }

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

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coEjecucionPresupuesto).ToString());
            this._libro = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos
            //Items para el rompimiento
            List<ReportParameterListItem> monedas = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Local"},
                new ReportParameterListItem() { Key = "2", Desc = "Extranjera"}
            };

            //Items para el rompimiento
            List<ReportParameterListItem> rompimiento = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Consolidado"},
                new ReportParameterListItem() { Key = "2", Desc = "Por Centro Costo"},
                new ReportParameterListItem() { Key = "3", Desc = "Por Proyecto"}
            };
            #endregion

            //Crea y carga los controles respectivamente  
            this.AddList("rompimiento", "Tipo Reporte", rompimiento, true, "1");
            this.AddList("monedaTipo", "Tipo Moneda", monedas, true, "1");          
            this.AddMaster("proyecto", AppMasters.coProyecto, true, null,false);
            this.AddMaster("centroCto", AppMasters.coCentroCosto, true, null,false);

            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.EnableControls();
            #endregion
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public Reports_PresupuestoContable()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.plEjecucionPresupuestal;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                string fechaIniString;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_PresupuestoContable.cs", "Report_PDF"));
            }
        }

        /// <summary>
        /// General el reporte de Ejecucion presupuestal por origen
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                string fechaIniString;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                //this._Query = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalXOrigen(this._fechaIni, this._tipoProyecto, this._rompimiento, this._proyecto,
                //    this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_PresupuestoContable.cs", "Report_XLS"));
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
            #region Carga el Rompimiento del reporte

            if (option.Equals("rompimiento"))
            {
                ReportParameterList rompimiento = (ReportParameterList)this.RPControls["rompimiento"];
                uc_MasterFind masterProy = (uc_MasterFind)this.RPControls["proyecto"];
                uc_MasterFind masterCentro = (uc_MasterFind)this.RPControls["centroCto"];
                this._rompimiento =  Convert.ToByte(rompimiento.SelectedListItem.Key);
                if (this._rompimiento == 1)
                {
                    masterProy.Visible = false;
                    masterCentro.Visible = false;
                }
                else if (this._rompimiento == 2)
                {
                    masterProy.Visible = false;
                    masterCentro.Visible = true;
                }
                else if (this._rompimiento == 3)
                {
                    masterProy.Visible = true;
                    masterCentro.Visible = false;
                }
            }
            else if (option.Equals("monedaTipo"))
            {
                ReportParameterList rompimiento = (ReportParameterList)this.RPControls["monedaTipo"];
                this._moneda = rompimiento.SelectedListItem.Key;
            }

            #endregion   
            #region Carga las maestras

            else if (option.Equals("proyecto"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)sender;
                this._proyecto = masterProyecto.ValidID ? masterProyecto.Value : string.Empty;
            }
            else if (option.Equals("centroCto"))
            {
                uc_MasterFind masterCentro = (uc_MasterFind)sender;
                this._centroCto = masterCentro.ValidID ? masterCentro.Value : string.Empty;
            }

            #endregion
        }

        #endregion
    }
}
