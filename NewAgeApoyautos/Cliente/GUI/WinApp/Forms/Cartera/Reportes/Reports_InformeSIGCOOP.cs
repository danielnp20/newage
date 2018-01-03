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
    class Reports_InformeSIGCOOP : ReportParametersForm
    {
        #region Variables

        //Variables para Reporte
        DateTime _Perido;
        string _formato = "-";

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de Inhabilitar los controles que no son necesarios para este reporte
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
        public Reports_InformeSIGCOOP()
        {
            this.Module = ModulesPrefix.cc;
            this.ReportForm = AppReportParametersForm.ccInformesSIGCOOP;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ccInformesSIGCOOP).ToString());

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListFormatos = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "-", Desc = "-" },
                new ReportParameterListItem() { Key = "F19", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoF19)},
                new ReportParameterListItem() { Key = "F21", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoF21)},
                new ReportParameterListItem() { Key = "F25", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoF25)},
                new ReportParameterListItem() { Key = "F47", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoF47)},
                new ReportParameterListItem() { Key = "F49", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FormatoF49)},
                new ReportParameterListItem() { Key = "FormatoEspecial", Desc = _bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_FormatoEspecial)},
            };

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("Formatos", (AppForms.ReportForm).ToString() + "_tipoReport", ListFormatos, true, "-");


            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
            this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

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
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._Perido = Convert.ToDateTime(fechaIniString);

                if (this._formato != "-")
                {
                    this._Query = this._bc.AdministrationModel.ReportesCartera_Cc_InformeSIGCOOP(this._Perido, this._formato);

                    if (this._Query.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(this._Query);
                        frm.Show();
                    }
                    else 
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_NoSeGeneroReporte));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_InformeSIGCOOP", "Report_XLS"));
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
            #region Carga el Fomato que desea Verificar

            if (option.Equals("Formatos"))
            {
                ReportParameterList formato = (ReportParameterList)this.RPControls["Formatos"];
                this._formato = formato.SelectedListItem.Key;
            }

            #endregion
        }

        #endregion

    }
}
