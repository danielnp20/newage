using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using SentenceTransformer;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Facturas de Venta
    /// </summary>
    public partial class FacturaVenta : DocumentFacturaForm
    {
        public FacturaVenta()
        {
            //InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            base.SaveMethod();

            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtFacturaNro.Enabled = true;
            this.masterCliente.EnableControl(true);
            if(this.ctrl != null && this.ctrl.DocumentoNro.Value != null)
            {
                this.txtFacturaNro.Text = this.ctrl.DocumentoNro.Value.ToString();
                this.txtNumeroDoc.Text = this.ctrl.NumeroDoc.Value.ToString();
            }               
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            base.SendToApproveMethod();

            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtFacturaNro.Enabled = true;
            this.masterCliente.EnableControl(true);
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        private DTO_coTercero _tercero = null;
        private string _coDocumentoID;
        private string _facturaTipoID;
        private bool _prefijoFocus = false;
        private bool _txtFacturaNroFocus = false;
        private bool _clientFocus = false;
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion del header  a partir de condiciones del formulario
        /// Trae una cuenta y el respectivo concepto de saldo
        /// </summary>
        private bool GetCuenta()
        {
            try
            {
                #region Tarea la cuenta de acuerdo al documento y la moneda
                int cMonedaOrigen = (int)this.tipoMonedaOr;
                string cta = cMonedaOrigen == (int)TipoMoneda_LocExt.Local ? this.coDocumento.CuentaLOC.Value : this.coDocumento.CuentaEXT.Value;
                DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, cta, true);
                if (dtoCuenta != null)
                {
                    this.concSaldoDoc = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, dtoCuenta.ConceptoSaldoID.Value, true);
                    if (this.concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Interno)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCuentaTipoFact));

                        this.CleanHeader(false);

                        this.isLoadingHeader = true;
                        this.EnableHeader(false);
                        this.masterPrefijo.EnableControl(true);
                        this.txtFacturaNro.Enabled = true;
                        this.masterCliente.EnableControl(true);
                        this.masterCliente.Focus();
                        this.isLoadingHeader = false;

                        return false;
                    }
                    else if (this.coDocumento.DocumentoID.Value != this.documentID.ToString())
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidDocFact));

                        this.CleanHeader(false);

                        this.isLoadingHeader = true;
                        this.EnableHeader(false);
                        this.masterPrefijo.EnableControl(true);
                        this.txtFacturaNro.Enabled = true;
                        this.masterCliente.EnableControl(true);
                        this.masterCliente.Focus();
                        this.isLoadingHeader = false;

                        return false;
                    }
                    else
                    {
                        this.cuentaDoc = dtoCuenta;
                        if (this.ctrl != null && string.IsNullOrEmpty(this.ctrl.CentroCostoID.Value))
                            this.masterCentroCosto.Value = this.defCentroCosto;
                        if (this.ctrl != null && string.IsNullOrEmpty(this.ctrl.ProyectoID.Value))
                            this.masterProyecto.Value = this.defProyecto;

                        #region Habilita o deshabilita los campos segun la cuenta

                        //// Habilita o deshabilita el lugar geografico
                        //if (this.cuentaDoc.LugarGeograficoInd.Value.Value)
                        //    this.masterLugarGeo.EnableControl(true);
                        //else
                        //    this.masterLugarGeo.EnableControl(false);
                        //// Habilita o deshabilita el centro de costo
                        //if (this.cuentaDoc.CentroCostoInd.Value.Value)
                        //    this.masterCentroCosto.EnableControl(true);
                        //else
                        //    this.masterCentroCosto.EnableControl(false);

                        //// Habilita o deshabilita el proyecto
                        //if (this.cuentaDoc.ProyectoInd.Value.Value)
                        //    this.masterProyecto.EnableControl(true);
                        //else
                        //    this.masterProyecto.EnableControl(false);
                        #endregion

                        return true;
                    }
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NocoContCta));

                    this.CleanHeader(false);

                    this.isLoadingHeader = true;
                    this.EnableHeader(false);
                    this.masterPrefijo.EnableControl(true);
                    this.txtFacturaNro.Enabled = true;
                    this.masterCliente.EnableControl(true);
                    //this.masterCliente.Focus();
                    this.isLoadingHeader = false;

                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetCuenta"));
                return false;
            }
        }

        /// <summary>
        /// Funcion para validacion de fechas
        /// </summary>
        private void ValidateDates()
        {
            try
            {
                int currentMonth = this.dtPeriod.DateTime.Month;
                int currentYear = this.dtPeriod.DateTime.Year;
                int minDay = 1;
                int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                this.dtFechaFact.Properties.MinValue = new DateTime(this.periodoFact.Year, this.periodoFact.Month, this.periodoFact.Day);
                DateTime maxfecha = this.dtFechaFact.Properties.MinValue.AddMonths(1);
                this.dtFechaFact.Properties.MaxValue = new DateTime(maxfecha.Year, maxfecha.Month, DateTime.DaysInMonth(maxfecha.Year, maxfecha.Month));

                //this.dtFechaVto.Properties.MinValue = this.dtFechaFact.DateTime;
                this.dtFechaVto.DateTime = new DateTime(currentYear, currentMonth, minDay);

                this.dtFechaProntoPago.Properties.MinValue = this.dtFechaFact.DateTime;
                this.dtFechaProntoPago.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                this.dtFechaProntoPago.DateTime = new DateTime(currentYear, currentMonth, minDay);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "ValidateDates"));
            }
        }

        /// <summary>
        /// Obtiene el saldo del tercero para Anticipos
        /// </summary>
        /// <returns></returns>
        private decimal GetSaldoAnticipo()
        {
            try
            {
                decimal saldoAntic = 0;
                string cuentaAntic = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.fa, AppControl.fa_CuentaAnticiposMdaLocal);
                string libroFunc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false);

                DTO_coCuentaSaldo saldoFilter = new DTO_coCuentaSaldo();
                saldoFilter.PeriodoID.Value = this.dtPeriod.DateTime;
                saldoFilter.BalanceTipoID.Value = libroFunc;
                saldoFilter.CuentaID.Value = cuentaAntic;
                saldoFilter.TerceroID.Value = cliente.TerceroID.Value;
                if (!string.IsNullOrEmpty(saldoFilter.CuentaID.Value))
                {
                    if (modules.Any(x => x.ModuloID.Value == ModulesPrefix.py.ToString()) && this.data != null)
                        saldoFilter.ProyectoID.Value = this.data.DocCtrl.ProyectoID.Value;
                    List<DTO_coCuentaSaldo> saldosList = this._bc.AdministrationModel.Saldos_GetByParameter(saldoFilter);
                    saldoAntic = Math.Abs(saldosList.Sum(x => x.DbOrigenLocML.Value.Value + x.DbOrigenExtML.Value.Value + x.CrOrigenLocML.Value.Value +
                        x.CrOrigenExtML.Value.Value + x.DbSaldoIniLocML.Value.Value + x.DbSaldoIniExtML.Value.Value + x.CrSaldoIniLocML.Value.Value + x.CrSaldoIniExtML.Value.Value));
                    
                }
                return saldoAntic;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetSaldoAnticipo"));
                return 0;
            }
        }

        /// <summary>
        /// Obtiene la cuenta de ICA
        /// </summary>
        /// <returns></returns>
        private string GetCuentaICA()
        {
            try
            {
                if (Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture) > 0)
                {
                    string impuestoICADef = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.fa, AppControl.fa_CodigoICA);
                    decimal porICA = Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture) / 10;

                    //Trae cuentas con el Impuesto ICA
                    DTO_glConsulta consulta = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    DTO_glConsultaFiltro filtro;

                    filtro = new DTO_glConsultaFiltro();
                    filtro.CampoFisico = "ImpuestoTipoID";
                    filtro.ValorFiltro = impuestoICADef;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtros.Add(filtro);

                    consulta.Filtros = filtros;
                    long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.coPlanCuenta, consulta, null, true);
                    List<DTO_coPlanCuenta> listCuentas = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coPlanCuenta, count, 1, consulta, null, true).Cast<DTO_coPlanCuenta>().ToList();

                    //Obtiene la cuenta que tenga el impuesto digitado
                    DTO_coPlanCuenta cta = listCuentas.Find(x => x.ImpuestoPorc.Value == porICA);

                    if (cta != null)
                    {
                        //Asigna la cuenta
                        base._cuentaRteICA = cta.ID.Value;
                    }
                    else
                    {
                        MessageBox.Show("No existe ninguna cuenta de Impuesto ICA con ese porcentaje");
                        this.txtPorcICA.EditValue = "0";
                        base._cuentaRteICA = string.Empty;
                    }
                }
                return _cuentaRteICA;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetCuentaICA"));
                return string.Empty;
            }
        }  

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            try
            {
                validRow = base.ValidateRow(fila);

                if (validRow)
                {
                    #region Valida que la suma de los netos no supere al maximo permitido en valorFondo ni inferior a 0
                    if (Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture) != 0)
                    {
                        GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                        string noCuentaIca = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoCuentaIca);

                        if (this.cuentaIcaNoExiste)
                        {
                            this.gvDocument.SetColumnError(col, noCuentaIca);
                            validRow = false;
                        }
                    }
                    #endregion
                }

                if (validRow)
                {
                    this.isValid = true;
                    this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.FacturaVenta;
            this.frmModule = ModulesPrefix.fa;
            InitializeComponent();

            base.SetInitParameters();

            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, false, true, false);
            this._bc.InitMasterUC(this.masterFacturaTipo, AppMasters.faFacturaTipo, true, true, true, false);
            this._bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            this._bc.InitMasterUC(this.masterAsesor, AppMasters.faAsesor, true, true, true, false);
            this._bc.InitMasterUC(this.masterMonedaFact, AppMasters.glMoneda, false, true, true, false);
            this._bc.InitMasterUC(this.masterMonedaPago, AppMasters.glMoneda, false, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
             this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            try
            {
                base.AfterInitialize();

                this.EnableHeader(false);
                this.dtFechaFact.Properties.MinValue = new DateTime(this.periodoFact.Year, this.periodoFact.Month, this.periodoFact.Day);
                DateTime maxfecha = this.dtFechaFact.Properties.MinValue.AddMonths(1);
                this.dtFechaFact.Properties.MaxValue = new DateTime(maxfecha.Year, maxfecha.Month, DateTime.DaysInMonth(maxfecha.Year, maxfecha.Month));
                this.dtFechaFact.DateTime = DateTime.Now;
                if (!headerLoaded)
                {   
                    this.txtNumeroDoc.Text = "0";
                    this.ValidateDates();
                    this.masterPrefijo.EnableControl(true);
                    this.masterCliente.EnableControl(true);
                    this.txtFacturaNro.Enabled = true;
                    if (string.IsNullOrEmpty(this.txtFacturaNro.Text)) this.txtFacturaNro.Text = "0";
                }              
                this.tlSeparatorPanel.RowStyles[0].Height = 45;
                this.tlSeparatorPanel.RowStyles[1].Height = 62;
                this.tlSeparatorPanel.RowStyles[2].Height = 198;

                this.gvDocument.OptionsView.ShowAutoFilterRow = true;
                this.masterPrefijo.Value = this.defPrefijo;
                this.masterPrefijo.Focus();
            }
            catch (Exception ex)
            {                
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "AfterInitialize"));
            }
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            //
            if (basic)
            {
                this.dtPeriod.DateTime = base.periodoFact;
                this.prefijoID = string.Empty;
                this.txtPrefix.Text = this.prefijoID;
                this.txtNumeroDoc.Text = "0";
                this.facturaNro = 0;
                this.masterPrefijo.Value = string.Empty;
                this.txtFacturaNro.Text = "0";
            }            

            this.coDocumento = new DTO_coDocumento();
            this.concSaldoDoc = new DTO_glConceptoSaldo();
            this.cuentaDoc = new DTO_coPlanCuenta();
            this.cliente = new DTO_faCliente();
            this.facturaTipo = new DTO_faFacturaTipo();
            this._tercero = new DTO_coTercero();
            
            this.masterCliente.Value = string.Empty;
            this.txtDescCliente.Text = string.Empty;
            this.masterFacturaTipo.Value = string.Empty;
            this.masterAsesor.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;
            this.masterMonedaFact.Value = string.Empty;
            this.masterMonedaPago.Value = string.Empty;
            this.masterZona.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;

            this.ValidateDates();
            this.txtTasaCambio.EditValue = "0";
            this.txtFormaPago.Text = string.Empty;
            this.txtDescrDoc.Text = string.Empty;
            this.txtPorcICA.EditValue = "0";
            this.txtPorcReteGarantia.EditValue = "0";
            this.txtValorProntoPago.EditValue = "0";
            this.txtValorAnticipos.EditValue = "0";
            this.monedaId = this.monedaLocal;

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.dtFecha.Enabled = false;
            this.dtPeriod.Enabled = false;
            this.txtPrefix.Enabled = false;
            this.txtPrefix.Enabled = false; 
            this.txtNumeroDoc.Enabled = false;

            this.masterPrefijo.EnableControl(enable);
            this.masterCliente.EnableControl(enable);
            this.masterFacturaTipo.EnableControl(enable);            
            this.masterMonedaFact.EnableControl(enable);

            this.txtFacturaNro.Enabled = enable;
            this.txtPorcICA.Enabled = (!this.indContabilizaRetencionFactura) ? false : enable;
            //this.btnQueryDoc.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_faFacturacion LoadTempHeader()
        {
            try
            {
                #region Load DocumentoControl
                if (this.ctrl.NumeroDoc.Value == 0 || this.ctrl.NumeroDoc.Value == null)
                {
                    this.ctrl.EmpresaID.Value = this.empresaID;
                    this.ctrl.TerceroID.Value = this.cliente.TerceroID.Value;
                    this.ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                    this.ctrl.ComprobanteID.Value = this.comprobanteID;
                    this.ctrl.ComprobanteIDNro.Value = 0;
                    this.ctrl.MonedaID.Value = this.masterMonedaFact.Value;
                    if (this.cuentaDoc != null)
                        this.ctrl.CuentaID.Value = this.cuentaDoc.ID.Value;
                    else
                    {
                        this.GetCuenta();
                        this.ctrl.CuentaID.Value = this.cuentaDoc.ID.Value;
                    }
                    this.ctrl.ProyectoID.Value = this.masterProyecto.Value;
                    this.ctrl.CentroCostoID.Value = this.masterCentroCosto.Value;
                    this.ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                    this.ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    this.ctrl.Fecha.Value = DateTime.Now;
                    this.ctrl.FechaDoc.Value = this.dtFechaFact.DateTime;
                    this.ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                    this.ctrl.PrefijoID.Value = this.masterPrefijo.Value;
                    this.ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    this.ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    this.ctrl.DocumentoNro.Value = Convert.ToInt32(txtFacturaNro.Text);
                    this.ctrl.DocumentoID.Value = this.documentID;
                    this.ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this.ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                    this.ctrl.seUsuarioID.Value = this.userID;
                    this.ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this.ctrl.ConsSaldo.Value = 0;
                    this.ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    this.ctrl.Observacion.Value = this.txtDescrDoc.Text;
                    this.ctrl.Descripcion.Value = base.txtDocDesc.Text;
                }
                else
                {
                    this.ctrl.ProyectoID.Value = this.masterProyecto.Value;
                    this.ctrl.CentroCostoID.Value = this.masterCentroCosto.Value;
                    this.ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                    this.ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    this.ctrl.Fecha.Value = DateTime.Now;
                    this.ctrl.FechaDoc.Value = this.dtFechaFact.DateTime;
                    this.ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                    this.ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    this.ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                    this.ctrl.Observacion.Value = this.txtDescrDoc.Text;
                    this.ctrl.Descripcion.Value = base.txtDocDesc.Text;
                }
                #endregion
                #region Load FacturaHeader
                this.factHeader.EmpresaID.Value = this.empresaID;
                this.factHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this.factHeader.AsesorID.Value = this.masterAsesor.Value;
                this.factHeader.FacturaTipoID.Value = this.masterFacturaTipo.Value;
                this.factHeader.DocumentoREL.Value = 0;
                this.factHeader.FacturaREL.Value = Convert.ToInt32(this.txtFacturaNro.Text); //
                this.factHeader.MonedaPago.Value = this.masterMonedaPago.Value;
                this.factHeader.ClienteID.Value = this.masterCliente.Value;
                this.factHeader.ListaPrecioID.Value = this.cliente.ListaPrecioID.Value;
                this.factHeader.ZonaID.Value = this.masterZona.Value;
                this.factHeader.TasaPago.Value = 1; // 0-FechaPago ; 1-Fecha Factura
                this.factHeader.FechaVto.Value = this.dtFechaVto.DateTime;
                //this.factHeader.ObservacionENC.Value = this.rtbEncabezado.Text;
                this.factHeader.FormaPago.Value = this.txtFormaPago.Text;
                if (!base.IVAUtilidad)
                {
                    this.factHeader.Valor.Value = 0;
                    this.factHeader.Iva.Value = 0;                 
                }
                this.factHeader.Bruto.Value = 0;
                this.factHeader.PorcPtoPago.Value = Convert.ToDecimal(this.txtPorcProntoPago.EditValue, CultureInfo.InvariantCulture);
                this.factHeader.PorcRteGarantia.Value = Convert.ToDecimal(this.txtPorcReteGarantia.EditValue, CultureInfo.InvariantCulture);
                this.factHeader.FechaPtoPago.Value = this.dtFechaProntoPago.DateTime;
                this.factHeader.ValorPtoPago.Value = Convert.ToDecimal(this.txtValorProntoPago.EditValue, CultureInfo.InvariantCulture);
                this.factHeader.Porcentaje1.Value = Convert.ToDecimal(this.txtPorcICA.EditValue, CultureInfo.InvariantCulture);
                this.factHeader.FacturaFijaInd.Value = false;
                this.factHeader.RteGarantiaIvaInd.Value = this.chkRteGarantiaInd.Checked;

                this.factHeader.PorcRteGarantia.Value = Convert.ToDecimal(this.txtPorcReteGarantia.EditValue, CultureInfo.InvariantCulture);
                if (this.data != null && this.factHeader.RteGarantiaIvaInd.Value.Value)
                    this.factHeader.ValorRteGarantia = this.data != null && this.data.Footer.Count > 0 ? Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto + x.ValorIVA) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0) : 0;
                else
                    this.factHeader.ValorRteGarantia = this.data != null && this.data.Footer.Count > 0 ? Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0) : 0;

                if (this.data != null && this.data.Header.FacturaTipoID.Value.Equals(this._tipoFacturaCtaCobro))
                    this.factHeader.ValorRteGarantia = 0;
                base.txtValorRteGarantia.EditValue = this.factHeader.ValorRteGarantia;

                //Anticipos
                this.factHeader.Retencion10.Value = this.factHeader.Retencion10.Value ?? 0;
                this.factHeader.ValorAnticipo = this.factHeader.Retencion10.Value.Value;
                base.txtValorAnticipoDet.EditValue =  this.factHeader.ValorAnticipo;
                #endregion

                this.tipoMonedaOr = this.ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                this.monedaId = this.ctrl.MonedaID.Value;

                DTO_faFacturacion fact = new DTO_faFacturacion();
                fact.Header = this.factHeader;
                fact.DocCtrl = this.ctrl;
                if (this.gvDocument.DataRowCount > 0)
                    fact.Footer = this.data.Footer;
                else
                    fact.Footer = new List<DTO_faFacturacionFooter>();
                this.factFooter = fact.Footer;

                return fact;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "LoadTempHeader"));
                return null;
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = (int)this.tipoMonedaOr;

            //Asigna monedaId
            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            //Si la empresa no permite mmultimoneda
            if (!this.multiMoneda)
            {
                this.txtTasaCambio.EditValue = 0;
            }
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(monOr, this.dtFechaFact.DateTime);
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (tc == 0)
                {
                    this.validHeader = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                    return false;
                }
            }

            if (!fromTop)
                this.validHeader = true;
            else
                this.validHeader = false;

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            #region Valida los datos obligatorios

            #region Valida que ya este asignada una tasa de cambio
            if (!this.AsignarTasaCambio(false))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                return false;
            }
            #endregion

            #region Valida datos en la maestra de Prefijo
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Cliente
            if (!this.masterCliente.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.CodeRsx);

                MessageBox.Show(msg);
                this.masterCliente.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de FacturaTipo
            if (!this.masterFacturaTipo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterFacturaTipo.CodeRsx);

                MessageBox.Show(msg);
                this.masterFacturaTipo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Lugar Geografico
            if (!this.masterLugarGeo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarGeo.CodeRsx);

                MessageBox.Show(msg);
                this.masterLugarGeo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra del proyecto
            if (!this.masterProyecto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra del centro costo
            if (!this.masterCentroCosto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroCosto.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCosto.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Asesor
            if (!this.masterAsesor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAsesor.CodeRsx);

                MessageBox.Show(msg);
                this.masterAsesor.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Moneda
            if (!this.masterMonedaFact.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaFact.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaFact.Focus();

                return false;
            }

            if (!this.masterMonedaPago.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaPago.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaPago.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Zona
            if (!this.masterZona.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterZona.CodeRsx);

                MessageBox.Show(msg);
                this.masterZona.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la fecha de la factura
            if (string.IsNullOrEmpty(this.dtFechaFact.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaFact");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaFact.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la fecha de vencimiento
            if (string.IsNullOrEmpty(this.dtFechaVto.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaVto");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaVto.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la forma de pago
            if (string.IsNullOrEmpty(this.txtFormaPago.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFormaPago");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.txtFormaPago.Focus();
                return false;
            }
            #endregion

            #region Valida datos en el descriptivo
            if (string.IsNullOrEmpty(this.txtDescrDoc.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.txtDescrDoc.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la fecha de pronto pago
            if (string.IsNullOrEmpty(this.dtFechaProntoPago.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaProntoPagot");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaProntoPago.Focus();
                return false;
            }
            #endregion

            #region Valida datos en el Porcentaje de pronto pago
            if (string.IsNullOrEmpty(this.txtPorcProntoPago.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblPorcProntoPago");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.txtPorcProntoPago.Focus();
                return false;
            }
            #endregion

            #region Valida datos en el valor de pronto pago
            if (string.IsNullOrEmpty(this.txtValorProntoPago.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblValorProntoPago");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.txtValorProntoPago.Focus();
                return false;
            }
            #endregion

            #endregion
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(DTO_faFacturacion fact)
        {
            try
            {
                DTO_glDocumentoControl ctrl = fact.DocCtrl;
                DTO_faFacturaDocu factHeader = fact.Header;

                if (fact.Footer == null)
                    fact.Footer = new List<DTO_faFacturacionFooter>();
                this.factFooter = fact.Footer;

                #region Trae los datos del formulario dado la factura
                this.facturaTipo = (DTO_faFacturaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, fact.Header.FacturaTipoID.Value, true);

                this.coDocumento = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this.facturaTipo.coDocumentoID.Value, true);
                this.prefijoID = ctrl.PrefijoID.Value;
                this.txtPrefix.Text = this.prefijoID;
                base.comprobanteID = this.coDocumento != null ? this.coDocumento.ComprobanteID.Value : string.Empty;
                #endregion

                this.masterPrefijo.Value = ctrl.PrefijoID.Value;
                this.masterCliente.Value = factHeader.ClienteID.Value;
                this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                this.masterFacturaTipo.Value = factHeader.FacturaTipoID.Value;
                this.txtDescCliente.Text = this.cliente.Descriptivo.Value;

                this.masterFacturaTipo.Value = this.cliente.FacturaTipoID.Value;  
                this.masterAsesor.Value = factHeader.AsesorID.Value;
                this.masterLugarGeo.Value = ctrl.LugarGeograficoID.Value;
                this.masterMonedaFact.Value = ctrl.MonedaID.Value;
                this.masterMonedaPago.Value = factHeader.MonedaPago.Value;
                this.masterZona.Value = factHeader.ZonaID.Value;
                this.masterProyecto.Value = base.GetAIUProyecto(ctrl.ProyectoID.Value);
                this.masterCentroCosto.Value = ctrl.CentroCostoID.Value;

                this.chkRteGarantiaInd.Checked = factHeader.RteGarantiaIvaInd.Value.Value;
                this.txtFacturaNro.Text = ctrl.DocumentoNro.Value.ToString();
                this.dtFechaFact.DateTime = ctrl.FechaDoc.Value.Value;
                this.dtFechaVto.DateTime = factHeader.FechaVto.Value.Value;
                this.txtTasaCambio.EditValue = ctrl.TasaCambioCONT.Value;
                this.txtFormaPago.Text = factHeader.FormaPago.Value;
                this.txtDescrDoc.Text = ctrl.Observacion.Value;
                //this.rtbEncabezado.Text = factHeader.ObservacionENC.Value;
                this.txtPorcICA.EditValue = factHeader.Porcentaje1.Value.Value;
                this._cuentaRteICA = this.GetCuentaICA();
                factHeader.CuentaRteICA = this._cuentaRteICA;
                this.dtFechaProntoPago.DateTime = factHeader.FechaPtoPago.Value.Value;
                this.txtPorcProntoPago.EditValue = factHeader.PorcPtoPago.Value;
                this.txtPorcReteGarantia.EditValue = factHeader.PorcRteGarantia.Value;
                this.txtValorProntoPago.EditValue = factHeader.ValorPtoPago.Value;
                base.txtValorAnticipoDet.EditValue = factHeader.Retencion10.Value;
                factHeader.ValorAnticipo = Convert.ToDecimal(base.txtValorAnticipoDet.EditValue,CultureInfo.InvariantCulture);

                this.monedaId = ctrl.MonedaID.Value;
                this.tipoMonedaOr = this.monedaId == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();

                //Si se presenta un problema asignando la tasa de cambio lo bloquea
                if (this.ValidateHeader())
                {
                    this.EnableHeader(false);
                    this.data = fact;
                    this.ctrl = fact.DocCtrl;
                    this.factHeader = fact.Header;
                    this.factFooter = fact.Footer;

                    this.validHeader = true;
                    this.headerLoaded = true;

                    this.LoadData(true);
                    this.gcDocument.Focus();

                    if (this.ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado && this.ctrl.Estado.Value != (byte)EstadoDocControl.ParaAprobacion)
                    {
                        this.masterFacturaTipo.EnableControl(true);
                        this.masterCliente.EnableControl(true);
                        this.dtFechaFact.Enabled = true;
                        this.dtFechaVto.Enabled = true;
                        this.dtFechaProntoPago.Enabled = true;
                    }                   
                }
                else
                    this.CleanHeader(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "LoadTempData"));
            }
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected override void CalcularTotal()
        {
            try
            {
                base.CalcularTotal();

                if (this.data != null && this.data.Footer.Count != 0)
                {
                    this.data.Header.Valor.Value = this.data.Footer.Sum(x => x.ValorBruto);
                    this.data.Header.Iva.Value = !this.IVAUtilidad? this.data.Footer.Sum(x => x.ValorIVA) : this.data.Header.Iva.Value;
                    this.data.Header.ValorPtoPago.Value = this.data.Footer.Sum(x => x.ValorNeto) * Math.Round(this.data.Header.PorcPtoPago.Value.Value / 100,0);
                    this.data.Header.Retencion1.Value = this.data.Footer.Sum(x => x.ValorRIVA);
                    this.data.Header.Retencion2.Value = this.data.Footer.Sum(x => x.ValorRFT);
                    this.data.Header.Retencion3.Value = this.data.Footer.Sum(x => x.ValorRICA);
                    this.data.Header.Retencion4.Value = this.data.Header.ValorRteGarantia;
                    this.data.Header.Retencion10.Value = this.data.Header.ValorAnticipo;

                    this.txtValorProntoPago.EditValue = this.data.Header.ValorPtoPago.Value;
                    this.txtPorcICA.EditValue = this.data.Header.Porcentaje1.Value;
                    this.txtPorcReteGarantia.EditValue = this.data.Header.PorcRteGarantia.Value;
                    this.txtValorAnticipos.EditValue = base._vlrAnticipoTerc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "CalcularTotal"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Enter(object sender, EventArgs e)
        {
            if (this._clientFocus) this._clientFocus = false;
            this._prefijoFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._prefijoFocus)
                {
                    this._prefijoFocus = false;
                    if (this.masterPrefijo.ValidID)
                    {
                        this.prefijoID = this.masterPrefijo.Value;
                        this.txtPrefix.Text = this.prefijoID;
                    }
                    else
                        CleanHeader(true);  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterPrefijo_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFacturaNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFacturaNro_Enter(object sender, EventArgs e)
        {
            if (this._clientFocus) this._clientFocus = false;
            this._txtFacturaNroFocus = true;
            if (!this.masterPrefijo.ValidID)
            {
                this._txtFacturaNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFacturaNro_Leave(object sender, EventArgs e)
        {
            if (this._txtFacturaNroFocus)
            {
                _txtFacturaNroFocus = false;
                if (this.txtFacturaNro.Text == string.Empty)
                    this.txtFacturaNro.Text = "0";

                if (this.txtFacturaNro.Text == "0")
                {
                    #region Nueva Factura
                    this.gcDocument.DataSource = null;
                    //this.Comprobante = null;
                    this.data = null;
                    this.newDoc = true;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_faFacturacion Fact = _bc.AdministrationModel.FacturaVenta_Load(this.documentID, this.masterPrefijo.Value, Convert.ToInt32(this.txtFacturaNro.Text));
                        //Valida si existe
                        if (Fact == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoFacturas));
                            //this.txtFacturaNro.Focus();
                            this.validHeader = false;
                            return;
                        }

                        //Valida el estado
                        if (Fact.DocCtrl.Estado.Value.Value != (byte)EstadoDocControl.ParaAprobacion && Fact.DocCtrl.Estado.Value.Value != (byte)EstadoDocControl.SinAprobar)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoRadicado));
                            FormProvider.Master.itemSendtoAppr.Enabled = false;
                            FormProvider.Master.itemSave.Enabled = false;
                            //return;
                        }
                        else
                        {
                            this.newDoc = false;
                            FormProvider.Master.itemSendtoAppr.Enabled = true;
                        }                       

                        //Carga los datos
                        this.ctrl = Fact.DocCtrl;
                        this.factHeader = Fact.Header;

                        if (this.AsignarTasaCambio(false))
                        {
                            if (Fact.Footer != null)
                                this.factFooter = Fact.Footer;
                            else
                                this.factFooter = new List<DTO_faFacturacionFooter>();

                            this.data = Fact;

                            #region Asigna los valores
                            this.txtNumeroDoc.Text = this.ctrl.NumeroDoc.Value.Value.ToString();
                            this.masterPrefijo.Value = this.ctrl.PrefijoID.Value;
                            this.masterCliente.Value = this.factHeader.ClienteID.Value;
                            this.masterFacturaTipo.Value = this.factHeader.FacturaTipoID.Value; 
                            this.masterAsesor.Value = this.factHeader.AsesorID.Value;
                            this.masterLugarGeo.Value = this.ctrl.LugarGeograficoID.Value;
                            this.masterMonedaFact.Value = this.ctrl.MonedaID.Value;
                            this.masterMonedaPago.Value = this.factHeader.MonedaPago.Value;
                            this.masterZona.Value = this.factHeader.ZonaID.Value;
                            this.masterProyecto.Value = base.GetAIUProyecto(this.ctrl.ProyectoID.Value);
                            this.masterCentroCosto.Value = this.ctrl.CentroCostoID.Value;

                            this.chkRteGarantiaInd.Checked = this.factHeader.RteGarantiaIvaInd.Value.Value;
                            this.txtFacturaNro.Text = this.ctrl.DocumentoNro.Value.ToString();
                            this.dtFechaFact.DateTime = this.ctrl.FechaDoc.Value.Value;
                            this.dtFechaVto.DateTime = this.factHeader.FechaVto.Value.Value;
                            this.txtTasaCambio.EditValue = this.ctrl.TasaCambioCONT.Value;
                            this.txtFormaPago.Text = this.factHeader.FormaPago.Value;
                            this.txtDescrDoc.Text = this.ctrl.Observacion.Value;
                            //this.rtbEncabezado.Text = this.factHeader.ObservacionENC.Value;
                            this.txtPorcICA.EditValue = this.factHeader.Porcentaje1.Value;
                            this._cuentaRteICA = this.GetCuentaICA();
                            this.factHeader.CuentaRteICA = this._cuentaRteICA;
                            this.dtFechaProntoPago.DateTime = !string.IsNullOrEmpty(this.factHeader.FechaPtoPago.ToString())? this.factHeader.FechaPtoPago.Value.Value : this.dtFechaFact.DateTime;
                            this.txtPorcProntoPago.EditValue = !string.IsNullOrEmpty(this.factHeader.PorcPtoPago.ToString())? this.factHeader.PorcPtoPago.Value : 0;
                            this.txtPorcReteGarantia.EditValue = !string.IsNullOrEmpty(this.factHeader.PorcRteGarantia.ToString()) ? this.factHeader.PorcRteGarantia.Value : 0;
                            this.txtValorProntoPago.EditValue = !string.IsNullOrEmpty(this.factHeader.ValorPtoPago.ToString())? this.factHeader.ValorPtoPago.Value.Value : 0;
                            this.tipoMonedaOr = this.ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                            this.monedaId = this.ctrl.MonedaID.Value;

                            //Anticipos
                            this.factHeader.Retencion10.Value = this.factHeader.Retencion10.Value ?? 0;
                            this.factHeader.ValorAnticipo = this.factHeader.Retencion10.Value.Value;
                            base.txtValorAnticipoDet.EditValue = this.factHeader.ValorAnticipo;

                            this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);
                            this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                            this._tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.cliente.TerceroID.Value, true);
                            this.coDocumento =  this.facturaTipo != null ?(DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this.facturaTipo.coDocumentoID.Value, true) : null;
                            base.comprobanteID = this.coDocumento != null ? this.coDocumento.ComprobanteID.Value : string.Empty;

                            this.prefijoID = ctrl.PrefijoID.Value;
                            this.txtPrefix.Text = this.prefijoID;                           
                            this.txtDescCliente.Text = this.cliente.Descriptivo.Value;

                            this.headerLoaded = true;                            
                       
                            this.LoadData(true);
                            this.GetCuenta();
                            #endregion
                        }
                        else
                            this.validHeader = false;
                           
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "txtFacturaNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el cliente control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCliente_Enter(object sender, EventArgs e)
        {
            this._clientFocus = true;
            //if (this.data != null && this.data.Footer.Count > 0)
            //    this.gcDocument.Focus();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterCliente control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._clientFocus)
                {
                    this._clientFocus = false;
                    if (this.masterCliente.ValidID)
                    {
                        this._clientFocus = false;
                        this.txtFacturaNro.Enabled = false;
                        this.newDoc = true;
                        if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                        {
                            //Trae informacion sobre el cliente
                            this.cliente = (DTO_faCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);

                            //Informacion sobre las monedas por defecto
                            this.masterMonedaFact.Value = this.monedaLocal;
                            this.masterMonedaPago.Value = this.monedaLocal;
                            this.monedaId = this.masterMonedaFact.Value;
                            this.tipoMonedaOr = TipoMoneda_LocExt.Local;

                            //Trae info del cliente                           
                            this.masterZona.Value = this.cliente.ZonaID.Value;
                            this.txtDescCliente.Text = this.cliente.Descriptivo.Value;
                            this._tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.cliente.TerceroID.Value, true);
                            this.masterLugarGeo.Value = this._tercero.LugarGeograficoID.Value;
                            this.dtFechaProntoPago.DateTime = !string.IsNullOrEmpty(this.cliente.DiasPtoPago.Value.ToString())?  this.dtFechaFact.DateTime.AddDays(this.cliente.DiasPtoPago.Value.Value) : this.dtFecha.DateTime;
                            this.txtPorcProntoPago.EditValue = !string.IsNullOrEmpty(this.cliente.PorcPtoPago.Value.ToString()) ? this.cliente.PorcPtoPago.Value.Value : 0;
                            this.masterFacturaTipo.EnableControl(true);                      
                            this._vlrAnticipoTerc = this.GetSaldoAnticipo();
                            this.txtValorAnticipos.EditValue = this._vlrAnticipoTerc;
                            base.txtValorAnticipoDet.EditValue = this._vlrAnticipoTerc;
                        }
                        //POne el tercero en cada registro del detalle
                        if (this.data != null)
                            this.data.Footer.ForEach(x => x.Movimiento.TerceroID.Value = this.cliente.TerceroID.Value);
                    }
                    else if (!string.IsNullOrEmpty(this.masterCliente.Value) || !string.IsNullOrWhiteSpace(this.masterCliente.Value))
                        this.masterCliente.Focus();
                    else 
                        this.txtFacturaNro.Focus();

                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void MasterControl_Enter(object sender, EventArgs e)
        {
            if (this._clientFocus || !this.masterCliente.ValidID)
                this.masterCliente.Focus();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterFacturaTipo control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterFacturaTipo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterFacturaTipo.ValidID)
                {
                    // Trae informacion sobre el tipo de la factura
                    this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);
                    this._facturaTipoID = this.masterFacturaTipo.Value;

                    if (this._coDocumentoID == null || this._coDocumentoID != this.facturaTipo.coDocumentoID.Value)
                    {
                        //Trae informacion sobre el coDocumento
                        this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this.facturaTipo.coDocumentoID.Value, true);
                        if (this.GetCuenta())
                        {
                            base.comprobanteID = this.coDocumento != null ? this.coDocumento.ComprobanteID.Value : string.Empty;
                            this._coDocumentoID = this.coDocumento.ID.Value;
                        }
                        else
                        {
                            this.coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._coDocumentoID, true);
                            this.masterFacturaTipo.Value = this._facturaTipoID;

                            //Trae informacion sobre el tipo de la factura
                            this.facturaTipo = (DTO_faFacturaTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.masterFacturaTipo.Value, true);
                        }
                    }

                    //Trae la observacion por defecto
                    this.txtDescrDoc.Text = this.facturaTipo.ObservacionENC.Value;
                    this.EnableHeader(true);
                }
                else
                    if (this.masterFacturaTipo.Enabled == true) this.masterFacturaTipo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterFacturaTipo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterLugarGeo control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterLugarGeo_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (!this.masterLugarGeo.ValidID)
                //    this.masterLugarGeo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterLugarGeo_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del  control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                this._vlrAnticipoTerc = this.GetSaldoAnticipo();
                this.txtValorAnticipos.EditValue = this._vlrAnticipoTerc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterProyecto_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del masterCliente control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterMonedaFact_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterMonedaFact.ValidID)
                {
                    if (this.monedaId != this.masterMonedaFact.Value)
                    {
                        this.tipoMonedaOr = this.masterMonedaFact.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                        //Trae informacion sobre la Cuenta
                        if (this.GetCuenta())
                            this.monedaId = this.masterMonedaFact.Value;
                        else
                        {
                            this.masterMonedaFact.Value = this.monedaId;
                            this.tipoMonedaOr = this.masterMonedaFact.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                        }
                    }
                }
                else
                    this.masterMonedaFact.Value = this.monedaId;
                //this.masterMonedaFact.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "masterMonedaFact_Leave"));
            }
        }

        /// <summary>
        /// valida la edición de las fechas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechas_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.periodoFact.Month != this.dtFechaFact.DateTime.Month)
                {
                                    
                }
                base.dtPeriod.DateTime = new DateTime(this.dtFechaFact.DateTime.Year, this.dtFechaFact.DateTime.Month, 1);  
                base.dtFecha.DateTime = new DateTime(this.dtFechaFact.DateTime.Year, this.dtFechaFact.DateTime.Month, this.dtFechaFact.DateTime.Day);

                if (this.dtFechaVto.DateTime < this.dtFechaFact.DateTime)
                    this.dtFechaVto.DateTime = this.dtFechaFact.DateTime;

                if (this.dtFechaProntoPago.DateTime < this.dtFechaFact.DateTime)
                    this.dtFechaProntoPago.DateTime = this.dtFechaFact.DateTime;
            }
            catch (Exception)
            { ; }
        }
        
        /// <summary>
        /// Clic para consultar documentos relacionados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.FacturaVenta);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtFacturaNro.Enabled = true;
                this.txtFacturaNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.prefijoID = this.masterPrefijo.Value;
                this.txtFacturaNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Clic para consultar documentos relacionados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtPorcReteGarantia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ( this.data != null && !this.data.Header.FacturaTipoID.Value.Equals(base._tipoFacturaCtaCobro))
                {
                    this.data.Header.PorcRteGarantia.Value = Convert.ToDecimal(this.txtPorcReteGarantia.EditValue, CultureInfo.InvariantCulture);
                    this.data.Header.PorcRteGarantia.Value = this.data.Header.PorcRteGarantia.Value ?? 0;
                    if (this.data != null && this.chkRteGarantiaInd.Checked)
                        this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto + x.ValorIVA) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                    else
                        this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                    base.txtValorRteGarantia.EditValue = this.data.Header.ValorRteGarantia;
                    base.CalcularTotal(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "txtPorcReteGarantia_EditValueChanged"));
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtPorcICA_Leave(object sender, EventArgs e)
        {
            try
            {                
                 this.GetCuentaICA();
            }
            catch (Exception ex)
            {
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "txtPorcICA_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorAnticipoDet_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Permite cambiar el calculo del valor indicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRteGarantiaInd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.data != null && this.factHeader != null)
                {
                    this.data.Header.RteGarantiaIvaInd.Value = this.chkRteGarantiaInd.Checked;
                    this.factHeader.RteGarantiaIvaInd.Value = this.chkRteGarantiaInd.Checked;

                    //this.data.Header.PorcRteGarantia.Value = Convert.ToDecimal(this.txtPorcReteGarantia.EditValue, CultureInfo.InvariantCulture);
                    this.data.Header.PorcRteGarantia.Value = this.data.Header.PorcRteGarantia.Value ?? 0;
                    if (this.data != null && this.chkRteGarantiaInd.Checked)
                        this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto + x.ValorIVA) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                    else
                        this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                    base.txtValorRteGarantia.EditValue = this.data.Header.ValorRteGarantia;
                    base.CalcularTotal(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "chkRteGarantiaInd_CheckedChanged"));
            }
        }

        #endregion      

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();
                if (this.txtNumeroDoc.Text == "0")
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
                else
                {
                    if (this.ctrl != null && this.ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;
                    }
                    else
                        FormProvider.Master.itemSendtoAppr.Enabled = true;
                       
                    FormProvider.Master.itemExport.Enabled = true;
                    FormProvider.Master.itemPrint.Enabled = true;
                }

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if (!this.headerLoaded)
                    {
                        DTO_faFacturacion fact = this.LoadTempHeader();
                        this.factHeader = fact.Header;
                        this.factFooter = fact.Footer;
                        this.TempData = fact;
                                               
                        this.LoadData(true);

                        this.UpdateTemp(this.data);
                        this.headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion
                #region Si ya tiene datos cargados
                if (!this.dataLoaded)
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                #endregion

                if (this.ctrl != null && this.ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado && this.ctrl.Estado.Value != (byte)EstadoDocControl.ParaAprobacion)
                {
                    this.masterFacturaTipo.EnableControl(true);
                    this.masterCliente.EnableControl(true);
                    this.dtFechaFact.Enabled = true;
                    this.dtFechaVto.Enabled = true;
                    this.dtFechaProntoPago.Enabled = true;
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
                if (this.data.Header.DocumentoREL == null || this.data.Header.DocumentoREL.Value.Value == 0)
                    base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);

                if (!this.validHeader)
                    this.masterCliente.Focus();

                if (this.txtNumeroDoc.Text != "0")
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
                else
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = true;
                    FormProvider.Master.itemPrint.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();
                if (this.cleanDoc)
                {
                    this.data = new DTO_faFacturacion();
                    this.ctrl = new DTO_glDocumentoControl();
                    this.factHeader = new DTO_faFacturaDocu();
                    this.factFooter = new List<DTO_faFacturacionFooter>();                    
                    this._coDocumentoID = string.Empty;                    
                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;

                    this._prefijoFocus = false;
                    this._txtFacturaNroFocus = false;
                    this._clientFocus = false;
                    this.CleanHeader(true);
                    this.EnableHeader(false);
                    this.masterPrefijo.EnableControl(true);
                    this.masterPrefijo.Value = this.defPrefijo;
                    this.masterPrefijo.Focus();
                    this.txtFacturaNro.Enabled = true;
                    this.masterCliente.EnableControl(true);
                    this.btnQueryDoc.Enabled = true;
                    this.headerLoaded = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();

                this.TempData = this.LoadTempHeader();
                this.UpdateTemp(this.TempData);

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid() && this.ValidateHeader())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "TBSave"));
            }
        }

        #endregion            
    }
}
