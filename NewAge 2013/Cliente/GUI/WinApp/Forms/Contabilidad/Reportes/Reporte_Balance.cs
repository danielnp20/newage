using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.Drawing;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reporte_Balance : ReportParametersForm
    {
        #region Variable

        //Variables del hilo
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        public string tipoBalance;
        public string _tipoReport = "DePrueba";
        private DateTime Periodo;
        private int LongitudCuenta = 0;
        private int SaldoInicial = 0;
        private string CuentaInicial = string.Empty;
        private string CuentaFinal = string.Empty;
        private string libro;
        private string Moneda;
        private byte Combo1 = 0;
        private byte Combo2 = 0;
        private int mesIni = 0;
        private int mesFin = 0;
        private int año;
        private string proyecto;
        private string centroCto;
        //Varibale para mensaje
        private string _errorConsuta;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Libro Mayor Report)
        /// </summary>
        public Reporte_Balance()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coLibroMayor;
        }

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.documentReportID = AppReports.coBalanceDePrueba;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePrueba).ToString());
            this._errorConsuta = _bc.GetResourceError(DictionaryMessages.Rpt_gl_NoSeGeneranDatos);
            base.ReportForm = AppReportParametersForm.coBalancePrueba;
        }
    
        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();

            #region Carga variables de control

            
            #region Filtro Libro
            ReportParameterList typeBook = (ReportParameterList)this.RPControls["4"];
            this.libro = typeBook.SelectedListItem.Key;
            #endregion
            #region Filtro Moneda
            ReportParameterList typeMOneda = (ReportParameterList)this.RPControls["2"];
            this.Moneda = typeMOneda.SelectedListItem.Key;
            #endregion
            #region Filtro Longitud
            ReportParameterList typeLong = (ReportParameterList)this.RPControls["8"];
            if (typeLong.SelectedListItem.Key.ToString() == "Max")
            {
                string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
                Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
                DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                this.LongitudCuenta = table.CodeLength(table.LevelsUsed());
            }
            else
                this.LongitudCuenta = !string.IsNullOrEmpty(typeLong.SelectedListItem.Key.ToString()) ? Convert.ToInt32(typeLong.SelectedListItem.Key) : 0;
            #endregion
            #region Filtro SaldoInicial
            CheckEdit typeSaldoIni = (CheckEdit)this.RPControls["9"];
            this.SaldoInicial = typeSaldoIni.Checked ? 1 : 0;
            #endregion
            #region Filtro TipoReporte
            ReportParameterList typeRpt = (ReportParameterList)this.RPControls["3"];
            this._tipoReport = typeRpt.SelectedListItem.Key;

            #endregion
            #region Filtro Rango Cuentas
            CheckEdit typeRango = (CheckEdit)this.RPControls["filtroCuentas"];
            bool rangoInd = typeRango.Checked;
            if (rangoInd)
            {
                Dictionary<string, string[]> rangoCuentas = this.GetValues();
                uc_MasterFind masterCtaIni = (uc_MasterFind)this.RPControls["CuentaIncial"];
                uc_MasterFind masterCtaFin = (uc_MasterFind)this.RPControls["CuentaFinal"];
                this.CuentaInicial = masterCtaIni.Value;// masterCtaIni.ValidID ? masterCtaIni.Value : string.Empty;
                this.CuentaFinal = masterCtaFin.Value;// masterCtaFin.ValidID ? masterCtaFin.Value : string.Empty;
            }
            else
            {
                this.CuentaInicial = "0";
                this.CuentaFinal = "999999999999";

            }
            #endregion
            #region Agrupamiento
            ReportParameterList typoAgrup = (ReportParameterList)this.RPControls["1"];
            string agrup = typoAgrup.SelectedListItem.Key;
            if(agrup.Equals("Detailed"))
            {
                #region Combo 1
                ReportParameterList typeRomp1 = (ReportParameterList)this.RPControls["5"];
                if ((typeRomp1.SelectedListItem.Key.Equals("CentroCostoID")))
                    this.Combo1 = 1;
                if ((typeRomp1.SelectedListItem.Key.Equals("ProyectoID")))
                    this.Combo1 = 2;
                if ((typeRomp1.SelectedListItem.Key.Equals("LineaPresupuestoID")))
                    this.Combo1 = 3;
                if ((typeRomp1.SelectedListItem.Key.Equals("")))
                    this.Combo1 = 4;
                #endregion
                #region Combo 2
                ReportParameterList typeRomp2 = (ReportParameterList)this.RPControls["6"];
                if ((typeRomp2.SelectedListItem.Key.Equals("ProyectoID")))
                    this.Combo2 = 1;
                if ((typeRomp2.SelectedListItem.Key.Equals("CentroCostoID")))
                    this.Combo2 = 2;
                if ((typeRomp2.SelectedListItem.Key.Equals("LineaPresupuestoID")))
                    this.Combo2 = 3;
                if ((typeRomp2.SelectedListItem.Key.Equals("-")))
                    this.Combo2 = 4;
                if ((typeRomp1.SelectedListItem.Key.Equals("")))
                    this.Combo2 = 5;
                #endregion
            }
            else
            {
                this.Combo1 = 0;
                this.Combo2 = 0;
            }
            #endregion            
            #region Filtro Fecha
            this.periodoFilter1.Year[0].ToString();
            if (this._tipoReport.Equals("DePrueba") || this._tipoReport.Equals("EstResultados") || this._tipoReport.Equals("EstResultadosMes"))
            {
                this.mesIni = this.periodoFilter1.Months[0];
                this.mesFin = this.periodoFilter1.Months[1];
            }           
            this.año = Convert.ToInt32(periodoFilter1.txtYear.Text);
            #endregion
            #region Filtro Proyecto-CentroCto
            if (this._tipoReport.Equals("EstResultados") || this._tipoReport.Equals("EstResultadosMes"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)this.RPControls["ProyectoID"];
                uc_MasterFind masterCentroCto = (uc_MasterFind)this.RPControls["CentroCostoID"];
                this.proyecto = masterProyecto.Value;
                this.centroCto = masterCentroCto.Value;
            }
            else
            {
                this.proyecto = string.Empty;
                this.centroCto = string.Empty;
            }
            #endregion         

            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;
            this.periodoFilter1.txtYear1.Visible = false;

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();
            #endregion
        }

        /// <summary>
        /// Excel data for the report from the form
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
               
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                #region Validar Fechas
                // Validar Año
                año = Convert.ToInt32(this.periodoFilter1.Year[0]);
                if (this._tipoReport.Equals("DePrueba") || this._tipoReport.Equals("EstResultados") || this._tipoReport.Equals("EstResultadosMes"))
                {
                    this.mesIni =this.periodoFilter1.Months[0];
                    this.mesFin = this.periodoFilter1.Months[1]; 
                }   
              
                #endregion
                #region Validar Libro
                ReportParameterList typeBook = (ReportParameterList)this.RPControls["4"];
                this.libro = typeBook.SelectedListItem.Key;
                #endregion
                #region Validar Moneda
                //Verifica la moneda a imprimir
                Moneda = string.Empty;
                switch (reportParameters["2"][0])
                {
                    case "Local":
                        Moneda = "Local";
                        break;

                    case "Foreign":
                        Moneda = "Foreign";
                        break;

                    case "Both":
                        Moneda = "Both";
                        break;
                }; 
                #endregion
                #region Validar Cuenta
                // Validar Cuentas
                CheckEdit typeRango = (CheckEdit)this.RPControls["filtroCuentas"];
                bool rangoInd = typeRango.Checked;
                if (rangoInd)
                {
                    Dictionary<string, string[]> rangoCuentas = this.GetValues();
                    uc_MasterFind masterCtaIni = (uc_MasterFind)this.RPControls["CuentaIncial"];
                    uc_MasterFind masterCtaFin = (uc_MasterFind)this.RPControls["CuentaFinal"];
                    this.CuentaInicial = masterCtaIni.Value;// masterCtaIni.ValidID ? masterCtaIni.Value : string.Empty;
                    this.CuentaFinal = masterCtaFin.Value;//  masterCtaFin.ValidID ? masterCtaFin.Value : string.Empty;
                }
                else
                {
                    this.CuentaInicial = "0";
                    this.CuentaFinal = "999999999999";

                } 
                #endregion
                #region Filtro Longitud
                ReportParameterList typeLong = (ReportParameterList)this.RPControls["8"];
                if (typeLong.SelectedListItem.Key.ToString() == "Max")
                {
                    string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
                    Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
                    DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                    this.LongitudCuenta = table.CodeLength(table.LevelsUsed());
                }
                else
                    this.LongitudCuenta = !string.IsNullOrEmpty(typeLong.SelectedListItem.Key.ToString()) ? Convert.ToInt32(typeLong.SelectedListItem.Key) : 0;
                #endregion
                #region Filtro SaldoInicial
                CheckEdit typeSaldoIni = (CheckEdit)this.RPControls["9"];
                this.SaldoInicial = typeSaldoIni.Checked ? 1 : 0;
                #endregion
                #region Filtro Proyecto-CentroCto
                if (this._tipoReport.Equals("EstResultados") || this._tipoReport.Equals("EstResultadosMes"))
                {
                    uc_MasterFind masterProyecto = (uc_MasterFind)this.RPControls["ProyectoID"];
                    uc_MasterFind masterCentroCto = (uc_MasterFind)this.RPControls["CentroCostoID"];
                    this.proyecto = masterProyecto.Value;
                    this.centroCto = masterCentroCto.Value;
                }
                else
                {
                    this.proyecto = string.Empty;
                    this.centroCto = string.Empty;
                }
                #endregion

                this._Query = this._bc.AdministrationModel.ReportesContabilidad_ReporteBalancePruebasXLS(this.año, this.LongitudCuenta, this.SaldoInicial, this.CuentaInicial,
                                               this.CuentaFinal, this.libro, this._tipoReport, this.Moneda, this.mesIni, this.mesFin, this.Combo1, this.Combo2,this.proyecto,this.centroCto);
                           
                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query,this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteBalPrueba", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
       
        #endregion

        #region Funciones Privadas
        private DTO_glConsultaFiltro Filtro(string campoFisico, string operadorFiltro, string operadorSentencia, string valorFiltro)
        {
            DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro();
            filter.CampoFisico = campoFisico;
            filter.OperadorFiltro = operadorFiltro;
            filter.OperadorSentencia = operadorSentencia;
            filter.ValorFiltro = valorFiltro;

            return filter;
        }
        #endregion

        #region Eventos

        protected override void ListValueChanged(string option, object sender)
        {
            this.btnExportToXLS.Visible = true;
            #region Filtro Libro
            if (option.Equals("4"))
            {
                ReportParameterList typeBook = (ReportParameterList)this.RPControls["4"];
                this.libro = typeBook.SelectedListItem.Key;
            }

            #endregion
            #region Filtro 3
            if (option.Equals("3"))
            {
                ReportParameterList typeRpt = (ReportParameterList)this.RPControls["3"];
                Dictionary<string, string[]> reportParameters = this.GetValues();
                this._tipoReport = typeRpt.SelectedListItem.Key;
                uc_MasterFind masterProyecto = (uc_MasterFind)this.RPControls["ProyectoID"];
                uc_MasterFind masterCentroCto = (uc_MasterFind)this.RPControls["CentroCostoID"];
                masterProyecto.Visible = false;
                masterCentroCto.Visible = false;

                switch (reportParameters["3"][0])
                {
                    case "DePrueba":
                        #region De prueba
                        //if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonthSpan || !this.periodoFilter1.monthCB.Name.Contains("13"))
                        //{
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                            //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                            this.periodoFilter1.monthCB.Name = "monthCB_13";
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB.Enabled = true;
                            this.periodoFilter1.monthCB.AllowDrop = true;
                            this.periodoFilter1.monthCB1.Name = "monthCB_13";
                            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date);
                            this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        //}

                        ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                        if (listReportType.Visible != true)
                        {
                            listReportType.RefreshList();
                            listReportType.Enabled = true;
                            listReportType.Visible = true;
                        }

                        ReportParameterList listMoneda = (ReportParameterList)this.RPControls["2"];
                        listMoneda.DefaultKey = listMoneda.SelectedListItem.Key;
                        listMoneda.RefreshList();

                        ReportParameterList cuentaLength = (ReportParameterList)this.RPControls["8"];
                        cuentaLength.DefaultKey = "Max";
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        CheckEdit saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        } 
                        #endregion
                        break;
                    case "PorM":
                        #region Por M
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false; 
                        #endregion
                        break;
                    case "PorQ":
                        #region POr Q
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false; 
                        #endregion
                        break;
                    case "Comparativo":
                        #region Comparativo
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB.Enabled = false;
                        this.periodoFilter1.monthCB.AllowDrop = false;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        } 
                        #endregion
                        break;
                    case "General":
                        #region General
                        if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonth)
                        {
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                            //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB.Enabled = true;
                            this.periodoFilter1.monthCB.AllowDrop = true;
                        }

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        if (listMoneda.SelectedListItem.Key == TipoMoneda.Both.ToString())
                            listMoneda.RefreshList();
                        if (listMoneda.ContainsItem(TipoMoneda.Both.ToString()))
                            listMoneda.RemoveItem(TipoMoneda.Both.ToString());

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        listReportType.RefreshList();
                        listReportType.Enabled = false;
                        listReportType.Visible = false;

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        cuentaLength.RefreshList();
                        cuentaLength.Enabled = false;
                        cuentaLength.Visible = false;

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Enabled = false;
                        saldoInicialInd.Visible = false; 
                        #endregion
                        break;
                    case "EstResultados":
                        #region De resultados
                        //if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonthSpan || !this.periodoFilter1.monthCB.Name.Contains("13"))
                        //{
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                            //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                            this.periodoFilter1.monthCB.Name = "monthCB_13";
                            this.periodoFilter1.monthCB.Items.Clear();
                            this.periodoFilter1.monthCB.Items.Add(1);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB.Enabled = false;
                            this.periodoFilter1.monthCB.AllowDrop = true;
                            this.periodoFilter1.monthCB1.Name = "monthCB_13";
                            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date);
                            this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        //}

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        if (listReportType.Visible != true)
                        {
                            listReportType.RefreshList();
                            listReportType.Enabled = true;
                            listReportType.Visible = true;
                        }

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        listMoneda.DefaultKey = listMoneda.SelectedListItem.Key;
                        listMoneda.RefreshList();

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        cuentaLength.DefaultKey = "6";
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        }

                        masterProyecto.Visible = true;
                        masterCentroCto.Visible = true;
                        this.btnExportToXLS.Visible = false;
                        #endregion
                        break;
                    case "EstResultadosMes":
                        #region De resultados Mensualizado
                        //if (this.periodoFilter1.FilterOptions != PeriodFilterOptions.YearMonthSpan || !this.periodoFilter1.monthCB.Name.Contains("13"))
                        //{
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                        //this.periodoFilter1.txtYear.Text = (dt.Year).ToString();
                        this.periodoFilter1.monthCB.Name = "monthCB_13";
                        this.periodoFilter1.monthCB.Items.Clear();
                        this.periodoFilter1.monthCB.Items.Add(1);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB.Enabled = false;
                        this.periodoFilter1.monthCB.AllowDrop = true;
                        this.periodoFilter1.monthCB1.Name = "monthCB_13";
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        //}

                        listReportType = (ReportParameterList)this.RPControls["1"];
                        if (listReportType.Visible != true)
                        {
                            listReportType.RefreshList();
                            listReportType.Enabled = true;
                            listReportType.Visible = true;
                        }

                        listMoneda = (ReportParameterList)this.RPControls["2"];
                        listMoneda.DefaultKey = listMoneda.SelectedListItem.Key;
                        listMoneda.RefreshList();

                        cuentaLength = (ReportParameterList)this.RPControls["8"];
                        cuentaLength.DefaultKey = "6";
                        if (cuentaLength.Visible != true)
                        {
                            cuentaLength.RefreshList();
                            cuentaLength.Enabled = true;
                            cuentaLength.Visible = true;
                        }

                        saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        if (saldoInicialInd.Visible != true)
                        {
                            saldoInicialInd.Enabled = true;
                            saldoInicialInd.Visible = true;
                            saldoInicialInd.Checked = true;
                        }                        
                        masterProyecto.Visible = true;
                        masterCentroCto.Visible = true;
                        this.btnExportToXLS.Visible = true;
                        #endregion
                        break;

                };
            };
            #endregion
            #region Filtro 1
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                switch (reportParameters["1"][0])
                {
                    case "Detailed":
                        ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                        listRompimiento1.Enabled = true;
                        listRompimiento1.Visible = true;
                        listRompimiento1.RefreshList();
                        listRompimiento1.RemoveItem("-");
                        ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                        listRompimiento2.Enabled = true;
                        listRompimiento2.Visible = true;
                        listRompimiento2.RefreshList();
                        listRompimiento2.RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                        break;
                    case "Consolidated":
                        listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                        listRompimiento1.RefreshList();
                        listRompimiento1.Enabled = false;
                        listRompimiento1.Visible = false;
                        listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                        listRompimiento2.RefreshList();
                        listRompimiento2.Enabled = false;
                        listRompimiento2.Visible = false;
                        break;
                };
            };
            #endregion
            #region Filtro 5
            if (option.Equals("5"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listRompimiento2 = (ReportParameterList)this.RPControls["6"];
                listRompimiento2.RefreshList();
                listRompimiento2.RemoveItem(((ReportParameterList)sender).GetSelectedValue()[0]);
            };
            #endregion
            #region Filtro 6
            if (option.Equals("6"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                if (reportParameters["6"][0] == "")
                {
                    ((ReportParameterList)sender).RefreshList();
                    ReportParameterList listRompimiento1 = (ReportParameterList)this.RPControls["5"];
                    ((ReportParameterList)sender).RemoveItem(listRompimiento1.GetSelectedValue()[0]);
                };
            };
            #endregion
            #region Filtro Rango Cuentas

            if (option.Equals("filtroCuentas"))
            {
                Dictionary<string, string[]> rangoCuentas = this.GetValues();
                uc_MasterFind masterCtaIni = (uc_MasterFind)this.RPControls["CuentaIncial"];
                uc_MasterFind masterCtaFin = (uc_MasterFind)this.RPControls["CuentaFinal"];

                switch (rangoCuentas["filtroCuentas"][0])
                {
                    case ("True"):

                        masterCtaIni.Visible = true;
                        masterCtaFin.Visible = true;

                        break;

                    case ("False"):

                        masterCtaIni.Visible = false;
                        masterCtaFin.Visible = false;
                        this.CuentaInicial = string.Empty;
                        this.CuentaFinal = string.Empty;

                        break;
                }

            }

            #endregion
            #region Carga la cuenta Inicial

            if (option.Equals("CuentaIncial"))
            {
                uc_MasterFind masterCuentaIni = (uc_MasterFind)sender;
                this.CuentaInicial = masterCuentaIni.Value;// masterCuentaIni.ValidID ? masterCuentaIni.Value : string.Empty;
            }
            if (option.Equals("CuentaFinal"))
            {
                uc_MasterFind masterCuentaFinal = (uc_MasterFind)sender;
                this.CuentaFinal = masterCuentaFinal.Value;// masterCuentaFinal.ValidID ? masterCuentaFinal.Value : string.Empty;
            }
            #endregion
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            string reportName;
            string fileURl;
            try
            {
                switch (this._tipoReport)
                {
                    case "DePrueba":
                        reportName = this._bc.AdministrationModel.ReportesContabilidad_ReportBalancePruebas(this.año, this.LongitudCuenta, this.SaldoInicial, this.CuentaInicial, this.CuentaFinal, this.libro, this._tipoReport, this.Moneda, this.mesIni, this.mesFin, this.Combo1, this.Combo2, string.Empty, string.Empty);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                        Process.Start(fileURl);
                        break;
                    case "PorM":
                        reportName = this._bc.AdministrationModel.ReportesContabilidad_ReportBalancePruebas(this.año, this.LongitudCuenta, this.SaldoInicial, this.CuentaInicial, this.CuentaFinal, this.libro, this._tipoReport, this.Moneda, this.mesIni, this.mesFin, this.Combo1, this.Combo2, string.Empty, string.Empty);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                        Process.Start(fileURl);                       
                        break;
                    case "PorQ":
                        break;
                    case "EstResultados":
                        reportName = this._bc.AdministrationModel.ReportesContabilidad_ReportBalancePruebas(this.año, this.LongitudCuenta, this.SaldoInicial, this.CuentaInicial, this.CuentaFinal, this.libro, this._tipoReport, this.Moneda, this.mesIni, this.mesFin, this.Combo1, this.Combo2, this.proyecto, this.centroCto);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                        Process.Start(fileURl);
                        break;
                    case "EstResultadosMes":
                        reportName = this._bc.AdministrationModel.ReportesContabilidad_ReportBalancePruebas(this.año, this.LongitudCuenta, this.SaldoInicial, this.CuentaInicial, this.CuentaFinal, this.libro, this._tipoReport, this.Moneda, this.mesIni, this.mesFin, this.Combo1, this.Combo2,this.proyecto,this.centroCto);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                        Process.Start(fileURl);
                        break;

                    default:
                        #region Imprime el reporte de Libro mayor en Excel

                        //reportName = this._bc.AdministrationModel.ReportesContabilidad_PlantillaExcelLibroMayor(this._fechaIni.Year, this._fechaIni.Month, this.tipoBalance);
                        ///*this._cuentaIni, this._cuentaFin);*/

                        //if (reportName != string.Empty)
                        //{
                        //    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                        //    Process.Start(fileURl);
                        //}
                        //else
                        //    MessageBox.Show(this._errorConsuta);

                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Reporte_Balance.cs-LoadReportMethod"));
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
       
       