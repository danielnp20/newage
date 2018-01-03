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
using SentenceTransformer;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloOpConjuntas : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_Billing _dal_Billing = null;
        private DAL_CashCall _dal_CashCall = null;
        private DAL_ocDetalleLegalizacion _dal_ocDetalleLegalizacion = null;
        private DAL_ReportesOperacionesConjuntas _dal_ReportesOperacionesConjuntas = null;
        private DAL_MasterComplex _dal_MasterComplex = null;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloGlobal _moduloGlobal = null;

        #endregion

        #endregion    
    
        /// <summary>
        /// Constructor Modulo Activos Fijos
        /// </summary>
        /// <param name="conn"></param>
        public ModuloOpConjuntas(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Billing

        /// <summary>
        /// Proceso para hacer la particion del Billing
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult Billing_Particion(int documentID, string actividadFlujoID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isMultimoneda = false;
            EstadoAjuste estadoCtrl = EstadoAjuste.NoData;
            DTO_coComprobante coComp = null;
            DTO_glDocumentoControl docCtrlML = null;
            DTO_glDocumentoControl docCtrlME = null;
            try
            {
                #region 1. Variables

                //Modulos y dals
                this._dal_Billing = (DAL_Billing)this.GetInstance(typeof(DAL_Billing), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string areafuncionalID = this.GetAreaFuncionalByUser();
                string prefijoDoc = this.GetPrefijoByDocumento(documentID);
                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.oc, AppControl.co_Periodo));
                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                isMultimoneda = this.Multimoneda();

                //Info de los comprobantes
                DTO_Comprobante compML = new DTO_Comprobante();
                DTO_Comprobante compME = new DTO_Comprobante();

                //Variables de los comprobantes
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteParticion);
                string compGrossID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobranteParticionGross);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string libroGross = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceGross);
                string libroPreliminar = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalancePreliminar);

                #endregion
                #region 2. Revisa si ya se hizo un ajuste previo y carga la informacion de los comprobantes
         
                estadoCtrl = this._moduloContabilidad.HasDocument(documentID, periodo, libroFunc);
                if (estadoCtrl == EstadoAjuste.Aprobado)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Oc_BillingAprobado + "&&" + periodo.Date.ToShortDateString();
                    return result;
                }
                else if (estadoCtrl == EstadoAjuste.Preliminar)
                {
                    this._dal_ocDetalleLegalizacion = (DAL_ocDetalleLegalizacion)this.GetInstance(typeof(DAL_ocDetalleLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_ocDetalleLegalizacion.DAL_ocDetalleLegalizacion_Delete(periodo);
                }
                else if (estadoCtrl == EstadoAjuste.NoData)
                    coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compID, true, false);

                batchProgress[tupProgress] = 10;

                #endregion
                #region 3. Trae el detalle de los comprobantes

                //Moneda local
                List<DTO_ComprobanteFooter> compsML = new List<DTO_ComprobanteFooter>();
                object dataML = this._dal_Billing.DAL_Billing_GetBilling(periodo, mdaLoc, true);
                if(dataML.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)dataML;
                    return result;
                }
                else
                    compsML = (List<DTO_ComprobanteFooter>)dataML;

                if (compsML.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_CompNoResults;
                    return result;
                }

                batchProgress[tupProgress] = 20;

                //Moneda extranjera
                List<DTO_ComprobanteFooter> compsME = new List<DTO_ComprobanteFooter>();
                if (isMultimoneda)
                {
                    object dataME = this._dal_Billing.DAL_Billing_GetBilling(periodo, mdaExt, false);
                    if (dataML.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)dataME;
                        return result;
                    }
                    else
                        compsME = (List<DTO_ComprobanteFooter>)dataME;

                    if (compsME.Count == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_CompNoResults;
                        return result;
                    }
                }

                batchProgress[tupProgress] = 30;
                #endregion
                #region 4. Carga la informacion de los comprobantes (Header)

                #region Info general

                //Libro Func ML
                compML.Header.LibroID.Value = libroPreliminar;
                compML.Header.ComprobanteID.Value = compID;
                compML.Header.MdaOrigen.Value = (Byte)TipoMoneda.Local;
                compML.Header.MdaTransacc.Value = mdaLoc;
                compML.Header.PeriodoID.Value = periodo;
                compML.Header.TasaCambioBase.Value = 0;
                compML.Header.TasaCambioOtr.Value = 0;

                if (isMultimoneda)
                {
                    //Libro Func ME
                    compML.Header.LibroID.Value = libroPreliminar;
                    compML.Header.ComprobanteID.Value = compID;
                    compML.Header.MdaOrigen.Value = (Byte)TipoMoneda.Foreign;
                    compML.Header.MdaTransacc.Value = mdaExt;
                    compML.Header.PeriodoID.Value = periodo;
                    compML.Header.TasaCambioBase.Value = 0;
                    compML.Header.TasaCambioOtr.Value = 0;
                }

                #endregion
                #region Parametros por estado
                if (estadoCtrl == EstadoAjuste.Preliminar)
                {
                    #region Preliminar

                    //Trae los documentos
                    docCtrlML = this._moduloGlobal.glDocumentoControl_GetByBilling(periodo, compID, mdaLoc);
                    if (isMultimoneda)
                        docCtrlME = this._moduloGlobal.glDocumentoControl_GetByBilling(periodo, compID, mdaExt);

                    //Borrar Auxiliar
                    this._moduloContabilidad.BorrarAuxiliar(periodo, docCtrlML.ComprobanteID.Value, docCtrlML.ComprobanteIDNro.Value.Value);
                    if (isMultimoneda)
                        this._moduloContabilidad.BorrarAuxiliar(periodo, docCtrlME.ComprobanteID.Value, docCtrlME.ComprobanteIDNro.Value.Value);

                    //Borrar info del balance preliminar
                    this._moduloContabilidad.BorrarSaldosXLibro(false, periodo, libroPreliminar);

                    //Func ML
                    compML.Header.ComprobanteNro.Value = docCtrlML.ComprobanteIDNro.Value;
                    compML.Header.Fecha.Value = docCtrlML.FechaDoc.Value;
                    compML.Header.NumeroDoc.Value = docCtrlML.NumeroDoc.Value;

                    if (isMultimoneda)
                    {
                        //Func ME
                        compME.Header.ComprobanteNro.Value = docCtrlME.ComprobanteIDNro.Value;
                        compME.Header.Fecha.Value = docCtrlME.FechaDoc.Value;
                        compME.Header.NumeroDoc.Value = docCtrlME.NumeroDoc.Value;
                    }
                    #endregion
                }
                else
                {
                    #region No Data
                    //Header Moneda Local
                    compML.Header.ComprobanteNro.Value = 0;
                    compML.Header.Fecha.Value = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    compML.Header.NumeroDoc.Value = 0;

                    if (isMultimoneda)
                    {
                        //Header Moneda extranjera
                        compME.Header.ComprobanteNro.Value = 0;
                        compME.Header.Fecha.Value = compML.Header.Fecha.Value;
                        compME.Header.NumeroDoc.Value = 0;
                    }
                    #endregion
                }
                #endregion

                compML.Footer = new List<DTO_ComprobanteFooter>();
                if (isMultimoneda)
                    compME.Footer = new List<DTO_ComprobanteFooter>();

                batchProgress[tupProgress] = 40;

                #endregion
                #region 5. Agrega los registros a glDocumentoControl
                if (estadoCtrl == EstadoAjuste.NoData)
                {
                    #region Func ML

                    //Campos Principales
                    docCtrlML = new DTO_glDocumentoControl();
                    docCtrlML.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrlML.DocumentoID.Value = documentID;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrlML.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrlML.Fecha.Value = DateTime.Now;
                    docCtrlML.PeriodoDoc.Value = periodo;
                    docCtrlML.PeriodoUltMov.Value = periodo;
                    docCtrlML.AreaFuncionalID.Value = areafuncionalID;
                    docCtrlML.PrefijoID.Value = prefijoDoc;
                    docCtrlML.DocumentoNro.Value = 0;
                    docCtrlML.MonedaID.Value = mdaLoc;
                    docCtrlML.TasaCambioDOCU.Value = 0;
                    docCtrlML.TasaCambioCONT.Value = 0;
                    docCtrlML.ComprobanteID.Value = compID;
                    docCtrlML.ComprobanteIDNro.Value = 0;
                    docCtrlML.FechaDoc.Value = compML.Header.Fecha.Value;
                    docCtrlML.Descripcion.Value = "CONT. BILLING ML";
                    docCtrlML.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                    DTO_TxResultDetail rdML = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrlML, true);
                    if (rdML.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rdML);

                        return result;
                    }

                    docCtrlML.NumeroDoc.Value = Convert.ToInt32(rdML.Key);
                    compML.Header.NumeroDoc.Value = docCtrlML.NumeroDoc.Value;

                    #endregion
                    if (isMultimoneda)
                    {
                        #region Func ME
                        //Campos Principales
                        docCtrlME = new DTO_glDocumentoControl();
                        docCtrlME.EmpresaID.Value = this.Empresa.ID.Value;
                        docCtrlME.DocumentoID.Value = documentID;
                        //dtoDC.NumeroDoc.Value IDENTITY
                        docCtrlME.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        docCtrlME.Fecha.Value = DateTime.Now;
                        docCtrlME.PeriodoDoc.Value = periodo;
                        docCtrlME.PeriodoUltMov.Value = periodo;
                        docCtrlME.AreaFuncionalID.Value = areafuncionalID;
                        docCtrlME.PrefijoID.Value = prefijoDoc;
                        docCtrlME.DocumentoNro.Value = 0;
                        docCtrlME.MonedaID.Value = mdaExt;
                        docCtrlME.TasaCambioDOCU.Value = 0;
                        docCtrlME.TasaCambioCONT.Value = 0;
                        docCtrlME.ComprobanteID.Value = compID;
                        docCtrlME.ComprobanteIDNro.Value = 0;
                        docCtrlME.FechaDoc.Value = compME.Header.Fecha.Value;
                        docCtrlME.Descripcion.Value = "CONT. BILLING ME";
                        docCtrlME.Estado.Value = Convert.ToByte(EstadoDocControl.ParaAprobacion);

                        DTO_TxResultDetail rdME = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrlME, true);
                        if (rdME.Message != ResultValue.OK.ToString())
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = "NOK";
                            result.Details.Add(rdME);

                            return result;
                        }

                        docCtrlME.NumeroDoc.Value = Convert.ToInt32(rdME.Key);
                        compME.Header.NumeroDoc.Value = docCtrlME.NumeroDoc.Value;

                        #endregion
                    }
                }

                batchProgress[tupProgress] = 50;

                #endregion
                #region 6. Contabiliza los comprobantes

                #region Libro func ML

                compML.Footer.AddRange(compsML);
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, compML, periodo, ModulesPrefix.oc, 0, false);

                if (result.Result == ResultValue.NOK)
                    return result;

                batchProgress[tupProgress] = 60;
                #endregion
                if (isMultimoneda)
                {
                    #region ME

                    compME.Footer.AddRange(compsME);
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, compME, periodo, ModulesPrefix.oc, 0, false);

                    if (result.Result == ResultValue.NOK)
                        return result;

                    batchProgress[tupProgress] = 80;
                    #endregion
                }
                #endregion

                batchProgress[tupProgress] = 90;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Billing_Particion");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    #region Commit
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (estadoCtrl == EstadoAjuste.NoData)
                        {
                            //ML
                            docCtrlML.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlML.PrefijoID.Value);
                            docCtrlML.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, docCtrlML.PrefijoID.Value, docCtrlML.PeriodoDoc.Value.Value, docCtrlML.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrlML, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(docCtrlML.NumeroDoc.Value.Value, docCtrlML.ComprobanteIDNro.Value.Value, false);

                            if (isMultimoneda)
                            {
                                //ME
                                docCtrlME.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlME.PrefijoID.Value);
                                docCtrlME.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, docCtrlME.PrefijoID.Value, docCtrlME.PeriodoDoc.Value.Value, docCtrlME.DocumentoNro.Value.Value);

                                this._moduloGlobal.ActualizaConsecutivos(docCtrlME, true, true, false);
                                this._moduloContabilidad.ActualizaComprobanteNro(docCtrlME.NumeroDoc.Value.Value, docCtrlME.ComprobanteIDNro.Value.Value, false);
                            }
                        }
                    }
                    else
                        throw new Exception("Billing_Particion - Los consecutivos deben ser generados por la transaccion padre");
                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();

                batchProgress[tupProgress] = 100;

            }
        }

        /// <summary>
        /// Procesa el billing y generas los comprobantes Gross
        /// </summary>
        /// <param name="actividadFlujoID">Identificador de la actividad de flujo</param>
        /// <param name="periodo">Periodo del billing</param>
        /// <returns>Retirna al resultado de la operación</returns>
        public DTO_TxResult ProcesarBilling(int documentID, string actividadFlujoID, DateTime periodo, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isMultimoneda = false;
            DTO_coComprobante coCompGross = null;
            DTO_glDocumentoControl docCtrlGrossML = null;
            DTO_glDocumentoControl docCtrlGrossME = null;
            try
            {
                #region 1. Variables
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string compID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteParticion);
                string compGrossID = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobranteParticionGross);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                string libroGross = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceGross);
                isMultimoneda = this.Multimoneda();

                DTO_Comprobante compGrossML = new DTO_Comprobante();
                DTO_Comprobante compGrossME = new DTO_Comprobante();

                List<DTO_ComprobanteAprobacion> compsFunc = new List<DTO_ComprobanteAprobacion>();

                DTO_glDocumentoControl docCtrlML = null;
                DTO_glDocumentoControl docCtrlME = null;
                docCtrlGrossML = new DTO_glDocumentoControl();
                docCtrlGrossME = new DTO_glDocumentoControl();
                coCompGross = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, compGrossID, true, false);

                #endregion
                #region 2. Aprueba los comprobantes funcionales
                #region Func ML
                docCtrlML = this._moduloGlobal.glDocumentoControl_GetByBilling(periodo, compID, mdaLoc);

                DTO_ComprobanteAprobacion aprobML = new DTO_ComprobanteAprobacion();
                aprobML.ComprobanteID.Value = docCtrlML.ComprobanteID.Value;
                aprobML.ComprobanteNro.Value = docCtrlML.ComprobanteIDNro.Value;
                aprobML.PeriodoID.Value = periodo;
                aprobML.NumeroDoc.Value = docCtrlML.NumeroDoc.Value;
                aprobML.DocumentoID.Value = AppProcess.ParticionBilling;
                aprobML.Observacion.Value = string.Empty;
                aprobML.Aprobado.Value = true;

                compsFunc.Add(aprobML);
                #endregion
                if (isMultimoneda)
                {
                    #region Func ME
                    docCtrlME = this._moduloGlobal.glDocumentoControl_GetByBilling(periodo, compID, mdaLoc);

                    DTO_ComprobanteAprobacion aprobME = new DTO_ComprobanteAprobacion();
                    aprobME.ComprobanteID.Value = docCtrlME.ComprobanteID.Value;
                    aprobME.ComprobanteNro.Value = docCtrlME.ComprobanteIDNro.Value;
                    aprobME.PeriodoID.Value = periodo;
                    aprobME.NumeroDoc.Value = docCtrlME.NumeroDoc.Value;
                    aprobME.DocumentoID.Value = AppProcess.ParticionBilling;
                    aprobME.Observacion.Value = string.Empty;
                    aprobME.Aprobado.Value = true;

                    compsFunc.Add(aprobME);
                    #endregion
                }

                batchProgress[tupProgress] = 30;

                //Procesa el balance preliminar
                List<DTO_TxResult> results = this._moduloContabilidad.Proceso_ProcesarBalancePreliminar(AppProcess.ParticionBilling, actividadFlujoID, compsFunc, periodo, libroFunc, new Dictionary<Tuple<int, int>, int>(), true);
                List<DTO_TxResult> resultsNOK = results.Where(x => x.Result == ResultValue.NOK).ToList();
                if (resultsNOK.Count > 0)
                {
                    result = resultsNOK.First();
                    return result;
                }

                batchProgress[tupProgress] = 50;


                #endregion
                
                #region 3. Gross ML

                #region Carga la info del documento
                //Campos Principales
                docCtrlGrossML.EmpresaID.Value = this.Empresa.ID.Value;
                docCtrlGrossML.DocumentoID.Value = docCtrlML.DocumentoID.Value;
                //dtoDC.NumeroDoc.Value IDENTITY
                docCtrlGrossML.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                docCtrlGrossML.Fecha.Value = DateTime.Now;
                docCtrlGrossML.PeriodoDoc.Value = periodo;
                docCtrlGrossML.PeriodoUltMov.Value = periodo;
                docCtrlGrossML.AreaFuncionalID.Value = docCtrlML.AreaFuncionalID.Value;
                docCtrlGrossML.PrefijoID.Value = docCtrlML.PrefijoID.Value;
                docCtrlGrossML.DocumentoNro.Value = 0;
                docCtrlGrossML.MonedaID.Value = mdaLoc;
                docCtrlGrossML.TasaCambioDOCU.Value = 0;
                docCtrlGrossML.TasaCambioCONT.Value = 0;
                docCtrlGrossML.ComprobanteID.Value = compGrossID;
                docCtrlGrossML.ComprobanteIDNro.Value = 0;
                docCtrlGrossML.FechaDoc.Value = compGrossML.Header.Fecha.Value;
                docCtrlGrossML.Descripcion.Value = "CONT. GROSS BILLING ML";
                docCtrlGrossML.Estado.Value = Convert.ToByte(EstadoDocControl.Aprobado);

                DTO_TxResultDetail rdGrossML = this._moduloGlobal.glDocumentoControl_Add(docCtrlML.DocumentoID.Value.Value, docCtrlGrossML, true);
                if (rdGrossML.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(rdGrossML);

                    return result;
                }

                docCtrlGrossML.NumeroDoc.Value = Convert.ToInt32(rdGrossML.Key);

                #endregion
                #region Carga y contabiliza el comprobante

                #region Header

                //Libro Gross ML
                compGrossML.Header.LibroID.Value = libroGross;
                compGrossML.Header.ComprobanteID.Value = compGrossID;
                compGrossML.Header.MdaOrigen.Value = (Byte)TipoMoneda.Local;
                compGrossML.Header.MdaTransacc.Value = mdaLoc;
                compGrossML.Header.PeriodoID.Value = periodo;
                compGrossML.Header.TasaCambioBase.Value = 0;
                compGrossML.Header.TasaCambioOtr.Value = 0;
                compGrossML.Header.ComprobanteNro.Value = 0;
                compGrossML.Header.Fecha.Value = docCtrlML.FechaDoc.Value;
                compGrossML.Header.NumeroDoc.Value = docCtrlGrossML.NumeroDoc.Value;

                #endregion
                #region Footer

                DTO_Comprobante compML = this._moduloContabilidad.Comprobante_GetAll(docCtrlML.NumeroDoc.Value.Value, false, periodo,
                    docCtrlML.ComprobanteID.Value, docCtrlML.ComprobanteIDNro.Value.Value);

                List<DTO_ComprobanteFooter> footerGrossML = this.CambiarSignoComprobante(compML.Footer);
                compGrossML.Footer = footerGrossML;
                #endregion

                result = this._moduloContabilidad.ContabilizarComprobante(docCtrlML.DocumentoID.Value.Value, compGrossML, periodo, ModulesPrefix.oc, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion

                #endregion
                if (isMultimoneda)
                {
                    #region 4. Gross ME

                    #region Carga la info del documento
                    //Campos Principales
                    docCtrlGrossME.EmpresaID.Value = this.Empresa.ID.Value;
                    docCtrlGrossME.DocumentoID.Value = docCtrlME.DocumentoID.Value;
                    //dtoDC.NumeroDoc.Value IDENTITY
                    docCtrlGrossME.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    docCtrlGrossME.Fecha.Value = DateTime.Now;
                    docCtrlGrossME.PeriodoDoc.Value = periodo;
                    docCtrlGrossME.PeriodoUltMov.Value = periodo;
                    docCtrlGrossME.AreaFuncionalID.Value = docCtrlME.AreaFuncionalID.Value;
                    docCtrlGrossME.PrefijoID.Value = docCtrlME.PrefijoID.Value;
                    docCtrlGrossME.DocumentoNro.Value = 0;
                    docCtrlGrossME.MonedaID.Value = mdaLoc;
                    docCtrlGrossME.TasaCambioDOCU.Value = 0;
                    docCtrlGrossME.TasaCambioCONT.Value = 0;
                    docCtrlGrossME.ComprobanteID.Value = compGrossID;
                    docCtrlGrossME.ComprobanteIDNro.Value = 0;
                    docCtrlGrossME.FechaDoc.Value = compGrossME.Header.Fecha.Value;
                    docCtrlGrossME.Descripcion.Value = "CONT. GROSS BILLING ME";
                    docCtrlGrossME.Estado.Value = Convert.ToByte(EstadoDocControl.Aprobado);

                    DTO_TxResultDetail rdGrossME = this._moduloGlobal.glDocumentoControl_Add(docCtrlME.DocumentoID.Value.Value, docCtrlGrossME, true);
                    if (rdGrossME.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rdGrossME);

                        return result;
                    }

                    docCtrlGrossME.NumeroDoc.Value = Convert.ToInt32(rdGrossME.Key);

                    #endregion
                    #region Carga y contabiliza el comprobante

                    #region Header

                    //Libro Gross ME
                    compGrossME.Header.LibroID.Value = libroGross;
                    compGrossME.Header.ComprobanteID.Value = compGrossID;
                    compGrossME.Header.MdaOrigen.Value = (Byte)TipoMoneda.Local;
                    compGrossME.Header.MdaTransacc.Value = mdaLoc;
                    compGrossME.Header.PeriodoID.Value = periodo;
                    compGrossME.Header.TasaCambioBase.Value = 0;
                    compGrossME.Header.TasaCambioOtr.Value = 0;
                    compGrossME.Header.ComprobanteNro.Value = 0;
                    compGrossME.Header.Fecha.Value = docCtrlME.FechaDoc.Value;
                    compGrossME.Header.NumeroDoc.Value = docCtrlGrossME.NumeroDoc.Value;

                    #endregion
                    #region Footer

                    DTO_Comprobante compME = this._moduloContabilidad.Comprobante_GetAll(docCtrlME.NumeroDoc.Value.Value, false, periodo,
                        docCtrlME.ComprobanteID.Value, docCtrlME.ComprobanteIDNro.Value.Value);

                    List<DTO_ComprobanteFooter> footerGrossME = this.CambiarSignoComprobante(compME.Footer);
                    compGrossME.Footer = footerGrossME;
                    #endregion

                    result = this._moduloContabilidad.ContabilizarComprobante(docCtrlME.DocumentoID.Value.Value, compGrossME, periodo, ModulesPrefix.oc, 0, false);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    #endregion

                    #endregion
                }
                batchProgress[tupProgress] = 90;

                return result;
            }
            catch (Exception ex)
            {
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Billing_Particion");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    #region Commit
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        //ML
                        docCtrlGrossML.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlGrossML.PrefijoID.Value);
                        docCtrlGrossML.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompGross, docCtrlGrossML.PrefijoID.Value, docCtrlGrossML.PeriodoDoc.Value.Value, docCtrlGrossML.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(docCtrlGrossML, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(docCtrlGrossML.NumeroDoc.Value.Value, docCtrlGrossML.ComprobanteIDNro.Value.Value, false);

                        if (isMultimoneda)
                        {
                            //ME
                            docCtrlGrossME.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrlGrossME.PrefijoID.Value);
                            docCtrlGrossME.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompGross, docCtrlGrossME.PrefijoID.Value, docCtrlGrossME.PeriodoDoc.Value.Value, docCtrlGrossME.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(docCtrlGrossME, true, true, false);
                            this._moduloContabilidad.ActualizaComprobanteNro(docCtrlGrossME.NumeroDoc.Value.Value, docCtrlGrossME.ComprobanteIDNro.Value.Value, false);
                        }
                    }
                    else
                        throw new Exception("Billing_Particion - Los consecutivos deben ser generados por la transaccion padre");
                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();

                batchProgress[tupProgress] = 100;
            }
        }

        #endregion

        #region CashCall

        /// <summary>
        /// Genera el proceso de cash call
        /// </summary>
        /// <param name="documentID">Documento que ejecuta el proceso</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CashCall_Procesar(int documentID, string actividadFlujoID, DateTime periodoID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_CashCall = (DAL_CashCall)this.GetInstance(typeof(DAL_CashCall), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Genera el proceso

                //Moneda local
                List<DTO_ComprobanteFooter> compsML = new List<DTO_ComprobanteFooter>();
                result = this._dal_CashCall.DAL_CashCall_Procesar(periodoID);


                #endregion

                batchProgress[tupProgress] = 90;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CashCall_Procesar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    #region Commit
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                    }
                    else
                        throw new Exception("Billing_Particion - Los consecutivos deben ser generados por la transaccion padre");
                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();

            }
        }

        #endregion

        #region Distribucion

        /// <summary>
        /// Distribuye entre los socios de acuerdo a los porcentajes dados
        /// </summary>
        /// <param name="periodo">PeriodoID</param>
        /// <param name="errorInd">indicador de error</param>
        /// <param name="errorDesc">descripcion de error</param>
        public DTO_TxResult Proceso_ocDetalleLegalizacion_Distribucion(DateTime periodo)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                int errorInd = 0;
                string errorDesc = string.Empty; 

                this._dal_ocDetalleLegalizacion = (DAL_ocDetalleLegalizacion)this.GetInstance(typeof(DAL_ocDetalleLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                _dal_ocDetalleLegalizacion.DAL_ocDetalleLegalizacion_Distribucion(periodo, out  errorInd, out errorDesc);

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Proceso_ocDetalleLegalizacion_Distribucion");
                throw exception;
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Obtiene de acuerdo a un filtro la info de Operaciones de detalle mensual
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="filter">objeto filtro</param>
        /// <param name="tipoInforme">Tipo de Informe</param>
        /// <param name="proType">Tipo de Proyecto</param>
        /// <param name="tipoMda">Tipo de Moneda</param>
        /// <param name="mdaOrigen">Tipo de Moneda Origen</param>
        /// <returns>Lista de informe  mensual</returns>
        public List<DTO_QueryInformeMensualCierre> ocDetalleLegalizacion_GetInfoMensual(int documentID, DTO_QueryInformeMensualCierre filter, byte tipoInforme, ProyectoTipo proType, TipoMoneda_LocExt tipoMda, TipoMoneda mdaOrigen)
        {
            try
            {
                this._dal_ocDetalleLegalizacion = (DAL_ocDetalleLegalizacion)base.GetInstance(typeof(DAL_ocDetalleLegalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_QueryInformeMensualCierre> detalleFinalList = new List<DTO_QueryInformeMensualCierre>();
                List<string> distinctRomp1 = new List<string>();
                List<string> distinctRomp2 = new List<string>();
                List<string> distinctRomp3 = new List<string>();
                List<string> distinctRomp4 = new List<string>();

                List<DTO_QueryInformeMensualCierre> listDetalleLegalizacion = this._dal_ocDetalleLegalizacion.DAL_ocDetalleLegalizacion_GetInfoMensual(filter, tipoInforme, proType, tipoMda, mdaOrigen);

                #region Asigna valores validados con el ProyectoTipo
                if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                {
                    this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (DTO_QueryInformeMensualCierre cierre in listDetalleLegalizacion)
                    {
                        #region Trae el RecursoID(cambia el que viene en la lista(plLineaPresupuesto.RecursoID))
                        #region Filtro para la tabla plActividadLineaPresupuestal
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "LineaPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = cierre.LineaPresupuestoID.Value,
                        });
                        consulta.Filtros = filtros;
                        #endregion

                        this._dal_MasterComplex.DocumentID = AppMasters.plActividadLineaPresupuestal;
                        long count = _dal_MasterComplex.DAL_MasterComplex_Count(consulta, true);
                        List<DTO_MasterComplex> masterComplex = _dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, consulta, true).ToList();

                        if (masterComplex.Count > 0)
                        {
                            DTO_plActividadLineaPresupuestal recursoCapex = masterComplex.Cast<DTO_plActividadLineaPresupuestal>().First();
                            cierre.RecursoID.Value = recursoCapex.RecursoID.Value;
                        }
                        #endregion
                        #region Trae el Grupo (cambia el que viene en la lista(coActividad.ActividadID))
                        DTO_plRecurso recurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, cierre.RecursoID.Value, true, false);
                        if (recurso != null)
                            cierre.Grupo.Value = recurso.RecursoGrupoID.Value;
                        #endregion
                        #region Valida el Presupuesto Inicial
                        if (cierre.PeriodoID.Value.Value.Month != 1)
                            cierre.SaldoInicial.Value = 0;
                        #endregion
                    }
                }
                else
                {
                    #region El Grupo es igual a la actividad del Proyecto
                    foreach (DTO_QueryInformeMensualCierre cierre in listDetalleLegalizacion)
                    {
                        cierre.Grupo.Value = cierre.ActividadID.Value;
                        #region Valida el Presupuesto Inicial
                        if (cierre.PeriodoID.Value.Value.Month != 1)
                            cierre.SaldoInicial.Value = 0;
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #region Realiza 1º Rompimiento(Por Socio)
                #region Obtiene IDs no duplicados
                distinctRomp1 = (from c in listDetalleLegalizacion select c.SocioID.Value).Distinct().ToList(); 
                #endregion
                foreach (string IDRomp1 in distinctRomp1.Where(x => x != string.Empty))
                {
                    DTO_QueryInformeMensualCierre dtoRomp1 = new DTO_QueryInformeMensualCierre(true);
                    List<DTO_QueryInformeMensualCierre>  detalleNivel2List = new List<DTO_QueryInformeMensualCierre>();   
                    #region Asigna Datos Iniciales 1º Rompimiento
                    DTO_ocSocio dtoSocio = (DTO_ocSocio)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ocSocio, IDRomp1, true, false);
                    dtoRomp1.SocioID.Value = dtoSocio.ID.Value;
                    dtoRomp1.Descriptivo.Value = dtoSocio.Descriptivo.Value;
                    dtoRomp1.DetalleNivel1.AddRange(listDetalleLegalizacion.Where(x => x.SocioID.Value == IDRomp1)); 
                    #endregion
                    #region Realiza 2º Rompimiento(Por Actividad/Grupo/Proyecto)
                    #region Obtiene IDs no duplicados
                    if (proType == ProyectoTipo.Opex)
                        distinctRomp2 = (from c in dtoRomp1.DetalleNivel1 select c.ActividadID.Value).Distinct().ToList();
                    else if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                        distinctRomp2 = (from c in dtoRomp1.DetalleNivel1 select c.Grupo.Value).Distinct().ToList();
                    else
                        distinctRomp2 = (from c in dtoRomp1.DetalleNivel1 select c.ProyectoID.Value).Distinct().ToList();
                    #endregion
                    foreach (string IDRomp2 in distinctRomp2.Where(x => x != string.Empty))
                    {
                        DTO_QueryInformeMensualCierre dtoRomp2 = new DTO_QueryInformeMensualCierre(true);
                        List<DTO_QueryInformeMensualCierre>  detalleNivel3List = new List<DTO_QueryInformeMensualCierre>();
                        #region Asigna datos Iniciales 2º Rompimiento
                        if (proType == ProyectoTipo.Opex)
                        {
                            DTO_coActividad dtoActiv = (DTO_coActividad)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, IDRomp2, true, false);
                            dtoRomp2.ActividadID.Value = dtoActiv.ID.Value;
                            dtoRomp2.Descriptivo.Value = dtoActiv.Descriptivo.Value;
                            dtoRomp2.DetalleNivel2.AddRange(dtoRomp1.DetalleNivel1.Where(x => x.ActividadID.Value == IDRomp2));
                        }
                        else if (proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                        {
                            DTO_MasterBasic dtoRecursoGrupo = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecursoGrupo, IDRomp2, true, false);
                            dtoRomp2.Grupo.Value = dtoRecursoGrupo.ID.Value;
                            dtoRomp2.Descriptivo.Value = dtoRecursoGrupo.Descriptivo.Value;
                            dtoRomp2.DetalleNivel2.AddRange(dtoRomp1.DetalleNivel1.Where(x => x.Grupo.Value == IDRomp2));
                        }
                        else
                        {
                            DTO_coProyecto dtoProyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, IDRomp2, true, false);
                            dtoRomp2.ProyectoID.Value = dtoProyecto.ID.Value;
                            dtoRomp2.Descriptivo.Value = dtoProyecto.Descriptivo.Value;
                            dtoRomp2.DetalleNivel2.AddRange(dtoRomp1.DetalleNivel1.Where(x => x.ProyectoID.Value == IDRomp2));
                        }

                        #endregion
                        #region Realiza 3º Rompimiento(Por Recurso/CentroCosto)
                        #region Obtiene IDs no duplicados
                        if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                            distinctRomp3 = (from c in dtoRomp2.DetalleNivel2 select c.RecursoID.Value).Distinct().ToList();
                        else
                            distinctRomp3 = (from c in dtoRomp2.DetalleNivel2 select c.CentroCostoID.Value).Distinct().ToList();
                        #endregion
                        foreach (string IDRomp3 in distinctRomp3.Where(x => x != string.Empty))
                        {
                            DTO_QueryInformeMensualCierre dtoRomp3 = new DTO_QueryInformeMensualCierre(true);
                            #region Asigna datos Iniciales 3º Rompimiento
                            if (proType == ProyectoTipo.Opex || proType == ProyectoTipo.Capex || proType == ProyectoTipo.Inversion)
                            {
                                DTO_plRecurso dtoRecurso = (DTO_plRecurso)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plRecurso, IDRomp3, true, false);
                                dtoRomp3.RecursoID.Value = dtoRecurso.ID.Value;
                                dtoRomp3.Descriptivo.Value = dtoRecurso.Descriptivo.Value;
                                dtoRomp3.DetalleNivel3.AddRange(dtoRomp2.DetalleNivel2.Where(x => x.RecursoID.Value == IDRomp3));
                            }
                            else
                            {
                                DTO_coCentroCosto dtoCentroCto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, IDRomp3, true, false);
                                dtoRomp3.CentroCostoID.Value = dtoCentroCto.ID.Value;
                                dtoRomp3.Descriptivo.Value = dtoCentroCto.Descriptivo.Value;
                                dtoRomp3.DetalleNivel3.AddRange(dtoRomp2.DetalleNivel2.Where(x => x.CentroCostoID.Value == IDRomp3));
                            }
                            #endregion
                            #region Realiza el 4º Rompimiento(Por LineaPresupuesto)
                            List<DTO_QueryInformeMensualCierre> detalleNivel4List = new List<DTO_QueryInformeMensualCierre>();
                            #region Obtiene IDs no duplicados
                            distinctRomp4 = (from c in dtoRomp3.DetalleNivel3 select c.LineaPresupuestoID.Value).Distinct().ToList(); 
                            #endregion
                           
                            foreach (string IDRomp4 in distinctRomp4.Where(x => x != string.Empty))
                            {
                                DTO_QueryInformeMensualCierre dtoRomp4 = new DTO_QueryInformeMensualCierre(true);                              
                                foreach (DTO_QueryInformeMensualCierre detRomp4 in dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4))
                                {
                                    #region Asigna datos y valores detalle 4º Rompimiento
                                    DTO_plLineaPresupuesto dtoLineaPresup = (DTO_plLineaPresupuesto)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, IDRomp4, true, false);
                                    dtoRomp4.Descriptivo.Value = dtoLineaPresup.Descriptivo.Value;
                                    dtoRomp4.RecursoID.Value = detRomp4.RecursoID.Value;
                                    dtoRomp4.ProyectoID.Value = detRomp4.ProyectoID.Value;
                                    dtoRomp4.CentroCostoID.Value = detRomp4.CentroCostoID.Value;
                                    dtoRomp4.LineaPresupuestoID.Value = detRomp4.LineaPresupuestoID.Value;
                                    dtoRomp4.SaldoInicial.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoInicial.Value.Value);
                                    dtoRomp4.SaldoEnero.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoEnero.Value.Value);
                                    dtoRomp4.SaldoFebrero.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoFebrero.Value.Value);
                                    dtoRomp4.SaldoMarzo.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoMarzo.Value.Value);
                                    dtoRomp4.SaldoAbril.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoAbril.Value.Value);
                                    dtoRomp4.SaldoMayo.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoMayo.Value.Value);
                                    dtoRomp4.SaldoJunio.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoJunio.Value.Value);
                                    dtoRomp4.SaldoJulio.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoJulio.Value.Value);
                                    dtoRomp4.SaldoAgosto.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoAgosto.Value.Value);
                                    dtoRomp4.SaldoSeptiembre.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoSeptiembre.Value.Value);
                                    dtoRomp4.SaldoOctubre.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoOctubre.Value.Value);
                                    dtoRomp4.SaldoNoviembre.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoNoviembre.Value.Value);
                                    dtoRomp4.SaldoDiciembre.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoDiciembre.Value.Value);
                                    dtoRomp4.SaldoFinal.Value = dtoRomp3.DetalleNivel3.Where(x => x.LineaPresupuestoID.Value == IDRomp4).Sum(x => x.SaldoFinal.Value.Value);
                                    detalleNivel4List.Add(dtoRomp4);
                                    #endregion
                                }

                            }//Romp 4º
                            #endregion
                            #region Asigna valores y detalle 3º Rompimiento
                            dtoRomp3.DetalleNivel3 = detalleNivel4List;
                            dtoRomp3.SaldoInicial.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoInicial.Value.Value);
                            dtoRomp3.SaldoEnero.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoEnero.Value.Value);
                            dtoRomp3.SaldoFebrero.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoFebrero.Value.Value);
                            dtoRomp3.SaldoMarzo.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoMarzo.Value.Value);
                            dtoRomp3.SaldoAbril.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoAbril.Value.Value);
                            dtoRomp3.SaldoMayo.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoMayo.Value.Value);
                            dtoRomp3.SaldoJunio.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoJunio.Value.Value);
                            dtoRomp3.SaldoJulio.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoJulio.Value.Value);
                            dtoRomp3.SaldoAgosto.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoAgosto.Value.Value);
                            dtoRomp3.SaldoSeptiembre.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoSeptiembre.Value.Value);
                            dtoRomp3.SaldoOctubre.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoOctubre.Value.Value);
                            dtoRomp3.SaldoNoviembre.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoNoviembre.Value.Value);
                            dtoRomp3.SaldoDiciembre.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoDiciembre.Value.Value);
                            dtoRomp3.SaldoFinal.Value += dtoRomp3.DetalleNivel3.Sum(x => x.SaldoFinal.Value.Value);
                            detalleNivel3List.Add(dtoRomp3); 
                            #endregion
                        }//Romp 3º
                        #endregion
                        #region Asigna valores y detalle 2º Rompimiento
                        dtoRomp2.DetalleNivel2 = detalleNivel3List;
                        dtoRomp2.SaldoInicial.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoInicial.Value.Value);
                        dtoRomp2.SaldoEnero.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoEnero.Value.Value);
                        dtoRomp2.SaldoFebrero.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoFebrero.Value.Value);
                        dtoRomp2.SaldoMarzo.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoMarzo.Value.Value);
                        dtoRomp2.SaldoAbril.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoAbril.Value.Value);
                        dtoRomp2.SaldoMayo.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoMayo.Value.Value);
                        dtoRomp2.SaldoJunio.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoJunio.Value.Value);
                        dtoRomp2.SaldoJulio.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoJulio.Value.Value);
                        dtoRomp2.SaldoAgosto.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoAgosto.Value.Value);
                        dtoRomp2.SaldoSeptiembre.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoSeptiembre.Value.Value);
                        dtoRomp2.SaldoOctubre.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoOctubre.Value.Value);
                        dtoRomp2.SaldoNoviembre.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoNoviembre.Value.Value);
                        dtoRomp2.SaldoDiciembre.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoDiciembre.Value.Value);
                        dtoRomp2.SaldoFinal.Value += dtoRomp2.DetalleNivel2.Sum(x => x.SaldoFinal.Value.Value);
                        detalleNivel2List.Add(dtoRomp2); 
                        #endregion
                    }//Romp 2º 
                    #endregion 
                    #region Asigna valores y Detalle 1º Rompimiento
                    dtoRomp1.DetalleNivel1 = detalleNivel2List;
                    dtoRomp1.SaldoInicial.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoInicial.Value.Value);
                    dtoRomp1.SaldoEnero.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoEnero.Value.Value);
                    dtoRomp1.SaldoFebrero.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoFebrero.Value.Value);
                    dtoRomp1.SaldoMarzo.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoMarzo.Value.Value);
                    dtoRomp1.SaldoAbril.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoAbril.Value.Value);
                    dtoRomp1.SaldoMayo.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoMayo.Value.Value);
                    dtoRomp1.SaldoJunio.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoJunio.Value.Value);
                    dtoRomp1.SaldoJulio.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoJulio.Value.Value);
                    dtoRomp1.SaldoAgosto.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoAgosto.Value.Value);
                    dtoRomp1.SaldoSeptiembre.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoSeptiembre.Value.Value);
                    dtoRomp1.SaldoOctubre.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoOctubre.Value.Value);
                    dtoRomp1.SaldoNoviembre.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoNoviembre.Value.Value);
                    dtoRomp1.SaldoDiciembre.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoDiciembre.Value.Value);
                    dtoRomp1.SaldoFinal.Value += dtoRomp1.DetalleNivel1.Sum(x => x.SaldoFinal.Value.Value);
                    detalleFinalList.Add(dtoRomp1);
                    #endregion
                }//Romp 1º
                #endregion

                return detalleFinalList;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plCierreLegalizacion_GetInfoMensual");
                return null;
            }
        }

        #endregion

        #region Reportes

        /// <summary>
        /// Funcion que se encarga de traer el cierre de OC
        /// </summary>
        /// <param name="Periodo">Periodo de Consulta</param>
        /// <returns>Tabla con resultados</returns>
        public DataTable ReportesOperacionesConjuntas_Legalizaciones(DateTime Periodo)
        {
            try
            {
                this._dal_ReportesOperacionesConjuntas = (DAL_ReportesOperacionesConjuntas)this.GetInstance(typeof(DAL_ReportesOperacionesConjuntas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                return this._dal_ReportesOperacionesConjuntas.DAL_ReportesOperacionesConjuntas_Legalizaciones(Periodo);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesOperacionesConjuntas");
                throw exception;
            }
        }

        #endregion
    }
}
