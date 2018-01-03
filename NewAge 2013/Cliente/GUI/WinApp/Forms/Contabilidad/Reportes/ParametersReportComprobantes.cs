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
using NewAge.DTO.Reportes;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersReportComprobantes : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                ReportParameterTextBox numeroComprobante = (ReportParameterTextBox)this.RPControls["5"];

                switch (this._report)
                {
                    case ("Detalle"):

                        #region Genera la consulta los reportes de comprobante detallados Preliminar o,con Moneda Local o Extrangera

                        if (_Preliminar == true)
                        {
                            #region Genera la consulta para el reporte Preliminar con Moneda Extranjera

                            if (_TipoMoneda == "2")
                            {
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobantePreliminarME(this._fechaIni.Year,
                                    this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._formatType);
                            }

                            #endregion

                            #region Genera la consulta para con los dos tipo de monedas (Extranjera y Local)

                            else if (_TipoMoneda == "3")
                            {
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobantePreliminarMLyME(this._fechaIni.Year,
                                   this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._formatType);
                            }

                            #endregion

                            #region Genera la consulta para el preliminar con moneda local

                            else
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobantePreliminar(this.documentReportID,this._fechaIni.Year,
                                    this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._formatType);

                            #endregion
                        }
                        else
                        {
                            #region Genera la consulta para el Reporte Con Moneda extrajera

                            if (_TipoMoneda == "2")
                            {
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobanteME(this._fechaIni.Year,
                                    this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._porHoja, this._formatType);
                            }

                            #endregion

                            #region Genera la consulta para El reporte con los Dos tipos de monedas (Extranjera y Local)

                            else if (_TipoMoneda == "3")
                            {
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobanteMLyME(this._fechaIni.Year,
                                    this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._porHoja, this._formatType);
                            }

                            #endregion

                            #region Genera la consulta para el reporte con Moneda Local

                            else
                            {
                                reportName = this._bc.AdministrationModel.ReportesContabilidad_Comprobante(this._fechaIni.Year,
                                    this._fechaIni.Month, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text, this._porHoja, this._formatType);
                            }

                            #endregion
                        }

                        #endregion

                        break;
                    case ("Control"):

                        #region Genera la consulta para el reporte de comprobate control

                        reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobanteControl(this._fechaIni.Year,
                                        this._fechaIni.Month, this._comprobante, this._formatType);

                        #endregion

                        break;
                }

                #region Genera el reporte

                //Genera el PDF
                if (this.reportName.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, this.reportName.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }

                //Verifica q se halla ejecutado con exito la generacion del PDF para mostrarlo al usuario
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReportComprobantes.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        private string _report = "Detalle";
        private string _TipoMoneda = "1";
        private string _tipoLibro;

        //Variables del hilo
        DateTime _fechaIni;
        DateTime _fechaFin;
        private bool _porHoja = false;
        public bool _Preliminar = false;
        private string _comprobante = "";
        private string _comprobanteIni = "";
        private string _comprobanteFinal = "";
        private DTO_TxResult reportName;
        private string fileURl;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer
        /// </summary>
        public ParametersReportComprobantes()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coComprobantes;
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Inicializa los controles para generar el reprote
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coReporteComprobantes;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coReporteComprobantes).ToString());

            #region Configurar Opciones

            this._tipoLibro = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

            #region Datos para cargar los combos

            //Items para el Combo de tipo de reporte
            List<ReportParameterListItem> tipoReporteComprobantes = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "Detalle", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed) },
                new ReportParameterListItem() { Key = "Control", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Control) },
                //new ReportParameterListItem() { Key = "plantilla", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Plantilla)}
            };

            //Itemns para el Combo tipo de Moneda
            List<ReportParameterListItem> tipoMoneda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = 1.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)},
                new ReportParameterListItem() { Key = 2.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)},
                new ReportParameterListItem() { Key = 3.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth)}
            };

            //Items para el combo de libros
            List<ReportParameterListItem> tipoLibros = new List<ReportParameterListItem>();
            long count = 0;
            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coBalanceTipo, null, null, true);
            IEnumerable<DTO_MasterBasic> TiposBalance = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coBalanceTipo, count, 1, null, null, true);
            foreach (var tipo in TiposBalance)
                tipoLibros.Add(new ReportParameterListItem() { Key = tipo.ID.ToString(), Desc = tipo.ID.ToString() + " - " + tipo.Descriptivo.ToString() });

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("1", (AppForms.ReportForm).ToString() + "_Reporte", tipoReporteComprobantes, true, "Detalle");
            this.AddList("2", (AppForms.ReportForm).ToString() + "_Moneda", tipoMoneda, true, 1.ToString());
            this.AddList("Libro", (AppForms.ReportForm).ToString() + "_TipoBalance", tipoLibros, true, this._tipoLibro);
            this.AddMaster("4", AppMasters.coComprobante, true, null);
            this.AddTextBox("5", true, (AppForms.ReportForm).ToString() + "_NoComprobante", false);
            this.AddCheck("preliminar", (AppForms.ReportForm).ToString() + "_ComprobantePre");
            this.AddCheck("ComprobantXHoja", (AppForms.ReportForm).ToString() + "_PorHoja", true);

            #endregion

            #region Configurar Filtros
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
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
            //string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            this._fechaIni = Convert.ToDateTime(fechaIniString);
            //this._fechaFin = Convert.ToDateTime(fechaFinString);

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte para plantlla de excel
        /// </summary>
        protected override void Report_XLS()
        {
            ReportParameterTextBox numeroComprobante = (ReportParameterTextBox)this.RPControls["5"];
            int year = Convert.ToInt16(this.periodoFilter1.txtYear.Text);
            int month = this.periodoFilter1.Months[0];

            this._fechaIni = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            this._Query = this._bc.AdministrationModel.ReportesContabilidad_ComprobanteXLS(this._fechaIni, this._comprobante, this._tipoLibro, numeroComprobante.txtFrom.Text, numeroComprobante.txtUntil.Text);

            if (this._Query.Rows.Count != 0)
            {
                ReportExcelBase frm = new ReportExcelBase(this._Query);
                frm.Show();
            }
            else
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
        }


        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            ReportParameterTextBox numeroComprobante = (ReportParameterTextBox)this.RPControls["5"];
            uc_MasterFind masterComprobante = (uc_MasterFind)this.RPControls["4"];
            CheckEdit porHojas = (CheckEdit)this.RPControls["ComprobantXHoja"];
            CheckEdit preliminar = (CheckEdit)this.RPControls["preliminar"];

            #region Opcion 1 Tipo de reporte
            if (option.Equals("1"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                this._report = listReportType.SelectedListItem.Key;

                //Verifica el tipo de reporte a imprimir, si es de control o plantilla oculta el check para que imprima comprobante por hoja
                switch (this._report)
                {
                    case ("Control"):
                        masterComprobante.Value = string.Empty;
                        masterComprobante.Visible = false;
                        numeroComprobante.Visible = false;
                        numeroComprobante.txtFrom.Text = string.Empty;
                        numeroComprobante.txtUntil.Text = string.Empty;
                        porHojas.Visible = false;
                        preliminar.Visible = false;
                        this.btnExportToPDF.Visible = true;
                        this.btnExportToXLS.Visible = false;
                        break;

                    case ("plantilla"):
                        porHojas.Visible = false;
                        masterComprobante.Visible = true;
                        preliminar.Visible = false;
                        this.btnExportToPDF.Visible = false;
                        this.btnExportToXLS.Visible = true;
                        break;

                    case ("Detalle"):
                        masterComprobante.Visible = true;
                        masterComprobante.Value = string.Empty;
                        porHojas.Visible = true;
                        preliminar.Visible = true;
                        this.btnExportToPDF.Visible = true;
                        this.btnExportToXLS.Visible = false;
                        break;
                    default:
                        break;
                }

            }
            #endregion

            #region Opcion 2 Tipo Moneda

            if (option.Equals("2"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];

                this._TipoMoneda = listReportType.SelectedListItem.Key.ToString();
            }

            #endregion

            #region Carga el libro con el que se desea trabajar
            if (option.Equals("Libro"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["Libro"];

                this._tipoLibro = listReportType.SelectedListItem.Key;
            }
            #endregion

            #region Opcion 4 Comprobante

            if (option.Equals("4"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._comprobante = master.ValidID ? master.Value : string.Empty;
                if (!string.IsNullOrEmpty(this._comprobante))
                    numeroComprobante.Visible = true;
                else
                {
                    numeroComprobante.Visible = false;
                    numeroComprobante.txtFrom.Text = string.Empty;
                    numeroComprobante.txtUntil.Text = string.Empty;
                }
            }
            #endregion

            #region Opcion Comprobante por hoja
            if (option.Equals("ComprobantXHoja"))
            {
                switch (reportParameters["ComprobantXHoja"][0])
                {
                    case "True":
                        this._porHoja = true;
                        break;
                    case "False":
                        this._porHoja = false;
                        break;
                }
            }
            #endregion

            #region Opcion de Comprobante Preliminar
            if (option.Equals("preliminar"))
            {
                switch (reportParameters["preliminar"][0])
                {
                    case "True":
                        this._Preliminar = true;
                        break;
                    case "False":
                        this._Preliminar = false;
                        break;
                }
            }

            #endregion
        }
        #endregion

    }
}
