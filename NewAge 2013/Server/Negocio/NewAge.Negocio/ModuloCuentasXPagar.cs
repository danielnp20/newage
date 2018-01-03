using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using System.Data.SqlClient;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Reportes;
using SentenceTransformer;
using System.Data;

namespace NewAge.Negocio
{
    public class ModuloCuentasXPagar : ModuloBase
    {
        #region Variables

        #region Dals

        private DAL_cpAnticipos _dal_cpAnticipos = null;
        private DAL_CuentasXPagar _dal_CuentasXPagar = null;
        private DAL_Legalizacion _dal_Legalizacion = null;
        private DAL_MasterComplex _dal_MasterComplex = null;
        private DAL_cpTarjetaDocu _dal_TarjetaDocu = null;
        private DAL_cpTarjetaPagos _dal_TarjetaPagos = null;
        private DAL_ReportesCuentasXPagar _dal_ReportCuentasXPagar = null;
        private DAL_tsBancosDocu _dal_tsBancosDocu = null;
        private DAL_tsBancosCuenta _dal_tsBancosCta = null;
        private DAL_ccSolicitudCompraCartera _dal_ccSolicitudCompraCar = null;

        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloContabilidad _moduloContabilidad = null;
        private ModuloGlobal _moduloGlobal = null;
        private ModuloProveedores _moduloProveedores = null;
        private ModuloPlaneacion _moduloPlaneacion = null;
        private ModuloNomina _moduloNomina = null;
        #endregion

        #endregion

        /// <summary>
        /// Constructor Fachada Cuentas X Pagar
        /// </summary>
        /// <param name="conn"></param>
        public ModuloCuentasXPagar(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        #region Anticipos

        #region Funciones Privadas

        /// <summary>
        /// Aprueba anticipo
        /// </summary>
        /// <param name="anticipo">Anticipo de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult cpAnticipos_Aprobar(int documentID, string actividadFlujoID, ModulesPrefix currentMod, DTO_AnticipoAprobacion anticipo, bool createDoc, bool insideAnotherTx, bool anticipoTarjetaCred = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl ctrlCxP = new DTO_glDocumentoControl();
            DTO_coComprobante coComp = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #region Carga las variables

                DTO_glDocumentoControl ctrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(anticipo.NumeroDoc.Value.Value);
                DTO_cpAnticipo _anticipo = this.cpAnticipos_GetByEstado(ctrlAnticipo.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion);
                DTO_cpAnticipoTipo _tipoAnticipo = (DTO_cpAnticipoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, _anticipo.AnticipoTipoID.Value, true, false);
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();

                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjera = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string conceptoCxP = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCxPAnticipos);

                string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lineaPresupuestal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                decimal vlrMdaLOC = 0;
                decimal vlrMdaEXT = 0;

                #endregion
                #region Footer para el anticipo

