using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;
using SentenceTransformer;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloContabilidad : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_Contabilidad _dal_Contabilidad = null;
        private DAL_Comprobante _dal_Comprobante = null;
        private DAL_Impuesto _dal_Impuesto = null;
        private DAL_ReportesContabilidad _dal_ReportContabiliadad = null;
        private DAL_coAuxiliarAjustaDeta _dal_coAuxiliarAjustaDeta = null;
        private DAL_coCierreMes _dal_coCierreMes = null;
        private DAL_coCompDistribuyeTabla _dal_coCompDistribuyeTabla = null;
        private DAL_coCompDistribuyeExcluye _dal_coCompDistribuyeExcluye = null;
        private DAL_coDocumentoAjuste _dal_coDocumentoAjuste = null;
        private DAL_coReclasificaBalance _dal_coReclasificaBalance = null;
        private DAL_coReclasificaBalExcluye _dal_coReclasificaBalExcluye = null;
        private DAL_coDocumentoRevelacion _dal_coDocumentoRevelacion = null;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloCuentasXPagar _moduloCxP = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloNomina _moduloNomina = null;
        private ModuloInventarios _moduloInventario = null;

        #endregion

        #endregion

        public ModuloContabilidad(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Cont - General y Procesos

        #region Fuciones privadas

        /// <summary>
        /// Proceso para consolidar balances entre empresas
        /// </summary>
        /// <param name="documentID">Identificador del documento que genera el proceso</param>
        /// <param name="list">Lista de empresas a consolidar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        private DTO_TxResult ConsolidarBalance(int documentID, string actividadFlujoID, DateTime periodo, DTO_ComprobanteConsolidacion empresa, DTO_coComprobante coComp, bool insideAnotherTx)
        {
            #region Variables de resultados
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_Comprobante comp = null;
            #endregion
            try
            {
                #region Variables

                //Modulos y dals
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Modulos y dals (nueva empresa)
                DTO_glEmpresa emp = (DTO_glEmpresa)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glEmpresa, empresa.EmpresaID.Value, true, false);
                DAL_Comprobante dal_ComprobanteNuevo = new DAL_Comprobante(this._mySqlConnection, this._mySqlConnectionTx, emp, this.UserId, this.loggerConnectionStr);

                //Variables de datos
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footerOld = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> footerOrigen = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                //glControl
                string prefijoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string linPresXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string LugGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string tipoBalFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string tipoBalCorp = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceCorporativo);

                //Identificador para saber de donde viene la info del proyecto
                bool proyInd = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TrasladoProyectosConsolidacion) == "1" ? true : false;
                int numDoc = 0;

                //Ctas
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta cta = null;
                #endregion
                #region Valida que no hayan datos en coAuxiliarPre
                int count = dal_ComprobanteNuevo.DAL_ComprobantePre_HasData(true, periodo);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;

                    return result;
                }
                #endregion
                #region Revisa si la empresa ya fue consolidada y elimina la info existente
                if (empresa.Procesado)
                {
                    footerOld = this._dal_Comprobante.DAL_Comprobante_GetConsolidacionAntigua(periodo, coComp.ID.Value, empresa.CentroCostoID.Value);
                    numDoc = this._dal_Comprobante.DAL_Comprobante_BorrarConsolidacionBalance(periodo, coComp.ID.Value, empresa.CentroCostoID.Value);
                }
                #endregion
                #region Carga o trae el documento

                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                if (numDoc == 0)
                {
                    #region Agregar registro a glDocumentoControl

                    //Campos Principales
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.Fecha.Value = periodo;
                    docCtrl.PeriodoDoc.Value = periodo;
                    docCtrl.PeriodoUltMov.Value = periodo;
                    docCtrl.AreaFuncionalID.Value = this._moduloGlobal.GetAreaFuncionalByUser();
                    docCtrl.PrefijoID.Value = this.GetPrefijoByDocumento(documentID);
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.MonedaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    docCtrl.TasaCambioDOCU.Value = 0;
                    docCtrl.TasaCambioCONT.Value = 0;
                    docCtrl.ComprobanteID.Value = coComp.ID.Value;
                    docCtrl.ComprobanteIDNro.Value = 0;
                    docCtrl.Observacion.Value = string.Empty;
                    docCtrl.Estado.Value = Convert.ToByte(EstadoDocControl.Aprobado);

                    DTO_TxResultDetail rd = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rd);

                        return result;
                    }

                    docCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                    #endregion
                }
                else
                {
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                    this.AsignarFlujo(documentID, numDoc, actividadFlujoID, false, string.Empty);
                }

                #endregion
                #region Actualiza la info de la empresa consolidada

                //Trae la informacion de todos los auxiliares
                footerOrigen = dal_ComprobanteNuevo.DAL_Comprobante_TraerInfoConsolidacion(periodo, tipoBalFunc, tipoBalCorp);
                if (footerOrigen == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CtaAlternaNull + "&&" + empresa.EmpresaID.Value;
                    return result;
                }

                //Revierte la info del comprobante original
                foreach (DTO_ComprobanteFooter det in footerOld)
                {
                    det.vlrMdaLoc.Value *= -1;
                    det.vlrMdaExt.Value *= -1;
                    det.vlrMdaOtr.Value *= -1;

                    footer.Add(det);
                }

                //Carga los nuevos de la empresa nueva
                foreach (DTO_ComprobanteFooter det in footerOrigen)
                {
                    List<DTO_ComprobanteFooter> listTemp = footer.Where(x => x.CuentaID.Value == det.CuentaID.Value
                                                                            && x.ProyectoID.Value == det.ProyectoID.Value
                                                                            && x.TerceroID.Value == det.TerceroID.Value).ToList();

                    if (listTemp.Count > 0)
                    {
                        listTemp.First().vlrMdaLoc.Value += det.vlrMdaLoc.Value.Value;
                        listTemp.First().vlrMdaExt.Value += det.vlrMdaExt.Value.Value;
                        listTemp.First().vlrMdaOtr.Value += det.vlrMdaOtr.Value.Value;
                    }
                    else
                    {
                        if (cacheCtas.ContainsKey(det.CuentaAlternaID.Value))
                            cta = cacheCtas[det.CuentaAlternaID.Value];
                        else
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, det.CuentaAlternaID.Value, true, false);
                            cacheCtas.Add(det.CuentaAlternaID.Value, cta);
                        }

                        DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                        detalle.CuentaID.Value = cta.ID.Value;
                        detalle.CentroCostoID.Value = empresa.CentroCostoID.Value;
                        detalle.ProyectoID.Value = proyInd ? det.ProyectoID.Value : proyectoXDef;
                        detalle.TerceroID.Value = ctrl.TerceroID.Value;

                        detalle.LineaPresupuestoID.Value = linPresXDef;
                        detalle.LugarGeograficoID.Value = LugGeoXDef;
                        detalle.PrefijoCOM.Value = prefijoXDef;
                        detalle.ConceptoCargoID.Value = concCargoXDef;
                        detalle.IdentificadorTR.Value = 0;
                        detalle.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
                        detalle.TasaCambio.Value = det.TasaCambio.Value;
                        detalle.DocumentoCOM.Value = detalle.DocumentoCOM.Value;
                        detalle.vlrMdaLoc.Value = detalle.vlrMdaLoc.Value;
                        detalle.vlrMdaExt.Value = detalle.vlrMdaExt.Value;
                        detalle.vlrMdaOtr.Value = detalle.vlrMdaOtr.Value;
                        detalle.vlrBaseML.Value = 0;
                        detalle.vlrBaseME.Value = 0;

                        footer.Add(detalle);
                    }
                }

                #endregion
                #region Crea el cabezote del comprobante

                header.ComprobanteID.Value = docCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = docCtrl.ComprobanteIDNro.Value;
                header.Fecha.Value = periodo;
                header.PeriodoID.Value = periodo;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.NumeroDoc.Value = docCtrl.NumeroDoc.Value.Value;
                header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Local;
                header.MdaTransacc.Value = docCtrl.MonedaID.Value;
                header.TasaCambioBase.Value = 0;
                header.TasaCambioOtr.Value = 0;

                #endregion
                #region Contabiliza el comprobante
                comp = new DTO_Comprobante();
                comp.Header = header;
                comp.Footer = footer;

                if (footer.Count > 0)
                    result = this.ContabilizarComprobante(documentID, comp, periodo, ModulesPrefix.co, 0, false);
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CompNoResults;
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ConsolidarBalance");
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

                        if (comp != null && !empresa.Procesado)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            comp.Header.ComprobanteNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                            this.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, comp.Header.ComprobanteNro.Value.Value, false);
                        }
                        #endregion
                    }
                    else
                        throw new Exception("ConsolidarBalance - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Verifica si se existe un documento en un periodo y un auxiliar relacionado
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna el estado del ajuste</returns>
        internal EstadoAjuste HasDocument(int documentoID, DateTime periodo, string libroID)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_HasDocument(documentoID, periodo, libroID);
        }

        /// <summary>
        /// Borra informacio de coAuxiliar
        /// </summary>
        /// <param name="periodo">periodo</param>
        /// <param name="comprobanteID">comprobanteID</param>
        /// <param name="compNro">comprobanteNro</param>
        internal void BorrarAuxiliar(DateTime periodo, string comprobanteID, int compNro)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante.DAL_Comprobante_BorrarAuxiliar(periodo, comprobanteID, compNro);
        }

        /// <summary>
        /// Borra la informacion de los saldos y del balance en un periodo
        /// </summary>
        /// <param name="isMensual">Indica si elimina informacion un mes o de todo el año</param>
        /// <param name="periodo">Periodo para aliminar los datos</param>
        /// <param name="libroPreliminar">Tipo de balance preliminar</param>
        internal void BorrarSaldosXLibro(bool isMensual, DateTime periodo, string libroPreliminar)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Contabilidad.DAL_Contabilidad_BorrarSaldosXLibro(isMensual, periodo, libroPreliminar);
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Genera los comprobantes y saldos para el ajuste en cambio
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="AreaFuncionalID">Area funcional desde ka cual se ejecuta el proceso (la del usuario)</param>
        /// <param name="ndML">Numero de documento de glDocumentoControl ML</param>
        /// <param name="ndME">Numero de documento de glDocumentoControl ME</param>
        /// <returns></returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> Proceso_AjusteEnCambio(int documentID, string actividadFlujoID, string areaFuncionalID,
            DateTime periodo, string libroID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            EstadoAjuste estadoCtrl = EstadoAjuste.Aprobado;
            DTO_coComprobante compML = null;
            DTO_coComprobante compME = null;
            DTO_glDocumentoControl docCtrlML = null;
            DTO_glDocumentoControl docCtrlME = null;
            try
            {
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region 1.Definicion Variables
                //Monedas
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                decimal tasaCierre = 0;

                //Variables de datos
                string prefix = this.GetPrefijoByDocumento(AppProcess.AjusteEnCambio);
                string comprobanteAjusteCambioML = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteAjCambioMdaLocal);
                string comprobanteAjusteCambioME = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteAjCambioMdaExtr);
                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);

                //Var x defecto
                string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string defPrefijo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string defTercero = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                //Info contrapartida
                string ctaContrapartidaMLDb = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCambioNoRealizadoLocalDeb);
                string ctaContrapartidaMLCr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCambioNoRealizadoLocalCred);
                string ctaContrapartidaMEDb = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCambioNoRealizadoExtrDeb);
                string ctaContrapartidaMECr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCambioNoRealizadoExtrCred);
                string concSaldoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);

                //Proceso
                DTO_Comprobante newComprobanteML = new DTO_Comprobante();
                DTO_Comprobante newComprobanteME = new DTO_Comprobante();
                Dictionary<string, DTO_coPlanCuenta> ctas = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, SaldoControl> ctrlSaldos = new Dictionary<string, SaldoControl>();

                //Progreso
                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100 / 5;

                #endregion
                #region 2.Asignacion tasa de cierre
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);

                DAL_MasterComplex dalcomplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalcomplex.DocumentID = AppMasters.coTasaCierre;

                DateTime periodoTasa = new DateTime(periodo.Year, periodo.Month, 1);
                Dictionary<string, string> keysTasa = new Dictionary<string, string>();
                keysTasa.Add("EmpresaGrupoID", this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTasaCierre, this.Empresa, egCtrl));
                keysTasa.Add("MonedaID", mdaExt);
                keysTasa.Add("PeriodoID", periodoTasa.ToString(FormatString.DB_Date_YYYY_MM_DD));
                DTO_coTasaCierre tasa = (DTO_coTasaCierre)dalcomplex.DAL_MasterComplex_GetByID(keysTasa, true);

                if (tasa == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_TasaCierre;
                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                }

                tasaCierre = tasa.TasaCambio.Value.Value;
                #endregion
                #region 3.Valida que no existan datos en el auxiliarPre
                int count = this._dal_Comprobante.DAL_ComprobantePre_HasData(true, periodo);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;
                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                }
                #endregion
                #region 4.Revisa si ya se hizo un ajuste previo y carga la informacion de los comprobantes
                estadoCtrl = this.HasDocument(AppDocuments.ComprobanteAjusteCambio, periodo, libroID);
                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_AjusteAprobado;
                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                }
                else if (estadoCtrl == EstadoAjuste.NoData)
                {
                    compML = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteAjusteCambioML, true, false);
                    compME = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteAjusteCambioME, true, false);
                }
                #endregion
                #region 5.Carga la informacion de los comprobantes (Header)

                #region Info general

                //Header Moneda Local
                newComprobanteML.Header.LibroID.Value = tipoBalancePreliminar;
                newComprobanteML.Header.ComprobanteID.Value = comprobanteAjusteCambioML;
                newComprobanteML.Header.PeriodoID.Value = periodo;
                newComprobanteML.Header.EmpresaID.Value = this.Empresa.ID.Value;
                newComprobanteML.Header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Local;
                newComprobanteML.Header.MdaTransacc.Value = mdaLoc;
                newComprobanteML.Header.TasaCambioBase.Value = tasaCierre;
                newComprobanteML.Header.TasaCambioOtr.Value = tasaCierre;

                //Header Moneda Ext
                newComprobanteME.Header.LibroID.Value = tipoBalancePreliminar;
                newComprobanteME.Header.ComprobanteID.Value = comprobanteAjusteCambioME;
                newComprobanteME.Header.PeriodoID.Value = periodo;
                newComprobanteME.Header.EmpresaID.Value = this.Empresa.ID.Value;
                newComprobanteME.Header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Foreign;
                newComprobanteME.Header.MdaTransacc.Value = mdaExt;
                newComprobanteME.Header.TasaCambioBase.Value = tasaCierre;
                newComprobanteME.Header.TasaCambioOtr.Value = tasaCierre;

                #endregion
                #region Parametros por estado
                if (estadoCtrl == EstadoAjuste.Preliminar)
                {
                    //Trae los documentos
                    List<DTO_glDocumentoControl> ctrls = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(AppDocuments.ComprobanteAjusteCambio, periodo);

                    //Borrar Auxiliar
                    this.BorrarAuxiliar(periodo, ctrls[0].ComprobanteID.Value, ctrls[0].ComprobanteIDNro.Value.Value);
                    this.BorrarAuxiliar(periodo, ctrls[1].ComprobanteID.Value, ctrls[1].ComprobanteIDNro.Value.Value);

                    //Borrar info del balance preliminar
                    this.BorrarSaldosXLibro(true, periodo, tipoBalancePreliminar);

                    //Header Moneda Local
                    newComprobanteML.Header.ComprobanteNro.Value = ctrls[1].ComprobanteIDNro.Value.Value;
                    newComprobanteML.Header.Fecha.Value = ctrls[1].FechaDoc.Value.Value;
                    newComprobanteML.Header.NumeroDoc.Value = ctrls[1].NumeroDoc.Value.Value;

                    //Header Moneda Ext
                    newComprobanteME.Header.ComprobanteNro.Value = ctrls[0].ComprobanteIDNro.Value.Value;
                    newComprobanteME.Header.Fecha.Value = ctrls[0].FechaDoc.Value.Value;
                    newComprobanteME.Header.NumeroDoc.Value = ctrls[0].NumeroDoc.Value.Value;
                }
                else
                {
                    //Header Moneda Local
                    newComprobanteML.Header.ComprobanteNro.Value = 0;
                    newComprobanteML.Header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    newComprobanteML.Header.NumeroDoc.Value = 0;

                    //Header Moneda extranjera
                    newComprobanteME.Header.ComprobanteNro.Value = 0;
                    newComprobanteME.Header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    newComprobanteME.Header.NumeroDoc.Value = 0;
                }
                #endregion

                newComprobanteML.Footer = new List<DTO_ComprobanteFooter>();
                newComprobanteME.Footer = new List<DTO_ComprobanteFooter>();

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region 6.Agrega los registros a glDocumentoControl
                if (estadoCtrl == EstadoAjuste.NoData)
                {
                    #region Agregar registro a glDocumentoControl ML
                    docCtrlML = new DTO_glDocumentoControl();

                    //Campos Principales
                    docCtrlML.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrlML.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrlML.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrlML.FechaDoc.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    docCtrlML.PeriodoDoc.Value = periodo;
                    docCtrlML.PeriodoUltMov.Value = periodo;
                    docCtrlML.AreaFuncionalID.Value = areaFuncionalID;
                    docCtrlML.PrefijoID.Value = prefix;
                    docCtrlML.DocumentoNro.Value = 0;
                    docCtrlML.MonedaID.Value = newComprobanteML.Header.MdaTransacc.Value;
                    docCtrlML.TasaCambioDOCU.Value = tasaCierre;
                    docCtrlML.TasaCambioCONT.Value = tasaCierre;
                    docCtrlML.ComprobanteID.Value = newComprobanteML.Header.ComprobanteID.Value;
                    docCtrlML.ComprobanteIDNro.Value = 0;
                    docCtrlML.Descripcion.Value = "CONT. AJUSTE EN CAMBIO ML";
                    docCtrlML.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                    DTO_TxResultDetail resultDetML = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrlML, true);
                    if (resultDetML.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultDetML);

                        return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    }

                    docCtrlML.NumeroDoc.Value = Convert.ToInt32(resultDetML.Key);
                    newComprobanteML.Header.NumeroDoc.Value = Convert.ToInt32(resultDetML.Key);
                    newComprobanteML.Header.ComprobanteNro.Value = 0;
                    #endregion
                    #region Agregar registro a glDocumentoControl ME
                    docCtrlME = new DTO_glDocumentoControl();

                    //Campos Principales
                    docCtrlME.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrlME.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrlME.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrlME.FechaDoc.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    docCtrlME.PeriodoDoc.Value = periodo;
                    docCtrlME.PeriodoUltMov.Value = periodo;
                    docCtrlME.AreaFuncionalID.Value = areaFuncionalID;
                    docCtrlME.PrefijoID.Value = prefix;
                    docCtrlME.DocumentoNro.Value = 0;
                    docCtrlME.MonedaID.Value = newComprobanteME.Header.MdaTransacc.Value;
                    docCtrlME.TasaCambioDOCU.Value = tasaCierre;
                    docCtrlME.TasaCambioCONT.Value = tasaCierre;
                    docCtrlME.ComprobanteID.Value = newComprobanteME.Header.ComprobanteID.Value;
                    docCtrlME.ComprobanteIDNro.Value = 0;
                    docCtrlME.Descripcion.Value = docCtrlML.Descripcion.Value = "CONT. AJUSTE EN CAMBIO ME";
                    docCtrlME.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                    DTO_TxResultDetail resultDetME = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrlME, true);
                    if (resultDetME.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultDetME);

                        return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    }
                    docCtrlME.NumeroDoc.Value = Convert.ToInt32(resultDetME.Key);
                    newComprobanteME.Header.NumeroDoc.Value = Convert.ToInt32(resultDetME.Key);
                    newComprobanteME.Header.ComprobanteNro.Value = 0;
                    #endregion
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region 7.Carga la info de los comprobantes segun los saldos
                List<DTO_coCuentaSaldo> saldosML = new List<DTO_coCuentaSaldo>();
                List<DTO_coCuentaSaldo> saldosME = new List<DTO_coCuentaSaldo>();
                TipoMoneda tipoIteracion = TipoMoneda.Local;
                DTO_coPlanCuenta cuenta;
                SaldoControl saldoCtrl;
                porcPrevio = porcTotal;

                for (int i = 0; i < 2; i++)
                {
                    List<DTO_ComprobanteFooter> footer = null;
                    List<DTO_ComprobanteFooter> contrapartida = new List<DTO_ComprobanteFooter>();

                    #region Carga los datos del footer segun el saldo

                    if (i == 0)
                    {
                        tipoIteracion = TipoMoneda.Local;
                        footer = newComprobanteML.Footer;
                    }
                    else if (i == 1)
                    {
                        tipoIteracion = TipoMoneda.Foreign;
                        footer = newComprobanteME.Footer;
                    }

                    List<DTO_coCuentaSaldo> saldosTemp = this._dal_Contabilidad.DAL_Contabilidad_GetSaldosForAjusteEnCambio(periodo, tipoIteracion, libroID);
                    foreach (DTO_coCuentaSaldo saldo in saldosTemp)
                    {
                        DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                        #region Carga la cuenta y el control de saldos

                        if (ctas.ContainsKey(saldo.CuentaID.Value))
                            cuenta = ctas[saldo.CuentaID.Value];
                        else
                        {
                            cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, saldo.CuentaID.Value, true, false);
                            ctas.Add(saldo.CuentaID.Value, cuenta);
                        }

                        if (ctrlSaldos.ContainsKey(cuenta.ConceptoSaldoID.Value))
                            saldoCtrl = ctrlSaldos[cuenta.ConceptoSaldoID.Value];
                        else
                        {
                            DTO_glConceptoSaldo concSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cuenta.ConceptoSaldoID.Value, true, false);
                            saldoCtrl = (SaldoControl)Enum.Parse(typeof(SaldoControl), concSaldo.coSaldoControl.Value.Value.ToString());
                            ctrlSaldos.Add(cuenta.ConceptoSaldoID.Value, saldoCtrl);
                        }

                        #endregion
                        #region Crea el detalle del comprobante

                        detalle.CentroCostoID.Value = saldo.CentroCostoID.Value;
                        detalle.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                        detalle.ConceptoSaldoID.Value = saldo.ConceptoSaldoID.Value;
                        detalle.CuentaID.Value = saldo.CuentaID.Value;
                        detalle.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                        detalle.LugarGeograficoID.Value = defLugarGeografico;
                        detalle.PrefijoCOM.Value = defPrefijo;
                        detalle.ProyectoID.Value = saldo.ProyectoID.Value;
                        detalle.TerceroID.Value = (cuenta.AjCambioTerceroInd.Value.Value) ? saldo.TerceroID.Value : defTercero;
                        detalle.TasaCambio.Value = tasaCierre;
                        detalle.vlrBaseML.Value = 0;
                        detalle.vlrBaseME.Value = 0;

                        detalle.IdentificadorTR.Value = saldo.IdentificadorTR.Value;

                        if (saldoCtrl == SaldoControl.Doc_Interno)
                        {
                            DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(saldo.IdentificadorTR.Value.Value));
                            detalle.DocumentoCOM.Value = docCtrl.DocumentoNro.Value.ToString();
                        }
                        else if (saldoCtrl == SaldoControl.Doc_Externo)
                        {
                            DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(saldo.IdentificadorTR.Value.Value));
                            detalle.DocumentoCOM.Value = docCtrl.DocumentoTercero.Value;
                        }
                        else
                            detalle.DocumentoCOM.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocumentoCOMAjusteXCuenta);
                        #endregion
                        #region Asignacion saldos y cuenta de la contrapartida

                        //Info segun los saldos
                        decimal saldoML = saldo.DbOrigenExtML.Value.Value + saldo.DbOrigenLocML.Value.Value + saldo.CrOrigenExtML.Value.Value + saldo.CrOrigenLocML.Value.Value
                            + saldo.DbSaldoIniExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value + saldo.CrSaldoIniExtML.Value.Value + saldo.CrSaldoIniLocML.Value.Value;
                        decimal saldoME = saldo.DbOrigenExtME.Value.Value + saldo.DbOrigenLocME.Value.Value + saldo.CrOrigenExtME.Value.Value + saldo.CrOrigenLocME.Value.Value
                            + saldo.DbSaldoIniExtME.Value.Value + saldo.DbSaldoIniLocME.Value.Value + saldo.CrSaldoIniExtME.Value.Value + saldo.CrSaldoIniLocME.Value.Value;

                        decimal ajuste = 0;
                        decimal saldoA_ML = saldoME * tasaCierre;
                        decimal saldoA_ME = saldoML / tasaCierre;

                        string ctaContrapartida = string.Empty;
                        switch (tipoIteracion)
                        {
                            case TipoMoneda.Local:
                                saldosML.Add(saldo);
                                ajuste = saldoA_ML - saldoML;
                                detalle.vlrMdaLoc.Value = ajuste;
                                detalle.vlrMdaExt.Value = 0;
                                newComprobanteML.Footer.Add(detalle);
                                ctaContrapartida = ajuste >= 0 ? ctaContrapartidaMLCr : ctaContrapartidaMLDb;
                                break;
                            case TipoMoneda.Foreign:
                                saldosME.Add(saldo);
                                ajuste = saldoA_ME - saldoME;
                                detalle.vlrMdaLoc.Value = 0;
                                detalle.vlrMdaExt.Value = ajuste;
                                newComprobanteME.Footer.Add(detalle);
                                ctaContrapartida = ajuste >= 0 ? ctaContrapartidaMECr : ctaContrapartidaMEDb;
                                break;
                        }
                        #endregion
                        #region Crea la contrapartida
                        List<DTO_ComprobanteFooter> busquedaContrapartida = contrapartida.Where
                        (
                            x =>
                            x.CentroCostoID.Value == detalle.CentroCostoID.Value &&
                            x.ConceptoSaldoID.Value == detalle.ConceptoSaldoID.Value &&
                            x.CuentaID.Value == ctaContrapartida &&
                            x.LineaPresupuestoID.Value == detalle.LineaPresupuestoID.Value &&
                            x.LugarGeograficoID.Value == detalle.LugarGeograficoID.Value &&
                            x.ProyectoID.Value == detalle.ProyectoID.Value
                        ).ToList();

                        DTO_ComprobanteFooter footerContrapartida = new DTO_ComprobanteFooter();
                        if (busquedaContrapartida != null && busquedaContrapartida.Count == 1)
                        {
                            #region Asigna la contrapartida del footer
                            footerContrapartida = busquedaContrapartida.First();
                            decimal ajusteContrapartida = 0;
                            switch (tipoIteracion)
                            {
                                case TipoMoneda.Local:
                                    ajusteContrapartida = footerContrapartida.vlrMdaLoc.Value.Value;
                                    ajusteContrapartida += (detalle.vlrMdaLoc.Value.Value * (-1));
                                    footerContrapartida.vlrMdaLoc.Value = ajusteContrapartida;
                                    break;
                                case TipoMoneda.Foreign:
                                    ajusteContrapartida = footerContrapartida.vlrMdaExt.Value.Value;
                                    ajusteContrapartida += (detalle.vlrMdaExt.Value.Value * (-1));
                                    footerContrapartida.vlrMdaExt.Value = ajusteContrapartida;
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Asigna la contrapartida del footer
                            PropertyInfo[] properties = detalle.GetType().GetProperties();
                            foreach (PropertyInfo pi in properties)
                            {
                                object o = pi.GetValue(detalle, null);
                                if (o is UDT)
                                {
                                    UDT udtOld = (UDT)o;
                                    UDT udtNew = (UDT)pi.GetValue(footerContrapartida, null);
                                    PropertyInfo piVal = udtOld.GetType().GetProperty("Value");
                                    piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                                }
                                else
                                {
                                    pi.SetValue(footerContrapartida, o, null);
                                }
                            }
                            FieldInfo[] fields = detalle.GetType().GetFields();
                            foreach (FieldInfo fi in fields)
                            {
                                object o = fi.GetValue(detalle);
                                if (o is UDT)
                                {
                                    UDT udtOld = (UDT)o;
                                    UDT udtNew = (UDT)fi.GetValue(footerContrapartida);
                                    PropertyInfo piVal = typeof(UDT).GetProperty("Value");
                                    piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                                }
                                else
                                {
                                    fi.SetValue(footerContrapartida, o);
                                }
                            }
                            footerContrapartida.CuentaID.Value = ctaContrapartida;
                            footerContrapartida.ConceptoSaldoID.Value = concSaldoDef;
                            footerContrapartida.IdentificadorTR.Value = 0;
                            footerContrapartida.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                            decimal ajusteContrapartida = 0;
                            switch (tipoIteracion)
                            {
                                case TipoMoneda.Local:
                                    ajusteContrapartida = (detalle.vlrMdaLoc.Value.Value * (-1));
                                    footerContrapartida.vlrMdaLoc.Value = ajusteContrapartida;
                                    break;
                                case TipoMoneda.Foreign:
                                    ajusteContrapartida = (detalle.vlrMdaExt.Value.Value * (-1));
                                    footerContrapartida.vlrMdaExt.Value = ajusteContrapartida;
                                    break;
                            }
                            contrapartida.Add(footerContrapartida);
                            #endregion
                        }
                        #endregion
                    }

                    footer.AddRange(contrapartida);
                    #endregion
                    porcTemp = (porcParte * i) / 2;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                #endregion
                #region 8.Genera la contabilizacion del comp ML
                result = this.ContabilizarComprobante(documentID, newComprobanteML, periodo, ModulesPrefix.co, 0, true);

                if (result.Result == ResultValue.NOK)
                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());

                DTO_ComprobanteAprobacion paraAprobacionML = new DTO_ComprobanteAprobacion();
                paraAprobacionML.ComprobanteID.Value = comprobanteAjusteCambioML;
                paraAprobacionML.ComprobanteNro.Value = newComprobanteML.Header.ComprobanteNro.Value.Value;
                paraAprobacionML.PeriodoID.Value = periodo;
                paraAprobacionML.NumeroDoc.Value = newComprobanteML.Header.NumeroDoc.Value.Value;
                paraAprobacionML.Aprobado.Value = true;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region 9.Genera la contabilizacion del comp ME
                result = this.ContabilizarComprobante(documentID, newComprobanteME, periodo, ModulesPrefix.co, 0, true);

                if (result.Result == ResultValue.NOK)
                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());

                DTO_ComprobanteAprobacion paraAprobacionME = new DTO_ComprobanteAprobacion();
                paraAprobacionME.ComprobanteID.Value = comprobanteAjusteCambioME;
                paraAprobacionME.ComprobanteNro.Value = newComprobanteME.Header.ComprobanteNro.Value.Value;
                paraAprobacionME.PeriodoID.Value = periodo;
                paraAprobacionME.NumeroDoc.Value = newComprobanteME.Header.NumeroDoc.Value.Value;
                paraAprobacionME.Aprobado.Value = true;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region 10.Actualiza en glControl el indicador del libro que se esta manejando
                string EmpNro = this.Empresa.NumeroControl.Value;
                string keyControl = EmpNro + "01" + AppControl.co_LibroOpConjuntas;
                DTO_glControl bloqueoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                bloqueoControl.Data.Value = libroID;
                this._moduloGlobal.glControl_Update(bloqueoControl);
                #endregion

                List<DTO_ComprobanteAprobacion> comprobantesAprob = new List<DTO_ComprobanteAprobacion>();
                comprobantesAprob.Add(paraAprobacionML);
                comprobantesAprob.Add(paraAprobacionME);

                Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> res = new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, comprobantesAprob);

                return res;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_AjusteEnCambio");
                return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Commit
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (estadoCtrl == EstadoAjuste.NoData)
                        {
                            //ML
                            docCtrlML.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlML.PrefijoID.Value);
                            docCtrlML.ComprobanteIDNro.Value = this.GenerarComprobanteNro(compML, docCtrlML.PrefijoID.Value, periodo, docCtrlML.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrlML, true, true, false);
                            this.ActualizaComprobanteNro(docCtrlML.NumeroDoc.Value.Value, docCtrlML.ComprobanteIDNro.Value.Value, false);


                            //ME
                            docCtrlME.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlME.PrefijoID.Value);
                            docCtrlME.ComprobanteIDNro.Value = this.GenerarComprobanteNro(compME, docCtrlME.PrefijoID.Value, periodo, docCtrlME.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrlME, true, true, false);
                            this.ActualizaComprobanteNro(docCtrlME.NumeroDoc.Value.Value, docCtrlME.ComprobanteIDNro.Value.Value, false);
                        }
                        #endregion
                    }
                    else
                        throw new Exception("Proceso_AjusteEnCambio - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="conceptoSaldoID">Id de concepto saldo</param>
        /// <returns>true si existe</returns>
        public DTO_TxResult Proceso_Mayorizar(int documentID, DateTime periodo, string tipoBalance, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            bool esCorporativa = false;
            string libroCorp = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceCorporativo);

            //Variable para CuentaAlterna para los libros de contabilidad exeptuando el libro Corporativo
            string CuentaAlternaID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CuentaCorporativaxDefecto);
            string CuentaID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaXDefecto);
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.co);

                //Valida si el libro es corporativo
                if (libroCorp == tipoBalance)
                    esCorporativa = true;

                if (estado != EstadoPeriodo.Abierto)
                {
                    if (estado == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (estado == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }

                DAL_Mayorizacion _dal_Mayorizacion = new DAL_Mayorizacion(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = _dal_Mayorizacion.Proceso_Mayorizar(documentID, periodo, tipoBalance, batchProgress, insideAnotherTx, CuentaAlternaID, esCorporativa, CuentaID);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_Mayorizacion");

                return result;
            }
        }

        /// <summary>
        /// Procesa el ajuste en cambio para un periodo seleccionado
        /// </summary>
        /// <param name="comps">Comprobantes para aprobar</param>
        /// <param name="periodo">Periodo del ajuste</param>
        /// <returns>Retorna el resultado de las operaciones</returns>
        public List<DTO_TxResult> Proceso_ProcesarBalancePreliminar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, DateTime periodo,
            string libroID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100 / 1;
                int i = 0;
                batchProgress[tupProgress] = 1;

                #region Valida si ya se aprobo y carga la informacion de los comprobantes
                EstadoAjuste estadoCtrl = this.HasDocument(documentID, periodo, libroID);
                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_DocumentoAprobado;
                    result.Details = new List<DTO_TxResultDetail>();
                    results.Add(result);

                    return results;
                }                
                #endregion
                #region Aprueba los comprobantes

                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);
                this.BorrarSaldosXLibro(false, periodo, tipoBalancePreliminar);

                foreach (DTO_ComprobanteAprobacion comp in comps)
                {
                    //Manejo de porcentajes para la aprobacion
                    porcTemp = (porcParte * i) / comps.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.ResultMessage = "OK";
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";
                    rd.line = i;

                    int numeroDoc = comp.NumeroDoc.Value.Value;
                    string compID = comp.ComprobanteID.Value;
                    int compNro = comp.ComprobanteNro.Value.Value;
                    string obs = comp.Observacion.Value;

                    try
                    {
                        result = this.Comprobante_Aprobar(documentID, actividadFlujoID, ModulesPrefix.co, numeroDoc,true, periodo, compID, compNro, obs,
                            true, false, true, true);
                    }
                    catch (Exception exAprob)
                    {
                        result.Result = ResultValue.NOK;
                        string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Proceso_ProcesarBalancePreliminar");
                        rd.Message = DictionaryMessages.Err_AprobComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                        result.Details.Add(rd);
                    }

                    results.Add(result);
                }
                #endregion
                #region Actualiza en glControl el indicador del libro que se esta manejando
                string EmpNro = this.Empresa.NumeroControl.Value;
                string keyControl = EmpNro + "01" + AppControl.co_LibroOpConjuntas;
                DTO_glControl bloqueoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                bloqueoControl.Data.Value = string.Empty;
                this._moduloGlobal.glControl_Update(bloqueoControl);
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ProcesarBalancePreliminar");
                result.Details = new List<DTO_TxResultDetail>();
                results.Add(result);

                return results;
            }
            finally
            {
                if (results != null)
                {
                    bool valid = true;
                    foreach (DTO_TxResult r in results)
                    {
                        if (r.Result == ResultValue.NOK)
                            valid = false;
                    }

                    if (!insideAnotherTx)
                        if (valid)
                            base._mySqlConnectionTx.Commit();
                        else
                            base._mySqlConnectionTx.Rollback();
                }
                else
                {
                    if (base._mySqlConnectionTx != null && !insideAnotherTx)
                        base._mySqlConnectionTx.Rollback();
                }
            }

        }

        /// <summary>
        /// Actualiza el valor de la cuenta alterna en las tablas de coBalance coCuentaSaldo y coAuxiliar
        /// </summary>
        public DTO_TxResult Proceso_CuentaAlterna(int documentID, string actividadFlujoID, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            //batchProgress[tupProgress] = 1;

            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad.DAL_Proceso_CuentaAlterna();
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CuentaAlterna");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Proceso de prorrateo IVA
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_ProrrateoIVA(int documentID, string actividadFlujoID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_glDocumentoControl docCtrl = null;
            DTO_Comprobante comp = null;
            DTO_coComprobante coComp = null;
            try
            {
                #region Variables
                //Modulos y dals
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //Porcentaje de resultados
                decimal porcTotal = 0;
                decimal porcParte = 100 / 5;
                batchProgress[tupProgress] = 1;
                //Variables de datos
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                //glControl
                string ctaContra = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaGastoReclasificaProrateoIVA);
                string prefijoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string linPresXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string LugGeoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                DateTime periodo = DateTime.Parse(periodoStr);
                //Totales
                decimal totalML = 0;
                decimal totalME = 0;
                decimal totalIVA_ML = 0;
                decimal totalIVA_ME = 0;
                decimal porcTotalIVA = 0;
                //Info temporal
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                DTO_coPlanCuenta cta = null;
                // variables por defecto
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                #endregion

                #region Valida que no hayan datos en coAuxiliarPre
                int count = this._dal_Comprobante.DAL_ComprobantePre_HasData(true, periodo);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;

                    return result;
                }
                #endregion
                #region Trae la info de comprobantes para Ingresos x venta
                List<DTO_ComprobanteFooter> detalle = this._dal_Comprobante.DAL_Comprobante_IvaProrrateoIngresos(periodo, TipoCuentaGrupo.IngresoXVenta);
                foreach (DTO_ComprobanteFooter d in detalle)
                {
                    if (cacheCtas.ContainsKey(d.CuentaID.Value))
                        cta = cacheCtas[d.CuentaID.Value];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, d.CuentaID.Value, true, false);
                        cacheCtas.Add(d.CuentaID.Value, cta);
                    }

                    //Va sumando el total de las cuentas
                    totalML = totalML + d.vlrMdaLoc.Value.Value;
                    totalME = totalME + d.vlrMdaExt.Value.Value;

                    //Verifica que cuentas no son de IVA pero tienen tarifa
                    if (d.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && !string.IsNullOrWhiteSpace(d.DatoAdd2.Value) && d.DatoAdd2.Value != "0")
                    {
                        totalIVA_ML = totalIVA_ML + d.vlrMdaLoc.Value.Value;
                        totalIVA_ME = totalIVA_ME + d.vlrMdaExt.Value.Value;
                    }
                }

                totalML = Math.Round(totalML, 2);
                totalME = Math.Round(totalME, 2);
                totalIVA_ML = Math.Round(totalIVA_ML, 2);
                totalIVA_ME = Math.Round(totalIVA_ME, 2);

                //Saca el porcentaje del valor final de registros 
                porcTotalIVA = Math.Round((totalIVA_ML * 100 / totalML), 2);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Trae los registros de IVA descontable
                List<DTO_ComprobanteFooter> detalleIVADesc = this._dal_Comprobante.DAL_Comprobante_IvaProrrateoDescontable(periodo, TipoCuentaGrupo.IVADescontable, terceroPorDefecto);
                List<DTO_ComprobanteFooter> tempList = new List<DTO_ComprobanteFooter>();
                if (detalleIVADesc.Count > 0)
                {
                    if (cacheCtas.ContainsKey(detalleIVADesc.First().CuentaID.Value))
                        cta = cacheCtas[detalleIVADesc.First().CuentaID.Value];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, detalleIVADesc.First().CuentaID.Value, true, false);
                        cacheCtas.Add(detalleIVADesc.First().CuentaID.Value, cta);
                    }

                    if (!string.IsNullOrWhiteSpace(cta.NITCierreAnual.Value))
                        tempList = detalleIVADesc.Where(x => x.TerceroID.Value != cta.NITCierreAnual.Value).ToList();
                }

                foreach (DTO_ComprobanteFooter det in tempList)
                {
                    if (cacheCtas.ContainsKey(det.CuentaID.Value))
                        cta = cacheCtas[det.CuentaID.Value];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, det.CuentaID.Value, true, false);
                        cacheCtas.Add(det.CuentaID.Value, cta);
                    }

                    #region Partida
                    det.PrefijoCOM.Value = prefijoXDef;
                    det.LineaPresupuestoID.Value = linPresXDef;
                    det.LugarGeograficoID.Value = LugGeoXDef;
                    det.ConceptoCargoID.Value = concCargoXDef;

                    det.TasaCambio.Value = 0;
                    det.IdentificadorTR.Value = 0;
                    det.DocumentoCOM.Value = string.Empty;
                    det.vlrMdaLoc.Value = Math.Round(det.vlrMdaLoc.Value.Value * porcTotalIVA / 100, 2);
                    det.vlrMdaExt.Value = Math.Round(det.vlrMdaExt.Value.Value * porcTotalIVA / 100, 2);
                    det.vlrBaseML.Value = Math.Round(det.vlrBaseML.Value.Value * porcTotalIVA / 100, 2);
                    det.vlrBaseME.Value = Math.Round(det.vlrBaseME.Value.Value * porcTotalIVA / 100, 2);

                    det.DatoAdd1.Value = AuxiliarDatoAdd1.IVA.ToString();
                    det.DatoAdd2.Value = cta.ImpuestoPorc.Value.Value.ToString();
                    footer.Add(det);
                    #endregion
                    #region Contrapartida
                    DTO_ComprobanteFooter contra = new DTO_ComprobanteFooter();
                    contra.CuentaID.Value = ctaContra;
                    contra.TerceroID.Value = det.TerceroID.Value;
                    contra.ProyectoID.Value = det.ProyectoID.Value;
                    contra.CentroCostoID.Value = det.CentroCostoID.Value;
                    contra.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                    contra.ConceptoCargoID.Value = det.ConceptoCargoID.Value;
                    contra.LugarGeograficoID.Value = det.LugarGeograficoID.Value;
                    contra.PrefijoCOM.Value = det.PrefijoCOM.Value;
                    contra.DocumentoCOM.Value = det.DocumentoCOM.Value;
                    contra.ActivoCOM.Value = det.ActivoCOM.Value;
                    contra.ConceptoSaldoID.Value = det.ConceptoSaldoID.Value;
                    contra.IdentificadorTR.Value = det.IdentificadorTR.Value;
                    contra.Descriptivo.Value = det.Descriptivo.Value;
                    contra.TasaCambio.Value = det.TasaCambio.Value;
                    contra.vlrBaseML.Value = det.vlrBaseML.Value * -1;
                    contra.vlrBaseME.Value = det.vlrBaseME.Value * -1;
                    contra.vlrMdaLoc.Value = det.vlrMdaLoc.Value * -1;
                    contra.vlrMdaExt.Value = det.vlrMdaExt.Value * -1;
                    contra.vlrMdaOtr.Value = det.vlrMdaOtr.Value * -1;
                    contra.DatoAdd1.Value = det.DatoAdd1.Value;
                    contra.DatoAdd2.Value = det.DatoAdd2.Value;
                    contra.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    footer.Add(contra);
                    #endregion
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                if (footer.Count > 0)
                {
                    string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteReclasificacionesBalanceFiscal); ;
                    coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);
                    docCtrl = new DTO_glDocumentoControl();
                    #region Agregar registro a glDocumentoControl

                    //Campos Principales
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.Fecha.Value = periodo;
                    docCtrl.PeriodoDoc.Value = periodo;
                    docCtrl.PeriodoUltMov.Value = periodo;
                    docCtrl.AreaFuncionalID.Value = this._moduloGlobal.GetAreaFuncionalByUser();
                    docCtrl.PrefijoID.Value = this.GetPrefijoByDocumento(documentID); ;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.MonedaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    docCtrl.TasaCambioDOCU.Value = 0;
                    docCtrl.TasaCambioCONT.Value = 0;
                    docCtrl.ComprobanteID.Value = compID;
                    docCtrl.ComprobanteIDNro.Value = 0;
                    docCtrl.Observacion.Value = string.Empty;
                    docCtrl.Estado.Value = Convert.ToByte(EstadoDocControl.Aprobado);

                    DTO_TxResultDetail rd = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rd);

                        return result;
                    }

                    docCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Crea el header del comprobante

                    string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                    header.LibroID.Value = tipoBalanceFuncional;
                    header.ComprobanteID.Value = docCtrl.ComprobanteID.Value;
                    header.ComprobanteNro.Value = 0;
                    header.Fecha.Value = periodo;
                    header.PeriodoID.Value = periodo;
                    header.EmpresaID.Value = this.Empresa.ID.Value;
                    header.NumeroDoc.Value = docCtrl.NumeroDoc.Value.Value;
                    header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Local;
                    header.MdaTransacc.Value = docCtrl.MonedaID.Value;
                    header.TasaCambioBase.Value = 0;
                    header.TasaCambioOtr.Value = 0;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Contabiliza el comprobante

                    comp = new DTO_Comprobante();
                    comp.Header = header;
                    comp.Footer = footer;

                    result = this.ContabilizarComprobante(documentID, comp, periodo, ModulesPrefix.co, 0, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                }

                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ProrrateoIVA");
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

                        if (comp != null)
                        {
                            docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                            comp.Header.ComprobanteNro.Value = this.GenerarComprobanteNro(coComp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, false);
                            this.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, comp.Header.ComprobanteNro.Value.Value, false);
                        }
                    }
                    else
                        throw new Exception("Proceso_ProrrateoIVA - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Proceso para consolidar balances entre empresas
        /// </summary>
        /// <param name="documentID">Identificador del documento que genera el proceso</param>
        /// <param name="list">Lista de empresas a consolidar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ConsolidacionBalances(int documentID, string actividadFlujoID, List<DTO_ComprobanteConsolidacion> list, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                #region Variables

                //Modulos y dals
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteConsolidacion);
                DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                DateTime periodo = DateTime.Parse(periodoStr);
                if (coComp == null)
                {
                    results.Clear();
                    result = new DTO_TxResult();
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_InvalidComp;
                    results.Add(result);

                    return results;
                }
                #endregion
                int i = 0;
                foreach (DTO_ComprobanteConsolidacion empresa in list)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / list.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    if (empresa.Consolidar)
                        result = this.ConsolidarBalance(documentID, actividadFlujoID, periodo, empresa, coComp, false);

                    results.Add(result);
                }

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ConsolidacionBalances");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Reclasifica un libro fiscal 
        /// </summary>
        /// <param name="documentID">Identificador del documento que ejecuta la transaccion</param>
        /// <param name="libroID">Identificador del libro fiscal</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult Proceso_ReclasificacionLibros(int documentID, DateTime periodoID, string libroID, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                string libroIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (libroID == libroIFRS)
                    result = this._dal_Contabilidad.DAL_Contabilidad_ReclasificarIFRS(periodoID);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_ReclasificacionLibros");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
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

        #region Ajuste comprobante

        #region Funciones privadas

        /// <summary>
        /// Aprueba ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public DTO_TxResult AjusteComprobante_Aprobar(int documentID, string actividadFlujoID, DTO_Comprobante comp, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                #region Declara variables
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coAuxiliarAjustaDeta = (DAL_coAuxiliarAjustaDeta)this.GetInstance(typeof(DAL_coAuxiliarAjustaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DateTime periodoID = comp.Header.PeriodoID.Value.Value;
                string comprobanteID = comp.Header.ComprobanteID.Value;
                int compNro = comp.Header.ComprobanteNro.Value.Value;
                #endregion
                #region Trae el documento y la info relacionada
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(comp.Header.NumeroDoc.Value.Value);
                if (docCtrl == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_NoDocument;
                    return result;
                }

                DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, docCtrl.DocumentoID.Value.Value.ToString(), true, true);
                DTO_coComprobante coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteID, true, false);
                ModulesPrefix mod = ModulesPrefix.co; // (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), doc.ModuloID.Value.ToLower());

                #endregion
                #region Validaciones

                #region Estado del documento
                if (docCtrl.Estado.Value.Value != (byte)EstadoDocControl.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_AjusteInvalidEstado;
                    return result;
                }
                #endregion
                #region Indicador de ajuste
                if (!doc.AjustaComprobanteInd.Value.Value)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_IndAjusteComp;
                    return result;
                }
                #endregion
                #endregion
                #region Retira los saldos del comprobante a ajustar
               
                DTO_Comprobante compOrigen = this.Comprobante_Get(true, false, periodoID, comprobanteID, compNro, null, null);
                result = this._dal_Contabilidad.DAL_Contabilidad_SustraerSaldos(documentID, compOrigen, coComp.BalanceTipoID.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Borra el comprobante de la tabla coAuxiliar
                this.BorrarAuxiliar(periodoID, comprobanteID, compNro);
                #endregion
                #region Actualiza las tabla de ajuste de comprobantes y auxiliarPre
                //Procesa la info
                this._dal_coAuxiliarAjustaDeta.DAL_coAuxiliarAjustaDeta_Procesar(docCtrl.NumeroDoc.Value.Value);
                //Elimina los datos del auxiliar pre
                this._dal_Comprobante.DAL_Comprobante_BorrarAuxiliar_Pre(periodoID, comprobanteID, compNro);

                #endregion
                #region Contabiliza el comprobante
                result = this.ContabilizarComprobante(documentID, comp, periodoID, mod, 0, true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Asigna el flujo
                this.AsignarFlujo(documentID, comp.Header.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_Ajuste");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Funciones públicas

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante AjusteComprobante_Get(DateTime periodo, string comprobanteID, int compNro)
        {
            try
            {
                this._dal_coAuxiliarAjustaDeta = (DAL_coAuxiliarAjustaDeta)this.GetInstance(typeof(DAL_coAuxiliarAjustaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int ajusteCount = this._dal_coAuxiliarAjustaDeta.DAL_coAuxiliarAjustaDeta_Count(periodo, comprobanteID, compNro);
                bool isPre = ajusteCount > 0 ? true : false;

                DTO_Comprobante comp = this.Comprobante_Get(true, isPre, periodo, comprobanteID, compNro, null, null);

                return comp;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AjusteComprobante_Get");
                return null;
            }
        }

        /// <summary>
        /// Ajusta un comprobante existente
        /// </summary>
        /// <param name="documentID">documento Id</param>
        /// <param name="comp">Comprobante a ajustar</param>
        /// <param name="insideAnotherTx">determina si viene de una transaccion</param>
        public DTO_TxResult AjusteComprobante_Generar(int documentID, string actividadFlujoID, DTO_Comprobante comp, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                int numeroDoc = comp.Header.NumeroDoc.Value.Value;
                this.AjusteComprobante_Eliminar(documentID, actividadFlujoID, numeroDoc, false, true);

                this._dal_coAuxiliarAjustaDeta.DAL_coAuxiliarAjustaDeta_Add(comp);

                result = this.ComprobantePre_Add(documentID, ModulesPrefix.co, comp, string.Empty, string.Empty, false, null, null, new Dictionary<Tuple<int, int>, int>(), true);
                if (result.Result == ResultValue.NOK)
                    return result;

                result = this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, false, "Reajuste");

                if (result.Result == ResultValue.OK)
                    result.ResultMessage = string.Empty;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AjusteComprobante_Generar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Elimina un ajuste de comprobante
        /// </summary>
        /// <param name="numeroDoc"></param>
        /// <returns></returns>
        public DTO_TxResult AjusteComprobante_Eliminar(int documentID, string actividadFlujoID, int numeroDoc, bool asignarFlujo, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                this._dal_coAuxiliarAjustaDeta = (DAL_coAuxiliarAjustaDeta)this.GetInstance(typeof(DAL_coAuxiliarAjustaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coAuxiliarAjustaDeta.DAL_coAuxiliarAjustaDeta_Delete(numeroDoc);

                if (asignarFlujo)
                    this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, true, string.Empty);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AjusteComprobante_Eliminar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae un listado de los ajustes pendientes de aprobart
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> AjusteComprobante_GetPendientes(string actividadFlujoID)
        {
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_coAuxiliarAjustaDeta = (DAL_coAuxiliarAjustaDeta)this.GetInstance(typeof(DAL_coAuxiliarAjustaDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
            string usuarioID = seUsuario.ID.Value;
            List<DTO_ComprobanteAprobacion> list = this._dal_coAuxiliarAjustaDeta.DAL_coAuxiliarAjustaDeta_GetPendientes(actividadFlujoID, usuarioID);
            //foreach (DTO_ComprobanteAprobacion item in list)
            //    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);

            return list;
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> AjusteComprobante_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                int i = 0;
                foreach (DTO_ComprobanteAprobacion comp in comps)
                {
                    #region Variables
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / comps.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    rd.line = i;
                    rd.Message = string.Empty;

                    int numeroDoc = comp.NumeroDoc.Value.Value;
                    DateTime periodo = comp.PeriodoID.Value.Value;
                    string compID = comp.ComprobanteID.Value;
                    int compNro = comp.ComprobanteNro.Value.Value;
                    string obs = comp.Observacion.Value;
                    #endregion

                    if (comp.Aprobado.Value.Value)
                    {
                        try
                        {
                            DTO_Comprobante comprobante = this.AjusteComprobante_Get(periodo, compID, compNro);
                            result = this.AjusteComprobante_Aprobar(documentID, actividadFlujoID, comprobante, false);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "AjusteComprobante_Aprobar");
                            rd.Message = DictionaryMessages.Err_AprobComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (comp.Rechazado.Value.Value)
                    {
                        try
                        {
                            result = this.AjusteComprobante_Eliminar(documentID, actividadFlujoID, numeroDoc, true, false);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "AjusteComprobante_Eliminar");
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        #endregion

        #endregion

        #region Comprobante

        #region Funciones Privadas

        #region Validaciones

        /// <summary>
        /// Valida que un comprobante cumpla las reglas
        /// </summary>
        /// <param name="header"></param>
        /// <param name="footer">Detalle del omprobante</param>
        /// <param name="currentMod">Modulo Actual</param>
        /// <param name="comp">Comprobante que esta trabajando</param>
        /// <returns>Retorna el resultado de la validacion</returns>
        private DTO_TxResult ValidacionHeader(int documentID, DTO_ComprobanteHeader header, List<DTO_ComprobanteFooter> footer, ModulesPrefix currentMod)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;

            try
            {
                result.Result = ResultValue.OK;
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_MasterBasic basic;
                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();

                #region Validacion de periodo abierto
                EstadoPeriodo validPeriod = this._moduloAplicacion.CheckPeriod(header.PeriodoID.Value.Value, currentMod);
                if (validPeriod != EstadoPeriodo.Abierto)
                {
                    rdF.Field = "PeriodoID";

                    if (validPeriod == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (validPeriod == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }
                #endregion
                #region Validacion que la fecha este en el periodo correspondiente
                if (header.PeriodoID.Value.Value.Year != header.Fecha.Value.Value.Year || header.PeriodoID.Value.Value.Month != header.Fecha.Value.Value.Month)
                {
                    rdF.Field = "Fecha";
                    result.ResultMessage = DictionaryMessages.Err_InvalidDatePeriod;

                    result.Result = ResultValue.NOK;
                    return result;
                }
                #endregion
                #region Validacion Creditos y debitos

                decimal sumML = 0;
                decimal sumME = 0;

                foreach (DTO_ComprobanteFooter detail in footer)
                {
                    sumML += detail.vlrMdaLoc.Value.Value;
                    sumME += detail.vlrMdaExt.Value.Value;
                }

                sumML = Math.Round(sumML, 2);
                sumME = Math.Round(sumME, 2);

                if (sumML != 0 || sumME != 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_InvalidDebCred;
                    return result;
                }

                #endregion
                #region Existencia de Datos (FKs)
                #region Comprobante

                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, header.ComprobanteID.Value, true, false);
                if (basic == null || basic.ID == null || basic.IdName == null)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ComprobanteID";
                    rdF.Message = DictionaryMessages.FkNotFound + "&&" + header.ComprobanteID.Value;
                    rd.DetailsFields.Add(rdF);

                    result.Details.Add(rd);
                    result.Result = ResultValue.NOK;
                    return result;
                }
                #endregion
                #region Moneda de transacción

                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glMoneda, header.MdaTransacc.Value, true, false);
                if (basic == null || basic.ID == null || basic.IdName == null)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "MdaTransacc";
                    rdF.Message = DictionaryMessages.FkNotFound + "&&" + header.MdaTransacc.Value;
                    rd.DetailsFields.Add(rdF);

                    result.Details.Add(rd);
                    result.Result = ResultValue.NOK;
                    return result;
                }
                #endregion
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ValidacionHeader");

                return result;
            }
        }

        /// <summary>
        /// Valida que un comprobante cumpla las reglas
        /// </summary>
        /// <param name="txInventarios">Indica si la transaccion es del modulo de inventarios</param>
        /// <param name="footer">Detalle del comprobante</param>
        /// <returns>Retorna el resultado de la validacion</returns>
        private DTO_TxResult ValidacionDetalle(int documentoID, bool txInventarios, DTO_Comprobante comp,
            Dictionary<string, DTO_coPlanCuenta> cacheCuentas, Dictionary<string, DTO_glConceptoSaldo> cacheConceptoSaldo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DAL_MasterHierarchy hierarchyDAL = (DAL_MasterHierarchy)this.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DAL_MasterComplex complexDAL = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            try
            {
                #region variables de la funcion
                #region Variables de cache
                //Info genérica tablas maestras
                DTO_MasterBasic basic;
                DTO_MasterComplex complexDTO;
                Dictionary<string, string> complex_pks;
                //DTOs Tablas maestras
                DTO_coPlanCuenta cta = null;
                DTO_glConceptoSaldo concSaldo = null;
                DTO_coCentroCosto ctoCosto = null;
                DTO_coProyecto proy = null;
                DTO_plLineaPresupuesto lineaPresupuesto = null;
                DTO_coTercero tercero = null;
                #endregion
                #region Diccionarios de cache
                //Diccionarios maestras
                Dictionary<string, DTO_coTercero> cacheTerceros = new Dictionary<string, DTO_coTercero>();
                Dictionary<string, DTO_MasterBasic> cacheProyectos = new Dictionary<string, DTO_MasterBasic>();
                Dictionary<string, DTO_MasterBasic> cacheCentrosCosto = new Dictionary<string, DTO_MasterBasic>();
                Dictionary<string, DTO_MasterBasic> cacheLineaPresupuestal = new Dictionary<string, DTO_MasterBasic>();
                Dictionary<string, DTO_MasterBasic> cacheConceptoCargo = new Dictionary<string, DTO_MasterBasic>();
                Dictionary<string, DTO_MasterBasic> cacheLugarGeograf = new Dictionary<string, DTO_MasterBasic>();
                #endregion
                #region Otras
                List<DTO_ComprobanteFooter> footer = comp.Footer;
                DateTime periodo = comp.Header.PeriodoID.Value.Value;
                string monedaComp = comp.Header.MdaTransacc.Value;
                string monedaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string terceroDIAN = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_NitDIAN);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                DTO_coComprobante compDTO = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);
                bool biMoneda = compDTO.biMonedaInd.Value.Value ? true : false;

                string consPresupuestal = this.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd);
                bool validateConsPresupuestal = false;
                if (consPresupuestal == "1")
                    validateConsPresupuestal = true;

                Dictionary<string, bool> modOpen = new Dictionary<string, bool>();
                Dictionary<string, bool> modCheck = new Dictionary<string, bool>();
                DateTime siguientePeriodo = periodo.AddMonths(1);
                #endregion
                #endregion
                #region Trae la lista de modulos y revisa si se debe validar
                List<DTO_aplModulo> mods = this._moduloAplicacion.aplModulo_GetByVisible(1, true).ToList();
                foreach (DTO_aplModulo m in mods)
                {
                    string mID = m.ModuloID.Value.ToLower();
                    if (mID == ModulesPrefix.co.ToString())
                    {
                        modOpen.Add(mID, true);
                        modCheck.Add(mID, false);
                    }
                    else
                    {
                        //Revisa si el periodo esta cerrado, en caso de estar abierto reviso que el mes siguiente
                        bool exists = false;
                        ModulesPrefix mPref = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mID);
                        bool pCerrado = this.UltimoMesCerrado(mPref, periodo, ref exists);
                        if (pCerrado)
                        {
                            //periodo cerrado
                            modOpen.Add(mID, false);
                            modCheck.Add(mID, false);
                        }
                        else if (exists)
                        {
                            //periodo abierto, despues de haber cerrado un mes 
                            modOpen.Add(mID, true);
                            modCheck.Add(mID, true);
                        }
                        else
                        {
                            //periodo abierto
                            modOpen.Add(mID, true);
                            modCheck.Add(mID, false);
                        }
                    }
                }
                #endregion
                for (int i = 0; i < footer.Count; ++i)
                {
                    DTO_ComprobanteFooter detail = footer[i];
                    #region Variables x detalle
                    bool isValid = true;
                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.line = i + 1;

                    cta = null;
                    concSaldo = null;
                    proy = null;
                    ctoCosto = null;
                    lineaPresupuesto = null;
                    tercero = null;
                    #endregion

                    #region 1. Control de componente (Validacion FKs)
                    try
                    {
                        #region Cuenta y Conc Saldo
                        if (cacheCuentas.ContainsKey(detail.CuentaID.Value))
                            cta = cacheCuentas[detail.CuentaID.Value];
                        else
                        {
                            cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, detail.CuentaID.Value, true, false);
                            cacheCuentas.Add(detail.CuentaID.Value, cta);
                        }

                        if (cta == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CuentaID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.CuentaID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else
                        {
                            #region Concepto saldo
                            if (cacheConceptoSaldo.ContainsKey(detail.ConceptoSaldoID.Value))
                                concSaldo = (DTO_glConceptoSaldo)cacheConceptoSaldo[detail.ConceptoSaldoID.Value];
                            else
                            {
                                concSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, detail.ConceptoSaldoID.Value, true, false);
                                cacheConceptoSaldo.Add(detail.ConceptoSaldoID.Value, concSaldo);
                            }
                            #endregion
                        }
                        #endregion
                        #region Proyecto

                        if (cacheProyectos.ContainsKey(detail.ProyectoID.Value))
                            basic = cacheProyectos[detail.ProyectoID.Value];
                        else
                        {
                            basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, detail.ProyectoID.Value, true, false);
                            cacheProyectos.Add(detail.ProyectoID.Value, basic);
                        }

                        if (basic == null || basic.ID == null || basic.IdName == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ProyectoID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.ProyectoID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else
                        {
                            proy = (DTO_coProyecto)basic;
                        }
                        #endregion
                        #region Centro de costo

                        if (cacheCentrosCosto.ContainsKey(detail.CentroCostoID.Value))
                            basic = cacheCentrosCosto[detail.CentroCostoID.Value];
                        else
                        {
                            basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, detail.CentroCostoID.Value, true, false);
                            cacheCentrosCosto.Add(detail.CentroCostoID.Value, basic);
                        }

                        if (basic == null || basic.ID == null || basic.IdName == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CentroCostoID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.CentroCostoID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else
                        {
                            ctoCosto = (DTO_coCentroCosto)basic;
                        }
                        #endregion
                        #region Linea presupuestal

                        if (cacheLineaPresupuestal.ContainsKey(detail.LineaPresupuestoID.Value))
                            basic = cacheLineaPresupuestal[detail.LineaPresupuestoID.Value];
                        else
                        {
                            basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, detail.LineaPresupuestoID.Value, true, false);
                            cacheLineaPresupuestal.Add(detail.LineaPresupuestoID.Value, basic);
                        }

                        if (basic == null || basic.ID == null || basic.IdName == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "LineaPresupuestoID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.LineaPresupuestoID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else
                        {
                            lineaPresupuesto = (DTO_plLineaPresupuesto)basic;
                        }
                        #endregion
                        #region Tercero

                        if (cacheTerceros.ContainsKey(detail.TerceroID.Value))
                            tercero = cacheTerceros[detail.TerceroID.Value];
                        else
                        {
                            tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, detail.TerceroID.Value, true, false);
                            cacheTerceros.Add(detail.TerceroID.Value, tercero);
                        }

                        if (tercero == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "TerceroID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.TerceroID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }

                        #endregion
                        #region Concepto cargo

                        if (cacheConceptoCargo.ContainsKey(detail.ConceptoCargoID.Value))
                            basic = cacheConceptoCargo[detail.ConceptoCargoID.Value];
                        else
                        {
                            basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coConceptoCargo, detail.ConceptoCargoID.Value, true, false);
                            cacheConceptoCargo.Add(detail.ConceptoCargoID.Value, basic);
                        }

                        if (basic == null || basic.ID == null || basic.IdName == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "ConceptoCargoID";
                            rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.ConceptoCargoID.Value;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        #endregion
                        #region Lugar Geográfico
                        if (detail.LugarGeograficoID != null && !string.IsNullOrWhiteSpace(detail.LugarGeograficoID.Value))
                        {

                            if (cacheLugarGeograf.ContainsKey(detail.LugarGeograficoID.Value))
                                basic = cacheLugarGeograf[detail.LugarGeograficoID.Value];
                            else
                            {
                                basic = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, detail.LugarGeograficoID.Value, true, false);
                                cacheLugarGeograf.Add(detail.LugarGeograficoID.Value, basic);
                            }

                            if (basic == null || basic.ID == null || basic.IdName == null)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "LugarGeograficoID";
                                rdF.Message = DictionaryMessages.FkNotFound + "&&" + detail.LugarGeograficoID.Value;
                                rd.DetailsFields.Add(rdF);

                                result.Result = ResultValue.NOK;
                                isValid = false;
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex1)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex1, this.UserId.ToString(), "ValidacionDetalle - Validacion Fks");
                        return result;
                    }
                    #endregion
                    #region 2. Control de periodo
                    if (documentoID != AppProcess.AjusteEnCambio && documentoID != AppDocuments.ComprobanteAjusteCambio &&
                        concSaldo.coSaldoControl.Value.Value != (int)SaldoControl.Cuenta && detail.IdentificadorTR.Value.Value != 0)
                    {
                        string mID = concSaldo.ModuloID.Value.ToLower();
                        if (!modOpen[mID])
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = "CuentaID";
                            rdF.Message = DictionaryMessages.Err_Co_CtaPeriodClosed;
                            rd.DetailsFields.Add(rdF);

                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else if (modCheck[mID])
                        {
                            bool hasSaldos = this.Saldos_ExistsByIdentificadorTR(Convert.ToInt64(detail.IdentificadorTR.Value.Value), siguientePeriodo, libroFunc);
                            if (hasSaldos)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "CuentaID";
                                rdF.Message = DictionaryMessages.Err_ModWithSaldos + "&&" + siguientePeriodo.ToString(FormatString.Period);
                                rd.DetailsFields.Add(rdF);

                                result.Result = ResultValue.NOK;
                                isValid = false;
                            }
                        }
                    }
                    #endregion
                    #region 3. Validacion de inventarios (Saldo Control)
                    if (isValid && !txInventarios && concSaldo.coSaldoControl.Value.Value == (short)SaldoControl.Inventario)
                    {
                        rd.Message = DictionaryMessages.Err_Co_CtaInv;
                        result.Result = ResultValue.NOK;
                        isValid = false;
                    }
                    #endregion
                    #region 4. Control de consistencia contable (Proy/CtoCosto - Operacion)
                    if (isValid)
                    {
                        //string oper = cta.ProyectoInd.Value.Value ? proy.OperacionID.Value : ctoCosto.OperacionID.Value;
                        string oper = !string.IsNullOrWhiteSpace(proy.OperacionID.Value) ? proy.OperacionID.Value : ctoCosto.OperacionID.Value;
                        if (string.IsNullOrWhiteSpace(oper))
                        {
                            rd.Message = DictionaryMessages.Err_OperacionIsNullorEmpty;
                            result.Result = ResultValue.NOK;
                            isValid = false;
                        }
                        else
                        {
                            try
                            {
                                bool needValidation = false;
                                #region Revisa si requiere validacion
                                DTO_coControl coCtrlDTO = null;

                                complexDAL.DocumentID = AppMasters.coControl;
                                complex_pks = new Dictionary<string, string>();

                                complex_pks.Add("CuentaID", string.Empty);
                                complex_pks.Add("OperacionID", oper);
                                complex_pks.Add("CentroCostoID", ctoCosto.ID.Value);

                                //Datos de jerarquia
                                hierarchyDAL.DocumentID = AppMasters.coPlanCuenta;
                                List<string> parents = hierarchyDAL.GetParents(cta.ID.Value);
                                for (int j = 0; j < parents.Count; ++j)
                                {
                                    complex_pks["CuentaID"] = parents[j];
                                    complexDTO = complexDAL.DAL_MasterComplex_GetByID(complex_pks, true);
                                    try
                                    {
                                        if (complexDTO != null)
                                        {
                                            needValidation = true;
                                            break;
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex2, this.UserId.ToString(), "ValidacionDetalle - Control Consistencia Contable - ex2");
                                    }
                                }

                                #endregion
                                #region Si se debe validar la combinacion
                                if (needValidation)
                                {
                                    bool hasRelelation = false;
                                    bool isCtaValid = true;

                                    for (int j = 0; j < parents.Count; ++j)
                                    {
                                        complex_pks["CuentaID"] = parents[j];
                                        complexDTO = complexDAL.DAL_MasterComplex_GetByID(complex_pks, true);

                                        //Si tiene relacion
                                        if (complexDTO != null)
                                        {
                                            coCtrlDTO = (DTO_coControl)complexDTO;
                                            hasRelelation = true;
                                            if (coCtrlDTO.ExcluyeInd.Value.Value)
                                            {
                                                isCtaValid = false;
                                                break;
                                            }
                                        }
                                    }

                                    if (!isCtaValid || !hasRelelation)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = "CuentaID - CentroCostoID - Operacion";
                                        rdF.Message = DictionaryMessages.Err_Co_InvalidCtaCtoCostoOp + "&&" + cta.ID.Value + "&&" + ctoCosto.ID.Value + "&&" + oper;
                                        rd.DetailsFields.Add(rdF);

                                        result.Result = ResultValue.NOK;
                                        isValid = false;
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex3)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex3, this.UserId.ToString(), "ValidacionDetalle - Control Consistencia contable - ex3");
                                return result;
                            }
                        }
                    }
                    #endregion
                    #region 5. Control de consistencia presupuestal (Act - Linea presupuestal)
                    if (result.Result == ResultValue.OK && validateConsPresupuestal)
                    {
                        try
                        {
                            complexDAL.DocumentID = AppMasters.plActividadLineaPresupuestal;
                            complex_pks = new Dictionary<string, string>();

                            complex_pks.Add("ActividadID", proy.ActividadID.Value);
                            complex_pks.Add("LineaPresupuestoID", lineaPresupuesto.ID.Value);

                            complexDTO = complexDAL.DAL_MasterComplex_GetByID(complex_pks, true);
                            if (complexDTO == null)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "ActividadID - LineaPresupuestoID";
                                rdF.Message = DictionaryMessages.Err_Co_InvalidActLineaPres + "&&" + proy.ActividadID.Value + "&&" + lineaPresupuesto.ID.Value;
                                rd.DetailsFields.Add(rdF);

                                result.Result = ResultValue.NOK;
                                isValid = false;
                            }

                        }
                        catch (Exception ex4)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex4, this.UserId.ToString(), "ValidacionDetalle - ControlConsistencia Presupuestal");
                            return result;
                        }
                    }
                    #endregion
                    #region 6. Control de consistencia fiscal (Impuestos)
                    if (isValid && detail.TerceroID.Value != terceroDef && detail.TerceroID.Value != terceroDIAN && !string.IsNullOrWhiteSpace(cta.ImpuestoTipoID.Value))
                    {
                        try
                        {
                            //Revisa que tenga base
                            if (detail.vlrBaseML.Value == null || detail.vlrBaseML.Value.Value <= 0)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "vlrBaseML";
                                rdF.Message = DictionaryMessages.ZeroField + "&&vlrBaseML";
                                rd.DetailsFields.Add(rdF);

                                result.Result = ResultValue.NOK;
                                isValid = false;
                            }
                            else if (cta.MontoMinimo.Value != null && detail.vlrBaseML.Value.Value > cta.MontoMinimo.Value.Value)
                            {
                                decimal impuesto = cta.ImpuestoPorc.Value.Value;
                                if (impuesto != 0)
                                {
                                    #region Valida los impuestos
                                    decimal baseValML = detail.vlrBaseML.Value.Value;
                                    decimal baseValME = detail.vlrBaseME.Value.Value;

                                    decimal impRealML = Math.Round(baseValML * impuesto / 100, 2);
                                    decimal impRealME = Math.Round(baseValME * impuesto / 100, 2);

                                    decimal valML = detail.vlrMdaLoc.Value.Value;
                                    decimal valME = detail.vlrMdaExt.Value.Value;

                                    if (cta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                    {
                                        valML *= -1;
                                        valME *= -1;
                                    }

                                    decimal difML = Math.Abs(impRealML) - Math.Abs(valML);
                                    decimal difMaxML = Math.Abs(baseValML * (Decimal)0.01 / 100);
                                    decimal difME = Math.Abs(impRealME) - Math.Abs(valME);
                                    decimal difMaxME = Math.Abs(baseValME * (Decimal)0.01 / 100);

                                    if (!biMoneda)
                                    {
                                        #region Revisa Moneda Local y Extranjera
                                        if (monedaComp == monedaLoc && Math.Abs(difML) > Math.Abs(difMaxML))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = "vlrMdaLoc";
                                            rdF.Message = DictionaryMessages.Err_Co_InvalidImpValue;
                                            rd.DetailsFields.Add(rdF);

                                            result.Result = ResultValue.NOK;
                                            isValid = false;
                                        }
                                        else if (monedaComp != monedaLoc && Math.Abs(difME) > Math.Abs(difMaxME))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = "vlrMdaExt";
                                            rdF.Message = DictionaryMessages.Err_Co_InvalidImpValue;
                                            rd.DetailsFields.Add(rdF);

                                            result.Result = ResultValue.NOK;
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Comprobante Bimoneda
                                        if (Math.Abs(difML) > Math.Abs(difMaxML))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = "vlrMdaLoc";
                                            rdF.Message = DictionaryMessages.Err_Co_InvalidImpValue;
                                            rd.DetailsFields.Add(rdF);

                                            result.Result = ResultValue.NOK;
                                            isValid = false;
                                        }
                                        if (Math.Abs(difME) > Math.Abs(difMaxME))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = "vlrMdaExt";
                                            rdF.Message = DictionaryMessages.Err_Co_InvalidImpValue;
                                            rd.DetailsFields.Add(rdF);

                                            result.Result = ResultValue.NOK;
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex5)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex5, this.UserId.ToString(), "ValidacionDetalle - Control consistencia fiscal");
                            return result;
                        }
                    }
                    #endregion

                    if (!isValid)
                    {
                        if (string.IsNullOrEmpty(rd.Message))
                            rd.Message = "NOK";
                        result.Details.Add(rd);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ValidacionDetalle");

                return result;
            }
        }

        /// <summary>
        /// Valida el conceptosaldoId y IdentificadorTR a los detalles
        /// </summary>
        /// <param name="detail">Detalle del comprobante</param>
        /// <param name="concSaldo">Concepto de saldo de la cuenta</param>
        /// <param name="concSaldoCtrlValue">Concepto de saldo por defecto</param>
        /// <returns>Valida que el concepto de saldo este correcto</returns>
        private DTO_TxResult ValidacionIdentificadorTR(DTO_ComprobanteFooter detail, DTO_glConceptoSaldo concSaldo,int numeroDoc, string prefXdef, string linPresXDef)
        {
            SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
            mySqlCommand.Transaction = base._mySqlConnectionTx;

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            result.Result = ResultValue.OK;

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string saldoVal = concSaldo.coSaldoControl.Value.Value.ToString();
                SaldoControl ctrlSaldo = (SaldoControl)Enum.Parse(typeof(SaldoControl), saldoVal);

                if (ctrlSaldo == SaldoControl.Cuenta)
                    detail.IdentificadorTR.Value = 0;
                else if (ctrlSaldo == SaldoControl.Doc_Interno)
                {
                    if (detail.DatoAdd4.Value != AuxiliarDatoAdd4.Reclasificacion.ToString())
                    {
                        //Valida el numero de documento para el concepto de saldos se usa el de la cuenta
                        DTO_glDocumentoControl doc = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(detail.IdentificadorTR.Value.Value));

                        #region Validar la info del documento control con el detalle del comprobante
                        if (doc == null || doc.TerceroID.Value != detail.TerceroID.Value || doc.ProyectoID.Value != detail.ProyectoID.Value ||
                            doc.CentroCostoID.Value != detail.CentroCostoID.Value || doc.LineaPresupuestoID.Value != detail.LineaPresupuestoID.Value ||
                            doc.PrefijoID.Value != detail.PrefijoCOM.Value || doc.DocumentoNro.Value.Value.ToString() != detail.DocumentoCOM.Value)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_InvalidDocIntData;
                        }
                        #endregion
                    }
                }
                else if (ctrlSaldo == SaldoControl.Doc_Externo || ctrlSaldo == SaldoControl.Componente_Documento)
                {
                    if (detail.DatoAdd4.Value != AuxiliarDatoAdd4.Reclasificacion.ToString())
                    {
                        //Valida el numero de documento para el concepto de saldos se usa el de la cuenta
                        DTO_glDocumentoControl doc = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(detail.IdentificadorTR.Value.Value));

                        #region Validar la info del documento control con el detalle del comprobante
                        if (doc == null || (!string.IsNullOrEmpty(doc.TerceroID.Value) && doc.TerceroID.Value != detail.TerceroID.Value) ||
                                           (!string.IsNullOrEmpty(doc.ProyectoID.Value) && doc.ProyectoID.Value != detail.ProyectoID.Value) ||
                                           (!string.IsNullOrEmpty(doc.CentroCostoID.Value) && doc.CentroCostoID.Value != detail.CentroCostoID.Value) ||
                                           (!string.IsNullOrEmpty(doc.LineaPresupuestoID.Value) && doc.LineaPresupuestoID.Value != detail.LineaPresupuestoID.Value) ||
                                           (!string.IsNullOrEmpty(doc.PrefijoID.Value) && doc.PrefijoID.Value != detail.PrefijoCOM.Value) ||
                                           (!string.IsNullOrEmpty(doc.DocumentoTercero.Value) && doc.DocumentoTercero.Value.ToLower() != detail.DocumentoCOM.Value.ToLower()))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_InvalidDocExtData;
                        }
                        #endregion
                    }
                }
                else if (ctrlSaldo == SaldoControl.Componente_Tercero)
                {
                    if (detail.DatoAdd4.Value != AuxiliarDatoAdd4.Reclasificacion.ToString())
                    {
                        //Valida el numero de documento para el concepto de saldos se usa el de la cuenta
                        DTO_glDocumentoControl doc = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);

                        #region Validar la info del documento control con el detalle del comprobante
                        if (detail.TerceroID.Value != detail.IdentificadorTR.Value.Value.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_InvalidIdentTR_Terc;
                        }
                        #endregion
                    }
                }
                else if (ctrlSaldo == SaldoControl.Componente_Activo)
                {
                    if (detail.DatoAdd4.Value != AuxiliarDatoAdd4.Reclasificacion.ToString())
                    {
                        //Valida el numero de documento para el concepto de saldos se usa el de la cuenta
                        ModuloActivosFijos moduloAC = (ModuloActivosFijos)this.GetInstance(typeof(ModuloActivosFijos), this._mySqlConnection, this.Empresa, this.UserId, this.loggerConnectionStr);
                        DTO_acActivoControl activo = moduloAC.acActivoControl_GetByID(Convert.ToInt32(detail.IdentificadorTR.Value.Value));

                        if (activo == null || activo.TerceroID.Value != detail.TerceroID.Value || activo.ProyectoID.Value != detail.ProyectoID.Value ||
                            activo.CentroCostoID.Value != detail.CentroCostoID.Value || linPresXDef != detail.LineaPresupuestoID.Value ||
                            prefXdef != detail.PrefijoCOM.Value || activo.PlaquetaID.Value.ToLower() != detail.ActivoCOM.Value.ToLower())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_InvalidActData;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloContabilidad-ValidacionIdentificadorTR");
                return result;
            }
        }

        #endregion

        #region Ajustes en Cambio

        /// <summary>
        /// Agrega los registros de ajuste en cambio para un comprobante
        /// </summary>
        /// <param name="comp">Comprobante sobre el cual se va hacer el ajuste en cambio</param>
        private DTO_TxResult AgregarAjusteEnCambio(DTO_Comprobante comp)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                if (this.Multimoneda())
                {
                    //Diccionario con el ID de la cuenta
                    Dictionary<string, Tuple<bool, DTO_coPlanCuenta>> cacheCuenta = new Dictionary<string, Tuple<bool, DTO_coPlanCuenta>>();
                    #region declaracion de variables
                    //Carga los valores por defecto (glControl)

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    //Datos por defecto
                    string defTercero = string.Empty;
                    string defPrefijo = string.Empty;
                    string defProyecto = string.Empty;
                    string defCentroCosto = string.Empty;
                    string defLineaPresupuesto = string.Empty;
                    string defConceptoCargo = string.Empty;
                    string defLugarGeo = string.Empty;
                    //Cuentas de control
                    DTO_coPlanCuenta ctaAjDebML = null;
                    DTO_coPlanCuenta ctaAjCreML = null;
                    DTO_coPlanCuenta ctaAjDebME = null;
                    DTO_coPlanCuenta ctaAjCreME = null;

                    bool ctaValid;
                    DTO_coPlanCuenta cta = new DTO_coPlanCuenta();
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    List<DTO_ComprobanteFooter> ajustes = new List<DTO_ComprobanteFooter>();
                    DTO_ComprobanteFooter ajuste;
                    DTO_ComprobanteFooter contrapartida;

                    decimal tc = 0;
                    bool isML = comp.Header.MdaOrigen.Value.Value == (int)TipoMoneda_LocExt.Local ? true : false;
                    #endregion

                    foreach (DTO_ComprobanteFooter det in comp.Footer)
                    {
                        cta = new DTO_coPlanCuenta();
                        ajuste = null;
                        contrapartida = null;
                        ctaValid = false;

                        #region Revisa si la cuenta permite hacer el ajuste en cambio
                        if (cacheCuenta.ContainsKey(det.CuentaID.Value))
                        {
                            if (cacheCuenta[det.CuentaID.Value].Item1)
                            {
                                ctaValid = true;
                                cta = cacheCuenta[det.CuentaID.Value].Item2;
                            }
                        }
                        else
                        {
                            if (det.DatoAdd4.Value != AuxiliarDatoAdd4.AjEnCambio.ToString() &&
                                det.DatoAdd4.Value != AuxiliarDatoAdd4.AjEnCambioContra.ToString() &&
                                det.DatoAdd4.Value != AuxiliarDatoAdd4.Contrapartida.ToString())
                            {
                                cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, det.CuentaID.Value, true, false);
                                DTO_glConceptoSaldo concSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                SaldoControl sControl = (SaldoControl)Enum.Parse(typeof(SaldoControl), concSaldo.coSaldoControl.Value.Value.ToString());
                                if (cta.AjCambioRealizadoInd.Value.Value && (sControl == SaldoControl.Doc_Externo || sControl == SaldoControl.Doc_Interno))
                                {
                                    ctaValid = true;
                                    if (ctaAjDebML == null)
                                    {
                                        #region carga la informacion de los datos que vienen de control
                                        //Informacion de datos por defecto
                                        defTercero = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                                        defPrefijo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                                        defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                                        defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                                        defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                                        defConceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                                        defLugarGeo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                                        //Cuenta Ajuste Debito ML
                                        string ctaDebML = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjDebML);
                                        if (string.IsNullOrWhiteSpace(ctaDebML))
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_CtaAjDebML + "&&" + string.Empty;

                                            return result;
                                        }
                                        ctaAjDebML = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaDebML, true, false);

                                        //Cuenta Ajuste Credito ML                                    
                                        string ctaCreML = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCreML);
                                        if (string.IsNullOrWhiteSpace(ctaCreML))
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_CtaAjCreML + "&&" + string.Empty;

                                            return result;
                                        }
                                        ctaAjCreML = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaCreML, true, false);


                                        //Cuenta Ajuste Debito ME
                                        string ctaDebME = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjDebME);
                                        if (string.IsNullOrWhiteSpace(ctaDebME))
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_CtaAjDebME + "&&" + string.Empty;

                                            return result;
                                        }
                                        ctaAjDebME = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaDebME, true, false);

                                        //Cuenta Ajuste Credito ME
                                        string ctaCreME = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjCreME);
                                        if (string.IsNullOrWhiteSpace(ctaCreME))
                                        {
                                            result.Result = ResultValue.NOK;
                                            result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_CtaAjCreME + "&&" + string.Empty;

                                            return result;
                                        }
                                        ctaAjCreME = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaCreME, true, false);

                                        #endregion
                                    }
                                }
                            }

                            Tuple<bool, DTO_coPlanCuenta> cacheTupla = new Tuple<bool, DTO_coPlanCuenta>(ctaValid, cta);
                            cacheCuenta.Add(det.CuentaID.Value, cacheTupla);
                        }
                        #endregion
                        if (ctaValid && det.IdentificadorTR != null && det.IdentificadorTR.Value.HasValue && det.IdentificadorTR.Value.Value != 0)
                        {
                            #region Asigna la tasa de cambio
                            ctrl = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(det.IdentificadorTR.Value.Value));
                            tc = ctrl.TasaCambioCONT.Value.Value;
                            #endregion
                            if (tc != 0 && tc != det.TasaCambio.Value.Value)
                            {
                                #region Si cumple las condiciones para crear registro de ajuste en cambio
                                Tuple<decimal, decimal> valores = this.ValoresAjusteEnCambio(tc, isML, det.vlrMdaLoc.Value.Value, det.vlrMdaExt.Value.Value);
                                ajuste = this.CrearAjusteEnCambio(false, tc, valores.Item1, valores.Item2, det, cta, defTercero, defPrefijo, defProyecto,
                                    defCentroCosto, defLineaPresupuesto, defConceptoCargo, defLugarGeo);

                                if (isML)
                                {
                                    if (det.vlrMdaLoc.Value.Value > 0)
                                        contrapartida = this.CrearAjusteEnCambio(true, tc, (valores.Item1 * -1), (valores.Item2 * -1), det, ctaAjDebML, defTercero, defPrefijo,
                                            defProyecto, defCentroCosto, defLineaPresupuesto, defConceptoCargo, defLugarGeo);
                                    else
                                        contrapartida = this.CrearAjusteEnCambio(true, tc, (valores.Item1 * -1), (valores.Item2 * -1), det, ctaAjCreML, defTercero, defPrefijo,
                                            defProyecto, defCentroCosto, defLineaPresupuesto, defConceptoCargo, defLugarGeo);
                                }
                                else
                                {
                                    if (det.vlrMdaExt.Value.Value > 0)
                                        contrapartida = this.CrearAjusteEnCambio(true, tc, (valores.Item1 * -1), (valores.Item2 * -1), det, ctaAjDebME, defTercero, defPrefijo,
                                            defProyecto, defCentroCosto, defLineaPresupuesto, defConceptoCargo, defLugarGeo);
                                    else
                                        contrapartida = this.CrearAjusteEnCambio(true, tc, (valores.Item1 * -1), (valores.Item2 * -1), det, ctaAjCreME, defTercero, defPrefijo,
                                            defProyecto, defCentroCosto, defLineaPresupuesto, defConceptoCargo, defLugarGeo);
                                }

                                ajustes.Add(ajuste);
                                ajustes.Add(contrapartida);
                                #endregion
                            }
                        }
                    }

                    foreach (DTO_ComprobanteFooter aj in ajustes)
                        comp.Footer.Add(aj);
                }


                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AgregarAjusteEnCambio");

                return result;
            }
        }

        /// <summary>
        /// Crea los registros para el ajuste en cambio
        /// </summary>
        /// <returns>Retorna una lista con los 2 registros que se deben agregar para el ajuste en cambio</returns>
        /// Item1: vlrAjML
        /// Item2: vlrAjME
        private Tuple<decimal, decimal> ValoresAjusteEnCambio(decimal tc, bool isML, decimal vlrML, decimal vlrME)
        {
            decimal vlrMda = 0;
            decimal vlrOtr = 0;
            decimal vlrDoc = 0;
            decimal ajuLoc = 0;
            decimal ajuExt = 0;

            if (isML)
            {
                vlrMda = vlrML;
                vlrOtr = vlrME;
                vlrDoc = Math.Round(vlrMda / tc, 2);
                ajuLoc = 0;
                ajuExt = vlrDoc - vlrOtr;
            }
            else
            {
                vlrMda = vlrME;
                vlrOtr = vlrML;
                vlrDoc = Math.Round(vlrMda * tc, 2);
                ajuLoc = vlrDoc - vlrOtr;
                ajuExt = 0;
            }

            return new Tuple<decimal, decimal>(ajuLoc, ajuExt);
        }

        /// <summary>
        /// Crea el detalle de un comprobante dado una cuenta
        /// </summary>
        private DTO_ComprobanteFooter CrearAjusteEnCambio(bool isContra, decimal tc, decimal vlrAjML, decimal vlrAjME, DTO_ComprobanteFooter det, DTO_coPlanCuenta cta, string defTercero,
            string defPrefijo, string defProyecto, string defCentroCosto, string defLineaPresupuesto, string defConceptoCargo, string defLugarGeo)
        {
            DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
            string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            newDet.CuentaID.Value = cta.ID.Value;
            #region Valores por defecto
            #region Tercero
            if (isContra)
                newDet.TerceroID.Value = terceroPorDefecto;
            else
                newDet.TerceroID.Value = det.TerceroID.Value;
            #endregion
            #region Proyecto
            if (!cta.ProyectoInd.Value.Value)
                newDet.ProyectoID.Value = defProyecto;
            else
                newDet.ProyectoID.Value = det.ProyectoID.Value;
            #endregion
            #region Centro Costo
            if (!cta.CentroCostoInd.Value.Value)
                newDet.CentroCostoID.Value = defCentroCosto;
            else
                newDet.CentroCostoID.Value = det.CentroCostoID.Value;
            #endregion
            #region Linea presupuesto
            if (!cta.LineaPresupuestalInd.Value.Value)
                newDet.LineaPresupuestoID.Value = defLineaPresupuesto;
            else
                newDet.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
            #endregion
            #region Concepto Cargo
            if (!cta.ConceptoCargoInd.Value.Value)
                newDet.ConceptoCargoID.Value = defConceptoCargo;
            else
                newDet.ConceptoCargoID.Value = det.ConceptoCargoID.Value;
            #endregion
            #region Lugar Geografico
            if (!cta.LugarGeograficoInd.Value.Value)
                newDet.LugarGeograficoID.Value = defLugarGeo;
            else
                newDet.LugarGeograficoID.Value = det.LugarGeograficoID.Value;
            #endregion
            #region Prefijo
            newDet.PrefijoCOM.Value = det.PrefijoCOM.Value;
            #endregion
            #endregion
            #region Datos del registro actual
            newDet.CuentaID.Value = cta.ID.Value;
            newDet.DocumentoCOM.Value = det.DocumentoCOM.Value;
            newDet.ActivoCOM.Value = det.ActivoCOM.Value;
            newDet.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
            newDet.IdentificadorTR.Value = isContra ? 0 : det.IdentificadorTR.Value;
            newDet.Descriptivo.Value = det.Descriptivo.Value;
            #endregion
            #region Nuevos valores
            newDet.TasaCambio.Value = tc;
            newDet.vlrBaseML.Value = 0; //cta.ImpuestoTipoID != null && !string.IsNullOrWhiteSpace(cta.ImpuestoTipoID.Value) ? 1 : 0;
            newDet.vlrBaseME.Value = 0; //cta.ImpuestoTipoID != null && !string.IsNullOrWhiteSpace(cta.ImpuestoTipoID.Value) ? 1 : 0;
            newDet.vlrMdaLoc.Value = vlrAjML;
            newDet.vlrMdaExt.Value = vlrAjME;
            newDet.vlrMdaOtr.Value = 0;
            newDet.DatoAdd4.Value = isContra ? AuxiliarDatoAdd4.AjEnCambioContra.ToString() : AuxiliarDatoAdd4.AjEnCambio.ToString();
            #endregion

            return newDet;
        }

        #endregion

        #region Otras

        /// <summary>
        /// Aprueba un comprobante
        /// </summary>
        /// <param name="currentMod">Modulo sobre el que se esta trabajando</param>
        /// <param name="numeroDoc">ID de glDocumentoControl</param>
        /// <param name="isPre">Indica si trae los datos del preAuxiliar</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <param name="obs">Observacion al aprobar</param>
        /// <param name="updDocCtrl">Indica si se debe actualizar el documentoControl</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        internal DTO_TxResult Comprobante_Aprobar(int documentID, string actividadFlujoID, ModulesPrefix currentMod, int numeroDoc, bool isPre, DateTime periodo, string comprobanteID,
            int compNro, string obs, bool updDocCtrl, bool createDoc, bool asignarFlujo, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            try
            {
                //Actualiza glDocumentoControl
                int bitacoraOrigen = 0;
                if (updDocCtrl)
                    bitacoraOrigen = this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, obs, true);

                //Verifica si el comprobante existe
                DTO_Comprobante comprobante = this.Comprobante_Get(true, isPre, periodo, comprobanteID, compNro, null, null);
                if (comprobante != null)
                {
                    //Agrega el auxiliar
                    this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_Comprobante.DAL_Comprobante_AgregarAuxFromPre(periodo, comprobanteID, compNro);

                    //Actualiza saldos
                    DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteID, true, false);
                    result = this.GenerarSaldos(documentID, currentMod, comprobante, bitacoraOrigen);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    //Elimina los datos del auxiliar pre
                    this._dal_Comprobante.DAL_Comprobante_BorrarAuxiliar_Pre(comprobante.Header.PeriodoID.Value.Value,
                        comprobante.Header.ComprobanteID.Value, comprobante.Header.ComprobanteNro.Value.Value);

                    //Asigna el nuevo flujo
                    if (asignarFlujo && !string.IsNullOrWhiteSpace(actividadFlujoID))
                    {
                        result = this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, false, obs);
                        if (result.Result == ResultValue.NOK)
                            return result;
                    }

                    //Genera el archivo
                    if (createDoc)
                    {
                        try
                        {
                            DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                            #region Genera el nuevo archivo
                            #region Comprobante manual
                            //if (docCtrl.DocumentoID.Value.Value == AppDocuments.ComprobanteManual)
                            //{
                            //    DTO_ReportComprobante2 rComp = this.DtoComprobanteReport(comprobante, docCtrl);
                            //    this.GenerarArchivo(documentID, numeroDoc, rComp);
                            //}
                            #endregion
                            #region Cruce cuentas
                            if (docCtrl.DocumentoID.Value.Value == AppDocuments.CruceCuentas)
                                this.GenerarArchivo(documentID, numeroDoc, this.DtoDocumentoContableReport(comprobante, docCtrl));
                            #endregion
                            #endregion
                        }
                        catch (Exception e1)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                            return result;
                        }
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Rechaza un comprobante
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <param name="obs">Observacion sobre el documento</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        internal void Comprobante_Rechazar(int documentID, string actividadFlujoID, int ctrlDocID, int numeroDoc, DateTime periodo, string comprobanteID, int compNro, string obs, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isValid = true;
            try
            {
                //Verifica si el comprobante existe
                bool compExists = this.ComprobantePre_Exists(ctrlDocID, periodo, comprobanteID, compNro);
                if (compExists)
                {
                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.SinAprobar, obs, true);
                    this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, true, obs);
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteCompr, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "RechazarComprobante");
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
        /// Contabiliza un comprobante
        /// </summary>
        /// <param name="documentID">Documento que realiza la contabilizacion</param>
        /// <param name="comp">Comprobante a contabilizar</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="mod">Modulo que realiza el proceso de contabilizacion</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="isBalancePre">Indica si el balance es preliminar</param>
        /// <param name="bitacoraOrigen">Indica si la operacion fue iniciada por un proceso padre (bitacoraID)</param>
        /// <param name="mayorizar">Indica si el proceso tambien debe mayorizar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        internal DTO_TxResult ContabilizarComprobante(int documentID, DTO_Comprobante comp, DateTime periodo, ModulesPrefix mod, int bitacoraOrigen, bool mayorizar)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                int numeroDoc = comp.Header.NumeroDoc.Value.Value;
                string libroID = this.GetLibroComprobante(comp.Header);

                // Asigna los registros de Ajuste en Cambio Realizado
                if (documentID != AppDocuments.ComprobanteAjusteCambio)
                {
                    result = this.AgregarAjusteEnCambio(comp);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }

                //Guarda el auxiliar
                this.AgregarAuxiliar(comp);

                //Genera los nuevos saldos
                result = this.GenerarSaldos(documentID, mod, comp, bitacoraOrigen);
                if (result.Result == ResultValue.NOK)
                    return result;

                //Realiza la mayorizacion
                if (mayorizar)
                    result = this.Proceso_Mayorizar(documentID, periodo, libroID, new Dictionary<Tuple<int, int>, int>(), true);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ContabilizarComprobante");

                return result;
            }
        }

        /// <summary>
        /// Actualiza los consecutivos del comprobante
        /// </summary>
        /// <param name="isPre">Verifica si la actualizacion se esta haciendo en el auxiliar Preliminar o en el real</param>
        /// <param name="numeroDoc">Identificador del documento control</param>
        /// <param name="comprobanteNro">Consecutivo del comprobante</param>
        internal void ActualizaComprobanteNro(int numeroDoc, int comprobanteNro, bool isPre, string comprobanteID = null)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante.DAL_Comprobante_UpdateConsecutivo(numeroDoc, comprobanteNro, isPre, comprobanteID);
        }

        /// <summary>
        /// Agrega registros a la tabla de auxiliar
        /// </summary>
        /// <param name="isNew">Indica si el comprobante (pre) es nuevo o si se esta actualizando</param>
        /// <param name="comprobante">Comprobante contable</param>
        internal DTO_TxResult AgregarAuxiliar_Pre(int documentoID, bool isNew, DTO_Comprobante comprobante, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isValid = true;
            List<DTO_ComprobanteFooter> footer = comprobante.Footer;
            DTO_TxResult result = new DTO_TxResult();

            try
            {
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_glConceptoSaldo saldo = null;
                Dictionary<string, DTO_glConceptoSaldo> cacheConceptoSaldo = new Dictionary<string, DTO_glConceptoSaldo>();

                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);
                string prefXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string linPresXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int count = this._dal_Contabilidad.DAL_Contabilidad_SaldoExistsPreliminares(tipoBalancePreliminar);
                if (count > 0)
                {
                    isValid = false;
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_BalPre;
                    return result;
                }

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!isNew)
                {
                    this._dal_Comprobante.DAL_Comprobante_BorrarAuxiliar_Pre(comprobante.Header.PeriodoID.Value.Value,
                        comprobante.Header.ComprobanteID.Value, comprobante.Header.ComprobanteNro.Value.Value);
                }

                int i = 0;
                foreach (DTO_ComprobanteFooter det in footer)
                {
                    ++i;
                    #region Generacion concepto saldo y validación identificador Tr

                    if (cacheConceptoSaldo.ContainsKey(det.ConceptoSaldoID.Value))
                        saldo = cacheConceptoSaldo[det.ConceptoSaldoID.Value];
                    else
                    {
                        saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, det.ConceptoSaldoID.Value, true, false);
                        cacheConceptoSaldo.Add(det.ConceptoSaldoID.Value, saldo);
                    }

                    //Revisa si toca asignar el identificadorTR
                    if (det.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString())
                    {
                        int identTR = comprobante.Header.NumeroDoc.Value.Value;
                        if (documentoID == AppDocuments.CruceCuentas)
                        {
                            DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, comprobante.Observacion, true, true);
                            DTO_glDocumentoControl ctrl;
                            string[] docs = det.DocumentoCOM.Value.Split('*');

                            if (doc.DocExternoInd.Value.Value)
                            {
                                ctrl = this._moduloGlobal.glDocumentoControl_GetExternalDocByCta(det.CuentaID.Value, det.TerceroID.Value, docs[0]);
                                det.PrefijoCOM = ctrl.PrefijoID;

                                identTR = ctrl.NumeroDoc.Value.Value;
                            }
                            else
                            {
                                int docNro = Convert.ToInt32(docs[1]);
                                ctrl = this._moduloGlobal.glDocumentoControl_GetInternalDocByCta(det.CuentaID.Value, det.PrefijoCOM.Value, docNro);
                                det.TerceroID = ctrl.TerceroID;

                                identTR = ctrl.NumeroDoc.Value.Value;
                            }
                        }

                        if (documentoID == AppDocuments.CruceCuentas)
                        {
                            string[] docs = det.DocumentoCOM.Value.Split('*');
                            det.DocumentoCOM.Value = docs[0];
                        }

                        det.IdentificadorTR.Value = identTR;
                    }

                    DTO_TxResult validateTR = this.ValidacionIdentificadorTR(det, saldo,comprobante.Header.NumeroDoc.Value.Value, prefXDef, linPresXDef);
                    if (validateTR.Result == ResultValue.NOK)
                    {
                        isValid = false;

                        DTO_TxResultDetail resDetail = new DTO_TxResultDetail();
                        resDetail.line = i;
                        resDetail.Message = validateTR.ResultMessage;

                        result.Result = ResultValue.NOK;
                        result.Details.Add(resDetail);
                    }

                    if (isValid)
                        this._dal_Comprobante.DAL_Comprobante_AgregarAuxiliar_Pre(isNew, comprobante, saldo, footer.IndexOf(det));

                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                isValid = false;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AgregarAuxiliar_Pre");
                return result;
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
        /// Borra registros de la tabla de auxiliar
        /// </summary>
        /// <param name="isNew">Indica si el comprobante (pre) es nuevo o si se esta actualizando</param>
        /// <param name="comprobante">Comprobante contable</param>
        /// <param name="compNro">Comprobante nro</param>
        /// <param name="numeroDoc">id del doc si existe</param>
        internal void BorrarAuxiliar_Pre(DateTime periodo, string comprobanteID, int compNro, int numeroDoc = 0)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante.DAL_Comprobante_BorrarAuxiliar_Pre(periodo, comprobanteID, compNro, numeroDoc);
        }

        /// <summary>
        /// Agrega registros a la tabla de auxiliar
        /// El tipo de balance saca por defecto el de coComprobante, a menos que este en el header (LibroID)
        /// </summary>
        /// <param name="comprobante">Comprobante contable</param>
        internal void AgregarAuxiliar(DTO_Comprobante comprobante)
        {
            try
            {
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Comprobante.DAL_Comprobante_AgregarAuxiliar(comprobante);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AgregarAuxiliar");
                throw ex;
            }
        }

        /// <summary>
        /// Crea la estructura de un comprobante q se debe eliminar
        /// </summary>
        /// <param name="header">Cabezote del comprobante</param>
        /// <returns>Retorna el comprobante a guardar</returns>
        internal DTO_Comprobante CreateEmptyAux(DTO_ComprobanteHeader header)
        {
            DTO_Comprobante result = new DTO_Comprobante();
            result.Header = header;
            result.Header.TasaCambioBase.Value = 0;
            result.Header.TasaCambioOtr.Value = 0;

            DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
            footer.TasaCambio.Value = 0;
            footer.CuentaID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaXDefecto);
            footer.TerceroID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            footer.ProyectoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            footer.CentroCostoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            footer.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            footer.ConceptoCargoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            footer.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            footer.PrefijoCOM.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            footer.DocumentoCOM.Value = string.Empty;
            footer.ActivoCOM.Value = string.Empty;
            footer.ConceptoSaldoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
            footer.IdentificadorTR.Value = 0;
            footer.Descriptivo.Value = "ANULADO";
            footer.vlrBaseML.Value = 0;
            footer.vlrBaseME.Value = 0;
            footer.vlrMdaLoc.Value = 0;
            footer.vlrMdaExt.Value = 0;
            footer.vlrMdaOtr.Value = 0;
            footer.DatoAdd1.Value = string.Empty;
            footer.DatoAdd2.Value = string.Empty;
            footer.DatoAdd3.Value = string.Empty;
            footer.DatoAdd4.Value = string.Empty;

            result.Footer.Add(footer);

            return result;
        }

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual a correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        internal bool Comprobante_ExistByIdentificadorTR(DateTime periodo, long identTR, bool isPre)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool exist = this._dal_Comprobante.DAL_Comprobante_ExistByIdentificadorTR(periodo, identTR, isPre);
            return exist;
        }

        /// <summary>
        /// Trae la lista de comprobantes relacionados con un solo numeroDoc
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        /// <returns></returns>
        internal List<Tuple<string, int>> Comprobante_GetComprobantesByNumeroDoc(int numeroDoc)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_GetComprobantesByNumeroDoc(numeroDoc);
        }

        /// <summary>
        /// Revierte un comprobante
        /// </summary>
        /// <param name="ctrlDocID">DocumentoID de glDocumentoControl</param>
        /// <param name="periodoOld">Periodo</param>
        /// <param name="comprobanteIDOld">Codigo del comprobante</param>
        /// <param name="compNroOld">Consecutivo del comprobante</param>
        internal DTO_TxResult Comprobante_Revertir(DTO_glDocumentoControl ctrl, DTO_Comprobante compOld, ModulesPrefix mod, ref DTO_coComprobante coComp)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                DTO_Comprobante comp = null;

                //Info de la fecha
                DateTime periodoOld = compOld.Header.PeriodoID.Value.Value;
                DateTime periodo = ctrl.PeriodoDoc.Value.Value;
                DateTime fecha = DateTime.Now;
                if (fecha.Year != periodo.Year || fecha.Month != periodo.Month)
                    fecha = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));


                #endregion
                #region Verifica que exista un comprobante de anulacion

                //Comprobante original
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compOld.Header.ComprobanteID.Value, true, false);
                if (string.IsNullOrWhiteSpace(coComp.ComprobanteAnulID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_NoCompAnula + "&&" + coComp.ID.Value;
                    return result;
                }

                //Comprobante de anulacion
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coComp.ComprobanteAnulID.Value, true, false);

                #endregion
                #region Revisa que no haya info con el tipo de balance preliminar
                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);
                int count = this._dal_Contabilidad.DAL_Contabilidad_SaldoExistsPreliminares(tipoBalancePreliminar);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_BalPre;
                    return result;
                }
                #endregion
                #region Genera el comprobante
                comp = ObjectCopier.Clone(compOld);
                comp.Header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                comp.Header.ComprobanteID.Value = coComp.ID.Value;
                comp.Header.ComprobanteNro.Value = 0;
                comp.Header.PeriodoID.Value = periodo;
                comp.Header.Fecha.Value = fecha;

                foreach (DTO_ComprobanteFooter det in comp.Footer)
                {
                    det.vlrBaseML.Value = det.vlrBaseML.Value * -1;
                    det.vlrBaseME.Value = det.vlrBaseME.Value * -1;
                    det.vlrMdaLoc.Value = det.vlrMdaLoc.Value * -1;
                    det.vlrMdaExt.Value = det.vlrMdaExt.Value * -1;
                    det.vlrMdaOtr.Value = det.vlrMdaOtr.Value * -1;

                    string prefDesc = "(Rev.Comp " + periodoOld.Year.ToString() + "-" + periodoOld.Month.ToString() + ":" + compOld.Header.ComprobanteID.Value + ") ";
                    var descrip = prefDesc + det.Descriptivo.Value;
                    det.Descriptivo.Value = descrip.Substring(0, Math.Min(descrip.Length, 99));
                }

                #endregion
                #region Contabiliza el comprobante

                result = this.ContabilizarComprobante(ctrl.DocumentoID.Value.Value, comp, periodoOld, mod, 0, false);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_Revertir");

                return result;
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Crea un dto de reporte para Documento comtable 
        /// </summary>
        /// <param name="data">DTO_Comprobante</param>
        /// <param name="docCtrl">DTO_glDocumentoControl</param>
        /// <returns></returns>
        private object DtoDocumentoContableReport(DTO_Comprobante data, DTO_glDocumentoControl docCtrl)
        {
            try
            {
                #region Obtener datos para el reporte
                int docRepID = docCtrl.DocumentoID.Value.Value;

                //int coDocLength = Convert.ToInt32(docCtrl.Observacion.Value.Substring(0, 2));
                //string coDocID = docCtrl.Observacion.Value.Substring(2, coDocLength);

                DTO_ReportDocumentoContable docContableReport = new DTO_ReportDocumentoContable();
                docContableReport.CentroCostoID = docCtrl.CentroCostoID.Value;
                //docContableReport.coDocumentoID = coDocID;
                docContableReport.CuentaID = docCtrl.CuentaID.Value;
                docContableReport.DocumentoTercero = docCtrl.DocumentoTercero.Value;
                docContableReport.LineaPresupuestoID = docCtrl.LineaPresupuestoID.Value;
                docContableReport.LugarGeograficoID = docCtrl.LugarGeograficoID.Value;
                //docContableReport.Observacion = docCtrl.Observacion.Value.Substring(coDocLength + 2);
                docContableReport.ProyectoID = docCtrl.ProyectoID.Value;
                docContableReport.TerceroID = docCtrl.TerceroID.Value;
                docContableReport.Header = data.Header;
                docContableReport.DocumentoNro = data.Header.ComprobanteNro.Value.Value.ToString();
                docContableReport.DocumentoEstado = ((EstadoDocControl)docCtrl.Estado.Value).ToString();
                docContableReport.DescMonedaOrigen = ((TipoMoneda)Convert.ToInt16(data.Header.MdaOrigen.Value)).ToString();
                if ((TipoMoneda)Convert.ToInt16(data.Header.MdaOrigen.Value) == TipoMoneda.Local)
                    docContableReport.ValorDoc = (-1) * data.Footer.Last().vlrMdaLoc.Value.Value;
                else if ((TipoMoneda)Convert.ToInt16(data.Header.MdaOrigen.Value) == TipoMoneda.Foreign)
                    docContableReport.ValorDoc = (-1) * data.Footer.Last().vlrMdaExt.Value.Value;

                DTO_glDocumento glDocInfo = null;
                DTO_coDocumento coDocInfo = null;
                DTO_coDocumentoAjuste docAj = null;
                if (docRepID == AppDocuments.DocumentoContable)
                {
                    coDocInfo = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, "01201",/*docContableReport.coDocumentoID, */true, false);
                    docContableReport.DescDocumento = ((DTO_coDocumento)coDocInfo).Descriptivo.ToString();
                }
                else
                {
                    glDocInfo = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, docContableReport.coDocumentoID, true, false);
                    docContableReport.DescDocumento = ((DTO_glDocumento)glDocInfo).Descriptivo.ToString();
                    docAj = this.coDocumentoAjuste_Get(docCtrl.NumeroDoc.Value.Value);
                    DTO_glDocumentoControl docCtrlAj = _moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(docAj.IdentificadorTR.Value.Value));
                    docContableReport.DocAjustadoID = docCtrlAj.DocumentoID.Value.ToString();
                    coDocInfo = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContableReport.DocAjustadoID, true, false);
                    docContableReport.DocAjustadoDesc = ((DTO_coDocumento)coDocInfo).Descriptivo.ToString();
                };

                DTO_coPlanCuenta cuentaInfo = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, docContableReport.CuentaID, true, false);
                docContableReport.DescCuenta = ((DTO_coPlanCuenta)cuentaInfo).Descriptivo.ToString();

                DTO_coCentroCosto centroCostoInfo = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, docContableReport.CentroCostoID, true, false);
                docContableReport.DescCentroCosto = ((DTO_coCentroCosto)centroCostoInfo).Descriptivo.ToString();

                DTO_coTercero terceroInfo = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, docContableReport.TerceroID, true, false);
                docContableReport.DescTercero = ((DTO_coTercero)terceroInfo).Descriptivo.ToString();

                DTO_coProyecto proyectoInfo = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, docContableReport.ProyectoID, true, false);
                docContableReport.DescProyecto = ((DTO_coProyecto)proyectoInfo).Descriptivo.ToString();

                DTO_glLugarGeografico lugarGeograficoInfo = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, docContableReport.LugarGeograficoID, true, false);
                docContableReport.DescLugarGeografico = ((DTO_glLugarGeografico)lugarGeograficoInfo).Descriptivo.ToString();

                docContableReport.DescMonedaTransac = ((DTO_MasterBasic)(this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glMoneda, docContableReport.Header.MdaTransacc.Value, true, false))).Descriptivo.ToString();

                docContableReport.footerReport = new List<DTO_ReportDocumentoContableFooter>();
                foreach (DTO_ComprobanteFooter footer in data.Footer)
                {
                    DTO_ReportDocumentoContableFooter docContableFooter = new DTO_ReportDocumentoContableFooter(footer);
                    DTO_coPlanCuenta cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, footer.CuentaID.Value, true, false);
                    docContableFooter.Debito = cuenta.Naturaleza.Value.Equals((byte)NaturalezaCuenta.Debito);
                    docContableReport.footerReport.Add(docContableFooter);
                };

                #endregion
                return docContableReport;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoDocumentoContableReport");
                return null;
            }
        }

        /// <summary>
        /// Crea un dto de reporte para el comprobante dado un comprobante
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private DTO_ReportComprobante2 DtoComprobanteReport(DTO_Comprobante comp, DTO_glDocumentoControl docCtrl)
        {
            try
            {
                DTO_ReportComprobante2 compReport = new DTO_ReportComprobante2();
                compReport.Header = comp.Header;
                compReport.Estado = ((EstadoDocControl)docCtrl.Estado.Value).ToString();
                compReport.footerReport = new List<DTO_ReportComprobanteFooter>();

                DTO_coPlanCuenta cta = new DTO_coPlanCuenta();
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                foreach (DTO_ComprobanteFooter f in comp.Footer)
                {
                    DTO_ReportComprobanteFooter f2 = new DTO_ReportComprobanteFooter(f);
                    if (cacheCtas.ContainsKey(f.CuentaID.Value))
                        cta = cacheCtas[f.CuentaID.Value];
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, f.CuentaID.Value, true, false);
                        cacheCtas.Add(f.CuentaID.Value, cta);
                    }

                    f2.Debito = cta.Naturaleza.Value.Equals((byte)NaturalezaCuenta.Debito);
                    compReport.footerReport.Add(f2);
                }
                return compReport;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoComprobanteReport");
                return null;
            }
        }

        #endregion

        #endregion

        #region AuxiliarPre

        /// <summary>
        /// Indica si hay un auxiliarPre
        /// </summary>
        /// <param name="empresaID">Codigo de la empresa</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public bool ComprobantePre_Exists(int ctrlDocID, DateTime periodo, string comprobanteID, int compNro)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByComprobante(ctrlDocID, periodo, comprobanteID, compNro);

            if (docCtrl == null || docCtrl.DocumentoID.Value.Value != ctrlDocID)
                return false;

            return this._dal_Comprobante.DAL_ComprobantePre_Exists(ctrlDocID, periodo, comprobanteID, compNro);
        }

        /// <summary>
        /// Indica si hay un comprobante en auxiliarPre
        /// </summary>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <returns>Retorna si hay o no un comprobante en la tabla auxiliarPre</returns>
        public bool ComprobanteExistsInAuxPre(string comprobanteID)
        {
            bool hay = true;
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            int count = this._dal_Comprobante.DAL_ComprobanteExistsInAuxPre(comprobanteID);
            if (count == 0)
                hay = false;
            return hay;
        }

        /// <summary>
        /// Agrega un comprobante (siempre debe estar asociado a un docCtrl. Si no es asi acá se debe crear)
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion</param>
        /// <param name="currentMod">Modulo que ejecuta la operacion</param>
        /// <param name="comprobante">Comprobante con los datos</param>
        /// <param name="areaFuncionalID">Identificador del area funcional</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="updDocCtrl">Indica si se debe actualizar la informacion de glDocumentoControl</param>
        /// <param name="numeroDoc">Pk de glDocumentoControl (Null: El comprobante es el encargado de generar el registro, siempre y cuando el parametro 'updDocCtrl' este habilitado)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ComprobantePre_Add(int documentID, ModulesPrefix currentMod, DTO_Comprobante comprobante, string areaFuncionalID, string prefijoID,
            bool updDocCtrl, int? numeroDoc, DTO_coDocumentoRevelacion revelacion, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isNew = false;
            DTO_glDocumentoControl docCtrl = null;
            DTO_coComprobante comp = null;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 3;

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobante.Header.ComprobanteID.Value, true, false);
                bool compExists = this.ComprobantePre_Exists(documentID, comprobante.Header.PeriodoID.Value.Value, comprobante.Header.ComprobanteID.Value, comprobante.Header.ComprobanteNro.Value.Value);
                #endregion
                #region Valida que si no actualiza el glDocCtrl el comprobante debe existir
                if (!updDocCtrl && !compExists && !insideAnotherTx)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "Error (ComprobantePre_Add - ln 2778): No se le esta asignando el comprobante al documento";
                    return result;
                }
                #endregion
                #region Carga el documento control
                if (numeroDoc.HasValue)
                {
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc.Value);
                    #region revisa que exista el glDocumentoControl
                    if (docCtrl == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_GettingData;
                        return result;
                    }
                    #endregion
                    #region Revisa que no haya sido aprobado, anulado o revertido
                    EstadoDocControl est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    if (est == EstadoDocControl.Anulado || est == EstadoDocControl.Aprobado || est == EstadoDocControl.Cerrado || est == EstadoDocControl.Devuelto)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_DocProcessed;
                        return result;
                    }
                    #endregion
                }
                #endregion
                #region Revisa que no haya info con el tipo de balance preliminar
                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int count = this._dal_Contabilidad.DAL_Contabilidad_SaldoExistsPreliminares(tipoBalancePreliminar);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_BalPre;
                    return result;
                }
                #endregion
                #region Valida la informacion principal del comprobante
                result = this.ValidacionHeader(documentID, comprobante.Header, comprobante.Footer, currentMod);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                if (updDocCtrl)
                {
                    string obs = comprobante.Footer.First().Descriptivo.Value;
                    if (numeroDoc.HasValue)
                    {
                        #region Comprobante generado por un documento existente
                        if (!compExists)
                        {
                            isNew = true;
                            docCtrl.Descripcion.Value = obs;
                            docCtrl.ComprobanteID.Value = comprobante.Header.ComprobanteID.Value;
                            docCtrl.ComprobanteIDNro.Value = 0;
                            if (documentID != AppDocuments.CajaMenor && documentID != AppDocuments.LegalizacionGastos)
                                this._moduloGlobal.glDocumentoControl_Update(docCtrl, true, true);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Documento a cargo del comprobante
                        if (compExists)
                        {
                            isNew = false;
                            docCtrl = this._moduloGlobal.glDocumentoControl_GetByComprobante(documentID, comprobante.Header.PeriodoID.Value.Value, comprobante.Header.ComprobanteID.Value, comprobante.Header.ComprobanteNro.Value.Value);
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, string.Empty, true);
                            comprobante.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value.Value;
                        }
                        else
                        {
                            isNew = true;
                            #region Agregar registro a glDocumentoControl
                            docCtrl = new DTO_glDocumentoControl();
                            //Campos Principales
                            docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                            docCtrl.DocumentoID.Value = documentID;
                            docCtrl.DocumentoTipo.Value = (byte)comprobante.TipoDoc;
                            docCtrl.FechaDoc.Value = comprobante.Header.Fecha.Value;
                            docCtrl.PeriodoDoc.Value = comprobante.Header.PeriodoID.Value;
                            docCtrl.PeriodoUltMov.Value = comprobante.Header.PeriodoID.Value;
                            docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                            docCtrl.PrefijoID.Value = prefijoID;
                            docCtrl.DocumentoNro.Value = 0;
                            docCtrl.MonedaID.Value = comprobante.Header.MdaTransacc.Value;
                            docCtrl.TasaCambioDOCU.Value = comprobante.Header.TasaCambioBase.Value;
                            docCtrl.TasaCambioCONT.Value = comprobante.Header.TasaCambioBase.Value;
                            docCtrl.ComprobanteID.Value = comprobante.Header.ComprobanteID.Value;
                            docCtrl.ComprobanteIDNro.Value = 0;
                            docCtrl.TerceroID.Value = comprobante.TerceroID;
                            docCtrl.DocumentoTercero.Value = comprobante.DocumentoTercero;
                            docCtrl.CuentaID.Value = comprobante.CuentaID;
                            docCtrl.Descripcion.Value = obs;

                            string defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                            string defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                            string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                            string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                            if (comprobante.ProyectoID == null)
                                docCtrl.ProyectoID.Value = defProyecto;
                            else
                                docCtrl.ProyectoID.Value = comprobante.ProyectoID;

                            if (comprobante.CentroCostoID == null)
                                docCtrl.CentroCostoID.Value = defCentroCosto;
                            else
                                docCtrl.CentroCostoID.Value = comprobante.CentroCostoID;

                            if (comprobante.LineaPresupuestoID == null)
                                docCtrl.LineaPresupuestoID.Value = defLineaPresupuesto;
                            else
                                docCtrl.LineaPresupuestoID.Value = comprobante.LineaPresupuestoID;

                            if (comprobante.LugarGeograficoID == null)
                                docCtrl.LugarGeograficoID.Value = defLugarGeografico;
                            else
                                docCtrl.LugarGeograficoID.Value = comprobante.LugarGeograficoID;

                            docCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            docCtrl.DocumentoAnula.Value = null;
                            docCtrl.PeriodoAnula.Value = comprobante.Header.PeriodoID.Value;

                            DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                            if (resultGLDC.Message != ResultValue.OK.ToString())
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = "NOK";
                                result.Details.Add(resultGLDC);

                                return result;
                            }

                            docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            comprobante.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value.Value;
                            #endregion
                        }

                        #endregion
                    }
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                // Asigna los registros de Ajuste en Cambio Realizado
                result = this.AgregarAjusteEnCambio(comprobante);
                if (result.Result == ResultValue.NOK)
                    return result;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result = this.AgregarAuxiliar_Pre(documentID, isNew, comprobante, true);

                if (result.Result == ResultValue.NOK)
                    return result;

                //Agrega el documento de revelacion
                if (revelacion != null)
                {
                    revelacion.EmpresaID.Value = this.Empresa.ID.Value;
                    revelacion.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    result = this.DocumentoRevelacion_Add(revelacion);

                    if (result.Result == ResultValue.NOK)
                        return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ComprobantePre_Add");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Commit y generacion de consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        bool updDocNro = false;
                        bool updCompNro = false;
                        if (updDocCtrl && docCtrl.DocumentoTipo.Value.Value == (short)DocumentoTipo.DocInterno)
                        {
                            updDocNro = true;
                            docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                        }

                        if (comprobante.Header.ComprobanteNro.Value.Value == 0)
                        {
                            updCompNro = true;

                            docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, docCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, docCtrl.DocumentoNro.Value.Value);
                            comprobante.Header.ComprobanteNro.Value = docCtrl.ComprobanteIDNro.Value.Value;

                            this.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, comprobante.Header.ComprobanteNro.Value.Value, true);
                        }

                        if (updDocNro || updCompNro)
                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, updDocNro, updCompNro, true);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            if (documentID == AppDocuments.CruceCuentas)
                result.ResultMessage = DictionaryMessages.Co_NumberDoc + "&&" + docCtrl.NumeroDoc;
            else
                result.ResultMessage = DictionaryMessages.Co_NumberComp + "&&" + comprobante.Header.ComprobanteID.Value + "&&" + comprobante.Header.ComprobanteNro.Value.Value.ToString();

            return result;
        }

        /// <summary>
        /// Elimina un auxiliar (pre) y crea el registro vacio
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public void ComprobantePre_Delete(int documentID, string actividadFlujoID, DateTime periodo, string comprobanteID, int compNro, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isValid = true;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByComprobante(documentID, periodo, comprobanteID, compNro);
                if (docCtrl != null)
                {
                    docCtrl.DocumentoAnula.Value = documentID;
                    this._moduloGlobal.glDocumentoControl_Update(docCtrl, false, true);
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.Anulado, EstadoDocControl.Anulado.ToString(), true);
                    this.DeshabilitarAlarma(docCtrl.NumeroDoc.Value.Value, actividadFlujoID);

                    //Verifica si el comprobante existe
                    DTO_Comprobante compAux = this.Comprobante_Get(true, true, periodo, comprobanteID, compNro, null, null);
                    if (compAux != null)
                    {
                        compAux = this.CreateEmptyAux(compAux.Header);
                        this.AgregarAuxiliar(compAux);
                        this.BorrarAuxiliar_Pre(periodo, comprobanteID, compNro);
                    }
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ComprobantePre_Delete");
                throw ex;
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
        /// Envia para aprobacion un comprobante
        /// </summary>
        /// <param name="currentMod">Modulo que esta ejecutando la operacion</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <param name="userId">Usuario que ejecuta la transaccion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject ComprobantePre_SendToAprob(int documentID, string actividadFlujoID, ModulesPrefix currentMod, DateTime periodo, string comprobanteID,
            int compNro, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100 / 3;

                DTO_Comprobante comprobante = this.Comprobante_Get(true, true, periodo, comprobanteID, compNro, null, null);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (comprobante != null)
                {
                    DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByComprobante(documentID, periodo, comprobanteID, compNro);
                    if (docCtrl != null)
                    {
                        EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                        if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                            return result;
                        }

                        #region Asigna el nuevo flujo
                        result = this.AsignarFlujo(documentID, docCtrl.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                        if (result.Result == ResultValue.NOK)
                            return result;
                        #endregion
                        #region Revisa si finaliza el proceso

                        bool finaliza = result.ResultMessage == true.ToString() ? true : false;
                        if (finaliza)
                            result = this.Comprobante_Aprobar(documentID, actividadFlujoID, currentMod, docCtrl.NumeroDoc.Value.Value, true, periodo, comprobanteID, compNro,
                                string.Empty, true, true, false, true);
                        else
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

                        if (result.Result == ResultValue.NOK)
                            return result;

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                        #endregion
                        #region Generar el archivo
                        if (createDoc)
                        {
                            try
                            {
                                #region Comprobante Manual
                                //if (documentID == AppDocuments.ComprobanteManual)
                                //{
                                //    object report = this.DtoComprobanteReport(comprobante, docCtrl);
                                //    this.GenerarArchivo(documentID, docCtrl.NumeroDoc.Value.Value, report);
                                //}
                                #endregion
                                #region Cruce de cuentas
                                if (documentID == AppDocuments.CruceCuentas)
                                {
                                    object report = this.DtoDocumentoContableReport(comprobante, docCtrl);
                                    this.GenerarArchivo(documentID, docCtrl.NumeroDoc.Value.Value, report);
                                }
                                #endregion

                                porcTotal += porcParte;
                                batchProgress[tupProgress] = (int)porcTotal;
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
                        #endregion

                        //Asigna las alarmas
                        DTO_Alarma alarma = this.GetFirstMailInfo(docCtrl.NumeroDoc.Value.Value, createDoc);
                        alarma.NumeroDoc = docCtrl.NumeroDoc.Value.ToString();
                        alarma.Finaliza = finaliza;
                        return alarma;
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ComprobantePre_SendToAprob");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae un listado de comprobantes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_ComprobanteAprobacion> ComprobantePre_GetPendientesByModulo(ModulesPrefix mod, string actividadFlujoID)
        {
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
            string usuarioID = seUsuario.ID.Value;
            List<DTO_ComprobanteAprobacion> list = this._dal_Comprobante.DAL_ComprobantePre_GetPendientesByModulo(mod, actividadFlujoID, usuarioID);
            foreach (DTO_ComprobanteAprobacion item in list)
                item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);

            return list;
        }

        #endregion

        #region Auxiliar

        /// <summary>
        /// Obtiene un auxiliar con correspondiente IdentificadorTR y periodo anterior o igual a correspondiente Periodo
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_BitacoraSaldo> Comprobante_GetByIdentificadorTR(DateTime periodo, long identTR)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_BitacoraSaldo> list = this._dal_Comprobante.DAL_Comprobante_GetByIdentificadorTR(periodo, identTR);
            return list;
        }

        /// <summary>
        /// Obtiene el numero de registros de un comprobante
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public int Comprobante_Count(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, DTO_glConsulta consulta = null)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_Count(allData, isPre, periodo, comprobanteID, compNro, consulta);
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves (si tiene el numero de documento busca las CxP)
        /// </summary>
        /// <param name="numDoc">Numero de documento de busqueda</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_GetAll(int numDoc, bool isPre, DateTime periodo, string comprobanteID, int? compNro)
        {
            try
            {
                DTO_Comprobante comp = null;

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);

                if (ctrl != null)
                {
                    isPre = false;
                    if (ctrl.Estado.Value.Value == (byte)EstadoDocControl.SinAprobar || ctrl.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion)
                        isPre = true;

                    periodo = ctrl.PeriodoDoc.Value.Value;
                    comprobanteID = ctrl.ComprobanteID.Value;
                    compNro = ctrl.ComprobanteIDNro.Value.Value;
                }

                if (!compNro.HasValue)
                    return comp;


                return this.Comprobante_Get(true, isPre, periodo, comprobanteID, compNro.Value, null, null);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_AprobarRechazar");
                return null;
            }
        }

        /// <summary>
        /// Obtiene un auxiliar a partir de las llaves
        /// </summary>
        /// <param name="allData">Dice si trae todos los datos incluyendo la contrapartida o solo los creados por el usuario</param>
        /// <param name="isPre">Indica si debe traer los datos de coAuxiliarPre (si es falso los trae de coAuxiliar)</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <returns>Retorna un auxiliar</returns>
        public DTO_Comprobante Comprobante_Get(bool allData, bool isPre, DateTime periodo, string comprobanteID, int compNro, int? pageSize, int? pageNum, DTO_glConsulta consulta = null)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_Get(allData, isPre, periodo, comprobanteID, compNro, pageSize, pageNum, consulta);
        }

        /// <summary>
        /// Recibe una lista de probobantes paar aprobar o rechazar
        /// </summary>
        /// <param name="comps">Comprobantes que se deben aprobar o rechazar</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <param name="updDocCtrl">Indica si se debe actualizar la informacion del documento control</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="progress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Comprobante_AprobarRechazar(int documentID, string actividadFlujoID, ModulesPrefix currentMod, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                int i = 0;
                foreach (DTO_ComprobanteAprobacion comp in comps)
                {
                    #region Variables
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / comps.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    rd.line = i;
                    rd.Message = string.Empty;

                    int numeroDoc = comp.NumeroDoc.Value.Value;
                    int ctrlDocID = comp.DocumentoID.Value.Value;
                    DateTime periodo = comp.PeriodoID.Value.Value;
                    string compID = comp.ComprobanteID.Value;
                    int compNro = comp.ComprobanteNro.Value.Value;
                    string obs = comp.Observacion.Value;
                    #endregion

                    if (comp.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.Comprobante_Aprobar(documentID, actividadFlujoID, currentMod, numeroDoc, true, periodo, compID, compNro, obs,
                                updDocCtrl, createDoc, true, false);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Comprobante_Aprobar");
                            rd.Message = DictionaryMessages.Err_AprobComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (comp.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.Comprobante_Rechazar(documentID, actividadFlujoID, ctrlDocID, numeroDoc, periodo, compID, compNro, obs, false);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Comprobante_Rechazar");
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Genera una lista de comprobantes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la operacion (los que se van a guardar en glDocumentoControl)</param>
        /// <param name="periodo">Periodo de migración</param>
        /// <param name="comps">Lista de comprobantes</param>
        /// <param name="areaFuncionalID">Area funcional del usuario</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="borraInfoPeriodo">Inidca si se debe borrar la información del periodo</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Comprobante_Migracion(int documentID, DateTime periodo, List<DTO_Comprobante> comps, string areaFuncionalID, string prefijoID,
            bool borraInfoPeriodo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            List<DTO_TxResult> results = new List<DTO_TxResult>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                if (borraInfoPeriodo)
                {
                    #region Borra la informacion de los saldos actual
                    this._dal_Contabilidad.DAL_Contabilidad_BorraInfoPeriodo(comps.First().Header.PeriodoID.Value.Value);
                    #endregion
                }
                else
                {
                    #region Valida que no existan los comprobantes
                    Dictionary<string, int> consecutivos = new Dictionary<string, int>();

                    foreach (DTO_Comprobante comp in comps)
                    {
                        DTO_ComprobanteHeader header = comp.Header;
                        if (!consecutivos.ContainsKey(comp.Header.ComprobanteID.Value))
                        {
                            #region nuevo comprobante
                            int ultimoCons = this._dal_Contabilidad.DAL_Contabilidad_UltimoComprobanteNro(periodo, comp.Header.ComprobanteID.Value);
                            consecutivos[comp.Header.ComprobanteID.Value] = ultimoCons;

                            if (ultimoCons > 0)
                            {
                                if (comp.Header.ComprobanteNro.Value.Value <= ultimoCons)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = ResultValue.NOK.ToString();

                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = 0;
                                    rd.Message = DictionaryMessages.Err_Co_CompAgregado + "&&" + comp.Header.ComprobanteID.Value + "&&" +
                                        comp.Header.ComprobanteNro.Value.Value.ToString();

                                    result.Details.Add(rd);
                                }
                                else if (comp.Header.ComprobanteNro.Value.Value != ultimoCons + 1)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = ResultValue.NOK.ToString();

                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.line = 0;
                                    rd.Message = DictionaryMessages.Err_Co_CompInvalidConsecutivo + "&&" + comp.Header.ComprobanteID.Value + "&&" +
                                        comp.Header.ComprobanteNro.Value.Value.ToString() + "&&" + ultimoCons.ToString();

                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                        else if (comp.Header.ComprobanteNro.Value.Value <= consecutivos[comp.Header.ComprobanteID.Value])
                        {
                            #region Comprobante existente
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = ResultValue.NOK.ToString();

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = 0;
                            rd.Message = DictionaryMessages.Err_Co_CompAgregado + "&&" + comp.Header.ComprobanteID.Value + "&&" +
                                comp.Header.ComprobanteNro.Value.Value.ToString();

                            result.Details.Add(rd);
                            #endregion
                        }
                    }
                    #endregion
                }

                if (result.Result == ResultValue.NOK)
                {
                    results.Add(result);
                    return results;
                }

                int i = 0;
                string userName = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).ID.Value;
                foreach (DTO_Comprobante comp in comps)
                {
                    //Manejo de porcentajes para la aprobacion
                    int percent = ((i + 1) * 100) / comps.Count;
                    batchProgress[tupProgress] = percent;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.line = i;
                    rd.Message = string.Empty;

                    #region Agregar registro a glDocumentoControl
                    DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();

                    //Campos Principales
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrl.DocumentoTipo.Value = Convert.ToByte(DocumentoTipo.DocExterno);
                    docCtrl.Fecha.Value = comp.Header.Fecha.Value;
                    docCtrl.PeriodoDoc.Value = comp.Header.PeriodoID.Value;
                    docCtrl.PeriodoUltMov.Value = comp.Header.PeriodoID.Value;
                    docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                    docCtrl.PrefijoID.Value = prefijoID;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.MonedaID.Value = comp.Header.MdaTransacc.Value;
                    docCtrl.TasaCambioDOCU.Value = comp.Header.TasaCambioBase.Value;
                    docCtrl.TasaCambioCONT.Value = comp.Header.TasaCambioBase.Value;
                    docCtrl.ComprobanteID.Value = comp.Header.ComprobanteID.Value;
                    docCtrl.ComprobanteIDNro.Value = comp.Header.ComprobanteNro.Value;
                    docCtrl.Observacion.Value = "Migracion";
                    docCtrl.Estado.Value = comp.Footer.Count == 1 ? Convert.ToByte(EstadoDocControl.Anulado) : Convert.ToByte(EstadoDocControl.Aprobado);
                    docCtrl.PeriodoAnula.Value = comp.Header.PeriodoID.Value;

                    rd = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                    }

                    comp.Header.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                    docCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                    #endregion
                    #region Asigna los registros de Ajuste en Cambio Realizado

                    if (result.Result == ResultValue.OK)
                        result = this.AgregarAjusteEnCambio(comp);

                    #endregion
                    #region Guarda el auxiliar

                    if (result.Result == ResultValue.OK)
                        this.AgregarAuxiliar(comp);

                    #endregion
                    #region Genera los saldos

                    if (result.Result == ResultValue.OK)
                        result = this.GenerarSaldos(documentID, ModulesPrefix.co, comp, 0);

                    #endregion
                    #region Actualiza la tabla de consecutivos

                    if (result.Result == ResultValue.OK)
                    {
                        this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_Comprobante.DAL_Comprobante_AgregarConsecutivoMigracion(comp.Header.ComprobanteID.Value, comp.Header.PeriodoID.Value.Value, comp.Header.ComprobanteNro.Value.Value);
                    }

                    #endregion

                    results.Add(result);
                }

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_Migracion");
                results.Add(result);

                return results;
            }
            finally
            {
                int count = results.Where(x => x.Result == ResultValue.NOK).Count();
                if (count == 0 && !insideAnotherTx)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Realiza la reclasificacion de saldos
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="numeroDoc">Numero de documento (PK)</param>
        /// <param name="proyectoID">Identificador del nuevo proyecto</param>
        /// <param name="ctoCostoID">Identificador del centro de costo</param>
        /// <param name="lgID">Identificador del lugar geografico</param>
        /// <param name="obs">Observacion del documento</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado del proceso</returns>
        public DTO_TxResult Comprobante_ReclasificacionSaldos(int documentID, string actividadFlujoID, int numeroDoc, string proyectoID, string ctoCostoID, string lgID, string obs, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_glDocumentoControl ctrl = null;
            DTO_coComprobante coComp = null;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl ctrlOld = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);

                #region Valida que se pueda hacer reclasificación
                if (ctrlOld == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_NoDocument;
                    return result;
                }
                if (string.IsNullOrWhiteSpace(ctrlOld.ComprobanteID.Value) || ctrlOld.ComprobanteIDNro.Value == null || !ctrlOld.ComprobanteIDNro.Value.HasValue || ctrlOld.ComprobanteIDNro.Value.Value == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_DocNoComp;
                    return result;
                }
                #endregion
                #region Variables

                // Variables por defecto
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                // Carga de datos
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CompReclasifYAjuste);
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);
                DTO_coPlanCuenta ctaCtrl = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctrlOld.CuentaID.Value, true, false);

                // Modulo
                DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaCtrl.ConceptoSaldoID.Value, true, false);
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), cSaldo.ModuloID.Value.ToLower());

                // Fechas
                string periodoStr = this.GetControlValueByCompany(mod, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                DateTime fechaDoc = DateTime.Now;
                if (periodo.Year != fechaDoc.Year || periodo.Month != fechaDoc.Month)
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                #endregion
                #region Nuevo documento
                ctrl = ObjectCopier.Clone(ctrlOld);
                ctrl.PeriodoDoc.Value = periodo;
                ctrl.FechaDoc.Value = fechaDoc;
                ctrl.NumeroDoc.Value = 0;
                ctrl.DocumentoID.Value = documentID;
                ctrl.DocumentoTipo.Value = (int)DocumentoTipo.DocInterno;
                ctrl.ProyectoID.Value = proyectoID;
                ctrl.CentroCostoID.Value = ctoCostoID;
                ctrl.LugarGeograficoID.Value = lgID;
                ctrl.Observacion.Value = obs;
                ctrl.Descripcion.Value = "RECLASIFICACION SALDOS";
                ctrl.ComprobanteID.Value = compID;
                ctrl.ComprobanteIDNro.Value = 0;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    return result;
                }

                ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                #endregion
                #region Trae la info de los saldos

                DTO_coCuentaSaldo saldoDTO = this.Saldo_GetByDocumento(ctaCtrl.ID.Value, ctaCtrl.ConceptoSaldoID.Value, ctrlOld.NumeroDoc.Value.Value, string.Empty);
                decimal saldoML = saldoDTO.DbOrigenLocML.Value.Value + saldoDTO.DbOrigenExtML.Value.Value + saldoDTO.CrOrigenLocML.Value.Value + saldoDTO.CrOrigenExtML.Value.Value
                    + saldoDTO.DbSaldoIniLocML.Value.Value + saldoDTO.DbSaldoIniExtML.Value.Value + saldoDTO.CrSaldoIniLocML.Value.Value + saldoDTO.CrSaldoIniExtML.Value.Value;
                saldoML = Math.Round(saldoML, 2);
                //Saldo ME
                decimal saldoME = saldoDTO.DbOrigenLocME.Value.Value + saldoDTO.DbOrigenExtME.Value.Value + saldoDTO.CrOrigenLocME.Value.Value + saldoDTO.CrOrigenExtME.Value.Value
                     + saldoDTO.DbSaldoIniLocME.Value.Value + saldoDTO.DbSaldoIniExtME.Value.Value + saldoDTO.CrSaldoIniLocME.Value.Value + saldoDTO.CrSaldoIniExtME.Value.Value;
                saldoME = Math.Round(saldoME, 2);

                #endregion
                #region Carga el comprobante y actualiza el documento original
                DTO_Comprobante comp = new DTO_Comprobante();

                //header
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.PeriodoID.Value = periodo;
                header.Fecha.Value = fechaDoc;
                header.ComprobanteID.Value = ctrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = 0;
                header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                header.MdaTransacc.Value = ctrlOld.MonedaID.Value;
                header.MdaOrigen.Value = ctrlOld.MonedaID.Value == mdaLoc ? (byte)TipoMoneda.Local : (byte)TipoMoneda.Foreign;
                header.TasaCambioBase.Value = 0;
                header.TasaCambioOtr.Value = 0;
                comp.Header = header;

                #region Carga la partida
                DTO_ComprobanteFooter origen = this.CrearComprobanteFooter(ctrlOld, header.TasaCambioBase.Value, concCargoDef, lgDef, linPresDef, saldoML * -1, saldoME * -1, false);
                origen.DatoAdd4.Value = AuxiliarDatoAdd4.Reclasificacion.ToString();
                comp.Footer.Add(origen);
                #endregion
                #region Actualiza glDocumentoControl

                //Asigna los nuevos valores
                ctrlOld.ProyectoID.Value = proyectoID;
                ctrlOld.CentroCostoID.Value = ctoCostoID;
                ctrlOld.LugarGeograficoID.Value = lgID;
                ctrlOld.Observacion.Value = obs;

                this._moduloGlobal.glDocumentoControl_Update(ctrlOld, true, true);
                this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, false, string.Empty);

                batchProgress[tupProgress] = 50;
                #endregion
                #region Carga la contrapartida
                DTO_ComprobanteFooter contra = this.CrearComprobanteFooter(ctrlOld, null, concCargoDef, lgDef, linPresDef, saldoML, saldoME, true);
                comp.Footer.Add(contra);
                #endregion
                #endregion
                #region Genera el comprobante
                result = this.ContabilizarComprobante(documentID, comp, ctrlOld.PeriodoDoc.Value.Value, ModulesPrefix.co, 0, false);
                #endregion

                batchProgress[tupProgress] = 100;
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Comprobante_ReclasificacionSaldos");
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

                        ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                        this.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                    }
                    else
                        throw new Exception("Comprobante_ReclasificacionSaldos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae el valor de las cuentas de costo
        /// </summary>
        /// <param name="numeroDoc">numero de documento</param>
        /// <returns></returns>
        public decimal Comprobante_GetValorByCuentaCosto(int numeroDoc)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_GetValorByCuentaCosto(numeroDoc);
        }

        /// <summary>
        /// Indica si se han hecho movimientos sobre un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento consultado</param>
        /// <param name="estados">Estados por los que se buscan</param>
        /// <param name="equal">Indica si los estados deben ser iguales o diferentes</param>
        /// <returns>Retorna true si encuentra movimientos</returns>
        public int Comprobante_GetMovimientosByEstados(int numeroDoc, List<EstadoDocControl> estados, bool equal)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_GetMovimientosByEstados(numeroDoc, estados, equal);
        }

        /// <summary>
        /// Trae las transferencias bancarias por tercero
        /// </summary>
        /// <param name="terceroID">tercero a validar</param>
        /// <param name="docTercero">numero de la factura</param>
        /// <returns>lista de comp</returns>
        public DTO_Comprobante Comprobante_GetTransfBancariaByTercero(string terceroID, string docTercero)
        {
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            
            //Trae la factura
            DTO_glDocumentoControl ctrlCxP = this._moduloGlobal.glDocumentoControl_GetExternalDoc(AppDocuments.CausarFacturas, terceroID, docTercero);
            if (ctrlCxP == null)
                return null;
            else
            {
                var comprobante =  this._dal_Comprobante.DAL_Comprobante_GetTransfBancariaByTercero(terceroID, ctrlCxP.NumeroDoc.Value.Value);
                if(comprobante != null)
                {
                   //Trae la cuenta del banco
                   DTO_glDocumentoControl ctrlTransf = this._moduloGlobal.glDocumentoControl_GetByID(comprobante.Header.NumeroDoc.Value.Value);
                   comprobante.CuentaID = ctrlTransf.CuentaID.Value;
                }

                return comprobante;
            }
        }

        /// <summary>
        /// Actualiza los datos secundarios de un auxiliar 
        /// </summary>       
        /// <param name="consecutivoAux">Consecutivo del comprobante</param>
        /// <param name="isPre">Verifica si la actualizacion se esta haciendo en el auxiliar Preliminar o en el real</param>
        public void Comprobante_Update(int consecutivoAux, DTO_ComprobanteFooter comp, bool isPre)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Comprobante.DAL_Comprobante_Update(consecutivoAux, comp, isPre);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo Inicial</param>
        /// <param name="periodoFinal">periodo final</param>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_QueryMvtoAuxiliar> Comprobante_GetAuxByParameter(DateTime? periodoInicial, DateTime? periodoFinal,DTO_QueryMvtoAuxiliar filter)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_GetAuxByParameter(periodoInicial, periodoFinal, filter);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="periodoInicial">periodo</param>
        /// <param name="lugarxDef">lugarGeo</param>
        /// <returns>Lista de auxiliares</returns>
        public List<DTO_PagoImpuesto> Comprobante_GetAuxForImpuesto(DateTime periodoFilter)
        {
            try
            {
                List<DTO_PagoImpuesto> result = new List<DTO_PagoImpuesto>();
                string lugarGeo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                DTO_glLugarGeografico dto = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, lugarGeo, true, false);
                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                if (dto != null)
                {
                    var auxs = this._dal_Comprobante.DAL_Comprobante_GetAuxForImpuesto(periodoFilter, dto);
                    List<string> distinct = (from c in auxs select c.ImpuestoTipoID.Value).Distinct().ToList();
                    List<string> distinctCuentas = (from c in auxs select c.CuentaID.Value).Distinct().ToList();
                    foreach (string imp in distinct)
                    {
                        DTO_PagoImpuesto dtoImp = new DTO_PagoImpuesto();
                        dtoImp.PeriodoID.Value = periodoFilter;
                        dtoImp.ImpuestoTipoID.Value = imp;
                        dtoImp.ImpuestoTipoDesc.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).ImpuestoTipoDesc.Value;
                        dtoImp.PeriodoPago.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).PeriodoPago.Value;
                        dtoImp.LugarGeoID.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).LugarGeoID.Value;
                        dtoImp.LugarGeoDesc.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).LugarGeoDesc.Value;
                        dtoImp.TerceroID.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).TerceroID.Value;
                        dtoImp.TerceroDesc.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp).TerceroDesc.Value; 
                        dtoImp.ValorTotal.Value = auxs.FindAll(x => x.ImpuestoTipoID.Value == imp).Sum(x => x.ValorLocal.Value);
                        dtoImp.ValorTotalMiles.Value = Math.Round((dtoImp.ValorTotal.Value.Value / 1000), 0) * 1000;
                        dtoImp.ValorTotalDif.Value = Math.Abs(dtoImp.ValorTotal.Value.Value - dtoImp.ValorTotalMiles.Value.Value);
                        dtoImp.Selected.Value = false;
                        foreach (string cta in distinctCuentas)
                        {
                            if(auxs.Exists(x => x.ImpuestoTipoID.Value == imp && x.CuentaID.Value == cta))
                            {
                                var dtoImpCuenta = ObjectCopier.Clone(dtoImp);
                                dtoImpCuenta.CuentaID.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp && x.CuentaID.Value == cta).CuentaID.Value;
                                dtoImpCuenta.CuentaDesc.Value = auxs.Find(x => x.ImpuestoTipoID.Value == imp && x.CuentaID.Value == cta).CuentaDesc.Value;
                                dtoImpCuenta.ValorLocal.Value = auxs.FindAll(x => x.ImpuestoTipoID.Value == imp && x.CuentaID.Value == cta).Sum(x => x.ValorLocal.Value);
                                dtoImp.Detalle.Add(dtoImpCuenta);           
                            }
                        }

                        result.Add(dtoImp);
                    }
                }

                //Filtra segun periodo de pago
                if (periodoFilter.Month == 1)
                    result.FindAll(x => x.PeriodoPago.Value == 1);// Solo mensual
                else if (periodoFilter.Month > 1 && periodoFilter.Month < 6)
                    result.FindAll(x => x.PeriodoPago.Value == 1 || x.PeriodoPago.Value == 2);// Mensual - Bimensual
                else if (periodoFilter.Month >= 6 && periodoFilter.Month < 12)
                    result.FindAll(x => x.PeriodoPago.Value == 1 || x.PeriodoPago.Value == 2 || x.PeriodoPago.Value == 3);// Mensual - Bimensual - Semestral
                
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Cierres

        #region Funciones Privadas

        /// <summary>
        /// Valida si se pueda iniciar el cierre anual
        /// </summary>
        /// <returns></returns>
        private DTO_TxResult ValidacionInicioCierreAnual(int year)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();

            try
            {
                string ComprobanteIDCierre = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteCierreAnual);
                if (!string.IsNullOrWhiteSpace(ComprobanteIDCierre))
                {
                    DateTime periodo = new DateTime(year, 12, 01);

                    #region Valida que el periodo 12 de contabilidad este cerrado
                    bool exists = false;
                    bool periodoCerrado = this.IsPeriodoCerrado(ModulesPrefix.co, periodo, ref exists);
                    if (!periodoCerrado)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_CierreAnualPeriodoOpen;
                        return result;
                    }
                    #endregion
                    #region Valida que no hayan datos en coAuxiliarPre
                    this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    int count = this._dal_Comprobante.DAL_ComprobantePre_HasData(true, periodo);
                    if (count > 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;
                        return result;
                    }
                    #endregion
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ComprobanteCierre;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ValidacionInicioCierreAnual");

                return result;
            }
        }

        /// <summary>
        /// Verifica si un periodo ya esta cerrado
        /// </summary>
        /// <param name="mod">Modulo que se esta cerrando</param>
        /// <param name="periodo">Periodo que se esta cerrando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        internal bool IsPeriodoCerrado(ModulesPrefix mod, DateTime periodo, ref bool exists)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_IsPeriodoCerrado(mod, periodo, ref exists);
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Obtiene ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <returns>Retorna periodo o null si no existe alguno</returns>
        public DateTime? GetUltimoMesCerrado(ModulesPrefix mod)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_Contabilidad.DAL_Contabilidad_GetUltimoMesCerrado(mod);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetUltimoMesCerrado");
                return null;
            }
        }

        /// <summary>
        /// Indica si el periodo enviado es el ultimo mes cerrado
        /// </summary>
        /// <param name="mod">Modulo de consulta</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna True si el aperiodo se puede abrir de lo contrario false</returns>
        public bool UltimoMesCerrado(ModulesPrefix mod, DateTime periodo, ref bool exists)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                if (mod != ModulesPrefix.co)
                    return this._dal_Contabilidad.DAL_Contabilidad_UltimoMesCerrado(mod, periodo, ref exists);

                return true;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "UltimoMesCerrado");
                return false;
            }
        }

        /// <summary>
        /// Hace el cierre mensual
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="modulo">Modulo de cierre</param>
        /// <param name="userId">Usuario que realiza la transaccion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_CierrePeriodo(int documentID, DateTime periodo, ModulesPrefix modulo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_glControl bloqueoControl = null;
            try
            {
                #region Variables
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloNomina = (ModuloNomina)this.GetInstance(typeof(ModuloNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcPrevio = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100 / 6;
                decimal porcTotal = 0;

                bool validateOrder = true;
                string moduloID = modulo.ToString();
                bool p14 = false;
                DateTime periodo13 = new DateTime(periodo.Year, periodo.Month, 2);
                DateTime periodo14 = new DateTime(periodo.Year, periodo.Month, 3);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string excluyeDocs = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocumentosExclCierreMes);

                //variables de ctas
                DTO_coPlanCuenta cta;
                NaturalezaCuenta naturaleza = NaturalezaCuenta.Debito;
                Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, NaturalezaCuenta> cacheNaturaleza = new Dictionary<string, NaturalezaCuenta>();

                //Variables de conceptos de saldo
                bool validateSaldo = false;
                SaldoControl saldoCtrl = SaldoControl.Cuenta;
                DTO_glConceptoSaldo cSaldo;
                Dictionary<string, bool> cacheSaldoControl = new Dictionary<string, bool>();

                #endregion
                #region Revisa si se necesita el mes 14
                string strPeriodo14 = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
                if (strPeriodo14.Equals("1") || strPeriodo14.Equals(true.ToString()))
                    p14 = true;
                #endregion
                #region Actualiza en glControl el indicador que se esta realizando un cierre
                string EmpNro = this.Empresa.NumeroControl.Value;
                string _modId = ((int)modulo).ToString();
                if (_modId.Length == 1)
                    _modId = "0" + _modId;

                string keyControl = EmpNro + _modId + AppControl.co_IndBloqueoModuloTransAuxPrelim;
                bloqueoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                bloqueoControl.Data.Value = "1";
                this._moduloGlobal.glControl_Update(bloqueoControl);
                #endregion
                #region Obtiene el siguiente periodo
                DateTime siguientePeriodo = periodo.AddMonths(1);
                if (modulo == ModulesPrefix.co && periodo.Month == 12)
                {
                    if (periodo.Day == 2 || periodo.Day == 3)
                        validateOrder = false;
                    #region Obtiene el periodo 13 o 14
                    if (periodo.Day == 1)//Periodo 12 a 13
                        siguientePeriodo = periodo.AddDays(1);
                    else
                        siguientePeriodo = new DateTime(periodo.Year + 1, 1, 1);
                    #endregion
                    #region Validaciones para el mes 14
                    if (periodo.Day == 2 && p14)//Periodo 13 a 14
                        siguientePeriodo = periodo.AddDays(1);
                    if (periodo.Day == 2 && !p14)//Perido 13 a nuevo año
                        siguientePeriodo = new DateTime(periodo.Year + 1, 1, 1);
                    if (periodo.Day == 3)//Periodo 14 a nuevo año
                        siguientePeriodo = new DateTime(periodo.Year + 1, 1, 1);
                    #endregion
                }
                #endregion
                #region Validaciones Generales
                #region Valida que no hayan documentos pendientes
                List<EstadoDocControl> estados = new List<EstadoDocControl>();
                estados.Add(EstadoDocControl.SinAprobar);
                estados.Add(EstadoDocControl.ParaAprobacion);
                estados.Add(EstadoDocControl.Revisado);
                estados.Add(EstadoDocControl.Radicado);

                int docPend = this._moduloGlobal.glDocumentoControl_CountDocumentsByEstado(periodo, modulo, estados, excluyeDocs);
                if (docPend > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_PendingDocs;
                    return result;
                }
                #endregion
                #region Valida que no haya informacion en el auxiliar preliminar
                bool hasPre = this._dal_Contabilidad.DAL_Contabilidad_HasPreliminares(periodo, moduloID);
                if (hasPre)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;
                    return result;
                }
                #endregion
                #region Valida que no existan documentos en estado radicado
                int rows = this._moduloGlobal.glDocumentoControl_CountByEstadoPeriodo(EstadoDocControl.Radicado, periodo, modulo);
                if (rows > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Gl_DocRadicados;
                    return result;
                }
                #endregion
                #region Valida que ya se haya realizado el proceso de mayorizacion (verificacion del balance)

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                string empGrupo = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                DAL_glTabla tableDAL = new DAL_glTabla(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glTabla table = tableDAL.DAL_glTabla_GetByTablaNombre("coPlanCuenta", empGrupo);

                int lonNivel = table.lonNivel1.Value.Value;
                bool balance = this._dal_Contabilidad.DAL_Contabilidad_SaldosExistsForBalance(periodo, lonNivel);

                if (!balance)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_NoBalance;
                    return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = Convert.ToInt32(porcTotal);
                #endregion
                #endregion
                #region Validaciones y operaciones x modulo
                int count = 1;
                if (validateOrder)
                {
                    Dictionary<ModulesPrefix, EstadoPeriodo> EstadoModulos = this._moduloAplicacion.aplModulo_EstadoPeriodoModulos(periodo);
                    #region Modulo
                    switch (modulo)
                    {
                        case ModulesPrefix.co:
                            #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo abierto antes de cerrar contabilidad
                                if (kvp.Key != modulo && kvp.Value == EstadoPeriodo.Abierto)
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;                                   
                                    det.Message = "Modulo Abierto:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }
                            }
                            if(result.Result ==  ResultValue.NOK)
                                 return result;

                            //Valida que los periodos del resto de modulos sean mayores al de contabilidad
                            Dictionary<ModulesPrefix, string> periodoModulos = this._moduloAplicacion.aplModulo_GetAllPeriodoActual();
                            bool validPeriodMod = periodoModulos.Any(x => Convert.ToDateTime(x.Value) <= periodo && x.Key != ModulesPrefix.co);
                            if (validPeriodMod)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = "Existen periodos menores o iguales al periodo de Contabilidad";
                                return result;
                            }

                            #endregion
                            #region Valida proceso de ajuste en cambio (Contabilidad - Periodo 12)
                            if (periodo.Day == 1 && this.Multimoneda())
                            {
                                bool res = this._moduloGlobal.glDocumentoControl_ValidaAjusteEnCambio(periodo);
                                if (!res)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoAjusteEnCambio;
                                    return result;
                                }
                            }
                            #endregion
                            #region Valida que se haya corrido el proceso de ProrrateoIVA
                            string validaCierreMes = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProrateaIVAIngresosExentos);
                            if (validaCierreMes == "1")
                            {
                                List<DTO_glDocumentoControl> docs = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(AppProcess.ProrrateoIVA, periodo);
                                if (docs.Count == 0)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoProrateoIVA;
                                    return result;
                                }
                            }
                            #endregion
                            #region Cierre mensual por modulo

                            this._dal_coCierreMes = (DAL_coCierreMes)this.GetInstance(typeof(DAL_coCierreMes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            List<DTO_coCierreMes> cierres = this._dal_coCierreMes.DAL_coCierreMes_GetForCierre(periodo);
                            int mes = periodo.Month;
                            foreach (DTO_coCierreMes c in cierres)
                            {
                                if (c.LocalDB01.Value.Value != 0 || c.LocalCR01.Value.Value != 0 || c.ExtraDB01.Value.Value != 0 || c.ExtraCR01.Value.Value != 0)
                                    this._dal_coCierreMes.DAL_coCierreMes_Add(c, mes);
                            }

                            #endregion
                            break;
                        case ModulesPrefix.oc:
                            #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo señalado aun abierto                                
                                if (kvp.Value == EstadoPeriodo.Abierto && kvp.Key != ModulesPrefix.co)
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;
                                    det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }                              
                            }
                            if (result.Result == ResultValue.NOK)
                                return result;

                            #endregion
                            break;
                        case ModulesPrefix.ac:
                            #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo señalado aun abierto                                
                                if (kvp.Value == EstadoPeriodo.Abierto && (kvp.Key == ModulesPrefix.cp || kvp.Key == ModulesPrefix.fa || kvp.Key == ModulesPrefix.@in))
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;
                                    det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }
                            }
                            if (result.Result == ResultValue.NOK)
                                return result;

                            #endregion
                            break;
                        case ModulesPrefix.pr:                            
                            break;
                        case ModulesPrefix.cp:
                            #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo señalado aun abierto                                
                                if (kvp.Value == EstadoPeriodo.Abierto && (kvp.Key == ModulesPrefix.cc ||  kvp.Key == ModulesPrefix.pr || kvp.Key == ModulesPrefix.@in))
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;
                                    det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }                              
                            }
                            if (result.Result == ResultValue.NOK)
                                return result;

                            #endregion
                            break;
                        case ModulesPrefix.ts:
                            #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo señalado aun abierto                                
                                if (kvp.Value == EstadoPeriodo.Abierto && (kvp.Key == ModulesPrefix.cp || kvp.Key == ModulesPrefix.fa))
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;
                                    det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }                              
                            }
                            if (result.Result == ResultValue.NOK)
                                return result;

                            #endregion
                            break;
                        case ModulesPrefix.fa:
                           #region Validaciones
                            foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            {
                                //Valida si hay algun modulo señalado aun abierto                                
                                if (kvp.Value == EstadoPeriodo.Abierto && kvp.Key == ModulesPrefix.@in)
                                {
                                    DTO_TxResultDetail det = new DTO_TxResultDetail();
                                    det.line = count;
                                    det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                                    result.Details.Add(det);
                                    result.Result = ResultValue.NOK;
                                    count++;
                                }                              
                            }
                            if (result.Result == ResultValue.NOK)
                                return result;

                            #endregion
                            break;
                        case ModulesPrefix.py:
                            #region Validaciones
                            //foreach (KeyValuePair<ModulesPrefix, EstadoPeriodo> kvp in EstadoModulos)
                            //{
                            //    //Valida si hay algun modulo señalado aun abierto                                
                            //    if (kvp.Value == EstadoPeriodo.Abierto && (kvp.Key == ModulesPrefix.@in || kvp.Key == ModulesPrefix.pr ||
                            //                                               kvp.Key == ModulesPrefix.fa || kvp.Key == ModulesPrefix.cp))
                            //    {
                            //        DTO_TxResultDetail det = new DTO_TxResultDetail();
                            //        det.line = count;
                            //        det.Message = "Debe cerrar antes el Módulo:  " + kvp.Key;
                            //        result.Details.Add(det);
                            //        result.Result = ResultValue.NOK;
                            //        count++;
                            //    }
                            //}
                            //if (result.Result == ResultValue.NOK)
                            //    return result;

                            #endregion
                            break;
                        case ModulesPrefix.no:
                            {
                            #region Valida Liquidaciones
                            result = this._moduloNomina.Proceso_CierreNomina();
                            if (result.Result == ResultValue.NOK)
                            {
                                return result;
                            }                   
                            #endregion
                            break;
                            }
                        case ModulesPrefix.@in:
                            #region Traslada saldos
                            this._moduloInventario = (ModuloInventarios)this.GetInstance(typeof(ModuloInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            result = this._moduloInventario.Proceso_TrasladoSaldosInventario(documentID, periodo, batchProgress, true);
                            if (result.Result == ResultValue.NOK)
                                return result;
                            //List<DTO_TxResult> results = this._moduloInventario.PosteoComprobantes(AppProcess.PosteoComprobantesIn, batchProgress, true);
                            //if (results.Any(x => x.Result == ResultValue.NOK))
                            //{
                            //    result.Result = ResultValue.NOK;
                            //    return results.Find(x => x.Result == ResultValue.NOK);
                            //}
                               
                            //List<string> actividades = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.PosteoComprobantesInAprob).ToList();
                            //if (actividades.Count == 0)
                            //{
                            //    DTO_TxResultDetail det = new DTO_TxResultDetail();
                            //    det.line = count;
                            //    det.Message = "NO existe la actividad flujo para Posteo de Comprobantes del documento ID :  " + AppDocuments.PosteoComprobantesInAprob.ToString();
                            //    result.Details.Add(det);
                            //    result.Result = ResultValue.NOK;
                            //    count++;
                            //}
                            //else
                            //{
                            //    this._moduloInventario.AprobarPosteo(documentID, actividades[0], ModulesPrefix.co, null, batchProgress, true);
                            //}
                          
                            #endregion
                            break;  
                        default:
                            break;
                    }
                    #endregion
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = Convert.ToInt32(porcTotal);

                #endregion
                #region Borra los saldos iniciales existentes
                this._dal_Contabilidad.DAL_Contabilidad_BorrarSaldosIniciales(siguientePeriodo, moduloID);
                #endregion
                #region Carga la lista de saldos para el nuevo periodo y valida la informacion segun las cuentas
                List<DTO_coCuentaSaldo> saldosInsert = new List<DTO_coCuentaSaldo>();
                List<DTO_coCuentaSaldo> saldosUpdate = new List<DTO_coCuentaSaldo>();
                decimal sumaML = 0;
                decimal sumaME = 0;
                this._dal_Contabilidad.DAL_Contabilidad_GetSaldosCierre(periodo, moduloID, siguientePeriodo, saldosInsert, saldosUpdate);
                #region Valida la informacion de los nuevos saldos
                int i = 0;
                porcPrevio = porcTotal;
                foreach (DTO_coCuentaSaldo saldoIns in saldosInsert)
                {
                    ++i;
                    porcTemp = (porcParte * i) / saldosInsert.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #region Trae la info de la cuenta

                    if (cacheCtas.ContainsKey(saldoIns.CuentaID.Value))
                    {
                        cta = cacheCtas[saldoIns.CuentaID.Value];
                        naturaleza = cacheNaturaleza[saldoIns.CuentaID.Value];
                    }
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, saldoIns.CuentaID.Value, true, false);
                        if (cta == null)
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail det = new DTO_TxResultDetail();
                            det.Message = "La cuenta " + saldoIns.CuentaID.Value + "no está activa o no existe";
                            result.Details.Add(det);
                            break;
                        }
                        naturaleza = (NaturalezaCuenta)Enum.Parse(typeof(NaturalezaCuenta), cta.Naturaleza.Value.Value.ToString());

                        cacheCtas.Add(saldoIns.CuentaID.Value, cta);
                        cacheNaturaleza.Add(saldoIns.CuentaID.Value, naturaleza);
                    }

                    #endregion
                    #region Trae la info del tipo de control de saldos
                    if (cacheSaldoControl.ContainsKey(saldoIns.ConceptoSaldoID.Value))
                        validateSaldo = cacheSaldoControl[saldoIns.ConceptoSaldoID.Value];
                    else
                    {
                        cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, saldoIns.ConceptoSaldoID.Value, true, false);
                        saldoCtrl = (SaldoControl)Enum.Parse(typeof(SaldoControl), cSaldo.coSaldoControl.Value.Value.ToString());

                        validateSaldo = false;
                        if (saldoIns.BalanceTipoID.Value == libroFunc && !cta.CtaCorrienteInd.Value.Value &&
                            saldoCtrl != SaldoControl.Cuenta && saldoCtrl != SaldoControl.Componente_Tercero)
                            validateSaldo = true;

                        cacheSaldoControl.Add(saldoIns.ConceptoSaldoID.Value, validateSaldo);
                    }
                    #endregion
                    if (validateSaldo)
                    {
                        DTO_TxResultDetail det = new DTO_TxResultDetail();
                        det.line = i;
                        #region Carga la informacion de las sumas
                        sumaML =
                            saldoIns.DbOrigenExtML.Value.Value +
                            saldoIns.DbOrigenLocML.Value.Value +
                            saldoIns.CrOrigenExtML.Value.Value +
                            saldoIns.CrOrigenLocML.Value.Value +
                            saldoIns.DbSaldoIniExtML.Value.Value +
                            saldoIns.DbSaldoIniLocML.Value.Value +
                            saldoIns.CrSaldoIniExtML.Value.Value +
                            saldoIns.CrSaldoIniLocML.Value.Value;

                        sumaME =
                            saldoIns.DbOrigenExtME.Value.Value +
                            saldoIns.DbOrigenLocME.Value.Value +
                            saldoIns.CrOrigenExtME.Value.Value +
                            saldoIns.CrOrigenLocME.Value.Value +
                            saldoIns.DbSaldoIniExtME.Value.Value +
                            saldoIns.DbSaldoIniLocME.Value.Value +
                            saldoIns.CrSaldoIniExtME.Value.Value +
                            saldoIns.CrSaldoIniLocME.Value.Value;
                        #endregion
                        #region Valida la naturaleza de las cuentas con los saldos
                        if (naturaleza == NaturalezaCuenta.Debito && (sumaML < 0 || sumaME < 0))
                        {
                            result.Result = ResultValue.NOK;
                            det.Message = DictionaryMessages.Err_SaldoIniCtaDeb + "&&" + saldoIns.CuentaID.Value;
                            result.Details.Add(det);
                        }
                        else if (naturaleza == NaturalezaCuenta.Credito && (sumaML > 0 || sumaME > 0))
                        {
                            result.Result = ResultValue.NOK;
                            det.Message = DictionaryMessages.Err_SaldoIniCtaCre + "&&" + saldoIns.CuentaID.Value;
                            result.Details.Add(det);
                        }
                        #endregion
                    }
                }
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion
                #region Valida la informacion de los saldos que se van a actualizar
                i = 0;
                porcPrevio = porcTotal;
                foreach (DTO_coCuentaSaldo saldoUpd in saldosUpdate)
                {
                    ++i;
                    porcTemp = (porcParte * i) / saldosUpdate.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #region Trae la info de la cuenta

                    if (cacheCtas.ContainsKey(saldoUpd.CuentaID.Value))
                    {
                        cta = cacheCtas[saldoUpd.CuentaID.Value];
                        naturaleza = cacheNaturaleza[saldoUpd.CuentaID.Value];
                    }
                    else
                    {
                        cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, saldoUpd.CuentaID.Value, true, false);
                        naturaleza = (NaturalezaCuenta)Enum.Parse(typeof(NaturalezaCuenta), cta.Naturaleza.Value.Value.ToString());

                        cacheCtas.Add(saldoUpd.CuentaID.Value, cta);
                        cacheNaturaleza.Add(saldoUpd.CuentaID.Value, naturaleza);
                    }

                    #endregion
                    #region Trae la info del tipo de control de saldos
                    if (cacheSaldoControl.ContainsKey(saldoUpd.ConceptoSaldoID.Value))
                        validateSaldo = cacheSaldoControl[saldoUpd.ConceptoSaldoID.Value];
                    else
                    {
                        cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, saldoUpd.ConceptoSaldoID.Value, true, false);
                        saldoCtrl = (SaldoControl)Enum.Parse(typeof(SaldoControl), cSaldo.coSaldoControl.Value.Value.ToString());

                        validateSaldo = false;
                        if (saldoUpd.BalanceTipoID.Value == libroFunc && !cta.CtaCorrienteInd.Value.Value &&
                            saldoCtrl != SaldoControl.Cuenta && saldoCtrl != SaldoControl.Componente_Tercero)
                            validateSaldo = true;

                        cacheSaldoControl.Add(saldoUpd.ConceptoSaldoID.Value, validateSaldo);
                    }
                    #endregion
                    if (validateSaldo)
                    {
                        DTO_TxResultDetail det = new DTO_TxResultDetail();
                        #region Carga la informacion de las sumas
                        sumaML = saldoUpd.DbOrigenExtML.Value.Value +
                            saldoUpd.DbOrigenLocML.Value.Value +
                            saldoUpd.CrOrigenExtML.Value.Value +
                            saldoUpd.CrOrigenLocML.Value.Value +
                            saldoUpd.DbSaldoIniExtML.Value.Value +
                            saldoUpd.DbSaldoIniLocML.Value.Value +
                            saldoUpd.CrSaldoIniExtML.Value.Value +
                            saldoUpd.CrSaldoIniLocML.Value.Value;

                        sumaME = saldoUpd.DbOrigenExtME.Value.Value +
                            saldoUpd.DbOrigenLocME.Value.Value +
                            saldoUpd.CrOrigenExtME.Value.Value +
                            saldoUpd.CrOrigenLocME.Value.Value +
                            saldoUpd.DbSaldoIniExtME.Value.Value +
                            saldoUpd.DbSaldoIniLocME.Value.Value +
                            saldoUpd.CrSaldoIniExtME.Value.Value +
                            saldoUpd.CrSaldoIniLocME.Value.Value;
                        #endregion
                        #region Valida la naturaleza de las cuentas con los saldos
                        if (!cta.CtaCorrienteInd.Value.Value)
                        {
                            if (naturaleza == NaturalezaCuenta.Debito && (sumaML < 0 || sumaME < 0))
                            {
                                result.Result = ResultValue.NOK;
                                det.Message = DictionaryMessages.Err_SaldoUpdCtaDeb + "&&" + saldoUpd.CuentaID.Value + "&&" + saldoUpd.TerceroID.Value + "&&" + " ";
                                result.Details.Add(det);
                            }
                            else if (naturaleza == NaturalezaCuenta.Credito && (sumaML > 0 || sumaME > 0))
                            {
                                result.Result = ResultValue.NOK;
                                det.Message = DictionaryMessages.Err_SaldoUpdCtaCre + "&&" + saldoUpd.CuentaID.Value + "&&" + saldoUpd.TerceroID.Value + "&&" + " ";
                                result.Details.Add(det);
                            } 
                        }
                        #endregion
                    }
                }
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion
                #endregion
                #region Ingresa los nuevos valores de los saldos y agrega la info en coCierresControl
                //Carga la info del mes 13 (14) para modulos que no sean de contabilidad
                if (modulo != ModulesPrefix.co && periodo.Month == 12)
                {
                    this._dal_Contabilidad.DAL_Contabilidad_HacerCierrePeriodo(periodo13, saldosInsert, saldosUpdate);
                    if (p14)
                        this._dal_Contabilidad.DAL_Contabilidad_HacerCierrePeriodo(periodo14, saldosInsert, saldosUpdate);
                }
                this._dal_Contabilidad.DAL_Contabilidad_HacerCierrePeriodo(siguientePeriodo, saldosInsert, saldosUpdate);
                this._dal_Contabilidad.DAL_Contabilidad_AgregarCierreControl(modulo, periodo);
                porcTotal += porcParte;
                batchProgress[tupProgress] = Convert.ToInt32(porcTotal);
                #endregion
                #region Actualizar periodo en glControl

                string key = EmpNro + _modId + AppControl.co_Periodo;
                DTO_glControl periodoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(key));
                periodoControl.Data.Value = siguientePeriodo.ToString(FormatString.ControlDate);
                this._moduloGlobal.glControl_Update(periodoControl);

                #endregion
                #region Activa el siguiente periodo si existe
                DateTime? ultimoPeriodo = this.GetUltimoMesCerrado(modulo);
                if (siguientePeriodo == ultimoPeriodo)
                    this._dal_Contabilidad.DAL_Contabilidad_AbrirMes(siguientePeriodo,modulo.ToString());
                #endregion
                #region Guarda en bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Add))), System.DateTime.Now,
                    this.UserId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = Convert.ToInt32(porcTotal);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CierrePeriodo");
                return result;
            }
            finally
            {
                bloqueoControl.Data.Value = "0";
                this._moduloGlobal.glControl_Update(bloqueoControl);

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Abre un nuevo mes
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <param name="periodo">Periodo para abrir</param>
        /// <param name="modulo">Modulo que se desea abrir</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_AbrirMes(int documentID, DateTime periodo, ModulesPrefix modulo, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                if (modulo != ModulesPrefix.co)
                {
                    bool exists = false;
                    bool res = this.UltimoMesCerrado(modulo, periodo, ref exists);
                    if (!res)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_OpenLastPeriod;
                        return result;
                    }
                }

                //Abre el mes en coCierreControl
                this._dal_Contabilidad.DAL_Contabilidad_AbrirMes(periodo, modulo.ToString());

                periodo = new DateTime(periodo.Year, periodo.Month, 1);
                #region Actualizar periodo en control
                string EmpNro = this.Empresa.NumeroControl.Value;
                string _modId = ((int)modulo).ToString();
                if (_modId.Length == 1)
                    _modId = "0" + _modId;
                string key = EmpNro + _modId + AppControl.co_Periodo;
                DTO_glControl periodoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(key));
                periodoControl.Data.Value = periodo.ToString(FormatString.ControlDate);
                this._moduloGlobal.glControl_Update(periodoControl);

                #endregion
                #region Guarda en bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Add))), System.DateTime.Now, this.UserId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CierrePeriodo");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Cierre anual
        /// </summary>
        /// <param name="year">Año de cierre</param>
        /// <param name="userId">Usuario que realiza la tranasaccion</param>
        /// <param name="batchProgress">Estado del proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public Tuple<DTO_TxResult, DTO_ComprobanteAprobacion> Proceso_CierreAnual(int documentID, string actividadFlujoID, string areaFuncionalID, int year, string libroID,
            Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            EstadoAjuste estadoCtrl = EstadoAjuste.NoData;
            DTO_glDocumentoControl docCtrl = null;
            DTO_coComprobante comp = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                Dictionary<string, DTO_coPlanCuenta> ctas = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, string> proys = new Dictionary<string, string>();

                #region 1.Definicion de variables

                DateTime periodo = new DateTime();
                string prefix = this.GetPrefijoByDocumento(documentID);
                string comprobanteIDCierre = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteCierreAnual);

                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);

                string defTercero = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string defConceptoSaldo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
                string defConceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string defPrefijo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);

                string cuentaIDContrapartida = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaCierreResultadosEjercicio);

                DTO_Comprobante newComprobante = new DTO_Comprobante();
                decimal porcTotal = 0;
                decimal porcParte = 100 / 6;
                #endregion
                #region 2.Valida que se pueda realizar el cierre

                //Valida el coDocumento
                if (string.IsNullOrWhiteSpace(cuentaIDContrapartida))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_CtaCierreResultadosEjercicio + "&&" + string.Empty;

                    return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, new DTO_ComprobanteAprobacion());
                }

                result = this.ValidacionInicioCierreAnual(year);
                if (result.Result == ResultValue.NOK)
                    return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, null);
                #endregion
                #region 3.Asigna el periodo
                string perido14 = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
                bool p14 = false;
                if (perido14.Equals("1") || perido14.Equals(true.ToString()))
                    p14 = true;

                periodo = new DateTime(year, 12, p14 ? 3 : 2);
                #endregion
                #region 4.Revisa si ya se hizo un ajuste previo y carga la informacion de los comprobantes
                //estadoCtrl = this.HasDocument(documentID, periodo, libroID);
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                ctrl.DocumentoID.Value = documentID;
                ctrl.PeriodoDoc.Value = periodo;
                ctrl.ComprobanteID.Value = comprobanteIDCierre;
                DTO_glDocumentoControl ctrlExist = this._moduloGlobal.glDocumentoControl_GetByParameter(ctrl).FirstOrDefault();
                if (ctrlExist != null)
                {
                    estadoCtrl = EstadoAjuste.Preliminar;
                    //DTO_ComprobanteAprobacion paraAprobacionExist = new DTO_ComprobanteAprobacion();
                    //paraAprobacionExist.ComprobanteID.Value = comprobanteIDCierre;
                    //paraAprobacionExist.ComprobanteNro.Value = ctrlExist.ComprobanteIDNro.Value.Value;
                    //paraAprobacionExist.PeriodoID.Value = periodo;
                    //paraAprobacionExist.NumeroDoc.Value = ctrlExist.NumeroDoc.Value.Value;
                    //paraAprobacionExist.Aprobado.Value = true;
                    //return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, paraAprobacionExist);
                }

                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CierreAnualAprobado + "&&" + year.ToString();
                    return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, new DTO_ComprobanteAprobacion());
                }
                else if (estadoCtrl == EstadoAjuste.NoData)
                    comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comprobanteIDCierre, true, false);
                #endregion
                #region 5.Carga la informacion del comprobante (header)

                #region Info general

                newComprobante.Header.LibroID.Value = tipoBalancePreliminar;
                newComprobante.Header.ComprobanteID.Value = comprobanteIDCierre;
                newComprobante.Header.PeriodoID.Value = periodo;
                newComprobante.Header.EmpresaID.Value = this.Empresa.ID.Value;
                newComprobante.Header.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                newComprobante.Header.MdaTransacc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                newComprobante.Header.TasaCambioBase.Value = 0;
                newComprobante.Header.TasaCambioOtr.Value = 0;

                #endregion
                #region Parametros por estado
                if (estadoCtrl == EstadoAjuste.Preliminar)
                {
                    //Trae los documentos
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByCierreAnual(periodo.Year);

                    //Borrar Auxiliar
                    this.BorrarAuxiliar(periodo, docCtrl.ComprobanteID.Value, docCtrl.ComprobanteIDNro.Value.Value);

                    //Borrar info del balance preliminar
                    this.BorrarSaldosXLibro(false, periodo, tipoBalancePreliminar);

                    //Header
                    newComprobante.Header.ComprobanteNro.Value = docCtrl.ComprobanteIDNro.Value.Value;
                    newComprobante.Header.Fecha.Value = docCtrl.FechaDoc.Value.Value;
                    newComprobante.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value.Value;
                }
                else
                {
                    newComprobante.Header.ComprobanteNro.Value = 0;
                    newComprobante.Header.Fecha.Value = new DateTime(periodo.Year, 12, 31);
                    newComprobante.Header.NumeroDoc.Value = 0;
                }
                #endregion

                newComprobante.Footer = new List<DTO_ComprobanteFooter>();

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region 6.Agregar registro a glDocumentoControl

                if (estadoCtrl == EstadoAjuste.NoData)
                {
                    docCtrl = new DTO_glDocumentoControl();

                    //Campos Principales
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.FechaDoc.Value = new DateTime(periodo.Year, 12, 31);
                    docCtrl.PeriodoDoc.Value = periodo;
                    docCtrl.PeriodoUltMov.Value = periodo;
                    docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                    docCtrl.PrefijoID.Value = prefix;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.MonedaID.Value = newComprobante.Header.MdaTransacc.Value;
                    docCtrl.TasaCambioDOCU.Value = newComprobante.Header.TasaCambioBase.Value;
                    docCtrl.TasaCambioCONT.Value = newComprobante.Header.TasaCambioBase.Value;
                    docCtrl.ComprobanteID.Value = newComprobante.Header.ComprobanteID.Value;
                    docCtrl.ComprobanteIDNro.Value = 0;
                    docCtrl.Descripcion.Value = "CONT. CIERRE ANUAL";
                    docCtrl.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                    DTO_TxResultDetail resultDetML = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (resultDetML.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultDetML);

                        return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, null);
                    }

                    docCtrl.NumeroDoc.Value = Convert.ToInt32(resultDetML.Key);
                    newComprobante.Header.NumeroDoc.Value = Convert.ToInt32(resultDetML.Key);

                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region 7.Carga el detalle de los comprobantes
                DTO_coPlanCuenta cuenta;
                #region Carga la informacion para cuentas de tipo resultado

                string proyectoID = string.Empty;
                List<DTO_ComprobanteFooter> partidasResultados = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> contrapartidasResultados = new List<DTO_ComprobanteFooter>();
                List<DTO_coCuentaSaldo> saldosCtaResultado = this._dal_Contabilidad.DAL_Contabilidad_GetSaldosForCierreAnual(true, periodo, libroID);
                foreach (DTO_coCuentaSaldo saldo in saldosCtaResultado)
                {
                    //Info segun los saldos
                    decimal saldoML = saldo.DbOrigenExtML.Value.Value + saldo.DbOrigenLocML.Value.Value + saldo.CrOrigenExtML.Value.Value + saldo.CrOrigenLocML.Value.Value
                        + saldo.DbSaldoIniExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value + saldo.CrSaldoIniExtML.Value.Value + saldo.CrSaldoIniLocML.Value.Value;
                    decimal saldoME = saldo.DbOrigenExtME.Value.Value + saldo.DbOrigenLocME.Value.Value + saldo.CrOrigenExtME.Value.Value + saldo.CrOrigenLocME.Value.Value
                        + saldo.DbSaldoIniExtME.Value.Value + saldo.DbSaldoIniLocME.Value.Value + saldo.CrSaldoIniExtME.Value.Value + saldo.CrSaldoIniLocME.Value.Value;

                    #region Crea el detalle del comprobante

                    DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                    #region Verifica la cuenta
                    if (ctas.ContainsKey(saldo.CuentaID.Value))
                        cuenta = ctas[saldo.CuentaID.Value];
                    else
                    {
                        cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, saldo.CuentaID.Value, true, false);
                        ctas.Add(saldo.CuentaID.Value, cuenta);
                    }
                    #endregion
                    //#region Verifica el proyecto
                    //if (proys.ContainsKey(saldo.ProyectoID.Value))
                    //    proyectoID = proys[saldo.ProyectoID.Value];
                    //else
                    //{
                    //    if (cuenta.ProyectoInd.Value.Value)
                    //    {
                    //        DTO_coProyecto pry = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, saldo.ProyectoID.Value, true, false);
                    //        if (string.IsNullOrWhiteSpace(pry.PryCapitalTrabajo.Value))
                    //            proyectoID = defProyecto;
                    //        else
                    //            proyectoID = pry.PryCapitalTrabajo.Value;
                    //    }
                    //    else
                    //        proyectoID = defProyecto;

                    //    proys.Add(saldo.ProyectoID.Value, proyectoID);
                    //}
                    //#endregion

                    #region Crea las partidas del comprobante
                    List<DTO_ComprobanteFooter> busquedaPartida = partidasResultados.Where
                    (
                        x =>
                        x.CuentaID.Value == saldo.CuentaID.Value //&&
                        //x.ProyectoID.Value == proyectoID
                    ).ToList();

                    if (busquedaPartida != null && busquedaPartida.Count == 1)
                    {
                        #region Actualiza la partida del footer
                        detalle = busquedaPartida.First();
                        detalle.vlrMdaLoc.Value += saldoML * -1;
                        detalle.vlrMdaExt.Value += saldoME * -1;
                        #endregion
                    }
                    else
                    {
                        #region Asigna la partida del footer
                        detalle.CuentaID.Value = saldo.CuentaID.Value;
                        detalle.ProyectoID.Value = defProyecto;//proyectoID;
                        detalle.CentroCostoID.Value = defCentroCosto;
                        detalle.LineaPresupuestoID.Value = defLineaPresupuesto;
                        detalle.LugarGeograficoID.Value = defLugarGeografico;
                        detalle.ConceptoCargoID.Value = defConceptoCargo;
                        detalle.ConceptoSaldoID.Value = defConceptoSaldo;
                        detalle.PrefijoCOM.Value = defPrefijo;
                        detalle.TerceroID.Value = defTercero;
                        detalle.TasaCambio.Value = 0;
                        detalle.DocumentoCOM.Value = "Cierre";
                        detalle.IdentificadorTR.Value = 0;
                        detalle.vlrMdaLoc.Value = saldoML * -1;
                        detalle.vlrMdaExt.Value = saldoME * -1;
                        detalle.vlrMdaOtr.Value = 0;
                        detalle.vlrBaseML.Value = detalle.vlrMdaLoc.Value.Value >= 0 ? 1 : -1;
                        detalle.vlrBaseME.Value = detalle.vlrMdaExt.Value.Value >= 0 ? 1 : -1;
                        detalle.Descriptivo.Value = "CONT. CIERRE ANUAL";

                        partidasResultados.Add(detalle);
                        #endregion
                    }

                    #endregion
                    #region Crea la contrapartida
                    List<DTO_ComprobanteFooter> busquedaContrapartida = contrapartidasResultados.Where
                    (
                        x =>
                        x.CuentaID.Value == cuentaIDContrapartida //&&
                        //x.ProyectoID.Value == detalle.ProyectoID.Value
                    ).ToList();

                    DTO_ComprobanteFooter footerContrapartida = new DTO_ComprobanteFooter();
                    if (busquedaContrapartida != null && busquedaContrapartida.Count == 1)
                    {
                        #region Actualiza la contra partida del footer
                        footerContrapartida = busquedaContrapartida.First();
                        footerContrapartida.vlrMdaLoc.Value += saldoML;
                        footerContrapartida.vlrMdaExt.Value += saldoME;
                        #endregion
                    }
                    else
                    {
                        #region Asigna la contrapartida del footer
                        PropertyInfo[] properties = detalle.GetType().GetProperties();
                        foreach (PropertyInfo pi in properties)
                        {
                            object o = pi.GetValue(detalle, null);
                            if (o is UDT)
                            {
                                UDT udtOld = (UDT)o;
                                UDT udtNew = (UDT)pi.GetValue(footerContrapartida, null);
                                PropertyInfo piVal = udtOld.GetType().GetProperty("Value");
                                piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                            }
                            else
                            {
                                pi.SetValue(footerContrapartida, o, null);
                            }
                        }
                        FieldInfo[] fields = detalle.GetType().GetFields();
                        foreach (FieldInfo fi in fields)
                        {
                            object o = fi.GetValue(detalle);
                            if (o is UDT)
                            {
                                UDT udtOld = (UDT)o;
                                UDT udtNew = (UDT)fi.GetValue(footerContrapartida);
                                PropertyInfo piVal = typeof(UDT).GetProperty("Value");
                                piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                            }
                            else
                            {
                                fi.SetValue(footerContrapartida, o);
                            }
                        }
                        footerContrapartida.CuentaID.Value = cuentaIDContrapartida;
                        footerContrapartida.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                        footerContrapartida.vlrMdaLoc.Value = saldoML;
                        footerContrapartida.vlrMdaExt.Value = saldoME;

                        contrapartidasResultados.Add(footerContrapartida);
                        #endregion
                    }
                    #endregion
                    #endregion
                }

                newComprobante.Footer.AddRange(partidasResultados);
                newComprobante.Footer.AddRange(contrapartidasResultados);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                decimal sumML = 0;
                decimal sumME = 0;

                foreach (DTO_ComprobanteFooter detail in newComprobante.Footer)
                {
                    sumML += detail.vlrMdaLoc.Value.Value;
                    sumME += detail.vlrMdaExt.Value.Value;
                }

                sumML = Math.Round(sumML, 2);
                sumME = Math.Round(sumME, 2);

                #endregion
                #region Trae los comprobantes con cuentas con nit de cierre
                List<DTO_ComprobanteFooter> contrapartidasNit = new List<DTO_ComprobanteFooter>();
                List<DTO_coCuentaSaldo> saldosCtaNit = this._dal_Contabilidad.DAL_Contabilidad_GetSaldosForCierreAnual(false, periodo, libroID);
                foreach (DTO_coCuentaSaldo saldo in saldosCtaNit)
                {
                    //Info segun los saldos
                    decimal saldoML = saldo.DbOrigenExtML.Value.Value + saldo.DbOrigenLocML.Value.Value + saldo.CrOrigenExtML.Value.Value + saldo.CrOrigenLocML.Value.Value
                        + saldo.DbSaldoIniExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value + saldo.CrSaldoIniExtML.Value.Value + saldo.CrSaldoIniLocML.Value.Value;
                    decimal saldoME = saldo.DbOrigenExtME.Value.Value + saldo.DbOrigenLocME.Value.Value + saldo.CrOrigenExtME.Value.Value + saldo.CrOrigenLocME.Value.Value
                        + saldo.DbSaldoIniExtME.Value.Value + saldo.DbSaldoIniLocME.Value.Value + saldo.CrSaldoIniExtME.Value.Value + saldo.CrSaldoIniLocME.Value.Value;

                    #region Crea el detalle del comprobante

                    DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
                    #region Verifica la cuenta
                    if (ctas.ContainsKey(saldo.CuentaID.Value))
                    {
                        cuenta = ctas[saldo.CuentaID.Value];
                    }
                    else
                    {
                        cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, saldo.CuentaID.Value, true, false);
                        ctas.Add(saldo.CuentaID.Value, cuenta);
                    }
                    #endregion
                    #region Asigna la partida del footer

                    detalle.CuentaID.Value = saldo.CuentaID.Value;
                    detalle.CentroCostoID.Value = saldo.CentroCostoID.Value;
                    detalle.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                    detalle.ConceptoSaldoID.Value = saldo.ConceptoSaldoID.Value;
                    detalle.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                    detalle.LugarGeograficoID.Value = defLugarGeografico;
                    detalle.ProyectoID.Value = saldo.ProyectoID.Value;
                    detalle.TerceroID.Value = saldo.TerceroID.Value;
                    detalle.TasaCambio.Value = 0;
                    detalle.vlrMdaLoc.Value = saldoML * -1;
                    detalle.vlrMdaExt.Value = saldoME * -1;
                    detalle.vlrMdaOtr.Value = 0;
                    detalle.vlrBaseML.Value = detalle.vlrMdaLoc.Value.Value >= 0 ? 1 : -1;
                    detalle.vlrBaseME.Value = detalle.vlrMdaExt.Value.Value >= 0 ? 1 : -1;
                    detalle.Descriptivo.Value = "CONT. CIERRE ANUAL";

                    DTO_glDocumentoControl docCtrl1 = this._moduloGlobal.glDocumentoControl_GetByID(Convert.ToInt32(saldo.IdentificadorTR.Value.Value));
                    if (docCtrl != null)
                    {
                        DocumentoTipo tipo = (DocumentoTipo)docCtrl.DocumentoTipo.Value.Value;
                        detalle.PrefijoCOM.Value = docCtrl.PrefijoID.Value;
                        detalle.DocumentoCOM.Value = tipo == DocumentoTipo.DocInterno ? docCtrl.DocumentoNro.Value.ToString() : docCtrl.DocumentoTercero.Value;
                        detalle.IdentificadorTR.Value = saldo.IdentificadorTR.Value;
                    }
                    else
                    {
                        detalle.PrefijoCOM.Value = defPrefijo;
                        detalle.DocumentoCOM.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocumentoCOMAjusteXCuenta);
                        detalle.IdentificadorTR.Value = 0;
                    }

                    newComprobante.Footer.Add(detalle);
                    #endregion

                    #endregion
                    #region Crea la contrapartida
                    List<DTO_ComprobanteFooter> busquedaContrapartida = contrapartidasNit.Where
                    (
                        x =>
                        x.CentroCostoID.Value == detalle.CentroCostoID.Value &&
                        x.ConceptoSaldoID.Value == detalle.ConceptoSaldoID.Value &&
                        x.CuentaID.Value == detalle.CuentaID.Value &&
                        x.TerceroID.Value == cuenta.NITCierreAnual.Value &&
                        x.IdentificadorTR.Value == detalle.IdentificadorTR.Value &&
                        x.LineaPresupuestoID.Value == detalle.LineaPresupuestoID.Value &&
                        x.LugarGeograficoID.Value == detalle.LugarGeograficoID.Value &&
                        x.ProyectoID.Value == detalle.ProyectoID.Value
                    ).ToList();

                    DTO_ComprobanteFooter footerContrapartida = new DTO_ComprobanteFooter();
                    if (busquedaContrapartida != null && busquedaContrapartida.Count == 1)
                    {
                        #region Actualiza la contra partida del footer
                        footerContrapartida = busquedaContrapartida.First();
                        footerContrapartida.vlrMdaLoc.Value += saldoML;
                        footerContrapartida.vlrMdaExt.Value += saldoME;
                        #endregion
                    }
                    else
                    {
                        #region Asigna la contrapartida del footer
                        PropertyInfo[] properties = detalle.GetType().GetProperties();
                        foreach (PropertyInfo pi in properties)
                        {
                            object o = pi.GetValue(detalle, null);
                            if (o is UDT)
                            {
                                UDT udtOld = (UDT)o;
                                UDT udtNew = (UDT)pi.GetValue(footerContrapartida, null);
                                PropertyInfo piVal = udtOld.GetType().GetProperty("Value");
                                piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                            }
                            else
                            {
                                pi.SetValue(footerContrapartida, o, null);
                            }
                        }
                        FieldInfo[] fields = detalle.GetType().GetFields();
                        foreach (FieldInfo fi in fields)
                        {
                            object o = fi.GetValue(detalle);
                            if (o is UDT)
                            {
                                UDT udtOld = (UDT)o;
                                UDT udtNew = (UDT)fi.GetValue(footerContrapartida);
                                PropertyInfo piVal = typeof(UDT).GetProperty("Value");
                                piVal.SetValue(udtNew, piVal.GetValue(udtOld, null), null);
                            }
                            else
                            {
                                fi.SetValue(footerContrapartida, o);
                            }
                        }
                        footerContrapartida.TerceroID.Value = cuenta.NITCierreAnual.Value;
                        footerContrapartida.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                        footerContrapartida.vlrMdaLoc.Value = saldoML;
                        footerContrapartida.vlrMdaExt.Value = saldoME;

                        contrapartidasNit.Add(footerContrapartida);
                        #endregion
                    }
                    #endregion
                }
                newComprobante.Footer.AddRange(contrapartidasNit);
                foreach (DTO_ComprobanteFooter item in newComprobante.Footer)
                {
                    if(string.IsNullOrEmpty(item.DocumentoCOM.Value))
                        item.DocumentoCOM.Value = "Cierre";
                }
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #endregion
                #region 8.Realiza la contabilizacion
                newComprobante.Footer = newComprobante.Footer.FindAll(x => x.vlrMdaLoc.Value != 0).ToList();
                result = this.ComprobantePre_Add(documentID, ModulesPrefix.co, newComprobante,docCtrl.AreaFuncionalID.Value, prefix,true, docCtrl.NumeroDoc.Value, null,batchProgress,true);
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                if (result.Result == ResultValue.NOK)
                    return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, new DTO_ComprobanteAprobacion());
                #endregion
                #region 9.Carga la info del comprobante de aprobacion
                DTO_ComprobanteAprobacion paraAprobacion = new DTO_ComprobanteAprobacion();
                paraAprobacion.ComprobanteID.Value = comprobanteIDCierre;
                paraAprobacion.ComprobanteNro.Value = newComprobante.Header.ComprobanteNro.Value.Value;
                paraAprobacion.PeriodoID.Value = periodo;
                paraAprobacion.NumeroDoc.Value = newComprobante.Header.NumeroDoc.Value.Value;
                paraAprobacion.Aprobado.Value = true;
                #endregion
                #region 10.Actualiza en glControl el indicador del libro que se esta manejando
                string EmpNro = this.Empresa.NumeroControl.Value;
                string keyControl = EmpNro + "01" + AppControl.co_LibroOpConjuntas;
                DTO_glControl bloqueoControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                bloqueoControl.Data.Value = libroID;
                this._moduloGlobal.glControl_Update(bloqueoControl);
                #endregion

                return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, paraAprobacion);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proceso_CierreAnual");
                return new Tuple<DTO_TxResult, DTO_ComprobanteAprobacion>(result, new DTO_ComprobanteAprobacion());
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        #region Asigna consecutivos
                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (estadoCtrl == EstadoAjuste.NoData && docCtrl != null)
                        {
                            docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                            docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, true);
                            this.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, docCtrl.ComprobanteIDNro.Value.Value, true);
                        }
                        #endregion
                    }
                    else
                        throw new Exception("Proceso_CierreAnual - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        ///  Carga la información para hacer un cierre Mensual
        /// </summary>
        /// <param name="año">Periodo de cierre</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetAll(Int16 año)
        {
            this._dal_coCierreMes = (DAL_coCierreMes)base.GetInstance(typeof(DAL_coCierreMes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_coCierreMes.DAL_coCierreMes_GetAll(año);
        }

        /// <summary>
        ///  Carga la información para hacer un cierre Mensual
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<DTO_coCierreMes> coCierreMes_GetByParameter(DTO_coCierreMes filter, RompimientoSaldos? romp1, RompimientoSaldos? romp2)
        {
            this._dal_coCierreMes = (DAL_coCierreMes)base.GetInstance(typeof(DAL_coCierreMes), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coCierreMes> resultTmp = null;
            List<DTO_coCierreMes> cierresFinal = new List<DTO_coCierreMes>();
            List<string> distinct = new List<string>();

            try
            {
                resultTmp = this._dal_coCierreMes.DAL_coCierreMes_GetByParameter(filter);

                #region Valida los datos hasta el periodo seleccionado
                foreach (var cierre in resultTmp)
                {
                    if (cierre.PeriodoID.Value.Value.Month > filter.PeriodoID.Value.Value.Month)
                    {
                        cierre.LocalINI.Value = 0;
                        cierre.LocalINI01.Value = 0;
                        cierre.LocalINI02.Value = 0;
                        cierre.LocalINI03.Value = 0;
                        cierre.LocalINI04.Value = 0;
                        cierre.LocalINI05.Value = 0;
                        cierre.LocalINI06.Value = 0;
                        cierre.LocalINI07.Value = 0;
                        cierre.LocalINI08.Value = 0;
                        cierre.LocalINI09.Value = 0;
                        cierre.LocalINI10.Value = 0;
                        cierre.LocalINI11.Value = 0;
                        cierre.LocalINI12.Value = 0;
                        cierre.LocalDB01.Value = 0;
                        cierre.LocalDB02.Value = 0;
                        cierre.LocalDB03.Value = 0;
                        cierre.LocalDB04.Value = 0;
                        cierre.LocalDB05.Value = 0;
                        cierre.LocalDB06.Value = 0;
                        cierre.LocalDB07.Value = 0;
                        cierre.LocalDB08.Value = 0;
                        cierre.LocalDB09.Value = 0;
                        cierre.LocalDB10.Value = 0;
                        cierre.LocalDB11.Value = 0;
                        cierre.LocalDB12.Value = 0;
                        cierre.LocalCR01.Value = 0;
                        cierre.LocalCR02.Value = 0;
                        cierre.LocalCR03.Value = 0;
                        cierre.LocalCR04.Value = 0;
                        cierre.LocalCR05.Value = 0;
                        cierre.LocalCR06.Value = 0;
                        cierre.LocalCR07.Value = 0;
                        cierre.LocalCR08.Value = 0;
                        cierre.LocalCR09.Value = 0;
                        cierre.LocalCR10.Value = 0;
                        cierre.LocalCR11.Value = 0;
                        cierre.LocalCR12.Value = 0;
                        cierre.ExtraINI.Value = 0;
                        cierre.ExtraINI01.Value = 0;
                        cierre.ExtraINI02.Value = 0;
                        cierre.ExtraINI03.Value = 0;
                        cierre.ExtraINI04.Value = 0;
                        cierre.ExtraINI05.Value = 0;
                        cierre.ExtraINI06.Value = 0;
                        cierre.ExtraINI07.Value = 0;
                        cierre.ExtraINI08.Value = 0;
                        cierre.ExtraINI09.Value = 0;
                        cierre.ExtraINI10.Value = 0;
                        cierre.ExtraINI11.Value = 0;
                        cierre.ExtraINI12.Value = 0;
                        cierre.ExtraDB01.Value = 0;
                        cierre.ExtraDB02.Value = 0;
                        cierre.ExtraDB03.Value = 0;
                        cierre.ExtraDB04.Value = 0;
                        cierre.ExtraDB05.Value = 0;
                        cierre.ExtraDB06.Value = 0;
                        cierre.ExtraDB07.Value = 0;
                        cierre.ExtraDB08.Value = 0;
                        cierre.ExtraDB09.Value = 0;
                        cierre.ExtraDB10.Value = 0;
                        cierre.ExtraDB11.Value = 0;
                        cierre.ExtraDB12.Value = 0;
                        cierre.ExtraCR01.Value = 0;
                        cierre.ExtraCR02.Value = 0;
                        cierre.ExtraCR03.Value = 0;
                        cierre.ExtraCR04.Value = 0;
                        cierre.ExtraCR05.Value = 0;
                        cierre.ExtraCR06.Value = 0;
                        cierre.ExtraCR07.Value = 0;
                        cierre.ExtraCR08.Value = 0;
                        cierre.ExtraCR09.Value = 0;
                        cierre.ExtraCR10.Value = 0;
                        cierre.ExtraCR11.Value = 0;
                        cierre.ExtraCR12.Value = 0;
                    } 
                #endregion
                }
                #region Obtiene IDs no duplicados segun Rompimiento 1
                if (romp1 == RompimientoSaldos.Cuenta)
                    distinct = (from c in resultTmp select c.CuentaID.Value).Distinct().ToList();
                else if (romp1 == RompimientoSaldos.Proyecto)
                    distinct = (from c in resultTmp select c.ProyectoID.Value).Distinct().ToList();
                else if (romp1 == RompimientoSaldos.CentroCosto)
                    distinct = (from c in resultTmp select c.CentroCostoID.Value).Distinct().ToList();
                else if (romp1 == RompimientoSaldos.LineaPresupuesto)
                    distinct = (from c in resultTmp select c.LineaPresupuestoID.Value).Distinct().ToList();
                else if (romp1 == RompimientoSaldos.ConceptoCargo)
                    distinct = (from c in resultTmp select c.ConceptoCargoID.Value).Distinct().ToList();
                else if (romp1 == RompimientoSaldos.Tercero)
                    distinct = (from c in resultTmp select c.TerceroID.Value).Distinct().ToList();
                #endregion

                foreach (string IDRomp1 in distinct)
                {
                    #region Declara variables
                    List<DTO_coCierreMes> detalle = new List<DTO_coCierreMes>();
                    DTO_coCierreMes cierre = new DTO_coCierreMes(true);                   
                    #endregion
                    #region Asigna ID-Descriptivo y detalle segun Rompimiento 1
                    if (romp1 == RompimientoSaldos.Cuenta)
                    {
                        DTO_coPlanCuenta tabla = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, IDRomp1, true, false);
                        cierre.CuentaID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.CuentaID.Value == IDRomp1));
                    }
                    else if (romp1 == RompimientoSaldos.Proyecto)
                    {
                        DTO_coProyecto tabla = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, IDRomp1, true, false);
                        cierre.ProyectoID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.ProyectoID.Value == IDRomp1));
                    }
                    else if (romp1 == RompimientoSaldos.CentroCosto)
                    {
                        DTO_coCentroCosto tabla = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, IDRomp1, true, false);
                        cierre.CentroCostoID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.CentroCostoID.Value == IDRomp1));
                    }
                    else if (romp1 == RompimientoSaldos.ConceptoCargo)
                    {
                        DTO_coConceptoCargo tabla = (DTO_coConceptoCargo)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, IDRomp1, true, false);
                        cierre.ConceptoCargoID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.ConceptoCargoID.Value == IDRomp1));
                    }
                    else if (romp1 == RompimientoSaldos.LineaPresupuesto)
                    {
                        DTO_plLineaPresupuesto tabla = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, IDRomp1, true, false);
                        cierre.LineaPresupuestoID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.LineaPresupuestoID.Value == IDRomp1));
                    }
                    else if (romp1 == RompimientoSaldos.Tercero)
                    {
                        DTO_coTercero tabla = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, IDRomp1, true, false);
                        cierre.TerceroID.Value = tabla != null ? tabla.ID.Value : string.Empty;
                        cierre.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        detalle.AddRange(resultTmp.Where(x => x.TerceroID.Value == IDRomp1));
                    }

                    #endregion
                    #region Obtiene el  Rompimiento 2 si existe
                    if (romp2 != null)
                    {
                        List<string> distinctRomp2 = new List<string>();
                        List<DTO_coCierreMes> cierresRomp2 = new List<DTO_coCierreMes>();
                        #region Cuenta
                        if (romp2 == RompimientoSaldos.Cuenta)
                        {
                            distinctRomp2 = (from c in detalle select c.CuentaID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.CuentaID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        #region Proyecto
                        else if (romp2 == RompimientoSaldos.Proyecto)
                        {
                            distinctRomp2 = (from c in detalle select c.ProyectoID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);  
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.ProyectoID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        #region Centro Costo
                        else if (romp2 == RompimientoSaldos.CentroCosto)
                        {
                            distinctRomp2 = (from c in detalle select c.CentroCostoID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.CentroCostoID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        #region Linea Presupuesto
                        else if (romp2 == RompimientoSaldos.LineaPresupuesto)
                        {
                            distinctRomp2 = (from c in detalle select c.LineaPresupuestoID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.LineaPresupuestoID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        #region Concepto Cargo
                        else if (romp2 == RompimientoSaldos.ConceptoCargo)
                        {
                            distinctRomp2 = (from c in detalle select c.ConceptoCargoID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.LineaPresupuestoID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        #region Tercero
                        else if (romp2 == RompimientoSaldos.Tercero)
                        {
                            distinctRomp2 = (from c in detalle select c.TerceroID.Value).Distinct().ToList();
                            foreach (string IDRomp2 in distinctRomp2)
                            {
                                DTO_coCierreMes res = new DTO_coCierreMes(true);                             
                                foreach (DTO_coCierreMes detRomp2 in detalle.Where(x => x.TerceroID.Value == IDRomp2))
                                {
                                    #region Acumula valores
                                    res.CuentaID.Value = detRomp2.CuentaID.Value;
                                    res.ProyectoID.Value = detRomp2.ProyectoID.Value;
                                    res.CentroCostoID.Value = detRomp2.CentroCostoID.Value;
                                    res.LineaPresupuestoID.Value = detRomp2.LineaPresupuestoID.Value;
                                    res.ConceptoCargoID.Value = detRomp2.ConceptoCargoID.Value;
                                    res.PeriodoID.Value = detRomp2.PeriodoID.Value;
                                    res.TerceroID.Value = detRomp2.TerceroID.Value;
                                    res.LocalINI.Value += detRomp2.LocalINI.Value;
                                    res.LocalINI01.Value += detRomp2.LocalINI01.Value;
                                    res.LocalINI02.Value += detRomp2.LocalINI02.Value;
                                    res.LocalINI03.Value += detRomp2.LocalINI03.Value;
                                    res.LocalINI04.Value += detRomp2.LocalINI04.Value;
                                    res.LocalINI05.Value += detRomp2.LocalINI05.Value;
                                    res.LocalINI06.Value += detRomp2.LocalINI06.Value;
                                    res.LocalINI07.Value += detRomp2.LocalINI07.Value;
                                    res.LocalINI08.Value += detRomp2.LocalINI08.Value;
                                    res.LocalINI09.Value += detRomp2.LocalINI09.Value;
                                    res.LocalINI10.Value += detRomp2.LocalINI10.Value;
                                    res.LocalINI11.Value += detRomp2.LocalINI11.Value;
                                    res.LocalINI12.Value += detRomp2.LocalINI12.Value;
                                    res.LocalDB01.Value += detRomp2.LocalDB01.Value;
                                    res.LocalDB02.Value += detRomp2.LocalDB02.Value;
                                    res.LocalDB03.Value += detRomp2.LocalDB03.Value;
                                    res.LocalDB04.Value += detRomp2.LocalDB04.Value;
                                    res.LocalDB05.Value += detRomp2.LocalDB05.Value;
                                    res.LocalDB06.Value += detRomp2.LocalDB06.Value;
                                    res.LocalDB07.Value += detRomp2.LocalDB07.Value;
                                    res.LocalDB08.Value += detRomp2.LocalDB08.Value;
                                    res.LocalDB09.Value += detRomp2.LocalDB09.Value;
                                    res.LocalDB10.Value += detRomp2.LocalDB10.Value;
                                    res.LocalDB11.Value += detRomp2.LocalDB11.Value;
                                    res.LocalDB12.Value += detRomp2.LocalDB12.Value;
                                    res.LocalCR01.Value += detRomp2.LocalCR01.Value;
                                    res.LocalCR02.Value += detRomp2.LocalCR02.Value;
                                    res.LocalCR03.Value += detRomp2.LocalCR03.Value;
                                    res.LocalCR04.Value += detRomp2.LocalCR04.Value;
                                    res.LocalCR05.Value += detRomp2.LocalCR05.Value;
                                    res.LocalCR06.Value += detRomp2.LocalCR06.Value;
                                    res.LocalCR07.Value += detRomp2.LocalCR07.Value;
                                    res.LocalCR08.Value += detRomp2.LocalCR08.Value;
                                    res.LocalCR09.Value += detRomp2.LocalCR09.Value;
                                    res.LocalCR10.Value += detRomp2.LocalCR10.Value;
                                    res.LocalCR11.Value += detRomp2.LocalCR11.Value;
                                    res.LocalCR12.Value += detRomp2.LocalCR12.Value;
                                    res.ExtraINI.Value += detRomp2.ExtraINI.Value;
                                    res.ExtraINI01.Value += detRomp2.ExtraINI01.Value;
                                    res.ExtraINI02.Value += detRomp2.ExtraINI02.Value;
                                    res.ExtraINI03.Value += detRomp2.ExtraINI03.Value;
                                    res.ExtraINI04.Value += detRomp2.ExtraINI04.Value;
                                    res.ExtraINI05.Value += detRomp2.ExtraINI05.Value;
                                    res.ExtraINI06.Value += detRomp2.ExtraINI06.Value;
                                    res.ExtraINI07.Value += detRomp2.ExtraINI07.Value;
                                    res.ExtraINI08.Value += detRomp2.ExtraINI08.Value;
                                    res.ExtraINI09.Value += detRomp2.ExtraINI09.Value;
                                    res.ExtraINI10.Value += detRomp2.ExtraINI10.Value;
                                    res.ExtraINI11.Value += detRomp2.ExtraINI11.Value;
                                    res.ExtraINI12.Value += detRomp2.ExtraINI12.Value;
                                    res.ExtraDB01.Value += detRomp2.ExtraDB01.Value;
                                    res.ExtraDB02.Value += detRomp2.ExtraDB02.Value;
                                    res.ExtraDB03.Value += detRomp2.ExtraDB03.Value;
                                    res.ExtraDB04.Value += detRomp2.ExtraDB04.Value;
                                    res.ExtraDB05.Value += detRomp2.ExtraDB05.Value;
                                    res.ExtraDB06.Value += detRomp2.ExtraDB06.Value;
                                    res.ExtraDB07.Value += detRomp2.ExtraDB07.Value;
                                    res.ExtraDB08.Value += detRomp2.ExtraDB08.Value;
                                    res.ExtraDB09.Value += detRomp2.ExtraDB09.Value;
                                    res.ExtraDB10.Value += detRomp2.ExtraDB10.Value;
                                    res.ExtraDB11.Value += detRomp2.ExtraDB11.Value;
                                    res.ExtraDB12.Value += detRomp2.ExtraDB12.Value;
                                    res.ExtraCR01.Value += detRomp2.ExtraCR01.Value;
                                    res.ExtraCR02.Value += detRomp2.ExtraCR02.Value;
                                    res.ExtraCR03.Value += detRomp2.ExtraCR03.Value;
                                    res.ExtraCR04.Value += detRomp2.ExtraCR04.Value;
                                    res.ExtraCR05.Value += detRomp2.ExtraCR05.Value;
                                    res.ExtraCR06.Value += detRomp2.ExtraCR06.Value;
                                    res.ExtraCR07.Value += detRomp2.ExtraCR07.Value;
                                    res.ExtraCR08.Value += detRomp2.ExtraCR08.Value;
                                    res.ExtraCR09.Value += detRomp2.ExtraCR09.Value;
                                    res.ExtraCR10.Value += detRomp2.ExtraCR10.Value;
                                    res.ExtraCR11.Value += detRomp2.ExtraCR11.Value;
                                    res.ExtraCR12.Value += detRomp2.ExtraCR12.Value;
                                    #endregion
                                }
                                cierresRomp2.Add(res);
                            }
                        }
                        #endregion
                        //Reasigna el detalle
                        detalle = cierresRomp2;
                    }
                    #endregion
                    #region Asigna valores de saldos segun detalle
                    foreach (DTO_coCierreMes saldo in detalle)
                    {
                        #region Revisa valores saldos(null a $0)
                        saldo.LocalINI.Value = saldo.LocalINI.Value ?? 0;
                        saldo.LocalINI01.Value = saldo.LocalINI01.Value ?? 0;
                        saldo.LocalINI02.Value = saldo.LocalINI02.Value ?? 0;
                        saldo.LocalINI03.Value = saldo.LocalINI03.Value ?? 0;
                        saldo.LocalINI04.Value = saldo.LocalINI04.Value ?? 0;
                        saldo.LocalINI05.Value = saldo.LocalINI05.Value ?? 0;
                        saldo.LocalINI06.Value = saldo.LocalINI06.Value ?? 0;
                        saldo.LocalINI07.Value = saldo.LocalINI07.Value ?? 0;
                        saldo.LocalINI08.Value = saldo.LocalINI08.Value ?? 0;
                        saldo.LocalINI09.Value = saldo.LocalINI09.Value ?? 0;
                        saldo.LocalINI10.Value = saldo.LocalINI10.Value ?? 0;
                        saldo.LocalINI11.Value = saldo.LocalINI11.Value ?? 0;
                        saldo.LocalINI12.Value = saldo.LocalINI12.Value ?? 0;
                        saldo.LocalDB01.Value = saldo.LocalDB01.Value ?? 0;
                        saldo.LocalDB02.Value = saldo.LocalDB02.Value ?? 0;
                        saldo.LocalDB03.Value = saldo.LocalDB03.Value ?? 0;
                        saldo.LocalDB04.Value = saldo.LocalDB04.Value ?? 0;
                        saldo.LocalDB05.Value = saldo.LocalDB05.Value ?? 0;
                        saldo.LocalDB06.Value = saldo.LocalDB06.Value ?? 0;
                        saldo.LocalDB07.Value = saldo.LocalDB07.Value ?? 0;
                        saldo.LocalDB08.Value = saldo.LocalDB08.Value ?? 0;
                        saldo.LocalDB09.Value = saldo.LocalDB09.Value ?? 0;
                        saldo.LocalDB10.Value = saldo.LocalDB10.Value ?? 0;
                        saldo.LocalDB11.Value = saldo.LocalDB11.Value ?? 0;
                        saldo.LocalDB12.Value = saldo.LocalDB12.Value ?? 0;
                        saldo.LocalCR01.Value = saldo.LocalCR01.Value ?? 0;
                        saldo.LocalCR02.Value = saldo.LocalCR02.Value ?? 0;
                        saldo.LocalCR03.Value = saldo.LocalCR03.Value ?? 0;
                        saldo.LocalCR04.Value = saldo.LocalCR04.Value ?? 0;
                        saldo.LocalCR05.Value = saldo.LocalCR05.Value ?? 0;
                        saldo.LocalCR06.Value = saldo.LocalCR06.Value ?? 0;
                        saldo.LocalCR07.Value = saldo.LocalCR07.Value ?? 0;
                        saldo.LocalCR08.Value = saldo.LocalCR08.Value ?? 0;
                        saldo.LocalCR09.Value = saldo.LocalCR09.Value ?? 0;
                        saldo.LocalCR10.Value = saldo.LocalCR10.Value ?? 0;
                        saldo.LocalCR11.Value = saldo.LocalCR11.Value ?? 0;
                        saldo.LocalCR12.Value = saldo.LocalCR12.Value ?? 0;
                        saldo.ExtraINI.Value = saldo.ExtraINI.Value ?? 0;
                        saldo.ExtraINI01.Value = saldo.ExtraINI01.Value ?? 0;
                        saldo.ExtraINI02.Value = saldo.ExtraINI02.Value ?? 0;
                        saldo.ExtraINI03.Value = saldo.ExtraINI03.Value ?? 0;
                        saldo.ExtraINI04.Value = saldo.ExtraINI04.Value ?? 0;
                        saldo.ExtraINI05.Value = saldo.ExtraINI05.Value ?? 0;
                        saldo.ExtraINI06.Value = saldo.ExtraINI06.Value ?? 0;
                        saldo.ExtraINI07.Value = saldo.ExtraINI07.Value ?? 0;
                        saldo.ExtraINI08.Value = saldo.ExtraINI08.Value ?? 0;
                        saldo.ExtraINI09.Value = saldo.ExtraINI09.Value ?? 0;
                        saldo.ExtraINI10.Value = saldo.ExtraINI10.Value ?? 0;
                        saldo.ExtraINI11.Value = saldo.ExtraINI11.Value ?? 0;
                        saldo.ExtraINI12.Value = saldo.ExtraINI12.Value ?? 0;
                        saldo.ExtraDB01.Value = saldo.ExtraDB01.Value ?? 0;
                        saldo.ExtraDB02.Value = saldo.ExtraDB02.Value ?? 0;
                        saldo.ExtraDB03.Value = saldo.ExtraDB03.Value ?? 0;
                        saldo.ExtraDB04.Value = saldo.ExtraDB04.Value ?? 0;
                        saldo.ExtraDB05.Value = saldo.ExtraDB05.Value ?? 0;
                        saldo.ExtraDB06.Value = saldo.ExtraDB06.Value ?? 0;
                        saldo.ExtraDB07.Value = saldo.ExtraDB07.Value ?? 0;
                        saldo.ExtraDB08.Value = saldo.ExtraDB08.Value ?? 0;
                        saldo.ExtraDB09.Value = saldo.ExtraDB09.Value ?? 0;
                        saldo.ExtraDB10.Value = saldo.ExtraDB10.Value ?? 0;
                        saldo.ExtraDB11.Value = saldo.ExtraDB11.Value ?? 0;
                        saldo.ExtraDB12.Value = saldo.ExtraDB12.Value ?? 0;
                        saldo.ExtraCR01.Value = saldo.ExtraCR01.Value ?? 0;
                        saldo.ExtraCR02.Value = saldo.ExtraCR02.Value ?? 0;
                        saldo.ExtraCR03.Value = saldo.ExtraCR03.Value ?? 0;
                        saldo.ExtraCR04.Value = saldo.ExtraCR04.Value ?? 0;
                        saldo.ExtraCR05.Value = saldo.ExtraCR05.Value ?? 0;
                        saldo.ExtraCR06.Value = saldo.ExtraCR06.Value ?? 0;
                        saldo.ExtraCR07.Value = saldo.ExtraCR07.Value ?? 0;
                        saldo.ExtraCR08.Value = saldo.ExtraCR08.Value ?? 0;
                        saldo.ExtraCR09.Value = saldo.ExtraCR09.Value ?? 0;
                        saldo.ExtraCR10.Value = saldo.ExtraCR10.Value ?? 0;
                        saldo.ExtraCR11.Value = saldo.ExtraCR11.Value ?? 0;
                        saldo.ExtraCR12.Value = saldo.ExtraCR12.Value ?? 0;
                        #endregion
                        #region Acumula Saldos
                        //Saldos Local
                        cierre.LocalINI.Value += saldo.LocalINI.Value;
                        cierre.LocalINI01.Value += saldo.LocalINI01.Value;
                        cierre.LocalINI02.Value += saldo.LocalINI02.Value;
                        cierre.LocalINI03.Value += saldo.LocalINI03.Value;
                        cierre.LocalINI04.Value += saldo.LocalINI04.Value;
                        cierre.LocalINI05.Value += saldo.LocalINI05.Value;
                        cierre.LocalINI06.Value += saldo.LocalINI06.Value;
                        cierre.LocalINI07.Value += saldo.LocalINI07.Value;
                        cierre.LocalINI08.Value += saldo.LocalINI08.Value;
                        cierre.LocalINI09.Value += saldo.LocalINI09.Value;
                        cierre.LocalINI10.Value += saldo.LocalINI10.Value;
                        cierre.LocalINI11.Value += saldo.LocalINI11.Value;
                        cierre.LocalINI12.Value += saldo.LocalINI12.Value;
                        cierre.LocalDB01.Value += saldo.LocalDB01.Value;
                        cierre.LocalDB02.Value += saldo.LocalDB02.Value;
                        cierre.LocalDB03.Value += saldo.LocalDB03.Value;
                        cierre.LocalDB04.Value += saldo.LocalDB04.Value;
                        cierre.LocalDB05.Value += saldo.LocalDB05.Value;
                        cierre.LocalDB06.Value += saldo.LocalDB06.Value;
                        cierre.LocalDB07.Value += saldo.LocalDB07.Value;
                        cierre.LocalDB08.Value += saldo.LocalDB08.Value;
                        cierre.LocalDB09.Value += saldo.LocalDB09.Value;
                        cierre.LocalDB10.Value += saldo.LocalDB10.Value;
                        cierre.LocalDB11.Value += saldo.LocalDB11.Value;
                        cierre.LocalDB12.Value += saldo.LocalDB12.Value;
                        cierre.LocalCR01.Value += saldo.LocalCR01.Value;
                        cierre.LocalCR02.Value += saldo.LocalCR02.Value;
                        cierre.LocalCR03.Value += saldo.LocalCR03.Value;
                        cierre.LocalCR04.Value += saldo.LocalCR04.Value;
                        cierre.LocalCR05.Value += saldo.LocalCR05.Value;
                        cierre.LocalCR06.Value += saldo.LocalCR06.Value;
                        cierre.LocalCR07.Value += saldo.LocalCR07.Value;
                        cierre.LocalCR08.Value += saldo.LocalCR08.Value;
                        cierre.LocalCR09.Value += saldo.LocalCR09.Value;
                        cierre.LocalCR10.Value += saldo.LocalCR10.Value;
                        cierre.LocalCR11.Value += saldo.LocalCR11.Value;
                        cierre.LocalCR12.Value += saldo.LocalCR12.Value;
                        //Saldos Extranjera
                        cierre.ExtraINI.Value += saldo.ExtraINI.Value;
                        cierre.ExtraINI01.Value += saldo.ExtraINI01.Value;
                        cierre.ExtraINI02.Value += saldo.ExtraINI02.Value;
                        cierre.ExtraINI03.Value += saldo.ExtraINI03.Value;
                        cierre.ExtraINI04.Value += saldo.ExtraINI04.Value;
                        cierre.ExtraINI05.Value += saldo.ExtraINI05.Value;
                        cierre.ExtraINI06.Value += saldo.ExtraINI06.Value;
                        cierre.ExtraINI07.Value += saldo.ExtraINI07.Value;
                        cierre.ExtraINI08.Value += saldo.ExtraINI08.Value;
                        cierre.ExtraINI09.Value += saldo.ExtraINI09.Value;
                        cierre.ExtraINI10.Value += saldo.ExtraINI10.Value;
                        cierre.ExtraINI11.Value += saldo.ExtraINI11.Value;
                        cierre.ExtraINI12.Value += saldo.ExtraINI12.Value;
                        cierre.ExtraDB01.Value += saldo.ExtraDB01.Value;
                        cierre.ExtraDB02.Value += saldo.ExtraDB02.Value;
                        cierre.ExtraDB03.Value += saldo.ExtraDB03.Value;
                        cierre.ExtraDB04.Value += saldo.ExtraDB04.Value;
                        cierre.ExtraDB05.Value += saldo.ExtraDB05.Value;
                        cierre.ExtraDB06.Value += saldo.ExtraDB06.Value;
                        cierre.ExtraDB07.Value += saldo.ExtraDB07.Value;
                        cierre.ExtraDB08.Value += saldo.ExtraDB08.Value;
                        cierre.ExtraDB09.Value += saldo.ExtraDB09.Value;
                        cierre.ExtraDB10.Value += saldo.ExtraDB10.Value;
                        cierre.ExtraDB11.Value += saldo.ExtraDB11.Value;
                        cierre.ExtraDB12.Value += saldo.ExtraDB12.Value;
                        cierre.ExtraCR01.Value += saldo.ExtraCR01.Value;
                        cierre.ExtraCR02.Value += saldo.ExtraCR02.Value;
                        cierre.ExtraCR03.Value += saldo.ExtraCR03.Value;
                        cierre.ExtraCR04.Value += saldo.ExtraCR04.Value;
                        cierre.ExtraCR05.Value += saldo.ExtraCR05.Value;
                        cierre.ExtraCR06.Value += saldo.ExtraCR06.Value;
                        cierre.ExtraCR07.Value += saldo.ExtraCR07.Value;
                        cierre.ExtraCR08.Value += saldo.ExtraCR08.Value;
                        cierre.ExtraCR09.Value += saldo.ExtraCR09.Value;
                        cierre.ExtraCR10.Value += saldo.ExtraCR10.Value;
                        cierre.ExtraCR11.Value += saldo.ExtraCR11.Value;
                        cierre.ExtraCR12.Value += saldo.ExtraCR12.Value;
                        #endregion
                        #region Asigna el saldo Total Segun Periodo Seleccionado
                        saldo.SaldoLocal.Value =
                            filter.PeriodoID.Value.Value.Month == 1 ? saldo.LocalINI01.Value + saldo.LocalDB01.Value + saldo.LocalCR01.Value :
                             filter.PeriodoID.Value.Value.Month == 2 ? saldo.LocalINI02.Value + saldo.LocalDB02.Value + saldo.LocalCR02.Value :
                              filter.PeriodoID.Value.Value.Month == 3 ? saldo.LocalINI03.Value + saldo.LocalDB03.Value + saldo.LocalCR03.Value :
                               filter.PeriodoID.Value.Value.Month == 4 ? saldo.LocalINI04.Value + saldo.LocalDB04.Value + saldo.LocalCR04.Value :
                                filter.PeriodoID.Value.Value.Month == 5 ? saldo.LocalINI05.Value + saldo.LocalDB05.Value + saldo.LocalCR05.Value :
                                 filter.PeriodoID.Value.Value.Month == 6 ? saldo.LocalINI06.Value + saldo.LocalDB06.Value + saldo.LocalCR06.Value :
                                  filter.PeriodoID.Value.Value.Month == 7 ? saldo.LocalINI07.Value + saldo.LocalDB07.Value + saldo.LocalCR07.Value :
                                   filter.PeriodoID.Value.Value.Month == 8 ? saldo.LocalINI08.Value + saldo.LocalDB08.Value + saldo.LocalCR08.Value :
                                    filter.PeriodoID.Value.Value.Month == 9 ? saldo.LocalINI09.Value + saldo.LocalDB09.Value + saldo.LocalCR09.Value :
                                     filter.PeriodoID.Value.Value.Month == 10 ? saldo.LocalINI10.Value + saldo.LocalDB10.Value + saldo.LocalCR10.Value :
                                      filter.PeriodoID.Value.Value.Month == 11 ? saldo.LocalINI11.Value + saldo.LocalDB11.Value + saldo.LocalCR11.Value :
                                       saldo.LocalINI12.Value + saldo.LocalDB12.Value + saldo.LocalCR12.Value;

                        //Total Extranjero
                        saldo.SaldoExtra.Value =
                            filter.PeriodoID.Value.Value.Month == 1 ? saldo.ExtraINI01.Value + saldo.ExtraDB01.Value + saldo.ExtraCR01.Value :
                             filter.PeriodoID.Value.Value.Month == 2 ? saldo.ExtraINI02.Value + saldo.ExtraDB02.Value + saldo.ExtraCR02.Value :
                              filter.PeriodoID.Value.Value.Month == 3 ? saldo.ExtraINI03.Value + saldo.ExtraDB03.Value + saldo.ExtraCR03.Value :
                               filter.PeriodoID.Value.Value.Month == 4 ? saldo.ExtraINI04.Value + saldo.ExtraDB04.Value + saldo.ExtraCR04.Value :
                                filter.PeriodoID.Value.Value.Month == 5 ? saldo.ExtraINI05.Value + saldo.ExtraDB05.Value + saldo.ExtraCR05.Value :
                                 filter.PeriodoID.Value.Value.Month == 6 ? saldo.ExtraINI06.Value + saldo.ExtraDB06.Value + saldo.ExtraCR06.Value :
                                  filter.PeriodoID.Value.Value.Month == 7 ? saldo.ExtraINI07.Value + saldo.ExtraDB07.Value + saldo.ExtraCR07.Value :
                                   filter.PeriodoID.Value.Value.Month == 8 ? saldo.ExtraINI08.Value + saldo.ExtraDB08.Value + saldo.ExtraCR08.Value :
                                    filter.PeriodoID.Value.Value.Month == 9 ? saldo.ExtraINI09.Value + saldo.ExtraDB09.Value + saldo.ExtraCR09.Value :
                                     filter.PeriodoID.Value.Value.Month == 10 ? saldo.ExtraINI10.Value + saldo.ExtraDB10.Value + saldo.ExtraCR10.Value :
                                      filter.PeriodoID.Value.Value.Month == 11 ? saldo.ExtraINI11.Value + saldo.ExtraDB11.Value + saldo.ExtraCR11.Value :
                                       saldo.ExtraINI12.Value + saldo.ExtraDB12.Value + saldo.ExtraCR12.Value;
                        #endregion
                        #region Asigna Descripcion segun el Rompimiento 2
                        if (romp2 == RompimientoSaldos.Cuenta)
                        {
                            DTO_coPlanCuenta tabla = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, saldo.CuentaID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        else if (romp2 == RompimientoSaldos.Proyecto)
                        {
                            DTO_coProyecto tabla = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, saldo.ProyectoID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        else if (romp2 == RompimientoSaldos.CentroCosto)
                        {
                            DTO_coCentroCosto tabla = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, saldo.CentroCostoID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        else if (romp2 == RompimientoSaldos.ConceptoCargo)
                        {
                            DTO_coConceptoCargo tabla = (DTO_coConceptoCargo)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, saldo.ConceptoCargoID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        else if (romp2 == RompimientoSaldos.LineaPresupuesto)
                        {
                            DTO_plLineaPresupuesto tabla = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, saldo.LineaPresupuestoID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        else if (romp2 == RompimientoSaldos.Tercero)
                        {
                            DTO_coTercero tabla = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, saldo.TerceroID.Value, true, false);
                            saldo.Descriptivo.Value = tabla != null ? tabla.Descriptivo.Value : string.Empty;
                        }
                        #endregion
                    }
                    #endregion
                    #region Asigna Detalle y saldo final
                    cierre.Detalle = detalle;
                    cierre.Detalle = cierre.Detalle.FindAll(x => x.SaldoLocal.Value != 0);
                    cierre.SaldoLocal.Value = cierre.Detalle.Sum(x => x.SaldoLocal.Value);
                    cierre.SaldoExtra.Value = cierre.Detalle.Sum(x => x.SaldoExtra.Value);
                    if (romp2 == null)
                        cierre.Detalle = new List<DTO_coCierreMes>();
                    cierresFinal.Add(cierre);
                    #endregion
                }
                return cierresFinal;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coCierreMes_GetByParameter");
                return null;
            }
        }

        #endregion

        #endregion

        #region Cruce Cuentas (Ajuste Saldos)

        #region Funciones Privadas

        /// <summary>
        /// Consulta una tabla coDocumentoAjuste segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        private DTO_coDocumentoAjuste coDocumentoAjuste_Get(int NumeroDoc)
        {
            this._dal_coDocumentoAjuste = (DAL_coDocumentoAjuste)base.GetInstance(typeof(DAL_coDocumentoAjuste), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_coDocumentoAjuste.DAL_coDocumentoAjuste_Get(NumeroDoc);
        }

        /// <summary>
        /// adiciona en tabla coDocumentoAjuste 
        /// </summary>
        /// <param name="ajuste">DocumentoAjuste</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        private DTO_TxResult coDocumentoAjuste_Add(DTO_coDocumentoAjuste ajuste)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_coDocumentoAjuste = (DAL_coDocumentoAjuste)base.GetInstance(typeof(DAL_coDocumentoAjuste), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coDocumentoAjuste.DAL_coDocumentoAjuste_Add(ajuste);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, AppDocuments.CruceCuentas, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, ajuste.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "coDocumentoAjuste_Add");
                return result;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Ajustar el documento
        /// </summary>
        /// <param name="ajuste">documento ajuste</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public DTO_TxResult CruceCuentas_Ajustar(int documentID, string actividadFlujoID, DTO_glDocumentoControl ctrl, DTO_coDocumentoAjuste ajuste, DTO_Comprobante comp,
            Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_coComprobante coCompr = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 3;
                int numeroDoc = 0;

                #region Guardar en glDocumentoControl
                ctrl.CuentaID.Value = string.Empty;
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    return result;
                }

                numeroDoc = Convert.ToInt32(resultGLDC.Key);
                ctrl.NumeroDoc.Value = numeroDoc;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Guardar en coDocumentoAjuste
                ajuste.NumeroDoc.Value = numeroDoc;
                result = this.coDocumentoAjuste_Add(ajuste);
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion;
                #region Contabiliza el comprobante
                coCompr = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);

                comp.Header.NumeroDoc.Value = numeroDoc;
                comp.Header.LibroID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                //comp.Footer.Last().IdentificadorTR.Value = numeroDoc;
                result = this.ContabilizarComprobante(documentID, comp, comp.Header.PeriodoID.Value.Value, ModulesPrefix.co, 0, false);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion

                //Asigna el numero Doc si el resultado es OK
                if (result.Result == ResultValue.OK)
                    result.ExtraField = numeroDoc.ToString();

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AjusteSaldos_Ajustar");
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

                        ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                        ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompr, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, true, false);
                        this.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);

                        result.ResultMessage = DictionaryMessages.Co_NumberComp + "&&" + ctrl.ComprobanteID.Value + "&&" + ctrl.ComprobanteIDNro.Value.Value.ToString();
                    }
                    else
                        throw new Exception("CruceCuentas_Ajustar - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #endregion

        #region Distribucion Comprobante

        #region Funciones Privadas

        /// <summary>
        /// genera un comprobante a partir de la distribucion (El comprobante Nro queda en 0)
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="origen">Origen del cual se va a obtener la informacion del auxiliar</param>
        /// <returns>Retorna la lista de comprobantes</returns>
        private DTO_Comprobante DistribuirComprobante(DateTime periodo, List<DTO_coCompDistribuyeTabla> origenes)
        {
            try
            {
                #region Variables
                DTO_Comprobante comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coCompDistribuyeExcluye = (DAL_coCompDistribuyeExcluye)this.GetInstance(typeof(DAL_coCompDistribuyeExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coCompDistribuyeTabla = (DAL_coCompDistribuyeTabla)this.GetInstance(typeof(DAL_coCompDistribuyeTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                Dictionary<string, string> cacheCtas = new Dictionary<string, string>();
                DTO_coPlanCuenta cta;
                #endregion

                foreach (DTO_coCompDistribuyeTabla origen in origenes)
                {
                    #region Carga el footer
                    List<DTO_coCompDistribuyeTabla> destinos = this._dal_coCompDistribuyeTabla.DAL_coCompDistribuyeTabla_GetByID(origen.Consecutivo.Value.Value);
                    List<DTO_coCompDistribuyeExcluye> excls = this._dal_coCompDistribuyeExcluye.DAL_coCompDistribuyeExcluye_GetByConsecutivo(origen.Consecutivo.Value.Value);

                    DTO_Comprobante auxiliares = this._dal_Comprobante.DAL_Comprobante_GetForDistribucion(periodo, origen, excls);

                    foreach (DTO_ComprobanteFooter detalleAux in auxiliares.Footer)
                    {
                        decimal baseML = 0;
                        decimal baseME = 0;
                        decimal vlrML = 0;
                        decimal vlrME = 0;
                        foreach (DTO_coCompDistribuyeTabla destino in destinos)
                        {
                            #region Carga el detalle del comprobante
                            DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();

                            #region Cuenta y concepto de saldo
                            if (string.IsNullOrWhiteSpace(destino.CuentaDEST.Value))
                            {
                                detalle.CuentaID.Value = detalleAux.CuentaID.Value;
                                detalle.IdentificadorTR.Value = detalleAux.IdentificadorTR.Value;
                                if (!cacheCtas.ContainsKey(detalle.CuentaID.Value))
                                    cacheCtas.Add(detalleAux.CuentaID.Value, detalleAux.ConceptoSaldoID.Value);
                            }
                            else
                            {
                                detalle.CuentaID.Value = destino.CuentaDEST.Value;
                                detalle.IdentificadorTR.Value = 0;
                                if (!cacheCtas.ContainsKey(detalle.CuentaID.Value))
                                {
                                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, detalle.CuentaID.Value, true, false);
                                    cacheCtas.Add(detalle.CuentaID.Value, cta.ConceptoSaldoID.Value);
                                }
                            }

                            detalle.ConceptoSaldoID.Value = cacheCtas[detalle.CuentaID.Value];
                            #endregion
                            #region Centro de costo y proyecto

                            //Cto costo
                            if (string.IsNullOrWhiteSpace(destino.CtoCostoDEST.Value))
                                detalle.CentroCostoID.Value = detalleAux.CentroCostoID.Value;
                            else
                                detalle.CentroCostoID.Value = destino.CtoCostoDEST.Value;

                            //Proyecto
                            if (string.IsNullOrWhiteSpace(destino.ProyectoDEST.Value))
                                detalle.ProyectoID.Value = detalleAux.ProyectoID.Value;
                            else
                                detalle.ProyectoID.Value = destino.ProyectoDEST.Value;

                            #endregion
                            #region Otros
                            detalle.LineaPresupuestoID.Value = detalleAux.LineaPresupuestoID.Value;
                            detalle.LugarGeograficoID.Value = detalleAux.LugarGeograficoID.Value;
                            detalle.PrefijoCOM.Value = detalleAux.PrefijoCOM.Value;
                            detalle.TerceroID.Value = detalleAux.TerceroID.Value;
                            detalle.ConceptoCargoID.Value = detalleAux.ConceptoCargoID.Value;

                            detalle.DocumentoCOM.Value = detalleAux.DocumentoCOM.Value;
                            detalle.TasaCambio.Value = detalleAux.TasaCambio.Value;
                            #endregion
                            #region Valores

                            detalle.vlrBaseML.Value = detalleAux.vlrBaseML.Value * destino.PorcentajeID.Value.Value / 100;
                            detalle.vlrBaseME.Value = detalleAux.vlrBaseME.Value * destino.PorcentajeID.Value.Value / 100;
                            detalle.vlrMdaLoc.Value = detalleAux.vlrMdaLoc.Value * destino.PorcentajeID.Value.Value / 100;
                            detalle.vlrMdaExt.Value = detalleAux.vlrMdaExt.Value * destino.PorcentajeID.Value.Value / 100;

                            baseML += detalle.vlrBaseML.Value.Value;
                            baseME += detalle.vlrBaseME.Value.Value;
                            vlrML += detalle.vlrMdaLoc.Value.Value;
                            vlrME += detalle.vlrMdaExt.Value.Value;

                            #endregion

                            detalle.DatoAdd3.Value = destino.CuentaORIG.Value + ", " + destino.ProyectoORIG.Value + ", " + destino.CtoCostoORIG.Value;
                            detalle.DatoAdd4.Value = AuxiliarDatoAdd4.Distribucion.ToString();

                            footer.Add(detalle);
                            #endregion
                        }

                        #region Carga la contrapartida
                        DTO_ComprobanteFooter contra = new DTO_ComprobanteFooter();

                        #region Cuenta y concepto de saldo
                        if (string.IsNullOrWhiteSpace(origen.CuentaCONT.Value))
                        {
                            contra.CuentaID.Value = detalleAux.CuentaID.Value;
                            contra.IdentificadorTR.Value = detalleAux.IdentificadorTR.Value;

                            if (!cacheCtas.ContainsKey(contra.CuentaID.Value))
                                cacheCtas.Add(detalleAux.CuentaID.Value, detalleAux.ConceptoSaldoID.Value);
                        }
                        else
                        {
                            contra.CuentaID.Value = origen.CuentaCONT.Value;
                            contra.IdentificadorTR.Value = 0;

                            if (!cacheCtas.ContainsKey(contra.CuentaID.Value))
                            {
                                cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, contra.CuentaID.Value, true, false);
                                cacheCtas.Add(contra.CuentaID.Value, cta.ConceptoSaldoID.Value);
                            }
                        }

                        contra.ConceptoSaldoID.Value = cacheCtas[contra.CuentaID.Value];
                        #endregion
                        #region Centro de costo y proyecto

                        //Cto costo
                        if (string.IsNullOrWhiteSpace(origen.CtoCostoCONT.Value))
                            contra.CentroCostoID.Value = detalleAux.CentroCostoID.Value;
                        else
                            contra.CentroCostoID.Value = origen.CtoCostoCONT.Value;

                        //Proyecto
                        if (string.IsNullOrWhiteSpace(origen.ProyectoCONT.Value))
                            contra.ProyectoID.Value = detalleAux.ProyectoID.Value;
                        else
                            contra.ProyectoID.Value = origen.ProyectoCONT.Value;

                        #endregion
                        #region Otros
                        contra.LineaPresupuestoID.Value = detalleAux.LineaPresupuestoID.Value;
                        contra.LugarGeograficoID.Value = detalleAux.LugarGeograficoID.Value;
                        contra.PrefijoCOM.Value = detalleAux.PrefijoCOM.Value;
                        contra.TerceroID.Value = detalleAux.TerceroID.Value;
                        contra.ConceptoCargoID.Value = detalleAux.ConceptoCargoID.Value;

                        contra.DocumentoCOM.Value = detalleAux.DocumentoCOM.Value;
                        contra.TasaCambio.Value = detalleAux.TasaCambio.Value;
                        #endregion
                        #region Valores
                        contra.vlrBaseML.Value = Math.Round(baseML * -1, 2);
                        contra.vlrBaseME.Value = Math.Round(baseME * -1, 2);
                        contra.vlrMdaLoc.Value = Math.Round(vlrML * -1, 2);
                        contra.vlrMdaExt.Value = Math.Round(vlrME * -1, 2);
                        #endregion

                        contra.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                        footer.Add(contra);
                        #endregion
                    }
                    #endregion
                }

                #region Carga el cabezote
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.PeriodoID.Value = periodo;
                header.ComprobanteID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteDistribucion);
                header.ComprobanteNro.Value = 0;
                header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                header.NumeroDoc.Value = 0;
                header.MdaTransacc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                header.MdaOrigen.Value = (Byte)TipoMoneda_LocExt.Local;
                header.TasaCambioBase.Value = 0;
                header.TasaCambioOtr.Value = 0;
                #endregion

                if (footer.Count == 0)
                    comp = this.CreateEmptyAux(header);
                else
                {
                    comp.Header = header;
                    comp.Footer = footer;
                }

                return comp;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DistribuirComprobante");
                throw ex;
            }
        }

        /// <summary>
        /// Genera un comprobante para revertir periodos anteriores
        /// </summary>
        /// <param name="periodoIni">Periodo Inicial</param>
        /// <param name="periodoFin">Periodo Final</param>
        /// <returns>Retorna un comprobante de reversion</returns>
        private DTO_Comprobante RevertirDistribucion(int documentID, DateTime periodo)
        {
            try
            {
                DTO_Comprobante comp = new DTO_Comprobante();
                List<DTO_glDocumentoControl> ctrls = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(documentID, periodo);
                if (ctrls.Count == 1)
                {
                    int numeroDoc = ctrls.First().NumeroDoc.Value.Value;

                    this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    List<DTO_ComprobanteFooter> footer = this._dal_Comprobante.DAL_Comprobante_GetByNumeroDoc(numeroDoc);

                    if (footer.Count > 0)
                    {
                        comp = new DTO_Comprobante();

                        #region Header
                        DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();

                        header.EmpresaID.Value = this.Empresa.ID.Value;
                        header.PeriodoID.Value = periodo;
                        header.ComprobanteID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteDistribucion);
                        header.ComprobanteNro.Value = 0;
                        header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                        header.NumeroDoc.Value = 0;
                        header.MdaTransacc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                        header.MdaOrigen.Value = (Byte)TipoMoneda_LocExt.Local;
                        header.TasaCambioBase.Value = 0;
                        header.TasaCambioOtr.Value = 0;
                        #endregion
                        #region Footer
                        List<DTO_ComprobanteFooter> newFooter = new List<DTO_ComprobanteFooter>();
                        foreach (DTO_ComprobanteFooter det in footer)
                        {
                            DTO_ComprobanteFooter newDet = det;

                            newDet.vlrBaseML.Value *= -1;
                            newDet.vlrBaseME.Value *= -1;
                            newDet.vlrMdaLoc.Value *= -1;
                            newDet.vlrMdaExt.Value *= -1;
                            newDet.vlrMdaOtr.Value *= -1;

                            newFooter.Add(newDet);
                        }
                        #endregion

                        comp.Header = header;
                        comp.Footer = newFooter;
                    }
                }

                return comp;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Distribuir_GetComprobanteReversion");
                throw ex;
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetDistribucion()
        {
            this._dal_coCompDistribuyeTabla = (DAL_coCompDistribuyeTabla)base.GetInstance(typeof(DAL_coCompDistribuyeTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coCompDistribuyeTabla> results = this._dal_coCompDistribuyeTabla.DAL_coCompDistribuyeTabla_GetAll();

            return results;
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeExcluye> ComprobanteDistribucion_GetExclusiones()
        {
            this._dal_coCompDistribuyeExcluye = (DAL_coCompDistribuyeExcluye)base.GetInstance(typeof(DAL_coCompDistribuyeExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coCompDistribuyeExcluye> results = this._dal_coCompDistribuyeExcluye.DAL_coCompDistribuyeExcluye_GetAll();

            return results;
        }

        /// <summary>
        /// Actualiza la informacion para la distribucion de comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="tablas">Registros de distribucion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ComprobanteDistribucion_Update(int documentID, List<DTO_coCompDistribuyeTabla> tablas, List<DTO_coCompDistribuyeExcluye> excluyen, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coCompDistribuyeTabla = (DAL_coCompDistribuyeTabla)this.GetInstance(typeof(DAL_coCompDistribuyeTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coCompDistribuyeExcluye = (DAL_coCompDistribuyeExcluye)this.GetInstance(typeof(DAL_coCompDistribuyeExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Elimina la info existente
                this._dal_coCompDistribuyeTabla.DAL_coCompDistribuyeTabla_Delete();
                this._dal_coCompDistribuyeExcluye.DAL_coCompDistribuyeExcluye_Delete();

                //Guarda la info de coCompDistribuyeTabla
                foreach (DTO_coCompDistribuyeTabla tabla in tablas)
                    this._dal_coCompDistribuyeTabla.DAL_coCompDistribuyeTabla_Add(tabla);

                //Guarda la info de coCompDistribuyeTabla
                foreach (DTO_coCompDistribuyeExcluye exc in excluyen)
                    this._dal_coCompDistribuyeExcluye.DAL_coCompDistribuyeExcluye_Add(exc);

                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), DateTime.Now,
                    this.UserId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ComprobanteDistribucion_Update");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la distribucion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coCompDistribuyeTabla> ComprobanteDistribucion_GetForProcess()
        {
            this._dal_coCompDistribuyeTabla = (DAL_coCompDistribuyeTabla)base.GetInstance(typeof(DAL_coCompDistribuyeTabla), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coCompDistribuyeTabla> results = this._dal_coCompDistribuyeTabla.DAL_coCompDistribuyeTabla_GetForProcess();

            return results;
        }

        /// <summary>
        /// Genera los preliminares y revierto los comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="origenes">Lista de comprobantes que se deben distribuir</param>
        /// <param name="periodoIni">Periodo Inicial</param>
        /// <param name="periodoFin">Periodo Final</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> ComprobanteDistribucion_GenerarPreliminar(int documentID, string actividadFlujoID,
            List<DTO_coCompDistribuyeTabla> origenes, DateTime periodoIni, DateTime periodoFin, string libroID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            bool commit = true;
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = null;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            EstadoAjuste estadoCtrl = EstadoAjuste.NoData;
            DTO_glDocumentoControl docCtrl = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Definicion de variables

                List<DTO_ComprobanteAprobacion> comprobantesAprob = new List<DTO_ComprobanteAprobacion>();

                string prefix = this.GetPrefijoByDocumento(documentID);
                string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string tipoBalancePreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);

                DTO_Comprobante comp = null;
                DTO_Comprobante compReversion = null;
                DTO_Comprobante compReProceso = new DTO_Comprobante();

                DTO_ComprobanteHeader headerRev = null;
                DTO_ComprobanteHeader headerReProc = null;

                List<DTO_ComprobanteFooter> footerRev = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> footerReProc = new List<DTO_ComprobanteFooter>();

                string areaFuncionalID = this._moduloGlobal.GetAreaFuncionalByUser();
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string comprobanteID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteDistribucion);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 4;

                #endregion
                #region Valida que no hayan datos en coAuxiliarPre
                int count = this._dal_Comprobante.DAL_ComprobantePre_HasData(true, periodoFin);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;

                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                }
                #endregion

                //Carga el estado del documento
                estadoCtrl = this.HasDocument(documentID, periodoFin, libroID);
                #region Carga la info de glDocumentoControl segun el estado
                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    #region Documento aprobado
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DistribucionAprobado;

                    return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    #endregion
                }


                if (estadoCtrl == EstadoAjuste.NoData)
                {
                    docCtrl = new DTO_glDocumentoControl();
                    #region Agregar registro a glDocumentoControl

                    //Campos Principales
                    docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrl.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrl.Fecha.Value = periodoFin;
                    docCtrl.PeriodoDoc.Value = periodoFin;
                    docCtrl.PeriodoUltMov.Value = periodoFin;
                    docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                    docCtrl.PrefijoID.Value = prefix;
                    docCtrl.DocumentoNro.Value = 0;
                    docCtrl.MonedaID.Value = monedaLocal;
                    docCtrl.TasaCambioDOCU.Value = 0;
                    docCtrl.TasaCambioCONT.Value = 0;
                    docCtrl.ComprobanteID.Value = comprobanteID;
                    docCtrl.ComprobanteIDNro.Value = 1;
                    docCtrl.Observacion.Value = string.Empty;
                    docCtrl.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                    rd = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rd);

                        return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    }

                    docCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                    #endregion
                }
                else
                {
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(documentID, periodoFin).First();

                    //Limpia la informacion existente
                    this.BorrarSaldosXLibro(true, periodoFin, tipoBalancePreliminar);
                    this.BorrarAuxiliar(periodoFin, comprobanteID, 1);
                    this.BorrarAuxiliar(periodoFin, comprobanteID, 2);
                    this.BorrarAuxiliar(periodoFin, comprobanteID, 3);
                }
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #region Crea la nueva distribucion
                if (periodoFin.Date == periodoIni.Date)
                {
                    comp = this.DistribuirComprobante(periodoFin, origenes);
                    if (comp == null)
                        commit = false;
                    else
                    {
                        #region Crea el comprobante de distribucion para el periodo
                        comp.Header.LibroID.Value = tipoBalancePreliminar;
                        comp.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                        comp.Header.ComprobanteNro.Value = 1;

                        result = this.ContabilizarComprobante(documentID, comp, periodoFin, ModulesPrefix.co, 0, true);
                        if (result.Result == ResultValue.NOK)
                            return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());

                        porcTotal = 80;
                        batchProgress[tupProgress] = (int)porcTotal;
                        #endregion
                        #region Actualiza la tabla de consecutivos de comprobantes
                        this._dal_Comprobante.DAL_Comprobante_AgregarConsecutivoMigracion(comprobanteID, periodoFin, 1);
                        #endregion
                        #region Genera los comprobantes para aprobacion
                        DTO_ComprobanteAprobacion paraAprobacion1 = new DTO_ComprobanteAprobacion();
                        paraAprobacion1.ComprobanteID.Value = comprobanteID;
                        paraAprobacion1.ComprobanteNro.Value = 1;
                        paraAprobacion1.PeriodoID.Value = periodoFin;
                        paraAprobacion1.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                        paraAprobacion1.Aprobado.Value = true;

                        comprobantesAprob.Add(paraAprobacion1);
                        #endregion
                    }
                }
                else
                {
                    #region Carga la información de reversion
                    int compNro = 0;

                    compReversion = new DTO_Comprobante();
                    compReProceso = new DTO_Comprobante();
                    #region Carga la info de los periodos anteriores

                    for (int i = periodoIni.Month; i < periodoFin.Month; ++i)
                    {
                        DateTime p = new DateTime(periodoIni.Year, i, 1);
                        DTO_Comprobante compRTemp = this.RevertirDistribucion(documentID, p);
                        DTO_Comprobante compReProcTemp = this.DistribuirComprobante(p, origenes);

                        //Carga la informacion del comprobante de reversion
                        if (compRTemp.Footer.Count > 0)
                        {
                            if (headerRev == null)
                                headerRev = compRTemp.Header;

                            footerRev.AddRange(compRTemp.Footer);
                        }

                        //Carga la informacion del comprobante de reproceso
                        if (compReProcTemp.Footer.Count > 0)
                        {
                            if (headerReProc == null)
                                headerReProc = compRTemp.Header;

                            footerReProc.AddRange(compReProcTemp.Footer);
                        }
                    }

                    porcTotal = 50;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Contabiliza los comprobantes de los periodos anteriores
                    //Reversion
                    if (footerRev.Count > 0)
                    {
                        compNro++;

                        compReversion.Header.LibroID.Value = tipoBalancePreliminar;
                        compReversion.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                        compReversion.Header.ComprobanteNro.Value = compNro;
                        compReversion.Header.PeriodoID.Value = periodoFin;

                        result = this.ContabilizarComprobante(documentID, compReversion, periodoFin, ModulesPrefix.co, 0, true);
                        if (result.Result == ResultValue.NOK)
                            return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    }

                    porcTotal = 60;
                    batchProgress[tupProgress] = (int)porcTotal;

                    //Reproceso
                    if (footerReProc.Count > 0)
                    {
                        compNro++;

                        compReProceso.Header.LibroID.Value = tipoBalancePreliminar;
                        compReProceso.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                        compReProceso.Header.ComprobanteNro.Value = compNro;
                        compReversion.Header.PeriodoID.Value = periodoFin;

                        result = this.ContabilizarComprobante(documentID, compReProceso, periodoFin, ModulesPrefix.co, 0, true);
                        if (result.Result == ResultValue.NOK)
                            return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
                    }

                    porcTotal = 70;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #endregion
                    #region Crea el comprobante de distribucion para el periodo
                    compNro++;

                    comp = this.DistribuirComprobante(periodoFin, origenes);
                    comp.Header.LibroID.Value = tipoBalancePreliminar;
                    comp.Header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    comp.Header.ComprobanteNro.Value = compNro;

                    result = this.ContabilizarComprobante(documentID, comp, periodoFin, ModulesPrefix.co, 0, true);
                    if (result.Result == ResultValue.NOK)
                        return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());

                    porcTotal = 90;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Actualiza la tabla de consecutivos de comprobantes
                    this._dal_Comprobante.DAL_Comprobante_AgregarConsecutivoMigracion(comprobanteID, periodoFin, 3);
                    #endregion
                    #region Genera los comprobantes para aprobacion
                    DTO_ComprobanteAprobacion paraAprobacion1 = new DTO_ComprobanteAprobacion();
                    paraAprobacion1.ComprobanteID.Value = comprobanteID;
                    paraAprobacion1.ComprobanteNro.Value = 1;
                    paraAprobacion1.PeriodoID.Value = periodoFin;
                    paraAprobacion1.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    paraAprobacion1.Aprobado.Value = true;

                    DTO_ComprobanteAprobacion paraAprobacion2 = new DTO_ComprobanteAprobacion();
                    paraAprobacion2.ComprobanteID.Value = comprobanteID;
                    paraAprobacion2.ComprobanteNro.Value = 2;
                    paraAprobacion2.PeriodoID.Value = periodoFin;
                    paraAprobacion2.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    paraAprobacion2.Aprobado.Value = true;

                    DTO_ComprobanteAprobacion paraAprobacion3 = new DTO_ComprobanteAprobacion();
                    paraAprobacion3.ComprobanteID.Value = comprobanteID;
                    paraAprobacion3.ComprobanteNro.Value = 3;
                    paraAprobacion3.PeriodoID.Value = periodoFin;
                    paraAprobacion3.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                    paraAprobacion3.Aprobado.Value = true;

                    comprobantesAprob.Add(paraAprobacion1);
                    comprobantesAprob.Add(paraAprobacion2);
                    comprobantesAprob.Add(paraAprobacion3);
                    #endregion

                    porcTotal = 100;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                #endregion

                return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, comprobantesAprob);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ComprobanteDistribucion_GenerarPreliminar");

                return new Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>>(result, new List<DTO_ComprobanteAprobacion>());
            }
            finally
            {
                if (result.Result == ResultValue.OK && commit)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (estadoCtrl == EstadoAjuste.NoData)
                        {
                            docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, false, false);
                        }
                    }
                    else
                        throw new Exception("ComprobanteDistribucion_GenerarPreliminar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #endregion

        #region Impuestos

        #region Funciones privadas

        /// <summary>
        /// Carga la informacion resumida de los renglones de los impuestos
        /// </summary>
        /// <param name="impuestoID">Identificador del impuesto</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año d declaracion</param>
        /// <param name="detaCuentas">Lista del detalle de las cuentas</param>
        /// <param name="detaRenglones">Lista del detalle de los renglones</param>
        /// <param name="vlrTotalLoc">Valor total de todos los renglones</param>
        /// <param name="totalAjuste">Valor total del ajuste</param>
        /// <param name="vlrTotalExtra">Valor total en la moneda extranjera</param>
        /// <param name="isMultimoneda">Identifica si es multimoneda</param>
        /// <param name="tc">Valor de la tasa de cambio</param>
        /// <returns>Retorna una lista de detalles</returns>
        private List<DTO_ComprobanteFooter> GetImpuestoDetalle(string impuestoID, short mesDeclaracion, short añoDeclaracion, List<DTO_coImpDeclaracionDetaRenglon> detaRenglones,
            List<DTO_coImpDeclaracionDetaCuenta> detaCuentas, out decimal vlrTotalLoc, out decimal vlrTotalExtra, out decimal totalAjuste, bool isMultimoneda, decimal tc)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DAL_MasterComplex _dal_MasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_coImpuestoDeclaracion imp = (DTO_coImpuestoDeclaracion)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, impuestoID, true, false);
                List<DTO_ComprobanteFooter> compFooter = new List<DTO_ComprobanteFooter>();

                totalAjuste = 0;
                vlrTotalLoc = 0;
                vlrTotalExtra = 0;

                #region Carga la info del periodo de consulta
                string queryPeriodo = string.Empty;
                DateTime fechaIni = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                PeriodoFiscal pFiscal = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), imp.PeriodoDeclaracion.Value.Value.ToString());
                switch (pFiscal)
                {
                    case PeriodoFiscal.Anual:
                        #region Periodo Anual
                        fechaIni = new DateTime(añoDeclaracion, 1, 1);
                        fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        #endregion
                        break;
                    case PeriodoFiscal.Semestral:
                        #region Periodo Semestral
                        if (mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Trimestral:
                        #region Periodo Trimestral
                        if (mesDeclaracion <= 3)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 3, 31);
                        }
                        else if (mesDeclaracion > 3 && mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 4, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else if (mesDeclaracion > 3 && mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 9, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 10, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Bimestral:
                        #region Periodo Bimestral
                        if (mesDeclaracion == 1 || mesDeclaracion == 2)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 2, DateTime.DaysInMonth(añoDeclaracion, 2));
                        }
                        else if (mesDeclaracion == 3 || mesDeclaracion == 4)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 3, 1);
                            fechaFin = new DateTime(añoDeclaracion, 4, 30);
                        }
                        else if (mesDeclaracion == 5 || mesDeclaracion == 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 5, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else if (mesDeclaracion == 7 || mesDeclaracion == 8)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 8, 30);
                        }
                        else if (mesDeclaracion == 9 || mesDeclaracion == 10)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 9, 1);
                            fechaFin = new DateTime(añoDeclaracion, 10, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 11, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Mensual:
                        #region Periodo Mensual
                        fechaIni = new DateTime(añoDeclaracion, mesDeclaracion, 1);
                        fechaFin = new DateTime(añoDeclaracion, mesDeclaracion, DateTime.DaysInMonth(añoDeclaracion, mesDeclaracion));
                        #endregion
                        break;
                }

                #endregion
                #region Trae la lista de los renglones

                DTO_glConsulta filtro = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                //Filtro de impuesto
                DTO_glConsultaFiltro campo = new DTO_glConsultaFiltro();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ImpuestoDeclID",
                    ValorFiltro = impuestoID,
                    OperadorFiltro = OperadorFiltro.Igual
                });
                filtro.Filtros = filtros;

                //Carga la informacion de los renglones
                _dal_MasterComplex.DocumentID = AppMasters.coImpDeclaracionRenglon;
                long count = _dal_MasterComplex.DAL_MasterComplex_Count(filtro, true);

                List<DTO_MasterComplex> complexRenglon = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, filtro, true).ToList();
                List<DTO_coImpDeclaracionRenglon> masterRenglones = complexRenglon.Cast<DTO_coImpDeclaracionRenglon>().ToList();
                #endregion

                foreach (DTO_coImpDeclaracionRenglon r in masterRenglones)
                {
                    #region Carga los valores de coImpDeclaracionDetaRenglon
                    DTO_coImpDeclaracionDetaRenglon rDeta = new DTO_coImpDeclaracionDetaRenglon();
                    rDeta.EmpresaID.Value = this.Empresa.ID.Value;
                    rDeta.Descriptivo.Value = r.Descriptivo.Value;
                    rDeta.Renglon.Value = r.Renglon.Value;
                    rDeta.Valor.Value = 0;
                    rDeta.ValorAjustado.Value = 0;
                    rDeta.SignoSuma.Value = r.SignoSuma.Value;

                    #endregion
                    #region Carga el comprobante y datos del renglon
                    decimal vlrAjuste = 0;
                    decimal vlrLoc = 0;
                    decimal vlrExt = 0;

                    List<DTO_ComprobanteFooter> tempFooter = this.CalcularImpuestoRenglon(imp, r, fechaIni, fechaFin, detaCuentas, ref vlrLoc, ref vlrExt, ref vlrAjuste, tc, isMultimoneda);
                    rDeta.Valor.Value = vlrLoc;
                    rDeta.ValorAjustado.Value = vlrAjuste;

                    totalAjuste += vlrAjuste;
                    vlrTotalLoc += vlrLoc;
                    vlrTotalExtra += vlrExt;
                    #endregion

                    detaRenglones.Add(rDeta);
                    compFooter.AddRange(tempFooter);
                }

                return compFooter;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetImpuestoDetalle");
                throw ex;
            }
        }

        /// <summary>
        /// Calcula la declaracion de un renglon
        /// </summary>
        /// <param name="imp">Impuesto a declarar</param>
        /// <param name="renglon">Renglon</param>
        /// <param name="fechaIni">Periodo inicial de consulta</param>
        /// <param name="fechaFin">Periodo final de consulta</param>
        /// <param name="detaCuentas">Lista del detalle de las cuentas</param>
        /// <param name="vlrTotalLoc">Valor total local(temp) del comprobante</param>
        /// <param name="vlrTotalExtra">Valor total extranjero(temp) del comprobante</param>
        /// <param name="totalAjuste">Valor total local(temp) del ajuste</param>
        /// <returns>Retorna la lista de comprobante de la declaracion</returns>
        private List<DTO_ComprobanteFooter> CalcularImpuestoRenglon(DTO_coImpuestoDeclaracion imp, DTO_coImpDeclaracionRenglon renglon, DateTime fechaIni,
            DateTime fechaFin, List<DTO_coImpDeclaracionDetaCuenta> detaCuentas, ref decimal vlrTotalLoc, ref decimal vlrTotalExtra, ref decimal totalAjuste,
            decimal tc, bool isMultimoneda)
        {
            List<DTO_ComprobanteFooter> compFooter = new List<DTO_ComprobanteFooter>();

            try
            {
                #region Valores por defecto
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                #endregion
                #region Trae las cuentas x renglon de los impuestos
                List<string> ctas = new List<string>();

                #region Filtros
                DTO_glConsulta filtro = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                //Filtro de tipo de impuesto
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ImpuestoDeclID",
                    ValorFiltro = imp.ID.Value,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });

                //Filtro del renglon
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "Renglon",
                    ValorFiltro = renglon.Renglon.Value,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });


                filtro.Filtros = filtros;
                #endregion
                #region Cuentas
                DAL_MasterComplex _dal_MasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                _dal_MasterComplex.DocumentID = AppMasters.coImpDeclaracionCuenta;
                long count = _dal_MasterComplex.DAL_MasterComplex_Count(filtro, true);

                List<DTO_MasterComplex> complexCtasRenglon = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, filtro, true).ToList();
                List<DTO_coImpDeclaracionCuenta> masterCuentas = complexCtasRenglon.Cast<DTO_coImpDeclaracionCuenta>().ToList();

                if (masterCuentas.Count == 0)
                    return compFooter;
                else
                {
                    masterCuentas.ForEach(x =>
                    {
                        ctas.Add(x.CuentaID.Value);
                    });
                }

                #endregion
                #endregion
                #region Trae la lista de cuentas de los comprobantes asociados al renglon
                string terceroCta;
                string concSaldo;
                DTO_coPlanCuenta dtoCta;
                Dictionary<string, string> ctasNit = new Dictionary<string, string>();
                Dictionary<string, string> ctaConsSaldo = new Dictionary<string, string>();

                List<DTO_ComprobanteFooter> footer = this._dal_Contabilidad.DAL_Contabilidad_GetAuxiliaresForImpuesto(imp, renglon, ctas, fechaIni, fechaFin);
                foreach (DTO_ComprobanteFooter det in footer)
                {
                    //Indica si ya se sumo la info del detalle
                    bool alreadySum = false;
                    int valAjuste = Evaluador.RedondearEntero(Convert.ToInt32(det.vlrMdaLoc.Value.Value), imp.DigitoAprox.Value.Value);

                    #region Crea el registro de la cuenta por renglon
                    DTO_coImpDeclaracionDetaCuenta detaCta = new DTO_coImpDeclaracionDetaCuenta();
                    detaCta.EmpresaID.Value = this.Empresa.ID.Value;
                    detaCta.Renglon.Value = renglon.Renglon.Value;
                    detaCta.CuentaID.Value = det.CuentaID.Value;
                    detaCta.VlrBaseML.Value = det.vlrBaseML.Value.Value;
                    detaCta.ValorML.Value = det.vlrMdaLoc.Value.Value;

                    detaCuentas.Add(detaCta);
                    #endregion

                    SignoSuma signo = (SignoSuma)Enum.Parse(typeof(SignoSuma), renglon.SignoSuma.Value.Value.ToString());
                    if (!alreadySum && signo != SignoSuma.NoAplica)
                    {
                        #region Trae la informacion del tercero y el conc de saldo
                        if (ctasNit.ContainsKey(det.CuentaID.Value))
                        {
                            terceroCta = ctasNit[det.CuentaID.Value];
                            concSaldo = ctaConsSaldo[det.CuentaID.Value];
                        }
                        else
                        {
                            dtoCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, det.CuentaID.Value, true, false);
                            terceroCta = dtoCta.NITCierreAnual.Value;
                            concSaldo = dtoCta.ConceptoSaldoID.Value;

                            ctasNit.Add(det.CuentaID.Value, terceroCta);
                            ctaConsSaldo.Add(det.CuentaID.Value, concSaldo);
                        }
                        #endregion
                        #region Completa la informacion del auxiliar
                        det.TerceroID.Value = terceroCta;
                        det.ProyectoID.Value = proyDef;
                        det.CentroCostoID.Value = ctoCostoDef;
                        det.LineaPresupuestoID.Value = linPresDef;
                        det.ConceptoCargoID.Value = concCargoDef;
                        det.LugarGeograficoID.Value = imp.LugarGeograficoID.Value;
                        det.PrefijoCOM.Value = prefijoDef;
                        det.DocumentoCOM.Value = "imp";
                        det.ActivoCOM.Value = string.Empty;
                        det.ConceptoSaldoID.Value = concSaldo;
                        det.IdentificadorTR.Value = 0;
                        det.Descriptivo.Value = string.Empty;
                        det.TasaCambio.Value = tc;
                        det.vlrBaseML.Value = det.vlrBaseML.Value.Value;
                        det.vlrBaseME.Value = isMultimoneda ? Math.Round(det.vlrBaseML.Value.Value / tc, 2) : 0;
                        det.vlrMdaLoc.Value = det.vlrMdaLoc.Value.Value;
                        det.vlrMdaExt.Value = isMultimoneda ? Math.Round(det.vlrMdaLoc.Value.Value / tc, 2) : 0;
                        det.vlrMdaOtr.Value = det.vlrMdaLoc.Value;
                        #endregion
                        #region Asigna el signo de los valores
                        if (signo == SignoSuma.Suma)
                        {
                            vlrTotalLoc += det.vlrMdaLoc.Value.Value;
                            vlrTotalExtra += det.vlrMdaExt.Value.Value;
                            totalAjuste += valAjuste;
                        }
                        else
                        {
                            vlrTotalLoc -= det.vlrMdaLoc.Value.Value;
                            vlrTotalExtra -= det.vlrMdaExt.Value.Value;
                            totalAjuste -= valAjuste;
                        }

                        #endregion

                        if (renglon.PagoInd.Value.Value)
                            compFooter.Add(det);
                    }
                }
                #endregion

                return compFooter;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CalcularImpuestoRenglon");
                throw ex;
            }
        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Trae una cuenta segun un concepto de cargo y un proyecto
        /// </summary>
        /// <param name="mod">Modulo que realiza la consulta</param>
        /// <param name="tercero">Tercero que esta ejecutando la consulta</param>
        /// <param name="cuentaCosto">Cuenta de costo (si existe)</param>
        /// <param name="valor">Valor sobre el cual se esta trabajando</param>
        /// <param name="conceptoCargoID">Identificador del concepto de cargo</param>
        /// <param name="lugarGeoID">Identificador del lugar geografico</param>
        /// <param name="lineaPresID">Identificador de la linea presupuestal</param>
        /// <param name="operacionID">Identificador de la operacion</param>
        /// <returns>Retorna una lista de cuentas</returns>
        public List<DTO_SerializedObject> LiquidarImpuestos(ModulesPrefix mod, DTO_coTercero tercero, string cuentaCosto, string conceptoCargoID, string operacionID, string lugarGeoID, string lineaPresID, decimal valor, string conceptoCargo2 = null)
        {
            string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            try
            {
                #region Variables
                //Resultados
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_SerializedObject> ctasImpuestos = new List<DTO_SerializedObject>();

                //Tercero de la empresa
                DTO_coTercero terceroEmp = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroPorDefecto, true, false);
                string regFisEmpID = terceroEmp.ReferenciaID.Value;
                string regFisTerceroID = tercero.ReferenciaID.Value;

                string tipoImpReteFte = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                Dictionary<string, string> pks = new Dictionary<string, string>();
                #endregion
                #region Trae la cuenta de costo (de ser necesario)
                if (string.IsNullOrEmpty(cuentaCosto))
                {
                    DTO_plLineaPresupuesto dtoLP = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, lineaPresID, true, false);
                    if (dtoLP.TablaControlInd.Value.Value)
                        lineaPresID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);

                    DTO_CuentaValor ctaCosto = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(conceptoCargoID, operacionID, lineaPresID, valor);
                    if (ctaCosto != null)
                    {
                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaCosto.CuentaID.Value, true, false);

                        ctaCosto.LugarGeograficoID = lugarGeoID;
                        ctaCosto.TipoImpuesto = cta.ImpuestoTipoID.Value;
                        cuentaCosto = ctaCosto.CuentaID.Value;
                        ctasImpuestos.Add(ctaCosto);
                    }
                    else
                        return ctasImpuestos;
                }
                #endregion
                List<DTO_SerializedObject> ctasImp = new List<DTO_SerializedObject>();
                #region Trae las cuentas de coImpuesto
                List<DTO_SerializedObject> ctasCoImp = this._moduloGlobal.coImpuesto_GetCuentasByPK(mod, tercero, conceptoCargoID, lugarGeoID, valor, conceptoCargo2);
                if (ctasCoImp.Count > 0)
                    if (ctasCoImp.First().GetType() == typeof(DTO_TxResult))
                        return ctasCoImp;

                ctasImp.AddRange(ctasCoImp);
                #endregion
                #region Trae las cuentas de coImpuestoLocal
                List<DTO_SerializedObject> ctasCoImpLocal = this._moduloGlobal.coImpuestoLocal_GetCuentasByPK(mod, tercero, tercero.ActEconomicaID.Value, lugarGeoID, valor);
                if (ctasCoImpLocal.Count > 0)
                    if (ctasCoImpLocal.First().GetType() == typeof(DTO_TxResult))
                        return ctasCoImpLocal;

                ctasImp.AddRange(ctasCoImpLocal);
                #endregion
                #region Trae las cuentas de reteIVA
                List<DTO_CuentaValor> ctasReteIVA = new List<DTO_CuentaValor>();
                if (!tercero.AutoRetIVAInd.Value.Value)
                {
                    DTO_coPlanCuenta ctaRete;
                    foreach (DTO_CuentaValor ctaVal in ctasImp)
                    {
                        string ctaID = ctaVal.CuentaID.Value;
                        pks = new Dictionary<string, string>();
                        pks.Add("RegimenFiscalEmpresaID", regFisEmpID);
                        pks.Add("RegimenFiscalTerceroID", regFisTerceroID);
                        pks.Add("CuentaIVA", ctaID);

                        DTO_coIvaRetencion ivaRete = (DTO_coIvaRetencion)this.GetMasterComplexDTO(AppMasters.coIVARetencion, pks, true);
                        if (ivaRete != null)
                        {
                            ctaRete = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ivaRete.CuentaReteIVA.Value, true, false);
                            decimal vlrCta = ctaRete.ImpuestoPorc != null && ctaRete.ImpuestoPorc.Value.HasValue ? valor * ctaRete.ImpuestoPorc.Value.Value / 100 : 0;
                            decimal baseCta = ctaVal.Base.Value.Value;

                            if (valor < 0)
                            {
                                vlrCta *= -1;
                                baseCta *= -1;
                            }

                            if (ctaRete.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                                vlrCta *= -1;

                            DTO_CuentaValor ctaValRete = new DTO_CuentaValor(ctaRete.ID.Value, vlrCta, baseCta, lugarGeoID, ctaRete.ImpuestoTipoID.Value);
                            ctasReteIVA.Add(ctaValRete);
                        }
                    }
                }
                #endregion
                #region Revisa las cuentas de IVA para ver si tienen costo
                //Trae la operacion
                DTO_coOperacion operacion = (DTO_coOperacion)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, operacionID, true, false);

                //Trae la info de coValIVA
                pks = new Dictionary<string, string>();
                pks.Add("RegimenFiscalEmpresaID", regFisEmpID);
                pks.Add("RegimenFiscalTerceroID", regFisTerceroID);
                pks.Add("OperacionID", operacionID);
                pks.Add("ConceptoCargoID", conceptoCargoID);

                DTO_coValIVA coValIVA = (DTO_coValIVA)this.GetMasterComplexDTO(AppMasters.coValIVA, pks, true);
                DTO_coPlanCuenta ctaIVA;

                string impIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                foreach (DTO_CuentaValor cta in ctasImp)
                {
                    ctaIVA = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, cta.CuentaID.Value, true, false);
                    if (ctaIVA.ImpuestoTipoID.Value == impIVA)
                    {
                        if (operacion.IvaCostoInd.Value.Value && coValIVA != null)
                            cta.CuentaID.Value = cuentaCosto;
                        else
                            cta.IVADescontable = true;
                    }
                }

                #endregion

                ctasImpuestos.AddRange(ctasImp);
                if (!tercero.AutoRetIVAInd.Value.Value)
                    ctasImpuestos.AddRange(ctasReteIVA);

                return ctasImpuestos;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloContabilidad_LiquidarImpuestos");

                List<DTO_SerializedObject> nl = new List<DTO_SerializedObject>();
                nl.Add(result);
                return nl;
            }
        }

        /// <summary>
        /// Trae la lista de declaracion de impuestos para un periodo
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>Retorna la lista de declaraciones</returns>
        public List<DTO_DeclaracionImpuesto> DeclaracionesImpuestos_Get(DateTime periodo)
        {
            try
            {
                int year = periodo.Year;
                int month = periodo.Month;

                DateTime fechaIni = new DateTime(year, month, 1);
                DateTime fechaFin = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                this._dal_Impuesto = (DAL_Impuesto)base.GetInstance(typeof(DAL_Impuesto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_DeclaracionImpuesto> results = this._dal_Impuesto.DAL_Impuesto_GetDeclaracionesByPeriodo(fechaIni, fechaFin);

                #region Asigna los estados y el periodo
                if (results.Count > 0)
                {
                    this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    Int16 estado = 0;
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    Dictionary<int, Int16> estados = new Dictionary<int, Int16>();
                    foreach (DTO_DeclaracionImpuesto imp in results)
                    {
                        #region Asigna el estado
                        if (imp.NumeroDoc != null && imp.NumeroDoc.Value.HasValue)
                        {
                            if (estados.ContainsKey(imp.NumeroDoc.Value.Value))
                                estado = estados[imp.NumeroDoc.Value.Value];
                            else
                            {
                                ctrl = this._moduloGlobal.glDocumentoControl_GetByID(imp.NumeroDoc.Value.Value);
                                estado = ctrl.Estado.Value.Value;
                                estados.Add(imp.NumeroDoc.Value.Value, estado);
                            }

                            EstadoDocControl est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), estado.ToString());
                            imp.Estado.Value = Convert.ToByte(estado);
                            imp.EstadoRsx = est.ToString();
                        }
                        #endregion
                        #region Asigna el periodo
                        DTO_coImpuestoDeclaracion decImp = (DTO_coImpuestoDeclaracion)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, imp.ImpuestoDeclID.Value, true, false);
                        PeriodoFiscal pFiscal = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), decImp.PeriodoDeclaracion.Value.Value.ToString());
                        switch (pFiscal)
                        {
                            case PeriodoFiscal.Anual:
                                imp.PeriodoConsulta.Value = 1;
                                break;
                            case PeriodoFiscal.Semestral:
                                #region Periodo Semestral
                                if (imp.PeriodoCalendario.Value.Value == 1)
                                    imp.PeriodoConsulta.Value = 1;
                                else
                                    imp.PeriodoConsulta.Value = 6;
                                #endregion
                                break;
                            case PeriodoFiscal.Trimestral:
                                #region Periodo Trimestral
                                if (imp.PeriodoCalendario.Value.Value == 1)
                                    imp.PeriodoConsulta.Value = 1;
                                else if (imp.PeriodoCalendario.Value.Value == 2)
                                    imp.PeriodoConsulta.Value = 4;
                                else if (imp.PeriodoCalendario.Value.Value == 3)
                                    imp.PeriodoConsulta.Value = 7;
                                else
                                    imp.PeriodoConsulta.Value = 10;
                                #endregion
                                break;
                            case PeriodoFiscal.Bimestral:
                                #region Periodo Bimestral
                                if (imp.PeriodoCalendario.Value.Value == 1)
                                    imp.PeriodoConsulta.Value = 1;
                                else if (imp.PeriodoCalendario.Value.Value == 2)
                                    imp.PeriodoConsulta.Value = 3;
                                else if (imp.PeriodoCalendario.Value.Value == 3)
                                    imp.PeriodoConsulta.Value = 5;
                                else if (imp.PeriodoCalendario.Value.Value == 4)
                                    imp.PeriodoConsulta.Value = 7;
                                else if (imp.PeriodoCalendario.Value.Value == 5)
                                    imp.PeriodoConsulta.Value = 9;
                                else
                                    imp.PeriodoConsulta.Value = 11;
                                #endregion
                                break;
                            case PeriodoFiscal.Mensual:
                                #region Periodo Mensual
                                imp.PeriodoConsulta.Value = imp.PeriodoCalendario.Value.Value;
                                #endregion
                                break;

                        }

                        #endregion
                    }
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DeclaracionesImpuestos_Get");

                return null;
            }
        }

        /// <summary>
        /// Trae la lista de renglones de una declaracion
        /// </summary>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <returns>Retorna la lista de renglones</returns>
        public List<DTO_coImpDeclaracionDetaRenglon> DeclaracionesRenglones_Get(int numeroDoc, string impuestoID, short mesDeclaracion, short añoDeclaracion)
        {
            try
            {
                List<DTO_coImpDeclaracionDetaRenglon> results = new List<DTO_coImpDeclaracionDetaRenglon>();

                if (numeroDoc != 0)
                {
                    DAL_coImpDeclaracionDetaRenglon _dal_coImpDeclaracionDetaRenglon = new DAL_coImpDeclaracionDetaRenglon(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    results = _dal_coImpDeclaracionDetaRenglon.DAL_coImpDeclaracionDetaRenglon_Get(numeroDoc);
                }
                else
                {
                    List<DTO_coImpDeclaracionDetaCuenta> detaCuentas = new List<DTO_coImpDeclaracionDetaCuenta>();
                    List<DTO_coImpDeclaracionDetaRenglon> detaRenglones = new List<DTO_coImpDeclaracionDetaRenglon>();

                    decimal vlrTotal = 0;
                    decimal vlrTotalExtra = 0;
                    decimal totalAjuste = 0;

                    List<DTO_ComprobanteFooter> details = this.GetImpuestoDetalle(impuestoID, mesDeclaracion, añoDeclaracion, detaRenglones, detaCuentas,
                        out vlrTotal, out totalAjuste, out vlrTotalExtra, false, 0);

                    results = detaRenglones;
                }


                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DeclaracionesRenglones_Get");

                return null;
            }
        }

        /// <summary>
        /// Trae los detalles de un renglon
        /// </summary>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="renglon">Renglon</param>
        /// <param name="mesDeclaracion">Mes de la declaracion</param>
        /// <param name="añoDeclaracion">Año de la declaracion</param>
        /// <returns>Retorna la lista de cuentas del detalle</returns>
        public List<DTO_DetalleRenglon> DetallesRenglon_Get(string impuestoID, string renglon, short mesDeclaracion, short añoDeclaracion)
        {
            try
            {
                List<DTO_DetalleRenglon> results = new List<DTO_DetalleRenglon>();
                #region Variables
                DTO_coImpuestoDeclaracion imp = (DTO_coImpuestoDeclaracion)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, impuestoID, true, false);
                DTO_coImpDeclaracionRenglon renglonDTO;
                #endregion
                #region Carga el periodo de consulta
                DateTime fechaIni = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                PeriodoFiscal pFiscal = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), imp.PeriodoDeclaracion.Value.Value.ToString());
                switch (pFiscal)
                {
                    case PeriodoFiscal.Anual:
                        #region Periodo Anual
                        fechaIni = new DateTime(añoDeclaracion, 1, 1);
                        fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        #endregion
                        break;
                    case PeriodoFiscal.Semestral:
                        #region Periodo Semestral
                        if (mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Trimestral:
                        #region Periodo Trimestral
                        if (mesDeclaracion <= 3)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 3, 31);
                        }
                        else if (mesDeclaracion > 3 && mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 4, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else if (mesDeclaracion > 3 && mesDeclaracion <= 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 9, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 10, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Bimestral:
                        #region Periodo Bimestral
                        if (mesDeclaracion == 1 || mesDeclaracion == 2)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 1, 1);
                            fechaFin = new DateTime(añoDeclaracion, 2, DateTime.DaysInMonth(añoDeclaracion, 2));
                        }
                        else if (mesDeclaracion == 3 || mesDeclaracion == 4)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 3, 1);
                            fechaFin = new DateTime(añoDeclaracion, 4, 30);
                        }
                        else if (mesDeclaracion == 5 || mesDeclaracion == 6)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 5, 1);
                            fechaFin = new DateTime(añoDeclaracion, 6, 30);
                        }
                        else if (mesDeclaracion == 7 || mesDeclaracion == 8)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 7, 1);
                            fechaFin = new DateTime(añoDeclaracion, 8, 30);
                        }
                        else if (mesDeclaracion == 9 || mesDeclaracion == 10)
                        {
                            fechaIni = new DateTime(añoDeclaracion, 9, 1);
                            fechaFin = new DateTime(añoDeclaracion, 10, 30);
                        }
                        else
                        {
                            fechaIni = new DateTime(añoDeclaracion, 11, 1);
                            fechaFin = new DateTime(añoDeclaracion, 12, 31);
                        }
                        #endregion
                        break;
                    case PeriodoFiscal.Mensual:
                        #region Periodo Mensual
                        fechaIni = new DateTime(añoDeclaracion, mesDeclaracion, 1);
                        fechaFin = new DateTime(añoDeclaracion, mesDeclaracion, DateTime.DaysInMonth(añoDeclaracion, mesDeclaracion));
                        #endregion
                        break;
                }
                #endregion
                #region Carga la info del renglon
                Dictionary<string, string> pks = new Dictionary<string, string>();
                pks.Add("ImpuestoDeclID", impuestoID);
                pks.Add("Renglon", renglon);

                renglonDTO = (DTO_coImpDeclaracionRenglon)this.GetMasterComplexDTO(AppMasters.coImpDeclaracionRenglon, pks, true);
                #endregion
                #region Trae las cuentas x renglon de los impuestos
                List<string> ctas = new List<string>();

                #region Filtros
                DTO_glConsulta filtro = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                //Filtro de tipo de impuesto
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ImpuestoDeclID",
                    ValorFiltro = imp.ID.Value,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });

                //Filtro del renglon
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "Renglon",
                    ValorFiltro = renglon,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });


                filtro.Filtros = filtros;
                #endregion
                #region Cuentas
                DAL_MasterComplex _dal_MasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                _dal_MasterComplex.DocumentID = AppMasters.coImpDeclaracionCuenta;
                long count = _dal_MasterComplex.DAL_MasterComplex_Count(filtro, true);

                List<DTO_MasterComplex> complexCtasRenglon = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, filtro, true).ToList();
                List<DTO_coImpDeclaracionCuenta> masterCuentas = complexCtasRenglon.Cast<DTO_coImpDeclaracionCuenta>().ToList();

                if (masterCuentas.Count == 0)
                    return results;
                else
                {
                    masterCuentas.ForEach(x =>
                    {
                        ctas.Add(x.CuentaID.Value);
                    });
                }

                #endregion
                #endregion

                this._dal_Impuesto = (DAL_Impuesto)this.GetInstance(typeof(DAL_Impuesto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                results = this._dal_Impuesto.DAL_Impuesto_GetCuentasByRenglon(imp, renglonDTO, ctas, fechaIni, fechaFin);

                return results;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DetallesRenglon_Get");
                return null;
            }

        }

        /// <summary>
        /// Procesa una declaracion
        /// </summary>
        /// <param name="documentID">Identificador del documnto que genera el proceso</param>
        /// <param name="impuestoID">Identificador de la declaracion</param>
        /// <param name="mesDeclaracion">Mes de declaracion</param>
        /// <param name="añoDeclaracion">Año de declaracion</param>
        /// <param name="numeroDoc">Numero de documento (si ya fue procesado previamente)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ProcesarDeclaracion(int documentID, string impuestoID, short periodoCalendario, short mesDeclaracion, short añoDeclaracion, int? numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;


            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrl = null;
            DTO_coComprobante coComp = null;
            try
            {
                decimal porcTotal = 0;
                decimal porcParte = 100 / 5;

                #region Dals y modulos
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloCxP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DAL_coImpDeclaracionDetaCuenta _dal_coImpDeclaracionDetaCuenta = new DAL_coImpDeclaracionDetaCuenta(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DAL_coImpDeclaracionDetaRenglon _dal_coImpDeclaracionDetaRenglon = new DAL_coImpDeclaracionDetaRenglon(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DAL_coImpDeclaracionDocu _dal_coImpDeclaracionDocu = new DAL_coImpDeclaracionDocu(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DAL_MasterComplex _dal_MasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #endregion
                #region Variables
                //Variables para carga de datos
                bool isMultimoneda = this.Multimoneda();
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string docTercero = impuestoID + " (" + añoDeclaracion.ToString() + "/" + mesDeclaracion.ToString() + ")";
                DateTime periodoDoc = new DateTime(añoDeclaracion, mesDeclaracion, 1);
                decimal tc = 0;
                //DTOs
                DTO_cpCuentaXPagar cxp = new DTO_cpCuentaXPagar();
                DTO_coImpuestoDeclaracion imp = (DTO_coImpuestoDeclaracion)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, impuestoID, true, false);
                //Variables por defecto
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string proyDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string ctoCostoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string linPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                //Listas
                List<DTO_coImpDeclaracionDetaCuenta> detaCuentas = new List<DTO_coImpDeclaracionDetaCuenta>();
                List<DTO_coImpDeclaracionDetaRenglon> detaRenglones = new List<DTO_coImpDeclaracionDetaRenglon>();
                //Valores
                decimal vlrTotal = 0;
                decimal vlrTotalExtra = 0;
                decimal totalAjuste = 0;
                #endregion
                #region Valida que exista la tasa de Cambio
                if (isMultimoneda)
                {
                    DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);
                    if (tc == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_NoTasaCambio;

                        return result;
                    }
                }
                #endregion
                #region Guarda la informacion
                #region Documento y CxP
                if (numeroDoc.HasValue)
                {
                    #region Documento existente
                    ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc.Value);

                    //Revisa que no haya sido aprobado
                    if (ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.DocumentAprob;
                        return result;
                    }

                    #region Borra la informacion de las tablas
                    _dal_coImpDeclaracionDetaCuenta.DAL_coImpDeclaracionDetaCuenta_Delete(numeroDoc.Value);
                    _dal_coImpDeclaracionDetaRenglon.DAL_coImpDeclaracionDetaRenglon_Delete(numeroDoc.Value);
                    #endregion
                    #region Documento Control

                    ctrl.Fecha.Value = DateTime.Now;
                    ctrl.PeriodoDoc.Value = periodoDoc;
                    ctrl.PeriodoUltMov.Value = DateTime.Now;
                    ctrl.TasaCambioDOCU.Value = tc;
                    ctrl.TasaCambioCONT.Value = tc;
                    ctrl.seUsuarioID.Value = this.UserId;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Update(ctrl, true, true);
                    if (resultGLDC.Message == "NOK")
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);

                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region CXP
                    cxp = this._moduloCxP.CuentasXPagar_Get(numeroDoc.Value);

                    cxp.RadicaFecha.Value = DateTime.Now;
                    cxp.Valor.Value = totalAjuste;
                    cxp.FacturaFecha.Value = DateTime.Now;
                    cxp.VtoFecha.Value = DateTime.Now;
                    cxp.TerceroID.Value = ctrl.TerceroID.Value;
                    result = this._moduloCxP.CuentasXPagar_Upd(cxp);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #endregion
                }
                else
                {
                    #region Nuevo documento
                    DTO_seUsuario user = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                    string concCXPID = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoConceptoCxPImp);

                    DTO_cpConceptoCXP concCXP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, concCXPID, true, false); ;
                    DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, concCXP.coDocumentoID.Value, true, false);

                    string ctaCtrlID = coDoc.CuentaLOC.Value;
                    coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                    #region Documento Control
                    ctrl = new DTO_glDocumentoControl();

                    ctrl.TerceroID.Value = terceroPorDefecto;
                    ctrl.DocumentoTercero.Value = docTercero;
                    ctrl.MonedaID.Value = mdaLoc;
                    ctrl.CuentaID.Value = ctaCtrlID;
                    ctrl.EmpresaID.Value = this.Empresa.ID.Value;
                    ctrl.ProyectoID.Value = proyDef;
                    ctrl.CentroCostoID.Value = ctoCostoDef;
                    ctrl.PeriodoDoc.Value = periodoDoc;
                    ctrl.PrefijoID.Value = prefijoDef;
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.AreaFuncionalID.Value = user.AreaFuncionalID.Value;
                    ctrl.ComprobanteID.Value = coComp.ID.Value;
                    ctrl.ComprobanteIDNro.Value = 0;
                    ctrl.DocumentoID.Value = AppDocuments.CausarFacturas;
                    ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                    ctrl.PeriodoUltMov.Value = DateTime.Now;
                    ctrl.Fecha.Value = DateTime.Now;
                    ctrl.TasaCambioDOCU.Value = tc;
                    ctrl.TasaCambioCONT.Value = tc;
                    ctrl.seUsuarioID.Value = this.UserId;
                    ctrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    ctrl.LugarGeograficoID.Value = lgDef;
                    ctrl.LineaPresupuestoID.Value = linPresDef;
                    ctrl.ConsSaldo.Value = 0;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);

                        return result;
                    }
                    ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    #endregion
                    #region CXP

                    cxp.EmpresaID.Value = Empresa.ID.Value;
                    cxp.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    cxp.RadicaFecha.Value = DateTime.Now;
                    cxp.ConceptoCxPID.Value = concCXPID;
                    cxp.Valor.Value = totalAjuste;
                    cxp.IVA.Value = 0;
                    cxp.MonedaPago.Value = mdaLoc;
                    cxp.FacturaFecha.Value = DateTime.Now;
                    cxp.VtoFecha.Value = DateTime.Now;
                    cxp.DistribuyeImpLocalInd.Value = false;
                    cxp.TerceroID.Value = ctrl.TerceroID.Value;
                    result = this._moduloCxP.CuentasXPagar_Add(cxp);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                    #region Impuesto Documento
                    DTO_coImpDeclaracionDocu impDocu = new DTO_coImpDeclaracionDocu();
                    impDocu.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    impDocu.ImpuestoDeclID.Value = impuestoID;
                    impDocu.Año.Value = añoDeclaracion;
                    impDocu.Periodo.Value = (Byte)mesDeclaracion;

                    _dal_coImpDeclaracionDocu.DAL_coImpDeclaracionDocu_Add(impDocu);
                    #endregion
                    #endregion
                }
                #endregion
                #region Crea el comprobante
                DTO_Comprobante comp = new DTO_Comprobante();
                #region Header
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.ComprobanteID.Value = ctrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = ctrl.ComprobanteIDNro.Value;
                header.Fecha.Value = DateTime.Now;
                header.MdaTransacc.Value = mdaLoc;
                header.MdaOrigen.Value = (byte)TipoMoneda_LocExt.Local;
                header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                header.PeriodoID.Value = periodoDoc;
                header.TasaCambioBase.Value = ctrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

                comp.Header = header;
                #endregion
                #region Footer
                #region Trae la informacion del detalle del comprobante
                List<DTO_ComprobanteFooter> compFooter = this.GetImpuestoDetalle(impuestoID, mesDeclaracion, añoDeclaracion, detaRenglones, detaCuentas,
                    out vlrTotal, out vlrTotalExtra, out totalAjuste, isMultimoneda, tc);

                comp.Footer = compFooter;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region Valor Extra Detalle

                decimal vlrLoc = Math.Abs(vlrTotal - totalAjuste);
                if (vlrTotal < totalAjuste)
                    vlrLoc *= -1;

                string ctaExtraID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaAjusteImpuesto);
                DTO_coPlanCuenta dtoCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaExtraID, true, false);
                if (dtoCta == null || string.IsNullOrEmpty(dtoCta.NITCierreAnual.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CtaAjImpuesto;

                    return result;
                }

                DTO_ComprobanteFooter detExtra = new DTO_ComprobanteFooter();
                detExtra.CuentaID.Value = ctaExtraID;
                detExtra.TerceroID.Value = dtoCta.NITCierreAnual.Value;
                detExtra.ProyectoID.Value = proyDef;
                detExtra.CentroCostoID.Value = ctoCostoDef;
                detExtra.LineaPresupuestoID.Value = linPresDef;
                detExtra.ConceptoCargoID.Value = concCargoDef;
                detExtra.LugarGeograficoID.Value = imp.LugarGeograficoID.Value;
                detExtra.PrefijoCOM.Value = prefijoDef;
                detExtra.DocumentoCOM.Value = "imp - extra";
                detExtra.ActivoCOM.Value = string.Empty;
                detExtra.ConceptoSaldoID.Value = dtoCta.ConceptoSaldoID.Value;
                detExtra.IdentificadorTR.Value = 0;
                detExtra.Descriptivo.Value = string.Empty;
                detExtra.TasaCambio.Value = tc;
                detExtra.vlrBaseML.Value = 0;
                detExtra.vlrBaseME.Value = 0;
                detExtra.vlrMdaLoc.Value = vlrLoc;
                detExtra.vlrMdaExt.Value = isMultimoneda ? Math.Round(detExtra.vlrMdaLoc.Value.Value / tc, 2) : 0;
                detExtra.vlrMdaOtr.Value = detExtra.vlrMdaLoc.Value;

                comp.Footer.Add(detExtra);
                #endregion
                #region ContraPartida
                DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(ctrl, tc, concCargoDef, ctrl.LugarGeograficoID.Value, ctrl.LineaPresupuestoID.Value,
                vlrTotal * -1, vlrTotalExtra * -1, true);

                comp.Footer.Add(contraP);
                #endregion
                #endregion
                #region Agregar a detaCuentas y detaRenglones el valor del numeroDoc
                foreach (DTO_coImpDeclaracionDetaCuenta cuenta in detaCuentas)
                {
                    cuenta.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    _dal_coImpDeclaracionDetaCuenta.DAL_coImpDeclaracionDetaCuenta_Add(cuenta);
                }

                foreach (DTO_coImpDeclaracionDetaRenglon renglon in detaRenglones)
                {
                    renglon.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                    _dal_coImpDeclaracionDetaRenglon.DAL_coImpDeclaracionDetaRenglon_Add(renglon);
                }
                #endregion

                result = this.ComprobantePre_Add(documentID, ModulesPrefix.co, comp, ctrl.AreaFuncionalID.Value, prefijoDef, false, ctrl.NumeroDoc.Value, null, new Dictionary<Tuple<int, int>, int>(), true);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region Actualiza coImpDeclaracionCalendario
                Dictionary<string, string> pks = new Dictionary<string, string>();
                pks.Add("ImpuestoDeclID", impuestoID);
                pks.Add("AñoFiscal", añoDeclaracion.ToString());
                pks.Add("Periodo", periodoCalendario.ToString());

                _dal_MasterComplex.DocumentID = AppMasters.coImpDeclaracionCalendario;
                DTO_coImpDeclaracionCalendario calendario = (DTO_coImpDeclaracionCalendario)_dal_MasterComplex.DAL_MasterComplex_GetByID(pks, true);
                calendario.NumeroDoc.Value = ctrl.NumeroDoc.Value;

                _dal_MasterComplex.DAL_MasterComplex_Update(calendario, true);
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                result.ResultMessage = ctrl.NumeroDoc.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ProcesarDeclaracion");

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

                        if (!numeroDoc.HasValue)
                        {
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, 0);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, true);
                            this.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, true);
                        }
                    }
                    else
                        throw new Exception("ProcesarDeclaracion - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #endregion

        #region Reclasificaciones Fiscales

        #region Funciones Privadas

        /// <summary>
        /// genera un comprobante a partir de la reclasificacion (El comprobante Nro queda en 0)
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="origen">Origen del cual se va a obtener la informacion del auxiliar</param>
        /// <returns>Retorna la lista de comprobantes</returns>
        private List<DTO_ComprobanteFooter> ReclasificarComprobante(DateTime periodo, string tipoBalance)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coReclasificaBalExcluye = (DAL_coReclasificaBalExcluye)this.GetInstance(typeof(DAL_coReclasificaBalExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coReclasificaBalance = (DAL_coReclasificaBalance)this.GetInstance(typeof(DAL_coReclasificaBalance), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                string lugarGeoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);

                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                List<DTO_coReclasificaBalance> origenes = this.ReclasificacionFiscal_GetForProcess(tipoBalance);

                Dictionary<string, string> cacheCtas = new Dictionary<string, string>();
                DTO_coPlanCuenta cta;

                #endregion

                decimal vlrML = 0;
                decimal vlrME = 0;

                foreach (DTO_coReclasificaBalance origen in origenes)
                {
                    #region Carga el footer
                    List<DTO_coReclasificaBalance> destinos = this._dal_coReclasificaBalance.DAL_coReclasificaBalance_GetByID(origen.Consecutivo.Value.Value);
                    List<DTO_coReclasificaBalExcluye> excls = this._dal_coReclasificaBalExcluye.DAL_coReclasificaBalExcluye_GetByConsecutivo(origen.Consecutivo.Value.Value);

                    List<DTO_coCuentaSaldo> saldos = this._dal_Contabilidad.DAL_Contabilidad_GetSaldosForReclasificacion(periodo, origen, excls, tipoBalance);

                    foreach (DTO_coCuentaSaldo saldo in saldos)
                    {
                        foreach (DTO_coReclasificaBalance destino in destinos)
                        {

                            #region Carga el detalle del comprobante
                            DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();

                            #region Cuenta y concepto de saldo
                            if (string.IsNullOrWhiteSpace(destino.CuentaDEST.Value))
                            {
                                detalle.CuentaID.Value = saldo.CuentaID.Value;
                                detalle.IdentificadorTR.Value = saldo.IdentificadorTR.Value;
                                if (!cacheCtas.ContainsKey(detalle.CuentaID.Value))
                                    cacheCtas.Add(saldo.CuentaID.Value, saldo.ConceptoSaldoID.Value);
                            }
                            else
                            {
                                detalle.CuentaID.Value = destino.CuentaDEST.Value;
                                detalle.IdentificadorTR.Value = saldo.IdentificadorTR.Value;
                                if (!cacheCtas.ContainsKey(detalle.CuentaID.Value))
                                {
                                    cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, detalle.CuentaID.Value, true, false);
                                    cacheCtas.Add(detalle.CuentaID.Value, cta.ConceptoSaldoID.Value);
                                }
                            }

                            detalle.ConceptoSaldoID.Value = cacheCtas[detalle.CuentaID.Value];
                            #endregion
                            #region Centro de costo y proyecto

                            //Cto costo
                            if (string.IsNullOrWhiteSpace(destino.CtoCostoDEST.Value))
                                detalle.CentroCostoID.Value = saldo.CentroCostoID.Value;
                            else
                                detalle.CentroCostoID.Value = destino.CtoCostoDEST.Value;

                            //Proyecto
                            if (string.IsNullOrWhiteSpace(destino.ProyectoDEST.Value))
                                detalle.ProyectoID.Value = saldo.ProyectoID.Value;
                            else
                                detalle.ProyectoID.Value = destino.ProyectoDEST.Value;

                            #endregion

                            #region Otros
                            detalle.LugarGeograficoID.Value = lugarGeoDef;
                            detalle.PrefijoCOM.Value = prefijoDef;
                            detalle.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                            detalle.TerceroID.Value = saldo.TerceroID.Value;
                            detalle.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;

                            detalle.DocumentoCOM.Value = AuxiliarDatoAdd4.Reclasificacion.ToString();
                            detalle.TasaCambio.Value = 0;
                            #endregion
                            #region Valores

                            detalle.vlrBaseML.Value = 0;
                            detalle.vlrBaseME.Value = 0;
                            detalle.vlrMdaLoc.Value = saldo.DbOrigenExtML.Value.Value + saldo.DbOrigenLocML.Value.Value + saldo.CrOrigenExtML.Value.Value +
                                saldo.CrOrigenLocML.Value.Value + saldo.DbSaldoIniExtML.Value.Value + saldo.DbSaldoIniLocML.Value.Value +
                                saldo.CrSaldoIniExtML.Value.Value + saldo.CrSaldoIniLocML.Value.Value; ;

                            detalle.vlrMdaExt.Value = saldo.DbOrigenExtME.Value.Value + saldo.DbOrigenLocME.Value.Value + saldo.CrOrigenExtME.Value.Value +
                                saldo.CrOrigenLocME.Value.Value + saldo.DbSaldoIniExtME.Value.Value + saldo.DbSaldoIniLocME.Value.Value +
                                saldo.CrSaldoIniExtME.Value.Value + saldo.CrSaldoIniLocME.Value.Value; ;

                            vlrML = Math.Round(vlrML + detalle.vlrMdaLoc.Value.Value, 2);
                            vlrME = Math.Round(vlrME + detalle.vlrMdaExt.Value.Value, 2);

                            #endregion

                            detalle.DatoAdd3.Value = destino.CuentaORIG.Value + ", " + destino.ProyectoORIG.Value + ", " + destino.CtoCostoORIG.Value;
                            detalle.DatoAdd4.Value = AuxiliarDatoAdd4.Reclasificacion.ToString();

                            footer.Add(detalle);
                            #endregion
                        }

                        #region Carga la contrapartida
                        DTO_ComprobanteFooter contra = new DTO_ComprobanteFooter();

                        #region Cuenta y concepto de saldo
                        if (string.IsNullOrWhiteSpace(origen.CuentaCONT.Value))
                        {
                            contra.CuentaID.Value = saldo.CuentaID.Value;
                            contra.IdentificadorTR.Value = 0;

                            if (!cacheCtas.ContainsKey(contra.CuentaID.Value))
                                cacheCtas.Add(saldo.CuentaID.Value, saldo.ConceptoSaldoID.Value);
                        }
                        else
                        {
                            contra.CuentaID.Value = origen.CuentaCONT.Value;
                            contra.IdentificadorTR.Value = 0;

                            if (!cacheCtas.ContainsKey(contra.CuentaID.Value))
                            {
                                cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, contra.CuentaID.Value, true, false);
                                cacheCtas.Add(contra.CuentaID.Value, cta.ConceptoSaldoID.Value);
                            }
                        }

                        contra.ConceptoSaldoID.Value = cacheCtas[contra.CuentaID.Value];
                        #endregion
                        #region Centro de costo y proyecto

                        //Cto costo
                        if (string.IsNullOrWhiteSpace(origen.CtoCostoCONT.Value))
                            contra.CentroCostoID.Value = saldo.CentroCostoID.Value;
                        else
                            contra.CentroCostoID.Value = origen.CtoCostoCONT.Value;

                        //Proyecto
                        if (string.IsNullOrWhiteSpace(origen.ProyectoCONT.Value))
                            contra.ProyectoID.Value = saldo.ProyectoID.Value;
                        else
                            contra.ProyectoID.Value = origen.ProyectoCONT.Value;

                        #endregion
                        #region Otros
                        contra.LugarGeograficoID.Value = lugarGeoDef;
                        contra.PrefijoCOM.Value = prefijoDef;
                        contra.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                        contra.TerceroID.Value = saldo.TerceroID.Value;
                        contra.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;

                        contra.DocumentoCOM.Value = saldo.IdentificadorTR.Value.Value.ToString();
                        contra.TasaCambio.Value = 0;
                        #endregion
                        #region Valores
                        contra.vlrBaseML.Value = 0;
                        contra.vlrBaseME.Value = 0;
                        contra.vlrMdaLoc.Value = Math.Round(vlrML * -1, 2);
                        contra.vlrMdaExt.Value = Math.Round(vlrME * -1, 2);
                        #endregion

                        contra.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                        footer.Add(contra);
                        #endregion
                    }
                    #endregion
                }

                return footer;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReclasificarComprobante");
                throw ex;
            }
        }

        #endregion

        #region Funciones Públicas

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetDistribucion()
        {
            this._dal_coReclasificaBalance = (DAL_coReclasificaBalance)base.GetInstance(typeof(DAL_coReclasificaBalance), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coReclasificaBalance> results = this._dal_coReclasificaBalance.DAL_coReclasificaBalance_GetAll();

            return results;
        }

        /// <summary>
        /// Obtiene la lista de registros de las exclusiones
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalExcluye> ReclasificacionFiscal_GetExclusiones()
        {
            this._dal_coReclasificaBalExcluye = (DAL_coReclasificaBalExcluye)base.GetInstance(typeof(DAL_coReclasificaBalExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coReclasificaBalExcluye> results = this._dal_coReclasificaBalExcluye.DAL_coReclasificaBalExcluye_GetAll();

            return results;
        }

        /// <summary>
        /// Actualiza la informacion para la reclasificacion de comprobantes
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <param name="tablas">Registros de reclasificacion</param>
        /// <param name="excluyen">Registros de exclucion</param>
        /// <returns></returns>
        public DTO_TxResult ReclasificacionFiscal_Update(int documentID, List<DTO_coReclasificaBalance> tablas, List<DTO_coReclasificaBalExcluye> excluyen, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coReclasificaBalance = (DAL_coReclasificaBalance)this.GetInstance(typeof(DAL_coReclasificaBalance), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coReclasificaBalExcluye = (DAL_coReclasificaBalExcluye)this.GetInstance(typeof(DAL_coReclasificaBalExcluye), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Elimina la info existente
                this._dal_coReclasificaBalance.DAL_coReclasificaBalance_Delete();
                this._dal_coReclasificaBalExcluye.DAL_coReclasificaBalExcluye_Delete();

                //Guarda la info de coReclasificaBalance
                foreach (DTO_coReclasificaBalance tabla in tablas)
                    this._dal_coReclasificaBalance.DAL_coReclasificaBalance_Add(tabla);

                //Guarda la info de coReclasificaBalance
                foreach (DTO_coReclasificaBalExcluye exc in excluyen)
                    this._dal_coReclasificaBalExcluye.DAL_coReclasificaBalExcluye_Add(exc);

                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, Convert.ToInt16(Math.Pow(2, Convert.ToInt32(FormsActions.Edit))), DateTime.Now,
                    this.UserId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReclasificacionFiscal_Update");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                        base._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene la lista de registros de la reclasificacion
        /// </summary>
        /// <returns></returns>
        public List<DTO_coReclasificaBalance> ReclasificacionFiscal_GetForProcess(string tipoBalanceID)
        {
            this._dal_coReclasificaBalance = (DAL_coReclasificaBalance)base.GetInstance(typeof(DAL_coReclasificaBalance), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_coReclasificaBalance> results = this._dal_coReclasificaBalance.DAL_coReclasificaBalance_GetForProcess(tipoBalanceID);

            return results;
        }

        /// <summary>
        /// Procesa la reclasificacion
        /// </summary>
        /// <param name="documentID">Identificador del documento</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ReclasificacionFiscal_Procesar(int documentID, string actividadFlujoID, string tipoBalanceID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = null;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl docCtrl = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Definicion de variables

                string prefix = this.GetPrefijoByDocumento(documentID);

                DTO_Comprobante comp = new DTO_Comprobante();
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                string areaFuncionalID = this._moduloGlobal.GetAreaFuncionalByUser();
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                string comprobanteID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteReclasificacionesBalanceFiscal);
                string tipoBalIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                if (tipoBalanceID == tipoBalIFRS)
                    comprobanteID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteReclasificacionesBalanceIFRS);

                string periodoSTR = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoSTR);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 4;

                #endregion
                #region Valida que no hayan datos en coAuxiliarPre
                int count = this._dal_Comprobante.DAL_ComprobantePre_HasData(true, periodo);
                if (count > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_AuxiliarPreNotClean;

                    return result;
                }
                #endregion
                #region Valida que no se haya ejecutado el proceso para el tipo de balance seleccionado

                List<DTO_glDocumentoControl> ctrls = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(documentID, periodo);
                if (ctrls.Count > 0)
                {
                    foreach (DTO_glDocumentoControl temp in ctrls)
                    {
                        if (temp.ComprobanteID.Value == comprobanteID)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_DocProcessed;

                            return result;
                        }
                    }
                }
                #endregion

                docCtrl = new DTO_glDocumentoControl();
                #region Agregar registro a glDocumentoControl

                //Campos Principales
                docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                docCtrl.DocumentoID.Value = documentID;
                //dtoDC.NumeroDoc.Value IDENTITY
                docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                docCtrl.Fecha.Value = periodo;
                docCtrl.PeriodoDoc.Value = periodo;
                docCtrl.PeriodoUltMov.Value = periodo;
                docCtrl.AreaFuncionalID.Value = areaFuncionalID;
                docCtrl.PrefijoID.Value = prefix;
                docCtrl.DocumentoNro.Value = 0;
                docCtrl.MonedaID.Value = monedaLocal;
                docCtrl.TasaCambioDOCU.Value = 0;
                docCtrl.TasaCambioCONT.Value = 0;
                docCtrl.ComprobanteID.Value = comprobanteID;
                docCtrl.ComprobanteIDNro.Value = 1;
                docCtrl.Observacion.Value = string.Empty;
                docCtrl.Estado.Value = Convert.ToByte(EstadoDocControl.Aprobado);

                rd = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                if (rd.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(rd);

                    return result;
                }

                docCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);
                #endregion

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #region Crea el comprobante de reclasificacion
                #region Carga el cabezote
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.PeriodoID.Value = periodo;
                header.ComprobanteID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteDistribucion);
                header.ComprobanteNro.Value = 1;
                header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                header.MdaTransacc.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                header.MdaOrigen.Value = (Byte)TipoMoneda_LocExt.Local;
                header.TasaCambioBase.Value = 0;
                header.TasaCambioOtr.Value = 0;
                #endregion
                footer = this.ReclasificarComprobante(periodo, tipoBalanceID);

                if (footer.Count == 0)
                    comp = this.CreateEmptyAux(header);
                else
                {
                    comp.Header = header;
                    comp.Footer = footer;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Crea el comprobante de distribucion para el periodo
                comp.Header.LibroID.Value = tipoBalanceID;
                result = this.ContabilizarComprobante(documentID, comp, periodo, ModulesPrefix.co, 0, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                porcTotal = 80;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion

                // Actualiza la tabla de consecutivos de comprobantes
                this._dal_Comprobante.DAL_Comprobante_AgregarConsecutivoMigracion(comprobanteID, periodo, 1);

                porcTotal = 100;
                batchProgress[tupProgress] = (int)porcTotal;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReclasificacionFiscal_Procesar");

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

                        docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, false, false);
                    }
                    else
                        throw new Exception("ReclasificacionFiscal_Procesar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #endregion

        #region Saldos

        #region Funciones Privadas

        /// <summary>
        /// Genera una lista con los saldos que se deben agregar
        /// </summary>
        /// <param name="bitacoraOrigen">Identificador de la bitacora que inicio el proceso</param>
        /// <param name="documentoID">Documento que origina la transaccion</param>
        /// <param name="txInventarios">Identificador si la transaccion inicia desde un proceso de inventarios</param>
        /// <param name="mod">Modulo que origina la transaccion</param>
        /// <param name="comprobante">Comprobante que va actualizar los saldos</param>
        /// <param name="tipBal">Tipo de balance</param>
        /// <param name="userId">Usuario que esta generando los saldos</param>
        /// <param name="isPre">Define si esta generando saldos con un tipo de balance preliminar</param>
        /// <returns>Retorna una lista </returns>
        private DTO_TxResult GenerarSaldos(int documentoID, ModulesPrefix mod, DTO_Comprobante comprobante, int bitacoraOrigen)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)base.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string libroID = this.GetLibroComprobante(comprobante.Header);
                result = this._dal_Contabilidad.DAL_Contabilidad_GenerarSaldos(documentoID, mod, comprobante, libroID);

                if (result.Result == ResultValue.NOK)
                {
                    int compNro = comprobante.Header.ComprobanteNro.Value != null && comprobante.Header.ComprobanteNro.Value.HasValue
                        ? comprobante.Header.ComprobanteNro.Value.Value : 0;

                    result.ResultMessage = DictionaryMessages.Err_Co_GenerarSaldos + "&&" + comprobante.Header.ComprobanteID.Value + "&&" +
                        compNro + "&&" + comprobante.Header.PeriodoID.Value.Value.ToString(FormatString.Period);
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GenerarSaldos");
                return result;
            }
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="idTR">Llave de busqueda</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>true si existe</returns>
        internal bool Saldos_ExistsByIdentificadorTR(long idTR, DateTime periodo, string libroID)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_SaldoExistsByIdentificadorTR(idTR, periodo, libroID);
        }

        /// <summary>
        /// Trae los saldos de un periodo dada la cuenta
        /// </summary>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retornala lista de saldos</returns>
        internal List<DTO_coCuentaSaldo> Saldos_GetSaldosByPeriodoCuenta(DateTime PeriodoID, string cuentaID, string libroID, bool fromMaster = false)
        {
            if (string.IsNullOrWhiteSpace(libroID))
                libroID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_GetSaldosByPeriodoCuenta(PeriodoID, cuentaID, fromMaster, libroID);
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="idTR">Llave de busqueda</param>
        /// <param name="periodo">Periodo de consulta</param>
        /// <returns>true si existe</returns>
        internal bool Saldos_CreditoHasSaldo(DateTime periodo, int idTR)
        {
            try
            {
                //Información componente Capital
                string compCapital = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                DTO_ccCarteraComponente compCapitalDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, compCapital, true, false);
                string concCapital = compCapitalDTO.ConceptoSaldoID.Value;

                //Información componente Seguro
                string compSeguro = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                DTO_ccCarteraComponente compSeguroDTO = (DTO_ccCarteraComponente)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, compSeguro, true, false);
                string concSeguro = compSeguroDTO.ConceptoSaldoID.Value;

                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                return this._dal_Contabilidad.DAL_Contabilidad_CreditoHasSaldo(periodo, concCapital, concSeguro, idTR, libroFunc);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Saldos_CreditoHasSaldo");
                throw ex;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldos de presupuesto
        /// </summary>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        ///  <param name="proyectoID">Identificador de la Proyecto</param>
        ///  <param name="centroCtoID">Identificador de la Centro Costo</param>
        ///  <param name="lineaPresID">Identificador de la lineaPres</param>
        /// <returns>Retorna lista de Saldos</returns>
        public List<DTO_coCuentaSaldo> Saldos_GetForPresupuesto(DateTime PeriodoID, string cuentaID, string proyectoID, string centroCtoID, string lineaPresID)
        {
            string libroID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LibroPresupuesto);
            List<DTO_coCuentaSaldo> cuentaSaldos = new List<DTO_coCuentaSaldo>();
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            cuentaSaldos = this._dal_Contabilidad.DAL_Contabilidad_GetSaldosForPresupuesto(PeriodoID, cuentaID, libroID, proyectoID, centroCtoID, lineaPresID);
            return cuentaSaldos;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un saldo</returns>
        public DTO_coCuentaSaldo Saldo_GetByDocumento(string cuentaID, string concSaldo, long identificadorTR, string balanceTipo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(balanceTipo))
                    balanceTipo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, concSaldo, true, false);
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), cSaldo.ModuloID.Value.ToLower());

                string periodoStr = this.GetControlValueByCompany(mod, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);

                this._dal_Contabilidad = (DAL_Contabilidad)base.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_Contabilidad.DAL_Contabilidad_GetSaldoByDocumento(periodo, cuentaID, concSaldo, identificadorTR, balanceTipo);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloContabilidad_Saldos_GetByDocumento");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si ya hay uno o más conceptos de saldo en coCuentaSaldo
        /// </summary>
        /// <param name="conceptoSaldoID">Id de concepto saldo</param>
        /// <returns>true si existe</returns>
        public bool Saldo_ExistsByCtaConcSaldo(string ConceptoSaldoIDNew, string conceptoSaldoIDOld, string cuentaID)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            try
            {
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                if (string.IsNullOrEmpty(cuentaID))
                    return this._dal_Contabilidad.DAL_Contabilidad_SaldoExistsByCtaConcSaldo(conceptoSaldoIDOld, cuentaID, libroFunc);

                List<DTO_coCuentaSaldo> listSaldos = new List<DTO_coCuentaSaldo>();
                DateTime periodoConsulta;
                string periodoNew = string.Empty;
                string periodoOld = string.Empty;
                DTO_glConceptoSaldo cptosaldoNew = (DTO_glConceptoSaldo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ConceptoSaldoIDNew, true, false);
                DTO_glConceptoSaldo cptosaldoOld = (DTO_glConceptoSaldo)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, conceptoSaldoIDOld, true, false);
                #region Consulta los periodos segun el modulo
                switch (cptosaldoNew.ModuloID.Value.ToString())
                {
                    case "AC":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);
                        break;
                    case "CC":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                        break;
                    case "CO":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                        break;
                    case "CP":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo);
                        break;
                    case "DI":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.di, AppControl.di_Periodo);
                        break;
                    case "FA":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo);
                        break;
                    case "IN":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo);
                        break;
                    case "NO":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
                        break;
                    case "OC":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.oc, AppControl.oc_Periodo);
                        break;
                    case "PL":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_Periodo);
                        break;
                    case "PY":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.py, AppControl.py_Periodo);
                        break;
                    case "PR":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo);
                        break;
                    case "RH":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.rh, AppControl.rh_Periodo);
                        break;
                    case "TS":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo);
                        break;
                }
                switch (cptosaldoOld.ModuloID.Value.ToString())
                {
                    case "AC":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);
                        break;
                    case "CC":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                        break;
                    case "CO":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                        break;
                    case "CP":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo);
                        break;
                    case "DI":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.di, AppControl.di_Periodo);
                        break;
                    case "FA":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo);
                        break;
                    //case "IM":
                    //    periodoOld = GetControlValueByCompany(ModulesPrefix.im, AppControl.im_Periodo);
                    case "IN":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo);
                        break;
                    case "NO":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
                        break;
                    case "OC":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.oc, AppControl.oc_Periodo);
                        break;
                    case "PL":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_Periodo);
                        break;
                    case "PR":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo);
                        break;
                    case "PY":
                        periodoNew = GetControlValueByCompany(ModulesPrefix.py, AppControl.py_Periodo);
                        break;
                    case "RH":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.rh, AppControl.rh_Periodo);
                        break;
                    case "TS":
                        periodoOld = GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo);
                        break;
                }
                #endregion
                //Saca la fecha minima de los dos periodos
                if (Convert.ToDateTime(periodoNew) < Convert.ToDateTime(periodoOld))
                    periodoConsulta = Convert.ToDateTime(periodoNew);
                else
                    periodoConsulta = Convert.ToDateTime(periodoOld);

                listSaldos = this.Saldos_GetSaldosByPeriodoCuenta(periodoConsulta, cuentaID, libroFunc, true);
                if (listSaldos.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloContabilidad_Saldos_Exists");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <param name="excludeIdTR">Lista de identificadores que se deben excluir </param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByPeriodoCuenta(bool isML, DateTime PeriodoID, string cuentaID, string libroID, List<int> excludeIdTRs = null)
        {
            if (string.IsNullOrWhiteSpace(libroID))
                libroID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            decimal result = this._dal_Contabilidad.DAL_Contabilidad_GetSaldoByPeriodoCuenta(isML, PeriodoID, cuentaID, libroID, excludeIdTRs);
            return Math.Round(result, 2);
        }

        /// <summary>
        /// Trae la informacion de los saldos en ML o ME
        /// </summary>
        /// <param name="isML">Indica si se deben consultar en ML</param>
        /// <param name="PeriodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta</param>
        /// <returns>Retorna el valor del saldo</returns>
        public decimal Saldo_GetByDocumentoCuenta(bool isML, DateTime PeriodoID, long identificadorTR, string cuentaID, string libroID)
        {
            if (string.IsNullOrWhiteSpace(libroID))
                libroID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_GetSaldoByDocumentoCuenta(isML, PeriodoID, identificadorTR, cuentaID, libroID);
        }

        /// <summary>
        /// Revisa si un documento ha tenido movimientos de saldos despues de su creación
        /// </summary>
        /// <param name="idTR">Identificador del documento</param>
        /// <param name="periodoID">Periodo de consulta</param>
        /// <param name="cuentaID">Identificador de la cuenta del documento</param>
        /// <param name="libroID">Libro de consulta</param>
        /// <returns>Retorna true si ha tenido nuevos movimientos, de lo contrario false</returns>
        public bool Saldo_DocumentoConMovimiento(int idTR, DateTime periodoID, DTO_coPlanCuenta cta, string libroID)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_HasMovimiento(idTR, periodoID, cta, libroID);
        }

        /// <summary>
        /// Trae la informacion de los saldosn de acuerdo a los filtros de la vista de resumen
        /// </summary>
        /// <param name="PeriodoID">Periodo</param>
        /// <param name="libroID">Tipo Balance</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="centroCostoID">Centro Costo</param>
        /// <param name="lineaPresupuestoID">Linea Presupuesto</param>
        /// <param name="conceptoSaldoID">Concepto Saldo</param>
        /// <param name="conceptoCargoID">Concept Cargo</param>
        /// <param name="identificadorTr">Identificador Tr</param>
        /// <returns></returns>
        public List<DTO_SaldosVista> Saldo_GetSaldosResumen(DateTime PeriodoID, string libroID, string cuentaID, string terceroID, string proyectoID,
                                                         string centroCostoID, string lineaPresupuestoID, string conceptoSaldoID, string conceptoCargoID,
                                                         int? identificadorTr)
        {
            this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Contabilidad.DAL_Contabilidad_GetSaldosResumen(PeriodoID, libroID, cuentaID, terceroID, proyectoID, centroCostoID, lineaPresupuestoID, conceptoSaldoID, conceptoCargoID, identificadorTr);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> Saldos_GetByParameter(DTO_coCuentaSaldo filter)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)base.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_Contabilidad.DAL_Contabilidad_GetByParameter(filter);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Saldos_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">filtro</param>
        /// <returns>Lista de saldos </returns>
        public List<DTO_coCuentaSaldo> Saldos_GetByTerceroCartera(DateTime periodoID, string terceroID, string cuentaCapital, string libroID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(libroID))
                    libroID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                this._dal_Contabilidad = (DAL_Contabilidad)base.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_Contabilidad.DAL_Contabilidad_GetByTerceroCartera(periodoID, terceroID, cuentaCapital, libroID);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Saldos_GetByParameter");
                throw exception;
            }
        }


        /// <summary>
        /// Trae los saldos de contabilidad de acuerdo a los filtros y al tipo de reporte
        /// </summary>
        /// <param name="reporteID">Reporte</param>
        /// <returns></returns>
        public List<DTO_coReporteLinea> Saldo_GetSaldosByLineaReporte(string reporteID,DateTime? fechaInicial, DateTime? fechaFinal, string terceroID, string proyectoID,string centroCtoID, string linePresup)
        {
            try
            {
                this._dal_Contabilidad = (DAL_Contabilidad)this.GetInstance(typeof(DAL_Contabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DAL_MasterComplex _dal_MasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                string balance = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                DTO_glConsulta filtro = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                //Filtro
                DTO_glConsultaFiltro campo = new DTO_glConsultaFiltro();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ReporteID",
                    ValorFiltro = reporteID,
                    OperadorFiltro = OperadorFiltro.Igual
                });
                filtro.Filtros = filtros;

                //Trae las lineas de reporte 
                _dal_MasterComplex.DocumentID = AppMasters.coReporteLinea;
                long count = _dal_MasterComplex.DAL_MasterComplex_Count(filtro, true);
                List<DTO_coReporteLinea> repLineas = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, filtro, true).Cast<DTO_coReporteLinea>().ToList();
                //Trae los filtros de reporte
                _dal_MasterComplex.DocumentID = AppMasters.coReporteFiltro;
                count = _dal_MasterComplex.DAL_MasterComplex_Count(filtro, true);
                List<DTO_coReporteFiltro> repLineasFiltro = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, filtro, true).Cast<DTO_coReporteFiltro>().ToList();

                if (!fechaInicial.HasValue)
                    fechaInicial = periodo;
                if (!fechaFinal.HasValue)
                    fechaFinal = periodo;

                //Recorre los datos para obtener saldos y totales
                foreach (DTO_coReporteLinea lin in repLineas)
                {
                    foreach (DTO_coReporteFiltro linFiltro in repLineasFiltro.FindAll(x => x.ReporteID.Value == lin.ReporteID.Value && x.RepLineaID.Value == lin.RepLineaID.Value))
                    {
                        //Asigna filtros dando prioridad a la maestra
                        linFiltro.TerceroID.Value = string.IsNullOrEmpty(linFiltro.TerceroID.Value) ? terceroID : linFiltro.TerceroID.Value;
                        linFiltro.CentroCosto.Value = string.IsNullOrEmpty(linFiltro.CentroCosto.Value) ? centroCtoID : linFiltro.CentroCosto.Value;
                        linFiltro.Proyecto.Value = string.IsNullOrEmpty(linFiltro.Proyecto.Value) ? proyectoID : linFiltro.Proyecto.Value;
                        linFiltro.LineaPre.Value = string.IsNullOrEmpty(linFiltro.LineaPre.Value) ? linePresup : linFiltro.LineaPre.Value;
                        lin.SaldosMLRepLinea.AddRange(this._dal_Contabilidad.DAL_Contabilidad_GetSaldosByLineaRep(fechaInicial.Value, fechaFinal.Value, balance, linFiltro.Cuenta.Value, linFiltro.TerceroID.Value, linFiltro.Proyecto.Value, linFiltro.CentroCosto.Value, linFiltro.LineaPre.Value, string.Empty, linFiltro.ConceptoCargo.Value, null));
                    }
                    lin.VlrTotalMLRepLinea.Value = lin.SaldosMLRepLinea.Sum(x => x.SaldoIniML.Value + x.DebitoML.Value + x.CreditoML.Value);
                }

                try { repLineas = repLineas.OrderBy(x => Convert.ToInt32(x.RepLineaID.Value)).ToList();}   catch (Exception) {;}
                for (int i = 0; i < repLineas.Count; i++)
                {
                    if (i != 0)
                    {
                        if(repLineas[i].DescripcionAgrupa.Value == repLineas[i-1].DescripcionAgrupa.Value)
                            repLineas[i].OrdenAgrupa.Value = repLineas[i - 1].OrdenAgrupa.Value;
                        else
                            repLineas[i].OrdenAgrupa.Value = i;
                    }
                    else
                        repLineas[i].OrdenAgrupa.Value = i;

                }

            return repLineas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Saldo_GetSaldosByLineaReporte");
                throw exception;
            }
        }



        #endregion

        #endregion

        #region Reportes

        #region Documentos

        #region Documento Contable

        /// <summary>
        /// Funcion que carga la lista de la fatura a causar
        /// </summary>
        /// <param name="numDoc">Identificador del con q se guardan los registro en la BD</param>
        /// <param name="isAprovada">Obtiene la informacion (true = Aprobada, Trae la info de coAuxilar; False = ParaAprobacion, Trae la info de coAuxilarPre</param>
        /// <returns>Listado de Factura a causar</returns>
        public List<DTO_ReportCxPTotales> ReportesContabilidad_DocumentoContable(int numDoc, bool isAprovada)
        {
            try
            {
                #region Variables

                List<DTO_ReportCxPTotales> result = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales docContable = new DTO_ReportCxPTotales();
                docContable.DetalleCausacionFactura = new List<DTO_ReportCausacionFacturas>();
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion

                docContable.DetalleCausacionFactura = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_DocumentoContable(numDoc, isAprovada);
                List<string> distinct = (from c in docContable.DetalleCausacionFactura select c.TerceroID.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportCxPTotales causacionFact = new DTO_ReportCxPTotales();
                    causacionFact.DetalleCausacionFactura = new List<DTO_ReportCausacionFacturas>();

                    causacionFact.DetalleCausacionFactura = docContable.DetalleCausacionFactura.Where(x => x.TerceroID.Value == item).ToList();
                    result.Add(causacionFact);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_DocumentoContable");
                return null;
            }
        }

        #endregion

        #region Comprobante Manual

        /// <summary>
        /// Funcion q se encarga de traer los datos para el comprobante manual
        /// </summary>
        /// <param name="numeroDoc">Numero Doc de identificacion</param>
        /// <param name="isAprovada">Verifica si es aprobada (True: Trae los Datos de la Tabla coAuxiliar, False: Trae los datos de la tabla coAuxiliarPre) </param>
        /// <param name="moneda">Verifica la moneda que se esta trabajando (True:Local, False: Extranjera) </param>
        /// <returns></returns>
        public List<DTO_ContabilidadTotal> ReportesContabilidad_ComprobanteManual(int numeroDoc, bool isAprovada, bool moneda)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ContabilidadTotal> result = new List<DTO_ContabilidadTotal>();
                DTO_ContabilidadTotal compro = new DTO_ContabilidadTotal();
                compro.DetallesComprobanteManual = new List<DTO_ReportComprobanteManual>();
                Dictionary<string, int> detalles = new Dictionary<string, int>();

                compro.DetallesComprobanteManual = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_ComprobanteManual(numeroDoc, isAprovada, moneda);

                foreach (var comprobante in compro.DetallesComprobanteManual)
                {
                    if (!detalles.Contains(new KeyValuePair<string, int>(comprobante.ComprobanteID.Value + "-" + comprobante.ComprobanteNro.ToString(), Convert.ToInt32(comprobante.ComprobanteNro.Value))))
                        detalles.Add(comprobante.ComprobanteID.Value + "-" + comprobante.ComprobanteNro.ToString(), Convert.ToInt32(comprobante.ComprobanteNro.Value));
                }

                foreach (var comprobantes in detalles)
                {
                    DTO_ContabilidadTotal comproTotal = new DTO_ContabilidadTotal();
                    comproTotal.DetallesComprobanteManual = new List<DTO_ReportComprobanteManual>();

                    string comprobanteID = comprobantes.Key.Split('-')[0].ToString(); ;
                    int comprobanteNro = Convert.ToInt32(comprobantes.Key.Split('-')[1]);
                    comproTotal.DetallesComprobanteManual = compro.DetallesComprobanteManual.Where(x => x.ComprobanteID.Value == comprobanteID && x.ComprobanteNro.Value == comprobanteNro).ToList();

                    result.Add(comproTotal);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_ComprobanteManual");
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Reportes PDF

        #region Auxiliar
        /// <summary>
        /// Funcion que genera el reportes desde el cliente
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial de la consulta</param>
        /// <param name="fechaFinal">Fecha Final de la consulta</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="cuentaFin">Tipo de  cuenta hasta donde se desea ver </param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <returns></returns>
        public List<DTO_ReportLibroDiarioTotales> ReportesContabilidad_Auxiliar(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                DTO_ReportLibroDiarioTotales aux = new DTO_ReportLibroDiarioTotales();
                List<DTO_ReportLibroDiarioTotales> listAux = new List<DTO_ReportLibroDiarioTotales>();

                aux.Detalles = new List<DTO_ReportLibroDiario>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                aux.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_AuxiliarCuentaFuncinal(fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal);
                List<string> distinct = (from c in aux.Detalles select c.CuentaID.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportLibroDiarioTotales auxTotal = new DTO_ReportLibroDiarioTotales();
                    auxTotal.Detalles = new List<DTO_ReportLibroDiario>();

                    auxTotal.Detalles = aux.Detalles.Where(x => x.CuentaID.Value == item).ToList();
                    listAux.Add(auxTotal);
                }
                return listAux;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_Auxiliar");
                throw ex;
            }

        }

        /// <summary>
        /// Funcion que genera el reportes desde el cliente
        /// </summary>
        /// <param name="año">Año que se va a mostrar</param>
        /// <param name="mesInicial">Mes por el cual se va a filtrar</param>
        /// <param name="mesFin">Mes por el cual se va a filtrar</param>
        /// <param name="libro">Tipo de libro a consultar</param>
        /// <param name="cuentaInicial">Tipo de cuenta que se desea ver</param>
        /// <param name="tercero">Tipo tercero que se desea ver</param>
        /// <param name="proyecto">Tipo proyecto que se desea ver</param>
        /// <param name="centroCosto">Tipo centro Costo que se desea ver</param>
        /// <param name="lineaPresupuestal">Tipo Linea presupuestal que se desea ver</param>
        /// <returns></returns>
        public List<DTO_ReportLibroDiarioTotales> ReportesContabilidad_AuxiliarxTercero(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                DTO_ReportLibroDiarioTotales aux = new DTO_ReportLibroDiarioTotales();
                List<DTO_ReportLibroDiarioTotales> listAux = new List<DTO_ReportLibroDiarioTotales>();
                List<DTO_ReportLibroDiarioTotales> PorCuenta = new List<DTO_ReportLibroDiarioTotales>();
                Dictionary<string, string> detalle = new Dictionary<string, string>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                aux.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_AuxiliarCuentaFuncinalxTercero( fechaInicial,  fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal);
                List<string> distinct = (from c in aux.Detalles select c.CuentaID.Value).Distinct().ToList();
                //List<string> distinct = (from c in aux.Detalles select c.TerceroID.Value).Distinct().ToList();

                #region Agrupamiento por cuenta

                foreach (var item in distinct)
                {
                    DTO_ReportLibroDiarioTotales porCuenta = new DTO_ReportLibroDiarioTotales();
                    List<DTO_ReportLibroDiarioTotales> totalCuenta = new List<DTO_ReportLibroDiarioTotales>();
                    porCuenta.Detalles = new List<DTO_ReportLibroDiario>();

                    porCuenta.Detalles = aux.Detalles.Where(x => x.CuentaID.Value == item).ToList();
                    porCuenta.Detalles.GroupBy(x => x.TerceroID.Value);

                    listAux.Add(porCuenta);
                }
                #endregion

                return listAux;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_AuxiliarxTercero");
                throw ex;
            }
        }

        /// <summary>
        /// Carga el DTO para generar el excel con los comprobante
        /// </summary>
        /// <param name="año">Año que se desea ver los comprobantes</param>
        /// <param name="mes">Mes Q se desea ver</param>
        /// <param name="comprobanteID">Filtra los comprobantes q se desean ver</param>
        /// <param name="libro">Libro con el cual se va filtrar</param>
        /// <param name="comprobanteInicial">Numero comprobante Inicial (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <param name="comprobanteFinal">Numero comprobante Final (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <returns></returns>
        public string ReportesContabilidad_PlantillaExcelAuxiliar(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial,
            string cuentaFin, string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportLibroDiario> plantillaExcel = new List<DTO_ReportLibroDiario>();
                string fileName;
                string str = this.GetCvsName(ExportType.Csv, out fileName);

                plantillaExcel = new List<DTO_ReportLibroDiario>();
                plantillaExcel = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_AuxiliarCuentaFuncinal(fechaInicial, fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                    proyecto, centroCosto, lineaPresupuestal);
                if (plantillaExcel.Count > 0)
                {
                    CsvExport<DTO_ReportLibroDiario> csv = new CsvExport<DTO_ReportLibroDiario>(plantillaExcel);
                    csv.ExportToFile(str, ExportType.Csv, true, string.Empty);
                }
                else
                    fileName = string.Empty;

                return fileName;

            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_LibroDiario");
                throw ex;
            }
        }

        #endregion

        #region Balance

        /// <summary>
        /// Funcion que Genera el reporte desde el cliente
        /// </summary>
        /// <param name="libroFUNC">Libro FUNCIONAL (Este es fijo)</param>
        /// <param name="libroAux">Parametro de entrada (Libro contra el cual se va a comparar el libro FUNC)</param>
        /// <param name="fechaFin">Parametro para ver el mes inicial que se desea imprimir</param>
        /// <param name="fechaIni">Paramtro del mes hasta donde se desea que se imprima el reporte</param>
        /// <returns>Lista de Objetos</returns>
        public List<DTO_ContabilidadTotal> ReportesContabilidad_BalanceComparativo(string libroAux, DateTime fechaFin, DateTime fechaIni, int año)
        {
            List<DTO_ContabilidadTotal> balance = new List<DTO_ContabilidadTotal>();
            DTO_ContabilidadTotal balanceComparativo = new DTO_ContabilidadTotal();
            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            string libroFUNC = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            balanceComparativo.DetallesBalancesComparativo = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_BalanceComparativo(libroFUNC, libroAux, fechaFin, fechaIni, año);
            balance.Add(balanceComparativo);

            return balance;
        }

        #endregion

        #region Certificados

        /// <summary>
        /// Funcion que se encarga de Generar el detalle del reporte
        /// </summary>
        /// <param name="Periodo">Perdio de consulta</param>
        /// <param name="Impuesto">Tipo de impuesto a generar el certificado de retencion</param>
        /// <returns>Listado de Detalles</returns>
        public List<DTO_ContabilidadTotal> ReportesContabilidad_CertificadoReteFuente(DateTime Periodo, string Impuesto)
        {
            try
            {
                List<DTO_ContabilidadTotal> result = new List<DTO_ContabilidadTotal>();
                DTO_ContabilidadTotal certificado = new DTO_ContabilidadTotal();
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //certificado.DetalleCertificadoReteFuente = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_CertificadoRetencion(Periodo, Impuesto);
                //result.Add(certificado);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_CertificadoReteFuente");
                throw ex;
            }
        }

        #endregion

        #region Comprobates

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Año que se va a mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtar el reporte</param>
        /// <returns></returns>
        public List<DTO_ReportComprobanteTotal> ReportesContabilidad_Comprobantes(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            DTO_ReportComprobanteTotal compro = new DTO_ReportComprobanteTotal();
            List<DTO_ReportComprobanteTotal> comprobante = new List<DTO_ReportComprobanteTotal>();
            Dictionary<string, int> detalles = new Dictionary<string, int>();

            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            compro.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_Comprobante(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
            compro.Detalles = compro.Detalles.OrderBy(x => x.ComprobanteID.Value).ThenBy(x => x.ComprobanteNro).ToList();

            foreach (var item in compro.Detalles)
            {
                if (!detalles.Contains(new KeyValuePair<string, int>(item.ComprobanteID.Value + "-" + item.ComprobanteNro.ToString(), item.ComprobanteNro)))
                    detalles.Add(item.ComprobanteID.Value + "-" + item.ComprobanteNro.ToString(), item.ComprobanteNro);
            }

            foreach (var item in detalles)
            {
                try
                {
                    DTO_ReportComprobanteTotal comproTotal = new DTO_ReportComprobanteTotal();
                    comproTotal.Detalles = new List<DTO_ReportComprobante>();

                    string comprobatID;
                    int comprobateNro;

                    comprobatID = item.Key.Split('-')[0].ToString();
                    comprobateNro = Convert.ToInt32(item.Key.Split('-')[1].ToString());
                    comproTotal.Detalles = compro.Detalles.Where(x => x.ComprobanteID.Value == comprobatID && x.ComprobanteNro == comprobateNro).ToList();

                    comprobante.Add(comproTotal);
                }
                catch (Exception ex)
                {
                    Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_Comprobantes");
                    throw ex;
                }
            }

            return comprobante;

        }

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Año que se va a mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtar el reporte</param>
        /// <returns></returns>
        public List<DTO_ReportComprobanteTotal> ReportesContabilidad_ComprobantesPreliminar(int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            DTO_ReportComprobanteTotal compro = new DTO_ReportComprobanteTotal();
            List<DTO_ReportComprobanteTotal> comprobante = new List<DTO_ReportComprobanteTotal>();
            Dictionary<string, int> detalles = new Dictionary<string, int>();

            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            compro.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_ComprobantePreliminar(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);

            foreach (var item in compro.Detalles)
            {
                if (!detalles.Contains(new KeyValuePair<string, int>(item.ComprobanteID.Value + "-" + item.ComprobanteNro.ToString(), item.ComprobanteNro)))
                    detalles.Add(item.ComprobanteID.Value + "-" + item.ComprobanteNro.ToString(), item.ComprobanteNro);
            }

            foreach (var item in detalles)
            {
                try
                {
                    DTO_ReportComprobanteTotal comproTotal = new DTO_ReportComprobanteTotal();
                    comproTotal.Detalles = new List<DTO_ReportComprobante>();

                    string comprobantID;
                    int comprobateNro;

                    comprobantID = item.Key.Split('-')[0].ToString();
                    comprobateNro = Convert.ToInt32(item.Key.Split('-')[1].ToString());
                    comproTotal.Detalles = compro.Detalles.Where(x => x.ComprobanteID.Value == comprobantID && x.ComprobanteNro == comprobateNro).ToList();

                    comprobante.Add(comproTotal);
                }
                catch (Exception ex)
                {
                    Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_ComprobantesPreliminar");
                    throw ex;
                }
            }

            return comprobante;

        }

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Año que se va a mostrar en el reporte</param>
        /// <param name="mes">Mes por el cual se va filtar el reporte</param>
        /// <returns></returns>
        public List<DTO_ReportComprobanteControlTotal> ReportesContabilidad_ComprobateControl(int año, int mes, string comprobanteID)
        {
            try
            {
                DTO_ReportComprobanteControlTotal control = new DTO_ReportComprobanteControlTotal();
                List<DTO_ReportComprobanteControlTotal> comproControl = new List<DTO_ReportComprobanteControlTotal>();
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                control.Detalles = _dal_ReportContabiliadad.DAL_ReportesContabilidad_ComprobanteControl(año, mes);
                int min;
                int max;
                List<string> distinct = (from c in control.Detalles select c.ComprobanteDesc.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportComprobanteControlTotal controlTotal = new DTO_ReportComprobanteControlTotal();
                    controlTotal.Detalles = new List<DTO_ReportComprobanteControl>();

                    controlTotal.Detalles = control.Detalles.Where(x => x.ComprobanteDesc.Value == item).ToList();
                    min = controlTotal.Detalles.Min(x => x.ComprobanteNro);
                    max = controlTotal.Detalles.Max(x => x.ComprobanteNro);
                    controlTotal.min = min;
                    controlTotal.max = max;
                    comproControl.Add(controlTotal);
                }
                return comproControl;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_ComprobateControl");
                throw ex;
            }
        }


        #endregion

        #region Inventario Balance

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="mes">Parametro para filtar por el mes deseado</param>
        /// <returns></returns>
        public List<DTO_ContabilidadTotal> ReportesContabilidad_InventarioBalance(int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin, int _año)
        {
            DTO_ContabilidadTotal inventarios = new DTO_ContabilidadTotal();
            List<DTO_ContabilidadTotal> balance = new List<DTO_ContabilidadTotal>();
            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_glTabla table = this._moduloGlobal.glTabla_GetByTablaNombre("coPlanCuenta", this.Empresa.ID.Value);
            int cuentaLength = table.CodeLength(table.LevelsUsed());

            //Trae los datos
            inventarios.DetallesInventarioBalance = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_InventariosBalance(mesIni, mesFin, Libro, cuentaIni, cuentaFin, _año);
            List<string> distinct = (from c in inventarios.DetallesInventarioBalance select c.cuentaID.Value).Distinct().ToList();
            
            foreach (string item in distinct)
            {
                try
                {
                    DTO_ContabilidadTotal balanTipo = new DTO_ContabilidadTotal();
                    balanTipo.DetallesInventarioBalance = new List<DTO_ReportInventariosBalance>();
                    balanTipo.DetallesInventarioBalance = inventarios.DetallesInventarioBalance.Where(x => x.cuentaID.Value == item).ToList();
                    if (item.Length == cuentaLength)
                    {
                        foreach (DTO_ReportInventariosBalance ctaLastLevel in balanTipo.DetallesInventarioBalance)
                        {
                            ctaLastLevel.InicialML_Cuenta.Value = balanTipo.DetallesInventarioBalance.Sum(x => x.InicialML_Tercero.Value);
                            ctaLastLevel.DebitoML_Cuenta.Value = balanTipo.DetallesInventarioBalance.Sum(x => x.DebitoML_Tercero.Value);
                            ctaLastLevel.CreditoML_Cuenta.Value = balanTipo.DetallesInventarioBalance.Sum(x => x.CreditoML_Tercero.Value);
                            ctaLastLevel.FinalML_Cuenta.Value = balanTipo.DetallesInventarioBalance.Sum(x => x.FinalML_Tercero.Value);
                        }
                    }
                    else
                    {
                        foreach (DTO_ReportInventariosBalance ctaLastLevel in balanTipo.DetallesInventarioBalance)
                            ctaLastLevel.FinalML_Cuenta.Value = ctaLastLevel.InicialML_Cuenta.Value + ctaLastLevel.DebitoML_Cuenta.Value - ctaLastLevel.CreditoML_Cuenta.Value;
                    }
                    balance.Add(balanTipo);
                }
                catch (Exception ex)
                {
                    Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_InventarioBalance");
                    throw ex;
                }
            }
            return balance.Distinct().ToList();
        }

        /// <summary>
        /// Funcion que se encarga de generar la logica para la plantilla de exce
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro a consultar</param>
        /// <param name="cuentaIni">Filtro rango d cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango d cuentas, Cuenta Final</param>
        /// <returns>URL con excel</returns>
        public string ReportesContabilidad_PlantillaExcelInventarioBalance(int mes, string Libro, string cuentaIni, string cuentaFin, int _año)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportInventariosBalance> plantillaExcel = new List<DTO_ReportInventariosBalance>();
                string fileName;
                string str = this.GetCvsName(ExportType.Csv, out fileName);

                plantillaExcel = new List<DTO_ReportInventariosBalance>();
                plantillaExcel = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_InventariosBalance(mes,mes, Libro, cuentaIni, cuentaFin, _año);

                if (plantillaExcel.Count > 0)
                {
                    CsvExport<DTO_ReportInventariosBalance> csv = new CsvExport<DTO_ReportInventariosBalance>(plantillaExcel);
                    csv.ExportToFile(str, ExportType.Csv, true, string.Empty);
                }
                else
                    fileName = string.Empty;

                return fileName;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Libros

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Parametro para filtar por el año deseado</param>
        /// <param name="mes">Parametro para filtar por el mes deseado</param>
        /// <param name="tipoBalance">Parametro para filtar por el tipo Balance deseado</param>
        /// <returns></returns>
        public List<DTO_ReportLibroDiarioTotales> ReportesContabilidad_LibroDiario(int año, int mes, string tipoBalance)
        {
            try
            {
                DTO_ReportLibroDiarioTotales aux = new DTO_ReportLibroDiarioTotales();
                List<DTO_ReportLibroDiarioTotales> listAux = new List<DTO_ReportLibroDiarioTotales>();

                aux.Detalles = new List<DTO_ReportLibroDiario>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                aux.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_LibroDiario(año, mes, tipoBalance);
                List<string> distinct = (from c in aux.Detalles select c.CuentaID.Value).Distinct().ToList();

                #region Agrupamiento por comprobante NO BORRAR
                //List<string> distinct = (from c in aux.Detalles select c.ComprobanteDesc.Value).Distinct().ToList();

                //foreach (string item in distinct)
                //{
                //    List<DTO_ReportLibroDiario> temp = new List<DTO_ReportLibroDiario>();
                //    DTO_ReportLibroDiarioTotales auxTotal = new DTO_ReportLibroDiarioTotales();

                //    foreach (var item1 in aux.Detalles.Where(x => x.ComprobanteDesc.Value == item))
                //    {
                //        if (!temp.Any(x => x.ComprobanteID.Value == item1.ComprobanteID.Value))
                //        {
                //            temp.Add(item1);
                //        }
                //        else
                //        {
                //            DTO_ReportLibroDiario dto = new DTO_ReportLibroDiario();
                //            dto = temp.Where(x => x.ComprobanteID.Value == item1.ComprobanteID.Value).FirstOrDefault();
                //            dto.DebitoML.Value += item1.DebitoML.Value;
                //            dto.CreditoML.Value += item1.CreditoML.Value;

                //            temp.Remove(dto);
                //            temp.Add(dto);
                //        }
                //    }

                //    auxTotal.Detalles = temp;
                //    listAux.Add(auxTotal);
                //} 
                #endregion

                foreach (var item in distinct)
                {
                    DTO_ReportLibroDiarioTotales libro = new DTO_ReportLibroDiarioTotales();
                    libro.Detalles = new List<DTO_ReportLibroDiario>();

                    libro.Detalles = aux.Detalles.Where(x => x.CuentaID.Value == item).ToList();
                    listAux.Add(libro);
                }

                return listAux;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_LibroDiario");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Parametro para filtar por el año deseado</param>
        /// <param name="mes">Parametro para filtar por el mes deseado</param>
        /// <param name="tipoBalance">Parametro para filtar por el tipo Balance deseado</param>
        /// <returns></returns>
        public List<DTO_ReportLibroDiarioTotales> ReportesContabilidad_LibroDiarioComprobante(int año, int mes, string tipoBalance)
        {
            try
            {
                DTO_ReportLibroDiarioTotales aux = new DTO_ReportLibroDiarioTotales();
                List<DTO_ReportLibroDiarioTotales> listAux = new List<DTO_ReportLibroDiarioTotales>();

                aux.Detalles = new List<DTO_ReportLibroDiario>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                aux.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_LibroDiario(año, mes, tipoBalance);
                List<string> distinct = (from c in aux.Detalles select c.ComprobanteID.Value).Distinct().ToList();

                foreach (string item in distinct)
                {
                    DTO_ReportLibroDiarioTotales auxTotal = new DTO_ReportLibroDiarioTotales();
                    auxTotal.Detalles = new List<DTO_ReportLibroDiario>();

                    auxTotal.Detalles = aux.Detalles.Where(x => x.ComprobanteID.Value == item).ToList();
                    listAux.Add(auxTotal);
                }
                return listAux;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_LibroDiarioComprobante");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que genera el reporte desde el cliente
        /// </summary>
        /// <param name="año">Parametro para filtar por el año deseado</param>
        /// <param name="mes">Parametro para filtar por el mes deseado</param>
        /// <param name="tipoBalance">Parametro para filtar por el tipo Balance deseado</param>
        /// <returns></returns>
        public List<DTO_ReportLibroMayorTotales> ReportesContabildiad_LibroMayor(int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            DTO_ReportLibroMayorTotales libro = new DTO_ReportLibroMayorTotales();
            List<DTO_ReportLibroMayorTotales> libroMayor = new List<DTO_ReportLibroMayorTotales>();
            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            libro.Detalles = new List<DTO_ReportLibroMayor>();
            libro.Detalles = this._dal_ReportContabiliadad.DAL_ReportesContabildiad_LibroMayor(año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);

            libroMayor.Add(libro);

            return libroMayor;

        }

        /// <summary>
        /// Funcion que se encarga de generar la logica para la plantilla de exce
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mes">Mes que se desea ver</param>
        /// <param name="tipoBalance">Libro a consultar</param>
        /// <param name="cuentaIni">Filtro rango d cuentas, Cuenta Inicial</param>
        /// <param name="cuentaFin">Filtro rango d cuentas, Cuenta Final</param>
        /// <returns>URL con excel</returns>
        public string ReportesContabilidad_PlantillaExcelLibroMayor(int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportLibroMayor> plantillaExcel = new List<DTO_ReportLibroMayor>();
                string fileName;
                string str = this.GetCvsName(ExportType.Csv, out fileName);

                plantillaExcel = new List<DTO_ReportLibroMayor>();
                plantillaExcel = this._dal_ReportContabiliadad.DAL_ReportesContabildiad_LibroMayor(año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);

                if (plantillaExcel.Count > 0)
                {
                    int valor = int.Parse(this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_NivelCuentasLibroMayor));
                    List<DTO_ReportLibroMayor> filtroRangoCuenta = new List<DTO_ReportLibroMayor>();
                    filtroRangoCuenta = plantillaExcel.Where(x => x.CuentaID.Value.Length == valor).ToList();

                    CsvExport<DTO_ReportLibroMayor> csv = new CsvExport<DTO_ReportLibroMayor>(filtroRangoCuenta);
                    csv.ExportToFile(str, ExportType.Csv, true, string.Empty);
                }
                else
                    fileName = string.Empty;

                return fileName;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Saldos

        #region Filtro Cuenta

        /// <summary>
        /// Funcion que genera el reporte desde el cliente por el primir filtro "CUENTA"
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <returns></returns>
        public List<DTO_ReportSaldosTotales> ReportesContabilidad_SaldosCuenta(int año, int mesInicial, int mesFin, string libro)
        {
            try
            {
                DTO_ReportSaldosTotales saldo = new DTO_ReportSaldosTotales();
                List<DTO_ReportSaldosTotales> saldos = new List<DTO_ReportSaldosTotales>();
                saldo.Detalles = new List<DTO_ReportSaldos>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                saldo.Detalles = this._dal_ReportContabiliadad.DAL_ReportesCantabilidad_SaldosFuncional(año, mesInicial, mesFin, libro);
                List<string> distinct = (from c in saldo.Detalles select c.NombreCuenta.Value).Distinct().ToList();
                foreach (var item in distinct)
                {
                    List<DTO_ReportSaldos> temp = new List<DTO_ReportSaldos>();
                    DTO_ReportSaldosTotales saldoTotal = new DTO_ReportSaldosTotales();
                    DTO_ReportSaldos dto = new DTO_ReportSaldos();

                    foreach (var item1 in saldo.Detalles.Where(x => x.NombreCuenta.Value == item))
                    {
                        if (!temp.Any(x => x.TerceroID.Value == item1.TerceroID.Value || x.CentroCostoID.Value == item1.CentroCostoID.Value
                           || x.ProyectoID.Value == item1.ProyectoID.Value || x.LineaPresupuestoID.Value == item1.LineaPresupuestoID.Value))
                        {
                            temp.Add(item1);
                        }
                        else
                        {

                            dto = temp.Where(x => x.TerceroID.Value == item1.TerceroID.Value || x.CentroCostoID.Value == item1.CentroCostoID.Value
                                || x.ProyectoID.Value == item1.ProyectoID.Value || x.LineaPresupuestoID.Value == item1.LineaPresupuestoID.Value).FirstOrDefault();

                            dto.SaldoInicialML.Value += item1.SaldoInicialML.Value;
                            dto.SaldoInicialME.Value += item1.SaldoInicialME.Value;
                            dto.DebitoML.Value += item1.DebitoML.Value;
                            dto.CreditoML.Value += item1.CreditoML.Value;
                            dto.DebitoME.Value += item1.DebitoME.Value;
                            dto.CreditoME.Value += item1.CreditoME.Value;
                            dto.FinalML.Value += item1.FinalML.Value;
                            dto.FinalME.Value += item1.FinalME.Value;

                            temp.Remove(dto);
                            temp.Add(dto);
                        }
                        //dto.SaldoInicialML.Value = dto.SaldoInicialML.Value > 0 ? dto.SaldoInicialML.Value : dto.SaldoInicialML.Value * (-1);
                        //dto.SaldoInicialME.Value = dto.SaldoInicialME.Value > 0 ? dto.SaldoInicialME.Value : dto.SaldoInicialME.Value * (-1);
                        //dto.CreditoML.Value = dto.CreditoML.Value > 0 ? dto.CreditoML.Value : dto.CreditoML.Value * (-1);
                        //dto.CreditoME.Value = dto.CreditoME.Value > 0 ? dto.CreditoME.Value : dto.CreditoME.Value * (-1);
                        //dto.FinalML.Value = dto.FinalML.Value > 0 ? dto.FinalML.Value : dto.FinalML.Value * (-1);
                        //dto.FinalME.Value = dto.FinalME.Value > 0 ? dto.FinalME.Value : dto.FinalME.Value * (-1);
                    }

                    saldoTotal.Detalles = temp;
                    saldos.Add(saldoTotal);
                }
                return saldos;
            }

            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_SaldosCuenta");
                throw ex;
            }

        }

        #endregion

        #region Filtro Tercero

        /// <summary>
        /// Funcion que genera el reporte desde el cliente por el primir filtro "TERCERO"
        /// </summary>
        /// <param name="año">Año que se desea ver</param>
        /// <param name="mesInicial">Mes inicial que se desea ver</param>
        /// <param name="mesFin">Mes Final,Hasta donde se desea ver</param>
        /// <param name="libro">Libro que se va a mostrar</param>
        /// <returns></returns>
        public List<DTO_ReportSaldosTotales> ReportesContabilidad_SaldosTercero(int año, int mesInicial, int mesFin, string libro)
        {
            try
            {
                DTO_ReportSaldosTotales saldo = new DTO_ReportSaldosTotales();
                List<DTO_ReportSaldosTotales> saldos = new List<DTO_ReportSaldosTotales>();
                saldo.Detalles = new List<DTO_ReportSaldos>();

                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                saldo.Detalles = this._dal_ReportContabiliadad.DAL_ReportesCantabilidad_SaldosFuncional(año, mesInicial, mesFin, libro);
                List<string> distinct = (from c in saldo.Detalles select c.NombreTercero.Value).Distinct().ToList();


                //foreach (var item in distinct)
                //{
                //    DTO_ReportSaldosTotales test = new DTO_ReportSaldosTotales();
                //    test.Detalles = new List<DTO_ReportSaldos>();

                //    test.Detalles = saldo.Detalles.Where(x => x.NombreTercero.Value == item).ToList();
                //    saldos.Add(test);
                //}

                foreach (var item in distinct)
                {
                    List<DTO_ReportSaldos> temp = new List<DTO_ReportSaldos>();
                    DTO_ReportSaldosTotales saldoTotal = new DTO_ReportSaldosTotales();
                    DTO_ReportSaldos dto = new DTO_ReportSaldos();

                    foreach (var item1 in saldo.Detalles.Where(x => x.NombreTercero.Value == item))
                    {
                        if (!temp.Any(x => x.CuentaID.Value == item1.CuentaID.Value))
                        {
                            temp.Add(item1);
                        }
                        else
                        {

                            dto = temp.Where(x => x.CuentaID.Value == item1.CuentaID.Value).FirstOrDefault();

                            dto.SaldoInicialML.Value += item1.SaldoInicialML.Value;
                            dto.SaldoInicialME.Value += item1.SaldoInicialME.Value;
                            dto.DebitoML.Value += item1.DebitoML.Value;
                            dto.CreditoML.Value += item1.CreditoML.Value;
                            dto.DebitoME.Value += item1.DebitoME.Value;
                            dto.CreditoME.Value += item1.CreditoME.Value;
                            dto.FinalML.Value += item1.FinalML.Value;
                            dto.FinalME.Value += item1.FinalME.Value;

                            temp.Remove(dto);
                            temp.Add(dto);
                        }
                        //dto.SaldoInicialML.Value = dto.SaldoInicialML.Value > 0 ? dto.SaldoInicialML.Value : dto.SaldoInicialML.Value * (-1);
                        //dto.SaldoInicialME.Value = dto.SaldoInicialME.Value > 0 ? dto.SaldoInicialME.Value : dto.SaldoInicialME.Value * (-1);
                        //dto.CreditoML.Value = dto.CreditoML.Value > 0 ? dto.CreditoML.Value : dto.CreditoML.Value * (-1);
                        //dto.CreditoME.Value = dto.CreditoME.Value > 0 ? dto.CreditoME.Value : dto.CreditoME.Value * (-1);
                        //dto.FinalML.Value = dto.FinalML.Value > 0 ? dto.FinalML.Value : dto.FinalML.Value * (-1);
                        //dto.FinalME.Value = dto.FinalME.Value > 0 ? dto.FinalME.Value : dto.FinalME.Value * (-1);
                    }

                    saldoTotal.Detalles = temp;
                    saldos.Add(saldoTotal);
                }
                return saldos;
            }

            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_SaldosTercero");
                throw ex;
            }

        }

        #endregion

        #endregion

        #region Tasas

        /// <summary>
        /// Funcion que se encarga de traer la informacion para el reporte de Tasas
        /// </summary>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="isDiaria">Tipo de reporte a imprimir (True: Reportes Tasa Cierre, False: Reprote Tasa Diaria)</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ContabilidadTotal> ReportesContabilidad_Tasas(DateTime Periodo, bool isDiaria)
        {
            this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_ContabilidadTotal> result = new List<DTO_ContabilidadTotal>();
            DTO_ContabilidadTotal tasas = new DTO_ContabilidadTotal(); this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            tasas.DetallesTasas = new List<DTO_ReportTasas>();

            tasas.DetallesTasas = this._dal_ReportContabiliadad.DAL_ReportesContabilidad_Tasas(Periodo, isDiaria);
            result.Add(tasas);

            return result;
        }

        #endregion

        #endregion

        #region Reportes XLS

        /// <summary>
        /// Obtiene un datatable con la info de Tesoreria segun filtros
        /// </summary>
        /// <param name="documentoID">Documento relacionado</param>
        /// <param name="tipoReporte">Tipo reporte</param>
        /// <param name="fechaIni">Fecha ini</param>
        /// <param name="fechaFin">Fecha Fin</param>
        /// <param name="terceroID">Tercero</param>
        /// <param name="cuentaID">Cuenta</param>
        /// <param name="centroCtoID">Centro Cto</param>
        /// <param name="proyectoID">Proyecto</param>
        /// <param name="lineaPresupID">Linea Presup</param>
        /// <param name="balanceTipo">Balance Tipo</param>
        /// <param name="comprobID">Comprobante ID</param>
        /// <param name="compNro">Comp nro</param>
        /// <param name="otroFilter">otro filtro</param>
        /// <param name="agrup">Agrupar</param>
        /// <param name="romp">romper u ordenar</param>
        /// <returns>datatable</returns>
        public DataTable Reportes_Co_ContabilidadToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string terceroID, string cuentaID, string centroCtoID, string proyectoID, string lineaPresupID, string balanceTipo, string comprobID,
                                                             string compNro, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ReportContabiliadad.DAL_Reportes_Co_ContabilidadToExcel(documentoID, tipoReporte, fechaIni, fechaFin, terceroID, cuentaID, centroCtoID, proyectoID,
                                                        lineaPresupID, balanceTipo, comprobID,compNro, otroFilter, agrup, romp);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Reportes_Co_ContabilidadToExcel");
                throw exception;
            }
        }

        #region Balance
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Periodo"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <returns></returns>
        public DataTable ReportesContabilidad_BalancePruebas(DateTime Periodo, int LongitudCuenta, int SaldoIncial, string CuentaInicial, string CuentaFinal,
            string libro, string tipoReport, string Moneda)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ReportContabiliadad.DAL_ReportesContabilidad_BalancePruebas(Periodo, LongitudCuenta, SaldoIncial, CuentaInicial, CuentaFinal, libro, tipoReport, Moneda);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesContabilidad_BalancePruebas");
                throw exception;
            }
        }

        /// <summary>
        /// Reporte Balance en Excel
        /// </summary>
        /// <param name="año"></param>
        /// <param name="LongitudCuenta"></param>
        /// <param name="SaldoIncial"></param>
        /// <param name="CuentaInicial"></param>
        /// <param name="CuentaFinal"></param>
        /// <param name="libro"></param>
        /// <param name="tipoReport"></param>
        /// <param name="Moneda"></param>
        /// <param name="MesInicial"></param>
        /// <param name="MesFinal"></param>
        /// <param name="Combo1"></param>
        /// <param name="Combo2"></param>
        /// <returns></returns>
        public DataTable ReportesContabilidad_ReporteBalancePruebasXLS(int año, int LongitudCuenta, int SaldoIncial, string CuentaInicial,
                                     string CuentaFinal, string libro, string tipoReport, string Moneda, int MesInicial, int MesFinal, byte? Combo1, byte? Combo2, string proyecto, string centroCto)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ReportContabiliadad.DAL_ReportesContabilidad_ReporteBalancePruebasXLS(año, LongitudCuenta, SaldoIncial, CuentaInicial,
                                     CuentaFinal, libro, tipoReport, Moneda, MesInicial, MesFinal, Combo1, Combo2, proyecto, centroCto);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesContabilidad_BalancePruebas");
                throw exception;
            }
        }
        #endregion

        #region Comprobante

        /// <summary>
        /// Carga el DTO para generar el excel con los comprobante
        /// </summary>
        /// <param name="Periodo">Periodo a Consultar</param>
        /// <param name="comprobanteID">Filtra los comprobantes q se desean ver</param>
        /// <param name="libro">Libro con el cual se va filtrar</param>
        /// <param name="comprobanteInicial">Numero comprobante Inicial (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <param name="comprobanteFinal">Numero comprobante Final (Si solo si se va a filtrar por un comprobante en especifico)</param>
        /// <returns></returns>
        public DataTable ReportesContabilidad_ComprobanteXLS(DateTime Periodo, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                this._dal_ReportContabiliadad = (DAL_ReportesContabilidad)this.GetInstance(typeof(DAL_ReportesContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ReportContabiliadad.DAL_ReportesContabilidad_ComprobanteXLS(Periodo, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesContabilidad_ComprobanteXLS");
                throw ex;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Revelaciones

        /// <summary>
        /// Documento Revelacion
        /// </summary>
        /// <param name="revelacion">objeto Revelacion</param>
        /// <returns></returns>
        public DTO_TxResult DocumentoRevelacion_Add(DTO_coDocumentoRevelacion revelacion)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                #region Valida Campos Not Null

                #endregion

                this._dal_coDocumentoRevelacion = (DAL_coDocumentoRevelacion)this.GetInstance(typeof(DAL_coDocumentoRevelacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_coDocumentoRevelacion.DAL_coDocumentoRevelacion_Add(revelacion);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DocumentoRevelacion_Add");
                return result;
            }
        }

        /// <summary>
        /// Obtiene un documento revelación por numero de documento
        /// </summary>
        ///<param name="numeroDoc">número de documento</param>
        ///<returns>Revelación</returns>
        public DTO_coDocumentoRevelacion DocumentoRevelacion_Get(int numeroDoc)
        {
            try
            {
                this._dal_coDocumentoRevelacion = (DAL_coDocumentoRevelacion)this.GetInstance(typeof(DAL_coDocumentoRevelacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                var result = this._dal_coDocumentoRevelacion.DAL_coDocumentoRevelacion_Get(numeroDoc);
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DocumentoRevelacion_Get");
                throw ex;
            }
        
        }

        #endregion

    }
}
