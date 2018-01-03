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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_Anticipos : ReportParametersForm, IFiltrable
    {
        #region Hilos
        /// <summary>
        /// Hilo para generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName;
                string fileURl;
                bool tipoReporte = true;
                if (this._Reporte.Equals("Resumido"))
                    tipoReporte = false;
                reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_Anticipos(this._fechaIni, this._Mda, this._coTercero.ID.Value, tipoReporte, this._formatType);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Anticipos.cs-LoadReportMethod"));
                throw;
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
        private DTO_coTercero _coTercero = new DTO_coTercero();
        private string _OrgMoneda = "Local";
        private string _Reporte = "Resumido";
        private int _Mda = 1;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Anticipos() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.cpAnticipos;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.cpAnticipos).ToString());

            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> origenMda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "Local", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Local)},
                new ReportParameterListItem { Key = "Extranjera", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Extranjera)},
                new ReportParameterListItem { Key = "Ambas", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Ambas)}
            };
            List<ReportParameterListItem> reporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "Resumido", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Resumido)},
                new ReportParameterListItem { Key = "Detallado", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Detallado)}
            };

            this.AddMaster("Tercero", AppMasters.coTercero, true, null); 
            this.AddList("Reporte", (AppForms.ReportForm).ToString() + "_Reporte", reporte, true, "Resumido");
            this.AddList("OrigenMda", (AppForms.ReportForm).ToString() + "_MonedaOrigen", origenMda, true, "Local");

            #endregion

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            this.btnExportToXLS.Visible = true;
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

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                int lastDay = DateTime.DaysInMonth( this.periodoFilter1.Year[0], this.periodoFilter1.Months[0]);
                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + lastDay;
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                byte agrupReport = 0;
                if (this._Reporte.Equals("Resumido"))
                    agrupReport = 1;

                this._Query = this._bc.AdministrationModel.Reportes_Cp_CxPToExcel(this.documentReportID, null, this._fechaIni, null, this._coTercero.ID.Value, string.Empty, 
                                                                                    string.Empty, string.Empty, this._Mda.ToString(), null, agrupReport, null);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query, this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_CxPPorEdadesDetallado.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Eventos
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList OrigenMda = (ReportParameterList)this.RPControls["OrigenMda"];
            ReportParameterList Reporte = (ReportParameterList)this.RPControls["Reporte"];

           if (option.Equals("Tercero"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (!string.IsNullOrWhiteSpace(master.Value))
                    this._coTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);
                else
                {
                    this._coTercero = new DTO_coTercero();
                    this._coTercero.ID.Value = "";
                }
            }
            if(option.Equals("OrigenMda"))
            {
                this._OrgMoneda = OrigenMda.SelectedListItem.Key;
                if (_OrgMoneda.Equals("Local"))
                    this._Mda = 1;
                if (_OrgMoneda.Equals("Extranjera"))
                    this._Mda = 2;
                if (_OrgMoneda.Equals("Ambas"))
                    this._Mda = 3;
            }
            if (option.Equals("Reporte"))
            {
                this._Reporte = Reporte.SelectedListItem.Key;
            }
        }

        #endregion

    }
}
