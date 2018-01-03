using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Globalization;
using System.Reflection;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QuerySaldosCierre : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private List<DTO_coCierreMes> _cierresQuery;
        private List<DTO_QuerySaldosCierre> _cierresSaldos;
        private DTO_coCierreMes _filtro = null;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private string _unboundPrefix = "Unbound_";
        private TipoMoneda _tipoMoneda = TipoMoneda.Both;
        private int _documentID;
        private string _cuenta = string.Empty;
        private string _proyecto = string.Empty;
        private string _centroCosto = string.Empty;
        private string _lineaPresup = string.Empty;
        private string _conceptoCargo = string.Empty;
        private string _tercero = string.Empty;
        private RompimientoSaldos? romp1 = RompimientoSaldos.Cuenta;
        private RompimientoSaldos? romp2 = null;

        private string tipoDato;

        #endregion

        public QuerySaldosCierre()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                // Trae la fuente de datos y los filtra
                this.AddGridCols();
                this.AddGridSaldosCols();
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "DashBoard"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySaldosCierre;
            this._frmModule = ModulesPrefix.co;
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void InitControls()
        {
            try
            {
                //Carga controles de maestras
                this._bc.InitMasterUC(this.masterLibro, AppMasters.coBalanceTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
                this._bc.InitMasterUC(this.masterConceptoCargo, AppMasters.coConceptoCargo, true, true, true, false);
                this._bc.InitMasterUC(this.masterLineaPresup, AppMasters.plLineaPresupuesto, true, true, true, false);
                this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);

                // Carga el combo TipoMoneda
                Dictionary<string, string> dicTipoMon = new Dictionary<string, string>();             
                dicTipoMon.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal));
                dicTipoMon.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign));
                dicTipoMon.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth));
                this.cmbTipoMoneda.Properties.DataSource = dicTipoMon;

                //Recursos iniciales
                this._cuenta = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Account);
                this._lineaPresup = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_LineaPresupuesto);
                this._proyecto = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Proyecto);
                this._centroCosto = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CentroCosto);
                this._conceptoCargo = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ConcepCargo);
                this._tercero = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Tercero);                

                // Carga el Romp 1
                Dictionary<string, string> dicRomp1 = new Dictionary<string, string>();
                dicRomp1.Add("1", this._cuenta);
                dicRomp1.Add("2", this._proyecto);
                dicRomp1.Add("3", this._centroCosto);
                dicRomp1.Add("4", this._lineaPresup);
                dicRomp1.Add("5", this._conceptoCargo);
                dicRomp1.Add("6", this._tercero);
                this.cmbRomp1.Properties.DataSource = dicRomp1;
                this.cmbRomp1.EditValue = "1";

                // Carga el Romp 2
                Dictionary<string, string> dicRomp2 = new Dictionary<string, string>();
                dicRomp2.Add("0", "No Aplica");
                dicRomp2.Add("1", this._cuenta);
                dicRomp2.Add("2", this._proyecto);
                dicRomp2.Add("3", this._centroCosto);
                dicRomp2.Add("4", this._lineaPresup);
                dicRomp2.Add("5", this._conceptoCargo);
                dicRomp2.Add("6", this._tercero);
                this.cmbRomp2.Properties.DataSource = dicRomp2;
                this.cmbRomp2.EditValue = "3";

                //Obtiene valores iniciales
                string periodoActual = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);     
                string libroFuncional = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                bool multimoneda = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda).Equals("0") ? false : true;
                DateTime? ultimoCierre = this._bc.AdministrationModel.GetUltimoMesCerrado(ModulesPrefix.co);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoActual);
                this.masterLibro.Value = libroFuncional;
                if (!multimoneda)
                {
                    this.cmbTipoMoneda.EditValue = "1"; //Local
                    this.cmbTipoMoneda.Properties.ReadOnly = true;
                }
                else
                    this.cmbTipoMoneda.EditValue = "3"; //Ambas

                //Carga los saldos de meses con $0
                this._cierresSaldos = new List<DTO_QuerySaldosCierre>();
                for (int i = 1; i <= 12; i++)
                {
                    DTO_QuerySaldosCierre cierre = new DTO_QuerySaldosCierre();
                    cierre.MesNro = i;
                    cierre.Mes = this.GetNameMonth(i);
                    cierre.SaldoLocalIni.Value = 0;
                    cierre.SaldoExtraIni.Value = 0;
                    cierre.LocalDB.Value = 0;
                    cierre.LocalCR.Value = 0;
                    cierre.ExtraDB.Value = 0;
                    cierre.ExtraCR.Value = 0;
                    cierre.SaldoLocalFinal.Value = 0;
                    cierre.SaldoExtraFinal.Value = 0;
                    this._cierresSaldos.Add(cierre);
                }
                this.gcSaldos.DataSource = this._cierresSaldos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Obtiene el nombre del mes requerido
        /// </summary>
        /// <param name="numeroMes">numero del mes</param>
        /// <returns>Nombre del Mes</returns>
        private string GetNameMonth (int numeroMes) 
        { 
            try 
            { 
               DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
               string nombreMesAbreviado = formatoFecha.GetMonthName(numeroMes);
               return nombreMesAbreviado; 
            } 
            catch 
            { return ""; } 
        } 

        /// <summary>
        /// Funcion  para cargar los saldos de los meses
        /// </summary>
        private void LoadSaldos(DTO_coCierreMes cierresNew)
        {
            try
            {
                foreach (DTO_QuerySaldosCierre mes in this._cierresSaldos)
                {                   
                    switch (mes.MesNro)
                    {
                        case 1: //Enero
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI01.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI01.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB01.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR01.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB01.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR01.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI01.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI01.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 2: //Febrero
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI02.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI02.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB02.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR02.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB02.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR02.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI02.Value +  mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI02.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 3: //Marzo
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI03.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI03.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB03.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR03.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB03.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR03.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI03.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI03.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 4://Abril
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI04.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI04.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB04.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR04.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB04.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR04.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI04.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI04.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 5://Mayo
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI05.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI05.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB05.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR05.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB05.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR05.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI05.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI05.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 6://Junio
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI06.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI06.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB06.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR06.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB06.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR06.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI06.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 7://Julio
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI07.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI07.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB07.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR07.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB07.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR07.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI07.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI07.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 8://Agosto
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI08.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI08.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB08.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR08.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB08.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR08.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI08.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI08.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 9://Septiembre
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI09.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI09.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB09.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR09.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB09.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR09.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI09.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI09.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 10://Octubre
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI10.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI10.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB10.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR10.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB10.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR10.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI10.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI10.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 11://Noviembre
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI11.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI11.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB11.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR11.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB11.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR11.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI11.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI11.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                        case 12://Diciembre
                            mes.SaldoLocalIni.Value = cierresNew.LocalINI12.Value;
                            mes.SaldoExtraIni.Value = cierresNew.ExtraINI12.Value;
                            mes.LocalDB.Value = cierresNew.LocalDB12.Value;
                            mes.LocalCR.Value = cierresNew.LocalCR12.Value;
                            mes.ExtraDB.Value = cierresNew.ExtraDB12.Value;
                            mes.ExtraCR.Value = cierresNew.ExtraCR12.Value;
                            mes.SaldoLocalFinal.Value = cierresNew.LocalINI12.Value + mes.LocalDB.Value + mes.LocalCR.Value;
                            mes.SaldoExtraFinal.Value = cierresNew.ExtraINI12.Value + mes.ExtraDB.Value + mes.ExtraCR.Value;
                        break;
                     }
                 }
                this.gcSaldos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "GetData"));
            }
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {                
                #region Columnas de grilla principal

                //CuentaID
                GridColumn CuentaID = new GridColumn();
                CuentaID.FieldName = this._unboundPrefix + "CuentaID";
                CuentaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuentaID");
                CuentaID.UnboundType = UnboundColumnType.String;
                CuentaID.VisibleIndex = 0;
                CuentaID.Width = 60;
                CuentaID.Visible = true;
                CuentaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CuentaID);

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CentroCostoID");
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.VisibleIndex = 0;
                CentroCostoID.Width = 60;
                CentroCostoID.Visible = false;
                CentroCostoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CentroCostoID);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 60;
                ProyectoID.Visible = false;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                //LineaPresupuesto
                GridColumn LineaPresupuesto = new GridColumn();
                LineaPresupuesto.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuesto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaPresupuestoID");
                LineaPresupuesto.UnboundType = UnboundColumnType.String;
                LineaPresupuesto.VisibleIndex = 0;
                LineaPresupuesto.Width = 60;
                LineaPresupuesto.Visible = false;
                LineaPresupuesto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(LineaPresupuesto);

                //ConceptoCargoID
                GridColumn ConceptoCargoID = new GridColumn();
                ConceptoCargoID.FieldName = this._unboundPrefix + "ConceptoCargoID";
                ConceptoCargoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ConceptoCargoID");
                ConceptoCargoID.UnboundType = UnboundColumnType.String;
                ConceptoCargoID.VisibleIndex = 0;
                ConceptoCargoID.Width = 60;
                ConceptoCargoID.Visible = false;
                ConceptoCargoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ConceptoCargoID);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 0;
                TerceroID.Width = 60;
                TerceroID.Visible = false;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 6;
                Descriptivo.Width = 180;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                //SaldoLoc
                GridColumn SaldoLoc = new GridColumn();
                SaldoLoc.FieldName = this._unboundPrefix + "SaldoLocal";
                SaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLoc");
                SaldoLoc.UnboundType = UnboundColumnType.Decimal;
                SaldoLoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLoc.AppearanceCell.Options.UseTextOptions = true;
                SaldoLoc.VisibleIndex = 7;
                SaldoLoc.Width = 90;
                SaldoLoc.Visible = true;
                SaldoLoc.ColumnEdit = this.TextEdit;
                SaldoLoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoLoc);

                //SaldoExtra
                GridColumn SaldoExtra = new GridColumn();
                SaldoExtra.FieldName = this._unboundPrefix + "SaldoExtra";
                SaldoExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExtra");
                SaldoExtra.UnboundType = UnboundColumnType.Decimal;
                SaldoExtra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoExtra.AppearanceCell.Options.UseTextOptions = true;
                SaldoExtra.VisibleIndex = 8;
                SaldoExtra.Width = 90;
                SaldoExtra.Visible = true;
                SaldoExtra.ColumnEdit = this.TextEdit;
                SaldoExtra.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoExtra);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;

                #endregion

                #region Grilla Interna de Detalle

                //CuentaID
                GridColumn CuentaIDDeta = new GridColumn();
                CuentaIDDeta.FieldName = this._unboundPrefix + "CuentaID";
                CuentaIDDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuentaID");
                CuentaIDDeta.UnboundType = UnboundColumnType.String;
                CuentaIDDeta.VisibleIndex = 0;
                CuentaIDDeta.Width = 60;
                CuentaIDDeta.Visible = false;
                CuentaIDDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CuentaIDDeta);

                //CentroCostoID
                GridColumn CentroCostoIDdeta = new GridColumn();
                CentroCostoIDdeta.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoIDdeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CentroCostoID");
                CentroCostoIDdeta.UnboundType = UnboundColumnType.String;
                CentroCostoIDdeta.VisibleIndex = 0;
                CentroCostoIDdeta.Width = 60;
                CentroCostoIDdeta.Visible = true;
                CentroCostoIDdeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CentroCostoIDdeta);

                //ProyectoID
                GridColumn ProyectoIDDeta = new GridColumn();
                ProyectoIDDeta.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoIDDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoIDDeta.UnboundType = UnboundColumnType.String;
                ProyectoIDDeta.VisibleIndex = 0;
                ProyectoIDDeta.Width = 60;
                ProyectoIDDeta.Visible = false;
                ProyectoIDDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ProyectoIDDeta);

                //LineaPresupuesto
                GridColumn LineaPresupuestoDeta = new GridColumn();
                LineaPresupuestoDeta.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuestoDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaPresupuestoID");
                LineaPresupuestoDeta.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDeta.VisibleIndex = 0;
                LineaPresupuestoDeta.Width = 60;
                LineaPresupuestoDeta.Visible = false;
                LineaPresupuestoDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(LineaPresupuestoDeta);

                //ConceptoCargoID
                GridColumn ConceptoCargoIDDeta = new GridColumn();
                ConceptoCargoIDDeta.FieldName = this._unboundPrefix + "ConceptoCargoID";
                ConceptoCargoIDDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ConceptoCargoID");
                ConceptoCargoIDDeta.UnboundType = UnboundColumnType.String;
                ConceptoCargoIDDeta.VisibleIndex = 0;
                ConceptoCargoIDDeta.Width = 60;
                ConceptoCargoIDDeta.Visible = false;
                ConceptoCargoIDDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ConceptoCargoIDDeta);

                //TerceroID
                GridColumn TerceroIDDeta = new GridColumn();
                TerceroIDDeta.FieldName = this._unboundPrefix + "TerceroID";
                TerceroIDDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroIDDeta.UnboundType = UnboundColumnType.String;
                TerceroIDDeta.VisibleIndex = 0;
                TerceroIDDeta.Width = 60;
                TerceroIDDeta.Visible = false;
                TerceroIDDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(TerceroIDDeta);

                //Descriptivo
                GridColumn DescriptivoDeta = new GridColumn();
                DescriptivoDeta.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta.UnboundType = UnboundColumnType.String;
                DescriptivoDeta.VisibleIndex = 6;
                DescriptivoDeta.Width = 180;
                DescriptivoDeta.Visible = true;
                DescriptivoDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DescriptivoDeta);

                //SaldoLoc
                GridColumn SaldoLocDeta = new GridColumn();
                SaldoLocDeta.FieldName = this._unboundPrefix + "SaldoLocal";
                SaldoLocDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLoc");
                SaldoLocDeta.UnboundType = UnboundColumnType.Decimal;
                SaldoLocDeta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLocDeta.AppearanceCell.Options.UseTextOptions = true;
                SaldoLocDeta.VisibleIndex = 7;
                SaldoLocDeta.Width = 90;
                SaldoLocDeta.Visible = true;
                SaldoLocDeta.ColumnEdit = this.TextEdit;
                SaldoLocDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SaldoLocDeta);

                //SaldoExtra
                GridColumn SaldoExtraDeta = new GridColumn();
                SaldoExtraDeta.FieldName = this._unboundPrefix + "SaldoExtra";
                SaldoExtraDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExtra");
                SaldoExtraDeta.UnboundType = UnboundColumnType.Decimal;
                SaldoExtraDeta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoExtraDeta.AppearanceCell.Options.UseTextOptions = true;
                SaldoExtraDeta.VisibleIndex = 8;
                SaldoExtraDeta.Width = 90;
                SaldoExtraDeta.Visible = true;
                SaldoExtraDeta.ColumnEdit = this.TextEdit;
                SaldoExtraDeta.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SaldoExtraDeta);

                this.gvDetalle.OptionsView.ColumnAutoWidth = true;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridSaldosCols()
        {
            try
            {
                #region Columnas de grilla principal

                //Mes
                GridColumn Periodo = new GridColumn();
                Periodo.FieldName = this._unboundPrefix + "Mes";
                Periodo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Mes");
                Periodo.UnboundType = UnboundColumnType.String;
                Periodo.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Periodo.AppearanceCell.Options.UseTextOptions = true;
                Periodo.AppearanceCell.Options.UseFont = true;
                Periodo.VisibleIndex = 0;
                Periodo.Width = 75;
                Periodo.Visible = true;
                Periodo.Fixed = FixedStyle.Left;
                Periodo.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(Periodo);

                //SaldoLocalIni
                GridColumn SaldoLocalIni = new GridColumn();
                SaldoLocalIni.FieldName = this._unboundPrefix + "SaldoLocalIni";
                SaldoLocalIni.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLocalIni");
                SaldoLocalIni.UnboundType = UnboundColumnType.Decimal;
                SaldoLocalIni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLocalIni.AppearanceCell.Options.UseTextOptions = true;
                SaldoLocalIni.VisibleIndex = 1;
                SaldoLocalIni.Width = 100;
                SaldoLocalIni.Visible = true;
                SaldoLocalIni.ColumnEdit = this.TextEdit;
                SaldoLocalIni.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(SaldoLocalIni);

                //SaldoExtraIni
                GridColumn SaldoExtraIni = new GridColumn();
                SaldoExtraIni.FieldName = this._unboundPrefix + "SaldoExtraIni";
                SaldoExtraIni.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExtraIni");
                SaldoExtraIni.UnboundType = UnboundColumnType.Decimal;
                SaldoExtraIni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoExtraIni.AppearanceCell.Options.UseTextOptions = true;
                SaldoExtraIni.VisibleIndex = 2;
                SaldoExtraIni.Width = 100;
                SaldoExtraIni.Visible = true;
                SaldoExtraIni.ColumnEdit = this.TextEdit;
                SaldoExtraIni.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(SaldoExtraIni);

                //LocalDB
                GridColumn LocalDB = new GridColumn();
                LocalDB.FieldName = this._unboundPrefix + "LocalDB";
                LocalDB.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LocalDB");
                LocalDB.UnboundType = UnboundColumnType.Decimal;
                LocalDB.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                LocalDB.AppearanceCell.Options.UseTextOptions = true;
                LocalDB.VisibleIndex = 3;
                LocalDB.Width = 100;
                LocalDB.Visible = true;
                LocalDB.ColumnEdit = this.TextEdit;
                LocalDB.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(LocalDB);

                //LocalCR
                GridColumn LocalCR = new GridColumn();
                LocalCR.FieldName = this._unboundPrefix + "LocalCR";
                LocalCR.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LocalCR");
                LocalCR.UnboundType = UnboundColumnType.Decimal;
                LocalCR.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                LocalCR.AppearanceCell.Options.UseTextOptions = true;
                LocalCR.VisibleIndex = 4;
                LocalCR.Width = 100;
                LocalCR.Visible = true;
                LocalCR.ColumnEdit = this.TextEdit;
                LocalCR.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(LocalCR);

                //ExtraDB
                GridColumn ExtraDB = new GridColumn();
                ExtraDB.FieldName = this._unboundPrefix + "ExtraDB";
                ExtraDB.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ExtraDB");
                ExtraDB.UnboundType = UnboundColumnType.Decimal;
                ExtraDB.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ExtraDB.AppearanceCell.Options.UseTextOptions = true;
                ExtraDB.VisibleIndex = 5;
                ExtraDB.Width = 100;
                ExtraDB.Visible = true;
                ExtraDB.ColumnEdit = this.TextEdit;
                ExtraDB.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(ExtraDB);

                //ExtraCR
                GridColumn ExtraCR = new GridColumn();
                ExtraCR.FieldName = this._unboundPrefix + "ExtraCR";
                ExtraCR.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ExtraCR");
                ExtraCR.UnboundType = UnboundColumnType.Decimal;
                ExtraCR.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ExtraCR.AppearanceCell.Options.UseTextOptions = true;
                ExtraCR.VisibleIndex = 6;
                ExtraCR.Width = 100;
                ExtraCR.Visible = true;
                ExtraCR.ColumnEdit = this.TextEdit;
                ExtraCR.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(ExtraCR);

                //SaldoLocalFinal
                GridColumn SaldoLocalFinal = new GridColumn();
                SaldoLocalFinal.FieldName = this._unboundPrefix + "SaldoLocalFinal";
                SaldoLocalFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoLocalFinal");
                SaldoLocalFinal.UnboundType = UnboundColumnType.Decimal;
                SaldoLocalFinal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoLocalFinal.AppearanceCell.Options.UseTextOptions = true;
                SaldoLocalFinal.VisibleIndex = 7;
                SaldoLocalFinal.Width = 100;
                SaldoLocalFinal.Visible = true;
                SaldoLocalFinal.ColumnEdit = this.TextEdit;
                SaldoLocalFinal.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(SaldoLocalFinal);

                //SaldoExtraFinal
                GridColumn SaldoExtraFinal = new GridColumn();
                SaldoExtraFinal.FieldName = this._unboundPrefix + "SaldoExtraFinal";
                SaldoExtraFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExtraFinal");
                SaldoExtraFinal.UnboundType = UnboundColumnType.Decimal;
                SaldoExtraFinal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoExtraFinal.AppearanceCell.Options.UseTextOptions = true;
                SaldoExtraFinal.VisibleIndex = 7;
                SaldoExtraFinal.Width = 100;
                SaldoExtraFinal.Visible = true;
                SaldoExtraFinal.ColumnEdit = this.TextEdit;
                SaldoExtraFinal.OptionsColumn.AllowEdit = false;
                this.gvSaldos.Columns.Add(SaldoExtraFinal);

                this.gvSaldos.BestFitColumns();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_FormClosing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_FormClosed"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
        }

        #endregion Eventos MDI

        #region Eventos 

        /// <summary>
        /// Valida el tipo de moneda al seleccionarla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoMoneda_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._tipoMoneda = (TipoMoneda)Enum.Parse(typeof(TipoMoneda), this.cmbTipoMoneda.EditValue.ToString());

                if (this._tipoMoneda == TipoMoneda.Local)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "SaldoExtra"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoExtra"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraIni"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "ExtraDB"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "ExtraCR"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraFinal"].Visible = false;

                    this.gvDocument.Columns[this._unboundPrefix + "SaldoLocal"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoLocal"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalIni"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalDB"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalCR"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalFinal"].Visible = true;
                }
                else if (this._tipoMoneda == TipoMoneda.Foreign)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "SaldoLocal"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoLocal"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalIni"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalDB"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalCR"].Visible = false;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalFinal"].Visible = false;

                    this.gvDocument.Columns[this._unboundPrefix + "SaldoExtra"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoExtra"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraIni"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalDB"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalCR"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraFinal"].Visible = true;
                }
                else if (this._tipoMoneda == TipoMoneda.Both)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "SaldoLocal"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoLocal"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalIni"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalDB"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalCR"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoLocalFinal"].Visible = true;

                    this.gvDocument.Columns[this._unboundPrefix + "SaldoExtra"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "SaldoExtra"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraIni"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalDB"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "LocalCR"].Visible = true;
                    this.gvSaldos.Columns[this._unboundPrefix + "SaldoExtraFinal"].Visible = true;
                }
                this.gvDocument.RefreshData();
                this.gvSaldos.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySaldoCierre.cs", "cmbTipoMoneda_EditValueChanged"));
            }
        }

        /// <summary>
        /// Valida el tipo de romp 1 al seleccionarlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbRomp1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.romp1 = (RompimientoSaldos)Enum.Parse(typeof(RompimientoSaldos), this.cmbRomp1.EditValue.ToString());
                if (this.romp1 == RompimientoSaldos.Cuenta)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp1 == RompimientoSaldos.Proyecto)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp1 == RompimientoSaldos.CentroCosto)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp1 == RompimientoSaldos.LineaPresupuesto)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp1 == RompimientoSaldos.ConceptoCargo)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp1 == RompimientoSaldos.Tercero)
                {
                    this.gvDocument.Columns[this._unboundPrefix + "TerceroID"].Visible = true;
                    this.gvDocument.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDocument.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;                  
                }
                //this.gvDocument.RefreshData();
                //this.gvDetalle.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySaldoCierre.cs", "cmbRomp1_EditValueChanged"));
            }
        }

        /// <summary>
        /// Valida el tipo de romp 2 al seleccionarlo 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbRomp2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbRomp2.EditValue != "0")
                    this.romp2 = (RompimientoSaldos)Enum.Parse(typeof(RompimientoSaldos), this.cmbRomp2.EditValue.ToString());
                else
                    this.romp2 = null;
                if (this.romp2 == RompimientoSaldos.Cuenta)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp2 == RompimientoSaldos.Proyecto)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp2 == RompimientoSaldos.CentroCosto)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp2 == RompimientoSaldos.LineaPresupuesto)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp2 == RompimientoSaldos.ConceptoCargo)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = false;
                }
                else if (this.romp2 == RompimientoSaldos.Tercero)
                {
                    this.gvDetalle.Columns[this._unboundPrefix + "TerceroID"].Visible = true;
                    this.gvDetalle.Columns[this._unboundPrefix + "CuentaID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "LineaPresupuestoID"].Visible = false;
                    this.gvDetalle.Columns[this._unboundPrefix + "ConceptoCargoID"].Visible = false;                 
                }
                //this.gvDocument.RefreshData();
                //this.gvDetalle.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySaldoCierre.cs", "cmbRomp2_EditValueChanged"));
            }
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                    }
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Se realiza cuando recorre la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DTO_coCierreMes mes = this.gvDocument.GetRow(e.FocusedRowHandle) != null ? (DTO_coCierreMes)this.gvDocument.GetRow(e.FocusedRowHandle) : new DTO_coCierreMes(true);
                this.LoadSaldos(mes);
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySaldosCierre", "gvDocument_FocusedRowChanged.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Se realiza cuando se clickea cada fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (this.gvDocument.DataRowCount > 0)
            {
                DTO_coCierreMes mes = this._cierresQuery[this.gvDocument.FocusedRowHandle].Detalle[e.RowHandle];
                this.LoadSaldos(mes);
            }
        }

        /// <summary>
        /// Se realiza cuando se clickea cada fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (this.gvDocument.DataRowCount == 1)
            {
                DTO_coCierreMes mes = this._cierresQuery[this.gvDocument.FocusedRowHandle];
                this.LoadSaldos(mes);
            }

        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._cierresQuery = new List<DTO_coCierreMes>();

                //Resetea los saldos de meses con $0
                foreach (var reset in  this._cierresSaldos)
                {
                    reset.SaldoLocalIni.Value = 0;
                    reset.SaldoExtraIni.Value = 0;
                    reset.LocalDB.Value = 0;
                    reset.LocalCR.Value = 0;
                    reset.ExtraDB.Value = 0;
                    reset.ExtraCR.Value = 0;
                    reset.SaldoLocalFinal.Value = 0;
                    reset.SaldoExtraFinal.Value = 0;
                }
                this.gcDocument.DataSource = this._cierresQuery;
                this.gvDocument.RefreshData();
                this.gvSaldos.RefreshData();
                this.cmbRomp1.ItemIndex = 0;
                this.cmbRomp2.ItemIndex = 3;
                this.romp1 = RompimientoSaldos.Cuenta;
                this.romp2 = RompimientoSaldos.CentroCosto;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {            
                if (this.masterLibro.ValidID)
                {
                    this._filtro = new DTO_coCierreMes();
                    this._filtro.PeriodoID.Value = this.dtPeriod.DateTime;
                    this._filtro.Ano.Value = (short)this.dtPeriod.DateTime.Year;
                    this._filtro.BalanceTipoID.Value = this.masterLibro.Value;
                    this._filtro.CuentaID.Value = this.masterCuenta.Value;
                    this._filtro.ProyectoID.Value = this.masterProyecto.Value;
                    this._filtro.CentroCostoID.Value = this.masterCentroCosto.Value;
                    this._filtro.LineaPresupuestoID.Value = this.masterLineaPresup.Value;
                    this._filtro.ConceptoCargoID.Value = this.masterConceptoCargo.Value;
                    this._filtro.TerceroID.Value = this.masterTercero.Value;
                    this._filtro.Rompimiento1.Value = false;
                    this._filtro.Rompimiento2.Value = this.cmbRomp2.EditValue != "0" ? true : false;

                    List<DTO_coCierreMes> listTmp = this._bc.AdministrationModel.coCierreMes_GetByParameter(this._filtro,romp1,romp2);

                    if (listTmp.Count > 0)
                        this.gvDocument.FocusedRowHandle = 0;

                    this._cierresQuery = listTmp.OrderBy(x=>x.CuentaID.Value).ToList();
                    this.gcDocument.DataSource = this._cierresQuery;
                    this.gcSaldos.RefreshDataSource();
                }
                else
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterLibro.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterLibro.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySaldoCierre.cs", "TBSearch"));
            }
        }

        #endregion Eventos Barra Herramientas
   
    }
}
