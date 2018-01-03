using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Globalization;
using NewAge.DTO.Attributes;
using DevExpress.XtraEditors;
using System.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class IncorporaDescuento : DocumentAprobBasicForm
    {

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Otros
        private DTO_MigrarIncorporacionDeta reincorporacion;
        private List<DTO_MigrarIncorporacionDeta> migracionIncorp = new List<DTO_MigrarIncorporacionDeta>();
        private DateTime periodo = DateTime.Now;
        private Dictionary<int, string> estadosCruce = new Dictionary<int, string>();
        private Dictionary<int, string> tiposNovedad = new Dictionary<int, string>();
        private Dictionary<string, DTO_ccPagaduria> pagadurias = new Dictionary<string, DTO_ccPagaduria>();
        private Dictionary<string, DTO_ccCliente> clientes = new Dictionary<string, DTO_ccCliente>();
        private bool validateData;

        //Recursos
        private string msgEmptyField;
        private string msgFkNotFound;
        private string msgPositive;
        private string msgLibranzaInvalida;
        private string msgLibranzaCancelada;
        private string msgLibranzaRepetida;

        //Variables de importacion
        private string format;
        private string formatSeparator = "\t";
        private PasteOpDTO pasteRet;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        private void RefreshGridMethod()
        {
            this.FilterData(null, null);
        }

        #endregion
        
        #region Constructor

        public IncorporaDescuento()
            : base()
        {
            //this.InitializeComponent();
        }

        public IncorporaDescuento(string mod)
            : base(mod)
        {
            this.Text = _bc.GetResource(LanguageTypes.Forms, AppDocuments.IncorporacionDirecta.ToString());
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ReincorporacionCartera;
            this.frmModule = ModulesPrefix.cc;

            //Carga el periodo
            string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.cc_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodo);
            this.dtFecha.DateTime = this.dtPeriodo.DateTime;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
            _bc.InitMasterUC(this.masterNovedad, AppMasters.ccIncorporacionNovedad, true, true, true, false);

            //Tipos de novedad (visible en la grilla)
            this.tiposNovedad[0] = string.Empty;
            this.tiposNovedad[1] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_1);
            this.tiposNovedad[2] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_2);
            this.tiposNovedad[3] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_3);
            this.tiposNovedad[4] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_4);
            this.tiposNovedad[5] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_5);
            this.tiposNovedad[6] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_6);

            //Diccionarios para los recursos de los estados de cruce
            this.estadosCruce[0] = string.Empty;
            this.estadosCruce[1] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_1);
            this.estadosCruce[2] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_2);
            this.estadosCruce[3] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_3);
            this.estadosCruce[4] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_4);
            this.estadosCruce[5] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_5);
            this.estadosCruce[6] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_6);
            this.estadosCruce[7] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_7);
            this.estadosCruce[8] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_8);
            this.estadosCruce[9] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_9);
            this.estadosCruce[10] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_10);
            this.estadosCruce[11] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_EstadoCruce_11);
            this.lkp_EstadoCruce.Properties.DataSource = this.estadosCruce;

            //Selección
            Dictionary<string, string> dictSeleccion = new Dictionary<string, string>();
            dictSeleccion.Add(DictionaryTables.Todos, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Todos));
            dictSeleccion.Add(DictionaryTables.Seleccionados, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Seleccionados));
            dictSeleccion.Add(DictionaryTables.NoSeleccionados, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.NoSeleccionados));
            this.lkp_Seleccion.Properties.DataSource = dictSeleccion;
            this.lkp_Seleccion.EditValue = DictionaryTables.Todos;

            //Recursos
            this.msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
            this.msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            this.msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            this.msgLibranzaInvalida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_InvalidLibranza);
            this.msgLibranzaCancelada = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Cc_ReIncorporacionLibranzaCancelada);
            this.msgLibranzaRepetida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CreditoAgregado);
            //this.msgInvalidTipoNov = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov);
            //this.msgInvalidTipoNov1 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov1);
            //this.msgInvalidTipoNov4 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov4);
            //this.msgInvalidTipoNov5 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov5);
            //this.msgInvalidTipoNovPagaduriasDif = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNovPagaduriasDif);

            //this.msgInvalidTipoNov1 = string.Format(this.msgInvalidTipoNov1, this.tiposNovedad[1].ToUpper());
            //this.msgInvalidTipoNov4 = string.Format(this.msgInvalidTipoNov4, this.tiposNovedad[4].ToUpper());
            //this.msgInvalidTipoNov5 = string.Format(this.msgInvalidTipoNov5, this.tiposNovedad[5].ToUpper());

            //Formato importar
            this.format = _bc.GetImportExportFormat(typeof(DTO_MigrarIncorporacionDeta), this.documentID);

            this.chkSeleccionar.Visible = false;
            this.lkp_EstadoCruce.Visible = false;
            this.lkp_Seleccion.Visible = false;
            this.masterNovedad.Visible = false;
            this.masterPagaduria.Visible = false;
            this.txtLibranza.Visible = false;
            this.lblLibranza.Visible = false;
            this.lblUserTareas.Visible = false;
            this.label3.Visible = false;
            this.label2.Visible = false;

            base.SetInitParameters();
            //this.gcDocuments.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            //new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            //new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
        }

        /// <summary>
        /// Evento que se ejecuta al cargar la info del formulario
        /// </summary>
        protected override void AfterInitialize()
        {
            this.dtPeriodo.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.dtPeriodo_ValueChanged);
            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);

            this.editCmb.Items.AddRange(this.tiposNovedad);
        }

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected void LoadData()
        {
            try
            {
                if (this.periodo != this.dtPeriodo.DateTime)
                {
                    this.migracionIncorp = new List<DTO_MigrarIncorporacionDeta>();

                    this.periodo = this.dtPeriodo.DateTime;
                    //this.reincorporaciones = this._bc.AdministrationModel.Reincorporacion_GetForReincorporacion(this.periodo, string.Empty);//, this.centroPagoID);
                    this.migracionIncorp.ForEach(x => x.TipoNovedad.Value = (byte)TipoNovedad.ReIncorporar);

                    int currentMonth = this.dtPeriodo.DateTime.Month;
                    int currentYear = this.dtPeriodo.DateTime.Year;
                    int minDay = 1;
                    int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                    this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);

                    this.reincorporacion = this.migracionIncorp.FirstOrDefault();                       

                    this.gcDocuments.DataSource = this.migracionIncorp;
                    this.gvDocuments.RefreshData();

                    this.validateData = true;                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                ////Aprobar
                //GridColumn aprob = new GridColumn();
                //aprob.FieldName = this.unboundPrefix + "Aprobado";
                //aprob.Caption = "√";
                //aprob.UnboundType = UnboundColumnType.Boolean;
                //aprob.VisibleIndex = 0;
                //aprob.Width = 7;
                //aprob.Visible = true;
                //aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                //aprob.AppearanceHeader.ForeColor = Color.Lime;
                //aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                //aprob.AppearanceHeader.Options.UseTextOptions = true;
                //aprob.AppearanceHeader.Options.UseFont = true;
                //aprob.AppearanceHeader.Options.UseForeColor = true;
                //this.gvDocuments.Columns.Add(aprob);

                //PagaduriaID
                GridColumn PagaduriaID = new GridColumn();
                PagaduriaID.FieldName = this.unboundPrefix + "PagaduriaID";
                PagaduriaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagaduriaID");
                PagaduriaID.UnboundType = UnboundColumnType.String;
                PagaduriaID.VisibleIndex = 1;
                PagaduriaID.Width = 40;
                PagaduriaID.Visible = true;
                PagaduriaID.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                PagaduriaID.OptionsColumn.AllowEdit = false;
                PagaduriaID.ColumnEdit = this.editBtnGrid;
                this.gvDocuments.Columns.Add(PagaduriaID);

                //PagaduriaDesc
                GridColumn PagaduriaDesc = new GridColumn();
                PagaduriaDesc.FieldName = this.unboundPrefix + "PagaduriaDesc";
                PagaduriaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagaduriaDesc");
                PagaduriaDesc.UnboundType = UnboundColumnType.String;
                PagaduriaDesc.VisibleIndex = 2;
                PagaduriaDesc.Width = 40;
                PagaduriaDesc.Visible = true;
                PagaduriaDesc.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                PagaduriaDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(PagaduriaDesc);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 40;
                clienteID.Visible = true;
                clienteID.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 4;
                nombre.Width = 70;
                nombre.Visible = true;
                nombre.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                libranza.AppearanceCell.Options.UseTextOptions = true;
                libranza.VisibleIndex = 5;
                libranza.Width = 20;
                libranza.Visible = true;
                libranza.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);

                //PlazoINC
                GridColumn PlazoINC = new GridColumn();
                PlazoINC.FieldName = this.unboundPrefix + "PlazoINC";
                PlazoINC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlazoINC");
                PlazoINC.UnboundType = UnboundColumnType.String;
                PlazoINC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                PlazoINC.AppearanceCell.Options.UseTextOptions = true;
                PlazoINC.VisibleIndex = 6;
                PlazoINC.Width = 20;
                PlazoINC.Visible = true;
                PlazoINC.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                PlazoINC.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(PlazoINC);

                //Vlr Cuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "ValorCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorCuota");
                vlrCuota.UnboundType = UnboundColumnType.String;
                vlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuota.AppearanceCell.Options.UseTextOptions = true;
                vlrCuota.VisibleIndex = 7;
                vlrCuota.Width = 40;
                vlrCuota.Visible = false;
                vlrCuota.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                vlrCuota.OptionsColumn.AllowEdit = true;
                vlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrCuota);

                //Vlr VlrMora
                GridColumn VlrMora = new GridColumn();
                VlrMora.FieldName = this.unboundPrefix + "VlrMora";
                VlrMora.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrMora");
                VlrMora.UnboundType = UnboundColumnType.String;
                VlrMora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrMora.AppearanceCell.Options.UseTextOptions = true;
                VlrMora.VisibleIndex = 8;
                VlrMora.Width = 40;
                VlrMora.Visible = false;
                VlrMora.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                VlrMora.OptionsColumn.AllowEdit = true;
                VlrMora.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrMora);

                //Vlr VlrIncrMora
                GridColumn VlrIncrMora = new GridColumn();
                VlrIncrMora.FieldName = this.unboundPrefix + "VlrIncrMora";
                VlrIncrMora.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIncrMora");
                VlrIncrMora.UnboundType = UnboundColumnType.String;
                VlrIncrMora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrIncrMora.AppearanceCell.Options.UseTextOptions = true;
                VlrIncrMora.VisibleIndex = 9;
                VlrIncrMora.Width = 40;
                VlrIncrMora.Visible = false;
                VlrIncrMora.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                VlrIncrMora.OptionsColumn.AllowEdit = true;
                VlrIncrMora.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrIncrMora);

                //Vlr VlrDtoMes
                GridColumn VlrDtoMes = new GridColumn();
                VlrDtoMes.FieldName = this.unboundPrefix + "VlrDtoMes";
                VlrDtoMes.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrDtoMes");
                VlrDtoMes.UnboundType = UnboundColumnType.String;
                VlrDtoMes.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrDtoMes.AppearanceCell.Options.UseTextOptions = true;
                VlrDtoMes.VisibleIndex = 10;
                VlrDtoMes.Width = 40;
                VlrDtoMes.Visible = true;
                VlrDtoMes.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                VlrDtoMes.OptionsColumn.AllowEdit = true;
                VlrDtoMes.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(VlrDtoMes);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.Width = 50;
                observacion.VisibleIndex = 11;
                observacion.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                observacion.Visible = true;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(observacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected override bool ValidateDocRow(int fila)
        {
            try
            {
                this.validRow = true;
                GridColumn colLibranza = this.gvDocuments.Columns[this.unboundPrefix + "Libranza"];
                GridColumn colNovedad = this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"];
                GridColumn colValorCuota = this.gvDocuments.Columns[this.unboundPrefix + "ValorCuota"];
                GridColumn colCentroPagoModificaID = this.gvDocuments.Columns[this.unboundPrefix + "CentroPagoModificaID"];
                GridColumn colTipoNovedad = this.gvDocuments.Columns[this.unboundPrefix + "TipoNovedad"];

                //this.reincorporacion = this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle];
                //if (this.reincorporacion.Aprobado.Value.Value)
                //{
                //    #region Valor Cuota
                //    if (this.reincorporacion.ValorCuota.Value == 0)
                //    {
                //        string msg = string.Format(this.msgPositive, colValorCuota.Caption);
                //        this.gvDocuments.SetColumnError(colValorCuota, msg);
                //        this.validRow = false;
                //    }
                //    else
                //    {
                //        this.gvDocuments.SetColumnError(colValorCuota, string.Empty);
                //    }
                //    #endregion
                //}
                //else
                //{
                //    this.gvDocuments.SetColumnError(colLibranza, string.Empty);
                //    this.gvDocuments.SetColumnError(colNovedad, string.Empty);
                //    this.gvDocuments.SetColumnError(colValorCuota, string.Empty);
                //    this.gvDocuments.SetColumnError(colCentroPagoModificaID, string.Empty);
                //    this.gvDocuments.SetColumnError(colTipoNovedad, string.Empty);
                //}

                return this.validRow;
            }
            catch (Exception ex)
            {
                this.validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "ValidateDocRow"));
                return false;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.validateData = false;
            
            this.periodo = DateTime.Now;
            string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.cc_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodo);

            this.masterPagaduria.Value = string.Empty;
            this.masterNovedad.Value = string.Empty;
            this.txtLibranza.Text = string.Empty;
            this.lkp_EstadoCruce.EditValue = 0;
            this.lkp_Seleccion.EditValue = DictionaryTables.Todos;

            this.currentRow = -1;
            this.migracionIncorp = new List<DTO_MigrarIncorporacionDeta>();
            this.gcDocuments.DataSource = this.migracionIncorp;
            this.masterPagaduria.Value = String.Empty;

            this.validateData = true;
        }

        /// <summary>
        /// Carga la información de una reincorporación a partir de la libranza
        /// </summary>
        /// <param name="libranza"></param>
        /// <returns></returns>
        private void LoadReincorporacionFromLibranza(DTO_MigrarIncorporacionDeta reinc)
        {
            try
            {
                //Valida el crédito
                DTO_ccCreditoDocu cred = _bc.AdministrationModel.GetCreditoByLibranza(reinc.Libranza.Value.Value);
                if (cred != null)
                {
                    reinc.NumDocCredito.Value = cred.NumeroDoc.Value;
                    reinc.NovedadIncorporaID.Value = cred.NovedadIncorporaID.Value;
                    //reinc.FechaCuota1.Value = cred.FechaCuota1.Value;
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "LoadReincorporacionFromLibranza"));
            }
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);

                FormProvider.Master.itemGenerateTemplate.Visible = true;
                FormProvider.Master.itemImport.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemGenerateTemplate.Enabled = true;
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Evento Header

        /// <summary>
        /// Evento al cambiar el periodo
        /// </summary>
        private void dtPeriodo_ValueChanged()
        {
        }

        /// <summary>
        /// Filters current data
        /// </summary>
        private void FilterData(object sender, EventArgs e)
        {
            try
            {
                if (this.migracionIncorp != null && this.migracionIncorp.Count > 0)
                {
                    this.validateData = false;
                    this.reincorporacion = this.migracionIncorp.FirstOrDefault();

                    this.gcDocuments.DataSource = this.migracionIncorp;
                    this.gvDocuments.RefreshData();
                    this.validateData = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "FilterData"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object obj = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                    PropertyInfo pi = obj.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(obj, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(obj, null), null);
                        }
                    else
                    {
                        FieldInfo fi = obj.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(obj);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(obj), null);
                            }
                        }
                    }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = obj.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(obj, null);
                    }
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(obj, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = obj.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(obj);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (this.validateData && this.currentRow != -1)
            {
                if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                    e.Allow = false;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.currentRow = e.FocusedRowHandle;
            if (this.validateData && this.currentRow >= 0 && this.migracionIncorp.Count > 0)
            {
                this.reincorporacion = this.migracionIncorp[this.currentRow];        
            }
            else
            {
                this.currentRow = -1;
                this.reincorporacion = this.migracionIncorp != null ? this.migracionIncorp.FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                //Valor Cuota
                if (fieldName == "Aprobado")
                {
                    if (!(bool)e.Value)
                    {
                        this.gvDocuments.RefreshData();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "gvDocuments_CellValueChanging"));
            }
        }

        /// <summary>
        /// Evento q valida antes de dejar la fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                            

                //Valor Cuota
                if (fieldName == "ValorCuota")
                {
                    if (!this.reincorporacion.ValorCuota.Value.HasValue)
                    {
                        this.reincorporacion.ValorCuota.Value = 0;
                        this.migracionIncorp[e.RowHandle].ValorCuota.Value = 0;
                    }
                }               
                this.gvDocuments.RefreshData();
                this.ValidateDocRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "gvDocuments_CellValueChanged"));
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                if (colName == "CentroPagoModificaID")
                    colName = "CentroPagoID";

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocuments.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (this.migracionIncorp != null && this.migracionIncorp.Count > 0)
                {
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para generar la plantilla de importar datos
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar los datos de la plantilla
        /// </summary>
        public override void TBImport()
        {
            try
            {
                //if (this.reincorporaciones.Count > 0)
                //{
                //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportThread);
                        process.Start();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBGenerateTemplate.cs", "TBImport"));
            }
        }

        /// <summary>
        /// Boton para exportar los datos de la plantilla
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.migracionIncorp.Count > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    DataTable tableExport = new DataTable();
                    tableExport.Columns.Add(this.lblPeriodo.Text);
                    tableExport.Columns.Add("PagaduriaID");
                    tableExport.Columns.Add("PagaduriaDesc");
                    tableExport.Columns.Add("ClienteID");
                    tableExport.Columns.Add("Nombre");
                    tableExport.Columns.Add("Libranza");                   
                    tableExport.Columns.Add("PlazoINC");
                    tableExport.Columns.Add("ValorCuota");
                    tableExport.Columns.Add("VlrMora");
                    tableExport.Columns.Add("VlrIncrMora");
                    tableExport.Columns.Add("VlrDtoMes");
                    tableExport.Columns.Add("Observacion");

                    foreach(DTO_MigrarIncorporacionDeta reinc in this.migracionIncorp)
                    {
                        string[] cols = 
                        {
                            this.dtPeriodo.DateTime.ToString(FormatString.Period),                           
                            reinc.ClienteID.Value,
                            reinc.Nombre.Value,
                            reinc.Libranza.Value.ToString(),
                            reinc.ValorCuota.Value != null? reinc.ValorCuota.Value.Value.ToString("n0"): "0",
                            reinc.VlrMora.Value != null? reinc.VlrMora.Value.Value.ToString("n0"): "0",
                            reinc.VlrIncrMora.Value != null? reinc.VlrIncrMora.Value.Value.ToString("n0"): "0",
                            reinc.VlrDtoMes.Value != null? reinc.VlrDtoMes.Value.Value.ToString("n0"): "0",
                            reinc.Observacion.Value.ToString(),
                        };
                        tableExport.Rows.Add(cols);
                    }

                    ReportExcelBase frm = new ReportExcelBase(tableExport, this.documentID, false);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "TBExport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        private void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error

                    DTO_TxResult result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    Dictionary<string, bool> novedades = new Dictionary<string, bool>();
                    Dictionary<string, bool> centrosPago = new Dictionary<string, bool>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();

                    //Mensajes de error
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgPkAdded = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReIncorporacionPkAdded);
                    string msgMultMod = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReIncorporacionMultipleCPMod);

                    List<DTO_MigrarIncorporacionDeta> list = new List<DTO_MigrarIncorporacionDeta>();
                    this.migracionIncorp = new List<DTO_MigrarIncorporacionDeta>();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigrarIncorporacionDeta).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer

                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }

                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            DTO_MigrarIncorporacionDeta det = new DTO_MigrarIncorporacionDeta();
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica

                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];                                  
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == "Libranza" || colName == "TipoNovedad"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                    if (val < 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                    if (val < 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                    if (val < 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                    if (val < 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                    else if (val < 0)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = string.Format(msgPositive, colRsx);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        }
                                        #endregion  Validacion si no es null y formatos

                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp-IncorporaDescuento.cs", "ImportThread - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la información de los resultados

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                                list.Add(det);
                            else
                                validList = false;

                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Validaciones de los datos

                    if (validList)
                    {
                        int i = 0;
                        percent = 0; 
                        Dictionary<Tuple<int, string>, int> pkAdded = new Dictionary<Tuple<int, string>, int>();
                        Dictionary<int, int> cpModificaIndex = new Dictionary<int, int>();
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        foreach (DTO_MigrarIncorporacionDeta reInc in list)
                        {
                            #region Carga de porcentaje
                            ++i;
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (list.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            reInc.TipoNovedad.Value = 2;
                            reInc.FechaNovedad.Value = new DateTime(this.dtFecha.DateTime.Year, this.dtFecha.DateTime.Month, DateTime.DaysInMonth(this.dtFecha.DateTime.Year, this.dtFecha.DateTime.Month));
                            reInc.FechaNovedad.Value = reInc.FechaNovedad.Value.Value.Day == 31 ? new DateTime(this.dtFecha.DateTime.Year, this.dtFecha.DateTime.Month, 30) : reInc.FechaNovedad.Value;
                            reInc.FechaCuota1.Value = reInc.FechaNovedad.Value;
                            reInc.ValorNomina.Value = reInc.VlrDtoMes.Value;
                            this.LoadReincorporacionFromLibranza(reInc);
                            this.migracionIncorp.Add(reInc);
                        }
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }

                    #endregion

                   this.Invoke(this.refreshGridDelegate);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "ImportThread"));
            }
            finally
            {
                if (!this.pasteRet.Success)
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_TxResult result = _bc.AdministrationModel.Incorporacion_Guardar(this.documentID, this.dtPeriodo.DateTime, this.dtFecha.DateTime, this.migracionIncorp);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (result.Result == ResultValue.OK)
                    this.Invoke(this.refreshData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-IncorporaDescuento.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
