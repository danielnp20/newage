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
    class Reports_EjecucionPresupuesto : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                switch (_Rompimiento)
                {
                    case "LinRecur":
                        reportName = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalLineaXRecurso(this._fechaIni, this._tipoProyecto, this._Rompimiento,
                            this._proyecto, this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo, moneda, this._formatType);
                        break;

                    case "RecurAct":
                        reportName = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalRecursoXActividad(this._fechaIni, this._tipoProyecto, this._Rompimiento,
                            this._proyecto, this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo, moneda, this._formatType);
                        break;

                    case "LineCosto":
                        reportName = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalLineaXCentroCto(this._fechaIni, this._tipoProyecto, this._Rompimiento,
                            this._proyecto, this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo, moneda, this._formatType);
                        break;

                    default:
                        reportName = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalProyectoxActividad(this._fechaIni, this._tipoProyecto, this._Rompimiento,
                            this._proyecto, this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo, moneda, this._formatType);
                        break;
                }

                #region Genera el reporte

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_EjecucionPresupuesto.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        private string _proyecto = string.Empty;
        private DTO_TxResult reportName;
        private string fileURl;
        private bool moneda = true;
        private string _actividad = string.Empty;
        private string _lineaPresu = string.Empty;
        private string _centroCosto = string.Empty;
        private string _recursoGrupo = string.Empty;

        //variables para opciones
        private string _Rompimiento = "ProyAct";
        private string _moneda = "local";
        private string _tipoProyecto = "*";        
        private string _etapa = string.Empty;
        private string _typeReport = "Moneda";
       
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que restablecer los valores iniciales de cada filtro
        /// </summary>
        private void cleanData()
        {
            this._proyecto = string.Empty;
            this._actividad = string.Empty;
            this._lineaPresu = string.Empty;
            this._centroCosto = string.Empty;
            this._recursoGrupo = string.Empty;
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
            this.Module = ModulesPrefix.pl;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.plEjecucionPresupuestal).ToString());
            this.btnExportToPDF.Visible = false;
            this.btnExportToXLS.Visible = false;

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para tipo de reporte a mostrar
            List<ReportParameterListItem> typeReprot = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "-", Desc = "-"},
                new ReportParameterListItem() { Key = "Moneda", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PorMoneda)},
                new ReportParameterListItem() { Key = "Origen", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PorOrigen)}
            };

            //Items para el rompimiento
            List<ReportParameterListItem> rompimiento = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "ProyAct", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ProyectoActividad)},
                new ReportParameterListItem() { Key = "LinRecur", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineasRecurso)},
                new ReportParameterListItem() { Key = "RecurAct", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RecursoActividad)},
                new ReportParameterListItem() { Key = "LineCosto", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineaCentroCost)}
            };

            //Items para el combo de Moneda
            List<ReportParameterListItem> moneda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "local", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Local) },
                new ReportParameterListItem() { Key = "extr", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Extranjera) }
            };

            //Items para el combo de tipo de proyecto
            List<ReportParameterListItem> tipoProyecto = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "*", Desc = "*" },
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Capex)},
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Opex)},
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inversion)},
                new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Administrativo)},
                new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inventarios)},
                new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CapTrabajo)},
                new ReportParameterListItem() { Key = "7", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Distribucion)}
            };

            #endregion

            //Crea y carga los controles respectivamente       _tipoReport
            this.AddList("typeReport", (AppForms.ReportForm).ToString() + "_tipoReport", typeReprot, true, "-");
            this.AddList("rompimiento", (AppForms.ReportForm).ToString() + "_lblRompimiento", rompimiento, true, "ProyAct");
            this.AddList("tipoMoneda", (AppForms.ReportForm).ToString() + "_Moneda", moneda, true, "local");
            this.AddList("tipoProyecto", (AppForms.ReportForm).ToString() + "_tipoProy", tipoProyecto, true, "*");
            this.AddMaster("proyecto", AppMasters.coProyecto, true, null);
            this.AddMaster("actividad", AppMasters.coActividad, true, null);
            this.AddMaster("lienaPres", AppMasters.plLineaPresupuesto, true, null, false);
            this.AddMaster("centroCos", AppMasters.coCentroCosto, true, null, false);
            this.AddMaster("recurGrupo", AppMasters.plRecursoGrupo, true, null, false);

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
        public Reports_EjecucionPresupuesto()
        {
            this.Module = ModulesPrefix.pl;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_EjecucionPresupuesto.cs", "Report_PDF"));
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
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                this._Query = this._bc.AdministrationModel.ReportesPlaneacion_EjecucionPresupuestalXOrigen(this._fechaIni, this._tipoProyecto, this._Rompimiento, this._proyecto,
                    this._actividad, this._lineaPresu, this._centroCosto, this._recursoGrupo);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_EjecucionPresupuesto.cs", "Report_XLS"));
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
            ReportParameterList tipoMoneda = (ReportParameterList)this.RPControls["tipoMoneda"];

            #region Carga el tipo de reporte a verificar
            if (option.Equals("typeReport"))
            {
                ReportParameterList reporte = (ReportParameterList)this.RPControls["typeReport"];
                this._typeReport = reporte.SelectedListItem.Key;
                if (this._typeReport == "Moneda")
                {
                    this.btnExportToPDF.Visible = true;
                    this.btnExportToPDF.Enabled = true;
                    this.btnExportToXLS.Visible = false;
                    this.btnExportToXLS.Enabled = false;
                    tipoMoneda.Visible = true;
                }
                else if (this._typeReport == "Origen")
                {
                    this.btnExportToXLS.Visible = true;
                    this.btnExportToXLS.Enabled = true;
                    this.btnExportToPDF.Visible = false;
                    this.btnExportToPDF.Enabled = false;
                    tipoMoneda.Visible = false;
                }
                else
                {
                    this.btnExportToXLS.Visible = false;
                    this.btnExportToXLS.Enabled = false;
                    this.btnExportToPDF.Visible = false;
                    this.btnExportToPDF.Enabled = false;
                }
            }

            #endregion
            #region Carga el Rompimiento del reporte

            if (option.Equals("rompimiento"))
            {
                ReportParameterList rompimiento = (ReportParameterList)this.RPControls["rompimiento"];
                this._Rompimiento = rompimiento.SelectedListItem.Key;
                uc_MasterFind proyect = (uc_MasterFind)this.RPControls["proyecto"];
                uc_MasterFind actividad = (uc_MasterFind)this.RPControls["actividad"];
                uc_MasterFind lineaPres = (uc_MasterFind)this.RPControls["lienaPres"];
                uc_MasterFind centroCos = (uc_MasterFind)this.RPControls["centroCos"];
                uc_MasterFind recurGrupo = (uc_MasterFind)this.RPControls["recurGrupo"];

                switch (this._Rompimiento)
                {
                    case "ProyAct":

                        // Habilida y Deshabilita los master necesario para cada Reporte
                        proyect.Visible = true;
                        actividad.Visible = true;
                        lineaPres.Visible = false;
                        centroCos.Visible = false;
                        recurGrupo.Visible = false;

                        //Limpia los controles
                        proyect.Value = string.Empty;
                        actividad.Value = string.Empty;
                        lineaPres.Value = string.Empty;
                        centroCos.Value = string.Empty;
                        recurGrupo.Value = string.Empty;
                        this.cleanData();                      

                        break;
                    case "LinRecur":

                        // Habilida y Deshabilita los master necesario para cada Reporte
                        proyect.Visible = false;
                        actividad.Visible = false;
                        lineaPres.Visible = true;
                        centroCos.Visible = false;
                        recurGrupo.Visible = true;

                        //Limpia los controles
                        proyect.Value = string.Empty;
                        actividad.Value = string.Empty;
                        lineaPres.Value = string.Empty;
                        centroCos.Value = string.Empty;
                        recurGrupo.Value = string.Empty;
                        this.cleanData();

                        break;
                    case "RecurAct":

                        // Habilida y Deshabilita los master necesario para cada Reporte
                        proyect.Visible = false;
                        actividad.Visible = true;
                        lineaPres.Visible = false;
                        centroCos.Visible = false;
                        recurGrupo.Visible = true;

                        //Limpia los controles
                        proyect.Value = string.Empty;
                        actividad.Value = string.Empty;
                        lineaPres.Value = string.Empty;
                        centroCos.Value = string.Empty;
                        recurGrupo.Value = string.Empty;
                        this.cleanData();

                        break;
                    case "LineCosto":

                        // Habilida y Deshabilita los master necesario para cada Reporte
                        proyect.Visible = false;
                        actividad.Visible = false;
                        lineaPres.Visible = true;
                        centroCos.Visible = true;
                        recurGrupo.Visible = false;

                        //Limpia los controles
                        proyect.Value = string.Empty;
                        actividad.Value = string.Empty;
                        lineaPres.Value = string.Empty;
                        centroCos.Value = string.Empty;
                        recurGrupo.Value = string.Empty;
                        this.cleanData();

                        break;
                }
            }

            #endregion
            #region Carga el tipo de moneda en que se quiere imprimir los reportes

            if (option.Equals("tipoMoneda"))
            {
                this._moneda = tipoMoneda.SelectedListItem.Key;

                if (this._moneda.Equals("local"))
                    moneda = true;
                else
                    moneda = false;
            }

            #endregion
            #region Cargar el tipo de proyeto

            if (option.Equals("tipoProyecto"))
            {
                ReportParameterList tipoProy = (ReportParameterList)this.RPControls["tipoProyecto"];
                this._tipoProyecto = tipoProy.SelectedListItem.Key;

            }

            #endregion
            #region Carga el proyecto que se desea verificar

            if (option.Equals("proyecto"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)sender;
                this._proyecto = masterProyecto.ValidID ? masterProyecto.Value : string.Empty;
            }

            #endregion
            #region Carga la Actividad que se desea verificar

            if (option.Equals("actividad"))
            {
                uc_MasterFind masterActividad = (uc_MasterFind)sender;
                this._actividad = masterActividad.ValidID ? masterActividad.Value : string.Empty;
            }

            #endregion
            #region Carga el Linea Presupuestal que se desea verificar

            if (option.Equals("lienaPres"))
            {
                uc_MasterFind masterLienaPresu = (uc_MasterFind)sender;
                this._lineaPresu = masterLienaPresu.ValidID ? masterLienaPresu.Value : string.Empty;
            }

            #endregion
            #region Carga el Centro Costo que se desea verificar

            if (option.Equals("centroCos"))
            {
                uc_MasterFind masterCentroCosto = (uc_MasterFind)sender;
                this._centroCosto = masterCentroCosto.ValidID ? masterCentroCosto.Value : string.Empty;
            }

            #endregion
            #region Carga el Grupo Recurso que se desea verificar

            if (option.Equals("recurGrupo"))
            {
                uc_MasterFind masterRecurGrupo = (uc_MasterFind)sender;
                this._recursoGrupo = masterRecurGrupo.ValidID ? masterRecurGrupo.Value : string.Empty;
            }

            #endregion
        }

        #endregion
    }
}
