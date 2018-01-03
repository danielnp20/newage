using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Text.RegularExpressions;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.ReportesComunes;
using DevExpress.XtraEditors;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentFacturaForm : DocumentForm
    {
        public DocumentFacturaForm()
        {
          //InitializeComponent();
        }
        
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            //this.gcDocument.DataSource = this.data.Footer;

            //this.CleanHeader(true);
            //this.EnableHeader(false);
            //this.EnableFooter(false);

            FormProvider.Master.itemSendtoAppr.Enabled = true;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            // this.TBNew();
            this.gcDocument.DataSource = this.data.Footer;

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.EnableFooter(false);

            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region variables privadas

        private BaseController _bc = BaseController.GetInstance();

        //Variables de data
        private TipoConcepto _tipoConcepto = 0;
        private string _conceptoCargo;
        private DTO_faConceptos _conceptos;
        private DTO_coPlanCuenta _cuenta;
        private DTO_coTercero _regFiscalEmp = new DTO_coTercero();
        private string _refFiscalTercero = string.Empty;
        private DTO_inCostosExistencias costos = null;
        private List<DTO_glConsultaFiltro> filtrosParam1 = new List<DTO_glConsultaFiltro>();
        private List<DTO_glConsultaFiltro> filtrosParam2 = new List<DTO_glConsultaFiltro>();

        //Variables generales
        private Dictionary<string, string> _pksPar1;
        private Dictionary<string, string> _pksPar2;
        private decimal _converFactor;
        private bool moduleInventarioActive = true;
        private bool _costeoTipoInd = false;
        private bool _serialInd = false;
        private decimal _saldoDisponible = 0;
        private decimal _cantidadEmp = 0;
        private string _codigoIVA = string.Empty;
        private string _codigoRFT = string.Empty;
        private string _codigoReteIVA = string.Empty;
        private string _codigoReteICA = string.Empty;
        private string _parametro1xDef = string.Empty;
        private string _parametro2xDef = string.Empty;
        protected string _tipoFacturaCtaCobro = string.Empty;

        //Variable para general el documento de factura
        private string reportName;
        #endregion

        #region Variables Protected

        //Variables formulario
        protected DTO_faFacturacion data = null;
        //Variables Moneda
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        protected TipoMoneda_LocExt tipoMonedaOr;
        //Indica si el header es valido
        protected bool validHeader;
        //variables para funciones particulares
        protected bool cleanDoc = true;
        //Variables por defecto
        protected string defTercero = string.Empty;
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLineaPresupuesto = string.Empty;
        protected string defConceptoCargo = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string defParametro1 = string.Empty;
        protected string defParametro2 = string.Empty;
        protected bool cuentaIcaNoExiste;
        protected bool indContabilizaRetencionFactura;
        //Variables del formulario
        protected int facturaNro = 0;
        protected DTO_glDocumentoControl ctrl = null;
        protected DTO_faFacturaDocu factHeader = null;
        protected DTO_faFacturacionFooter _rowCurrent = new DTO_faFacturacionFooter();
        protected List<DTO_faFacturacionFooter> factFooter = null;
        protected DTO_coDocumento coDocumento = null;
        protected DTO_glConceptoSaldo concSaldoDoc = null;
        protected DTO_coPlanCuenta cuentaDoc = null;
        protected DTO_faCliente cliente = null;
        protected DTO_faFacturaTipo facturaTipo = null;
        protected decimal _vlrAnticipoTerc = 0;
        protected bool headerLoaded = false;
        protected bool isLoadingHeader = false;
        protected string _cuentaRteICA = string.Empty;
        protected DateTime periodoFact;
        protected bool IVAUtilidad = false;
        #endregion

        #region Propiedades

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get   { return this.data.Footer.FindIndex(det => det.Index == this.indexFila);  }
        }
        
        //Servicio
        private DTO_faServicios _servicio = null;
        protected DTO_faServicios Servicio
        {
            get
            {
                return this._servicio;
            }
            set
            {
                this._servicio = value;
                int index = this.NumFila;
                
                if (value == null)
                {
                    #region Si el servicio no existe
                    this._rowCurrent.Movimiento.ServicioID.Value = string.Empty;
                    this.masterServicio.Value = string.Empty;
                    this.Bodega = null;
                    this._tipoConcepto = 0;
                    this.Tercero = null;

                    //this.CleanFooter(true);
                    this.EnableFooter(false);
                    this.masterServicio.EnableControl(true);
                    this.masterTareaProy.EnableControl(this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false);
                    this.btnTareas.Enabled = this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false;
                    #endregion
                }
                else
                {
                    #region Si el servicio Si Existe
                    this._conceptos = (DTO_faConceptos)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, false, value.ConceptoIngresoID.Value, true);
                    this._tipoConcepto = (TipoConcepto)Enum.Parse(typeof(TipoConcepto), this._conceptos.TipoConcepto.Value.Value.ToString());
                    if (this._tipoConcepto == TipoConcepto.VentaInv && !this.moduleInventarioActive)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_ServicioInvalid));
                        this.masterServicio.Focus();
                        return;
                    }
                    this._conceptoCargo = this._conceptos.ConceptoCargoID.Value;
                    this._cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false,!this.IVAUtilidad? this._conceptos.CuentaID.Value : this._conceptos.CuentaAIU.Value, true);

                    switch (this._tipoConcepto)
                    {
                        case TipoConcepto.Servicio:
                            this.EnableFooter(false);
                            this.masterServicio.EnableControl(true);
                            this.masterTareaProy.EnableControl(this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false);
                            this.btnTareas.Enabled = this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false;
                            this.txtCantidad.Enabled = true;
                            this.txtDescr.Enabled = true;
                            this.gvDocument.Columns[this.unboundPrefix + "CantidadUNI"].OptionsColumn.AllowEdit = true;
                            break;
                        case TipoConcepto.VentaInv:
                            this.EnableFooter(true);
                            this.masterEmpaque.EnableControl(false);
                            this.masterReferencia.EnableControl(false);
                            this.btnReferencia.Enabled = false;
                            this.masterParametro1.EnableControl(false);
                            this.masterParametro2.EnableControl(false);
                            this.cmbSerialDisp.Enabled = false;
                            this.txtCantEmpaque.Enabled = false;
                            this.txtCantidad.Enabled = false;
                            this.txtDescr.Enabled = false;
                            this.gvDocument.Columns[this.unboundPrefix + "CantidadUNI"].OptionsColumn.AllowEdit = false;
                            break;
                        case TipoConcepto.Otros:
                            this.EnableFooter(false);
                            this.masterServicio.EnableControl(true);
                            this.masterTareaProy.EnableControl(false);
                            this.btnTareas.Enabled = false;
                            this.txtCantidad.Enabled = true;
                            this.txtDescr.Enabled = true;
                            this.gvDocument.Columns[this.unboundPrefix + "CantidadUNI"].OptionsColumn.AllowEdit = true;
                            break;
                        default:
                            this.EnableFooter(false);
                            this.masterServicio.EnableControl(true);
                            this.masterTareaProy.EnableControl(this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false);
                            this.btnTareas.Enabled = this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false;
                            break;                            
                    }
                    #endregion
                }
                this.btnNotaEnv.Enabled = true;
                this.gvDocument.RefreshData();
            }
        }
        
        //Tercero
        private DTO_coTercero _tercero = null;
        protected DTO_coTercero Tercero
        {
            get
            {
                return this._tercero;
            }
            set
            {
                this._tercero = value;
                int index = this.NumFila;

                if (value == null)
                {
                    #region Si el tercero No existe
                    this._refFiscalTercero = string.Empty;
                    #endregion
                }
                else
                {
                    #region Si el tercero Si existe
                    this._refFiscalTercero = this._tercero.ReferenciaID.Value;
                    #endregion
                }
                this.gvDocument.RefreshData();
            }
        }

        //Activo
        private DTO_acActivoControl _activo = null;
        protected DTO_acActivoControl Activo
        {
            get
            {
                return this._activo;
            }
            set
            {
                this._activo = value;
                int index = this.NumFila;

                if (value == null)
                {
                    #region Si el tercero No existe
                    //this._activoID = 0;
                    this.txtPlaqueta.Text = string.Empty;
                    this._rowCurrent.Movimiento.PlaquetaID.Value = string.Empty;
                    this._rowCurrent.Movimiento.ActivoID.Value = 0;
                    #endregion
                }
                else
                {
                    #region Si el tercero Si existe
                    //this._activoID = this._activo.ActivoID.Value.Value;
                    this.txtPlaqueta.Text = this._activo.PlaquetaID.Value;
                    this._rowCurrent.Movimiento.PlaquetaID.Value = this._activo.PlaquetaID.Value;
                    this._rowCurrent.Movimiento.ActivoID.Value = this._activo.ActivoID.Value.Value;
                    #endregion
                }
                this.gvDocument.RefreshData();
            }
        }
        
        //Bodega
        private DTO_inBodega _bodega = null;
        protected DTO_inBodega Bodega
        {
            get
            {
                return this._bodega;
            }
            set
            {
                this._bodega = value;
                int index = this.NumFila;

                if (value == null)
                {
                    #region Si la bodega no existe
                    this._rowCurrent.Movimiento.BodegaID.Value = string.Empty;
                    this.masterBodega.Value = string.Empty;

                    this.Referencia = null;
                    this.masterReferencia.EnableControl(false);
                    this.btnReferencia.Enabled = false;
                   #endregion
                }
                else
                {
                    #region Si la bodega Si Existe
                    this._costeoTipoInd = false;
                    DTO_inCosteoGrupo costeoGrupo = (DTO_inCosteoGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, this._bodega.CosteoGrupoInvID.Value, true);
                    if (costeoGrupo.CosteoTipo.Value == (int)TipoCosteoInv.Promedio)
                    {
                        this.masterReferencia.EnableControl(true);
                        this.btnReferencia.Enabled = true;
                        if (this.masterReferencia.ValidID)
                            this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);
                        else
                            this.masterReferencia.Focus();
                    }
                    else
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_TipoCosteoOnlyPromedio));
                        this.masterReferencia.EnableControl(false);
                        this.btnReferencia.Enabled = false;
                        this.masterBodega.Focus();
                        this._costeoTipoInd = true;
                    }
                    #endregion
                }
                this.gvDocument.RefreshData();
            }
        }

        //Referencia
        private DTO_inReferencia _referencia = null;
        private DTO_inReferencia Referencia
        {
            get
            {
                return this._referencia;
            }
            set
            {
                this._referencia = value;
                int index = this.NumFila;
                //this.EnableFooter(true);

                if (value == null)
                {
                    #region Si referencia no existe
                    this.TipoRef = null;
                    this.Empaque = null;

                    this._rowCurrent.Movimiento.inReferenciaID.Value = string.Empty;
                    this._rowCurrent.Movimiento.DescripTExt.Value = string.Empty;
                    this.masterReferencia.Value = string.Empty;
                    this.txtDescr.Text = string.Empty;
                   // this.txtDescr.Enabled = false;
                    this.masterEmpaque.EnableControl(false);
                    #endregion
                }
                else
                {
                    this.txtDescr.Text = string.IsNullOrEmpty(this._rowCurrent.Movimiento.DescripTExt.Value) ? this._referencia.Descriptivo.Value : this._rowCurrent.Movimiento.DescripTExt.Value;
                    this._rowCurrent.Movimiento.DescripTExt.Value = this.txtDescr.Text;
                    this.masterEmpaque.EnableControl(true);
                    this.txtDescr.Enabled = true;

                    this.TipoRef = (DTO_inRefTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, this._referencia.TipoInvID.Value, true);
                    this.Empaque = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, this._referencia.EmpaqueInvID.Value, true);
                }

                this.gvDocument.RefreshData();
            }
        }

        //Referencia Tipo
        private DTO_inRefTipo _tipoRef = null;
        private DTO_inRefTipo TipoRef
        {
            get
            {
                return this._tipoRef;
            }
            set
            {
                this._tipoRef = value;
                int index = this.NumFila;

                _pksPar1 = new Dictionary<string, string>();
                _pksPar2 = new Dictionary<string, string>();

                if (value == null)
                {
                    this._rowCurrent.Movimiento.Parametro1.Value = string.Empty;
                    this._rowCurrent.Movimiento.Parametro2.Value = string.Empty;
                    this.masterParametro1.EnableControl(false);
                    this.masterParametro2.EnableControl(false);
                    this.cmbSerialDisp.Enabled = false;

                    this.Activo = null;
                }
                else
                {
                    #region Trae los parametros si estan habilitados
                    _pksPar1 = new Dictionary<string, string>();
                    _pksPar2 = new Dictionary<string, string>();        

                    if (_tipoRef.Parametro1Ind.Value.Value)
                    {
                        filtrosParam1 = InventoryParameters.GetQueryParameters(_tipoRef.ID.Value, true);
                        this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, true, filtrosParam1);
                        this.masterParametro1.EnableControl(true);
                    }
                    else
                    {
                        _bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, false);
                        this.masterParametro1.EnableControl(false);
                        this.masterParametro1.Value = this._parametro1xDef;
                        this._rowCurrent.Movimiento.Parametro1.Value = this.masterParametro1.Value;
                    }
                    if (_tipoRef.Parametro2Ind.Value.Value)
                    {
                        filtrosParam2 = InventoryParameters.GetQueryParameters(_tipoRef.ID.Value, false);
                        _bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, true, filtrosParam2);
                        this.masterParametro2.EnableControl(true);
                    }
                    else
                    {
                        _bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, false);
                        this.masterParametro2.EnableControl(false);
                        this.masterParametro2.Value = this._parametro2xDef;
                        this._rowCurrent.Movimiento.Parametro2.Value = this.masterParametro2.Value;
                    }
                    _pksPar1.Add("TipoInvID", this._tipoRef.ID.Value);
                    _pksPar1.Add("Parametro1ID", this._rowCurrent.Movimiento.Parametro1.Value);

                    _pksPar2.Add("TipoInvID", this._tipoRef.ID.Value);
                    _pksPar2.Add("Parametro2ID", this._rowCurrent.Movimiento.Parametro2.Value);
                    #endregion

                    #region  Trae una lista de seriales segun la referencia
                    this._serialInd = false;
                    this.cmbSerialDisp.Items.Clear();
                    if (this._tipoRef.SerializadoInd.Value.Value)
                    {
                        this._cantidadEmp = 1;
                        this.txtCantEmpaque.Enabled = false;
                        this.cmbSerialDisp.Enabled = true;

                        DTO_acActivoControl activo = new DTO_acActivoControl();
                        activo.inReferenciaID.Value = this.Referencia.ID.Value;
                        activo.BodegaID.Value = this.Bodega.ID.Value;
                        List<DTO_acActivoControl> result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                        foreach (DTO_acActivoControl serial in result)
                        {
                            this.cmbSerialDisp.Items.Add(serial.SerialID.Value);
                        }
                        if (this.cmbSerialDisp.Items.Count == 0)
                        {
                            string message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NotExistSerial);
                            MessageBox.Show(String.Format(message, this.Bodega.ID.Value, this.Referencia.ID.Value.ToString()));

                            this.cmbSerialDisp.Enabled = false;
                            this.masterParametro1.EnableControl(false);
                            this.masterParametro2.EnableControl(false);
                            this.masterEmpaque.EnableControl(false);
                            this.masterReferencia.Focus();
                            this._serialInd = true;
                        }
                    }
                    else
                    {
                        this._cantidadEmp = 0;
                        this.txtCantEmpaque.Enabled = true;
                        this.cmbSerialDisp.Enabled = false;
                    }
                    #endregion
                    
                    //this.GetSaldoDisponible();
                }
            }
        }
        
        //Empaque
        private DTO_inEmpaque _empaque = null;
        protected DTO_inEmpaque Empaque
        {
            get
            {
                return this._empaque;
            }
            set
            {
                this._empaque = value;
                int index = this.NumFila;

                if (value == null)
                {
                    #region Si el empaque no existe
                    this._rowCurrent.Movimiento.EmpaqueInvID.Value = string.Empty;
                    this._rowCurrent.Movimiento.CantidadEMP.Value = 0;
                    this._rowCurrent.Movimiento.CantidadUNI.Value = 0;
                    this.masterEmpaque.Value = string.Empty;
                    this.txtCantEmpaque.EditValue = 0;
                    this.txtCantidad.EditValue = 0;
                    this.txtCantEmpaque.Enabled = false;
                    #endregion
                }
                else
                {
                    #region Si el empaque Si Existe
                    //this._converFactor = 1;
                    //this._cantidadEmp = 0;
                    
                    DTO_inUnidad unidadEmp = (DTO_inUnidad)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, this._empaque.UnidadInvID.Value, true);
                    DTO_inUnidad unidadRef = (DTO_inUnidad)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, this._referencia.UnidadInvID.Value, true);

                    if (unidadEmp.ID.Value != unidadRef.ID.Value)
                    {
                        if (unidadEmp.TipoMedida.Value == unidadRef.TipoMedida.Value)
                        {
                            Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                            keysConvert.Add("UnidadInvID", unidadEmp.ID.Value);
                            keysConvert.Add("UnidadBase", unidadRef.ID.Value);
                            DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)_bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                            if (conversion != null)
                            {
                                this._converFactor = conversion.Factor.Value.Value;
                                this._rowCurrent.Movimiento.EmpaqueInvID.Value = this._empaque.ID.Value;
                                this.masterEmpaque.Value = this._empaque.ID.Value;
                                this._cantidadEmp = this._empaque.Cantidad.Value.Value;
                                this.txtCantEmpaque.Enabled = !this.TipoRef.SerializadoInd.Value.Value;
                            }
                            else
                            {
                                this._cantidadEmp = 0;
                                this._converFactor = 0;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistConvertUnit));
                                masterEmpaque.Focus();
                            }
                        }
                        else
                        {
                            this._cantidadEmp = 0;
                            this._converFactor = 0;
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UnitInvalidEmp));
                            masterEmpaque.Focus();
                        }
                    }
                    else
                    {
                        this._rowCurrent.Movimiento.EmpaqueInvID.Value = this._empaque.ID.Value;
                        this.masterEmpaque.Value = this._empaque.ID.Value;
                        this._cantidadEmp = this._empaque.Cantidad.Value.Value; 
                        this._converFactor = 1;
                    }
                    #endregion
                }
                this.gvDocument.RefreshData();
            }
        }

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        protected virtual DTO_faFacturacion TempData
        {
            get
            {
                return (DTO_faFacturacion)this.data;
            }
            set
            {
                this.data = value;
            }
        }
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        private void GenerateReport(bool show, bool allFields=false)
        {
        }

        /// <summary>
        /// Calcula los valores de retenciones     
        /// </summary>
        /// <param name="masivoInd">Indica si es masivo el calculo</param>
        private void GetRetenciones(bool masivoInd)
        {
            try
            {
                decimal porcIVA = 0;
                decimal porcRIVA = 0;
                decimal porcRFT = 0;
                decimal porcOtros = 0;

                if (!masivoInd)
                {
                    //Calcular impuestos para el item actual
                    this.cuentaIcaNoExiste = true;
                    if (!string.IsNullOrEmpty(this._rowCurrent.Movimiento.TerceroID.Value) && !string.IsNullOrEmpty(this._conceptoCargo))
                    {
                        #region Crea consulta para la tabla coImpuesto
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        DTO_glConsultaFiltro filtro;

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalEmpresaID";
                        filtro.ValorFiltro = _regFiscalEmp.ReferenciaID.Value;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalTerceroID";
                        filtro.ValorFiltro = this._refFiscalTercero;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ConceptoCargoID";
                        filtro.ValorFiltro = this._conceptoCargo;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "LugarGeograficoID";
                        filtro.ValorFiltro = this.data.DocCtrl.LugarGeograficoID.Value;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        if (this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_IndContabilizaRetencionFactura) != "1")
                        {
                            filtro = new DTO_glConsultaFiltro();
                            filtro.CampoFisico = "ImpuestoTipoID";
                            filtro.ValorFiltro = this._codigoIVA;
                            filtro.OperadorFiltro = OperadorFiltro.Igual;
                            filtro.OperadorSentencia = "and";
                            filtros.Add(filtro);
                        }

                        consulta.Filtros = filtros;
                        #endregion
                        #region Trae los impuestos existentes
                        long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                        var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null).ToList();
                        #endregion
                        #region Valida la información para obtener los porcentajes de Retenciones
                        if (listCoImp != null)
                        {
                            foreach (var coImp in listCoImp)
                            {
                                DTO_coImpuesto dtoImp = (DTO_coImpuesto)coImp;
                                if (dtoImp.RegimenFiscalEmpresaID.Value == _regFiscalEmp.ReferenciaID.Value && dtoImp.RegimenFiscalTerceroID.Value == this._refFiscalTercero &&
                                    dtoImp.ConceptoCargoID.Value == this._conceptoCargo && dtoImp.LugarGeograficoID.Value == this.data.DocCtrl.LugarGeograficoID.Value &&
                                    (this.indContabilizaRetencionFactura || dtoImp.ImpuestoTipoID.Value == this._codigoIVA))
                                {
                                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dtoImp.CuentaID.Value, true);
                                    if (dtoImp.ImpuestoTipoID.Value == this._codigoIVA)
                                    {
                                        try
                                        {
                                            porcIVA = cta.ImpuestoPorc.Value.Value;
                                            #region Obtiene ReteIVA
                                            Dictionary<string, string> keyReteIva = new Dictionary<string, string>();
                                            keyReteIva.Add("EmpresaGrupoID", this._bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coIVARetencion));
                                            keyReteIva.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                                            keyReteIva.Add("RegimenFiscalTerceroID", _refFiscalTercero);
                                            keyReteIva.Add("CuentaIVA", cta.ID.Value);
                                            DTO_coIvaRetencion reteIVA = (DTO_coIvaRetencion)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.coIVARetencion, keyReteIva, true);
                                            if (reteIVA != null)
                                            {
                                                DTO_coPlanCuenta ctaReteIVA = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, reteIVA.CuentaReteIVA.Value, true);
                                                porcRIVA = ctaReteIVA.ImpuestoPorc.Value.Value;
                                            }
                                            #endregion
                                        }
                                        catch (Exception)
                                        {
                                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_PorcentajeImpCuentaInvalid);
                                            MessageBox.Show(string.Format(msg, cta.ID.Value));
                                        }
                                    }
                                    if (cta.Naturaleza.Value.Value != (byte)NaturalezaCuenta.Credito)//Discrimina si es IVA(Crédito)
                                    {
                                        if (dtoImp.ImpuestoTipoID.Value == this._codigoRFT)
                                            porcRFT = cta.ImpuestoPorc.Value.Value;
                                        else if (dtoImp.ImpuestoTipoID.Value == this._codigoReteIVA)
                                            porcRIVA = cta.ImpuestoPorc.Value.Value;
                                        else if (dtoImp.ImpuestoTipoID.Value == this._codigoReteICA)
                                            this.cuentaIcaNoExiste = false;
                                    }
                                }
                            }
                        }
                        if (this.cuentaIcaNoExiste && this.data.Header.Porcentaje1.Value != 0)
                        {
                            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                            string noCuentaIca = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoCuentaIca);
                            this.gvDocument.SetColumnError(col, noCuentaIca);
                            MessageBox.Show(noCuentaIca);
                        }
                        #endregion
                    }

                    this._rowCurrent.PorcIVA = porcIVA;
                    this._rowCurrent.PorcRIVA = porcRIVA;
                    this._rowCurrent.PorcRFT = porcRFT;
                    this._rowCurrent.PorcOtros = porcOtros;
                }
                else
                {
                    //Calcula impuestos para todo el detalle
                    foreach (DTO_faFacturacionFooter f in this.data.Footer)
                    {
                        this.cuentaIcaNoExiste = true;
                        this.Servicio = (DTO_faServicios)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, f.Movimiento.ServicioID.Value, true);

                        this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, f.Movimiento.TerceroID.Value, true);

                        if (!string.IsNullOrEmpty(this.masterBodega.Value))
                            this.Bodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, f.Movimiento.BodegaID.Value, true);

                        if (!string.IsNullOrEmpty(this.masterReferencia.Value))
                            this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, f.Movimiento.inReferenciaID.Value, true);

                        if (!string.IsNullOrEmpty(this.masterEmpaque.Value))
                            this.Empaque = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, f.Movimiento.EmpaqueInvID.Value, true);

                        if (this._rowCurrent.Movimiento.ActivoID.Value != null)
                            this.Activo = _bc.AdministrationModel.acActivoControl_GetByID(f.Movimiento.ActivoID.Value.Value);

                        if (!string.IsNullOrEmpty(f.Movimiento.TerceroID.Value) && !string.IsNullOrEmpty(this._conceptoCargo))
                        {
                            #region Crea consulta para la tabla coImpuesto
                            DTO_glConsulta consulta = new DTO_glConsulta();
                            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                            DTO_glConsultaFiltro filtro;

                            filtro = new DTO_glConsultaFiltro();
                            filtro.CampoFisico = "RegimenFiscalEmpresaID";
                            filtro.ValorFiltro = _regFiscalEmp.ReferenciaID.Value;
                            filtro.OperadorFiltro = OperadorFiltro.Igual;
                            filtro.OperadorSentencia = "and";
                            filtros.Add(filtro);

                            filtro = new DTO_glConsultaFiltro();
                            filtro.CampoFisico = "RegimenFiscalTerceroID";
                            filtro.ValorFiltro = this._refFiscalTercero;
                            filtro.OperadorFiltro = OperadorFiltro.Igual;
                            filtro.OperadorSentencia = "and";
                            filtros.Add(filtro);

                            filtro = new DTO_glConsultaFiltro();
                            filtro.CampoFisico = "ConceptoCargoID";
                            filtro.ValorFiltro = this._conceptoCargo;
                            filtro.OperadorFiltro = OperadorFiltro.Igual;
                            filtro.OperadorSentencia = "and";
                            filtros.Add(filtro);

                            filtro = new DTO_glConsultaFiltro();
                            filtro.CampoFisico = "LugarGeograficoID";
                            filtro.ValorFiltro = this.data.DocCtrl.LugarGeograficoID.Value;
                            filtro.OperadorFiltro = OperadorFiltro.Igual;
                            filtro.OperadorSentencia = "and";
                            filtros.Add(filtro);

                            if (this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_IndContabilizaRetencionFactura) != "1")
                            {
                                filtro = new DTO_glConsultaFiltro();
                                filtro.CampoFisico = "ImpuestoTipoID";
                                filtro.ValorFiltro = this._codigoIVA;
                                filtro.OperadorFiltro = OperadorFiltro.Igual;
                                filtro.OperadorSentencia = "and";
                                filtros.Add(filtro);
                            }

                            consulta.Filtros = filtros;
                            #endregion
                            #region Trae los impuestos existentes
                            long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                            var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null).ToList();
                            #endregion
                            #region Valida la información para obtener los porcentajes de Retenciones
                            if (listCoImp != null)
                            {
                                foreach (var coImp in listCoImp)
                                {
                                    DTO_coImpuesto dtoImp = (DTO_coImpuesto)coImp;
                                    if (dtoImp.RegimenFiscalEmpresaID.Value == _regFiscalEmp.ReferenciaID.Value && dtoImp.RegimenFiscalTerceroID.Value == this._refFiscalTercero &&
                                        dtoImp.ConceptoCargoID.Value == this._conceptoCargo && dtoImp.LugarGeograficoID.Value == this.data.DocCtrl.LugarGeograficoID.Value &&
                                        (this.indContabilizaRetencionFactura || dtoImp.ImpuestoTipoID.Value == this._codigoIVA))
                                    {
                                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dtoImp.CuentaID.Value, true);
                                        if (dtoImp.ImpuestoTipoID.Value == this._codigoIVA)
                                        {
                                            try
                                            {
                                                porcIVA = cta.ImpuestoPorc.Value.Value;
                                                #region Obtiene ReteIVA
                                                Dictionary<string, string> keyReteIva = new Dictionary<string, string>();
                                                keyReteIva.Add("EmpresaGrupoID", this._bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coIVARetencion));
                                                keyReteIva.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                                                keyReteIva.Add("RegimenFiscalTerceroID", _refFiscalTercero);
                                                keyReteIva.Add("CuentaIVA", cta.ID.Value);
                                                DTO_coIvaRetencion reteIVA = (DTO_coIvaRetencion)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.coIVARetencion, keyReteIva, true);
                                                if (reteIVA != null)
                                                {
                                                    DTO_coPlanCuenta ctaReteIVA = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, reteIVA.CuentaReteIVA.Value, true);
                                                    porcRIVA = ctaReteIVA.ImpuestoPorc.Value.Value;
                                                }
                                                #endregion
                                            }
                                            catch (Exception)
                                            {
                                                string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_PorcentajeImpCuentaInvalid);
                                                MessageBox.Show(string.Format(msg, cta.ID.Value));
                                            }
                                        }
                                        if (cta.Naturaleza.Value.Value != (byte)NaturalezaCuenta.Credito)//Discrimina si es IVA(Crédito)
                                        {
                                            if (dtoImp.ImpuestoTipoID.Value == this._codigoRFT)
                                                porcRFT = cta.ImpuestoPorc.Value.Value;
                                            else if (dtoImp.ImpuestoTipoID.Value == this._codigoReteIVA)
                                                porcRIVA = cta.ImpuestoPorc.Value.Value;
                                            else if (dtoImp.ImpuestoTipoID.Value == this._codigoReteICA)
                                                this.cuentaIcaNoExiste = false;
                                        }
                                    }
                                }
                            }
                            if (this.cuentaIcaNoExiste && this.data.Header.Porcentaje1.Value != 0)
                            {
                                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                                string noCuentaIca = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoCuentaIca);
                                this.gvDocument.SetColumnError(col, noCuentaIca);
                                MessageBox.Show(noCuentaIca);
                            }
                            #endregion
                        }

                        f.PorcIVA = porcIVA;
                        f.PorcRIVA = porcRIVA;
                        f.PorcRFT = porcRFT;
                        f.PorcOtros = porcOtros; 
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "GetRetenciones"));
            }
        }

        /// <summary>
        /// Calcula los valores      
        /// </summary>
        /// <param name="masivoInd">Indica si es masivo el calculo</param>
        private void LoadValuesDet(bool masivoInd)
        {
            try
            {
                decimal vlrUNI = 0;
                decimal cantUNI = 0;
                decimal vlrBruto = 0;
                decimal vlrIva = 0;
                decimal vlrRiva = 0;
                decimal vlrIca = 0;
                decimal vlrRft = 0;
                decimal vlrRetenciones = 0;
                decimal vlrTotal = 0;
                decimal vlrNeto = 0;

                if (!masivoInd)
                {
                    //Calcula valores en el item actual
                    #region Calcular Valores
                    vlrUNI = this._rowCurrent.Movimiento.ValorUNI.Value.Value;
                    cantUNI = this._rowCurrent.Movimiento.CantidadUNI.Value.Value;
                    vlrBruto = Math.Round(cantUNI * vlrUNI, 0);

                    if (this.data != null && !this.data.Header.FacturaTipoID.Value.Equals(this._tipoFacturaCtaCobro))
                    {
                        vlrIva = Math.Round((this._rowCurrent.PorcIVA / 100) * vlrBruto, 0);
                        vlrRiva = Math.Round((this._rowCurrent.PorcRIVA / 100) * vlrBruto, 0);
                        vlrIca = Math.Round((this.data.Header.Porcentaje1.Value.Value / 1000) * vlrBruto, 0);
                        vlrRft = Math.Round((this._rowCurrent.PorcRFT / 100) * vlrBruto, 0);
                    }
                    vlrRetenciones = vlrRiva + vlrIca + vlrRft;
                    vlrTotal = vlrBruto + vlrIva;
                    vlrNeto = vlrTotal - vlrRetenciones;
                    #endregion
                    #region Asignar Valores
                    this._rowCurrent.ValorBruto = vlrBruto;

                    this._rowCurrent.ValorIVA = vlrIva;
                    this._rowCurrent.ValorRIVA = vlrRiva;
                    this._rowCurrent.ValorRICA = vlrIca;
                    this._rowCurrent.ValorRFT = vlrRft;

                    this._rowCurrent.ValorRetenciones = vlrRetenciones;
                    this._rowCurrent.ValorTotal = vlrTotal;
                    this._rowCurrent.ValorNeto = vlrNeto;
                    #endregion
                }
                else
                {
                    //calcula valores en todo el detalle
                    foreach (DTO_faFacturacionFooter f in this.data.Footer)
                    {
                        #region Calcular Valores
                        vlrUNI = f.Movimiento.ValorUNI.Value.Value;
                        cantUNI = f.Movimiento.CantidadUNI.Value.Value;
                        vlrBruto = Math.Round(cantUNI * vlrUNI, 0);

                        if (this.data != null && !this.data.Header.FacturaTipoID.Value.Equals(this._tipoFacturaCtaCobro))
                        {
                            vlrIva = Math.Round((f.PorcIVA / 100) * vlrBruto, 0);
                            vlrRiva = Math.Round((f.PorcRIVA / 100) * vlrBruto, 0);
                            vlrIca = Math.Round((this.data.Header.Porcentaje1.Value.Value / 1000) * vlrBruto, 0);
                            vlrRft = Math.Round((f.PorcRFT / 100) * vlrBruto, 0);
                        }
                        vlrRetenciones = vlrRiva + vlrIca + vlrRft;
                        vlrTotal = vlrBruto + vlrIva;
                        vlrNeto = vlrTotal - vlrRetenciones;
                        #endregion
                        #region Asignar Valores
                        f.ValorBruto = vlrBruto;

                        f.ValorIVA = vlrIva;
                        f.ValorRIVA = vlrRiva;
                        f.ValorRICA = vlrIca;
                        f.ValorRFT = vlrRft;

                        f.ValorRetenciones = vlrRetenciones;
                        f.ValorTotal = vlrTotal;
                        f.ValorNeto = vlrNeto;
                        #endregion  
                    }
                }

                this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "GetRetenciones"));
            }
        }

        /// <summary>
        /// Calcula el saldo de existencias en las salidas y traslados
        /// </summary>
        private void GetSaldoDisponible(int index)
        {
            if (this.data != null && this.data.DocCtrl.Estado.Value != 3 && (this.data.Header.DocumentoREL == null || this.data.Header.DocumentoREL.Value.Value == 0))
            {
                try
                {
                    //int index = this.NumFila;
                    //decimal cantidadDisp = 0;
                    DTO_inControlSaldosCostos saldos;
                    if (this.Referencia != null &&
                        !string.IsNullOrEmpty(this._rowCurrent.Movimiento.Parametro1.Value) &&
                        !string.IsNullOrEmpty(this._rowCurrent.Movimiento.Parametro2.Value) &&
                        !string.IsNullOrEmpty(this.cmbEstado.Text) && this._rowCurrent.Movimiento.ActivoID != null)
                    {
                        this.costos = new DTO_inCostosExistencias();
                        saldos = new DTO_inControlSaldosCostos();
                        saldos.BodegaID.Value = this.Bodega.ID.Value;
                        saldos.inReferenciaID.Value = this.Referencia.ID.Value;
                        saldos.ActivoID.Value = this.TipoRef.SerializadoInd.Value.Value ? this._rowCurrent.Movimiento.ActivoID.Value : 0;
                        saldos.EstadoInv.Value = this._rowCurrent.Movimiento.EstadoInv.Value;
                        saldos.Parametro1.Value = this._rowCurrent.Movimiento.Parametro1.Value;
                        saldos.Parametro2.Value = this._rowCurrent.Movimiento.Parametro2.Value;
                        decimal saldoDispInit = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref costos);

                        decimal cant = this.data.Footer.Where(x => x.Movimiento.BodegaID.Value == saldos.BodegaID.Value &&
                            x.Movimiento.inReferenciaID.Value == saldos.inReferenciaID.Value &&
                            x.Movimiento.Parametro1.Value == saldos.Parametro1.Value &&
                            x.Movimiento.Parametro2.Value == saldos.Parametro2.Value &&
                            x.Movimiento.EstadoInv.Value.Value == saldos.EstadoInv.Value.Value &&
                            x.Movimiento.ActivoID.Value.Value == saldos.ActivoID.Value.Value).Sum(x => x.Movimiento.CantidadUNI.Value.Value);

                        this._cantidadEmp = this._rowCurrent.Movimiento.CantidadEMP.Value.Value;
                        this._saldoDisponible = saldoDispInit - cant + this._rowCurrent.Movimiento.CantidadUNI.Value.Value;
                        if (this._rowCurrent.Movimiento.CantidadUNI.Value.Value > this._saldoDisponible)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_CantityAvailable);
                            MessageBox.Show(string.Format(msg, this._saldoDisponible.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "GetSaldoDisponible"));
                }
            }
        }

        #endregion

        #region Funciones Abstractas (Implementacion DocumentForm)

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            try
            {
                this.gcDocument.DataSource = this.data.Footer;
                this.gcDocument.RefreshDataSource();
                bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
                if (hasItems)
                    this.gvDocument.MoveFirst();

                this.dataLoaded = true;
                this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            try
            {
                this.newReg = false;
                int cFila = fila;
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                this.indexFila = Convert.ToInt32(this.gvDocument.GetRowCellValue(cFila, col));

                this.LoadEditGridData(false, cFila);               
                this.isValid = true;

                if (oper)
                    this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "RowIndexChanged"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();

            try
            {
                // Carga info de las monedas
                this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                this.periodoFact = Convert.ToDateTime(this._bc.GetControlValueByCompany(this.frmModule, AppControl.fa_Periodo));

                FormProvider.Master.itemPaste.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;

                //Controles del detalle
                this._bc.InitMasterUC(this.masterServicio, AppMasters.faServicios, true, false, false, false);
                this._bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, true, false);
                this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, false, true, false);
                this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, false);
                this._bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, false);
                this._bc.InitMasterUC(this.masterEmpaque, AppMasters.inEmpaque, true, true, true, false);
                this._bc.InitMasterUC(this.masterLineaPresup, AppMasters.plLineaPresupuesto, true, true, true, false);
                this._bc.InitMasterUC(this.masterTareaProy, AppMasters.pyTarea, true, true, true, false);
                this.masterParametro1.BringToFront();

                //Llena los combos
                TablesResources.GetTableResources(this.cmbEstado, typeof(EstadoInv));

                //Carga Valores por defecto
                this.defTercero = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                this.defPrefijo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                this.defConceptoCargo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                this._codigoIVA = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoIVA);
                this._codigoRFT = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteFuente);
                this._codigoReteIVA = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteIVA);
                this._codigoReteICA = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoICA);
                this._tipoFacturaCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_TipoFacturaCtaCobro);

                var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false).ToList();

                if (modules.Exists(x => x.ModuloID.Value == ModulesPrefix.@in.ToString()))
                {
                    this._parametro1xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                    this._parametro2xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                }
                else
                    this.moduleInventarioActive = false;

                this.data = new DTO_faFacturacion();
                this.ctrl = new DTO_glDocumentoControl();
                this.factHeader = new DTO_faFacturaDocu();
                this.factFooter = new List<DTO_faFacturacionFooter>();

                this.indContabilizaRetencionFactura = (this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_IndContabilizaRetencionFactura) == "0") ? false : true;

                //Campos de valores
                //this.txtValorBruto.Enabled = false;
                //this.txtValorIVA.Enabled = false;
                //this.txtValorRetenciones.Enabled = false;
                //this.txtValorNeto.Enabled = false;
                //this.txtValorRIVA.Enabled = false;
                //this.txtValorRFT.Enabled = false;
                //this.txtValorRICA.Enabled = false;
                ////this.txtValorAnticipoDet.Enabled = false;
                //this.txtValorRteGarantia.Enabled = false;
                //this.txtValorOtros.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "SetInitParameters"));
            }

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            if (this.dtPeriod.DateTime.Month == DateTime.Now.Month)
                base.dtFecha.DateTime = DateTime.Now;

            this.dtFecha.Properties.MinValue = new DateTime(this.periodoFact.Year, this.periodoFact.Month, this.periodoFact.Day);
            DateTime maxfecha = this.dtFecha.Properties.MinValue.AddMonths(1);
            this.dtFecha.Properties.MaxValue = new DateTime(maxfecha.Year, maxfecha.Month, DateTime.DaysInMonth(maxfecha.Year, maxfecha.Month));
            base.dtFecha.DateTime = DateTime.Now;

            this.AddGridCols();
            this.lastColName = this.unboundPrefix + "TerceroID";

            //Trae Impuestos Ventas por Defecto


            this.EnableFooter(false);

            //Trae el Reg Fiscal de la empresa
            DTO_glEmpresa emp = (DTO_glEmpresa)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, false, this.empresaID, true);
            this._regFiscalEmp = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto), true);
            
            #region Carga Temporales
            if (false)
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DTO_faFacturacion factTemp = (DTO_faFacturacion)_bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (factTemp != null)
                        {
                            try
                            {
                                this.LoadTempData(factTemp);
                            }
                            catch (Exception ex1)
                            {
                                this.validHeader = false;
                                MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                            }
                        }
                        else
                        {
                            this.validHeader = false;
                            MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                            _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        }
                    }
                    else
                    {
                        this.validHeader = false;
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "AfterInitialize: " + ex.Message));
                }
            } 
            #endregion
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas Visibles
                //NroItem
                GridColumn NroItem = new GridColumn();
                NroItem.FieldName = this.unboundPrefix + "NroItem";
                NroItem.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NroItem");
                NroItem.UnboundType = UnboundColumnType.Integer;
                NroItem.VisibleIndex = 0;
                NroItem.Width = 50;
                NroItem.Visible = true;
                NroItem.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(NroItem);

                //ImprimeInd
                GridColumn ImprimeInd = new GridColumn();
                ImprimeInd.FieldName = this.unboundPrefix + "ImprimeInd";
                ImprimeInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ImprimeInd");
                ImprimeInd.UnboundType = UnboundColumnType.Boolean;
                ImprimeInd.VisibleIndex = 0;
                ImprimeInd.Width = 30;
                ImprimeInd.Visible = true;
                ImprimeInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ImprimeInd);

                //CodigoServicios
                GridColumn codServ = new GridColumn();
                codServ.FieldName = this.unboundPrefix + "ServicioID";
                codServ.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ServicioID");
                codServ.UnboundType = UnboundColumnType.String;
                codServ.VisibleIndex = 1;
                codServ.Width = 70;
                codServ.Visible = true;
                codServ.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codServ);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 50;
                codRef.Visible = true;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codRef);

                //DescripTExt
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "DescripTExt";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 150;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);
               
                //ValorUnitario
                GridColumn vlrUnitario = new GridColumn();
                vlrUnitario.FieldName = this.unboundPrefix + "ValorUNI";
                vlrUnitario.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUNI");
                vlrUnitario.UnboundType = UnboundColumnType.Decimal;
                vlrUnitario.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrUnitario.AppearanceCell.Options.UseTextOptions = true;
                vlrUnitario.VisibleIndex = 4;
                vlrUnitario.Width = 100;
                vlrUnitario.Visible = true;
                vlrUnitario.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(vlrUnitario);

                //Cantidad
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadUNI";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadUNI");
                cant.UnboundType = UnboundColumnType.Decimal;
                cant.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cant.AppearanceCell.Options.UseTextOptions = true;
                cant.VisibleIndex = 5;
                cant.Width = 100;
                cant.Visible = true;
                cant.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(cant);

                //ValorBruto
                GridColumn vlrBruto = new GridColumn();
                vlrBruto.FieldName = this.unboundPrefix + "ValorBruto";
                vlrBruto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorBruto");
                vlrBruto.UnboundType = UnboundColumnType.Decimal;
                vlrBruto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrBruto.AppearanceCell.Options.UseTextOptions = true;
                vlrBruto.VisibleIndex = 6;
                vlrBruto.Width = 100;
                vlrBruto.Visible = true;
                vlrBruto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrBruto);

                //ValorIVA
                GridColumn vlrIVA = new GridColumn();
                vlrIVA.FieldName = this.unboundPrefix + "ValorIVA";
                vlrIVA.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorIVA");
                vlrIVA.UnboundType = UnboundColumnType.Decimal;
                vlrIVA.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrIVA.AppearanceCell.Options.UseTextOptions = true;
                vlrIVA.VisibleIndex = 7;
                vlrIVA.Width = 100;
                vlrIVA.Visible = true;
                vlrIVA.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrIVA);

                //ValorTotal
                GridColumn vlrTotal = new GridColumn();
                vlrTotal.FieldName = this.unboundPrefix + "ValorTotal";
                vlrTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotal");
                vlrTotal.UnboundType = UnboundColumnType.Decimal;
                vlrTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrTotal.AppearanceCell.Options.UseTextOptions = true;
                vlrTotal.VisibleIndex = 8;
                vlrTotal.Width = 100;
                vlrTotal.Visible = true;
                vlrTotal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrTotal);

                //DocSoporte
                GridColumn docSoporte = new GridColumn();
                docSoporte.FieldName = this.unboundPrefix + "DocSoporteTER";
                docSoporte.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocSoporteTER");
                docSoporte.UnboundType = UnboundColumnType.String;
                docSoporte.VisibleIndex = 9;
                docSoporte.Width = 80;
                docSoporte.Visible = true;
                docSoporte.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(docSoporte);

                //DocServicio                                                                   
                GridColumn docServicio = new GridColumn();
                docServicio.FieldName = this.unboundPrefix + "DocSoporte";
                docServicio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocSoporte");
                docServicio.UnboundType = UnboundColumnType.Integer;
                docServicio.VisibleIndex = 10;
                docServicio.Width = 80;
                docServicio.Visible = false;
                docServicio.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(docServicio);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this.unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 11;
                tercero.Width = 70;
                tercero.Visible = true;
                tercero.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(tercero);

                //LineaPresupuesto
                GridColumn LineaPresupuesto = new GridColumn();
                LineaPresupuesto.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                LineaPresupuesto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
                LineaPresupuesto.UnboundType = UnboundColumnType.String;
                LineaPresupuesto.VisibleIndex = 12;
                LineaPresupuesto.Width = 90;
                LineaPresupuesto.Visible = true;
                LineaPresupuesto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(LineaPresupuesto);   

                //Centro de costo 
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 13;
                ctoCosto.Width = 90;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ctoCosto);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 14;
                proyecto.Width = 90;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(proyecto);              
                #endregion
                #region Columnas No Visibles
                //EstadoInv
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.UnboundType = UnboundColumnType.Integer;
                estado.Visible = false;
                this.gvDocument.Columns.Add(estado);

                //BodegaID
                GridColumn bodega = new GridColumn();
                bodega.FieldName = this.unboundPrefix + "BodegaID";
                bodega.UnboundType = UnboundColumnType.String;
                bodega.Visible = false;
                this.gvDocument.Columns.Add(bodega);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;
                param1.Visible = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDocument.Columns.Add(param2);

                //IdentificadorTr
                GridColumn param3 = new GridColumn();
                param3.FieldName = this.unboundPrefix + "IdentificadorTr";
                param3.UnboundType = UnboundColumnType.Integer;
                param3.Visible = false;
                this.gvDocument.Columns.Add(param3);

                //Activo
                GridColumn act = new GridColumn();
                act.FieldName = this.unboundPrefix + "ActivoID";
                act.UnboundType = UnboundColumnType.Integer;
                act.Visible = false;
                this.gvDocument.Columns.Add(act);

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.UnboundType = UnboundColumnType.String;
                serial.Visible = false;
                this.gvDocument.Columns.Add(serial);

                //Plaqueta                                              
                GridColumn plaqueta = new GridColumn();
                plaqueta.FieldName = this.unboundPrefix + "PlaquetaID";
                plaqueta.UnboundType = UnboundColumnType.String;
                plaqueta.Visible = false;
                this.gvDocument.Columns.Add(plaqueta);

                //Empaque
                GridColumn emapaque = new GridColumn();
                emapaque.FieldName = this.unboundPrefix + "EmpaqueInvID";
                emapaque.UnboundType = UnboundColumnType.String;
                emapaque.Visible = false;
                this.gvDocument.Columns.Add(emapaque);

                //CantEmpaque
                GridColumn cantEmpaque = new GridColumn();
                cantEmpaque.FieldName = this.unboundPrefix + "CantidadEMP";
                cantEmpaque.UnboundType = UnboundColumnType.Decimal;
                cantEmpaque.Visible = false;
                this.gvDocument.Columns.Add(cantEmpaque);

                //ValorNeto
                GridColumn vlrNeto= new GridColumn();
                vlrNeto.FieldName = this.unboundPrefix + "ValorNeto";
                vlrNeto.UnboundType = UnboundColumnType.Decimal;
                vlrNeto.Visible = false;
                this.gvDocument.Columns.Add(vlrNeto);

                //ValorRIVA
                GridColumn vlrRIVA = new GridColumn();
                vlrRIVA.FieldName = this.unboundPrefix + "ValorRIVA";
                vlrRIVA.UnboundType = UnboundColumnType.Decimal;
                vlrRIVA.Visible = false;
                this.gvDocument.Columns.Add(vlrRIVA);

                //ValorRFT
                GridColumn vlrRFT = new GridColumn();
                vlrRFT.FieldName = this.unboundPrefix + "ValorRFT";
                vlrRFT.UnboundType = UnboundColumnType.Decimal;
                vlrRFT.Visible = false;
                this.gvDocument.Columns.Add(vlrRFT);

                //ValorRICA
                GridColumn vlrRICA = new GridColumn();
                vlrRICA.FieldName = this.unboundPrefix + "ValorRICA";
                vlrRICA.UnboundType = UnboundColumnType.Decimal;
                vlrRICA.Visible = false;
                this.gvDocument.Columns.Add(vlrRICA);

                //ValorRemesa
                GridColumn vlrRemesa = new GridColumn();
                vlrRemesa.FieldName = this.unboundPrefix + "ValorRemesa";
                vlrRemesa.UnboundType = UnboundColumnType.Decimal;
                vlrRemesa.Visible = false;
                this.gvDocument.Columns.Add(vlrRemesa);

                //ValorOtros
                GridColumn vlrOtros = new GridColumn();
                vlrOtros.FieldName = this.unboundPrefix + "ValorOtros";
                vlrOtros.UnboundType = UnboundColumnType.Decimal;
                vlrOtros.Visible = false;
                this.gvDocument.Columns.Add(vlrOtros);

                //ValorRetenciones
                GridColumn vlrRet = new GridColumn();
                vlrRet.FieldName = this.unboundPrefix + "ValorRetenciones";
                vlrRet.UnboundType = UnboundColumnType.Decimal;
                vlrRet.Visible = false;
                this.gvDocument.Columns.Add(vlrRet);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "AddGridCols"));
            }
        }
        
        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_faFacturacionFooter footerDet = new DTO_faFacturacionFooter();

            #region Asigna datos a la fila
            if (this.data.Footer.Count > 0)
            {
                footerDet.Index = this.data.Footer.Last().Index + 1;
                footerDet.Movimiento.NroItem.Value = footerDet.Movimiento.NroItem.Value + 1;
                footerDet.Movimiento.ServicioID.Value = this.data.Footer.Last().Movimiento.ServicioID.Value;
                footerDet.Movimiento.TerceroID.Value = this.data.Footer.Last().Movimiento.TerceroID.Value;
                footerDet.Movimiento.ProyectoID.Value = this.data.Footer.Last().Movimiento.ProyectoID.Value;
                footerDet.Movimiento.CentroCostoID.Value = this.data.Footer.Last().Movimiento.CentroCostoID.Value;
                footerDet.Movimiento.LineaPresupuestoID.Value = this.data.Footer.Last().Movimiento.LineaPresupuestoID.Value;
            }
            else
            {
                footerDet.Index = 0;
                footerDet.Movimiento.NroItem.Value = 1;
                footerDet.Movimiento.ServicioID.Value = string.Empty;
                footerDet.Movimiento.CentroCostoID.Value =  this.data != null? this.data.DocCtrl.CentroCostoID.Value : string.Empty;
                footerDet.Movimiento.LineaPresupuestoID.Value =  this.data != null? this.data.DocCtrl.LineaPresupuestoID.Value: string.Empty;
                footerDet.Movimiento.TerceroID.Value = this.cliente.TerceroID.Value;
                footerDet.Movimiento.ProyectoID.Value = this.data != null? this.data.DocCtrl.ProyectoID.Value : string.Empty;
            }
            footerDet.Movimiento.ImprimeInd.Value = true;
            footerDet.Movimiento.EmpresaID.Value = this.empresaID;
            footerDet.Movimiento.BodegaID.Value = string.Empty;
            footerDet.Movimiento.PlaquetaID.Value = string.Empty;
            footerDet.Movimiento.inReferenciaID.Value = string.Empty;
            footerDet.Movimiento.EstadoInv.Value = (int)EstadoInv.Activo;
            footerDet.Movimiento.Parametro1.Value = string.Empty;
            footerDet.Movimiento.Parametro2.Value = string.Empty;
            footerDet.Movimiento.IdentificadorTr.Value = 0;
            footerDet.Movimiento.SerialID.Value = string.Empty;
            footerDet.Movimiento.EmpaqueInvID.Value = string.Empty;
            footerDet.Movimiento.CantidadEMP.Value = 0;
            footerDet.Movimiento.CantidadUNI.Value = 0;
            footerDet.Movimiento.DescripTExt.Value = string.Empty;
            footerDet.Movimiento.ActivoID.Value = 0;
            footerDet.Movimiento.DocSoporteTER.Value = "N/A";
            footerDet.Movimiento.ValorUNI.Value = 0;
            footerDet.ValorBruto = 0;
            footerDet.ValorIVA = 0;
            footerDet.ValorTotal = 0;
            footerDet.ValorNeto = 0;
            footerDet.ValorOtros = 0;
            footerDet.ValorRetenciones = 0;
            footerDet.ValorRFT = 0;
            footerDet.ValorRICA = 0;
            footerDet.ValorRIVA = 0;
            #endregion

            this.data.Footer.Add(footerDet);
            this.gvDocument.RefreshData();
            this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

            this.isValid = false;

            this.Servicio = null;

            if (!string.IsNullOrEmpty(footerDet.Movimiento.ServicioID.Value))
                this.Servicio = (DTO_faServicios)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, footerDet.Movimiento.ServicioID.Value, true);

            if (!string.IsNullOrEmpty(footerDet.Movimiento.TerceroID.Value))
                this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, footerDet.Movimiento.TerceroID.Value, true);
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected virtual bool AsignarTasaCambio(bool fromTop) { return true; }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            if (!this.disableValidate)
            {
                try
                {   
                    #region Asignacion de Valores
                    this.masterServicio.Value = this._rowCurrent.Movimiento.ServicioID.Value;// ;
                    this.masterBodega.Value = this._rowCurrent.Movimiento.BodegaID.Value;//;
                    this.masterLineaPresup.Value = this._rowCurrent.Movimiento.LineaPresupuestoID.Value;//;
                    this.masterReferencia.Value = this._rowCurrent.Movimiento.inReferenciaID.Value;//;
                    this.masterParametro1.Value = this._rowCurrent.Movimiento.Parametro1.Value;//;
                    this.masterTareaProy.Value = this._rowCurrent.Movimiento.TareaID.Value;//;
                    this.masterParametro2.Value = this._rowCurrent.Movimiento.Parametro2.Value;//;
                    this.txtPlaqueta.Text = this._rowCurrent.Movimiento.PlaquetaID.Value;//;
                    this.masterEmpaque.Value = this._rowCurrent.Movimiento.EmpaqueInvID.Value;//;
                    this.txtCantEmpaque.EditValue = this._rowCurrent.Movimiento.CantidadEMP.Value;//;
                    this.txtCantidad.EditValue = this._rowCurrent.Movimiento.CantidadUNI.Value;//;
                    this.txtDescr.Text = this._rowCurrent.Movimiento.DescripTExt.Value;//;
                    this.cmbEstado.SelectedItem = this._rowCurrent.Movimiento.EstadoInv.Value != null ? this.cmbEstado.GetItem(this._rowCurrent.Movimiento.EstadoInv.Value.ToString()) : this.cmbEstado.GetItem(((byte)EstadoInv.Activo).ToString());
                    if (!string.IsNullOrEmpty(this._rowCurrent.Movimiento.SerialID.Value))
                    {
                        this.cmbSerialDisp.Items.Add(this._rowCurrent.Movimiento.SerialID.Value);
                        this.cmbSerialDisp.SelectedItem = this._rowCurrent.Movimiento.SerialID.Value;
                    }
                    else
                        this.cmbSerialDisp.Text = this._rowCurrent.Movimiento.SerialID.Value;

                    #endregion

                    if (this.newDoc)
                    {
                        this.EnableFooter(false);
                        this.newDoc = false;
                        this.masterServicio.EnableControl(true);
                        this.masterTareaProy.EnableControl(this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false);
                        this.btnTareas.Enabled = this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.masterServicio.Value))
                            this.Servicio = (DTO_faServicios)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, this.masterServicio.Value, true);

                        this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._rowCurrent.Movimiento.TerceroID.Value, true);

                        if (!string.IsNullOrEmpty(this.masterBodega.Value))
                            this.Bodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodega.Value, true);

                        if (!string.IsNullOrEmpty(this.masterReferencia.Value))
                            this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);

                        if (!string.IsNullOrEmpty(this.masterEmpaque.Value))
                            this.Empaque = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, this.masterEmpaque.Value, true);

                        if (this._rowCurrent.Movimiento.ActivoID.Value != null)
                            this.Activo = _bc.AdministrationModel.acActivoControl_GetByID(this._rowCurrent.Movimiento.ActivoID.Value.Value);

                        if (this.data.Header.DocumentoREL != null || this.data.Header.DocumentoREL.Value.Value == 0)
                        {
                            //this.EnableFooter(false);
                            this.btnNotaEnv.Enabled = true;
                            this.gvDocument.Columns[this.unboundPrefix + "TerceroID"].OptionsColumn.AllowEdit = true;
                        }
                    }
                    this.CalcularTotal();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "LoadEditGridData"));
                }
            }
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected virtual decimal LoadTasaCambio(int monOr, DateTime fecha)
        {
            try
            {
                decimal valor = 0;
                string tasaMon = this.monedaId;
                if (monOr == (int)TipoMoneda.Local)
                    tasaMon = this.monedaExtranjera;

                valor = _bc.AdministrationModel.TasaDeCambio_Get(tasaMon, fecha);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "LoadTasaCambio"));
                return 0;
            }
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            //if (this.gvDocument.DataRowCount == 0)
            //    enable = false;

            //Campos de maestras
            this.masterServicio.EnableControl(enable);
            this.masterBodega.EnableControl(enable);
            this.masterLineaPresup.EnableControl(enable);
            this.masterReferencia.EnableControl(enable);
            this.btnReferencia.Enabled = enable;
            this.masterParametro1.EnableControl(enable);
            this.masterParametro2.EnableControl(enable);
            this.masterEmpaque.EnableControl(enable);
            this.masterTareaProy.EnableControl(this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false);
            this.btnTareas.Enabled = this.facturaTipo != null && this.facturaTipo.TipoControlInventarios.Value == 2 ? true : false;
            //Campos de texto
            this.txtCantidad.Enabled = enable;
            this.txtDescr.Enabled = true;
            this.cmbSerialDisp.Enabled = enable;
            this.txtPlaqueta.Enabled = false;
            this.txtCantEmpaque.Enabled = enable;

            //Otros
            this.btnNotaEnv.Enabled = enable;
            this.cmbEstado.Enabled = enable;
            if (this.facturaTipo != null)
                this.btnDescargaInventario.Enabled = this.facturaTipo.TipoControlInventarios.Value == 1 ? false : true;
            else
                this.btnDescargaInventario.Enabled = false;

            this.txtValorAdmin.Visible = false;
            this.txtValorImprev.Visible = false;
            this.txtValorUtil.Visible = false;
            this.lblAdmin.Visible = false;
            this.lblImprev.Visible = false;
            this.lblUtil.Visible = false;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del footer
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanFooter(bool basic)
        {
            if (basic)
            {
                this.masterServicio.Value = string.Empty;
            }
            this.masterBodega.Value = string.Empty;
            this.masterLineaPresup.Value = string.Empty;
            this.masterReferencia.Value = string.Empty;
            this.cmbEstado.SelectedItem = this.cmbEstado.GetItem(((byte)EstadoInv.Activo).ToString());
            this.masterParametro1.Value = string.Empty;
            this.masterParametro2.Value = string.Empty;
            this.masterTareaProy.Value = string.Empty;
            this.cmbSerialDisp.SelectedIndex = -1;
            this.txtPlaqueta.Text = string.Empty;
            this.masterEmpaque.Value = string.Empty;
            this.txtCantEmpaque.EditValue = 0;
            this.txtCantidad.EditValue = 0;
            this.txtDescr.Text = string.Empty;

            this.txtValorAdmin.EditValue = 0;
            this.txtValorImprev.EditValue = 0;
            this.txtValorUtil.EditValue = 0;
            this.txtValorAIU.EditValue = 0;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanHeader(bool basic)
        {
            if (this.dtPeriod.DateTime.Month == DateTime.Now.Month)
                base.dtFecha.DateTime = DateTime.Now;

            this.validHeader = false;
            this.ValidHeaderTB();

            this.CalcularTotal();
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected virtual void CalcularTotal()
        {
            try
            {
                decimal sumBruto =0;
                decimal sumAIU = 0;
                decimal sumIva = 0; 
                decimal sumRiva = 0; 
                decimal sumRica = 0; 
                decimal sumRft = 0;
                decimal sumRteGarantia = 0;               
                decimal sumRetenciones = 0;
                decimal sumNeto = 0;

                if (this.data != null && this.data.Footer.Count > 0)
                {
                    this.GetAIUProyecto(this.data.DocCtrl.ProyectoID.Value);
                    sumBruto = this.data.Footer.Sum(x => x.ValorBruto);
                    if(!this.data.Header.FacturaTipoID.Value.Equals(this._tipoFacturaCtaCobro))
                    {
                        sumAIU = this.data.Header.Administracion.Value.Value + this.data.Header.Imprevistos.Value.Value + this.data.Header.Utilidad.Value.Value;
                        sumIva = !this.IVAUtilidad ? this.data.Footer.Sum(x => x.ValorIVA) : this.data.Header.Iva.Value.Value;
                        sumRiva = this.data.Footer.Sum(x => x.ValorRIVA);
                        sumRica = this.data.Footer.Sum(x => x.ValorRICA);
                        sumRft = this.data.Footer.Sum(x => x.ValorRFT);
                        sumRteGarantia = this.data.Header.ValorRteGarantia;
                        sumRetenciones = this.data.Footer.Sum(x => x.ValorRetenciones) + this.data.Header.ValorRteGarantia;
                    }
                    sumNeto = (sumBruto + sumAIU + sumIva) - this.data.Header.ValorAnticipo - sumRetenciones; 
                }                

                this.txtValorBruto.EditValue = sumBruto;
                this.txtValorAIU.EditValue = sumAIU;
                this.txtValorIVA.EditValue = !this.IVAUtilidad ? sumIva : this.data.Header.Iva.Value;
                this.txtValorRIVA.EditValue = sumRiva;
                this.txtValorRICA.EditValue = sumRica;
                this.txtValorRFT.EditValue = sumRft;
                this.txtValorRteGarantia.EditValue = sumRteGarantia;
                this.txtValorRetenciones.EditValue = sumRetenciones;
                this.txtValorNeto.EditValue = sumNeto;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "CalcularTotal"));
            }
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected virtual void EnableHeader(bool enable) { }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected virtual DTO_faFacturacion LoadTempHeader() { return null; }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                GridColumn col = new GridColumn();
                #region Validacion de nulls y Fks
                #region Codigo Servicio
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ServicioID", false, true, true, AppMasters.faServicios);
                if (!validField)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Código Servicio vacío"));
                    validRow = false;
                }
                #endregion
                #region Bodega
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "BodegaID", (this._tipoConcepto == TipoConcepto.VentaInv) ? false : true, true, false, AppMasters.inBodega);
                if (!validField)
                    validRow = false;
                #endregion
                #region Referencia
                if (this._tipoConcepto == TipoConcepto.VentaInv)
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "inReferenciaID", false, true, false, AppMasters.inReferencia);
                    if (!validField)
                        validRow = false;
                }
                #endregion
                #region EstadoInv
                if (this._tipoConcepto == TipoConcepto.VentaInv)
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "EstadoInv", false, false, false, null);
                    if (!validField)
                        validRow = false;
                }
                #endregion
                #region SerialID
                if (this._tipoConcepto == TipoConcepto.VentaInv)
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "SerialID",
                        (this._tipoRef == null || !this._tipoRef.SerializadoInd.Value.Value) ? true : false, false, false, null);
                    if (!validField)
                        validRow = false;
                }
                #endregion
                #region Parametro1
                if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive)
                {
                    if (_pksPar1 == null)
                        validField = false;
                    else
                        if (this._pksPar1["Parametro1ID"] != this._parametro1xDef)
                            validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, fila, "Parametro1", this._pksPar1, (this.TipoRef == null || this.TipoRef.Parametro1Ind.Value == null || this.TipoRef.Parametro1Ind.Value.Value) ? false : true, AppMasters.inTipoParameter1);
                    if (!validField)
                        validRow = false;
                }
                #endregion
                #region Parametro2
                if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive)
                {
                    if (_pksPar2 == null)
                        validField = false;
                    else
                        if (this._pksPar2["Parametro2ID"] != this._parametro2xDef)
                            validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, fila, "Parametro2", this._pksPar2, (this._tipoConcepto != TipoConcepto.VentaInv) ? true : (this.TipoRef == null || this.TipoRef.Parametro2Ind.Value == null || this.TipoRef.Parametro2Ind.Value.Value) ? false : true, AppMasters.inTipoParameter2);
                    if (!validField)
                        validRow = false;
                }
                #endregion
                #region DescripTExt
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "DescripTExt", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion                
                #region DocSoporteTER (DocSoporte)
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "DocSoporteTER", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region Tercero
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "TerceroID", false, true, false, AppMasters.coTercero);
                if (!validField)
                    validRow = false;
                #endregion
                #region Proyecto
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProyectoID", false, true, true, AppMasters.coProyecto);
                if (!validField)
                    validRow = false;
                #endregion
                #region Centro Costo
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                if (!validField)
                    validRow = false;
                #endregion
                #region Linea PResupuesto
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "LineaPresupuestoID", true, true, false, AppMasters.plLineaPresupuesto);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de valores
                #region ValorUNI
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorUNI", false, true, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region CantidadUNI
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadUNI", false, false, true, false);
                if (!validField)
                    validRow = false;
                else
                    if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive && this.data.DocCtrl.Estado.Value != 3 && (this.data.Header.DocumentoREL == null || this.data.Header.DocumentoREL.Value.Value == 0))
                    {
                        string rsxMsg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory);
                        string msg = string.Empty;
                        col = this.gvDocument.Columns[this.unboundPrefix + "CantidadUNI"];
                        decimal colVal = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, col));

                        this.GetSaldoDisponible(fila);

                        if (colVal > this._saldoDisponible)
                        {
                            msg = rsxMsg;
                            validRow = false;
                        }

                        this.gvDocument.SetColumnError(col, msg);
                    }
                #endregion
                #region CantidadEMP
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadEMP",true,true, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                if (validRow)
                {
                    this.isValid = true;
                    this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "ValidateRow"));
            }

            this.hasChanges = true;
            return validRow;
        }

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (this.data.Footer != null && this.data.Footer.Count == 0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }

            if (!this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected override void ValidHeaderTB()
        {
            if (this.validHeader)
            {
                //Habilita el boton de salvar
                if (SecurityManager.HasAccess(this.documentID, FormsActions.Add) || SecurityManager.HasAccess(this.documentID, FormsActions.Edit))
                    FormProvider.Master.itemSave.Enabled = this.ctrl != null && (this.ctrl.Estado.Value == (byte)EstadoDocControl.SinAprobar || this.ctrl.Estado.Value == (byte)EstadoDocControl.ParaAprobacion) ?  true : false;
                else
                    FormProvider.Master.itemSave.Enabled = false;

                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
            else
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected virtual bool ValidateHeader() { return true; }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanSave(int monOr) { return false; }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(DTO_faFacturacion aux) { }

        /// <summary>
        /// Obtiene el AIU del proyecto
        /// </summary>
        protected string GetAIUProyecto(string proyectoID)
        {
            try
            {
                if (this.data != null && !this.data.Header.FacturaTipoID.Value.Equals(this._tipoFacturaCtaCobro))
                {
                    DTO_SolicitudTrabajo solicitud = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, string.Empty, null, null, string.Empty, this.data.DocCtrl.ProyectoID.Value, false, false, false, false, false);

                    if (solicitud != null && solicitud.HeaderProyecto.APUIncluyeAIUInd.Value.Value)
                    {
                        this.IVAUtilidad = true;
                        this.data.Header.PorcAdministracion = solicitud.HeaderProyecto.PorEmpresaADM.Value.Value;
                        this.data.Header.PorcImprevistos = solicitud.HeaderProyecto.PorEmpresaIMP.Value.Value;
                        this.data.Header.PorcUtilidad = solicitud.HeaderProyecto.PorEmpresaUTI.Value.Value;

                        this.data.Header.Administracion.Value = Math.Round(this.data.Footer.Sum(x => x.ValorBruto) * (this.data.Header.PorcAdministracion / 100), 0);
                        this.data.Header.Imprevistos.Value = Math.Round(this.data.Footer.Sum(x => x.ValorBruto) * (this.data.Header.PorcImprevistos / 100), 0);
                        this.data.Header.Utilidad.Value = Math.Round(this.data.Footer.Sum(x => x.ValorBruto) * (this.data.Header.PorcUtilidad / 100), 0);
                        this.data.Header.Iva.Value = solicitud.HeaderProyecto.PorIVA.Value.HasValue?  Math.Round(this.data.Header.Utilidad.Value.Value * (solicitud.HeaderProyecto.PorIVA.Value.Value / 100), 0) : 0;

                        this.txtValorAdmin.EditValue = this.data.Header.Administracion.Value;
                        this.txtValorImprev.EditValue = this.data.Header.Imprevistos.Value;
                        this.txtValorUtil.EditValue = this.data.Header.Utilidad.Value;
                        this.txtValorAIU.EditValue = this.data.Header.Administracion.Value + this.data.Header.Imprevistos.Value + this.data.Header.Utilidad.Value;

                        this.txtValorAdmin.Visible = true;
                        this.txtValorImprev.Visible = true;
                        this.txtValorUtil.Visible = true;
                        this.lblAdmin.Visible = true;
                        this.lblImprev.Visible = true;
                        this.lblUtil.Visible = true;

                    }
                    else
                    {
                        this.data.Header.Administracion.Value = 0;
                        this.data.Header.Imprevistos.Value = 0;
                        this.data.Header.Utilidad.Value = 0;
                        this.txtValorAdmin.EditValue = 0;
                        this.txtValorImprev.EditValue = 0;
                        this.txtValorUtil.EditValue = 0;
                        this.txtValorAIU.EditValue = 0;

                        this.txtValorAdmin.Visible = false;
                        this.txtValorImprev.Visible = false;
                        this.txtValorUtil.Visible = false;
                        this.lblAdmin.Visible = false;
                        this.lblImprev.Visible = false;
                        this.lblUtil.Visible = false;
                        this.IVAUtilidad = false;
                    }
                }
                return proyectoID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetAIUProyecto"));
                return proyectoID;
            }
        }


        #endregion

        #region Eventos del MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.tbBreak0.Enabled = false;
                FormProvider.Master.itemGenerateTemplate.Enabled = false;
                FormProvider.Master.itemCopy.Enabled = false;
                FormProvider.Master.itemPaste.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;
                FormProvider.Master.itemExport.Enabled = false;
                FormProvider.Master.tbBreak1.Enabled = false;
                FormProvider.Master.itemRevert.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemFilter.Enabled = false;
                FormProvider.Master.itemFilterDef.Enabled = false;

                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        #endregion
        
        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.validHeader)
                {
                    if (this.data == null)
                    {
                        this.gcDocument.Focus();
                        e.Handled = true;
                    }

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        if (this.gvDocument.ActiveFilterString != string.Empty)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                        else
                        {
                            this.deleteOP = false;
                            if (this.isValid)
                            {
                                this.newReg = true;
                                this.AddNewRow();
                                //EnableFooter(true);

                                this.masterServicio.Focus();
                            }
                            else
                            {
                                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                if (isV)
                                {
                                    this.newReg = true;
                                    this.AddNewRow();
                                    //EnableFooter(true);
                                }
                            }
                        }
                    }
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.deleteOP = true;
                            int rowHandle = this.gvDocument.FocusedRowHandle;

                            if (this.data.Footer.Count == 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                e.Handled = true;
                            }
                            else
                            {
                                this.data.Footer.RemoveAll(x => x.Index == this.indexFila);
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvDocument.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvDocument.FocusedRowHandle = rowHandle - 1;

                                this.UpdateTemp(this.data);

                                this.gvDocument.RefreshData();
                                this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
                this.editValue.Mask.EditMask = "n2";
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (fieldName == "ValorUNI" || fieldName == "ValorBruto" || fieldName == "ValorIVA" || fieldName == "ValorTotal" || fieldName == "CantidadUNI")
                    e.RepositoryItem = this.editSpin;

                if (fieldName == "CantidadUNI")
                    e.RepositoryItem = this.editValue;

                if (fieldName == "ProyectoID" || fieldName == "CentroCostoID" || fieldName == "TerceroID" || fieldName == "LineaPresupuestoID")
                    e.RepositoryItem = this.editBtnGrid;          
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName != "Marca")
                {
                    int index = this.NumFila;

                    bool validField = true;

                    #region Se modifican FKs
                    if (fieldName == "ServicioID")
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.faServicios);
                    if (fieldName == "CentroCostoID")
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                    if (fieldName == "LineaPresupuestoID")
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, true, true, false, AppMasters.plLineaPresupuesto);
                    if (fieldName == "ProyectoID")
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coProyecto);
                    if (fieldName == "TerceroID")
                    {
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.coTercero);
                        if (validField)
                        {
                            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
                            string tercID = this.gvDocument.GetRowCellValue(e.RowHandle, col).ToString();
                            if (this.Tercero == null || this.Tercero.ID.Value != tercID)
                                this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, tercID, true);

                            this.GetRetenciones(false);
                            this.LoadValuesDet(false);
                        }
                    }
                    #endregion
                    #region Se modifican ValorUNI o CantidadUNI
                    if (fieldName == "ValorUNI" || fieldName == "CantidadUNI")
                    {
                        if (this._rowCurrent.Movimiento.ValorUNI.Value == null)
                            this._rowCurrent.Movimiento.ValorUNI.Value = 0;

                        if (this._rowCurrent.Movimiento.CantidadUNI.Value == null)
                            this._rowCurrent.Movimiento.CantidadUNI.Value = 0;

                        if (Convert.ToDecimal(this.txtCantidad.EditValue, CultureInfo.InvariantCulture) != this._rowCurrent.Movimiento.CantidadUNI.Value.Value)
                            this.txtCantidad.EditValue = this._rowCurrent.Movimiento.CantidadUNI.Value.Value;

                        this.LoadValuesDet(false);
                        this.data.Header.PorcRteGarantia.Value = this.data.Header.PorcRteGarantia.Value ?? 0;
                        if (this.data != null  && this.data.Header.RteGarantiaIvaInd.Value.Value)
                            this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorNeto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                        else
                            this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                        this.txtValorRteGarantia.EditValue = this.data.Header.ValorRteGarantia;
                        this.CalcularTotal();

                        validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, false, false);
                    }
                    if (fieldName == "CantidadEMP")
                    {
                        if (this._rowCurrent.Movimiento.CantidadEMP.Value == null)
                            this._rowCurrent.Movimiento.CantidadEMP.Value = 0;

                        if (Convert.ToDecimal(this.txtCantEmpaque.EditValue, CultureInfo.InvariantCulture) != this._rowCurrent.Movimiento.CantidadEMP.Value.Value)
                            this.txtCantEmpaque.EditValue = this._rowCurrent.Movimiento.CantidadEMP.Value.Value;

                        //this.CalcularValores();

                        validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, false, false);

                        this._rowCurrent.Movimiento.CantidadUNI.Value = this._rowCurrent.Movimiento.CantidadEMP.Value.Value * this._converFactor;
                        this.txtCantidad.EditValue = this._rowCurrent.Movimiento.CantidadUNI.Value.Value;
                        this.gvDocument.SetRowCellValue(index, this.unboundPrefix + "CantidadUNI", this._rowCurrent.Movimiento.CantidadUNI.Value);

                        this.GetSaldoDisponible(index);
                        this.data.Header.PorcRteGarantia.Value = this.data.Header.PorcRteGarantia.Value ?? 0;
                        if (this.data != null && this.data.Header.RteGarantiaIvaInd.Value.Value)
                            this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x=>x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorNeto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                        else
                            this.data.Header.ValorRteGarantia = Math.Round(this.data.Footer.Where(x => x.Movimiento.ImprimeInd.Value.Value).Sum(x => x.ValorBruto) * (this.data.Header.PorcRteGarantia.Value.Value / 100), 0);
                        this.txtValorRteGarantia.EditValue = this.data.Header.ValorRteGarantia;
                        this.CalcularTotal();
                    }

                    #endregion
                    #region Se modifican Otros
                    if (fieldName == "DocSoporteTER" || fieldName == "DescripTExt")
                        validField = _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, fieldName, false, false, false, null);
                    #endregion

                    //this.RowEdited = true;
                    if (!validField)
                        this.isValid = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    if (this.ctrl != null && this.ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                        FormProvider.Master.itemSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.disableValidate && e.RowHandle >= 0)
            {
                bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                this.deleteOP = false;

                if (validRow)
                {
                    this.isValid = true;
                }
                else
                {
                    e.Allow = false;
                    this.isValid = false;
                }
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (this.documentID != AppDocuments.FacturaVenta && this.documentID != AppDocuments.NotaCredito)
                base.gvDocument_CustomUnboundColumnData(sender, e);
            else
            {
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    if (fieldName == "Marca" && e.Value == null)
                        e.Value = this.select.Contains(e.ListSourceRowIndex);
                    else
                    {
                        PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal")
                                e.Value = pi.GetValue(dto, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                        else
                        {
                            FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || fi.FieldType.Name == "Decimal")
                                    e.Value = fi.GetValue(dto);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                            else
                            {
                                DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                                pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (pi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Decimal")
                                        e.Value = pi.GetValue(dtoM.Movimiento, null);
                                    else
                                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                                }
                                else
                                {
                                    fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                    if (fi != null)
                                    {
                                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || fi.FieldType.Name == "Decimal")
                                            e.Value = fi.GetValue(dtoM.Movimiento);
                                        else
                                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                                    }
                                }
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
                            else
                            {
                                DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                                pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (pi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                        e.Value = pi.GetValue(dtoM.Movimiento, null);
                                    else
                                    {
                                        UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                                else
                                {
                                    fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                    if (fi != null)
                                    {
                                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                        {
                                            //e.Value = pi.GetValue(dto, null);
                                        }
                                        else
                                        {
                                            UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                            udtProp.SetValueFromString(e.Value.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            bool editableCell = true;
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

            #region Validacion de valores
            if (fieldName == "ValorBruto" || fieldName == "ValorIVA" || fieldName == "ValorTotal")
                    editableCell = false;       
            #endregion
            #region Validacion de otros campos
            if (fieldName == "inReferenciaID" && this._tipoConcepto != TipoConcepto.VentaInv)
                editableCell = false;     
            #endregion

            if (editableCell)
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.White;
            else
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.Lavender;
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowCurrent = (DTO_faFacturacionFooter)this.gvDocument.GetRow(e.FocusedRowHandle);
                    this.newReg = false;
                    this.indexFila = this._rowCurrent.Index;
                    this.LoadEditGridData(false, e.FocusedRowHandle);
                    this.isValid = true;
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true;
                }
                else
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "gvDocument_FocusedRowChanged"));
            }
        }


        #endregion

        #region Eventos Detalle (footer)

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterDetails_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0)
                {
                    ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
                    GridColumn col = new GridColumn();
                    int index = this.NumFila;
                    switch (master.ColId)
                    {
                        case "ServicioID":
                            #region Codigo Servicio
                            if (master.ValidID)
                            {                                
                                if (this.Servicio == null || master.Value != this.Servicio.ID.Value)
                                {                     
                                    this.Servicio = (DTO_faServicios)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, master.Value, true);
                                    
                                    this._rowCurrent.Movimiento.ServicioID.Value = master.Value;
                                    this._rowCurrent.Movimiento.ValorUNI.Value = 0;

                                    this.CleanFooter(false);
                                    if (this.Servicio.DescrVariableInd.Value.Value)
                                        this.txtDescr.Text = this.Servicio.DescVariable.Value;
                                    else
                                        this.txtDescr.Text = this.Servicio.Descriptivo.Value;

                                    this._rowCurrent.Movimiento.DescripTExt.Value = this.txtDescr.Text;
                                    this.masterBodega.Focus();

                                }

                                if (this.Tercero != null)
                                    this.GetRetenciones(false);
                            }
                            else
                            {
                                this.Servicio = null;
                                //this.masterServicio.Focus();
                            }
                            _bc.ValidGridCell(this.gvDocument, string.Empty, index, "ServicioID", false, true, true, AppMasters.faServicios);                                    
                            #endregion
                            break;
                        case "BodegaID":
                            #region Codigo Bodega
                            //this._bodegaFocused = true;
                            if (master.ValidID)
                            {
                                if (this.Bodega == null || master.Value != this.Bodega.ID.Value)
                                {
                                    //this._bodegaFocused = false;
                                    this._rowCurrent.Movimiento.BodegaID.Value = master.Value;
                                    this.Bodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                                    string proyectoAdmin = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                                    if (this.Bodega != null && !proyectoAdmin.Equals(this.Bodega.ProyectoID.Value))
                                    {
                                        MessageBox.Show("No puede seleccionar una bodega de proyecto controlado para hacer salidas");
                                        this.Bodega = null;
                                        this.masterBodega.Value = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive)
                                    this.Bodega = null;
                            }
                             _bc.ValidGridCell(this.gvDocument, string.Empty, index, "BodegaID",
                                (this._tipoConcepto != TipoConcepto.VentaInv) ? true : false, true, false, AppMasters.inBodega);
                            #endregion
                            break;
                        case "inReferenciaID":
                            #region Codigo Referencia
                            if (master.ValidID)
                            {
                                if (this.Referencia == null || master.Value != this.Referencia.ID.Value)
                                {
                                    //this._referFocused = false;

                                    this._rowCurrent.Movimiento.Parametro1.Value = string.Empty;
                                    this._rowCurrent.Movimiento.Parametro2.Value = string.Empty;
                                    this.masterParametro1.Value = string.Empty;
                                    this.masterParametro2.Value = string.Empty; 

                                    this._rowCurrent.Movimiento.inReferenciaID.Value = master.Value;
                                    this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);

                                    this._rowCurrent.Movimiento.CantidadEMP.Value = this._cantidadEmp;
                                    if (this.Referencia != null)
                                        this.txtDescr.Text = this.Referencia.Descriptivo.Value;

                                    this._rowCurrent.Movimiento.DescripTExt.Value = this.txtDescr.Text;
                                    this.gvDocument.SetRowCellValue(index, this.unboundPrefix + "CantidadEMP", this._cantidadEmp);
                                }
                            }
                            else
                            {
                                if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive)
                                    this.Referencia = null;
                                //this.masterReferencia.Focus();
                            }
                            _bc.ValidGridCell(this.gvDocument, string.Empty, index, "inReferenciaID", 
                                (this._tipoConcepto != TipoConcepto.VentaInv) ? true : false, true, false, AppMasters.inReferencia);
                            
                            #endregion
                            break;
                        case "EmpaqueInvID":
                            #region Codigo Empaque
                            if (master.ValidID)
                            {
                                if (this.Empaque == null || master.Value != this.Empaque.ID.Value || this._converFactor == 0)
                                {
                                    this.Empaque = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, master.Value, true);

                                    this._rowCurrent.Movimiento.CantidadEMP.Value = this._cantidadEmp;
                                    this.gvDocument.SetRowCellValue(index, this.unboundPrefix + "CantidadEMP", this._cantidadEmp);
                                }
                            }
                            else
                            {
                                if (this._tipoConcepto == TipoConcepto.VentaInv && this.moduleInventarioActive)
                                    this.Empaque = null;
                                //this.masterEmpaque.Focus();
                            }
                            _bc.ValidGridCell(this.gvDocument, string.Empty, index, "EmpaqueInvID", 
                                (this._tipoConcepto != TipoConcepto.VentaInv) ? true : false, true, false, AppMasters.inEmpaque);
                            #endregion
                            break;
                        case "Parametro1ID":
                            #region Codigo Parametro 1
                            if (master.ValidID)
                            {
                                this._rowCurrent.Movimiento.Parametro1.Value = master.Value;
                                this._pksPar1["Parametro1ID"] = master.Value;
                                this.GetSaldoDisponible(index);
                            }
                            else
                            {
                                //this.masterParametro1.Focus();
                            }
                            bool validPar1 = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, index, "Parametro1", _pksPar1,
                                (this._tipoConcepto != TipoConcepto.VentaInv) ? true :
                                (this.TipoRef == null || this.TipoRef.Parametro1Ind.Value == null || this.TipoRef.Parametro1Ind.Value.Value) ? false : true, AppMasters.inTipoParameter1);
                            #endregion
                            break;
                        case "Parametro2ID":
                            #region Codigo Parametro 2
                            if (master.ValidID)
                            {
                                this._rowCurrent.Movimiento.Parametro2.Value = master.Value;
                                this._pksPar2["Parametro2ID"] = master.Value;
                                this.GetSaldoDisponible(index);
                            }
                            else
                            {
                                //this.masterParametro2.Focus();
                            }
                            bool validPar2 = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, index, "Parametro2", _pksPar2,
                                (this._tipoConcepto != TipoConcepto.VentaInv) ? true :
                                (this.TipoRef == null || this.TipoRef.Parametro1Ind.Value == null || this.TipoRef.Parametro1Ind.Value.Value) ? false : true, AppMasters.inTipoParameter2);

                            //if (!validPar2)
                            //    this.masterParametro2.BackColor = Color.LightSalmon;
                            //else
                            //    this.masterParametro2.BackColor = Color.Transparent;
                            #endregion
                            break;
                    }

                    this.gvDocument.RefreshData();
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    if (this.ctrl != null && this.ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                        FormProvider.Master.itemSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "masterDetails_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtControl_Leave(object sender, EventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            GridColumn col = new GridColumn();
            int index = this.NumFila;

            switch (ctrl.Name)
            {
                //case "txtBodega":
                //    this._rowCurrent.Movimiento.BodegaID.Value = ctrl.Text;
                //    break;
                //case "txtReferencia":
                //    this._rowCurrent.Movimiento.inReferenciaID.Value = ctrl.Text;
                //    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "inReferenciaID", false, false, false, null);
                //    break;
                //case "txtEstado":
                //    this._rowCurrent.Movimiento.EstadoInv.Value = Convert.ToByte(ctrl.Text);
                //    break;
                //case "txtParameter1":
                //    this._rowCurrent.Movimiento.Parametro1.Value = ctrl.Text;
                //    break;
                //case "txtParameter2":
                //    this._rowCurrent.Movimiento.Parametro2.Value = ctrl.Text;
                //    break;
                //case "txtIdentificadorTr":
                //    this._rowCurrent.Movimiento.IdentificadorTr.Value = Convert.ToInt32(ctrl.Text);
                //    break;
                //case "txtSerial":
                //    this._rowCurrent.Movimiento.SerialID.Value = ctrl.Text;
                //    break;
                //case "txtEmpaque":
                //    this._rowCurrent.Movimiento.EmpaqueInvID.Value = ctrl.Text;
                //    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "EmpaqueInvID", false, false, false, null);
                //    break;
                //case "txtCantEmpaque":
                //    this._rowCurrent.Movimiento.CantidadEMP.Value = Convert.ToInt32(ctrl.Text);
                //    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "CantidadEMP", false, false, false, null);
                //    break;
                //case "txtCantidad":
                //    if (string.IsNullOrEmpty(this.txtCantidad.Text.Trim()))
                //       this.txtCantidad.EditValue = 0;
                //    this._rowCurrent.Movimiento.CantidadUNI.Value = Convert.ToInt32(ctrl.Text);
                //    this.CalcularValores();
                //    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "CantidadUNI", false, false, false, null);
                //    if (this.data.Header.Valor.Value.Value > 0 && this._indContabilizaRetencionFactura)
                //    {
                //        this.txtValorRICA.Enabled = true;
                //        this.txtValorRemesa.Enabled = true;
                //    };                    
                //    break;                    
                case "txtPlaqueta":
                    //this._rowCurrent.Plaqueta = ctrl.Text;
                    //if (this.Activo == null || ctrl.Text != this.Activo.PlaquetaID.Value)
                    //{
                    //    this.Activo = this._bc.AdministrationModel.acActivoControl_GetByPlaqueta(this._rowCurrent.Plaqueta);
                    //    this._rowCurrent.Movimiento.ActivoID.Value = this._activoID;
                    //}
                    break;
                case "txtDescr":
                    this._rowCurrent.Movimiento.DescripTExt.Value = ctrl.Text;
                    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "DescripTExt", false, false, false, null);
                    break;
            }

            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtEditControl_Leave(object sender, EventArgs e)
        {
            int index = this.NumFila;
            TextEdit ctrl = (TextEdit)sender;
            int count;
            try
            {
                switch (ctrl.Name)
                {
                    case "txtValorRICA":
                        #region Valor RICA
                        if (Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) <= 0)
                        {
                            decimal porcIca = Convert.ToDecimal(this.txtValorRICA.EditValue, CultureInfo.InvariantCulture) / this.data.Header.Valor.Value.Value * 1000 * (-1);
                            count = this.data.Footer.Count;
                            for (int i = 0; i < count; i++)
                            {
                                this.data.Footer[i].ValorNeto = this.data.Footer[i].ValorNeto - this.data.Footer[i].ValorRetenciones;
                                this.data.Footer[i].ValorRetenciones = this.data.Footer[i].ValorRetenciones - this.data.Footer[i].ValorRICA;
                                //this.data.Footer[i].ValorRICA = this.data.Footer[i].ValorRICA / this.data.Header.Porcentaje1.Value.Value * porcIca;
                                this.data.Footer[i].ValorRICA = Math.Round(this.data.Footer[i].ValorBruto * (porcIca / 1000), 0) * (-1);
                                this.data.Footer[i].ValorRetenciones = this.data.Footer[i].ValorRetenciones + this.data.Footer[i].ValorRICA;
                                this.data.Footer[i].ValorNeto = this.data.Footer[i].ValorNeto + this.data.Footer[i].ValorRetenciones;
                            }
                            this.data.Header.Porcentaje1.Value = porcIca;
                            this.gcDocument.RefreshDataSource();
                            this.CalcularTotal();
                        }
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_InvalidNumberPositive));
                            ctrl.EditValue = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) * -1;
                            //ctrl.Focus();
                        }
                        #endregion
                        break;
                    case "txtValorAnticipoDet":
                        #region Valor Anticipos
                        decimal vlrAntNew = Convert.ToDecimal(this.txtValorAnticipoDet.EditValue, CultureInfo.InvariantCulture);
                        if (vlrAntNew > this._vlrAnticipoTerc)
                        {
                            if (MessageBox.Show("Este valor de anticipo no está contabilizado, si desea  continuar no olvide registrarlo en el sistema. Continuar?", "Registro de anticipos de terceros", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.data.Header.ValorAnticipo = vlrAntNew;
                                this.data.Header.Retencion10.Value = vlrAntNew;
                                this.CalcularTotal();
                            }
                            else
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "El valor no puede ser mayor al Saldo de anticipos"));
                                this.txtValorAnticipoDet.EditValue = this._vlrAnticipoTerc;
                            }
                        }
                        else
                        {
                            this.data.Header.ValorAnticipo = vlrAntNew;
                            this.data.Header.Retencion10.Value = vlrAntNew;
                            this.CalcularTotal();
                        }
                        //this.txtValorAnticipoDet.EditValue = this.GetSaldoProyecto();
                        #endregion
                        break;
                    case "txtCantEmpaque":
                        #region Cantidad Empaque
                        if (string.IsNullOrEmpty(this.txtCantEmpaque.Text.Trim()))
                            this.txtCantEmpaque.EditValue = 0;
                        this.txtCantidad.EditValue = 0;
                        this._cantidadEmp = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                        this._rowCurrent.Movimiento.CantidadEMP.Value = this._cantidadEmp;

                        this.gvDocument.SetRowCellValue(index, this.unboundPrefix + "CantidadEMP", this._cantidadEmp);
        
                        _bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "CantidadEMP", false, false, true, false);
                        #endregion
                        break;
                    case "txtCantidad":
                        #region Cantidad Unit
                        if (string.IsNullOrEmpty(this.txtCantidad.Text.Trim()))
                            this.txtCantidad.EditValue = 0;
                        this._rowCurrent.Movimiento.CantidadUNI.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                        this.LoadValuesDet(false);
                        _bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "CantidadUNI", false, false, true, false);
                        #endregion
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "txtEditControl_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del conboBox
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbControl_Leave(object sender, EventArgs e)
        {
            ComboBoxEx ctrl = (ComboBoxEx)sender;
            int index = this.NumFila;

            try
            {
                switch (ctrl.Name)
                {
                    case "cmbEstado":
                        if (!string.IsNullOrEmpty(ctrl.Text))
                        {
                            this._rowCurrent.Movimiento.EstadoInv.Value = Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value);
                            this.GetSaldoDisponible(index);
                        }
                        break;
                    case "cmbSerialDisp":
                        if (this.TipoRef != null && this.TipoRef.SerializadoInd.Value.Value && !string.IsNullOrEmpty(ctrl.Text))
                        {
                            DTO_acActivoControl activo;
                            List<DTO_acActivoControl> listActivos;
                           
                            #region Verifica que el serial ingresado exista
                            activo = new DTO_acActivoControl();
                            activo.SerialID.Value = ctrl.Text;
                            activo.BodegaID.Value = this.Bodega.ID.Value;
                            activo.inReferenciaID.Value = this.Referencia.ID.Value;
                            listActivos = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                            if (listActivos.Count > 0)
                            {
                                this.Activo = listActivos[0];

                                this._rowCurrent.Movimiento.ActivoID.Value = this.Activo.ActivoID.Value;
                                this._rowCurrent.Movimiento.SerialID.Value = this.Activo.SerialID.Value;
                                this.GetSaldoDisponible(index);
                            }
                            else
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSerial));
                            #endregion
                            
                        }
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Carga las notas de envio (interaccion con inventarios)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNotaEnv_Click(object sender, EventArgs e)
        {
            try
            {
                bool servicioEmptyInd = false;
                ModalNotasEnvio modNotas = new ModalNotasEnvio((this.data.Header.DocumentoREL == null) ? 0 : this.data.Header.DocumentoREL.Value.Value);
                modNotas.ShowDialog();
                if (modNotas.ReturnVals)
                {
                    #region Limpia la grilla y footer
                    this.data.Footer.Clear();
                    this.deleteOP = true;
                    this.disableValidate = true;
                    this.LoadData(false);
                    this.disableValidate = false;
                    this.deleteOP = false;
                    this.CleanFooter(true);
                    this.UpdateTemp(this.data);
                    #endregion

                    DTO_MvtoInventarios mov = this._bc.AdministrationModel.Transaccion_Get(AppDocuments.NotaEnvio, modNotas.ReturnList[0].NumeroDoc.Value.Value);
                    
                    #region Agrega los registros de los solicitudes
                    int index = 0;
                    mov.Footer.ForEach(movDet =>
                    {
                        DTO_faFacturacionFooter newDet = new DTO_faFacturacionFooter();
                        newDet.Movimiento = movDet.Movimiento;
                        newDet.Movimiento.Index = index;
                        newDet.Movimiento.NroItem.Value = index+1;
                        newDet.Movimiento.ImprimeInd.Value = true;
                        newDet.Movimiento.ServicioID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioxDefecto);
                        newDet.Movimiento.DocSoporteTER.Value = mov.DocCtrl.PrefijoID.Value + " - " + mov.DocCtrl.DocumentoNro.Value;
                        newDet.Movimiento.TerceroID.Value = this.data.DocCtrl.TerceroID.Value;

                        if (string.IsNullOrEmpty(newDet.Movimiento.ServicioID.Value))
                        { 
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Fa_ServicioxDefEmpty));
                            servicioEmptyInd = true;
                            return;
                        }
                        this.data.Footer.Add(newDet);
                        index++;
                    });
                    #endregion

                    if (!servicioEmptyInd)
                    {
                        this.data.Header.DocumentoREL.Value = mov.DocCtrl.NumeroDoc.Value.Value;
                        #region Calcula valores y actualiza la grilla
                        this.LoadData(false);
                        this.UpdateTemp(this.data);

                        this.GetRetenciones(false);
                        this.LoadValuesDet(false);

                        this.EnableFooter(false);
                        this.btnNotaEnv.Enabled = true;
                        this.gvDocument.Columns[this.unboundPrefix + "TerceroID"].OptionsColumn.AllowEdit = true; 
                    }
                    #endregion
                }
                else
                {
                    if (this.data.Header.DocumentoREL != null && this.data.Header.DocumentoREL.Value.Value != 0)
                    {
                        #region Limpia la grilla y footer
                        this.data.Footer.Clear();
                        this.deleteOP = true;
                        this.disableValidate = true;
                        this.LoadData(false);
                        this.disableValidate = false;
                        this.deleteOP = false;
                        this.CleanFooter(true);
                        #endregion
                    }
                    this.data.Header.DocumentoREL.Value = 0;
                    this.UpdateTemp(this.data);
                    if (this.gvDocument.RowCount == 0) this.AddNewRow();
                    //this.EnableFooter(false);
                    //this.masterServicio.EnableControl(true);
                    this.masterServicio.Focus();
                    //this.btnNotaEnv.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "btnNotaEnv_Click"));
            }
        }

        /// <summary>
        /// Carga las notas de envio (interaccion con inventarios)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterDetails_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0)
                {
                    if(this.data.DocCtrl.Estado.Value != 3 && (this.data.Header.DocumentoREL == null || this.data.Header.DocumentoREL.Value.Value == 0))
                    {                    
                        ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
                        GridColumn col = new GridColumn();
                        int index = this.NumFila;
                        switch (master.ColId)
                        {
                            case "BodegaID":
                                #region Codigo Bodega
                                if (this._tipoConcepto != TipoConcepto.VentaInv)
                                    this.txtCantidad.Focus();
                                #endregion
                                break;
                            case "inReferenciaID":
                                #region Codigo Referencia
                                if (this._tipoConcepto != TipoConcepto.VentaInv)
                                    this.txtCantidad.Focus();
                                else if (!this.masterBodega.ValidID || this._costeoTipoInd)
                                    this.masterBodega.Focus();
                                //else
                                //    this._referFocused = true;
                                #endregion
                                break;
                            case "Parametro1ID":
                                #region Codigo Parametro1ID
                                if (this._tipoConcepto != TipoConcepto.VentaInv)
                                    this.txtCantidad.Focus();
                                else
                                {
                                    if (!this.masterBodega.ValidID || this._costeoTipoInd)
                                        this.masterBodega.Focus();
                                    if (!this.masterReferencia.ValidID || this._serialInd)
                                    {
                                        this.masterReferencia.Focus();
                                        if (this._serialInd)
                                        {
                                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NotExistSerial);
                                            MessageBox.Show(String.Format(msg, this.Bodega.ID.Value, this.Referencia.ID.Value.ToString()));
                                            this.masterReferencia.Focus();
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "Parametro2ID":
                                #region Codigo Parametro2ID
                                if (this._tipoConcepto != TipoConcepto.VentaInv)
                                    this.txtCantidad.Focus();
                                else
                                {
                                    if (!this.masterBodega.ValidID || this._costeoTipoInd)
                                        this.masterBodega.Focus();
                                    if (!this.masterReferencia.ValidID || this._serialInd)
                                    {
                                        this.masterReferencia.Focus();
                                        if (this._serialInd)
                                        {
                                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NotExistSerial);
                                            MessageBox.Show(String.Format(msg, this.Bodega.ID.Value, this.Referencia.ID.Value.ToString()));
                                            this.masterReferencia.Focus();
                                        }
                                    }
                                }
                                #endregion
                                break;
                            case "EmpaqueInvID":
                                #region Codigo Empaque
                                if (this._tipoConcepto != TipoConcepto.VentaInv)
                                    this.txtCantidad.Focus();
                                else
                                {
                                    if (!this.masterBodega.ValidID || this._costeoTipoInd)
                                        this.masterBodega.Focus();
                                    if (!this.masterReferencia.ValidID || this._serialInd)
                                    {
                                        this.masterReferencia.Focus();
                                        if (this._serialInd)
                                        {
                                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NotExistSerial);
                                            MessageBox.Show(String.Format(msg, this.Bodega.ID.Value, this.Referencia.ID.Value.ToString()));
                                            this.masterReferencia.Focus();
                                        }
                                    }
                                }
                                #endregion
                                break;
                        }

                        this.gvDocument.RefreshData();                       
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                        if (this.ctrl != null && this.ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                            FormProvider.Master.itemSave.Enabled = false;
                    }
                }
                else 
                {
                    this.gvDocument.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "masterDetails_Leave"));
            }

        }

        /// <summary>
        /// Carga las notas de envio (interaccion con inventarios)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorAnticipoDet_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Calcula los impuestos para el detalle digitado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLiqImpuestos_Click(object sender, EventArgs e)
        {
            this.GetRetenciones(true);
            this.LoadValuesDet(true);
        }

        /// <summary>
        /// Permite ver los saldos de inventario de los items del proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDescargaInventario_Click(object sender, EventArgs e)
        {
            ModalInventarioProyecto mod = new ModalInventarioProyecto(this.factFooter, this.data.DocCtrl.ProyectoID.Value, 0);
            mod.ShowDialog();
            this.factFooter.RemoveAll(x => !string.IsNullOrEmpty(x.Movimiento.BodegaID.Value) && !string.IsNullOrEmpty(x.Movimiento.inReferenciaID.Value));
            this.factFooter.AddRange(mod.DetalleSelected.FindAll(x => x.Movimiento.CantidadUNI.Value > 0));
            this.gcDocument.RefreshDataSource();
            this.gvDocument.FocusedRowHandle = 0;
        }

        /// <summary>
        /// Trae la informacion del proyecto relacionado
        /// </summary>
        /// <param name="sender">Objeto del Evento</param>
        /// <param name="e">Evento</param>
        private void btnTareas_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.data != null)
            {
                ModalProyectoMvto dialog = new ModalProyectoMvto(this.data.DocCtrl.ProyectoID.Value);
                dialog.ShowDialog();
                if (dialog.TareaSelected != null)
                {
                    this.btnDescargaInventario.Enabled = this.facturaTipo.TipoControlInventarios.Value == 1 ? false : true;
                    this.masterTareaProy.Value = dialog.TareaSelected.TareaID.Value;
                    this._rowCurrent.Movimiento.TareaID.Value = dialog.TareaSelected.TareaID.Value;
                    this._rowCurrent.Movimiento.DocSoporte.Value = dialog.TareaSelected.Consecutivo.Value;
                    this._rowCurrent.Movimiento.DescriptivoTarea.Value = dialog.TareaSelected.Descriptivo.Value;
                    this.gvDocument.RefreshData();
                }
                else
                {
                    this.btnDescargaInventario.Enabled = this.factFooter.Any(x => x.Movimiento.DocSoporte.Value.HasValue) ? true : false;
                    this.masterTareaProy.Value = string.Empty;
                    this._rowCurrent.Movimiento.TareaID.Value = string.Empty;
                    this._rowCurrent.Movimiento.DocSoporte.Value = null;
                    this._rowCurrent.Movimiento.DescriptivoTarea.Value = string.Empty;
                    this.gvDocument.RefreshData();
                }

            }
        }

        /// <summary>
        /// Al dar clic en la referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReferencia_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (this.masterBodega.ValidID)
                {
                    ModalReferenciasFilter mod = new ModalReferenciasFilter(this.masterBodega.Value, this.documentID);
                    mod.ShowDialog();
                    this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, this.unboundPrefix + "inReferenciaID", mod.IDSelected);
                    this.masterReferencia.Value = mod.IDSelected;
                    this.masterReferencia.Focus(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "TBPaste: " + ex.Message));
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
                if (this.validHeader)
                {
                    this.cleanDoc = false;
                    if (this.ReplaceDocument())
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        this.cleanDoc = true;

                        this.validHeader = false;
                        this.ValidHeaderTB();
                    }
                }

                if (this.cleanDoc)
                {
                    this.newDoc = true;
                    this.deleteOP = true;

                    this.EnableFooter(false);
                    this.CleanFooter(true);
                    //this.newDoc = false;

                    this._servicio = new DTO_faServicios();
                    this._conceptos = new DTO_faConceptos();
                    this._cuenta = new DTO_coPlanCuenta();
                    this._tercero = new DTO_coTercero();
                    this._activo = new DTO_acActivoControl();
                    this._tipoConcepto = 0;
                    this._conceptoCargo = null;
                    this.isValid = true;
                    this._cuentaRteICA = string.Empty;
                    //this._porcIVA = 0;
                    //this._porcRFT = 0;
                    //this._porcRIVA = 0;
                    //this._porcOtros = 0;

                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gcDocument.Focus();
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                if (this.TempData.DocCtrl != null && this.TempData.DocCtrl.NumeroDoc.Value != null && this.TempData.DocCtrl.NumeroDoc.Value != 0)
                {
                    bool isAprobada = this.TempData.DocCtrl.Estado.Value == 3? true : false;
                    int documentoReport = this.TempData.Header.FacturaTipoID.Value == this._tipoFacturaCtaCobro ? AppReports.faCuentaCobro : AppDocuments.FacturaVenta;
                    string reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(documentoReport, this.TempData.DocCtrl.NumeroDoc.Value.ToString(), isAprobada, ExportFormatType.pdf, 
                                        this.TempData.Header.ValorAnticipo, this.TempData.Header.ValorRteGarantia, this.TempData.Header.PorcRteGarantia.Value,this.IVAUtilidad);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(this.TempData.DocCtrl.NumeroDoc.Value.Value), null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBPrint"));
            }
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilterDef()
        {
            if (this.ValidateRow(this.gvDocument.FocusedRowHandle))
            {
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.gvDocument.RowCount > 0)
                {
                    this.gvDocument.MoveFirst();
                }
                this.CalcularTotal();
            }
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilter()
        {
            try
            {
                if (this.data.Footer.Count() > 0 && this.ValidateRow(this.gvDocument.FocusedRowHandle))
                {
                    MasterQuery mq = new MasterQuery(this, this.documentID, _bc.AdministrationModel.User.ReplicaID.Value.Value, false, typeof(DTO_cpLegalizaFooter), typeof(Filtrable));
                    #region definir Fks
                    #endregion
                    mq.ShowDialog();
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBCopy()
        {
            try
            {
                if (this.ValidGrid())
                {
                    _bc.AdministrationModel.DataCopied = this.data.Footer;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBCopy"));
            }
        }

        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBPaste()
        {
            try
            {
                //Carga la info del comprobante
                List<DTO_ComprobanteFooter> compDet = new List<DTO_ComprobanteFooter>();
                try
                {
                    object o = _bc.AdministrationModel.DataCopied;
                    compDet = (List<DTO_ComprobanteFooter>)o;

                    if (compDet == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                    return;
                }

                //Revisa que cumple las condiciones
                if (!this.ReplaceDocument())
                    return;

                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                //this.data.Footer = compDet;
                this.LoadData(true);
                this.UpdateTemp(this.data);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBPaste"));
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
                DTO_SerializedObject result = new DTO_SerializedObject();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                bool update = false;
                if (this.ctrl.NumeroDoc.Value.Value != 0)
                {
                    numeroDoc = this.ctrl.NumeroDoc.Value.Value;
                    update = true;
                }

                //Asigna el valor final del anticipo
                this.TempData.Header.ValorAnticipo = Convert.ToDecimal(this.txtValorAnticipoDet.EditValue, CultureInfo.InvariantCulture);
                this.TempData.Header.Valor.Value = Convert.ToDecimal(this.txtValorNeto.EditValue, CultureInfo.InvariantCulture);
                this.TempData.DocCtrl.Valor.Value = this.TempData.Header.Valor.Value;
                this.TempData.Header.CuentaRteICA = this._cuentaRteICA;

                result = _bc.AdministrationModel.FacturaVenta_Guardar(this.documentID, this.TempData.DocCtrl, this.TempData.Header, this.TempData.Footer, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, result, true);

                #region Genera Reporte

                if (result.GetType() == typeof(DTO_Alarma))
                {
                    string reportName;
                    string fileURl;
                    DTO_Alarma alarma = (DTO_Alarma)result;
                    string numeroDocu = alarma.NumeroDoc;

                    int documentoReport = this.TempData.Header.FacturaTipoID.Value == this._tipoFacturaCtaCobro ? AppReports.faCuentaCobro : AppDocuments.FacturaVenta;
                    reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(documentoReport, numeroDocu, false, ExportFormatType.pdf,
                                 this.TempData.Header.ValorAnticipo, this.TempData.Header.ValorRteGarantia, this.TempData.Header.PorcRteGarantia.Value,this.IVAUtilidad);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(numeroDocu), null, reportName.ToString());
                    Process.Start(fileURl);
                } 

                #endregion
               
                if (isOK)
                {
                    this.ctrl.NumeroDoc.Value = numeroDoc;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "SaveThread"));
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
                int numeroDoc = this.data.Header.NumeroDoc.Value.Value;
                if (numeroDoc == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                    return;
                }

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

                object obj;
                if (this.ctrl == null || this.ctrl.PeriodoDoc == null || string.IsNullOrEmpty(this.ctrl.ComprobanteID.Value))
                {
                    FormProvider.Master.StopProgressBarThread(this.documentID);

                    result.Result = ResultValue.NOK;
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }
                else
                {
                    obj = _bc.AdministrationModel.ComprobantePre_SendToAprob(this.documentID, this._actFlujo.ID.Value, ModulesPrefix.fa, this.ctrl.PeriodoDoc.Value.Value, this.ctrl.ComprobanteID.Value, this.ctrl.ComprobanteIDNro.Value.Value, false);

                    #region Genera el reporte 

                    if (obj.GetType() == typeof(DTO_Alarma))
                    {
                        string numDoc = ((DTO_Alarma)obj).NumeroDoc;
                        int documentoReport = this.TempData.Header.FacturaTipoID.Value == this._tipoFacturaCtaCobro ? AppReports.faCuentaCobro : AppDocuments.FacturaVenta;
                        reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(documentoReport, numDoc, false, ExportFormatType.pdf,0,0,0,this.IVAUtilidad);
                    }
                    
                    #endregion

                    FormProvider.Master.StopProgressBarThread(this.documentID);
                    bool isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                    if (isOK)
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        this.newDoc = true;
                        this.deleteOP = true;
                        this.data = new DTO_faFacturacion();
                        this.ctrl = new DTO_glDocumentoControl();
                        this.factHeader = new DTO_faFacturaDocu();
                        this.factFooter = new List<DTO_faFacturacionFooter>();
                        this.headerLoaded = false;
                        this.Invoke(this.sendToApproveDelegate);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "SendTiApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion               
  
    }
}
