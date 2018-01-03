using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using SentenceTransformer;

namespace NewAge.Negocio
{
    public class ModuloFacturacion : ModuloBase
    {
        #region Variables
        #region Dals

        private DAL_faFacturaDocu _dal_faFacturaDocu = null;
        private DAL_tsBancosDocu _dal_tsBancosDocu = null;
        private DAL_tsBancosCuenta _dal_tsBancosCta = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_ReportesFacturacion _dal_ReportesFacturacion = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_pyActaEntregaDeta _dal_actasEntrega = null;
        #endregion
        #region Modulos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloActivosFijos _moduloActivosFijos = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloInventarios _moduloInventarios = null;
        private ModuloProyectos _moduloProy = null;

        #endregion
        #endregion

        /// <summary>
        /// Constructor Módulo Facturacion
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tx"></param>
        /// <param name="emp"></param>
        /// <param name="userID"></param>
        public ModuloFacturacion(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Factura Venta

        #region Funciones Privadas

        /// <summary>
        /// Consulta una tabla faFacturaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la factura</param>
        /// <returns></returns>
        public DTO_faFacturaDocu faFacturaDocu_Get(int NumeroDoc)
        {
            this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_faFacturaDocu.DAL_faFacturaDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Adiciona en la tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">DTO_faFacturaDocu</param>
        /// <returns></returns>
        private DTO_TxResult faFacturaDocu_Add(DTO_faFacturaDocu fact, int documentID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_faFacturaDocu.DAL_faFacturaDocu_Add(fact);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, fact.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "faFacturaDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// ALlena DTO_faFacturacionFooter usando valores de DTO_glMovimientoDeta y DTO_faFacturaDocu
        /// </summary>
        /// <param name="docCtrl">glDocumentoControl</param>
        /// <param name="header">faFacturaDocu</param>
        /// <param name="movDet">List of DTO_glMovimientoDeta</param>
        /// <returns></returns>
        private List<DTO_faFacturacionFooter> faFacturaFooter_Load(DTO_glDocumentoControl docCtrl, DTO_faFacturaDocu header, List<DTO_glMovimientoDeta> movDet)
        {
            try
            {
                #region Variables
                this._moduloProy = (ModuloProyectos)base.GetInstance(typeof(ModuloProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                List<DTO_faFacturacionFooter> footer = new List<DTO_faFacturacionFooter>();
                Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<int, DTO_acActivoControl> cacheActivo = new Dictionary<int, DTO_acActivoControl>();
                Dictionary<string, DTO_faServicios> cacheServicio = new Dictionary<string, DTO_faServicios>();
                Dictionary<string, DTO_faConceptos> cacheConcepto = new Dictionary<string, DTO_faConceptos>();
                Dictionary<string, DTO_coTercero> cacheTerceroEmp = new Dictionary<string, DTO_coTercero>();
                Dictionary<string, DTO_coTercero> cacheTercero = new Dictionary<string, DTO_coTercero>();
                DTO_coPlanCuenta _planCuenta;
                DTO_faServicios _servicio;
                DTO_faConceptos _concepto;
                DTO_coTercero _regFiscalEmp;
                DTO_coTercero _refFiscalTercero;
                decimal _porcIVA = 0;
                decimal _porcRIVA = 0;
                decimal _porcRFT = 0;
                string codigoIVA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoIVA);
                string codigoRFT = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteFuente);
                string codigoReteIVA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteIVA);
                string codigoReteICA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoICA);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal _porcICA =header.Porcentaje1.Value.HasValue? header.Porcentaje1.Value.Value : 0;

                #endregion
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                foreach (DTO_glMovimientoDeta mvto in movDet)
                {
                    DTO_faFacturacionFooter footerItem = new DTO_faFacturacionFooter();
                    footerItem.Movimiento = mvto;
                    footerItem.Index = mvto.Index;
                    
                    #region Obtiene porcentajes de los impuestos
                    #region Carga datos para filtrar la tabla coImpuesto
                    #region Carga el Servicio
                    if (cacheServicio.ContainsKey(footerItem.Movimiento.ServicioID.Value))
                        _servicio = cacheServicio[footerItem.Movimiento.ServicioID.Value];
                    else
                    {
                        _servicio = (DTO_faServicios)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, footerItem.Movimiento.ServicioID.Value, true, false);
                        cacheServicio.Add(footerItem.Movimiento.ServicioID.Value, _servicio);
                    }
                    #endregion
                    #region Carga el Concepto
                    if (cacheConcepto.ContainsKey(_servicio.ConceptoIngresoID.Value))
                        _concepto = cacheConcepto[_servicio.ConceptoIngresoID.Value];
                    else
                    {
                        _concepto = (DTO_faConceptos)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, _servicio.ConceptoIngresoID.Value, true, false);
                        cacheConcepto.Add(_servicio.ConceptoIngresoID.Value, _concepto);
                    } 
                    #endregion
                    #region Carga el Tercero de la Empresa
                    if (cacheTerceroEmp.ContainsKey(terceroPorDefecto))
                        _regFiscalEmp = cacheTerceroEmp[terceroPorDefecto];
                    else
                    {
                        _regFiscalEmp = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                        cacheTerceroEmp.Add(terceroPorDefecto, _regFiscalEmp);
                    }
                    #endregion
                    #region Carga el Tercero
                    if (cacheTercero.ContainsKey(footerItem.Movimiento.TerceroID.Value))
                        _refFiscalTercero = cacheTercero[footerItem.Movimiento.TerceroID.Value];
                    else
                    {
                        _refFiscalTercero = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, footerItem.Movimiento.TerceroID.Value, true, false);
                        cacheTercero.Add(footerItem.Movimiento.TerceroID.Value, _refFiscalTercero);
                    }
                    #endregion
                    #endregion
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
                    filtro.ValorFiltro = _refFiscalTercero.ReferenciaID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    filtro = new DTO_glConsultaFiltro();
                    filtro.CampoFisico = "ConceptoCargoID";
                    filtro.ValorFiltro = _concepto.ConceptoCargoID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    filtro = new DTO_glConsultaFiltro();
                    filtro.CampoFisico = "LugarGeograficoID";
                    filtro.ValorFiltro = docCtrl.LugarGeograficoID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    if (this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_IndContabilizaRetencionFactura) != "1")
                    {
                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ImpuestoTipoID";
                        filtro.ValorFiltro = codigoIVA;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);
                    }

