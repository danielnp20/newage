using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Clase padre de todos los formularios de parámetros para reportes
    /// </summary>
    public partial class ReportParametersForm : Form, IFiltrable
    {
        #region Delegados

        /// <summary>
        /// Delegado que actualiza la barra de progreso para reportes PDF
        /// </summary>
        /// <param name="progress"></param>
        protected delegate void LoadReport_PDF();
        protected LoadReport_PDF LoadReportDelegate_PDF;
        protected virtual void LoadReportMethod_PDF() { } //PDF

        ///// <summary>
        ///// Delegado que actualiza la barra de progreso para reportes XLS
        ///// </summary>
        //protected delegate void LoadReport_XLS();
        //protected LoadReport_XLS LoadReportDelegate_XLS;
        //protected virtual void LoadReportMethod_XLS() { }

        /// <summary>
        /// Delegado que finaliza la generacion del reporte
        /// </summary>
        public delegate void EndGenerar();
        public EndGenerar EndGenerarDelegate;
        public virtual void EndGenerarMethod()
        {
            this.btnFilter.Enabled = true;
            this.btnResetFilter.Enabled = true;
            this.periodoFilter1.Enabled = true;
            this.btnExportToPDF.Enabled = true;
            this.btnExportToXLS.Enabled = true;
        }

        #endregion

        #region Variables

        //Privadas
        private DTO_glConsulta _consulta = null;
        private List<ConsultasFields> Fields = null;

        //Protected
        protected BaseController _bc;
        protected MasterQuery mq;
        protected DateTime periodo;
        protected int documentReportID = 0;
        protected Dictionary<string, Control> RPControls = new Dictionary<string, Control>();

        protected ExportFormatType _formatType;
        protected DataTable _Query = new DataTable();
        #endregion

        #region Propiedades

        /// <summary>
        /// Filtros
        /// </summary>
        public DTO_glConsulta Consulta
        {
            get { return _consulta; }
            set
            {
                _consulta = value;
                this.lblFilter.Text = string.Empty;
                if (value != null && value.Filtros != null)
                {
                    foreach (DTO_glConsultaFiltro filtro in value.Filtros)
                    {
                        this.lblFilter.Text += filtro.CampoDesc + filtro.OperadorFiltro + filtro.ValorFiltro + "\n";
                    }
                }
            }
        }

        public ModulesPrefix Module
        {
            set
            {
                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(periodoStr);
            }
        }

        /// <summary>
        /// Propiedad para configurar las propiedades de cada formulario
        /// </summary>
        private AppReportParametersForm _reportForm;
        public AppReportParametersForm ReportForm
        {
            get { return _reportForm; }
            set
            {
                this._reportForm = value;

                //Tipo de libro
                string libro = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                //Libros
                List<ReportParameterListItem> tipoLibros = new List<ReportParameterListItem>();
                long count = 0;
                count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coBalanceTipo, null, null, true);
                IEnumerable<DTO_MasterBasic> TiposBalance = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coBalanceTipo, count, 1, null, null, true);
                foreach (var tipo in TiposBalance)
                    tipoLibros.Add(new ReportParameterListItem() { Key = tipo.ID.ToString(), Desc = tipo.ID.ToString() + " - " + tipo.Descriptivo.ToString() });

                switch (value)
                {
                    case AppReportParametersForm.coBalancePrueba:

                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePrueba).ToString());
                        #region Configurar Opciones
                        //Filtro para especificar el tipo de reporte a Imprimir
                        List<ReportParameterListItem> tipoReporteBalancePrueba = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() { Key = "DePrueba", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_dePrueba) },
                            new ReportParameterListItem() { Key = "PorM", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Months) },
                            new ReportParameterListItem() { Key = "PorQ", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Quarters) },
                            new ReportParameterListItem() { Key = "Comparativo", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Comparativo) },
                            new ReportParameterListItem() { Key = "General", Desc =  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_General) } ,
                            new ReportParameterListItem() { Key = "EstResultados", Desc = _bc.GetResource(LanguageTypes.Tables, "Estado de Resultados") } };

                //Filtro para Monedas
                List<ReportParameterListItem> tipoMoneda = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() {Key = "Local", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() {Key = "Foreign", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) },
                            new ReportParameterListItem() {Key = "Both", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) } };


                        this.AddList("3", (AppForms.ReportForm).ToString() + "_tipoReport", tipoReporteBalancePrueba, true, "DePrueba");
                        this.AddList("2", (AppForms.ReportForm).ToString() + "_Moneda", tipoMoneda, true, "1");
                        this.AddList("4", (AppForms.ReportForm).ToString() + "_TipoBalance", tipoLibros, true, libro);

                        List<ReportParameterListItem> cuentaLength = new List<ReportParameterListItem>();
                        for (int i = 1; i < 9; i++)
                            cuentaLength.Add(new ReportParameterListItem() { Key = i.ToString(), Desc = i.ToString() });
                        cuentaLength.Add(new ReportParameterListItem() { Key = "Max", Desc = "Maximum" });
                        this.AddList("8", (AppForms.ReportForm).ToString() + "_lblNumberOfDigits", cuentaLength, true, "Max");
                        this.AddCheck("9", "Con Saldo Inicial");
                        this.AddCheck("filtroCuentas", (AppForms.ReportForm).ToString() + "_RangoCuenta", true);
                        this.AddMaster("CuentaIncial", AppMasters.coPlanCuenta, true, null, false);
                        this.AddMaster("CuentaFinal", AppMasters.coPlanCuenta, true, null, false);

                        List<ReportParameterListItem> tipoReporteBalancePrueba_agrupamiento = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = "Consolidated", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consolidated) },
                            new ReportParameterListItem() { Key = "Detailed", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed) } };
                        this.AddList("1", (AppForms.ReportForm).ToString() + "_Agrupamiento", tipoReporteBalancePrueba_agrupamiento, true, "Consolidated");

                        List<ReportParameterListItem> rompimientosBalancePrueba = new List<ReportParameterListItem>() {                            
                            new ReportParameterListItem() { Key = "CentroCostoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CentroCosto) }, 
                            new ReportParameterListItem() { Key = "ProyectoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Proyecto) }, 
                            new ReportParameterListItem() { Key = "LineaPresupuestoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineaPresupuesto) }, 
                            new ReportParameterListItem() { Key = "-", Desc = "-" } };
                        this.AddList("5", (AppForms.ReportForm).ToString() + "_lblRompimiento1", rompimientosBalancePrueba, true, "CentroCostoID");
                        this.AddList("6", (AppForms.ReportForm).ToString() + "_lblRompimiento2", rompimientosBalancePrueba, false, "-");

                        CheckEdit saldoInicialInd = (CheckEdit)this.RPControls["9"];
                        saldoInicialInd.Checked = true;
                        ReportParameterList bpRompimiento1 = (ReportParameterList)this.RPControls["5"];
                        bpRompimiento1.Enabled = false;
                        bpRompimiento1.Visible = false;
                        ReportParameterList bpRompimiento2 = (ReportParameterList)this.RPControls["6"];
                        bpRompimiento2.Enabled = false;
                        bpRompimiento2.Visible = false;
                        #endregion
                        #region Configurar Filtros
                        this.btnExportToXLS.Visible = true;
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.Name = "monthCB_13";
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.Name = "monthCB_13";
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fields = new List<ConsultasFields>();
                        fields.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
                        fields.Add(new ConsultasFields("CentroCostoID", "Centro Costo", typeof(string)));
                        fields.Add(new ConsultasFields("ProyectoID", "Proyecto", typeof(string)));
                        fields.Add(new ConsultasFields("LineaPresupuestoID", "Linea Presupuesto", typeof(string)));
                        mq = new MasterQuery(this, AppReports.coBalanceDePrueba, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fields);
                        mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                        mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
                        mq.SetFK("ProyectoID", AppMasters.coProyecto, _bc.CreateFKConfig(AppMasters.coProyecto));
                        mq.SetFK("LineaPresupuestoID", AppMasters.plLineaPresupuesto, _bc.CreateFKConfig(AppMasters.plLineaPresupuesto));
                        #endregion
                        break;
                    case AppReportParametersForm.coSaldos:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coSaldos).ToString());
                        #region Configurar Opciones
                        //Campos para cargar el primer rompimiento
                        List<ReportParameterListItem> itemsSaldos = new List<ReportParameterListItem>()
                        {
                            new ReportParameterListItem() { Key = "CuentaID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) },
                            new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) },
                            new ReportParameterListItem() { Key = "CentroCostoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CentroCosto) }, 
                            new ReportParameterListItem() { Key = "ProyectoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Proyecto) }, 
                            new ReportParameterListItem() { Key = "LineaPresupuestoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineaPresupuesto) }, 
                            new ReportParameterListItem() { Key = "DocumentoID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Documento) },
                            new ReportParameterListItem() { Key = "-", Desc = "-" }
                        };

                        List<ReportParameterListItem> typeMoneda = new List<ReportParameterListItem>(){
                            new ReportParameterListItem() {Key = "Local", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() {Key = "Foreign", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) },
                            new ReportParameterListItem() {Key = "Both", Desc=_bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth) } };

                        this.AddList("1", (AppForms.ReportForm).ToString() + "_Moneda", typeMoneda, true, "Local");
                        this.AddList("2", (AppForms.ReportForm).ToString() + "_TipoBalance", tipoLibros, true, libro);
                        this.AddList("3", (AppForms.ReportForm).ToString() + "_lblRompimiento1", itemsSaldos, true, "CuentaID");
                        ReportParameterList sRompimiento1 = (ReportParameterList)this.RPControls["3"];
                        sRompimiento1.RemoveItem("-");
                        this.AddList("4", (AppForms.ReportForm).ToString() + "_lblRompimiento2", itemsSaldos, false, "-");
                        ReportParameterList sRompimiento2 = (ReportParameterList)this.RPControls["4"];
                        sRompimiento2.RemoveItem("CuentaID");
                        this.AddList("5", (AppForms.ReportForm).ToString() + "_lblRompimiento3", itemsSaldos, false, "-");
                        ReportParameterList sRompimiento3 = (ReportParameterList)RPControls["5"];
                        sRompimiento3.Enabled = false;
                        this.AddCheck("filtroCuentas", (AppForms.ReportForm).ToString() + "_RangoCuenta", true);
                        this.AddMaster("CuentaIncial", AppMasters.coPlanCuenta, true, null, false);
                        this.AddMaster("CuentaFinal", AppMasters.coPlanCuenta, true, null, false);
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.Name = "monthCB_13";
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.Name = "monthCB_13";
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fieldsSaldos = new List<ConsultasFields>();
                        fieldsSaldos.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
                        fieldsSaldos.Add(new ConsultasFields("CentroCostoID", "Centro Costo", typeof(string)));
                        fieldsSaldos.Add(new ConsultasFields("ProyectoID", "Proyecto", typeof(string)));
                        fieldsSaldos.Add(new ConsultasFields("LineaPresupuestoID", "Linea Presupuesto", typeof(string)));
                        fieldsSaldos.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        //fieldsSaldos.Add(new ConsultasFields("DocumentoID", "Documento", typeof(string))); 
                        mq = new MasterQuery(this, AppReports.coSaldos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsSaldos);
                        mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                        mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
                        mq.SetFK("ProyectoID", AppMasters.coProyecto, _bc.CreateFKConfig(AppMasters.coProyecto));
                        mq.SetFK("LineaPresupuestoID", AppMasters.plLineaPresupuesto, _bc.CreateFKConfig(AppMasters.plLineaPresupuesto));
                        mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        //mq.SetFK("DocumentoID", AppMasters.coDocumento, _bc.CreateFKConfig(AppMasters.coDocumento));
                        #endregion
                        break;
                    case AppReportParametersForm.coFormularios:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coFormularios).ToString());
                        #region Configurar Opciones
                        this.AddList("1", ReportParameterListSource.DeclaracionImpuesto, true);

                        List<ReportParameterListItem> formularioReportType = new List<ReportParameterListItem>()
                        {
                            new ReportParameterListItem() { Key = "Soporte", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Soporte) },
                            new ReportParameterListItem() { Key = "Cuenta", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) }, 
                        };
                        this.AddList("2", (AppForms.ReportForm).ToString() + "_TipoReport", formularioReportType, true, "Soporte");

                        ReportParameterList FormularioReportType = (ReportParameterList)this.RPControls["2"];
                        FormularioReportType.Enabled = false;
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(1);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.lblFromMonth.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblPeriodo");
                        this.btnFilter.Enabled = false;
                        this.btnResetFilter.Enabled = false;
                        this.btnFilter.Visible = false;
                        this.btnResetFilter.Visible = false;
                        this.periodoFilter1.Enabled = false;
                        #endregion
                        break;
                    case AppReportParametersForm.coRelacionDocumentos:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coRelacionDocumentos).ToString());
                        #region Configurar Opciones

                        #region Lista de los documentos
                        List<ReportParameterListItem> itemsDocumento_RelDoc = new List<ReportParameterListItem>();

                        long countCoDocs = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coDocumento, null, null, true);
                        List<DTO_coDocumento> coDocs = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coDocumento, countCoDocs, 1, null, null, true).Cast<DTO_coDocumento>().ToList();

                        Dictionary<string, bool> cacheCuenta = new Dictionary<string, bool>();
                        DTO_coPlanCuenta cuentaDTO = null;
                        DTO_glConceptoSaldo saldoDTO = null;

                        foreach (DTO_coDocumento item in coDocs)
                        {
                            if (!string.IsNullOrEmpty(item.CuentaLOC.Value) && !cacheCuenta.ContainsKey(item.CuentaLOC.Value))
                            {
                                cuentaDTO = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = item.CuentaLOC.Value }, true);
                                saldoDTO = (DTO_glConceptoSaldo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, new UDT_BasicID() { Value = cuentaDTO.ConceptoSaldoID.Value }, true);
                                if (saldoDTO.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Interno || saldoDTO.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Externo)
                                    cacheCuenta.Add(item.CuentaLOC.Value, true);
                                else
                                    cacheCuenta.Add(item.CuentaLOC.Value, false);
                            }
                            if (!string.IsNullOrEmpty(item.CuentaEXT.Value) && !cacheCuenta.ContainsKey(item.CuentaEXT.Value))
                            {
                                cuentaDTO = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = item.CuentaEXT.Value }, true);
                                saldoDTO = (DTO_glConceptoSaldo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, new UDT_BasicID() { Value = cuentaDTO.ConceptoSaldoID.Value }, true);
                                if (saldoDTO.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Interno || saldoDTO.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Externo)
                                    cacheCuenta.Add(item.CuentaEXT.Value, true);
                                else
                                    cacheCuenta.Add(item.CuentaEXT.Value, false);
                            }
                        }

                        List<string> docs = (from data in coDocs
                                             where !string.IsNullOrEmpty(data.CuentaLOC.Value) && cacheCuenta[data.CuentaLOC.Value] ||
                                                 !string.IsNullOrEmpty(data.CuentaEXT.Value) && cacheCuenta[data.CuentaEXT.Value]
                                             select data.DocumentoID.Value).Distinct().ToList();

                        DTO_glDocumento glDoc = null;
                        foreach (string doc in docs)
                        {
                            glDoc = (DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = doc }, true);
                            itemsDocumento_RelDoc.Add(new ReportParameterListItem() { Key = doc, Desc = doc + " - " + glDoc.Descriptivo.ToString() });
                        }
                        #endregion

                        List<ReportParameterListItem> rompimiento_RelDoc = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "Consecutivo", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consecutivo) }, 
                           new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) },
                           new ReportParameterListItem() { Key = "PorMeses", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PorMeses) },
                        };

                        this.AddList("1", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Documento), itemsDocumento_RelDoc, true);
                        //this.AddList("2", (AppForms.ReportForm).ToString() + "_lblEstado", itemsEstado, true, "3");
                        //this.AddList("3", ReportParameterListSource.Moneda, true, TipoMoneda.Both.ToString());
                        this.AddList("4", (AppForms.ReportForm).ToString() + "_lblRompimiento", rompimiento_RelDoc, true, "Consecutivo");
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fieldsRD = new List<ConsultasFields>();
                        //fieldsRD.Add(new ConsultasFields("DocumentoID", "Documento", typeof(string)));
                        fieldsRD.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        fieldsRD.Add(new ConsultasFields("PrefijoID", "Documetno Prefijo", typeof(string)));
                        fieldsRD.Add(new ConsultasFields("DocumentoNro", "Documento Numero", typeof(string)));
                        mq = new MasterQuery(this, AppReports.coRelacionDocumentos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsRD);
                        mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        mq.SetFK("PrefijoID", AppMasters.glPrefijo, _bc.CreateFKConfig(AppMasters.glPrefijo));
                        #endregion
                        break;
                    case AppReportParametersForm.coSaldosDocumentos:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coSaldosDocumentos).ToString());
                        #region Configurar Opciones
                        List<ReportParameterListItem> itemsDocumento_SaldoDoc = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = "11", Desc = "11 - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = "11" }, true))).Descriptivo.ToString() },
                            new ReportParameterListItem() { Key = "12", Desc = "12 - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = "12" }, true))).Descriptivo.ToString() },
                            new ReportParameterListItem() { Key = "21", Desc = "21 - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = "21" }, true))).Descriptivo.ToString() },
                            new ReportParameterListItem() { Key = "22", Desc = "22 - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = "22" }, true))).Descriptivo.ToString() }, };

                        List<ReportParameterListItem> tipoReporte_SaldosDoc = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = "Detailed", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed) },
                            new ReportParameterListItem() { Key = "Summarized", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Summarized) }, 
                            new ReportParameterListItem() { Key = "Consolidated", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consolidated) }, };

                        List<ReportParameterListItem> monedaOrigen_SaldosDoc = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = TipoMoneda.Local.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() { Key = TipoMoneda.Foreign.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) }, };

                        //List<ReportParameterListItem> itemsEstado = new List<ReportParameterListItem>()
                        //{
                        //    new ReportParameterListItem() { Key = "-1", Desc = ((EstadoDocControl)(-1)).ToString()},
                        //    new ReportParameterListItem() { Key = "0", Desc = ((EstadoDocControl)(0)).ToString()},
                        //    new ReportParameterListItem() { Key = "1", Desc = ((EstadoDocControl)(1)).ToString()},
                        //    new ReportParameterListItem() { Key = "2", Desc = ((EstadoDocControl)(2)).ToString()},
                        //    new ReportParameterListItem() { Key = "3", Desc = ((EstadoDocControl)(3)).ToString()},
                        //    new ReportParameterListItem() { Key = "4", Desc = ((EstadoDocControl)(4)).ToString()},
                        //};

                        this.AddList("1", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Documento), itemsDocumento_SaldoDoc, true, "12");
                        //this.AddList("2", (AppForms.ReportForm).ToString() + "_lblEstado", itemsEstado, true, "3");
                        this.AddList("3", ReportParameterListSource.Moneda, true, TipoMoneda.Both.ToString());
                        this.AddList("4", (AppForms.ReportForm).ToString() + "_lblRompimiento", new List<ReportParameterListItem> { new ReportParameterListItem() { Key = "CuentaID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account) }, new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) } }, true, "CuentaID");
                        this.AddList("5", (AppForms.ReportForm).ToString() + "_TipoReport", tipoReporte_SaldosDoc, true, "Detailed");
                        this.AddList("6", (AppForms.ReportForm).ToString() + "_MonedaOrigen", monedaOrigen_SaldosDoc, true, TipoMoneda.Local.ToString());

                        ReportParameterList monedaOr_SaldoDoc = (ReportParameterList)this.RPControls["6"];
                        monedaOr_SaldoDoc.Enabled = false;
                        monedaOr_SaldoDoc.Visible = false;
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        //List<ConsultasFields> fieldsSD = new List<ConsultasFields>();
                        //fieldsSD.Add(new ConsultasFields("CuentaID", "Cuenta", typeof(string)));
                        //fieldsSD.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        //fieldsSD.Add(new ConsultasFields("DocumentoPrefijo", "Documetno Prefijo", typeof(string)));
                        //fieldsSD.Add(new ConsultasFields("DocumentoNumero", "Documento Numero", typeof(string)));
                        //mq = new MasterQuery(this, AppReports.SaldosDocumentos, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsSD);
                        //mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                        //mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        //mq.SetFK("DocumentoPrefijo", AppMasters.glPrefijo, _bc.CreateFKConfig(AppMasters.glPrefijo));
                        #endregion
                        break;
                    case AppReportParametersForm.cpFacturasPorPagar:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPorPagar).ToString());
                        #region Configurar Opciones
                        List<ReportParameterListItem> itemsDocumento_FxP = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = "21", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_FacturasXPagar)},
                            new ReportParameterListItem() { Key = "22", Desc = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_AnticiposPend)}, 
                        };

                        List<ReportParameterListItem> tipoReporte_FacturasPorPagar = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = "Detailed", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Detailed) },
                            new ReportParameterListItem() { Key = "Summarized", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Summarized) }, };

                        List<ReportParameterListItem> monedaOrigen_FacturasPorPagar = new List<ReportParameterListItem>() {
                            new ReportParameterListItem() { Key = TipoMoneda.Local.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal) },
                            new ReportParameterListItem() { Key = TipoMoneda.Foreign.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign) }, };

                        this.AddList("1", _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Documento), itemsDocumento_FxP, true, "21");
                        this.AddList("2", (AppForms.ReportForm).ToString() + "_TipoReport", tipoReporte_FacturasPorPagar, true, "Detailed");
                        this.AddList("3", (AppForms.ReportForm).ToString() + "_MonedaOrigen", monedaOrigen_FacturasPorPagar, true, TipoMoneda.Local.ToString());
                        this.AddList("4", ReportParameterListSource.Moneda, true, TipoMoneda.Local.ToString());
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        //List<ConsultasFields> fieldsFP = new List<ConsultasFields>();
                        //fieldsFP.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        //mq = new MasterQuery(this, AppReports.FacturasPorPagar, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsFP);
                        //mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        //fieldsFP.Add(new ConsultasFields("AnticipoTipoID", "AnticipoTipo", typeof(string)));
                        //mq = new MasterQuery(this, AppReports.FacturasPorPagar, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsFP);
                        //mq.SetFK("TipoAnticipoID", AppMasters.cpAnticipoTipo, _bc.CreateFKConfig(AppMasters.cpAnticipoTipo));
                        #endregion
                        break;
                    case AppReportParametersForm.tsReciboCaja:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coReciboCaja).ToString());
                        #region Configurar Opciones

                        List<ReportParameterListItem> reciboReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "Consecutivo", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consecutivo) }, 
                           new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) },
                           new ReportParameterListItem() { Key = "ReciboCaja", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ReciboCaja) },
                        };

                        this.AddList("1", (AppForms.ReportForm).ToString() + "_tipoReport", reciboReportType, true, "Consecutivo");
                        //this.AddTextBox("2", false, (AppForms.ReportForm).ToString() + "_Caja");
                        this.AddList("2", ReportParameterListSource.Caja, true);
                        this.AddTextBox("3", true, (AppForms.ReportForm).ToString() + "_Recibo");

                        //ReportParameterTextBox tbCaja = (ReportParameterTextBox)this.RPControls["2"];                        
                        //tbCaja.Enabled = false;
                        //tbCaja.Visible = false;
                        ReportParameterList listCaja = (ReportParameterList)this.RPControls["2"];
                        listCaja.Enabled = false;
                        listCaja.Visible = false;
                        ReportParameterTextBox tbRecibo = (ReportParameterTextBox)this.RPControls["3"];
                        tbRecibo.Enabled = false;
                        tbRecibo.Visible = false;
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fieldsRC = new List<ConsultasFields>();
                        fieldsRC.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        mq = new MasterQuery(this, AppReports.coReciboCaja, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsRC);
                        mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        #endregion
                        break;
                    case AppReportParametersForm.cpMovimientosPeriodo:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpMovimientosPeriodo).ToString());
                        #region Configurar Opciones

                        List<ReportParameterListItem> movimientoCxPReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "Nombre", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Name) }, 
                           new ReportParameterListItem() { Key = "TerceroID", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero) },
                        };
                        this.AddList("1", (AppForms.ReportForm).ToString() + "_tipoReport", movimientoCxPReportType, true, "TerceroID");
                        this.AddList("2", ReportParameterListSource.MonedaExcl, true, TipoMoneda.Local.ToString());
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fieldsRM = new List<ConsultasFields>();
                        fieldsRM.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        mq = new MasterQuery(this, AppReports.coReciboCaja, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsRM);
                        mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        #endregion
                        break;
                    case AppReportParametersForm.noPrestamo:
                        this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.noPrestamo).ToString());
                        #region Configurar Opciones

                        //Se establece la lista del combo con sus respectivos valores
                        List<ReportParameterListItem> noPrestamoParameterReportType = new List<ReportParameterListItem>()
                        {
                           new ReportParameterListItem() { Key = "RelacionPrestamos", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_RelacionPendientes) }, 
                        };

                        //Determina el nombre del combo y el item donde debe quedar
                        this.AddList("1", (AppForms.ReportForm).ToString() + "_Tipo", noPrestamoParameterReportType, true, "RelacionPrestamos");
                        this.AddMaster("3", AppMasters.coTercero, false, null, true);
                        #endregion
                        #region Configurar Filtros
                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                        this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                        this.periodoFilter1.monthCB1.SelectedIndex = 0;
                        List<ConsultasFields> fieldsnoPrestamo = new List<ConsultasFields>();
                        fieldsnoPrestamo.Add(new ConsultasFields("TerceroID", "Tercero", typeof(string)));
                        mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsnoPrestamo);
                        mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                        #endregion
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ReportParametersForm()
        {
            InitializeComponent();
            try
            {
                _bc = BaseController.GetInstance();
                FormProvider.LoadResources(this, AppForms.ReportForm);

                this.LoadReportDelegate_PDF = new LoadReport_PDF(this.LoadReportMethod_PDF);
                //this.LoadReportDelegate_XLS = new LoadReport_XLS(this.LoadReportMethod_XLS);
                this.EndGenerarDelegate = new EndGenerar(this.EndGenerarMethod);

                this._formatType = ExportFormatType.pdf;
                this.InitReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm"));
            }
        }

        #region Funciones Protected

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected virtual void InitReport()
        {
            this.btnExportToPDF.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_btnExportToPDF");
            this.btnExportToXLS.Text = _bc.GetResource(LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_btnExportToXLS");
        }

        /// <summary>
        /// Metodo para generar el reporte PDF
        /// </summary>
        protected virtual void Report_PDF() { }

        /// <summary>
        /// Metodo para generar el reporte XLS
        /// </summary>
        protected virtual void Report_XLS() { }

        /// <summary>
        /// Retorna los valores de la opciones disponibles en el formulario
        /// </summary>
        /// <returns>Diccionario (opcion,valor) </returns>
        protected Dictionary<string, string[]> GetValues()
        {
            Dictionary<string, string[]> res = new Dictionary<string, string[]>();
            string[] value;
            foreach (KeyValuePair<string, Control> kvp in this.RPControls)
            {
                if (kvp.Value is IMultiReportParameter)
                {
                    value = (kvp.Value as IMultiReportParameter).GetSelectedValue();
                    res.Add(kvp.Key, value.ToArray());
                }

                if (kvp.Value is CheckEdit)
                {
                    value = new string[1] { (kvp.Value as CheckEdit).Checked.ToString() };
                    res.Add(kvp.Key, value.ToArray());
                }
            }
            return res;
        }

        /// <summary>
        /// Añade una lista para manejar una opción
        /// </summary>
        /// <param name="option">llave de la opcion</param>
        /// <param name="source">Fuente de la lista para llenar automaticamente los items</param>
        /// <param name="mandatory">Indica si es obligatoria de llenar</param>
        /// <param name="defaultKey">llave por defecto</param>    
        /// <param name="isVisible">Muestra u oculta el control de acuerdo a su necesidad</param>
        protected void AddList(string option, ReportParameterListSource source, bool mandatory = false, string defaultKey = "", bool isVisible = true)
        {
            ReportParameterList l1 = new ReportParameterList();
            l1.Visible = isVisible;
            l1.Width = 250;
            l1.ValueChangedEvent += new ListValueChangedHandler(EventListValueChanged);
            l1.Source = source;
            l1.Mandatory = mandatory;
            if (!string.IsNullOrWhiteSpace(defaultKey))
                l1.DefaultKey = defaultKey;
            try
            {
                RPControls.Add(option, l1);
                panelOptions.Controls.Add(l1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm.cs-AddList"));
            }
        }

        /// <summary>
        /// Añade una lista a una opcion dada una lista de items
        /// </summary>
        /// <param name="option">llave de la opcion</param>
        /// <param name="name">Nombre de la lista</param>
        /// <param name="items">Lista de items</param>
        /// <param name="mandatory">Indica si es obligatoria</param>
        /// <param name="defaultKey">Llave por defecto</param>
        /// <param name="isVisible">Muestra u oculta el control de acuerdo a su necesidad</param>
        protected void AddList(string option, string name, List<ReportParameterListItem> items, bool mandatory = false, string defaultKey = "", bool isVisible = true)
        {
            ReportParameterList l1 = new ReportParameterList();
            l1.Visible = isVisible;
            l1.Width = 250;
            l1.ValueChangedEvent += new ListValueChangedHandler(EventListValueChanged);
            l1.SetItems(name, items);
            l1.Mandatory = mandatory;
            if (!string.IsNullOrWhiteSpace(defaultKey))
                l1.DefaultKey = defaultKey;
            try
            {
                RPControls.Add(option, l1);
                panelOptions.Controls.Add(l1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm.cs-AddList"));
            }
        }

        /// <summary>
        /// Añade una casilla de check para una opcion
        /// </summary>
        /// <param name="opcion">Llave de la Opcion</param>
        /// <param name="name">Nombre de casilla</param>
        /// <param name="isVisible">Muestra u oculta el control de acuerdo a su necesidad</param>
        protected void AddCheck(string opcion, string name, bool isVisible = true)
        {
            CheckEdit check = new CheckEdit();
            check.CheckedChanged += new EventHandler(EventListValueChanged);
            check.Text = _bc.GetResource(LanguageTypes.Forms, name);
            check.Size = new Size(200, 10);
            check.Visible = isVisible;
            try
            {
                RPControls.Add(opcion, check);
                panelOptions.Controls.Add(check);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm.cs-AddCheck"));
            }
        }

        /// <summary>
        /// Añade unun campo para digitar los datos
        /// </summary>
        /// <param name="opcion">Llave de la Opcion</param>
        /// <param name="rangeInd">true - para ingresar los limites del diapasón de los valores(dos campos); false - para ingresar un valor </param>
        /// <param name="name">Nombre de casilla</param>
        /// <param name="isVisible">Muestra u oculta el control de acuerdo a su necesidad</param>
        protected void AddTextBox(string opcion, bool rangeInd, string name, bool isVisible = true)
        {
            ReportParameterTextBox text = new ReportParameterTextBox();
            text.FilterOptions = rangeInd ? Options.RangeCondition : Options.SingleCondition;
            text.SetItems(name, null);
            text.Visible = isVisible;
            try
            {
                RPControls.Add(opcion, text);
                panelOptions.Controls.Add(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm.cs-AddTextBox"));
            }
        }

        /// <summary>
        /// Añade un MasterFind 
        /// </summary>
        /// <param name="opcion">Opcion de control</param>
        /// <param name="masterDocID">Maestra para inicializar</param>
        /// <param name="onlyRoots"></param>
        /// <param name="filtros">Filtros </param>
        /// <param name="isVisible">Muestra u oculta el control de acuerdo a su necesidad</param>
        protected void AddMaster(string opcion, int masterDocID, bool onlyRoots, List<DTO_glConsultaFiltro> filtros = null, bool isVisible = true)
        {
            //Crea el control
            uc_MasterFind master = new uc_MasterFind();
            master.BackColor = Color.Transparent;
            master.Margin = new Padding(6);
            master.Size = new Size(291, 26);
            master.Visible = isVisible;
            try
            {
                master.Leave += new EventHandler(EventListValueChanged);
                RPControls.Add(opcion, master);
                panelOptions.Controls.Add(master);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReportParametersForm.cs-AddMaster"));
            }
            //Inicia el control
            _bc.InitMasterUC(master, masterDocID, true, true, onlyRoots, false, filtros);

        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Funcion llamada al cambiarse el valor de una lista
        /// </summary>
        /// <param name="option"></param>
        protected virtual void ListValueChanged(string option, object sender) { }

        /// <summary>
        /// Evento que captura el cambio de una lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void EventListValueChanged(object sender, EventArgs e)
        {
            string option = string.Empty;
            foreach (KeyValuePair<string, Control> kvp in RPControls)
            {
                if ((Control)kvp.Value == sender)
                    option = kvp.Key;
            }

            if (!string.IsNullOrEmpty(option))
                this.ListValueChanged(option, sender);
        }

        /// <summary>
        /// Abre la pantalla de filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnFilter_Click(object sender, EventArgs e)
        {
            if (mq != null)
                mq.ShowDialog();
        }

        /// <summary>
        /// Limpia la consulta de filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            this.Consulta = null;
        }

        /// <summary>
        /// Exporta la info a pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToPDF_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                switch (this._reportForm)
                {
                    case AppReportParametersForm.coLibroMayor:
                        if (this.periodoFilter1.Year[0] == 0 || this.periodoFilter1.Year[0] < 1900 || this.periodoFilter1.Year[0] > DateTime.Now.Year) throw new ArgumentNullException();
                        break;
                    case AppReportParametersForm.coBalancePrueba:
                        if (this.periodoFilter1.Year[0] == 0 || this.periodoFilter1.Year[0] < 1900 || this.periodoFilter1.Year[0] > DateTime.Now.Year) throw new ArgumentNullException();
                        if (this.periodoFilter1.FilterOptions == PeriodFilterOptions.YearMonthSpan && reportParameters["3"][0] == "DePrueba" && this.periodoFilter1.Months[1] < this.periodoFilter1.Months[0]) throw new ArgumentNullException();
                        break;
                    case AppReportParametersForm.coAuxiliar:
                    case AppReportParametersForm.coSaldos:
                        if (this.periodoFilter1.Year[0] == 0 || this.periodoFilter1.Year[0] < 1900 || this.periodoFilter1.Year[0] > DateTime.Now.Year) throw new ArgumentNullException();
                        if (this.periodoFilter1.FilterOptions == PeriodFilterOptions.YearMonthSpan && this.periodoFilter1.Months[1] < this.periodoFilter1.Months[0]) throw new ArgumentNullException();
                        break;
                }


                this.Report_PDF();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
            }
        }

        /// <summary>
        /// Exporta la info a xls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            this.Report_XLS();
        }

        #endregion

        #region Filtros

        /// <summary>
        /// Metodo que recibe la consulta seleccionada en la pantalla de filtros
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            this.Consulta = consulta;
            this.Fields = fields;
        }

        #endregion

    }
}
