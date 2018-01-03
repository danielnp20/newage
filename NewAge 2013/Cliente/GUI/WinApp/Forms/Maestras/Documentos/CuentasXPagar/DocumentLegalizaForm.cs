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
using DevExpress.XtraEditors;
using System.Globalization;
using DevExpress.XtraReports.UI;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentLegalizaForm : DocumentForm
    {
        //public DocumentLegalizaForm()
        //{
        //    InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region variables privadas

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private string _regFiscalTercero;
        private string _conceptoCargo = string.Empty;
        private string _conceptoCargoIVA2 = string.Empty;
        private string _operacion;
        private bool _declaraIVA;
        private DTO_coRegimenFiscal dtoRegFiscalTer = new DTO_coRegimenFiscal();
        private DTO_cpLegalizaFooter footerCurrent = new DTO_cpLegalizaFooter();
        #endregion

        #region Variables Protected

        //Variables formulario
        protected DTO_Legalizacion data = null;
        //Variables Moneda
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        protected TipoMoneda _tipoMoneda;
        protected decimal saldoActualDocExt = 0;
        protected TipoMoneda_LocExt _tipoMonedaOr;
        //Indica si el header es valido
        protected bool validHeader;
        protected bool _canSendApprove = false;
        //variables para funciones particulares
        protected bool cleanDoc = true;
        protected bool isDocExterno = false;
        //Variables por defecto
        protected string defLugarGeo = string.Empty;
        protected DTO_coTercero _regFiscalEmp = new DTO_coTercero();

        #endregion

        #region Propiedades
       
        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this.data.Footer.FindIndex(det => det.Index == this.indexFila);
            }
        }

        //Cargo Especial
        private DTO_cpCargoEspecial _cargo = null;
        private DTO_coPlanCuenta _ctaIVA1 = null;
        private DTO_coPlanCuenta _ctaIVA2 = null;
        private List<DTO_MasterComplex> _listImpuestos = null;

        protected DTO_cpCargoEspecial Cargo
        {
            get
            {
                return this._cargo;
            }
            set
            {
                this._cargo = value;
                int index = this.NumFila;
                
                if (value == null)
                {
                    #region Si el cargo no existe
                    this.data.Footer[index].CargoEspecialID.Value = string.Empty;
                    this.masterCargoEspecial.Value = string.Empty;

                   // this.EnableFooter(false);
                    this.masterCargoEspecial.EnableControl(true);
                    #endregion
                }
                else
                {
                    #region Si el cargo Si Existe
                    this.EnableFooter(true);
                    //this._cta1 = null;// (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, value.CuentaIVA1.Value, true);
                    //this._cta2 = null;// (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, value.CuentaIVA2.Value, true);
                    this._conceptoCargo = value.ConceptoCargoID.Value;
                    this._conceptoCargoIVA2 = value.ConceptoCargo1.Value; 
                    //if (this._cta1 != null)
                    //   this._cta1.ImpuestoPorc.Value = string.IsNullOrEmpty(this._cta1.ImpuestoPorc.Value.ToString()) ? 0 : this._cta1.ImpuestoPorc.Value.Value;
                    //if (this._cta2 != null)
                    //    this._cta2.ImpuestoPorc.Value = string.IsNullOrEmpty(this._cta2.ImpuestoPorc.Value.ToString()) ? 0 : this._cta2.ImpuestoPorc.Value.Value;
                    //this.data.Footer[index].PorIVA1.Value = this._cta1 == null ? 0 : this._cta1.ImpuestoPorc.Value.Value;
                    //this.data.Footer[index].PorIVA2.Value = this._cta2 != null ? 0 : this._cta2.ImpuestoPorc.Value.Value;

                    #endregion
                }
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
                    this.data.Footer[index].TerceroID.Value = this.masterTercero.Value;

                    this.txtNombre.Enabled = true;
                    this.chkNuevo.Checked = true;
                    #endregion
                }
                else
                {
                    #region Si el tercero Si existe

                    this.data.Footer[index].Nombre.Value = this._tercero.Descriptivo.Value;
                    this.data.Footer[index].NuevoTerceroInd.Value = false;

                    #endregion
                }
                this.gvDocument.RefreshData();
            }
        }

        #endregion

        #region Funciones Privadas y protected

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        private ComprobanteReport GenerateReport(bool show, bool allFields=false)
        {
            switch (this.documentID)
            {
                #region CajaMenor Report
                case AppDocuments.CajaMenor:
                    DTO_glDocumentoControl ctrl = this.data.DocCtrl;
                    DTO_cpLegalizaDocu legaDoc = this.data.Header;
                    List<DTO_cpLegalizaFooter> legaDet = this.data.Footer;
                    if (ctrl != null && legaDoc != null && legaDet != null && legaDet.Count != 0)
                    {
                        #region Obtener datos para el reporte
                        DTO_ReportCajaMenor reportCaja = new DTO_ReportCajaMenor();
                        DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = ctrl.TerceroID.Value }, true);
                        reportCaja.Responsable = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                        reportCaja.FechaIni = legaDoc.FechaIni.Value.Value;
                        reportCaja.FechaFin = legaDoc.FechaFin.Value.Value;
                        reportCaja.CajaMenorNro = ctrl.DocumentoNro.Value.Value.ToString();
                        reportCaja.FechaCont = legaDoc.FechaCont.Value.Value;
                        reportCaja.ValorCajaMenor = legaDoc.ValorFondo.Value.Value;
                        reportCaja.Factura = ctrl.DocumentoNro.Value.Value.ToString();
                        reportCaja.UsuarioElab = ctrl.seUsuarioID.Value.Value.ToString();
                        reportCaja.UsuarioSol = legaDoc.UsuarioSolicita.Value;
                        reportCaja.UsuarioRev = legaDoc.UsuarioRevisa.Value;
                        reportCaja.UsuarioSV = legaDoc.UsuarioSupervisa.Value;
                        reportCaja.UsuarioApr = legaDoc.UsuarioAprueba.Value;
                        reportCaja.UsuarioCont = legaDoc.UsuarioAprueba.Value;
                        EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), ctrl.Estado.Value.Value.ToString());
                        reportCaja.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)? false:true;
                    
                        reportCaja.CajaMenorDetail = new List<DTO_CajaMenorDetail>();

                        foreach (DTO_cpLegalizaFooter footer in legaDet)
                        {
                            DTO_CajaMenorDetail reportCajaDet = new DTO_CajaMenorDetail();

                            reportCajaDet.CargoEspID = footer.CargoEspecialID.Value;
                            DTO_cpCargoEspecial ceInfo = (DTO_cpCargoEspecial)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.cpCargoEspecial, new UDT_BasicID() { Value = footer.CargoEspecialID.Value }, true);
                            reportCajaDet.CargoEspDesc = ((DTO_cpCargoEspecial)ceInfo).Descriptivo.Value;
                            reportCajaDet.Fecha = footer.Fecha.Value.Value;
                            reportCajaDet.Documento = footer.Factura.Value;
                            reportCajaDet.TerceroID = footer.TerceroID.Value;
                            terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = footer.TerceroID.Value }, true);
                            reportCajaDet.TerceroDesc = ((DTO_coTercero)terceroInfo).Descriptivo.Value;
                            DTO_coCentroCosto ccInfo = (DTO_coCentroCosto)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coCentroCosto, new UDT_BasicID() { Value = footer.CentroCostoID.Value }, true);
                            reportCajaDet.CentroCostoDesc = ((DTO_coCentroCosto)ccInfo).Descriptivo.Value;
                            DTO_coProyecto proyectoInfo = (DTO_coProyecto)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coProyecto, new UDT_BasicID() { Value = footer.ProyectoID.Value }, true);
                            reportCajaDet.ProyectoDesc = ((DTO_coProyecto)proyectoInfo).Descriptivo.Value;
                            reportCajaDet.FacturaDesc = footer.Descriptivo.Value;
                            reportCajaDet.ValorBruto = footer.ValorBruto.Value.Value;
                            reportCajaDet.ValorIva = footer.ValorIVA1.Value.Value + footer.ValorIVA2.Value.Value;
                            reportCajaDet.ValorRteF = footer.ValorRteFuente.Value.Value;
                            reportCajaDet.ValorRteIVA = footer.ValorRteIVA1.Value.Value;
                            reportCajaDet.ValorRteICA = footer.ValorRteICA.Value.Value;

                            reportCaja.CajaMenorDetail.Add(reportCajaDet);
                        }

                        reportCaja.ValorSoportes = reportCaja.CajaMenorDetail.Sum(x => x.ValorBruto) + reportCaja.CajaMenorDetail.Sum(x => x.ValorIva)
                            - reportCaja.CajaMenorDetail.Sum(x => x.ValorRteF) - reportCaja.CajaMenorDetail.Sum(x => x.ValorRteIVA) - reportCaja.CajaMenorDetail.Sum(x => x.ValorRteICA);
                        reportCaja.ValorDisponible = reportCaja.ValorCajaMenor - reportCaja.ValorSoportes;
                        reportCaja.RegNro = reportCaja.CajaMenorDetail.Count;
                        #endregion
                        CajaMenorReport report = new CajaMenorReport(AppReports.cpCajaMenor, new List<DTO_ReportCajaMenor> { reportCaja }, ColumnsInfo.CajaMenorFields, reportCaja.EstadoInd, _bc);
                        report.ShowPreview();
                    }
                    return null;
                #endregion
                #region LegalizacionGastos Report
                case AppDocuments.LegalizacionGastos:
                    try
                    {
                        #region Variables
                        DTO_cpCargoEspecial _cargoEsp;
                        TipoCargo _tipocargo;
                        Dictionary<string, DTO_cpCargoEspecial> cacheCargoEsp = new Dictionary<string, DTO_cpCargoEspecial>();
                        Dictionary<TipoCargo, Tuple<string, string, string, string>> det = new Dictionary<TipoCargo,Tuple<string,string,string,string>>();
                
                        DTO_glDocumentoControl legaDocCtrl = this.data.DocCtrl;
                        DTO_cpLegalizaDocu legaHead = this.data.Header;
                        List<DTO_cpLegalizaFooter> legaFoot = this.data.Footer;

                        EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), legaDocCtrl.Estado.Value.Value.ToString());
                        bool isPre = estado == EstadoDocControl.ParaAprobacion || estado == EstadoDocControl.SinAprobar || estado == EstadoDocControl.Radicado ? true : false;
                       #endregion
                        if (legaDocCtrl != null && legaHead != null && legaFoot != null && legaFoot.Count != 0)
                        {
                            #region Asignar los datos para el reporte
                            #region Header
                            DTO_ReportLegalizacionGastos reportLega = new DTO_ReportLegalizacionGastos();
                            reportLega.Header.NumeroDoc = legaDocCtrl.NumeroDoc.Value.Value;
                            reportLega.Header.Prefijo = legaDocCtrl.PrefijoID.Value;
                            reportLega.Header.DocumentoNro = legaDocCtrl.DocumentoNro.Value.Value;
                            reportLega.Header.DocumentoDesc = legaDocCtrl.Observacion.Value;
                            reportLega.Header.Fecha = legaDocCtrl.Fecha.Value.Value;
                            reportLega.Header.MonedaID = legaDocCtrl.MonedaID.Value;
                            reportLega.Header.TerceroID = legaDocCtrl.TerceroID.Value;
                            DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = legaDocCtrl.TerceroID.Value }, true);
                            reportLega.Header.TerceroDesc = terceroInfo.Descriptivo.Value;
                            reportLega.Header.LugarGeograficoID = terceroInfo.LugarGeograficoID.Value;
                            DTO_glLugarGeografico lugarInfo = (DTO_glLugarGeografico)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glLugarGeografico, new UDT_BasicID() { Value = reportLega.Header.LugarGeograficoID }, true);
                            reportLega.Header.LugarGeograficoDesc = lugarInfo.Descriptivo.Value;
                            reportLega.Header.EstadoInd = isPre;
                            #endregion
                            #region Detail
                            foreach (DTO_cpLegalizaFooter footer in legaFoot)
                            {
                                #region Carga el Cargo Especial
                                if (cacheCargoEsp.ContainsKey(footer.CargoEspecialID.Value))
                                {
                                    _cargoEsp = cacheCargoEsp[footer.CargoEspecialID.Value];
                                }
                                else
                                {
                                    _cargoEsp = (DTO_cpCargoEspecial)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.cpCargoEspecial, new UDT_BasicID() { Value = footer.CargoEspecialID.Value }, true);
                                    cacheCargoEsp.Add(footer.CargoEspecialID.Value, _cargoEsp);
                                }
                                _tipocargo = (TipoCargo)Enum.Parse(typeof(TipoCargo), _cargoEsp.CargoTipo.Value.Value.ToString());

                                if (!det.ContainsKey(_tipocargo))
                                {
                                    Tuple<string, string, string, string> detData = new Tuple<string, string, string, string>("?", footer.ProyectoID.Value, footer.CentroCostoID.Value, footer.LugarGeograficoID.Value);
                                    det.Add(_tipocargo, detData);
                                }
                                #endregion

                                DTO_ReportLegaFooter reportLegaFooter = new DTO_ReportLegaFooter();
                                reportLegaFooter.Fecha = footer.Fecha.Value.Value;
                                reportLegaFooter.Observacion = footer.Descriptivo.Value;

                                reportLegaFooter.ValorAlojamiento = (_tipocargo == TipoCargo.Alojamiento) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorAlimentacion = (_tipocargo == TipoCargo.Alimentacion) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorTranspAer = (_tipocargo == TipoCargo.TransporteAereo) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorTranspTer = (_tipocargo == TipoCargo.TransporteTerrestre) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorViaticos = (_tipocargo == TipoCargo.Viaticos) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorOtros = (_tipocargo == TipoCargo.Otros) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorImpuestos = (_tipocargo == TipoCargo.Impuestos) ? footer.ValorNeto.Value.Value : 0;
                                reportLegaFooter.ValorTotal = reportLegaFooter.ValorAlojamiento + reportLegaFooter.ValorAlimentacion + reportLegaFooter.ValorTranspAer + reportLegaFooter.ValorTranspTer + reportLegaFooter.ValorImpuestos + reportLegaFooter.ValorViaticos + reportLegaFooter.ValorOtros;

                                reportLega.Footer.Add(reportLegaFooter);
                            }
                            #endregion
                            #region Summary
                            DTO_ReportLegaDetail reportLegaDet;

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.Alojamiento);
                            reportLegaDet.TipoCargoDesc = TipoCargo.Alojamiento.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorAlojamiento);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.Alojamiento) ? det[TipoCargo.Alojamiento].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.Alojamiento) ? det[TipoCargo.Alojamiento].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.Alojamiento) ? det[TipoCargo.Alojamiento].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.Alojamiento) ? det[TipoCargo.Alojamiento].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.Alimentacion);
                            reportLegaDet.TipoCargoDesc = TipoCargo.Alimentacion.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorAlimentacion);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.Alimentacion) ? det[TipoCargo.Alimentacion].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.Alimentacion) ? det[TipoCargo.Alimentacion].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.Alimentacion) ? det[TipoCargo.Alimentacion].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.Alimentacion) ? det[TipoCargo.Alimentacion].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.TransporteAereo);
                            reportLegaDet.TipoCargoDesc = TipoCargo.TransporteAereo.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorTranspAer);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.TransporteAereo) ? det[TipoCargo.TransporteAereo].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.TransporteAereo) ? det[TipoCargo.TransporteAereo].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.TransporteAereo) ? det[TipoCargo.TransporteAereo].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.TransporteAereo) ? det[TipoCargo.TransporteAereo].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.TransporteTerrestre);
                            reportLegaDet.TipoCargoDesc = TipoCargo.TransporteTerrestre.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorTranspTer);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.TransporteTerrestre) ? det[TipoCargo.TransporteTerrestre].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.TransporteTerrestre) ? det[TipoCargo.TransporteTerrestre].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.TransporteTerrestre) ? det[TipoCargo.TransporteTerrestre].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.TransporteTerrestre) ? det[TipoCargo.TransporteTerrestre].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.Viaticos);
                            reportLegaDet.TipoCargoDesc = TipoCargo.Viaticos.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorViaticos);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.Viaticos) ? det[TipoCargo.Viaticos].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.Viaticos) ? det[TipoCargo.Viaticos].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.Viaticos) ? det[TipoCargo.Viaticos].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.Viaticos) ? det[TipoCargo.Viaticos].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.Otros);
                            reportLegaDet.TipoCargoDesc = TipoCargo.Otros.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorOtros);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.Otros) ? det[TipoCargo.Otros].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.Otros) ? det[TipoCargo.Otros].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.Otros) ? det[TipoCargo.Otros].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.Otros) ? det[TipoCargo.Otros].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);

                            reportLegaDet = new DTO_ReportLegaDetail();
                            reportLegaDet.TipoCargoID = Convert.ToInt16(TipoCargo.Impuestos);
                            reportLegaDet.TipoCargoDesc = TipoCargo.Impuestos.ToString();
                            reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorImpuestos);
                            reportLegaDet.CuentaID = det.ContainsKey(TipoCargo.Impuestos) ? det[TipoCargo.Impuestos].Item1 : " - ";
                            reportLegaDet.ProyectoID = det.ContainsKey(TipoCargo.Impuestos) ? det[TipoCargo.Impuestos].Item2 : " - ";
                            reportLegaDet.CentroCostoID = det.ContainsKey(TipoCargo.Impuestos) ? det[TipoCargo.Impuestos].Item3 : " - ";
                            reportLegaDet.LugarGeoID = det.ContainsKey(TipoCargo.Impuestos) ? det[TipoCargo.Impuestos].Item4 : " - ";
                            reportLega.Detail.Add(reportLegaDet);
                            #endregion
                            #endregion       
                            LegalizacionGastosReport report = new LegalizacionGastosReport(AppReports.cpLegalizacionGastos, new List<DTO_ReportLegalizacionGastos> { reportLega }, ColumnsInfo.LegaGastosFooterFields, ColumnsInfo.LegaGastosDetailFields, reportLega.Header.EstadoInd, _bc);
                            report.ShowPreview();
                        }
                    }
                    catch (Exception ex) { throw ex; }
                    return null;
                #endregion
                default:
                    return null;
            };
        }

        /// <summary>
        /// Habilita la region para cargo de impuesos
        /// </summary>
        /// <param name="enable">Indica si esta habilitado o deshabilitado</param>
        /// <param name="isNew">Indica si es una nueva fila</param>
        private void EnableImpuestos(bool enable, bool isNew)
        {

            #region Habilita o no los controles de impuestos
            this.txtBaseIVA1.Enabled = enable;
            this.txtBaseIVA2.Enabled = enable;
            //this.txtValorIVA1.Properties.ReadOnly = !enable;
            this.txtValorIVA2.Enabled = enable;
            this.txtValorRfte.Enabled = enable;
            this.txtValorRIva.Enabled = enable;
            this.txtValorRIva2.Enabled = enable;
            this.txtValorRIca.Enabled = enable;
            this.chkRfte.Enabled = enable;
            this.chkRIca.Enabled = enable;
            this.chkRIva.Enabled = enable;
            this.chkRIva2.Enabled = enable;
            #endregion
            #region Limpia los campos de impuestos
            if (isNew)
            {
                this.txtBaseIVA1.EditValue = "0";
                this.txtBaseIVA2.EditValue = "0";              
                this.txtBaseRfte.EditValue = "0";
                this.txtBaseRIva.EditValue = "0";
                this.txtBaseRIva2.EditValue = "0";
                this.txtBaseRIca.EditValue = "0";
                this.txtBaseConsumo.EditValue = "0";
                this.txtValorIVA1.EditValue = "0";
                this.txtValorIVA2.EditValue = "0";
                this.txtValorRfte.EditValue = "0";
                this.txtValorRIva.EditValue = "0";
                this.txtValorRIva2.EditValue = "0";
                this.txtValorRIca.EditValue = "0";
                this.txtValorConsumo.EditValue = "0";
                this.txtPorcApRfte.EditValue = "0";
                this.txtPorcApRIva.EditValue = "0";
                this.txtPorcApRIva2.EditValue = "0";
                this.txtPorcApRIca.EditValue = "0";
                this.txtPorConsumo.EditValue = "0";
                this.chkRfte.Checked = false;
                this.chkRIca.Checked = false;
                this.chkRIva.Checked = false;
                this.chkRIva2.Checked = false;
                this.txtReteFuente.Text = string.Empty;
                this.txtReteIVA.Text = string.Empty;
                this.txtReteIVA2.Text = string.Empty;
                this.txtReteICA.Text = string.Empty;
                this.txtReteICA.Text = string.Empty;
                this.txtConsumo.Text = string.Empty;
            }
            #endregion

        }

        /// <summary>
        /// Revisa si el documento actual tiene temporales
        /// </summary>
        /// <returns></returns>
        private bool HasTemporales()
        {
            return _bc.AdministrationModel.aplTemporales_HasTemp(this.documentID.ToString(), _bc.AdministrationModel.User);
        }

        /// <summary>
        /// Trae las retenciones al momento de tener la informacion necesaria
        /// </summary>
        private void GetImpuestos()
        {
            try
            {
                if (this.masterCargoEspecial.ValidID && this.masterTercero.ValidID && this.masterLugarGeo.ValidID && (this.masterProyecto.ValidID || this.masterCentroCosto.ValidID))
                {

                    #region Resetea Controles de Imp
                    this.txtReteFuente.Text = string.Empty;
                    this.txtReteIVA.Text = string.Empty;
                    this.txtReteIVA2.Text = string.Empty;
                    this.txtReteICA.Text = string.Empty;
                    this.txtConsumo.Text = string.Empty;
                    this.txtPorcApRfte.EditValue = 0;
                    this.txtPorcApRIva.EditValue = 0;
                    this.txtPorcApRIva2.EditValue = 0;
                    this.txtPorcApRIca.EditValue = 0;
                    this.txtPorcApIVA1.EditValue = 0;
                    this.txtPorcApIVA2.EditValue = 0;
                    this.txtPorConsumo.EditValue = 0;
                    this.grpCtrlImpuestos.Enabled = true;
                    int index = this.NumFila;
                    this.data.Footer[index].PorIVA1.Value = 0;
                    this.data.Footer[index].PorIVA2.Value = 0;
                    this.data.Footer[index].PorRteFuente.Value = 0;
                    this.data.Footer[index].PorRteICA.Value = 0;
                    this.data.Footer[index].PorRteIVA1.Value = 0;
                    this.data.Footer[index].PorRteIVA2.Value = 0;
                    this.data.Footer[index].PorImpConsumo.Value = 0;
                    #endregion
                    #region Trae los impuestos existentes
                    if (string.IsNullOrEmpty(this._regFiscalTercero))
                    {
                        DTO_coTercero terceroReg = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.data.Footer[indexFila].TerceroID.Value, true);
                        this._regFiscalTercero = terceroReg.ReferenciaID.Value;
                    }
                    DTO_glConsulta consulta = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    //Filtro del Impuesto
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "RegimenFiscalEmpresaID",
                        ValorFiltro = _regFiscalEmp.ReferenciaID.Value,
                        OperadorFiltro = OperadorFiltro.Igual,
                        OperadorSentencia = "AND"
                    });
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "RegimenFiscalTerceroID",
                        ValorFiltro = _regFiscalTercero,
                        OperadorFiltro = OperadorFiltro.Igual,
                         OperadorSentencia = "AND"
                    });
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LugarGeograficoID",
                        ValorFiltro = this.masterLugarGeo.Value,
                        OperadorFiltro = OperadorFiltro.Igual,
                        OperadorSentencia = "AND"
                    });
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ConceptoCargoID",
                        ValorFiltro = this._conceptoCargo,
                        OperadorFiltro = OperadorFiltro.Igual
                    });
                    consulta.Filtros = filtros;
                    long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, true);
                    _listImpuestos = (List<DTO_MasterComplex>)_bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, true).ToList();
                    #endregion                    
                    #region Valida la información para obtener los porcentajes de Retenciones
                    if (_listImpuestos != null)
                    {                            
                        DTO_coPlanCuenta cta;
                        #region IVAs/ RFte / ICA / Consumo
                        foreach (var itemImp in _listImpuestos)
                        {
                            DTO_coImpuesto _impuesto = (DTO_coImpuesto)itemImp;
                            cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, _impuesto.CuentaID.Value, true);

                            if (_impuesto.ImpuestoTipoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.cp, Librerias.Project.AppControl.cp_CodigoIVA) &&
                                _impuesto.LugarGeograficoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.co, Librerias.Project.AppControl.co_LugarGeoXDefecto))
                            {
                                this._ctaIVA1 = cta;
                                this.txtPorcApIVA1.EditValue = cta.ImpuestoPorc.Value != null? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].PorIVA1.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                            }
                            else if (_impuesto.ImpuestoTipoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.cp, Librerias.Project.AppControl.cp_CodigoReteFuente))
                            {
                                this.txtReteFuente.Text = _impuesto.ImpuestoTipoID.Value;
                                this.txtPorcApRfte.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0; 
                                this.data.Footer[index].ImpRFteDesc.Value = this.txtReteFuente.Text;
                                this.data.Footer[index].PorRteFuente.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].MontoMinimo.Value = cta.MontoMinimo.Value;
                            }
                            else if (_impuesto.ImpuestoTipoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                            {
                                this.txtReteICA.Text = _impuesto.ImpuestoTipoID.Value;
                                this.txtPorcApRIca.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].ImpRICADesc.Value = this.txtReteICA.Text;
                                this.data.Footer[index].PorRteICA.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0; 
                            }
                            else if (_impuesto.ImpuestoTipoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo))
                            {
                                this.txtConsumo.Text = _impuesto.ImpuestoTipoID.Value;
                                this.txtPorConsumo.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].ImpConsumoDesc.Value = this.txtConsumo.Text;
                                this.data.Footer[index].PorImpConsumo.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                            }
                            if (!string.IsNullOrEmpty(this._conceptoCargoIVA2) && this._ctaIVA2 == null)
                            {
                               #region Obtiene el IVA 2 si existe el concepto cargo correspondiente
                                int indexConcepto = filtros.FindIndex(x => x.CampoFisico.Equals("ConceptoCargoID"));
                                filtros.RemoveAt(indexConcepto);
                                filtros.Add(new DTO_glConsultaFiltro()
                                {
                                    CampoFisico = "ConceptoCargoID",
                                    ValorFiltro = this._conceptoCargoIVA2,
                                    OperadorFiltro = OperadorFiltro.Igual
                                });
                                consulta.Filtros = filtros;
                                long countIVA2 = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, true);
                                List<DTO_MasterComplex> _impuestosIVA2 = (List<DTO_MasterComplex>)_bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, true).ToList();

                                if (_impuestosIVA2 != null)
                                {
                                    DTO_coImpuesto impuestoIVA2 = (DTO_coImpuesto)_impuestosIVA2[0];
                                    cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, impuestoIVA2.CuentaID.Value, true);
                                    if (impuestoIVA2.ImpuestoTipoID.Value == _bc.GetControlValueByCompany(ModulesPrefix.cp, Librerias.Project.AppControl.cp_CodigoIVA))
                                    {
                                        this._ctaIVA2 = cta;
                                        this.txtPorcApIVA2.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                        this.data.Footer[index].PorIVA2.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                    }
                                } 
                               #endregion
                            }
                        }
                        #endregion
                        #region ReteIVA
                        if (this.data.Footer[index].PorIVA1.Value != 0 && this._ctaIVA1 != null)
                        {
                            //coIVARetencion
                            Dictionary<string, string> keysReteIva1 = new Dictionary<string, string>();
                            keysReteIva1.Add("EmpresaGrupoID", this.empresaID);
                            keysReteIva1.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                            keysReteIva1.Add("RegimenFiscalTerceroID", _regFiscalTercero);
                            keysReteIva1.Add("CuentaIVA", this._ctaIVA1.ID.Value);
                            DTO_coIvaRetencion _reteIVA1 = (DTO_coIvaRetencion)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.coIVARetencion, keysReteIva1, true);
                            if (_reteIVA1 != null)
                            {
                                cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, _reteIVA1.CuentaReteIVA.Value, true);
                                this.txtReteIVA.Text = cta.ImpuestoTipoID.Value;
                                this.txtPorcApRIva.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].ImpRIVA1Desc.Value = this.txtReteIVA.Text;
                                this.data.Footer[index].PorRteIVA1.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                            }
                        }
                        else
                        {
                            this.txtReteIVA.Text = string.Empty;
                            this.txtPorcApRIva.EditValue = 0;
                            this.data.Footer[index].ImpRIVA1Desc.Value = string.Empty;
                            this.data.Footer[index].PorRteIVA1.Value = 0;
                        }

                        if (this.data.Footer[index].PorIVA2.Value != 0 && this._ctaIVA2 != null)
                        {
                            //coIVARetencion
                            Dictionary<string, string> keysReteIva2 = new Dictionary<string, string>();
                            keysReteIva2.Add("EmpresaGrupoID", this.empresaID);
                            keysReteIva2.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                            keysReteIva2.Add("RegimenFiscalTerceroID", _regFiscalTercero);
                            keysReteIva2.Add("CuentaIVA", this._ctaIVA2.ID.Value);
                            DTO_coIvaRetencion _reteIVA2 = (DTO_coIvaRetencion)this._bc.AdministrationModel.MasterComplex_GetByID(AppMasters.coIVARetencion, keysReteIva2, true);
                            if (_reteIVA2 != null)
                            {
                                cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, _reteIVA2.CuentaReteIVA.Value, true);
                                this.txtReteIVA2.Text = cta.ImpuestoTipoID.Value;
                                this.txtPorcApRIva2.EditValue = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                                this.data.Footer[index].ImpRIVA2Desc.Value = this.txtReteIVA2.Text;
                                this.data.Footer[index].PorRteIVA2.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value : 0;
                            }
                        }
                        else
                        {
                            this.txtReteIVA2.Text = string.Empty;
                            this.txtPorcApRIva2.EditValue = 0;
                            this.data.Footer[index].ImpRIVA2Desc.Value = string.Empty;
                            this.data.Footer[index].PorRteIVA2.Value = 0;
                        } 
                        #endregion                        
                    } 
                    #endregion
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Deben estar parametrizadas las cuentas de impuestos")); ;
            }
        }

        /// <summary>
        /// Actualiza la informacion de los temporales
        /// </summary>
        protected void UpdateTemp(object data)
        {
            try
            {
                _bc.AdministrationModel.aplTemporales_Save(this.documentID.ToString(), _bc.AdministrationModel.User, data);
            }
            catch (Exception ex)
            {
                throw;
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
            //this.data.Footer = this.data.Footer.OrderBy(X => X.Item.Value).ToList();
            this.gcDocument.DataSource = this.data.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
            {
                this.gvDocument.MoveFirst();
            }

            this.dataLoaded = true;
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
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(fila, col));

                this.LoadEditGridData(false, fila);    
            
                this.isValid = true;

                if (oper)
                    this.CalcularTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "RowIndexChanged"));
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
            
            // Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            //Controles del detalle
            if (this.documentID == AppDocuments.LegalizaTarjeta)
            {
                List<DTO_glConsultaFiltro> filtro = new List<DTO_glConsultaFiltro>();
                filtro.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "CargoTipo",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "9",
                });
                _bc.InitMasterUC(this.masterCargoEspecial, AppMasters.cpCargoEspecial, true, true, true, false, filtro);
            }
            else
                _bc.InitMasterUC(this.masterCargoEspecial, AppMasters.cpCargoEspecial, true, true, true, false);
            _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, false, true, false);
            _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            //Carga Valores por defecto
            this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            //this.format = _bc.GetImportExportFormat(typeof(DTO_ComprobanteFooter), this.documentID);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.dtFecha.DateTime = this.dtPeriod.DateTime;

            this.AddGridCols();
            this.lastColName = this.unboundPrefix + "IndImpAsumido";

            //Trae el Reg Fiscal de la empresa
            DTO_glEmpresa emp = (DTO_glEmpresa)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, false, this.empresaID, true);
            this._regFiscalEmp = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto), true);

            //Habilita el boton de eliminar personalizado
            base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Visible = true;
            base.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.tlSeparatorPanel.RowStyles[2].Height = 190;

            #region Carga Temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DTO_Legalizacion legTemp = (DTO_Legalizacion)_bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (legTemp != null)
                        {
                            try
                            {
                                this.LoadTempData(legTemp);
                                this.GetImpuestos();
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
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "AfterInitialize: " + ex.Message));
                }
            } 
            #endregion
        }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected override void ValidHeaderTB()
        {
            if (this.validHeader && data.Footer.Count > 0 && this._canSendApprove)
            {
                FormProvider.Master.itemFilterDef.Enabled = true;
                FormProvider.Master.itemFilter.Enabled = true;

                //Habilita el boton de salvar
                if (SecurityManager.HasAccess(this.documentID, FormsActions.Add) || SecurityManager.HasAccess(this.documentID, FormsActions.Edit))
                    FormProvider.Master.itemSave.Enabled = true;
                else
                    FormProvider.Master.itemSave.Enabled = false;

                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
            else
            {
                if (data.Footer.Count == 0)
                    FormProvider.Master.itemSave.Enabled = false;
                else
                    FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemFilterDef.Enabled = false;
                FormProvider.Master.itemFilter.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
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
                #region Columnas Visibles

                //Item
                GridColumn item = new GridColumn();
                item.FieldName = this.unboundPrefix + "Item";
                item.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Item");
                item.UnboundType = UnboundColumnType.Integer;
                item.VisibleIndex = 0;
                item.Width = 27;
                item.Visible = true;
                this.gvDocument.Columns.Add(item);

                //Factura
                GridColumn factura = new GridColumn();
                factura.FieldName = this.unboundPrefix + "Factura";
                factura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Factura");
                factura.UnboundType = UnboundColumnType.Integer;
                factura.VisibleIndex = 1;
                factura.Width = 70;
                factura.Visible = true;
                this.gvDocument.Columns.Add(factura);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 2;
                fecha.Width = 70;
                fecha.Visible = true;
                this.gvDocument.Columns.Add(fecha);

                //NombreTercero
                GridColumn nombreTercero = new GridColumn();
                nombreTercero.FieldName = this.unboundPrefix + "Nombre";
                nombreTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NombreTercero");
                nombreTercero.UnboundType = UnboundColumnType.String;
                nombreTercero.VisibleIndex = 3;
                nombreTercero.Width = 70;
                nombreTercero.Visible = true;
                this.gvDocument.Columns.Add(nombreTercero);

                //Tercero
                GridColumn tercero = new GridColumn();
                tercero.FieldName = this.unboundPrefix + "TerceroID";
                tercero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                tercero.UnboundType = UnboundColumnType.String;
                tercero.VisibleIndex = 4;
                tercero.Width = 70;
                tercero.Visible = true;
                this.gvDocument.Columns.Add(tercero);

                //ValorCargo(ValorBruto)
                GridColumn valorCargo = new GridColumn();
                valorCargo.FieldName = this.unboundPrefix + "ValorBruto";
                valorCargo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorBruto");
                valorCargo.UnboundType = UnboundColumnType.Decimal;
                valorCargo.VisibleIndex = 5;
                valorCargo.Width = 100;
                valorCargo.Visible = true;
                this.gvDocument.Columns.Add(valorCargo);

                //Sumatoria IVA
                GridColumn sumIVA = new GridColumn();
                sumIVA.FieldName = this.unboundPrefix + "SumIVA";
                sumIVA.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SumIVA");
                sumIVA.UnboundType = UnboundColumnType.Decimal;
                sumIVA.VisibleIndex = 6;
                sumIVA.Width = 100;
                sumIVA.Visible = true;
                this.gvDocument.Columns.Add(sumIVA);

                //Sumatoria Retenciones
                GridColumn SumRete = new GridColumn();
                SumRete.FieldName = this.unboundPrefix + "SumRete";
                SumRete.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SumRete");
                SumRete.UnboundType = UnboundColumnType.Decimal;
                SumRete.VisibleIndex = 7;
                SumRete.Width = 100;
                SumRete.Visible = true;
                this.gvDocument.Columns.Add(SumRete);

                //Valor Neto
                GridColumn valorNeto = new GridColumn();
                valorNeto.FieldName = this.unboundPrefix + "ValorNeto";
                valorNeto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorNeto");
                valorNeto.UnboundType = UnboundColumnType.Decimal;
                valorNeto.VisibleIndex = 8;
                valorNeto.Width = 100;
                valorNeto.Visible = true;
                this.gvDocument.Columns.Add(valorNeto);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 9;
                proyecto.Width = 100;
                proyecto.Visible = true;
                this.gvDocument.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 10;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                this.gvDocument.Columns.Add(ctoCosto);

                //Lugar Geografico(ciudad)
                GridColumn lg = new GridColumn();
                lg.FieldName = this.unboundPrefix + "LugarGeograficoID";
                lg.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeograficoID");
                lg.UnboundType = UnboundColumnType.String;
                lg.VisibleIndex = 11;
                lg.Width = 100;
                lg.Visible = true;
                this.gvDocument.Columns.Add(lg);

                //Indicador ReteFuenteAsumido
                GridColumn rteFteAsumidoInd = new GridColumn();
                rteFteAsumidoInd.FieldName = this.unboundPrefix + "RteFteAsumidoInd";
                rteFteAsumidoInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RteFteAsumidoInd");
                rteFteAsumidoInd.UnboundType = UnboundColumnType.Boolean;
                rteFteAsumidoInd.VisibleIndex = 12;
                rteFteAsumidoInd.Width = 50;
                rteFteAsumidoInd.Visible = true;
                rteFteAsumidoInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvDocument.Columns.Add(rteFteAsumidoInd);

                //Indicador ReteIVA Asumido
                GridColumn rteIVA1AsumidoInd = new GridColumn();    
                rteIVA1AsumidoInd.FieldName = this.unboundPrefix + "RteIVA1AsumidoInd";
                rteIVA1AsumidoInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RteIVA1AsumidoInd");
                rteIVA1AsumidoInd.UnboundType = UnboundColumnType.Boolean;
                rteIVA1AsumidoInd.VisibleIndex = 13;
                rteIVA1AsumidoInd.Width = 50;
                rteIVA1AsumidoInd.Visible = true;
                rteIVA1AsumidoInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvDocument.Columns.Add(rteIVA1AsumidoInd);


                ////Indicador ReteIVA Asumido
                GridColumn rteIVA2AsumidoInd = new GridColumn();
                rteIVA2AsumidoInd.FieldName = this.unboundPrefix + "RteIVA2AsumidoInd";
                rteIVA2AsumidoInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RteIVA2AsumidoInd");
                rteIVA2AsumidoInd.UnboundType = UnboundColumnType.Boolean;
                rteIVA2AsumidoInd.VisibleIndex = 14;
                rteIVA2AsumidoInd.Width = 50;
                rteIVA2AsumidoInd.Visible = true;
                rteIVA2AsumidoInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvDocument.Columns.Add(rteIVA2AsumidoInd);


                //Indicador ReteICA Asumido
                GridColumn rteICAAsumidoInd = new GridColumn();
                rteICAAsumidoInd.FieldName = this.unboundPrefix + "RteICAAsumidoInd";
                rteICAAsumidoInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RteICAAsumidoInd");
                rteICAAsumidoInd.UnboundType = UnboundColumnType.Boolean;
                rteICAAsumidoInd.VisibleIndex = 15;
                rteICAAsumidoInd.Width = 50;
                rteICAAsumidoInd.Visible = true;
                rteICAAsumidoInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                this.gvDocument.Columns.Add(rteICAAsumidoInd);

                #endregion
                #region Columnas No Visibles

                //MonedaID
                GridColumn monedaIdCol = new GridColumn();
                monedaIdCol.FieldName = this.unboundPrefix + "MonedaID";
                monedaIdCol.UnboundType = UnboundColumnType.String;
                monedaIdCol.Visible = false;
                this.gvDocument.Columns.Add(monedaIdCol);

                //CargoEspecialID
                GridColumn cargoEspecialId = new GridColumn();
                cargoEspecialId.FieldName = this.unboundPrefix + "CargoEspecialID";
                cargoEspecialId.UnboundType = UnboundColumnType.String;
                cargoEspecialId.Visible = false;
                this.gvDocument.Columns.Add(cargoEspecialId);

                //FacturaEquivalente
                GridColumn facturaEquivalente = new GridColumn();
                facturaEquivalente.FieldName = this.unboundPrefix + "FactEquivalente";
                facturaEquivalente.UnboundType = UnboundColumnType.String;
                facturaEquivalente.Visible = false;
                this.gvDocument.Columns.Add(facturaEquivalente);

                //TasaCambioDOCU
                GridColumn tasaCambioDOCU = new GridColumn();
                tasaCambioDOCU.FieldName = this.unboundPrefix + "TasaCambioDOCU";
                tasaCambioDOCU.UnboundType = UnboundColumnType.Decimal;
                tasaCambioDOCU.Visible = false;
                this.gvDocument.Columns.Add(tasaCambioDOCU);

                //TasaCambioCONT
                GridColumn tasaCambioCONT = new GridColumn();
                tasaCambioCONT.FieldName = this.unboundPrefix + "TasaCambioCONT";
                tasaCambioCONT.UnboundType = UnboundColumnType.Decimal;
                tasaCambioCONT.Visible = false;
                this.gvDocument.Columns.Add(tasaCambioCONT);

                //NuevoTerceroInd
                GridColumn nuevoTerceroInd = new GridColumn();
                nuevoTerceroInd.FieldName = this.unboundPrefix + "NuevoTerceroInd";
                nuevoTerceroInd.UnboundType = UnboundColumnType.Boolean;
                nuevoTerceroInd.Visible = false;
                this.gvDocument.Columns.Add(nuevoTerceroInd);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.Visible = false;
                this.gvDocument.Columns.Add(descriptivo);

                //PorIVA1
                GridColumn porIVA1 = new GridColumn();
                porIVA1.FieldName = this.unboundPrefix + "PorIVA1";
                porIVA1.UnboundType = UnboundColumnType.Decimal;
                porIVA1.Visible = false;
                this.gvDocument.Columns.Add(porIVA1);

                //BaseIVA1
                GridColumn baseIVA1 = new GridColumn();
                baseIVA1.FieldName = this.unboundPrefix + "BaseIVA1";
                baseIVA1.UnboundType = UnboundColumnType.Decimal;
                baseIVA1.Visible = false;
                this.gvDocument.Columns.Add(baseIVA1);

                //ValorIVA1
                GridColumn valorIVA1 = new GridColumn();
                valorIVA1.FieldName = this.unboundPrefix + "ValorIVA1";
                valorIVA1.UnboundType = UnboundColumnType.Decimal;
                valorIVA1.Visible = false;
                this.gvDocument.Columns.Add(valorIVA1);

                //PorIVA2
                GridColumn porIVA2 = new GridColumn();
                porIVA2.FieldName = this.unboundPrefix + "PorIVA2";
                porIVA2.UnboundType = UnboundColumnType.Decimal;
                porIVA2.Visible = false;
                this.gvDocument.Columns.Add(porIVA2);

                //BaseIVA2
                GridColumn baseIVA2 = new GridColumn();
                baseIVA2.FieldName = this.unboundPrefix + "BaseIVA2";
                baseIVA2.UnboundType = UnboundColumnType.Decimal;
                baseIVA2.Visible = false;
                this.gvDocument.Columns.Add(baseIVA2);

                //ValorIVA2
                GridColumn valorIVA2 = new GridColumn();
                valorIVA2.FieldName = this.unboundPrefix + "ValorIVA2";
                valorIVA2.UnboundType = UnboundColumnType.Decimal;
                valorIVA2.Visible = false;
                this.gvDocument.Columns.Add(valorIVA2);

                //PorRteIVA
                GridColumn porRteIVA1 = new GridColumn();
                porRteIVA1.FieldName = this.unboundPrefix + "PorRteIVA1";
                porRteIVA1.UnboundType = UnboundColumnType.Decimal;
                porRteIVA1.Visible = false;
                this.gvDocument.Columns.Add(porRteIVA1);

                //PorRteIVA2
                GridColumn porRteIVA2 = new GridColumn();
                porRteIVA2.FieldName = this.unboundPrefix + "PorRteIVA2";
                porRteIVA2.UnboundType = UnboundColumnType.Decimal;
                porRteIVA2.Visible = false;
                this.gvDocument.Columns.Add(porRteIVA2);

                //BaseRteIVA
                GridColumn baseRteIVA1 = new GridColumn();
                baseRteIVA1.FieldName = this.unboundPrefix + "BaseRteIVA1";
                baseRteIVA1.UnboundType = UnboundColumnType.Decimal;
                baseRteIVA1.Visible = false;
                this.gvDocument.Columns.Add(baseRteIVA1);

                //BaseRteIVA2
                GridColumn baseRteIVA2 = new GridColumn();
                baseRteIVA2.FieldName = this.unboundPrefix + "BaseRteIVA2";
                baseRteIVA2.UnboundType = UnboundColumnType.Decimal;
                baseRteIVA2.Visible = false;
                this.gvDocument.Columns.Add(baseRteIVA2);

                //ValorRteIVA
                GridColumn valorRteIVA1 = new GridColumn();
                valorRteIVA1.FieldName = this.unboundPrefix + "ValorRteIVA1";
                valorRteIVA1.UnboundType = UnboundColumnType.Decimal;
                valorRteIVA1.Visible = false;
                this.gvDocument.Columns.Add(valorRteIVA1);

                //ValorRteIVA2
                GridColumn valorRteIVA2 = new GridColumn();
                valorRteIVA2.FieldName = this.unboundPrefix + "ValorRteIVA2";
                valorRteIVA2.UnboundType = UnboundColumnType.Decimal;
                valorRteIVA2.Visible = false;
                this.gvDocument.Columns.Add(valorRteIVA2);

                //PorRteFuente
                GridColumn porRteFuente = new GridColumn();
                porRteFuente.FieldName = this.unboundPrefix + "PorRteFuente";
                porRteFuente.UnboundType = UnboundColumnType.Decimal;
                porRteFuente.Visible = false;
                this.gvDocument.Columns.Add(porRteFuente);

                //BaseRteFuente
                GridColumn baseRteFuente = new GridColumn();
                baseRteFuente.FieldName = this.unboundPrefix + "BaseRteFuente";
                baseRteFuente.UnboundType = UnboundColumnType.Decimal;
                baseRteFuente.Visible = false;
                this.gvDocument.Columns.Add(baseRteFuente);

                //ValorRteFuente
                GridColumn valorRteFuente = new GridColumn();
                valorRteFuente.FieldName = this.unboundPrefix + "ValorRteFuente";
                valorRteFuente.UnboundType = UnboundColumnType.Decimal;
                valorRteFuente.Visible = false;
                this.gvDocument.Columns.Add(valorRteFuente);

                //PorRteICA
                GridColumn porRteICA = new GridColumn();
                porRteICA.FieldName = this.unboundPrefix + "PorRteICA";
                porRteICA.UnboundType = UnboundColumnType.Decimal;
                porRteICA.Visible = false;
                this.gvDocument.Columns.Add(porRteICA);

                //BaseRteICA
                GridColumn baseRteICA = new GridColumn();
                baseRteICA.FieldName = this.unboundPrefix + "BaseRteICA";
                baseRteICA.UnboundType = UnboundColumnType.Decimal;
                baseRteICA.Visible = false;
                this.gvDocument.Columns.Add(baseRteICA);

                //ValorRteICA
                GridColumn valorRteICA = new GridColumn();
                valorRteICA.FieldName = this.unboundPrefix + "ValorRteICA";
                valorRteICA.UnboundType = UnboundColumnType.Decimal;
                valorRteICA.Visible = false;
                this.gvDocument.Columns.Add(valorRteICA);

                //PorImpConsumo
                GridColumn PorImpConsumo = new GridColumn();
                PorImpConsumo.FieldName = this.unboundPrefix + "PorImpConsumo";
                PorImpConsumo.UnboundType = UnboundColumnType.Decimal;
                PorImpConsumo.Visible = false;
                this.gvDocument.Columns.Add(PorImpConsumo);

                //BaseImpConsumo
                GridColumn BaseImpConsumo = new GridColumn();
                BaseImpConsumo.FieldName = this.unboundPrefix + "BaseImpConsumo";
                BaseImpConsumo.UnboundType = UnboundColumnType.Decimal;
                BaseImpConsumo.Visible = false;
                this.gvDocument.Columns.Add(BaseImpConsumo);

                //ValorImpConsumo
                GridColumn ValorImpConsumo = new GridColumn();
                ValorImpConsumo.FieldName = this.unboundPrefix + "ValorImpConsumo";
                ValorImpConsumo.UnboundType = UnboundColumnType.Decimal;
                ValorImpConsumo.Visible = false;
                this.gvDocument.Columns.Add(ValorImpConsumo);

                //ImpuestoRFte
                GridColumn ImpuestoRFte = new GridColumn();
                ImpuestoRFte.FieldName = this.unboundPrefix + "ImpRFteDesc";
                ImpuestoRFte.UnboundType = UnboundColumnType.String;
                ImpuestoRFte.Visible = false;
                this.gvDocument.Columns.Add(ImpuestoRFte);

                //ImpuestoRIVA
                GridColumn ImpuestoRIVA = new GridColumn();
                ImpuestoRIVA.FieldName = this.unboundPrefix + "ImpRIVA1Desc";
                ImpuestoRIVA.UnboundType = UnboundColumnType.String;
                ImpuestoRIVA.Visible = false;
                this.gvDocument.Columns.Add(ImpuestoRIVA);

                //ImpuestoRIVA2
                GridColumn ImpuestoRIVA2 = new GridColumn();
                ImpuestoRIVA2.FieldName = this.unboundPrefix + "ImpRIVA2Desc";
                ImpuestoRIVA2.UnboundType = UnboundColumnType.String;
                ImpuestoRIVA2.Visible = false;
                this.gvDocument.Columns.Add(ImpuestoRIVA2);

                //ImpuestoRICA
                GridColumn ImpuestoRICA = new GridColumn();
                ImpuestoRICA.FieldName = this.unboundPrefix + "ImpRICADesc";
                ImpuestoRICA.UnboundType = UnboundColumnType.String;
                ImpuestoRICA.Visible = false;
                this.gvDocument.Columns.Add(ImpuestoRICA);

                //ImpConsumoDesc
                GridColumn ImpConsumoDesc = new GridColumn();
                ImpConsumoDesc.FieldName = this.unboundPrefix + "ImpConsumoDesc";
                ImpConsumoDesc.UnboundType = UnboundColumnType.String;
                ImpConsumoDesc.Visible = false;
                this.gvDocument.Columns.Add(ImpConsumoDesc);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "AddGridCols"));
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
                    if (this.newDoc)
                    {
                        this.EnableFooter(true);
                        this.EnableImpuestos(false, true);
                        this.newDoc = false;
                        this.masterCargoEspecial.EnableControl(true);

                        if (this.footerCurrent != null)
                        {
                            this.masterProyecto.Value = this.footerCurrent.ProyectoID.Value;//val_Proyecto;
                            this.masterCentroCosto.Value = this.footerCurrent.CentroCostoID.Value;//val_CentroCosto;
                            this.masterLugarGeo.Value = this.footerCurrent.LugarGeograficoID.Value;//val_LugarGeo;
                        }
                    }
                    else
                    {
                        #region Campos Impuestos
                        //string val_ValorBruto = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorBruto"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorBruto"]).ToString();
                        //string val_ValorNeto = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorNeto"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorNeto"]).ToString();
                        //string val_PorIVA1 = (isNew || gvDocument.Columns[this.unboundPrefix + "PorIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorIVA1"]).ToString();
                        //string val_BaseIVA1 = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseIVA1"]).ToString();
                        //string val_ValorIVA1 = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorIVA1"]).ToString();
                        //string val_PorIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "PorIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorIVA2"]).ToString();
                        //string val_BaseIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseIVA2"]).ToString();
                        //string val_ValorIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorIVA2"]).ToString();
                        //string val_RteIVAAsumidoInd = (isNew || gvDocument.Columns[this.unboundPrefix + "RteIVA1AsumidoInd"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "RteIVA1AsumidoInd"]).ToString();
                        //string val_RteIVAAsumidoInd2 = this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "RteIVA1AsumidoInd"]).ToString();
                        //string val_RteIVA2AsumidoInd = (isNew || gvDocument.Columns[this.unboundPrefix + "RteIVA2AsumidoInd"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "RteIVA2AsumidoInd"]).ToString();
                        //string val_PorRteIVA = (isNew || gvDocument.Columns[this.unboundPrefix + "PorRteIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorRteIVA1"]).ToString();
                        //string val_PorRteIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "PorRteIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorRteIVA2"]).ToString();
                        //string val_BaseRteIVA = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseRteIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseRteIVA1"]).ToString();
                        //string val_BaseRteIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseRteIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseRteIVA2"]).ToString();
                        //string val_ValorRteIVA = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorRteIVA1"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorRteIVA1"]).ToString();
                        //string val_ValorRteIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorRteIVA2"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorRteIVA2"]).ToString();
                        //string val_RteFteAsumidoInd = (isNew || gvDocument.Columns[this.unboundPrefix + "RteFteAsumidoInd"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "RteFteAsumidoInd"]).ToString();
                        //string val_PorRteFuente = (isNew || gvDocument.Columns[this.unboundPrefix + "PorRteFuente"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorRteFuente"]).ToString();
                        //string val_BaseRteFuente = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseRteFuente"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseRteFuente"]).ToString();
                        //string val_ValorRteFuente = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorRteFuente"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorRteFuente"]).ToString();
                        //string val_RteICAAsumidoInd = (isNew || gvDocument.Columns[this.unboundPrefix + "RteICAAsumidoInd"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "RteICAAsumidoInd"]).ToString();
                        //string val_PorRteICA = (isNew || gvDocument.Columns[this.unboundPrefix + "PorRteICA"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorRteICA"]).ToString();
                        //string val_BaseRteICA = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseRteICA"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseRteICA"]).ToString();
                        //string val_ValorRteICA = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorRteICA"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorRteICA"]).ToString();
                        //string val_PorImpConsumo = (isNew || gvDocument.Columns[this.unboundPrefix + "PorImpConsumo"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PorImpConsumo"]).ToString();
                        //string val_BaseImpConsumo = (isNew || gvDocument.Columns[this.unboundPrefix + "BaseImpConsumo"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "BaseImpConsumo"]).ToString();
                        //string val_ValorImpConsumo = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorImpConsumo"] == null || gvDocument.RowCount == 0) ? "0" : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorImpConsumo"]).ToString();
                        //string val_ImpuestoRFte = (isNew || gvDocument.Columns[this.unboundPrefix + "ImpRFteDesc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ImpRFteDesc"]).ToString();
                        //string val_ImpuestoRIVA = (isNew || gvDocument.Columns[this.unboundPrefix + "ImpRIVA1Desc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ImpRIVA1Desc"]).ToString();
                        //string val_ImpuestoRIVA2 = (isNew || gvDocument.Columns[this.unboundPrefix + "ImpRIVA2Desc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ImpRIVA2Desc"]).ToString();
                        //string val_ImpuestoRICA = (isNew || gvDocument.Columns[this.unboundPrefix + "ImpRICADesc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ImpRICADesc"]).ToString();
                        //string val_ImpuestoConsumo = (isNew || gvDocument.Columns[this.unboundPrefix + "ImpConsumoDesc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ImpConsumoDesc"]).ToString();

                        #endregion
                        #region Asignacion de Valores
                        this.masterCargoEspecial.Value = this.footerCurrent.CargoEspecialID.Value;// val_CargoEspecial;
                        this.masterTercero.Value = this.footerCurrent.TerceroID.Value;//val_Tercero;
                        this.txtNombre.Text = this.footerCurrent.Nombre.Value;//val_NombreTercero;
                        this.txtFactura.Text = this.footerCurrent.Factura.Value;//val_Factura;
                        this.txtFacturaEqui.Text = this.footerCurrent.FactEquivalente.Value;//val_FacturaEquivalente;
                        this.dtFechaDetalle.DateTime = this.footerCurrent.Fecha.Value.Value;//Convert.ToDateTime(val_Fecha);
                        this.txtTasaCambioDoc.EditValue = this.footerCurrent.TasaCambioDOCU.Value;//val_TasaCambioDocu;
                        this.txtTasaCambioCont.EditValue = this.footerCurrent.TasaCambioCONT.Value;//val_TasaCambioCont;
                        this.txtDescripcionCosto.Text = this.footerCurrent.Descriptivo.Value;//val_Descripcion;
                        this.masterProyecto.Value = this.footerCurrent.ProyectoID.Value;//val_Proyecto;
                        this.masterCentroCosto.Value = this.footerCurrent.CentroCostoID.Value;//val_CentroCosto;
                        this.masterLugarGeo.Value = this.footerCurrent.LugarGeograficoID.Value;//val_LugarGeo;
                        this.masterMoneda.Value = this.footerCurrent.MonedaID.Value;//val_Moneda;
                        this.txtValor.EditValue = this.footerCurrent.ValorBruto.Value;//val_ValorBruto;
                        this.txtNeto.EditValue = this.footerCurrent.ValorNeto.Value;//val_ValorNeto;
                        this.txtPorcApIVA1.EditValue = this.footerCurrent.PorIVA1.Value;//val_PorIVA1;
                        this.txtBaseIVA1.EditValue = this.footerCurrent.BaseIVA1.Value;//val_BaseIVA1;
                        this.txtValorIVA1.EditValue = this.footerCurrent.ValorIVA1.Value;//val_ValorIVA1.Replace(",",".");
                        this.txtPorcApIVA2.EditValue = this.footerCurrent.PorIVA2.Value;//val_PorIVA2;
                        this.txtBaseIVA2.EditValue = this.footerCurrent.BaseIVA2.Value;//val_BaseIVA2;
                        this.txtValorIVA2.EditValue = this.footerCurrent.ValorIVA2.Value;//val_ValorIVA2.Replace(",", "."); 
                        this.chkRIva.Checked = this.footerCurrent.RteIVA1AsumidoInd.Value.Value;//Convert.ToBoolean(val_RteIVAAsumidoInd) ? true : false;
                        this.chkRIva2.Checked = this.footerCurrent.RteIVA2AsumidoInd.Value.Value;//Convert.ToBoolean(val_RteIVA2AsumidoInd) ? true : false;
                        this.txtPorcApRIva.EditValue = this.footerCurrent.PorRteIVA1.Value;//val_PorRteIVA;
                        this.txtPorcApRIva2.EditValue = this.footerCurrent.PorRteIVA2.Value;//val_PorRteIVA2;
                        this.txtBaseRIva.EditValue = this.footerCurrent.BaseRteIVA1.Value;//val_BaseRteIVA;
                        this.txtBaseRIva2.EditValue = this.footerCurrent.BaseRteIVA2.Value;//val_BaseRteIVA2;
                        this.txtValorRIva.EditValue = this.footerCurrent.ValorRteIVA1.Value;//val_ValorRteIVA.Replace(",", ".");
                        this.txtValorRIva2.EditValue = this.footerCurrent.ValorRteIVA2.Value;//val_ValorRteIVA2.Replace(",", ".");
                        this.chkRfte.Checked = this.footerCurrent.RteFteAsumidoInd.Value.Value;//Convert.ToBoolean(val_RteFteAsumidoInd) ? true : false;
                        this.txtPorcApRfte.EditValue = this.footerCurrent.PorRteFuente.Value;//val_PorRteFuente;
                        this.txtBaseRfte.EditValue = this.footerCurrent.BaseRteFuente.Value;//val_BaseRteFuente;
                        this.txtValorRfte.EditValue = this.footerCurrent.ValorRteFuente.Value;//val_ValorRteFuente.Replace(",", ".");
                        this.chkRIca.Checked = this.footerCurrent.RteICAAsumidoInd.Value.Value;//Convert.ToBoolean(val_RteICAAsumidoInd) ? true : false;
                        this.txtPorcApRIca.EditValue = this.footerCurrent.PorRteICA.Value;//val_PorRteICA;
                        this.txtBaseRIca.EditValue = this.footerCurrent.BaseRteICA.Value;//val_BaseRteICA;
                        this.txtValorRIca.EditValue = this.footerCurrent.ValorRteICA.Value;//val_ValorRteICA.Replace(",", ".");
                        this.txtPorConsumo.EditValue = this.footerCurrent.PorImpConsumo.Value;//val_PorImpConsumo;
                        this.txtBaseConsumo.EditValue = this.footerCurrent.BaseImpConsumo.Value;//val_BaseImpConsumo;
                        this.txtValorConsumo.EditValue = this.footerCurrent.ValorImpConsumo.Value;//val_ValorImpConsumo.Replace(",", ".");   
                        this.txtReteFuente.Text = this.footerCurrent.ImpRFteDesc.Value;//val_ImpuestoRFte;
                        this.txtReteIVA.Text = this.footerCurrent.ImpRIVA1Desc.Value;//val_ImpuestoRIVA;
                        this.txtReteIVA2.Text = this.footerCurrent.ImpRIVA2Desc.Value;//val_ImpuestoRIVA2;
                        this.txtReteICA.Text = this.footerCurrent.ImpRICADesc.Value;//val_ImpuestoRICA;
                        this.txtConsumo.Text = this.footerCurrent.ImpConsumoDesc.Value;//val_ImpuestoConsumo;
                        #endregion
                        
                        this.Cargo = (DTO_cpCargoEspecial)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, false, this.footerCurrent.CargoEspecialID.Value, true);
                        this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.footerCurrent.TerceroID.Value, true);
                        if (this.Tercero != null)
                        {
                            this._regFiscalTercero = this.Tercero.ReferenciaID.Value;
                            this.dtoRegFiscalTer = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, this._regFiscalTercero, true);
                            this._declaraIVA = this.Tercero.DeclaraIVAInd.Value.Value;
                            this.EnableImpuestos(this._declaraIVA, isNew);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "LoadEditGridData"));
                }
            }
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
                    GridColumn col = new GridColumn();
                    #region Validacion de nulls y Fks
                    #region CargoEspecial
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CargoEspecialID", false, true, false, AppMasters.cpCargoEspecial);
                    if (!validField)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CargoEspecialEmpty));
                        validRow = false;
                    }
                    #endregion
                    #region Factura
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Factura", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Fecha
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Fecha", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Tercero
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "TerceroID", false, false, false, null);
                    //validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "TerceroID", false, true, false, AppMasters.coTercero);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region Nombre Tercero
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Nombre", false, false, false, null);
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
                    #region Lugar Geografico
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "LugarGeograficoID", false, true, true, AppMasters.glLugarGeografico);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #endregion
                    if (validRow)
                    {
                        this.isValid = true;
                        if (!this.newReg)
                            this.UpdateTemp(this.data);
                    }
                    else
                        this.isValid = false; 
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "ValidateRow"));
            }

            this.hasChanges = true;
            return validRow;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "LoadTasaCambio"));
                return 0;
            }
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
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            #region Habilita/Deshabilita los controles
            //Campos de maestras
            this.masterCargoEspecial.EnableControl(enable);
            this.masterTercero.EnableControl(enable);
            this.masterProyecto.EnableControl(enable);
            this.masterCentroCosto.EnableControl(enable);
            this.masterLugarGeo.EnableControl(enable);
            this.masterMoneda.EnableControl(enable);
            //Campos de texto
            this.txtNombre.Enabled = enable;
            this.txtFactura.Enabled = enable;
            this.txtDescripcionCosto.Enabled = enable;
            //Campos de fechas
            this.dtFechaDetalle.Enabled = enable;
            //Campos de valores
            this.txtValor.Enabled = enable;
            this.txtBaseRIva.Enabled = enable;
            this.txtBaseRIva2.Enabled = enable;
            this.txtBaseIVA1.Enabled = enable;
            this.txtBaseIVA2.Enabled = enable;
            this.txtValorIVA1.Enabled = enable;
            this.txtValorIVA2.Enabled = enable;
            this.txtValorRfte.Enabled = enable;
            this.txtValorRIva.Enabled = enable;
            this.txtValorRIva2.Enabled = enable;
            this.txtValorRIca.Enabled = enable;
            this.txtValorConsumo.Enabled = enable;
            //Campos de actividad
            this.chkRfte.Enabled = enable;
            this.chkRIca.Enabled = enable;
            this.chkRIva.Enabled = enable;
            this.chkRIva2.Enabled = enable;
            //Verifica el documento para manejar controles
            if (this.documentID == AppDocuments.CajaMenor)
            {
                this.lblTasaCambioDoc.Visible = false;
                this.txtTasaCambioDoc.Visible = false;
                this.lblDescripcionCosto.Location = new System.Drawing.Point(317, 78);
                this.txtDescripcionCosto.Location = new System.Drawing.Point(390, 77);
                this.txtDescripcionCosto.Size = new System.Drawing.Size(123, 55);
                this.masterMoneda.EnableControl(false);
            }


            #endregion

            if (!enable)
            {
                #region Limpia los campos
                //Campos de maestras
                this.masterCargoEspecial.Value = string.Empty;
                this.masterTercero.Value = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.masterCentroCosto.Value = string.Empty;
                this.masterLugarGeo.Value = string.Empty;
                this.masterMoneda.Value = string.Empty;
                //Campos de texto
                this.txtNombre.Text = string.Empty;
                this.txtFactura.Text = string.Empty;
                this.txtDescripcionCosto.Text = string.Empty;
                this.txtReteFuente.Text = string.Empty;
                this.txtReteIVA.Text = string.Empty;
                this.txtReteIVA2.Text = string.Empty;
                this.txtReteICA.Text = string.Empty;
                this.txtConsumo.Text = string.Empty;
                //Campos de fechas
                this.dtFechaDetalle.DateTime = this.dtFecha.DateTime;
                //Campos de valores
                this.txtTasaCambioDoc.EditValue = 0;
                this.txtTasaCambioCont.EditValue = 0;
                this.txtValor.EditValue = "0";
                this.txtNeto.EditValue = "0";
                this.txtBaseIVA1.EditValue = "0";
                this.txtBaseIVA2.EditValue = "0";
                this.txtBaseRfte.EditValue = "0";               
                this.txtBaseRIva.EditValue = "0";
                this.txtBaseRIva2.EditValue = "0";
                this.txtBaseRIca.EditValue = "0";
                this.txtBaseConsumo.EditValue = "0";
                this.txtValorIVA1.EditValue = "0";
                this.txtValorIVA2.EditValue = "0";                          
                this.txtValorRfte.EditValue = "0";               
                this.txtValorRIva.EditValue = "0";
                this.txtValorRIva2.EditValue = "0";
                this.txtValorRIca.EditValue = "0";
                this.txtValorConsumo.EditValue = "0";
                this.txtPorcApIVA1.EditValue = "0";
                this.txtPorcApIVA2.EditValue = "0";  
                this.txtPorcApRfte.EditValue = 0;              
                this.txtPorcApRIva.EditValue = 0;
                this.txtPorcApRIva2.EditValue = 0;
                this.txtPorcApRIca.EditValue = 0;
                this.txtPorConsumo.EditValue = 0;
                //Campos de actividad
                this.chkNuevo.Checked = false;
                this.chkRfte.Checked = false;
                this.chkRIca.Checked = false;
                this.chkRIva.Checked = false;
                this.chkRIva2.Checked = false;
                #endregion
            }
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
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected virtual decimal CalcularTotal()
        {
            try
            {
                decimal sumNeto = 0;
                for (int i = 0; i < this.gvDocument.DataRowCount; i++)
                {
                    decimal neto = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, this.unboundPrefix + "ValorNeto"), CultureInfo.InvariantCulture);
                    sumNeto += neto;
                }

                return sumNeto;
            }
            catch (Exception e)
            {
                throw;
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
        protected virtual DTO_Legalizacion LoadTempHeader() { return null; }

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
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(DTO_Legalizacion aux) { }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            try
            {
                DTO_cpLegalizaFooter footerDet = new DTO_cpLegalizaFooter();

                #region Asigna datos a la fila

                if (this.data.Footer.Count > 0)
                {
                    footerDet.Index = this.data.Footer.Last().Index + 1;
                    footerDet.Item.Value = footerDet.Index + 1;
                    footerDet.EmpresaID.Value = this.empresaID;
                    footerDet.Fecha.Value = this.data.Footer.Last().Fecha.Value;
                    footerDet.MonedaID.Value = this.data.Footer.Last().MonedaID.Value;
                    footerDet.CentroCostoID.Value = this.data.Footer.Last().CentroCostoID.Value;
                    footerDet.ProyectoID.Value = this.data.Footer.Last().ProyectoID.Value;
                    footerDet.LugarGeograficoID.Value = this.data.Footer.Last().LugarGeograficoID.Value;
                }
                else
                {
                    footerDet.Index = 0;
                    footerDet.Item.Value = footerDet.Index + 1;
                    footerDet.EmpresaID.Value = this.empresaID;
                    footerDet.Fecha.Value = dtFecha.DateTime;
                    footerDet.MonedaID.Value = this.monedaId;
                    footerDet.CentroCostoID.Value = string.Empty;
                    footerDet.ProyectoID.Value = string.Empty;
                    footerDet.LugarGeograficoID.Value = this.defLugarGeo;                  
                }

                footerDet.ValorBruto.Value = 0;
                footerDet.ValorNeto.Value = 0;
                footerDet.BaseIVA1.Value = 0;
                footerDet.BaseIVA2.Value = 0;
                footerDet.BaseRteFuente.Value = 0;
                footerDet.BaseRteIVA1.Value = 0;
                footerDet.BaseRteIVA2.Value = 0;
                footerDet.BaseRteICA.Value = 0;
                footerDet.BaseImpConsumo.Value = 0;
                footerDet.ValorIVA1.Value = 0;
                footerDet.ValorIVA2.Value = 0;              
                footerDet.ValorRteFuente.Value = 0;
                footerDet.ValorRteIVA1.Value = 0;
                footerDet.ValorRteIVA2.Value = 0;
                footerDet.ValorRteICA.Value = 0;
                footerDet.ValorImpConsumo.Value = 0;
                footerDet.PorIVA1.Value = 0;
                footerDet.PorIVA2.Value = 0;
                footerDet.PorRteFuente.Value = 0;
                footerDet.PorRteIVA1.Value = 0;
                footerDet.PorRteIVA2.Value = 0;
                footerDet.PorRteICA.Value = 0;
                footerDet.PorImpConsumo.Value = 0;
                footerDet.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambioCont.EditValue, CultureInfo.InvariantCulture);
                footerDet.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambioDoc.EditValue, CultureInfo.InvariantCulture);
                footerDet.RteFteAsumidoInd.Value = false;
                footerDet.RteIVA1AsumidoInd.Value = false;
                footerDet.RteIVA2AsumidoInd.Value = false;
                footerDet.RteICAAsumidoInd.Value = false;
                footerDet.SumIVA.Value = 0;
                footerDet.SumRete.Value = 0;
                #endregion

                this.data.Footer.Add(footerDet);
                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                this.isValid = false;
                this.EnableFooter(true);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;
                this.masterCargoEspecial.Focus();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizacion.cs", "AddNewRow"));
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

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.HasTemporales())
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgLostInfo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    base.Form_FormClosing(sender, e);
                }
                else
                {
                    e.Cancel = true;
                }
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
            if (this.validHeader)
            {
                if (this.data == null)
                {
                    this.gcDocument.Focus();
                    e.Handled = true;
                }

                if (e.Button.ImageIndex == 6)
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
                if (e.Button.ImageIndex == 7)
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
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_RowsNeededLegal));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.Footer.RemoveAll(x => x.Index == this.footerCurrent.Index);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvDocument.RefreshData();
                            if (this.gvDocument.FocusedRowHandle >= 0)
                            {
                                this.footerCurrent = (DTO_cpLegalizaFooter)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                                this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                                this.ValidateRow(rowHandle - 1);
                            }
                        }
                    }
                    e.Handled = true;
                }
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
            if (fieldName == "Fecha")
            {
                e.RepositoryItem = this.editDate;
            }
            if (fieldName == "ValorBruto" || fieldName == "SumIVA" || fieldName == "SumRete" || fieldName == "ValorNeto")
            {
                e.RepositoryItem = e.RepositoryItem = this.editValue; //this.editSpin;
            }
            base.gvDocument.OptionsBehavior.Editable = false;
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
                    this.footerCurrent = (DTO_cpLegalizaFooter)this.gvDocument.GetRow(e.FocusedRowHandle);
                    if (this.footerCurrent != null)
                        this.RowIndexChanged(e.FocusedRowHandle, false);
                }
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "gvDocument_FocusedRowChanged"));
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
                        case "CargoEspecialID":
                            #region Cargo Especial
                            if (master.ValidID)
                            {
                                if (this.Cargo == null || master.Value != this.Cargo.ID.Value)
                                {
                                    this.data.Footer[index].CargoEspecialID.Value = master.Value;
                                    this.Cargo = (DTO_cpCargoEspecial)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, false, master.Value, true);
                                    this.GetImpuestos();
                                }
                             }
                            else
                            {
                                this.Cargo = null;
                                this.data.Footer[index].CargoEspecialID.Value = string.Empty;
                                this.data.Footer[index].MonedaID.Value = this.masterMoneda.Value;
                                this.EnableImpuestos(false, false);
                                this.txtPorcApIVA1.EditValue = 0;
                                this.txtPorcApIVA2.EditValue = 0;
                            }
                            if (Convert.ToDouble(this.txtValor.EditValue) != 0)
                                this.textEditControl_Leave(this.txtValor, e);
                            this.masterTercero.Focus();
                            #endregion
                            break;
                        case "TerceroID":
                            #region Tercero                                  
                            this.data.Footer[index].TerceroID.Value = master.Value;
                            this.Tercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);
                            if (this.Tercero == null)
                            {
                                this.txtNombre.Text = string.Empty;
                                this.txtNombre.Enabled = true;
                                this.chkNuevo.Checked = true;
                            }
                            else
                            {
                                this._regFiscalTercero = this.Tercero.ReferenciaID.Value;
                                this.txtNombre.Text = this.Tercero.Descriptivo.Value;
                                this.txtNombre.Enabled = false;
                                this.chkNuevo.Checked = false;
                                this._declaraIVA = this.Tercero.DeclaraIVAInd.Value.Value;
                                this.dtoRegFiscalTer = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, _regFiscalTercero, true);
                                if (!_declaraIVA && (this.dtoRegFiscalTer.TipoTercero.Value.Value == 1 || this.dtoRegFiscalTer.TipoTercero.Value.Value == 2))
                                    this.EnableImpuestos(_declaraIVA, true);
                                else
                                    this.EnableImpuestos(_declaraIVA, true);
                            }
                            if (this.chkNuevo.Checked)
                            {
                                this.Tercero = null;
                                this.EnableImpuestos(false, true);
                            }
                            if (Convert.ToDouble(this.txtValor.EditValue) != 0)
                                this.textEditControl_Leave(this.txtValor, e);
                            #endregion
                            break;
                        case "ProyectoID":
                            #region Proyecto
                            if (master.ValidID)
                            {
                                this.data.Footer[index].ProyectoID.Value = master.Value;
                                DTO_coProyecto proy = (DTO_coProyecto)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, master.Value, true);
                                this._operacion = proy.OperacionID.Value;
                            }
                            else
                                this.data.Footer[index].ProyectoID.Value = string.Empty;
                            this.GetImpuestos();
                            if (Convert.ToDouble(this.txtValor.EditValue) != 0)
                                this.textEditControl_Leave(this.txtValor, e);
                            #endregion
                            break;
                        case "CentroCostoID":
                            #region Centro Costo
                            if (master.ValidID)
                            {
                                this.data.Footer[index].CentroCostoID.Value = master.Value;
                                DTO_coCentroCosto cto = (DTO_coCentroCosto)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, master.Value, true);
                                this._operacion = cto.OperacionID.Value;
                            }
                            else
                                this.data.Footer[index].CentroCostoID.Value = string.Empty;
                            this.GetImpuestos();
                            if (Convert.ToDouble(this.txtValor.EditValue) != 0)
                                this.textEditControl_Leave(this.txtValor, e);
                            #endregion
                            break;
                        case "LugarGeograficoID":
                            #region Lugar Geografico
                            if (master.ValidID)
                            {
                                DTO_glLugarGeografico lugarGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false, master.Value, true);
                                if (!lugarGeo.DistribuyeInd.Value.Value)
                                {
                                    this.data.Footer[index].LugarGeograficoID.Value = master.Value;
                                    this.GetImpuestos();
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvalidLugarGeo));
                                    master.Focus();
                                }
                            }
                            else
                            {
                                this.data.Footer[index].LugarGeograficoID.Value = string.Empty;
                                this.EnableImpuestos(_declaraIVA, true);
                            }
                            if (Convert.ToDouble(this.txtValor.EditValue) != 0)
                                this.textEditControl_Leave(this.txtValor, e);
                            #endregion
                            break;
                        case "MonedaID":
                            #region Moneda
                            if (master.ValidID)
                                this.data.Footer[index].MonedaID.Value = master.Value;
                            else
                                this.data.Footer[index].MonedaID.Value = string.Empty;
                           #endregion
                            break;
                    }           
                    this.gvDocument.RefreshData();
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    this.newReg = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "masterDetails_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
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
                case "txtNombre":
                    this.data.Footer[index].Nombre.Value = ctrl.Text;
                    break;
                case "txtFactura":
                    this.data.Footer[index].Factura.Value = ctrl.Text;
                    DTO_coConceptoCargo concepto = (DTO_coConceptoCargo)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, false, this._conceptoCargo, true);
                    DTO_coPlanCuenta cta = concepto != null ? (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, concepto.CuentaID.Value, true): null;
                    if (cta != null)
                    {
                        DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cta.ConceptoSaldoID.Value, true);
                        if (saldo.coSaldoControl.Value == (int)SaldoControl.Doc_Externo)
                        {

                            DTO_glDocumentoControl DocCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(AppDocuments.CausarFacturas, this.masterTercero.Value, this.txtFactura.Text);

                            DTO_coComprobante comp = DocCtrl != null ? (DTO_coComprobante)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, DocCtrl.ComprobanteID.Value, true) : null;
                            DTO_coCuentaSaldo saldoDoc = comp != null ? _bc.AdministrationModel.Saldo_GetByDocumento(cta.ID.Value, cta.ConceptoSaldoID.Value, DocCtrl.NumeroDoc.Value.Value, string.Empty) : null;
                            if (saldoDoc != null)
                            {
                                 isDocExterno = true;
                                 TipoMoneda monOrigConcepCargo = (TipoMoneda)cta.OrigenMonetario.Value;

                                 if (monOrigConcepCargo == TipoMoneda.Local) //Trae el valor local si es origen local
                                 {
                                     this.saldoActualDocExt = (saldoDoc.DbOrigenLocML.Value.Value + saldoDoc.DbOrigenExtML.Value.Value + saldoDoc.CrOrigenLocML.Value.Value + saldoDoc.CrOrigenExtML.Value.Value +
                                         saldoDoc.DbSaldoIniLocML.Value.Value + saldoDoc.DbSaldoIniExtML.Value.Value + saldoDoc.CrSaldoIniLocML.Value.Value + saldoDoc.CrSaldoIniExtML.Value.Value) * -1;

                                     this.saldoActualDocExt = Math.Round(this.saldoActualDocExt, 2);
                                 }
                                 else if (monOrigConcepCargo == TipoMoneda.Foreign)  //Trae el valor extranjero si es origen extranjero
                                 {
                                     this.saldoActualDocExt = (saldoDoc.DbOrigenLocME.Value.Value + saldoDoc.DbOrigenExtME.Value.Value + saldoDoc.CrOrigenLocME.Value.Value + saldoDoc.CrOrigenExtME.Value.Value +
                                          saldoDoc.DbSaldoIniLocME.Value.Value + saldoDoc.DbSaldoIniExtME.Value.Value + saldoDoc.CrSaldoIniLocME.Value.Value + saldoDoc.CrSaldoIniExtME.Value.Value);

                                     this.saldoActualDocExt = Math.Round(this.saldoActualDocExt, 2);
                                 }
                            }
                            else
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));
                        }
                    }
                    break;
                case "txtFacturaEqui":
                    this.data.Footer[index].FactEquivalente.Value = ctrl.Text;
                    break;
                case "txtDescripcionCosto":
                    this.data.Footer[index].Descriptivo.Value = ctrl.Text;
                    this.txtValor.Focus();
                    break;
            }

            this.gvDocument.RefreshData();
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            this.newReg = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void textEditControl_Leave(object sender, EventArgs e)
        {
            TextEdit ctrl = (TextEdit)sender;
            int index = this.NumFila;
            decimal vlr = 0;
            decimal PorcImpActual = 0;

            try
            {
                decimal val = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture); //Convert.ToDecimal(ctrl.EditValue);
                if (val >= 0)
                {
                    switch (ctrl.Name)
                    {
                        case "txtValor":
                            #region Valor Bruto
                            if (isDocExterno)
                            {
                                if (val >= this.saldoActualDocExt)
                                {
                                    ctrl.EditValue = this.data.Footer[index].ValorBruto.Value;
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ValueBrutoInvalid));
                                    isDocExterno = false;
                                    return;
                                }
                            }
                            if (val != 0 && this.dtoRegFiscalTer.TipoTercero.Value == 1) // Si es Reg comun
                            {
                                this.data.Footer[index].ValorBruto.Value = val;

                                //Valida el regimen para calcular el IVA
                                if (this.dtoRegFiscalTer != null && this.dtoRegFiscalTer.TipoTercero.Value == 1)// Valida si es Regimen Comun
                                {
                                    this.txtBaseIVA1.EditValue = this.data.Footer[index].ValorBruto.Value.Value;                                 
                                    this.data.Footer[index].BaseIVA1.Value = this.data.Footer[index].ValorBruto.Value.Value;
                                    //Calcula el valor del Iva1
                                    decimal iva = Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApIVA1.EditValue, CultureInfo.InvariantCulture) / 100;
                                    this.data.Footer[index].ValorIVA1.Value = Convert.ToDecimal(Math.Round(iva, 0), CultureInfo.InvariantCulture);
                                    this.txtValorIVA1.EditValue = this.data.Footer[index].ValorIVA1.Value.Value;
                                    
                                    ////Suma de nuevo el valor de Iva1 al Valor Neto
                                    //this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA1.Value;
                                    //this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                  
                                else
                                    this.txtBaseIVA1.EditValue = 0;

                                //ImpConsumo
                                this.txtBaseConsumo.EditValue = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].BaseImpConsumo.Value = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].PorImpConsumo.Value = Convert.ToDecimal(this.txtPorConsumo.EditValue, CultureInfo.InvariantCulture);

                                vlr = Convert.ToDecimal(this.txtBaseConsumo.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorConsumo.EditValue, CultureInfo.InvariantCulture) / 100;
                                this.data.Footer[index].ValorImpConsumo.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorConsumo.EditValue = this.data.Footer[index].ValorImpConsumo.Value;

                                this.data.Footer[index].BaseIVA1.Value = Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseIVA2.Value = Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseRteFuente.Value = Convert.ToDecimal(this.txtBaseRfte.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseRteIVA1.Value = Convert.ToDecimal(this.txtBaseRIva.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseRteIVA2.Value = Convert.ToDecimal(this.txtBaseRIva2.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseRteICA.Value = Convert.ToDecimal(this.txtBaseRIca.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].BaseImpConsumo.Value = Convert.ToDecimal(this.txtBaseConsumo.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorIVA1.Value = Convert.ToDecimal(this.txtValorIVA1.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorIVA2.Value = Convert.ToDecimal(this.txtValorIVA2.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorRteFuente.Value = Convert.ToDecimal(this.txtValorRfte.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorRteIVA1.Value = Convert.ToDecimal(this.txtValorRIva.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorRteIVA2.Value = Convert.ToDecimal(this.txtValorRIva2.EditValue, CultureInfo.InvariantCulture);
                                this.data.Footer[index].ValorRteICA.Value = Convert.ToDecimal(this.txtValorRIca.EditValue, CultureInfo.InvariantCulture);
                                //this.data.Footer[index].ValorImpConsumo.Value = Convert.ToDecimal(this.txtValorConsumo.EditValue, CultureInfo.InvariantCulture);

                                //Asigna valor neto = VB + IVAs +ImpConsumo - Retenciones 
                                this.data.Footer[index].ValorNeto.Value = this.data.Footer[index].ValorBruto.Value.Value;
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA1.Value;
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA2.Value;
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorImpConsumo.Value;

                                //RefeFuente
                                this.txtBaseRfte.EditValue = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].BaseRteFuente.Value = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].PorRteFuente.Value = Convert.ToDecimal(this.txtPorcApRfte.EditValue, CultureInfo.InvariantCulture);
                                vlr = Convert.ToDecimal(this.txtBaseRfte.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRfte.EditValue, CultureInfo.InvariantCulture) / 100;
                                vlr = this.data.Footer[index].ValorBruto.Value < this.data.Footer[index].MontoMinimo.Value ? 0 : vlr;
                                this.data.Footer[index].ValorRteFuente.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorRfte.EditValue = this.data.Footer[index].ValorRteFuente.Value;
                                if (!this.chkRfte.Checked)
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteFuente.Value;

                                //ReteIVA1
                                this.txtBaseRIva.EditValue = this.data.Footer[index].BaseIVA1.Value;
                                this.data.Footer[index].BaseRteIVA1.Value = this.data.Footer[index].BaseIVA1.Value;
                                this.data.Footer[index].PorRteIVA1.Value = Convert.ToDecimal(this.txtPorcApRIva.EditValue, CultureInfo.InvariantCulture);
                                this.txtPorcApRIva.EditValue = this.txtPorcApRIva.EditValue;

                                vlr = Convert.ToDecimal(this.txtBaseRIva.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRIva.EditValue, CultureInfo.InvariantCulture) / 100;
                                this.data.Footer[index].ValorRteIVA1.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorRIva.EditValue = this.data.Footer[index].ValorRteIVA1.Value;
                                if (!this.chkRIva.Checked)
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA1.Value;

                                //ReteIVA2
                                this.txtBaseRIva2.EditValue = this.data.Footer[index].BaseIVA2.Value;
                                this.data.Footer[index].BaseRteIVA2.Value = this.data.Footer[index].BaseIVA2.Value;
                                this.data.Footer[index].PorRteIVA2.Value = Convert.ToDecimal(this.txtPorcApRIva2.EditValue, CultureInfo.InvariantCulture);
                                this.txtPorcApRIva2.EditValue = this.txtPorcApRIva2.EditValue;

                                vlr = Convert.ToDecimal(this.txtBaseRIva2.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRIva2.EditValue, CultureInfo.InvariantCulture) / 100;
                                this.data.Footer[index].ValorRteIVA2.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorRIva2.EditValue = this.data.Footer[index].ValorRteIVA2.Value;
                                if (!this.chkRIva2.Checked)
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA2.Value;
                                
                                //ReteICA
                                this.txtBaseRIca.EditValue = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].BaseRteICA.Value = this.data.Footer[index].ValorBruto.Value;
                                this.data.Footer[index].PorRteICA.Value = Convert.ToDecimal(this.txtPorcApRIca.EditValue, CultureInfo.InvariantCulture);

                                vlr = Convert.ToDecimal(this.txtBaseRIca.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRIca.EditValue, CultureInfo.InvariantCulture) / 100;
                                this.data.Footer[index].ValorRteICA.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorRIca.EditValue = this.data.Footer[index].ValorRteICA.Value;
                                if (!this.chkRIca.Checked)
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteICA.Value;
                                
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                             

                                //SumIVA
                                this.data.Footer[index].SumIVA.Value = this.data.Footer[index].ValorIVA1.Value + this.data.Footer[index].ValorIVA2.Value;
                                
                                //SumRete
                                this.data.Footer[index].SumRete.Value = this.data.Footer[index].ValorRteFuente.Value + this.data.Footer[index].ValorRteIVA1.Value + this.data.Footer[index].ValorRteIVA2.Value + this.data.Footer[index].ValorRteICA.Value;
                                this.chkRIca.Enabled = true;
                                this.chkRfte.Enabled = false;
                            }
                            else if (this.dtoRegFiscalTer.TipoTercero.Value == 2) //Si es Simplificado
                            {
                                this.data.Footer[index].ValorBruto.Value = val;
                                decimal baseIVA  = Math.Round(val/(1+(this.data.Footer[index].PorIVA1.Value.Value/100)),0);

                                vlr = baseIVA * Convert.ToDecimal(this.data.Footer[index].PorIVA1.Value, CultureInfo.InvariantCulture) / 100;
                                this.data.Footer[index].ValorIVA1.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                this.txtValorIVA1.EditValue = this.data.Footer[index].ValorIVA1.Value;
                                
                                vlr = (decimal)(this.data.Footer[index].ValorBruto.Value - this.data.Footer[index].ValorIVA1.Value);
                                this.data.Footer[index].BaseIVA1.Value = baseIVA;
                                this.txtBaseIVA1.EditValue = this.data.Footer[index].BaseIVA1.Value.Value;//this.data.Footer[index].BaseIVA1.Value.Value 

                                this.data.Footer[index].PorRteIVA1.Value = this.data.Footer[index].PorIVA1.Value;
                                this.txtPorcApRIva.EditValue = this.data.Footer[index].PorRteIVA1.Value;
                                this.data.Footer[index].BaseRteIVA1.Value = this.data.Footer[index].BaseIVA1.Value;
                                this.txtBaseRIva.EditValue = this.data.Footer[index].BaseRteIVA1.Value.Value;//this.data.Footer[index].BaseRteIVA1.Value.ToString();
                                
                                this.data.Footer[index].ValorRteIVA1.Value = this.data.Footer[index].ValorIVA1.Value;
                                this.txtValorRIva.EditValue = this.data.Footer[index].ValorRteIVA1.Value;
                                this.data.Footer[index].ValorNeto.Value = this.data.Footer[index].ValorBruto.Value.Value + this.data.Footer[index].ValorIVA1.Value - this.data.Footer[index].ValorRteIVA1.Value;
                                
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;

                                if (this.data.Footer[index].PorRteFuente.Value != 0 && this.data.Footer[index].PorRteFuente.Value != null)
                                {
                                    this.data.Footer[index].BaseRteFuente.Value = baseIVA;
                                    this.txtBaseRfte.EditValue = this.data.Footer[index].BaseRteFuente.Value;
                                    
                                    vlr = (decimal)(this.data.Footer[index].BaseRteFuente.Value * this.data.Footer[index].PorRteFuente.Value) / 100;
                                    vlr = this.data.Footer[index].ValorBruto.Value < this.data.Footer[index].MontoMinimo.Value ? 0 : vlr;
                                    this.data.Footer[index].ValorRteFuente.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorRfte.EditValue = this.data.Footer[index].ValorRteFuente.Value;
                                    this.data.Footer[index].RteFteAsumidoInd.Value = true;
                                    this.chkRfte.Checked = true;
                                    this.chkRfte.Enabled = false;
                                }
                                if (this.data.Footer[index].PorRteICA.Value != 0 && this.data.Footer[index].PorRteICA.Value != null)
                                {
                                    this.data.Footer[index].BaseRteICA.Value = baseIVA;
                                    this.txtBaseRIca.EditValue = this.data.Footer[index].BaseRteICA.Value;
                                   
                                   
                                    vlr = (decimal)(this.data.Footer[index].BaseRteICA.Value * this.data.Footer[index].PorRteICA.Value) / 100;
                                    this.data.Footer[index].ValorRteICA.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorRIca.EditValue = this.data.Footer[index].ValorRteICA.Value;
                                    this.data.Footer[index].RteICAAsumidoInd.Value = true;
                                    this.chkRIca.Checked = true;
                                    this.chkRIca.Enabled = false;
                                }
                                //SumIVA
                                this.data.Footer[index].SumIVA.Value = this.data.Footer[index].ValorIVA1.Value;
                                //SumRete
                                this.data.Footer[index].SumRete.Value = this.data.Footer[index].ValorRteFuente.Value + this.data.Footer[index].ValorRteIVA1.Value + this.data.Footer[index].ValorRteIVA2.Value + this.data.Footer[index].ValorRteICA.Value;
                            }

                            #endregion
                            break;
                        case "txtBaseIVA1":
                            #region Base IVA 1
                            if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                vlr = Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture) + Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture);
                                if (vlr <= Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture))
                                {
                                    //Resta el valor del Iva1 para actualizar el valor Neto 
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorIVA1.Value;
                                    //Calcula el valor del Iva1
                                    this.data.Footer[index].BaseIVA1.Value = val;
                                    vlr = Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApIVA1.EditValue, CultureInfo.InvariantCulture) / 100;
                                    this.data.Footer[index].ValorIVA1.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorIVA1.EditValue = this.data.Footer[index].ValorIVA1.Value.Value;
                                    
                                    //Suma de nuevo el valor de Iva1 al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA1.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                    
                                    //ReteIVA1
                                    this.txtBaseRIva.EditValue = this.data.Footer[index].BaseIVA1.Value;
                                    this.data.Footer[index].BaseRteIVA1.Value = this.data.Footer[index].BaseIVA1.Value;
                                    this.data.Footer[index].PorRteIVA1.Value = Convert.ToDecimal(this.txtPorcApRIva.EditValue, CultureInfo.InvariantCulture);

                                    vlr = Convert.ToDecimal(this.txtBaseRIva.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRIva.EditValue, CultureInfo.InvariantCulture) / 100;
                                    this.data.Footer[index].ValorRteIVA1.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorRIva.EditValue = this.data.Footer[index].ValorRteIVA1.Value;
                                    if (!this.chkRIva.Checked)
                                        this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA1.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ValueBaseIVAInvalid));
                                    ctrl.EditValue = this.data.Footer[index].BaseIVA1.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtBaseIVA2":
                            #region Base IVA 2
                            if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                vlr = Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture) + Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture);
                                if (vlr <= Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture))
                                {
                                    //Resta el valor del Iva2 para actualizar el valor Neto 
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorIVA2.Value;
                                    
                                    //Calcula el valor del Iva2
                                    this.data.Footer[index].BaseIVA2.Value = val;
                                    vlr = Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApIVA2.EditValue, CultureInfo.InvariantCulture) / 100;
                                    this.data.Footer[index].ValorIVA2.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorIVA2.EditValue = this.data.Footer[index].ValorIVA2.Value.Value;
                                    
                                    //Suma de nuevo el valor de Iva2 al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA2.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                    
                                    //ReteIVA2
                                    this.txtBaseRIva2.EditValue = this.data.Footer[index].BaseIVA2.Value;
                                    this.data.Footer[index].BaseRteIVA2.Value = this.data.Footer[index].BaseIVA2.Value;
                                    this.data.Footer[index].PorRteIVA2.Value = Convert.ToDecimal(this.txtPorcApRIva2.EditValue, CultureInfo.InvariantCulture);

                                    vlr = Convert.ToDecimal(this.txtBaseRIva2.EditValue, CultureInfo.InvariantCulture) * Convert.ToDecimal(this.txtPorcApRIva2.EditValue, CultureInfo.InvariantCulture) / 100;
                                    this.data.Footer[index].ValorRteIVA2.Value = Convert.ToDecimal(Math.Round(vlr, 0), CultureInfo.InvariantCulture);
                                    this.txtValorRIva2.EditValue = this.data.Footer[index].ValorRteIVA2.Value;
                                    if (!this.chkRIva2.Checked)
                                        this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA2.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ValueBaseIVAInvalid));
                                    ctrl.EditValue = this.data.Footer[index].BaseIVA2.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorIVA1":
                            #region Valor IVA 1
                            if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del IVA1
                                if (this._declaraIVA)
                                    vlr = Convert.ToDecimal(this.txtValorIVA1.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseIVA1.EditValue, CultureInfo.InvariantCulture);
                                else
                                    vlr = Convert.ToDecimal(this.txtValorIVA1.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorIVA1.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Resta el valor del Iva1 para actualizar el valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorIVA1.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorIVA1.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    
                                    //Suma de nuevo el valor de Iva1 al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA1.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorIVA1.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorIVA2":
                            #region Valor IVA 2
                            if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del IVA2
                                vlr = Convert.ToDecimal(this.txtValorIVA2.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseIVA2.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorIVA2.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Resta el valor del Iva2 para actualizar el valor Neto 
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorIVA2.Value;
                                    vlr = val;

                                    this.data.Footer[index].ValorIVA2.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    //Suma de nuevo el valor de Iva2 al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorIVA2.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorIVA2.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorRfte":
                            #region Valor Rfte
                            if (Convert.ToDecimal(this.txtBaseRfte.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor de la Rfte
                                vlr = Convert.ToDecimal(this.txtValorRfte.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseRfte.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorRteFuente.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual && this.data.Footer[index].ValorBruto.Value < this.data.Footer[index].MontoMinimo.Value)
                                {
                                    //Suma el valor de la Rfte para actualizar el valor Neto 
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteFuente.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorRteFuente.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    
                                    //Resta de nuevo el valor de la Rfte al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteFuente.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorRteFuente.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorRIva":
                            #region Valor RIVA
                            if (Convert.ToDecimal(this.txtBaseRIva.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del RIva
                                vlr = Convert.ToDecimal(this.txtValorRIva.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseRIva.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorRteIVA1.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Suma el valor del RIva para actualizar el valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteIVA1.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorRteIVA1.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    
                                    //Resta de nuevo el valor del RIva al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA1.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorRteIVA1.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorRIva2":
                            #region Valor RIVA2
                            if (Convert.ToDecimal(this.txtBaseRIva2.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del RIva2
                                vlr = Convert.ToDecimal(this.txtValorRIva2.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseRIva2.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorRteIVA2.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Suma el valor del RIva2 para actualizar el valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteIVA2.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorRteIVA2.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    
                                    //Resta de nuevo el valor del RIva2 al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA2.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorRteIVA2.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorRIca":
                            #region Valor RICA
                            if (Convert.ToDecimal(this.txtBaseRIca.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del RIca
                                vlr = Convert.ToDecimal(this.txtValorRIca.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseRIca.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorRteICA.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Suma el valor del RIca para actualizar el valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteICA.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorRteICA.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);
                                    
                                    //Resta de nuevo el valor del RIca al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteICA.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorRteICA.Value;
                                }
                            }
                            #endregion
                            break;
                        case "txtValorConsumo":
                            #region Valor Imp Consumo
                            if (Convert.ToDecimal(this.txtBaseConsumo.EditValue, CultureInfo.InvariantCulture) != 0)
                            {
                                //Verifica que el porcentaje del impuesto siga siendo válido para el nuevo valor del Consumo
                                vlr = Convert.ToDecimal(this.txtValorConsumo.EditValue, CultureInfo.InvariantCulture) * 100 / Convert.ToDecimal(this.txtBaseConsumo.EditValue, CultureInfo.InvariantCulture);
                                vlr = Math.Round(vlr, 0);
                                PorcImpActual = (decimal)Math.Truncate(this.data.Footer[index].PorImpConsumo.Value.Value * 10) / 10;

                                if (vlr == PorcImpActual)
                                {
                                    //Resta el valor del IpConsumo para actualizar el valor Neto
                                    this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorImpConsumo.Value;
                                    vlr = val;
                                    this.data.Footer[index].ValorImpConsumo.Value = Convert.ToDecimal(vlr, CultureInfo.InvariantCulture);

                                    //Suma de nuevo el valor de IpConsumo al Valor Neto
                                    this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorImpConsumo.Value;
                                    this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                                }
                                else
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_CajaMenorValueOut));
                                    ctrl.EditValue = this.data.Footer[index].ValorImpConsumo.Value;
                                }
                            }
                            #endregion
                            break;
                    }
                    this.gvDocument.RefreshData();
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    this.newReg = true;
                }
                else
                {
                    ctrl.EditValue = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) * -1;
                    ctrl.Focus();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentLegalizaForm.cs", "txtEditControl_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dateControl_Leave(object sender, EventArgs e)
        {
            DateEdit ctrl = (DateEdit)sender;
            GridColumn col = new GridColumn();
            int index = this.NumFila;

            switch (ctrl.Name)
            {
                case "dtFechaDetalle":
                    this.data.Footer[index].Fecha.Value = ctrl.DateTime;
                    break;
            }
            int monOr = (int)this._tipoMonedaOr;
            this.txtTasaCambioDoc.EditValue = this.LoadTasaCambio(monOr, ctrl.DateTime);
            this.gvDocument.RefreshData();
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            this.newReg = true;
        }

        /// <summary>
        /// Cuando cambia el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkControl_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit ctrl = (CheckEdit)sender;
            int index = this.NumFila;
            if (ctrl.Focus())
            {
                switch (ctrl.Name)
                {
                    case "chkRfte":
                        #region Valor RFte
                        if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && this.chkRfte.Capture)
                        {
                            this.data.Footer[index].RteFteAsumidoInd.Value = ctrl.Checked;
                            if (this.data.Footer[index].RteFteAsumidoInd.Value.Value)
                            {
                                //Suma el valor de la Rfte para actualizar el valor Neto 
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteFuente.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                            else
                            {
                                //Resta de nuevo el valor de la Rfte al Valor Neto
                                this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteFuente.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                        }
                        #endregion
                        break;
                    case "chkRIva":
                        #region Valor RIva
                        if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && this.chkRIva.Capture)
                        {
                            this.data.Footer[index].RteIVA1AsumidoInd.Value = ctrl.Checked;
                            if (this.data.Footer[index].RteIVA1AsumidoInd.Value.Value)
                            {
                                //Suma el valor del RIva para actualizar el valor Neto
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteIVA1.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                            else
                            {
                                //Resta de nuevo el valor del RIva al Valor Neto
                                this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA1.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                        }
                        #endregion
                        break;
                    case "chkRIva2":
                        #region Valor RIva2
                        if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && this.chkRIva2.Capture)
                        {
                            this.data.Footer[index].RteIVA2AsumidoInd.Value = ctrl.Checked;
                            if (this.data.Footer[index].RteIVA2AsumidoInd.Value.Value)
                            {
                                //Suma el valor del RIva para actualizar el valor Neto
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteIVA2.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                            else
                            {
                                //Resta de nuevo el valor del RIva al Valor Neto
                                this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteIVA2.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                        }
                        #endregion
                        break;
                    case "chkRIca":
                        #region Valor RIca
                        if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) != 0 && this.chkRIca.Capture)
                        {
                            this.data.Footer[index].RteICAAsumidoInd.Value = ctrl.Checked;
                            if (this.data.Footer[index].RteICAAsumidoInd.Value.Value)
                            {
                                //Resta el valor del RIca para actualizar el valor Neto
                                this.data.Footer[index].ValorNeto.Value += this.data.Footer[index].ValorRteICA.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                            else
                            {
                                //Suma de nuevo el valor del RIca al Valor Neto
                                this.data.Footer[index].ValorNeto.Value -= this.data.Footer[index].ValorRteICA.Value;
                                this.txtNeto.EditValue = this.data.Footer[index].ValorNeto.Value.Value;
                            }
                        }
                        #endregion
                        break;
                }

                this.gvDocument.RefreshData();
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                this.newReg = true;
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
                        this.isValid = true;
                        this._cargo = new DTO_cpCargoEspecial();
                        this._tercero = new DTO_coTercero();

                        this.validHeader = false;
                        this.ValidHeaderTB();
                    }
                }

                if (this.cleanDoc)
                {
                    this.newDoc = true;
                    this.deleteOP = true;

                    this.EnableFooter(false);
                    this.masterCargoEspecial.Value = string.Empty;
                    //this.newDoc = false;

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
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            #region Genera Reporte
            if (this.data != null && this.data.DocCtrl.NumeroDoc.Value.HasValue && this.data.DocCtrl.NumeroDoc.Value != 0)
            {
                string reportName = this._bc.AdministrationModel.Report_Cp_CajaMenor(this.data.DocCtrl.NumeroDoc.Value.Value, string.Empty, null, true);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this.data.DocCtrl.NumeroDoc.Value.Value, null, reportName.ToString());
                Process.Start(fileURl);
            }

            #endregion
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
                    mq.SetFK("TerceroID", AppMasters.coTercero, _bc.CreateFKConfig(AppMasters.coTercero));
                    mq.SetFK("ProyectoID", AppMasters.coProyecto, _bc.CreateFKConfig(AppMasters.coProyecto));
                    mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, _bc.CreateFKConfig(AppMasters.coCentroCosto));
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
                    _bc.AdministrationModel.DataCopied = this.data.Footer;
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
                //this.data.Footer = compDet;
                this.LoadData(true);
                this.UpdateTemp(this.data);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