                    consulta.Filtros = filtros;
                    #endregion
                    #region Carga la tabla coImpuesto
                    _dal_MasterComplex.DocumentID = AppMasters.coImpuesto;
                    long count = _dal_MasterComplex.DAL_MasterComplex_Count(consulta, true);
                    List<DTO_MasterComplex> complexImpuesto = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, consulta, true).ToList();
                    List<DTO_coImpuesto> masterImpuesto = complexImpuesto.Cast<DTO_coImpuesto>().ToList();
                    #endregion
                    #region Obtenga los porcentajes de Retenciones
                    foreach (DTO_coImpuesto coImp in masterImpuesto)
                    {
                        if (cacheCuenta.ContainsKey(coImp.CuentaID.Value))
                            _planCuenta = cacheCuenta[coImp.CuentaID.Value];
                        else
                        {
                            _planCuenta = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, coImp.CuentaID.Value, true, false);
                            cacheCuenta.Add(coImp.CuentaID.Value, _planCuenta);
                        }
                        if (coImp.ImpuestoTipoID.Value == codigoIVA)
                        {
                            _porcIVA =  _planCuenta.ImpuestoPorc.Value.HasValue? _planCuenta.ImpuestoPorc.Value.Value : 0;
                            #region Obtiene ReteIVA
                            Dictionary<string, string> keyReteIva = new Dictionary<string, string>();
                            keyReteIva.Add("EmpresaGrupoID", this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coIVARetencion, this.Empresa, egCtrl));
                            keyReteIva.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                            keyReteIva.Add("RegimenFiscalTerceroID", _refFiscalTercero.ReferenciaID.Value);
                            keyReteIva.Add("CuentaIVA", _planCuenta.ID.Value);
                            this._dal_MasterComplex.DocumentID = AppMasters.coIVARetencion;
                            DTO_coIvaRetencion reteIVA = (DTO_coIvaRetencion)this._dal_MasterComplex.DAL_MasterComplex_GetByID(keyReteIva, true);
                            if (reteIVA != null)
                            {
                                DTO_coPlanCuenta ctaReteIVA = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, reteIVA.CuentaReteIVA.Value, true, false);
                                _porcRIVA = ctaReteIVA.ImpuestoPorc.Value.HasValue? ctaReteIVA.ImpuestoPorc.Value.Value : 0;
                            }
                            #endregion
                        }
                        if (_planCuenta.Naturaleza.Value.Value != (byte)NaturalezaCuenta.Credito)//Discrimina si es IVA(Crédito)
                        {
                            if (coImp.ImpuestoTipoID.Value == codigoRFT)
                                _porcRFT = _planCuenta.ImpuestoPorc.Value.HasValue? _planCuenta.ImpuestoPorc.Value.Value : 0;
                            else if (coImp.ImpuestoTipoID.Value == codigoReteIVA && _porcRIVA == 0)
                                _porcRIVA = _planCuenta.ImpuestoPorc.Value.HasValue? _planCuenta.ImpuestoPorc.Value.Value : 0;
                        }
                    }
                    #endregion
                    #endregion
                    #region Calcular Valores
                    decimal vlrBruto = footerItem.Movimiento.CantidadUNI.Value.Value * footerItem.Movimiento.ValorUNI.Value.Value;
                    decimal vlrIva = Math.Round((_porcIVA / 100) * vlrBruto,0);
                    decimal vlrRiva = Math.Round((_porcRIVA / 100) * vlrBruto, 0);
                    decimal vlrIca = Math.Round((_porcICA / 1000) * vlrBruto,0);
                    decimal vlrRft = Math.Round((_porcRFT / 100) * vlrBruto,0);
                    #endregion
                    #region Asignar Valores
                    footerItem.PorcIVA = _porcIVA;
                    footerItem.PorcRIVA = _porcRIVA;
                    footerItem.PorcRFT = _porcRFT;
                    footerItem.ValorBruto = docCtrl.MonedaID.Value == monedaLocal ? mvto.Valor1LOC.Value.Value : mvto.Valor1EXT.Value.Value;
                    footerItem.ValorIVA = docCtrl.MonedaID.Value == monedaLocal ? (mvto.Valor2LOC.Value == 0? Math.Round((_porcIVA/100) * vlrBruto, 0): mvto.Valor2LOC.Value.Value) : mvto.Valor2EXT.Value.Value;
                    footerItem.ValorRIVA = vlrRiva;
                    footerItem.ValorRICA = vlrIca;
                    footerItem.ValorRFT = vlrRft;

                    footerItem.ValorRetenciones = vlrRiva + vlrIca + vlrRft; 
                    footerItem.ValorTotal = vlrBruto + vlrIva;
                    footerItem.ValorNeto = footerItem.ValorTotal - vlrRiva - vlrIca - vlrRft;;
                    #endregion
                    #region Obtiene la info de Proyectos si se requiere
                    if (mvto.DocSoporte.Value.HasValue)
                    {
                        DTO_pyProyectoTarea tarea = this._moduloProy.pyProyectoTarea_GetByConsecutivo(mvto.DocSoporte.Value.Value);
                        if (tarea != null)
                        {
                            mvto.TareaID.Value = tarea.TareaID.Value;
                            mvto.DescriptivoTarea.Value = tarea.Descriptivo.Value;
                        }
                    }
                    #endregion

                    footer.Add(footerItem);
                }
                return footer;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "faFacturaFooter_Load");
                return new List<DTO_faFacturacionFooter>();
            }

        }

        /// <summary>
        /// Llena el Comprobante con los datos de la factura
        /// </summary>
        /// <param name="ctrl">glDocumentoControl</param>
        /// <param name="header">faFacturaDocu</param>
        /// <param name="footer">List of DTO_faFacturacionFooter</param>
        /// <returns></returns>
        private List<DTO_ComprobanteFooter> faComprobanteFooter_Load(DTO_glDocumentoControl ctrl, DTO_faFacturaDocu header, List<DTO_faFacturacionFooter> footer)
        {
            try
            {
                #region Variables
                this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_ComprobanteFooter> compFooter = new List<DTO_ComprobanteFooter>();

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                string codigoIVA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoIVA);
                string codigoRFT = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteFuente);
                string codigoReteIVA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoReteIVA);
                string codigoReteICA = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CodigoICA);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                decimal sign = (ctrl.DocumentoID.Value.Value == AppDocuments.NotaCredito) ? 1 : -1;

                decimal _porcICA = header.Porcentaje1.Value.Value;
                //decimal _porcRemesa = header.Porcentaje2.Value.Value;
                decimal _valorTotal = 0;
                //DTO_coPlanCuenta _cuentaRemesaInfo = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CuentaRemesa), true, false);

                DTO_coPlanCuenta _planCuentaImp;
                DTO_coPlanCuenta _planCuentaServ;
                DTO_faServicios _servicio;
                DTO_faConceptos _concepto;
                DTO_coTercero _regFiscalEmp;
                DTO_coTercero _refFiscalTercero;

                List<Tuple<string, string, string, decimal, decimal>> listImpuestos = new List<Tuple<string, string, string, decimal, decimal>>();
                Dictionary<string, DTO_coPlanCuenta> cacheCuentaImp = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, DTO_coPlanCuenta> cacheCuentaServ = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, DTO_faServicios> cacheServicio = new Dictionary<string, DTO_faServicios>();
                Dictionary<string, DTO_faConceptos> cacheConcepto = new Dictionary<string, DTO_faConceptos>();
                Dictionary<string, DTO_coTercero> cacheTerceroEmp = new Dictionary<string, DTO_coTercero>();
                Dictionary<string, DTO_coTercero> cacheTercero = new Dictionary<string, DTO_coTercero>();
                #endregion
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                foreach (DTO_faFacturacionFooter footerItem in footer.FindAll(x=>x.ValorBruto > 0))
                {
                    #region Obtiene datos de los impuestos
                    #region Carga datos para filtrar coImpuesto
                    #region Carga el Servicio y el Concepto
                    if (cacheServicio.ContainsKey(footerItem.Movimiento.ServicioID.Value))
                        _servicio = cacheServicio[footerItem.Movimiento.ServicioID.Value];
                    else
                    {
                        _servicio = (DTO_faServicios)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, footerItem.Movimiento.ServicioID.Value, true, false);
                        cacheServicio.Add(footerItem.Movimiento.ServicioID.Value, _servicio);
                    }

                    if (cacheConcepto.ContainsKey(_servicio.ConceptoIngresoID.Value))
                        _concepto = cacheConcepto[_servicio.ConceptoIngresoID.Value];
                    else
                    {
                        _concepto = (DTO_faConceptos)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, _servicio.ConceptoIngresoID.Value, true, false);
                        cacheConcepto.Add(_servicio.ConceptoIngresoID.Value, _concepto);
                    }

                    if (cacheCuentaServ.ContainsKey(_concepto.CuentaID.Value))
                        _planCuentaServ = cacheCuentaServ[_concepto.CuentaID.Value];
                    else
                    {
                        _planCuentaServ = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, _concepto.CuentaID.Value, true, false);
                        cacheCuentaServ.Add(_concepto.CuentaID.Value, _planCuentaServ);
                    }
                    #endregion
                    #region Carga el Tercero de la Empresa
                    if (cacheTerceroEmp.ContainsKey(terceroPorDefecto))
                        _regFiscalEmp = cacheTerceroEmp[terceroPorDefecto];
                    else
                    {
                        _regFiscalEmp = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                        cacheTerceroEmp.Add(terceroPorDefecto, _regFiscalEmp);
                    }
                    #endregion
                    #region Carga el Tercero
                    if (cacheTercero.ContainsKey(footerItem.Movimiento.TerceroID.Value))
                        _refFiscalTercero = cacheTercero[footerItem.Movimiento.TerceroID.Value];
                    else
                    {
                        _refFiscalTercero = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, footerItem.Movimiento.TerceroID.Value, true, false);
                        cacheTercero.Add(footerItem.Movimiento.TerceroID.Value, _refFiscalTercero);
                    }
                    #endregion
                    #endregion
                    #region Carga la tabla coImpuesto
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
                    filtro.ValorFiltro = _refFiscalTercero.ReferenciaID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    filtro = new DTO_glConsultaFiltro();
                    filtro.CampoFisico = "ConceptoCargoID";
                    filtro.ValorFiltro = _concepto.ConceptoCargoID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    filtro = new DTO_glConsultaFiltro();
                    filtro.CampoFisico = "LugarGeograficoID";
                    filtro.ValorFiltro = ctrl.LugarGeograficoID.Value;
                    filtro.OperadorFiltro = OperadorFiltro.Igual;
                    filtro.OperadorSentencia = "and";
                    filtros.Add(filtro);

                    if (this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_IndContabilizaRetencionFactura) != "1")
                    {
                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ImpuestoTipoID";
                        filtro.ValorFiltro = codigoIVA;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);
                    }

                    consulta.Filtros = filtros;
                    #endregion

                    this._dal_MasterComplex.DocumentID = AppMasters.coImpuesto;
                    long count = _dal_MasterComplex.DAL_MasterComplex_Count(consulta, true);
                    List<DTO_MasterComplex> complexImpuesto = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, consulta, true).ToList();
                    List<DTO_coImpuesto> masterImpuesto = complexImpuesto.Cast<DTO_coImpuesto>().ToList();
                    #endregion
                    #region Obtiene datos de los Retenciones
                    foreach (DTO_coImpuesto coImp in masterImpuesto)
                    {
                        if (cacheCuentaImp.ContainsKey(coImp.CuentaID.Value))
                            _planCuentaImp = cacheCuentaImp[coImp.CuentaID.Value];
                        else
                        {
                            _planCuentaImp = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, coImp.CuentaID.Value, true, false);
                            cacheCuentaImp.Add(coImp.CuentaID.Value, _planCuentaImp);
                        }
                        if (coImp.ImpuestoTipoID.Value == codigoIVA && footerItem.ValorIVA != 0)
                        {
                            if (ctrl.DocumentoID.Value == AppDocuments.NotaCredito)
                                #region Revisa si tiene Cuenta de Anulacion y lo asigna
                                if (!string.IsNullOrEmpty(_planCuentaImp.CuentaAnulaID.Value.ToString()))
                                    listImpuestos.Add(Tuple.Create(_planCuentaImp.CuentaAnulaID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorIVA * sign));
                                else
                                    listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorIVA * sign));
                                #endregion
                            else
                                listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorIVA * sign));

                            #region Obtiene ReteIVA
                            Dictionary<string, string> keyReteIva = new Dictionary<string, string>();
                            keyReteIva.Add("EmpresaGrupoID", this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coIVARetencion, this.Empresa, egCtrl));
                            keyReteIva.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                            keyReteIva.Add("RegimenFiscalTerceroID", _refFiscalTercero.ReferenciaID.Value);
                            keyReteIva.Add("CuentaIVA", _planCuentaImp.ID.Value);
                            this._dal_MasterComplex.DocumentID = AppMasters.coIVARetencion;
                            DTO_coIvaRetencion reteIVA = (DTO_coIvaRetencion)this._dal_MasterComplex.DAL_MasterComplex_GetByID(keyReteIva, true);
                            if (reteIVA != null)
                            {
                                DTO_coPlanCuenta ctaReteIVA = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, reteIVA.CuentaReteIVA.Value, true, false);
                                if (ctrl.DocumentoID.Value == AppDocuments.NotaCredito)
                                    #region Revisa si tiene Cuenta de Anulacion y lo asigna
                                    if (!string.IsNullOrEmpty(ctaReteIVA.CuentaAnulaID.Value.ToString()))
                                        listImpuestos.Add(Tuple.Create(ctaReteIVA.CuentaAnulaID.Value, ctaReteIVA.ConceptoSaldoID.Value, ctaReteIVA.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorIVA * sign));
                                    else
                                        listImpuestos.Add(Tuple.Create(ctaReteIVA.ID.Value, ctaReteIVA.ConceptoSaldoID.Value, ctaReteIVA.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorIVA * sign));
                                    #endregion
                                else
                                    listImpuestos.Add(Tuple.Create(ctaReteIVA.ID.Value, ctaReteIVA.ConceptoSaldoID.Value, ctaReteIVA.ImpuestoTipoID.Value, footerItem.ValorBruto, footerItem.ValorRIVA));
                            }
                            #endregion
                        }
                        if (_planCuentaImp.Naturaleza.Value.Value != (byte)NaturalezaCuenta.Credito)//Discrimina si es IVA(Crédito)
                        {
                            if (coImp.ImpuestoTipoID.Value == codigoRFT && footerItem.ValorRFT != 0)
                            {
                                if (ctrl.DocumentoID.Value == AppDocuments.NotaCredito)
                                    #region Revisa si tiene Cuenta de Anulacion y lo asigna
                                    if (!string.IsNullOrEmpty(_planCuentaImp.CuentaAnulaID.Value.ToString()))
                                        listImpuestos.Add(Tuple.Create(_planCuentaImp.CuentaAnulaID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorRFT * sign));
                                    else
                                        listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorRFT * sign));
                                    #endregion
                                else
                                    listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto, footerItem.ValorRFT)); //footerItem.ValorBruto * (-1) * sign
                            }
                            else if (coImp.ImpuestoTipoID.Value == codigoReteIVA && footerItem.ValorRIVA != 0)
                            {
                                if (!listImpuestos.Exists(x=>x.Item3 == coImp.ImpuestoTipoID.Value))
                                {
                                    if (ctrl.DocumentoID.Value == AppDocuments.NotaCredito)
                                        #region Revisa si tiene Cuenta de Anulacion y lo asigna
                                        if (!string.IsNullOrEmpty(_planCuentaImp.CuentaAnulaID.Value.ToString()))
                                            listImpuestos.Add(Tuple.Create(_planCuentaImp.CuentaAnulaID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorIVA * sign, footerItem.ValorRIVA * sign));
                                        else
                                            listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorIVA * sign, footerItem.ValorRIVA * sign));
                                        #endregion
                                    else
                                        listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto, footerItem.ValorRIVA)); //footerItem.ValorBruto * (-1) * sign
                                    
                                }
                            }
                            else if (coImp.ImpuestoTipoID.Value == codigoReteICA && footerItem.ValorRICA != 0)
                            {
                                if (ctrl.DocumentoID.Value == AppDocuments.NotaCredito)
                                    #region Revisa si tiene Cuenta de Anulacion y lo asigna
                                    if (!string.IsNullOrEmpty(_planCuentaImp.CuentaAnulaID.Value.ToString()))
                                        listImpuestos.Add(Tuple.Create(_planCuentaImp.CuentaAnulaID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorRICA * sign));
                                    else
                                        listImpuestos.Add(Tuple.Create(_planCuentaImp.ID.Value, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto * sign, footerItem.ValorRICA * sign));
                                    #endregion
                                else
                                    listImpuestos.Add(Tuple.Create(header.CuentaRteICA, _planCuentaImp.ConceptoSaldoID.Value, _planCuentaImp.ImpuestoTipoID.Value, footerItem.ValorBruto, footerItem.ValorRICA)); //footerItem.ValorBruto * (-1) * sign
                            }
                        }
                    }

                    #endregion
                    #region Crea el registro contable de Valor Bruto
                    DTO_coPlanCuenta cuentaVlrBruto = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, _concepto.CuentaID.Value, true, false);
                    DTO_ComprobanteFooter compFooterValorBruto = new DTO_ComprobanteFooter();
                    compFooterValorBruto.Index = compFooter.Count;
                    compFooterValorBruto.CuentaID.Value = _concepto.CuentaID.Value;
                    compFooterValorBruto.TerceroID.Value = footerItem.Movimiento.TerceroID.Value;
                    compFooterValorBruto.ProyectoID.Value = cuentaVlrBruto.ProyectoInd.Value.Value? footerItem.Movimiento.ProyectoID.Value : ctrl.ProyectoID.Value;
                    compFooterValorBruto.CentroCostoID.Value = cuentaVlrBruto.CentroCostoInd.Value.Value ? footerItem.Movimiento.CentroCostoID.Value : ctrl.CentroCostoID.Value;
                    compFooterValorBruto.LineaPresupuestoID.Value = cuentaVlrBruto.LineaPresupuestalInd.Value.Value ? footerItem.Movimiento.LineaPresupuestoID.Value : ctrl.LineaPresupuestoID.Value;

                    compFooterValorBruto.ConceptoCargoID.Value = cuentaVlrBruto.ConceptoCargoInd.Value.Value?_concepto.ConceptoCargoID.Value : this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                    compFooterValorBruto.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                    compFooterValorBruto.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                    compFooterValorBruto.DocumentoCOM.Value = ctrl.DocumentoNro.Value.Value.ToString(); 
                    compFooterValorBruto.ActivoCOM.Value = string.Empty;
                    compFooterValorBruto.ConceptoSaldoID.Value = _planCuentaServ.ConceptoSaldoID.Value;
                    compFooterValorBruto.IdentificadorTR.Value = ctrl.NumeroDoc.Value.Value;
                    compFooterValorBruto.Descriptivo.Value = ctrl.Descripcion.Value;
                    compFooterValorBruto.TasaCambio.Value = ctrl.TasaCambioCONT.Value;
                    compFooterValorBruto.IdentificadorTR.Value = header.NumeroDoc.Value;

                    compFooterValorBruto.vlrBaseML.Value = 0;
                    compFooterValorBruto.vlrBaseME.Value = 0;
                    if (monedaLocal == ctrl.MonedaID.Value)
                    {
                        compFooterValorBruto.vlrMdaLoc.Value = footerItem.ValorBruto * sign;
                        compFooterValorBruto.vlrMdaExt.Value = (compFooterValorBruto.TasaCambio.Value.Value == 0) ? 0 : Math.Round(compFooterValorBruto.vlrMdaLoc.Value.Value / compFooterValorBruto.TasaCambio.Value.Value,2);
                        compFooterValorBruto.vlrMdaOtr.Value = compFooterValorBruto.vlrMdaLoc.Value;
                    }
                    else
                    {
                        compFooterValorBruto.vlrMdaExt.Value = footerItem.ValorBruto * sign;
                        compFooterValorBruto.vlrMdaLoc.Value = compFooterValorBruto.vlrMdaExt.Value * compFooterValorBruto.TasaCambio.Value.Value;
                        compFooterValorBruto.vlrMdaOtr.Value = compFooterValorBruto.vlrMdaExt.Value;
                    };
                    _valorTotal += compFooterValorBruto.vlrMdaLoc.Value.Value;
                    compFooter.Add(compFooterValorBruto);

                    #endregion
                    #region Crea registros contables de Impuestos
                    foreach (var impuesto in listImpuestos)
                    {
                        _planCuentaImp = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, impuesto.Item1, true, false);
                        
                        DTO_ComprobanteFooter compFooterImpItem = new DTO_ComprobanteFooter();
                        compFooterImpItem.TasaCambio.Value = ctrl.TasaCambioDOCU.Value;
                        compFooterImpItem.TerceroID.Value = footerItem.Movimiento.TerceroID.Value;
                        compFooterImpItem.DocumentoCOM.Value = ctrl.DocumentoNro.Value.Value.ToString(); 
                        compFooterImpItem.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                        compFooterImpItem.ProyectoID.Value = _planCuentaImp.ProyectoInd.Value.Value ? footerItem.Movimiento.ProyectoID.Value : ctrl.ProyectoID.Value;
                        compFooterImpItem.CentroCostoID.Value = _planCuentaImp.CentroCostoInd.Value.Value ? footerItem.Movimiento.CentroCostoID.Value : ctrl.CentroCostoID.Value;
                        compFooterImpItem.LineaPresupuestoID.Value = _planCuentaImp.LineaPresupuestalInd.Value.Value ? footerItem.Movimiento.LineaPresupuestoID.Value : ctrl.LineaPresupuestoID.Value;
                        compFooterImpItem.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                        compFooterImpItem.CuentaID.Value = impuesto.Item1;
                        compFooterImpItem.DatoAdd1.Value = (impuesto.Item3 != null && impuesto.Item3 == codigoIVA) ? AuxiliarDatoAdd1.IVA.ToString() : string.Empty;

                        compFooterImpItem.Descriptivo.Value = ctrl.Descripcion.Value;
                        compFooterImpItem.ConceptoSaldoID.Value = impuesto.Item2;
                        compFooterImpItem.ConceptoCargoID.Value = _concepto.ConceptoCargoID.Value; 
                        compFooterImpItem.IdentificadorTR.Value = header.NumeroDoc.Value;

                        if (monedaLocal == ctrl.MonedaID.Value)
                        {
                            compFooterImpItem.vlrBaseML.Value = impuesto.Item4;
                            compFooterImpItem.vlrBaseME.Value = (compFooterImpItem.TasaCambio.Value.Value == 0) ? 0 : Math.Round(compFooterImpItem.vlrBaseML.Value.Value / compFooterImpItem.TasaCambio.Value.Value,2);
                            compFooterImpItem.vlrMdaLoc.Value = impuesto.Item5;
                            compFooterImpItem.vlrMdaExt.Value = (compFooterImpItem.TasaCambio.Value.Value == 0) ? 0 : Math.Round(compFooterImpItem.vlrMdaLoc.Value.Value / compFooterImpItem.TasaCambio.Value.Value,2);
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaLoc.Value;
                        }
                        else
                        {
                            compFooterImpItem.vlrBaseME.Value = impuesto.Item4;
                            compFooterImpItem.vlrBaseML.Value = compFooterImpItem.vlrBaseME.Value.Value * compFooterImpItem.TasaCambio.Value.Value;
                            compFooterImpItem.vlrMdaExt.Value = impuesto.Item5;
                            compFooterImpItem.vlrMdaLoc.Value = compFooterImpItem.vlrMdaExt.Value.Value * compFooterImpItem.TasaCambio.Value.Value;
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaExt.Value;
                        }

                        compFooter.Add(compFooterImpItem);
                    }
                    _valorTotal += listImpuestos.Sum(x => x.Item5);
                    listImpuestos.Clear();
                    #endregion                    
                    #endregion
                }
                #region Crea registro de Anticipo
                if (header.ValorAnticipo != 0)
                {
                    string ctaAnticipo = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CuentaAnticiposMdaLocal);
                    string conceptoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaAnticipo, true, false);
                    if (cta != null)
                    {
                        DTO_ComprobanteFooter compFooterImpItem = new DTO_ComprobanteFooter();
                        compFooterImpItem.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                        compFooterImpItem.ProyectoID.Value = ctrl.ProyectoID.Value;
                        compFooterImpItem.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                        compFooterImpItem.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                        compFooterImpItem.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                        compFooterImpItem.TerceroID.Value = ctrl.TerceroID.Value;
                        compFooterImpItem.TasaCambio.Value = ctrl.TasaCambioDOCU.Value;                       
                        compFooterImpItem.DocumentoCOM.Value = ctrl.DocumentoNro.Value.ToString();                      
                        compFooterImpItem.CuentaID.Value = ctaAnticipo;

                        compFooterImpItem.Descriptivo.Value = "Abona Anticipo"; 
                        compFooterImpItem.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
                        compFooterImpItem.ConceptoCargoID.Value = conceptoxDef;
                        
                        //Con la consulta del anticipo
                        compFooterImpItem.IdentificadorTR.Value = header.NumeroDoc.Value;

                        if (monedaLocal == ctrl.MonedaID.Value)
                        {
                            compFooterImpItem.vlrBaseML.Value = 0; 
                            compFooterImpItem.vlrBaseME.Value = 0;
                            compFooterImpItem.vlrMdaLoc.Value = header.ValorAnticipo;
                            compFooterImpItem.vlrMdaExt.Value = (ctrl.TasaCambioDOCU.Value == 0) ? 0 : Math.Round(compFooterImpItem.vlrMdaLoc.Value.Value / ctrl.TasaCambioDOCU.Value.Value, 2);
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaLoc.Value;
                        }
                        else
                        {
                            compFooterImpItem.vlrBaseME.Value =0;
                            compFooterImpItem.vlrBaseML.Value = 0;
                            compFooterImpItem.vlrMdaExt.Value = header.ValorAnticipo;
                            compFooterImpItem.vlrMdaLoc.Value = compFooterImpItem.vlrMdaExt.Value * ctrl.TasaCambioDOCU.Value;
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaExt.Value;
                        }
                        compFooter.Add(compFooterImpItem);
                        _valorTotal += header.ValorAnticipo; 
                    }
                }
               
                #endregion
                #region Crea registro de ReteGarantia
                if (header.ValorRteGarantia != 0)
                {
                    string ctaRteGarantia = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CuentaReteGarantia);
                    string conceptoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaRteGarantia, true, false);
                    if (cta != null)
                    {
                        DTO_ComprobanteFooter compFooterImpItem = new DTO_ComprobanteFooter();
                        compFooterImpItem.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                        compFooterImpItem.ProyectoID.Value = ctrl.ProyectoID.Value;
                        compFooterImpItem.TasaCambio.Value = ctrl.TasaCambioDOCU.Value;
                        compFooterImpItem.TerceroID.Value = ctrl.TerceroID.Value;                       
                        compFooterImpItem.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                        compFooterImpItem.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                        compFooterImpItem.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                        compFooterImpItem.DocumentoCOM.Value = ctrl.DocumentoNro.Value.ToString();
                        compFooterImpItem.CuentaID.Value = ctaRteGarantia;

                        compFooterImpItem.Descriptivo.Value = "Cancela ReteGarantia"; 
                        compFooterImpItem.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
                        compFooterImpItem.ConceptoCargoID.Value = conceptoxDef;
                        compFooterImpItem.DatoAdd2.Value = "Porcentaje: " + header.PorcRteGarantia.Value.ToString(); 

                        if (monedaLocal == ctrl.MonedaID.Value)
                        {
                            compFooterImpItem.vlrBaseML.Value = footer.Sum(x => x.ValorBruto);
                            compFooterImpItem.vlrBaseME.Value = (ctrl.TasaCambioDOCU.Value == 0) ? 0 : Math.Round(footer.Sum(x => x.ValorBruto) / ctrl.TasaCambioDOCU.Value.Value, 2);
                            compFooterImpItem.vlrMdaLoc.Value = header.ValorRteGarantia;
                            compFooterImpItem.vlrMdaExt.Value = (ctrl.TasaCambioDOCU.Value == 0) ? 0 : Math.Round(compFooterImpItem.vlrMdaLoc.Value.Value / ctrl.TasaCambioDOCU.Value.Value, 2);
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaLoc.Value;
                        }
                        else
                        {
                            compFooterImpItem.vlrBaseME.Value = footer.Sum(x => x.ValorBruto);
                            compFooterImpItem.vlrBaseML.Value = compFooterImpItem.vlrBaseME.Value * ctrl.TasaCambioDOCU.Value;
                            compFooterImpItem.vlrMdaExt.Value = header.ValorRteGarantia;
                            compFooterImpItem.vlrMdaLoc.Value = compFooterImpItem.vlrMdaExt.Value * ctrl.TasaCambioDOCU.Value;
                            compFooterImpItem.vlrMdaOtr.Value = compFooterImpItem.vlrMdaExt.Value;
                        }
                        compFooter.Add(compFooterImpItem);
                        _valorTotal += header.ValorRteGarantia;
                    }
                }

                #endregion

                #region Crea el ultimo registro de la Cuenta por Cobrar(Contrapartida)
                DTO_faFacturaTipo facTipo = (DTO_faFacturaTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, header.FacturaTipoID.Value, true, false);
                //DTO_faMovimientoTipo mvto = (DTO_faMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faMovimientoTipo,header.MvtoTipoCarID.Value, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, facTipo.coDocumentoID.Value, true, false);

                DTO_ComprobanteFooter compFooterContrItem = new DTO_ComprobanteFooter();
                compFooterContrItem.Index = compFooter.Count;
                compFooterContrItem.CuentaID.Value = monedaLocal == ctrl.MonedaID.Value ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                compFooterContrItem.TerceroID.Value = ctrl.TerceroID.Value;
                compFooterContrItem.ProyectoID.Value = ctrl.ProyectoID.Value;
                compFooterContrItem.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                compFooterContrItem.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                DTO_coPlanCuenta cuentaInfo = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, compFooterContrItem.CuentaID.Value, true, false);
                if (cuentaInfo == null)
                    return null;
                compFooterContrItem.ConceptoCargoID.Value = string.IsNullOrEmpty(cuentaInfo.ConceptoCargoID.Value) ? this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto) : cuentaInfo.ConceptoCargoID.Value;
                compFooterContrItem.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                compFooterContrItem.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                compFooterContrItem.DocumentoCOM.Value = ctrl.DocumentoNro.Value.Value.ToString();
                compFooterContrItem.ActivoCOM.Value = string.Empty;
                compFooterContrItem.ConceptoSaldoID.Value = cuentaInfo.ConceptoSaldoID.Value;
                compFooterContrItem.IdentificadorTR.Value = 0;
                compFooterContrItem.Descriptivo.Value = ctrl.Descripcion.Value;
                compFooterContrItem.TasaCambio.Value = ctrl.TasaCambioCONT.Value;
                compFooterContrItem.vlrBaseML.Value = 0;
                compFooterContrItem.vlrBaseME.Value = 0;
                if (monedaLocal == ctrl.MonedaID.Value)
                {
                    compFooterContrItem.vlrMdaLoc.Value = _valorTotal * (-1);
                    compFooterContrItem.vlrMdaExt.Value = compFooter.Sum(x => x.vlrMdaExt.Value.Value) * (-1);
                    compFooterContrItem.vlrMdaOtr.Value = compFooterContrItem.vlrMdaLoc.Value;
                }
                else
                {
                    compFooterContrItem.vlrMdaExt.Value = _valorTotal * (-1);
                    compFooterContrItem.vlrMdaLoc.Value = compFooterContrItem.vlrMdaExt.Value * compFooterContrItem.TasaCambio.Value.Value;
                    compFooterContrItem.vlrMdaOtr.Value = compFooterContrItem.vlrMdaExt.Value;
                };

                compFooterContrItem.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                compFooter.Add(compFooterContrItem);
                _valorTotal = 0;
                #endregion

                List<string> cuentas = compFooter.Select(x => x.CuentaID.Value).Distinct().ToList();
                List<string> lineas = compFooter.Select(x => x.LineaPresupuestoID.Value).Distinct().ToList();
                List<DTO_ComprobanteFooter> compsFinal = new List<DTO_ComprobanteFooter>();
                foreach (string cta in cuentas)
                {
                    List<DTO_ComprobanteFooter> comps = compFooter.FindAll(x => x.CuentaID.Value == cta);
                    foreach (string lin in lineas)
	                {
                        DTO_ComprobanteFooter nuevo = compFooter.Find(x =>x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin);
                        if (nuevo != null)
                        {
                            nuevo.vlrMdaLoc.Value = Math.Round(compFooter.FindAll(x => x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin).Sum(x => x.vlrMdaLoc.Value.Value), 2);
                            nuevo.vlrMdaExt.Value = Math.Round(compFooter.FindAll(x => x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin).Sum(x => x.vlrMdaExt.Value.Value), 2);
                            nuevo.vlrBaseML.Value = Math.Round(compFooter.FindAll(x => x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin).Sum(x => x.vlrBaseML.Value.Value), 2);
                            nuevo.vlrBaseME.Value = Math.Round(compFooter.FindAll(x => x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin).Sum(x => x.vlrBaseME.Value.Value), 2);
                            nuevo.vlrMdaOtr.Value = Math.Round(compFooter.FindAll(x => x.CuentaID.Value == cta && x.LineaPresupuestoID.Value == lin).Sum(x => x.vlrMdaOtr.Value.Value), 2);
                            compsFinal.Add(nuevo);
                            
                        }
                    }
                }

                compFooter = compsFinal;
                return compFooter;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "faComprobanteFooter_Load");
                return new List<DTO_ComprobanteFooter>();
            }

        }

        /// <summary>
        /// Aprueba una factura
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="actividadFlujoID">Actividad de flujo actual</param>
        /// <param name="fact">Factura a aprobar</param>
        /// <param name="periodo">Periodo de aprobacion</param>
        /// <param name="compID">Identificador del comprobante</param>
        /// <param name="compNro">Numero del comprobante</param>
        /// <param name="obs">Observacion de la aprobacion</param>
        /// <param name="cacheBodega">Cache con la lista de bodegas</param>
        private DTO_TxResult FacturaVenta_Aprobar(int documentID, string actividadFlujoID, DTO_faFacturacionAprobacion fact, DateTime periodo, string compID,
                                                  int compNro, string obs, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            DTO_SerializedObject resultInventario = null;
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloInventarios = (ModuloInventarios)base.GetInstance(typeof(ModuloInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                int numeroDoc = fact.NumeroDoc.Value.Value;
                Dictionary<string, DTO_MvtoInventarios> cacheBodega = new Dictionary<string, DTO_MvtoInventarios>();

                #region Borra de glMovimientoDetaPRE / Agrega a glMovimientoDeta
                List<DTO_glMovimientoDeta> listMvtoDeta = this._moduloGlobal.glMovimientoDetaPRE_GetNumeroDoc(numeroDoc);
                this._moduloGlobal.glMovimientoDetaPRE_Delete(fact.NumeroDoc.Value.Value, true);

                result = this._moduloGlobal.glMovimientoDeta_Add(listMvtoDeta, true, true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Aprobar Comprobante y DocCtrl
                result = this._moduloContabilidad.Comprobante_Aprobar(documentID, actividadFlujoID, ModulesPrefix.fa, numeroDoc, true, periodo, compID,
                    compNro, obs, true, false, true, true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Crear registros de Inventarios
                DTO_faFacturacion factura = this.FacturaVenta_Load(fact.DocumentoID.Value.Value, fact.PrefijoID.Value, fact.DocumentoNro.Value.Value);

                if (factura.Header.DocumentoREL == null || factura.Header.DocumentoREL.Value.Value == 0)
                {
                    Dictionary<string, TipoConcepto> cacheServ = new Dictionary<string, TipoConcepto>();
                    TipoConcepto tipoConcepto = 0;
                    DTO_MvtoInventarios movBodega;

                    foreach (DTO_faFacturacionFooter reg in factura.Footer)
                    {
                        if (!cacheServ.ContainsKey(reg.Movimiento.ServicioID.Value))
                        {
                            DTO_faServicios serv = (DTO_faServicios)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, reg.Movimiento.ServicioID.Value, true, false);
                            DTO_faConceptos concepto = (DTO_faConceptos)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, serv.ConceptoIngresoID.Value, true, false);
                            tipoConcepto = (TipoConcepto)Enum.Parse(typeof(TipoConcepto), concepto.TipoConcepto.Value.Value.ToString());
                            cacheServ.Add(reg.Movimiento.ServicioID.Value, tipoConcepto);
                        }
                        else
                            tipoConcepto = cacheServ[reg.Movimiento.ServicioID.Value];

                        if (tipoConcepto == TipoConcepto.VentaInv)
                        {
                            #region Crea movimiento
                            if (!cacheBodega.ContainsKey(reg.Movimiento.BodegaID.Value))
                            {
                                DTO_MvtoInventarios mov = new DTO_MvtoInventarios();

                                mov.Header.MvtoTipoInvID.Value = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovVentasLoc);
                                if (string.IsNullOrEmpty(mov.Header.MvtoTipoInvID.Value))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_In_MvtoVentasNotExist;
                                    numeroDoc = 0;
                                    return result;
                                }
                                else
                                {
                                    DTO_inMovimientoTipo movVta = (DTO_inMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, mov.Header.MvtoTipoInvID.Value, true, false);
                                    if (movVta != null && movVta.TipoMovimiento.Value != (byte)TipoMovimientoInv.Salidas)
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_MvtoVentasInvalid + "&&" + mov.Header.MvtoTipoInvID.Value;
                                        numeroDoc = 0;
                                        return result;  
                                    }                                                                
                                }
                                mov.Header.BodegaOrigenID.Value = reg.Movimiento.BodegaID.Value;
                                mov.Header.BodegaDestinoID.Value = string.Empty;
                                mov.Header.EmpresaID.Value = factura.Header.EmpresaID.Value;
                                mov.Header.AsesorID.Value = factura.Header.AsesorID.Value;
                                mov.Header.ClienteID.Value = factura.Header.ClienteID.Value;
                                mov.Header.DocumentoREL.Value = factura.Header.NumeroDoc.Value.Value;
                                mov.Header.VtoFecha.Value = factura.Header.FechaVto.Value;
                                mov.Header.NumeroDoc.Value = 0;

                                mov.DocCtrl.TerceroID.Value = factura.DocCtrl.TerceroID.Value;
                                mov.DocCtrl.NumeroDoc.Value = 0;
                                mov.DocCtrl.MonedaID.Value = factura.DocCtrl.MonedaID.Value;
                                mov.DocCtrl.ProyectoID.Value = factura.DocCtrl.ProyectoID.Value;
                                mov.DocCtrl.CentroCostoID.Value = factura.DocCtrl.CentroCostoID.Value;
                                mov.DocCtrl.PrefijoID.Value = factura.DocCtrl.PrefijoID.Value;
                                mov.DocCtrl.Fecha.Value = factura.DocCtrl.Fecha.Value;
                                mov.DocCtrl.PeriodoDoc.Value = factura.DocCtrl.PeriodoDoc.Value;
                                mov.DocCtrl.TasaCambioCONT.Value = factura.DocCtrl.TasaCambioCONT.Value;
                                mov.DocCtrl.TasaCambioDOCU.Value = factura.DocCtrl.TasaCambioDOCU.Value;
                                mov.DocCtrl.DocumentoID.Value = AppDocuments.TransaccionAutomatica;
                                mov.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                                mov.DocCtrl.PeriodoUltMov.Value = factura.DocCtrl.PeriodoUltMov.Value;
                                mov.DocCtrl.seUsuarioID.Value = factura.DocCtrl.seUsuarioID.Value;
                                mov.DocCtrl.AreaFuncionalID.Value = factura.DocCtrl.AreaFuncionalID.Value;
                                mov.DocCtrl.ConsSaldo.Value = 0;
                                mov.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                                mov.DocCtrl.FechaDoc.Value = factura.DocCtrl.FechaDoc.Value;
                                mov.DocCtrl.Descripcion.Value = "Transaccion Automatica Inv (FacturaVenta)";
                                mov.DocCtrl.Valor.Value = factura.DocCtrl.Valor.Value;
                                mov.DocCtrl.Iva.Value = factura.DocCtrl.Iva.Value;
                                mov.DocCtrl.DocumentoPadre.Value = numeroDoc;

                                cacheBodega.Add(reg.Movimiento.BodegaID.Value, mov);
                            }
                            movBodega = cacheBodega[reg.Movimiento.BodegaID.Value];

                            DTO_inMovimientoFooter footerMov = new DTO_inMovimientoFooter();
                            footerMov.Movimiento = reg.Movimiento;
                            footerMov.Movimiento.ServicioID.Value = string.Empty;
                            footerMov.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                            movBodega.Footer.Add(footerMov);
                            #endregion
                        }
                    }
                }
                #endregion                
                #region Generar los registros de Inventarios
                if (cacheBodega.Count > 0)
                {
                    List<DTO_MvtoInventarios> listBodegas = cacheBodega.Values.ToList();
                    int numDicInventarios = 0;
                    foreach (DTO_MvtoInventarios item in listBodegas)
                    {
                        if (item.DocCtrl.TasaCambioDOCU.Value != 0)
                        {
                            foreach (var mov in item.Footer)
                            {
                                mov.Movimiento.Valor1EXT.Value = mov.Movimiento.Valor1LOC.Value / item.DocCtrl.TasaCambioDOCU.Value;
                                mov.Movimiento.Valor2EXT.Value = mov.Movimiento.Valor2LOC.Value / item.DocCtrl.TasaCambioDOCU.Value;
                            }
                        }
                        resultInventario = this._moduloInventarios.Transaccion_Add(AppDocuments.TransaccionAutomatica, item, false, out numDicInventarios, new Dictionary<Tuple<int, int>, int>(), true);
                        if (resultInventario.GetType() == result.GetType())
                        {
                            DTO_TxResult res = (DTO_TxResult)resultInventario;
                            if (res.Result == ResultValue.NOK)
                                return result;
                        }
                        #region Actualiza el documento generado por el Doc padre
                        DTO_glDocumentoControl ctrlHijo = this._moduloGlobal.glDocumentoControl_GetByID(numDicInventarios);
                        ctrlHijo.DocumentoPadre.Value = numeroDoc;
                        this._moduloGlobal.glDocumentoControl_Update(ctrlHijo, true, true);
                        #endregion
                    }
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "FacturaVenta_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloInventarios._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (resultInventario != null)
                        {
                            DTO_Alarma res = (DTO_Alarma)resultInventario;
                            int numDoc = Convert.ToInt32(res.NumeroDoc);
                            DTO_glDocumentoControl docCtrlMovInventario = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                            docCtrlMovInventario.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.TransaccionAutomatica, docCtrlMovInventario.PrefijoID.Value);
                            //Comprobante 
                            DTO_coComprobante comproInvent = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docCtrlMovInventario.ComprobanteID.Value, true, false);
                            if (comproInvent != null)
                                docCtrlMovInventario.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comproInvent, docCtrlMovInventario.PrefijoID.Value, docCtrlMovInventario.PeriodoDoc.Value.Value, docCtrlMovInventario.DocumentoNro.Value.Value);
                            else
                                docCtrlMovInventario.ComprobanteIDNro.Value = 0;
                            this._moduloGlobal.ActualizaConsecutivos(docCtrlMovInventario, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(docCtrlMovInventario.NumeroDoc.Value.Value, docCtrlMovInventario.ComprobanteIDNro.Value.Value, false);

                            res.Consecutivo = docCtrlMovInventario.DocumentoNro.Value.ToString();
                            result.ResultMessage = DictionaryMessages.Co_NumberDoc + "&&" + docCtrlMovInventario.DocumentoNro.Value.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="facturaNro">Numero de Documento interno</param>
        /// <returns>Retorna la factura</returns>
        public DTO_faFacturacion FacturaVenta_Load(int documentID, string prefijoID, int facturaNro)
        {
            try
            {
                DTO_faFacturacion fact = new DTO_faFacturacion();

                //Trae glDocumentoControl
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, prefijoID, facturaNro);

                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;

                //Verifica documentoID
                fact.DocCtrl = docCtrl;
                if (docCtrl.DocumentoID.Value.Value != documentID)
                    return null;

                //Carga faFacturaDocu
                DTO_faFacturaDocu facturaHeader = this.faFacturaDocu_Get(docCtrl.NumeroDoc.Value.Value);
                fact.Header = facturaHeader;

                List<DTO_glMovimientoDeta> facturaDetail = null;
                //Obtenga datos de la tabla glMovimientoDeta
                if (docCtrl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                    facturaDetail = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                else
                    facturaDetail = this._moduloGlobal.glMovimientoDetaPRE_GetNumeroDoc(docCtrl.NumeroDoc.Value.Value);

                //Llena detalle de la factura (DTO_faFacturacionFooter)
                List<DTO_faFacturacionFooter> facturaFooter = this.faFacturaFooter_Load(docCtrl, facturaHeader, facturaDetail);
                fact.Footer = facturaFooter;

                return fact;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "FacturaVenta_Load");
                throw exception;
            }
        }

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="header">faFacturaDocu</param>
        /// <param name="footer">la lista de DTO_faFacturacionFooter</param>
        /// <param name="update">true si la factura esta actualizada</param>
        /// <param name="numeroDoc">identificador interior del documento</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject FacturaVenta_Guardar(int documentID, DTO_glDocumentoControl ctrl, DTO_faFacturaDocu header, List<DTO_faFacturacionFooter> footer, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, bool ContabilizaInd = true)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;
            DTO_Comprobante comp = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Declara las variables
                int documentoNro = 0;
                int compNro = 0;
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                DTO_glDocumentoControl docCtrl = null;
                #endregion

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.ComprobanteIDNro.Value = 0;
                    ctrl.Valor.Value = header.Valor.Value;
                    ctrl.Iva.Value = header.Iva.Value;
                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);

                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guardar en faFacturaDocu
                    header.Retencion10.Value = header.ValorAnticipo;
                    header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    foreach (DTO_faFacturacionFooter fac in footer)
                        header.Bruto.Value += fac.ValorBruto;
                    result = this.faFacturaDocu_Add(header, ctrl.DocumentoID.Value.Value);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en glMovimientoDetaPRE
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    foreach (DTO_faFacturacionFooter item in footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        item.Movimiento.Valor1LOC.Value = item.ValorBruto;
                        item.Movimiento.Valor2LOC.Value = item.ValorIVA;
                        item.Movimiento.Valor1EXT.Value = ctrl.TasaCambioDOCU.Value != 0 ? Math.Round(item.ValorBruto / ctrl.TasaCambioDOCU.Value.Value,2) : 0;
                        item.Movimiento.Valor2EXT.Value = ctrl.TasaCambioDOCU.Value != 0 ?  Math.Round(item.ValorIVA / ctrl.TasaCambioDOCU.Value.Value,2) : 0;
                        item.Movimiento.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        mov = item.Movimiento;
                        movDeta.Add(mov);
                    }
                    result = this._moduloGlobal.glMovimientoDetaPRE_Add(movDeta, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                }
                else
                {
                    header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(ctrl.NumeroDoc.Value.Value);
                    documentoNro = docCtrl.DocumentoNro.Value.Value;
                    compNro = docCtrl.ComprobanteIDNro.Value.Value;

                    #region Actualiza glDocumentoControl
                    docCtrl.CuentaID.Value = ctrl.CuentaID.Value;
                    docCtrl.TerceroID.Value = ctrl.TerceroID.Value;
                    docCtrl.ProyectoID.Value = ctrl.ProyectoID.Value;
                    docCtrl.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                    docCtrl.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                    docCtrl.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                    docCtrl.Fecha.Value = ctrl.Fecha.Value;
                    docCtrl.TasaCambioDOCU.Value = ctrl.TasaCambioDOCU.Value;
                    docCtrl.TasaCambioCONT.Value = ctrl.TasaCambioCONT.Value;
                    docCtrl.Observacion.Value = ctrl.Observacion.Value;
                    docCtrl.Iva.Value = header.Iva.Value;
                    docCtrl.Valor.Value = header.Valor.Value;
                    docCtrl.FechaDoc.Value = ctrl.FechaDoc.Value;
                    docCtrl.PeriodoDoc.Value = ctrl.PeriodoDoc.Value;
                    docCtrl.PeriodoUltMov.Value = ctrl.PeriodoUltMov.Value;
                    docCtrl.Descripcion.Value = docCtrl.Descripcion.Value.Contains("PREFACTURA VENTA DE ACTA ENTREGA") ? "FACTURA VENTA - ACTA ENTREGA PROY " : docCtrl.Descripcion.Value;

                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID,ctrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);
                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                    if (act.Count > 0)
                        this.AsignarFlujo(documentID, ctrl.NumeroDoc.Value.Value, act[0], false, string.Empty);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza en faFacturaDocu
                    List<DTO_faFacturaDocu> listHeader = new List<DTO_faFacturaDocu>();
                    foreach (DTO_faFacturacionFooter fac in footer)
                        header.Bruto.Value += fac.ValorBruto;
                    header.Retencion10.Value = header.ValorAnticipo;
                    listHeader.Add(header);
                    this._dal_faFacturaDocu.DAL_faFacturaDocu_Upd(header, false);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza en glMovimientoDetaPRE
                    #region Elimina los datos viejos
                    result = this._moduloGlobal.glMovimientoDetaPRE_Delete(header.NumeroDoc.Value.Value, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Guarda los datos nuevos
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    foreach (DTO_faFacturacionFooter item in footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        item.Movimiento.NumeroDoc.Value = header.NumeroDoc.Value.Value;
                        item.Movimiento.Valor1LOC.Value = item.ValorBruto;
                        item.Movimiento.Valor2LOC.Value = item.ValorIVA;
                        item.Movimiento.Valor1EXT.Value = ctrl.TasaCambioDOCU.Value != 0 ? Math.Round(item.ValorBruto / ctrl.TasaCambioDOCU.Value.Value,2) : 0;
                        item.Movimiento.Valor2EXT.Value = ctrl.TasaCambioDOCU.Value != 0 ? Math.Round(item.ValorIVA / ctrl.TasaCambioDOCU.Value.Value,2) : 0;
                        mov = item.Movimiento;
                        movDeta.Add(mov);
                    }
                    result = this._moduloGlobal.glMovimientoDetaPRE_Add(movDeta, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = header.NumeroDoc.Value.Value;
                        //result.Result = ResultValue.NOK;
                        //result.ResultMessage = "NOK";
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #endregion;
                }
                numeroDoc = header.NumeroDoc.Value.Value;
                ctrl.NumeroDoc.Value = numeroDoc;
                if (ContabilizaInd)
                {
                    #region Generar Comprobante
                    #region Crear Comprobante
                    comp = new DTO_Comprobante();

                    DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                    compHeader.ComprobanteID.Value = ctrl.ComprobanteID.Value;
                    compHeader.ComprobanteNro.Value = compNro;
                    compHeader.EmpresaID.Value = ctrl.EmpresaID.Value;
                    compHeader.Fecha.Value = ctrl.FechaDoc.Value.Value;
                    compHeader.NumeroDoc.Value = numeroDoc;

                    compHeader.MdaOrigen.Value = (ctrl.MonedaID.Value == monedaLocal) ? Convert.ToByte(TipoMoneda_LocExt.Local) : Convert.ToByte(TipoMoneda_LocExt.Foreign);
                    compHeader.MdaTransacc.Value = ctrl.MonedaID.Value;
                    compHeader.PeriodoID.Value = ctrl.PeriodoDoc.Value;
                    compHeader.TasaCambioBase.Value = ctrl.TasaCambioCONT.Value;
                    compHeader.TasaCambioOtr.Value = ctrl.TasaCambioCONT.Value;

                    comp.Header = compHeader;
                    comp.Footer = new List<DTO_ComprobanteFooter>();

                    comp.coDocumentoID = ctrl.DocumentoID.Value.Value.ToString();
                    comp.PrefijoID = ctrl.PrefijoID.Value.ToString();
                    comp.DocumentoNro = ctrl.DocumentoNro.Value.Value;
                    comp.CuentaID = ctrl.CuentaID.Value;
                    comp.TerceroID = ctrl.TerceroID.Value;
                    comp.ProyectoID = ctrl.ProyectoID.Value;
                    comp.CentroCostoID = ctrl.CentroCostoID.Value;
                    comp.LineaPresupuestoID = ctrl.LineaPresupuestoID.Value;
                    comp.LugarGeograficoID = ctrl.LugarGeograficoID.Value;
                    //comp.Observacion = this.txtDescrDoc.Text;

                    List<DTO_ComprobanteFooter> compFooter = this.faComprobanteFooter_Load(ctrl, header, footer);
                    if (compFooter == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Co_NocoContCta;
                        numeroDoc = 0;
                        return result;
                    }
                    comp.Footer = compFooter;
                    #endregion
                    #region Actualiza el valor del documento
                    decimal valorDoc = 0;
                    valorDoc = ctrl.Valor.Value.Value;

                    if (!update)
                    {
                        ctrl.Valor.Value = Math.Abs(valorDoc);
                        this._moduloGlobal.glDocumentoControl_Update(ctrl, true, true);
                    }
                    else
                    {
                        docCtrl.Valor.Value = Math.Abs(valorDoc);
                        this._moduloGlobal.glDocumentoControl_Update(docCtrl, true, true);
                        this._moduloGlobal.glDocumentoControl_UpdatePeriodo(numeroDoc, docCtrl.PeriodoDoc.Value.Value);
                    }

                    #endregion
                    #region Guardar comprobante
                    result = this._moduloContabilidad.ComprobantePre_Add(documentID, ModulesPrefix.fa, comp, ctrl.AreaFuncionalID.Value, ctrl.PrefijoID.Value, true, numeroDoc, null, batchProgress, true);

                    if (result.Result == ResultValue.NOK)
                    {
                        if (!update)
                            numeroDoc = 0;

                        return result;
                    }
                    ctrl.NumeroDoc.Value = numeroDoc;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #endregion 
                }

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(ctrl.NumeroDoc.Value.Value, true);
                        alarma.NumeroDoc = numeroDoc.ToString();
                        return alarma;
                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "FacturaVenta_Guardar");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Genera los consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (comp != null && comp.DocumentoNro == 0 && comp.Header.ComprobanteNro.Value.Value == 0)
                        {
                            DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrl.ComprobanteID.Value, true, false);
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, true);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, true);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                            result.ResultMessage = DictionaryMessages.Co_NumberDoc + "&&" + ctrl.DocumentoNro.Value.ToString();
                        }
                        else if (comp == null && ctrl.DocumentoNro.Value == 0)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, true);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                            result.ResultMessage = DictionaryMessages.Co_NumberDoc + "&&" + ctrl.DocumentoNro.Value.ToString();
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Trae un listado de las facturas pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_faFacturacionAprobacion> FacturaVenta_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(NewAge.ADO.DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                List<DTO_faFacturacionAprobacion> list = this._dal_faFacturaDocu.DAL_faFacturaDocu_GetPendientesByModulo(mod, actFlujoID, usuarioID);

                foreach (DTO_faFacturacionAprobacion item in list)
                    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "FacturaVenta_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de las facturas para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="fact">facturas que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> FacturaVenta_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_faFacturacionAprobacion> factList,
            Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                int i = 0;

                foreach (DTO_faFacturacionAprobacion fact in factList)
                {
                    #region Variables
                    int percent = ((i + 1) * 100) / factList.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    rd.line = i;
                    rd.Message = string.Empty;

                    int numeroDoc = fact.NumeroDoc.Value.Value;
                    DateTime periodo = fact.PeriodoID.Value.Value;
                    string compID = fact.ComprobanteID.Value;
                    int compNro = fact.ComprobanteNro.Value.Value;
                    string obs = fact.ObservacionDoc.Value;

                    #endregion

                    if (fact.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = FacturaVenta_Aprobar(documentID, actividadFlujoID, fact, periodo, compID, compNro, obs, false);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "FacturaVenta_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_AprobComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (fact.Rechazado.Value.Value)
                    {
                        try
                        {
                            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._moduloContabilidad.Comprobante_Rechazar(documentID, actividadFlujoID, fact.DocumentoID.Value.Value, numeroDoc, periodo, compID, compNro, obs, false);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "FacturaVenta_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_RechazarComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }


                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                        results.Add(alarma);
                    }

                }

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "FacturaVenta_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las facturas</param>
        /// <param name="terceroID">Tercero del Cliente</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturacionResumen> Facturacion_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_faFacturaDocu.DAL_faFacturaDocu_GetResumen(periodo, tipoMoneda, terceroID);
        }

        /// <summary>
        /// Retorna una lista de facturas 
        /// </summary>
        /// <param name="clienteID">Periodo de consulta</param>
        /// <param name="NotaEnvioEmptyInd">filtrar por nota de envio</param>
        /// <returns>Retorna una lista de facturas</returns>
        public List<DTO_faFacturaDocu> FacturaVenta_GetByCliente(int documentID, string clienteID, bool NotaEnvioEmptyInd)
        {
            this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_faFacturaDocu.DAL_faFacturaDocu_GetByCliente(clienteID, NotaEnvioEmptyInd);
        }

        /// <summary>
        /// Actualiza la tabla faFacturaDocu 
        /// </summary>
        /// <param name="fact">DTO_faFacturaDocu</param>
        /// <param name="OnlyFacturaFija">Indica si solo guarda el indicador de facturaFija</param>
        /// <returns></returns>
        public DTO_TxResult FacturaDocu_Upd(int documentoID, List<DTO_faFacturaDocu> fact, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, bool OnlyFacturaFija = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (DTO_faFacturaDocu f in fact)
                {
                    this._dal_faFacturaDocu.DAL_faFacturaDocu_Upd(f, OnlyFacturaFija);
                    batchProgress[tupProgress] = 100;

                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, f.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                }
                    #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "faFacturaDocu_Upd");
                return result;
            }
        }

        /// <summary>
        /// Agrega una lista de facturas
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="listMigracion">Datos para migrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult FacturaVenta_MigracionGeneral(int documentID, string actividadFlujoID, List<DTO_MigrarFacturaVenta> listMigracion, ref List<int> numDocs, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            numDocs = new List<int>();
            try
            {

                DTO_TxResult resultFactDocu = new DTO_TxResult();
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterSimple = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables por defecto
                string zona = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ZonaxDefecto);
                string listaPrecio = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ListaPreciosxdefecto);
                string asesor = this.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_AsesorPorDefecto);
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteMigracion);
                string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaPresup = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string balaceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string pais = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                #region Valida la existencia de FKs
                DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);
                if (coComp == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "ComprobanteID" + "&&" + compID;
                    return result;
                }
                DTO_MasterBasic zonavalid = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glZona, zona, true, false);
                if (zonavalid == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "ZonaID" + "&&" + zona;
                    return result;
                }
                DTO_MasterBasic listaPreciovalid = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faListaPrecio, listaPrecio, true, false);
                if (listaPreciovalid == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "ListaPrecioID" + "&&" + listaPrecio;
                    return result;
                }
                DTO_MasterBasic asesorvalid = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faAsesor, asesor, true, false);
                if (asesorvalid == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_CodeInvalid + "&&" + "AsesorID" + "&&" + asesor;
                    return result;
                }
                #endregion

                #endregion
                #region Valida si los terceros y clientes existen para crearlos
                List<string> listTerceros = listMigracion.Select(x => x.TerceroID.Value).Distinct().ToList();
                foreach (string tercero in listTerceros)
                {
                    DTO_coTercero dtoTercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, tercero, true, false);
                    DTO_TxResultDetail detailResult;
                    if (dtoTercero == null)
                    {
                        DTO_MigrarFacturaVenta nuevoTer = listMigracion.Find(x => x.TerceroID.Value == tercero);                      
                        #region Crea el Nuevo tercero
                        dtoTercero = new DTO_coTercero();
                        dtoTercero.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                        dtoTercero.ID.Value = tercero;
                        dtoTercero.Descriptivo.Value = nuevoTer.Apellido1.Value + " " + nuevoTer.Apellido2.Value + " " + nuevoTer.Nombre1.Value + " " + nuevoTer.Nombre2.Value;
                        dtoTercero.ApellidoPri.Value = nuevoTer.Apellido1.Value;
                        dtoTercero.ApellidoSdo.Value = nuevoTer.Apellido2.Value;
                        dtoTercero.NombrePri.Value = nuevoTer.Nombre1.Value;
                        dtoTercero.NombreSdo.Value = nuevoTer.Nombre2.Value;
                        dtoTercero.Direccion.Value = nuevoTer.Direccion.Value;
                        dtoTercero.Tel1.Value = nuevoTer.Telefono.Value;
                        dtoTercero.CECorporativo.Value = nuevoTer.CorreoElectronico.Value;
                        dtoTercero.LugarGeograficoID.Value = nuevoTer.Ciudad.Value;
                        dtoTercero.ReferenciaID.Value = nuevoTer.RegFiscal.Value;
                        dtoTercero.ActEconomicaID.Value = nuevoTer.ActEconomicaID.Value;
                        DTO_glLugarGeografico lugar = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, nuevoTer.Ciudad.Value, true, false);
                        dtoTercero.Pais.Value = lugar.PaisID.Value;
                        dtoTercero.TerceroDocTipoID.Value = nuevoTer.TipoDocumento.Value;
                        dtoTercero.AutoRetRentaInd.Value = nuevoTer.AutoRetenedorInd.Value;
                        dtoTercero.AutoRetIVAInd.Value = nuevoTer.AutoRetenedorIVAInd.Value;
                        dtoTercero.DeclaraIVAInd.Value = nuevoTer.DeclaraIVAInd.Value;
                        dtoTercero.DeclaraRentaInd.Value = nuevoTer.DeclaraRentaInd.Value;
                        dtoTercero.ExcluyeCREEInd.Value = nuevoTer.ExcluyeCREEInd.Value;
                        dtoTercero.IndependienteEMPInd.Value = nuevoTer.IndependienteEMPInd.Value;
                        dtoTercero.RadicaDirectoInd.Value = false;
                        dtoTercero.ActivoInd.Value = true;
                        dtoTercero.CtrlVersion.Value = 1;
                        this._dal_MasterSimple.DocumentID = AppMasters.coTercero;
                        detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoTercero);
                        result.Details = new List<DTO_TxResultDetail>();
                        result.Details.Add(detailResult);

                        if (detailResult.Message == ResultValue.NOK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            break;
                        }
                        #endregion
                        #region Crea el Nuevo Cliente
                        DTO_faCliente dtoCliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, nuevoTer.ClienteID.Value, true, false);
                        if (dtoCliente == null)
                        {
                            dtoCliente = new DTO_faCliente();
                            dtoCliente.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoCliente.ID.Value = nuevoTer.ClienteID.Value;
                            dtoCliente.Descriptivo.Value = nuevoTer.Apellido1.Value + " " + nuevoTer.Apellido2.Value + " " + nuevoTer.Nombre1.Value + " " + nuevoTer.Nombre2.Value;
                            dtoCliente.TerceroID.Value = tercero;
                            dtoCliente.ZonaID.Value = zona;
                            dtoCliente.ListaPrecioID.Value = listaPrecio;
                            dtoCliente.FacturaTipoID.Value = nuevoTer.FacturaTipoID.Value;
                            dtoCliente.ActivoInd.Value = true;
                            dtoCliente.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.faCliente;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoCliente);
                            result.Details = new List<DTO_TxResultDetail>();
                            result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Crea el Nuevo Cliente sino Existe
                        DTO_faCliente dtoCliente = (DTO_faCliente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, tercero, true, false);
                        if (dtoCliente == null)
                        {
                            DTO_MigrarFacturaVenta nuevoTer = listMigracion.Find(x => x.TerceroID.Value == tercero);
                            dtoCliente = new DTO_faCliente();
                            dtoCliente.EmpresaGrupoID.Value = this.Empresa.ID.Value;
                            dtoCliente.ID.Value = tercero;
                            dtoCliente.Descriptivo.Value = nuevoTer.Apellido1.Value + " " + nuevoTer.Apellido2.Value + " " + nuevoTer.Nombre1.Value + " " + nuevoTer.Nombre2.Value;
                            dtoCliente.TerceroID.Value = tercero;
                            dtoCliente.ZonaID.Value = zona;
                            dtoCliente.ListaPrecioID.Value = listaPrecio;
                            dtoCliente.FacturaTipoID.Value = nuevoTer.FacturaTipoID.Value;
                            dtoCliente.ActivoInd.Value = true;
                            dtoCliente.CtrlVersion.Value = 1;
                            this._dal_MasterSimple.DocumentID = AppMasters.faCliente;
                            detailResult = this._dal_MasterSimple.DAL_MasterSimple_AddItem(dtoCliente);
                            result.Details = new List<DTO_TxResultDetail>();
                            result.Details.Add(detailResult);
                            if (detailResult.Message == ResultValue.NOK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                break;
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                if (result.Result == ResultValue.OK)
                {
                    int i = 0;
                    List<DTO_MigrarFacturaVenta> listMigracionDistinct = new List<DTO_MigrarFacturaVenta>();
                    List<int?> factItems = listMigracion.Select(x => x.FacturaItem.Value).Distinct().ToList();
                    foreach (int factItem in factItems)
                    {
                        DTO_MigrarFacturaVenta migTmp = listMigracion.FindAll(x => x.FacturaItem.Value.Equals(factItem)).First();
                        listMigracionDistinct.Add(migTmp);
                    }
                    foreach (DTO_MigrarFacturaVenta factura in listMigracionDistinct)
                    {
                        DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                        DTO_faFacturaDocu header = new DTO_faFacturaDocu();
                        List<DTO_faFacturacionFooter> footerList = new List<DTO_faFacturacionFooter>();
                        List<DTO_glMovimientoDeta> deta = new List<DTO_glMovimientoDeta>();
                        int percent = ((i + 1) * 100) / listMigracionDistinct.Count;
                        batchProgress[tupProgress] = percent;

                        int numeroDoc = 0;
                        #region Asigna Documento Control
                        //Documento Control
                        docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                        docCtrl.DocumentoID.Value = AppDocuments.FacturaVenta;
                        docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        docCtrl.PeriodoDoc.Value = factura.Fecha.Value;
                        docCtrl.Fecha.Value = DateTime.Now;
                        docCtrl.FechaDoc.Value = factura.Fecha.Value;
                        docCtrl.PeriodoUltMov.Value = factura.Fecha.Value;
                        docCtrl.MonedaID.Value = factura.Moneda.Value;
                        docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        docCtrl.PrefijoID.Value = factura.PrefijoID.Value;
                        docCtrl.TerceroID.Value = factura.TerceroID.Value;
                        docCtrl.TasaCambioCONT.Value = 0;
                        docCtrl.TasaCambioDOCU.Value = 0;
                        docCtrl.LineaPresupuestoID.Value = lineaPresup;
                        docCtrl.LugarGeograficoID.Value = lugGeoDef;
                        docCtrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                        docCtrl.seUsuarioID.Value = this.UserId;
                        docCtrl.Descripcion.Value = "MIGRACION FACTURA VENTA";
                        docCtrl.ProyectoID.Value = factura.ProyectoID.Value;
                        docCtrl.CentroCostoID.Value = factura.CentroCostoID.Value;
                        docCtrl.Observacion.Value = factura.Descripcion.Value;
                        #region Valida la Cuenta del Tipo de factura
                        DTO_faFacturaTipo facTipo = (DTO_faFacturaTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, factura.FacturaTipoID.Value, true, false);
                        DTO_coDocumento coDocumento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, facTipo.coDocumentoID.Value, true, false);
                        string cta = factura.Moneda.Value == monedaLocal ? coDocumento.CuentaLOC.Value : coDocumento.CuentaEXT.Value;
                        DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cta, true, false);
                        if (dtoCuenta != null)
                        {
                            DTO_glConceptoSaldo concSaldoDoc = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, dtoCuenta.ConceptoSaldoID.Value, true, false);
                            if (concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Interno)
                            {
                                result.ResultMessage = DictionaryMessages.Err_InvalidCuentaTipoFact;
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                            else if (coDocumento.DocumentoID.Value != documentID.ToString())
                            {
                                result.ResultMessage = DictionaryMessages.Err_InvalidDocFact;
                                result.Result = ResultValue.NOK;
                                return result;
                            }
                        }         
                        #endregion            
                        docCtrl.CuentaID.Value = cta;
                        docCtrl.ComprobanteID.Value = coDocumento.ComprobanteID.Value;
                        docCtrl.ComprobanteIDNro.Value = 0;
                        docCtrl.DocumentoNro.Value = 0;
                        docCtrl.ConsSaldo.Value = 0;
                        #endregion
                        #region Asigna faFacturaDocu
                        header.EmpresaID.Value = this.Empresa.ID.Value;
                        header.NumeroDoc.Value = 0;
                        header.AsesorID.Value = asesor;
                        header.FacturaTipoID.Value = factura.FacturaTipoID.Value;
                        header.DocumentoREL.Value = 0;
                        header.FacturaREL.Value = 0;
                        header.MonedaPago.Value = factura.Moneda.Value;
                        header.ClienteID.Value = factura.ClienteID.Value;
                        header.ListaPrecioID.Value = listaPrecio;
                        header.ZonaID.Value = zona;
                        header.TasaPago.Value = 1;
                        header.FechaVto.Value = factura.Fecha.Value;
                        header.FormaPago.Value = "Efectivo";
                        header.Valor.Value = listMigracion.Where(x => x.FacturaItem.Value.Equals(factura.FacturaItem.Value)).Sum(x => x.Valor.Value);
                        header.Iva.Value = listMigracion.Where(x => x.FacturaItem.Value.Equals(factura.FacturaItem.Value)).Sum(x => x.Iva.Value);
                        header.Bruto.Value = header.Valor.Value;
                        header.Porcentaje1.Value = 0;
                        header.Porcentaje2.Value = 0;
                        header.PorcPtoPago.Value = 0;
                        header.FechaPtoPago.Value = factura.Fecha.Value;
                        header.ValorPtoPago.Value = 0;
                        header.Retencion1.Value = 0;
                        header.Retencion2.Value = 0;
                        header.Retencion3.Value = 0;
                        header.FacturaFijaInd.Value = false;
                        header.RteGarantiaIvaInd.Value = false;
                        #endregion
                        #region Asigna glMovimientoDeta
                        //Recorre los datos migrados y valida el detalle de cada factura
                        foreach (DTO_MigrarFacturaVenta detalleFact in listMigracion.FindAll(x => x.FacturaItem.Value.Equals(factura.FacturaItem.Value)))
                        {
                            DTO_faFacturacionFooter footer = new DTO_faFacturacionFooter();
                            footer.Index = 0;
                            footer.Movimiento.NroItem.Value = footerList.Count() + 1;
                            footer.Movimiento.ImprimeInd.Value = true;
                            footer.Movimiento.ServicioID.Value = detalleFact.ServicioID.Value;
                            footer.Movimiento.CentroCostoID.Value = detalleFact.CentroCostoID.Value;
                            footer.Movimiento.TerceroID.Value = detalleFact.TerceroID.Value;
                            footer.Movimiento.ProyectoID.Value = detalleFact.ProyectoID.Value;
                            footer.Movimiento.EmpresaID.Value = this.Empresa.ID.Value;

                            footer.Movimiento.BodegaID.Value = string.Empty;
                            footer.Movimiento.PlaquetaID.Value = string.Empty;
                            footer.Movimiento.inReferenciaID.Value = string.Empty;
                            footer.Movimiento.EstadoInv.Value = (int)EstadoInv.Activo;
                            footer.Movimiento.Parametro1.Value = string.Empty;
                            footer.Movimiento.Parametro2.Value = string.Empty;
                            footer.Movimiento.IdentificadorTr.Value = 0;
                            footer.Movimiento.SerialID.Value = string.Empty;
                            footer.Movimiento.EmpaqueInvID.Value = string.Empty;
                            footer.Movimiento.CantidadEMP.Value = 0;
                            footer.Movimiento.CantidadUNI.Value = detalleFact.Cantidad.Value;
                            footer.Movimiento.ValorUNI.Value = detalleFact.Valor.Value;
                            footer.ValorBruto = detalleFact.Valor.Value.Value;
                            footer.ValorIVA = detalleFact.Iva.Value.Value;
                            footer.ValorTotal = detalleFact.Valor.Value.Value;
                            footer.Movimiento.DescripTExt.Value = detalleFact.Descripcion.Value;
                            footer.ValorNeto = detalleFact.Valor.Value.Value + detalleFact.Iva.Value.Value;
                            footer.Movimiento.DocSoporte.Value = 0;
                            footerList.Add(footer);
                        }
                        #endregion
                        #region Guarda la factura Nueva
                        DTO_SerializedObject resulFact = this.FacturaVenta_Guardar(AppDocuments.FacturaVenta, docCtrl, header, footerList, false, out numeroDoc, batchProgress, true);
                        if (resulFact.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult restmp = (DTO_TxResult)resulFact;
                            result = restmp;
                            break;
                        }
                        numDocs.Add(numeroDoc);
                        #endregion
                        i++;
                    }
                }

                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                batchProgress[tupProgress] = 100;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "FacturaVenta_MigracionGeneral");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    base._mySqlConnectionTx.Commit();

                    base._mySqlConnectionTx = null;
                    this._moduloAplicacion._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;
                    this._moduloContabilidad._mySqlConnectionTx = null;

                    foreach (int num in numDocs)
                    {
                        DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(num);
                        DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docCtrl.ComprobanteID.Value, true, false);
                        docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                        docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, true);
                        this._moduloContabilidad.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, docCtrl.ComprobanteIDNro.Value.Value, true);
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Anula una factura de venta
        /// </summary>
        /// <param name="numDocFacts">nums de las facturas a anular</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult FacturaVenta_Anular(int documentID, List<int> numDocFacts, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._moduloProy = (ModuloProyectos)base.GetInstance(typeof(ModuloProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_actasEntrega = (DAL_pyActaEntregaDeta)base.GetInstance(typeof(DAL_pyActaEntregaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._moduloGlobal.glDocumentoControl_Anular(documentID, numDocFacts, batchProgress, true);

                if (result.Result == ResultValue.NOK)
                    return result;

                //Actualiza las actas de entrega que tenga relacionadas cada factura si es creada por el modulo de Proyectos
                foreach (int num in numDocFacts)
	            {
                   DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(num);
                   var proyecto = this._moduloProy.SolicitudProyecto_Load(AppDocuments.Proyecto,string.Empty,null,null,string.Empty,ctrl.ProyectoID.Value,false,false,false,false);
                   if (proyecto != null)
                   {
                       DTO_pyActaEntregaDeta filter = new DTO_pyActaEntregaDeta();
                       filter.NumDocFactura.Value = num;
                       filter.NumDocProyecto.Value = proyecto.DocCtrl.NumeroDoc.Value;
                       List<DTO_pyActaEntregaDeta> actasFactura = this._dal_actasEntrega.DAL_pyActaEntregaDeta_GetByParameter(filter);
                       foreach (DTO_pyActaEntregaDeta acta in actasFactura)
                       {
                           acta.NumDocFactura.Value = null;
                           this._dal_actasEntrega.DAL_pyActaEntregaDeta_Upd(acta);
                       }               
                   }  
	            }      
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "faFacturaDocu_Upd");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }


        #endregion

        #endregion

        #region CxC

        /// <summary>
        /// Agrega una lista de CxC
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxcFactList">Lista de cuentas por cobrar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXCobrar_Migracion(int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrlList, List<DTO_faFacturaDocu> cxcFactList, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                DTO_TxResult resultFactDocu = new DTO_TxResult();
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteMigracion);
                string prefDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string lugGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string balaceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);
                DTO_glDocumentoControl ctrlAux = ctrlList.First();
                string ctaID = ctrlAux.CuentaID.Value;
                DateTime periodo = ctrlAux.PeriodoDoc.Value.Value;
                #endregion
                #region Crea el comprobante
                DTO_Comprobante comp = new DTO_Comprobante();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                #region Header
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.LibroID.Value = balaceFuncional;
                header.PeriodoID.Value = periodo;
                header.ComprobanteID.Value = compID;
                header.ComprobanteNro.Value = this.GenerarComprobanteNro(coComp, prefDef, ctrlAux.PeriodoDoc.Value.Value, 0);
                header.Fecha.Value = periodo;
                header.NumeroDoc.Value = 0;
                header.MdaTransacc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Local;
                header.TasaCambioBase.Value = ctrlAux.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = ctrlAux.TasaCambioCONT.Value;

                comp.Header = header;
                #endregion
                #region Asigna al detalle del comprobante la info de los saldos actual
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                List<DTO_coCuentaSaldo> saldos = this._moduloContabilidad.Saldos_GetSaldosByPeriodoCuenta(ctrlAux.PeriodoDoc.Value.Value, ctrlAux.CuentaID.Value, libroFunc);
                foreach (DTO_coCuentaSaldo saldo in saldos)
                {
                    //Info segun los saldos
                    decimal saldoML = saldo.DbOrigenExtML.Value.Value + saldo.DbOrigenLocML.Value.Value + saldo.CrOrigenExtML.Value.Value + saldo.CrOrigenLocML.Value.Value
                        + saldo.DbSaldoIniExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value + saldo.CrSaldoIniExtML.Value.Value + saldo.CrSaldoIniLocML.Value.Value;
                    decimal saldoME = saldo.DbOrigenExtME.Value.Value + saldo.DbOrigenLocME.Value.Value + saldo.CrOrigenExtME.Value.Value + saldo.CrOrigenLocME.Value.Value
                        + saldo.DbSaldoIniExtME.Value.Value + saldo.DbSaldoIniLocME.Value.Value + saldo.CrSaldoIniExtME.Value.Value + saldo.CrSaldoIniLocME.Value.Value;

                    DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                    detalle.CuentaID.Value = saldo.CuentaID.Value;
                    detalle.ProyectoID.Value = saldo.ProyectoID.Value;
                    detalle.CentroCostoID.Value = saldo.CentroCostoID.Value;
                    detalle.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                    detalle.LugarGeograficoID.Value = lugGeoDef;
                    detalle.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                    detalle.ConceptoSaldoID.Value = saldo.ConceptoSaldoID.Value;
                    detalle.PrefijoCOM.Value = prefDef;
                    detalle.TerceroID.Value = saldo.TerceroID.Value;
                    detalle.DocumentoCOM.Value = string.Empty;
                    detalle.IdentificadorTR.Value = 0;
                    detalle.vlrMdaLoc.Value = saldoML * -1;
                    detalle.vlrMdaExt.Value = saldoME * -1;
                    detalle.vlrBaseML.Value = 0;
                    detalle.vlrBaseME.Value = 0;

                    footer.Add(detalle);
                }
                #endregion
                #endregion
                #region Agrega la info de los documentos
                for (int i = 0; i < ctrlList.Count; i++)
                {
                    int percent = ((i + 1) * 100) / ctrlList.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_glDocumentoControl ctrl = ctrlList[i];
                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    decimal vlrML = 0;
                    decimal vlrME = 0;
                    #region Valida que no exista el documento
                    ctrlAux = this._moduloGlobal.glDocumentoControl_GetInternalDoc(ctrl.DocumentoID.Value.Value, ctrl.PrefijoID.Value, ctrl.DocumentoNro.Value.Value);
                    if (ctrlAux != null)
                    {
                        rd = new DTO_TxResultDetail();
                        rd.line = i + 1;
                        rd.Message = DictionaryMessages.Err_Gl_DocIntAdded;

                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                    }
                    #endregion
                    #region Guarda la info en glDocumentoControl
                    if (result.Result == ResultValue.OK)
                    {
                        ctrl.ComprobanteID.Value = header.ComprobanteID.Value;
                        ctrl.ComprobanteIDNro.Value = header.ComprobanteNro.Value;
                        rd = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                        if (rd.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.Details.Add(rd);
                        }
                        else
                            ctrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                    }
                    #endregion
                    #region Guarda la info de la CxC
                    if (result.Result == ResultValue.OK)
                    {
                        DTO_faFacturaDocu cxp = cxcFactList[i];
                        cxp.NumeroDoc.Value = ctrl.NumeroDoc.Value.Value;
                        vlrML = cxp.Valor.Value.Value;
                        vlrME = ctrl.TasaCambioDOCU.Value.Value != 0 ? cxp.Valor.Value.Value / ctrl.TasaCambioDOCU.Value.Value : 0;

                        resultFactDocu = this.faFacturaDocu_Add(cxp, documentID);
                        if (resultFactDocu.Result == ResultValue.NOK)
                        {
                            rd = new DTO_TxResultDetail();
                            rd.line = i + 1;
                            rd.Message = result.ResultMessage;

                            result.Result = ResultValue.NOK;
                            result.Details.Add(rd);
                        }
                    }
                    #endregion
                    #region Agrega el detalle al comprobante
                    DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                    detalle.CuentaID.Value = ctrl.CuentaID.Value;
                    detalle.ProyectoID.Value = ctrl.ProyectoID.Value;
                    detalle.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                    detalle.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                    detalle.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                    detalle.ConceptoCargoID.Value = concCargoDef;
                    detalle.ConceptoSaldoID.Value = concSaldoID;
                    detalle.PrefijoCOM.Value = ctrl.PrefijoID.Value;
                    detalle.TerceroID.Value = ctrl.TerceroID.Value;
                    detalle.DocumentoCOM.Value = ctrl.DocumentoTercero.Value;
                    detalle.IdentificadorTR.Value = ctrl.NumeroDoc.Value.Value;
                    detalle.vlrMdaLoc.Value = vlrML;
                    detalle.vlrMdaExt.Value = vlrME;
                    detalle.vlrBaseML.Value = 0;
                    detalle.vlrBaseME.Value = 0;

                    footer.Add(detalle);
                    #endregion
                }
                #endregion
                #region Cambia el concepto de saldo de la cuenta
                if (result.Result == ResultValue.OK)
                {
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaID, true, false);
                    DAL_coPlanCuenta _dalCta = new DAL_coPlanCuenta(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    _dalCta.DAL_coPlanCuenta_UpdateConceptoSaldo(cta.ReplicaID.Value.Value, concSaldoID);
                }
                #endregion
                #region Contabiliza el comprobante
                if (result.Result == ResultValue.OK)
                {
                    comp.Footer = footer;
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, periodo, ModulesPrefix.cp, 0, false);
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXCobrar_Migracion");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Función que carga una lista de facrutas
        /// </summary>
        /// <param name="año">Año para filtrar</param>
        /// <param name="terceroId">Filtro del tercero</param>
        /// <param name="factNro">Numero de factura</param>
        /// <returns>Lista de Facturas</returns>
        public List<DTO_QueryHeadFactura> ConsultarFacturas(DateTime ano, string terceroId, int tipoConsulta, string Asesor, string Zona, string Proyecto, int TipoFact, string NumFact, string Prefijo, bool facturaFijaInd)
        {
            try
            {
                #region Variables

                List<DTO_QueryHeadFactura> facturasList = new List<DTO_QueryHeadFactura>();
                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)base.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosCta = (DAL_tsBancosCuenta)base.GetInstance(typeof(DAL_tsBancosCuenta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);


                #endregion

                //Trae los datos
                facturasList = this._dal_faFacturaDocu.Consultar_Facturas(ano, terceroId, tipoConsulta, Asesor, Zona, Proyecto, TipoFact, NumFact, Prefijo, facturaFijaInd);
                if (facturasList.Count != 0)
                {
                    List<DTO_QueryHeadFactura> head = new List<DTO_QueryHeadFactura>();
                    List<int> distinct = (from c in facturasList select c.NumeroDoc.Value.Value).Distinct().ToList();
                    foreach (int num in distinct)
                    {
                        DTO_QueryHeadFactura dto = facturasList.Find(x=>x.NumeroDoc.Value == num);
                        head.Add(dto);
                    }
                    facturasList = head;
                    foreach (DTO_QueryHeadFactura item in facturasList)
                    {
                        List<DTO_QueryDetailFactura> facturaDeta = new List<DTO_QueryDetailFactura>();
                        item.Detalle = new List<DTO_QueryDetailFactura>();
                        facturaDeta = this._dal_faFacturaDocu.Consultar_Facturas_Detalle((int)item.NumeroDoc.Value, ano);
                        foreach (DTO_QueryDetailFactura item2 in facturaDeta)
                        {
                            if (item2.DocumentoTipo.Value == AppDocuments.DesembolsoFacturas)
                            {
                                DTO_tsBancosDocu tsBancosDocu = this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(item2.NumeroDoc.Value.Value);
                                DTO_tsBancosCuenta tsBancosCta = (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, tsBancosDocu.BancoCuentaID.Value, true, false);
                                DTO_tsBanco tsBanco = (DTO_tsBanco)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBanco, tsBancosCta.BancoID.Value, true, false);

                                item2.Banco.Value = tsBanco.Descriptivo.Value;
                            }
                        }
                        item.Detalle.AddRange(facturaDeta);
                    }
                }
                return facturasList;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ConsultarFacturas");
                return new List<DTO_QueryHeadFactura>();
            }

        }

        #endregion

        #region Reportes

        /// <summary>
        /// Funcion q carga la lista para generar la factura
        /// </summary>
        /// <param name="numDoc">Numero Docuemento con los datos q se van a imprimir</param>
        /// <returns></returns>
        public List<DTO_FacturacionTotales> ReportesFacturacion_FacturaVenta(string numDoc, bool isAprobada)
        {
            try
            {
                //Variables
                List<DTO_FacturacionTotales> facturaVenta = new List<DTO_FacturacionTotales>();
                DTO_FacturacionTotales factura = new DTO_FacturacionTotales();
                factura.DetalleFacturaVenta = new List<DTO_ReportFacturaVenta>();
                this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<string> datosEmpresa = new List<string>();
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                //Carga la dto de la empresa
                DTO_coTercero terceroEmpresa = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                factura.DetalleFacturaVenta = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_FacturaVenta(numDoc, terceroEmpresa, isAprobada);
                facturaVenta.Add(factura);

                return facturaVenta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesFacturacion_FacturaVenta");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion q carga la lista para generar las facturas
        /// </summary>
        /// <param name="numDocs">Numeros Docuementos con los datos q se van a imprimir</param>
        /// <returns></returns>
        public List<DTO_FacturacionTotales> ReportesFacturacion_FacturaVentaMasivo(string prefijo, int docNroIni, int docnroFin)
        {
            try
            {
                //Variables
                List<DTO_FacturacionTotales> facturaVenta = new List<DTO_FacturacionTotales>();
                //DTO_FacturacionTotales factura = new DTO_FacturacionTotales();
                //factura.DetalleFacturaVenta = new List<DTO_ReportFacturaVenta>();
                //this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //List<string> datosEmpresa = new List<string>();
                //string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                ////Carga la lista para los datos de la empresa
                //DTO_coTercero terceroEmpresa = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                //datosEmpresa.Add(terceroPorDefecto);
                //datosEmpresa.Add(terceroEmpresa.Direccion.Value);
                //datosEmpresa.Add(terceroEmpresa.Tel1.Value);

                //factura.DetalleFacturaVenta = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_FacturaVenta(numDoc, datosEmpresa, isAprobada);
                //facturaVenta.Add(factura);

                return facturaVenta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesFacturacion_FacturaVenta");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga la lista de las cuentas por cobrar Detalladas
        /// </summary>
        /// <param name="fechaCorte">Fecha en que se esta haciendo la consulta</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <returns></returns>
        public List<DTO_FacturacionTotales> ReportesFacturacion_CxCPorEdadesDetalladas(DateTime fechaCorte, string tercero, bool isDetallada)
        {
            try
            {
                this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_FacturacionTotales> result = new List<DTO_FacturacionTotales>();
                DTO_FacturacionTotales detalle = new DTO_FacturacionTotales();
                detalle.DetalleCxCPorEdades = new List<DTO_ReportCxCPorEdades>();
                decimal total = 0;

                detalle.DetalleCxCPorEdades = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_CxCPOrEdades(fechaCorte, tercero, isDetallada);
                List<string> distinct = (from c in detalle.DetalleCxCPorEdades select c.TerceroID.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    total = 0;
                    DTO_FacturacionTotales detalleTotal = new DTO_FacturacionTotales();
                    detalleTotal.DetalleCxCPorEdades = new List<DTO_ReportCxCPorEdades>();

                    detalleTotal.DetalleCxCPorEdades = detalle.DetalleCxCPorEdades.Where(x => x.TerceroID.Value == item).ToList();

                    foreach (DTO_ReportCxCPorEdades sum in  detalleTotal.DetalleCxCPorEdades)
                    {
                        total += (sum.Treinta.Value.Value + sum.Sesenta.Value.Value + sum.Noventa.Value.Value + sum.COchenta.Value.Value + sum.MasCOchenta.Value.Value);
                        detalleTotal.ValorTotalDeta.Value = total;
                    }

                    result.Add(detalleTotal);
                }
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesFacturacion_CxCPorEdadesDetalladas");
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que carga la lista de las cuentas por cobrar Resumida
        /// </summary>
        /// <param name="fechaCorte">Fecha en que se esta haciendo la consulta</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <returns></returns>
        public List<DTO_FacturacionTotales> ReportesFacturacion_CxCPorEdadesResumida(DateTime fechaCorte, string tercero, bool isDetallada)
        {
            try
            {
                this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_FacturacionTotales> result = new List<DTO_FacturacionTotales>();
                DTO_FacturacionTotales detalle = new DTO_FacturacionTotales();
                detalle.DetalleCxCPorEdades = new List<DTO_ReportCxCPorEdades>();

                detalle.DetalleCxCPorEdades = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_CxCPOrEdades(fechaCorte,tercero,isDetallada);
                result.Add(detalle);

                return result;

            }
            catch (Exception ex)
            {
               Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesFacturacion_CxCPorEdadesDetalladas");
               throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga la lista de las cuentas por cobrar Resumida
        /// </summary>
        /// <param name="fechaCorte">Fecha en que se esta haciendo la consulta</param>
        /// <param name="tercero">Si se quiere filtrar por un tercero en especifico</param>
        /// <param name="isDetallada">Revisa si la consulta es detalla o resumida (true) detallada (false) resumida</param>
        /// <returns></returns>
        public List<DTO_FacturacionTotales> ReportesFacturacion_LibroVentas(DateTime periodo, int diaFinal, string cliente, string prefijo, string NroFactura)
        {
            try
            {
                this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_FacturacionTotales> result = new List<DTO_FacturacionTotales>();
                DTO_FacturacionTotales libros = new DTO_FacturacionTotales();
                libros.DetalleLibroVentas = new List<DTO_ReportLibroVentas>();

                libros.DetalleLibroVentas = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_LibroVentas(periodo, diaFinal, cliente, prefijo, NroFactura);
                List<DateTime> distinct = (from c in libros.DetalleLibroVentas select c.FechaDoc.Value.Value).ToList();

                foreach (var item in distinct)
                {
                    DTO_FacturacionTotales librosVentas = new DTO_FacturacionTotales();
                    librosVentas.DetalleLibroVentas = new List<DTO_ReportLibroVentas>();

                    librosVentas.DetalleLibroVentas = libros.DetalleLibroVentas.Where(x => x.FechaDoc.Value.Value == item).ToList();
                    result.Add(librosVentas);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesFacturacion_LibroVentas");
                throw ex;
            }


        }

        /// <summary>
        /// Funcion que arma el objeto que toma el reporte de Cuentas por cobrar
        /// </summary>
        /// <param name="fecha">Fecha por consultar</param>
        /// <returns>Lista de facturas por pagar por cliente</returns>
        public List<DTO_FacturacionTotales> Report_CuentasXCobrar(string Tercero, int Moneda, string Cuenta, DateTime fecha)
        {
            #region Variables

            DTO_FacturacionTotales dtoTotalesReturn = new DTO_FacturacionTotales();
            List<DTO_FacturacionTotales> dtoTotales = new List<DTO_FacturacionTotales>();
            this._dal_ReportesFacturacion = (DAL_ReportesFacturacion)this.GetInstance(typeof(DAL_ReportesFacturacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.DetalleCuentasPorCobrar = new List<DTO_ReportCuentasPorCobrar>();

                dtoTotalesReturn.DetalleCuentasPorCobrar = this._dal_ReportesFacturacion.DAL_ReportesFacturacion_CuentasPorCobrar(Tercero, Moneda, Cuenta, fecha);

                List<string> distinct = (from c in dtoTotalesReturn.DetalleCuentasPorCobrar select c.TerceroID.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    DTO_FacturacionTotales obj = new DTO_FacturacionTotales();
                    obj.DetalleCuentasPorCobrar = new List<DTO_ReportCuentasPorCobrar>();

                    obj.DetalleCuentasPorCobrar = dtoTotalesReturn.DetalleCuentasPorCobrar.Where(x => x.TerceroID.Value == item).ToList();
                    dtoTotales.Add(obj);
                }
                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_FacturasXPagar");
                return null;
            }
        }
        #endregion
    }
}