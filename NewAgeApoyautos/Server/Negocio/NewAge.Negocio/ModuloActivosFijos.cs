using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.ADO.Documentos.Activos_Fijos;
using SentenceTransformer;

namespace NewAge.Negocio
{
    public class ModuloActivosFijos : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_acActivoControl _dal_acActivoControl = null;
        private DAL_ActivosFijos _dal_ActivosFijos = null;
        private DAL_glControl _dal_glControl = null;
        private DAL_acActivoDocu _dal_acActivoDocu = null;
        private DAL_MasterComplex _dal_masterComplex = null;
        private DAL_MasterSimple _dal_masterSimple = null;
        private DAL_acSaldos _dal_acSaldos = null;
        private DAL_Comprobante _dalComprobante = null;
        private DAL_ReportesActivos _dal_ReportesActivos = null;
        private DAL_acActivoGarantia _dal_acActivoGarantia = null;

        #endregion
        #region Modulos
        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloProveedores _moduloProveedores = null;
        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Activos Fijos
        /// </summary>
        /// <param name="conn"></param>
        public ModuloActivosFijos(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Procesos

        /// <summary>
        /// Genera la depreciacion de los activos
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="periodo">Periodo de cierre</param>
        /// <param name="ctrlFunc">DocumentoControl del comprobante funcional</param>
        /// <param name="ctrlIFRS">DocumentoControl del comprobante IFRS</param>
        /// <param name="coComp">Comprobante funcional (maestra)</param>
        /// <param name="coCompIFRS">Comprobante IFRS (maestra)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Proceso_GenerarDepreciacionActivos(int documentID, DateTime periodo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_glDocumentoControl ctrlFunc = null;
            DTO_coComprobante coComp = null;
            DTO_coComprobante coCompIFRS = null;
            try
            {
                #region Variables

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Variables por defecto
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lugGeoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string ctoCostoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                // Variables del documento
                string af = this.GetAreaFuncionalByUser();
                string prefijoDoc = this.GetPrefijoByDocumento(AppDocuments.DepreciacionActivos);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                DateTime fechaDoc = DateTime.Now;
                if (DateTime.Now > periodo)
                {
                    int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, day);
                }

                //Info del coDocumento
                DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoMvto",
                    OperadorFiltro = "=",
                    ValorFiltro = "8",
                    OperadorSentencia = string.Empty
                };

                DTO_glConsulta query = new DTO_glConsulta();
                query.Filtros.Add(filter);

                DAL_MasterSimple dalMaster = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalMaster.DocumentID = AppMasters.acMovimientoTipo;
                IEnumerable<DTO_MasterBasic> acMovimientos = dalMaster.DAL_MasterSimple_GetPaged(1, 1, query, new List<DTO_glConsultaFiltro>(), true);

                //Libros
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string libroIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                #endregion
                #region Validaciones

                //Valida el libro funcional
                if (string.IsNullOrWhiteSpace(libroFunc))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_TipoBalanceFuncional + "&&" + string.Empty;

