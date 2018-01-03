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
    public class Reports_Aportes_Cartera : ReportParametersForm, IFiltrable
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
                if (this._report == 1 || this._report == 0)
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_Aportes_a_Clientes(this.Año, this.Mes, this._tercero, this._formatType); 
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    Process.Start(fileURl);
                }
                if (this._report == 2)
                {
                    reportName = this._bc.AdministrationModel.Report_Cc_EstadoDeCuenta(this._fechaIni, this._fechaFin, this._filtro,this._tercero, this._formatType);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Aportes_Cartera.cs-LoadReportMethod"));
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
        private int Año = 0;
        private int Mes = 0;
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        private string _tercero = string.Empty;
        //Filtro
        private string _filtro;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Aportes_Cartera() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.ccAportesCartera).ToString());
            #region Configurar Opciones
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> noFondoCajaCompensacionReportType = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_AportesCliente) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_EstadoCuenta) }, 
            };
            //Determina el nombre del combo y el item donde debe quedar
            this.AddList("1", (AppForms.ReportForm).ToString() + "_Tipo", noFondoCajaCompensacionReportType, true, "1");

            //Filtro por Cliente
            this.AddMaster("Cliente", AppMasters.ccCliente, true, null, true);

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
            //this.periodoFilter1.txtYear1.Visible = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            if (this._report == 2)
            {
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaFin = Convert.ToDateTime(fechaFinString);
            }

            this.periodoFilter1.Year[0].ToString();
            Año = Convert.ToInt32(this.periodoFilter1.Year[0]);
            Mes = Convert.ToInt32(this.periodoFilter1.Months[0]);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];

                if (listReportType.SelectedListItem.Key == 1.ToString())
                {
                    this._report = 1;
                    #region Configurar Filtros
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;

                    this.btnFilter.Enabled = true;
                    this.btnResetFilter.Enabled = true;
                    this.btnFilter.Visible = true;
                    this.btnResetFilter.Visible = true;
                    List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
                    fieldsNRT.Add(new ConsultasFields("PagaduriaID", "Pagaduria", typeof(string)));
                    mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
                    mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));

                    #endregion
                }

                if (listReportType.SelectedListItem.Key == 2.ToString())
                {
                    this._report = 2;
                    #region Configurar Filtros
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(1);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;
                    this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB1.SelectedIndex = 0;
                    
                    List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
                    fieldsNRT.Add(new ConsultasFields("EmpleadoID", "Cedula", typeof(string)));
                    mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
                    mq.SetFK("EmpleadoID", AppMasters.noEmpleado, _bc.CreateFKConfig(AppMasters.noEmpleado));
                    #endregion
                }
            }
            if (option.Equals("Cliente"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._tercero = masterCliente.ValidID ? masterCliente.Value : string.Empty; //// Asiganar valor a la variable de tercero y poder filtrar                    
            }
        }
        #endregion

        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            foreach (var fil in consulta.Filtros)
                this._filtro = fil.ValorFiltro;
        }
    }
}
