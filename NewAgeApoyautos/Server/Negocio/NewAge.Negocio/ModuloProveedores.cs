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
using System.Security;
using NewAge.DTO.Reportes;
using SentenceTransformer;

namespace NewAge.Negocio
{
    public class ModuloProveedores : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_prCierres _dal_prCierres = null;
        private DAL_prCierreMesCostos _dal_prCierreMesCostos = null; 
        private DAL_prDetalleDocu _dal_prDetalleDocu = null;
        private DAL_prSolicitudCargos _dal_prSolicitudCargos = null;
        private DAL_prSolicitudDocu _dal_prSolicitudDocu = null;
        private DAL_prOrdenCompraDocu _dal_prOrdenCompraDocu = null;
        private DAL_prContratoDocu _dal_prContratoDocu = null;
        private DAL_prOrdenCompraCotiza _dal_prOrdenCompraCotiza = null;
        private DAL_prSaldosDocu _dal_prSaldosDocu = null;
        private DAL_prRecibidoDocu _dal_prRecibidoDocu = null;
        private DAL_prConvenio _dal_prConvenio = null;
        private DAL_prContratoPlanPago dal_prContratoPlanPago = null;
        private DAL_prDetalleCargos _dal_prDetalleCargos = null;
        private DAL_prConvenioSolicitudDocu _dal_prConvenioSolicitudDocu = null;
        private DAL_prConvenioConsumoDirecto _dal_prConvenioConsumoDirecto = null;
        private DAL_ReportesProveedores _dal_ReportesProveedores = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_MasterHierarchy _dal_MasterHierarchy = null;
        private DAL_MasterSimple _dal_MasterSimple = null;
        private DAL_glDocumentoAprueba _dal_glDocumentoAprueba = null;
        private DAL_prContratoPolizas _dal_prContratoPolizas = null;
        private DAL_prSolicitudDirectaDocu _dal_prSolicitudDirectaDocu = null;
        private DAL_plPlaneacion_Proveedores _dal_plPlaneacionProveedor = null;
        private DAL_pyProyectoMvto _dal_pyProyectoMvto = null;

        #endregion
        #region Modulos

        private ModuloGlobal _moduloGlobal = null;
        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloActivosFijos _moduloActivosFijos = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloInventarios _moduloInventarios = null;
        private ModuloCuentasXPagar _moduloCxP = null;
        private ModuloPlaneacion _moduloPlaneacion = null;
        private ModuloProyectos _moduloProyectos = null;

        #endregion

        #endregion

        /// <summary>
        /// Constructor Modulo Activos Fijos
        /// </summary>
        /// <param name="conn"></param>
        public ModuloProveedores(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region COM

        /// <summary>
        /// Actualiza un registro de plSobreEjecucion
        /// </summary>
        /// <param name="deta">Cierre</param>
        /// <param name="dia">Dia de cierre</param>
        /// <returns></returns>
        public void prCierreMesCostos_Add(DTO_prCierreMesCostos deta)
        {
            this._dal_prCierreMesCostos = (DAL_prCierreMesCostos)this.GetInstance(typeof(DAL_prCierreMesCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prCierreMesCostos.DAL_prCierreMesCostos_Add(deta);
        }

        /// <summary>
        /// Obtiene una lista de cierres
        /// </summary>
        /// <param name="periodo">periodo de consulta</param>
        /// <param name="codigoBSID">codigo Bien servicio</param>
        /// <param name="referenciaID">referencia</param>
        /// <param name="numeroDocOC">numero Doc Orden Compra</param>
        /// <returns>lista de cierres</returns>
        public List<DTO_prCierreMesCostos> prCierreMesCostos_GetByParameter(DateTime periodo, string codigoBSID, string referenciaID, int? numeroDocOC)
        {
            try
            {
                this._dal_prCierreMesCostos = (DAL_prCierreMesCostos)base.GetInstance(typeof(DAL_prCierreMesCostos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_prCierreMesCostos> cierresCostos = this._dal_prCierreMesCostos.DAL_prCierreMesCostos_GetByParameter(periodo, codigoBSID, referenciaID, numeroDocOC);
                return cierresCostos;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "plPresupuestoPxQDeta_GetByNumeroDoc");
                return null;
            }
        }


        #endregion

        #region Cierres

        #region Funciones privadas

        /// <summary>
        /// Realiza el proceso de cierre diario
        /// </summary>
        private DTO_TxResult CerrarDia(DateTime periodo, DateTime fechaCierre, decimal tc, string keyControl)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
                this._moduloGlobal._mySqlConnectionTx = base._mySqlConnectionTx;
                this._dal_prCierres.MySqlConnectionTx = base._mySqlConnectionTx;

                result = this._dal_prCierres.DAL_prCierreDia_Procesar(periodo, fechaCierre, tc);
                if(result.Result == ResultValue.OK)
                {
                    // Actualiza el dia de cierre en glControl
                    DTO_glControl diaCierreControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                    diaCierreControl.Data.Value = fechaCierre.Day.ToString();
                    this._moduloGlobal.glControl_Update(diaCierreControl);

                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proveedores_CerrarDia");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                    base._mySqlConnectionTx.Commit();
                else
                    base._mySqlConnectionTx.Rollback();

                this._dal_prCierres.MySqlConnectionTx = null;
                this._moduloGlobal._mySqlConnectionTx = null;
            }
        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Realiza el proceso de cierre diario
        /// </summary>
        public DTO_TxResult Proceso_CierreDia()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prCierres = (DAL_prCierres)base.GetInstance(typeof(DAL_prCierres), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                
                #region Variables y validaciones

                DateTime periodo = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo));
                string monedaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                //Fecha final 
                int maxDay = DateTime.Now.Day;
                DateTime maxDate = DateTime.Now;
                if (periodo.Year != maxDate.Year || periodo.Month != maxDate.Month)
                {
                    maxDay = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    maxDate = new DateTime(periodo.Year, periodo.Month, maxDay);
                }

                //Fecha inicial
                string diaIniStr = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiaUltimoCierre);
                int diaIni = string.IsNullOrWhiteSpace(diaIniStr) ? 1 : Convert.ToInt16(diaIniStr) + 1;

                #endregion

                if (diaIni <= maxDay)
                {
                    for (int i = diaIni; i <= maxDay; ++i)
                    {
                        #region Realiza el proceso de cierre por día

                        //Carga las variables
                        DateTime fechaCierre = new DateTime(periodo.Year, periodo.Month, i);
                        decimal tc = this._moduloGlobal.TasaDeCambio_Get(monedaLoc, fechaCierre);
                        string EmpNro = this.Empresa.NumeroControl.Value;
                        string _modId = ((int)ModulesPrefix.pr).ToString();
                        if (_modId.Length == 1)
                            _modId = "0" + _modId;
                        string keyControl = EmpNro + _modId + AppControl.pr_DiaUltimoCierre;

                        //Hace el cierre del día
                        result = this.CerrarDia(periodo, fechaCierre, tc, keyControl);
                        if(result.Result == ResultValue.NOK)
                        {
                            result.ResultMessage = "No se pudo procesar el cierre del día " + fechaCierre.Day.ToString();
                            return result;
                        }

                        #endregion
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proveedores_Proceso_CierreDia");

                return result;
            }
        }

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="footer">la lista de detalle</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject CierreDetalle_Guardar(int documentID, DTO_glDocumentoControl ctrl, List<DTO_prDetalleDocu> footer, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Declara las variables
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                #endregion

                #region Guardar en glDocumentoControl
                ctrl.DocumentoNro.Value = 0;
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

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion                
                #region Guardar en prDetalleDocu y prSolicitudCargos
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_prSolicitudCargos> cargos = new List<DTO_prSolicitudCargos>();
                foreach (DTO_prDetalleDocu det in footer)
                {
                    det.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    det.DatoAdd3.Value = "Cerrado";
                    det.CantidadxEmpaque.Value = !det.CantidadxEmpaque.Value.HasValue || det.CantidadxEmpaque.Value == 0 ? 1 : det.CantidadxEmpaque.Value;
                    det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                    if (documentID == AppDocuments.CierreDetalleSolicitud)
                    {
                        det.CantidadSol.Value = det.CantidadCierre.Value * -1;
                        det.CantidadOC.Value = null;
                        det.CantidadRec.Value = null; 
                    }                       
                    else if (documentID == AppDocuments.CierreDetalleOrdenComp)
                    {
                        det.CantidadOC.Value = det.CantidadCierre.Value * -1;
                        det.ValorTotML.Value = Math.Round(det.ValorUni.Value.Value * (det.CantidadOC.Value.Value/det.CantidadxEmpaque.Value.Value),2);
                        det.IvaTotML.Value = Math.Round(det.IVAUni.Value.Value * (det.CantidadOC.Value.Value/det.CantidadxEmpaque.Value.Value), 2);
                        det.CantidadSol.Value = null;
                        det.CantidadRec.Value = null; 
                    }                       
                    else if (documentID == AppDocuments.CierreDetalleRecibidos)
                    {
                        det.CantidadRec.Value = det.CantidadCierre.Value * -1;
                        det.ValorTotML.Value = Math.Round(det.ValorUni.Value.Value * det.CantidadRec.Value.Value,2);
                        det.IvaTotML.Value = Math.Round(det.IVAUni.Value.Value * det.CantidadRec.Value.Value,2);
                        det.CantidadSol.Value = null;
                        det.CantidadOC.Value = null; 
                    }                        
                    this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);

                    if (documentID == AppDocuments.CierreDetalleSolicitud ||  documentID == AppDocuments.CierreDetalleRecibidos)
                    {
                        #region Actualiza movimientos de Proyectos si existen (pyProhyectoMvto)
                        DTO_pyProyectoMvto mvtoProyecto = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(det.Detalle4ID.Value);
                        if (mvtoProyecto != null)
                        {
                            if (documentID == AppDocuments.CierreDetalleSolicitud)
                                mvtoProyecto.CantidadPROV.Value = mvtoProyecto.CantidadPROV.Value + det.CantidadSol.Value;
                            else if (documentID == AppDocuments.CierreDetalleRecibidos)
                                mvtoProyecto.CantidadREC.Value = mvtoProyecto.CantidadREC.Value + det.CantidadRec.Value;
                            this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoProyecto);
                        }
                        #endregion  
                    }                      

                    //foreach (DTO_prSolicitudCargos itemCargos in itemFooter.SolicitudCargos)
                    //{
                    //    itemCargos.NumeroDoc.Value = det.NumeroDoc.Value;
                    //    itemCargos.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                    //    if (string.IsNullOrEmpty(det.LineaPresupuestoID.Value))
                    //    {
                    //        DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                    //        DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                    //        itemCargos.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                    //    }
                    //    else
                    //        itemCargos.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                    //    cargos.Add(itemCargos);
                    //}
                }
                //result = this.prSolicitudCargos_Add(cargos);

                //if (result.Result == ResultValue.NOK)
                //{
                //    numeroDoc = 0;
                //    return result;
                //}
               
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(ctrl.NumeroDoc.Value.Value, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    return alarma;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CierreDetalle_Guardar");
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
                        this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, true);
                        alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        #endregion

        #endregion

        #region General

        /// <summary>
        /// Consulta los modulos activos
        /// </summary>
        /// <returns></returns>
        internal bool GetModuleActive(ModulesPrefix mod)
        {
            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool ModuleActive = false;
            List<DTO_aplModulo> modsActive = this._moduloAplicacion.aplModulo_GetByVisible(1, true).ToList();
            foreach (DTO_aplModulo m in modsActive)
            {
                string item = m.ModuloID.Value.ToLower();
                if (item == mod.ToString())
                {
                    ModuleActive = true;
                    break;
                }
            }
            return ModuleActive;
        }

        #endregion

        #region prDetalleDocu

        #region Funciones Publicas
        /// <summary>
        /// Trae un activo control de auerdo a la llave primaria
        /// </summary>
        /// <param name="activoId">Identificador del activo</param>
        /// <returns>Retorna el activo</returns>
        public DTO_prDetalleDocu prDetalleDocu_GetByID(int detalleID)
        {
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByID(detalleID);
        }

        /// <summary>
        /// Trae la lista de prDetalleDocu segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <param name="isFactura">Valida si se filtra por identificador de FacturaDocuID</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByNumeroDoc(int NumeroDoc, bool isFactura)
        {
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByNumeroDoc(NumeroDoc, isFactura);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="document">Documento a filtrar</param>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="consecutivoDeta">identificador del detalle si se requiere</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetByDocument(int document, int numeroDoc, int consecutivoDeta)
        {
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByDocument(document, numeroDoc, consecutivoDeta);
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter">Filtro de la tabla</param>
        /// <returns>Lista Dto de Detalle Docu</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetParameter(DTO_prDetalleDocu filter)
        {            
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByParameter(filter);
        }

        /// <summary>
        /// Agrega un registro a rDetalleDocu
        /// </summary>
        /// <param name="rec">Registroo para agregar</param>
        /// <returns></returns>
        public DTO_TxResultDetail prDetalleDocu_Add(int documentoID, DTO_prDetalleDocu rec, bool insideAnotherTx)
        {
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                rd.Message = "OK";
                bool validDto = true;

                string msg_FkNotFound = DictionaryMessages.FkNotFound;

                #region Validar FKs

                DAL_MasterSimple dalMasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region inReferencia
                dalMasterSimple.DocumentID = AppMasters.inReferencia;
                if (!string.IsNullOrWhiteSpace(rec.inReferenciaID.Value) && dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = rec.inReferenciaID.Value }, true) == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "inReferencia";
                    rdF.Message = msg_FkNotFound + "&&" + rec.inReferenciaID.Value;
                    rd.DetailsFields.Add(rdF);
                    validDto = false;
                }
                #endregion
                #region EmpaqueInvID
                if (string.IsNullOrWhiteSpace(rec.EmpaqueInvID.Value))
                {
                    if (!string.IsNullOrWhiteSpace(rec.inReferenciaID.Value))
                    {
                        DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                        rec.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                    }
                    else
                    {
                        DTO_inEmpaque emp = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, rec.UnidadInvID.Value, true, false);
                        if (emp != null)
                            rec.EmpaqueInvID.Value = rec.UnidadInvID.Value;
                    }
                }                
                #endregion
                #region Moneda
                dalMasterSimple.DocumentID = AppMasters.glMoneda;
                if (!string.IsNullOrWhiteSpace(rec.MonedaID.Value) && dalMasterSimple.DAL_MasterSimple_GetByID(new UDT_BasicID() { Value = rec.MonedaID.Value }, true) == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "MonedaID";
                    rdF.Message = msg_FkNotFound + "&&" + rec.MonedaID.Value;
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

                int actId = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(rec);

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, documentoID.ToString(), actId.ToString(), string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                rd.Key = actId.ToString();
                return rd;
            }
            catch (Exception ex)
            {
                rd.Message = ResultValue.NOK.ToString();
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloProveedores_prDetalleDocu_Add");
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
        /// Edita un registro de detalleDocu
        /// </summary>
        /// <param name="rec">Documento que se va a editar</param>
        /// <param name="updBitacora">Indica si se debe actualizar la bitacora</param>
        public void prDetalleDocu_Update(int documentoID, DTO_prDetalleDocu rec, bool updBitacora, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.Message = "OK";

            try
            {
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Valida EmpaqueInvID
                if (string.IsNullOrWhiteSpace(rec.EmpaqueInvID.Value))
                {
                    if (!string.IsNullOrWhiteSpace(rec.inReferenciaID.Value))
                    {
                        DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, rec.inReferenciaID.Value, true, false);
                        rec.EmpaqueInvID.Value = refer.EmpaqueInvID.Value;
                    }
                    else
                    {
                        DTO_inEmpaque emp = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, rec.UnidadInvID.Value, true, false);
                        if (emp != null)
                            rec.EmpaqueInvID.Value = rec.UnidadInvID.Value;
                    }
                }
                #endregion
                this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(rec);

                if (updBitacora)
                {
                    #region Guarda en la bitacora
                    this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, documentoID.ToString(), rec.NumeroDoc.Value.ToString(), string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //Log error
                rd.Message = "NOK";
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloProveedores_prDetalleDocu_Update");
                throw ex;
            }
            finally
            {
                if (rd.Message == ResultValue.OK.ToString())
                {
                    if (!insideAnotherTx)
                        this._mySqlConnectionTx.Commit();
                }
                else
                {
                    rd.Message = ResultValue.NOK.ToString();
                    if (base._mySqlConnectionTx != null && !insideAnotherTx)
                        this._mySqlConnectionTx.Rollback();
                }
            }

        }

        /// <summary>
        /// Obtiene los items para cerrar de un documento
        /// </summary>
        /// <param name="documentFilter">Documento  a cerrar</param>
        /// <param name="prefijoID">Prefijo del doc</param>
        /// <param name="docNro">nro  del Doc</param>
        /// <param name="proveedorID">Proveedor</param>
        /// <param name="referenciaID">Referencia</param>
        /// <param name="codigoBS">Codigo BS</param>
        /// <returns>Lista de detalle</returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetPendienteForCierre(int documentFilter, string prefijoID, int? docNro, string proveedorID, string referenciaID, string codigoBS)
        {
            List<DTO_prDetalleDocu> result = new List<DTO_prDetalleDocu>();
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            result = this._dal_prDetalleDocu.DAL_prDetalleDocu_GetPendienteForCierre(documentFilter, prefijoID, docNro, proveedorID, referenciaID,codigoBS);

            foreach (var r in result)
            {
                DTO_MasterBasic proyecto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Hierarchy,AppMasters.coProyecto,r.ProyectoID.Value,true,false);
                r.ProyectoDesc.Value = proyecto != null? proyecto.Descriptivo.Value : string.Empty;
                DTO_MasterBasic proveedor = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, r.ProveedorID.Value, true, false);
                r.ProveedorDesc.Value = proveedor != null? proveedor.Descriptivo.Value : string.Empty;
            }
            
            return result;
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro para proyectos 
        /// </summary>
        /// <param name="numeroDoc">identificador del documento a filtrar</param>
        /// <param name="proyectoID">proyecto filtrado</param>
        /// <returns></returns>
        public List<DTO_prDetalleDocu> prDetalleDocu_GetSolicitudByProyecto(int documentFilter, int? numeroDoc, string proyectoID)
        {
            this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleDocu.DAL_prDetalleDocu_GetSolicitudByProyecto(documentFilter,numeroDoc,proyectoID);
        }

        #endregion

        #endregion

        #region Solicitud

        #region prSolicitudCargos

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de prSolicitudCargos segun el identificador del detalle
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        public List<DTO_prSolicitudCargos> prSolicitudCargos_GetByConsecutivoDetaID(int documentID, int ConsecutivoDetaID)
        {
            this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(ConsecutivoDetaID);
        }

        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Trae la lista de prSolicitudCargos segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prSolicitudCargos> prSolicitudCargos_GetByNumeroDoc(int NumeroDoc)
        {
            this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Guarda la lista de prSolicitudCargos en base de datos
        /// </summary>
        /// <param name="solCargos">la lista de DTO_prSolicitudCargos</param>
        /// <returns></returns>
        private DTO_TxResult prSolicitudCargos_Add(List<DTO_prSolicitudCargos> solCargos)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos.DAL_prSolicitudCargos_Add(solCargos);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prSolicitudCargos_Add");
                return result;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prSolicitudCargos
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void prSolicitudCargos_Delete(int NumeroDoc)
        {
            this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prSolicitudCargos.DAL_prSolicitudCargos_Delete(NumeroDoc);
        }
        #endregion

        #endregion

        #region prSolicitudDocu
        /// <summary>
        /// Adiciona en la tabla prSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prSolicitudDocu_Add(DTO_prSolicitudDocu solDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudDocu.DAL_prSolicitudDocu_Add(solDocu);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, solDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prSolicitudDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prSolicitudDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        private DTO_prSolicitudDocu prSolicitudDocu_Get(int NumeroDoc)
        {
            this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prSolicitudDocu.DAL_prSolicitudDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prSolicitudDocu_Upd(DTO_prSolicitudDocu solDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudDocu.DAL_prSolicitudDocu_Upd(solDocu);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, solDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prSolicitudDocu_Upd");
                return result;
            }
        }
        #endregion

        #region prSolicitudDirectaDocu
        /// <summary>
        /// Adiciona en la tabla prSolicitudDirectaDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDirectaDocu</param>
        /// <returns></returns>
        private DTO_TxResult prSolicitudDirectaDocu_Add(DTO_prSolicitudDirectaDocu solDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prSolicitudDirectaDocu = (DAL_prSolicitudDirectaDocu)base.GetInstance(typeof(DAL_prSolicitudDirectaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudDirectaDocu.DAL_prSolicitudDirectaDocu_Add(solDocu);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, solDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prSolicitudDirectaDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prSolicitudDirectaDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        private DTO_prSolicitudDirectaDocu prSolicitudDirectaDocu_Get(int NumeroDoc)
        {
            this._dal_prSolicitudDirectaDocu = (DAL_prSolicitudDirectaDocu)base.GetInstance(typeof(DAL_prSolicitudDirectaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prSolicitudDirectaDocu.DAL_prSolicitudDirectaDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prSolicitudDirectaDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDirectaDocu</param>
        /// <returns></returns>
        private DTO_TxResult prSolicitudDirectaDocu_Upd(DTO_prSolicitudDirectaDocu solDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prSolicitudDirectaDocu = (DAL_prSolicitudDirectaDocu)base.GetInstance(typeof(DAL_prSolicitudDirectaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudDirectaDocu.DAL_prSolicitudDirectaDocu_Upd(solDocu);

                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, solDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prSolicitudDirectaDocu_Upd");
                return result;
            }
        }
        #endregion

        #region Funciones privadas

        /// <summary>
        /// LLena DTO_prSolicitudFooter con los datos de DTO_prDetalleDocu y DTO_prSolicitudCargos
        /// </summary>
        /// <param name="detail">la lista de DTO_prDetalleDocu</param>
        /// <param name="cargos">la lista de DTO_prSolicitudCargos</param>
        /// <returns></returns>
        private List<DTO_prSolicitudFooter> SolicitudFooter_Load(List<DTO_prDetalleDocu> detail, List<DTO_prSolicitudCargos> cargos)
        {
            List<DTO_prSolicitudFooter> solFooter = new List<DTO_prSolicitudFooter>();
            DTO_prSolicitudFooter solItem;
            int index;
            foreach (DTO_prDetalleDocu det in detail)
            {
                solItem = new DTO_prSolicitudFooter();
                solItem.DetalleDocu = det;
                index = 0;
                foreach (DTO_prSolicitudCargos cargo in cargos)
                {
                    if (cargo.NumeroDoc.Value == det.NumeroDoc.Value && cargo.ConsecutivoDetaID.Value == det.ConsecutivoDetaID.Value)
                    {
                        cargo.Index = index;
                        cargo.IndexDet = solItem.DetalleDocu.Index;
                        solItem.SolicitudCargos.Add(cargo);
                        index++;
                    }
                }
                solItem.ProyectoID = solItem.SolicitudCargos[0].ProyectoID.Value;
                solItem.CentroCostoID = solItem.SolicitudCargos[0].CentroCostoID.Value;

                solFooter.Add(solItem);
            }
            return solFooter;
        }

        /// <summary>
        /// Aprueba Solicitud
        /// </summary>
        /// <param name="sol"> Solicitudde la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult Solicitud_Aprobar(int documentID, string actividadFlujoID, object item, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            int numeroDocRecibido = 0;
            bool modulePlaneacionActive = false;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                
                if (documentID == AppDocuments.SolicitudAprob)
                {
                    #region Aprueba Solicitud Corriente
                    DTO_prSolicitudAprobacion sol = (DTO_prSolicitudAprobacion)item;
                    DTO_glActividadFlujo act = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadFlujoID, true, false);

                    if (act.DocumentoID.Value == AppDocuments.SolicitudPreAprob.ToString())
                    {
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, sol.Observacion.Value, true);
                        this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actividadFlujoID, false, sol.Observacion.Value);
                    }
                    else if (act.DocumentoID.Value == AppDocuments.SolicitudAprob.ToString())
                    {
                        DTO_glDocumentoAprueba docAprueba = this._moduloGlobal.glDocumentoAprueba_UpdateUserApprover(sol.NumeroDoc.Value.Value);
                        if (docAprueba == null)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_UpdateData;
                        }
                        //Si UsuarioAprueba es null realiza el proceso de Aprobacion final
                        else if (docAprueba.UsuarioAprueba.Value == null)
                        {
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, sol.Observacion.Value, true);
                            this._moduloProyectos = (ModuloProyectos)this.GetInstance(typeof(ModuloProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)this.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            string trabajoXDef = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
                            List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(sol.NumeroDoc.Value.Value, false);
                            
                            #region Genera el registro en prSaldosDocu
                            //foreach (DTO_prDetalleDocu det in listDet)
                            //{
                            //    DTO_prSaldosDocu saldo = new DTO_prSaldosDocu();
                            //    saldo.EmpresaID.Value = det.EmpresaID.Value;
                            //    saldo.NumeroDoc.Value = det.NumeroDoc.Value;
                            //    saldo.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                            //    saldo.CantidadDocu.Value = det.CantidadSol.Value;
                            //    saldo.CantidadMovi.Value = 0;
                            //    this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldo);
                            //}
                            #endregion     
                            this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actividadFlujoID, false, sol.Observacion.Value);
                        }
                        else
                        {
                            #region Guarda en la bitacora
                            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Approve, DateTime.Now,
                                this.UserId, this.Empresa.ID.Value, sol.NumeroDoc.Value.Value.ToString(), documentID.ToString(), string.Empty,
                                string.Empty, string.Empty, 0, 0, 0);

                            #endregion
                        }
                    }
                    #endregion
                }
                else if (documentID == AppDocuments.SolicitudDirectaAprob)
                {
                    #region Aprueba Solicitud Directa
                    DTO_prSolicitudDirectaAprob sol = (DTO_prSolicitudDirectaAprob)item;

                    DTO_glDocumentoAprueba docAprueba = this._moduloGlobal.glDocumentoAprueba_UpdateUserApprover(sol.NumeroDoc.Value.Value);
                    if (docAprueba == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_UpdateData;
                    }
                    //Si UsuarioAprueba es null realiza el proceso de Aprobacion final
                    else if (docAprueba.UsuarioAprueba.Value == null)
                    {
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, sol.Observacion.Value, true);
                       
                        this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_MasterHierarchy = (DAL_MasterHierarchy)this.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)this.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_plPlaneacionProveedor = (DAL_plPlaneacion_Proveedores)this.GetInstance(typeof(DAL_plPlaneacion_Proveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                        DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(sol.NumeroDoc.Value.Value);
                        List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(sol.NumeroDoc.Value.Value, false);
                        string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);

                        foreach (DTO_prDetalleDocu det in listDet)
                        {
                            #region Guarda en prSaldosDocu(SolicituDirecta)
                            //DTO_prSaldosDocu saldo = new DTO_prSaldosDocu();
                            //saldo.EmpresaID.Value = det.EmpresaID.Value;
                            //saldo.NumeroDoc.Value = det.NumeroDoc.Value;
                            //saldo.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                            //saldo.CantidadDocu.Value = det.CantidadDoc5.Value;
                            //saldo.CantidadMovi.Value = 0;
                            //this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldo);
                            #endregion
                            #region Actualiza prBienServicio con Info de la Soldirecta
                            DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);

                            if (bienServicioClase != null)
                            {
                                bienServicio.DocCompra.Value = docCtrl.NumeroDoc.Value;
                                bienServicio.MonCompra.Value = docCtrl.MonedaID.Value;
                                bienServicio.VlrCompra.Value = docCtrl.Valor.Value;
                                bienServicio.CtrlVersion.Value = bienServicio.CtrlVersion.Value++;
                                this._dal_MasterHierarchy.DocumentID = AppMasters.prBienServicio;
                                result = this._dal_MasterHierarchy.DAL_MasterHierarchy_Update(bienServicio, true);

                                if (result.Result == ResultValue.NOK)
                                    return result;                              
                            }

                            #endregion
                            #region Guarda en plPlaneacionProveedores
                            List<DTO_prSolicitudCargos> cargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(det.Detalle5ID.Value.Value);
                            if (cargos.Count > 0)
                            {
                                DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, cargos.First().ProyectoID.Value, true, false);

                                Dictionary<string, string> pks = new Dictionary<string, string>();
                                pks.Add("ActividadID", proy.ActividadID.Value);
                                pks.Add("LineaPresupuestoID", cargos.First().LineaPresupuestoID.Value);
                                DTO_plActividadLineaPresupuestal actLinea = (DTO_plActividadLineaPresupuestal)this.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pks, true);
                                if (actLinea != null)
                                {
                                    DTO_plPlaneacion_Proveedores planeaProveedor = new DTO_plPlaneacion_Proveedores();
                                    planeaProveedor.ConsActLinea.Value = actLinea.ReplicaID.Value;
                                    planeaProveedor.CodigoBSID.Value = det.CodigoBSID.Value;
                                    planeaProveedor.inReferenciaID.Value = !string.IsNullOrEmpty(det.inReferenciaID.Value) ? det.inReferenciaID.Value : refxDefecto;
                                    List<DTO_plPlaneacion_Proveedores> exist = this._dal_plPlaneacionProveedor.DAL_plPlaneacion_Proveedores_GetByParameter(planeaProveedor);
                                    if (exist.Count == 0)
                                    {
                                        try { this._dal_plPlaneacionProveedor.DAL_plPlaneacion_Proveedores_Add(planeaProveedor); }
                                        catch
                                        {
                                            result.ResultMessage = DictionaryMessages.Err_Pl_SavePlaneacionProveedor;
                                            result.Result = ResultValue.NOK;
                                            return result;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                       
                        #region Crea Recibido
                        DTO_prRecibido _data = new DTO_prRecibido();
                        DTO_prSolicitud solicitud = this.Solicitud_Load(AppDocuments.SolicitudDirecta, docCtrl.PrefijoID.Value, docCtrl.DocumentoNro.Value.Value);
                        DTO_prProveedor proveedorDto = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, solicitud.HeaderSolDirecta.ProveedorID.Value, true, false);
                        string bodegaTransito = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
                        DTO_inBodega bodegaDto = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaTransito, true, false);
                        string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                        string monedaExtran = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                        #region Carga Header
                        _data.Header.NumeroDoc.Value = 0;
                        _data.Header.BodegaID.Value = bodegaTransito;
                        _data.Header.ProveedorID.Value = solicitud.HeaderSolDirecta.ProveedorID.Value;
                        _data.Header.LugarEntrega.Value = bodegaDto.LocFisicaID.Value;
                        #endregion
                        #region Carga DocumentoControl
                        _data.DocCtrl.NumeroDoc.Value = 0;
                        _data.DocCtrl.DocumentoID.Value = AppDocuments.Recibido;
                        _data.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        _data.DocCtrl.TerceroID.Value = proveedorDto.TerceroID.Value;
                        _data.DocCtrl.ComprobanteID.Value = string.Empty;
                        _data.DocCtrl.ComprobanteIDNro.Value = 0;
                        _data.DocCtrl.MonedaID.Value = proveedorDto.TipoProveedor.Value == (byte)TipoProveedor.Local? monedaLocal : monedaExtran;
                        _data.DocCtrl.CuentaID.Value = string.Empty;
                        _data.DocCtrl.ProyectoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                        _data.DocCtrl.CentroCostoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                        _data.DocCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                        _data.DocCtrl.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                        _data.DocCtrl.Fecha.Value = DateTime.Now;
                        _data.DocCtrl.PeriodoDoc.Value = sol.PeriodoDoc.Value;
                        _data.DocCtrl.PrefijoID.Value = sol.PrefijoID.Value;
                        _data.DocCtrl.TasaCambioCONT.Value = _data.DocCtrl.MonedaID.Value == monedaLocal? 0: this._moduloGlobal.TasaDeCambio_Get(monedaExtran, DateTime.Now.Date);
                        _data.DocCtrl.TasaCambioDOCU.Value = _data.DocCtrl.TasaCambioCONT.Value;
                        _data.DocCtrl.DocumentoNro.Value = 0;
                        _data.DocCtrl.PeriodoUltMov.Value = sol.PeriodoDoc.Value;
                        _data.DocCtrl.seUsuarioID.Value = this.UserId;
                        _data.DocCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                        _data.DocCtrl.ConsSaldo.Value = 0;
                        _data.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        _data.DocCtrl.Observacion.Value = string.Empty;
                        _data.DocCtrl.FechaDoc.Value =  DateTime.Now.Date;
                        _data.DocCtrl.Descripcion.Value = "Recibido por Solicitud Directa";
                        _data.DocCtrl.Valor.Value = sol.MonedaID.Value == monedaLocal ? sol.TotalML.Value : sol.TotalME.Value;
                        _data.DocCtrl.Iva.Value = sol.MonedaID.Value == monedaLocal ? sol.IvaML.Value : sol.IvaME.Value;
                        #endregion
                        #region Carga Footer
                        List<DTO_prOrdenCompraResumen> _listRecibidos = new List<DTO_prOrdenCompraResumen>();
                        foreach (var footer in solicitud.Footer)
                        {
                            DTO_prOrdenCompraResumen recibido = new DTO_prOrdenCompraResumen();
                            DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, footer.DetalleDocu.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bsc = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                            TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsc.TipoCodigo.Value.Value.ToString());
                            if (tipoCodigo == TipoCodigo.Inventario)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Pr_TipoCodigoInvalid;
                                return result;
                            }
                            recibido.CodigoBSID.Value = footer.DetalleDocu.CodigoBSID.Value;
                            recibido.ClaseBS = tipoCodigo;
                            recibido.CantidadRec.Value = footer.DetalleDocu.CantidadDoc5.Value;
                            recibido.Descriptivo.Value = footer.DetalleDocu.Descriptivo.Value;
                            recibido.inReferenciaID.Value = footer.DetalleDocu.inReferenciaID.Value;
                            recibido.OrdCompraDetaID.Value = footer.DetalleDocu.Detalle5ID.Value; //Guarda el cons del detalle de la Sol Directa
                            recibido.OrdCompraDocuID.Value = footer.DetalleDocu.Documento5ID.Value; //Guarda el numero Doc de la Sol Direct
                            _listRecibidos.Add(recibido);
                        }

                        #endregion
                        #region Guarda el Recibido
                        DTO_SerializedObject res = this.Recibido_Guardar(AppDocuments.Recibido, _data.DocCtrl, _data.Header, _listRecibidos, out numeroDocRecibido, string.Empty, string.Empty, batchProgress, true);
                        if (res.GetType() == typeof(DTO_TxResult))
                        {
                            result = (DTO_TxResult)res;
                            return result;
                        }
                        //Verifica si tiene activo el Modulo de Planeacion
                        modulePlaneacionActive = this.GetModuleActive(ModulesPrefix.pl);
                        #endregion
                        #region Aprueba el Recibido
                        #region Genera saldo de Recibido y actualiza el saldo de Solicitud Directa
                        //List<DTO_prDetalleDocu> listDetRec = this.prDetalleDocu_GetByNumeroDoc(numeroDocRecibido, false);
                        //foreach (DTO_prDetalleDocu det in listDetRec)
                        //{
                        //    DTO_prSaldosDocu saldoRec = new DTO_prSaldosDocu();
                        //    saldoRec.EmpresaID.Value = det.EmpresaID.Value;
                        //    saldoRec.NumeroDoc.Value = det.NumeroDoc.Value;
                        //    saldoRec.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                        //    saldoRec.CantidadDocu.Value = det.CantidadRec.Value;
                        //    saldoRec.CantidadMovi.Value = 0;

                        //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                        //    saldoOC = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.Detalle5ID.Value.Value);
                        //    if (saldoOC != null)
                        //    {
                        //        saldoOC.CantidadMovi.Value += det.CantidadRec.Value.Value;
                        //        if (saldoOC.CantidadMovi.Value.Value > saldoOC.CantidadDocu.Value.Value)
                        //        {
                        //            result.Result = ResultValue.NOK;
                        //            result.ResultMessage = "Saldos inválidos en proveedores (prSaldosDocu)";
                        //        }                                   
                        //        else
                        //        {
                        //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoRec);
                        //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoOC);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        result.Result = ResultValue.NOK;
                        //        result.ResultMessage = "Error al traer saldos de Orden Compra: Revisar Codigo";
                        //    }

                        //}
                        #endregion
                        #region Cierra el flujo Recibido
                        List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RecibidoAprob);
                        if (act.Count > 0)
                            this.AsignarFlujo(documentID, numeroDocRecibido, act[0], false, string.Empty);
                        #endregion                        
                        #endregion