                    return result;
                }

                //Valida el libro IFRS
                if (string.IsNullOrWhiteSpace(libroIFRS))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_TipoBalanceIFRS + "&&" + string.Empty;

                    return result;
                }

                //Valida el tipo de movimiento
                if (acMovimientos == null || acMovimientos.Count() == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_NoMovDepreciacion;

                    return result;
                }

                DTO_acMovimientoTipo mov = (DTO_acMovimientoTipo)acMovimientos.First();
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, mov.coDocumentoID.Value, true, false);

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                    return result;
                }
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                //Valida que el comprobante IFRS
                if (string.IsNullOrWhiteSpace(coComp.ComprobanteIFRS.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_NoCompIFRS + "&&" + coDoc.ComprobanteID.Value;

                    return result;
                }
                coCompIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coComp.ComprobanteIFRS.Value, true, false);

                //Valida que el documento asociado tenga cuenta local
                if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + mov.coDocumentoID.Value;

                    return result;
                }

                #endregion
                #region Carga la información de la depreciacion
                this._dal_ActivosFijos = (DAL_ActivosFijos)base.GetInstance(typeof(DAL_ActivosFijos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                object obj = this._dal_ActivosFijos.DAL_ActivosFijos_GetComprobanteForDepreciacion(periodo, terceroPorDefecto);
                #endregion
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    return result;
                }
                else
                {
                    #region Trae el footer del comprobante funcional
                    List<DTO_ComprobanteFooter> footerFunc = ((List<DTO_ComprobanteFooter>)obj).Where(x => x.DatoAdd10.Value == libroFunc).ToList();
                    if (footerFunc.Count == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Ac_NoDepreciaFunc;
                        return result;
                    }
                    #endregion
                    #region Trae el footer del comprobante IFRS
                    List<DTO_ComprobanteFooter> footerIFRS = ((List<DTO_ComprobanteFooter>)obj).Where(x => x.DatoAdd10.Value == libroIFRS).ToList();
                    if (footerIFRS.Count == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Ac_NoDepreciaIFRS;
                        return result;
                    }
                    #endregion
                    #region Crea glDocumentoControl funcional e IFRS

                    ctrlFunc = new DTO_glDocumentoControl();
                    ctrlFunc.DocumentoNro.Value = 0;
                    ctrlFunc.DocumentoID.Value = AppDocuments.DepreciacionActivos;
                    ctrlFunc.LugarGeograficoID.Value = lugGeoXdef;
                    ctrlFunc.NumeroDoc.Value = 0;
                    ctrlFunc.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    ctrlFunc.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                    ctrlFunc.Fecha.Value = DateTime.Now;
                    ctrlFunc.FechaDoc.Value = fechaDoc;
                    ctrlFunc.PeriodoDoc.Value = periodo;
                    ctrlFunc.PeriodoUltMov.Value = periodo;
                    ctrlFunc.CuentaID.Value = coDoc.CuentaLOC.Value;
                    ctrlFunc.AreaFuncionalID.Value = af;
                    ctrlFunc.PrefijoID.Value = prefijoDoc;
                    ctrlFunc.ProyectoID.Value = proyXdef;
                    ctrlFunc.CentroCostoID.Value = ctoCostoXdef;
                    ctrlFunc.LineaPresupuestoID.Value = lineaXdef;
                    ctrlFunc.TerceroID.Value = terceroPorDefecto;
                    ctrlFunc.MonedaID.Value = mdaLoc;
                    ctrlFunc.TasaCambioCONT.Value = 0;
                    ctrlFunc.TasaCambioDOCU.Value = 0;
                    ctrlFunc.Descripcion.Value = "Depreciacion Activos (libro funcional e IFRS)";
                    ctrlFunc.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    ctrlFunc.seUsuarioID.Value = this.UserId;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.LiquidacionCredito, ctrlFunc, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);
                        return result;
                    }

                    ctrlFunc.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key); ;
                    #endregion
                    #region Carga el comprobante funcional

                    DTO_Comprobante comprobanteFunc = new DTO_Comprobante();
                    DTO_ComprobanteHeader headerFunc = new DTO_ComprobanteHeader();
                    headerFunc.ComprobanteID.Value = coComp.ID.Value;
                    headerFunc.ComprobanteNro.Value = 0;
                    headerFunc.Fecha.Value = fechaDoc;
                    headerFunc.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                    headerFunc.MdaTransacc.Value = mdaLoc;
                    headerFunc.NumeroDoc.Value = ctrlFunc.NumeroDoc.Value;
                    headerFunc.PeriodoID.Value = periodo;
                    headerFunc.TasaCambioBase.Value = 0;
                    headerFunc.TasaCambioOtr.Value = 0;

                    comprobanteFunc.Header = headerFunc;
                    comprobanteFunc.Footer = footerFunc;
                    #endregion
                    #region Carga el comprobante IFRS

                    DTO_Comprobante comprobanteIFRS = new DTO_Comprobante();
                    DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                    headerIFRS.ComprobanteID.Value = coCompIFRS.ID.Value;
                    headerIFRS.ComprobanteNro.Value = 0;
                    headerIFRS.Fecha.Value = fechaDoc;
                    headerIFRS.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                    headerIFRS.MdaTransacc.Value = mdaLoc;
                    headerIFRS.NumeroDoc.Value = ctrlFunc.NumeroDoc.Value;
                    headerIFRS.PeriodoID.Value = periodo;
                    headerIFRS.TasaCambioBase.Value = 0;
                    headerIFRS.TasaCambioOtr.Value = 0;

                    comprobanteIFRS.Header = headerIFRS;
                    comprobanteIFRS.Footer = footerIFRS;
                    #endregion
                    #region Contabiliza los comprobantes

                    //Funcional
                    result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.DepreciacionActivos, comprobanteFunc, periodo, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    //IFRS
                    result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.DepreciacionActivos, comprobanteIFRS, periodo, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    #endregion
                    #region Mayoriza los libros

                    //Funcional 
                    result = this._moduloContabilidad.Proceso_Mayorizar(AppDocuments.DepreciacionActivos, periodo, libroFunc, new Dictionary<Tuple<int, int>, int>(), true);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    //IFRS 
                    result = this._moduloContabilidad.Proceso_Mayorizar(AppDocuments.DepreciacionActivos, periodo, libroIFRS, new Dictionary<Tuple<int, int>, int>(), true);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GenerarDepreciacion");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Genera el consecutivo
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(ctrlFunc.DocumentoID.Value.Value, ctrlFunc.PrefijoID.Value));
                        int comprobanteNro = this.GenerarComprobanteNro(coComp, ctrlFunc.PrefijoID.Value, ctrlFunc.PeriodoDoc.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlFunc, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlFunc.NumeroDoc.Value.Value, comprobanteNro, false, coComp.ID.Value);

                        //Obtiene ComprobanteNro para IFRS 
                        int comprobanteNroIFRS = this.GenerarComprobanteNro(coCompIFRS, ctrlFunc.PrefijoID.Value, ctrlFunc.PeriodoDoc.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlFunc, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlFunc.NumeroDoc.Value.Value, comprobanteNroIFRS, false, coCompIFRS.ID.Value);


                        #endregion
                    }
                    else
                        throw new Exception("ContabilizaLiquidacion - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }


        /// <summary>
        /// Genera un Reproceso de la Depreciación por Unidades de Producción
        /// </summary>
        /// </summary>
        /// <param name="documentoID">Identificador del documento</param>
        /// <param name="fechaIni">fechaIni</param>
        /// <param name="fechaFinal">fechaFinal</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_TxResult> Proceso_ReProcesoDepreciacion(int documentID, DateTime fechaIni, DateTime fechaFinal, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            List<DTO_TxResult> lResults = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            DTO_glDocumentoControl ctrlFunc = null;
            DTO_coComprobante coComp = null;
            DTO_coComprobante coCompIFRS = null;
            DateTime periodo = fechaIni;
            List<DTO_coComprobante> coCompsReversion = null;
            List<DTO_glDocumentoControl> ctrlsReversion = null;

            try
            {

                #region Variables

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                // Variables por defecto
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lugGeoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string ctoCostoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                // Variables del documento
                string af = this.GetAreaFuncionalByUser();
                string prefijoDoc = this.GetPrefijoByDocumento(AppDocuments.DepreciacionActivos);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                long meses = meses = DateTimeExtension.DateDiff(DateInterval.Month, fechaIni, fechaFinal) + 1;
                DateTime fechaDoc = DateTime.Now;

                //Info del coDocumento
                DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoMvto",
                    OperadorFiltro = "=",
                    ValorFiltro = "8",
                    OperadorSentencia = string.Empty
                };

                DTO_glConsulta query = new DTO_glConsulta();
                query.Filtros.Add(filter);

                DAL_MasterSimple dalMaster = (DAL_MasterSimple)base.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                dalMaster.DocumentID = AppMasters.acMovimientoTipo;
                IEnumerable<DTO_MasterBasic> acMovimientos = dalMaster.DAL_MasterSimple_GetPaged(1, 1, query, new List<DTO_glConsultaFiltro>(), true);

                //Libros
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string libroIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                #endregion

                for (int i = 0; i < meses; i++)
                {

                    #region Validaciones

                    if (DateTime.Now > periodo)
                    {
                        int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                        fechaDoc = new DateTime(periodo.Year, periodo.Month, day);
                    }

                    //Valida el libro funcional
                    if (string.IsNullOrWhiteSpace(libroFunc))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_TipoBalanceFuncional + "&&" + string.Empty;

                        lResults.Add(result);
                    }

                    //Valida el libro IFRS
                    if (string.IsNullOrWhiteSpace(libroIFRS))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.co).ToString() + AppControl.co_TipoBalanceIFRS + "&&" + string.Empty;

                        lResults.Add(result);
                    }

                    //Valida el tipo de movimiento
                    if (acMovimientos == null || acMovimientos.Count() == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Ac_NoMovDepreciacion;

                        lResults.Add(result);
                    }

                    DTO_acMovimientoTipo mov = (DTO_acMovimientoTipo)acMovimientos.First();
                    DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, mov.coDocumentoID.Value, true, false);

                    //Valida que tenga comprobante
                    if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                        lResults.Add(result);
                    }
                    coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                    //Valida que el comprobante IFRS
                    if (string.IsNullOrWhiteSpace(coComp.ComprobanteIFRS.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_NoCompIFRS + "&&" + coDoc.ComprobanteID.Value;

                        lResults.Add(result);
                    }
                    coCompIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coComp.ComprobanteIFRS.Value, true, false);

                    //Valida que el documento asociado tenga cuenta local
                    if (string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + mov.coDocumentoID.Value;

                        lResults.Add(result);
                    }

                    #endregion
                    #region Carga la información de la depreciacion
                    this._dal_ActivosFijos = (DAL_ActivosFijos)base.GetInstance(typeof(DAL_ActivosFijos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    object obj = this._dal_ActivosFijos.DAL_ActivosFijos_GetComprobanteForDepreciacion(periodo, terceroPorDefecto);
                    #endregion
                    if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)obj;
                        lResults.Add(result);
                    }
                    else
                    {
                        #region Validacion Existe y Reversion de Documento
                        DTO_glDocumentoControl docDepre = this._moduloGlobal.glDocumentoControl_GetByPeriodoDocumento(AppDocuments.DepreciacionActivos, periodo).FirstOrDefault();
                        if (docDepre != null)
                        {
                            result = this._moduloGlobal.glDocumentoControl_Revertir(docDepre.DocumentoID.Value.Value, docDepre.NumeroDoc.Value.Value, null, ref ctrlsReversion, ref coCompsReversion, true);
                            if (result.Result == ResultValue.NOK)
                            {
                                lResults.Add(result);
                                return lResults;
                            }
                        }
                        else
                        {
                            if (result.Result == ResultValue.NOK)
                            {
                                result.ResultMessage = DictionaryMessages.Err_Ac_CierrePeriodo + "&&" + periodo.ToString();
                                lResults.Add(result);
                                return lResults;
                            }
                        }

                        #endregion
                        #region Trae el footer del comprobante funcional
                        List<DTO_ComprobanteFooter> footerFunc = ((List<DTO_ComprobanteFooter>)obj).Where(x => x.DatoAdd10.Value == libroFunc).ToList();
                        if (footerFunc.Count == 0)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Ac_NoDepreciaFunc;
                            lResults.Add(result);
                            return lResults;
                        }
                        #endregion
                        #region Trae el footer del comprobante IFRS
                        List<DTO_ComprobanteFooter> footerIFRS = ((List<DTO_ComprobanteFooter>)obj).Where(x => x.DatoAdd10.Value == libroIFRS).ToList();
                        if (footerIFRS.Count == 0)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Ac_NoDepreciaIFRS;
                            lResults.Add(result);
                            return lResults;
                        }
                        #endregion
                        #region Crea glDocumentoControl para Comprobante Funcional e IFRS

                        ctrlFunc = new DTO_glDocumentoControl();
                        ctrlFunc.DocumentoNro.Value = 0;
                        ctrlFunc.DocumentoID.Value = AppDocuments.DepreciacionActivos;
                        ctrlFunc.LugarGeograficoID.Value = lugGeoXdef;
                        ctrlFunc.NumeroDoc.Value = 0;
                        ctrlFunc.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        ctrlFunc.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                        ctrlFunc.Fecha.Value = DateTime.Now;
                        ctrlFunc.FechaDoc.Value = fechaDoc;
                        ctrlFunc.PeriodoDoc.Value = periodo;
                        ctrlFunc.PeriodoUltMov.Value = periodo;
                        ctrlFunc.CuentaID.Value = coDoc.CuentaLOC.Value;
                        ctrlFunc.AreaFuncionalID.Value = af;
                        ctrlFunc.PrefijoID.Value = prefijoDoc;
                        ctrlFunc.ProyectoID.Value = proyXdef;
                        ctrlFunc.CentroCostoID.Value = ctoCostoXdef;
                        ctrlFunc.LineaPresupuestoID.Value = lineaXdef;
                        ctrlFunc.TerceroID.Value = terceroPorDefecto;
                        ctrlFunc.MonedaID.Value = mdaLoc;
                        ctrlFunc.TasaCambioCONT.Value = 0;
                        ctrlFunc.TasaCambioDOCU.Value = 0;
                        ctrlFunc.Descripcion.Value = "Depreciacion Activos (libro funcional e IFRS)";
                        ctrlFunc.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        ctrlFunc.seUsuarioID.Value = this.UserId;

                        DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.LiquidacionCredito, ctrlFunc, true);
                        if (resultGLDC.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(resultGLDC);
                            lResults.Add(result);
                            return lResults;
                        }

                        ctrlFunc.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key); ;
                        #endregion
                        #region Carga el comprobante funcional

                        DTO_Comprobante comprobanteFunc = new DTO_Comprobante();
                        DTO_ComprobanteHeader headerFunc = new DTO_ComprobanteHeader();
                        headerFunc.ComprobanteID.Value = coComp.ID.Value;
                        headerFunc.ComprobanteNro.Value = 0;
                        headerFunc.Fecha.Value = fechaDoc;
                        headerFunc.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                        headerFunc.MdaTransacc.Value = mdaLoc;
                        headerFunc.NumeroDoc.Value = ctrlFunc.NumeroDoc.Value;
                        headerFunc.PeriodoID.Value = periodo;
                        headerFunc.TasaCambioBase.Value = 0;
                        headerFunc.TasaCambioOtr.Value = 0;

                        comprobanteFunc.Header = headerFunc;
                        comprobanteFunc.Footer = footerFunc;
                        #endregion
                        #region Carga el comprobante IFRS

                        DTO_Comprobante comprobanteIFRS = new DTO_Comprobante();
                        DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                        headerIFRS.ComprobanteID.Value = coCompIFRS.ID.Value;
                        headerIFRS.ComprobanteNro.Value = 0;
                        headerIFRS.Fecha.Value = fechaDoc;
                        headerIFRS.MdaOrigen.Value = (int)TipoMoneda_LocExt.Local;
                        headerIFRS.MdaTransacc.Value = mdaLoc;
                        headerIFRS.NumeroDoc.Value = ctrlFunc.NumeroDoc.Value;
                        headerIFRS.PeriodoID.Value = periodo;
                        headerIFRS.TasaCambioBase.Value = 0;
                        headerIFRS.TasaCambioOtr.Value = 0;

                        comprobanteIFRS.Header = headerIFRS;
                        comprobanteIFRS.Footer = footerIFRS;
                        #endregion
                        #region Contabiliza los comprobantes

                        //Funcional
                        result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.DepreciacionActivos, comprobanteFunc, periodo, ModulesPrefix.ac, 0, false);
                        if (result.Result == ResultValue.NOK)
                        {
                            lResults.Add(result);
                            return lResults;
                        }

                        //IFRS
                        result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.DepreciacionActivos, comprobanteIFRS, periodo, ModulesPrefix.ac, 0, false);
                        if (result.Result == ResultValue.NOK)
                        {
                            lResults.Add(result);
                            return lResults;
                        }

                        #endregion
                        #region Mayoriza los libros

                        //Funcional 
                        result = this._moduloContabilidad.Proceso_Mayorizar(AppDocuments.DepreciacionActivos, periodo, libroFunc, new Dictionary<Tuple<int, int>, int>(), true);
                        if (result.Result == ResultValue.NOK)
                        {
                            lResults.Add(result);
                            return lResults;
                        }

                        //IFRS 
                        result = this._moduloContabilidad.Proceso_Mayorizar(AppDocuments.DepreciacionActivos, periodo, libroIFRS, new Dictionary<Tuple<int, int>, int>(), true);
                        if (result.Result == ResultValue.NOK)
                        {
                            lResults.Add(result);
                            return lResults;
                        }
                        #endregion
                    }
                    #region Reasignacion Variables
                    periodo = periodo.AddMonths(1);
                    #endregion
                }

                return lResults;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GenerarDepreciacion");
                lResults.Add(result);
                return lResults;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Genera el consecutivo
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(ctrlFunc.DocumentoID.Value.Value, ctrlFunc.PrefijoID.Value));
                        int comprobanteNro = this.GenerarComprobanteNro(coComp, ctrlFunc.PrefijoID.Value, ctrlFunc.PeriodoDoc.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlFunc, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlFunc.NumeroDoc.Value.Value, comprobanteNro, false, coComp.ID.Value);

                        //Obtiene ComprobanteNro para IFRS 
                        int comprobanteNroIFRS = this.GenerarComprobanteNro(coCompIFRS, ctrlFunc.PrefijoID.Value, ctrlFunc.PeriodoDoc.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlFunc, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlFunc.NumeroDoc.Value.Value, comprobanteNroIFRS, false, coCompIFRS.ID.Value);


                        #endregion
                    }
                    else
                        throw new Exception("ContabilizaLiquidacion - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }


        #endregion

        #region acActivoDocu

        /// <summary>
        /// Guarda en acActivoDocu
        /// </summary>
        /// <param name="acActDocu">acActDocu</param>
        /// <returns>Objeto de resultado</returns>
        private DTO_TxResult acActivoDocu_Add(DTO_acActivoDocu acActDocu)
        {
            this._dal_acActivoDocu = (DAL_acActivoDocu)this.GetInstance(typeof(DAL_acActivoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoDocu.DAL_acActivoDocu_add(acActDocu);
        }

        #endregion

        #region acActivoControl

        /// <summary>
        /// Agrega un registro a acActivoControl
        /// </summary>
        /// <param name="documentID">Documento que realiza el proceso</param>
        /// <param name="acCtrl">Registroo para agregar</param>
        /// <param name="updateRecibido">Indica si se debe actualizar el registro de DTO_prDetalleDocu (documento en el campo ActivoID)</param>
        /// <returns></returns>
        public DTO_TxResultDetail acActivoControl_Add(int documentoID, DTO_acActivoControl acCtrl, string ActivoDocuID, bool updateRecibido, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            DAL_MasterSimple dalMasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_MasterBasic master = new DTO_MasterBasic();
                rd.Message = "OK";
                bool validDto = true;

                #region Validar FKs

                string msg_FkNotFound = DictionaryMessages.FkNotFound;
                string msg_EmptyField = DictionaryMessages.EmptyField;
                #region Moneda

                dalMasterSimple.DocumentID = AppMasters.glMoneda;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glMoneda, acCtrl.MonedaID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.MonedaID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "MonedaID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.MonedaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region Tercero

                dalMasterSimple.DocumentID = AppMasters.coTercero;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, acCtrl.TerceroID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.TerceroID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "TerceroID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.TerceroID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region Proyecto

                dalMasterSimple.DocumentID = AppMasters.coProyecto;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, acCtrl.ProyectoID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.ProyectoID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ProyectoID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.ProyectoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region CentroCosto

                dalMasterSimple.DocumentID = AppMasters.coCentroCosto;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, acCtrl.CentroCostoID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.CentroCostoID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CentroCostoID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.CentroCostoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region Tipo

                //dalMasterSimple.DocumentID = AppMasters.acTipo;
                //master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acTipo, acCtrl.Tipo.Value.ToString(), true, false);
                //if (!string.IsNullOrWhiteSpace(acCtrl.ActivoTipoID.Value) && master == null)
                //{
                //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                //    rdF.Field = "ActivoTipoID";
                //    rdF.Message = msg_FkNotFound + "&&" + acCtrl.ActivoTipoID.Value;
                //    rd.DetailsFields.Add(rdF);
                //    validDto = false;
                //}

                #endregion
                #region DocumentoID

                dalMasterSimple.DocumentID = AppMasters.glDocumento;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, acCtrl.DocumentoID.Value.ToString(), true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.DocumentoID.Value.ToString()) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "DocumentoID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.DocumentoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region CodigoBSID
                dalMasterSimple.DocumentID = AppMasters.prBienServicio;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, acCtrl.CodigoBSID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.CodigoBSID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "CodigoBSID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.CodigoBSID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region inReferenciID
                dalMasterSimple.DocumentID = AppMasters.inReferencia;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, acCtrl.inReferenciaID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.inReferenciaID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "inReferenciaID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.inReferenciaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region ActivoGrupoID

                dalMasterSimple.DocumentID = AppMasters.acGrupo;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acGrupo, acCtrl.ActivoGrupoID.Value, true, false);
                if (string.IsNullOrWhiteSpace(acCtrl.ActivoGrupoID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoGrupoID";
                    rdF.Message = msg_EmptyField + "&&" + acCtrl.ActivoGrupoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                else if (!string.IsNullOrWhiteSpace(acCtrl.ActivoGrupoID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoGrupoID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.ActivoGrupoID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region ActivoClaseID

                dalMasterSimple.DocumentID = AppMasters.acClase;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acClase, acCtrl.ActivoClaseID.Value, true, false);
                if (string.IsNullOrWhiteSpace(acCtrl.ActivoGrupoID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoClaseID";
                    rdF.Message = msg_EmptyField + "&&" + acCtrl.ActivoClaseID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                else if (!string.IsNullOrWhiteSpace(acCtrl.ActivoGrupoID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "ActivoClaseID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.ActivoClaseID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region LocFisicaID

                dalMasterSimple.DocumentID = AppMasters.glLocFisica;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLocFisica, acCtrl.LocFisicaID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.LocFisicaID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "LocFisicaID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.LocFisicaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region BodegaID

                dalMasterSimple.DocumentID = AppMasters.inBodega;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, acCtrl.BodegaID.Value, true, false);
                if (!string.IsNullOrWhiteSpace(acCtrl.BodegaID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "BodegaID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.BodegaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                #region EstadoActID

                dalMasterSimple.DocumentID = AppMasters.acEstado;
                master = (DTO_MasterBasic)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acEstado, acCtrl.EstadoActID.Value, true, false);
                if (string.IsNullOrWhiteSpace(acCtrl.EstadoActID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EstadoActID";
                    rdF.Message = msg_EmptyField;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                else if (!string.IsNullOrWhiteSpace(acCtrl.EstadoActID.Value) && master == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "EstadoActID";
                    rdF.Message = msg_FkNotFound + "&&" + acCtrl.EstadoActID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }

                #endregion
                if (!validDto)
                {
                    rd.Message = "NOK";
                    return rd;
                }
                #endregion

                int actId = this._dal_acActivoControl.DAL_acActivoControl_Add(acCtrl);
                if (actId != 0)
                {
                    rd.Key = actId.ToString();
                    #region Revisa si debe actualizar el registro de DetalleDocu
                    if (updateRecibido)
                    {
                        this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        DTO_prDetalleDocu rec = this._moduloProveedores.prDetalleDocu_GetByID(acCtrl.ConsecutivoDetaID.Value.Value);
                        rec.ActivoID.Value = actId;
                        rec.ActivoDocuID.Value = Convert.ToInt32(ActivoDocuID);

                        this._moduloProveedores.prDetalleDocu_Update(documentoID, rec, true, true);
                    }
                    #endregion
                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, acCtrl.DocumentoID.Value.Value, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, acCtrl.DocumentoID.Value.Value.ToString(), actId.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                    rd.Key = actId.ToString();
                    #endregion
                }
                else
                {
                    rd.Message = ResultValue.NOK.ToString();
                }
                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = ResultValue.NOK.ToString();
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloActivosFijos_acActivoControl_Add");
                throw exception;
            }
            finally
            {
                if (rd.Message == ResultValue.OK.ToString())
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    this._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Agrega una lista de registros a acActivoControl
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccion</param>
        /// <param name="acActivoControlList">Lista de registros</param>
        /// <returns>Retorna una lista de resultados (uno por cada registro)</returns>
        public List<DTO_TxResult> acActivoControl_AddList(int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto, decimal tasaCambio, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobante = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante comp = null;
            DTO_coComprobante compIFRS = null;


            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);


                #region Variables
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                int numDoc = Convert.ToInt32(acActivoControlList.First().NumeroDoc.Value);
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.ComprobanteIFRS.Value, true, false);
                var modulosActivos = this._moduloAplicacion.aplModulo_GetByVisible(1, false);

                bool modProveedores = true;
                bool modCxP = true;

                if (!modulosActivos.Any(x => x.ModuloID.Value == ModulesPrefix.pr.ToString()))
                    modProveedores = false;

                if (!modulosActivos.Any(x => x.ModuloID.Value == ModulesPrefix.cp.ToString()))
                    modCxP = false;

                //Costos contrapartida
                decimal CostoLOCConPtida = 0;
                decimal CostoEXTConPtida = 0;
                decimal CostoOTRConPtida = 0;

                // Variables por defecto
                string componente = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);
                #endregion
                #region Crea GlDocumentoControl LOCAL

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);

                glCtrl = new DTO_glDocumentoControl();
                DTO_glDocumentoControl factura = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = modCxP ? factura.TasaCambioCONT.Value : tasaCambio;
                glCtrl.TasaCambioDOCU.Value = modCxP ? factura.TasaCambioCONT.Value : tasaCambio;
                glCtrl.ComprobanteID.Value = comp.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.TerceroID.Value = modCxP ? factura.TerceroID.Value : string.Empty;
                glCtrl.DocumentoTercero.Value = modCxP ? factura.DocumentoTercero.Value : string.Empty;
                glCtrl.CuentaID.Value = mdaLoc == (modCxP ? factura.MonedaID.Value : mdaLoc) ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                glCtrl.MonedaID.Value = modCxP ? factura.MonedaID.Value : mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                if (glCtrl.CuentaID.Value == string.Empty)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_CountNoAssigned;
                    results.Add(result);
                    return results;
                }

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea acActivoDocu
                DTO_acActivoDocu acDocu = new DTO_acActivoDocu();

                acDocu.EmpresaID.Value = acActivoControlList.First().EmpresaID.Value;
                acDocu.DatoAdd1.Value = null;
                acDocu.DatoAdd2.Value = null;
                acDocu.DatoAdd3.Value = null;
                acDocu.DatoAdd4.Value = null;
                acDocu.DatoAdd5.Value = null;
                acDocu.DocumentoREL.Value = numDoc;
                acDocu.Iva.Value = 0;
                acDocu.Valor.Value = 0;
                acDocu.LocFisicaID.Value = acActivoControlList.First().LocFisicaID.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo
                acDocu.MvtoTipoActID.Value = tipoMvto;
                acDocu.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                acDocu.Observacion.Value = acActivoControlList.First().Descriptivo.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo

                result = this.acActivoDocu_Add(acDocu);

                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();    //Comprobante Contabilidad Local
                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = comp.ID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.FechaDoc.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;

                //DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                //headerIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                //headerIFRS.ComprobanteNro.Value = 0;
                //headerIFRS.EmpresaID.Value = glCtrl.EmpresaID.Value;
                //headerIFRS.Fecha.Value = glCtrl.FechaDoc.Value;
                //headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                //headerIFRS.MdaTransacc.Value = glCtrl.MonedaID.Value;
                //headerIFRS.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                //headerIFRS.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                //headerIFRS.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                //headerIFRS.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                //comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    DTO_acActivoControl acCtrl = acActivoControlList[i];
                    acCtrl.ValorSalvamentoIFRSUS.Value = factura.TasaCambioCONT.Value != 0  ? acCtrl.ValorSalvamentoIFRS.Value / factura.TasaCambioCONT.Value : 0;
                    acCtrl.NumeroDocCompra.Value = glCtrl.NumeroDoc.Value;

                    int? activoID = null;
                    if (!string.IsNullOrEmpty(acCtrl.SerialID.Value))
                    {
                        var actTemp = this._dal_acActivoControl.DAL_acActivoControl_GetBySerial(acCtrl.SerialID.Value);
                        if (actTemp != null)
                            activoID = actTemp.ActivoID.Value;
                    }
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;
                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Agregar AcActivoControl
                    if (activoID == null)
                    {
                        bool updateRecibido = true;
                        if (!modProveedores)
                            updateRecibido = false;

                        rd = this.acActivoControl_Add(documentoID, acCtrl, doc6ID, updateRecibido, true);
                        if (rd.Message == ResultValue.NOK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_AddDocument;
                            result.Details.Add(rd);
                            results.Add(result);
                            return results;
                        }
                    }
                    else
                    {
                        acCtrl.ActivoID.Value = activoID;

                        this._dal_acActivoControl.DAL_acActivoControl_Update(acCtrl, acCtrl.ActivoID.Value.Value);
                    }
                    results.Add(result);
                    #endregion
                    #region Agregar Footer

                    #region Traer Cuentas

                    DTO_acContabiliza acCont = new DTO_acContabiliza();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                    dic.Add("ComponenteActivoID", componente);

                    acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                    if (acCont == null)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                        return results;
                    }
                    #endregion

                    #region Crea el detalle comprobante  LOCAL

                    footer = new DTO_ComprobanteFooter();
                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    string actId = rd.Key;

                    if (activoID != null)
                    {
                        actId = activoID.ToString();
                        rd.Key = activoID.ToString();
                    }

                    footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    footer.CuentaID.Value = acCont.CuentaID.Value;
                    footer.ConceptoCargoID.Value = concCargoDef;
                    footer.DocumentoCOM.Value = string.Empty;
                    footer.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = Convert.ToInt32(actId);
                    footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = lgDef;
                    footer.PrefijoCOM.Value = prefijDef;
                    footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    footer.TerceroID.Value = glCtrl.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = acCtrl.CostoEXT.Value;
                    footer.vlrMdaLoc.Value = acCtrl.CostoLOC.Value;
                    footer.vlrMdaOtr.Value = acCtrl.CostoLOC.Value;

                    CostoLOCConPtida += acCtrl.CostoLOC.Value.Value;
                    CostoEXTConPtida += acCtrl.CostoEXT.Value.Value;
                    CostoOTRConPtida += acCtrl.CostoLOC.Value.Value;

                    comprobante.Footer.Add(footer);

                    #endregion

                    #region Asigna Componente de Retiro para IFRS


                    //DTO_ComprobanteFooter footerIFRS = new DTO_ComprobanteFooter();
                    //DTO_coPlanCuenta ctaProvIFRS = new DTO_coPlanCuenta();
                    //ctaProvIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    //footerIFRS.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    //footerIFRS.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    //footerIFRS.CuentaID.Value = acCont.CuentaID.Value;
                    //footerIFRS.ConceptoCargoID.Value = concCargoDef;
                    //footerIFRS.DocumentoCOM.Value = string.Empty;
                    //footerIFRS.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    //footerIFRS.ConceptoSaldoID.Value = ctaProvIFRS.ConceptoSaldoID.Value;
                    //footerIFRS.IdentificadorTR.Value = Convert.ToInt32(actId);
                    //footerIFRS.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    //footerIFRS.LugarGeograficoID.Value = lgDef;
                    //footerIFRS.PrefijoCOM.Value = prefijDef;
                    //footerIFRS.ProyectoID.Value = glCtrl.ProyectoID.Value;
                    //footerIFRS.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    //footerIFRS.TerceroID.Value = glCtrl.TerceroID.Value;
                    //footerIFRS.vlrBaseME.Value = 0;
                    //footerIFRS.vlrBaseML.Value = 0;
                    //footerIFRS.vlrMdaExt.Value = acCtrl.CostoEXT.Value + (glCtrl.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrl.TasaCambioCONT.Value : 0);
                    //footerIFRS.vlrMdaLoc.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;
                    //footerIFRS.vlrMdaOtr.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;

                    //comprobanteIFRS.Footer.Add(footerIFRS);

                    #region Costo Retiro IFRS

                    //if (acCtrl.ValorRetiroIFRS.Value.Value > 0)
                    //{
                    //    string ctaProvIFRScomp = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCostoDesmantelamiento);
                    //    dic.Clear();
                    //    dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                    //    dic.Add("ComponenteActivoID", ctaProvIFRScomp);

                    //    acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                    //    if (acCont == null)
                    //    {
                    //        result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                    //        result.Result = ResultValue.NOK;
                    //        results.Add(result);
                    //        return results;
                    //    }

                    //    DTO_ComprobanteFooter footerCostoRet = new DTO_ComprobanteFooter();
                    //    DTO_coPlanCuenta coPlanCtaRetIFRS = new DTO_coPlanCuenta();

                    //    //Obtiene la cuenta de Provisiones
                    //    coPlanCtaRetIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    //    footerCostoRet.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    //    footerCostoRet.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    //    footerCostoRet.CuentaID.Value = coPlanCtaRetIFRS.ID.Value;
                    //    footerCostoRet.ConceptoCargoID.Value = concCargoDef;
                    //    footerCostoRet.DocumentoCOM.Value = string.Empty;
                    //    footerCostoRet.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    //    footerCostoRet.ConceptoSaldoID.Value = coPlanCtaRetIFRS.ConceptoSaldoID.Value;
                    //    footerCostoRet.IdentificadorTR.Value = Convert.ToInt32(actId);
                    //    footerCostoRet.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    //    footerCostoRet.LugarGeograficoID.Value = lgDef;
                    //    footerCostoRet.PrefijoCOM.Value = prefijDef;
                    //    footerCostoRet.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    //    footerCostoRet.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    //    footerCostoRet.TerceroID.Value = glCtrl.TerceroID.Value;
                    //    footerCostoRet.vlrBaseME.Value = 0;
                    //    footerCostoRet.vlrBaseML.Value = 0;
                    //    footerCostoRet.vlrMdaExt.Value = (glCtrl.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrl.TasaCambioCONT.Value : 0) * -1;
                    //    footerCostoRet.vlrMdaLoc.Value = acCtrl.ValorRetiroIFRS.Value * -1;
                    //    footerCostoRet.vlrMdaOtr.Value = acCtrl.ValorRetiroIFRS.Value * -1;

                    //    comprobanteIFRS.Footer.Add(footerCostoRet);
                    //}

                    #endregion

                    #endregion

                    #endregion

                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = Convert.ToInt32(actId);
                    mov.EstadoInv.Value = acActivoControlList[i].EstadoInv.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mov.DocSoporteTER.Value = acActivoControlList[i].NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acActivoControlList[i].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoControlList[i].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoControlList[i].EmpresaID.Value;
                    mov.IdentificadorTr.Value = Convert.ToInt32(actId);
                    mov.inReferenciaID.Value = acActivoControlList[i].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    mov.ProyectoID.Value = acActivoControlList[i].ProyectoID.Value;
                    mov.SerialID.Value = acActivoControlList[i].SerialID.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta, false, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }
                #region Crea la contrapartida
                DTO_ComprobanteFooter footerConPtda = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerRetConPtda = new DTO_ComprobanteFooter();
                DTO_coPlanCuenta coPlanCptda = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, glCtrl.CuentaID.Value, true, false);

                string actID2 = rd.Key;

                footerConPtda.ActivoCOM.Value = null;
                footerConPtda.CentroCostoID.Value = cCosto;
                footerConPtda.CuentaID.Value = glCtrl.CuentaID.Value;
                footerConPtda.ConceptoCargoID.Value = concCargoDef;
                footerConPtda.DocumentoCOM.Value = "0";
                footerConPtda.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - CONTRAPARTIDA";
                footerConPtda.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                footerConPtda.ConceptoSaldoID.Value = coPlanCptda.ConceptoSaldoID.Value;
                footerConPtda.IdentificadorTR.Value = Convert.ToInt32(actID2);
                footerConPtda.LineaPresupuestoID.Value = lineaPres;
                footerConPtda.LugarGeograficoID.Value = glCtrl.LugarGeograficoID.Value;
                footerConPtda.PrefijoCOM.Value = prefijoID;
                footerConPtda.ProyectoID.Value = proyectoXDef;
                footerConPtda.TasaCambio.Value = glCtrl.TasaCambioDOCU.Value;
                footerConPtda.TerceroID.Value = glCtrl.TerceroID.Value;
                footerConPtda.vlrBaseME.Value = 0;
                footerConPtda.vlrBaseML.Value = 0;
                footerConPtda.vlrMdaExt.Value = (Math.Round(CostoEXTConPtida, 2) * -1);
                footerConPtda.vlrMdaLoc.Value = (Math.Round(CostoLOCConPtida, 2) * -1);
                footerConPtda.vlrMdaOtr.Value = (Math.Round(CostoOTRConPtida, 2) * -1);

                comprobante.Footer.Add(footerConPtda);
                //comprobanteIFRS.Footer.Add(footerConPtda);


                #endregion


                #region Contabiliza el comprobante

                //Contabiliza Comprobante Fiscal
                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }
                //else
                //{
                //    //Contabiliza Comprobante IFRS
                //    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                //    if (result.Result == ResultValue.NOK)
                //    {
                //        results.Clear();
                //        results.Add(result);
                //        return results;
                //    }
                //}

                #endregion
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_AddList");
                results.Add(result);

                return results;
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

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        int comprobanteNro = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNro, false, comp.ID.Value);

                        //Obtiene ComprobanteNro para IFRS 
                        //int comprobanteNroIFRS = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        //this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        //this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNroIFRS, false, compIFRS.ID.Value);

                    }
                    else
                        throw new Exception("acActivoControl_AddList - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Actualiza informacion el el acActivoControl
        /// </summary>
        /// <param name="acCtrl">Lista de DTO_AcActivoControl</param>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <returns>Lista de resultados</returns>
        public DTO_TxResult acActivoControl_Update(List<DTO_acActivoControl> acActivoControlList, string tipoMvto, int activoID)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            rd.line = 1;
            rd.Message = "OK";
            try
            {
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    DTO_acActivoControl acCtrl = acActivoControlList[i];

                    this._dal_acActivoControl = (DAL_acActivoControl)base.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_acActivoControl.DAL_acActivoControl_Update(acCtrl, activoID);

                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, acCtrl.DocumentoID.Value.Value, (int)FormsActions.Add, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, acCtrl.DocumentoID.Value.Value.ToString(), activoID.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);
                    rd.Key = activoID.ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_acActivoControl_Update");
            }
            return result;
        }

        /// <summary>
        /// Crea glDocumentoCtro, glMvtoDeta y llama la funcion de actualizar
        /// </summary>
        /// <param name="acActivoControl">Dto_acActivoControl</param>
        /// <param name="tipoMvto">Tipo de Movimiento del activo</param>
        /// <param name="activoID">id del activo</param>
        /// <returns>Objeto de resultados</returns>
        public DTO_TxResult acActivoControl_UpdatePlaqueta(List<DTO_acActivoControl> acActivoControl, string tipoMvto, int activoID, string prefijo, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            result.Result = ResultValue.OK;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl glCtrl = null;
            int docID = 0;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Variables
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);
                string periodo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);
                #endregion
                #region Update Activo control

                result = this.acActivoControl_Update(acActivoControl, tipoMvto, activoID);

                #endregion
                #region Crea glDocumentoControl

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = acActivoControl.First().DocumentoID.Value;
                glCtrl.Fecha.Value = acActivoControl.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = acActivoControl.First().Periodo.Value;
                glCtrl.PrefijoID.Value = prefijo;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = 0;
                glCtrl.TasaCambioDOCU.Value = 0;
                glCtrl.TerceroID.Value = acActivoControl.First().TerceroID.Value;
                glCtrl.DocumentoTercero.Value = acActivoControl.First().DocumentoTercero.Value;
                glCtrl.CentroCostoID.Value = acActivoControl.First().CentroCostoID.Value;
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodo);
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(glCtrl.DocumentoID.Value.Value, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    return result;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                #endregion
                #region Guarda el glMovimientoDeta
                for (int j = 0; j < acActivoControl.Count; j++)
                {
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                    mov.ActivoID.Value = acActivoControl[j].ActivoID.Value;
                    mov.CentroCostoID.Value = acActivoControl[j].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoControl[j].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoControl[j].EmpresaID.Value;
                    mov.IdentificadorTr.Value = acActivoControl[j].ActivoID.Value;
                    mov.inReferenciaID.Value = acActivoControl[j].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = glCtrl.NumeroDoc.Value;
                    mov.ProyectoID.Value = acActivoControl[j].ProyectoID.Value;
                    mov.SerialID.Value = acActivoControl[j].SerialID.Value;
                    mov.TerceroID.Value = acActivoControl[j].TerceroID.Value;
                    //mov.Valor1LOC.Value = acActivoControl[j].ValorSalvamentoLOC.Value;
                    //mov.Valor2LOC.Value = acActivoControl[j].ValorSalvamentoLOC.Value;
                    //mov.Valor1EXT.Value = acActivoControl[j].ValorSalvamentoEXT.Value;
                    //mov.Valor2EXT.Value = acActivoControl[j].ValorSalvamentoEXT.Value;
                    //mov.DatoAdd2.Value = acActivoControl[j].PlaquetaID.Value;
                    //mobDeta.Add(mov);
                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta);
                    if (result.Result == ResultValue.NOK)
                        result.Result = ResultValue.NOK;
                }
                #endregion
            }
            catch (Exception ex)
            {
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_UpdatePLaqueta");

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

                        glCtrl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijo));

                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, false, false);
                    }
                    else
                        throw new Exception("acActivoControl_UpdatePLaqueta - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }


            return result;
        }

        /// <summary>
        /// Trae los activos fijos para la Empresa actual
        /// </summary>
        /// <returns></returns>
        public List<DTO_acActivoControl> acActivoControl_Get()
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_Get();
        }

        /// <summary>
        /// Obtiene los activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public List<DTO_acActivoControl> acActivoControl_GetFilters(string serialID,
                                                                        string PlaquetaID,
                                                                        string locFisicaID,
                                                                        string referenciaID,
                                                                        string centroCosto,
                                                                        string proyecto,
                                                                        string clase,
                                                                        string tipo,
                                                                        string grupo,
                                                                        string responsable,
                                                                        bool isContenedor,
                                                                        int pageSize,
                                                                        int pageNum)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetFilters(serialID, PlaquetaID, locFisicaID, referenciaID, centroCosto, proyecto, clase, tipo, grupo, responsable, isContenedor, pageSize, pageNum);
        }


        /// <summary>
        /// Obtiene el numero de activos segun los filtros dados
        /// </summary>
        /// <param name="serialID">identificador de serial</param>
        /// <param name="PlaquetaID">identificador de plaqueta</param>
        /// <param name="locFisicaID">identificador de localizacion fisica</param>
        /// <param name="referenciaID">identifiador de referencia</param>
        /// <param name="centroCosto">identificador centro de costo</param>
        /// <param name="proyecto">identificador de proyecto</param>
        /// <param name="clase">identificador de clase</param>
        /// <param name="tipo">identificador de tipo</param>
        /// <param name="grupo">identificador de grupo</param>
        /// <param name="responsable">resposable</param>
        /// <param name="isContenedor">indica si es contendor</param>
        /// <returns>listado de Activos</returns>
        public int acActivoControl_GetFiltersCount(string serialID,
                                                        string PlaquetaID,
                                                        string locFisicaID,
                                                        string referenciaID,
                                                        string centroCosto,
                                                        string proyecto,
                                                        string clase,
                                                        string tipo,
                                                        string grupo,
                                                        string responsable,
                                                        bool isContenedor
                                                       )
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetFiltersCount(serialID, PlaquetaID, locFisicaID, referenciaID, centroCosto, proyecto, clase, tipo, grupo, responsable, isContenedor);

        }


        /// <summary>
        /// Trae un activo control de auerdo a la llave primaria
        /// </summary>
        /// <param name="activoID">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_acActivoControl acActivoControl_GetByID(int activoID)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetByID(activoID);
        }

        /// <summary>
        /// Trae un activo control
        /// </summary>
        /// <param name="plaqueta">Plaqueta</param>
        /// <returns>Retorna un activo</returns>
        public DTO_acActivoControl acActivoControl_GetByPlaqueta(string plaquetaID)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetByPlaqueta(plaquetaID);
        }

        /// <summary>
        /// Función que trae un acActivoControl de acuerdo al parametro.
        /// </summary>
        /// <param name="acCtrl">Dto</param>
        /// <returns></returns>
        public List<DTO_acActivoControl> acActivoControl_GetByParameter(DTO_acActivoControl acCtrl)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_acActivoControl> list = this._dal_acActivoControl.DAL_acActivoControl_GetByParameter(acCtrl);
            return list;
        }

        /// <summary>
        /// Función que trae un acActivoControl de acuerdo al parametro y tipo de movimiento.
        /// </summary>
        /// <param name="acCtrl">Dto</param>
        /// <returns></returns>
        public List<DTO_acActivoControl> acActivoControl_GetByParameterForTranfer(DTO_acActivoControl acCtrl)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_acActivoControl> list = this._dal_acActivoControl.DAL_acActivoControl_GetByParameterForTranfer(acCtrl);
            return list;
        }

        /// <summary>
        /// Trae los saldos de un activo
        /// </summary>
        /// <param name="acCtrl">DTO_acActivoControl</param>
        /// <returns>Lista de objetos de Resultados</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarSaldos(int identificadorTR, DateTime periodo, string clase)
        {
            string conceptoSaldo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ConceptoSaldo);
            string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetSaldo(periodo, conceptoSaldo, identificadorTR, tipoBalanceFuncional, clase);
        }

        /// <summary>
        /// Carga una lista de  dto_ActivoControl con los saldos por meses de acuerdo al año del periodo
        /// </summary>
        /// <param name="periodo">Periodo de busqueda</param>
        /// <param name="identificadorTR">ActivoID</param>
        /// <returns>Lista de saldos de un activo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarSaldos_Meses(string año, int identificadorTR, string activoClaseID, bool mLocal)
        {
            string conceptoSaldo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ConceptoSaldo);
            string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetSaldo_Meses(año, conceptoSaldo, identificadorTR, tipoBalanceFuncional, activoClaseID, mLocal);
        }

        /// <summary>
        /// Trae los Movimientos de un activo
        /// </summary>
        /// <param name="acCtrl">DTO_acActivoControl</param>
        /// <returns>Lista de objetos de Resultados</returns>
        public List<DTO_acActivoSaldo> acActivoControl_CargarMvtos(int identificadorTR, DateTime periodo, string clase)
        {
            string conceptoSaldo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ConceptoSaldo);
            string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetMvtos(periodo, conceptoSaldo, identificadorTR, tipoBalanceFuncional, clase);
        }

        /// <summary>
        /// Funcion que trae el comprobante de acuerdo al numeroDoc y el idTR
        /// </summary>
        /// <param name="numeroDoc">Numero del documento</param>
        /// <param name="identTR">IdentificadorTR</param>
        /// <returns>Retorna una lista de comprobanteFooter</returns>
        public List<DTO_ComprobanteFooter> acActivoControl_GetByIdentificadorTR(int numeroDoc, int identificadorTR)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_ComprobanteFooter> list = this._dal_acActivoControl.DAL_acActivoControl_GetByIdentificadorTR(numeroDoc, identificadorTR);
            return list;
        }

        /// <summary>
        /// Funcion para actualizar los saaldos por componente
        /// </summary>
        /// <param name="documentoID">Numero de documento</param>
        /// <param name="List<DTO_acActivoSaldo>">Lista de saldos para hacer el Update </param>
        /// <param name="identTR">id tr del activo</param>
        /// <param name="conceptoSaldo">LLave del Update</param>
        /// <param name="batchProgress"></param>
        /// <param name="insideAnotherTx"></param>
        /// <returns>Objeto de resultados</returns>
        public List<DTO_TxResult> acActivoControl_UpdateSaldos(int documentoID, string actividadFlujoID, string prefijoID, string tipoMvto, List<DTO_acActivoSaldo> dto_activoSaldo, List<DTO_acActivoControl> acActivoCtrl, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;
            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobante = null;
            DTO_coComprobante comp = null;
            try
            {
                #region Instancia de Modulos
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion
                #region Variables
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                int numDoc = Convert.ToInt32(acActivoCtrl.First().NumeroDoc.Value);
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                // Variables por defecto
                string componente = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ConceptoSaldo);
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);
                string terceroDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string concSaldoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoSaldoXDefecto);
                //Bool para verificar las iteraciones
                List<DTO_acActivoSaldo> saldosCu = new List<DTO_acActivoSaldo>();
                #endregion
                #region Crea GlDocumentoControl

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoCtrl.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = 0; // TODO PREGUNTAR POR ESTE CAMPO 
                glCtrl.TasaCambioDOCU.Value = 0; // TODO PREGUNTAR POR ESTE CAMPO
                glCtrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.TerceroID.Value = terceroDef;
                glCtrl.DocumentoTercero.Value = null;
                glCtrl.CuentaID.Value = coDoc.CuentaLOC.Value;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.PeriodoUltMov.Value = acActivoCtrl.First().Periodo.Value;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                if (glCtrl.CuentaID.Value == "")
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_CountNoAssigned;
                    results.Add(result);
                    return results;
                }

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea acActivoDocu
                DTO_acActivoDocu acDocu = new DTO_acActivoDocu();

                acDocu.EmpresaID.Value = acActivoCtrl.First().EmpresaID.Value;
                acDocu.DatoAdd1.Value = null;
                acDocu.DatoAdd2.Value = null;
                acDocu.DatoAdd3.Value = null;
                acDocu.DatoAdd4.Value = null;
                acDocu.DatoAdd5.Value = null;
                acDocu.DocumentoREL.Value = numDoc;
                acDocu.Iva.Value = 0;
                acDocu.Valor.Value = 0;
                acDocu.LocFisicaID.Value = acActivoCtrl.First().LocFisicaID.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo
                acDocu.MvtoTipoActID.Value = tipoMvto;
                acDocu.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                acDocu.Observacion.Value = acActivoCtrl.First().Observacion.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo

                result = this.acActivoDocu_Add(acDocu);

                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = glCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = glCtrl.ComprobanteIDNro.Value;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.Fecha.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;
                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter contraPtda = new DTO_ComprobanteFooter();
                List<DTO_ComprobanteFooter> contras = new List<DTO_ComprobanteFooter>();
                List<DTO_ComprobanteFooter> listTemp = new List<DTO_ComprobanteFooter>();
                List<DTO_acActivoSaldo> saldosCU = new List<DTO_acActivoSaldo>();
                List<DTO_acActivoControl> acvs = new List<DTO_acActivoControl>();
                List<int> idsTR = new List<int>();
                int idTR = 0;
                for (int i = 0; i < acActivoCtrl.Count; i++)
                {
                    if (idsTR.Contains(idTR))
                        break;

                    idTR = Convert.ToInt32(acActivoCtrl[i].ActivoID.Value);
                    idsTR.Add(idTR);

                    DTO_acActivoControl acCtrl = acActivoCtrl[i];

                    int percent = ((i + 1) * 100) / acActivoCtrl.Count;
                    batchProgress[tupProgress] = percent;
                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Agregar Footer

                    #region Crea el detalle

                    saldosCU = dto_activoSaldo.Where(x => x.IdentificadorTR.Value == acActivoCtrl[i].ActivoID.Value).ToList();

                    for (int j = 0; j < dto_activoSaldo.Count; j++)
                    {
                        #region Traer Cuentas

                        DTO_acContabiliza acCont = new DTO_acContabiliza();
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ConceptoSaldoID", componente);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                        if (acCont == null)
                        {
                            result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                            result.Result = ResultValue.NOK;
                            results.Add(result);
                            return results;
                        }
                        #endregion

                        #region Crea las partidas
                        footer = new DTO_ComprobanteFooter();
                        acvs = acActivoCtrl.Where(x => x.ActivoID.Value == dto_activoSaldo[j].IdentificadorTR.Value).ToList();
                        //CostoLOCConPtida += saldosCU[j].SaldoExt.Value.Value;
                        //CostoEXTConPtida += saldosCU[j].SaldoLoc.Value.Value;
                        //CostoOTRConPtida += saldosCU[j].SaldoLoc.Value.Value;

                        footer.ActivoCOM.Value = acvs.First().PlaquetaID.Value;
                        footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                        footer.CuentaID.Value = acCont.CuentaID.Value;
                        //footer.ConceptoCargoID.Value = acCont.ConceptoCargoID.Value;
                        footer.DocumentoCOM.Value = string.Empty;
                        footer.Descriptivo.Value = acCtrl.Descriptivo.Value;
                        footer.ConceptoSaldoID.Value = dto_activoSaldo[j].acComponenteID.Value;
                        footer.IdentificadorTR.Value = dto_activoSaldo[j].IdentificadorTR.Value;
                        footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                        footer.LugarGeograficoID.Value = lgDef;
                        footer.PrefijoCOM.Value = prefijDef;
                        footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                        footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                        footer.TerceroID.Value = glCtrl.TerceroID.Value;
                        footer.vlrBaseME.Value = 0;
                        footer.vlrBaseML.Value = 0;
                        footer.vlrMdaExt.Value = dto_activoSaldo[j].SaldoExt.Value.Value;
                        footer.vlrMdaLoc.Value = dto_activoSaldo[j].SaldoLoc.Value.Value;
                        footer.vlrMdaOtr.Value = dto_activoSaldo[j].SaldoLoc.Value.Value;

                        comprobante.Footer.Add(footer);
                        #endregion
                        #region Crea la Contrapartida

                        if (acActivoCtrl[i].CentroCostoID.Value != null && acActivoCtrl[i].ProyectoID.Value != null)
                            listTemp = contras.Where(x => x.CentroCostoID.Value == acActivoCtrl[i].CentroCostoID.Value && x.ProyectoID.Value == acActivoCtrl[i].ProyectoID.Value).ToList();
                        else if (acActivoCtrl[i].ProyectoID.Value != null)
                            listTemp = contras.Where(x => x.ProyectoID.Value == acActivoCtrl[i].ProyectoID.Value).ToList();
                        else if (acActivoCtrl[i].CentroCostoID.Value != null)
                            listTemp = contras.Where(x => x.CentroCostoID.Value == acActivoCtrl[i].CentroCostoID.Value).ToList();

                        if (listTemp.Count == 0)
                        {
                            contraPtda.ActivoCOM.Value = null;
                            contraPtda.CentroCostoID.Value = cCosto;
                            contraPtda.CuentaID.Value = glCtrl.CuentaID.Value;
                            contraPtda.ConceptoCargoID.Value = concCargoDef;
                            contraPtda.DocumentoCOM.Value = "0";
                            contraPtda.Descriptivo.Value = glCtrl.Observacion.Value;
                            contraPtda.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                            contraPtda.ConceptoSaldoID.Value = concSaldoDef;
                            contraPtda.IdentificadorTR.Value = 0;
                            contraPtda.LineaPresupuestoID.Value = lineaPres;
                            contraPtda.LugarGeograficoID.Value = glCtrl.LugarGeograficoID.Value;
                            contraPtda.PrefijoCOM.Value = prefijoID;
                            contraPtda.ProyectoID.Value = proyectoXDef;
                            contraPtda.TasaCambio.Value = glCtrl.TasaCambioDOCU.Value;
                            contraPtda.TerceroID.Value = glCtrl.TerceroID.Value;
                            contraPtda.vlrBaseME.Value = 0;
                            contraPtda.vlrBaseML.Value = 0;
                            contraPtda.vlrMdaExt.Value = footer.vlrMdaExt.Value.Value * -1;
                            contraPtda.vlrMdaLoc.Value = footer.vlrMdaLoc.Value.Value * -1;
                            contraPtda.vlrMdaOtr.Value = footer.vlrMdaOtr.Value.Value * -1;

                            contras.Add(contraPtda);
                        }
                        else
                        {
                            listTemp.First().vlrMdaExt.Value = listTemp.First().vlrMdaExt.Value.Value + footer.vlrMdaExt.Value.Value * -1;
                            listTemp.First().vlrMdaLoc.Value = listTemp.First().vlrMdaLoc.Value.Value + footer.vlrMdaLoc.Value.Value * -1;
                            listTemp.First().vlrMdaOtr.Value = listTemp.First().vlrMdaLoc.Value.Value + footer.vlrMdaOtr.Value.Value * -1;
                        }
                        #endregion
                    }

                    #endregion
                    #endregion
                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = dto_activoSaldo[i].IdentificadorTR.Value;
                    mov.EstadoInv.Value = acActivoCtrl[i].EstadoInv.Value;
                    mov.TerceroID.Value = acActivoCtrl[i].TerceroID.Value;
                    mov.DocSoporteTER.Value = acActivoCtrl[i].NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acActivoCtrl[i].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoCtrl[i].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoCtrl[i].EmpresaID.Value;
                    mov.IdentificadorTr.Value = dto_activoSaldo[i].IdentificadorTR.Value; ;
                    mov.inReferenciaID.Value = acActivoCtrl[i].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    mov.ProyectoID.Value = acActivoCtrl[i].ProyectoID.Value;
                    mov.SerialID.Value = acActivoCtrl[i].SerialID.Value;
                    mov.TerceroID.Value = acActivoCtrl[i].TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }
                    #endregion
                }

                #region Recorre las contras y las a grega al comprobante
                foreach (var item in contras)
                    comprobante.Footer.Add(item);

                #endregion

                #region Contabiliza el comprobante

                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }
                #endregion
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_AddList");
                results.Add(result);

                return results;
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

                        glCtrl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        glCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, glCtrl.ComprobanteIDNro.Value.Value, false);
                    }
                    else
                        throw new Exception("acActivoControl_UpdateSaldos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Trae los compponentes de acContabiliza de acuerdo a la clase del activo
        /// </summary>
        /// <param name="clase">Activo clase </param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetComponentesPorClaseActivoID(string clase)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetComponentesPorClaseActivoID(clase);
        }

        /// <summary>
        /// Funcion que trae una lista de saldos por componente
        /// </summary>
        /// <param name="periodo">preiodo del modulo</param>
        /// <param name="componentes">Lista con info del componetne</param>
        /// <param name="concSaldo">Compnente</param>
        /// <param name="identificadorTR">Id del activo</param>
        /// <param name="activoClaseID">Clase del activoID</param>
        /// <returns>Lista de activosaldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoXComponente(DateTime periodo, List<DTO_acActivoSaldo> componentes, int identificadorTR, string activoClaseID)
        {
            string tipoBalanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_acActivoSaldo saldo = new DTO_acActivoSaldo();
            List<DTO_acActivoSaldo> saldos = new List<DTO_acActivoSaldo>();
            foreach (DTO_acActivoSaldo item in componentes)
            {
                saldo = this._dal_acActivoControl.DAL_acActivoControl_GetSaldoXComponente(periodo, item.CuentaID.Value, item.Descriptivo.Value, item.acComponenteID.Value, identificadorTR, tipoBalanceFuncional, activoClaseID);
                if (saldo == null)
                {
                    saldo = new DTO_acActivoSaldo();
                    saldo.acComponenteID.Value = item.acComponenteID.Value;
                    saldo.Descriptivo.Value = item.Descriptivo.Value;
                    saldo.CuentaID.Value = item.CuentaID.Value;
                    saldo.SaldoMLoc.Value = 0;
                    saldo.SaldoMExt.Value = 0;
                    saldo.IdentificadorTR.Value = identificadorTR;
                }
                saldos.Add(saldo);
            }
            return saldos;
        }

        /// <summary>
        /// Trae un listado de activos que tengan referencias con garantias
        /// </summary>
        /// <returns>Datos</returns>
        public List<DTO_acActivoControl> acActivoControl_GetForGarantia()
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetForGarantia();           
        }

        /// <summary>
        /// Cambia el estado del activo Control
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <param name="estadoInv">estado</param>
        internal void acActivoControl_ChangeStatus(int activoID, byte estadoInv)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_acActivoControl.DAL_acActivoControl_ChangeStatus(activoID, estadoInv);
        }

        #endregion

        #region Alta de Activos

        /// <summary>
        /// Obtiene la lista de activos recibidos por numero de Factura
        /// </summary>
        /// <param name="numeroDoc">Numero de factura</param>
        /// <returns>Lista de activos</returns>
        public List<DTO_acActivoControl> AltaActivos_GetActivosByNumDoc(int numeroDoc)
        {
            this._dal_ActivosFijos = (DAL_ActivosFijos)this.GetInstance(typeof(DAL_ActivosFijos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<DTO_acActivoControl> list = this._dal_ActivosFijos.DAL_ActivosFijos_GetActivosByNumDoc(numeroDoc);
            return list;
        }

        #endregion

        #region Adicion Activos

        /// <summary>
        /// Agrega una lista de registros a acActivoControl
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccion</param>
        /// <param name="acActivoControlList">Lista de registros</param>
        /// <returns>Retorna una lista de resultados (uno por cada registro)</returns>
        public List<DTO_TxResult> acActivoControl_AddListActivos(int documentoID, int activoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto, decimal tasaCambio, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobante = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante comp = null;
            DTO_coComprobante compIFRS = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                int numDoc = Convert.ToInt32(acActivoControlList.First().NumeroDoc.Value);
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.ComprobanteIFRS.Value, true, false);
                var modulosActivos = this._moduloAplicacion.aplModulo_GetByVisible(1, false);

                bool modProveedores = true;
                bool modCxP = true;

                if (!modulosActivos.Any(x => x.ModuloID.Value == ModulesPrefix.pr.ToString()))
                    modProveedores = true;

                if (!modulosActivos.Any(x => x.ModuloID.Value == ModulesPrefix.cp.ToString()))
                    modCxP = true;

                //Costos contrapartida
                decimal CostoLOCConPtida = 0;
                decimal CostoEXTConPtida = 0;
                decimal CostoOTRConPtida = 0;

                // Variables por defecto
                string componente = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);
                #endregion
                #region Crea GlDocumentoControl LOCAL

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);

                glCtrl = new DTO_glDocumentoControl();
                DTO_glDocumentoControl factura = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = modCxP ? factura.TasaCambioCONT.Value : tasaCambio;
                glCtrl.TasaCambioDOCU.Value = modCxP ? factura.TasaCambioDOCU.Value : tasaCambio;
                glCtrl.ComprobanteID.Value = comp.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.TerceroID.Value = modCxP ? factura.TerceroID.Value : string.Empty;
                glCtrl.DocumentoTercero.Value = modCxP ? factura.DocumentoTercero.Value : string.Empty;
                glCtrl.CuentaID.Value = mdaLoc == (modCxP ? factura.MonedaID.Value : mdaLoc) ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                glCtrl.MonedaID.Value = modCxP ? factura.MonedaID.Value : mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                if (glCtrl.CuentaID.Value == string.Empty)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_CountNoAssigned;
                    results.Add(result);
                    return results;
                }

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea acActivoDocu
                DTO_acActivoDocu acDocu = new DTO_acActivoDocu();

                acDocu.EmpresaID.Value = acActivoControlList.First().EmpresaID.Value;
                acDocu.DatoAdd1.Value = null;
                acDocu.DatoAdd2.Value = null;
                acDocu.DatoAdd3.Value = null;
                acDocu.DatoAdd4.Value = null;
                acDocu.DatoAdd5.Value = null;
                acDocu.DocumentoREL.Value = numDoc;
                acDocu.Iva.Value = 0;
                acDocu.Valor.Value = 0;
                acDocu.LocFisicaID.Value = acActivoControlList.First().LocFisicaID.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo
                acDocu.MvtoTipoActID.Value = tipoMvto;
                acDocu.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                acDocu.Observacion.Value = acActivoControlList.First().Descriptivo.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo

                result = this.acActivoDocu_Add(acDocu);

                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();    //Comprobante Contabilidad Local
                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = comp.ID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.FechaDoc.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;

                DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                headerIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                headerIFRS.ComprobanteNro.Value = 0;
                headerIFRS.EmpresaID.Value = glCtrl.EmpresaID.Value;
                headerIFRS.Fecha.Value = glCtrl.FechaDoc.Value;
                headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                headerIFRS.MdaTransacc.Value = glCtrl.MonedaID.Value;
                headerIFRS.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                headerIFRS.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                headerIFRS.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                headerIFRS.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    DTO_acActivoControl acCtrl = acActivoControlList[i];
                    acCtrl.ValorSalvamentoIFRSUS.Value = acCtrl.ValorSalvamentoIFRS.Value / factura.TasaCambioCONT.Value;
                    acCtrl.NumeroDocCompra.Value = glCtrl.NumeroDoc.Value;

                    int? actID = null;
                    if (!string.IsNullOrEmpty(acCtrl.SerialID.Value))
                    {
                        var actTemp = this._dal_acActivoControl.DAL_acActivoControl_GetBySerial(acCtrl.SerialID.Value);
                        if (actTemp != null)
                            actID = actTemp.ActivoID.Value;
                    }
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;
                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Agregar AcActivoControl
                    if (actID == null)
                    {
                        bool updateRecibido = true;
                        if (!modProveedores)
                            updateRecibido = false;

                        rd = this.acActivoControl_Add(documentoID, acCtrl, doc6ID, updateRecibido, true);
                        if (rd.Message == ResultValue.NOK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_AddDocument;
                            result.Details.Add(rd);
                            results.Add(result);
                            return results;
                        }
                    }
                    else
                    {
                        acCtrl.ActivoID.Value = actID;
                        this._dal_acActivoControl.DAL_acActivoControl_Update(acCtrl, acCtrl.ActivoID.Value.Value);
                    }
                    results.Add(result);
                    #endregion
                    #region Agregar Footer

                    #region Traer Cuentas

                    DTO_acContabiliza acCont = new DTO_acContabiliza();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                    dic.Add("ComponenteActivoID", componente);

                    acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                    if (acCont == null)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                        return results;
                    }
                    #endregion

                    #region Crea el detalle comprobante  LOCAL

                    footer = new DTO_ComprobanteFooter();
                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    string actId = rd.Key;

                    footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    footer.CuentaID.Value = acCont.CuentaID.Value;
                    footer.ConceptoCargoID.Value = concCargoDef;
                    footer.DocumentoCOM.Value = string.Empty;
                    footer.Descriptivo.Value = "ADICION POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = activoID;
                    footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = lgDef;
                    footer.PrefijoCOM.Value = prefijDef;
                    footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    footer.TerceroID.Value = glCtrl.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = acCtrl.CostoEXT.Value;
                    footer.vlrMdaLoc.Value = acCtrl.CostoLOC.Value;
                    footer.vlrMdaOtr.Value = acCtrl.CostoLOC.Value;

                    CostoLOCConPtida += acCtrl.CostoLOC.Value.Value;
                    CostoEXTConPtida += acCtrl.CostoEXT.Value.Value;
                    CostoOTRConPtida += acCtrl.CostoLOC.Value.Value;

                    comprobante.Footer.Add(footer);

                    #endregion

                    #region Asigna Componente de Retiro para IFRS


                    DTO_ComprobanteFooter footerIFRS = new DTO_ComprobanteFooter();
                    DTO_coPlanCuenta ctaProvIFRS = new DTO_coPlanCuenta();
                    ctaProvIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    footerIFRS.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    footerIFRS.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    footerIFRS.CuentaID.Value = acCont.CuentaID.Value;
                    footerIFRS.ConceptoCargoID.Value = concCargoDef;
                    footerIFRS.DocumentoCOM.Value = string.Empty;
                    footerIFRS.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    footerIFRS.ConceptoSaldoID.Value = ctaProvIFRS.ConceptoSaldoID.Value;
                    footerIFRS.IdentificadorTR.Value = activoID;
                    footerIFRS.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    footerIFRS.LugarGeograficoID.Value = lgDef;
                    footerIFRS.PrefijoCOM.Value = prefijDef;
                    footerIFRS.ProyectoID.Value = glCtrl.ProyectoID.Value;
                    footerIFRS.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    footerIFRS.TerceroID.Value = glCtrl.TerceroID.Value;
                    footerIFRS.vlrBaseME.Value = 0;
                    footerIFRS.vlrBaseML.Value = 0;
                    footerIFRS.vlrMdaExt.Value = acCtrl.CostoEXT.Value + (glCtrl.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrl.TasaCambioCONT.Value : 0);
                    footerIFRS.vlrMdaLoc.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;
                    footerIFRS.vlrMdaOtr.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;

                    comprobanteIFRS.Footer.Add(footerIFRS);

                    #region Costo Retiro IFRS

                    if (acCtrl.ValorRetiroIFRS.Value.Value > 0)
                    {
                        string ctaProvIFRScomp = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCostoDesmantelamiento);
                        dic.Clear();
                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ComponenteActivoID", ctaProvIFRScomp);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                        if (acCont == null)
                        {
                            result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                            result.Result = ResultValue.NOK;
                            results.Add(result);
                            return results;
                        }

                        DTO_ComprobanteFooter footerCostoRet = new DTO_ComprobanteFooter();
                        DTO_coPlanCuenta coPlanCtaRetIFRS = new DTO_coPlanCuenta();

                        //Obtiene la cuenta de Provisiones
                        coPlanCtaRetIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                        footerCostoRet.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                        footerCostoRet.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                        footerCostoRet.CuentaID.Value = coPlanCtaRetIFRS.ID.Value;
                        footerCostoRet.ConceptoCargoID.Value = concCargoDef;
                        footerCostoRet.DocumentoCOM.Value = string.Empty;
                        footerCostoRet.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                        footerCostoRet.ConceptoSaldoID.Value = coPlanCtaRetIFRS.ConceptoSaldoID.Value;
                        footerCostoRet.IdentificadorTR.Value = activoID;
                        footerCostoRet.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                        footerCostoRet.LugarGeograficoID.Value = lgDef;
                        footerCostoRet.PrefijoCOM.Value = prefijDef;
                        footerCostoRet.ProyectoID.Value = acCtrl.ProyectoID.Value;
                        footerCostoRet.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                        footerCostoRet.TerceroID.Value = glCtrl.TerceroID.Value;
                        footerCostoRet.vlrBaseME.Value = 0;
                        footerCostoRet.vlrBaseML.Value = 0;
                        footerCostoRet.vlrMdaExt.Value = (glCtrl.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrl.TasaCambioCONT.Value : 0) * -1;
                        footerCostoRet.vlrMdaLoc.Value = acCtrl.ValorRetiroIFRS.Value * -1;
                        footerCostoRet.vlrMdaOtr.Value = acCtrl.ValorRetiroIFRS.Value * -1;

                        comprobanteIFRS.Footer.Add(footerCostoRet);
                    }

                    #endregion

                    #endregion

                    #endregion

                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = Convert.ToInt32(actId);
                    mov.EstadoInv.Value = acActivoControlList[i].EstadoInv.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mov.DocSoporteTER.Value = acActivoControlList[i].NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acActivoControlList[i].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoControlList[i].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoControlList[i].EmpresaID.Value;
                    mov.IdentificadorTr.Value = Convert.ToInt32(actId);
                    mov.inReferenciaID.Value = acActivoControlList[i].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    mov.ProyectoID.Value = acActivoControlList[i].ProyectoID.Value;
                    mov.SerialID.Value = acActivoControlList[i].SerialID.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta, false, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }
                #region Crea la contrapartida
                DTO_ComprobanteFooter footerConPtda = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerRetConPtda = new DTO_ComprobanteFooter();
                DTO_coPlanCuenta coPlanCptda = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, glCtrl.CuentaID.Value, true, false);

                string actID2 = rd.Key;

                footerConPtda.ActivoCOM.Value = null;
                footerConPtda.CentroCostoID.Value = cCosto;
                footerConPtda.CuentaID.Value = glCtrl.CuentaID.Value;
                footerConPtda.ConceptoCargoID.Value = concCargoDef;
                footerConPtda.DocumentoCOM.Value = "0";
                footerConPtda.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - CONTRAPARTIDA";
                footerConPtda.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                footerConPtda.ConceptoSaldoID.Value = coPlanCptda.ConceptoSaldoID.Value;
                footerConPtda.IdentificadorTR.Value = Convert.ToInt32(actID2);
                footerConPtda.LineaPresupuestoID.Value = lineaPres;
                footerConPtda.LugarGeograficoID.Value = glCtrl.LugarGeograficoID.Value;
                footerConPtda.PrefijoCOM.Value = prefijoID;
                footerConPtda.ProyectoID.Value = proyectoXDef;
                footerConPtda.TasaCambio.Value = glCtrl.TasaCambioDOCU.Value;
                footerConPtda.TerceroID.Value = glCtrl.TerceroID.Value;
                footerConPtda.vlrBaseME.Value = 0;
                footerConPtda.vlrBaseML.Value = 0;
                footerConPtda.vlrMdaExt.Value = (Math.Round(CostoEXTConPtida, 2) * -1);
                footerConPtda.vlrMdaLoc.Value = (Math.Round(CostoLOCConPtida, 2) * -1);
                footerConPtda.vlrMdaOtr.Value = (Math.Round(CostoOTRConPtida, 2) * -1);

                comprobante.Footer.Add(footerConPtda);
                comprobanteIFRS.Footer.Add(footerConPtda);


                #endregion
                #region Contabiliza el comprobante

                //Contabiliza Comprobante Fiscal
                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }
                else
                {
                    //Contabiliza Comprobante IFRS
                    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Clear();
                        results.Add(result);
                        return results;
                    }
                }

                #endregion
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_AddList");
                results.Add(result);

                return results;
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

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        int comprobanteNro = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNro, false, comp.ID.Value);

                        //Obtiene ComprobanteNro para IFRS 
                        int comprobanteNroIFRS = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNroIFRS, false, compIFRS.ID.Value);

                    }
                    else
                        throw new Exception("acActivoControl_AddList - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }


        #endregion

        #region Deterioro Activos

        /// <summary>
        /// Trae un saldo
        /// </summary>
        /// <param name="tipoBalance">Tipo de balance</param>
        /// <param name="concSaldo">Concepto de saldo</param>
        /// <param name="identificadorTR">Consecutivo del socumento por el cual se va a buscar el saldo</param>
        /// <returns>Retorna un listado de saldo</returns>
        public List<DTO_acActivoSaldo> acActivoControl_GetSaldoCompraActivo(string concSaldo, int identificadorTR, string tipoBalance)
        {
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoControl.DAL_acActivoControl_GetSaldoCompraActivo(concSaldo, identificadorTR, tipoBalance);
        }

        /// <summary>
        /// Agrega una lista de registros a acActivoControl
        /// </summary>
        /// <param name="documentoID">Documento que ejecuta la transaccion</param>
        /// <param name="acActivoControlList">Lista de registros</param>
        /// <returns>Retorna una lista de resultados (uno por cada registro)</returns>
        public List<DTO_TxResult> acActivoControl_Deterioro(int documentoID, string actividadFlujoID, string balanceID, string prefijoID, bool deterioroInd, string tipoMov, DTO_coDocumentoRevelacion revelacion, List<DTO_acActivoControl> acActivoControlList, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante compIFRS = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_masterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                string prefijoDoc = deterioroInd == true ? "DETERIORO" : "REVALORIZACION";
                string componente = deterioroInd == true ? this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro) : this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);

                DTO_acComponenteActivo componenteActivo = (DTO_acComponenteActivo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acComponenteActivo, componente, true, false);
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMov, true, false);

                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                DTO_acContabiliza acCont = new DTO_acContabiliza();
                Dictionary<string, string> dic = new Dictionary<string, string>();

                // Variables por defecto
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);

                #endregion

                #region Crea GlDocumentoControl IFRS

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);
                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = tc;
                glCtrl.TasaCambioDOCU.Value = tc;
                glCtrl.ComprobanteID.Value = compIFRS.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea Comprobante

                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                headerIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                headerIFRS.ComprobanteNro.Value = 0;
                headerIFRS.EmpresaID.Value = glCtrl.EmpresaID.Value;
                headerIFRS.Fecha.Value = glCtrl.FechaDoc.Value;
                headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                headerIFRS.MdaTransacc.Value = glCtrl.MonedaID.Value;
                headerIFRS.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                headerIFRS.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                headerIFRS.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                headerIFRS.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerCtraPartida = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_acActivoControl acCtrl = acActivoControlList[i];

                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Busca Componentes

                    this._dal_masterSimple.DocumentID = AppMasters.acComponenteActivo;
                    long count = this._dal_masterSimple.DAL_MasterSimple_Count(null, null, true);
                    var componentes = this._dal_masterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);

                    decimal vlrML = 0, vlrME = 0;
                    //Recorre los componentes parametrizados 
                    foreach (var item in componentes)
                    {
                        string conceptoSaldoID = ((DTO_acComponenteActivo)item).ConceptoSaldoID.Value;
                        DTO_coCuentaSaldo saldoDoc = null;

                        dic.Clear();
                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ComponenteActivoID", item.ID.Value);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);
                        if (acCont != null)
                        {
                            string cuentaID = string.Empty;
                            saldoDoc = this._moduloContabilidad.Saldo_GetByDocumento(cuentaID, conceptoSaldoID, acCtrl.ActivoID.Value.Value, balanceID);
                            if (saldoDoc != null)
                            {
                                vlrML += saldoDoc.DbOrigenLocML.Value.Value + saldoDoc.DbOrigenExtML.Value.Value + saldoDoc.CrOrigenLocML.Value.Value + saldoDoc.CrOrigenExtML.Value.Value
                                        + saldoDoc.DbSaldoIniLocML.Value.Value + saldoDoc.DbSaldoIniExtML.Value.Value + saldoDoc.CrSaldoIniLocML.Value.Value + saldoDoc.CrSaldoIniExtML.Value.Value;

                                vlrME += saldoDoc.DbOrigenLocME.Value.Value + saldoDoc.DbOrigenExtME.Value.Value + saldoDoc.CrOrigenLocME.Value.Value + saldoDoc.CrOrigenExtME.Value.Value
                                        + saldoDoc.DbSaldoIniLocME.Value.Value + saldoDoc.DbSaldoIniExtME.Value.Value + saldoDoc.CrSaldoIniLocME.Value.Value + saldoDoc.CrSaldoIniExtME.Value.Value;

                            }
                        }
                    }

                    #endregion
                    #region Crea Comprobante Deterioro
                    if (vlrML > 0 && vlrME > 0)
                    {
                        #region Agregar Footer

                        footer = new DTO_ComprobanteFooter();

                        dic.Clear();
                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ComponenteActivoID", componente);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);
                        if (acCont != null)
                        {

                            DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                            coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                            footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                            footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                            footer.CuentaID.Value = acCont.CuentaID.Value;
                            footer.ConceptoCargoID.Value = concCargoDef;
                            footer.DocumentoCOM.Value = string.Empty;
                            footer.Descriptivo.Value = prefijoDoc + " ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                            footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                            footer.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                            footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                            footer.LugarGeograficoID.Value = lgDef;
                            footer.PrefijoCOM.Value = prefijDef;
                            footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                            footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                            footer.TerceroID.Value = acCtrl.TerceroID.Value;
                            footer.vlrBaseME.Value = 0;
                            footer.vlrBaseML.Value = 0;
                            footer.vlrMdaExt.Value = deterioroInd ? (vlrME - acCtrl.CostoEXT.Value.Value) * -1 : vlrME - acCtrl.CostoEXT.Value.Value;
                            footer.vlrMdaLoc.Value = deterioroInd ? (vlrML - acCtrl.CostoLOC.Value.Value) * -1 : vlrML - acCtrl.CostoLOC.Value.Value;
                            footer.vlrMdaOtr.Value = deterioroInd ? (vlrML - acCtrl.CostoLOC.Value.Value) * -1 : vlrML - acCtrl.CostoLOC.Value.Value;

                            comprobanteIFRS.Footer.Add(footer);

                            //Crea la contraPartida y asiga el centro de costo y proyecto
                            footerCtraPartida = new DTO_ComprobanteFooter();

                            //Obtiene la cuenta a partir del Concepto Bien y Servicio
                            string operacion = string.Empty;
                            string linePresupuestal = string.Empty;

                            DTO_glBienServicioClase claseByS = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, acCont.ClaseBSID.Value, true, false);
                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coProyecto, acCtrl.ProyectoID.Value, true, false);
                            operacion = proyecto.OperacionID.Value;
                            if (string.IsNullOrEmpty(operacion))
                            {
                                DTO_coCentroCosto centroCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCentroCosto, acCtrl.CentroCostoID.Value, true, false);
                                operacion = centroCosto.OperacionID.Value;
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Co_NoOper;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(claseByS.LineaPresupuestoID.Value))
                                linePresupuestal = lineaPresDef;
                            else
                                linePresupuestal = claseByS.LineaPresupuestoID.Value;

                            DTO_CuentaValor cuenta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(claseByS.ConceptoCargoID.Value, operacion, claseByS.LineaPresupuestoID.Value, 0);
                            if (cuenta.GetType() == typeof(List<DTO_TxResult>))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                                break;
                            }

                            footerCtraPartida.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                            footerCtraPartida.CuentaID.Value = cuenta.CuentaID.Value;
                            footerCtraPartida.ConceptoCargoID.Value = claseByS.ConceptoCargoID.Value;
                            footerCtraPartida.DocumentoCOM.Value = string.Empty;
                            footerCtraPartida.Descriptivo.Value = prefijoDoc + " ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                            footerCtraPartida.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                            footerCtraPartida.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                            footerCtraPartida.LineaPresupuestoID.Value = linePresupuestal;
                            footerCtraPartida.LugarGeograficoID.Value = lgDef;
                            footerCtraPartida.PrefijoCOM.Value = prefijDef;
                            footerCtraPartida.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                            footerCtraPartida.TerceroID.Value = acCtrl.TerceroID.Value;
                            footerCtraPartida.vlrBaseME.Value = 0;
                            footerCtraPartida.vlrBaseML.Value = 0;
                            footerCtraPartida.ProyectoID.Value = acCtrl.ProyectoID.Value;
                            footerCtraPartida.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                            footerCtraPartida.vlrMdaExt.Value = deterioroInd ? (vlrME - acCtrl.CostoEXT.Value.Value) : (vlrME - acCtrl.CostoEXT.Value.Value) * -1;
                            footerCtraPartida.vlrMdaLoc.Value = deterioroInd ? (vlrML - acCtrl.CostoLOC.Value.Value) : (vlrML - acCtrl.CostoLOC.Value.Value) * -1;
                            footerCtraPartida.vlrMdaOtr.Value = deterioroInd ? (vlrML - acCtrl.CostoLOC.Value.Value) : (vlrML - acCtrl.CostoLOC.Value.Value) * -1;

                            comprobanteIFRS.Footer.Add(footerCtraPartida);
                        }
                        #endregion
                    }
                    #endregion
                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = acCtrl.ActivoID.Value.Value;
                    mov.EstadoInv.Value = acCtrl.EstadoInv.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mov.DocSoporteTER.Value = acCtrl.NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    mov.CodigoBSID.Value = acCtrl.CodigoBSID.Value;
                    mov.EmpresaID.Value = acCtrl.EmpresaID.Value;
                    mov.IdentificadorTr.Value = acCtrl.ActivoID.Value.Value;
                    mov.inReferenciaID.Value = acCtrl.inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = MvtoTipo.ID.Value;
                    mov.NumeroDoc.Value = glCtrl.NumeroDoc.Value;
                    mov.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    mov.SerialID.Value = acCtrl.SerialID.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta, false, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                    #region Guarda la Revelacion

                    revelacion.NumeroDoc.Value = glCtrl.NumeroDoc.Value;
                    revelacion.EmpresaID.Value = this.Empresa.ID.Value;
                    result = this._moduloContabilidad.DocumentoRevelacion_Add(revelacion);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }

                #region Contabiliza el comprobante

                //Contabiliza Comprobante Fiscal
                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }

                #endregion

                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_MovActivos");
                results.Add(result);

                return results;
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

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        //Obtiene ComprobanteNro para IFRS 
                        int comprobanteNroIFRS = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, glCtrl.PeriodoDoc.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNroIFRS, false, compIFRS.ID.Value);

                    }
                    else
                        throw new Exception("acActivoControl_Deterioro - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Movimientos Activos

        /// <summary>
        /// Movimientos Activos 
        /// </summary>
        /// <param name="documentoID">documentoID</param>
        /// <param name="activoID">activoID</param>
        /// <param name="actividadFlujoID">actividadFlujoID</param>
        /// <param name="prefijoID">prefijoID</param>
        /// <param name="acActivoControlList">Listado de Activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="proyectoID">proyectoID</param>
        /// <param name="centroCostoID">centroCostoID</param>
        /// <param name="locFisicaID">locFisicaID</param>
        /// <param name="responsable">responsable</param>
        /// <param name="batchProgress">indicador de progreso</param>
        /// <param name="insideAnotherTx">indica si viene de una transaccion</param>
        /// <returns>lista de resultados</returns>
        public List<DTO_TxResult> acActivoControl_MovActivos(int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto,
                                                             string proyectoID, string centroCostoID, string locFisicaID, string responsable, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobante = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante comp = null;
            DTO_coComprobante compIFRS = null;
            bool HasChangeCentroCosto = false, HasChangeProyecto = false;

            #region Inicializar Variables Locales
            if (!string.IsNullOrEmpty(centroCostoID))
                HasChangeCentroCosto = true;
            if (!string.IsNullOrEmpty(proyectoID))
                HasChangeProyecto = true;
            #endregion

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_masterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                string compDeteirioro = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro);
                string compRevalorizacion = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);

                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.ComprobanteIFRS.Value, true, false);

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                // Variables por defecto
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);
                #endregion

                #region Crea GlDocumentoControl LOCAL

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);
                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = tc;
                glCtrl.TasaCambioDOCU.Value = tc;
                glCtrl.ComprobanteID.Value = comp.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();    //Comprobante Contabilidad Local
                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = comp.ID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.FechaDoc.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;

                DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                headerIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                headerIFRS.ComprobanteNro.Value = 0;
                headerIFRS.EmpresaID.Value = glCtrl.EmpresaID.Value;
                headerIFRS.Fecha.Value = glCtrl.FechaDoc.Value;
                headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                headerIFRS.MdaTransacc.Value = glCtrl.MonedaID.Value;
                headerIFRS.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                headerIFRS.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                headerIFRS.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                headerIFRS.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerCtraPartida = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_acActivoControl acCtrl = acActivoControlList[i];

                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion

                    if (HasChangeCentroCosto || HasChangeProyecto)
                    {
                        #region Busca Componentes

                        this._dal_masterSimple.DocumentID = AppMasters.acComponenteActivo;
                        long count = this._dal_masterSimple.DAL_MasterSimple_Count(null, null, true);
                        var componentes = this._dal_masterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);


                        //Recorre los componentes parametrizados 
                        foreach (var item in componentes)
                        {

                            string conceptoSaldo = ((DTO_acComponenteActivo)item).ConceptoSaldoID.Value;
                            DTO_coCuentaSaldo saldoDoc = null;
                            decimal vlrML = 0, vlrME = 0;

                            DTO_acContabiliza acCont = new DTO_acContabiliza();
                            Dictionary<string, string> dic = new Dictionary<string, string>();

                            dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                            dic.Add("ComponenteActivoID", item.ID.Value);

                            acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);
                            if (acCont != null)
                            {
                                string cuentaID = string.Empty;
                                saldoDoc = this._moduloContabilidad.Saldo_GetByDocumento(cuentaID, conceptoSaldo, acCtrl.ActivoID.Value.Value, comp.BalanceTipoID.Value);
                                if (saldoDoc != null)
                                {
                                    vlrML = saldoDoc.DbOrigenLocML.Value.Value + saldoDoc.DbOrigenExtML.Value.Value + saldoDoc.CrOrigenLocML.Value.Value + saldoDoc.CrOrigenExtML.Value.Value
                                            + saldoDoc.DbSaldoIniLocML.Value.Value + saldoDoc.DbSaldoIniExtML.Value.Value + saldoDoc.CrSaldoIniLocML.Value.Value + saldoDoc.CrSaldoIniExtML.Value.Value;

                                    vlrME = saldoDoc.DbOrigenLocME.Value.Value + saldoDoc.DbOrigenExtME.Value.Value + saldoDoc.CrOrigenLocME.Value.Value + saldoDoc.CrOrigenExtME.Value.Value
                                            + saldoDoc.DbSaldoIniLocME.Value.Value + saldoDoc.DbSaldoIniExtME.Value.Value + saldoDoc.CrSaldoIniLocME.Value.Value + saldoDoc.CrSaldoIniExtME.Value.Value;

                                    if (vlrML > 0 && vlrME > 0)
                                    {
                                        #region Agregar Footer

                                        footer = new DTO_ComprobanteFooter();
                                        DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                                        coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                                        footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                                        footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                                        footer.CuentaID.Value = acCont.CuentaID.Value;
                                        footer.ConceptoCargoID.Value = concCargoDef;
                                        footer.DocumentoCOM.Value = string.Empty;
                                        footer.Descriptivo.Value = "MOVIMIENTO ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                                        footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                                        footer.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                        footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                                        footer.LugarGeograficoID.Value = lgDef;
                                        footer.PrefijoCOM.Value = prefijDef;
                                        footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                                        footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                                        footer.TerceroID.Value = acCtrl.TerceroID.Value;
                                        footer.vlrBaseME.Value = 0;
                                        footer.vlrBaseML.Value = 0;
                                        footer.vlrMdaExt.Value = vlrME * -1;
                                        footer.vlrMdaLoc.Value = vlrML * -1;
                                        footer.vlrMdaOtr.Value = vlrML * -1;

                                        if (item.ID.Value != compDeteirioro && item.ID.Value != compRevalorizacion)
                                            comprobante.Footer.Add(footer);

                                        comprobanteIFRS.Footer.Add(footer);

                                        //Crea la contraPartida y asiga el centro de costo y proyecto
                                        footerCtraPartida = new DTO_ComprobanteFooter();

                                        footerCtraPartida.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                                        footerCtraPartida.CuentaID.Value = acCont.CuentaID.Value;
                                        footerCtraPartida.ConceptoCargoID.Value = concCargoDef;
                                        footerCtraPartida.DocumentoCOM.Value = string.Empty;
                                        footerCtraPartida.Descriptivo.Value = "MOVIMIENTO ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                                        footerCtraPartida.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                                        footerCtraPartida.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                        footerCtraPartida.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                                        footerCtraPartida.LugarGeograficoID.Value = lgDef;
                                        footerCtraPartida.PrefijoCOM.Value = prefijDef;
                                        footerCtraPartida.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                                        footerCtraPartida.TerceroID.Value = acCtrl.TerceroID.Value;
                                        footerCtraPartida.vlrBaseME.Value = 0;
                                        footerCtraPartida.vlrBaseML.Value = 0;
                                        footerCtraPartida.vlrMdaExt.Value = vlrME;
                                        footerCtraPartida.vlrMdaLoc.Value = vlrML;
                                        footerCtraPartida.vlrMdaOtr.Value = vlrML;
                                        footerCtraPartida.CentroCostoID.Value = !string.IsNullOrEmpty(centroCostoID) ? centroCostoID : footerCtraPartida.CentroCostoID.Value;
                                        footerCtraPartida.ProyectoID.Value = !string.IsNullOrEmpty(proyectoID) ? proyectoID : footerCtraPartida.ProyectoID.Value;

                                        if (item.ID.Value != compDeteirioro && item.ID.Value != compRevalorizacion)
                                            comprobante.Footer.Add(footerCtraPartida);

                                        comprobanteIFRS.Footer.Add(footerCtraPartida);

                                        #endregion
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                    #region Actualiza acActivoControl

                    if (!string.IsNullOrEmpty(centroCostoID))
                        acCtrl.CentroCostoID.Value = centroCostoID;
                    if (!string.IsNullOrEmpty(proyectoID))
                        acCtrl.ProyectoID.Value = proyectoID;
                    if (!string.IsNullOrEmpty(locFisicaID))
                        acCtrl.LocFisicaID.Value = locFisicaID;

                    this._dal_acActivoControl.DAL_acActivoControl_Update(acCtrl, acCtrl.ActivoID.Value.Value);

                    #endregion
                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = acCtrl.ActivoID.Value.Value;
                    mov.EstadoInv.Value = acCtrl.EstadoInv.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mov.DocSoporteTER.Value = acCtrl.NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    mov.CodigoBSID.Value = acCtrl.CodigoBSID.Value;
                    mov.EmpresaID.Value = acCtrl.EmpresaID.Value;
                    mov.IdentificadorTr.Value = acCtrl.ActivoID.Value.Value;
                    mov.inReferenciaID.Value = acCtrl.inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = glCtrl.NumeroDoc.Value;
                    mov.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    mov.SerialID.Value = acCtrl.SerialID.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta, false, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }
                if (HasChangeCentroCosto || HasChangeProyecto)
                {
                    #region Contabiliza el comprobante

                    //Contabiliza Comprobante Fiscal
                    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Clear();
                        results.Add(result);
                        return results;
                    }
                    else
                    {
                        //Contabiliza Comprobante IFRS
                        result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                        if (result.Result == ResultValue.NOK)
                        {
                            results.Clear();
                            results.Add(result);
                            return results;
                        }
                    }

                    #endregion
                }
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_MovActivos");
                results.Add(result);

                return results;
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
                        if (HasChangeCentroCosto || HasChangeProyecto)
                        {
                            int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                            int comprobanteNro = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                            this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNro, false, comp.ID.Value);

                            //Obtiene ComprobanteNro para IFRS 
                            int comprobanteNroIFRS = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                            this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNroIFRS, false, compIFRS.ID.Value);
                        }
                    }
                    else
                        throw new Exception("acActivoControl_MovActivos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }


        #endregion

        #region Traslado Activos

        /// <summary>
        /// Actualiza informacion en el activo control a partir del traslado
        /// </summary>
        /// <param name="acActivoControlList">Lista de DTO_AcActivoControl</param>
        /// <param name="activoID">ActivoID</param>
        /// <returns>Lista de resultados</returns>
        public List<DTO_TxResult> TrasladoActivos(int documentoID, List<DTO_acActivoControl> acActivoControlList, int activoID, string prefijoID, string tipoMvto, DateTime fechaDocu, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();
            string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;
            DTO_glDocumentoControl glCtrl = null;
            DTO_coComprobante comp = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)base.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Variables

                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);
                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                if (comp == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_NoCompForMove;
                    results.Add(result);
                    return results;
                }
                //Por defecto
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string monedaLOC = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string asesorID = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_CodigoAsesorID);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);

                #endregion
                #region Crea GlDocumentoControl

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = fechaDocu;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = 0;
                glCtrl.TasaCambioDOCU.Value = 0;
                glCtrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.TerceroID.Value = terceroPorDefecto;
                glCtrl.DocumentoTercero.Value = null;
                glCtrl.CuentaID.Value = null;
                glCtrl.MonedaID.Value = monedaLOC;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                #endregion

                int i = 0;
                foreach (DTO_acActivoControl row in acActivoControlList)
                {
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;
                    i++;
                    rd.line = 1;
                    rd.Message = "OK";
                    result = this.acActivoControl_Update(acActivoControlList, tipoMvto, activoID);
                }
                #region Guarda el glMovimientoDeta

                for (int j = 0; j < acActivoControlList.Count; j++)
                {
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();
                    mov.ActivoID.Value = acActivoControlList[j].ActivoID.Value;
                    mov.CentroCostoID.Value = acActivoControlList[j].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoControlList[j].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoControlList[j].EmpresaID.Value;
                    mov.IdentificadorTr.Value = acActivoControlList[j].ActivoID.Value;
                    mov.inReferenciaID.Value = acActivoControlList[j].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    mov.ProyectoID.Value = acActivoControlList[j].ProyectoID.Value;
                    mov.SerialID.Value = acActivoControlList[j].SerialID.Value;
                    mov.TerceroID.Value = acActivoControlList[j].TerceroID.Value;
                    //mov.Valor1LOC.Value = acActivoControlList[j].ValorSalvamentoLOC.Value;
                    //mov.Valor2LOC.Value = acActivoControlList[j].ValorSalvamentoLOC.Value;
                    //mov.Valor1EXT.Value = acActivoControlList[j].ValorSalvamentoEXT.Value;
                    //mov.Valor2EXT.Value = acActivoControlList[j].ValorSalvamentoEXT.Value;

                    mobDeta.Add(mov);
                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }
                }
                #endregion
                #region Crea acActivoDocu

                DTO_acActivoDocu acDocu = new DTO_acActivoDocu();

                acDocu.EmpresaID.Value = acActivoControlList.First().EmpresaID.Value;
                acDocu.AsesorID.Value = asesorID;
                acDocu.DatoAdd1.Value = null;
                acDocu.DatoAdd2.Value = null;
                acDocu.DatoAdd3.Value = null;
                acDocu.DatoAdd4.Value = null;
                acDocu.DatoAdd5.Value = null;
                acDocu.DocumentoREL.Value = Convert.ToInt32(resultGLDC.Key);
                acDocu.Iva.Value = 0;
                acDocu.Valor.Value = 0;
                acDocu.LocFisicaID.Value = acActivoControlList.First().LocFisicaID.Value;
                acDocu.MvtoTipoActID.Value = tipoMvto;
                acDocu.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                acDocu.Observacion.Value = "Traslado de activos"; //TODO: Preguntar cual debe ir aca y reemplazarlo

                result = this.acActivoDocu_Add(acDocu);
                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    results.Add(result);
                    return results;
                }

                #endregion
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "TrasladoActivos");
                results.Add(result);

                return results;
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

                        glCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(docID, prefijoID);
                        glCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, glCtrl.PeriodoDoc.Value.Value, glCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                    }
                    else
                        throw new Exception("Traslado de Activos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
            return results;
        }

        #endregion

        #region Retiro Activos

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> RetiroActivos_GetComponenentes(int activoID, string tipoBalance)
        {
            try
            {
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_acRetiroActivoComponente> list = this._dal_acActivoControl.DAL_acActivoControl_GetComponentes(activoID, tipoBalance);
                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "RetiroActivos_GetComponenentes");
                throw exception;
            }
        }

        /// <summary>
        /// Listado de Componentes y sus saldos por activo
        /// </summary>
        /// <param name="activoID">identificador del activo</param>
        /// <returns></returns>
        public List<DTO_acRetiroActivoComponente> acActivoFijos_GetComponenentes(int activoID)
        {
            try
            {
                List<DTO_acRetiroActivoComponente> list = new List<DTO_acRetiroActivoComponente>();
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string balanceFuncional = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string balanceIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

                var listFuncional = this._dal_acActivoControl.DAL_acActivoControl_GetComponentes(activoID, balanceFuncional);
                var listIFRS = this._dal_acActivoControl.DAL_acActivoControl_GetComponentes(activoID, balanceIFRS);

                foreach (var item in listFuncional)
                {
                    foreach (var itemIFRS in listIFRS)
                    {
                        if (itemIFRS.ComponenteActivoID.Value == item.ComponenteActivoID.Value)
                        {
                            item.VlrSaldoIFRSML.Value = itemIFRS.VlrSaldoML.Value;
                            item.VlrSaldoIFRSME.Value = itemIFRS.VlrSaldoME.Value;
                            list.Add(item);
                        }
                        else
                        {
                            itemIFRS.VlrSaldoIFRSML.Value = itemIFRS.VlrSaldoML.Value;
                            itemIFRS.VlrSaldoIFRSME.Value = itemIFRS.VlrSaldoME.Value;
                            itemIFRS.VlrSaldoML.Value = 0;
                            itemIFRS.VlrSaldoME.Value = 0;
                            list.Add(itemIFRS);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "RetiroActivos_GetComponenentes");
                throw exception;
            }
        }

        /// <summary>
        /// Da de baja un activo
        /// </summary>
        /// <param name="documentoID">Identificador Documento</param>
        /// <param name="actividadFlujoID">Identificador Actividad</param>
        /// <param name="prefijoID">Prefijo</param>
        /// <param name="acActivoControlList">Lista activos</param>
        /// <param name="tipoMvto">Tipo Movimiento</param>
        /// <param name="batchProgress">Indicador de Progreso</param>
        /// <param name="insideAnotherTx">Indica si viene de una Transacción</param>
        /// <returns></returns>
        public List<DTO_TxResult> acActivoControl_RetiroActivos(int documentoID, string actividadFlujoID, string prefijoID, List<DTO_acActivoControl> acActivoControlList, string tipoMvto, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_Comprobante comprobante = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante comp = null;
            DTO_coComprobante compIFRS = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_masterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);


                #region Variables

                string compDeteirioro = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro);
                string compRevalorizacion = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);

                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.ComprobanteIFRS.Value, true, false);

                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                // Variables por defecto
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);

                string accionMov = documentoID == AppDocuments.RetiroActivos ? "RETIRO" : "VENTA";
                byte estadoInv = documentoID == AppDocuments.RetiroActivos ? (byte)EstadoInv.Retirado : (byte)EstadoInv.Vendido;


                #endregion
                #region Crea GlDocumentoControl LOCAL

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);
                decimal tc = this._moduloGlobal.TasaDeCambio_Get(mdaExt, today);

                glCtrl = new DTO_glDocumentoControl();

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = tc;
                glCtrl.TasaCambioDOCU.Value = tc;
                glCtrl.ComprobanteID.Value = comp.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.MonedaID.Value = mdaLoc;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();    //Comprobante Contabilidad Local
                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = comp.ID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.FechaDoc.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;

                DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                headerIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                headerIFRS.ComprobanteNro.Value = 0;
                headerIFRS.EmpresaID.Value = glCtrl.EmpresaID.Value;
                headerIFRS.Fecha.Value = glCtrl.FechaDoc.Value;
                headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                headerIFRS.MdaTransacc.Value = glCtrl.MonedaID.Value;
                headerIFRS.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                headerIFRS.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                headerIFRS.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                headerIFRS.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerCtraPartida = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;

                    DTO_acActivoControl acCtrl = acActivoControlList[i];

                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Busca Componentes

                    this._dal_masterSimple.DocumentID = AppMasters.acComponenteActivo;
                    long count = this._dal_masterSimple.DAL_MasterSimple_Count(null, null, true);
                    var componentes = this._dal_masterSimple.DAL_MasterSimple_GetPaged(count, 1, null, null, true);


                    //Recorre los componentes parametrizados 
                    foreach (var item in componentes)
                    {

                        string conceptoSaldo = ((DTO_acComponenteActivo)item).ConceptoSaldoID.Value;
                        DTO_coCuentaSaldo saldoDoc = null;
                        decimal vlrML = 0, vlrME = 0;

                        DTO_acContabiliza acCont = new DTO_acContabiliza();
                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ComponenteActivoID", item.ID.Value);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);
                        if (acCont != null)
                        {
                            string cuentaID = string.Empty;
                            saldoDoc = this._moduloContabilidad.Saldo_GetByDocumento(cuentaID, conceptoSaldo, acCtrl.ActivoID.Value.Value, comp.BalanceTipoID.Value);
                            if (saldoDoc != null)
                            {
                                vlrML = saldoDoc.DbOrigenLocML.Value.Value + saldoDoc.DbOrigenExtML.Value.Value + saldoDoc.CrOrigenLocML.Value.Value + saldoDoc.CrOrigenExtML.Value.Value
                                        + saldoDoc.DbSaldoIniLocML.Value.Value + saldoDoc.DbSaldoIniExtML.Value.Value + saldoDoc.CrSaldoIniLocML.Value.Value + saldoDoc.CrSaldoIniExtML.Value.Value;

                                vlrME = saldoDoc.DbOrigenLocME.Value.Value + saldoDoc.DbOrigenExtME.Value.Value + saldoDoc.CrOrigenLocME.Value.Value + saldoDoc.CrOrigenExtME.Value.Value
                                        + saldoDoc.DbSaldoIniLocME.Value.Value + saldoDoc.DbSaldoIniExtME.Value.Value + saldoDoc.CrSaldoIniLocME.Value.Value + saldoDoc.CrSaldoIniExtME.Value.Value;

                                if (vlrML > 0 && vlrME > 0)
                                {
                                    #region Agregar Footer

                                    footer = new DTO_ComprobanteFooter();
                                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                                    footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                                    footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                                    footer.CuentaID.Value = acCont.CuentaID.Value;
                                    footer.ConceptoCargoID.Value = concCargoDef;
                                    footer.DocumentoCOM.Value = string.Empty;
                                    footer.Descriptivo.Value = accionMov + " ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                                    footer.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                    footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                                    footer.LugarGeograficoID.Value = lgDef;
                                    footer.PrefijoCOM.Value = prefijDef;
                                    footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                                    footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                                    footer.TerceroID.Value = acCtrl.TerceroID.Value;
                                    footer.vlrBaseME.Value = 0;
                                    footer.vlrBaseML.Value = 0;
                                    footer.vlrMdaExt.Value = vlrME * -1;
                                    footer.vlrMdaLoc.Value = vlrML * -1;
                                    footer.vlrMdaOtr.Value = vlrML * -1;

                                    if (item.ID.Value != compDeteirioro && item.ID.Value != compRevalorizacion)
                                        comprobante.Footer.Add(footer);

                                    comprobanteIFRS.Footer.Add(footer);

                                    //Crea la contraPartida y asiga el centro de costo y proyecto
                                    footerCtraPartida = new DTO_ComprobanteFooter();

                                    footerCtraPartida.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                                    footerCtraPartida.CuentaID.Value = acCont.CuentaID.Value;
                                    footerCtraPartida.ConceptoCargoID.Value = concCargoDef;
                                    footerCtraPartida.DocumentoCOM.Value = string.Empty;
                                    footerCtraPartida.Descriptivo.Value = accionMov + " ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                                    footerCtraPartida.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                                    footerCtraPartida.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                    footerCtraPartida.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                                    footerCtraPartida.LugarGeograficoID.Value = lgDef;
                                    footerCtraPartida.PrefijoCOM.Value = prefijDef;
                                    footerCtraPartida.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                                    footerCtraPartida.TerceroID.Value = acCtrl.TerceroID.Value;
                                    footerCtraPartida.vlrBaseME.Value = 0;
                                    footerCtraPartida.vlrBaseML.Value = 0;
                                    footerCtraPartida.vlrMdaExt.Value = vlrME;
                                    footerCtraPartida.vlrMdaLoc.Value = vlrML;
                                    footerCtraPartida.vlrMdaOtr.Value = vlrML;
                                    footerCtraPartida.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                                    footerCtraPartida.ProyectoID.Value = acCtrl.ProyectoID.Value;

                                    if (item.ID.Value != compDeteirioro && item.ID.Value != compRevalorizacion)
                                        comprobante.Footer.Add(footerCtraPartida);

                                    comprobanteIFRS.Footer.Add(footerCtraPartida);

                                    #endregion
                                }
                            }
                        }
                    }

                    #endregion
                    #region Cambia Estado AcActivoControl

                    this._dal_acActivoControl.DAL_acActivoControl_ChangeStatus(acCtrl.ActivoID.Value.Value, estadoInv);

                    #endregion
                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = acCtrl.ActivoID.Value.Value;
                    mov.EstadoInv.Value = acCtrl.EstadoInv.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mov.DocSoporteTER.Value = acCtrl.NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    mov.CodigoBSID.Value = acCtrl.CodigoBSID.Value;
                    mov.EmpresaID.Value = acCtrl.EmpresaID.Value;
                    mov.IdentificadorTr.Value = acCtrl.ActivoID.Value.Value;
                    mov.inReferenciaID.Value = acCtrl.inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = glCtrl.NumeroDoc.Value;
                    mov.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    mov.SerialID.Value = acCtrl.SerialID.Value;
                    mov.TerceroID.Value = acCtrl.TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta, false, true);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }

                #endregion
                #region Contabiliza el comprobante

                //Contabiliza Comprobante Fiscal
                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }
                else
                {
                    //Contabiliza Comprobante IFRS
                    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Clear();
                        results.Add(result);
                        return results;
                    }
                }

                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_RetiroActivos");
                results.Add(result);

                return results;
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

                        int documentoNro = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        int comprobanteNro = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNro, false, comp.ID.Value);

                        //Obtiene ComprobanteNro para IFRS 
                        int comprobanteNroIFRS = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, documentoNro);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, comprobanteNroIFRS, false, compIFRS.ID.Value);

                    }
                    else
                        throw new Exception("acActivoControl_RetiroActivos - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }


        #endregion

        #region Contenedores

        /// <summary>
        /// Obtiene los activos hijos 
        /// </summary>
        /// <param name="activoID">activoID padre</param>
        /// <returns>listado de activos control</returns>
        public List<DTO_acActivoControl> acActivoControl_GetChildrenActivos(int activoID)
        {
            try
            {
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_acActivoControl> list = this._dal_acActivoControl.DAL_acActivoControl_GetChildrenActivos(activoID);
                return list;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "acActivoControl_GetChildrenActivos");
                throw exception;
            }
        }

        /*
        public List<DTO_TxResult> acActivoControl_TransalteContenedor(int documentoID, DTO_acActivoControl contenedor, List<DTO_acActivoControl> children, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        { 
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            List<DTO_TxResult> results = new List<DTO_TxResult>();
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            int docID = 0;

            DTO_glDocumentoControl glCtrl = null;
            DTO_glDocumentoControl glCtrlIFRS = null;
            DTO_Comprobante comprobante = null;
            DTO_Comprobante comprobanteIFRS = null;
            DTO_coComprobante comp = null;
            DTO_coComprobante compIFRS = null;

            try
            {

                foreach (var child in children)
                {
                    
                }

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string lineaPres = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string cCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string proyectoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                int numDoc = Convert.ToInt32(acActivoControlList.First().NumeroDoc.Value);
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, tipoMvto, true, false);
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, MvtoTipo.coDocumentoID.Value, true, false);
                docID = Convert.ToInt32(coDoc.DocumentoID.Value);

                comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                compIFRS = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.ComprobanteIFRS.Value, true, false);

                //Costos contrapartida
                decimal CostoLOCConPtida = 0;
                decimal CostoEXTConPtida = 0;
                decimal CostoOTRConPtida = 0;

                // Variables por defecto
                string componente = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
                string proyectoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string costoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prefijDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string periodoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.ac_Periodo);
                #endregion
                #region Crea GlDocumentoControl LOCAL

                DTO_glDocumento documento = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, false);

                glCtrl = new DTO_glDocumentoControl();
                DTO_glDocumentoControl factura = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);

                glCtrl.DocumentoID.Value = documentoID;
                glCtrl.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrl.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.Descripcion.Value = documento.Descriptivo.Value;
                glCtrl.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrl.PrefijoID.Value = prefijoID;
                glCtrl.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrl.TasaCambioCONT.Value = factura.TasaCambioCONT.Value;
                glCtrl.TasaCambioDOCU.Value = factura.TasaCambioDOCU.Value;
                glCtrl.ComprobanteID.Value = comp.ID.Value;
                glCtrl.ComprobanteIDNro.Value = 0;
                glCtrl.TerceroID.Value = factura.TerceroID.Value;
                glCtrl.DocumentoTercero.Value = factura.DocumentoTercero.Value;
                glCtrl.CuentaID.Value = mdaLoc == factura.MonedaID.Value ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                glCtrl.MonedaID.Value = factura.MonedaID.Value;
                glCtrl.ProyectoID.Value = proyectoDef;
                glCtrl.CentroCostoID.Value = costoDef;
                glCtrl.LugarGeograficoID.Value = lgDef;
                glCtrl.LineaPresupuestoID.Value = lineaPresDef;
                glCtrl.Observacion.Value = string.Empty;
                glCtrl.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrl.DocumentoNro.Value = 0;
                glCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrl.seUsuarioID.Value = this.UserId;

                if (glCtrl.CuentaID.Value == string.Empty)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_CountNoAssigned;
                    results.Add(result);
                    return results;
                }

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                glCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                string doc6ID = resultGLDC.Key;

                #endregion
                #region Crea GlDocumentoControl IFRS

                glCtrlIFRS = new DTO_glDocumentoControl();
                glCtrlIFRS.DocumentoID.Value = documentoID;
                glCtrlIFRS.Fecha.Value = acActivoControlList.First().Fecha.Value;
                glCtrlIFRS.PeriodoDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrlIFRS.PeriodoUltMov.Value = Convert.ToDateTime(periodoxDef);
                glCtrlIFRS.Descripcion.Value = documento.Descriptivo.Value;
                glCtrlIFRS.FechaDoc.Value = Convert.ToDateTime(periodoxDef);
                glCtrlIFRS.PrefijoID.Value = prefijoID;
                glCtrlIFRS.DocumentoTipo.Value = (Byte)DocumentoTipo.DocInterno;
                glCtrlIFRS.TasaCambioCONT.Value = factura.TasaCambioCONT.Value;
                glCtrlIFRS.TasaCambioDOCU.Value = factura.TasaCambioCONT.Value;
                glCtrlIFRS.ComprobanteID.Value = compIFRS.ID.Value;
                glCtrlIFRS.ComprobanteIDNro.Value = 0;
                glCtrlIFRS.TerceroID.Value = factura.TerceroID.Value;
                glCtrlIFRS.DocumentoTercero.Value = factura.DocumentoTercero.Value;
                glCtrlIFRS.CuentaID.Value = mdaLoc == factura.MonedaID.Value ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                glCtrlIFRS.MonedaID.Value = factura.MonedaID.Value;
                glCtrlIFRS.ProyectoID.Value = proyectoDef;
                glCtrlIFRS.CentroCostoID.Value = costoDef;
                glCtrlIFRS.LugarGeograficoID.Value = lgDef;
                glCtrlIFRS.LineaPresupuestoID.Value = lineaPresDef;
                glCtrlIFRS.Observacion.Value = string.Empty;
                glCtrlIFRS.Estado.Value = (Byte)EstadoDocControl.Aprobado;
                glCtrlIFRS.DocumentoNro.Value = 0;
                glCtrlIFRS.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                glCtrlIFRS.seUsuarioID.Value = this.UserId;

                if (glCtrlIFRS.CuentaID.Value == string.Empty)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Ac_CountNoAssigned;
                    results.Add(result);
                    return results;
                }

                DTO_TxResultDetail resultIFRSGLDC = this._moduloGlobal.glDocumentoControl_Add(documentoID, glCtrlIFRS, true);
                if (resultIFRSGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }

                glCtrlIFRS.NumeroDoc.Value = Convert.ToInt32(resultIFRSGLDC.Key);
                string docIFRS6ID = resultIFRSGLDC.Key;

                #endregion
                #region Crea acActivoDocu
                DTO_acActivoDocu acDocu = new DTO_acActivoDocu();

                acDocu.EmpresaID.Value = acActivoControlList.First().EmpresaID.Value;
                acDocu.DatoAdd1.Value = null;
                acDocu.DatoAdd2.Value = null;
                acDocu.DatoAdd3.Value = null;
                acDocu.DatoAdd4.Value = null;
                acDocu.DatoAdd5.Value = null;
                acDocu.DocumentoREL.Value = numDoc;
                acDocu.Iva.Value = 0;
                acDocu.Valor.Value = 0;
                acDocu.LocFisicaID.Value = acActivoControlList.First().LocFisicaID.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo
                acDocu.MvtoTipoActID.Value = tipoMvto;
                acDocu.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                acDocu.Observacion.Value = acActivoControlList.First().Descriptivo.Value;//TODO: Preguntar cual debe ir aca y reemplazarlo

                result = this.acActivoDocu_Add(acDocu);

                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    results.Add(result);
                    return results;
                }
                #endregion
                #region Crea Comprobante
                comprobante = new DTO_Comprobante();    //Comprobante Contabilidad Local
                comprobanteIFRS = new DTO_Comprobante();    //Comprobante Contabilidad IFRS
                TipoMoneda_LocExt tipoM = glCtrl.MonedaID.Value == mdaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = glCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = glCtrl.ComprobanteIDNro.Value;
                header.EmpresaID.Value = glCtrl.EmpresaID.Value;
                header.Fecha.Value = glCtrl.FechaDoc.Value;
                header.MdaOrigen.Value = (Byte)tipoM;
                header.MdaTransacc.Value = glCtrl.MonedaID.Value;
                header.NumeroDoc.Value = Convert.ToInt32(doc6ID);
                header.PeriodoID.Value = glCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = glCtrl.TasaCambioCONT.Value;
                header.TasaCambioOtr.Value = glCtrl.TasaCambioDOCU.Value;
                comprobante.Header = header;

                DTO_ComprobanteHeader headerIFRS = new DTO_ComprobanteHeader();
                headerIFRS.ComprobanteID.Value = glCtrlIFRS.ComprobanteID.Value;
                headerIFRS.ComprobanteNro.Value = glCtrlIFRS.ComprobanteIDNro.Value;
                headerIFRS.EmpresaID.Value = glCtrlIFRS.EmpresaID.Value;
                headerIFRS.Fecha.Value = glCtrlIFRS.FechaDoc.Value;
                headerIFRS.MdaOrigen.Value = (Byte)tipoM;
                headerIFRS.MdaTransacc.Value = glCtrlIFRS.MonedaID.Value;
                headerIFRS.NumeroDoc.Value = Convert.ToInt32(docIFRS6ID);
                headerIFRS.PeriodoID.Value = glCtrlIFRS.PeriodoDoc.Value;
                headerIFRS.TasaCambioBase.Value = glCtrlIFRS.TasaCambioCONT.Value;
                headerIFRS.TasaCambioOtr.Value = glCtrlIFRS.TasaCambioDOCU.Value;
                comprobanteIFRS.Header = headerIFRS;

                #endregion
                #region Carga la info del comprobante y actualiza la info
                DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                for (int i = 0; i < acActivoControlList.Count; i++)
                {
                    DTO_acActivoControl acCtrl = acActivoControlList[i];
                    acCtrl.ValorSalvamentoIFRSUS.Value = acCtrl.ValorSalvamentoIFRS.Value / factura.TasaCambioCONT.Value;

                    int percent = ((i + 1) * 100) / acActivoControlList.Count;
                    batchProgress[tupProgress] = percent;
                    #region Variables de Resultado
                    result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    result.Result = ResultValue.OK;
                    #endregion
                    #region Agregar AcActivoControl

                    rd = this.acActivoControl_Add(documentoID, acCtrl, doc6ID, true, true);
                    if (rd.Message == ResultValue.NOK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_AddDocument;
                        result.Details.Add(rd);
                        results.Add(result);
                        return results;
                    }
                    results.Add(result);
                    #endregion
                    #region Agregar Footer

                    #region Traer Cuentas

                    DTO_acContabiliza acCont = new DTO_acContabiliza();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                    dic.Add("ComponenteActivoID", componente);

                    acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                    if (acCont == null)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                        return results;
                    }
                    #endregion

                    #region Crea el detalle comprobante  LOCAL

                    footer = new DTO_ComprobanteFooter();
                    DTO_coPlanCuenta coPlanCta = new DTO_coPlanCuenta();
                    coPlanCta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    string actId = rd.Key;

                    footer.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    footer.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    footer.CuentaID.Value = acCont.CuentaID.Value;
                    footer.ConceptoCargoID.Value = concCargoDef;
                    footer.DocumentoCOM.Value = string.Empty;
                    footer.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    footer.ConceptoSaldoID.Value = coPlanCta.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = Convert.ToInt32(actId);
                    footer.LineaPresupuestoID.Value = glCtrl.LineaPresupuestoID.Value;
                    footer.LugarGeograficoID.Value = lgDef;
                    footer.PrefijoCOM.Value = prefijDef;
                    footer.ProyectoID.Value = acCtrl.ProyectoID.Value;
                    footer.TasaCambio.Value = glCtrl.TasaCambioCONT.Value;
                    footer.TerceroID.Value = glCtrl.TerceroID.Value;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrBaseML.Value = 0;
                    footer.vlrMdaExt.Value = acCtrl.CostoEXT.Value;
                    footer.vlrMdaLoc.Value = acCtrl.CostoLOC.Value;
                    footer.vlrMdaOtr.Value = acCtrl.CostoLOC.Value;

                    CostoLOCConPtida += acCtrl.CostoLOC.Value.Value;
                    CostoEXTConPtida += acCtrl.CostoEXT.Value.Value;
                    CostoOTRConPtida += acCtrl.CostoLOC.Value.Value;

                    comprobante.Footer.Add(footer);

                    #endregion

                    #region Asigna Componente de Retiro para IFRS


                    DTO_ComprobanteFooter footerIFRS = new DTO_ComprobanteFooter();
                    DTO_coPlanCuenta ctaProvIFRS = new DTO_coPlanCuenta();
                    ctaProvIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                    footerIFRS.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                    footerIFRS.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                    footerIFRS.CuentaID.Value = acCont.CuentaID.Value;
                    footerIFRS.ConceptoCargoID.Value = concCargoDef;
                    footerIFRS.DocumentoCOM.Value = string.Empty;
                    footerIFRS.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                    footerIFRS.ConceptoSaldoID.Value = ctaProvIFRS.ConceptoSaldoID.Value;
                    footerIFRS.IdentificadorTR.Value = Convert.ToInt32(actId);
                    footerIFRS.LineaPresupuestoID.Value = glCtrlIFRS.LineaPresupuestoID.Value;
                    footerIFRS.LugarGeograficoID.Value = lgDef;
                    footerIFRS.PrefijoCOM.Value = prefijDef;
                    footerIFRS.ProyectoID.Value = glCtrlIFRS.ProyectoID.Value;
                    footerIFRS.TasaCambio.Value = glCtrlIFRS.TasaCambioCONT.Value;
                    footerIFRS.TerceroID.Value = glCtrlIFRS.TerceroID.Value;
                    footerIFRS.vlrBaseME.Value = 0;
                    footerIFRS.vlrBaseML.Value = 0;
                    footerIFRS.vlrMdaExt.Value = acCtrl.CostoEXT.Value + (glCtrlIFRS.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrlIFRS.TasaCambioCONT.Value : 0);
                    footerIFRS.vlrMdaLoc.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;
                    footerIFRS.vlrMdaOtr.Value = acCtrl.CostoLOC.Value + acCtrl.ValorRetiroIFRS.Value;

                    comprobanteIFRS.Footer.Add(footerIFRS);

                    #region Costo Retiro IFRS

                    if (acCtrl.ValorRetiroIFRS.Value.Value > 0)
                    {
                        string ctaProvIFRScomp = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCostoDesmantelamiento);
                        dic.Clear();
                        dic.Add("ActivoClaseID", acCtrl.ActivoClaseID.Value);
                        dic.Add("ComponenteActivoID", ctaProvIFRScomp);

                        acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                        if (acCont == null)
                        {
                            result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                            result.Result = ResultValue.NOK;
                            results.Add(result);
                            return results;
                        }

                        DTO_ComprobanteFooter footerCostoRet = new DTO_ComprobanteFooter();
                        DTO_coPlanCuenta coPlanCtaRetIFRS = new DTO_coPlanCuenta();

                        //Obtiene la cuenta de Provisiones
                        coPlanCtaRetIFRS = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);

                        footerCostoRet.ActivoCOM.Value = acCtrl.PlaquetaID.Value;
                        footerCostoRet.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                        footerCostoRet.CuentaID.Value = coPlanCtaRetIFRS.ID.Value;
                        footerCostoRet.ConceptoCargoID.Value = concCargoDef;
                        footerCostoRet.DocumentoCOM.Value = string.Empty;
                        footerCostoRet.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - PLAQUETA - " + acCtrl.PlaquetaID.Value;
                        footerCostoRet.ConceptoSaldoID.Value = coPlanCtaRetIFRS.ConceptoSaldoID.Value;
                        footerCostoRet.IdentificadorTR.Value = Convert.ToInt32(actId);
                        footerCostoRet.LineaPresupuestoID.Value = glCtrlIFRS.LineaPresupuestoID.Value;
                        footerCostoRet.LugarGeograficoID.Value = lgDef;
                        footerCostoRet.PrefijoCOM.Value = prefijDef;
                        footerCostoRet.ProyectoID.Value = acCtrl.ProyectoID.Value;
                        footerCostoRet.TasaCambio.Value = glCtrlIFRS.TasaCambioCONT.Value;
                        footerCostoRet.TerceroID.Value = glCtrlIFRS.TerceroID.Value;
                        footerCostoRet.vlrBaseME.Value = 0;
                        footerCostoRet.vlrBaseML.Value = 0;
                        footerCostoRet.vlrMdaExt.Value = (glCtrlIFRS.TasaCambioCONT.Value > 0 ? acCtrl.ValorRetiroIFRS.Value / glCtrlIFRS.TasaCambioCONT.Value : 0) * -1;
                        footerCostoRet.vlrMdaLoc.Value = acCtrl.ValorRetiroIFRS.Value * -1;
                        footerCostoRet.vlrMdaOtr.Value = acCtrl.ValorRetiroIFRS.Value * -1;

                        comprobanteIFRS.Footer.Add(footerCostoRet);
                    }

                    #endregion

                    #endregion

                    #endregion

                    #region Guarda el glMovimientoDeta
                    List<DTO_glMovimientoDeta> mobDeta = new List<DTO_glMovimientoDeta>();
                    DTO_glMovimientoDeta mov = new DTO_glMovimientoDeta();

                    mov.ActivoID.Value = Convert.ToInt32(actId);
                    mov.EstadoInv.Value = acActivoControlList[i].EstadoInv.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mov.DocSoporteTER.Value = acActivoControlList[i].NumeroDoc.Value.ToString();
                    mov.CentroCostoID.Value = acActivoControlList[i].CentroCostoID.Value;
                    mov.CodigoBSID.Value = acActivoControlList[i].CodigoBSID.Value;
                    mov.EmpresaID.Value = acActivoControlList[i].EmpresaID.Value;
                    mov.IdentificadorTr.Value = Convert.ToInt32(actId);
                    mov.inReferenciaID.Value = acActivoControlList[i].inReferenciaID.Value;
                    mov.MvtoTipoActID.Value = tipoMvto;
                    mov.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    mov.ProyectoID.Value = acActivoControlList[i].ProyectoID.Value;
                    mov.SerialID.Value = acActivoControlList[i].SerialID.Value;
                    mov.TerceroID.Value = acActivoControlList[i].TerceroID.Value;
                    mobDeta.Add(mov);

                    result = this._moduloGlobal.glMovimientoDeta_Add(mobDeta);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.Result = ResultValue.NOK;
                        results.Add(result);
                    }

                    #endregion
                }
                #region Crea la contrapartida
                DTO_ComprobanteFooter footerConPtda = new DTO_ComprobanteFooter();
                DTO_ComprobanteFooter footerRetConPtda = new DTO_ComprobanteFooter();
                DTO_coPlanCuenta coPlanCptda = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, glCtrl.CuentaID.Value, true, false);

                string actID2 = rd.Key;

                footerConPtda.ActivoCOM.Value = null;
                footerConPtda.CentroCostoID.Value = cCosto;
                footerConPtda.CuentaID.Value = glCtrl.CuentaID.Value;
                footerConPtda.ConceptoCargoID.Value = concCargoDef;
                footerConPtda.DocumentoCOM.Value = "0";
                footerConPtda.Descriptivo.Value = "ALTA POR COMPRA DE ACTIVO - CONTRAPARTIDA";
                footerConPtda.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                footerConPtda.ConceptoSaldoID.Value = coPlanCptda.ConceptoSaldoID.Value;
                footerConPtda.IdentificadorTR.Value = Convert.ToInt32(actID2);
                footerConPtda.LineaPresupuestoID.Value = lineaPres;
                footerConPtda.LugarGeograficoID.Value = glCtrl.LugarGeograficoID.Value;
                footerConPtda.PrefijoCOM.Value = prefijoID;
                footerConPtda.ProyectoID.Value = proyectoXDef;
                footerConPtda.TasaCambio.Value = glCtrl.TasaCambioDOCU.Value;
                footerConPtda.TerceroID.Value = glCtrl.TerceroID.Value;
                footerConPtda.vlrBaseME.Value = 0;
                footerConPtda.vlrBaseML.Value = 0;
                footerConPtda.vlrMdaExt.Value = (Math.Round(CostoEXTConPtida, 2) * -1);
                footerConPtda.vlrMdaLoc.Value = (Math.Round(CostoLOCConPtida, 2) * -1);
                footerConPtda.vlrMdaOtr.Value = (Math.Round(CostoOTRConPtida, 2) * -1);

                comprobante.Footer.Add(footerConPtda);
                comprobanteIFRS.Footer.Add(footerConPtda);


                #endregion


                #region Contabiliza el comprobante

                //Contabiliza Comprobante Fiscal
                result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, glCtrl.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    results.Clear();
                    results.Add(result);
                    return results;
                }
                else
                {
                    //Contabiliza Comprobante IFRS
                    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobanteIFRS, glCtrlIFRS.PeriodoDoc.Value.Value, ModulesPrefix.ac, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        results.Clear();
                        results.Add(result);
                        return results;
                    }
                }

                #endregion
                #endregion

                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoControl_AddList");
                results.Add(result);

                return results;
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

                        glCtrl.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        glCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrl.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrl.NumeroDoc.Value.Value, glCtrl.ComprobanteIDNro.Value.Value, false);

                        //Obtiene ComprobanteNro para IFRS 
                        glCtrlIFRS.DocumentoNro.Value = Convert.ToInt32(this.GenerarDocumentoNro(docID, prefijoID));
                        glCtrlIFRS.ComprobanteIDNro.Value = this.GenerarComprobanteNro(compIFRS, glCtrl.PrefijoID.Value, comprobante.Header.PeriodoID.Value.Value, glCtrl.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(glCtrlIFRS, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(glCtrlIFRS.NumeroDoc.Value.Value, glCtrlIFRS.ComprobanteIDNro.Value.Value, false);

                    }
                    else
                        throw new Exception("acActivoControl_AddList - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        */
        #endregion

        #region acActivoGarantia

        /// <summary>
        /// Trae un activo control por segun la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        private DTO_acActivoGarantia acActivoGarantia_GetByID(int activoID)
        {
            this._dal_acActivoGarantia = (DAL_acActivoGarantia)this.GetInstance(typeof(DAL_acActivoGarantia), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_acActivoGarantia.DAL_acActivoGarantia_GetByID(activoID);   
        }

        /// <summary>
        /// Trae lista activo garantia para plantilla de importacion
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public List<DTO_acActivoGarantia> acActivoGarantia_GetForImport()
        {
            List<DTO_acActivoGarantia> result = new List<DTO_acActivoGarantia>();
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_acActivoGarantia = (DAL_acActivoGarantia)this.GetInstance(typeof(DAL_acActivoGarantia), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            List<DTO_acActivoControl> activos = this._dal_acActivoControl.DAL_acActivoControl_GetForGarantia();
            foreach (DTO_acActivoControl ac in activos)
            {
                DTO_acActivoGarantia gar = new DTO_acActivoGarantia();
                gar.ActivoID.Value = ac.ActivoID.Value;
                gar.ProyectoID.Value = ac.ProyectoID.Value;
                gar.TipoAct.Value = ac.TipoAct.Value;
                gar.inReferenciaID.Value = ac.inReferenciaID.Value;
                gar.Descriptivo.Value = ac.Descriptivo.Value;
                gar.Serial.Value = ac.SerialID.Value;
                gar.ProveedorID.Value = string.Empty;
                gar.FechaCompra.Value = null;
                DTO_acActivoGarantia exist = this.acActivoGarantia_GetByID(ac.ActivoID.Value.Value);
                if (exist != null)
                {
                    gar.GarantiaRef.Value = exist.GarantiaRef.Value;
                    gar.FechaINICliente.Value = exist.FechaINICliente.Value;
                    gar.FechaFINCliente.Value = exist.FechaFINCliente.Value;
                    gar.FechaINIProveedor.Value = exist.FechaINIProveedor.Value;
                    gar.FechaFINProveedor.Value = exist.FechaFINProveedor.Value;
                    gar.FechaFINEmpresa.Value = exist.FechaFINEmpresa.Value;
                }
                result.Add(gar);
            }
            
            return result;
        }

        /// <summary>
        /// Agrega una lista de activo garantia
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_TxResult acActivoGarantia_Add(int documentoID, List<DTO_acActivoGarantia> acGar, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                this._dal_acActivoGarantia = (DAL_acActivoGarantia)this.GetInstance(typeof(DAL_acActivoGarantia), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentoID);
                batchProgress[tupProgress] = 1;
                decimal porcTotal = 0;
                decimal porcParte = 100 / acGar.Count > 0 ? acGar.Count : 1;
                foreach (var g in acGar)
                {
                    bool exist = this._dal_acActivoGarantia.DAL_acActivoGarantia_Exist(g.ActivoID.Value);
                    if (exist)
                        this._dal_acActivoGarantia.DAL_acActivoGarantia_Update(g);
                    else
                    {
                        var existActivo = this._dal_acActivoControl.DAL_acActivoControl_GetByID(g.ActivoID.Value.Value);
                        if (existActivo != null)
                            this._dal_acActivoGarantia.DAL_acActivoGarantia_Add(g);
                        else
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = 1;
                            rd.Message = "Activo no existe: " + g.ActivoID.Value.ToString();
                            result.Details.Add(rd);
                        }
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                }
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "acActivoGarantia_Add");
                result.Result = ResultValue.NOK;
                result.ResultMessage = ex.Message;
                return result;
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
        /// Funcion que carga el DTO de Acitivos Saldos para mostrar el valor por componente
        /// </summary>
        /// <param name="libro">Libro que se desea Mostrar</param>
        /// <param name="Periodo">Periodo que se desea mostrar</param>
        /// <param name="plaqueta">Filtra una plaqueta especifica para mostrar</param>
        /// <param name="serial"> Filtra un serial especifico para mostrar</param>
        /// <param name="referencia"> Filtar una referencia especifica para mostrar</param>
        /// <param name="clase">Filtra una clase especifica para mostrar</param>
        /// <param name="tipo">Filtra un tipo especifico a mostrar </param>
        /// <param name="grupos">Filtra un grupo especifico para mostrar</param>
        /// <param name="propietario">Filtra un propietario especifico para mostrar</param>
        /// <param name="tipoLibro">Muestra un libro espeficico</param>
        /// <param name="isMonedaLoc">Tipo Moneda en que se desea ver el reporte</param>
        /// <param name="ConceptoSaldo">Lista de los conceptos saldo de Activos Fijos</param>
        /// <returns>DTO_Activos</returns>
        public List<DTO_ActivosTotales> ReportesActivos_Saldos(string libro, DateTime Periodo, string plaqueta, string serial, string referencia, string clase, string tipo,
            string grupos, string propietario, bool isMonedaLoc)
        {
            try
            {
                #region Variables y Asiganciones a valores

                //Variables
                List<DTO_ActivosTotales> saldo = new List<DTO_ActivosTotales>();
                DTO_ActivosTotales saldos = new DTO_ActivosTotales();
                List<string> conceptoSaldo = new List<string>();

                this._dal_ReportesActivos = (DAL_ReportesActivos)this.GetInstance(typeof(DAL_ReportesActivos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_coBalanceTipo tipoLibro = (DTO_coBalanceTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coBalanceTipo, libro, true, false);


                //Cargo el Diccionario de los componentes
                string compSaldo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
                string compDepre = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDepreciacion);
                string compDeterioro = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro);
                string compRevaloriza = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);
                string compDesmantelamiento = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCostoDesmantelamiento);

                conceptoSaldo.Add(compSaldo);
                conceptoSaldo.Add(compDepre);
                conceptoSaldo.Add(compDeterioro);
                conceptoSaldo.Add(compRevaloriza);
                conceptoSaldo.Add(compDesmantelamiento);

                #endregion

                saldos.DetallesSaldosComponentes = this._dal_ReportesActivos.DAL_ReportesActivos_Saldos(libro, Periodo, plaqueta, serial, referencia, clase, tipo, grupos,
                    propietario, tipoLibro.Descriptivo.Value, isMonedaLoc, conceptoSaldo);
                List<string> distinct = (from c in saldos.DetallesSaldosComponentes select c.ActivoClaseID.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ActivosTotales componenteSaldo = new DTO_ActivosTotales();
                    componenteSaldo.DetallesSaldosComponentes = new List<DTO_acCostos>();

                    componenteSaldo.DetallesSaldosComponentes = saldos.DetallesSaldosComponentes.Where(x => x.ActivoClaseID.Value == item).ToList();
                    saldo.Add(componenteSaldo);
                }

                return saldo;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesActivos_Saldos");
                throw ex;
            }
        }

        public List<DTO_ActivosTotales> ReportesActivos_ComparacionLibros(int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis, string proyecto)
        {
            List<DTO_ActivosTotales> comparativo = new List<DTO_ActivosTotales>();
            DTO_ActivosTotales comparacion = new DTO_ActivosTotales();
            this._dal_ReportesActivos = (DAL_ReportesActivos)this.GetInstance(typeof(DAL_ReportesActivos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            //Diccionario que carga los componentes que estan en glcontrol
            Dictionary<int, string> componente = new Dictionary<int, string>();
            string compSaldo = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
            string compDepre = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDepreciacion);
            string compDeterioro = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro);
            string compRevaloriza = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);

            componente.Add(1, compSaldo);
            componente.Add(2, compDepre);
            componente.Add(3, compDeterioro);
            componente.Add(4, compRevaloriza);

            //Diccionario que carga los libros que estan en control
            Dictionary<int, string> libros = new Dictionary<int, string>();
            string libroFUNC = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            string libroIFRS = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

            libros.Add(1, libroFUNC);
            libros.Add(2, libroIFRS);

            comparacion.DetallesComparacion = this._dal_ReportesActivos.DAL_ReportesActivos_ComparacionLibros(libros, año, mes, clase, tipo, grupo, centroCost, logFis, proyecto);
            List<string> distinct = (from c in comparacion.DetallesComparacion select c.ActivoClaseID.Value).Distinct().ToList();

            foreach (var item in distinct)
            {
                DTO_ActivosTotales compTotal = new DTO_ActivosTotales();
                compTotal.DetallesComparacion = new List<DTO_acComparacionLibros>();

                compTotal.DetallesComparacion = compTotal.DetallesComparacion.Where(x => x.ActivoClaseID.Value == item).ToList();
                comparativo.Add(compTotal);
            }

            return comparativo;
        }

        #endregion

        #region Equipos

        public List<DTO_ActivosTotales> ReportesActivos_EquiposArrendados(DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento)
        {
            try
            {
                this._dal_ReportesActivos = (DAL_ReportesActivos)this.GetInstance(typeof(DAL_ReportesActivos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ActivosTotales> dtosGeneral = new List<DTO_ActivosTotales>();
                DTO_ActivosTotales deta = new DTO_ActivosTotales();
                deta.DetallesEquipos = new List<DTO_ReportEquiposArrendados>();
                deta.DetallesEquipos = this._dal_ReportesActivos.DAL_ReportesActivos_EquiposArrendados(Periodo,Estado,Tercero,Plaqueta,Serial,TipoRef,Rompimiento);
                if (Rompimiento == "Tercero-TipoRef")
                {
                    List<string> distinct = (from c in deta.DetallesEquipos select c.TerceroID.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_ActivosTotales aux = new DTO_ActivosTotales();
                        aux.DetallesEquipos = new List<DTO_ReportEquiposArrendados>();

                        aux.DetallesEquipos = deta.DetallesEquipos.Where(x => x.TerceroID.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                if (Rompimiento == "TipoRef-Tercero")
                {
                    List<string> distinct = (from c in deta.DetallesEquipos select c.TipoRef.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_ActivosTotales aux = new DTO_ActivosTotales();
                        aux.DetallesEquipos = new List<DTO_ReportEquiposArrendados>();

                        aux.DetallesEquipos = deta.DetallesEquipos.Where(x => x.TipoRef.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ac_EquiposArrendados");
                return null;
            }
        }

        public List<DTO_ActivosTotales> ReportesActivos_ImportacionesTemporales(DateTime Periodo, string Plaqueta, string Serial, string TipoRef, string Rompimiento)
        {
            try
            {
                this._dal_ReportesActivos = (DAL_ReportesActivos)this.GetInstance(typeof(DAL_ReportesActivos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ActivosTotales> dtosGeneral = new List<DTO_ActivosTotales>();
                DTO_ActivosTotales deta = new DTO_ActivosTotales();
                deta.DetallesImportacionesT = new List<DTO_ReportImportacionesTemporales>();
                deta.DetallesImportacionesT = this._dal_ReportesActivos.DAL_ReportesActivos_ImportacionesTemporales(Periodo, Plaqueta, Serial, TipoRef);
                if (Rompimiento == "MesVencimiento-TipoRef")
                {
                    List<DateTime> distinct = (from c in deta.DetallesImportacionesT select c.FechaVencimiento.Value.Value).Distinct().ToList();
                    foreach (DateTime item in distinct)
                    {
                        DTO_ActivosTotales aux = new DTO_ActivosTotales();
                        aux.DetallesImportacionesT = new List<DTO_ReportImportacionesTemporales>();

                        aux.DetallesImportacionesT = deta.DetallesImportacionesT.Where(x => x.FechaVencimiento.Value.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                if (Rompimiento == "TipoRef-MesVencimiento")
                {
                    List<string> distinct = (from c in deta.DetallesImportacionesT select c.TipoRef.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_ActivosTotales aux = new DTO_ActivosTotales();
                        aux.DetallesImportacionesT = new List<DTO_ReportImportacionesTemporales>();

                        aux.DetallesImportacionesT = deta.DetallesImportacionesT.Where(x => x.TipoRef.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Ac_EquiposArrendados");
                return null;
            }
        }
        #endregion

        #region Consultas

        /// <summary>
        /// Funcion que obtiene la información del activo de acuerdo al filtro
        /// </summary>
        /// <param name="plaqueta">Filtro de PLaquetaID</param>
        /// <param name="serial">Filtro de SerialID</param>
        /// <param name="referencia">Filtro de Referencia</param>
        /// <param name="clase">Filtro de ActivoClaseID</param>
        /// <param name="tipo">Filtro de ActivoTipoID</param>
        /// <param name="grupo">Filtro de ActivoGrupoID</param>
        /// <param name="locFisica">Filtro de LocfisicaIF</param>
        /// <param name="contenedor">Bool que dice si trae o no los activos contenidos</param>
        /// <param name="responsable">Filtro de Responsable - TerceroID</param>
        /// <returns>Lista de detalles para la consulta</returns>
        public List<DTO_acQueryActivoControl> ActivoGetByParameter(string plaqueta, string serial, string referencia, string clase, string tipo, string grupo, string locFisica, bool contenedor, string responsable)
        {
            #region Varibles

            List<DTO_acQueryActivoControl> data = new List<DTO_acQueryActivoControl>();
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            try
            {
                data = this._dal_acActivoControl.DAL_acActivoControl_GetByParameter(plaqueta, serial, referencia, clase, tipo, grupo, locFisica, contenedor, responsable);
                return data;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ActivoGetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        /// <summary>
        /// Funcionque obrtinee los saldos correspondientes al activo
        /// </summary>
        /// <param name="identificadorTr">Activo ID del activo</param>
        /// <param name="balanceTipoID">Tipo de Libro (Funcional o IFRS)</param>
        /// <param name="año">Año que se va a consultar</param>
        /// <param name="mes">Mes que se quiere consultar</param>
        /// <returns>Lista de saldos por activo</returns>
        public List<DTO_acActivoQuerySaldos> ActivoControl_GetSaldosByMesYLibro(int identificadorTr, string balanceTipoID, DateTime periodo)
        {
            #region Variables

            List<DTO_acActivoQuerySaldos> saldos = new List<DTO_acActivoQuerySaldos>();
            this._dal_acActivoControl = (DAL_acActivoControl)this.GetInstance(typeof(DAL_acActivoControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            List<string> componentesColtrl = new List<string>();
            string compDepreciacion = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDepreciacion);
            componentesColtrl.Add(compDepreciacion);
            string compCosto = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);
            componentesColtrl.Add(compCosto);
            string compDeterioro = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteDeterioro);
            componentesColtrl.Add(compDeterioro);
            string compValorizacion = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteRevalorizacion);
            componentesColtrl.Add(compValorizacion);

            #endregion
            try
            {
                #region Asignacion de detalles por componente

                foreach (string componenteID in componentesColtrl)
                {
                    List<DTO_acActivoQuerySaldos> data = this._dal_acActivoControl.DAL_acActivoControl_GetSaldosByMesYLibro(identificadorTr, balanceTipoID, periodo, componenteID);
                    saldos.AddRange(data);
                }
                #endregion
                return saldos;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ActivoControl_GetSaldosByMesYLibro");
                throw exception;
            }
        }

        #endregion

        #region Reversiones

        /// <summary>
        /// Revierte una Documento de Actrivos Fijos
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="numeroDoc">numero documento</param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult ActivosFijos_RevertirAltas(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls,
            ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (consecutivoPos.HasValue)
                consecutivoPos += 1;
            else
            {
                consecutivoPos = 0;
                ctrls = new List<DTO_glDocumentoControl>();
                coComps = new List<DTO_coComprobante>();
            }

            #endregion
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_acActivoDocu = (DAL_acActivoDocu)base.GetInstance(typeof(DAL_acActivoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Variables

                //Variables de CxP
                DTO_glDocumentoControl ctrlOld = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                DTO_acActivoDocu activoDocu = this._dal_acActivoDocu.DAL_acActivoDocu_Get(numeroDoc);

                List<DTO_acActivoControl> lActivos = this._dal_acActivoControl.DAL_acActivoControl_GetByDocument(ctrlOld.NumeroDoc.Value.Value);

                // Variables por defecto
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string componente = this.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_ComponenteCosto100);

                foreach (var act in lActivos)
                {
                    #region Traer Cuentas

                    DTO_acContabiliza acCont = new DTO_acContabiliza();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    dic.Add("ActivoClaseID", act.ActivoClaseID.Value);
                    dic.Add("ComponenteActivoID", componente);

                    acCont = (DTO_acContabiliza)this.GetMasterComplexDTO(AppMasters.acContabiliza, dic, true);

                    if (acCont == null)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Ac_Count;
                        result.Result = ResultValue.NOK;
                        return result;
                    }

                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, acCont.CuentaID.Value, true, false);


                    #endregion

                    bool isML = mdaLoc == ctrlOld.MonedaID.Value ? true : false;
                    DTO_coCuentaSaldo saldoDTO = this._moduloContabilidad.Saldo_GetByDocumento(cta.ID.Value, cta.ConceptoSaldoID.Value, numeroDoc, string.Empty);

                    //Saldo ML
                    decimal saldoML = saldoDTO.DbOrigenLocML.Value.Value + saldoDTO.DbOrigenExtML.Value.Value + saldoDTO.CrOrigenLocML.Value.Value + saldoDTO.CrOrigenExtML.Value.Value
                        + saldoDTO.DbSaldoIniLocML.Value.Value + saldoDTO.DbSaldoIniExtML.Value.Value + saldoDTO.CrSaldoIniLocML.Value.Value + saldoDTO.CrSaldoIniExtML.Value.Value;
                    saldoML = Math.Round(saldoML, 2);
                    //Saldo ME
                    decimal saldoME = saldoDTO.DbOrigenLocME.Value.Value + saldoDTO.DbOrigenExtME.Value.Value + saldoDTO.CrOrigenLocME.Value.Value + saldoDTO.CrOrigenExtME.Value.Value
                         + saldoDTO.DbSaldoIniLocME.Value.Value + saldoDTO.DbSaldoIniExtME.Value.Value + saldoDTO.CrSaldoIniLocME.Value.Value + saldoDTO.CrSaldoIniExtME.Value.Value;
                    saldoME = Math.Round(saldoME, 2);

                    decimal saldo = isML ? saldoML : saldoME;
                #endregion

                    #region Validaciones

                    // Valida que el saldo de la cta sea igual al valor de la CxP en la mda origen
                    if (activoDocu.Valor.Value.Value * -1 != saldo)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidSaldoDoc;
                        return result;
                    }

                    #endregion
                }

                #region Revierte el documento
                result = this._moduloGlobal.glDocumentoControl_Revertir(documentID, numeroDoc, consecutivoPos, ref ctrls, ref coComps, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ActivosFijos_Revertir");

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
                            if (coCompAnula != null)
                                ctrlAnula.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAnula, ctrlAnula.PrefijoID.Value, ctrlAnula.PeriodoDoc.Value.Value, ctrlAnula.DocumentoNro.Value.Value);

                            ctrlAnula.DocumentoNro.Value = this.GenerarDocumentoNro(ctrlAnula.DocumentoID.Value.Value, ctrlAnula.PrefijoID.Value);
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
    }
}