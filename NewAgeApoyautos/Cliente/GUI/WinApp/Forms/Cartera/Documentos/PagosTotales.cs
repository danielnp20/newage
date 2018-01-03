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
using System.Globalization;
using SentenceTransformer;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PagosTotales : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID = string.Empty;
        private int proposito;
        private DTO_ccCliente cliente;
        private List<DTO_ccCreditoDocu> _creditos = null;
        private DTO_ccCreditoDocu _credito = null;
        private List<DTO_ccEstadoCuentaComponentes> componentes = new List<DTO_ccEstadoCuentaComponentes>();
        private DTO_ccEstadoCuentaComponentes componente = new DTO_ccEstadoCuentaComponentes();
        private List<DTO_ccFormasPago> formasPago = new List<DTO_ccFormasPago>();

        private bool canEdit = false;
        private bool validComponentes = true;
        private bool validFormasPago = true;
        private DateTime fechaPerido;
        private bool _allowEditValuesInd = true;
        private string _userAutoriza = string.Empty;

        private List<DTO_glConsultaFiltro> filtrosExtras = new List<DTO_glConsultaFiltro>();

        // Mensajes Grillas
        private string msgValorNeg;
        private string msgExtraEmpty;
        private string msgExtraCero;
        private string msgExtraInvalid;

        #endregion

        public PagosTotales()
            : base()
        {
            //InitializeComponent();
        }

        public PagosTotales(string mod)
            : base(mod)
        {
        }

        #region Funciones Privadas

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de componentes
        /// </summary>
        private void CalcularValores()
        {
            //Opcion original
            decimal vlrAPagar = 0;
            decimal vlrAbonoPrevio = 0;
            if(this.componentes.Count > 0)
            {
                vlrAPagar = (from c in this.componentes select c.AbonoValor.Value.Value).Sum();
                vlrAbonoPrevio = this.componentes[0].VlrAbonoPrevio.Value.Value;
            }

            decimal vlrSaldo = vlrAPagar - vlrAbonoPrevio;

            this.txtVlrDeuda.EditValue = vlrAPagar;
            this.txtTotalComponentes.EditValue = vlrAPagar;
            this.txtVlrAbonado.EditValue = vlrAbonoPrevio;
            this.txtVlrSaldo.EditValue = vlrSaldo;
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
            this.canEdit = false;
            this.cliente = null;
            this.masterCaja.Value = String.Empty;
            this.masterBanco.Value = String.Empty;
            this.masterCliente.Value = String.Empty;
            this.libranzaID = string.Empty;
            this.lkp_Libranzas.Properties.DataSource = string.Empty;

            this.txtTotalComponentes.EditValue = 0;
            this.txtTotalFormaPago.EditValue = 0;
            this.txtVlrDeuda.EditValue = 0;
            this.txtVlrSaldo.EditValue = 0;

            this.componentes = null;
            this.componente = null;

            this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = false;

            this.gcDocument.DataSource = this.componentes;
            this.gcPagos.DataSource = null;
        }

        /// <summary>
        /// Funcion para cargar la grilla de pagos 
        /// </summary>
        private void LoadFormasPagosGrid()
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "LoadPagosGrid"));
            }
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
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

            if (this.componentes.Count == 0)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lkp_Libranzas.Text);
                MessageBox.Show(msg);
                this.lkp_Libranzas.Focus();
                return false;
            }

            decimal vlrSaldo = Convert.ToDecimal(this.txtVlrSaldo.EditValue, CultureInfo.InvariantCulture);
            if(vlrSaldo < 0)
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_SaldoPositivo);
                MessageBox.Show(msg);
                return false;
            }

            decimal vlrPago = Convert.ToDecimal(this.txtTotalFormaPago.EditValue, CultureInfo.InvariantCulture);
            if (vlrPago < 0)
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagoPositivo);
                MessageBox.Show(msg);
                return false;
            }

            decimal vlrTotalAPagar = Convert.ToDecimal(this.txtTotalFormaPago.EditValue, CultureInfo.InvariantCulture);
            if (vlrTotalAPagar > vlrSaldo)
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagoTotalExedido);
                MessageBox.Show(msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fila"></param>
        /// <returns></returns>
        private void ValidateRow_Componentes(string fieldName, int fila)
        {
            this.validComponentes = true;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            if (this.componente.Editable.Value.Value && fieldName == "ComponenteCarteraID")
            {
                if (string.IsNullOrWhiteSpace(this.componente.Descriptivo.Value))
                {
                    this.gvDocument.SetColumnError(col, msgExtraEmpty);
                    this.validComponentes = false;
                }
                else
                    this.gvDocument.SetColumnError(col, string.Empty);
            }
            
            if (fieldName == "AbonoValor")
            {
                if (this.componente.AbonoValor.Value < 0)
                {
                    string msg = string.Format(this.msgValorNeg, col.Caption);
                    this.gvDocument.SetColumnError(col, msg);
                    this.validComponentes = false;
                }
                else if (this.componente.Editable.Value.Value && this.componente.AbonoValor.Value == 0)
                {
                    string msg = string.Format(this.msgValorNeg, col.Caption);
                    this.gvDocument.SetColumnError(col, msg);
                    this.validComponentes = false;
                }
                else
                    this.gvDocument.SetColumnError(col, string.Empty);
            }
        }

        /// <summary>
        /// 
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

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddComponente()
        {
            try
            {
                DTO_ccEstadoCuentaComponentes newComp = new DTO_ccEstadoCuentaComponentes();
                newComp.NumeroDoc.Value = this._credito.DocEstadoCuenta.Value;
                newComp.ComponenteCarteraID.Value = string.Empty;
                newComp.Descriptivo.Value = string.Empty;
                newComp.SaldoValor.Value = 0;
                newComp.PagoValor.Value = 0;
                newComp.AbonoValor.Value = 0;
                newComp.Editable.Value = true;

                this.componentes.Add(newComp);
                this.gcDocument.DataSource = this.componentes;
                this.gvDocument.PostEditor();
                this.gvDocument.FocusedRowHandle = this.componentes.Count - 1;

                this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "AddComponente"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.PagosTotales;
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

            this.AddComponentesCols();
            this.AddPagosCols();

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[2].Height = 230;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            base.dtFecha.Enabled = false;

            //Carga la Informacion del Hedear
            _bc.InitMasterUC(this.masterCaja, AppMasters.tsCaja, true, true, true, false);
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            _bc.InitMasterUC(this.masterBanco, AppMasters.tsBancosCuenta,true,true,true,false);
            this.fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));

            //Pone la fecha de aplica con base a la del periodo
            bool cierreValido = true;
            int diaCierre = 1;
            string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
            if(indCierreDiaStr == "1")
            {
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                    diaCierreStr = "1";

                diaCierre = Convert.ToInt16(diaCierreStr);
                if(diaCierre > DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month))
                {
                    cierreValido = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                    this.masterCliente.EnableControl(false);
                    this.dtFechaConsigna.Enabled = false;

                    FormProvider.Master.itemNew.Enabled = false;
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
            }

            if (cierreValido)
            {
                base.dtFecha.Enabled = true;
             
                //Revisa si permite editar valores
                this.btnUpdValues.Visible = SecurityManager.HasAccess(this.documentID, FormsActions.SpecialRights);

                //Carga la info de los mensajes
                this.msgValorNeg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                this.msgExtraEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraVacios);
                this.msgExtraCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompExtraCero);
                this.msgExtraInvalid = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_CompVlrExtra);

                //Filtros de componentes extras
                this.filtrosExtras.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoComponente",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "5"
                });
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
                    this.dtFechaConsigna.DateTime = DateTime.Now.Date;
                }
                else
                {
                    this.dtFecha.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
                    this.dtFechaConsigna.DateTime = this.dtFecha.DateTime;
                }

                //Pone la fecha de consignacion con base a la del periodo
                this.dtFechaConsigna.Properties.MaxValue = this.dtFecha.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AfterInitialize"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de detalles
        /// </summary>
        protected void AddComponentesCols()
        {
            try
            {
                //Codigo Componente
                GridColumn comptCarteraID = new GridColumn();
                comptCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                comptCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                comptCarteraID.UnboundType = UnboundColumnType.String;
                comptCarteraID.VisibleIndex = 0;
                comptCarteraID.Width = 100;
                comptCarteraID.Visible = true;
                comptCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                comptCarteraID.OptionsColumn.AllowEdit = false;
                comptCarteraID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(comptCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);                

                //SaldoValor
                GridColumn saldoValor = new GridColumn();
                saldoValor.FieldName = this.unboundPrefix + "SaldoValor";
                saldoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoValor");
                saldoValor.UnboundType = UnboundColumnType.Integer;
                saldoValor.VisibleIndex = 2;
                saldoValor.Width = 200;
                saldoValor.Visible = true;
                saldoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                saldoValor.AppearanceCell.Options.UseTextOptions = true;
                saldoValor.OptionsColumn.AllowEdit = false;
                saldoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(saldoValor);

                //PagoValor
                GridColumn pagoValor = new GridColumn();
                pagoValor.FieldName = this.unboundPrefix + "PagoValor";
                pagoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoValor");
                pagoValor.UnboundType = UnboundColumnType.Integer;
                pagoValor.VisibleIndex = 3;
                pagoValor.Width = 200;
                pagoValor.Visible = true;
                pagoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                pagoValor.AppearanceCell.Options.UseTextOptions = true;
                pagoValor.OptionsColumn.AllowEdit = false;
                pagoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(pagoValor);

                //Abono Valor
                GridColumn abonoValor = new GridColumn();
                abonoValor.FieldName = this.unboundPrefix + "AbonoValor";
                abonoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AbonoValor");
                abonoValor.UnboundType = UnboundColumnType.Integer;
                abonoValor.VisibleIndex = 4;
                abonoValor.Width = 200;
                abonoValor.Visible = true;
                abonoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                abonoValor.AppearanceCell.Options.UseTextOptions = true;
                abonoValor.OptionsColumn.AllowEdit = false;
                abonoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(abonoValor);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "AddDocumentCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de forma de pagos
        /// </summary>
        protected virtual void AddPagosCols()
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
                descripcion.Width = 70;
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
                ValorPago.ColumnEdit = this.editSpin;
                this.gvPagos.Columns.Add(ValorPago);

                this.gvPagos.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "AddPagosCols"));
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
                FormProvider.Master.itemSave.Enabled = FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemGenerateTemplate.Visible = false;              
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
                this._creditos = new List<DTO_ccCreditoDocu>();
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    if (this.masterCliente.ValidID)
                    {
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.proposito = Convert.ToInt16(PropositoEstadoCuenta.Prepago);
                        this._creditos = _bc.AdministrationModel.GetCreditosPendientesByProposito(this.masterCliente.Value, this.proposito);

                        if (this._creditos.Count == 0)
                        {
                            string tmpCaja = this.masterCaja.Value;
                            DateTime fechaConsignaTemp = this.dtFechaConsigna.DateTime;
                            string clienteTemp = this.masterCliente.Value;

                            string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteForPagoTotal), this.masterCliente.Value);
                            MessageBox.Show(msg);
                            this.CleanData();

                            this.masterCaja.Value = tmpCaja;
                            this.dtFechaConsigna.DateTime = fechaConsignaTemp;
                            this.masterCliente.Value = clienteTemp;
                        }
                        else
                        {
                            this.lkp_Libranzas.Properties.DataSource = this._creditos;
                        }
                    }
                    else
                    {
                        this.cliente = null;
                        this.lkp_Libranzas.Properties.DataSource = string.Empty;
                        this.componentes = null;
                        this.gcDocument.DataSource = this.componentes;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "masterCliente_Leave"));
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
                if (!String.IsNullOrWhiteSpace(this.lkp_Libranzas.Text) && this.libranzaID != this.lkp_Libranzas.Text)
                {
                    List<DTO_ccCreditoDocu> temp = (from c in this._creditos where c.Libranza.Value.Value.ToString() == this.lkp_Libranzas.Text && (c.EC_FijadoInd.Value == true || c.EC_FijadoInd.Value == null) select c).ToList();
                    if (temp.Count > 0)
                    {
                        this.libranzaID = this.lkp_Libranzas.Text;
                        this._credito = temp.First();
                        int numeroDoc = this._credito.DocEstadoCuenta.Value.Value;
                        this.componentes = this._bc.AdministrationModel.EstadoCuenta_GetComponentesByNumeroDoc(numeroDoc, true);

                        this.LoadFormasPagosGrid();
                        this.CalcularValores();
                        this.gcDocument.DataSource = this.componentes;
                        this.gvDocument.PostEditor();
                    }
                    else
                    {
                        this.libranzaID = string.Empty;
                        this.gcDocument.DataSource = null;
                        this.gcPagos.DataSource = null;
                        this.txtTotalComponentes.EditValue = 0;
                        this.txtTotalFormaPago.EditValue = 0;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaNoFijado);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "lkp_Libranzas_Leave"));
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
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;

            this.dtFechaConsigna.Properties.MaxValue = this.dtFecha.DateTime;
            this.dtFechaConsigna.DateTime = base.dtFecha.DateTime;
        }

        /// <summary>
        /// Evento que indica si el usuario puede actualizar los valores de los componentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdValues_Click(object sender, EventArgs e)
        {
            if (this.componentes != null && this.componentes.Count > 0)
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgUpdateVals = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.UpdateValues);
                if (MessageBox.Show(msgUpdateVals, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.canEdit = true;
                    this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                }
            }
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
            if (e.IsSetData)
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

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (!this.canEdit || this.componentes.Count <= 0)
                {
                    e.Handled = true;
                    return;
                }

                //if (this.componentes.Count > 0)
                //{
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    this.AddComponente();
                }
                else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    if (this.componentes[this.gvDocument.FocusedRowHandle].Editable.Value.Value)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.componente = null;
                            this.componentes.RemoveAll(c => c.ComponenteCarteraID.Value == this.componentes[this.gvDocument.FocusedRowHandle].ComponenteCarteraID.Value);
                            this.gcDocument.DataSource = this.componentes;
                            this.CalcularValores();
                        }
                        e.Handled = true;
                    }
                    else
                        e.Handled = true;
                }
                //}
                //else
                //    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "gcDetails_EmbeddedNavigator_ButtonClick"));
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

            if (fieldName == "ComponenteCarteraID")
            {
                DTO_ccCarteraComponente compTemp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, e.Value.ToString(), true, this.filtrosExtras);
                string desc = compTemp != null ? compTemp.Descriptivo.Value : string.Empty;

                GridColumn col = this.gvDocument.Columns[unboundPrefix + "Descriptivo"];
                this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, col, desc);

                this.ValidateRow_Componentes(fieldName, e.RowHandle);
            }

            if (fieldName == "AbonoValor")
            {
                if (!this.componente.AbonoValor.Value.HasValue)
                {
                    this.componente.AbonoValor.Value = 0;
                    this.componentes[e.RowHandle].AbonoValor.Value = 0;
                }

                if (this.componente.Editable.Value.Value)
                {
                    GridColumn col = this.gvDocument.Columns[unboundPrefix + "SaldoValor"];
                    this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, col, e.Value.ToString());

                    GridColumn col1 = this.gvDocument.Columns[unboundPrefix + "PagoValor"];
                    this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, col1, e.Value.ToString());
                }

                this.ValidateRow_Componentes(fieldName, e.RowHandle);
                this.CalcularValores();
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (e.RowHandle >= 0 && this.componente != null && this.canEdit)
            {
                this.ValidateRow_Componentes("ComponenteCarteraID", e.RowHandle);
                if (this.validComponentes)
                    this.ValidateRow_Componentes("AbonoValor", e.RowHandle);

                if (!this.validComponentes)
                    e.Allow = false;
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
                if (row >= 0)
                {
                    this.componente = (DTO_ccEstadoCuentaComponentes)this.gvDocument.GetRow(e.FocusedRowHandle);

                    if (this.canEdit)
                    {
                        //this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = true;
                        if (this.componente.Editable.Value.Value)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "gvDetails_FocusedRowChanged"));
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
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin, this.filtrosExtras);
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
        private void gvPagos_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.masterCaja.Focus();
                this.gvDocument.PostEditor();
                this.gvPagos.PostEditor();

                bool isValid = this.ValidateDoc();
                bool isPagoParcial = false;

                if (isValid && this.validComponentes && this.validFormasPago)
                {
                    #region Carga el DTO de SaldoComponentes
                    List<DTO_ccSaldosComponentes> saldoComponentes = new List<DTO_ccSaldosComponentes>();
                    foreach (DTO_ccEstadoCuentaComponentes item in this.componentes)
                    {
                        DTO_ccSaldosComponentes saldoComp = new DTO_ccSaldosComponentes();
                        saldoComp.ComponenteCarteraID.Value = item.ComponenteCarteraID.Value;
                        saldoComp.Descriptivo.Value = item.Descriptivo.Value;

                        saldoComp.TotalSaldo.Value = item.SaldoValor.Value;
                        saldoComp.CuotaSaldo.Value = item.AbonoValor.Value;
                        saldoComp.AbonoValor.Value = item.AbonoValor.Value;

                        saldoComponentes.Add(saldoComp);
                    }
                    #endregion
                    #region Valida si es pago parcial o pago total
                    if (this.txtTotalFormaPago.Text != this.txtVlrSaldo.Text)
                    {
                        string msgTitle = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PagoTotal_PagoParcial);
                        if (MessageBox.Show(msg, msgTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            isPagoParcial = true;
                        else 
                            return;
                    }

                    #endregion
                    #region Carga el DTO de recibo de caja
                    DTO_tsReciboCajaDocu reciboCaja = new DTO_tsReciboCajaDocu();
                    reciboCaja.CajaID.Value = this.masterCaja.Value;
                    reciboCaja.BancoCuentaID.Value = this.masterBanco.ValidID ? this.masterBanco.Value : string.Empty;
                    reciboCaja.Valor.Value = Convert.ToDecimal(this.txtTotalFormaPago.EditValue, CultureInfo.InvariantCulture);
                    reciboCaja.IVA.Value = 0;
                    reciboCaja.ClienteID.Value = this.masterCliente.Value;
                    reciboCaja.TerceroID.Value = this.masterCliente.Value;
                    reciboCaja.FechaAplica.Value = base.dtFecha.DateTime;
                    reciboCaja.FechaConsignacion.Value = this.dtFechaConsigna.DateTime;
                    #endregion
                    #region Guarda la info
                    
                    DTO_ccCreditoDocu creditoDocuTemp = ObjectCopier.Clone(this._credito);
                    creditoDocuTemp.FechaPagoParcial.Value = this.dtFechaConsigna.DateTime;
                    
                    DTO_TxResult result = _bc.AdministrationModel.PagosCreditos_Total(this.documentID, this._actFlujo.ID.Value, base.dtFecha.DateTime, 
                        this.dtFechaConsigna.DateTime, reciboCaja, creditoDocuTemp, this.componentes, saldoComponentes, isPagoParcial);
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();

                        // En comentarios
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
                        if (!string.IsNullOrEmpty(result.ExtraField))
                        {
                            int numDoc = Convert.ToInt32(result.ExtraField.ToString());
                            //Genera el reporte
                            this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numDoc);
                            string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, null);
                            Process.Start(fileURl); 
                        }
                        #endregion

                        this.CleanData();
                        this.masterCaja.Focus();
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                    #endregion

                    //Valida si tiene autorizacion de edicion
                    this._allowEditValuesInd = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CambioVlrConPassword).Equals("1") ? false : true;
                    if (this._userAutoriza.Equals(this._bc.AdministrationModel.User.ID.Value))
                        this._allowEditValuesInd = true;
                    this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagosTotales.cs", "TBSave"));
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
                            this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = this._allowEditValuesInd;
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
