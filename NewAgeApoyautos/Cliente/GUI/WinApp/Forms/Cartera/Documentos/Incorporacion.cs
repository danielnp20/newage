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
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Diagnostics;
using SentenceTransformer;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Incorporacion : SolicitudCreditoChequeo
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.currentRow = -1;
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.solicitudes = new List<DTO_ccSolicitudDocu>();
            this.comboTipoIncorp.SelectedIndex = -1;

            this.firstTime = true;
            this.LoadDocuments();
            this.FilterData();
        }

        public delegate void RefreshGrid();
        public RefreshGrid refreshGridDelegate;
        public virtual void RefreshGridMethod()
        {
            this.validateData = false;
            if (this.isCredito)
            {
                this.gcDocuments.DataSource = this.creditos;
                this.credito = this.creditos[0];
            }
            else
            {
                this.gcDocuments.DataSource = this.solicitudes;
                this.solicitud = this.solicitudes[0];
            }

            this.gvDocuments.RefreshData();
            this.validateData = false;
        }

        #endregion

        public Incorporacion()
            : base()
        {
            //InitializeComponent();
        }

        public Incorporacion(string mod)
            : base(mod)
        {
        }

        #region Variables Formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Variables control
        string pagaduiaISS = string.Empty;
        string pagaduriaPonal = string.Empty;
        string pagaduriaCagenPonal = string.Empty;
        string pagaduriaCasurPonal = string.Empty;
        string pagaduriaCremilPonal = string.Empty;
        string pagaduriaCremilEjercito = string.Empty;

        //DTO's
        private List<DTO_ccSolicitudDocu> solicitudes = new List<DTO_ccSolicitudDocu>();
        private List<DTO_ccSolicitudDocu> filterSolicitudes = new List<DTO_ccSolicitudDocu>();
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> filterCreditos = new List<DTO_ccCreditoDocu>();

        //Variables Privadas
        private bool canEdit = false;
        private bool validateData = false;
        private bool validCreditos = true;
        private bool isCredito = false;
        private DTO_ccSolicitudDocu solicitud;
        private DTO_ccCreditoDocu credito;
        private int diaTope;
        private bool recaudoMes;
        private string pagaduriaID = string.Empty;
        private string centroPagoID = string.Empty;
        private bool firstTime = true;
        private List<DTO_glConsultaFiltro> filtrosPagadurias = new List<DTO_glConsultaFiltro>();
        private Dictionary<int, string> tiposNovedad = new Dictionary<int, string>();
        private Dictionary<string, DTO_ccCentroPagoPAG> centrosPago = new Dictionary<string, DTO_ccCentroPagoPAG>();
        private Dictionary<string, DTO_ccPagaduria> pagadurias = new Dictionary<string, DTO_ccPagaduria>();
        private DTO_ccCentroPagoPAG centroPago;
        private DTO_ccPagaduria pagaduria;

        //Mensajes
        private string msgEmptyField;
        private string msgFkNotFound;
        private string msgPositive;
        private string msgInvalidCP;
        private string msgInvalidTipoNov;
        private string msgInvalidTipoNov1;
        private string msgInvalidTipoNov5;
        private string msgInvalidTipoNovPagaduriasDif;
        private string msgLibranzaInvalida;
        private string msgLibranzaCancelada;
        private string msgLibranzaRepetida;
        private string msgCredExiste;

        //Rsx
        private string libranzaRsx;
        private string centroPagoRsx;
        private string novedadRsx;
        private string tipoNovedadRsx;
        private string vlrCuotaRsx;

        private string format;
        private string formatSeparator = "\t";
        private PasteOpDTO pasteRet;

        //Temp
        private bool loadDocumentsFromDate = false;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        ///  Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Incorporacion;
            this.frmModule = ModulesPrefix.cc;

            //Carga los recursos del combo
            TablesResources.GetTableResources(this.comboTipoIncorp, typeof(TipoIncorporaCartera));

            //Variables de control
            string pagaduiaISS = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionISS);
            string pagaduriaPonal = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionPNAL);
            string pagaduriaCagenPonal = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionCAGENPNAL);
            string pagaduriaCasurPonal = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionCASURPNAL);
            string pagaduriaCremilPonal = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionCREMILPNAL);
            string pagaduriaCremilEjercito = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_CodigoIncorporcionCREMILEJERC);

            //Permite modificar los paneles
            this.tableLayoutPanel1.RowStyles[0].Height = 40;
            this.tableLayoutPanel1.RowStyles[1].Height = 250;
            this.tableLayoutPanel1.RowStyles[2].Height = 0;
            this.tableLayoutPanel1.RowStyles[3].Height = 0;

            //Habilita los botones +- de la grilla
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Enabled = true;
            this.gcDocuments.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = true;

            //Pone la fecha de aplica con base a la del periodo
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            DateTime fechaPerido = Convert.ToDateTime(strPeriodo);
            fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtFechaIncorpora.Properties.MaxValue = new DateTime(fechaPerido.Year, fechaPerido.Month, DateTime.DaysInMonth(fechaPerido.Year, fechaPerido.Month));
            this.dtFechaIncorpora.Properties.MinValue = new DateTime(fechaPerido.Year, fechaPerido.Month, 1);
            this.dtFechaIncorpora.DateTime = new DateTime(fechaPerido.Year, fechaPerido.Month, fechaPerido.Day);

            //Inicializa el control del periodo
            this.dtMesIncorpora.MinValue = new DateTime(2013, 1, 1);
            this.dtMesIncorpora.DateTime = new DateTime(fechaPerido.Year, fechaPerido.Month, fechaPerido.Day);
            this._bc.InitPeriodUC(this.dtMesIncorpora, 0);

            this.FilterPagadurias();

            //Tipos de novedad (visible en la grilla)
            this.tiposNovedad[1] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_1);
            this.tiposNovedad[2] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_2);
            this.tiposNovedad[3] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_3);
            this.tiposNovedad[4] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_4);
            this.tiposNovedad[5] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_5);
            //this.tiposNovedad[7] = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_7);

            //Recursos
            this.msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
            this.msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            this.msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            this.msgLibranzaInvalida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_InvalidLibranza);
            this.msgLibranzaCancelada = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CreditoCancelado);
            this.msgLibranzaRepetida = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CreditoAgregado);
            this.msgInvalidCP = _bc.GetResourceError(DictionaryMessages.Err_Cc_InvalidCentroPagoLibranza);
            this.msgInvalidTipoNov = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov);
            this.msgInvalidTipoNov1 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov1);
            this.msgInvalidTipoNov5 = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNov5);
            this.msgInvalidTipoNovPagaduriasDif = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ReincorporacionInvalidTipoNipoNovPagaduriasDif);
            this.msgCredExiste = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaExiste);

            this.msgInvalidTipoNov1 = string.Format(this.msgInvalidTipoNov1, this.tiposNovedad[1]);
            this.msgInvalidTipoNov5 = string.Format(this.msgInvalidTipoNov5, this.tiposNovedad[5]);

            FormProvider.LoadResources(this, AppDocuments.Incorporacion);
            base.SetInitParameters();

            this.refreshGridDelegate = new RefreshGrid(RefreshGridMethod);
            //this.gvDocuments.OptionsView.ColumnAutoWidth = false;.            
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Fecha de incorporación
            this.dtFechaIncorpora.DateTimeChanged += new System.EventHandler(this.dtFechaIncorpora_DateTimeChanged);
            this.dtFechaIncorpora_DateTimeChanged(null, null);

            //Tipo de incorporación
            this.comboTipoIncorp.SelectedIndex = 0;
            this.comboTipoIncorp.SelectedIndexChanged += new System.EventHandler(this.comboTipoIncorp_SelectedIndexChanged);
            this.comboTipoIncorp_SelectedIndexChanged(null, null);
            this.editCmbDict.DataSource = this.tiposNovedad;
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
                aprob.Width = 50;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 20;
                noAprob.Visible = false;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.Integer;
                libranza.VisibleIndex = 2;
                libranza.Width = 65;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);
                this.libranzaRsx = libranza.Caption;

                //Centro Pago
                GridColumn cp = new GridColumn();
                cp.FieldName = this.unboundPrefix + "Otro1";
                cp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Otro1");
                cp.UnboundType = UnboundColumnType.String;
                cp.VisibleIndex = 3;
                cp.Width = 100;
                cp.Visible = true;
                cp.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(cp);
                this.centroPagoRsx = cp.Caption;

                //TipoCredito
                GridColumn tipoCred = new GridColumn();
                tipoCred.FieldName = this.unboundPrefix + "TipoCreditoID";
                tipoCred.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoCredito");
                tipoCred.UnboundType = UnboundColumnType.Integer;
                tipoCred.VisibleIndex = 4;
                tipoCred.Width = 40;
                tipoCred.Visible = true;
                tipoCred.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(tipoCred);

                //Num Reincorpora
                GridColumn numReincorpora = new GridColumn();
                numReincorpora.FieldName = this.unboundPrefix + "NumReincorpora";
                numReincorpora.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumReincorpora");
                numReincorpora.UnboundType = UnboundColumnType.Integer;
                numReincorpora.VisibleIndex = 5;
                numReincorpora.Width = 50;
                numReincorpora.Visible = true;
                numReincorpora.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(numReincorpora);

                //Novedad
                GridColumn novedad = new GridColumn();
                novedad.FieldName = this.unboundPrefix + "NovedadIncorporaID";
                novedad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Novedad");
                novedad.UnboundType = UnboundColumnType.String;
                novedad.VisibleIndex = 6;
                novedad.Width = 60;
                novedad.Visible = true;
                novedad.OptionsColumn.AllowEdit = false;
                novedad.ColumnEdit = this.editBtnGrid;
                this.gvDocuments.Columns.Add(novedad);
                this.novedadRsx = novedad.Caption;

                //TipoNovedad
                GridColumn tipoNovedad = new GridColumn();
                tipoNovedad.FieldName = this.unboundPrefix + "Otro";
                tipoNovedad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoNovedad");
                tipoNovedad.UnboundType = UnboundColumnType.String;
                tipoNovedad.VisibleIndex = 7;
                tipoNovedad.Width = 50;
                tipoNovedad.Visible = true;
                tipoNovedad.OptionsColumn.AllowEdit = false;
                tipoNovedad.ColumnEdit = this.editCmbDict;
                this.gvDocuments.Columns.Add(tipoNovedad);
                this.tipoNovedadRsx = tipoNovedad.Caption;

                //Cliente Id
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 8;
                clienteID.Width = 70;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 9;
                nombCliente.Width = 100;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombCliente);

                //Valor Libranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Decimal;
                vlrLibranza.VisibleIndex = 10;
                vlrLibranza.Width = 120;
                vlrLibranza.Visible = true;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                vlrLibranza.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrLibranza);

                //Valor Cuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Decimal;
                vlrCuota.VisibleIndex = 11;
                vlrCuota.Width = 90;
                vlrCuota.Visible = true;
                vlrCuota.OptionsColumn.AllowEdit = false;
                vlrCuota.ColumnEdit = this.editSpin;
                this.gvDocuments.Columns.Add(vlrCuota);
                this.vlrCuotaRsx = vlrCuota.Caption;

                //Plazo
                GridColumn plazo = new GridColumn();
                plazo.FieldName = this.unboundPrefix + "Plazo";
                plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Plazo");
                plazo.UnboundType = UnboundColumnType.Decimal;
                plazo.VisibleIndex = 12;
                plazo.Width = 60;
                plazo.Visible = true;
                plazo.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(plazo);

                //Fecha Cuota 1
                GridColumn FechaCuota1 = new GridColumn();
                FechaCuota1.FieldName = this.unboundPrefix + "FechaCuota1";
                FechaCuota1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCuota1");
                FechaCuota1.UnboundType = UnboundColumnType.DateTime;
                FechaCuota1.VisibleIndex = 13;
                FechaCuota1.Width = 120;
                FechaCuota1.Visible = true;
                FechaCuota1.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(FechaCuota1);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 14;
                desc.Width = 100;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga el cabezote con los documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                // Carga la data
                if (this.comboTipoIncorp.SelectedIndex == 0)
                {
                    this.creditos = this._bc.AdministrationModel.IncorporacionCredito_GetByCentroPago(string.Empty, this.dtFechaIncorpora.DateTime.Date, string.Empty,this.chkGetPendIncorp.Checked);
                    this.filterCreditos = this.creditos;
                    this.credito = this.filterCreditos.FirstOrDefault();
                    this.solicitudes = new List<DTO_ccSolicitudDocu>();
                    this.filterSolicitudes = new List<DTO_ccSolicitudDocu>();

                    this.centroPagoID = this.credito != null ? this.credito.CentroPagoID.Value : string.Empty;
                }
                else if (this.comboTipoIncorp.SelectedIndex == 1)
                {
                    this.solicitudes = this._bc.AdministrationModel.IncorporacionSolicitud_GetByCentroPago(string.Empty, this.dtFechaIncorpora.DateTime.Date, string.Empty);
                    this.filterSolicitudes = this.solicitudes;
                    this.solicitud = this.filterSolicitudes.FirstOrDefault(); 
                    this.creditos = new List<DTO_ccCreditoDocu>();
                    this.filterCreditos = new List<DTO_ccCreditoDocu>();

                    this.centroPagoID = this.solicitud != null ? this.solicitud.CentroPagoID.Value : string.Empty;
                }

                // Carga el centro de pago y la pagaduria
                if (!string.IsNullOrWhiteSpace(this.centroPagoID))
                {
                    //Centro de pago
                    if (this.centrosPago.ContainsKey(this.centroPagoID))
                        this.centroPago = this.centrosPago[this.centroPagoID];
                    else
                    {
                        this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.centroPagoID, true, null);
                        this.centrosPago[this.centroPagoID] = this.centroPago;
                    }

                    //Pagaduria
                    this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                    this.pagaduriaID = this.pagaduria.ID.Value;
                    this.recaudoMes = this.pagaduria.RecaudoMes.Value.Value;
                    this.diaTope = this.pagaduria.DiaTope.Value.Value;
                }

                this.FilterData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected override bool ValidateDocRow(int fila)
        {
            try
            {
                this.validCreditos = base.ValidateDocRow(fila);
                if (this.validCreditos && this.allowValidate)
                {
                    byte tipoNov = 0;
                    bool validTipoNovedad = false;
                    if (this.credito.Editable.Value.Value)
                    {
                        //Tipo Novedad
                        validTipoNovedad = _bc.ValidGridCellValue(this.gvDocuments, this.unboundPrefix, fila, "Otro", false, false, true, false);
                        if(validTipoNovedad)
                            tipoNov = Convert.ToByte(this.credito.Otro.Value);

                        #region Libranza
                        GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Libranza"];
                        if (this.credito.NumeroDoc.Value == null)
                        {
                            this.gvDocuments.SetColumnError(col, this.msgLibranzaInvalida);
                            this.validCreditos = false;
                        }
                        else if (this.credito.CanceladoInd.Value.Value)
                        {
                            this.gvDocuments.SetColumnError(col, string.Format(this.msgLibranzaCancelada, this.credito.Libranza.Value.ToString()));
                            this.validCreditos = false;
                        }
                        else if (this.credito.CentroPagoID.Value != this.centroPagoID)
                        {
                            if (!validTipoNovedad || tipoNov != (byte)TipoNovedad.CambioPagaduria)
                            {
                                this.gvDocuments.SetColumnError(col, this.msgInvalidTipoNovPagaduriasDif);
                                this.validCreditos = false;
                            }
                        }
                        else
                        {
                            int count = this.creditos.Count(c => c.Libranza.Value == this.credito.Libranza.Value);
                            if (count > 1)
                            {
                                int index = this.creditos.FindIndex(c => c.Libranza.Value == this.credito.Libranza.Value);
                                this.gvDocuments.SetColumnError(col, string.Format(this.msgLibranzaRepetida, this.credito.Libranza.Value.ToString(), index + 1));
                                this.validCreditos = false;
                            }
                            else
                                this.gvDocuments.SetColumnError(col, string.Empty);
                        }
                        #endregion
                        #region NovedadIncorporaID

                        bool validNovedad = _bc.ValidGridCell(this.gvDocuments, this.unboundPrefix, fila, "NovedadIncorporaID", true, true, false, AppMasters.ccIncorporacionNovedad);
                        if (!validNovedad)
                            this.validCreditos = false;

                        #endregion
                        #region Tipo Novedad

                        if (!validTipoNovedad)
                            this.validCreditos = false;
                        else 
                        {
                            if (tipoNov == (byte)TipoNovedad.AdicionDigito && !this.centroPago.DigitoReincorporaIND.Value.Value)
                            {
                                this.gvDocuments.SetColumnError(col, this.msgInvalidTipoNov1);
                                this.validCreditos = false;
                            }
                            else if (tipoNov == (byte)TipoNovedad.CambioPagaduria && this.credito.CentroPagoID.Value == this.centroPagoID)
                            {
                                this.gvDocuments.SetColumnError(col, this.msgInvalidTipoNov5);
                                this.validCreditos = false;
                            }
                        }

                        #endregion
                    }

                    #region VlrCuota

                    bool valirVlrCuota = _bc.ValidGridCellValue(this.gvDocuments, this.unboundPrefix, fila, "VlrCuota", false, false, true, false);
                    if (!valirVlrCuota)
                        this.validCreditos = false;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.isOK = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudCreditoChequeo.cs", "ValidateDocRow"));
            }
            return this.validCreditos;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Filter invalid pagadurias
        /// </summary>
        private void FilterPagadurias()
        {
            try
            {
                List<string> invalidPagadurias = _bc.AdministrationModel.IncorporacionCredito_GetInvalidPagadurias(this.dtFechaIncorpora.DateTime);

                this.filtrosPagadurias = new List<DTO_glConsultaFiltro>();
                invalidPagadurias.ForEach(p =>
                {
                    this.filtrosPagadurias.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "PagaduriaID",
                        OperadorFiltro = OperadorFiltro.Diferente,
                        ValorFiltro = p,
                        OperadorSentencia = OperadorSentencia.And
                    });
                });

                //Cargar los Controles de Mestras
                this._bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, false, false, this.filtrosPagadurias);
                this.masterCentroPago.Value = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "LoadPagadurias"));
            }
        }

        /// <summary>
        /// Funcion que establece la fecha de incorporacion de un credito
        /// </summary>
        private void CalcularFechaIncorpora()
        {
            try
            {
                int diaIncorporacion = this.dtFechaIncorpora.DateTime.Day;
                if (diaIncorporacion <= this.diaTope && this.recaudoMes)
                {
                    //this.dtMesIncorpora.DateTime = new DateTime(this.dtFechaIncorpora.DateTime.Year, this.dtFechaIncorpora.DateTime.Month, diaIncorporacion);
                    this.dtMesIncorpora.DateTime = this.dtFechaIncorpora.DateTime; 
                }
                else
                {
                    //this.dtMesIncorpora.DateTime = new DateTime(this.dtFechaIncorpora.DateTime.Year, this.dtFechaIncorpora.DateTime.Month + 1, diaIncorporacion);
                    this.dtMesIncorpora.DateTime = this.dtFechaIncorpora.DateTime.AddMonths(1);
                }

                if (diaIncorporacion > this.diaTope && !this.recaudoMes)
                {
                    //this.dtMesIncorpora.DateTime = new DateTime(this.dtFechaIncorpora.DateTime.Year, this.dtFechaIncorpora.DateTime.Month + 2, diaIncorporacion);
                    this.dtMesIncorpora.DateTime = this.dtFechaIncorpora.DateTime.AddMonths(2);
                }

                if (this.isCredito)
                {
                    foreach (DTO_ccCreditoDocu cred in this.creditos)
                        cred.FechaIncorpora.Value = this.dtFechaIncorpora.DateTime;
                }
                else
                {
                    foreach (DTO_ccSolicitudDocu docu in this.solicitudes)
                        docu.FechaIncorpora.Value = this.dtFechaIncorpora.DateTime;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "CalcularFechaIncorpora"));
            }
        }

        /// <summary>
        /// Funcion que valida las condiciones para que el documento se pueda guardar
        /// </summary>
        private bool ValidateDoc()
        {
            //if (!this.masterCentroPago.ValidID)
            //{
            //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
            //    MessageBox.Show(msg);
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// Funcion que limpia los controles de la pantalla
        /// </summary>
        private void CleanData()
        {
            this.validateData = false;

            //Header
            this.masterCentroPago.Value = String.Empty;

            //Footer           
            this.credito = null;
            this.solicitud = null;
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.filterCreditos = new List<DTO_ccCreditoDocu>();
            this.solicitudes = new List<DTO_ccSolicitudDocu>();
            this.filterSolicitudes = new List<DTO_ccSolicitudDocu>();

            //Variables
            this.canEdit = false;
            this.centroPagoID = String.Empty;

            this.gcDocuments.Enabled = true;
            this.gcDocuments.DataSource = null;
        }

        /// <summary>
        /// Filters grid data
        /// </summary>
        private void FilterData()
        {
            try
            {
                this.validateData = false;
                this.currentDoc = null;

                this.currentRow = -1;
                this.canEdit = true;
                this.detailsLoaded = false;
                this.allowValidate = false;
                this.validateData = false;

                this.currentRow = 0;
                if (this.comboTipoIncorp.SelectedIndex == 0)
                {
                    this.gcDocuments.DataSource = this.filterCreditos;
                }
                else
                {
                    this.gcDocuments.DataSource = this.filterSolicitudes;
                }


                if (!detailsLoaded)
                    this.currentDoc = this.gvDocuments.GetRow(this.currentRow);

                this.gvDocuments.RefreshData();
                this.firstTime = false;
                this.allowValidate = true;
                this.validateData = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "FilterData"));
            }
        }

        /// <summary>
        /// Carga las pagadurias que se encuentran en el control
        /// </summary>
        /// <returns></returns>
        private bool ValidateCentroPago()
        {
            return this.pagaduriaID == this.pagaduiaISS
                || this.pagaduriaID == this.pagaduriaPonal
                || this.pagaduriaID == this.pagaduriaCagenPonal
                || this.pagaduriaID == this.pagaduriaCasurPonal
                || this.pagaduriaID == this.pagaduriaCremilPonal
                || this.pagaduriaID == this.pagaduriaCremilEjercito;
        }

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddIncorporacion()
        {
            try
            {
                DTO_ccCreditoDocu newCred = new DTO_ccCreditoDocu();
                newCred.Editable.Value = true;
                newCred.Aprobado.Value = false;
                newCred.Rechazado.Value = false;
                newCred.NumeroINC.Value = 0;
                newCred.Otro.Value = ((byte)TipoNovedad.Actualizar).ToString();
                newCred.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();

                this.creditos.Add(newCred);
                this.gcDocuments.DataSource = this.filterCreditos;
                this.gvDocuments.PostEditor();
                this.gvDocuments.FocusedRowHandle = this.filterCreditos.Count - 1;
                this.gvDocuments.RefreshData();

                this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns[this.unboundPrefix + "Otro"].OptionsColumn.AllowEdit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "AddComponente"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Carga el Boton de Exportar Archivo
        /// </summary>
        /// <param name="sender">Evento que se ejecuta cuando sale del control</param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.itemNew.Visible = true;
            FormProvider.Master.itemNew.Enabled = true;
            FormProvider.Master.itemExport.Visible = true;
            FormProvider.Master.itemGenerateTemplate.Visible = true;
            FormProvider.Master.itemImport.Visible = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemGenerateTemplate.Enabled = true;
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al modificar la seleccion del combo, especifica como se debe filtrar la incopracion
        /// </summary>
        private void comboTipoIncorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.firstTime)
                {
                    this.centroPagoID = String.Empty;
                    this.masterCentroPago.Value = string.Empty;
                    this.allowValidate = false;
                    this.LoadDocuments();
                    this.allowValidate = true;
                }

                if (this.comboTipoIncorp.SelectedIndex == 0) // Incorpora Liquida
                {
                    this.isCredito = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "NumReincorpora"].Visible = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].Visible = true;
                    this.gvDocuments.Columns[this.unboundPrefix + "Otro"].Visible = true; //TipoNovedad
         
                    this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].VisibleIndex = 0;
                    this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].VisibleIndex = 1;
                    this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].VisibleIndex = 2;
                    this.gvDocuments.Columns[this.unboundPrefix + "Otro1"].VisibleIndex = 3; // Centro Pago
                    this.gvDocuments.Columns[this.unboundPrefix + "TipoCreditoID"].VisibleIndex = 4;
                    this.gvDocuments.Columns[this.unboundPrefix + "NumReincorpora"].VisibleIndex = 5;
                    this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].VisibleIndex = 6;
                    this.gvDocuments.Columns[this.unboundPrefix + "Otro"].VisibleIndex = 7; //TipoNovedad
                    this.gvDocuments.Columns[this.unboundPrefix + "ClienteID"].VisibleIndex = 8;
                    this.gvDocuments.Columns[this.unboundPrefix + "Nombre"].VisibleIndex = 9;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrLibranza"].VisibleIndex = 10;
                    this.gvDocuments.Columns[this.unboundPrefix + "VlrCuota"].VisibleIndex = 11;
                    this.gvDocuments.Columns[this.unboundPrefix + "Plazo"].VisibleIndex = 12;
                    this.gvDocuments.Columns[this.unboundPrefix + "FechaCuota1"].VisibleIndex = 13;
                    this.gvDocuments.Columns[this.unboundPrefix + "Observacion"].VisibleIndex = 14;

                    this.gvDocuments.Columns[this.unboundPrefix + "VlrCuota"].OptionsColumn.AllowEdit = true;

                    this.format = this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].Caption + this.formatSeparator +
                        this.gvDocuments.Columns[this.unboundPrefix + "Otro1"].Caption + this.formatSeparator + 
                        this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].Caption + this.formatSeparator +
                        this.gvDocuments.Columns[this.unboundPrefix + "Otro"].Caption + this.formatSeparator +
                        this.gvDocuments.Columns[this.unboundPrefix + "VlrCuota"].Caption;


                    this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].Visible = false;
                }
                else
                {
                    this.isCredito = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].Visible = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "NumReincorpora"].Visible = false;
                    this.gvDocuments.Columns[this.unboundPrefix + "Otro"].Visible = false; //TipoNovedad

                    this.gvDocuments.Columns[this.unboundPrefix + "VlrCuota"].OptionsColumn.AllowEdit = false;

                    this.format = this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].Caption;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "comboTipoIncorp_SelectedIndexChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el valor de la tasa de venta
        /// </summary>       
        private void dtFechaIncorpora_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.loadDocumentsFromDate)
                {
                    this.FilterPagadurias();
                    this.LoadDocuments();
                }
                this.loadDocumentsFromDate = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "dtFechaIncorpora_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Evento que filtra los documentos de acuerdo a la pagaduria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.validCreditos)
                {
                    if (this.isCredito && this.creditos.Count > 0)
                    {
                        this.filterCreditos = string.IsNullOrWhiteSpace(this.masterCentroPago.Value) ? this.creditos : this.creditos.Where(x => x.CentroPagoID.Value == this.masterCentroPago.Value).ToList();
                    }
                    else if (!this.isCredito && this.solicitudes.Count > 0)
                    {
                        this.filterSolicitudes = !string.IsNullOrWhiteSpace(this.masterCentroPago.Value) ? this.solicitudes : this.solicitudes.Where(x => x.CentroPagoID.Value == this.masterCentroPago.Value).ToList();
                    }

                    this.FilterData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "masterCentroPago_Leave"));
            }
        }

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    if (this.comboTipoIncorp.SelectedIndex == 0)
                    {
                        this.creditos[i].Aprobado.Value = true;
                        this.creditos[i].Rechazado.Value = false;
                    }
                    else
                    {
                        this.solicitudes[i].Aprobado.Value = true;
                        this.solicitudes[i].Rechazado.Value = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < gvDocuments.DataRowCount; i++)
                {
                    if (this.comboTipoIncorp.SelectedIndex == 0)
                    {
                        this.creditos[i].Aprobado.Value = false;
                        this.creditos[i].Rechazado.Value = false;
                    }
                    else
                    {
                        this.solicitudes[i].Aprobado.Value = false;
                        this.solicitudes[i].Rechazado.Value = false;
                    }
                }
            }
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocuments_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (!this.canEdit)
                {
                    e.Handled = true;
                    return;
                }

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.isCredito)
                    {
                        if (this.creditos.Count > 0)
                            this.ValidateDocRow(this.gvDocuments.FocusedRowHandle);

                        if(this.validCreditos)
                            this.AddIncorporacion();
                    }
                }
                else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    if (this.credito == null || !this.credito.Editable.Value.Value)
                        e.Handled = true;
                    else
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int fila = this.gvDocuments.FocusedRowHandle;
                            this.credito = null;
                            this.creditos.RemoveAt(fila);

                            if (fila == 0)
                                this.gvDocuments.FocusedRowHandle = 0;
                            else
                                this.gvDocuments.FocusedRowHandle = fila - 1;

                            this.gvDocuments.RefreshData();
                            if(this.creditos.Count > 0)
                                this.credito = this.creditos[this.gvDocuments.FocusedRowHandle];
                        }
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "gcDocuments_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            #region Generales

            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    if (this.comboTipoIncorp.SelectedIndex == 0)
                        this.creditos[e.RowHandle].Rechazado.Value = false;
                    else
                        this.solicitudes[e.RowHandle].Rechazado.Value = false;
                }

                this.gcDocuments.RefreshDataSource();
                this.ValidateDocRow(e.RowHandle);
            }
            

            #endregion
        }

        /// <summary>
        /// Evento que valida las columna de la grilla despues de editarlas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gcDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + fieldName];

                if (fieldName == "Libranza")
                {
                    string novedad = this.credito.NovedadIncorporaID.Value;
                    string tipoNovedad = this.credito.Otro.Value;
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                    {
                        this.credito = new DTO_ccCreditoDocu();
                        this.credito.Editable.Value = true;
                        this.credito.Aprobado.Value = true;
                        this.credito.Rechazado.Value = false;
                        this.credito.Libranza.Value = 0;
                    }
                    else
                    {
                        int libTemp = (int)e.Value;
                        List<DTO_ccCreditoDocu> crediTemp = (from c in this.creditos where c.Libranza.Value == libTemp select c).ToList();
                        if (crediTemp.Count > 1)
                        {
                            this.gvDocuments.SetColumnError(col, this.msgCredExiste);
                            this.validCreditos = false;
                        }
                        else
                        {
                            string obs = this.credito.Observacion.Value;
                            string cp = string.Empty;
                            this.credito = _bc.AdministrationModel.GetCreditoByLibranza((int)e.Value);
                            if (this.credito == null)
                            {
                                this.credito = new DTO_ccCreditoDocu();
                                this.credito.Libranza.Value = Convert.ToInt32(e.Value);
                            }
                            else
                            {
                                //Centro de pago
                                if (this.centrosPago.ContainsKey(this.credito.CentroPagoID.Value))
                                    this.centroPago = this.centrosPago[this.credito.CentroPagoID.Value];
                                else
                                {
                                    this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.credito.CentroPagoID.Value, true, null);
                                    this.centrosPago[this.credito.CentroPagoID.Value] = this.centroPago;
                                }
                                cp = this.centroPago.Descriptivo.Value;
                                this.centroPagoID = this.centroPago.ID.Value;

                                //Pagaduria
                                this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                this.pagaduriaID = this.pagaduria.ID.Value;
                                this.recaudoMes = this.pagaduria.RecaudoMes.Value.Value;
                                this.diaTope = this.pagaduria.DiaTope.Value.Value;

                                //Cliente
                                DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.credito.ClienteID.Value, true);
                                this.credito.Nombre.Value = cliente.Descriptivo.Value;
                            }

                            this.credito.Editable.Value = true;
                            this.credito.Aprobado.Value = false;
                            this.credito.Rechazado.Value = false;
                            this.credito.NumeroINC.Value = 0;
                            this.credito.Otro.Value = ((byte)TipoNovedad.NuevoDescuento).ToString();
                            this.credito.Otro1.Value = cp;
                            this.credito.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();
                            this.credito.Observacion.Value = obs;
                        }
                    }

                    this.credito.NovedadIncorporaID.Value = novedad;
                    this.credito.Otro.Value = tipoNovedad;
                }

                if (fieldName == "NovedadIncorporaID")
                {
                    this.credito.NovedadIncorporaID.Value = e.Value.ToString();
                }

                //Tipo Novedad
                if (fieldName == "Otro")
                {
                    this.credito.Otro.Value = e.Value.ToString();
                    byte tipoNov = Convert.ToByte(e.Value);
                    if (tipoNov == (byte)TipoNovedad.AdicionDigito)
                    {
                        this.creditos[e.RowHandle].NumReincorpora.Value = this.creditos[e.RowHandle].NumeroINC.Value;
                        this.credito.NumReincorpora.Value = this.credito.NumeroINC.Value;
                    }
                    else
                    {
                        this.creditos[e.RowHandle].NumReincorpora.Value = 0;
                        this.credito.NumReincorpora.Value = 0;
                    }
                }

                if (fieldName == "VlrCuota")
                {
                    if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
                        this.credito.VlrCuota.Value = 0;
                    else
                        this.credito.VlrCuota.Value = Convert.ToInt32(e.Value);
                }

                this.creditos[e.RowHandle] = this.credito;
                this.gcDocuments.RefreshDataSource();

                this.ValidateDocRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporaciones.cs", "gcDocuments_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.validateData && e.RowHandle >= 0 && this.isCredito && this.credito != null && this.canEdit)
            {
                this.ValidateDocRow(e.RowHandle);
                if (!this.validCreditos)
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
            try
            {
                int row = e.FocusedRowHandle;
                if (this.validateData && row >= 0)
                {
                    if (this.isCredito)
                    {
                        this.credito = (DTO_ccCreditoDocu)this.gvDocuments.GetRow(e.FocusedRowHandle);
                        this.centroPagoID = this.credito.CentroPagoID.Value;

                        //Habilita o deshabilita las columnas de acuerdo al tipo deincorporacion
                        if (this.credito.Editable.Value.Value)
                        {
                            this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].OptionsColumn.AllowEdit = true;
                            this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].OptionsColumn.AllowEdit = true;
                            this.gvDocuments.Columns[this.unboundPrefix + "Otro"].OptionsColumn.AllowEdit = true; //TipoNovedad
                        }
                        else
                        {
                            this.gvDocuments.Columns[this.unboundPrefix + "Libranza"].OptionsColumn.AllowEdit = false;
                            this.gvDocuments.Columns[this.unboundPrefix + "NovedadIncorporaID"].OptionsColumn.AllowEdit = false;
                            this.gvDocuments.Columns[this.unboundPrefix + "Otro"].OptionsColumn.AllowEdit = false; //TipoNovedad
                        }
                    }
                    else
                    {
                        this.solicitud = (DTO_ccSolicitudDocu)this.gvDocuments.GetRow(e.FocusedRowHandle);
                        this.centroPagoID = this.solicitud.CentroPagoID.Value;
                    }

                    // Carga el centro de pago y la pagaduria
                    if (!string.IsNullOrWhiteSpace(this.centroPagoID))
                    {
                        //Centro de pago
                        if (this.centrosPago.ContainsKey(this.centroPagoID))
                            this.centroPago = this.centrosPago[this.centroPagoID];
                        else
                        {
                            this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, this.centroPagoID, true, null);
                            this.centrosPago[this.centroPagoID] = this.centroPago;
                        }

                        //Pagaduria
                        this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                        this.pagaduriaID = this.pagaduria.ID.Value;
                        this.recaudoMes = this.pagaduria.RecaudoMes.Value.Value;
                        this.diaTope = this.pagaduria.DiaTope.Value.Value;
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "gvDetails_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.comboTipoIncorp.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                bool isValid = this.validCreditos && this.ValidateDoc();
                if (((this.creditos != null && this.creditos.Count != 0) || (this.solicitudes != null && this.solicitudes.Count != 0)) && isValid)
                {
                    this.CalcularFechaIncorpora();

                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton que exporta archivos planos para las pagadurias
        /// </summary>
        public override void TBExport()
        {
            try
            {
                DataTableOperations tableOp = new DataTableOperations();
                DataTable tableExport = new DataTable();

                if (this.comboTipoIncorp.SelectedIndex == 0)
                {                  
                    #region Exporta Creditos
		            tableExport.Columns.Add("Libranza");
                    tableExport.Columns.Add("Otro1");
                    tableExport.Columns.Add("TipoCredito");
                    tableExport.Columns.Add("NumReincorpora");
                    tableExport.Columns.Add("NovedadIncorporaID");
                    tableExport.Columns.Add("Otro");
                    tableExport.Columns.Add("ClienteID");
                    tableExport.Columns.Add("Nombre");
                    tableExport.Columns.Add("VlrLibranza");
                    tableExport.Columns.Add("VlrCuota");
                    tableExport.Columns.Add("Plazo");
                    tableExport.Columns.Add("FechaCuota1");
                    tableExport.Columns.Add("Observacion");
                    foreach (DTO_ccCreditoDocu cred in this.filterCreditos)
                    {
                        string[] cols = 
                        {
                            cred.Libranza.Value.ToString(),
                            cred.Otro1.Value,
                            cred.TipoCredito.Value.ToString(),
                            cred.NumReincorpora.Value.ToString(),
                            cred.NovedadIncorporaID.Value,
                            this.tiposNovedad[Convert.ToInt32(cred.Otro.Value)],
                            cred.ClienteID.Value,
                            cred.Nombre.Value,
                            cred.VlrLibranza.Value != null? cred.VlrLibranza.Value.Value.ToString("n0"): "0",
                            cred.VlrCuota.Value != null? cred.VlrCuota.Value.Value.ToString("n0"): "0",
                            cred.Plazo.Value.ToString(),
                            cred.FechaCuota1.Value != null? cred.FechaCuota1.Value.Value.ToString("dd/MM/yyyy"): cred.FechaCuota1.Value.ToString(),
                            cred.Observacion.Value,
                        };
                        tableExport.Rows.Add(cols);
                    }
                    #endregion
                }
                else if (this.comboTipoIncorp.SelectedIndex == 1)
                {
                    #region Exporta solicitudes
                    tableExport.Columns.Add("Libranza");
                    tableExport.Columns.Add("TipoCredito");
                    tableExport.Columns.Add("Otro");
                    tableExport.Columns.Add("ClienteID");
                    tableExport.Columns.Add("Nombre");
                    tableExport.Columns.Add("VlrLibranza");
                    tableExport.Columns.Add("VlrCuota");
                    tableExport.Columns.Add("Plazo");
                    tableExport.Columns.Add("FechaCuota1");
                    tableExport.Columns.Add("Observacion");
                    foreach (DTO_ccSolicitudDocu cred in this.filterSolicitudes)
                    {
                        string[] cols = 
                        {
                            cred.Libranza.Value.ToString(),
                            cred.TipoCredito.Value.ToString(),
                            cred.Otro.Value,
                            cred.ClienteID.Value,
                            cred.Nombre.Value,
                            cred.VlrLibranza.Value != null? cred.VlrLibranza.Value.Value.ToString("n0"): "0",
                            cred.VlrCuota.Value != null? cred.VlrCuota.Value.Value.ToString("n0"): "0",
                            cred.Plazo.Value.ToString(),
                            cred.FechaCuota1.Value != null? cred.FechaCuota1.Value.Value.ToString("dd/MM/yyyy"): cred.FechaCuota1.Value.ToString(),
                            cred.Observacion.Value,
                        };
                        tableExport.Rows.Add(cols);
                    } 
                    #endregion
                }

                ReportExcelBase frm = new ReportExcelBase(tableExport, this.documentID);
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "TBExport"));
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
        /// Evento para importar datos de excel
        /// </summary>
        public override void TBImport()
        {
            try
            {
                if (this.isCredito)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportCreditosThread);
                        process.Start();
                    }
                }
                else if (this.solicitudes != null && this.solicitudes.Count != 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportSolicitudesThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBGenerateTemplate.cs", "TBImport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Export grid data
        /// </summary>
        protected virtual void ExportThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_TxResult resultReport = _bc.AdministrationModel.Report_Cc_ArchivosPlanos(this.pagaduriaID);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(resultReport);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "ExportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        private void ImportSolicitudesThread()
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
                    List<int> currentLibranzas = this.solicitudes.Select(l => l.Libranza.Value.Value).ToList();
                    List<DTO_ccSolicitudDocu> solicitudesTemp = ObjectCopier.Clone(this.solicitudes);

                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgInvalidLibranza = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                    string msgInvalidibranza = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_InvalidLibranza);

                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<int> libranzas = new List<int>();
                    List<string> colNames = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    string colRsx = colNames[0];

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer

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
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Message = "OK";

                            bool validLibranza = true;
                            string libranza = string.Empty;

                            #region Info básica

                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length > 1)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                validLibranza = false;
                                continue;
                            }
                            else
                            {
                                libranza = line[0];
                            }

                            #endregion
                            #region Validacion de Nulls
                            if (string.IsNullOrEmpty(libranza))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = colRsx;
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                validLibranza = false;
                            }
                            #endregion
                            #region Validacion Formatos
                            if (validLibranza)
                            {
                                try
                                {
                                    int val = Convert.ToInt32(libranza);
                                }
                                catch (Exception ex)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = colRsx;
                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                    rd.DetailsFields.Add(rdF);

                                    validLibranza = false;
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }

                            if (validLibranza && validList)
                                libranzas.Add(Convert.ToInt32(libranza));
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida que existan las libranza

                    if (validList)
                    {
                        int i = 0;
                        foreach (int lib in libranzas)
                        {
                            ++i;
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer

                            if (lines.Length == 1)
                            {
                                result.ResultMessage = msgNoCopyField;
                                result.Result = ResultValue.NOK;
                                validList = false;
                            }

                            #endregion
                            #region Valida que la libranza no exista en la lista
                            if (!currentLibranzas.Contains(lib))
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.Message =string.Format(msgInvalidibranza, lib);
                                rd.line = i + 1;

                                result.Result = ResultValue.NOK;
                                result.Details.Add(rd);
                                validList = false;
                            }
                            else
                            {
                                currentLibranzas.Add(lib);
                            }
                            #endregion
                        }
                    }

                    #endregion

                    if (validList)
                    {
                        libranzas.ForEach(l => this.solicitudes.First(s => s.Libranza.Value.Value == l).Aprobado.Value = true);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "ImportThread"));
            }
            finally
            {
                if (!this.pasteRet.Success)
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        private void ImportCreditosThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error

                    string msgNoSaldo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);

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

                    List<DTO_ccCreditoDocu> list = new List<DTO_ccCreditoDocu>();
                    List<DTO_ccCreditoDocu> finalList = new List<DTO_ccCreditoDocu>();

                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    colNames.Add(this.libranzaRsx, "Libranza");
                    colNames.Add(this.centroPagoRsx, "CentroPagoID");
                    colNames.Add(this.novedadRsx, "NovedadIncorporaID");
                    colNames.Add(this.tipoNovedadRsx, "Otro");
                    colNames.Add(this.vlrCuotaRsx, "VlrCuota");

                    colVals.Add(this.libranzaRsx, string.Empty);
                    colVals.Add(this.centroPagoRsx, string.Empty);
                    colVals.Add(this.novedadRsx, string.Empty);
                    colVals.Add(this.tipoNovedadRsx, string.Empty);
                    colVals.Add(this.vlrCuotaRsx, string.Empty);

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
                            createDTO = true;

                            DTO_ccCreditoDocu cred = new DTO_ccCreditoDocu();
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
                                        #region Centro de pago
                                        if (colRsx == this.centroPagoRsx)
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
                                    }
                                    #endregion // Carga la info de las fks
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                cred = new DTO_ccCreditoDocu();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == this.libranzaRsx || colName == this.masterCentroPago.ColId))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = cred.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(cred, null);
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
                                                    if (val <= 0)
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
                                                    if (val <= 0)
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
                                                    if (val <= 0)
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
                                                    if (val <= 0)
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
                                                    else if (val <= 0)
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
                                list.Add(cred);
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
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        foreach (DTO_ccCreditoDocu cred in list)
                        {
                            bool isValid = true;
                           
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
                            #region Valida el tipo de novedad

                            if (isValid)
                            {
                                if (!this.creditos.Any(c => c.Libranza.Value == cred.Libranza.Value) && string.IsNullOrWhiteSpace(cred.Otro.Value))
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = this.tipoNovedadRsx + " " + this.msgEmptyField;
                                    result.Details.Add(rd);
                                    validList = false;
                                }
                                else
                                {
                                    try
                                    {
                                        byte tipoNov = Convert.ToByte(cred.Otro.Value);
                                        if (tipoNov == 0 || tipoNov > 5)
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
                                    }
                                    catch (Exception ex)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.tipoNovedadRsx + " " + msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                }
                            }

                            #endregion
                            #region Valida los nuevos créditos
                            if (isValid)
                            {
                                DTO_ccCreditoDocu newCred = null;
                                if (this.creditos.Any(c => c.Libranza.Value == cred.Libranza.Value && !c.Editable.Value.Value))
                                {
                                    #region Créditos existentes

                                    newCred = ObjectCopier.Clone(this.creditos.FirstOrDefault(c => c.Libranza.Value == cred.Libranza.Value));

                                    //Novedad
                                    if (!string.IsNullOrWhiteSpace(cred.NovedadIncorporaID.Value))
                                        newCred.NovedadIncorporaID.Value = cred.NovedadIncorporaID.Value;

                                    //Tipo Novedad
                                    if (!string.IsNullOrWhiteSpace(cred.Otro.Value))
                                        newCred.Otro.Value = cred.Otro.Value;

                                    //Valor Cuota
                                    if (cred.VlrCuota.Value == null || !cred.VlrCuota.Value.HasValue || cred.VlrCuota.Value.Value == 0)
                                        newCred.VlrCuota.Value = cred.VlrCuota.Value;

                                    #region Carga el centro de pago y la pagaduria

                                    //Centro de pago
                                    if (this.centrosPago.ContainsKey(newCred.CentroPagoID.Value))
                                        this.centroPago = this.centrosPago[newCred.CentroPagoID.Value];
                                    else
                                    {
                                        this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, newCred.CentroPagoID.Value, true, null);
                                        this.centrosPago[newCred.CentroPagoID.Value] = this.centroPago;
                                    }

                                    //Pagaduria
                                    this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                    this.pagaduriaID = this.pagaduria.ID.Value;
                                    this.recaudoMes = this.pagaduria.RecaudoMes.Value.Value;
                                    this.diaTope = this.pagaduria.DiaTope.Value.Value;

                                    #endregion
                                    #region Valida el centro de pago con el tipo de novedad

                                    if (Convert.ToByte(newCred.Otro.Value) == (byte)TipoNovedad.AdicionDigito && !this.centroPago.DigitoReincorporaIND.Value.Value)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.tipoNovedadRsx + " " + this.msgInvalidTipoNov1;
                                        result.Details.Add(rd);
                                        validList = false;
                                    }

                                    #endregion

                                    #endregion
                                }
                                else
                                {
                                    #region Nuevos créditos

                                    DTO_ccCreditoDocu temp = _bc.AdministrationModel.GetCreditoByLibranza(cred.Libranza.Value.Value);
                                    if (temp == null)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = string.Format(this.msgLibranzaInvalida, cred.Libranza.Value.ToString());
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else if(temp.CanceladoInd.Value.Value)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = string.Format(this.msgLibranzaCancelada, cred.Libranza.Value.ToString());
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else if (temp.CentroPagoID.Value == cred.CentroPagoID.Value && Convert.ToByte(cred.Otro.Value) == (byte)TipoNovedad.CambioPagaduria)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.msgInvalidTipoNov5;
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else if (temp.CentroPagoID.Value != cred.CentroPagoID.Value && Convert.ToByte(cred.Otro.Value) != (byte)TipoNovedad.CambioPagaduria)
                                    {
                                        isValid = false;
                                        result.Result = ResultValue.NOK;
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = this.msgInvalidTipoNovPagaduriasDif;
                                        result.Details.Add(rd);
                                        validList = false;
                                    }
                                    else
                                    {
                                        newCred = ObjectCopier.Clone(temp);

                                        //Novedades
                                        if (string.IsNullOrWhiteSpace(cred.NovedadIncorporaID.Value))
                                        {
                                            isValid = false;
                                            result.Result = ResultValue.NOK;
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.line = i;
                                            rd.Message = this.novedadRsx + " " + this.msgEmptyField;
                                            result.Details.Add(rd);
                                            validList = false;
                                        }

                                        // VlrCuota
                                        if (cred.VlrCuota.Value == null || !cred.VlrCuota.Value.HasValue || cred.VlrCuota.Value.Value == 0)
                                        {
                                            isValid = false;
                                            result.Result = ResultValue.NOK;
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.line = i;
                                            rd.Message = this.vlrCuotaRsx + " " + this.msgPositive;
                                            result.Details.Add(rd);
                                            validList = false;
                                        }

                                        #region Valida el centro de pago

                                        //if (newCred.CentroPagoID.Value != cred.CentroPagoID.Value)
                                        //{
                                        //    isValid = false;
                                        //    result.Result = ResultValue.NOK;
                                        //    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        //    rd.line = i;
                                        //    rd.Message = this.msgInvalidCP;
                                        //    result.Details.Add(rd);
                                        //    validList = false;
                                        //}
                                        //else
                                        //{
                                            //Centro de pago
                                            if (this.centrosPago.ContainsKey(cred.CentroPagoID.Value))
                                                this.centroPago = this.centrosPago[cred.CentroPagoID.Value];
                                            else
                                            {
                                                this.centroPago = (DTO_ccCentroPagoPAG)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, cred.CentroPagoID.Value, true, null);
                                                this.centrosPago[cred.CentroPagoID.Value] = this.centroPago;
                                            }

                                            //Pagaduria
                                            this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.centroPago.PagaduriaID.Value, true);
                                            this.pagaduriaID = this.pagaduria.ID.Value;
                                            this.recaudoMes = this.pagaduria.RecaudoMes.Value.Value;
                                            this.diaTope = this.pagaduria.DiaTope.Value.Value;
                                        //}
                                        #endregion
                                        #region Valida el centro de pago con el tipo de novedad

                                        if (Convert.ToByte(cred.Otro.Value) == (byte)TipoNovedad.AdicionDigito && !this.centroPago.DigitoReincorporaIND.Value.Value)
                                        {
                                            isValid = false;
                                            result.Result = ResultValue.NOK;
                                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                            rd.line = i;
                                            rd.Message = this.tipoNovedadRsx + " " + this.msgInvalidTipoNov1;
                                            result.Details.Add(rd);
                                            validList = false;
                                        }

                                        #endregion

                                        if(isValid)
                                        {
                                            DTO_ccCentroPagoPAG cp = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, cred.CentroPagoID.Value, false); 
                                            
                                            newCred.Editable.Value = true;
                                            newCred.NovedadIncorporaID.Value = cred.NovedadIncorporaID.Value;
                                            newCred.Otro.Value = cred.Otro.Value;
                                            newCred.Otro1.Value = cp.Descriptivo.Value;
                                            newCred.Otro2.Value = ((byte)OrigenDatoIncorporacion.Manual).ToString();
                                            newCred.VlrCuota.Value = cred.VlrCuota.Value;
                                            newCred.NumReincorpora.Value = 0;
                                            newCred.NumeroINC.Value = 0;
                                        }
                                    }
                                    #endregion
                                }

                                if (isValid)
                                {
                                    newCred.Aprobado.Value = true;
                                    newCred.Rechazado.Value = false;

                                    //Tipos de novedad
                                    if (Convert.ToByte(newCred.Otro.Value) == (byte)TipoNovedad.AdicionDigito)
                                        newCred.NumReincorpora.Value = newCred.NumeroINC.Value;
                                    else
                                        newCred.NumReincorpora.Value = 0;

                                    finalList.Add(newCred);
                                }
                            }
                            
                            #endregion
                        }
                    }

                    #endregion

                    if (validList)
                    {
                        List<int> libs = finalList.Select(l => l.Libranza.Value.Value).Distinct().OrderByDescending(l1 => l1).ToList();
                        List<DTO_ccCreditoDocu> newList = new List<DTO_ccCreditoDocu>();
                        this.creditos.RemoveAll(cr => cr.Editable.Value.Value);
                        libs.ForEach(l =>
                        {
                            this.creditos.RemoveAll(r => r.Libranza.Value == l);

                            var listLib = finalList.Where(fl => fl.Libranza.Value == l);
                            newList.AddRange(listLib);
                        });

                        this.creditos.InsertRange(0, newList);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "ImportThread"));
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

                Tuple<int, List<DTO_SerializedObject>, string> tuple;
                decimal vlrIncorporacion = 0;

                if (this.isCredito)
                {
                    vlrIncorporacion = (from c in this.creditos where c.Aprobado.Value.Value select c.VlrLibranza.Value.Value).Sum();
                    this.solicitudes = new List<DTO_ccSolicitudDocu>();
                    tuple = _bc.AdministrationModel.IncorporacionCredito_Aprobar(documentID, this.actividadFlujoID, this.centroPagoID, this.dtFechaIncorpora.DateTime, vlrIncorporacion, this.creditos, this.solicitudes);
                }
                else
                {
                    vlrIncorporacion = (from c in this.solicitudes where c.Aprobado.Value.Value select c.VlrLibranza.Value.Value).Sum();
                    this.creditos = new List<DTO_ccCreditoDocu>();
                    tuple = _bc.AdministrationModel.IncorporacionCredito_Aprobar(documentID, this.actividadFlujoID, this.centroPagoID, this.dtFechaIncorpora.DateTime, vlrIncorporacion, this.creditos, this.solicitudes);
                }

                FormProvider.Master.StopProgressBarThread(this.documentID);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<DTO_SerializedObject> results = tuple.Item2;
                int i = 0;
                int percent = 0;

                string fileURl;

                #region Variables para el mail


                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.actividadDTO.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (this.isCredito)
                    {
                        #region Envia el correo de los creditos aprobados
                        DTO_ccCreditoDocu crediAprobacion = this.creditos[i];
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txtResult = (DTO_TxResult)result;
                            if (txtResult.Result == ResultValue.NOK)
                                resultsNOK.Add(txtResult);
                        }
                        else
                        {
                            #region Envia el correo
                            if (crediAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, crediAprobacion.NumeroDoc.Value, crediAprobacion.ClienteID.Value,
                                    crediAprobacion.Observacion.Value);


                            }
                            else if (crediAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, crediAprobacion.Observacion.Value, crediAprobacion.NumeroDoc.Value,
                                    crediAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Envia el correo de las solicitudes aprobadas
                        DTO_ccSolicitudDocu soliAprobacion = this.solicitudes[i];
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txtResult = (DTO_TxResult)result;
                            if (txtResult.Result == ResultValue.NOK)
                                resultsNOK.Add(txtResult);
                        }
                        else
                        {
                            #region Envia el correo
                            if (soliAprobacion.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, soliAprobacion.NumeroDoc.Value, soliAprobacion.ClienteID.Value,
                                    soliAprobacion.Observacion.Value);
                            }
                            else if (soliAprobacion.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, soliAprobacion.Observacion.Value, soliAprobacion.NumeroDoc.Value,
                                    soliAprobacion.ClienteID.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                        #endregion
                    }
                    i++;
                }

                MessageForm frm = new MessageForm(resultsNOK);
                frm.ShowDialog();
                if (resultsNOK.Count == 0)
                {
                    this.Invoke(this.refreshData);

                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, tuple.Item3);
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Incorporacion.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGetPendIncorp_CheckedChanged(object sender, EventArgs e)
        {
            if(!this.chkGetPendIncorp.Checked)
                FormProvider.Master.itemSave.Enabled =  SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            else
                FormProvider.Master.itemSave.Enabled = false;
            this.LoadDocuments();              
        }
    }
}
