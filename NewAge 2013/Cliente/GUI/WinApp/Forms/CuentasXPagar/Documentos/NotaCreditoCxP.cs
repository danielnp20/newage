﻿using System;
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
using DevExpress.Data;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class NotaCreditoCxP : DocumentAuxiliarForm
    {
        //public NotaCreditoCxP()
        //{
        //    InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            if (this.TempData.DocControl.NumeroDoc.Value.HasValue)
                this.txtNumeroDoc.Text = this.TempData.DocControl.NumeroDoc.Value.Value.ToString();
            
            this.gcDocument.DataSource = this.Comprobante.Footer;
            this.gvDocument.RefreshData();
            if (this.resultOK)
            { 
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer;
            FormProvider.Master.itemSendtoAppr.Enabled = false;

            this.CleanHeader(true);
            this.EnableHeader(false);

            this.masterTercero_.EnableControl(true);
            this.btnAnticipo.Enabled = false;
            this.btnLiquida.Enabled = false;
            //this.btnResumenImp.Enabled = false;

            this.txtFact.Enabled = true;
            this.masterConceptoCXP.Focus();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private string _terceroID;
        private DTO_cpConceptoCXP _cpConcepto = null;
        private DTO_coDocumento _coDocumento = null;
        private DTO_coPlanCuenta _cuentaDoc = null;
        private DTO_glConceptoSaldo _concSaldoDoc = null;
        private int _compNro = 0;
        private string _nDias;
        private bool _radicaObliga = true;

        private DTO_glDocumentoControl _ctrl;
        private DTO_cpCuentaXPagar _cxp;

        //Variables para no repetir validaciones sobre un control
        private bool _terceroFocus;
        private bool _txtFactFocus;
        private bool _compLoaded = false;
        private bool _headerLoaded = false;
        //Variables de anticipos y simulador
        private List<DTO_AnticiposResumen> _anticipos;

        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_CuentaXPagar TempData
        {
            get
            {
                return (DTO_CuentaXPagar)this.data;
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
            try
            {
                this.prefijoID = this._coDocumento.PrefijoID.Value;
                this.txtPrefix.Text = this.prefijoID;
                
                int cMonedaOrigen = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
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

                    if (this._concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Externo)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocExt));
                        this.masterConceptoCXP.Focus();

                        return false;
                    }
                    //else if (this._coDocumento.DocumentoID.Value != this.documentID.ToString())
                    //{
                    //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocID));
                    //    this.masterConceptoCXP.Focus();

                    //    return false;
                    //}
                    else
                    {
                        this._cuentaDoc = cta;
                        this.masterCuenta_.Value = this._cuentaDoc.ID.Value;

                        #region Asigna los valores segun la cuenta

                        //Asigna el centro de costo
                        if (this._cuentaDoc.CentroCostoInd.Value.Value)
                        {
                            this.masterCentroCosto.EnableControl(true);
                            this.masterCentroCosto.Value = this._ctrl.CentroCostoID.Value;
                        }
                        else
                        {
                            this.masterCentroCosto.EnableControl(false);
                            this.masterCentroCosto.Value = this.defCentroCosto;
                        }

                        //Asigna el proyecto
                        if (this._cuentaDoc.ProyectoInd.Value.Value)
                        {
                            this.masterProyecto.EnableControl(true);
                            this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
                        }
                        else
                        {
                            this.masterProyecto.EnableControl(false);
                            this.masterProyecto.Value = this.defProyecto;
                        }

                        //Asigna el Lugar geografico
                        if (this._cuentaDoc.LugarGeograficoInd.Value.Value)
                        {
                            this.masterLugarGeo.EnableControl(true);
                            this.masterLugarGeo.Value = this._ctrl.LugarGeograficoID.Value;
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
                    this.masterConceptoCXP.Focus();

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "LoadHeader"));
                return false;
            }
        }

        /// <summary>
        /// Funcion que calcula el valor total de los anticipos pendientes
        /// </summary>
        private void CalcularValorAnticipos()
        {
            decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            decimal antVal = _bc.AdministrationModel.cpAnticipos_GetResumenVal(this.dtPeriod.DateTime, this.tipoMoneda, tc, this.masterTercero_.Value);

            this.txtAnticipos.EditValue = antVal;
        }

        /// <summary>
        /// Asigna las fechas
        /// </summary>
        private void AsignarFechas()
        {
            try
            {
                this.dtFechaFactura.EditValueChanged -= new System.EventHandler(this.dtFechaFactura_EditValueChanged);
                if (this.chkProvisiones.Checked)
                {
                    DateTime provisionDate = this.dtPeriod.DateTime.AddMonths(-1);
                    int provisionMonth = provisionDate.Month;
                    int provisionYear = provisionDate.Year;
                    int provisionLastDay = DateTime.DaysInMonth(provisionYear, provisionMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(provisionYear, provisionMonth, 1);
                    this.dtFecha.Properties.MaxValue = new DateTime(provisionYear, provisionMonth, provisionLastDay);
                    this.dtFecha.DateTime = new DateTime(provisionYear, provisionMonth, 1);

                    this.dtFechaFactura.Properties.MinValue = new DateTime(provisionYear, provisionMonth, 1);
                    this.dtFechaFactura.Properties.MaxValue = new DateTime(provisionYear, provisionMonth, provisionLastDay);
                    this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
                }
                else
                {
                    int currentMonth = this.dtPeriod.DateTime.Month;
                    int currentYear = this.dtPeriod.DateTime.Year;
                    int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, 1);
                    this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, 1);

                    this.dtFechaFactura.Properties.MinValue = new DateTime(currentYear, currentMonth, 1);
                    this.dtFechaFactura.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
                }

                this.dtFechaFactura.EditValueChanged += new System.EventHandler(this.dtFechaFactura_EditValueChanged);

                this.dtFechaVencimiento.Properties.MinValue = this.dtFecha.DateTime;
                this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "AsignarFechas"));
            }
        }

        #endregion

        #region Funciones Virtuales

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
                //footerDet.CuentaID.Value = this._ctrl.CuentaID.Value;
                footerDet.TerceroID.Value = this._ctrl.TerceroID.Value;
                footerDet.ProyectoID.Value = this._ctrl.ProyectoID.Value;
                footerDet.CentroCostoID.Value = this._ctrl.CentroCostoID.Value;
                footerDet.LineaPresupuestoID.Value = this._ctrl.LineaPresupuestoID.Value;
                footerDet.PrefijoCOM.Value = this.defPrefijo;
                footerDet.DocumentoCOM.Value = this._ctrl.DocumentoTercero.Value;
                footerDet.ActivoCOM.Value = string.Empty;
                footerDet.LugarGeograficoID.Value = this._ctrl.LugarGeograficoID.Value;
                footerDet.Descriptivo.Value = this.txtDescrDoc.Text;
                footerDet.IdentificadorTR.Value = 0;
                footerDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                footerDet.DatoAdd1.Value = string.Empty;
                footerDet.DatoAdd2.Value = string.Empty;
                footerDet.PrefDoc = footerDet.PrefijoCOM.Value + " - " + footerDet.DocumentoCOM.Value; ;
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
                base.txtDocumento.Text = footerDet.DocumentoCOM.Value;
                base.txtDescripcion.Text = footerDet.Descriptivo.Value;
            }
            else
            {
                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
            }

            this.isValid = false;
            this.EnableFooter(false);
            base.masterCuenta.EnableControl(true);
            if (base.masterCuenta.ValidID)
                this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, base.masterCuenta.Value, true);

            base.masterCuenta.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.NotaCreditoCxP;
            InitializeComponent();

            this.frmModule = ModulesPrefix.cp;
            base.SetInitParameters();
            _bc.InitMasterUC(this.masterTercero_, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterConceptoCXP, AppMasters.cpConceptoCXP, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterCuenta_, AppMasters.coPlanCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            this.masterConceptoCXP.EnableControl(false);
            this.masterCuenta_.EnableControl(false);
            this.masterMoneda.EnableControl(false);

            this._anticipos = new List<DTO_AnticiposResumen>();
            this.tipoMoneda = TipoMoneda.Local;
            this._nDias = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DiasPlazoPagoFact);
            this.dtFecha.Enabled = false;

            string _radicaObligaStr = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_IndRadicacionObligatoria);
            this._radicaObliga = _radicaObligaStr == "0" ? false : true;

            this._ctrl = new DTO_glDocumentoControl();
            this._cxp = new DTO_cpCuentaXPagar();

            //Llena los combos
            TablesResources.GetTableResources(this.cmbMonedaOrigen, typeof(TipoMoneda_LocExt));
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.EnableHeader(false);
            this.masterTercero_.EnableControl(true);
            this.txtFact.Enabled = true;
            this.btnAnticipo.Enabled = false;
            this.btnLiquida.Enabled = false;
            
            this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;

            base.AfterInitialize();

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
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                this.dtPeriod.Text = periodo;

                this.masterConceptoCXP.Value = string.Empty;
                this.txtFact.Text = string.Empty;

                this._terceroID = string.Empty;
                this._cpConcepto = null;
                this._coDocumento = null;
                this._cuentaDoc = null;
                this._concSaldoDoc = null;
                this._compNro = 0;
            }

            this._anticipos = new List<DTO_AnticiposResumen>();
            
            this.masterCuenta_.Value = string.Empty;
            this.masterTercero_.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;

            this.txtFact.Text = string.Empty;
            this.monedaId = this.monedaLocal;
            this.cmbMonedaOrigen.SelectedIndex = 0;
            this.masterMoneda.Value = this.monedaId;
            this.txtTasaCambio.Text = "0";
            this.txtAnticipos.EditValue = 0;
            this.txtValorBruto.EditValue = 0;
            this.txtIVA.EditValue = 0;
            this.txtDescrDoc.Text = string.Empty;

            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;
            this.chkProvisiones.Checked = false;

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterTercero_.EnableControl(enable);
            this.txtFact.Enabled = enable;

            this.masterProyecto.EnableControl(enable);
            this.masterCentroCosto.EnableControl(enable);
            this.masterLugarGeo.EnableControl(enable);
            this.masterConceptoCXP.EnableControl(enable);

            this.txtIVA.Enabled = enable;
            this.txtValorBruto.Enabled = enable;
            this.txtDescrDoc.Enabled = enable;

            this.dtFechaFactura.Enabled = enable;
            this.dtFechaVencimiento.Enabled = enable;

            this.btnAnticipo.Enabled = enable ? false : true;
            this.btnLiquida.Enabled = enable ? false : true;

            if (enable || this.chkProvisiones.Checked)
                this.btnAnticipo.Enabled = false;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override Object LoadTempHeader()
        {
            int numDoc = Convert.ToInt32(this.txtNumeroDoc.Text);
            #region Nueva CxP
            if (numDoc == 0)
            {
                #region Doc Ctrl
                this._ctrl = new DTO_glDocumentoControl();
                this._ctrl.DocumentoID.Value = AppDocuments.NotaCreditoCxP;
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._ctrl.DocumentoNro.Value = 0;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.PrefijoID.Value = this.txtPrefix.Text;
                this._ctrl.Observacion.Value = "CAUSACIÓN NOTA CREDITO CXP";
                this._ctrl.Descripcion.Value = this.txtDescrDoc.Text;
                this._ctrl.MonedaID.Value = this.monedaId;

                this._ctrl.TasaCambioCONT.Value = this.multiMoneda ? _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFecha.DateTime) : 0;
                this._ctrl.TasaCambioDOCU.Value = this._ctrl.TasaCambioCONT.Value;

                this._ctrl.Estado.Value = (byte)EstadoDocControl.Radicado;
                this._ctrl.seUsuarioID.Value = this.userID;
                #endregion
                #region CxP
                this._cxp = new DTO_cpCuentaXPagar();
                this._cxp.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
                this._cxp.MonedaPago.Value = this.monedaId;
                this._cxp.DistribuyeImpLocalInd.Value = false;
                #endregion
            }
            else
                this._ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(numDoc);
            #endregion

            #region Documento ctrl;
            this._ctrl.TerceroID.Value = this.masterTercero_.Value;
            this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._ctrl.DocumentoTercero.Value = this.txtFact.Text;
            this._ctrl.MonedaID.Value = this.masterMoneda.Value;
            this._ctrl.CuentaID.Value = this.masterCuenta_.Value;
            this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
            this._ctrl.CentroCostoID.Value = this.masterCentroCosto.Value;
            this._ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
            this._ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
            this._ctrl.Descripcion.Value = this.txtDescrDoc.Text;
            this._ctrl.Fecha.Value = this.dtFechaFactura.DateTime;
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValorBruto.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.Iva.Value = Convert.ToDecimal(this.txtIVA.EditValue, CultureInfo.InvariantCulture);
            #endregion
            #region CxP
            this._cxp.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._cxp.Valor.Value = Convert.ToDecimal(this.txtValorBruto.EditValue, CultureInfo.InvariantCulture);
            this._cxp.IVA.Value = Convert.ToDecimal(this.txtIVA.EditValue, CultureInfo.InvariantCulture);
            this._cxp.FacturaFecha.Value = this.dtFechaFactura.DateTime;
            this._cxp.VtoFecha.Value = this.dtFechaVencimiento.DateTime;
            this._cxp.ConceptoCxPID.Value = this.masterConceptoCXP.Value;
            this._cxp.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
            #endregion
            #region Comprobante
            DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
            compHeader.ComprobanteID.Value = this.comprobanteID;
            compHeader.ComprobanteNro.Value = this._compNro;
            compHeader.EmpresaID.Value = this.empresaID;
            compHeader.Fecha.Value = this.dtFecha.DateTime;
            compHeader.NumeroDoc.Value = this._ctrl.NumeroDoc.Value.Value;
            compHeader.MdaOrigen.Value = Convert.ToByte((((ComboBoxItem)(this.cmbMonedaOrigen.SelectedItem)).Value));
            compHeader.MdaTransacc.Value = this.monedaId;
            compHeader.PeriodoID.Value = this.dtPeriod.DateTime;
            compHeader.TasaCambioBase.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            compHeader.TasaCambioOtr.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

            DTO_Comprobante comp = new DTO_Comprobante();
            comp.Header = compHeader;
            comp.Footer = new List<DTO_ComprobanteFooter>();
            comp.CuentaID = this._cuentaDoc.ID.Value;
            comp.ProyectoID = this.masterProyecto.Value;
            comp.CentroCostoID = this.masterCentroCosto.Value;
            comp.LineaPresupuestoID = this.defLineaPresupuesto;
            comp.LugarGeograficoID = this.masterLugarGeo.Value;
            #endregion

            DTO_CuentaXPagar CXP = new DTO_CuentaXPagar();
            CXP.DocControl = this._ctrl;
            CXP.CxP = this._cxp;
            CXP.Comp = comp;
            return CXP;
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected override decimal LoadTasaCambio(int monOr)
        {
            try
            {
                decimal valor = 0;
                string tasaMon = this.monedaId;
                if (monOr == (int)TipoMoneda.Local)
                    tasaMon = this.monedaExtranjera;

                valor = _bc.AdministrationModel.TasaDeCambio_Get(tasaMon, this.dtFechaFactura.DateTime);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CausacionFacturas.cs", "LoadTasaCambio"));
                return 0;
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            this.tipoMoneda = (TipoMoneda)monOr;

            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            this.masterMoneda.Value = this.monedaId;
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
                this.cmbMonedaOrigen.Focus();
                return false;
            }

            //Valida datos en la maestra de documento contable
            if (!this.masterConceptoCXP.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterConceptoCXP.CodeRsx);

                MessageBox.Show(msg);
                this.masterConceptoCXP.Focus();

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

            //Valida que el documentoCOM tenga información
            if (string.IsNullOrWhiteSpace(this.txtIVA.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDocumentoCOM");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtIVA.Focus();

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
            //if (this._coDocumento.DocumentoID.Value != this.documentID.ToString() && this.txtFact.Text == "0")
            //{
            //    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_IncorrectConcCxPDocID);

            //    MessageBox.Show(msg);
            //    this.masterConceptoCXP.Focus();

            //    return false;
            //}
            #endregion
            #region Valida las fechas
            if (this.dtFechaFactura.DateTime > this.dtFechaVencimiento.DateTime)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvFechaFact));
                this.dtFechaFactura.Focus();
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
                if (this._cuentaDoc.Naturaleza == null || !this._cuentaDoc.Naturaleza.Value.HasValue)
                    return false;

                decimal difLoc = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                decimal difExt = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);
                decimal difValor = Convert.ToDecimal(this.txtValorBruto.EditValue, CultureInfo.InvariantCulture);

                this._cpConcepto = (DTO_cpConceptoCXP)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, false, this.masterConceptoCXP.Value, true);
                if (this._cpConcepto.ControlCostoInd.Value.Value)
                {
                    difLoc = difValor;
                    difExt = difValor;

                    #region Cuanto el concepto CxP NO maneja control de costos
                    //Revisa que la sumatoria de las cuentas que tengan el indicador de costos sumen el valor bruto de la factura

                    Dictionary<string, DTO_coCuentaGrupo> cacheCtas = new Dictionary<string, DTO_coCuentaGrupo>();
                    DTO_coPlanCuenta cta = new DTO_coPlanCuenta();
                    DTO_coCuentaGrupo grp = null;

                    #region Calcula los nuevos valores
                    foreach (DTO_ComprobanteFooter f in this.Comprobante.Footer)
                    {
                        if (cacheCtas.ContainsKey(f.CuentaID.Value))
                            grp = cacheCtas[f.CuentaID.Value];
                        else
                        {
                            cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, f.CuentaID.Value, true);
                            if (!string.IsNullOrWhiteSpace(cta.CuentaGrupoID.Value))
                            {
                                grp = (DTO_coCuentaGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCuentaGrupo, false, cta.CuentaGrupoID.Value, true);
                                cacheCtas.Add(f.CuentaID.Value, grp);
                            }
                            else
                                cacheCtas.Add(f.CuentaID.Value, null);
                        }

                        if (grp != null && grp.CostoInd.Value.Value && f.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString())
                        {
                            difLoc = Math.Round(difLoc - f.vlrMdaLoc.Value.Value, 2);
                            difExt = Math.Round(difExt - f.vlrMdaExt.Value.Value, 2);
                        }
                    }
                    #endregion
                    #region Realiza la validacion
                    if (monOr == (int)TipoMoneda.Local)
                    {
                        if (difLoc != 0)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvalidTotalCostoML);
                            MessageBox.Show(string.Format(msg, difLoc));
                            return false;
                        }
                    }
                    else
                    {
                        if (difExt != 0)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvalidTotalCostoME);
                            MessageBox.Show(string.Format(msg, difExt));
                            return false;
                        }
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    #region Cuanto el concepto CxP NO maneja control de costos
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
                    #endregion
                }

                if (this.TempData.CxP.Dato1.Value == EstadoResumenImp.SinAprobar.ToString())
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoResumenImpAprob));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs-", "CanSave"));
                return false;
            }
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            try
            {
                DTO_CuentaXPagar CXP = (DTO_CuentaXPagar)aux;
                DTO_glDocumentoControl ctrl = CXP.DocControl;
                DTO_cpCuentaXPagar cxp = CXP.CxP;
                DTO_Comprobante comp = CXP.Comp;

                DTO_ComprobanteHeader header = comp.Header;
                this.comprobanteID = header.ComprobanteID.Value;
                this._compNro = header.ComprobanteNro.Value.Value;

                if (comp.Footer == null)
                    comp.Footer = new List<DTO_ComprobanteFooter>();

                bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, ctrl.PeriodoDoc.Value.Value, this.comprobanteID, this._compNro);
                if (usefulTemp || this._compNro == 0)
                {
                    #region Trae los datos del formulario dado el documento contable
                    //Trae el concepto CxP y el coDocumento
                    this._cpConcepto = (DTO_cpConceptoCXP)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, false, cxp.ConceptoCxPID.Value, true);
                    this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cpConcepto.coDocumentoID.Value, true);

                    this.prefijoID = this._coDocumento.PrefijoID.Value;
                    this.txtPrefix.Text = this.prefijoID;
                    #endregion
                    #region Trae la cuenta y el concepto de saldo
                    //Trae la cuenta y el concepto de saldo
                    this._cuentaDoc = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, ctrl.CuentaID.Value, true);
                    this._concSaldoDoc = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, this._cuentaDoc.ConceptoSaldoID.Value, true);
                    #endregion
                    #region Carga la info del header
                    this.masterConceptoCXP.Value = this._cpConcepto.ID.Value;
                    this.txtFact.Text = ctrl.DocumentoTercero.Value;
                    this.masterMoneda.Value = ctrl.MonedaID.Value;
                    TipoMoneda_LocExt tm = ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                    string tmVal = ((int)tm).ToString();
                    this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(tmVal);

                    this.masterCuenta_.Value = ctrl.CuentaID.Value;
                    this.masterTercero_.Value = ctrl.TerceroID.Value;
                    this.masterProyecto.Value = ctrl.ProyectoID.Value;
                    this.masterCentroCosto.Value = ctrl.CentroCostoID.Value;
                    this.masterLugarGeo.Value = ctrl.LugarGeograficoID.Value;

                    comp.CuentaID = this.masterCuenta_.Value;
                    comp.ProyectoID = this.masterProyecto.Value;
                    comp.CentroCostoID = this.masterCentroCosto.Value;
                    comp.LineaPresupuestoID = this.defLineaPresupuesto;
                    comp.LugarGeograficoID = this.masterLugarGeo.Value;

                    this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();
                    this.txtValorBruto.EditValue = cxp.Valor.Value;
                    this.txtIVA.EditValue = cxp.IVA.Value.Value;
                    this.txtDescrDoc.Text = ctrl.Observacion.Value;

                    if (this.dtPeriod.DateTime.Month != cxp.FacturaFecha.Value.Value.Month)
                        this.chkProvisiones.Checked = true;
                    else
                        this.chkProvisiones.Checked = false;

                    //Fechas
                    DateTime fechaVto = cxp.VtoFecha.Value.Value;
                    this.dtFecha.DateTime = cxp.FacturaFecha.Value.Value;
                    this.dtFechaFactura.DateTime = cxp.FacturaFecha.Value.Value;
                    this.dtFechaVencimiento.DateTime = fechaVto;
                    #endregion

                    //Si se presenta un problema asignando la tasa de cambio lo bloquea
                    if (this.ValidateHeader())
                    {
                        #region Header válido
                        this._ctrl.DocumentoTercero.Value = ctrl.DocumentoTercero.Value;

                        //this.EnableHeader(false);

                        this.LoadHeader();
                        this.masterConceptoCXP.EnableControl(true);
                        this.txtIVA.Enabled = true;
                        this.txtValorBruto.Enabled = true;
                        this.txtDescrDoc.Enabled = true;
                        this.dtFechaFactura.Enabled = true;
                        this.dtFechaVencimiento.Enabled = true;
                        
                        this.CalcularValorAnticipos();

                        this.Comprobante = comp;
                        this.data = CXP;
                        this.LoadData(true);
                        this.validHeader = true;
                        this.gcDocument.Focus();

                        #region Carga los datos de los anticipos
                        this._anticipos = new List<DTO_AnticiposResumen>();
                        foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                        {
                            if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString())
                            {
                                DTO_AnticiposResumen antRes = new DTO_AnticiposResumen();
                                antRes.CuentaID.Value = det.CuentaID.Value;
                                antRes.TerceroID.Value = det.TerceroID.Value;
                                antRes.ProyectoID.Value = det.ProyectoID.Value;
                                antRes.CentroCostoID.Value = det.CentroCostoID.Value;
                                antRes.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                                antRes.ConceptoCargoID.Value = det.ConceptoCargoID.Value;
                                antRes.PrefijoID.Value = det.PrefijoCOM.Value;
                                antRes.ConceptoSaldoID.Value = det.ConceptoSaldoID.Value;
                                antRes.IdentificadorTR.Value = Convert.ToInt32(det.IdentificadorTR.Value);
                                antRes.ML.Value = det.vlrMdaLoc.Value;
                                antRes.ME.Value = det.vlrMdaExt.Value;

                                this._anticipos.Add(antRes);
                            }

                        }
                        #endregion

                        this.btnAnticipo.Enabled = true;
                        this.btnLiquida.Enabled = true;
                        this._compLoaded = true;

                        if (this.chkProvisiones.Enabled)
                            this.btnAnticipo.Enabled = false;
                        #endregion
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
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs-", "LoadTempData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();

                this.gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "DocumentoCOM"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ActivoCOM"].Visible = false;

                // Prefijo y Dcumento
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 3;
                prefDoc.Width = 70;
                prefDoc.Visible = true;
                prefDoc.Fixed = FixedStyle.Left;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(prefDoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar del control de documento contable
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterTercero_Enter(object sender, EventArgs e)
        {
            this._terceroFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._terceroFocus)
                {
                    this._terceroFocus = false;
                    ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                    if (master.ValidID)
                    {
                        if (string.IsNullOrWhiteSpace(this._terceroID) || master.Value != this._terceroID)
                            this._terceroID = master.Value;
                    }
                    else
                    {
                        this._terceroID = string.Empty;
                        this._cpConcepto = null;
                        this._coDocumento = null;
                        this._cuentaDoc = null;
                        this._concSaldoDoc = null;
                        this._compNro = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "masterTercero_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFact_Enter(object sender, EventArgs e)
        {
            this._txtFactFocus = true;
            if (!this.masterTercero_.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTercero_.CodeRsx);

                MessageBox.Show(msg);
                this.masterTercero_.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtFact_Leave(object sender, EventArgs e)
        {
            if (this._txtFactFocus)
            {
                this._txtFactFocus = false;
                if (this.txtFact.Text == string.Empty || this.txtFact.Text == "0")
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoFactNro));
                    this.masterTercero_.Focus();
                    return;
                }

                try
                {
                    this.validHeader = false;
                    DTO_CuentaXPagar CxP = _bc.AdministrationModel.CuentasXPagar_GetForCausacion(this.documentID, this.masterTercero_.Value, this.txtFact.Text);
                    if (CxP != null)
                    {
                        #region Validaciones

                        // Valida si existe
                        if (CxP == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoFact));
                            this.txtFact.Focus();
                            this.validHeader = false;
                            return;
                        }

                        // Valida el estado
                        if (CxP.DocControl.Estado.Value.Value != (byte)EstadoDocControl.Radicado &&
                            CxP.DocControl.Estado.Value.Value != (byte)EstadoDocControl.ParaAprobacion &&
                            CxP.DocControl.Estado.Value.Value != (byte)EstadoDocControl.SinAprobar)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoRadicado));
                            this.txtFact.Focus();
                            this.validHeader = false;
                            return;
                        }

                        #endregion
                        #region Carga los datos
                        this._ctrl = CxP.DocControl;
                        this._cxp = CxP.CxP;

                        this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
                        this.masterMoneda.Value = this._ctrl.MonedaID.Value;
                        TipoMoneda_LocExt tm = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                        string tmVal = ((int)tm).ToString();
                        this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(tmVal);
                        this.masterConceptoCXP.Value = this._cxp.ConceptoCxPID.Value;

                        if (this.dtPeriod.DateTime.Month != this._cxp.FacturaFecha.Value.Value.Month)
                            this.chkProvisiones.Checked = true;
                        else
                            this.chkProvisiones.Checked = false;

                        //Fechas
                        DateTime fechaVto = this._cxp.VtoFecha.Value.Value;
                        this.dtFecha.DateTime = this._cxp.FacturaFecha.Value.Value;
                        this.dtFechaFactura.DateTime = this._cxp.FacturaFecha.Value.Value;
                        this.dtFechaVencimiento.DateTime = fechaVto;

                        #endregion
                        #region Trae el conceptoCXP y el coDocumento
                        this._cpConcepto = (DTO_cpConceptoCXP)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, false, this.masterConceptoCXP.Value, true);
                        this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cpConcepto.coDocumentoID.Value, true);
                        this.comprobanteID = this._coDocumento.ComprobanteID.Value;

                        this.prefijoID = this._coDocumento.PrefijoID.Value;
                        this.txtPrefix.Text = this.prefijoID;
                        #endregion
                        #region Revisa si tiene datos del comprobante

                        this.masterConceptoCXP.EnableControl(true);
                        this.txtIVA.Enabled = true;
                        this.txtValorBruto.Enabled = true;
                        this.txtDescrDoc.Enabled = true;
                        this.dtFechaFactura.Enabled = true;
                        this.dtFechaVencimiento.Enabled = true;

                        if (this.multiMoneda)
                            this.cmbMonedaOrigen.Enabled = true;

                        if (CxP.Comp != null)
                        {
                            this.Comprobante = CxP.Comp;
                            this._compNro = this.Comprobante.Header.ComprobanteNro.Value.Value;
                            //this.dtFecha.DateTime = this.Comprobante.Header.Fecha.Value.Value;

                            this._compLoaded = true;
                            if (this.Comprobante.Footer != null && this.Comprobante.Footer.Count > 0)
                                this.Comprobante.Footer.RemoveAt(this.Comprobante.Footer.Count - 1);

                            this.gcDocument.DataSource = this.Comprobante.Footer;
                            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                        }
                        else
                        {
                            this.Comprobante = new DTO_Comprobante();
                            this._compLoaded = false;
                            this.gvDocument.OptionsBehavior.ReadOnly = true;
                        }

                        #endregion

                        if (this.AsignarTasaCambio(false) && this.LoadHeader())
                        {
                            #region Asigna los valores
                            this.CalcularValorAnticipos();

                            this.txtIVA.EditValue = this._cxp.IVA.Value;
                            this.txtValorBruto.EditValue = this._cxp.Valor.Value;
                            this.txtDescrDoc.Text = this._ctrl.Observacion.Value.Length > 50 ? this._ctrl.Observacion.Value.Substring(0, 49) : this._ctrl.Observacion.Value;

                            if (CxP.Comp != null)
                            {
                                this.dataLoaded = true;
                                this.gcDocument.Focus();
                                if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                                    this.CambiarSignoValor();

                                this._compNro = CxP.Comp.Header.ComprobanteNro.Value.Value;
                                this.dataLoaded = false;
                            }

                            this.LoadData(true);
                            this.data = CxP;
                            #endregion
                        }
                    }
                    else
                    {
                        this._ctrl = new DTO_glDocumentoControl();
                        this._cxp = new DTO_cpCuentaXPagar();
                        this.Comprobante = new DTO_Comprobante();
                        this._compLoaded = false;

                        this.masterConceptoCXP.EnableControl(true);
                        this.txtIVA.Enabled = true;
                        this.txtValorBruto.Enabled = true;
                        this.txtDescrDoc.Enabled = true;
                        this.dtFechaFactura.Enabled = true;
                        this.dtFechaVencimiento.Enabled = true;

                        this.masterMoneda.Value = this.monedaLocal;
                        this.dataLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CausacionFacturas.cs-", "txtNumber_Leave"));
                }
            }
        }

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterConceptoCXP_Leave(object sender, EventArgs e)
        {
            if (!this.masterConceptoCXP.ValidID)
            {
                this._cpConcepto = null;
                this._coDocumento = null;

                this.prefijoID = string.Empty;
                this.txtPrefix.Text = this.prefijoID;
            }
            else
            {
                //Trae el concepto CxP y el coDocumento
                this._cpConcepto = (DTO_cpConceptoCXP)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, false, this.masterConceptoCXP.Value, true);
                this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cpConcepto.coDocumentoID.Value, true);
                this.comprobanteID = this._coDocumento.ComprobanteID.Value;

                if (this._ctrl != null && !string.IsNullOrWhiteSpace(this._ctrl.ComprobanteID.Value) && this._ctrl.ComprobanteID.Value != this.comprobanteID)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ConceptoCxPInvalid));
                    this.masterConceptoCXP.Focus();
                }
                this.LoadHeader();
            }
        }

        /// <summary>
        /// Evento que abre la grilla de anticipos
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnAnticipo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Comprobante != null && this.Comprobante.Footer.Count > 0)
                {
                    if (!ValidateRow(this.gvDocument.FocusedRowHandle))
                        return;
                }

                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                ModalAnticipos ant = new ModalAnticipos(this._anticipos, this.dtPeriod.DateTime, this.tipoMoneda, tc, this.masterTercero_.Value);
                ant.ShowDialog();
                if (ant.ReturnVals)
                {
                    #region Carga la nueva lista de anticipos
                    List<DTO_AnticiposResumen> emptyList = new List<DTO_AnticiposResumen>();
                    ant.ReturnList.ForEach(retList =>
                    {
                        bool finded = false;
                        this._anticipos.ForEach(currAnt =>
                        {
                            if
                            (
                                !finded &&
                                retList.CuentaID.Value == currAnt.CuentaID.Value &&
                                retList.TerceroID.Value == currAnt.TerceroID.Value &&
                                retList.ProyectoID.Value == currAnt.ProyectoID.Value &&
                                retList.CentroCostoID.Value == currAnt.CentroCostoID.Value &&
                                retList.LineaPresupuestoID.Value == currAnt.LineaPresupuestoID.Value &&
                                retList.ConceptoSaldoID.Value == currAnt.ConceptoSaldoID.Value &&
                                retList.ConceptoCargoID.Value == currAnt.ConceptoCargoID.Value &&
                                retList.IdentificadorTR.Value == currAnt.IdentificadorTR.Value
                            )
                            {
                                currAnt.MaxVal.Value = retList.MaxVal.Value;
                                emptyList.Add(currAnt);
                                finded = true;
                            }
                        });
                        if (!finded)
                            emptyList.Add(retList);
                    });

                    this._anticipos = emptyList;
                    #endregion
                    #region Elimina los anticipos existentes
                    bool isAnt = false;
                    int index = 0;
                    int removeCount = 0;
                    foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                    {
                        if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString())
                        {
                            isAnt = true;
                            removeCount++;
                        }
                        if (!isAnt)
                            index++;
                    }
                    this.Comprobante.Footer.RemoveRange(index, removeCount);
                    #endregion
                    #region Agrega los registros de los anticipos
                    index = this.Comprobante.Footer.Count;
                    this._anticipos.ForEach(newAnt =>
                    {
                        DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
                        newDet.Index = index;
                        newDet.CuentaID.Value = newAnt.CuentaID.Value;
                        newDet.TerceroID.Value = newAnt.TerceroID.Value;
                        newDet.ProyectoID.Value = newAnt.ProyectoID.Value;
                        newDet.CentroCostoID.Value = newAnt.CentroCostoID.Value;
                        newDet.LineaPresupuestoID.Value = newAnt.LineaPresupuestoID.Value;
                        newDet.ConceptoCargoID.Value = newAnt.ConceptoCargoID.Value;
                        newDet.LugarGeograficoID.Value = this.defLugarGeo;
                        newDet.PrefijoCOM.Value = newAnt.PrefijoID.Value;
                        newDet.DocumentoCOM.Value = newAnt.DocumentoTercero.Value;
                        newDet.PrefDoc = newAnt.PrefijoID.Value + " - " + newAnt.DocumentoTercero.Value;
                        newDet.ActivoCOM.Value = string.Empty;
                        newDet.ConceptoSaldoID.Value = newAnt.ConceptoSaldoID.Value;
                        newDet.IdentificadorTR.Value = newAnt.IdentificadorTR.Value;
                        newDet.Descriptivo.Value = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoAbona);
                        newDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value != null && this.Comprobante.Header.TasaCambioBase.Value.HasValue ?
                            this.Comprobante.Header.TasaCambioBase.Value.Value : 0;

                        newDet.vlrBaseML.Value = 0;
                        newDet.vlrBaseME.Value = 0;
                        newDet.vlrMdaLoc.Value = newAnt.ML.Value * -1;
                        newDet.vlrMdaExt.Value = newAnt.ME.Value * -1;
                        newDet.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? newDet.vlrMdaLoc.Value : newDet.vlrMdaExt.Value;
                        newDet.DatoAdd3.Value = newAnt.MaxVal.Value.Value.ToString();
                        newDet.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        this.Comprobante.Footer.Add(newDet);
                        index++;
                    });

                    #endregion
                    #region Actualiza la grilla
                    this.deleteOP = true;
                    this.LoadData(true);

                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.gcDocument.RefreshDataSource();
                    this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                    this.UpdateTemp(this.data);
                    this.isValid = false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "btnAnticipo_Click"));
            }
        }

        /// <summary>
        /// Saca la lista de impuestos pendientes de liquidar
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnLiquida_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0 && ValidateRow(this.gvDocument.FocusedRowHandle))
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
                    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GetTaxes);

                    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        List<DTO_ComprobanteFooter> detailsImp = new List<DTO_ComprobanteFooter>();
                        List<DTO_ComprobanteFooter> detailsCostos = new List<DTO_ComprobanteFooter>();
                        #region Elimina la lista de impuestos actual
                        bool isImp = false;
                        int index = 0;
                        int removeCount = 0;

                        foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                        {
                            if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Impuesto.ToString())
                            {
                                isImp = true;
                                removeCount++;
                            }
                            if (!isImp)
                                index++;
                        }
                        this.Comprobante.Footer.RemoveRange(index, removeCount);
                        #endregion
                        #region Carga la lista de nuevos registros
                        #region Variables
                        //Variables de los impuestos
                        Dictionary<string, DTO_coProyecto> cacheProys = new Dictionary<string, DTO_coProyecto>();
                        Dictionary<string, DTO_coCentroCosto> cacheCtosCosto = new Dictionary<string, DTO_coCentroCosto>();
                        Dictionary<string, DTO_coImpuestoTipo> cacheImpTipo = new Dictionary<string, DTO_coImpuestoTipo>();
                        DTO_coProyecto proy;
                        DTO_coCentroCosto ctoCosto;
                        DTO_coImpuestoTipo impTipo;

                        //Variables del detalle
                        Dictionary<string, DTO_coPlanCuenta> cuentas = new Dictionary<string, DTO_coPlanCuenta>();
                        Dictionary<string, DTO_coTercero> terceros = new Dictionary<string, DTO_coTercero>();
                        Dictionary<string, string> proyectos = new Dictionary<string, string>();
                        Dictionary<string, string> centrosCosto = new Dictionary<string, string>();

                        index = 0;
                        string operacion;
                        DTO_coPlanCuenta cuentaDet;
                        DTO_coPlanCuenta cuentaImp;
                        DTO_coTercero tercero;
                        DTO_coProyecto proyecto;
                        DTO_coCentroCosto centroCosto;
                        List<DTO_ComprobanteFooter> impuestosAll = new List<DTO_ComprobanteFooter>();

                        int line = 0;
                        DTO_TxResult result = new DTO_TxResult();
                        result.Details = new List<DTO_TxResultDetail>();
                        #endregion
                        foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                        {
                            line++;
                            if (det.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString())
                            {
                                #region Trae el tercero
                                if (terceros.ContainsKey(det.TerceroID.Value))
                                    tercero = terceros[det.TerceroID.Value];
                                else
                                {
                                    tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, det.TerceroID.Value, true);
                                    terceros.Add(det.TerceroID.Value, tercero);
                                }
                                #endregion
                                #region Trae la cuenta
                                if (cuentas.ContainsKey(det.CuentaID.Value))
                                    cuentaDet = cuentas[det.CuentaID.Value];
                                else
                                {
                                    cuentaDet = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, det.CuentaID.Value, true);
                                    cuentas.Add(det.CuentaID.Value, cuentaDet);
                                }
                                #endregion
                                #region Trae la operacion
                                //if (cuentaDet.ProyectoInd.Value.Value)
                                //{
                                if (proyectos.ContainsKey(det.ProyectoID.Value))
                                    operacion = proyectos[det.ProyectoID.Value];
                                else
                                {
                                    proyecto = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, det.ProyectoID.Value, true);
                                    operacion = proyecto.OperacionID.Value;
                                    proyectos.Add(det.ProyectoID.Value, operacion);
                                }
                                //}
                                //else
                                //{
                                if (string.IsNullOrWhiteSpace(operacion))
                                {
                                    if (centrosCosto.ContainsKey(det.CentroCostoID.Value))
                                        operacion = centrosCosto[det.CentroCostoID.Value];
                                    else
                                    {
                                        centroCosto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, det.CentroCostoID.Value, true);
                                        operacion = centroCosto.OperacionID.Value;
                                        centrosCosto.Add(det.CentroCostoID.Value, operacion);
                                    }
                                }
                                //}
                                #endregion
                                #region Carga los impuestos de la cuenta
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                   
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = line;
                                    rd.Message = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_OperacionIsNullorEmpty);
                                    result.Details.Add(rd);
                                }
                                else if (result.Result == ResultValue.OK)
                                {
                                    List<DTO_SerializedObject> res = _bc.AdministrationModel.LiquidarImpuestos(ModulesPrefix.cp, tercero, det.CuentaID.Value, det.ConceptoCargoID.Value, operacion, det.LugarGeograficoID.Value, det.LineaPresupuestoID.Value, det.vlrMdaLoc.Value.Value);
                                    if (res.Count > 0 && res.First().GetType() == typeof(DTO_TxResult))
                                    {
                                        result.Result = ResultValue.NOK;

                                        DTO_TxResult rTemp = (DTO_TxResult)res.First();
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = line;
                                        rd.Message = rTemp.ResultMessage;
                                        result.Details.Add(rd);
                                    }
                                    else
                                    {
                                        //Lista de impuestos x registro del detalle
                                        List<DTO_SerializedObject> lt = (List<DTO_SerializedObject>)res;
                                        List<DTO_CuentaValor> impuestos = lt.Cast<DTO_CuentaValor>().ToList();
                                        #region Carga el valor del nuevo registro
                                        impuestos.ForEach(imp =>
                                        {
                                            index++;
                                            DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
                                            #region Cuenta
                                            if (cuentas.ContainsKey(imp.CuentaID.Value))
                                                cuentaImp = cuentas[imp.CuentaID.Value];
                                            else
                                            {
                                                cuentaImp = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, imp.CuentaID.Value, true);
                                                cuentas.Add(imp.CuentaID.Value, cuentaImp);
                                            }
                                            #endregion
                                            #region Valores del registro original

                                            //General
                                            newDet.Index = index;
                                            newDet.CuentaID.Value = cuentaImp.ID.Value;
                                            newDet.TerceroID.Value = det.TerceroID.Value;
                                            newDet.ConceptoCargoID.Value = det.ConceptoCargoID.Value;
                                            newDet.DocumentoCOM.Value = det.DocumentoCOM.Value;
                                            newDet.ActivoCOM.Value = string.Empty;
                                            newDet.ConceptoSaldoID.Value = cuentaImp.ConceptoSaldoID.Value;
                                            newDet.IdentificadorTR.Value = 0;
                                            newDet.Descriptivo.Value = det.Descriptivo.Value;
                                            newDet.TasaCambio.Value = det.TasaCambio.Value;

                                            //FK
                                            newDet.LugarGeograficoID.Value = imp.LugarGeograficoID;

                                            //Valores
                                            newDet.vlrBaseML.Value = imp.Base.Value.Value;
                                            newDet.vlrBaseME.Value = this.multiMoneda ? Math.Round(newDet.vlrBaseML.Value.Value / newDet.TasaCambio.Value.Value, 2) : 0;
                                            newDet.vlrMdaLoc.Value = imp.Valor.Value;
                                            newDet.vlrMdaExt.Value = this.multiMoneda ? Math.Round(newDet.vlrMdaLoc.Value.Value / newDet.TasaCambio.Value.Value, 2) : 0;
                                            newDet.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? newDet.vlrMdaLoc.Value : newDet.vlrMdaExt.Value;

                                            //Dato Add 4
                                            newDet.DatoAdd4.Value = AuxiliarDatoAdd4.Impuesto.ToString();

                                            #endregion
                                            #region Varifica si hay que cambiar la tarifa del costo original
                                            if (imp.TarifaCosto != 0)
                                                det.DatoAdd2.Value = imp.TarifaCosto.ToString();
                                            #endregion
                                            if (cuentaImp.ID.Value == det.CuentaID.Value)
                                            {
                                                #region Cuentas de costo
                                                #region Info de FKs
                                                newDet.ProyectoID.Value = det.ProyectoID.Value;
                                                newDet.CentroCostoID.Value = det.CentroCostoID.Value;
                                                newDet.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                                                //newDet.LugarGeograficoID.Value = det.LugarGeograficoID.Value;
                                                newDet.PrefijoCOM.Value = det.PrefijoCOM.Value;
                                                newDet.PrefDoc = newDet.PrefijoCOM.Value + " - " + newDet.DocumentoCOM.Value;
                                                #endregion
                                                #region Revisa si debe asignar la info de IVA y tarifa
                                                newDet.DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                                                if (cuentaImp.ImpuestoTipoID.Value == this.tipoImpuestoIVA)
                                                    newDet.DatoAdd2.Value = cuentaImp.ImpuestoPorc.Value.Value.ToString();
                                                else
                                                    newDet.DatoAdd2.Value = "0";
                                                #endregion

                                                detailsCostos.Add(newDet);
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Otras cuentas
                                                #region Info de FKs
                                                if (imp.IVADescontable)
                                                {
                                                    #region Proyecto
                                                    if (cacheProys.ContainsKey(det.ProyectoID.Value))
                                                        proy = cacheProys[det.ProyectoID.Value];
                                                    else
                                                    {
                                                        proy = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, det.ProyectoID.Value, true);
                                                        cacheProys.Add(det.ProyectoID.Value, proy);
                                                    }

                                                    newDet.ProyectoID.Value = string.IsNullOrWhiteSpace(proy.PryCapitalTrabajo.Value) ? this.defProyecto : proy.PryCapitalTrabajo.Value;
                                                    #endregion
                                                    #region Centro Costo
                                                    if (cacheCtosCosto.ContainsKey(det.CentroCostoID.Value))
                                                        ctoCosto = cacheCtosCosto[det.CentroCostoID.Value];
                                                    else
                                                    {
                                                        ctoCosto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, det.CentroCostoID.Value, true);
                                                        cacheCtosCosto.Add(det.CentroCostoID.Value, ctoCosto);
                                                    }

                                                    newDet.CentroCostoID.Value = string.IsNullOrWhiteSpace(ctoCosto.CtoCapitalTrabajo.Value) ? this.defCentroCosto : ctoCosto.CtoCapitalTrabajo.Value;
                                                    #endregion
                                                }
                                                else
                                                {
                                                    newDet.ProyectoID.Value = this.defProyecto;
                                                    newDet.CentroCostoID.Value = this.defCentroCosto;
                                                }

                                                #region Lugar geografico (comentareado)
                                                //if (!string.IsNullOrWhiteSpace(cuentaImp.ImpuestoTipoID.Value))
                                                //{
                                                //    if (cacheImpTipo.ContainsKey(cuentaImp.ImpuestoTipoID.Value))
                                                //        impTipo = cacheImpTipo[cuentaImp.ImpuestoTipoID.Value];
                                                //    else
                                                //    {
                                                //        impTipo = (DTO_coImpuestoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoTipo, false, cuentaImp.ImpuestoTipoID.Value, true);
                                                //        cacheImpTipo.Add(cuentaImp.ImpuestoTipoID.Value, impTipo);
                                                //    }

                                                //    if (impTipo.ImpuestoAlcance.Value.Value == (short)ImpuestoAlcance.Municipal)
                                                //        newDet.LugarGeograficoID.Value = det.LugarGeograficoID.Value;
                                                //    else
                                                //        newDet.LugarGeograficoID.Value = this.defLugarGeo;
                                                //}
                                                //else
                                                //    newDet.LugarGeograficoID.Value = this.defLugarGeo;
                                                #endregion

                                                newDet.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                                newDet.PrefijoCOM.Value = this.defPrefijo;
                                                newDet.PrefDoc = newDet.PrefijoCOM.Value + " - " + newDet.DocumentoCOM.Value;
                                                #endregion
                                                #region Revisa si existe o se debe agregar
                                                //Revisa si el registro existe
                                                List<DTO_ComprobanteFooter> vals = detailsImp.Where(
                                                    temp =>
                                                        temp.CuentaID.Value == newDet.CuentaID.Value &&
                                                        temp.TerceroID.Value == newDet.TerceroID.Value &&
                                                        temp.ConceptoCargoID.Value == newDet.ConceptoCargoID.Value &&
                                                        temp.ProyectoID.Value == newDet.ProyectoID.Value &&
                                                        temp.CentroCostoID.Value == newDet.CentroCostoID.Value &&
                                                        temp.LugarGeograficoID.Value == newDet.LugarGeograficoID.Value
                                                    ).ToList();

                                                if (vals.Count() > 0)
                                                {
                                                    vals.First().vlrBaseML.Value = Math.Round(vals.First().vlrBaseML.Value.Value + newDet.vlrBaseML.Value.Value, 2);
                                                    vals.First().vlrMdaLoc.Value = Math.Round(vals.First().vlrMdaLoc.Value.Value + newDet.vlrMdaLoc.Value.Value, 2);
                                                    vals.First().vlrBaseME.Value = Math.Round(vals.First().vlrBaseME.Value.Value + newDet.vlrBaseME.Value.Value, 2);
                                                    vals.First().vlrMdaExt.Value = Math.Round(vals.First().vlrMdaExt.Value.Value + newDet.vlrMdaExt.Value.Value, 2);
                                                    vals.First().vlrMdaOtr.Value = Math.Round(vals.First().vlrMdaOtr.Value.Value + newDet.vlrMdaOtr.Value.Value, 2);
                                                }
                                                else
                                                {
                                                    #region Revisa si debe asignar la info de IVA y tarifa
                                                    if (cuentaImp.ImpuestoTipoID.Value == this.tipoImpuestoIVA)
                                                    {
                                                        newDet.DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                                                        newDet.DatoAdd2.Value = cuentaImp.ImpuestoPorc.Value.Value.ToString();
                                                    }
                                                    else
                                                        newDet.DatoAdd2.Value = "0";
                                                    #endregion
                                                    detailsImp.Add(newDet);
                                                }

                                                #endregion
                                                #endregion
                                            }

                                        });

                                        #endregion
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                        #region Carga la nueva lista de datos y refresca la grilla
                        if (result.Result == ResultValue.OK)
                        {
                            //Carga la informacion de costos
                            detailsCostos.ForEach(det =>
                            {
                                det.Index = index;
                                index++;
                                this.Comprobante.Footer.Add(det);
                            });

                            //Carga la informacion de impuestos
                            detailsImp.ForEach(det =>
                            {
                                det.Index = index;
                                index++;
                                this.Comprobante.Footer.Add(det);
                            });

                            this.disableValidate = true;
                            this.LoadData(true);
                            this.disableValidate = false; 
                            
                            this.LoadEditGridData(false, 0);
                            this.UpdateTemp(this.data);
                        }
                        else
                        {
                            MessageForm msgForm = new MessageForm(result);
                            msgForm.ShowDialog();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "btnLiquida_Click"));
            }
        }

        /// <summary>
        /// Saca el resumen de impuestos
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnResumenImp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0 && ValidateRow(this.gvDocument.FocusedRowHandle))
                {
                    ResumenImpuestos resumenImp = new ResumenImpuestos(this.Comprobante.Footer, this.tipoMoneda, this.masterTercero_.Value);
                    resumenImp.ShowDialog();
                    if (resumenImp._aprobado)
                        this.TempData.CxP.Dato1.Value = EstadoResumenImp.Aprobado.ToString();
                    else
                        this.TempData.CxP.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "btnResumenImp_Click"));
            }
        }

        /// <summary>
        /// Modifica el valor de la fecha de vencimiento con respecto a la fecha de factura
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void dtFechaFactura_EditValueChanged(object sender, EventArgs e)
        {
            this.dtFecha.DateTime = this.dtFechaFactura.DateTime;
            this.dtFechaVencimiento.Properties.MinValue = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFechaFactura.DateTime.AddDays(double.Parse(this._nDias));
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la seleccion de provisiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkProvisiones_CheckedChanged(object sender, EventArgs e)
        {
            this.AsignarFechas();
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
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }

                #region Si ya tiene datos cargados
                if (!this.dataLoaded)
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                #endregion
                #region Si entra al detalle y no tiene datos

                //this.EnableHeader(false);
                this.masterTercero_.EnableControl(false);
                this.txtFact.Enabled = false;
                this.btnAnticipo.Enabled = this.chkProvisiones.Checked ? false : true;
                this.btnLiquida.Enabled = true;
                
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_CuentaXPagar cxp = (DTO_CuentaXPagar)this.LoadTempHeader();
                        DTO_Comprobante comp = cxp.Comp;

                        if (this._compLoaded)
                            comp.Footer = this.Comprobante.Footer;

                        cxp.Comp = comp;
                        this.TempData = cxp;

                        if (this.Comprobante.Footer.Count == 0)
                        {
                            FormProvider.Master.itemExport.Enabled = false;
                            FormProvider.Master.itemPrint.Enabled = false;
                            FormProvider.Master.itemSendtoAppr.Enabled = false;

                            this.LoadData(true);
                        }

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCredito.cs", "grlDocument_Enter" + ex.Message));
                }
                #endregion
            }
            //else
            //{
            //    this.masterConceptoCXP.Focus();
            //}
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
            if (!this.validHeader)
                this.masterTercero_.Focus();

            if (this.TempData.CxP.Dato1.Value == EstadoResumenImp.Aprobado.ToString())
                this.TempData.CxP.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            base.gvDocument_CellValueChanged(sender, e);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "DatoAdd4"];
            string colVal = this.gvDocument.GetRowCellValue(this.gvDocument.FocusedRowHandle, col).ToString();

            if (colVal == AuxiliarDatoAdd4.Anticipo.ToString() && (fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt"))
            {
                this.isValid = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
            }

            if (this.TempData.CxP.Dato1.Value == EstadoResumenImp.Aprobado.ToString())
                this.TempData.CxP.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
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
                    this.data = new DTO_CuentaXPagar();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;

                    this.masterTercero_.Focus();
                    this.EnableHeader(false);

                    this.btnAnticipo.Enabled = false;
                    this.btnLiquida.Enabled = false;

                    this.masterTercero_.EnableControl(true);
                    this.txtFact.Enabled = true;
                    this._compLoaded = false;
                    this._headerLoaded = false;
                    this.CalcularTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "TBNew"));
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

                int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                this.gvDocument.ActiveFilterString = string.Empty;
               
                this.CalcularTotal();
                if (this.ValidGrid() && this.CanSave(monOr))
                {
                    //Si es cta debito cambia los valores
                    decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                    decimal ME = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);
                    decimal tc = this.Comprobante.Header.TasaCambioBase.Value.Value;
                    decimal mdaTemp = 0;
                    decimal diferencia = 0;

                    if(tc != 0)
                    {
                        #region Corrige las diferencias
                        if (this.monedaId == this.monedaLocal)
                        {
                            mdaTemp = Math.Round(ML / tc, 2);
                            if (mdaTemp != ME)
                            {
                                if (ME > mdaTemp)
                                {
                                    diferencia = ME - mdaTemp;
                                    this.Comprobante.Footer.Last().vlrMdaExt.Value -= diferencia;
                                    ME -= diferencia;
                                }
                                else
                                {
                                    diferencia = mdaTemp - ME;
                                    this.Comprobante.Footer.Last().vlrMdaExt.Value += diferencia;
                                    ME += diferencia;
                                }
                            }
                        }
                        else
                        {
                            mdaTemp = Math.Round(ME * tc, 2);
                            if (mdaTemp != ML)
                            {
                                if (ML > mdaTemp)
                                {
                                    diferencia = ML - mdaTemp;
                                    this.Comprobante.Footer.Last().vlrMdaLoc.Value -= diferencia;
                                    ML -= diferencia;
                                }
                                else
                                {
                                    diferencia = mdaTemp - ML;
                                    this.Comprobante.Footer.Last().vlrMdaLoc.Value += diferencia;
                                    ML += diferencia;
                                }
                            }
                        }
                        #endregion
                    }

                    if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                        this.CambiarSignoValor();
                    else
                    {
                        ML *= -1;
                        ME *= -1;
                    }

                    #region Actualiza la CxP y el documento
                    DTO_CuentaXPagar cxp = (DTO_CuentaXPagar)this.LoadTempHeader();
                    cxp.DocControl.Valor.Value = this.monedaId == this.monedaLocal ? ML * -1 : ME * -1;
                    this.TempData.DocControl = cxp.DocControl;
                    this.TempData.CxP = cxp.CxP;
                    #endregion                  
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
                    last.PrefijoCOM.Value = this._ctrl.PrefijoID.Value;
                    last.DocumentoCOM.Value = this._ctrl.DocumentoTercero.Value;
                    last.ActivoCOM.Value = string.Empty;
                    last.ConceptoSaldoID.Value = this._cuentaDoc.ConceptoSaldoID.Value;
                    last.IdentificadorTR.Value = 0;
                    last.Descriptivo.Value = this.txtDescrDoc.Text.Length <= 50 ? this.txtDescrDoc.Text : this.txtDescrDoc.Text.Substring(0, 50);
                    last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                    decimal vlrDoc = Convert.ToDecimal(this.txtValorBruto.EditValue, CultureInfo.InvariantCulture);
                    last.vlrBaseML.Value = 0;
                    last.vlrBaseME.Value = 0;
                    last.vlrMdaLoc.Value = ML;
                    last.vlrMdaExt.Value = ME;
                    last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                    last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    this.Comprobante.Footer.Add(last);

                    #endregion
                    #region Actualiza la fecha del comprobante para provisiones
                    if (this.chkProvisiones.Checked)
                        this.Comprobante.Header.Fecha.Value = this.dtPeriod.DateTime;
                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBSave.cs-", "txtNumber_Leave"));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                base.TBDelete();
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

                        this.btnAnticipo.Enabled = false;
                        this.btnLiquida.Enabled = false;
                        this.masterTercero_.EnableControl(true);
                        this.txtFact.Enabled = true;
                        this._compLoaded = false;
                        this._headerLoaded = false;

                        this.newDoc = true;
                        this.deleteOP = true;

                        this.data = new DTO_CuentaXPagar();
                        this.Comprobante = new DTO_Comprobante();
                        //this.disableValidate = true;
                        this.gcDocument.DataSource = this.Comprobante.Footer;
                        //this.disableValidate = false;

                        this.CleanHeader(true);
                        this.EnableHeader(true);
                        this.masterTercero_.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotacreditoCxP.cs-", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotacreditoCxP.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBPaste()
        {
            base.TBPaste();
            if (!FormProvider.Master.itemSendtoAppr.Enabled)
            {
                this._anticipos = new List<DTO_AnticiposResumen>();
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

                result = _bc.AdministrationModel.CuentasXPagar_Causar(this.documentID, this.TempData.DocControl, this.TempData.CxP, this.Comprobante);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                //Remueve el ultimo registro
                this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());//At(this.Comprobante.Footer.Count - 1);

                // Cambia los valores de signos segun la cuenta
                if (this._cuentaDoc.Naturaleza.Value.Value == (int)NaturalezaCuenta.Debito)
                    this.CambiarSignoValor();

                if (result.Result.Equals(ResultValue.OK))
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    if (this._compNro == 0)
                    {
                        DTO_CuentaXPagar CxP = _bc.AdministrationModel.CuentasXPagar_GetForCausacion(this.documentID, this.masterTercero_.Value, this.txtFact.Text);
                        this._compNro = CxP.DocControl.ComprobanteIDNro.Value.Value;

                        this.Comprobante.Header.ComprobanteNro.Value = this._compNro;
                    }

                    this.resultOK = true;
                }
                else
                {
                    this.resultOK = false;
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
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

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.ComprobantePre_SendToAprob(this.documentID, this._actFlujo.ID.Value, this.frmModule, this.dtPeriod.DateTime, this.comprobanteID, this._compNro, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                #region Gerera Reporte

                if (obj.GetType() == typeof(DTO_Alarma))
                {
                    string numDoc = ((DTO_Alarma)obj).NumeroDoc;
                    bool finaliza = ((DTO_Alarma)obj).Finaliza;
                    string reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(Convert.ToInt32(numDoc), finaliza,true, ExportFormatType.pdf);

                    if (reportName == string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                }

                #endregion

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_CuentaXPagar();
                    this.Comprobante = new DTO_Comprobante();
                    this._compLoaded = false;
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotaCreditoCxP.cs", "SendToApproveThread"));
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
                    this._anticipos = new List<DTO_AnticiposResumen>();
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