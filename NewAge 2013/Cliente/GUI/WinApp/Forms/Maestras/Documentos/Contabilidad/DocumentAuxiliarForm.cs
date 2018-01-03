using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using DevExpress.XtraReports.UI;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentAuxiliarForm : DocumentForm
    {
        public DocumentAuxiliarForm()
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
            //FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region Variables privadas

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _docCtrlDTO = null;

        //Variables con los recursos de las Fks
        private string _cuentaRsx = string.Empty;
        private string _terceroRsx = string.Empty;
        private string _prefijoRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;
        private string _lineaPresupRsx = string.Empty;
        private string _conceptoCargoRsx = string.Empty;
        private string _lugarGeoRsx = string.Empty;

        private bool _validCtaProyCC;
        private bool _refreshGrid = true;

        private Dictionary<string, DTO_coConceptoCargo> cacheConcepto = new Dictionary<string, DTO_coConceptoCargo>();
        private Dictionary<string, DTO_coProyecto> cacheProy = new Dictionary<string, DTO_coProyecto>();
        private Dictionary<string, DTO_coCentroCosto> cacheCentroCto = new Dictionary<string, DTO_coCentroCosto>();
        private string operacion = string.Empty;
        #endregion

        #region Variables Protected

        //Variables formulario
        protected Object data = null;
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        protected bool biMoneda = false;
        protected TipoMoneda tipoMoneda;
        protected bool resultOK = true;

        //Indica si el header es valido
        protected bool validHeader;

        //Variables con valores x defecto (glControl)
        protected string defTercero = string.Empty;
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLineaPresupuesto = string.Empty;
        protected string defConceptoCargo = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string nitDIAN = string.Empty;

        //variables para funciones particulares
        protected bool cleanDoc = true;
        protected bool _terceroRadicaDirecto = false;

        //Lista de tarifas
        protected List<decimal> tarifas;
        protected string tipoImpuestoIVA = string.Empty;

        #endregion

        #region Propiedades

        /// <summary>
        /// Comprobante sobre el cual se esta trabajando
        /// </summary>
        private DTO_Comprobante _comprobante = null;
        protected virtual DTO_Comprobante Comprobante
        {
            get { return this._comprobante; }
            set { this._comprobante = value; }
        }

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this.Comprobante == null || this.Comprobante.Footer == null ? -1 :
                    this.Comprobante.Footer.FindIndex(det => det.Index == this.indexFila);
            }
        }

        /// <summary>
        /// Se usa para las cuentas de tipo Componente Tercero
        /// </summary>
        protected DTO_coTercero Tercero
        {
            get;
            set;
        }

        //Cuenta
        private string _conceptoCargoCta = string.Empty;
        private DTO_coPlanCuenta _cuenta = null;
        protected virtual DTO_coPlanCuenta Cuenta
        {
            get
            {
                return this._cuenta;
            }
            set
            {
                this._cuenta = value;
                int index = this.NumFila;
                this._docCtrlDTO = null;

                if (value == null)
                {
                    #region Si la cuenta no existe
                    this._conceptoCargoCta = string.Empty;
                    this.ConceptoSaldo = null;

                    if (this.Comprobante != null && this.Comprobante.Footer.Count > 0)
                    {
                        this.Comprobante.Footer[index].CuentaID.Value = string.Empty;
                        this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                        this.Comprobante.Footer[index].PrefijoCOM.Value = string.Empty;
                        this.Comprobante.Footer[index].TerceroID.Value = string.Empty;
                        this.Comprobante.Footer[index].ActivoCOM.Value = string.Empty;
                        this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                        this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                        this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                        this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                        this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                    }

                    this.masterTercero.Value = string.Empty;
                    this.masterPrefijo.Value = string.Empty;
                    this.masterConceptoCargo.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;
                    this.masterLineaPre.Value = string.Empty;
                    this.masterLugarGeo.Value = string.Empty;
                    this.txtDocumento.Text = string.Empty;
                    this.txtActivo.Text = string.Empty;
                    this.txtDescripcion.Text = string.Empty;
                    this.txtValorML.EditValue = "0";
                    this.txtValorME.EditValue = "0";
                    this.CalcularTotal();
                    this.EnableFooter(false);
                    #endregion
                }
                else
                {
                    this.EnableFooter(true);
                    this._refreshGrid = false;
                    #region Asigna el concepto saldo
                    string conceptoVal = this._cuenta.ConceptoSaldoID.Value;
                    this.ConceptoSaldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, conceptoVal, true);
                    this.Comprobante.Footer[index].ConceptoSaldoID.Value = this.ConceptoSaldo.ID.Value;
                    #endregion
                    #region Asigna la info de la base segun el tipo de impuesto
                    if ((this._cuenta.ImpuestoTipoID == null || string.IsNullOrEmpty(this._cuenta.ImpuestoTipoID.Value)) && this.Comprobante.Footer[index].DatoAdd4.Value != AuxiliarDatoAdd4.Impuesto.ToString())
                    {
                        this.Comprobante.Footer[index].vlrBaseML.Value = 0;
                        this.Comprobante.Footer[index].vlrBaseME.Value = 0;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
                        this.txtBaseML.EditValue = "0";
                        this.txtBaseME.EditValue = "0";
                        this.txtBaseML.Enabled = false;
                        this.txtBaseME.Enabled = false;
                    }
                    else
                    {
                        if (this.biMoneda)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = true;
                            this.txtBaseML.Enabled = true;
                            this.txtBaseME.Enabled =  true;
                        }
                        else if (this.monedaId == this.monedaLocal)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
                            this.txtBaseML.Enabled = true;
                            this.txtBaseME.Enabled = false;
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = true;
                            this.txtBaseML.Enabled = false;
                            this.txtBaseME.Enabled = true;
                        }
                    }
                    #endregion
                    #region Habilita o deshabilita las columnas de los valores
                    if (this.biMoneda)
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = true;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = true;
                        this.txtValorML.Enabled = true;
                        this.txtValorME.Enabled = true;
                    }
                    else if (this.monedaId == this.monedaLocal)
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = true;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
                        this.txtValorML.Enabled = true;
                        this.txtValorME.Enabled = false;
                    }
                    else
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = true;
                        this.txtValorML.Enabled = false;
                        this.txtValorME.Enabled = true;
                    }
                    #endregion
                    #region Habilita las columnas segun el concepto de saldo (o anticipos)
                    if (this.saldoControl == SaldoControl.Cuenta)
                    {
                        #region Tercero
                        //if (!this._cuenta.TerceroInd.Value.Value)
                        //    this.masterTercero.EnableControl(false);
                        #endregion
                        #region Proyecto
                        if (this._cuenta.ProyectoInd.Value.Value)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = true;
                            this.masterProyecto.EnableControl(true);
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                            this.masterProyecto.EnableControl(false);
                        }
                        #endregion
                        #region Centro Costo
                        if (this._cuenta.CentroCostoInd.Value.Value)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = true;
                            this.masterCentroCosto.EnableControl(true);
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                            this.masterCentroCosto.EnableControl(false);
                        }
                        #endregion
                        #region Linea presupuesto
                        if (this._cuenta.LineaPresupuestalInd.Value.Value)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = true;
                            this.masterLineaPre.EnableControl(true);
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                            this.masterLineaPre.EnableControl(false);
                        }
                        #endregion
                        #region Concepto Cargo
                        if (this._cuenta.ConceptoCargoInd.Value.Value)
                        {
                            if (this._cuenta.ConceptoCargoID == null || string.IsNullOrWhiteSpace(this._cuenta.ConceptoCargoID.Value))
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = true;
                                this.masterConceptoCargo.EnableControl(true);
                                this._conceptoCargoCta = string.Empty;
                            }
                            else
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                                this.masterConceptoCargo.EnableControl(false);
                                this._conceptoCargoCta = this._cuenta.ConceptoCargoID.Value;
                            }
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                            this.masterConceptoCargo.EnableControl(false);
                        }
                        #endregion
                        #region Lugar Geografico
                        if (this._cuenta.LugarGeograficoInd.Value.Value)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = true;
                            this.masterLugarGeo.EnableControl(true);
                        }
                        else
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                            this.masterLugarGeo.EnableControl(false);
                        }
                        #endregion
                    }
                    else
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                        this.masterConceptoCargo.EnableControl(false);
                        this.masterLugarGeo.EnableControl(false);

                    }
                    #endregion
                    #region Habilita los controles

                    if (this.saldoControl == SaldoControl.Cuenta)
                    {
                        #region Cuenta
                        this.Tercero = null;
                        this.txtActivo.Enabled = false;
                        this.masterPrefijo.EnableControl(false);
                        this.btnSaldos.Visible = false;

                        if (this._cuenta.ImpuestoTipoID.Value == this.tipoImpuestoIVA && !string.IsNullOrWhiteSpace(this.tipoImpuestoIVA))
                        {
                            this.chkbIVA.Enabled = false;
                            this.cmbTarifa.Enabled = false;

                            this.chkbIVA.Checked = true;
                            this.cmbTarifa.SelectedItem =  this._cuenta.ImpuestoPorc.Value;

                            this.Comprobante.Footer[index].DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                            this.Comprobante.Footer[index].DatoAdd2.Value = this.cmbTarifa.SelectedItem.ToString();
                        }
                        else 
                        {
                            if (this.Comprobante.Footer[index].DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString())
                                this.chkbIVA.Checked = true;
                            else
                                this.chkbIVA.Checked = false;

                            if (this.Comprobante.Footer[index].DatoAdd4.Value != AuxiliarDatoAdd4.Impuesto.ToString())
                            {
                                this.chkbIVA.Enabled = true;
                                this.cmbTarifa.Enabled = true;

                                if (string.IsNullOrWhiteSpace(this.Comprobante.Footer[index].DatoAdd2.Value))
                                    this.cmbTarifa.SelectedIndex = 0;
                                else if (this.Comprobante.Footer[index].DatoAdd2.Value == "0")
                                    this.cmbTarifa.SelectedIndex = 1;
                                else
                                    this.cmbTarifa.SelectedItem = Convert.ToDecimal(this.Comprobante.Footer[index].DatoAdd2.Value, CultureInfo.InvariantCulture);
                            }
                        }
                        #endregion
                    }
                    else if (this.saldoControl == SaldoControl.Componente_Tercero)
                    {
                        #region Componente Tercero
                        this.txtActivo.Enabled = false;
                        this.masterPrefijo.EnableControl(false);
                        this.btnSaldosByCuenta.Enabled = this.masterTercero.ValidID;

                        this.chkbIVA.Enabled = false;
                        this.cmbTarifa.Enabled = false;
                        this.chkbIVA.Checked = false;
                        this.cmbTarifa.SelectedIndex = 0;

                        //this.txtDocumento.Enabled = false;
                        #endregion
                    }
                    else if (this.saldoControl == SaldoControl.Doc_Externo || this.saldoControl == SaldoControl.Componente_Documento)
                    {
                        #region Doc Externo
                        this.Tercero = null;
                        this.txtActivo.Enabled = false;
                        this.masterPrefijo.EnableControl(false);
                        this.btnSaldosByCuenta.Enabled = this.masterTercero.ValidID;

                        this.chkbIVA.Enabled = false;
                        this.cmbTarifa.Enabled = false;
                        this.chkbIVA.Checked = false;
                        this.cmbTarifa.SelectedIndex = 0;
                        #endregion
                    }
                    else if (this.saldoControl == SaldoControl.Doc_Interno)
                    {
                        #region Doc Interno
                        this.Tercero = null;
                        this.txtActivo.Enabled = false;
                        //this.btnSaldos.Visible = true;
                        //this.masterTercero.EnableControl(false);

                        this.chkbIVA.Enabled = false;
                        this.cmbTarifa.Enabled = false;
                        this.chkbIVA.Checked = false;
                        this.cmbTarifa.SelectedIndex = 0;
                        #endregion
                    }
                    else
                    {
                        this.Tercero = null;
                        this.chkbIVA.Enabled = false;
                        this.cmbTarifa.Enabled = false;
                        this.chkbIVA.Checked = false;
                        this.cmbTarifa.SelectedIndex = 0;
                    }

                    #endregion
                    #region Excepciones para Anticipos
                    if (this.Comprobante.Footer[index].DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString())
                    {
                        this.masterCuenta.EnableControl(false);
                        this.masterTercero.EnableControl(false);
                        this.masterPrefijo.EnableControl(false);
                        this.txtDescripcion.Enabled = false;
                        this.txtDocumento.Enabled = false;
                        this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                        this.masterProyecto.EnableControl(false);
                        this.masterCentroCosto.EnableControl(false);
                        this.masterConceptoCargo.EnableControl(false);
                        this.masterLineaPre.EnableControl(false);
                        this.masterLugarGeo.EnableControl(false);

                        this._docCtrlDTO = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(this.Cuenta.ID.Value, this.masterTercero.Value, this.txtDocumento.Text);
                        //this.btnSaldos.Enabled = true;
                    }
                    else
                        this.txtDescripcion.Enabled = true;

                    #endregion
                    #region Excepciones para Impuestos
                    if (this.Comprobante.Footer[index].DatoAdd4.Value == AuxiliarDatoAdd4.Impuesto.ToString())
                    {
                        this.chkbIVA.Enabled = false;
                        this.cmbTarifa.Enabled = false;

                        if (this.Comprobante.Footer[index].DatoAdd2.Value == "0")
                            this.cmbTarifa.SelectedIndex = 1;
                        else
                            this.cmbTarifa.SelectedItem = Convert.ToDecimal(this.Comprobante.Footer[index].DatoAdd2.Value, CultureInfo.InvariantCulture);
                    }
                    #endregion
                    this._refreshGrid = true;
                }
                this.gvDocument.RefreshData();
            }
        }

        //Concepto de saldo
        private DTO_glConceptoSaldo _conceptoSaldo = null;
        protected SaldoControl saldoControl = SaldoControl.Cuenta;
        protected DTO_glConceptoSaldo ConceptoSaldo
        {
            get
            {
                return this._conceptoSaldo;
            }
            set
            {
                this._conceptoSaldo = value;
                if (value != null)
                    this.saldoControl = (SaldoControl)Enum.Parse(typeof(SaldoControl), this._conceptoSaldo.coSaldoControl.Value.Value.ToString());
            }
        }

        #endregion

        #region Funciones Privadas y protected (del auxiliar)

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        private ComprobanteReport GenerateReport(bool show, bool allFields = false)
        {
            try
            {
                switch (this.documentID)
                {
                    case AppDocuments.ComprobanteManual:
                        #region Reporte Comprobante Manual
                        DTO_ReportComprobante2 compReport = new DTO_ReportComprobante2();
                        DTO_Comprobante cmTemp = (DTO_Comprobante)this.data;
                        if (cmTemp != null)
                        {
                            #region Obtener loas datos para el reporte
                            compReport.Header = cmTemp.Header;
                            DTO_glDocumentoControl rdDocCtrl = _bc.AdministrationModel.glDocumentoControl_GetByID(cmTemp.Header.NumeroDoc.Value.Value);
                            bool isPre = true;
                            if (rdDocCtrl == null)
                                compReport.Estado = DictionaryMessages.Error.ToString();
                            else
                            {
                                compReport.Estado = ((EstadoDocControl)rdDocCtrl.Estado.Value).ToString();
                                if ((EstadoDocControl)rdDocCtrl.Estado.Value == EstadoDocControl.Aprobado)
                                    isPre = false;
                            }

                            compReport.footerReport = new List<DTO_ReportComprobanteFooter>();
                            foreach (DTO_ComprobanteFooter f in cmTemp.Footer)
                            {
                                DTO_ReportComprobanteFooter f2 = new DTO_ReportComprobanteFooter(f);
                                DTO_coPlanCuenta cuenta = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = f.CuentaID.Value }, true);
                                f2.Debito = cuenta.Naturaleza.Value.Equals((byte)NaturalezaCuenta.Debito);
                                compReport.footerReport.Add(f2);
                            }

                            List<DTO_ReportComprobante2> list = new List<DTO_ReportComprobante2>();
                            list.Add(compReport);

                            bool MM = this.multiMoneda;
                            #endregion
                            ComprobanteReportBuilder b = new ComprobanteReportBuilder(this.documentID, MM, list, isPre, show, allFields);
                        }
                        return null;
                        #endregion
                    case AppDocuments.DocumentoContable:
                        #region Reporte Documento Contable
                        DTO_Comprobante dcTemp = (DTO_Comprobante)this.data;
                        if (dcTemp != null)
                        {
                            DocumentoContableReportBuilder dcrb = new DocumentoContableReportBuilder(this.documentID, this.multiMoneda, dcTemp, show, true);
                        }
                        return null;
                        #endregion
                    case AppDocuments.CausarFacturas:
                        #region Reporte Autorizacion de Giro
                        DTO_CuentaXPagar cfCXP = (DTO_CuentaXPagar)this.data;
                        if (cfCXP != null)
                        {
                            AutorizacionDeGiroReportBuilder adgrb = new AutorizacionDeGiroReportBuilder((DTO_CuentaXPagar)this.data, this.multiMoneda, show, true);
                        }
                        return null;
                        #endregion
                    case AppDocuments.CruceCuentas:
                        #region Reporte Ajuste saldos
                        DTO_CruceCuentas asTemp = (DTO_CruceCuentas)this.data;
                        if (asTemp != null)
                        {
                            DTO_Comprobante compTemp = asTemp.Comp;
                            DocumentoContableReportBuilder asrb = new DocumentoContableReportBuilder(this.documentID, this.multiMoneda, compTemp, show, true);
                        }
                        return null;
                        #endregion
                    case AppDocuments.ReciboCaja:
                        #region Reporte Recibo de caja
                        DTO_ReciboCaja rcTemp = (DTO_ReciboCaja)this.data;
                        if (rcTemp != null)
                        {
                            #region Carga datos en DTO_ReportReciboCaja
                            DTO_tsReciboCajaDocu recibo = rcTemp.ReciboCajaDoc;
                            DTO_glDocumentoControl ctrl = rcTemp.DocControl;
                            DTO_Comprobante comp = rcTemp.Comp;
                            DTO_coPlanCuenta cuentaInfo;

                            #region Crea el ultimo registro del comprobante
                            decimal ML = comp.Footer.Sum(x => x.vlrMdaLoc.Value.Value);
                            decimal ME = comp.Footer.Sum(x => x.vlrMdaExt.Value.Value);

                            DTO_ComprobanteFooter last = new DTO_ComprobanteFooter();
                            last.Index = comp.Footer.Count;
                            cuentaInfo = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = ctrl.CuentaID.Value }, true);
                            last.CuentaID.Value = ((DTO_coPlanCuenta)cuentaInfo).ID.Value;
                            last.TerceroID.Value = ctrl.TerceroID.Value;
                            last.ProyectoID.Value = ctrl.ProyectoID.Value;
                            last.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                            last.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                            last.ConceptoCargoID.Value = string.IsNullOrEmpty(((DTO_coPlanCuenta)cuentaInfo).ConceptoCargoID.Value) ? this.defConceptoCargo : ((DTO_coPlanCuenta)cuentaInfo).ConceptoCargoID.Value;
                            last.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                            last.PrefijoCOM.Value = this.prefijoID;
                            last.DocumentoCOM.Value = ctrl.DocumentoNro.Value.Value.ToString();
                            last.ActivoCOM.Value = string.Empty;
                            last.ConceptoSaldoID.Value = ((DTO_coPlanCuenta)cuentaInfo).ConceptoSaldoID.Value;
                            last.IdentificadorTR.Value = 0;
                            last.Descriptivo.Value = ctrl.Observacion.Value;
                            last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                            last.vlrBaseML.Value = 0;
                            last.vlrBaseME.Value = 0;
                            last.vlrMdaLoc.Value = ML;
                            last.vlrMdaExt.Value = ME;
                            last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                            last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                            comp.Footer.Add(last);
                            #endregion

                            DTO_ReportReciboCaja reportRecibo = new DTO_ReportReciboCaja();
                            reportRecibo.CajaID = recibo.CajaID.Value;
                            DTO_tsCaja cajaInfo = (DTO_tsCaja)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.tsCaja, new UDT_BasicID() { Value = recibo.CajaID.Value }, true);
                            reportRecibo.CajaDesc = ((DTO_tsCaja)cajaInfo).Descriptivo.Value;
                            reportRecibo.ComprobanteID = ctrl.ComprobanteID.Value;
                            reportRecibo.ComprobanteNro = ctrl.ComprobanteIDNro.Value.Value.ToString();
                            reportRecibo.Fecha = ctrl.Fecha.Value.Value;
                            reportRecibo.DocumentoDesc = ctrl.Observacion.Value;
                            reportRecibo.MonedaID = ctrl.MonedaID.Value;
                            reportRecibo.ReciboNro = ctrl.DocumentoNro.Value.Value.ToString();
                            reportRecibo.TerceroID = ctrl.TerceroID.Value;
                            DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = ctrl.TerceroID.Value }, true);
                            reportRecibo.TerceroDesc = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                            reportRecibo.Valor = recibo.Valor.Value.Value;
                            reportRecibo.Valor_letters = CurrencyFormater.GetCurrencyString("ES1", reportRecibo.MonedaID, reportRecibo.Valor);

                            reportRecibo.ReciboDetail = new List<DTO_ReciboDetail>();

                            foreach (DTO_ComprobanteFooter footer in comp.Footer)
                            {
                                DTO_ReciboDetail reportReciboDet = new DTO_ReciboDetail();
                                reportReciboDet.CuentaID = footer.CuentaID.Value;
                                cuentaInfo = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = footer.CuentaID.Value }, true);
                                reportReciboDet.CuentaDesc = ((DTO_coPlanCuenta)cuentaInfo).Descriptivo.Value;
                                reportReciboDet.TerceroID_cuenta = footer.TerceroID.Value;
                                reportReciboDet.ValorML_cuenta = footer.vlrMdaLoc.Value.Value;

                                reportRecibo.ReciboDetail.Add(reportReciboDet);
                            }
                            #endregion

                            ReciboCajaReport rcReport = new ReciboCajaReport(AppReports.coReciboCaja, new List<DTO_ReportReciboCaja>() { reportRecibo }, ColumnsInfo.ReciboCajaFields, _bc);
                            rcReport.ShowPreview();
                        }
                        return null;
                        #endregion
                    default:
                        return null;
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Recorre lalista de datos del footer y le cambia los signos
        /// </summary>
        protected void CambiarSignoValor()
        {
            try
            {
                for (int i = 0; i < this.Comprobante.Footer.Count; i++)
                {
                    this.Comprobante.Footer[i].vlrBaseML.Value = this.Comprobante.Footer[i].vlrBaseML.Value * -1;
                    this.Comprobante.Footer[i].vlrMdaLoc.Value = this.Comprobante.Footer[i].vlrMdaLoc.Value * -1;
                    this.Comprobante.Footer[i].vlrBaseME.Value = this.Comprobante.Footer[i].vlrBaseME.Value * -1;
                    this.Comprobante.Footer[i].vlrMdaExt.Value = this.Comprobante.Footer[i].vlrMdaExt.Value * -1;

                    this.Comprobante.Footer[i].vlrMdaOtr.Value = this.Comprobante.Footer[i].vlrMdaOtr.Value * -1;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected virtual void CalcularTotal()
        {
            try
            {
                decimal sumBaseLoc = 0;
                decimal sumBaseExt = 0;
                decimal sumLocal = 0;
                decimal sumExtran = 0;
                decimal credlocal = 0;
                decimal credextran = 0;
                for (int i = 0; i < this.gvDocument.DataRowCount; i++)
                {
                    decimal vlrBaseLoc = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, this.unboundPrefix + "vlrBaseML"), CultureInfo.InvariantCulture);
                    decimal vlrBaseExt = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, this.unboundPrefix + "vlrBaseME"), CultureInfo.InvariantCulture);
                    decimal vlrLocal = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, this.unboundPrefix + "vlrMdaLoc"), CultureInfo.InvariantCulture);
                    decimal vlrExtran = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, this.unboundPrefix + "vlrMdaExt"), CultureInfo.InvariantCulture);

                    if (vlrLocal < 0)
                        credlocal += vlrLocal;

                    if (vlrExtran < 0)
                        credextran += vlrExtran;

                    sumBaseLoc += Math.Round(vlrBaseLoc,2);
                    sumBaseExt +=  Math.Round(vlrBaseExt,2);
                    sumLocal += Math.Round(vlrLocal,2);
                    sumExtran += Math.Round(vlrExtran, 2);
                }
                this.txtDebLocal.EditValue = sumLocal - credlocal;
                this.txtDebForeign.EditValue = sumExtran - credextran;
                this.txtCredLocal.EditValue = credlocal;
                this.txtCredForeign.EditValue = credextran;
                this.txtTotalLocal.EditValue = sumLocal;
                this.txtTotalForeign.EditValue = sumExtran;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "CalcularTotal"));
            }
        }

        /// <summary>
        /// Trae y asigna la cuenta por medio de otros valores
        /// </summary>
        private void AsignarCuenta(string fkID, string value)
        {
            DTO_coConceptoCargo concCargo;
            DTO_coProyecto proyecto;
            DTO_coCentroCosto centroCto;
            try
            {

                switch (fkID)
                {
                    case "ConceptoCargoID":
                        #region Consulta el Concepto Cargo
                        if (cacheConcepto.ContainsKey(value))
                            concCargo = cacheConcepto[value];
                        else
                        {
                            concCargo = (DTO_coConceptoCargo)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, false, value, true);
                            cacheConcepto.Add(value, concCargo);
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(concCargo.CuentaID.Value))
                        {
                            this.masterCuenta.Value = concCargo.CuentaID.Value;
                            this.Comprobante.Footer[this.indexFila].CuentaID.Value = concCargo.CuentaID.Value;
                            this.Cuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, concCargo.CuentaID.Value, true);
                            this.operacion = string.Empty;
                        }
                        else
                        {
                            if (this.saldoControl == SaldoControl.Cuenta)
                            {
                                this.masterProyecto.EnableControl(true);
                                this.masterCentroCosto.EnableControl(true);
                                this.masterLineaPre.EnableControl(true);
                            }
                            #region Consulta el Centro Cto
                            if (this.masterCentroCosto.ValidID)
                            {
                                if (cacheCentroCto.ContainsKey(value))
                                    centroCto = cacheCentroCto[value];
                                else
                                {
                                    centroCto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, this.masterCentroCosto.Value, true);
                                    cacheCentroCto.Add(value, centroCto);
                                }
                                this.operacion = centroCto != null ? centroCto.OperacionID.Value : string.Empty;
                            }

                            #endregion
                            #region Consulta el Proyecto
                            if (string.IsNullOrEmpty(this.operacion) && this.masterProyecto.ValidID)
                            {
                                if (cacheProy.ContainsKey(value))
                                    proyecto = cacheProy[value];
                                else
                                {
                                    proyecto = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                                    cacheProy.Add(value, proyecto);
                                }
                                this.operacion = proyecto != null ? proyecto.OperacionID.Value : string.Empty;
                            }
                            #endregion
                        }
                        break;
                    case "ProyectoID":
                        #region Consulta el Proyecto
                        if (cacheProy.ContainsKey(value))
                            proyecto = cacheProy[value];
                        else
                        {
                            proyecto = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, value, true);
                            cacheProy.Add(value, proyecto);
                        }
                        this.operacion = proyecto != null ? proyecto.OperacionID.Value : string.Empty;
                        #endregion
                        break;
                    case "CentroCostoID":
                        #region Consulta el CentroCosto
                        if (cacheCentroCto.ContainsKey(value))
                            centroCto = cacheCentroCto[value];
                        else
                        {
                            centroCto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, false, this.masterCentroCosto.Value, true);
                            cacheCentroCto.Add(value, centroCto);
                        }
                        this.operacion = centroCto != null ? centroCto.OperacionID.Value : string.Empty;
                        #endregion
                        break;
                }

                #region Consulta la Cuenta por coCargoCosto
                if (!string.IsNullOrEmpty(operacion) && this.masterConceptoCargo.ValidID && this.masterLineaPre.ValidID)
                {
                    #region Filtro
                    DTO_glConsulta query = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ConceptoCargoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = this.masterConceptoCargo.Value,
                        OperadorSentencia = OperadorSentencia.And
                    });
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LineaPresupuestoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = this.masterLineaPre.Value,
                        OperadorSentencia = OperadorSentencia.And
                    });
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "OperacionID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = this.operacion
                    });
                    query.Filtros = filtros;
                    #endregion
                    long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coCargoCosto, query, null);
                    List<DTO_coCargoCosto> complexCargo = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coCargoCosto, count, 1, query, null).Cast<DTO_coCargoCosto>().ToList();

                    //if (complexCargo.Count < 1)
                    //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCostoNoExiste));
                    if (complexCargo.Count > 1)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCostoNoUnica));
                    else if (complexCargo.Count == 1)
                    {
                        this.masterCuenta.Value = complexCargo[0].CuentaID.Value;
                        this.Comprobante.Footer[this.indexFila].CuentaID.Value = this.masterCuenta.Value;
                        this.Cuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, complexCargo[0].CuentaID.Value, true);
                    }
                }
                #endregion
                #region Asigna valores a columnas
                if (this.Cuenta != null && this.saldoControl == SaldoControl.Cuenta)
                {
                    this.Comprobante.Footer[this.indexFila].IdentificadorTR.Value = 0;

                    if (string.IsNullOrEmpty(operacion))
                    {
                        #region Proyecto
                        if (!this._cuenta.ProyectoInd.Value.Value)
                        {
                            this.masterProyecto.Value = string.IsNullOrEmpty(this.masterProyecto.Value) ? this.defProyecto : this.masterProyecto.Value;
                            this.Comprobante.Footer[this.indexFila].ProyectoID.Value = this.defProyecto;
                        }
                        #endregion
                        #region Centro Costo
                        if (!this._cuenta.CentroCostoInd.Value.Value)
                        {
                            this.masterCentroCosto.Value = string.IsNullOrEmpty(this.masterCentroCosto.Value) ? this.defCentroCosto : this.masterCentroCosto.Value;
                            this.Comprobante.Footer[this.indexFila].CentroCostoID.Value = this.defCentroCosto;
                        }
                        #endregion
                        #region Linea presupuesto
                        if (!this._cuenta.LineaPresupuestalInd.Value.Value)
                        {
                            this.masterLineaPre.Value = string.IsNullOrEmpty(this.masterLineaPre.Value) ? this.defLineaPresupuesto : this.masterLineaPre.Value;
                            this.Comprobante.Footer[this.indexFila].LineaPresupuestoID.Value = this.defLineaPresupuesto;
                        }
                        #endregion
                    }
                    #region Tercero
                    if (!this._cuenta.TerceroInd.Value.Value)
                    {
                        this.masterTercero.Value = string.IsNullOrEmpty(this.masterTercero.Value) ? this.defTercero : this.masterTercero.Value;
                        this.Comprobante.Footer[this.indexFila].TerceroID.Value = this.defTercero;
                    }
                    #endregion
                    #region Concepto Cargo
                    if (!this._cuenta.ConceptoCargoInd.Value.Value)
                    {
                        this.masterConceptoCargo.Value = string.IsNullOrEmpty(this.masterConceptoCargo.Value) ? this.defConceptoCargo : this.masterConceptoCargo.Value;
                        this.Comprobante.Footer[this.indexFila].ConceptoCargoID.Value = this.defConceptoCargo;
                    }
                    else
                    {
                        if (this._conceptoCargoCta != string.Empty)
                        {
                            this.masterConceptoCargo.Value = this._conceptoCargoCta;
                            this.Comprobante.Footer[this.indexFila].ConceptoCargoID.Value = this._conceptoCargoCta;
                        }
                    }
                    #endregion
                    #region Lugar Geografico
                    if (!this._cuenta.LugarGeograficoInd.Value.Value)
                    {
                        this.masterLugarGeo.Value = string.IsNullOrEmpty(this.masterLugarGeo.Value) ? this.defLugarGeo : this.masterLugarGeo.Value;
                        this.Comprobante.Footer[this.indexFila].LugarGeograficoID.Value = this.defLugarGeo;
                    }
                    #endregion
                    #region Prefijo
                    this.masterPrefijo.Value = string.IsNullOrEmpty(this.masterPrefijo.Value) ? this.defPrefijo : this.masterPrefijo.Value;
                    this.Comprobante.Footer[this.indexFila].PrefijoCOM.Value = this.defPrefijo;
                    #endregion
                }

                #endregion

                this.ValidateRow(this.gvDocument.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "AssignCuenta"));
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
            //if (firstTime)
            //    disableValidate = true;

            this.gcDocument.DataSource = this.Comprobante.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.Comprobante.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();

            this.dataLoaded = true;
            this.disableValidate = false;

            this.CalcularTotal();
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Cuenta
            if (colName == this._cuentaRsx)
                return AppMasters.coPlanCuenta;
            //Tercero
            if (colName == this._terceroRsx)
                return AppMasters.coTercero;
            //Prefijo
            if (colName == this._prefijoRsx)
                return AppMasters.glPrefijo;
            //Proyecto
            if (colName == this._proyectoRsx)
                return AppMasters.coProyecto;
            //Cwentro Costo
            if (colName == this._centroCostoRsx)
                return AppMasters.coCentroCosto;
            //Linea presupuestal
            if (colName == this._lineaPresupRsx)
                return AppMasters.plLineaPresupuesto;
            //Concepto Cargo
            if (colName == this._conceptoCargoRsx)
                return AppMasters.coConceptoCargo;
            //Lugar Geografico
            if (colName == this._lugarGeoRsx)
                return AppMasters.glLugarGeografico;

            return 0;
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
                this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(cFila, col));

                this.LoadEditGridData(false, cFila);
                this.isValid = true;

                if (oper)
                    this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "RowIndexChanged"));
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

            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga los valores por defecto
            this.defTercero = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.defPrefijo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.defConceptoCargo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            this.nitDIAN = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_NitDIAN);
            //Carga los recursos de las Fks
            this._cuentaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            this._terceroRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
            this._prefijoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoCOM");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._centroCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            this._lineaPresupRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            this._conceptoCargoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoCargoID");
            this._lugarGeoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeograficoID");

            this.tipoImpuestoIVA = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);

            //Carga la info de las tarifas
            this.tarifas = _bc.AdministrationModel.coPlanCuenta_TarifasImpuestos();
            
            this.cmbTarifa.Items.Add(string.Empty);
            this.cmbTarifa.Items.Add("0"); 
            foreach (decimal t in this.tarifas)
                this.cmbTarifa.Items.Add(t);

            this.cmbTarifa.SelectedIndex = 0;
            this.cmbTarifa.SelectedValueChanged += new System.EventHandler(this.cmbTarifas_SelectedValueChanged);

            FormProvider.Master.itemPaste.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;

            //Controles del detalle
            _bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            _bc.InitMasterUC(this.masterConceptoCargo, AppMasters.coConceptoCargo, true, true, true, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLineaPre, AppMasters.plLineaPresupuesto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);

            this.masterCuenta.txtCode.DoubleClick +=new EventHandler(txtCode_DoubleClick);
            this.tlSeparatorPanel.RowStyles[0].Height = 43;
            this.tlSeparatorPanel.RowStyles[2].Height = 200;
            this.format = _bc.GetImportExportFormat(typeof(DTO_ComprobanteFooter), this.documentID);          
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.AddGridCols();
            this.lastColName = this.unboundPrefix + "Descriptivo";
            this.EnableFooter(false);
            this.chkbIVA.Text = AuxiliarDatoAdd1.IVA.ToString();

            string v = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ValidCta_Operacion);
            this._validCtaProyCC = v == "1" ? true : false;

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
               this.groupCtrlCurrency2.Visible = false;
               this.txtBaseME.Visible = false;
               this.txtValorME.Visible = false;
               this.lblBaseME.Visible = false;
               this.lblValorME.Visible = false;
            }

            #region Carga temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var aux = _bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (aux != null)
                        {
                            try
                            {
                                this.LoadTempData(aux);
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
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "AfterInitialize: " + ex.Message));
                }
            }
            #endregion
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected override void CleanFormat()
        {
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            string f = string.Empty;
            foreach (string col in cols)
            {
                if (col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaOtr") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd1") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd2") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd3") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd4") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd5") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd6") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd7") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd8") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd9") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd10") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambio"))
                {
                    if (col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrBaseME") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaExt"))
                    {
                        if (this.multiMoneda)
                            f += col + this.formatSeparator;
                    }
                    else
                        f += col + this.formatSeparator;
                }
            }

            this.format = f;
        }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected override void ValidHeaderTB()
        {
            if (this.validHeader)
            {
                FormProvider.Master.itemFilterDef.Enabled = true;
                FormProvider.Master.itemFilter.Enabled = true;

                //Habilita el boton de salvar
                if (SecurityManager.HasAccess(this.documentID, FormsActions.Add) || SecurityManager.HasAccess(this.documentID, FormsActions.Edit))
                    FormProvider.Master.itemSave.Enabled = true;
                else
                    FormProvider.Master.itemSave.Enabled = false;

                FormProvider.Master.itemRevert.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Revert);
                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                FormProvider.Master.itemCopy.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Copy);
                FormProvider.Master.itemPaste.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Paste);
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
            else
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemRevert.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemFilterDef.Enabled = false;
                FormProvider.Master.itemFilter.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemExport.Enabled = false;
                FormProvider.Master.itemCopy.Enabled = false;
                FormProvider.Master.itemPaste.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
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

                #region Pks
                //Cuenta
                GridColumn cuenta = new GridColumn();
                cuenta.FieldName = this.unboundPrefix + "CuentaID";
                cuenta.Caption = this._cuentaRsx;
                cuenta.UnboundType = UnboundColumnType.String;
                cuenta.VisibleIndex = 1;
                cuenta.Width = 70;
                cuenta.Visible = true;
                cuenta.Fixed = FixedStyle.Left;
                cuenta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cuenta);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this.unboundPrefix + "TerceroID";
                tercero.Caption = this._terceroRsx;
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 2;
                tercero.Width = 70;
                tercero.Visible = true;
                tercero.Fixed = FixedStyle.Left;
                tercero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(tercero);

                //Prefijo
                GridColumn pref = new GridColumn();
                pref.FieldName = this.unboundPrefix + "PrefijoCOM";
                pref.Caption = this._prefijoRsx;
                pref.UnboundType = UnboundColumnType.String;
                pref.VisibleIndex = 3;
                pref.Width = 70;
                pref.Visible = true;
                pref.Fixed = FixedStyle.Left;
                pref.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(pref);

                //Documento
                GridColumn doc = new GridColumn();
                doc.FieldName = this.unboundPrefix + "DocumentoCOM";
                doc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                doc.UnboundType = UnboundColumnType.String;
                doc.VisibleIndex = 4;
                doc.Width = 70;
                doc.Visible = true;
                doc.Fixed = FixedStyle.Left;
                doc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(doc);

                #endregion
                #region Columnas extras

                //Activo
                GridColumn act = new GridColumn();
                act.FieldName = this.unboundPrefix + "ActivoCOM";
                act.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
                act.UnboundType = UnboundColumnType.String;
                act.VisibleIndex = 5;
                act.Width = 100;
                act.Visible = true;
                act.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(act);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = this._proyectoRsx;
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 6;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = this._centroCostoRsx;
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 7;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ctoCosto);

                //Linea Presupuestal
                GridColumn linPresup = new GridColumn();
                linPresup.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                linPresup.Caption = this._lineaPresupRsx;
                linPresup.UnboundType = UnboundColumnType.String;
                linPresup.VisibleIndex = 8;
                linPresup.Width = 100;
                linPresup.Visible = true;
                linPresup.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(linPresup);

                //Concepto cargo
                GridColumn concCargo = new GridColumn();
                concCargo.FieldName = this.unboundPrefix + "ConceptoCargoID";
                concCargo.Caption = this._conceptoCargoRsx;
                concCargo.UnboundType = UnboundColumnType.String;
                concCargo.VisibleIndex = 9;
                concCargo.Width = 100;
                concCargo.Visible = true;
                concCargo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(concCargo);

                //Lugar Geografico
                GridColumn lg = new GridColumn();
                lg.FieldName = this.unboundPrefix + "LugarGeograficoID";
                lg.Caption = this._lugarGeoRsx;
                lg.UnboundType = UnboundColumnType.String;
                lg.VisibleIndex = 10;
                lg.Width = 100;
                lg.Visible = true;
                lg.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(lg);

                //Valor Base ML
                GridColumn vlrBaseML = new GridColumn();
                vlrBaseML.FieldName = this.unboundPrefix + "vlrBaseML";
                vlrBaseML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
                vlrBaseML.UnboundType = UnboundColumnType.Decimal;
                vlrBaseML.VisibleIndex = 11;
                vlrBaseML.Width = 120;
                vlrBaseML.Visible = true;
                vlrBaseML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrBaseML);

                //Valor Moneda local
                GridColumn vlrMdaLoc = new GridColumn();
                vlrMdaLoc.FieldName = this.unboundPrefix + "vlrMdaLoc";
                vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                vlrMdaLoc.VisibleIndex = 12;
                vlrMdaLoc.Width = 150;
                vlrMdaLoc.Visible = true;
                vlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrMdaLoc);

                //Tasa de Cambio
                GridColumn tasaCambio = new GridColumn();
                tasaCambio.FieldName = this.unboundPrefix + "TasaCambio";
                tasaCambio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblExchangeRate");
                tasaCambio.UnboundType = UnboundColumnType.Decimal;
                tasaCambio.VisibleIndex = 13;
                tasaCambio.Width = 100;
                tasaCambio.Visible = this.multiMoneda;
                tasaCambio.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(tasaCambio);

                //Valor Base ME
                GridColumn vlrBaseME = new GridColumn();
                vlrBaseME.FieldName = this.unboundPrefix + "vlrBaseME";
                vlrBaseME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                vlrBaseME.UnboundType = UnboundColumnType.Decimal;
                vlrBaseME.VisibleIndex = 14;
                vlrBaseME.Width = 120;
                vlrBaseME.Visible = this.multiMoneda;// true;
                vlrBaseME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrBaseME);

                //Valor Moneda Ext
                GridColumn vlrMdaExt = new GridColumn();
                vlrMdaExt.FieldName = this.unboundPrefix + "vlrMdaExt";
                vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                vlrMdaExt.VisibleIndex = 15;
                vlrMdaExt.Width = 150;
                vlrMdaExt.Visible = this.multiMoneda;// true;
                vlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrMdaExt);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 16;
                desc.Width = 100;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);


                //Concepto Saldo
                GridColumn cs = new GridColumn();
                cs.FieldName = this.unboundPrefix + "ConceptoSaldoID";
                cs.UnboundType = UnboundColumnType.String;
                cs.Visible = false;
                this.gvDocument.Columns.Add(cs);

                //Dato adicional 3
                GridColumn datoAdd3 = new GridColumn();
                datoAdd3.FieldName = this.unboundPrefix + "DatoAdd3";
                datoAdd3.UnboundType = UnboundColumnType.String;
                datoAdd3.Visible = false;
                this.gvDocument.Columns.Add(datoAdd3);

                //Dato adicional 4
                GridColumn datoAdd4 = new GridColumn();
                datoAdd4.FieldName = this.unboundPrefix + "DatoAdd4";
                datoAdd4.UnboundType = UnboundColumnType.String;
                datoAdd4.Visible = false;
                this.gvDocument.Columns.Add(datoAdd4);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "AddGridCols"));
            }
        }

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
                    string val_Cuenta = (isNew || gvDocument.Columns[this.unboundPrefix + "CuentaID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "CuentaID"]).ToString();
                    string val_Tercero = (isNew || gvDocument.Columns[this.unboundPrefix + "TerceroID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "TerceroID"]).ToString();
                    string val_Prefijo = (isNew || gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"]).ToString();
                    string val_Documento = (isNew || gvDocument.Columns[this.unboundPrefix + "DocumentoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "DocumentoCOM"]).ToString();
                    string val_Activo = (isNew || gvDocument.Columns[this.unboundPrefix + "ActivoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ActivoCOM"]).ToString();
                    string val_Descr = (isNew || gvDocument.Columns[this.unboundPrefix + "Descriptivo"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "Descriptivo"]).ToString();
                    string val_ConceptoCargo = (isNew || gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"]).ToString();
                    string val_Proyecto = (isNew || gvDocument.Columns[this.unboundPrefix + "ProyectoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ProyectoID"]).ToString();
                    string val_CentroCosto = (isNew || gvDocument.Columns[this.unboundPrefix + "CentroCostoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "CentroCostoID"]).ToString();
                    string val_LineaPres = (isNew || gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"]).ToString();
                    string val_LugarGeo = (isNew || gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"]).ToString();
                    string val_BaseML = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrBaseML"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrBaseML"]).ToString();
                    string val_BaseME = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrBaseME"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrBaseME"]).ToString();
                    string val_ValorML = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"]).ToString();
                    string val_ValorME = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"]).ToString();

                    this.masterCuenta.Value = val_Cuenta;
                    this.masterTercero.Value = val_Tercero;
                    this.masterPrefijo.Value = val_Prefijo;
                    this.txtDocumento.Text = val_Documento;
                    this.txtActivo.Text = val_Activo;
                    this.txtDescripcion.Text = val_Descr;
                    this.masterConceptoCargo.Value = val_ConceptoCargo;
                    this.masterProyecto.Value = val_Proyecto;
                    this.masterCentroCosto.Value = val_CentroCosto;
                    this.masterLineaPre.Value = val_LineaPres;
                    this.masterLugarGeo.Value = val_LugarGeo;
                    this.txtBaseML.EditValue = val_BaseML;
                    this.txtBaseME.EditValue = val_BaseME;
                    this.txtValorML.EditValue = val_ValorML;
                    this.txtValorME.EditValue = val_ValorME;

                    if (this.newDoc)
                    {
                        this.EnableFooter(false);
                        this.newDoc = false;
                    }
                    else
                    {
                        this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, val_Cuenta, true);
                        if (this.saldoControl == SaldoControl.Componente_Tercero)
                            this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, val_Tercero, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "LoadEditGridData"));
                }
            }
        }

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
                footerDet.TerceroID.Value = string.Empty;
                footerDet.ProyectoID.Value = string.Empty;
                footerDet.CentroCostoID.Value = string.Empty;
                footerDet.LineaPresupuestoID.Value = string.Empty;
                footerDet.ConceptoCargoID.Value = string.Empty;
                footerDet.PrefijoCOM.Value = string.Empty;
                footerDet.DocumentoCOM.Value = string.Empty;
                footerDet.ActivoCOM.Value = string.Empty;
                footerDet.LugarGeograficoID.Value = string.Empty;
                footerDet.ConceptoSaldoID.Value = string.Empty;
                footerDet.Descriptivo.Value = string.Empty;
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
            this.masterConceptoCargo.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCosto.EnableControl(false);
            this.masterLineaPre.EnableControl(false);
            this.masterLugarGeo.EnableControl(false);
            //Valores
            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "TasaCambio"].OptionsColumn.AllowEdit = false;
            this.txtBaseML.Enabled = false;
            this.txtBaseME.Enabled = false;
            this.txtValorML.Enabled = false;
            this.txtValorME.Enabled = false;
            #endregion

            this.Comprobante.Footer.Add(footerDet);
            this.gvDocument.RefreshData();
            this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

            this.isValid = false;
            this.EnableFooter(false);
            this.masterCuenta.EnableControl(true);          
            if (this.masterCuenta.ValidID)
                this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this.masterCuenta.Value, true);
            this.masterConceptoCargo.EnableControl(true);
            this.masterCuenta.Focus();
        }

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
                if (fila >= 0)
                {
                    string msg;
                    string colVal;
                    GridColumn col = new GridColumn();

                    #region Validacion de nulls y Fks
                    string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    #region Cuenta
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CuentaID", false, true, true, AppMasters.coPlanCuenta);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Tercero

                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "TerceroID", false, true, false, AppMasters.coTercero);
                    if (!validField)
                        validRow = false;
                    
                    #endregion
                    #region Documento
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "DocumentoCOM", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Prefijo
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "PrefijoCOM", false, true, false, AppMasters.glPrefijo);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Proyecto
                    //if (this.Cuenta.ProyectoInd.Value.Value)
                    //{
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProyectoID", false, true, true, AppMasters.coProyecto);
                    if (!validField)
                        validRow = false;
                    //}
                    #endregion
                    #region Centro Costo
                    //if (this.Cuenta.CentroCostoInd.Value.Value)
                    //{
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                    if (!validField)
                        validRow = false;
                    //}
                    #endregion
                    #region Linea Presupuesto
                    //if (this.Cuenta.LineaPresupuestalInd.Value.Value)
                    //{
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "LineaPresupuestoID", false, true, false, AppMasters.plLineaPresupuesto);
                    if (!validField)
                        validRow = false;
                    //}
                    #endregion
                    #region Concepto Cargo

                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ConceptoCargoID", false, true, true, AppMasters.coConceptoCargo);
                    if (!validField)
                        validRow = false;
                    else if (this._terceroRadicaDirecto)
                    {
                        col = this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"];
                        string conceptoCargoID = this.gvDocument.GetRowCellValue(fila, col).ToString();
                        DTO_coConceptoCargo concep = (DTO_coConceptoCargo)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, false, conceptoCargoID, true, null);
                        if (concep != null && concep.TipoConcepto.Value != (byte)TipoConcepto.Servicio)
                        {
                            msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_TerceroRadicaDirecto);
                            this.gvDocument.SetColumnError(col, msg);
                            validRow = false;
                        }
                    }

                    #endregion
                    #region Lugar Geografico
                    //if (this.Cuenta.LugarGeograficoInd.Value.Value)
                    //{
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "LugarGeograficoID", false, true, true, AppMasters.glLugarGeografico);
                    if (!validField)
                        validRow = false;
                    //}
                    #endregion
                    #region Activo (Plaqueta)
                    col = this.gvDocument.Columns[this.unboundPrefix + "ActivoCOM"];
                    colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();

                    if (string.IsNullOrEmpty(colVal) && this.saldoControl == SaldoControl.Componente_Activo)
                    {
                        msg = string.Format(rsxEmpty, col.Caption);
                        this.gvDocument.SetColumnError(col, msg);
                        validRow = false;
                    }
                    #endregion
                    #region Descriptivo
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Descriptivo", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #endregion
                    #region Validacion Cuenta periodo abierto
                    if (validRow)
                    {
                        ModulesPrefix cMod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), this.ConceptoSaldo.ModuloID.Value.ToLower());
                        if (this.saldoControl != SaldoControl.Cuenta)
                        {
                            if (cMod != ModulesPrefix.apl && cMod != ModulesPrefix.gl && cMod != ModulesPrefix.co)
                            {
                                string currentP = this.dtPeriod.DateTime.ToString(FormatString.ControlDate);
                                string perAbierto = _bc.GetControlValueByCompany(cMod, AppControl.co_Periodo);

                                if (perAbierto == string.Empty || currentP != perAbierto)
                                {
                                    msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CtaPeriodClosed);
                                    col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                    colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();
                                    this.gvDocument.SetColumnError(col, msg);

                                    validRow = false;
                                }
                            }
                        }
                    }

                    #endregion
                    #region Validacion Control de saldos (saldo control = cuenta)
                    if (validRow && this.saldoControl == SaldoControl.Cuenta && this._validCtaProyCC && this.Cuenta.ConceptoCargoInd.Value.Value)
                    {
                        col = this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"];
                        string concCargoID = this.gvDocument.GetRowCellValue(fila, col).ToString();

                        if (this.Cuenta.ProyectoInd.Value.Value)
                        {
                            string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                            col = this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"];
                            string proyID = this.gvDocument.GetRowCellValue(fila, col).ToString();
                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, proyID, string.Empty, linPresID).Trim();
                            if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != this.Cuenta.ID.Value)
                            {
                                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                                col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();
                                this.gvDocument.SetColumnError(col, msg);

                                validRow = false;
                            }
                        }
                        else if (this.Cuenta.CentroCostoInd.Value.Value)
                        {
                            col = this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"];
                            string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                            string ctoCostoID = this.gvDocument.GetRowCellValue(fila, col).ToString();
                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, string.Empty, ctoCostoID, linPresID).Trim();
                            if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != this.Cuenta.ID.Value)
                            {
                                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                                col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();
                                this.gvDocument.SetColumnError(col, msg);

                                validRow = false;
                            }
                        }
                    }
                    #endregion
                    #region Validaciones de valores

                    if (this.Cuenta != null)
                    {
                        decimal impuesto = this.Cuenta.ImpuestoPorc != null && this.Cuenta.ImpuestoPorc.Value.HasValue ? this.Cuenta.ImpuestoPorc.Value.Value : 0;

                        decimal baseValML = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"]), CultureInfo.InvariantCulture);
                        decimal baseValME = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"]), CultureInfo.InvariantCulture);

                        decimal impRealML = Math.Round(baseValML * impuesto / 100);
                        decimal impRealME = Math.Round(baseValME * impuesto / 100);

                        decimal valML = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"]), CultureInfo.InvariantCulture);
                        decimal valME = Convert.ToDecimal(this.gvDocument.GetRowCellValue(fila, this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"]), CultureInfo.InvariantCulture);

                        if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                        {
                            valML *= -1;
                            valME *= -1;
                        }

                        decimal difML = Math.Abs(impRealML) - Math.Abs(valML);
                        decimal difMaxML = Math.Abs(baseValML * (Decimal)0.01 / 100);
                        decimal difME = Math.Abs(impRealME) - Math.Abs(valME);
                        decimal difMaxME = Math.Abs(baseValME * (Decimal)0.01 / 100);

                        bool acceptCeroML = false;
                        bool acceptCeroME = false;
                        bool invalidImpML = false;
                        bool invalidImpME = false;
                        if (this.Comprobante.Footer[fila].TerceroID.Value != this.nitDIAN)
                        {
                            if (!this.biMoneda)
                            {
                                if (this.monedaId == monedaLocal)
                                {
                                    acceptCeroME = true;
                                    if (impuesto != 0 && Math.Abs(difML) > Math.Abs(difMaxML))
                                        invalidImpML = true;
                                }
                                else
                                {
                                    acceptCeroML = true;
                                    if (impuesto != 0 && Math.Abs(difME) > Math.Abs(difMaxME))
                                        invalidImpME = true;
                                }
                            }
                            else
                            {
                                if (impuesto != 0 && Math.Abs(difML) > Math.Abs(difMaxML))
                                    invalidImpML = true;
                                if (impuesto != 0 && Math.Abs(difME) > Math.Abs(difMaxME))
                                    invalidImpME = true;
                            } 
                        }

                        #region vlrMdaLoc
                        validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "vlrMdaLoc", false, acceptCeroML, false, invalidImpML);
                        if (!validField)
                            validRow = false;
                        #endregion
                        #region vlrMdaExt
                        if (this.multiMoneda)
                        {
                            validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "vlrMdaExt", false, acceptCeroME, false, invalidImpME);
                            if (!validField)
                                validRow = false;
                        }
                        #endregion
                    }

                    #endregion
                    #region Validacion de impuestos

                    if (this.Comprobante.Footer[this.NumFila].DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString() &&
                        string.IsNullOrWhiteSpace(this.Comprobante.Footer[this.NumFila].DatoAdd2.Value))
                    {
                        msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaIVATarifa);
                        col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                        this.gvDocument.SetColumnError(col, msg);

                        validRow = false;
                    }

                    #endregion
                    #region Validacion de los anticipos

                    if (this.Comprobante.Footer[this.NumFila].DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString())
                    {
                        decimal minVal = 0;
                        decimal val = 0;
                        try
                        {
                            minVal = Convert.ToDecimal(this.Comprobante.Footer[this.NumFila].DatoAdd3.Value, CultureInfo.InvariantCulture);
                            minVal *= -1;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.AnticipoNoMaxValue));
                            validRow = false;
                        }

                        string rsxMax = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.AnticipoMaxValue);
                        string rsxMin = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.AnticipoMinValue);

                        val = this.tipoMoneda == TipoMoneda.Local ?
                            Convert.ToDecimal(this.Comprobante.Footer[this.NumFila].vlrMdaLoc.Value.Value, CultureInfo.InvariantCulture) :
                            Convert.ToDecimal(this.Comprobante.Footer[this.NumFila].vlrMdaExt.Value.Value, CultureInfo.InvariantCulture);

                        if (val < minVal)
                        {
                            msg = string.Format(rsxMax, minVal.ToString());
                            col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                            this.gvDocument.SetColumnError(col, msg);
                            validRow = false;
                        }
                        else if (val > 0)
                        {
                            col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                            this.gvDocument.SetColumnError(col, rsxMin);
                            validRow = false;
                        }
                    }

                    #endregion
                    #region Validacion de cuenta del modulo de Cartera 
                    if (this.ConceptoSaldo != null && this.ConceptoSaldo.ModuloID.Value.ToLower() == ModulesPrefix.cc.ToString().ToLower())
                    {    
                        msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_CuentaInvalidModule);
                        col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                        this.gvDocument.SetColumnError(col, msg);
                        validRow = false;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "ValidateRow"));
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

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected virtual decimal LoadTasaCambio(int monOr)
        {
            try
            {
                decimal valor = 0;
                string tasaMon = this.monedaId;
                if (monOr == (int)TipoMoneda.Local)
                    tasaMon = this.monedaExtranjera;

                valor = _bc.AdministrationModel.TasaDeCambio_Get(tasaMon, this.dtFecha.DateTime);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "LoadTasaCambio"));
                return 0;
            }
        }

        /// <summary>
        /// Calcula el valor local o extranjero segun la moneda origen y la tasa de cambio
        /// </summary>
        /// <returns>Retorna el valor calculado y el resultado de la operacion</returns>
        protected virtual bool CalcularValorExtra()
        {
            int index = this.NumFila;
            if (!this.biMoneda && this.monedaId == this.monedaLocal)
            {
                decimal valAuxExt = this.Comprobante.Footer[index].TasaCambio.Value.Value == 0 ?
                    0 : this.Comprobante.Footer[index].vlrMdaLoc.Value.Value / this.Comprobante.Footer[index].TasaCambio.Value.Value;

                this.Comprobante.Footer[index].vlrMdaExt.Value = Math.Round(valAuxExt, 2);
                this.txtValorME.EditValue = this.Comprobante.Footer[index].vlrMdaExt.Value;
            }
            else if (!this.biMoneda && this.monedaId == this.monedaExtranjera)
            {
                decimal valAuxLoc = this.Comprobante.Footer[index].TasaCambio.Value.Value == 0 ?
                    0 : this.Comprobante.Footer[index].vlrMdaExt.Value.Value * this.Comprobante.Footer[index].TasaCambio.Value.Value;

                this.Comprobante.Footer[index].vlrMdaLoc.Value = Math.Round(valAuxLoc, 2);
            }

            this.Comprobante.Footer[index].vlrMdaOtr.Value = this.monedaId == this.monedaLocal ?
                this.Comprobante.Footer[index].vlrMdaLoc.Value : this.Comprobante.Footer[index].vlrMdaExt.Value;

            bool acceptCeroML = false;
            bool acceptCeroME = false;
            bool invalidImpML = false;
            bool invalidImpME = false;

            decimal impuesto = this.Cuenta.ImpuestoPorc != null && this.Cuenta.ImpuestoPorc.Value.HasValue ? this.Cuenta.ImpuestoPorc.Value.Value : 0;

            decimal baseValML = this.Comprobante.Footer[index].vlrBaseML.Value.Value;
            decimal impRealML = Math.Round(baseValML * impuesto / 100);
            decimal baseValME = this.Comprobante.Footer[index].vlrBaseME.Value.Value;
            decimal impRealME = baseValME * impuesto / 100;

            decimal cellValML = this.Comprobante.Footer[index].vlrMdaLoc.Value.Value;
            decimal cellValME = this.Comprobante.Footer[index].vlrMdaExt.Value.Value;

            decimal difML = Math.Abs(impRealML) - Math.Abs(cellValML);
            decimal difMaxML = baseValML * (Decimal)0.01 / 100;
            decimal difME = Math.Abs(impRealME) - Math.Abs(cellValME);
            decimal difMaxME = baseValME * (Decimal)0.01 / 100;

            if (!this.biMoneda)
            {
                if (this.monedaId == monedaLocal)
                {
                    acceptCeroME = true;
                    if (impuesto != 0 && difML > difMaxML) invalidImpML = true;
                }
                else
                {
                    acceptCeroML = true;
                    if (impuesto != 0 && difME > difMaxME) invalidImpME = true;
                }
            }
            else
            {
                if (impuesto != 0 && difML > difMaxML) invalidImpML = true;
                if (impuesto != 0 && difME > difMaxME) invalidImpME = true;
            }

            bool valLoc = _bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "vlrMdaLoc", false, acceptCeroML, false, invalidImpML);
            bool valExt = !this.multiMoneda ? true : _bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "vlrMdaExt", false, acceptCeroME, false, invalidImpME);

            this.CalcularTotal();

            return (valLoc && valExt) ? true : false;
        }

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (this.Comprobante.Footer != null && this.Comprobante.Footer.Count == 0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }

            if (!this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            if (this.gvDocument.DataRowCount == 0)
                enable = false;

            this.masterCuenta.EnableControl(enable);
            this.masterTercero.EnableControl(enable);
            this.masterPrefijo.EnableControl(enable);
            this.txtDocumento.Enabled = enable;
            this.txtActivo.Enabled = enable;
            this.txtDescripcion.Enabled = enable;
            this.masterConceptoCargo.EnableControl(enable);
            this.masterProyecto.EnableControl(enable);
            this.masterCentroCosto.EnableControl(enable);
            this.masterLineaPre.EnableControl(enable);
            this.masterLugarGeo.EnableControl(enable);
            this.chkbIVA.Enabled = enable;
            this.cmbTarifa.Enabled = enable;
            this.btnSaldosByCuenta.Enabled = enable;
            this.txtBaseML.Enabled = enable;
            this.txtBaseME.Enabled = enable;
            this.txtValorML.Enabled = enable;
            this.txtValorME.Enabled = enable;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanHeader(bool basic)
        {
            this.dtFecha.DateTime = this.dtPeriod.DateTime;

            this.validHeader = false;
            this.ValidHeaderTB();

            this.CalcularTotal();
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected virtual void EnableHeader(bool enable) { }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected virtual Object LoadTempHeader() { return null; }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected virtual bool AsignarTasaCambio(bool fromTop) { return false; }

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
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="cta">Cuenta del detalle</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgVals">Mensaje paralos valores incorrectos</param>
        protected virtual void ValidateValoresImport(DTO_ComprobanteFooter dto, DTO_coPlanCuenta cta, DTO_TxResultDetail rd, string msgCero, string msgVals) 
        {
            bool createDTO = true;
            decimal impuesto = 0;

            #region Asignacion de tasa de cambio
            if (!this.multiMoneda)
                dto.TasaCambio.Value = 0;
            else
                dto.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
            #endregion
            #region Validacion de cuentas con impuestos
            if (cta.ImpuestoTipoID == null || string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
            {
                dto.vlrBaseML.Value = 0;
                dto.vlrBaseME.Value = 0;
            }
            else
            {
                impuesto = cta.ImpuestoPorc.Value.Value;
                if (this.biMoneda)
                {
                    #region Revisa que se hayan ingresado los valores bases de las monedas
                    if (dto.vlrBaseML.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    else if (this.multiMoneda && dto.vlrBaseME.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    #endregion
                }
                else
                {
                    #region Revisa los valores bases de la moneda con la que se esta trabajando
                    if (this.monedaId == this.monedaLocal && dto.vlrBaseML.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    else if (this.multiMoneda && this.monedaId == this.monedaExtranjera && dto.vlrBaseME.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    #endregion
                }
            }
            #endregion
            #region Validacion de cuentas que no manejan impuestos
            if (!this.biMoneda)
            {
                #region Revisa que exista el valor para la moneda que se esta ingresando
                if (this.monedaId == this.monedaLocal)
                {
                    if (dto.vlrMdaLoc.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                else if (this.monedaId == this.monedaExtranjera)
                {
                    if (this.multiMoneda && dto.vlrMdaExt.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                #endregion
            }
            #endregion
            #region Validacion de valores para cuentas con impuestos
            if (cta.ImpuestoTipoID != null && !string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
            {
                if (!this.multiMoneda)
                {
                    dto.vlrBaseME.Value = 0;
                    dto.vlrMdaExt.Value = 0;
                }

                decimal impRealML = Math.Round(dto.vlrBaseML.Value.Value * impuesto / 100);
                decimal impRealME = dto.vlrBaseME.Value.Value * impuesto / 100;

                decimal valML = dto.vlrMdaLoc.Value.Value;
                decimal valME = dto.vlrMdaExt.Value.Value;

                if (cta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                {
                    valML *= -1;
                    valME *= -1;
                }

                decimal difML = Math.Abs(impRealML) - Math.Abs(valML);
                decimal difMaxML = Math.Abs(dto.vlrBaseML.Value.Value * (Decimal)0.01 / 100);
                decimal difME = Math.Abs(impRealME) - Math.Abs(valME);
                decimal difMaxME = Math.Abs(dto.vlrBaseME.Value.Value * (Decimal)0.01 / 100);

                if (this.biMoneda)
                {
                    if (Math.Abs(difML) > Math.Abs(difMaxML))
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgVals;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    if (Math.Abs(difME) > Math.Abs(difMaxME))
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgVals;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                else if (this.monedaId == this.monedaLocal && Math.Abs(difML) > Math.Abs(difMaxML))
                {
                    string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = rsxField;
                    rdF.Message = msgVals;
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                else if (this.multiMoneda && this.monedaId == this.monedaExtranjera && Math.Abs(difME) > Math.Abs(difMaxME))
                {
                    string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = rsxField;
                    rdF.Message = msgVals;
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
            }

            #endregion
            #region Asigna los valores que deben ser calculados
            if (createDTO)
            {
                if (!this.multiMoneda)
                {
                    dto.vlrBaseME.Value = 0;
                    dto.vlrMdaExt.Value = 0;
                }

                if (cta.ImpuestoTipoID == null || string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
                {
                    dto.vlrBaseML.Value = 0;
                    dto.vlrBaseME.Value = 0;
                }

                dto.TasaCambio.Value = Math.Round(dto.TasaCambio.Value.Value, 2);
                dto.vlrBaseME.Value = Math.Round(dto.vlrBaseME.Value.Value, 2);
                dto.vlrMdaExt.Value = Math.Round(dto.vlrMdaExt.Value.Value, 2);
                //Valor de moneda extranjera
                dto.vlrBaseML.Value = Math.Round(dto.vlrBaseML.Value.Value, 2);
                dto.vlrMdaLoc.Value = Math.Round(dto.vlrMdaLoc.Value.Value, 2);

                if (this.biMoneda)
                {
                    //Valor de moneda local
                    dto.vlrBaseME.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseML.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaExt.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaLoc.Value / dto.TasaCambio.Value.Value;
                    //Valor de moneda extranjera
                    dto.vlrBaseML.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseME.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaLoc.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaExt.Value * dto.TasaCambio.Value.Value;

                    dto.vlrMdaOtr.Value = dto.vlrMdaLoc.Value;
                }
                else if (this.monedaId == this.monedaLocal)
                {
                    dto.vlrBaseME.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseML.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaExt.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaLoc.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaOtr.Value = dto.vlrMdaLoc.Value;
                }
                else if (this.multiMoneda && this.monedaId == this.monedaExtranjera)
                {
                    dto.vlrBaseML.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseME.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaLoc.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaExt.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaOtr.Value = dto.vlrMdaExt.Value;
                }

                //Valor de moneda local
                dto.vlrBaseME.Value = Math.Round(dto.vlrBaseME.Value.Value, 2);
                dto.vlrMdaExt.Value = Math.Round(dto.vlrMdaExt.Value.Value, 2);
                dto.vlrMdaOtr.Value = Math.Round(dto.vlrMdaOtr.Value.Value, 2);
                //Valor de moneda extranjera
                dto.vlrBaseML.Value = Math.Round(dto.vlrBaseML.Value.Value, 2);
                dto.vlrMdaLoc.Value = Math.Round(dto.vlrMdaLoc.Value.Value, 2);
            }
            #endregion
            #region Asihgna la informacion a las cuentas de IVA
            if (cta.ImpuestoTipoID.Value == this.tipoImpuestoIVA)
            {
                dto.DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                dto.DatoAdd2.Value = cta.ImpuestoPorc.Value.Value.ToString();
            }
            else
            {
                dto.DatoAdd1.Value = string.Empty;
                dto.DatoAdd2.Value = string.Empty;
            }
            #endregion
        }

        /// <summary>
        /// Trae los filtros complejos necesarios para la pnatalla de una maestra
        /// </summary>
        /// <param name="docIdFK"></param>
        /// <returns></returns>
        protected override List<DTO_glConsultaFiltro> GetFiltroComplejo(int docIdFK)
        {
            if (this.ConceptoSaldo.coSaldoControl.Value.Value == (byte)SaldoControl.Cuenta && docIdFK == AppMasters.coProyecto)
            {
                List<DTO_glConsultaFiltro> listaRel = new List<DTO_glConsultaFiltro>();
                List<DTO_glConsultaFiltro> listaFil = new List<DTO_glConsultaFiltro>();
                Dictionary<string, Type> types = new Dictionary<string, Type>();
                //FKs
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("OperacionID", "OperacionID");
                dict.Add("eg_coOperacion", "EmpresaGrupoID");

                //Valores que ya se tienen // los types se llenan con los datos que no estan en loas DTOS ... ej IDs de maestras y eg_s
                DTO_glConsultaFiltro eg = new DTO_glConsultaFiltro();
                eg.CampoFisico = "EmpresaGrupoID";
                eg.OperadorFiltro = OperadorFiltro.Igual;
                eg.ValorFiltro = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coControl);
                eg.OperadorSentencia = "AND";

                DTO_glConsultaFiltro cuentaId = new DTO_glConsultaFiltro();
                cuentaId.CampoFisico = "CuentaID";
                cuentaId.OperadorFiltro = OperadorFiltro.Igual;
                cuentaId.ValorFiltro = this.Cuenta.ID.Value;
                cuentaId.OperadorSentencia = "AND";
                types.Add("CuentaID", typeof(string));

                DTO_glConsultaFiltro egCuenta = new DTO_glConsultaFiltro();
                egCuenta.CampoFisico = "eg_coPlanCuenta";
                egCuenta.OperadorFiltro = OperadorFiltro.Igual;
                egCuenta.ValorFiltro = this.Cuenta.EmpresaGrupoID.Value;
                egCuenta.OperadorSentencia = "AND";
                types.Add("eg_coPlanCuenta", typeof(string));

                listaFil.Add(eg);
                listaFil.Add(cuentaId);
                listaFil.Add(egCuenta);

                DTO_glConsultaFiltroComplejo filtro = new DTO_glConsultaFiltroComplejo(AppMasters.coControl, dict, listaFil, types);
                listaRel.Add(filtro);
                return listaRel;
            }
            return null;
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
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, FormTypes.Document, this.frmModule);
                FormProvider.Master.itemImport.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemDelete.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    FormProvider.Master.itemCopy.Enabled = false;
                    FormProvider.Master.itemPaste.Enabled = false;
                    //FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemRevert.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos header superior

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_Leave(object sender, EventArgs e)
        {
            base.dtFecha_Leave(sender, e);
            bool tc = this.AsignarTasaCambio(true);
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
                    else
                        this.gvDocument.PostEditor();

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
                            }
                            else
                            {
                                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                if (isV)
                                {
                                    this.newReg = true;
                                    this.AddNewRow();
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

                            if (this.Comprobante.Footer.Count == 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                e.Handled = true;
                            }
                            else
                            {
                                this.Comprobante.Footer.RemoveAll(x => x.Index == this.indexFila);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs-", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "ProyectoID" || fieldName == "CentroCostoID" || fieldName == "LineaPresupuestoID" ||
                fieldName == "ConceptoCargoID" || fieldName == "LugarGeograficoID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "vlrBaseML" || fieldName == "vlrBaseME" || fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt")
            {
                e.RepositoryItem = this.editSpin;
            }
            if (fieldName == "TasaCambio")
            {
                e.RepositoryItem = this.editSpin4;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName != "Marca")
            {
                int index = this.NumFila;
                decimal impuesto = this.Cuenta.ImpuestoPorc != null && this.Cuenta.ImpuestoPorc.Value.HasValue ? this.Cuenta.ImpuestoPorc.Value.Value : 0;

                bool validField = true;
                #region FKs
                if (fieldName == "CentroCostoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                    if (validField)
                        this.masterCentroCosto.Value = e.Value.ToString();
                }
                if (fieldName == "LineaPresupuestoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.plLineaPresupuesto);
                    if (validField)
                        this.masterLineaPre.Value = e.Value.ToString();
                }
                if (fieldName == "LugarGeograficoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.glLugarGeografico);
                    if (validField)
                        this.masterLugarGeo.Value = e.Value.ToString();
                }
                if (fieldName == "ConceptoCargoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coConceptoCargo);
                    if (validField)
                        this.masterConceptoCargo.Value = e.Value.ToString();
                }
                if (fieldName == "ProyectoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coProyecto);
                    if (validField)
                        this.masterProyecto.Value = e.Value.ToString();
                }
                #endregion
                #region Proyecto y Concepto de Cargo con la tabla de coCargoCosto
                if (fieldName == "ConceptoCargoID" || fieldName == "ProyectoID")
                {
                    bool validCargo = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, "ConceptoCargoID", false, true, true, AppMasters.coConceptoCargo);
                    if (validCargo && this.saldoControl == SaldoControl.Cuenta && this._validCtaProyCC && this.Cuenta.ConceptoCargoInd.Value.Value)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                        GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"];
                        string concCargoID = this.gvDocument.GetRowCellValue(e.RowHandle, col).ToString();

                        if (this.Cuenta.ProyectoInd.Value.Value)
                        {
                            bool validProy = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, "ProyectoID", false, true, true, AppMasters.coProyecto);
                            if (validProy)
                            {
                                //Proyecto
                                col = this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"];
                                string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                                string proyID = this.gvDocument.GetRowCellValue(e.RowHandle, col).ToString();
                                string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, proyID, string.Empty, linPresID).Trim();
                                if (string.IsNullOrWhiteSpace(ctaCargoCosto))
                                {
                                    col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                    this.gvDocument.SetColumnError(col, msg);

                                    validField = false;
                                }
                            }
                        }
                        else if (this.Cuenta.CentroCostoInd.Value.Value)
                        {
                            bool validCtoCosto = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                            if (validCtoCosto)
                            {
                                //Proyecto
                                col = this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"];
                                string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                                string ctoCostoID = this.gvDocument.GetRowCellValue(e.RowHandle, col).ToString();
                                string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, string.Empty, ctoCostoID, linPresID).Trim();
                                if (string.IsNullOrWhiteSpace(ctaCargoCosto))
                                {
                                    col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                    this.gvDocument.SetColumnError(col, msg);

                                    validField = false;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Tasa de cambio
                if (fieldName == "TasaCambio")
                {
                    if (this.Comprobante.Footer[index].TasaCambio.Value == null)
                        this.Comprobante.Footer[index].TasaCambio.Value = 0;

                    if (!this.biMoneda)
                    {
                        if (this.monedaId == this.monedaLocal)
                        {
                            decimal impExt = this.Comprobante.Footer[index].vlrBaseME.Value.Value * impuesto / 100;
                            decimal currExt = this.Comprobante.Footer[index].vlrBaseME.Value.Value;
                            decimal vlrExt = currExt + impExt;

                            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                vlrExt *= -1;

                            this.Comprobante.Footer[index].vlrMdaExt.Value = vlrExt;
                        }
                        else if (this.multiMoneda)
                        {
                            decimal impLoc = this.Comprobante.Footer[index].vlrBaseML.Value.Value * impuesto / 100;
                            decimal currLoc = this.Comprobante.Footer[index].vlrBaseML.Value.Value;
                            decimal vlrLoc = currLoc + impLoc;

                            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                vlrLoc *= -1;

                            this.Comprobante.Footer[index].vlrMdaLoc.Value = vlrLoc;
                        }
                    }

                    validField = this.CalcularValorExtra();
                }
                #endregion
                #region Se modifican los valores bases
                if (fieldName == "vlrBaseML")
                {
                    if (this.Comprobante.Footer[index].vlrBaseML.Value == null)
                        this.Comprobante.Footer[index].vlrBaseML.Value = 0;

                    decimal imp = Math.Round(this.Comprobante.Footer[index].vlrBaseML.Value.Value * impuesto / 100);

                    if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                        imp *= -1;

                    this.Comprobante.Footer[index].vlrMdaLoc.Value = imp;
                    this.txtBaseML.EditValue = e.Value;
                    this.txtValorML.EditValue = imp;
                    validField = this.CalcularValorExtra();
                }
                if (fieldName == "vlrBaseME")
                {
                    if (this.Comprobante.Footer[index].vlrBaseME.Value == null)
                        this.Comprobante.Footer[index].vlrBaseME.Value = 0;

                    decimal imp = this.Comprobante.Footer[index].vlrBaseME.Value.Value * impuesto / 100;

                    if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                        imp *= -1;

                    this.Comprobante.Footer[index].vlrMdaExt.Value = imp;
                    this.txtBaseME.EditValue = e.Value;
                    this.txtValorME.EditValue = imp;
                    validField = this.CalcularValorExtra();
                }
                #endregion
                #region Se modifican los valores totales
                if (fieldName == "vlrMdaLoc" || fieldName == "vlrMdaExt")
                {
                    if (impuesto != 0 && this.Comprobante.Footer[index].TerceroID.Value != this.nitDIAN)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_ValorBaseInvalid);
                        GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

                        if (fieldName == "vlrMdaLoc")
                        {
                            decimal impReal = this.Comprobante.Footer[index].vlrBaseML.Value.Value * impuesto / 100;
                            decimal cellVal = this.Comprobante.Footer[index].vlrMdaLoc.Value.Value;

                            decimal dif = Math.Abs(impReal - cellVal);
                            decimal difMax = this.Comprobante.Footer[index].vlrBaseML.Value.Value * (Decimal)0.01 / 100;
                            this.txtValorML.EditValue = cellVal;

                            if (dif > difMax)
                            {
                                decimal nuevaBase = Math.Round((Math.Abs(cellVal) / impuesto) * 100,2);
                                string msgBase = string.Format(msg, nuevaBase);
                                this.gvDocument.SetColumnError(col, msgBase);
                                validField = false;
                            }
                            else
                                validField = this.CalcularValorExtra();
                        }
                        if (fieldName == "vlrMdaExt")
                        {
                            decimal impReal = this.Comprobante.Footer[index].vlrBaseME.Value.Value * impuesto / 100;
                            decimal cellVal = this.Comprobante.Footer[index].vlrMdaExt.Value.Value;

                            decimal dif = Math.Abs(impReal - cellVal);
                            decimal difMax = this.Comprobante.Footer[index].vlrBaseME.Value.Value * (Decimal)0.01 / 100;
                            this.txtValorME.EditValue = cellVal;

                            if (dif > difMax)
                            {
                                decimal nuevaBase = Math.Round((Math.Abs(cellVal) / impuesto) * 100, 2);
                                string msgBase = string.Format(msg, nuevaBase);
                                this.gvDocument.SetColumnError(col, msgBase);
                                validField = false;
                            }
                            else
                                validField = this.CalcularValorExtra();
                        }
                    }
                    else
                    {
                        validField = this.CalcularValorExtra();
                    }
                }
                #endregion

                //this.RowEdited = true;
                if (!validField)
                    this.isValid = false;

                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.disableValidate)
            {
                bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                this.deleteOP = false;

                if (validRow)
                    this.isValid = true;
                else
                {
                    e.Allow = false;
                    this.isValid = false;
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

            #region Validacion para llaves foraneas
            if (this.saldoControl == SaldoControl.Cuenta && this._cuenta != null)
            {
                if
                (
                    (!this._cuenta.ProyectoInd.Value.Value && fieldName == "ProyectoID") ||
                    (!this._cuenta.CentroCostoInd.Value.Value && fieldName == "CentroCostoID") ||
                    (!this._cuenta.LineaPresupuestalInd.Value.Value && fieldName == "LineaPresupuestoID") ||
                    (!this._cuenta.LugarGeograficoInd.Value.Value && fieldName == "LugarGeograficoID")
                )
                    editableCell = false;
                if (fieldName == "ConceptoCargoID" && this._conceptoCargoCta != string.Empty)
                {
                    editableCell = false;
                }
            }
            else
            {
                if (fieldName == "ConceptoCargoID" || fieldName == "LugarGeograficoID")
                {
                    editableCell = false;
                }
            }
            #endregion
            #region Validacion de la base segun el tipo de impuesto
            if (this._cuenta != null && (this._cuenta.ImpuestoTipoID == null || string.IsNullOrEmpty(this._cuenta.ImpuestoTipoID.Value)))
            {
                if (fieldName == "vlrBaseML" || fieldName == "vlrBaseME")
                    editableCell = false;
            }
            else if (!this.biMoneda)
            {
                if (this.monedaId == this.monedaLocal && fieldName == "vlrBaseME")
                    editableCell = false;
                else if (this.monedaId == this.monedaExtranjera && fieldName == "vlrBaseML")
                    editableCell = false;
            }
            #endregion
            #region Validacion de valores
            if (!this.biMoneda)
            {
                if (this.monedaId == this.monedaLocal && fieldName == "vlrMdaExt")
                    editableCell = false;
                else if (this.monedaId == this.monedaExtranjera && fieldName == "vlrMdaLoc")
                    editableCell = false;
            }
            #endregion

            if (editableCell)
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.White;
            else
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.Lavender;
        }

        #endregion

        #region Eventos Detalle (footer)

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void masterDetails_Leave(object sender, EventArgs e)
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
                        case "CuentaID":
                            #region Cuenta
                            if (master.ValidID)
                            {
                                if (this.Cuenta == null || master.Value != this.Cuenta.ID.Value)
                                {
                                    this.Comprobante.Footer[index].CuentaID.Value = master.Value;
                                    this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, master.Value, true);
                                    #region Limpia los valores y los controles
                                    this.masterCuenta.EnableControl(true);
                                    this.txtDescripcion.Enabled = true;
                                    #endregion
                                    #region Asigna valores a columnas
                                    if (this.saldoControl == SaldoControl.Cuenta)
                                    {
                                        this.Comprobante.Footer[index].IdentificadorTR.Value = 0;
                                        #region Tercero
                                        if (!this._cuenta.TerceroInd.Value.Value)
                                        {
                                            this.masterTercero.Value = this.defTercero;
                                            this.Comprobante.Footer[index].TerceroID.Value = this.defTercero;
                                        }
                                        #endregion
                                        #region Proyecto
                                        if (!this._cuenta.ProyectoInd.Value.Value)
                                        {
                                            this.masterProyecto.Value = this.defProyecto;
                                            this.Comprobante.Footer[index].ProyectoID.Value = this.defProyecto;
                                        }
                                        #endregion
                                        #region Centro Costo
                                        if (!this._cuenta.CentroCostoInd.Value.Value)
                                        {
                                            this.masterCentroCosto.Value = this.defCentroCosto;
                                            this.Comprobante.Footer[index].CentroCostoID.Value = this.defCentroCosto;
                                        }
                                        #endregion
                                        #region Linea presupuesto
                                        if (!this._cuenta.LineaPresupuestalInd.Value.Value)
                                        {
                                            this.masterLineaPre.Value = this.defLineaPresupuesto;
                                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                        }
                                        #endregion
                                        #region Concepto Cargo
                                        if (!this._cuenta.ConceptoCargoInd.Value.Value)
                                        {
                                            this.masterConceptoCargo.Value = this.defConceptoCargo;
                                            this.Comprobante.Footer[index].ConceptoCargoID.Value = this.defConceptoCargo;
                                        }
                                        else
                                        {
                                            if (this._conceptoCargoCta != string.Empty)
                                            {
                                                this.masterConceptoCargo.Value = this._conceptoCargoCta;
                                                this.Comprobante.Footer[index].ConceptoCargoID.Value = this._conceptoCargoCta;
                                            }
                                        }
                                        #endregion
                                        #region Lugar Geografico
                                        if (!this._cuenta.LugarGeograficoInd.Value.Value)
                                        {
                                            this.masterLugarGeo.Value = this.defLugarGeo;
                                            this.Comprobante.Footer[index].LugarGeograficoID.Value = this.defLugarGeo;
                                        }
                                        #endregion
                                        #region Prefijo
                                        this.masterPrefijo.Value = this.defPrefijo;
                                        this.Comprobante.Footer[index].PrefijoCOM.Value = this.defPrefijo;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Limpia los controles
                                        this.masterTercero.Value = string.Empty;
                                        this.masterPrefijo.Value = string.Empty;
                                        this.masterConceptoCargo.Value = string.Empty;
                                        this.masterProyecto.Value = string.Empty;
                                        this.masterCentroCosto.Value = string.Empty;
                                        this.masterLineaPre.Value = string.Empty;
                                        this.masterLugarGeo.Value = string.Empty;
                                        this.txtDocumento.Text = string.Empty;
                                        this.txtActivo.Text = string.Empty;
                                        //this.txtDescripcion.Text = string.Empty;

                                        this.Comprobante.Footer[index].TerceroID.Value = string.Empty;
                                        this.Comprobante.Footer[index].PrefijoCOM.Value = string.Empty;
                                        this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                                        this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                                        this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                                        this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                                        this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                                        this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                                        this.Comprobante.Footer[index].ActivoCOM.Value = string.Empty;
                                        //this.Comprobante.Footer[index].Descriptivo.Value = string.Empty;
                                        #endregion

                                        this.Comprobante.Footer[index].ConceptoCargoID.Value = this.defConceptoCargo;
                                        this.masterConceptoCargo.Value = this.defConceptoCargo;
                                        this.Comprobante.Footer[index].LugarGeograficoID.Value = this.defLugarGeo;
                                        this.masterLugarGeo.Value = this.defLugarGeo;
                                        this.Comprobante.Footer[index].DatoAdd1.Value = string.Empty;
                                        this.Comprobante.Footer[index].DatoAdd2.Value = string.Empty;
                                    }

                                    #endregion
                                    
                                    this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                }
                            }
                            else if(this.Cuenta != null)
                            {
                                #region Limpia los controles
                                this.masterTercero.Value = string.Empty;
                                this.masterPrefijo.Value = string.Empty;
                                this.masterConceptoCargo.Value = string.Empty;
                                this.masterProyecto.Value = string.Empty;
                                this.masterCentroCosto.Value = string.Empty;
                                this.masterLineaPre.Value = string.Empty;
                                this.masterLugarGeo.Value = string.Empty;
                                this.txtDocumento.Text = string.Empty;
                                this.txtActivo.Text = string.Empty;
                                //this.txtDescripcion.Text = string.Empty;

                                this.Comprobante.Footer[index].TerceroID.Value = string.Empty;
                                this.Comprobante.Footer[index].PrefijoCOM.Value = string.Empty;
                                this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                                this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                                this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                                this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                                this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                                this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                                this.Comprobante.Footer[index].ActivoCOM.Value = string.Empty;
                                this.Comprobante.Footer[index].PrefDoc = string.Empty;
                                #endregion

                                this.Cuenta = null;
                                this.Tercero = null;
                                this.ConceptoSaldo = null;
                                this.masterCuenta.EnableControl(true);
                                this.btnSaldosByCuenta.Enabled = false;
                            }

                            #endregion
                            break;
                        case "TerceroID":
                            #region Tercero
                            if (master.ValidID)
                            {
                                this.Comprobante.Footer[index].TerceroID.Value = master.Value;

                                col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                                this.gvDocument.SetColumnError(col, string.Empty);
                                this.btnSaldosByCuenta.Enabled = this.masterCuenta.ValidID && (this.saldoControl == SaldoControl.Componente_Documento || this.saldoControl == SaldoControl.Doc_Interno ||  this.saldoControl == SaldoControl.Doc_Externo) ? true : false;
                            }
                            else
                            {
                                this.Comprobante.Footer[index].TerceroID.Value = string.Empty;
                                this.btnSaldosByCuenta.Enabled = false;
                            }

                            this.txtActivo.Text = string.Empty;

                            #region Componente tercero
                            if (this.saldoControl == SaldoControl.Componente_Tercero)
                            {
                                if (this.masterTercero.ValidID)
                                {
                                    if (this.Tercero == null || master.Value != this.Tercero.ID.Value)
                                    {
                                        this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);
                                        long idTR = Convert.ToInt64(master.Value);

                                        DTO_coCuentaSaldo saldo = _bc.AdministrationModel.Saldo_GetByDocumento(this.Cuenta.ID.Value, this.ConceptoSaldo.ID.Value, idTR, string.Empty);
                                        this.Comprobante.Footer[index].IdentificadorTR.Value = idTR;
                                        #region Asigna valores a columnas
                                        if (saldo == null)
                                        {
                                            #region Nuevo saldo

                                            #region Proyecto
                                            if (!this._cuenta.ProyectoInd.Value.Value)
                                            {
                                                this.masterProyecto.Value = this.defProyecto;
                                                this.Comprobante.Footer[index].ProyectoID.Value = this.defProyecto;
                                            }
                                            #endregion
                                            #region Centro Costo
                                            if (!this._cuenta.CentroCostoInd.Value.Value)
                                            {
                                                this.masterCentroCosto.Value = this.defCentroCosto;
                                                this.Comprobante.Footer[index].CentroCostoID.Value = this.defCentroCosto;
                                            }
                                            #endregion
                                            #region Linea presupuesto
                                            if (!this._cuenta.LineaPresupuestalInd.Value.Value)
                                            {
                                                this.masterLineaPre.Value = this.defLineaPresupuesto;
                                                this.Comprobante.Footer[index].LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                            }
                                            #endregion
                                            #region Concepto Cargo
                                            if (!this._cuenta.ConceptoCargoInd.Value.Value)
                                            {
                                                this.masterConceptoCargo.Value = this.defConceptoCargo;
                                                this.Comprobante.Footer[index].ConceptoCargoID.Value = this.defConceptoCargo;
                                            }
                                            else
                                            {
                                                if (this._conceptoCargoCta != string.Empty)
                                                {
                                                    this.masterConceptoCargo.Value = this._conceptoCargoCta;
                                                    this.Comprobante.Footer[index].ConceptoCargoID.Value = this._conceptoCargoCta;
                                                }
                                            }
                                            #endregion
                                            #region Lugar Geografico
                                            if (!this._cuenta.LugarGeograficoInd.Value.Value)
                                            {
                                                this.masterLugarGeo.Value = this.defLugarGeo;
                                                this.Comprobante.Footer[index].LugarGeograficoID.Value = this.defLugarGeo;
                                            }
                                            #endregion

                                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = true;
                                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = true;
                                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = true;
                                            this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = true;
                                            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = true;
                                            this.masterProyecto.EnableControl(true);
                                            this.masterCentroCosto.EnableControl(true);
                                            this.masterLineaPre.EnableControl(true);
                                            this.masterConceptoCargo.EnableControl(true);
                                            this.masterLugarGeo.EnableControl(true);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Saldo existente
                                            this.Comprobante.Footer[index].ProyectoID.Value = saldo.ProyectoID.Value;
                                            this.Comprobante.Footer[index].CentroCostoID.Value = saldo.CentroCostoID.Value;
                                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                                            this.Comprobante.Footer[index].ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                                            this.Comprobante.Footer[index].LugarGeograficoID.Value = this.defLugarGeo;
                                            this.masterProyecto.Value = saldo.ProyectoID.Value;
                                            this.masterCentroCosto.Value =saldo.CentroCostoID.Value;
                                            this.masterLineaPre.Value = saldo.LineaPresupuestoID.Value;
                                            this.masterConceptoCargo.Value = saldo.ConceptoCargoID.Value;
                                            this.masterLugarGeo.Value = this.defLugarGeo;
                                            #endregion
                                        }

                                        #region Prefijo
                                        this.masterPrefijo.Value = this.defPrefijo;
                                        this.Comprobante.Footer[index].PrefijoCOM.Value = this.defPrefijo;
                                        #endregion

                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region Tercero invalido

                                    this.Tercero = null;

                                    this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                                    this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                                    this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                                    this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                                    this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                                    this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                                    this.masterProyecto.Value = string.Empty;
                                    this.masterCentroCosto.Value = string.Empty;
                                    this.masterLineaPre.Value = string.Empty;
                                    this.masterConceptoCargo.Value = string.Empty;
                                    this.masterLugarGeo.Value = string.Empty;

                                    this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                                    this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                                    this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                                    this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                                    this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                                    this.masterProyecto.EnableControl(false);
                                    this.masterCentroCosto.EnableControl(false);
                                    this.masterLineaPre.EnableControl(false);
                                    this.masterConceptoCargo.EnableControl(false);
                                    this.masterLugarGeo.EnableControl(false);
                                    #endregion
                                }

                                _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "ProyectoID", false, true, false, AppMasters.coProyecto);
                                _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "CentroCostoID", false, true, false, AppMasters.coCentroCosto);
                                _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "LineaPresupuestoID", false, true, false, AppMasters.plLineaPresupuesto);
                                _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "ConceptoCargoID", false, true, false, AppMasters.coConceptoCargo);
                                _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "LugarGeograficoID", false, true, false, AppMasters.glLugarGeografico);
                            }
                            #endregion

                            _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "TerceroID", false, true, false, AppMasters.coTercero);
                            #endregion
                            break;
                        case "PrefijoID":
                            #region Prefijo COM
                            if (master.ValidID)
                            {
                                this.Comprobante.Footer[index].PrefijoCOM.Value = master.Value;

                                col = this.gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"];
                                this.gvDocument.SetColumnError(col, string.Empty);                               
                            }
                            else
                                this.Comprobante.Footer[index].PrefijoCOM.Value = string.Empty;
                                    
                            this.txtActivo.Text = string.Empty;
                            _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "PrefijoCOM", false, true, false, AppMasters.glPrefijo);

                            if (documentID == AppDocuments.CausarFacturas)
                            {
                                this.Comprobante.Footer[index].PrefDoc = this.Comprobante.Footer[index].PrefijoCOM.Value.ToString().Trim() + "  -  " + this.Comprobante.Footer[index].DocumentoCOM.Value.ToString().Trim();
                                this.gvDocument.SetColumnError(this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"], this.gvDocument.GetColumnError(col));
                            }
                            #endregion
                            break;
                        case "ConceptoCargoID" :
                            #region ConceptoCargo
                            if (master.ValidID)
                            {
                                if (master.txtCode.Enabled)
                                {
                                    this.Comprobante.Footer[index].ConceptoCargoID.Value = master.Value;
                                    col = this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"];
                                    this.gvDocument.SetColumnError(col, string.Empty);

                                    this.AsignarCuenta(master.ColId, master.Value);

                                    if (this.saldoControl == SaldoControl.Cuenta && this._validCtaProyCC && this.Cuenta != null && this.Cuenta.ConceptoCargoInd.Value.Value)
                                    {
                                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                                        string concCargoID = master.Value;

                                        if (this.Cuenta.ProyectoInd.Value.Value && this.masterProyecto.ValidID)
                                        {
                                            string linPresID = this.masterLineaPre.Value;
                                            string proyID = this.masterProyecto.Value;
                                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, proyID, string.Empty, linPresID).Trim();
                                            if (string.IsNullOrWhiteSpace(ctaCargoCosto))
                                            {
                                                col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                                this.gvDocument.SetColumnError(col, msg);
                                            }
                                        }
                                        else if (this.Cuenta.CentroCostoInd.Value.Value && this.masterCentroCosto.ValidID)
                                        {
                                            col = this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"];
                                            string linPresID = this.masterLineaPre.Value;
                                            string ctoCostoID = this.masterCentroCosto.Value;
                                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(concCargoID, string.Empty, ctoCostoID, linPresID).Trim();
                                            if (string.IsNullOrWhiteSpace(ctaCargoCosto))
                                            {
                                                col = this.gvDocument.Columns[this.unboundPrefix + "CuentaID"];
                                                this.gvDocument.SetColumnError(col, msg);
                                            }
                                        }
                                    } 
                                }
                            }
                            else
                            {
                                this.masterConceptoCargo.Value = string.Empty;
                                this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                            }
                           
                            #endregion
                            break;                
                        case "ProyectoID":
                            #region ProyectoID
                            if (master.ValidID)
                            {
                                if (master.txtCode.Enabled)
                                {
                                    this.Comprobante.Footer[index].ProyectoID.Value = master.Value;
                                    col = this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"];
                                    this.gvDocument.SetColumnError(col, string.Empty);

                                    this.AsignarCuenta(master.ColId, master.Value);
                                }
                            }
                            else
                            {
                                this.masterProyecto.Value = string.Empty;
                                this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                            }

                            #endregion
                            break;
                        case "CentroCostoID":
                            #region CentroCostoID
                            if (master.ValidID)
                            {
                                if (master.txtCode.Enabled)
                                {
                                    this.Comprobante.Footer[index].CentroCostoID.Value = master.Value;
                                    col = this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"];
                                    this.gvDocument.SetColumnError(col, string.Empty);

                                    this.AsignarCuenta(master.ColId, master.Value);
                                }
                            }
                            else
                            {
                                this.masterCentroCosto.Value = string.Empty;
                                this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                            }

                            #endregion
                            break;
                        case "LineaPresupuestoID":
                            #region LineaPresupuestoID
                            if (master.ValidID)
                            {
                                if (master.txtCode.Enabled)
                                {
                                    this.Comprobante.Footer[index].LineaPresupuestoID.Value = master.Value;
                                    col = this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"];
                                    this.gvDocument.SetColumnError(col, string.Empty);

                                    this.AsignarCuenta(master.ColId, master.Value);
                                }
                            }
                            else
                            {
                                this.masterLineaPre.Value = string.Empty;
                                this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                            }

                            #endregion
                            break;
                        case "LugarGeograficoID":
                            #region LugarGeograficoID
                            if (master.ValidID)
                            {
                                if (master.txtCode.Enabled)
                                {
                                    this.Comprobante.Footer[index].LugarGeograficoID.Value = master.Value;
                                    col = this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"];
                                    this.gvDocument.SetColumnError(col, string.Empty);
                                }
                            }
                            else
                            {
                                this.masterLugarGeo.Value = string.Empty;
                                this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                            }

                            #endregion
                            break;
                    }

                    this.gvDocument.RefreshData();
                    //this.RowEdited = true;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "masterDetails_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de descripcion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            GridColumn col = new GridColumn();
            int index = this.NumFila;

            switch (ctrl.Name)
            {
                // Documento
                case "txtDocumento":
                    #region Documento
                    DTO_glDocumentoControl docCtrl = null;
                    if (this.saldoControl == SaldoControl.Doc_Interno)
                    {
                        #region Documento Interno
                        #region Valida que exista un prefijo
                        if (!this.masterPrefijo.ValidID)
                        {
                            string rsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppMasters.glPrefijo + "_" + "PrefijoID");
                            string res = string.Format(rsx, colRsx);
                            MessageBox.Show(res);
                            return;
                        }
                        #endregion
                        #region Revisa que el documento sea numerico
                        int docIntId = 0;
                        try
                        {
                            if (string.IsNullOrEmpty(this.txtDocumento.Text))
                                return;
                            docIntId = Convert.ToInt16(this.txtDocumento.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NumericDocInt));
                            return;
                        }
                        #endregion
                        #region Trae el documento
                        docCtrl = _bc.AdministrationModel.glDocumentoControl_GetInternalDocByCta(this.Cuenta.ID.Value, this.masterPrefijo.Value, docIntId);
                        if (docCtrl == null)
                        {
                            this.btnSaldos.Enabled = false;
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                            this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                        }
                        else
                        {
                            #region Carga la info del documento
                            //Tercero
                            this.Comprobante.Footer[index].TerceroID.Value = docCtrl.TerceroID.Value;
                            this.masterTercero.Value = docCtrl.TerceroID.Value;
                            // Proyecto
                            this.Comprobante.Footer[index].ProyectoID.Value = docCtrl.ProyectoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                            this.masterProyecto.Value = docCtrl.ProyectoID.Value;
                            // CentrocostoID
                            this.Comprobante.Footer[index].CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                            this.masterCentroCosto.Value = docCtrl.CentroCostoID.Value;
                            // LineaPresupuestoID
                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                            this.masterLineaPre.Value = docCtrl.LineaPresupuestoID.Value;
                            // Valor
                            if (this.documentID == AppDocuments.CruceCuentas)
                            {
                                this.Comprobante.Footer[index].Descriptivo.Value = this.txtDescripcion.Text;
                                this.txtDescripcion.Text = "CRUCE DE CUENTAS";                               
                                this.Comprobante.Footer[index].vlrMdaLoc.Value = docCtrl.Valor.Value;
                                this.txtValorML.EditValue = docCtrl.Valor.Value;
                                this.txtEdit_Leave(this.txtValorML, e);
                            }
                        
                            //Asigna el valor a la variable DTO
                            this._docCtrlDTO = docCtrl;
                            //Habilita el boton de saldos
                            this.btnSaldos.Enabled = true;

                            //Info de las columnas
                            this.Comprobante.Footer[index].DocumentoCOM.Value = ctrl.Text;
                            this.Comprobante.Footer[index].IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                            #endregion
                            this.ValidateRow(this.gvDocument.FocusedRowHandle);
                        }
                        #endregion
                        #endregion
                    }
                    else if (this.saldoControl == SaldoControl.Doc_Externo || this.saldoControl == SaldoControl.Componente_Documento)
                    {
                        #region Documento Externo
                        #region Valida que exista un tercero
                        if (!this.masterTercero.ValidID)
                        {
                            string rsx = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppMasters.coTercero + "_" + "TerceroID");
                            string res = string.Format(rsx, colRsx);
                            MessageBox.Show(res);
                            return;
                        }
                        #endregion
                        #region Trae el documento
                        docCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(this.Cuenta.ID.Value, this.masterTercero.Value, this.txtDocumento.Text);
                        if (docCtrl == null)
                        {
                            this.btnSaldos.Enabled = false;
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                            this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                        }
                        else
                        {
                            #region Carga la info del documento
                            // Prefijo
                            this.Comprobante.Footer[index].PrefijoCOM.Value = docCtrl.PrefijoID.Value;
                            this.masterPrefijo.Value = docCtrl.PrefijoID.Value;
                            // Proyecto
                            this.Comprobante.Footer[index].ProyectoID.Value = docCtrl.ProyectoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                            // CentrocostoID
                            this.Comprobante.Footer[index].CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                            // LineaPresupuestoID
                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                            //Asigna el valor a la variable DTO
                            this._docCtrlDTO = docCtrl;
                            //Habilita el boton de saldos
                            this.btnSaldos.Enabled = true;

                            //Info de las columnas
                            this.Comprobante.Footer[index].DocumentoCOM.Value = ctrl.Text;
                            this.Comprobante.Footer[index].IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;

                            #endregion
                            this.ValidateRow(this.gvDocument.FocusedRowHandle);
                        }
                        #endregion
                        #endregion
                    }
                    else
                    {
                        if (this.Comprobante.Footer != null && this.Comprobante.Footer.Count > 0)
                            this.Comprobante.Footer[index].DocumentoCOM.Value = this.txtDocumento.Text;

                        _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "DocumentoCOM", false, false, false, null);
                        this.btnSaldos.Visible = false;

                        if (documentID == AppDocuments.CausarFacturas)
                        {
                            this.Comprobante.Footer[index].PrefDoc = this.Comprobante.Footer[index].PrefijoCOM.Value.ToString().Trim() + "  -  " + this.Comprobante.Footer[index].DocumentoCOM.Value.ToString().Trim();
                            this.gvDocument.SetColumnError(this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"], this.gvDocument.GetColumnError(col));
                        }
                    }

                    #endregion
                    break;

                case "txtActivo":
                    #region Activo
                    DTO_acActivoControl acCtrl = _bc.AdministrationModel.acActivoControl_GetByPlaqueta(this.txtActivo.Text);
                    if (acCtrl == null)
                    {
                        // No existe el documento
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidActData));

                        //Info de las columnas
                        this.Comprobante.Footer[index].ActivoCOM.Value = string.Empty;
                        this.txtDocumento.Focus();
                    }
                    else
                    {
                        #region Carga la info del documento
                        // Tercero 
                        this.masterTercero.Value = acCtrl.TerceroID.Value;
                        this.Comprobante.Footer[index].TerceroID.Value = acCtrl.TerceroID.Value;
                        // Prefijo
                        this.masterPrefijo.Value = defPrefijo;
                        this.Comprobante.Footer[index].PrefijoCOM.Value = defPrefijo;
                        // Documento
                        this.txtDocumento.Text = ctrl.Text;
                        this.Comprobante.Footer[index].DocumentoCOM.Value = ctrl.Text;
                        // Proyecto
                        this.Comprobante.Footer[index].ProyectoID.Value = acCtrl.ProyectoID.Value;
                        this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                        // CentrocostoID
                        this.Comprobante.Footer[index].CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                        this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                        // LineaPresupuestoID
                        this.Comprobante.Footer[index].LineaPresupuestoID.Value = defLineaPresupuesto;
                        this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                        //Info de las columnas
                        this.Comprobante.Footer[index].ActivoCOM.Value = ctrl.Text;
                        this.Comprobante.Footer[index].IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                        #endregion
                    }

                    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "ActivoCOM", false, false, false, null);

                    #endregion
                    break;

                case "txtDescripcion":
                    #region Descriptivo
                    this.Comprobante.Footer[index].Descriptivo.Value = ctrl.Text;
                    _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "Descriptivo", false, false, false, null);
                    #endregion
                    break;
            }

            this.gvDocument.RefreshData();
            //this.RowEdited = true;
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de descripcion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtEdit_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0)
                {
                    DevExpress.XtraEditors.TextEdit txt = (DevExpress.XtraEditors.TextEdit)sender;
                    GridColumn col = new GridColumn();
                    string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                    int index = this.NumFila;
                    decimal impuesto = this.Cuenta.ImpuestoPorc != null && this.Cuenta.ImpuestoPorc.Value.HasValue ? this.Cuenta.ImpuestoPorc.Value.Value : 0;
                    bool validField = true;
                    decimal imp = 0;

                    switch (txt.Name)
                    {
                        case "txtBaseML":
                            this.Comprobante.Footer[index].vlrBaseML.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                            imp = Math.Round(this.Comprobante.Footer[index].vlrBaseML.Value.Value * impuesto / 100);

                            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                imp *= -1;

                            if (this.documentID != AppDocuments.ComprobanteAjuste)
                            {
                                this.Comprobante.Footer[index].vlrMdaLoc.Value = imp;
                                this.txtValorML.EditValue = imp;
                                validField = this.CalcularValorExtra();
                            }                           
                            break;
                        case "txtBaseME":
                            this.Comprobante.Footer[index].vlrBaseME.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                            imp = this.Comprobante.Footer[index].vlrBaseME.Value.Value * impuesto / 100;

                            if (this.Cuenta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                imp *= -1;
                            this.Comprobante.Footer[index].vlrMdaExt.Value = imp;
                            this.txtValorME.EditValue = imp;
                            validField = this.CalcularValorExtra();
                            break;
                        case "txtValorML":
                            this.Comprobante.Footer[index].vlrMdaLoc.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                            if (impuesto != 0 && this.Comprobante.Footer[index].TerceroID.Value != this.nitDIAN)
                            {
                                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_ValorBaseInvalid);

                                col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

                                decimal impReal = this.Comprobante.Footer[index].vlrBaseML.Value.Value * impuesto / 100;
                                decimal cellVal = this.Comprobante.Footer[index].vlrMdaLoc.Value.Value;

                                decimal dif = Math.Abs(impReal - cellVal);
                                decimal difMax = this.Comprobante.Footer[index].vlrBaseML.Value.Value * (Decimal)0.01 / 100;

                                if (dif > difMax)
                                {
                                    decimal nuevaBase = Math.Round((Math.Abs(cellVal) / impuesto) * 100, 2);
                                    string msgBase = string.Format(msg, nuevaBase);
                                    this.gvDocument.SetColumnError(col, msgBase);
                                    validField = false;
                                }
                                else
                                    validField = this.CalcularValorExtra();
                            }
                            else
                                validField = this.CalcularValorExtra();
                            break;
                        case "txtValorME":
                            this.Comprobante.Footer[index].vlrMdaExt.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                            if (impuesto != 0 && this.Comprobante.Footer[index].TerceroID.Value != this.nitDIAN)
                            {
                                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_ValorBaseInvalid);
                                col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

                                decimal impReal = this.Comprobante.Footer[index].vlrBaseME.Value.Value * impuesto / 100;
                                decimal cellVal = this.Comprobante.Footer[index].vlrMdaExt.Value.Value;

                                decimal dif = Math.Abs(impReal - cellVal);
                                decimal difMax = this.Comprobante.Footer[index].vlrBaseME.Value.Value * (Decimal)0.01 / 100;

                                if (dif > difMax)
                                {
                                    decimal nuevaBase = Math.Round((Math.Abs(cellVal) / impuesto) * 100, 2);
                                    string msgBase = string.Format(msg, nuevaBase);
                                    this.gvDocument.SetColumnError(col, msgBase);
                                    validField = false;
                                }
                                else
                                    validField = this.CalcularValorExtra();
                            }
                            else
                                validField = this.CalcularValorExtra();
                            break;

                    }
                    if (!validField)
                        this.isValid = false;

                    this.gvDocument.RefreshData();
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "txtEdit_Leave"));
            }
        }

        /// <summary>
        /// boton para ver los saldos de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSaldos_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_coCuentaSaldo saldoDoc = null;
                DTO_coComprobante comprobante = null;

                if (this._docCtrlDTO == null || string.IsNullOrEmpty(txtDocumento.Text))
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                else
                {
                    //Trae el comprobante actual para obtener saldos
                    comprobante = (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, this.comprobanteID, true);
                    if (comprobante != null)
                        saldoDoc = _bc.AdministrationModel.Saldo_GetByDocumento(this.Cuenta.ID.Value, this.ConceptoSaldo.ID.Value,this._docCtrlDTO.NumeroDoc.Value.Value, string.Empty);

                    if (saldoDoc != null)
                    {
                        BalanceForm frm = new BalanceForm(saldoDoc, this._docCtrlDTO, this.monedaLocal, this.monedaExtranjera);
                        frm.ShowDialog();
                    }
                    else
                        //No tiene saldos el documento
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "btnSaldos_Click"));
            }
        }

        /// <summary>
        /// boton para ver los saldos de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSaldosByCuenta_Click(object sender, EventArgs e)
        {
            {
                try
                {                    
                    if (this.masterCuenta.ValidID && this.masterTercero.ValidID)
                    {
                        ModalSaldosDocument frm = new ModalSaldosDocument(this.dtPeriod.DateTime,this.masterCuenta.Value,this.masterTercero.Value,this.comprobanteID,true);                
                        frm.ShowDialog();
                        if (frm.ListaSaldosSelected.Count > 0)
                        {
                            #region Asigna datos a la fila actual
                            int index = this.NumFila;
                            this.Comprobante.Footer[index].CentroCostoID.Value = frm.ListaSaldosSelected.First().CentroCostoID.Value;
                            this.Comprobante.Footer[index].ConceptoCargoID.Value = frm.ListaSaldosSelected.First().ConceptoCargoID.Value;
                            this.Comprobante.Footer[index].ConceptoSaldoID.Value = frm.ListaSaldosSelected.First().ConceptoSaldoID.Value;
                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = frm.ListaSaldosSelected.First().LineaPresupuestoID.Value;
                            this.Comprobante.Footer[index].LugarGeograficoID.Value = frm.ListaSaldosSelected.First().LugarGeograficoID.Value;
                            this.Comprobante.Footer[index].ProyectoID.Value = frm.ListaSaldosSelected.First().ProyectoID.Value;
                            this.Comprobante.Footer[index].DocumentoCOM.Value = frm.ListaSaldosSelected.First().PrefDoc.Value;
                            this.Comprobante.Footer[index].PrefijoCOM.Value = frm.ListaSaldosSelected.First().PrefijoID.Value;
                            this.Comprobante.Footer[index].vlrBaseML.Value = 0;
                            this.Comprobante.Footer[index].vlrBaseME.Value = 0;
                            this.Comprobante.Footer[index].vlrMdaOtr.Value = 0;
                            this.Comprobante.Footer[index].vlrMdaLoc.Value = frm.ListaSaldosSelected.First().SaldoTotalML.Value;
                            this.Comprobante.Footer[index].vlrMdaExt.Value = frm.ListaSaldosSelected.First().SaldoTotalME.Value;
                            this.Comprobante.Footer[index].PrefDoc = frm.ListaSaldosSelected.First().PrefDoc.Value;
                            this.Comprobante.Footer[index].Descriptivo.Value = "Cancela Saldo Documento";
                            this.Comprobante.Footer[index].IdentificadorTR.Value = frm.ListaSaldosSelected.First().IdentificadorTR.Value;
                            this.Comprobante.Footer[index].TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                            this.Comprobante.Footer[index].DatoAdd1.Value = string.Empty;
                            this.Comprobante.Footer[index].DatoAdd2.Value = string.Empty;
                            bool validField = this.CalcularValorExtra();
                            this.ValidateRow(this.gvDocument.FocusedRowHandle); 
                            #endregion
                            #region Agrega nuevas filas con los saldos seleccionados
                            if (frm.ListaSaldosSelected.Count > 1)
                            {
                                for (int i = 1; i < frm.ListaSaldosSelected.Count; i++)
                                {
                                    DTO_ComprobanteFooter footerDet = new DTO_ComprobanteFooter();
                                    DTO_coCuentaSaldo saldo = frm.ListaSaldosSelected[i];
                                    #region Asigna datos a la nueva fila
                                    {
                                        footerDet.Index = 0;
                                        footerDet.CuentaID.Value = saldo.CuentaID.Value;
                                        footerDet.TerceroID.Value = saldo.TerceroID.Value;
                                        footerDet.ProyectoID.Value = saldo.ProyectoID.Value;
                                        footerDet.CentroCostoID.Value = saldo.CentroCostoID.Value;
                                        footerDet.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                                        footerDet.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                                        footerDet.PrefijoCOM.Value = saldo.PrefijoID.Value;
                                        footerDet.DocumentoCOM.Value = saldo.PrefDoc.Value;
                                        footerDet.ActivoCOM.Value = string.Empty;
                                        footerDet.LugarGeograficoID.Value = saldo.LugarGeograficoID.Value;
                                        footerDet.ConceptoSaldoID.Value = saldo.ConceptoSaldoID.Value;
                                        footerDet.Descriptivo.Value = "Cancela Saldo Documento";
                                        footerDet.IdentificadorTR.Value = saldo.IdentificadorTR.Value;
                                        footerDet.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
                                        footerDet.DatoAdd1.Value = string.Empty;
                                        footerDet.DatoAdd2.Value = string.Empty;
                                        footerDet.PrefDoc = saldo.PrefDoc.Value;
                                        footerDet.vlrBaseML.Value = 0;
                                        footerDet.vlrBaseME.Value = 0;
                                        footerDet.vlrMdaLoc.Value = saldo.SaldoTotalML.Value;
                                        footerDet.vlrMdaExt.Value = saldo.SaldoTotalME.Value;
                                        footerDet.vlrMdaOtr.Value = 0;
                                    }

                                    footerDet.DatoAdd3.Value = string.Empty;
                                    footerDet.DatoAdd4.Value = string.Empty;
                                    #endregion                                   
                                    this.Comprobante.Footer.Add(footerDet);
                                    this.gvDocument.RefreshData();
                                    this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                                    this.isValid = false;
                                    this.EnableFooter(false);
                                    this.masterCuenta.EnableControl(true);
                                    if (this.masterCuenta.ValidID)
                                        this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this.masterCuenta.Value, true);
                                }
                                this.masterCuenta.Focus();
                            } 
                            #endregion
                            #region Asigna datos a los controles
                            this.masterCentroCosto.Value = frm.ListaSaldosSelected.Last().CentroCostoID.Value;
                            this.masterConceptoCargo.Value = frm.ListaSaldosSelected.Last().ConceptoCargoID.Value;
                            this.masterLineaPre.Value = frm.ListaSaldosSelected.Last().LineaPresupuestoID.Value;
                            this.masterLugarGeo.Value = frm.ListaSaldosSelected.Last().LugarGeograficoID.Value;
                            this.masterProyecto.Value = frm.ListaSaldosSelected.Last().ProyectoID.Value;
                            this.txtDocumento.Text = frm.ListaSaldosSelected.Last().PrefDoc.Value;
                            this.masterPrefijo.Value = frm.ListaSaldosSelected.Last().PrefijoID.Value;
                            this.txtBaseML.EditValue = 0;
                            this.txtBaseME.EditValue = 0;
                            this.txtValorML.EditValue = frm.ListaSaldosSelected.Last().SaldoTotalML.Value;
                            this.txtValorME.EditValue = frm.ListaSaldosSelected.Last().SaldoTotalME.Value;

                            if (this.masterProyecto.ValidID)
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                                this.masterProyecto.EnableControl(false);
                            }
                            if (this.masterCentroCosto.ValidID)
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                                this.masterCentroCosto.EnableControl(false);
                            }
                            if (this.masterLineaPre.ValidID)
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                                this.masterLineaPre.EnableControl(false);
                            }
                            if (this.masterLugarGeo.ValidID)
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                                this.masterLugarGeo.EnableControl(false);
                            }
                            if (this.masterConceptoCargo.ValidID)
                            {
                                this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                                this.masterConceptoCargo.EnableControl(false);
                            }
                            #endregion

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "btnSaldos_Click"));
                }
            }
        }

        /// <summary>
        /// Cuando cambia el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkbIVA_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkbIVA.Checked)
                this.Comprobante.Footer[this.NumFila].DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
            else
                this.Comprobante.Footer[this.NumFila].DatoAdd1.Value = string.Empty;

            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Cuando cambia el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTarifas_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Comprobante.Footer[this.NumFila].DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString() && this._refreshGrid)
                {
                    this.Comprobante.Footer[this.NumFila].DatoAdd2.Value = this.cmbTarifa.SelectedItem.ToString();
                    this.gvDocument.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs", "cmbTarifas_SelectedValueChanged"));
            }
        }

        /// <summary>
        /// Carga la cuenta desde un formulario modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCode_DoubleClick(object sender, EventArgs e)
        {
            int index = this.NumFila;
            DTO_ComprobanteFooter compReg = new DTO_ComprobanteFooter();
            //DTO_DigitacionCuentas cuentaDig = new DTO_DigitacionCuentas();
            if (this.Comprobante.Footer.Count > 1)
                compReg = this.Comprobante.Footer[index];
            else 
            {
                compReg.ConceptoCargoID.Value = this.defConceptoCargo;
                compReg.ProyectoID.Value = this.defProyecto;
                compReg.CentroCostoID.Value = this.defCentroCosto;
                compReg.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                compReg.LugarGeograficoID.Value = this.defLugarGeo;
                compReg.CuentaID.Value = string.Empty;
            }

            DigitacionCuentas digitacion = new DigitacionCuentas(compReg, this.biMoneda, (int)this.Comprobante.Header.MdaOrigen.Value.Value);
            digitacion.ShowDialog();

            if (digitacion.ReturnData != null)
            { 
                #region Cuenta
                if (this.Cuenta == null || digitacion.ReturnData.CuentaID.Value != this.Cuenta.ID.Value)
                {
                    this.masterCuenta.Value = digitacion.ReturnData.CuentaID.Value;
                    this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, digitacion.ReturnData.CuentaID.Value, true);
                    this.Comprobante.Footer[index].CuentaID.Value = digitacion.ReturnData.CuentaID.Value;
                    #region Limpia los valores y los controles
                    this.masterCuenta.EnableControl(true);
                    this.txtDescripcion.Enabled = true;
                    #endregion
                    #region Asigna de valores a columnas
                    if (this.saldoControl == SaldoControl.Cuenta)
                    {
                        this.Comprobante.Footer[index].IdentificadorTR.Value = 0;
                        #region Tercero
                        if (!this._cuenta.TerceroInd.Value.Value)
                        {
                            this.masterTercero.Value = this.defTercero;
                            this.Comprobante.Footer[index].TerceroID.Value = this.defTercero;
                        }
                        #endregion
                        #region Prefijo
                        this.masterPrefijo.Value = this.defPrefijo;
                        this.Comprobante.Footer[index].PrefijoCOM.Value = this.defPrefijo;
                        #endregion
                        #region ConceptoCargo
                        if (!this._cuenta.ConceptoCargoInd.Value.Value)
                        {
                            this.masterConceptoCargo.Value = this.defConceptoCargo;
                            this.Comprobante.Footer[index].ConceptoCargoID.Value = this.defConceptoCargo;
                        }
                        #endregion
                        #region Proyecto
                        if (!this._cuenta.ProyectoInd.Value.Value)
                        {
                            this.masterProyecto.Value = this.defConceptoCargo;
                            this.Comprobante.Footer[index].ProyectoID.Value = this.defProyecto;
                        }
                        #endregion
                        #region CentroCosto
                        if (!this._cuenta.CentroCostoInd.Value.Value)
                        {
                            this.masterCentroCosto.Value = this.defCentroCosto;
                            this.Comprobante.Footer[index].CentroCostoID.Value = this.defCentroCosto;
                        }
                        #endregion
                        #region LineaPresupuesto
                        if (!this._cuenta.LineaPresupuestalInd.Value.Value)
                        {
                            this.masterLineaPre.Value = this.defLineaPresupuesto;
                            this.Comprobante.Footer[index].LineaPresupuestoID.Value = this.defLineaPresupuesto;
                        }
                        #endregion
                        #region LugarGeo
                        if (!this._cuenta.LugarGeograficoInd.Value.Value)
                        {
                            this.masterLugarGeo.Value = this.defLugarGeo;
                            this.Comprobante.Footer[index].LugarGeograficoID.Value = this.defLugarGeo;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Limpia los controles
                        this.masterTercero.Value = string.Empty;
                        this.masterPrefijo.Value = string.Empty;
                        this.masterConceptoCargo.Value = string.Empty;
                        this.masterProyecto.Value = string.Empty;
                        this.masterCentroCosto.Value = string.Empty;
                        this.masterLineaPre.Value = string.Empty;
                        this.masterLugarGeo.Value = string.Empty;
                        this.txtDocumento.Text = string.Empty;
                        this.txtActivo.Text = string.Empty;

                        this.Comprobante.Footer[index].TerceroID.Value = string.Empty;
                        this.Comprobante.Footer[index].PrefijoCOM.Value = string.Empty;
                        this.Comprobante.Footer[index].ConceptoCargoID.Value = string.Empty;
                        this.Comprobante.Footer[index].ProyectoID.Value = string.Empty;
                        this.Comprobante.Footer[index].CentroCostoID.Value = string.Empty;
                        this.Comprobante.Footer[index].LineaPresupuestoID.Value = string.Empty;
                        this.Comprobante.Footer[index].LugarGeograficoID.Value = string.Empty;
                        this.Comprobante.Footer[index].DocumentoCOM.Value = string.Empty;
                        this.Comprobante.Footer[index].ActivoCOM.Value = string.Empty;
                        #endregion
                        this.Comprobante.Footer[index].DatoAdd1.Value = string.Empty;
                    }
                    #endregion
                }
                #endregion

                this.Comprobante.Footer[index].CentroCostoID.Value = digitacion.ReturnData.CentroCostoID.Value;
                this.Comprobante.Footer[index].ConceptoCargoID.Value = digitacion.ReturnData.ConceptoCargoID.Value;
                this.Comprobante.Footer[index].ConceptoSaldoID.Value = digitacion.ReturnData.ConceptoSaldoID.Value;
                this.Comprobante.Footer[index].LineaPresupuestoID.Value = digitacion.ReturnData.LineaPresupuestoID.Value;
                this.Comprobante.Footer[index].LugarGeograficoID.Value = digitacion.ReturnData.LugarGeograficoID.Value;
                this.Comprobante.Footer[index].ProyectoID.Value = digitacion.ReturnData.ProyectoID.Value;

                this.Comprobante.Footer[index].vlrBaseML.Value = digitacion.ReturnData.vlrBaseML.Value;
                this.Comprobante.Footer[index].vlrBaseME.Value = digitacion.ReturnData.vlrBaseME.Value;
                this.Comprobante.Footer[index].vlrMdaLoc.Value = digitacion.ReturnData.vlrMdaLoc.Value;
                this.Comprobante.Footer[index].vlrMdaExt.Value = digitacion.ReturnData.vlrMdaExt.Value;

                this.masterCentroCosto.Value = digitacion.ReturnData.CentroCostoID.Value;
                this.masterConceptoCargo.Value = digitacion.ReturnData.ConceptoCargoID.Value;
                this.masterLineaPre.Value = digitacion.ReturnData.LineaPresupuestoID.Value;
                this.masterLugarGeo.Value = digitacion.ReturnData.LugarGeograficoID.Value;
                this.masterProyecto.Value = digitacion.ReturnData.ProyectoID.Value;

                this.txtBaseML.EditValue = digitacion.ReturnData.vlrBaseML.Value;
                this.txtBaseME.EditValue = digitacion.ReturnData.vlrBaseME.Value;
                this.txtValorML.EditValue = digitacion.ReturnData.vlrMdaLoc.Value;
                this.txtValorME.EditValue = digitacion.ReturnData.vlrMdaExt.Value;

                this.Comprobante.Footer[index].PrefDoc = this.Comprobante.Footer[index].PrefijoCOM.Value + " - " + this.Comprobante.Footer[index].DocumentoCOM.Value;

                bool validField = this.CalcularValorExtra();

                this.ValidateRow(this.gvDocument.FocusedRowHandle);
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

                    this.masterCuenta.Value = string.Empty;
                    this.masterTercero.Value = string.Empty;
                    this.masterPrefijo.Value = string.Empty;
                    this.masterConceptoCargo.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;
                    this.masterLineaPre.Value = string.Empty;
                    this.masterLugarGeo.Value = string.Empty;
                    this.txtDocumento.Text = string.Empty;
                    this.txtActivo.Text = string.Empty;
                    this.txtDescripcion.Text = string.Empty;
                    this.EnableFooter(false);
                    this.newDoc = false;
                    this.btnSaldosByCuenta.Enabled = false;
                    this.Cuenta = null;
                    this.Tercero = null;
                    this.CleanHeader(true);
                    this.EnableHeader(true);
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            this.gvDocument.ActiveFilterString = string.Empty;
        }

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            this.GenerateReport(true);
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilterDef()
        {
            bool validRow = true;
            if (this.gvDocument.RowCount > 0)
                validRow = this.ValidateRow(this.gvDocument.FocusedRowHandle);

            if (validRow)
            {
                this.gvDocument.ActiveFilterString = string.Empty;

                if (this.gvDocument.RowCount > 0)
                    this.gvDocument.MoveFirst();
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
                if (this.Comprobante.Footer.Count() > 0 && this.ValidateRow(this.gvDocument.FocusedRowHandle))
                {
                    MasterQuery mq = new MasterQuery(this, this.documentID, _bc.AdministrationModel.User.ReplicaID.Value.Value, false, typeof(DTO_ComprobanteFooter), typeof(Filtrable));
                    #region definir Fks
                    mq.SetFK("CuentaID", AppMasters.coPlanCuenta, _bc.CreateFKConfig(AppMasters.coPlanCuenta));
                    mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                    mq.SetFK("PrefijoID", AppMasters.glPrefijo, _bc.CreateFKConfig(AppMasters.glPrefijo));
                    mq.SetFK("ProyectoID", AppMasters.coProyecto, _bc.CreateFKConfig(AppMasters.coProyecto));
                    mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
                    mq.SetFK("LineaPresupuestoID", AppMasters.plLineaPresupuesto, _bc.CreateFKConfig(AppMasters.plLineaPresupuesto));
                    mq.SetFK("ConceptoCargoID", AppMasters.coConceptoCargo, _bc.CreateFKConfig(AppMasters.coConceptoCargo));
                    mq.SetFK("LugarGeograficoID", AppMasters.glLugarGeografico, _bc.CreateFKConfig(AppMasters.glLugarGeografico));
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
                    _bc.AdministrationModel.DataCopied = this.Comprobante.Footer;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                this.Comprobante.Footer = compDet;
                this.LoadData(true);
                this.UpdateTemp(this.data);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para revertir un comprobante
        /// </summary>
        public override void TBRevert()
        {
            try
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_RevertComp);

                if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                    {
                        det.vlrMdaLoc.Value = det.vlrMdaLoc.Value.Value * -1;
                        det.vlrMdaExt.Value = det.vlrMdaExt.Value.Value * -1;
                    }

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.LoadData(true);
                    this.UpdateTemp(this.data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            //Revisa que cumple las condiciones
            if (!this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;

            bool hasItems = this.Comprobante.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_ComprobanteFooter> list = new List<DTO_ComprobanteFooter>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, DTO_coTercero> terceros = new Dictionary<string, DTO_coTercero>();
                    DTO_coTercero ter = null;
                    Dictionary<string, Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>> ctas = new Dictionary<string, Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
                    string msgVals = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_ValorBaseInvalid);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgCtaCargoProy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                    string msgCtaPeriodClosed = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CtaPeriodClosed);
                    string msgInvalidDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument);
                    //Popiedades de un comprobante
                    DTO_ComprobanteFooter det = new DTO_ComprobanteFooter();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_ComprobanteFooter).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    //Fks
                    fks.Add(_cuentaRsx, new List<Tuple<string, bool>>());
                    fks.Add(_terceroRsx, new List<Tuple<string, bool>>());
                    fks.Add(_prefijoRsx, new List<Tuple<string, bool>>());
                    fks.Add(_proyectoRsx, new List<Tuple<string, bool>>());
                    fks.Add(_centroCostoRsx, new List<Tuple<string, bool>>());
                    fks.Add(_lineaPresupRsx, new List<Tuple<string, bool>>());
                    fks.Add(_conceptoCargoRsx, new List<Tuple<string, bool>>());
                    fks.Add(_lugarGeoRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        //Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];

                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        if (colRsx == _cuentaRsx ||
                                            colRsx == _terceroRsx ||
                                            colRsx == _prefijoRsx ||
                                            colRsx == _proyectoRsx ||
                                            colRsx == _centroCostoRsx ||
                                            colRsx == _lineaPresupRsx ||
                                            colRsx == _conceptoCargoRsx ||
                                            colRsx == _lugarGeoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                            {
                                                continue;
                                            }
                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                    #region Asigna los valores de las cuentas
                                                    if (colRsx == _cuentaRsx)
                                                    {
                                                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)dto;
                                                        DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cta.ConceptoSaldoID.Value, true);
                                                        Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo> tup = new Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>(cta, cSaldo);
                                                        ctas.Add(line[colIndex].Trim(), tup);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        }
                                    }
                                    #endregion Revisa la info de las FKs
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                det = new DTO_ComprobanteFooter();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                                (colRsx == _cuentaRsx ||
                                                colName == "Descriptivo" ||
                                                colName == "TasaCambio" ||
                                                colName == "vlrBaseML" ||
                                                colName == "vlrBaseME" ||
                                                colName == "vlrMdaLoc" ||
                                                colName == "vlrMdaExt")
                                        )
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        } //validacion si no es null
                                        #endregion

                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DocumentAuxiliarForm.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la infoirmación de los resultados
                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                                list.Add(det);
                            else
                                validList = false;
                            #endregion
                        }
                    }
                    #endregion
                    #region Valida las restricciones particulares del comprobante
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;
                        foreach (DTO_ComprobanteFooter dto in list)
                        {
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (list.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }

                            dto.Index = i;
                            i++;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            DTO_coPlanCuenta cta = ctas[dto.CuentaID.Value].Item1;
                            DTO_glConceptoSaldo cSaldo = ctas[dto.CuentaID.Value].Item2;
                            dto.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;

                            #region Validaciones de valores al importar
                            this.ValidateValoresImport(dto, cta, rd, msgCero, msgVals);
                            #endregion
                            #region Validacion Cuenta periodo abierto

                            ModulesPrefix cMod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), cSaldo.ModuloID.Value.ToLower());
                            if (cSaldo.coSaldoControl.Value != (int)SaldoControl.Cuenta)
                            {
                                if (cMod != ModulesPrefix.apl && cMod != ModulesPrefix.gl && cMod != ModulesPrefix.co)
                                {
                                    string currentP = this.dtPeriod.DateTime.ToString(FormatString.ControlDate);
                                    string perAbierto = _bc.GetControlValueByCompany(cMod, AppControl.co_Periodo);

                                    if (perAbierto == string.Empty || currentP != perAbierto)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _cuentaRsx;
                                        rdF.Message = msgCtaPeriodClosed;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }

                            #endregion
                            #region Validacion segun el control de saldos
                            dto.DatoAdd1.Value = string.Empty;
                            switch (cSaldo.coSaldoControl.Value)
                            {
                                case (int)SaldoControl.Cuenta:
                                    #region Por Cuenta
                                    #region Tercero
                                    if (!cta.TerceroInd.Value.Value)
                                    {
                                        dto.TerceroID.Value = this.defTercero;
                                    }
                                    else if (string.IsNullOrWhiteSpace(dto.TerceroID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _terceroRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Proyecto
                                    if (!cta.ProyectoInd.Value.Value)
                                    {
                                        dto.ProyectoID.Value = this.defProyecto;
                                    }
                                    else if (string.IsNullOrWhiteSpace(dto.ProyectoID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _proyectoRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Centro Costo
                                    if (!cta.CentroCostoInd.Value.Value)
                                    {
                                        dto.CentroCostoID.Value = this.defCentroCosto;
                                    }
                                    else if (string.IsNullOrWhiteSpace(dto.CentroCostoID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _centroCostoRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Linea presupuesto
                                    if (!cta.LineaPresupuestalInd.Value.Value)
                                    {
                                        dto.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                    }
                                    else if (string.IsNullOrWhiteSpace(dto.LineaPresupuestoID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _lineaPresupRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Concepto Cargo
                                    if (!cta.ConceptoCargoInd.Value.Value)
                                        dto.ConceptoCargoID.Value = this.defConceptoCargo;
                                    else if (cta.ConceptoCargoID != null && !string.IsNullOrWhiteSpace(cta.ConceptoCargoID.Value))
                                        dto.ConceptoCargoID.Value = cta.ConceptoCargoID.Value;

                                    //Hace la validacion con la tabla de costos de cargos
                                    if (cta.ConceptoCargoInd.Value.Value && this._validCtaProyCC)
                                    {
                                        if (cta.ProyectoInd.Value.Value)
                                        {
                                            string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(dto.ConceptoCargoID.Value, dto.ProyectoID.Value, string.Empty, linPresID).Trim();
                                            if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != cta.ID.Value)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _proyectoRsx + " - " + _conceptoCargoRsx;
                                                rdF.Message = msgCtaCargoProy;
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        else if (cta.CentroCostoInd.Value.Value)
                                        {
                                            string linPresID = this.Comprobante.Footer[this.NumFila].LineaPresupuestoID.Value;
                                            string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(dto.ConceptoCargoID.Value, string.Empty, dto.ProyectoID.Value, linPresID).Trim();
                                            if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != cta.ID.Value)
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _proyectoRsx + " - " + _conceptoCargoRsx;
                                                rdF.Message = msgCtaCargoProy;
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                    }

                                    #endregion
                                    #region Lugar Geografico
                                    if (!cta.LugarGeograficoInd.Value.Value)
                                    {
                                        dto.LugarGeograficoID.Value = this.defLugarGeo;
                                    }
                                    else if (string.IsNullOrWhiteSpace(dto.LugarGeograficoID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _lugarGeoRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Prefijo
                                    dto.PrefijoCOM.Value = this.defPrefijo;
                                    #endregion
                                    #region Documento
                                    if (string.IsNullOrWhiteSpace(dto.DocumentoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    else
                                        dto.PrefDoc = dto.PrefijoCOM.Value + " - " + dto.DocumentoCOM.Value;
                                    #endregion
                                    #region Validacion de cuenta de IVA
                                    if (cta.ImpuestoTipoID.Value == this.tipoImpuestoIVA)
                                    {
                                        dto.DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                                        dto.DatoAdd2.Value = cta.ImpuestoPorc.Value.ToString();
                                    }
                                    #endregion
                                    dto.IdentificadorTR.Value = 0;
                                    #endregion
                                    break;
                                case (int)SaldoControl.Doc_Interno:
                                    #region Documento Interno
                                    dto.ConceptoCargoID.Value = this.defConceptoCargo;
                                    dto.LugarGeograficoID.Value = this.defLugarGeo;

                                    #region Revisa que haya documento
                                    if (string.IsNullOrWhiteSpace(dto.DocumentoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que tenga prefijo
                                    if (string.IsNullOrWhiteSpace(dto.PrefijoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _prefijoRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que el documento sea numerico
                                    if (createDTO)
                                    {
                                        try
                                        {
                                            int dInt = Convert.ToInt32(dto.DocumentoCOM.Value);
                                        }
                                        catch (Exception)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NumericDocInt);
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                    }
                                    #endregion
                                    #region Trae la info del documento
                                    if (createDTO)
                                    {
                                        int docIntId = Convert.ToInt32(dto.DocumentoCOM.Value);
                                        DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetInternalDocByCta(dto.CuentaID.Value, dto.PrefijoCOM.Value, docIntId);

                                        if (docCtrl == null)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgInvalidDoc;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            dto.TerceroID.Value = docCtrl.TerceroID.Value;
                                            dto.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                            dto.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                            dto.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                            dto.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                case (int)SaldoControl.Doc_Externo:
                                    #region Documento externo
                                    dto.ConceptoCargoID.Value = this.defConceptoCargo;
                                    dto.LugarGeograficoID.Value = this.defLugarGeo;

                                    #region Revisa que haya documento
                                    if (string.IsNullOrWhiteSpace(dto.DocumentoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que tenga tercero
                                    if (string.IsNullOrWhiteSpace(dto.TerceroID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _terceroRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Trae la info del documento
                                    if (createDTO)
                                    {
                                        DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(dto.CuentaID.Value, dto.TerceroID.Value, dto.DocumentoCOM.Value);

                                        if (docCtrl == null)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgInvalidDoc;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            dto.TerceroID.Value = docCtrl.TerceroID.Value;
                                            dto.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                            dto.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                            dto.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                            dto.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                        }

                                    }
                                    #endregion
                                    #endregion
                                    break;
                                case (int)SaldoControl.Componente_Tercero:
                                    #region Componente Tercero
                                    dto.IdentificadorTR.Value = Convert.ToInt32(dto.TerceroID.Value);
                                    #endregion
                                    break;
                                case (int)SaldoControl.Componente_Activo:
                                    #region Activo
                                    dto.ConceptoCargoID.Value = this.defConceptoCargo;
                                    dto.LugarGeograficoID.Value = this.defLugarGeo;

                                    #region Revisa que tenga activo
                                    if (string.IsNullOrWhiteSpace(dto.ActivoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Trae la info del activo
                                    if (createDTO)
                                    {
                                        DTO_acActivoControl acCtrl = _bc.AdministrationModel.acActivoControl_GetByPlaqueta(dto.ActivoCOM.Value);
                                        if (acCtrl == null)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
                                            rdF.Message = msgInvalidDoc;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            dto.TerceroID.Value = acCtrl.TerceroID.Value;
                                            dto.PrefijoCOM.Value = defPrefijo;
                                            dto.DocumentoCOM.Value = dto.ActivoCOM.Value;
                                            dto.ProyectoID.Value = acCtrl.ProyectoID.Value;
                                            dto.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                                            dto.LineaPresupuestoID.Value = defLineaPresupuesto;
                                            dto.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                case (int)SaldoControl.Componente_Documento:
                                    #region Componente Documento
                                    dto.ConceptoCargoID.Value = this.defConceptoCargo;
                                    dto.LugarGeograficoID.Value = this.defLugarGeo;

                                    #region Revisa que haya documento
                                    if (string.IsNullOrWhiteSpace(dto.DocumentoCOM.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que tenga tercero
                                    if (string.IsNullOrWhiteSpace(dto.TerceroID.Value))
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _terceroRsx;
                                        rdF.Message = msgEmptyField;
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Trae la info del documento
                                    if (createDTO)
                                    {
                                        DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(dto.CuentaID.Value, dto.TerceroID.Value, dto.DocumentoCOM.Value);

                                        if (docCtrl == null)
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgInvalidDoc;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            dto.TerceroID.Value = docCtrl.TerceroID.Value;
                                            dto.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                            dto.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                            dto.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                            dto.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                        }

                                    }
                                    #endregion
                                    #endregion
                                    break;
                            }
                            #endregion
                            #region Validacion de tercero responsable
                            if(terceros.ContainsKey(dto.TerceroID.Value))
                                ter = terceros[dto.TerceroID.Value];
                            else
                            {
                                ter = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, dto.TerceroID.Value, true, null);
                                terceros.Add(dto.TerceroID.Value, ter);
                            }
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = ResultValue.NOK.ToString();
                            }
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.Comprobante.Footer = list;
                            this.UpdateTemp(this.data);
                            this.Invoke(this.refreshGridDelegate);
                        }

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp-DocumentAuxiliarForm.cs", "ImportThread"));
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
