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
    public class Reports_FacturasXPagar : ReportParametersForm, IFiltrable
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
                string multi = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
                bool isMultimoneda = false;
                if (multi.Equals("1"))
                    isMultimoneda = true;
                reportName = this._bc.AdministrationModel.Reporte_Cp_FacturasXPagar(this._fechaIni, this._coTercero.ID.Value, this._Mda, this._coPlanCuenta, isMultimoneda, this._formatType);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_FacturasXPagar.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        private int _report = 0;
        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        //Filtro
        private string _clienteFiltro;
        private string _libranzaFiltro;
        //Filtros Cheks
        private bool _carteraPropia = false;
        private bool _cedida = false;
        private ControlsUC.uc_MasterFind uc_MfPagaduria;
        private ControlsUC.uc_MasterFind uc_MfCompradorC;
        private bool _toda = false;
        private List<string> _tipo = new List<string>();
        //Variables Dto
        private DTO_ccPagaduria _dto_Pagaduria = null;
        private DTO_coTercero _coTercero = new DTO_coTercero();
        private string _OrgMoneda = "Local";
        private int _Mda = 1;
        private string _coPlanCuenta="";
        private DTO_ccCompradorCartera _dto_Comprador = null;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_FacturasXPagar() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cp;
            this.documentReportID = AppReports.cpFacturasXPagar;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.cpFacturasXPagar).ToString());

            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> origenMda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "Local", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Local)},
                new ReportParameterListItem { Key = "Extranjera", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Extranjera)},
                new ReportParameterListItem { Key = "Ambas", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Ambas)}
            };

            this.AddMaster("1", AppMasters.coTercero, true, null);
            this.AddList("OrigenMda", (AppForms.ReportForm).ToString() + "_MonedaOrigen", origenMda, true, "Local");
            this.AddMaster("Cuenta", AppMasters.coPlanCuenta, true, null);

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
                base.btnExportToPDF.Enabled = false;
                base.btnExportToXLS.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                string multi = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
                this._Query = this._bc.AdministrationModel.Reportes_Cp_CxPToExcel(this.documentReportID, null, this._fechaIni, null, this._coTercero.ID.Value,string.Empty,
                                                                                  this._coPlanCuenta, string.Empty, this._Mda.ToString(), multi, null, null);

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

           if (option.Equals("1"))
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
            if (option.Equals("Cuenta"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (!string.IsNullOrEmpty(master.Value))
                    this._coPlanCuenta = master.Value;
                else
                    this._coPlanCuenta = "";
            }
        }

        #endregion

        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            foreach (var fil in consulta.Filtros)
                this._clienteFiltro = fil.ValorFiltro;
        }
    }
}