                        #endregion
                        
                        this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actividadFlujoID, false, sol.Observacion.Value);
                    }
                    else
                    {
                        #region Guarda en la bitacora
                        this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Approve, DateTime.Now,
                            this.UserId, this.Empresa.ID.Value, sol.NumeroDoc.Value.Value.ToString(), documentID.ToString(), string.Empty,
                            string.Empty, string.Empty, 0, 0, 0);

                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK && !insideAnotherTx)
                {
                    base._mySqlConnectionTx.Commit();
                    #region Genera Consecutivos Solicitud Directa
                    if (documentID == AppDocuments.SolicitudDirectaAprob)
                    {
                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        if (numeroDocRecibido != 0)
                        {
                            #region Documento Recibido
                            DTO_glDocumentoControl docCtrlRecib = this._moduloGlobal.glDocumentoControl_GetByID(numeroDocRecibido);
                            docCtrlRecib.DocumentoNro.Value = this.GenerarDocumentoNro(docCtrlRecib.DocumentoID.Value.Value, docCtrlRecib.PrefijoID.Value);                                                   
                            this._moduloGlobal.ActualizaConsecutivos(docCtrlRecib, true, false, false);
                            #endregion
                        }
                    }
                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza Solicitud
        /// </summary>
        /// <param name="sol"> Solicitud de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void Solicitud_Rechazar(int documentID, string actividadFlujoID, int numeroDoc, string observ, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool isValid = true;
            try
            {
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.SinAprobar, observ, true);
                this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, true, observ);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Err_Pr_SolicitudRechazar, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Solicitud_Rechazar");
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
        /// Asogna Solicitud
        /// </summary>
        /// <param name="sol"> Solicitud de la lista</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult Solicitud_AsignarDocumento(int documentID, string actividadFlujoID, DTO_prSolicitudAsignacion sol, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                int i = 0;

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Obtiene el header asociado de la tabla prSolicitudDOcu
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glActividadFlujo act = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadFlujoID, true, false);

                if (act.DocumentoID.Value == AppDocuments.SolicitudAsign.ToString())
                {
                    foreach (DTO_prSolicitudAsignDet solDet in sol.SolicitudAsignDet)
                    {
                        if (solDet.Asignado.Value.Value)
                        {
                            DTO_prDetalleDocu det = this.prDetalleDocu_GetByID(solDet.ConsecutivoDetaID.Value.Value);
                            det.DatoAdd1.Value = solDet.DatoAdd1.Value;
                            this.prDetalleDocu_Update(documentID, det, false, true);

                            int percent = ((i + 1) * 100) / (sol.SolicitudAsignDet.Where(x => x.Asignado.Value.Value).Count());
                            batchProgress[tupProgress] = percent;
                            i++;
                        }
                    }

                    if (sol.Asignado.Value.Value)
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, sol.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, sol.ObservacionDoc.Value, true);
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    return result;
                }
                this.AsignarFlujo(documentID, sol.NumeroDoc.Value.Value, actividadFlujoID, false, sol.ObservacionDoc.Value);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_AsignarDocumento");

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

            return result;
        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="ctrl">referencia a glDocumentoControl</param>
        /// <param name="header">prSolicitudDocu</param>
        /// <param name="footer">la lista de DTO_prSolicitudFooter</param>
        /// <param name="update">true si la solicitud esta actualizada</param>
        /// <param name="numeroDoc">identificador interior del documento</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject Solicitud_Guardar(int documentID, DTO_glDocumentoControl ctrl, DTO_prSolicitudDocu header, DTO_prSolicitudDirectaDocu headerDirecta, List<DTO_prSolicitudFooter> footer, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProyectos = (ModuloProyectos)base.GetInstance(typeof(ModuloProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_glDocumentoAprueba = (DAL_glDocumentoAprueba)base.GetInstance(typeof(DAL_glDocumentoAprueba), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Declara las variables
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                #endregion

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.ComprobanteIDNro.Value = 0;
                    if (documentID != AppDocuments.Solicitud)
                    {
                        ctrl.Valor.Value = monedaLocal == ctrl.MonedaID.Value ? footer.Sum(x => x.DetalleDocu.ValorTotML.Value) : footer.Sum(x => x.DetalleDocu.ValorTotME.Value);
                        ctrl.Iva.Value = monedaLocal == ctrl.MonedaID.Value ? footer.Sum(x => x.DetalleDocu.IvaTotML.Value) : footer.Sum(x => x.DetalleDocu.IvaTotME.Value);
                    }
                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(resultGLDC);

                        numeroDoc = 0;
                        return result;
                    }
                    ctrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guardar en prSolicitudDocu o prSolicitudDirectaDocu
                    if (documentID == AppDocuments.Solicitud)
                    {
                        header.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        result = this.prSolicitudDocu_Add(header, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                    }
                    else //Solicitud Directa
                    {
                        headerDirecta.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                        result = this.prSolicitudDirectaDocu_Add(headerDirecta, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                    }

                    #endregion;
                    #region Guardar en prDetalleDocu y prSolicitudCargos
                    this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    List<DTO_prSolicitudCargos> cargos = new List<DTO_prSolicitudCargos>();
                    foreach (DTO_prSolicitudFooter itemFooter in footer)
                    {
                        #region Obtiene valores del Proyecto si existen
                        if (itemFooter.DetalleDocu.ConsecTarea.HasValue)
                        {
                            //Trae la tarea solicitada
                            DTO_pyProyectoTarea tarea = this._moduloProyectos.pyProyectoTarea_GetByConsecutivo(itemFooter.DetalleDocu.ConsecTarea.Value);
                            string recursoID = !string.IsNullOrEmpty(itemFooter.DetalleDocu.inReferenciaID.Value) ? itemFooter.DetalleDocu.inReferenciaID.Value : itemFooter.DetalleDocu.CodigoBSID.Value;

                            if (tarea != null)
                            {                             
                                itemFooter.DetalleDocu.LineaPresupuestoID.Value = tarea.LineaPresupuestoID.Value;
                                itemFooter.DetalleDocu.Detalle5ID.Value = tarea.Consecutivo.Value;//Guarda el consecutivo de la tarea 
                            }
                        }
                        #endregion
                        DTO_prDetalleDocu det = itemFooter.DetalleDocu;
                        det.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(itemFooter.DetalleDocu);
                        if (documentID == AppDocuments.Solicitud)
                        {
                            det.SolicitudDocuID.Value = det.NumeroDoc.Value;
                            det.SolicitudDetaID.Value = det.ConsecutivoDetaID.Value;
                        }
                        else if (documentID == AppDocuments.SolicitudDirecta) //Solicitud Directa
                        {
                            det.Documento5ID.Value = det.NumeroDoc.Value;
                            det.Detalle5ID.Value = det.ConsecutivoDetaID.Value;
                        }
                        this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(itemFooter.DetalleDocu);

                        #region Agrega los cargos
                        foreach (DTO_prSolicitudCargos itemCargos in itemFooter.SolicitudCargos)
                        {
                            itemCargos.NumeroDoc.Value = det.NumeroDoc.Value;
                            itemCargos.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                            if (string.IsNullOrEmpty(det.LineaPresupuestoID.Value))
                            {
                                DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                                DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                                itemCargos.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                            }
                            else
                                itemCargos.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                            cargos.Add(itemCargos);
                        } 
                        #endregion                        
                    }
                    result = this.prSolicitudCargos_Add(cargos);

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

                    #region Actualiza glDocumentoControl
                    this._moduloGlobal.glDocumentoControl_Update(ctrl, true, true);

                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(ctrl.DocumentoID.Value.Value);
                    if (act.Count > 0)
                        this.AsignarFlujo(ctrl.DocumentoID.Value.Value, ctrl.NumeroDoc.Value.Value, act[0], false, string.Empty);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza en prSolicitudDocu o prSolicitudDirectaDocu
                    if (header != null)
                    {
                        result = this.prSolicitudDocu_Upd(header, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = header.NumeroDoc.Value.Value;
                            return result;
                        }
                    }
                    else //Solicitud Directa
                    {
                        result = this.prSolicitudDirectaDocu_Upd(headerDirecta, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = headerDirecta.NumeroDoc.Value.Value;
                            return result;
                        }
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza en prDetalleDocu y prSolicitudCargos
                    try
                    {
                        this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                        List<DTO_prDetalleDocu> detDocu = new List<DTO_prDetalleDocu>();
                        List<DTO_prSolicitudCargos> cargos = new List<DTO_prSolicitudCargos>();
                        detDocu = this.prDetalleDocu_GetByNumeroDoc(ctrl.NumeroDoc.Value.Value, false);

                        foreach (DTO_prSolicitudFooter itemFooter in footer)
                        {
                            DTO_prDetalleDocu det = itemFooter.DetalleDocu;

                            if (detDocu.Exists(x => x.ConsecutivoDetaID.Value.Equals(det.ConsecutivoDetaID.Value)))
                            {
                                this.prDetalleDocu_Update(documentID, det, false, true);
                                detDocu.RemoveAll(x => x.ConsecutivoDetaID.Value.Equals(det.ConsecutivoDetaID.Value));
                            }
                            else
                            {
                                det.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                                det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                                if (documentID == AppDocuments.Solicitud)
                                {
                                    det.SolicitudDocuID.Value = det.NumeroDoc.Value;
                                    det.SolicitudDetaID.Value = det.ConsecutivoDetaID.Value;
                                }
                                else //Solicitud Directa
                                {
                                    det.Documento5ID.Value = det.NumeroDoc.Value;
                                    det.Detalle5ID.Value = det.ConsecutivoDetaID.Value;
                                }
                                this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                            }
                            foreach (DTO_prSolicitudCargos itemCargos in itemFooter.SolicitudCargos)
                            {
                                itemCargos.NumeroDoc.Value = det.NumeroDoc.Value;
                                itemCargos.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                                if (string.IsNullOrEmpty(det.LineaPresupuestoID.Value))
                                {
                                    DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                                    DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                                    itemCargos.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                                }
                                else
                                    itemCargos.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                                cargos.Add(itemCargos);
                            }
                        }

                        if (detDocu.Count > 0)
                        {
                            foreach (DTO_prDetalleDocu det in detDocu)
                                this._dal_prDetalleDocu.DAL_prDetalleDocu_Delete(det.ConsecutivoDetaID.Value.Value);
                        }

                        this.prSolicitudCargos_Delete(ctrl.NumeroDoc.Value.Value);
                        result = this.prSolicitudCargos_Add(cargos);

                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Result = ResultValue.NOK;
                        numeroDoc = 0;
                        result.ResultMessage = "NOK";
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                }
                if (documentID == AppDocuments.Solicitud)
                    numeroDoc = header.NumeroDoc.Value.Value;
                else
                    numeroDoc = headerDirecta.NumeroDoc.Value.Value;
                ctrl.NumeroDoc.Value = numeroDoc;

                #region Guardar o actualiza en glDocumentoAprueba

                result = this._moduloGlobal.glDocumentoAprueba_AddByNivelApr(documentID, numeroDoc, 0);
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    ctrl.NumeroDoc.Value = 0;
                    if (documentID == AppDocuments.Solicitud)
                        header.NumeroDoc.Value = 0;
                    else if (documentID == AppDocuments.SolicitudDirecta)
                        headerDirecta.NumeroDoc.Value = 0;
                    return result;
                }
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    return alarma;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_Guardar");
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

                        if (!update)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, true);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de Documento interno</param>
        /// <returns>Retorna la solicitud</returns>
        public DTO_prSolicitud Solicitud_Load(int documentID, string prefijoID, int solNro)
        {
            try
            {
                DTO_prSolicitud sol = new DTO_prSolicitud();

                //Trae glDocumentoControl
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, prefijoID, solNro);
                sol.DocCtrl = docCtrl;

                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;

                //Carga prSolicitudDocu o prSolicitudDocuDirecta
                if (documentID == AppDocuments.Solicitud)
                {
                    DTO_prSolicitudDocu solHeader = this.prSolicitudDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    sol.Header = solHeader;
                }
                else
                {
                    DTO_prSolicitudDirectaDocu solHeader = this.prSolicitudDirectaDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    sol.HeaderSolDirecta = solHeader;
                }

                //Obtenga datos de la tabla prDetalleDocu
                List<DTO_prDetalleDocu> detDocu = this.prDetalleDocu_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value, false);
                //foreach (DTO_prDetalleDocu det in detDocu)
                //{
                //    if (det.Detalle4ID.Value.HasValue)
                //    {
                //        //Si existe obtiene los datos del proyecto
                //        DTO_pyProyectoMvto filter = new DTO_pyProyectoMvto();
                //        filter.Consecutivo.Value = det.Detalle4ID.Value;
                //        List<DTO_pyProyectoMvto> mvtos = this._moduloProyectos.pyProyectoMvto_GetParameter(filter).ToList();
                //        if (mvtos.Count > 0)
                //            det.TareaID = mvtos.First().TareaID.Value;
                //    }
                //    else if (det.Detalle5ID.Value.HasValue && sol.Header != null) //Revisa si existe el consecutivo de la tarea
                //    {
                //        //Trae la tarea solicitada
                //        DTO_pyProyectoTarea tarea = this._moduloProyectos.pyProyectoTarea_GetByConsecutivo(det.Detalle5ID.Value.Value);
                //        det.TareaID = tarea.TareaID.Value;                        
                //    } 
                //}

                //Obtenga datos de la tabla prSolicitudCargos
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_prSolicitudCargos> solCargos = this.prSolicitudCargos_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);

                //Llena detalle de la factura (DTO_faFacturacionFooter)
                List<DTO_prSolicitudFooter> solFooter = this.SolicitudFooter_Load(detDocu, solCargos);
                sol.Footer = solFooter;

                return sol;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Solicitud_Load");
                throw exception;
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
        public DTO_SerializedObject Solicitud_SendToAprob(int documentID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (docCtrl != null)
                {
                    #region Validacion del estado del documento
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }
                    #endregion
                    #region Se envia para aprobacion
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    if (createDoc)
                    {
                        try
                        {
                            #region Generar el nuevo archivo
                            if (documentID == AppDocuments.Solicitud)
                                this.GenerarArchivo(documentID, docCtrl.NumeroDoc.Value.Value, DtoReportSolicitud(docCtrl.NumeroDoc.Value.Value, true));
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
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.NumeroDoc = numeroDoc.ToString();
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_SendToAprob");
                throw ex;
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
        /// Trae un listado de las solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAprobacion> Solicitud_GetPendientesByModulo(ModulesPrefix mod, int documentoID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prSolicitudAprobacion> result = new List<DTO_prSolicitudAprobacion>();
                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<object> list = this._dal_prSolicitudDocu.DAL_prSolicitudDocu_GetPendientesByModulo(mod, documentoID, actFlujoID, usuario);

                foreach (Object item in list)
                {
                    ((DTO_prSolicitudAprobacion)item).FileUrl = base.GetFileRemotePath(((DTO_prSolicitudAprobacion)item).NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                    result.Add((DTO_prSolicitudAprobacion)item);
                }
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_prSolicitudAprobacion> sol, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in sol)
                {
                    porcTemp = (porcParte * i) / sol.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.Solicitud_Aprobar(documentID, actividadFlujoID, item, createDoc, new Dictionary<Tuple<int, int>, int>(), insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Solicitud_Aprobar");
                            rd.Message = DictionaryMessages.Err_Pr_SolicitudAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.Solicitud_Rechazar(documentID, actividadFlujoID, item.NumeroDoc.Value.Value, item.Observacion.Value, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Solicitud_Rechazar");
                            rd.Message = DictionaryMessages.Err_Pr_SolicitudRechazar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }

                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Trae un listado de las solicitudes pendientes para asignar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudAsignacion> Solicitud_GetPendientesParaAsignar(ModulesPrefix mod, int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prSolicitudAsignacion> result = new List<DTO_prSolicitudAsignacion>();
                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<object> list = this._dal_prSolicitudDocu.DAL_prSolicitudDocu_GetPendientesByModulo(mod, documentID, actFlujoID, usuario);

                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                foreach (Object item in list)
                {
                    DTO_prSolicitudAsignacion itemDoc = (DTO_prSolicitudAsignacion)item;
                    foreach (DTO_prSolicitudAsignDet itemDet in itemDoc.SolicitudAsignDet)
                        itemDet.SolicitudCargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(itemDet.ConsecutivoDetaID.Value.Value);
                    result.Add(itemDoc);
                }
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_GetPendientesParaAsignar");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes para asignar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Solicitud_Asignar(int documentID, string actividadFlujoID, List<DTO_prSolicitudAsignacion> sol, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in sol)
                {
                    porcTemp = (porcParte * i) / sol.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.SolicitudAsignDet.Exists(x => x.Asignado.Value.Value))
                    {
                        try
                        {
                            result = this.Solicitud_AsignarDocumento(documentID, actividadFlujoID, item, new Dictionary<Tuple<int, int>, int>(), insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Solicitud_Asignar");
                            rd.Message = DictionaryMessages.Err_Pr_SolicitudAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
                        alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                        alarma.Consecutivo = item.DocumentoNro.Value.ToString();
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_Asignar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Trae un listado de las solicitudes para orden de compra
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudResumen> Solicitud_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, TipoMoneda origenMonet)
        {
            try
            {
                //Obtiene si necesita asignacion
                bool asignInd = Convert.ToInt32(GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_IndAsignacionDirectaSolicitudes)) > 0;

                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_prSolicitudResumen> list = this._dal_prSolicitudDocu.DAL_prSolicitudDocu_GetResumen(documentID, asignInd, usuario, mod,origenMonet,(documentID == AppDocuments.OrdenCompra ? false : true));
                foreach (DTO_prSolicitudResumen item in list)
                    item.SolicitudCargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(item.ConsecutivoDetaID.Value.Value);
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_GetResumen");
                return null;
            }
        }

        /// <summary>
        /// Trae un listado de las solicitudes direcctas pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes</returns>
        public List<DTO_prSolicitudDirectaAprob> SolicitudDirecta_GetPendientesByModulo(int documentoID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prSolicitudDirectaAprob> result = new List<DTO_prSolicitudDirectaAprob>();
                this._dal_prSolicitudDirectaDocu = (DAL_prSolicitudDirectaDocu)base.GetInstance(typeof(DAL_prSolicitudDirectaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentoID.ToString(), true, true);
                result = this._dal_prSolicitudDirectaDocu.DAL_prSolicitudDirectaDocu_GetPendientesByModulo(doc, actFlujoID, usuario);

                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                foreach (var r in result)
                {
                    if (r.MonedaID.Value == monedaLocal)
                    {
                        r.TotalML.Value = r.SolicitudDirectaAprobDet.Sum(x => x.ValorTotML.Value);
                        r.IvaML.Value = r.SolicitudDirectaAprobDet.Sum(x => x.IvaTotML.Value);
                    }
                    else
                    {
                        r.TotalML.Value = r.SolicitudDirectaAprobDet.Sum(x => x.ValorTotME.Value);
                        r.IvaME.Value = r.SolicitudDirectaAprobDet.Sum(x => x.IvaTotME.Value);
                    }
                }
                //foreach (Object item in list)
                //{
                //    ((DTO_prSolicitudAprobacion)item).FileUrl = base.GetFileRemotePath(((DTO_prSolicitudAprobacion)item).NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                //    result.Add((DTO_prSolicitudAprobacion)item);
                //}
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de solicitudes directas para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="actividadFlujoID">actividad del documento</param>
        /// <param name="sol">solicitud que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> SolicitudDirecta_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_prSolicitudDirectaAprob> sol, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in sol)
                {
                    porcTemp = (porcParte * i) / sol.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.Solicitud_Aprobar(documentID, actividadFlujoID, item, createDoc, new Dictionary<Tuple<int, int>, int>(), insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Solicitud_Aprobar");
                            rd.Message = DictionaryMessages.Err_Pr_SolicitudAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.Solicitud_Rechazar(documentID, actividadFlujoID, item.NumeroDoc.Value.Value, item.Observacion.Value, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Solicitud_Rechazar");
                            rd.Message = DictionaryMessages.Err_Pr_SolicitudRechazar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }

                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Solicitud_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        #endregion
        #endregion

        #region Orden de Compra

        #region prOrdenCompraDocu

        /// <summary>
        /// Adiciona en la tabla prSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prOrdenCompraDocu_Add(DTO_prOrdenCompraDocu ordenDocu, int documetnoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)base.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Add(ordenDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documetnoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, ordenDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prOrdenCompraDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prSolicitudDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        public DTO_prOrdenCompraDocu prOrdenCompraDocu_Get(int NumeroDoc)
        {
            this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)base.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prOrdenCompraDocu_Upd(DTO_prOrdenCompraDocu ordenDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)base.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Upd(ordenDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, ordenDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prOrdenCompraDocu_Upd");
                return result;
            }
        }
        #endregion

        #region prOrdenCompraCotiza
        #region Funciones Privadas
        /// <summary>
        /// Trae la lista de prOrdenCompraCotiza segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prOrdenCompraCotiza> prOrdenCompraCotiza_GetByNumeroDoc(int NumeroDoc)
        {
            this._dal_prOrdenCompraCotiza = (DAL_prOrdenCompraCotiza)base.GetInstance(typeof(DAL_prOrdenCompraCotiza), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prOrdenCompraCotiza.DAL_prOrdenCompraCotiza_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Guarda la lista de prOrdenCompraCotiza en base de datos
        /// </summary>
        /// <param name="ordenCOtiza">la lista de DTO_prOrdenCompraCotiza</param>
        /// <returns></returns>
        private DTO_TxResult prOrdenCompraCotiza_Add(List<DTO_prOrdenCompraCotiza> ordenCOtiza)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_prOrdenCompraCotiza = (DAL_prOrdenCompraCotiza)base.GetInstance(typeof(DAL_prOrdenCompraCotiza), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prOrdenCompraCotiza.DAL_prOrdenCompraCotiza_Add(ordenCOtiza);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_prOrdenCompraCotiza_Add");
                return result;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prOrdenCompraCotiza
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void prOrdenCompraCotiza_Delete(int NumeroDoc)
        {
            this._dal_prOrdenCompraCotiza = (DAL_prOrdenCompraCotiza)base.GetInstance(typeof(DAL_prOrdenCompraCotiza), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prOrdenCompraCotiza.DAL_prOrdenCompraCotiza_Delete(NumeroDoc);
        }
        #endregion
        #endregion

        #region Funciones privadas

        /// <summary>
        /// Aprueba Orden de Compra
        /// </summary>
        /// <param name="ord"> Orden de Compra de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult OrdenCompra_Aprobar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prOrdenCompraAprob ord, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, decimal porcTotal, decimal porcParte, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl _dtoControl = null;
            ModulesPrefix mod = ModulesPrefix.pl;
            bool modulePlaneacionActive = false;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)this.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)this.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Obtiene el header asociado de la tabla prOrdenCompraDocu
                DTO_prOrdenCompraDocu ordHeader = this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Get(ord.NumeroDoc.Value.Value);
                _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(ord.NumeroDoc.Value.Value);

                DTO_glDocumentoAprueba docAprueba = this._moduloGlobal.glDocumentoAprueba_UpdateUserApprover(ord.NumeroDoc.Value.Value);
                if (docAprueba == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_UpdateData;
                }
                //Si UsuarioAprueba es null realiza el proceso de Aprobacion final
                else if (docAprueba.UsuarioAprueba.Value == null)
                {
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ord.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, ord.Observacion.Value, true);
                    
                    DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(ord.NumeroDoc.Value.Value);
                    if (ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_MasterHierarchy = (DAL_MasterHierarchy)this.GetInstance(typeof(DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_plPlaneacionProveedor = (DAL_plPlaneacion_Proveedores)this.GetInstance(typeof(DAL_plPlaneacion_Proveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)this.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                        List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(ord.NumeroDoc.Value.Value, false);
                        string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                        foreach (DTO_prDetalleDocu det in listDet)
                        {
                            #region Guarda o actualiza prSaldosDocu(Solicitud-OrdenCompra)
                            //DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                            //saldoOC.EmpresaID.Value = det.EmpresaID.Value;
                            //saldoOC.NumeroDoc.Value = det.NumeroDoc.Value;
                            //saldoOC.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                            //saldoOC.CantidadDocu.Value = det.CantidadOC.Value;
                            //saldoOC.CantidadMovi.Value = 0;

                            //DTO_prSaldosDocu saldoSol = new DTO_prSaldosDocu();
                            //saldoSol = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.SolicitudDetaID.Value.Value);
                            //if (saldoSol != null)
                            //{
                            //    saldoSol.CantidadMovi.Value += det.CantidadOC.Value.Value;
                            //    if (saldoSol.CantidadMovi.Value.Value > saldoSol.CantidadDocu.Value.Value)
                            //    {
                            //        result.Result = ResultValue.NOK;
                            //        result.ResultMessage = "Saldos inválidos en proveedores (prSaldosDocu)";
                            //    }   
                            //    else
                            //    {
                            //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoOC);
                            //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoSol);
                            //    }
                            //}
                            //else
                            //{
                            //    result.Result = ResultValue.NOK;
                            //    result.ResultMessage = "Error al traer saldos(prSaldosDocu): Revisar Codigo";
                            //}
                            #endregion
                            #region Actualiza Tablas(prBienServicio-inReferencia) con Info de la OC
                            DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase,bienServicio.ClaseBSID.Value, true, false);

                            if (bienServicioClase != null)
                            {
                                #region Actualiza prBienServicio
                                bienServicio.DocCompra.Value = ctrl.NumeroDoc.Value;
                                bienServicio.MonCompra.Value = ctrl.MonedaID.Value;
                                bienServicio.VlrCompra.Value = ctrl.Valor.Value;
                                bienServicio.CtrlVersion.Value = bienServicio.CtrlVersion.Value++;
                                this._dal_MasterHierarchy.DocumentID = AppMasters.prBienServicio;
                                result = this._dal_MasterHierarchy.DAL_MasterHierarchy_Update(bienServicio, true);

                                if (result.Result == ResultValue.NOK)
                                    return result;
                                #endregion
                                if (bienServicioClase.TipoCodigo.Value == (byte)TipoCodigo.Inventario)
                                {
                                    this._dal_MasterSimple = (DAL_MasterSimple)this.GetInstance(typeof(DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                    #region Actualiza inReferencia
                                    DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, det.inReferenciaID.Value, true, false);
                                    if (referencia != null)
                                    {
                                        referencia.DocCompra.Value = ctrl.NumeroDoc.Value;
                                        referencia.MonCompra.Value = ctrl.MonedaID.Value;
                                        referencia.VlrCompra.Value = ctrl.Valor.Value;
                                        referencia.CtrlVersion.Value = referencia.CtrlVersion.Value++;
                                        this._dal_MasterSimple.DocumentID = AppMasters.inReferencia;
                                        result = this._dal_MasterSimple.DAL_MasterSimple_Update(referencia, true);
                                        if (result.Result == ResultValue.NOK)
                                            return result; 
                                    }
                                    #endregion
                                }
                            }  
                            #endregion
                            #region Crea info en plPlaneacionProveedores
                            List<DTO_prSolicitudCargos> cargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(det.SolicitudDetaID.Value.Value);
                            if (cargos.Count > 0)
                            {
                                DTO_coProyecto proy = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, cargos.First().ProyectoID.Value, true, false);
                                
                                Dictionary<string, string> pks = new Dictionary<string, string>();
                                pks.Add("ActividadID", proy.ActividadID.Value);
                                pks.Add("LineaPresupuestoID",cargos.First().LineaPresupuestoID.Value);
                                DTO_plActividadLineaPresupuestal actLinea = (DTO_plActividadLineaPresupuestal)this.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pks, true);
                                if (actLinea != null)
                                {
                                    DTO_plPlaneacion_Proveedores planeaProveedor = new DTO_plPlaneacion_Proveedores();
                                    planeaProveedor.ConsActLinea.Value = actLinea.ReplicaID.Value;
                                    planeaProveedor.CodigoBSID.Value = det.CodigoBSID.Value;
                                    planeaProveedor.inReferenciaID.Value = !string.IsNullOrEmpty(det.inReferenciaID.Value)? det.inReferenciaID.Value : refxDefecto;
                                    List<DTO_plPlaneacion_Proveedores> exist = this._dal_plPlaneacionProveedor.DAL_plPlaneacion_Proveedores_GetByParameter(planeaProveedor);
                                    if (exist.Count == 0)
                                    {
                                        try { this._dal_plPlaneacionProveedor.DAL_plPlaneacion_Proveedores_Add(planeaProveedor); }
                                        catch
                                        {
                                            result.ResultMessage = DictionaryMessages.Err_Pl_SavePlaneacionProveedor;
                                            result.Result = ResultValue.NOK;
                                            return result;
                                        } 
                                    }
                                }
                            }
                            #endregion
                        }
                        
                    }
                   
                    #region Presupuesto
                    modulePlaneacionActive = this.GetModuleActive(mod);
                    //Verifica si tiene activo el Modulo de Planeacion
                    if (modulePlaneacionActive)
                    {
                        this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        result = this._moduloPlaneacion.GeneraPresupuesto(AppDocuments.OrdenCompra, ord.NumeroDoc.Value.Value);
                        if (result.Result == ResultValue.NOK)
                            return result;
                    }
                    #endregion
                    this.AsignarFlujo(documentID, ctrl.NumeroDoc.Value.Value, actividadFlujoID, false, ctrl.Observacion.Value);
                }
                else
                {
                    #region Guarda en la bitacora
                    int bId = this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Approve, DateTime.Now,
                        this.UserId, this.Empresa.ID.Value, ord.NumeroDoc.Value.Value.ToString(), documentID.ToString(), string.Empty,
                        string.Empty, string.Empty, 0, 0, 0);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenCompra_Aprobar");

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
                        this._moduloContabilidad._mySqlConnectionTx = null;                       
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza OrdenCompra
        /// </summary>
        /// <param name="sol"> OrdenCompra de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void OrdenCompra_Rechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prOrdenCompraAprob ord, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool isValid = true;
            try
            {
                //Obtiene el header asociado de la tabla prSolicitudDOcu
                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)this.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prOrdenCompraDocu ordHeader = this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Get(ord.NumeroDoc.Value.Value);

                ordHeader.UsuarioRechaza.Value = usuario.ID.Value;
                ordHeader.ObservRechazo.Value = ord.Observacion.Value;
                ordHeader.FechaERechazo.Value = DateTime.Now;

                this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_Upd(ordHeader);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ord.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, ord.Observacion.Value, true);
                this.AsignarFlujo(documentID, ord.NumeroDoc.Value.Value, actividadFlujoID, true, ord.Observacion.Value);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Err_Pr_OrdenCompraRechazar, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "OrdenCompra_Rechazar");
                throw exception;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        this._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// LLena DTO_prSolicitudFooter con los datos de DTO_prDetalleDocu y DTO_prSolicitudCargos
        /// </summary>
        /// <param name="detail">la lista de DTO_prDetalleDocu</param>
        /// <param name="cargos">la lista de DTO_prSolicitudCargos</param>
        /// <returns></returns>
        private List<DTO_prOrdenCompraFooter> OrdenCompraFooter_Load(List<DTO_prDetalleDocu> detail)
        {
            #region Variables
            List<DTO_prOrdenCompraFooter> ordenFooter = new List<DTO_prOrdenCompraFooter>();
            DTO_prOrdenCompraFooter ordenItem;
            List<DTO_prSolicitudCargos> cargos;
            Dictionary<int, DTO_prSolicitudDocu> cacheSol = new Dictionary<int, DTO_prSolicitudDocu>();
            DTO_prSolicitudDocu _solDocu;

            int index;
            string msg = string.Empty;

            this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            foreach (DTO_prDetalleDocu det in detail)
            {
                ordenItem = new DTO_prOrdenCompraFooter();
                index = 0;

                #region Trae Solicitud correspondiente
                if (cacheSol.ContainsKey(det.SolicitudDocuID.Value.Value))
                    _solDocu = cacheSol[det.SolicitudDocuID.Value.Value];
                else
                {
                    _solDocu = this.prSolicitudDocu_Get(det.SolicitudDocuID.Value.Value);
                    cacheSol.Add(det.SolicitudDocuID.Value.Value, _solDocu);
                }
                #endregion

                ordenItem.DetalleDocu = det;

                cargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(det.SolicitudDetaID.Value.Value);

                foreach (DTO_prSolicitudCargos cargo in cargos)
                {
                    cargo.Index = index;
                    cargo.IndexDet = ordenItem.DetalleDocu.Index;
                    ordenItem.SolicitudCargos.Add(cargo);
                    index++;
                }

                ordenFooter.Add(ordenItem);
            }

            return ordenFooter;
        }
        #endregion

        #region Funciones publicas

        /// <summary>
        /// Guardar nuevo Orden de Compra o Contrato y asociar en glDocumentoControl
        /// </summary>
        /// <param name="orden">Data completa</param>
        /// <returns></returns>
        public DTO_SerializedObject OrdenCompra_Guardar(int documentID, DTO_prOrdenCompra orden, bool update, out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Declara y carga las variables
            DTO_glDocumentoControl docCtrlExist = null;
            DTO_glDocumentoControl ctrl = orden.DocCtrl;
            DTO_prOrdenCompraDocu headerOrden = orden.HeaderOrdenCompra;
            List<DTO_prOrdenCompraCotiza> cotiza = orden.Cotizacion;
            List<DTO_prOrdenCompraFooter> footer = orden.Footer;
            DTO_prContratoDocu headerContrato = documentID != AppDocuments.OrdenCompra ? orden.HeaderContrato : new DTO_prContratoDocu();
            List<DTO_prConvenio> convenio = documentID != AppDocuments.OrdenCompra ? orden.Convenio : new List<DTO_prConvenio>();
            List<DTO_prContratoPolizas> polizas = orden.Polizas != null? orden.Polizas : new List<DTO_prContratoPolizas>();
            List<DTO_prContratoPlanPago> contratoPlanPago = orden.ContratoPlanPagos != null ? orden.ContratoPlanPagos : new List<DTO_prContratoPlanPago>();
            #endregion
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    ctrl.DocumentoNro.Value = 0;
                    ctrl.ComprobanteIDNro.Value = 0;
                    ctrl.Observacion.Value = documentID == AppDocuments.OrdenCompra ? orden.HeaderOrdenCompra.Observaciones.Value : orden.HeaderContrato.Observaciones.Value;
                    ctrl.Valor.Value = documentID == AppDocuments.OrdenCompra ? orden.HeaderOrdenCompra.Valor.Value : orden.HeaderContrato.Valor.Value;
                    ctrl.Iva.Value = documentID == AppDocuments.OrdenCompra ? orden.HeaderOrdenCompra.IVA.Value : orden.HeaderContrato.IVA.Value;

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
                    #region Guardar en prOrdenCompraDocu o prContratoDocu
                    if (documentID == AppDocuments.OrdenCompra)
                    {
                        headerOrden.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        result = this.prOrdenCompraDocu_Add(headerOrden, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }
                    }
                    else
                    {
                        headerContrato.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        result = this.prContratoDocu_Add(headerContrato, ctrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en prDetalleDocu
                    foreach (DTO_prOrdenCompraFooter itemFooter in footer)
                    {
                        if (documentID == AppDocuments.OrdenCompra)
                        {
                            DTO_prDetalleDocu det = itemFooter.DetalleDocu;
                            det.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            det.OrdCompraDocuID.Value = det.NumeroDoc.Value;
                            det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(itemFooter.DetalleDocu);
                            det.OrdCompraDetaID.Value = det.ConsecutivoDetaID.Value;
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(itemFooter.DetalleDocu);
                        }
                        else
                        {
                            DTO_prDetalleDocu det = itemFooter.DetalleDocu;
                            det.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            det.ContratoDocuID.Value = det.NumeroDoc.Value;
                            det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(itemFooter.DetalleDocu);
                            det.ContratoDetaID.Value = det.ConsecutivoDetaID.Value;
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(itemFooter.DetalleDocu);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en prOrdenCompraCotiza
                    cotiza.ForEach(cot => cot.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key));
                    if (cotiza.Count > 0)
                        result = this.prOrdenCompraCotiza_Add(cotiza);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Guardar en prContratoPolizas
                    polizas.ForEach(pol => pol.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key));
                    if (polizas.Count > 0)
                        result = this.ContratoPolizas_Add(polizas);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Guardar en prContratoPlanPago
                    contratoPlanPago.ForEach(conv => conv.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key));
                    if (contratoPlanPago.Count > 0)
                        result = this.prContratoPlanPago_Add(contratoPlanPago);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    if (documentID != AppDocuments.OrdenCompra)
                    {
                        #region Guardar en prConvenio
                        convenio.ForEach(conv => conv.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key));
                        if (convenio.Count > 0)
                            result = this.prConvenio_Add(convenio);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }
                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                        #endregion 
                    }
                    numeroDoc = Convert.ToInt32(resultGLDC.Key);
                }
                else
                {
                    docCtrlExist = this._moduloGlobal.glDocumentoControl_GetByID(ctrl.NumeroDoc.Value.Value);
                    #region Actualiza glDocumentoControl
                    docCtrlExist.MonedaID.Value = ctrl.MonedaID.Value;
                    docCtrlExist.CuentaID.Value = ctrl.CuentaID.Value;
                    docCtrlExist.ProyectoID.Value = ctrl.ProyectoID.Value;
                    docCtrlExist.CentroCostoID.Value = ctrl.CentroCostoID.Value;
                    docCtrlExist.LineaPresupuestoID.Value = ctrl.LineaPresupuestoID.Value;
                    docCtrlExist.LugarGeograficoID.Value = ctrl.LugarGeograficoID.Value;
                    docCtrlExist.Fecha.Value = ctrl.Fecha.Value;
                    docCtrlExist.FechaDoc.Value = ctrl.FechaDoc.Value;
                    docCtrlExist.TasaCambioDOCU.Value = ctrl.TasaCambioDOCU.Value;
                    docCtrlExist.TasaCambioCONT.Value = ctrl.TasaCambioCONT.Value;
                    docCtrlExist.Observacion.Value = ctrl.Observacion.Value;
                    docCtrlExist.Valor.Value = ctrl.Valor.Value;
                    docCtrlExist.Iva.Value = ctrl.Iva.Value;
                    docCtrlExist.Observacion.Value = documentID == AppDocuments.OrdenCompra ? headerOrden.Observaciones.Value : headerOrden.Observaciones.Value;
                    this._moduloGlobal.glDocumentoControl_Update(docCtrlExist, true, true);
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrl.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, ctrl.Observacion.Value, true);

                    List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(docCtrlExist.DocumentoID.Value.Value);
                    if (act.Count > 0)
                        this.AsignarFlujo(docCtrlExist.DocumentoID.Value.Value, ctrl.NumeroDoc.Value.Value, act[0], false, string.Empty);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza prOrdenCompraDocu o prContratoDocu
                    if (documentID == AppDocuments.OrdenCompra)
                        result = this.prOrdenCompraDocu_Upd(headerOrden, ctrl.DocumentoID.Value.Value);
                    else
                        result = this.prContratoDocu_Upd(headerContrato, ctrl.DocumentoID.Value.Value);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = ctrl.NumeroDoc.Value.Value;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                    #region Actualiza en prDetalleDocu
                    List<DTO_prDetalleDocu> detDocu = new List<DTO_prDetalleDocu>();
                    detDocu = this.prDetalleDocu_GetByNumeroDoc(ctrl.NumeroDoc.Value.Value, false);

                    foreach (DTO_prOrdenCompraFooter itemFooter in footer)
                    {
                        DTO_prDetalleDocu det = itemFooter.DetalleDocu;
                        if (detDocu.Exists(x => x.ConsecutivoDetaID.Value.Equals(det.ConsecutivoDetaID.Value)))
                        {
                            this.prDetalleDocu_Update(documentID, det, false, true);
                            detDocu.RemoveAll(x => x.ConsecutivoDetaID.Value.Equals(det.ConsecutivoDetaID.Value));
                        }
                        else
                        {
                            if (documentID == AppDocuments.OrdenCompra)
                            {
                                det.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                                det.OrdCompraDocuID.Value =  det.NumeroDoc.Value;
                                det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                                det.OrdCompraDetaID.Value = det.ConsecutivoDetaID.Value;
                            }
                            else
                            {
                                det.NumeroDoc.Value = ctrl.NumeroDoc.Value;
                                det.ContratoDocuID.Value = det.NumeroDoc.Value;
                                det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                                det.ContratoDetaID.Value = det.ConsecutivoDetaID.Value;
                            }
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                        }
                    }
                    if (detDocu.Count > 0)
                    {
                        foreach (DTO_prDetalleDocu det in detDocu)
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Delete(det.ConsecutivoDetaID.Value.Value);
                    }

                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion;
                    #region Actualiza en prOrdenCompraCotiza
                    this.prOrdenCompraCotiza_Delete(docCtrlExist.NumeroDoc.Value.Value);
                    result = this.prOrdenCompraCotiza_Add(cotiza);
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion;
                    #region Actualiza en prContratoPlanPago
                    if (contratoPlanPago.Count > 0)
                    {
                        contratoPlanPago.ForEach(cont => cont.NumeroDoc.Value = ctrl.NumeroDoc.Value.Value);
                        result = this.prContratoPlanPago_Upd(contratoPlanPago, ctrl.NumeroDoc.Value.Value);
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Guardar en prContratoPolizas
                    if (polizas.Count > 0)
                    {
                        polizas.ForEach(pol => pol.NumeroDoc.Value = ctrl.NumeroDoc.Value.Value);
                        result = this.ContratoPolizas_Upd(polizas, ctrl.NumeroDoc.Value.Value);
                    }
                    if (result.Result == ResultValue.NOK)
                    {
                        numeroDoc = 0;
                        return result;
                    }
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    if (documentID != AppDocuments.OrdenCompra)
                    {
                        #region Actualiza en prConvenio
                        if (convenio.Count > 0)
                        {
                            convenio.ForEach(conv => conv.NumeroDoc.Value = ctrl.NumeroDoc.Value.Value);
                            result = this.prConvenio_Upd(convenio);
                        }                          
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            return result;
                        }
                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                        #endregion
                    }
                    numeroDoc = ctrl.NumeroDoc.Value.Value;
                }
                ctrl.NumeroDoc.Value = numeroDoc;

                #region Guardar o actualiza en glDocumentoAprueba
                result = this._moduloGlobal.glDocumentoAprueba_AddByNivelApr(documentID, numeroDoc, ctrl.Valor.Value.Value);
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    return result;
                }
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    alarma.NumeroDoc = numeroDoc.ToString();
                    return alarma;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                ctrl.NumeroDoc.Value = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenCompra_Guardar");
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

                        if (!update)
                        {
                            ctrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, ctrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(ctrl, true, false, true);
                            alarma.Consecutivo = ctrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Carga la informacion completa del Orden de compra
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Consecutivo del Orden de Compra</param>
        /// <param name="numeroDoc">numero Doc de la OC</param>
        /// <returns>Retorna datos del Orden de compra</returns>
        public DTO_prOrdenCompra OrdenCompra_Load(int documentID, string prefijoID, int ordenNro, int NumeroDoc = 0)
        {
            try
            {
                DTO_prOrdenCompra orden = new DTO_prOrdenCompra();

                //Trae glDocumentoControl
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = null;
                if (NumeroDoc == 0)
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, prefijoID, ordenNro);
                else
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(NumeroDoc);

                //Si no existe devuelve null
                if (docCtrl != null)
                    orden.DocCtrl = docCtrl;
                else
                    return null;

                //Obtenga datos de la tabla prDetalleDocu
                List<DTO_prDetalleDocu> detDocu = this.prDetalleDocu_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value, false);

                //Llena detalle del orden de compra (DTO_prOrdenCompraFooter)
                List<DTO_prOrdenCompraFooter> ordenFooter = this.OrdenCompraFooter_Load(detDocu);
                orden.Footer = ordenFooter;

                //Carga prOrdenCompraCotiza
                List<DTO_prOrdenCompraCotiza> ordenCotiza = this.prOrdenCompraCotiza_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                orden.Cotizacion = ordenCotiza;

                //Carga prConvenio
                List<DTO_prContratoPlanPago> contratoPlanPago = this.prContratoPlanPago_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                orden.ContratoPlanPagos = contratoPlanPago;

                if (documentID == AppDocuments.OrdenCompra)
                {
                    //Carga prOrdenCompraDocu
                    DTO_prOrdenCompraDocu ordenHeader = this.prOrdenCompraDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    ordenHeader.ProyectoID.Value = docCtrl.ProyectoID.Value;
                    orden.HeaderOrdenCompra = ordenHeader;
                }
                else
                {
                    //ó Carga prContratoDocu
                    DTO_prContratoDocu contratoHeader = this.prContratoDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    orden.HeaderContrato = contratoHeader;

                    //Carga prConvenio
                    List<DTO_prConvenio> convenios = this.prConvenio_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                    orden.Convenio = convenios;    
                }
                return orden;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "OrdenCompra_Load");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de los ordenes de compra pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraAprob> OrdenCompra_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                DTO_glActividadPermiso actPerm = null;

                DTO_glDocumento doc = (DTO_glDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);

                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)base.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_prOrdenCompraAprob> list = this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_GetPendientesByModulo(doc, actFlujoID, actPerm, usuario);

                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                foreach (DTO_prOrdenCompraAprob item in list)
                {
                    item.OrdenCompraAprobDet = item.OrdenCompraAprobDet.OrderBy(x => x.Descriptivo.Value).ToList();
                    foreach (DTO_prOrdenCompraAprobDet itemDet in item.OrdenCompraAprobDet)
                        itemDet.SolicitudCargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(itemDet.SolicitudDetaID.Value.Value);
                    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                }

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenCompra_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de ordenes de compra para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> OrdenCompra_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prOrdenCompraAprob> ord, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in ord)
                {
                    porcTemp = (porcParte * i) / ord.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.OrdenCompra_Aprobar(documentID, actividadFlujoID, usuario, item, createDoc, batchProgress, porcTotal, porcParte, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "OrdenCompra_Aprobar");
                            rd.Message = DictionaryMessages.Err_Pr_OrdenCompraAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.OrdenCompra_Rechazar(documentID, actividadFlujoID, usuario, item, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "OrdenCompra_Rechazar");
                            rd.Message = DictionaryMessages.Err_Pr_OrdenCompraRechazar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }

                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenCompra_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Trae un listado de los Ordenes de compra para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prOrdenCompraResumen> OrdenCompra_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, List<Tuple<string, string>> filtros)
        {
            try
            {
                Dictionary<string, TipoCodigo> cacheBS = new Dictionary<string, TipoCodigo>();
                Dictionary<string, bool> cacheRef = new Dictionary<string, bool>();
                TipoCodigo tipoCodigo;
                bool serialInd = false;

                this._dal_prOrdenCompraDocu = (DAL_prOrdenCompraDocu)base.GetInstance(typeof(DAL_prOrdenCompraDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_prOrdenCompraResumen> list = this._dal_prOrdenCompraDocu.DAL_prOrdenCompraDocu_GetResumen(documentID, usuario, mod, filtros);
                List<DTO_prOrdenCompraResumen> newList = new List<DTO_prOrdenCompraResumen>();

                int index = 0;
                foreach (DTO_prOrdenCompraResumen item in list)
                {
                    if (cacheBS.ContainsKey(item.CodigoBSID.Value))
                        tipoCodigo = cacheBS[item.CodigoBSID.Value];
                    else
                    {
                        DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, item.CodigoBSID.Value, true, false);
                        DTO_glBienServicioClase bsc = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                        tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsc.TipoCodigo.Value.Value.ToString());
                        cacheBS.Add(item.CodigoBSID.Value, tipoCodigo);
                    };
                    item.ClaseBS = tipoCodigo;

                    if (tipoCodigo == TipoCodigo.Inventario || tipoCodigo == TipoCodigo.Activo)
                    {
                        if (cacheRef.ContainsKey(item.inReferenciaID.Value))
                            serialInd = cacheRef[item.inReferenciaID.Value];
                        else
                        {
                            DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, item.inReferenciaID.Value, true, false);
                            DTO_inRefTipo tipoRef = (DTO_inRefTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, refer.TipoInvID.Value, true, false);
                            serialInd = tipoRef.SerializadoInd.Value.Value;
                            cacheRef.Add(item.inReferenciaID.Value, serialInd);
                        }

                        if (serialInd && item.CantidadOC.Value.Value >= 1)
                        {
                            int count = (int)item.CantidadOC.Value.Value;

                            for (int i = 0; i < count; i++)
                            {
                                DTO_prOrdenCompraResumen itemInv = new DTO_prOrdenCompraResumen();
                                itemInv.Index = index;
                                itemInv.invSerialInd = true;
                                itemInv.NumeroDoc.Value = item.NumeroDoc.Value.Value;
                                itemInv.ConsecutivoDetaID.Value = item.ConsecutivoDetaID.Value.Value;
                                itemInv.SolicitudDocuID.Value = item.SolicitudDocuID.Value.Value;
                                itemInv.SolicitudDetaID.Value = item.SolicitudDetaID.Value.Value;
                                itemInv.PrefijoIDSol.Value = item.PrefijoIDSol.Value;
                                itemInv.DocumentoNroSol.Value = item.DocumentoNroSol.Value.Value;
                                itemInv.PrefDocSol = item.PrefDocSol;
                                itemInv.OrdCompraDocuID.Value = item.OrdCompraDocuID.Value.Value;
                                itemInv.OrdCompraDetaID.Value = item.OrdCompraDetaID.Value.Value;
                                itemInv.Documento1ID.Value = item.Documento1ID.Value;
                                itemInv.Detalle1ID.Value = item.Detalle1ID.Value;
                                itemInv.CantidadDoc1.Value = item.CantidadDoc1.Value;
                                itemInv.Documento2ID.Value = item.Documento2ID.Value;
                                itemInv.Detalle2ID.Value = item.Detalle2ID.Value;
                                itemInv.CantidadDoc2.Value = item.CantidadDoc2.Value;
                                itemInv.Documento3ID.Value = item.Documento3ID.Value;
                                itemInv.Detalle3ID.Value = item.Detalle3ID.Value;
                                itemInv.CantidadDoc3.Value = item.CantidadDoc3.Value;
                                itemInv.Documento4ID.Value = item.Documento4ID.Value;
                                itemInv.Detalle4ID.Value = item.Detalle4ID.Value;
                                itemInv.CantidadDoc4.Value = item.CantidadDoc4.Value;
                                itemInv.Documento5ID.Value = item.Documento5ID.Value;
                                itemInv.Detalle5ID.Value = item.Detalle5ID.Value;
                                itemInv.CantidadDoc5.Value = item.CantidadDoc5.Value;
                                itemInv.PrefijoIDOC.Value = item.PrefijoIDOC.Value;
                                itemInv.PrefDocOC = item.PrefDocOC;
                                itemInv.DocumentoNroOC.Value = item.DocumentoNroOC.Value.Value;
                                itemInv.FechaOC.Value = item.FechaOC.Value.Value;
                                itemInv.CodigoBSID.Value = item.CodigoBSID.Value;
                                itemInv.inReferenciaID.Value = item.inReferenciaID.Value;
                                itemInv.LineaPresupuestoID.Value = item.LineaPresupuestoID.Value;
                                itemInv.Descriptivo.Value = item.Descriptivo.Value;
                                itemInv.CantidadSol.Value = serialInd ? 1 : item.CantidadSol.Value.Value;
                                itemInv.CantidadRec.Value = item.CantidadRec.Value.Value;
                                itemInv.MonedaIDOC.Value = item.MonedaIDOC.Value;
                                itemInv.SerialID.Value = item.SerialID.Value;
                                itemInv.UnidadInvID.Value = item.UnidadInvID.Value;
                                itemInv.CantidadxEmpaque.Value = item.CantidadxEmpaque.Value;
                                itemInv.UnidadEmpaque.Value = item.UnidadEmpaque.Value;
                                itemInv.CantidadOC.Value = 1;
                                itemInv.ClaseBS = TipoCodigo.Inventario;
                                newList.Add(itemInv);
                                index++;
                            }
                        }
                        else
                        {
                            item.Index = index;
                            newList.Add(item);
                            index++;
                        }
                    }
                    else
                    {
                        item.Index = index;
                        newList.Add(item);
                        index++;
                    }
                }

                return newList;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "OrdenCompraDocu_GetResumen");
                return null;
            }
        }
        #endregion
        #endregion

        #region Contrato

        #region prContratoDocu

        /// <summary>
        /// Adiciona en la tabla prContratoDocu 
        /// </summary>
        /// <param name="fact">DTO_prContratoDocu</param>
        /// <returns></returns>
        private DTO_TxResult prContratoDocu_Add(DTO_prContratoDocu ordenDocu, int documentID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prContratoDocu = (DAL_prContratoDocu)base.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prContratoDocu.DAL_prContratoDocu_Add(ordenDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, ordenDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prContratoDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prSolicitudDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        private DTO_prContratoDocu prContratoDocu_Get(int NumeroDoc)
        {
            this._dal_prContratoDocu = (DAL_prContratoDocu)base.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prContratoDocu.DAL_prContratoDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prContratoDocu_Upd(DTO_prContratoDocu ordenDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prContratoDocu = (DAL_prContratoDocu)base.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prContratoDocu.DAL_prContratoDocu_Upd(ordenDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, ordenDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prContratoDocu_Upd");
                return result;
            }
        }
        #endregion

        #region prConvenio

        #region Funciones Privadas

        /// <summary>
        /// Guarda la lista de prConvenio en base de datos
        /// </summary>
        /// <param name="listConvenios">la lista de DTO_prConvenio</param>
        /// <returns></returns>
        private DTO_TxResult prConvenio_Add(List<DTO_prConvenio> listConvenios)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_prConvenio = (DAL_prConvenio)base.GetInstance(typeof(DAL_prConvenio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var convenio in listConvenios)
                    this._dal_prConvenio.DAL_prConvenio_Add(convenio);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prConvenio_Add");
                return result;
            }
        }

        /// <summary>
        /// Actualiza la lista de prConvenio en base de datos
        /// </summary>
        /// <param name="listConvenios">la lista de DTO_prConvenio</param>
        /// <returns></returns>
        private DTO_TxResult prConvenio_Upd(List<DTO_prConvenio> listConvenios)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this._dal_prConvenio = (DAL_prConvenio)base.GetInstance(typeof(DAL_prConvenio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var convenio in listConvenios)
                    this._dal_prConvenio.DAL_prConvenio_Upd(convenio);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prConvenio_Upd");
                return result;
            }
        }

        /// <summary>
        /// Trae la lista de prConvenio segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prConvenio> prConvenio_GetByNumeroDoc(int NumeroDoc)
        {
            this._dal_prConvenio = (DAL_prConvenio)base.GetInstance(typeof(DAL_prConvenio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prConvenio.DAL_prConvenio_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenio
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        private void prConvenio_Delete(int NumeroDoc)
        {
            this._dal_prConvenio = (DAL_prConvenio)base.GetInstance(typeof(DAL_prConvenio), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prConvenio.DAL_prConvenio_Delete(NumeroDoc);
        }

        #endregion

        #endregion

        #region prContratoPlanPago

        #region Funciones Privadas

        /// <summary>
        /// Guarda la lista de prOrdenCompraCotiza en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        private DTO_TxResult prContratoPlanPago_Add(List<DTO_prContratoPlanPago> listContPlanPago)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this.dal_prContratoPlanPago = (DAL_prContratoPlanPago)base.GetInstance(typeof(DAL_prContratoPlanPago), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                foreach (var pago in listContPlanPago)
                    this.dal_prContratoPlanPago.DAL_prContratoPlanPago_Add(pago);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prContratoPlanPago_Add");
                return result;
            }
        }

        /// <summary>
        /// Trae la lista de prContratoPlanPago segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prContratoPlanPago> prContratoPlanPago_GetByNumeroDoc(int NumeroDoc)
        {
            this.dal_prContratoPlanPago = (DAL_prContratoPlanPago)base.GetInstance(typeof(DAL_prContratoPlanPago), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this.dal_prContratoPlanPago.DAL_prContratoPlanPago_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Elimina registros de la tabla de prContratoPlanPago
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        private void prContratoPlanPago_Delete(int NumeroDoc)
        {
            this.dal_prContratoPlanPago = (DAL_prContratoPlanPago)base.GetInstance(typeof(DAL_prContratoPlanPago), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this.dal_prContratoPlanPago.DAL_prContratoPlanPago_Delete(NumeroDoc);
        }

        #endregion

        /// <summary>
        /// Actualiza la lista de prContratoPlanPago en base de datos
        /// </summary>
        /// <param name="listContPlanPago">la lista de DTO_prContratoPlanPago</param>
        /// <returns></returns>
        public DTO_TxResult prContratoPlanPago_Upd(List<DTO_prContratoPlanPago> listContPlanPago, int numeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {
                this.prContratoPlanPago_Delete(numeroDoc);
                result = this.prContratoPlanPago_Add(listContPlanPago);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prContratoPlanPago_Upd");
                return result;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Aprueba Orden de Compra
        /// </summary>
        /// <param name="contrato"> Orden de Compra de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult Contrato_Aprobar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prContratoAprob contrato, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, decimal porcTotal, decimal porcParte, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            int numeroDocOC = 0;
            DTO_SerializedObject resultOrden = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            ModulesPrefix mod = ModulesPrefix.pl;
            bool modulePlaneacionActive = false;

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Obtiene el header asociado de la tabla prSolicitudDocu
                this._dal_prContratoDocu = (DAL_prContratoDocu)this.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prOrdenCompra contratoExist = this.OrdenCompra_Load(AppDocuments.Contrato, contrato.PrefijoID.Value, contrato.DocumentoNro.Value.Value);

                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, contrato.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, contrato.Observacion.Value, true);

                #region Genera registro de Contrato y Actualiza el registro de Solicitud en prSaldosDocu
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(contrato.NumeroDoc.Value.Value);
                if (ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                {
                    this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    // List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(contrato.NumeroDoc.Value.Value, false);

                    //foreach (DTO_prDetalleDocu det in listDet)
                    //{
                    //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                    //    saldoOC.EmpresaID.Value = det.EmpresaID.Value;
                    //    saldoOC.NumeroDoc.Value = det.NumeroDoc.Value;
                    //    saldoOC.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                    //    saldoOC.CantidadDocu.Value = det.CantidadOC.Value;
                    //    saldoOC.CantidadMovi.Value = 0;

                    //    DTO_prSaldosDocu saldoSol = new DTO_prSaldosDocu();
                    //    saldoSol = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.SolicitudDetaID.Value.Value);
                    //    saldoSol.CantidadMovi.Value += det.CantidadOC.Value.Value;
                    //    if (saldoSol.CantidadMovi.Value.Value > saldoSol.CantidadDocu.Value.Value)
                    //    {
                    //        result.Result = ResultValue.NOK;
                    //        result.ResultMessage = DictionaryMessages.Err_Pr_SaldoInvalid;
                    //        return result;
                    //    }
                    //    else
                    //    {
                    //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoOC);
                    //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoSol);
                    //    }
                    //}
                }
                #endregion
                #region Crea una orden de compra
                DTO_prOrdenCompra ordenNew = ObjectCopier.Clone(contratoExist);
                #region Load DocumentoControl
                ordenNew.DocCtrl.DocumentoID.Value = AppDocuments.OrdenCompra;
                ordenNew.DocCtrl.NumeroDoc.Value = 0;
                ordenNew.DocCtrl.ComprobanteIDNro.Value = 0;
                ordenNew.DocCtrl.DocumentoNro.Value = 0;
                ordenNew.DocCtrl.Descripcion.Value = "ORDEN COMPRA PROVEEDORES(Contrato)";
                ordenNew.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                #endregion
                #region Load OrdenCompraHeader
                ordenNew.HeaderOrdenCompra.NumeroDoc.Value = 0;
                ordenNew.HeaderOrdenCompra.EmpresaID.Value = this.Empresa.ID.Value;
                ordenNew.HeaderOrdenCompra.ProveedorID.Value = contratoExist.HeaderContrato.ProveedorID.Value;
                ordenNew.HeaderOrdenCompra.ContratoNro.Value = contratoExist.DocCtrl.NumeroDoc.Value;
                ordenNew.HeaderOrdenCompra.MonedaOrden.Value = contratoExist.HeaderContrato.MonedaOrden.Value;
                ordenNew.HeaderOrdenCompra.MonedaPago.Value = contratoExist.HeaderContrato.MonedaPago.Value;
                ordenNew.HeaderOrdenCompra.LugarEntrega.Value = contratoExist.HeaderContrato.LugarEntrega.Value;
                ordenNew.HeaderOrdenCompra.FechaEntrega.Value = contratoExist.HeaderContrato.FechaPago1.Value;
                ordenNew.HeaderOrdenCompra.TerminosInd.Value = contratoExist.HeaderContrato.TerminosInd.Value;
                ordenNew.HeaderOrdenCompra.PagoVariablend.Value = contratoExist.HeaderContrato.PagoVariableInd.Value;
                ordenNew.HeaderOrdenCompra.VlrAnticipo.Value = contratoExist.HeaderContrato.VlrAnticipo.Value;
                ordenNew.HeaderOrdenCompra.DtoProntoPago.Value = contratoExist.HeaderContrato.DtoProntoPago.Value;
                ordenNew.HeaderOrdenCompra.DiasPtoPago.Value = contratoExist.HeaderContrato.DiasPtoPago.Value;
                ordenNew.HeaderOrdenCompra.FormaPago.Value = contratoExist.HeaderContrato.FormaPago.Value;
                ordenNew.HeaderOrdenCompra.Instrucciones.Value = contratoExist.HeaderContrato.Instrucciones.Value;
                ordenNew.HeaderOrdenCompra.Observaciones.Value = contratoExist.HeaderContrato.Observaciones.Value;
                ordenNew.HeaderOrdenCompra.ObservRechazo.Value = contratoExist.HeaderContrato.ObservRechazo.Value;
                ordenNew.HeaderOrdenCompra.FechaERechazo.Value = contratoExist.HeaderContrato.FechaERechazo.Value;
                ordenNew.HeaderOrdenCompra.TasaOrden.Value = contratoExist.HeaderContrato.TasaOrden.Value;
                ordenNew.HeaderOrdenCompra.UsuarioRechaza.Value = contratoExist.HeaderContrato.UsuarioRechaza.Value;
                ordenNew.HeaderOrdenCompra.PorcentAdministra.Value = contratoExist.HeaderContrato.PorcentAdministra.Value;
                ordenNew.HeaderOrdenCompra.PorcentHolgura.Value = contratoExist.HeaderContrato.PorcentHolgura.Value;
                ordenNew.HeaderOrdenCompra.Porcentimprevisto.Value = contratoExist.HeaderContrato.Porcentimprevisto.Value;
                ordenNew.HeaderOrdenCompra.PorcentUtilidad.Value = contratoExist.HeaderContrato.PorcentUtilidad.Value;
                ordenNew.HeaderOrdenCompra.IncluyeAUICosto.Value = contratoExist.HeaderContrato.IncluyeAUICosto.Value;
                //ordenNew.HeaderOrdenCompra.Vigencia.Value = contratoExist.HeaderContrato.Vigencia.Value;
                ordenNew.HeaderOrdenCompra.AreaAprobacion.Value = contratoExist.HeaderContrato.AreaAprobacion.Value;
                ordenNew.HeaderOrdenCompra.UsuarioSolicita.Value = contratoExist.HeaderContrato.UsuarioSolicita.Value;
                ordenNew.HeaderOrdenCompra.Prioridad.Value = contratoExist.HeaderContrato.Prioridad.Value;
                ordenNew.HeaderOrdenCompra.Valor.Value = contratoExist.HeaderContrato.Valor.Value;
                ordenNew.HeaderOrdenCompra.IVA.Value = contratoExist.HeaderContrato.IVA.Value;
                ordenNew.HeaderOrdenCompra.VlrPagoMes.Value = contratoExist.HeaderContrato.VlrPagoMes.Value;
                ordenNew.HeaderOrdenCompra.NroPagos.Value = contratoExist.HeaderContrato.NroPagos.Value;
                ordenNew.HeaderOrdenCompra.FechaPago1.Value = contratoExist.HeaderContrato.FechaPago1.Value;
                ordenNew.HeaderOrdenCompra.FechaVencimiento.Value = contratoExist.HeaderContrato.FechaVencimiento.Value;
                #endregion
                #region Load Cotizacion
                ordenNew.Cotizacion = contratoExist.Cotizacion;
                foreach (var cot in ordenNew.Cotizacion)
                    cot.NumeroDoc.Value = 0;
                #endregion
                #region Load Footer
                ordenNew.Footer = contratoExist.Footer;
                foreach (var tmp in ordenNew.Footer)
                {
                    DTO_prDetalleDocu footer = (DTO_prDetalleDocu)tmp.DetalleDocu;
                    footer.CantidadOC.Value = footer.CantidadCont.Value;
                    footer.NumeroDoc.Value = 0;
                    footer.OrdCompraDocuID.Value = 0;
                    //footer.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(footer);
                    //footer.OrdCompraDetaID.Value = footer.ConsecutivoDetaID.Value;
                    //this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(footer);

                    //List<DTO_prSolicitudCargos> cargos = (List<DTO_prSolicitudCargos>)tmp.SolicitudCargos;
                    //foreach (var cargo in cargos)
                    //{
                    //    cargo.ConsecutivoDetaID.Value = footer.ConsecutivoDetaID.Value;
                    //    cargo.NumeroDoc.Value = 0;
                    //}
                }
                #endregion
                #region Guarda Orden de Compra
                resultOrden = this.OrdenCompra_Guardar(AppDocuments.OrdenCompra, ordenNew, false, out numeroDocOC, batchProgress, true);
                if (resultOrden.GetType() == result.GetType())
                {
                    DTO_TxResult res = (DTO_TxResult)resultOrden;
                    if (res.Result == ResultValue.NOK)
                        return result;
                }
                #endregion
                #region Genera registro de Orden Compra y Actualiza el registro de Solicitud en prSaldosDocu
                //foreach (var tmp in ordenNew.Footer)
                //{
                //    DTO_prDetalleDocu det = (DTO_prDetalleDocu)tmp.DetalleDocu;
                //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                //    saldoOC.EmpresaID.Value = det.EmpresaID.Value;
                //    saldoOC.NumeroDoc.Value = det.NumeroDoc.Value;
                //    saldoOC.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                //    saldoOC.CantidadDocu.Value = det.CantidadOC.Value;
                //    saldoOC.CantidadMovi.Value = 0;

                //    DTO_prSaldosDocu saldoSol = new DTO_prSaldosDocu();
                //    saldoSol = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.SolicitudDetaID.Value.Value);
                //    saldoSol.CantidadMovi.Value += det.CantidadOC.Value.Value;
                //    if (saldoSol.CantidadMovi.Value.Value > saldoSol.CantidadDocu.Value.Value)
                //    {
                //        result.Result = ResultValue.NOK;
                //        result.ResultMessage = "Saldos inválidos en proveedores (prSaldosDocu)";
                //    }   
                //    else
                //    {
                //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoOC);
                //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoSol);
                //    }
                //}
                #endregion
                #endregion
                #region Presupuesto
                modulePlaneacionActive = this.GetModuleActive(mod);
                //Verifica si tiene activo el Modulo de Planeacion
                if (modulePlaneacionActive)
                {
                    this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    result = this._moduloPlaneacion.GeneraPresupuesto(AppDocuments.OrdenCompra, numeroDocOC);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }               

                #endregion
                this.AsignarFlujo(documentID, ctrl.NumeroDoc.Value.Value, actividadFlujoID, false, ctrl.Observacion.Value);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Contrato_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK && !insideAnotherTx)
                {
                    base._mySqlConnectionTx.Commit();
                    base._mySqlConnectionTx = null;
                    this._moduloGlobal._mySqlConnectionTx = null;
                    this._moduloContabilidad._mySqlConnectionTx = null;

                    #region Genera consecutivo para el documento de orden de compra
                    if (resultOrden != null)
                    {
                        DTO_Alarma res = (DTO_Alarma)resultOrden;
                        int numDoc = Convert.ToInt32(res.NumeroDoc);
                        DTO_glDocumentoControl docCtrlOrdenCompra = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                        docCtrlOrdenCompra.DocumentoNro.Value = this.GenerarDocumentoNro(docCtrlOrdenCompra.DocumentoID.Value.Value, docCtrlOrdenCompra.PrefijoID.Value);                     
                        this._moduloGlobal.ActualizaConsecutivos(docCtrlOrdenCompra, true, false, false);
                    }

                    #endregion
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza OrdenCompra
        /// </summary>
        /// <param name="sol"> OrdenCompra de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void Contrato_Rechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prContratoAprob contrato, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool isValid = true;
            try
            {
                //Obtiene el header asociado de la tabla prSolicitudDOcu
                this._dal_prContratoDocu = (DAL_prContratoDocu)this.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prContratoDocu contratoHeader = this._dal_prContratoDocu.DAL_prContratoDocu_Get(contrato.NumeroDoc.Value.Value);

                contratoHeader.UsuarioRechaza.Value = usuario.ID.Value;
                contratoHeader.ObservRechazo.Value = contrato.Observacion.Value;
                contratoHeader.FechaERechazo.Value = DateTime.Now;

                this._dal_prContratoDocu.DAL_prContratoDocu_Upd(contratoHeader);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, contrato.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, contrato.Observacion.Value, true);
                this.AsignarFlujo(documentID, contrato.NumeroDoc.Value.Value, actividadFlujoID, true, contrato.Observacion.Value);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Err_Pr_ContratoRechazar, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Contrato_Rechazar");
                throw exception;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        this._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Trae un listado de los contratos pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de pendientes para aprobar</returns>
        public List<DTO_prContratoAprob> Contrato_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                DTO_glDocumento doc = (DTO_glDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);

                this._dal_prContratoDocu = (DAL_prContratoDocu)base.GetInstance(typeof(DAL_prContratoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_prContratoAprob> list = this._dal_prContratoDocu.DAL_prContratoDocu_GetPendientesByModulo(doc, actFlujoID, usuario);

                foreach (DTO_prContratoAprob item in list)
                {
                    foreach (DTO_prContratoAprobDet itemDet in item.ContratoAprobDet)
                        itemDet.SolicitudCargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(itemDet.SolicitudDetaID.Value.Value);
                    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                }
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Contrato_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de contratos para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="contrato">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Contrato_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prContratoAprob> contrato, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in contrato)
                {
                    porcTemp = (porcParte * i) / contrato.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.Contrato_Aprobar(documentID, actividadFlujoID, usuario, item, createDoc, batchProgress, porcTotal, porcParte, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Contrato_Aprobar");
                            rd.Message = DictionaryMessages.Err_Pr_ContratoAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.Contrato_Rechazar(documentID, actividadFlujoID, usuario, item, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Contrato_Rechazar");
                            rd.Message = DictionaryMessages.Err_Pr_ContratoRechazar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Contrato_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        #endregion

        #endregion

        #region Recibido

        #region prRecibidoDocu
        /// <summary>
        /// Adiciona en la tabla prRecibidoDocu 
        /// </summary>
        /// <param name="fact">DTO_prRecibidoDocu</param>
        /// <returns></returns>
        private DTO_TxResult prRecibidoDocu_Add(DTO_prRecibidoDocu recDocu, int documetnoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prRecibidoDocu.DAL_prRecibidoDocu_Add(recDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documetnoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, recDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prReciboDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prRecibidoDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        private DTO_prRecibidoDocu prRecibidoDocu_Get(int NumeroDoc)
        {
            this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prRecibidoDocu.DAL_prRecibidoDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prRecibidoDocu 
        /// </summary>
        /// <param name="fact">DTO_prRecibidoDocu</param>
        /// <returns></returns>
        private DTO_TxResult prRecibidoDocu_Upd(DTO_prRecibidoDocu recDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prRecibidoDocu.DAL_prRecibidoDocu_Upd(recDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, recDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prRecibidoDocu_Upd");
                return result;
            }
        }
        #endregion

        #region Funciones privadas

        /// <summary>
        /// Aprueba Orden de Compra
        /// </summary>
        /// <param name="ord"> Orden de Compra de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult Recibido_Aprobar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prRecibidoAprob rec, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, decimal porcTotal, decimal porcParte, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Obtiene el header asociado de la tabla prRecibidoDocu
                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)this.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prRecibidoDocu recHeader = this._dal_prRecibidoDocu.DAL_prRecibidoDocu_Get(rec.NumeroDoc.Value.Value);
                recHeader.ConformidadInd.Value = rec.ConformidadInd.Value;
                recHeader.Calificacion.Value = rec.Calificacion.Value;

                this.prRecibidoDocu_Upd(recHeader, AppDocuments.Recibido);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, rec.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, rec.Observacion.Value, true);

                #region Genera saldo de OrdenCompra y actualiza el saldo de Solicitud
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(rec.NumeroDoc.Value.Value);
                if (ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                {
                    this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    //List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(rec.NumeroDoc.Value.Value, false);

                    //foreach (DTO_prDetalleDocu det in listDet)
                    //{
                    //    DTO_prSaldosDocu saldoRec = new DTO_prSaldosDocu();
                    //    saldoRec.EmpresaID.Value = det.EmpresaID.Value;
                    //    saldoRec.NumeroDoc.Value = det.NumeroDoc.Value;
                    //    saldoRec.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                    //    saldoRec.CantidadDocu.Value = det.CantidadRec.Value;
                    //    saldoRec.CantidadMovi.Value = 0;

                    //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                    //    saldoOC = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.OrdCompraDetaID.Value.Value);
                    //    if (saldoOC != null)
                    //    {
                    //        saldoOC.CantidadMovi.Value += det.CantidadRec.Value.Value;
                    //        if (saldoOC.CantidadMovi.Value.Value > saldoOC.CantidadDocu.Value.Value)
                    //        {
                    //            result.Result = ResultValue.NOK;
                    //            result.ResultMessage = "Saldos inválidos en proveedores (prSaldosDocu)";
                    //        }   
                    //        else
                    //        {
                    //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoRec);
                    //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoOC);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        result.Result = ResultValue.NOK;
                    //        result.ResultMessage = "Error al traer saldos de Orden Compra: Revisar Codigo";
                    //    }

                    //}
                }
                #endregion
                this.AsignarFlujo(documentID, ctrl.NumeroDoc.Value.Value, actividadFlujoID, false, ctrl.Observacion.Value);

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK && !insideAnotherTx)
                    base._mySqlConnectionTx.Commit();
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza OrdenCompra
        /// </summary>
        /// <param name="sol"> OrdenCompra de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void Recibido_Rechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_prRecibidoAprob rec, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            bool isValid = true;
            try
            {
                //Obtiene el header asociado de la tabla prSolicitudDOcu
                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)this.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prRecibidoDocu recHeader = this._dal_prRecibidoDocu.DAL_prRecibidoDocu_Get(rec.NumeroDoc.Value.Value);
                recHeader.ObsRechazo.Value = rec.Observacion.Value;

                this.prRecibidoDocu_Upd(recHeader, documentID);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, rec.NumeroDoc.Value.Value, EstadoDocControl.Anulado, rec.Observacion.Value, true);
                this.AsignarFlujo(documentID, rec.NumeroDoc.Value.Value, actividadFlujoID, true, rec.Observacion.Value);

            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Err_Pr_RecibidoRechazar, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Recibido_Rechazar");
                throw exception;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        this._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }
        #endregion

        #region Funciones publicas

        /// <summary>
        /// Guardar nuevo Recibido y asociar en glDocumentoControl
        /// </summary>
        /// <param name="ctrlRecibido">Referencia documento</param>
        /// <param name="header">RecibidoDocu</param>
        /// <param name="footer">RecibidoFooter</param>
        /// <param name="numDoc">Numero doc del documento guardado</param>
        /// <param name="docTransporte">documento de transporte para inventarios</param>
        /// <param name="manifCarga">maminifiesto de carga para inventarios</param>
        /// <returns>Resultado</returns>
        public DTO_SerializedObject Recibido_Guardar(int documentID, DTO_glDocumentoControl ctrlRecibido, DTO_prRecibidoDocu header, List<DTO_prOrdenCompraResumen> footer, out int numeroDoc, string docTransporte, string manifCarga, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            DTO_Alarma alarma = null;
            DTO_SerializedObject resultInventario = null;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 4;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_MvtoInventarios mov = null;
            ModulesPrefix mod = ModulesPrefix.pl;
            int transacNumeroDoc = 0;
            bool modulePlaneacionActive = false;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloInventarios = (ModuloInventarios)base.GetInstance(typeof(ModuloInventarios), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)base.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                Dictionary<int,decimal> consecMvtosProyecto = new Dictionary<int,decimal>();
                #region Guardar en glDocumentoControl
                ctrlRecibido.DocumentoNro.Value = 0;
                ctrlRecibido.ComprobanteIDNro.Value = 0;

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(AppDocuments.Recibido, ctrlRecibido, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    numeroDoc = 0;
                    return result;
                }
                numeroDoc = Convert.ToInt32(resultGLDC.Key);
                ctrlRecibido.NumeroDoc.Value = numeroDoc;

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Guardar en prRecibidoDocu
                header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                result = this.prRecibidoDocu_Add(header, ctrlRecibido.DocumentoID.Value.Value);
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion;
                #region Guardar en prDetalleDocu y generar registro de Movimiento (en caso de inventarios)

                List<DTO_inMovimientoFooter> movInventarioList = new List<DTO_inMovimientoFooter>();
                DTO_inBodega bodega = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, header.BodegaID.Value, true, false);
                DTO_prProveedor proveedor = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, header.ProveedorID.Value, true, false);

                try
                {
                    this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_prDetalleDocu detTemp;
                    DTO_prDetalleDocu det;
                    decimal valorMdaLocal = 0;
                    decimal valorMdaExtr = 0;
                    foreach (DTO_prOrdenCompraResumen itemFooter in footer)
                    {
                        if (itemFooter.CantidadRec.Value.Value > 0)
                        {
                            #region Guarda el registro en prDetalleDocu
                            detTemp = new DTO_prDetalleDocu();
                            det = new DTO_prDetalleDocu();
                            detTemp = this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByID(itemFooter.OrdCompraDetaID.Value.Value);
                            det = ObjectCopier.Clone(detTemp);
                            det.SerialID.Value = itemFooter.SerialID.Value;
                            det.CantidadSol.Value = 0;
                            if(documentID != AppDocuments.Recibido)
                                det.CantidadDoc1.Value = (-1) * itemFooter.CantidadRec.Value.Value;
                            else
                                det.CantidadOC.Value = (-1) * itemFooter.CantidadRec.Value.Value;
                            det.ValorUni.Value = Math.Round(detTemp.ValorUni.Value.Value / detTemp.CantidadxEmpaque.Value.Value,4);
                            det.IVAUni.Value = Math.Round(detTemp.IVAUni.Value.Value / detTemp.CantidadxEmpaque.Value.Value,2);
                            if (monedaLocal.Equals(itemFooter.MonedaIDOC.Value))
                            {
                                if (itemFooter.CantidadRec.Value == detTemp.CantidadOC.Value)
                                {
                                    det.IvaTotML.Value = detTemp.IvaTotML.Value;
                                    det.ValorTotML.Value = detTemp.ValorTotML.Value;
                                    det.ValorTotME.Value = detTemp.ValorTotME.Value;
                                    det.IvaTotME.Value = detTemp.IvaTotME.Value;
                                }
                                else
                                {
                                    det.IvaTotML.Value = Math.Round(itemFooter.CantidadRec.Value.Value * det.IVAUni.Value.Value, 2);
                                    det.ValorTotML.Value = Math.Round(itemFooter.CantidadRec.Value.Value * det.ValorUni.Value.Value, 2);
                                    det.ValorTotME.Value = detTemp.CantidadOC.Value != 0 ? (detTemp.ValorTotME.Value / (detTemp.CantidadOC.Value / detTemp.CantidadxEmpaque.Value)) * itemFooter.CantidadRec.Value : 0;
                                    det.IvaTotME.Value = detTemp.CantidadOC.Value != 0 ? detTemp.IvaTotME.Value / (detTemp.CantidadOC.Value / detTemp.CantidadxEmpaque.Value) * itemFooter.CantidadRec.Value : 0;
                                }
                            }
                            else
                            {
                                if (itemFooter.CantidadRec.Value == detTemp.CantidadOC.Value)
                                {
                                    det.IvaTotML.Value = detTemp.IvaTotML.Value;
                                    det.ValorTotML.Value = detTemp.ValorTotML.Value;
                                    det.ValorTotME.Value = detTemp.ValorTotME.Value;
                                    det.IvaTotME.Value = detTemp.IvaTotME.Value;                              
                                }
                                else
                                {
                                    det.IvaTotME.Value = itemFooter.CantidadRec.Value * det.IVAUni.Value;
                                    det.ValorTotME.Value = itemFooter.CantidadRec.Value * det.ValorUni.Value;
                                    det.ValorTotML.Value = Math.Round(detTemp.CantidadOC.Value != 0 ? (detTemp.ValorTotML.Value.Value / (detTemp.CantidadOC.Value.Value / detTemp.CantidadxEmpaque.Value.Value)) * itemFooter.CantidadRec.Value.Value : 0, 2);
                                    det.IvaTotML.Value = Math.Round(detTemp.CantidadOC.Value != 0 ? detTemp.IvaTotML.Value.Value / (itemFooter.CantidadOC.Value.Value / detTemp.CantidadxEmpaque.Value.Value) * itemFooter.CantidadRec.Value.Value : 0, 2);
                                }
                            }
                            det.CantidadRec.Value = itemFooter.CantidadRec.Value.Value;
                            det.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            det.RecibidoDocuID.Value = det.NumeroDoc.Value.Value;
                            det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                            det.RecibidoDetaID.Value = det.ConsecutivoDetaID.Value;
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                            valorMdaLocal += det.ValorTotML.Value.Value;
                            valorMdaExtr += det.ValorTotME.Value.Value;
                            #endregion
                            #region Carga Movimiento de entrada para Inventarios
                            if (itemFooter.ClaseBS == TipoCodigo.Inventario)
                            {
                                DTO_glMovimientoDeta movDet = new DTO_glMovimientoDeta();
                                movDet.NumeroDoc.Value = 0;
                                movDet.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                                movDet.BodegaID.Value = bodega.ID.Value;
                                movDet.Fecha.Value = ctrlRecibido.PeriodoDoc.Value;
                                movDet.inReferenciaID.Value = det.inReferenciaID.Value;
                                movDet.Parametro1.Value = string.IsNullOrEmpty(det.Parametro1.Value) ? this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto) : det.Parametro1.Value;
                                movDet.Parametro2.Value = string.IsNullOrEmpty(det.Parametro2.Value) ? this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto) : det.Parametro2.Value;
                                movDet.EstadoInv.Value = string.IsNullOrEmpty(det.Parametro2.Value) ? (byte)EstadoInv.Activo : det.EstadoInv.Value;
                                movDet.PrefijoID.Value = bodega.PrefijoID.Value;
                                movDet.ActivoID.Value = det.ActivoID.Value;
                                movDet.SerialID.Value = det.SerialID.Value;
                                movDet.CantidadUNI.Value = det.CantidadRec.Value;
                                movDet.EmpaqueInvID.Value = det.EmpaqueInvID.Value;
                                DTO_inEmpaque empaque = (DTO_inEmpaque)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, movDet.EmpaqueInvID.Value, true, false);
                                movDet.CantidadEMP.Value = empaque != null && empaque.Cantidad.Value != 0 ? movDet.CantidadUNI.Value / empaque.Cantidad.Value : 0;
                                movDet.CentroCostoID.Value = bodega.CentroCostoID.Value;
                                movDet.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                                movDet.ProyectoID.Value = itemFooter.ProyectoID.Value;// bodega.ProyectoID.Value; Cual es ?
                                movDet.ValorUNI.Value = det.ValorUni.Value;
                                movDet.DocSoporte.Value = det.Detalle4ID.Value;
                                if (proveedor.TipoProveedor.Value != null && proveedor.TipoProveedor.Value == (byte)TipoProveedor.Extranjero)
                                {
                                    movDet.Valor2EXT.Value = det.ValorTotME.Value.Value;
                                    movDet.Valor2LOC.Value = det.ValorTotML.Value.Value;
                                    movDet.Valor1LOC.Value = 0;
                                    movDet.Valor1EXT.Value = 0;
                                    //Iva
                                    movDet.Valor3EXT.Value = det.IvaTotME.Value.Value;
                                    movDet.Valor3LOC.Value = det.IvaTotML.Value.Value;
                                }
                                else
                                {
                                    movDet.Valor1LOC.Value = det.ValorTotML.Value.Value;
                                    movDet.Valor1EXT.Value = det.ValorTotME.Value.Value;
                                    movDet.Valor2EXT.Value = 0;
                                    movDet.Valor2LOC.Value = 0;
                                }
                                if (det.Detalle4ID.Value != null)
                                {
                                    if(!consecMvtosProyecto.ContainsKey(det.Detalle4ID.Value.Value))
                                        consecMvtosProyecto.Add(det.Detalle4ID.Value.Value,det.CantidadRec.Value.Value);
                                    else
                                        consecMvtosProyecto[det.Detalle4ID.Value.Value] += det.CantidadRec.Value.Value;
                                }
                                DTO_inMovimientoFooter movFooter = new DTO_inMovimientoFooter();
                                movFooter.Movimiento = movDet;
                                movInventarioList.Add(movFooter);
                            }
                            #endregion
                        }
                    }
                    ctrlRecibido.Valor.Value = ctrlRecibido.MonedaID.Value != monedaLocal ? valorMdaExtr : valorMdaLocal;
                    rd = this._moduloGlobal.glDocumentoControl_Update(ctrlRecibido, true, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK Update Doc";
                        numeroDoc = 0;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    result.Result = ResultValue.NOK;
                    numeroDoc = 0;
                    result.ResultMessage = "NOK";
                    return result;
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion;
                #region Crear movimiento Inventarios
                string tipoMvtoEntradaComprasLoc = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovEntradaComprasLoc);
                string tipoMvtoEntradaComprasExtr = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovEntradaComprasExt);
                List<DTO_inMovimientoDocu> movDocuExist = null;
                if (movInventarioList.Count > 0)
                {
                    DateTime periodoInv = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
                    mov = new DTO_MvtoInventarios();
                    movDocuExist = new List<DTO_inMovimientoDocu>();
                    mov.Header.EmpresaID.Value = ctrlRecibido.EmpresaID.Value;
                    #region Asigna y valida que el Doc de Transporte no exista
                    mov.Header.DatoAdd1.Value = docTransporte;
                    if (!string.IsNullOrEmpty(docTransporte))
                        movDocuExist = this._moduloInventarios.inMovimientoDocu_GetByParameter(documentID, mov.Header);
                    #endregion

                    if (movDocuExist.Count == 0)
                    {
                        #region Valida el Tipo de Mov de acuerdo al Proveedor
                        string tipoMvto = string.Empty;
                        tipoMvto = (proveedor != null && (proveedor.TipoProveedor.Value == null || proveedor.TipoProveedor.Value == (byte)TipoProveedor.Local)) ?
                                   tipoMvtoEntradaComprasLoc : tipoMvtoEntradaComprasExtr;
                        if (string.IsNullOrEmpty(tipoMvto))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_In_MvtoComprasNotExist;
                            numeroDoc = 0;
                            return result;
                        }
                        DTO_inMovimientoTipo tipoMovInv = (DTO_inMovimientoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, tipoMvto, true, false);
                        if (tipoMovInv == null)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_In_TipoMovInvNotExist;
                            numeroDoc = 0;
                            return result;
                        } 
                        #endregion
                        #region  Carga Header Mvto
                        mov.Header.MvtoTipoInvID.Value = tipoMvto;
                        mov.Header.BodegaOrigenID.Value = bodega.ID.Value;
                        mov.Header.BodegaDestinoID.Value = string.Empty;
                        mov.Header.AsesorID.Value = string.Empty;
                        mov.Header.ClienteID.Value = string.Empty;
                        mov.Header.DocumentoREL.Value = header.NumeroDoc.Value.Value;
                        mov.Header.VtoFecha.Value = ctrlRecibido.Fecha.Value.Value;
                        mov.Header.NumeroDoc.Value = 0;
                        mov.Header.DatoAdd2.Value = manifCarga;//Almacena el Manif de carga para Importaciones 
                        #endregion
                        #region Carga DocControl Mvto
                        mov.DocCtrl.TerceroID.Value = ctrlRecibido.TerceroID.Value;
                        mov.DocCtrl.NumeroDoc.Value = 0;
                        mov.DocCtrl.MonedaID.Value = ctrlRecibido.MonedaID.Value;
                        mov.DocCtrl.ProyectoID.Value = ctrlRecibido.ProyectoID.Value;
                        mov.DocCtrl.CentroCostoID.Value = ctrlRecibido.CentroCostoID.Value;
                        mov.DocCtrl.PrefijoID.Value = ctrlRecibido.PrefijoID.Value;
                        mov.DocCtrl.Fecha.Value = DateTime.Now;
                        mov.DocCtrl.PeriodoDoc.Value = periodoInv;
                        mov.DocCtrl.TasaCambioCONT.Value = ctrlRecibido.TasaCambioCONT.Value;
                        mov.DocCtrl.TasaCambioDOCU.Value = ctrlRecibido.TasaCambioDOCU.Value;
                        mov.DocCtrl.DocumentoID.Value = AppDocuments.TransaccionAutomatica;
                        mov.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        mov.DocCtrl.PeriodoUltMov.Value = ctrlRecibido.PeriodoUltMov.Value;
                        mov.DocCtrl.seUsuarioID.Value = ctrlRecibido.seUsuarioID.Value;
                        mov.DocCtrl.AreaFuncionalID.Value = ctrlRecibido.AreaFuncionalID.Value;
                        mov.DocCtrl.ConsSaldo.Value = 0;
                        mov.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        mov.DocCtrl.FechaDoc.Value = periodoInv.Month == ctrlRecibido.PeriodoDoc.Value.Value.Month? ctrlRecibido.FechaDoc.Value : new DateTime(periodoInv.Year,periodoInv.Month,DateTime.DaysInMonth(periodoInv.Year,periodoInv.Month));
                        mov.DocCtrl.Descripcion.Value = "Transaccion Automatica Inv(Recibido)";
                        mov.DocCtrl.DocumentoPadre.Value = numeroDoc;
                        if (mov.DocCtrl.TasaCambioDOCU.Value != 0)
                        {
                            foreach (var item in movInventarioList)
                            {
                                item.Movimiento.Valor1EXT.Value = Math.Round(item.Movimiento.Valor1LOC.Value.Value / mov.DocCtrl.TasaCambioDOCU.Value.Value, 2);
                                item.Movimiento.Valor2EXT.Value = Math.Round(item.Movimiento.Valor2LOC.Value.Value / mov.DocCtrl.TasaCambioDOCU.Value.Value, 2);
                            }
                        }
                        mov.DocCtrl.Valor.Value = ctrlRecibido.Valor.Value;
                        mov.DocCtrl.Iva.Value = ctrlRecibido.Iva.Value;
                        mov.Footer = movInventarioList; 
                        #endregion
                        resultInventario = this._moduloInventarios.Transaccion_Add(AppDocuments.TransaccionAutomatica, mov, false, out transacNumeroDoc, batchProgress, true, true);
                        if (resultInventario.GetType() == result.GetType())
                        {
                            DTO_TxResult res = (DTO_TxResult)resultInventario;
                            if (res.Result == ResultValue.NOK)
                            {
                                result = res;
                                numeroDoc = 0;
                                return result;
                            }
                        }
                        #region Actualiza movimientos de proyectos si existen
                        foreach (var mvto in consecMvtosProyecto)
                        {
                            DTO_pyProyectoMvto mvtoProyecto = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(mvto.Key);
                            if (mvtoProyecto != null)
                            {
                                mvtoProyecto.CantidadREC.Value = mvtoProyecto.CantidadREC.Value + mvto.Value;
                                this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvtoProyecto);
                            }
                        } 
                        #endregion                       
                    }
                    else
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Pr_DocTransporteExist;
                        numeroDoc = 0;
                        return result;
                    }
                }
                #endregion

                #region Presupuesto
                //Verifica si tiene activo el Modulo de Planeacion
                modulePlaneacionActive = this.GetModuleActive(mod);
                if (modulePlaneacionActive)
                {
                    this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    result = this._moduloPlaneacion.GeneraPresupuesto(AppDocuments.Recibido, numeroDoc);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }
                #endregion

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    return alarma;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_Guardar");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloInventarios._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        DTO_glDocumentoControl docCtrlMovInventario = null;

                        #region Genera Consecutivos
                        #region Documento Recibido
                        ctrlRecibido.DocumentoNro.Value = this.GenerarDocumentoNro(ctrlRecibido.DocumentoID.Value.Value, ctrlRecibido.PrefijoID.Value);                    
                        alarma.Consecutivo = ctrlRecibido.DocumentoNro.Value.ToString();
                        #endregion
                        #region Documento de inventarios(56)(si existe)
                        if (resultInventario != null)
                        {
                            DTO_Alarma res = (DTO_Alarma)resultInventario;
                            int numDoc = Convert.ToInt32(res.NumeroDoc);
                            docCtrlMovInventario = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                            docCtrlMovInventario.DocumentoNro.Value = this.GenerarDocumentoNro(AppDocuments.TransaccionAutomatica, ctrlRecibido.PrefijoID.Value);
                            
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
                        this._moduloGlobal.ActualizaConsecutivos(ctrlRecibido, true, false, true);
                       
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="solNro">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_prRecibido Recibido_Load(int documentID, string prefijoID, int recNro, int NumeroDoc)
        {
            try
            {
                DTO_prRecibido rec = new DTO_prRecibido();

                //Trae glDocumentoControl
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = null;
                if (NumeroDoc == 0)
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, prefijoID, recNro);
                else
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(NumeroDoc);
                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;

                //Verifica documentoID
                rec.DocCtrl = docCtrl;
                if (docCtrl.DocumentoID.Value.Value != documentID)
                    return null;

                //Carga prRecibidoDocu
                DTO_prRecibidoDocu recHeader = this.prRecibidoDocu_Get(docCtrl.NumeroDoc.Value.Value);
                rec.Header = recHeader;

                //Obtenga datos de la tabla prDetalleDocu
                List<DTO_prDetalleDocu> detDocu = this.prDetalleDocu_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value, false);

                //Llena detalle de recibido(DTO_prOrdenCompraFooter)
                List<DTO_prOrdenCompraFooter> recFooter = this.OrdenCompraFooter_Load(detDocu);
                rec.Footer = recFooter;

                return rec;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Recibido_Load");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de los Recibidos pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prRecibidoAprob> Recibido_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumento doc = (DTO_glDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);

                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                List<DTO_prRecibidoAprob> list = this._dal_prRecibidoDocu.DAL_prRecibidoDocu_GetPendientesByModulo(doc, actFlujoID, usuarioID);

                foreach (DTO_prRecibidoAprob rec in list)
                {
                    foreach (DTO_prRecibidoAprobDet d in rec.Detalle)
                    {
                        var ctrlOC =  this._moduloGlobal.glDocumentoControl_GetByID(d.OrdCompraDocuID.Value.Value);
                        d.PrefDoc = ctrlOC.PrefDoc.Value;
                        d.FileUrlDet = base.GetFileRemotePath(d.OrdCompraDocuID.Value.ToString(), TipoArchivo.Documentos);
                    }
                    rec.FileUrl = base.GetFileRemotePath(rec.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                }
                   

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Trae un listado de los Recibidos no facturados ya aprobados
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="actFlujoID">Actividad Flujo</param>
        /// <param name="usuario">usuario actual</param>
        /// <param name="NroDocfacturaExist">Numero Doc del documento de la factura si existe</param>
        /// <returns>Lista de Recibidos Aprobados</returns>
        public List<DTO_prRecibidoAprob> Recibido_GetRecibidoNoFacturado(int documentID, string actFlujoID, string proveedor, int NroDocfacturaExist)
        {
            try
            {
                DTO_glDocumento doc = (DTO_glDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);

                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_prRecibidoAprob> list = this._dal_prRecibidoDocu.DAL_prRecibidoDocu_GetRecibidoNoFacturado(doc,proveedor,NroDocfacturaExist);
                #region Variables
                string codigoIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                Dictionary<string, Tuple<DTO_prBienServicio, decimal>> cacheBienServ = new Dictionary<string, Tuple<DTO_prBienServicio, decimal>>();
                Dictionary<string, decimal> cacheBienServClase = new Dictionary<string, decimal>();
                Dictionary<string, decimal> cacheConCargo = new Dictionary<string, decimal>();
                Dictionary<string, decimal> cacheCuenta = new Dictionary<string, decimal>();
                DTO_glBienServicioClase _bienServClase;

                #endregion
                foreach (DTO_prRecibidoAprob rec in list)
                {
                    foreach (DTO_prRecibidoAprobDet det in rec.Detalle)
                    {
                        decimal iva = 0;
                        DTO_prBienServicio _bienServ = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio,det.CodigoBSID.Value,true,false);
                        #region Trae BienServicio de la solicitud
                        if (cacheBienServClase.ContainsKey(_bienServ.ClaseBSID.Value))
                            det.PorcIVA = cacheBienServClase[_bienServ.ClaseBSID.Value];// cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, cacheBienServClase[_bienServ.ClaseBSID.Value]));
                        else
                        {
                            _bienServClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, _bienServ.ClaseBSID.Value, true,false);
                            if (cacheConCargo.ContainsKey(_bienServClase.ConceptoCargoID.Value))
                            {
                                det.PorcIVA = cacheConCargo[_bienServClase.ConceptoCargoID.Value];// cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, cacheConCargo[_bienServClase.ConceptoCargoID.Value]));
                                cacheBienServClase.Add(_bienServ.ClaseBSID.Value, cacheConCargo[_bienServClase.ConceptoCargoID.Value]);
                            }
                            else
                            {       
                                if (!string.IsNullOrEmpty(_bienServClase.ConceptoCargoID.Value))
                                {
                                    Dictionary<string, string> keyscoImpuesto = new Dictionary<string, string>();
                                    keyscoImpuesto.Add("ConceptoCargoID", _bienServClase.ConceptoCargoID.Value);
                                    keyscoImpuesto.Add("ImpuestoTipoID", codigoIVA);
                                    DTO_coImpuesto imp = (DTO_coImpuesto)this.GetMasterComplexDTO(AppMasters.coImpuesto, keyscoImpuesto, true);
                                    if (imp != null)
                                    {
                                        #region Trae la cuenta
                                        if (cacheCuenta.ContainsKey(imp.CuentaID.Value))
                                            iva = cacheCuenta[imp.CuentaID.Value];
                                        else
                                        {
                                            DTO_coPlanCuenta _cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta,imp.CuentaID.Value, true,false);
                                            if (!string.IsNullOrEmpty(_cuenta.ImpuestoPorc.Value.ToString()))
                                            {
                                                iva = _cuenta.ImpuestoPorc.Value.Value;
                                                cacheCuenta.Add(_cuenta.ID.Value, iva);
                                            }
                                        }
                                        #endregion
                                    }
                                    //cacheBienServ.Add(_bienServ.ID.Value, Tuple.Create(_bienServ, iva));
                                    cacheBienServClase.Add(_bienServ.ClaseBSID.Value, iva);
                                    cacheConCargo.Add(_bienServClase.ConceptoCargoID.Value, iva);
                                }
                                det.PorcIVA = iva;
                            }
                        }
                        #endregion
                    }
                }                
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_GetRecibidoNoFacturado");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de Recibido para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Recibido_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_prRecibidoAprob> rec, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in rec)
                {
                    porcTemp = (porcParte * i) / rec.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.Recibido_Aprobar(documentID, actividadFlujoID, usuario, item, createDoc, batchProgress, porcTotal, porcParte, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Recibido_Aprobar");
                            rd.Message = DictionaryMessages.Err_Pr_RecibidoAprobar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this.Recibido_Rechazar(documentID, actividadFlujoID, usuario, item, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Recibido_Rechazar");
                            rd.Message = DictionaryMessages.Err_Pr_RecibidoRechazar + "&&" + item.PrefijoID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_AprobarRechazar");
                results.Add(result);

                return results;
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
        public DTO_SerializedObject Recibido_SendToAprob(int documentID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (docCtrl != null)
                {
                    #region Validacion del estado del documento
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }
                    #endregion
                    #region Se envia para aprobacion
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    if (createDoc)
                    {
                        try
                        {
                            #region Generar el nuevo archivo
                            if (documentID == AppDocuments.Solicitud)
                                this.GenerarArchivo(documentID, docCtrl.NumeroDoc.Value.Value, DtoReportSolicitud(docCtrl.NumeroDoc.Value.Value, true));
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
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_SendToAprob");
                throw ex;
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
        /// Radicar Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="_dtoCtrlCxP">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_SerializedObject Recibido_RadicarDevolver(int documentID, List<DTO_prRecibidoAprob> recibidosNoFactSelect, DTO_glDocumentoControl _dtoCtrlCxP, DTO_cpCuentaXPagar cta, bool update, out int numeroDoc, bool devolverInd, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            int _numeroDocInvent = 0;
            try
            {
                this._moduloCxP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                decimal _valorInventario = 0;
                #region Radica factura de Recibidos
                if (!devolverInd)
                {
                    #region Radica la CxP
                    int numeroDocCXP = 0;
                    DTO_SerializedObject resultCxP = this._moduloCxP.CuentasXPagar_Radicar(documentID, _dtoCtrlCxP, cta, false, update, out numeroDocCXP, batchProgress, true, true);
                    if (resultCxP.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult resultDev = (DTO_TxResult)resultCxP;
                        if (resultDev.Result == ResultValue.NOK)
                        {
                            result.Result = ResultValue.NOK;
                            numeroDoc = 0;
                            return resultCxP;
                        }
                    }

                    #endregion
                    _dtoCtrlCxP.NumeroDoc.Value = cta.NumeroDoc.Value.Value;
                    numeroDoc = cta.NumeroDoc.Value.Value;
                    #region Asigna la factura al detalle relacionado
                    foreach (var recibido in recibidosNoFactSelect)
                    {
                        if (recibido.Seleccionar.Value.Value)
                        {
                            List<DTO_prDetalleDocu> detRecibido = this.prDetalleDocu_GetByNumeroDoc(recibido.NumeroDoc.Value.Value, false);
                            foreach (DTO_prDetalleDocu det in detRecibido)
                            {
                                DTO_prRecibidoAprobDet recDet = recibido.Detalle.Find(x=>x.RecibidoDetaID.Value == det.RecibidoDetaID.Value);
                                det.FacturaDocuID.Value = numeroDoc;
                                if (recDet != null)
                                {
                                    det.ValorTotML.Value = recDet.ValorMLDet.Value;
                                    det.ValorTotME.Value = recDet.ValorMEDet.Value;
                                    det.IvaTotML.Value = recDet.ValorIvaMLDet.Value;
                                    det.IvaTotME.Value = recDet.ValorIvaMEDet.Value;  
                                }
                                this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                            }
                        }
                        #region Verifica si existe movimiento de inventarios en el recibido
                        foreach (DTO_prRecibidoAprobDet rec in recibido.Detalle)
                        {
                            DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, rec.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                            if (bsClase.TipoCodigo.Value == (byte)TipoCodigo.Inventario)
                            {
                                DTO_glDocumentoControl docCtrlRecibido = new DTO_glDocumentoControl();
                                docCtrlRecibido.DocumentoPadre.Value = recibido.NumeroDoc.Value;
                                List<DTO_glDocumentoControl> docCtrlInventario = this._moduloGlobal.glDocumentoControl_GetByParameter(docCtrlRecibido);
                                if (docCtrlInventario.Count > 0)
                                {
                                    _numeroDocInvent = docCtrlInventario[0].NumeroDoc.Value.Value;
                                    _valorInventario = docCtrlInventario[0].Valor.Value.Value;

                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                #region Devuelve factura de Recibidos
                else
                {
                    DTO_SerializedObject res = this._moduloCxP.CuentasXPagar_Devolver(documentID, _dtoCtrlCxP, cta, false, batchProgress, true);
                    if (res.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult resultDev = (DTO_TxResult)res;
                        if (resultDev.Result == ResultValue.NOK)
                        {
                            result.Result = ResultValue.NOK;
                            numeroDoc = 0;
                            return res;
                        }
                    }
                    _dtoCtrlCxP.NumeroDoc.Value = cta.NumeroDoc.Value.Value;
                    numeroDoc = cta.NumeroDoc.Value.Value;

                    foreach (var recibido in recibidosNoFactSelect)
                    {
                        List<DTO_prDetalleDocu> detRecibido = this.prDetalleDocu_GetByNumeroDoc(recibido.NumeroDoc.Value.Value, false);
                        foreach (DTO_prDetalleDocu det in detRecibido)
                        {
                            det.FacturaDocuID.Value = null;
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                        }
                    }

                }
                #endregion
                if (result.Result == ResultValue.NOK)
                {
                    numeroDoc = 0;
                    return result;
                }
                //Trae la info de la alarma
                alarma = this.GetFirstMailInfo(numeroDoc, false);
                alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                alarma.Consecutivo = _dtoCtrlCxP.DocumentoNro.Value.ToString();
                return alarma;
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_RadicarDevolver");

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

                        this._moduloCxP._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        #region Genera consecutivos
                        if (!update)
                        {
                            _dtoCtrlCxP.DocumentoNro.Value = this.GenerarDocumentoNro(_dtoCtrlCxP.DocumentoID.Value.Value, _dtoCtrlCxP.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(_dtoCtrlCxP, true, false, false);
                            alarma.Consecutivo = _dtoCtrlCxP.DocumentoNro.Value.ToString();

                            #region Actualiza la cxp de la radicacion
                            this._moduloCxP = (ModuloCuentasXPagar)this.GetInstance(typeof(ModuloCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            DTO_cpCuentaXPagar cxpNroRadica = this._moduloCxP.CuentasXPagar_Get(cta.NumeroDoc.Value.Value);
                            cxpNroRadica.NumeroRadica.Value = _dtoCtrlCxP.DocumentoNro.Value;
                            this._moduloCxP.CuentasXPagar_Upd(cxpNroRadica);
                            #endregion

                        }
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Funcion para devolver una factura radicada
        /// </summary>
        /// <param name="dtoCtrl">documento control</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario</param>
        /// <returns></returns>
        public DTO_TxResult Recibido_Devolver(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cta, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                if (!insideAnotherTx)
                    base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, dtoCtrl.NumeroDoc.Value.Value, EstadoDocControl.Devuelto, EstadoDocControl.Devuelto.ToString(), true);

                //Verifica si el comprobante existe
                if (dtoCtrl.ComprobanteIDNro != null && dtoCtrl.ComprobanteIDNro.Value.HasValue)
                {
                    this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_Comprobante compAux = this._moduloContabilidad.Comprobante_Get(true, true, dtoCtrl.PeriodoDoc.Value.Value, dtoCtrl.ComprobanteID.Value, dtoCtrl.ComprobanteIDNro.Value.Value, null, null);
                    if (compAux != null)
                    {
                        compAux = this._moduloContabilidad.CreateEmptyAux(compAux.Header);
                        this._moduloContabilidad.AgregarAuxiliar(compAux);
                        this._moduloContabilidad.BorrarAuxiliar_Pre(dtoCtrl.PeriodoDoc.Value.Value, dtoCtrl.ComprobanteID.Value, dtoCtrl.ComprobanteIDNro.Value.Value);
                    }
                }

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                //Actualiza Tabla cpCuentaXPagar
                DTO_TxResult text = null;//this.CuentasXPagar_Upd(cta);

                //Elimina las alarmas
                this.DeshabilitarAlarma(cta.NumeroDoc.Value.Value, string.Empty);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_Devolver");

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
        /// Genera el detalle de cargos de los Recibidos aprobados pendientes
        /// </summary>
        /// <returns>lista de resultados</returns>
        public List<DTO_SerializedObject> Recibido_GenerarDetalleCargosRecib(int documentID, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            List<DTO_prRecibidoAprob> listRecibidosAprob = new List<DTO_prRecibidoAprob>();
            List<DTO_prDetalleCargos> listDetalleCargo = new List<DTO_prDetalleCargos>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            bool isValid = true;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            try
            {
                #region Instancia y declara variables
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                #endregion

                //Trae los Recibidos Aprobados pendientes por generar cargos
                listRecibidosAprob = this._dal_prDetalleDocu.DAL_prDetalleDocu_GetPendientesCargosRecib();

                foreach (var recibido in listRecibidosAprob)
                {
                    #region Variables de resultado y progreso
                    porcTemp = (porcParte * i) / listRecibidosAprob.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";
                    rd.line = i;
                    #endregion

                    try
                    {
                        //Trae el cargo de la solicitud(prSolicitudCargo) filtrado por el item de solicitud del detalleDocu
                        var listSolicitudCargos = this._dal_prSolicitudCargos.DAL_prSolicitudCargos_GetByID(recibido.Detalle[0].SolicitudDetaID.Value.Value);

                        #region LLena lista de dto con la Info necesaria
                        for (int j = 0; j < listSolicitudCargos.Count; j++)
                        {
                            DTO_prDetalleCargos detalle = new DTO_prDetalleCargos();
                            detalle.ConsecutivoDetaID.Value = recibido.Detalle[0].ConsecutivoDetaID.Value;
                            detalle.EmpresaID.Value = recibido.EmpresaID.Value;
                            detalle.NumeroDoc.Value = recibido.NumeroDoc.Value;
                            detalle.vlrMdaLoc.Value = recibido.CostoML.Value;// +recibido.CostoIvaML.Value;
                            detalle.vlrMdaExt.Value = recibido.CostoME.Value;// +recibido.CostoIvaME.Value;
                            detalle.ProyectoID.Value = listSolicitudCargos[j].ProyectoID.Value;
                            detalle.CentroCostoID.Value = listSolicitudCargos[j].CentroCostoID.Value;
                            detalle.PorcentajeID.Value = listSolicitudCargos[j].PorcentajeID.Value;

                            DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, recibido.Detalle[0].CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                            detalle.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                            detalle.ConceptoCargoID.Value = bienServicioClase.ConceptoCargoID.Value;

                            DTO_prProveedor proveedor = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, recibido.ProveedorID.Value, true, false);
                            detalle.TerceroID.Value = proveedor.TerceroID.Value;

                            DTO_coProyecto proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, detalle.ProyectoID.Value, true, false);
                            DTO_coCentroCosto centroCto = null;
                            string operacion = string.Empty;
                            if (string.IsNullOrWhiteSpace(proyecto.OperacionID.Value))
                            {
                                centroCto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, detalle.CentroCostoID.Value, true, false);
                                if (string.IsNullOrWhiteSpace(centroCto.OperacionID.Value))
                                    operacion = proyecto.OperacionID.Value;
                                else
                                {
                                    isValid = false;
                                    result.Result = ResultValue.NOK;
                                    rd.Message = DictionaryMessages.Err_OperacionIsNullorEmpty;
                                    result.Details.Add(rd);
                                }
                            }
                            else
                                operacion = proyecto.OperacionID.Value;

                            DTO_CuentaValor cta = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(detalle.ConceptoCargoID.Value, operacion, detalle.LineaPresupuestoID.Value, detalle.vlrMdaLoc.Value.Value);
                            if (cta != null)
                            {
                                detalle.CuentaID.Value = cta.CuentaID.Value;
                                listDetalleCargo.Add(detalle);
                            }
                            else
                            {
                                isValid = false;
                                result.Result = ResultValue.NOK;
                                rd.Message = DictionaryMessages.Err_Co_NoCtaCargoCosto + "&&" + detalle.ConceptoCargoID.Value + "&&" +
                                    detalle.LineaPresupuestoID.Value + "&&" + detalle.ProyectoID.Value + "&&" + detalle.CentroCostoID.Value;
                                result.Details.Add(rd);
                            }
                        }
                        #endregion
                    }
                    catch (Exception exAprob)
                    {
                        isValid = false;
                        result.Result = ResultValue.NOK;
                        string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Recibido_GenerarDetalleCargosRecib");
                        rd.Message = DictionaryMessages.Err_Pr_DetalleCargo + errMsg;
                        result.Details.Add(rd);
                    }
                }
                #region Agrega los items de Detalle Cargos
                if (result.Result == ResultValue.OK)
                {
                    result = this.prDetalleCargos_Add(listDetalleCargo);
                    if (result.Result == ResultValue.NOK)
                        isValid = false;
                }
                #endregion

                batchProgress[tupProgress] = 100;

                results.Add(result);
                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Recibido_GenerarDetalleCargosRecib");
                results.Add(result);

                return results;
            }
            finally
            {
                if (isValid)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();
                        this._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                    }
                    else
                        throw new Exception("Recibido_GenerarDetalleCargosRecib - Esta adentro otra transaccioncion");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion
        #endregion

        #region Convenio(Solicitud/Consumo/RecibidoConsumo)

        #region Funciones Privadas

        #region prConvenioSolicitudDocu

        /// <summary>
        /// Adiciona en la tabla prConvenioSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prConvenioSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prConvenioSolicitudDocu_Add(DTO_prConvenioSolicitudDocu convDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_Add(convDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, convDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prConvenioSolicitudDocu_Add");
                return result;
            }
        }

        /// <summary>
        /// Consulta una tabla prConvenioSolicitudDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de la solicitud</param>
        /// <returns></returns>
        private DTO_prConvenioSolicitudDocu prConvenioSolicitudDocu_Get(int NumeroDoc)
        {
            this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_Get(NumeroDoc);
        }

        /// <summary>
        /// Actualiza la tabla prConvenioSolicitudDocu 
        /// </summary>
        /// <param name="fact">DTO_prConvenioSolicitudDocu</param>
        /// <returns></returns>
        private DTO_TxResult prConvenioSolicitudDocu_Upd(DTO_prConvenioSolicitudDocu convDocu, int documentoID)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                SqlCommand mySqlCommand = base._mySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base._mySqlConnectionTx;

                this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_Upd(convDocu);

                #region Guarda en la bitacora
                DAL_aplBitacora bitDAL = (DAL_aplBitacora)base.GetInstance(typeof(DAL_aplBitacora), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                bitDAL.DAL_aplBitacora_Add(this.Empresa.ID.Value, documentoID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, convDocu.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prConvenioSolicitudDocu_Upd");
                return result;
            }
        }

        #endregion

        #region prConvenioConsumoDirecto

        /// <summary>
        /// Aprueba Orden de Compra
        /// </summary>
        /// <param name="conv"> Orden de Compra de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult ConsumoProyecto_Aprobar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, DTO_ConvenioAprob conv, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, decimal porcTotal, decimal porcParte, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            bool modulePlaneacionActive = false;
            bool moduleProyectosActive = false;

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)this.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);

                List<DTO_prSolicitudCargos> cargos = new List<DTO_prSolicitudCargos>();
                moduleProyectosActive = this.GetModuleActive(ModulesPrefix.py);
                bool controlSolicitudxProyInd = this.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndControlSolicitudesBS).Equals("1") ? true : false;

                #region Guardar en prDetalleDocu y prSolicitudCargos
                foreach (DTO_prConvenioConsumoDirectoAprobDet itemFooter in conv.listConvenioConsumoDet)
                {
                    DTO_prDetalleDocu det = new DTO_prDetalleDocu();
                    det.NumeroDoc.Value = conv.NumeroDoc.Value;
                    det.OrdCompraDocuID.Value = itemFooter.NumeroDocOC.Value;
                    det.Documento1ID.Value = conv.NumeroDoc.Value; //Documento de Planilla Consumo
                    det.CodigoBSID.Value = itemFooter.CodigoBSID.Value;
                    if (itemFooter.inReferenciaID.Value == refxDefecto)
                    {
                        DTO_prBienServicio descr = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                        det.Descriptivo.Value = descr.Descriptivo.Value;
                    }
                    else
                    {
                        DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, itemFooter.inReferenciaID.Value, true, false);
                        det.inReferenciaID.Value = referencia.ID.Value;
                        det.Descriptivo.Value = referencia.Descriptivo.Value;
                        det.UnidadInvID.Value = referencia.UnidadInvID.Value;
                    }
                    det.SerialID.Value = itemFooter.SerialID.Value;
                    det.MonedaID.Value = conv.Moneda.Value;
                    det.CantidadDoc1.Value = itemFooter.Cantidad.Value;
                    det.ValorTotME.Value = 0;
                    det.IvaTotML.Value = 0;
                    det.IvaTotME.Value = 0;
                    det.ValorUni.Value = itemFooter.Cantidad.Value != 0 ? itemFooter.ValorDet.Value / itemFooter.Cantidad.Value : 0;
                    det.ValorTotML.Value = itemFooter.ValorDet.Value;                 
                    det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                    det.Detalle1ID.Value = det.ConsecutivoDetaID.Value;
                    if (moduleProyectosActive && controlSolicitudxProyInd)
                    {
                        //Trae el numero Doc de la solicitud de servicios que debe estar registrada en la OC
                        DTO_prDetalleDocu detOC = this.prDetalleDocu_GetByNumeroDoc(itemFooter.NumeroDocOC.Value.Value, false).First();
                        det.Documento2ID.Value = detOC.Documento2ID.Value;
                        det.CantidadDoc2.Value = itemFooter.Cantidad.Value;
                        det.Detalle2ID.Value = det.ConsecutivoDetaID.Value;
                        //Asigna la info de la solicitud 
                        det.SolicitudDocuID.Value = detOC.SolicitudDocuID.Value;
                    }
                    this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);

                    DTO_prSolicitudCargos cargo = new DTO_prSolicitudCargos();
                    cargo.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                    cargo.NumeroDoc.Value = conv.NumeroDoc.Value;
                    cargo.ProyectoID.Value = conv.ProyectoID.Value;
                    cargo.CentroCostoID.Value = conv.CentroCostoID.Value;
                    if (string.IsNullOrEmpty(det.LineaPresupuestoID.Value))
                    {
                        DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                        DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                        cargo.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                    }
                    else
                        cargo.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                    cargo.PorcentajeID.Value = 100;
                    cargos.Add(cargo);
                }
                result = this.prSolicitudCargos_Add(cargos);

                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = "NOK";
                    return result;
                }
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion;
                #region Genera registro de ConsumoProyecto en prSaldosDocu
                List<DTO_prDetalleDocu> listDet = this.prDetalleDocu_GetByNumeroDoc(conv.NumeroDoc.Value.Value, false);

                //foreach (DTO_prDetalleDocu det in listDet)
                //{
                //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                //    saldoOC.EmpresaID.Value = det.EmpresaID.Value;
                //    saldoOC.NumeroDoc.Value = det.NumeroDoc.Value;
                //    saldoOC.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                //    saldoOC.CantidadDocu.Value = det.CantidadDoc1.Value;
                //    saldoOC.CantidadMovi.Value = 0;
                //    this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoOC);
                //}
                #endregion
                #region Genera registro de RecibidoConsumo y Actualiza el registro de ConsumoProyecto en prSaldosDocu
                //this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //List<DTO_prDetalleDocu> listDet = this._dal_prDetalleDocu.DAL_prDetalleDocu_GetByNumeroDoc(conv.NumeroDoc.Value.Value, false);

                //foreach (DTO_prDetalleDocu det in listDet)
                //{
                //    DTO_prSaldosDocu saldoOC = new DTO_prSaldosDocu();
                //    saldoOC.EmpresaID.Value = det.EmpresaID.Value;
                //    saldoOC.NumeroDoc.Value = det.NumeroDoc.Value;
                //    saldoOC.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                //    saldoOC.CantidadDocu.Value = det.CantidadDoc1.Value;
                //    saldoOC.CantidadMovi.Value = 0;

                //    DTO_prSaldosDocu saldoSol = new DTO_prSaldosDocu();
                //    saldoSol = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.SolicitudDetaID.Value.Value);
                //    saldoSol.CantidadMovi.Value += det.CantidadOC.Value.Value;
                //    if (saldoSol.CantidadMovi.Value.Value > saldoSol.CantidadDocu.Value.Value)
                //        result.Result = ResultValue.NOK;
                //    else
                //    {
                //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoOC);
                //        this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoSol);
                //    }
                //}
                #endregion
                #region Presupuesto
                modulePlaneacionActive = this.GetModuleActive(ModulesPrefix.pl);
                //Verifica si tiene activo el Modulo de Planeacion
                if (modulePlaneacionActive)
                {
                    this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    result = this._moduloPlaneacion.GeneraPresupuesto(AppDocuments.ConsumoProyecto, conv.NumeroDoc.Value.Value);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }
                #endregion
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, conv.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, conv.Observacion.Value, true);

                this.AsignarFlujo(documentID, conv.NumeroDoc.Value.Value, actividadFlujoID, false, conv.Observacion.Value);
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ConsumoProyecto_Aprobar");

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
                        this._moduloContabilidad._mySqlConnectionTx = null;                        
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Guarda la lista de prConvenioConsumoDirecto en base de datos
        /// </summary>
        /// <param name="consumo">la lista de DTO_prConvenioConsumoDirecto</param>
        /// <returns></returns>
        private DTO_TxResult prConvenioConsumoDirecto_Add(List<DTO_prConvenioConsumoDirecto> consumo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)base.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prConvenioConsumoDirecto.DAL_prConvenioConsumoDirecto_Add(consumo);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prConvenioConsumoDirecto_Add");
                return result;
            }
        }

        /// <summary>
        /// Trae la lista de prConvenioConsumoDirecto segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prConvenioConsumoDirecto> prConvenioConsumoDirecto_GetByNumeroDoc(int NumeroDoc)
        {
            this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)base.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prConvenioConsumoDirecto.DAL_prConvenioConsumoDirecto_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenioConsumoDirecto
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        private void prConvenioConsumoDirecto_Delete(int NumeroDoc)
        {
            this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)base.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prConvenioConsumoDirecto.DAL_prConvenioConsumoDirecto_Delete(NumeroDoc);
        }

        #endregion

        #endregion

        #region Solicitud Despacho Convenios

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="documentID">ID del documento</param>
        /// <param name="data">data a guardar</param>
        /// <param name="numeroDoc">identificador interior del documento</param>
        /// <returns>si la operacion es exitosa</returns>
        public DTO_SerializedObject Convenio_Add(int documentID, DTO_Convenios data, out int numeroDoc, bool update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                if (!update)
                {
                    #region Guardar en glDocumentoControl
                    data.DocCtrl.DocumentoNro.Value = 0;
                    data.DocCtrl.ComprobanteIDNro.Value = 0;
                    data.DocCtrl.Valor.Value = data.Header.Valor.Value;
                    data.DocCtrl.Iva.Value = data.Header.IVA.Value;

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, data.DocCtrl, true);
                    if (resultGLDC.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        //result.Details.Add(resultGLDC);

                        numeroDoc = 0;
                        return result;
                    }
                    data.DocCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion

                    if (documentID == AppDocuments.SolicitudDespachoConvenio)
                    {
                        #region Guardar en prConvenioSolicitudDocu
                        data.Header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        result = this.prConvenioSolicitudDocu_Add(data.Header, data.DocCtrl.DocumentoID.Value.Value);
                        if (result.Result == ResultValue.NOK)
                        {
                            numeroDoc = 0;
                            result.ResultMessage = "NOK";
                            return result;
                        }

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;

                        #endregion;
                        #region Guardar en prDetalleDocu y prSolicitudCargos
                        string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                        string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                        string monedaExtran = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                        List<DTO_prSolicitudCargos> cargos = new List<DTO_prSolicitudCargos>();
                        foreach (DTO_SolicitudDespachoFooter itemFooter in data.FooterSolDespacho)
                        {
                            DTO_prDetalleDocu det = new DTO_prDetalleDocu();
                            DTO_prSolicitudCargos itemCargos = new DTO_prSolicitudCargos();
                            det.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value;
                            det.Documento1ID.Value =   data.DocCtrl.NumeroDoc.Value; //Documento de Solicitud Despacho Convenios
                            det.CodigoBSID.Value = itemFooter.CodigoBSID.Value;
                            if (itemFooter.inReferenciaID.Value == refxDefecto)
                            {
                                DTO_prBienServicio descr = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                                det.Descriptivo.Value = descr.Descriptivo.Value;
                            }
                            else
                            {
                                DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, itemFooter.inReferenciaID.Value, true, false);
                                det.inReferenciaID.Value = referencia.ID.Value;
                                det.Descriptivo.Value = referencia.Descriptivo.Value;
                                det.UnidadInvID.Value = referencia.UnidadInvID.Value;
                            }
                            det.MonedaID.Value = data.DocCtrl.MonedaID.Value;
                            det.CantidadDoc1.Value = itemFooter.CantidadSol.Value;
                            det.IvaTotML.Value = 0;
                            det.IvaTotME.Value = 0;
                            det.ValorTotME.Value = data.DocCtrl.TasaCambioDOCU.Value != 0? itemFooter.Valor.Value/data.DocCtrl.TasaCambioDOCU.Value : 0;
                            det.ValorUni.Value = itemFooter.ValorUni.Value;
                            det.ValorTotML.Value = itemFooter.Valor.Value;
                            det.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                            det.Detalle1ID.Value = det.ConsecutivoDetaID.Value;  //ConsecutivoDeta de Solicitud Despacho Convenios
                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);

                            #region Asigna datos de Solicitud Cargos
                            itemCargos.NumeroDoc.Value = det.NumeroDoc.Value;
                            itemCargos.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                            itemCargos.ProyectoID.Value = itemFooter.ProyectoID.Value;
                            itemCargos.CentroCostoID.Value = itemFooter.CentroCostoID.Value;
                            itemCargos.PorcentajeID.Value = itemFooter.Porcentaje.Value;
                            if (string.IsNullOrEmpty(det.LineaPresupuestoID.Value))
                            {
                                DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                                DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                                itemCargos.LineaPresupuestoID.Value = bienServicioClase.LineaPresupuestoID.Value;
                            }
                            else
                                itemCargos.LineaPresupuestoID.Value = det.LineaPresupuestoID.Value;
                            cargos.Add(itemCargos); 
                            #endregion
                        }
                        result = this.prSolicitudCargos_Add(cargos);

                        if (result.Result == ResultValue.NOK)
                        {
                            result.ResultMessage = "NOK";
                            numeroDoc = 0;
                            return result;
                        }
                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;

                        #endregion;                        
                    }
                    else if (documentID == AppDocuments.ConsumoProyecto) //Planilla Consumo
                        #region Guardar en prConvenioConsumoDirecto
                        try
                        {
                            this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)this.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            foreach (DTO_prConvenioConsumoDirecto itemFooter in data.FooterConsumo)
                                itemFooter.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                            this.prConvenioConsumoDirecto_Add(data.FooterConsumo);
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            numeroDoc = 0;
                            result.ResultMessage = "NOK";
                            return result;
                        }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                        #endregion;
                }
                else
                {
                    if (documentID == AppDocuments.SolicitudDespachoConvenio)
                    {
                        List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                        if (act.Count > 0)
                            this.AsignarFlujo(documentID, data.DocCtrl.NumeroDoc.Value.Value, act[0], false, string.Empty);
                        #region Actualiza en prConvenioSolicitudDocu
                        this.prConvenioSolicitudDocu_Upd(data.Header,documentID);

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;

                        #endregion;
                        #region Actualiza en prDetalleDocu
                        List<DTO_prDetalleDocu> detDocu = new List<DTO_prDetalleDocu>();
                        detDocu = this.prDetalleDocu_GetByNumeroDoc(data.DocCtrl.NumeroDoc.Value.Value, false);

                        foreach (DTO_SolicitudDespachoFooter itemFooter in data.FooterSolDespacho)
                        {
                            DTO_prDetalleDocu det = new DTO_prDetalleDocu();
                            if (detDocu.Exists(x => x.NumeroDoc.Value.Equals(itemFooter.NumeroDoc.Value) && 
                                                    x.CodigoBSID.Value.Equals(itemFooter.CodigoBSID.Value) && 
                                                    x.inReferenciaID.Value.Equals(itemFooter.inReferenciaID.Value)))
                            {
                                this.prDetalleDocu_Update(documentID, det, false, true);
                                //detDocu.RemoveAll(x => x.ConsecutivoDetaID.Value.Equals(det.ConsecutivoDetaID.Value));
                            }
                            else
                            {
                                string refxDefecto = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_ReferenciaporDefecto);
                                DTO_prDetalleDocu detNew = new DTO_prDetalleDocu();
                                detNew.NumeroDoc.Value = data.DocCtrl.NumeroDoc.Value.Value;
                                detNew.Documento1ID.Value = data.DocCtrl.NumeroDoc.Value.Value; //Documento de Solicitud Despacho Convenios
                                detNew.CodigoBSID.Value = itemFooter.CodigoBSID.Value;
                                if (itemFooter.inReferenciaID.Value == refxDefecto)
                                {
                                    DTO_prBienServicio descr = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, detNew.CodigoBSID.Value, true, false);
                                    detNew.Descriptivo.Value = descr.Descriptivo.Value;
                                }
                                else
                                {
                                    DTO_inReferencia referencia = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, itemFooter.inReferenciaID.Value, true, false);
                                    detNew.inReferenciaID.Value = referencia.ID.Value;
                                    detNew.Descriptivo.Value = referencia.Descriptivo.Value;
                                    detNew.UnidadInvID.Value = referencia.UnidadInvID.Value;
                                }
                                detNew.MonedaID.Value = data.DocCtrl.MonedaID.Value;
                                detNew.CantidadDoc1.Value = itemFooter.CantidadSol.Value;
                                det.ValorUni.Value = itemFooter.ValorUni.Value;
                                det.ValorTotML.Value = itemFooter.Valor.Value;
                                detNew.ConsecutivoDetaID.Value = this._dal_prDetalleDocu.DAL_prDetalleDocu_Add(det);
                                detNew.Detalle1ID.Value = det.ConsecutivoDetaID.Value;
                                this._dal_prDetalleDocu.DAL_prDetalleDocu_Update(det);
                            }
                        }   

                        porcTotal += porcParte;
                        batchProgress[tupProgress] = (int)porcTotal;
                        #endregion;
                    }
                    else if (documentID == AppDocuments.ConsumoProyecto) //Planilla Consumo
                        #region Actualiza en prConvenioConsumoDirecto
                        try
                        {
                            List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                            if (act.Count > 0)
                                this.AsignarFlujo(documentID, data.DocCtrl.NumeroDoc.Value.Value, act[0], false, string.Empty);

                            numeroDoc = data.DocCtrl.NumeroDoc.Value.Value;
                            this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)this.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                            this._dal_prConvenioConsumoDirecto.DAL_prConvenioConsumoDirecto_Delete(data.DocCtrl.NumeroDoc.Value.Value);
                            this.prConvenioConsumoDirecto_Add(data.FooterConsumo);
                        }
                        catch (Exception ex)
                        {
                            result.Result = ResultValue.NOK;
                            numeroDoc = 0;
                            result.ResultMessage = "NOK";
                            return result;
                        }

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                        #endregion;
                }

                numeroDoc = data.DocCtrl.NumeroDoc.Value.Value;
                data.DocCtrl.NumeroDoc.Value = numeroDoc;

                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    return alarma;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Convenio_Add");
                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (!update)
                        {
                            data.DocCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, data.DocCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(data.DocCtrl, true, false, true);
                            alarma.Consecutivo = data.DocCtrl.DocumentoNro.Value.ToString();
                        }
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroContrato">Numero de Documento interno</param>
        /// <returns>Retorna el Recibido</returns>
        public DTO_Convenios Convenio_GetByNroContrato(int documentID, string prefijoID, int NroContrato)
        {
            try
            {
                DTO_Convenios convenios = null;

                //Consulta si la solicitud de convenio ya existe  
                this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_prConvenioSolicitudDocu convenioDocu = this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_GetByNroContrato(NroContrato);

                if (convenioDocu != null)
                {
                    convenios = new DTO_Convenios();

                    //Trae glDocumentoControl
                    this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(convenioDocu.NumeroDoc.Value.Value);
                    string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    if (docCtrl == null)
                        return null;
                    else
                        convenios.DocCtrl = docCtrl;

                    //Carga ConvenioDocu
                    DTO_prConvenioSolicitudDocu recHeader = this.prConvenioSolicitudDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    convenios.Header = recHeader;

                    //Obtiene datos de la tabla prDetalleDocu
                    List<DTO_prDetalleDocu> detDocu = this.prDetalleDocu_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value, false);
                    convenios.DetalleDocu = detDocu;

                    //Asigna el footer de Solicitud Despacho de acuerdo al prDetalleDocu
                    List<DTO_SolicitudDespachoFooter> solDespacho = new List<DTO_SolicitudDespachoFooter>();
                    foreach (DTO_prDetalleDocu det in detDocu)
                    {
                        DTO_SolicitudDespachoFooter d = new DTO_SolicitudDespachoFooter();
                        d.CantidadSol.Value = det.CantidadDoc1.Value;
                        d.CodigoBSID.Value = det.CodigoBSID.Value;
                        d.inReferenciaID.Value = det.inReferenciaID.Value;
                        d.NumeroDoc.Value = det.NumeroDoc.Value;
                        d.Valor.Value = docCtrl.MonedaID.Value == monedaLocal ? det.ValorTotML.Value : det.ValorTotME.Value;
                        d.ValorUni.Value = d.Valor.Value / d.CantidadSol.Value;
                        solDespacho.Add(d);
                    }
                    convenios.FooterSolDespacho = solDespacho;
                }

                return convenios;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Convenio_GetByNroContrato");
                throw exception;
            }
        }

        /// <summary>
        /// Carga la informacion completa del documento
        /// </summary>
        /// <param name="documentID">Documento ID</param>
        /// <param name="prefijoID">PrefijoID</param>
        /// <param name="NroConsecutivo">Numero de Documento consecutivo</param>
        /// <returns>Retorna el Convenio</returns>
        public DTO_Convenios Convenio_Get(int documentID, string prefijoID, int NroConsecutivo, bool ConsumoProyectoInd)
        {
            try
            {
                DTO_Convenios convenios = new DTO_Convenios();

                //Trae glDocumentoControl
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetInternalDoc(documentID, prefijoID, NroConsecutivo);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);

                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;
                else
                    convenios.DocCtrl = docCtrl;

                if (!ConsumoProyectoInd)
                {
                    //Carga header de prOrdenCompraDocu
                    DTO_prConvenioSolicitudDocu recHeader = this.prConvenioSolicitudDocu_Get(docCtrl.NumeroDoc.Value.Value);
                    convenios.Header = recHeader;

                    //Obtiene datos de la tabla prDetalleDocu
                    List<DTO_prDetalleDocu> detDocu = this.prDetalleDocu_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value, false);
                    convenios.DetalleDocu = detDocu;

                    //Asigna el footer de Solicitud Despacho de acuerdo al prDetalleDocu
                    List<DTO_SolicitudDespachoFooter> solDespacho = new List<DTO_SolicitudDespachoFooter>();
                    foreach (DTO_prDetalleDocu det in detDocu)
                    {
                        DTO_SolicitudDespachoFooter d = new DTO_SolicitudDespachoFooter();
                        d.CantidadSol.Value = det.CantidadDoc1.Value;
                        d.CodigoBSID.Value = det.CodigoBSID.Value;
                        d.inReferenciaID.Value = det.inReferenciaID.Value;
                        d.NumeroDoc.Value = det.NumeroDoc.Value;
                        d.Valor.Value = docCtrl.MonedaID.Value == monedaLocal ? det.ValorTotML.Value : det.ValorTotME.Value;
                        d.ValorUni.Value = d.Valor.Value / d.CantidadSol.Value;
                        solDespacho.Add(d);
                    }
                    convenios.FooterSolDespacho = solDespacho;
                }
                else
                {
                    //Carga detalle de prConvenioConsumoDirecto
                    List<DTO_prConvenioConsumoDirecto> detDocu = this.prConvenioConsumoDirecto_GetByNumeroDoc(docCtrl.NumeroDoc.Value.Value);
                    foreach (var det in detDocu)
                    {
                        //Obtiene la orden de compra asociada a cada detalle
                        DTO_glDocumentoControl docCtrlOrdenCompra = this._moduloGlobal.glDocumentoControl_GetByID(det.NumeroDocOC.Value.Value);
                        if (docCtrlOrdenCompra != null)
                        {
                            det.PrefijoID.Value = docCtrlOrdenCompra.PrefijoID.Value;
                            det.DocumentoNro.Value = docCtrlOrdenCompra.DocumentoNro.Value;
                        }
                        else
                            return null;
                        DTO_prOrdenCompra ordenCompra = this.OrdenCompra_Load(AppDocuments.OrdenCompra, det.PrefijoID.Value, det.DocumentoNro.Value.Value);

                        //Si la orden de compra tiene contrato asociado lo carga
                        if (ordenCompra != null && ordenCompra.HeaderOrdenCompra.ContratoNro.Value != null)
                        {
                            //Obtiene el contrato de la orden de compra
                            DTO_glDocumentoControl docCtrlContrato = this._moduloGlobal.glDocumentoControl_GetByID(ordenCompra.HeaderOrdenCompra.ContratoNro.Value.Value);
                            DTO_prOrdenCompra contrato = this.OrdenCompra_Load(AppDocuments.Contrato, ordenCompra.DocCtrl.PrefijoID.Value, docCtrlContrato.DocumentoNro.Value.Value);

                            //Si existen convenios del contrato de la OC llena los valores 
                            if (contrato != null && contrato.Convenio.Count > 0)
                            {
                                if (contrato.Convenio.Exists(d => d.CodigoBSID.Value.Equals(det.CodigoBSID.Value) && d.inReferenciaID.Value.Equals(det.inReferenciaID.Value)))
                                {
                                    object res = contrato.Convenio.Where(x => x.CodigoBSID.Value.Equals(det.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(det.inReferenciaID.Value)).First();
                                    DTO_prConvenio convenio = (DTO_prConvenio)res;
                                    det.ValorUni.Value = convenio.Valor.Value;
                                    det.Valor.Value = det.ValorUni.Value * det.Cantidad.Value;
                                }
                            }
                            else
                            {
                                //Si no existen convenios llena los valores con el detalle de la orden de compra
                                if (ordenCompra.Footer.Exists(d => d.DetalleDocu.CodigoBSID.Value.Equals(det.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(det.inReferenciaID.Value)))
                                {
                                    object res = ordenCompra.Footer.Where(d => d.DetalleDocu.CodigoBSID.Value.Equals(det.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(det.inReferenciaID.Value)).First();
                                    DTO_prOrdenCompraFooter ordenFooter = (DTO_prOrdenCompraFooter)res;
                                    det.ValorUni.Value = ordenFooter.DetalleDocu.ValorUni.Value;
                                    det.Valor.Value = det.ValorUni.Value * det.Cantidad.Value;
                                }
                            }
                        }
                    }
                    convenios.FooterConsumo = detDocu;
                }

                return convenios;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "Convenio_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de los Convenios pendientes para aprobar(Solicitudes y Consumos)
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConvenioAprob> Convenio_GetPendientesByModulo(int documentID, string actFlujoID, DTO_seUsuario usuario, bool SolicitudInd)
        {
            try
            {
                List<DTO_ConvenioAprob> list = null;
                if (documentID == AppDocuments.ConvenioSolicitudAprob)
                {
                    string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    list = this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_GetPendientesByModulo(documentID, actFlujoID, usuario);

                    foreach (DTO_ConvenioAprob item in list)
                    {
                        item.ValorDoc.Value = monedaLocal == item.Moneda.Value ? item.listConvenioSolicitudDet.Sum(x => x.ValorTotML.Value) : item.listConvenioSolicitudDet.Sum(x => x.ValorTotME.Value);
                        item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                    }
                }
                else
                {
                    this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)base.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    list = this._dal_prConvenioConsumoDirecto.DAL_prConsumoProyecto_GetPendientesByModulo(documentID, actFlujoID, usuario);

                    foreach (var itemList in list)
                    {
                        decimal ValorTotal = 0;
                        foreach (var consumo in itemList.listConvenioConsumoDet)
                        {
                            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                            //Obtiene la orden de compra asociada a cada detalle
                            DTO_glDocumentoControl docCtrlOrdenCompra = this._moduloGlobal.glDocumentoControl_GetByID(consumo.NumeroDocOC.Value.Value);

                            if (docCtrlOrdenCompra != null)
                            {
                                DTO_prOrdenCompra ordenCompra = this.OrdenCompra_Load(AppDocuments.OrdenCompra, docCtrlOrdenCompra.PrefijoID.Value, docCtrlOrdenCompra.DocumentoNro.Value.Value);

                                //Si la orden de compra tiene contrato asociado lo carga
                                if (ordenCompra != null && ordenCompra.HeaderOrdenCompra.ContratoNro.Value != null)
                                {
                                    //Obtiene el contrato de la orden de compra
                                    DTO_glDocumentoControl docCtrlContrato = this._moduloGlobal.glDocumentoControl_GetByID(ordenCompra.HeaderOrdenCompra.ContratoNro.Value.Value);
                                    DTO_prOrdenCompra contrato = this.OrdenCompra_Load(AppDocuments.Contrato, ordenCompra.DocCtrl.PrefijoID.Value, docCtrlContrato.DocumentoNro.Value.Value);

                                    //Si existen convenios del contrato de la OC llena los valores 
                                    if (contrato != null && contrato.Convenio.Count > 0)
                                    {
                                        if (contrato.Convenio.Exists(d => d.CodigoBSID.Value.Equals(consumo.CodigoBSID.Value) && d.inReferenciaID.Value.Equals(consumo.inReferenciaID.Value)))
                                        {
                                            object res = contrato.Convenio.Where(x => x.CodigoBSID.Value.Equals(consumo.CodigoBSID.Value) && x.inReferenciaID.Value.Equals(consumo.inReferenciaID.Value)).First();
                                            DTO_prConvenio convenio = (DTO_prConvenio)res;
                                            consumo.ValorUniDet.Value = convenio.Valor.Value;
                                            consumo.ValorDet.Value = consumo.ValorUniDet.Value * consumo.Cantidad.Value;
                                            ValorTotal += consumo.ValorDet.Value.Value;
                                        }
                                    }
                                    else
                                    {
                                        //Si no existen convenios llena los valores con el detalle de la orden de compra
                                        if (ordenCompra.Footer.Exists(d => d.DetalleDocu.CodigoBSID.Value.Equals(consumo.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(consumo.inReferenciaID.Value)))
                                        {
                                            object res = ordenCompra.Footer.Where(d => d.DetalleDocu.CodigoBSID.Value.Equals(consumo.CodigoBSID.Value) && d.DetalleDocu.inReferenciaID.Value.Equals(consumo.inReferenciaID.Value)).First();
                                            DTO_prOrdenCompraFooter ordenFooter = (DTO_prOrdenCompraFooter)res;
                                            consumo.ValorUniDet.Value = ordenFooter.DetalleDocu.ValorUni.Value;
                                            consumo.ValorDet.Value = consumo.ValorUniDet.Value * consumo.Cantidad.Value;
                                            ValorTotal += consumo.ValorDet.Value.Value;
                                        }
                                    }
                                }
                            }
                        }
                        itemList.ValorDoc.Value = ValorTotal;
                    }
                    foreach (DTO_ConvenioAprob item in list)
                        item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                }
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Convenio_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Recibe una lista de Convenios para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="ord">ordenes de compra que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Convenio_AprobarRechazar(int documentID, string actividadFlujoID, DTO_seUsuario usuario, List<DTO_ConvenioAprob> convenios, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in convenios)
                {
                    porcTemp = (porcParte * i) / convenios.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            if (documentID == AppDocuments.ConvenioSolicitudAprob)
                            {
                                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, item.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, item.Observacion.Value, true);
                                this.AsignarFlujo(documentID, item.NumeroDoc.Value.Value, actividadFlujoID, false, item.Observacion.Value);
                            }
                            else
                            {
                                this.ConsumoProyecto_Aprobar(documentID, actividadFlujoID, usuario, item, createDoc, batchProgress, porcTotal, porcParte, insideAnotherTx);
                            }
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Convenio_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_Pr_RecibidoAprobar + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, item.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, item.Observacion.Value, true);
                            this.AsignarFlujo(documentID, item.NumeroDoc.Value.Value, actividadFlujoID, true, item.Observacion.Value);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Convenio_AprobarRechazar");
                            rd.Message = DictionaryMessages.Err_Pr_RecibidoRechazar + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Convenio_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        ///   Envia para aprobacion un convenio
        /// </summary>
        /// <param name="documentID">Documento de aprobacion</param>
        /// <param name="actividadFlujoID">actividad actual</param>
        /// <param name="numeroDoc">identificador del documento a aprobar</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Convenio_SendToAprob(int documentID, string actividadFlujoID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (docCtrl != null)
                {
                    #region Validacion del estado del documento
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }
                    #endregion

                    #region Asigna el nuevo flujo
                    result = this.AsignarFlujo(documentID, docCtrl.NumeroDoc.Value.Value, actividadFlujoID, false, string.Empty);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                    #region Revisa si finaliza el proceso y Aprueba Directo

                    bool finaliza = result.ResultMessage == true.ToString() ? true : false;
                    if (finaliza)
                    {
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, "Aprobado Directo", true);
                        this.AsignarFlujo(documentID, numeroDoc, actividadFlujoID, false, string.Empty);
                    }
                    else
                        this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, docCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    if (result.Result == ResultValue.NOK)
                        return result;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_GettingDocument;
                    return result;
                }
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(numeroDoc, true);
                    alarma.PrefijoID = alarma.PrefijoID.TrimEnd();
                    alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                    return alarma;
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Convenio_SendToAprob");
                throw ex;
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
        /// Trae un listado de Solicitudes de Despacho Convenios para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<DTO_ConveniosResumen> SolicitudDespachoConvenio_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, string proveedor)
        {
            try
            {
                this._dal_prConvenioSolicitudDocu = (DAL_prConvenioSolicitudDocu)base.GetInstance(typeof(DAL_prConvenioSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ConveniosResumen> list = this._dal_prConvenioSolicitudDocu.DAL_prConvenioSolicitudDocu_GetResumen(documentID, usuario, mod, proveedor);
                foreach (DTO_ConveniosResumen itemConsumo in list)
                {
                    decimal Valor = 0;
                    decimal IVA = 0;
                    foreach (DTO_ConveniosResumenDet consumoDet in itemConsumo.Detalle)
                    {
                        Valor += consumoDet.ValorTotal.Value != null ? consumoDet.ValorTotal.Value.Value : 0;
                        IVA += consumoDet.IVATotal.Value != null ? consumoDet.IVATotal.Value.Value : 0;
                    }
                    itemConsumo.Valor.Value = Valor;
                    itemConsumo.IVA.Value = IVA;
                }
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "SolicitudDespachoConvenio_GetResumen");
                return null;
            }
        }

        #endregion       

        #region prConvenioConsumoDirecto

        /// <summary>
        /// Trae un listado de los Consumos de Proyecto para recibido
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConveniosResumen> ConsumoProyecto_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, DateTime fechaCorte, string proveedorID)
        {
            try
            {
                this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)base.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ConveniosResumen> list = this._dal_prConvenioConsumoDirecto.DAL_prConsumoDirecto_GetResumen(documentID, usuario, mod, fechaCorte, proveedorID);
                foreach (DTO_ConveniosResumen itemConsumo in list)
                {
                    decimal Valor = 0;
                    decimal IVA = 0;
                    foreach (DTO_ConveniosResumenDet consumoDet in itemConsumo.Detalle)
                    {
                        Valor += consumoDet.ValorTotal.Value != null ? consumoDet.ValorTotal.Value.Value : 0;
                        IVA += consumoDet.IVATotal.Value != null ? consumoDet.IVATotal.Value.Value : 0;
                    }
                    itemConsumo.Valor.Value = Valor;
                    itemConsumo.IVA.Value = IVA;
                }
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ConsumoProyecto_GetResumen");
                return null;
            }
        }

        /// <summary>
        /// Agrega un recibido de consumo y Recibe los consumos de proyecto en los saldos 
        /// </summary>
        /// <param name="documentID">Documento actual</param>
        /// <param name="docCtrl">Doc control de recibido consumo</param>
        /// <param name="recibidosConvenio">Lista de items a recibir</param>
        /// <param name="numeroDoc">Identificador del documento actual</param>
        /// <returns>Respuesta</returns>
        public DTO_SerializedObject RecibidoConvenios_Add(int documentID, List<DTO_ConveniosResumen> recibidosConvenio, string proveedorID,DateTime fechaDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            bool modulePlaneacionActive = false;
            int numeroDocRecibido = 0; 
            DTO_Alarma alarma = new DTO_Alarma();
            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;          

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)this.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prConvenioConsumoDirecto = (DAL_prConvenioConsumoDirecto)this.GetInstance(typeof(DAL_prConvenioConsumoDirecto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSaldosDocu = (DAL_prSaldosDocu)this.GetInstance(typeof(DAL_prSaldosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                
                string periodoDoc = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo);             
                modulePlaneacionActive = this.GetModuleActive(ModulesPrefix.pl);
                DTO_prProveedor proveedorDto = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, proveedorID, true, false);
                string bodegaTransito = this.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
                DTO_inBodega bodegaDto = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaTransito, true, false);
                if (bodegaDto == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_In_BodegaTransRequired;
                    return result;
                }
                #region Carga Variables
                DTO_prRecibido _data = new DTO_prRecibido();
                List<DTO_prOrdenCompraResumen> _listRecibidos = new List<DTO_prOrdenCompraResumen>();
                #endregion
                #region Crea Recibido
                #region Carga Header
                _data.Header.NumeroDoc.Value = 0;
                _data.Header.BodegaID.Value = bodegaTransito;
                _data.Header.ProveedorID.Value = proveedorID;
                _data.Header.LugarEntrega.Value = bodegaDto.LocFisicaID.Value;
                #endregion
                #region Carga DocumentoControl
                _data.DocCtrl.NumeroDoc.Value = 0;
                _data.DocCtrl.DocumentoID.Value = AppDocuments.Recibido;
                _data.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                _data.DocCtrl.TerceroID.Value = proveedorDto.TerceroID.Value;
                _data.DocCtrl.ComprobanteID.Value = string.Empty;
                _data.DocCtrl.ComprobanteIDNro.Value = 0;
                _data.DocCtrl.MonedaID.Value = recibidosConvenio.First().MonedaIDConvenio.Value;
                _data.DocCtrl.CuentaID.Value = string.Empty;
                _data.DocCtrl.ProyectoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                _data.DocCtrl.CentroCostoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                _data.DocCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                _data.DocCtrl.LineaPresupuestoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                _data.DocCtrl.Fecha.Value = DateTime.Now;
                _data.DocCtrl.PeriodoDoc.Value = !string.IsNullOrEmpty(periodoDoc) ? Convert.ToDateTime(periodoDoc) : _data.DocCtrl.Fecha.Value;
                _data.DocCtrl.PrefijoID.Value = this.GetPrefijoByDocumento(AppDocuments.Recibido);
                _data.DocCtrl.TasaCambioCONT.Value = this._moduloGlobal.TasaDeCambio_Get(_data.DocCtrl.MonedaID.Value, fechaDoc);
                _data.DocCtrl.TasaCambioDOCU.Value = _data.DocCtrl.TasaCambioCONT.Value;
                _data.DocCtrl.DocumentoNro.Value = 0;
                _data.DocCtrl.PeriodoUltMov.Value = _data.DocCtrl.PeriodoDoc.Value;
                _data.DocCtrl.seUsuarioID.Value = this.UserId;
                _data.DocCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                _data.DocCtrl.ConsSaldo.Value = 0;
                _data.DocCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                _data.DocCtrl.Observacion.Value = string.Empty;
                _data.DocCtrl.FechaDoc.Value = fechaDoc;
                _data.DocCtrl.Descripcion.Value = documentID == AppDocuments.RecibidoPlanillaConsumoProy ? "Recibido por Planilla Consumo Proyecto" : "Recibido por Solicitud Despacho Convenio";
                _data.DocCtrl.Valor.Value = recibidosConvenio.Where(x => x.Seleccionar.Value == true).Sum(x => x.Valor.Value);
                _data.DocCtrl.Iva.Value = recibidosConvenio.Where(x => x.Seleccionar.Value == true).Sum(x => x.IVA.Value);

                #endregion
                #region Carga Footer
                foreach (var recibido in recibidosConvenio)
                { 
                    if (recibido.Seleccionar.Value.Value)
                    {  
                        foreach (var detRecibido in recibido.Detalle)
                        {                              
                            DTO_prOrdenCompraResumen recibidoNew = new DTO_prOrdenCompraResumen();
                            DTO_prBienServicio bs = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, detRecibido.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bs.ClaseBSID.Value, true, false);
                            TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString());
                            recibidoNew.CodigoBSID.Value = detRecibido.CodigoBSID.Value;
                            recibidoNew.ClaseBS = tipoCodigo;
                            recibidoNew.CantidadRec.Value = detRecibido.CantidadConvenio.Value;
                            recibidoNew.Descriptivo.Value = detRecibido.DescripDetalle.Value;
                            recibidoNew.inReferenciaID.Value = detRecibido.inReferenciaID.Value;
                            if (documentID == AppDocuments.RecibidoPlanillaConsumoProy)
                            {
                                recibidoNew.OrdCompraDetaID.Value = detRecibido.ConsumoDetaID.Value; //Guarda el cons del detalle del consumo proy
                                recibidoNew.OrdCompraDocuID.Value = detRecibido.ConsumoDocuID.Value; //Guarda el numero Doc del consumo proy
                            }
                            else
                            {
                                recibidoNew.OrdCompraDetaID.Value = detRecibido.SolicitudDespachoDetaID.Value; //Guarda el cons del detalle de la solic Despacho
                                recibidoNew.OrdCompraDocuID.Value = detRecibido.SolicitudDespachoDocuID.Value; //Guarda el numero Doc de la solic Despacho
                            }
                            _listRecibidos.Add(recibidoNew);                          
                        }
                    }
                }
                #endregion
                #region Guarda el Recibido
                DTO_SerializedObject res = this.Recibido_Guardar(documentID, _data.DocCtrl, _data.Header, _listRecibidos, out numeroDocRecibido, string.Empty, string.Empty, batchProgress, true);
                if (res.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)res;
                    return result;
                }
                #endregion
                #region Aprueba el Recibido
                #region Actualiza el registro de ConsumoProyecto en prSaldosDocu
                if (documentID == AppDocuments.RecibidoPlanillaConsumoProy)
                {
                    //List<DTO_prDetalleDocu> listDetRec = this.prDetalleDocu_GetByNumeroDoc(numeroDocRecibido, false);
                    //foreach (DTO_prDetalleDocu det in listDetRec)
                    //{
                    //    //Asigna el nuevo saldo
                    //    DTO_prSaldosDocu saldoRec = new DTO_prSaldosDocu();
                    //    saldoRec.EmpresaID.Value = det.EmpresaID.Value;
                    //    saldoRec.NumeroDoc.Value = det.NumeroDoc.Value;
                    //    saldoRec.ConsecutivoDetaID.Value = det.ConsecutivoDetaID.Value;
                    //    saldoRec.CantidadDocu.Value = det.CantidadRec.Value;
                    //    saldoRec.CantidadMovi.Value = 0;

                    //    DTO_prSaldosDocu saldoConsumo = new DTO_prSaldosDocu();
                    //    saldoConsumo = this._dal_prSaldosDocu.DAL_prSaldosDocu_GetByID(det.Detalle1ID.Value.Value);
                    //    if (saldoConsumo != null)
                    //    {
                    //        saldoConsumo.CantidadMovi.Value += det.CantidadRec.Value.Value;
                    //        if (saldoConsumo.CantidadMovi.Value.Value > saldoConsumo.CantidadDocu.Value.Value)
                    //        {
                    //            result.Result = ResultValue.NOK;
                    //            result.ResultMessage = "Saldos inválidos en proveedores (prSaldosDocu)";
                    //        }
                               
                    //        else
                    //        {
                    //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Add(saldoRec);
                    //            this._dal_prSaldosDocu.DAL_prSaldosDocu_Upd(saldoConsumo);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        result.Result = ResultValue.NOK;
                    //        result.ResultMessage = "Error al traer saldos de Consumo Proyecto: Revisar Codigo";
                    //    } 
                    //}

                }
        #endregion
                #region Cierra el flujo Recibido
                List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RecibidoAprob);
                if (act.Count > 0)
                    this.AsignarFlujo(documentID, numeroDocRecibido, act[0], false, string.Empty);
                #endregion
                #endregion                
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #region Crea la alarma
                if (result.Result == ResultValue.OK)
                {
                    alarma = this.GetFirstMailInfo(numeroDocRecibido, false);
                    return alarma;
                }
                else
                    return result;
                #endregion              
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "RecibidoConvenios_Add");

                return result;
            }
            finally
            {
                porcTotal += porcParte;
                batchProgress[tupProgress] = 100;

                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        #region Genera Consecutivos
                        DTO_glDocumentoControl docCtrlRecib = this._moduloGlobal.glDocumentoControl_GetByID(numeroDocRecibido);
                        docCtrlRecib.DocumentoNro.Value = this.GenerarDocumentoNro(docCtrlRecib.DocumentoID.Value.Value, docCtrlRecib.PrefijoID.Value);
                        alarma.Consecutivo = docCtrlRecib.DocumentoNro.Value.ToString();
                        this._moduloGlobal.ActualizaConsecutivos(docCtrlRecib, true, false, false); 
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }
        #endregion

        #endregion

        #region Polizas

        /// <summary>
        /// Guardar el documento
        /// </summary>
        /// <param name="polizas">Polizas a Guardar</param>
        /// <returns>si la operacion es exitosa</returns>
        private DTO_TxResult ContratoPolizas_Add(List<DTO_prContratoPolizas> polizas)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_prContratoPolizas = (DAL_prContratoPolizas)base.GetInstance(typeof(DAL_prContratoPolizas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prContratoPolizas.DAL_prContratoPolizas_Add(polizas);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ContratoPolizas_Add");
                return result;
            }
        }

        /// <summary>
        /// Actualiza el documento
        /// </summary>
        /// <param name="polizas">Polizas a actualizar</param>
        /// <returns>si la operacion es exitosa</returns>
        private DTO_TxResult ContratoPolizas_Upd(List<DTO_prContratoPolizas> polizas,int numeroDoc)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this.ContratoPolizas_Delete(numeroDoc);
                this.ContratoPolizas_Add(polizas);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ContratoPolizas_Upd");
                return result;
            }
        }

        /// <summary>
        /// Carga la informacion completa de las polizas
        /// </summary>
        /// <param name="numeroDoc">Numero Doc</param>
        /// <returns>Retorna el  ContratoPolizas</returns>
        private List<DTO_prContratoPolizas> ContratoPolizas_GetByNumeroDoc(int numeroDoc)
        {
            this._dal_prContratoPolizas = (DAL_prContratoPolizas)base.GetInstance(typeof(DAL_prContratoPolizas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prContratoPolizas.DAL_prContratoPolizas_GetByNumeroDoc(numeroDoc);
        }

        /// <summary>
        /// Borra una poliza
        /// </summary>
        /// <param name="documentID">numeroDoc</param>
        private void ContratoPolizas_Delete(int numeroDoc)
        {
            this._dal_prContratoPolizas = (DAL_prContratoPolizas)base.GetInstance(typeof(DAL_prContratoPolizas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prContratoPolizas.DAL_prContratoPolizas_Delete(numeroDoc);
        }

        #endregion

        #region prDetalleCargos

        /// <summary>
        /// Trae la lista de prDetalleCargos segun el numero del documento
        /// </summary>
        /// <param name="NumeroDoc">Numero del documento</param>
        /// <returns></returns>
        private List<DTO_prDetalleCargos> prDetalleCargos_GetByNumeroDoc(int NumeroDoc)
        {
            this._dal_prDetalleCargos = (DAL_prDetalleCargos)base.GetInstance(typeof(DAL_prDetalleCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_prDetalleCargos.DAL_prDetalleCargos_GetByNumeroDoc(NumeroDoc);
        }

        /// <summary>
        /// Guarda la lista de prDetalleCargos en base de datos
        /// </summary>
        /// <param name="solCargos">la lista de DTO_prSolicitudCargos</param>
        /// <returns></returns>
        private DTO_TxResult prDetalleCargos_Add(List<DTO_prDetalleCargos> solCargos)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                this._dal_prDetalleCargos = (DAL_prDetalleCargos)base.GetInstance(typeof(DAL_prDetalleCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleCargos.DAL_prDetalleCargos_Add(solCargos);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "prDetalleCargos_Add");
                return result;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prDetalleCargos
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        private void prDetalleCargos_Delete(int NumeroDoc)
        {
            this._dal_prDetalleCargos = (DAL_prDetalleCargos)base.GetInstance(typeof(DAL_prDetalleCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_prDetalleCargos.DAL_prDetalleCargos_Delete(NumeroDoc);
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
        public DTO_TxResult Proveedores_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls, ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();

            List<DTO_coComprobante> coCompsReversion = null;
            List<DTO_glDocumentoControl> ctrlsReversion = null;
            List<DTO_prDetalleDocu> footerDeta = new List<DTO_prDetalleDocu>();
            DTO_prDetalleDocu detaDocu = null;

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
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Trae detalle(DetalleDocu) del Documento con un filtro
                DTO_glDocumentoControl ctrlSelect = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                switch (ctrlSelect.DocumentoID.Value)
                {
                    case AppDocuments.Solicitud:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.SolicitudDocuID.Value = numeroDoc;
                        break;
                    case AppDocuments.SolicitudDirecta:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.Documento5ID.Value = numeroDoc;
                        break;
                    case AppDocuments.OrdenCompra:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.OrdCompraDocuID.Value = numeroDoc;
                        break;
                    case AppDocuments.Contrato:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.ContratoDocuID.Value = numeroDoc;
                        break;
                    case AppDocuments.ConsumoProyecto:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.Documento1ID.Value = numeroDoc;
                        break;
                    case AppDocuments.SolicitudDespachoConvenio:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.Documento1ID.Value = numeroDoc;
                        break;
                    case AppDocuments.Recibido:
                        detaDocu = new DTO_prDetalleDocu();
                        detaDocu.RecibidoDocuID.Value = numeroDoc;
                        break;
                }
                footerDeta = this.prDetalleDocu_GetParameter(detaDocu);
                #endregion

                foreach (DTO_prDetalleDocu footer in footerDeta)
                {
                    #region Solicitud
                    if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.Solicitud)
                    {
                        if (footer.NumeroDoc.Value != footer.SolicitudDocuID.Value)
                        {
                            #region Orden de Compra
                            if (footer.NumeroDoc.Value == footer.OrdCompraDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.OrdCompraDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "OrdenCompra";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                        else
                        { 
                            //Valida si la solicitud viene del modulo de Proyectos
                            if (footer.Detalle4ID.Value != null && footer.Documento4ID.Value != null)
                            {
                                //Carga el movimiento de proyectos    
                                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)base.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                DTO_pyProyectoMvto mvto = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(footer.Detalle4ID.Value.Value);
                                if (mvto != null)
                                {
                                    mvto.CantidadPROV.Value = mvto.CantidadPROV.Value - footer.CantidadDoc4.Value;
                                    this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvto); 
                                }                                   
                            }                        
                        }
                    }
                    #endregion
                    #region Solicitud Directa
                    if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.SolicitudDirecta)
                    {
                        if (footer.NumeroDoc.Value != footer.Documento5ID.Value)
                        {
                            #region Recibido
                            if (footer.NumeroDoc.Value == footer.RecibidoDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.RecibidoDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "Recibido";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Orden Compra
                    else if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.OrdenCompra)
                    {
                        if (footer.NumeroDoc.Value != footer.OrdCompraDocuID.Value)
                        {
                            #region Recibido
                            if (footer.NumeroDoc.Value == footer.RecibidoDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.RecibidoDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "Recibido";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                        #region Contrato
                        if (footer.NumeroDoc.Value == footer.ContratoDocuID.Value)
                        {
                            DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.ContratoDocuID.Value.Value);
                            if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = "Contrato";
                                rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                rd.DetailsFields.Add(rdF);
                                result.Details.Add(rd);
                            }
                        }
                        #endregion

                    }
                    #endregion
                    #region Recibido
                    else if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.Recibido)
                    {
                        if (footer.NumeroDoc.Value != footer.RecibidoDocuID.Value)
                        {
                            #region Factura
                            if (footer.NumeroDoc.Value == footer.FacturaDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.FacturaDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "Factura";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //Valida si el recibido viene del modulo de Proyectos
                            if (footer.Detalle4ID.Value != null && footer.Documento4ID.Value != null)
                            {
                                //Carga el movimiento de proyectos    
                                this._dal_pyProyectoMvto = (DAL_pyProyectoMvto)base.GetInstance(typeof(DAL_pyProyectoMvto), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                DTO_pyProyectoMvto mvto = this._dal_pyProyectoMvto.DAL_pyProyectoMvto_GetByConsecutivo(footer.Detalle4ID.Value.Value);
                                if (mvto != null)
                                {
                                    mvto.CantidadPROV.Value = mvto.CantidadREC.Value - footer.CantidadDoc4.Value;
                                    this._dal_pyProyectoMvto.DAL_pyProyectoMvto_Upd(mvto);
                                }
                            }


                            this._dal_prDetalleDocu.DAL_prDetalleDocu_Delete(footer.ConsecutivoDetaID.Value.Value);   
                        }
                    }
                    #endregion
                    #region Consumo Proyecto
                    else if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.ConsumoProyecto)
                    {
                        if (footer.NumeroDoc.Value != footer.Documento1ID.Value)
                        {
                            #region Recibido
                            if (footer.NumeroDoc.Value == footer.RecibidoDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.RecibidoDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "Recibido";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Solicitud Despacho
                    else if (ctrlSelect.DocumentoID.Value == (byte)AppDocuments.SolicitudDespachoConvenio)
                    {
                        if (footer.NumeroDoc.Value != footer.Documento1ID.Value)
                        {
                            #region Recibido
                            if (footer.NumeroDoc.Value == footer.RecibidoDocuID.Value)
                            {
                                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(footer.RecibidoDocuID.Value.Value);
                                if (ctrl.Estado.Value != (byte)EstadoDocControl.Revertido && ctrl.Estado.Value != (byte)EstadoDocControl.Anulado)
                                {
                                    result.Result = ResultValue.NOK;
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = "Recibido";
                                    rdF.Message = DictionaryMessages.Pr_ReversionInvalid + "&&" + numeroDoc + "&&" + ctrl.NumeroDoc.Value.ToString();
                                    rd.DetailsFields.Add(rdF);
                                    result.Details.Add(rd);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }

                if (result.Result == ResultValue.NOK)
                    return result;

                #region Revierte el documento
                result = this._moduloGlobal.glDocumentoControl_Revertir(ctrlSelect.DocumentoID.Value.Value, numeroDoc, consecutivoPos, ref ctrlsReversion, ref coCompsReversion, true);
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Proveedores_Revertir");
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

                        for (int i = 0; i < ctrlsReversion.Count; ++i)
                        {
                            DTO_glDocumentoControl ctrlAnula = ctrlsReversion[i];
                            DTO_coComprobante coCompAnula = coCompsReversion[i];

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

        #region Provision (Proceso)

        /// <summary>
        /// Guarda documento de Provision con un comprobante de los Recibidos Pendientes por facturar
        /// </summary>
        /// <param name="documentID">documento de provision</param>
        /// <returns>Lista de Resultados</returns>
        public DTO_TxResult Provision_RecibidoNotFacturadoAdd(int documentID,Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_Comprobante comprobante = null;
            DTO_coComprobante comprobanteDto = null;
            DTO_glDocumentoControl ctrlProvision = new DTO_glDocumentoControl() ;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();   
            try
            {
                #region Trae Recibidos No Facturados
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prRecibidoDocu = (DAL_prRecibidoDocu)base.GetInstance(typeof(DAL_prRecibidoDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumento doc = (DTO_glDocumento)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, false);
                List<DTO_prRecibidoAprob> listRecibidos = this._dal_prRecibidoDocu.DAL_prRecibidoDocu_GetRecibidoNoFacturado(doc,string.Empty, 0);
                #endregion
                #region Trae variables por defecto
                string lugarGeoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string proyectoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string centroCtoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lineaPresxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string conceptoCargoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string prefijoXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                DateTime periodo = DateTime.Parse(this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo));
                string coDocumentoProvision = this.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DocContableProvisiones);
                string terceroIDXDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranj = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string monedaProvision =listRecibidos.Count > 0? listRecibidos.First().MonedaID.Value: monedaLocal;
                bool isML = monedaLocal == monedaProvision ? true : false;
                decimal tasaCambio =  !isML? this._moduloGlobal.TasaDeCambio_Get(monedaExtranj, DateTime.Now) : 0; 
                #endregion
                #region Validaciones
                //Valida el coDocumento
                if (string.IsNullOrWhiteSpace(coDocumentoProvision))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }
                DTO_coDocumento coDocumento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento,coDocumentoProvision, true, false);
                //Revisa que tenga comprobante
                if (coDocumento != null && string.IsNullOrWhiteSpace(coDocumento.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                //Valida que el documento asociado tenga cuenta
                if ( (isML && string.IsNullOrWhiteSpace(coDocumento.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDocumento.CuentaEXT.Value)))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDocumento.ID.Value;
                    return result;
                }
                //Valida que la cuenta sea de Componente Doc
                string ctaIDProvision = isML ? coDocumento.CuentaLOC.Value : coDocumento.CuentaEXT.Value;
                DTO_coPlanCuenta ctaProvis = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaIDProvision, true, false);
                DTO_glConceptoSaldo concepSaldoProvis = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaProvis.ConceptoSaldoID.Value, true, false);
                if (concepSaldoProvis.coSaldoControl.Value.Value != (Int16)SaldoControl.Componente_Documento)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_ConcSaldoContraInvalid + "&&" + ctaIDProvision.Trim();
                    return result;
                }      
                //Revisa que el modulo se encuentre abierto
                EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.pr);
                if (estado != EstadoPeriodo.Abierto)
                {
                    if (estado == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (estado == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }          
                comprobanteDto = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDocumento.ComprobanteID.Value, true, false);
                #endregion
                #region Crea y Guarda el documento de provisiones (glDocumentoCtrl)
                ctrlProvision.EmpresaID.Value = this.Empresa.ID.Value;
                ctrlProvision.PeriodoDoc.Value = periodo;
                ctrlProvision.PeriodoUltMov.Value = periodo;
                ctrlProvision.NumeroDoc.Value = 0;
                ctrlProvision.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                ctrlProvision.DocumentoID.Value = AppDocuments.ProvisionRecibNoFact;
                ctrlProvision.DocumentoNro.Value = 0;
                ctrlProvision.CuentaID.Value = ctaIDProvision;
                ctrlProvision.ComprobanteID.Value = coDocumento.ComprobanteID.Value;
                ctrlProvision.ComprobanteIDNro.Value = 0;
                ctrlProvision.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrlProvision.TerceroID.Value = terceroIDXDef; 
                ctrlProvision.MonedaID.Value = monedaProvision;
                ctrlProvision.ProyectoID.Value = proyectoxDef; 
                ctrlProvision.CentroCostoID.Value = centroCtoXDef; 
                ctrlProvision.LugarGeograficoID.Value = lugarGeoxDef; 
                ctrlProvision.LineaPresupuestoID.Value = lineaPresxDef;
                ctrlProvision.Fecha.Value = DateTime.Now;
                ctrlProvision.PrefijoID.Value = prefijoXDef;
                ctrlProvision.TasaCambioCONT.Value = tasaCambio;
                ctrlProvision.TasaCambioDOCU.Value = tasaCambio;              
                ctrlProvision.seUsuarioID.Value = this.UserId;
                ctrlProvision.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                ctrlProvision.ConsSaldo.Value = 0;
                ctrlProvision.FechaDoc.Value = DateTime.Now.Date;
                ctrlProvision.Descripcion.Value = "Provision Recibidos No Facturados";
                ctrlProvision.Valor.Value = 0;
                ctrlProvision.Iva.Value = 0;
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, ctrlProvision, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }
                ctrlProvision.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                #endregion               
                #region  Crea el comprobante
                comprobante = new DTO_Comprobante();              
                #region Header
                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = comprobanteDto.ID.Value;
                header.ComprobanteNro.Value = 0;
                header.Fecha.Value = ctrlProvision.FechaDoc.Value;
                header.MdaOrigen.Value = ctrlProvision.MonedaID.Value == monedaLocal ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                header.MdaTransacc.Value = ctrlProvision.MonedaID.Value;
                header.NumeroDoc.Value =ctrlProvision.NumeroDoc.Value;
                header.PeriodoID.Value = periodo;
                header.TasaCambioBase.Value = ctrlProvision.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = ctrlProvision.TasaCambioDOCU.Value;
                comprobante.Header = header;
                #endregion
                #region Footer - Recorre los recibidos con su detalle
                foreach (DTO_prRecibidoAprob recibido in listRecibidos)
                {
                    List<DTO_prDetalleDocu> detRecibidos = this.prDetalleDocu_GetByNumeroDoc(recibido.NumeroDoc.Value.Value, false);
                    List<DTO_prSolicitudCargos> cargosSol = new List<DTO_prSolicitudCargos>();
                    if (detRecibidos != null && detRecibidos.Count > 0)
                    {
                        #region Declara variables de datos
                        int index = 0;
                        string operacion = string.Empty;
                        Dictionary<string, DTO_coProyecto> cacheProyecto = new Dictionary<string, DTO_coProyecto>();
                        Dictionary<string, DTO_coCentroCosto> cacheCtoCosto = new Dictionary<string, DTO_coCentroCosto>();                       
                        DTO_coProyecto _proyecto;
                        DTO_coCentroCosto _ctoCosto;
                        #endregion
                        foreach (DTO_prDetalleDocu det in detRecibidos)
                        {
                            #region Carga datos iniciales
                            DTO_prBienServicio bienServicio = (DTO_prBienServicio)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, det.CodigoBSID.Value, true, false);
                            DTO_glBienServicioClase bienServicioClase = (DTO_glBienServicioClase)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, bienServicio.ClaseBSID.Value, true, false);
                            DTO_coConceptoCargo conceptoCargo = (DTO_coConceptoCargo)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coConceptoCargo, bienServicioClase.ConceptoCargoID.Value, true, false);
                            int consecDeta = 0;
                            if      (!string.IsNullOrEmpty(det.SolicitudDetaID.Value.ToString()))  consecDeta = det.SolicitudDetaID.Value.Value; //Solicitud Proveedor
                            else if (!string.IsNullOrEmpty(det.Detalle1ID.Value.ToString())) consecDeta = det.Detalle1ID.Value.Value;  //Planilla Consumo Proyecto o Solicitud Despacho
                            else if (!string.IsNullOrEmpty(det.Detalle5ID.Value.ToString())) consecDeta = det.Detalle5ID.Value.Value; //Solicitud Directa Proveedor
                            cargosSol = this.prSolicitudCargos_GetByConsecutivoDetaID(documentID,consecDeta);
                            if (cargosSol.Count == 0)
                            {
                                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                rd.Message = DictionaryMessages.Err_Pr_CargosEmptyRecibido + "&&" + recibido.PrefDoc;
                                rd.line = index;
                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }
                            #endregion
                            foreach (DTO_prSolicitudCargos sol in cargosSol)
                            {
                                decimal valorPorc = det.ValorTotML.Value.Value * (sol.PorcentajeID.Value.Value / 100);
                                #region Carga el proyecto
                                if (cacheProyecto.ContainsKey(sol.ProyectoID.Value))
                                    _proyecto = cacheProyecto[sol.ProyectoID.Value];
                                else
                                {
                                    _proyecto = (DTO_coProyecto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, sol.ProyectoID.Value, true, false);
                                    cacheProyecto.Add(sol.ProyectoID.Value, _proyecto);
                                    operacion = _proyecto.OperacionID.Value;
                                }
                                #endregion
                                #region Carga el Centro Costo
                                if (string.IsNullOrEmpty(operacion))
                                    if (cacheCtoCosto.ContainsKey(sol.CentroCostoID.Value))
                                        _ctoCosto = cacheCtoCosto[sol.CentroCostoID.Value];
                                    else
                                    {
                                        _ctoCosto = (DTO_coCentroCosto)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, sol.CentroCostoID.Value, true, false);
                                        cacheCtoCosto.Add(sol.CentroCostoID.Value, _ctoCosto);
                                        operacion = _ctoCosto.OperacionID.Value;
                                    }
                                #endregion
                                #region Valida la operacion
                                if (string.IsNullOrEmpty(operacion))
                                {
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.Message = DictionaryMessages.Err_Co_NoOper + "&&" + sol.ProyectoID.Value.ToString() + "&&" + sol.CentroCostoID.Value.ToString();
                                    rd.line = index;
                                    result.Details.Add(rd);
                                    result.Result = ResultValue.NOK;
                                }
                                #endregion
                                #region Trae Cuenta (coCargoCosto)
                                DTO_CuentaValor ctaValor = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(conceptoCargo.ID.Value, operacion, bienServicioClase.LineaPresupuestoID.Value, valorPorc);
                                if (ctaValor == null)
                                {
                                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                                    rd.Message = DictionaryMessages.Err_Co_NoCtaCargoCosto + "&&" + conceptoCargo.ID.Value + "&&" + bienServicioClase.LineaPresupuestoID.Value + "&&" + sol.ProyectoID.Value + "&&" + sol.CentroCostoID.Value;
                                    rd.line = index;
                                    result.Details.Add(rd);
                                    result.Result = ResultValue.NOK;
                                }
                                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, ctaValor.CuentaID.Value, true, false);
                                DTO_glConceptoSaldo conceptoSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                #endregion
                                #region Carga el detalle del Comprobante
                                DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
                                newDet.Index = index;
                                newDet.CuentaID.Value = cta.ID.Value;
                                newDet.TerceroID.Value = terceroIDXDef;
                                newDet.ProyectoID.Value = sol.ProyectoID.Value;
                                newDet.CentroCostoID.Value = sol.CentroCostoID.Value;
                                newDet.LineaPresupuestoID.Value = string.IsNullOrEmpty(det.LineaPresupuestoID.Value)? bienServicioClase.LineaPresupuestoID.Value : det.LineaPresupuestoID.Value;
                                newDet.ConceptoCargoID.Value = bienServicioClase.ConceptoCargoID.Value;
                                newDet.LugarGeograficoID.Value = lugarGeoxDef;
                                newDet.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
                                newDet.IdentificadorTR.Value = 0;
                                newDet.PrefijoCOM.Value = recibido.PrefijoID.Value;
                                newDet.DatoAdd10.Value = AuxiliarDatoAdd10.Proveedores.ToString();
                                newDet.DocumentoCOM.Value = recibido.DocumentoNro.Value.ToString();
                                newDet.Descriptivo.Value = det.Descriptivo.Value;
                                newDet.TasaCambio.Value = recibido.TasaCambioDOCU.Value;
                                newDet.vlrBaseML.Value = 0;
                                newDet.vlrBaseME.Value = 0;
                                newDet.vlrMdaLoc.Value = det.ValorTotML.Value;
                                newDet.vlrMdaExt.Value = det.ValorTotME.Value;
                                newDet.vlrMdaOtr.Value = recibido.MonedaID.Value == monedaLocal ? newDet.vlrMdaLoc.Value : newDet.vlrMdaExt.Value;
                                #region Si el saldo control es Cuenta
                                if (conceptoSaldo.coSaldoControl.Value.Value == (byte)SaldoControl.Cuenta)
                                {
                                    #region Tercero
                                    if (!cta.TerceroInd.Value.Value)
                                        newDet.TerceroID.Value = terceroIDXDef;
                                    #endregion
                                    #region Proyecto
                                    if (!cta.ProyectoInd.Value.Value)
                                        newDet.ProyectoID.Value = proyectoxDef;
                                    #endregion
                                    #region Centro Costo
                                    if (!cta.CentroCostoInd.Value.Value)
                                        newDet.CentroCostoID.Value = centroCtoXDef;
                                    #endregion
                                    #region Linea presupuesto
                                    if (!cta.LineaPresupuestalInd.Value.Value)
                                        newDet.LineaPresupuestoID.Value = lineaPresxDef;
                                    #endregion
                                    #region Concepto Cargo
                                    if (!cta.ConceptoCargoInd.Value.Value)
                                        newDet.ConceptoCargoID.Value = conceptoCargoxDef;
                                    #endregion
                                    #region Lugar Geografico
                                    if (!cta.LugarGeograficoInd.Value.Value)
                                        newDet.LugarGeograficoID.Value = lugarGeoxDef;
                                    #endregion
                                    #region Prefijo
                                    newDet.PrefijoCOM.Value = prefijoXDef;
                                    #endregion
                                }
                                #endregion
                                comprobante.Footer.Add(newDet);
                                #endregion
                            }
                            index++;
                        }                        
                    }
                    #region Contrapartida
                    if (cargosSol.Count > 0)
                    {
                        DTO_ComprobanteFooter contrapartidaRec = new DTO_ComprobanteFooter();
                        contrapartidaRec.CuentaID.Value = ctaIDProvision;
                        contrapartidaRec.TerceroID.Value = terceroIDXDef;
                        contrapartidaRec.ProyectoID.Value = proyectoxDef;
                        contrapartidaRec.CentroCostoID.Value = centroCtoXDef;
                        contrapartidaRec.LineaPresupuestoID.Value = lineaPresxDef;
                        contrapartidaRec.ConceptoCargoID.Value = conceptoCargoxDef;
                        contrapartidaRec.LugarGeograficoID.Value = lugarGeoxDef;
                        contrapartidaRec.ConceptoSaldoID.Value = ctaProvis.ConceptoSaldoID.Value;
                        contrapartidaRec.IdentificadorTR.Value = recibido.NumeroDoc.Value;
                        contrapartidaRec.PrefijoCOM.Value = recibido.PrefijoID.Value;
                        contrapartidaRec.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                        contrapartidaRec.DocumentoCOM.Value = recibido.DocumentoNro.Value.ToString();
                        contrapartidaRec.TasaCambio.Value = recibido.TasaCambioDOCU.Value;
                        contrapartidaRec.vlrBaseML.Value = 0;
                        contrapartidaRec.vlrBaseME.Value = 0;
                        contrapartidaRec.vlrMdaLoc.Value = detRecibidos.Sum(x => x.ValorTotML.Value) * (-1);
                        contrapartidaRec.vlrMdaExt.Value = detRecibidos.Sum(x => x.ValorTotME.Value) * (-1);
                        contrapartidaRec.vlrMdaOtr.Value = recibido.MonedaID.Value == monedaLocal ? contrapartidaRec.vlrMdaLoc.Value : contrapartidaRec.vlrMdaExt.Value;
                        comprobante.Footer.Add(contrapartidaRec);
                    }
                    #endregion
                }    
                #endregion
                #endregion
               
                #region Genera el comprobante
                if (result.Result == ResultValue.OK)
                {
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comprobante, periodo, ModulesPrefix.pr, 0, false);
                    if (result.Result == ResultValue.NOK)
                    {
                        result.ResultMessage = DictionaryMessages.Err_Co_NoContab;
                        return result;
                    }
                }
                return result;
                #endregion
            }
           catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Provision_ProcessRecibidoNotFacturado");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Asigna consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        ctrlProvision.DocumentoNro.Value = this.GenerarDocumentoNro(ctrlProvision.DocumentoID.Value.Value, ctrlProvision.PrefijoID.Value);
                        ctrlProvision.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comprobanteDto, ctrlProvision.PrefijoID.Value, ctrlProvision.PeriodoDoc.Value.Value, ctrlProvision.DocumentoNro.Value.Value);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlProvision.NumeroDoc.Value.Value, ctrlProvision.ComprobanteIDNro.Value.Value, false);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlProvision, true, true, false);
                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Consultas

        /// <summary>
        /// Trae un listado de las documentos con detalle como consulta
        /// </summary>
        /// <param name="documentID">documento relacionado</param>
        /// <param name="ctrls">Lista de documentos a consultar</param>
        /// <returns>Detalle de la consulta</returns>
        public List<DTO_ConsultaCompras> ConsultaCompras_Get(int documentID, List<DTO_glDocumentoControl> ctrls)
        {
            try
            {
                this._dal_prSolicitudDocu = (DAL_prSolicitudDocu)base.GetInstance(typeof(DAL_prSolicitudDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prSolicitudCargos = (DAL_prSolicitudCargos)base.GetInstance(typeof(DAL_prSolicitudCargos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_prDetalleDocu = (DAL_prDetalleDocu)base.GetInstance(typeof(DAL_prDetalleDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_ConsultaCompras> list = new List<DTO_ConsultaCompras>();

                if (documentID == AppDocuments.Solicitud)
                {
                    //Recorre los documentos seleccionados
                    foreach (DTO_glDocumentoControl doc in ctrls)
                    {
                        DTO_ConsultaCompras consulta = new DTO_ConsultaCompras();
                        decimal cantSolicitudTotal = 0;
                        decimal cantOrdCompraTotal = 0;
                        decimal cantRecibidoTotal = 0;
                        //Carga la Solicitud
                        DTO_prSolicitud sol = this.Solicitud_Load(documentID, doc.PrefijoID.Value, doc.DocumentoNro.Value.Value);
                        if (sol != null)
                        {
                            #region Asigna valores
                            consulta.PrefDoc.Value = sol.DocCtrl.PrefDoc.Value;
                            consulta.NumeroDoc.Value = sol.DocCtrl.NumeroDoc.Value;
                            consulta.Fecha.Value = sol.DocCtrl.FechaDoc.Value;
                            consulta.AreaFuncionalID.Value = sol.DocCtrl.AreaFuncionalID.Value;
                            consulta.Estado.Value = sol.DocCtrl.Estado.Value.Value;
                            #region Recorre el detalle de la Solicitud
                            foreach (DTO_prSolicitudFooter footer in sol.Footer)
                            {
                                DTO_ConsultaComprasDet consultaDet = new DTO_ConsultaComprasDet();
                                consultaDet.CodigoBSID.Value = footer.DetalleDocu.CodigoBSID.Value;
                                consultaDet.inReferenciaID.Value = footer.DetalleDocu.inReferenciaID.Value;
                                if (!string.IsNullOrEmpty(consultaDet.inReferenciaID.Value))
                                {
                                    DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, consultaDet.inReferenciaID.Value, true, false);
                                    if (refer != null)
                                    {
                                        consultaDet.MarcaInvID.Value = refer.MarcaInvDesc.Value;
                                        consultaDet.RefProveedor.Value = refer.RefProveedor.Value;
                                    }
                                }                                
                                consultaDet.Descriptivo.Value = footer.DetalleDocu.Descriptivo.Value;
                                consultaDet.SolicitudDetaID.Value = footer.DetalleDocu.SolicitudDetaID.Value;
                                consultaDet.SolicitudDocuID.Value = footer.DetalleDocu.NumeroDoc.Value;
                                consultaDet.CantidadSol.Value = footer.DetalleDocu.CantidadSol.Value;
                                consultaDet.ProyectoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).ProyectoID.Value;
                                consultaDet.CentroCostoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).CentroCostoID.Value;
                                consultaDet.CantidadOC.Value = 0;
                                consultaDet.CantidadRec.Value = 0;

                                #region Consulta si tiene documentos relacionados
                                DTO_prDetalleDocu detalle = new DTO_prDetalleDocu();
                                detalle.SolicitudDocuID.Value = footer.DetalleDocu.SolicitudDocuID.Value.Value;
                                detalle.SolicitudDetaID.Value = footer.DetalleDocu.SolicitudDetaID.Value;
                                List<DTO_prDetalleDocu> listDetalle = this.prDetalleDocu_GetParameter(detalle);
                                foreach (DTO_prDetalleDocu detDocu in listDetalle)
                                {
                                    //Excluye el detalle de solicitudes
                                    if (detDocu.NumeroDoc.Value != detDocu.SolicitudDocuID.Value)
                                    {
                                        //Valida si es de Orden Compra
                                        if (detDocu.OrdCompraDocuID.Value != null && detDocu.RecibidoDocuID.Value == null)
                                        {
                                            consultaDet.CantidadOC.Value += detDocu.CantidadOC.Value.Value;
                                            cantOrdCompraTotal += detDocu.CantidadOC.Value.Value;
                                        }
                                        //Valida si es de Recibido
                                        else if (detDocu.RecibidoDocuID.Value != null)
                                        {
                                            consultaDet.CantidadRec.Value += detDocu.CantidadRec.Value.Value;
                                            cantRecibidoTotal += detDocu.CantidadRec.Value.Value;
                                        }
                                    }
                                    else
                                        cantSolicitudTotal += consultaDet.CantidadSol.Value.Value;
                                }
                                consulta.Detalle.Add(consultaDet);
                                #endregion
                            }
                            #endregion
                            consulta.CantidadSol.Value = cantSolicitudTotal;
                            consulta.CantidadOC.Value = cantOrdCompraTotal;
                            consulta.CantidadRec.Value = cantRecibidoTotal;
                            consulta.CantidadPendOC.Value = cantOrdCompraTotal == 0 ? 0 : cantSolicitudTotal - cantOrdCompraTotal;
                            consulta.CantidadPendRec.Value = cantRecibidoTotal == 0 ? 0 : cantOrdCompraTotal - cantRecibidoTotal;
                            list.Add(consulta);
                            #endregion
                        }
                    }
                }
                else if (documentID == AppDocuments.OrdenCompra)
                {
                    string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    //Recorre los documentos seleccionados
                    foreach (DTO_glDocumentoControl doc in ctrls)
                    {
                        DTO_ConsultaCompras consulta = new DTO_ConsultaCompras();
                        decimal cantSolicitudTotal = 0;
                        decimal cantOrdCompraTotal = 0;
                        decimal cantRecibidoTotal = 0;
                        //Carga la Orden Compra
                        DTO_prOrdenCompra oc = this.OrdenCompra_Load(documentID, doc.PrefijoID.Value, doc.DocumentoNro.Value.Value);
                        if (oc != null)
                        {
                            #region Asigna valores
                            consulta.PrefDoc.Value = oc.DocCtrl.PrefDoc.Value;
                            consulta.NumeroDoc.Value = oc.DocCtrl.NumeroDoc.Value;
                            consulta.Fecha.Value = oc.DocCtrl.FechaDoc.Value;
                            consulta.ProveedorID.Value = oc.HeaderOrdenCompra.ProveedorID.Value;
                            DTO_prProveedor prov = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, oc.HeaderOrdenCompra.ProveedorID.Value, true, false);
                            consulta.ProveedorNombre.Value = prov.Descriptivo.Value;
                            consulta.Estado.Value = oc.DocCtrl.Estado.Value.Value;
                            consulta.MonedaPago.Value = oc.HeaderOrdenCompra.MonedaPago.Value;
                            consulta.MonedaOC.Value = oc.HeaderOrdenCompra.MonedaOrden.Value;
                            #region Recorre el detalle de la OrdenCompra
                            foreach (DTO_prOrdenCompraFooter footer in oc.Footer)
                            {
                                DTO_ConsultaComprasDet consultaDet = new DTO_ConsultaComprasDet();
                                consultaDet.CodigoBSID.Value = footer.DetalleDocu.CodigoBSID.Value;
                                consultaDet.inReferenciaID.Value = footer.DetalleDocu.inReferenciaID.Value;
                                if (!string.IsNullOrEmpty(consultaDet.inReferenciaID.Value))
                                {
                                    DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, consultaDet.inReferenciaID.Value, true, false);
                                    if (refer != null)
                                    {
                                        consultaDet.MarcaInvID.Value = refer.MarcaInvDesc.Value;
                                        consultaDet.RefProveedor.Value = refer.RefProveedor.Value;
                                    }
                                }  
                                consultaDet.Descriptivo.Value = footer.DetalleDocu.Descriptivo.Value;
                                consultaDet.SolicitudDocuID.Value = footer.DetalleDocu.SolicitudDocuID.Value;
                                consultaDet.SolicitudDetaID.Value = footer.DetalleDocu.SolicitudDetaID.Value;
                                consultaDet.OrdCompraDetaID.Value = footer.DetalleDocu.OrdCompraDetaID.Value;
                                consultaDet.OrdCompraDocuID.Value = footer.DetalleDocu.NumeroDoc.Value;
                                consultaDet.CantidadOC.Value = footer.DetalleDocu.CantidadOC.Value;
                                consultaDet.ProyectoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).ProyectoID.Value;
                                consultaDet.CentroCostoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).CentroCostoID.Value;
                                consultaDet.CantidadSol.Value = 0;
                                consultaDet.CantidadRec.Value = 0;

                                #region Consulta si tiene documentos relacionados
                                DTO_prDetalleDocu detalle = new DTO_prDetalleDocu();
                                detalle.OrdCompraDocuID.Value = footer.DetalleDocu.OrdCompraDocuID.Value.Value;
                                detalle.OrdCompraDetaID.Value = footer.DetalleDocu.OrdCompraDetaID.Value;
                                List<DTO_prDetalleDocu> listDetalle = this.prDetalleDocu_GetParameter(detalle);
                                foreach (DTO_prDetalleDocu detDocu in listDetalle)
                                {
                                    //Valida si es de Recibido
                                    if (detDocu.RecibidoDocuID.Value != null)
                                    {
                                        consultaDet.CantidadRec.Value += detDocu.CantidadRec.Value.Value;
                                        cantRecibidoTotal += detDocu.CantidadRec.Value.Value;
                                    }
                                    else
                                        cantOrdCompraTotal += consultaDet.CantidadOC.Value.Value;
                                }
                                consulta.Detalle.Add(consultaDet);
                                #endregion
                            }
                            #endregion
                            consulta.CantidadSol.Value = cantSolicitudTotal;
                            consulta.CantidadOC.Value = cantOrdCompraTotal;
                            consulta.CantidadRec.Value = cantRecibidoTotal;
                            consulta.CantidadPendOC.Value = 0;
                            consulta.CantidadPendRec.Value = cantRecibidoTotal == 0 ? 0 : cantOrdCompraTotal - cantRecibidoTotal;
                            consulta.Valor.Value = monedaLocal == oc.HeaderOrdenCompra.MonedaOrden.Value ? oc.Footer.Sum(x => x.DetalleDocu.ValorTotML.Value) : oc.Footer.Sum(x => x.DetalleDocu.ValorTotME.Value);
                            list.Add(consulta);
                            #endregion
                        }
                    }
                }
                else if (documentID == AppDocuments.Recibido)
                {
                    string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    //Recorre los documentos seleccionados
                    foreach (DTO_glDocumentoControl doc in ctrls)
                    {
                        DTO_ConsultaCompras consulta = new DTO_ConsultaCompras();
                        decimal cantRecibidoTotal = 0;
                        decimal cantFacturaTotal = 0;
                        //Carga el Recibido
                        DTO_prRecibido rec = this.Recibido_Load(documentID, doc.PrefijoID.Value, doc.DocumentoNro.Value.Value, 0);
                        if (rec != null)
                        {
                            #region Asigna valores
                            consulta.PrefDoc.Value = rec.DocCtrl.PrefDoc.Value;
                            consulta.NumeroDoc.Value = rec.DocCtrl.NumeroDoc.Value;
                            consulta.Fecha.Value = rec.DocCtrl.FechaDoc.Value;
                            consulta.AreaFuncionalID.Value = rec.DocCtrl.AreaFuncionalID.Value;
                            consulta.ProveedorID.Value = rec.Header.ProveedorID.Value;
                            DTO_prProveedor prov = (DTO_prProveedor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, rec.Header.ProveedorID.Value, true, false);
                            consulta.ProveedorNombre.Value = prov.Descriptivo.Value;
                            DTO_inBodega bod = (DTO_inBodega)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, rec.Header.BodegaID.Value, true, false);
                            if (bod != null)
                                consulta.Bodega.Value = bod.ID.Value + "-"+ bod.Descriptivo.Value;
                            consulta.Estado.Value = rec.DocCtrl.Estado.Value.Value;
                            #region Recorre el detalle de la OrdenCompra
                            foreach (DTO_prOrdenCompraFooter footer in rec.Footer)
                            {
                                DTO_ConsultaComprasDet consultaDet = new DTO_ConsultaComprasDet();
                                consultaDet.CodigoBSID.Value = footer.DetalleDocu.CodigoBSID.Value;
                                consultaDet.inReferenciaID.Value = footer.DetalleDocu.inReferenciaID.Value;
                                if (!string.IsNullOrEmpty(consultaDet.inReferenciaID.Value))
                                {
                                    DTO_inReferencia refer = (DTO_inReferencia)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, consultaDet.inReferenciaID.Value, true, false);
                                    if (refer != null)
                                    {
                                        consultaDet.MarcaInvID.Value = refer.MarcaInvDesc.Value;
                                        consultaDet.RefProveedor.Value = refer.RefProveedor.Value;
                                    }
                                }  
                                consultaDet.Descriptivo.Value = footer.DetalleDocu.Descriptivo.Value;
                                consultaDet.SolicitudDocuID.Value = footer.DetalleDocu.SolicitudDocuID.Value;
                                consultaDet.SolicitudDetaID.Value = footer.DetalleDocu.SolicitudDetaID.Value;
                                consultaDet.OrdCompraDocuID.Value = footer.DetalleDocu.OrdCompraDocuID.Value;
                                consultaDet.OrdCompraDetaID.Value = footer.DetalleDocu.OrdCompraDetaID.Value;
                                consultaDet.RecibidoDocuID.Value = footer.DetalleDocu.NumeroDoc.Value;
                                consultaDet.RecibidoDetaID.Value = footer.DetalleDocu.RecibidoDetaID.Value;
                                consultaDet.CantidadRec.Value = footer.DetalleDocu.CantidadRec.Value;
                                consultaDet.ProyectoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).ProyectoID.Value;
                                consultaDet.CentroCostoID.Value = footer.SolicitudCargos.Find(x => x.ConsecutivoDetaID.Value == footer.DetalleDocu.SolicitudDetaID.Value).CentroCostoID.Value;
                                consultaDet.CantidadSol.Value = 0;
                                consultaDet.CantidadOC.Value = 0;
                                consultaDet.CantidadFact.Value = 0;
                                cantRecibidoTotal += consultaDet.CantidadRec.Value.Value;

                                #region Consulta si tiene documentos relacionados
                                DTO_prDetalleDocu detalle = new DTO_prDetalleDocu();
                                detalle.RecibidoDocuID.Value = footer.DetalleDocu.RecibidoDocuID.Value.Value;
                                detalle.RecibidoDetaID.Value = footer.DetalleDocu.RecibidoDetaID.Value;
                                List<DTO_prDetalleDocu> listDetalle = this.prDetalleDocu_GetParameter(detalle);
                                foreach (DTO_prDetalleDocu detDocu in listDetalle)
                                {
                                    //Valida si es de Facturacion
                                    if (detDocu.FacturaDocuID.Value != null)
                                    {
                                        consultaDet.CantidadFact.Value = detDocu.CantidadRec.Value.Value;
                                        cantFacturaTotal += consultaDet.CantidadFact.Value.Value;
                                    }
                                }
                                consulta.Detalle.Add(consultaDet);
                                #endregion
                            }
                            #endregion
                            consulta.CantidadSol.Value = 0;
                            consulta.CantidadOC.Value = 0;
                            consulta.CantidadRec.Value = cantRecibidoTotal;
                            consulta.CantidadFact.Value = cantFacturaTotal;
                            consulta.Valor.Value = monedaLocal == rec.DocCtrl.MonedaID.Value ? rec.Footer.Sum(x => x.DetalleDocu.ValorTotML.Value) : rec.Footer.Sum(x => x.DetalleDocu.ValorTotME.Value);
                            list.Add(consulta);
                            #endregion
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ConsultaCompras_Get");
                return null;
            }
        }

        #endregion

        #region Reportes

        #region Reporte en esquema Viejo
        // <summary>
        //Crea un dto de reporte Solicitud
        //</summary>
        //<param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        //<returns></returns>
        internal object DtoReportSolicitud(int numeroDoc, bool isApro)
        {
            try
            {
                #region Variables

                //El Dto a Devolver
                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                List<DTO_prDetalleDocu> prDetails = prDetalleDocu_GetByNumeroDoc(numeroDoc, false);
                DTO_prSolicitudDocu solicDTO = prSolicitudDocu_Get(numeroDoc);
                docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);

                #endregion

                #region Asignar los datos para el reporte

                DTO_ReportSolicitud reportSolic = new DTO_ReportSolicitud();
                DTO_prDetalleDocu detalle = new DTO_prDetalleDocu();
                foreach (var detalleDoc in prDetails)
                {
                    detalle.SolicitudDocuID.Value = detalleDoc.SolicitudDocuID.Value;
                    detalle.EstadoInv.Value = detalleDoc.EstadoInv.Value;
                }

                #region Header

                DTO_seUsuario user = _moduloGlobal.seUsuario_GetUserByReplicaID(docCtrl.seUsuarioID.Value.Value);
                DTO_seUsuario usuarioDesc = (DTO_seUsuario)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, user.ID.Value, true, false);
                reportSolic.Header.Digitador = usuarioDesc.Descriptivo.Value;
                reportSolic.Header.Solicitante = solicDTO.UsuarioSolicita.Value;
                reportSolic.Header.Solicitud = docCtrl.PrefijoID.Value + " - " + docCtrl.DocumentoNro.Value;
                reportSolic.Header.Fecha = docCtrl.Fecha.Value.Value;
                if (detalle.EstadoInv.Value != null)
                    reportSolic.Header.Estado = detalle.EstadoInv.Value.Value;
                reportSolic.Header.Documento = docCtrl.DocumentoTercero.Value;
                if (isApro)
                    reportSolic.isApro = true;
                else
                    reportSolic.isApro = false;

                #endregion
                #region Detail

                foreach (DTO_prDetalleDocu detail in prDetails)
                {
                    DTO_ReportSolicDetail reportSolicDetail = new DTO_ReportSolicDetail();
                    DTO_inReferencia inRefDesc = (DTO_inReferencia)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, detail.inReferenciaID.Value, true, false);

                    reportSolicDetail.CodigoBSID = detail.CodigoBSID.Value;
                    if (inRefDesc != null)
                        reportSolicDetail.inReferenciaID = inRefDesc.Descriptivo.Value;
                    reportSolicDetail.Descripcion = detail.Descriptivo.Value;
                    reportSolicDetail.UnidadInv = detail.UnidadInvID.Value;
                    reportSolicDetail.CantidadSol = detail.CantidadSol.Value.Value.ToString();
                    reportSolicDetail.Proyecto = docCtrl.ProyectoID.Value;
                    reportSolicDetail.CentroCosto = docCtrl.CentroCostoID.Value;

                    reportSolic.Detail.Add(reportSolicDetail);
                }
                #endregion
                #endregion

                return reportSolic;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoLegalizacionGastosReport");
                return null;
            }
        }
        #endregion

        #region Compromisos VS Facturas

        /// <summary>
        /// Funcion que se encarga de  traer los compromisos contra las facturas
        /// </summary>
        /// <param name="FechaInicial">Fecha de consulta inicial</param>
        /// <param name="FechaFinal">Fecha de consulta final</param>
        /// <param name="proveedor">Filtra un proveedor en especifico</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ProveedoresTotal> ReportesProveedores_CompromisosVSFacturas(DateTime FechaInicial, DateTime FechaFinal, string proveedor)
        {
            List<DTO_ProveedoresTotal> result = new List<DTO_ProveedoresTotal>();
            DTO_ProveedoresTotal compro = new DTO_ProveedoresTotal();
            this._dal_ReportesProveedores = (DAL_ReportesProveedores)base.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            compro.DetalleCompromisosVSFacturas = this._dal_ReportesProveedores.DAL_ReportesProveedores_CompromisosVSFacturas(FechaInicial, FechaFinal, proveedor);
            List<string> distinct = (from c in compro.DetalleCompromisosVSFacturas select c.Proveedor.Value).Distinct().ToList();

            foreach (var prov in distinct)
            {
                DTO_ProveedoresTotal comproFac = new DTO_ProveedoresTotal();
                comproFac.DetalleCompromisosVSFacturas = new List<DTO_ReportCompromisoVSFacturas>();

                comproFac.DetalleCompromisosVSFacturas = compro.DetalleCompromisosVSFacturas.Where(x => x.Proveedor.Value == prov).ToList();
                result.Add(comproFac);
            }
            return result;
        }

        #endregion

        #region Orden Compras

        /// <summary>
        /// Funcion que se encarda de traer los datos de orden de compras
        /// </summary>
        /// <param name="FechaIni">Fecha incial que se desea ver los datos</param>
        /// <param name="FechaFin">Fecha Final hasta donde quiere verificar los datos</param>
        /// <param name="Proveedor">Filtra un proveedor en especifico</param>
        /// <param name="Estado">Filtra el estado de la orden de compra</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <param name="Moneda">Verifica la moneda que se desea mostrar</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ProveedoresTotal> ReportesProveedores_OrdenCompraArchivo(DateTime FechaIni, DateTime FechaFin, string Proveedor, int Estado, bool isDetallado, string Moneda)
        {
            try
            {
                List<DTO_ProveedoresTotal> result = new List<DTO_ProveedoresTotal>();
                DTO_ProveedoresTotal ordComp = new DTO_ProveedoresTotal();
                this._dal_ReportesProveedores = (DAL_ReportesProveedores)base.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                ordComp.DetallesOrdenCompra = this._dal_ReportesProveedores.DAL_ReportesProveedores_OrdenCompraArchivo(FechaIni, FechaFin, Proveedor, Estado, isDetallado, Moneda);

                if (!isDetallado)
                {
                    List<string> distinct = (from c in ordComp.DetallesOrdenCompra select c.PrefijoID.Value).Distinct().ToList();

                    foreach (var pref in distinct)
                    {
                        DTO_ProveedoresTotal compr = new DTO_ProveedoresTotal();
                        compr.DetallesOrdenCompra = new List<DTO_ReportOrdenCompra>();

                        compr.DetallesOrdenCompra = ordComp.DetallesOrdenCompra.Where(x => x.PrefijoID.Value == pref).ToList();
                        result.Add(compr);
                    }
                }

                else
                {
                    List<string> distinct = (from c in ordComp.DetallesOrdenCompra select c.OrdenCompra.Value).Distinct().ToList();

                    foreach (var orden in distinct)
                    {
                        DTO_ProveedoresTotal compr = new DTO_ProveedoresTotal();
                        compr.DetallesOrdenCompra = new List<DTO_ReportOrdenCompra>();

                        compr.DetallesOrdenCompra = ordComp.DetallesOrdenCompra.Where(x => x.OrdenCompra.Value == orden).ToList();
                        result.Add(compr);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesProveedores_Solicitudes");
                throw ex;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer y armar el listado de las facturas por pagar
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a Pagar</param>
        /// <returns>Listado DTO con las facturas a pagar</returns>
        public List<DTO_ProveedoresTotal> ReportesProveedores_OrdenCompra(int numDoc)
        {
            try
            {
                this._dal_ReportesProveedores = (DAL_ReportesProveedores)this.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_ProveedoresTotal> result = new List<DTO_ProveedoresTotal>();
                List<DTO_ReportOrdenCompraDoc> detOC = new List<DTO_ReportOrdenCompraDoc>();
                DTO_ProveedoresTotal res = new DTO_ProveedoresTotal();

                //Trae el detalle
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                List<DTO_ReportOrdenCompraDoc> query = this._dal_ReportesProveedores.DAL_ReportesOrdenCompra(numDoc, monedaLocal).ToList();
                //Asigna ID para agrupar
                foreach (DTO_ReportOrdenCompraDoc oc in query)
                {
                    if (string.IsNullOrEmpty(oc.Referencia.Value))
                        oc.Referencia.Value = oc.BienServicio.Value;
                    if (oc.Estado.Value == "APROBADO")
                        oc.Aprobo.Value = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId).Descriptivo.Value;
                    if (oc.CantidadEmpaque.Value != null && oc.CantidadEmpaque.Value != 0 && oc.CantidadEmpaque.Value != 1)
                    {
                        oc.Unidad.Value = oc.EmpaqueInvID.Value;
                        oc.Cantidad.Value = Math.Ceiling(oc.Cantidad.Value.Value / oc.CantidadEmpaque.Value.Value);
                        //oc.ValorUnitario.Value = oc.ValorUnitario.Value * oc.CantidadEmpaque.Value;
                    }
                }
                List<string> distinct = (from c in query select c.Referencia.Value).Distinct().ToList();
                int count = 1;
                foreach (var item in distinct)
                {                  
                    DTO_ReportOrdenCompraDoc r = new DTO_ReportOrdenCompraDoc();
                    r = query.Find(x => x.Referencia.Value == item);
                    r.Cantidad.Value = query.FindAll(x => x.Referencia.Value == item).Sum(y => y.Cantidad.Value);
                    r.ValorTotal.Value = query.FindAll(x => x.Referencia.Value == item).Sum(y => y.ValorTotal.Value);
                    r.ValorIVA.Value = query.FindAll(x => x.Referencia.Value == item).Sum(y => y.ValorIVA.Value);
                    r.Item.Value = (count++).ToString();
                    detOC.Add(r);
                }
                res.DetallesOrdenCompraDoc = detOC;
                result.Add(res);

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_OrdenCompra");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que se encarga de traer y armar el listado de las facturas por pagar
        /// </summary>
        /// <param name="numDoc">Identificador de las facturas a Pagar</param>
        /// <returns>Listado DTO con las facturas a pagar</returns>
        public List<DTO_ProveedoresTotal> ReportesProveedores_OrdenCompraAnexo(int numDoc)
        {
            try
            {
                this._dal_ReportesProveedores = (DAL_ReportesProveedores)this.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProyectos = (ModuloProyectos)this.GetInstance(typeof(ModuloProyectos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                List<DTO_ProveedoresTotal> results = new List<DTO_ProveedoresTotal>();
                DTO_ProveedoresTotal res = new DTO_ProveedoresTotal();
                Dictionary<int, List<DTO_pyProyectoMvto>> cacheDetas = new Dictionary<int, List<DTO_pyProyectoMvto>>();
                DTO_SolicitudTrabajo proyecto;

                //Trae items Orden de Compra
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                res.DetallesOrdenCompraDoc = this._dal_ReportesProveedores.DAL_ReportesOrdenCompraAnexo(numDoc, monedaLocal);
                foreach (var r in  res.DetallesOrdenCompraDoc)
                {
                    List<DTO_pyProyectoMvto> listMvto = new List<DTO_pyProyectoMvto>();

                    //Trae los movimientos del Proyecto(Modulo Proyectos)
                    if (cacheDetas.ContainsKey(r.NumDocProyMvto.Value.Value))
                        listMvto = cacheDetas[r.NumDocProyMvto.Value.Value];
                    else
                    {
                        proyecto = this._moduloProyectos.SolicitudProyecto_Load(AppDocuments.Proyecto, string.Empty, null, r.NumDocProyMvto.Value, string.Empty,string.Empty, false,true,false,false);
                        if (proyecto != null)
                        {
                            listMvto.AddRange(proyecto.Movimientos);
                            cacheDetas.Add(r.NumDocProyMvto.Value.Value, listMvto);
                        }                      
                    }
                    if (r.CantidadEmpaque.Value != null && r.CantidadEmpaque.Value != 0 && r.CantidadEmpaque.Value != 1)
                    {
                        r.Unidad.Value = r.EmpaqueInvID.Value;
                        r.Cantidad.Value = Math.Ceiling(r.Cantidad.Value.Value / r.CantidadEmpaque.Value.Value);
                        //oc.ValorUnitario.Value = oc.ValorUnitario.Value * oc.CantidadEmpaque.Value;
                    }
                    if (!string.IsNullOrEmpty(r.Referencia.Value))
                    {
                        //Referencias
                        var items = listMvto.FindAll(x => x.RecursoID.Value == r.Referencia.Value && x.CantidadPROV.Value > 0).ToList();
                        items.ForEach(x => r.Item.Value += (x.TareaCliente.Value + "-"));
                    }
                    else
                    {
                        //Codigos BS
                        string rec = listMvto.Find(x => x.Consecutivo.Value == r.ConsProyMvto.Value).RecursoID.Value;
                        var items = listMvto.FindAll(x => x.RecursoID.Value == rec && x.CantidadPROV.Value > 0).ToList();
                        items.ForEach(x => r.Item.Value += (x.TareaCliente.Value + "-"));
                    }
                    if(r.Item.Value.Length > 0)
                        r.Item.Value = r.Item.Value.Remove(r.Item.Value.Length - 1);
                }

                results.Add(res);
                return results;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ReportesProveedores_OrdenCompraAnexo");
                throw exception;
            }
        }

        #endregion

        #region Recibidos

        /// <summary>
        /// Funcion que se encarga de traer las ordenes recibidas
        /// </summary>
        /// <param name="Periodo">Periodo que se desea consultar</param>
        /// <param name="proveedor">Filtrar un Proveedor especifico</param>
        /// <param name="isDetallado">Verifica que tipo de reporte mostrar (True: Muestra Orden Compra Detallada, False: Muestra Orden Compra Resumida)</param>
        /// <returns></returns>
        public List<DTO_ProveedoresTotal> ReportesProveedores_Recibidos(DateTime Periodo, string proveedor, bool isDetallado, int? numDoc, bool isFacturdo)
        {
            try
            {
                Dictionary<string, string> detalles = new Dictionary<string, string>();
                List<DTO_ProveedoresTotal> result = new List<DTO_ProveedoresTotal>();
                DTO_ProveedoresTotal recibi = new DTO_ProveedoresTotal();
                this._dal_ReportesProveedores = (DAL_ReportesProveedores)this.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                recibi.DetallesRecibidos = this._dal_ReportesProveedores.DAL_ReportesProveedores_Recibidos(Periodo, proveedor, isDetallado, numDoc, isFacturdo);

                //Carga la informacion para el reporte resumido
                if (!isDetallado)
                    result.Add(recibi);

                //Carga la informacion para el reporte Detallado
                else
                {
                    foreach (DTO_ReportProveedoresRecibidos recibidos in recibi.DetallesRecibidos)
                    {
                        if (!detalles.Contains(new KeyValuePair<string, string>(recibidos.Recibido.Value + "/" + recibidos.Proveedor.Value, recibidos.Proveedor.Value)))
                            detalles.Add(recibidos.Recibido.Value + "/" + recibidos.Proveedor.Value, recibidos.Proveedor.Value);
                    }

                    foreach (var ProveedorRecib in detalles)
                    {

                        DTO_ProveedoresTotal recibProv = new DTO_ProveedoresTotal();
                        recibProv.DetallesRecibidos = new List<DTO_ReportProveedoresRecibidos>();

                        string recibido;
                        string proveedorID;

                        recibido = ProveedorRecib.Key.Split('/')[0].ToString();
                        proveedorID = ProveedorRecib.Key.Split('/')[1].ToString();
                        recibProv.DetallesRecibidos = recibi.DetallesRecibidos.Where(x => x.Recibido.Value == recibido && x.Proveedor.Value == proveedorID).ToList();

                        result.Add(recibProv);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesProveedores_Recibidos");
                throw ex;
            }
        }

        #endregion

        #region Solicitudes / Recibidos (Documentos)
        public List<DTO_ProveedoresTotal> ReportesProveedores_Solicitudes(Dictionary<int, string> filtros, int? numDoc = null)
        {
            try
            {
                List<DTO_ProveedoresTotal> soli = new List<DTO_ProveedoresTotal>();
                DTO_ProveedoresTotal solicitudes = new DTO_ProveedoresTotal();
                this._dal_ReportesProveedores = (DAL_ReportesProveedores)base.GetInstance(typeof(DAL_ReportesProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                solicitudes.DetallesSolicitudes = _dal_ReportesProveedores.DAL_ReportesProveedores_Solicitudes(filtros,numDoc);
                List<string> distinct = (from c in solicitudes.DetallesSolicitudes select c.PrefDoc.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ProveedoresTotal solicitud = new DTO_ProveedoresTotal();
                    solicitud.DetallesSolicitudes = new List<DTO_ReportProveedoresSolicitudes>();

                    solicitud.DetallesSolicitudes = solicitudes.DetallesSolicitudes.Where(x => x.PrefDoc.Value == item).ToList();
                    soli.Add(solicitud);
                }
                return soli;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ReportesProveedores_Solicitudes");
                throw ex;
            }
        }
        #endregion

        #endregion
    }
}
