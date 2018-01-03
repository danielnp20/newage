using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteBalComparativo : ReportParametersForm
    {

        #region Hilos

        protected override void LoadReportMethod_PDF()
        {
            //Variable para reporte
            string reportName;
            string fileURl;

            try
            {
                if (_reporte == "Saldos")
                {
                    switch (_moneda)
                    {
                        case "Local":
                            reportName = this._bc.AdministrationModel.ReportesContabilidad_BalanceComparativosSaldosML(this._libro, this._tipoMoneda, this._fechaIni.Year, this._fechaFin, this._fechaIni, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                            break;

                        case "Foreign":
                            reportName = this._bc.AdministrationModel.ReportesContabilidad_BalanceComparativosSaldosME(this._libro, this._tipoMoneda, this._fechaIni.Year, this._fechaFin, this._fechaIni, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                            break;

                        case "Both":
                            reportName = this._bc.AdministrationModel.ReportesContabilidad_BalanceComparativosSaldosAM(this._libro, this._tipoMoneda, this._fechaIni.Year, this._fechaFin, this._fechaIni, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                            break;
                    }
                }
                else
                {
                    switch (_moneda)
                    {
                        case "Local":
                            reportName = this._bc.AdministrationModel.ReporteContabilidad_BalanceComparativo(this._libro, this._tipoMoneda, this._fechaIni.Year, this._fechaFin, this._fechaIni, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                            break;

                        case "Foreign":
                            reportName = this._bc.AdministrationModel.ReporteContabilidad_BalanceComparativoME(this._libro, this._tipoMoneda, this._fechaIni.Year, this._fechaFin, this._fechaIni, this._formatType);
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                            Process.Start(fileURl);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ParametersReporteBalComparativo.cs-LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variable

        //Varibable para Iniciar el reporte
        DateTime _fechaIni;
        DateTime _fechaFin;
        string _reporte = "Saldos";
        string _moneda = "Local";
        string _libro = "";
        string _tipoMoneda;

        //Variables internas
        private long count = 0;
        IEnumerable<DTO_MasterBasic> librosAll = null;
        IEnumerable<DTO_MasterBasic> libros = null;
        List<ReportParameterListItem> librosComparados = new List<ReportParameterListItem>();

        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceComparativos).ToString());

            #region Configurarcion de Opciones

            //Carga la moneda por Defecto
            this._tipoMoneda = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);

            //Carga el Combo del tipo de reporte
            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>(){
                new ReportParameterListItem () {Key = "Saldos", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Saldo) },
                new ReportParameterListItem () {Key = "Movimientos", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Movimientos) } };

            //Carga la Lista Monedas
            List<ReportParameterListItem> moneda = new List<ReportParameterListItem>(){
                new ReportParameterListItem () {Key = "Local", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                new ReportParameterListItem () {Key = "Foreign", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) },
                new ReportParameterListItem () {Key = "Both", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) }};

            this.count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coBalanceTipo, null, null, true);
            this.librosAll = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coBalanceTipo, count, 1, null, null, true);
            //this.libros = librosAll.Where(x => x.ID.Value != this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional));
            //this._libro = this.libros.ElementAt(0).ID.Value;
            foreach (var libro in this.librosAll)
                librosComparados.Add(new ReportParameterListItem() { Key = libro.ID.ToString(), Desc = libro.ID.ToString() + " " + "-" + " " + libro.Descriptivo.ToString() });

            AddList("0", (AppForms.ReportForm).ToString() + "_Balance", tipoReporte, true, "1");
            AddList("1", (AppForms.ReportForm).ToString() + "_Moneda", moneda, true, "1");
            AddList("2", (AppForms.ReportForm).ToString() + "_LibrosComparar", librosComparados, true, _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceCorporativo));

            #endregion

            #region Configuracion filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.Name = "monthCB_13";
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB1.Name = "monthCB_13";
            this.periodoFilter1.monthCB1.SelectedIndex = 0;
            #endregion
        }

        /// <summary>
        /// Form Constructer (for Balance de Prueba Report)
        /// </summary>
        public ParametersReporteBalComparativo()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coBalanceComparativos;
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
            string fechaFinString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[1];
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
            ReportParameterList tipoReporte = (ReportParameterList)this.RPControls["0"];
            ReportParameterList listMoneda = (ReportParameterList)this.RPControls["1"];
            ReportParameterList listLibro = (ReportParameterList)this.RPControls["2"];

            //Cambia las monedas de acuerdo al tipo de reporte a imprimir
            if (option.Equals("0"))
            {
                //Carga el tipo de reporte a imprimir
                this._reporte = tipoReporte.SelectedListItem.Key;

                switch (tipoReporte.SelectedListItem.Key)
                {
                    case "Movimientos":
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());
                        break;
                    case "Saldos":
                        listMoneda.DefaultKey = listMoneda.SelectedListItem.Key;
                        listMoneda.RefreshList();
                        break;
                }
            }

            //Cambia else tipo de moneda
            if (option.Equals("1"))
            {
                this._moneda = listMoneda.SelectedListItem.Key;
                //Carga El tipo de moneda
                if (this._moneda == "Local")
                    this._tipoMoneda = listMoneda.SelectedListItem.Desc;
                else if (this._moneda == "Foreign")
                    this._tipoMoneda = listMoneda.SelectedListItem.Desc;
                else
                    this._tipoMoneda = listMoneda.SelectedListItem.Desc;
            }

            //Libro 
            if (option.Equals("2"))
            {
                this._libro = listLibro.SelectedListItem.Key;
            }
        }
        #endregion
    }
}
