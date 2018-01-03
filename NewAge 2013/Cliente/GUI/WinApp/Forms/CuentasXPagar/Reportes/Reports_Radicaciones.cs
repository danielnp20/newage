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
    public class Reports_Radicaciones : ReportParametersForm, IFiltrable
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
                reportName = this._bc.AdministrationModel.Reporte_Cp_Radicaciones(this._yearIni, this._yearFin,this._fechaIni, this._fechaFin,this._tercero, this._tipoReporte, this._Orden,this._formatType);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reports_Radicaciones.cs-LoadReportMethod"));
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
        //Filtro
        private string _tipoReporte = "PorCausar";
        private string _Orden = "Consecutivo";
        private string _tercero;
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        private int _yearIni;
        private int _yearFin;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Radicaciones() 
        {
            this.Module = ModulesPrefix.cp;
            this.ReportForm = AppReportParametersForm.cpRadicaciones;
        }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cp;
            this.documentReportID = AppReports.cpRadicaciones;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.cpRadicaciones).ToString());

            #region Configuracion opciones

            //Lista que carga el combo del tipo de reporte
            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "Todos", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Todos)},
                new ReportParameterListItem { Key = "Causados", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Causadas)},
                new ReportParameterListItem { Key = "PorCausar", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_PorCausar)},
                new ReportParameterListItem { Key = "Devuelto", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Devueltos)}
            };

            this.AddList("Reporte", (AppForms.ReportForm).ToString() + "_Filtro", tipoReporte, true, "PorCausar");
            List<ReportParameterListItem> orden = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem { Key = "Consecutivo", Desc= this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Consecutivo)},
                new ReportParameterListItem { Key = "Nombre", Desc = this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Name)}
            };
            this.AddList("Orden", (AppForms.ReportForm).ToString() + "_Orden", orden, true, "Consecutivo");
            this.AddMaster("1", AppMasters.coTercero, true, null); 

            #endregion

            #region Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
            this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString(); 
            this.periodoFilter1.txtYear1.Text = DateTime.Now.Year.ToString();
            this.periodoFilter1.monthCB.Items.Add(1);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(DateTime.Now.Month);
            this.periodoFilter1.monthCB1.SelectedIndex = 0;

            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.btnFilter.Visible = true;
            this.btnResetFilter.Visible = true;
            base.btnExportToXLS.Visible = true;
            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                this.btnExportToPDF.Enabled = false;

                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = true;
                this.periodoFilter1.Year[0].ToString();
                this._yearIni = this.periodoFilter1.Year[0];
                this._yearFin = this.periodoFilter1.Year[1];
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception)
            {               
                ;
            }
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                base.btnExportToXLS.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = true;
                this.periodoFilter1.Year[0].ToString();
                this._yearIni = this.periodoFilter1.Year[0];
                this._yearFin = this.periodoFilter1.Year[1];
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                this._fechaFin = Convert.ToDateTime(fechaFinString);

                byte tipoRep = this._tipoReporte.Equals("Todos") ? (byte)0 :
                                 this._tipoReporte.Equals("Causados") ? (byte)1 :
                                    this._tipoReporte.Equals("PorCausar") ? (byte)2 : (byte)3;

                byte orden = this._Orden.Equals("Consecutivo") ? (byte)1 :(byte)2;

                this._Query = this._bc.AdministrationModel.Reportes_Cp_CxPToExcel(this.documentReportID, tipoRep, this._fechaIni, this._fechaFin, this._tercero,string.Empty, string.Empty, string.Empty,string.Empty,null, null, orden);

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
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            ReportParameterList Item = (ReportParameterList)this.RPControls["Reporte"];
            ReportParameterList ItemOrd = (ReportParameterList)this.RPControls["Orden"];
            if(option.Equals("1"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._tercero = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
            if (option.Equals("Reporte"))
            {
                this._tipoReporte = Item.SelectedListItem.Key;
                
            }
            if (option.Equals("Orden"))
            {
                this._Orden = ItemOrd.SelectedListItem.Key;

            }
        }
        #endregion
    }
}
