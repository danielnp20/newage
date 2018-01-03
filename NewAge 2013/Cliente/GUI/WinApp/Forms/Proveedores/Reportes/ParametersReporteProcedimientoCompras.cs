using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class ParametersReporteProcedimientoCompras : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Se encarga de Generar el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string name = string.Empty;
                switch (this._report)
                {
                    case "solicitud":
                        
                            name = this._bc.AdministrationModel.ReportesProveedores_Solicitudes(this.filtros, this._formatType);
                            if (string.IsNullOrEmpty(name))
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                                return;
                            }           
                            break;
                        
                    case "ordenCompra":
                         
                            if (isDetallado == true)
                                reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenComprasDetallada(this._fechaIni, this._fechaFin, this._proveedor, this.filtros, true, this._moneda, this._formatType);
                            else
                                reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompras(this._fechaIni, this._fechaFin, this._proveedor, this.filtros, false, this._moneda, this._formatType);
                            
                            //Genera el PDF
                            if (reportName.Result == ResultValue.OK)
                                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ExtraField);
                            else
                            {
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                                return;
                            }
                          break;
                        
                    case "recibidos":
                        if (_facturado)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                            MessageBox.Show(msg);
                            return;
                        }
                        else
                        {
                            this.reportName = this._bc.AdministrationModel.ReportesProveedores_Recibidos(this._Periodo, this._proveedor, this.isDetallado, this.filtros,false, this._formatType);
                        }
                        break;
                    case "trazabilidad":
                        name = this._bc.AdministrationModel.Report_ProveedoresTrazabilizad(this._fechaIni, this._fechaFin,this.isDetallado?false: true, this._tipoEstado == "10" ? false : true, this._proveedor,this.proyecto);
                        if (string.IsNullOrEmpty(name))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                            return;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteProcedimientoCompras.cs", "LoadReportMethod"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        //protected override void LoadReportMethod_XLS()
        //{
        //    try
        //    {
        //        string name = string.Empty;
        //        switch (this._report)
        //        {
        //            case "solicitud":

        //                name = this._bc.AdministrationModel.ReportesProveedores_Solicitudes(this.filtros, this._formatType);
        //                if (string.IsNullOrEmpty(name))
        //                {
        //                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
        //                    return;
        //                }
        //                break;

        //            case "ordenCompra":

        //                if (isDetallado == true)
        //                    reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenComprasDetallada(this._fechaIni, this._fechaFin, this._proveedor, this.filtros, true, this._moneda, this._formatType);
        //                else
        //                    reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompras(this._fechaIni, this._fechaFin, this._proveedor, this.filtros, false, this._moneda, this._formatType);

        //                //Genera el PDF
        //                if (reportName.Result == ResultValue.OK)
        //                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ExtraField);
        //                else
        //                {
        //                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
        //                    MessageBox.Show(msg);
        //                    return;
        //                }
        //                break;

        //            case "recibidos":
        //                if (_facturado)
        //                {
        //                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
        //                    MessageBox.Show(msg);
        //                    return;
        //                }
        //                else
        //                {
        //                    this.reportName = this._bc.AdministrationModel.ReportesProveedores_Recibidos(this._Periodo, this._proveedor, this.isDetallado, this.filtros, false, this._formatType);
        //                }
        //                break;
        //            case "trazabilidad":
        //                name = this._bc.AdministrationModel.Report_ProveedoresTrazabilizad(this._fechaIni, this._fechaFin, this.isDetallado ? false : true, this._tipoEstado == "10" ? false : true, this._proveedor, this.proyecto);
        //                if (string.IsNullOrEmpty(name))
        //                {
        //                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
        //                    return;
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteProcedimientoCompras.cs", "LoadReportMethod"));
        //        throw;
        //    }
        //    finally
        //    {
        //        this.Invoke(this.EndGenerarDelegate);
        //    }
        //}

        #endregion

        #region Variable

        private string _report = "0", _tipoEstado = "0";
        private Dictionary<int, string> filtros = new Dictionary<int, string>();
        private string centroCosto = "";
        private string proyecto = string.Empty;
        private string _proveedor = string.Empty;
        private bool _facturado = false;
        private string _moneda = string.Empty;

        //Variables del hilo
        DateTime _Periodo,_fechaIni,_fechaFin;
        private DTO_TxResult reportName;
        private string fileURl;
        private bool isDetallado = true;

        
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de desahbilitar los controles que no se utilizan
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

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.pr;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.prProcedimientoCompras).ToString());
            
            #region Configurar Opciones

            #region Carga el Diccionario para filtros

            filtros.Add(0, string.Empty); //Centro de Costo
            filtros.Add(1, string.Empty); //Proyecto
            filtros.Add(2, string.Empty);
            filtros.Add(3, string.Empty);
            filtros.Add(4, string.Empty);
            filtros.Add(5, string.Empty); 
            filtros.Add(6, string.Empty); 
            filtros.Add(7, string.Empty); //Estado
            filtros.Add(8, string.Empty); //Proveedor
            filtros.Add(9, string.Empty); //Tipo Reporte
            filtros.Add(10, string.Empty);//Moneda
            
            


            #endregion

            #region Opciones de los combos

            #region Combos generales

            //Carga la Informacion del combo para saber el reporte a mostrar
            List<ReportParameterListItem> reporte = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem(){Key ="0", Desc = "-"},
                new ReportParameterListItem(){Key = "solicitud", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Solicitudes)},
                new ReportParameterListItem(){Key = "ordenCompra", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_OrdenCompra)},
                new ReportParameterListItem(){Key = "recibidos", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Recibidos)},
                new ReportParameterListItem(){Key = "trazabilidad", Desc = _bc.GetResource(LanguageTypes.Tables, "Trazabilidad Compras")}
            };

            List<ReportParameterListItem> estado = new List<ReportParameterListItem>()
            {
                
                new ReportParameterListItem(){Key = "10", Desc = "Todos"},
                new ReportParameterListItem(){Key = "11", Desc = "Pendientes"},                
            };

            List<ReportParameterListItem> moneda = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem(){Key="Loc", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal ) },
                new ReportParameterListItem(){Key="Ext", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign ) }
            //    new ReportParameterListItem(){Key="Ambas", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth ) }
            };

            //Carga la informacion para el combo del tipo de reporte a mostrar
            List<ReportParameterListItem> tipoReporte = new List<ReportParameterListItem>()
             {
                new ReportParameterListItem(){Key = "Detallado", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed)},
                new ReportParameterListItem(){Key = "Resumido", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Summarized)}
             };

            #endregion

            #region No Borrar
            //Carga el combo de prioridad
            //List<ReportParameterListItem> solicitudes = new List<ReportParameterListItem>(){
            //    new ReportParameterListItem(){Key="0", Desc = "-" },
            //    new ReportParameterListItem(){Key="1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Edicion ) },
            //    new ReportParameterListItem(){Key="2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PreAprobada ) },
            //    new ReportParameterListItem(){Key="3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Aprobadas ) },
            //    new ReportParameterListItem(){Key="4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_EnTramite ) },
            //    new ReportParameterListItem(){Key="5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Cumplidas ) } };

            ////Carga el combo de destino
            //List<ReportParameterListItem> destino = new List<ReportParameterListItem>(){
            //    new ReportParameterListItem(){Key="0", Desc = "-" },
            //    new ReportParameterListItem(){Key="ordenCompra", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_OrdenCompra ) },
            //    new ReportParameterListItem(){Key="contrato", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoContrato ) } }; 
            #endregion

            #endregion

            //Inicializa el reporte
            this.AddList("Reporte", "Tipo Reporte", reporte, true, "0");
            this.AddList("Estado", "Estado", estado, true, "1");

            this.AddList("TipoReporte", (AppForms.ReportForm).ToString() + "_Reporte", tipoReporte, true, "Detallado", false);
            this.AddMaster("Proveedor", AppMasters.prProveedor, true, null, false);
            this.AddCheck("NoFacturado", (AppForms.ReportForm).ToString() + "_NoFacturado",false);

            this.AddList("Moneda", (AppForms.ReportForm).ToString() + "_Moneda", moneda, true, "Loc");

            this.AddMaster("Proyecto", AppMasters.coProyecto, true, null, true);
            
            //this.AddMaster("centroCosto", AppMasters.coCentroCosto, true, null, false);
            //this.AddMaster("bienServicio", AppMasters.glBienServicioClase, true, null, false);
            //this.AddMaster("referencia", AppMasters.inReferencia, true, null, false);
            //this.AddMaster("area", AppMasters.glAreaFisica, true, null, false);
            //this.AddMaster("usuario", AppMasters.seUsuario, true, null, false);

            #endregion

            #region Configurar Filtros

            this.EnableControls();
            #endregion
        }

        /// <summary>
        /// Form Constructer (for Libro Diario Report)
        /// </summary>
        public ParametersReporteProcedimientoCompras()
        {
            this.Module = ModulesPrefix.pr;
            this.ReportForm = AppReportParametersForm.prProcedimientoCompras;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._Periodo = Convert.ToDateTime(fechaIniString);
                if (this._report == "ordenCompra" || this._report == "trazabilidad")
                {
                    int añoInicial = Convert.ToInt32(this.periodoFilter1.txtYear.Text);
                    int añoFinal = Convert.ToInt32(this.periodoFilter1.txtYear1.Text);
                    int mesIni = this.periodoFilter1.Months[0];
                    int mesFin = this.periodoFilter1.Months[1];

                    this._fechaIni = new DateTime(añoInicial, mesIni, 1);
                    this._fechaFin = new DateTime(añoFinal, mesFin, DateTime.DaysInMonth(añoFinal, mesFin));
                }

                if (this._report != "0")
                {
                    this.LoadReportMethod_PDF();
                }
                else
                {
                    string mensaje = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_NoSeGeneroReporte);
                    MessageBox.Show(mensaje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Valide que los parámetros y fechas sean correctas");
            }
        }

         ///<summary>
         ///General el reporte en excel  from the form
         ///</summary>
        protected override void Report_XLS()
        {
            try
            {
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._Periodo = Convert.ToDateTime(fechaIniString);
                this._fechaIni = this._Periodo;
                this._fechaFin = this._Periodo;

                if (this._report == "ordenCompra" || this._report == "trazabilidad")
                {
                    int añoInicial = Convert.ToInt32(this.periodoFilter1.txtYear.Text);
                    int añoFinal = Convert.ToInt32(this.periodoFilter1.txtYear1.Text);
                    int mesIni = this.periodoFilter1.Months[0];
                    int mesFin = this.periodoFilter1.Months[1];

                    this._fechaIni = new DateTime(añoInicial, mesIni, 1);
                    this._fechaFin = new DateTime(añoFinal, mesFin, DateTime.DaysInMonth(añoFinal, mesFin));
                }


                string name = string.Empty;
                this._Query = this._bc.AdministrationModel.ReportesProveedores_ProcedimientoComprasXLS(this.filtros,this._fechaIni,this._fechaFin);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_EjecucionPresupuesto.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Funcion que maneja los envento de los contorles
        /// </summary>
        /// <param name="option"></param>
        /// <param name="sender"></param>
        protected override void ListValueChanged(string option, object sender)
        {
            try
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["TipoReporte"];                
                ReportParameterList listReport = (ReportParameterList)this.RPControls["Reporte"];
                ReportParameterList listEstado = (ReportParameterList)this.RPControls["Estado"];
                Dictionary<string, string[]> reportParameters = this.GetValues();

                this._report = listReport.SelectedListItem.Key;
                this._tipoEstado = listEstado.SelectedListItem.Key;    

                #region Carga los controles de acuerdo a el reporte que se desea imprimir

                if (option.Equals("Reporte"))
                {
                    //Inicia los controles                
                    uc_MasterFind proveedor = (uc_MasterFind)this.RPControls["Proveedor"];
                    uc_MasterFind proyecto = (uc_MasterFind)this.RPControls["Proyecto"];
                    //ReportParameterList listReport = (ReportParameterList)this.RPControls["Reporte"];
                    //ReportParameterList listEstado = (ReportParameterList)this.RPControls["Estado"];
                    ReportParameterList moneda = (ReportParameterList)this.RPControls["Moneda"];

                    CheckEdit facturado = (CheckEdit)this.RPControls["NoFacturado"];

                    listReportType.Visible = true;                   
                    switch (this._report)
                    {
                        #region Carga los Controles necesarios para el reporte de Solicitudes
                            

                        case "solicitud":
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            proveedor.Visible = false;
                            proveedor.Value = string.Empty;
                            moneda.Visible = false;
                            this._proveedor = string.Empty;
                            facturado.Visible = true;
                            facturado.Checked = false;
                            this.btnExportToXLS.Visible = true;

                            #region Carga el combo de destino
                            //List<ReportParameterListItem> destino = new List<ReportParameterListItem>(){
                            //    new ReportParameterListItem(){Key="0", Desc = "-" },
                            //    new ReportParameterListItem(){Key="ordenCompra", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_OrdenCompra ) },
                            //    new ReportParameterListItem(){Key="contrato", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_DestinoContrato ) } };

                            #endregion;
                            //this.AddList("destino", (AppForms.ReportForm).ToString() + "_destino", destino, true, "0");

                            break;

                        #endregion

                        #region Carga los controles necesario para el reprote de Orden de Compra

                        case "ordenCompra":
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString(); 
                            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year+1).ToString();
                            this.periodoFilter1.monthCB.Items.Add(1);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            this.periodoFilter1.monthCB1.Items.Add(1);
                            this.periodoFilter1.monthCB1.SelectedIndex = 0;
                            proveedor.Visible = true;
                            moneda.Visible = true;
                            proveedor.Value = string.Empty;
                            this._proveedor = string.Empty;
                            facturado.Visible = false;
                            facturado.Checked = false;
                            this.btnExportToXLS.Visible = true;
                            break;

                        #endregion

                        #region Carga los controles necesarios para el reporte de Recibidos

                        case "recibidos":
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;
                            proveedor.Visible = true;
                            proveedor.Value = string.Empty;
                            this._proveedor = string.Empty;
                            facturado.Visible = true;
                            facturado.Checked = false;
                            moneda.Visible = false;
                            this.btnExportToXLS.Visible = true;
                            break;

                        #endregion

                        #region Carga los controles necesarios para el reporte de trazabilidad

                        case "trazabilidad":
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedItem = this.periodo.Date.Month;
                            this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                            this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB1.SelectedItem = this.periodo.Date.Month;
                            proveedor.Visible = true;
                            proveedor.Value = string.Empty;
                            this._proveedor = string.Empty;
                            proyecto.Visible = true;
                            proyecto.Value = string.Empty;
                            this.proyecto = string.Empty;
                            moneda.Visible = false;
                            this.btnExportToXLS.Visible = false;
                            break;

                            #endregion
                    }
                }

                #endregion
                #region Verifica si se va a imprimir el reporte detallado o Resumido
           



                if (option.Equals("TipoReporte"))
                {
                    var tipo = listReportType.SelectedListItem.Key;

                    if (tipo == "Detallado")
                    {
                        this.isDetallado = true;
                        this.btnExportToXLS.Visible = true;
                    }
                    else
                    {
                        this.isDetallado = false;
                        this.btnExportToXLS.Visible = false;
                    }
                }

                #endregion
                #region Valida Filtros
                if (option.Equals("centroCosto"))
                {
                    uc_MasterFind master = (uc_MasterFind)sender;
                    centroCosto = master.ValidID ? master.Value : string.Empty;
                    this.filtros[0] = centroCosto;
                }
                if (option.Equals("Proyecto"))
                {
                    uc_MasterFind master = (uc_MasterFind)sender;
                    this.proyecto = master.ValidID ? master.Value : string.Empty;
                    this.filtros[1] = this.proyecto;
                }

                this.filtros[9]= listReport.SelectedListItem.Key;

                if (option.Equals("Estado"))
                {
                    this.filtros[7] = this._tipoEstado;
                }
                
                //if (option.Equals("4"))
                //{
                //    uc_MasterFind master = (uc_MasterFind)sender;
                //    this.bienServicio = master.ValidID ? master.Value : string.Empty;
                //}


                
                if (option.Equals("Moneda"))
                {
                    ReportParameterList listReportMon = (ReportParameterList)this.RPControls["Moneda"];
                    this._moneda = listReportMon.SelectedListItem.Key;
                    this.filtros[10] = this._moneda;
                }


                if (option.Equals("Proveedor"))
                {
                    uc_MasterFind masterProveedor = (uc_MasterFind)sender;
                    if (masterProveedor.ValidID)
                    {
                        DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, masterProveedor.Value, true);
                        this._proveedor = prov.TerceroID.Value;
                        this.filtros[8] = this._proveedor;
                    }
                    else
                        this._proveedor = string.Empty;
                }


                if (option.Equals("NoFacturado"))
                {
                    switch (reportParameters["NoFacturado"][0])
                    {
                        case "True":
                            this._facturado = true;
                            break;
                        case "False":
                            this._facturado = false;
                            break;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersReporteProcedimientoCompras.cs", "ListValueChanged"));
            }
        }

        #endregion
    }
}
