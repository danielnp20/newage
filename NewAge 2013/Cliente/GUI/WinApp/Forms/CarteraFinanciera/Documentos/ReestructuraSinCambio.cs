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
using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using System.Globalization;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReestructuraSinCambio : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private DTO_ccCliente _cliente = new DTO_ccCliente();
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_ccSolicitudDocu _headerSolicitud = new DTO_ccSolicitudDocu();
        private DTO_ccCreditoDocu _headerCredito = new DTO_ccCreditoDocu();
        private DTO_Credito _credito = new DTO_Credito();
        private List<DTO_ccSolicitudComponentes> _componentesVisibles = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudComponentes> _componentesContab = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudCompraCartera> _compCartera = new List<DTO_ccSolicitudCompraCartera>();
        private List<DTO_Cuota> _planPagos = new List<DTO_Cuota>();
        private List<DTO_Cuota> _cuotasExtras = new List<DTO_Cuota>();
        private DTO_PlanDePagos _liquidador = new DTO_PlanDePagos();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        //Variables formulario (campos)
        private string _tipoCreditoID = string.Empty;
        private string _clienteID = string.Empty;
        private string _lineaCreditoID = string.Empty;
        private string _plazo = string.Empty;
        private string _plazoPoliza = string.Empty;
        private int _libranzaID = 0;
        private int _libranzaValidID = 0;
        private TipoCredito _tipoCredito = TipoCredito.Nuevo;
        private SectorCartera _sector;

        //Variables auxiliares del formulario
        private DateTime periodo;
        private bool isValid;
        private bool validateData;
        private bool deleteOp;
        private bool liquidaInd = false;
        private bool liquidaAll = true;
        Dictionary<int, string> tiposCredito = new Dictionary<int, string>();
        Dictionary<string, decimal> compsNuevoValor = new Dictionary<string, decimal>();
        private string _cuentaAbonoCap;
        private bool readOnly = false;

        //Valores temporales
        private decimal vlrLibranza;
        private decimal vlrGiro;
        private int vlrSolicitadoPrestamo;
        private int vlrSolicitadoPoliza;
        private decimal porInteres;
        private decimal porInteresPoliza;
        private int edad = 0;
        private int vlrCuotaCancelada = 0;

        //Variables de mensajes
        private string msgFinDoc;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReestructuraSinCambio()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReestructuraSinCambio(string mod)
        {
            this.Constructor(null,mod);
        }

         /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReestructuraSinCambio(int libranza, string mod)
        {
            this.Constructor(libranza, mod);
        }
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public ReestructuraSinCambio(int libranza, bool readOnly)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.cf;
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddComponentesCols();
                this.AddComprasCols();
                this.AddPlanPagosCols();
                this.AddCuotasExtrasCols();

                //Deshabilita los controles de la poliza
                //this.dtPolizaIni.Enabled = false;
                this.dtPagoPoliza.Enabled = false;
                this.txtVlrSolicitadoPoliza.Enabled = false;
                //this.txtVlrPoliza.Enabled = false;
                //this.txtVlrCuotaPol.Enabled = false;
                this.txtInteresPoliza.Enabled = false;
                this.comboPlazoPol.Enabled = false;
                this.txtCta1Pol.Enabled = false;

                #region Carga la info de las actividades

                List<string> actividadesSolicitud = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DigitacionCreditoFinanciera);
                if (actividadesSolicitud.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, AppDocuments.DigitacionCreditoFinanciera.ToString()));
                }
                else
                {
                    string actividadFlujoSolicitud = actividadesSolicitud[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
                }

                #endregion
                #region Trae la solicitud
                this.readOnly = readOnly;
                DTO_DigitacionCredito sol = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(libranza, this._actFlujo.ID.Value);
                this.masterTipoCredito.Value = sol.Header.TipoCreditoID.Value;
                this.masterTipoCredito_Leave(null, null);
                this.masterCliente.Value = sol.Header.ClienteID.Value;
                this.masterCliente_Leave(null, null);
                this.lkpObligaciones.EditValue = libranza.ToString();
                FormProvider.Master.itemSave.Enabled = false;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "ReestructuraSinCambio"));
            }
        }

    /// <summary>
    /// Contrustor del formulario
    /// </summary>
    public void Constructor(int? libranza, string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);
                
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddComponentesCols();
                this.AddComprasCols();
                this.AddPlanPagosCols();
                this.AddCuotasExtrasCols();

                //Deshabilita los controles de la poliza
                //this.dtPolizaIni.Enabled = false;
                this.dtPagoPoliza.Enabled = false;
                this.txtVlrSolicitadoPoliza.Enabled = false;
                //this.txtVlrPoliza.Enabled = false;
                //this.txtVlrCuotaPol.Enabled = false;
                this.txtInteresPoliza.Enabled = false;
                this.comboPlazoPol.Enabled = false;
                this.txtCta1Pol.Enabled = false;

                #region Carga la info de las actividades

                List<string> actividadesSolicitud = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DigitacionCreditoFinanciera);
                if (actividadesSolicitud.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, AppDocuments.DigitacionCreditoFinanciera.ToString()));
                }
                else
                {
                    string actividadFlujoSolicitud = actividadesSolicitud[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoSolicitud, true);
                }

                #endregion
                #region Revisa si el formulario esta solo en modo de lectura
                this.readOnly = libranza.HasValue;
                if (this.readOnly)
                {
                    DTO_ccCreditoDocu cred = this._bc.AdministrationModel.GetCreditoByLibranza(libranza.Value);
                    this.masterTipoCredito.Value = cred.TipoCreditoID.Value;
                    this.masterTipoCredito_Leave(null, null);
                    this.masterCliente.Value = cred.ClienteID.Value;
                    this.masterCliente_Leave(null, null);
                    this.lkpObligaciones.EditValue = libranza.ToString();
                    FormProvider.Master.itemSave.Enabled = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "ReestructuraSinCambio"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.ReestructuracionSinCambio;
            this._frmModule = ModulesPrefix.cc;

            //Carga los datos por defecto
            string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            this._sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorLib);
            if (this._sector == SectorCartera.Financiero)
            {
                this.lblVlrFuturo.Visible = false;
                this.txtVlrFuturo.Visible = false;
                this.btnCrearCliente.Visible = false;
                this.masterPagaduria.Visible = false;
                this.chkDto1Cuota.Visible = false;
                this.txtVlrDto1Cuota.Visible = false;
            }
            else
            {
                this.txtPagare.Visible = false;
                this.txtPagarePol.Visible = false;
                this.txtPoliza.Visible = false;
                this.lblPagare.Visible = false;
                this.lblPagarePol.Visible = false;
                this.lblPoliza.Visible = false;
                this.chkDto1Cuota.Visible = true;
                this.txtVlrDto1Cuota.Visible = true;
            }

            #region Filtros
            List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();
            DTO_glConsultaFiltro fil = new DTO_glConsultaFiltro();
            fil.CampoFisico = "TipoCredito";
            fil.OperadorFiltro = OperadorFiltro.Igual;
            fil.ValorFiltro = "6";
            filtrosComplejos.Add(fil);
            #endregion
            this._bc.InitMasterUC(this.masterTipoCredito, AppMasters.ccTipoCredito, true, true, true, false, filtrosComplejos);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
            this._bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
            this._bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
            this._bc.InitMasterUC(this.masterConcesionario, AppMasters.ccConcesionario, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor1, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor2, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterCodeudor3, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);

            this.masterCentroCosto.EnableControl(false);
            this.masterPagaduria.EnableControl(false);
            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);

            //Establece la fecha de solicitud del credito
            //this.dtFechaCredito.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            //this.dtFechaCredito.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtFechaCredito.DateTime = DateTime.Today.Date;// new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            //Establece la fecha de fecha cuota 1 del credito
            this.dtFechaCta1.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtFechaCta1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));

            //Establece la fecha de solicitud de la Poliza
            this.dtPolizaIni.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            this.dtPolizaIni.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtPolizaIni.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            //Establece la fecha de la cuota 1 de la poliza
            this.dtPagoPoliza.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            this.dtPagoPoliza.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));

            //Carga los mensajes de la grilla
            this.msgFinDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFinDoc);

            //Solo se habilita cuando se quiere comprar cartera
            this.gcCompra.Enabled = false;

            //Crea las opciones del combo tipo credito
            tiposCredito.Add(1, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v1"));
            tiposCredito.Add(2, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v2"));
            tiposCredito.Add(3, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v3"));
            tiposCredito.Add(4, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v4"));
            tiposCredito.Add(5, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v5"));
            tiposCredito.Add(6, this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoCredito_v6"));

            this.txtCta1Pol.Text = "1";

            this.lkpObligaciones.Enabled = false;
            this.EnableHeaderNuevo(false);

            this._cuentaAbonoCap = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaPagosAbonosDeuda);
            
            //Ultimo consecutivo
            string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
            this.txtUltSolicitud.Text = consecutivoSolStr;
        }

        /// <summary>
        /// Agrega las columnas a la grilla 1
        /// </summary>
        private void AddComponentesCols()
        {
            try
            {
                #region Doc Solicitud Componentes
                //Campo de codigo
                GridColumn codigo = new GridColumn();
                codigo.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                codigo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_ComponenteCarteraID");
                codigo.UnboundType = UnboundColumnType.String;
                codigo.VisibleIndex = 0;
                codigo.Width = 50;
                codigo.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(codigo);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descripcion";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_Descripcion");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 1;
                descriptivo.Width = 100;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(descriptivo);

                //Cuota Valor
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this._unboundPrefix + "CuotaValor";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_ValorCuota");
                cuotaValor.UnboundType = UnboundColumnType.Decimal;
                cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cuotaValor.AppearanceCell.Options.UseTextOptions = true;
                cuotaValor.VisibleIndex = 2;
                cuotaValor.Width = 100;
                cuotaValor.OptionsColumn.AllowEdit = false;
                cuotaValor.ColumnEdit = this.editSpin;

                this.gvComponentes.Columns.Add(cuotaValor);

                //Valor Total
                GridColumn valorTotal = new GridColumn();
                valorTotal.FieldName = this._unboundPrefix + "TotalValor";
                valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_ValorTotal");
                valorTotal.UnboundType = UnboundColumnType.Decimal;
                valorTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotal.AppearanceCell.Options.UseTextOptions = true;
                valorTotal.VisibleIndex = 3;
                valorTotal.Width = 150;
                valorTotal.OptionsColumn.AllowEdit = false;
                valorTotal.ColumnEdit = this.editSpin;
                this.gvComponentes.Columns.Add(valorTotal);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "AddComponentesCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla 2
        /// </summary>
        private void AddComprasCols()
        {
            try
            {
                //FinancieraID
                GridColumn financieraID = new GridColumn();
                financieraID.FieldName = this._unboundPrefix + "FinancieraID";
                financieraID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_FinancieraID");
                financieraID.UnboundType = UnboundColumnType.String;
                financieraID.VisibleIndex = 0;
                financieraID.Width = 100;
                financieraID.ColumnEdit = this.editBtnGrid;
                financieraID.OptionsColumn.AllowEdit = false;
                this.gvCompra.Columns.Add(financieraID);

                //Libranza externa
                GridColumn documento = new GridColumn();
                documento.FieldName = this._unboundPrefix + "Documento";
                documento.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_DocumentoID");
                documento.UnboundType = UnboundColumnType.Integer;
                documento.VisibleIndex = 1;
                documento.Width = 80;
                documento.OptionsColumn.AllowEdit = false;
                this.gvCompra.Columns.Add(documento);

                //Valor Cuota
                GridColumn valorCuota = new GridColumn();
                valorCuota.FieldName = this._unboundPrefix + "VlrCuota";
                valorCuota.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_ValorCuota");
                valorCuota.UnboundType = UnboundColumnType.Decimal;
                valorCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorCuota.AppearanceCell.Options.UseTextOptions = true;
                valorCuota.VisibleIndex = 2;
                valorCuota.Width = 120;
                valorCuota.Visible = true;
                valorCuota.ColumnEdit = this.editSpin;
                valorCuota.OptionsColumn.AllowEdit = false;
                this.gvCompra.Columns.Add(valorCuota);

                //Valor Saldo
                GridColumn valorSaldo = new GridColumn();
                valorSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                valorSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_ValorSaldo");
                valorSaldo.UnboundType = UnboundColumnType.Decimal;
                valorSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorSaldo.AppearanceCell.Options.UseTextOptions = true;
                valorSaldo.VisibleIndex = 3;
                valorSaldo.Width = 120;
                valorSaldo.Visible = true;
                valorSaldo.ColumnEdit = this.editSpin;
                valorSaldo.OptionsColumn.AllowEdit = false;
                this.gvCompra.Columns.Add(valorSaldo);

                //AnticipoInd
                GridColumn docAnticipo = new GridColumn();
                docAnticipo.FieldName = this._unboundPrefix + "AnticipoInd";
                docAnticipo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_DocAnticipo");
                docAnticipo.UnboundType = UnboundColumnType.Boolean;
                docAnticipo.VisibleIndex = 4;
                docAnticipo.Width = 60;
                docAnticipo.Visible = true;
                docAnticipo.OptionsColumn.AllowEdit = false;
                this.gvCompra.Columns.Add(docAnticipo);

                //Ind Paz y Salvo
                GridColumn indPazySalvo = new GridColumn();
                indPazySalvo.FieldName = this._unboundPrefix + "IndRecibePazySalvo";
                indPazySalvo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCredito + "_RecibePazySalvo");
                indPazySalvo.UnboundType = UnboundColumnType.Boolean;
                indPazySalvo.VisibleIndex = 5;
                indPazySalvo.Width = 60;
                indPazySalvo.OptionsColumn.AllowEdit = false;
                indPazySalvo.Visible = true;

                this.gvCompra.Columns.Add(indPazySalvo);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "AddComprasCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de las cuota
        /// </summary>
        private void AddPlanPagosCols()
        {
            try
            {
                //Num Cuota
                GridColumn numCuota = new GridColumn();
                numCuota.FieldName = this._unboundPrefix + "NumCuota";
                numCuota.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_NumCuota");
                numCuota.UnboundType = UnboundColumnType.String;
                numCuota.VisibleIndex = 0;
                numCuota.Width = 50;
                numCuota.OptionsColumn.AllowEdit = false;
                this.gvPlanPagos.Columns.Add(numCuota);

                //FechaCuota
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 1;
                fecha.Width = 100;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvPlanPagos.Columns.Add(fecha);

                //Valor Cuota
                GridColumn valorCuota = new GridColumn();
                valorCuota.FieldName = this._unboundPrefix + "ValorCuota";
                valorCuota.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_ValorCuota");
                valorCuota.UnboundType = UnboundColumnType.Decimal;
                valorCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorCuota.AppearanceCell.Options.UseTextOptions = true;
                valorCuota.VisibleIndex = 2;
                valorCuota.Width = 100;
                valorCuota.OptionsColumn.AllowEdit = false;
                valorCuota.ColumnEdit = this.editSpin;
                this.gvPlanPagos.Columns.Add(valorCuota);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "AddCtasCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de las cuotas extras 
        /// </summary>
        private void AddCuotasExtrasCols()
        {
            try
            {
                //Campo de código
                GridColumn cuotaExtraID = new GridColumn();
                cuotaExtraID.FieldName = this._unboundPrefix + "NumCuota";
                cuotaExtraID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_CuotaID");
                cuotaExtraID.UnboundType = UnboundColumnType.Integer;
                cuotaExtraID.VisibleIndex = 0;
                cuotaExtraID.Width = 50;
                cuotaExtraID.OptionsColumn.AllowEdit = true;
                cuotaExtraID.ColumnEdit = this.editNums;
                this.gvCuotasExtras.Columns.Add(cuotaExtraID);

                //Cuota Valor
                GridColumn cuotaExtraValor = new GridColumn();
                cuotaExtraValor.FieldName = this._unboundPrefix + "ValorCuota";
                cuotaExtraValor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.DigitacionCreditoFinanciera + "_ValorCuota");
                cuotaExtraValor.UnboundType = UnboundColumnType.Decimal;
                cuotaExtraValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cuotaExtraValor.AppearanceCell.Options.UseTextOptions = true;
                cuotaExtraValor.VisibleIndex = 1;
                cuotaExtraValor.Width = 100;
                cuotaExtraValor.OptionsColumn.AllowEdit = true;
                cuotaExtraValor.ColumnEdit = this.editSpin;
                this.gvCuotasExtras.Columns.Add(cuotaExtraValor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "AddCuotasExtrasCols"));
            }
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeaderNuevo(bool enabled)
        {
            try
            {
                //Datos Generales
                this.lkpObligaciones.Enabled = enabled;
                this.masterCliente.EnableControl(enabled);
                this.txtPagare.Enabled = enabled;
                this.txtPagarePol.Enabled = enabled;
                this.masterAsesor.EnableControl(enabled);
                this.masterConcesionario.EnableControl(enabled);
                //Codeudores
                //this.masterCodeudor1.EnableControl(enabled);
                //this.masterCodeudor2.EnableControl(enabled);
                //this.masterCodeudor3.EnableControl(enabled);

                //Prestamo
                this.btn_Liquidar.Enabled = enabled;
                this.dtFechaCredito.Enabled = enabled;
                this.dtFechaCta1.Enabled = enabled;
                this.masterLineaCredito.EnableControl(enabled);
                this.txtPorCredito.Enabled = enabled;
                this.txtPlazo.Enabled = enabled;

                //Datos Liquidacion
                this.txtVlrSolicitado.Enabled = enabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnablePoliza(bool enabled)
        {
            try
            {
                //this.dtPolizaIni.Enabled = enabled;
                //this.dtPagoPoliza.Enabled = enabled;
                this.txtVlrSolicitadoPoliza.Enabled = enabled;
                //this.txtVlrPoliza.Enabled = enabled;
                //this.txtVlrCuotaPol.Enabled = enabled;
                this.txtInteresPoliza.Enabled = enabled;
                this.comboPlazoPol.Enabled = enabled;
                this.txtCta1Pol.Enabled = enabled;

             }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData(bool cleanObligaciones)
        {
            try
            {
                this.validateData = false;

                //Asigna eventos
                //this.chkCompraCartera.CheckedChanged -= new EventHandler(this.chkCompraCartera_CheckedChanged);
                this.gcCompra.Enabled = false;

                //Datos Generales
                if (cleanObligaciones)
                {
                    List<int> sol = new List<int>();
                    this.lkpObligaciones.Properties.DataSource = sol;
                    this.lkpObligaciones.EditValue = null;
                    this._libranzaID = 0;
                }

                this.masterCliente.Value = string.Empty;
                this.txtPagare.Text = string.Empty;
                this.txtPagarePol.Text = string.Empty;
                this.txtPoliza.Text = string.Empty;
                this.masterAsesor.Value = string.Empty;
                this.masterConcesionario.Value = string.Empty;
                this.masterCentroCosto.Value = string.Empty;

                //Codeudores
                this.masterCodeudor1.Value = string.Empty;
                this.masterCodeudor2.Value = string.Empty;
                this.masterCodeudor3.Value = string.Empty;
                this.masterPagaduria.Value = string.Empty;
                //Prestamo
                this.chkDto1Cuota.Checked = false;
                this.masterLineaCredito.Value = string.Empty;
                this.txtPorCredito.EditValue = string.Empty;
                this.txtPlazo.Text = string.Empty;

                //Datos Liquidacion
                this.txtVlrSolicitado.EditValue = 0;
                this.txtVlrGiro.EditValue = 0;
                this.txtVlrPrestamo.EditValue = 0;
                this.txtVlrAdicional.EditValue = 0;
                this.txtVlrCompra.EditValue = 0;
                this.txtVlrDescuento.EditValue = 0;
                this.txtVlrDto1Cuota.EditValue = 0;
                this.txtVlrFuturo.EditValue = 0;               

                //Datos Poliza
                this.txtVlrSolicitadoPoliza.EditValue = 0;
                this.txtVlrPoliza.EditValue = 0;
                this.txtVlrCuotaPol.EditValue = 0;
                this.txtInteresPoliza.EditValue = 0;
                this.comboPlazoPol.Text = string.Empty;
                this.txtCta1Pol.Text = string.Empty;
                this.dtPagoPoliza.Enabled = false;
                this.txtVlrSolicitadoPoliza.Enabled = false;
                this.txtInteresPoliza.Enabled = false;
                this.comboPlazoPol.Enabled = false;
                this.txtCta1Pol.Enabled = false;

                //Valores Cuotas
                this.txtCtaCredito.EditValue = 0;
                this.txtCtaPoliza.EditValue = 0;
                this.txtCtaTotal.EditValue = 0;

                //Footer
                this._ctrl = new DTO_glDocumentoControl();
                this._headerSolicitud = new DTO_ccSolicitudDocu();
                this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                this._compCartera = new List<DTO_ccSolicitudCompraCartera>();
                this._liquidador = new DTO_PlanDePagos();
                this._planPagos = new List<DTO_Cuota>();
                this._cuotasExtras = new List<DTO_Cuota>();
                this._credito = null;

                //Variables
                this.vlrLibranza = 0;
                this.vlrGiro = 0;
                this._clienteID = String.Empty;
                this._lineaCreditoID = String.Empty;
                this._plazo = String.Empty;
                this._plazoPoliza = String.Empty;
                this.vlrCuotaCancelada = 0;
                this.compsNuevoValor = new Dictionary<string, decimal>();

                //Ultimo Consecutivo
                string consecutivoSolStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoSolicitudes);
                this.txtUltSolicitud.Text = consecutivoSolStr;

                //Grillas
                this.gcComponentes.DataSource = this._componentesVisibles;
                this.gcCompra.DataSource = this._compCartera;
                this.gcPlanPagos.DataSource = this._planPagos;
                this.gcCuotasExtras.DataSource = this._cuotasExtras;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "CleanData"));
            }
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            try
            {
                #region Hace las Validaciones

                //Valida que el tipo de credito exista
                if (!this.masterTipoCredito.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoCredito.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterTipoCredito.Focus();
                    return false;
                }

                //Valida que exista el pagaré Poliza y Poliza si es Nuevo
                if (this._tipoCredito == TipoCredito.Nuevo && this._sector == SectorCartera.Financiero)
                {
                    if (string.IsNullOrEmpty(this.txtPagare.Text))
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPagare.Text);
                        MessageBox.Show(msg);
                        this.txtPagare.Focus();
                        return false;
                    }
                    if (string.IsNullOrEmpty(this.txtPagarePol.Text))
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPagarePol.Text);
                        MessageBox.Show(msg);
                        this.txtPagarePol.Focus();
                        return false;
                    }
                }             

                //Valida que el usuario exista
                if (!this.masterCliente.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCliente.Focus();
                    return false;
                }

                //Valida que el asesor exista
                if (!this.masterAsesor.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAsesor.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterAsesor.Focus();
                    return false;
                }

                //Valida que el asesor exista
                if (!this.masterConcesionario.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterConcesionario.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterConcesionario.Focus();
                    return false;
                }

                //Valida que la linea de credito exista
                if (!this.masterLineaCredito.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterAsesor.Focus();
                    return false;
                }

                //Valida que el valor prestamo sea valido
                if (Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) < 0)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitado.Text);
                    MessageBox.Show(msg);
                    return false;
                }

                //Valida que el valor de Giro sea valido
                if (Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture) < 0)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrGiro.Text);
                    MessageBox.Show(msg);
                    return false;
                }

                //Valida que la grilla de componentes no este vacia
                if ((this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio) && this._componentesVisibles.Count <= 0)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded);
                    MessageBox.Show(msg);
                    return false;
                }

                if(this._liquidador.Cuotas.Count == 0)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_LiquidacionRequerida);
                    MessageBox.Show(msg);
                    return false;
                }

                //Valida la info de la poliza
                if (!String.IsNullOrWhiteSpace(this.txtPoliza.Text))
                {
                    ////Valida el valor solicitado de la poliza
                    //if ((string.IsNullOrWhiteSpace(this.txtVlrSolicitadoPoliza.Text) || Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue) == 0) &&
                    //    this._poliza != null && this._poliza.FinanciadaIND.Value.Value)
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                    //    MessageBox.Show(msg);
                    //    return false;
                    //}

                    ////Valida el valor de la poliza
                    //if ((string.IsNullOrWhiteSpace(this.txtVlrPoliza.Text) || Convert.ToDecimal(this.txtVlrPoliza.EditValue) == 0) &&  
                    //    this._poliza != null && this._poliza.FinanciadaIND.Value.Value)
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrPoliza.Text);
                    //    MessageBox.Show(msg);
                    //    return false;
                    //}

                    //Agregar un indicador para ver si la info de la póliza es válida
                }

                //Cuotas extras
                if (this._cuotasExtras.Count > 0)
                {
                    int cuotasExtrasNum = this._cuotasExtras.Sum(x => x.NumCuota);
                    if (cuotasExtrasNum != Convert.ToInt32(this._plazo))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DifCuotasExtrasPlazo));
                        return false;
                    }                  
                    decimal sumPlanPagos = this._planPagos.Sum(x => x.ValorCuota);
                    decimal sumComponentes = this._componentesContab.FindAll(x=>!x.CompInvisibleInd.Value.Value).Sum(x => x.TotalValor.Value.Value);
                    if (sumComponentes != sumPlanPagos)
                    {
                        //Valida si existe el componente de vlrNominal para asignarle la diferencia de las cuotas extra
                        //bool compTipoLiquidaVlrNominalExist = false;
                        //string componenteVlrNominal = string.Empty;
                        //foreach (var comp in this._componentesContab)
                        //{
                        //    DTO_ccCarteraComponente dto = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, comp.ComponenteCarteraID.Value, true);
                        //    if (dto.TipoLiquida.Value == (byte)TipoLiquidacionCartera.AjustaVlrNominal)
                        //    {
                        //        compTipoLiquidaVlrNominalExist = true;
                        //        componenteVlrNominal = comp.ComponenteCarteraID.Value;
                        //        break;
                        //    }
                        //}
                        //if (!compTipoLiquidaVlrNominalExist)
                        //{
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "La sumatoria de las cuotas es diferente a la suma de los componentes, verifique nuevamente"));// DictionaryMessages.Cc_VlrCuotasExtras));
                            return false;
                        //}
                        //else
                        //{
                        //    DTO_ccSolicitudComponentes compVlrnominal = this._componentesContab.Find(x => x.ComponenteCarteraID.Value == componenteVlrNominal);
                        //    compVlrnominal.CuotaValor.Value = sumPlanPagos - sumComponentes; 
                        //}
                    }
                }

                //Valida que la linea de credito tenga habilitado el indicador para no permitir desembolso
                if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                {
                    if (this.vlrGiro != 0)
                    {
                        MessageBox.Show("El valor de Giro debe ser igual a $0 cuando es una reestructuración sin cambio de crédito");
                        this.txtVlrGiro.Focus();
                        return false;
                    }
                }               
                #endregion

                if (this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                {
                    // Nuevo
                    #region Carga los datos del header (ccSolicitudDocu)

                    this._headerSolicitud.TipoCreditoID.Value = this.masterTipoCredito.Value;
                    this._headerSolicitud.ClienteID.Value = this.masterCliente.Value;
                    this._headerSolicitud.ClienteRadica.Value = this.masterCliente.Value;
                    if (!string.IsNullOrWhiteSpace(this.txtPagare.Text))
                        this._headerSolicitud.Pagare.Value = (this.txtPagare.Text);
                    this._headerSolicitud.PagarePOL.Value = this.txtPagarePol.Text;
                    this._headerSolicitud.Poliza.Value = this.txtPoliza.Text;
                    this._headerSolicitud.LineaCreditoID.Value = this.masterLineaCredito.Value;
                    this._headerSolicitud.AsesorID.Value = this.masterAsesor.Value;
                    this._headerSolicitud.ConcesionarioID.Value = this.masterConcesionario.Value;
                    this._headerSolicitud.ConcesionarioID.Value = this.masterConcesionario.Value;
                    this._headerSolicitud.PagaduriaID.Value = this.masterPagaduria.Value;
                    this._headerSolicitud.Codeudor1.Value = this.masterCodeudor1.Value;
                    this._headerSolicitud.Codeudor2.Value = this.masterCodeudor2.Value;
                    this._headerSolicitud.Codeudor3.Value = this.masterCodeudor3.Value;
                    this._headerSolicitud.TipoCredito.Value = Convert.ToByte(this._tipoCredito);
                    this._headerSolicitud.FechaCuota1.Value = this.dtFechaCta1.DateTime;
                    this._headerSolicitud.PorInteres.Value = Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.TasaEfectivaCredito.Value = Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrSolicitado.Value = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrAdicional.Value = Convert.ToDecimal(this.txtVlrAdicional.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrPrestamo.Value = Convert.ToDecimal(this.txtVlrPrestamo.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrLibranza.Value = this.vlrLibranza;
                    this._headerSolicitud.VlrDescuento.Value = Convert.ToDecimal(this.txtVlrDescuento.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrGiro.Value = Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.Plazo.Value = Convert.ToInt16(this.txtPlazo.Text);
                    this._headerSolicitud.VlrCuota.Value = Convert.ToDecimal(this.txtCtaCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerSolicitud.VlrCupoDisponible.Value = 0;
                    this._headerSolicitud.ComponenteExtraInd.Value = this.chkExcluyeCompExtra.Checked;
                    this._headerSolicitud.DtoPrimeraCuotaInd.Value = this.chkDto1Cuota.Checked;
                    this._headerSolicitud.ValorDtoPrimeraCuota.Value = Convert.ToDecimal(this.txtVlrDto1Cuota.EditValue, CultureInfo.InvariantCulture);


                    //Compra
                    this._headerSolicitud.VlrCompra.Value = Convert.ToDecimal(this.txtVlrCompra.EditValue, CultureInfo.InvariantCulture);

                    //Poliza
                    this._headerSolicitud.PlazoSeguro.Value = this.comboPlazoPol.Text == "" ? Convert.ToInt16(0) : Convert.ToInt16(this.comboPlazoPol.Text);
                    this._headerSolicitud.VlrPoliza.Value = Convert.ToDecimal(this.txtVlrPoliza.EditValue, CultureInfo.InvariantCulture);
                    if (this._headerSolicitud.PlazoSeguro.Value != 0 && this._headerSolicitud.VlrPoliza.Value != 0)
                    {
                        this._headerSolicitud.FechaLiqSeguro.Value = this.dtPolizaIni.DateTime;
                        this._headerSolicitud.FechaVigenciaINI.Value = this.dtPagoPoliza.DateTime;
                        this._headerSolicitud.FechaVigenciaFIN.Value = this.dtPagoPoliza.DateTime.AddMonths(this._headerSolicitud.PlazoSeguro.Value.Value).AddDays(-1);
                        this._headerSolicitud.PorSeguro.Value = Convert.ToDecimal(this.txtInteresPoliza.EditValue, CultureInfo.InvariantCulture);
                        this._headerSolicitud.Cuota1Seguro.Value = this.txtCta1Pol.Text == "" ? Convert.ToInt16(0) : Convert.ToInt16(this.txtCta1Pol.Text);
                        this._headerSolicitud.VlrCuotaSeguro.Value = Convert.ToDecimal(this.txtCtaPoliza.EditValue, CultureInfo.InvariantCulture);
                        this._headerSolicitud.VlrFinanciaSeguro.Value = Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this._headerSolicitud.FechaLiqSeguro.Value = null;
                        this._headerSolicitud.FechaVigenciaINI.Value = null;
                        this._headerSolicitud.FechaVigenciaFIN.Value = null;
                        this._headerSolicitud.PorSeguro.Value = 0;
                        this._headerSolicitud.PlazoSeguro.Value = 0;
                        this._headerSolicitud.Cuota1Seguro.Value = 0;
                        this._headerSolicitud.VlrPoliza.Value = 0;
                        this._headerSolicitud.VlrFinanciaSeguro.Value = 0;
                        this._headerSolicitud.VlrCuotaSeguro.Value = 0;
                    }

                    #endregion
                    #region Carga los datos de la póliza

                    //if (this._poliza != null)
                    //{
                    //    this._poliza.PlazoFinancia.Value = this._headerSolicitud.PlazoSeguro.Value;
                    //    this._poliza.TasaFinancia.Value = this._headerSolicitud.PorSeguro.Value;
                    //    this._poliza.VlrCuotaFinancia.Value = this._headerSolicitud.VlrCuotaSeguro.Value;
                    //    this._poliza.Cuota1Financia.Value = this._headerSolicitud.Cuota1Seguro.Value;
                    //    this._poliza.ValorFinancia.Value = this._headerSolicitud.VlrFinanciaSeguro.Value;
                    //}

                    #endregion
                }
                else if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                {
                    // RestructuracionSinCambio
                    #region Carga los datos del header (ccCreditoDocu)

                    //Info básica
                    this._headerCredito.TipoCreditoID.Value = this.masterTipoCredito.Value;
                    this._headerCredito.ClienteID.Value = this.masterCliente.Value;
                    this._headerCredito.Poliza.Value = this.txtPoliza.Text;
                    this._headerCredito.TipoCredito.Value = Convert.ToByte(this._tipoCredito);

                    this._headerCredito.TipoCreditoID.Value = this.masterTipoCredito.Value;
                    this._headerCredito.ClienteID.Value = this.masterCliente.Value;
                    if (!string.IsNullOrWhiteSpace(this.txtPagare.Text))
                        this._headerCredito.Pagare.Value = (this.txtPagare.Text);
                    this._headerCredito.PagarePOL.Value = this.txtPagarePol.Text;
                    this._headerCredito.Poliza.Value = this.txtPoliza.Text;
                    this._headerCredito.LineaCreditoID.Value = this.masterLineaCredito.Value;
                    this._headerCredito.AsesorID.Value = this.masterAsesor.Value;
                    this._headerCredito.ConcesionarioID.Value = this.masterConcesionario.Value;
                    this._headerCredito.PagaduriaID.Value = this.masterPagaduria.Value;
                    this._headerCredito.Codeudor1.Value = this.masterCodeudor1.Value;
                    this._headerCredito.Codeudor2.Value = this.masterCodeudor2.Value;
                    this._headerCredito.Codeudor3.Value = this.masterCodeudor3.Value;
                    this._headerCredito.TipoCredito.Value = Convert.ToByte(this._tipoCredito);
                    this._headerCredito.Plazo.Value = Convert.ToInt16(this.txtPlazo.Text);
                    this._headerCredito.FechaCuota1.Value = this.dtFechaCta1.DateTime;
                    this._headerCredito.FechaVto.Value = this.dtFechaCredito.DateTime.AddMonths(this._headerCredito.Plazo.Value.Value);
                    this._headerCredito.PorInteres.Value = Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.TasaEfectivaCredito.Value = Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrSolicitado.Value = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrAdicional.Value = Convert.ToDecimal(this.txtVlrAdicional.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrPrestamo.Value = Convert.ToDecimal(this.txtVlrPrestamo.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrLibranza.Value = this.vlrLibranza;
                    this._headerCredito.VlrDescuento.Value = Convert.ToDecimal(this.txtVlrDescuento.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrGiro.Value = Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture);                  
                    this._headerCredito.VlrCuota.Value = Convert.ToDecimal(this.txtCtaCredito.EditValue, CultureInfo.InvariantCulture);
                    this._headerCredito.VlrCupoDisponible.Value = 0;
                    this._headerCredito.ComponenteExtraInd.Value = this.chkExcluyeCompExtra.Checked;

                    //Compra
                    this._headerSolicitud.VlrCompra.Value = Convert.ToDecimal(this.txtVlrCompra.EditValue, CultureInfo.InvariantCulture);                    
                    #endregion                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "ValidateHeader"));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Funcion que carga los valores previamente guardados
        /// </summary>
        private void SetValuesForReestructuracion()
        {
            try
            {
                #region Header

                this.masterCliente.Value = this._headerCredito.ClienteID.Value;
                this.txtPagare.Text = this._headerCredito.Pagare.Value;
                this.txtPagarePol.Text = this._headerCredito.PagarePOL.Value;
                this.masterPagaduria.Value = this._headerCredito.PagaduriaID.Value;
                this.masterCodeudor1.Value = this._headerCredito.Codeudor1.Value;
                this.masterCodeudor2.Value = this._headerCredito.Codeudor2.Value;
                this.masterCodeudor3.Value = this._headerCredito.Codeudor3.Value;
                this.masterAsesor.Value = this._headerCredito.AsesorID.Value;
                this.masterLineaCredito.Value = this._headerCredito.LineaCreditoID.Value;
                this.masterConcesionario.Value = this._headerCredito.ConcesionarioID.Value;
                this.txtPlazo.Text = this._headerCredito.Plazo.Value.ToString();
                this.txtVlrSolicitado.EditValue = this._compCartera.Sum(x => x.VlrSaldo.Value);
                this.txtPorCredito.EditValue = this._headerCredito.PorInteres.Value;

                DTO_ccConcesionario conc = (DTO_ccConcesionario)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, false, this.masterConcesionario.Value, true);
                if (conc != null)
                    this.masterCentroCosto.Value = conc.CtoCostoNormal.Value;

                this.dtFechaCta1.DateTime = this._headerCredito.FechaCuota1.Value.Value;
                #endregion
                #region Valores del crédito

                this.vlrGiro = this._headerCredito.VlrGiro.Value.Value;
                this.txtVlrAdicional.EditValue = 0;
                this.txtVlrCompra.EditValue = this._headerCredito.VlrCompra.Value;
                this.txtVlrGiro.EditValue = 0;
                this.txtVlrPrestamo.EditValue = this._compCartera.Sum(x => x.VlrSaldo.Value);
                this.txtVlrDescuento.EditValue = 0;
                #endregion
                #region Recalcula el valor futuro
                List<DTO_ccCarteraComponente> comps = this._bc.AdministrationModel.ccCarteraComponente_GetByLineaCredito(this._headerCredito.LineaCreditoID.Value);
                if ( this._sector == SectorCartera.Solidario && comps.Exists(x => x.TipoLiquida.Value == (byte)TipoLiquidacionCartera.AjustaVlrNominal))
                {
                    DTO_ccCarteraComponente compAjuste = comps.Find(x => x.TipoLiquida.Value == (byte)TipoLiquidacionCartera.AjustaVlrNominal);
                    decimal factor = compAjuste.PorcentajeID.Value.HasValue ? compAjuste.PorcentajeID.Value.Value : 0; //Trae el factor
                    decimal vlrCuotaIni = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) * (factor / 100);//Calcular el valor de la Cuota
                    decimal vlCuotaFin = (Math.Round(vlrCuotaIni / 50, 0) * 50); // Redondea el valor de la cuota a multiplos de 50
                    decimal vlrTotal1 = vlCuotaFin * Convert.ToInt32(this.txtPlazo.Text); // Calcular el nuevo valor solicitado

                    decimal vlrTotal2 = (Math.Round(Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) / Convert.ToInt32(this.txtPlazo.Text) / 500, 0) * 500) * Convert.ToInt32(this.txtPlazo.Text);// Calcula el valor solicitado a multiplos de 500
                    this.txtVlrFuturo.EditValue = Convert.ToInt32(vlrTotal1 + vlrTotal2);//suma los 2 valores
                }
                #endregion
                #region Otras variables y controles

                //Variables
                this._libranzaID = Convert.ToInt32(this.lkpObligaciones.EditValue);
                this._plazo = this._headerCredito.Plazo.Value.ToString();
                //this._polizaID = this.txtPoliza.Text;
                #endregion
                #region Poliza

                //this.EnablePoliza(true);
                //this.txtPoliza.Text = this._poliza.Poliza.Value;
                //this.dtFechaCredito.DateTime = this._poliza.FechaLiqSeguro.Value.Value;
                //this.dtPolizaIni.DateTime = this._poliza.FechaVigenciaINI.Value.Value;
                //this.dtPagoPoliza.DateTime = this._poliza.FechaPagoSeguro.Value.Value;
                //this.txtVlrPoliza.EditValue = this._poliza.VlrPoliza.Value.Value;

                //if (this._poliza.Solicitud.Value.HasValue)
                //{
                //    this._solicitudRenovacion = this._poliza.Solicitud.Value.Value;

                //    this._plazoPoliza = this._poliza.PlazoFinancia.Value.ToString();
                //    this.comboPlazoPol.Text = this._plazoPoliza;
                //    this.txtCta1Pol.Text = this._poliza.Cuota1Financia.Value.ToString();
                //    this.txtVlrSolicitadoPoliza.EditValue = this._poliza.ValorFinancia.Value.HasValue ? this._poliza.ValorFinancia.Value : this._poliza.VlrPoliza.Value;
                //    this.txtInteresPoliza.EditValue =  this._poliza.TasaFinancia.Value.HasValue? this._poliza.TasaFinancia.Value : 0;

                //    this.btn_Liquidar_Click(null, null);
                //}
                //else
                {
                    //if (this._poliza.VlrPoliza.Value == 0)
                        this.txtVlrSolicitadoPoliza.EditValue = 0;
                    this.txtVlrCuotaPol.EditValue = 0;
                    this.txtInteresPoliza.EditValue = 0.0;
                    this.txtCta1Pol.Text = "1";// this._credito.PlanPagos.FirstOrDefault(p => p.VlrSeguro.Value == 0 && p.FechaCuota.Value > this.dtFechaCredito.DateTime).CuotaID.Value.ToString();

                    this.comboPlazoPol.Text = "12";
                }

                #endregion
                this.LoadCompraCartera();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "GetValuesPoliza"));
                this._libranzaValidID = 0;
                this.lkpObligaciones.Focus();
            }
        }

        /// <summary>
        /// Funcion que trae el valor de los interes de los componentes
        /// </summary>
        private void GetIntereses(List<DTO_ccSolicitudComponentes> componentes)
        {
            string compInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
            string compInteresSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            foreach (DTO_ccSolicitudComponentes componente in componentes)
            {
                if (componente.ComponenteCarteraID.Value == compInteres)
                {
                    if(Convert.ToDecimal(this.txtPorCredito.EditValue) != 0)
                        this.porInteres = Convert.ToDecimal(this.txtPorCredito.EditValue);
                    else
                        this.porInteres = componente.Porcentaje.Value.HasValue ? componente.Porcentaje.Value.Value : 0;
                }
                else if (componente.ComponenteCarteraID.Value == compInteresSeguro)
                {
                    if (Convert.ToDecimal(this.txtInteresPoliza.EditValue) != 0)
                        this.porInteresPoliza = Convert.ToDecimal(this.txtInteresPoliza.EditValue);
                    else
                        this.porInteresPoliza = componente.Porcentaje.Value.HasValue ? componente.Porcentaje.Value.Value : 0;
                }
            }
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow_Compra()
        {
            DTO_ccSolicitudCompraCartera solCompra = new DTO_ccSolicitudCompraCartera();
            try
            {
                isValid = false;
                #region Asigna datos a la fila
                solCompra.FinancieraID.Value = string.Empty;
                solCompra.Documento.Value = 0;
                solCompra.DocCompra.Value = 0;
                solCompra.VlrCuota.Value = 0;
                solCompra.VlrSaldo.Value = 0;
                solCompra.AnticipoInd.Value = false;
                solCompra.IndRecibePazySalvo.Value = false;
                solCompra.ExternaInd.Value = true;
                #endregion
                this._compCartera.Add(solCompra);
                this.gcCompra.DataSource = this._compCartera;
                this.gcCompra.RefreshDataSource();
                if (this._sector == SectorCartera.Solidario && this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                    this.vlrCuotaCancelada = Convert.ToInt32(this._compCartera.Sum(x => x.VlrCuota.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "AddNewRow"));
            }
        }

        ///<summary>
        ///Metodo que calcula el valor de la compra de la cartera
        ///</summary>
        private void CalcularGiro()
        {
            try
            {
                decimal dto1Cuota = this._headerSolicitud != null && this._headerSolicitud.ValorDtoPrimeraCuota.Value.HasValue ? this._headerSolicitud.ValorDtoPrimeraCuota.Value.Value : 0;
                decimal vlrTotalCompra = (from p in this._compCartera select p.VlrSaldo.Value.Value).Sum();
                this.txtVlrGiro.EditValue = _liquidador.VlrPrestamo - vlrTotalCompra - _liquidador.VlrDescuento;
                this._liquidador.VlrGiro = Convert.ToInt32(this.txtVlrGiro.EditValue);
                this.vlrGiro = this._liquidador.VlrGiro;
                this.txtVlrCompra.EditValue = vlrTotalCompra;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "CalcularGiro"));
            }

        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_CompraCartera(int fila)
        {
            try
            {
                this.gvCompra.PostEditor();
                this.isValid = true;

                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;

                    #region FinancieraID

                    rowValid = true;
                    fieldName = "FinancieraID";
                    GridColumn colFinanciera = this.gvCompra.Columns[this._unboundPrefix + fieldName];

                    //Valida la financiera
                    rowValid = _bc.ValidGridCell(this.gvCompra, this._unboundPrefix, fila, fieldName, false, true, false, AppMasters.ccFinanciera);
                    if (rowValid)
                        this.gvCompra.SetColumnError(colFinanciera, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region Documento

                    rowValid = true;
                    fieldName = "Documento";
                    GridColumn colDocumento = this.gvCompra.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvCompra, this._unboundPrefix, fila, fieldName, false, false, true, false);
                    if (rowValid)
                        this.gvCompra.SetColumnError(colDocumento, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region VlrSaldo

                    fieldName = "VlrSaldo";
                    GridColumn colVlrSaldo = this.gvCompra.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvCompra, this._unboundPrefix, fila, fieldName, false, false, true, false);
                    if (!rowValid)
                        this.isValid = false;

                    if (rowValid)
                        this.gvCompra.SetColumnError(colVlrSaldo, string.Empty);

                    #endregion
                    #region VlrCuota

                    rowValid = true;
                    fieldName = "VlrCuota";
                    GridColumn colVlrCuota = this.gvCompra.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvCompra, this._unboundPrefix, fila, fieldName, false, true, true, false);
                    if (rowValid)
                        this.gvCompra.SetColumnError(colVlrCuota, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region Relacion documento / financiera

                    if (this.isValid)
                    {
                        int count =
                        (
                            from c in this._compCartera
                            where c.FinancieraID.Value == this._compCartera[fila].FinancieraID.Value && c.Documento.Value.Value == this._compCartera[fila].Documento.Value.Value
                            select c
                        ).Count();

                        if (count > 1)
                        {
                            this.gvCompra.SetColumnError(colDocumento, this.msgFinDoc);
                            this.isValid = false;
                            rowValid = false;
                        }
                        else
                            this.gvCompra.SetColumnError(colDocumento, string.Empty);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "ValidateRow_CompraCartera"));
            }
        }

        /// <summary>
        /// Funcion que valida las columnas de la grilla
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateCalcularLiquidacion()
        {
            try
            {
                liquidaInd = true;
                //Valida que exista el cliente
                if (!this.masterCliente.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterAsesor.Focus();
                    liquidaInd = false;
                }

                //Valida que la linea de credito exista
                if (!this.masterLineaCredito.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterLineaCredito.Focus();
                    liquidaInd = false;
                }

                if (this.liquidaAll)
                {
                    //Valida que el valor prestamo sea valido
                    if (Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) <= 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitado.Text);
                        MessageBox.Show(msg);
                        this.txtVlrSolicitado.Focus();
                        liquidaInd = false;
                    }

                    //Valida que el interes del credito sea valido
                    if (Convert.ToDecimal(this.txtPorCredito.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrPoliza.Text);
                        MessageBox.Show(msg);
                        this.txtPorCredito.Focus();
                        liquidaInd = false;
                    }

                    //Valida que el plazo sea valido
                    if (string.IsNullOrWhiteSpace(this.txtPlazo.Text))
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazo.Text);
                        MessageBox.Show(msg);
                        this.txtPlazo.Focus();
                        liquidaInd = false;
                    }

                    if (this._cuotasExtras.Count > 0)
                    {
                        this._plazo = this.txtPlazo.Text;
                        int cuotasExtrasNum = this._cuotasExtras.Sum(x => x.NumCuota);
                        if (cuotasExtrasNum != Convert.ToInt32(this._plazo))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DifCuotasExtrasPlazo));
                            liquidaInd = false;
                        }
                        else if(string.IsNullOrEmpty(this._plazo))
                        {
                            MessageBox.Show(string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col),this.txtPlazo.Text));
                            liquidaInd = false;
                        }
                    }
                }

                //Valida que el valor de la poliza sea positivo
                if (Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture) == 0)
                {
                    //if (!this.liquidaAll)
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                    //    MessageBox.Show(msg);
                    //    this.txtVlrSolicitado.Focus();
                    //    liquidaInd = false;

                    //}
                    //else
                    //{

                    //    if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                    //    {
                    //        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    //        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_CreditoSinPoliza);
                    //        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                    //        {
                    //            this.txtVlrSolicitadoPoliza.Focus();
                    //            liquidaInd = false;
                    //        }
                    //    }
                    //}
                }
                else if (!liquidaAll && Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture) < 0)
                {
                //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitadoPoliza.Text);
                //    MessageBox.Show(msg);
                //    this.txtVlrSolicitado.Focus();
                //    liquidaInd = false;
                }
                else
                {
                    ////Valida que el interes de la poliza sea valido
                    //if (Convert.ToDecimal(this.txtInteresPoliza.EditValue, CultureInfo.InvariantCulture) < 0)
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblInteresPol.Text);
                    //    MessageBox.Show(msg);
                    //    this.txtInteresPoliza.Focus();
                    //    liquidaInd = false;
                    //}

                    ////Valida que el plazo de la poliza sea valido
                    //if (string.IsNullOrWhiteSpace(this.comboPlazoPol.Text))
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazoPol.Text);
                    //    MessageBox.Show(msg);
                    //    this.txtPlazo.Focus();
                    //    liquidaInd = false;
                    //}

                    ////Valida que el plazo de la poliza sea valido
                    //if (string.IsNullOrWhiteSpace(this.txtCta1Pol.Text))
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblCta1Pol.Text);
                    //    MessageBox.Show(msg);
                    //    this.txtPlazo.Focus();
                    //    liquidaInd = false;
                    //}

                    ////Valida que el plazo de la poliza sea valido
                    //if (Convert.ToInt32(this.txtCta1Pol.Text) <= 0)
                    //{
                    //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblCta1Pol.Text);
                    //    MessageBox.Show(msg);
                    //    this.txtPlazo.Focus();
                    //    liquidaInd = false;
                    //}
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "ValidateGetLiquidacion"));
            }
        }

        /// <summary>
        /// Crea un plan de pagos de una cuota sin intereses
        /// </summary>
        private object CalcularPlanPagosNoFinanciado()
        {
            try
            {
                DTO_PlanDePagos planPagos = new DTO_PlanDePagos();
                
                #region Variables

                string componenteSeguroID = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                DTO_ccCarteraComponente componenteSeguro = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, componenteSeguroID, true);

                int vlrCuota = Convert.ToInt32(this.txtVlrSolicitadoPoliza.EditValue);
                List<DTO_Cuota> cuotas = new List<DTO_Cuota>();
                List<string> componentesIDs = new List<string>();
                List<int> valoresComponentes = new List<int>();
                List<DTO_ccSolicitudComponentes> componentes = new List<DTO_ccSolicitudComponentes>();

                #endregion
                #region Calcula los componentes

                //componentesDesc.Add(componenteSeguro.Descriptivo.Value);
                //valoresComponentes.Add(vlrCuota);

                DTO_ccSolicitudComponentes comp = new DTO_ccSolicitudComponentes();
                comp.ComponenteCarteraID.Value = componenteSeguro.ID.Value;
                comp.Descripcion.Value = componenteSeguro.Descriptivo.Value;
                comp.CuotaValor.Value = vlrCuota;
                comp.TotalValor.Value = vlrCuota;
                comp.Porcentaje.Value = 100;
                comp.CompInvisibleInd.Value = false;
                comp.PorCapital.Value = 100;

                componentes.Add(comp);

                #endregion
                #region Calcula la cuota

                DTO_Cuota cuota = new DTO_Cuota();
                cuota.Capital = 0;
                cuota.Intereses = 0;
                cuota.Seguro = vlrCuota;
                //cuota.Fecha = this._poliza.FechaVigenciaINI.Value.Value;
                cuota.Componentes = componentesIDs;
                cuota.ValoresComponentes = valoresComponentes;
                cuota.ValorCuota = vlrCuota;
                cuota.NumCuota = 1;

                cuotas.Add(cuota);
                #endregion
                #region Agrega los valores al plan de pagos
            
                planPagos.ComponentesAll = componentes;
                planPagos.ComponentesUsuario = componentes;
                planPagos.Cuotas = cuotas;
                planPagos.CuotasCredito = cuotas;
                planPagos.VlrPrestamoPoliza = vlrCuota;
                planPagos.VlrCompra = 0;
                planPagos.VlrGiro = 0; ;
                planPagos.VlrPoliza = vlrCuota;
                //planPagos.VlrCuota = vlrCuota;
                planPagos.VlrCuotaPoliza = vlrCuota;
                planPagos.TasaTotal = 0;

                #endregion

                this.porInteresPoliza = 0;
                return planPagos;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "CalcularPlanPagosNoFinanciado");

                return result;
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            //this.isModalFormOpened = true;
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
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {
               // this.isModalFormOpened = false;
            }
        }

        /// <summary>
        /// Carga las compras de cartera
        /// </summary>
        private void LoadCompraCartera()
        {
            try
            {
                    if (this._compCartera.Count > 0)
                    {
                        this.validateData = true;
                        this.gcCompra.Enabled = true;
                        this.gcCompra.DataSource = this._compCartera;
                        if (this._sector == SectorCartera.Solidario && this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                            this.vlrCuotaCancelada = Convert.ToInt32(this._compCartera.Sum(x => x.VlrCuota.Value));
                        
                        this.gvCompra.MoveFirst();
                        if (this._compCartera[0].ExternaInd.Value.Value && !this._compCartera[0].IndRecibePazySalvo.Value.Value)
                        {
                            foreach (GridColumn c in this.gvCompra.Columns)
                                if (c.FieldName != this._unboundPrefix + "IndRecibePazySalvo")
                                    c.OptionsColumn.AllowEdit = false;
                        }
                        else
                        {
                            foreach (GridColumn c in this.gvCompra.Columns)
                                c.OptionsColumn.AllowEdit = false;
                        }

                        #region Valida el saldo de la compra cuando es Reestructuracion-Abono capital
                        if (this._compCartera[0].EC_Proposito.Value.HasValue && 
                            (this._compCartera[0].EC_Proposito.Value.Value == (byte)PropositoEstadoCuenta.RestructuracionAbono ||
                            this._compCartera[0].EC_Proposito.Value.Value == (byte)PropositoEstadoCuenta.RestructuracionPlazo))
                        {
                            bool validSaldo = true;
                            string msg = string.Empty;
                            //Valida que el vlr solicitado sea igual al valor del Estado de Cuenta
                            if (this._compCartera.Sum(x => x.VlrSaldo.Value) != Convert.ToInt64(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture))
                            {
                                msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_SaldoAbonoInvalid);
                                validSaldo = false;
                            }
                            //Trae y valida la cuenta para abonos de credito
                            DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, _cuentaAbonoCap, true);
                            if (cta != null && validSaldo)
                            {
                                //decimal vlrSaldo = this._bc.AdministrationModel.Saldo_GetByDocumentoCuenta(true, this.periodo, Convert.ToInt64(this._cliente.TerceroID.Value), _cuentaAbonoCap, string.Empty);
                                //decimal vlrSaldo = 0;
                                //if (saldo != null)
                                //{
                                //     vlrSaldo = saldo.DbOrigenLocML.Value.Value + saldo.DbOrigenExtML.Value.Value + saldo.CrOrigenLocML.Value.Value +
                                //                       saldo.CrOrigenExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value + saldo.DbSaldoIniExtML.Value.Value + 
                                //                       saldo.CrSaldoIniLocML.Value.Value + saldo.CrSaldoIniExtML.Value.Value;                                       
                                //}
                                //Valida si existe saldos para el vlr de abono del estado Cuenta
                                //if (vlrSaldo == 0)
                                //{
                                //    msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_SaldoAbonoNotRegist);
                                //    validSaldo = false;
                                //}
                                //else if (Math.Abs(vlrSaldo) != this._compCartera.Sum(x => x.EC_ValorAbono.Value))
                                //{
                                //    msg = this._bc.GetResource(LanguageTypes.Messages, "El valor de abono ($" + vlrSaldo.ToString() + ") no corresponde con el registrado en el estado de cuenta");
                                //    validSaldo = false;
                                //}
                            }
                            else if (validSaldo)
                            {
                                msg = this._bc.GetResource(LanguageTypes.Messages, "NO hay cuenta registrada para abono de Deuda (16" + AppControl.cc_CuentaPagosAbonosDeuda + ")");
                                validSaldo = false;
                            }

                            if (!validSaldo)
                            {
                                MessageBox.Show(msg);
                                //this.chkCompraCartera.Checked = false;
                                this.validateData = false;
                                this.gcCompra.Enabled = true;
                                this.gcCompra.DataSource = null;
                                if (this._sector == SectorCartera.Solidario)
                                    this.vlrCuotaCancelada = 0;
                                return;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        this.validateData = false;
                        this.gcCompra.Enabled = true;
                        this.gcCompra.DataSource = null;
                        if (this._sector == SectorCartera.Solidario)
                            this.vlrCuotaCancelada = 0;
                    }
                  
                if (this._sector == SectorCartera.Solidario)
                {
                    this.btn_Liquidar_Click(null, null);
                }

                this.validateData = true;
                this.CalcularGiro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "LoadCompraCartera"));
            }
        }

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
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemUpdate.Visible = true;
                    FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSearch.Visible = false;
                    FormProvider.Master.itemUpdate.Visible = false;
                    if(this.readOnly)
                        FormProvider.Master.itemSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Trae los datos de coTercero, para almacenarlos en los campos de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterTipoCredito_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._tipoCreditoID != this.masterTipoCredito.Value)
                {
                    this.CleanData(true);
                    this.EnableHeaderNuevo(false);
                    this.EnablePoliza(false);

                    if (this.masterTipoCredito.ValidID)
                    {
                        this.masterCliente.EnableControl(true);
                        this._tipoCreditoID = this.masterTipoCredito.Value;
                        DTO_ccTipoCredito tipoDTO = (DTO_ccTipoCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, false, this.masterTipoCredito.Value, true);

                        this._tipoCredito = (TipoCredito)Enum.Parse(typeof(TipoCredito), tipoDTO.TipoCredito.Value.Value.ToString());
                        this.txtTipoCredito.Text = this.tiposCredito[tipoDTO.TipoCredito.Value.Value];

                        if (this._tipoCredito == TipoCredito.Nuevo)
                        { 
                            this.liquidaAll = true;
                        }
                        else if (this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                        {
                           // this.chkCompraCartera.Enabled = false;
                        }                        
                    }
                    else
                    {
                        //this.chkCompraCartera.Enabled = false;
                        this.txtTipoCredito.Text = string.Empty;
                        this.masterCliente.EnableControl(false);
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoCredito.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                this._tipoCreditoID = string.Empty;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Trae los datos de coTercero, para almacenarlos en los campos de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._clienteID != this.masterCliente.Value)
                {
                    this.validateData = false;
                    if (this.masterCliente.ValidID)
                    {
                        this.btnCrearCliente.Enabled = false;
                        this._clienteID = this.masterCliente.Value;
                        this._cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.lkpObligaciones.Enabled = true;

                        if (this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                        {
                            //Nuevo
                            List<DTO_ccSolicitudDocu> solicitudes = this._bc.AdministrationModel.GetSolicitudesByCliente(this._clienteID);
                            solicitudes = solicitudes.Where(s => s.Estado.Value == (byte)EstadoDocControl.SinAprobar || s.Estado.Value == (byte)EstadoDocControl.ParaAprobacion).ToList();
                            solicitudes = solicitudes.Where(s => s.VlrSolicitado.Value.Value > 0).ToList();
                          
                            List<int?> sol = (from p in solicitudes select p.Libranza.Value).Distinct().ToList();
                            this.lkpObligaciones.Properties.DataSource = sol;
                        }
                        else if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                        {
                            //RestructuracionSinCambio
                            List<DTO_ccCreditoDocu> creditos = this._bc.AdministrationModel.GetCreditoByCliente(this._clienteID);
                            creditos = creditos.Where(c => !c.CanceladoInd.Value.Value).ToList();
                            List<int?> creds = (from p in creditos select p.Libranza.Value).Distinct().ToList();
                            this.lkpObligaciones.Properties.DataSource = creds;
                        }                       
                    }
                    else
                    {
                        this.lkpObligaciones.Enabled = false;
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                        MessageBox.Show(msg);
                        this.btnCrearCliente.Enabled = true;
                    }

                    this.validateData = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Trae los datos del centro de costo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterConcesionario_Leave(object sender, EventArgs e)
        {
            if (this.masterConcesionario.ValidID)
            {
                //Llena el centro de costo
                DTO_ccConcesionario conc = (DTO_ccConcesionario)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, false, this.masterConcesionario.Value, true);
                this.masterCentroCosto.Value = conc.CtoCostoNormal.Value;
            }
            else
                this.masterCentroCosto.Value = string.Empty;
        }

        /// <summary>
        /// Cambio de solicitud
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkpObligaciones_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int currentLib = Convert.ToInt32(this.lkpObligaciones.EditValue);
                if (this.lkpObligaciones.EditValue != null && !String.IsNullOrWhiteSpace(this.lkpObligaciones.EditValue.ToString()) && currentLib != 0)
                {
                    bool gotInfo = false;
                    string tmp = this.lkpObligaciones.EditValue.ToString();
                    this._libranzaID = Convert.ToInt32(this.lkpObligaciones.EditValue);
                                       
                    this.CleanData(false);

                    if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                    {
                        //Restructuracion Sin Cambio
                        #region Trae la informacion del credito

                        this._credito = this._bc.AdministrationModel.GetCredito_All(this._libranzaID);
                        if (this._credito.CreditoDocu != null)
                        {                            
                            #region Trae y valida la información de la póliza
                            this._headerCredito = this._credito.CreditoDocu;
                            #endregion
                            #region Carga la info del crédito y la póliza
                           
                            gotInfo = true;
                            this._libranzaValidID = this._libranzaID;
                            this._ctrl = this._credito.DocControl;

                            //Habilita los controles
                            this.btn_Liquidar.Enabled = true;

                            if (this._sector == SectorCartera.Solidario && this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                                this.vlrCuotaCancelada = Convert.ToInt32(this._compCartera.Sum(x => x.VlrCuota.Value));


                            #region Carga los valores del crédito y la póliza

                            this.EnableHeaderNuevo(true);

                            //Asigna las compras del credito de acuerdo al tipo de Credito
                            DTO_ccTipoCredito tipoDTO = (DTO_ccTipoCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, false, this._tipoCreditoID, true);
                            //if (this._sector == SectorCartera.Financiero)
                            //{
                            //    if (tipoDTO != null && tipoDTO.AbonaCapitalInd.Value.Value)
                            //        this._compCartera = this._credito.CompraCartera.FindAll(x => x.EC_Proposito.Value == (byte)PropositoEstadoCuenta.RestructuracionAbono);
                            //    else
                            //        this._compCartera = this._credito.CompraCartera.FindAll(x => x.EC_Proposito.Value == (byte)PropositoEstadoCuenta.RecogeSaldo);
                            //}
                            //else
                            this._compCartera = this._credito.CompraCartera;


                            //Carga los valores 
                            this.SetValuesForReestructuracion();

                            #endregion
                            #region Carga grillas en inicia controles
                            this.masterCliente_Leave(sender, e);
                            if (Convert.ToDecimal(this.txtPorCredito.EditValue) != 0)
                                this.btn_Liquidar_Click(sender, e);
                            else
                            {
                                this.gcCompra.Enabled = false;
                                this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                                this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                                //this._compCartera = new List<DTO_ccSolicitudCompraCartera>();
                                this._liquidador = new DTO_PlanDePagos();
                                this._planPagos = new List<DTO_Cuota>();
                                this._cuotasExtras = new List<DTO_Cuota>();

                                this.gcComponentes.DataSource = this._componentesVisibles;
                                //this.gcCompra.DataSource = this._compCartera;
                                this.gcPlanPagos.DataSource = this._planPagos;
                                this.gcCuotasExtras.DataSource = this._cuotasExtras;
                            }
                            #endregion                            
                            this.txtVlrSolicitado.ReadOnly = true;
                            this.dtFechaCredito.Enabled = true;
                            this.dtFechaCta1.Enabled = true;
                            //Llama los eventos
                            this.validateData = true;
                            this.isValid = true;
                            #endregion
                        }
                        #endregion
                    }      
                    if (!gotInfo)
                    {
                        this._libranzaValidID = 0;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                        MessageBox.Show(msg);

                        this.lkpObligaciones.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "lkpSolicitudes_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que calcula nuevamente los valores de la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrSolicitado_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal vlrSolicitadoTemp = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                if (validateData && this.vlrSolicitadoPrestamo != vlrSolicitadoTemp)
                {
                    this._lineaCreditoID = string.Empty;
                    this._plazo = string.Empty;
                    this.btn_Liquidar_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "txtVlrSolicitado_Leave"));
            }
        }

        /// <summary>
        /// Evento que se encarga de calcular la liquidacion del credito
        /// </summary>
        private void btn_Liquidar_Click(object sender, EventArgs e)
        {
            try
            {
                this.chkVerCompExtra.Checked = false;
                int vlrFuturo = Convert.ToInt32(this.txtVlrFuturo.EditValue, CultureInfo.InvariantCulture);
                if (vlrFuturo == 0 && this.txtVlrFuturo.Visible)
                {
                    MessageBox.Show("Debe digitar un valor futuro diferente de $0  antes de realizar la liquidación");
                    return;
                }
                else
                {
                    if (this._sector != SectorCartera.Financiero)
                    {
                        #region Recalcula el valor futuro
                        List<DTO_ccCarteraComponente> comps = this._bc.AdministrationModel.ccCarteraComponente_GetByLineaCredito(this._headerSolicitud.LineaCreditoID.Value);
                        if (comps.Exists(x => x.TipoLiquida.Value == (byte)TipoLiquidacionCartera.AjustaVlrNominal))
                        {
                            DTO_ccCarteraComponente compAjuste = comps.Find(x => x.TipoLiquida.Value == (byte)TipoLiquidacionCartera.AjustaVlrNominal);
                            decimal factor = compAjuste.PorcentajeID.Value.HasValue ? compAjuste.PorcentajeID.Value.Value : 0; //Trae el factor
                            decimal vlrCuotaIni = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) * (factor / 100);//Calcular el valor de la Cuota
                            decimal vlCuotaFin = (Math.Round(vlrCuotaIni / 50, 0) * 50); // Redondea el valor de la cuota a multiplos de 50
                            decimal vlrTotal1 = vlCuotaFin * Convert.ToInt32(this.txtPlazo.Text); // Calcular el nuevo valor solicitado

                            decimal vlrTotal2 = (Math.Round(Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) / Convert.ToInt32(this.txtPlazo.Text) / 500, 0) * 500) * Convert.ToInt32(this.txtPlazo.Text);// Calcula el valor solicitado a multiplos de 500
                            vlrFuturo = Convert.ToInt32(vlrTotal1 + vlrTotal2);//suma los 2 valores
                            this.txtVlrFuturo.EditValue = vlrFuturo;//Nuevo valor futuro
                        }
                        #endregion
                    }
                }
                this.ValidateCalcularLiquidacion();
                if (this.liquidaInd)
                {
                    this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                    this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                    bool hasLiquidacion = true;
                    
                    #region Establece los valores para calcular el credito

                    int vlrGiro = Convert.ToInt32(Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture));
                    this.vlrSolicitadoPrestamo = Convert.ToInt32(Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture));
                    this.vlrSolicitadoPoliza = Convert.ToInt32(Convert.ToDecimal(this.txtVlrSolicitadoPoliza.EditValue, CultureInfo.InvariantCulture));

                    this._lineaCreditoID = this.masterLineaCredito.Value;
                    this._plazo = this.txtPlazo.Text;
                    this.porInteres = Convert.ToDecimal(this.txtPorCredito.Text.Replace("%", string.Empty));
                    
                    int plazoPol = 0;
                    int cta1Pol = 0;
                    int vlrCuotaPol = 0;
                    int plazo = Convert.ToInt32(this._plazo);
                    if (this.vlrSolicitadoPoliza > 0)
                    {
                        this._plazoPoliza = this.comboPlazoPol.Text;
                        this.porInteresPoliza = Convert.ToDecimal(this.txtInteresPoliza.Text.Replace("%", string.Empty).Trim());

                        plazoPol = Convert.ToInt32(this._plazoPoliza);
                        cta1Pol = Convert.ToInt16(this.txtCta1Pol.Text);
                    }

                    #endregion
                    #region Cálculo de variables

                    //Cálculo de la edad
                    TimeSpan difFecha = this.dtFechaCredito.DateTime.Subtract(this._cliente.FechaNacimiento.Value.Value);
                    this.edad = (int)Math.Floor((double)difFecha.Days / 365);

                    //Calcula la fecha de liquidación y la fecha de la primera cuota
                    DateTime fechaCuota1 = this.dtFechaCta1.DateTime;                    
                    #endregion

                    object res = null;
                    {
                        res = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._lineaCreditoID, this.vlrSolicitadoPrestamo, this.vlrSolicitadoPoliza, vlrFuturo,
                            vlrGiro, plazo, plazoPol, this.edad, this.dtFechaCredito.DateTime, fechaCuota1, this.porInteres, this.porInteresPoliza,
                            cta1Pol, vlrCuotaPol, this.liquidaAll, this._cuotasExtras, this.compsNuevoValor, this._ctrl.NumeroDoc.Value.Value, this.masterTipoCredito.Value, this.chkExcluyeCompExtra.Checked);
                    }

                    if (res.GetType() == typeof(DTO_TxResult))
                    {
                        hasLiquidacion = false;
                        MessageForm msg = new MessageForm((DTO_TxResult)res);
                        msg.ShowDialog();
                        this.isValid = false;
                    }
                    else
                    {
                        this.isValid = true;
                        this._liquidador = (DTO_PlanDePagos)res;

                        res = this._bc.AdministrationModel.GenerarPlanPagosFinanciera(this._lineaCreditoID, this.vlrSolicitadoPrestamo, this.vlrSolicitadoPoliza, vlrFuturo,
                          vlrGiro, plazo, plazoPol, this.edad, this.dtFechaCredito.DateTime, fechaCuota1, this.porInteres, this.porInteresPoliza,
                         cta1Pol, vlrCuotaPol, this.liquidaAll, this._cuotasExtras,this.compsNuevoValor, this._ctrl.NumeroDoc.Value.Value, this.masterTipoCredito.Value, this.chkExcluyeCompExtra.Checked);

                        this._liquidador = (DTO_PlanDePagos)res;
                        this._liquidador.VlrAdicional = 0;
                        this._liquidador.VlrDescuento = 0;

                        #region Valida las fechas si corresponde al ultimo dia para recalcular
                        if (this._sector != SectorCartera.Financiero)
                        {
                            if (fechaCuota1.Day == DateTime.DaysInMonth(fechaCuota1.Year, fechaCuota1.Month))
                            {
                                foreach (DTO_Cuota cta in this._liquidador.Cuotas)
                                {
                                    cta.Fecha = new DateTime(cta.Fecha.Year, cta.Fecha.Month, DateTime.DaysInMonth(cta.Fecha.Year, cta.Fecha.Month));
                                    if (cta.Fecha.Day == 31)
                                        cta.Fecha = new DateTime(cta.Fecha.Year, cta.Fecha.Month, 30);
                                }
                                foreach (DTO_Cuota ctaExt in this._liquidador.CuotasExtras)
                                {
                                    ctaExt.Fecha = new DateTime(ctaExt.Fecha.Year, ctaExt.Fecha.Month, DateTime.DaysInMonth(ctaExt.Fecha.Year, ctaExt.Fecha.Month));
                                    if (ctaExt.Fecha.Day == 31)
                                        ctaExt.Fecha = new DateTime(ctaExt.Fecha.Year, ctaExt.Fecha.Month, 30);
                                }
                                foreach (DTO_Cuota ctaCred in this._liquidador.CuotasCredito)
                                {
                                    ctaCred.Fecha = new DateTime(ctaCred.Fecha.Year, ctaCred.Fecha.Month, DateTime.DaysInMonth(ctaCred.Fecha.Year, ctaCred.Fecha.Month));
                                    if (ctaCred.Fecha.Day == 31)
                                        ctaCred.Fecha = new DateTime(ctaCred.Fecha.Year, ctaCred.Fecha.Month, 30);
                                }
                            }                            
                        }
                        #endregion

                        if (this._tipoCredito == TipoCredito.Nuevo || this._tipoCredito == TipoCredito.Refinanciado || this._tipoCredito == TipoCredito.RestructuracionConCambio)
                        {
                            #region Credito Nuevo

                            #region Carga las variables y calcula el interes
                            this._componentesVisibles = this._liquidador.ComponentesUsuario;
                            this._componentesContab = this._liquidador.ComponentesAll;
                            this._planPagos = this._liquidador.Cuotas;
                            this._cuotasExtras = ObjectCopier.Clone(this._liquidador.CuotasExtras);

                            if (this._liquidador.Cuotas.Count != 0)
                            {
                                this.txtCtaCredito.EditValue = this._liquidador.VlrCuota;
                                if (this._sector != SectorCartera.Financiero && this.chkDto1Cuota.Checked)
                                    this.txtVlrDto1Cuota.EditValue = this.vlrCuotaCancelada <= this._liquidador.VlrCuota? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0;
                                this.txtCtaPoliza.EditValue = this._liquidador.VlrCuotaPoliza;
                                this.txtCtaTotal.EditValue = this._liquidador.VlrCuota + this._liquidador.VlrCuotaPoliza;
                            }

                            this.GetIntereses(this._componentesVisibles);
                            #endregion
                            #region Asigna los valores calculados

                            //Valores Credito
                            if (this._sector != SectorCartera.Financiero && this.chkDto1Cuota.Checked)
                            {
                                this._liquidador.VlrDescuento += Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                                this._liquidador.VlrGiro -= Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                            }
                            this.vlrGiro = this._liquidador.VlrGiro;
                            this.txtVlrAdicional.EditValue = 0;
                            this.txtVlrCompra.EditValue = this._liquidador.VlrCompra;
                            this.txtVlrGiro.EditValue = this._liquidador.VlrGiro;
                            this.txtVlrPrestamo.EditValue = this._liquidador.VlrPrestamo;                           
                            this.txtVlrDescuento.EditValue = 0;
                            this.txtVlrCuotaPol.EditValue =0;
                            this.txtPorCredito.EditValue = this.porInteres;

                            //this.txtVlrPoliza.EditValue = this._liquidador.VlrPoliza);
                            this._liquidador.VlrPoliza = 0;

                            //Valores Poliza
                            this.txtInteresPoliza.EditValue = this.porInteresPoliza;

                            //Variables
                            this.vlrLibranza = this._liquidador.VlrLibranza;

                            this.CalcularGiro();

                            #endregion
                            #region Actualiza las grillas

                            if (hasLiquidacion && this._componentesVisibles.Count == 0)
                            {
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                                MessageBox.Show(msg);
                            }

                            this.gcComponentes.DataSource = this._componentesVisibles;
                            this.gcPlanPagos.DataSource = this._planPagos;
                            this.gcCuotasExtras.DataSource = this._cuotasExtras;

                            #endregion

                            #endregion
                        }
                        else if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                        {
                            #region Carga las variables y calcula el interes
                            this._componentesVisibles = this._liquidador.ComponentesUsuario;
                            this._componentesContab = this._liquidador.ComponentesAll;
                            this._planPagos = this._liquidador.Cuotas;
                            this._cuotasExtras = ObjectCopier.Clone(this._liquidador.CuotasExtras);

                            if (this._liquidador.Cuotas.Count != 0)
                            {
                                this.txtCtaCredito.EditValue = this._liquidador.VlrCuota;
                                if (this._sector != SectorCartera.Financiero && this.chkDto1Cuota.Checked)
                                    this.txtVlrDto1Cuota.EditValue = this.vlrCuotaCancelada <= this._liquidador.VlrCuota ? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0;
                                this.txtCtaPoliza.EditValue =0;
                                this.txtCtaTotal.EditValue = this._liquidador.VlrCuota + this._liquidador.VlrCuotaPoliza;
                            }

                            //this.GetIntereses(this._componentesVisibles);
                            #endregion
                            #region Asigna los valores calculados

                            //Valores Credito
                            if (this._sector != SectorCartera.Financiero && this.chkDto1Cuota.Checked)
                            {
                                this._liquidador.VlrDescuento += Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                                this._liquidador.VlrGiro -= Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                            }
                            this.vlrGiro = this._liquidador.VlrGiro;
                            this.txtVlrAdicional.EditValue = this._liquidador.VlrAdicional;
                            this.txtVlrCompra.EditValue = this._liquidador.VlrCompra;
                            this.txtVlrGiro.EditValue = this._liquidador.VlrGiro;
                            this.txtVlrPrestamo.EditValue = this._liquidador.VlrPrestamo;
                            this.txtVlrDescuento.EditValue = 0;
                            this.txtVlrCuotaPol.EditValue = 0;
                            this.txtPorCredito.EditValue = this.porInteres;

                            //this.txtVlrPoliza.EditValue = this._liquidador.VlrPoliza);
                            this._liquidador.VlrPoliza = 0;//this.vlrSolicitadoPoliza;

                            //Valores Poliza
                            this.txtInteresPoliza.EditValue = 0;// this.porInteresPoliza;

                            //Variables
                            this.vlrLibranza = this._liquidador.VlrLibranza;

                            this.CalcularGiro();

                            #endregion
                            #region Actualiza las grillas

                            if (hasLiquidacion && this._componentesVisibles.Count == 0)
                            {
                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                                MessageBox.Show(msg);
                            }

                            this.gcComponentes.DataSource = this._componentesVisibles;
                            this.gcPlanPagos.DataSource = this._planPagos;
                            this.gcCuotasExtras.DataSource = this._cuotasExtras;

                            #endregion

                            #region RestructuracionSinCambio

                            if (this._liquidador.Cuotas.Count != 0)
                            {
                                //this.txtCtaPoliza.EditValue = this._liquidador.VlrCuotaPoliza;
                                //this.txtCtaTotal.EditValue = this._liquidador.VlrCuota + this._liquidador.VlrCuotaPoliza;
                                //this.txtVlrCuotaPol.EditValue = this._liquidador.VlrCuotaPoliza;
                                //int cuotaID = Convert.ToInt32(this.txtCta1Pol.Text);
                                //List<DTO_Cuota> cuotasTemp = ObjectCopier.Clone(this._planPagos);                               
                                //foreach (DTO_Cuota cuota in this._liquidador.Cuotas)
                                //{
                                //    try {
                                //        cuotasTemp[cuotaID - 1].ValorCuota += cuota.ValorCuota;
                                //        cuotaID++;
                                //    }
                                //    catch (Exception)
                                //    {
                                //        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "Cantidad de cuotas de la póliza excede a la última cuota del crédito"));
                                //        return;
                                //    }
                                //}                            

                                //this._liquidador.VlrPoliza = this.vlrSolicitadoPoliza;

                                ////Valores Poliza
                                //this.txtInteresPoliza.EditValue = this.porInteresPoliza;

                                ////Actualiza el plan de pagos
                                //this.gcPlanPagos.DataSource = cuotasTemp;
                            }

                            #endregion
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "btn_Liquidar_Click"));
            }
        }

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Deshabilita el scroll del spin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEditSpin_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }        

        /// <summary>
        /// Event que se ejecuta al marcar la opcion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkVerCompExtra_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkVerCompExtra.Checked)
                {
                    this._componentesContab = this._componentesContab.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                    this.gcComponentes.DataSource = this._componentesContab;
                }                   
                else
                {
                    this._componentesVisibles = this._componentesVisibles.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                    this.gcComponentes.DataSource = this._componentesVisibles;
                }   
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaCredito_EditValueChanged(object sender, EventArgs e)
        {
            if (this._headerSolicitud != null && !this._headerSolicitud.FechaCuota1.Value.HasValue)              
            {
                if (this._sector == SectorCartera.Financiero)
                    this.dtFechaCta1.DateTime = this.dtFechaCredito.DateTime.AddMonths(1);
                else
                    this.dtFechaCta1.DateTime = new DateTime(this.dtFechaCredito.DateTime.AddMonths(1).Year, this.dtFechaCredito.DateTime.AddMonths(1).Month, this.dtFechaCredito.DateTime.AddMonths(1).Month == 2? 28 : 30);
            }
        }


        /// <summary>
        /// valida el valor futuro del credito
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void txtVlrFuturo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt64(this.txtVlrFuturo.EditValue) < Convert.ToInt64(this.txtVlrSolicitado.EditValue))
                    MessageBox.Show("El valor futuro no puede ser inferior al valor solicitado");
                else
                {
                    decimal plazo = Convert.ToInt32(this.txtPlazo.Text);
                    if ((long)(Convert.ToInt64(this.txtVlrFuturo.EditValue) / plazo) != Convert.ToDecimal(this.txtVlrFuturo.EditValue) / plazo)
                    {
                        MessageBox.Show("El valor de la cuota debe ser un número entero, (Vlr Futuro/Plazo) ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "txtVlrFuturo_Leave"));
            }
        }

        /// <summary>
        /// Crea los clientes de una solicitud
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void btnCrearCliente_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                var data = this._bc.AdministrationModel.RegistroSolicitud_GetBySolicitud(this._libranzaID);
                if (data == null)
                {
                    MessageBox.Show("No existe una libranza valida para crear el cliente");
                    return;
                }
                DTO_TxResultDetail res = this._bc.AdministrationModel.ccCliente_AddFromSource(this._documentID, data);
                result.Details = new List<DTO_TxResultDetail>();
                result.Details.Add(res);
                if (res.Message == ResultValue.OK.ToString())
                    result.Result = ResultValue.OK;
                else
                    result.Result = ResultValue.NOK;

                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();

                if (result.Result == ResultValue.OK)
                {
                    DTO_DigitacionCredito sol = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(this._libranzaID, this._actFlujo.ID.Value);                   
                    this.masterTipoCredito.Value = sol.Header.TipoCreditoID.Value;
                    this.masterTipoCredito_Leave(null, null);
                    this.masterCliente.Value = sol.Header.ClienteID.Value;
                    this.masterCliente_Leave(null, null);
                    //this._libranzaID = 0;
                    this.lkpObligaciones.EditValue = null;
                    this.lkpObligaciones.EditValue = this._libranzaID.ToString();
                    FormProvider.Master.itemSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "btnCrearClientes_Click"));
            }
        }

        /// <summary>
        /// Al cambiar el dto de cuota 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDto1Cuota_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkDto1Cuota.Checked && this._liquidador != null)
            {
                this.txtVlrDto1Cuota.EditValue = this.vlrCuotaCancelada <= this._liquidador.VlrCuota ? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0;
                if (this._headerSolicitud != null && this._sector != SectorCartera.Financiero)
                    this._headerSolicitud.ValorDtoPrimeraCuota.Value = this.vlrCuotaCancelada <= this._liquidador.VlrCuota ? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0;
                this._liquidador.VlrDescuento += (this.vlrCuotaCancelada <= this._liquidador.VlrCuota ? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0);
                this.txtVlrDescuento.EditValue = this._liquidador.VlrDescuento;
                this.CalcularGiro();
            }
            else
            {
                this.txtVlrDto1Cuota.EditValue = 0;
                if (this._headerSolicitud != null && this._sector != SectorCartera.Financiero)
                    this._headerSolicitud.ValorDtoPrimeraCuota.Value = 0;
                this._liquidador.VlrDescuento -= (this.vlrCuotaCancelada <= this._liquidador.VlrCuota ? this._liquidador.VlrCuota - this.vlrCuotaCancelada : 0);
                this.txtVlrDescuento.EditValue = this._liquidador.VlrDescuento;
                this.CalcularGiro();
            }

        }

        #endregion Eventos Formulario

        #region Eventos Grillas

        //Generales
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "DateTime")
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "DateTime")
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double"
                        || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime")
                    {
                        e.Value = e.Value.ToString();//pi.GetValue(dto, null);
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double"
                            || fi.FieldType.Name == "Decimal" || fi.FieldType.Name == "DateTime")
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
        private void gvComponentes_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                GridColumn col = this.gvComponentes.Columns[this._unboundPrefix + fieldName];
                if (fieldName == "TotalValor")
                {
                    int vlrAdicional = 0;
                    int vlrDescuento = 0;
                    int vlrSolicitado = Convert.ToInt32(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                    foreach (var c in this._componentesContab.FindAll(x=>x.CompInvisibleInd.Value.Value))
                    {
                        DTO_ccCarteraComponente dto = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, c.ComponenteCarteraID.Value, true);
                        c.TotalValor.Value = c.TotalValor.Value ?? 0;
                        if (dto.TipoComponente.Value == (byte)TipoComponente.MayorValor)
                            vlrAdicional += (int)c.TotalValor.Value;
                        else if (dto.TipoComponente.Value == (byte)TipoComponente.DescuentoGiro)
                            vlrDescuento += (int)c.TotalValor.Value;
                    }
                    this._liquidador.VlrAdicional = vlrAdicional;
                    this._liquidador.VlrDescuento = vlrDescuento; 
                    this.txtVlrAdicional.EditValue = this._liquidador.VlrAdicional;
                    if (this._sector != SectorCartera.Financiero && this.chkDto1Cuota.Checked)
                    {
                        this._liquidador.VlrDescuento += Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                        this._liquidador.VlrGiro -= Convert.ToInt32(this.txtVlrDto1Cuota.EditValue);
                    }
                    this.txtVlrDescuento.EditValue = 0;
                    this.txtVlrPrestamo.EditValue = vlrSolicitado + this._liquidador.VlrAdicional;
                    this.txtVlrGiro.EditValue = vlrSolicitado - this._liquidador.VlrDescuento - this._liquidador.VlrCompra;
                    this._liquidador.VlrGiro = vlrSolicitado - this._liquidador.VlrDescuento - this._liquidador.VlrCompra;

                    //Agrega los componentes modificados
                    DTO_ccSolicitudComponentes row = e.RowHandle >= 0 ? (DTO_ccSolicitudComponentes)this.gvComponentes.GetRow(e.RowHandle) : null;
                    if (row != null)
                    {
                        this.compsNuevoValor.Remove(row.ComponenteCarteraID.Value);
                        this.compsNuevoValor.Add(row.ComponenteCarteraID.Value, row.TotalValor.Value.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "gvComponentes_CellValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta la momento de cambiar entre filas de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvComponentes_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridColumn col = this.gvComponentes.Columns[this._unboundPrefix + "TotalValor"];
                DTO_ccSolicitudComponentes row = (DTO_ccSolicitudComponentes)this.gvComponentes.GetRow(e.FocusedRowHandle);
                if (row != null)
                {
                    if (row.CompInvisibleInd.Value.Value)
                        col.OptionsColumn.AllowEdit = true;
                    else
                        col.OptionsColumn.AllowEdit = false;
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "gvComponentes_FocusedRowChanged"));
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
                string colName = this.gvCompra.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
                int fila = this.gvPlanPagos.FocusedRowHandle;
                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(fila, colName, origin);
                if (!String.IsNullOrWhiteSpace(origin.Text))
                {
                    DTO_ccFinanciera financiera = (DTO_ccFinanciera)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFinanciera, false, origin.Text, false);
                    bool pazySalvo = financiera.PazySalvoInd.Value.Value;
                    this._compCartera[fila].IndRecibePazySalvo.Value = !pazySalvo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "editBtnGrid_ButtonClick"));
            }
        }

        //Detalles

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcCompra_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvCompra.FocusedRowHandle;
                if (validateData)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.ValidateRow_CompraCartera(fila);
                        if (this.isValid)
                        {
                            this.AddNewRow_Compra();
                            this.gvCompra.FocusedRowHandle = this._compCartera.Count - 1;
                        }
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        if (fila >= 0 && this._compCartera[fila].ExternaInd.Value.Value && !this._compCartera[fila].IndRecibePazySalvo.Value.Value)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.isValid = true;
                                this.deleteOp = true;
                                this._compCartera.RemoveAt(fila);
                                this.gcCompra.RefreshDataSource();
                                if (this._compCartera.Count > 0)
                                    this.gvCompra.FocusedRowHandle = fila - 1;
                                this.CalcularGiro();
                                this.deleteOp = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla antes de salir de esta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.validateData)
            {
                int fila = e.RowHandle;
                if (!this.deleteOp)
                {
                    this.ValidateRow_CompraCartera(fila);
                    if (!this.isValid)
                        e.Allow = false;
                }

                this.CalcularGiro();
            }
        }

        /// <summary>
        /// Evento que valida las columna de la grilla despues de editarlas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (fieldName == "FinancieraID")
                {
                    DTO_ccFinanciera financiera = (DTO_ccFinanciera)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFinanciera, false, e.Value.ToString(), false);
                    if (financiera != null && !financiera.PazySalvoInd.Value.Value)
                        this._compCartera[e.RowHandle].IndRecibePazySalvo.Value = true;
                    else
                        this._compCartera[e.RowHandle].IndRecibePazySalvo.Value = false;
                }

                if (fieldName == "VlrSaldo")
                {
                    if (e.Value == null)
                        this._compCartera[e.RowHandle].VlrSaldo.Value = 0;
                    this.CalcularGiro();
                }

                if (fieldName == "VlrCuota")
                {
                    if (e.Value == null)
                        this._compCartera[e.RowHandle].VlrCuota.Value = 0;
                }
                this.gcCompra.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "gvDetail_CellValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta la momento de cambiar entre filas de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetail_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int fila = e.FocusedRowHandle;
            if (this.validateData && fila >= 0)
            {
                if (this._compCartera[fila].ExternaInd.Value.Value && !this._compCartera[fila].IndRecibePazySalvo.Value.Value)
                {
                    foreach (GridColumn c in this.gvCompra.Columns)
                        if (c.FieldName != this._unboundPrefix + "IndRecibePazySalvo" && c.FieldName != this._unboundPrefix + "AnticipoInd")
                            c.OptionsColumn.AllowEdit = true;
                }
                else
                {
                    foreach (GridColumn c in this.gvCompra.Columns)
                        c.OptionsColumn.AllowEdit = false;
                }
            }
        }

        //Cuotas Extras

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcCuotasExtra_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this._tipoCredito != TipoCredito.RestructuracionSinCambio || this._ctrl == null)
                        e.Handled = true;

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this._cuotasExtras.Add(new DTO_Cuota() { NumCuota = 0, ValorCuota = 0 });
                        this.gvCuotasExtras.RefreshData();
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this._cuotasExtras.RemoveAt(this.gvCuotasExtras.FocusedRowHandle);
                            this.gvCuotasExtras.RefreshData();
                        }

                        e.Handled = true;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs-", "gcCuotasExtra_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCuotasExtras_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "NumCuota") 
            {
                int value = Convert.ToInt32(((GridView)sender).EditingValue);
                this._cuotasExtras[e.RowHandle].NumCuota = value;
            }
            else if (fieldName == "ValorCuota")
            {
                int value = Convert.ToInt32(((GridView)sender).EditingValue);
                this._cuotasExtras[e.RowHandle].ValorCuota = value;
            }
        }

        #endregion Enventos Grilla

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.lkpObligaciones.EditValue = 0;
                this._libranzaID = 0;
                this._libranzaValidID = 0;
                this.vlrCuotaCancelada = 0;
                this._tipoCreditoID = string.Empty;
                this.masterTipoCredito.Value = string.Empty;
                this.CleanData(true);
                this.EnableHeaderNuevo(false);
                this.EnablePoliza(false);

                this.masterTipoCredito.Value = string.Empty;
                this._tipoCreditoID = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvPlanPagos.PostEditor();
                this.gvCompra.PostEditor();
                this.gvCuotasExtras.PostEditor();

                if (this._compCartera.Count > 0)
                    this.ValidateRow_CompraCartera(this.gvCompra.FocusedRowHandle);

                if (this.ValidateHeader() && this.isValid)
                {
                    DTO_TxResult result = new DTO_TxResult();
                    if (this._tipoCredito == TipoCredito.RestructuracionSinCambio)
                    {
                        #region Restructuracion Sin Cambio Credito
                        DTO_DigitacionCredito digCredito = new DTO_DigitacionCredito();
                        //Asigna a this._ctrl la fecha selecionada en el documento
                        this._ctrl.FechaDoc.Value = this.liquidaAll == true ? this.dtFechaCredito.DateTime : this.dtPolizaIni.DateTime;

                        //Agrega el indicador del tipo de credito a realizar (Credito o Poliza)
                        this._headerSolicitud.LiquidaAll.Value = this.liquidaAll;
                        //Agrega las cuota extras
                        this._liquidador.CuotasExtras = this._cuotasExtras;

                        //Carga la info del crédito
                        digCredito.AddData(this._ctrl, this._headerSolicitud, this._liquidador, this._componentesContab, this._compCartera);

                        result = _bc.AdministrationModel.RestructuracionSinCambio(AppDocuments.DigitacionCreditoFinanciera,this._actFlujo.ID.Value, this._headerCredito, digCredito);

                        #endregion
                    }                   

                    if (result.Result == ResultValue.OK)
                    {
                        this.CleanData(true);
                        this.EnableHeaderNuevo(false);
                        this.EnablePoliza(false);

                        this._tipoCreditoID = string.Empty;
                        this.masterTipoCredito.Value = string.Empty;
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();

                        //CIerra el formulario
                        FormProvider.CloseCurrent();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReestructuraSinCambio.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas
    }
}