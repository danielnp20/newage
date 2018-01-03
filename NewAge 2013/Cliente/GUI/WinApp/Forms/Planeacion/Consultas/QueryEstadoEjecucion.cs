

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
    public partial class QueryEstadoEjecucion : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private List<DTO_QueryEstadoEjecucion> _cierresQuery;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private string _unboundPrefix = "Unbound_";
        private int _documentID;

        private string _monedaLocal = string.Empty;
        private string _monedaExtr = string.Empty;

        #endregion

        public QueryEstadoEjecucion()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "QueryEstadoEjecucion"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryEstadoEjecucion;
            this._frmModule = ModulesPrefix.pl;
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
                this._bc.InitMasterUC(this.masterBloque, AppMasters.ocBloque, true, true, true, false); //falta maestra bloque
                this._bc.InitMasterUC(this.masterCampo, AppMasters.glAreaFisica, false, true, true, false);
                this._bc.InitMasterUC(this.masterPozo, AppMasters.glLocFisica, false, true, true, false);
                this._bc.InitMasterUC(this.masterRecurso, AppMasters.plRecurso, true, true, true, false);
                this._bc.InitMasterUC(this.masterGrupo, AppMasters.coActividad, false, true, true, false);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columnas de Grilla principal

                #region Columnas visibles

                //ProyectoID(AFE)
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 70;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

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

                //Disponible
                GridColumn Disponible = new GridColumn();
                Disponible.FieldName = this._unboundPrefix + "Disponible";
                Disponible.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Disponible");
                Disponible.UnboundType = UnboundColumnType.Decimal;
                Disponible.VisibleIndex = 2;
                Disponible.Width = 70;
                Disponible.Visible = true;
                Disponible.ColumnEdit = this.TextEdit;
                Disponible.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Disponible);

                //Presupuesto
                GridColumn Presupuesto = new GridColumn();
                Presupuesto.FieldName = this._unboundPrefix + "Presupuesto";
                Presupuesto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Presupuesto");
                Presupuesto.UnboundType = UnboundColumnType.Decimal;
                Presupuesto.VisibleIndex = 3;
                Presupuesto.Width = 70;
                Presupuesto.Visible = true;
                Presupuesto.ColumnEdit = this.TextEdit;
                Presupuesto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Presupuesto);

                //Compromisos
                GridColumn Compromisos = new GridColumn();
                Compromisos.FieldName = this._unboundPrefix + "Compromisos";
                Compromisos.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Compromisos");
                Compromisos.UnboundType = UnboundColumnType.Decimal;
                Compromisos.VisibleIndex = 4;
                Compromisos.Width = 70;
                Compromisos.Visible = true;
                Compromisos.ColumnEdit = this.TextEdit;
                Compromisos.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Compromisos);

                //Recibidos
                GridColumn Recibidos = new GridColumn();
                Recibidos.FieldName = this._unboundPrefix + "Recibidos";
                Recibidos.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Recibidos");
                Recibidos.UnboundType = UnboundColumnType.Decimal;
                Recibidos.VisibleIndex = 5;
                Recibidos.Width = 70;
                Recibidos.Visible = true;
                Recibidos.ColumnEdit = this.TextEdit;
                Recibidos.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Recibidos);

                //Ejecutado
                GridColumn Ejecutado = new GridColumn();
                Ejecutado.FieldName = this._unboundPrefix + "Ejecutado";
                Ejecutado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ejecutado");
                Ejecutado.UnboundType = UnboundColumnType.Decimal;
                Ejecutado.VisibleIndex = 6;
                Ejecutado.Width = 70;
                Ejecutado.Visible = true;
                Ejecutado.ColumnEdit = this.TextEdit;
                Ejecutado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Ejecutado);

                //EjeVsPre
                GridColumn EjeVsPre = new GridColumn();
                EjeVsPre.FieldName = this._unboundPrefix + "EjeVsPre";
                EjeVsPre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_EjeVsPre");
                EjeVsPre.UnboundType = UnboundColumnType.Decimal;
                EjeVsPre.VisibleIndex = 7;
                EjeVsPre.Width = 70;
                EjeVsPre.Visible = true;
                EjeVsPre.ColumnEdit = this.TextEdit2;
                EjeVsPre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(EjeVsPre);

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

                //RecursoID
                GridColumn RecursoIDDeta1 = new GridColumn();
                RecursoIDDeta1.FieldName = this._unboundPrefix + "RecursoID";
                RecursoIDDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_RecursoID");
                RecursoIDDeta1.UnboundType = UnboundColumnType.String;
                RecursoIDDeta1.VisibleIndex = 0;
                RecursoIDDeta1.Width = 70;
                RecursoIDDeta1.Visible = true;
                RecursoIDDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(RecursoIDDeta1);

                //CentroCostoID
                GridColumn CentroCostoIDeta1 = new GridColumn();
                CentroCostoIDeta1.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoIDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CentroCostoID");
                CentroCostoIDeta1.UnboundType = UnboundColumnType.String;
                CentroCostoIDeta1.VisibleIndex = 0;
                CentroCostoIDeta1.Width = 70;
                CentroCostoIDeta1.Visible = false;
                CentroCostoIDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(CentroCostoIDeta1);

                //DescriptivoDeta1
                GridColumn DescriptivoDeta1 = new GridColumn();
                DescriptivoDeta1.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta1.UnboundType = UnboundColumnType.String;
                DescriptivoDeta1.VisibleIndex = 1;
                DescriptivoDeta1.Width = 120;
                DescriptivoDeta1.Visible = true;
                DescriptivoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(DescriptivoDeta1);

                //Disponible
                GridColumn DisponibleDeta1 = new GridColumn();
                DisponibleDeta1.FieldName = this._unboundPrefix + "Disponible";
                DisponibleDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Disponible");
                DisponibleDeta1.UnboundType = UnboundColumnType.Decimal;
                DisponibleDeta1.VisibleIndex = 2;
                DisponibleDeta1.Width = 70;
                DisponibleDeta1.Visible = true;
                DisponibleDeta1.ColumnEdit = this.TextEdit;
                DisponibleDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(DisponibleDeta1);

                //Presupuesto
                GridColumn PresupuestoDeta1 = new GridColumn();
                PresupuestoDeta1.FieldName = this._unboundPrefix + "Presupuesto";
                PresupuestoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Presupuesto");
                PresupuestoDeta1.UnboundType = UnboundColumnType.Decimal;
                PresupuestoDeta1.VisibleIndex = 3;
                PresupuestoDeta1.Width = 70;
                PresupuestoDeta1.Visible = true;
                PresupuestoDeta1.ColumnEdit = this.TextEdit;
                PresupuestoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(PresupuestoDeta1);

                //Compromisos
                GridColumn CompromisosDeta1 = new GridColumn();
                CompromisosDeta1.FieldName = this._unboundPrefix + "Compromisos";
                CompromisosDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Compromisos");
                CompromisosDeta1.UnboundType = UnboundColumnType.Decimal;
                CompromisosDeta1.VisibleIndex = 4;
                CompromisosDeta1.Width = 70;
                CompromisosDeta1.Visible = true;
                CompromisosDeta1.ColumnEdit = this.TextEdit;
                CompromisosDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(CompromisosDeta1);

                //Recibidos
                GridColumn RecibidosDeta1 = new GridColumn();
                RecibidosDeta1.FieldName = this._unboundPrefix + "Recibidos";
                RecibidosDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Recibidos");
                RecibidosDeta1.UnboundType = UnboundColumnType.Decimal;
                RecibidosDeta1.VisibleIndex = 5;
                RecibidosDeta1.Width = 70;
                RecibidosDeta1.Visible = true;
                RecibidosDeta1.ColumnEdit = this.TextEdit;
                RecibidosDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(RecibidosDeta1);

                //Ejecutado
                GridColumn EjecutadoDeta1 = new GridColumn();
                EjecutadoDeta1.FieldName = this._unboundPrefix + "Ejecutado";
                EjecutadoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ejecutado");
                EjecutadoDeta1.UnboundType = UnboundColumnType.Decimal;
                EjecutadoDeta1.VisibleIndex = 6;
                EjecutadoDeta1.Width = 70;
                EjecutadoDeta1.Visible = true;
                EjecutadoDeta1.ColumnEdit = this.TextEdit;
                EjecutadoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(EjecutadoDeta1);

                //EjeVsPre
                GridColumn EjeVsPreDeta1 = new GridColumn();
                EjeVsPreDeta1.FieldName = this._unboundPrefix + "EjeVsPre";
                EjeVsPreDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_EjeVsPre");
                EjeVsPreDeta1.UnboundType = UnboundColumnType.Decimal;
                EjeVsPreDeta1.VisibleIndex = 7;
                EjeVsPreDeta1.Width = 70;
                EjeVsPreDeta1.Visible = true;
                EjeVsPreDeta1.ColumnEdit = this.TextEdit2;
                EjeVsPreDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel1.Columns.Add(EjeVsPreDeta1);
             
                #endregion
                #endregion

                #region Columnas Detalle Nivel 2
                #region Columnas visibles

                //LineaPresupuesto
                GridColumn LineaPresupuestoDeta2 = new GridColumn();
                LineaPresupuestoDeta2.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuestoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaPresupuestoID");
                LineaPresupuestoDeta2.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDeta2.VisibleIndex = 0;
                LineaPresupuestoDeta2.Width = 70;
                LineaPresupuestoDeta2.Visible = true;
                LineaPresupuestoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(LineaPresupuestoDeta2);

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

                //DisponibleDeta2
                GridColumn DisponibleDeta2 = new GridColumn();
                DisponibleDeta2.FieldName = this._unboundPrefix + "Disponible";
                DisponibleDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Disponible");
                DisponibleDeta2.UnboundType = UnboundColumnType.Decimal;
                DisponibleDeta2.VisibleIndex = 2;
                DisponibleDeta2.Width = 70;
                DisponibleDeta2.Visible = true;
                DisponibleDeta2.ColumnEdit = this.TextEdit;
                DisponibleDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(DisponibleDeta2);

                //Presupuesto
                GridColumn EneroDeta2 = new GridColumn();
                EneroDeta2.FieldName = this._unboundPrefix + "Presupuesto";
                EneroDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Presupuesto");
                EneroDeta2.UnboundType = UnboundColumnType.Decimal;
                EneroDeta2.VisibleIndex = 3;
                EneroDeta2.Width = 70;
                EneroDeta2.Visible = true;
                EneroDeta2.ColumnEdit = this.TextEdit;
                EneroDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(EneroDeta2);

                //Compromisos
                GridColumn CompromisosDeta2 = new GridColumn();
                CompromisosDeta2.FieldName = this._unboundPrefix + "Compromisos";
                CompromisosDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Compromisos");
                CompromisosDeta2.UnboundType = UnboundColumnType.Decimal;
                CompromisosDeta2.VisibleIndex = 4;
                CompromisosDeta2.Width = 70;
                CompromisosDeta2.Visible = true;
                CompromisosDeta2.ColumnEdit = this.TextEdit;
                CompromisosDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(CompromisosDeta2);

                //Recibidos
                GridColumn RecibidosDeta2 = new GridColumn();
                RecibidosDeta2.FieldName = this._unboundPrefix + "Recibidos";
                RecibidosDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Recibidos");
                RecibidosDeta2.UnboundType = UnboundColumnType.Decimal;
                RecibidosDeta2.VisibleIndex = 5;
                RecibidosDeta2.Width = 70;
                RecibidosDeta2.Visible = true;
                RecibidosDeta2.ColumnEdit = this.TextEdit;
                RecibidosDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(RecibidosDeta2);

                //Ejecutado
                GridColumn EjecutadoDeta2 = new GridColumn();
                EjecutadoDeta2.FieldName = this._unboundPrefix + "Ejecutado";
                EjecutadoDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ejecutado");
                EjecutadoDeta2.UnboundType = UnboundColumnType.Decimal;
                EjecutadoDeta2.VisibleIndex = 6;
                EjecutadoDeta2.Width = 70;
                EjecutadoDeta2.Visible = true;
                EjecutadoDeta2.ColumnEdit = this.TextEdit;
                EjecutadoDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(EjecutadoDeta2);

                //EjeVsPre
                GridColumn EjeVsPreDeta2 = new GridColumn();
                EjeVsPreDeta2.FieldName = this._unboundPrefix + "EjeVsPre";
                EjeVsPreDeta2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_EjeVsPre");
                EjeVsPreDeta2.UnboundType = UnboundColumnType.Decimal;
                EjeVsPreDeta2.VisibleIndex = 7;
                EjeVsPreDeta2.Width = 70;
                EjeVsPreDeta2.Visible = true;
                EjeVsPreDeta2.ColumnEdit = this.TextEdit2;
                EjeVsPreDeta2.OptionsColumn.AllowEdit = false;
                this.gvDetalleNivel2.Columns.Add(EjeVsPreDeta2);
             
                #endregion
                #endregion
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.gvDetalleNivel1.OptionsView.ColumnAutoWidth = true;
                this.gvDetalleNivel2.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "QueryEstadoEjecucion.cs-AddGridCols"));
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

        /// <summary>
        /// Habilita o deshabilita el header
        /// </summary>
        /// <param name="enable">indica si habilita o deshabilita</param>
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "Form_Enter: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "Form_Leave: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "Form_Closing: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "Form_FormClosed: " + ex.Message));
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
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "RecursoID"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                }
                else if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex ||
                         Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                { 
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "RecursoID"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "CentroCostoID"].Visible = false;
                }
                else
                {
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "CentroCostoID"].Visible = true;
                    this.gvDetalleNivel1.Columns[this._unboundPrefix + "RecursoID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "cmbProyectoTipo_EditValueChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "cmbMdOrigen_EditValueChanged"));
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
                DTO_coActividad actividad = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proy.ActividadID.Value, true);
                DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                this.masterPozo.Value = proy.LocFisicaID.Value;
                this.masterContrato.Value = proy.ContratoID.Value;
                this.cmbTipoProyecto.EditValue = actividad.ProyectoTipo.Value.ToString();
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
                this._cierresQuery = new List<DTO_QueryEstadoEjecucion>();

                this.gcDocument.DataSource = this._cierresQuery;
                this.gvDocument.RefreshData();
                this.panel1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "TBNew"));
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
                    DTO_QueryEstadoEjecucion dtoFilterCierre = new DTO_QueryEstadoEjecucion();
                    dtoFilterCierre.PeriodoID.Value = this.dtPeriod.DateTime;
                    dtoFilterCierre.ProyectoID.Value = this.masterProyecto.Value;
                    dtoFilterCierre.RecursoID.Value = this.masterRecurso.Value;
                    dtoFilterCierre.ActividadID.Value = this.masterGrupo.Value;
                    dtoFilterCierre.Campo.Value = this.masterCampo.Value;
                    dtoFilterCierre.ContratoID.Value = this.masterContrato.Value;
                    dtoFilterCierre.Grupo.Value = this.masterGrupo.Value;
                    dtoFilterCierre.Pozo.Value = this.masterPozo.Value;
                    TipoMoneda_LocExt tipoMda = (TipoMoneda_LocExt)Enum.Parse(typeof(TipoMoneda_LocExt), this.cmbTipoMda.EditValue.ToString());
                    TipoMoneda mdaOrigen = (TipoMoneda)Enum.Parse(typeof(TipoMoneda), this.cmbOrigenMda.EditValue.ToString());
                    ProyectoTipo proyType = (ProyectoTipo)Enum.Parse(typeof(ProyectoTipo), this.cmbTipoProyecto.EditValue.ToString());
                    this._cierresQuery = this._bc.AdministrationModel.plCierreLegalizacion_GetEstadoEjecByPeriodo(this._documentID, dtoFilterCierre, proyType, tipoMda, mdaOrigen);
                    if (this._cierresQuery.Count > 0)
                        this.gvDocument.FocusedRowHandle = 0;

                    this.gcDocument.DataSource = this._cierresQuery;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "TBSearch"));
            }
        }

        #endregion

    }
}
