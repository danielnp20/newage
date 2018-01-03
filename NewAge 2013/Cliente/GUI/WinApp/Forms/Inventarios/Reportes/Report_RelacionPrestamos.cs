﻿using System;
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
    class Report_RelacionPrestamos : ReportParametersForm
    {
        #region Variables

        //Variables del Reporte
        private string _proyecto = string.Empty;     
        private int? _docNro;
        private byte _tipoReporte = 1;
        private string _inreferencia = string.Empty;

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
        public Report_RelacionPrestamos()
        {
            this.Module = ModulesPrefix.@in;
            this.documentReportID = AppReports.inRelacionPrestamos;
        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.@in;
            this.btnExportToXLS.Visible = false;
            this.btnExportToPDF.Visible = true;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.pyPresupuesto).ToString());

            #region Configurar Opciones

            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>()
            {
                 new ReportParameterListItem() { Key = "1", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)},
                new ReportParameterListItem() { Key = "2", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido)},
                new ReportParameterListItem() { Key = "3", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consolidated)},
            };
            //this.AddList("TipoReporte", "TipoReporte", tipoReporte, true, "1");
            //Crea y carga los controles respectivamente
            this.AddMaster("ProyectoID", AppMasters.coProyecto, true, null);

            #endregion
            #region Configurar Filtros del periodo

            //this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            //this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.None;

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

                //this.periodoFilter1.Year[0].ToString();
                //fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                //this._fechaIni = Convert.ToDateTime(fechaIniString);

                //this._Query = this._bc.AdministrationModel.ReportesProyectos_Presupuesto(DateTime.Now, this._proyecto);

                //if (this._Query.Rows.Count != 0)
                //{
                //    ReportExcelBase frm = new ReportExcelBase(this._Query);
                //    frm.Show();
                //}
                //else
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Report_RelacionPrestamos.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                Dictionary<string, string[]> reportParameters = this.GetValues();
                //ReportParameterList listReportType = (ReportParameterList)this.RPControls["TipoReporte"];
                //this._tipoReporte = Convert.ToByte(listReportType.SelectedListItem.Key);
                //ReportParameterTextBox docNro = (ReportParameterTextBox)this.RPControls["DocNro"];
                //this._docNro = !string.IsNullOrEmpty(docNro.txtFrom.Text) ? Convert.ToInt32((docNro.txtFrom.Text)) : this._docNro;

                uc_MasterFind masterProy = (uc_MasterFind)this.RPControls["ProyectoID"];
                if (masterProy != null && !string.IsNullOrEmpty(masterProy.Value) && !masterProy.ValidID)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Código del Proyecto no válido, verifique"));
                    return;
                }

                string reportName = this._bc.AdministrationModel.Report_RelacionPrestamos(this._proyecto, this._inreferencia);
                this.btnExportToPDF.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Report_SolicitudTrabajo.cs", "InitReport: " + ex.Message));
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
            uc_MasterFind master;
            if (option.Equals("ProyectoID"))
            {
                master = (uc_MasterFind)sender;
                this._proyecto = master.ValidID ? master.Value : string.Empty;
            }           

            #endregion
        }

        #endregion   
    }
}