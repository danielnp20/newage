using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;
using System.Threading;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Report_Documentos : ReportParametersForm
    {
        #region Variables
        //Variables para Reporte
        public string _clienteID = string.Empty;
        public DateTime _fechaIni = DateTime.Now;
        public string _bodega = string.Empty;
        public string _proyecto = string.Empty;
        public string _movimiento = string.Empty;

        public DateTime _fechaFin = DateTime.Now;
        DateTime _fechaUltimoCierre;
        public string _report = " ";
        public byte _resumido = 1;
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
        public Report_Documentos()
        {
            this.Module = ModulesPrefix.@in;

        }

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.@in;
            this.documentReportID = AppReports.inRepDocumentos;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = true;
            this.btnExportToPDF.Enabled = true;
            this.btnExportToXLS.Enabled = true;

            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.inRepDocumentos).ToString());
            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos

            //Items para el Combo del Formatos
            List<ReportParameterListItem> ListReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "0", Desc = "Todos" },
                new ReportParameterListItem() { Key = "1", Desc = "Entrada" },
                new ReportParameterListItem() { Key = "2", Desc = "Salida" },
                new ReportParameterListItem() { Key = "3", Desc = "Traslado" },
            };
            this.AddMaster("masterTipo", AppMasters.inMovimientoTipo, true, null);

            this.AddList("tipoReporte", "Tipo Reporte", ListReport, true, "0");
            List<ReportParameterListItem> ListDetalle = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = "Resumido" },
                new ReportParameterListItem() { Key = "2", Desc = "Detallado" },
                new ReportParameterListItem() { Key = "3", Desc = "Consolidado" },
            };
            this.AddList("tipoDetalle", "Detalle", ListDetalle, true, "1");

            this.AddMaster("masterBodega", AppMasters.inBodega, true, null);
            this.AddMaster("masterProyecto", AppMasters.coProyecto, true, null);

            #endregion

            #region Obtiene ultima fecha de corte

            var ultimoCierre = this._bc.AdministrationModel.ccCierreDiaCartera_GetUltimoDiaCierre(string.Empty, null);
            if (ultimoCierre != null)
                this._fechaUltimoCierre = ultimoCierre.Fecha.Value.Value;
            else
                this._fechaUltimoCierre = this.periodo;

            #endregion

            #endregion

            #region Configurar Filtros del periodo
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this._fechaUltimoCierre.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Clear();
            this.periodoFilter1.monthCB.Items.Add(this._fechaUltimoCierre.Date.Month);
            this.periodoFilter1.monthCB.SelectedItem = this._fechaUltimoCierre.Date.Month;
            this.EnableControls();
            #endregion


            #region Asigna Fecha de Ultimo Cierre
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;

            #endregion
        }
        #endregion

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
            DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
            this._fechaIni = fechaIniString;
            this._fechaFin = fechaIniString;

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

                #region Carga filtros
                uc_MasterFind txttipo = (uc_MasterFind)this.RPControls["masterTipo"];
                this._movimiento = txttipo.Value;

                uc_MasterFind txtbodega = (uc_MasterFind)this.RPControls["masterBodega"];
                this._bodega = txtbodega.Value;
                uc_MasterFind txtproyecto = (uc_MasterFind)this.RPControls["masterProyecto"];
                this._proyecto = txtproyecto.Value;

                #endregion

                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnExportToXLS.Enabled = false;
                this.periodoFilter1.Year[0].ToString();
                DateTime fechaIniString = Convert.ToDateTime(this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0]);
                this._fechaIni = fechaIniString;
                this._fechaFin = fechaIniString;
                ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                this._report = txtTipo.SelectedListItem.Key.ToString();

                ReportParameterList txtDetalle = (ReportParameterList)this.RPControls["tipoDetalle"];
                this._resumido = Convert.ToByte(txtDetalle.SelectedListItem.Key);

                if (this._resumido != 3)
                    this._Query = this._bc.AdministrationModel.Reportes_In_InventarioToExcel(this.documentReportID, this._fechaIni, new DateTime(this._fechaIni.Year, this._fechaIni.Month, DateTime.DaysInMonth(this._fechaIni.Year, this._fechaIni.Month)), this._bodega,
                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, this._proyecto, this._movimiento, null, this._resumido, null);
                else
                    this._Query = this._bc.AdministrationModel.Reportes_In_DocumentoToExcel(this._movimiento, this._bodega, this._proyecto, this._report, this._fechaIni, this._resumido);

                //Exporta Reporte a Excel.
                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query, this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));

                this.btnExportToPDF.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Report_Documentos", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            //ReportParameterList txtDetalle = (ReportParameterList)this.RPControls["tipoDetalle"];
            //  this._detalle = Convert.ToByte(txtDetalle.SelectedListItem.Key);
            //  if (this._detalle == 1 )
            //  {
            //      this.btnExportToPDF.Enabled = true;
            //      this.btnExportToXLS.Enabled = false;
            //  }
            //  else
            //  {
            //      this.btnExportToPDF.Enabled = false;
            //      this.btnExportToXLS.Enabled = true;
            //  }

        }

        #endregion

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                #region Carga filtros
                uc_MasterFind txttipo = (uc_MasterFind)this.RPControls["masterTipo"];
                this._movimiento = txttipo.Value;

                uc_MasterFind txtbodega = (uc_MasterFind)this.RPControls["masterBodega"];
                this._bodega = txtbodega.Value;
                uc_MasterFind txtproyecto = (uc_MasterFind)this.RPControls["masterProyecto"];
                this._proyecto = txtproyecto.Value;

                #endregion

                ReportParameterList txtTipo = (ReportParameterList)this.RPControls["tipoReporte"];
                this._report = txtTipo.SelectedListItem.Key.ToString();

                ReportParameterList txtDetalle = (ReportParameterList)this.RPControls["tipoDetalle"];
                this._resumido = Convert.ToByte(txtDetalle.SelectedListItem.Key);

                string reportName = this._bc.AdministrationModel.Report_Movimientos(this._movimiento, this._bodega, this._proyecto, this._report, this._fechaIni, this._resumido);

                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Report_Documentos.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

    }
}
