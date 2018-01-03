using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Reportes;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloInventarios : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_inMovimientoDocu _dal_inMovimientoDocu = null;
        private DAL_MvtoSaldosCostos _dal_mvtoSaldosCostos = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_acActivoControl _dal_acActivoControl = null;
        private DAL_inFisicoInventario _dal_inFisicoInventario = null;
        private DAL_ReportesInventarios _dal_ReportesInventarios = null;
        private DAL_inDistribucionCosto _dal_inDistribucionCosto = null;
        private DAL_glMovimientoDeta _dal_glMovimientoDeta = null;
        private DAL_inImportacionDocu _dal_inImportacionDocu = null;
        private DAL_inImportacionDeta _dal_inImportacionDeta = null;
        private DAL_faFacturaDocu _dal_faFacturaDocu = null;
        private DAL_inOrdenSalidaDeta _dal_inOrdenSalidaDeta = null;
        private DAL_inOrdenSalidaDocu _dal_inOrdenSalidaDocu = null;
        private DAL_pyProyectoMvto _dal_pyProyectoMvto = null;
        private DAL_prDetalleDocu _dal_prDetalleDocu = null;
        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloFacturacion _moduloFacturacion = null;
        private ModuloActivosFijos _moduloActivos = null;
        private ModuloProveedores _moduloProveedor = null;
        private ModuloProyectos _moduloProyectos = null;
        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Inventarios
        /// </summary>
        /// <param name="conn"></param>
        public ModuloInventarios(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Comprobantes

        #region Funciones Privadas

        /// <summary>
        /// Genera el comprobante de posteo de un documento
        /// </summary>
        /// <param name="ctrl">Documento control</param>
        /// <param name="cacheReferencias">Lista de referencias buscadas</param>
        /// <param name="cacheBodegas">Lista de bodegas buscadas</param>
        /// <param name="cacheContabiliza">Lista de informacion de las contabilizaciones (cuentas) buscadas</param>
        /// <param name="cacheCtas">Lista de cuentas buscadas</param>
        /// <param name="terceroXDef">Tercero por defecto</param>
        /// <param name="concCargoXdef">Concepto cargo por defecto</param>
        /// <param name="lgXdef">Lugar geografico por defecto</param>
        /// <param name="lineaXdef">Linea presupuestalpor defecto</param>
        /// <returns>Retorna un comprobante</returns>
        private object GenerarComprobantePosteo(DTO_glDocumentoControl ctrl, 
            Dictionary<string, DTO_coComprobante> cacheCoComps, 
            Dictionary<string, DTO_inReferencia> cacheReferencias, 
            Dictionary<string, DTO_inBodega> cacheBodegas, 
            Dictionary<Tuple<string, string>, DTO_inContabiliza> cacheContabiliza, 
            Dictionary<string, DTO_coPlanCuenta> cacheCtas,
            Dictionary<string, DTO_inRefGrupo> cacheRefGrupos, 
            Dictionary<string, DTO_coProyecto> cacheProys, 
            Dictionary<string, DTO_coCentroCosto> cacheCtosCosto,
            string terceroXDef, string concCargoXdef, string lgXdef, string lineaXdef)
        {
            try
            {
                DTO_Comprobante comp = new DTO_Comprobante();
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.Err_AddCompr;
                result.Details = new List<DTO_TxResultDetail>();

                #region Variables
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(NewAge.ADO.DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //Info del comprobante
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> contras = new List<DTO_ComprobanteFooter>();
                string compId;
                string ctaContraId;
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                //Info de inventarios
                DTO_inMovimientoDocu mvtoDoc = this._dal_inMovimientoDocu.DAL_inMovimientoDocu_Get(ctrl.NumeroDoc.Value.Value);
                List<DTO_glMovimientoDeta> mvtoDeta = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(ctrl.NumeroDoc.Value.Value);
                //Info de cache
                DTO_inReferencia refDTO;
                DTO_inBodega bodegaDTO;
                DTO_inRefGrupo refGrupoDTO;
                DTO_inContabiliza contaDTO;
                DTO_coPlanCuenta ctaCosto;
                DTO_coPlanCuenta ctaFOB;
                DTO_coPlanCuenta ctaContra;
                DTO_coProyecto proy;
                DTO_coCentroCosto ctoCosto;
                #endregion
                #region Obtiene la info del comprobante y la contrapartida
                DTO_inMovimientoTipo mvtoTipo = (DTO_inMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, mvtoDoc.MvtoTipoInvID.Value, true, false);
                DTO_glBienServicioClase claseBS = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, mvtoDoc.MvtoTipoInvID.Value, true, false);
                DTO_coConceptoCargo concCargo = (DTO_coConceptoCargo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coConceptoCargo, claseBS.ConceptoCargoID.Value, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, mvtoTipo.coDocumentoID.Value, true, false);
                compId = coDoc.ComprobanteID.Value;

                //Trae la info del comprobante
                if (!cacheCoComps.ContainsKey(compId))
                {
                    DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compId, true, false);
                    cacheCoComps.Add(compId, coComp);
                }
                #endregion
                #region Crea el cabezote
                header.ComprobanteID.Value = compId;
                header.ComprobanteNro.Value = ctrl.ComprobanteIDNro.Value.HasValue && ctrl.ComprobanteIDNro.Value.Value != 0 ? ctrl.ComprobanteIDNro.Value.Value : 0;
                header.Fecha.Value = ctrl.Fecha.Value;
                header.MdaOrigen.Value = ctrl.MonedaID.Value == mdaLoc ? (Byte)TipoMoneda.Local : (Byte)TipoMoneda.Foreign;
                header.MdaTransacc.Value = ctrl.MonedaID.Value;
                header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                header.PeriodoID.Value = ctrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = ctrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = ctrl.TasaCambioCONT.Value;
                #endregion
                #region Carga la info del detalle
                int i = 0;
                foreach (DTO_glMovimientoDeta deta in mvtoDeta)
                {
                    i++;

                    #region Trae la info de la bodega
                    //Trae la bodega
                    if (cacheBodegas.ContainsKey(deta.BodegaID.Value))
                        bodegaDTO = cacheBodegas[deta.BodegaID.Value];
                    else
                    {
                        bodegaDTO = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, deta.BodegaID.Value, true, false);
                        cacheBodegas.Add(deta.BodegaID.Value, bodegaDTO);
                    }
                    #endregion
                    #region Trae la referencia
                    if (cacheReferencias.ContainsKey(deta.inReferenciaID.Value))
                        refDTO = cacheReferencias[deta.inReferenciaID.Value];
                    else
                    {
                        refDTO = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, deta.inReferenciaID.Value, true, false);
                        cacheReferencias.Add(deta.inReferenciaID.Value, refDTO);
                    }
                    #endregion
                    #region Trae el grupo de las referencias
                    if (cacheRefGrupos.ContainsKey(refDTO.GrupoInvID.Value))
                        refGrupoDTO = cacheRefGrupos[refDTO.GrupoInvID.Value];
                    else
                    {
                        refGrupoDTO = (DTO_inRefGrupo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefGrupo, refDTO.GrupoInvID.Value, true, false);
                        cacheRefGrupos.Add(refDTO.GrupoInvID.Value, refGrupoDTO);
                    }
                    #endregion
                    #region Trae la informacion de la tabla inContabiliza
                    Tuple<string, string> tupla = new Tuple<string, string>(deta.BodegaID.Value, deta.inReferenciaID.Value);
                    if (cacheContabiliza.ContainsKey(tupla))
                    {
                        contaDTO = cacheContabiliza[tupla];
                        if (contaDTO == null)
                        {
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = DictionaryMessages.Err_In_NoKeyInContab + "&&" + bodegaDTO.inBodegaContabID.Value + "&&" + refDTO.GrupoInvID.Value;

                            result.Details.Add(rd);
                        }
                    }
                    else
                    {
                        //Trae la info de la maestra de contabilizacion
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("inBodegaContabID", bodegaDTO.inBodegaContabID.Value);
                        pks.Add("GrupoInvID", refDTO.GrupoInvID.Value);

                        contaDTO = (DTO_inContabiliza)this.GetMasterComplexDTO(AppMasters.inContabiliza, pks, true);
                        cacheContabiliza.Add(tupla, contaDTO);

                        if (contaDTO == null)
                        {
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = i;
                            rd.Message = DictionaryMessages.Err_In_NoKeyInContab + "&&" + bodegaDTO.inBodegaContabID.Value + "&&" + refDTO.GrupoInvID.Value;

                            result.Details.Add(rd);
                        }
                    }
                    #endregion
                    if (result.Details.Count == 0)
                    {
                        #region Crea el detalle del costo

                        #region Trae la info de la cuenta de costo
                        if (cacheCtas.ContainsKey(contaDTO.CuentaCosto.Value))
                            ctaCosto = cacheCtas[contaDTO.CuentaCosto.Value];
                        else
                        {
                            ctaCosto = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, contaDTO.CuentaCosto.Value, true, false);
                            cacheCtas.Add(contaDTO.CuentaCosto.Value, ctaCosto);
                        }
                        #endregion

                        DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                        List<DTO_ComprobanteFooter> footerTemp =
                            footer.Where(x => x.CuentaID.Value == ctaCosto.ID.Value &&
                                              x.ProyectoID.Value == bodegaDTO.ProyectoID.Value &&
                                              x.CentroCostoID.Value == bodegaDTO.CentroCostoID.Value).ToList();

                        if (footer.Count > 0)
                        {
                            #region Llave existente
                            detalle = footerTemp.First();
                            detalle.vlrMdaLoc.Value += deta.Valor1LOC.Value;

                            if (deta.Valor1EXT.Value.HasValue)
                                detalle.vlrMdaExt.Value += deta.Valor1EXT.Value;
                            #endregion
                        }
                        else
                        {
                            #region Nuevo registro
                            detalle.CuentaID.Value = ctaCosto.ID.Value;
                            detalle.ConceptoSaldoID.Value = ctaCosto.ConceptoSaldoID.Value;

                            detalle.ProyectoID.Value = bodegaDTO.ProyectoID.Value;
                            detalle.CentroCostoID.Value = bodegaDTO.CentroCostoID.Value;

                            detalle.LineaPresupuestoID.Value = lineaXdef;
                            detalle.TerceroID.Value = terceroXDef;
                            detalle.ConceptoCargoID.Value = concCargoXdef;
                            detalle.LugarGeograficoID.Value = lgXdef;
                            detalle.PrefijoCOM.Value = ctrl.PrefijoID.Value;

                            detalle.IdentificadorTR.Value = 0;
                            detalle.TasaCambio.Value = ctrl.TasaCambioCONT.Value;
                            detalle.DocumentoCOM.Value = ctrl.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno ? ctrl.DocumentoNro.Value.Value.ToString() : ctrl.DocumentoTercero.Value;
                            detalle.vlrMdaLoc.Value = deta.Valor1LOC.Value;
                            detalle.vlrMdaExt.Value = deta.Valor1EXT.Value;
                            detalle.vlrBaseML.Value = 0;
                            detalle.vlrBaseME.Value = 0;

                            footer.Add(detalle);
                            #endregion
                        }
                        #endregion
                        #region Crea el detalle del FOB
                        if (!string.IsNullOrWhiteSpace(contaDTO.CuentaFOB.Value))
                        {
                            #region Trae la info de la cuenta FOB
                            if (cacheCtas.ContainsKey(contaDTO.CuentaFOB.Value))
                                ctaFOB = cacheCtas[contaDTO.CuentaFOB.Value];
                            else
                            {
                                ctaFOB = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, contaDTO.CuentaFOB.Value, true, false);
                                cacheCtas.Add(contaDTO.CuentaFOB.Value, ctaFOB);
                            }
                            #endregion

                            DTO_ComprobanteFooter detalleFOB = new DTO_ComprobanteFooter();
                            List<DTO_ComprobanteFooter> footerTempFOB =
                                footer.Where(x => x.CuentaID.Value == ctaFOB.ID.Value &&
                                                  x.ProyectoID.Value == bodegaDTO.ProyectoID.Value &&
                                                  x.CentroCostoID.Value == bodegaDTO.CentroCostoID.Value).ToList();

                            if (footer.Count > 0)
                            {
                                #region Llave existente
                                detalleFOB = footerTempFOB.First();
                                detalleFOB.vlrMdaLoc.Value += deta.Valor2LOC.Value;

                                if (deta.Valor2EXT.Value.HasValue)
                                    detalleFOB.vlrMdaExt.Value += deta.Valor2EXT.Value;
                                #endregion
                            }
                            else
                            {
                                #region Nuevo detalle FOB
                                detalleFOB.CuentaID.Value = ctaFOB.ID.Value;
                                detalleFOB.ConceptoSaldoID.Value = ctaFOB.ConceptoSaldoID.Value;

                                detalleFOB.ProyectoID.Value = bodegaDTO.ProyectoID.Value;
                                detalleFOB.CentroCostoID.Value = bodegaDTO.CentroCostoID.Value;

                                detalleFOB.LineaPresupuestoID.Value = lineaXdef;
                                detalleFOB.TerceroID.Value = terceroXDef;
                                detalleFOB.ConceptoCargoID.Value = concCargoXdef;

                                detalleFOB.LugarGeograficoID.Value = lgXdef;
                                detalleFOB.PrefijoCOM.Value = ctrl.PrefijoID.Value;

                                detalleFOB.IdentificadorTR.Value = 0;
                                detalleFOB.TasaCambio.Value = ctrl.TasaCambioCONT.Value;
                                detalleFOB.DocumentoCOM.Value = ctrl.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno ? ctrl.DocumentoNro.Value.Value.ToString() : ctrl.DocumentoTercero.Value;
                                detalleFOB.vlrMdaLoc.Value = deta.Valor2LOC.Value;
                                detalleFOB.vlrMdaExt.Value = deta.Valor2EXT.Value;
                                detalleFOB.vlrBaseML.Value = 0;
                                detalleFOB.vlrBaseME.Value = 0;

                                footer.Add(detalleFOB);
                                #endregion
                            }
                        }
                        #endregion
                        #region Crea la contrapartida
                        DTO_ComprobanteFooter contra = new DTO_ComprobanteFooter();
                        ctaContra = null;

                        #region Trae la Linea Presupuestal

                        string linPrespContra = lineaXdef;
                        if (!string.IsNullOrEmpty(refGrupoDTO.LineaPresupuestoID.Value))
                            linPrespContra = refGrupoDTO.LineaPresupuestoID.Value;
                        else if (!string.IsNullOrEmpty(claseBS.LineaPresupuestoID.Value))
                            linPrespContra = claseBS.LineaPresupuestoID.Value;

                        #endregion
                        #region Trae la cuenta, proy y cto costo de la contra partida
                        string proyContra = !string.IsNullOrWhiteSpace(deta.ProyectoID.Value) ? deta.ProyectoID.Value : bodegaDTO.ProyectoID.Value;
                        string ctoCostoContra = !string.IsNullOrWhiteSpace(deta.CentroCostoID.Value) ? deta.CentroCostoID.Value : bodegaDTO.CentroCostoID.Value;

                        if (!string.IsNullOrWhiteSpace(concCargo.CuentaID.Value))
                        {
                            #region Cuenta
                            if (cacheCtas.ContainsKey(concCargo.CuentaID.Value))
                                ctaContra = cacheCtas[concCargo.CuentaID.Value];
                            else
                            {
                                ctaContra = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, concCargo.CuentaID.Value, true, false);
                                cacheCtas.Add(concCargo.CuentaID.Value, ctaContra);
                            }
                            #endregion
                        }
                        else
                        {
                            string operID;
                            #region Trae la operacion

                            //proyecto
                            if (cacheProys.ContainsKey(proyContra))
                                proy = cacheProys[proyContra];
                            else
                            {
                                proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, proyContra, true, false);
                                cacheProys.Add(proyContra, proy);
                            }

                            if (!string.IsNullOrWhiteSpace(proy.OperacionID.Value))
                                operID = proy.OperacionID.Value;
                            else
                            {
                                //Centros de costo
                                if (cacheCtosCosto.ContainsKey(ctoCostoContra))
                                    ctoCosto = cacheCtosCosto[ctoCostoContra];
                                else
                                {
                                    ctoCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, ctoCostoContra, true, false);
                                    cacheCtosCosto.Add(proyContra, ctoCosto);
                                }

                                operID = ctoCosto.OperacionID.Value;
                                if (string.IsNullOrWhiteSpace(operID))
                                {
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = i;
                                    rd.Message = DictionaryMessages.Err_Co_NoOper + "&&" + proyContra + "&&" + ctoCostoContra;

                                    result.Details.Add(rd);
                                }
                            }

                            #endregion
                            #region Trae la cuenta
                            if (!string.IsNullOrWhiteSpace(operID))
                            {
                                ctaContraId = this._moduloGlobal.coCargoCosto_GetCuentaIDByCargoOper(claseBS.ConceptoCargoID.Value, operID, linPrespContra);
                                if (cacheCtas.ContainsKey(ctaContraId))
                                    ctaContra = cacheCtas[ctaContraId];
                                else
                                {
                                    ctaContra = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaContraId, true, false);
                                    if (ctaContra == null)
                                    {
                                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                        rd.line = i;
                                        rd.Message = DictionaryMessages.Err_Co_NoCtaCargoCosto + "&&" + claseBS.ConceptoCargoID.Value + "&&" +
                                            linPrespContra + "&&" + proyContra + "&&" + ctoCostoContra;

                                        result.Details.Add(rd);
                                    }
                                    else
                                        cacheCtas.Add(ctaContraId, ctaContra);
                                }
                            }
                            #endregion
                        }
                        #endregion

                        if (ctaContra != null)
                        {
                            #region Agerga la contrapartida

                            List<DTO_ComprobanteFooter> footerContra =
                                contras.Where(x => x.CuentaID.Value == ctaCosto.ID.Value &&
                                                  x.ProyectoID.Value == proyContra &&
                                                  x.CentroCostoID.Value == ctoCostoContra &&
                                                  x.LineaPresupuestoID.Value == linPrespContra &&
                                                  x.ConceptoCargoID.Value == claseBS.ConceptoCargoID.Value &&
                                                  x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                                              ).ToList();

                            if (footerContra.Count > 0)
                            {
                                #region Llave existente
                                contra = footerTemp.First();
                                contra.vlrMdaLoc.Value += deta.Valor1LOC.Value * -1;

                                if (deta.Valor1EXT.Value.HasValue)
                                    contra.vlrMdaExt.Value += deta.Valor1EXT.Value * -1;
                                #endregion
                            }
                            else
                            {
                                #region Nuevo registro
                                contra.CuentaID.Value = ctaContra.ID.Value;
                                contra.ConceptoSaldoID.Value = ctaContra.ConceptoSaldoID.Value;
                                contra.IdentificadorTR.Value = ctrl.NumeroDoc.Value.Value;

                                contra.ProyectoID.Value = proyContra;
                                contra.CentroCostoID.Value = ctoCostoContra;

                                contra.LineaPresupuestoID.Value = linPrespContra;
                                contra.ConceptoCargoID.Value = claseBS.ConceptoCargoID.Value;
                                contra.TerceroID.Value = ctrl.TerceroID.Value;
                                contra.LugarGeograficoID.Value = lgXdef;
                                contra.PrefijoCOM.Value = ctrl.PrefijoID.Value;

                                contra.TasaCambio.Value = ctrl.TasaCambioCONT.Value;
                                contra.DocumentoCOM.Value = ctrl.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno ? ctrl.DocumentoNro.Value.Value.ToString() : ctrl.DocumentoTercero.Value;
                                contra.vlrMdaLoc.Value = deta.Valor1LOC.Value * -1;
                                contra.vlrMdaExt.Value = deta.Valor1EXT.Value * -1;
                                contra.vlrBaseML.Value = 0;
                                contra.vlrBaseME.Value = 0;

                                contra.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                                footer.Add(contra);
                                #endregion
                            }
                            #endregion
                        }

                        #endregion
                    }
                }
                #endregion

                if (result.Result == ResultValue.OK)
                {
                    comp.Header = header;
                    comp.Footer = footer;

                    return comp;
                }
                else
                    return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GenerarComprobantePosteo");
                return null;
            }
        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Proceso de posteo de comprobantes para el modulo de inventarios
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna la lista de resultados (uno por cada comprobante)</returns>
        public List<DTO_TxResult> PosteoComprobantes(int documentID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult(); 
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isOK = true;
            List<DTO_glDocumentoControl> docs = new List<DTO_glDocumentoControl>();
            Dictionary<string, DTO_coComprobante> cacheCoComps = new Dictionary<string,DTO_coComprobante>();
            try
            {
                #region Variables

                //Modulos y dals
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables de cache
                Dictionary<string, DTO_inReferencia> cacheReferencias = new Dictionary<string,DTO_inReferencia>();
                Dictionary<string, DTO_inBodega> cacheBodegas = new Dictionary<string,DTO_inBodega>();
                Dictionary<Tuple<string, string>, DTO_inContabiliza> cacheContabiliza = new Dictionary<Tuple<string,string>,DTO_inContabiliza>();
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string,DTO_coPlanCuenta>();
                Dictionary<string, DTO_inRefGrupo> cacheRefGrupos = new Dictionary<string,DTO_inRefGrupo>();
                Dictionary<string, DTO_coProyecto> cacheProys = new  Dictionary<string,DTO_coProyecto>();
                Dictionary<string, DTO_coCentroCosto> cacheCtosCosto = new Dictionary<string,DTO_coCentroCosto>();

                //Variables por defecto
                string terceroXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                string areaFunc = this.GetAreaFuncionalByUser();
                string prefijoID = this.GetPrefijoByDocumento(documentID);
                docs = this._moduloGlobal.glDocumentoControl_GetForPosteo(ModulesPrefix.@in, false);
                #endregion
                #region Crea los comprobantes
                int i = 0;
                foreach (DTO_glDocumentoControl ctrl in docs)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / docs.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    object obj = this.GenerarComprobantePosteo(ctrl, cacheCoComps, cacheReferencias, cacheBodegas, cacheContabiliza,
                        cacheCtas, cacheRefGrupos, cacheProys, cacheCtosCosto, terceroXDef, concCargoXdef, lgXdef, lineaXdef);

                    if (obj == null)
                    {
                        isOK = false;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddCompr;
                    }
                    else if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        isOK = false;
                        result = (DTO_TxResult)obj;
                    }
                    else
                    {
                        DTO_Comprobante comp = (DTO_Comprobante)obj;

                        if (comp.Footer.Count == 0)
                        {
                            isOK = false;
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_CompNoResults;
                        }
                        else
                        {
                            result = this._moduloContabilidad.ComprobantePre_Add(documentID, ModulesPrefix.@in, comp, areaFunc, prefijoID, false,
                                ctrl.NumeroDoc.Value.Value, null, new Dictionary<Tuple<int, int>, int>(), true);

                            if (result.Result == ResultValue.NOK)
                                isOK = false;
                        }
                    }

                    results.Add(result);
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                isOK = false;
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "PosteoComprobantes");
                results.Add(result);

                return results;
            }
            finally
            {
                if (isOK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        foreach (DTO_glDocumentoControl ctrl in docs)
                        {
                            if (!ctrl.ComprobanteIDNro.Value.HasValue || ctrl.ComprobanteIDNro.Value.Value == 0)
                            {
                                DTO_coComprobante coComp = cacheCoComps[ctrl.ComprobanteID.Value];
                                ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                                this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, false);
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, true);
                            }
                        }
                    }
                    else
                        throw new Exception("Posteo de comprobantes - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Aprueba una lista de posteo de comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento qu eejecuta la transaccion</param>
        /// <param name="mod">Modulo del cual se esta aprobando el posteo</param>
        /// <param name="docs">Lista de documentos a aprobar</param>
        /// <returns>Retorna el resultado dela operacion</returns>
        public List<DTO_TxResult> AprobarPosteo(int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_glDocumentoControl> docs, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isOK = true;
            try
            {
                #region Variables

                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string areaFunc = this.GetAreaFuncionalByUser();
                string prefijoID = this.GetPrefijoByDocumento(documentID);

                #endregion
                #region Aprueba los comprobantes
                int i = 0;
                foreach (DTO_glDocumentoControl ctrl in docs)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / docs.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    try
                    {
                        result = this._moduloContabilidad.Comprobante_Aprobar(documentID, actividadFlujoID, currentMod, ctrl.NumeroDoc.Value.Value, true, ctrl.PeriodoDoc.Value.Value,
                            ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, string.Empty, false, false, true, true);
                    }
                    catch (Exception exAprob)
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();

                        result.Result = ResultValue.NOK;
                        string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "AprobarPosteo");
                        rd.Message = DictionaryMessages.Err_AprobComp + "&&" + ctrl.PeriodoDoc.Value.Value.ToString() + "&&" +
                            ctrl.ComprobanteID.Value + "&&" + ctrl.ComprobanteIDNro.Value.Value + ". " + errMsg;
                        result.Details.Add(rd);
                    }

                    results.Add(result);
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                isOK = false;
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AprobarPosteoInv");
                results.Add(result);

                return results;
            }
            finally
            {
                if (isOK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #endregion

        #region MOVIMIENTOS

        #region Funciones Privadas
        
        /// <summary>
        /// Carga los movimientos de inventario que genera el Inventario Fisico
        /// </summary>
        /// <param name="mvtoEntrada">Movimientos de entrada</param>
        /// <param name="mvtoSalida">Movimientos de salida</param>
        private void LoadMvtoInventarioFisico(DTO_inBodega bodega, DateTime periodo, ref List<DTO_glMovimientoDeta> mvtoEntrada, ref List<DTO_glMovimientoDeta> mvtoSalida)
        {
            this._dal_inFisicoInventario = (DAL_inFisicoInventario)this.GetInstance(typeof(DAL_inFisicoInventario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            List<DTO_inFisicoInventario> listInvFisico = new List<DTO_inFisicoInventario>();
            DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
            try
            {
                fisico.BodegaID.Value = bodega.ID.Value;
                fisico.Periodo.Value = periodo;
                listInvFisico = this._dal_inFisicoInventario.DAL_inFisicoInventario_GetByParameter(fisico);
                foreach (var inv in listInvFisico)
                {
                    if (inv.CantAjuste.Value > 0)
                    {
                        #region Carga Movimientos de entrada
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        mov.NumeroDoc.Value = 0;
                        mov.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                        mov.BodegaID.Value = bodega.ID.Value;
                        mov.Fecha.Value = inv.Periodo.Value;
                        mov.inReferenciaID.Value = inv.inReferenciaID.Value;
                        mov.Parametro1.Value = inv.Parametro1.Value;
                        mov.Parametro2.Value = inv.Parametro2.Value;
                        mov.EstadoInv.Value = inv.EstadoInv.Value;
                        mov.PrefijoID.Value = bodega.PrefijoID.Value;
                        mov.ActivoID.Value = inv.ActivoID.Value;
                        mov.SerialID.Value = inv.SerialID.Value;
                        mov.CantidadUNI.Value = inv.CantAjuste.Value;
                        mov.CentroCostoID.Value = bodega.CentroCostoID.Value;
                        mov.ProyectoID.Value = bodega.ProyectoID.Value;
                        mov.Valor1LOC.Value = inv.CantKardex.Value != 0?((inv.CostoLocal.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value) : inv.CostoLocal.Value.Value;
                        mov.Valor1EXT.Value = inv.CantKardex.Value != 0 ? ((inv.CostoExtra.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value) : inv.CostoExtra.Value.Value;
                        mov.Valor2LOC.Value = inv.FobLocal.Value;
                        mov.Valor2EXT.Value = inv.FobExtra.Value;
                        mvtoEntrada.Add(mov);
                        #endregion
                    }
                    if (inv.CantAjuste.Value < 0)
                    {
                        #region Carga Movimientos de Salida
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        mov.NumeroDoc.Value = 0;
                        mov.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                        mov.BodegaID.Value = bodega.ID.Value;
                        mov.Fecha.Value = inv.Periodo.Value;
                        mov.inReferenciaID.Value = inv.inReferenciaID.Value;
                        mov.Parametro1.Value = inv.Parametro1.Value;
                        mov.Parametro2.Value = inv.Parametro2.Value;
                        mov.EstadoInv.Value = inv.EstadoInv.Value;
                        mov.PrefijoID.Value = bodega.PrefijoID.Value;
                        mov.ActivoID.Value = inv.ActivoID.Value;
                        mov.SerialID.Value = inv.SerialID.Value;
                        mov.CantidadUNI.Value = Math.Abs(inv.CantAjuste.Value.Value);
                        mov.CentroCostoID.Value = bodega.CentroCostoID.Value;
                        mov.ProyectoID.Value = bodega.ProyectoID.Value;
                        mov.Valor1LOC.Value = inv.CantKardex.Value != 0 ? (Math.Abs((inv.CostoLocal.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value.Value)) : inv.CostoLocal.Value.Value;
                        mov.Valor1EXT.Value = inv.CantKardex.Value != 0 ? (Math.Abs((inv.CostoExtra.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value.Value)) : inv.CostoExtra.Value.Value;
                        mov.Valor2LOC.Value = Math.Abs(inv.FobLocal.Value.Value);
                        mov.Valor2EXT.Value = Math.Abs(inv.FobExtra.Value.Value);
                        mvtoSalida.Add(mov);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "LoadMvtoInventarioFisico");
            }
        }

        #region inMovimientoDocu (Transaccion Manual/Nota Envio)

        /// <summary>
        /// Adiciona en la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        private DTO_TxResult inMovimientoDocu_Add(int documentoID, DTO_inMovimientoDocu header)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inMovimientoDocu.DAL_inMovimientoDocu_Add(header);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, header.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inMovimientoDocu_Add");
                return result;
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Crea un dto de reporte Transaccion Manual
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        /// <param name="isApro">Si es aprobado</param>
        /// <param name="transaccion">transaccion a mostrar</param>
        /// <returns>Objeto </returns>
        internal object DtoReportTansaccMnl(int numeroDoc, bool isApro, DTO_MvtoInventarios transaccion)
        {
            try
            {
                #region Variables

                //El Dto a Devolver
                List<DTO_inMovimientoFooter> _footer = new List<DTO_inMovimientoFooter>();
                DTO_MvtoInventarios mvtos = Transaccion_Get(numeroDoc);
                List<DTO_glMovimientoDeta> mvtoDetail = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(numeroDoc);

                foreach (DTO_glMovimientoDeta mov in mvtoDetail)
                {
                    DTO_inMovimientoFooter dtoItem = new DTO_inMovimientoFooter();
                    dtoItem.Movimiento = mov;
                    _footer.Add(dtoItem);
                }
                mvtos.Footer = _footer;

                DTO_glDocumentoControl mvtosDoc = mvtos.DocCtrl;
                DTO_inMovimientoDocu mvtosHeader = mvtos.Header;


                #endregion

                #region Asignar los datos para el reporte

                DTO_ReportTransaccionManual reportTransacc = new DTO_ReportTransaccionManual();

                if (transaccion == null)
                {
                    DTO_inMovimientoTipo movDesc = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, mvtosHeader.MvtoTipoInvID.Value, true, false);
                    #region Header
                    reportTransacc.Header.TerceroID = mvtos.DocCtrl.TerceroID.Value;
                    reportTransacc.Header.CentroCostoID = mvtos.DocCtrl.CentroCostoID.Value;
                    reportTransacc.Header.Fecha = mvtos.DocCtrl.Fecha.Value.Value;
                    reportTransacc.Header.BodegaOrigen = mvtosHeader.BodegaOrigenID.Value;
                    reportTransacc.Header.BodegaDestino = mvtosHeader.BodegaDestinoID.Value;
                    reportTransacc.Header.MvtoTipoInvID = movDesc.Descriptivo.Value;
                    reportTransacc.Header.EmpresaID = mvtosHeader.EmpresaID.Value;

                    if (isApro)
                        reportTransacc.isApro = true;
                    else
                        reportTransacc.isApro = false;

                    #endregion
                    #region Detail

                    foreach (DTO_inMovimientoFooter footer in _footer)
                    {
                        DTO_ReportTransacFooter reportLegaFooter = new DTO_ReportTransacFooter();
                        DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, footer.Movimiento.inReferenciaID.Value, true, false);

                        reportLegaFooter.inReferenciaID = footer.Movimiento.inReferenciaID.Value;
                        reportLegaFooter.DocSoporte = footer.Movimiento.DocSoporte.Value.Value;
                        reportLegaFooter.DescripReferencia = inRefDesc.Descriptivo.Value;
                        reportLegaFooter.CantidadUni = footer.Movimiento.CantidadUNI.Value.Value;
                        reportLegaFooter.Serial = footer.Movimiento.SerialID.Value;

                        reportTransacc.Footer.Add(reportLegaFooter);
                    }
                    #endregion
                }
                else
                {
                    DTO_inMovimientoDocu mvtosHeaderTransac = transaccion.Header;
                    foreach (DTO_inMovimientoFooter mov in transaccion.Footer)
                    {
                        _footer.Add(mov);
                    }
                    mvtos.Footer = _footer;
                    #region Header
                    reportTransacc.Header.TerceroID = transaccion.DocCtrl.TerceroID.Value;
                    reportTransacc.Header.CentroCostoID = transaccion.DocCtrl.CentroCostoID.Value;
                    reportTransacc.Header.Fecha = transaccion.DocCtrl.Fecha.Value.Value;
                    reportTransacc.Header.BodegaOrigen = mvtosHeaderTransac.BodegaOrigenID.Value;
                    reportTransacc.Header.BodegaDestino = mvtosHeaderTransac.BodegaDestinoID.Value;
                    reportTransacc.Header.MvtoTipoInvID = mvtosHeaderTransac.MvtoTipoInvID.Value;
                    reportTransacc.Header.EmpresaID = mvtosHeaderTransac.EmpresaID.Value;

                    if (isApro)
                        reportTransacc.isApro = true;
                    else
                        reportTransacc.isApro = false;

                    #endregion
                    #region Detail

                    foreach (DTO_inMovimientoFooter footer in _footer)
                    {
                        DTO_ReportTransacFooter reportLegaFooter = new DTO_ReportTransacFooter();
                        DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, footer.Movimiento.inReferenciaID.Value, true, false);

                        reportLegaFooter.inReferenciaID = footer.Movimiento.inReferenciaID.Value;
                        reportLegaFooter.DocSoporte = footer.Movimiento.DocSoporte.Value.Value;
                        reportLegaFooter.DescripReferencia = inRefDesc.Descriptivo.Value;
                        reportLegaFooter.CantidadUni = footer.Movimiento.CantidadUNI.Value.Value;
                        reportLegaFooter.Serial = footer.Movimiento.SerialID.Value;

                        reportTransacc.Footer.Add(reportLegaFooter);
                    }
                    #endregion
                }

                #endregion

                return reportTransacc;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportTansaccMnl");
                return null;
            }
        }

        /// <summary>
        /// Crea un dto de reporte Nota de Envio
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        /// <param name="isApro">Si es aprobado</param>
        /// <param name="transaccion">transaccion a mostrar</param>
        /// <returns>Objeto </returns>
        internal object DtoReportNotaEnvio(int numeroDoc, bool isApro, DTO_MvtoInventarios transaccion)
        {
            try
            {
                #region Variables

                //El Dto a Devolver
                List<DTO_inMovimientoFooter> _footer = new List<DTO_inMovimientoFooter>();
                DTO_MvtoInventarios mvtos = Transaccion_Get(numeroDoc);
                DTO_glMovimientoDeta mvtoDetaFilter = new DTO_glMovimientoDeta();
                mvtoDetaFilter.NumeroDoc.Value = numeroDoc;
                mvtoDetaFilter.EntradaSalida.Value = Convert.ToByte(EntradaSalida.Salida);
                List<DTO_glMovimientoDeta> mvtoDetail = this._moduloGlobal.glMovimientoDeta_GetByParameter(mvtoDetaFilter, false);

                foreach (DTO_glMovimientoDeta mov in mvtoDetail)
                {
                    DTO_inMovimientoFooter dtoItem = new DTO_inMovimientoFooter();
                    dtoItem.Movimiento = mov;
                    _footer.Add(dtoItem);
                }
                mvtos.Footer = _footer;

                DTO_glDocumentoControl mvtosDoc = mvtos.DocCtrl;
                DTO_inMovimientoDocu mvtosHeader = mvtos.Header;

                #endregion

                #region Asignar los datos para el reporte

                DTO_ReportNotaEnvio reportNotaE = new DTO_ReportNotaEnvio();

                DTO_inMovimientoTipo movDesc = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, mvtosHeader.MvtoTipoInvID.Value, true, false);
                DTO_inBodega bodegaOrigenDesc = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, mvtosHeader.BodegaOrigenID.Value, true, false);
                DTO_inBodega bodegaDestDesc = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, mvtosHeader.BodegaDestinoID.Value, true, false);

                if (transaccion == null)
                {
                    if (isApro)
                        reportNotaE.isApro = true;
                    else
                        reportNotaE.isApro = false;

                    #region Header
                    reportNotaE.Header.Cliente = mvtos.Header.ClienteID.Value;
                    reportNotaE.Header.TipoVehiculo = mvtos.Header.DatoAdd2.Value;
                    reportNotaE.Header.Conductor = mvtos.Header.DatoAdd4.Value;
                    reportNotaE.Header.Placa = mvtos.Header.DatoAdd3.Value;
                    reportNotaE.Header.BodegaOrigen = bodegaOrigenDesc.Descriptivo.Value;
                    if (bodegaDestDesc != null)
                        reportNotaE.Header.BodegaDestino = bodegaDestDesc.Descriptivo.Value;
                    else
                        reportNotaE.Header.BodegaDestino = mvtosHeader.BodegaDestinoID.Value;
                    reportNotaE.Header.MvtoTipoInvID = movDesc.Descriptivo.Value;
                    reportNotaE.Header.cedula = mvtos.Header.DatoAdd5.Value;
                    reportNotaE.Header.Fecha = mvtos.DocCtrl.Fecha.Value.Value;
                    reportNotaE.Header.Observacion = mvtos.Header.Observacion.Value;
                    reportNotaE.Header.Documento = mvtos.DocCtrl.PrefijoID.Value + "_" + mvtos.DocCtrl.NumeroDoc.Value;

                    if (isApro)
                        reportNotaE.isApro = true;
                    else
                        reportNotaE.isApro = false;

                    #endregion
                    #region Detail

                    foreach (DTO_inMovimientoFooter footer in _footer)
                    {
                        DTO_ReportNotaEnvioFooter reportLegaFooter = new DTO_ReportNotaEnvioFooter();
                        DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, footer.Movimiento.inReferenciaID.Value, true, false);

                        reportLegaFooter.inReferenciaID = footer.Movimiento.inReferenciaID.Value;
                        reportLegaFooter.DescripReferencia = inRefDesc.Descriptivo.Value;
                        reportLegaFooter.CantidadUni = footer.Movimiento.CantidadUNI.Value.Value;
                        reportLegaFooter.Serial = footer.Movimiento.SerialID.Value;

                        reportNotaE.Footer.Add(reportLegaFooter);
                    }
                    #endregion
                    #region Temporal
                    //if (transaccion == null)
                    //{

                    //}
                    //else
                    //{
                    //    DTO_inMovimientoHeader mvtosHeaderTransac = transaccion.Header;
                    //    foreach (DTO_inMovimientoFooter mov in transaccion.Footer)
                    //    {
                    //        _footer.Add(mov);
                    //    }
                    //    mvtos.Footer = _footer;
                    //    #region Header
                    //    reportTransacc.Header.TerceroID = transaccion.DocCtrl.TerceroID.Value;
                    //    reportTransacc.Header.CentroCostoID = transaccion.DocCtrl.CentroCostoID.Value;
                    //    reportTransacc.Header.Fecha = transaccion.DocCtrl.Fecha.Value.Value;
                    //    reportTransacc.Header.BodegaOrigen = mvtosHeaderTransac.BodegaOrigenID.Value;
                    //    reportTransacc.Header.BodegaDestino = mvtosHeaderTransac.BodegaDestinoID.Value;
                    //    reportTransacc.Header.MvtoTipoInvID = mvtosHeaderTransac.MvtoTipoInvID.Value;
                    //    reportTransacc.Header.EmpresaID = mvtosHeaderTransac.EmpresaID.Value;

                    //    if (isApro)
                    //        reportTransacc.isApro = true;
                    //    else
                    //        reportTransacc.isApro = false;

                    //    #endregion
                    //    #region Detail

                    //    foreach (DTO_inMovimientoFooter footer in _footer)
                    //    {
                    //        DTO_ReportTransacFooter reportLegaFooter = new DTO_ReportTransacFooter();
                    //        DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, footer.Movimiento.inReferenciaID.Value, true, false);

                    //        reportLegaFooter.inReferenciaID = footer.Movimiento.inReferenciaID.Value;
                    //        reportLegaFooter.DocSoporte = footer.Movimiento.DocSoporte.Value.Value;
                    //        reportLegaFooter.DescripReferencia = inRefDesc.Descriptivo.Value;
                    //        reportLegaFooter.CantidadUni = footer.Movimiento.CantidadUNI.Value.Value;
                    //        reportLegaFooter.Serial = footer.Movimiento.SerialID.Value;

                    //        reportTransacc.Footer.Add(reportLegaFooter);
                    //    }
                    //    #endregion
                    //}

                    #endregion
                    #region Footer

                    reportNotaE.Detail.EnviadoPor = UserId.ToString();

                    #endregion
                }
                else
                {
                    DTO_inMovimientoTipo moviDesc = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, transaccion.Header.MvtoTipoInvID.Value, true, false);
                    DTO_inBodega bodegaOriDesc = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, transaccion.Header.BodegaOrigenID.Value, true, false);
                    DTO_inBodega bodegaDesDesc = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, transaccion.Header.BodegaDestinoID.Value, true, false);
                    #region Header
                    reportNotaE.Header.Cliente = transaccion.Header.ClienteID.Value;
                    reportNotaE.Header.TipoVehiculo = transaccion.Header.DatoAdd2.Value;
                    reportNotaE.Header.Conductor = transaccion.Header.DatoAdd4.Value;
                    reportNotaE.Header.Placa = transaccion.Header.DatoAdd3.Value;
                    reportNotaE.Header.BodegaOrigen = bodegaOriDesc.Descriptivo.Value;
                    if (bodegaDestDesc != null)
                        reportNotaE.Header.BodegaDestino = bodegaDesDesc.Descriptivo.Value;
                    else
                        reportNotaE.Header.BodegaDestino = transaccion.Header.BodegaDestinoID.Value;
                    reportNotaE.Header.MvtoTipoInvID = moviDesc.Descriptivo.Value;
                    reportNotaE.Header.cedula = transaccion.Header.DatoAdd5.Value;
                    reportNotaE.Header.Fecha = transaccion.DocCtrl.Fecha.Value.Value;
                    reportNotaE.Header.Observacion = transaccion.Header.Observacion.Value;
                    reportNotaE.Header.Documento = transaccion.DocCtrl.PrefijoID.Value + "_" + transaccion.DocCtrl.NumeroDoc.Value;

                    if (isApro)
                        reportNotaE.isApro = true;
                    else
                        reportNotaE.isApro = false;

                    #endregion
                    DTO_inMovimientoDocu mvtosHeaderTransac = transaccion.Header;
                    foreach (DTO_inMovimientoFooter mov in transaccion.Footer)
                    {
                        _footer.Add(mov);
                    }
                    mvtos.Footer = _footer;
                    #region Detail

                    foreach (DTO_inMovimientoFooter footer in _footer)
                    {
                        DTO_ReportNotaEnvioFooter reportLegaFooter = new DTO_ReportNotaEnvioFooter();
                        DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, footer.Movimiento.inReferenciaID.Value, true, false);

                        reportLegaFooter.inReferenciaID = footer.Movimiento.inReferenciaID.Value;
                        reportLegaFooter.DescripReferencia = inRefDesc.Descriptivo.Value;
                        reportLegaFooter.CantidadUni = footer.Movimiento.CantidadUNI.Value.Value;
                        reportLegaFooter.Serial = footer.Movimiento.SerialID.Value;

                        reportNotaE.Footer.Add(reportLegaFooter);
                    }
                    #endregion
                    #region Footer

                    reportNotaE.Detail.EnviadoPor = UserId.ToString();

                    #endregion
                }
                #endregion

                return reportNotaE;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNotaEnvio");
                return null;
            }
        }
        #endregion

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Transaccion_Add(int documentID, DTO_MvtoInventarios transaccion, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, bool isRecibidoProv = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;

            DTO_coComprobante coComprob = new DTO_coComprobante();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_faFacturaDocu = (DAL_faFacturaDocu)base.GetInstance(typeof(DAL_faFacturaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)base.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)base.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal valorMvto = 0;
                #region Trae la actividad del documento
                List<string> actFlujoID = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                if (actFlujoID.Count == 0)
                {
                    numeroDoc = 0;
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_DocMultActivities;
                    return result;
                } 
                #endregion
                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    transaccion.DocCtrl.DocumentoNro.Value = 0;
                    transaccion.DocCtrl.ComprobanteIDNro.Value = 0;
                    foreach (var item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (transaccion.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    transaccion.DocCtrl.Valor.Value = valorMvto;                  
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, transaccion.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);

                        //numeroDoc = 0;
                        return result;
                    }                 
                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                    //transaccion.DocCtrl.NumeroDoc.Value = numeroDoc;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guardar en inMovimientoDocu
                    transaccion.Header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    if (documentID == AppDocuments.NotaEnvio)
                    {
                        transaccion.Header.NotaEnvioREL.Value = Convert.ToInt32(resultGLDC.Key);
                        #region Actualiza Factura Venta relacionada
                        if (transaccion.Footer.Count == 0)
                        {
                            DTO_faFacturaDocu facturaDoc = this._moduloFacturacion.faFacturaDocu_Get(transaccion.Header.DocumentoREL.Value.Value);
                            facturaDoc.NotaEnvioREL.Value = Convert.ToInt32(resultGLDC.Key);
                            this._dal_faFacturaDocu.DAL_faFacturaDocu_Upd(facturaDoc,false);
                        } 
                        #endregion
                    }
                    result = this.inMovimientoDocu_Add(documentID, transaccion.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, numeroDoc.ToString(), string.Empty, string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                    #endregion
                    #region Guardar en glMovimientoDeta
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    DTO_inBodega bodega = null;
                    bool bodegaTransaccional = false;
                    string bodegaID = string.Empty;
                    DTO_inMovimientoTipo tipoMov = (DTO_inMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, transaccion.Header.MvtoTipoInvID.Value, true, false);
                    if (tipoMov.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                        bodegaID = transaccion.Header.BodegaOrigenID.Value;
                    else if (tipoMov.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                        bodegaID = transaccion.Header.BodegaDestinoID.Value;
                    if (tipoMov.TipoMovimiento.Value != (byte)TipoMovimientoInv.Salidas)
                    {
                        bodega = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaID, true, false);
                        DTO_inCosteoGrupo costeo = (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodega.CosteoGrupoInvID.Value, true, false);
                        bodegaTransaccional = costeo.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? true : false;
                    }
                    foreach (DTO_inMovimientoFooter item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();                    
                        item.Movimiento.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        item.Movimiento.DatoAdd3.Value = item.Movimiento.DocSoporte.Value.ToString();
                        item.Movimiento.MvtoTipoInvID.Value = transaccion.Header.MvtoTipoInvID.Value;
                        mov = item.Movimiento;
                        movDeta.Add(mov);
                    }
                    if (movDeta.Any(x =>string.IsNullOrEmpty(x.BodegaID.Value)))
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "La bodega en el detalle esta vacía, revisar";
                        return result;
                    } 
                    result = this._moduloGlobal.glMovimientoDeta_Add(movDeta,true,true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #region Valida si es traslado y si es prestamo de proyectos
                    if (tipoMov.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados &&
                        transaccion.Header.TipoTraslado.Value.HasValue && transaccion.Header.TipoTraslado.Value == (byte)TipoTraslado.PrestamoProyecto)
                    {
                        List < DTO_glMovimientoDeta > detaExist = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(numeroDoc);
                        foreach (var d in detaExist)
                            d.ConsecutivoPrestamo.Value = d.Consecutivo.Value;
                        this._moduloGlobal.glMovimientoDeta_Update(detaExist, true);
                    }
                    #endregion

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza el documento control(cambia el estado)
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, string.Empty, true);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region PROCESA MOVIMIENTOS DE INVENTARIOS
                    result = this.Transaccion_Aprobar(documentID, numeroDoc,actFlujoID[0], true, batchProgress, true, isRecibidoProv);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    transaccion.DocCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza Cantidad de Inv de Proyectos si es BodegaStock
                    if (bodega != null)
                    {
                        DTO_inBodegaTipo bodTipo = (DTO_inBodegaTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, bodega.BodegaTipoID.Value, true, false);
                        if (bodTipo.BodegaTipo.Value == (byte)TipoBodega.Stock && tipoMov.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                        {
                            foreach (var mvto in transaccion.Footer)
                            {
                                DTO_pyProyectoMvto mvtoProyecto = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(mvto.Movimiento.DocSoporte.Value);
                                if (mvtoProyecto != null)
                                {
                                    mvtoProyecto.CantidadINV.Value = mvtoProyecto.CantidadINV.Value + mvto.Movimiento.CantidadUNI.Value;
                                    this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoProyecto);
                                }
                            }  
                        }
                    }
                    #endregion                       
                    #region Guardar en prDetalleDocu
                    this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_inMovimientoFooter inv in transaccion.Footer.FindAll(x => x.DetalleSolicitud.Count > 0))
                    {
                        foreach (DTO_prSolicitudResumen detSol in inv.DetalleSolicitud)
                        {
                            DTO_prDetalleDocu d = this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByID(detSol.ConsecutivoDetaID.Value.Value);
                            d.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            //d.DatoAdd5.Value = "Inventario";
                            d.CantidadINV.Value = inv.Movimiento.CantidadUNI.Value;
                            d.ValorTotML.Value = 0;
                            d.IvaTotML.Value = 0;
                            d.ValorTotME.Value = 0;
                            d.IvaTotME.Value = 0;
                            d.ValorUni.Value = 0;
                            d.CantidadSol.Value = d.CantidadINV.Value * -1;
                            d.InventarioDocuID.Value = d.NumeroDoc.Value;
                            d.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(d);
                            d.InventarioDetaID.Value = d.ConsecutivoDetaID.Value;
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(d);
                        }
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion                    
                    #region Contabiliza el movimiento
                    #region Crear Comprobante
                    DTO_Comprobante comp = new DTO_Comprobante();
                    var res = this._dal_mvtoSaldosCostos.DAL_MvtoSaldosCostos_GetComprobanteMvto(numeroDoc);
                    if (res.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)res;
                        return result;
                    }
                    else
                    {
                        comp = (DTO_Comprobante)res;
                        coComprob = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);
                        transaccion.DocCtrl.ComprobanteID.Value = comp.Header.ComprobanteID.Value;
                        this._moduloGlobal.glDocumentoControl_Update(transaccion.DocCtrl, false, true);
                    }
    
                    comp.coDocumentoID = transaccion.DocCtrl.DocumentoID.Value.Value.ToString();
                    comp.PrefijoID = transaccion.DocCtrl.PrefijoID.Value.ToString();
                    comp.DocumentoNro = transaccion.DocCtrl.DocumentoNro.Value.Value;
                    comp.CuentaID = transaccion.DocCtrl.CuentaID.Value;
                    comp.TerceroID = transaccion.DocCtrl.TerceroID.Value;
                    comp.ProyectoID = transaccion.DocCtrl.ProyectoID.Value;
                    comp.CentroCostoID = transaccion.DocCtrl.CentroCostoID.Value;
                    comp.LineaPresupuestoID = transaccion.DocCtrl.LineaPresupuestoID.Value;
                    comp.LugarGeograficoID = transaccion.DocCtrl.LugarGeograficoID.Value;
                    #endregion
                    #region Guardar comprobante
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, comp.Header.PeriodoID.Value.Value, ModulesPrefix.@in, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        if (!update)
                            numeroDoc = 0;

                        return result;
                    }
                    transaccion.DocCtrl.NumeroDoc.Value = numeroDoc;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #endregion
                }
                else
                {                
                    #region Actualiza glDocumentoControl
                    foreach (var item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (transaccion.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    transaccion.DocCtrl.Valor.Value = valorMvto;

                    this._moduloGlobal.glDocumentoControl_Update(transaccion.DocCtrl, true, true);
                    this.AsignarFlujo(transaccion.DocCtrl.DocumentoID.Value.Value, transaccion.DocCtrl.NumeroDoc.Value.Value, actFlujoID[0], false, string.Empty);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza en inMovimientoDocu
                    transaccion.Header.NumeroDoc.Value = transaccion.DocCtrl.NumeroDoc.Value;
                    result = this.inMovimientoDocu_Upd(documentID, transaccion.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;                    
                    #region Actualiza en glMovimientoDeta
                    foreach (DTO_inMovimientoFooter item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                       this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Update(mov);
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                }
                numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
              
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {                  
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(numeroDoc, true);
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
                return result;
            }
            catch (Exception ex)
            {
                transaccion.DocCtrl.NumeroDoc.Value = 0;
                numeroDoc = update ? transaccion.Header.NumeroDoc.Value.Value : 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Transaccion_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (transaccion.DocCtrl.DocumentoNro.Value == 0)
                        {
                            transaccion.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, transaccion.DocCtrl.PrefijoID.Value);
                            transaccion.DocCtrl.ComprobanteID.Value = coComprob.ID.Value;
                            transaccion.DocCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComprob, transaccion.DocCtrl.PrefijoID.Value, transaccion.DocCtrl.PeriodoDoc.Value.Value, transaccion.DocCtrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(transaccion.DocCtrl, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(transaccion.DocCtrl.NumeroDoc.Value.Value, transaccion.DocCtrl.ComprobanteIDNro.Value.Value, false);

                            alarma.Consecutivo = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                {
                    if(transaccion != null)
                       transaccion.DocCtrl.NumeroDoc.Value = 0;
                    base._mySqlConnectionTx.Rollback();
                }
            }         
        }

        /// <summary>
        ///Obtiene una transaccion manual
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <param name="trasladoNotaEnvio">Indica si es traslado de Nota Envio</param>
        /// <returns>DTO_inControlSaldosCostos</returns>
        public DTO_MvtoInventarios Transaccion_Get(int numeroDoc,bool trasladoNotaEnvio = false)
        {
            try
            {
                DTO_MvtoInventarios _mvto = new DTO_MvtoInventarios();
                DTO_glDocumentoControl _docCtrl = new DTO_glDocumentoControl();
                DTO_inMovimientoDocu _header = new DTO_inMovimientoDocu();
                List<DTO_inMovimientoFooter> _footer = new List<DTO_inMovimientoFooter>();
                string param1xDef = this.GetControlValueByCompany(ModulesPrefix.@in,AppControl.in_Parametro1xDefecto);
                string param2xDef = this.GetControlValueByCompany(ModulesPrefix.@in,AppControl.in_Parametro2xDefecto);

                #region Trae el Doc Control
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                _docCtrl = _moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                _mvto.DocCtrl = _docCtrl; 
                #endregion

                #region Trae el Header
                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(NewAge.ADO.DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                _header = this._dal_inMovimientoDocu.DAL_inMovimientoDocu_Get(numeroDoc);
                _mvto.Header = _header; 
                #endregion

                #region Trae el detalle
                //Obtiene de glMovimientoDeta y llena el movimientoFooter
                List<DTO_glMovimientoDeta> mvtoDetail;
                if (_mvto.DocCtrl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado || _mvto.DocCtrl.Estado.Value.Value == (byte)EstadoDocControl.Revertido)// && _mvto.Header.NotaEnvioREL.Value != null && _mvto.Header.NotaEnvioREL.Value == _mvto.DocCtrl.NumeroDoc.Value.Value)
                {
                    if (trasladoNotaEnvio)
                    {
                        #region Trae el detalle con filtro
                        DTO_glMovimientoDeta mvto = new DTO_glMovimientoDeta();
                        mvto.NumeroDoc.Value = _docCtrl.NumeroDoc.Value.Value;
                        mvto.BodegaID.Value = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
                        mvto.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                        mvtoDetail = this._moduloGlobal.glMovimientoDeta_GetByParameter(mvto, false);
                        #endregion
                    }
                    else
                        mvtoDetail = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(_docCtrl.NumeroDoc.Value.Value);
                }
                else
                    return null;

                #region Completa informacion del detalle
                string refP1P2ID = string.Empty;
                string refP1P2Desc = string.Empty;
                foreach (DTO_glMovimientoDeta mov in mvtoDetail)
                {
                    DTO_inMovimientoFooter dtoItem = new DTO_inMovimientoFooter();
                    dtoItem.Movimiento = mov;
                    DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, mov.inReferenciaID.Value,true,false);
                    dtoItem.UnidadRef.Value = referencia.UnidadInvID.Value;
                    if (string.IsNullOrEmpty(dtoItem.Movimiento.EmpaqueInvID.Value))
                        dtoItem.Movimiento.EmpaqueInvID.Value = referencia.EmpaqueInvID.Value;
                    refP1P2ID = referencia.ID.Value;
                    refP1P2Desc = referencia.Descriptivo.Value;
                    if (!string.IsNullOrEmpty(mov.Parametro1.Value) && mov.Parametro1.Value != param1xDef.ToUpper())
                    {
                        DTO_MasterBasic param1 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro1, mov.Parametro1.Value, true, false);
                        refP1P2ID += "-" + param1.ID.Value;
                        refP1P2Desc += "-" + param1.Descriptivo.Value;
                    }
                    if (!string.IsNullOrEmpty(mov.Parametro2.Value) && mov.Parametro2.Value != param2xDef.ToUpper())
                    {
                        DTO_MasterBasic param2 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro2, mov.Parametro2.Value, true, false);
                        refP1P2ID += "-" + param2.ID.Value;
                        refP1P2Desc += "-" + param2.Descriptivo.Value;
                    }
                    dtoItem.ReferenciaIDP1P2.Value = refP1P2ID;
                    dtoItem.ReferenciaIDP1P2Desc.Value = refP1P2Desc;
                    _footer.Add(dtoItem);
                } 
                #endregion
                _mvto.Footer = _footer; 
                #endregion

                return _mvto;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Transaccion_Get");
                return new DTO_MvtoInventarios();
            }
        }

        /// <summary>
        /// Envia para aprobacion una transaccion manual
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la transacción</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Transaccion_SendToAprob(int documentID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_MvtoInventarios mvto = this.Transaccion_Get(numeroDoc);
                List<string> actFlujoID = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                if (actFlujoID.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_DocMultActivities;
                    return result;
                }
                if (mvto != null)
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), mvto.DocCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    result = this.Transaccion_Aprobar(documentID, numeroDoc, actFlujoID[0], createDoc, batchProgress, insideAnotherTx, false);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_SendToAprobCompr;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Transaccion_SendToAprob");
                return result;
            }
        }

        /// <summary>
        /// Aprueba una transaccion Manual
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="numeroDoc">numero identificador de la transaccion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult Transaccion_Aprobar(int documentID, int numeroDoc, string actFlujoID, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, bool isRecibidoProv)
        {
            #region Variables
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            DTO_glDocumentoControl _dtoControl = null;
            DTO_inSaldosExistencias saldos = null;
            DTO_inCostosExistencias costos = null;
            DTO_inBodega bodegaOrigen = null;
            DTO_inCosteoGrupo costeoOrigen = null;
            DTO_inBodega bodegaDestino = null;
            DTO_inCosteoGrupo costeoDestino = null;
            bool modulePlaneacionActive = false; 
            #endregion

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)this.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedor = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DateTime periodoInv = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));

                if (documentID != AppDocuments.DistribucionCostos && documentID != AppDocuments.DeterioroInvAprob && documentID != AppDocuments.RevalorizacionInvAprob)
                {
                    #region Carga datos
                    string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    string monedaExtrangera = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    string parametro1xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                    string parametro2xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                 
                    List<DTO_glMovimientoDeta> listMvtoDeta = new List<DTO_glMovimientoDeta>();
                    string bodegaTransacional = string.Empty;
                    int regSaldos = 0;
                    int regCostos = 0;     

                    DTO_MvtoInventarios transaccion = this.Transaccion_Get(numeroDoc);
                    _dtoControl = transaccion.DocCtrl;
                    bodegaOrigen = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, transaccion.Header.BodegaOrigenID.Value, true, false);
                    costeoOrigen = (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodegaOrigen.CosteoGrupoInvID.Value, true, false);
                    DTO_inMovimientoTipo movimientoTipo = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, transaccion.Header.MvtoTipoInvID.Value, true, false);
                    if (movimientoTipo != null && movimientoTipo.TipoMovimiento.Value != (byte)TipoMovimientoInv.Entradas)
                    {
                        bodegaDestino = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, transaccion.Header.BodegaDestinoID.Value, true, false);
                        costeoDestino = bodegaDestino != null ? (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodegaDestino.CosteoGrupoInvID.Value, true, false) : null;
                    }

                    if (movimientoTipo != null && movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados && documentID == AppDocuments.NotaEnvio)
                        transaccion = this.Transaccion_Get(numeroDoc, true);
                           
                    #endregion
                    #region Realiza Mvto Recorriendo el detalle
                    foreach (DTO_inMovimientoFooter footer in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mvtoDeta = (DTO_glMovimientoDeta)footer.Movimiento;

                        #region 1. Revisa si existe el movimiento en saldos y costos
                        DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                        saldosCostos.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        saldosCostos.Periodo.Value = periodoInv;// _dtoControl.DocumentoAnula.Value.HasValue && _dtoControl.PeriodoAnula.Value.HasValue? _dtoControl.PeriodoAnula.Value : periodoInv;
                        saldosCostos.BodegaID.Value = mvtoDeta.BodegaID.Value;
                        saldosCostos.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        saldosCostos.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        saldosCostos.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        saldosCostos.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        saldosCostos.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        saldosCostos.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.IdentificadorTr.Value : null;
                        DTO_inControlSaldosCostos saldosCostosResult = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);

                        #endregion

                        #region 2. Entradas
                        if (movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                        {
                            if (costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto)
                            {
                                #region Agrega Activos si existen referencias serializadas
                                if (!string.IsNullOrEmpty(mvtoDeta.SerialID.Value))
                                {
                                    DTO_acActivoControl activo = new DTO_acActivoControl();
                                    activo.BodegaID.Value = bodegaOrigen.ID.Value;
                                    activo.ProyectoID.Value = bodegaOrigen.ProyectoID.Value;
                                    activo.CentroCostoID.Value = _dtoControl.CentroCostoID.Value;
                                    activo.DocumentoID.Value = _dtoControl.DocumentoID.Value;
                                    activo.DocumentoTercero.Value = _dtoControl.DocumentoTercero.Value;
                                    activo.EmpresaID.Value = _dtoControl.EmpresaID.Value;
                                    activo.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                    activo.Fecha.Value = _dtoControl.Fecha.Value;
                                    activo.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                    activo.MonedaID.Value = _dtoControl.MonedaID.Value;
                                    activo.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                                    activo.NumeroDocCompra.Value = _dtoControl.NumeroDoc.Value;
                                    activo.Observacion.Value = mvtoDeta.DescripTExt.Value;
                                    activo.Periodo.Value = _dtoControl.PeriodoDoc.Value;
                                    activo.ProyectoID.Value = _dtoControl.ProyectoID.Value;
                                    activo.Responsable.Value = _dtoControl.TerceroID.Value;
                                    activo.SerialID.Value = mvtoDeta.SerialID.Value;
                                    activo.TerceroID.Value = _dtoControl.TerceroID.Value;
                                    activo.Tipo.Value = (byte)TipoActivo.Inventario;
                                    activo.DatoAdd4.Value = mvtoDeta.DocSoporte.Value.ToString();
                                    activo.VidaUtilLOC.Value = 0;
                                    activo.VidaUtilIFRS.Value = 0;
                                    activo.TipoDepreIFRS.Value = 0;
                                    activo.VidaUtilUSG.Value = 0;
                                    activo.ValorSalvamentoLOC.Value = 0;
                                    activo.ValorSalvamentoUSG.Value = 0;
                                    activo.ValorSalvamentoIFRS.Value = 0;
                                    activo.ValorSalvamentoIFRSUS.Value = 0;
                                    activo.ValorRetiroIFRS.Value = 0;
                                    activo.NumeroDocUltMvto.Value = 0;
                                    this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                    mvtoDeta.ActivoID.Value = this._dal_acActivoControl.DAL_acActivoControl_Add(activo);
                                }
                                #endregion

                                if (saldosCostosResult == null)
                                {
                                    #region Agrega un Saldo de Existencias
                                    DTO_inSaldosExistencias saldosNew = new DTO_inSaldosExistencias();
                                    saldosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                    saldosNew.BodegaID.Value = transaccion.Header.BodegaOrigenID.Value;
                                    saldosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                    saldosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                    saldosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                    saldosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                    saldosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                    saldosNew.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                    saldosNew.CantInicial.Value = 0;
                                    saldosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                    saldosNew.CantRetiro.Value = 0;
                                    regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                                    #endregion
                                    #region Agrega un Costo de Existencias
                                    DTO_inCostosExistencias costosNew = new DTO_inCostosExistencias(true);
                                    costosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                    costosNew.CosteoGrupoInvID.Value = bodegaOrigen.CosteoGrupoInvID.Value;
                                    costosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                    costosNew.ActivoID.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Individual?  mvtoDeta.ActivoID.Value : 0;
                                    costosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                    costosNew.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                    costosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                    costosNew.FobLocEntrada.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                    costosNew.CtoLocEntrada.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                    costosNew.FobExtEntrada.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                    costosNew.CtoExtEntrada.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value; 
                                    if (movimientoTipo.LibroIFRSInd.Value.Value)
                                    {                                                                            
                                        costosNew.FobLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                        costosNew.CtoLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                        costosNew.FobExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                        costosNew.CtoExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value; 
                                    }
                                    #region Si existe el costo lo actualiza  sino lo agrega
                                    DTO_inCostosExistencias costoExist = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_GetByParameter(costosNew);
                                    if (costoExist != null)
                                    {
                                        costoExist.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                        costoExist.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                        costoExist.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                        costoExist.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                        costoExist.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                        if (movimientoTipo.LibroIFRSInd.Value.Value)
                                        {
                                            costoExist.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                            costoExist.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                            costoExist.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                            costoExist.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costoExist);
                                        regCostos = costoExist.Consecutivo.Value.Value;
                                    }
                                    else
                                        regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew); 
                                    #endregion
                                    #endregion
                                    #region Agrega un Control de Saldos y Costos
                                    DTO_inControlSaldosCostos saldosCostosNew = new DTO_inControlSaldosCostos();
                                    saldosCostosNew.EmpresaID.Value = transaccion.Header.EmpresaID.Value;
                                    saldosCostosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                    saldosCostosNew.BodegaID.Value = transaccion.Header.BodegaOrigenID.Value;
                                    saldosCostosNew.CosteoGrupoInvID.Value = bodegaOrigen.CosteoGrupoInvID.Value;
                                    saldosCostosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                    saldosCostosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                    saldosCostosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                    saldosCostosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                    saldosCostosNew.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                    saldosCostosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                    saldosCostosNew.RegistroCosto.Value = regCostos;
                                    saldosCostosNew.RegistroSaldo.Value = regSaldos;
                                    this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(saldosCostosNew);
                                    #endregion
                                }
                                else
                                {
                                    //Verifica que el periodo corresponda 
                                    if (transaccion.DocCtrl.PeriodoDoc.Value == saldosCostosResult.Periodo.Value)
                                    {
                                        #region Actualiza un Saldo de Exitencias
                                        saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosResult.RegistroSaldo.Value.Value);
                                        saldos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                        #endregion
                                        #region Actualiza un Costo de existencia
                                        costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);
                                        costos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                        costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;//Revisar la operacion para obtener el valor unitario teniendo en cuenta que no siempre valdra lo mismo cada referencia unit
                                        costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                        costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                        costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                        if (movimientoTipo.LibroIFRSInd.Value.Value)
                                        {
                                            costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                        #endregion
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidPeriod;
                                        return result;
                                    }
                                }
                            }
                        }
                        #endregion
                        #region 3. Salidas
                        else if (movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas)
                        {
                            if (saldosCostosResult != null)
                            {
                                mvtoDeta.IdentificadorTr.Value = saldosCostosResult.IdentificadorTr.Value;
                                //Verifica que el periodo corresponda 
                                if (transaccion.DocCtrl.PeriodoDoc.Value == saldosCostosResult.Periodo.Value)
                                {
                                    #region Actualiza un Saldo de Exitencias
                                    saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosResult.RegistroSaldo.Value.Value);

                                    //Verifica que la cantidad de las salidas sean validas
                                    decimal totalSalidaSaldos = Convert.ToDecimal(saldos.CantRetiro.Value.Value + mvtoDeta.CantidadUNI.Value.Value);
                                    if (totalSalidaSaldos <= (saldos.CantInicial.Value.Value + saldos.CantEntrada.Value.Value))
                                    {
                                        saldos.CantRetiro.Value += mvtoDeta.CantidadUNI.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion
                                    #region Actualiza un Costo de existencia
                                    costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);

                                    #region Calcula valores de acuerdo al valor unitario y de unidades de salida
                                    if (costos != null)
                                    {
                                        if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Promedio)
                                        {
                                            decimal cantidadExist = costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value - costos.CantRetiro.Value.Value;
                                            //if (_dtoControl.MonedaID.Value != monedaLocal)
                                            //{
                                            mvtoDeta.Valor1EXT.Value = Math.Round((costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value) /
                                                                        (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value,4);
                                            mvtoDeta.Valor2EXT.Value = Math.Round((costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value) /
                                                                        (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                            //}
                                            mvtoDeta.Valor1LOC.Value = Math.Round((costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value) /
                                                                       (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                            mvtoDeta.Valor2LOC.Value = Math.Round((costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value) /
                                                                       (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                            mvtoDeta.ValorUNI.Value = Math.Round(mvtoDeta.Valor1LOC.Value.Value / mvtoDeta.CantidadUNI.Value.Value, 4);
                                        }
                                        else if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional)
                                        {
                                            costos.IdentificadorTr.Value = mvtoDeta.Consecutivo.Value;
                                            //if (_dtoControl.MonedaID.Value != monedaLocal)
                                            //{
                                            mvtoDeta.Valor1EXT.Value = (costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value);
                                            mvtoDeta.Valor2EXT.Value = (costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value);
                                            //}
                                            mvtoDeta.Valor1LOC.Value = (costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value);
                                            mvtoDeta.Valor2LOC.Value = (costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value);
                                            mvtoDeta.ValorUNI.Value = mvtoDeta.Valor1LOC.Value.Value / mvtoDeta.CantidadUNI.Value.Value;
                                        }
                                        else
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = "Falta definir otros tipos de costeo";
                                            return result;
                                        }

                                    }
                                    #endregion

                                    //Verifica que la cantidad de las salidas sean validas
                                    decimal totalSalidaCostos = Convert.ToDecimal(costos.CantRetiro.Value.Value + mvtoDeta.CantidadUNI.Value.Value);
                                    if (totalSalidaCostos <= (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                    {
                                        costos.CantRetiro.Value += mvtoDeta.CantidadUNI.Value.Value;
                                        if (totalSalidaCostos == (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                        {
                                            costos.CtoLocSalida.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                            costos.CtoExtSalida.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                            costos.FobLocSalida.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            costos.FobExtSalida.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            if (movimientoTipo.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocSalidaIFRS.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                                costos.CtoExtSalidaIFRS.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                                costos.FobLocSalidaIFRS.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                costos.FobExtSalidaIFRS.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            }
                                        }
                                        else
                                        {
                                            costos.CtoLocSalida.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtSalida.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocSalida.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtSalida.Value += mvtoDeta.Valor2EXT.Value;
                                            if (movimientoTipo.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                            }
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_In_InvalidPeriod;
                                    return result;
                                }
                            }
                            else
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = "Error: Salida o Traslado no validado antes de generar movimiento, revisar code";
                                return result;
                            }
                        }
                        #endregion
                        #region 4. Traslados
                        else if (movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                        {
                            if (costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio &&
                                costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio)
                            {
                                if (saldosCostosResult != null)
                                {
                                    mvtoDeta.IdentificadorTr.Value = saldosCostosResult.IdentificadorTr.Value;
                                    //Verifica que el periodo corresponda 
                                    if (transaccion.DocCtrl.PeriodoDoc.Value == saldosCostosResult.Periodo.Value)
                                    {
                                        #region Realiza Salida de Saldo en Bodega Origen
                                        saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosResult.RegistroSaldo.Value.Value);
                                        //Verifica que la cantidad de las salidas sean validas
                                        decimal totalSalidaSaldos = Convert.ToDecimal(saldos.CantRetiro.Value.Value + mvtoDeta.CantidadUNI.Value.Value);
                                        if (totalSalidaSaldos <= (saldos.CantInicial.Value.Value + saldos.CantEntrada.Value.Value))
                                        {
                                            saldos.CantRetiro.Value += mvtoDeta.CantidadUNI.Value.Value;
                                            this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                        }
                                        else
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                            return result;
                                        }
                                        #endregion
                                        #region Realiza Salida de Costo en Bodega Origen
                                        costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);

                                        #region Calcula valores de acuerdo al valor unitario y de unidades de salida
                                        if (costos != null)
                                        {
                                            if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Promedio)
                                            {
                                                decimal cantidadExist = costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value - costos.CantRetiro.Value.Value;
                                                mvtoDeta.Valor1EXT.Value = Math.Round((costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value) /
                                                                            (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value,4);
                                                mvtoDeta.Valor2EXT.Value = Math.Round((costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value) /
                                                                            (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                                mvtoDeta.Valor1LOC.Value = Math.Round((costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value) /
                                                                            (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                                mvtoDeta.Valor2LOC.Value = Math.Round((costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value) /
                                                                            (cantidadExist) * mvtoDeta.CantidadUNI.Value.Value, 4);
                                                mvtoDeta.ValorUNI.Value = Math.Round(mvtoDeta.Valor1LOC.Value.Value / mvtoDeta.CantidadUNI.Value.Value, 4);
                                            }
                                            else if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional)
                                            {
                                                mvtoDeta.Valor1EXT.Value = (costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value);
                                                mvtoDeta.Valor2EXT.Value = (costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value);
                                                mvtoDeta.Valor1LOC.Value = (costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value);
                                                mvtoDeta.Valor2LOC.Value = (costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value);
                                                mvtoDeta.ValorUNI.Value = mvtoDeta.Valor1LOC.Value.Value / mvtoDeta.CantidadUNI.Value.Value;
                                            }
                                            else
                                            {
                                                result.Result = ResultValue.NOK;
                                                result.ResultMessage = "Falta definir otros tipos de costeo";
                                                return result;
                                            }
                                        }
                                        #endregion

                                        //Verifica que la cantidad de las salidas sean validas
                                        decimal totalSalidaCostos = Convert.ToDecimal(costos.CantRetiro.Value.Value + mvtoDeta.CantidadUNI.Value.Value);
                                        if (totalSalidaCostos <= (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                        {
                                            costos.CantRetiro.Value += mvtoDeta.CantidadUNI.Value.Value;
                                            if (totalSalidaCostos == (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                            {
                                                costos.CtoLocSalida.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                                costos.CtoExtSalida.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                                costos.FobLocSalida.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                costos.FobExtSalida.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                {
                                                    costos.CtoLocSalidaIFRS.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                                    costos.CtoExtSalidaIFRS.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                                    costos.FobLocSalidaIFRS.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                    costos.FobExtSalidaIFRS.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                }
                                            }
                                            else
                                            {
                                                costos.CtoLocSalida.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtSalida.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocSalida.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtSalida.Value += mvtoDeta.Valor2EXT.Value;
                                                if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                {
                                                    costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                    costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                    costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                    costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                }
                                            }
                                            this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                        }
                                        else
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                            return result;
                                        }
                                        #endregion

                                        if (documentID != AppDocuments.NotaEnvio)
                                        {
                                            #region Realiza entrada de Saldo en bodega destino si existe o sino crea un nuevo item
                                            saldosCostos.BodegaID.Value = transaccion.Header.BodegaDestinoID.Value;
                                            saldosCostos.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.IdentificadorTr.Value : null;
                                            //saldosCostos.CosteoGrupoInvID.Value = costeoDestino.ID.Value;
                                           DTO_inControlSaldosCostos saldosCostosDestino = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);
                                            if (saldosCostosDestino != null)
                                            {
                                                saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosDestino.RegistroSaldo.Value.Value);
                                                saldos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                            }
                                            else
                                            {
                                                #region Agrega un Saldo de Existencias
                                                DTO_inSaldosExistencias saldosNew = new DTO_inSaldosExistencias();
                                                saldosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                saldosNew.BodegaID.Value = transaccion.Header.BodegaDestinoID.Value;
                                                saldosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                saldosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                                saldosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                                saldosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                                saldosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                saldosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                saldosNew.CantInicial.Value = 0;
                                                saldosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                                saldosNew.CantRetiro.Value = 0;
                                                regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                                                #endregion
                                            }

                                            #endregion
                                            #region Realiza entrada de Costo en bodega destino si existe o sino crea un nuevo item
                                            if (saldosCostosDestino != null && saldosCostosDestino.CosteoGrupoInvID.Value == costeoDestino.ID.Value)
                                            {
                                                costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosDestino.RegistroCosto.Value.Value);
                                                costos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                {
                                                    costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                    costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                    costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                    costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                }
                                                this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                            }
                                            else
                                            {
                                                //Agrega un item si el grupo de costeo es diferente, sino actualiza el costo de origen
                                                if (costeoOrigen.ID.Value != costeoDestino.ID.Value)
                                                {
                                                    #region Agrega un Costo de Existencias
                                                    DTO_inCostosExistencias costosNew = new DTO_inCostosExistencias(true);
                                                    costosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                    costosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                                    costosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                    costosNew.ActivoID.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Individual ? mvtoDeta.ActivoID.Value : 0;
                                                    costosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                    costosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                    costosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                                    costosNew.FobLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                    costosNew.CtoLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                    costosNew.FobExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                    costosNew.CtoExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value; ;
                                                    if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                    {
                                                        costosNew.FobLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                        costosNew.CtoLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                        costosNew.FobExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                        costosNew.CtoExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value;
                                                    }
                                                    #region Si existe el costo lo actualiza  sino lo agrega
                                                    DTO_inCostosExistencias costoExist = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_GetByParameter(costosNew);
                                                    if (costoExist != null)
                                                    {
                                                        costoExist.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                        costoExist.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                        costoExist.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                        costoExist.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                        costoExist.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                        if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                        {
                                                            costoExist.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                            costoExist.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                            costoExist.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                            costoExist.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                        }
                                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costoExist);
                                                        regCostos = costoExist.Consecutivo.Value.Value;
                                                    }
                                                    else
                                                        regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew);
                                                    #endregion
                                                    #endregion
                                                }
                                                else
                                                {
                                                    costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);
                                                    costos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                    costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                    costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                    costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                    costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                    if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                    {
                                                        costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                        costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                        costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                        costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                    }
                                                    this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                                }
                                            }

                                            #endregion
                                            #region Agrega un Control de Saldos y Costos
                                            if (saldosCostosDestino == null)
                                            {
                                                DTO_inControlSaldosCostos saldosCostosNew = new DTO_inControlSaldosCostos();
                                                saldosCostosNew.EmpresaID.Value = transaccion.Header.EmpresaID.Value;
                                                saldosCostosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                                saldosCostosNew.BodegaID.Value = transaccion.Header.BodegaDestinoID.Value;
                                                saldosCostosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                                saldosCostosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                saldosCostosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                saldosCostosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                                saldosCostosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                                saldosCostosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                saldosCostosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                saldosCostosNew.RegistroCosto.Value = regCostos == 0 ? saldosCostosResult.RegistroCosto.Value.Value : regCostos;
                                                saldosCostosNew.RegistroSaldo.Value = regSaldos;
                                                this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(saldosCostosNew);
                                            }
                                            else if (saldosCostosDestino.CosteoGrupoInvID.Value != costeoDestino.ID.Value)//Actualiza el registro del costo si es diferente
                                            {
                                                saldosCostosDestino.CosteoGrupoInvID.Value = costeoDestino.ID.Value;
                                                saldosCostosDestino.RegistroCosto.Value = regCostos;
                                                this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_UpdConsecutivos(saldosCostosDestino);

                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Realiza entrada de Saldo en bodega destino si existe o sino crea un nuevo item
                                            bodegaTransacional = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
                                            DTO_inBodega bodegaTrans = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaTransacional, true, false);
                                            DTO_inCosteoGrupo costeoTrans = bodegaDestino != null ? (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodegaTrans.CosteoGrupoInvID.Value, true, false) : null;
                                            saldosCostos.BodegaID.Value = bodegaTrans.ID.Value;
                                            saldosCostos.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.IdentificadorTr.Value : null;
                                            //saldosCostos.CosteoGrupoInvID.Value = costeoDestino.ID.Value;
                                            DTO_inControlSaldosCostos saldosCostosDestino = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);
                                            if (saldosCostosDestino != null)
                                            {
                                                saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosDestino.RegistroSaldo.Value.Value);
                                                saldos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                            }
                                            else
                                            {
                                                #region Agrega un Saldo de Existencias
                                                DTO_inSaldosExistencias saldosNew = new DTO_inSaldosExistencias();
                                                saldosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                saldosNew.BodegaID.Value = bodegaTrans.ID.Value;
                                                saldosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                saldosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                                saldosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                                saldosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                                saldosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                saldosNew.IdentificadorTr.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                saldosNew.CantInicial.Value = 0;
                                                saldosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                                saldosNew.CantRetiro.Value = 0;
                                                regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                                                #endregion
                                            }

                                            #endregion
                                            #region Realiza entrada de Costo en bodega destino si existe o sino crea un nuevo item
                                            if (saldosCostosDestino != null && saldosCostosDestino.CosteoGrupoInvID.Value == costeoDestino.ID.Value)
                                            {
                                                costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosDestino.RegistroCosto.Value.Value);
                                                costos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                {
                                                    costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                    costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                    costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                    costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                }
                                                this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                            }
                                            else
                                            {
                                                //Agrega un item si el grupo de costeo es diferente, sino actualiza el costo de origen
                                                if (costeoOrigen.ID.Value != costeoTrans.ID.Value)
                                                {
                                                    #region Agrega un Costo de Existencias
                                                    DTO_inCostosExistencias costosNew = new DTO_inCostosExistencias(true);
                                                    costosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                    costosNew.CosteoGrupoInvID.Value = bodegaTrans.CosteoGrupoInvID.Value;
                                                    costosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                    costosNew.ActivoID.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.Individual ? mvtoDeta.ActivoID.Value : 0;
                                                    costosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                    costosNew.IdentificadorTr.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                    costosNew.CantEntrada.Value = mvtoDeta.CantidadUNI.Value;
                                                    costosNew.FobLocEntrada.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                    costosNew.CtoLocEntrada.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                    costosNew.FobExtEntrada.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                    costosNew.CtoExtEntrada.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value; ;
                                                    if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                    {
                                                        costosNew.FobLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                        costosNew.CtoLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                        costosNew.FobExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                        costosNew.CtoExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value;
                                                    }
                                                    #region Si existe el costo lo actualiza  sino lo agrega
                                                    DTO_inCostosExistencias costoExist = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_GetByParameter(costosNew);
                                                    if (costoExist != null)
                                                    {
                                                        costoExist.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                        costoExist.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                        costoExist.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                        costoExist.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                        costoExist.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                        if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                        {
                                                            costoExist.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                            costoExist.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                            costoExist.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                            costoExist.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                        }
                                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costoExist);
                                                        regCostos = costoExist.Consecutivo.Value.Value;
                                                    }
                                                    else
                                                        regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew);
                                                    #endregion
                                                    #endregion
                                                }
                                                else
                                                {
                                                    costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);
                                                    costos.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                    costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                    costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                    costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                    costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                    if (movimientoTipo.LibroIFRSInd.Value.Value)
                                                    {
                                                        costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                        costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                        costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                        costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                    }
                                                    this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                                }
                                            }

                                            #endregion
                                            #region Agrega un Control de Saldos y Costos
                                            if (saldosCostosDestino == null)
                                            {
                                                DTO_inControlSaldosCostos saldosCostosNew = new DTO_inControlSaldosCostos();
                                                saldosCostosNew.EmpresaID.Value = transaccion.Header.EmpresaID.Value;
                                                saldosCostosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                                saldosCostosNew.BodegaID.Value = bodegaTrans.ID.Value;
                                                saldosCostosNew.CosteoGrupoInvID.Value = bodegaTrans.CosteoGrupoInvID.Value;
                                                saldosCostosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                                saldosCostosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                                saldosCostosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                                saldosCostosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                                saldosCostosNew.IdentificadorTr.Value = costeoTrans.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                                saldosCostosNew.Periodo.Value = transaccion.DocCtrl.PeriodoDoc.Value;
                                                saldosCostosNew.RegistroCosto.Value = regCostos == 0 ? saldosCostosResult.RegistroCosto.Value.Value : regCostos;
                                                saldosCostosNew.RegistroSaldo.Value = regSaldos;
                                                this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(saldosCostosNew);
                                            }
                                            else if (saldosCostosDestino.CosteoGrupoInvID.Value != costeoDestino.ID.Value)//Actualiza el registro del costo si es diferente
                                            {
                                                saldosCostosDestino.CosteoGrupoInvID.Value = costeoDestino.ID.Value;
                                                saldosCostosDestino.RegistroCosto.Value = regCostos;
                                                this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_UpdConsecutivos(saldosCostosDestino);

                                            }
                                            #endregion
                                        }
                                        #region Si es serializada Modifica la bodega del serial en ActivoControl
                                        if (mvtoDeta.ActivoID.Value != 0)
                                        {
                                            DTO_acActivoControl activo = this._dal_acActivoControl.DAL_acActivoControl_GetByID(mvtoDeta.ActivoID.Value.Value);
                                            activo.BodegaID.Value = string.IsNullOrEmpty(bodegaTransacional) ? bodegaDestino.ID.Value : bodegaTransacional;
                                            this._dal_acActivoControl.DAL_acActivoControl_Update(activo, activo.ActivoID.Value.Value);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidPeriod;
                                        return result;
                                    }
                                }
                                else
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = "Error: Salida o Traslado no validado antes de generar movimiento, revisar code";
                                    return result;
                                }
                            }
                            else if (costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio && costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.Transaccional)
                            {

                            }
                            listMvtoDeta.Add(mvtoDeta);
                        }
                        #endregion
                        mvtoDeta.ProyectoID.Value = bodegaOrigen.ProyectoID.Value;
                        this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Update(mvtoDeta);
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal; 
                    #endregion
                    #region 5. Borra de glMovimientoDetaPRE / Agrega a glMovimientoDeta
                    DTO_glMovimientoDeta traslado = null;
                    List<DTO_glMovimientoDeta> listTraslado = new List<DTO_glMovimientoDeta>();

                    this._moduloGlobal.glMovimientoDetaPRE_Delete(numeroDoc, true);
                    if (movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                    {                       
                        foreach (var deta in listMvtoDeta)
                        {
                            traslado = new DTO_glMovimientoDeta();
                            traslado.ActivoID.Value = deta.ActivoID.Value;
                            traslado.BodegaID.Value = documentID == AppDocuments.NotaEnvio && movimientoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados ? bodegaTransacional : bodegaDestino.ID.Value;
                            traslado.CantidadEMP.Value = deta.CantidadEMP.Value;
                            traslado.CantidadUNI.Value = deta.CantidadUNI.Value;
                            traslado.CentroCostoID.Value = deta.CentroCostoID.Value;
                            traslado.Consecutivo.Value = deta.Consecutivo.Value;
                            traslado.DescripTExt.Value = deta.DescripTExt.Value;
                            traslado.DocSoporte.Value = deta.DocSoporte.Value;
                            traslado.DocSoporteTER.Value = deta.DocSoporteTER.Value;
                            traslado.EmpaqueInvID.Value = deta.EmpaqueInvID.Value;
                            traslado.EmpresaID.Value = deta.EmpresaID.Value;
                            traslado.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                            traslado.EstadoInv.Value = deta.EstadoInv.Value;
                            traslado.Fecha.Value = deta.Fecha.Value;
                            traslado.Index = listMvtoDeta.Count + listTraslado.Count;                          
                            traslado.inReferenciaID.Value = deta.inReferenciaID.Value;
                            traslado.Kit.Value = deta.Kit.Value;
                            traslado.CodigoBSID.Value = deta.CodigoBSID.Value;
                            traslado.MvtoTipoInvID.Value = deta.MvtoTipoInvID.Value;
                            traslado.NumeroDoc.Value = deta.NumeroDoc.Value;
                            traslado.Parametro1.Value = deta.Parametro1.Value;
                            traslado.Parametro2.Value = deta.Parametro2.Value;
                            traslado.ProyectoID.Value = bodegaDestino.ProyectoID.Value;// deta.ProyectoID.Value;
                            traslado.TerceroID.Value = deta.TerceroID.Value;
                            traslado.LineaPresupuestoID.Value = deta.LineaPresupuestoID.Value;
                            traslado.DatoAdd1.Value = deta.DatoAdd1.Value;
                            traslado.DatoAdd2.Value = deta.DatoAdd2.Value;
                            traslado.DatoAdd3.Value = deta.DatoAdd3.Value;
                            traslado.DatoAdd4.Value = deta.DatoAdd4.Value;
                            traslado.DatoAdd5.Value = deta.DatoAdd5.Value;
                            traslado.SerialID.Value = deta.SerialID.Value;
                            traslado.Valor1LOC.Value = deta.Valor1LOC.Value;
                            traslado.Valor1EXT.Value = deta.Valor1EXT.Value;
                            traslado.Valor2LOC.Value = deta.Valor2LOC.Value;
                            traslado.Valor2EXT.Value = deta.Valor2EXT.Value;
                            traslado.ValorUNI.Value = deta.ValorUNI.Value;
                            listTraslado.Add(traslado);
                        }
                        if (listTraslado.Any(x => string.IsNullOrEmpty(x.BodegaID.Value)))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "La bodega en el detalle esta vacía, revisar";
                            return result;
                        } 
                        result = this._moduloGlobal.glMovimientoDeta_Add(listTraslado, true, true);
                    }
                   
                    if (result.Result == ResultValue.NOK)
                        return result;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region 6. Actualiza el documento control
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, _dtoControl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion                    
                }
                else
                {
                    #region Carga las variables
                    this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)this.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    List<DTO_glMovimientoDeta> footer = null;
                    if(documentID == AppDocuments.DistribucionCostos)
                        footer = this._moduloGlobal.glMovimientoDeta_GetByNumeroDoc(numeroDoc);
                    else
                        footer = this._moduloGlobal.glMovimientoDetaPRE_GetNumeroDoc(numeroDoc);
                    DTO_inMovimientoDocu movDocu = this._dal_inMovimientoDocu.DAL_inMovimientoDocu_Get(numeroDoc);
                    DTO_inMovimientoTipo movimientoTipo = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, movDocu.MvtoTipoInvID.Value, true, false);
                    _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);

                    #endregion
                    foreach (DTO_glMovimientoDeta mvtoDeta in footer)
                    {
                        #region 1. Revisa si existe el movimiento en saldos y costos
                        DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                        saldosCostos.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        saldosCostos.Periodo.Value = periodoInv;
                        saldosCostos.BodegaID.Value = mvtoDeta.BodegaID.Value;
                        saldosCostos.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        saldosCostos.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        saldosCostos.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        saldosCostos.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        saldosCostos.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        //saldosCostos.CosteoGrupoInvID.Value = costeoDestino.ID.Value;
                        DTO_inControlSaldosCostos saldosCostosResult = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);

                        #endregion
                        #region 2. Actualiza el costo de los items
                        if (saldosCostosResult != null)
                        {
                            mvtoDeta.IdentificadorTr.Value = saldosCostosResult.IdentificadorTr.Value;
                             
                            #region Actualiza un Costo de existencia
                            costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);

                            if (costos != null)
                            {
                                costos.CtoLocEntrada.Value += documentID != AppDocuments.DeterioroInv ? mvtoDeta.Valor1LOC.Value : (mvtoDeta.Valor1LOC.Value*-1);
                                costos.CtoExtEntrada.Value += documentID != AppDocuments.DeterioroInv ? mvtoDeta.Valor1EXT.Value : (mvtoDeta.Valor1EXT.Value*-1);
                                if (movimientoTipo.LibroIFRSInd.Value.Value)
                                {
                                    costos.CtoLocEntradaIFRS.Value += documentID != AppDocuments.DeterioroInv ? mvtoDeta.Valor1LOC.Value : (mvtoDeta.Valor1LOC.Value * -1);
                                    costos.CtoExtEntradaIFRS.Value += documentID != AppDocuments.DeterioroInv ? mvtoDeta.Valor1EXT.Value : (mvtoDeta.Valor1EXT.Value * -1);
                                }
                                this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                            }
                            #endregion                             
                            else
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                return result;
                            }
                        }
                        else
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "Error: Salida o Traslado no validado antes de generar movimiento, revisar code";
                            return result;
                        }
                        #endregion                      
                    }
                    this._moduloGlobal.glMovimientoDetaPRE_Delete(numeroDoc, true);
                }
                batchProgress[tupProgress] = 100;

                //result = this.AsignarFlujo(documentID, numeroDoc, actFlujoID, false, string.Empty);
                //if (result.Result == ResultValue.NOK)
                //    return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Transaccion_Aprobar");

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
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (_dtoControl.DocumentoNro.Value == 0)
                            _dtoControl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, _dtoControl.PrefijoID.Value);                         
                        this._moduloGlobal.ActualizaConsecutivos(_dtoControl, true, modulePlaneacionActive, false);
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
            return result;
        }

        /// <summary>
        ///Obtiene la relacion de saldos y costos para las salidas por referencia
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="saldo">dto para filtrar</param>
        /// <param name="costoTotal">costoTotal</param>
        public decimal Transaccion_SaldoExistByReferencia(int documentID, DTO_inControlSaldosCostos saldo, ref DTO_inCostosExistencias costoTotal)
        {
            decimal cantidadDisponible = 0;
            try
            {
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_inControlSaldosCostos> listSaldoExist = this.inControlSaldosCostos_GetByParameter(documentID, saldo);
                List<int> distCosto = (from c in listSaldoExist select c.RegistroCosto.Value.Value).Distinct().ToList();

                //Trae los costos
                foreach (int d in distCosto)
                {
                    DTO_inControlSaldosCostos itemCostoSaldo = listSaldoExist.Find(x => x.RegistroCosto.Value == d);
                    DTO_inSaldosExistencias saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(itemCostoSaldo.RegistroSaldo.Value.Value);
                    DTO_inCostosExistencias costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(itemCostoSaldo.RegistroCosto.Value.Value);
                    cantidadDisponible += (saldos.CantInicial.Value.Value + saldos.CantEntrada.Value.Value - saldos.CantRetiro.Value.Value);
                    costoTotal.CantInicial.Value += costos.CantInicial.Value;
                    costoTotal.CantEntrada.Value += costos.CantEntrada.Value;
                    costoTotal.CantRetiro.Value += costos.CantRetiro.Value;
                    costoTotal.CtoLocSaldoIni.Value += costos.CtoLocSaldoIni.Value;
                    costoTotal.CtoLocEntrada.Value += costos.CtoLocEntrada.Value;
                    costoTotal.CtoLocSalida.Value += costos.CtoLocSalida.Value;
                    costoTotal.CtoExtSaldoIni.Value += costos.CtoExtSaldoIni.Value;
                    costoTotal.CtoExtEntrada.Value += costos.CtoExtEntrada.Value;
                    costoTotal.CtoExtSalida.Value += costos.CtoExtSalida.Value;
                    costoTotal.FobLocSaldoIni.Value += costos.FobLocSaldoIni.Value;
                    costoTotal.FobLocEntrada.Value += costos.FobLocEntrada.Value;
                    costoTotal.FobLocSalida.Value += costos.FobLocSalida.Value;
                    costoTotal.FobExtSaldoIni.Value += costos.FobExtSaldoIni.Value;
                    costoTotal.FobExtEntrada.Value += costos.FobExtEntrada.Value;
                    costoTotal.FobExtSalida.Value += costos.FobExtSalida.Value;
                    costoTotal.IdentificadorTr.Value = costos.IdentificadorTr.Value; 
                }
                List<int> distSaldos = (from c in listSaldoExist select c.RegistroSaldo.Value.Value).Distinct().ToList();
                return cantidadDisponible;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Transaccion_SaldoExistByReferencia");
                return cantidadDisponible;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="ctrlSaldoCosto"></param>
        /// <returns>Dto de Control de saldos y costos</returns>
        public List<DTO_inControlSaldosCostos> inControlSaldosCostos_GetByParameter(int documentID, DTO_inControlSaldosCostos ctrlSaldoCosto)
        {
            this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloActivos = (ModuloActivosFijos)this.GetInstance(typeof(ModuloActivosFijos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string param1xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
            string param2xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
            DateTime periodoInv = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));

            if (!ctrlSaldoCosto.Periodo.Value.HasValue)
                ctrlSaldoCosto.Periodo.Value = periodoInv;
            List<DTO_inControlSaldosCostos> list = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_GetByParameter(ctrlSaldoCosto);    
            DTO_inCostosExistencias costos = new DTO_inCostosExistencias();

            foreach (DTO_inControlSaldosCostos saldoCosto in list)
            {
                DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, saldoCosto.inReferenciaID.Value, true, false);
                if (referencia != null)
                {
                    string referenciaParametrosID = referencia.ID.Value;
                    string referenciaParametrosDesc = referencia.Descriptivo.Value;
                    //DTO_inSaldosExistencias saldo = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldoCosto.RegistroSaldo.Value.Value);
                    //saldoCosto.CantidadDisp.Value = saldo != null ? (saldo.CantInicial.Value + saldo.CantEntrada.Value - saldo.CantRetiro.Value) : 0;
                    if (saldoCosto.EstadoInv.Value != (byte)EstadoInv.Activo)
                        referenciaParametrosID += "-U";
                    //Trae Info del Activo
                    if (saldoCosto.ActivoID.Value != 0)
                    {
                        DTO_acActivoControl activo = this._moduloActivos.acActivoControl_GetByID(saldoCosto.ActivoID.Value.Value);
                        saldoCosto.SerialID.Value = activo != null ? activo.SerialID.Value : string.Empty;
                        saldoCosto.Seleccion.Value = false;
                    }
                    //Trae info del parametro 1
                    if (saldoCosto.Parametro1.Value != param1xDef.ToUpper())
                    {
                        DTO_MasterBasic param1 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro1, saldoCosto.Parametro1.Value, true, false);
                        referenciaParametrosID += "-" + param1.ID.Value;
                        referenciaParametrosDesc += "-" + param1.Descriptivo.Value;
                    }
                    //Trae info del parametro 2
                    if (saldoCosto.Parametro2.Value != param2xDef.ToUpper())
                    {
                        DTO_MasterBasic param2 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro2, saldoCosto.Parametro2.Value, true, false);
                        referenciaParametrosID += "-" + param2.ID.Value;
                        referenciaParametrosDesc += "-" + param2.Descriptivo.Value;
                    }
                    //Trae Unidad del Recurso o la referencia
                    DTO_pyRecurso recurso = (DTO_pyRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, saldoCosto.inReferenciaID.Value, true, false);
                    if (recurso != null)
                        saldoCosto.UnidadInvID.Value = recurso.UnidadInvID.Value;
                    else
                        saldoCosto.UnidadInvID.Value = referencia.UnidadInvID.Value;

                    saldoCosto.ReferenciaP1P2ID.Value = referenciaParametrosID;
                    saldoCosto.ReferenciaP1P2Desc.Value = referenciaParametrosDesc; 
                }
            }          
            return list;
        }

        /// <summary>
        /// Retorna una lista de notas envio 
        /// </summary>        
        /// <returns>Retorna una lista de notas envio</returns>
        public List<DTO_NotasEnvioResumen> NotasEnvio_GetResumen()
        {
            this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_inMovimientoDocu.DAL_NotasEnvio_GetResumen();
        }

        /// <summary>
        /// Recibe o devuelve entradas de una Nota de Envio
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="notaEnvio">resumen de mvtos</param>
        /// <param name="actFlujoID">Actividad actual</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_SerializedObject NotaEnvio_RecibirDevolver(int documentID, DTO_MvtoInventarios notaEnvio, string actFlujoID, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl _dtoControl = null;
            DTO_inSaldosExistencias saldos = null;
            DTO_inCostosExistencias costos = null;
            DTO_Alarma alarma = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Carga las variables
                DTO_MvtoInventarios transaccionNotaEnvio = notaEnvio;
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string bodegaTransacional = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
                DateTime periodoInv = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
                _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(transaccionNotaEnvio.DocCtrl.NumeroDoc.Value.Value);
                DTO_inBodega bodegaOrigen = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaTransacional, true, false);
                DTO_inCosteoGrupo costeoOrigen = (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodegaOrigen.CosteoGrupoInvID.Value, true, false);
                DTO_inBodega bodegaDestino = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, transaccionNotaEnvio.Header.BodegaDestinoID.Value, true, false);
                DTO_inCosteoGrupo costeoDestino = bodegaDestino != null ? (DTO_inCosteoGrupo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodegaDestino.CosteoGrupoInvID.Value, true, false) : null;
                DTO_inMovimientoTipo dtoMvto = (DTO_inMovimientoTipo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, transaccionNotaEnvio.Header.MvtoTipoInvID.Value, true, false);
                List<DTO_glMovimientoDeta> listMvtoDeta = new List<DTO_glMovimientoDeta>();
                int regSaldos = 0;
                int regCostos = 0;
                #endregion

                foreach (DTO_inMovimientoFooter itemMvto in transaccionNotaEnvio.Footer)
                {
                    if (itemMvto.RecibirInd.Value.Value)
                    {
                        DTO_glMovimientoDeta mvtoDeta = (DTO_glMovimientoDeta)itemMvto.Movimiento;
                        mvtoDeta.BodegaID.Value = bodegaTransacional;

                        #region Revisa si existe el movimiento en saldos y costos
                        DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                        saldosCostos.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        saldosCostos.Periodo.Value = periodoInv;
                        saldosCostos.BodegaID.Value = mvtoDeta.BodegaID.Value;
                        saldosCostos.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        saldosCostos.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        saldosCostos.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        saldosCostos.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        saldosCostos.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        saldosCostos.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                        DTO_inControlSaldosCostos saldosCostosResult = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);

                        #endregion
                        #region Traslados
                        if (costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio &&
                            costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio)
                        {
                            if (saldosCostosResult != null)
                            {
                                //Verifica que el periodo corresponda 
                                if (transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value == saldosCostosResult.Periodo.Value)
                                {
                                    #region Realiza Salida de Saldo en Bodega Origen
                                    saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosResult.RegistroSaldo.Value.Value);
                                    //Verifica que la cantidad de las salidas sean validas
                                    int totalSalidaSaldos = Convert.ToInt32(saldos.CantRetiro.Value.Value + itemMvto.CantidadTraslado.Value.Value);
                                    if (totalSalidaSaldos <= (saldos.CantInicial.Value.Value + saldos.CantEntrada.Value.Value))
                                    {
                                        saldos.CantRetiro.Value += itemMvto.CantidadTraslado.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion
                                    #region Realiza Salida de Costo en Bodega Origen
                                    costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);

                                    #region Calcula valores de acuerdo al valor unitario y de unidades de salida
                                    if (costos != null)
                                    {
                                        if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Promedio || costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional)
                                        {
                                            decimal cantidadExist = costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value - costos.CantRetiro.Value.Value;
                                            //if (_dtoControl.MonedaID.Value != monedaLocal)
                                            //{
                                            mvtoDeta.Valor1EXT.Value = (costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.Valor2EXT.Value = (costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value) /
                                                                            (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            //}
                                            mvtoDeta.Valor1LOC.Value = (costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.Valor2LOC.Value = (costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.ValorUNI.Value = mvtoDeta.Valor1LOC.Value.Value / itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.CantidadUNI.Value = itemMvto.CantidadTraslado.Value;
                                        }
                                        else
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = "Falta definir otros tipos de costeo";
                                            return result;
                                        }
                                    }
                                    #endregion

                                    //Verifica que la cantidad de las salidas sean validas
                                    int totalSalidaCostos = Convert.ToInt32(costos.CantRetiro.Value.Value + itemMvto.CantidadTraslado.Value.Value);
                                    if (totalSalidaCostos <= (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                    {
                                        costos.CantRetiro.Value += itemMvto.CantidadTraslado.Value.Value;
                                        if (totalSalidaCostos == (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                        {
                                            costos.CtoLocSalida.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                            costos.CtoExtSalida.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                            costos.FobLocSalida.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            costos.FobExtSalida.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocSalidaIFRS.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                                costos.CtoExtSalidaIFRS.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                                costos.FobLocSalidaIFRS.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                costos.FobExtSalidaIFRS.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            }
                                        }
                                        else
                                        {
                                            costos.CtoLocSalida.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtSalida.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocSalida.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtSalida.Value += mvtoDeta.Valor2EXT.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                            }
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion

                                    #region Realiza entrada de Saldo en bodega destino si existe o sino crea un nuevo item
                                    saldosCostos.BodegaID.Value = transaccionNotaEnvio.Header.BodegaDestinoID.Value;
                                    saldosCostos.IdentificadorTr.Value = 0;
                                    DTO_inControlSaldosCostos saldosCostosDestino = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);
                                    if (saldosCostosDestino != null)
                                    {
                                        saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosDestino.RegistroSaldo.Value.Value);
                                        saldos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                    }
                                    else
                                    {
                                        #region Agrega un Saldo de Existencias
                                        DTO_inSaldosExistencias saldosNew = new DTO_inSaldosExistencias();
                                        saldosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                        saldosNew.BodegaID.Value = transaccionNotaEnvio.Header.BodegaDestinoID.Value;
                                        saldosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                        saldosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                        saldosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                        saldosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                        saldosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                        saldosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0; 
                                        saldosNew.CantInicial.Value = 0;
                                        saldosNew.CantEntrada.Value = itemMvto.CantidadTraslado.Value;
                                        saldosNew.CantRetiro.Value = 0;
                                        regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                                        #endregion
                                    }

                                    #endregion
                                    #region Realiza entrada de Costo en bodega destino si existe o sino crea un nuevo item
                                    if (saldosCostosDestino != null)
                                    {
                                        costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosDestino.RegistroCosto.Value.Value);
                                        costos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                        costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                        costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                        costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                        costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                        if (dtoMvto.LibroIFRSInd.Value.Value)
                                        {
                                            costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                    }
                                    else
                                    {
                                        //Agrega un item si el grupo de costeo es diferente, sino actualiza el costo de origen
                                        if (costeoOrigen.ID.Value != costeoDestino.ID.Value)
                                        {
                                            #region Agrega un Costo de Existencias
                                            DTO_inCostosExistencias costosNew = new DTO_inCostosExistencias(true);
                                            costosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                            costosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                            costosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                            costosNew.ActivoID.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Individual ? mvtoDeta.ActivoID.Value : 0;
                                            costosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                            costosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                            costosNew.CantEntrada.Value = itemMvto.CantidadTraslado.Value;
                                            costosNew.FobLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                            costosNew.CtoLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                            costosNew.FobExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                            costosNew.CtoExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value; ;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costosNew.FobLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                costosNew.CtoLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                costosNew.FobExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                costosNew.CtoExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value;
                                            }
                                            #region Si existe el costo lo actualiza  sino lo agrega
                                            DTO_inCostosExistencias costoExist = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_GetByParameter(costosNew);
                                            if (costoExist != null)
                                            {
                                                costoExist.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                costoExist.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                costoExist.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                costoExist.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                costoExist.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                if (dtoMvto.LibroIFRSInd.Value.Value)
                                                {
                                                    costoExist.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                    costoExist.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                    costoExist.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                    costoExist.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                }
                                                this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costoExist);
                                                regCostos = costoExist.Consecutivo.Value.Value;
                                            }
                                            else
                                                regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew);
                                            #endregion
                                            #endregion
                                        }
                                        else
                                        {
                                            costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);
                                            costos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                            costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                            }
                                            this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                        }
                                    }

                                    #endregion
                                    #region Agrega un Control de Saldos y Costos
                                    if (saldosCostosDestino == null)
                                    {
                                        DTO_inControlSaldosCostos saldosCostosNew = new DTO_inControlSaldosCostos();
                                        saldosCostosNew.EmpresaID.Value = transaccionNotaEnvio.Header.EmpresaID.Value;
                                        saldosCostosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                        saldosCostosNew.BodegaID.Value = transaccionNotaEnvio.Header.BodegaDestinoID.Value;
                                        saldosCostosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                        saldosCostosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                        saldosCostosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                        saldosCostosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                        saldosCostosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                        saldosCostosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                        saldosCostosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                        saldosCostosNew.RegistroCosto.Value = regCostos == 0 ? saldosCostosResult.RegistroCosto.Value.Value : regCostos;
                                        saldosCostosNew.RegistroSaldo.Value = regSaldos;
                                        this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(saldosCostosNew);
                                    }
                                    #endregion  
                                }
                                else
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_In_InvalidPeriod;
                                    return result;
                                }
                            }
                            else
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = "Error: Salida o Traslado no validado antes de generar movimiento, revisar code";
                                return result;
                            }
                        }
                        listMvtoDeta.Add(mvtoDeta);
                        #endregion                       
                        #region Agrega un mvto de entrada a bodega destino
                        DTO_glMovimientoDeta traslado = new DTO_glMovimientoDeta();
                        traslado.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        traslado.BodegaID.Value = documentID != AppDocuments.NotaEnvio ? bodegaDestino.ID.Value : bodegaTransacional;
                        traslado.CantidadEMP.Value = mvtoDeta.CantidadEMP.Value;
                        traslado.CantidadUNI.Value = itemMvto.CantidadTraslado.Value;
                        traslado.CentroCostoID.Value = mvtoDeta.CentroCostoID.Value;
                        traslado.Consecutivo.Value = mvtoDeta.Consecutivo.Value;
                        traslado.DescripTExt.Value = mvtoDeta.DescripTExt.Value;
                        traslado.DocSoporte.Value = mvtoDeta.DocSoporte.Value;
                        traslado.EmpaqueInvID.Value = mvtoDeta.EmpaqueInvID.Value;
                        traslado.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        traslado.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                        traslado.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        traslado.Fecha.Value = mvtoDeta.Fecha.Value;
                        traslado.Index = listMvtoDeta.Count - 1;
                        traslado.IdentificadorTr.Value = mvtoDeta.IdentificadorTr.Value;
                        traslado.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        traslado.Kit.Value = mvtoDeta.Kit.Value;
                        traslado.MvtoTipoInvID.Value = mvtoDeta.MvtoTipoInvID.Value;
                        traslado.NumeroDoc.Value = mvtoDeta.NumeroDoc.Value;
                        traslado.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        traslado.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        traslado.ProyectoID.Value = mvtoDeta.ProyectoID.Value;
                        traslado.TerceroID.Value = mvtoDeta.TerceroID.Value;
                        traslado.SerialID.Value = mvtoDeta.SerialID.Value;
                        traslado.Valor1LOC.Value = mvtoDeta.Valor1LOC.Value;
                        traslado.Valor1EXT.Value = mvtoDeta.Valor1EXT.Value;
                        traslado.Valor2LOC.Value = mvtoDeta.Valor2LOC.Value;
                        traslado.Valor2EXT.Value = mvtoDeta.Valor2EXT.Value;
                        traslado.ValorUNI.Value = mvtoDeta.ValorUNI.Value;
                        listMvtoDeta.Add(traslado);
                        #endregion
                        this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actFlujoID, false, _dtoControl.Observacion.Value);
                    }
                    else if (itemMvto.DevolverInd.Value.Value)
                    {
                        DTO_glMovimientoDeta mvtoDeta = (DTO_glMovimientoDeta)itemMvto.Movimiento;
                        mvtoDeta.BodegaID.Value = bodegaTransacional;

                        #region Revisa si existe el movimiento en saldos y costos
                        DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                        saldosCostos.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        saldosCostos.Periodo.Value = periodoInv;
                        saldosCostos.BodegaID.Value = mvtoDeta.BodegaID.Value;
                        saldosCostos.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        saldosCostos.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        saldosCostos.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        saldosCostos.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        saldosCostos.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        saldosCostos.IdentificadorTr.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                        DTO_inControlSaldosCostos saldosCostosResult = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);

                        #endregion
                        #region Traslados
                        if (costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoOrigen.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio &&
                            costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.SinCosto || costeoDestino.CosteoTipo.Value != (byte)TipoCosteoInv.Promedio)
                        {
                            if (saldosCostosResult != null)
                            {
                                //Verifica que el periodo corresponda 
                                if (transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value == saldosCostosResult.Periodo.Value)
                                {
                                    #region Realiza Salida de Saldo en Bodega Origen
                                    saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosResult.RegistroSaldo.Value.Value);
                                    //Verifica que la cantidad de las salidas sean validas
                                    int totalSalidaSaldos = Convert.ToInt32(saldos.CantRetiro.Value.Value + itemMvto.CantidadTraslado.Value.Value);
                                    if (totalSalidaSaldos <= (saldos.CantInicial.Value.Value + saldos.CantEntrada.Value.Value))
                                    {
                                        saldos.CantRetiro.Value += itemMvto.CantidadTraslado.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion
                                    #region Realiza Salida de Costo en Bodega Origen
                                    costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);

                                    #region Calcula valores de acuerdo al valor unitario y de unidades de salida
                                    if (costos != null)
                                    {
                                        if (costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Promedio || costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional)
                                        {
                                            decimal cantidadExist = costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value - costos.CantRetiro.Value.Value;
                                            //if (_dtoControl.MonedaID.Value != monedaLocal)
                                            //{
                                            mvtoDeta.Valor1EXT.Value = (costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value - costos.CtoExtSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.Valor2EXT.Value = (costos.FobExtSaldoIni.Value.Value + costos.FobExtEntrada.Value.Value - costos.FobExtSalida.Value.Value) /
                                                                            (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            //}
                                            mvtoDeta.Valor1LOC.Value = (costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value - costos.CtoLocSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.Valor2LOC.Value = (costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value - costos.FobLocSalida.Value.Value) /
                                                                        (cantidadExist) * itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.ValorUNI.Value = mvtoDeta.Valor1LOC.Value.Value / itemMvto.CantidadTraslado.Value.Value;
                                            mvtoDeta.CantidadUNI.Value = itemMvto.CantidadTraslado.Value;
                                        }
                                        else
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = "Falta definir otros tipos de costeo";
                                            return result;
                                        }
                                    }
                                    #endregion

                                    //Verifica que la cantidad de las salidas sean validas
                                    int totalSalidaCostos = Convert.ToInt32(costos.CantRetiro.Value.Value + itemMvto.CantidadTraslado.Value.Value);
                                    if (totalSalidaCostos <= (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                    {
                                        costos.CantRetiro.Value += itemMvto.CantidadTraslado.Value.Value;
                                        if (totalSalidaCostos == (costos.CantInicial.Value.Value + costos.CantEntrada.Value.Value))
                                        {
                                            costos.CtoLocSalida.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                            costos.CtoExtSalida.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                            costos.FobLocSalida.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            costos.FobExtSalida.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocSalidaIFRS.Value = costos.CtoLocSaldoIni.Value.Value + costos.CtoLocEntrada.Value.Value;
                                                costos.CtoExtSalidaIFRS.Value = costos.CtoExtSaldoIni.Value.Value + costos.CtoExtEntrada.Value.Value;
                                                costos.FobLocSalidaIFRS.Value = costos.FobLocSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                                costos.FobExtSalidaIFRS.Value = costos.FobExtSaldoIni.Value.Value + costos.FobLocEntrada.Value.Value;
                                            }
                                        }
                                        else
                                        {
                                            costos.CtoLocSalida.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtSalida.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocSalida.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtSalida.Value += mvtoDeta.Valor2EXT.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                            }
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                    }
                                    else
                                    {
                                        result.Result = ResultValue.NOK;
                                        result.ResultMessage = DictionaryMessages.Err_In_InvalidOutExistSaldo;
                                        return result;
                                    }
                                    #endregion

                                    #region Realiza entrada de Saldo en bodega destino si existe o sino crea un nuevo item
                                    saldosCostos.BodegaID.Value = transaccionNotaEnvio.Header.BodegaOrigenID.Value;
                                    DTO_inControlSaldosCostos saldosCostosDestino = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Get(saldosCostos);
                                    if (saldosCostosDestino != null)
                                    {
                                        saldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(saldosCostosDestino.RegistroSaldo.Value.Value);
                                        saldos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                        this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Upd(saldos);
                                    }
                                    else
                                    {
                                        #region Agrega un Saldo de Existencias
                                        DTO_inSaldosExistencias saldosNew = new DTO_inSaldosExistencias();
                                        saldosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                        saldosNew.BodegaID.Value = transaccionNotaEnvio.Header.BodegaOrigenID.Value;
                                        saldosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                        saldosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                        saldosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                        saldosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                        saldosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                        saldosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0; 
                                        saldosNew.CantInicial.Value = 0;
                                        saldosNew.CantEntrada.Value = itemMvto.CantidadTraslado.Value;
                                        saldosNew.CantRetiro.Value = 0;
                                        regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                                        #endregion
                                    }

                                    #endregion
                                    #region Realiza entrada de Costo en bodega destino si existe o sino crea un nuevo item
                                    if (saldosCostosDestino != null)
                                    {
                                        costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosDestino.RegistroCosto.Value.Value);
                                        costos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                        costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                        costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                        costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                        costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                        if (dtoMvto.LibroIFRSInd.Value.Value)
                                        {
                                            costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                        }
                                        this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                    }
                                    else
                                    {
                                        //Agrega un item si el grupo de costeo es diferente, sino actualiza el costo de origen
                                        if (costeoOrigen.ID.Value != costeoDestino.ID.Value)
                                        {
                                            #region Agrega un Costo de Existencias
                                            DTO_inCostosExistencias costosNew = new DTO_inCostosExistencias(true);
                                            costosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                            costosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                            costosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                            costosNew.ActivoID.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Individual ? mvtoDeta.ActivoID.Value : 0;
                                            costosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                            costosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                            costosNew.CantEntrada.Value = itemMvto.CantidadTraslado.Value;
                                            costosNew.FobLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                            costosNew.CtoLocEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                            costosNew.FobExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                            costosNew.CtoExtEntrada.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costosNew.FobLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2LOC.Value;
                                                costosNew.CtoLocEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1LOC.Value;
                                                costosNew.FobExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor2EXT.Value;
                                                costosNew.CtoExtEntradaIFRS.Value = costeoOrigen.CosteoTipo.Value == (byte)TipoCosteoInv.SinCosto ? 0 : mvtoDeta.Valor1EXT.Value;
                                            }
                                            #region Si existe el costo lo actualiza  sino lo agrega
                                            DTO_inCostosExistencias costoExist = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_GetByParameter(costosNew);
                                            if (costoExist != null)
                                            {
                                                costoExist.CantEntrada.Value += mvtoDeta.CantidadUNI.Value.Value;
                                                costoExist.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                                costoExist.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                                costoExist.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                                costoExist.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                                if (dtoMvto.LibroIFRSInd.Value.Value)
                                                {
                                                    costoExist.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                    costoExist.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                    costoExist.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                    costoExist.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                                }
                                                this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costoExist);
                                                regCostos = costoExist.Consecutivo.Value.Value;
                                            }
                                            else
                                                regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew);
                                            #endregion
                                            #endregion
                                        }
                                        else
                                        {
                                            costos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(saldosCostosResult.RegistroCosto.Value.Value);
                                            costos.CantEntrada.Value += itemMvto.CantidadTraslado.Value.Value;
                                            costos.CtoLocEntrada.Value += mvtoDeta.Valor1LOC.Value;
                                            costos.CtoExtEntrada.Value += mvtoDeta.Valor1EXT.Value;
                                            costos.FobLocEntrada.Value += mvtoDeta.Valor2LOC.Value;
                                            costos.FobExtEntrada.Value += mvtoDeta.Valor2EXT.Value;
                                            if (dtoMvto.LibroIFRSInd.Value.Value)
                                            {
                                                costos.CtoLocEntradaIFRS.Value += mvtoDeta.Valor1LOC.Value;
                                                costos.CtoExtEntradaIFRS.Value += mvtoDeta.Valor1EXT.Value;
                                                costos.FobLocEntradaIFRS.Value += mvtoDeta.Valor2LOC.Value;
                                                costos.FobExtEntradaIFRS.Value += mvtoDeta.Valor2EXT.Value;
                                            }
                                            this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Upd(costos);
                                        }
                                    }

                                    #endregion
                                    #region Agrega un Control de Saldos y Costos
                                    if (saldosCostosDestino == null)
                                    {
                                        DTO_inControlSaldosCostos saldosCostosNew = new DTO_inControlSaldosCostos();
                                        saldosCostosNew.EmpresaID.Value = transaccionNotaEnvio.Header.EmpresaID.Value;
                                        saldosCostosNew.ActivoID.Value = mvtoDeta.ActivoID.Value;
                                        saldosCostosNew.BodegaID.Value = transaccionNotaEnvio.Header.BodegaOrigenID.Value;
                                        saldosCostosNew.CosteoGrupoInvID.Value = bodegaDestino.CosteoGrupoInvID.Value;
                                        saldosCostosNew.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                                        saldosCostosNew.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                                        saldosCostosNew.Parametro1.Value = mvtoDeta.Parametro1.Value;
                                        saldosCostosNew.Parametro2.Value = mvtoDeta.Parametro2.Value;
                                        saldosCostosNew.IdentificadorTr.Value = costeoDestino.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? mvtoDeta.Consecutivo.Value : 0;
                                        saldosCostosNew.Periodo.Value = transaccionNotaEnvio.DocCtrl.PeriodoDoc.Value;
                                        saldosCostosNew.RegistroCosto.Value = regCostos == 0 ? saldosCostosResult.RegistroCosto.Value.Value : regCostos;
                                        saldosCostosNew.RegistroSaldo.Value = regSaldos;
                                        this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(saldosCostosNew);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_In_InvalidPeriod;
                                    return result;
                                }
                            }
                            else
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = "Error: Salida o Traslado no validado antes de generar movimiento, revisar code";
                                return result;
                            }
                        }
                        listMvtoDeta.Add(mvtoDeta);
                        #endregion                        
                        #region Agrega un mvto de entrada a bodega destino
                        DTO_glMovimientoDeta traslado = new DTO_glMovimientoDeta();
                        traslado.ActivoID.Value = mvtoDeta.ActivoID.Value;
                        traslado.BodegaID.Value = documentID != AppDocuments.NotaEnvio ? bodegaDestino.ID.Value : bodegaTransacional;
                        traslado.CantidadEMP.Value = mvtoDeta.CantidadEMP.Value;
                        traslado.CantidadUNI.Value = itemMvto.CantidadTraslado.Value;
                        traslado.CentroCostoID.Value = mvtoDeta.CentroCostoID.Value;
                        traslado.Consecutivo.Value = mvtoDeta.Consecutivo.Value;
                        traslado.DescripTExt.Value = mvtoDeta.DescripTExt.Value;
                        traslado.DocSoporte.Value = mvtoDeta.DocSoporte.Value;
                        traslado.EmpaqueInvID.Value = mvtoDeta.EmpaqueInvID.Value;
                        traslado.EmpresaID.Value = mvtoDeta.EmpresaID.Value;
                        traslado.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                        traslado.EstadoInv.Value = mvtoDeta.EstadoInv.Value;
                        traslado.Fecha.Value = mvtoDeta.Fecha.Value;
                        traslado.Index = listMvtoDeta.Count-1;
                        traslado.IdentificadorTr.Value = mvtoDeta.IdentificadorTr.Value;
                        traslado.inReferenciaID.Value = mvtoDeta.inReferenciaID.Value;
                        traslado.Kit.Value = mvtoDeta.Kit.Value;
                        traslado.MvtoTipoInvID.Value = mvtoDeta.MvtoTipoInvID.Value;
                        traslado.NumeroDoc.Value = mvtoDeta.NumeroDoc.Value;
                        traslado.Parametro1.Value = mvtoDeta.Parametro1.Value;
                        traslado.Parametro2.Value = mvtoDeta.Parametro2.Value;
                        traslado.ProyectoID.Value = mvtoDeta.ProyectoID.Value;
                        traslado.TerceroID.Value = mvtoDeta.TerceroID.Value;
                        traslado.SerialID.Value = mvtoDeta.SerialID.Value;
                        traslado.Valor1LOC.Value = mvtoDeta.Valor1LOC.Value;
                        traslado.Valor1EXT.Value = mvtoDeta.Valor1EXT.Value;
                        traslado.Valor2LOC.Value = mvtoDeta.Valor2LOC.Value;
                        traslado.Valor2EXT.Value = mvtoDeta.Valor2EXT.Value;
                        traslado.ValorUNI.Value = mvtoDeta.ValorUNI.Value;
                        listMvtoDeta.Add(traslado);
                        #endregion
                        this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actFlujoID, true, _dtoControl.Observacion.Value);
                    }
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #region Borra de glMovimientoDetaPRE / Agrega a glMovimientoDeta
                this._moduloGlobal.glMovimientoDetaPRE_Delete(transaccionNotaEnvio.DocCtrl.NumeroDoc.Value.Value,true);
                if (listMvtoDeta.Any(x => string.IsNullOrEmpty(x.BodegaID.Value)))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "La bodega en el detalle esta vacía, revisar";
                    return result;
                } 
                result = this._moduloGlobal.glMovimientoDeta_Add(listMvtoDeta,true,true);
                if (result.Result == ResultValue.NOK)
                    return result;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Actualiza el documento control
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, _dtoControl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(_dtoControl.NumeroDoc.Value.Value, true);
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
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "NotaEnvio_RecibirDevolver");

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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (_dtoControl.DocumentoNro.Value == 0)
                        {
                            _dtoControl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, _dtoControl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(_dtoControl, true, false, false);
                            alarma.Consecutivo = _dtoControl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            #region Genera el reporte de Transaccion Manual aprobacion
            if (createDoc)
            {
                try
                {
                    if (_dtoControl.DocumentoID.Value.Value == AppDocuments.TransaccionManual)
                    {
                        DTO_MvtoInventarios transaccion = null;
                        //this._moduloAplicacion.GenerarArchivo(documentID, _dtoControl.NumeroDoc.Value.Value, DtoReportTansaccMnl(numeroDoc, true, transaccion));
                    }
                    else if (_dtoControl.DocumentoID.Value.Value == AppDocuments.NotaEnvio)
                    {
                        DTO_MvtoInventarios transaccion = null;
                        //this._moduloAplicacion.GenerarArchivo(documentID, _dtoControl.NumeroDoc.Value.Value, DtoReportNotaEnvio(numeroDoc, true, transaccion));
                    }
                }
                catch (Exception)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Obtiene una lista de movimientos Docu
        /// </summary>
        /// <param name="documentoID">Documento Asociado</param>
        /// <param name="header">Filtro para consulta</param>
        /// <returns>List DTO_inMovimientoDocu </returns>
        public List<DTO_inMovimientoDocu> inMovimientoDocu_GetByParameter(int documentoID, DTO_inMovimientoDocu header)
        {
            List<DTO_inMovimientoDocu> result = new List<DTO_inMovimientoDocu>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_inMovimientoDocu.DAL_inMovimientoDocu_GetByParameter(header);
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inMovimientoDocu_GetByParameter");
                return new List<DTO_inMovimientoDocu>();
            }
        }

        /// <summary>
        /// Actualiza la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        public DTO_TxResult inMovimientoDocu_Upd(int documentoID, DTO_inMovimientoDocu header)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inMovimientoDocu.DAL_inMovimientoDocu_Upd(header);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, header.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inMovimientoDocu_Upd");
                return result;
            }
        }

        /// <summary>
        /// Actualiza la tabla inMovimientoDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        public List<DTO_inDeterioroAprob> inMovimientoDocu_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inMovimientoDocu = (DAL_inMovimientoDocu)base.GetInstance(typeof(DAL_inMovimientoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                List<DTO_inDeterioroAprob> list = this._dal_inMovimientoDocu.DAL_inMovimientoDocu_GetPendientesByModulo(mod, actFlujoID, usuarioID);
                foreach (DTO_inDeterioroAprob itemAprob in list)
                {
                    itemAprob.ValorTotalML.Value = itemAprob.Detalle.Sum(x => x.Valor1LOC.Value.Value);
                    itemAprob.ValorTotalME.Value = itemAprob.Detalle.Sum(x => x.Valor1EXT.Value.Value);
                }

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inMovimientoDocu_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Traslado Saldos de inventario a un nuevo periodo
        /// </summary>
        /// <param name="documentID"> documento relacionado</param>
        /// <param name="periodoID">periodo actual del modulo</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        public DTO_TxResult Proceso_TrasladoSaldosInventario(int documentID, DateTime periodoOld, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            #region Variables
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            #endregion

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                DateTime periodoNew = periodoOld.AddMonths(1);  
                DTO_inControlSaldosCostos filter = new DTO_inControlSaldosCostos();
                filter.Periodo.Value = periodoOld;
                List<DTO_inControlSaldosCostos> all = this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_GetByParameter(filter);
                porcParte = all.Count != 0? 100 / all.Count : 100;
                Dictionary<int, int> consecCostos = new Dictionary<int, int>();
                #endregion
                #region Agrega los saldos al nuevo periodo
                foreach (DTO_inControlSaldosCostos f in all)
                {
                    porcTotal += porcParte;
                    #region Agrega un Saldo de Existencias
                    int regSaldos = 0;
                    DTO_inSaldosExistencias saldosNew = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Get(f.RegistroSaldo.Value.Value);
                    saldosNew.CantInicial.Value = saldosNew.CantInicial.Value + saldosNew.CantEntrada.Value - saldosNew.CantRetiro.Value;
                    saldosNew.CantEntrada.Value = 0;
                    saldosNew.CantRetiro.Value = 0;
                    saldosNew.Consecutivo.Value = null;
                    saldosNew.Periodo.Value = periodoNew;
                    if (saldosNew.CantInicial.Value != 0)
                        regSaldos = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_Add(saldosNew);
                    #endregion
                    #region Agrega un Costo de Existencias si no existe
                    int regCostos = f.RegistroCosto.Value.Value;
                    DTO_inCostosExistencias costosNew = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Get(f.RegistroCosto.Value.Value);
                    costosNew.Consecutivo.Value = null;
                    costosNew.Periodo.Value = periodoNew;
                    if (!consecCostos.ContainsKey(f.RegistroCosto.Value.Value))
                    {
                        costosNew.CantInicial.Value = costosNew.CantInicial.Value + costosNew.CantEntrada.Value - costosNew.CantRetiro.Value;
                        costosNew.CantEntrada.Value = 0;
                        costosNew.CantRetiro.Value = 0;
                        costosNew.FobExtSaldoIni.Value = costosNew.FobExtSaldoIni.Value + costosNew.FobExtEntrada.Value - costosNew.FobExtSalida.Value;
                        costosNew.FobLocSaldoIni.Value = costosNew.FobLocSaldoIni.Value + costosNew.FobLocEntrada.Value - costosNew.FobLocSalida.Value;
                        costosNew.CtoLocSaldoIni.Value = costosNew.CtoLocSaldoIni.Value + costosNew.CtoLocEntrada.Value - costosNew.CtoLocSalida.Value;
                        costosNew.CtoExtSaldoIni.Value = costosNew.CtoExtSaldoIni.Value + costosNew.CtoExtEntrada.Value - costosNew.CtoExtSalida.Value;
                        costosNew.FobExtEntrada.Value = 0;
                        costosNew.FobExtSalida.Value = 0;
                        costosNew.FobLocEntrada.Value = 0;
                        costosNew.FobLocSalida.Value = 0;
                        costosNew.CtoLocEntrada.Value = 0;
                        costosNew.CtoLocSalida.Value = 0;
                        costosNew.CtoExtEntrada.Value = 0;
                        costosNew.CtoExtSalida.Value = 0;
                        if (costosNew.CantInicial.Value != 0)
                            regCostos = this._dal_mvtoSaldosCostos.DAL_inCostosExistencias_Add(costosNew);
                        consecCostos.Add(f.RegistroCosto.Value.Value, regCostos);
                    }
                    else
                        regCostos = consecCostos[f.RegistroCosto.Value.Value];
                    #endregion
                        #region Agrega un Control de Saldos y Costos
                    f.Periodo.Value = periodoNew;               
                    f.RegistroSaldo.Value = regSaldos;
                    f.RegistroCosto.Value = regCostos;
                    if(regSaldos != 0)
                        this._dal_mvtoSaldosCostos.DAL_inControlSaldosCostos_Add(f);
                    #endregion
                }                 
 
                porcTotal = 100;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion                                        
             
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.NOK)
                    return result;
                else
                {
                    #region Actualiza Periodo
                    //string EmpNro = this.Empresa.NumeroControl.Value;
                    //string keyControl = EmpNro + "08" + AppControl.in_Periodo;
                    //DTO_glControl glControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                    //if (glControl != null)
                    //{
                    //    glControl.Data.Value = periodoNew.ToString(FormatString.ControlDate);
                    //    this._moduloGlobal.glControl_Update(glControl);
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_TrasladoSaldosInv");

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
            return result;
        }

        #endregion

        #endregion

        #region Inventario Fisico

        /// <summary>
        /// Rechaza  Inventario fisico
        /// </summary>
        /// <param name="documentID">Documento asociado</param>
        /// <param name="invAprob"> Inventario Fisico</param>
        /// <param name="actFlujoID"> Actividad actual</param>
        private void InventarioFisico_Rechazar(int documentID, DTO_InvFisicoAprobacion invAprob, string actFlujoID,bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool isValid = true;
            try
            {
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, invAprob.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, invAprob.Observacion.Value, true);
                this.AsignarFlujo(documentID, invAprob.NumeroDoc.Value.Value, actFlujoID, true,invAprob.Observacion.Value);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.In_InventarioFisicoNotExist, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "InventarioFisico_Rechazar");
                throw exception;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Guardar el inventario fisico
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="bodega">Bodega relacionada</param>
        /// <param name="fechaDoc">Periodo del inventario fisico</param>
        /// <param name="numeroDoc">Numero Doc relacionado</param>
        /// <param name="fisicoInventario"></param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject InventarioFisico_Add(int documentID, string bodega, DateTime fechaDoc, out int numeroDoc, List<DTO_inFisicoInventario> fisicoInventario, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            numeroDoc = 0;
            DTO_glDocumentoControl _dtoControl = null;
            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inFisicoInventario = (DAL_inFisicoInventario)base.GetInstance(typeof(DAL_inFisicoInventario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Guarda un Doc Control
               
                if (fisicoInventario.Count > 0)
                {
                    bool _existInvFisico = this._dal_inFisicoInventario.DAL_inFisicoInventario_Exist(bodega, new DateTime(fechaDoc.Year, fechaDoc.Month, 1));
                    #region Agrega un Doc Control
                    if (!_existInvFisico)
                    {
                        _dtoControl = new DTO_glDocumentoControl();
                        DTO_inBodega _bodega = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodega, true, false);
                        DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(),true, true);

                        _dtoControl.NumeroDoc.Value = 0;
                        _dtoControl.DocumentoNro.Value = 0;
                        _dtoControl.ComprobanteIDNro.Value = 0;
                        _dtoControl.ProyectoID.Value = _bodega.ProyectoID.Value;
                        _dtoControl.CentroCostoID.Value = _bodega.CentroCostoID.Value;
                        _dtoControl.PrefijoID.Value = _bodega.PrefijoID.Value;
                        _dtoControl.Fecha.Value = DateTime.Now;
                        _dtoControl.FechaDoc.Value = fechaDoc;
                        _dtoControl.PeriodoDoc.Value = new DateTime(fechaDoc.Year,fechaDoc.Month,1);
                        _dtoControl.TasaCambioCONT.Value = 0;
                        _dtoControl.TasaCambioDOCU.Value = 0;
                        _dtoControl.DocumentoID.Value = documentID;
                        _dtoControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        _dtoControl.PeriodoUltMov.Value = _dtoControl.PeriodoDoc.Value;
                        _dtoControl.seUsuarioID.Value = this.UserId;
                        _dtoControl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        _dtoControl.ConsSaldo.Value = 0;
                        _dtoControl.Observacion.Value = bodega;
                        _dtoControl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        _dtoControl.Descripcion.Value = doc.Descriptivo.Value;

                        resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, _dtoControl, true);
                        if (resultGLDC.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(resultGLDC);
                            return result;
                        }
                        else
                        {
                            _dtoControl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            numeroDoc = _dtoControl.NumeroDoc.Value.Value;
                        }
                    }
                    else
                    {
                        _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(fisicoInventario[0].NumeroDoc.Value.Value);
                        numeroDoc = _dtoControl.NumeroDoc.Value.Value;
                    }
                    #endregion
                    #region Guarda en la bitacora
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, _dtoControl.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                    #endregion
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                #endregion
                #region Carga los saldos a inFisicoInventario
                if (fisicoInventario.Count > 0)
                    this._dal_inFisicoInventario.DAL_inFisicoInventario_Delete(numeroDoc);
                foreach (var fisico in fisicoInventario)
                {
                    fisico.NumeroDoc.Value = numeroDoc;
                    this._dal_inFisicoInventario.DAL_inFisicoInventario_Add(fisico);
                }
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                        return alarma;
                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    }
                    #endregion
                }
                return result;

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (_dtoControl.DocumentoNro.Value == 0)
                        {
                            _dtoControl.DocumentoNro.Value = this.GenerarDocumentoNro(_dtoControl.DocumentoID.Value.Value, _dtoControl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(_dtoControl, true, false, false);
                            //alarma.Consecutivo = _dtoControl.DocumentoNro.Value.ToString();
                        }
                    }
                    else
                        throw new Exception("InventarioFisico_Add - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

          
        }

        /// <summary>
        ///Obtiene un inventario fisico
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="fisicoInv">Dto filtro</param>
        /// <returns> lista de DTO_inFisicoInventario</returns>
        public List<DTO_inFisicoInventario> InventarioFisico_Get(int documentID, DTO_inFisicoInventario fisicoInv)
        {
            try
            {
                List<DTO_inFisicoInventario> listFisicoInv = new List<DTO_inFisicoInventario>();
                this._dal_inFisicoInventario = (DAL_inFisicoInventario)base.GetInstance(typeof(NewAge.ADO.DAL_inFisicoInventario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloActivos = (ModuloActivosFijos)this.GetInstance(typeof(ModuloActivosFijos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                listFisicoInv = this._dal_inFisicoInventario.DAL_inFisicoInventario_GetByParameter(fisicoInv);
                string param1xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                string param2xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                foreach (var item in listFisicoInv)
                {
                    DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, item.inReferenciaID.Value, true, false);
                    string referenciaParametrosID = referencia.ID.Value;
                    string referenciaParametrosDesc = referencia.Descriptivo.Value;
                    if (item.EstadoInv.Value != (byte)EstadoInv.Activo)
                        referenciaParametrosID += "-U";
                    if (item.ActivoID.Value != 0)
                    {
                        DTO_acActivoControl activo = this._moduloActivos.acActivoControl_GetByID(item.ActivoID.Value.Value);
                        item.SerialID.Value = activo != null ? activo.SerialID.Value : string.Empty;
                    }
                    if (item.Parametro1.Value != param1xDef.ToUpper())
                    {
                        DTO_MasterBasic param1 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro1, item.Parametro1.Value, true, false);
                        referenciaParametrosID += "-" + param1.ID.Value;
                        referenciaParametrosDesc += "-" + param1.Descriptivo.Value;
                    }
                    if (item.Parametro2.Value != param2xDef.ToUpper())
                    {
                        DTO_MasterBasic param2 = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro2, item.Parametro2.Value, true, false);
                        referenciaParametrosID += "-" + param2.ID.Value;
                        referenciaParametrosDesc += "-" + param2.Descriptivo.Value;
                    }
                    if (string.IsNullOrEmpty(item.NumeroDoc.Value.ToString()))
                        item.NumeroDoc.Value = 0;
                    item.ReferenciaP1P2ID.Value = referenciaParametrosID;
                    item.ReferenciaP1P2Desc.Value = referenciaParametrosDesc;
                }
                return listFisicoInv;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_Get");
                return new List<DTO_inFisicoInventario>();
            }
        }

        /// <summary>
        /// Envia para aprobacion un inventario fisico
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject InventarioFisico_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl _docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (_docCtrl != null)
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), _docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }

                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #region Asigna el nuevo flujo
                    result = this.AsignarFlujo(documentID, _docCtrl.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion

                    if (createDoc)
                    {
                        try
                        {
                            #region Generar el nuevo archivo
                            //if (documentID == AppDocuments.CajaMenor)
                            //    this.GenerarArchivo(documentID, numeroDoc, DtoCajaMenorReport(numeroDoc));
                            #endregion

                            //Asigna las alarmas
                            DTO_Alarma alarma = this.GetFirstMailInfo(_docCtrl.NumeroDoc.Value.Value, createDoc);
                            return alarma;
                        }
                        catch (Exception)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                            return result;
                        }
                    }
                    else
                        batchProgress[tupProgress] = 100;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_SendToAprobCompr;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_SendToAprob");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Aprueba o rechaza Inventario Fisico
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="invFisico">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> InventarioFisico_AprobarRechazar(int documentID, List<DTO_InvFisicoAprobacion> invFisico, string actFlujoID, bool updDocCtrl, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100 / 2;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var invAprob in invFisico)
                {
                    porcTemp = (porcParte * i) / invFisico.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (invAprob.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.InventarioFisico_Aprobar(documentID, invAprob, actFlujoID, createDoc, batchProgress, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "InventarioFisico_Aprobar");
                            rd.Message = DictionaryMessages.Err_In_BodegaRechazar + "&&" + invAprob.BodegaID.Value.ToString() + "&&" + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (invAprob.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.InventarioFisico_Rechazar(documentID, invAprob,actFlujoID, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "InventarioFisico_Rechazar");
                            rd.Message = DictionaryMessages.Err_In_BodegaRechazar + "&&" + invAprob.BodegaID.Value.ToString() + "&&" + errMsg;
                            result.Details.Add(rd);
                        }
                    }                  
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(invAprob.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Envia para aprobacion un inventario fisico
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="invAprob">DTO de inventario aprobacion</param>
        /// <param name="actFlujoID">Actividad Actual</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult InventarioFisico_Aprobar(int documentID,DTO_InvFisicoAprobacion invAprob, string actFlujoID, bool createDoc,Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            decimal porcTotal = 0;
            decimal porcParte = 100 / 2;
            DTO_glDocumentoControl _dtoControlEntradas = null;
            DTO_glDocumentoControl _dtoControlSalidas = null;
            int numeroDocEntrada = 0;
            int numeroDocSalida = 0;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)this.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjera = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string prefijo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                DTO_glDocumentoControl ctrlInvFisico = this._moduloGlobal.glDocumentoControl_GetByID(invAprob.NumeroDoc.Value.Value);
                //Cambia estado del doc de Inventario Fisico
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, invAprob.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, invAprob.Observacion.Value, true);
                    
                DTO_inBodega bodega = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, invAprob.BodegaID.Value, true, false);
                DTO_inCosteoGrupo costeoBod = bodega != null? (DTO_inCosteoGrupo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, bodega.CosteoGrupoInvID.Value, true, false) : null;
                #region Crea Movimientos de Entrada y Salida (glMovimientoDeta)
                List<DTO_glMovimientoDeta> mvtoEntrada = new List<DTO_glMovimientoDeta>();
                List<DTO_glMovimientoDeta> mvtoSalida = new List<DTO_glMovimientoDeta>();
                List<DTO_inMovimientoFooter> footerMvto = new List<DTO_inMovimientoFooter>();
                this.LoadMvtoInventarioFisico(bodega, invAprob.PeriodoID.Value.Value, ref mvtoEntrada, ref mvtoSalida);
                #endregion;
                if (mvtoEntrada.Count > 0)
                {
                    DTO_MvtoInventarios transaccionEntrada = new DTO_MvtoInventarios();
                    #region Crea Doc Control Entradas
                    _dtoControlEntradas = new DTO_glDocumentoControl();
                    _dtoControlEntradas.NumeroDoc.Value = 0;
                    _dtoControlEntradas.DocumentoID.Value = AppDocuments.TransaccionAutomatica;
                    _dtoControlEntradas.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    _dtoControlEntradas.ProyectoID.Value = bodega.ProyectoID.Value;
                    _dtoControlEntradas.CentroCostoID.Value = bodega.CentroCostoID.Value;
                    _dtoControlEntradas.DocumentoNro.Value = 0;
                    _dtoControlEntradas.ComprobanteIDNro.Value = 0;
                    _dtoControlEntradas.PrefijoID.Value = prefijo;
                    _dtoControlEntradas.Fecha.Value = DateTime.Now;
                    _dtoControlEntradas.PeriodoDoc.Value = invAprob.PeriodoID.Value.Value;
                    _dtoControlEntradas.PeriodoUltMov.Value = invAprob.PeriodoID.Value.Value;
                    _dtoControlEntradas.FechaDoc.Value = ctrlInvFisico.FechaDoc.Value;
                    _dtoControlEntradas.TasaCambioCONT.Value = 0;
                    _dtoControlEntradas.TasaCambioDOCU.Value = 0;                   
                    _dtoControlEntradas.seUsuarioID.Value = this.UserId;
                    _dtoControlEntradas.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    _dtoControlEntradas.ConsSaldo.Value = 0;
                    _dtoControlEntradas.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    _dtoControlEntradas.Descripcion.Value = "Entrada Inventario Fisico ";
                    _dtoControlEntradas.Observacion.Value = "Entrada por Inventario Fisico " + _dtoControlEntradas.Fecha.Value.ToString();
                    transaccionEntrada.DocCtrl = _dtoControlEntradas;
                    #endregion
                    #region Crea inMovimientoDocu Entradas
                    DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
                    header.NumeroDoc.Value = _dtoControlEntradas.NumeroDoc.Value;
                    header.BodegaOrigenID.Value = invAprob.BodegaID.Value;
                    header.MvtoTipoInvID.Value = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovEntradaInvFisico);
                    transaccionEntrada.Header = header;
                    #endregion;
                    #region Crea movimientos de Entradas
                    List<DTO_inMovimientoFooter> movInventarioList = new List<DTO_inMovimientoFooter>();                   
                    foreach (DTO_glMovimientoDeta mov in mvtoEntrada)
                    {
                        DTO_inMovimientoFooter movFooter = new DTO_inMovimientoFooter();
                        movFooter.Movimiento = mov;
                        movInventarioList.Add(movFooter);
                    }                     
                    transaccionEntrada.Footer = movInventarioList;
                    #endregion                    
                    var resultEntrada = this.Transaccion_Add(AppDocuments.TransaccionAutomatica,transaccionEntrada, false, out numeroDocEntrada, batchProgress, true);
                    if (resultEntrada.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult resInv = (DTO_TxResult)resultEntrada;
                        if (resInv.Result == ResultValue.NOK)
                        {
                            result = resInv;
                            return result;
                        }
                    }
                    else
                        _dtoControlEntradas.NumeroDoc.Value = numeroDocEntrada;     
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                if (mvtoSalida.Count > 0)
                {
                    DTO_MvtoInventarios transaccionSalida = new DTO_MvtoInventarios();
                    #region Crea Doc Control Salidas
                    _dtoControlSalidas = new DTO_glDocumentoControl();
                    _dtoControlSalidas.NumeroDoc.Value = 0;
                    _dtoControlSalidas.DocumentoID.Value = AppDocuments.TransaccionAutomatica;
                    _dtoControlSalidas.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    _dtoControlSalidas.ProyectoID.Value = bodega.ProyectoID.Value;
                    _dtoControlSalidas.CentroCostoID.Value = bodega.CentroCostoID.Value;
                    _dtoControlSalidas.DocumentoNro.Value = 0;
                    _dtoControlSalidas.ComprobanteIDNro.Value = 0;
                    _dtoControlSalidas.PrefijoID.Value = prefijo;
                    _dtoControlSalidas.Fecha.Value = DateTime.Now;
                    _dtoControlSalidas.FechaDoc.Value = ctrlInvFisico.FechaDoc.Value;
                    _dtoControlSalidas.PeriodoDoc.Value = invAprob.PeriodoID.Value.Value;
                    _dtoControlSalidas.PeriodoUltMov.Value = invAprob.PeriodoID.Value.Value;
                    _dtoControlSalidas.TasaCambioCONT.Value = 0;
                    _dtoControlSalidas.TasaCambioDOCU.Value = 0;
                    _dtoControlSalidas.seUsuarioID.Value = this.UserId;
                    _dtoControlSalidas.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    _dtoControlSalidas.ConsSaldo.Value = 0;
                    _dtoControlSalidas.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    _dtoControlSalidas.Descripcion.Value = "Salida Inventario Fisico ";
                    _dtoControlSalidas.Observacion.Value = "Salida por Inventario Fisico " + _dtoControlSalidas.Fecha.Value.ToString();
                    transaccionSalida.DocCtrl = _dtoControlSalidas;
                    #endregion
                    #region Crea en inMovimientoDocu Salidas
                    DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
                    header.NumeroDoc.Value = _dtoControlSalidas.NumeroDoc.Value;
                    header.BodegaOrigenID.Value = invAprob.BodegaID.Value ;
                    header.MvtoTipoInvID.Value = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovSalidaInvFisico);
                    transaccionSalida.Header = header;
                    #endregion;
                    #region Crea movimientos de Salidas
                    List<DTO_inMovimientoFooter> movInventarioList = new List<DTO_inMovimientoFooter>();
                    foreach (DTO_glMovimientoDeta mov in mvtoSalida)
                    {
                        DTO_inMovimientoFooter movFooter = new DTO_inMovimientoFooter();
                        movFooter.Movimiento = mov;
                        movInventarioList.Add(movFooter);
                    }
                    transaccionSalida.Footer = movInventarioList;
                    #endregion
                    var resultSalida = this.Transaccion_Add(AppDocuments.TransaccionAutomatica, transaccionSalida, false, out numeroDocSalida, batchProgress, true);
                    if (resultSalida.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult resInv = (DTO_TxResult)resultSalida;
                        if (resInv.Result == ResultValue.NOK)
                        {
                            result = resInv;
                            return result;
                        }
                    }
                    else
                        _dtoControlSalidas.NumeroDoc.Value = numeroDocSalida;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                batchProgress[tupProgress] = 100;
                this.AsignarFlujo(documentID, invAprob.NumeroDoc.Value.Value, actFlujoID,false, invAprob.Observacion.Value);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_Aprobar");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        #region Documento de inventarios(56)(si existe)
                        if (numeroDocEntrada != 0 && _dtoControlEntradas != null && _dtoControlEntradas.DocumentoNro.Value == 0)
                        {
                            _dtoControlEntradas.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.TransaccionAutomatica, _dtoControlEntradas.PrefijoID.Value);

                            //Comprobante 
                            DTO_coComprobante comproInvent = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, _dtoControlEntradas.ComprobanteID.Value, true, false);
                            if (comproInvent != null)
                                _dtoControlEntradas.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comproInvent, _dtoControlEntradas.PrefijoID.Value, _dtoControlEntradas.PeriodoDoc.Value.Value, _dtoControlEntradas.DocumentoNro.Value.Value);
                            else
                                _dtoControlEntradas.ComprobanteIDNro.Value = 0;
                            this._moduloGlobal.ActualizaConsecutivos(_dtoControlEntradas, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(numeroDocEntrada, _dtoControlEntradas.ComprobanteIDNro.Value.Value, false);
                        }
                        #endregion
                        if (numeroDocSalida != 0 && _dtoControlSalidas != null && _dtoControlSalidas.DocumentoNro.Value == 0) 
                        {
                            _dtoControlSalidas.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.TransaccionAutomatica, _dtoControlEntradas.PrefijoID.Value);

                            //Comprobante 
                            DTO_coComprobante comproInvent = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, _dtoControlSalidas.ComprobanteID.Value, true, false);
                            if (comproInvent != null)
                                _dtoControlSalidas.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comproInvent, _dtoControlSalidas.PrefijoID.Value, _dtoControlSalidas.PeriodoDoc.Value.Value, _dtoControlSalidas.DocumentoNro.Value.Value);
                            else
                                _dtoControlSalidas.ComprobanteIDNro.Value = 0;
                            this._moduloGlobal.ActualizaConsecutivos(_dtoControlSalidas, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(numeroDocSalida, _dtoControlSalidas.ComprobanteIDNro.Value.Value, false);
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae un listado de inventario fisico para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        ///  <param name="actFlujoID">Actividad Actual</param>
        /// <returns>Retorna un listado</returns>
        public List<DTO_InvFisicoAprobacion> InventarioFisico_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inFisicoInventario = (DAL_inFisicoInventario)base.GetInstance(typeof(DAL_inFisicoInventario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                List<DTO_InvFisicoAprobacion> list = this._dal_inFisicoInventario.DAL_inFisicoInventario_GetPendientesByModulo(mod, actFlujoID, usuarioID);
                foreach (DTO_InvFisicoAprobacion itemAprob in list)
                {
                    DTO_inBodega bodega = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega,itemAprob.BodegaID.Value, true, false);
                    itemAprob.BodegaDesc.Value = bodega.Descriptivo.Value;
                    itemAprob.FileUrl = base.GetFileRemotePath(itemAprob.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                    #region Calcula Cantidades y Valores de Ajuste para Aprobar
                    DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
                    fisico.BodegaID.Value = itemAprob.BodegaID.Value;
                    List<DTO_inFisicoInventario> fisicoInventario = this._dal_inFisicoInventario.DAL_inFisicoInventario_GetByParameter(fisico);
                    decimal _unidAjEntrada = 0;
                    decimal _unidAjSalida = 0;
                    decimal _valEntrada = 0;
                    decimal _valSalida = 0;
                    foreach (var itemFisico in fisicoInventario)
                    {
                        if (itemFisico.CantAjuste.Value > 0)
                        {
                            _unidAjEntrada += itemFisico.CantAjuste.Value.Value;
                            _valEntrada += itemFisico.CantKardex.Value != 0 ? ((itemFisico.CostoLocal.Value.Value / itemFisico.CantKardex.Value.Value) * itemFisico.CantAjuste.Value.Value) : itemFisico.CostoLocal.Value.Value;
                        }
                        else if (itemFisico.CantAjuste.Value < 0)
                        {
                            _unidAjSalida += itemFisico.CantAjuste.Value.Value;
                            _valSalida += itemFisico.CantKardex.Value != 0 ? (itemFisico.CostoLocal.Value.Value / itemFisico.CantKardex.Value.Value) * Math.Abs(itemFisico.CantAjuste.Value.Value) : itemFisico.CostoLocal.Value.Value;
                        }
                    }

                    itemAprob.CantAjusteEntrada.Value = _unidAjEntrada;
                    itemAprob.CantAjusteSalida.Value = _unidAjSalida;
                    itemAprob.ValorAjusteEntrada.Value = _valEntrada;
                    itemAprob.ValorAjusteSalida.Value = _valSalida;
                    #endregion               
                }

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "InventarioFisico_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Elimina los saldos guardados de la bodega actual
        /// </summary>        
        public void InventarioFisico_Delete(int numeroDoc)
        {
            this._dal_inFisicoInventario = (DAL_inFisicoInventario)base.GetInstance(typeof(DAL_inFisicoInventario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_inFisicoInventario.DAL_inFisicoInventario_Delete(numeroDoc);
        }
        #endregion

        #region Distribucion Costos

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="transaccion">Dto del movimiento completo</param>
        /// <param name="costosDist">Dto de Distribucion Costos</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject inDistribucionCostos_Add(int documentID, DTO_MvtoInventarios transaccion, List<DTO_inDistribucionCosto> costosDist, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inDistribucionCosto = (DAL_inDistribucionCosto)base.GetInstance(typeof(DAL_inDistribucionCosto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal valorMvto = 0;
                DTO_glDocumentoControl docCtrl = null;
                #region Trae la actividad del documento
                List<string> actFlujoID = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                if (actFlujoID.Count == 0)
                {
                    numeroDoc = 0;
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_DocMultActivities;
                    return result;
                }
                #endregion
                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    transaccion.DocCtrl.DocumentoNro.Value = 0;
                    transaccion.DocCtrl.ComprobanteIDNro.Value = 0;
                    foreach (var item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (transaccion.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    transaccion.DocCtrl.Valor.Value = valorMvto;
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, transaccion.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                    transaccion.DocCtrl.NumeroDoc.Value = numeroDoc;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion                                        
                    #region Guardar en glMovimientoDeta
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    foreach (DTO_inMovimientoFooter item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        item.Movimiento.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        mov = item.Movimiento;
                        movDeta.Add(mov);
                    }
                    if (movDeta.Any(x => string.IsNullOrEmpty(x.BodegaID.Value)))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "La Bodega en el detalle esta vacía, revisar";
                        return result;
                    } 
                    result = this._moduloGlobal.glMovimientoDeta_Add(movDeta,true,true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #region Actualiza el detalle original con el DocSoporte(Consecutivo)
                    DTO_MvtoInventarios transaccionUpdate = this.Transaccion_Get(transaccion.Header.NumeroDoc.Value.Value);
                    foreach (DTO_inMovimientoFooter detaOrigen in transaccionUpdate.Footer)
                    {
                        detaOrigen.Movimiento.DocSoporte.Value = detaOrigen.Movimiento.Consecutivo.Value;
                        this._dal_glMovimientoDeta.DAL_glMovimientoDeta_Update(detaOrigen.Movimiento);
                    } 
                    #endregion

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en inDistribucionCostos
                    foreach (var item in costosDist)
                        item.NumeroDocINV.Value = transaccion.Header.NumeroDoc.Value;
                    this._dal_inDistribucionCosto.DAL_inDistribucionCosto_Add(costosDist);
                    #endregion
                    #region Guardar en inMovimientoDocu de Distribucion
                    DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
                    header.NumeroDoc.Value = numeroDoc;
                    header.BodegaOrigenID.Value = transaccion.Footer.First().Movimiento.BodegaID.Value;
                    header.MvtoTipoInvID.Value = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovDistribucionCostos);
                    header.DocumentoREL.Value = costosDist.First().NumeroDocCto.Value;// Relaciona la factura que fue distribuida
                    result = this.inMovimientoDocu_Add(documentID, header);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion;
                    #region Actualiza el documento control(cambia el estado)
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, string.Empty, true);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region PROCESA MOVIMIENTOS DE INVENTARIOS
                    result = this.Transaccion_Aprobar(documentID, transaccion.Header.NumeroDoc.Value.Value,actFlujoID[0], true, batchProgress, true,false);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion                                    
                }
                else
                {
                    #region Actualiza glDocumentoControl
                    foreach (var item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (transaccion.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    transaccion.DocCtrl.Valor.Value = valorMvto;
                    this._moduloGlobal.glDocumentoControl_Update(transaccion.DocCtrl, true, true);
                    this.AsignarFlujo(transaccion.DocCtrl.DocumentoID.Value.Value, transaccion.DocCtrl.NumeroDoc.Value.Value, actFlujoID[0], false, string.Empty);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza en inDistribucionCostos
                    this._dal_inDistribucionCosto.DAL_inDistribucionCosto_Delete(transaccion.DocCtrl.NumeroDoc.Value.Value);
                    this._dal_inDistribucionCosto.DAL_inDistribucionCosto_Add(costosDist);
                    #endregion
                    #region Actualiza en inMovimientoDocu
                    result = this.inMovimientoDocu_Upd(documentID, transaccion.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza en glMovimientoDeta
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    foreach (DTO_inMovimientoFooter item in transaccion.Footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                        item.Movimiento.NumeroDoc.Value = transaccion.Header.NumeroDoc.Value.Value;
                        item.Movimiento.CantidadUNI.Value = 0;
                        item.Movimiento.CantidadDoc.Value = 0;
                        item.Movimiento.CantidadEMP.Value = 0;
                        mov = item.Movimiento;
                        movDeta.Add(mov);
                    }
                    result = this._moduloGlobal.glMovimientoDeta_Update(movDeta,true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                }

                numeroDoc = transaccion.Header.NumeroDoc.Value.Value;
                docCtrl.NumeroDoc.Value = numeroDoc;


                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(docCtrl.NumeroDoc.Value.Value, true);
                        alarma.NumeroDoc = docCtrl.NumeroDoc.Value.ToString();
                        return alarma;
                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    }
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = update ? transaccion.Header.NumeroDoc.Value.Value : 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inDistribucionCostos_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (transaccion.DocCtrl.DocumentoNro.Value == 0)
                        {
                            transaccion.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(transaccion.DocCtrl.DocumentoID.Value.Value, transaccion.DocCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(transaccion.DocCtrl, true, false, false);
                            alarma.Consecutivo = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        ///Obtiene una lista de Distribucion Costo
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <param name="byNroDocFact">Indica si filtra por numero doc de la factura</param>
        /// <returns> lista de DTO_inDistribucionCosto</returns>
        public List<DTO_inDistribucionCosto> inDistribucionCosto_GetByNumeroDoc(int documentID,int numeroDoc, bool byNroDocFact)
        {
            try
            {
                List<DTO_inDistribucionCosto> listDistribucionCosto = new List<DTO_inDistribucionCosto>();
                this._dal_inDistribucionCosto = (DAL_inDistribucionCosto)base.GetInstance(typeof(DAL_inDistribucionCosto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                listDistribucionCosto = this._dal_inDistribucionCosto.DAL_inDistribucionCosto_GetByNumeroDoc(numeroDoc,byNroDocFact);

                return listDistribucionCosto;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inDistribucionCosto_GetByNumeroDoc");
                return new List<DTO_inDistribucionCosto>();
            }
        }

        #endregion

        #region Liquidacion de Importacion

        #region Funciones Privadas

        /// <summary>
        /// Adiciona en tabla inImportacionDocu
        /// </summary>
        /// <param name="importacionDocu">items a agregar a inImportacionDocu</param>
        /// <returns>Numero Doc</returns>
        internal DTO_TxResult inImportacionDocu_Add(int documentID, DTO_inImportacionDocu importacionDocu)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_inImportacionDocu = (DAL_inImportacionDocu)base.GetInstance(typeof(DAL_inImportacionDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inImportacionDocu.DAL_inImportacionDocu_Add(importacionDocu);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, importacionDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inImportacionDocu_Add");
                return result;
            }

        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="fisicoInventario"></param>
        /// <returns>una liquidacion Docu</returns>
        internal DTO_inImportacionDocu inImportacionDocu_GetByNumeroDoc(int numeroDoc)
        {
            this._dal_inImportacionDocu = (DAL_inImportacionDocu)base.GetInstance(typeof(DAL_inImportacionDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_inImportacionDocu docu =  this._dal_inImportacionDocu.DAL_inImportacionDocu_GetByNumeroDoc(numeroDoc);
            return docu;
        }

        /// <summary>
        /// Consulta una importacion Header segun un filtro de parametros
        /// </summary>
        /// <param name="header">Filtro de parametros</param>
        /// <returns>Dto de importacion Header</returns>
        internal List<DTO_inImportacionDocu> inImportacionDocu_GetByParameter(DTO_inImportacionDocu header)
        {
            List<DTO_inImportacionDocu> result = new List<DTO_inImportacionDocu>();
            this._dal_inImportacionDocu = (DAL_inImportacionDocu)base.GetInstance(typeof(DAL_inImportacionDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_inImportacionDocu> listDocu = this._dal_inImportacionDocu.DAL_inImportacionDocu_GetByParameter(header);
            return listDocu;
        }

        #endregion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="importacion">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject LiquidacionImportacion_Add(int documentID, DTO_LiquidacionImportacion importacion, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inImportacionDeta = (DAL_inImportacionDeta)base.GetInstance(typeof(DAL_inImportacionDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inImportacionDocu = (DAL_inImportacionDocu)base.GetInstance(typeof(DAL_inImportacionDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal valorMvto = 0;             

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    importacion.DocCtrl.DocumentoNro.Value = 0;
                    importacion.DocCtrl.ComprobanteIDNro.Value = 0;
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, importacion.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                    importacion.DocCtrl.NumeroDoc.Value = numeroDoc;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guardar en inImportacionDocu
                    importacion.Header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    result = this.inImportacionDocu_Add(documentID,importacion.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en inImportacionDeta
                    foreach (DTO_inImportacionDeta item in importacion.Footer)
                    {
                        item.Movimiento.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        item.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        this._dal_inImportacionDeta.DAL_inImportacionDeta_Add(item);
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;                    
                    #region Actualiza el documento control(cambia el estado)
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, string.Empty, true);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region PROCESA MOVIMIENTOS DE INVENTARIOS
                    //result = this.Transaccion_Aprobar(documentID, importacion.Header.NumeroDoc.Value.Value, true, batchProgress, true);
                    //if (result.Result == ResultValue.NOK)
                    //{
                    //    numeroDoc = 0;
                    //    return result;
                    //}
                    //porcTotal += porcParte;
                    //batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Genera el reporte de Transaccion Manual/Nota Envio aprobado
                    try
                    {
                    }
                    catch (Exception)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    }
                    #endregion
                }
                else
                {
                    #region Actualiza Doc Control
                    foreach (var item in importacion.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (importacion.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    importacion.DocCtrl.Valor.Value = valorMvto;
                    this._moduloGlobal.glDocumentoControl_Update(importacion.DocCtrl, true, true);

                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(importacion.DocCtrl.DocumentoID.Value.Value);
                    if (act.Count > 0)
                        this.AsignarFlujo(importacion.DocCtrl.DocumentoID.Value.Value, importacion.DocCtrl.NumeroDoc.Value.Value, act[0], false, string.Empty);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza en inImportacionDocu

                    this._dal_inImportacionDocu.DAL_inImportacionDocu_Upd(importacion.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = importacion.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza en inImportacionDeta
                    foreach (DTO_inImportacionDeta item in importacion.Footer)
                        this._dal_inImportacionDeta.DAL_inImportacionDeta_Upd(item);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }                  
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;                    
                    #region Actualiza en glMovimientoDeta
                    //List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    //foreach (DTO_inMovimientoFooter item in importacion.Footer)
                    //{
                    //    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                    //    item.Movimiento.NumeroDoc.Value = importacion.Header.NumeroDoc.Value.Value;
                    //    item.Movimiento.CantidadUNI.Value = 0;
                    //    item.Movimiento.CantidadDoc.Value = 0;
                    //    item.Movimiento.CantidadEMP.Value = 0;
                    //    mov = item.Movimiento;
                    //    movDeta.Add(mov);
                    //}
                    //result = this._moduloGlobal.glMovimientoDeta_Update(movDeta, true);
                    //if (result.Result == ResultValue.NOK)
                    //{
                    //    numeroDoc = importacion.Header.NumeroDoc.Value.Value;
                    //    return result;
                    //}

                    //porcTotal += porcParte;
                    //batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                }

                numeroDoc = importacion.Header.NumeroDoc.Value.Value;
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(numeroDoc, true);
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
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = update ? importacion.Header.NumeroDoc.Value.Value : 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "LiquidacionImportacion_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (importacion.DocCtrl.DocumentoNro.Value == 0)
                        {
                            importacion.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, importacion.DocCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(importacion.DocCtrl, true, false, false);
                            alarma.Consecutivo = importacion.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Deterioro / Revalorizacion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="deterioro">Dto del movimiento completo</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject Deterioro_Add(int documentID, DTO_MvtoInventarios deterioro, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                decimal valorMvto = 0;

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    deterioro.DocCtrl.DocumentoNro.Value = 0;
                    deterioro.DocCtrl.ComprobanteIDNro.Value = 0;
                    foreach (var item in deterioro.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (deterioro.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    deterioro.DocCtrl.Valor.Value = valorMvto;
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, deterioro.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                    deterioro.DocCtrl.NumeroDoc.Value = numeroDoc;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guardar en inMovimientoDocu
                    deterioro.Header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);                    
                    result = this.inMovimientoDocu_Add(documentID, deterioro.Header);
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
                    foreach (DTO_inMovimientoFooter item in deterioro.Footer)
                    {
                        DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
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
                    #region Actualiza Doc Control
		            foreach (var item in deterioro.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        if (deterioro.DocCtrl.MonedaID.Value == monedaLocal)
                            valorMvto += mov.Valor1LOC.Value.Value + mov.Valor2LOC.Value.Value;
                        else
                            valorMvto += mov.Valor1EXT.Value.Value + +mov.Valor2EXT.Value.Value;
                    }
                    deterioro.DocCtrl.Valor.Value = valorMvto;
                    this._moduloGlobal.glDocumentoControl_Update(deterioro.DocCtrl, true, true);

                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID( deterioro.DocCtrl.DocumentoID.Value.Value);
                    if (act.Count > 0)
                        this.AsignarFlujo( deterioro.DocCtrl.DocumentoID.Value.Value,  deterioro.DocCtrl.NumeroDoc.Value.Value, act[0], false, string.Empty);
                    
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal; 
	                #endregion
                    #region Actualiza en inMovimientoDocu
                    result = this.inMovimientoDocu_Upd(documentID, deterioro.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = deterioro.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;                    
                    #region Actualiza en glMovimientoDeta
                    foreach (DTO_inMovimientoFooter item in deterioro.Footer)
                    {
                        DTO_glMovimientoDeta mov = (DTO_glMovimientoDeta)item.Movimiento;
                        this._dal_glMovimientoDeta.DAL_glMovimientoDetaPRE_Update(mov);
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = deterioro.Header.NumeroDoc.Value.Value;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                }

                numeroDoc = deterioro.Header.NumeroDoc.Value.Value;
                
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(numeroDoc, true);
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
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = update ? deterioro.Header.NumeroDoc.Value.Value : 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Deterioro_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (deterioro.DocCtrl.DocumentoNro.Value == 0)
                        {
                            deterioro.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, deterioro.DocCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(deterioro.DocCtrl, true, false, false);
                            alarma.Consecutivo = deterioro.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Envia para aprobacion un deterioro
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="actividadFlujoID">Actividad actual</param>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Deterioro_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl _docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (_docCtrl != null)
                {
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), _docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }

                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #region Asigna el nuevo flujo
                    result = this.AsignarFlujo(documentID, _docCtrl.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion

                    if (createDoc)
                    {
                        try
                        {                           
                            //Asigna las alarmas
                            DTO_Alarma alarma = this.GetFirstMailInfo(_docCtrl.NumeroDoc.Value.Value, createDoc);
                            return alarma;
                        }
                        catch (Exception)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                            return result;
                        }
                    }
                    else
                        batchProgress[tupProgress] = 100;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_SendToAprobCompr;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Deterioro_SendToAprob");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Aprueba o rechaza un Deterioro
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="deterioro">lista de inventarios de bodega</param>
        ///  <param name="actFlujoID">Actividad reciente</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DTO</returns>
        public List<DTO_SerializedObject> Deterioro_AprobarRechazar(int documentID, List<DTO_inDeterioroAprob> deterioro, string actFlujoID, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100 / 2;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var invAprob in deterioro)
                {
                    porcTemp = (porcParte * i) / deterioro.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (invAprob.Aprobado.Value.Value)
                    {
                        try
                        {
                            this.Transaccion_Aprobar(documentID, invAprob.NumeroDoc.Value.Value,actFlujoID, true, batchProgress, false, false);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Deterioro_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_AprobarDoc + "&&" + invAprob.PeriodoID.Value.ToString() + "&&" + invAprob.PrefijoID.Value.ToString() + "&&" + invAprob.DocumentoNro.Value.ToString();
                            result.Details.Add(rd);
                        }
                    }
                    else if (invAprob.Rechazado.Value.Value)
                    {
                        try
                        {
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, invAprob.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, invAprob.Observacion.Value, true);
                            this.AsignarFlujo(documentID, invAprob.NumeroDoc.Value.Value, actFlujoID, true, invAprob.Observacion.Value);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Deterioro_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_RechazarDoc + "&&" + invAprob.PeriodoID.Value.ToString() + "&&" + invAprob.PrefijoID.Value.ToString() + "&&" + invAprob.DocumentoNro.Value.ToString();
                            result.Details.Add(rd);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(invAprob.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Deterioro_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        #endregion

        #region Orden Salida

        #region inOrdenSalidaDocu

        /// <summary>
        /// Adiciona en la tabla inOrdenSalidaDocu 
        /// </summary>
        /// <param name="documentoID">Documento asociado</param>
        /// <param name="header">dto a ingresar</param>
        /// <returns>Resultado</returns>
        private DTO_TxResult inOrdenSalidaDocu_Add(int documentoID, DTO_inOrdenSalidaDocu header)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_inOrdenSalidaDocu = (DAL_inOrdenSalidaDocu)base.GetInstance(typeof(DAL_inOrdenSalidaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inOrdenSalidaDocu.DAL_inOrdenSalidaDocu_Add(header);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, header.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inOrdenSalidaDocu_Add");
                return result;
            }
        }

        #endregion

        #region inOrdenSalidaDeta

        /// <summary>
        /// Guarda la lista de prOrdenCompraCotiza en base de datos
        /// </summary>
        /// <param name="salidas">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        private DTO_TxResult inOrdenSalidaDeta_AddItem(List<DTO_inOrdenSalidaDeta> salidas)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_inOrdenSalidaDeta = (DAL_inOrdenSalidaDeta)base.GetInstance(typeof(DAL_inOrdenSalidaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var salida in salidas)
                    this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_Add(salida);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message.ToString();
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inOrdenSalidaDeta_AddItem");
                return result;
            }
        }

        /// <summary>
        /// Actualiza la lista de inOrdenSalida en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_inOrdenSalidaDeta</param>
        /// <returns></returns>
        private DTO_TxResult inOrdenSalidaDeta_Upd(List<DTO_inOrdenSalidaDeta> salidas, int numeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_inOrdenSalidaDeta = (DAL_inOrdenSalidaDeta)base.GetInstance(typeof(DAL_inOrdenSalidaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var ord in salidas)
                {
                    DTO_inOrdenSalidaDeta exist = this.inOrdenSalidaDeta_GetByConsecutivo(ord.Consecutivo.Value);
                    if(exist != null)
                        this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_Upd(ord);
                    else
                    {
                        ord.NumeroDoc.Value = numeroDoc;
                        this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_Add(ord);
                    }
                      
                }
                

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inOrdenSalidaDeta_Upd");
                return result;
            }
        }

        /// <summary>
        ///Obtiene una lista de Orden SalidaDeta
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <returns> lista de DTO_inOrdenSalidaDeta</returns>
        private List<DTO_inOrdenSalidaDeta> inOrdenSalidaDeta_GetByNumeroDoc(int documentID, int numeroDoc)
        {
            try
            {
                List<DTO_inOrdenSalidaDeta> listOrdenSalidaDeta = new List<DTO_inOrdenSalidaDeta>();
                this._dal_inOrdenSalidaDeta = (DAL_inOrdenSalidaDeta)base.GetInstance(typeof(DAL_inOrdenSalidaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                listOrdenSalidaDeta = this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_GetByNumeroDoc(numeroDoc);

                return listOrdenSalidaDeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inOrdenSalidaDeta_GetByNumeroDoc");
                return new List<DTO_inOrdenSalidaDeta>();
            }
        }

        /// <summary>
        ///Obtiene una lista de Orden SalidaDeta
        /// </summary>
        /// <param name="documentID">Documento Asociado</param>
        /// <param name="numeroDoc">numero Doc inv</param>
        /// <returns> lista de DTO_inOrdenSalidaDeta</returns>
        private DTO_inOrdenSalidaDeta inOrdenSalidaDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_inOrdenSalidaDeta ordenDeta = new DTO_inOrdenSalidaDeta();
                this._dal_inOrdenSalidaDeta = (DAL_inOrdenSalidaDeta)base.GetInstance(typeof(DAL_inOrdenSalidaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                ordenDeta = this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_GetByConsecutivo(consec);

                return ordenDeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inOrdenSalidaDeta_GetByConsecutivo");
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Guardar la transaccion en las tablas relacionadas
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="ordenSalida">Dto de la Orden Salida</param>
        /// <param name="update">Indica si se actualiza o no</param>
        /// <param name="numeroDoc">Numero Doc de la transaccion</param>
        /// <returns>resultado de la transaccion</returns>
        public DTO_SerializedObject OrdenSalida_Add(int documentID, DTO_OrdenSalida ordenSalida, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail resultGLDC;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                
                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    ordenSalida.DocCtrl.DocumentoNro.Value = 0;
                    ordenSalida.DocCtrl.ComprobanteIDNro.Value = 0;
                    ordenSalida.DocCtrl.Valor.Value = 0;
                    ordenSalida.DocCtrl.Iva.Value = 0;
                    ordenSalida.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ordenSalida.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                    ordenSalida.DocCtrl.NumeroDoc.Value = numeroDoc;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion                    
                    #region Guardar en inOrdenSalidaDocu
                    ordenSalida.Header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    result = this.inOrdenSalidaDocu_Add(documentID, ordenSalida.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en inOrdenSalidaDeta
                    foreach (var item in ordenSalida.Footer)
                        item.NumeroDoc.Value = ordenSalida.DocCtrl.NumeroDoc.Value;
                    result = this.inOrdenSalidaDeta_AddItem(ordenSalida.Footer);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #endregion                      
                }
                else
                {                    
                    #region Actualiza en inOrdenSalidaDeta
                    this.inOrdenSalidaDeta_Upd(ordenSalida.Footer,ordenSalida.DocCtrl.NumeroDoc.Value.Value);
                    #endregion                        
                }

                numeroDoc = ordenSalida.DocCtrl.NumeroDoc.Value.Value;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(numeroDoc, true);
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
                return result;
            }
            catch (Exception ex)
            {
                numeroDoc = update ? ordenSalida.DocCtrl.NumeroDoc.Value.Value : 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenSalida_Add");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (ordenSalida.DocCtrl.DocumentoNro.Value == 0)
                        {
                            ordenSalida.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ordenSalida.DocCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ordenSalida.DocCtrl, true, false, false);
                            alarma.Consecutivo = ordenSalida.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene una Orden de Salida
        /// </summary>
        /// <param name="bodegaID">Bodega a Filtrar</param>
        /// <returns>Una orden</returns>
        public DTO_OrdenSalida OrdenSalida_GeyByBodega(string bodegaID, int? numeroDoc)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inOrdenSalidaDocu = (DAL_inOrdenSalidaDocu)base.GetInstance(typeof(DAL_inOrdenSalidaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inOrdenSalidaDeta = (DAL_inOrdenSalidaDeta)base.GetInstance(typeof(DAL_inOrdenSalidaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_OrdenSalida orden = new DTO_OrdenSalida();
                
                //Carga inOrdenSalidaDocu
                DTO_inOrdenSalidaDocu filter = new DTO_inOrdenSalidaDocu();
                filter.BodegaID.Value = bodegaID;
                filter.NumeroDoc.Value = numeroDoc;
                DTO_inOrdenSalidaDocu header = this._dal_inOrdenSalidaDocu.DAL_inOrdenSalidaDocu_GetByParameter(filter).Last();
                orden.Header = header;

                //Si no existe devuelve null
                if (header == null)
                    return null;

                //Trae glDocumentoControl
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(header.NumeroDoc.Value.Value);
                orden.DocCtrl = docCtrl;

                if (docCtrl == null)
                    return null;

                //Carga inOrdenSalidaDeta
                List<DTO_inOrdenSalidaDeta> deta = this._dal_inOrdenSalidaDeta.DAL_inOrdenSalidaDeta_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                orden.Footer = deta;

                return orden;

            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenSalida_GeyByBodega");
                return null;
            }
        }

        /// <summary>
        /// Aprueba la Orden de Salida y cambia el estado
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveOrden(int documentID, DTO_OrdenSalida orden, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, orden.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);
                //this.AsignarFlujo(documentID, orden.DocCtrl.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    try
                    {
                        ////Trae la info de la alarma
                        //DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                        //return alarma;
                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    }
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenSalida_GeyByBodega");
                return result;
            }
        }

        /// <summary>
        /// Genera un  mvto de salida de Inventarios a partir de la ORden de Salida
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <param name="orden">Orden de salida</param>
        /// <returns>Una orden</returns>
        public DTO_SerializedObject OrdenSalida_ApproveMvtoInv(int documentID, DTO_OrdenSalida orden, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            DTO_MvtoInventarios mov = null;
            DTO_SerializedObject resultInventario = null;
            int transacNumeroDoc = 0;
            DTO_Alarma alarma = null;
            try
            {
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_inOrdenSalidaDocu = (DAL_inOrdenSalidaDocu)base.GetInstance(typeof(DAL_inOrdenSalidaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)base.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_inMovimientoFooter> mvtoDetaFinal = new List<DTO_inMovimientoFooter>();
                string param1xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                string param2xDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                string empaquexDef = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_EmpaquexDef);
                string tipoMvtoDespachoInv = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovDespachoInventario);

                #region Generar registro de Movimiento de Inv
                foreach (DTO_inOrdenSalidaDeta det in orden.Footer)
                {
                    #region Obtiene el mvto de Entrada
                    DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                    filter.BodegaID.Value = orden.Header.BodegaID.Value;
                    filter.inReferenciaID.Value = det.inReferenciaID.Value;
                    filter.DocSoporte.Value = det.ConsProyectoMvto.Value;
                    filter.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                    var mvtosDeta = this._dal_glMovimientoDeta.DAL_glMovimientoDeta_GetByParameter(filter,false,true);
                    if (mvtosDeta == null || mvtosDeta.Count == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "Inconsistencia de datos al crear el Movimiento de Inventarios";
                        return result;
                    }
                    #endregion
                    #region Carga Movimiento de salida para Inventarios
                    DTO_inMovimientoFooter movFooter = new DTO_inMovimientoFooter();
                    DTO_glMovimientoDeta movDet = mvtosDeta.First();
                    movDet.NumeroDoc.Value = 0;
                    movDet.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                    movDet.Fecha.Value = orden.DocCtrl.PeriodoDoc.Value;
                    movDet.CantidadUNI.Value = det.CantidadAPR.Value;
                    //DTO_inEmpaque empaque = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, movDet.EmpaqueInvID.Value, true, false);
                    movDet.CantidadEMP.Value = movDet.CantidadUNI.Value;//empaque != null && empaque.Cantidad.Value != 0 ? movDet.CantidadUNI.Value / empaque.Cantidad.Value : movDet.CantidadUNI.Value;     
                    movDet.Valor1LOC.Value = movDet.ValorUNI.Value * movDet.CantidadUNI.Value;
                    movDet.Valor2LOC.Value = 0;
                    movDet.Valor1EXT.Value = 0;
                    movDet.Valor2EXT.Value = 0;
                    //Iva
                    movDet.Valor3EXT.Value = 0;
                    movDet.Valor3LOC.Value = 0;
                    movDet.Consecutivo.Value = 0;

                    movFooter.Movimiento = movDet;
                    mvtoDetaFinal.Add(movFooter);
                    #endregion
                }
                #endregion;

                if (mvtoDetaFinal.Count > 0)
                {
                    #region Carga Header del Mvto
                    mov = new DTO_MvtoInventarios();
                    mov.Header.EmpresaID.Value = this.Empresa.ID.Value;
                    mov.Header.MvtoTipoInvID.Value = tipoMvtoDespachoInv;
                    mov.Header.BodegaOrigenID.Value = orden.Header.BodegaID.Value;
                    mov.Header.BodegaDestinoID.Value = string.Empty;
                    mov.Header.AsesorID.Value = string.Empty;
                    mov.Header.ClienteID.Value = string.Empty;
                    mov.Header.DocumentoREL.Value = orden.DocCtrl.NumeroDoc.Value.Value;
                    mov.Header.VtoFecha.Value = orden.DocCtrl.Fecha.Value.Value;
                    mov.Header.NumeroDoc.Value = 0;
                    #endregion
                    #region Carga DocControl Mvto
                    mov.DocCtrl.TerceroID.Value = orden.DocCtrl.TerceroID.Value;
                    mov.DocCtrl.NumeroDoc.Value = 0;
                    mov.DocCtrl.MonedaID.Value = orden.DocCtrl.MonedaID.Value;
                    mov.DocCtrl.ProyectoID.Value = orden.DocCtrl.ProyectoID.Value;
                    mov.DocCtrl.CentroCostoID.Value = orden.DocCtrl.CentroCostoID.Value;
                    mov.DocCtrl.PrefijoID.Value = orden.DocCtrl.PrefijoID.Value;
                    mov.DocCtrl.Fecha.Value = DateTime.Now;
                    mov.DocCtrl.PeriodoDoc.Value = orden.DocCtrl.PeriodoDoc.Value;
                    mov.DocCtrl.TasaCambioCONT.Value = orden.DocCtrl.TasaCambioCONT.Value;
                    mov.DocCtrl.TasaCambioDOCU.Value = orden.DocCtrl.TasaCambioDOCU.Value;
                    mov.DocCtrl.DocumentoID.Value = AppDocuments.TransaccionAutomatica;
                    mov.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    mov.DocCtrl.PeriodoUltMov.Value = orden.DocCtrl.PeriodoUltMov.Value;
                    mov.DocCtrl.seUsuarioID.Value = orden.DocCtrl.seUsuarioID.Value;
                    mov.DocCtrl.AreaFuncionalID.Value = orden.DocCtrl.AreaFuncionalID.Value;
                    mov.DocCtrl.ConsSaldo.Value = 0;
                    mov.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    mov.DocCtrl.FechaDoc.Value = orden.DocCtrl.FechaDoc.Value;
                    mov.DocCtrl.Descripcion.Value = "Transaccion Automatica Inv(Despacho)";
                    mov.DocCtrl.DocumentoPadre.Value = orden.DocCtrl.NumeroDoc.Value;
                    if (mov.DocCtrl.TasaCambioDOCU.Value != 0)
                    {
                        foreach (var item in mvtoDetaFinal)
                        {
                            item.Movimiento.Valor1EXT.Value = Math.Round(item.Movimiento.Valor1LOC.Value.Value / mov.DocCtrl.TasaCambioDOCU.Value.Value, 2);
                            item.Movimiento.Valor2EXT.Value = Math.Round(item.Movimiento.Valor2LOC.Value.Value / mov.DocCtrl.TasaCambioDOCU.Value.Value, 2);
                        }
                    }
                    mov.DocCtrl.Valor.Value = orden.DocCtrl.Valor.Value;
                    mov.DocCtrl.Iva.Value = orden.DocCtrl.Iva.Value;
                    mov.Footer = mvtoDetaFinal;
                    #endregion
                    #region Guarda el  Mvto de Inventarios
                    resultInventario = this.Transaccion_Add(AppDocuments.TransaccionAutomatica, mov, false, out transacNumeroDoc, batchProgress, true, true);
                    if (resultInventario.GetType() == result.GetType())
                    {
                        DTO_TxResult res = (DTO_TxResult)resultInventario;
                        if (res.Result == ResultValue.NOK)
                        {
                            result = res;
                            return result;
                        }
                    }
                    #endregion
                    #region Actualiza la Orden de Salida con el Mvto de Inventarios(inOrdenSalidaDocu)
                    orden.Header.DocSalidaINV.Value = transacNumeroDoc;
                    this._dal_inOrdenSalidaDocu.DAL_inOrdenSalidaDocu_Upd(orden.Header);
                    #endregion
                    #region Actualiza las Cantidades en los movimientos del Proyecto(pyProyectoMvto)
                    foreach (var ord in orden.Footer)
                    {
                        DTO_pyProyectoMvto mvtoProy = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(ord.ConsProyectoMvto.Value);
                        mvtoProy.CantidadBOD.Value = mvtoProy.CantidadBOD.Value + ord.CantidadAPR.Value;
                        this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoProy);
                    }
                    #endregion
                }
                else
                    result.Result = ResultValue.NOK;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(transacNumeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    return alarma;
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {               
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenSalida_ApproveMvtoInv");
                return result;
            }
            finally
            {
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        DTO_glDocumentoControl docCtrlMovInventario = null;
                        #region Genera Consecutivos - Documento de inventarios(56)
                        if (resultInventario != null)
                        {
                            docCtrlMovInventario = this._moduloGlobal.glDocumentoControl_GetByID(transacNumeroDoc);
                            docCtrlMovInventario.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.TransaccionAutomatica, docCtrlMovInventario.PrefijoID.Value);
                            //Comprobante 
                            DTO_coComprobante comproInvent = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docCtrlMovInventario.ComprobanteID.Value, true, false);
                            if (comproInvent != null)
                                docCtrlMovInventario.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comproInvent, docCtrlMovInventario.PrefijoID.Value, docCtrlMovInventario.PeriodoDoc.Value.Value, docCtrlMovInventario.DocumentoNro.Value.Value);
                            else
                                docCtrlMovInventario.ComprobanteIDNro.Value = 0;
                            this._moduloGlobal.ActualizaConsecutivos(docCtrlMovInventario, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(docCtrlMovInventario.NumeroDoc.Value.Value, docCtrlMovInventario.ComprobanteIDNro.Value.Value, false);
                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        } 

        #endregion

        #region Reversion Documentos

        /// <summary>
        /// Revierte una Documento de Proveedores
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Inventarios_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls, ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();            
            DTO_coComprobante coComprob = new DTO_coComprobante();
            if (!consecutivoPos.HasValue)
            {
                ctrls = new List<DTO_glDocumentoControl>();
                coComps = new List<DTO_coComprobante>();
            }

            #endregion

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glMovimientoDeta = (DAL_glMovimientoDeta)base.GetInstance(typeof(DAL_glMovimientoDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)base.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //string mvtoEntrada = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovEntradaComprasLoc);
                //string mvtoSalida = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovSalidaDevolComprasLoc);
                //string mvtoVentas = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovVentasLoc);

                //if (string.IsNullOrEmpty(mvtoEntrada))
                //{
                //    result.Result = ResultValue.NOK;
                //    result.ResultMessage = "No Existe el tipo de movimiento de Entrada parametrizado.  ";
                //    numeroDoc = 0;
                //    return result;
                //}
                //if (string.IsNullOrEmpty(mvtoSalida))
                //{
                //    result.Result = ResultValue.NOK;
                //    result.ResultMessage = "No Existe el tipo de movimiento de Salida parametrizado";
                //    numeroDoc = 0;
                //    return result;
                //}

                #region Trae detalle del Documento con un filtro
                DTO_MvtoInventarios mvtoRevertido = this.Transaccion_Get(numeroDoc);
                #endregion                
                #region Revierte el documento y crea el Doc de Reversion
                result = this._moduloGlobal.glDocumentoControl_Revertir(mvtoRevertido.DocCtrl.DocumentoID.Value.Value, numeroDoc, consecutivoPos, ref ctrls, ref coComps, true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion 
                #region Crea la transaccion de reversion               
                if (ctrls.Count > 0)
                {
                    int numDocMvtoRever = ctrls.First().NumeroDoc.Value.Value;
                    mvtoRevertido.DocCtrl = ctrls.First();
                    mvtoRevertido.DocCtrl.DocumentoNro.Value = 0;
                    mvtoRevertido.DocCtrl.ComprobanteIDNro.Value = 0;
                    #region Guardar en inMovimientoDocu
                    DTO_inMovimientoTipo mvtoTipo = (DTO_inMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, mvtoRevertido.Header.MvtoTipoInvID.Value, true, false);
                    mvtoRevertido.Header.MvtoTipoInvID.Value = mvtoTipo.MvtoTipoReversion.Value;
                    if (string.IsNullOrEmpty(mvtoRevertido.Header.MvtoTipoInvID.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "No Existe el tipo de movimiento de Reversion para el Tipo Mvto  " + mvtoTipo.ID.Value;
                        numeroDoc = 0;
                        return result;
                    }
                    mvtoRevertido.Header.NumeroDoc.Value = numDocMvtoRever;
                    result = this.inMovimientoDocu_Add(mvtoRevertido.DocCtrl.DocumentoID.Value.Value, mvtoRevertido.Header);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #endregion;                    
                    #region Guardar en glMovimientoDeta
                    List<DTO_glMovimientoDeta> movDeta = new List<DTO_glMovimientoDeta>();
                    foreach (var d in mvtoRevertido.Footer)
                    {
                        d.Movimiento.Consecutivo.Value = null;
                        d.Movimiento.NumeroDoc.Value = numDocMvtoRever;
                        d.Movimiento.EntradaSalida.Value = d.Movimiento.EntradaSalida.Value == 1?(byte)2 : (byte)1;
                        d.Movimiento.MvtoTipoInvID.Value = mvtoRevertido.Header.MvtoTipoInvID.Value;
                        movDeta.Add(d.Movimiento);
                    }
                    result = this._moduloGlobal.glMovimientoDeta_Add(movDeta, true, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #endregion;   
                    #region PROCESA MOVIMIENTOS DE INVENTARIOS
                    result = this.Transaccion_Aprobar(mvtoRevertido.DocCtrl.DocumentoID.Value.Value, numDocMvtoRever, string.Empty, true, DictionaryProgress.BatchProgress, true, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    #endregion 
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Inventarios_Revertir");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Commit y consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        for (int i = 0; i < ctrls.Count; ++i)
                        {
                            DTO_glDocumentoControl ctrlAnula = ctrls[i];
                            DTO_coComprobante coCompAnula = coComps[i];

                            //Obtiene el consecutivo del comprobante (cuando existe)
                            ctrlAnula.DocumentoNro.Value = this.GenerarDocumentoNro(ctrlAnula.DocumentoID.Value.Value, ctrlAnula.PrefijoID.Value);
                            if (coCompAnula != null)
                                ctrlAnula.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAnula, ctrlAnula.PrefijoID.Value, ctrlAnula.PeriodoDoc.Value.Value, ctrlAnula.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(ctrlAnula, true, coCompAnula != null, false);

                            if (coCompAnula != null)
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrlAnula.NumeroDoc.Value.Value, ctrlAnula.ComprobanteIDNro.Value.Value, false);
                        }

                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        /// <param name="documentoID">Documento Asosiado</param>
        ///<param name="_listFisicoInventario">Lista de items a mostrar</param>
        ///<param name="tipoReporte">Tipo de Reporte</param>
        ///<returns>Resultado</returns>
        public DTO_TxResult GenerarReporteInvFisico(int documentoID, List<DTO_inFisicoInventario> _listFisicoInventario, InventarioFisicoReportType tipoReporte)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.ResultMessage = "OK";
            try
            {
                this.GenerarArchivo(documentoID, _listFisicoInventario.FirstOrDefault().NumeroDoc.Value.Value, DtoReportInventarioFisico(_listFisicoInventario, tipoReporte, true));
            }
            catch (Exception)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.Err_ReportCreate;
            }
            return result;
        }

        /// <summary>
        ///Crea un dto de reporte 
        ///</summary>
        ///<param name="_listFisicoInventario">Lista de items a mostrar</param>
        ///<param name="tipoReporte">Tipo de Reporte</param>
        ///<returns>Resultado</returns>
        internal object DtoReportInventarioFisico(List<DTO_inFisicoInventario> _listFisicoInventario, InventarioFisicoReportType tipoReoprte, bool isApro)
        {
            try
            {
                #region Variables
                string bodegaID = null;
                bodegaID = _listFisicoInventario.FirstOrDefault().BodegaID.Value;
                DTO_inBodega dto_bodega = (DTO_inBodega)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaID, true, false);
                DTO_ReportInvFisico dto_ReportFisico = new DTO_ReportInvFisico();
                #endregion
                #region Asignar los datos para el reporte

                switch (tipoReoprte)
                {
                    case InventarioFisicoReportType.Fisico:
                        dto_ReportFisico.TipoReport = InventarioFisicoReportType.Fisico;
                        #region Header
                        dto_ReportFisico.Header.Bodega = dto_bodega.IdName;
                        #endregion
                        #region Detail
                        foreach (var dtoInvFisicoDetail in _listFisicoInventario)
                        {
                            DTO_InvFisicoDetail detail = new DTO_InvFisicoDetail();
                            detail.Codigo = dtoInvFisicoDetail.ReferenciaP1P2ID.Value;
                            detail.Descripcion = dtoInvFisicoDetail.ReferenciaP1P2Desc.Value;
                            detail.Kardex = dtoInvFisicoDetail.CantKardex.Value.Value;
                            detail.Fisico = dtoInvFisicoDetail.CantFisico.Value.Value;
                            detail.Diferencia = dtoInvFisicoDetail.CantAjuste.Value.Value;
                            detail.UnidadLoc = dtoInvFisicoDetail.CantAjuste.Value != 0? dtoInvFisicoDetail.CostoLocal.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalLoc = dtoInvFisicoDetail.CostoLocal.Value.Value;
                            detail.UnidadExt = dtoInvFisicoDetail.CantAjuste.Value!= 0? dtoInvFisicoDetail.CostoExtra.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalExt = dtoInvFisicoDetail.CostoExtra.Value.Value;
                            dto_ReportFisico.Detail.Add(detail);
                        }
                        #endregion
                        break;
                    case InventarioFisicoReportType.Conteo:
                        dto_ReportFisico.TipoReport = InventarioFisicoReportType.Conteo;
                        #region Header
                        dto_ReportFisico.Header.Bodega = dto_bodega.IdName;
                        dto_ReportFisico.Header.Fecha = _listFisicoInventario.FirstOrDefault().Periodo.Value.Value;
                        #endregion
                        #region Detail
                        foreach (var dtoInvFisicoDetail in _listFisicoInventario)
                        {
                            DTO_InvFisicoDetail detail = new DTO_InvFisicoDetail();
                            detail.Codigo = dtoInvFisicoDetail.ReferenciaP1P2ID.Value;
                            detail.Descripcion = dtoInvFisicoDetail.ReferenciaP1P2Desc.Value;
                            detail.Kardex = dtoInvFisicoDetail.CantKardex.Value.Value;
                            detail.Fisico = dtoInvFisicoDetail.CantFisico.Value.Value;
                            detail.Diferencia = dtoInvFisicoDetail.CantAjuste.Value.Value;
                            detail.UnidadLoc = dtoInvFisicoDetail.CantAjuste.Value != 0 ? dtoInvFisicoDetail.CostoLocal.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalLoc = dtoInvFisicoDetail.CostoLocal.Value.Value;
                            detail.UnidadExt = dtoInvFisicoDetail.CantAjuste.Value != 0 ? dtoInvFisicoDetail.CostoExtra.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalExt = dtoInvFisicoDetail.CostoExtra.Value.Value;

                            dto_ReportFisico.Detail.Add(detail);
                        }
                        #endregion
                        break;
                    case InventarioFisicoReportType.Diferencia:
                        dto_ReportFisico.TipoReport = InventarioFisicoReportType.Diferencia;
                        #region Header
                        dto_ReportFisico.Header.Bodega = dto_bodega.IdName;
                        dto_ReportFisico.Header.Fecha = _listFisicoInventario.FirstOrDefault().Periodo.Value.Value;
                        #endregion
                        #region Detail
                        foreach (var dtoInvFisicoDetail in _listFisicoInventario)
                        {
                            DTO_InvFisicoDetail detail = new DTO_InvFisicoDetail();
                            detail.Codigo = dtoInvFisicoDetail.ReferenciaP1P2ID.Value;
                            detail.Descripcion = dtoInvFisicoDetail.ReferenciaP1P2Desc.Value;
                            detail.Kardex = dtoInvFisicoDetail.CantKardex.Value.Value;
                            detail.Fisico = dtoInvFisicoDetail.CantFisico.Value.Value;
                            detail.Diferencia = dtoInvFisicoDetail.CantAjuste.Value.Value;
                            detail.UnidadLoc = dtoInvFisicoDetail.CantAjuste.Value != 0 ? dtoInvFisicoDetail.CostoLocal.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalLoc = dtoInvFisicoDetail.CostoLocal.Value.Value;
                            detail.UnidadExt = dtoInvFisicoDetail.CantAjuste.Value != 0 ? dtoInvFisicoDetail.CostoExtra.Value.Value / dtoInvFisicoDetail.CantAjuste.Value.Value : 0;
                            detail.TotalExt = dtoInvFisicoDetail.CostoExtra.Value.Value;

                            dto_ReportFisico.Detail.Add(detail);
                        }
                        #endregion
                        break;
                }
                #endregion
                return dto_ReportFisico;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportInventarioFisico");
                return null;
            }
        }

        #region Saldos y Kardex

        /// <summary>
        /// Funcion que carga el DTO De saldos de Inventarios con parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns>Retorna los campos de la consulta</returns>
        /// Esta funcion sirve para el reporte de saldos y Kardex de inventarios
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_Saldos(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro = null, string tipoReporte=" ")
        {

            DTO_ReportInventariosSaldosTotal inven = new DTO_ReportInventariosSaldosTotal();
            List<DTO_ReportInventariosSaldosTotal> inventario = new List<DTO_ReportInventariosSaldosTotal>();
            inven.Detalles = new List<DTO_ReportInventariosSaldos>();

            this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            inven.Detalles = _dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro,tipoReporte);
            List<string> distinct = (from c in inven.Detalles select c.BodegaID.Value).Distinct().ToList();

            foreach (string item in distinct)
            {
                DTO_ReportInventariosSaldosTotal invetarios = new DTO_ReportInventariosSaldosTotal();
                invetarios.Detalles = new List<DTO_ReportInventariosSaldos>();

                invetarios.Detalles = inven.Detalles.Where(x => x.BodegaID.Value == item).ToList();
                inventario.Add(invetarios);
            }

            return inventario;
        }

        /// <summary>
        /// Funcion que carga el DTO De saldos de Inventarios sin Parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns>Retorna los campos de la consulta</returns>
        /// Esta funcion sirve para el reporte de saldos y Kardex de inventarios
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_SaldosSinParametros(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro = null, string tipoReporte = " ")
        {
            try
            {

                DTO_ReportInventariosSaldosTotal inven = new DTO_ReportInventariosSaldosTotal();
                List<DTO_ReportInventariosSaldosTotal> inventario = new List<DTO_ReportInventariosSaldosTotal>();
                inven.Detalles = new List<DTO_ReportInventariosSaldos>();

                this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                inven.Detalles = _dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie,
                    material, isSerial, Libro, tipoReporte);
                List<string> distinct = (from c in inven.Detalles select c.BodegaID.Value).Distinct().ToList();

                foreach (string item in distinct)
                {
                    List<DTO_ReportInventariosSaldos> sinParametros = new List<DTO_ReportInventariosSaldos>();
                    DTO_ReportInventariosSaldosTotal invetarios = new DTO_ReportInventariosSaldosTotal();

                    foreach (var item1 in inven.Detalles.Where(x => x.BodegaID.Value == item).ToList())
                    {
                        if (!sinParametros.Any(x => x.Referencia.Value == item1.Referencia.Value))
                            sinParametros.Add(item1);
                        else
                        {
                            DTO_ReportInventariosSaldos dto = new DTO_ReportInventariosSaldos();
                            dto = sinParametros.Where(x => x.Referencia.Value == item1.Referencia.Value).FirstOrDefault();
                            dto.VlrUnidadLoc.Value += item1.VlrUnidadLoc.Value;
                            dto.VlrUnidadExt.Value += item1.ValorExt.Value;
                            dto.ValorLocal.Value += item1.ValorLocal.Value;
                            dto.ValorExt.Value += item1.ValorExt.Value;
                            dto.CantidadLoc.Value += item1.CantidadLoc.Value;
                            dto.Inicial.Value += item1.Inicial.Value;
                            dto.Entrada.Value += item1.Entrada.Value;
                            dto.Salidas.Value += item1.Salidas.Value;

                            sinParametros.Remove(dto);
                            sinParametros.Add(dto);
                        }
                    }
                    invetarios.Detalles = sinParametros;
                    inventario.Add(invetarios);
                }

                return inventario;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesInventarios_SaldosSinParametros");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que carga el DTO De saldos de Inventarios con Parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns>Retorna los campos de la consulta</returns>
        /// Esta funcion sirve para el reporte de saldos y Kardex de inventarios
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_SaldosxReferencia(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro = null, string tipoReporte = " ")
        {

            DTO_ReportInventariosSaldosTotal inven = new DTO_ReportInventariosSaldosTotal();
            List<DTO_ReportInventariosSaldosTotal> inventario = new List<DTO_ReportInventariosSaldosTotal>();
            inven.Detalles = new List<DTO_ReportInventariosSaldos>();

            this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            inven.Detalles = _dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro,tipoReporte);
            List<string> distinct = (from c in inven.Detalles select c.Referencia.Value).Distinct().ToList();

            foreach (string item in distinct)
            {
                DTO_ReportInventariosSaldosTotal invetarios = new DTO_ReportInventariosSaldosTotal();
                invetarios.Detalles = new List<DTO_ReportInventariosSaldos>();

                invetarios.Detalles = inven.Detalles.Where(x => x.Referencia.Value == item).ToList();
                inventario.Add(invetarios);
            }

            return inventario;
        }

        /// <summary>
        /// Funcion que carga el DTO De saldos de Inventarios con sin  parametros
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes a consultar</param>
        /// <param name="bodega"> Bodega que se quiere revisar</param>
        /// Si se filtra por bodega se filtra por  puede filtrar por el tipo de bodega
        /// <param name="tipoBodega">Tipo de Bodega</param>
        /// <param name="referencia">Refencia del Producto </param>
        /// si se filtra por Referencia se puede especificar que tipo de producto se busca
        /// <param name="grupo">Grupo al que pertenece el producto</param>
        /// <param name="clase">Clase de producto</param>
        /// <param name="tipo">Tipo de producto</param>
        /// <param name="serie">Seria de producto</param>
        /// <param name="material">Material del producto producto</param>
        /// <returns>Retorna los campos de la consulta</returns>
        /// Esta funcion sirve para el reporte de saldos y Kardex de inventarios
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_SaldosxReferenciaSinParametros(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro = null, string tipoReporte = " ")
        {

            DTO_ReportInventariosSaldosTotal inven = new DTO_ReportInventariosSaldosTotal();
            List<DTO_ReportInventariosSaldosTotal> inventario = new List<DTO_ReportInventariosSaldosTotal>();
            inven.Detalles = new List<DTO_ReportInventariosSaldos>();

            this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            inven.Detalles = _dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, Libro,tipoReporte);
            List<string> distinct = (from c in inven.Detalles select c.Referencia.Value).Distinct().ToList();

            foreach (string item in distinct)
            {
                List<DTO_ReportInventariosSaldos> sinParametros = new List<DTO_ReportInventariosSaldos>();
                DTO_ReportInventariosSaldosTotal invetarios = new DTO_ReportInventariosSaldosTotal();

                foreach (var item1 in inven.Detalles.Where(x => x.Referencia.Value == item).ToList())
                {
                    if (!sinParametros.Any(x => x.BodegaID.Value == item1.BodegaID.Value))
                        sinParametros.Add(item1);
                    else 
                    {
                        DTO_ReportInventariosSaldos dto = new DTO_ReportInventariosSaldos();
                        dto = sinParametros.Where(x => x.BodegaID.Value == item1.BodegaID.Value).FirstOrDefault();
                        dto.VlrUnidadLoc.Value += item1.VlrUnidadLoc.Value;
                        dto.VlrUnidadExt.Value += item1.ValorExt.Value;
                        dto.ValorLocal.Value += item1.ValorLocal.Value;
                        dto.ValorExt.Value += item1.ValorExt.Value;
                        dto.CantidadLoc.Value += item1.CantidadLoc.Value;
                        dto.Inicial.Value += item1.Inicial.Value;
                        dto.Entrada.Value += item1.Entrada.Value;
                        dto.Salidas.Value += item1.Salidas.Value;

                        sinParametros.Remove(dto);
                        sinParametros.Add(dto);
                    }
                }
                invetarios.Detalles = sinParametros;
                inventario.Add(invetarios);
            }

            return inventario;
        } 

        #endregion

        #region Serial

        /// <summary>
        /// Funcion que genera el reporte de Serial sin Costo desde el cliente
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <returns></returns>
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_SerialSinCostos(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial,string tipoReporte)
        {
            DTO_ReportInventariosSaldosTotal saldosSerial = new DTO_ReportInventariosSaldosTotal();
            List<DTO_ReportInventariosSaldosTotal> serial = new List<DTO_ReportInventariosSaldosTotal>();
            Dictionary<string, string> detalle = new Dictionary<string, string>();

            this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            saldosSerial.Detalles = this._dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo,
                serie, material, isSerial,null,tipoReporte);

            foreach (var item in saldosSerial.Detalles )
            {
                if(!detalle.Contains( new KeyValuePair<string,string>(item.BodegaID.Value + "-" + item.Referencia.Value, item.Referencia.Value)))
                    detalle.Add(item.BodegaID.Value + "-" + item.Referencia.Value, item.Referencia.Value);
            }

            foreach (var item in detalle)
            {
                try
                {
                    List<DTO_ReportInventariosSaldos> saldosFilter = new List<DTO_ReportInventariosSaldos>();
                    DTO_ReportInventariosSaldosTotal sumSaldos = new DTO_ReportInventariosSaldosTotal();

                    string Bodega, Referencias;

                    Bodega = item.Key.Split('-')[0].ToString();
                    Referencias = item.Key.Split('-')[1].ToString();

                    foreach (var item2 in saldosSerial.Detalles.Where( x=> x.BodegaID.Value == Bodega && x.Referencia.Value == Referencias).ToList())
                    {
                        if (!saldosFilter.Any(x => x.BodegaID.Value == item2.BodegaID.Value && x.Referencia.Value == item2.Referencia.Value))
                        {
                            item2.Serial = "Serial:" + " " + item2.Serial;
                            saldosFilter.Add(item2);
                        }
                        else
                        {
                            DTO_ReportInventariosSaldos dto = new DTO_ReportInventariosSaldos();
                            dto = saldosFilter.Where(x => x.BodegaID.Value == item2.BodegaID.Value && x.Referencia.Value == Referencias).FirstOrDefault();
                            dto.Serial = dto.Serial.Trim() + "," + " " + " " + "Serial:" + " "  + item2.Serial.Trim();

                            saldosFilter.Remove(dto);
                            saldosFilter.Add(dto);
                        }
                    }
                    sumSaldos.Detalles = saldosFilter;
                    serial.Add(sumSaldos);                
                }
                catch (Exception ex)
                {
                     Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesInventarios_Serial");
                    throw ex;
                }
            }
            return serial;
 
        }

        /// <summary>
        /// Funcion que genera el reporte de Serial con Costo desde el cliente
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesIni">Mes por el cual se va a filtrar</param>
        /// <param name="bodega">Bodega por la cual se desea filtrar</param>
        /// <param name="tipoBodega">Tipo de Bodega por e l cual se desea filtrar</param>
        /// <param name="referencia">Referencia por el cual se desea filtrar</param>
        /// <param name="grupo">Grupo por el cual se desea filtrar</param>
        /// <param name="clase">Clase por el cual se desea filtrar</param>
        /// <param name="tipo">Tipo por el cual se desea filtrar</param>
        /// <param name="serie">Serie por el cual se desea filtrar</param>
        /// <param name="material">Material por el cual se desea filtrar</param>
        /// <param name="isSerial">Parametro para imprimir reporte de Serial</param>
        /// <returns></returns>
        public List<DTO_ReportInventariosSaldosTotal> ReportesInventarios_SerialConCostos(int año, int mesIni, string bodega, string tipoBodega,
                string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string tipoReporte)
        {
            DTO_ReportInventariosSaldosTotal saldosSerial = new DTO_ReportInventariosSaldosTotal();
            List<DTO_ReportInventariosSaldosTotal> serial = new List<DTO_ReportInventariosSaldosTotal>();
            Dictionary<string, string> detalle = new Dictionary<string, string>();

            this._dal_ReportesInventarios = (DAL_ReportesInventarios)base.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            saldosSerial.Detalles = this._dal_ReportesInventarios.DAL_ReportesInventarios_Saldos(año, mesIni, bodega, tipoBodega, referencia, grupo, clase, tipo,
                serie, material, isSerial,null,tipoReporte);

            foreach (var item in saldosSerial.Detalles)
            {
                if (!detalle.Contains(new KeyValuePair<string, string>(item.BodegaID.Value + "-" + item.Referencia.Value, item.Referencia.Value)))
                    detalle.Add(item.BodegaID.Value + "-" + item.Referencia.Value, item.Referencia.Value);
            }

            foreach (var item in detalle)
            {
                try
                {
                    DTO_ReportInventariosSaldosTotal serialTotal = new DTO_ReportInventariosSaldosTotal();
                    serialTotal.Detalles = new List<DTO_ReportInventariosSaldos>();

                    string Bodega, Referencias;

                    Bodega = item.Key.Split('-')[0].ToString();
                    Referencias = item.Key.Split('-')[1].ToString();

                    serialTotal.Detalles = saldosSerial.Detalles.Where(x => x.BodegaID.Value == Bodega && x.Referencia.Value == Referencias).ToList();

                    serial.Add(serialTotal);
                }

                catch (Exception ex)
                {
                    Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesInventarios_SerialConCostos");
                    throw ex;
                }
            }
            return serial;
        }

        #endregion

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="documentoID">Tipo de Reporte a Generar</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="mesFin">Fecha Final</param>
        /// <param name="bodega">bodega</param>
        /// <param name="tipoBodega">tipoBodega</param>
        /// <param name="grupo">Grupo</param>
        /// <param name="clase">tipoBodega</param>
        /// <param name="Tipo">Tipo</param>
        /// <param name="serie">serie</param>
        /// <param name="material">material</param>
        /// <param name="isSerial">isSerial</param>
        /// <param name="otroFilter">otroFilter</param>
        /// <param name="agrup">agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_In_InventarioToExcel(int documentID, DateTime? mesIni, DateTime? mesFin, string bodega, string tipoBodega, string referencia, string grupo, string clase, string tipo,
                                                       string serie, string material, bool isSerial, string libro, string proyecto, string mvtoTipoID, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                DataTable result;
                this._dal_ReportesInventarios = (DAL_ReportesInventarios)this.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ReportesInventarios.DAL_Reportes_In_InventarioToExcel(documentID, mesIni, mesFin, bodega, tipoBodega, referencia, grupo, clase, tipo, serie, material, isSerial, libro,proyecto,mvtoTipoID, otroFilter, agrup, romp);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_In_InventarioToExcel");
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene un datatable con la info de Inventarios segun filtros
        /// </summary>
        /// <param name="movimiento">Tipo  de movimiento</param>
        /// <param name="bodega">bodega</param>
        /// <param name="proyecto">proyecto</param>
        /// <param name="TipoReporte">report</param>
        /// <param name="mesIni">Fecha Inicial</param>
        /// <param name="detalle">detalle</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_In_DocumentoToExcel(string movimientoID, string bodegaID, string proyectoID, string tipoReporte, DateTime fechaIni, byte detalle)
        {
            try
            {
                DataTable result;
                this._dal_ReportesInventarios = (DAL_ReportesInventarios)this.GetInstance(typeof(DAL_ReportesInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ReportesInventarios.DAL_Reportes_In_DocumentoToExcel(movimientoID, bodegaID, proyectoID, tipoReporte, fechaIni, detalle);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_In_InventarioToExcel");
                throw ex;
            }
        }
        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obriene la informacion del serial
        /// </summary>
        /// <param name="serial">Filtro SerialID</param>
        /// <param name="bodegaID">Filtro de BodegaID</param>
        /// <param name="inReferenciaID">Filtro de Referencia</param>
        /// <param name="inCliente">Filtro de TerceroID</param>
        /// <returns>Lista de detalles de las consultas</returns>
        public List<DTO_inQuerySeriales> inSaldosExistencias_GetBySerial(string serial, string bodegaID, string inReferenciaID, string inCliente)
        {
            #region Variables

            List<DTO_inQuerySeriales> dataList = new List<DTO_inQuerySeriales>();
            this._dal_mvtoSaldosCostos = (DAL_MvtoSaldosCostos)this.GetInstance(typeof(DAL_MvtoSaldosCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            try
            {
                dataList = this._dal_mvtoSaldosCostos.DAL_inSaldosExistencias_GetBySerial(serial, bodegaID,inReferenciaID, inCliente);
                return dataList;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "inSaldosExistencias_GetBySerial");
                return new List<DTO_inQuerySeriales>();
            }
        }

        #endregion
    }
}
