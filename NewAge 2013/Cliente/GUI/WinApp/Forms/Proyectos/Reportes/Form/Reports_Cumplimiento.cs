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
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_Cumplimiento : ReportParametersForm
    {
        #region Variables

        //Variables para Reporte
        DateTime _fechaIni;
        private string _proyecto = string.Empty;
        private string _estado = "*";
        private string _lineaFlujo = string.Empty;
        private string _etapa = string.Empty;
        
        #endregion

        #region Funciones Privadas

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
        public Reports_Cumplimiento()
        {
            this.Module = ModulesPrefix.py;
            this.ReportForm = AppReportParametersForm.pyCumplimiento;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.py;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.pyCumplimiento).ToString());

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Estado
            List<ReportParameterListItem> ListEstado = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "*", Desc = "*" },
                new ReportParameterListItem() { Key = "0", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_SinIniciar)},
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_EnDesarrollo)},
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCerrado)}
            };

            #endregion

            //Crea y carga los controles respectivamente
            this.AddMaster("proyecto", AppMasters.coProyecto, true, null);
            this.AddMaster("lineaFlujo", AppMasters.pyLineaFlujo, true, null);
            this.AddMaster("Etapa", AppMasters.pyEtapa, true, null);
            this.AddList("Estado", (AppForms.ReportForm).ToString() + "_Estado", ListEstado, true, "*");

            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.SelectedItem = DateTime.Now.Day;

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

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + "/" + this.periodoFilter1.Months[1];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                this._Query = this._bc.AdministrationModel.ReportesProyectos_Cumplimiento(this._fechaIni, this._proyecto, _estado, _lineaFlujo, _etapa);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Cumplimiento", "Report_XLS"));

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
            #region Carga el proyecto que se desea verificar

            if (option.Equals("proyecto"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)sender;
                this._proyecto = masterProyecto.ValidID ? masterProyecto.Value : string.Empty;
            }

            #endregion
            #region Carga la Linea de Flujo que desea verificar

            if (option.Equals("lineaFlujo"))
            {
                uc_MasterFind masterLineaFlujo = (uc_MasterFind)sender;
                this._lineaFlujo = masterLineaFlujo.ValidID ? masterLineaFlujo.Value : string.Empty;
            }

            #endregion
            #region Carga la Etapa que desea verificar

            if (option.Equals("Etapa"))
            {
                uc_MasterFind masterEtapa = (uc_MasterFind)sender;
                this._lineaFlujo = masterEtapa.ValidID ? masterEtapa.Value : string.Empty;
            }

            #endregion
            #region Carga el estado que desea Verificar

            if (option.Equals("Estado"))
            {
                ReportParameterList estado = (ReportParameterList)this.RPControls["tipoReporte"];
                this._estado = estado.SelectedListItem.Key;
            }
            
            #endregion
        }

        #endregion

    }
}
