using System;
using System.Collections.Generic;
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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EstadoCuenta : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        private int numeroDoc = 0;
        private int primeraCuota = 0;
        private string libranzaID = string.Empty;
        private DateTime periodo;
        private DTO_ccCliente cliente;
        private DTO_ccCreditoDocu credito;
        private List<DTO_ccCreditoDocu> creditos;
        private List<DTO_ccEstadoCuentaComponentes> saldosTotales;
        private DTO_InfoCredito infoCreditoTemp = new DTO_InfoCredito();
        private DTO_InfoCredito infoCredito = new DTO_InfoCredito();
        private DTO_InfoCredito infoSaldos = new DTO_InfoCredito();
        private DTO_InfoCredito infoPagos = new DTO_InfoCredito();

        //Actividades
        private DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();
        private string actFlujoDigitacionID;

        //Info de control
        private string componenteCapitalID;
        private string componenteMoraID;
        private string componenteSancionID;
        private string componenteInteres;
        private string componenteInteresNoCausado;
        private string componenteSeguro;
        private string componenteAportes;

        private string compradorCarteraPropia;
        decimal multa = 0;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public EstadoCuenta()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public EstadoCuenta(string mod)
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

                this.btn_CuotasPagadas.Enabled = false;
                this.btn_CuotasPdts.Enabled = false;
                this.btn_Movimiento.Enabled = false;

                this.componenteCapitalID = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                this.componenteInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
                this.componenteMoraID = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
                this.componenteSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                this.componenteAportes = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentedeAportes);
                this.componenteInteresNoCausado = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresNoCausaDeuda);
                this.componenteSancionID = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteMultaPrepago);

                this.compradorCarteraPropia = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);

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
                List<string> actividadesDigitacion = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.DigitacionCredito);

                if (actividadesDigitacion.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, AppDocuments.DigitacionCredito.ToString()));
                }
                else
                {
                    this.actFlujoDigitacionID = actividadesDigitacion[0];
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "EstadoCuenta"));
            }
        }

        #region Funciones Privadas

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
        }

        /// <summary>
        /// Funcion q inicializa los controles del fpormulario
        /// </summary>
        private void InitControls()
        {
            try
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(0, string.Empty);
                dic.Add((byte)PropositoEstadoCuenta.Proyeccion, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Proyeccion"));
                dic.Add((byte)PropositoEstadoCuenta.Prepago, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_CancelacionDirecta"));
                dic.Add((byte)PropositoEstadoCuenta.RecogeSaldo, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_RecogeSaldo"));
                dic.Add((byte)PropositoEstadoCuenta.RestructuracionAbono, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Restructuracion"));
                dic.Add((byte)PropositoEstadoCuenta.EnvioCobroJuridico, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_EnvioCobroJuridico")); 
                dic.Add((byte)PropositoEstadoCuenta.Desistimiento, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Desistimiento"));
                
                this.lkp_Proposito.Properties.DataSource = dic;
                this.lkp_Proposito.Enabled = false;

                string diasLimite = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiasValidezEstado);
                int diaLimite = Convert.ToInt16(diasLimite);
               
                //Estable las fechas con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                bool cierreValido = true;
                int diaCierre = 1;
                string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
                if (indCierreDiaStr == "1")
                {
                    string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                    if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                        diaCierreStr = "1";

                    diaCierre = Convert.ToInt16(diaCierreStr);
                    if (diaCierre > DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month))
                    {
                        cierreValido = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                        this.masterCliente.EnableControl(false);
                        this.dt_FechaCorte.Enabled = false;

                        FormProvider.Master.itemNew.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemPrint.Enabled = false;
                    }
                }

                if (cierreValido)
                {
                    this.dt_FechaCorte.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                    this.dt_FechaCorte.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.AddMonths(1).Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.AddMonths(1).Month));
                    int diaActual = DateTime.Now.Day;
                    
                    if (this.periodo.Month == 2 && diaActual > 28)
                        diaActual = 28;
                    else if (diaActual > DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month))
                        diaActual = 30;
                    this.dt_FechaCorte.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, diaCierre);
                    this.dt_FechaLimite.DateTime = this.dt_FechaCorte.DateTime.AddDays(diaLimite);
                }
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
                //ComponenteCarteraID
                GridColumn componenteCarteraID = new GridColumn();
                componenteCarteraID.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                componenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComponenteCarteraID");
                componenteCarteraID.UnboundType = UnboundColumnType.String;
                componenteCarteraID.VisibleIndex = 0;
                componenteCarteraID.Width = 50;
                componenteCarteraID.Visible = true;
                componenteCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(componenteCarteraID);

                //Descripcion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "Descriptivo";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 1;
                Descripcion.Width = 50;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                //PagoValor
                GridColumn pagoValor = new GridColumn();
                pagoValor.FieldName = this._unboundPrefix + "PagoValor";
                pagoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PagoValor");
                pagoValor.UnboundType = UnboundColumnType.Decimal;
                pagoValor.VisibleIndex = 2;
                pagoValor.Width = 100;
                pagoValor.Visible = true;
                pagoValor.OptionsColumn.AllowEdit = false;
                pagoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(pagoValor);

                //Abono
                GridColumn abonoValor = new GridColumn();
                abonoValor.FieldName = this._unboundPrefix + "AbonoValor";
                abonoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPagarCalculado");
                abonoValor.UnboundType = UnboundColumnType.Decimal;
                abonoValor.VisibleIndex = 3;
                abonoValor.Width = 150;
                abonoValor.Visible = true;
                abonoValor.OptionsColumn.AllowEdit = false;
                abonoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(abonoValor);

                ////VlrPagar
                //GridColumn vlrPagar = new GridColumn();
                //vlrPagar.FieldName = this._unboundPrefix + "VlrPagar";
                //vlrPagar.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPagar");
                //vlrPagar.UnboundType = UnboundColumnType.Decimal;
                //vlrPagar.VisibleIndex = 4;
                //vlrPagar.Width = 150;
                //vlrPagar.Visible = true;
                //vlrPagar.OptionsColumn.AllowEdit = false;
                //vlrPagar.ColumnEdit = this.editSpin;
                //this.gvDocument.Columns.Add(vlrPagar);

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
            this.libranzaID = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.txt_SaldoMora.Text = "0";
            this.txt_SaldoPendiente.Text = "0";
            this.txt_SaldoTotal.Text = "0";
            this.txt_Altura.Text = string.Empty;
            this.txt_CuotasMora.Text = string.Empty;
            this.txt_NetoEstadoCta.Text = string.Empty;
            this.txt_Plazo.Text = string.Empty;
            this.txt_VlrCredito.Text = "0";
            this.txt_VlrCuota.Text = "0";
            this.txt_VlrLibranza.Text = "0";
            this.txt_ValorTotal.Text = "0";
            this.chkEstadoFijado.Checked = false;

            //Variables
            this.cliente = null;
            this.credito = null;
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.saldosTotales = new List<DTO_ccEstadoCuentaComponentes>();
            this.infoCreditoTemp = new DTO_InfoCredito();
            this.infoCredito = new DTO_InfoCredito();
            this.infoSaldos = new DTO_InfoCredito();
            this.infoPagos = new DTO_InfoCredito();

            //Combos
            this.lkp_Proposito.EditValue = 0;
            this.lkp_Libranzas.Properties.DataSource = this.creditos;
            this.cmbCuota.SelectedIndex = -1;
            this.cmbCuota.Items.Clear();

            //Deshabilitar controles
            this.lkp_Proposito.Enabled = false;
            this.btn_CuotasPdts.Enabled = false;
            this.btn_CuotasPagadas.Enabled = false;
            this.btn_Movimiento.Enabled = false;
            this.btnAnexos.Enabled = false;

            this.LoadGridData();
        }

        /// <summary>
        /// Funcion que no permite editar los controles de la pantalla
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los controles</param>
        private void EnableHeader(bool enabled)
        {
            this.lkp_Proposito.Enabled = enabled;
            this.dt_FechaCorte.Enabled = enabled;
            this.dt_FechaLimite.Enabled = enabled;
            this.cmbCuota.Enabled = enabled;
            this.chkEstadoFijado.Enabled = enabled;
        }

        /// <summary>
        /// Funcion que carga los controles
        /// </summary>
        private void LoadHeader()
        {
            try
            {
                //Valores del credito
                this.txt_VlrCredito.EditValue = this.credito.VlrGiro.Value;
                this.txt_VlrLibranza.EditValue = this.credito.VlrLibranza.Value;
                this.txt_Plazo.Text = this.credito.Plazo.Value.ToString();
                this.txt_VlrCuota.EditValue = this.credito.VlrCuota.Value;

                //Valores del saldo
                this.txt_SaldoTotal.EditValue = Convert.ToInt32((from c in this.saldosTotales select c.PagoValor.Value.Value).Sum());
                this.txt_SaldoPendiente.EditValue = this.txt_SaldoTotal.EditValue.ToString();
                this.txt_SaldoMora.EditValue = Convert.ToInt32((from c in this.saldosTotales where c.ComponenteCarteraID.Value == this.componenteMoraID select c.PagoValor.Value.Value).Sum());
                this.txt_ValorTotal.EditValue = 0;

                //Valores de pagos
                this.txt_NetoEstadoCta.EditValue = 0;
                this.txt_CuotasMora.Text = (from pp in this.infoCredito.PlanPagos where pp.VlrMoraLiquida.Value.Value != 0 select pp.CuotaID.Value.Value).Count().ToString();

                //Combo de primeras cuotas
                this.cmbCuota.Items.Clear();
                List<int> cuotas = (from pp in this.infoSaldos.PlanPagos select pp.CuotaID.Value.Value).Take(3).ToList();

                if (cuotas.Count > 0)
                {
                    this.txt_Altura.Text = cuotas.First().ToString();
                    foreach (int c in cuotas)
                        this.cmbCuota.Items.Add(c);

                    this.cmbCuota.SelectedIndex = 0;
                    this.lkp_Proposito.Enabled = true;
                }
                else
                {
                    this.txt_Altura.Text = "0";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "LoadHeader"));
            }
        }

        /// <summary>
        /// Funcion que carga la informacion del documento
        /// </summary>
        private void LoadCreditoInfo()
        {
            this.numeroDoc = Convert.ToInt32(this.lkp_Libranzas.EditValue.ToString());
            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numeroDoc, this.dt_FechaCorte.DateTime);
            this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);

            //Proposito
            this.lkp_Proposito.EditValue = this.credito.EC_Proposito.Value != null ? this.credito.EC_Proposito.Value.Value : 0;
            this.LoadGridData();

            this.LoadComponentesSaldos();
            decimal saldoPendienteTemp = Convert.ToDecimal(this.txt_SaldoPendiente.EditValue, CultureInfo.InvariantCulture);
            decimal pagoTotalTemp = Convert.ToDecimal(this.txt_ValorTotal.EditValue, CultureInfo.InvariantCulture);
            this.LoadComponentesPagos();

            this.LoadHeader();
            this.txt_SaldoPendiente.Text = saldoPendienteTemp.ToString();

            if (this.lkp_Proposito.EditValue.ToString() != "")
                this.txt_ValorTotal.Text = this.credito.EC_ValorPago.Value != null ? Convert.ToInt32(this.credito.EC_ValorPago.Value.Value).ToString() : pagoTotalTemp.ToString();

            this.btnAnexos.Enabled = true;
            if (infoCredito.EstadoCompraCartera.Value != 0)
            {
                if (infoCredito.EstadoCompraCartera.Value == (byte)EstadoDocControl.Aprobado)
                {
                    this.EnableHeader(false);
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaComprado);
                    MessageBox.Show(string.Format(msg, infoCredito.LibranzaCompraCartera.Value.ToString()));
                }
                else if(infoCredito.ActFlujoSolicitudCompraCartera.Value.Trim() != this.actFlujoDigitacionID.Trim())
                {
                    this.EnableHeader(false);
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaEnCompra);
                    MessageBox.Show(string.Format(msg, infoCredito.LibranzaCompraCartera.Value.ToString()));
                }
                else
                {
                    this.EnableHeader(true);
                }
            }
            else
                this.EnableHeader(true);

            this.chkEstadoFijado.Checked = this.credito.EC_FijadoInd.Value != null && this.credito.EC_FijadoInd.Value.Value ? true : false;
            this.btn_CuotasPagadas.Enabled = true;
            this.btn_CuotasPdts.Enabled = true;
            this.btn_Movimiento.Enabled = true;
        }

        /// <summary>
        /// Funcion que filta los componentes con los saldos actualizados
        /// </summary>
        private void LoadComponentesSaldos()
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
                decimal interesNoCausado = this.infoCreditoTemp.SaldosComponentes.Where(c => c.ComponenteCarteraID.Value == this.componenteInteresNoCausado).Sum(s => s.CuotaSaldo.Value.Value);
                this.txt_SaldoPendiente.EditValue = Convert.ToInt32((from c in componentes select (c.CuotaSaldo.Value.Value - c.AbonoValor.Value.Value)).Sum());
                this.txt_ValorTotal.EditValue = Convert.ToInt32(this.txt_SaldoTotal.EditValue) - Convert.ToInt32(this.txt_SaldoPendiente.EditValue) + Convert.ToInt32(this.multa) + interesNoCausado;
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
        private void LoadGridData()
        {
            this.saldosTotales = new List<DTO_ccEstadoCuentaComponentes>();
            List<DTO_ccSaldosComponentes> totales = new List<DTO_ccSaldosComponentes>();
            List<string> compsID = (from c in this.infoCreditoTemp.SaldosComponentes select c.ComponenteCarteraID.Value).Distinct().ToList();
            List<string> compsDescr = (from c in this.infoCreditoTemp.SaldosComponentes select c.Descriptivo.Value).Distinct().ToList();
            for (int i = 0; i < compsID.Count(); ++i)
            {
                DTO_ccEstadoCuentaComponentes comp = new DTO_ccEstadoCuentaComponentes();
                comp.ComponenteCarteraID.Value = compsID[i];
                comp.Descriptivo.Value = compsDescr[i];

                //comp.SaldoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where c.ComponenteCarteraID.Value == compsID[i] select c.CuotaSaldo.Value).Sum();
                //comp.PagoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where c.ComponenteCarteraID.Value == compsID[i] select c.AbonoValor.Value).Sum();
                //comp.AbonoValor.Value = comp.PagoValor.Value;
                //comp.VlrPagar.Value = comp.AbonoValor.Value;

                //Se ponen asi porque la columna de pago valor debe mostrar siempre el saldo y la columna de saldo se usa para cálculos
                comp.PagoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where c.ComponenteCarteraID.Value == compsID[i] select c.CuotaSaldo.Value).Sum();
                comp.SaldoValor.Value = (from c in this.infoCreditoTemp.SaldosComponentes where c.ComponenteCarteraID.Value == compsID[i] select c.AbonoValor.Value).Sum();
                comp.AbonoValor.Value = comp.SaldoValor.Value;
                comp.VlrPagar.Value = comp.AbonoValor.Value;


                this.saldosTotales.Add(comp);
            }

            this.gcDocument.DataSource = this.saldosTotales;
        }

        /// <summary>
        /// Carga la informacion de acuerdo al proposito
        /// </summary>
        private void LoadProposito(int primeraCuota)
        {
            try
            {
                this.infoCreditoTemp = ObjectCopier.Clone(this.infoCredito);
                this.infoCreditoTemp.PlanPagos = this.infoCreditoTemp.PlanPagos.Where(x => x.CuotaID.Value >= primeraCuota).ToList();
                this.infoCreditoTemp.SaldosComponentes = this.infoCreditoTemp.SaldosComponentes.Where(x => x.CuotaID.Value >= primeraCuota).ToList();
                this.multa = 0;

                if (this.lkp_Proposito.EditValue != null)
                {
                    DTO_ccSaldosComponentes ccIntNoCausado = null;
                    byte proposito = Convert.ToByte(this.lkp_Proposito.EditValue);
                    if (proposito == (byte)PropositoEstadoCuenta.Proyeccion)
                    {
                        #region Proyección

                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                        {
                            comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }
                            

                        #endregion
                    }
                    else if (proposito == (byte)PropositoEstadoCuenta.Prepago || proposito == (byte)PropositoEstadoCuenta.RecogeSaldo || proposito == (byte)PropositoEstadoCuenta.RestructuracionAbono)
                    {
                        #region Cancelacion Directa, Recompra y Restructuración (Abono Capital)
                        Dictionary<int, DateTime> pagosFecha = new Dictionary<int, DateTime>();
                        for (int i = 0; i < this.infoCreditoTemp.PlanPagos.Count; ++i)
                        {
                            DTO_ccCreditoPlanPagos pago = this.infoCreditoTemp.PlanPagos[i];
                            pagosFecha.Add(pago.CuotaID.Value.Value, pago.FechaCuota.Value.Value.Date);
                        }

                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                        {
                            if (comp.PagoTotalInd.Value.Value && comp.CuotaID.Value >= primeraCuota && comp.CuotaSaldo.Value.Value > 0)
                            {
                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                            }                                
                            else if (this.dt_FechaCorte.DateTime.Date > pagosFecha[comp.CuotaID.Value.Value] && comp.CuotaSaldo.Value.Value > 0
                                && comp.CuotaID.Value >= primeraCuota)
                            {
                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                            }
                            else if (this.dt_FechaCorte.DateTime.Month == pagosFecha[comp.CuotaID.Value.Value].Month && this.dt_FechaCorte.DateTime.Year == pagosFecha[comp.CuotaID.Value.Value].Year
                               && comp.ComponenteCarteraID.Value == this.componenteInteres && comp.CuotaID.Value >= primeraCuota)
                            {
                                int diasACausar = this.dt_FechaCorte.DateTime.Day;
                                decimal vlrComp = comp.CuotaSaldo.Value.Value;
                                decimal vlrCausado = Math.Round(diasACausar * vlrComp / 30);
                                ccIntNoCausado = this.LoadComponenteIntNoCausado(comp.CuotaID.Value.Value, vlrCausado);
                            }
                            else if ((comp.ComponenteCarteraID.Value == this.componenteAportes || comp.ComponenteCarteraID.Value == this.componenteSeguro) && 
                                this.dt_FechaCorte.DateTime.Month == pagosFecha[comp.CuotaID.Value.Value].Month && this.dt_FechaCorte.DateTime.Year == pagosFecha[comp.CuotaID.Value.Value].Year)
                            {
                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                            }
                        }

                        if (ccIntNoCausado != null)
                        {
                            this.infoCreditoTemp.SaldosComponentes.Add(ccIntNoCausado);
                        }

                        if (!String.IsNullOrWhiteSpace(this.componenteSancionID))
                        {

                            DTO_ccCreditoPlanPagos cuotaSancion = this.LoadCuotaSancion(this.infoCreditoTemp.PlanPagos.Last());
                            DTO_ccSaldosComponentes cSancion = this.LoadComponenteSancion(cuotaSancion.CuotaID.Value.Value, cuotaSancion.VlrCuota.Value.Value);

                            this.infoCreditoTemp.PlanPagos.Add(cuotaSancion);
                            this.infoCreditoTemp.SaldosComponentes.Add(cSancion);
                            this.multa = cSancion.CuotaInicial.Value.Value;
                        }
                        #endregion
                    }
                    else if (proposito == (byte)PropositoEstadoCuenta.EnvioCobroJuridico)
                    {
                        #region Envio Cobro jurídico
                        Dictionary<int, DateTime> pagosFecha = new Dictionary<int, DateTime>();
                        for (int i = 0; i < this.infoCreditoTemp.PlanPagos.Count; ++i)
                        {
                            DTO_ccCreditoPlanPagos pago = this.infoCreditoTemp.PlanPagos[i];
                            pagosFecha.Add(pago.CuotaID.Value.Value, pago.FechaCuota.Value.Value.Date);
                        }

                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                        {
                            if
                            (
                                (comp.ComponenteCarteraID.Value == this.componenteCapitalID || this.dt_FechaCorte.DateTime.Date > pagosFecha[comp.CuotaID.Value.Value])
                                && comp.CuotaSaldo.Value.Value > 0 && comp.CuotaID.Value >= Convert.ToInt32(this.cmbCuota.SelectedItem)
                            )
                                comp.AbonoValor.Value = comp.CuotaSaldo.Value;
                        }

                        #endregion
                    }
                    else if (proposito == (byte)PropositoEstadoCuenta.Desistimiento)
                    {
                        #region Desistimiento

                        foreach (DTO_ccSaldosComponentes comp in this.infoCreditoTemp.SaldosComponentes)
                            comp.AbonoValor.Value = 0;

                        #endregion
                    }
                }

                this.LoadGridData();
                this.LoadComponentesSaldos();
                //this.LoadComponentesPagos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "LoadPropositos"));
            }
        }

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
            cuotaSancion.FechaCuota.Value = this.dt_FechaCorte.DateTime;
            cuotaSancion.FechaLiquidaMora.Value = this.dt_FechaCorte.DateTime;
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
        private DTO_ccSaldosComponentes LoadComponenteSancion(int cuotaID, decimal valor)
        {
            DTO_ccCarteraComponente comp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, componenteSancionID, true);

            DTO_ccSaldosComponentes cSancion = new DTO_ccSaldosComponentes();
            cSancion.CuotaID.Value = cuotaID;
            cSancion.ComponenteCarteraID.Value = componenteSancionID;
            cSancion.Descriptivo.Value = comp.Descriptivo.Value;
            cSancion.ComponenteFijo.Value = true;
            cSancion.PagoTotalInd.Value = true;
            cSancion.CuotaInicial.Value = valor;
            cSancion.TotalInicial.Value = valor;
            cSancion.CuotaSaldo.Value = valor;
            cSancion.TotalSaldo.Value = valor;
            cSancion.AbonoValor.Value = valor;

            return cSancion;
        }

        /// <summary>
        /// Carga el componente de sancion para la ultima cuota
        /// </summary>
        /// <param name="cuotaID">Identificador de la cuota</param>
        /// <param name="valor">Valor a pagar</param>
        /// <returns>Retorna el componente</returns>
        private DTO_ccSaldosComponentes LoadComponenteIntNoCausado(int cuotaID, decimal valor)
        {
            DTO_ccCarteraComponente comp = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, this.componenteInteresNoCausado, true);

            DTO_ccSaldosComponentes cInteresCausado = new DTO_ccSaldosComponentes();
            cInteresCausado.CuotaID.Value = cuotaID;
            cInteresCausado.ComponenteCarteraID.Value = this.componenteInteresNoCausado;
            cInteresCausado.Descriptivo.Value = comp.Descriptivo.Value;
            cInteresCausado.ComponenteFijo.Value = true;
            cInteresCausado.PagoTotalInd.Value = true;
            cInteresCausado.CuotaInicial.Value = valor;
            cInteresCausado.TotalInicial.Value = valor;
            cInteresCausado.CuotaSaldo.Value = valor;
            cInteresCausado.TotalSaldo.Value = valor;
            cInteresCausado.AbonoValor.Value = valor;

            return cInteresCausado;
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
                    FormProvider.Master.itemSearch.Visible = false;
                }
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
                this.creditos = new List<DTO_ccCreditoDocu>();
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    if (this.masterCliente.ValidID)
                    {
                        this.cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        this.creditos = _bc.AdministrationModel.GetCreditosPendientesByCliente(this.masterCliente.Value);

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
                            this.lkp_Libranzas.Properties.DataSource = this.creditos;
                        }
                    }
                    else
                        this.cliente = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que carga los controles de acuerdo a la libranza seleccionada
        /// </summary>
        /// <param name="sender">Evento q envia el evento</param>
        /// <param name="e"></param>
        private void lkp_Libranzas_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.lkp_Libranzas.Text) && this.libranzaID != this.lkp_Libranzas.Text)
                {
                    this.lkp_Proposito.EditValue = 0;
                    List<DTO_ccCreditoDocu> temp = (from c in this.creditos where c.Libranza.Value.Value.ToString() == this.lkp_Libranzas.Text select c).ToList();
                    if (temp.Count > 0)
                    {
                        this.libranzaID = this.lkp_Libranzas.Text;
                        this.credito = temp.First();
                        this.LoadCreditoInfo();
                    }
                    else
                    {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Evento que define la operacion de la pantalla de acuerdo al proposito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Proposito_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.cmbCuota.Text))
            {
                this.primeraCuota = Convert.ToInt16(this.cmbCuota.Text);
                this.txt_Altura.Text = this.primeraCuota.ToString();
                this.LoadProposito(this.primeraCuota);
            }
        }

        /// <summary>
        /// Evento al seleccionar una nueva cuota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCuota_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(this.cmbCuota.Text))
            {
                this.primeraCuota = Convert.ToInt16(this.cmbCuota.Text);
                this.txt_Altura.Text = this.primeraCuota.ToString();
                this.LoadProposito(this.primeraCuota);
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
                AnexosDocumentos anexos = new AnexosDocumentos(this.numeroDoc, ModulesPrefix.cc);
                anexos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadoCuenta.cs", "btnAnexos_Click"));
            }
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

        #endregion Eventos Grilla

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
            modalinfoCredito.lblTitle.Text = _bc.GetResource(LanguageTypes.Forms, "1028_CuotasPagadas");
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
                if (this.creditos != null && this.creditos.Count > 0)
                    this.LoadCreditoInfo();
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
                if (this.lkp_Proposito.EditValue == null)
                    return;

                byte proposito = Convert.ToByte(this.lkp_Proposito.EditValue);

                #region Validaciones
                
                //EC válido
                if (proposito == (byte)PropositoEstadoCuenta.Proyeccion)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_SelectProposito), this.lkp_Libranzas.Text);
                    MessageBox.Show(msg);
                    this.lkp_Libranzas.Focus();
                    return;
                }

                //Validacion desistimiento
                if (proposito == (byte)PropositoEstadoCuenta.Desistimiento
                    && !string.IsNullOrWhiteSpace(this.credito.CompradorCarteraID.Value)
                    && this.credito.CompradorCarteraID.Value != this.compradorCarteraPropia)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidEC_Desistimiento));
                    return;
                }

                #endregion

                if (this.infoSaldos.SaldosComponentes.Count > 0)
                {
                    this.gvDocument.PostEditor();
                    DTO_EstadoCuenta estadoCuenta = new DTO_EstadoCuenta();
                    #region Llena la informacion del estado de cuenta
                    #region Estado Cuenta Historia
                    estadoCuenta.EstadoCuentaHistoria.EC_Proposito.Value = Convert.ToByte(this.lkp_Proposito.EditValue);
                    estadoCuenta.EstadoCuentaHistoria.EC_Fecha.Value = this.dt_FechaCorte.DateTime;
                    estadoCuenta.EstadoCuentaHistoria.EC_FechaLimite.Value = this.dt_FechaLimite.DateTime;
                    estadoCuenta.EstadoCuentaHistoria.EC_Altura.Value = Convert.ToInt16(this.txt_Altura.Text);
                    estadoCuenta.EstadoCuentaHistoria.EC_CuotasMora.Value = Convert.ToInt16(this.txt_CuotasMora.Text);
                    estadoCuenta.EstadoCuentaHistoria.EC_PrimeraCtaPagada.Value = Convert.ToInt16(this.cmbCuota.Text);
                    estadoCuenta.EstadoCuentaHistoria.EC_SaldoPend.Value = Convert.ToInt32(this.txt_SaldoPendiente.EditValue);
                    estadoCuenta.EstadoCuentaHistoria.EC_SaldoMora.Value = Convert.ToInt32(this.txt_SaldoMora.EditValue);
                    estadoCuenta.EstadoCuentaHistoria.EC_SaldoTotal.Value = Convert.ToInt32(this.txt_SaldoTotal.EditValue);
                    estadoCuenta.EstadoCuentaHistoria.EC_ValorPago.Value = Convert.ToInt32(this.txt_ValorTotal.EditValue);
                    estadoCuenta.EstadoCuentaHistoria.EC_FijadoInd.Value = this.chkEstadoFijado.Checked;
                    estadoCuenta.EstadoCuentaHistoria.EC_NormalizaInd.Value = false; 
                    #endregion
                    #region Estado Cuenta Componentes
                    estadoCuenta.EstadoCuentaComponentes = this.saldosTotales;
                    #endregion
                    #endregion
                    #region Guarda la info
                    DTO_TxResult result = _bc.AdministrationModel.EstadoCuenta_Add(this._documentID, this.actFlujo.ID.Value, this.infoCreditoTemp, estadoCuenta);
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
                        //this.CleanData();
                        this.masterCliente.Focus();
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

        #endregion Eventos Barra Herramientas

    }
}