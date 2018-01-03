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
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class ReciboCaja : DocumentAuxiliarForm
    {
        //public ReciboCaja()
        //{
        //    InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer;
            this.gvDocument.RefreshData();
            if (this.resultOK)
            {
                this._facturas = new List<DTO_faFacturacionResumen>();

                this.CleanHeader(true);
                this.EnableHeader(true);

                this.masterCodigoCaja.Focus();
            }
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private string _cajaID;
        private DTO_coDocumento _coDocumento = null;
        private DTO_coPlanCuenta _cuentaDoc = null;
        private DTO_tsCaja _tsCaja = null;
        private DTO_glConceptoSaldo _concSaldoDoc = null;

        private int _compNro = 0;

        private DTO_glDocumentoControl _ctrl;
        private DTO_tsReciboCajaDocu _recibo;

        //Variables para no repetir validaciones sobre un control
        private bool _codigoCajaFocus = false;
        private bool _txtReciboNroFocus = false;
        private bool _terceroFocus = false;
        private bool _isLoadingHeader = false;
        private bool _compLoaded = false;
        private bool _headerLoaded = false;

        //Variables de facturas y simulador
        private List<DTO_faFacturacionResumen> _facturas;
        private TipoMoneda_LocExt _tipoMonedaOr;

        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_ReciboCaja TempData
        {
            get
            {
                return (DTO_ReciboCaja)this.data;
            }
            set
            {
                this.data = value;
                if (value.Comp != null)
                {
                    this.Comprobante = value.Comp;
                }
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion del header  a partir de condiciones del formulario
        /// Trae una cuenta y el respectivo concepto de saldo
        /// </summary>
        private bool LoadHeader()
        {
            int cMonedaOrigen = (int)this._tipoMonedaOr;
            UDT_BasicID udt = new UDT_BasicID();

            //Tarea la cuenta de acuerdo al documento y la moneda
            udt.Value = cMonedaOrigen == (int)TipoMoneda_LocExt.Local ? this._coDocumento.CuentaLOC.Value : this._coDocumento.CuentaEXT.Value;
            DTO_MasterBasic basic = _bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, udt, true);
            if (basic != null)
            {
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)basic;

                udt.Value = cta.ConceptoSaldoID.Value;
                basic = _bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, udt, true);
                this._concSaldoDoc = (DTO_glConceptoSaldo)basic;
                if (this._concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Cuenta)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectCtaDocContable);
                    MessageBox.Show(string.Format(msg, this._coDocumento.ID.Value));
                    
                    this.CleanHeader(false);

                    this._isLoadingHeader = true;
                    this.EnableHeader(true);
                    this._isLoadingHeader = false;

                    return false;
                }
                else if (this._coDocumento.DocumentoID.Value != this.documentID.ToString() && string.IsNullOrEmpty(this.masterBancoCuenta.Value))
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectDocContable);
                    MessageBox.Show(string.Format(msg, this._coDocumento.ID.Value));                    

                    this.CleanHeader(false);

                    this._isLoadingHeader = true;
                    this.EnableHeader(true);
                    this._isLoadingHeader = false;

                    return false;
                }
                else
                {
                    //this.EnableHeader(false);
                    this._cuentaDoc = cta;
                    this.masterCuenta_.Value = this._cuentaDoc.ID.Value;
                    this.masterCentroCosto.Value = this._tsCaja.CentroCostoID.Value;
                    this.masterProyecto.Value = this._tsCaja.ProyectoID.Value;

                    #region Asigna los valores segun la cuenta
                   
                    //Asigna el Lugar geografico
                    if (this._cuentaDoc.LugarGeograficoInd.Value.Value)
                    {
                        this.masterLugarGeo.EnableControl(true);
                        if (this._ctrl.LugarGeograficoID != null) this.masterLugarGeo.Value = this._ctrl.LugarGeograficoID.Value;
                    }
                    else
                    {
                        this.masterLugarGeo.EnableControl(false);
                        this.masterLugarGeo.Value = this.defLugarGeo;
                    }

                    #endregion

                    return true;
                }
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NocoContCta));
               
                this.CleanHeader(false);

                this._isLoadingHeader = true;
                this.EnableHeader(true);
                this._isLoadingHeader = false;

                return false;
            }
        }

        /// <summary>
        /// Funcion para validacion de fechas
        /// </summary>
        private void ValidateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFechaRec.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaRec.DateTime = new DateTime(currentYear, currentMonth, minDay);
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_ComprobanteFooter footerDet = new DTO_ComprobanteFooter();

            #region Asigna datos a la fila
            if (this.Comprobante.Footer.Count > 0)
            {
                footerDet.Index = this.Comprobante.Footer.Last().Index + 1;
                footerDet.CuentaID.Value = this.Comprobante.Footer.Last().CuentaID.Value;
                footerDet.TerceroID.Value = this.Comprobante.Footer.Last().TerceroID.Value;
                footerDet.ProyectoID.Value = this.Comprobante.Footer.Last().ProyectoID.Value;
                footerDet.CentroCostoID.Value = this.Comprobante.Footer.Last().CentroCostoID.Value;
                footerDet.LineaPresupuestoID.Value = this.Comprobante.Footer.Last().LineaPresupuestoID.Value;
                footerDet.ConceptoCargoID.Value = this.Comprobante.Footer.Last().ConceptoCargoID.Value;
                footerDet.PrefijoCOM.Value = this.Comprobante.Footer.Last().PrefijoCOM.Value;
                footerDet.DocumentoCOM.Value = this.Comprobante.Footer.Last().DocumentoCOM.Value;
                footerDet.ActivoCOM.Value = this.Comprobante.Footer.Last().ActivoCOM.Value;
                footerDet.LugarGeograficoID.Value = this.Comprobante.Footer.Last().LugarGeograficoID.Value;
                footerDet.ConceptoSaldoID.Value = this.Comprobante.Footer.Last().ConceptoSaldoID.Value;
                footerDet.Descriptivo.Value = this.Comprobante.Footer.Last().Descriptivo.Value;
                footerDet.IdentificadorTR.Value = this.Comprobante.Footer.Last().IdentificadorTR.Value;
                footerDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                footerDet.DatoAdd1.Value = this.Comprobante.Footer.Last().DatoAdd1.Value;
                footerDet.DatoAdd2.Value = this.Comprobante.Footer.Last().DatoAdd2.Value;
                footerDet.PrefDoc = this.Comprobante.Footer.Last().PrefDoc;
            }
            else
            {
                footerDet.Index = 0;
                footerDet.CuentaID.Value = string.Empty;
                footerDet.TerceroID.Value = this._ctrl.TerceroID.Value;
                footerDet.ProyectoID.Value = this._ctrl.ProyectoID.Value;
                footerDet.CentroCostoID.Value = this._ctrl.CentroCostoID.Value;
                footerDet.LineaPresupuestoID.Value = this._ctrl.LineaPresupuestoID.Value;
                footerDet.ConceptoCargoID.Value = string.Empty;
                footerDet.PrefijoCOM.Value = string.Empty;
                footerDet.DocumentoCOM.Value = string.Empty;
                footerDet.ActivoCOM.Value = string.Empty;
                footerDet.LugarGeograficoID.Value = this._ctrl.LugarGeograficoID.Value;
                footerDet.ConceptoSaldoID.Value = string.Empty;
                footerDet.Descriptivo.Value = this.txtDescrDoc.Text;
                footerDet.IdentificadorTR.Value = 0;
                footerDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                footerDet.DatoAdd1.Value = string.Empty;
                footerDet.DatoAdd2.Value = string.Empty;
                footerDet.PrefDoc = string.Empty;
            }

            footerDet.vlrBaseML.Value = 0;
            footerDet.vlrBaseME.Value = 0;
            footerDet.vlrMdaLoc.Value = 0;
            footerDet.vlrMdaExt.Value = 0;
            footerDet.vlrMdaOtr.Value = 0;
            footerDet.DatoAdd3.Value = string.Empty;
            footerDet.DatoAdd4.Value = string.Empty;
            #endregion
            #region Asigna la visibilidad de las columnas
            //Fks
            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
            //Valores
            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "TasaCambio"].OptionsColumn.AllowEdit = false;
            #endregion

            this.Comprobante.Footer.Add(footerDet);
            if (this.Comprobante.Footer.Count == 1)
            {
                this.disableValidate = true;
                this.gvDocument.RefreshData();
                this.disableValidate = false;
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                base.masterTercero.Value = footerDet.TerceroID.Value;
                base.masterPrefijo.Value = footerDet.PrefijoCOM.Value;
                base.masterCentroCosto.Value = footerDet.CentroCostoID.Value;
                base.masterProyecto.Value = footerDet.ProyectoID.Value;
                base.txtDocumento.Text = footerDet.DocumentoCOM.Value;
                base.masterLineaPre.Value = footerDet.LineaPresupuestoID.Value;
                base.masterLugarGeo.Value = footerDet.LugarGeograficoID.Value;
                base.txtDescripcion.Text = footerDet.Descriptivo.Value;            
            }
            else
            {
                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
            }


            this.isValid = false;
            this.EnableFooter(false);
            this.masterCuenta.EnableControl(true);
            if (this.masterCuenta.ValidID)
                this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this.masterCuenta.Value, true);
            this.masterConceptoCargo.EnableControl(true);
            this.masterCuenta.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ReciboCaja;
            InitializeComponent();

            this.frmModule = ModulesPrefix.ts;
            base.SetInitParameters();
            _bc.InitMasterUC(this.masterCodigoCaja, AppMasters.tsCaja, true, true, true, false);
            _bc.InitMasterUC(this.masterTercero_, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterBancoCuenta, AppMasters.tsBancosCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            _bc.InitMasterUC(this.masterCuenta_, AppMasters.coPlanCuenta, true, true, true, false);

            this._tipoMonedaOr = TipoMoneda_LocExt.Local;

            this._ctrl = new DTO_glDocumentoControl();
            this._recibo = new DTO_tsReciboCajaDocu();
            this._facturas = new List<DTO_faFacturacionResumen>();          
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected override void CleanFormat()
        {
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
            
            string f = string.Empty;
            foreach(string col in cols)
            {
                if (col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() +  "_" + "vlrMdaOtr") && 
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() +  "_" + "DatoAdd1") && 
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() +  "_" + "DatoAdd2") && 
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() +  "_" + "DatoAdd3") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd4") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambio"))
                {
                    if (col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrBaseME") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaExt"))
                    {
                        if (this.multiMoneda)
                            f += col + this.formatSeparator;
                    }
                    else
                    {
                        f += col + this.formatSeparator;
                    }
                }
            }
            
            this.format = f;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.EnableHeader(true);

            base.AfterInitialize();
            if (!this._headerLoaded)
            {                
                this.prefijoID = string.Empty;
                this.txtPrefix.Text = this.prefijoID;
                this.txtReciboNro.Text = "0";
                this.txtNumeroDoc.Text = "0";
                this.ValidateDates();
            }

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.lblTasaCambio.Visible = false;
                this.txtTasaCambio.Visible = false;
            }
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                this.txtPrefix.Text = string.Empty;
                this.dtFecha.DateTime = this.dtPeriod.DateTime;
                this.txtNumeroDoc.Text = "0";

                this.masterCodigoCaja.Value = string.Empty;
                this.txtReciboNro.Text = "0";
                this.masterBancoCuenta.Value = string.Empty;
                this.masterTercero_.Value = string.Empty;

                this._cajaID = null;
                this._tsCaja = null;
                this._coDocumento = null;
                this._cuentaDoc = null;
                this._concSaldoDoc = null;
                this._compNro = 0;
            }

            this._facturas = new List<DTO_faFacturacionResumen>();
            
            this.masterCuenta_.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;
            this.dtFechaRec.DateTime =  this.dtFecha.DateTime;
            
            this.monedaId = this.monedaLocal;
            this.masterMoneda.Value = string.Empty;
            this.txtTasaCambio.Text = string.Empty;
            this.txtValor.EditValue = "0";
            this.txtDescrDoc.Text = string.Empty;
            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.dtFecha.Enabled = enable;
            this.masterCodigoCaja.EnableControl(enable);
            this.txtReciboNro.Enabled = enable;
            this.masterBancoCuenta.EnableControl(enable);
            this.masterTercero_.EnableControl(enable);

            this.masterMoneda.EnableControl(false);
            this.masterCuenta_.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCosto.EnableControl(false);
            this.masterLugarGeo.EnableControl(false);

            this.txtValor.Enabled = enable;
            this.txtDescrDoc.Enabled = enable;

            this.dtFechaRec.Enabled = enable;
            this.btnFacturas.Enabled = enable ? false : true;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override Object LoadTempHeader()
        {
            try
            {
                #region Load DocumentoControl

                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ctrl.Observacion.Value = this.txtDescrDoc.Text;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                this._ctrl.DocumentoNro.Value = 0;
                this._ctrl.ComprobanteID.Value = this.comprobanteID;
                this._ctrl.ComprobanteIDNro.Value = this._compNro;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.PrefijoID.Value = this.prefijoID;
                this._ctrl.MonedaID.Value = this.masterMoneda.Value;
                this._ctrl.TerceroID.Value = this.masterTercero_.Value;
                this._ctrl.CuentaID.Value = this.masterCuenta_.Value;
                this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
                this._ctrl.CentroCostoID.Value = this.masterCentroCosto.Value;
                this._ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                this._ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                this._ctrl.DocumentoPadre.Value = null;
                this._ctrl.Estado.Value = (Int16)EstadoDocControl.Aprobado;
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.seUsuarioID.Value = this.userID;
                this._ctrl.ConsSaldo.Value = 0;
                this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;

                #endregion
                #region Load ReciboCajaDoc
                this._recibo.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._recibo.CajaID.Value = this.masterCodigoCaja.Value;
                this._recibo.BancoCuentaID.Value = (string.IsNullOrEmpty(this.masterBancoCuenta.Value)) ? "" : this.masterBancoCuenta.Value;
                this._recibo.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                this._recibo.IVA.Value = 0;
                this._recibo.TerceroID.Value = this.masterTercero_.Value;
                #endregion
                #region Load Comprobante
                DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                compHeader.ComprobanteID.Value = this.comprobanteID;
                compHeader.ComprobanteNro.Value = this._compNro;
                compHeader.EmpresaID.Value = this.empresaID;
                compHeader.Fecha.Value = this.dtFecha.DateTime;
                compHeader.NumeroDoc.Value = this._ctrl.NumeroDoc.Value.Value;
                compHeader.MdaOrigen.Value = Convert.ToByte(this._tipoMonedaOr);
                compHeader.MdaTransacc.Value = this.monedaId;
                compHeader.PeriodoID.Value = this.dtPeriod.DateTime;
                compHeader.TasaCambioBase.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                compHeader.TasaCambioOtr.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

                DTO_Comprobante comp = new DTO_Comprobante();
                comp.Header = compHeader;
                comp.Footer = new List<DTO_ComprobanteFooter>();

                comp.coDocumentoID = this.documentID.ToString();
                comp.PrefijoID = this.prefijoID;
                comp.DocumentoNro = Convert.ToInt32(this.txtReciboNro.Text);
                comp.CuentaID = this._cuentaDoc.ID.Value;
                comp.ProyectoID = this.masterProyecto.Value;
                comp.CentroCostoID = this.masterCentroCosto.Value;
                comp.LineaPresupuestoID = this.defLineaPresupuesto;
                comp.LugarGeograficoID = this.masterLugarGeo.Value;
                comp.Observacion = this.txtDescrDoc.Text;
                #endregion

                DTO_ReciboCaja RC = new DTO_ReciboCaja();
                RC.DocControl = this._ctrl;
                RC.ReciboCajaDoc = this._recibo;
                RC.Comp = comp;
                return RC;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "LoadTempHeader"));
                return null;
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = (int)this._tipoMonedaOr;

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
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(monOr);
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
            //Valida que ya este asignada una tasa de cambio
            if (!this.AsignarTasaCambio(false))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                return false;
            }

            //Valida datos en la maestra de documento contable
            if (!this.masterCodigoCaja.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCodigoCaja.CodeRsx);

                MessageBox.Show(msg);
                this.masterCodigoCaja.Focus();

                return false;
            }

            //Valida datos en la maestra de cuentas
            if (!this.masterCuenta_.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCuenta_.CodeRsx);

                MessageBox.Show(msg);
                this.masterCuenta_.Focus();

                return false;
            }

            //Valida datos en la maestra de terceros
            if (!this.masterTercero_.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTercero_.CodeRsx);

                MessageBox.Show(msg);
                this.masterTercero_.Focus();

                return false;
            }

            //Valida datos en la maestra de proyectos
            if (!this.masterProyecto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();

                return false;
            }

            //Valida datos en la maestra de centros de costo
            if (!this.masterCentroCosto.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroCosto.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCosto.Focus();

                return false;
            }

            //Valida datos en la maestra de lugares geograficos
            if (!this.masterLugarGeo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLugarGeo.CodeRsx);

                MessageBox.Show(msg);
                this.masterLugarGeo.Focus();

                return false;
            }

            //Valida Valor digitado por usuario
            if (string.IsNullOrWhiteSpace(this.txtValor.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_txtValor");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtValor.Focus();

                return false;
            }
            else if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) <= 0)
            {

                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_txtValor");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), txtRsx);

                MessageBox.Show(msg);
                this.txtValor.Focus();

                return false;
            }

            //Valida que la observacion tenga información
            if (string.IsNullOrWhiteSpace(this.txtDescrDoc.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtDescrDoc.Focus();

                return false;
            }
            #endregion
            #region Valida que el documento del concepto de saldo sea el mismo que el documento sobre el cual se esta trabajando
            if (this._coDocumento.DocumentoID.Value != this.documentID.ToString() && this.txtReciboNro.Text == "0" && string.IsNullOrEmpty(this.masterBancoCuenta.Value))
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectDocContable);
                MessageBox.Show(string.Format(msg, this._coDocumento.ID.Value)); 
                this.masterCodigoCaja.Focus();

                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave(int monOr)
        {
            try
            {
                decimal difLoc = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                decimal difExt = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

                decimal difValor = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);

                if (this._cuentaDoc.Naturaleza == null || !this._cuentaDoc.Naturaleza.Value.HasValue)
                    return false;

                if (monOr == (int)TipoMoneda.Local)
                {
                    if (difLoc != difValor)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotalML));
                        return false;
                    }
                }
                else
                {
                    if (difExt != difValor)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotalME));
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
            }
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            DTO_ReciboCaja RC = (DTO_ReciboCaja)aux;
            DTO_glDocumentoControl ctrl = RC.DocControl;
            DTO_tsReciboCajaDocu recibo = RC.ReciboCajaDoc;
            DTO_Comprobante comp = RC.Comp;

            DTO_ComprobanteHeader header = comp.Header;
            this.comprobanteID = header.ComprobanteID.Value;
            this._compNro = header.ComprobanteNro.Value.Value;

            if (comp.Footer == null)
                comp.Footer = new List<DTO_ComprobanteFooter>();

            bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, ctrl.PeriodoDoc.Value.Value, this.comprobanteID, this._compNro);
            if (usefulTemp || this._compNro == 0)
            {
                #region Trae los datos del formulario dado el documento contable
                //Trae la info de tsCaja
                UDT_BasicID udt = new UDT_BasicID() { Value = recibo.CajaID.Value };
                this. _tsCaja = (DTO_tsCaja)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsCaja, false, recibo.CajaID.Value, true);

                //Trae la info de coDocumento
                this._coDocumento = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._tsCaja.coDocumentoID.Value, true);

                this.prefijoID = this._coDocumento.PrefijoID.Value;
                this.txtPrefix.Text = this.prefijoID;
                #endregion
                #region Trae la cuenta y el concepto de saldo
                //Trae la cuenta
                udt.Value = ctrl.CuentaID.Value;
                this._cuentaDoc = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, udt, true);

                ////Trae el concepto de saldo
                udt.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                this._concSaldoDoc = (DTO_glConceptoSaldo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glConceptoSaldo, udt, true);
                #endregion

                this.masterCodigoCaja.Value = recibo.CajaID.Value;
                this.txtReciboNro.Text = ctrl.DocumentoNro.Value.ToString();
                this.masterBancoCuenta.Value = recibo.BancoCuentaID.Value;
                this.masterTercero_.Value = ctrl.TerceroID.Value;
                this.masterMoneda.Value = ctrl.MonedaID.Value;
                this._tipoMonedaOr = ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                
                this.masterCuenta_.Value = ctrl.CuentaID.Value;
                this.masterProyecto.Value = ctrl.ProyectoID.Value;
                this.masterCentroCosto.Value = ctrl.CentroCostoID.Value;
                this.masterLugarGeo.Value = ctrl.LugarGeograficoID.Value;

                this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
                this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();
                this.dtFechaRec.DateTime = recibo.FechaConsignacion.Value.Value;
                this.dtFecha.DateTime = ctrl.FechaDoc.Value.Value;

                this.txtValor.EditValue = recibo.Valor.Value.Value;
                this.txtDescrDoc.Text = ctrl.Observacion.Value;

                ctrl.PeriodoUltMov.Value = DateTime.Now.Date;

                comp.coDocumentoID = this.documentID.ToString();
                comp.PrefijoID = ctrl.PrefijoID.Value;
                comp.DocumentoNro = ctrl.DocumentoNro.Value.Value;
                comp.CuentaID = this.masterCuenta_.Value;
                comp.ProyectoID = this.masterProyecto.Value;
                comp.CentroCostoID = this.masterCentroCosto.Value;
                comp.LineaPresupuestoID = this.defLineaPresupuesto;
                comp.LugarGeograficoID = this.masterLugarGeo.Value;
                comp.Observacion = this.txtDescrDoc.Text;
                
                //Si se presenta un problema cargando el cabezote lo bloquea
                if (this.ValidateHeader())
                {
                    this.EnableHeader(false);

                    this.Comprobante = comp;
                    this._ctrl = RC.DocControl;
                    this._recibo = RC.ReciboCajaDoc;
                    this.data = RC;
                    this.LoadData(true);
                    this.validHeader = true;
                    this._headerLoaded = true;
                    this.gcDocument.Focus();

                    #region Carga los datos de los anticipos
                    this._facturas = new List<DTO_faFacturacionResumen>();
                    foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                    {
                        if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Factura.ToString())
                        {
                            DTO_faFacturacionResumen factRes = new DTO_faFacturacionResumen();
                            factRes.CuentaID.Value = det.CuentaID.Value;
                            factRes.TerceroID.Value = det.TerceroID.Value;
                            factRes.ProyectoID.Value = det.ProyectoID.Value;
                            factRes.CentroCostoID.Value = det.CentroCostoID.Value;
                            factRes.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                            factRes.ConceptoCargoID.Value = det.ConceptoCargoID.Value;
                            factRes.PrefijoID.Value = det.PrefijoCOM.Value;
                            factRes.ConceptoSaldoID.Value = det.ConceptoSaldoID.Value;
                            factRes.IdentificadorTR.Value = Convert.ToInt32(det.IdentificadorTR.Value);
                            factRes.ML.Value = det.vlrMdaLoc.Value;
                            factRes.ME.Value = det.vlrMdaExt.Value;

                            this._facturas.Add(factRes);
                        }

                    }
                    #endregion

                    this.btnFacturas.Enabled = true;
                    this._compLoaded = true;
                }
                else
                {
                    this.CleanHeader(true);
                }
            }
            else
            {
                this.validHeader = false;
                string rsx = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCompTemp);
                string msg = string.Format(rsx, this.comprobanteID, this._compNro, ctrl.PeriodoDoc.Value);
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// Habilita o deshabilita el footer
        /// </summary>
        /// <param name="isNew">Indica si es una fila nueva</param>
        /// <param name="rowIndex">Indice de la fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            base.LoadEditGridData(isNew, rowIndex);
            if (this.txtNumeroDoc.Text != "0" && !string.IsNullOrEmpty(this.txtNumeroDoc.Text))
            {
                this.EnableFooter(false);
                this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
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
            FormProvider.Master.itemSendtoAppr.Visible = false;
        }

        #endregion

        #region Eventos Header
       
        /// <summary>
        /// Evento que se ejecuta al entrar del codigo caja control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterCodigoCaja_Enter(object sender, EventArgs e)
        {
            this._codigoCajaFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del del codigo caja control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCodigoCaja_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._codigoCajaFocus && !this._isLoadingHeader)
                {
                    this._codigoCajaFocus = false;
                    ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                    if (master.ValidID)
                    {
                        if (string.IsNullOrWhiteSpace(this._cajaID) || master.Value != this._cajaID)
                        {
                            //if (!string.IsNullOrWhiteSpace(this._cajaID))
                            //    this.CleanHeader(true);
                            this._cajaID = master.Value;

                            //Trae el documento
                            UDT_BasicID udt = new UDT_BasicID() { Value = this.masterCodigoCaja.Value };
                            this._tsCaja = (DTO_tsCaja)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.tsCaja, udt, true);
                            this.monedaId = this._tsCaja.MonedaCaja.Value;
                            this.masterMoneda.Value = this.monedaId;
                            this._tipoMonedaOr = this.monedaId == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;  

                            //Trae el coDocumento
                            udt.Value = this._tsCaja.coDocumentoID.Value;
                            this._coDocumento = (DTO_coDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coDocumento, udt, true);
                            this.comprobanteID = this._coDocumento.ComprobanteID.Value;                            

                            //Asigna prefijo segun CajaID
                            this.prefijoID = this._coDocumento.PrefijoID.Value;
                            this.txtPrefix.Text = this.prefijoID;

                            this.LoadHeader();
                        }
                    }
                    else
                    {
                        this._cajaID = string.Empty;
                        this._coDocumento = null;
                        this._cuentaDoc = null;
                        this._tsCaja = null;
                        this._compNro = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "masterCodigoCaja_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del del codigo caja control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterBancoCuenta_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterBancoCuenta.ValidID)
                {
                    DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterBancoCuenta.Value, true);
                    this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, bancoCuenta.coDocumentoID.Value, true);
                    if(this._coDocumento != null)
                       this.LoadHeader();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "masterBancoCuenta_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReciboNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una recibo existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtReciboNro_Enter(object sender, EventArgs e)
        {
            this._txtReciboNroFocus = true;
            if (!this.masterCodigoCaja.ValidID)
            {
                this._txtReciboNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCodigoCaja.CodeRsx);

                MessageBox.Show(msg);
                this.masterCodigoCaja.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtReciboNro_Leave(object sender, EventArgs e)
        {
            if (this._txtReciboNroFocus)
            {
                this._txtReciboNroFocus = false;
                if (this.txtReciboNro.Text == string.Empty)
                    this.txtReciboNro.Text = "0";

                if (this.txtReciboNro.Text == "0")
                {
                    #region Nuevo Recibo
                    this.gcDocument.DataSource = null;
                    this.Comprobante = null;
                    this.data = null;
                    this.masterTercero_.Enabled = true;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_ReciboCaja RC = _bc.AdministrationModel.ReciboCaja_GetForLoad(this.documentID, this.prefijoID.ToString(), Convert.ToInt32(this.txtReciboNro.Text));
                        //Valida si existe
                        if (RC == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoRecibo));
                            this.txtReciboNro.Focus();
                            this.validHeader = false;
                            return;
                        }

                        //Carga los datos
                        this._ctrl = RC.DocControl;
                        this._recibo = RC.ReciboCajaDoc;
                        
                        if (this.AsignarTasaCambio(false))
                        {
                            #region Asigna los valores
                            this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
                            this.dtFechaRec.DateTime = this._recibo.FechaConsignacion.Value.Value;
                            this.masterTercero_.Value = this._recibo.TerceroID.Value;
                            this.masterBancoCuenta.Value = this._recibo.BancoCuentaID.Value ?? string.Empty;

                            //Trae la cuenta
                            if (this.LoadHeader())
                            {
                                //this.CalcularValorFacturas();

                                this.txtValor.EditValue = this._recibo.Valor.Value.Value;
                                this.txtDescrDoc.Text = this._ctrl.Observacion.Value;

                                this.Comprobante = RC.Comp;
                                if (RC.Comp != null)
                                {
                                    if (this.Comprobante.Footer != null && this.Comprobante.Footer.Count > 0)
                                        this.Comprobante.Footer.RemoveAt(this.Comprobante.Footer.Count - 1);

                                    this.Comprobante.coDocumentoID = this.documentID.ToString();
                                    this.Comprobante.PrefijoID = this._ctrl.PrefijoID.Value;
                                    this.Comprobante.DocumentoNro = this._ctrl.DocumentoNro.Value.Value;
                                    this.Comprobante.CuentaID = this.masterCuenta_.Value;
                                    this.Comprobante.ProyectoID = this.masterProyecto.Value;
                                    this.Comprobante.CentroCostoID = this.masterCentroCosto.Value;
                                    this.Comprobante.LineaPresupuestoID = this.defLineaPresupuesto;
                                    this.Comprobante.LugarGeograficoID = this.masterLugarGeo.Value;
                                    this.Comprobante.Observacion = this.txtDescrDoc.Text;

                                    this._compNro = this.Comprobante.Header.ComprobanteNro.Value.Value;
                                    this._compLoaded = true;

                                    this.LoadData(true);
                                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                                        this.CambiarSignoValor();

                                    this.gcDocument.Focus();
                                }
                                else
                                {
                                    this.Comprobante = new DTO_Comprobante();
                                    this._compLoaded = false;
                                }

                                this.LoadData(true);
                                this.data = RC;
                            }
                            else
                                this.validHeader = false;
                            #endregion
                        }
                        else
                            this.validHeader = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "txtReciboNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado un codigo caja existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTercero__Enter(object sender, EventArgs e)
        {
            if (this.txtReciboNro.Text == "0")
            {
                this._terceroFocus = true;
                if (!this.masterCodigoCaja.ValidID)
                {
                    this._terceroFocus = false;
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCodigoCaja.CodeRsx);

                    MessageBox.Show(msg);
                    this.masterCodigoCaja.Focus();
                }
            }
            else 
                this.txtReciboNro.Focus(); 
        }

        /// <summary>
        /// Usuario puede ingresar tercero si el recibo es nuevo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTercero__Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._terceroFocus)
                {
                    this._terceroFocus = false;
                    if (this.masterTercero_.ValidID)
                    {
                        this.txtReciboNro.Enabled = false;
                        this.masterCodigoCaja.EnableControl(false);
                        if (this.AsignarTasaCambio(false))
                        {
                            this.Comprobante = new DTO_Comprobante();
                            this._compLoaded = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Evento que abre la grilla de facturas
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnFacturas_Click(object sender, EventArgs e)
        {           
            try
            {
                if (this.Comprobante != null && this.Comprobante.Footer.Count > 0)
                {
                    if (!ValidateRow(this.gvDocument.FocusedRowHandle))
                        return;
                }
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                TipoMoneda tm = (TipoMoneda)this._tipoMonedaOr;
                ModalFacturacion fact = new ModalFacturacion(this._facturas, this.dtPeriod.DateTime, tm, tc, this.masterTercero_.Value);
                fact.ShowDialog();
                if (fact.ReturnVals)
                {
                    #region Carga la nueva lista de facturas
                    List<DTO_faFacturacionResumen> emptyList = new List<DTO_faFacturacionResumen>();
                    fact.ReturnList.ForEach(retList =>
                    {
                        bool found = false;
                        this._facturas.ForEach(currFact =>
                        {
                            if
                            (
                                !found &&
                                retList.CuentaID.Value == currFact.CuentaID.Value &&
                                retList.TerceroID.Value == currFact.TerceroID.Value &&
                                retList.ProyectoID.Value == currFact.ProyectoID.Value &&
                                retList.CentroCostoID.Value == currFact.CentroCostoID.Value &&
                                retList.LineaPresupuestoID.Value == currFact.LineaPresupuestoID.Value &&
                                retList.ConceptoSaldoID.Value == currFact.ConceptoSaldoID.Value &&
                                retList.ConceptoCargoID.Value == currFact.ConceptoCargoID.Value &&
                                retList.IdentificadorTR.Value == currFact.IdentificadorTR.Value
                            )
                            {
                                //currAnt.MaxVal.Value = retList.MaxVal.Value;
                                emptyList.Add(currFact);
                                found = true;
                            }
                        });
                        if (!found)
                            emptyList.Add(retList);
                    });

                    this._facturas = emptyList;
                    #endregion
                    #region Elimina las facturas existentes
                    bool isFact = false;
                    int index = 0;
                    int removeCount = 0;
                    foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                    {
                        if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Factura.ToString())
                        {
                            isFact = true;
                            removeCount++;
                        }
                        if (!isFact)
                            index++;
                    }
                    this.Comprobante.Footer.RemoveRange(index, removeCount);
                    #endregion
                    #region Agrega los registros de las facturas
                    index = this.Comprobante.Footer.Count;
                    this._facturas.ForEach(newFact =>
                    {
                        DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
                        newDet.Index = index;
                        newDet.CuentaID.Value = newFact.CuentaID.Value;
                        newDet.TerceroID.Value = newFact.TerceroID.Value;
                        newDet.ProyectoID.Value = newFact.ProyectoID.Value;
                        newDet.CentroCostoID.Value = newFact.CentroCostoID.Value;
                        newDet.LineaPresupuestoID.Value = newFact.LineaPresupuestoID.Value;
                        newDet.ConceptoCargoID.Value = newFact.ConceptoCargoID.Value;
                        newDet.LugarGeograficoID.Value = this.defLugarGeo;
                        newDet.PrefijoCOM.Value = newFact.PrefijoID.Value;
                        newDet.DocumentoCOM.Value = newFact.DocumentoNro.Value.Value.ToString();
                        newDet.ActivoCOM.Value = string.Empty;
                        newDet.ConceptoSaldoID.Value = newFact.ConceptoSaldoID.Value;
                        newDet.IdentificadorTR.Value = newFact.IdentificadorTR.Value;
                        newDet.Descriptivo.Value = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_FacturacionAbona);
                        newDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                        newDet.vlrBaseML.Value = 0;
                        newDet.vlrBaseME.Value = 0;
                        newDet.vlrMdaLoc.Value = newFact.ML.Value;
                        newDet.vlrMdaExt.Value = newFact.ME.Value;
                        newDet.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? newDet.vlrMdaLoc.Value : newDet.vlrMdaExt.Value;
                        //newDet.DatoAdd3.Value = newFact.MaxVal.Value.Value.ToString();
                        newDet.DatoAdd4.Value = AuxiliarDatoAdd4.Factura.ToString();

                        this.Comprobante.Footer.Add(newDet);
                        index++;
                    });

                    #endregion
                    #region Actualiza la grilla
                    this.deleteOP = true;
                    this.LoadData(true);
                    this.UpdateTemp(this.data);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                
                if (this._compNro == 0)
                    FormProvider.Master.itemPrint.Enabled = false;

                if (this.txtNumeroDoc.Text != "0" && !string.IsNullOrEmpty(this.txtNumeroDoc.Text))
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemPaste.Enabled = false;
                    FormProvider.Master.itemImport.Enabled = false;
                    //this.masterCuenta.EnableControl(false);
                }
          
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_ReciboCaja rc = (DTO_ReciboCaja)this.LoadTempHeader();
                        DTO_Comprobante comp = rc.Comp;

                        if (this._compLoaded)
                            comp.Footer = this.Comprobante.Footer;

                        rc.Comp = comp;
                        this.TempData = rc;

                        if (this.Comprobante.Footer.Count == 0)
                        {
                            FormProvider.Master.itemPrint.Enabled = false;
                            FormProvider.Master.itemSave.Enabled = false;
                            this.LoadData(true);
                        }

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion
            }
            else
            {
                this.masterCodigoCaja.Focus();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this.txtNumeroDoc.Text == "0" || string.IsNullOrEmpty(this.txtNumeroDoc.Text))
            {
                base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
                if (!this.validHeader)
                    this.masterCodigoCaja.Focus();
            }
            else
                e.Handled = true;
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
                    this.data = new DTO_ReciboCaja();
                    this.Comprobante = new DTO_Comprobante();
                    this._facturas = new List<DTO_faFacturacionResumen>();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;

                    this.btnFacturas.Enabled = false;

                    this.masterCodigoCaja.Focus();
                    
                    this._compLoaded = false;
                    this._headerLoaded = false;
                    this._cajaID = string.Empty;
                    this._compNro = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "TBNew" + ex.Message));
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

                int monOr = (int)this._tipoMonedaOr;
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();
                if (this.ValidGrid() && this.CanSave(monOr))
                {
                    //Si es cta debito cambia los valores
                    decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture) *-1;
                    decimal ME = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture) *-1;
                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                    {
                        this.CambiarSignoValor();
                        ML *= -1;
                        ME *= -1;
                    }

                    #region Crea el ultimo registro
                    DTO_ComprobanteFooter last = new DTO_ComprobanteFooter();
                    last.Index = this.Comprobante.Footer.Count;
                    last.CuentaID.Value = this._cuentaDoc.ID.Value;
                    last.TerceroID.Value = this.masterTercero_.Value;
                    last.ProyectoID.Value = this.masterProyecto.Value;
                    last.CentroCostoID.Value = this.masterCentroCosto.Value;
                    last.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    last.ConceptoCargoID.Value = string.IsNullOrEmpty(this._cuentaDoc.ConceptoCargoID.Value) ? this.defConceptoCargo : this._cuentaDoc.ConceptoCargoID.Value;
                    last.LugarGeograficoID.Value = this.masterLugarGeo.Value;
                    last.PrefijoCOM.Value = this.prefijoID;
                    last.DocumentoCOM.Value = this.txtReciboNro.Text;
                    last.ActivoCOM.Value = string.Empty;
                    last.ConceptoSaldoID.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                    last.IdentificadorTR.Value = 0;
                    last.Descriptivo.Value = this.txtDescrDoc.Text;
                    last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                    decimal vlrDoc = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                    last.vlrBaseML.Value = 0;
                    last.vlrBaseME.Value = 0;
                    last.vlrMdaLoc.Value = ML;
                    last.vlrMdaExt.Value = ME;
                    last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                    last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    this.Comprobante.Footer.Add(last);

                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "TBSave" + ex.Message));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            base.TBDelete();
            try
            {
                if (this.ValidGrid())
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgDelDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Document);

                    if (MessageBox.Show(msgDelDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.gvDocument.ActiveFilterString = string.Empty;
                        if (this._compNro == 0)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                            return;
                        }
                        _bc.AdministrationModel.ComprobantePre_Delete(this.documentID, this._actFlujo.ID.Value, this.dtPeriod.DateTime, this.comprobanteID, this._compNro);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CancelledComp));

                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        this.data = new DTO_ReciboCaja();
                        this.Comprobante = new DTO_Comprobante();

                        this.gvDocument.ActiveFilterString = string.Empty;
                        this.disableValidate = true;
                        this.gcDocument.DataSource = this.Comprobante.Footer;
                        this.disableValidate = false;

                        this._compLoaded = false;
                        this._headerLoaded = false;

                        this.newDoc = true;
                        this.deleteOP = true;
                        this.CleanHeader(true);
                        this.EnableHeader(true);
                        this.masterCodigoCaja.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "TBDelete" + ex.Message));
            }
        }
        
        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBPaste()
        {
            try
            {
                base.TBPaste();
                this._facturas = new List<DTO_faFacturacionResumen>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "TBPaste" + ex.Message));
            }
        }

        /// <summary>
        /// Boton para importir datos
        /// </summary>
        public override void TBImport()
        {
            try
            {
                base.TBImport();
                FormProvider.Master.itemSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReciboCaja.cs", "TBImport" + ex.Message));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;

                this.TempData.DocControl.Descripcion.Value = this.Comprobante.Footer.First().Descriptivo.Value;
                result = _bc.AdministrationModel.ReciboCaja_Guardar(this.documentID, this._actFlujo.ID.Value, this.TempData.DocControl, this.TempData.ReciboCajaDoc, this.Comprobante, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                
                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (result.Result.Equals(ResultValue.OK))
                {
                    this.resultOK = true;
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    //Genera el reporte
                    string reportName = this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numeroDoc);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, null);
                    Process.Start(fileURl);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_CuentaXPagar();
                    this.Comprobante = new DTO_Comprobante();
                    this._compLoaded = false;
                    this._headerLoaded = false;
                    this.resultOK = true;
                                    
                }
                else
                {
                    this.resultOK = false;
                    
                    //Remueve el ultimo registro
                    this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());//At(this.Comprobante.Footer.Count - 1);

                    // Cambia los valores de signos segun la cuenta
                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                        this.CambiarSignoValor();
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    base.ImportThread();
                    this._facturas = new List<DTO_faFacturacionResumen>();
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
