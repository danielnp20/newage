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
    public partial class Reincorporacion : DocumentAprobBasicForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
            this.LoadDocuments();
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

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
    
        //Otros
        private DTO_ccReincorporacionDeta reincorporacion;
        private List<DTO_ccReincorporacionDeta> reincorporaciones = new List<DTO_ccReincorporacionDeta>();
        private List<DTO_ccReincorporacionDeta> reincorporacionesFilter = new List<DTO_ccReincorporacionDeta>();
        private DateTime periodo = DateTime.Now;
        private Dictionary<int, string> estadosCruce = new Dictionary<int, string>();
        private Dictionary<int, string> tiposNovedad = new Dictionary<int, string>();
        private Dictionary<string, DTO_ccCentroPagoPAG> centrosPago = new Dictionary<string, DTO_ccCentroPagoPAG>();
        private Dictionary<string, DTO_ccPagaduria> pagadurias = new Dictionary<string, DTO_ccPagaduria>();
        private Dictionary<string, DTO_ccCliente> clientes = new Dictionary<string, DTO_ccCliente>();
        private DTO_ccCentroPagoPAG centroPago;
        private DTO_ccPagaduria pagaduria;
        private bool validateData;
        private bool isCreditoCancelado;

        //Recursos
        private string msgEmptyField;
        private string msgFkNotFound;
        private string msgPositive;
        private string msgInvalidTipoNov;
        private string msgInvalidTipoNov1;
        private string msgInvalidTipoNov4;
        private string msgInvalidTipoNov5;
        private string msgInvalidTipoNovPagaduriasDif;
        private string msgLibranzaInvalida;
        private string msgLibranzaCancelada;
        private string msgLibranzaRepetida;

        //Variables de importacion
        private string novedadRsx = string.Empty;
        private string tipoNovedadRsx = string.Empty;
        private string centroPagoModificaRsx = string.Empty;
        private string format;
        private string formatSeparator = "\t";
        private PasteOpDTO pasteRet;

        #endregion

        #region Constructor

        public Reincorporacion()
            : base()
        {
            //this.InitializeComponent();
        }

        public Reincorporacion(string mod)
            : base(mod)
        {
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
            _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
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
            this.msgInvalidTipoNov = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov);
            this.msgInvalidTipoNov1 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov1);
            this.msgInvalidTipoNov4 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov4);
            this.msgInvalidTipoNov5 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov5);
            this.msgInvalidTipoNovPagaduriasDif = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNovPagaduriasDif);

            this.msgInvalidTipoNov1 = string.Format(this.msgInvalidTipoNov1, this.tiposNovedad[1].ToUpper());
            this.msgInvalidTipoNov4 = string.Format(this.msgInvalidTipoNov4, this.tiposNovedad[4].ToUpper());
            this.msgInvalidTipoNov5 = string.Format(this.msgInvalidTipoNov5, this.tiposNovedad[5].ToUpper());

            //Formato importar
            this.format = _bc.GetImportExportFormat(typeof(DTO_ccReincorporacionDeta), this.documentID);

            this.chkSeleccionar.Visible = false;

            base.SetInitParameters();
            this.gcDocuments.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Visible = true;
            this.gcDocuments.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDocuments_EmbeddedNavigator_ButtonClick);
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
        protected override void LoadDocuments()
        {
            try
            {
                if (this.periodo != this.dtPeriodo.DateTime)// || this.centroPagoID != this.masterCentroPago.Value)
                {
                    bool update = true;
                    if (this.reincorporaciones != null && this.reincorporaciones.Count > 0)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgUpdate = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UpdateData);
                        if (MessageBox.Show(msgUpdate, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                            update = false;
                    }

                    if (update)
                    {
                        this.reincorporaciones = new List<DTO_ccReincorporacionDeta>();
                        this.reincorporacionesFilter = new List<DTO_ccReincorporacionDeta>();

                        this.periodo = this.dtPeriodo.DateTime;
                        this.reincorporaciones = this._bc.AdministrationModel.Reincorporacion_GetForReincorporacion(this.periodo, string.Empty);//, this.centroPagoID);
                        this.reincorporacionesFilter = this.reincorporaciones;

                        int currentMonth = this.dtPeriodo.DateTime.Month;
                        int currentYear = this.dtPeriodo.DateTime.Year;
                        int minDay = 1;
                        int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                        this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                        this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                        this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);

                        this.reincorporacion = this.reincorporacionesFilter.FirstOrDefault();
                        if (this.reincorporacion != null)
                        {
                            //Carga el centro de pago
                            if (this.centrosPago.ContainsKey(this.reincorporacion.CentroPagoID.Value))
                                this.centroPago = this.centrosPago[this.reincorporacion.CentroPagoID.Value];
                            else
                            {
                                this.centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.reincorporacion.CentroPagoID.Value, true);
                                this.centrosPago[this.reincorporacion.CentroPagoID.Value] = this.centroPago;
                            }

                            //Carga la pagaduria
                            if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                                this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                            else
                            {
                                this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                            }
                        }

                        this.gcDocuments.DataSource = this.reincorporacionesFilter;
                        this.gvDocuments.RefreshData();

                        this.validateData = true;
                    }
                    else 
                    {
                        this.periodo = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 7;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 20;
                libranza.Visible = true;
                libranza.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 2;
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
                nombre.VisibleIndex = 3;
                nombre.Width = 70;
                nombre.Visible = true;
                nombre.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //CentroPagoID
                GridColumn cp = new GridColumn();
                cp.FieldName = this.unboundPrefix + "CentroPagoID";
                cp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroPagoID");
                cp.UnboundType = UnboundColumnType.String;
                cp.VisibleIndex = 4;
                cp.Width = 40;
                cp.Visible = false;
                cp.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                cp.OptionsColumn.AllowEdit = false;
                cp.ColumnEdit = this.editBtnGrid;
                this.gvDocuments.Columns.Add(cp);

                //CentroPagoDesc
                GridColumn cpDesc = new GridColumn();
                cpDesc.FieldName = this.unboundPrefix + "CentroPagoDesc";
                cpDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroPagoDesc");
                cpDesc.UnboundType = UnboundColumnType.String;
                cpDesc.VisibleIndex = 4;
                cpDesc.Width = 40;
                cpDesc.Visible = true;
                cpDesc.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                cpDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cpDesc);

                //Novedad
                GridColumn novedad = new GridColumn();
                novedad.FieldName = this.unboundPrefix + "NovedadIncorporaID";
                novedad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NovedadIncorporaID");
                novedad.UnboundType = UnboundColumnType.String;
                novedad.VisibleIndex = 5;
                novedad.Width = 40;
                novedad.Visible = true;
                novedad.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                novedad.OptionsColumn.AllowEdit = true;
                novedad.ColumnEdit = this.editBtnGrid;
                this.gvDocuments.Columns.Add(novedad);
                this.novedadRsx = novedad.Caption;

                //TipoNovedad
                GridColumn tipoNovedad = new GridColumn();
                tipoNovedad.FieldName = this.unboundPrefix + "TipoNovedad";
                tipoNovedad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoNovedad");
                tipoNovedad.UnboundType = UnboundColumnType.String;
                tipoNovedad.VisibleIndex = 6;
                tipoNovedad.Width = 40;
                tipoNovedad.Visible = true;
                tipoNovedad.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                tipoNovedad.OptionsColumn.AllowEdit = true;
                tipoNovedad.ColumnEdit = this.editCmb;
                this.gvDocuments.Columns.Add(tipoNovedad);
                this.tipoNovedadRsx = tipoNovedad.Caption;

                //CentroPagoModificaID
                GridColumn cpModifica = new GridColumn();
                cpModifica.FieldName = this.unboundPrefix + "CentroPagoModificaID";
                cpModifica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroPagoModificaID");
                cpModifica.UnboundType = UnboundColumnType.String;
                cpModifica.VisibleIndex = 7;
                cpModifica.Width = 30;
                cpModifica.Visible = true;
                cpModifica.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                cpModifica.OptionsColumn.AllowEdit = false;
                cpModifica.ColumnEdit = this.editBtnGrid;
                this.gvDocuments.Columns.Add(cpModifica);
                this.centroPagoModificaRsx = cpModifica.Caption;

                //CentroPagoModificaDesc
                GridColumn cpModificaDesc = new GridColumn();
                cpModificaDesc.FieldName = this.unboundPrefix + "CentroPagoModificaDesc";
                cpModificaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroPagoModificaDesc");
                cpModificaDesc.UnboundType = UnboundColumnType.String;
                cpModificaDesc.VisibleIndex = 8;
                cpModificaDesc.Width = 30;
                cpModificaDesc.Visible = true;
                cpModificaDesc.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                cpModificaDesc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cpModificaDesc);

                //CambioCentroPagoIND
                GridColumn cambioCentroPagoIND = new GridColumn();
                cambioCentroPagoIND.FieldName = this.unboundPrefix + "CambioCentroPagoIND";
                cambioCentroPagoIND.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CambioCentroPagoIND");
                cambioCentroPagoIND.UnboundType = UnboundColumnType.Boolean;
                cambioCentroPagoIND.VisibleIndex = 8;
                cambioCentroPagoIND.Width = 9;
                cambioCentroPagoIND.Visible = true;
                cambioCentroPagoIND.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                cambioCentroPagoIND.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cambioCentroPagoIND);

                //NumeroInc
                GridColumn numeroInc = new GridColumn();
                numeroInc.FieldName = this.unboundPrefix + "NumeroINC";
                numeroInc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumeroInc");
                numeroInc.UnboundType = UnboundColumnType.String;
                numeroInc.VisibleIndex = 10;
                numeroInc.Width = 20;
                numeroInc.Visible = true;
                numeroInc.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                numeroInc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(numeroInc);
                
                //Vlr Cuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "ValorCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorCuota");
                vlrCuota.UnboundType = UnboundColumnType.String;
                vlrCuota.VisibleIndex = 11;
                vlrCuota.Width = 40;
                vlrCuota.Visible = true;
                vlrCuota.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                vlrCuota.OptionsColumn.AllowEdit = true;
                vlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrCuota);

                //Estado de cruce
                GridColumn estadoCruce = new GridColumn();
                estadoCruce.FieldName = this.unboundPrefix + "EstadoCruce";
                estadoCruce.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoCruce");
                estadoCruce.UnboundType = UnboundColumnType.Decimal;
                estadoCruce.VisibleIndex = 12;
                estadoCruce.Width = 40;
                estadoCruce.Visible = true;
                estadoCruce.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                estadoCruce.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(estadoCruce);
            
                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.Width = 50;
                observacion.VisibleIndex = 13;
                observacion.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                observacion.Visible = false;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(observacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "AddDocumentCols"));
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

                this.reincorporacion = this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle];
                if (this.reincorporacion.Aprobado.Value.Value)
                {
                    #region Crédito

                    this.validRow = _bc.ValidGridCellValue(this.gvDocuments, this.unboundPrefix, fila, "Libranza", false, false, true, false);
                    if (this.validRow)
                    {
                        if(this.reincorporacion.Extra.Value.Value)// && this.reincorporacion.TipoNovedad.Value != (byte)TipoNovedad.Desincorpora)
                        {
                            if (this.reincorporacion.Libranza.Value == 0)
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, string.Format(this.msgLibranzaInvalida, this.reincorporacion.Libranza.Value.ToString()));
                                this.validRow = false;
                            }
                            else if (this.isCreditoCancelado && this.reincorporacion.TipoNovedad.Value != (byte)TipoNovedad.Desincorpora)
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, string.Format(this.msgLibranzaCancelada, this.reincorporacion.Libranza.Value.ToString(), this.tiposNovedad[6]));
                                this.validRow = false;
                            }
                            else 
                            {
                                this.gvDocuments.SetColumnError(colLibranza, string.Empty);
                            }
                        }

                        this.gvDocuments.SetColumnError(colLibranza, string.Empty);
                    }
                    #endregion
                    #region Novedad
                    if (!string.IsNullOrWhiteSpace(this.reincorporacion.NovedadIncorporaID.Value))
                    {
                        DTO_MasterBasic novedad = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccIncorporacionNovedad, false, this.reincorporacion.NovedadIncorporaID.Value, true);
                        if (novedad == null)
                        {
                            string msg = string.Format(this.msgFkNotFound, this.reincorporacion.NovedadIncorporaID.Value);
                            this.gvDocuments.SetColumnError(colNovedad, msg);
                            this.validRow = false;
                        }
                        else
                            this.gvDocuments.SetColumnError(colNovedad, string.Empty);
                    }
                    else
                        this.gvDocuments.SetColumnError(colNovedad, string.Empty);

                    #endregion
                    #region Valor Cuota
                    if (this.reincorporacion.ValorCuota.Value == 0)
                    {
                        string msg = string.Format(this.msgPositive, colValorCuota.Caption);
                        this.gvDocuments.SetColumnError(colValorCuota, msg);
                        this.validRow = false;
                    }
                    else
                    {
                        this.gvDocuments.SetColumnError(colValorCuota, string.Empty);
                    }
                    #endregion
                    #region CentroPago Modifica

                    //Revisa si debe obligar al cambio de CP
                    if (string.IsNullOrWhiteSpace(this.reincorporacion.CentroPagoModificaID.Value))
                    {
                        if (this.reincorporacion.CambioCentroPagoIND.Value.Value)
                        {
                            this.gvDocuments.SetColumnError(colCentroPagoModificaID, this.msgEmptyField);
                            this.validRow = false;
                        }
                        else
                        {
                            this.gvDocuments.SetColumnError(colCentroPagoModificaID, string.Empty);
                        }
                    }
                    else 
                    {
                        bool valid = _bc.ValidGridCell(this.gvDocuments, this.unboundPrefix, fila, "CentroPagoModificaID", true, true, false, AppMasters.ccCentroPagoPAG);
                        if (!valid)
                            this.validRow = false;
                    }
                    #endregion
                    #region Tipo Novedad

                    if (this.reincorporacion.TipoNovedad.Value == 0)
                    {
                        this.gvDocuments.SetColumnError(colTipoNovedad, this.msgEmptyField);
                        this.validRow = false;
                    }
                    else
                    {
                        if (this.centroPago.DigitoReincorporaIND.Value != null && this.centroPago.DigitoReincorporaIND.Value.Value)
                        {
                            if (this.reincorporacion.TipoNovedad.Value == (byte)TipoNovedad.NuevoDescuento)
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, this.msgInvalidTipoNov4);
                                this.validRow = false;
                            }
                            else
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, string.Empty);
                            }
                        }
                        else
                        {
                            if (this.reincorporacion.TipoNovedad.Value == (byte)TipoNovedad.AdicionDigito)
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, this.msgInvalidTipoNov1);
                                this.validRow = false;
                            }
                            else
                            {
                                this.gvDocuments.SetColumnError(colTipoNovedad, string.Empty);
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    this.gvDocuments.SetColumnError(colLibranza, string.Empty);
                    this.gvDocuments.SetColumnError(colNovedad, string.Empty);
                    this.gvDocuments.SetColumnError(colValorCuota, string.Empty);
                    this.gvDocuments.SetColumnError(colCentroPagoModificaID, string.Empty);
                    this.gvDocuments.SetColumnError(colTipoNovedad, string.Empty);
                }

                return this.validRow;
            }
            catch (Exception ex)
            {
                this.validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "ValidateDocRow"));
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
            this.centroPago = null;
            this.centrosPago = new Dictionary<string, DTO_ccCentroPagoPAG>();
            string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.cc_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodo);

            this.masterCentroPago.Value = string.Empty;
            this.masterNovedad.Value = string.Empty;
            this.txtLibranza.Text = string.Empty;
            this.lkp_EstadoCruce.EditValue = 0;
            this.lkp_Seleccion.EditValue = DictionaryTables.Todos;

            this.currentRow = -1;
            this.reincorporaciones = new List<DTO_ccReincorporacionDeta>();
            this.reincorporacionesFilter = new List<DTO_ccReincorporacionDeta>();
            this.gcDocuments.DataSource = this.reincorporacionesFilter;
            this.masterCentroPago.Value = String.Empty;

            this.validateData = true;
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow()
        {
            this.reincorporacion = new DTO_ccReincorporacionDeta();
            
            try
            {
                #region Asigna datos a la fila

                this.reincorporacion.Aprobado.Value = false;
                this.reincorporacion.Extra.Value = true;
                this.reincorporacion.Libranza.Value = 0;
                this.reincorporacion.ClienteID.Value = string.Empty;
                this.reincorporacion.Observacion.Value = string.Empty;
                this.reincorporacion.EstadoCruce.Value = 1;
                this.reincorporacion.TipoNovedad.Value = 0;
                this.reincorporacion.NumeroINC.Value = 0;
                this.reincorporacion.NumeroINCIni.Value = 1;
                this.reincorporacion.CambioCentroPagoIND.Value = false;

                #endregion

                //Centro de pago
                this.masterCentroPago.Value = string.Empty;
                this.masterNovedad.Value = string.Empty;
                this.txtLibranza.Text = string.Empty;
                this.lkp_EstadoCruce.EditValue = 1;

                this.reincorporaciones.Add(this.reincorporacion);
                this.reincorporacionesFilter = this.reincorporaciones;
                this.gcDocuments.DataSource = this.reincorporacionesFilter;
                this.gcDocuments.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DesIncorporacion.cs", "AddNewRow"));
            }
        }

        /// <summary>
        /// Carga la información de una reincorporación a partir de la libranza
        /// </summary>
        /// <param name="libranza"></param>
        /// <returns></returns>
        private void LoadReincorporacionFromLibranza(DTO_ccReincorporacionDeta reinc, bool updateGrilla)
        {

            try
            {
                //Valida el crédito
                this.isCreditoCancelado = false;
                DTO_ccCreditoDocu cred = _bc.AdministrationModel.GetCreditoByLibranza(reinc.Libranza.Value.Value);
                if (cred == null)
                {
                    this.CleanReincorporacion(reinc, updateGrilla);
                    return;
                }
                else if (cred.CanceladoInd.Value.Value && this.reincorporacion.TipoNovedad.Value != (byte)TipoNovedad.Desincorpora)
                {
                    this.CleanReincorporacion(reinc, updateGrilla);
                    this.isCreditoCancelado = true;
                    return;
                }

                //Trae la info de la reincorporación
                reinc.Libranza.Value = cred.Libranza.Value;
                reinc.NumDocCredito.Value = cred.NumeroDoc.Value;
                reinc.NumeroINC.Value = 0;
                reinc.NumeroINCIni.Value = 0;
                //reinc.EstadoCruce.Value = 0;
                reinc.ClienteID.Value = cred.ClienteID.Value;
                reinc.CentroPagoID.Value = cred.CentroPagoID.Value;
                reinc.VlrLibranza.Value = cred.VlrLibranza.Value;
                //reinc.VlrSaldo.Value = cred.Libranza.Value;
                reinc.ValorCuota.Value = cred.VlrCuota.Value;
                reinc.VlrCuotaCredito.Value = cred.VlrCuota.Value;
                reinc.FechaLiquida.Value = cred.FechaLiquida.Value;
                reinc.CobranzaEstadoID.Value = cred.CobranzaEstadoID.Value;
                reinc.CobranzaGestionID.Value = cred.CobranzaGestionID.Value;
                reinc.SiniestroEstadoID.Value = cred.SiniestroEstadoID.Value;
                reinc.NovedadIncorporaID.Value = cred.NovedadIncorporaID.Value;
                reinc.PlazoCredito.Value = cred.Plazo.Value;
                reinc.FechaCuota1.Value = cred.FechaCuota1.Value;

                //Info del cliente
                DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, reinc.ClienteID.Value, false);
                reinc.Nombre.Value = cliente.Descriptivo.Value;
                reinc.ProfesionID.Value = cliente.ProfesionID.Value;
                reinc.EmpleadoCodigo.Value = cliente.EmpleadoCodigo.Value;

                //Carga el centro de pago
                if (this.centrosPago.ContainsKey(this.reincorporacion.CentroPagoID.Value))
                    this.centroPago = this.centrosPago[this.reincorporacion.CentroPagoID.Value];
                else
                {
                    this.centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.reincorporacion.CentroPagoID.Value, true);
                    this.centrosPago[this.reincorporacion.CentroPagoID.Value] = this.centroPago;
                }
                reinc.CentroPagoDesc.Value = this.centroPago.Descriptivo.Value;

                //Carga la pagaduria
                if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                    this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                else
                {
                    this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                    this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                }

                //Trae el saldo
                List<DTO_ccCreditoPlanPagos> pp = _bc.AdministrationModel.GetPlanPagos(reinc.NumDocCredito.Value.Value);
                decimal sumVlrCuota = pp.Sum(x => x.VlrCuota.Value.Value);
                decimal sumVlrPagado = pp.Sum(x => x.VlrPagadoCuota.Value.Value);
                reinc.VlrSaldo.Value = sumVlrCuota - sumVlrPagado;

                //PlazoInc
                reinc.PlazoINC.Value = Convert.ToInt16(Math.Ceiling(reinc.VlrSaldo.Value.Value / reinc.VlrCuotaCredito.Value.Value));
                if(this.pagaduria.MaxmesesInc.Value.HasValue && reinc.PlazoINC.Value.Value > this.pagaduria.MaxmesesInc.Value.Value)
                    reinc.PlazoINC.Value = this.pagaduria.MaxmesesInc.Value;


                //Ultimo pago
                int numDocRC = 0;
                DTO_glDocumentoControl ctrlUltRecaudo = null;
                if(cred.DocUltRecaudo.Value.HasValue)
                    ctrlUltRecaudo = _bc.AdministrationModel.glDocumentoControl_GetByID(cred.DocUltRecaudo.Value.Value);

                DTO_glDocumentoControl ctrlUltNomina = null;
                if(cred.DocUltNomina.Value.HasValue)
                    ctrlUltNomina = _bc.AdministrationModel.glDocumentoControl_GetByID(cred.DocUltNomina.Value.Value);

                if(ctrlUltRecaudo != null)
                {
                    if(ctrlUltNomina != null)
                    {
                        if(ctrlUltRecaudo.FechaDoc.Value.Value > ctrlUltNomina.FechaDoc.Value.Value)
                        {
                            reinc.FechaDoc.Value = ctrlUltRecaudo.FechaDoc.Value;
                            reinc.TipoMvto.Value = "Rec Manual";
                            reinc.Valor.Value = ctrlUltRecaudo.Valor.Value;
                            numDocRC = ctrlUltRecaudo.NumeroDoc.Value.Value;
                        }
                        else
                        {
                            reinc.FechaDoc.Value = ctrlUltNomina.FechaDoc.Value;
                            reinc.TipoMvto.Value = "Mig Nómina";
                            reinc.Valor.Value = ctrlUltNomina.Valor.Value;
                            numDocRC = ctrlUltNomina.NumeroDoc.Value.Value;
                        }
                    }
                    else
                    {
                        reinc.FechaDoc.Value = ctrlUltRecaudo.FechaDoc.Value;
                        reinc.TipoMvto.Value = "Rec Manual";
                        reinc.Valor.Value = ctrlUltRecaudo.Valor.Value;
                        numDocRC = ctrlUltRecaudo.NumeroDoc.Value.Value;
                    }
                }
                else if (ctrlUltNomina != null)
                {
                    reinc.FechaDoc.Value = ctrlUltNomina.FechaDoc.Value;
                    reinc.TipoMvto.Value = "Mig Nómina";
                    reinc.Valor.Value = ctrlUltNomina.Valor.Value;
                    numDocRC = ctrlUltNomina.NumeroDoc.Value.Value;
                }

              
                //Actualiza la grilla
                if (updateGrilla)
                {
                    this.UpdateGrillInfo(reinc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "CleanReincorporacion"));
            }
        }

        /// <summary>
        /// Carga la información de una reincorporación a partir de la libranza
        /// </summary>
        /// <param name="libranza"></param>
        /// <returns></returns>
        private void CleanReincorporacion(DTO_ccReincorporacionDeta reinc, bool updateGrilla)
        {
            try
            {
                //Cambios en la reincorporacion
                reinc.Libranza.Value = 0;
                reinc.NumDocCredito.Value = 0;
                reinc.NumeroINC.Value = 0;
                reinc.NumeroINCIni.Value = 0;
                reinc.PeriodoNomina.Value = null;
                reinc.EstadoCruce.Value = 0;
                reinc.ClienteID.Value = string.Empty;
                reinc.Nombre.Value = string.Empty;
                reinc.ProfesionID.Value = string.Empty;
                reinc.CentroPagoID.Value = string.Empty;
                reinc.CentroPagoDesc.Value = string.Empty;
                reinc.EmpleadoCodigo.Value = string.Empty;
                reinc.VlrLibranza.Value = 0; ;
                reinc.VlrSaldo.Value = 0;
                //reinc.ValorCuota.Value = 0;
                reinc.VlrCuotaCredito.Value = 0;
                reinc.FechaLiquida.Value = null;
                reinc.CobranzaEstadoID.Value = string.Empty;
                reinc.CobranzaGestionID.Value = string.Empty;
                reinc.SiniestroEstadoID.Value = string.Empty;
                reinc.NovedadIncorporaID.Value = string.Empty;
                reinc.PlazoCredito.Value = 0;
                reinc.PlazoINC.Value = 0;
                reinc.FechaCuota1.Value = null;
                reinc.FechaDoc.Value = null;
                reinc.TipoMvto.Value = string.Empty;
                reinc.FechaNomina.Value = null;
                reinc.Valor.Value = 0;

                //Actualiza la grilla
                if(updateGrilla)
                {
                    this.UpdateGrillInfo(reinc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "CleanReincorporacion"));
            }
        }

        /// <summary>
        /// Actualiza el registro actual de la grilla con la información de una reincorporación
        /// </summary>
        /// <param name="reinc"></param>
        private void UpdateGrillInfo(DTO_ccReincorporacionDeta reinc) 
        {
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].Libranza.Value = reinc.Libranza.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].NumDocCredito.Value = reinc.NumDocCredito.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].NumeroINC.Value = reinc.NumeroINC.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].NumeroINCIni.Value = reinc.NumeroINCIni.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PeriodoNomina.Value = reinc.PeriodoNomina.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].EstadoCruce.Value = reinc.EstadoCruce.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].ClienteID.Value = reinc.ClienteID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].Nombre.Value = reinc.Nombre.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].ProfesionID.Value = reinc.ProfesionID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CentroPagoID.Value = reinc.CentroPagoID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CentroPagoDesc.Value = reinc.CentroPagoDesc.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].EmpleadoCodigo.Value = reinc.EmpleadoCodigo.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].VlrLibranza.Value = reinc.VlrLibranza.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].VlrSaldo.Value = reinc.VlrSaldo.Value;
            //this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].ValorCuota.Value = 0;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].VlrCuotaCredito.Value = reinc.VlrCuotaCredito.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].FechaLiquida.Value = reinc.FechaLiquida.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CobranzaEstadoID.Value = reinc.CobranzaEstadoID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CobranzaGestionID.Value = reinc.CobranzaGestionID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].SiniestroEstadoID.Value = reinc.SiniestroEstadoID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].NovedadIncorporaID.Value = reinc.NovedadIncorporaID.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoCredito.Value = reinc.PlazoCredito.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoINC.Value = reinc.PlazoINC.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].FechaCuota1.Value = reinc.FechaCuota1.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].FechaDoc.Value = reinc.FechaDoc.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].TipoMvto.Value = reinc.TipoMvto.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].FechaNomina.Value = reinc.FechaNomina.Value;
            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].Valor.Value = reinc.Valor.Value;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Evento Header

        /// <summary>
        /// Evento al cambiar el periodo
        /// </summary>
        private void dtPeriodo_ValueChanged()
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Filters current data
        /// </summary>
        private void FilterData(object sender, EventArgs e)
        {
            try
            {
                if (this.reincorporaciones != null && this.reincorporaciones.Count > 0 && this.validRow)
                {
                    this.validateData = false;
                    this.reincorporacionesFilter = this.reincorporaciones;

                    //Selección
                    if (this.lkp_Seleccion.EditValue != DictionaryTables.Todos)
                    {
                        if (this.lkp_EstadoCruce.EditValue == DictionaryTables.Seleccionados)
                            this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.Aprobado.Value == true).ToList();
                        else
                            this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.Aprobado.Value == false).ToList();
                    }

                    //Centro de pago
                    if (!string.IsNullOrWhiteSpace(this.masterCentroPago.Value))
                    {
                        this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.CentroPagoID.Value == this.masterCentroPago.Value).ToList();
                    }
                    
                    //Novedad
                    if (!string.IsNullOrWhiteSpace(this.masterNovedad.Value))
                        this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.NovedadIncorporaID.Value == this.masterNovedad.Value).ToList();

                    //Libranza
                    if (!string.IsNullOrWhiteSpace(this.txtLibranza.Text))
                        this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.Libranza.Value == Convert.ToInt32(this.txtLibranza.Text)).ToList();

                    //Estado de cruce
                    if (Convert.ToInt32(this.lkp_EstadoCruce.EditValue) != 0)
                        this.reincorporacionesFilter = this.reincorporacionesFilter.Where(r => r.EstadoCruce.Value == Convert.ToInt32(this.lkp_EstadoCruce.EditValue)).ToList();

                    this.reincorporacion = this.reincorporacionesFilter.FirstOrDefault();

                    //Carga el centro de pago
                    if (this.centrosPago.ContainsKey(this.reincorporacion.CentroPagoID.Value))
                        this.centroPago = this.centrosPago[this.reincorporacion.CentroPagoID.Value];
                    else
                    {
                        this.centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.reincorporacion.CentroPagoID.Value, true);
                        this.centrosPago[this.reincorporacion.CentroPagoID.Value] = this.centroPago;
                    }

                    //Carga la pagaduria
                    if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                        this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                    else
                    {
                        this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                        this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                    }

                    

                    this.gcDocuments.DataSource = this.reincorporacionesFilter;
                    this.gvDocuments.RefreshData();
                    this.validateData = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "FilterData"));
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
                if (fieldName == "EstadoCruce")
                {
                    DTO_ccReincorporacionDeta dto = (DTO_ccReincorporacionDeta)obj;
                    e.Value = this.estadosCruce[dto.EstadoCruce.Value.Value];
                }
                else
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
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocuments_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;
                if (this.validateData)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.ValidateDocRow(fila);
                        if (this.validRow)
                        {
                            this.AddNewRow();
                            this.gvDocuments.FocusedRowHandle = this.reincorporacionesFilter.Count - 1;
                        }
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        if (fila >= 0 && this.reincorporacionesFilter[fila].Extra.Value.Value)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.validRow = true;
                                this.reincorporacionesFilter.RemoveAt(fila);
                                this.gcDocuments.RefreshDataSource();

                                if (fila == 0)
                                    this.gvDocuments.FocusedRowHandle = 0;
                                else
                                    this.gvDocuments.FocusedRowHandle = fila - 1;
                            }

                            e.Handled = true;
                        }
                        else
                            e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "gcDocuments_EmbeddedNavigator_ButtonClick"));
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
            if (this.validateData && this.currentRow >= 0 && this.reincorporacionesFilter.Count > 0)
            {
                this.reincorporacion = this.reincorporacionesFilter[this.currentRow];

                //Libranza
                if (this.reincorporacionesFilter[this.currentRow].Extra.Value.Value)
                    gvDocuments.Columns[1].OptionsColumn.AllowEdit = true;
                else
                    gvDocuments.Columns[1].OptionsColumn.AllowEdit = false;

                //Carga el centro de pago
                if(this.centrosPago.ContainsKey(this.reincorporacion.CentroPagoID.Value))
                    this.centroPago = this.centrosPago[this.reincorporacion.CentroPagoID.Value];
                else 
                {
                    this.centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.reincorporacion.CentroPagoID.Value, true);
                    this.centrosPago[this.reincorporacion.CentroPagoID.Value] = this.centroPago;
                }

                //Carga la pagaduria
                if (this.centroPago != null)
                {
                    if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                        this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                    else
                    {
                        this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                        this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                    }
                }
            }
            else
            {
                this.currentRow = -1;
                this.reincorporacion = this.reincorporacionesFilter != null ? this.reincorporacionesFilter.FirstOrDefault() : null;
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
                        this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].NovedadIncorporaID.Value = string.Empty;
                        this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].TipoNovedad.Value = 0;
                        this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CentroPagoModificaID.Value = string.Empty;
                        this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CentroPagoModificaDesc.Value = string.Empty;
                        this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].CambioCentroPagoIND.Value = false;

                        this.gvDocuments.RefreshData();
                    }
                    else
                    {
                        if (this.pagaduria.MaxmesesInc.Value.HasValue &&
                            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoINC.Value.Value > this.pagaduria.MaxmesesInc.Value.Value)
                        {
                            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoINC.Value = this.pagaduria.MaxmesesInc.Value;
                        }
                        else
                        {
                            this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoINC.Value = this.reincorporacionesFilter[this.gvDocuments.FocusedRowHandle].PlazoCredito.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "gvDocuments_CellValueChanging"));
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

                //Libranza
                if (fieldName == "Libranza")
                {
                    if (!this.reincorporacion.Libranza.Value.HasValue)
                        this.CleanReincorporacion(this.reincorporacion, true);
                    else
                    {
                        this.LoadReincorporacionFromLibranza(this.reincorporacion, true);
                    }
                }

                //Valor Cuota
                if (fieldName == "ValorCuota")
                {
                    if (!this.reincorporacion.ValorCuota.Value.HasValue)
                    {
                        this.reincorporacion.ValorCuota.Value = 0;
                        this.reincorporacionesFilter[e.RowHandle].ValorCuota.Value = 0;
                    }
                }

                //Tipo Novedad
                if (fieldName == "TipoNovedad")
                {
                    byte tipoNov = Convert.ToByte(e.Value);
                    if (tipoNov == (byte)TipoNovedad.AdicionDigito)
                    {
                        this.reincorporacionesFilter[e.RowHandle].NumeroINC.Value = this.reincorporacionesFilter[e.RowHandle].NumeroINCIni.Value;
                    }
                    else
                    {
                        this.reincorporacionesFilter[e.RowHandle].NumeroINC.Value = 0;
                    }
                    
                    if (tipoNov == (byte)TipoNovedad.NuevoDescuento)
                    {
                        this.reincorporacionesFilter[e.RowHandle].CambioCentroPagoIND.Value = false;
                        this.gvDocuments.Columns[unboundPrefix + "CambioCentroPagoIND"].OptionsColumn.AllowEdit = true;
                        this.gvDocuments.Columns[unboundPrefix + "CentroPagoModificaID"].OptionsColumn.AllowEdit = true;
                    }
                    else if (tipoNov == (byte)TipoNovedad.CambioPagaduria)
                    {
                        this.reincorporacionesFilter[e.RowHandle].CambioCentroPagoIND.Value = true;
                        this.gvDocuments.Columns[unboundPrefix + "CambioCentroPagoIND"].OptionsColumn.AllowEdit = false;
                        this.gvDocuments.Columns[unboundPrefix + "CentroPagoModificaID"].OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        this.reincorporacionesFilter[e.RowHandle].CambioCentroPagoIND.Value = false;
                        this.reincorporacionesFilter[e.RowHandle].CentroPagoModificaID.Value = string.Empty;
                        this.reincorporacionesFilter[e.RowHandle].CentroPagoModificaDesc.Value = string.Empty;
                        this.gvDocuments.Columns[unboundPrefix + "CambioCentroPagoIND"].OptionsColumn.AllowEdit = false;
                        this.gvDocuments.Columns[unboundPrefix + "CentroPagoModificaID"].OptionsColumn.AllowEdit = false;
                    }
                }

                //Centro Pago Modifica
                if (fieldName == "CentroPagoModificaID")
                {
                    if(string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        this.reincorporacionesFilter[e.RowHandle].CentroPagoModificaDesc.Value = string.Empty;
                    }
                    else 
                    {
                        DTO_ccCentroPagoPAG cpModifica = null;
                        if (this.centrosPago.ContainsKey(e.Value.ToString()))
                            cpModifica = this.centrosPago[e.Value.ToString()];
                        else
                        {
                            cpModifica = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, e.Value.ToString(), true);
                            this.centrosPago[e.Value.ToString()] = cpModifica;
                        }

                        if(cpModifica == null)
                            this.reincorporacionesFilter[e.RowHandle].CentroPagoModificaDesc.Value = string.Empty;
                        else
                            this.reincorporacionesFilter[e.RowHandle].CentroPagoModificaDesc.Value = cpModifica.Descriptivo.Value;
                    }
                }

                this.gvDocuments.RefreshData();
                this.ValidateDocRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "gvDocuments_CellValueChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "editBtnGrid_ButtonClick"));
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
                if (this.reincorporaciones != null && this.reincorporaciones.Count > 0 && this.validRow)
                {
                    List<DTO_ccReincorporacionDeta> temp = this.reincorporaciones.Where(i => i.Aprobado.Value.Value).ToList();
                    if (temp.Count > 0)
                    {
                        Thread process = new Thread(this.ApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                bool update = true;                
                if (this.reincorporaciones != null && this.reincorporaciones.Count > 0)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgUpdate = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UpdateData);
                    if (MessageBox.Show(msgUpdate, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                        update = false;
                }

                if (update)
                {
                    this.CleanData();
                    this.LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "TBUpdate"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar los datos de la plantilla
        /// </summary>
        public override void TBImport()
        {
            try
            {
                if (this.reincorporaciones.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportThread);
                        process.Start();
                    }
                }
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
                if (this.reincorporaciones.Count > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    DataTable tableExport = new DataTable();
                    tableExport.Columns.Add(this.lblPeriodo.Text);
                    tableExport.Columns.Add("Libranza");
                    tableExport.Columns.Add("ClienteID");
                    tableExport.Columns.Add("Nombre");
                    tableExport.Columns.Add("ProfesionID");
                    tableExport.Columns.Add("CentroPagoID");
                    tableExport.Columns.Add("CentroPagoDesc");
                    tableExport.Columns.Add("EmpleadoCodigo");
                    tableExport.Columns.Add("VlrLibranza");
                    tableExport.Columns.Add("VlrSaldo");
                    tableExport.Columns.Add("VlrCuota");
                    tableExport.Columns.Add("ValorCuota");
                    tableExport.Columns.Add("FechaLiquida");
                    tableExport.Columns.Add("CobranzaEstadoID");
                    tableExport.Columns.Add("CobranzaGestionID");
                    tableExport.Columns.Add("SiniestroEstadoID");
                    tableExport.Columns.Add("NovedadIncorporaID");
                    tableExport.Columns.Add("FechaDoc");
                    tableExport.Columns.Add("TipoMvto");
                    tableExport.Columns.Add("FechaNomina");
                    tableExport.Columns.Add("Valor");
                    tableExport.Columns.Add("EstadoCruce");

                    foreach(DTO_ccReincorporacionDeta reinc in this.reincorporacionesFilter)
                    {
                        string[] cols = 
                        {
                            this.dtPeriodo.DateTime.ToString(FormatString.Period),
                            reinc.Libranza.Value.ToString(),
                            reinc.ClienteID.Value,
                            reinc.Nombre.Value,
                            reinc.ProfesionID.Value,
                            reinc.CentroPagoID.Value,
                            reinc.CentroPagoDesc.Value,
                            reinc.EmpleadoCodigo.Value,
                            reinc.VlrLibranza.Value != null? reinc.VlrLibranza.Value.Value.ToString("n0"): "0",
                            reinc.VlrSaldo.Value != null? reinc.VlrSaldo.Value.Value.ToString("n0"): "0",
                            reinc.VlrCuotaCredito.Value != null? reinc.VlrCuotaCredito.Value.Value.ToString("n0"): "0",
                            reinc.ValorCuota.Value != null? reinc.ValorCuota.Value.Value.ToString("n0"): "0", 
                            reinc.FechaLiquida.Value != null? reinc.FechaLiquida.Value.Value.ToString("dd/MM/yyyy"): reinc.FechaLiquida.Value.ToString(),
                            reinc.CobranzaEstadoID.Value,
                            reinc.CobranzaGestionID.Value,
                            reinc.SiniestroEstadoID.Value,
                            reinc.NovedadIncorporaID.Value,
                            reinc.FechaDoc.Value.ToString(),
                            reinc.TipoMvto.Value,
                            reinc.FechaNomina.Value != null? reinc.FechaNomina.Value.Value.ToString("dd/MM/yyyy") : reinc.FechaNomina.Value.ToString(),
                            reinc.Valor.Value != null? reinc.Valor.Value.Value.ToString("n0") : "0", 
                            this.estadosCruce[reinc.EstadoCruce.Value.Value].ToString()
                        };
                        tableExport.Rows.Add(cols);
                    }

                    ReportExcelBase frm = new ReportExcelBase(tableExport, this.documentID, false);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "TBExport"));
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

                    List<DTO_ccReincorporacionDeta> list = new List<DTO_ccReincorporacionDeta>();
                    List<DTO_ccReincorporacionDeta> finalList = new List<DTO_ccReincorporacionDeta>();
                    List<DTO_ccReincorporacionDeta> reincorporacionesTemp = ObjectCopier.Clone(this.reincorporaciones);

                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_ccReincorporacionDeta).GetProperties();

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
                            DTO_ccReincorporacionDeta det = new DTO_ccReincorporacionDeta();
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

                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        #region Novedades
                                        if (colRsx == this.novedadRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();
                                            if (novedades.ContainsKey(line[colIndex].Trim()))
                                            {
                                                if (novedades[line[colIndex].Trim()])
                                                    continue;
                                                else
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            else
                                            {
                                                DTO_MasterBasic novedad = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccIncorporacionNovedad, false, line[colIndex], true);

                                                if (novedad != null)
                                                {
                                                    novedades[colRsx] = true;
                                                }
                                                else
                                                {
                                                    novedades[colRsx] = false;

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Centros de pago
                                        if (colRsx == this.centroPagoModificaRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();
                                            if (centrosPago.ContainsKey(line[colIndex].Trim()))
                                            {
                                                if (centrosPago[line[colIndex].Trim()])
                                                    continue;
                                                else
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            else
                                            {
                                                DTO_MasterBasic cp = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, line[colIndex], true);
                                                if (cp != null)
                                                {
                                                    centrosPago[colRsx] = true;
                                                }
                                                else
                                                {
                                                    centrosPago[colRsx] = false;

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion // Carga la info de las fks
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp-Reincorporacion.cs", "ImportThread - Creacion de DTO y validacion Formatos");
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
                        foreach (DTO_ccReincorporacionDeta reInc in list)
                        {
                            bool isValid = true;
                            if (reInc.CambioCentroPagoIND.Value == null)
                                reInc.CambioCentroPagoIND.Value = false;

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
                            #region Valida que las libranzas existan

                            DTO_ccReincorporacionDeta newReinc = null;
                            if (reincorporacionesTemp.Any(c => c.Libranza.Value == reInc.Libranza.Value))
                            {
                                #region Libranza existente
                                var aux = reincorporacionesTemp.FirstOrDefault(c => c.Libranza.Value == reInc.Libranza.Value);

                                newReinc = ObjectCopier.Clone(aux);
                                newReinc.Aprobado.Value = true;
                                newReinc.NovedadIncorporaID.Value = reInc.NovedadIncorporaID.Value;
                                
                                //Valor de la cuota
                                if (reInc.ValorCuota.Value.HasValue)
                                    newReinc.ValorCuota.Value = reInc.ValorCuota.Value;
                               
                                //Indicador para modificar el CP
                                newReinc.CambioCentroPagoIND.Value = reInc.CambioCentroPagoIND.Value;
                                
                                //Tipo de novedad
                                newReinc.TipoNovedad.Value = reInc.TipoNovedad.Value;

                                //Carga el centro de pago
                                if (this.centrosPago.ContainsKey(aux.CentroPagoID.Value))
                                    this.centroPago = this.centrosPago[aux.CentroPagoID.Value];
                                else
                                {
                                    this.centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, aux.CentroPagoID.Value, true);
                                    this.centrosPago[aux.CentroPagoID.Value] = this.centroPago;
                                }

                                //Valida el centro de pago
                                if (!string.IsNullOrWhiteSpace(reInc.CentroPagoModificaID.Value))
                                {
                                    DTO_ccCentroPagoPAG cpModifica = null;
                                    if (this.centrosPago.ContainsKey(aux.CentroPagoModificaID.Value))
                                        cpModifica = this.centrosPago[aux.CentroPagoModificaID.Value];
                                    else
                                    {
                                        cpModifica = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, aux.CentroPagoModificaID.Value, true);
                                        this.centrosPago[aux.CentroPagoModificaID.Value] = cpModifica;
                                    }

                                    newReinc.CentroPagoModificaID.Value = reInc.CentroPagoModificaID.Value;
                                    newReinc.CentroPagoModificaDesc.Value = cpModifica.Descriptivo.Value;
                                }

                                //Carga la pagaduria
                                if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                                    this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                                else
                                {
                                    this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                    this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                                }

                                //Carga el plazo
                                if (this.pagaduria.MaxmesesInc.Value.HasValue && newReinc.PlazoINC.Value.Value > this.pagaduria.MaxmesesInc.Value.Value)
                                {
                                    newReinc.PlazoINC.Value = this.pagaduria.MaxmesesInc.Value;
                                }
                                else
                                {
                                    newReinc.PlazoINC.Value = newReinc.PlazoCredito.Value;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Nuevos créditos

                                newReinc = new DTO_ccReincorporacionDeta();
                                newReinc.Libranza.Value = reInc.Libranza.Value;
                                this.LoadReincorporacionFromLibranza(newReinc, false);

                                if (newReinc.Libranza.Value == 0)
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = string.Format(this.msgLibranzaInvalida, newReinc.Libranza.Value.ToString());
                                    result.Details.Add(rd);
                                    validList = false;
                                }
                                else if (this.isCreditoCancelado && newReinc.TipoNovedad.Value != (byte)TipoNovedad.Desincorpora)
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = string.Format(this.msgLibranzaCancelada, newReinc.Libranza.Value.ToString(), this.tiposNovedad[6]);
                                    result.Details.Add(rd);
                                    validList = false;
                                }
                                else
                                {
                                    newReinc.Aprobado.Value = true;
                                    newReinc.Extra.Value = true;
                                    newReinc.PeriodoNomina.Value = this.dtFecha.DateTime.Date;
                                    newReinc.NumeroINC.Value = 0;
                                    newReinc.NumeroINCIni.Value = 1;
                                    newReinc.EstadoCruce.Value = 1;
                                    newReinc.Observacion.Value = string.Empty;

                                    //Novedad
                                    newReinc.NovedadIncorporaID.Value = reInc.NovedadIncorporaID.Value;

                                    //Valor de la cuota
                                    if (reInc.ValorCuota.Value.HasValue)
                                        newReinc.ValorCuota.Value = reInc.ValorCuota.Value;
                               
                                    //Indicador para modificar el CP
                                    newReinc.CambioCentroPagoIND.Value = reInc.CambioCentroPagoIND.Value;
                                
                                    //Tipo de novedad
                                    newReinc.TipoNovedad.Value = reInc.TipoNovedad.Value;

                                    //Valida el centro de pago
                                    if (!string.IsNullOrWhiteSpace(reInc.CentroPagoModificaID.Value))
                                    {
                                        DTO_ccCentroPagoPAG cpModifica = null;
                                        if (this.centrosPago.ContainsKey(newReinc.CentroPagoModificaID.Value))
                                            cpModifica = this.centrosPago[newReinc.CentroPagoModificaID.Value];
                                        else
                                        {
                                            cpModifica = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, newReinc.CentroPagoModificaID.Value, true);
                                            this.centrosPago[newReinc.CentroPagoModificaID.Value] = cpModifica;
                                        }

                                        newReinc.CentroPagoModificaID.Value = reInc.CentroPagoModificaID.Value;
                                        newReinc.CentroPagoModificaDesc.Value = cpModifica.Descriptivo.Value;
                                    }

                                    //Carga la pagaduria
                                    if (this.pagadurias.ContainsKey(this.centroPago.PagaduriaID.Value))
                                        this.pagaduria = this.pagadurias[this.centroPago.PagaduriaID.Value];
                                    else
                                    {
                                        this.pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                        this.pagadurias[this.centroPago.PagaduriaID.Value] = this.pagaduria;
                                    }

                                    //Carga el plazo
                                    if (this.pagaduria.MaxmesesInc.Value.HasValue && newReinc.PlazoINC.Value.Value > this.pagaduria.MaxmesesInc.Value.Value)
                                    {
                                        newReinc.PlazoINC.Value = this.pagaduria.MaxmesesInc.Value;
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            #region Valida el tipo de novedad

                            if (isValid)
                            {
                                if (reInc.TipoNovedad.Value == 0 || reInc.TipoNovedad.Value.Value > 6)
                                {
                                    #region Valida que sea valido
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = this.tipoNovedadRsx + " " + this.msgInvalidTipoNov;
                                    result.Details.Add(rd);
                                    validList = false;
                                    #endregion
                                }
                                else
                                {
                                    #region Valida el centro de pago con el tipo de novedad

                                    if (this.centroPago.DigitoReincorporaIND.Value != null && this.centroPago.DigitoReincorporaIND.Value.Value)
                                    {
                                        if (reInc.TipoNovedad.Value == (byte)TipoNovedad.NuevoDescuento)
                                        {
                                            isValid = false;
                                            result.Result = ResultValue.NOK;
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.line = i;
                                            rd.Message = this.tipoNovedadRsx + " - " + this.msgInvalidTipoNov4;
                                            result.Details.Add(rd);
                                            validList = false;
                                        }
                                    }
                                    else
                                    {
                                        if (reInc.TipoNovedad.Value == (byte)TipoNovedad.AdicionDigito)
                                        {
                                            isValid = false;
                                            result.Result = ResultValue.NOK;
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.line = i;
                                            rd.Message = this.tipoNovedadRsx + " - " + this.msgInvalidTipoNov1;
                                            result.Details.Add(rd);
                                            validList = false;
                                        }
                                    }

                                    if (isValid)
                                    {
                                        if (reInc.TipoNovedad.Value == (byte)TipoNovedad.AdicionDigito)
                                            reInc.NumeroINC.Value = reInc.NumeroINCIni.Value;
                                        else
                                            reInc.NumeroINC.Value = 0;
                                    }

                                    #endregion
                                }
                            }

                            if (isValid && reInc.TipoNovedad.Value == 5 && !reInc.CambioCentroPagoIND.Value.Value)
                            {
                                isValid = false;
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = i;
                                rd.Message = this.tipoNovedadRsx + " " + this.msgInvalidTipoNovPagaduriasDif;
                                result.Details.Add(rd);
                                validList = false;

                            }

                            #endregion
                            #region Valida el indicador de cambio de CP

                            if (isValid && reInc.CambioCentroPagoIND.Value.Value && string.IsNullOrWhiteSpace(reInc.CentroPagoModificaID.Value))
                            {
                                isValid = false;
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.line = i;
                                rd.Message = this.centroPagoModificaRsx + " " + this.msgEmptyField;
                                result.Details.Add(rd);
                                validList = false;
                            }

                            #endregion
                            #region Valida que no se repita un crédito y centro de pago

                            if(isValid)
                            {
                                Tuple<int, string> tuple = new Tuple<int, string>(reInc.Libranza.Value.Value, reInc.CentroPagoModificaID.Value);
                                if (pkAdded.ContainsKey(tuple))
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = string.Format(msgPkAdded, reInc.Libranza.Value.ToString(), reInc.CentroPagoModificaID.Value, pkAdded[tuple]);
                                    result.Details.Add(rd);
                                    validList = false;
                                }
                                else
                                {
                                    pkAdded.Add(tuple, i);
                                }
                            }

                            #endregion
                            #region Valida que un crédito solo pueda tener un centro de pago para modificar

                            if (isValid && reInc.CambioCentroPagoIND.Value.Value)
                            {
                                if (cpModificaIndex.ContainsKey(reInc.Libranza.Value.Value))
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = string.Format(msgMultMod, reInc.Libranza.Value.ToString(), cpModificaIndex[reInc.Libranza.Value.Value]);
                                    result.Details.Add(rd);
                                    validList = false;
                                }
                                else
                                {
                                    cpModificaIndex.Add(reInc.Libranza.Value.Value, i);
                                }
                            }

                            #endregion
                            
                            if(isValid)
                            {
                                finalList.Add(newReinc);
                            }
                        }
                    }

                    #endregion

                    if (validList)
                    {
                        this.reincorporaciones.RemoveAll(cr => cr.Extra.Value.Value);
                        List<int> libs = finalList.Select(l => l.Libranza.Value.Value).Distinct().OrderByDescending(l1 => l1).ToList();
                        List<DTO_ccReincorporacionDeta> newList = new List<DTO_ccReincorporacionDeta>();
                        libs.ForEach(l =>
                        {
                            this.reincorporaciones.RemoveAll(r => r.Libranza.Value == l);

                            var listLib = finalList.Where(fl => fl.Libranza.Value == l);
                            newList.AddRange(listLib);
                        });

                        this.reincorporaciones.InsertRange(0, newList);
                        this.Invoke(this.refreshGridDelegate);
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "ImportThread"));
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

                List<DTO_ccReincorporacionDeta> tmp = ObjectCopier.Clone(this.reincorporaciones);
                tmp.Where(t => t.TipoNovedad.Value == 0).ToList().ForEach(r => r.TipoNovedad.Value = null);
                DTO_TxResult result = _bc.AdministrationModel.Reincorporacion_Aprobar(this.documentID, this.actividadFlujoID, this.periodo, this.dtFecha.DateTime, tmp);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (result.Result == ResultValue.OK)
                    this.Invoke(this.refreshData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reincorporacion.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
