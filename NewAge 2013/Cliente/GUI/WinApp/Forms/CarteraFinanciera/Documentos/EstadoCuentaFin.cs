using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;
using SentenceTransformer;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EstadoCuentaFin : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        private int numDocCredito = 0;
        private int numeroDocEstCuenta = 0;
        private int primeraCuota = 0;
        private string libranzaID = string.Empty;
        private DateTime periodo;
        private string clienteID = string.Empty;
        private DTO_ccCliente cliente;
        private DTO_ccCreditoDocu credito;
        private List<DTO_ccCreditoDocu> creditos;
        private List<DTO_ccEstadoCuentaComponentes> saldosTotales;
        private DTO_InfoCredito infoCreditoTemp = new DTO_InfoCredito();
        private DTO_InfoCredito infoCredito = new DTO_InfoCredito();
        private DTO_InfoCredito infoSaldos = new DTO_InfoCredito();
        private DTO_InfoCredito infoPagos = new DTO_InfoCredito();
        private DTO_EstadoCuenta estadoCuenta = null;
        private DTO_ccEstadoCuentaHistoria estadoCuentaHistoria = null;
        private Dictionary<string, DTO_ccCarteraComponente> componentesAll = new Dictionary<string, DTO_ccCarteraComponente>();

        //Actividades
        private DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();
        private string actFlujoDigitacionID;

        //Componentes
        private string componenteCapital;
        private string componenteMora;
        private string componenteSancionID;
        private string componenteInteres;
        private string componenteInteresNoCausado;
        private string componenteSeguro;
        private string componenteInteresSeguro;
        private string componenteAportes;
        private string componentePolizaEC;
        private string componenteInteresNoCausadoCapital;
        private string componenteInteresNoCausadoSeguro;
        private string componenteNoCausadoSeguro;
        private string componentePrejuridico;

        //Info de control
        private bool validate = true;
        private bool validComponentes = true;
        private decimal porcRevocatoriaSeguro;
        private string compradorCarteraPropia;
        private int diaLimite = 0;
        private bool _allowEditValuesInd = true;
        private string _userAutoriza = string.Empty;
        private bool _liquidarFinanNoCausadaInd = true;

        //Mensajes de error
        private string msgPagoInd;
        private string msgExtraCero;
        private string msgExtraInvalid;
        private string msgExtraEmpty;
        private string msgValorNeg;

        //Otros
        List<DTO_glConsultaFiltro> filtrosExtras = new List<DTO_glConsultaFiltro>();
        private DateTime fechaCorte;
        private decimal vlrRevSeg = 0;
        private decimal vlrRevIntSeg = 0;
        private string _nameProposito = string.Empty;
        private SectorCartera sector = SectorCartera.Solidario;
        private bool compSeguroExist = true;
        private byte _diasFNC = 0;
        private DateTime? _fechaInicioFNC = null;
        #endregion Variables

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public EstadoCuentaFin()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public EstadoCuentaFin(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
                this.InitControls();

                this.btnCuotasPagadas.Enabled = false;
                this.btnCuotasPendientes.Enabled = false;
                this.btnMovimiento.Enabled = false;

                string sectorStr = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                this.sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);

                this.componenteCapital = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                this.componenteInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                this.componenteMora = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
                this.componenteSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                this.componenteAportes = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentedeAportes);
                this.componenteInteresNoCausado = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresNoCausaDeuda);
                this.componenteSancionID = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteSancion);
                this.componenteInteresNoCausadoCapital = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteInteresNoCausaDeuda);
                this.componentePrejuridico = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);
                this._liquidarFinanNoCausadaInd =  this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_LiquidarFinanNoCausadaInd).Equals("1") ? true : false;

                if (this.sector == SectorCartera.Financiero)
                {
                    this.componenteInteresSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
                    this.componentePolizaEC = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePolizaEC);
                    this.componenteInteresNoCausadoSeguro = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteInteresNoCausaSeguro);
                    this.componenteNoCausadoSeguro = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteNoCausaSeguro);
                }
                else 
                {
                    this.lbl_Poliza.Visible = false;
                    this.lkpPoliza.Visible = false;
                    this.lbl_VlrSeguroVida.Visible = false;
                    this.txtSeguroVida.Visible = false;
                }

                this.porcRevocatoriaSeguro = 0;
                this.compradorCarteraPropia = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);
                string porcRevocatoriaSeguroStr = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PorcentajeRevocatoria);
                if (!string.IsNullOrWhiteSpace(porcRevocatoriaSeguroStr))
                    this.porcRevocatoriaSeguro = Convert.ToDecimal(porcRevocatoriaSeguroStr);

                #region Carga la info de la actividad del estado de cuenta
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this.actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
                #region Carga la info de la actividad de la digitación del crédito
                //List<string> actividadesDigitacion = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DigitacionCredito);

                //if (actividadesDigitacion.Count != 1)
                //{
                //    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                //    MessageBox.Show(string.Format(msg, AppDocuments.DigitacionCredito.ToString()));
                //}
                //else
                //{
                //    this.actFlujoDigitacionID = actividadesDigitacion[0];
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "EstadoCuenta"));
            }
        }

        #endregion

        #region Funciones Privadas

        #region Funciones básicas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.EstadoCuenta;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
            this.btnAnexos.Enabled = false;

            //Carga la info de los mensajes
            this.msgPagoInd = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_AbonoNoValue);
            this.msgExtraCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraCero);
            this.msgExtraInvalid = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompVlrExtra);
            this.msgExtraEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraVacios);
            this.msgValorNeg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);

            //Valida si permite editar los valores o pide autorizacion
            this._allowEditValuesInd = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CambioVlrConPassword).Equals("1") ? false : true;
            this._userAutoriza = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_UsuarioAutorizaCambiosVlr);
            if (string.IsNullOrEmpty(this._userAutoriza))
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No existe el usuario de autorización"));

            if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                this._allowEditValuesInd = true;

            //Filtros de componentes estras
            this.filtrosExtras.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "TipoComponente",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "5"
            });
        }

        /// <summary>
        /// Funcion q inicializa los controles del fpormulario
        /// </summary>
        private void InitControls()
        {
            try
            {
                Dictionary<string, string> dicEstado = new Dictionary<string, string>();
                dicEstado.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_Normal"));
                dicEstado.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_Mora"));
                dicEstado.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_Prejuridico"));
                dicEstado.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_CobroJuridico"));
                dicEstado.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoPago"));
                dicEstado.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoIncumplido"));
                dicEstado.Add("7", this._bc.GetResource(LanguageTypes.Tables, "tbl_Castigada"));
                this.cmbEstado.Properties.DataSource = dicEstado;

                Dictionary<byte, string> dicProp = new Dictionary<byte, string>();
                dicProp.Add(0, string.Empty);
                dicProp.Add((byte)PropositoEstadoCuenta.Proyeccion, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Proyeccion"));
                dicProp.Add((byte)PropositoEstadoCuenta.Prepago, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_CancelacionDirecta"));
                dicProp.Add((byte)PropositoEstadoCuenta.RecogeSaldo, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_RecogeSaldo"));
                if(sector == SectorCartera.Solidario)
                    dicProp.Add((byte)PropositoEstadoCuenta.RestructuracionAbono, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Restructuracion"));
                dicProp.Add((byte)PropositoEstadoCuenta.RestructuracionPlazo, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_RestructuracionCambio"));
                dicProp.Add((byte)PropositoEstadoCuenta.EnvioCobroJuridico, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_EnvioCobroJuridico"));
                dicProp.Add((byte)PropositoEstadoCuenta.Desistimiento, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Desistimiento"));
                //dicProp.Add((byte)PropositoEstadoCuenta.CondonacionTotal, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_CondonacionTotal"));
                //dicProp.Add((byte)PropositoEstadoCuenta.CondonacionParcial, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_CondonacionParcial"));
                //dicProp.Add((byte)PropositoEstadoCuenta.Normalizacion", this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Normalizacion"));
                dicProp.Add((byte)PropositoEstadoCuenta.CancelaPoliza, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_CancelaPoliza"));
                this.lkpProposito.Properties.DataSource = dicProp;
                this.lkpProposito.Enabled = false;

                Dictionary<string, string> dicPoliza = new Dictionary<string, string>();
                //dicPoliza.Add(string.Empty, string.Empty);
                dicPoliza.Add("0", this._bc.GetResource(LanguageTypes.Tables, "162_tbl_NoAplica"));
                dicPoliza.Add("1", this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Continua"));
                dicPoliza.Add("2", this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Paga"));
                dicPoliza.Add("3", this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Revoca"));
                this.lkpPoliza.Properties.DataSource = dicPoliza;
                this.lkpPoliza.Enabled = false;
                this.lkpPoliza.EditValue = "0";

                string diasLimite = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiasValidezEstado);
                this.diaLimite = Convert.ToInt16(diasLimite);

                //Estable las fechas con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                DateTime mesSiguiente = this.periodo.AddMonths(2);
                this.dt_FechaCorte.Properties.MaxValue = new DateTime(mesSiguiente.Year, mesSiguiente.Month, DateTime.DaysInMonth(mesSiguiente.Year, mesSiguiente.Month));
                int diaActual = DateTime.Today.Day;
                if (this.periodo.Month == 2 && diaActual > 28)
                    diaActual = 28;
                else if (diaActual > DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month))
                    diaActual = 30;
                this.dt_FechaCorte.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, diaActual);
                this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime.AddDays(this.diaLimite);

                this.fechaCorte = this.dt_FechaCorte.DateTime;
                this.cmbCuota.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla 1
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Superior
                //Fijado
                GridColumn Fijado = new GridColumn();
                Fijado.FieldName = this._unboundPrefix + "EC_FijadoInd";
                Fijado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fijado");
                Fijado.UnboundType = UnboundColumnType.Boolean;
                Fijado.VisibleIndex = 0;
                Fijado.Width = 35;
                Fijado.Visible = true;
                Fijado.OptionsColumn.AllowEdit = true;
                this.gvLibranza.Columns.Add(Fijado);

                //Proposito
                GridColumn Proposito = new GridColumn();
                Proposito.FieldName = this._unboundPrefix + "EC_Proposito";
                Proposito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Proposito");
                Proposito.UnboundType = UnboundColumnType.String;
                Proposito.VisibleIndex = 1;
                Proposito.Width = 50;
                Proposito.Visible = true;
                Proposito.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(Proposito);

                //Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.String;
                Libranza.VisibleIndex = 2;
                Libranza.Width = 65;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(Libranza);

                //Altura
                GridColumn Altura = new GridColumn();
                Altura.FieldName = this._unboundPrefix + "PrimeraCuota";
                Altura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Altura");
                Altura.UnboundType = UnboundColumnType.Integer;
                Altura.VisibleIndex = 3;
                Altura.Width = 55;
                Altura.Visible = true;
                Altura.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(Altura);

                //FechaVto(Vencimiento)
                GridColumn FechaVto = new GridColumn();
                FechaVto.FieldName = this._unboundPrefix + "FechaVto";
                FechaVto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVto");
                FechaVto.UnboundType = UnboundColumnType.DateTime;
                FechaVto.VisibleIndex = 4;
                FechaVto.Width = 80;
                FechaVto.Visible = true;
                FechaVto.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(FechaVto);

                //FechaUltPago(Ult Pago)
                GridColumn FechaUltPago = new GridColumn();
                FechaUltPago.FieldName = this._unboundPrefix + "FechaUltPago";
                FechaUltPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaUltPago");
                FechaUltPago.UnboundType = UnboundColumnType.DateTime;
                FechaUltPago.VisibleIndex = 5;
                FechaUltPago.Width = 80;
                FechaUltPago.Visible = true;
                FechaUltPago.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(FechaUltPago);

                //DiasMora
                GridColumn DiasMora = new GridColumn();
                DiasMora.FieldName = this._unboundPrefix + "DiasMora";
                DiasMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DiasMora");
                DiasMora.UnboundType = UnboundColumnType.Integer;
                DiasMora.VisibleIndex = 6;
                DiasMora.Width = 60;
                DiasMora.Visible = true;
                DiasMora.OptionsColumn.AllowEdit = false;
                this.gvLibranza.Columns.Add(DiasMora);

                //VlrCapital
                GridColumn VlrSaldoCapital = new GridColumn();
                VlrSaldoCapital.FieldName = this._unboundPrefix + "VlrCapital";
                VlrSaldoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                VlrSaldoCapital.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoCapital.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoCapital.VisibleIndex = 7;
                VlrSaldoCapital.Width = 120;
                VlrSaldoCapital.Visible = true;
                VlrSaldoCapital.OptionsColumn.AllowEdit = false;
                VlrSaldoCapital.ColumnEdit = this.editSpin;
                this.gvLibranza.Columns.Add(VlrSaldoCapital);

                //VlrSaldoSeguro
                GridColumn VlrSaldoSeguro = new GridColumn();
                VlrSaldoSeguro.FieldName = this._unboundPrefix + "VlrSaldoSeguro";
                VlrSaldoSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoSeguro");
                VlrSaldoSeguro.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoSeguro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoSeguro.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoSeguro.VisibleIndex = 8;
                VlrSaldoSeguro.Width = 120;
                VlrSaldoSeguro.Visible = true;
                VlrSaldoSeguro.OptionsColumn.AllowEdit = false;
                VlrSaldoSeguro.ColumnEdit = this.editSpin;
                this.gvLibranza.Columns.Add(VlrSaldoSeguro);

                //VlrSaldoVencido
                GridColumn VlrSaldoVencido = new GridColumn();
                VlrSaldoVencido.FieldName = this._unboundPrefix + "VlrSaldoVencido";
                VlrSaldoVencido.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoVencido");
                VlrSaldoVencido.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoVencido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoVencido.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoVencido.VisibleIndex = 9;
                VlrSaldoVencido.Width = 120;
                VlrSaldoVencido.Visible = true;
                VlrSaldoVencido.OptionsColumn.AllowEdit = false;
                VlrSaldoVencido.ColumnEdit = this.editSpin;
                this.gvLibranza.Columns.Add(VlrSaldoVencido);

                //VlrSaldoOtros
                GridColumn VlrSaldoOtros = new GridColumn();
                VlrSaldoOtros.FieldName = this._unboundPrefix + "VlrSaldoOtros";
                VlrSaldoOtros.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoOtros");
                VlrSaldoOtros.UnboundType = UnboundColumnType.Decimal;
                VlrSaldoOtros.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSaldoOtros.AppearanceCell.Options.UseTextOptions = true;
                VlrSaldoOtros.VisibleIndex = 10;
                VlrSaldoOtros.Width = 120;
                VlrSaldoOtros.Visible = true;
                VlrSaldoOtros.OptionsColumn.AllowEdit = false;
                VlrSaldoOtros.ColumnEdit = this.editSpin;
                this.gvLibranza.Columns.Add(VlrSaldoOtros);
                #endregion
                #region Grilla Inferior

                //PagoInd
                GridColumn pagarInd = new GridColumn();
                pagarInd.FieldName = this._unboundPrefix + "PagoInd";
                pagarInd.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PagoInd");
                pagarInd.UnboundType = UnboundColumnType.Boolean;
                pagarInd.VisibleIndex = 0;
                pagarInd.Width = 35;
                pagarInd.Visible = true;
                pagarInd.OptionsColumn.AllowEdit = true;
                this.gvComponentes.Columns.Add(pagarInd);

                //ComponenteCarteraID
                GridColumn componenteCarteraID = new GridColumn();
                componenteCarteraID.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                componenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComponenteCarteraID");
                componenteCarteraID.UnboundType = UnboundColumnType.String;
                componenteCarteraID.VisibleIndex = 1;
                componenteCarteraID.Width = 50;
                componenteCarteraID.Visible = true;
                componenteCarteraID.OptionsColumn.AllowEdit = false;
                componenteCarteraID.ColumnEdit = this.editBtnGrid;
                this.gvComponentes.Columns.Add(componenteCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this._unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 2;
                descripcion.Width = 50;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(descripcion);

                //PagoValor
                GridColumn pagoValor = new GridColumn();
                pagoValor.FieldName = this._unboundPrefix + "PagoValor";
                pagoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PagoValor");
                pagoValor.UnboundType = UnboundColumnType.Decimal;
                pagoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                pagoValor.AppearanceCell.Options.UseTextOptions = true;
                pagoValor.VisibleIndex = 3;
                pagoValor.Width = 100;
                pagoValor.Visible = true;
                pagoValor.OptionsColumn.AllowEdit = false;
                pagoValor.ColumnEdit = this.editSpin;
                this.gvComponentes.Columns.Add(pagoValor);

                //Abono
                GridColumn abonoValor = new GridColumn();
                abonoValor.FieldName = this._unboundPrefix + "AbonoValor";
                abonoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPagarCalculado");
                abonoValor.UnboundType = UnboundColumnType.Decimal;
                abonoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                abonoValor.AppearanceCell.Options.UseTextOptions = true;
                abonoValor.VisibleIndex = 4;
                abonoValor.Width = 150;
                abonoValor.Visible = true;
                abonoValor.OptionsColumn.AllowEdit = false;
                abonoValor.ColumnEdit = this.editSpin;
                this.gvComponentes.Columns.Add(abonoValor);

                //VlrPagar
                GridColumn vlrPagar = new GridColumn();
                vlrPagar.FieldName = this._unboundPrefix + "VlrPagar";
                vlrPagar.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPagar");
                vlrPagar.UnboundType = UnboundColumnType.Decimal;
                vlrPagar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPagar.AppearanceCell.Options.UseTextOptions = true;
                vlrPagar.VisibleIndex = 5;
                vlrPagar.Width = 150;
                vlrPagar.Visible = true;
                vlrPagar.OptionsColumn.AllowEdit = this._allowEditValuesInd;
                vlrPagar.ColumnEdit = this.editSpin;
                this.gvComponentes.Columns.Add(vlrPagar);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "EstadoCuenta.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Limpia los datos del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            
            this.libranzaID = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.txtSaldoMora.Text = "0";
            this.txtSaldoPendiente.Text = "0";
            this.txtSaldoTotal.Text = "0";
            this.txtCuotasMora.Text = string.Empty;
            this.txtPlazo.Text = string.Empty;
            this.txtVlrCuota.Text = "0";
            this.txtVlrOtrosCompon.Text = "0";
            this.txtVlrAbono.Text = "0";
            this.txtSeguroVida.Text = "0";

            //Variables
            this.numDocCredito = 0;
            this.numeroDocEstCuenta = 0;
            this.cliente = null;
            this.credito = null;
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.saldosTotales = new List<DTO_ccEstadoCuentaComponentes>();
            this.infoCreditoTemp = new DTO_InfoCredito();
            this.infoCredito = new DTO_InfoCredito();
            this.infoSaldos = new DTO_InfoCredito();
            this.infoPagos = new DTO_InfoCredito();
            this._diasFNC = 0;
            this._fechaInicioFNC = null;

            //Combos
            this.lkpProposito.EditValue = 0;
            this.lkpPoliza.EditValue = "0";
            this.lkp_Libranzas.Properties.DataSource = this.creditos;
            this.cmbCuota.SelectedIndex = -1;
            this.cmbCuota.Items.Clear();
            this.cmbEstado.EditValue = "";
            this.estadoCuentaHistoria = null;

            //Deshabilitar controles
            this.lkpProposito.Enabled = false;
            this.lkpPoliza.Enabled = false;
            this.btnCuotasPendientes.Enabled = false;
            this.btnCuotasPagadas.Enabled = false;
            this.btnMovimiento.Enabled = false;
            this.btnAnexos.Enabled = false;
            this.lblCobroJurid.Visible = false;

            this.LoadGridCompData();
            this.gcLibranza.DataSource = this.creditos;
            this.gvLibranza.RefreshData();
            this.masterCliente.Focus();

            this.validate = true;
        }

        /// <summary>
        /// Funcion que no permite editar los controles de la pantalla
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los controles</param>
        private void EnableHeader(bool enabled)
        {
            this.lkpProposito.Enabled = enabled;
            this.dt_FechaCorte.Enabled = enabled;
            this.dt_FechaLimite.Enabled = enabled;

            if(enabled && !this.credito.EC_FijadoInd.Value.Value)
                this.lkpProposito.Enabled = false;

            //this.cmbCuota.Enabled = enabled;
            //this.chkEstadoFijado.Enabled = enabled;
        }

        /// <summary>
        /// Carga la información de un componente
        /// </summary>
        /// <param name="componenteCarteraID">Identificador del componente</param>
        private DTO_ccCarteraComponente GetComponenteInfo(string componenteCarteraID)
        {
            if (!this.componentesAll.ContainsKey(componenteCarteraID))
            {
                DTO_ccCarteraComponente comp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, componenteCarteraID, true);
                this.componentesAll.Add(componenteCarteraID, comp);
            }

            return this.componentesAll[componenteCarteraID];
        }

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddComponente()
        {
            try
            {
                DTO_ccEstadoCuentaComponentes newComp = new DTO_ccEstadoCuentaComponentes();
                newComp.PagoInd.Value = true;
                newComp.ComponenteCarteraID.Value = string.Empty;
                newComp.Descriptivo.Value = string.Empty;
                newComp.PagoValor.Value = 0;
                newComp.AbonoValor.Value = 0;
                newComp.VlrPagar.Value = 0;
                newComp.Editable.Value = true;

                this.saldosTotales.Add(newComp);
                this.gcComponentes.DataSource = this.saldosTotales;
                this.gvComponentes.PostEditor();
                this.gvComponentes.FocusedRowHandle = this.saldosTotales.Count - 1;

                this.gvComponentes.Columns[this._unboundPrefix + "PagoInd"].OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns[this._unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                //this.gvDocument.Columns[this._unboundPrefix + "VlrPagar"].OptionsColumn.AllowEdit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddComponente"));
            }
        }

        /// <summary>
        /// Valida una fila del plan de pagos
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_Componentes(int fila)
        {
            try
            {
                if (this.validate)
                {
                    bool validComp = _bc.ValidGridCell(this.gvComponentes, this._unboundPrefix, fila, "ComponenteCarteraID", false, true, false, AppMasters.ccCarteraComponente);
                    bool validVlr = _bc.ValidGridCellValue(this.gvComponentes, this._unboundPrefix, fila, "VlrPagar", false, true, false, false);
                   
                    this.validComponentes = validComp && validVlr;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "ValidateRow_Componentes"));
                this.validComponentes = false;
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            this.IsModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        #endregion

        #region Carga de datos

        /// <summary>
        /// Carga las libranzas del cliente obteniendo su info
        /// </summary>
        private void LoadLibranzas()
        {
            try
            {
                //Recorre cada credito
                foreach (var cred in this.creditos)
                {
                    //Variables
                    this.fechaCorte = this.dt_FechaCorte.DateTime;
                    DTO_InfoCredito info = this._bc.AdministrationModel.GetInfoCredito(cred.NumeroDoc.Value.Value, this.fechaCorte.Date);
                    List<DTO_ccCreditoPlanPagos> planPagos = new List<DTO_ccCreditoPlanPagos>();
                    List<DTO_ccSaldosComponentes> componentesVencidos = new List<DTO_ccSaldosComponentes>();
                    List<DTO_ccSaldosComponentes> componentesNoVencidos = new List<DTO_ccSaldosComponentes>();

                    #region Llena el plan de pagos y obtiene los componentes vencidos y no vencidos
                    foreach (DTO_ccCreditoPlanPagos p in info.PlanPagos)
                    {
                        List<DTO_ccSaldosComponentes> componentesCuota = (from c in info.SaldosComponentes where c.CuotaID.Value == p.CuotaID.Value select c).ToList();

                        //Revisa si la cuota esta vencida
                        decimal saldoPendiente = (from c in componentesCuota select c.CuotaSaldo.Value.Value).Sum();
                        if (saldoPendiente > 0 && p.FechaCuota.Value.Value.Date <= this.fechaCorte.Date)
                            componentesVencidos.AddRange(componentesCuota);
                        else
                            componentesNoVencidos.AddRange(componentesCuota);

                        if (saldoPendiente > 0)
                        {
                            p.VlrSaldo.Value = saldoPendiente;
                            p.VlrPagadoCuota.Value = 0;
                            planPagos.Add(p);
                        }                         
                    }
                    #endregion
                    #region Obtiene la información de saldos y mora para las cuotas no pagadas
                    if (planPagos.Count > 0)
                    {
                        DTO_ccCreditoPlanPagos pp = planPagos.First();

                        cred.PrimeraCuota.Value = pp.CuotaID.Value;
                        cred.FechaVto.Value = pp.FechaCuota.Value;

                        cred.DiasMora.Value = 0;

                        //DTO_ccSaldosComponentes compMora = (from nv in componentesVencidos where nv.ComponenteCarteraID.Value == componenteMora select nv)
                        //    .OrderBy(m => m.CuotaID.Value.Value)
                        //    .FirstOrDefault();

                        List<int> cuotasMora = (from c in planPagos
                                                where c.FechaCuota.Value.Value.Date <= this.fechaCorte.Date
                                                select c.CuotaID.Value.Value).ToList();


                        if (cuotasMora.Count > 0)
                            cred.DiasMora.Value = (this.fechaCorte - pp.FechaCuota.Value.Value).Days;

                        //Saldo capital
                        //cred.VlrCapital.Value = (from x in info.SaldosComponentes where x.ComponenteCarteraID.Value == this.componenteCapital select x.CuotaSaldo.Value).Sum();
                        cred.VlrCapital.Value = (from x in componentesNoVencidos where x.ComponenteCarteraID.Value == this.componenteCapital select x.CuotaSaldo.Value).Sum();

                        //Saldo seguro
                        //cred.VlrSaldoSeguro.Value = (from x in info.SaldosComponentes where x.ComponenteCarteraID.Value == this.componenteSeguro select x.CuotaSaldo.Value).Sum();
                        cred.VlrSaldoSeguro.Value = (from x in componentesNoVencidos where x.ComponenteCarteraID.Value == this.componenteSeguro select x.CuotaSaldo.Value).Sum();

                        //Suma componentes Vencidos con Capital y Seguro (1 y 4) que tengan PagoTotalInd en true 
                        cred.VlrSaldoVencido.Value = (from x in componentesVencidos
                                                      where x.TipoComponente.Value == (byte)TipoComponente.CapitalSolicitado ||
                                                            x.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota
                                                      select x.CuotaSaldo.Value).Sum();
                        cred.VlrSaldoVencido.Value += (from x in componentesVencidos
                                                       where x.ComponenteCarteraID.Value == componenteMora || x.ComponenteCarteraID.Value == componentePrejuridico
                                                       select x.CuotaSaldo.Value).Sum();

                        //Suma componentes Vencidos con TipoComponente diferente de 1 y 4 
                        cred.VlrSaldoOtros.Value = (from x in componentesNoVencidos
                                                    where x.TipoComponente.Value != (byte)TipoComponente.CapitalSolicitado && x.TipoComponente.Value != (byte)TipoComponente.ComponenteCuota
                                                    select x.CuotaSaldo.Value).Sum();


                        cred.VlrSaldo.Value = (from x in info.SaldosComponentes select x.CuotaSaldo.Value).Sum();
                        cred.FechaUltPago.Value = info.FechaUltPago.Value;

                        //Revisa el estado de la poliza
                        DTO_ccSaldosComponentes compPoliza = (from nv in componentesVencidos where nv.ComponenteCarteraID.Value == this.componenteSeguro select nv).FirstOrDefault();
                        if (info.SaldosComponentes.Any(x => x.ComponenteCarteraID.Value == this.componenteSeguro))
                        {
                            this.lkpPoliza.EditValue = "1";
                            this.lkpPoliza.Enabled = true;
                            this.compSeguroExist = true;
                        }                            
                        else
                        {                           
                            this.lkpPoliza.EditValue = "0";
                            this.lkpPoliza.Enabled = false;
                            this.compSeguroExist = false;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Funcion que carga la informacion del documento
        /// </summary>
        private void LoadCreditoInfo()
        {
            try
            {
                //Trae la información del estado de cuenta
                if (this.credito.DocEstadoCuenta.Value.HasValue)
                {
                    this.estadoCuenta = _bc.AdministrationModel.EstadoCuenta_GetAll(this.credito.DocEstadoCuenta.Value.Value);
                    this.numeroDocEstCuenta = this.credito.DocEstadoCuenta.Value.Value;
                }
                else
                {
                    this.estadoCuenta = null;
                    this.numeroDocEstCuenta = 0;
                }

                this.numDocCredito = this.credito.NumeroDoc.Value.Value;

                //Revisa si tiene estado de cuenta previo y carga la grilla de los componentes y el header
                if (this.estadoCuenta != null)
                {                  
                    this.estadoCuentaHistoria = this.estadoCuenta.EstadoCuentaHistoria;

                    #region Asigna la información del estado de cuenta

                    //Saldos totales
                    this.saldosTotales = this.estadoCuenta.EstadoCuentaComponentes;

                    decimal saldo = 0;
                    decimal pago = 0;
                    decimal abono = 0;
                    foreach (var s in this.saldosTotales)
                    {
                        saldo = s.SaldoValor.Value.Value;
                        pago = s.PagoValor.Value.Value;
                        abono = s.AbonoValor.Value.Value;

                        s.PagoValor.Value = saldo;
                        s.AbonoValor.Value = pago;
                        s.VlrPagar.Value = abono;
                    }
                    this.gcComponentes.DataSource = this.saldosTotales;

                    //Estado cuenta historia
                    this.lkpProposito.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_Proposito.Value.Value;
                    this.dt_FechaCorte.DateTime = this.estadoCuenta.EstadoCuentaHistoria.EC_Fecha.Value.Value;
                    this.dt_FechaLimite.DateTime = this.estadoCuenta.EstadoCuentaHistoria.EC_FechaLimite.Value.Value;
                    this.primeraCuota = this.estadoCuenta.EstadoCuentaHistoria.EC_Altura.Value.Value;
                    this.txtCuotasMora.Text = this.estadoCuenta.EstadoCuentaHistoria.EC_CuotasMora.Value.Value.ToString();
                    this.cmbCuota.Text = this.estadoCuenta.EstadoCuentaHistoria.EC_PrimeraCtaPagada.Value.Value.ToString();
                    this.fechaCorte = this.dt_FechaCorte.DateTime;

                    //Valores
                    this.txtSaldoPendiente.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_SaldoPend.Value.Value;
                    this.txtSaldoTotal.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_SaldoTotal.Value.Value;
                    this.txtSaldoMora.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_SaldoMora.Value.Value;

                    //Recarga Libranzas
                    this.LoadLibranzas();

                    //Poliza
                    if (this.estadoCuenta.EstadoCuentaHistoria.EC_PolizaMvto.Value.HasValue)
                        this.lkpPoliza.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_PolizaMvto.Value.ToString();
                    else
                        this.lkpPoliza.EditValue = this.compSeguroExist? "1" : "0";

                    if (this.estadoCuenta.EstadoCuentaHistoria.EC_PolizaMvto.Value.HasValue)
                        this.txtVlrAbono.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_ValorAbono.Value.Value;
                    else
                        this.txtVlrAbono.EditValue = 0;

                    if (this.estadoCuenta.EstadoCuentaHistoria.EC_PolizaMvto.Value.HasValue)
                        this.txtSeguroVida.EditValue = this.estadoCuenta.EstadoCuentaHistoria.EC_SeguroVida.Value.Value;
                    else
                        this.txtSeguroVida.EditValue = 0;

                    #endregion

                    if (this.sector == SectorCartera.Financiero && (Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Prepago ||
                        Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RecogeSaldo || Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                        this.lkpPoliza.Enabled = true;               

                    //Información del crédito
                    this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(this.numDocCredito, this.dt_FechaCorte.DateTime);
                    this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);

                    //Carga la información de pagos, saldos y los valores calculados
                    this.LoadComponentesSaldos(true);
                    this.LoadComponentesPagos();
                }
                else
                {                 
                    //Información del crédito
                    this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(this.numDocCredito, this.dt_FechaCorte.DateTime);
                    this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);
                }

                if (this.estadoCuenta == null)
                {
                    //Carga la grilla de componentes
                    this.LoadGridCompData();
                }

                //Habilita la poliza  si tiene seguro
                this.compSeguroExist = this.saldosTotales.Any(x => x.ComponenteCarteraID.Value == this.componenteSeguro);

                if (this.estadoCuenta == null)
                {
                    #region Carga la informacion del crédito cuando no hay EC asignado

                    //Estado cuenta historia
                    this.lkpPoliza.EditValue = this.compSeguroExist ? "1" : "0";
                    this.lkpProposito.EditValue = 0;

                    //Valores
                    this.txtSaldoPendiente.EditValue = 0;
                    this.txtSaldoTotal.EditValue = 0;

                    //Mora
                    decimal saldoMora = 0;
                    List<int> cuotasMora = (from c in this.infoCredito.PlanPagos
                                            where c.FechaCuota.Value.Value.Date <= this.fechaCorte.Date
                                            select c.CuotaID.Value.Value).ToList();

                    foreach (int cuotaMora in cuotasMora)
                    {
                        saldoMora += (from c in this.infoCredito.SaldosComponentes
                                      where c.CuotaID.Value == cuotaMora
                                        && (c.TipoComponente.Value == (byte)TipoComponente.CapitalSolicitado || c.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota)
                                      select c.CuotaSaldo.Value.Value).Sum();
                    }
                    this.txtSaldoMora.EditValue = saldoMora;
                    this.txtCuotasMora.Text = cuotasMora.Count().ToString();

                    //Poliza                 
                    this.lkpPoliza.EditValue = this.compSeguroExist ? "1" : "0"; 
                    //this.txtVlrAbono.EditValue = 0;
                    this.txtSeguroVida.EditValue = 0;
                    if (this.sector == SectorCartera.Financiero && (Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Prepago ||
                        Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RecogeSaldo || Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                        this.lkpPoliza.Enabled = true;
                    else
                        this.lkpPoliza.Enabled = false;

                    //Carga la información de pagos, saldos y los valores calculados
                    this.LoadComponentesSaldos(true);
                    this.LoadComponentesPagos();

                    //Combo de primeras cuotas
                    this.cmbCuota.Items.Clear();
                    List<int> cuotas = (from pp in this.infoSaldos.PlanPagos select pp.CuotaID.Value.Value).Take(1).ToList();
                    if (cuotas.Count > 0)
                    {
                        foreach (int c in cuotas)
                            this.cmbCuota.Items.Add(c);

                        this.cmbCuota.Text = cuotas[0].ToString();
                    }

                    #endregion
                }

                this.btnAnexos.Enabled = true;
                if (infoCredito.EstadoCompraCartera.Value != 0)
                {
                    if (infoCredito.EstadoCompraCartera.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        //this.EnableHeader(false);
                        //string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaComprado);
                        //MessageBox.Show(string.Format(msg, infoCredito.LibranzaCompraCartera.Value.ToString()));
                    }
                    //else if (infoCredito.ActFlujoSolicitudCompraCartera.Value.Trim() != this.actFlujoDigitacionID.Trim())
                    //{
                    //    //this.EnableHeader(false);
                    //    //string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaEnCompra);
                    //    //MessageBox.Show(string.Format(msg, infoCredito.LibranzaCompraCartera.Value.ToString()));
                    //}
                    else
                    {
                        this.EnableHeader(true);
                    }
                }
                else
                    this.EnableHeader(true);

                DTO_ccCobranzaEstado cobranzaEstado = (DTO_ccCobranzaEstado)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCobranzaEstado, false, this.credito.CobranzaEstadoID.Value, true);
                if (cobranzaEstado != null)
                {
                    if (cobranzaEstado.CobroJuridicoInd.Value.Value)
                        this.lblCobroJurid.Visible = true;
                    else
                        this.lblCobroJurid.Visible = false;
                }
                else
                    this.lblCobroJurid.Visible = false;

                //Información del credito
                this.txtPlazo.Text = this.credito.Plazo.Value.ToString();
                this.txtVlrCuota.EditValue = this.credito.VlrCuota.Value;

                this.btnCuotasPagadas.Enabled = true;
                this.btnCuotasPendientes.Enabled = true;
                this.btnMovimiento.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "LoadCreditoInfo"));
            }
        }

        /// <summary>
        /// Carga la informacion de acuerdo al proposito
        /// </summary>
        private void LoadProposito(int primeraCuota)
        {
            try
            {
                byte proposito = this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) != 0 ? Convert.ToByte(this.lkpProposito.EditValue) : (byte)PropositoEstadoCuenta.Proyeccion;

                #region Inicia los controles y variables que se calculan de acuerdo al porpósito

                this.lkpPoliza.Enabled = false;
                this.txtSeguroVida.EditValue = 0;
                this.vlrRevSeg = 0;
                this.vlrRevIntSeg = 0;

                this.txtVlrOtrosCompon.Properties.ReadOnly = true;
                this.txtVlrOtrosCompon.Properties.ReadOnly = true;
                this.txtVlrOtrosCompon.Properties.ReadOnly = true;

                this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);
                this.infoCreditoTemp.PlanPagos = this.infoCreditoTemp.PlanPagos.Where(x => x.CuotaID.Value >= primeraCuota).ToList();
                this.infoCreditoTemp.SaldosComponentes = this.infoCreditoTemp.SaldosComponentes.Where(x => x.CuotaID.Value >= primeraCuota).ToList();

                #endregion
                #region Calcula las fechas de pagos de cada cuota

                Dictionary<int, DateTime> pagosFecha = new Dictionary<int, DateTime>();
                for (int i = 0; i < this.infoCreditoTemp.PlanPagos.Count; ++i)
                {
                    DTO_ccCreditoPlanPagos pago = this.infoCreditoTemp.PlanPagos[i];
                    pagosFecha.Add(pago.CuotaID.Value.Value, pago.FechaCuota.Value.Value.Date);
                }

                #endregion
                #region Asigna la información segun el propósito
                if (proposito == (byte)PropositoEstadoCuenta.Proyeccion)
                {
                    #region Proyección

                    foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                        comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                  
                    #endregion
                }
                else if (proposito == (byte)PropositoEstadoCuenta.Prepago || proposito == (byte)PropositoEstadoCuenta.RecogeSaldo ||
                         proposito == (byte)PropositoEstadoCuenta.RestructuracionAbono || proposito == (byte)PropositoEstadoCuenta.RestructuracionPlazo)
                {
                    #region Prepago-RecogeSaldo-AbonoCapital

                    this.lkpPoliza.Enabled = true;
                    DTO_ccSaldosComponentes ccRevSeguro = null;
                    DTO_ccSaldosComponentes ccIntNoCausado = null;
                    DTO_ccSaldosComponentes ccFinanciaIntCapital = null;
                    DTO_ccSaldosComponentes ccFinanciaIntSeguro = null;
                    DTO_ccSaldosComponentes ccFinanciaSeguro = null;

                    byte poliza = Convert.ToByte(this.lkpPoliza.EditValue);
                    foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                    {
                        #region Cálculo de los pagos

                        if (this.fechaCorte.Date >= pagosFecha[comp.CuotaID.Value.Value].Date  && comp.CuotaID.Value >= primeraCuota)
                        {
                            //Pago de componentes atrasados
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        else if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota )
                        {
                            //Componentes de pago total
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        else if ((comp.ComponenteCarteraID.Value == this.componenteInteres || comp.ComponenteCarteraID.Value == this.componenteInteresSeguro)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                            && comp.CuotaID.Value >= primeraCuota)
                        {
                            //Componente de interes para el periodo actual
                            int diasACausar = this.fechaCorte.Day;
                            decimal vlrComp = comp.CuotaSaldo.Value.Value;
                            decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                            ccIntNoCausado = this.LoadComponenteExtra(this.componenteInteresNoCausado, comp.CuotaID.Value.Value, vlrCausado);
                        }
                        else if ((comp.ComponenteCarteraID.Value == this.componenteAportes)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                        {
                            //Aportes y seguro del periodo actual
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        else if(this.sector != SectorCartera.Financiero 
                            && (comp.ComponenteCarteraID.Value == this.componenteAportes)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                        {
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        #endregion
                        #region Poliza

                        if (this.sector == SectorCartera.Financiero && this.lkpPoliza.Enabled  && comp.CuotaID.Value >= primeraCuota)
                        {
                            #region Componente seguro
                            if (comp.ComponenteCarteraID.Value == componenteSeguro)
                            {
                                //Para paga ya se incluye con el total valor
                                //comp.AbonoValor.Value = 0;
                                if (poliza == 1)
                                {
                                    //Continua
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                }
                                else if (poliza == 2)
                                {
                                    //Paga
                                    this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                }
                                else if (poliza == 3)
                                {
                                    //Revoca
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                    else
                                    {
                                        //decimal valRevTemp = this.porcRevocatoriaSeguro > 0 ? comp.CuotaSaldo.Value.Value * this.porcRevocatoriaSeguro / 100 : 0;
                                        //valRevTemp = Convert.ToInt32(valRevTemp);
                                        //this.vlrRevSeg += valRevTemp;
                                    }                                    
                                }
                            }
                            #endregion
                            #region Componente interes seguro
                            if (comp.ComponenteCarteraID.Value == componenteInteresSeguro)
                            {
                                //Para paga ya se incluye con el total valor
                                if (poliza == 1)
                                {
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevIntSeg += comp.CuotaSaldo.Value.Value;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    //Componentes extras
                    #region Poliza EC
                    if (this.sector == SectorCartera.Financiero && vlrRevSeg > 0)
                    {
                        if (poliza == 3) //Revoca
                        {
                            ccRevSeguro = this.LoadComponenteExtra(this.componentePolizaEC, primeraCuota, this.vlrRevSeg);
                            this.infoCreditoTemp.SaldosComponentes.Add(ccRevSeguro);
                        }                        
                    }
                    #endregion
                    #region Interes financiado no causado

                    DTO_ccCreditoPlanPagos proxCuota = this.infoCreditoTemp.PlanPagos.Where(w => w.FechaCuota.Value.Value.Date > this.fechaCorte.Date)
                        .OrderBy(o => o.FechaCuota.Value.Value).FirstOrDefault();

                    if (proxCuota != null)
                    {
                        int diaCuota = proxCuota.FechaCuota.Value.Value.Day;
                        if (diaCuota == 31)
                            diaCuota = 30;

                        int diaCierre = this.fechaCorte.Day;
                        if (diaCierre == 31)
                            diaCierre = 30;

                        int difDias = 0;
                        if (diaCierre > diaCuota)
                            difDias = diaCierre - diaCuota;
                        else if (diaCierre < diaCuota)
                            difDias = 30 - diaCuota + diaCierre;

                        int divisor = !this.credito.PeriodoPago.Value.HasValue || this.credito.PeriodoPago.Value.Value == (byte)PeriodoPago.PrimeraQuincena ? 30 : 15;
                        DateTime newfechaCuota = proxCuota.FechaCuota.Value.Value;
                        if (divisor == 30) //Mensual
                            newfechaCuota = proxCuota.FechaCuota.Value.Value.AddMonths(-1);
                        else //Quincenal
                            newfechaCuota = proxCuota.FechaCuota.Value.Value.AddDays(-15);
                        newfechaCuota = newfechaCuota.AddDays(1);
                        difDias = (this.fechaCorte.Date-newfechaCuota.Date).Days >= 0? (this.fechaCorte.Date-newfechaCuota.Date).Days+1 : 0;                       

                        if (difDias != 0)
                        {
                            this._diasFNC = Convert.ToByte(difDias);
                            this._fechaInicioFNC = newfechaCuota;

                            //Interes Capital 
                            if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoCapital))
                            {
                                DTO_ccSaldosComponentes compInteres = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteInteres).FirstOrDefault();

                                if (compInteres != null && compInteres.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compInteres.CuotaInicial.Value.Value - compInteres.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaIntCap = (compInteres.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaIntCapital = this.LoadComponenteExtra(this.componenteInteresNoCausadoCapital, primeraCuota, Convert.ToInt32(vlrFinanciaIntCap));
                                    if(this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntCapital);
                                }
                            }

                            //Interes Seguro 
                            if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoSeguro) && (poliza == 2 || poliza == 3))
                            {
                                DTO_ccSaldosComponentes compInteresSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteInteresSeguro).FirstOrDefault();

                                if (compInteresSeguro!= null && compInteresSeguro.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compInteresSeguro.CuotaInicial.Value.Value - compInteresSeguro.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaIntSeg = (compInteresSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaIntSeguro = this.LoadComponenteExtra(this.componenteInteresNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaIntSeg));
                                    if (this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntSeguro);
                                }
                            }

                            //Seguro 
                            if (!string.IsNullOrWhiteSpace(this.componenteNoCausadoSeguro) && poliza == 3)
                            {
                                DTO_ccSaldosComponentes compSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteSeguro).FirstOrDefault();

                                if (compSeguro != null && compSeguro.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compSeguro.CuotaInicial.Value.Value - compSeguro.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaSeg = (compSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaSeguro = this.LoadComponenteExtra(this.componenteNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaSeg));
                                    if (this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaSeguro);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Sanción
                    if (!String.IsNullOrWhiteSpace(this.componenteSancionID))
                    {
                        DTO_ccCreditoPlanPagos cuotaSancion = this.LoadCuotaSancion(this.infoCreditoTemp.PlanPagos.Last());
                        DTO_ccSaldosComponentes cSancion = this.LoadComponenteExtra(this.componenteSancionID, cuotaSancion.CuotaID.Value.Value, cuotaSancion.VlrCuota.Value.Value);

                        this.infoCreditoTemp.PlanPagos.Add(cuotaSancion);
                        this.infoCreditoTemp.SaldosComponentes.Add(cSancion);
                    }

                    #endregion

                    #endregion
                }
                else if (proposito == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                {
                    #region Cobro jurídico

                    decimal vlrPolizaCap = 0;
                    decimal vlrAbonoPoliza = 0;
                    DTO_ccPolizaEstado polizaCJ = _bc.AdministrationModel.PolizaEstado_GetLastPoliza(this.credito.NumeroDoc.Value.Value, this.credito.Libranza.Value.Value);

                    //Filtra los datos
                    this.infoCreditoTemp.SaldosComponentes.RemoveAll(s => s.ComponenteCarteraID.Value == this.componenteMora);
                    this.infoCreditoTemp.PlanPagos.ForEach(pp =>
                    {
                        pp.VlrMoraLiquida.Value = 0; 
                        pp.VlrMoraPago.Value = 0;
                    });

                    foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                    {
                        if (comp.CuotaID.Value >= primeraCuota)
                        {
                            //Pago del capital
                            if (comp.ComponenteCarteraID.Value == this.componenteCapital)
                            {
                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                            }
                            else if (comp.ComponenteCarteraID.Value == this.componenteSeguro)
                            {
                                if (polizaCJ != null && (polizaCJ.FechaLiqSeguro.Value.Value.Date > this.fechaCorte.Date || comp.CuotaID.Value.Value < polizaCJ.Cuota1Financia.Value.Value))
                                    vlrPolizaCap += comp.CuotaSaldo.Value.Value;
                                else
                                    vlrAbonoPoliza += comp.CuotaSaldo.Value.Value;
                            }
                        }
                    }

                    //ComponentesExtras
                    #region Capital póliza
                     
                    if (polizaCJ != null)
                    {
                        DTO_ccSaldosComponentes compCap = this.infoCreditoTemp.SaldosComponentes.Where(w => w.ComponenteCarteraID.Value == this.componenteCapital).Last();
                        List<DTO_ccSaldosComponentes> compsSeg = this.infoCreditoTemp.SaldosComponentes.Where(w => w.ComponenteCarteraID.Value == this.componenteSeguro).ToList();

                        decimal saldoSeguro = compsSeg.Sum(c => c.CuotaSaldo.Value.Value);
                        decimal vlrDif = saldoSeguro - polizaCJ.VlrPoliza.Value.Value;

                        ////Limpia el valor a pagar del seguro
                        //compsSeg.ForEach(seg => seg.AbonoValor.Value = 0);

                        //if(vlrDif >= 0)
                        //{
                        //    compCap.AbonoValor.Value = compCap.AbonoValor.Value + vlrDif;
                        //    compsSeg.Last().AbonoValor.Value = polizaCJ.VlrPoliza.Value.Value;
                        //}
                        //else
                        //{
                        //    compCap.AbonoValor.Value = compCap.AbonoValor.Value;
                        //    compsSeg.Last().AbonoValor.Value = saldoSeguro;
                        //}
                    }

                    #endregion

                    #endregion
                }
                else if (proposito == (byte)PropositoEstadoCuenta.Desistimiento)
                {
                    #region Desistimiento

                    foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                        comp.AbonoValor.Value = 0;

                    #endregion
                }
                else if (proposito == (byte)PropositoEstadoCuenta.CancelaPoliza)
                {

                    #region Cancelacion Poliza (CancelaPoliza)

                    //Poliza
                    if (this.credito.Poliza.Value != null && this.credito.VlrPoliza.Value != null && this.credito.VlrPoliza.Value != 0)
                        this.lkpPoliza.Enabled = true;

                    DTO_ccSaldosComponentes ccRevSeguro = null;
                    DTO_ccSaldosComponentes ccIntNoCausado = null;
                    DTO_ccSaldosComponentes ccFinanciaIntCapital = null;
                    DTO_ccSaldosComponentes ccFinanciaIntSeguro = null;
                    DTO_ccSaldosComponentes ccFinanciaSeguro = null;

                    byte poliza = Convert.ToByte(this.lkpPoliza.EditValue);
                    decimal vlrCapitalVencido = 0;
                    foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                    {
                        #region Cálculo de los pagos

                        if (this.fechaCorte >= pagosFecha[comp.CuotaID.Value.Value]  && comp.CuotaID.Value >= primeraCuota)
                        {
                            //Pago de componentes atrasados
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                            if (comp.ComponenteCarteraID.Value == this.componenteCapital || comp.ComponenteCarteraID.Value == this.componenteInteres)
                                vlrCapitalVencido += comp.CuotaSaldo.Value.Value;
                        }
                        else if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota)
                        {
                            //Componentes de pago total
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        else if ((comp.ComponenteCarteraID.Value == this.componenteInteres || comp.ComponenteCarteraID.Value == this.componenteInteresSeguro)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                            && comp.CuotaID.Value >= primeraCuota)
                        {
                            //Componente de interes para el periodo actual
                            int diasACausar = this.fechaCorte.Day;
                            decimal vlrComp = comp.CuotaSaldo.Value.Value;
                            decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                            ccIntNoCausado = this.LoadComponenteExtra(this.componenteInteresNoCausado, comp.CuotaID.Value.Value, vlrCausado);
                        }
                        else if ((comp.ComponenteCarteraID.Value == this.componenteAportes)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                        {
                            //Aportes y seguro del periodo actual
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        else if (this.sector != SectorCartera.Financiero
                            && (comp.ComponenteCarteraID.Value == this.componenteAportes)
                            && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                            && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                        {
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                        #endregion
                        #region Poliza

                        if (this.sector == SectorCartera.Financiero && this.lkpPoliza.Enabled  && comp.CuotaID.Value >= primeraCuota)
                        {
                            #region Componente seguro
                            if (comp.ComponenteCarteraID.Value == componenteSeguro)
                            {
                                //Para paga ya se incluye con el total valor
                                //comp.AbonoValor.Value = 0;
                                if (poliza == 1)
                                {
                                    //Continua
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                }
                                else if (poliza == 2)
                                {
                                    //Paga
                                    this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                }
                                else if (poliza == 3)
                                {
                                    //Revoca
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                    else
                                    {
                                        //decimal valRevTemp = this.porcRevocatoriaSeguro > 0 ? comp.CuotaSaldo.Value.Value * this.porcRevocatoriaSeguro / 100 : 0;
                                        //valRevTemp = Convert.ToInt32(valRevTemp);
                                        //this.vlrRevSeg += valRevTemp;
                                    }
                                }
                            }
                            #endregion
                            #region Componente interes seguro
                            if (comp.ComponenteCarteraID.Value == componenteInteresSeguro)
                            {
                                //Para paga ya se incluye con el total valor
                                if (poliza == 1)
                                {
                                    DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                    if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                        this.vlrRevIntSeg += comp.CuotaSaldo.Value.Value;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    //Componentes extras
                    #region Poliza EC
                    if (this.sector == SectorCartera.Financiero && vlrRevSeg > 0)
                    {
                        if (poliza == 3) //Revoca
                        {
                            ccRevSeguro = this.LoadComponenteExtra(this.componentePolizaEC, primeraCuota, this.vlrRevSeg);
                            this.infoCreditoTemp.SaldosComponentes.Add(ccRevSeguro);
                        }
                    }
                    #endregion
                    #region Interes financiado no causado

                    DTO_ccCreditoPlanPagos proxCuota = this.infoCreditoTemp.PlanPagos.Where(w => w.FechaCuota.Value.Value.Date > this.fechaCorte.Date)
                        .OrderBy(o => o.FechaCuota.Value.Value).FirstOrDefault();

                    if (proxCuota != null)
                    {
                        int diaCuota = proxCuota.FechaCuota.Value.Value.Day;
                        if (diaCuota == 31)
                            diaCuota = 30;

                        int diaCierre = this.fechaCorte.Day;
                        if (diaCierre == 31)
                            diaCierre = 30;

                        int difDias = 0;
                        if (diaCierre > diaCuota)
                        {
                            difDias = diaCierre - diaCuota;
                        }
                        else if (diaCierre < diaCuota)
                        {
                            difDias = 30 - diaCuota + diaCierre;
                        }

                        if (difDias != 0)
                        {
                            int divisor = !this.credito.PeriodoPago.Value.HasValue || this.credito.PeriodoPago.Value.Value == (byte)PeriodoPago.PrimeraQuincena ? 30 : 15;

                            //Interes Capital 
                            if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoCapital))
                            {
                                DTO_ccSaldosComponentes compInteres = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteInteres).FirstOrDefault();

                                if (compInteres != null && compInteres.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compInteres.CuotaInicial.Value.Value - compInteres.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaIntCap = (compInteres.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaIntCapital = this.LoadComponenteExtra(this.componenteInteresNoCausadoCapital, primeraCuota, Convert.ToInt32(vlrFinanciaIntCap));
                                    if (this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntCapital);
                                }
                            }

                            //Interes Seguro 
                            if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoSeguro) && (poliza == 2 || poliza == 3))
                            {
                                DTO_ccSaldosComponentes compInteresSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteInteresSeguro).FirstOrDefault();

                                if (compInteresSeguro != null && compInteresSeguro.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compInteresSeguro.CuotaInicial.Value.Value - compInteresSeguro.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaIntSeg = (compInteresSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaIntSeguro = this.LoadComponenteExtra(this.componenteInteresNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaIntSeg));
                                    if (this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntSeguro);
                                }
                            }

                            //Seguro 
                            if (!string.IsNullOrWhiteSpace(this.componenteNoCausadoSeguro) && poliza == 3)
                            {
                                DTO_ccSaldosComponentes compSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                    && w.ComponenteCarteraID.Value == this.componenteSeguro).FirstOrDefault();

                                if (compSeguro != null && compSeguro.CuotaSaldo.Value.Value > 0)
                                {
                                    decimal vlrFinanciaPago = compSeguro.CuotaInicial.Value.Value - compSeguro.CuotaSaldo.Value.Value;
                                    decimal vlrFinanciaSeg = (compSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                    ccFinanciaSeguro = this.LoadComponenteExtra(this.componenteNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaSeg));
                                    if (this._liquidarFinanNoCausadaInd)
                                        this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaSeguro);
                                }
                            }
                        }
                    }
                    #endregion  
                    #endregion

                    if (vlrCapitalVencido > 0 && this.infoCreditoTemp.SaldosComponentes.Exists(x=>x.ComponenteCarteraID.Value == this.componenteMora))
                    {
                        MessageBox.Show("No puede seleccionar Cancelar Póliza si tiene saldo de capital atrasado");
                        return;
                    }

                    this.infoCreditoTemp.SaldosComponentes = this.infoCreditoTemp.SaldosComponentes.FindAll(x => x.ComponenteCarteraID.Value == this.componenteSeguro || x.ComponenteCarteraID.Value == this.componenteInteresSeguro ||
                                                                                                            x.ComponenteCarteraID.Value == this.componenteMora || x.ComponenteCarteraID.Value == this.componentePrejuridico);
                }

                #region Otros propósitos (En comentarios)
                //else if (proposito == "4" || proposito == "5")
                //{
                //    #region Restructuración (Abono capital / Cambia cuota)

                //    //Campos extras
                //    this.txtVlrTotal.Properties.ReadOnly = false;
                //    this.txtVlrSeguroVida.Properties.ReadOnly = false;

                //    //Poliza
                //    if (this.credito.Poliza.Value != null && this.credito.VlrPoliza.Value != null && this.credito.VlrPoliza.Value != 0)
                //        this.cmbPoliza.Enabled = true;

                //Dictionary<int, DateTime> pagosFecha = new Dictionary<int, DateTime>();
                //for (int i = 0; i < this.infoCreditoTemp.PlanPagos.Count; ++i)
                //{
                //    DTO_ccCreditoPlanPagos pago = this.infoCreditoTemp.PlanPagos[i];
                //    pagosFecha.Add(pago.CuotaID.Value.Value, pago.FechaCuota.Value.Value.Date);
                //}

                //string poliza = this.cmbPoliza.EditValue.ToString();
                //foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                //{
                //    #region Cálculo de los pagos

                //    if (this.dt_FechaCorte.DateTime.Date > pagosFecha[comp.CuotaID.Value.Value] && comp.CuotaSaldo.Value.Value > 0
                //        && comp.CuotaID.Value >= primeraCuota)
                //    {
                //        //Pago de componentes atrasados
                //        comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                //    }
                //    else if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota && comp.CuotaSaldo.Value.Value > 0)
                //    {
                //        //Componentes de pago total
                //        comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                //    }
                //    else if ((comp.ComponenteCarteraID.Value == this.componenteInteres || comp.ComponenteCarteraID.Value == this.componenteInteresSeguro)
                //        && this.dt_FechaCorte.DateTime.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                //        && this.dt_FechaCorte.DateTime.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                //        && comp.CuotaID.Value >= primeraCuota)
                //    {
                //        //Componente de interes para el periodo actual
                //        int diasACausar = this.dt_FechaCorte.DateTime.Day;
                //        decimal vlrComp = comp.CuotaSaldo.Value.Value;
                //        decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                //        ccIntNoCausado = this.LoadComponenteIntNoCausado(comp.CuotaID.Value.Value, vlrCausado);
                //    }
                //    else if ((comp.ComponenteCarteraID.Value == this.componenteAportes || comp.ComponenteCarteraID.Value == this.componenteSeguro) &&
                //        this.dt_FechaCorte.DateTime.Month == pagosFecha[comp.CuotaID.Value.Value].Month && this.dt_FechaCorte.DateTime.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                //    {
                //        //Aportes y seguro del periodo actual
                //        comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                //    }
                //    #endregion
                //    #region Poliza
                //    if (this.cmbPoliza.Enabled && this.dt_FechaCorte.DateTime.Date <= pagosFecha[comp.CuotaID.Value.Value] && comp.CuotaSaldo.Value.Value > 0
                //        && comp.CuotaID.Value >= primeraCuota)
                //    {
                //        //Para paga ya se incluye con el total valor
                //        if (comp.ComponenteCarteraID.Value == componenteSeguro)
                //        {
                //            if (poliza == "1")
                //            {
                //                //Continua
                //                comp.AbonoValor.Value = 0;
                //            }
                //            else if (poliza == "3")
                //            {
                //                //Revoca
                //                comp.AbonoValor.Value = porcRevocatoriaSeguro > 0 ? comp.CuotaSaldo.Value * porcRevocatoriaSeguro / 100 : 0;
                //            }
                //        }
                //    }
                //    #endregion
                //}

                ////Interes no causado
                //if (ccIntNoCausado != null)
                //    this.infoCreditoTemp.SaldosComponentes.Add(ccIntNoCausado);

                ////Sanción
                //if (!String.IsNullOrWhiteSpace(this.componenteSancionID))
                //{
                //    DTO_ccCreditoPlanPagos cuotaSancion = this.LoadCuotaSancion(this.infoCreditoTemp.PlanPagos.Last());
                //    DTO_ccSaldosComponentes cSancion = this.LoadComponenteSancion(cuotaSancion.CuotaID.Value.Value, cuotaSancion.VlrCuota.Value.Value);

                //    this.infoCreditoTemp.PlanPagos.Add(cuotaSancion);
                //    this.infoCreditoTemp.SaldosComponentes.Add(cSancion);
                //    this.multa = cSancion.CuotaInicial.Value.Value;
                //}

                //    #endregion
                //}
                #endregion

                #endregion

                this.validate = false;
                this.LoadGridCompData();
                this.LoadComponentesSaldos(true);
                this.validate = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "LoadPropositos"));
            }
        }

        /// <summary>
        /// Asigna el valor total de los pagos
        /// </summary>
        private void CalcularValorPago(bool getCalculatedValues)
        {
            try
            {
                if (getCalculatedValues)
                {
                    decimal vlrSaldoPend = 0;
                    decimal vlrSaldoOtros = 0;
                    decimal vlrPagarOtros = 0;
                    foreach (var c in this.saldosTotales)
                    {
                        DTO_ccSaldosComponentes comp = this.infoSaldos.SaldosComponentes.Where(x => x.ComponenteCarteraID.Value == c.ComponenteCarteraID.Value).FirstOrDefault();
                        byte tipoComp = comp != null && comp.TipoComponente.Value.HasValue ? comp.TipoComponente.Value.Value : (byte)0;
                        if (tipoComp == (byte)TipoComponente.CapitalSolicitado || tipoComp == (byte)TipoComponente.ComponenteCuota)
                            vlrSaldoPend += c.VlrPagar.Value.Value;
                        if (tipoComp == (byte)TipoComponente.ComponenteCuota && c.ComponenteCarteraID.Value != this.componenteInteres)
                        {
                            vlrSaldoOtros += c.PagoValor.Value.Value;
                            vlrPagarOtros += c.VlrPagar.Value.Value;
                        }                             
                    }

                    //Capital
                    this.txtSaldoCapital.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteCapital).Sum(x => x.PagoValor.Value);
                    this.txtVlrCapital.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteCapital).Sum(x => x.VlrPagar.Value);
                    //Interes
                    this.txtSaldoInter.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteInteres).Sum(x => x.PagoValor.Value);
                    this.txtVlrInteres.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteInteres).Sum(x => x.VlrPagar.Value);
                    //Otros
                    this.txtSaldoOtros.EditValue = vlrSaldoOtros;
                    this.txtVlrOtros.EditValue = vlrPagarOtros;
                    //Total
                    this.txtSaldoTotComp.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteCapital || x.ComponenteCarteraID.Value == this.componenteInteres).Sum(x => x.PagoValor.Value) + vlrSaldoOtros;
                    this.txtVlrTotalComp.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value == this.componenteCapital || x.ComponenteCarteraID.Value == this.componenteInteres).Sum(x => x.VlrPagar.Value) + vlrPagarOtros;

                    //Saldo Pendiente
                    this.txtSaldoPendiente.EditValue = this.txtVlrTotalComp.EditValue;
                    this.txtVlrOtrosCompon.EditValue = this.saldosTotales.Sum(c => c.VlrPagar.Value) - Convert.ToInt32(this.txtVlrTotalComp.EditValue, CultureInfo.InvariantCulture);
                    this.txtSaldoTotal.EditValue = this.saldosTotales.Sum(c => c.VlrPagar.Value);//Vlr Bruto Pagar
                }
                this.txtVlrPago.EditValue = Convert.ToInt64(this.txtSaldoTotal.EditValue) - Convert.ToInt32(this.txtVlrAbono.EditValue) + Convert.ToInt32(this.txtSeguroVida.EditValue);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "CalcularValorPago"));
            }
        }

        #endregion

        #region Carga información de las grillas

        /// <summary>
        /// Funcion que filta los componentes con los saldos actualizados
        /// </summary>
        private void LoadComponentesSaldos(bool getCalculatedValues)
        {
            try
            {
                this.infoSaldos = new DTO_InfoCredito();

                List<DTO_ccCreditoPlanPagos> planPagosSaldos = new List<DTO_ccCreditoPlanPagos>();
                List<DTO_ccCreditoPlanPagos> planPagosTemp = ObjectCopier.Clone(this.infoCreditoTemp.PlanPagos);
                List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();

                foreach (DTO_ccCreditoPlanPagos p in planPagosTemp)
                {
                    List<DTO_ccSaldosComponentes> componentesCuota = (from c in this.infoCreditoTemp.SaldosComponentes where c.CuotaID.Value == p.CuotaID.Value select c).ToList();
                    decimal vlrTotal = (from c in componentesCuota select c.CuotaSaldo.Value.Value).Sum();
                    if (vlrTotal > 0)
                    {
                        p.VlrSaldo.Value = vlrTotal;
                        p.VlrPagadoCuota.Value = 0;
                        planPagosSaldos.Add(p);
                        componentes.AddRange(componentesCuota);
                    }
                }

                this.infoSaldos.AddData(planPagosSaldos, componentes);

                //Calcula el nuevo saldo pendiente
                this.CalcularValorPago(getCalculatedValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "LoadComponentesSaldos"));
            }
        }

        /// <summary>
        /// Funcion que filta los componentes con los saldos pagos actualizados
        /// </summary>
        private void LoadComponentesPagos()
        {
            this.infoPagos = new DTO_InfoCredito();

            List<DTO_ccCreditoPlanPagos> planPagos = new List<DTO_ccCreditoPlanPagos>();
            List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();
            List<DTO_ccSaldosComponentes> componentesCopy = ObjectCopier.Clone(this.infoCreditoTemp.SaldosComponentes);

            List<DTO_ccCreditoPlanPagos> planPagosFilter = this.infoCreditoTemp.PlanPagos.Where(c => c.VlrPagadoCuota.Value != 0 || c.VlrPagadoExtras.Value != 0 || c.VlrMoraPago.Value != 0).ToList();
            foreach (DTO_ccCreditoPlanPagos p in planPagosFilter)
            {
                List<DTO_ccSaldosComponentes> compCuota = (from c in componentesCopy where c.CuotaID.Value == p.CuotaID.Value select c).ToList();
                foreach (DTO_ccSaldosComponentes item in compCuota)
                {
                    if (item.CuotaInicial.Value != item.CuotaSaldo.Value)
                    {
                        item.CuotaSaldo.Value = item.CuotaInicial.Value - item.CuotaSaldo.Value;
                        item.AbonoValor.Value = item.CuotaSaldo.Value;
                        item.TotalSaldo.Value = item.TotalInicial.Value - item.CuotaSaldo.Value;
                    }
                }

                planPagos.Add(p);
                componentes.AddRange(compCuota);
            }

            this.infoPagos.AddData(planPagos, componentes);
        }

        /// <summary>
        /// Carga la informacion de la grilla
        /// </summary>
        private void LoadGridCompData(List<DTO_ccEstadoCuentaComponentes> exist = null)
        {
            this.saldosTotales = new List<DTO_ccEstadoCuentaComponentes>();
            List<string> compsID = (from c in this.infoCreditoTemp.SaldosComponentes select c.ComponenteCarteraID.Value).Distinct().ToList();
            List<string> compsDescr = (from c in this.infoCreditoTemp.SaldosComponentes select c.Descriptivo.Value).Distinct().ToList();
            byte poliza = Convert.ToByte(this.lkpPoliza.EditValue);
            for (int i = 0; i < compsID.Count(); ++i)
            {
                bool addComponente = true;
                DTO_ccEstadoCuentaComponentes compEC = new DTO_ccEstadoCuentaComponentes();
                compEC.ComponenteCarteraID.Value = compsID[i];
                compEC.Descriptivo.Value = compsDescr[i];
                compEC.Editable.Value = false;

                //Se ponen asi porque la columna de pago valor debe mostrar siempre el saldo total y la columna de saldo se usa para cálculos
            
                //Total
                compEC.PagoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where (c.ComponenteCarteraID.Value == compsID[i] && 
                                              (c.TipoComponente.Value == (byte)TipoComponente.CapitalSolicitado ||c.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota)) select c.CuotaSaldo.Value).Sum();
                //Abono calculado
                compEC.SaldoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where c.ComponenteCarteraID.Value == compsID[i] select c.AbonoValor.Value).Sum();
                var q = this.infoCreditoTemp.SaldosComponentes.FindAll(x => x.ComponenteCarteraID.Value == componenteSeguro).ToList();
                //Abono calculado
                compEC.AbonoValor.Value = compEC.SaldoValor.Value;
                //Valor a pagar
                compEC.VlrPagar.Value = compEC.SaldoValor.Value;
                if (compEC.ComponenteCarteraID.Value == componenteSeguro && Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                {
                    compEC.AbonoValor.Value = compEC.PagoValor.Value;
                    compEC.VlrPagar.Value = compEC.PagoValor.Value;
                }

                #region Validaciones de acuerdo al propósito

                #region Prepago (Póliza continua)
                if (this.lkpProposito.EditValue != null && poliza == 1 && (Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Prepago ||
                                                                           Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RecogeSaldo ||
                                                                           Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionAbono||
                                                                           Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                {
                    if (compEC.ComponenteCarteraID.Value == componenteSeguro)
                    {
                        compEC.PagoValor.Value = this.vlrRevSeg;
                        compEC.AbonoValor.Value = this.vlrRevSeg;
                        compEC.VlrPagar.Value = this.vlrRevSeg;
                    }
                    else if (compEC.ComponenteCarteraID.Value == componenteInteresSeguro)
                    {
                        compEC.PagoValor.Value = this.vlrRevIntSeg;
                        compEC.AbonoValor.Value = this.vlrRevIntSeg;
                        compEC.VlrPagar.Value = this.vlrRevIntSeg;
                    }                       
                }
                else if(poliza == 3)
                {
                    if (compEC.ComponenteCarteraID.Value == componenteSeguro)
                    {
                        compEC.AbonoValor.Value = this.vlrRevSeg;
                        compEC.VlrPagar.Value = this.vlrRevSeg;
                    }
                    else if (compEC.ComponenteCarteraID.Value == this.componentePolizaEC)
                    {
                        DTO_ccEstadoCuentaComponentes seg = this.saldosTotales.Find(x => x.ComponenteCarteraID.Value == componenteSeguro);
                        if (seg != null)
                        {
                            int valRevTemp = Convert.ToInt32((seg.PagoValor.Value.Value - seg.AbonoValor.Value.Value) * this.porcRevocatoriaSeguro / 100);
                            compEC.AbonoValor.Value = valRevTemp;
                            compEC.VlrPagar.Value = valRevTemp;
                        }
                        else
                        {
                            compEC.AbonoValor.Value = 0;
                            compEC.VlrPagar.Value = 0;
                        }                       
                    }                   
                }
                #endregion
                #region Cobro jurídico
                DTO_ccCarteraComponente comp = this.GetComponenteInfo(compEC.ComponenteCarteraID.Value);
                if (this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.EnvioCobroJuridico 
                    && comp.TipoComponente.Value.Value != (byte)TipoComponente.CapitalSolicitado
                    && comp.TipoComponente.Value.Value != (byte)TipoComponente.ComponenteCuota
                    && comp.TipoComponente.Value.Value != (byte)TipoComponente.ComponenteGasto)
                {
                    addComponente = false;
                }
                #endregion

                #endregion

                if (addComponente)
                    this.saldosTotales.Add(compEC);
            }
          
            #region  Agrega el componente de Revocatoria sino existe en el estado de cuenta
            if (poliza == 3 && !this.saldosTotales.Exists(x => x.ComponenteCarteraID.Value == this.componentePolizaEC))
            {
                DTO_ccEstadoCuentaComponentes rev = new DTO_ccEstadoCuentaComponentes();
                DTO_ccCarteraComponente compon = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, this.componentePolizaEC, true);
                DTO_ccEstadoCuentaComponentes seg = this.saldosTotales.Find(x => x.ComponenteCarteraID.Value == componenteSeguro);
                rev.ComponenteCarteraID.Value = this.componentePolizaEC;
                rev.Descriptivo.Value = compon.Descriptivo.Value;
                rev.PagoValor.Value = 0;
                rev.SaldoValor.Value = 0;
                rev.Editable.Value = false;
                if (seg != null)
                {
                    int valRevTemp = Convert.ToInt32((seg.PagoValor.Value.Value - seg.AbonoValor.Value.Value) * this.porcRevocatoriaSeguro / 100);
                    rev.AbonoValor.Value = valRevTemp;
                    rev.VlrPagar.Value = valRevTemp;
                }
                else
                {
                    rev.AbonoValor.Value = 0;
                    rev.VlrPagar.Value = 0;
                }
                this.saldosTotales.Add(rev);
            } 
            #endregion

            if (Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionPlazo)
                this.txtVlrAbono.EditValue = this.saldosTotales.FindAll(x => x.ComponenteCarteraID.Value != this.componenteCapital).Sum(x => x.AbonoValor.Value);

            if (exist != null)
                this.saldosTotales = exist; 

            this.gcComponentes.DataSource = this.saldosTotales;
        }

        #endregion

        #region Carga componentes extras

        /// <summary>
        /// Carga una cuota de sancion pra un plan de pagos
        /// </summary>
        /// <param name="ultima">Cuota guia</param>
        /// <returns>Retorna la nueva cuota</returns>
        private DTO_ccCreditoPlanPagos LoadCuotaSancion(DTO_ccCreditoPlanPagos cuotaCopia)
        {
            DTO_ccCreditoPlanPagos cuotaSancion = new DTO_ccCreditoPlanPagos();
            cuotaSancion.CuotaID.Value = cuotaCopia.CuotaID.Value.Value + 1;
            cuotaSancion.CuotaFijadaInd.Value = true;
            cuotaSancion.FechaCuota.Value = this.fechaCorte.Date;
            cuotaSancion.FechaLiquidaMora.Value = this.fechaCorte.Date;
            cuotaSancion.NumeroDoc.Value = this.credito.NumeroDoc.Value;
            cuotaSancion.PagoInd.Value = true;
            cuotaSancion.TipoPago.Value = cuotaCopia.TipoPago.Value;
            cuotaSancion.VlrCuota.Value = cuotaCopia.VlrCuota.Value;
            cuotaSancion.VlrCapital.Value = cuotaCopia.VlrCuota.Value;
            cuotaSancion.VlrInteres.Value = 0;
            cuotaSancion.VlrSeguro.Value = 0;
            cuotaSancion.VlrMoraLiquida.Value = 0;
            cuotaSancion.VlrMoraPago.Value = cuotaCopia.VlrMoraPago.Value;
            cuotaSancion.VlrOtro1.Value = cuotaCopia.VlrOtro1.Value;
            cuotaSancion.VlrOtro2.Value = cuotaCopia.VlrOtro2.Value;
            cuotaSancion.VlrOtro3.Value = cuotaCopia.VlrOtro3.Value;
            cuotaSancion.VlrOtrosFijos.Value = cuotaCopia.VlrOtrosFijos.Value;
            cuotaSancion.VlrPagadoCuota.Value = 0;
            cuotaSancion.VlrPagadoExtras.Value = 0;
            cuotaSancion.VlrSaldoCapital.Value = 0;

            return cuotaSancion;
        }

        /// <summary>
        /// Carga el componente de sancion para la ultima cuota
        /// </summary>
        /// <param name="cuotaID">Identificador de la cuota</param>
        /// <param name="valor">Valor a pagar</param>
        /// <returns>Retorna el componente</returns>
        private DTO_ccSaldosComponentes LoadComponenteExtra(string componenteID, int cuotaID, decimal valor)
        {
            DTO_ccCarteraComponente comp = this.GetComponenteInfo(componenteID);

            DTO_ccSaldosComponentes cSancion = new DTO_ccSaldosComponentes();
            cSancion.CuotaID.Value = cuotaID;
            cSancion.ComponenteCarteraID.Value = componenteID;
            cSancion.Descriptivo.Value = comp.Descriptivo.Value;
            cSancion.ComponenteFijo.Value = true;
            cSancion.PagoTotalInd.Value = true;
            cSancion.TipoComponente.Value = comp.TipoComponente.Value;
            cSancion.CuotaInicial.Value = valor;
            cSancion.TotalInicial.Value = valor;
            cSancion.CuotaSaldo.Value = valor;
            cSancion.TotalSaldo.Value = valor;
            cSancion.AbonoValor.Value = valor;

            return cSancion;
        }

        #endregion

        #endregion Funciones Privadas

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
                
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemEdit.Visible = true;               
                FormProvider.Master.itemEdit.ToolTipText = "Editar los valores de los componentes";
                FormProvider.Master.itemEdit.Enabled = !this._allowEditValuesInd;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSearch.Enabled = true;
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Print);                 
                }

               this.gbGridComponentes.Visible = SecurityManager.HasAccess(AppQueries.QueryEstadoCtaComp, FormsActions.Get);
               this.gbConsultas.Visible = SecurityManager.HasAccess(AppQueries.QueryEstadoCtaComp, FormsActions.Get);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "DigitacionCredito.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "DigitacionCredito.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "DigitacionCredito.cs-Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Header

        /// <summary>
        /// Trae los datos de coTercero, para almacenarlos en los campos de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {                
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    this.creditos = new List<DTO_ccCreditoDocu>();
                    if (this.masterCliente.ValidID)
                    {
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.creditos = _bc.AdministrationModel.GetCreditosPendientesByCliente(this.masterCliente.Value);
                        this.creditos = this.creditos.Where(c => c.EstadoDeuda.Value != (byte)EstadoDeuda.Juridico 
                            && c.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPago
                            && c.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPagoIncumplido).ToList();

                        if (this.creditos.Count == 0)
                        {
                            string clienteTmp = this.masterCliente.Value;
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinCreditosEC);
                            MessageBox.Show(msg);
                            this.CleanData();
                            this.masterCliente.Value = clienteTmp;
                        }
                        else
                        {
                            this.cmbEstado.EditValue = this.cliente.EstadoCartera.Value.ToString();
                            this.LoadLibranzas();

                            this.gvLibranza.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvLibranza_FocusedRowChanged);
                            this.gcLibranza.DataSource = this.creditos;
                            this.gvLibranza.RefreshData();
                            this.gvLibranza.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvLibranza_FocusedRowChanged);

                            if (this.gvLibranza.DataRowCount > 0)
                            {
                                DTO_ccCreditoDocu temp = (DTO_ccCreditoDocu)this.gvLibranza.GetRow(0);
                                if (temp != null)
                                {
                                    this.libranzaID = temp.Libranza.Value.ToString();
                                    this.credito = temp;
                                    this.LoadCreditoInfo();
                                    #region Recalcula los valores de los componentes  
                                    byte proposito = this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) != 0 ? Convert.ToByte(this.lkpProposito.EditValue) : (byte)PropositoEstadoCuenta.Proyeccion;
                                    #region Inicia los controles y variables que se calculan de acuerdo al porpósito
                                    this.lkpPoliza.Enabled = false;
                                    this.txtSeguroVida.EditValue = 0;
                                    this.vlrRevSeg = 0;
                                    this.vlrRevIntSeg = 0;

                                    this.txtVlrOtrosCompon.Properties.ReadOnly = true;
                                    this.txtVlrOtrosCompon.Properties.ReadOnly = true;
                                    this.txtVlrOtrosCompon.Properties.ReadOnly = true;

                                    this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);
                                    this.infoCreditoTemp.PlanPagos = this.infoCreditoTemp.PlanPagos.Where(x => x.CuotaID.Value >= primeraCuota).ToList();
                                    this.infoCreditoTemp.SaldosComponentes = this.infoCreditoTemp.SaldosComponentes.Where(x => x.CuotaID.Value >= primeraCuota).ToList();

                                    #endregion
                                    #region Calcula las fechas de pagos de cada cuota
                                    Dictionary<int, DateTime> pagosFecha = new Dictionary<int, DateTime>();
                                    for (int i = 0; i < this.infoCreditoTemp.PlanPagos.Count; ++i)
                                    {
                                        DTO_ccCreditoPlanPagos pago = this.infoCreditoTemp.PlanPagos[i];
                                        pagosFecha.Add(pago.CuotaID.Value.Value, pago.FechaCuota.Value.Value.Date);
                                    }

                                    #endregion
                                    #region Asigna la información segun el propósito
                                    if (proposito == (byte)PropositoEstadoCuenta.Proyeccion)
                                    {
                                        #region Proyección
                                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                        #endregion
                                    }
                                    else if (proposito == (byte)PropositoEstadoCuenta.Prepago || proposito == (byte)PropositoEstadoCuenta.RecogeSaldo ||
                                             proposito == (byte)PropositoEstadoCuenta.RestructuracionAbono || proposito == (byte)PropositoEstadoCuenta.RestructuracionPlazo)
                                    {
                                        #region Prepago-RecogeSaldo-AbonoCapital

                                        this.lkpPoliza.Enabled = true;
                                        DTO_ccSaldosComponentes ccRevSeguro = null;
                                        DTO_ccSaldosComponentes ccIntNoCausado = null;
                                        DTO_ccSaldosComponentes ccFinanciaIntCapital = null;
                                        DTO_ccSaldosComponentes ccFinanciaIntSeguro = null;
                                        DTO_ccSaldosComponentes ccFinanciaSeguro = null;

                                        byte poliza = Convert.ToByte(this.lkpPoliza.EditValue);
                                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                                        {
                                            #region Cálculo de los pagos

                                            if (this.fechaCorte.Date >= pagosFecha[comp.CuotaID.Value.Value].Date  && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                //Pago de componentes atrasados
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota )
                                            {
                                                //Componentes de pago total
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if ((comp.ComponenteCarteraID.Value == this.componenteInteres || comp.ComponenteCarteraID.Value == this.componenteInteresSeguro)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                                                && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                //Componente de interes para el periodo actual
                                                int diasACausar = this.fechaCorte.Day;
                                                decimal vlrComp = comp.CuotaSaldo.Value.Value;
                                                decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                                                ccIntNoCausado = this.LoadComponenteExtra(this.componenteInteresNoCausado, comp.CuotaID.Value.Value, vlrCausado);
                                            }
                                            else if ((comp.ComponenteCarteraID.Value == this.componenteAportes)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                                            {
                                                //Aportes y seguro del periodo actual
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if (this.sector != SectorCartera.Financiero
                                                && (comp.ComponenteCarteraID.Value == this.componenteAportes)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                                            {
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            #endregion
                                            #region Poliza
                                            if (this.sector == SectorCartera.Financiero && this.lkpPoliza.Enabled  && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                #region Componente seguro
                                                if (comp.ComponenteCarteraID.Value == componenteSeguro)
                                                {
                                                    //comp.AbonoValor.Value = 0;
                                                    if (poliza == 1)
                                                    {
                                                        //Continua
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date )
                                                            this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                    else if (poliza == 2)
                                                    {
                                                        //Paga
                                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                    else if (poliza == 3)
                                                    {
                                                        //Revoca
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date )
                                                            this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                        else
                                                        {
                                                            //decimal valRevTemp = this.porcRevocatoriaSeguro > 0 ? comp.CuotaSaldo.Value.Value * this.porcRevocatoriaSeguro / 100 : 0;
                                                            //valRevTemp = Convert.ToInt32(valRevTemp);
                                                            //this.vlrRevSeg += valRevTemp;
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Componente interes seguro
                                                if (comp.ComponenteCarteraID.Value == componenteInteresSeguro)
                                                {
                                                    //Para paga ya se incluye con el total valor
                                                    if (poliza == 1)
                                                    {
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                                            this.vlrRevIntSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        //Componentes extras
                                        #region Poliza EC
                                        if (this.sector == SectorCartera.Financiero && vlrRevSeg > 0)
                                        {
                                            if (poliza == 3) //Revoca
                                            {
                                                ccRevSeguro = this.LoadComponenteExtra(this.componentePolizaEC, primeraCuota, this.vlrRevSeg);
                                                this.infoCreditoTemp.SaldosComponentes.Add(ccRevSeguro);
                                            }
                                        }
                                        #endregion
                                        #region Interes financiado no causado

                                        DTO_ccCreditoPlanPagos proxCuota = this.infoCreditoTemp.PlanPagos.Where(w => w.FechaCuota.Value.Value.Date > this.fechaCorte.Date)
                                            .OrderBy(o => o.FechaCuota.Value.Value).FirstOrDefault();

                                        if (proxCuota != null)
                                        {
                                            int diaCuota = proxCuota.FechaCuota.Value.Value.Day;
                                            if (diaCuota == 31)
                                                diaCuota = 30;

                                            int diaCierre = this.fechaCorte.Day;
                                            if (diaCierre == 31)
                                                diaCierre = 30;

                                            int difDias = 0;
                                            if (diaCierre > diaCuota)
                                                difDias = diaCierre - diaCuota;
                                            else if (diaCierre < diaCuota)
                                                difDias = 30 - diaCuota + diaCierre;

                                            int divisor = !this.credito.PeriodoPago.Value.HasValue || this.credito.PeriodoPago.Value.Value == (byte)PeriodoPago.PrimeraQuincena ? 30 : 15;
                                            DateTime newfechaCuota = proxCuota.FechaCuota.Value.Value;
                                            if (divisor == 30) //Mensual
                                                newfechaCuota = proxCuota.FechaCuota.Value.Value.AddMonths(-1);
                                            else //Quincenal
                                                newfechaCuota = proxCuota.FechaCuota.Value.Value.AddDays(-15);
                                            newfechaCuota = newfechaCuota.AddDays(1);
                                            difDias = (this.fechaCorte.Date - newfechaCuota.Date).Days >= 0 ? (this.fechaCorte.Date - newfechaCuota.Date).Days + 1 : 0;

                                            if (difDias != 0)
                                            {
                                                this._diasFNC = Convert.ToByte(difDias);
                                                this._fechaInicioFNC = newfechaCuota;

                                                //Interes Capital 
                                                if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoCapital))
                                                {
                                                    DTO_ccSaldosComponentes compInteres = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteInteres).FirstOrDefault();

                                                    if (compInteres != null && compInteres.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compInteres.CuotaInicial.Value.Value - compInteres.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaIntCap = (compInteres.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaIntCapital = this.LoadComponenteExtra(this.componenteInteresNoCausadoCapital, primeraCuota, Convert.ToInt32(vlrFinanciaIntCap));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntCapital);
                                                    }
                                                }

                                                //Interes Seguro 
                                                if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoSeguro) && (poliza == 2 || poliza == 3))
                                                {
                                                    DTO_ccSaldosComponentes compInteresSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteInteresSeguro).FirstOrDefault();

                                                    if (compInteresSeguro != null && compInteresSeguro.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compInteresSeguro.CuotaInicial.Value.Value - compInteresSeguro.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaIntSeg = (compInteresSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaIntSeguro = this.LoadComponenteExtra(this.componenteInteresNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaIntSeg));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntSeguro);
                                                    }
                                                }

                                                //Seguro 
                                                if (!string.IsNullOrWhiteSpace(this.componenteNoCausadoSeguro) && poliza == 3)
                                                {
                                                    DTO_ccSaldosComponentes compSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteSeguro).FirstOrDefault();

                                                    if (compSeguro != null && compSeguro.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compSeguro.CuotaInicial.Value.Value - compSeguro.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaSeg = (compSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaSeguro = this.LoadComponenteExtra(this.componenteNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaSeg));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaSeguro);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Sanción
                                        if (!String.IsNullOrWhiteSpace(this.componenteSancionID))
                                        {
                                            DTO_ccCreditoPlanPagos cuotaSancion = this.LoadCuotaSancion(this.infoCreditoTemp.PlanPagos.Last());
                                            DTO_ccSaldosComponentes cSancion = this.LoadComponenteExtra(this.componenteSancionID, cuotaSancion.CuotaID.Value.Value, cuotaSancion.VlrCuota.Value.Value);

                                            this.infoCreditoTemp.PlanPagos.Add(cuotaSancion);
                                            this.infoCreditoTemp.SaldosComponentes.Add(cSancion);
                                        }

                                        #endregion
                                        #endregion
                                    }
                                    else if (proposito == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                                    {
                                        #region Cobro jurídico

                                        decimal vlrPolizaCap = 0;
                                        decimal vlrAbonoPoliza = 0;
                                        DTO_ccPolizaEstado polizaCJ = _bc.AdministrationModel.PolizaEstado_GetLastPoliza(this.credito.NumeroDoc.Value.Value, this.credito.Libranza.Value.Value);

                                        //Filtra los datos
                                        this.infoCreditoTemp.SaldosComponentes.RemoveAll(s => s.ComponenteCarteraID.Value == this.componenteMora);
                                        this.infoCreditoTemp.PlanPagos.ForEach(pp =>
                                        {
                                            pp.VlrMoraLiquida.Value = 0;
                                            pp.VlrMoraPago.Value = 0;
                                        });

                                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                                        {
                                            if (comp.CuotaID.Value >= primeraCuota)
                                            {
                                                //Pago del capital
                                                if (comp.ComponenteCarteraID.Value == this.componenteCapital)
                                                {
                                                    comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                                }
                                                else if (comp.ComponenteCarteraID.Value == this.componenteSeguro)
                                                {
                                                    if (polizaCJ != null && (polizaCJ.FechaLiqSeguro.Value.Value.Date > this.fechaCorte.Date || comp.CuotaID.Value.Value < polizaCJ.Cuota1Financia.Value.Value))
                                                        vlrPolizaCap += comp.CuotaSaldo.Value.Value;
                                                    else
                                                        vlrAbonoPoliza += comp.CuotaSaldo.Value.Value;
                                                }
                                            }
                                        }

                                        //ComponentesExtras
                                        #region Capital póliza

                                        if (polizaCJ != null)
                                        {
                                            DTO_ccSaldosComponentes compCap = this.infoCreditoTemp.SaldosComponentes.Where(w => w.ComponenteCarteraID.Value == this.componenteCapital).Last();
                                            List<DTO_ccSaldosComponentes> compsSeg = this.infoCreditoTemp.SaldosComponentes.Where(w => w.ComponenteCarteraID.Value == this.componenteSeguro).ToList();

                                            decimal saldoSeguro = compsSeg.Sum(c => c.CuotaSaldo.Value.Value);
                                            decimal vlrDif = saldoSeguro - polizaCJ.VlrPoliza.Value.Value;

                                            //Limpia el valor a pagar del seguro
                                            compsSeg.ForEach(seg => seg.AbonoValor.Value = 0);
                                            if (vlrDif >= 0)
                                            {
                                                compCap.AbonoValor.Value = compCap.AbonoValor.Value + vlrDif;
                                                compsSeg.Last().AbonoValor.Value = polizaCJ.VlrPoliza.Value.Value;
                                            }
                                            else
                                            {
                                                compCap.AbonoValor.Value = compCap.AbonoValor.Value;
                                                compsSeg.Last().AbonoValor.Value = saldoSeguro;
                                            }
                                        }

                                        #endregion

                                        #endregion
                                    }
                                    else if (proposito == (byte)PropositoEstadoCuenta.Desistimiento)
                                    {
                                        #region Desistimiento

                                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                                            comp.AbonoValor.Value = 0;

                                        #endregion
                                    }
                                    else if (proposito == (byte)PropositoEstadoCuenta.CancelaPoliza)
                                    {
                                        #region Cancelacion Poliza (CancelaPoliza)

                                        //Poliza
                                        if (this.credito.Poliza.Value != null && this.credito.VlrPoliza.Value != null && this.credito.VlrPoliza.Value != 0)
                                            this.lkpPoliza.Enabled = true;

                                        DTO_ccSaldosComponentes ccRevSeguro = null;
                                        DTO_ccSaldosComponentes ccIntNoCausado = null;
                                        DTO_ccSaldosComponentes ccFinanciaIntCapital = null;
                                        DTO_ccSaldosComponentes ccFinanciaIntSeguro = null;
                                        DTO_ccSaldosComponentes ccFinanciaSeguro = null;

                                        byte poliza = Convert.ToByte(this.lkpPoliza.EditValue);
                                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                                        {
                                            #region Cálculo de los pagos

                                            if (this.fechaCorte >= pagosFecha[comp.CuotaID.Value.Value] && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                //Pago de componentes atrasados
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota )
                                            {
                                                //Componentes de pago total
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if ((comp.ComponenteCarteraID.Value == this.componenteInteres || comp.ComponenteCarteraID.Value == this.componenteInteresSeguro)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                                                && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                //Componente de interes para el periodo actual
                                                int diasACausar = this.fechaCorte.Day;
                                                decimal vlrComp = comp.CuotaSaldo.Value.Value;
                                                decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                                                ccIntNoCausado = this.LoadComponenteExtra(this.componenteInteresNoCausado, comp.CuotaID.Value.Value, vlrCausado);
                                            }
                                            else if ((comp.ComponenteCarteraID.Value == this.componenteAportes)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                                            {
                                                //Aportes y seguro del periodo actual
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            else if (this.sector != SectorCartera.Financiero
                                                && (comp.ComponenteCarteraID.Value == this.componenteAportes)
                                                && this.fechaCorte.Month == pagosFecha[comp.CuotaID.Value.Value].Month
                                                && this.fechaCorte.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                                            {
                                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                                            }
                                            #endregion
                                            #region Poliza

                                            if (this.sector == SectorCartera.Financiero && this.lkpPoliza.Enabled && comp.CuotaID.Value >= primeraCuota)
                                            {
                                                #region Componente seguro
                                                if (comp.ComponenteCarteraID.Value == componenteSeguro)
                                                {
                                                    //Para paga ya se incluye con el total valor
                                                    //comp.AbonoValor.Value = 0;
                                                    if (poliza == 1)
                                                    {
                                                        //Continua
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date )
                                                            this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                    else if (poliza == 2)
                                                    {
                                                        //Paga
                                                        this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                    else if (poliza == 3)
                                                    {
                                                        //Revoca
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date)
                                                            this.vlrRevSeg += comp.CuotaSaldo.Value.Value;
                                                        else
                                                        {
                                                            //decimal valRevTemp = this.porcRevocatoriaSeguro > 0 ? comp.CuotaSaldo.Value.Value * this.porcRevocatoriaSeguro / 100 : 0;
                                                            //valRevTemp = Convert.ToInt32(valRevTemp);
                                                            //this.vlrRevSeg += valRevTemp;
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Componente interes seguro
                                                if (comp.ComponenteCarteraID.Value == componenteInteresSeguro)
                                                {
                                                    //Para paga ya se incluye con el total valor
                                                    if (poliza == 1)
                                                    {
                                                        DTO_ccCreditoPlanPagos cuota = this.infoCreditoTemp.PlanPagos.Where(c => c.CuotaID.Value == comp.CuotaID.Value).FirstOrDefault();
                                                        if (this.fechaCorte.Date >= cuota.FechaCuota.Value.Value.Date && comp.CuotaSaldo.Value.Value > 0)
                                                            this.vlrRevIntSeg += comp.CuotaSaldo.Value.Value;
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }

                                        //Componentes extras
                                        #region Poliza EC
                                        if (this.sector == SectorCartera.Financiero && vlrRevSeg > 0)
                                        {
                                            if (poliza == 3) //Revoca
                                            {
                                                ccRevSeguro = this.LoadComponenteExtra(this.componentePolizaEC, primeraCuota, this.vlrRevSeg);
                                                this.infoCreditoTemp.SaldosComponentes.Add(ccRevSeguro);
                                            }
                                        }
                                        #endregion
                                        #region Interes financiado no causado

                                        DTO_ccCreditoPlanPagos proxCuota = this.infoCreditoTemp.PlanPagos.Where(w => w.FechaCuota.Value.Value.Date > this.fechaCorte.Date)
                                            .OrderBy(o => o.FechaCuota.Value.Value).FirstOrDefault();

                                        if (proxCuota != null)
                                        {
                                            int diaCuota = proxCuota.FechaCuota.Value.Value.Day;
                                            if (diaCuota == 31)
                                                diaCuota = 30;

                                            int diaCierre = this.fechaCorte.Day;
                                            if (diaCierre == 31)
                                                diaCierre = 30;

                                            int difDias = 0;
                                            if (diaCierre > diaCuota)
                                            {
                                                difDias = diaCierre - diaCuota;
                                            }
                                            else if (diaCierre < diaCuota)
                                            {
                                                difDias = 30 - diaCuota + diaCierre;
                                            }

                                            if (difDias != 0)
                                            {
                                                int divisor = !this.credito.PeriodoPago.Value.HasValue || this.credito.PeriodoPago.Value.Value == (byte)PeriodoPago.PrimeraQuincena ? 30 : 15;

                                                //Interes Capital 
                                                if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoCapital))
                                                {
                                                    DTO_ccSaldosComponentes compInteres = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteInteres).FirstOrDefault();

                                                    if (compInteres != null && compInteres.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compInteres.CuotaInicial.Value.Value - compInteres.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaIntCap = (compInteres.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaIntCapital = this.LoadComponenteExtra(this.componenteInteresNoCausadoCapital, primeraCuota, Convert.ToInt32(vlrFinanciaIntCap));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntCapital);
                                                    }
                                                }

                                                //Interes Seguro 
                                                if (!string.IsNullOrWhiteSpace(this.componenteInteresNoCausadoSeguro) && (poliza == 2 || poliza == 3))
                                                {
                                                    DTO_ccSaldosComponentes compInteresSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteInteresSeguro).FirstOrDefault();

                                                    if (compInteresSeguro != null && compInteresSeguro.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compInteresSeguro.CuotaInicial.Value.Value - compInteresSeguro.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaIntSeg = (compInteresSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaIntSeguro = this.LoadComponenteExtra(this.componenteInteresNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaIntSeg));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaIntSeguro);
                                                    }
                                                }

                                                //Seguro 
                                                if (!string.IsNullOrWhiteSpace(this.componenteNoCausadoSeguro) && poliza == 3)
                                                {
                                                    DTO_ccSaldosComponentes compSeguro = this.infoCredito.SaldosComponentes.Where(w => w.CuotaID.Value == proxCuota.CuotaID.Value
                                                        && w.ComponenteCarteraID.Value == this.componenteSeguro).FirstOrDefault();

                                                    if (compSeguro != null && compSeguro.CuotaSaldo.Value.Value > 0)
                                                    {
                                                        decimal vlrFinanciaPago = compSeguro.CuotaInicial.Value.Value - compSeguro.CuotaSaldo.Value.Value;
                                                        decimal vlrFinanciaSeg = (compSeguro.CuotaInicial.Value.Value / divisor * difDias) - vlrFinanciaPago;

                                                        ccFinanciaSeguro = this.LoadComponenteExtra(this.componenteNoCausadoSeguro, primeraCuota, Convert.ToInt32(vlrFinanciaSeg));
                                                        if (this._liquidarFinanNoCausadaInd)
                                                            this.infoCreditoTemp.SaldosComponentes.Add(ccFinanciaSeguro);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #endregion
                                        this.infoCreditoTemp.SaldosComponentes = this.infoCreditoTemp.SaldosComponentes.FindAll(x => x.ComponenteCarteraID.Value == this.componenteSeguro || x.ComponenteCarteraID.Value == this.componenteInteresSeguro ||
                                                                                                                                x.ComponenteCarteraID.Value == this.componenteMora || x.ComponenteCarteraID.Value == this.componentePrejuridico);

                                    }

                                    #endregion
                                    #endregion
                                }
                            }
                            else
                            {
                                this.EnableHeader(false);
                                this.libranzaID = string.Empty;
                                this.btnAnexos.Enabled = false;
                                this.credito = new DTO_ccCreditoDocu();
                                this.masterCliente.Focus();
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                            }
                        }
                    }
                    else
                    {
                        this.cliente = null;
                        this.cmbEstado.EditValue = "";
                        this.lblCobroJurid.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que define la operacion de la pantalla de acuerdo al proposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Proposito_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                byte proposito = this.lkpProposito.EditValue != null ? Convert.ToByte(this.lkpProposito.EditValue) : (byte)0;

                #region Nombre Tipo del Proposito
                switch (proposito)
                {
                    case (byte)PropositoEstadoCuenta.Proyeccion:
                        _nameProposito = "Proyección";
                        break;
                    case (byte)PropositoEstadoCuenta.Prepago:
                        _nameProposito = "Prepago";
                        break;
                    case (byte)PropositoEstadoCuenta.RecogeSaldo:
                        _nameProposito = "Reoperación";
                        break;
                    case (byte)PropositoEstadoCuenta.RestructuracionAbono:
                        _nameProposito = "Restructuración Abono";
                        break;
                    case (byte)PropositoEstadoCuenta.RestructuracionPlazo:
                        _nameProposito = "Restructuración Plazo";
                        break;
                    case (byte)PropositoEstadoCuenta.EnvioCobroJuridico:
                        _nameProposito = "Envío Cobro Juridico";
                        break;
                    case (byte)PropositoEstadoCuenta.Desistimiento:
                        _nameProposito = "Desistimiento";
                        break;
                    case (byte)PropositoEstadoCuenta.CondonacionTotal:
                        _nameProposito = "Condonación Total";
                        break;
                    case (byte)PropositoEstadoCuenta.CondonacionParcial:
                        _nameProposito = "Condonación Parcial";
                        break;
                    case (byte)PropositoEstadoCuenta.Normalizacion:
                        _nameProposito = "Normalización";
                        break;
                    case (byte)PropositoEstadoCuenta.CancelaPoliza:
                        _nameProposito = "Cancela Póliza";
                        break;
                } 
                #endregion

                #region Combo Poliza según el Proposito
                this.txtVlrAbono.ReadOnly = true;
                if ((proposito == (byte)PropositoEstadoCuenta.Prepago || proposito == (byte)PropositoEstadoCuenta.RecogeSaldo ||
                    proposito == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                {
                    this.lkpPoliza.EditValue = "1";
                    this.lkpPoliza.Enabled = true;
                }
                else if (proposito == (byte)PropositoEstadoCuenta.RestructuracionAbono)
                {
                    this.lkpPoliza.EditValue = "1";
                    this.lkpPoliza.Enabled = false;
                    this.txtVlrAbono.ReadOnly = false;
                }
                else if (proposito == (byte)PropositoEstadoCuenta.CancelaPoliza)
                {
                    //this.lkpPoliza.EditValue = "2";
                    this.lkpPoliza.Enabled = true;
                }
                else
                {
                    this.lkpPoliza.EditValue = this.compSeguroExist ? "1" : "0";
                    this.lkpPoliza.Enabled = false;
                }
                #endregion

                #region Cambia la fecha mínima para el CJ

                if (this.sector == SectorCartera.Financiero && proposito == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                {
                    this.dt_FechaCorte.DateTime = this.credito.FechaVto.Value.HasValue ? this.credito.FechaVto.Value.Value : new DateTime(2000, 1, 1);
                    this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime;
                    this.dt_FechaLimite.Enabled = false;
                }
                else
                {
                    //this.dt_FechaCorte.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                    if (this.dt_FechaCorte.DateTime < this.dt_FechaCorte.Properties.MinValue)
                        this.dt_FechaCorte.DateTime = this.dt_FechaCorte.Properties.MinValue;

                    this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime.AddDays(diaLimite);
                    this.dt_FechaLimite.Enabled = true;
                }
              
                this.fechaCorte = this.dt_FechaCorte.DateTime;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "lkp_Proposito_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento para aasignar anexos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnexos_Click(object sender, EventArgs e)
        {
            try
            {
                AnexosDocumentos anexos = new AnexosDocumentos(this.credito.NumeroDoc.Value.Value, ModulesPrefix.cc);
                anexos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "btnAnexos_Click"));
            }
        }

        /// <summary>
        /// Evento al cambiar la fecha de corte 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dt_FechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                byte proposito = this.lkpProposito.EditValue != null ? Convert.ToByte(this.lkpProposito.EditValue) : (byte)0;

                if (this.sector == SectorCartera.Financiero && proposito == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                    this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "dt_FechaCorte_EditValueChanged"));
            }

        }

        /// <summary>
        /// Evento que define la operacion de la pantalla de acuerdo al proposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkpPoliza_EditValueChanged(object sender, EventArgs e)
        {
            if (this.sector == SectorCartera.Financiero)
             {
                if (this.compSeguroExist && this.lkpPoliza.EditValue.Equals("0"))
                {
                    this.lkpPoliza.EditValue = "1";
                }
                else if (!this.compSeguroExist)
                {
                    this.lkpPoliza.EditValue = "0";
                }
                else if (this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.CancelaPoliza &&
                        (!this.lkpPoliza.EditValue.Equals("2") && !this.lkpPoliza.EditValue.Equals("3")))
                {
                    this.lkpPoliza.EditValue = "2";
                } 
            }
        }

        /// <summary>
        /// Evento que define la operacion de la pantalla de acuerdo al proposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrAbono_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt64(this.txtSaldoTotal.EditValue) < Convert.ToInt32(this.txtVlrAbono.EditValue))              
                this.txtVlrAbono.EditValue = this.txtSaldoTotal.EditValue;
              this.CalcularValorPago(false);
        }


        #endregion Eventos Formulario

        #region Eventos Grillas libranzas

        /// <summary>
        /// Al cambiar cada fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranza_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gvLibranza.DataRowCount > 0)
                {
                    DTO_ccCreditoDocu temp = (DTO_ccCreditoDocu)this.gvLibranza.GetRow(e.FocusedRowHandle);
                    if (temp != null)
                    {
                        this.gvComponentes.MoveFirst();                 
                        this.libranzaID = temp.Libranza.Value.ToString();
                        this.credito = temp;
                        this.LoadCreditoInfo();

                        if (this.sector == SectorCartera.Financiero && 
                            Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.EnvioCobroJuridico &&
                            temp.EC_FijadoInd.Value.Value)
                        {
                            this.dt_FechaCorte.DateTime = this.credito.FechaVto.Value.HasValue ? this.credito.FechaVto.Value.Value : new DateTime(2000, 1, 1);
                            this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime;
                            this.dt_FechaLimite.Enabled = false;
                        }
                    }
                    else
                    {
                        this.EnableHeader(false);
                        this.libranzaID = string.Empty;
                        this.btnAnexos.Enabled = false;
                        this.credito = new DTO_ccCreditoDocu();
                        this.masterCliente.Focus();
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "gvLibranza_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Al cambiar el valor de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranza_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "EC_FijadoInd")
                {
                    if (Convert.ToBoolean(e.Value))
                    {
                        //Permite que SOLO 1 item sea seleccionado
                        this.lkpProposito.Enabled = true;
                        this.creditos.ForEach(x => x.EC_FijadoInd.Value = false);
                        this.creditos[e.RowHandle].EC_FijadoInd.Value = true;
                        this.LoadProposito(this.primeraCuota);
                    }
                    else
                    {
                        this.lkpProposito.Enabled = false;
                        this.lkpPoliza.Enabled = false;
                        this.LoadProposito(this.primeraCuota);
                    }
                }
                this.gvLibranza.RefreshData();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "gvLibranza_CellValueChanging"));
            }
        }

        /// <summary>
        /// Al dejar una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranza_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.gvComponentes.HasColumnErrors)
                e.Allow = false;
        }

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

        #endregion

        #region Eventos Grillas componentes

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComponentes_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.saldosTotales.Count > 0)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.gvComponentes.PostEditor();
                        this.ValidateRow_Componentes(this.gvComponentes.FocusedRowHandle);
                        if (this.validComponentes)
                            this.AddComponente();
                    }
                    else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        this.gvComponentes.PostEditor();
                        if (this.saldosTotales[this.gvComponentes.FocusedRowHandle].Editable.Value.Value)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                int fila = this.gvComponentes.FocusedRowHandle;

                                this.validate = false;
                                this.saldosTotales.RemoveAt(fila);
                                this.gcComponentes.DataSource = this.saldosTotales;
                                this.gvComponentes.PostEditor();
                                this.gvComponentes.FocusedRowHandle = fila - 1;
                                this.gvComponentes.RefreshData();
                                this.validate = true;

                                //Calcula el nuevo saldo pendiente
                                this.CalcularValorPago(true);
                    }

                            e.Handled = true;
                }
                        else
                            e.Handled = true;
            }
                }
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvComponentes_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this.validate)
                {
                    this.ValidateRow_Componentes(e.RowHandle);
                    if (!this.validComponentes)
                        e.Allow = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcComponentes_Leave(object sender, EventArgs e)
        {
            try
            {
                this.gvComponentes.PostEditor();
                if (this.gvComponentes.DataRowCount > 0 && this.gvComponentes.FocusedRowHandle >= 0)
                    this.ValidateRow_Componentes(this.gvComponentes.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFInan.cs", "gcDocument_Leave"));
            }
        }

        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvComponentes_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.validate)
                {
                    int fila = e.FocusedRowHandle;
                    if (fila >= 0)
                    {
                        DTO_ccEstadoCuentaComponentes compTemp = (DTO_ccEstadoCuentaComponentes)this.gvComponentes.GetRow(e.FocusedRowHandle);
                        if (compTemp != null  && compTemp.Editable.Value.Value)
                        {
                            this.gvComponentes.Columns[this._unboundPrefix + "PagoInd"].OptionsColumn.AllowEdit = false;
                            this.gvComponentes.Columns[this._unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                            //this.gvDocument.Columns[this._unboundPrefix + "VlrPagar"].OptionsColumn.AllowEdit = true;
                        }
                        else
                        {
                            this.gvComponentes.Columns[this._unboundPrefix + "PagoInd"].OptionsColumn.AllowEdit = true;
                            this.gvComponentes.Columns[this._unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                            //this.gvDocument.Columns[this._unboundPrefix + "VlrPagar"].OptionsColumn.AllowEdit = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Evento q valida antes de dejar el row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvComponentes_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int fila = this.gvComponentes.FocusedRowHandle;
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "PagoInd")
                {
                    this.saldosTotales[fila].VlrPagar.Value = (bool)e.Value ? this.saldosTotales[fila].PagoValor.Value.Value : 0;
                    this.gcComponentes.RefreshDataSource();
                }

                if (fieldName == "ComponenteCarteraID")
                {
                    DTO_ccCarteraComponente compTemp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, e.Value.ToString(), true, this.filtrosExtras);
                    this.saldosTotales[fila].Descriptivo.Value = compTemp != null ? compTemp.Descriptivo.Value : string.Empty;
                    this.gcComponentes.RefreshDataSource();
                }

                if (fieldName == "VlrPagar")
                {
                    if (!this.saldosTotales[fila].VlrPagar.Value.HasValue)
                        this.saldosTotales[fila].VlrPagar.Value = 0;

                    _bc.ValidGridCellValue(this.gvComponentes, this._unboundPrefix, fila, "VlrPagar", false, true, false, false);
                }

                //Calcula el nuevo saldo pendiente
                this.CalcularValorPago(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvComponentes.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvComponentes.FocusedRowHandle, colName, origin, this.filtrosExtras);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuentaFin.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Footer

        /// <summary>
        /// Evento que abre un pantalla modal con la informacion de saldosCartera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CuotasPendientes_Click(object sender, EventArgs e)
        {
            ModalSaldosCartera modalinfoCredito = new ModalSaldosCartera(this.infoSaldos, true);
            modalinfoCredito.ShowDialog();
        }

        /// <summary>
        /// Evento que abre una pantalla modal con los saldos de cuotas pagadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CuotasPagadas_Click(object sender, EventArgs e)
        {
            ModalSaldosCartera modalinfoCredito = new ModalSaldosCartera(this.infoPagos, false);
            modalinfoCredito.ShowDialog();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para actualizar la pantalla
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (this.creditos.Count > 0)
                    this.LoadCreditoInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para actualizar la pantalla
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.gvComponentes.Focus();
                if (this.creditos.Count > 0)
                {
                    this.validate = false;
                    this.ValidateRow_Componentes(this.gvComponentes.FocusedRowHandle);
                    if (!this.validComponentes)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ErrorComponentes));
                        return;
                    }

                    #region Validaciones

                    //Validacion cobro jurídico
                    if (this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.EnvioCobroJuridico
                        && !string.IsNullOrWhiteSpace(this.credito.CompradorCarteraID.Value) 
                        && this.credito.CompradorCarteraID.Value != this.compradorCarteraPropia)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidEC_CobroJuridico));
                        return;
                    }

                    //Validacion redención
                    if (this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Desistimiento
                        && !string.IsNullOrWhiteSpace(this.credito.CompradorCarteraID.Value)
                        && this.credito.CompradorCarteraID.Value != this.compradorCarteraPropia)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidEC_Desistimiento));
                        return;
                    }

                    #endregion

                    //Información del crédito
                    this.fechaCorte = this.dt_FechaCorte.DateTime.Date;
                    this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(this.numDocCredito, this.fechaCorte.Date);
                    this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);

                    //Carga la grilla de componentes
                    this.LoadGridCompData();

                    #region Carga la información del header

                    //Mora
                    decimal saldoMora = 0;
                    List<int> cuotasMora = (from c in this.infoCredito.PlanPagos
                                            where c.FechaCuota.Value.Value <= this.fechaCorte.Date
                                            select c.CuotaID.Value.Value).ToList();

                    foreach (int cuotaMora in cuotasMora)
                    {
                        saldoMora += (from c in this.infoCredito.SaldosComponentes
                                      where c.CuotaID.Value == cuotaMora
                                        && (c.TipoComponente.Value == (byte)TipoComponente.CapitalSolicitado || c.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota)
                                      select c.CuotaSaldo.Value.Value).Sum();
                    }
                    this.txtSaldoMora.EditValue = saldoMora;
                    this.txtCuotasMora.Text = cuotasMora.Count().ToString();

                    //Poliza
                    this.txtVlrAbono.EditValue = Convert.ToByte(this.lkpProposito.EditValue) != (byte)PropositoEstadoCuenta.RestructuracionAbono &&
                                                 Convert.ToByte(this.lkpProposito.EditValue) != (byte)PropositoEstadoCuenta.RestructuracionPlazo  ? 0 : this.txtVlrAbono.EditValue;
                    this.txtSeguroVida.EditValue = 0;
                    if (this.sector == SectorCartera.Financiero && (Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Prepago || 
                        Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RecogeSaldo || Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                        this.lkpPoliza.Enabled = true;
                    else
                        this.lkpPoliza.Enabled = false;

                    //Carga la información de pagos, saldos y los valores calculados
                    this.LoadComponentesSaldos(true);
                    this.LoadComponentesPagos();

                    //Combo de primeras cuotas
                    this.cmbCuota.Items.Clear();
                    List<int> cuotas = (from pp in this.infoSaldos.PlanPagos select pp.CuotaID.Value.Value).Take(1).ToList();
                    if (cuotas.Count > 0)
                    {
                        foreach (int c in cuotas)
                            this.cmbCuota.Items.Add(c);

                        this.cmbCuota.Text = cuotas[0].ToString();
                    }

                    #endregion

                    //Carga la información según el propósito
                    this.LoadProposito(this.primeraCuota);

                    #region Estado Cuenta Historia
                    if (Convert.ToByte(this.lkpProposito.EditValue) == 0 || Convert.ToByte(this.lkpProposito.EditValue) == (byte)PropositoEstadoCuenta.Proyeccion)
                    {
                        this.estadoCuentaHistoria = null;
                    }
                    else
                    {
                        this.estadoCuentaHistoria = new DTO_ccEstadoCuentaHistoria();
                        this.estadoCuentaHistoria.EC_Proposito.Value = Convert.ToByte(this.lkpProposito.EditValue);
                        this.estadoCuentaHistoria.EC_Fecha.Value = this.fechaCorte.Date;
                        this.estadoCuentaHistoria.EC_FechaLimite.Value = this.dt_FechaLimite.DateTime;
                        this.estadoCuentaHistoria.EC_Altura.Value = this.primeraCuota;
                        this.estadoCuentaHistoria.EC_CuotasMora.Value = Convert.ToInt16(this.txtCuotasMora.Text);
                        this.estadoCuentaHistoria.EC_PrimeraCtaPagada.Value = !string.IsNullOrEmpty(this.cmbCuota.Text) ? Convert.ToInt16(this.cmbCuota.Text) : this.estadoCuentaHistoria.EC_PrimeraCtaPagada.Value;

                        //Valores
                        this.estadoCuentaHistoria.EC_SaldoPend.Value = Convert.ToInt32(this.txtSaldoPendiente.EditValue);
                        this.estadoCuentaHistoria.EC_SaldoMora.Value = Convert.ToInt32(this.txtSaldoMora.EditValue);
                        this.estadoCuentaHistoria.EC_SaldoTotal.Value = Convert.ToInt32(this.txtSaldoTotal.EditValue);
                        this.estadoCuentaHistoria.EC_ValorPago.Value = Convert.ToInt32(this.txtVlrPago.EditValue);

                        //Poliza
                        this.estadoCuentaHistoria.EC_PolizaMvto.Value = Convert.ToByte(this.lkpPoliza.EditValue);
                        this.estadoCuentaHistoria.EC_ValorAbono.Value = Convert.ToInt32(this.txtVlrAbono.EditValue);
                        this.estadoCuentaHistoria.EC_SeguroVida.Value = Convert.ToInt32(this.txtSeguroVida.EditValue);

                        //Otros campos
                        this.estadoCuentaHistoria.EC_FijadoInd.Value = this.creditos.Exists(x => x.EC_FijadoInd.Value.Value) ? true : false;
                        this.estadoCuentaHistoria.EC_NormalizaInd.Value = false;                        
                        this.estadoCuentaHistoria.FechaInicialFNC.Value = this._fechaInicioFNC;
                        this.estadoCuentaHistoria.DiasFNC.Value = this._diasFNC;
                    }
                    #endregion
                    this.validate = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvLibranza.PostEditor();
                this.gvComponentes.PostEditor();
                if (!this.validComponentes)
                    return;

                byte proposito = this.lkpProposito.EditValue != null && Convert.ToByte(this.lkpProposito.EditValue) != 0 ? Convert.ToByte(this.lkpProposito.EditValue) : (byte)PropositoEstadoCuenta.Proyeccion;
                if (proposito == 0 || proposito == (byte)PropositoEstadoCuenta.Proyeccion)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_SelectProposito), this.lkp_Libranzas.Text);
                    MessageBox.Show(msg);
                    this.lkp_Libranzas.Focus();
                    return;
                }

                if (this.estadoCuentaHistoria == null || this.estadoCuentaHistoria.EC_Proposito.Value.Value != Convert.ToByte(this.lkpProposito.EditValue))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidECProposito));
                    this.lkp_Libranzas.Focus();
                    return;
                }

                if (this.estadoCuentaHistoria == null || this.estadoCuentaHistoria.EC_PolizaMvto.Value.Value != Convert.ToByte(this.lkpPoliza.EditValue))
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidECPoliza));
                    this.lkp_Libranzas.Focus();
                    return;
                }

                if (this.infoSaldos.SaldosComponentes.Count > 0)
                {
                    this.gvComponentes.PostEditor();
                    this.estadoCuenta = new DTO_EstadoCuenta();

                    #region Estado Cuenta Componentes
              
                    this.estadoCuentaHistoria.EC_FijadoInd.Value = this.creditos.Exists(x => x.EC_FijadoInd.Value.Value) ? true : false;
                    this.estadoCuenta.EstadoCuentaHistoria = this.estadoCuentaHistoria;
                    this.estadoCuenta.EstadoCuentaComponentes = this.saldosTotales;

                    //Valores
                    this.estadoCuentaHistoria.EC_SaldoPend.Value = Convert.ToInt32(this.txtSaldoPendiente.EditValue);
                    this.estadoCuentaHistoria.EC_SaldoMora.Value = Convert.ToInt32(this.txtSaldoMora.EditValue);
                    this.estadoCuentaHistoria.EC_SaldoTotal.Value = Convert.ToInt32(this.txtSaldoTotal.EditValue);
                    this.estadoCuentaHistoria.EC_ValorPago.Value = Convert.ToInt32(this.txtVlrPago.EditValue);
                    this.estadoCuentaHistoria.EC_ValorAbono.Value = Convert.ToInt32(this.txtVlrAbono.EditValue);

                    #endregion

                    #region Guarda la info
                    DTO_TxResult result = _bc.AdministrationModel.EstadoCuenta_Add(this._documentID, this.actFlujo.ID.Value, this.infoCreditoTemp, this.estadoCuenta);
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                        #region Variables para el mail

                        ////    //DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                        ////    //string body = string.Empty;
                        ////    //string subject = string.Empty;
                        ////    //string email = user.CorreoElectronico.Value;

                        ////    //string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                        ////    //string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                        ////    //string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                        #endregion
                        #region Envia el correo
                        ////    //subject = string.Format(subjectApr, formName);
                        ////    //body = string.Format(bodyApr, formName, this._credito.Libranza.Value, this.cliente.Descriptivo.Value, string.Empty);
                        ////    //_bc.SendMail(this.documentID, subject, body, email);
                        #endregion
                        var saldosTmp = ObjectCopier.Clone(this.saldosTotales);
                        this.clienteID = this.masterCliente.Value;
                        this.CleanData();
                        this.masterCliente.Value = this.clienteID;
                        this.masterCliente.Focus();
                        this.gvLibranza.Focus();
                        this.numeroDocEstCuenta = Convert.ToInt32(result.ExtraField);
                        this.LoadGridCompData(saldosTmp);

                        //Valida si tiene autorizacion de edicion
                        this._allowEditValuesInd = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CambioVlrConPassword).Equals("1") ? false : true;
                        if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                            this._allowEditValuesInd = true;                      
                        this.gvComponentes.Columns[this._unboundPrefix + "VlrPagar"].OptionsColumn.AllowEdit = this._allowEditValuesInd;

                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                    #endregion
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoComponentes);
                    MessageBox.Show(msg);
                    this.masterCliente.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para Imprimir archivo
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                #region Genera Reporte

                if (this.numeroDocEstCuenta != 0)
                {
                    byte diasFNC = this.estadoCuentaHistoria.DiasFNC.Value.HasValue ? this.estadoCuentaHistoria.DiasFNC.Value.Value : (byte)0;
                    string reportName = this._bc.AdministrationModel.Report_Cc_CarteraByNumeroDoc(this._documentID, this._nameProposito, this.numeroDocEstCuenta, this.dt_FechaCorte.DateTime, this.estadoCuentaHistoria.FechaInicialFNC.Value, diasFNC, false, ExportFormatType.pdf);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(this.numeroDocEstCuenta), null, reportName.ToString());
                    Process.Start(fileURl);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "TBPrint"));
            }
        }

        /// <summary>
        /// Click del boton Editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBEdit()
        {
            try
            {
                if (!this._allowEditValuesInd)
                {                  
                    string pass = string.Empty;
                    //Pide la contrasena del usuario que autoriza la edicion
                    if (Prompt.InputBox("Autorización de edición ", this._userAutoriza + " por favor ingrese su contraseña: ", ref pass, true) == DialogResult.OK)
                    {
                        if (string.IsNullOrEmpty(pass))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginNoData));
                            return;
                        }

                        #region Valida las credenciales y activa la edicion
                        UserResult userVal = _bc.AdministrationModel.seUsuario_ValidateUserCredentials(this._userAutoriza, pass);
                        if (userVal == UserResult.NotExists)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginFailure));
                            return;
                        }
                        else if (userVal == UserResult.IncorrectPassword)
                        {
                            DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._userAutoriza);
                            string ctrl = _bc.GetControlValue(AppControl.RepeticionesContrasenaBloqueo);
                            int repPermitidas = Convert.ToInt16(ctrl);

                            if ((repPermitidas - user.ContrasenaRep.Value.Value) == 0)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginBlockUser));
                            }
                            else
                            {
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LoginIncorrectPwd);
                                msg = string.Format(msg, repPermitidas - user.ContrasenaRep.Value.Value);
                                MessageBox.Show(msg);
                            }

                            return;
                        }
                        else if (userVal == UserResult.AlreadyMember)
                        {
                            this._allowEditValuesInd = true;
                            this.gvComponentes.Columns[this._unboundPrefix + "VlrPagar"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "itemEdit_Click"));
            }


        }

        #endregion         
    }
}