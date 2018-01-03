using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_ReciboCaja : ReportParametersForm, IFiltrable
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

                reportName = this._bc.AdministrationModel.Reporte_Ts_RecibosDeCaja(this._fechaIni, this._fechaFin, this._coTercero.ID.Value ,this._tsCaja.ID.Value);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_AportesNomina.cs-", "LoadReportMethod"));
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
        private DTO_tsCaja _tsCaja = new DTO_tsCaja();
        private DTO_coTercero _coTercero = new DTO_coTercero();
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_ReciboCaja() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.tsReciboCaja;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.tsReciboCaja).ToString());
            #region Configurar Opciones
            this.AddMaster("1", AppMasters.tsCaja, true, null);
            this.AddMaster("2", AppMasters.coTercero, true, null);

            #endregion
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

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;

            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            this._fechaFin = Convert.ToDateTime(fechaFinString);

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
                uc_MasterFind master = (uc_MasterFind)sender;
                if (!string.IsNullOrWhiteSpace(master.Value))
                    this._tsCaja = (DTO_tsCaja)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsCaja, false, master.Value, true);
                else
                    this._tsCaja = new DTO_tsCaja();
            }
            if (option.Equals("2"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                if (!string.IsNullOrWhiteSpace(master.Value))
                    this._coTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);
                else
                    this._coTercero  = new DTO_coTercero();
            }
        }
        #endregion
    }
}
