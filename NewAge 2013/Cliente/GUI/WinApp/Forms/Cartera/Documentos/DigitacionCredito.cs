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
    public partial class DigitacionCredito : FormWithToolbar
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
        private DTO_ccCliente cliente = new DTO_ccCliente(); 
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_ccSolicitudDocu _header = new DTO_ccSolicitudDocu();
        private List<DTO_ccSolicitudComponentes> _componentesVisibles = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudComponentes> _componentesContab = new List<DTO_ccSolicitudComponentes>();
        private List<DTO_ccSolicitudCompraCartera> _compCartera = new List<DTO_ccSolicitudCompraCartera>();
        private DTO_PlanDePagos _liquidador = new DTO_PlanDePagos();
        private DTO_Cuota _cuota = new DTO_Cuota();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private List<DTO_ccSolicitudDetallePago> _detallesPago = new List<DTO_ccSolicitudDetallePago>();

        //Variables formulario (campos)
        private string _clienteID = string.Empty;
        private string _pagaduriaID = string.Empty;
        private string _lineaCreditoID = string.Empty;
        private string _plazo = string.Empty;
        private string _libranzaID = string.Empty;
        private string _libranzaValidID = string.Empty;
        private string _centroPagoID = string.Empty;
        private bool _chkCompraCartera = false;
        private bool _chkPagoVentanilla = false;
        private bool _dtCta1Loaded = false;
        private string centroCostoSol = string.Empty;

        //Variables auxiliares del formulario
        private bool readOnly = false;
        private DateTime periodo;
        private DateTime fechaVto;
        private bool isValid;
        private bool validateData;
        private bool deleteOp;
        private bool isModalFormOpened;
        private bool cargaFechaDef = true;
        private int diaTope;
        private string rechazoObs = string.Empty;

        //Valores temporales
        private int edad;
        private int vlrSolicitado;
        private int vlrGiro;
        private int vlrDescuento;
        private decimal vlrTerceros;
        private decimal porInteres;
        private decimal porSeguro;
        private decimal porComponente1;
        private decimal porComponente2;
        private decimal porComponente3;
        private decimal tasaEfectivaAnual;

        //Variables de mensajes
        private string msgFinDoc;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DigitacionCredito()
        {
            this.Constructor(null);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DigitacionCredito(string mod)
        {
            this.Constructor(null, mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DigitacionCredito(int libranza, string mod)
        {
            this.Constructor(libranza, mod);
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
                this.AddGridCols();
                this.AddGridColsDetail();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    this.EnableHeader(false);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);

                }
                #endregion
                #region Flujo permitido para devolver

                Dictionary<string, string> dictAct = new Dictionary<string, string>();

                //Libre
                dictAct[string.Empty] = string.Empty;

                //Anular
                string anularStr = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado).ToUpper();
                dictAct["AN"] = anularStr;

                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                foreach (DTO_glActividadFlujo act in actPadres)
                    dictAct[act.ID.Value] = act.Descriptivo.Value;

                this.lkp_Flujo.Properties.DataSource = dictAct;
                this.lkp_Flujo.Enabled = false;

                #endregion
                #region Revisa si el formulario esta solo en modo de lectura
                this.readOnly = libranza.HasValue;
                if (this.readOnly)
                {
                    this.txtLibranza.Text = libranza.Value.ToString();
                    this.txtLibranza_Leave(null, null);

                    this.txtLibranza.Enabled = false;
                    this.lkp_Flujo.Enabled = false;

                    //Documents
                    foreach (var col in this.gvDocument.Columns)
                    {
                        ((GridColumn)col).OptionsColumn.AllowEdit = false;
                    }

                    //Details
                    foreach (var col in this.gvDetail.Columns)
                    {
                        ((GridColumn)col).OptionsColumn.AllowEdit = false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "DigitacionCredito"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.DigitacionCredito;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
            _bc.InitMasterUC(this.masterCodeudor1, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.masterCodeudor2, AppMasters.coTercero, false, true, true, false);
            _bc.InitMasterUC(this.masterTipoCredito, AppMasters.ccTipoCredito, true, true, true, false);
            _bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
            _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);
            _bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
            _bc.InitMasterUC(this.masterAnalista, AppMasters.seUsuario, true, true, true, false);
            _bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
            _bc.InitMasterUC(this.masterCooperativa, AppMasters.ccCooperativa, true, true, true, false);
            _bc.InitMasterUC(this.masterAgencia, AppMasters.ccConcesionario, false, true, true, false);

            this.masterPagaduria.EnableControl(false);
            this.masterZona.EnableControl(false);

            //Carga la informacion del combo de incorporacion
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(string.Empty, string.Empty);
            dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, "32555_tbl_Liquidacion"));
            dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, "32555_tbl_Previa"));
            dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, "32555_tbl_Visado"));
            this.lkp_Incorporacion.Properties.DataSource = dic;

            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            this.dtFecha.DateTime = DateTime.Now;
            this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));

            DateTime nextMonth = this.periodo.AddMonths(1);
            if (this.dtFecha.DateTime >= nextMonth)
                this.dtFecha.DateTime = nextMonth.AddDays(-1);

            //Establece la fecha incial de la primera cuota
            this.dtFechaCuota1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            this.EnableHeader(false);
            this.txtLibranza.Enabled = true;

            //Carga los mensajes de la grilla
            this.msgFinDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFinDoc);

            //Solo se habilita cuando se quiere comprar cartera
            this.gcDetail.Enabled = false;
        }

        /// <summary>
        /// Agrega las columnas a la grilla 1
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Doc Solicitud Componentes
                //Campo de codigo
                GridColumn codigo = new GridColumn();
                codigo.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                codigo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComponenteCarteraID");
                codigo.UnboundType = UnboundColumnType.String;
                codigo.VisibleIndex = 0;
                codigo.Width = 50;
                codigo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codigo);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descripcion";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 1;
                descriptivo.Width = 100;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descriptivo);

                //Cuota Valor
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this._unboundPrefix + "CuotaValor";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCuota");
                cuotaValor.UnboundType = UnboundColumnType.Decimal;
                cuotaValor.VisibleIndex = 2;
                cuotaValor.Width = 100;
                cuotaValor.OptionsColumn.AllowEdit = false;
                cuotaValor.ColumnEdit = this.editSpin;

                this.gvDocument.Columns.Add(cuotaValor);

                //Valor Total
                GridColumn valorTotal = new GridColumn();
                valorTotal.FieldName = this._unboundPrefix + "TotalValor";
                valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorTotal");
                valorTotal.UnboundType = UnboundColumnType.Decimal;
                valorTotal.VisibleIndex = 3;
                valorTotal.Width = 150;
                valorTotal.OptionsColumn.AllowEdit = false;
                valorTotal.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(valorTotal);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla 2
        /// </summary>
        private void AddGridColsDetail()
        {
            try
            {
                //FinancieraID
                GridColumn financieraID = new GridColumn();
                financieraID.FieldName = this._unboundPrefix + "FinancieraID";
                financieraID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FinancieraID");
                financieraID.UnboundType = UnboundColumnType.String;
                financieraID.VisibleIndex = 0;
                financieraID.Width = 100;
                financieraID.ColumnEdit = this.editBtnGrid;
                this.gvDetail.Columns.Add(financieraID);

                //Libranza externa
                GridColumn documento = new GridColumn();
                documento.FieldName = this._unboundPrefix + "Documento";
                documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                documento.UnboundType = UnboundColumnType.Integer;
                documento.VisibleIndex = 1;
                documento.Width = 80;
                //documento.ColumnEdit = this.editSpin;
                this.gvDetail.Columns.Add(documento);

                //Valor Cuota
                GridColumn valorCuota = new GridColumn();
                valorCuota.FieldName = this._unboundPrefix + "VlrCuota";
                valorCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCuota");
                valorCuota.UnboundType = UnboundColumnType.Decimal;
                valorCuota.VisibleIndex = 2;
                valorCuota.Width = 120;
                valorCuota.Visible = true;
                valorCuota.ColumnEdit = this.editSpin;
                this.gvDetail.Columns.Add(valorCuota);

                //Valor Saldo
                GridColumn valorSaldo = new GridColumn();
                valorSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                valorSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorSaldo");
                valorSaldo.UnboundType = UnboundColumnType.Decimal;
                valorSaldo.VisibleIndex = 3;
                valorSaldo.Width = 120;
                valorSaldo.Visible = true;
                valorSaldo.ColumnEdit = this.editSpin;
                this.gvDetail.Columns.Add(valorSaldo);

                //AnticipoInd
                GridColumn docAnticipo = new GridColumn();
                docAnticipo.FieldName = this._unboundPrefix + "AnticipoInd";
                docAnticipo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocAnticipo");
                docAnticipo.UnboundType = UnboundColumnType.Boolean;
                docAnticipo.VisibleIndex = 4;
                docAnticipo.Width = 60;
                docAnticipo.Visible = true;
                docAnticipo.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(docAnticipo);

                //Ind Paz y Salvo
                GridColumn indPazySalvo = new GridColumn();
                indPazySalvo.FieldName = this._unboundPrefix + "IndRecibePazySalvo";
                indPazySalvo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RecibePazySalvo");
                indPazySalvo.UnboundType = UnboundColumnType.Boolean;
                indPazySalvo.VisibleIndex = 5;
                indPazySalvo.Width = 60;
                indPazySalvo.OptionsColumn.AllowEdit = false;
                indPazySalvo.Visible = true;

                this.gvDetail.Columns.Add(indPazySalvo);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "AddGridColsDetail"));
            }
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            try
            {
                this.txtLibranza.Enabled = enabled;
                this.dtFecha.Enabled = enabled;
                this.masterCliente.EnableControl(enabled);
                this.masterCooperativa.EnableControl(enabled);
                this.txtPriApellido.Enabled = enabled;
                this.txtSdoApellido.Enabled = enabled;
                this.txtPriNombre.Enabled = enabled;
                this.txtSdoNombre.Enabled = enabled;
                this.masterCodeudor1.EnableControl(enabled);
                this.masterCodeudor2.EnableControl(enabled);
                this.masterTipoCredito.EnableControl(enabled);
                this.masterAsesor.EnableControl(enabled);
                this.masterCentroPago.EnableControl(enabled);
                this.masterPagaduria.EnableControl(enabled);
                this.masterCiudad.EnableControl(enabled);
                this.masterAgencia.EnableControl(enabled);
                //this.masterZona.EnableControl(enabled);
                this.masterAnalista.EnableControl(enabled);
                this.txtVlrCapacidad.Enabled = enabled;
                this.txtVlrCupoDisp.Enabled = enabled;
                this.txtObservacion.Enabled = enabled;
                this.lkp_Flujo.Enabled = enabled;

                this.lkp_Incorporacion.Enabled = enabled;
                this.dtFechaCuota1.Enabled = enabled;
                this.chkPagoVentanilla.Enabled = enabled;
                this.masterLineaCredito.EnableControl(enabled);
                this.chkCompraCartera.Enabled = enabled;
                this.comboPlazo.Enabled = enabled;
                this.txtVlrSolicitado.Enabled = enabled;
                this.btn_Beneficiario.Enabled = enabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            try
            {
                this.validateData = false;

                this.EnableHeader(false);
                this.txtLibranza.Enabled = true;

                //Asigna eventos
                this.chkCompraCartera.CheckedChanged -= new EventHandler(this.chkCompraCartera_CheckedChanged);
                this.gcDetail.Enabled = false;

                //Header
                this._libranzaValidID = String.Empty;
                this.txtLibranza.Text = String.Empty;
                this.txtPriApellido.Text = String.Empty;
                this.txtSdoApellido.Text = String.Empty;
                this.txtPriNombre.Text = String.Empty;
                this.txtSdoNombre.Text = String.Empty;
                this.masterCliente.Value = String.Empty;
                this.masterCooperativa.Value = string.Empty;
                this.masterCodeudor1.Value = String.Empty;
                this.masterCodeudor2.Value = String.Empty;
                this.masterTipoCredito.Value = String.Empty;
                this.masterAsesor.Value = String.Empty;
                this.masterCentroPago.Value = String.Empty;
                this.masterPagaduria.Value = String.Empty;
                this.masterCiudad.Value = String.Empty;
                this.masterAgencia.Value = string.Empty;
                this.masterZona.Value = String.Empty;
                this.txtVlrCupoDisp.EditValue = 0;
                this.txtVlrCapacidad.EditValue = 0;
                this.masterAnalista.Value = String.Empty;
                this.txtObservacion.Text = String.Empty;
                this.lkp_Flujo.EditValue = string.Empty;

                this.txtCodEmpleado.Text = string.Empty;
                this.chkRecuadoMes.Checked = false;
                this.lkp_Incorporacion.ItemIndex = 0;
                this.dtFechaCuota1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);
                this.chkPagoVentanilla.Checked = false;
                this.masterLineaCredito.Value = String.Empty;
                this.chkCompraCartera.Checked = false;
                this.comboPlazo.SelectedIndex = -1;
                this.txtVlrSolicitado.EditValue = 0;
                this.txtVlrAdicional.EditValue = 0;
                this.txtVlrPrestamo.EditValue = 0;
                this.txtVlrCompra.EditValue = 0;
                this.txtIntereses.EditValue = 0;
                this.txtVlrCuota.EditValue = 0;
                this.txtVlrLibranza.EditValue = 0;
                this.txtVlrGiroTerceros.EditValue = 0;
                this.txtVlrCliente.EditValue = 0;
                this.txtVlrGiro.EditValue = 0;
                this.txtVlrDescuento.EditValue = 0;
                
                // Fecha
                DateTime nextMonth = this.periodo.AddMonths(1);
                if (this.dtFecha.DateTime >= nextMonth)
                    this.dtFecha.DateTime = nextMonth.AddDays(-1);


                //Footer
                this._ctrl = new DTO_glDocumentoControl();
                this._header = new DTO_ccSolicitudDocu();
                this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                this._compCartera = new List<DTO_ccSolicitudCompraCartera>();
                this._detallesPago = new List<DTO_ccSolicitudDetallePago>();

                //Variables
                this.rechazoObs = string.Empty;
                this._clienteID = String.Empty;
                this._libranzaID = String.Empty;
                this._lineaCreditoID = String.Empty;
                this._libranzaID = String.Empty;
                this._plazo = String.Empty;
                this._centroPagoID = String.Empty;
                this.vlrGiro = 0;
                this._chkCompraCartera = false;
                this._chkPagoVentanilla = false;
                this._dtCta1Loaded = false;
                this.centroCostoSol = string.Empty;

                //Grillas
                // this.gcDocument.Enabled = true;
                this.gcDocument.DataSource = this._componentesContab;
                this.gcDetail.DataSource = this._compCartera;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "CleanData"));
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
                #region Validaciones básicas
                //Valida que este escrito el numero de Libranza
                if (string.IsNullOrEmpty(this.txtLibranza.Text) || string.IsNullOrWhiteSpace(this._libranzaValidID))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lblLibranza.Text);
                    MessageBox.Show(msg);
                    this.txtLibranza.Focus();
                    return false;
                }

                //Valida que el usuario exista
                if (!this.masterCliente.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCliente.Focus();
                    return false;
                }

                //Valida que este escrito el plazo
                if (string.IsNullOrEmpty(this.comboPlazo.Text))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazo.Text);
                    MessageBox.Show(msg);
                    this.comboPlazo.Focus();
                    return false;
                }

                #endregion
                if (string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
                {
                    #region Otras validaciones

                    //Valida la cooperativa
                    if (!this.masterCooperativa.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCooperativa.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterCooperativa.Focus();
                        return false;
                    }

                    //Valida que este escrito el primer apellido
                    if (string.IsNullOrEmpty(this.txtPriApellido.Text))
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriApellido.Text);
                        MessageBox.Show(msg);
                        this.txtPriApellido.Focus();
                        return false;
                    }

                    //Valida que este escrito el Primer nombre
                    if (string.IsNullOrEmpty(this.txtPriNombre.Text))
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriNombre.Text);
                        MessageBox.Show(msg);
                        this.txtPriNombre.Focus();
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

                    //Valida qu el centro de pago exista 
                    if (!this.masterCentroPago.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterCentroPago.Focus();
                        return false;
                    }

                    //Valida que la pagaduria exista
                    if (!this.masterPagaduria.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPagaduria.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterPagaduria.Focus();
                        return false;
                    }

                    //Valida que la ciudad exista
                    if (!this.masterCiudad.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCiudad.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterCiudad.Focus();
                        return false;
                    }

                    //Valida que la agencia exista
                    if (!this.masterAgencia.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAgencia.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterAgencia.Focus();
                        return false;
                    }

                    //Valida que la zona exista
                    if (!this.masterZona.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterZona.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterZona.Focus();
                        return false;
                    }

                    //Valida que el analista exista
                    if (!this.masterAnalista.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAnalista.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterAnalista.Focus();
                        return false;
                    }

                    //Valida que ell tipo de credito exista
                    if (!this.masterTipoCredito.ValidID)
                    {
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoCredito.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterTipoCredito.Focus();
                        return false;
                    }

                    //Valida que el valor capicidad sea valido
                    if (Convert.ToDecimal(this.txtVlrCapacidad.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrCapacidad.Text);
                        MessageBox.Show(msg);
                        return false;
                    }

                    //Valida que el valor cupo disponible sea valido
                    if (Convert.ToDecimal(this.txtVlrCupoDisp.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrCupoDisp.Text);
                        MessageBox.Show(msg);
                        this.txtVlrCupoDisp.Focus();
                        return false;
                    }

                    //Valida que la linea de credito exista
                    if (!this.masterLineaCredito.ValidID)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterLineaCredito.Focus();
                        return false;
                    }

                    //Valida que este escrito el plazo
                    if (this.lkp_Incorporacion.ItemIndex == 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblIncorpPrevia.Text);
                        MessageBox.Show(msg);
                        this.lkp_Incorporacion.Focus();
                        return false;
                    }

                    //Valida que el valor prestamo sea valido
                    if (Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrSolicitado.Text);
                        MessageBox.Show(msg);
                        return false;
                    }

                    //Valida que el valor prestamo sea valido
                    if (Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrGiro.Text);
                        MessageBox.Show(msg);
                        return false;
                    }

                    //Valida que el valor de descuento sea valido
                    if (Convert.ToDecimal(this.txtVlrDescuento.EditValue, CultureInfo.InvariantCulture) < 0)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrDescuento.Text);
                        MessageBox.Show(msg);
                        return false;
                    }

                    //Valida que la grilla de componentes no este vacia
                    if (this._componentesVisibles.Count <= 0)
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded);
                        MessageBox.Show(msg);
                        return false;
                    }

                    //Valida que la informacion en los beneficiarios este correcta
                    if (!this.chkPagoVentanilla.Checked)
                    {
                        //Valida que el valor prestamo sea valido
                        if (Convert.ToDecimal(this.txtVlrGiroTerceros.EditValue, CultureInfo.InvariantCulture) < 0)
                        {
                            string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrGiroTerceros.Text);
                            MessageBox.Show(msg);
                            return false;
                        }

                        //Valida que el valor prestamo sea valido
                        if (Convert.ToDecimal(this.txtVlrCliente.EditValue, CultureInfo.InvariantCulture) < 0)
                        {
                            string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblVlrCliente.Text);
                            MessageBox.Show(msg);
                            return false;
                        }
                        //Valida que exista el Centro de Costo
                        if (string.IsNullOrEmpty(this.centroCostoSol))
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), "Centro Costo del Área Fisica ");
                            MessageBox.Show(msg);
                            return false;
                        }
                    }

                    #endregion
                    #region Carga los datos del header (ccSolicitudDocu)
                    this._header.Libranza.Value = Convert.ToInt32(this.txtLibranza.Text);
                    this._header.ClienteID.Value = this.masterCliente.Value;
                    this._header.CooperativaID.Value = this.masterCooperativa.Value;
                    this._header.ClienteRadica.Value = this.masterCliente.Value;
                    this._header.ApellidoPri.Value = this.txtPriApellido.Text;
                    this._header.ApellidoSdo.Value = this.txtSdoApellido.Text;
                    this._header.NombrePri.Value = this.txtPriNombre.Text;
                    this._header.NombreSdo.Value = this.txtSdoNombre.Text;
                    this._header.Codeudor1.Value = this.masterCodeudor1.Value;
                    this._header.Codeudor2.Value = this.masterCodeudor2.Value;
                    this._header.TipoCreditoID.Value = this.masterTipoCredito.Value;
                    this._header.AsesorID.Value = this.masterAsesor.Value;
                    this._header.CentroPagoID.Value = this.masterCentroPago.Value;
                    this._header.PagaduriaID.Value = this.masterPagaduria.Value;
                    this._header.Ciudad.Value = this.masterCiudad.Value;
                    this._header.ZonaID.Value = this.masterZona.Value;
                    this._header.ConcesionarioID.Value = this.masterAgencia.Value;
                    this._header.VlrCapacidad.Value = Convert.ToDecimal(this.txtVlrCapacidad.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrCupoDisponible.Value = Convert.ToDecimal(this.txtVlrCupoDisp.EditValue, CultureInfo.InvariantCulture);
                    this._header.Observacion.Value = this.txtObservacion.Text;

                    this._header.IncorporacionTipo.Value = Convert.ToByte(this.lkp_Incorporacion.EditValue);
                    if (this._header.IncorporacionTipo.Value != 1)
                        this._header.IncorporacionPreviaInd.Value = true;
                    else
                        this._header.IncorporacionPreviaInd.Value = false;
                    this._header.IncorporaMesInd.Value = this.chkRecuadoMes.Checked;
                    this._header.FechaCuota1.Value = this.dtFechaCuota1.DateTime;
                    this._header.FechaVto.Value = this.fechaVto;
                    this._header.Plazo.Value = Convert.ToInt16(this._plazo);
                    this._header.PagoVentanillaInd.Value = this.chkPagoVentanilla.Checked;
                    this._header.LineaCreditoID.Value = this.masterLineaCredito.Value;
                    this._header.TipoCredito.Value = this.chkCompraCartera.Checked ? (byte)2 : (byte)1;
                    this._header.CompraCarteraInd.Value = this.chkCompraCartera.Checked;
                    this._header.PorInteres.Value = Convert.ToDecimal(this.txtIntereses.EditValue, CultureInfo.InvariantCulture);
                    this._header.PorSeguro.Value = this.porSeguro;
                    this._header.PorComponente1.Value = this.porComponente1;
                    this._header.PorComponente2.Value = this.porComponente2;
                    this._header.PorComponente3.Value = this.porComponente3;
                    this._header.TasaEfectivaCredito.Value = this.tasaEfectivaAnual;
                    this._header.VlrPrestamo.Value = Convert.ToDecimal(this.txtVlrPrestamo.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrSolicitado.Value = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrLibranza.Value = Convert.ToDecimal(this.txtVlrLibranza.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrCompra.Value = Convert.ToDecimal(this.txtVlrCompra.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrDescuento.Value = this.vlrDescuento;
                    this._header.VlrAdicional.Value = Convert.ToDecimal(this.txtVlrAdicional.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrGiro.Value = Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture);
                    this._header.VlrCuota.Value = Convert.ToDecimal(this.txtVlrCuota.EditValue, CultureInfo.InvariantCulture);

                    this._header.RechazoInd.Value = false;
                    this._header.AnalisisUsuario.Value = this.masterAnalista.Value;
                    //Variable exclusive de la financiera (por eso siempre es true)
                    this._header.LiquidaAll.Value = true;
                    #endregion
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this.rechazoObs))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DevolucionFlujoObsVacio));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "ValidateHeader"));
            }

            return true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void GetValues()
        {
            try
            {
                //Trae la fecha del documento
                //this.dtFecha.DateTime = this._ctrl.FechaDoc.Value.Value;

                //Header
                this.txtLibranza.Text = this._header.Libranza.Value.ToString();
                this.masterCliente.Value = this._header.ClienteRadica.Value;
                this.txtPriApellido.Text = this._header.ApellidoPri.Value;
                this.txtSdoApellido.Text = this._header.ApellidoSdo.Value;
                this.txtPriNombre.Text = this._header.NombrePri.Value;
                this.txtSdoNombre.Text = this._header.NombreSdo.Value;
                this.masterCodeudor1.Value = this._header.Codeudor1.Value;
                this.masterCodeudor2.Value = this._header.Codeudor2.Value;
                this.masterTipoCredito.Value = this._header.TipoCreditoID.Value;
                this.masterCooperativa.Value = this._header.CooperativaID.Value;
                this.masterAsesor.Value = this._header.AsesorID.Value;
                this.masterCentroPago.Value = this._header.CentroPagoID.Value;
                this.masterPagaduria.Value = this._header.PagaduriaID.Value;
                this.masterCiudad.Value = this._header.Ciudad.Value;
                this.masterAgencia.Value = this._header.ConcesionarioID.Value;
                this.masterZona.Value = this._header.ZonaID.Value;
                this.masterAnalista.Value = !string.IsNullOrWhiteSpace(this._header.AnalisisUsuario.Value) ? this._header.AnalisisUsuario.Value : _bc.AdministrationModel.User.ID.Value;
                this.txtVlrCupoDisp.Text = this._header.VlrCupoDisponible.Value.ToString();
                this.txtVlrCapacidad.Text = this._header.VlrCapacidad.Value.ToString();
                this.txtObservacion.Text = this._header.Observacion.Value;

                this.lkp_Incorporacion.ItemIndex = this._header.IncorporacionTipo.Value.Value;
                this.chkRecuadoMes.Checked = this._header.IncorporaMesInd.Value == null ? false : this._header.IncorporaMesInd.Value.Value;//UNDONE: Revisar el undone de arriba
                if (this._header.FechaCuota1.Value != null)
                {
                    this.dtFechaCuota1.DateTime = this._header.FechaCuota1.Value.Value;
                    this._dtCta1Loaded = true;
                    this.fechaVto = this.dtFechaCuota1.DateTime.AddMonths(Convert.ToInt16(this._header.Plazo.Value));
                }
                this.chkPagoVentanilla.Checked = this._header.PagoVentanillaInd.Value.Value;
                this.masterLineaCredito.Value = this._header.LineaCreditoID.Value;
                this.comboPlazo.Text = this._header.Plazo.Value.ToString();

                this.txtVlrAdicional.EditValue = this._header.VlrAdicional.Value;
                this.txtVlrSolicitado.EditValue = this._header.VlrSolicitado.Value;
                this.txtVlrPrestamo.EditValue = this._header.VlrPrestamo.Value;
                this.txtIntereses.EditValue = this._header.PorInteres.Value;
                this.txtVlrCuota.EditValue = this._header.VlrCuota.Value;
                this.txtVlrLibranza.EditValue = this._header.VlrLibranza.Value;
                this.txtVlrCompra.EditValue = this._header.VlrCompra.Value;
                this.txtVlrGiro.EditValue = this._header.VlrGiro.Value;

                if (this._detallesPago.Count > 0)
                {
                    this.vlrTerceros = (from c in this._detallesPago select c.Valor.Value.Value).Sum();
                    this.txtVlrGiroTerceros.EditValue = this.vlrTerceros;
                    this.txtVlrCliente.EditValue = this._header.VlrGiro.Value.Value - vlrTerceros;
                }
                //Footer
                this.gcDocument.DataSource = this._componentesContab;

                //Variables
                this._libranzaID = this.txtLibranza.Text;
                this._clienteID = this.masterCliente.Value;
                this._plazo = this._header.Plazo.Value.ToString();
                this._centroPagoID = this._header.CentroPagoID.Value;

                //Se carga alfinal para autocalcular el valor del giro
                this.chkCompraCartera.Checked = this._header.CompraCarteraInd.Value.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "GetValues"));
            }
        }

        /// <summary>
        /// Funcion que trae el valor de los interes de los componentes
        /// </summary>
        private void GetIntereses(List<DTO_ccSolicitudComponentes> componentes)
        {
            string compCapital = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
            string compInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
            string compSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            string compAportes = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentedeAportes);
            foreach (DTO_ccSolicitudComponentes componente in componentes)
            {
                if (componente.ComponenteCarteraID.Value == compInteres)
                    this.porInteres = componente.Porcentaje.Value.Value;
                else if (componente.ComponenteCarteraID.Value == compSeguro)
                    this.porSeguro = componente.Porcentaje.Value == null ? 0 : componente.Porcentaje.Value.Value;
                else if (componente.ComponenteCarteraID.Value == compAportes)
                    this.porComponente1 = componente.Porcentaje.Value == null ? 0 : componente.Porcentaje.Value.Value;
                else if (componente.ComponenteCarteraID.Value != compCapital)
                    this.porComponente2 = componente.Porcentaje.Value == null ? 0 : componente.Porcentaje.Value.Value;
                else if (this.porComponente2 != 0)
                    this.porComponente3 = componente.Porcentaje.Value == null ? 0 : componente.Porcentaje.Value.Value;
            }
            //Calcula tasa efectiva Anual
            this.tasaEfectivaAnual = Convert.ToDecimal(Math.Pow((1 + (double)this.porInteres/100), 12 - 1));//= 37,8328546 % 
        }

        /// <summary>
        /// Metodo que permite crea una nueva fila en una grilla
        /// </summary>
        private void AddNewRow()
        {
            DTO_ccSolicitudCompraCartera solCompra = new DTO_ccSolicitudCompraCartera();
            try
            {
                isValid = false;

                #region Asigna datos a la fila

                solCompra.FinancieraID.Value = string.Empty;
                solCompra.Documento.Value = Convert.ToInt32(this.txtLibranza.Text);
                solCompra.DocCompra.Value = 0;
                solCompra.VlrCuota.Value = 0;
                solCompra.VlrSaldo.Value = 0;
                solCompra.AnticipoInd.Value = false;
                solCompra.IndRecibePazySalvo.Value = false;
                solCompra.ExternaInd.Value = true;

                #endregion

                this._compCartera.Add(solCompra);
                this.gcDetail.DataSource = this._compCartera;
                this.gcDetail.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "AddNewRow"));
            }
        }

        ///<summary>
        ///Metodo que calcula el valor de la compra de la cartera
        ///</summary>
        private void CalcularGiro()
        {
            try
            {
                decimal vlrTotalCompra = this.chkCompraCartera.Checked ? (from p in this._compCartera select p.VlrSaldo.Value.Value).Sum() : 0;
                this.txtVlrGiro.EditValue = this.vlrGiro - vlrTotalCompra;
                this.txtVlrCompra.EditValue = vlrTotalCompra;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "CalcularGiro"));
            }
        }

        /// <summary>
        /// Funcion que calcula la fecha de la primera cuota del credito
        /// </summary>
        private void CalcularFechaCuota1(DateTime fechaLiquida)
        {
            int ultimoDia = 0;
            int diaLiquida = this.dtFecha.DateTime.Day;
            if (diaLiquida <= this.diaTope && this.chkRecuadoMes.Checked)
            {
                ultimoDia = DateTime.DaysInMonth(fechaLiquida.Year, fechaLiquida.Month);
                ultimoDia = ultimoDia == 31 ? ultimoDia - 1 : ultimoDia;
                this.dtFechaCuota1.Properties.MaxValue = new DateTime(fechaLiquida.Year, fechaLiquida.Month, ultimoDia);
                this.dtFechaCuota1.DateTime = new DateTime(fechaLiquida.Year, fechaLiquida.Month, ultimoDia);
                if (!String.IsNullOrWhiteSpace(this._plazo))
                    this.fechaVto = this.dtFechaCuota1.DateTime.AddMonths(Convert.ToInt16(this._plazo));
            }
            else
            {
                DateTime mesSiguiente = fechaLiquida.AddMonths(1);
                ultimoDia = DateTime.DaysInMonth(mesSiguiente.Year, mesSiguiente.Month);
                ultimoDia = ultimoDia == 31 ? ultimoDia - 1 : ultimoDia;
                this.dtFechaCuota1.Properties.MaxValue = new DateTime(mesSiguiente.Year, mesSiguiente.Month, ultimoDia);
                this.dtFechaCuota1.DateTime = new DateTime(mesSiguiente.Year, mesSiguiente.Month, ultimoDia);
                if (!String.IsNullOrWhiteSpace(this._plazo))
                    this.fechaVto = this.dtFechaCuota1.DateTime.AddMonths(Convert.ToInt16(this._plazo));
            }

            if (diaLiquida > this.diaTope && !this.chkRecuadoMes.Checked)
            {
                DateTime mesSiguiente = fechaLiquida.AddMonths(2);
                ultimoDia = DateTime.DaysInMonth(mesSiguiente.Year, mesSiguiente.Month);
                ultimoDia = ultimoDia == 31 ? ultimoDia - 1 : ultimoDia;
                this.dtFechaCuota1.Properties.MaxValue = new DateTime(mesSiguiente.Year, mesSiguiente.Month, ultimoDia);
                this.dtFechaCuota1.DateTime = new DateTime(mesSiguiente.Year, mesSiguiente.Month, ultimoDia);
                if (!String.IsNullOrWhiteSpace(this._plazo))
                    this.fechaVto = this.dtFechaCuota1.DateTime.AddMonths(Convert.ToInt16(this._plazo));
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
                this.gvDetail.PostEditor();
                this.isValid = true;

                if (fila >= 0)
                {
                    bool rowValid = true;
                    string fieldName;

                    #region FinancieraID

                    rowValid = true;
                    fieldName = "FinancieraID";
                    GridColumn colFinanciera = this.gvDetail.Columns[this._unboundPrefix + fieldName];

                    //Valida la financiera
                    rowValid = _bc.ValidGridCell(this.gvDetail, this._unboundPrefix, fila, fieldName, false, true, false, AppMasters.ccFinanciera);
                    if (rowValid)
                        this.gvDetail.SetColumnError(colFinanciera, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region Documento

                    rowValid = true;
                    fieldName = "Documento";
                    GridColumn colDocumento = this.gvDetail.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvDetail, this._unboundPrefix, fila, fieldName, false, false, true, false);
                    if (rowValid)
                        this.gvDetail.SetColumnError(colDocumento, string.Empty);
                    else
                        this.isValid = false;

                    #endregion
                    #region VlrSaldo

                    fieldName = "VlrSaldo";
                    GridColumn colVlrSaldo = this.gvDetail.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvDetail, this._unboundPrefix, fila, fieldName, false, true, true, false);
                    if (!rowValid)
                        this.isValid = false;

                    if (rowValid)
                        this.gvDetail.SetColumnError(colVlrSaldo, string.Empty);

                    #endregion
                    #region VlrCuota

                    rowValid = true;
                    fieldName = "VlrCuota";
                    GridColumn colVlrCuota = this.gvDetail.Columns[this._unboundPrefix + fieldName];

                    //Valida que tenga valores positivos
                    rowValid = _bc.ValidGridCellValue(this.gvDetail, this._unboundPrefix, fila, fieldName, false, true, true, false);
                    if (rowValid)
                        this.gvDetail.SetColumnError(colVlrCuota, string.Empty);
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
                            this.gvDetail.SetColumnError(colDocumento, this.msgFinDoc);
                            this.isValid = false;
                            rowValid = false;
                        }
                        else
                            this.gvDetail.SetColumnError(colDocumento, string.Empty);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "ValidateRow_CompraCartera"));
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
            this.isModalFormOpened = true;
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
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    if (col.Equals("FinancieraID"))
                    {
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TipoEmpresa",
                            OperadorFiltro = OperadorFiltro.Diferente,
                            ValorFiltro = "0"
                        });  
                    }            

                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false,filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.isModalFormOpened = false;
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
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = this.readOnly ? false : 
                        SecurityManager.HasAccess(this._documentID, FormsActions.Approve) || SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    
                    FormProvider.Master.itemSave.Enabled = this.readOnly ? false : SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

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
                    if (this.masterCliente.ValidID)
                    {
                        this._clienteID = this.masterCliente.Value;
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        DTO_coTercero clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.cliente.TerceroID.Value, true);
                        this.txtPriApellido.Text = clienteTercero.ApellidoPri.ToString();
                        this.txtSdoApellido.Text = clienteTercero.ApellidoSdo.ToString();
                        this.txtPriNombre.Text = clienteTercero.NombrePri.ToString();
                        this.txtSdoNombre.Text = clienteTercero.NombreSdo.ToString();
                        this.masterAsesor.Value = this.cliente.AsesorID.Value;
                        //this.masterCentroPago.Value = this.cliente.CentroPago1.Value;
                        this.masterCiudad.Value = this.cliente.LaboralCiudad.Value;
                        int vltOtrosIng = this.cliente.VlrOtrosIng.Value == null ? 0 : Convert.ToInt32(this.cliente.VlrOtrosIng.Value.Value);
                        this.txtVlrCupoDisp.EditValue = this.cliente.VlrDevengado.Value + vltOtrosIng - this.cliente.VlrDeducido.Value;
                        this.txtCodEmpleado.Text = cliente.EmpleadoCodigo.Value;
                    }
                    else
                    {
                        this.txtPriApellido.Text = String.Empty;
                        this.txtSdoApellido.Text = String.Empty;
                        this.txtPriNombre.Text = String.Empty;
                        this.txtSdoNombre.Text = String.Empty;
                        this.masterAsesor.Value = String.Empty;
                        this.masterCentroPago.Value = String.Empty;
                        this.masterPagaduria.Value = String.Empty;
                        this.masterCiudad.Value = String.Empty;
                        this.txtVlrCupoDisp.Text = "0";
                        this.txtVlrCapacidad.Text = "0";
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Eventro que carga la pagaduria cuando se seleciona un centro pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            if (this._centroPagoID != this.masterCentroPago.Value)
            {
                if (this.masterCentroPago.ValidID)
                {
                    this._centroPagoID = this.masterCentroPago.Value;
                    DTO_ccCentroPagoPAG centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, masterCentroPago.Value, true);
                    this.masterPagaduria.Value = centroPago.PagaduriaID.Value;
                    this.masterPagaduria_Leave(sender, e);

                }
                else
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                    MessageBox.Show(msg);
                }
            }
        }

        /// <summary>
        /// Evento que se usa para establecer la fecha de la primera cuota con base la informacion de la pagaduria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterPagaduria_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._pagaduriaID != this.masterPagaduria.Value)
                {
                    if (this.masterPagaduria.ValidID)
                    {
                        DTO_ccPagaduria pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.masterPagaduria.Value, true);
                        this.chkRecuadoMes.Checked = pagaduria.RecaudoMes.Value.Value;
                        this.diaTope = pagaduria.DiaTope.Value.Value;
                        if (!this._dtCta1Loaded)
                            this.CalcularFechaCuota1(this.dtFecha.DateTime);
                    }
                    else
                    {
                        this.masterPagaduria.Value = String.Empty;
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPagaduria.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que permite crear, habilitar o deshabilitar las propiedades del documento con base a la Libranza  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            string tmp = this.txtLibranza.Text;
            try
            {
                if (this._libranzaID != this.txtLibranza.Text.Trim() && !String.IsNullOrWhiteSpace(this.txtLibranza.Text))
                {
                    this._libranzaID = this.txtLibranza.Text.Trim();
                    int libranzaTmp = Convert.ToInt32(this.txtLibranza.Text);

                    string actFlujo = this.readOnly ? string.Empty : this._actFlujo.ID.Value;
                    DTO_DigitacionCredito digCredito = _bc.AdministrationModel.DigitacionCredito_GetByLibranza(libranzaTmp, actFlujo);
                    if (digCredito.DocCtrl != null)
                    {
                        if (!this.readOnly)
                            this.EnableHeader(true);

                        this._libranzaValidID = this._libranzaID.ToString();
                        this._ctrl = digCredito.DocCtrl;
                        this.centroCostoSol = this._ctrl.CentroCostoID.Value;
                        this._header = digCredito.Header;
                        this._detallesPago = digCredito.DetaPagos;

                        //Carga la compra de cartera y  asigna el evento
                        this.chkCompraCartera.CheckedChanged += new EventHandler(this.chkCompraCartera_CheckedChanged);
                        this._compCartera = digCredito.CompraCartera;
                        this.GetValues();

                        // Asigna los valores del cliente
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.txtCodEmpleado.Text = cliente.EmpleadoCodigo.Value;

                        int vltOtrosIng = this.cliente.VlrOtrosIng.Value == null ? 0 : Convert.ToInt32(this.cliente.VlrOtrosIng.Value.Value);
                        this.txtVlrCupoDisp.EditValue = this.cliente.VlrDevengado.Value + vltOtrosIng - this.cliente.VlrDeducido.Value;

                        //Asigna la zona
                        this.masterAgencia.Value = digCredito.Header.ConcesionarioID.Value;
                        DTO_ccConcesionario agencia = (DTO_ccConcesionario)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, false, digCredito.Header.ConcesionarioID.Value, true);
                        this.masterZona.Value = agencia != null? agencia.ZonaID.Value : string.Empty;

                        //Asigna el centro de costo
                        DTO_glZona zona = (DTO_glZona)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glZona, false, this.masterZona.Value, true);
                        DTO_glAreaFisica areaFis = (DTO_glAreaFisica)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false,(zona != null? zona.AreaFisica.Value :string.Empty), true);
                        if (areaFis != null)
                            this.centroCostoSol = areaFis.CentroCostoID.Value;                     

                        //Llama al evento de la linea credito
                        this.masterPagaduria_Leave(sender, e);
                        this.masterLineaCredito_Leave(sender, e);
                        this.validateData = true;
                        this.isValid = true;
                        this._dtCta1Loaded = false;

                        if (!this.masterZona.ValidID)
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ZonaEmpty));
                    }
                    else
                    {
                        this._libranzaValidID = string.Empty;
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                this._libranzaValidID = string.Empty;
                this.CleanData();
                this.txtLibranza.Text = tmp;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que trae los componentes de cartera con base a la linea de credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterLineaCredito_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._lineaCreditoID != this.masterLineaCredito.Value)
                {
                    this._lineaCreditoID = this.masterLineaCredito.Value;
                    this._componentesVisibles = new List<DTO_ccSolicitudComponentes>();
                    this._componentesContab = new List<DTO_ccSolicitudComponentes>();
                    bool hasLiquidacion = true;

                    if (this.masterLineaCredito.ValidID)
                    {
                         this._plazo = this.comboPlazo.Text;
                         decimal vlrSolTemp = Convert.ToDecimal(this.txtVlrSolicitado.EditValue, CultureInfo.InvariantCulture);
                         this.vlrSolicitado = Convert.ToInt32(vlrSolTemp);
                         ﻿decimal vlrGiroTemp = Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture);
                         this.vlrGiro = Convert.ToInt32(vlrGiroTemp);
                         int plazo = Convert.ToInt32(this.comboPlazo.Text);
                         //Calculo la edad
                         TimeSpan difFecha = this.dtFecha.DateTime.Subtract(this.cliente.FechaNacimiento.Value.Value);
                         this.edad = (int)Math.Floor((double)difFecha.Days / 365);
                         object res = _bc.AdministrationModel.GenerarLiquidacionCartera(this.masterLineaCredito.Value, this.masterPagaduria.Value, vlrSolicitado,
                             this.vlrGiro, plazo, this.edad, this.dtFecha.DateTime, null, this.dtFechaCuota1.DateTime);

                         if (res.GetType() == typeof(DTO_TxResult))
                         {
                             hasLiquidacion = false;
                             MessageForm msg = new MessageForm((DTO_TxResult)res);
                             msg.ShowDialog();
                         }
                         else
                         {
                             this._liquidador = (DTO_PlanDePagos)res;
                             if (this._liquidador.ComponentesAll.Count == 0)
                             {
                                 string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                                 MessageBox.Show(msg);
                                 return;
                             }

                             this._componentesVisibles = this._liquidador.ComponentesUsuario;
                             this._componentesContab = this._liquidador.ComponentesAll;

                             if (this._liquidador.Cuotas.Count != 0)
                                 this.txtVlrCuota.Text = this._liquidador.Cuotas[1].ValorCuota.ToString();

                             this.GetIntereses(this._componentesVisibles);
                             #region Asigna los valores calculados
                             this.txtVlrAdicional.EditValue = this._liquidador.VlrAdicional;
                             this.txtVlrCompra.EditValue = this._liquidador.VlrCompra;
                             this.txtVlrGiro.EditValue = this._liquidador.VlrGiro;
                             this.txtVlrLibranza.EditValue = this._liquidador.VlrLibranza;
                             this.txtVlrCuota.EditValue = this._liquidador.VlrCuota;
                             this.txtVlrPrestamo.EditValue = this._liquidador.VlrPrestamo;
                             this.txtIntereses.EditValue = this.porInteres;
                             this.vlrDescuento = this._liquidador.VlrDescuento;
                             this.txtVlrDescuento.EditValue = this.vlrDescuento;
                             this.vlrGiro = this._liquidador.VlrGiro;
                             this.CalcularGiro();
                             #endregion
                             #region Valida la línea y el valor de giro

                             DTO_ccLineaCredito linea = (DTO_ccLineaCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccLineaCredito, false, this._lineaCreditoID, true);
                             if (linea.IndSinDesembolso.Value.HasValue && linea.IndSinDesembolso.Value.Value && Convert.ToDecimal(this.txtVlrGiro.EditValue) > 0)
                             {

                             }

                             #endregion

                             this.lkp_Flujo.EditValue = string.Empty;
                         }
                    }

                    if (hasLiquidacion && this._componentesVisibles.Count == 0)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                        MessageBox.Show(msg);
                    }

                    this.gcDocument.DataSource = this._componentesContab;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterZona_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterZona.ValidID)
                {
                        DTO_glZona zona = (DTO_glZona)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glZona, false,this.masterZona.Value, true);
                        DTO_glAreaFisica areaFis = (DTO_glAreaFisica)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false,zona.AreaFisica.Value, true);
                        if (areaFis == null || string.IsNullOrEmpty(areaFis.CentroCostoID.Value))
                        {
                            this.centroCostoSol = string.Empty;
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), "Centro Costo del Área Fisica ");
                            MessageBox.Show(msg);
                        }
                        else
                            this.centroCostoSol = areaFis.CentroCostoID.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "masterZona_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterAgencia_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterAgencia.ValidID)
                {
                    DTO_ccConcesionario agen = (DTO_ccConcesionario)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccConcesionario, false, this.masterAgencia.Value, true);
                    this.masterZona.Value = agen.ZonaID.Value;
                    if (this.masterZona.ValidID)
                    {
                        DTO_glZona zona = (DTO_glZona)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glZona, false, this.masterZona.Value, true);
                        DTO_glAreaFisica areaFis = (DTO_glAreaFisica)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false, zona.AreaFisica.Value, true);
                        if (areaFis == null || string.IsNullOrEmpty(areaFis.CentroCostoID.Value))
                        {
                            this.centroCostoSol = string.Empty;
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), "Centro Costo del Área Fisica ");
                            MessageBox.Show(msg);
                        }
                        else
                            this.centroCostoSol = areaFis.CentroCostoID.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "masterAgencia_Leave"));
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
                if (validateData && this.vlrSolicitado != vlrSolicitadoTemp)
                {
                    this._lineaCreditoID = string.Empty;
                    this._plazo = string.Empty;
                    this.masterLineaCredito_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "txtVlrSolicitado_Leave"));
            }
        }

        /// <summary>
        /// Evento que genera nuevos calculos de liquidacion con base al valor del plazo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboPlazo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (validateData)
            {
                this._lineaCreditoID = string.Empty;
                this._plazo = string.Empty;
                this.masterLineaCredito_Leave(sender, e);
            }
        }

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia la fecha de la liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            if (!this.cargaFechaDef)
                this.CalcularFechaCuota1(this.dtFecha.DateTime);
            this.cargaFechaDef = false;
        }

        /// <summary>
        /// Carga la info de compra de cartera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCompraCartera_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._chkCompraCartera != this.chkCompraCartera.Checked)
                {
                    this._chkCompraCartera = this.chkCompraCartera.Checked;
                    if (this.chkCompraCartera.Checked)
                    {
                        if (this._compCartera.Count > 0)
                        {
                            this.validateData = true;
                            this.gcDetail.Enabled = true;
                            this.gcDetail.DataSource = this._compCartera;
                            this.gvDetail.MoveFirst();
                            if (this._compCartera[0].ExternaInd.Value.Value && !this._compCartera[0].IndRecibePazySalvo.Value.Value)
                            {
                                foreach (GridColumn c in this.gvDetail.Columns)
                                    if (c.FieldName != this._unboundPrefix + "IndRecibePazySalvo")
                                        c.OptionsColumn.AllowEdit = false;
                            }
                            else
                            {
                                foreach (GridColumn c in this.gvDetail.Columns)
                                    c.OptionsColumn.AllowEdit = false;
                            }
                        }
                        else
                        {
                            this.validateData = false;
                            this.gcDetail.Enabled = true;
                            this.gcDetail.DataSource = null;
                        }
                    }
                    else
                    {
                        this.validateData = false;
                        this.gcDetail.Enabled = false;
                        this.gcDetail.DataSource = null;
                    }

                    this.validateData = true;
                    this.CalcularGiro();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm.cs", "chkCompraCartera_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambio el estado de la casilla
        /// </summary>
        private void chkPagoVentanilla_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._chkPagoVentanilla != this.chkPagoVentanilla.Checked)
                {
                    this._chkPagoVentanilla = this.chkPagoVentanilla.Checked;
                    if (this.chkPagoVentanilla.Checked)
                    {
                        this.btn_Beneficiario.Enabled = false;
                        this.txtVlrGiroTerceros.EditValue = 0;
                        this.txtVlrCliente.EditValue = 0;
                        this.vlrTerceros = 0;
                    }
                    else
                    {
                        this.btn_Beneficiario.Enabled = true;
                        this.vlrTerceros = (from c in this._detallesPago select c.Valor.Value.Value).Sum();
                        this.txtVlrGiroTerceros.EditValue = this.vlrTerceros;
                        this.txtVlrCliente.EditValue = this.vlrGiro - this.vlrTerceros;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "chkPagoVentanilla_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que se llama al momneto de cambiar el tipo de incorporacion del credito
        /// </summary>
        private void lkp_Incorporacion_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Boton que carga la pantalla modal de los beneficiarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Beneficiario_Click(object sender, EventArgs e)
        {
            try
            {
                ModalBeneficiariosCartera modalBeneficiario = new ModalBeneficiariosCartera(this._detallesPago, this.vlrGiro);
                modalBeneficiario.ShowDialog();
                this.vlrTerceros = (from c in this._detallesPago select c.Valor.Value.Value).Sum();
                this.txtVlrGiroTerceros.EditValue = this.vlrTerceros;
                this.txtVlrCliente.EditValue = this.vlrGiro - this.vlrTerceros;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "btn_Beneficiario_Click"));
            }
        }

        /// <summary>
        /// Boton para poner una observación de rechazo del documento
        /// </summary>
        private void btnFlujoRechazo_Click(object sender, EventArgs e)
        {
            try
            {
                string title = _bc.GetResource(LanguageTypes.Forms, "32017_lblTitle");
                string label = _bc.GetResource(LanguageTypes.Forms, "1015_lblObservacion");
                string currentObs = this.rechazoObs;
                if (Prompt.InputBox(title, label, ref currentObs,false) == DialogResult.OK)
                    this.rechazoObs = currentObs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Referenciacion.cs", "btnFlujoRechazo_Click"));
            }
        }

        /// <summary>
        /// Boton para habilitar o deshabilitar el boton de rechazo de un documento
        /// </summary>
        private void lkp_Flujo_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
            {
                this.btnFlujoRechazo.Enabled = false;
                this.rechazoObs = string.Empty;
            }
            else
            {
                this.btnFlujoRechazo.Enabled = true;
            }
        }

        /// <summary>
        /// Deshabilita el scroll del spin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrSolicitado_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        #endregion Eventos Formulario

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
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDetail_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                int fila = this.gvDetail.FocusedRowHandle;
                if (validateData)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.ValidateRow_CompraCartera(fila);
                        if (this.isValid)
                        {
                            this.AddNewRow();
                            this.gvDetail.FocusedRowHandle = this._compCartera.Count - 1;
                        }
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        if (fila >= 0 && this._compCartera[fila].ExternaInd.Value.Value && !this._compCartera[fila].AnticipoInd.Value.Value && this._compCartera[fila].TipoEmpresa.Value != 0)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.isValid = true;
                                this.deleteOp = true;
                                this._compCartera.RemoveAt(fila);
                                this.gcDetail.RefreshDataSource();
                                if (this._compCartera.Count > 0)
                                    this.gvDetail.FocusedRowHandle = fila - 1;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
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
                    this._compCartera[e.RowHandle].TipoEmpresa.Value = financiera != null? financiera.TipoEmpresa.Value : null;
                    if (financiera != null && financiera.TipoEmpresa.Value == 0)
                        this._compCartera[e.RowHandle].FinancieraID.Value = string.Empty;
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
                this.gcDetail.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "gvDetail_CellValueChanged"));
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
                if (this._compCartera[fila].ExternaInd.Value.Value && !this._compCartera[fila].AnticipoInd.Value.Value)
                {
                    foreach (GridColumn c in this.gvDetail.Columns)
                        if (c.FieldName != this._unboundPrefix + "IndRecibePazySalvo" && c.FieldName != this._unboundPrefix + "AnticipoInd")
                            c.OptionsColumn.AllowEdit = true;
                }
                else
                {
                    foreach (GridColumn c in this.gvDetail.Columns)
                        c.OptionsColumn.AllowEdit = false;
                }
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDetail.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
                int fila = this.gvDetail.FocusedRowHandle;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "editBtnGrid_ButtonClick"));
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
                this.CleanData();
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();
                this.gvDetail.PostEditor();

                if (this.chkCompraCartera.Checked && this._compCartera.Count > 0)
                    this.ValidateRow_CompraCartera(this.gvDetail.FocusedRowHandle);

                if (this.ValidateHeader() && this.isValid)
                {
                    DTO_TxResult result = new DTO_TxResult();
                    if (!string.IsNullOrWhiteSpace(this.lkp_Flujo.EditValue.ToString()))
                    {
                        string titleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgWarning = this.lkp_Flujo.EditValue == "AN" ? _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Anular_Doc) :
                            _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActualizarFlujo);

                        if (MessageBox.Show(msgWarning, titleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                        else
                        {
                            DTO_SolicitudAprobacionCartera sol = new DTO_SolicitudAprobacionCartera();
                            sol.Aprobado.Value = false;
                            sol.Rechazado.Value = true;
                            sol.NumeroDoc.Value = this._ctrl.NumeroDoc.Value;
                            sol.ActividadFlujoReversion.Value = this.lkp_Flujo.EditValue.ToString() == "AN" ? string.Empty : this.lkp_Flujo.EditValue.ToString();
                            sol.Observacion.Value = this.rechazoObs;

                            List<DTO_SolicitudAprobacionCartera> list = new List<DTO_SolicitudAprobacionCartera>();
                            list.Add(sol);

                            List<DTO_SerializedObject> results = _bc.AdministrationModel.SolicitudLibranza_AprobarRechazar(this._documentID, this._actFlujo.ID.Value, list,
                                new List<DTO_ccSolicitudAnexo>(), new List<DTO_glDocumentoChequeoLista>());

                            object r = results.First();
                            if (r.GetType() == typeof(DTO_TxResult))
                                result = (DTO_TxResult)r;
                            else
                            {
                                result.Result = ResultValue.OK;
                                result.ResultMessage = string.Empty;

                                this.CleanData();
                                this.txtLibranza.Focus();
                            }

                            MessageForm frm = new MessageForm(result);
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        //Asigna a this._ctrl la fecha selecionada en el documento
                        this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                        this._ctrl.CentroCostoID.Value = this.centroCostoSol;
                        DTO_DigitacionCredito digCredito = new DTO_DigitacionCredito();
                        List<DTO_ccSolicitudCompraCartera> compraTemp = new List<DTO_ccSolicitudCompraCartera>();
                        List<DTO_ccSolicitudDetallePago> detaPagoTemp = new List<DTO_ccSolicitudDetallePago>();
                        if (this.chkCompraCartera.Checked)
                            compraTemp = this._compCartera;

                        if (!this.chkPagoVentanilla.Checked)
                            detaPagoTemp = this._detallesPago;

                        digCredito.AddData(this._ctrl, this._header, null, this._liquidador, this._componentesContab, compraTemp, detaPagoTemp);

                        result = _bc.AdministrationModel.DigitacionCredito_Add(this._documentID, this._actFlujo.ID.Value, digCredito, new List<DTO_Cuota>());
                        if (result.Result == ResultValue.OK)
                        {
                            #region obtiene el nombre

                            string nombre = this._header.NombrePri.Value;
                            if (!string.IsNullOrWhiteSpace(this._header.NombreSdo.Value))
                                nombre += " " + this._header.NombreSdo.Value;
                            if (!string.IsNullOrWhiteSpace(this._header.ApellidoPri.Value))
                                nombre += " " + this._header.ApellidoPri.Value;
                            if (!string.IsNullOrWhiteSpace(this._header.ApellidoSdo.Value))
                                nombre += " " + this._header.ApellidoSdo.Value;

                            #endregion
                            #region Variables para el mail

                            DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                            string body = string.Empty;
                            string subject = string.Empty;
                            string email = user.CorreoElectronico.Value;

                            string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                            string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                            string formName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                            #endregion
                            #region Envia el correo
                            subject = string.Format(subjectApr, formName);
                            body = string.Format(bodyApr, formName, this._libranzaID, nombre, digCredito.Header.Observacion.Value);
                            _bc.SendMail(this._documentID, subject, body, email);
                            #endregion
                            this.CleanData();
                            this.txtLibranza.Focus();

                            MessageForm frm = new MessageForm(result);
                            frm.ShowDialog();
                        }
                        else
                        {
                            MessageForm msg = new MessageForm(result);
                            msg.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas
     
    }
}