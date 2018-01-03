

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
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryInformeMensualOC : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private List<DTO_QueryInformeMensualCierre> _cierresQuery;
        private List<DTO_QueryInformeMensualCierre> _cierresSaldos;
        private DTO_QueryInformeMensualCierre _filtro = null;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private string _unboundPrefix = "Unbound_";
        private int _documentID;

        private string _monedaLocal = string.Empty;
        private string _monedaExtr = string.Empty;

        #endregion

        public QueryInformeMensualOC()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();
                this._frmName = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                // Trae la fuente de datos y los filtra
                this.AddGridCols();
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "QueryInformeMensualOC"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryInformeMensualOC;
            this._frmModule = ModulesPrefix.oc;
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtr = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                //Carga controles de maestras
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, false, true, true, false);
                this._bc.InitMasterUC(this.masterContrato, AppMasters.pyContrato, true, true, true, false);
                this._bc.InitMasterUC(this.masterBloque, AppMasters.ocBloque, true, true, true, false); 
                this._bc.InitMasterUC(this.masterCampo, AppMasters.glAreaFisica, false, true, true, false);
                this._bc.InitMasterUC(this.masterPozo, AppMasters.glLocFisica, false, true, true, false);
                this._bc.InitMasterUC(this.masterRecurso, AppMasters.plRecurso, true, true, true, false);
                this._bc.InitMasterUC(this.masterGrupo, AppMasters.coActividad, false, true, true, false);
                this._bc.InitMasterUC(this.masterSocio, AppMasters.ocSocio, true, true, true, false);

                // Carga combos
                Dictionary<string, string> dicTipoInforme = new Dictionary<string, string>();
                dicTipoInforme.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoInforme_Ejecucion));
                dicTipoInforme.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TipoInforme_Presupuesto));
                this.cmbTipoInforme.Properties.DataSource = dicTipoInforme;
                this.cmbTipoInforme.EditValue = 1;

                Dictionary<string, string> dicTipoProyecto = new Dictionary<string, string>();
                dicTipoProyecto.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Capex));
                dicTipoProyecto.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Opex));
                dicTipoProyecto.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inversion));
                dicTipoProyecto.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Administrativo));
                dicTipoProyecto.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inventarios));
                dicTipoProyecto.Add("7", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Comercial));
                this.cmbTipoProyecto.Properties.DataSource = dicTipoProyecto;
                this.cmbTipoProyecto.EditValue = 2;

                Dictionary<string, string> dicTipoMoneda = new Dictionary<string, string>();
                dicTipoMoneda.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal));
                dicTipoMoneda.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign));
                this.cmbTipoMda.Properties.DataSource = dicTipoMoneda;
                this.cmbTipoMda.EditValue = 1;

                Dictionary<string, string> dicMdaOrigen = new Dictionary<string, string>();
                dicMdaOrigen.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal));
                dicMdaOrigen.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign));
                dicMdaOrigen.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Consolidated));
                this.cmbOrigenMda.Properties.DataSource = dicMdaOrigen;
                this.cmbOrigenMda.EditValue = 1;

                //Obtiene valores iniciales
                string periodoActual = this._bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_Periodo);
                bool multimoneda = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda).Equals("0") ? false : true;
                DateTime? ultimoCierre = this._bc.AdministrationModel.GetUltimoMesCerrado(ModulesPrefix.co);
                this.dtPeriod.DateTime = ultimoCierre != null ? Convert.ToDateTime(ultimoCierre) : Convert.ToDateTime(periodoActual);
                if (!multimoneda)
                {
                    this.cmbTipoMda.EditValue = "1"; //Local
                    this.cmbTipoMda.Properties.ReadOnly = true;
                    this.cmbOrigenMda.EditValue = "1"; //Local
                    this.cmbOrigenMda.Properties.ReadOnly = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columnas Detalle Nivel Principal

                #region Columnas visibles

                //SocioID
                GridColumn SocioID = new GridColumn();
                SocioID.FieldName = this._unboundPrefix + "SocioID";
                SocioID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SocioID");
                SocioID.UnboundType = UnboundColumnType.String;
                SocioID.VisibleIndex = 0;
                SocioID.Width = 70;
                SocioID.Visible = true;
                SocioID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SocioID);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 1;
                Descriptivo.Width = 125;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                //SaldoInicial
                GridColumn SaldoInicial = new GridColumn();
                SaldoInicial.FieldName = this._unboundPrefix + "SaldoInicial";
                SaldoInicial.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoInicial");
                SaldoInicial.UnboundType = UnboundColumnType.Decimal;
                SaldoInicial.VisibleIndex = 2;
                SaldoInicial.Width = 70;
                SaldoInicial.Visible = true;
                SaldoInicial.ColumnEdit = this.TextEdit;
                SaldoInicial.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoInicial);

                //Enero
                GridColumn SaldoEnero = new GridColumn();
                SaldoEnero.FieldName = this._unboundPrefix + "SaldoEnero";
                SaldoEnero.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoEnero");
                SaldoEnero.UnboundType = UnboundColumnType.Decimal;
                SaldoEnero.VisibleIndex = 3;
                SaldoEnero.Width = 70;
                SaldoEnero.Visible = true;
                SaldoEnero.ColumnEdit = this.TextEdit;
                SaldoEnero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoEnero);

                //Febrero
                GridColumn SaldoFebrero = new GridColumn();
                SaldoFebrero.FieldName = this._unboundPrefix + "SaldoFebrero";
                SaldoFebrero.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFebrero");
                SaldoFebrero.UnboundType = UnboundColumnType.Decimal;
                SaldoFebrero.VisibleIndex = 4;
                SaldoFebrero.Width = 70;
                SaldoFebrero.Visible = true;
                SaldoFebrero.ColumnEdit = this.TextEdit;
                SaldoFebrero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoFebrero);

                //Marzo
                GridColumn SaldoMarzo = new GridColumn();
                SaldoMarzo.FieldName = this._unboundPrefix + "SaldoMarzo";
                SaldoMarzo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMarzo");
                SaldoMarzo.UnboundType = UnboundColumnType.Decimal;
                SaldoMarzo.VisibleIndex = 5;
                SaldoMarzo.Width = 70;
                SaldoMarzo.Visible = true;
                SaldoMarzo.ColumnEdit = this.TextEdit;
                SaldoMarzo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoMarzo);

                //Abril
                GridColumn SaldoAbril = new GridColumn();
                SaldoAbril.FieldName = this._unboundPrefix + "SaldoAbril";
                SaldoAbril.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAbril");
                SaldoAbril.UnboundType = UnboundColumnType.Decimal;
                SaldoAbril.VisibleIndex = 6;
                SaldoAbril.Width = 70;
                SaldoAbril.Visible = true;
                SaldoAbril.ColumnEdit = this.TextEdit;
                SaldoAbril.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoAbril);

                //Mayo
                GridColumn SaldoMayo = new GridColumn();
                SaldoMayo.FieldName = this._unboundPrefix + "SaldoMayo";
                SaldoMayo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMayo");
                SaldoMayo.UnboundType = UnboundColumnType.Decimal;
                SaldoMayo.VisibleIndex = 7;
                SaldoMayo.Width = 70;
                SaldoMayo.Visible = true;
                SaldoMayo.ColumnEdit = this.TextEdit;
                SaldoMayo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoMayo);

                //Junio
                GridColumn SaldoJunio = new GridColumn();
                SaldoJunio.FieldName = this._unboundPrefix + "SaldoJunio";
                SaldoJunio.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJunio");
                SaldoJunio.UnboundType = UnboundColumnType.Decimal;
                SaldoJunio.VisibleIndex = 8;
                SaldoJunio.Width = 70;
                SaldoJunio.Visible = true;
                SaldoJunio.ColumnEdit = this.TextEdit;
                SaldoJunio.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoJunio);

                //Julio
                GridColumn SaldoJulio = new GridColumn();
                SaldoJulio.FieldName = this._unboundPrefix + "SaldoJulio";
                SaldoJulio.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJulio");
                SaldoJulio.UnboundType = UnboundColumnType.Decimal;
                SaldoJulio.VisibleIndex = 9;
                SaldoJulio.Width = 70;
                SaldoJulio.Visible = true;
                SaldoJulio.ColumnEdit = this.TextEdit;
                SaldoJulio.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoJulio);

                //Agosto
                GridColumn SaldoAgosto = new GridColumn();
                SaldoAgosto.FieldName = this._unboundPrefix + "SaldoAgosto";
                SaldoAgosto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAgosto");
                SaldoAgosto.UnboundType = UnboundColumnType.Decimal;
                SaldoAgosto.VisibleIndex = 10;
                SaldoAgosto.Width = 70;
                SaldoAgosto.Visible = true;
                SaldoAgosto.ColumnEdit = this.TextEdit;
                SaldoAgosto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoAgosto);

                //Septiembre
                GridColumn SaldoSeptiembre = new GridColumn();
                SaldoSeptiembre.FieldName = this._unboundPrefix + "SaldoSeptiembre";
                SaldoSeptiembre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoSeptiembre");
                SaldoSeptiembre.UnboundType = UnboundColumnType.Decimal;
                SaldoSeptiembre.VisibleIndex = 11;
                SaldoSeptiembre.Width = 70;
                SaldoSeptiembre.Visible = true;
                SaldoSeptiembre.ColumnEdit = this.TextEdit;
                SaldoSeptiembre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoSeptiembre);

                //Octubre
                GridColumn SaldoOctubre = new GridColumn();
                SaldoOctubre.FieldName = this._unboundPrefix + "SaldoOctubre";
                SaldoOctubre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoOctubre");
                SaldoOctubre.UnboundType = UnboundColumnType.Decimal;
                SaldoOctubre.VisibleIndex = 12;
                SaldoOctubre.Width = 70;
                SaldoOctubre.Visible = true;
                SaldoOctubre.ColumnEdit = this.TextEdit;
                SaldoOctubre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoOctubre);

                //Noviembre
                GridColumn SaldoNoviembre = new GridColumn();
                SaldoNoviembre.FieldName = this._unboundPrefix + "SaldoNoviembre";
                SaldoNoviembre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoNoviembre");
                SaldoNoviembre.UnboundType = UnboundColumnType.Decimal;
                SaldoNoviembre.VisibleIndex = 13;
                SaldoNoviembre.Width = 70;
                SaldoNoviembre.Visible = true;
                SaldoNoviembre.ColumnEdit = this.TextEdit;
                SaldoNoviembre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoNoviembre);

                //Diciembre
                GridColumn SaldoDiciembre = new GridColumn();
                SaldoDiciembre.FieldName = this._unboundPrefix + "SaldoDiciembre";
                SaldoDiciembre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoDiciembre");
                SaldoDiciembre.UnboundType = UnboundColumnType.Decimal;
                SaldoDiciembre.VisibleIndex = 14;
                SaldoDiciembre.Width = 70;
                SaldoDiciembre.Visible = true;
                SaldoDiciembre.ColumnEdit = this.TextEdit;
                SaldoDiciembre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoDiciembre);

                //SaldoFinal
                GridColumn SaldoFinal = new GridColumn();
                SaldoFinal.FieldName = this._unboundPrefix + "SaldoFinal";
                SaldoFinal.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFinal");
                SaldoFinal.UnboundType = UnboundColumnType.Decimal;
                SaldoFinal.VisibleIndex = 15;
                SaldoFinal.Width = 90;
                SaldoFinal.Visible = true;
                SaldoFinal.ColumnEdit = this.TextEdit;
                SaldoFinal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoFinal);


                #endregion
                #region Columnas no Visibles

                //MOnedaID
                GridColumn MonedaID = new GridColumn();
                MonedaID.FieldName = this._unboundPrefix + "MonedaID";
                MonedaID.UnboundType = UnboundColumnType.String;
                MonedaID.Visible = false;
                MonedaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaID);

                //ContratoID
                GridColumn ContratoID = new GridColumn();
                ContratoID.FieldName = this._unboundPrefix + "ContratoID";
                ContratoID.UnboundType = UnboundColumnType.String;
                ContratoID.Visible = false;
                ContratoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ContratoID);

                //BloqueID
                GridColumn BloqueID = new GridColumn();
                BloqueID.FieldName = this._unboundPrefix + "BloqueID";
                BloqueID.UnboundType = UnboundColumnType.String;
                BloqueID.Visible = false;
                BloqueID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(BloqueID);

                //Campo(AreaFisicaID)
                GridColumn Campo = new GridColumn();
                Campo.FieldName = this._unboundPrefix + "Campo";
                Campo.UnboundType = UnboundColumnType.String;
                Campo.Visible = false;
                Campo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Campo);

                //Pozo(LocFisica)
                GridColumn Pozo = new GridColumn();
                Pozo.FieldName = this._unboundPrefix + "Pozo";
                Pozo.UnboundType = UnboundColumnType.String;
                Pozo.Visible = false;
                Pozo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Pozo);

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.Visible = false;
                this.gvDocument.Columns.Add(CentroCostoID);

                //LineaPresupuesto
                GridColumn LineaPresupuesto = new GridColumn();
                LineaPresupuesto.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuesto.UnboundType = UnboundColumnType.String;
                LineaPresupuesto.Visible = false;
                this.gvDocument.Columns.Add(LineaPresupuesto);
                #endregion

                #endregion

                #region Columnas Detalle Nivel 1

                #region Columnas visibles

                //RecursoGrupoID(ActividadID)
                GridColumn GrupoDeta1 = new GridColumn();
                GrupoDeta1.FieldName = this._unboundPrefix + "Grupo";
                GrupoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Grupo");
                GrupoDeta1.UnboundType = UnboundColumnType.String;
                GrupoDeta1.VisibleIndex = 0;
                GrupoDeta1.Width = 70;
                GrupoDeta1.Visible = true;
                GrupoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(GrupoDeta1);

                //ActividadID
                GridColumn ActividadIDeta1 = new GridColumn();
                ActividadIDeta1.FieldName = this._unboundPrefix + "ActividadID";
                ActividadIDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadID");
                ActividadIDeta1.UnboundType = UnboundColumnType.String;
                ActividadIDeta1.VisibleIndex = 0;
                ActividadIDeta1.Width = 70;
                ActividadIDeta1.Visible = false;
                ActividadIDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(ActividadIDeta1);

                //ProyectoIDDeta1(AFE)
                GridColumn ProyectoIDDeta1 = new GridColumn();
                ProyectoIDDeta1.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoIDDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoIDDeta1.UnboundType = UnboundColumnType.String;
                ProyectoIDDeta1.VisibleIndex = 0;
                ProyectoIDDeta1.Width = 70;
                ProyectoIDDeta1.Visible = false;
                ProyectoIDDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(ProyectoIDDeta1);

                //Descriptivo
                GridColumn DescriptivoDeta1 = new GridColumn();
                DescriptivoDeta1.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta1.UnboundType = UnboundColumnType.String;
                DescriptivoDeta1.VisibleIndex = 1;
                DescriptivoDeta1.Width = 125;
                DescriptivoDeta1.Visible = true;
                DescriptivoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(DescriptivoDeta1);

                //SaldoInicial
                GridColumn SaldoInicialDeta1 = new GridColumn();
                SaldoInicialDeta1.FieldName = this._unboundPrefix + "SaldoInicial";
                SaldoInicialDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoInicial");
                SaldoInicialDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoInicialDeta1.VisibleIndex = 2;
                SaldoInicialDeta1.Width = 70;
                SaldoInicialDeta1.Visible = true;
                SaldoInicialDeta1.ColumnEdit = this.TextEdit;
                SaldoInicialDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoInicialDeta1);

                //Enero
                GridColumn SaldoEneroDeta1 = new GridColumn();
                SaldoEneroDeta1.FieldName = this._unboundPrefix + "SaldoEnero";
                SaldoEneroDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoEnero");
                SaldoEneroDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoEneroDeta1.VisibleIndex = 3;
                SaldoEneroDeta1.Width = 70;
                SaldoEneroDeta1.Visible = true;
                SaldoEneroDeta1.ColumnEdit = this.TextEdit;
                SaldoEneroDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoEneroDeta1);

                //Febrero
                GridColumn SaldoFebreroDeta1 = new GridColumn();
                SaldoFebreroDeta1.FieldName = this._unboundPrefix + "SaldoFebrero";
                SaldoFebreroDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFebrero");
                SaldoFebreroDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoFebreroDeta1.VisibleIndex = 4;
                SaldoFebreroDeta1.Width = 70;
                SaldoFebreroDeta1.Visible = true;
                SaldoFebreroDeta1.ColumnEdit = this.TextEdit;
                SaldoFebreroDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoFebreroDeta1);

                //Marzo
                GridColumn SaldoMarzoDeta1 = new GridColumn();
                SaldoMarzoDeta1.FieldName = this._unboundPrefix + "SaldoMarzo";
                SaldoMarzoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMarzo");
                SaldoMarzoDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoMarzoDeta1.VisibleIndex = 5;
                SaldoMarzoDeta1.Width = 70;
                SaldoMarzoDeta1.Visible = true;
                SaldoMarzoDeta1.ColumnEdit = this.TextEdit;
                SaldoMarzoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoMarzoDeta1);

                //Abril
                GridColumn SaldoAbrilDeta1 = new GridColumn();
                SaldoAbrilDeta1.FieldName = this._unboundPrefix + "SaldoAbril";
                SaldoAbrilDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAbril");
                SaldoAbrilDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoAbrilDeta1.VisibleIndex = 6;
                SaldoAbrilDeta1.Width = 70;
                SaldoAbrilDeta1.Visible = true;
                SaldoAbrilDeta1.ColumnEdit = this.TextEdit;
                SaldoAbrilDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoAbrilDeta1);

                //Mayo
                GridColumn SaldoMayoDeta1 = new GridColumn();
                SaldoMayoDeta1.FieldName = this._unboundPrefix + "SaldoMayo";
                SaldoMayoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMayo");
                SaldoMayoDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoMayoDeta1.VisibleIndex = 7;
                SaldoMayoDeta1.Width = 70;
                SaldoMayoDeta1.Visible = true;
                SaldoMayoDeta1.ColumnEdit = this.TextEdit;
                SaldoMayoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoMayoDeta1);

                //Junio
                GridColumn SaldoJunioDeta1 = new GridColumn();
                SaldoJunioDeta1.FieldName = this._unboundPrefix + "SaldoJunio";
                SaldoJunioDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJunio");
                SaldoJunioDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoJunioDeta1.VisibleIndex = 8;
                SaldoJunioDeta1.Width = 70;
                SaldoJunioDeta1.Visible = true;
                SaldoJunioDeta1.ColumnEdit = this.TextEdit;
                SaldoJunioDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoJunioDeta1);

                //Julio
                GridColumn SaldoJulioDeta1 = new GridColumn();
                SaldoJulioDeta1.FieldName = this._unboundPrefix + "SaldoJulio";
                SaldoJulioDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJulio");
                SaldoJulioDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoJulioDeta1.VisibleIndex = 9;
                SaldoJulioDeta1.Width = 70;
                SaldoJulioDeta1.Visible = true;
                SaldoJulioDeta1.ColumnEdit = this.TextEdit;
                SaldoJulioDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoJulioDeta1);

                //Agosto
                GridColumn SaldoAgostoDeta1 = new GridColumn();
                SaldoAgostoDeta1.FieldName = this._unboundPrefix + "SaldoAgosto";
                SaldoAgostoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAgosto");
                SaldoAgostoDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoAgostoDeta1.VisibleIndex = 10;
                SaldoAgostoDeta1.Width = 70;
                SaldoAgostoDeta1.Visible = true;
                SaldoAgostoDeta1.ColumnEdit = this.TextEdit;
                SaldoAgostoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoAgostoDeta1);

                //Septiembre
                GridColumn SaldoSeptiembreDeta1 = new GridColumn();
                SaldoSeptiembreDeta1.FieldName = this._unboundPrefix + "SaldoSeptiembre";
                SaldoSeptiembreDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoSeptiembre");
                SaldoSeptiembreDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoSeptiembreDeta1.VisibleIndex = 11;
                SaldoSeptiembreDeta1.Width = 70;
                SaldoSeptiembreDeta1.Visible = true;
                SaldoSeptiembreDeta1.ColumnEdit = this.TextEdit;
                SaldoSeptiembreDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoSeptiembreDeta1);

                //Octubre
                GridColumn SaldoOctubreDeta1 = new GridColumn();
                SaldoOctubreDeta1.FieldName = this._unboundPrefix + "SaldoOctubre";
                SaldoOctubreDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoOctubre");
                SaldoOctubreDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoOctubreDeta1.VisibleIndex = 12;
                SaldoOctubreDeta1.Width = 70;
                SaldoOctubreDeta1.Visible = true;
                SaldoOctubreDeta1.ColumnEdit = this.TextEdit;
                SaldoOctubreDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoOctubreDeta1);

                //Noviembre
                GridColumn SaldoNoviembreDeta1 = new GridColumn();
                SaldoNoviembreDeta1.FieldName = this._unboundPrefix + "SaldoNoviembre";
                SaldoNoviembreDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoNoviembre");
                SaldoNoviembreDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoNoviembreDeta1.VisibleIndex = 13;
                SaldoNoviembreDeta1.Width = 70;
                SaldoNoviembreDeta1.Visible = true;
                SaldoNoviembreDeta1.ColumnEdit = this.TextEdit;
                SaldoNoviembreDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoNoviembreDeta1);

                //Diciembre
                GridColumn SaldoDiciembreDeta1 = new GridColumn();
                SaldoDiciembreDeta1.FieldName = this._unboundPrefix + "SaldoDiciembre";
                SaldoDiciembreDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoDiciembre");
                SaldoDiciembreDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoDiciembreDeta1.VisibleIndex = 14;
                SaldoDiciembreDeta1.Width = 70;
                SaldoDiciembreDeta1.Visible = true;
                SaldoDiciembreDeta1.ColumnEdit = this.TextEdit;
                SaldoDiciembreDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoDiciembreDeta1);

                //SaldoFinal
                GridColumn SaldoFinalDeta1 = new GridColumn();
                SaldoFinalDeta1.FieldName = this._unboundPrefix + "SaldoFinal";
                SaldoFinalDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFinal");
                SaldoFinalDeta1.UnboundType = UnboundColumnType.Decimal;
                SaldoFinalDeta1.VisibleIndex = 15;
                SaldoFinalDeta1.Width = 90;
                SaldoFinalDeta1.Visible = true;
                SaldoFinalDeta1.ColumnEdit = this.TextEdit;
                SaldoFinalDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(SaldoFinalDeta1);


                #endregion
                #region Columnas no Visibles

                //ContratoID
                GridColumn ContratoIDDeta1 = new GridColumn();
                ContratoIDDeta1.FieldName = this._unboundPrefix + "ContratoID";
                ContratoIDDeta1.UnboundType = UnboundColumnType.String;
                ContratoIDDeta1.Visible = false;
                ContratoIDDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(ContratoIDDeta1);

                //BloqueID
                GridColumn BloqueIDDeta1 = new GridColumn();
                BloqueIDDeta1.FieldName = this._unboundPrefix + "BloqueID";
                BloqueIDDeta1.UnboundType = UnboundColumnType.String;
                BloqueIDDeta1.Visible = false;
                BloqueIDDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(BloqueIDDeta1);

                //Campo(AreaFisicaID)
                GridColumn CampoDeta1 = new GridColumn();
                CampoDeta1.FieldName = this._unboundPrefix + "Campo";
                CampoDeta1.UnboundType = UnboundColumnType.String;
                CampoDeta1.Visible = false;
                CampoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(CampoDeta1);

                //Pozo(LocFisica)
                GridColumn PozoDeta1 = new GridColumn();
                PozoDeta1.FieldName = this._unboundPrefix + "Pozo";
                PozoDeta1.UnboundType = UnboundColumnType.String;
                PozoDeta1.Visible = false;
                PozoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(PozoDeta1);

                //CentroCostoID
                GridColumn CentroCostoIDDeta1 = new GridColumn();
                CentroCostoIDDeta1.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoIDDeta1.UnboundType = UnboundColumnType.String;
                CentroCostoIDDeta1.Visible = false;
                this.gvDetalleNivel1.Columns.Add(CentroCostoIDDeta1);

                //LineaPresupuesto
                GridColumn LineaPresupuestoDeta1 = new GridColumn();
                LineaPresupuestoDeta1.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuestoDeta1.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDeta1.Visible = false;
                this.gvDetalleNivel1.Columns.Add(LineaPresupuestoDeta1);
                #endregion

                #endregion

                #region Columnas Detalle Nivel 2
                #region Columnas visibles

                //RecursoID
                GridColumn RecursoIDDeta2 = new GridColumn();
                RecursoIDDeta2.FieldName = this._unboundPrefix + "RecursoID";
                RecursoIDDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_RecursoID");
                RecursoIDDeta2.UnboundType = UnboundColumnType.String;
                RecursoIDDeta2.VisibleIndex = 0;
                RecursoIDDeta2.Width = 70;
                RecursoIDDeta2.Visible = true;
                RecursoIDDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(RecursoIDDeta2);

                //CentroCostoID
                GridColumn CentroCostoIDeta2 = new GridColumn();
                CentroCostoIDeta2.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoIDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CentroCostoID");
                CentroCostoIDeta2.UnboundType = UnboundColumnType.String;
                CentroCostoIDeta2.VisibleIndex = 0;
                CentroCostoIDeta2.Width = 70;
                CentroCostoIDeta2.Visible = false;
                CentroCostoIDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(CentroCostoIDeta2);

                //DescriptivoDeta2
                GridColumn DescriptivoDeta2 = new GridColumn();
                DescriptivoDeta2.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta2.UnboundType = UnboundColumnType.String;
                DescriptivoDeta2.VisibleIndex = 1;
                DescriptivoDeta2.Width = 120;
                DescriptivoDeta2.Visible = true;
                DescriptivoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(DescriptivoDeta2);

                //SaldoInicialDeta2
                GridColumn SaldoInicialDeta2 = new GridColumn();
                SaldoInicialDeta2.FieldName = this._unboundPrefix + "SaldoInicial";
                SaldoInicialDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoInicial");
                SaldoInicialDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoInicialDeta2.VisibleIndex = 2;
                SaldoInicialDeta2.Width = 70;
                SaldoInicialDeta2.Visible = true;
                SaldoInicialDeta2.ColumnEdit = this.TextEdit;
                SaldoInicialDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoInicialDeta2);

                //EneroDeta2
                GridColumn EneroDeta2 = new GridColumn();
                EneroDeta2.FieldName = this._unboundPrefix + "SaldoEnero";
                EneroDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoEnero");
                EneroDeta2.UnboundType = UnboundColumnType.Decimal;
                EneroDeta2.VisibleIndex = 3;
                EneroDeta2.Width = 70;
                EneroDeta2.Visible = true;
                EneroDeta2.ColumnEdit = this.TextEdit;
                EneroDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(EneroDeta2);

                //Febrero
                GridColumn SaldoFebreroDeta2 = new GridColumn();
                SaldoFebreroDeta2.FieldName = this._unboundPrefix + "SaldoFebrero";
                SaldoFebreroDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFebrero");
                SaldoFebreroDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoFebreroDeta2.VisibleIndex = 4;
                SaldoFebreroDeta2.Width = 70;
                SaldoFebreroDeta2.Visible = true;
                SaldoFebreroDeta2.ColumnEdit = this.TextEdit;
                SaldoFebreroDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoFebreroDeta2);

                //Marzo
                GridColumn SaldoMarzoDeta2 = new GridColumn();
                SaldoMarzoDeta2.FieldName = this._unboundPrefix + "SaldoMarzo";
                SaldoMarzoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMarzo");
                SaldoMarzoDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoMarzoDeta2.VisibleIndex = 5;
                SaldoMarzoDeta2.Width = 70;
                SaldoMarzoDeta2.Visible = true;
                SaldoMarzoDeta2.ColumnEdit = this.TextEdit;
                SaldoMarzoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoMarzoDeta2);

                //Abril
                GridColumn SaldoAbrilDeta2 = new GridColumn();
                SaldoAbrilDeta2.FieldName = this._unboundPrefix + "SaldoAbril";
                SaldoAbrilDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAbril");
                SaldoAbrilDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoAbrilDeta2.VisibleIndex = 6;
                SaldoAbrilDeta2.Width = 70;
                SaldoAbrilDeta2.Visible = true;
                SaldoAbrilDeta2.ColumnEdit = this.TextEdit;
                SaldoAbrilDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoAbrilDeta2);

                //Mayo
                GridColumn SaldoMayoDeta2 = new GridColumn();
                SaldoMayoDeta2.FieldName = this._unboundPrefix + "SaldoMayo";
                SaldoMayoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMayo");
                SaldoMayoDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoMayoDeta2.VisibleIndex = 7;
                SaldoMayoDeta2.Width = 70;
                SaldoMayoDeta2.Visible = true;
                SaldoMayoDeta2.ColumnEdit = this.TextEdit;
                SaldoMayoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoMayoDeta2);

                //Junio
                GridColumn SaldoJunioDeta2 = new GridColumn();
                SaldoJunioDeta2.FieldName = this._unboundPrefix + "SaldoJunio";
                SaldoJunioDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJunio");
                SaldoJunioDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoJunioDeta2.VisibleIndex = 8;
                SaldoJunioDeta2.Width = 70;
                SaldoJunioDeta2.Visible = true;
                SaldoJunioDeta2.ColumnEdit = this.TextEdit;
                SaldoJunioDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoJunioDeta2);

                //Julio
                GridColumn SaldoJulioDeta2 = new GridColumn();
                SaldoJulioDeta2.FieldName = this._unboundPrefix + "SaldoJulio";
                SaldoJulioDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJulio");
                SaldoJulioDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoJulioDeta2.VisibleIndex = 9;
                SaldoJulioDeta2.Width = 70;
                SaldoJulioDeta2.Visible = true;
                SaldoJulioDeta2.ColumnEdit = this.TextEdit;
                SaldoJulioDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoJulioDeta2);

                //Agosto
                GridColumn SaldoAgostoDeta2 = new GridColumn();
                SaldoAgostoDeta2.FieldName = this._unboundPrefix + "SaldoAgosto";
                SaldoAgostoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAgosto");
                SaldoAgostoDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoAgostoDeta2.VisibleIndex = 10;
                SaldoAgostoDeta2.Width = 70;
                SaldoAgostoDeta2.Visible = true;
                SaldoAgostoDeta2.ColumnEdit = this.TextEdit;
                SaldoAgostoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoAgostoDeta2);

                //Septiembre
                GridColumn SaldoSeptiembreDeta2 = new GridColumn();
                SaldoSeptiembreDeta2.FieldName = this._unboundPrefix + "SaldoSeptiembre";
                SaldoSeptiembreDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoSeptiembre");
                SaldoSeptiembreDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoSeptiembreDeta2.VisibleIndex = 11;
                SaldoSeptiembreDeta2.Width = 70;
                SaldoSeptiembreDeta2.Visible = true;
                SaldoSeptiembreDeta2.ColumnEdit = this.TextEdit;
                SaldoSeptiembreDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoSeptiembreDeta2);

                //Octubre
                GridColumn SaldoOctubreDeta2 = new GridColumn();
                SaldoOctubreDeta2.FieldName = this._unboundPrefix + "SaldoOctubre";
                SaldoOctubreDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoOctubre");
                SaldoOctubreDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoOctubreDeta2.VisibleIndex = 12;
                SaldoOctubreDeta2.Width = 70;
                SaldoOctubreDeta2.Visible = true;
                SaldoOctubreDeta2.ColumnEdit = this.TextEdit;
                SaldoOctubreDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoOctubreDeta2);

                //Noviembre
                GridColumn SaldoNoviembreDeta2 = new GridColumn();
                SaldoNoviembreDeta2.FieldName = this._unboundPrefix + "SaldoNoviembre";
                SaldoNoviembreDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoNoviembre");
                SaldoNoviembreDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoNoviembreDeta2.VisibleIndex = 13;
                SaldoNoviembreDeta2.Width = 70;
                SaldoNoviembreDeta2.Visible = true;
                SaldoNoviembreDeta2.ColumnEdit = this.TextEdit;
                SaldoNoviembreDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoNoviembreDeta2);

                //Diciembre
                GridColumn SaldoDiciembreDeta2 = new GridColumn();
                SaldoDiciembreDeta2.FieldName = this._unboundPrefix + "SaldoDiciembre";
                SaldoDiciembreDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoDiciembre");
                SaldoDiciembreDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoDiciembreDeta2.VisibleIndex = 14;
                SaldoDiciembreDeta2.Width = 70;
                SaldoDiciembreDeta2.Visible = true;
                SaldoDiciembreDeta2.ColumnEdit = this.TextEdit;
                SaldoDiciembreDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoDiciembreDeta2);

                //SaldoFinal
                GridColumn SaldoFinalDeta2 = new GridColumn();
                SaldoFinalDeta2.FieldName = this._unboundPrefix + "SaldoFinal";
                SaldoFinalDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFinal");
                SaldoFinalDeta2.UnboundType = UnboundColumnType.Decimal;
                SaldoFinalDeta2.VisibleIndex = 15;
                SaldoFinalDeta2.Width = 90;
                SaldoFinalDeta2.Visible = true;
                SaldoFinalDeta2.ColumnEdit = this.TextEdit;
                SaldoFinalDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(SaldoFinalDeta2);
                #endregion
                #endregion

                #region Columnas Detalle Nivel 3
                #region Columnas visibles

                //LineaPresupuesto
                GridColumn LineaPresupuestoDeta3 = new GridColumn();
                LineaPresupuestoDeta3.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuestoDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaPresupuestoID");
                LineaPresupuestoDeta3.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDeta3.VisibleIndex = 0;
                LineaPresupuestoDeta3.Width = 70;
                LineaPresupuestoDeta3.Visible = true;
                LineaPresupuestoDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(LineaPresupuestoDeta3);

                //DescriptivoDeta3
                GridColumn DescriptivoDeta3 = new GridColumn();
                DescriptivoDeta3.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta3.UnboundType = UnboundColumnType.String;
                DescriptivoDeta3.VisibleIndex = 1;
                DescriptivoDeta3.Width = 120;
                DescriptivoDeta3.Visible = true;
                DescriptivoDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(DescriptivoDeta3);

                //SaldoInicialDeta3
                GridColumn SaldoInicialDeta3 = new GridColumn();
                SaldoInicialDeta3.FieldName = this._unboundPrefix + "SaldoInicial";
                SaldoInicialDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoInicial");
                SaldoInicialDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoInicialDeta3.VisibleIndex = 2;
                SaldoInicialDeta3.Width = 70;
                SaldoInicialDeta3.Visible = true;
                SaldoInicialDeta3.ColumnEdit = this.TextEdit;
                SaldoInicialDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoInicialDeta3);

                //EneroDeta3
                GridColumn EneroDeta3 = new GridColumn();
                EneroDeta3.FieldName = this._unboundPrefix + "SaldoEnero";
                EneroDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoEnero");
                EneroDeta3.UnboundType = UnboundColumnType.Decimal;
                EneroDeta3.VisibleIndex = 3;
                EneroDeta3.Width = 70;
                EneroDeta3.Visible = true;
                EneroDeta3.ColumnEdit = this.TextEdit;
                EneroDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(EneroDeta3);

                //Febrero
                GridColumn SaldoFebreroDeta3 = new GridColumn();
                SaldoFebreroDeta3.FieldName = this._unboundPrefix + "SaldoFebrero";
                SaldoFebreroDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFebrero");
                SaldoFebreroDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoFebreroDeta3.VisibleIndex = 4;
                SaldoFebreroDeta3.Width = 70;
                SaldoFebreroDeta3.Visible = true;
                SaldoFebreroDeta3.ColumnEdit = this.TextEdit;
                SaldoFebreroDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoFebreroDeta3);

                //Marzo
                GridColumn SaldoMarzoDeta3 = new GridColumn();
                SaldoMarzoDeta3.FieldName = this._unboundPrefix + "SaldoMarzo";
                SaldoMarzoDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMarzo");
                SaldoMarzoDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoMarzoDeta3.VisibleIndex = 5;
                SaldoMarzoDeta3.Width = 70;
                SaldoMarzoDeta3.Visible = true;
                SaldoMarzoDeta3.ColumnEdit = this.TextEdit;
                SaldoMarzoDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoMarzoDeta3);

                //Abril
                GridColumn SaldoAbrilDeta3 = new GridColumn();
                SaldoAbrilDeta3.FieldName = this._unboundPrefix + "SaldoAbril";
                SaldoAbrilDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAbril");
                SaldoAbrilDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoAbrilDeta3.VisibleIndex = 6;
                SaldoAbrilDeta3.Width = 70;
                SaldoAbrilDeta3.Visible = true;
                SaldoAbrilDeta3.ColumnEdit = this.TextEdit;
                SaldoAbrilDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoAbrilDeta3);

                //Mayo
                GridColumn SaldoMayoDeta3 = new GridColumn();
                SaldoMayoDeta3.FieldName = this._unboundPrefix + "SaldoMayo";
                SaldoMayoDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoMayo");
                SaldoMayoDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoMayoDeta3.VisibleIndex = 7;
                SaldoMayoDeta3.Width = 70;
                SaldoMayoDeta3.Visible = true;
                SaldoMayoDeta3.ColumnEdit = this.TextEdit;
                SaldoMayoDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoMayoDeta3);

                //Junio
                GridColumn SaldoJunioDeta3 = new GridColumn();
                SaldoJunioDeta3.FieldName = this._unboundPrefix + "SaldoJunio";
                SaldoJunioDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJunio");
                SaldoJunioDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoJunioDeta3.VisibleIndex = 8;
                SaldoJunioDeta3.Width = 70;
                SaldoJunioDeta3.Visible = true;
                SaldoJunioDeta3.ColumnEdit = this.TextEdit;
                SaldoJunioDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoJunioDeta3);

                //Julio
                GridColumn SaldoJulioDeta3 = new GridColumn();
                SaldoJulioDeta3.FieldName = this._unboundPrefix + "SaldoJulio";
                SaldoJulioDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoJulio");
                SaldoJulioDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoJulioDeta3.VisibleIndex = 9;
                SaldoJulioDeta3.Width = 70;
                SaldoJulioDeta3.Visible = true;
                SaldoJulioDeta3.ColumnEdit = this.TextEdit;
                SaldoJulioDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoJulioDeta3);

                //Agosto
                GridColumn SaldoAgostoDeta3 = new GridColumn();
                SaldoAgostoDeta3.FieldName = this._unboundPrefix + "SaldoAgosto";
                SaldoAgostoDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoAgosto");
                SaldoAgostoDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoAgostoDeta3.VisibleIndex = 10;
                SaldoAgostoDeta3.Width = 70;
                SaldoAgostoDeta3.Visible = true;
                SaldoAgostoDeta3.ColumnEdit = this.TextEdit;
                SaldoAgostoDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoAgostoDeta3);

                //Septiembre
                GridColumn SaldoSeptiembreDeta3 = new GridColumn();
                SaldoSeptiembreDeta3.FieldName = this._unboundPrefix + "SaldoSeptiembre";
                SaldoSeptiembreDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoSeptiembre");
                SaldoSeptiembreDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoSeptiembreDeta3.VisibleIndex = 11;
                SaldoSeptiembreDeta3.Width = 70;
                SaldoSeptiembreDeta3.Visible = true;
                SaldoSeptiembreDeta3.ColumnEdit = this.TextEdit;
                SaldoSeptiembreDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoSeptiembreDeta3);

                //Octubre
                GridColumn SaldoOctubreDeta3 = new GridColumn();
                SaldoOctubreDeta3.FieldName = this._unboundPrefix + "SaldoOctubre";
                SaldoOctubreDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoOctubre");
                SaldoOctubreDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoOctubreDeta3.VisibleIndex = 12;
                SaldoOctubreDeta3.Width = 70;
                SaldoOctubreDeta3.Visible = true;
                SaldoOctubreDeta3.ColumnEdit = this.TextEdit;
                SaldoOctubreDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoOctubreDeta3);

                //Noviembre
                GridColumn SaldoNoviembreDeta3 = new GridColumn();
                SaldoNoviembreDeta3.FieldName = this._unboundPrefix + "SaldoNoviembre";
                SaldoNoviembreDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoNoviembre");
                SaldoNoviembreDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoNoviembreDeta3.VisibleIndex = 13;
                SaldoNoviembreDeta3.Width = 70;
                SaldoNoviembreDeta3.Visible = true;
                SaldoNoviembreDeta3.ColumnEdit = this.TextEdit;
                SaldoNoviembreDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoNoviembreDeta3);

                //Diciembre
                GridColumn SaldoDiciembreDeta3 = new GridColumn();
                SaldoDiciembreDeta3.FieldName = this._unboundPrefix + "SaldoDiciembre";
                SaldoDiciembreDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoDiciembre");
                SaldoDiciembreDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoDiciembreDeta3.VisibleIndex = 14;
                SaldoDiciembreDeta3.Width = 70;
                SaldoDiciembreDeta3.Visible = true;
                SaldoDiciembreDeta3.ColumnEdit = this.TextEdit;
                SaldoDiciembreDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoDiciembreDeta3);

                //SaldoFinal
                GridColumn SaldoFinalDeta3 = new GridColumn();
                SaldoFinalDeta3.FieldName = this._unboundPrefix + "SaldoFinal";
                SaldoFinalDeta3.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFinal");
                SaldoFinalDeta3.UnboundType = UnboundColumnType.Decimal;
                SaldoFinalDeta3.VisibleIndex = 15;
                SaldoFinalDeta3.Width = 90;
                SaldoFinalDeta3.Visible = true;
                SaldoFinalDeta3.ColumnEdit = this.TextEdit;
                SaldoFinalDeta3.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel3.Columns.Add(SaldoFinalDeta3);
                #endregion
                #endregion

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.gvDetalleNivel1.OptionsView.ColumnAutoWidth = true;
                this.gvDetalleNivel2.OptionsView.ColumnAutoWidth = true;
                this.gvDetalleNivel3.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "QueryInformeMensualOC.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            #region Valida datos en el combo de Proyecto Tipo
            if (string.IsNullOrEmpty(this.cmbTipoProyecto.EditValue.ToString()))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblTipoProyecto.Text);
                MessageBox.Show(msg);
                this.cmbTipoProyecto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en el combo de Moneda Origen
            if (string.IsNullOrEmpty(this.cmbTipoMda.EditValue.ToString()))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblTipoMda.Text);
                MessageBox.Show(msg);
                this.cmbTipoMda.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de MonedaPago
            if ((Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Opex && !this.masterContrato.ValidID ||
                 Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex && !this.masterContrato.ValidID) && !this.masterContrato.ValidID)
            {
                //string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterContrato.CodeRsx);

                //MessageBox.Show(msg);
                //this.masterContrato.Focus();

                //return false;
            }
            #endregion

            return true;
        }

        //Habilita o deshabilita el header
        private void EnabledHeader(bool enable)
        {
            //this.panel1.Enabled = enable;
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
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Get);
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "Form_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "Form_Leave: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "Form_Closing: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                    else
                    {
                        DTO_QueryInformeMensualCierre dtoDet = (DTO_QueryInformeMensualCierre)e.Row;
                        pi = dtoDet.DetalleNivel1.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                e.Value = pi.GetValue(dtoDet.DetalleNivel1, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet.DetalleNivel1, null), null);
                        }
                        else
                        {
                            fi = dtoDet.DetalleNivel1.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                    e.Value = fi.GetValue(dtoDet.DetalleNivel1);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet.DetalleNivel1), null);
                            }
                        }
                    }
                }
            }

            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        pi.SetValue(dto, e.Value, null);
                        //e.Value = pi.GetValue(dto, null);
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
                            pi.SetValue(dto, e.Value, null);
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        DTO_QueryInformeMensualCierre dtoDet = (DTO_QueryInformeMensualCierre)e.Row;
                        pi = dtoDet.DetalleNivel1.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                e.Value = pi.GetValue(dtoDet.DetalleNivel1, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)pi.GetValue(dtoDet.DetalleNivel1, null);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            fi = dtoDet.DetalleNivel1.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                {
                                    pi.SetValue(dto, e.Value, null);
                                    //e.Value = pi.GetValue(dto, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)fi.GetValue(dtoDet.DetalleNivel1);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Valida el tipo de proyecto al seleccionarlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbProyectoTipo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Opex)
                {
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ActividadID"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "Grupo"].Visible = false;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;

                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "RecursoID"].Visible = true;
                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                }
                else if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex ||
                         Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                {
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "Grupo"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ActividadID"].Visible = false;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ProyectoID"].Visible = false;

                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "RecursoID"].Visible = true;
                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                }
                else
                {
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ProyectoID"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "ActividadID"].Visible = false;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "Grupo"].Visible = false;

                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "CentroCostoID"].Visible = true;
                    this.gvDetalleNivel2.Columns[this._unboundPrefix + "RecursoID"].Visible = false;
                }
                //this.gvDocument.RefreshData();
                //this.gvDetalle.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "cmbProyectoTipo_EditValueChanged"));
            }
        }

        /// <summary>
        /// Valida el tipo de moneda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbMdOrigen_EditValueChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "cmbMdOrigen_EditValueChanged"));
            }
        }

        /// <summary>
        ///Al salir del control se ejecuta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            if (master.DocId == AppMasters.coProyecto && master.ValidID)
            {
                DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, master.Value, true);
                DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                this.masterPozo.Value = proy.LocFisicaID.Value;
                if (locFisica != null)
                    this.masterCampo.Value = locFisica.AreaFisica.Value;
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
                this._cierresQuery = new List<DTO_QueryInformeMensualCierre>();

                this.gcDocument.DataSource = this._cierresQuery;
                this.gvDocument.RefreshData();
                //this.panel1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (this.ValidateHeader())
                {
                    DTO_QueryInformeMensualCierre dtoFilterCierre = new DTO_QueryInformeMensualCierre();
                    dtoFilterCierre.PeriodoID.Value = this.dtPeriod.DateTime;
                    dtoFilterCierre.ProyectoID.Value = this.masterProyecto.Value;
                    dtoFilterCierre.RecursoID.Value = this.masterRecurso.Value;
                    dtoFilterCierre.ActividadID.Value = this.masterGrupo.Value;
                    dtoFilterCierre.Campo.Value = this.masterCampo.Value;
                    dtoFilterCierre.ContratoID.Value = this.masterContrato.Value;
                    dtoFilterCierre.Grupo.Value = this.masterGrupo.Value;
                    dtoFilterCierre.Pozo.Value = this.masterPozo.Value;
                    dtoFilterCierre.SocioID.Value = this.masterSocio.Value;
                    TipoMoneda_LocExt tipoMda = (TipoMoneda_LocExt)Enum.Parse(typeof(TipoMoneda_LocExt), this.cmbTipoMda.EditValue.ToString());
                    TipoMoneda mdaOrigen = (TipoMoneda)Enum.Parse(typeof(TipoMoneda), this.cmbOrigenMda.EditValue.ToString());
                    byte tipoInforme = Convert.ToByte(this.cmbTipoInforme.EditValue.ToString());
                    ProyectoTipo proyType = (ProyectoTipo)Enum.Parse(typeof(ProyectoTipo), this.cmbTipoProyecto.EditValue.ToString());
                    this._cierresQuery = this._bc.AdministrationModel.ocDetalleLegalizacion_GetInfoMensual(this._documentID, dtoFilterCierre, tipoInforme, proyType, tipoMda, mdaOrigen);
                    if (this._cierresQuery.Count > 0)
                        this.gvDocument.FocusedRowHandle = 0;

                    this.gcDocument.DataSource = this._cierresQuery;
                    //this.panel1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryInformeMensualOC.cs", "TBSearch"));
            }
        }

        #endregion

    }
}
