using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using SentenceTransformer;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RecaudosManuales : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID = string.Empty;
        private DateTime fechaRecaudo = DateTime.Now.Date;
        private DTO_ccCliente cliente;
        private DTO_InfoCredito infoCartera = new DTO_InfoCredito();
        private List<DTO_ccCreditoDocu> _creditos = null;
        private DTO_ccCreditoDocu _credito = null;
        private DTO_ccCreditoPlanPagos cuota = new DTO_ccCreditoPlanPagos();
        private List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();
        private List<DTO_ccFormasPago> formasPago = new List<DTO_ccFormasPago>();

        private bool validPlanPagos = true;
        private bool validFormasPago = true;
        private bool validate = true;
        private DateTime fechaPerido;
        private decimal vlrExtra;
        private decimal vlrUsura;
        private decimal porcHonorarios;
        private bool _allowEditValuesInd = true;
        private string _userAutoriza = string.Empty;

        //Mensajes de error
        private string msgPagoInd;
        private string msgExtraCero;
        private string msgExtraInvalid;
        private string msgExtraEmpty;
        private string msgValorNeg;

        //Info de componentes
        private string componenteInteres;
        private string componenteUsura;
        private string componentePJ;

        List<DTO_glConsultaFiltro> filtrosExtras = new List<DTO_glConsultaFiltro>();

        #endregion

        public RecaudosManuales()
            : base()
        {
            //InitializeComponent();
        }

        public RecaudosManuales(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.RecaudosManuales;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            //Valida si permite editar los valores o pide autorizacion
            this._allowEditValuesInd = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CambioVlrConPassword).Equals("1") ? false : true;
            this._userAutoriza = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_UsuarioAutorizaCambiosVlr);
            if (string.IsNullOrEmpty(this._userAutoriza))
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No existe el usuario de autorización"));

            if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                this._allowEditValuesInd = true;

            this.AddPagosCols();
            this.AddDocumentCols();
            this.AddDetailCols();

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[2].Height = 200;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Carga la Informacion del Hedear
            _bc.InitMasterUC(this.masterCaja, AppMasters.tsCaja, true, true, true, false);
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            _bc.InitMasterUC(this.masterBanco, AppMasters.tsBancosCuenta, true, true, true, false);
            this.fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
            this.dtPeriod.DateTime = this.fechaPerido;

            bool cierreValido = true;
            int diaCierre = 1;
            string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
            if (indCierreDiaStr == "1")
            {
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                    diaCierreStr = "1";

                diaCierre = Convert.ToInt16(diaCierreStr);
                if (diaCierre > DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month))
                {
                    cierreValido = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                    this.masterCliente.EnableControl(false);
                    this.dtFechaAplica.Enabled = false;
                    this.dtFechaConsigna.Enabled = false;

                    FormProvider.Master.itemNew.Enabled = false;
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;

                    base.dtFecha.Enabled = false;
                }
            }

            if (cierreValido)
            {
                base.dtFecha.Enabled = true;

                //Carga la info de los mensajes
                this.msgPagoInd = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_AbonoNoValue);
                this.msgExtraCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraCero);
                this.msgExtraInvalid = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompVlrExtra);
                this.msgExtraEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraVacios);
                this.msgValorNeg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);

                //Componentes
                this.componenteInteres = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                this.componenteUsura = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);
                this.componentePJ = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);

                //Filtros de componentes estras
                this.filtrosExtras.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoComponente",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "5",
                    OperadorSentencia = OperadorSentencia.Or
                });
                this.filtrosExtras.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoComponente",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "6"
                });

                SectorCartera sector = SectorCartera.Solidario;
                string sectorStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                if (!string.IsNullOrWhiteSpace(sectorStr))
                    sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);

                if (sector == SectorCartera.Financiero)
                {
                    this.lblFechaAplica.Visible = false;
                    this.dtFechaAplica.Visible = false;
                }
            }
        }

        ///// <summary>
        ///// Agrega las columnas a la grilla superior
        ///// </summary>
        protected virtual void AddDocumentCols()
        {
            try
            {
                //Indicar Pago
                GridColumn indPago = new GridColumn();
                indPago.FieldName = this.unboundPrefix + "PagoInd";
                indPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoInd");
                indPago.UnboundType = UnboundColumnType.Boolean;
                indPago.VisibleIndex = 0;
                indPago.Width = 80;
                indPago.Visible = true;
                indPago.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(indPago);

                //Num Cuota
                GridColumn numCuota = new GridColumn();
                numCuota.FieldName = this.unboundPrefix + "CuotaID";
                numCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumCuota");
                numCuota.UnboundType = UnboundColumnType.Integer;
                numCuota.VisibleIndex = 1;
                numCuota.Width = 100;
                numCuota.Visible = true;
                numCuota.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numCuota);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaCuota";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 2;
                fecha.Width = 150;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                fecha.ColumnEdit = this.editDate;
                this.gvDocument.Columns.Add(fecha);

                //Saldo
                GridColumn saldo = new GridColumn();
                saldo.FieldName = this.unboundPrefix + "VlrSaldoComponentes";
                saldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Saldo");
                saldo.UnboundType = UnboundColumnType.Integer;
                saldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                saldo.AppearanceCell.Options.UseTextOptions = true;
                saldo.VisibleIndex = 3;
                saldo.Width = 200;
                saldo.Visible = true;
                saldo.OptionsColumn.AllowEdit = false;
                saldo.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(saldo);

                //Valor Mora Liquida
                GridColumn vlrMoraLiq = new GridColumn();
                vlrMoraLiq.FieldName = this.unboundPrefix + "VlrMora";
                vlrMoraLiq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrMoraLiquida");
                vlrMoraLiq.UnboundType = UnboundColumnType.Integer;
                vlrMoraLiq.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrMoraLiq.AppearanceCell.Options.UseTextOptions = true;
                vlrMoraLiq.VisibleIndex = 4;
                vlrMoraLiq.Width = 110;
                vlrMoraLiq.Visible = true;
                vlrMoraLiq.OptionsColumn.AllowEdit = false;
                vlrMoraLiq.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(vlrMoraLiq);

                //Valor PJ
                GridColumn vlrJP = new GridColumn();
                vlrJP.FieldName = this.unboundPrefix + "VlrPrejuridico";
                vlrJP.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrejuridico");
                vlrJP.UnboundType = UnboundColumnType.Integer;
                vlrJP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrJP.AppearanceCell.Options.UseTextOptions = true;
                vlrJP.VisibleIndex = 5;
                vlrJP.Width = 110;
                vlrJP.Visible = true;
                vlrJP.OptionsColumn.AllowEdit = false;
                vlrJP.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(vlrJP);

                //Valor Usura
                GridColumn vlrUsura = new GridColumn();
                vlrUsura.FieldName = this.unboundPrefix + "VlrAjusteUsura";
                vlrUsura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrAjusteUsura");
                vlrUsura.UnboundType = UnboundColumnType.Integer;
                vlrUsura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrUsura.AppearanceCell.Options.UseTextOptions = true;
                vlrUsura.VisibleIndex = 6;
                vlrUsura.Width = 110;
                vlrUsura.Visible = true;
                vlrUsura.OptionsColumn.AllowEdit = false;
                vlrUsura.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(vlrUsura);

                //Valor Pago
                GridColumn vlrPago = new GridColumn();
                vlrPago.FieldName = this.unboundPrefix + "VlrPagadoCuota";
                vlrPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrPago");
                vlrPago.UnboundType = UnboundColumnType.Integer;
                vlrPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPago.AppearanceCell.Options.UseTextOptions = true;
                vlrPago.VisibleIndex = 7;
                vlrPago.Width = 160;
                vlrPago.Visible = true;
                vlrPago.OptionsColumn.AllowEdit = this._allowEditValuesInd;
                vlrPago.ColumnEdit = this.editSpin0;
                this.gvDocument.Columns.Add(vlrPago);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            try
            {
                base.AfterInitialize();
                if (this.fechaPerido.Month == DateTime.Now.Date.Month)
                {
                    this.dtFecha.DateTime = DateTime.Now.Date;
                    this.dtFechaAplica.DateTime = DateTime.Now.Date;
                    this.dtFechaConsigna.DateTime = DateTime.Now.Date;
                }
                else
                {
                    this.dtFecha.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
                    this.dtFechaAplica.DateTime = this.dtFecha.DateTime;
                    this.dtFechaConsigna.DateTime = this.dtFecha.DateTime;
                }

                //Pone la fecha de consignacion con base a la del periodo
                this.dtFechaConsigna.Properties.MaxValue = this.dtFecha.DateTime;
                if (this.dtFechaAplica.DateTime.Day == 31)
                    this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);
               
                this.dtFechaAplica.DateTimeChanged += new System.EventHandler(this.dtFechaAplica_DateTimeChanged);
                this.dtFechaConsigna.DateTimeChanged += new System.EventHandler(this.dtFechaConsigna_DateTimeChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AfterInitialize"));
            }

        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla de detalles
        /// </summary>
        private void AddDetailCols()
        {
            try
            {
                //Codigo Componente
                GridColumn comptCarteraID = new GridColumn();
                comptCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                comptCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                comptCarteraID.UnboundType = UnboundColumnType.String;
                comptCarteraID.VisibleIndex = 0;
                comptCarteraID.Width = 30;
                comptCarteraID.Visible = true;
                comptCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                comptCarteraID.OptionsColumn.AllowEdit = false;
                comptCarteraID.ColumnEdit = this.editBtnGrid;
                this.gvDetails.Columns.Add(comptCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 30;
                descripcion.Visible = true;
                descripcion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(descripcion);

                //SaldoTotal
                GridColumn TotalValor = new GridColumn();
                TotalValor.FieldName = this.unboundPrefix + "TotalSaldo";
                TotalValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TotalValor");
                TotalValor.UnboundType = UnboundColumnType.Integer;
                TotalValor.VisibleIndex = 2;
                TotalValor.Width = 40;
                TotalValor.Visible = true;
                TotalValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TotalValor.AppearanceCell.Options.UseTextOptions = true;
                TotalValor.OptionsColumn.AllowEdit = false;
                TotalValor.ColumnEdit = this.editSpin0;
                this.gvDetails.Columns.Add(TotalValor);

                //Valor Cuota
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this.unboundPrefix + "CuotaSaldo";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaValor");
                cuotaValor.UnboundType = UnboundColumnType.Integer;
                cuotaValor.VisibleIndex = 3;
                cuotaValor.Width = 40;
                cuotaValor.Visible = true;
                cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cuotaValor.AppearanceCell.Options.UseTextOptions = true;
                cuotaValor.OptionsColumn.AllowEdit = false;
                cuotaValor.ColumnEdit = this.editSpin0;
                this.gvDetails.Columns.Add(cuotaValor);

                //Pago Cuota
                GridColumn pagoCuota = new GridColumn();
                pagoCuota.FieldName = this.unboundPrefix + "AbonoValor";
                pagoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoCuota");
                pagoCuota.UnboundType = UnboundColumnType.Integer;
                pagoCuota.VisibleIndex = 4;
                pagoCuota.Width = 40;
                pagoCuota.Visible = true;
                pagoCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                pagoCuota.AppearanceCell.Options.UseTextOptions = true;
                pagoCuota.OptionsColumn.AllowEdit = this._allowEditValuesInd;
                pagoCuota.ColumnEdit = this.editSpin0;
                this.gvDetails.Columns.Add(pagoCuota);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de forma de pagos
        /// </summary>
        private void AddPagosCols()
        {
            try
            {
                //FormaPagoID
                GridColumn codigoPago = new GridColumn();
                codigoPago.FieldName = this.unboundPrefix + "FormaPagoID";
                codigoPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Codigo");
                codigoPago.UnboundType = UnboundColumnType.String;
                codigoPago.VisibleIndex = 0;
                codigoPago.Width = 30;
                codigoPago.Visible = true;
                codigoPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                codigoPago.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(codigoPago);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 30;
                descripcion.Visible = true;
                descripcion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(descripcion);

                //Documento Recibo Caja
                GridColumn Documento = new GridColumn();
                Documento.FieldName = this.unboundPrefix + "Documento";
                Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
                Documento.UnboundType = UnboundColumnType.String;
                Documento.VisibleIndex = 2;
                Documento.Width = 40;
                Documento.Visible = true;
                Documento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                Documento.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(Documento);

                //DescBanco
                GridColumn ValorPago = new GridColumn();
                ValorPago.FieldName = this.unboundPrefix + "VlrPago";
                ValorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorPago");
                ValorPago.UnboundType = UnboundColumnType.Decimal;
                ValorPago.VisibleIndex = 3;
                ValorPago.Width = 30;
                ValorPago.Visible = true;
                ValorPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorPago.AppearanceCell.Options.UseTextOptions = true;
                ValorPago.OptionsColumn.AllowEdit = true;
                ValorPago.ColumnEdit = this.editSpin0;
                this.gvPagos.Columns.Add(ValorPago);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddPagosCols"));
            }

        }

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de plan de pagos
        /// </summary>
        private void CalcularTotal_PlanPagos()
        {
            decimal vlrTotalPlanPagos = (from p in this.infoCartera.PlanPagos where p.PagoInd.Value.Value select p.VlrPagadoCuota.Value.Value).Sum();
            this.txtTotalPlanPagos.EditValue = vlrTotalPlanPagos;

            this.CalcularTotal_Componentes();
        }

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de componentes
        /// </summary>
        private void CalcularTotal_Componentes()
        {
            decimal vlrTotalComp = (from c in this.componentes select c.AbonoValor.Value.Value).Sum();
            this.txtTotalComponentes.EditValue = vlrTotalComp;
        }

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de forma de pago
        /// </summary>
        /// <param name="ValorPago">Valor a pagar </param>
        private void CalcularTotal_FormaPago()
        {
            decimal vlrTotalFormasPagos = (from f in this.formasPago select f.VlrPago.Value.Value).Sum();
            this.txtTotalFormaPago.EditValue = vlrTotalFormasPagos;
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;

            this.libranzaID = string.Empty;
            this.vlrExtra = 0;
            this.cliente = null;
            this.masterCaja.Value = String.Empty;
            this.masterBanco.Value = String.Empty;
            this.masterCliente.Value = String.Empty;
            this.lkp_Libranzas.Properties.DataSource = string.Empty;

            this.txtTotalPlanPagos.Text = "0";
            this.txtTotalComponentes.Text = "0";
            this.txtTotalFormaPago.Text = "0";

            this.infoCartera.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.infoCartera.SaldosComponentes = new List<DTO_ccSaldosComponentes>();
            this.gcDocument.DataSource = this.infoCartera.PlanPagos;
            this.gcDetails.DataSource = this.infoCartera.SaldosComponentes;
            this.gcPagos.DataSource = null;

            this.validate = true;
        }

        /// <summary>
        /// Funcion para cargar la grilla de pagos 
        /// </summary>
        private void LoadPagosGrid()
        {
            this.formasPago = new List<DTO_ccFormasPago>();
            List<DTO_MasterBasic> masterPagos = new List<DTO_MasterBasic>();
            long coutRegs = 0;
            try
            {
                coutRegs = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.tsFormaPago, null, null, true);
                masterPagos = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.tsFormaPago, coutRegs, 1, null, null, true).ToList();

                for (int i = 0; i < masterPagos.Count; i++)
                {
                    DTO_ccFormasPago formaPago = new DTO_ccFormasPago();
                    formaPago.FormaPagoID.Value = masterPagos[i].ID.Value;
                    formaPago.Descripcion.Value = masterPagos[i].Descriptivo.Value;

                    this.formasPago.Add(formaPago);
                }
                this.gcPagos.DataSource = formasPago;

                this.cuota = this.infoCartera.PlanPagos[0];
                this.componentes = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();
                this.vlrUsura = (from c in this.componentes where c.ComponenteCarteraID.Value == this.componenteUsura select c.CuotaSaldo.Value.Value).Sum();

                this.componentes.RemoveAll(x => x.CuotaSaldo.Value == 0 && x.TotalSaldo.Value == 0 && x.AbonoValor.Value == 0);
                this.componentes = this.componentes.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                this.gcDetails.DataSource = this.componentes;
                this.CalcularTotal_Componentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "LoadPagosGrid"));
            }
        }

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddComponente()
        {
            try
            {
                this.cuota = (DTO_ccCreditoPlanPagos)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);

                DTO_ccSaldosComponentes newComp = new DTO_ccSaldosComponentes();
                newComp.CuotaID.Value = this.cuota.CuotaID.Value;
                newComp.ComponenteCarteraID.Value = string.Empty;
                newComp.Descriptivo.Value = string.Empty;
                newComp.ComponenteFijo.Value = false;
                newComp.PagoTotalInd.Value = true;
                newComp.CuotaInicial.Value = 0;
                newComp.TotalInicial.Value = 0;
                newComp.CuotaSaldo.Value = 0;
                newComp.TotalSaldo.Value = 0;
                newComp.Editable.Value = true;

                this.infoCartera.SaldosComponentes.Insert(0, newComp);
                this.componentes = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();
                this.vlrUsura = (from c in this.componentes where c.ComponenteCarteraID.Value == this.componenteUsura select c.CuotaSaldo.Value.Value).Sum();

                this.componentes.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                this.gcDetails.DataSource = this.componentes;

                this.gvDetails.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddComponente"));
            }
        }

        /// <summary>
        /// Calcula el pago de los componentes
        /// </summary>
        private void CalcularPagoComponentes(int fila, decimal abono)
        {
            try
            {
                if (this.componentes.Count > 0)
                {
                    #region Calcula el cambio de valor en las cuotas

                    this.componentes.Where(c => !c.Editable.Value.Value).ToList().ForEach(c => c.AbonoValor.Value = 0);
                    abono -= this.componentes.Where(c => c.Editable.Value.Value).Sum(c => c.AbonoValor.Value.Value);

                    #region Revisa si la cuota tiene componente prejurídico

                    bool hasPJ = this.componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).Count() > 0 ? true : false;
                    if (hasPJ && this.porcHonorarios > 0)
                    {
                        decimal totalPJ = this.componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).First().CuotaSaldo.Value.Value;
                        decimal vlrTmp = abono - (Convert.ToInt32(abono / ((this.porcHonorarios / 100) + 1)));
                        decimal abonoPJ = Math.Round(vlrTmp / 1000, 0) * 1000;
                        abonoPJ = Convert.ToInt32(1000 * Math.Round(Convert.ToDouble(abonoPJ) / 1000));
                        if (abonoPJ > totalPJ)
                            abonoPJ = Convert.ToInt32(totalPJ);

                        abono -= abonoPJ;
                        this.componentes.Where(c => c.ComponenteCarteraID.Value == componentePJ).First().AbonoValor.Value = abonoPJ;
                    }

                    #endregion

                    //Paga el valor de los componentes
                    decimal abonoAct = 0;
                    for (int i = this.componentes.Count; i > 0; i--)
                    {
                        #region Asigna los pagos a los componentes de la cuota

                        if (!this.componentes[i - 1].Editable.Value.Value
                            && this.componentes[i - 1].ComponenteCarteraID.Value != componenteUsura
                            && this.componentes[i - 1].ComponenteCarteraID.Value != componentePJ)
                        {
                            decimal vlrDescuentoComp = 0;
                            decimal cuotaSaldoFinal = this.componentes[i - 1].CuotaSaldo.Value.Value;

                            if (this.vlrUsura != 0 && this.componentes[i - 1].ComponenteCarteraID.Value == this.componenteInteres)
                            {
                                #region Componentes dependientes de la usura
                                decimal interesReal = cuotaSaldoFinal + this.vlrUsura;

                                if (abono <= interesReal)
                                {
                                    this.componentes[i - 1].AbonoValor.Value = abono;
                                    abono = 0;

                                    break;
                                }
                                else
                                {
                                    abonoAct = abono - interesReal;
                                    this.componentes[i - 1].AbonoValor.Value = cuotaSaldoFinal;

                                    // Pago de usura
                                    DTO_ccSaldosComponentes usura = this.componentes.Where(c => c.ComponenteCarteraID.Value == this.componenteUsura).First();
                                    usura.AbonoValor.Value = usura.CuotaSaldo.Value;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Componentes fijos de pago
                                if (abono <= cuotaSaldoFinal)
                                {
                                    vlrDescuentoComp = abono;

                                    this.componentes[i - 1].AbonoValor.Value = vlrDescuentoComp;
                                    abono = 0;

                                    break;
                                }
                                else
                                {
                                    vlrDescuentoComp = cuotaSaldoFinal;

                                    abonoAct = abono - cuotaSaldoFinal;
                                    this.componentes[i - 1].AbonoValor.Value = this.componentes[i - 1].CuotaSaldo.Value.Value;
                                }
                                #endregion
                            }

                            abono = abonoAct;
                        }

                        #endregion
                    }

                    #endregion
                    
                    this.gcDetails.DataSource = null;
                    this.componentes.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                    this.gcDetails.DataSource = this.componentes;

                    //this.vlrExtra = abono > 0 ? abono : 0;
                    this.vlrExtra = this.componentes.Sum(c => c.AbonoValor.Value.Value);
                    this.vlrExtra = this.infoCartera.PlanPagos[this.gvDocument.FocusedRowHandle].VlrPagadoCuota.Value.Value - this.vlrExtra;
                    this.CalcularTotal_Componentes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "CalcularPagoComponentes"));
            }
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            this.ValidateRow_PlanPagos("PagoInd", this.gvDocument.FocusedRowHandle);

            if (!this.masterCaja.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCaja.LabelRsx);
                MessageBox.Show(msg);
                this.masterCaja.Focus();
                return false;
            }

            if (!this.masterCliente.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                MessageBox.Show(msg);
                this.masterCliente.Focus();
                return false;
            }

            if (this.infoCartera.PlanPagos.Count == 0)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lkp_Libranzas.Text);
                MessageBox.Show(msg);
                this.lkp_Libranzas.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida una fila del plan de pagos
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_PlanPagos(string fieldName, int fila)
        {
            this.validPlanPagos = true;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "PagoInd")
            {
                if (infoCartera.PlanPagos[fila].PagoInd.Value == true)
                {
                    if (infoCartera.PlanPagos[fila].VlrPagadoCuota.Value == 0 &&  !this.componentes.Any(x => x.CuotaSaldo.Value != 0))
                    {
                        this.gvDocument.SetColumnError(col, msgPagoInd);
                        this.validPlanPagos = false;
                    }
                    else if (this.vlrExtra != 0)
                    {
                        string m = string.Format(msgExtraInvalid, this.vlrExtra.ToString());
                        this.gvDocument.SetColumnError(col, m);
                        this.validPlanPagos = false;
                    }
                    else if ((from c in this.componentes where string.IsNullOrWhiteSpace(c.Descriptivo.Value) select c).Count() > 0)
                    {
                        this.gvDocument.SetColumnError(col, msgExtraEmpty);
                        this.validPlanPagos = false;
                    }
                    else if ((from c in this.componentes where c.Editable.Value.Value && c.AbonoValor.Value.Value == 0 select c).Count() > 0)
                    {
                        this.gvDocument.SetColumnError(col, msgExtraCero);
                        this.validPlanPagos = false;
                    }
                }

                if (this.validPlanPagos)
                    this.gvDocument.SetColumnError(col, string.Empty);
            }

            if (fieldName == "VlrPagadoCuota")
            {
                if (this.infoCartera.PlanPagos[fila].VlrPagadoCuota.Value < 0)
                {
                    string msg = string.Format(this.msgValorNeg, fieldName);
                    this.gvDocument.SetColumnError(col, msg);
                    this.validPlanPagos = false;
                }

                if (this.validPlanPagos)
                    this.gvDocument.SetColumnError(col, string.Empty);
            }
        }

        /// <summary>
        /// Valida los compoennetes extras
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private bool ValidateRow_Componentes(int fila)
        {
            if(this.componentes[fila].Editable.Value.Value)
            {
                if(string.IsNullOrWhiteSpace(this.componentes[fila].Descriptivo.Value))
                {
                    MessageBox.Show(msgExtraEmpty);
                    return false;
                }
                else if(this.componentes[fila].AbonoValor.Value == 0)
                {
                    MessageBox.Show(msgExtraCero);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Valida una fila de la grilla de formas de pago
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_FormaPago(string fieldName, int fila)
        {
            this.validFormasPago = true;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
            if (fieldName == "ValorPago")
            {
                if (this.formasPago[fila].VlrPago.Value < 0)
                {
                    string msg = string.Format(this.msgValorNeg, fieldName);
                    this.gvDocument.SetColumnError(col, msg);
                    this.validFormasPago = false;

                }

                if (this.validFormasPago)
                    this.gvDocument.SetColumnError(col, string.Empty);
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
            base.Form_Enter(sender, e); 
            
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;         
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemEdit.Visible = true;           
            FormProvider.Master.itemEdit.ToolTipText = "Editar los valores de los componentes";
            FormProvider.Master.itemEdit.Enabled = !this._allowEditValuesInd;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
              
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que filtra una lista de DTO_ccCreditoDocu de acuerdo al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    this._creditos = new List<DTO_ccCreditoDocu>();
                    if (this.masterCliente.ValidID)
                    {
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this._creditos = _bc.AdministrationModel.GetCreditosPendientesByCliente(this.masterCliente.Value);

                        if (this._creditos.Count == 0)
                        {
                            string tmpCaja = this.masterCaja.Value;
                            DateTime fechaConsignaTemp = this.dtFechaConsigna.DateTime;
                            DateTime fechaAplicaTemp = this.dtFechaAplica.DateTime;
                            string clienteTemp = this.masterCliente.Value;

                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinCreditoRecaudos);
                            MessageBox.Show(msg);
                            this.CleanData();

                            this.masterCaja.Value = tmpCaja;
                            this.dtFechaConsigna.DateTime = fechaConsignaTemp;
                            this.dtFechaAplica.DateTime = fechaAplicaTemp;
                            this.masterCliente.Value = clienteTemp;

                            if (this.dtFechaAplica.DateTime.Day == 31)
                                this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);
                        }
                        else
                        {
                            this.lkp_Libranzas.Properties.DataSource = this._creditos;
                        }
                    }
                    else
                        this.cliente = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que carga la grilla del cliente de acuerdo a la libranza seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Libranzas_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.lkp_Libranzas.Text)
                    && (this.libranzaID != this.lkp_Libranzas.Text || this.fechaRecaudo != this.dtFechaConsigna.DateTime.Date))
                {
                    List<DTO_ccCreditoDocu> temp = (from c in this._creditos where c.Libranza.Value.Value.ToString() == this.lkp_Libranzas.Text select c).ToList();
                    if (temp.Count > 0)
                    {
                        this.fechaRecaudo = this.dtFechaConsigna.DateTime.Date;
                        this.libranzaID = this.lkp_Libranzas.Text;
                        this._credito = temp.First();
                        int numeroDoc = Convert.ToInt32(this.lkp_Libranzas.EditValue.ToString());
                        this.infoCartera = this._bc.AdministrationModel.GetSaldoCredito(numeroDoc, this.fechaRecaudo, true, true, true,false);

                        foreach (var pp in this.infoCartera.PlanPagos)
                        {
                            var comps = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == pp.CuotaID.Value).ToList();
                            pp.VlrSaldoComponentes.Value = comps.Sum(x => x.CuotaSaldo.Value); 
                        }

                        if (this.infoCartera.PlanPagos.Count > 0)
                        {
                            if (this._credito.EC_FijadoInd.Value == false || this._credito.EC_FijadoInd.Value == null)
                            {
                                this.infoCartera.PlanPagos.ForEach(x => x.PagoInd.Value = false);

                                #region Info para el cobro prejurídico

                                bool hasPJ = this.infoCartera.SaldosComponentes.Where(x => x.ComponenteCarteraID.Value == this.componentePJ).Count() > 0 ? true : false;
                                if (hasPJ)
                                {
                                    this.porcHonorarios = 0;
                                    string abogadoID = this.cliente.AbogadoID.Value;
                                    if (string.IsNullOrWhiteSpace(abogadoID))
                                        abogadoID = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AbogadoCobroPrejuridico);

                                    if (!string.IsNullOrWhiteSpace(abogadoID))
                                    {
                                        DTO_ccAbogado abogado = (DTO_ccAbogado)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAbogado, false, abogadoID, true);
                                        this.porcHonorarios = abogado.PorHonorarios.Value.Value;
                                    }
                                }

                                #endregion

                                this.LoadPagosGrid();
                                this.gcDocument.DataSource = this.infoCartera.PlanPagos;
                                this.gvDocument.PostEditor();
                            }
                            else
                            {
                                bool isCobroJuridicoAcuerdo = this._credito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico || this._credito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago || this._credito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido ? true : false;
                                if (this.infoCartera.PlanPagos.First().CuotaID.Value < this._credito.EC_PrimeraCtaPagada.Value || isCobroJuridicoAcuerdo)
                                {
                                    if (!isCobroJuridicoAcuerdo)
                                        this.infoCartera.PlanPagos = this.infoCartera.PlanPagos.Where(x => x.CuotaID.Value < this._credito.EC_PrimeraCtaPagada.Value).ToList();
                                    this.infoCartera.PlanPagos.ForEach(x => x.PagoInd.Value = false);

                                    #region Info para el cobro prejurídico

                                    bool hasPJ = this.infoCartera.SaldosComponentes.Where(x => x.ComponenteCarteraID.Value == this.componentePJ).Count() > 0 ? true : false;
                                    if (hasPJ)
                                    {
                                        this.porcHonorarios = 0;
                                        string abogadoID = this.cliente.AbogadoID.Value;
                                        if (string.IsNullOrWhiteSpace(abogadoID))
                                            abogadoID = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AbogadoCobroPrejuridico);

                                        if (!string.IsNullOrWhiteSpace(abogadoID))
                                        {
                                            DTO_ccAbogado abogado = (DTO_ccAbogado)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAbogado, false, abogadoID, true);
                                            this.porcHonorarios = abogado.PorHonorarios.Value.Value;
                                        }
                                    }

                                    #endregion

                                    this.LoadPagosGrid();
                                    this.gcDocument.DataSource = this.infoCartera.PlanPagos;
                                    this.gvDocument.PostEditor();
                                }
                                else
                                {
                                    this.validate = false;
                                    this.libranzaID = string.Empty;
                                    this.gcDocument.DataSource = null;
                                    this.gcPagos.DataSource = null;
                                    this.txtTotalComponentes.Text = "0";
                                    this.txtTotalFormaPago.Text = "0";
                                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaFijado);
                                    MessageBox.Show(msg);
                                    this.validate = true;
                                }
                            }
                        }
                        else
                        {
                            this._credito = new DTO_ccCreditoDocu();
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);
                            MessageBox.Show(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }

            this.dtFechaConsigna.Properties.MaxValue = this.dtFecha.DateTime;
            this.dtFechaConsigna.DateTime = base.dtFecha.DateTime;

            //Pone la fecha de aplica con base a la del periodo
            this.dtFechaAplica.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
            if (this.dtFechaAplica.DateTime.Day == 31)
                this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);

            //Carga la infor del crédito
            this.lkp_Libranzas_Leave(sender, e);
        }

        /// <summary>
        /// Evento que asigna el valor maximo de la fecha de consignacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaAplica_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                this.dtFechaAplica.DateTimeChanged -= new System.EventHandler(this.dtFechaAplica_DateTimeChanged);

                this.dtFechaAplica.DateTime = new DateTime(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month, DateTime.DaysInMonth(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month));
                if (this.dtFechaAplica.DateTime.Day == 31)
                    this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);

                this.dtFechaAplica.DateTimeChanged += new System.EventHandler(this.dtFechaAplica_DateTimeChanged);

                this.dtFechaConsigna.Properties.MaxValue = this.dtFechaAplica.DateTime;
                this.dtFechaConsigna.DateTime = this.dtFechaConsigna.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "dtFechaAplica_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechaConsigna_DateTimeChanged(object sender, EventArgs e)
        {
            this.lkp_Libranzas_Leave(sender, e);
        }

        #endregion

        #region Eventos Grilla Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
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
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
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
        }

        /// <summary>
        /// Evento q valida antes de dejar el row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrPagadoCuota")
            {
                #region Valor a pagar
                decimal abono = 0;
                if (e.Value != null)
                    abono = (decimal)e.Value;
                else
                    this.infoCartera.PlanPagos[e.RowHandle].VlrPagadoCuota.Value = 0;

                this.ValidateRow_PlanPagos(fieldName, e.RowHandle);

                if (this.validPlanPagos)
                {
                    #region Habilita columnas de la grilla de documentos
                    if (this.infoCartera.PlanPagos[e.RowHandle].VlrPagadoCuota.Value > 0)
                        this.infoCartera.PlanPagos[e.RowHandle].PagoInd.Value = true;
                    else
                        this.infoCartera.PlanPagos[e.RowHandle].PagoInd.Value = false;

                    if (this.infoCartera.PlanPagos[e.RowHandle].VlrPagadoCuota.Value != this.infoCartera.PlanPagos[e.RowHandle].VlrCuota.Value)
                    {
                        for (int i = e.RowHandle + 1; i < this.infoCartera.PlanPagos.Count; ++i)
                        {
                            this.infoCartera.PlanPagos[i].VlrPagadoCuota.Value = 0;
                            this.infoCartera.PlanPagos[i].PagoInd.Value = false;
                        }
                    }

                    this.gcDocument.RefreshDataSource();
                    #endregion

                    this.CalcularPagoComponentes(e.RowHandle, abono);
                }

                this.CalcularTotal_PlanPagos();

                #endregion
            }

            if (fieldName == "PagoInd")
            {
                #region Cuotas a pagar
                if ((bool)e.Value)
                {
                    if (infoCartera.PlanPagos[e.RowHandle].VlrPagadoCuota.Value == 0)
                    {
                        infoCartera.PlanPagos[e.RowHandle].VlrPagadoCuota.Value = infoCartera.PlanPagos[e.RowHandle].VlrSaldo.Value;
                        this.CalcularPagoComponentes(e.RowHandle, infoCartera.PlanPagos[e.RowHandle].VlrSaldo.Value.Value);
                    }
                }
                else
                {
                    for (int i = e.RowHandle + 1; i < this.infoCartera.PlanPagos.Count; ++i)
                    {
                        infoCartera.PlanPagos[i].VlrPagadoCuota.Value = 0;
                        infoCartera.PlanPagos[i].PagoInd.Value = false;
                       
                        //Actualiza la info de los componentes
                        infoCartera.SaldosComponentes.RemoveAll(c => c.CuotaID.Value == infoCartera.PlanPagos[i].CuotaID.Value && c.Editable.Value.Value);
                        infoCartera.SaldosComponentes.Where(c => c.CuotaID.Value == infoCartera.PlanPagos[i].CuotaID.Value).ToList().ForEach(cc => cc.AbonoValor.Value = 0);
                    }
                    this.gcDocument.RefreshDataSource();
                }

                this.CalcularTotal_PlanPagos();
                #endregion
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.validate)
            {
                this.ValidateRow_PlanPagos("PagoInd", e.RowHandle);
                if (!this.validPlanPagos)
                    e.Allow = false;

                this.ValidateRow_PlanPagos("VlrPagadoCuota", e.RowHandle);
                if (!this.validPlanPagos)
                    e.Allow = false;

                if(this.validPlanPagos && !infoCartera.PlanPagos[e.RowHandle].PagoInd.Value.Value)
                {
                    this.infoCartera.SaldosComponentes.RemoveAll(c =>
                                                        c.CuotaID.Value == this.componentes[this.gvDetails.FocusedRowHandle].CuotaID.Value &&
                                                        c.Editable.Value.Value
                                                    );
                }
            }
        }

        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                int row = e.FocusedRowHandle;
                this.vlrExtra = 0;
                if (row > 0)
                {
                    if (this.infoCartera.PlanPagos[row - 1].VlrSaldo.Value == this.infoCartera.PlanPagos[row - 1].VlrPagadoCuota.Value &&
                        this.infoCartera.PlanPagos[row - 1].PagoInd.Value.Value)
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "VlrPagadoCuota"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                    }
                    else
                        this.gvDocument.Columns[this.unboundPrefix + "VlrPagadoCuota"].OptionsColumn.AllowEdit = false;
                }
                else
                    this.gvDocument.Columns[this.unboundPrefix + "VlrPagadoCuota"].OptionsColumn.AllowEdit = this._allowEditValuesInd;

                this.cuota = (DTO_ccCreditoPlanPagos)this.gvDocument.GetRow(e.FocusedRowHandle);
                this.componentes = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();
                this.vlrUsura = (from c in this.componentes where c.ComponenteCarteraID.Value == this.componenteUsura select c.CuotaSaldo.Value.Value).Sum();

                if (this.componentes.Count > 0)
                    this.cuota.VlrSaldoComponentes.Value = this.componentes.Sum(x => x.CuotaSaldo.Value); 

                if (this.componentes.Count > 0)
                {
                    if (this.componentes[0].Editable.Value.Value)
                    {
                        this.gvDetails.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                        //this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        this.gvDetails.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                        //this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = false;
                    }
                }

                //this.gvDocument.RefreshRow(this.gvDocument.FocusedRowHandle);
                this.componentes.RemoveAll(x => x.CuotaSaldo.Value == 0 && x.TotalSaldo.Value == 0 && x.AbonoValor.Value == 0);
                this.componentes.OrderBy(x => x.ComponenteCarteraID.Value).ToList();
                this.gcDetails.DataSource = this.componentes;
                this.CalcularTotal_Componentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "gldocuments_focusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Grilla Detalles

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDetails_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.componentes.Count > 0)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.gvDetails.PostEditor();
                        if (ValidateRow_Componentes(this.gvDetails.FocusedRowHandle))
                            this.AddComponente();
                    }
                    else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        this.gvDetails.PostEditor();
                        if (this.componentes[this.gvDetails.FocusedRowHandle].Editable.Value.Value)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.infoCartera.SaldosComponentes.RemoveAll(c =>
                                    c.CuotaID.Value == this.componentes[this.gvDetails.FocusedRowHandle].CuotaID.Value &&
                                    c.ComponenteCarteraID.Value == this.componentes[this.gvDetails.FocusedRowHandle].ComponenteCarteraID.Value
                                );

                                this.componentes = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();
                                this.vlrUsura = (from c in this.componentes where c.ComponenteCarteraID.Value == this.componenteUsura select c.CuotaSaldo.Value.Value).Sum();

                                this.CalcularPagoComponentes(this.gvDocument.FocusedRowHandle, this.infoCartera.PlanPagos[this.gvDocument.FocusedRowHandle].VlrPagadoCuota.Value.Value);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "gcDetails_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento q valida antes de dejar el row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ComponenteCarteraID")
            {
                DTO_ccCarteraComponente compTemp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, e.Value.ToString(), true, this.filtrosExtras);

                GridColumn col = this.gvDetails.Columns[unboundPrefix + "Descriptivo"];
                if (compTemp != null)
                {
                    this.componentes[this.gvDetails.FocusedRowHandle].TipoComponente.Value = compTemp.TipoComponente.Value;
                    this.gvDetails.SetRowCellValue(this.gvDetails.FocusedRowHandle, col, compTemp.Descriptivo.Value);
                }
                else
                {
                    this.componentes[this.gvDetails.FocusedRowHandle].TipoComponente.Value = null;
                    this.gvDetails.SetRowCellValue(this.gvDetails.FocusedRowHandle, col, string.Empty);
                }
            }

            if (fieldName == "AbonoValor")
            {
                if (this.componentes[e.RowHandle].Editable.Value.Value)
                {
                    GridColumn col = this.gvDetails.Columns[unboundPrefix + "CuotaSaldo"];
                    this.gvDetails.SetRowCellValue(this.gvDetails.FocusedRowHandle, col, e.Value.ToString());
                }
               
                this.cuota.VlrPagadoCuota.Value = this.componentes.Sum(c => c.AbonoValor.Value.Value);
                this.gvDocument.RefreshRow(this.gvDocument.FocusedRowHandle);
                this.CalcularTotal_PlanPagos();
                this.vlrExtra = this.componentes.Sum(c => c.AbonoValor.Value.Value);
                this.vlrExtra = this.infoCartera.PlanPagos[this.gvDocument.FocusedRowHandle].VlrPagadoCuota.Value.Value - this.vlrExtra;
            }
        }

        /// <summary>
        /// Evento q valida al salir de un Detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                int row = e.FocusedRowHandle;
                if (row >= 0)
                {
                    DTO_ccSaldosComponentes compTemp = (DTO_ccSaldosComponentes)this.gvDetails.GetRow(e.FocusedRowHandle);
                    if (compTemp.Editable.Value.Value)
                    {
                        this.gvDetails.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                        //this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        this.gvDetails.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                        //this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "gvDetails_FocusedRowChanged"));
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
                string colName = this.gvDetails.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDetails.FocusedRowHandle, colName, origin, this.filtrosExtras);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Grilla Formas de Pago

        /// <summary>
        /// Evento que valida el dato que se digita en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPagos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrPago")
            {
                this.ValidateRow_FormaPago(fieldName, e.RowHandle);
                this.CalcularTotal_FormaPago();
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvPagos_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            this.ValidateRow_FormaPago("ValorPago", e.RowHandle);
            if (!this.validFormasPago)
                e.Allow = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "TBNew"));
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
                this.gvDetails.PostEditor();
                this.gvPagos.PostEditor();

                bool isValid = this.ValidateDoc();

                //Valida la ultima fila del plan de pagos
                if (isValid && this.validPlanPagos && this.validFormasPago)
                {
                    List<DTO_ccCreditoPlanPagos> planPagos = (from p in this.infoCartera.PlanPagos.Where(p => p.PagoInd.Value.Value) select p).ToList();
                    List<DTO_ccSaldosComponentes> componentesPagos = new List<DTO_ccSaldosComponentes>();
                    foreach (DTO_ccCreditoPlanPagos p in planPagos)
                        componentesPagos.AddRange(from c in this.infoCartera.SaldosComponentes.Where(c => c.CuotaID.Value.Value == p.CuotaID.Value.Value) select c);

                    #region Validaciones

                    if (planPagos.Count == 0)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_PagoInd);
                        MessageBox.Show(msg);
                        return;
                    }

                    this.txtTotalComponentes.EditValue = (from p in componentesPagos select p.AbonoValor.Value.Value).Sum();
                    if (this.txtTotalPlanPagos.Text != this.txtTotalFormaPago.Text || this.txtTotalPlanPagos.Text != this.txtTotalComponentes.Text)
                    {
                        string mgsVlrTotales = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_ValoresNotEqual);
                        MessageBox.Show(mgsVlrTotales);
                        return;
                    }

                    #endregion
                    #region Carga el DTO de recibo de caja
                    DTO_tsReciboCajaDocu reciboCaja = new DTO_tsReciboCajaDocu();
                    reciboCaja.CajaID.Value = this.masterCaja.Value;
                    reciboCaja.BancoCuentaID.Value = this.masterBanco.ValidID ? this.masterBanco.Value : string.Empty;
                    reciboCaja.Valor.Value = Convert.ToDecimal(this.txtTotalPlanPagos.EditValue, CultureInfo.InvariantCulture);
                    reciboCaja.IVA.Value = 0;
                    reciboCaja.ClienteID.Value = this.masterCliente.Value;
                    reciboCaja.TerceroID.Value = this.masterCliente.Value;
                    reciboCaja.FechaConsignacion.Value = this.dtFechaConsigna.DateTime.Date;
                    reciboCaja.FechaAplica.Value = this.dtFechaAplica.DateTime.Date;
                    #endregion
                    #region Guarda la info
                    TipoRecaudo tipo = this._credito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico ? TipoRecaudo.CobroJuridico : TipoRecaudo.Normal;
                    componentesPagos.RemoveAll(x => x.CuotaSaldo.Value == 0 && x.TotalSaldo.Value == 0 && x.AbonoValor.Value == 0);
                    DTO_TxResult result = _bc.AdministrationModel.PagosCreditos_Parcial(tipo, this.documentID, this._actFlujo.ID.Value, base.dtFecha.DateTime, this.fechaRecaudo,
                        reciboCaja, this._credito, planPagos,null, componentesPagos);
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                        #region Variables para el mail

                        //DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                        //string body = string.Empty;
                        //string subject = string.Empty;
                        //string email = user.CorreoElectronico.Value;

                        //string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                        //string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                        //string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                        #endregion
                        #region Envia el correo
                        //subject = string.Format(subjectApr, formName);
                        //body = string.Format(bodyApr, formName, this._credito.Libranza.Value, this.cliente.Descriptivo.Value, string.Empty);
                        //_bc.SendMail(this.documentID, subject, body, email);
                        #endregion
                        #region Impresion del Reporte
                        int numDoc = Convert.ToInt32(result.ExtraField.ToString());
                        //Genera el reporte
                        this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numDoc);
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, null);
                        Process.Start(fileURl);
                        #endregion

                        this.CleanData();
                        this.masterCaja.Focus();
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                        if(result.ResultMessage == this._bc.GetResource(LanguageTypes.Error,DictionaryMessages.Err_ReportCreate))
                            this.CleanData();
                    }
                    #endregion
                    
                    //Valida si tiene autorizacion de edicion
                    this._allowEditValuesInd = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CambioVlrConPassword).Equals("1") ? false : true;
                    if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                        this._allowEditValuesInd = true;
                    this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                    this.gvDocument.Columns[this.unboundPrefix + "VlrPagadoCuota"].OptionsColumn.AllowEdit = this._allowEditValuesInd;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "TBSave"));
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
                            this.gvDetails.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                            this.gvDocument.Columns[this.unboundPrefix + "VlrPagadoCuota"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
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
