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
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using System.Runtime.Serialization;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentOrdenCompForm : DocumentForm
    {
        public DocumentOrdenCompForm()
        {
           // InitializeComponent();
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
        #endregion

        #region Variables privadas
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Variables para gvCargos
        private string unboundPrefixCargo = "UnboundPref_";
        private string _codBienServicio = string.Empty;
        //Variables con los recursos de las Fks
        private string _codigoRsx = string.Empty;
        private string _referenciaRsx = string.Empty;
        private string _parametro1Rsx = string.Empty;
        private string _parametro2Rsx = string.Empty;
        private string _unidadRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;

        //Variables con otros recursos
        private string _cantidadSolRsx = string.Empty;
        private string _cantidadOCRsx = string.Empty;
        private string _porcentajeRsx = string.Empty;
        private string _descRsx = string.Empty;
        private string _solCargosRsx = string.Empty;
        protected byte _tipoReporte = 1;

        Dictionary<string, Tuple<DTO_prBienServicio, decimal>> cacheBienServ = new Dictionary<string, Tuple<DTO_prBienServicio, decimal>>();
        Dictionary<string, decimal> cacheBienServClase = new Dictionary<string, decimal>();
        Dictionary<string, decimal> cacheConCargo = new Dictionary<string, decimal>();
        Dictionary<string, decimal> cacheCuenta = new Dictionary<string, decimal>();
        #endregion

        #region Variables Protected
        //Variables formulario
        protected DTO_prOrdenCompra data = null;
        protected List<DTO_prOrdenCompraFooter> footerAIU = null;
        //Variables Moneda
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        protected string monedaOrden;
        protected bool biMoneda = false;
        protected TipoMoneda _tipoMonedaOr = TipoMoneda.Local;
        protected decimal valorTotalDoc = 0;
        protected decimal valorIVATotalDoc = 0; 
        protected int _daysEntr = 0;
        protected decimal valorTotalPagosMes = 0;
        protected decimal _vlrTasaCambio = 0;

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
        protected string defLocFisica = string.Empty;
        protected string defAreaFunc = string.Empty;

        //variables para funciones particulares
        protected bool cleanDoc = true;
        protected List<DTO_prSolicitudResumen> _solicitudes;
        protected DTO_prOrdenCompraFooter _currentRow = new DTO_prOrdenCompraFooter();
        #endregion

        #region Propiedades

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this.data.Footer.FindIndex(det => det.DetalleDocu.Index == this.indexFila);
            }
        }     
        
        //BienServicio

        private DTO_prBienServicio _bienServ = null;
        private DTO_prBienServicio BienServ
        {
            get
            {
                return this._bienServ;
            }
            set
            {
                this._bienServ = value;

                if (value != null)
                {                              
                    #region Variables
                    DTO_glBienServicioClase _bienServClase;
                    DTO_coPlanCuenta _cuenta;

                    DTO_glConsulta consulta;
                    List<DTO_glConsultaFiltro> filtros;
                    DTO_glConsultaFiltro filtro;
                    #endregion
                    #region Trae BienServicio de la solicitud
                    try
                    {
                        if (cacheBienServClase.ContainsKey(_bienServ.ClaseBSID.Value))
                            cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, cacheBienServClase[_bienServ.ClaseBSID.Value]));
                        else
                        {
                            _bienServClase = (DTO_glBienServicioClase)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, _bienServ.ClaseBSID.Value, true);
                            if (cacheConCargo.ContainsKey(_bienServClase.ConceptoCargoID.Value))
                            {
                                cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, cacheConCargo[_bienServClase.ConceptoCargoID.Value]));
                                cacheBienServClase.Add(_bienServ.ClaseBSID.Value, cacheConCargo[_bienServClase.ConceptoCargoID.Value]);
                            }
                            else
                            {
                                decimal iva = 0;
                                string proveedor = this.documentID == AppDocuments.OrdenCompra ? this.data.HeaderOrdenCompra.ProveedorID.Value : this.data.HeaderContrato.ProveedorID.Value;
                                DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, proveedor, true);
                                DTO_coTercero terc = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, prov.TerceroID.Value, true);
                                #region Consulta para la tabla de los impuestos
                                consulta = new DTO_glConsulta();
                                filtros = new List<DTO_glConsultaFiltro>();

                                filtro = new DTO_glConsultaFiltro();
                                filtro.CampoFisico = "ImpuestoTipoID";
                                filtro.ValorFiltro = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                                filtro.OperadorFiltro = OperadorFiltro.Igual;
                                filtro.OperadorSentencia = "and";
                                filtros.Add(filtro);

                                filtro = new DTO_glConsultaFiltro();
                                filtro.CampoFisico = "ConceptoCargoID";
                                filtro.ValorFiltro = _bienServClase.ConceptoCargoID.Value;
                                filtro.OperadorFiltro = OperadorFiltro.Igual;
                                filtro.OperadorSentencia = "and";
                                filtros.Add(filtro);

                                if (terc != null)
                                {
                                    filtro = new DTO_glConsultaFiltro();
                                    filtro.CampoFisico = "RegimenFiscalTerceroID";
                                    filtro.ValorFiltro = terc.ReferenciaID.Value;
                                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                                    filtro.OperadorSentencia = "and";
                                    filtros.Add(filtro); 
                                }

                                consulta.Filtros = filtros;
                                #endregion
                                long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                                if (count > 0)
                                {
                                    var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null);
                                    List<DTO_coImpuesto> mastercoImp = listCoImp.Cast<DTO_coImpuesto>().ToList();

                                    #region Trae la cuenta
                                    if (cacheCuenta.ContainsKey(mastercoImp[0].CuentaID.Value))
                                    {
                                        iva = cacheCuenta[mastercoImp[0].CuentaID.Value];
                                    }
                                    else
                                    {
                                        _cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, mastercoImp[0].CuentaID.Value, true);
                                        if (!string.IsNullOrEmpty(_cuenta.ImpuestoPorc.Value.ToString()))
                                        {
                                            iva = _cuenta.ImpuestoPorc.Value.Value;
                                            cacheCuenta.Add(_cuenta.ID.Value, iva);
                                        }
                                    }
                                    #endregion
                                }
                                cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, iva));
                                cacheBienServClase.Add(_bienServ.ClaseBSID.Value, iva);
                                cacheConCargo.Add(_bienServClase.ConceptoCargoID.Value, iva);
                            }
                        }
                    }
                    catch (Exception ex)
                    {                        
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Co_CtaIVATarifa));
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region Funciones Privadas y protected

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddCargosGridCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCargo + "ProyectoID";
                proyecto.Caption = this._proyectoRsx;
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 80;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvCargos.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCargo + "CentroCostoID";
                ctoCosto.Caption = this._centroCostoRsx;
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 80;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = false;
                this.gvCargos.Columns.Add(ctoCosto);

                //LineaPresupuestoID
                GridColumn LineaPresupuestoID = new GridColumn();
                LineaPresupuestoID.FieldName = this.unboundPrefixCargo + "LineaPresupuestoID";
                LineaPresupuestoID.Caption ="Línea Presupuesto";
                LineaPresupuestoID.UnboundType = UnboundColumnType.String;
                LineaPresupuestoID.VisibleIndex = 3;
                LineaPresupuestoID.Width = 80;
                LineaPresupuestoID.Visible = true;
                LineaPresupuestoID.OptionsColumn.AllowEdit = false;
                this.gvCargos.Columns.Add(LineaPresupuestoID);

                //Porcentaje
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCargo + "PorcentajeID";
                percent.Caption = this._porcentajeRsx;
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.VisibleIndex = 4;
                percent.Width = 80;
                percent.Visible = true;
                percent.OptionsColumn.AllowEdit = false;
                this.gvCargos.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCargo + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvCargos.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCargo + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvCargos.Columns.Add(consDeta);

                //Indice de la fila de la grilla de los cargos
                GridColumn cargoColIndex = new GridColumn();
                cargoColIndex.FieldName = this.unboundPrefixCargo + "Index";
                cargoColIndex.UnboundType = UnboundColumnType.Integer;
                cargoColIndex.Visible = false;
                this.gvCargos.Columns.Add(cargoColIndex);

                //Indice de la fila la grilla principal
                GridColumn detColIndex = new GridColumn();
                detColIndex.FieldName = this.unboundPrefixCargo + "IndexDet";
                detColIndex.UnboundType = UnboundColumnType.Integer;
                detColIndex.Visible = false;
                this.gvCargos.Columns.Add(detColIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "AddCargosGridCols"));
            }
        }

        /// <summary>
        /// Calcula valores de AIU
        /// </summary>
        protected List<DTO_prOrdenCompraFooter> GetValuesAIU(DTO_prOrdenCompraFooter det, int count)
        {
            List<DTO_prOrdenCompraFooter> cargosNoIncluidos = null;
            DTO_prOrdenCompraFooter newCargo = null;

            try
            {
                decimal cantFinal = 0;
                if (string.IsNullOrEmpty(det.DetalleDocu.inReferenciaID.Value))
                    cantFinal = det.DetalleDocu.CantidadOC.Value.Value;
                else
                    cantFinal = (det.DetalleDocu.UnidadInvID.Value.Equals(det.DetalleDocu.EmpaqueInvID.Value) ? det.DetalleDocu.CantidadOC.Value.Value : det.DetalleDocu.CantEmpaque.Value.Value);

                decimal baseAIU  = det.DetalleDocu.ValorUni.Value.Value * cantFinal;

                det.DetalleDocu.ValorBaseAIU.Value = baseAIU;

                decimal porcA = (!string.IsNullOrEmpty(det.DetalleDocu.CodigoAdminAIU.Value.Trim())) ?
                    this.data.HeaderOrdenCompra.PorcentAdministra.Value.Value / 100 : 0;
                decimal porcI = (!string.IsNullOrEmpty(det.DetalleDocu.CodigoImprevAIU.Value.Trim())) ?
                    this.data.HeaderOrdenCompra.Porcentimprevisto.Value.Value / 100 : 0;
                decimal porcU = (!string.IsNullOrEmpty(det.DetalleDocu.CodigoUtilidadAIU.Value.Trim())) ?
                    this.data.HeaderOrdenCompra.PorcentUtilidad.Value.Value / 100 : 0;

                decimal vlrUni = det.DetalleDocu.ValorUni.Value.Value;
                decimal vlrTot = det.DetalleDocu.ValorUni.Value.Value * cantFinal;
                decimal vlrA = baseAIU * porcA;
                decimal vlrI = baseAIU * porcI;
                decimal vlrU = baseAIU * porcU;
                decimal vlrAIU = vlrA + vlrI + vlrU;

                decimal porcIva = det.DetalleDocu.PorcentajeIVA.Value.Value / 100;
                decimal porcIvaA = det.DetalleDocu.PorIVAAdminAIU.Value.Value / 100;
                decimal porcIvaI = det.DetalleDocu.PorIVAImprevAIU.Value.Value / 100;
                decimal porcIvaU = det.DetalleDocu.PorIVAUtilidadAIU.Value.Value / 100;
                decimal porcIvaAIU = porcIvaA + porcIvaI + porcIvaU;

                decimal ivaUni = vlrUni * porcIva;
                decimal ivaTot = vlrTot * porcIva;
                decimal ivaA = vlrA * porcIvaA;
                decimal ivaI = vlrI * porcIvaI;
                decimal ivaU = vlrU * porcIvaU;
                decimal ivaAIU = ivaA + ivaI + ivaU;

                det.DetalleDocu.IVAUni.Value = ivaUni;

                det.DetalleDocu.IVAAdminAIU.Value = ivaA;
                det.DetalleDocu.IVAImprevAIU.Value = ivaI;
                det.DetalleDocu.IVAUtilidadAIU.Value = ivaU;
                det.DetalleDocu.VlrIVAAIU.Value = ivaAIU;

                if (this.monedaLocal.Equals(this.monedaOrden))
                {
                    det.DetalleDocu.IvaTotML.Value = Math.Round(ivaTot,0);
                    det.DetalleDocu.ValorTotML.Value = Math.Round((vlrTot + (!this.data.HeaderOrdenCompra.IncluyeAUICosto.Value.Value ? vlrAIU : 0)),2);
                    det.DetalleDocu.ValorTotME.Value = 0;
                    det.DetalleDocu.IvaTotME.Value = 0;
                }
                else
                {
                    det.DetalleDocu.IvaTotME.Value = ivaTot;
                    det.DetalleDocu.ValorTotME.Value = Math.Round((vlrTot + (!this.data.HeaderOrdenCompra.IncluyeAUICosto.Value.Value ? vlrAIU : 0)),2);
                    det.DetalleDocu.ValorTotML.Value = 0;
                    det.DetalleDocu.IvaTotML.Value = 0;
                }
                det.DetalleDocu.ValorAdminAIU.Value = vlrA;
                det.DetalleDocu.ValorImprevAIU.Value = vlrI;
                det.DetalleDocu.ValorUtilidadAIU.Value = vlrU;
                det.DetalleDocu.ValorAIU.Value = vlrAIU;

                if (!this.data.HeaderOrdenCompra.IncluyeAUICosto.Value.Value)
                {
                    //index = this._cargosNoIncluidos.Count;
                    cargosNoIncluidos = new List<DTO_prOrdenCompraFooter>();

                    #region Llena paramentos particulares del footer que corresponden a AIU
                    #region Administracion
                    if (!string.IsNullOrEmpty(det.DetalleDocu.CodigoAdminAIU.Value))
                    {
                        newCargo = new DTO_prOrdenCompraFooter();
                        newCargo.DetalleDocu.Index = count;

                        newCargo.DetalleDocu.CodigoBSID.Value = det.DetalleDocu.CodigoAdminAIU.Value;
                        newCargo.DetalleDocu.ValorUni.Value = det.DetalleDocu.ValorAdminAIU.Value.Value;
                        newCargo.DetalleDocu.PorcentajeIVA.Value = det.DetalleDocu.PorIVAAdminAIU.Value.Value;
                        newCargo.DetalleDocu.IVAUni.Value = det.DetalleDocu.IVAAdminAIU.Value.Value;

                        cargosNoIncluidos.Add(newCargo);
                        count++;
                    }
                    #endregion
                    #region Imprevistos
                    if (!string.IsNullOrEmpty(det.DetalleDocu.CodigoImprevAIU.Value))
                    {
                        newCargo = new DTO_prOrdenCompraFooter();
                        newCargo.DetalleDocu.Index = count;

                        newCargo.DetalleDocu.CodigoBSID.Value = det.DetalleDocu.CodigoImprevAIU.Value;
                        newCargo.DetalleDocu.ValorUni.Value = det.DetalleDocu.ValorImprevAIU.Value.Value;
                        newCargo.DetalleDocu.PorcentajeIVA.Value = det.DetalleDocu.PorIVAImprevAIU.Value.Value;
                        newCargo.DetalleDocu.IVAUni.Value = det.DetalleDocu.IVAImprevAIU.Value.Value;

                        cargosNoIncluidos.Add(newCargo);
                        count++;
                    }
                    #endregion
                    #region Utilidad
                    if (!string.IsNullOrEmpty(det.DetalleDocu.CodigoUtilidadAIU.Value))
                    {
                        newCargo = new DTO_prOrdenCompraFooter();
                        newCargo.DetalleDocu.Index = count;

                        newCargo.DetalleDocu.CodigoBSID.Value = det.DetalleDocu.CodigoUtilidadAIU.Value;
                        newCargo.DetalleDocu.ValorUni.Value = det.DetalleDocu.ValorUtilidadAIU.Value.Value;
                        newCargo.DetalleDocu.PorcentajeIVA.Value = det.DetalleDocu.PorIVAUtilidadAIU.Value.Value;
                        newCargo.DetalleDocu.IVAUni.Value = det.DetalleDocu.IVAUtilidadAIU.Value.Value;

                        cargosNoIncluidos.Add(newCargo);
                        count++;
                    }
                    #endregion
                    #endregion

                    if (cargosNoIncluidos.Count > 0)
                    {
                        #region Llena otros paramentos del footer
                        foreach (DTO_prOrdenCompraFooter cargo in cargosNoIncluidos)
                        {
                            if (!string.IsNullOrEmpty(det.DetalleDocu.CodigoAdminAIU.Value))
                            {
                                cargo.DetalleDocu.EmpresaID.Value = this.data.HeaderOrdenCompra.EmpresaID.Value;
                                cargo.DetalleDocu.EstadoInv.Value = 1;
                                cargo.DetalleDocu.MonedaID.Value = this.monedaId;
                                cargo.DetalleDocu.OrigenMonetario.Value = Convert.ToByte((int)this._tipoMonedaOr);

                                cargo.DetalleDocu.inReferenciaID.Value = string.Empty;
                                cargo.DetalleDocu.Parametro1.Value = string.Empty;
                                cargo.DetalleDocu.Parametro2.Value = string.Empty;
                                cargo.DetalleDocu.SolicitudDetaID.Value = det.DetalleDocu.SolicitudDetaID.Value.Value;
                                cargo.DetalleDocu.SolicitudDocuID.Value = det.DetalleDocu.SolicitudDocuID.Value.Value;
                                cargo.DetalleDocu.Descriptivo.Value = cacheBienServ.Count > 0 ? cacheBienServ[cargo.DetalleDocu.CodigoBSID.Value].Item1.Descriptivo.Value : string.Empty;
                                cargo.DetalleDocu.UnidadInvID.Value = string.Empty;
                                cargo.DetalleDocu.CantidadSol.Value = 0;
                                cargo.DetalleDocu.CantidadOC.Value = 1;
                                cargo.DetalleDocu.CantEmpaque.Value = 1;
                                cargo.DetalleDocu.DiasEntrega.Value = det.DetalleDocu.DiasEntrega.Value.Value;

                                if (this.monedaLocal.Equals(this.monedaOrden))
                                {
                                    cargo.DetalleDocu.IvaTotML.Value = Math.Round(cargo.DetalleDocu.IVAUni.Value.Value * cargo.DetalleDocu.CantidadOC.Value.Value,2);                                
                                    cargo.DetalleDocu.ValorTotML.Value = Math.Round(cargo.DetalleDocu.ValorUni.Value.Value * cargo.DetalleDocu.CantidadOC.Value.Value,2);
                                    cargo.DetalleDocu.ValorTotME.Value = 0;
                                    cargo.DetalleDocu.IvaTotME.Value = 0;
                                }
                                else
                                {
                                    cargo.DetalleDocu.IvaTotME.Value = cargo.DetalleDocu.IVAUni.Value.Value * cargo.DetalleDocu.CantidadOC.Value.Value;
                                    cargo.DetalleDocu.ValorTotME.Value = cargo.DetalleDocu.ValorUni.Value.Value * cargo.DetalleDocu.CantidadOC.Value.Value;
                                    cargo.DetalleDocu.ValorTotML.Value = 0;
                                    cargo.DetalleDocu.IvaTotML.Value = 0;
                                }
                               

                                #region AIU

                                #region Admin
                                cargo.DetalleDocu.CodigoAdminAIU.Value = string.Empty;
                                cargo.DetalleDocu.PorIVAAdminAIU.Value = 0;
                                cargo.DetalleDocu.IVAAdminAIU.Value = 0;
                                cargo.DetalleDocu.ValorAdminAIU.Value = 0;
                                #endregion
                                #region Imprev
                                cargo.DetalleDocu.CodigoImprevAIU.Value = string.Empty;
                                cargo.DetalleDocu.PorIVAImprevAIU.Value = 0;
                                cargo.DetalleDocu.IVAImprevAIU.Value = 0;
                                cargo.DetalleDocu.ValorImprevAIU.Value = 0;
                                #endregion
                                #region Utilidad
                                cargo.DetalleDocu.CodigoUtilidadAIU.Value = string.Empty;
                                cargo.DetalleDocu.PorIVAUtilidadAIU.Value = 0;
                                cargo.DetalleDocu.IVAUtilidadAIU.Value = 0;
                                cargo.DetalleDocu.ValorUtilidadAIU.Value = 0;
                                #endregion

                                cargo.DetalleDocu.ValorBaseAIU.Value = 0;
                                cargo.DetalleDocu.VlrIVAAIU.Value = 0;
                                cargo.DetalleDocu.ValorAIU.Value = 0;
                                #endregion

                                cargo.SolicitudCargos = new List<DTO_prSolicitudCargos>();
                            }
                        }
                        #endregion
                    }
                    else
                        cargosNoIncluidos = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "GetValuesAIU"));
            }

            return cargosNoIncluidos;
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Cuenta
            if (colName == this._codigoRsx)
                return AppMasters.prBienServicio;
            //Tercero
            if (colName == this._referenciaRsx)
                return AppMasters.inReferencia;
            //Prefijo
            if (colName == this._parametro1Rsx)
                return AppMasters.inRefParametro1;
            //Prefijo
            if (colName == this._parametro2Rsx)
                return AppMasters.inRefParametro2;
            //Linea presupuestal
            if (colName == this._unidadRsx)
                return AppMasters.inUnidad;
            //Proyecto
            if (colName == this._proyectoRsx)
                return AppMasters.coProyecto;
            //Cwentro Costo
            if (colName == this._centroCostoRsx)
                return AppMasters.coCentroCosto;

            return 0;   
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            try
            {
                this.data.Footer = this.data.Footer.OrderBy(x => x.DetalleDocu.Descriptivo.Value).ThenBy(x => x.DetalleDocu.ConsecutivoDetaID.Value).ToList();
                for (int i = 0; i < this.data.Footer.Count; i++)
                    this.data.Footer[i].DetalleDocu.Index = i;
                this.gcDocument.DataSource = this.data.Footer;
                this.gcDocument.RefreshDataSource();
                bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
                if (hasItems)
                {
                    this.gvDocument.MoveFirst();
                    this.gcCargos.DataSource = this._currentRow.SolicitudCargos;
                    this.gcCargos.RefreshDataSource();
                    bool hasItemsCargos = this._currentRow.SolicitudCargos.GetEnumerator().MoveNext() ? true : false;
                    if (hasItems)
                        this.gvCargos.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "LoadData"));
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
                int cFila = fila;
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(cFila, col));

                this.LoadEditGridData(false, cFila);
                
                if (this.data.Footer.Count > 0)
                {
                    this.gcCargos.DataSource = this._currentRow.SolicitudCargos;// this.data.Footer[this.indexFila].SolicitudCargos;
                    this.gcCargos.RefreshDataSource();
                    this.gvCargos.RefreshData();
                    this.gvCargos.FocusedRowHandle = 0;
                }
                else
                {
                    this.gcCargos.DataSource = null;
                    this.gcCargos.RefreshDataSource();
                }

                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "RowIndexChanged"));
            }
        }

        /// <summary>
        /// Calcula valores globales del documento
        /// </summary>
        /// <param name="index">identificador de fila actual de la grilla</param>
        protected virtual void GetValuesDocument(int index,bool isIndividual,bool isDescuento) { }

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        protected void GenerateReport(bool show, bool allFields = false)
        {
            try
            {
                string reportName;
                if (this.data.DocCtrl.NumeroDoc.Value != null && this.data.DocCtrl.NumeroDoc.Value != 0)
                    reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompra(this.data.DocCtrl.NumeroDoc.Value.Value, this._tipoReporte, true);
                else
                    MessageBox.Show("Debe guardar antes el documento actual para ver el reporte");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "GenerateReport"));
            }

        }
        
        /// <summary>
        /// Calcula los valores de cada item de solicitud de OC
        /// </summary>
        protected void CalculateValues(int index)
        {
            try
            {
                #region ValorUni
                if (this._currentRow.DetalleDocu.ValorUni.Value >= 0)
                {
                    List<DTO_prOrdenCompraFooter> tempFooterList = this.documentID == AppDocuments.OrdenCompra ? this.GetValuesAIU(this._currentRow, this.data.Footer.Count) : null;
                    if (tempFooterList != null)
                    {
                        if (string.IsNullOrEmpty(this._codBienServicio))
                        {
                            this.footerAIU = new List<DTO_prOrdenCompraFooter>();
                            this.footerAIU.AddRange(tempFooterList);
                            //this.data.Footer.AddRange(tempFooterList);
                            this._codBienServicio = this._currentRow.DetalleDocu.CodigoBSID.Value;
                        }
                    }

                    if (this._solicitudes.Count > 0)
                        if (this._solicitudes.Exists(x => x.ConsecutivoDetaID.Value == this._currentRow.DetalleDocu.SolicitudDetaID.Value))
                            this._solicitudes.Find(x => x.ConsecutivoDetaID.Value == this._currentRow.DetalleDocu.SolicitudDetaID.Value).ValorUni.Value = this._currentRow.DetalleDocu.ValorUni.Value.Value;

                    this.GetValuesDocument(index,true,false);

                    this.gvDocument.RefreshData();
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "CalculateValues"));
            }

        }

        /// <summary>
        /// MUestra la disponibilidad del item actual SI pertenece a un proyecto de presupuesto
        /// </summary>
        /// <param name="consMvto">Consecutivo del item de mvto del proyecto</param>
        protected void ViewTrazabilidad(int? numDocProy, int? consMvto, string recursoID)
        {
            try
            {
                ModalProyectoMvto viewDocs = new ModalProyectoMvto(numDocProy,consMvto,recursoID);
                viewDocs.Show(); 
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "ViewTrazabilidad"));
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
            this.defAreaFunc = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //this.defLocFisica = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto)
           
            //Carga los recursos de las Fks
            this._codigoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            this._referenciaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            this._parametro1Rsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro1");
            this._parametro2Rsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro2");
            this._unidadRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._centroCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");

            //Carga los recursos de los valores            
            this._cantidadSolRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
            this._cantidadOCRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadOC");
            this._porcentajeRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
            this._descRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            this._solCargosRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SolicitudCargos");

            this.gcDocument.ShowOnlyPredefinedDetails = true;    
            this.gvDocument.RowCellStyle += new RowCellStyleEventHandler(gvDocument_RowCellStyle);

            FormProvider.Master.itemPaste.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;
            
            this._solicitudes = new List<DTO_prSolicitudResumen>();

            this.format = string.Empty;

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize() 
        {
            base.AfterInitialize();
            this.gvDocument.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.AddGridCols();
            this.AddCargosGridCols();
            this.lastColName = this.unboundPrefix + "CantidadOC";
            this.EnableFooter(false);

            this._bc.InitMasterUC(this.masterParam1, AppMasters.inRefParametro1, true, false, true, false);
            this._bc.InitMasterUC(this.masterParam2, AppMasters.inRefParametro2, true, false, true, false);
            this._bc.InitMasterUC(this.masterEmpaque, AppMasters.inEmpaque, false, false, true, false);
            this._bc.InitMasterUC(this.masterUnidad, AppMasters.inUnidad, false, false, true, false);
            this.masterParam1.EnableControl(false);
            this.masterParam2.EnableControl(false);
            this.masterEmpaque.EnableControl(false);
            this.masterUnidad.EnableControl(false);
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
                        var det = _bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (det != null)
                        {
                            try
                            {
                                this.LoadTempData(det);
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
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "AfterInitialize: " + ex.Message));
                }
            }
            #endregion
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

                if (this.data != null && this.data.DocCtrl != null && this.data.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                }              

                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
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
                #region Columnas basicas
                base.editValue.Mask.EditMask = "n2";

                //CodigoServicios
                GridColumn codBS= new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = this._codigoRsx;
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 60;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = this._referenciaRsx;
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 60;
                codRef.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codRef);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID"); 
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 3;
                MarcaInvID.Width = 60;
                MarcaInvID.Visible = true;
                MarcaInvID.Fixed = FixedStyle.Left;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MarcaInvID);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor"); 
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 4;
                RefProveedor.Width = 60;
                RefProveedor.Visible = true;
                RefProveedor.Fixed = FixedStyle.Left;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RefProveedor);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                #endregion
                #region Columnas extras
                #region Columnas Visible
                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = this._descRsx;
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 5;
                desc.Width = 230;
                desc.Visible = true;
                desc.Fixed = FixedStyle.Left;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = "UM";
                unidad.UnboundType = UnboundColumnType.String;
                unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                unidad.AppearanceCell.Options.UseTextOptions = true;
                unidad.VisibleIndex = 6;
                unidad.Width = 43;
                unidad.Visible = false;
                unidad.Fixed = FixedStyle.Left;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(unidad);

                //unidadEmp
                GridColumn EmpaqueInvID = new GridColumn();
                EmpaqueInvID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                EmpaqueInvID.Caption = "Empaque";
                EmpaqueInvID.UnboundType = UnboundColumnType.String;
                EmpaqueInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                EmpaqueInvID.AppearanceCell.Options.UseTextOptions = true;
                EmpaqueInvID.AppearanceCell.ForeColor = Color.Gray;
                EmpaqueInvID.AppearanceCell.Options.UseForeColor = true;
                EmpaqueInvID.VisibleIndex = 6;
                EmpaqueInvID.Width = 55;
                EmpaqueInvID.Visible = true;
                EmpaqueInvID.Fixed = FixedStyle.Left;
                EmpaqueInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(EmpaqueInvID);

                //Cantidad Emp
                GridColumn cantEmpaque = new GridColumn();
                cantEmpaque.FieldName = this.unboundPrefix + "CantEmpaque";
                cantEmpaque.Caption = "Cant Empaque";
                cantEmpaque.UnboundType = UnboundColumnType.Integer;
                cantEmpaque.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantEmpaque.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 7.3F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                cantEmpaque.AppearanceCell.Options.UseTextOptions = true;
                cantEmpaque.AppearanceCell.ForeColor = Color.Gray;
                cantEmpaque.AppearanceCell.Options.UseForeColor = true;
                cantEmpaque.AppearanceHeader.Options.UseFont = true;
                cantEmpaque.VisibleIndex = 7;
                cantEmpaque.Width = 60;
                cantEmpaque.Visible = true;
                cantEmpaque.Fixed = FixedStyle.Left;
                cantEmpaque.ColumnEdit = base.editValue;
                cantEmpaque.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cantEmpaque);

                //Cantidad Orde de Compra
                GridColumn cantOC = new GridColumn();
                cantOC.FieldName = this.unboundPrefix + "CantidadOC";
                cantOC.Caption = this._cantidadOCRsx;
                cantOC.UnboundType = UnboundColumnType.Integer;
                cantOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantOC.AppearanceCell.Options.UseTextOptions = true;
                cantOC.VisibleIndex = 7;
                cantOC.Width = 60;
                cantOC.Visible = true;
                cantOC.Fixed = FixedStyle.Left;
                cantOC.ColumnEdit = base.editValue;
                cantOC.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cantOC);

                //ValorUni
                GridColumn valorUni = new GridColumn();
                valorUni.FieldName = this.unboundPrefix + "ValorUni";
                valorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                valorUni.UnboundType = UnboundColumnType.Decimal;
                valorUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorUni.AppearanceCell.Options.UseTextOptions = true;
                valorUni.VisibleIndex = 8;
                valorUni.Width = 80;
                valorUni.Visible = true;
                valorUni.Fixed = FixedStyle.Left;
                valorUni.ColumnEdit = base.editValue4;
                valorUni.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valorUni);

                //IvaUni
                GridColumn ivaUni = new GridColumn();
                ivaUni.FieldName = this.unboundPrefix + "IVAUni";
                ivaUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IVAUni");
                ivaUni.UnboundType = UnboundColumnType.Decimal;
                ivaUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ivaUni.AppearanceCell.Options.UseTextOptions = true;
                ivaUni.VisibleIndex = 9;
                ivaUni.Width = 70;
                ivaUni.Visible = true;
                ivaUni.Fixed = FixedStyle.Left;
                ivaUni.ColumnEdit = base.editSpin;
                ivaUni.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ivaUni);

                //ValorTotML
                GridColumn valorTotML = new GridColumn();
                valorTotML.FieldName = this.unboundPrefix + "ValorTotML";
                valorTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotML.UnboundType = UnboundColumnType.Decimal;
                valorTotML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotML.AppearanceCell.Options.UseTextOptions = true;
                valorTotML.VisibleIndex = 10;
                valorTotML.Width = 120;
                valorTotML.Visible = true;
                valorTotML.ColumnEdit = base.editSpin;
                valorTotML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotML);

                //IvaTotML
                GridColumn ivaTotML = new GridColumn();
                ivaTotML.FieldName = this.unboundPrefix + "IvaTotML";
                ivaTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotML");
                ivaTotML.UnboundType = UnboundColumnType.Decimal;
                ivaTotML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ivaTotML.AppearanceCell.Options.UseTextOptions = true;
                ivaTotML.VisibleIndex = 11;
                ivaTotML.Width = 90;
                ivaTotML.Visible = true;
                ivaTotML.ColumnEdit = base.editSpin;
                ivaTotML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ivaTotML);
             
                #endregion
                #region Columnas No Visibles

                //valorTotME
                GridColumn valorTotME = new GridColumn();
                valorTotME.FieldName = this.unboundPrefix + "ValorTotME";
                valorTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotME.UnboundType = UnboundColumnType.Decimal;
                valorTotME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotME.AppearanceCell.Options.UseTextOptions = true;
                valorTotME.VisibleIndex = 12;
                valorTotME.Width = 120;
                valorTotME.Visible = false;
                valorTotME.ColumnEdit = base.editSpin;
                valorTotME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotME);

                //IvaTotME
                GridColumn ivaTotME = new GridColumn();
                ivaTotME.FieldName = this.unboundPrefix + "IvaTotME";
                ivaTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotML");
                ivaTotME.UnboundType = UnboundColumnType.Decimal;
                ivaTotME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ivaTotME.AppearanceCell.Options.UseTextOptions = true;
                ivaTotME.VisibleIndex = 13;
                ivaTotME.Width = 90;
                ivaTotME.Visible = false;
                ivaTotME.ColumnEdit = base.editSpin;
                ivaTotME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ivaTotME);

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDocument.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDocument.Columns.Add(consDeta);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDocument.Columns.Add(solDocu);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDocument.Columns.Add(solDeta);

                //OrdCompraDocuID
                GridColumn ordCompDocu = new GridColumn();
                ordCompDocu.FieldName = this.unboundPrefix + "OrdCompraDocuID";
                ordCompDocu.UnboundType = UnboundColumnType.Integer;
                ordCompDocu.Visible = false;
                this.gvDocument.Columns.Add(ordCompDocu);

                //OrdCompraDetaID
                GridColumn ordCompDeta = new GridColumn();
                ordCompDeta.FieldName = this.unboundPrefix + "OrdCompraDetaID";
                ordCompDeta.UnboundType = UnboundColumnType.Integer;
                ordCompDeta.Visible = false;
                this.gvDocument.Columns.Add(ordCompDeta);

                //Cantidad Solicitud
                GridColumn cantSol = new GridColumn();
                cantSol.FieldName = this.unboundPrefix + "CantidadSol";
                cantSol.UnboundType = UnboundColumnType.Integer;
                cantSol.Visible = false;
                this.gvDocument.Columns.Add(cantSol);

                //ValorAIU
                GridColumn valorAIU = new GridColumn();
                valorAIU.FieldName = this.unboundPrefix + "ValorAIU";
                valorAIU.UnboundType = UnboundColumnType.Decimal;
                valorAIU.Visible = false;
                this.gvDocument.Columns.Add(valorAIU);

                //ValorIVAAIU
                GridColumn valorIVAAIU = new GridColumn();
                valorIVAAIU.FieldName = this.unboundPrefix + "VlrIVAAIU";
                valorIVAAIU.UnboundType = UnboundColumnType.Decimal;
                valorIVAAIU.Visible = false;
                this.gvDocument.Columns.Add(valorIVAAIU);

                //FechaEntr
                GridColumn fechaEntr = new GridColumn();
                fechaEntr.FieldName = this.unboundPrefix + "FechaEntr";
                fechaEntr.UnboundType = UnboundColumnType.DateTime;
                fechaEntr.Visible = false;
                this.gvDocument.Columns.Add(fechaEntr);

                //DiasEntr
                GridColumn diasEntr = new GridColumn();
                diasEntr.FieldName = this.unboundPrefix + "DiasEntrega";
                diasEntr.UnboundType = UnboundColumnType.Integer;
                diasEntr.Visible = false;
                this.gvDocument.Columns.Add(diasEntr);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "AddGridCols"));
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
                    if (rowIndex >= 0)
                        this._currentRow = (DTO_prOrdenCompraFooter)this.gvDocument.GetRow(rowIndex);
                    
                    string val_DiasEntrega = (isNew || gvDocument.Columns[this.unboundPrefix + "DiasEntrega"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "DiasEntrega"]).ToString();
                    string val_ValorIVAAIU = (isNew || gvDocument.Columns[this.unboundPrefix + "ValorIVAAIU"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ValorIVAAIU"]).ToString();

                    this.txtDiasEnrt.Text = val_DiasEntrega;
                    this.masterParam1.Value = this._currentRow.DetalleDocu.Parametro1.Value;
                    this.masterParam2.Value = this._currentRow.DetalleDocu.Parametro2.Value;
                    this.masterUnidad.Value = this._currentRow.DetalleDocu.UnidadInvID.Value;
                    this.masterEmpaque.Value = this._currentRow.DetalleDocu.EmpaqueInvID.Value;
                    this.dtFechaEntr.DateTime = Convert.ToDateTime(this.data.DocCtrl.Fecha.Value.Value.AddDays(Convert.ToInt32(this._currentRow.DetalleDocu.DiasEntrega.Value.ToString() ?? "0")));

                    this.txtCantidadUM.EditValue = this._currentRow.DetalleDocu.CantidadOC.Value;
                    this.txtCantEmpaque.EditValue = this._currentRow.DetalleDocu.CantEmpaque.Value;
                    this.txtCantSolicitud.EditValue = this._currentRow.DetalleDocu.CantidadSol.Value*-1;                   
                    this.txtValorIVAUnit.EditValue = this._currentRow.DetalleDocu.IVAUni.Value;                  
                    this.txtValorUni.EditValue = this._currentRow.DetalleDocu.ValorUni.Value;
                    if (this.monedaLocal.Equals(this.monedaOrden))
                    {
                        this.txtValorTotal.EditValue = this._currentRow.DetalleDocu.ValorTotML.Value;
                        this.txtValorIVATotal.EditValue = this._currentRow.DetalleDocu.IvaTotML.Value;
                    }
                    else
                    {
                        this.txtValorTotal.EditValue = this._currentRow.DetalleDocu.ValorTotME.Value;
                        this.txtValorIVATotal.EditValue = this._currentRow.DetalleDocu.IvaTotME.Value;
                    }
                    this.txtValorAIU.EditValue = this._currentRow.DetalleDocu.ValorAIU.Value;
                    this.txtValorIVAAIU.EditValue = val_ValorIVAAIU; 

                    if (this._currentRow.DetalleDocu.Detalle4ID.Value.HasValue)//Si tiene relacionado una tarea de proyecto
                    {
                       DTO_pyProyectoMvto mvto = this._bc.AdministrationModel.pyProyectoMvto_GetByConsecutivo(this._currentRow.DetalleDocu.Detalle4ID.Value);
                        this.txtTareaID.EditValue = mvto != null ? mvto.TareaID.Value : string.Empty;
                        this.txtTareaDescripcion.EditValue = mvto != null ? mvto.TareaDesc.Value: string.Empty;
                        this._currentRow.DetalleDocu.TareaID.Value = this.txtTareaID.EditValue.ToString();
                    }
                    else
                    {
                        this.txtTareaID.EditValue = string.Empty;
                        this.txtTareaDescripcion.EditValue = string.Empty;
                        this._currentRow.DetalleDocu.TareaID.Value = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "LoadEditGridData"));
                }
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow() { }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected virtual decimal LoadTasaCambio(int monOr, DateTime fechaOC)
        {
            try
            {
                decimal valor = 0;
                string tasaMon = this.monedaId;
                if (monOr == (int)TipoMoneda.Local)
                    tasaMon = this.monedaExtranjera;

                valor = _bc.AdministrationModel.TasaDeCambio_Get(tasaMon, fechaOC);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "LoadTasaCambio"));
                return 0;
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
                GridColumn col = new GridColumn();
                               
                #region CantidadOC
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadOC", false, false, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region ValorUni
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorUni", false, true, true, false);
                if (!validField)
                    validRow = false;
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
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "ValidateRow"));
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
            if (this.data != null && this.data.Footer != null && this.data.Footer.Count == 0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }
            else if (this.data.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_DocumentEstateAprob));
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
            this.txtValorUni.Enabled = enable;
            this.masterEmpaque.EnableControl(enable);
            this.dtFechaEntr.Enabled = false;
            this.btnSolicitudes.Enabled = enable;
            this.btnEstadoEjecucion.Enabled = enable;

            if (this.gvDocument.DataRowCount == 0)
                this.txtValorUni.Enabled = false;
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

            this._solicitudes = new List<DTO_prSolicitudResumen>();
        }

        /// <summary>
        /// Limpia y deja vacio los controles del footer
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanFooter()
        {
            this.txtCantidadUM.EditValue = "0";
            this.txtCantEmpaque.EditValue = "0";
            this.txtCantSolicitud.EditValue = "0";
            this.txtValorUni.EditValue = "0";
            this.txtValorTotal.EditValue = "0";
            this.txtValorIVAUnit.EditValue = "0";
            this.txtValorIVATotal.EditValue = "0";
            this.txtValorAIU.EditValue = "0";
            this.txtValorIVAAIU.EditValue = "0";

            this.txtDiasEnrt.EditValue = "0";
            this.masterParam1.Value = string.Empty;
            this.masterParam2.Value = string.Empty;
            this.masterEmpaque.Value = string.Empty;
            this.masterUnidad.Value = string.Empty;
            this.txtTareaID.EditValue = string.Empty;
            this.txtTareaDescripcion.EditValue = string.Empty;
            this.dtFechaEntr.DateTime = this.dtFecha.DateTime;
            this._codBienServicio = null;
            this.gvCargos.ActiveFilterString = string.Empty;
            this.gcCargos.DataSource = null;
            this.gvCargos.RefreshData();
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected virtual void EnableHeader(bool enable) { }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected virtual object LoadTempHeader() { return null; }

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
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(object orden) { }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanSave() 
        {
            bool gridValid = true;
            try
            {    
                if (this.data != null && this.data.Footer != null && this.data.Footer.Count != 0)
                {
                    GridColumn col = new GridColumn();
                    decimal val;

                    for (int i = 0; i < this.gvDocument.RowCount; i++)
                    {
                        col = this.gvDocument.Columns[unboundPrefix + "ValorUni"];
                        val = Convert.ToDecimal(this.gvDocument.GetRowCellValue(i, col), CultureInfo.InvariantCulture);           
                        if (val < 0)
                            gridValid = false;
                    }
                    if (((this.valorTotalDoc + this.valorIVATotalDoc) < this.valorTotalPagosMes))
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_ValueTotalInvalid));
                        gridValid = false;
                    }

                    if (this._vlrTasaCambio == 0 && (this.data.HeaderOrdenCompra.MonedaPago.Value == this.monedaExtranjera ||
                       this.data.HeaderOrdenCompra.MonedaOrden.Value == this.monedaExtranjera))
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                        gridValid = false;
                    }
                }
                else
                    gridValid = false;
                return gridValid;
            }
            catch (Exception)
            {  
                return gridValid = false;
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
            //FormProvider.Master.itemSendtoAppr.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print); 
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
                    return;
                }

                this.gvDocument.PostEditor();

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
                            this._solicitudes.RemoveAll(x => x.ConsecutivoDetaID.Value == this._currentRow.DetalleDocu.SolicitudDetaID.Value);
                            this.data.Footer.RemoveAll(x => x.DetalleDocu.CodigoBSID.Value == this._currentRow.DetalleDocu.CodigoBSID.Value &&
                                                        x.DetalleDocu.inReferenciaID.Value == this._currentRow.DetalleDocu.inReferenciaID.Value &&
                                                        x.DetalleDocu.SolicitudDetaID.Value == this._currentRow.DetalleDocu.SolicitudDetaID.Value);

                            this.data.Footer.ForEach(x => { if (x.DetalleDocu.Index > this.indexFila) x.DetalleDocu.Index--; });

                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvDocument.RefreshData();
                            this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                            this.data.Footer = this.data.Footer.OrderBy(x => x.DetalleDocu.Descriptivo.Value).ThenBy(x=>x.DetalleDocu.ConsecutivoDetaID.Value).ToList();
                            this.gcDocument.DataSource = this.data.Footer;
                            this.gcDocument.RefreshDataSource();
                            this.CalculateValues(this.gvDocument.FocusedRowHandle);
                        }
                    }
                    e.Handled = true;
                }
            }            
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.disableValidate)
                {
                    bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                    this.deleteOP = false;

                    if (!validRow)
                    {
                        e.Allow = false;
                        this.isValid = false;
                    }
                    if (this.data.Footer.Count > 0 && this._currentRow.DetalleDocu.ValorUni.Value < 0)
                        this.txtValorUni.Focus();
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (this.documentID != AppDocuments.OrdenCompra && this.documentID != AppDocuments.Contrato)
                base.gvDocument_CustomUnboundColumnData(sender, e);
            else
            {
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                        else
                        {
                            DTO_prOrdenCompraFooter dtoDet = (DTO_prOrdenCompraFooter)e.Row;
                            pi = dtoDet.DetalleDocu.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    e.Value = pi.GetValue(dtoDet.DetalleDocu, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet.DetalleDocu, null), null);
                            }
                            else
                            {
                                fi = dtoDet.DetalleDocu.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                        e.Value = fi.GetValue(dtoDet.DetalleDocu);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet.DetalleDocu), null);
                                }
                            }
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                        {
                            pi.SetValue(dto, e.Value ,null);
                            //e.Value = pi.GetValue(dto, null);
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                            {
                                pi.SetValue(dto, e.Value, null);
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
                            DTO_prOrdenCompraFooter dtoDet = (DTO_prOrdenCompraFooter)e.Row;
                            pi = dtoDet.DetalleDocu.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                {
                                    e.Value = pi.GetValue(dtoDet.DetalleDocu, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoDet.DetalleDocu, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoDet.DetalleDocu.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    {
                                        pi.SetValue(dto, e.Value, null);
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoDet.DetalleDocu);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
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
                base.gvDocument_FocusedRowChanged(sender, e);
                if (e.FocusedRowHandle >= 0 && this.data.Footer.Count > 0)
                {
                    this._currentRow = (DTO_prOrdenCompraFooter)this.gvDocument.GetRow(e.FocusedRowHandle);
                    if (this._currentRow.DetalleDocu.ValorUni.Value < 0)
                        this.txtValorUni.Focus();

                    //Valida si activa la vista de los datos del proyecto de presupuesto
                    if (this._currentRow != null && this._currentRow.DetalleDocu.Documento4ID.Value.HasValue)
                        this.btnEstadoEjecucion.Enabled = true;
                    else
                        this.btnEstadoEjecucion.Enabled = false;
                }
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) { }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = (GridView)sender;
            if (e.Column.FieldName == unboundPrefix + "ValorUni" && e.RowHandle >= 0)
            {
                decimal cell = Convert.ToDecimal(View.GetRowCellValue(e.RowHandle, e.Column.FieldName), CultureInfo.InvariantCulture);
                if (cell >= 0)
                    e.Appearance.ForeColor = Color.Black;
                else
                    e.Appearance.ForeColor = Color.Red;
            }
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
                if (e.Column.FieldName == this.unboundPrefix + "ValorUni")
                {                    
                    this._currentRow.DetalleDocu.ValorUni.Value = Convert.ToDecimal(e.Value);
                    this._currentRow.DetalleDocu.VlrLocal01.Value = this._currentRow.DetalleDocu.ValorUni.Value;//Base de Descuento
                    this.txtValorUni.EditValue = this._currentRow.DetalleDocu.ValorUni.Value;
                    this._bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "ValorUni", false, true, true, false);
                    this.CalculateValues(this._currentRow.DetalleDocu.Index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "textControl_Leave"));
            }      
        }


        #endregion

        #region Eventos Grilla Cargos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_prSolicitudCargos dto = (DTO_prSolicitudCargos)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefixCargo.Length);

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
        #endregion

        #region Eventos Detalle (footer)

        /// <summary>
        /// Evento que se ejecuta al salir del campo de descripcion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int index = this.NumFila;
                TextEdit txt = (TextEdit)sender;
                if (txt.Name == this.txtValorUni.Name)//Valor Unitario
                {
                    this._currentRow.DetalleDocu.ValorUni.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    this._currentRow.DetalleDocu.VlrLocal01.Value = this._currentRow.DetalleDocu.ValorUni.Value;//Base de Descuento
                    this._bc.ValidGridCellValue(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "ValorUni", false,true, true, false);
                }
                this.CalculateValues(this._currentRow.DetalleDocu.Index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "textControl_Leave"));
            }       
        }

        /// <summary>
        /// Trae solicitudes asignados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSolicitudes_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.data.HeaderOrdenCompra.ProveedorID.Value, true);
                if (prov != null && prov.TipoProveedor.Value == 3)
                    this._tipoMonedaOr = TipoMoneda.Both;
                else
                    this._tipoMonedaOr = this.monedaOrden != this.monedaLocal ? TipoMoneda.Foreign : TipoMoneda.Local;
                ModalSolicitudes modSol = new ModalSolicitudes(this._solicitudes, this.dtPeriod.DateTime, this.documentID, this._tipoMonedaOr);
                modSol.ShowDialog();
               
                if (modSol.ReturnVals)
                {
                    #region Limpia la grilla y footer
                    List<DTO_prOrdenCompraFooter> footerTemp = ObjectCopier.Clone(this.data.Footer);
                    this.data.Footer.Clear();
                    this.deleteOP = true;
                    this.disableValidate = true;
                    this.LoadData(false);
                    this.disableValidate = false;
                    this.deleteOP = false;
                    this.CleanFooter();
                    this.UpdateTemp(this.data);
                    #endregion

                    DTO_prBienServicio bs;
                    this._solicitudes = modSol.ReturnList;

                    #region Filtra si el proveedor es multimoneda
                    if (prov != null && prov.TipoProveedor.Value == 3)
                    {
                        if (this.monedaOrden == this.monedaLocal)
                            this._solicitudes = this._solicitudes.FindAll(x => x.OrigenMonetario.Value == 1 || x.OrigenMonetario.Value == null);
                        else
                            this._solicitudes = this._solicitudes.FindAll(x => x.OrigenMonetario.Value == 2);
                    } 
                    #endregion                    

                    #region Agrega los registros de los solicitudes
                    int index = 0;
                    this._solicitudes = this._solicitudes.OrderBy(x => x.Descriptivo.Value).ThenBy(x => x.ConsecutivoDetaID.Value).ToList();
                    this._solicitudes.ForEach(newSol =>
                    {
                        DTO_prOrdenCompraFooter newDet = new DTO_prOrdenCompraFooter();
                        newDet.DetalleDocu.Index = index;
                        newDet.DetalleDocu.EmpresaID.Value = this.documentID == AppDocuments.OrdenCompra ? this.data.HeaderOrdenCompra.EmpresaID.Value : this.data.HeaderContrato.EmpresaID.Value;
                        newDet.DetalleDocu.EstadoInv.Value = 1;
                        newDet.DetalleDocu.MonedaID.Value = this.monedaId;
                        newDet.DetalleDocu.OrigenMonetario.Value = Convert.ToByte((int)this._tipoMonedaOr);
                        newDet.DetalleDocu.CodigoBSID.Value = newSol.CodigoBSID.Value;
                        newDet.DetalleDocu.inReferenciaID.Value = newSol.inReferenciaID.Value;
                        newDet.DetalleDocu.Parametro1.Value = newSol.Parametro1.Value;
                        newDet.DetalleDocu.Parametro2.Value = newSol.Parametro2.Value;
                        newDet.DetalleDocu.SolicitudDetaID.Value = newSol.ConsecutivoDetaID.Value;
                        newDet.DetalleDocu.SolicitudDocuID.Value = newSol.SolicitudDocuID.Value;
                        newDet.DetalleDocu.Documento1ID.Value = newSol.Documento1ID.Value;
                        newDet.DetalleDocu.Detalle1ID.Value = newSol.Detalle1ID.Value;
                        newDet.DetalleDocu.CantidadDoc1.Value = newSol.CantidadDoc1.Value;
                        newDet.DetalleDocu.DatoAdd1.Value = newSol.DatoAdd1.Value;
                        newDet.DetalleDocu.Documento2ID.Value = newSol.Documento2ID.Value;
                        newDet.DetalleDocu.Detalle2ID.Value = newSol.Detalle2ID.Value;
                        newDet.DetalleDocu.CantidadDoc2.Value = newSol.CantidadDoc2.Value;
                        newDet.DetalleDocu.DatoAdd2.Value = newSol.DatoAdd2.Value;
                        newDet.DetalleDocu.Documento3ID.Value = newSol.Documento3ID.Value;
                        newDet.DetalleDocu.Detalle3ID.Value = newSol.Detalle3ID.Value;
                        newDet.DetalleDocu.DatoAdd3.Value = newSol.DatoAdd3.Value;
                        newDet.DetalleDocu.CantidadDoc3.Value = newSol.CantidadDoc3.Value;
                        newDet.DetalleDocu.Documento4ID.Value = newSol.Documento4ID.Value;
                        newDet.DetalleDocu.Detalle4ID.Value = newSol.Detalle4ID.Value;                        
                        newDet.DetalleDocu.CantidadDoc4.Value = newSol.CantidadDoc4.Value;
                        newDet.DetalleDocu.DatoAdd4.Value = newSol.DatoAdd4.Value;
                        newDet.DetalleDocu.Documento5ID.Value = newSol.Documento5ID.Value;
                        newDet.DetalleDocu.Detalle5ID.Value = newSol.Detalle5ID.Value;
                        newDet.DetalleDocu.CantidadDoc5.Value = newSol.CantidadDoc5.Value;
                        newDet.DetalleDocu.DatoAdd5.Value = newSol.DatoAdd5.Value;
                        newDet.DetalleDocu.Descriptivo.Value = newSol.Descriptivo.Value;
                        newDet.DetalleDocu.UnidadInvID.Value = newSol.UnidadInvID.Value;
                        newDet.DetalleDocu.EmpaqueInvID.Value = newSol.EmpaqueInvID.Value;
                        newDet.DetalleDocu.CantidadxEmpaque.Value = newSol.CantidadxEmpaque.Value;
                        newDet.DetalleDocu.CantEmpaque.Value = newSol.CantidadxEmpaque.Value > 1? Math.Ceiling(newSol.CantidadOrdenComp.Value.Value / newSol.CantidadxEmpaque.Value.Value) : 1;
                        newDet.DetalleDocu.UnidadEmpaque.Value = newSol.UnidadEmpaque.Value;
                        newDet.DetalleDocu.MarcaInvID.Value = newSol.MarcaInvID.Value;
                        newDet.DetalleDocu.RefProveedor.Value = newSol.RefProveedor.Value;
                        newDet.DetalleDocu.LineaPresupuestoID.Value = newSol.LineaPresupuestoID.Value;
                        newDet.DetalleDocu.CantidadSol.Value = (-1) * newSol.CantidadOrdenComp.Value;
                        newDet.DetalleDocu.CantidadOC.Value = newSol.CantidadOrdenComp.Value;
                        newDet.DetalleDocu.CantidadCont.Value = this.documentID != AppDocuments.OrdenCompra? newSol.CantidadOrdenComp.Value: 0;
                        newDet.DetalleDocu.DiasEntrega.Value = Convert.ToByte(this._daysEntr);

                        #region Trae BienServicio de la solicitud
                        if (!cacheBienServ.ContainsKey(newDet.DetalleDocu.CodigoBSID.Value))
                            this.BienServ = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, newDet.DetalleDocu.CodigoBSID.Value, true);
                        bs = cacheBienServ[newDet.DetalleDocu.CodigoBSID.Value].Item1;
                        #endregion

                        newDet.DetalleDocu.ValorUni.Value = newSol.ValorUni.Value.Value;
                        newDet.DetalleDocu.PorcentajeIVA.Value = cacheBienServ[newDet.DetalleDocu.CodigoBSID.Value].Item2;
                        newDet.DetalleDocu.IVAUni.Value = 0;
                        newDet.DetalleDocu.IvaTotML.Value = 0;
                        newDet.DetalleDocu.IvaTotME.Value = 0;
                        newDet.DetalleDocu.ValorTotML.Value = 0;
                        newDet.DetalleDocu.ValorTotME.Value = 0;

                        #region AIU

                        #region Admin
                        newDet.DetalleDocu.CodigoAdminAIU.Value = (string.IsNullOrEmpty(bs.CBS_AdminContrConstr.Value)) ?
                            ((string.IsNullOrEmpty(bs.CBS_AdminGtosReem.Value)) ? string.Empty : bs.CBS_AdminGtosReem.Value) :
                            _bienServ.CBS_AdminContrConstr.Value;
                        newDet.DetalleDocu.PorIVAAdminAIU.Value = 0;
                        if (!string.IsNullOrEmpty(newDet.DetalleDocu.CodigoAdminAIU.Value))
                        {
                            if (!cacheBienServ.ContainsKey(newDet.DetalleDocu.CodigoAdminAIU.Value))
                                this.BienServ = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, newDet.DetalleDocu.CodigoAdminAIU.Value, true);
                            newDet.DetalleDocu.PorIVAAdminAIU.Value = cacheBienServ[newDet.DetalleDocu.CodigoAdminAIU.Value].Item2;
                        }                            
                        newDet.DetalleDocu.IVAAdminAIU.Value = 0;
                        newDet.DetalleDocu.ValorAdminAIU.Value = 0;
                        #endregion
                        #region Imprev
                        newDet.DetalleDocu.CodigoImprevAIU.Value = (string.IsNullOrEmpty(bs.CBS_ImprevContrConstr.Value)) ? string.Empty : bs.CBS_ImprevContrConstr.Value;
                        newDet.DetalleDocu.PorIVAImprevAIU.Value = 0;
                        if (!string.IsNullOrEmpty(newDet.DetalleDocu.CodigoImprevAIU.Value))
                        {
                            if (!cacheBienServ.ContainsKey(newDet.DetalleDocu.CodigoImprevAIU.Value))
                                this.BienServ = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, newDet.DetalleDocu.CodigoImprevAIU.Value, true);
                            newDet.DetalleDocu.PorIVAImprevAIU.Value = cacheBienServ[newDet.DetalleDocu.CodigoImprevAIU.Value].Item2;
                        }                            
                        newDet.DetalleDocu.IVAImprevAIU.Value = 0;
                        newDet.DetalleDocu.ValorImprevAIU.Value = 0;
                        #endregion
                        #region Utilidad
                        newDet.DetalleDocu.CodigoUtilidadAIU.Value = (string.IsNullOrEmpty(bs.CBS_UtiliContrConstr.Value)) ? string.Empty : bs.CBS_UtiliContrConstr.Value;
                        newDet.DetalleDocu.PorIVAUtilidadAIU.Value = 0;
                        if (!string.IsNullOrEmpty(newDet.DetalleDocu.CodigoUtilidadAIU.Value))
                        {
                            if (!cacheBienServ.ContainsKey(newDet.DetalleDocu.CodigoUtilidadAIU.Value))
                                this.BienServ = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, newDet.DetalleDocu.CodigoUtilidadAIU.Value, true);
                            newDet.DetalleDocu.PorIVAUtilidadAIU.Value = cacheBienServ[newDet.DetalleDocu.CodigoUtilidadAIU.Value].Item2;
                        }
                        newDet.DetalleDocu.IVAUtilidadAIU.Value = 0;
                        newDet.DetalleDocu.ValorUtilidadAIU.Value = 0;
                        #endregion

                        newDet.DetalleDocu.ValorBaseAIU.Value = 0;
                        newDet.DetalleDocu.VlrIVAAIU.Value = 0;
                        newDet.DetalleDocu.ValorAIU.Value = 0;
                        #endregion

                        newDet.SolicitudCargos = newSol.SolicitudCargos;
                        this.data.Footer.Add(newDet);

                        index++;
                    });
                    #endregion
                    //Asigna los items que ya existen en bd
                    List<DTO_prOrdenCompraFooter> footerExist = new List<DTO_prOrdenCompraFooter>();
                    foreach (DTO_prOrdenCompraFooter oc in this.data.Footer)
                    {
                        DTO_prOrdenCompraFooter exist = footerTemp.Find(x => x.DetalleDocu.SolicitudDetaID.Value == oc.DetalleDocu.SolicitudDetaID.Value);
                        if (exist != null)
                        {
                            exist.DetalleDocu.CantidadSol.Value = oc.DetalleDocu.CantidadSol.Value;
                            exist.DetalleDocu.CantidadOC.Value = oc.DetalleDocu.CantidadOC.Value;
                            exist.DetalleDocu.CantidadxEmpaque.Value = oc.DetalleDocu.CantidadxEmpaque.Value;
                            exist.DetalleDocu.CantEmpaque.Value = oc.DetalleDocu.CantEmpaque.Value ;
                            oc.DetalleDocu = exist.DetalleDocu;
                            oc.SolicitudCargos = exist.SolicitudCargos;
                        }
                        else 
                        {
                            //Valida si el item nuevo ya existe y lo agrega aparte
                            DTO_prOrdenCompraFooter rowExist = footerTemp.Find(x => x.DetalleDocu.inReferenciaID.Value == oc.DetalleDocu.inReferenciaID.Value && 
                                                                x.DetalleDocu.CodigoBSID.Value == oc.DetalleDocu.CodigoBSID.Value && !x.DetalleDocu.ConsecutivoDetaID.Value.HasValue);
                            //if (rowExist != null)
                            //    footerExist.Add(rowExist);
                        }  
                    }
                    this.data.Footer.AddRange(footerExist);

                    this.data.Footer = this.data.Footer.OrderBy(x => x.DetalleDocu.Descriptivo.Value).ThenBy(x => x.DetalleDocu.ConsecutivoDetaID.Value).ToList();
                    for (int i = 0; i < this.data.Footer.Count; i++)
                    {
                        DTO_prOrdenCompraFooter det = this.data.Footer[i];
                        det.DetalleDocu.Index = i;
                        det.DetalleDocu.PorcentajeIVA.Value = cacheBienServ[det.DetalleDocu.CodigoBSID.Value].Item2;
                        //Valida que existe una Unidad de Empaque
                        if (!string.IsNullOrEmpty(det.DetalleDocu.inReferenciaID.Value) && string.IsNullOrEmpty(det.DetalleDocu.UnidadEmpaque.Value))
                        {
                            DTO_inReferencia refer = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.DetalleDocu.inReferenciaID.Value, true);
                            DTO_inEmpaque emp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, refer.EmpaqueInvID.Value, true);
                            det.DetalleDocu.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                            det.DetalleDocu.UnidadEmpaque.Value = emp.UnidadInvID.Value;
                        }
                        //Valida si las unidades de medida son diferentes
                        if (!string.IsNullOrEmpty(det.DetalleDocu.inReferenciaID.Value) && det.DetalleDocu.UnidadInvID.Value == det.DetalleDocu.UnidadEmpaque.Value)
                        {
                            if (string.IsNullOrEmpty(det.DetalleDocu.EmpaqueInvID.Value))
                            {
                                DTO_inReferencia refer = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.DetalleDocu.inReferenciaID.Value, true);
                                DTO_inEmpaque emp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, refer.EmpaqueInvID.Value, true);
                                det.DetalleDocu.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                                det.DetalleDocu.CantidadxEmpaque.Value = emp.Cantidad.Value;
                                det.DetalleDocu.CantEmpaque.Value = emp.Cantidad.Value > 1 ? Math.Ceiling(Math.Abs(det.DetalleDocu.CantidadSol.Value.Value) / emp.Cantidad.Value.Value) : 1;
                                det.DetalleDocu.CantidadOC.Value =  det.DetalleDocu.CantidadxEmpaque.Value != 1 ? 
                                                                    det.DetalleDocu.CantidadxEmpaque.Value * det.DetalleDocu.CantEmpaque.Value : Math.Abs(det.DetalleDocu.CantidadSol.Value.Value);
                            }
                            else
                            {
                                det.DetalleDocu.CantEmpaque.Value = det.DetalleDocu.CantEmpaque.Value ?? 1;
                                det.DetalleDocu.CantidadOC.Value =  det.DetalleDocu.CantidadxEmpaque.Value != 1 ? 
                                                                    det.DetalleDocu.CantidadxEmpaque.Value * det.DetalleDocu.CantEmpaque.Value : Math.Abs(det.DetalleDocu.CantidadSol.Value.Value);
                            }
                        }

                     }
                    #region Obtiene los Precios por proveedor si existen(No borrar)
                    index = 0;
                    //foreach (var det in this.data.Footer)
                    //{
                        //Dictionary<string, string> pks = new Dictionary<string, string>();
                        //pks.Add("ProveedorID", this.data.HeaderOrdenCompra.ProveedorID.Value);
                        //pks.Add("inReferenciaID", det.DetalleDocu.inReferenciaID.Value);
                        //DTO_prProveedorPrecio precio = (DTO_prProveedorPrecio)this._bc.GetMasterComplexDTO(AppMasters.prProveedorPrecio, pks, true);
                        //if(this._tipoMonedaOr == TipoMoneda.Local)
                        //    det.DetalleDocu.ValorUni.Value = precio != null ? precio.ValorLocal.Value : det.DetalleDocu.ValorUni.Value;
                        //else if (this._tipoMonedaOr == TipoMoneda.Foreign)
                        //    det.DetalleDocu.ValorUni.Value = precio != null ? precio.ValorExtra.Value : det.DetalleDocu.ValorUni.Value;
                        //else
                        //{
                        //    if (precio != null)
                        //        det.DetalleDocu.ValorUni.Value = this.monedaOrden == this.monedaLocal ? precio.ValorLocal.Value : precio.ValorExtra.Value;
                        //    else
                        //        det.DetalleDocu.ValorUni.Value = det.DetalleDocu.ValorUni.Value;                          
                        //}      
                        //this.CalculateValues(index);
                        //index++;
                    //}
                    this.EnableFooter(true);
                    this.gvDocument.Focus();
                    #endregion                       
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "btnSolicitudes_Click"));
            }
        }

        /// <summary>
        /// Trae solicitudes asignados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnEstadoEjecucion_Click(object sender, EventArgs e)
        {
            if (this.gvDocument.FocusedRowHandle >= 0)
            {
                this._currentRow = (DTO_prOrdenCompraFooter)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (this._currentRow != null)
                {
                    string recursoID = !string.IsNullOrEmpty(this._currentRow.DetalleDocu.inReferenciaID.Value)?this._currentRow.DetalleDocu.inReferenciaID.Value :this._currentRow.DetalleDocu.CodigoBSID.Value;
                    if (this._currentRow.DetalleDocu.Documento4ID.Value.HasValue)
                        this.ViewTrazabilidad(this._currentRow.DetalleDocu.Documento4ID.Value, this._currentRow.DetalleDocu.Detalle4ID.Value, recursoID); 
                }
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterEmpaque_Leave(object sender, EventArgs e)
        {
            #region Codigo Empaque
            if (this.masterEmpaque.ValidID && this._currentRow != null && !string.IsNullOrEmpty(this._currentRow.DetalleDocu.inReferenciaID.Value))
            {
                DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, this.masterEmpaque.Value, true);
                DTO_inUnidad unidadEmp = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, empaqueEmp.UnidadInvID.Value, true);
                DTO_inUnidad unidadRef = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, this._currentRow.DetalleDocu.UnidadInvID.Value, true);
                decimal cantEmp = Convert.ToDecimal(this.txtCantEmpaque.EditValue, CultureInfo.InvariantCulture);

                if (unidadEmp.ID.Value != unidadRef.ID.Value)
                {
                    #region Verifica el Tipo de Medida de la Unidad
                    if (unidadEmp.TipoMedida.Value == unidadRef.TipoMedida.Value)
                        this._currentRow.DetalleDocu.EmpaqueInvID.Value = this.masterEmpaque.Value;
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UnitInvalidEmp));
                        this.masterEmpaque.Value = string.Empty;
                        this.masterEmpaque.Focus();
                        return;
                    }
                    #region Realiza la conversion de unidad si es necesario
                    //DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, this.masterEmpaque.Value, true);
                    //this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);
                    //if (empaqueEmp.UnidadInvID.Value == this._referenciaInv.UnidadInvID.Value)
                    //{
                    //    decimal tmp = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                    //    this._unidadesFinalxRef = cantidadEmp * empaqueEmp.Cantidad.Value.Value;
                    //}
                    //else
                    //{
                    //    if (!empaqueEmp.UnidadInvID.Value.Equals(this._empaqueInvIdDef))
                    //    {
                    //        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                    //        keysConvert.Add("UnidadInvID", empaqueEmp.UnidadInvID.Value);
                    //        keysConvert.Add("UnidadBase", this._referenciaInv.UnidadInvID.Value);
                    //        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                    //        if (conversion != null)
                    //            this._unidadesFinalxRef = (conversion.Factor.Value.Value * cantidadEmp * empaqueEmp.Cantidad.Value.Value);
                    //        else
                    //        {
                    //            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistConvertUnit));
                    //            ctrl.EditValue = this._serializadoInd ? 1 : 0;
                    //            return false;
                    //        }
                    //    }
                    //    else
                    //        this._unidadesFinalxRef = cantidadEmp;
                    //}
                    #endregion
                    #endregion                 
                }
                else
                    this._currentRow.DetalleDocu.EmpaqueInvID.Value = this.masterEmpaque.Value;

                this._currentRow.DetalleDocu.CantidadxEmpaque.Value = empaqueEmp.Cantidad.Value;
                this._currentRow.DetalleDocu.CantEmpaque.Value = empaqueEmp.Cantidad.Value > 1 ? Math.Ceiling(Math.Abs(this._currentRow.DetalleDocu.CantidadSol.Value.Value) / empaqueEmp.Cantidad.Value.Value) : 1;
                this._currentRow.DetalleDocu.CantidadOC.Value = this._currentRow.DetalleDocu.CantidadxEmpaque.Value != 1 ?
                                                                this._currentRow.DetalleDocu.CantidadxEmpaque.Value * this._currentRow.DetalleDocu.CantEmpaque.Value : Math.Abs(this._currentRow.DetalleDocu.CantidadSol.Value.Value);

                this.CalculateValues(this._currentRow.DetalleDocu.Index);
                this.LoadEditGridData(false, this._currentRow.DetalleDocu.Index);
            }
            #endregion
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
                    this.deleteOP = true;
                    this.valorTotalDoc = 0;
                    this.valorIVATotalDoc = 0;
                    this.CleanFooter();
                    this.EnableFooter(false);                    
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
                this.cacheBienServ = new Dictionary<string, Tuple<DTO_prBienServicio, decimal>>();
                this.cacheBienServClase = new Dictionary<string, decimal>();
                this.cacheConCargo = new Dictionary<string, decimal>();
                this.cacheCuenta = new Dictionary<string, decimal>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "TBNew"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "TBCopy"));
            }
        }

        #endregion        

    }
}
