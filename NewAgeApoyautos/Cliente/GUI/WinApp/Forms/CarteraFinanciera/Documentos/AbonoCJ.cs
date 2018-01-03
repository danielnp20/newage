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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AbonoCJ : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Form Data
        private string libranzaID = string.Empty;
        private DateTime fechaRecaudo = DateTime.Now.Date;
        private DTO_InfoCredito infoCartera = new DTO_InfoCredito();
        private List<DTO_ccCreditoDocu> creditos = null;
        private DTO_ccCreditoDocu credito = null;
        private DTO_ccCreditoPlanPagos cuota = new DTO_ccCreditoPlanPagos();
        private List<DTO_ccSaldosComponentes> componentesSaldosAll = new List<DTO_ccSaldosComponentes>();
        private List<DTO_ccSaldosComponentes> componentesSaldosTemp = new List<DTO_ccSaldosComponentes>();
        private List<DTO_ccFormasPago> formasPago = new List<DTO_ccFormasPago>();

        //Variables de operación
        private string clienteID = String.Empty;
        private DTO_ccCliente cliente;
        private byte claseDeuda = 1;
        private bool isValid = true;
        private bool validFormasPago = true;
        private int diasInteres = 0; 
        private DTO_ccCJHistorico cjHistoricoPrincipal = null;
        private DTO_ccCJHistorico cjHistoricoPoliza = null;
        private TipoRecaudo tipoRecaudo = TipoRecaudo.CobroJuridico;

        //Info de componentes
        private string componenteCapital;
        private string componenteSeguro;
        private string componenteInteres;
        private string componenteMora;
        private DTO_ccCarteraComponente componenteMoraDTO;

        //Mensajes de error
        private string msgPagoInd;
        private string msgValorNeg;

        //Variables globales
        private int documentID;
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private ModulesPrefix _frmModule;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private string unboundPrefix = "Unbound_";

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public AbonoCJ()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public AbonoCJ(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Funcion que llama el Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this.SetInitParameters();
            this._frmName = _bc.GetResource(LanguageTypes.Menu, "mnu_cc_AbonoCJ");//_bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            this._frmModule = ModulesPrefix.cf;

            FormProvider.Master.Form_Load(this, this._frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

            this.msgValorNeg = string.Format(this.msgValorNeg, this.lblVlrAPagar.Text);
            #region Carga la info de las actividades
            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

            if (actividades.Count != 1)
            {
                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                string actividadFlujoID = actividades[0];
                this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
            }
            #endregion
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.RecaudosManuales;
                this._frmModule = ModulesPrefix.cc;

                #region Info del header superior
                DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                this.txtDocumentoID.Text = this.documentID.ToString();
                this.txtDocDesc.Text = _bc.GetResource(LanguageTypes.Menu, "mnu_cc_AbonoCJ");

                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.dtPeriodo.DateTime = Convert.ToDateTime(periodoStr);
                this.dtPeriodo.Enabled = false;

                #endregion
                #region Inicia los controles y las grillas
               
                //Carga la grilla con las columnas
                this.AddGridCols();

                //Carga la maestra de comprador de cartera
                _bc.InitMasterUC(this.masterCaja, AppMasters.tsCaja, true, true, true, false);
                _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
                _bc.InitMasterUC(this.masterBanco, AppMasters.tsBancosCuenta, true, true, true, false);

                #endregion
                #region Carga los diccionarios para los ddl

                //Clases 
                Dictionary<byte, string> dicClaseDeuda = new Dictionary<byte, string>();
                dicClaseDeuda.Add((byte)ClaseDeuda.Principal, this._bc.GetResource(LanguageTypes.Tables, "tbl_Principal"));
                dicClaseDeuda.Add((byte)ClaseDeuda.Adicional, this._bc.GetResource(LanguageTypes.Tables, "tbl_Adicional"));
                this.cmbClaseDeuda.Properties.DataSource = dicClaseDeuda;
                this.cmbClaseDeuda.ItemIndex = 0;
                this.claseDeuda = 1;

                #endregion
                #region Estable la fecha con base a la fecha del periodo y fecha ultimo dia Cierre
                bool cierreValido = true;
                int diaCierre = 1;
                DateTime periodo = this.dtPeriodo.DateTime.Date;
                string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
                if (indCierreDiaStr == "1")
                {
                    string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                    if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                        diaCierreStr = "1";

                    diaCierre = Convert.ToInt16(diaCierreStr);
                    if (diaCierre > DateTime.DaysInMonth(periodo.Year, periodo.Month))
                    {
                        cierreValido = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                        this.masterCliente.EnableControl(false);
                        this.dtFechaAplica.Enabled = false;
                        this.dtFechaConsigna.Enabled = false;

                        FormProvider.Master.itemNew.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemPrint.Enabled = false;
                    }
                }
                #endregion
                #region Revisa si el cierre es válido
                if (cierreValido)
                {
                    //Pone la fecha de aplica con base a la del periodo
                    this.dtFechaAplica.Properties.MaxValue = new DateTime(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month, DateTime.DaysInMonth(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month));
                    this.dtFechaAplica.Properties.MinValue = new DateTime(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month, diaCierre);
                    this.dtFechaAplica.DateTime = new DateTime(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month, this.dtPeriodo.DateTime.Day);

                    //Pone la fecha de consignacion con base a la del periodo
                    this.dtFechaConsigna.Properties.MaxValue = this.dtFechaAplica.Properties.MaxValue;
                    this.dtFechaConsigna.Properties.MinValue = this.dtFechaAplica.Properties.MinValue;
                    this.dtFechaConsigna.DateTime = this.dtFechaAplica.DateTime;
                }
                #endregion
                #region Componentes y mensajes de error
                //Componentes
                this.componenteCapital = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                this.componenteSeguro = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                this.componenteInteres = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                this.componenteMora = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);

                //Mensajes de error
                this.msgPagoInd = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_AbonoNoValue);
                this.msgValorNeg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);

                #endregion
                #region Carga la información de la tasa de mora

                //Tasa de usura
                DTO_glDatosMensuales datosAnuales = (DTO_glDatosMensuales)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDatosAnuales, false, this.dtPeriodo.DateTime.ToString(FormatString.ControlDate), true);
                decimal tasaUsura = datosAnuales == null ? 0 : datosAnuales.Tasa2.Value.Value;

                //Tasa de mora
                this.componenteMoraDTO = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, this.componenteMora, true);
                decimal tasaMora = componenteMora == null ? 0 : componenteMoraDTO.PorcentajeID.Value.Value;
                if (this.componenteMoraDTO.PorcentajeID.Value.HasValue && this.componenteMoraDTO.PorcentajeID.Value.Value > tasaUsura && tasaUsura > 0)
                    this.componenteMoraDTO.PorcentajeID.Value = tasaUsura;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla de componentes
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
                TotalValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                TotalValor.OptionsColumn.AllowEdit = false;
                TotalValor.ColumnEdit = this.editSpin;
                this.gvDetails.Columns.Add(TotalValor);

                //Valor Cuota
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this.unboundPrefix + "CuotaSaldo";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaValor");
                cuotaValor.UnboundType = UnboundColumnType.Integer;
                cuotaValor.VisibleIndex = 3;
                cuotaValor.Width = 40;
                cuotaValor.Visible = true;
                cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cuotaValor.OptionsColumn.AllowEdit = false;
                cuotaValor.ColumnEdit = this.editSpin;
                this.gvDetails.Columns.Add(cuotaValor);

                //Pago Cuota
                GridColumn pagoCuota = new GridColumn();
                pagoCuota.FieldName = this.unboundPrefix + "AbonoValor";
                pagoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PagoCuota");
                pagoCuota.UnboundType = UnboundColumnType.Integer;
                pagoCuota.VisibleIndex = 4;
                pagoCuota.Width = 40;
                pagoCuota.Visible = true;
                pagoCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                pagoCuota.OptionsColumn.AllowEdit = false;
                pagoCuota.ColumnEdit = this.editSpin;
                this.gvDetails.Columns.Add(pagoCuota);

                #endregion
                #region Grilla de pagos
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
                GridColumn descripcionPag = new GridColumn();
                descripcionPag.FieldName = this.unboundPrefix + "Descripcion";
                descripcionPag.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcionPag.UnboundType = UnboundColumnType.String;
                descripcionPag.VisibleIndex = 1;
                descripcionPag.Width = 30;
                descripcionPag.Visible = true;
                descripcionPag.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcionPag.OptionsColumn.AllowEdit = false;
                this.gvPagos.Columns.Add(descripcionPag);

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
                ValorPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                ValorPago.OptionsColumn.AllowEdit = true;
                ValorPago.ColumnEdit = this.editSpin;
                this.gvPagos.Columns.Add(ValorPago);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            //Form Data
            this.libranzaID = string.Empty;

            //Variables de operación
            this.clienteID = string.Empty;
            this.cliente = null;
            this.credito = null;
            this.creditos = null;
            this.cuota = null;
            this.componentesSaldosAll = null;
            this.componentesSaldosTemp = null;
            this.formasPago = null;
            this.isValid = true;
            this.validFormasPago = true;
            this.claseDeuda = 1;
            this.diasInteres = 0;
            this.infoCartera = new DTO_InfoCredito();
            this.infoCartera.PlanPagos = new List<DTO_ccCreditoPlanPagos>();
            this.infoCartera.SaldosComponentes = new List<DTO_ccSaldosComponentes>();
            this.cjHistoricoPrincipal = null;
            this.cjHistoricoPoliza = null;

            //Controles formulario
            this.txtEstado.Text = string.Empty;
            this.masterCaja.Value = String.Empty;
            this.masterBanco.Value = String.Empty;
            this.masterCliente.Value = String.Empty;
            this.lkp_Libranzas.Properties.DataSource = string.Empty;
            this.txtVlrAPagar.EditValue = 0;
            this.txtVlrPend.EditValue = 0;
            this.txtVlrPendCap.EditValue = 0;
            this.txtVlrPendPol.EditValue = 0;
            this.txtVlrPendOtros.EditValue = 0;
            this.txtTotalComponentes.EditValue = 0;
            this.txtTotalFormaPago.EditValue = 0;

            //Grillas
            this.gcDetails.DataSource = this.infoCartera.SaldosComponentes;
            this.gcPagos.DataSource = null;
        }

        /// <summary>
        /// Calcula y asigna la información de los componentes a pagar
        /// </summary>
        private void LoadComponentesAPagar()
        {
            try
            {
                this.componentesSaldosTemp = new List<DTO_ccSaldosComponentes>();
                if (this.claseDeuda == (byte)ClaseDeuda.Principal)
                {
                    //Capital
                    this.componentesSaldosTemp = this.componentesSaldosAll.Where(c => c.ComponenteCarteraID.Value == this.componenteCapital).ToList();
                }
                else
                {
                    //Poliza, componentes de gasto y otros
                    this.componentesSaldosTemp = this.componentesSaldosAll.Where(c => c.ComponenteCarteraID.Value == this.componenteSeguro || c.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto).ToList();
                }

                this.LoadComponenteMora();
                this.gcDetails.DataSource = this.componentesSaldosTemp;
                this.CalcularTotal_Componentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "LoadComponentesAPagar"));
            }
        }

        /// <summary>
        /// Calcula la información para el componente de mora
        /// </summary>
        private void LoadComponenteMora()
        {
            try 
            {
                this.diasInteres = 0;
                if (this.claseDeuda == (byte)ClaseDeuda.Principal)
                {
                    #region Principal
                    if (Convert.ToDecimal(this.txtVlrPendCap.EditValue) > 0 && this.cjHistoricoPrincipal != null
                        && this.cjHistoricoPrincipal.FechaMvto.Value.HasValue && this.cjHistoricoPrincipal.SaldoCapital.Value.HasValue)
                    {
                        //this.cjHistoricoPoliza
                        DateTime fechaUltMvto = this.cjHistoricoPrincipal.FechaMvto.Value.Value;

                        //Asigna los componentes de interes
                        this.diasInteres = (this.dtFechaConsigna.DateTime - fechaUltMvto).Days;
                        decimal vlrInteres = Convert.ToInt32(this.cjHistoricoPrincipal.SaldoCapital.Value.Value / 30 * this.diasInteres * this.componenteMoraDTO.PorcentajeID.Value.Value / 100);
                        vlrInteres += this.cjHistoricoPrincipal.SaldoInteres.Value.Value;

                        //Crea el componente de mora
                        DTO_ccSaldosComponentes newComp = new DTO_ccSaldosComponentes();
                        newComp.CuotaID.Value = this.cuota.CuotaID.Value;
                        newComp.ComponenteCarteraID.Value = this.componenteMora;
                        newComp.Descriptivo.Value = this.componenteMoraDTO.Descriptivo.Value;
                        newComp.TipoComponente.Value = this.componenteMoraDTO.TipoComponente.Value;
                        newComp.ComponenteFijo.Value = false;
                        newComp.PagoTotalInd.Value = true;
                        newComp.CuotaInicial.Value = vlrInteres;
                        newComp.TotalInicial.Value = vlrInteres;
                        newComp.CuotaSaldo.Value = vlrInteres;
                        newComp.TotalSaldo.Value = vlrInteres;
                        newComp.Editable.Value = false;

                        if(vlrInteres > 0)
                            this.componentesSaldosTemp.Insert(this.componentesSaldosTemp.Count(), newComp);
                    }
                    #endregion
                }
                else
                {
                    #region Adicional
                    decimal sumAdicional = this.componentesSaldosTemp.Sum(c => c.CuotaSaldo.Value.Value);

                    if (sumAdicional > 0 && this.cjHistoricoPoliza != null
                        && this.cjHistoricoPoliza.FechaMvto.Value.HasValue && this.cjHistoricoPoliza.SaldoCapital.Value.HasValue)
                    {
                        //this.cjHistoricoPoliza
                        DateTime fechaUltMvto = this.cjHistoricoPoliza.FechaMvto.Value.Value;

                        //Asigna los componentes de interes
                        this.diasInteres = (this.dtFechaConsigna.DateTime - fechaUltMvto).Days;
                        decimal vlrInteres = Convert.ToInt32(this.cjHistoricoPoliza.SaldoCapital.Value.Value / 30 * this.diasInteres * this.componenteMoraDTO.PorcentajeID.Value.Value / 100);
                        vlrInteres += this.cjHistoricoPoliza.SaldoInteres.Value.Value;

                        //Crea el componente de mora
                        DTO_ccSaldosComponentes newComp = new DTO_ccSaldosComponentes();
                        newComp.CuotaID.Value = this.cuota.CuotaID.Value;
                        newComp.ComponenteCarteraID.Value = this.componenteMora;
                        newComp.Descriptivo.Value = this.componenteMoraDTO.Descriptivo.Value;
                        newComp.TipoComponente.Value = this.componenteMoraDTO.TipoComponente.Value;
                        newComp.ComponenteFijo.Value = false;
                        newComp.PagoTotalInd.Value = true;
                        newComp.CuotaInicial.Value = vlrInteres;
                        newComp.TotalInicial.Value = vlrInteres;
                        newComp.CuotaSaldo.Value = vlrInteres;
                        newComp.TotalSaldo.Value = vlrInteres;
                        newComp.Editable.Value = false;

                        if (vlrInteres > 0)
                            this.componentesSaldosTemp.Insert(this.componentesSaldosTemp.Count(), newComp);
                    }
                    #endregion
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "CalcularComponenteMora"));
            }
        }

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de componentes
        /// </summary>
        private void CalcularTotal_Componentes()
        {
            decimal vlrTotalComp = (from c in this.componentesSaldosTemp select c.AbonoValor.Value.Value).Sum();
            this.txtTotalComponentes.EditValue = vlrTotalComp;
        }

        /// <summary>
        /// Funcion que calcula el valor total de la grilla de forma de pago
        /// </summary>
        /// <param name="ValorPago">Valor a pagar </param>
        private void CalcularTotal_FormaPago()
        {
            decimal vlrTotalFormasPagos = (from f in this.formasPago select f.VlrPago.Value.Value).Sum();
            this.txtTotalFormaPago.Text = vlrTotalFormasPagos.ToString();
        }

        /// <summary>
        /// Funcion para cargar la grilla de pagos 
        /// </summary>
        private void LoadFormasPago()
        {
            this.formasPago = new List<DTO_ccFormasPago>();
            try
            {
                long coutRegs = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.tsFormaPago, null, null, true);
                List<DTO_MasterBasic> masterPagos = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.tsFormaPago, coutRegs, 1, null, null, true).ToList();

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "LoadPagosGrid"));
            }
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            if (String.IsNullOrWhiteSpace(this.masterCaja.Value))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCaja.LabelRsx);
                MessageBox.Show(msg);
                this.masterCaja.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.masterCliente.Value))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                MessageBox.Show(msg);
                this.masterCliente.Focus();
                return false;
            }

            if (this.cuota == null)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lkp_Libranzas.Text);
                MessageBox.Show(msg);
                this.lkp_Libranzas.Focus();
                return false;
            }

            decimal vlrPago = Convert.ToDecimal(this.txtVlrAPagar.EditValue);
            if (vlrPago <= 0)
            {
                MessageBox.Show(this.msgValorNeg);
                this.txtVlrAPagar.Focus();
                return false;
            }

            if (this.txtTotalFormaPago.Text != this.txtTotalComponentes.Text)
            {
                string mgsVlrTotales = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecaudoManual_ValoresNotEqual);
                MessageBox.Show(mgsVlrTotales);
                return false;
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
            GridColumn col = this.gvPagos.Columns[this.unboundPrefix + fieldName];
            if (fieldName == "ValorPago")
            {
                if (this.formasPago[fila].VlrPago.Value < 0)
                {
                    string msg = string.Format(this.msgValorNeg, fieldName);
                    this.gvPagos.SetColumnError(col, msg);
                    this.validFormasPago = false;
                }
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this._frmModule);

                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al momento de salir del cliente
        /// </summary>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                this.creditos = new List<DTO_ccCreditoDocu>();
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    if (this.masterCliente.ValidID)
                    {
                        List<byte> estados = new List<byte>();
                        estados.Add((byte)EstadoDeuda.Juridico);
                        estados.Add((byte)EstadoDeuda.AcuerdoPago);
                        estados.Add((byte)EstadoDeuda.AcuerdoPagoIncumplido);

                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.creditos = _bc.AdministrationModel.GetCreditosPendientesByClienteAndEstado(this.masterCliente.Value, estados);

                        if (this.creditos.Count == 0)
                        {
                            string tmpCaja = this.masterCaja.Value;
                            DateTime fechaConsignaTemp = this.dtFechaConsigna.DateTime;
                            DateTime fechaAplicaTemp = this.dtFechaAplica.DateTime;
                            string clienteTemp = this.masterCliente.Value;

                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinCreditoRecaudosCJ);
                            MessageBox.Show(msg);
                            this.CleanData();

                            this.masterCaja.Value = tmpCaja;
                            this.dtFechaConsigna.DateTime = fechaConsignaTemp;
                            this.dtFechaAplica.DateTime = fechaAplicaTemp;
                            this.masterCliente.Value = clienteTemp;
                        }
                        else
                        {
                            this.lkp_Libranzas.Properties.DataSource = this.creditos;
                        }
                    }
                    else
                        this.cliente = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "masterCliente_Leave"));
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
                if (!String.IsNullOrWhiteSpace(this.lkp_Libranzas.Text) && (this.libranzaID != this.lkp_Libranzas.Text || this.fechaRecaudo != this.dtFechaConsigna.DateTime.Date))
                {
                    this.txtVlrAPagar.EditValue = 0;
                    List<DTO_ccCreditoDocu> temp = (from c in this.creditos where c.Libranza.Value.Value.ToString() == this.lkp_Libranzas.Text select c).ToList();
                    if (temp.Any())
                    {
                        #region Asigna las variables

                        this.credito = temp.First();
                        this.fechaRecaudo = this.dtFechaConsigna.DateTime.Date;
                        this.libranzaID = this.lkp_Libranzas.Text;
                        
                        //Trae los saldos del crédito
                        int numeroDoc = Convert.ToInt32(this.lkp_Libranzas.EditValue.ToString());
                        this.infoCartera = this._bc.AdministrationModel.GetSaldoCredito(numeroDoc, this.dtFechaConsigna.DateTime, false, false, false);

                        #endregion
                        #region Calcula el estado
              
                        this.cmbClaseDeuda.ItemIndex = 0;
                        if (this.credito.EstadoDeuda.Value.Value == (byte)EstadoDeuda.Juridico)
                        {
                            this.txtEstado.Text = this._bc.GetResource(LanguageTypes.Tables, "tbl_CobroJuridico");
                        }
                        else if (this.credito.EstadoDeuda.Value.Value == (byte)EstadoDeuda.AcuerdoPago)
                        {
                            this.txtEstado.Text = this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoPago");
                        }
                        else if (this.credito.EstadoDeuda.Value.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                        {
                            this.txtEstado.Text = this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoIncumplido");
                        }
                        #endregion

                        //validacion plan de pagos
                        if (this.infoCartera.PlanPagos.Count == 1)
                        {
                            this.infoCartera.PlanPagos.ForEach(x => x.PagoInd.Value = true);
                            this.cuota = this.infoCartera.PlanPagos[0];
                            this.componentesSaldosAll = this.infoCartera.SaldosComponentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();

                            this.txtVlrPend.EditValue = this.componentesSaldosAll.Sum(c => c.CuotaSaldo.Value.Value);
                            this.txtVlrPendCap.EditValue = this.componentesSaldosAll.Where(w => w.ComponenteCarteraID.Value == this.componenteCapital).Sum(c => c.CuotaSaldo.Value.Value);
                            this.txtVlrPendPol.EditValue = this.componentesSaldosAll.Where(w => w.ComponenteCarteraID.Value == this.componenteSeguro).Sum(c => c.CuotaSaldo.Value.Value);
                            this.txtVlrPendOtros.EditValue = this.componentesSaldosAll.Where(w => w.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto).Sum(c => c.CuotaSaldo.Value.Value);

                            //Revisa la información de los últimos movimetnos de CJ
                            var cjHistoricos = _bc.AdministrationModel.ccCJHistorico_GetForAbono(numeroDoc);
                            this.cjHistoricoPrincipal = cjHistoricos.Item1;
                            if (this.cjHistoricoPrincipal != null && !this.cjHistoricoPrincipal.SaldoInteres.Value.HasValue)
                                this.cjHistoricoPrincipal.SaldoInteres.Value = 0;
                            
                            this.cjHistoricoPoliza = cjHistoricos.Item2;
                            if (this.cjHistoricoPoliza != null && !this.cjHistoricoPoliza.SaldoInteres.Value.HasValue)
                                this.cjHistoricoPoliza.SaldoInteres.Value = 0;

                            //Asigna la información a las grillas
                            this.LoadFormasPago();
                            this.LoadComponentesAPagar();

                            //Asigna el tipo de recuado
                            this.tipoRecaudo = TipoRecaudo.CobroJuridico;
                            if(credito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago)
                            {
                                this.tipoRecaudo = TipoRecaudo.AcuerdoPago;
                            }
                            else if (credito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                            {
                                this.tipoRecaudo = TipoRecaudo.AcuerdoPagoIncumplido;
                            }
                        }
                        else if (this.infoCartera.PlanPagos.Count > 1)
                        {
                            this.credito = new DTO_ccCreditoDocu();
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_CreditoCJMultiplesCuotas);
                            MessageBox.Show(msg);
                        }
                        else
                        {
                            this.credito = new DTO_ccCreditoDocu();
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);
                            MessageBox.Show(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Evento que asigna el valor maximo de la fecha de consignacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaAplica_DateTimeChanged(object sender, EventArgs e)
        {
            this.dtFechaConsigna.Properties.MaxValue = new DateTime(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month, this.dtFechaAplica.DateTime.Day);
            this.dtFechaConsigna.DateTime = new DateTime(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month, this.dtFechaAplica.DateTime.Day);
        }

        /// <summary>
        /// Evento que recalcula el plan de pagos con base a la fecha seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaConsigna_DateTimeChanged(object sender, EventArgs e)
        {
            this.lkp_Libranzas_Leave(sender, e);
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la clase de deuda
        /// </summary>
        private void cmbClaseDeuda_EditValueChanged(object sender, EventArgs e)
        {
            if (this.claseDeuda != Convert.ToByte(this.cmbClaseDeuda.EditValue))
            {
                this.claseDeuda = Convert.ToByte(this.cmbClaseDeuda.EditValue);
                this.txtVlrAPagar.EditValue = 0;
                this.componentesSaldosAll.ForEach(c => c.AbonoValor.Value = 0);
                this.LoadComponentesAPagar();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control con el valor a pagar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrAPagar_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal abono = Convert.ToDecimal(this.txtVlrAPagar.EditValue, CultureInfo.InvariantCulture);
                if (abono < 0)
                {
                    this.componentesSaldosAll.ForEach(c => c.AbonoValor.Value = 0);
                    this.txtVlrAPagar.EditValue = 0;
                    return;
                }
                else if (abono > this.componentesSaldosTemp.Sum(c => c.CuotaSaldo.Value.Value))
                {
                    abono = this.componentesSaldosTemp.Sum(c => c.CuotaSaldo.Value.Value);
                    this.txtVlrAPagar.EditValue = abono;
                }

                if (this.componentesSaldosTemp.Count >= 0)
                {
                    #region Calcula el cambio de valor en las cuotas

                    this.componentesSaldosTemp.ForEach(c => c.AbonoValor.Value = 0);

                    //Paga el valor de los componentes
                    decimal abonoAct = 0;
                    for (int i = this.componentesSaldosTemp.Count; i > 0; i--)
                    {
                        #region Asigna los pagos a los componentes de la cuota

                        decimal vlrDescuentoComp = 0;
                        decimal cuotaSaldoFinal = this.componentesSaldosTemp[i - 1].CuotaSaldo.Value.Value;

                        if (abono <= cuotaSaldoFinal)
                        {
                            vlrDescuentoComp = abono;

                            this.componentesSaldosTemp[i - 1].AbonoValor.Value = vlrDescuentoComp;
                            abono = 0;

                            break;
                        }
                        else
                        {
                            vlrDescuentoComp = cuotaSaldoFinal;

                            abonoAct = abono - cuotaSaldoFinal;
                            this.componentesSaldosTemp[i - 1].AbonoValor.Value = this.componentesSaldosTemp[i - 1].CuotaSaldo.Value.Value;
                        }
                        #endregion

                        abono = abonoAct;
                    }

                    #endregion

                    this.gcDetails.DataSource = null;
                    this.gcDetails.DataSource = this.componentesSaldosTemp;
                }

                this.CalcularTotal_Componentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "txtVlrAPagar_Leave"));
            }
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
            if (e.RowHandle >= 0 && this.formasPago != null && e.RowHandle < this.formasPago.Count())
            {
                this.ValidateRow_FormaPago("ValorPago", e.RowHandle);
                if (!this.validFormasPago)
                    e.Allow = false;
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbonoCJ.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvPagos.PostEditor();
            try
            {
                this.gvPagos.PostEditor();
                bool isValid = this.ValidateDoc();

                if (isValid && this.validFormasPago)
                {
                    decimal vlrAPagar = this.componentesSaldosTemp.Sum(c => c.CuotaSaldo.Value.Value);
                    decimal saldoSeguroYGastos = this.componentesSaldosAll.Where(c => c.ComponenteCarteraID.Value != this.componenteCapital).Sum(s => s.CuotaSaldo.Value.Value);
                    if (this.claseDeuda == (byte)ClaseDeuda.Principal && Convert.ToDecimal(this.txtVlrAPagar.EditValue) >= vlrAPagar && saldoSeguroYGastos > 0)
                    {
                        string msgTitle = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgCapital = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidPagoCapital);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgCapital, msgTitle, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    List<DTO_ccCreditoPlanPagos> planPagos = new List<DTO_ccCreditoPlanPagos>();
                    planPagos.Add(this.cuota);

                    #region Carga el DTO de recibo de caja
                    DTO_tsReciboCajaDocu reciboCaja = new DTO_tsReciboCajaDocu();
                    reciboCaja.CajaID.Value = this.masterCaja.Value;
                    reciboCaja.BancoCuentaID.Value = this.masterBanco.ValidID ? this.masterBanco.Value : string.Empty;
                    reciboCaja.Valor.Value = Convert.ToDecimal(this.txtVlrAPagar.EditValue, CultureInfo.InvariantCulture);
                    reciboCaja.IVA.Value = 0;
                    reciboCaja.ClienteID.Value = this.masterCliente.Value;
                    reciboCaja.TerceroID.Value = this.masterCliente.Value;
                    reciboCaja.FechaConsignacion.Value = this.dtFechaConsigna.DateTime.Date;
                    reciboCaja.FechaAplica.Value = this.dtFechaAplica.DateTime.Date;
                    #endregion
                    #region Creal los registros de ccCJHistorico

                    #region Interes

                    decimal vlrSaldoAntInteres = 0;
                    if (this.claseDeuda == (byte)ClaseDeuda.Principal && this.cjHistoricoPrincipal != null)
                        vlrSaldoAntInteres = this.cjHistoricoPrincipal.SaldoInteres.Value.Value;
                    else if (this.claseDeuda == (byte)ClaseDeuda.Adicional && this.cjHistoricoPoliza != null)
                        vlrSaldoAntInteres = this.cjHistoricoPrincipal.SaldoInteres.Value.Value;

                    decimal vlrSaldoTotalInteres = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteMora).Sum(s => s.CuotaSaldo.Value.Value);
                    decimal vlrAbonoInteres = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteMora).Sum(s => s.AbonoValor.Value.Value); 


                    DTO_ccCJHistorico moraCJ = new DTO_ccCJHistorico();
                    moraCJ.NumeroDoc.Value = credito.NumeroDoc.Value;
                    moraCJ.PeriodoID.Value = this.dtPeriodo.DateTime;
                    moraCJ.ClaseDeuda.Value = this.claseDeuda;
                    moraCJ.EstadoDeuda.Value = credito.EstadoDeuda.Value.Value;
                    moraCJ.FechaMvto.Value = this.dtFechaConsigna.DateTime;
                    moraCJ.FechaInicial.Value = this.dtFechaConsigna.DateTime;
                    moraCJ.VlrCuota.Value = this.cuota.VlrCuota.Value;
                    moraCJ.FechaFinal.Value = this.dtFechaConsigna.DateTime;

                    //Valores por registro
                    moraCJ.TipoMvto.Value = (byte)TipoMovimiento_CJHistorico.InteresMora;
                    moraCJ.Observacion.Value = "LIQUIDA INTERES";
                    moraCJ.Dias.Value = this.diasInteres;
                    moraCJ.PorInteres.Value = this.componenteMoraDTO.PorcentajeID.Value.Value;
                    moraCJ.MvtoCapital.Value = null;
                    moraCJ.SaldoCapital.Value = null;
                    moraCJ.MvtoInteres.Value = (vlrSaldoTotalInteres - vlrSaldoAntInteres) * -1;
                    moraCJ.SaldoInteres.Value = vlrSaldoTotalInteres;
                
                    reciboCaja.ccCJHistoricoInteres = moraCJ;

                    #endregion
                    #region Pago

                    decimal vlrSaldoInicialCapital = 0;
                    decimal vlrSaldoInicialGastos = 0;
                    decimal vlrMvtoCapital = 0;
                    decimal vlrMvtoGastos = 0;
                    if (this.claseDeuda == (byte)ClaseDeuda.Principal)
                    {
                        vlrSaldoInicialCapital = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteCapital).Sum(s => s.CuotaSaldo.Value.Value);
                        vlrMvtoCapital = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteCapital).Sum(s => s.AbonoValor.Value.Value);
                    }
                    else 
                    {
                        vlrSaldoInicialCapital = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteSeguro).Sum(s => s.CuotaSaldo.Value.Value);
                        vlrSaldoInicialGastos = this.componentesSaldosTemp.Where(c => c.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto).Sum(s => s.CuotaSaldo.Value.Value);
                        vlrMvtoCapital = this.componentesSaldosTemp.Where(c => c.ComponenteCarteraID.Value == componenteSeguro).Sum(s => s.AbonoValor.Value.Value);
                        vlrMvtoGastos = this.componentesSaldosTemp.Where(c => c.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto).Sum(s => s.AbonoValor.Value.Value);
                    }

                    DTO_ccCJHistorico pagoCJ = ObjectCopier.Clone(moraCJ);

                    //Valores por registro
                    pagoCJ.TipoMvto.Value = (byte)TipoMovimiento_CJHistorico.Abono;
                    pagoCJ.Observacion.Value = "ABONO DEUDA";
                    pagoCJ.Dias.Value = null;
                    pagoCJ.PorInteres.Value = null;
                    pagoCJ.MvtoCapital.Value = vlrMvtoCapital * -1;
                    pagoCJ.SaldoCapital.Value = vlrSaldoInicialCapital - vlrMvtoCapital;
                    pagoCJ.MvtoInteres.Value = vlrAbonoInteres * -1;
                    pagoCJ.SaldoInteres.Value = vlrSaldoTotalInteres - vlrAbonoInteres;

                    reciboCaja.ccCJHistoricoPago = pagoCJ;

                    if (vlrAbonoInteres > 0)
                    {
                        planPagos[0].VlrPagadoCuota.Value = vlrMvtoCapital;
                        planPagos[0].FechaLiquidaMora.Value = this.dtFechaConsigna.DateTime;
                        planPagos[0].VlrMoraLiquida.Value = vlrSaldoTotalInteres;
                        planPagos[0].VlrMoraPago.Value = vlrAbonoInteres;
                    }

                    #endregion

                    #endregion
                    #region Guarda la info

                    DTO_TxResult result = _bc.AdministrationModel.PagosCreditos_Parcial(this.tipoRecaudo, this.documentID, this._actFlujo.ID.Value,
                        this.dtFechaAplica.DateTime, this.dtFechaConsigna.DateTime, reciboCaja, this.credito, planPagos,null, this.componentesSaldosTemp);

                    #endregion

                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                        
                        this.CleanData();
                        this.masterCliente.Focus();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridico.cs", "TBSave"));
            }
        }

        #endregion

    }
}