                if (monedaLocal == ctrlAnticipo.MonedaID.Value)
                {
                    vlrMdaLOC = _anticipo.Valor.Value.Value;
                    vlrMdaEXT = ctrlAnticipo.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_anticipo.Valor.Value.Value / ctrlAnticipo.TasaCambioDOCU.Value.Value), 2);
                }
                else
                {
                    vlrMdaLOC = ctrlAnticipo.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_anticipo.Valor.Value.Value * ctrlAnticipo.TasaCambioDOCU.Value.Value), 2); ;
                    vlrMdaEXT = _anticipo.Valor.Value.Value;
                }

                DTO_ComprobanteFooter _anticipoComprobante = this.CrearComprobanteFooter(ctrlAnticipo, ctrlAnticipo.TasaCambioDOCU.Value.Value, conceptoCargo,
                    lugarGeografico, lineaPresupuestal, vlrMdaLOC, vlrMdaEXT, false);

                footer.Add(_anticipoComprobante);
                #endregion
                #region Crea y contabiliza la CxP

                object obj = this.CuentasXPagar_Generar(ctrlAnticipo, conceptoCxP, anticipo.Valor.Value.Value, footer, ModulesPrefix.cp, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    return result;
                }

                //Trae la CxP para actualizar los consecutivos
                ctrlCxP = (DTO_glDocumentoControl)obj;
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);

                #endregion
                #region Aprueba el documento del anticipo
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, ctrlAnticipo.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, anticipo.Observacion.Value, true);
                #endregion
                #region Asigna el flujo para el anticipo
                result = this.AsignarFlujo(documentID, anticipo.NumeroDoc.Value.Value, actividadFlujoID, false, anticipo.Observacion.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion
                #region Genera el reporte del anticipo
                if (createDoc)
                {
                    try
                    {
                        //if (!_tipoAnticipo.GastosViajeInd.Value.Value)
                        //    this.ReportesCuentasXPagar_DocumentoAnticipo(ctrlAnticipo.NumeroDoc.Value.Value);
                        //else 
                        //    this.ReportesCuentasXPagar_DocumentoAnticipoViaje(ctrlAnticipo.NumeroDoc.Value.Value);
                        //object report = this.DtoAnticipoReport(_anticipo, ctrlAnticipo, _tipoAnticipo, true);
                        //this.GenerarArchivo(documentID, ctrlAnticipo.NumeroDoc.Value.Value, report);
                    }
                    catch (Exception)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                        return result;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpAnticipos_Aprobar");
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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                    else
                        if (!anticipoTarjetaCred)
                            throw new Exception("cpAnticipos_Aprobar - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza anticipo
        /// </summary>
        /// <param name="anticipo">Anticipo de la lista</param>
        /// <param name="rd"></param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private DTO_TxResult cpAnticipos_Rechazar(int documentID, string actividadFlujoID, DTO_AnticipoAprobacion anticipo, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, anticipo.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, anticipo.Observacion.Value, true);
                this.AsignarAlarma(anticipo.NumeroDoc.Value.Value, actividadFlujoID, false);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                var exception = new Exception(DictionaryMessages.Cp_AnticipoNoExiste, ex);
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "cpAnticipos_Rechazar");

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
        /// Crea un dto de reporte para anticipos
        /// </summary>
        /// <param name="aprobacionInd">Indica si el reporte esta creando en el processo de aprobacion (durante aprobacion - true) </param>
        /// <returns></returns>
        private object DtoAnticipoReport(DTO_cpAnticipo anticipo, DTO_glDocumentoControl dtoControl, DTO_cpAnticipoTipo tipoAnticipo, bool aprobacionInd = false)
        {
            try
            {
                DTO_ReportAnticipo dtoAnticipoViaje = new DTO_ReportAnticipo();
                DTO_coTercero terceroInfo = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, dtoControl.TerceroID.Value, true, false);
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), dtoControl.Estado.Value.Value.ToString());

                if (tipoAnticipo.GastosViajeInd.Value.HasValue && (bool)tipoAnticipo.GastosViajeInd.Value)
                {
                    #region Anticipos de Viaje Report
                    DTO_ReportAnticipoViaje dtoAnticipoViajeReport = new DTO_ReportAnticipoViaje();

                    dtoAnticipoViajeReport.No = ""; ////////////
                    dtoAnticipoViajeReport.Fecha = Convert.ToDateTime(dtoControl.FechaDoc.Value);
                    dtoAnticipoViajeReport.EmpresaID = anticipo.EmpresaID.Value.ToString();
                    dtoAnticipoViajeReport.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar) ? false : true;
                    if (aprobacionInd) dtoAnticipoViajeReport.EstadoInd = false;
                    dtoAnticipoViajeReport.Area = ""; /////////////
                    dtoAnticipoViajeReport.DocumentoIdent = dtoControl.TerceroID.Value.ToString();
                    dtoAnticipoViajeReport.Nombres = ((NewAge.DTO.Negocio.DTO_MasterBasic)(terceroInfo)).Descriptivo.ToString();
                    dtoAnticipoViajeReport.MotivoViaje = ""; /////////////
                    dtoAnticipoViajeReport.Destino = ""; /////////////
                    dtoAnticipoViajeReport.DiasAlojamiento = (int)anticipo.DiasAlojamiento.Value;
                    dtoAnticipoViajeReport.ValorAlojamiento = (decimal)anticipo.ValorAlojamiento.Value;
                    dtoAnticipoViajeReport.TotalAlojamiento = dtoAnticipoViajeReport.DiasAlojamiento * dtoAnticipoViajeReport.ValorAlojamiento;
                    dtoAnticipoViajeReport.DiasAlimentacion = (int)anticipo.DiasAlimentacion.Value;
                    dtoAnticipoViajeReport.ValorAlimentacion = (decimal)anticipo.ValorAlimentacion.Value;
                    dtoAnticipoViajeReport.TotalAlimentacion = dtoAnticipoViajeReport.DiasAlimentacion * dtoAnticipoViajeReport.ValorAlimentacion;
                    dtoAnticipoViajeReport.DiasTransporte = (int)anticipo.DiasTransporte.Value;
                    dtoAnticipoViajeReport.ValorTransporte = (decimal)anticipo.ValorTransporte.Value;
                    dtoAnticipoViajeReport.TotalTransporte = dtoAnticipoViajeReport.DiasTransporte * dtoAnticipoViajeReport.ValorTransporte;
                    dtoAnticipoViajeReport.DiasOtrosGastos = (int)anticipo.DiasOtrosGastos.Value;
                    dtoAnticipoViajeReport.ValorOtrosGastos = (decimal)anticipo.ValorOtrosGastos.Value;
                    dtoAnticipoViajeReport.TotalOtrosGastos = dtoAnticipoViajeReport.DiasOtrosGastos * dtoAnticipoViajeReport.ValorOtrosGastos;
                    dtoAnticipoViajeReport.TotalAnticipo = dtoAnticipoViajeReport.TotalAlojamiento + dtoAnticipoViajeReport.TotalAlimentacion + dtoAnticipoViajeReport.TotalTransporte + dtoAnticipoViajeReport.TotalOtrosGastos;
                    dtoAnticipoViajeReport.Funcionario = "";
                    dtoAnticipoViajeReport.Autorizado = "";
                    return dtoAnticipoViajeReport;
                    #endregion
                }
                return dtoAnticipoViaje;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoAnticipoReport");
                return null;
            }
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Obtiene un objeto DTO_cpAnticipo por numero de documento
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        public DTO_cpAnticipo cpAnticipos_GetByEstado(int numeroDoc, EstadoDocControl estado)
        {
            this._dal_cpAnticipos = (DAL_cpAnticipos)base.GetInstance(typeof(NewAge.ADO.DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_cpAnticipos.DAL_cpAnticipos_GetByEstado(numeroDoc, estado);
        }

        /// <summary>
        /// Guarda o actualiza documento de anticipo
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_anticipo">anticipo</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject cpAnticipos_Guardar(int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpAnticipo _anticipo, bool _update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                if (!_update)
                {
                    #region Guarda en glDocumentoControl

                    string defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                    string defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                    string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                    if (_dtoCtrl.ProyectoID == null)
                        _dtoCtrl.ProyectoID.Value = defProyecto;

                    if (_dtoCtrl.CentroCostoID == null)
                        _dtoCtrl.CentroCostoID.Value = defCentroCosto;

                    if (_dtoCtrl.LineaPresupuestoID == null)
                        _dtoCtrl.LineaPresupuestoID.Value = defLineaPresupuesto;

                    if (_dtoCtrl.LugarGeograficoID == null)
                        _dtoCtrl.LugarGeograficoID.Value = defLugarGeografico;

                    _dtoCtrl.DocumentoNro.Value = 0;
                    rd = this._moduloGlobal.glDocumentoControl_Add(documentID, _dtoCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rd);

                        return result;
                    }
                    else
                        _dtoCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region  Guarda en Anticipo
                    _anticipo.NumeroDoc.Value = _dtoCtrl.NumeroDoc.Value.Value;
                    this._dal_cpAnticipos = (DAL_cpAnticipos)this.GetInstance(typeof(DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    #region Validar cpConceptoCxPID

                    DTO_cpConceptoCXP concCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, _anticipo.ConceptoCxPID.Value, true, false);
                    if (concCxP == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ConceptoCxPID";
                        rdF.Message = DictionaryMessages.FkNotFound + "&&" + _anticipo.ConceptoCxPID.Value;

                        rd = new DTO_TxResultDetail();
                        rd.line = 1;
                        rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                        rd.DetailsFields.Add(rdF);

                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                        return result;
                    }

                    #endregion

                    this._dal_cpAnticipos.DAL_cpAnticipos_Add(_anticipo);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Consecutivo Anticipo
                    string EmpNro = this.Empresa.NumeroControl.Value;
                    string keyControl = EmpNro + "02" + AppControl.cp_ConsecutivoAnticipo;
                    DTO_glControl glControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                    if (glControl != null)
                    {
                        int newConsec = glControl.Data != null ? Convert.ToInt32(glControl.Data.Value) + 1 : 1;
                        // Actualiza el consecutivo de anticipos en glControl
                        glControl.Data.Value = newConsec.ToString();
                        this._moduloGlobal.glControl_Update(glControl);
                    }
                    #endregion
                    #region Asigna Alarma

                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(_dtoCtrl.NumeroDoc.Value.Value, true);
                    alarma.NumeroDoc = _dtoCtrl.NumeroDoc.Value.ToString();

                    #region Genera el reporte del anticipo
                    //if (result.Result == ResultValue.OK)
                    //{
                    //    try
                    //    {
                    //        DTO_cpAnticipoTipo antTipo = (DTO_cpAnticipoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, _anticipo.AnticipoTipoID.Value, true, false);
                    //        object report = this.DtoAnticipoReport(_anticipo, _dtoCtrl, antTipo, false);
                    //        this.GenerarArchivo(documentID, _dtoCtrl.NumeroDoc.Value.Value, report);
                    //    }
                    //    catch (Exception)
                    //    {
                    //        result.Result = ResultValue.NOK;
                    //        result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                    //        return result;
                    //    }
                    //}
                    #endregion
                    return alarma;

                    #endregion
                }
                else
                {
                    #region Revisa que no haya sido aprobado, anulado o revertido
                    DTO_glDocumentoControl ctrlTemp = this._moduloGlobal.glDocumentoControl_GetByID(_dtoCtrl.NumeroDoc.Value.Value);
                    EstadoDocControl est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), ctrlTemp.Estado.Value.Value.ToString());
                    if (est == EstadoDocControl.Anulado || est == EstadoDocControl.Aprobado || est == EstadoDocControl.Cerrado || est == EstadoDocControl.Devuelto)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_DocProcessed;
                        return result;
                    }
                    #endregion
                    #region Actualiza en glDocumentoControl

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloGlobal.glDocumentoControl_Update(_dtoCtrl, true, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Actualiza en cpAnticipo
                    this._dal_cpAnticipos = (DAL_cpAnticipos)this.GetInstance(typeof(DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    _dal_cpAnticipos.DAL_cpAnticipos_Upd(_anticipo);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Asigna Alarma

                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(_dtoCtrl.NumeroDoc.Value.Value, true);
                    alarma.NumeroDoc = _dtoCtrl.NumeroDoc.Value.ToString();
                    return alarma;

                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpAnticipos_Guardar");
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
        /// Retorna el valor total para una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo los anticipos</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de los anticipos</returns>
        public decimal cpAnticipos_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            this._dal_cpAnticipos = (DAL_cpAnticipos)base.GetInstance(typeof(NewAge.ADO.DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_cpAnticipos.DAL_cpAnticipos_GetResumenVal(periodo, tm, tc, terceroID);
        }

        /// <summary>
        /// Retorna una lista de anticipos 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer los anticipos</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <param name="anticipoTarjeta">Indica si es anticipo de tarjeta de credito</param>
        /// <returns>Retorna una lista de anticipos</returns>
        public List<DTO_AnticiposResumen> cpAnticipos_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID, bool anticipoTipo)
        {
            this._dal_cpAnticipos = (DAL_cpAnticipos)base.GetInstance(typeof(NewAge.ADO.DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_cpAnticipos.DAL_cpAnticipos_GetResumen(periodo, tipoMoneda, terceroID, anticipoTipo);
        }

        /// <summary>
        /// Trae un listado de anticipos pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpAnticipos_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_cpAnticipos = (DAL_cpAnticipos)base.GetInstance(typeof(NewAge.ADO.DAL_cpAnticipos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
            string usuarioID = seUsuario.ID.Value;
            List<DTO_AnticipoAprobacion> list = this._dal_cpAnticipos.DAL_cpAnticipos_GetPendientesByModulo(mod, actFlujoID, usuarioID);
            foreach (DTO_AnticipoAprobacion item in list)
                item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
            return list;
        }

        /// <summary>
        /// Recibe una lista de anticipos para aprobar o rechazar
        /// </summary>
        /// <param name="anticipos">Anticipos que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> cpAnticipos_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> anticipos, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                decimal porcTotal = 0;
                int i = 0;
                foreach (var item in anticipos)
                {
                    porcTotal = anticipos.Count * i / 100;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    #region Aprobar o Rechazar Anticipos
                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.cpAnticipos_Aprobar(documentID, actividadFlujoID, ModulesPrefix.cp, item, createDoc, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "cpAnticipos_Aprobar");
                            rd.Message = DictionaryMessages.Err_Cp_AnticipoAprobar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.DocumentoTercero.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            string tipoAnticipoCartera = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_TipoAnticipoCompraCartera);
                            if (item.AnticipoTipoID.Value.Equals(tipoAnticipoCartera))
                            {
                                this._dal_ccSolicitudCompraCar = (DAL_ccSolicitudCompraCartera)this.GetInstance(typeof(DAL_ccSolicitudCompraCartera), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                DTO_ccSolicitudCompraCartera dto = this._dal_ccSolicitudCompraCar.DAL_ccSolicitudCompraCartera_GetByDocAnticipo(item.NumeroDoc.Value.Value);
                                dto.DocAnticipo.Value = null;
                                this._dal_ccSolicitudCompraCar.DAL_ccSolicitudCompraCartera_Update(dto);
                            }
                            result = this.cpAnticipos_Rechazar(documentID, actividadFlujoID, item, insideAnotherTx);

                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "cpAnticipos_Rechazar");
                            rd.Message = DictionaryMessages.Err_Cp_AnticipoRechazar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.DocumentoTercero.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }

                    #endregion

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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpAnticipos_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        #endregion

        #endregion

        #region Cuentas X Pagar

        #region Funciones Privadas

        /// <summary>
        /// Actualizar ñla Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        internal DTO_TxResult CuentasXPagar_Upd(DTO_cpCuentaXPagar cta)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            rd.line = 1;
            rd.Message = "OK";

            try
            {
                this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CuentasXPagar.DAL_CuentasXPagar_Upd(cta);
                return result;

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CajaMenor_Aprobar");
                return result;
            }
        }

        /// <summary>
        /// Crea una CXP dado un documento
        /// </summary>
        /// <param name="ctrlOld">Documento Generador</param>
        /// <param name="conceptoCxP">concepto cuenta por pagar</param>
        /// <param name="valor">valor del documetno</param>
        /// <param name="footer">Lista de detalles del footer</param>
        /// <param name="numeroDocCXP">numero documento CXP</param>
        /// <param name="mod">Modulo de operacion</param>
        /// <param name="isPre">Indica si se debe generar en el auxiliar preliminar</param>
        /// <returns>objeto resultado</returns>
        internal object CuentasXPagar_Generar(DTO_glDocumentoControl ctrlOld, string conceptoCxP, decimal valor, List<DTO_ComprobanteFooter> footer, ModulesPrefix mod, 
            bool isPre, string docTerceroFinal = "")
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                #region Variables

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string lineaPresupuestal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);

                string pagoAprobacionStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_IndicadorPagosAprobacion);
                bool pagoAprobacionInd = pagoAprobacionStr != "0";

                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                bool isML = ctrlOld.MonedaID.Value == mdaLoc ? true : false;

                DateTime periodo = Convert.ToDateTime(ctrlOld.PeriodoDoc.Value);
                DateTime fechaDoc;
                if (ctrlOld.FechaDoc.Value.HasValue)
                    fechaDoc = ctrlOld.FechaDoc.Value.Value;
                else
                    fechaDoc = DateTime.Now;

                if (fechaDoc.Month != periodo.Month || fechaDoc.Year != periodo.Year)
                    fechaDoc = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                valor = Math.Abs(valor);

                int documentoID = AppDocuments.CausarFacturas;
                DTO_glDocumentoControl ctrlCxp = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();
                DTO_coDocumento coDoc = new DTO_coDocumento();
                DTO_coComprobante coComp = new DTO_coComprobante();

                #endregion
                #region Validaciones

                //Valida el concepto CxP
                if (string.IsNullOrWhiteSpace(conceptoCxP))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }

                DTO_cpConceptoCXP concCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxP, true, false);
                if (concCxP == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }

                coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, concCxP.coDocumentoID.Value, true, false);

                //Revisa que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }

                //Valida que el documento asociado tenga cuenta
                if (
                    (isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) ||
                    (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value))
                    )
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                    return result;
                }

                //Valida que la cuenta sea de un documento externo
                string ctaID = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctaID, true, false);
                DTO_glConceptoSaldo csaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                if (csaldo.coSaldoControl.Value.Value != (Int16)SaldoControl.Doc_Externo)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_InvalidCtaConcCxP + "&&" + ctaID.Trim() + "&&" + conceptoCxP.Trim();
                    return result;
                }

                //Revisa que el modulo se encuentre abierto
                EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, mod);
                if (estado != EstadoPeriodo.Abierto)
                {
                    if (estado == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (estado == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }

                //Revisa que el modulo de Cuenta X pagar se encuentre abierto 
                EstadoPeriodo estadoCxP = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.cp);
                if (estadoCxP != EstadoPeriodo.Abierto)
                {
                    if (estadoCxP == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (estadoCxP == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }

                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                #endregion
                #region Crea el documento CxP

                ctrlCxp.DocumentoID.Value = documentoID;
                ctrlCxp.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                ctrlCxp.DocumentoNro.Value = 0;
                ctrlCxp.FechaDoc.Value = fechaDoc;
                ctrlCxp.PeriodoDoc.Value = periodo;
                ctrlCxp.PeriodoUltMov.Value = periodo;
                ctrlCxp.ComprobanteID.Value = coComp.ID.Value;
                ctrlCxp.ComprobanteIDNro.Value = 0;
                ctrlCxp.CuentaID.Value = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                ctrlCxp.AreaFuncionalID.Value = ctrlOld.AreaFuncionalID.Value;
                ctrlCxp.PrefijoID.Value = ctrlOld.PrefijoID.Value;
                ctrlCxp.Observacion.Value = ctrlOld.Observacion.Value;
                ctrlCxp.Descripcion.Value = "(CXP) " + ctrlOld.Descripcion.Value;
                ctrlCxp.Observacion.Value = "(CXP) " + ctrlOld.Descripcion.Value;
                ctrlCxp.ConsSaldo.Value = ctrlOld.NumeroDoc.Value;
                ctrlCxp.MonedaID.Value = ctrlOld.MonedaID.Value;
                ctrlCxp.TasaCambioCONT.Value = ctrlOld.TasaCambioCONT.Value;
                ctrlCxp.TasaCambioDOCU.Value = ctrlOld.TasaCambioDOCU.Value;
                ctrlCxp.TerceroID.Value = ctrlOld.TerceroID.Value;
                ctrlCxp.DocumentoTercero.Value = string.IsNullOrEmpty(docTerceroFinal) ? this.GetFactura(ctrlOld.FechaDoc.Value.Value, ctrlOld.NumeroDoc.Value.Value) : docTerceroFinal;
                ctrlCxp.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrlCxp.seUsuarioID.Value = this.UserId;
                ctrlCxp.LineaPresupuestoID.Value = lineaPresupuestal;
                ctrlCxp.LugarGeograficoID.Value = lugarGeografico;
                ctrlCxp.ProyectoID.Value = ctrlOld.ProyectoID.Value;
                ctrlCxp.CentroCostoID.Value = ctrlOld.CentroCostoID.Value;
                ctrlCxp.Valor.Value = valor;

                ctrlCxp.DocumentoPadre.Value = ctrlOld.NumeroDoc.Value != 0 ? ctrlOld.NumeroDoc.Value : null;
                #endregion
                #region Crea la CxP

                DTO_cpCuentaXPagar _ctaXPagar = new DTO_cpCuentaXPagar();
                _ctaXPagar.ConceptoCxPID.Value = conceptoCxP;
                _ctaXPagar.Valor.Value = valor;
                _ctaXPagar.IVA.Value = 0;
                _ctaXPagar.MonedaPago.Value = ctrlOld.MonedaID.Value;
                _ctaXPagar.FacturaFecha.Value = ctrlOld.FechaDoc.Value;
                _ctaXPagar.ContabFecha.Value = fechaDoc;
                _ctaXPagar.VtoFecha.Value = ctrlOld.FechaDoc.Value.Value.AddDays(2);
                _ctaXPagar.DistribuyeImpLocalInd.Value = false;
                _ctaXPagar.TerceroID.Value = ctrlOld.TerceroID.Value;
                _ctaXPagar.RadicaCodigo.Value = DateTime.Now.ToShortDateString();
                _ctaXPagar.NumeroDocPadre.Value = ctrlOld.NumeroDoc.Value != 0 ? ctrlOld.NumeroDoc.Value : null;
                _ctaXPagar.PagoAprobacionInd.Value = pagoAprobacionInd;

                #endregion
                #region  Radica la CuentaXPagar
                int _cxpNumDoc = 0;

                object obj = this.CuentasXPagar_Radicar(documentoID, ctrlCxp, _ctaXPagar, false, false, out _cxpNumDoc, new Dictionary<Tuple<int, int>, int>(), true);
                if (obj.GetType() == typeof(DTO_TxResult))
                    return (DTO_TxResult)obj;

                ctrlCxp.NumeroDoc.Value = _cxpNumDoc;

                //Quita las alarmas para enviar a aprobacion
                this.AsignarAlarma(_cxpNumDoc, string.Empty, false);

                #endregion
                #region Carga el cabezote del comprobante

                DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                compHeader.ComprobanteID.Value = coComp.ID.Value;
                compHeader.ComprobanteNro.Value = 0;
                compHeader.EmpresaID.Value = this.Empresa.ID.Value;
                compHeader.Fecha.Value = fechaDoc;
                compHeader.NumeroDoc.Value = _cxpNumDoc;
                compHeader.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                compHeader.MdaTransacc.Value = ctrlCxp.MonedaID.Value;
                compHeader.PeriodoID.Value = periodo;
                compHeader.TasaCambioBase.Value = 0;
                compHeader.TasaCambioOtr.Value = 0;

                comprobante.Header = compHeader;

                #endregion
                #region Carga el footer del comprobante

                decimal totalML = 0;
                decimal totalME = 0;

                // Asigna los valores de la moneda local y extranjera
                foreach (DTO_ComprobanteFooter item in footer)
                {
                    totalML += item.vlrMdaLoc.Value.Value;
                    totalME += item.vlrMdaExt.Value.Value;
                }

                DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(ctrlCxp, ctrlCxp.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestal, totalML * -1, totalME * -1, true);
                compFooterTemp.Descriptivo.Value = "ContraPartida CxP";
                footer.Add(compFooterTemp);

                comprobante.Footer = footer;

                #endregion
                #region Genera el comprobante
                if (isPre)
                    result = this._moduloContabilidad.ComprobantePre_Add(documentoID, ModulesPrefix.cp, comprobante, ctrlCxp.AreaFuncionalID.Value,
                        ctrlCxp.PrefijoID.Value, false, null, null, new Dictionary<Tuple<int, int>, int>(), true);
                else
                    result = this._moduloContabilidad.ContabilizarComprobante(documentoID, comprobante, periodo, ModulesPrefix.cp, 0, false);
                #endregion

                if (result.Result == ResultValue.NOK)
                    return result;
                else
                    return ctrlCxp;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Generar");
                return result;
            }
        }

        /// <summary>
        /// Aprueba una CxP
        /// </summary>
        /// <param name="numeroDoc">ID de glDocumentoControl</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="compID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        /// <param name="obs">Observacion al aprobar</param>
        /// <param name="updDocCtrl">Indica si se debe actualizar el documentoControl</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        private DTO_TxResult CuentasXPagar_Aprobar(int documentID, string actividadFlujoID, int numeroDoc, DateTime periodo, string compID, int compNro, string obs, bool updDocCtrl)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_CuentasXPagar = (DAL_CuentasXPagar)this.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_glDocumentoControl ctrlProvision = null;
            DTO_coComprobante coComp = null;
            try
            {
                DTO_glDocumentoControl ctrlCxP = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                if (ctrlCxP.PeriodoDoc.Value.Value.Month == ctrlCxP.FechaDoc.Value.Value.Month)
                {
                    result = this._moduloContabilidad.Comprobante_Aprobar(documentID, actividadFlujoID, ModulesPrefix.cp, numeroDoc, true, periodo, compID, compNro, obs,
                        updDocCtrl, false, true, true);
                }
                else
                {
                    #region Provisiones

                    #region Variables y validaciones

                    DateTime periodoCxP = ctrlCxP.PeriodoDoc.Value.Value;
                    string monedaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    string docProvision = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DocContableProvisionesFacturas);
                    DTO_coDocumento ctaProvision = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docProvision, true, false);

                    if (ctaProvision == null  || (ctrlCxP.MonedaID.Value == monedaLoc  && string.IsNullOrWhiteSpace(ctaProvision.CuentaLOC.Value)))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_DocContableProvisionesFacturas + "&&" + string.Empty;

                        return result;
                    }
                    else if(ctrlCxP.MonedaID.Value != monedaLoc  && string.IsNullOrWhiteSpace(ctaProvision.CuentaEXT.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_DocContableProvisionesFacturas + "&&" + string.Empty;

                        return result;
                    }

                    DTO_coPlanCuenta ctaProv = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctrlCxP.MonedaID.Value == monedaLoc ? ctaProvision.CuentaLOC.Value : ctaProvision.CuentaEXT.Value, true, false);
                    coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);

                    #endregion
                    #region Crea el documento de provisiones

                    //Se asigna el 
                    ctrlProvision = ObjectCopier.Clone(ctrlCxP);
                    ctrlProvision.PeriodoDoc.Value = periodoCxP.AddMonths(-1);
                    ctrlProvision.NumeroDoc.Value = 0;
                    ctrlProvision.DocumentoID.Value = AppDocuments.ProvisionesCxP;
                    ctrlProvision.CuentaID.Value = ctaProvision.CuentaLOC.Value;
                    ctrlProvision.ComprobanteIDNro.Value = 0;
                    ctrlProvision.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    ctrlProvision.NumeroDoc.Value = numeroDoc;
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
                    #region Actualiza el documento de CxP

                    ctrlCxP.FechaDoc.Value = new DateTime(periodoCxP.Year, periodoCxP.Month, 1);
                    this._moduloGlobal.glDocumentoControl_Update(ctrlCxP, true, true);
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, numeroDoc, EstadoDocControl.Aprobado, obs, true);

                    #endregion
                    #region Carga la info del comprobante actual y elimina el preliminar existente

                    DTO_Comprobante comprobanteOld = this._moduloContabilidad.Comprobante_Get(true, true, periodo, compID, compNro, null, null);
                    DTO_ComprobanteFooter contraCxP = comprobanteOld.Footer[comprobanteOld.Footer.Count - 1];

                    this._moduloContabilidad.BorrarAuxiliar_Pre(periodo, compID, compNro);

                    #endregion
                    #region Carga el comprobante de provisiones

                    DTO_Comprobante comprobanteProv = ObjectCopier.Clone(comprobanteOld);
                    //Correccion Header
                    comprobanteProv.Header.PeriodoID.Value = ctrlProvision.PeriodoDoc.Value;
                    comprobanteProv.Header.Fecha.Value = ctrlProvision.FechaDoc.Value;
                    comprobanteProv.Header.NumeroDoc.Value = ctrlProvision.NumeroDoc.Value;
                    comprobanteProv.Header.ComprobanteNro.Value = 0;
                    //Correccion Footer
                    comprobanteProv.Footer.RemoveAt(comprobanteProv.Footer.Count - 1);
                    DTO_ComprobanteFooter contraProv = this.CrearComprobanteFooter(ctrlProvision, contraCxP.TasaCambio.Value.Value, contraCxP.ConceptoCargoID.Value,
                        contraCxP.LugarGeograficoID.Value, contraCxP.LineaPresupuestoID.Value, contraCxP.vlrMdaLoc.Value.Value, contraCxP.vlrMdaExt.Value.Value, true);
                    comprobanteProv.Footer.Add(contraProv);

                    //Contabiliza
                    result = this._moduloContabilidad.ContabilizarComprobante(AppDocuments.Provisiones, comprobanteProv, ctrlProvision.PeriodoDoc.Value.Value,
                        ModulesPrefix.cp, 0, false);

                    if (result.Result == ResultValue.NOK)
                        return result;

                    #endregion
                    #region Carga el comprobante de CxP

                    DTO_Comprobante comprobanteCxP = ObjectCopier.Clone(comprobanteOld);
                    //Correccion Footer
                    comprobanteCxP.Footer = new List<DTO_ComprobanteFooter>();
                    //Partida
                    contraProv.vlrMdaLoc.Value *= -1;
                    contraProv.vlrMdaExt.Value *= -1;
                    contraProv.DatoAdd4.Value = string.Empty;
                    comprobanteCxP.Footer.Add(contraProv);
                    //Contrapartida
                    comprobanteCxP.Footer.Add(contraCxP);

                    //Contabiliza
                    result = this._moduloContabilidad.ContabilizarComprobante(documentID, comprobanteCxP, ctrlCxP.PeriodoDoc.Value.Value,
                        ModulesPrefix.cp, 0, false);

                    if (result.Result == ResultValue.NOK)
                        return result;

                    #endregion

                    #endregion
                }

                #region Guarda en noNovedadesNomina si requiere
                DTO_cpCuentaXPagar cxp = this._dal_CuentasXPagar.DAL_CuentasXPagar_Get(numeroDoc);
                string conceptoCxPNominaAntic = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPAnticiposEmpl);
                if (conceptoCxPNominaAntic.Equals(cxp.ConceptoCxPID.Value) && !string.IsNullOrEmpty(cxp.Dato9.Value) && !string.IsNullOrEmpty(cxp.Dato10.Value))
                {
                    this._moduloNomina = (ModuloNomina)this.GetInstance(typeof(ModuloNomina), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    List<DTO_noNovedadesNomina> list = new List<DTO_noNovedadesNomina>();
                    DTO_noNovedadesNomina dto = new DTO_noNovedadesNomina();
                    dto.EmpresaID.Value = this.Empresa.ID.Value;
                    dto.Valor.Value = ctrlCxP.Valor.Value;
                    dto.ConceptoNOID.Value = cxp.Dato10.Value;
                    dto.EmpleadoID.Value = ctrlCxP.TerceroID.Value;
                    dto.PeriodoPago.Value = Convert.ToByte(cxp.Dato9.Value);
                    dto.FijaInd.Value = string.IsNullOrEmpty(cxp.Dato8.Value)? false :  Convert.ToBoolean(cxp.Dato8.Value);
                    dto.OrigenNovedad.Value = 2;
                    dto.ActivaInd.Value = true;
                    list.Add(dto);
                    result = this._moduloNomina.Nomina_AddNovedadNomina(list,true);
                    if (result.Result == ResultValue.NOK)
                        return result;
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Aprobar");

                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.Err_AprobComp + "&&" + periodo.ToString() + "&&" + compID + "&&" + compNro.ToString() + ". " + errMsg;
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    #region Commit y generacion de consecutivos
                    base._mySqlConnectionTx.Commit();

                    if (ctrlProvision != null)
                    {
                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        ctrlProvision.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrlProvision.PrefijoID.Value, ctrlProvision.PeriodoDoc.Value.Value,
                            ctrlProvision.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(ctrlProvision, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlProvision.NumeroDoc.Value.Value, ctrlProvision.ComprobanteIDNro.Value.Value, false);
                    }
                    #endregion
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        #endregion

        #region Funcion Publicas

        /// <summary>
        /// Agrega una lista de CxP
        /// </summary>
        /// <param name="documentID">Documnto que inicia la tx</param>
        /// <param name="actividadFlujoID">Actividad de flujo</param>
        /// <param name="concSaldoID">Nuevo concepto de saldo para las cuentas</param>
        /// <param name="ctrlList">Lista de documentos</param>
        /// <param name="cxpList">Lista de cuentas por pagar</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Migracion(int documentID, string actividadFlujoID, string concSaldoID, List<DTO_glDocumentoControl> ctrlList, List<DTO_cpCuentaXPagar> cxpList, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                DTO_TxResult rSuppl = new DTO_TxResult();
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
                    ctrlAux = this._moduloGlobal.glDocumentoControl_GetExternalDoc(ctrl.DocumentoID.Value.Value, ctrl.TerceroID.Value, ctrl.DocumentoTercero.Value);
                    if (ctrlAux != null)
                    {
                        rd = new DTO_TxResultDetail();
                        rd.line = i + 1;
                        rd.Message = DictionaryMessages.Err_Gl_DocExtAdded;

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
                    #region Guarda la info de la CxP
                    if (result.Result == ResultValue.OK)
                    {
                        DTO_cpCuentaXPagar cxp = cxpList[i];
                        cxp.NumeroDoc.Value = ctrl.NumeroDoc.Value.Value;
                        cxp.TerceroID.Value = ctrl.TerceroID.Value;
                        vlrML = cxp.ValorLocal.Value.Value;
                        vlrME = cxp.ValorExtra.Value.Value;

                        rSuppl = this.CuentasXPagar_Add(cxp);
                        if (rSuppl.Result == ResultValue.NOK)
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloGlobal_glDocumentoControl_AddList");
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

        /// <summary>
        /// Consulta una cuenta por pagar segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_cpCuentaXPagar CuentasXPagar_Get(int NumeroDoc)
        {
            this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_CuentasXPagar.DAL_CuentasXPagar_Get(NumeroDoc);
        }

        /// <summary>
        /// adiciona en tabla cpCuentaXPagar 
        /// </summary>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Add(DTO_cpCuentaXPagar cta)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            bool validDto = true;
            try
            {
                #region Validar FKs

                #region cpConceptoCxPID

                if (!string.IsNullOrWhiteSpace(cta.ConceptoCxPID.Value))
                {
                    DAL_MasterSimple dalMasterSimple = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalMasterSimple.DocumentID = AppMasters.cpConceptoCXP;
                    DTO_MasterBasic dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, cta.ConceptoCxPID.Value, true, false);

                    if (dto == null)
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.Message = DictionaryMessages.Cp_NoCtaCxP;
                        rd.line = 1;

                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "ConceptoCxPID";
                        rdF.Message = DictionaryMessages.FkNotFound + "&&" + cta.ConceptoCxPID.Value;
                        rd.DetailsFields.Add(rdF);

                        result.Details.Add(rd);
                        result.Result = ResultValue.NOK;
                        validDto = false;
                    }
                }

                #endregion

                #endregion
                if (!validDto)
                {
                    result.ResultMessage = "NOK";
                    result.Result = ResultValue.NOK;
                    return result;
                }

                this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CuentasXPagar.DAL_CuentasXPagar_Add(cta);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Add");
                return result;
            }
        }

        /// <summary>
        /// Radicar Factura en tabla cpCuentaXPagar y asociar en glDocumentoControl
        /// </summary>
        /// <param name="_dtoCtrl">referencia documento</param>
        /// <param name="cta">factura</param>
        /// <param name="userId">usuario digitador</param>
        /// <returns></returns>
        public DTO_SerializedObject CuentasXPagar_Radicar(int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpCuentaXPagar cta, bool mainWindow, bool update, 
            out int numeroDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx, bool checkNivelApprove = false)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_Alarma alarma = null;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                numeroDoc = 0;
                int radicaCodigo = 0;

                #region Validaciones
                #region Valida info de control

                string docTipo = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_TipoDocumentoRadicacionFact);
                string docClase = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ClaseDocumentoRadicacionFact);
                string docTipoMvto = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_TipoMovimientoRadicacionFact);

                if (mainWindow)
                {
                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docTipo))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_TipoDocumentoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }

                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docClase))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_ClaseDocumentoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }

                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docTipoMvto))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_TipoMovimientoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }
                }

                #endregion
                #region Validacion de periodo abierto
                EstadoPeriodo validPeriod = this._moduloAplicacion.CheckPeriod(_dtoCtrl.PeriodoDoc.Value.Value, ModulesPrefix.cp);
                if (validPeriod != EstadoPeriodo.Abierto)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = "PeriodoID";

                    if (validPeriod == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (validPeriod == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    //Se deja en comentarios para poder mostrar el mensaje correctamente
                    numeroDoc = 0;
                    result.Result = ResultValue.NOK;
                    return result;
                }
                #endregion
                #region Validacion de provisiones
                if (_dtoCtrl.PeriodoDoc.Value.Value.Month != _dtoCtrl.FechaDoc.Value.Value.Month)
                {
                    DateTime periodoValida = _dtoCtrl.PeriodoDoc.Value.Value.AddMonths(-1);

                    #region Valida el periodo de contabilidad
                    validPeriod = this._moduloAplicacion.CheckPeriod(periodoValida, ModulesPrefix.co);
                    if (validPeriod != EstadoPeriodo.Abierto)
                    {
                        //Se deja en comentarios para poder mostrar el mensaje correctamente
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_InvalidPeriodoProvisiones + "&&" + periodoValida.ToString(FormatString.ControlDate);
                        return result;
                    }
                    #endregion
                    #region Valida el periodo de impuestos
                    validPeriod = this._moduloAplicacion.CheckPeriod(periodoValida, ModulesPrefix.co);
                    if (validPeriod != EstadoPeriodo.Abierto)
                    {
                        //Se deja en comentarios para poder mostrar el mensaje correctamente
                        numeroDoc = 0;
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_InvalidPeriodoProvisiones + "&&" + periodoValida.ToString(FormatString.ControlDate);
                        return result;
                    }
                    #endregion
                }

                #endregion
                #endregion

                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                if (!update)
                {
                    #region Guardar en glDocumentoControl

                    string defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                    string defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                    string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                    if (_dtoCtrl.ProyectoID == null)
                        _dtoCtrl.ProyectoID.Value = defProyecto;

                    if (_dtoCtrl.CentroCostoID == null)
                        _dtoCtrl.CentroCostoID.Value = defCentroCosto;

                    if (_dtoCtrl.LineaPresupuestoID == null)
                        _dtoCtrl.LineaPresupuestoID.Value = defLineaPresupuesto;

                    if (_dtoCtrl.LugarGeograficoID == null)
                        _dtoCtrl.LugarGeograficoID.Value = defLugarGeografico;

                    _dtoCtrl.DocumentoNro.Value = 0;

                    int docTemp = _dtoCtrl.DocumentoID.Value.Value == AppDocuments.CausarFacturas ? documentID : AppDocuments.RadicacionNotaCreditoCxP;
                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(docTemp, _dtoCtrl, true);
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
                    #region Consecutivo radicación
                    DTO_coTercero tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, _dtoCtrl.TerceroID.Value, true, false);
                    if (mainWindow)
                    {
                        DTO_glGestionDocumentalBitacora bita = new DTO_glGestionDocumentalBitacora();
                        bita.DocumentoTipoID.Value = docTipo;
                        bita.DocumentoClaseID.Value = docClase;
                        bita.DocTipoMovimientoID.Value = docTipoMvto;
                        bita.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                        bita.Fecha.Value = DateTime.Now;
                        bita.TerceroID.Value = tercero.ID.Value;
                        bita.Nombre.Value = tercero.Descriptivo.Value;
                        bita.DocumentoTercero.Value = _dtoCtrl.DocumentoTercero.Value;
                        bita.Observacion.Value = _dtoCtrl.Observacion.Value;

                        radicaCodigo = this._moduloGlobal.glGestionDocumentalBitacora_Add(bita);
                    }
                    #endregion
                    #region Consecutivo FacturaEquivalente 
                    if (!string.IsNullOrEmpty(tercero.ReferenciaID.Value))
                    {
                        DTO_coRegimenFiscal reg = (DTO_coRegimenFiscal)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, tercero.ReferenciaID.Value, true, false);
                        if (reg.FactEquivalenteInd.Value.Value)
                        {
                            string EmpNro = this.Empresa.NumeroControl.Value;
                            string keyControl = EmpNro + "02" + AppControl.cp_ConsecutivoFacturaEquivalente;
                            DTO_glControl glControl = this._moduloGlobal.GetControlByID(Convert.ToInt32(keyControl));
                            if (glControl != null)
                            {
                                int newConsec = glControl.Data != null ? Convert.ToInt32(glControl.Data.Value) + 1 : 1;
                                // Actualiza el consecutivo de factura equivalente en glControl
                                glControl.Data.Value = newConsec.ToString();
                                this._moduloGlobal.glControl_Update(glControl);
                            }  
                        }                       
                    }
                    #endregion
                    #region Guardar en cpCuentasXPagar
                    cta.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                    cta.RadicaCodigo.Value = radicaCodigo.ToString();
                    cta.TerceroID.Value = _dtoCtrl.TerceroID.Value;
                    result = this.CuentasXPagar_Add(cta);
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
                    #region Actualiza en glDocumentoControl

                    DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Update(_dtoCtrl, false, true);
                    if (resultGLDC.Message == "NOK")
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
                    #region Consecutivo radicación
                    if (mainWindow)
                    {
                        DTO_coTercero tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, _dtoCtrl.TerceroID.Value, true, false);

                        DTO_glGestionDocumentalBitacora bita = new DTO_glGestionDocumentalBitacora();
                        bita.DocumentoTipoID.Value = docTipo;
                        bita.DocumentoClaseID.Value = docClase;
                        bita.DocTipoMovimientoID.Value = docTipoMvto;
                        bita.NumeroDoc.Value = _dtoCtrl.NumeroDoc.Value;
                        bita.Fecha.Value = DateTime.Now;
                        bita.TerceroID.Value = tercero.ID.Value;
                        bita.Nombre.Value = tercero.Descriptivo.Value;
                        bita.DocumentoTercero.Value = _dtoCtrl.DocumentoTercero.Value;
                        bita.Observacion.Value = _dtoCtrl.Observacion.Value;

                        radicaCodigo = this._moduloGlobal.glGestionDocumentalBitacora_Add(bita);
                    }
                    #endregion
                    #region Actualiza en cpCuentaXPagar

                    cta.RadicaCodigo.Value = radicaCodigo.ToString();
                    cta.TerceroID.Value = _dtoCtrl.TerceroID.Value;
                    this.CuentasXPagar_Upd(cta);
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                }

                #region Guardar o actualiza en glDocumentoAprueba(Aprobacion Facturas)
                bool radicaObliga = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_IndRadicacionObligatoria).Equals("1") ? true : false;
                if (radicaObliga && checkNivelApprove)
                {
                    DTO_glConsulta consulta = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    List<string> actividades = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RadicacionFactura);
                    if (actividades.Count > 0)
                    {
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "ActividadPadre",
                            ValorFiltro = actividades[0],
                            OperadorFiltro = OperadorFiltro.Igual,
                        });
                        consulta.Filtros = filtros;

                        #region Verifica si existe la aprobacion de Radicacion con la actividad
                        this._dal_MasterComplex.DocumentID = AppMasters.glProcedimientoFlujo;
                        long count = this._dal_MasterComplex.DAL_MasterComplex_Count(consulta, true);
                        List<DTO_MasterComplex> _lisActividad = this._dal_MasterComplex.DAL_MasterComplex_GetPaged(count, 1, consulta, true).ToList();
                        if (_lisActividad.Count > 0)
                        {
                            DTO_glProcedimientoFlujo _flujo = (DTO_glProcedimientoFlujo)_lisActividad[0];
                            List<string> actividadesAprob = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RadicacionFactAprob);
                            if (actividadesAprob.Count > 0 && _flujo.ActividadHija.Value.Equals(actividadesAprob[0].TrimEnd()))
                            {
                                result = this._moduloGlobal.glDocumentoAprueba_AddByNivelApr(documentID, cta.NumeroDoc.Value.Value, cta.Valor.Value.Value);
                                if (update)
                                    result = this.AsignarFlujo(documentID, cta.NumeroDoc.Value.Value, actividades[0], false, string.Empty);
                                if (result.Result == ResultValue.NOK)
                                {
                                    numeroDoc = 0;
                                    return result;
                                }
                                porcTotal += porcParte;
                                batchProgress[tupProgress] = (int)porcTotal;
                            }
                        }
                        #endregion
                    }
                }
                #endregion;

                numeroDoc = cta.NumeroDoc.Value.Value;

                //Trae la info de la alarma
                alarma = this.GetFirstMailInfo(numeroDoc, false);
                alarma.ExtraField = radicaCodigo.ToString();
                return alarma;
            }
            catch (Exception ex)
            {
                numeroDoc = 0;
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Radicar");

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

                        #region Genera consecutivos
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;

                        if (!update && _dtoCtrl.DocumentoTipo.Value.Value == (short)DocumentoTipo.DocInterno)
                        {
                            _dtoCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, _dtoCtrl.PrefijoID.Value);
                            this._moduloGlobal.ActualizaConsecutivos(_dtoCtrl, true, false, false);
                            alarma.Consecutivo = _dtoCtrl.DocumentoNro.Value.ToString();
                        }
                        #endregion
                    }
                    else if (!update && _dtoCtrl.DocumentoTipo.Value.Value == (short)DocumentoTipo.DocInterno)
                        throw new Exception("CuentasXPagar_Radicar - Los consecutivos deben ser generados por la transaccion padre");

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
        public DTO_TxResult CuentasXPagar_Devolver(int documentID, DTO_glDocumentoControl dtoCtrl, DTO_cpCuentaXPagar cta, bool mainWindow, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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

                #region Valida info de control

                string docTipo = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_TipoDocumentoRadicacionFact);
                string docClase = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ClaseDocumentoRadicacionFact);
                string docTipoMvto = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_TipoMovimientoDevolucionFact);

                if (mainWindow)
                {
                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docTipo))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_TipoDocumentoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }

                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docClase))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_ClaseDocumentoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }

                    //Valida que tenga tipo de documento para radicación
                    if (string.IsNullOrWhiteSpace(docTipoMvto))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + ((int)ModulesPrefix.cp).ToString() + AppControl.cp_TipoMovimientoRadicacionFact + "&&" + string.Empty;

                        return result;
                    }
                }

                #endregion
                #region Valida si el comprobante existe
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
                #endregion
                #region Consecutivo radicación
                int devolucionCodigo = 0;
                if (mainWindow)
                {
                    DTO_coTercero tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, dtoCtrl.TerceroID.Value, true, false);

                    DTO_glGestionDocumentalBitacora bita = new DTO_glGestionDocumentalBitacora();
                    bita.DocumentoTipoID.Value = docTipo;
                    bita.DocumentoClaseID.Value = docClase;
                    bita.DocTipoMovimientoID.Value = docTipoMvto;
                    bita.NumeroDoc.Value = dtoCtrl.NumeroDoc.Value.Value;
                    bita.Fecha.Value = DateTime.Now;
                    bita.TerceroID.Value = tercero.ID.Value;
                    bita.Nombre.Value = tercero.Descriptivo.Value;
                    bita.DocumentoTercero.Value = dtoCtrl.DocumentoTercero.Value;
                    bita.Observacion.Value = dtoCtrl.Observacion.Value;

                    devolucionCodigo = this._moduloGlobal.glGestionDocumentalBitacora_Add(bita);
                    result.ExtraField = devolucionCodigo.ToString();
                }
                #endregion
                #region Actualiza el documento y la CxP
                EstadoDocControl estadoNuevo = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), dtoCtrl.Estado.Value.Value.ToString());
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, dtoCtrl.NumeroDoc.Value.Value, estadoNuevo, estadoNuevo.ToString(), true);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                //Actualiza Tabla cpCuentaXPagar
                cta.RadicaCodigo.Value = devolucionCodigo.ToString();
                DTO_TxResult text = this.CuentasXPagar_Upd(cta);
                #endregion


                //Elimina las alarmas
                this.DeshabilitarAlarma(cta.NumeroDoc.Value.Value, string.Empty);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Devolver");

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
        /// Carga la informacion completa de una facturA
        /// </summary>
        /// <param name="terceroID">Identificador del tercero</param>
        /// <param name="documentoTercero">Documento del tercero</param>
        /// <returns>Retorna la factura</returns>
        public DTO_CuentaXPagar CuentasXPagar_GetForCausacion(int documentoID, string terceroID, string documentoTercero, bool checkEstado = true)
        {
            try
            {
                DTO_CuentaXPagar res = new DTO_CuentaXPagar();

                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetExternalDoc(documentoID, terceroID, documentoTercero);

                //Si no existe devuelve null
                if (docCtrl == null)
                    return null;

                //Verifica el estado
                res.DocControl = docCtrl;
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                if (checkEstado)
                {
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                        return res;
                }

                //Carga la CxP
                DTO_cpCuentaXPagar cxp = this.CuentasXPagar_Get(docCtrl.NumeroDoc.Value.Value);
                res.CxP = cxp;

                //Revisa si tiene comprobante
                if (!string.IsNullOrEmpty(docCtrl.ComprobanteID.Value))
                {
                    bool isPre = estado == EstadoDocControl.ParaAprobacion || estado == EstadoDocControl.SinAprobar || estado == EstadoDocControl.Radicado ? true : false;
                    DTO_Comprobante comp = this._moduloContabilidad.Comprobante_Get(true, isPre, docCtrl.PeriodoDoc.Value.Value, docCtrl.ComprobanteID.Value, docCtrl.ComprobanteIDNro.Value.Value, null, null);

                    if (documentoID == AppDocuments.NotaCreditoCxP)
                    {
                        DTO_Comprobante compNC = new DTO_Comprobante();
                        compNC.Header = comp.Header;
                        compNC.Footer = this.CambiarSignoComprobante(comp.Footer);
                        res.Comp = compNC;
                    }
                    else
                        res.Comp = comp;
                }
                else
                    res.Comp = null;

                return res;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_GetForCausacion");
                return null;
            }
        }

        /// <summary>
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="cxp">Cuenta por pagar</param>
        /// <param name="comp">Comprobante que se debe agregar</param>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_Causar(int documentID, DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, DTO_Comprobante comp, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                if (cxp.NumeroDoc.Value == 0)
                {
                    int numDoc = 0;
                    #region Causa una factura que no ha sido previamente radicada
                    DTO_SerializedObject obj = this.CuentasXPagar_Radicar(documentID, ctrl, cxp, false, false, out numDoc, new Dictionary<Tuple<int, int>, int>(), true, true);
                    if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)obj;
                        return result;
                    }
                    #endregion

                    docCtrl = ctrl;
                    docCtrl.NumeroDoc.Value = numDoc;
                    comp.Header.NumeroDoc.Value = numDoc;
                }
                else
                {
                    #region Revisa si tiene actividad de flujo activa la radicacion

                    string radicaObligaStr = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_IndRadicacionObligatoria);
                    bool radicaObliga = radicaObligaStr == "0" ? false : true;

                    if (!radicaObliga)
                    {
                        string actAprobRadicaID = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.RadicacionFactAprob).First();
                        result = this.AsignarFlujo(documentID, cxp.NumeroDoc.Value.Value, actAprobRadicaID, false, string.Empty);

                        if (result.Result == ResultValue.NOK)
                            return result;
                    }
                    #endregion
                    #region Factura existente
                    docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(cxp.NumeroDoc.Value.Value);
                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_FacturaCausar;
                        return result;
                    }

                    #region Actualiza glDocumentoControl
                    this._moduloGlobal.glDocumentoControl_Update(ctrl, false, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                    #region Actualiza cpCuentaXPagar
                    DTO_cpCuentaXPagar cxpCausar = this.CuentasXPagar_Get(cxp.NumeroDoc.Value.Value);
                    cxpCausar.FacturaFecha.Value = cxp.FacturaFecha.Value;
                    cxpCausar.ContabFecha.Value = DateTime.Now;
                    cxpCausar.ConceptoCxPID.Value = cxp.ConceptoCxPID.Value;
                    cxpCausar.VtoFecha.Value = cxp.VtoFecha.Value;
                    cxpCausar.Valor.Value = cxp.Valor.Value;
                    cxpCausar.IVA.Value = cxp.IVA.Value;
                    cxpCausar.ValorTercero.Value = cxp.ValorTercero.Value;
                    cxpCausar.TerceroID.Value = ctrl.TerceroID.Value;
                    result = this.CuentasXPagar_Upd(cxpCausar);
                    if (result.Result == ResultValue.NOK)
                        return result;

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion                    
                    #endregion                    
                    #region Presupuesto
                    //Verifica si tiene activo el Modulo de Planeacion
                    bool modulePlaneacionActive = this._moduloProveedores.GetModuleActive(ModulesPrefix.pl);
                    if (modulePlaneacionActive)
                    {
                        //this._moduloPlaneacion = (ModuloPlaneacion)this.GetInstance(typeof(ModuloPlaneacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                        //result = this._moduloPlaneacion.GeneraPresupuesto(AppDocuments.CausarFacturas, cxp.NumeroDoc.Value.Value);
                        //if (result.Result == ResultValue.NOK)
                        //    return result;
                    }
                    #endregion
                }

                #region Guarda la info
                if (documentID == AppDocuments.NotaCreditoCxP)
                {
                    DTO_Comprobante compNC = new DTO_Comprobante();
                    compNC.Header = comp.Header;
                    compNC.Footer = this.CambiarSignoComprobante(comp.Footer);

                    result = this._moduloContabilidad.ComprobantePre_Add(documentID, ModulesPrefix.cp, compNC, docCtrl.AreaFuncionalID.Value, docCtrl.PrefijoID.Value, true, cxp.NumeroDoc.Value.Value, null, new Dictionary<Tuple<int, int>, int>(), true);
                }
                else
                    result = this._moduloContabilidad.ComprobantePre_Add(documentID, ModulesPrefix.cp, comp, docCtrl.AreaFuncionalID.Value, docCtrl.PrefijoID.Value, true, cxp.NumeroDoc.Value.Value, null, new Dictionary<Tuple<int, int>, int>(), true);
                #endregion
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_cpCuentasXPagar_Causar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Genera consecutivos

                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        this._moduloProveedores._mySqlConnectionTx = null;

                        if (comp.Header.ComprobanteNro.Value.Value == 0)
                        {
                            DTO_coComprobante coComp = coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, comp.Header.ComprobanteID.Value, true, false);
                            ctrl.ComprobanteID.Value = coComp.ID.Value;
                            ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, ctrl.PrefijoID.Value, comp.Header.PeriodoID.Value.Value, ctrl.DocumentoNro.Value.Value);

                            this._moduloGlobal.ActualizaConsecutivos(ctrl, false, true, true);
                            this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, true);

                            result.ResultMessage = DictionaryMessages.Co_NumberComp + "&&" + ctrl.ComprobanteID.Value + "&&" + ctrl.ComprobanteIDNro.Value.Value.ToString();
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
        /// Causa una cuenta por pagar radicada
        /// </summary>
        /// <param name="listCxP">Lista de Cuenta por pagar</param>
        /// <returns></returns>
        public DTO_TxResult CuentasXPagar_CausarMasivo(int documentID, List<DTO_CuentaXPagar> listCxP, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            decimal porcTotal = 0;
            decimal porcParte = 100 / 3;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloProveedores = (ModuloProveedores)this.GetInstance(typeof(ModuloProveedores), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                foreach (DTO_CuentaXPagar cxp in listCxP)               
                {
                    #region Crea cada CxP
                    result = this.CuentasXPagar_Causar(documentID,cxp.DocControl,cxp.CxP,cxp.Comp,batchProgress,true);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion                  
                }                
                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_CausarMasivo");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        #region Genera consecutivos
                        base._mySqlConnectionTx.Commit();

                        base._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        this._moduloProveedores._mySqlConnectionTx = null;

                        foreach (DTO_CuentaXPagar cxp in listCxP)
                        {
                            if (cxp.Comp.Header.ComprobanteNro.Value.Value == 0)
                            {
                                DTO_coComprobante coComp = coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, cxp.Comp.Header.ComprobanteID.Value, true, false);
                                cxp.DocControl.ComprobanteID.Value = coComp.ID.Value;
                                cxp.DocControl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp,  cxp.DocControl.PrefijoID.Value, cxp.Comp.Header.PeriodoID.Value.Value,  cxp.DocControl.DocumentoNro.Value.Value);

                                this._moduloGlobal.ActualizaConsecutivos( cxp.DocControl, false, true, true);
                                this._moduloContabilidad.ActualizaComprobanteNro( cxp.DocControl.NumeroDoc.Value.Value,  cxp.DocControl.ComprobanteIDNro.Value.Value, true);

                                result.ResultMessage = DictionaryMessages.Co_NumberComp + "&&" +  cxp.DocControl.ComprobanteID.Value + "&&" +  cxp.DocControl.ComprobanteIDNro.Value.Value.ToString();
                            } 
                        }

                        #endregion
                    }
                }
                else if (base._mySqlConnectionTx != null)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Trae un listado de causaciones pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_CausacionAprobacion> CuentasXPagar_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID, bool checkUser)
        {
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                DTO_glActividadFlujo act = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actFlujoID, true, false);
                int documento = Convert.ToInt32(act.DocumentoID.Value);

                List<DTO_CausacionAprobacion> list = this._dal_CuentasXPagar.DAL_CuentasXPagar_GetPendientesByModulo(documento, mod, actFlujoID, (documento == AppDocuments.RadicacionFactAprob ? seUsuario.ReplicaID.Value.ToString() : seUsuario.ID.Value), checkUser);
                foreach (DTO_CausacionAprobacion item in list)
                    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);

                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_GetPendientesByModulo");
                return null;
            }
        }

        /// <summary>
        /// Aprueba o rechazas causacion de facturas
        /// </summary>
        /// <param name="documentID">id documento</param>
        /// <param name="comps">lista comprobantes</param>
        /// <param name="updDocCtrl">actualiza el documento control</param>
        /// <param name="createDoc">genera archivo fisico</param>
        /// <param name="batchProgress">progreso proceso</param>
        /// <returns>listado de DT</returns>
        public List<DTO_SerializedObject> CuentasXPagar_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_ComprobanteAprobacion> comps, bool updDocCtrl, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();

            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                decimal porcPrevio = 0;
                decimal porcTotal = 0;
                decimal porcTemp = 0;
                decimal porcParte = 100;
                int i = 0;

                foreach (DTO_ComprobanteAprobacion comp in comps)
                {

                    #region Variables
                    porcTemp = (porcParte * i) / comps.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
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
                    int compNro = comp.ComprobanteNro.Value != null ? comp.ComprobanteNro.Value.Value : 0;
                    string obs = comp.Observacion.Value;
                    #endregion
                    if (comp.Aprobado.Value.Value)
                    {
                        if (documentID != AppDocuments.RadicacionFactAprob)
                            result = this.CuentasXPagar_Aprobar(documentID, actividadFlujoID, numeroDoc, periodo, compID, compNro, obs, updDocCtrl);
                        else
                        {
                            if (this._moduloGlobal == null)
                                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                            DTO_glDocumentoAprueba docAprueba = this._moduloGlobal.glDocumentoAprueba_UpdateUserApprover(comp.NumeroDoc.Value.Value);
                            if (docAprueba == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_UpdateData;
                            }
                            //Si UsuarioAprueba es null realiza el proceso de Aprobacion final
                            else if (docAprueba.UsuarioAprueba.Value == null)
                            {
                                this.AsignarFlujo(documentID, comp.NumeroDoc.Value.Value, actividadFlujoID, false, comp.Observacion.Value);
                            }
                        }
                    }
                    else if (comp.Rechazado.Value.Value)
                    {
                        try
                        {
                            if (documentID != AppDocuments.RadicacionFactAprob)
                            {

                                if (this._moduloContabilidad == null)
                                {
                                    this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                                }
                                this._moduloContabilidad.Comprobante_Rechazar(documentID, actividadFlujoID, ctrlDocID, numeroDoc, periodo, compID, compNro, obs, false);
                            }
                            else
                            {
                                if (this._moduloGlobal == null)
                                    this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, comp.NumeroDoc.Value.Value, EstadoDocControl.Radicado, comp.Observacion.Value, true);
                                this.AsignarFlujo(documentID, comp.NumeroDoc.Value.Value, actividadFlujoID, true, comp.Observacion.Value);
                            }
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "CuentasXPagar_AprobarRechazar");
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Revierte una cuenta por pagar
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="numeroDoc"></param>
        /// <param name="consecutivoPos">Posicion del los documentos en la lista. Lleva el control de los consecutivos (Null si es el primero)</param>
        /// <param name="ctrls">Documento de reversion</param>
        /// <param name="coComps">Comprobante (maestra) de reversion (si existe)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_TxResult CuentasXPagar_Revertir(int documentID, int numeroDoc, int? consecutivoPos, ref List<DTO_glDocumentoControl> ctrls,
            ref List<DTO_coComprobante> coComps, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Inicia las variables globales

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

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

                #region Variables

                //Variables de CxP
                DTO_glDocumentoControl ctrlOld = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);

                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctrlOld.CuentaID.Value, true, false);
                bool isML = mdaLoc == ctrlOld.MonedaID.Value ? true : false;

                DTO_coCuentaSaldo saldoDTO = this._moduloContabilidad.Saldo_GetByDocumento(cta.ID.Value, cta.ConceptoSaldoID.Value, numeroDoc, string.Empty);

                if (saldoDTO != null)
                {
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
                    if (ctrlOld.Valor.Value.Value != Math.Abs(saldo))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidSaldoDoc;
                        return result;
                    } 
                }

                // Valida que todos los pagos esten anulados
                List<EstadoDocControl> estados = new List<EstadoDocControl>();
                estados.Add(EstadoDocControl.Anulado);
                estados.Add(EstadoDocControl.Revertido);
                int pagosAnulados = this._moduloContabilidad.Comprobante_GetMovimientosByEstados(numeroDoc, estados, false);
                if (pagosAnulados > 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_DocConMvtos;
                    return result;
                }

                #endregion
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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_Revertir");

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

        /// <summary>
        /// Crea un dto de reporte para causacion factura (Autorizacion de giro)
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        /// <param name="aprobacionInd">Indica si el reporte esta creando en el processo de aprobacion (durante aprobacion - true) </param>
        /// <returns></returns>
        public DTO_SerializedObject DtoAutorizGiroReport(int numeroDoc, bool aprobacionInd = false)
        {
            try
            {
                #region Variables
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_coPlanCuenta _cuenta = new DTO_coPlanCuenta();
                DTO_coTercero _tercero = new DTO_coTercero();
                DTO_coCuentaGrupo _cuentaGrupo = new DTO_coCuentaGrupo();
                Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
                Dictionary<string, DTO_coTercero> cacheTercero = new Dictionary<string, DTO_coTercero>();
                Dictionary<string, DTO_coCuentaGrupo> cacheCuentaGrupo = new Dictionary<string, DTO_coCuentaGrupo>();

                DTO_glDocumentoControl docCtrl = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                DTO_cpCuentaXPagar cuentaXPagar = this.CuentasXPagar_Get(numeroDoc);
                bool isPre = (int)docCtrl.Estado.Value == (int)EstadoDocControl.ParaAprobacion || (int)docCtrl.Estado.Value == (int)EstadoDocControl.SinAprobar ? true : false;
                DTO_Comprobante comp = this._moduloContabilidad.Comprobante_Get(true, isPre, docCtrl.PeriodoDoc.Value.Value, docCtrl.ComprobanteID.Value, docCtrl.ComprobanteIDNro.Value.Value, null, null);
                DTO_cpConceptoCXP conceptoCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, cuentaXPagar.ConceptoCxPID.Value, true, false);

                DTO_ReportAutorizacionDeGiro autGiro = new DTO_ReportAutorizacionDeGiro();
                #endregion

                #region Llena DTO del reporte
                #region Header
                autGiro.EmpresaID = cuentaXPagar.EmpresaID.Value;
                autGiro.TerceroID = docCtrl.TerceroID.Value;
                #region Trae Tercero
                if (cacheTercero.ContainsKey(docCtrl.TerceroID.Value))
                    _tercero = cacheTercero[docCtrl.TerceroID.Value];
                else
                {
                    _tercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, docCtrl.TerceroID.Value, true, false);
                    cacheTercero.Add(docCtrl.TerceroID.Value, _tercero);
                }
                #endregion
                autGiro.TerceroDesc = _tercero.Descriptivo.ToString();
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), docCtrl.Estado.Value.Value.ToString());
                autGiro.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar) ? false : true;
                autGiro.DocumentoTercero = docCtrl.DocumentoTercero.Value;
                autGiro.FechaFact = cuentaXPagar.FacturaFecha.Value.Value;
                autGiro.Fecha = docCtrl.FechaDoc.Value.Value;
                autGiro.FechaVto = cuentaXPagar.VtoFecha.Value.Value;
                autGiro.Descripcion = docCtrl.Observacion.Value;
                autGiro.ComprobanteID = docCtrl.ComprobanteID.Value.Trim();
                autGiro.ComprobanteNro = docCtrl.ComprobanteIDNro.Value.ToString().Trim();
                autGiro.TasaCambio = docCtrl.TasaCambioCONT.Value.Value;
                autGiro.DocumentoID = docCtrl.DocumentoID.Value.ToString().Trim();
                #endregion
                #region Detail
                autGiro.AutorizacionDetail = new List<DTO_AutorizacionDetail>();
                foreach (DTO_ComprobanteFooter footer in comp.Footer)
                {
                    if (footer.DatoAdd4.Value != AuxiliarDatoAdd4.Contrapartida.ToString())
                    {
                        DTO_AutorizacionDetail autGiroDet = new DTO_AutorizacionDetail();
                        #region Trae Cuenta y CuentaGrupo
                        if (cacheCuenta.ContainsKey(footer.CuentaID.Value))
                            _cuenta = cacheCuenta[footer.CuentaID.Value];
                        else
                        {
                            _cuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, footer.CuentaID.Value, true, false);
                            cacheCuenta.Add(footer.CuentaID.Value, _cuenta);
                        }

                        if (cacheCuentaGrupo.ContainsKey(_cuenta.CuentaGrupoID.Value))
                            _cuentaGrupo = cacheCuentaGrupo[_cuenta.CuentaGrupoID.Value];
                        else
                        {
                            _cuentaGrupo = (DTO_coCuentaGrupo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCuentaGrupo, _cuenta.CuentaGrupoID.Value, true, false);
                            cacheCuentaGrupo.Add(_cuenta.CuentaGrupoID.Value, _cuentaGrupo);
                        }
                        #endregion
                        autGiroDet.CuentaID = footer.CuentaID.Value;
                        autGiroDet.CentroCostoID = footer.CentroCostoID.Value;
                        autGiroDet.ProyectoID = footer.ProyectoID.Value;
                        autGiroDet.LineaPresupuestoID = footer.LineaPresupuestoID.Value;
                        autGiroDet.CuentaDesc = _cuenta.Descriptivo.Value.ToString();
                        autGiroDet.ImpuestoTipoID = _cuenta.ImpuestoTipoID.Value;

                        //autGiroDet.ValorME_SinSigno = 0;
                        autGiroDet.ValorME = 0;
                        autGiroDet.VrBrutoME = 0;
                        autGiroDet.IvaME = 0;
                        autGiroDet.ReteIvaME = 0;
                        autGiroDet.ReteFuenteME = 0;
                        autGiroDet.ReteIcaME = 0;
                        autGiroDet.TimbreME = 0;
                        autGiroDet.AnticiposME = 0;
                        autGiroDet.OtrosDtosME = 0;

                        if (conceptoCxP.ControlCostoInd.Value.Value)
                        {
                            //autGiroDet.ValorML_SinSigno = footer.vlrMdaLoc.Value.Value;
                            autGiroDet.ValorML = footer.vlrMdaLoc.Value.Value;//(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value;
                            autGiroDet.VrBrutoML = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString()
                                && footer.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && _cuentaGrupo.CostoInd.Value.Value) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.IvaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA)
                                || _cuentaGrupo.CostoInd.Value.Value && !string.IsNullOrEmpty(footer.DatoAdd1.Value) && footer.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString()) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.ReteIvaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA)) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.ReteFuenteML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente)) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.ReteIcaML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA)) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.TimbreML = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo)) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.AnticiposML = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString()) ? footer.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.OtrosDtosML = footer.vlrMdaLoc.Value.Value - autGiroDet.VrBrutoML - autGiroDet.IvaML - autGiroDet.ReteIvaML - autGiroDet.ReteFuenteML
                                - autGiroDet.ReteIcaML - autGiroDet.TimbreML - autGiroDet.AnticiposML;
                            if (this.Multimoneda())
                            {
                                //autGiroDet.ValorME_SinSigno = footer.vlrMdaExt.Value.Value;
                                autGiroDet.ValorME = footer.vlrMdaExt.Value.Value;//(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaExt.Value.Value * (-1) : footer.vlrMdaExt.Value.Value;
                                autGiroDet.VrBrutoME = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString()
                                && footer.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && _cuentaGrupo.CostoInd.Value.Value) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.IvaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA)
                                    || !string.IsNullOrEmpty(footer.DatoAdd1.Value) && footer.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString()) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.ReteIvaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA)) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.ReteFuenteME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente)) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.ReteIcaME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA)) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.TimbreME = (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo)) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.AnticiposME = (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && footer.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString()) ? footer.vlrMdaExt.Value.Value : 0;
                                autGiroDet.OtrosDtosME = footer.vlrMdaExt.Value.Value - autGiroDet.VrBrutoME - autGiroDet.IvaME - autGiroDet.ReteIvaME - autGiroDet.ReteFuenteME
                                    - autGiroDet.ReteIcaME - autGiroDet.TimbreME - autGiroDet.AnticiposME;
                            }
                        }
                        else
                        {
                            //autGiroDet.ValorML_SinSigno = footer.vlrMdaLoc.Value.Value;
                            autGiroDet.ValorML = footer.vlrMdaLoc.Value.Value;//(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value;
                            autGiroDet.VrBrutoML = footer.vlrMdaLoc.Value.Value; //(_cuenta.NITCierreAnual.Value != autGiro.TerceroID) ? autGiroDet.vlrMdaLoc.Value.Value : 0;
                            autGiroDet.IvaML = 0;
                            autGiroDet.ReteIvaML = 0;
                            autGiroDet.ReteFuenteML = 0;
                            autGiroDet.ReteIcaML = 0;
                            autGiroDet.TimbreML = 0;
                            autGiroDet.AnticiposML = 0;
                            autGiroDet.OtrosDtosML = 0;
                            if (this.Multimoneda())
                            {
                                //autGiroDet.ValorME_SinSigno = footer.vlrMdaExt.Value.Value;
                                autGiroDet.ValorME = footer.vlrMdaExt.Value.Value;
                                autGiroDet.VrBrutoME = footer.vlrMdaExt.Value.Value;//(_cuenta.Naturaleza.Value.Value == (int)NaturalezaCuenta.Credito) ? footer.vlrMdaLoc.Value.Value * (-1) : footer.vlrMdaLoc.Value.Value;
                            }
                        }
                        autGiroDet.BaseML = footer.vlrBaseML.Value.Value;
                        autGiroDet.Percent = (footer.vlrBaseML.Value.Value != 0) ? (Math.Round(footer.vlrMdaLoc.Value.Value / footer.vlrBaseML.Value.Value, 2)).ToString() : "*";
                        autGiroDet.VrNetoML = autGiroDet.VrBrutoML + autGiroDet.IvaML + autGiroDet.ReteIvaML + autGiroDet.ReteFuenteML + autGiroDet.ReteIcaML
                            + autGiroDet.TimbreML + autGiroDet.AnticiposML + autGiroDet.OtrosDtosML + autGiroDet.VrNetoML;
                        autGiroDet.VrNetoME = autGiroDet.VrBrutoME + autGiroDet.IvaME + autGiroDet.ReteIvaME + autGiroDet.ReteFuenteME + autGiroDet.ReteIcaME
                            + autGiroDet.TimbreME + autGiroDet.AnticiposME + autGiroDet.OtrosDtosME + autGiroDet.VrNetoME;

                        autGiro.AutorizacionDetail.Add(autGiroDet);
                    }
                };
                #endregion
                #region Footer
                autGiro.VrBrutoML = autGiro.AutorizacionDetail.Sum(x => x.VrBrutoML);
                autGiro.IvaML = autGiro.AutorizacionDetail.Sum(x => x.IvaML);
                autGiro.ReteIvaML = autGiro.AutorizacionDetail.Sum(x => x.ReteIvaML) * (-1);
                autGiro.ReteFuenteML = autGiro.AutorizacionDetail.Sum(x => x.ReteFuenteML) * (-1);
                autGiro.ReteIcaML = autGiro.AutorizacionDetail.Sum(x => x.ReteIcaML) * (-1);
                autGiro.TimbreML = autGiro.AutorizacionDetail.Sum(x => x.TimbreML) * (-1);
                autGiro.AnticiposML = autGiro.AutorizacionDetail.Sum(x => x.AnticiposML) * (-1);
                autGiro.OtrosDtosML = autGiro.AutorizacionDetail.Sum(x => x.OtrosDtosML) * (-1);
                autGiro.VrNetoML = autGiro.AutorizacionDetail.Sum(x => x.VrNetoML);

                autGiro.VrBrutoME = autGiro.AutorizacionDetail.Sum(x => x.VrBrutoME);
                autGiro.IvaME = autGiro.AutorizacionDetail.Sum(x => x.IvaME);
                autGiro.ReteIvaME = autGiro.AutorizacionDetail.Sum(x => x.ReteIvaME) * (-1);
                autGiro.ReteFuenteME = autGiro.AutorizacionDetail.Sum(x => x.ReteFuenteME) * (-1);
                autGiro.ReteIcaME = autGiro.AutorizacionDetail.Sum(x => x.ReteIcaME) * (-1);
                autGiro.TimbreME = autGiro.AutorizacionDetail.Sum(x => x.TimbreME) * (-1);
                autGiro.AnticiposME = autGiro.AutorizacionDetail.Sum(x => x.AnticiposME) * (-1);
                autGiro.OtrosDtosME = autGiro.AutorizacionDetail.Sum(x => x.OtrosDtosME) * (-1);
                autGiro.VrNetoME = autGiro.AutorizacionDetail.Sum(x => x.VrNetoME);
                #endregion
                List<DTO_ReportAutorizacionDeGiro> autGiroList = new List<DTO_ReportAutorizacionDeGiro>();
                autGiroList.Add(autGiro);
                #endregion

                return autGiro;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoAutorizGiroReport");
                return null;
            }
        }

        /// <summary>
        /// Generar los consecutivos de las Facturas Equivalentes
        /// </summary>
        /// <param name="periodo">periodo</param>
        public DTO_TxResult CuentasXPagar_ConsecutivoFactEquivalente(DateTime periodo)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            try
            {

                this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CuentasXPagar.DAL_CuentasXPagar_ConsecutivoFactEquivalente(periodo);
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CuentasXPagar_ConsecutivoFactEquivalente");
                return result;
            }
        }

        /// <summary>
        /// Aprueba Caja Menor
        /// </summary>
        /// <param name="leg"> Caja Menor de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult PagoImpuestos_Aprobar(int documentID,List<DTO_PagoImpuesto> pagos, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_Comprobante _comprobante = null;
            List<DTO_glDocumentoControl> ctrls = new List<DTO_glDocumentoControl>();
            List<DTO_coComprobante> coComps = new List<DTO_coComprobante>();
            try
            {
              
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DateTime periodoCxP = Convert.ToDateTime(this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo));
                string prefijoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                string monedaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string proyectoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                string centroxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                string lineaPxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string concCargoXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgXdef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Recorre los impuestos
                foreach (DTO_PagoImpuesto pag in pagos.FindAll(x=>x.Selected.Value.Value))
                {
                    DTO_Comprobante comprobanteCxP = new DTO_Comprobante();
                    comprobanteCxP.Footer = new List<DTO_ComprobanteFooter>();
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    ctrl.FechaDoc.Value = periodoCxP;
                    ctrl.PeriodoDoc.Value = periodoCxP;
                    ctrl.PeriodoUltMov.Value = periodoCxP;
                    ctrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                    ctrl.PrefijoID.Value = prefijoxDef;
                    ctrl.Observacion.Value = "Pago Impuesto";
                    ctrl.Descripcion.Value = "Pago Impuesto";
                    ctrl.MonedaID.Value = monedaLoc;
                    ctrl.TasaCambioCONT.Value = 0;
                    ctrl.TasaCambioDOCU.Value = 0;
                    ctrl.TerceroID.Value = pag.TerceroID.Value;
                    ctrl.ProyectoID.Value = proyectoxDef;
                    ctrl.CentroCostoID.Value = centroxDef;

                    DTO_coImpuestoTipo imp = (DTO_coImpuestoTipo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoTipo, pag.ImpuestoTipoID.Value, true, false);
                    if (imp != null && string.IsNullOrEmpty(imp.ConceptoCxPID.Value))
                    {
                        result.Result = ResultValue.NOK;
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.Message = DictionaryMessages.Cp_NoCtaCxP;
                        rd.line = 1;
                        result.Details.Add(rd);
                    }

                    //Recorre las cuentas de cada impuesto
                    foreach (var det in pag.Detalle)
                    {
                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, det.CuentaID.Value, true, false);
                        DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);

                        DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(ctrl, cta, cSaldo, 0, concCargoXdef, lgXdef, lineaPxDef, det.ValorLocal.Value.Value,0, false);
                        compFooterTemp.Descriptivo.Value = "Pago Impuesto" + imp.Descriptivo.Value;
                        comprobanteCxP.Footer.Add(compFooterTemp);
                    }                    

                    #region Crea la CxP
                    _comprobante.Footer = comprobanteCxP.Footer;
                    object obj = this.CuentasXPagar_Generar(ctrl, imp.ConceptoCxPID.Value, pag.ValorTotalMiles.Value.Value, _comprobante.Footer, ModulesPrefix.cp, false);
                    if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)obj;
                        return result;
                    }
                    else
                    {
                        DTO_glDocumentoControl ctrlRes = (DTO_glDocumentoControl)obj;
                        DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlRes.ComprobanteID.Value, true, false);
                        ctrls.Add(ctrlRes);
                        coComps.Add(comp);                        
                    }
                    #endregion                    
                }                            
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CajaMenor_Aprobar");
                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        #region Genera consecutivos
                        base._mySqlConnectionTx = null;
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;
                        foreach (var ctrl in ctrls)
                        {
                            DTO_coComprobante coCompAnula = coComps.Find(x=>x.ID.Value == ctrl.ComprobanteID.Value);
                            if (coCompAnula != null)
                            {
                                ctrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coCompAnula, ctrl.PrefijoID.Value, ctrl.PeriodoDoc.Value.Value, ctrl.DocumentoNro.Value.Value);
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrl.NumeroDoc.Value.Value, ctrl.ComprobanteIDNro.Value.Value, false);
                            }                            
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
        /// Reclasificacion SaldosCxP
        /// </summary>
        /// <param name="facturas">Pagos a programar o desprogramar</param>
        /// <param name="cuenta">cuenta contrapartida</param>
        /// <param name="fecha">fecha Doc</param>
        /// <param name="tc">tasa de cambio</param>
        /// <returns>Retorna el resultado de la operación</returns>
        public DTO_TxResult ReclasificacionSaldosCxP(int documentID, string actividadFlujoID, List<DTO_ProgramacionPagos> facturas,string cuenta,
            DateTime fecha, decimal tc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            DTO_glDocumentoControl docCtrl = null;
            DTO_Comprobante comp = null;
            DTO_coComprobante coComp = null;
            try
            {
                #region Variables

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //this._dal_Tesoreria = (DAL_Tesoreria)this.GetInstance(typeof(DAL_Tesoreria), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                //this._dal_tsBancosDocu = (DAL_tsBancosDocu)this.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                string mdaLoc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string mdaExt = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                string libroFunc = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);

                //Variables por defecto
                string concCargoDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string lgDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string lineaDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                string terceroxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                DTO_coTercero dtoTercero = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroxDef, true, false);
                string docContable = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DocContableReclasificaSaldo);

                //Periodo
                string periodoStr = this.GetControlValueByCompany(ModulesPrefix.ts, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);

                //Variables de operación
                bool isMultimoneda = this.Multimoneda();
                DTO_coDocumento coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, docContable, true, false);

                decimal vlrContraLoc = 0;
                decimal vlrContraExt = 0;

                #endregion
                #region Validaciones

                //Valida que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;

                    return result;
                }
                coComp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);
                #endregion
                #region Carga el glDocumentoControl

                docCtrl = new DTO_glDocumentoControl();
                docCtrl.TerceroID.Value = terceroxDef;
                docCtrl.MonedaID.Value = (short)coDoc.MonedaOrigen.Value != (short)TipoMoneda_CoDocumento.Foreign ? mdaLoc : mdaExt;
                docCtrl.CuentaID.Value = cuenta;
                docCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                docCtrl.ProyectoID.Value = string.Empty; //bancoCuenta.ProyectoID.Value;
                docCtrl.CentroCostoID.Value = string.Empty;// bancoCuenta.CentroCostoID.Value;
                docCtrl.FechaDoc.Value = fecha;
                docCtrl.PeriodoDoc.Value = periodo;
                docCtrl.PrefijoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                docCtrl.ComprobanteID.Value = coDoc.ComprobanteID.Value;
                docCtrl.DocumentoNro.Value = 0;
                docCtrl.ComprobanteIDNro.Value = 0;
                docCtrl.DocumentoTercero.Value = string.Empty;// bancoCuenta.ChequeInicial.Value.Value.ToString();
                docCtrl.TasaCambioCONT.Value = tc;
                docCtrl.TasaCambioDOCU.Value = tc;
                docCtrl.DocumentoID.Value = documentID;
                docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                docCtrl.PeriodoUltMov.Value = periodo;
                docCtrl.seUsuarioID.Value = this.UserId;
                docCtrl.AreaFuncionalID.Value = this.GetAreaFuncionalByUser();
                docCtrl.LugarGeograficoID.Value = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                docCtrl.Descripcion.Value = "CONT. RECLASIFICACION SALDOS CXP";

                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);

                    return result;
                }
                docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                #endregion
                #region Carga la info del banco
                //DTO_tsBancosDocu banco = new DTO_tsBancosDocu();

                //banco.EmpresaID.Value = this.Empresa.ID.Value;
                //banco.BancoCuentaID.Value = bancoCuenta.ID.Value;
                //banco.Beneficiario.Value = dtoTercero.Descriptivo.Value;
                //banco.NroCheque.Value = bancoCuenta.ChequeInicial.Value;
                //banco.Valor.Value = (from p in programacionesPagos select p.ValorPago.Value.Value).Sum();
                //banco.IVA.Value = 0;
                //banco.MonedaPago.Value = docCtrl.MonedaID.Value;
                //banco.NumeroDoc.Value = docCtrl.NumeroDoc.Value;

                //if (isMultimoneda)
                //{
                //    banco.ValorLocal.Value = docCtrl.MonedaID.Value == mdaLoc ? banco.Valor.Value : Math.Round(banco.Valor.Value.Value * tc, 2);
                //    banco.ValorExtra.Value = docCtrl.MonedaID.Value == mdaLoc ? Math.Round(banco.Valor.Value.Value / tc, 2) : banco.Valor.Value;
                //}
                //else
                //{
                //    banco.ValorLocal.Value = banco.Valor.Value;
                //    banco.ValorExtra.Value = 0;
                //}

                //this._dal_tsBancosDocu.DAL_tsBancosDocu_Add(banco);

                #endregion
                #region Revisa los saldos, actualiza la CxP y carga la info del comprobante

                int i = 0;
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                foreach (DTO_ProgramacionPagos prog in facturas)
                {
                    int percent = ((i + 1) * 100) / facturas.Count;
                    batchProgress[tupProgress] = percent >= 80 ? 80 : percent;

                    //Trae el saldo
                    DTO_glDocumentoControl ctrlCxP = this._moduloGlobal.glDocumentoControl_GetByID(prog.NumeroDoc.Value.Value);
                    decimal saldoML = this._moduloContabilidad.Saldo_GetByDocumentoCuenta(true, periodo, ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.CuentaID.Value, libroFunc);

                    //Revisa si el saldo es el mismo
                    if (Math.Abs(saldoML) != Math.Abs(prog.SaldoML.Value.Value))
                    {
                        #region Error de saldos

                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.line = i;
                        rd.Message = DictionaryMessages.Err_Ts_SaldosMvto;

                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                        return result;

                        #endregion
                    }
                    else
                    {
                        #region Crea el detalle del comprobante

                        decimal vlrPagoLoc = prog.ValorPago.Value.Value;
                        decimal vlrPagoExt = 0;

                        if (isMultimoneda)
                        {
                            vlrPagoLoc = ctrlCxP.MonedaID.Value == mdaLoc ? prog.ValorPago.Value.Value : prog.ValorPago.Value.Value * tc;
                            vlrPagoExt = ctrlCxP.MonedaID.Value == mdaLoc ? Math.Round(prog.ValorPago.Value.Value / tc, 2) : prog.ValorPago.Value.Value;
                        }

                        //vlrContraLoc += vlrPagoLoc;
                        //vlrContraExt += vlrPagoExt;
                        DTO_ComprobanteFooter footerDet = this.CrearComprobanteFooter(ctrlCxP, docCtrl.TasaCambioDOCU.Value, concCargoDef, lgDef, lineaDef,
                            vlrPagoLoc, vlrPagoExt, false);

                        if (!string.IsNullOrWhiteSpace(prog.TerceroID.Value))
                            footerDet.TerceroID.Value = prog.TerceroID.Value;

                        footer.Add(footerDet);
                        #endregion

                        DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(docCtrl, docCtrl.TasaCambioDOCU.Value, concCargoDef, lgDef, lineaDef,
                        vlrPagoLoc * -1, vlrPagoExt * -1, true);
                        footer.Add(contraP);
                    }

                    i++;
                }
                #endregion
                #region Contabiliza el comprobante

                comp = new DTO_Comprobante();
                #region Carga el cabezote

                DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                header.ComprobanteID.Value = docCtrl.ComprobanteID.Value;
                header.ComprobanteNro.Value = 0;
                header.EmpresaID.Value = this.Empresa.ID.Value;
                header.Fecha.Value = fecha;
                header.MdaTransacc.Value = docCtrl.MonedaID.Value;
                header.MdaOrigen.Value = header.MdaTransacc.Value == mdaLoc ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                header.NumeroDoc.Value = docCtrl.NumeroDoc.Value;
                header.PeriodoID.Value = docCtrl.PeriodoDoc.Value;
                header.TasaCambioBase.Value = docCtrl.TasaCambioDOCU.Value;
                header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

                comp.Header = header;
                #endregion
                #region Crea la contrapartida

                //DTO_ComprobanteFooter contraP = this.CrearComprobanteFooter(docCtrl, header.TasaCambioBase.Value, concCargoDef, lgDef, lineaDef,
                //    vlrContraLoc * -1, vlrContraExt * -1, true);
                //footer.Add(contraP);

                comp.Footer = footer;
                #endregion

                //Contabiliza
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, comp, header.PeriodoID.Value.Value, ModulesPrefix.ts, 0, false);
                if (result.Result == ResultValue.NOK)
                    return result;

                #endregion
                #region Incrementa el cheque inicial
                ////result = this.IncrementarChequeInicial(bancoCuenta.ID.Value);
                //if (result.Result == ResultValue.NOK)
                //    return result;
                #endregion

                //Asigana el numero doc para generar el reporte
                result.ExtraField = docCtrl.NumeroDoc.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ProgramacionPagos_ProgramarPagos");

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
                        this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                        docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(coComp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);

                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(docCtrl.NumeroDoc.Value.Value, docCtrl.ComprobanteIDNro.Value.Value, false);
                        #endregion
                    }
                    else
                        throw new Exception("RegistroPagoFactura - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        #endregion

        #endregion

        #region Caja menor/Legalizacion Gastos/Tarjeta Credito

        #region Funciones Privadas

        #region General

        /// <summary>
        /// Le cambia el estado a una Legalizacion
        /// </summary>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) de cpLegalizaDocu y glDocumentoControl</param>
        /// <param name="estado">Nuevo estado</param>
        /// <param name="alarmEnabled">Indica si se debe activar o no la alarma (Null: para no tocar las alarmas)</param>
        /// <param name="alarmTareaID">Indica una tarea particular para asignar (Null: asigna la que venga de glDocumento)</param>
        /// <param name="compAnulaID">Identificador de comprobante de anulacion</param>
        /// <param name="compNroAnula">Numero de comprobante de anulacion</param>
        /// <param name="userId">Identificador del usuario que esta ejecutando la operacion</param>
        /// <returns>Retorna el identificador de la bitacora con que se guardo la info</returns>
        private void cpLegalizaDocu_ChangeDocumentStatus(int numeroDoc, EstadoInterCajaMenor estado, DTO_cpLegalizaDocu header)
        {
            this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Legalizacion.DAL_cpLegalizaDocu_ChangeDocumentStatus(numeroDoc, estado, header);
        }

        /// <summary>
        /// Actualiza la tabla LegalizaDeta
        /// </summary>
        /// <param name="numeroDoc">Numero de documento - PK (NumeroDoc) </param>
        ///  <param name="numeroDoc">Numero de factura equivalente </param>
        private void LegalizaFooter_UpdFactEquiv(int numeroDoc, string factEquivalente)
        {
            this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_Legalizacion.DAL_LegalizaFooter_UpdFactEquiv(numeroDoc, factEquivalente);
        }

        /// <summary>
        /// Permite obtener los impuestos y el detalle del comprobante
        /// </summary>
        /// <param name="leg">Legalizacion o caja</param>
        /// <param name="_dtoControl">documento control</param>
        /// <param name="_comprobante">Comprobante a modificar</param>
        /// <param name="_sumImp">sumatoria de los valores de impuestos y costos</param>
        /// <returns>Devuelve los datos de impuestos con sus cuentas</returns>
        private DTO_TxResult GetDetailImpuestos(DTO_Legalizacion leg, DTO_glDocumentoControl _dtoControl, ref DTO_Comprobante _comprobante, ref decimal _sumImp)
        {
            #region Declara y Carga variables
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #region Variables por defecto
            string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string lineaPresupuestal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            string codigoIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
            string codigoRfte = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
            string codigoReteIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
            string codigoReteICA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
            string codigoImpConsumo = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo);
            string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
            this._dal_MasterComplex = (DAL_MasterComplex)this.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion
            #region Variables de datos e indicadores
            DTO_ComprobanteFooter footerCaja;
            DTO_coPlanCuenta _planCuenta;
            DTO_coProyecto _proyecto;
            DTO_coCentroCosto _ctoCosto;
            DTO_cpCargoEspecial _cargoEsp;
            DTO_coTercero _regFiscalEmp;
            DTO_coTercero _refFiscalTercero;
            DTO_glConceptoSaldo _conceptoSaldo;
            string operacion = string.Empty;
            string _ctaIVA1 = string.Empty;
            string _ctaIVA2 = string.Empty;
            string _ctaReteIVA1 = string.Empty;
            string _ctaReteIVA2 = string.Empty;
            #endregion
            #region Variables de diccionarios
            //Diccionario para impuestos: <Cuenta, Bases, ValorImp, Descr imp, % imp>, y Dicccionarios de cache de datos
            List<Tuple<string, decimal, decimal, string, string>> listValuesCompFooter = new List<Tuple<string, decimal, decimal, string, string>>();
            Dictionary<string, DTO_coProyecto> cacheProyecto = new Dictionary<string, DTO_coProyecto>();
            Dictionary<string, DTO_coCentroCosto> cacheCtoCosto = new Dictionary<string, DTO_coCentroCosto>();
            Dictionary<string, DTO_cpCargoEspecial> cacheCargoEsp = new Dictionary<string, DTO_cpCargoEspecial>();
            Dictionary<string, DTO_coTercero> cacheTerceroEmp = new Dictionary<string, DTO_coTercero>();
            Dictionary<string, DTO_coTercero> cacheTercero = new Dictionary<string, DTO_coTercero>();
            Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
            Dictionary<string, DTO_glConceptoSaldo> cacheConceptoSaldo = new Dictionary<string, DTO_glConceptoSaldo>();
            #endregion
            #endregion
            try
            {
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                foreach (var footer in leg.Footer)
                {
                    #region Obtiene datos generales
                    #region Carga el proyecto
                    if (cacheProyecto.ContainsKey(footer.ProyectoID.Value))
                        _proyecto = cacheProyecto[footer.ProyectoID.Value];
                    else
                    {
                        _proyecto = (DTO_coProyecto)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, footer.ProyectoID.Value, true, false);
                        cacheProyecto.Add(footer.ProyectoID.Value, _proyecto);
                        operacion = _proyecto.OperacionID.Value;
                    }
                    #endregion
                    #region Carga el Centro Costo
                    if (string.IsNullOrEmpty(operacion))
                    {
                        if (cacheCtoCosto.ContainsKey(footer.CentroCostoID.Value))
                            _ctoCosto = cacheCtoCosto[footer.CentroCostoID.Value];
                        else
                        {
                            _ctoCosto = (DTO_coCentroCosto)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, footer.CentroCostoID.Value, true, false);
                            cacheCtoCosto.Add(footer.CentroCostoID.Value, _ctoCosto);
                            operacion = _ctoCosto.OperacionID.Value;
                        }
                    }
                    if (string.IsNullOrEmpty(operacion))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_NoOper + "&&" + footer.ProyectoID.Value + "&&" + footer.CentroCostoID.Value;
                        return result;
                    }
                    #endregion
                    #region Carga el Cargo Especial
                    if (cacheCargoEsp.ContainsKey(footer.CargoEspecialID.Value))
                        _cargoEsp = cacheCargoEsp[footer.CargoEspecialID.Value];
                    else
                    {
                        _cargoEsp = (DTO_cpCargoEspecial)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, footer.CargoEspecialID.Value, true, false);
                        cacheCargoEsp.Add(footer.CargoEspecialID.Value, _cargoEsp);
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
                    if (cacheTercero.ContainsKey(footer.TerceroID.Value))
                        _refFiscalTercero = cacheTercero[footer.TerceroID.Value];
                    else
                    {
                        _refFiscalTercero = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, footer.TerceroID.Value, true, false);
                        cacheTercero.Add(footer.TerceroID.Value, _refFiscalTercero);
                    }
                    #endregion
                    #endregion
                    #region Obtiene Cuentas
                    DTO_CuentaValor _ctaCosto = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(_cargoEsp.ConceptoCargoID.Value, operacion, lineaPresupuestal, footer.ValorBruto.Value.Value);
                    if (_ctaCosto == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_NoCtaCargoCosto + "&&" + _cargoEsp.ConceptoCargoID.Value + "&&" + lineaPresupuestal + "&&" + footer.ProyectoID.Value + "&&" + footer.CentroCostoID.Value;
                        return result;
                    }
                    List<DTO_SerializedObject> _listCtasImpuestos = this._moduloContabilidad.LiquidarImpuestos(ModulesPrefix.cp, _refFiscalTercero, _ctaCosto.CuentaID.Value, _cargoEsp.ConceptoCargoID.Value, operacion, footer.LugarGeograficoID.Value, lineaPresupuestal, footer.ValorBruto.Value.Value, _cargoEsp.ConceptoCargo1.Value);
                    if (_listCtasImpuestos.Count > 0)
                    {
                        if (_listCtasImpuestos.First().GetType() == typeof(DTO_TxResult))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_LiquidaImpuestosInvalid;
                            return result;
                        }
                    }
                    #endregion
                    #region LLena lista de Impuestos asignados
                    #region Costos(Bases)
                    if (footer.ValorBruto.Value != 0)//&& footer.BaseIVA1.Value == 0 && footer.BaseIVA2.Value == 0)
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.ValorBruto.Value.Value, 0), string.Empty, "0"));
                    else if (footer.ValorBruto.Value != 0 && footer.ValorBruto.Value == footer.BaseIVA1.Value && footer.BaseIVA2.Value == 0)
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, footer.BaseIVA1.Value.Value, Math.Round(footer.ValorIVA1.Value.Value, 0), string.Empty, footer.PorIVA1.Value.ToString()));
                    else if (footer.BaseIVA1.Value != 0 && footer.ValorBruto.Value != footer.BaseIVA1.Value && footer.BaseIVA2.Value == 0)
                    {
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.BaseIVA1.Value.Value, 0), string.Empty, footer.PorIVA1.Value.ToString()));
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round((footer.ValorBruto.Value.Value - footer.BaseIVA1.Value.Value), 0), string.Empty, "0"));
                    }
                    else if (footer.BaseIVA1.Value != 0 && footer.BaseIVA2.Value != 0 && footer.ValorBruto.Value == (footer.BaseIVA1.Value.Value + footer.BaseIVA2.Value.Value))
                    {
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.BaseIVA1.Value.Value, 0), string.Empty, footer.PorIVA1.Value.ToString()));
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.BaseIVA2.Value.Value, 0), string.Empty, footer.PorIVA2.Value.ToString()));
                    }
                    else if (footer.BaseIVA1.Value != 0 && footer.BaseIVA2.Value != 0 && footer.ValorBruto.Value != (footer.BaseIVA1.Value.Value + footer.BaseIVA2.Value.Value))
                    {
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.BaseIVA1.Value.Value, 0), string.Empty, footer.PorIVA1.Value.ToString()));
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(footer.BaseIVA2.Value.Value, 0), string.Empty, footer.PorIVA2.Value.ToString()));
                        listValuesCompFooter.Add(Tuple.Create(_ctaCosto.CuentaID.Value, Convert.ToDecimal(0), Math.Round(Math.Abs(footer.ValorBruto.Value.Value - (footer.BaseIVA1.Value.Value + footer.BaseIVA2.Value.Value)), 0), string.Empty, "0"));
                    }
                    #endregion
                    foreach (var item in _listCtasImpuestos)
                    {
                        DTO_CuentaValor cta = (DTO_CuentaValor)item;
                        #region IVAs
                        DTO_coOperacion oper = (DTO_coOperacion)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coOperacion, operacion, true, false);
                        if (cta.TipoImpuesto == codigoIVA && !listValuesCompFooter.Exists(x=>x.Item1 ==cta.CuentaID.Value))
                        {
                            #region IVA 1
                            if (footer.BaseIVA1.Value != 0 && footer.PorIVA1.Value != 0)
                            {
                                if (oper.IvaCostoInd.Value.Value)
                                {
                                    _ctaIVA1 = _ctaCosto.CuentaID.Value;
                                    listValuesCompFooter.Add(Tuple.Create(_ctaIVA1, footer.BaseIVA1.Value.Value, Math.Round(footer.ValorIVA1.Value.Value, 0), codigoIVA, footer.PorIVA1.Value.ToString()));
                                }
                                else
                                {
                                    _ctaIVA1 = cta.CuentaID.Value;
                                    listValuesCompFooter.Add(Tuple.Create(_ctaIVA1, footer.BaseIVA1.Value.Value, Math.Round(footer.ValorIVA1.Value.Value, 0), codigoIVA, footer.PorIVA1.Value.ToString()));
                                }
                            }
                            #endregion
                            #region IVA 2
                            if (footer.BaseIVA2.Value != 0 && footer.PorIVA2.Value != 0)
                            {
                                Dictionary<string, string> keyscoImpuestoIva2 = new Dictionary<string, string>();
                                keyscoImpuestoIva2.Add("EmpresaGrupoID", this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coImpuesto, this.Empresa, egCtrl));
                                keyscoImpuestoIva2.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                                keyscoImpuestoIva2.Add("RegimenFiscalTerceroID", _refFiscalTercero.ReferenciaID.Value);
                                keyscoImpuestoIva2.Add("LugarGeograficoID", this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto));
                                keyscoImpuestoIva2.Add("ConceptoCargoID", _cargoEsp.ConceptoCargo1.Value);
                                keyscoImpuestoIva2.Add("ImpuestoTipoID", codigoIVA);
                                this._dal_MasterComplex.DocumentID = AppMasters.coImpuesto;
                                DTO_coImpuesto _impIva2 = (DTO_coImpuesto)this._dal_MasterComplex.DAL_MasterComplex_GetByID(keyscoImpuestoIva2, true);
                                if (_impIva2 != null)
                                {
                                    if (oper.IvaCostoInd.Value.Value)
                                    {
                                        _ctaIVA2 = _ctaCosto.CuentaID.Value;
                                        listValuesCompFooter.Add(Tuple.Create(_ctaIVA2, footer.BaseIVA2.Value.Value, Math.Round(footer.ValorIVA2.Value.Value, 0), codigoIVA, footer.PorIVA2.Value.ToString()));
                                    }
                                    else
                                    {
                                        _ctaIVA2 = _impIva2.CuentaID.Value;
                                        listValuesCompFooter.Add(Tuple.Create(_ctaIVA2, footer.BaseIVA2.Value.Value, Math.Round(footer.ValorIVA2.Value.Value, 0), codigoIVA, footer.PorIVA2.Value.ToString()));
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #region ReteIVAs
                        if (cta.TipoImpuesto == codigoReteIVA && !listValuesCompFooter.Exists(x => x.Item1 == cta.CuentaID.Value))
                        {
                            #region ReteIVA1
                            if (footer.BaseRteIVA1.Value != 0 && footer.PorRteIVA1.Value != 0)
                            {
                                _ctaReteIVA1 = cta.CuentaID.Value;
                                listValuesCompFooter.Add(Tuple.Create(_ctaReteIVA1, footer.BaseRteIVA1.Value.Value, Math.Round(footer.ValorRteIVA1.Value.Value * (-1), 0), string.Empty, "0"));
                                if (footer.RteIVA1AsumidoInd.Value.Value)
                                    listValuesCompFooter.Add(Tuple.Create(_ctaReteIVA1, footer.BaseRteIVA1.Value.Value, Math.Round(footer.ValorRteIVA1.Value.Value, 0), string.Empty, "0"));
                            }
                            #endregion
                            #region ReteIVA2
                            if (footer.BaseRteIVA2.Value != 0 && footer.PorRteIVA2.Value != 0)
                            {
                                Dictionary<string, string> keysReteIva = new Dictionary<string, string>();
                                keysReteIva.Add("EmpresaGrupoID", this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coIVARetencion, this.Empresa, egCtrl));
                                keysReteIva.Add("RegimenFiscalEmpresaID", _regFiscalEmp.ReferenciaID.Value);
                                keysReteIva.Add("RegimenFiscalTerceroID", _refFiscalTercero.ReferenciaID.Value);
                                keysReteIva.Add("CuentaIVA", _ctaIVA2);
                                this._dal_MasterComplex.DocumentID = AppMasters.coIVARetencion;
                                DTO_coIvaRetencion _reteIVA2 = (DTO_coIvaRetencion)this._dal_MasterComplex.DAL_MasterComplex_GetByID(keysReteIva, true);
                                if (_reteIVA2 != null)
                                {
                                    _ctaReteIVA2 = _reteIVA2.CuentaReteIVA.Value;
                                    listValuesCompFooter.Add(Tuple.Create(_ctaReteIVA2, footer.BaseRteIVA2.Value.Value, Math.Round(footer.ValorRteIVA2.Value.Value * (-1), 0), string.Empty, "0"));
                                    if (footer.RteIVA2AsumidoInd.Value.Value && _reteIVA2 != null)
                                        listValuesCompFooter.Add(Tuple.Create(_ctaReteIVA2, footer.BaseRteIVA2.Value.Value, Math.Round(footer.ValorRteIVA2.Value.Value, 0), string.Empty, "0"));
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #region ReteFuente
                        if (cta.TipoImpuesto == codigoRfte && !listValuesCompFooter.Exists(x => x.Item1 == cta.CuentaID.Value))
                        {
                            if (footer.BaseRteFuente.Value != 0 && footer.PorRteFuente.Value != 0 && footer.ValorRteFuente.Value != 0)
                            {
                                listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseRteFuente.Value.Value, Math.Round(footer.ValorRteFuente.Value.Value * (-1), 0), string.Empty, "0"));
                                if (footer.RteFteAsumidoInd.Value.Value)
                                    listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseRteFuente.Value.Value, Math.Round(footer.ValorRteFuente.Value.Value, 0), string.Empty, "0"));
                            }
                        }
                        #endregion
                        #region ReteICA
                        if (cta.TipoImpuesto == codigoReteICA && !listValuesCompFooter.Exists(x => x.Item1 == cta.CuentaID.Value))
                        {
                            if (footer.BaseRteICA.Value != 0 && footer.PorRteICA.Value != 0)
                            {
                                listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseRteICA.Value.Value, Math.Round(footer.ValorRteICA.Value.Value * (-1), 0), string.Empty, "0"));
                                if (footer.RteICAAsumidoInd.Value.Value)
                                    listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseRteICA.Value.Value, Math.Round(footer.ValorRteICA.Value.Value, 0), string.Empty, "0"));
                            }
                        }
                        #endregion
                        #region ImpConsumo
                        if (cta.TipoImpuesto == codigoImpConsumo && !listValuesCompFooter.Exists(x => x.Item1 == cta.CuentaID.Value))
                        {
                            if (footer.BaseImpConsumo.Value != 0 && footer.PorImpConsumo.Value != 0)
                            {
                                listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseImpConsumo.Value.Value, Math.Round(footer.ValorImpConsumo.Value.Value * (-1), 0), string.Empty, "0"));
                                //if (footer.RteICAAsumidoInd.Value.Value)
                                //    listValuesCompFooter.Add(Tuple.Create(cta.CuentaID.Value, footer.BaseImpConsumo.Value.Value, Math.Round(footer.ValorImpConsumo.Value.Value, 0), string.Empty, "0"));
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Crea registros del Footer

                    DTO_glDocumentoControl _facturaCtrl = _moduloGlobal.glDocumentoControl_GetExternalDoc(AppDocuments.CausarFacturas, footer.TerceroID.Value, footer.Factura.Value);
                    foreach (var valueImp in listValuesCompFooter)
                    {
                        footerCaja = new DTO_ComprobanteFooter();

                        #region Info general del detalle
                        footerCaja.TerceroID.Value = footer.TerceroID.Value;
                        footerCaja.DocumentoCOM.Value = footer.Factura.Value;
                        footerCaja.DatoAdd1.Value = valueImp.Item4;
                        footerCaja.DatoAdd2.Value = valueImp.Item5;
                        footerCaja.DatoAdd3.Value = footer.Item.Value.ToString(); //Consecutivo de la Caja Menor en pantalla
                        #endregion

                        #region Carga la Cuenta para validar concepto saldo
                        if (cacheCuenta.ContainsKey(valueImp.Item1))
                            _planCuenta = cacheCuenta[valueImp.Item1];
                        else
                        {
                            _planCuenta = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, valueImp.Item1, true, false);
                            cacheCuenta.Add(valueImp.Item1, _planCuenta);
                        }
                        #endregion
                        #region Carga el ConceptoSaldo
                        if (cacheConceptoSaldo.ContainsKey(_planCuenta.ConceptoSaldoID.Value))
                            _conceptoSaldo = cacheConceptoSaldo[_planCuenta.ConceptoSaldoID.Value];
                        else
                        {
                            _conceptoSaldo = (DTO_glConceptoSaldo)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glConceptoSaldo, _planCuenta.ConceptoSaldoID.Value, true, false);
                            cacheConceptoSaldo.Add(_planCuenta.ConceptoSaldoID.Value, _conceptoSaldo);
                        }
                        #endregion

                        //Revisa si tiene una factura relacionada
                        if (_facturaCtrl != null && _conceptoSaldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Externo)
                        {                         
                            #region Asigna el detalle igual a la factura
                            footerCaja.CentroCostoID.Value = _facturaCtrl.CentroCostoID.Value;
                            footerCaja.ProyectoID.Value = _facturaCtrl.ProyectoID.Value;
                            footerCaja.TasaCambio.Value = _facturaCtrl.TasaCambioDOCU.Value;
                            footerCaja.PrefijoCOM.Value = _facturaCtrl.PrefijoID.Value;
                            footerCaja.LineaPresupuestoID.Value = _facturaCtrl.LineaPresupuestoID.Value;
                            footerCaja.LugarGeograficoID.Value = _facturaCtrl.LugarGeograficoID.Value;
                            footerCaja.CuentaID.Value = _facturaCtrl.CuentaID.Value;
                            footerCaja.Descriptivo.Value = _facturaCtrl.Descripcion.Value;
                            footerCaja.IdentificadorTR.Value = _facturaCtrl.NumeroDoc.Value; 
                            #endregion
                        }
                        else
                        {
                            #region Valores propios del documento principal
                            footerCaja.CentroCostoID.Value = footer.CentroCostoID.Value;
                            footerCaja.ProyectoID.Value = footer.ProyectoID.Value;
                            footerCaja.TasaCambio.Value = footer.TasaCambioDOCU.Value;
                            footerCaja.PrefijoCOM.Value = _dtoControl.PrefijoID.Value;
                            footerCaja.LineaPresupuestoID.Value = lineaPresupuestal;
                            footerCaja.LugarGeograficoID.Value = footer.LugarGeograficoID.Value;
                            footerCaja.CuentaID.Value = valueImp.Item1;
                            footerCaja.Descriptivo.Value = _dtoControl.Descripcion.Value;
                            footerCaja.vlrBaseML.Value = monedaLocal != _dtoControl.MonedaID.Value ? 0 : valueImp.Item2;
                            #region Carga la Cuenta
                            if (cacheCuenta.ContainsKey(footerCaja.CuentaID.Value))
                                _planCuenta = cacheCuenta[footerCaja.CuentaID.Value];
                            else
                            {
                                _planCuenta = (DTO_coPlanCuenta)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, footerCaja.CuentaID.Value, true, false);
                                cacheCuenta.Add(footerCaja.CuentaID.Value, _planCuenta);
                            }
                            #endregion
                            #region Carga el ConceptoSaldo
                            if (cacheConceptoSaldo.ContainsKey(_planCuenta.ConceptoSaldoID.Value))
                                _conceptoSaldo = cacheConceptoSaldo[_planCuenta.ConceptoSaldoID.Value];
                            else
                            {
                                _conceptoSaldo = (DTO_glConceptoSaldo)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glConceptoSaldo, _planCuenta.ConceptoSaldoID.Value, true, false);
                                cacheConceptoSaldo.Add(_planCuenta.ConceptoSaldoID.Value, _conceptoSaldo);
                            }
                            #endregion
                            footerCaja.IdentificadorTR.Value = this._moduloAplicacion.AsignarIdentificadorTR(footerCaja, _conceptoSaldo);

                            #endregion
                        }

                        #region Carga los valores
                        footerCaja.vlrBaseML.Value = monedaLocal != _dtoControl.MonedaID.Value ? 0 : valueImp.Item2;
                        if (footerCaja.vlrBaseML.Value != 0)
                            footerCaja.vlrBaseME.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : (valueImp.Item2 / _dtoControl.TasaCambioDOCU.Value.Value);
                        else
                            footerCaja.vlrBaseME.Value = 0;

                        if (monedaLocal == _dtoControl.MonedaID.Value)
                        {
                            footerCaja.vlrMdaLoc.Value = Math.Round(valueImp.Item3, 2);
                            footerCaja.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : (valueImp.Item3 / _dtoControl.TasaCambioDOCU.Value.Value);
                            footerCaja.vlrMdaOtr.Value = footerCaja.vlrMdaLoc.Value;
                        }
                        else
                        {
                            footerCaja.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((valueImp.Item3 * _dtoControl.TasaCambioDOCU.Value.Value), 2);
                            footerCaja.vlrMdaExt.Value = Math.Round(valueImp.Item3, 2);
                            footerCaja.vlrMdaOtr.Value = footerCaja.vlrMdaExt.Value;
                        }
                        #endregion

                        footerCaja.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        footerCaja.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(footerCaja);

                        _sumImp += Math.Round(valueImp.Item3, 0);
                    }
                    listValuesCompFooter.Clear();
                    #endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetDetailImpuestos");
                return result;
            }
        }

        #endregion

        #region Caja Menor

        /// <summary>
        /// Aprueba Caja Menor
        /// </summary>
        /// <param name="leg"> Caja Menor de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult CajaMenor_Aprobar(int documentID, ModulesPrefix currentMod, string actividadFlujoID, DTO_LegalizacionAprobacion leg, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl _dtoControl = null;
            DTO_glDocumentoControl ctrlCxP = new DTO_glDocumentoControl();
            DTO_Comprobante _comprobante = null;
            DTO_coComprobante _comp = null;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Legalizacion = (DAL_Legalizacion)this.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Obtiene el documento control asociado
                _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(leg.NumeroDoc.Value.Value);

                //Obtiene el header asociado de la tabla cpLegalizaDocu
                DTO_cpLegalizaDocu header = this._dal_Legalizacion.DAL_LegalizaHeader_Get(_dtoControl.NumeroDoc.Value.Value);

                if (documentID == AppDocuments.CajaMenorAprob)
                {
                    #region Carga las variables
                    DTO_Legalizacion _leg = this.Legalizacion_Get(_dtoControl.NumeroDoc.Value.Value); ;
                    DTO_cpCajaMenor _caja = (DTO_cpCajaMenor)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, _leg.Header.CajaMenorID.Value, true, false);
                    DTO_coDocumento _documento = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, _caja.coDocumentoID.Value, true, false);
                    this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                    string monedaLocal = _moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                    string conceptoCxp = _moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPCajaMenor);
                    string factEquivalente = GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoFacturaEquivalente);
                    decimal _sumTotal = 0;
                    #endregion
                    #region Cambia estado en la tabla complementaria cpLegalizaDocu
                    header.UsuarioAprueba.Value = this.UserId.ToString();
                    header.FechaAprueba.Value = DateTime.Now;
                    //this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Contabilizado, header);
                    #endregion
                    #region Actualiza el footer para la Factura Equivalente
                    this.LegalizaFooter_UpdFactEquiv(_leg.Header.NumeroDoc.Value.Value, factEquivalente);
                    #endregion
                    #region Asigna el flujo para la caja menor
                    result = this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actividadFlujoID, false, _dtoControl.Observacion.Value);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                    #region Actualiza el estado del documento
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, _dtoControl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, leg.Observacion.Value, true);
                    #endregion
                    #region Carga los datos del comprobante
                    _comprobante = new DTO_Comprobante();
                    if (_leg.Header.NumeroDocCXP.Value.HasValue && this._moduloContabilidad.Comprobante_ExistByIdentificadorTR(_dtoControl.PeriodoDoc.Value.Value, _leg.Header.NumeroDocCXP.Value.Value, true))
                    {
                        #region Carga la info del comprobante actual y elimina el preliminar existente
                        ctrlCxP = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.NumeroDocCXP.Value.Value);
                        _comprobante = this._moduloContabilidad.Comprobante_Get(true, true, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.ComprobanteID.Value, 0, null, null);
                        //DTO_ComprobanteFooter contraCxP = comprobanteOld.Footer[comprobanteOld.Footer.Count - 1];

                        this._moduloContabilidad.BorrarAuxiliar_Pre(ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.ComprobanteID.Value, 0);

                        #endregion
                        #region Hace la contabilizacon
                        result = this._moduloContabilidad.ContabilizarComprobante(documentID, _comprobante, ctrlCxP.PeriodoDoc.Value.Value,
                            ModulesPrefix.cp, 0, false);

                        if (result.Result == ResultValue.NOK)
                            return result;

                        #endregion
                    }
                    else
                    {
                        #region Footer para la CajaMenor
                        result = this.GetDetailImpuestos(_leg, _dtoControl, ref _comprobante, ref _sumTotal);
                        if (result.Result == ResultValue.NOK)
                            return result;
                        #endregion                  
                        #region Crea la CxP
                        object obj = this.CuentasXPagar_Generar(_dtoControl, conceptoCxp, _sumTotal, _comprobante.Footer, ModulesPrefix.cp, false);
                        if (obj.GetType() == typeof(DTO_TxResult))
                        {
                            result = (DTO_TxResult)obj;
                            return result;
                        }

                        //Trae la CxP para actualizar los consecutivos
                        ctrlCxP = (DTO_glDocumentoControl)obj;
                        _comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                        #endregion                        
                    }
                    #endregion
                    #region Actualiza el documento generador con el numDoc de la CxP
                    header.NumeroDocCXP.Value = ctrlCxP.NumeroDoc.Value;
                    this._dal_Legalizacion.DAL_LegalizaHeader_Upd(header);
                    #endregion
                }
                else
                {
                    #region Cambia estado en la tabla complementaria cpLegalizaDocu
                    if (documentID == AppDocuments.CajaMenorSolicita)
                    {
                        header.UsuarioSolicita.Value = this.UserId.ToString();
                        header.FechaSolicita.Value = DateTime.Now;
                        this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Solicitar, header);
                    }
                    else if (documentID == AppDocuments.CajaMenorRevisa)
                    {
                        header.UsuarioRevisa.Value = this.UserId.ToString();
                        header.FechaRevisa.Value = DateTime.Now;
                        this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Revisar, header);
                    }
                    else if (documentID == AppDocuments.CajaMenorSupervisa)
                    {
                        header.UsuarioSupervisa.Value = this.UserId.ToString();
                        header.FechaSupervisa.Value = DateTime.Now;
                        this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Supervisar, header);
                    }
                    else if (documentID == AppDocuments.CajaMenorContabiliza)
                    {
                        header.UsuarioContabiliza.Value = this.UserId.ToString();
                        header.FechaContabiliza.Value = DateTime.Now;
                        this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Contabilizar, header);
                    }
                    #endregion
                    #region Asigna el flujo para la caja menor
                    createDoc = false;
                    result = this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actividadFlujoID, false, _dtoControl.Observacion.Value);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion                 
                }
                
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CajaMenor_Aprobar");

                return result;
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                {
                    if (!insideAnotherTx)
                    {
                        base._mySqlConnectionTx.Commit();

                        #region Genera consecutivos
                        base._mySqlConnectionTx = null;
                        //this._moduloAplicacion._mySqlConnectionTx = null;
                        this._moduloGlobal._mySqlConnectionTx = null;
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        if (documentID == AppDocuments.CajaMenorAprob)
                        {
                            if (ctrlCxP.ComprobanteIDNro.Value.Value == 0 || ctrlCxP.ComprobanteIDNro.Value.Value == null)
                            {
                                DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                                ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);
                                this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                            }
                        }
                        #endregion
                    }
                    else
                        throw new Exception("CajaMenor_Aprobar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
            return result;
        }

        /// <summary>
        /// Rechaza  Caja Menor
        /// </summary>
        /// <param name="leg"> Caja Menor de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void CajaMenor_Rechazar(int documentID, string actividadFlujoID, DTO_LegalizacionAprobacion leg, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();


            bool isValid = true;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, leg.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, leg.Observacion.Value, true);
                this.AsignarFlujo(documentID, leg.NumeroDoc.Value.Value, actividadFlujoID, true, leg.Observacion.Value);
                //this.AsignarAlarma(leg.NumeroDoc.Value.Value, actividadFlujoID, false);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Cp_AnticipoNoExiste, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "CajaMenor_Rechazar");
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

        #endregion

        #region Legalizacion de Gastos

        /// <summary>
        /// Aprueba Legalizacion de gastos
        /// </summary>
        /// <param name="legAprob">Legalizacion de gastos de la lista</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult LegalizacionGastos_Aprobar(int documentID, ModulesPrefix currentMod, string actividadFlujoID, DTO_LegalizacionAprobacion legAprob,
            bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl _dtoControl = null;
            DTO_glDocumentoControl ctrlCxP = new DTO_glDocumentoControl();
            DTO_Comprobante _comprobante = null;
            DTO_coComprobante _comp = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Legalizacion = (DAL_Legalizacion)this.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(legAprob.NumeroDoc.Value.Value);

                //Obtiene el header asociado de la tabla cpLegalizaDocu
                DTO_cpLegalizaDocu header = this._dal_Legalizacion.DAL_LegalizaHeader_Get(_dtoControl.NumeroDoc.Value.Value);
                #region Carga las variables
                DTO_Legalizacion _leg = this.Legalizacion_Get(_dtoControl.NumeroDoc.Value.Value);
                string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string conceptoCxp = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPLegalizacionGastos);
                string coDocumentoTarjetas = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DocContableLegTarjetasCredito);
                string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);

                string lineaPresupuestal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                bool isML = _dtoControl.MonedaID.Value == monedaLocal ? true : false;
                DateTime periodo = _dtoControl.PeriodoDoc.Value.Value;
                DateTime fecha = DateTime.Now;
                if (DateTime.Now > periodo)
                {
                    int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    fecha = new DateTime(periodo.Year, periodo.Month, day);
                }
                DTO_coDocumento coDoc = new DTO_coDocumento();

                string factEquivalente = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoFacturaEquivalente);
                bool resultDetail = true;
                DTO_coPlanCuenta _planCuenta;
                decimal _sumTotal = 0;
                #endregion
                if (documentID == AppDocuments.LegalGastosAprob || documentID == AppDocuments.LegalizacionTarjetasAprob)
                {
                    #region Cambia el estado de la tabla complementaria cpLegalizaDocu
                    header.UsuarioContabiliza.Value = this.UserId.ToString();
                    header.FechaContabiliza.Value = DateTime.Now;
                    this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Contabilizar, header);
                    #endregion
                    #region Actualiza el footer para la Factura Equivalente
                    this.LegalizaFooter_UpdFactEquiv(_leg.Header.NumeroDoc.Value.Value, factEquivalente);
                    #endregion
                    #region Asigna el flujo para la legalizacion gastos
                    result = this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actividadFlujoID, false, _dtoControl.Observacion.Value);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                    #region Actualiza el estado del documento
                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, _dtoControl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, legAprob.Observacion.Value, true);
                    #endregion
                    #region Carga los datos del comprobante
                    _comprobante = new DTO_Comprobante();
                    if (_leg.Header.NumeroDocCXP.Value.HasValue && this._moduloContabilidad.Comprobante_ExistByIdentificadorTR(_dtoControl.PeriodoDoc.Value.Value, _leg.Header.NumeroDocCXP.Value.Value, true))
                    {
                        #region Carga la info del comprobante actual y elimina el preliminar existente
                        ctrlCxP = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.NumeroDocCXP.Value.Value);
                        _comprobante = this._moduloContabilidad.Comprobante_Get(true, true, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.ComprobanteID.Value, 0, null, null);
                        this._moduloContabilidad.BorrarAuxiliar_Pre(ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.ComprobanteID.Value, 0);

                        #endregion
                        #region Footer para existencia de Anticipos
                        DTO_ComprobanteFooter _anticipoComprobante;
                        DTO_glDocumentoControl docCtrlAnticipo;
                        #region Anticipo 1
                        if (_leg.Header.IdentificadorAnt1.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt1.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;

                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo1.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 2
                        if (_leg.Header.IdentificadorAnt2.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt2.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo2.Value.Value * (-1);

                        }
                        #endregion
                        #region Anticipo 3
                        if (_leg.Header.IdentificadorAnt3.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt3.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo3.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 4
                        if (_leg.Header.IdentificadorAnt4.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt4.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo4.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 5
                        if (_leg.Header.IdentificadorAnt5.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt5.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo5.Value.Value * (-1);
                        }
                        #endregion
                        #endregion
                        if (documentID == AppDocuments.LegalizacionTarjetasAprob)
                        {
                            #region Contabiliza Legalizacion Tarjetas
                            #region Validaciones

                            //Valida el coDocumento
                            if (string.IsNullOrWhiteSpace(coDocumentoTarjetas))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                                return result;
                            }
                            coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocumentoTarjetas, true, false);
                            if (coDoc == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                                return result;
                            }
                            //Revisa que tenga comprobante
                            if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                                return result;
                            }
                            else
                                _dtoControl.ComprobanteID.Value = coDoc.ComprobanteID.Value;

                            //Valida que el documento asociado tenga cuenta
                            if ((isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value)))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                                return result;
                            }
                            else
                            {
                                //Valida que la cuenta sea de Componente Tercero
                                string cuenta = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuenta, true, false);
                                DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                if (saldo.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                                    return result;
                                }
                            }

                            //Revisa que el modulo se encuentre abierto
                            EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.cp);
                            if (estado != EstadoPeriodo.Abierto)
                            {
                                if (estado == EstadoPeriodo.Cerrado)
                                    result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                                if (estado == EstadoPeriodo.EnCierre)
                                    result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                                result.Result = ResultValue.NOK;
                                return result;
                            }

                            _comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                            #endregion
                            #region Carga el cabezote del comprobante

                            DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                            compHeader.ComprobanteID.Value = _comp.ID.Value;
                            compHeader.ComprobanteNro.Value = 0;
                            compHeader.EmpresaID.Value = this.Empresa.ID.Value;
                            compHeader.Fecha.Value = fecha;
                            compHeader.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                            compHeader.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                            compHeader.MdaTransacc.Value = _dtoControl.MonedaID.Value;
                            compHeader.PeriodoID.Value = periodo;
                            compHeader.TasaCambioBase.Value = 0;
                            compHeader.TasaCambioOtr.Value = 0;

                            _comprobante.Header = compHeader;

                            #endregion
                            #region Carga el footer del comprobante

                            decimal totalML = 0;
                            decimal totalME = 0;

                            // Asigna los valores de la moneda local y extranjera
                            foreach (DTO_ComprobanteFooter item in _comprobante.Footer)
                            {
                                totalML += item.vlrMdaLoc.Value.Value;
                                totalME += item.vlrMdaExt.Value.Value;
                            }
                            _dtoControl.CuentaID.Value = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                            DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(_dtoControl, _dtoControl.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestal, totalML * -1, totalME * -1, false);
                            compFooterTemp.Descriptivo.Value = "Cuenta x Cobrar Empleados";
                            _comprobante.Footer.Add(compFooterTemp);

                            _comprobante.Footer = _comprobante.Footer;

                            #endregion
                            #region Genera el comprobante
                            result = this._moduloContabilidad.ContabilizarComprobante(documentID, _comprobante, periodo, ModulesPrefix.cp, 0, false);
                            #endregion
                            #endregion
                        }
                        else
                            #region Hace la contabilizacon
                            result = this._moduloContabilidad.ContabilizarComprobante(documentID, _comprobante, ctrlCxP.PeriodoDoc.Value.Value,
                                ModulesPrefix.cp, 0, false);

                        if (result.Result == ResultValue.NOK)
                            return result;

                            #endregion
                    }
                    else
                    {
                        #region Cuando no existe la CxP (Comprobante Preliminar)
                        #region Footer para la Legalizacion de Gastos y legalizacion tarjetas
                        result = this.GetDetailImpuestos(_leg, _dtoControl, ref _comprobante, ref _sumTotal);
                        if (result.Result == ResultValue.NOK)
                            return result;
                        #endregion
                        #region Footer para existencia de Anticipos
                        DTO_ComprobanteFooter _anticipoComprobante;
                        DTO_glDocumentoControl docCtrlAnticipo;
                        #region Anticipo 1
                        if (_leg.Header.IdentificadorAnt1.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt1.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;

                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo1.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 2
                        if (_leg.Header.IdentificadorAnt2.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt2.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo2.Value.Value * (-1);

                        }
                        #endregion
                        #region Anticipo 3
                        if (_leg.Header.IdentificadorAnt3.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt3.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo3.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 4
                        if (_leg.Header.IdentificadorAnt4.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt4.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo4.Value.Value * (-1);
                        }
                        #endregion
                        #region Anticipo 5
                        if (_leg.Header.IdentificadorAnt5.Value != null)
                        {
                            docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt5.Value.Value);
                            _anticipoComprobante = new DTO_ComprobanteFooter();
                            _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                            _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                            _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                            _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                            _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                            _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                            _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                            _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                            _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                            _anticipoComprobante.vlrBaseME.Value = 0;
                            _anticipoComprobante.vlrBaseML.Value = 0;
                            if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                            }
                            else
                            {
                                _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                                _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                                _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                            }
                            _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                            _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                            _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                            //Cuenta
                            _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                            _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                            _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                            _comprobante.Footer.Add(_anticipoComprobante);
                            _sumTotal += _leg.Header.ValorAnticipo5.Value.Value * (-1);
                        }
                        #endregion
                        #endregion

                        if (documentID == AppDocuments.LegalizacionTarjetasAprob)
                        {
                            #region Contabiliza Legalizacion Tarjetas
                            #region Validaciones

                            //Valida el coDocumento
                            if (string.IsNullOrWhiteSpace(coDocumentoTarjetas))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                                return result;
                            }
                            coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocumentoTarjetas, true, false);
                            if (coDoc == null)
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                                return result;
                            }
                            //Revisa que tenga comprobante
                            if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                                return result;
                            }
                            else
                                _dtoControl.ComprobanteID.Value = coDoc.ComprobanteID.Value;

                            //Valida que el documento asociado tenga cuenta
                            if ((isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value)))
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                                return result;
                            }
                            else
                            {
                                //Valida que la cuenta sea de Componente Tercero
                                string cuenta = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuenta, true, false);
                                DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                                if (saldo.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                                {
                                    result.Result = ResultValue.NOK;
                                    result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                                    return result;
                                }
                            }

                            //Revisa que el modulo se encuentre abierto
                            EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.cp);
                            if (estado != EstadoPeriodo.Abierto)
                            {
                                if (estado == EstadoPeriodo.Cerrado)
                                    result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                                if (estado == EstadoPeriodo.EnCierre)
                                    result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                                result.Result = ResultValue.NOK;
                                return result;
                            }

                            _comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                            #endregion
                            #region Carga el cabezote del comprobante

                            DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                            compHeader.ComprobanteID.Value = _comp.ID.Value;
                            compHeader.ComprobanteNro.Value = 0;
                            compHeader.EmpresaID.Value = this.Empresa.ID.Value;
                            compHeader.Fecha.Value = fecha;
                            compHeader.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                            compHeader.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                            compHeader.MdaTransacc.Value = _dtoControl.MonedaID.Value;
                            compHeader.PeriodoID.Value = periodo;
                            compHeader.TasaCambioBase.Value = 0;
                            compHeader.TasaCambioOtr.Value = 0;

                            _comprobante.Header = compHeader;

                            #endregion
                            #region Carga el footer del comprobante

                            decimal totalML = 0;
                            decimal totalME = 0;

                            // Asigna los valores de la moneda local y extranjera
                            foreach (DTO_ComprobanteFooter item in _comprobante.Footer)
                            {
                                totalML += item.vlrMdaLoc.Value.Value;
                                totalME += item.vlrMdaExt.Value.Value;
                            }
                            _dtoControl.CuentaID.Value = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                            DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(_dtoControl, _dtoControl.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestal, totalML * -1, totalME * -1, false);
                            compFooterTemp.Descriptivo.Value = "Cuenta x Cobrar Empleados";
                            _comprobante.Footer.Add(compFooterTemp);

                            _comprobante.Footer = _comprobante.Footer;

                            #endregion
                            #region Genera el comprobante
                            result = this._moduloContabilidad.ContabilizarComprobante(documentID, _comprobante, periodo, ModulesPrefix.cp, 0, false);
                            #endregion
                            #endregion
                        }
                        else
                        {
                            #region Crea la CxP y Contabiliza Legalizacion  Gastos
                            object obj = this.CuentasXPagar_Generar(_dtoControl, conceptoCxp, _sumTotal, _comprobante.Footer, ModulesPrefix.cp, false);
                            if (obj.GetType() == typeof(DTO_TxResult))
                            {
                                result = (DTO_TxResult)obj;
                                return result;
                            }

                            //Trae el doc de CxP para actualizar los consecutivos
                            ctrlCxP = (DTO_glDocumentoControl)obj;
                            _comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                            #endregion
                        } 
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region Cambia estado en la tabla complementaria cpLegalizaDocu
                    if (documentID == AppDocuments.LegalGastosContabiliza)
                    {
                        header.UsuarioContabiliza.Value = this.UserId.ToString();
                        header.FechaContabiliza.Value = DateTime.Now;
                        this.cpLegalizaDocu_ChangeDocumentStatus(_dtoControl.NumeroDoc.Value.Value, EstadoInterCajaMenor.Contabilizar, header);
                    }
                    #endregion
                    #region Asigna el flujo para la caja menor
                    createDoc = false;
                    result = this.AsignarFlujo(documentID, _dtoControl.NumeroDoc.Value.Value, actividadFlujoID, false, _dtoControl.Observacion.Value);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "LegalizacionGastos_Aprobar");

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

                        if (documentID == AppDocuments.LegalGastosAprob)
                        {
                            DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);
                            if (ctrlCxP.ComprobanteIDNro.Value.Value == 0 || ctrlCxP.ComprobanteIDNro.Value == null)
                            {
                                ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(comp, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);

                                this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                                this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);
                            }
                        }
                        else if (documentID == AppDocuments.LegalizacionTarjetasAprob)
                        {   
                            DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, _dtoControl.ComprobanteID.Value, true, false);
                            if (_dtoControl.ComprobanteIDNro.Value.Value == 0 || _dtoControl.ComprobanteIDNro.Value == null)
                            {
                                _dtoControl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(_comp, _dtoControl.PrefijoID.Value, _comprobante.Header.PeriodoID.Value.Value, _dtoControl.DocumentoNro.Value.Value);

                                this._moduloGlobal.ActualizaConsecutivos(_dtoControl, false, true, false);
                                this._moduloContabilidad.ActualizaComprobanteNro(_dtoControl.NumeroDoc.Value.Value, _dtoControl.ComprobanteIDNro.Value.Value, false);
                            }                                
                        }
                    }
                    else
                        throw new Exception("LegalizacionGastos_Aprobar - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
            return result;
        }

        /// <summary>
        /// Rechaza Legalizacion de gastos
        /// </summary>
        /// <param name="leg"> Legalizacion de gastos de la lista</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private void LegalizacionGastos_Rechazar(int documentID, string actividadFlujoID, DTO_LegalizacionAprobacion leg, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            bool isValid = true;
            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, leg.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, leg.Observacion.Value, true);
                this.AsignarFlujo(documentID, leg.NumeroDoc.Value.Value, actividadFlujoID, true, leg.Observacion.Value);
            }
            catch (Exception ex)
            {
                isValid = false;
                var exception = new Exception(DictionaryMessages.Cp_AnticipoNoExiste, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "LegalizacionGastos_Rechazar");
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
        /// Crea un dto de reporte para caja menor
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc de glDocumentoControl</param>
        /// <returns></returns>
        internal object DtoLegalizacionGastosReport(int numeroDoc)
        {
            try
            {
                #region Variables
                DTO_cpCargoEspecial _cargoEsp;
                TipoCargo _tipocargo;
                Dictionary<string, DTO_cpCargoEspecial> cacheCargoEsp = new Dictionary<string, DTO_cpCargoEspecial>();
                Dictionary<TipoCargo, Tuple<string, string, string, string>> det = new Dictionary<TipoCargo, Tuple<string, string, string, string>>();
                //this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_Legalizacion leg = this.Legalizacion_Get(numeroDoc);

                DTO_glDocumentoControl legaDoc = leg.DocCtrl;
                DTO_cpLegalizaDocu legaHead = leg.Header;
                List<DTO_cpLegalizaFooter> legaFoot = leg.Footer;

                //DTO_Comprobante legaComp = new DTO_Comprobante();
                EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), legaDoc.Estado.Value.Value.ToString());
                bool isPre = estado == EstadoDocControl.ParaAprobacion || estado == EstadoDocControl.SinAprobar || estado == EstadoDocControl.Radicado ? true : false;
                //DTO_Comprobante comp = this._moduloContabilidad.Comprobante_Get(true, isPre, legaDoc.PeriodoDoc.Value.Value, legaDoc.ComprobanteID.Value, legaDoc.ComprobanteIDNro.Value.Value, null, null);
                //legaComp = comp;
                #endregion

                #region Asignar los datos para el reporte
                #region Header
                DTO_ReportLegalizacionGastos reportLega = new DTO_ReportLegalizacionGastos();
                reportLega.Header.NumeroDoc = legaDoc.NumeroDoc.Value.Value;
                reportLega.Header.Prefijo = legaDoc.PrefijoID.Value;
                reportLega.Header.DocumentoNro = legaDoc.DocumentoNro.Value.Value;
                reportLega.Header.DocumentoDesc = legaDoc.Observacion.Value;
                reportLega.Header.Fecha = legaDoc.FechaDoc.Value.Value;
                reportLega.Header.MonedaID = legaDoc.MonedaID.Value;
                reportLega.Header.TerceroID = legaDoc.TerceroID.Value;
                DTO_coTercero terceroInfo = (DTO_coTercero)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, legaDoc.TerceroID.Value, true, false);
                if (terceroInfo != null)
                reportLega.Header.TerceroDesc = terceroInfo.Descriptivo.Value;
                reportLega.Header.LugarGeograficoID = terceroInfo.LugarGeograficoID.Value;
                DTO_glLugarGeografico lugarInfo = (DTO_glLugarGeografico)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, reportLega.Header.LugarGeograficoID, true, false);
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
                        _cargoEsp = (DTO_cpCargoEspecial)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, footer.CargoEspecialID.Value, true, false);
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

                return reportLega;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoLegalizacionGastosReport");
                return null;
            }
        }

        #endregion

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Adiciona en una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject Legalizacion_Add(int documentID, DTO_Legalizacion leg, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_glDocumentoControl docCtrl = null;
            DTO_Alarma alarma = null;
            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Declara y carga las variables

                decimal porcTotal = 0;
                decimal porcParte = 100 / 3;

                DTO_cpLegalizaDocu header = leg.Header;
                List<DTO_cpLegalizaFooter> footer = leg.Footer;
                docCtrl = leg.DocCtrl;
                #endregion
                #region Guarda el glDocumentoControl
                docCtrl.ComprobanteIDNro.Value = 0;
                docCtrl.DocumentoNro.Value = 0;
                docCtrl.Valor.Value = header.Valor.Value;
                docCtrl.Iva.Value = header.IVA.Value;
                DTO_TxResultDetail resultGLDC = this._moduloGlobal.glDocumentoControl_Add(documentID, docCtrl, true);
                if (resultGLDC.Message != ResultValue.OK.ToString())
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = "NOK";
                    result.Details.Add(resultGLDC);
                    return result;
                }
                else
                    docCtrl.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Carga la info de la legalizacion
                this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                #region Header

                #region Valida la CajaMenor (cpCajaMenor)

                if (!string.IsNullOrWhiteSpace(header.CajaMenorID.Value))
                {
                    DAL_MasterSimple dalMasterSimple = new DAL_MasterSimple(base._mySqlConnection, base._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    dalMasterSimple.DocumentID = AppMasters.cpCajaMenor;
                    DTO_MasterBasic dto = (DTO_MasterBasic)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, header.CajaMenorID.Value, true, false);

                    if (dto == null)
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.Message = DictionaryMessages.Cp_NoCtaCxP;
                        rd.line = 1;

                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = "CajaMenorID";
                        rdF.Message = DictionaryMessages.FkNotFound + "&&" + header.CajaMenorID.Value;
                        rd.DetailsFields.Add(rdF);

                        result.Details.Add(rd);
                        result.Result = ResultValue.NOK;
                        return result;
                    }
                }

                #endregion

                int nivelIntermAprob = GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_NivIntermediosAprobCajaMenor) != "0" ? (int)EstadoInterCajaMenor.Solicitar : 0;
                header.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);
                if (documentID == AppDocuments.CajaMenor)
                    header.Estado.Value = Convert.ToByte(nivelIntermAprob);
                else
                    header.Estado.Value = (byte)EstadoInterCajaMenor.NoAplica;

                this._dal_Legalizacion.DAL_LegalizaHeader_Add(header);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;

                #endregion
                #region Footer
                foreach (var item in footer)
                    item.NumeroDoc.Value = Convert.ToInt32(resultGLDC.Key);

                this._dal_Legalizacion.DAL_LegalizaFooter_Add(footer);

                porcTotal += porcParte;
                batchProgress[tupProgress] = (int)porcTotal;
                #endregion
                #endregion
                #region Guarda en la bitacora
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloAplicacion.aplBitacora_Add(this.Empresa.ID.Value, documentID, (int)FormsActions.Add, DateTime.Now,
                    this.UserId, this.Empresa.ID.Value, leg.DocCtrl.NumeroDoc.Value.ToString(), string.Empty, string.Empty,
                    string.Empty, string.Empty, 0, 0, 0);
                #endregion
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma
                    //Trae la info de la alarma
                    alarma = this.GetFirstMailInfo(docCtrl.NumeroDoc.Value.Value, true);
                    return alarma;
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Legalizacion_Add");
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

                        DTO_coComprobante _comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, docCtrl.ComprobanteID.Value, true, false);
                        docCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, docCtrl.PrefijoID.Value);
                        //docCtrl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(_comp, docCtrl.PrefijoID.Value, docCtrl.PeriodoDoc.Value.Value, docCtrl.DocumentoNro.Value.Value);
                        alarma.Consecutivo = docCtrl.DocumentoNro.Value.ToString();
                        this._moduloGlobal.ActualizaConsecutivos(docCtrl, true, true, false);
                    }
                    else
                        throw new Exception("Legalizacion_Add - Los consecutivos deben ser generados por la transaccion padre");

                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

        }

        /// <summary>
        /// Implementacion Legalizacion
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Asociado</param>
        /// <returns>DTO_LegalizaHeader</returns>
        public DTO_Legalizacion Legalizacion_Get(int numeroDoc)
        {
            try
            {
                DTO_Legalizacion leg = new DTO_Legalizacion();
                DTO_glDocumentoControl docCtrl = new DTO_glDocumentoControl();
                DTO_cpLegalizaDocu header = new DTO_cpLegalizaDocu();
                List<DTO_cpLegalizaFooter> footer = new List<DTO_cpLegalizaFooter>();

                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                docCtrl = _moduloGlobal.glDocumentoControl_GetByID(numeroDoc);
                leg.DocCtrl = docCtrl;

                this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(NewAge.ADO.DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                header = this._dal_Legalizacion.DAL_LegalizaHeader_Get(numeroDoc);
                leg.Header = header;

                footer = this._dal_Legalizacion.DAL_LegalizaFooter_Get(numeroDoc);
                leg.Footer = footer;
                return leg;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Legalizacion_Get");
                return new DTO_Legalizacion();
            }
        }

        /// <summary>
        /// Actualiza una legalizacion
        /// </summary>
        /// <param name="documentID">Documento que envia a aprobacion</param>
        /// <param name="leg">legalizacion</param>
        /// <returns></returns>
        public DTO_TxResult Legalizacion_Update(int documentID, List<DTO_cpLegalizaFooter> leg, DTO_cpLegalizaDocu header, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {

                this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(NewAge.ADO.DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                
                #region Actualiza docControl
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, header.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar,string.Empty, true);
                List<string> act = this._moduloGlobal.glActividadFlujo_GetActividadesByDocumentID(documentID);
                if (act.Count > 0)
                    this.AsignarFlujo(documentID, header.NumeroDoc.Value.Value, act[0], false, string.Empty);
                #endregion
                #region Actualiza el detalle
                DTO_cpLegalizaFooter dtoFooter = leg.Count > 0 ? leg[0] : null;
                if (dtoFooter != null)
                {
                    int NroReg = 1;
                    foreach (var item in leg)
                    {
                        item.NumeroDoc.Value = Convert.ToInt32(dtoFooter.NumeroDoc.Value.Value);
                        item.Item.Value = Convert.ToByte(NroReg);
                        NroReg++;
                    }
                    this._dal_Legalizacion.DAL_LegalizaFooter_Delete(dtoFooter.NumeroDoc.Value.Value);
                    this._dal_Legalizacion.DAL_LegalizaFooter_Add(leg);
                }
                #endregion
                #region Actualiza el cabezote
                this._dal_Legalizacion.DAL_LegalizaHeader_Upd(header);
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Legalizacion_Update");
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
        /// Envia para aprobacion una legalizacion
        /// </summary>
        /// <param name="documentID">documento que envia a aprobacion</param>
        /// <param name="numeroDoc">numeroDoc de la legalizacion</param>
        /// <param name="createDoc">Indica si se debe o no crear el archivo(pdf)</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public DTO_SerializedObject Legalizacion_SendToAprob(int documentID, int numeroDoc, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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

                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                DTO_Legalizacion Leg = this.Legalizacion_Get(numeroDoc);
                if (Leg != null)
                {

                    EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), Leg.DocCtrl.Estado.Value.Value.ToString());
                    if (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_SendToAprobDoc;
                        return result;
                    }

                    this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, Leg.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    //string EstadoIntermedio = GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_NivIntermediosAprobCajaMenor);

                    //if (Convert.ToInt32(EstadoIntermedio) != 0 && documentID == AppDocuments.CajaMenor)
                    //    this.cpLegalizaDocu_ChangeDocumentStatus(numeroDoc, EstadoInterCajaMenor.Solicitado, Leg.Header);
                    //else
                    this.cpLegalizaDocu_ChangeDocumentStatus(numeroDoc, EstadoInterCajaMenor.NoAplica, Leg.Header);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    if (createDoc)
                    {
                        //try
                        //{
                        //    //#region Generar el nuevo archivo
                        //    //if (documentID == AppDocuments.LegalizacionGastos)
                        //    //    this.GenerarArchivo(documentID, Leg.DocCtrl.NumeroDoc.Value.Value, DtoLegalizacionGastosReport(numeroDoc));
                        //    //#endregion

                        //    porcTotal += porcParte;
                        //    batchProgress[tupProgress] = (int)porcTotal;
                        //}
                        //catch (Exception)
                        //{
                        //    result.Result = ResultValue.NOK;
                        //    result.ResultMessage = DictionaryMessages.Err_ReportCreate;
                        //    return result;
                        //}
                    }
                    else
                        batchProgress[tupProgress] = 100;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_SendToAprobCompr;
                }
                if (result.Result == ResultValue.OK)
                {
                    #region Asigna el usuario con la alarma

                    //Trae la info de la alarma
                    DTO_Alarma alarma = this.GetFirstMailInfo(Leg.DocCtrl.NumeroDoc.Value.Value, true);
                    return alarma;

                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CajaMenor_Aprobar");
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
        /// Trae un listado de cajas menores pendientes para aprobar
        /// </summary>
        /// <param name="documentID">documento relacionado a las aprobaciones</param>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_LegalizacionAprobacion> Legalizacion_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            List<DTO_LegalizacionAprobacion> list = null;
            try
            {
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(NewAge.ADO.DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
                string usuarioID = seUsuario.ID.Value;
                list = this._dal_Legalizacion.DAL_Legalizacion_GetPendientesByModulo(mod, actFlujoID, usuarioID);
                DTO_glActividadFlujo act = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actFlujoID, true, false);

                foreach (var item in list)
                {
                    if (act.DocumentoID.Value == AppDocuments.CajaMenorSolicita.ToString() || act.DocumentoID.Value == AppDocuments.CajaMenorRevisa.ToString() ||
                        act.DocumentoID.Value == AppDocuments.CajaMenorSupervisa.ToString() || act.DocumentoID.Value == AppDocuments.CajaMenorContabiliza.ToString() ||
                        act.DocumentoID.Value == AppDocuments.CajaMenorAprob.ToString())
                    {
                        DTO_cpCajaMenor caja = (DTO_cpCajaMenor)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, item.CajaMenorID.Value, true, false);
                        item.Descriptivo.Value = caja.Descriptivo.Value;
                    }
                    else if (act.DocumentoID.Value == AppDocuments.LegalizacionTarjetasAprob.ToString() || act.DocumentoID.Value == AppDocuments.LegalGastosAprob.ToString() || act.DocumentoID.Value == AppDocuments.LegalGastosContabiliza.ToString())
                    {
                        DTO_coTercero tercero = (DTO_coTercero)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, item.TerceroID.Value, true, false);
                        item.NombreTercero.Value = tercero.Descriptivo.Value;
                    }
                    else if (act.DocumentoID.Value == AppDocuments.LegalizacionTarjetasAprob.ToString())
                    {
                        DTO_cpTarjetaCredito tarjeta = (DTO_cpTarjetaCredito)GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpTarjetaCredito, item.TarjetaCreditoID.Value, true, false);
                        item.Descriptivo.Value = tarjeta.Descriptivo.Value;
                    }
                }
                foreach (DTO_LegalizacionAprobacion item in list)
                    item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
                return list;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Legalizacion_GetPendientesByModulo");
                return list;
            }
        }

        /// <summary>
        /// Recibe una lista de cajas menores para aprobar o rechazar
        /// </summary>
        /// <param name="documentID">documento que relaciona la aprobacion</param>
        /// <param name="leg">Cajas menores que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> Legalizacion_AprobarRechazar(int documentID, string actFlujoID, List<DTO_LegalizacionAprobacion> leg, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
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
                decimal porcParte = 100 / 6;
                int i = 0;
                porcPrevio = porcTotal;
                foreach (var item in leg)
                {
                    porcTemp = (porcParte * i) / leg.Count;
                    porcTotal = porcPrevio + porcTemp;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    #region Aprobar o Rechazar

                    #region Caja Menor
                    if (documentID == AppDocuments.CajaMenorSolicita || documentID == AppDocuments.CajaMenorRevisa ||
                        documentID == AppDocuments.CajaMenorSupervisa || documentID == AppDocuments.CajaMenorAprob || documentID == AppDocuments.CajaMenorContabiliza)
                    {
                        if (item.Aprobado.Value.Value)
                        {
                            try
                            {
                                result = CajaMenor_Aprobar(documentID, ModulesPrefix.cp, actFlujoID, item, createDoc, new Dictionary<Tuple<int, int>, int>(), insideAnotherTx);
                            }
                            catch (Exception exAprob)
                            {
                                result.Result = ResultValue.NOK;
                                string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "CajaMenor_Aprobar");
                                rd.Message = DictionaryMessages.Err_Cp_CajaAprobar + "&&" + item.CajaMenorID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                                result.Details.Add(rd);
                            }
                        }
                        else if (item.Rechazado.Value.Value)
                        {
                            try
                            {
                                this.CajaMenor_Rechazar(documentID, actFlujoID, item, insideAnotherTx);
                            }
                            catch (Exception exRech)
                            {
                                result.Result = ResultValue.NOK;
                                string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "CajaMenor_Rechazar");
                                rd.Message = DictionaryMessages.Err_Cp_CajaRechazar + "&&" + item.CajaMenorID.Value.ToString() + "&&" + item.DocumentoNro.Value.ToString() + ". " + errMsg;
                                result.Details.Add(rd);
                            }
                        }
                    }
                    #endregion
                    #region Legalizacion Gastos
                    else
                        if (item.Aprobado.Value.Value)
                        {
                            try
                            {
                                result = LegalizacionGastos_Aprobar(documentID, ModulesPrefix.cp, actFlujoID, item, createDoc, new Dictionary<Tuple<int, int>, int>(), insideAnotherTx);
                            }
                            catch (Exception exAprob)
                            {
                                result.Result = ResultValue.NOK;
                                string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "Legalizacion_Aprobar");
                                rd.Message = DictionaryMessages.Err_Cp_LegalizacionAprobar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.NombreTercero.Value.ToString() + ". " + errMsg;
                                result.Details.Add(rd);
                            }
                        }
                        else if (item.Rechazado.Value.Value)
                        {
                            try
                            {
                                this.LegalizacionGastos_Rechazar(documentID, actFlujoID, item, insideAnotherTx);
                            }
                            catch (Exception exRech)
                            {
                                result.Result = ResultValue.NOK;
                                string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "Legalizacion_Rechazar");
                                rd.Message = DictionaryMessages.Err_Cp_LegalizacionRechazar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.NombreTercero.Value.ToString() + ". " + errMsg;
                                result.Details.Add(rd);
                            }
                        }
                    #endregion

                    if (result.Result == ResultValue.NOK)
                        results.Add(result);
                    else
                    {
                        base._mySqlConnectionTx = null;
                        DTO_Alarma alarma = this.GetFirstMailInfo(item.NumeroDoc.Value.Value, false);
                        results.Add(alarma);
                    }
                    #endregion
                }
                return results;
            }
            catch (Exception ex)
            {
                results.Clear();
                result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Legalizacion_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Guardar el Auxiliar Pre
        /// </summary>
        /// <param name="documentID">Documento de la transaccion</param>
        /// <param name="leg">Legalizacion a agregar con la informacion necesaria</param>
        /// <returns>resultado</returns>
        public DTO_TxResult Legalizacion_ComprobantePreAdd(int documentID, DTO_Legalizacion leg, Dictionary<Tuple<int, int>, int> batchProgress)
        {

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;
            DTO_glDocumentoControl _dtoControl = null;
            DTO_Comprobante _comprobante = null;

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloContabilidad = (ModuloContabilidad)base.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_MasterComplex = (DAL_MasterComplex)base.GetInstance(typeof(DAL_MasterComplex), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_Legalizacion = (DAL_Legalizacion)base.GetInstance(typeof(DAL_Legalizacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //bool exist = leg.Header.NumeroDocCXP.Value.HasValue? this._moduloContabilidad.Comprobante_ExistByIdentificadorTR(leg.DocCtrl.PeriodoDoc.Value.Value, leg.Header.NumeroDocCXP.Value.Value, true) : false;

                _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(leg.DocCtrl.NumeroDoc.Value.Value);

                #region  Carga las variables
                DTO_Legalizacion _leg = this.Legalizacion_Get(leg.DocCtrl.NumeroDoc.Value.Value);
                string monedaLocal = this._moduloAplicacion.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string conceptoCxp = this._moduloAplicacion.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPCajaMenor);
                string lugarGeografico = this._moduloAplicacion.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string conceptoCargo = this._moduloAplicacion.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base._mySqlConnection, base._mySqlConnectionTx, this.loggerConnectionStr);
                DTO_coPlanCuenta _planCuenta;
                decimal _sumTotal = 0;
                #endregion
                if (_dtoControl.DocumentoID.Value == AppDocuments.CajaMenor || _dtoControl.DocumentoID.Value == AppDocuments.LegalizacionGastos)
                {
                    #region Carga los datos del comprobante
                    _comprobante = new DTO_Comprobante();
                    #region Borra AuxiliarPre
                    if (leg.Header.NumeroDocCXP.Value.HasValue && this._moduloContabilidad.Comprobante_ExistByIdentificadorTR(_dtoControl.PeriodoDoc.Value.Value, leg.Header.NumeroDocCXP.Value.Value, true))
                        this._moduloContabilidad.BorrarAuxiliar_Pre(leg.DocCtrl.PeriodoDoc.Value.Value, leg.DocCtrl.ComprobanteID.Value, leg.DocCtrl.ComprobanteIDNro.Value.Value);
                    #endregion
                    #region Footer para la Legalizacion
                    result = this.GetDetailImpuestos(_leg, _dtoControl, ref _comprobante, ref _sumTotal);
                    if (result.Result == ResultValue.NOK)
                        return result;
                    #endregion
                    #region Footer para existencia de Anticipos
                    DTO_ComprobanteFooter _anticipoComprobante;
                    DTO_glDocumentoControl docCtrlAnticipo;
                    #region Anticipo 1
                    if (_leg.Header.IdentificadorAnt1.Value != null)
                    {
                        docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt1.Value.Value);
                        _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                        _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                        _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;
                        if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                        }
                        else
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo1.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo1.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                        }
                        _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                        _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        //Cuenta
                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;

                        _comprobante.Footer.Add(_anticipoComprobante);
                        _sumTotal += _leg.Header.ValorAnticipo1.Value.Value * (-1);
                    }
                    #endregion
                    #region Anticipo 2
                    if (_leg.Header.IdentificadorAnt2.Value != null)
                    {
                        docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt2.Value.Value);
                        _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                        _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                        _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;
                        if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                        }
                        else
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo2.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo2.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                        }
                        _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                        _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        //Cuenta
                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(_anticipoComprobante);
                        _sumTotal += _leg.Header.ValorAnticipo2.Value.Value * (-1);

                    }
                    #endregion
                    #region Anticipo 3
                    if (_leg.Header.IdentificadorAnt3.Value != null)
                    {
                        docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt3.Value.Value);
                        _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                        _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                        _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;
                        if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                        }
                        else
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo3.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo3.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                        }
                        _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                        _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        //Cuenta
                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(_anticipoComprobante);
                        _sumTotal += _leg.Header.ValorAnticipo3.Value.Value * (-1);
                    }
                    #endregion
                    #region Anticipo 4
                    if (_leg.Header.IdentificadorAnt4.Value != null)
                    {
                        docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt4.Value.Value);
                        _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                        _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                        _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;
                        if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                        }
                        else
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo4.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo4.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                        }
                        _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                        _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        //Cuenta
                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(_anticipoComprobante);
                        _sumTotal += _leg.Header.ValorAnticipo4.Value.Value * (-1);
                    }
                    #endregion
                    #region Anticipo 5
                    if (_leg.Header.IdentificadorAnt5.Value != null)
                    {
                        docCtrlAnticipo = this._moduloGlobal.glDocumentoControl_GetByID(_leg.Header.IdentificadorAnt5.Value.Value);
                        _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = docCtrlAnticipo.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = docCtrlAnticipo.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = docCtrlAnticipo.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = docCtrlAnticipo.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = docCtrlAnticipo.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = docCtrlAnticipo.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = docCtrlAnticipo.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = docCtrlAnticipo.LugarGeograficoID.Value;
                        _anticipoComprobante.Descriptivo.Value = docCtrlAnticipo.Observacion.Value;
                        _anticipoComprobante.CuentaID.Value = docCtrlAnticipo.CuentaID.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;
                        if (monedaLocal == docCtrlAnticipo.MonedaID.Value)
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaExt.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) / _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaLoc.Value;
                        }
                        else
                        {
                            _anticipoComprobante.vlrMdaLoc.Value = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((_leg.Header.ValorAnticipo5.Value.Value * (-1) * _dtoControl.TasaCambioDOCU.Value.Value), 2);//
                            _anticipoComprobante.vlrMdaExt.Value = _leg.Header.ValorAnticipo5.Value.Value * (-1);
                            _anticipoComprobante.vlrMdaOtr.Value = _anticipoComprobante.vlrMdaExt.Value;
                        }
                        _anticipoComprobante.IdentificadorTR.Value = docCtrlAnticipo.NumeroDoc.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = lugarGeografico;
                        _anticipoComprobante.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        //Cuenta
                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(_anticipoComprobante);
                        _sumTotal += _leg.Header.ValorAnticipo5.Value.Value * (-1);
                    }
                    #endregion
                    #endregion
                    #endregion
                    #region Crea la CxP
                    object obj = this.CuentasXPagar_Generar(_dtoControl, conceptoCxp, _sumTotal, _comprobante.Footer, ModulesPrefix.cp, true);
                    if (obj.GetType() == typeof(DTO_TxResult))
                    {
                        result = (DTO_TxResult)obj;
                        return result;
                    }
                    else
                    {
                        DTO_glDocumentoControl ctrlCxP = (DTO_glDocumentoControl)obj;
                        leg.Header.NumeroDocCXP.Value = ctrlCxP.NumeroDoc.Value;
                        this._dal_Legalizacion.DAL_LegalizaHeader_Upd(leg.Header);
                    }
                    #endregion
                }
                else
                {
                    #region Variables tarjeta
                    string coDocumentoTarjetas = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DocContableLegTarjetasCredito);
                    string lineaPresupuestal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    bool isML = _dtoControl.MonedaID.Value == monedaLocal ? true : false;
                    DateTime periodo = _dtoControl.PeriodoDoc.Value.Value;
                    DateTime fecha = DateTime.Now;
                    if (DateTime.Now > periodo)
                    {
                        int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                        fecha = new DateTime(periodo.Year, periodo.Month, day);
                    }
                    DTO_coDocumento coDoc = new DTO_coDocumento(); 
                    #endregion
                    #region Contabiliza Legalizacion Tarjetas
                    #region Validaciones

                    //Valida el coDocumento
                    if (string.IsNullOrWhiteSpace(coDocumentoTarjetas))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                        return result;
                    }
                    coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocumentoTarjetas, true, false);
                    if (coDoc == null)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                        return result;
                    }
                    //Revisa que tenga comprobante
                    if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                        return result;
                    }
                    else
                        _dtoControl.ComprobanteID.Value = coDoc.ComprobanteID.Value;

                    //Valida que el documento asociado tenga cuenta
                    if ((isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value)))
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                        return result;
                    }
                    else
                    {
                        //Valida que la cuenta sea de Componente Tercero
                        string cuenta = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuenta, true, false);
                        DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                        if (saldo.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                            return result;
                        }
                    }

                    //Revisa que el modulo se encuentre abierto
                    EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.cp);
                    if (estado != EstadoPeriodo.Abierto)
                    {
                        if (estado == EstadoPeriodo.Cerrado)
                            result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                        if (estado == EstadoPeriodo.EnCierre)
                            result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                        result.Result = ResultValue.NOK;
                        return result;
                    }
                    #endregion
                    #region Carga el cabezote del comprobante

                    DTO_ComprobanteHeader compHeader = new DTO_ComprobanteHeader();
                    compHeader.ComprobanteID.Value = _dtoControl.ComprobanteID.Value;
                    compHeader.ComprobanteNro.Value = 0;
                    compHeader.EmpresaID.Value = this.Empresa.ID.Value;
                    compHeader.Fecha.Value = fecha;
                    compHeader.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                    compHeader.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                    compHeader.MdaTransacc.Value = _dtoControl.MonedaID.Value;
                    compHeader.PeriodoID.Value = periodo;
                    compHeader.TasaCambioBase.Value = 0;
                    compHeader.TasaCambioOtr.Value = 0;

                    _comprobante.Header = compHeader;

                    #endregion
                    #region Carga el footer del comprobante

                    decimal totalML = 0;
                    decimal totalME = 0;

                    // Asigna los valores de la moneda local y extranjera
                    foreach (DTO_ComprobanteFooter item in _comprobante.Footer)
                    {
                        totalML += item.vlrMdaLoc.Value.Value;
                        totalME += item.vlrMdaExt.Value.Value;
                    }
                    _dtoControl.CuentaID.Value = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                    DTO_ComprobanteFooter compFooterTemp = this.CrearComprobanteFooter(_dtoControl, _dtoControl.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestal, totalML * -1, totalME * -1, false);
                    compFooterTemp.Descriptivo.Value = "Cuenta x Cobrar Empleados";
                    _comprobante.Footer.Add(compFooterTemp);

                    _comprobante.Footer = _comprobante.Footer;

                    #endregion
                    #region Genera el comprobante
                    result = this._moduloContabilidad.ComprobantePre_Add(documentID,ModulesPrefix.cp, _comprobante,_dtoControl.AreaFuncionalID.Value,
                                                                         _dtoControl.PrefijoID.Value,false,_dtoControl.NumeroDoc.Value,null,batchProgress,false);
                    #endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "CajaMenor_ComprobantePreAdd");
                return result;
            }
            return result;
        }

        #endregion

        #endregion

        #region Tarjeta Credito

        #region Funciones Privadas

        /// <summary>
        /// Aprueba Tarjeta Credito
        /// </summary>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="porcTotal">Progreso Total</param>
        /// <param name="porcParte">Parte del progreso</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns></returns>
        private DTO_TxResult cpTarjetaDocu_Aprobar(int documentID, string actividadFlujoID, ModulesPrefix currentMod, DTO_AnticipoAprobacion tarjetaPago, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            #region Carga las variables
            this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._moduloContabilidad = (ModuloContabilidad)this.GetInstance(typeof(ModuloContabilidad), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            DTO_Comprobante _comprobante = new DTO_Comprobante();
            DTO_glDocumentoControl ctrlCxP = new DTO_glDocumentoControl();
            DTO_glDocumentoControl _dtoControl = null;
            DTO_cpTarjetaDocu _tarjetaCreditoDocu = null;
            List<DTO_cpTarjetaPagos> listTarjetaPagos = null;
            DTO_coComprobante _coCompConsumo = null;
            DTO_coComprobante _coCompPagoCuota = null;
            DTO_coDocumento coDoc = null;
            DTO_coPlanCuenta _planCuenta = null;
            DTO_coProyecto proyectoID = null;
            DTO_coCentroCosto centroCostoID = null;
            string operacion = string.Empty;

            _dtoControl = this._moduloGlobal.glDocumentoControl_GetByID(tarjetaPago.NumeroDoc.Value.Value);
            _tarjetaCreditoDocu = this.cpTarjetaDocu_GetByEstado(_dtoControl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, out listTarjetaPagos);

            string monedaLocal = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string lineaPresupuestalxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            string lugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            string conceptoCargo = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            string coDocumentoTarjetas = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DocContableLegTarjetasCredito);
            string cuentaxPagarAcreedores = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CuentaDirectaAcreedoresVarios);
            string conceptoCxPTarjetaCredito = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCxPparaTarjetascredito);

            bool isML = _dtoControl.MonedaID.Value == monedaLocal ? true : false;
            DateTime periodo = _dtoControl.PeriodoDoc.Value.Value;
            DateTime fecha = DateTime.Now;
            if (DateTime.Now > periodo)
            {
                int day = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                fecha = new DateTime(periodo.Year, periodo.Month, day);
            }

            #endregion
            try
            {
                #region Actualiza el estado del documento de la tarjeta Pago
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, _dtoControl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, tarjetaPago.Descripcion.Value, true);
                #endregion
                #region Asigna el flujo de la tarjeta Pago
                result = this.AsignarFlujo(documentID, tarjetaPago.NumeroDoc.Value.Value, actividadFlujoID, false, tarjetaPago.Descripcion.Value);
                if (result.Result == ResultValue.NOK)
                    return result;
                #endregion

                #region 1er Comprobante(Consumo)
                #region Validaciones

                //Valida el documento Contable
                if (string.IsNullOrWhiteSpace(coDocumentoTarjetas))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoDocContable;
                    return result;
                }

                coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, coDocumentoTarjetas, true, false);

                //Revisa que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                else
                    _dtoControl.ComprobanteID.Value = coDoc.ComprobanteID.Value;

                //Valida que el documento asociado tenga cuenta
                if ((isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value)))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                    return result;
                }
                else
                {
                    //Valida que la cuenta sea de Componente Tercero
                    string cuenta = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuenta, true, false);
                    DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                    if (saldo.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                        return result;
                    }
                }

                //Revisa que el modulo se encuentre abierto
                EstadoPeriodo estado = this._moduloAplicacion.CheckPeriod(periodo, ModulesPrefix.cp);
                if (estado != EstadoPeriodo.Abierto)
                {
                    if (estado == EstadoPeriodo.Cerrado)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoCerrado;
                    if (estado == EstadoPeriodo.EnCierre)
                        result.ResultMessage = DictionaryMessages.Err_PeriodoEnCierre;

                    result.Result = ResultValue.NOK;
                    return result;
                }
                //Valida que la Cuenta de Acreedores Varios sea de Componente Tercero 
                if (string.IsNullOrWhiteSpace(cuentaxPagarAcreedores))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_AccountNotExist;
                    return result;
                }
                DTO_coPlanCuenta ctaAcreedor = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuentaxPagarAcreedores, true, false);
                DTO_glConceptoSaldo saldoAcreedor = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, ctaAcreedor.ConceptoSaldoID.Value, true, false);
                if (saldoAcreedor.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                    return result;
                }

                _coCompConsumo = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                #endregion
                #region Carga el cabezote del comprobante

                DTO_ComprobanteHeader compHeaderConsumo = new DTO_ComprobanteHeader();
                compHeaderConsumo.ComprobanteID.Value = _coCompConsumo.ID.Value;
                compHeaderConsumo.ComprobanteNro.Value = 0;
                compHeaderConsumo.EmpresaID.Value = this.Empresa.ID.Value;
                compHeaderConsumo.Fecha.Value = fecha;
                compHeaderConsumo.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                compHeaderConsumo.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                compHeaderConsumo.MdaTransacc.Value = _dtoControl.MonedaID.Value;
                compHeaderConsumo.PeriodoID.Value = periodo;
                compHeaderConsumo.TasaCambioBase.Value = 0;
                compHeaderConsumo.TasaCambioOtr.Value = 0;

                _comprobante.Header = compHeaderConsumo;

                #endregion
                #region Carga el footer del comprobante

                DTO_ComprobanteFooter compFooterTemp;
                decimal ValorConsumoLOC = isML ? _tarjetaCreditoDocu.Valor.Value.Value : (_tarjetaCreditoDocu.Valor.Value.Value * _dtoControl.TasaCambioCONT.Value.Value);
                decimal ValorConsumoEXT = isML ? (_tarjetaCreditoDocu.Valor.Value.Value * _dtoControl.TasaCambioCONT.Value.Value) : _tarjetaCreditoDocu.Valor.Value.Value;

                _dtoControl.CuentaID.Value = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                compFooterTemp = this.CrearComprobanteFooter(_dtoControl, _dtoControl.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestalxDef, ValorConsumoLOC, ValorConsumoEXT, false);
                compFooterTemp.Descriptivo.Value = "Cuenta x Cobrar Empleados";
                _comprobante.Footer.Add(compFooterTemp);

                _dtoControl.CuentaID.Value = cuentaxPagarAcreedores;
                compFooterTemp = this.CrearComprobanteFooter(_dtoControl, _dtoControl.TasaCambioCONT.Value, conceptoCargo, lugarGeografico, lineaPresupuestalxDef, ValorConsumoLOC * -1, ValorConsumoEXT * -1, false);
                compFooterTemp.Descriptivo.Value = "Cuenta x Pagar Acreedores Varios";
                _comprobante.Footer.Add(compFooterTemp);

                _comprobante.Footer = _comprobante.Footer;
                #endregion
                #region Genera el comprobante
                result = this._moduloContabilidad.ContabilizarComprobante(documentID, _comprobante, periodo, ModulesPrefix.cp, 0, false);
                if (result.Result == ResultValue.NOK)
                {
                    result.ResultMessage = DictionaryMessages.Err_Co_NoContab;
                    return result;
                }
                #endregion
                #endregion
                #region 2do Comprobante(Pago Cuota)
                _comprobante = new DTO_Comprobante();
                #region Validaciones

                //Valida el concepto CxP
                if (string.IsNullOrWhiteSpace(conceptoCxPTarjetaCredito))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }
                DTO_cpConceptoCXP concCxP = (DTO_cpConceptoCXP)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, conceptoCxPTarjetaCredito, true, false);
                if (concCxP == null)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Cp_NoConcCxP;
                    return result;
                }
                coDoc = (DTO_coDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, concCxP.coDocumentoID.Value, true, false);
                //Revisa que tenga comprobante
                if (string.IsNullOrWhiteSpace(coDoc.ComprobanteID.Value))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_InvalidCompDoc;
                    return result;
                }
                else
                    _dtoControl.ComprobanteID.Value = coDoc.ComprobanteID.Value;

                //Valida que el documento asociado tenga cuenta
                if ((isML && string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value)) || (!isML && string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value)))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta + "&&" + coDoc.ID.Value;
                    return result;
                }
                else
                {
                    //Valida que la cuenta sea de Componente Tercero
                    string cuenta = isML ? coDoc.CuentaLOC.Value : coDoc.CuentaEXT.Value;
                    DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, cuenta, true, false);
                    DTO_glConceptoSaldo saldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
                    if (saldo.coSaldoControl.Value != (byte)SaldoControl.Componente_Tercero)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Cp_CuentaDocInvalid;
                        return result;
                    }
                }
                _coCompPagoCuota = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, coDoc.ComprobanteID.Value, true, false);

                #endregion
                #region Carga la operacion

                proyectoID = (DTO_coProyecto)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, _dtoControl.ProyectoID.Value, true, false);
                if (proyectoID != null && proyectoID.OperacionID != null)
                    operacion = proyectoID.OperacionID.Value;
                else
                {
                    centroCostoID = (DTO_coCentroCosto)GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, _dtoControl.CentroCostoID.Value, true, false);
                    if (centroCostoID != null)
                        operacion = centroCostoID.OperacionID.Value;
                }

                if (string.IsNullOrEmpty(operacion))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_OperacionIsNullorEmpty;
                    return result;
                }
                #endregion
                #region Carga el cabezote del comprobante

                DTO_ComprobanteHeader compHeaderPagoCuota = new DTO_ComprobanteHeader();
                compHeaderPagoCuota.ComprobanteID.Value = _coCompPagoCuota.ID.Value;
                compHeaderPagoCuota.ComprobanteNro.Value = 0;
                compHeaderPagoCuota.EmpresaID.Value = this.Empresa.ID.Value;
                compHeaderPagoCuota.Fecha.Value = fecha;
                compHeaderPagoCuota.NumeroDoc.Value = _dtoControl.NumeroDoc.Value;
                compHeaderPagoCuota.MdaOrigen.Value = isML ? (byte)TipoMoneda_LocExt.Local : (byte)TipoMoneda_LocExt.Foreign;
                compHeaderPagoCuota.MdaTransacc.Value = _dtoControl.MonedaID.Value;
                compHeaderPagoCuota.PeriodoID.Value = periodo;
                compHeaderPagoCuota.TasaCambioBase.Value = 0;
                compHeaderPagoCuota.TasaCambioOtr.Value = 0;

                _comprobante.Header = compHeaderPagoCuota;

                #endregion
                #region Carga el footer del comprobante desde lista de pagos de tarjeta

                decimal vlrMdaLOC = 0;
                decimal vlrMdaEXT = 0;
                decimal valorTotalPagosLocal = 0;
                decimal valorTotalPagosExtr = 0;
                foreach (var pago in listTarjetaPagos)
                {
                    if (pago.Valor.Value != 0)
                    {
                        DTO_ComprobanteFooter _anticipoComprobante = new DTO_ComprobanteFooter();
                        _anticipoComprobante.CentroCostoID.Value = _dtoControl.CentroCostoID.Value;
                        _anticipoComprobante.ProyectoID.Value = _dtoControl.ProyectoID.Value;
                        _anticipoComprobante.TasaCambio.Value = _dtoControl.TasaCambioDOCU.Value;
                        _anticipoComprobante.TerceroID.Value = _dtoControl.TerceroID.Value;
                        _anticipoComprobante.DocumentoCOM.Value = _dtoControl.DocumentoTercero.Value;
                        _anticipoComprobante.PrefijoCOM.Value = _dtoControl.PrefijoID.Value;
                        _anticipoComprobante.LineaPresupuestoID.Value = _dtoControl.LineaPresupuestoID.Value;
                        _anticipoComprobante.LugarGeograficoID.Value = _dtoControl.LugarGeograficoID.Value;

                        DTO_cpCargoEspecial cargoEsp = (DTO_cpCargoEspecial)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, pago.CargoEspecialID.Value, true, false);
                        DTO_CuentaValor ctaPago = this._moduloGlobal.coCargoCosto_GetCuentaByCargoOper(cargoEsp.ConceptoCargoID.Value, operacion, (cargoEsp.LineaPresupuestoID.Value != null ? cargoEsp.LineaPresupuestoID.Value : lineaPresupuestalxDef), pago.Valor.Value.Value);
                        if (ctaPago.GetType() == typeof(List<DTO_TxResult>))
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_Co_DocNoCta;
                            return result;
                        }
                        _anticipoComprobante.CuentaID.Value = ctaPago.CuentaID.Value;
                        _anticipoComprobante.Descriptivo.Value = pago.Descriptivo.Value;
                        _anticipoComprobante.vlrBaseME.Value = 0;
                        _anticipoComprobante.vlrBaseML.Value = 0;

                        if (monedaLocal == _dtoControl.MonedaID.Value)
                        {
                            vlrMdaLOC = pago.Valor.Value.Value;
                            vlrMdaEXT = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((pago.Valor.Value.Value / _dtoControl.TasaCambioDOCU.Value.Value), 2);
                        }
                        else
                        {
                            vlrMdaLOC = _dtoControl.TasaCambioDOCU.Value.Value == 0 ? 0 : Math.Round((pago.Valor.Value.Value * _dtoControl.TasaCambioDOCU.Value.Value), 2); ;
                            vlrMdaEXT = pago.Valor.Value.Value;
                        }

                        _anticipoComprobante.vlrMdaLoc.Value = vlrMdaLOC;
                        _anticipoComprobante.vlrMdaExt.Value = vlrMdaEXT;
                        _anticipoComprobante.vlrMdaOtr.Value = monedaLocal == _dtoControl.MonedaID.Value ? vlrMdaLOC : vlrMdaEXT;

                        _anticipoComprobante.IdentificadorTR.Value = _dtoControl.NumeroDoc.Value;

                        _planCuenta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, _anticipoComprobante.CuentaID.Value, true, false);
                        _anticipoComprobante.ConceptoSaldoID.Value = _planCuenta.ConceptoSaldoID.Value;
                        _anticipoComprobante.ConceptoCargoID.Value = conceptoCargo;
                        _comprobante.Footer.Add(_anticipoComprobante);
                        valorTotalPagosLocal += vlrMdaLOC;
                        valorTotalPagosExtr += vlrMdaEXT;
                    }
                }
                #endregion
                #region Crea la CxP
                object obj = this.CuentasXPagar_Generar(_dtoControl, conceptoCxPTarjetaCredito, Math.Round((monedaLocal == _dtoControl.MonedaID.Value ? valorTotalPagosLocal : valorTotalPagosExtr), 2), _comprobante.Footer, ModulesPrefix.cp, false);
                if (obj.GetType() == typeof(DTO_TxResult))
                {
                    result = (DTO_TxResult)obj;
                    return result;
                }

                //Trae la CxP para actualizar los consecutivos
                ctrlCxP = (DTO_glDocumentoControl)obj;
                _coCompPagoCuota = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, ctrlCxP.ComprobanteID.Value, true, false);

                #endregion
                #region Actualiza el documento complementario con el numDoc de la CxP
                _tarjetaCreditoDocu.NumeroDocCXP.Value = ctrlCxP.NumeroDoc.Value;
                this._dal_TarjetaDocu.DAL_cpTarjetaDocu_Upd(_tarjetaCreditoDocu);
                #endregion
                #endregion

            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpTarjetaDocu_Aprobar");
                throw ex;
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
                        this._moduloContabilidad._mySqlConnectionTx = null;

                        #region Consecutivos de Consumo
                        _dtoControl.ComprobanteIDNro.Value = this.GenerarComprobanteNro(_coCompConsumo, _dtoControl.PrefijoID.Value, _dtoControl.PeriodoDoc.Value.Value, _dtoControl.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(_dtoControl, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(_dtoControl.NumeroDoc.Value.Value, _dtoControl.ComprobanteIDNro.Value.Value, false);
                        #endregion
                        #region Consecutivos Pago Cuota
                        ctrlCxP.ComprobanteIDNro.Value = this.GenerarComprobanteNro(_coCompPagoCuota, ctrlCxP.PrefijoID.Value, ctrlCxP.PeriodoDoc.Value.Value, ctrlCxP.DocumentoNro.Value.Value);
                        this._moduloGlobal.ActualizaConsecutivos(ctrlCxP, false, true, false);
                        this._moduloContabilidad.ActualizaComprobanteNro(ctrlCxP.NumeroDoc.Value.Value, ctrlCxP.ComprobanteIDNro.Value.Value, false);

                        #endregion
                        #endregion
                    }
                    else
                        throw new Exception("cpTarjetaPagos_Aprobar - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }

            return result;
        }

        /// <summary>
        /// Rechaza Tarjeta
        /// </summary>
        /// <param name="tarjetaPago">Tarjeta de la lista</param>
        /// <param name="rd"></param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        private DTO_TxResult cpTarjetaDocu_Rechazar(int documentID, string actividadFlujoID, DTO_AnticipoAprobacion tarjetaPago, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = string.Empty;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(documentID, tarjetaPago.NumeroDoc.Value.Value, EstadoDocControl.SinAprobar, tarjetaPago.Descripcion.Value, true);
                this.AsignarAlarma(tarjetaPago.NumeroDoc.Value.Value, actividadFlujoID, false);

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                var exception = new Exception(DictionaryMessages.Cp_AnticipoNoExiste, ex);
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "cpTarjetaDocu_Rechazar");

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
        #region Funciones Publicas

        /// <summary>
        /// Guarda o actualiza documento de Tarjeta Credito
        /// </summary>
        /// <param name="_dtoCtrl">documento asociado</param>
        /// <param name="_tarjetaDocu">Tarjeta Docu</param>
        /// <param name="userID">usuario</param>
        /// <param name="update">bandera actualizacion</param>
        /// <returns></returns>
        public DTO_SerializedObject cpTarjetaDocu_Guardar(int documentID, DTO_glDocumentoControl _dtoCtrl, DTO_cpTarjetaDocu _tarjetaDocu, List<DTO_cpTarjetaPagos> _listTarjetaPago, bool _update, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            DTO_Alarma alarma = null;

            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            if (!insideAnotherTx)
                base._mySqlConnectionTx = base._mySqlConnection.BeginTransaction();

            try
            {
                this._moduloAplicacion = (ModuloAplicacion)this.GetInstance(typeof(ModuloAplicacion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                decimal porcTotal = 0;
                decimal porcParte = 100 / 2;

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                if (!_update)
                {
                    #region Guarda en glDocumentoControl

                    string defProyecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
                    string defCentroCosto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
                    string defLineaPresupuesto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                    string defLugarGeografico = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                    if (_dtoCtrl.ProyectoID == null)
                        _dtoCtrl.ProyectoID.Value = defProyecto;

                    if (_dtoCtrl.CentroCostoID == null)
                        _dtoCtrl.CentroCostoID.Value = defCentroCosto;

                    if (_dtoCtrl.LineaPresupuestoID == null)
                        _dtoCtrl.LineaPresupuestoID.Value = defLineaPresupuesto;

                    if (_dtoCtrl.LugarGeograficoID == null)
                        _dtoCtrl.LugarGeograficoID.Value = defLugarGeografico;

                    _dtoCtrl.DocumentoNro.Value = 0;
                    _dtoCtrl.Valor.Value = _tarjetaDocu.Valor.Value;
                    _dtoCtrl.Iva.Value = _tarjetaDocu.IVA.Value;

                    rd = this._moduloGlobal.glDocumentoControl_Add(documentID, _dtoCtrl, true);
                    if (rd.Message != ResultValue.OK.ToString())
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = "NOK";
                        result.Details.Add(rd);

                        return result;
                    }
                    else
                        _dtoCtrl.NumeroDoc.Value = Convert.ToInt32(rd.Key);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region  Guarda cpTarjetaDocu
                    _tarjetaDocu.NumeroDoc.Value = _dtoCtrl.NumeroDoc.Value.Value;
                    this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_TarjetaDocu.DAL_cpTarjetaDocu_Add(_tarjetaDocu);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Guarda cpTarjetaPagos
                    foreach (var pago in _listTarjetaPago)
                        pago.NumeroDoc.Value = _dtoCtrl.NumeroDoc.Value.Value;
                    this._dal_TarjetaPagos = (DAL_cpTarjetaPagos)this.GetInstance(typeof(DAL_cpTarjetaPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    foreach (var pago in _listTarjetaPago)
                        this._dal_TarjetaPagos.DAL_cpTarjetaPagos_Add(pago);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                }
                else
                {
                    #region Revisa que no haya sido aprobado, anulado o revertido
                    DTO_glDocumentoControl ctrlTemp = this._moduloGlobal.glDocumentoControl_GetByID(_dtoCtrl.NumeroDoc.Value.Value);
                    EstadoDocControl est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), ctrlTemp.Estado.Value.Value.ToString());
                    if (est == EstadoDocControl.Anulado || est == EstadoDocControl.Aprobado || est == EstadoDocControl.Cerrado || est == EstadoDocControl.Devuelto)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_DocProcessed;
                        return result;
                    }
                    #endregion
                    #region Actualiza en glDocumentoControl

                    this._moduloGlobal = (ModuloGlobal)this.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._moduloGlobal.glDocumentoControl_Update(_dtoCtrl, true, true);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Actualiza cpTarjetaDocu
                    this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    _dal_TarjetaDocu.DAL_cpTarjetaDocu_Upd(_tarjetaDocu);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;

                    #endregion
                    #region Actualiza cpTarjetaPagos

                    this._dal_TarjetaPagos = (DAL_cpTarjetaPagos)this.GetInstance(typeof(DAL_cpTarjetaPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    this._dal_TarjetaPagos.DAL_cpTarjetaPagos_Delete(_dtoCtrl.NumeroDoc.Value.Value);
                    foreach (var pago in _listTarjetaPago)
                        this._dal_TarjetaPagos.DAL_cpTarjetaPagos_Add(pago);

                    porcTotal += porcParte;
                    batchProgress[tupProgress] = (int)porcTotal;
                    #endregion
                }

                if (result.Result == ResultValue.OK)
                {
                    #region Genera el reporte y asigna el usuario con la alarma
                    try
                    {
                        //Trae la info de la alarma
                        alarma = this.GetFirstMailInfo(_dtoCtrl.NumeroDoc.Value.Value, true);
                        alarma.NumeroDoc = _dtoCtrl.NumeroDoc.Value.ToString();
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpTarjetaDocu_Guardar");
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

                        #region Genera consecutivos
                        _dtoCtrl.DocumentoNro.Value = this.GenerarDocumentoNro(documentID, _dtoCtrl.PrefijoID.Value);
                        this._moduloGlobal.ActualizaConsecutivos(_dtoCtrl, true, false, false);
                        alarma.Consecutivo = _dtoCtrl.DocumentoNro.Value.ToString();
                        #endregion
                    }
                    else
                        throw new Exception("CuentasXPagar_Radicar - Los consecutivos deben ser generados por la transaccion padre");
                }
                else if (base._mySqlConnectionTx != null && !insideAnotherTx)
                    base._mySqlConnectionTx.Rollback();
            }
        }

        /// <summary>
        /// Obtiene un objeto DTO_cpTarjetaDocu por numero de documento
        /// </summary>
        /// <param name="numeroDoc">numeroDoc</param>
        /// <returns></returns>
        public DTO_cpTarjetaDocu cpTarjetaDocu_GetByEstado(int numeroDoc, EstadoDocControl estado, out List<DTO_cpTarjetaPagos> lisTarjetaPago)
        {
            this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)base.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_TarjetaPagos = (DAL_cpTarjetaPagos)base.GetInstance(typeof(DAL_cpTarjetaPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            lisTarjetaPago = this._dal_TarjetaPagos.DAL_cpTarjetaPagos_Get(numeroDoc);
            foreach (var cargo in lisTarjetaPago)
            {
                DTO_cpCargoEspecial cargoEsp = (DTO_cpCargoEspecial)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, cargo.CargoEspecialID.Value, true, false);
                cargo.Descriptivo.Value = cargoEsp.Descriptivo.Value;
            }
            lisTarjetaPago.Sort((p, q) => string.Compare(p.CargoEspecialID.Value, q.CargoEspecialID.Value));

            return this._dal_TarjetaDocu.DAL_cpTarjetaDocu_GetByEstado(numeroDoc, estado);
        }

        /// <summary>
        /// Retorna el valor total para una lista de Tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tm">Tipo de moneda sobre el cual estan viendo las tarjetas</param>
        /// <param name="tc">Tasa de cambio en el dia</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna el valor total de las tarjetas</returns>
        public decimal cpTarjetaDocu_GetResumenVal(DateTime periodo, TipoMoneda tm, decimal tc, string terceroID)
        {
            this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_TarjetaDocu.DAL_cpTarjetaDocu_GetResumenVal(periodo, tm, tc, terceroID);
        }

        /// <summary>
        /// Recibe una lista de Tarjetas Docu para aprobar o rechazar
        /// </summary>
        /// <param name="tarjetaDocu">Tarjetas Pago que se deben aprobar o rechazar</param>
        /// <param name="createDoc">Indica si se debe generar un documento(pdf) con la operacion</param>
        /// <param name="batchProgress">Progreso de la operacion</param>
        /// <param name="insideAnotherTx">Indica si se esta ejecutando dentro de otra transaccion</param>
        /// <returns>Retorna el resultado de la operacion</returns>
        public List<DTO_SerializedObject> cpTarjetaDocu_AprobarRechazar(int documentID, string actividadFlujoID, List<DTO_AnticipoAprobacion> tarjetaDocu, bool createDoc, Dictionary<Tuple<int, int>, int> batchProgress, bool insideAnotherTx)
        {
            DTO_TxResult result = new DTO_TxResult();
            List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
            Tuple<int, int> tupProgress = new Tuple<int, int>(this.UserId, documentID);
            batchProgress[tupProgress] = 1;

            try
            {
                decimal porcTotal = 0;
                int i = 0;
                foreach (var item in tarjetaDocu)
                {
                    porcTotal = tarjetaDocu.Count * i / 100;
                    batchProgress[tupProgress] = (int)porcTotal;
                    i++;

                    result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();

                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.Message = "OK";

                    #region Aprobar o Rechazar Pago Tarjetas Credito
                    if (item.Aprobado.Value.Value)
                    {
                        try
                        {
                            result = this.cpTarjetaDocu_Aprobar(documentID, actividadFlujoID, ModulesPrefix.cp, item, createDoc, batchProgress, insideAnotherTx);
                        }
                        catch (Exception exAprob)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exAprob, this.UserId.ToString(), "cpTarjetaDocu_Aprobar");
                            rd.Message = DictionaryMessages.Err_Cp_AnticipoAprobar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.DocumentoTercero.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }
                    else if (item.Rechazado.Value.Value)
                    {
                        try
                        {
                            result = this.cpTarjetaDocu_Rechazar(documentID, actividadFlujoID, item, insideAnotherTx);
                        }
                        catch (Exception exRech)
                        {
                            result.Result = ResultValue.NOK;
                            string errMsg = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exRech, this.UserId.ToString(), "cpTarjetaDocu_Rechazar");
                            rd.Message = DictionaryMessages.Err_Cp_AnticipoRechazar + "&&" + item.TerceroID.Value.ToString() + "&&" + item.DocumentoTercero.Value.ToString() + ". " + errMsg;
                            result.Details.Add(rd);
                        }
                    }

                    #endregion

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
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "cpTarjetaDocu_AprobarRechazar");
                results.Add(result);

                return results;
            }
        }

        /// <summary>
        /// Retorna una lista de tarjetas Docu 
        /// </summary>
        /// <param name="periodo">Periodo de consulta</param>
        /// <param name="tipoMoneda">Tipo de moneda de los cuales hay que traer las tarjetas</param>
        /// <param name="terceroID">Tercero de la CxP</param>
        /// <returns>Retorna una lista de Tarjetas pago</returns>
        public List<DTO_AnticiposResumen> cpTarjetaDocu_GetResumen(DateTime periodo, TipoMoneda tipoMoneda, string terceroID)
        {
            this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_TarjetaDocu.DAL_cpTarjetaDocu_GetResumen(periodo, tipoMoneda, terceroID);
        }

        /// <summary>
        /// Trae un listado de Tarjetas Docu pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns>Retorna un auxiliar</returns>
        public List<DTO_AnticipoAprobacion> cpTarjetaDocu_GetPendientesByModulo(ModulesPrefix mod, string actFlujoID)
        {
            this._moduloGlobal = (ModuloGlobal)base.GetInstance(typeof(ModuloGlobal), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_TarjetaDocu = (DAL_cpTarjetaDocu)this.GetInstance(typeof(DAL_cpTarjetaDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario seUsuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this.UserId);
            string usuarioID = seUsuario.ID.Value;
            List<DTO_AnticipoAprobacion> list = this._dal_TarjetaDocu.DAL_cpTarjetaDocu_GetPendientesByModulo(mod, actFlujoID, usuarioID);

            foreach (DTO_AnticipoAprobacion item in list)
            {
                #region Consulta Pagos o resumen del consumo
                this._dal_TarjetaPagos = (DAL_cpTarjetaPagos)this.GetInstance(typeof(DAL_cpTarjetaPagos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_cpTarjetaPagos> lisTarjetaPago = this._dal_TarjetaPagos.DAL_cpTarjetaPagos_Get(item.NumeroDoc.Value.Value);
                decimal valorTot = 0;
                foreach (var cargo in lisTarjetaPago)
                {
                    DTO_cpCargoEspecial cargoEsp = (DTO_cpCargoEspecial)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCargoEspecial, cargo.CargoEspecialID.Value, true, false);
                    cargo.Descriptivo.Value = cargoEsp.Descriptivo.Value;
                    valorTot += cargo.Valor.Value.Value;
                }

                lisTarjetaPago.Sort((p, q) => string.Compare(p.CargoEspecialID.Value, q.CargoEspecialID.Value));

                item.Valor.Value = valorTot;
                item.Detalle = lisTarjetaPago;
                #endregion

                item.FileUrl = base.GetFileRemotePath(item.NumeroDoc.Value.ToString(), TipoArchivo.Documentos);
            }
            return list;
        }

        #endregion

        #endregion

        #region Reportes

        #region Edades
        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los detalla por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> Report_Cp_PorEdadesDetallado(DateTime fechaIni, string terceroID,string cuentaID, bool isDetallada)
        {
            #region Variables

            DTO_ReportCxPTotales dtoTotalesReturn = new DTO_ReportCxPTotales();
            List<DTO_ReportCxPTotales> dtoTotales = new List<DTO_ReportCxPTotales>();
            this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            decimal totales = 0;
            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.DetallesPorEdades = new List<DTO_ReportCxPPorEdades>();
                dtoTotalesReturn.DetallesPorEdades = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_CuentasPorPagarPorEdades(fechaIni, terceroID,cuentaID, isDetallada);

                List<string> distinct = (from c in dtoTotalesReturn.DetallesPorEdades select c.TerceroID.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    totales = 0;
                    DTO_ReportCxPTotales obj = new DTO_ReportCxPTotales();
                    obj.DetallesPorEdades = new List<DTO_ReportCxPPorEdades>();
                    obj.FechaIni = fechaIni;
                    obj.DetallesPorEdades = dtoTotalesReturn.DetallesPorEdades.Where(x => x.TerceroID.Value == item).ToList();

                    foreach (DTO_ReportCxPPorEdades c in obj.DetallesPorEdades)
                    {
                        totales += (c.No_Vencidas.Value.Value + c.Treinta.Value.Value + c.Sesenta.Value.Value + c.Noventa.Value.Value + c.COchenta.Value.Value + c.MasCOchenta.Value.Value);
                        obj.ValorTotalDeta.Value = totales;
                    }
                    dtoTotales.Add(obj);
                }
                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesDetallado");
                return null;
            }
        }

        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los resume por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> Report_Cp_PorEdadesResumido(DateTime fechaCorte, string terceroID,string cuentaID, bool isDetallada)
        {
            try
            {
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> dtosGeneral = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales deta = new DTO_ReportCxPTotales();
                deta.DetallesPorEdades = new List<DTO_ReportCxPPorEdades>();

                deta.DetallesPorEdades = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_CuentasPorPagarPorEdades(fechaCorte, terceroID,cuentaID, isDetallada);
                dtosGeneral.Add(deta);

                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesResumido");
                return null;
            }
        }
        #endregion

        #region Facturas
        /// <summary>
        /// Funcion que arma el objeto que toma el reporte de facturasXPagar
        /// </summary>
        /// <param name="fecha">Fecha por consultar</param>
        /// <returns>Lista de facturas por pagar por cliente</returns>
        public List<DTO_ReportCxpFacturasXPagarTotales> Report_FacturasXPagar(string Tercero, int Moneda, string Cuenta, DateTime fecha, bool isMultimoneda)
        {
            #region Variables

            DTO_ReportCxpFacturasXPagarTotales dtoTotalesReturn = new DTO_ReportCxpFacturasXPagarTotales();
            List<DTO_ReportCxpFacturasXPagarTotales> dtoTotales = new List<DTO_ReportCxpFacturasXPagarTotales>();
            this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.Detalles = new List<DTO_ReportFacturasXPagar>();

                dtoTotalesReturn.Detalles = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_FacturasXPagar(Tercero, Moneda, Cuenta, fecha, isMultimoneda);

                List<string> distinct =
                    (from c in dtoTotalesReturn.Detalles select c.TerceroID.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    DTO_ReportCxpFacturasXPagarTotales obj = new DTO_ReportCxpFacturasXPagarTotales();
                    obj.Detalles = new List<DTO_ReportFacturasXPagar>();

                    obj.Detalles = dtoTotalesReturn.Detalles.Where(x => x.TerceroID.Value == item).ToList();
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

        /// <summary>
        /// Funcion que trae una lista de comprobantes de acuerdo a un periodo (factuas pagadas)
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="fechaFin">Fecha final del reporte</param>
        /// <param name="filtro">Filtro</param>
        /// <returns>Lista de comprobantes (facturas pagadas)</returns>
        public List<DTO_ReportCxpFacturasPagadasTotales> Report_Cp_FacturasPagadas(DateTime fechaIni, DateTime fechaFin, string filtro)
        {
            #region Variables

            DTO_ReportCxpFacturasPagadasTotales dtoTotalesReturn = new DTO_ReportCxpFacturasPagadasTotales();
            List<DTO_ReportCxpFacturasPagadasTotales> dtoTotales = new List<DTO_ReportCxpFacturasPagadasTotales>();
            this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            #endregion

            try
            {
                //Trae los datos
                dtoTotalesReturn.Detalles = new List<DTO_ReportFacturasPagadas>();
                dtoTotalesReturn.Detalles = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_FacturasPagadas(fechaIni, fechaFin, filtro);

                List<string> distinct = (from c in dtoTotalesReturn.Detalles select c.TerceroId.Value).Distinct().ToList();
                foreach (string item in distinct)
                {
                    DTO_ReportCxpFacturasPagadasTotales obj = new DTO_ReportCxpFacturasPagadasTotales();
                    obj.Detalles = new List<DTO_ReportFacturasPagadas>();
                    obj.FechaIni = fechaIni;
                    obj.FechaFin = fechaFin;
                    obj.Detalles = dtoTotalesReturn.Detalles.Where(x => x.TerceroId.Value == item).ToList();
                    dtoTotales.Add(obj);
                }
                return dtoTotales;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_FacturasPagadas");
                return null;
            }
        }

        /// <summary>
        /// Funcion que carga la lista de la fatura a causar
        /// </summary>
        /// <param name="numDoc">Identificador de la factura a Causar</param>
        /// <returns>Listado de Factura a causar</returns>
        public List<DTO_ReportCxPTotales> Reportes_Cp_CausacionFacturas(int numDoc, bool isAprovada)
        {
            try
            {
                #region Variables

                List<DTO_ReportCxPTotales> result = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales causacion = new DTO_ReportCxPTotales();
                causacion.DetalleCausacionFactura = new List<DTO_ReportCausacionFacturas>();
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables para la consulta de los impuesta por cuenta
                List<string> impuestos = new List<string>();
                string iva = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                string reteIva = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
                string reteFuente = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                string reteIca = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
                impuestos.Add(iva);
                impuestos.Add(reteIva);
                impuestos.Add(reteFuente);
                impuestos.Add(reteIca);

                #endregion

                causacion.DetalleCausacionFactura = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_CausacionFacturas(numDoc, impuestos, isAprovada);
                List<string> distinct = (from c in causacion.DetalleCausacionFactura select c.TerceroID.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportCxPTotales causacionFact = new DTO_ReportCxPTotales();
                    causacionFact.DetalleCausacionFactura = new List<DTO_ReportCausacionFacturas>();

                    causacionFact.DetalleCausacionFactura = causacion.DetalleCausacionFactura.Where(x => x.TerceroID.Value == item).ToList();

                    result.Add(causacionFact);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_Cp_CausacionFacturas");
                return null;
            }
        }

        /// <summary>
        /// Funcion que se encarga de Llenar el DTO para la factura Equivalente
        /// </summary>
        /// <param name="fecha">Fecha de la factura equivalente</param>
        /// <param name="tercero">Tercero a quien se le genera la Factura Equivalente</param>
        /// <param name="facturaEquivalente">Verifica si se desea imprimir la factura Equivalente</param>
        /// <returns>Listado DTO</returns>
        public List<DTO_ReportCxPTotales> Reportes_Cp_FacturaEquivalente(DateTime fecha, string tercero, bool facturaEquivalente)
        {
            try
            {
                List<DTO_ReportCxPTotales> result = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales factura = new DTO_ReportCxPTotales();
                factura.DetalleLibroCompras = new List<DTO_ReportLibroCompras>();
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string iva = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                string nitEmpresa = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                factura.DetalleLibroCompras = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_FacturaEquivalente(fecha, tercero, iva, nitEmpresa, facturaEquivalente);
                List<string> distinct = (from c in factura.DetalleLibroCompras select c.comprobante.Value).Distinct().ToList();

                foreach (var comprobantes in distinct)
                {
                    DTO_ReportCxPTotales comprobante = new DTO_ReportCxPTotales();
                    comprobante.DetalleLibroCompras = new List<DTO_ReportLibroCompras>();

                    comprobante.DetalleLibroCompras = factura.DetalleLibroCompras.Where(x => x.comprobante.Value == comprobantes).ToList();
                    result.Add(comprobante);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_Cp_FacturaEquivalente");
                return null;
            }
        }

        #endregion

        #region Flujos
        /// <summary>
        /// Funcion que trae las cuentas x Pagar de manera resumida en un flujo semanal
        /// </summary>
        /// <param name="fechaCorte">Fecha limite del flujo</param>
        /// <param name="filtro">Tercero Id</param>
        /// <returns>Lista de Cunetas por pagar </returns>
        public List<DTO_ReportCxPFlujoSemanalResumido> Report_Cp_FlujoSemanalResumido(DateTime fechaCorte, string filtro)
        {
            try
            {
                #region Variables
                List<DTO_ReportCxPFlujoSemanalResumido> flujoList = new List<DTO_ReportCxPFlujoSemanalResumido>();
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string year = fechaCorte.Year.ToString();
                string month = fechaCorte.Month.ToString();
                string day = "1";
                string fecha = day + "/" + month + "/" + year + " 21:00 a.m";

                DateTime fechaInicial = fechaCorte;
                DateTime fechaFinal = fechaInicial.AddDays(-1);
                int TotalDiasDelMes = DateTime.DaysInMonth(fechaCorte.Year, fechaCorte.Month);
                #endregion
                //Trae los Datos
                flujoList = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_FlujoSemanalResumido(fechaCorte, filtro);
                if (flujoList.Count != 0)
                {
                    flujoList.FirstOrDefault().FechaConrte.Value = fechaCorte;
                    #region Validacion para los días segun el mes y el dia inicial
                    if (flujoList.Count != 0)
                    {
                        switch (fechaInicial.DayOfWeek.ToString())
                        {
                            case "Saturday":
                                flujoList.FirstOrDefault().Dias1 = "3 - 7.";
                                break;
                            case "Sunday":
                                flujoList.FirstOrDefault().Dias1 = "2 - 6.";
                                break;
                            case "Monday":
                                flujoList.FirstOrDefault().Dias1 = "1 - 5";
                                break;
                            case "Tuesday":
                                flujoList.FirstOrDefault().Dias1 = "1 - 4";
                                break;
                            case "Wednesday":
                                flujoList.FirstOrDefault().Dias1 = "1 - 3";
                                break;
                            case "Thursday":
                                flujoList.FirstOrDefault().Dias1 = "1 - 2";
                                break;
                            case "Friday":
                                flujoList.FirstOrDefault().Dias1 = "1 - 1";
                                break;
                        }
                        switch (flujoList.FirstOrDefault().Dias1)
                        {
                            case "3 - 7.":
                                flujoList.FirstOrDefault().Dias2 = "10 - 15";
                                break;
                            case "2 - 6.":
                                flujoList.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "3 - 7":
                                flujoList.FirstOrDefault().Dias2 = "8 - 12";
                                break;
                            case "2 - 6":
                                flujoList.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "1 - 5":
                                flujoList.FirstOrDefault().Dias2 = "8 - 12";
                                break;
                            case "1 - 4":
                                flujoList.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "1 - 3":
                                flujoList.FirstOrDefault().Dias2 = "6 - 10";
                                break;
                            case "1 - 2":
                                flujoList.FirstOrDefault().Dias2 = "5 - 9";
                                break;
                            case "1 - 1":
                                flujoList.FirstOrDefault().Dias2 = "4 - 8";
                                break;
                        }
                        switch (flujoList.FirstOrDefault().Dias2)
                        {
                            case "10 - 15":
                                flujoList.FirstOrDefault().Dias3 = "18 - 22";
                                break;
                            case "8 - 12":
                                flujoList.FirstOrDefault().Dias3 = "15 - 19";
                                break;
                            case "7 - 11":
                                flujoList.FirstOrDefault().Dias3 = "14 - 18";
                                break;
                            case "1 - 3":
                                flujoList.FirstOrDefault().Dias3 = "13 - 17";
                                break;
                            case "1 - 2":
                                flujoList.FirstOrDefault().Dias3 = "12 - 16";
                                break;
                            case "1 - 1":
                                flujoList.FirstOrDefault().Dias3 = "11 - 15";
                                break;
                        }
                        switch (flujoList.FirstOrDefault().Dias3)
                        {
                            case "18 - 22":
                                flujoList.FirstOrDefault().Dias4 = "25 - 29";
                                break;
                            case "15 - 19":
                                flujoList.FirstOrDefault().Dias4 = "22 - 26";
                                break;
                            case "14 - 18":
                                flujoList.FirstOrDefault().Dias4 = "21 - 25";
                                break;
                            case "13 - 17":
                                flujoList.FirstOrDefault().Dias4 = "20 - 24";
                                break;
                            case "12 - 16":
                                flujoList.FirstOrDefault().Dias4 = "19 - 23";
                                break;
                            case "11 - 15":
                                flujoList.FirstOrDefault().Dias4 = "18 - 22";
                                break;
                        }
                        if (TotalDiasDelMes == 30)
                        {
                            switch (flujoList.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoList.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoList.FirstOrDefault().Dias5 = "29 - 30";
                                    break;
                                case "21 - 25":
                                    flujoList.FirstOrDefault().Dias5 = "28 - 30";
                                    break;
                                case "20 - 24":
                                    flujoList.FirstOrDefault().Dias5 = "27 - 30";
                                    break;
                                case "19 - 23":
                                    flujoList.FirstOrDefault().Dias5 = "26 - 30";
                                    break;
                                case "18 - 22":
                                    flujoList.FirstOrDefault().Dias5 = "25 - 29";
                                    break;
                            }
                        }
                        if (TotalDiasDelMes == 31)
                        {
                            switch (flujoList.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoList.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoList.FirstOrDefault().Dias5 = "29 - 31";
                                    break;
                                case "21 - 25":
                                    flujoList.FirstOrDefault().Dias5 = "28 - 31";
                                    break;
                                case "20 - 24":
                                    flujoList.FirstOrDefault().Dias5 = "27 - 31";
                                    break;
                                case "19 - 23":
                                    flujoList.FirstOrDefault().Dias5 = "26 - 30";
                                    break;
                                case "18 - 22":
                                    flujoList.FirstOrDefault().Dias5 = "25 - 29";
                                    break;
                            }
                        }
                        if (TotalDiasDelMes == 28)
                        {
                            switch (flujoList.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoList.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoList.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "21 - 25":
                                    flujoList.FirstOrDefault().Dias5 = "28 - 28";
                                    break;
                                case "20 - 24":
                                    flujoList.FirstOrDefault().Dias5 = "27 - 28";
                                    break;
                                case "19 - 23":
                                    flujoList.FirstOrDefault().Dias5 = "26 - 28";
                                    break;
                                case "18 - 22":
                                    flujoList.FirstOrDefault().Dias5 = "25 - 28";
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                return flujoList;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_FlujoSemanalResumido");
                return null;
            }
        }

        /// <summary>
        /// Funcion que trae las cuentas x Pagar de manera resumida en un flujo semanal
        /// </summary>
        /// <param name="fechaCorte">Fecha limite del flujo</param>
        /// <param name="filtro">Tercero Id</param>
        /// <returns>Lista de Cunetas por pagar </returns>
        public List<DTO_ReportCxPFuljoSemanalResumidoTotales> Report_Cp_FlujoSemanalDetallado(DateTime fechaCorte, string filtro)
        {
            try
            {
                #region Variables
                List<DTO_ReportCxPFuljoSemanalResumidoTotales> flujoTotalesRerutn = new List<DTO_ReportCxPFuljoSemanalResumidoTotales>();
                DTO_ReportCxPFuljoSemanalResumidoTotales flujoItem = new DTO_ReportCxPFuljoSemanalResumidoTotales();
                flujoItem.Detalles = new List<DTO_ReportCxPFlujoSemanalResumido>();

                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                string year = fechaCorte.Year.ToString();
                string month = fechaCorte.Month.ToString();
                string day = "1";
                string fecha = day + "/" + month + "/" + year + " 21:00 a.m";

                DateTime fechaInicial = fechaCorte;
                DateTime fechaFinal = fechaInicial.AddDays(-1);
                int TotalDiasDelMes = DateTime.DaysInMonth(fechaCorte.Year, fechaCorte.Month);
                #endregion
                //Trae los Datos
                flujoItem.Detalles = this._dal_ReportCuentasXPagar.DAL_Report_Cp_FlujoSemanalDetallado(fechaCorte, filtro);
                if (flujoItem.Detalles.Count != 0)
                {
                    flujoItem.Detalles.FirstOrDefault().FechaConrte.Value = fechaCorte;

                    List<string> distinct = (from c in flujoItem.Detalles select c.TerceroID.Value).Distinct().ToList();
                    foreach (string item in distinct)
                    {
                        DTO_ReportCxPFuljoSemanalResumidoTotales flujoTotales = new DTO_ReportCxPFuljoSemanalResumidoTotales();
                        flujoTotales.Detalles = flujoItem.Detalles.Where(x => x.TerceroID.Value == item).ToList();
                        flujoTotalesRerutn.Add(flujoTotales);
                    }
                    #region Validacion para los días segun el mes y el dia inicial
                    if (flujoTotalesRerutn.Count != 0)
                    {
                        switch (fechaInicial.DayOfWeek.ToString())
                        {
                            case "Saturday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "3 - 7.";
                                break;
                            case "Sunday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "2 - 6.";
                                break;
                            case "Monday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "1 - 5";
                                break;
                            case "Tuesday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "1 - 4";
                                break;
                            case "Wednesday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "1 - 3";
                                break;
                            case "Thursday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "1 - 2";
                                break;
                            case "Friday":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1 = "1 - 1";
                                break;
                        }
                        switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias1)
                        {
                            case "3 - 7.":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "10 - 15";
                                break;
                            case "2 - 6.":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "3 - 7":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "8 - 12";
                                break;
                            case "2 - 6":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "1 - 5":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "8 - 12";
                                break;
                            case "1 - 4":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "7 - 11";
                                break;
                            case "1 - 3":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "6 - 10";
                                break;
                            case "1 - 2":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "5 - 9";
                                break;
                            case "1 - 1":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2 = "4 - 8";
                                break;
                        }
                        switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias2)
                        {
                            case "10 - 15":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "18 - 22";
                                break;
                            case "8 - 12":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "15 - 19";
                                break;
                            case "7 - 11":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "14 - 18";
                                break;
                            case "1 - 3":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "13 - 17";
                                break;
                            case "1 - 2":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "12 - 16";
                                break;
                            case "1 - 1":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3 = "11 - 15";
                                break;
                        }
                        switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias3)
                        {
                            case "18 - 22":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "25 - 29";
                                break;
                            case "15 - 19":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "22 - 26";
                                break;
                            case "14 - 18":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "21 - 25";
                                break;
                            case "13 - 17":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "20 - 24";
                                break;
                            case "12 - 16":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "19 - 23";
                                break;
                            case "11 - 15":
                                flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4 = "18 - 22";
                                break;
                        }
                        if (TotalDiasDelMes == 30)
                        {
                            switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "29 - 30";
                                    break;
                                case "21 - 25":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "28 - 30";
                                    break;
                                case "20 - 24":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "27 - 30";
                                    break;
                                case "19 - 23":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "26 - 30";
                                    break;
                                case "18 - 22":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "25 - 29";
                                    break;
                            }
                        }
                        if (TotalDiasDelMes == 31)
                        {
                            switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "29 - 31";
                                    break;
                                case "21 - 25":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "28 - 31";
                                    break;
                                case "20 - 24":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "27 - 31";
                                    break;
                                case "19 - 23":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "26 - 30";
                                    break;
                                case "18 - 22":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "25 - 29";
                                    break;
                            }
                        }
                        if (TotalDiasDelMes == 28)
                        {
                            switch (flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias4)
                            {
                                case "25 - 29":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "22 - 26":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = " - ";
                                    break;
                                case "21 - 25":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "28 - 28";
                                    break;
                                case "20 - 24":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "27 - 28";
                                    break;
                                case "19 - 23":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "26 - 28";
                                    break;
                                case "18 - 22":
                                    flujoTotalesRerutn.FirstOrDefault().Detalles.FirstOrDefault().Dias5 = "25 - 28";
                                    break;
                            }
                        }
                    }
                    #endregion
                }
                return flujoTotalesRerutn;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_FlujoSemanalResumido");
                return null;
            }
        }

        /// <summary>
        /// Funcion que separa los registros del flujo semanal por tercero
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> ReportesCuentasXPagar_FlujoSemanalDetallado(List<DateTime> Fecha, int Moneda, string Tercero, bool isDetallado)
        {
            try
            {

                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> dtosGeneral = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales deta = new DTO_ReportCxPTotales();
                deta.DetalleFlujoSemanal = new List<DTO_ReportCxPFlujoSemanalDetallado>();

                deta.DetalleFlujoSemanal = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_FlujoSemanalDetallado(Fecha, Moneda, Tercero, isDetallado);
                if (isDetallado)
                {
                    List<string> distinct = (from c in deta.DetalleFlujoSemanal select c.TerceroID.Value).Distinct().ToList();

                    foreach (string item in distinct)
                    {
                        DTO_ReportCxPTotales aux = new DTO_ReportCxPTotales();
                        aux.DetalleFlujoSemanal = new List<DTO_ReportCxPFlujoSemanalDetallado>();

                        aux.DetalleFlujoSemanal = deta.DetalleFlujoSemanal.Where(x => x.TerceroID.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                else
                {
                    dtosGeneral.Add(deta);
                }

                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesResumido");
                return null;
            }
        }
        #endregion

        #region Libro Compras

        /// <summary>
        /// Listado de DTO de las comrpras
        /// </summary>
        /// <param name="fecha">Fecha q se desea ver las compras</param>
        /// <param name="tercero">Tercero especifico q se desea ver</param>
        /// <returns>Listado de DTO</returns>
        public List<DTO_ReportCxPTotales> Reportes_Cp_LibroCompras(DateTime fecha, string tercero, bool facturaEquivalente)
        {
            try
            {
                #region Variables

                List<DTO_ReportCxPTotales> result = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales libroCompras = new DTO_ReportCxPTotales();
                libroCompras.DetalleLibroCompras = new List<DTO_ReportLibroCompras>();
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                //Variables para la consulta de los impuesta por cuenta
                List<string> impuestos = new List<string>();
                string iva = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                string reteIva = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
                string reteFuente = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                string reteIca = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
                impuestos.Add(iva);
                impuestos.Add(reteIva);
                impuestos.Add(reteFuente);
                impuestos.Add(reteIca);

                #endregion

                libroCompras.DetalleLibroCompras = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_LibroCompras(impuestos, fecha, tercero, facturaEquivalente);
                List<string> distinct = (from c in libroCompras.DetalleLibroCompras select c.documentoCOM.Value).Distinct().ToList();

                foreach (var facturas in distinct)
                {
                    List<DTO_ReportLibroCompras> compras = new List<DTO_ReportLibroCompras>();
                    DTO_ReportLibroCompras libro = new DTO_ReportLibroCompras();
                    DTO_ReportCxPTotales libroComp = new DTO_ReportCxPTotales();

                    foreach (var terceroFacturas in libroCompras.DetalleLibroCompras.Where(x => x.documentoCOM.Value == facturas))
                    {

                        if (!compras.Any(x => x.TerceroID.Value == terceroFacturas.TerceroID.Value))
                        {
                            compras.Add(terceroFacturas);
                        }
                        else
                        {
                            libro = compras.Where(x => x.TerceroID.Value == terceroFacturas.TerceroID.Value).FirstOrDefault();

                            libro.vlrBruto.Value += terceroFacturas.vlrBruto.Value;
                            libro.iva.Value += terceroFacturas.iva.Value;
                            libro.reteIva.Value += terceroFacturas.reteIva.Value;
                            libro.reteFuente.Value += terceroFacturas.reteFuente.Value;
                            libro.reteIca.Value += terceroFacturas.reteIca.Value;
                            libro.total.Value += terceroFacturas.total.Value;

                            compras.Remove(libro);
                            compras.Add(libro);
                        }
                    }
                    libroComp.DetalleLibroCompras = compras;
                    result.Add(libroComp);
                }

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_Cp_LibroCompras");
                return null;
            }
        }

        #endregion

        #region Radicaciones
        /// <summary>
        /// Funcion q trate todas las radicaciones
        /// </summary>
        /// <returns>Lista de radicaciones</returns>
        public List<DTO_ReportCxPTotales> Report_Radicaciones(int yearIni, int yearFin, DateTime fechaIni, DateTime fechaFin, string Tercero, string Estado, string Orden)
        {
            try
            {
                List<DTO_ReportCxPTotales> result = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales radicaciones = new DTO_ReportCxPTotales();
                radicaciones.DetalleRadicaciones = new List<DTO_ReportRadicaciones>();
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                radicaciones.DetalleRadicaciones = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_Radicaciones(yearIni, yearFin, fechaIni, fechaFin, Tercero, Estado, Orden);
                result.Add(radicaciones);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetail");
                return null;
            }
        }

        #endregion

        #region Tarjetas
        /// <summary>
        /// Funcion que trate todas las Tarjetas Pago de un numero de documento asociado
        /// </summary>
        /// <returns>Lista de tarjetas</returns>
        public List<DTO_ReportBaseCXP> ReportesCuentasXPagar_TarjetasPago(int NumDocu)
        {
            //Variables

            try
            {
                DTO_ReportBaseCXP tarjetas = new DTO_ReportBaseCXP();
                List<DTO_ReportBaseCXP> tarjetasPagas = new List<DTO_ReportBaseCXP>();
                tarjetas.DetalleTarjetaPago = new List<DTO_ReportTarjetaPago>();

                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                tarjetas.DetalleTarjetaPago = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_TarjetasPago(NumDocu);
                List<string> distinct = (from c in tarjetas.DetalleTarjetaPago select c.NumTarjeta.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportBaseCXP tar = new DTO_ReportBaseCXP();
                    tar.DetalleTarjetaPago = new List<DTO_ReportTarjetaPago>();

                    tar.DetalleTarjetaPago = tarjetas.DetalleTarjetaPago.Where(x => x.NumTarjeta.Value == item).ToList();
                    tarjetasPagas.Add(tar);
                }
                return tarjetasPagas;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetail");
                return null;
            }
        }

        /// <summary>
        /// Funcion que trate todas las Legalizaciones de tarjeta de un numero de documento asociado
        /// </summary>
        /// <returns>Lista de tarjetas</returns>
        public List<DTO_ReportBaseCXP> ReportesCuentasXPagar_LegalizaTarjeta(int NumDocu)
        {
            //Variables

            try
            {
                DTO_ReportBaseCXP tarjetas = new DTO_ReportBaseCXP();
                List<DTO_ReportBaseCXP> LegalizaTarjeta = new List<DTO_ReportBaseCXP>();
                tarjetas.DetalleLegalizaTarjeta = new List<DTO_ReportLegalizacionTarjetas>();

                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                tarjetas.DetalleLegalizaTarjeta = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_LegalizaTarjetas(NumDocu);
                List<string> distinct = (from c in tarjetas.DetalleLegalizaTarjeta select c.TarjetaCredito.Value).Distinct().ToList();

                foreach (var item in distinct)
                {
                    DTO_ReportBaseCXP tar = new DTO_ReportBaseCXP();
                    tar.DetalleLegalizaTarjeta = new List<DTO_ReportLegalizacionTarjetas>();

                    tar.DetalleLegalizaTarjeta = tarjetas.DetalleLegalizaTarjeta.Where(x => x.TarjetaCredito.Value == item).ToList();
                    LegalizaTarjeta.Add(tar);
                }
                return LegalizaTarjeta;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DtoReportNominaDetail");
                return null;
            }
        }

        #endregion

        #region Anticipos

        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los resume por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> ReportesCuentasXPagar_Anticipos(DateTime Fecha, int Moneda, string Tercero, bool isDetallado)
        {
            try
            {
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> dtosGeneral = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales deta = new DTO_ReportCxPTotales();
                deta.DetalleAnticipos = new List<DTO_ReportAnticiposDetallado>();

                deta.DetalleAnticipos = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_Anticipos(Fecha, Moneda, Tercero, isDetallado);
                if (isDetallado)
                {
                    List<string> distinct = (from c in deta.DetalleAnticipos select c.TerceroID.Value).Distinct().ToList();

                    foreach (string item in distinct)
                    {
                        DTO_ReportCxPTotales aux = new DTO_ReportCxPTotales();
                        aux.DetalleAnticipos = new List<DTO_ReportAnticiposDetallado>();

                        aux.DetalleAnticipos = deta.DetalleAnticipos.Where(x => x.TerceroID.Value == item).ToList();
                        dtosGeneral.Add(aux);
                    }
                }
                else
                {
                    dtosGeneral.Add(deta);
                }

                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesResumido");
                return null;
            }
        }

        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los resume por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> ReportesCuentasXPagar_DocumentoAnticipo(int NumeroDoc)
        {
            try
            {
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> dtosGeneral = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales deta = new DTO_ReportCxPTotales();
                deta.DetalleAnticipos = new List<DTO_ReportAnticiposDetallado>();
                deta.DetalleAnticipos = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_DocumentoAnticipo(NumeroDoc);

                dtosGeneral.Add(deta);
                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesResumido");
                return null;
            }
        }

        /// <summary>
        /// Funcion que carga las facturas correspondientes aun periodo y los resume por edades
        /// </summary>
        /// <param name="fechaIni">Fecha inicial del reporte</param>
        /// <param name="filtro">terceroID</param>
        /// <returns>Lista de facturas</returns>
        public List<DTO_ReportCxPTotales> ReportesCuentasXPagar_DocumentoAnticipoViaje(int NumeroDoc)
        {
            try
            {
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                List<DTO_ReportCxPTotales> dtosGeneral = new List<DTO_ReportCxPTotales>();
                DTO_ReportCxPTotales deta = new DTO_ReportCxPTotales();
                deta.DetalleAnticiposViaje = new List<DTO_ReportAnticiposViaje>();
                deta.DetalleAnticiposViaje = this._dal_ReportCuentasXPagar.DAL_ReportesCuentasXPagar_DocumentoAnticipoViaje(NumeroDoc);

                dtosGeneral.Add(deta);
                return dtosGeneral;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Report_Cp_PorEdadesResumido");
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Obtiene un datatable con la info de CxP segun filtros
        /// </summary>
        /// <param name="tipoReporte">Tipo de Reporte a Generar</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="tercero">tercero</param>
        /// <param name="facturaNro">facturaNro</param>
        /// <param name="bancoCuenta">bancoCuenta</param>
        /// <param name="Agrupamiento">Agrupamiento</param>
        /// <param name="Romp">Rompimiento</param>
        /// <returns>Datatable</returns>
        public DataTable Reportes_Cp_CxPToExcel(int documentoID, byte? tipoReporte, DateTime? fechaIni, DateTime? fechaFin, string tercero, string facturaNro,
                                                string cuentaID, string bancoCuentaID, string moneda, object otroFilter, byte? agrup, byte? romp)
        {
            try
            {
                DataTable result;
                this._dal_ReportCuentasXPagar = (DAL_ReportesCuentasXPagar)this.GetInstance(typeof(DAL_ReportesCuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                result = this._dal_ReportCuentasXPagar.DAL_Reportes_Cp_CxPToExcel(documentoID, tipoReporte, fechaIni, fechaFin, tercero, facturaNro, cuentaID, bancoCuentaID,moneda,otroFilter, agrup, romp);

                return result;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Reportes_Cp_CxPToExcel");
                throw ex;
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
        public List<DTO_QueryFacturas> ConsultarFacturas(DateTime periodo, string terceroId,string conceptoCxP, string factNro, int tipoConsul, int? tipoFact)
        {
            try
            {
                #region Variables

                List<DTO_QueryFacturas> facturasList = new List<DTO_QueryFacturas>();
                int mes = 0;
                int año = 0;
                this._dal_tsBancosDocu = (DAL_tsBancosDocu)base.GetInstance(typeof(DAL_tsBancosDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_tsBancosCta = (DAL_tsBancosCuenta)base.GetInstance(typeof(DAL_tsBancosCuenta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_CuentasXPagar = (DAL_CuentasXPagar)base.GetInstance(typeof(DAL_CuentasXPagar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

                #endregion

                //Trae los datos
                año = periodo.Year;
                mes = periodo.Month;
                facturasList = this._dal_CuentasXPagar.Consultar_Facturas(año, mes, terceroId,conceptoCxP, factNro, tipoConsul, tipoFact);
                if (facturasList.Count != 0)
                {
                    foreach (DTO_QueryFacturas item in facturasList)
                    {
                        List<DTO_QueryFacturasDetail> facturaDeta = new List<DTO_QueryFacturasDetail>();
                        item.Detalle = new List<DTO_QueryFacturasDetail>();
                        facturaDeta = this._dal_CuentasXPagar.Consultar_Facturas_Detalle((int)item.NumeroDoc.Value, periodo);
                        foreach (DTO_QueryFacturasDetail item2 in facturaDeta)
                        {
                            if (item2.DocumentoID.Value == AppDocuments.DesembolsoFacturas || item2.DocumentoID.Value == AppDocuments.TransferenciasBancarias)
                            {
                                DTO_tsBancosDocu tsBancosDocu = this._dal_tsBancosDocu.DAL_tsBancosDocu_Get(item2.NumeroDoc.Value.Value);
                                DTO_tsBancosCuenta tsBancosCta = tsBancosDocu != null? (DTO_tsBancosCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, tsBancosDocu.BancoCuentaID.Value, true, false): null;
                                item2.Banco.Value = tsBancosCta.Descriptivo.Value;
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
                return new List<DTO_QueryFacturas>();
            }

        }

        #endregion
    }
}
