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
    public class ParametersCarteraForm : ReportParametersForm, IFiltrable
    {
        #region Variables

        //Variable para reporte
        protected int documentReportID;
        protected DateTime? _fechaIni;
        protected DateTime _fechaFin;

        //Variables para filtros
        protected string _cliente = "";
        protected int? _libranza;
        protected string _zona = "";
        protected string _ciudad = "";
        protected string _concesionario = "";
        protected string _asesor = "";
        protected string _lineaCredi = "";
        protected string _compradorCatera = "";
        protected string _pagaduria = "";
        protected byte? _tipoReporte = 1;
        protected byte? _rompimiento;
        protected byte? _agrupamiento;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ParametersCarteraForm() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.cc;
            this.Text = _bc.GetResource(LanguageTypes.Forms, this.documentReportID.ToString());
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);

            #region Carga las lista de controles segun el Reporte
            if (this.documentReportID == AppReports.ccRepAnalisisPagos)
            {
                List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_All)}, 
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Creditos)}, 
                    new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_TotalRecaudos)}, 
                    new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RecaudosMasivos)}, 
                    new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RecaudosManuales)}, 
                    new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PagosTotal)}, 
                    new ReportParameterListItem() { Key = "7", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_AjustesCartera)}, 
                    new ReportParameterListItem() { Key = "8", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Refinanciacion)}, 
                };
                this.AddList("TipoReporte", "TipoReporte", reportType, true, "1");
            }
            else if (this.documentReportID == AppReports.ccReportesCartera)
            {
                List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Saldo)}, 
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Cuota)},
                };
                this.AddList("TipoReporte", "TipoReporte", reportType, true, "1");

                List<ReportParameterListItem> rompimiento = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "0", Desc = string.Empty}, 
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Concesionario)},
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Asesor)},
                    new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineaCredito)},
                    new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CompradorCart)},
                };
                this.AddList("Rompimiento", "Rompimiento", rompimiento, true, "2");
            }
            else if (this.documentReportID == AppReports.ccProyeccionVencim)
            {
                List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_Vencimiento5Dias)}, 
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables,DictionaryTables.Rpt_ProyeccionPagos)},
                };
                this.AddList("TipoReporte", "TipoReporte", reportType, true, "1");
            }

            else if (this.documentReportID == AppReports.ccEstadoCesionCartera)
            {
                List<ReportParameterListItem> reportType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "Estado Cuenta Cesión")}, 
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, "Amortización Derechos")},
                    new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, "Cesión y Recompra Mes")},
                    new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, "Rentabilidad Mensual")},
                    new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, "Detalle Saldo")},
                    new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, "Resumen Saldo")}
                };
                this.AddList("TipoReporte", "TipoReporte", reportType, true, "1");

                List<ReportParameterListItem> agrupamiento = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, "No Incluye Cartera Propia")}, 
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, "Incluye Cartera Propia")},
                    
                    //new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Resumido)}, 
                    //new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detallado)},
                };
                this.AddList("Agrupamiento", "Detalle", agrupamiento, true, "1");

            }
                // Crear Listas 
            else if (this.documentReportID == AppReports.ccCobroJuridico) // Jam
            {
                List<ReportParameterListItem> TipoReporte = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CJJuzgado)}, 
                    new ReportParameterListItem() { Key = "2", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CJTotal)},
                    new ReportParameterListItem() { Key = "3", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CJEstadoClie)},
                };
                this.AddList("TipoReporte", "TipoReporte", TipoReporte, true, "1");

                List<ReportParameterListItem> Estado = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "0", Desc = this._bc.GetResource(LanguageTypes.Tables, "Todos")}, 
                    new ReportParameterListItem() { Key = "3", Desc = this._bc.GetResource(LanguageTypes.Tables, "CJU")}, 
                    new ReportParameterListItem() { Key = "4", Desc = this._bc.GetResource(LanguageTypes.Tables, "ACP")},
                    new ReportParameterListItem() { Key = "5", Desc = this._bc.GetResource(LanguageTypes.Tables, "API")},
                };
                this.AddList("Estado", "Estado", Estado, false, "0",false);

                List<ReportParameterListItem> ClaseDeuda = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CJPrincipal)}, 
                    new ReportParameterListItem() { Key = "2", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CJAdicional)},
                };
                this.AddList("ClaseDeuda", "ClaseDeuda", ClaseDeuda, true, "1");

        
            }


            #endregion

            //Controles Filtro Generales     
            if (this.documentReportID == AppReports.ccEstadoCesionCartera)
                this.AddMaster("CompradorCarteraID", AppMasters.ccCompradorCartera, true, null);
            this.AddMaster("ClienteID", AppMasters.ccCliente, true, null);
            this.AddTextBox("Obligacion", false, "Obligación");          
            if (this.documentReportID != AppReports.ccCobroJuridico &&
                this.documentReportID != AppReports.ccPolizaEstado && this.documentReportID != AppReports.ccEstadoCesionCartera)
            {
                this.AddMaster("ZonaID", AppMasters.glZona, true, null);
                this.AddMaster("Ciudad", AppMasters.glLugarGeografico, true, null);
                this.AddMaster("ConcesionarioID", AppMasters.ccConcesionario, true, null);
                this.AddMaster("AsesorID", AppMasters.ccAsesor, true, null);
                this.AddMaster("LineaCreditoID", AppMasters.ccLineaCredito, true, null);
                this.AddMaster("CompradorCarteraID", AppMasters.ccCompradorCartera, true, null); 
            }

            #region Configuracion Filtros

            this.btnExportToXLS.Visible = true;
            if (this.documentReportID != AppReports.ccCobroJuridico)
            {
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB1.SelectedItem = this.periodo.Date.Month;

                this.btnFilter.Enabled = true;
                this.btnResetFilter.Enabled = true;
                this.btnFilter.Visible = true;
                this.btnResetFilter.Visible = true; 
            }
            if (this.documentReportID == AppReports.ccEstadoCesionCartera)
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            #endregion
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            ReportParameterList tipoReporte = new ReportParameterList();

            if(this.documentReportID != AppReports.ccPolizaEstado && 
                this.documentReportID != AppReports.ccAmortizacion
                && this.documentReportID != AppReports.ccSaldoCapital)
                tipoReporte = (ReportParameterList)this.RPControls["TipoReporte"];

            if (this.documentReportID == AppReports.ccCobroJuridico)
            {
                tipoReporte = (ReportParameterList)this.RPControls["TipoReporte"];
                if (tipoReporte.SelectedListItem.Key == "3")
                {
                    ReportParameterList tipoReporte2 = (ReportParameterList)this.RPControls["Estado"];
                    tipoReporte2.Visible=true;
                }
                else
                {
                    ReportParameterList tipoReporte2 = (ReportParameterList)this.RPControls["Estado"];
                    tipoReporte2.Visible = false;
                
                }
                
            }
            if (this.documentReportID == AppReports.ccEstadoCesionCartera)
            {
                 tipoReporte = (ReportParameterList)this.RPControls["TipoReporte"];
                 if (tipoReporte.SelectedListItem.Key == "5")
                 {
                     ReportParameterList txtAgrup = (ReportParameterList)this.RPControls["Agrupamiento"];
                     txtAgrup.Visible = true;
                 }
                 else
                 {
                     ReportParameterList txtAgrup = (ReportParameterList)this.RPControls["Agrupamiento"];
                     txtAgrup.Visible = false;
                 }
                 if(tipoReporte.SelectedListItem.Key != "1" && tipoReporte.SelectedListItem.Key != "4")
                    this.btnExportToXLS.Visible = true;
                 else
                    this.btnExportToXLS.Visible = false;

            }
            #region Carga el tipo de reporte a mostrar

            if (option.Equals("TipoReporte"))
                this._tipoReporte = Convert.ToByte(tipoReporte.SelectedListItem.Key);

            #endregion


            #region Carga el filtro por Cliente
            else if (option.Equals("ClienteID"))
            {
                uc_MasterFind masterCliente = (uc_MasterFind)sender;
                this._cliente = masterCliente.ValidID ? masterCliente.Value : string.Empty;
            }
            #endregion
            #region Carga el filtro por Tercero - Comprador Cart
            else if (option.Equals("TerceroID"))
            {
                uc_MasterFind masterCompr = (uc_MasterFind)sender;
                this._compradorCatera = masterCompr.ValidID ? masterCompr.Value : string.Empty;
            }
            #endregion
            #region Carga el filtro por Zona

            else if (option.Equals("ZonaID"))
            {
                uc_MasterFind masterZona = (uc_MasterFind)sender;
                this._zona = masterZona.ValidID ? masterZona.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Ciudad
            else if (option.Equals("Ciudad"))
            {
                uc_MasterFind masterCiudad = (uc_MasterFind)sender;
                this._ciudad = masterCiudad.ValidID ? masterCiudad.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Pagaduria

            else if (option.Equals("PagaduriaID"))
            {
                uc_MasterFind masterPagaduria = (uc_MasterFind)sender;
                this._concesionario = masterPagaduria.ValidID ? masterPagaduria.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por pagaduria

            else if (option.Equals("PagaduriaID"))
            {
                uc_MasterFind masterPagaduria = (uc_MasterFind)sender;
                this._concesionario = masterPagaduria.ValidID ? masterPagaduria.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Asesor

            else if (option.Equals("AsesorID"))
            {
                uc_MasterFind masterasesor = (uc_MasterFind)sender;
                this._asesor = masterasesor.ValidID ? masterasesor.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Linea de Credito

            else if (option.Equals("LineaCreditoID"))
            {
                uc_MasterFind masterlineaCre = (uc_MasterFind)sender;
                this._lineaCredi = masterlineaCre.ValidID ? masterlineaCre.Value : string.Empty;
            }

            #endregion
            #region Carga el filtro por Comprador de Cartera

            else if (option.Equals("CompradorCarteraID"))
            {
                uc_MasterFind mastercompCartera = (uc_MasterFind)sender;
                this._compradorCatera = mastercompCartera.ValidID ? mastercompCartera.Value : string.Empty;
            }

            #endregion

        }

        #endregion
    }
}
