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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersReportSerial : ReportParametersForm
    {

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;

                switch (_rompimientoSerial)
                {
                    case "1":

                        if (_rompimiento == "1")
                        {
                            reportName = this._bc.AdministrationModel.ReportesInventarios_SerialxBodega(this._fechaIni.Year, this._fechaIni.Month, this._bodega,
                                this._tipoBodega, this._referencia, this._grupo, this._clase, this._tipo, this._serie, this._material, _isSerializable, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        else
                        {
                            reportName = this._bc.AdministrationModel.ReportesInventarios_SerialxReferencia(this._fechaIni.Year, this._fechaIni.Month, this._bodega,
                                this._tipoBodega, this._referencia, this._grupo, this._clase, this._tipo, this._serie, this._material, _isSerializable, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        break;

                    case "2":
                        if (_rompimiento == "1")
                        {
                            reportName = this._bc.AdministrationModel.ReportesInventarios_SerialxBodegaCosto(this._fechaIni.Year, this._fechaIni.Month, this._bodega,
                                this._tipoBodega, this._referencia, this._grupo, this._clase, this._tipo, this._serie, this._material, _isSerializable,this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        else
                        {
                             reportName = this._bc.AdministrationModel.ReportesInventarios_SerialxReferenciaCosto(this._fechaIni.Year, this._fechaIni.Month, this._bodega,
                                this._tipoBodega, this._referencia, this._grupo, this._clase, this._tipo, this._serie, this._material, _isSerializable,string.Empty, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReportSerial.cs", "LoadReportMethod"));
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
        DateTime _fechaFin;
        private string _rompimientoSerial = "1";
        private string _rompimiento = "1";
        private bool _parametro = false;
        private string _bodega = "";
        private string _tipoBodega = "";
        private string _referencia = "";
        private string _grupo = "";
        private string _clase = "";
        private string _tipo = "";
        private string _serie = "";
        private string _material = "";
        private bool _isSerializable = true;

        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.inSerial).ToString());

            #region Configurar Opciones

            //Items para el Combo del primer rompimiento
            List<ReportParameterListItem> tipoRompimientoCosto = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SinCostos) },
                            new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ConCostos) } };

            //Items para el Combo del segundo rompimiento
            List<ReportParameterListItem> tipoRompimiento = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Bodega) },
                            new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Referencia) } };


            //Crea y carga los controles respectivamente
            this.AddList("1", (AppForms.ReportForm).ToString() + "_tipoReport", tipoRompimientoCosto, true, "1");
            this.AddList("2", (AppForms.ReportForm).ToString() + "_Rompimiento", tipoRompimiento, true, "1");
            this.AddCheck("3", (AppForms.ReportForm).ToString() + "_xBodega");
            this.AddMaster("4", AppMasters.inBodega, true, null, false);
            this.AddMaster("5", AppMasters.inBodegaTipo, true, null, false);
            this.AddCheck("6", (AppForms.ReportForm).ToString() + "_xReferencia");
            this.AddMaster("7", AppMasters.inReferencia, true, null, false);
            this.AddMaster("8", AppMasters.inRefGrupo, true, null, false);
            this.AddMaster("9", AppMasters.inRefClase, true, null, false);
            this.AddMaster("10", AppMasters.inRefTipo, true, null, false);
            this.AddMaster("11", AppMasters.inSerie, true, null, false);
            this.AddMaster("12", AppMasters.inMaterial, true, null, false);

            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;

            #endregion
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public ParametersReportSerial()
        {
            this.Module = ModulesPrefix.@in;
            this.ReportForm = AppReportParametersForm.inSerial;
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
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            //string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            //this._fechaFin = Convert.ToDateTime(fechaFinString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }
        #endregion

        #region Funciones Privadas
        private DTO_glConsultaFiltro Filtro(string campoFisico, string operadorFiltro, string operadorSentencia, string valorFiltro)
        {
            DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro();
            filter.CampoFisico = campoFisico;
            filter.OperadorFiltro = operadorFiltro;
            filter.OperadorSentencia = operadorSentencia;
            filter.ValorFiltro = valorFiltro;

            return filter;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            ReportParameterList tipoRompimientoCosto = (ReportParameterList)this.RPControls["1"];
            ReportParameterList tipoRompimiento = (ReportParameterList)this.RPControls["2"];
            uc_MasterFind masterBodega = (uc_MasterFind)this.RPControls["4"];
            uc_MasterFind masterTipoBode = (uc_MasterFind)this.RPControls["5"];

            uc_MasterFind masterReferencia = (uc_MasterFind)this.RPControls["7"];
            uc_MasterFind masterGrupo = (uc_MasterFind)this.RPControls["8"];
            uc_MasterFind masterClase = (uc_MasterFind)this.RPControls["9"];
            uc_MasterFind masterTipo = (uc_MasterFind)this.RPControls["10"];
            uc_MasterFind masterSerie = (uc_MasterFind)this.RPControls["11"];
            uc_MasterFind masterMaterial = (uc_MasterFind)this.RPControls["12"];

            #region Opcion 1 Tipo de Rompimiento

            //Carga el valor de combo del rompimiento para el filtro por Bodega o Referencia
            if (option.Equals("1"))
                this._rompimientoSerial = tipoRompimientoCosto.SelectedListItem.Key;

            #endregion

            #region Opcion 2 Parametro

            if (option.Equals("2"))
            {
                this._rompimiento = tipoRompimiento.SelectedListItem.Key;

                masterBodega.Value = string.Empty;
                masterTipoBode.Value = string.Empty;
                masterReferencia.Value = string.Empty;
                masterGrupo.Value = string.Empty;
                masterClase.Value = string.Empty;
                masterTipo.Value = string.Empty;
                masterSerie.Value = string.Empty;
                masterMaterial.Value = string.Empty;
            }

            #endregion

            #region Opcion 3 Verifica si el Usuario quiere Filtro por Bodega

            if (option.Equals("3"))
            {
                switch (reportParameters["3"][0])
                {
                    case "True":
                        {
                            //Muestra Los master para filtrar
                            masterBodega.Visible = true;
                            masterTipoBode.Visible = true;
                        }
                        break;
                    case "False":
                        {
                            //Oculta los master y sin valor
                            masterTipoBode.Visible = false;
                            masterBodega.Visible = false;
                            masterTipoBode.Value = "";
                            masterBodega.Value = "";
                        }
                        break;
                }
            }

            #endregion

            #region Opcion 4 Filtro Bodega
            if (option.Equals("4"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._bodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                this._bodega = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

            #region Opcion 5 Filtro Tipo Bodega

            if (option.Equals("5"))
            {

                uc_MasterFind master = (uc_MasterFind)sender;
                //this._tipoBodega = (DTO_inBodegaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, master.Value, true);
                this._tipoBodega = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Opcion 6 Verifica so el Usuario quiere Filtro por Referencia
            if (option.Equals("6"))
            {
                switch (reportParameters["6"][0])
                {
                    case "True":
                        {
                            //Hace visible los master para el filtro por referencia
                            masterGrupo.Visible = true;
                            masterClase.Visible = true;
                            masterTipo.Visible = true;
                            masterSerie.Visible = true;
                            masterMaterial.Visible = true;
                            masterReferencia.Visible = true;
                        }
                        break;
                    case "False":
                        {
                            //Oculta los master y sin valor
                            masterGrupo.Visible = false;
                            masterClase.Visible = false;
                            masterTipo.Visible = false;
                            masterSerie.Visible = false;
                            masterMaterial.Visible = false;
                            masterReferencia.Visible = false;

                            masterGrupo.Value = "";
                            masterClase.Value = "";
                            masterTipo.Value = "";
                            masterSerie.Value = "";
                            masterMaterial.Value = "";
                            masterReferencia.Value = "";
                        }
                        break;
                }
            }
            #endregion

            #region Opcion 7 referencia

            if (option.Equals("7"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);
                this._referencia = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

            #region Opcion 8 Grupo

            if (option.Equals("8"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._grupo = (DTO_inRefGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefGrupo, false, master.Value, true);
                this._grupo = master.ValidID ? master.Value : string.Empty;
            }
            #endregion

            #region Opcion 9 Clase

            if (option.Equals("9"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._clase = (DTO_inRefClase)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefClase, false, master.Value, true);
                this._clase = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Opcion 10 Tipo

            if (option.Equals("10"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._tipo = (DTO_inRefTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, master.Value, true);
                this._tipo = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Opcion 11 Serie

            if (option.Equals("11"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._serie = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inSerie, false, master.Value, true);
                this._serie = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

            #region Opcion 12 Material

            if (option.Equals("12"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                //this._material = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMaterial, false, master.Value, true);
                this._material = master.ValidID ? master.Value : string.Empty;
            }

            #endregion

        }
        #endregion
    }
}
