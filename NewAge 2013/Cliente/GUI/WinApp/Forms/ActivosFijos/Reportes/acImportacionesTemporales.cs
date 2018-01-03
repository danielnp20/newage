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
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class acImportacionesTemporales : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string fileURl;
                this.result = this._bc.AdministrationModel.ReportesActivos_ImportacionesTemporales(this._fechaIni, this._Plaqueta, this._Serial, this._TipoRef, this.Rompimiento, this._formatType);

                #region Generacion del reporte

                if (this.result.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, this.result.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReportSaldos.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        DateTime _fechaIni;
        DateTime _fechaFin;
        private string _Plaqueta = "";
        private string _Serial = "";
        private string _TipoRef = "";
        DTO_TxResult result = new DTO_TxResult();
        private string Rompimiento = "MesVencimiento-TipoRef";
        private bool _Excel = false;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public acImportacionesTemporales() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.ac;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.acEquiposArrendados).ToString());

            #region Configurar Opciones

            List<ReportParameterListItem> Rompimiento = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "MesVencimiento-TipoRef", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_FechaVencimiento)},
                new ReportParameterListItem { Key = "TipoRef-MesVencimiento", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_TipoReferencia)}
            };

            this.AddMaster("Plaqueta", AppMasters.ccAsesor, true, null, true);
            this.AddMaster("Serial", AppMasters.ccPagaduria, true, null, true);
            this.AddMaster("TipoRef", AppMasters.ccPagaduria, true, null, true);
            this.AddList("Rompimiento", (AppForms.ReportForm).ToString() + "_Rompimiento", Rompimiento, true, "Tercero-TipoRef");
            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
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
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            this._fechaIni = Convert.ToDateTime(fechaIniString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        #endregion

        #region Eventos
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList Romp = (ReportParameterList)this.RPControls["Rompimiento"];
            #region Carga el filtro del cliente
            Dictionary<string, string[]> reportParameters = this.GetValues();
            if (option.Equals("Plaqueta"))
            {
                uc_MasterFind Plaqueta = (uc_MasterFind)sender;
                this._Plaqueta = Plaqueta.ValidID ? Plaqueta.Value : string.Empty;
            }
            if (option.Equals("Serial"))
            {
                uc_MasterFind Serial = (uc_MasterFind)sender;
                this._Serial = Serial.ValidID ? Serial.Value : string.Empty;
            }
            if (option.Equals("TipoRef"))
            {
                uc_MasterFind TipoRef = (uc_MasterFind)sender;
                this._TipoRef = TipoRef.ValidID ? TipoRef.Value : string.Empty;
            }
            if (option.Equals("Rompimiento"))
            {
                this.Rompimiento = Romp.SelectedListItem.Key;
            }
            #endregion
        }

        #endregion

    }
}
